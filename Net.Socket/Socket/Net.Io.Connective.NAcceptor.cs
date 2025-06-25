//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                          Network Socket Classes                           *
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
//  class CGDK.Net.Io.Connective.NAcceptor
//
// * Parameters
//   1) "Address"
//   2) "Port"
//   3) "accept_prepare_on_start"
//   4) "accept_must_prepare"
//
//----------------------------------------------------------------------------
namespace CGDK.Net.Io.Connective
{
	public abstract class NAcceptor : 
		Nreferenceable,
		IAcceptor,
        IDisposable
	{
	// constructor)
		public NAcceptor(string _name = null, int _max_allocate = int.MaxValue)
		{
			// 1) 이름과 MaxAllocate 수를 설정한다.
			m_name = _name;
			m_max_allocate = _max_allocate;

			// 2) NSocket 상태를 Closed로 초기화 한다.
			m_socket_accept.SocketState = eSOCKET_STATE.CLOSED;

			// 3) Socket객체용 Pool을 생성한다.
			m_factory_async_args = new Factory.Object<SocketAsyncEventArgs>("AsyncEventArgs", 
			()=>
			{
				SocketAsyncEventArgs async_accept = new();
				async_accept.Completed += new EventHandler<SocketAsyncEventArgs>(this.ProcessConnectiveConnect);

				return async_accept;
			});

			// 4) State
			this.m_component_state = new ObjectState
			{
				Target = this,
				notifyOnInitializing = this._ProcessOnInitializing,
				notifyOnInitialize = this._ProcessOnInitialize,
				notifyOnDestroying = this._ProcessOnDestroying,
				notifyOnDestroy = this._ProcessOnDestroy,

				notifyOnStarting = this._ProcessOnStarting,
				notifyOnStart = this._ProcessOnStart,
				notifyOnStopping = this._ProcessOnStopping,
				notifyOnStop = this._ProcessOnStop
			};
		}
		~NAcceptor()
		{
			this.Dispose(false);
		}

	// defintions) 
		public const int				NO_START_ACCEPT	 = (-1);

	// publics) 
		public string					Name
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

		public eOBJECT_STATE			Now
		{
			get
			{
				return this.m_component_state.Now;
			}
			set
			{
				this.m_component_state.Now= value;
			}
		}
		public bool						SetObjectStateIf(eOBJECT_STATE _value, eOBJECT_STATE _compare)
		{
			return this.m_component_state.SetObjectStateIf(_value, _compare);
		}

		public bool						Initialize(Context _context)
		{
			return this.m_component_state.Initialize(_context);
		}
		public bool						Destroy()
		{
			return this.m_component_state.Destroy();
		}
		public bool						Start(Context _context)
		{
			return this.m_component_state.Start(_context);
		}
		public bool						Start(IPEndPoint _remote_endpoint, int _accept_prepare_on_start = 0, int _accept_must_prepare = 0)
		{
			// 1) 전달할 Context를 만든다.
			var	temp = new CGDK.Context();
			temp["Address"]				 = _remote_endpoint;
			temp["accept_prepare_on_start"] = _accept_prepare_on_start;
			temp["accept_must_prepare"]	 = _accept_must_prepare;

			// 2) 시작을 요청한다.
			return this.Start(temp);
		}
		public bool						Start(int _port, int _accept_prepare_on_start = 0, int _accept_must_prepare = 0)
		{
			// 1) 전달할 Context를 만든다.
			var temp = new CGDK.Context();
			temp["Port"]				 = _port;
			temp["accept_prepare_on_start"] = _accept_prepare_on_start;
			temp["accept_must_prepare"]	 = _accept_must_prepare;

			// 2) 시작을 요청한다.
			return this.Start(temp);
		}
		public bool						Start(string _address, int _port, int _accept_prepare_on_start = 0, int _accept_must_prepare = 0)
		{
			// 1) 전달할 Context를 만든다.
			var temp = new CGDK.Context();
			temp["Address"]				 = _address;
			temp["accept_prepare_on_start"] = _accept_prepare_on_start;
			temp["accept_must_prepare"]	 = _accept_must_prepare;

			// 2) 시작을 요청한다.
			return this.Start(temp);
		}
		public bool						Start(string _address, string _port, int _accept_prepare_on_start = 0, int _accept_must_prepare = 0)
		{
			// 1) 전달할 Context를 만든다.
			var temp = new CGDK.Context();
			temp["Address"]				 = _address;
			temp["Port"]				 = _port;
			temp["accept_prepare_on_start"] = _accept_prepare_on_start;
			temp["accept_must_prepare"]	 = _accept_must_prepare;

			// 2) 시작을 요청한다.
			return this.Start(temp);
		}
		public bool						Stop()
		{
			return this.m_component_state.Stop();
		}
		public bool						Attach(IObjectStateable _child)
		{
			return this.m_component_state.Attach(_child);
		}
		public int						Detach(IObjectStateable _child)
		{
			return this.m_component_state.Detach(_child);
		}
		public IEnumerator				GetEnumerator()
		{
			return this.m_component_state.GetEnumerator();
		}

		public void						DisconnectAll()
		{
			//m_list_connective.De
		}
		public void						CloseSocketAll()
		{
			// declare) 
			IConnectable[] list_temp;

			lock(this.m_list_connective)
			{
				// check) 
				if(this.m_list_connective.Count == 0)
				{
					return;
				}

				// 1) 갯수만큼 array를 생성한다.
				list_temp = new IConnectable[this.m_list_connective.Count];

				// 2) 임시 배열에다 복사한다.
				this.m_list_connective.CopyTo(list_temp);
			}

			// 2) 모든 Socket들을 Close한다.
			for(int i=0; i<list_temp.Length; ++i)
			{
				list_temp[i].CloseSocket();
			}
		}

		public int						Count
		{
			get {	return this.m_list_connective.Count();}
		}
		public bool						Enable
		{
			get
			{
				return this.m_bEnable!=0;
			}
			set
			{
				var	value_new = value ? 1: 0;
				var value_old = Interlocked.Exchange(ref this.m_bEnable, value_new);

				if(value_old != value_new)
				{
					if(value)
					{
						// Trace) 성공한 Accpet갯수를 출력한다.
						LOG.INFO_LOW(null, "  (Info) Acceptor['" + Name + "'] Enabled (hSocket:" + this.m_socket_accept.SocketHandle.Handle + ")");
					}
					else
					{
						// Trace) 성공한 Accpet갯수를 출력한다.
						LOG.INFO_LOW(null, "  (Info) Acceptor['" + Name + "'] Disabled (hSocket:" + this.m_socket_accept.SocketHandle.Handle + ")");
					}
				}
			}
		}
		public Io.NSocket				AcceptSocket
		{
			get {	return this.m_socket_accept;}
		}
		public Statistics.Nconnective	Statistics
		{
			get { return this.m_statisticsConnective;}
		}


	// frameworks)
		protected virtual void			OnInitializing(Context _context) {}
		protected virtual void			OnInitialize(Context _context) {}
		protected virtual void			OnDestroying() {}
		protected virtual void			OnDestroy() {}

		protected virtual void			OnStarting(Context _context) {}
		protected virtual void			OnStart(Context _context) {}
		protected virtual void			OnStopping() {}
		protected virtual void			OnStop() {}

		protected virtual void			OnRequestAccept() {}
		protected virtual void			OnAccept(IConnectable _connectable) {}
		protected virtual void			OnFailAccept(IConnectable _connectable)	{}

		protected virtual void			OnPrepareAccept() {}
		protected virtual void			OnCloseSocket()	{}

		protected override void			OnFinalRelease()
		{
			// Trace) 
			Trace.WriteLine("(Info) Acceptor: Acceptor Successfully Released[\""+Name+"\"] ("+"Acceptor.OnFinalRelease"+")");

			// 1) 처리...
		}

	// implementations)
		protected void					_ProcessOnInitializing(object _object, CGDK.Context _context)
		{
			this.OnInitializing(_context);
		}
		protected void					_ProcessOnInitialize(object _object, CGDK.Context _context)
		{
			this.OnInitialize(_context);
		}
		protected void					_ProcessOnDestroying(object _object)
		{
			this.OnDestroying();
		}
		protected void					_ProcessOnDestroy(object _object)
		{
			this.OnDestroy();
		}
		protected virtual void			_ProcessOnStarting(object _object, CGDK.Context _context)
		{
			// Trace)
			LOG.PROGRESS(null, "@ <BEGIN> Start ACCEPTOR['"+Name+"']");

			// 1) Context로 Casting
			var context_now = _context ?? throw new System.Exception("invalid Parameter (it's not Context object)");


			//-----------------------------------------------------------------
			// 1. OnStarting 함수 호출
			//-----------------------------------------------------------------
			// 1) OnInitializing 함수를 호출한다.
			this.OnStarting(_context);


			//-----------------------------------------------------------------
			// 2. Parameter 읽기
			//-----------------------------------------------------------------
			// 1)  Default값을 설정한다.
			IPEndPoint remote_ep = new(IPAddress.IPv6Any, 0);			
			int accept_prepare_on_start = 0;
			int accept_must_prepare = 0;

			// 2) Address 값을 읽는다.
			var	str_parameter = context_now["Address"];

			if(str_parameter.IsExist)
			{
				if(str_parameter != "")
				{
					// - Address를 저장한다.
					remote_ep = str_parameter;

					// log) Success
					LOG.INFO(null, "  + parameter['Address']: '"+str_parameter.Value+"'");
				}
				else
				{
					// log) Failed
					LOG.ERROR(null, "  + parameter['Address']: ERROR ('"+str_parameter.Value+"') ");
				}
			}

			// 3) Port 값을 읽는다.
			str_parameter = context_now["Port"];

			if(str_parameter.IsExist)
			{
				// - Port값을 변환한다.
				int	port = str_parameter;

				if(port!=0)
				{
					// - port를 저장한다.
					remote_ep.Port = port;

					// log) Success
					LOG.INFO(null, "  + parameter['Port']: "+str_parameter.Value);
				}
				else
				{
					// log) Failed
					LOG.ERROR(null, "  + parameter['Port']: INVALID PORT ('"+str_parameter.Value +"') ");
				}
			}

			// 4) accept_prepare_on_start
			str_parameter = context_now["accept_prepare_on_start"];

			if(str_parameter.IsExist)
			{
				if(str_parameter.Value=="default")
				{
					accept_prepare_on_start	 = 0;

					// Trace)
					LOG.INFO(null, "  + parameter['accept_prepare_on_start']: 'Default'["+str_parameter.Value +"]");
				}
				else
				{
					accept_prepare_on_start	 = str_parameter;

					// Trace)
					LOG.INFO(null, "  + parameter['accept_prepare_on_start']: "+str_parameter.Value);
				}
			}

			// 5) accept_must_prepare
			str_parameter = context_now["accept_must_prepare"];
			if(str_parameter.IsExist)
			{
				if( str_parameter.Value!="default")
				{
					accept_must_prepare = 0;

					// Trace)
					LOG.INFO(null, "  + parameter['accept_prepare_on_start']: 'default'["+str_parameter.Value +"]");
				}
				else
				{
					accept_must_prepare = str_parameter;

					// Trace)
					LOG.INFO(null, "  + parameter['accept_prepare_on_start']: "+str_parameter.Value);

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


				// 2) Socket을 준비한다.
				this.ProcessPrepareSocket();

				// check) NSocket Status를 Listen상태로 변경한다. (NSocket State가 CLOSED 상태여야만 한다.)
				if(this.m_socket_accept.SetSocketStateIf(eSOCKET_STATE.CLOSED, eSOCKET_STATE.LISTEN) !=eSOCKET_STATE.CLOSED)
				{
					return;
				}

				// 3) Bind한다.
				this.m_socket_accept.RequestBind(remote_ep);

				// Trace)
				LOG.INFO(null, "  > binded to " + this.m_socket_accept.LocalEndPoint.ToString() + " [Requested:"+remote_ep.ToString() + "][hSocket=" + this.m_socket_accept.SocketHandle.Handle + "] ");

				// 4) Listen한다.
				this.m_socket_accept.RequestListen(0);

				// Trace)
				LOG.INFO(null, "  > listen on " + this.m_socket_accept.LocalEndPoint + " [hSocket:" + this.m_socket_accept.SocketHandle.Handle + "]");

				// check) 만약 accept_prepare_on_start가 -1이면 startAccept를 수행하지 않고 그냥 0를 Return한다.
				if(accept_prepare_on_start == NO_START_ACCEPT)
				{
					// Trace) 
					LOG.INFO(null, "  - accepting is not ready because accept_prepare_on_start is 'NO_START_ACCEPT' [Address=" + this.m_socket_accept.LocalEndPoint + "][hSocket=" + this.m_socket_accept.SocketHandle.Handle + "]");

					// return) 
					return;
				}


				//-----------------------------------------------------------------
				// 5. Accept를 건다.
				//-----------------------------------------------------------------
				// Trace)
				LOG.INFO(null, "  - accept_prepare_on_start="+accept_prepare_on_start+", accept_must_prepare="+accept_must_prepare);

				// 1) Status를 Enable로 설정한다.
				this.Enable = true;

				// declare) 
				int accept_count = 0;

				// 3) Accept 시도 횟수를 계산한다.
				accept_prepare_on_start	-= this.m_socket_prepared_to_accept;

				try
				{
					// - accept_prepare_on_start만큼 Accept를 건다.
					for(; accept_prepare_on_start>0; --accept_prepare_on_start, ++accept_count)
					{
						// - Accept를 수행한다.
						this.RequestAccept();
					}

					// Trace) 걸어놓은 Accpet갯수를 출력한다.
					LOG.INFO(null, "  > Acceptor started ["+accept_count+" sockets are ready for accepting] ");
				}
				catch(System.Exception)
				{
					// log) 
					LOG.ERROR(null, "@ (Excp) System.Exception Complete[RequestAccept:\"" + Name + "\"(hSocket:" + m_socket_accept.SocketHandle.Handle + ")] (CGDK.Net.NAcceptor._ProcessOnStarting)");

					// reraise)
					throw;
				}

				// 4) 값을 설정한다.
				this.m_accept_must_prepare = accept_must_prepare;
			}
			catch(System.Exception)
			{
				// log) 
				LOG.ERROR(null, "@ (Excp) System.Exception Complete[RequestAccept:\"" + Name + "\"(hSocket:" + m_socket_accept.SocketHandle.Handle + ")] (CGDK.Net.NAcceptor._ProcessOnStarting)");

				// - 닫는다!
				this._ProcessOnStopping(_object);

				// Reriase)
				throw;
			}
		}
		protected virtual void			_ProcessOnStart(object _object, Context _context)
		{
			// 1) Hook함수를 호출한다.
			this.OnStart(_context);

			// Trace)
			LOG.PROGRESS(null, "@ <END> Start ACCEPTOR['"+Name+"']");
		}
		protected virtual void			_ProcessOnStopping(object _object)
		{
			// Trace)
			LOG.PROGRESS(null, "@ <BEGIN> Stop ACCEPTOR['"+Name+"']");

			// 1) Hook함수를 호출
			this.OnStopping();

			// 2) Disable로 상태를 바꾼다.
			this.Enable = false;

			// 3) Socket을 닫는다.
			this.ProcessCloseSocket();

			// Trace) 걸어놓은 Accpet갯수를 출력한다.
			LOG.INFO(null, "  < ACCEPTOR["+Name+"] Closed");
		}
		protected virtual void			_ProcessOnStop(object _object)
		{
			// 4) Hook함수를 호출
			this.OnStop();

			// Trace)
			LOG.PROGRESS(null, "@ <END> Stop ACCEPTOR['"+Name+"']");
		}
	
		protected bool					ProcessConnectiveAccept(SocketAsyncEventArgs _args)
		{
			// check) SocketError가 발생했거나 최대 할당 갯수를 초과했을 경우 System.Exception을 던진다.
			if(_args.SocketError!= SocketError.Success || m_nSocketAllocated>m_max_allocate)
				return false;
		
			//-----------------------------------------------------------------
			// 1. Connectable 객체 준비하기.
			//-----------------------------------------------------------------
			// declare) 
			IConnectable pconnectable = null;

			// check) AcceptSocket null이면 당연히 안된다.
			Debug.Assert(_args.AcceptSocket != null);

			// check) 
			Debug.Assert(_args.AcceptSocket.Handle != 0);

			try
			{
				// 1) Port를 Reuse하도록 한다.(중요:Connect의 경우 Pool로 동작할 때 NSocket Address를 Reuse하지 않으면 Error를 발생시킬수 있다.)
				_args.AcceptSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
				_args.AcceptSocket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);

				// 2) 새로운 Accept를 댄다.
				pconnectable = this.ProcessAllocConnectable();

				// Reference Counting) 
				pconnectable.AddReference();

				// check)
				Debug.Assert(pconnectable.ReferenceCount==1);
		
				// 3) Socket상태를 SYN으로 바꾼다.
				pconnectable.Socket.SocketState = eSOCKET_STATE.SYN;

				// 4) set default Socket option
				_args.AcceptSocket.Blocking = false;

				// 5) Socket을 설정한다.
				pconnectable.Socket.SocketHandle = _args.AcceptSocket;
			}
			// Case FAIL) Accept가 제대로 되지 않았을 경우.
			catch(System.Exception e)
			{
				// - On Fail Accept
				try
				{
					this.OnFailAccept(pconnectable);
				}
				catch(System.Exception e2)
				{
					// Trace) 
					Trace.WriteLine("(Excp) Acceptor: System.Exception Complete [NotifyOnFailAccept] (" + "CGDK.Net.NAcceptor.ProcessConnectiveAccept" + ")");
					Trace.WriteLine("       "+e2);
				}

				// Trace) 
				Trace.WriteLine("(Excp) Acceptor: System.Exception Complete [OnAccept:" + _args.AcceptSocket + "] ("+ "CGDK.Net.NAcceptor.ProcessConnectiveAccept" + ")");
				Trace.WriteLine("       "+e);

				// return) 
				return false;
			}

			// 주의) 여기까지 통과했으면 Return값은 true로 해줘야 한다.


			//-----------------------------------------------------------------
			// 2. Accept받은 Socket의 처리.
			// 
			//    Case SUCCRESS) Accept가 제대로 되었을 경우.
			//    - Accept가 제대로 되었을 때는 Accept처리를 한다.
			//    - Accept 받은 Socket의 Option을 설정하고... (UpdateAcceptContext)
			//    - NotifyOnAccept()함수를 호출하고...
			//    - pconnectable의 ProcessCompleteConnect()함수를 호출하여 
			//      소켓별 접속처리를 진행한다.
			//-----------------------------------------------------------------
			try
			{
				// 1) Hook함수를 호출한다.
				this.OnAccept(pconnectable);

				// Statistics) 접속성공 횟수
				this.Statistics.statistics_on_success_connect();

				// 2) Connectable의 Connective를 this로 한다.
				pconnectable.Connective = this;

				// check) 
				Debug.Assert(pconnectable.ReferenceCount==1);

				// 3) Connected List에 추가한다.
				this.AddConnectable(pconnectable);

				// 4) ProcessCompleteConnect를 수행한다.
				if(pconnectable.ProcessCompleteConnect(_args)==false)
				{
					// - Connectable에서 제거한다.
					this.RemoveConnectable(pconnectable);

					// - Connective에서 Reset
					pconnectable.Connective = null;
				}
			}
			catch(System.Exception e)
			{
				// - On Fail Accept
				try
				{
					this.OnFailAccept(pconnectable);
				}
				catch(System.Exception e2)
				{
					// Trace) 
					Trace.WriteLine("(Excp) Acceptor: System.Exception Complete [NotifyOnFailAccept] (" + "CGDK.Net.NAcceptor.ProcessConnectiveAccept" + ")");
					Trace.WriteLine("       "+e2);
				}

				// Trace) 
				Trace.WriteLine("(Excp) Acceptor: System.Exception Complete [NotifyOnAccept] (" + "CGDK.Net.NAcceptor.ProcessConnectiveAccept" + ")");
				Trace.WriteLine("       "+e);

				// - Connectable의 Socket을 닫는다.
				pconnectable.CloseSocket();

				// Statistics) 접속실패 횟수
				this.Statistics.StatisticsOnFailConnect();

				// - 할당된 소켓수를 줄인다.
				--this.m_nSocketAllocated;
			}

			// Reference Counting) 
			pconnectable.Release();

			// return) 
			return true;
		}
		public void						ProcessConnectiveConnect(object _source, SocketAsyncEventArgs _args)
		{
			//-----------------------------------------------------------------
			// ProcessCompleteAccept()함수는 크게 두 부분으로 나눌수 있다.
			//
			// * 첫째는 Accept받은 Socket의 접속처리 부분.
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
			var count_prepare_to_accept	 = Interlocked.Decrement(ref this.m_socket_prepared_to_accept);


			//-----------------------------------------------------------------
			// 2. 현재 Acceptor가 Disable 혹은 소켓 최대 한도 초과 시
			//-----------------------------------------------------------------
			// check) Accept가 Disable된 상태나 최대 할당 수보다 많은 경우라면 끝낸다.
			if(this.Enable == false)
			{
				// Statistics)
				this.Statistics.StatisticsOnFailConnect();

				// - Connectable의 Socket을 닫는다.
				_args.AcceptSocket?.Close();

				// - 할당된 소켓수를 증가시킨다.
				--this.m_nSocketAllocated;

				// - 할당해제.
				this.m_factory_async_args.Free(_args);

				// - Signal
				if(count_prepare_to_accept == 0)
					this.m_eventClose.Set();

				// referece counting)
				this.Release();

				// return) Return한다.
				return;
			}

			// 1) Accept처리를 한다.
			var result = this.ProcessConnectiveAccept(_args);

			// check) 
			if(result == false)
			{
				// - Socket을 Close한다.
				_args.AcceptSocket?.Close();

				// Statistics) 접속실패 횟수
				Statistics.StatisticsOnFailConnect();

				// 5) 할당된 소켓수를 줄인다.
				--this.m_nSocketAllocated;
			}

			// 2) AcceptSocket 바로 Clear해버린다.
			_args.AcceptSocket = null;


			//-----------------------------------------------------------------
			// 4. 새로운 Accept를 걸기.
			//-----------------------------------------------------------------
			try
			{
				// 1) Prepare할 갯수를 얻는다. (최하값이 0이 되도록 한다.)
				int	iRequest = (this.m_accept_must_prepare > this.m_socket_prepared_to_accept) ? this.m_accept_must_prepare - this.m_socket_prepared_to_accept : 0;

				// 2) Prepare할 갯수만큼 Accept를 수행한다.
				for(; iRequest > 0; --iRequest)
				{
					this.RequestAccept();	// (*) System.Exception이 발생할 수 있다.
				}

				// check) 준비한 Accept NSocket 수가 0보다 작을 수는 없음.
				Debug.Assert(this.m_socket_prepared_to_accept>=0);
			}
			catch(System.Exception e)
			{
				// Trace) 
				Trace.WriteLine("(Excp) Acceptor: System.Exception Complete [RequestAccept:\"" + Name+"\"] ("+ "CGDK.Net.NAcceptor.ProcessConnectiveConnect" + ")");
				Trace.WriteLine("       "+e);
			}

			// Statistics) Overlapped 걸린 수를 줄인다.
			this.Statistics.StatisticsOnDecreseAsync();

			// 3) 할당해제.
			this.m_factory_async_args.Free(_args);

			// referece counting)
			this.Release();
		}
		public void						ProcessConnectiveDisconnect(IConnectable _pconnectable)
		{
			// 1) Connectable Manager에서 제거한다. 
			this.RemoveConnectable(_pconnectable);

			// 2) Connectable의 Connective를 this로 한다.
			_pconnectable.Connective = null;

			// 4) Allocated된 Socket수를 줄인다.
			--this.m_nSocketAllocated;

			// Statistics)
			this.Statistics.StatisticsOnErrorDisconnect(_pconnectable.Statistics.is_error_disconnected());

			// Statistics) 
			this.Statistics.StatisticsOnDisconnect();

			// check) 만약 Prepare된 숫자가 모자랄 경우 추가 Accept를 건다.
			if(this.Enable && this.m_accept_must_prepare > this.m_socket_prepared_to_accept)
			{
				// - Exhaust될 경우 추가 Accept를 수행한다.
				try
				{
					this.RequestAccept();	// (*) Exception이 발생할 수 있다.
				}
				catch(System.Exception e)
				{
					// Trace)
					Trace.WriteLine("(Err ) Acceptor: System.Exception Complete [RequestAccept:\"" + Name+"\" [Handle] ("+"Acceptor.ProcessConnectiveDisconnect"+")");
					Trace.WriteLine("       "+e);
				}
			}
		}
		protected void					ProcessPrepareSocket()
		{
			// 1) Socket을 새로 만든다.
			var result = this.m_socket_accept.CreateSocketHandle(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);

			// check) NSocket 생성하지 못했으면 던진다~
			if(result != true)
				throw new System.Exception();

			// 2) set default parameters
			this.m_socket_accept.SocketHandle.Blocking = false;

			// 3) Socket을 Closed로 한다.
			this.m_socket_accept.SocketState = eSOCKET_STATE.CLOSED;

			try
			{
				// Statistics1)
				this.Statistics.Reset();

				// 4) call 'OnPrepareAccept'
				this.OnPrepareAccept();
			}
			catch(System.Exception e)
			{
				// Trace)
				Trace.WriteLine("(Err ) Acceptor: Fail on OnPrepareSocket [\""+Name+"\" ("+"Acceptor.ProcessPrepareSocket"+")");
				Trace.WriteLine("       "+e);

				// - Socket을 닫는다.
				this.m_socket_accept.CloseSocketHandle();

				// reraise)
				throw;
			}

			// referece counting)
			this.AddReference();
		}
		protected bool					ProcessCloseSocket(ulong _disconnect_reason = DISCONNECT_REASON.NONE)
		{
			// 1) NSocket Status를 Disconnected로...
			eSOCKET_STATE pre_socket_status = this.m_socket_accept.ExchangeSocketState(eSOCKET_STATE.CLOSED);

			// check) 이전 Status도 Close상태라면 그냥 함수를 종료한다.
			if(pre_socket_status == eSOCKET_STATE.CLOSED)
				return false;

			// Trace) 
			Trace.WriteLine("(Info) Acceptor: Closing Acceptor. Waiting for completion of "+m_socket_prepared_to_accept+" asychronous accepts [\""+Name+"\": "+m_socket_accept.LocalEndPoint+"] ("+"Acceptor.ProcessCloseSocket"+")");

			// 2) HOOK함수 호출
			this.OnCloseSocket();

			// 3) Socket을 닫는다.
			this.m_socket_accept.CloseSocketHandle();

			// 4) 모든 Accepted된 Socket을 종료한다.
			this.CloseSocketAll();

			// 5) 완전히 종료될때까지 기다린다.
			if(this.m_socket_prepared_to_accept!=0)
			{
				// - 기다린다.
				this.m_eventClose.WaitOne();
			}

			// Trace) 
			Trace.WriteLine("(Info) Acceptor: Close to Success!! [\""+Name+"\"] ("+"Acceptor.OnFinalRelease"+")");

			// referece counting)
			this.Release();

			// return) 
			return true;
		}
		public abstract	IConnectable	ProcessAllocConnectable();

		public virtual bool				RequestAccept()
		{
			// check) Accept가 Disable된 상태라면 잘못된거다.(프로그래밍을 잘못 짠것임.)
			Debug.Assert(this.Enable);

			// check) Listen Socket이 닫힌 상태라면 끝낸다.
			if(this.m_socket_accept.SocketState != eSOCKET_STATE.LISTEN)
				return false;

			// 2) 가로채기 함수 호출.
			this.OnRequestAccept();

			// declare) 
			SocketAsyncEventArgs async_accept = this.m_factory_async_args.Alloc();

			// 3) Accept를 걸기 위해서는 AcceptSocket null로 만들어줘야 한다.
			async_accept.AcceptSocket = null;

			// referece counting)
			this.AddReference();

			try
			{
				// 4) Accept를 수행한다.
				var result = this.m_socket_accept.AcceptAsync(async_accept);

				// check) 
				if(result == false)
				{
					// - Return값이 false면 즉시 호출한다.
					this.ProcessConnectiveConnect(null, async_accept);
				}
			}
			catch(System.Exception e)
			{
				// Trace)
				Trace.WriteLine("(Err ) Acceptor: AcceptAsync()수행에 실패했습니다.[" + Name + "/ Error:"+async_accept.SocketError + "] (" + "Acceptor.RequestAccept" + ")");
				Trace.WriteLine("       "+e);

				// - 할당받았던 async_accept객체를 Free한다.
				this.m_factory_async_args.Free(async_accept);

				// referece counting)
				this.Release();

				// return) 실패!!!
				return false;
			}

			// 5) Accept한 Socket을 하나 더한다.
			Interlocked.Increment(ref this.m_socket_prepared_to_accept);

			// Statistics) 접속시도 횟수.
			this.Statistics.statistics_on_try();

			// 6) 할당된 소켓수를 증가시킨다.
			Interlocked.Increment(ref this.m_nSocketAllocated);

			// return) 성공~
			return true;
		}
		protected bool					AddConnectable(IConnectable _pconnectable)
		{
			lock(this.m_list_connective)
			{
				this.m_list_connective.Add(_pconnectable);
			}

			// return) 
			return true;
		}
		protected bool					RemoveConnectable(IConnectable _pconnectable)
		{
			lock(this.m_list_connective)
			{
				// 1) 추가한다.
				this.m_list_connective.Remove(_pconnectable);
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
			if (this.m_is_disposed)
				return;

			if (_is_dispose)
			{
				// 1) eventClose를 Dispose한다.
			#if _SUPPORT_NET40
				this.m_eventClose.Dispose();
			#else
				this.m_eventClose.Close();
			#endif

				// 2) Accept Socket을 Dispose한다.
				this.m_socket_accept.Dispose();
			}

			// 3) dispose를 true로...
			this.m_is_disposed = true;
		}

	// implementations)
		private	string					m_name = "";
		private	int						m_bEnable = 1;
		private	int						m_socket_prepared_to_accept = 0;
		private	int						m_accept_must_prepare = 0;
		private	int						m_nSocketAllocated = 0;
		private	int						m_max_allocate = int.MaxValue;

		// - Objects for Interal use
		private	AutoResetEvent			m_eventClose = new(false);
		private Factory.Object<SocketAsyncEventArgs> m_factory_async_args;

		// - Connective manager
		private	List<IConnectable>		m_list_connective = new();

		// - NSocket for accept
		private Io.NSocket				m_socket_accept = new();

		// - Status object
		private	ObjectState				m_component_state = new();

		// - Statistics
		private Statistics.Nconnective	m_statisticsConnective = new();

		// - Disposed
		private	bool					m_is_disposed = false;
	}
}
