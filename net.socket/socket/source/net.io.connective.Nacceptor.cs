//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                          Network socket Classes                           *
//*                                                                           *
//*                                                                           *
//*                                                                           *
//*                                                                           *
//*  This Program is programmed by Cho sanghyun. sangducks@cgcii.co.kr        *
//*  Best for Game Developement and Optimized for Game Developement.          *
//*                                                                           *
//*                (c) 2008 Cho sanghyun. All right reserved.                 *
//*                          http://www.CGCII.co.kr                           *
//*                                                                           *
//*****************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using CGDK;

//----------------------------------------------------------------------------
//
//  class CGDK.net.io.connective.Nacceptor
//
// * Parameters
//   1) "Address"
//   2) "Port"
//   3) "accept_prepare_on_start"
//   4) "accept_must_prepare"
//
//----------------------------------------------------------------------------
namespace CGDK.net.io.connective
{
	public abstract class Nacceptor : 
		Nreferenceable,
		Iacceptor,
        IDisposable
	{
	// constructor)
		public Nacceptor(string _name = null, int _max_allocate = int.MaxValue)
		{
			// 1) 이름과 MaxAllocate 수를 설정한다.
			m_name = _name;
			m_max_allocate = _max_allocate;

			// 2) NSocket 상태를 Closed로 초기화 한다.
			m_socket_accept.socket_state = eSOCKET_STATE.CLOSED;

			// 3) socket객체용 Pool을 생성한다.
			m_factory_async_args = new factory.Cobject<SocketAsyncEventArgs>("AsyncEventArgs", 
			()=>
			{
				SocketAsyncEventArgs async_accept = new SocketAsyncEventArgs();
				async_accept.Completed += new EventHandler<SocketAsyncEventArgs>(this.process_connective_connect);

				return	async_accept;
			});

			// 4) State
			m_component_state = new Cobject_state();

			m_component_state.target				 = this;
			m_component_state.notify_on_initializing = _process_on_initializing;
			m_component_state.notify_on_initialize	 = _process_on_initialize;
			m_component_state.notify_on_destroying	 = _process_on_destroying;
			m_component_state.notify_on_destroy		 = _process_on_destroy;

			m_component_state.notify_on_starting	 = _process_on_starting;
			m_component_state.notify_on_start		 = _process_on_start;
			m_component_state.notify_on_stopping	 = _process_on_stopping;
			m_component_state.notify_on_stop		 = _process_on_stop;
		}
		~Nacceptor()
		{
			this.Dispose(false);
		}

	// defintions) 
		public const int				NO_START_ACCEPT	 = (-1);

	// publics) 
		public string					name
		{
			get
			{
				return m_name;
			}
			set
			{
				m_name = value;
			}
		}

		public eOBJECT_STATE			object_state
		{
			get
			{
				return m_component_state.object_state;
			}
			set
			{
				m_component_state.object_state = value;
			}
		}
		public bool						set_object_state_if(eOBJECT_STATE _value, eOBJECT_STATE _compare)
		{
			return m_component_state.set_object_state_if(_value, _compare);
		}

		public bool						initialize(context _context)
		{
			return m_component_state.initialize(_context);
		}
		public bool						destroy()
		{
			return m_component_state.destroy();
		}
		public bool						start(context _context)
		{
			return m_component_state.start(_context);
		}
		public bool						start(IPEndPoint _remote_endpoint, int _accept_prepare_on_start = 0, int _accept_must_prepare = 0)
		{
			// 1) 전달할 Context를 만든다.
			var	temp = new CGDK.context();
			temp["Address"]				 = _remote_endpoint;
			temp["accept_prepare_on_start"] = _accept_prepare_on_start;
			temp["accept_must_prepare"]	 = _accept_must_prepare;

			// 2) 시작을 요청한다.
			return start(temp);
		}
		public bool						start(int _port, int _accept_prepare_on_start = 0, int _accept_must_prepare = 0)
		{
			// 1) 전달할 Context를 만든다.
			var temp = new CGDK.context();
			temp["Port"]				 = _port;
			temp["accept_prepare_on_start"] = _accept_prepare_on_start;
			temp["accept_must_prepare"]	 = _accept_must_prepare;

			// 2) 시작을 요청한다.
			return start(temp);
		}
		public bool						start(string _address, int _port, int _accept_prepare_on_start = 0, int _accept_must_prepare = 0)
		{
			// 1) 전달할 Context를 만든다.
			var temp = new CGDK.context();
			temp["Address"]				 = _address;
			temp["accept_prepare_on_start"] = _accept_prepare_on_start;
			temp["accept_must_prepare"]	 = _accept_must_prepare;

			// 2) 시작을 요청한다.
			return start(temp);
		}
		public bool						start(string _address, string _port, int _accept_prepare_on_start = 0, int _accept_must_prepare = 0)
		{
			// 1) 전달할 Context를 만든다.
			var temp = new CGDK.context();
			temp["Address"]				 = _address;
			temp["Port"]				 = _port;
			temp["accept_prepare_on_start"] = _accept_prepare_on_start;
			temp["accept_must_prepare"]	 = _accept_must_prepare;

			// 2) 시작을 요청한다.
			return start(temp);
		}
		public bool						stop()
		{
			return m_component_state.stop();
		}
		public bool						attach(Iobject_stateable _child)
		{
			return m_component_state.attach(_child);
		}
		public int						detach(Iobject_stateable _child)
		{
			return m_component_state.detach(_child);
		}
		public IEnumerator				GetEnumerator()
		{
			return m_component_state.GetEnumerator();
		}

		public void						disconnect_all()
		{
			//m_list_connective.De
		}
		public void						closesocket_all()
		{
			// declare) 
			Iconnectable[]	list_temp;

			lock(m_list_connective)
			{
				// check) 
				if(m_list_connective.Count == 0)
				{
					return;
				}

				// 1) 갯수만큼 array를 생성한다.
				list_temp	 = new Iconnectable[m_list_connective.Count];

				// 2) 임시 배열에다 복사한다.
				m_list_connective.CopyTo(list_temp);
			}

			// 2) 모든 socket들을 Close한다.
			for(int i=0; i<list_temp.Length; ++i)
			{
				list_temp[i].closesocket();
			}
		}

		public int						Count
		{
			get {	return	m_list_connective.Count();}
		}
		public bool						enable
		{
			get
			{
				return m_bEnable!=0;
			}
			set
			{
				var	value_new = value ? 1: 0;
				var value_old = Interlocked.Exchange(ref m_bEnable, value_new);

				if(value_old != value_new)
				{
					if(value)
					{
						// Trace) 성공한 Accpet갯수를 출력한다.
						LOG.INFO_LOW(null, "  (Info) Acceptor['" + name + "'] Enabled (hSocket:" + m_socket_accept.socket_handle.Handle + ")");
					}
					else
					{
						// Trace) 성공한 Accpet갯수를 출력한다.
						LOG.INFO_LOW(null, "  (Info) Acceptor['" + name + "'] Disabled (hSocket:" + m_socket_accept.socket_handle.Handle + ")");
					}
				}
			}
		}
		public io.NSocket				accept_socket
		{
			get {	return m_socket_accept;}
		}
		public statistics.Nconnective	statistics
		{
			get { return m_statisticsConnective;}
		}


	// frameworks)
		protected virtual void			on_initializing(context _context)			{}
		protected virtual void			on_initialize(context _context)				{}
		protected virtual void			on_destroying()								{}
		protected virtual void			on_destroy()								{}

		protected virtual void			on_starting(context _context)				{}
		protected virtual void			on_start(context _context)					{}
		protected virtual void			on_stopping()								{}
		protected virtual void			on_stop()									{}

		protected virtual void			on_request_accept()							{}
		protected virtual void			on_accept(Iconnectable _connectable)		{}
		protected virtual void			on_fail_accept(Iconnectable _connectable)	{}

		protected virtual void			on_prepare_accept()							{}
		protected virtual void			on_closesocket()							{}

		protected override void			on_final_release()
		{
			// Trace) 
			Trace.WriteLine("(Info) Acceptor: Acceptor Successfully Released[\""+name+"\"] ("+"Acceptor.on_final_release"+")");

			// 1) 처리...
		}

	// implementations)
		protected void					_process_on_initializing(object _object, CGDK.context _context)
		{
			on_initializing(_context);
		}
		protected void					_process_on_initialize(object _object, CGDK.context _context)
		{
			on_initialize(_context);
		}
		protected void					_process_on_destroying(object _object)
		{
			on_destroying();
		}
		protected void					_process_on_destroy(object _object)
		{
			on_destroy();
		}
		protected virtual void			_process_on_starting(object _object, CGDK.context _context)
		{
			// Trace)
			LOG.PROGRESS(null, "@ <BEGIN> start ACCEPTOR['"+name+"']");

			// 1) Context로 Casting
			var context_now = _context;

			// check)
			if(context_now == null)
				throw	new System.Exception("invalid Parameter (it's not context object)");


			//-----------------------------------------------------------------
			// 1. on_starting 함수 호출
			//-----------------------------------------------------------------
			// 1) on_initializing 함수를 호출한다.
			on_starting(_context);


			//-----------------------------------------------------------------
			// 2. Parameter 읽기
			//-----------------------------------------------------------------
			// 1)  Default값을 설정한다.
			IPEndPoint remote_ep = new IPEndPoint(IPAddress.Any, 0);
			int accept_prepare_on_start = 0;
			int accept_must_prepare = 0;

			// 2) Address 값을 읽는다.
			var	str_parameter = context_now["Address"];

			if(str_parameter.exist)
			{
				if(str_parameter != "")
				{
					// - Address를 저장한다.
					remote_ep = str_parameter;

					// log) Success
					LOG.INFO(null, "  + parameter['Address']: '"+str_parameter.value+"'");
				}
				else
				{
					// log) Failed
					LOG.ERROR(null, "  + parameter['Address']: ERROR ('"+str_parameter.value+"') ");
				}
			}

			// 3) Port 값을 읽는다.
			str_parameter = context_now["Port"];

			if(str_parameter.exist)
			{
				// - Port값을 변환한다.
				int	port = str_parameter;

				if(port!=0)
				{
					// - port를 저장한다.
					remote_ep.Port = port;

					// log) Success
					LOG.INFO(null, "  + parameter['Port']: "+str_parameter.value);
				}
				else
				{
					// log) Failed
					LOG.ERROR(null, "  + parameter['Port']: INVALID PORT ('"+str_parameter.value+"') ");
				}
			}

			// 4) accept_prepare_on_start
			str_parameter = context_now["accept_prepare_on_start"];

			if(str_parameter.exist)
			{
				if(str_parameter.value=="default")
				{
					accept_prepare_on_start	 = 0;

					// Trace)
					LOG.INFO(null, "  + parameter['accept_prepare_on_start']: 'Default'["+str_parameter.value+"]");
				}
				else
				{
					accept_prepare_on_start	 = str_parameter;

					// Trace)
					LOG.INFO(null, "  + parameter['accept_prepare_on_start']: "+str_parameter.value);
				}
			}

			// 5) accept_must_prepare
			str_parameter = context_now["accept_must_prepare"];
			if(str_parameter.exist)
			{
				if( str_parameter.value!="default")
				{
					accept_must_prepare = 0;

					// Trace)
					LOG.INFO(null, "  + parameter['accept_prepare_on_start']: 'default'["+str_parameter.value+"]");
				}
				else
				{
					accept_must_prepare = str_parameter;

					// Trace)
					LOG.INFO(null, "  + parameter['accept_prepare_on_start']: "+str_parameter.value);

				}
			}


			//-----------------------------------------------------------------
			// 3. Parameter들을 계산한다.
			//-----------------------------------------------------------------
			// 1) 특별히 미리 준비할 NSocket 수를 설정하지 않는다면 
			if(accept_prepare_on_start == 0)
			{
				// - Thread 갯수를 계산한다.
				accept_prepare_on_start = Environment.ProcessorCount*16+32;
			}

			// check) accept_prepare_on_start가 혹시 0개이면 끝낸다.(Prepare이 하나도 없으면 Accept가 되지를 않는다.)
			Debug.Assert(accept_prepare_on_start != 0);

			// 2) 반드시 준비해야할 Accept
			if(accept_must_prepare == 0)
			{
				if(accept_prepare_on_start != 0)
				{
					// - 시작시 준비하는 Socketdml 1/4로 설정한다.
					accept_must_prepare	= accept_prepare_on_start / 2;

					// - 최하 1개는 되어야 한다.
					if(accept_must_prepare<1)
					{
						accept_must_prepare = 1;
					}
				}
				else
				{
					// - Thread 갯수를 계산한다.
					accept_must_prepare = Environment.ProcessorCount*8;
				}
			}

			// 3) 값을 설정한다.
			accept_prepare_on_start = Math.Max(accept_prepare_on_start, accept_must_prepare);


			try
			{
				//-----------------------------------------------------------------
				// 4. Listen을 시작한다.
				//-----------------------------------------------------------------
				// 1) port를 확인한다.


				// 2) socket을 준비한다.
				process_prepare_socket();

				// check) NSocket Status를 Listen상태로 변경한다. (NSocket State가 CLOSED 상태여야만 한다.)
				if(m_socket_accept.set_socket_state_if(eSOCKET_STATE.CLOSED, eSOCKET_STATE.LISTEN) !=eSOCKET_STATE.CLOSED)
				{
					return;
				}

				// 3) Bind한다.
				m_socket_accept.request_bind(remote_ep);

				// Trace)
				LOG.INFO(null, "  > binded to " + m_socket_accept.local_end_point.ToString() + " [Requested:"+remote_ep.ToString() + "][hSocket=" + m_socket_accept.socket_handle.Handle + "] ");

				// 4) Listen한다.
				m_socket_accept.request_listen(0);

				// Trace)
				LOG.INFO(null, "  > listen on " + m_socket_accept.local_end_point + " [hSocket:" + m_socket_accept.socket_handle.Handle + "]");

				// check) 만약 accept_prepare_on_start가 -1이면 startAccept를 수행하지 않고 그냥 0를 Return한다.
				if(accept_prepare_on_start == NO_START_ACCEPT)
				{
					// Trace) 
					LOG.INFO(null, "  - accepting is not ready because accept_prepare_on_start is 'NO_START_ACCEPT' [Address=" + m_socket_accept.local_end_point + "][hSocket=" + m_socket_accept.socket_handle.Handle + "]");

					// return) 
					return;
				}


				//-----------------------------------------------------------------
				// 5. Accept를 건다.
				//-----------------------------------------------------------------
				// Trace)
				LOG.INFO(null, "  - accept_prepare_on_start="+accept_prepare_on_start+", accept_must_prepare="+accept_must_prepare);

				// 1) Status를 Enable로 설정한다.
				enable = true;

				// declare) 
				int accept_count = 0;

				// 3) Accept 시도 횟수를 계산한다.
				accept_prepare_on_start	-= m_socket_prepared_to_accept;

				try
				{
					// - accept_prepare_on_start만큼 Accept를 건다.
					for(; accept_prepare_on_start>0; --accept_prepare_on_start, ++accept_count)
					{
						// - Accept를 수행한다.
						request_accept();
					}

					// Trace) 걸어놓은 Accpet갯수를 출력한다.
					LOG.INFO(null, "  > Acceptor started ["+accept_count+" sockets are ready for accepting] ");
				}
				catch(System.Exception)
				{
					// log) 
					LOG.ERROR(null, "@ (Excp) System.Exception Complete[request_accept:\"" + name + "\"(hSocket:" + m_socket_accept.socket_handle.Handle + ")] (CGDK.net.Nacceptor._process_on_starting)");

					// reraise)
					throw;
				}

				// 4) 값을 설정한다.
				m_accept_must_prepare = accept_must_prepare;
			}
			catch(System.Exception)
			{
				// log) 
				LOG.ERROR(null, "@ (Excp) System.Exception Complete[request_accept:\"" + name + "\"(hSocket:" + m_socket_accept.socket_handle.Handle + ")] (CGDK.net.Nacceptor._process_on_starting)");

				// - 닫는다!
				_process_on_stopping(_object);

				// Reriase)
				throw;
			}
		}
		protected virtual void			_process_on_start(object _object, context _context)
		{
			// 1) Hook함수를 호출한다.
			on_start(_context);

			// Trace)
			LOG.PROGRESS(null, "@ <END> start ACCEPTOR['"+name+"']");
		}
		protected virtual void			_process_on_stopping(object _object)
		{
			// Trace)
			LOG.PROGRESS(null, "@ <BEGIN> stop ACCEPTOR['"+name+"']");

			// 1) Hook함수를 호출
			on_stopping();

			// 2) Disable로 상태를 바꾼다.
			enable = false;

			// 3) socket을 닫는다.
			process_closesocket();

			// Trace) 걸어놓은 Accpet갯수를 출력한다.
			LOG.INFO(null, "  < ACCEPTOR["+name+"] Closed");
		}
		protected virtual void			_process_on_stop(object _object)
		{
			// 4) Hook함수를 호출
			on_stop();

			// Trace)
			LOG.PROGRESS(null, "@ <END> stop ACCEPTOR['"+name+"']");
		}
	
		protected bool					process_connective_accept(SocketAsyncEventArgs _args)
		{
			// check) SocketError가 발생했거나 최대 할당 갯수를 초과했을 경우 System.Exception을 던진다.
			if(_args.SocketError!= SocketError.Success || m_nSocketAllocated>m_max_allocate)
			{
				// return) 
				return false;
			}

		
			//-----------------------------------------------------------------
			// 1. Connectable 객체 준비하기.
			//-----------------------------------------------------------------
			// declare) 
			Iconnectable pconnectable = null;

			// check) AcceptSocket null이면 당연히 안된다.
			Debug.Assert(_args.AcceptSocket != null);

			// check) 
			Debug.Assert(_args.AcceptSocket.Handle != null);

			try
			{
				// 1) Port를 Reuse하도록 한다.(중요:Connect의 경우 Pool로 동작할 때 NSocket Address를 Reuse하지 않으면 Error를 발생시킬수 있다.)
				_args.AcceptSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

				// 2) 새로운 Accept를 댄다.
				pconnectable = process_alloc_connectable();

				// Reference Counting) 
				pconnectable.add_ref();

				// check)
				Debug.Assert(pconnectable.reference_count==1);
		
				// 3) socket상태를 SYN으로 바꾼다.
				pconnectable.socket.socket_state = eSOCKET_STATE.SYN;

				// 4) set default socket option
				_args.AcceptSocket.Blocking = false;

				// 5) socket을 설정한다.
				pconnectable.socket.socket_handle = _args.AcceptSocket;
			}
			// Case FAIL) Accept가 제대로 되지 않았을 경우.
			catch(System.Exception e)
			{
				// - On Fail Accept
				try
				{
					on_fail_accept(pconnectable);
				}
				catch(System.Exception e2)
				{
					// Trace) 
					Trace.WriteLine("(Excp) Acceptor: System.Exception Complete [notify_on_fail_accept] (" + "CGDK.net.Nacceptor.process_connective_accept" + ")");
					Trace.WriteLine("       "+e2);
				}

				// Trace) 
				Trace.WriteLine("(Excp) Acceptor: System.Exception Complete [on_accept:" + _args.AcceptSocket + "] ("+ "CGDK.net.Nacceptor.process_connective_accept" + ")");
				Trace.WriteLine("       "+e);

				// return) 
				return false;
			}

			// 주의) 여기까지 통과했으면 Return값은 true로 해줘야 한다.


			//-----------------------------------------------------------------
			// 2. Accept받은 socket의 처리.
			// 
			//    Case SUCCRESS) Accept가 제대로 되었을 경우.
			//    - Accept가 제대로 되었을 때는 Accept처리를 한다.
			//    - Accept 받은 socket의 Option을 설정하고... (UpdateAcceptContext)
			//    - notify_on_accept()함수를 호출하고...
			//    - pconnectable의 process_complete_connect()함수를 호출하여 
			//      소켓별 접속처리를 진행한다.
			//-----------------------------------------------------------------
			try
			{
				// 1) Hook함수를 호출한다.
				on_accept(pconnectable);

				// statistics) 접속성공 횟수
				statistics.statistics_on_success_connect();

				// 2) Connectable의 connective를 this로 한다.
				pconnectable.connective = this;

				// check) 
				Debug.Assert(pconnectable.reference_count==1);

				// 3) Connected List에 추가한다.
				add_connectable(pconnectable);

				// 4) process_complete_connect를 수행한다.
				if(pconnectable.process_complete_connect(_args)==false)
				{
					// - Connectable에서 제거한다.
					remove_connectable(pconnectable);

					// - connective에서 Reset
					pconnectable.connective = null;
				}
			}
			catch(System.Exception e)
			{
				// - On Fail Accept
				try
				{
					on_fail_accept(pconnectable);
				}
				catch(System.Exception e2)
				{
					// Trace) 
					Trace.WriteLine("(Excp) Acceptor: System.Exception Complete [notify_on_fail_accept] (" + "CGDK.net.Nacceptor.process_connective_accept" + ")");
					Trace.WriteLine("       "+e2);
				}

				// Trace) 
				Trace.WriteLine("(Excp) Acceptor: System.Exception Complete [notify_on_accept] (" + "CGDK.net.Nacceptor.process_connective_accept" + ")");
				Trace.WriteLine("       "+e);

				// - Connectable의 socket을 닫는다.
				pconnectable.closesocket();

				// statistics) 접속실패 횟수
				statistics.statistics_on_fail_connect();

				// - 할당된 소켓수를 줄인다.
				--m_nSocketAllocated;
			}

			// Reference Counting) 
			pconnectable.release();

			// return) 
			return true;
		}
		public void						process_connective_connect(object _source, SocketAsyncEventArgs _args)
		{
			//-----------------------------------------------------------------
			// ProcessCompleteAccept()함수는 크게 두 부분으로 나눌수 있다.
			//
			// * 첫째는 Accept받은 socket의 접속처리 부분.
			// * 둘째는 새로운 Accept를 거는 부분
			//
			// 으로 나눌 수 있다.
			// 하나의 처리가 다른 하나의 처리에 영향을 미치지 않고 따로 Error나 System.Exception 
			// 처리를 수행한다.
			//-----------------------------------------------------------------

			//-----------------------------------------------------------------
			// 1. Accept완료를 했으므로 걸어 놓은 수를 줄인다.
			//-----------------------------------------------------------------
			// 1) Accept를 받았으므로 준비중인 Accept수를 하나 줄인다.
			var count_prepare_to_accept	 = Interlocked.Decrement(ref m_socket_prepared_to_accept);


			//-----------------------------------------------------------------
			// 2. 현재 Acceptor가 Disable 혹은 소켓 최대 한도 초과 시
			//-----------------------------------------------------------------
			// check) Accept가 Disable된 상태나 최대 할당 수보다 많은 경우라면 끝낸다.
			if(enable == false)
			{
				// statistics)
				statistics.statistics_on_fail_connect();

				// - Connectable의 socket을 닫는다.
				if(_args.AcceptSocket != null)
				{
					_args.AcceptSocket.Close();
				}

				// - 할당된 소켓수를 증가시킨다.
				--m_nSocketAllocated;

				// - 할당해제.
				m_factory_async_args.free(_args);

				// - Signal
				if(count_prepare_to_accept==0)
				{
					m_eventClose.Set();
				}

				// referece counting)
				release();

				// return) Return한다.
				return;
			}

			// 1) Accept처리를 한다.
			var result = process_connective_accept(_args);

			// check) 
			if(result == false)
			{
				// - socket을 Close한다.
				if(_args.AcceptSocket != null)
				{
					_args.AcceptSocket.Close();
				}

				// statistics) 접속실패 횟수
				statistics.statistics_on_fail_connect();

				// 5) 할당된 소켓수를 줄인다.
				--m_nSocketAllocated;
			}

			// 2) AcceptSocket 바로 Clear해버린다.
			_args.AcceptSocket = null;


			//-----------------------------------------------------------------
			// 4. 새로운 Accept를 걸기.
			//-----------------------------------------------------------------
			try
			{
				// 1) Prepare할 갯수를 얻는다. (최하값이 0이 되도록 한다.)
				int	iRequest = (m_accept_must_prepare>m_socket_prepared_to_accept) ? m_accept_must_prepare-m_socket_prepared_to_accept : 0;

				// 2) Prepare할 갯수만큼 Accept를 수행한다.
				for(; iRequest > 0; --iRequest)
				{
					request_accept();	// (*) System.Exception이 발생할 수 있다.
				}

				// check) 준비한 Accept NSocket 수가 0보다 작을 수는 없음.
				Debug.Assert(m_socket_prepared_to_accept>=0);
			}
			catch(System.Exception e)
			{
				// Trace) 
				Trace.WriteLine("(Excp) Acceptor: System.Exception Complete [request_accept:\"" + name+"\"] ("+ "CGDK.net.Nacceptor.process_connective_connect" + ")");
				Trace.WriteLine("       "+e);
			}

			// statistics) Overlapped 걸린 수를 줄인다.
			statistics.statistics_on_decrese_async();

			// 3) 할당해제.
			m_factory_async_args.free(_args);

			// referece counting)
			release();
		}
		public void						process_connective_disconnect(Iconnectable _pconnectable)
		{
			// 1) Connectable Manager에서 제거한다. 
			remove_connectable(_pconnectable);

			// 2) Connectable의 connective를 this로 한다.
			_pconnectable.connective = null;

			// 4) Allocated된 socket수를 줄인다.
			--m_nSocketAllocated;

			// statistics)
			statistics.statistics_on_error_disconnect(_pconnectable.statistics.is_error_disconnected());

			// statistics) 
			statistics.statistics_on_disconnect();

			// check) 만약 Prepare된 숫자가 모자랄 경우 추가 Accept를 건다.
			if(enable && m_accept_must_prepare>m_socket_prepared_to_accept)
			{
				// - Exhaust될 경우 추가 Accept를 수행한다.
				try
				{
					request_accept();	// (*) Exception이 발생할 수 있다.
				}
				catch(System.Exception e)
				{
					// Trace)
					Trace.WriteLine("(Err ) Acceptor: System.Exception Complete [request_accept:\"" + name+"\" [Handle] ("+"Acceptor.process_connective_disconnect"+")");
					Trace.WriteLine("       "+e);
				}
			}
		}
		protected void					process_prepare_socket()
		{
			// 1) socket을 새로 만든다.
			var result = m_socket_accept.create_socket_handle(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			// check) NSocket 생성하지 못했으면 던진다~
			if(result!=true)
			{
				throw new System.Exception();
			}

			// 2) set default parameters
			m_socket_accept.socket_handle.Blocking = false;

			// 3) socket을 Closed로 한다.
			m_socket_accept.socket_state = eSOCKET_STATE.CLOSED;

			try
			{
				// Statistics1)
				statistics.reset();

				// 4) call 'on_prepare_accept'
				on_prepare_accept();
			}
			catch(System.Exception e)
			{
				// Trace)
				Trace.WriteLine("(Err ) Acceptor: Fail on OnPrepareSocket [\""+name+"\" ("+"Acceptor.process_prepare_socket"+")");
				Trace.WriteLine("       "+e);

				// - socket을 닫는다.
				m_socket_accept.close_socket_handle();

				// reraise)
				throw;
			}

			// referece counting)
			add_ref();
		}
		protected bool					process_closesocket(ulong _disconnect_reason = DISCONNECT_REASON.NONE)
		{
			// 1) NSocket Status를 Disconnected로...
			eSOCKET_STATE pre_socket_status = m_socket_accept.exchange_socket_state(eSOCKET_STATE.CLOSED);

			// check) 이전 Status도 Close상태라면 그냥 함수를 종료한다.
			if(pre_socket_status == eSOCKET_STATE.CLOSED)
			{
				return false;
			}

			// Trace) 
			Trace.WriteLine("(Info) Acceptor: Closing Acceptor. Waiting for completion of "+m_socket_prepared_to_accept+" asychronous accepts [\""+name+"\": "+m_socket_accept.local_end_point+"] ("+"Acceptor.process_closesocket"+")");

			// 2) HOOK함수 호출
			on_closesocket();

			// 3) socket을 닫는다.
			m_socket_accept.close_socket_handle();

			// 4) 모든 Accepted된 socket을 종료한다.
			closesocket_all();

			// 5) 완전히 종료될때까지 기다린다.
			if(m_socket_prepared_to_accept!=0)
			{
				// - 기다린다.
				m_eventClose.WaitOne();
			}

			// Trace) 
			Trace.WriteLine("(Info) Acceptor: Close to Success!! [\""+name+"\"] ("+"Acceptor.on_final_release"+")");

			// referece counting)
			release();

			// return) 
			return true;
		}
		public abstract	Iconnectable	process_alloc_connectable();

		public virtual bool				request_accept()
		{
			// check) Accept가 Disable된 상태라면 잘못된거다.(프로그래밍을 잘못 짠것임.)
			Debug.Assert(enable);

			// check) Listen socket이 닫힌 상태라면 끝낸다.
			if(m_socket_accept.socket_state != eSOCKET_STATE.LISTEN)
				return false;

			// 2) 가로채기 함수 호출.
			on_request_accept();

			// declare) 
			SocketAsyncEventArgs async_accept = m_factory_async_args.alloc();

			// 3) Accept를 걸기 위해서는 AcceptSocket null로 만들어줘야 한다.
			async_accept.AcceptSocket = null;

			// referece counting)
			add_ref();

			try
			{
				// 4) Accept를 수행한다.
				var result = m_socket_accept.accept_async(async_accept);

				// check) 
				if(result == false)
				{
					// - Return값이 false면 즉시 호출한다.
					process_connective_connect(null, async_accept);
				}
			}
			catch(System.Exception e)
			{
				// Trace)
				Trace.WriteLine("(Err ) Acceptor: AcceptAsync()수행에 실패했습니다.[" + name + "/ Error:"+async_accept.SocketError + "] (" + "Acceptor.request_accept" + ")");
				Trace.WriteLine("       "+e);

				// - 할당받았던 async_accept객체를 Free한다.
				m_factory_async_args.free(async_accept);

				// referece counting)
				release();

				// return) 실패!!!
				return false;
			}

			// 5) Accept한 socket을 하나 더한다.
			Interlocked.Increment(ref m_socket_prepared_to_accept);

			// statistics) 접속시도 횟수.
			statistics.statistics_on_try();

			// 6) 할당된 소켓수를 증가시킨다.
			Interlocked.Increment(ref m_nSocketAllocated);

			// return) 성공~
			return true;
		}
		protected bool					add_connectable(Iconnectable _pconnectable)
		{
			lock(m_list_connective)
			{
				m_list_connective.Add(_pconnectable);
			}

			// return) 
			return true;
		}
		protected bool					remove_connectable(Iconnectable _pconnectable)
		{
			lock(m_list_connective)
			{
				// 1) 추가한다.
				m_list_connective.Remove(_pconnectable);
			}

			// return) 
			return true;
		}
		public void						Dispose()
		{
			// 1) Dispose
			this.Dispose(true);

			// 2) GC
			GC.SuppressFinalize(this);
		}
		protected virtual void			Dispose(bool _is_dispose)
		{
			// check) 이미 Disposed되어 있으면 그냥 끝낸다.
			if (m_is_disposed)
				return;

			if (_is_dispose)
			{
				// 1) eventClose를 Dispose한다.
			#if _SUPPORT_NET40
				m_eventClose.Dispose();
			#else
				m_eventClose.Close();
			#endif

				// 2) Accept socket을 Dispose한다.
				m_socket_accept.Dispose();
			}

			// 3) dispose를 true로...
			m_is_disposed = true;
		}

	// implementations)
		private	string					m_name						 = "";
		private	int						m_bEnable					 = 1;
		private	int						m_socket_prepared_to_accept	 = 0;
		private	int						m_accept_must_prepare		 = 0;
		private	int						m_nSocketAllocated			 = 0;
		private	int						m_max_allocate				 = int.MaxValue;

		// - Objects for Interal use
		private	AutoResetEvent			m_eventClose				 = new AutoResetEvent(false);
		private factory.Cobject<SocketAsyncEventArgs> m_factory_async_args;

		// - connective manager
		private	List<Iconnectable>		m_list_connective			 = new List<Iconnectable>();

		// - NSocket for accept
		private io.NSocket				m_socket_accept				 = new io.NSocket();

		// - Status object
		private	Cobject_state			m_component_state			 = new Cobject_state();

		// - statistics
		private statistics.Nconnective	m_statisticsConnective		 = new statistics.Nconnective();

		// - Disposed
		private	bool					m_is_disposed				 = false;
	}
}
