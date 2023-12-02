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
using System.Net;
using System.Net.Sockets;
using CGDK;

//----------------------------------------------------------------------------
//
//  class CGDK.Net.Socket.ITcpClient
//
//    "[Name]/Address"				접속할 주소(주소와 포트 모두 포함)
//	  "[Name]/Port"					접속할 포트
//	  "[Name]/EnableReconnection"	재접속을 활성화한다.
//	  "[Name]/DisableReconnection"	재접속을 비활성화한다.
//	  "[Name]/Reconnection"			재접속 상태를 설정한다.(true/false)
//
//----------------------------------------------------------------------------
namespace CGDK.Net.Socket
{
	public class ITcpClient : 
		ITcp,
		Io.IConnectRequestable,
        CGDK.INameable,
		CGDK.IObjectStateable,
        CGDK.IInitializable,
        CGDK.IStartable
	{
	// constructors) 
		public ITcpClient()
		{
			this.m_component_state = new ObjectState
			{
				Target = this,
				notifyOnInitializing = _ProcessOnInitializing,
				notifyOnInitialize = _ProcessOnInitialize,
				notifyOnDestroying = _ProcessOnDestroying,
				notifyOnDestroy = _ProcessOnDestroy,
				notifyOnStarting = _ProcessOnStarting,
				notifyOnStart = _ProcessOnStart,
				notifyOnStopping = _ProcessOnStopping,
				notifyOnStop = _ProcessOnStop
			};
		}
		public ITcpClient(string _name) : this()
		{
			this.m_name = _name;
		}

	// publics) 
		public string					Name
		{
			get { return this.m_name;}
			set { this.m_name=value;}
		}

		public eOBJECT_STATE			Now
		{
			get
			{
				return this.m_component_state.Now;
			}
			set
			{
				this.m_component_state.Now = value;
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
		public bool						Start(string _address, short _port)
		{
			// declare) 
			IPEndPoint remote_endpoint;
				
			try
			{		
				// 1) IPEndPoint를 구한다.
				remote_endpoint = CGDK.Net.Dns.MakeEndpoint(_address);
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
			return this.ProcessRequestConnecting(remote_endpoint);
		}
		public bool						Start(string _address, string _port)
		{
			// declare) 
			IPEndPoint remote_endpoint;
				
			try
			{		
				// 1) IPEndPoint를 구한다.
				remote_endpoint  = CGDK.Net.Dns.MakeEndpoint(_address);
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
			return this.ProcessRequestConnecting(remote_endpoint);
		}
		public bool						Start(IPEndPoint _remote_endpoint)
		{
			return this.ProcessRequestConnecting(_remote_endpoint);
		}
		public bool						Stop()
		{
			return this.CloseSocket();
		}
		public	bool					Attach(IObjectStateable _child)
		{
			return false;
		}
		public	int						Detach(IObjectStateable _child)
		{
			return 0;
		}
		public IEnumerator				GetEnumerator()
		{
			return m_component_state.GetEnumerator();
		}
		public bool						EnableReconnection
		{
			get
			{
				return this.m_enable_reconnection;
			}
			set
			{
				this.m_enable_reconnection = value;
			}
		}
		public long						ReconnectionInterval
		{
			get
			{
				return this.m_reconnection_interval;
			}
			set
			{
				this.m_reconnection_interval = value;
			}
		}

	// frameworks)
		protected virtual void			OnRequestConnect(IPEndPoint _remote_endpoint)
		{
		}
		protected virtual void			OnRequestReconnect(CGDK.Net.Io.IConnective _connective)
		{
		}

		protected virtual void			OnInitializing(object _object, Context _context)
		{
		}
		protected virtual void			OnInitialize(object _object, Context _context)
		{
		}
		protected virtual void			OnDestroying(object _object)
		{
		}
		protected virtual void			OnDestroy(object _object)
		{
		}

		protected virtual void			OnStarting(object _object, Context _context)
		{
		}
		protected virtual void			OnStart(object _object, Context _context)
		{
		}
		protected virtual void			OnStopping(object _object)
		{
		}
		protected virtual void			OnStop(object _object)
		{
		}


	// implementation)
		protected virtual void			_ProcessOnRequestConnect(IPEndPoint	_remote_endpoint)
		{
			this.OnRequestConnect(_remote_endpoint);
		}
		protected virtual void			_ProcessOnRequestReconnect(CGDK.Net.Io.IConnective _connective)
		{
			this.OnRequestReconnect(_connective);
		}

		protected virtual void			_ProcessOnInitializing(object _object, Context _context)
		{
			this.OnInitializing(_object, _context);
		}
		protected virtual void			_ProcessOnInitialize(object _object, Context _context)
		{
			this.OnInitialize(_object, _context);
		}
		protected virtual void			_ProcessOnDestroying(object _object)
		{
			this.OnDestroying(_object);
		}
		protected virtual void			_ProcessOnDestroy(object _object)
		{
			this.OnDestroy(_object);
		}

		protected virtual void			_ProcessOnStarting(object _object, Context _context)
		{
			// log)
			LOG.PROGRESS(null, "@ <BEGIN> Start CONNECTOR['" + Name + "']");


			//-----------------------------------------------------------------
			// 1. OnStarting 함수 호출
			//-----------------------------------------------------------------
			// 1) 먼저...
			OnStarting(_object, _context);


			//-----------------------------------------------------------------
			// 2. Parameter 읽기
			//"   [Name]/Address"		접속할 주소(주소와 포트 모두 포함)
			//"   [Name]/Port"			접속할 포트
			//"   [Name]/Reconnection"	재접속 상태를 설정한다.(true/false)
			//-----------------------------------------------------------------
			// 1) Context를 얻는다.
			Context	context_now = _context ?? throw new System.Exception("invalid parameter (it's not Context object)");

			// declare) 
			IPEndPoint ipPoint = null;

			// 3) IP Address
			{
				var	param = context_now["Address"];
			
				if(param.IsExist)
				{
					try
					{
						ipPoint	 = CGDK.Net.Dns.MakeEndpoint(param.Value);

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
			
				if(param.IsExist)
				{
					switch(param.Value)
					{
					case	"true":
					case	"Enable":
							{
								this.EnableReconnection = true;

								// Trace)
								LOG.INFO(null, "  + parameter['Reconnection']: 'Enable'");
							}
							break;

					case	"false":
					case	"disable":
							{
								this.EnableReconnection = false;

								// Trace)
								LOG.INFO(null, "  + parameter['Reconnection']: 'disable'");
							}
							break;

					case	"toggle":
							{
								bReconnection = !bReconnection;
								this.EnableReconnection = bReconnection;

								// Trace)
								LOG.INFO(null, "  + parameter['Reconnection']: 'toggle'["+(bReconnection ? "Enable" : "disable")+"]");
							}
							break;

					case	"default":
							{
								this.EnableReconnection = false;

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
			
				if(param.IsExist)
				{
					var	temp_reconnection_interval = Int32.Parse(param.Value);

					if(temp_reconnection_interval > 0)
					{
						// - Port값을 저장한다.
						ReconnectionInterval = temp_reconnection_interval * TimeSpan.TicksPerSecond;

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
			this.ProcessRequestConnecting(ipPoint);
		}
		protected virtual void			_ProcessOnStart(object _object, CGDK.Context _context)
		{
			// 1) 
			this.OnStart(_object, _context);

			// log)
			LOG.PROGRESS(null, "@ <END> Start CONNECTOR['" + Name + "']");
		}
		protected virtual void			_ProcessOnStopping(object _object)
		{
			// log)
			LOG.PROGRESS(null, "@ <BEGIN> Stop CONNECTOR['" + Name + "']");

			// 1) OnStopping
			this.OnStopping(_object);

			// 2) Reconnect를 Disable 한다.
			this.EnableReconnection = false;

			// 3) 기존 Reconnect도 Cancel한다.
			this.CancelReconnector();
		}
		protected virtual void			_ProcessOnStop(object _object)
		{
			// 1) On Stop
			this.OnStop(_object);

			// log)
			LOG.PROGRESS(null, "@ <END> Stop CONNECTOR['" + Name + "']");
		}

		public bool						ProcessRequestConnecting(IPEndPoint _remote_endpoint)
		{
			// 1) _paddrPeer가 nullptr이 아닐 경우 모든 List를 제거하고 _paddrPeer하나만 등록한다.
			if(_remote_endpoint != null)
			{
				// - 모든 PeerAddress를 제거한다.
				this.RemoveAllPeerAddress();

				// - 새로 들어온 PeerAddress를 추가한다.
				this.AddPeerAddress(_remote_endpoint);
			}

			// check)
			if(_remote_endpoint == null)
				return false;

			// declare) 
			IPEndPoint address_connect;

			// check) Pointer가 nullptr이라면 하나도 추가가 되지 않은 것이다.
			if(this.m_list_peer_address.Count == 0)
				return false;

			// 2) Address를 하나 읽어 접속을 시도한다.
			lock(this.m_list_peer_address)
			{
				// - Index
				this.m_index_peer_address = (this.m_index_peer_address++) % this.m_list_peer_address.Count;

				// - 현재 Address를 얻고 다음 Address로 변경한다.
				address_connect = this.m_list_peer_address[this.m_index_peer_address];
			}
	
			// 3) 접속 시도시 m_tick_disconnected 시간을 갱신한다.
			this.m_tick_disconnected = System.DateTime.Now.Ticks;
	
			// 4) Reconning을 false로 한다.
			this.m_is_reconnecting = false;


			// * 상태변경
			//
			// - 상태변경을 하면서 이미 Connect상태이거나 Connect를 시도한 상태인지
			//   를 구분하여 만약 이미 Connect를 시도한 상태이거나 Connect상태이면
			//   바로 Return할수 있도록 처리를 해주어야 한다.
			// - 이를 위해 atomic를 사용하여 SocketState를 변경하여 그 이전
			//   값을 Check하여 처리한다.
			// 

			// 5) Socket상태를 eSOCKET_STATE.SYN으로 바꾼다.
			//    - 내부적으로 atomic함수를 통해 CLOSED 상태로 변경한다.
			//    - 여기서 atomic함수를 사용하는 이유는 상황에 따라 중복적으로 
			//      Connect와 Disconnect등 Socket의  상태가 불려지는 함수가 겹쳐
			//      서 호출될수 있기 때문이다. 
			//    - Socket의 상태가 CLOSED일 때만 Connect함수가 수행되고 아닐 경우 
			//      더 이상 진행하지 않고 false를 리턴한다.
			var	pre_socket_status = Socket.SetSocketStateIf(eSOCKET_STATE.CLOSED, eSOCKET_STATE.SYN);

			// check) 이전 상태가 CLOSED가 아니면 안된다.
			if(pre_socket_status != eSOCKET_STATE.CLOSED)
			{
				return false;
			}

			// 6) Socket을 준비한다.
			this.ProcessPrepareSocket(_remote_endpoint.AddressFamily);

      		// 7) OnRequestConnect를 호출한다.
			this._ProcessOnRequestConnect(address_connect);

			// 8) Async Event 객체를 생성한다.
			SocketAsyncEventArgs_connect async_event = m_factory_async_args_connect.Alloc();

			// 9) Address를 설정한다.
			async_event.SetBuffer(null, 0, 0);
			async_event.BufferList = null;
			async_event.RemoteEndPoint = address_connect;
			async_event.SocketFlags = SocketFlags.None;
			async_event.AcceptSocket = null;
			async_event.UserToken = this;

			// declare)
			bool result;

			// Reference Counting) 
			this.AddReference();

			// 10) 접속을 시도한다. Connect to the remote endpoint.
			result = this.Socket.ConnectAsync(async_event);

			// check) ConnectAsync함수의 성공여부를 확인다.
			if(result == false)
			{
				// - Socket을 닫는다.
				this.ProcessCloseSocket();

				// log) 
				LOG.ERROR(null, "(Err ) CONNECTOR['" + Name + "']: Fail to connecting Socket [Error:"+async_event.SocketError+"] (CGDK.Net.Socket.ITcpClient.ProcessRequestConnecting)");

				// - 접속종료상태로 변경.
				this.Socket.SocketState = eSOCKET_STATE.CLOSED;

				// - _args 초기화 및 할당해제
				async_event.UserToken = null;
				async_event.RemoteEndPoint = null;
				m_factory_async_args_connect.Free(async_event);

				// Reference Counting) 
				this.Release();

				// return) 실패를 return한다.
				return false;
			}

			// Statistics) 접속 시도 횟수
			this.Statistics.statistics_on_connect_try();

			// log) 걸어놓은 Accpet갯수를 출력한다.
			LOG.INFO_LOW(null, "  > trying to connect '"+address_connect.ToString()+"'");

			// return) 접속시도에 성공했다!
			return true;
		}
		protected static void			ProcessCompleteConnecting(object _source, SocketAsyncEventArgs _args)
		{
			// 1) Socket을 얻는다.
			var psocket_tcp = _args.UserToken as ITcpClient;

			// declare) 
			bool result = false;

			try
			{
				// 2) ProcessCompleteConnect함수를 호출한다.
				result = psocket_tcp.ProcessCompleteConnect(_args);
			}
			catch(System.Exception )
			{
			}

			// 3) _args 초기화 및 할당해제
			_args.RemoteEndPoint = null;
			_args.UserToken = null;
			m_factory_async_args_connect.Free(_args as SocketAsyncEventArgs_connect);

			// 4) Connection에 실패했을 경우 다시 Reconnect를 요청한다.
			if(result == false)
			{
				psocket_tcp.RequestReconnect();
			}
			else
			{
				// - 접속에 성공했으므로 재접속처리를 종료한다.
				psocket_tcp.CancelReconnector();
			}

			// 6) Socket을 Release한다.
			psocket_tcp.Release();
		}
		protected override void			ProcessDisconnect()
		{
			// 1) Connective의 ProcessConnectiveDisconnect를 호출한다.
			Connective?.ProcessConnectiveDisconnect(this);

			// 2) 재접속처리
			ProcessDisconnectReconnection();

			// Statistics) 
			Io.Statistics.Nconnective.total.StatisticsOnDisconnect();
		}
		public void						ProcessWaitReconnect(object _object, object _param)
		{
			lock(this.m_cs_reconnector)
			{
				// check) Connection상태면 더이상 진행하지 않는다.
				if(m_is_reconnecting == false)
					return;

				// check) 만약에 Disable상태라면 재접속을 하지 않는다.
				if(EnableReconnection == false)
				{
					// 1) Reconnector를 저장해 놓는다.
					Reconnector	Reconnector = m_reconnector;

					// check) Reconnector가 Empty인지 확인한다.
					if(Reconnector != null)
					{
						// 2) Reconnector를 Reset한다.
						m_reconnector = null;

						// Critical) Criticalsection Leave!!
						//LOCK_LEAVE(cs)

						// 3) Reconnector를 Detach한다.
						Reconnector.Detach();
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
				LOG.INFO_LOW(null, "(Prgr) CGNetSocket['" + Name + "']: Trying to reconnection (CGDK.Net.Socket.ITcpClient.ProcessWaitReconnect)");

				// 4) OnRequestReconnect함수를 호출한다.
				_ProcessOnRequestReconnect(null);
			}

			// 5) 기존 Socket을 닫는다.
			CloseSocket();

			// 6) 다시 접속한다.(재접속을 위해서는 nullptr을 넘겨야 한다.)
			ProcessRequestConnecting(null);
		}
		private void					ProcessDisconnectReconnection()
		{
			lock(this.m_cs_reconnector)
			{
				// 1) Connective를 Reset한다.
				Connective = null;

				// 2) 접속 종료시 m_tick_disconnect시간을 설정한다.
				var	tick_now = System.DateTime.Now.Ticks;
				var	tick_differ_execute = tick_now - this.m_tick_disconnected;

				// check) 마지막 접속시도 시간보다 m_dwReconnectInterval이  
				//        Case 지나지 않았으면 -> 마지막 접속시간+재접속시도간격
				//        Case 지났으면       -> 현재시간+재접속시도간격
				if(tick_differ_execute < this.m_reconnection_interval)
				{
					this.m_tick_disconnected += this.m_reconnection_interval;
				}
				else
				{
					this.m_tick_disconnected = tick_now + this.m_reconnection_interval;
				}
			}

			// 3) Reconnect를 진행한다.
			RequestReconnect();
		}

		private void					RequestReconnect()
		{
			// check) Reconnection이 Disable되었을 경우 그냥 끝낸다.
			if(this.m_enable_reconnection == false)
			{
				// - Reconnect를 Cancel한다.
				this.CancelReconnector();

				// return) 
				return;
			}

			lock(this.m_cs_reconnector)
			{
				// 1) Reconnection을 시작했음으로 설정한다.
				this.m_is_reconnecting = true;

				// check) 이미 Reconnect가 걸린 경우 그냥 끝낸다.
				if(this.m_reconnector != null)
					return;

				// 2) Reconnector를 설정한다.
				this.m_reconnector = new Reconnector();

				// 3) Attach한다.
				if(this.m_reconnector.Attach(this)==false)
				{
					this.m_reconnector = null;
					this.m_is_reconnecting = false;
				}
			}
		}
		private void					CancelReconnector()
		{
			// declare) 
			Reconnector Reconnector;

			lock(this.m_cs_reconnector)
			{
				// 1) Notifier를 얻는다.
				Reconnector = this.m_reconnector;

				// check) Reconnector가 Empty인지 확인한다.
				if(Reconnector == null)
					return;

				// 2) Reconnection을 false로...
				this.m_is_reconnecting = false;

				// 3) Reconnector를 Reset한다.
				this.m_reconnector = null;
			}

			// 4) Detach한다.
			Reconnector.Detach();
		}

		public bool						AddPeerAddress(IPEndPoint _remote_endpoint)
		{
			lock(this.m_list_peer_address)
			{
				// check) 먼저 등록된 것이 있는지 찾는다.
				if(this.m_list_peer_address.FindIndex(x => x==_remote_endpoint)>=0)
					return	false;

				// 1) 등록된 것이 없으면 등록한다.
				this.m_list_peer_address.Add(_remote_endpoint);
			}

			// return) 
			return true;
		}
		public bool						RemovePeerAddress(IPEndPoint _remote_endpoint)
		{
			lock(this.m_list_peer_address)
			{
				// 1) 찾는다.
				var	bFind = this.m_list_peer_address.Remove(_remote_endpoint);

				// check) 존재하지 않으면 false를 리턴한다.
				if(bFind==false)
					return	false;

				// 2) 만약 지우는 것이 현재의 것이라면 현재값을 변경시킨다.
				if(this.m_list_peer_address.Count <= this.m_index_peer_address)
				{
					this.m_index_peer_address = 0;
				}
			}

			// return) 
			return	true;
		}
		public void						RemoveAllPeerAddress()
		{
			lock(this.m_list_peer_address)
			{
				// 1) list를 제거한다.
				this.m_list_peer_address.Clear();

				// 2) Peer Address의 Iterator도 Reset한다.
				this.m_index_peer_address = 0;
			}
		}

		protected class SocketAsyncEventArgs_connect : SocketAsyncEventArgs
		{
			public SocketAsyncEventArgs_connect Clone()
			{
				return (SocketAsyncEventArgs_connect)this.MemberwiseClone();
			}
		}

		private static Factory.Object<SocketAsyncEventArgs_connect> m_factory_async_args_connect = new("AsyncEventArgsConnect", ()=>
		{
			// 1) Object를 생성해서...
			var	asyncCreated = new SocketAsyncEventArgs_connect();

			// 2) 설정한다.
			asyncCreated.Completed	+= new EventHandler<SocketAsyncEventArgs>(ITcpClient.ProcessCompleteConnecting);

			// return) 
			return asyncCreated;
		});

		// - For Reconnection
		private	bool					m_enable_reconnection = false;
		private long					m_reconnection_interval = 5 * TimeSpan.TicksPerSecond;
		private	long					m_tick_disconnected = 0;
		private Reconnector		    	m_reconnector = null;
		private bool					m_is_reconnecting = false;
		private object					m_cs_reconnector = new();

		private	List<IPEndPoint>		m_list_peer_address = new();
		private	int						m_index_peer_address = 0;

		private	string					m_name;
		private ObjectState				m_component_state;

		private class Reconnector : Schedulable.Executable
		{
			public	bool				Attach(ITcpClient _connector)
			{
				// 1) Reconnector의 값을 설정한다.
				this.m_connector = _connector;
				Target = new ExecutableDelegate(_connector.ProcessWaitReconnect);

				// 2) Notifier에 붙인다.
				var result = SystemExecutor.RegisterSchedulable(this);

				// check) Executor 붙이기를 실패했을 경우 Reconnector를 Reset한다.
				if(result==false)
				{
					this.m_connector = null;

					// return) 
					return false;
				}

				// return) 
				return true;
			}
			public	void				Detach()
			{
				// 1) Notifier를 Reset한다.
				lock(this.m_cs_connector)
				{
					// - Notifier와 Connector를 Reset한다.
					this.m_connector = null;
				}

				// 2) Notifier에 떼낸다.
				SystemExecutor.UnregisterSchedulable(this);
			}

			private ITcpClient m_connector;
			private	object		m_cs_connector = new();
		}
	}
}