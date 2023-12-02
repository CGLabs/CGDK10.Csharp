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
using System.Net;
using System.Net.Sockets;
using CGDK;

//----------------------------------------------------------------------------
//
//  class CGDK.net.socket.Itcp_client
//
//    "[Name]/Address"				접속할 주소(주소와 포트 모두 포함)
//	  "[Name]/Port"					접속할 포트
//	  "[Name]/enable_reconnection"	재접속을 활성화한다.
//	  "[Name]/DisableReconnection"	재접속을 비활성화한다.
//	  "[Name]/Reconnection"			재접속 상태를 설정한다.(true/false)
//
//----------------------------------------------------------------------------
namespace CGDK.net.socket
{
	public class Itcp_client : 
		Itcp,
		io.Iconnect_requestable,
        CGDK.Inameable,
		CGDK.Iobject_stateable,
        CGDK.Istartable,
        CGDK.Iinitializable
	{
	// constructors) 
		public Itcp_client()
		{
			m_component_state						 = new Cobject_state();
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
		public Itcp_client(string _name) : this()
		{
			m_name = _name;
		}

	// publics) 
		public string					name
		{
			get { return m_name;}
			set { m_name=value;}
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
		public bool						start(string _address, short _port)
		{
			// declare) 
			IPEndPoint remote_endpoint = null;
				
			try
			{		
				// 1) IPEndPoint를 구한다.
				remote_endpoint = CGDK.net.dns.make_endpoint(_address);
			}
			catch(FormatException)
			{
				return	false;
			}
			catch(System.Exception)
			{
				return	false;
			}

			// 2) Port를 구한다.
			remote_endpoint.Port = _port;

			// 3) 접속을 시도한다.
			return process_request_connecting(remote_endpoint);
		}
		public bool						start(string _address, string _port)
		{
			// declare) 
			IPEndPoint remote_endpoint = null;
				
			try
			{		
				// 1) IPEndPoint를 구한다.
				remote_endpoint  = CGDK.net.dns.make_endpoint(_address);
			}
			catch(FormatException)
			{
				return false;
			}
			catch(System.Exception)
			{
				return false;
			}

			// 2) Port를 구한다.
			remote_endpoint.Port = int.Parse(_port);

			// 3) 접속을 시도한다.
			return process_request_connecting(remote_endpoint);
		}
		public bool						start(IPEndPoint _remote_endpoint)
		{
			return process_request_connecting(_remote_endpoint);
		}
		public bool						stop()
		{
			return	closesocket();
		}
		public	bool					attach(Iobject_stateable _child)
		{
			return false;
		}
		public	int						detach(Iobject_stateable _child)
		{
			return 0;
		}
		public IEnumerator				GetEnumerator()
		{
			return m_component_state.GetEnumerator();
		}
		public bool						enable_reconnection
		{
			get
			{
				return m_enable_reconnection;
			}
			set
			{
				m_enable_reconnection = value;
			}
		}
		public long						reconnection_interval
		{
			get
			{
				return m_reconnection_interval;
			}
			set
			{
				m_reconnection_interval = value;
			}
		}

	// frameworks)
		protected virtual void			on_request_connect(IPEndPoint _remote_endpoint)
		{
		}
		protected virtual void			on_request_reconnect(CGDK.net.io.Iconnective _connective)
		{
		}

		protected virtual void			on_initializing(object _object, context _context)
		{
		}
		protected virtual void			on_initialize(object _object, context _context)
		{
		}
		protected virtual void			on_destroying(object _object)
		{
		}
		protected virtual void			on_destroy(object _object)
		{
		}

		protected virtual void			on_starting(object _object, context _context)
		{
		}
		protected virtual void			on_start(object _object, context _context)
		{
		}
		protected virtual void			on_stopping(object _object)
		{
		}
		protected virtual void			on_stop(object _object)
		{
		}


	// implementation)
		protected virtual void			_process_on_request_connect(IPEndPoint	_remote_endpoint)
		{
			on_request_connect(_remote_endpoint);
		}
		protected virtual void			_process_on_request_reconnect(CGDK.net.io.Iconnective _connective)
		{
			on_request_reconnect(_connective);
		}

		protected virtual void			_process_on_initializing(object _object, context _context)
		{
			on_initializing(_object, _context);
		}
		protected virtual void			_process_on_initialize(object _object, context _context)
		{
			on_initialize(_object, _context);
		}
		protected virtual void			_process_on_destroying(object _object)
		{
			on_destroying(_object);
		}
		protected virtual void			_process_on_destroy(object _object)
		{
			on_destroy(_object);
		}

		protected virtual void			_process_on_starting(object _object, context _context)
		{
			// log)
			LOG.PROGRESS(null, "@ <BEGIN> start CONNECTOR['" + name + "']");


			//-----------------------------------------------------------------
			// 1. on_starting 함수 호출
			//-----------------------------------------------------------------
			// 1) 먼저...
			on_starting(_object, _context);


			//-----------------------------------------------------------------
			// 2. Parameter 읽기
			//"   [Name]/Address"		접속할 주소(주소와 포트 모두 포함)
			//"   [Name]/Port"			접속할 포트
			//"   [Name]/Reconnection"	재접속 상태를 설정한다.(true/false)
			//-----------------------------------------------------------------
			// 1) Context를 얻는다.
			context	context_now = _context;

			// check)
			if(context_now == null)
				throw new System.Exception("invalid parameter (it's not context object)");

			// declare) 
			IPEndPoint ipPoint = null;

			// 3) IP Address
			{
				var	param = context_now["Address"];
			
				if(param.exist)
				{
					try
					{
						ipPoint	 = CGDK.net.dns.make_endpoint(param.value);

						// log)
						LOG.INFO(null, "  + parameter['Address']: '"+ipPoint.ToString()+"'");
					}
					catch(FormatException)
					{
						// log) 
						LOG.ERROR(null, "  + parameter['Address']: ERROR ('"+param+"') ");
					}
					catch(System.Exception)
					{
						// log) 
						LOG.ERROR(null, "  + parameter['Address']: ERROR ('"+param+"') ");
					}
				}
			}
			// 4) Reconnection 
			{
				bool bReconnection = false;
				var param = context_now["Reconnection"];
			
				if(param.exist)
				{
					switch(param.value)
					{
					case	"true":
					case	"enable":
							{
								enable_reconnection	 = true;

								// Trace)
								LOG.INFO(null, "  + parameter['Reconnection']: 'enable'");
							}
							break;

					case	"false":
					case	"disable":
							{
								enable_reconnection	 = false;

								// Trace)
								LOG.INFO(null, "  + parameter['Reconnection']: 'disable'");
							}
							break;

					case	"toggle":
							{
								bReconnection = !bReconnection;
								enable_reconnection	 = bReconnection;

								// Trace)
								LOG.INFO(null, "  + parameter['Reconnection']: 'toggle'["+(bReconnection ? "enable" : "disable")+"]");
							}
							break;

					case	"default":
							{
								enable_reconnection	 = false;

								// Trace)
								LOG.INFO(null, "  + parameter['Reconnection']: 'default'[disable]");
							}
							break;

					default:
							break;
					}
				}
			}
			// 5) Reconnection Interval
			{
				var	param = context_now["ReconnectionInterval"];
			
				if(param.exist)
				{
					var	temp_reconnection_interval = Int32.Parse(param.value);

					if(temp_reconnection_interval > 0)
					{
						// - Port값을 저장한다.
						reconnection_interval = temp_reconnection_interval * TimeSpan.TicksPerSecond;

						// log)
						LOG.INFO(null, "  + parameter['ReconnectionInterval']: " + temp_reconnection_interval);
					}
					else
					{
						// log) 
						LOG.ERROR(null, "  + parameter['Port']: INVALID ReconnectionInterval ('" + temp_reconnection_interval+"') ");
					}
				}
			}  

			// check) Address가 null이면 System.Exception을 던진다.
			if(ipPoint == null)
			{
				throw new System.Exception();
			}

			// 6) Request Connect
			process_request_connecting(ipPoint);
		}
		protected virtual void			_process_on_start(object _object, CGDK.context _context)
		{
			// 1) 
			on_start(_object, _context);

			// log)
			LOG.PROGRESS(null, "@ <END> start CONNECTOR['" + name + "']");
		}
		protected virtual void			_process_on_stopping(object _object)
		{
			// log)
			LOG.PROGRESS(null, "@ <BEGIN> stop CONNECTOR['" + name + "']");

			// 1) on_stopping
			on_stopping(_object);

			// 2) Reconnect를 Disable 한다.
			enable_reconnection	 = false;

			// 3) 기존 Reconnect도 Cancel한다.
			cancel_reconnector();
		}
		protected virtual void			_process_on_stop(object _object)
		{
			// 1) On stop
			on_stop(_object);

			// log)
			LOG.PROGRESS(null, "@ <END> stop CONNECTOR['" + name + "']");
		}

		public bool						process_request_connecting(IPEndPoint _remote_endpoint)
		{
			// 1) _paddrPeer가 nullptr이 아닐 경우 모든 List를 제거하고 _paddrPeer하나만 등록한다.
			if(_remote_endpoint != null)
			{
				// - 모든 PeerAddress를 제거한다.
				remove_all_peer_address();

				// - 새로 들어온 PeerAddress를 추가한다.
				add_peer_address(_remote_endpoint);
			}

			// check)
			if(_remote_endpoint == null)
				return false;

			// declare) 
			IPEndPoint	address_connect;

			// check) Pointer가 nullptr이라면 하나도 추가가 되지 않은 것이다.
			if(m_list_peer_address.Count == 0)
				return false;

			// 2) Address를 하나 읽어 접속을 시도한다.
			lock(m_list_peer_address)
			{
				// - Index
				m_index_peer_address = (m_index_peer_address++) % m_list_peer_address.Count;

				// - 현재 Address를 얻고 다음 Address로 변경한다.
				address_connect = m_list_peer_address[m_index_peer_address];
			}
	
			// 3) 접속 시도시 m_tick_disconnected 시간을 갱신한다.
			m_tick_disconnected = System.DateTime.Now.Ticks;
	
			// 4) Reconning을 false로 한다.
			m_is_reconnecting = false;


			// * 상태변경
			//
			// - 상태변경을 하면서 이미 Connect상태이거나 Connect를 시도한 상태인지
			//   를 구분하여 만약 이미 Connect를 시도한 상태이거나 Connect상태이면
			//   바로 Return할수 있도록 처리를 해주어야 한다.
			// - 이를 위해 atomic를 사용하여 socket_state를 변경하여 그 이전
			//   값을 Check하여 처리한다.
			// 

			// 5) socket상태를 eSOCKET_STATE.SYN으로 바꾼다.
			//    - 내부적으로 atomic함수를 통해 CLOSED 상태로 변경한다.
			//    - 여기서 atomic함수를 사용하는 이유는 상황에 따라 중복적으로 
			//      Connect와 Disconnect등 socket의  상태가 불려지는 함수가 겹쳐
			//      서 호출될수 있기 때문이다. 
			//    - socket의 상태가 CLOSED일 때만 Connect함수가 수행되고 아닐 경우 
			//      더 이상 진행하지 않고 false를 리턴한다.
			var	pre_socket_status = socket.set_socket_state_if(eSOCKET_STATE.CLOSED, eSOCKET_STATE.SYN);

			// check) 이전 상태가 CLOSED가 아니면 안된다.
			if(pre_socket_status != eSOCKET_STATE.CLOSED)
			{
				return false;
			}

			// 6) socket을 준비한다.
			process_prepare_socket(_remote_endpoint.AddressFamily);

      		// 7) on_request_connect를 호출한다.
			_process_on_request_connect(address_connect);

			// 8) Async Event 객체를 생성한다.
			SocketAsyncEventArgs_connect async_event = m_factory_async_args_connect.alloc();

			// 9) Address를 설정한다.
			async_event.SetBuffer(null, 0, 0);
			async_event.BufferList		 = null;
			async_event.RemoteEndPoint   = address_connect;
			async_event.SocketFlags		 = SocketFlags.None;
			async_event.AcceptSocket     = null;
			async_event.UserToken        = this;

			// declare)
			bool result = false;

			// Reference Counting) 
			add_ref();

			// 10) 접속을 시도한다. Connect to the remote endpoint.
			result = socket.connect_async(async_event);

			// check) ConnectAsync함수의 성공여부를 확인다.
			if(result == false)
			{
				// - socket을 닫는다.
				process_closesocket();

				// log) 
				LOG.ERROR(null, "(Err ) CONNECTOR['" + name + "']: Fail to connecting socket [Error:"+async_event.SocketError+"] (CGDK.net.socket.Itcp_client.process_request_connecting)");

				// - 접속종료상태로 변경.
				socket.socket_state = eSOCKET_STATE.CLOSED;

				// - _args 초기화 및 할당해제
				async_event.UserToken = null;
				async_event.RemoteEndPoint = null;
				m_factory_async_args_connect.free(async_event);

				// Reference Counting) 
				release();

				// return) 실패를 return한다.
				return false;
			}

			// statistics) 접속 시도 횟수
			statistics.statistics_on_connect_try();

			// log) 걸어놓은 Accpet갯수를 출력한다.
			LOG.INFO_LOW(null, "  > trying to connect '"+address_connect.ToString()+"'");

			// return) 접속시도에 성공했다!
			return true;
		}
		protected static void			process_complete_connecting(object _source, SocketAsyncEventArgs _args)
		{
			// 1) socket을 얻는다.
			var psocket_tcp = _args.UserToken as Itcp_client;

			// declare) 
			bool result = false;

			try
			{
				// 2) process_complete_connect함수를 호출한다.
				result = psocket_tcp.process_complete_connect(_args);
			}
			catch(System.Exception )
			{
			}

			// 3) _args 초기화 및 할당해제
			_args.RemoteEndPoint = null;
			_args.UserToken = null;
			m_factory_async_args_connect.free(_args as SocketAsyncEventArgs_connect);

			// 4) Connection에 실패했을 경우 다시 Reconnect를 요청한다.
			if(result == false)
			{
				psocket_tcp.request_reconnect();
			}
			else
			{
				// - 접속에 성공했으므로 재접속처리를 종료한다.
				psocket_tcp.cancel_reconnector();
			}

			// 6) socket을 Release한다.
			psocket_tcp.release();
		}
		protected override void			process_disconnect()
		{
			// 1) connective의 process_connective_disconnect를 호출한다.
			if(connective != null)
			{
				// - connective의 Disconnect 처리
				connective.process_connective_disconnect(this);
			}

			// 2) 재접속처리
			process_disconnect_reconnection();

			// statistics) 
			io.statistics.Nconnective.total.statistics_on_disconnect();
		}
		public void						process_wait_reconnect(object _object, object _param)
		{
			lock(m_cs_reconnector)
			{
				// check) Connection상태면 더이상 진행하지 않는다.
				if(m_is_reconnecting == false)
					return;

				// check) 만약에 Disable상태라면 재접속을 하지 않는다.
				if(enable_reconnection == false)
				{
					// 1) Reconnector를 저장해 놓는다.
					reconnector	reconnector = m_reconnector;

					// check) Reconnector가 Empty인지 확인한다.
					if(reconnector != null)
					{
						// 2) Reconnector를 Reset한다.
						m_reconnector = null;

						// Critical) Criticalsection Leave!!
						//LOCK_LEAVE(cs)

						// 3) Reconnector를 detach한다.
						reconnector.detach();
					}

					// return) 
					return;
				}

				// 1) 접속 종료한 시점 이후의 tick을 구한다.
				long tickdifferExecute = System.DateTime.Now.Ticks - m_tick_disconnected;

				// check) 접속 종료후 m_reconnection_interval보다 시간이 더 지나지 않았으면 그냥 끝낸다.
				if(tickdifferExecute < m_reconnection_interval)
					return;

				// 2) 재접속 시도 시간을 설정한다.
				m_tick_disconnected	+= m_reconnection_interval;

				// 3) Reconnection을 시작했음으로 설정한다.
				m_is_reconnecting = false;

				// Trace) 
				LOG.INFO_LOW(null, "(Prgr) CGNetSocket['" + name + "']: Trying to reconnection (CGDK.net.socket.Itcp_client.process_wait_reconnect)");

				// 4) on_request_reconnect함수를 호출한다.
				_process_on_request_reconnect(null);
			}

			// 5) 기존 socket을 닫는다.
			closesocket();

			// 6) 다시 접속한다.(재접속을 위해서는 nullptr을 넘겨야 한다.)
			process_request_connecting(null);
		}
		private void					process_disconnect_reconnection()
		{
			lock(m_cs_reconnector)
			{
				// 1) connective를 Reset한다.
				connective = null;

				// 2) 접속 종료시 m_tick_disconnect시간을 설정한다.
				var	tick_now = System.DateTime.Now.Ticks;
				var	tick_differ_execute = tick_now - m_tick_disconnected;

				// check) 마지막 접속시도 시간보다 m_dwReconnectInterval이  
				//        Case 지나지 않았으면 -> 마지막 접속시간+재접속시도간격
				//        Case 지났으면       -> 현재시간+재접속시도간격
				if(tick_differ_execute < m_reconnection_interval)
				{
					m_tick_disconnected = m_tick_disconnected + m_reconnection_interval;
				}
				else
				{
					m_tick_disconnected = tick_now + m_reconnection_interval;
				}
			}

			// 3) Reconnect를 진행한다.
			request_reconnect();
		}

		private void					request_reconnect()
		{
			// check) Reconnection이 Disable되었을 경우 그냥 끝낸다.
			if(m_enable_reconnection == false)
			{
				// - Reconnect를 Cancel한다.
				cancel_reconnector();

				// return) 
				return;
			}

			lock(m_cs_reconnector)
			{
				// 1) Reconnection을 시작했음으로 설정한다.
				m_is_reconnecting = true;

				// check) 이미 Reconnect가 걸린 경우 그냥 끝낸다.
				if(m_reconnector != null)
					return;

				// 2) Reconnector를 설정한다.
				m_reconnector = new reconnector();

				// 3) attach한다.
				if(m_reconnector.attach(this)==false)
				{
					m_reconnector = null;
					m_is_reconnecting = false;
				}
			}
		}
		private void					cancel_reconnector()
		{
			// declare) 
			reconnector reconnector;

			lock(m_cs_reconnector)
			{
				// 1) Notifier를 얻는다.
				reconnector = m_reconnector;

				// check) Reconnector가 Empty인지 확인한다.
				if(reconnector == null)
					return;

				// 2) Reconnection을 false로...
				m_is_reconnecting = false;

				// 3) Reconnector를 Reset한다.
				m_reconnector = null;
			}

			// 4) detach한다.
			reconnector.detach();
		}

		public bool						add_peer_address(IPEndPoint _remote_endpoint)
		{
			lock(m_list_peer_address)
			{
				// check) 먼저 등록된 것이 있는지 찾는다.
				if(m_list_peer_address.FindIndex(x => x==_remote_endpoint)>=0)
					return	false;

				// 1) 등록된 것이 없으면 등록한다.
				m_list_peer_address.Add(_remote_endpoint);
			}

			// return) 
			return	true;
		}
		public bool						remove_peer_address(IPEndPoint _remote_endpoint)
		{
			lock(m_list_peer_address)
			{
				// 1) 찾는다.
				var	bFind = m_list_peer_address.Remove(_remote_endpoint);

				// check) 존재하지 않으면 false를 리턴한다.
				if(bFind==false)
					return	false;

				// 2) 만약 지우는 것이 현재의 것이라면 현재값을 변경시킨다.
				if(m_list_peer_address.Count<=m_index_peer_address)
				{
					m_index_peer_address = 0;
				}
			}

			// return) 
			return	true;
		}
		public void						remove_all_peer_address()
		{
			lock(m_list_peer_address)
			{
				// 1) list를 제거한다.
				m_list_peer_address.Clear();

				// 2) Peer Address의 Iterator도 Reset한다.
				m_index_peer_address = 0;
			}
		}

		protected class SocketAsyncEventArgs_connect : SocketAsyncEventArgs
		{
			public SocketAsyncEventArgs_connect Clone()
			{
				return (SocketAsyncEventArgs_connect)this.MemberwiseClone();
			}
		}

		private static factory.Cobject<SocketAsyncEventArgs_connect> m_factory_async_args_connect = new factory.Cobject<SocketAsyncEventArgs_connect>("AsyncEventArgsConnect", ()=>
		{
			// 1) Object를 생성해서...
			var	asyncCreated = new SocketAsyncEventArgs_connect();

			// 2) 설정한다.
			asyncCreated.Completed	+= new EventHandler<SocketAsyncEventArgs>(Itcp_client.process_complete_connecting);

			// return) 
			return asyncCreated;
		});

		// - For Reconnection
		private	bool					m_enable_reconnection	 = false;
		private long					m_reconnection_interval	 = 5 * TimeSpan.TicksPerSecond;
		private	long					m_tick_disconnected		 = 0;
		private reconnector		    	m_reconnector			 = null;
		private bool					m_is_reconnecting		 = false;
		private object					m_cs_reconnector		 = new object();

		private	List<IPEndPoint>		m_list_peer_address		 = new List<IPEndPoint>();
		private	int						m_index_peer_address	 = 0;

		private	string					m_name;
		private Cobject_state			m_component_state;

		private class reconnector : schedulable.Cexecutable
		{
			public	bool				attach(Itcp_client _connector)
			{
				// 1) Reconnector의 값을 설정한다.
				m_connector = _connector;
				executable = new executable_delegate(_connector.process_wait_reconnect);

				// 2) Notifier에 붙인다.
				var result = system_executor.register_schedulable(this);

				// check) Executor 붙이기를 실패했을 경우 Reconnector를 Reset한다.
				if(result==false)
				{
					m_connector	 = null;

					// return) 
					return false;
				}

				// return) 
				return true;
			}
			public	void				detach()
			{
				// 1) Notifier를 Reset한다.
				lock(m_cs_connector)
				{
					// - Notifier와 Connector를 Reset한다.
					m_connector = null;
				}

				// 2) Notifier에 떼낸다.
				system_executor.unregister_schedulable(this);
			}

			private Itcp_client m_connector;
			private	object		m_cs_connector = new object();
		}
	}
}