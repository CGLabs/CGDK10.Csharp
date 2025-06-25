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
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using CGDK;
using CGDK.Factory;
using CGDK.Net.Io;


//----------------------------------------------------------------------------
//
//  class CGDK.Net.Socket.IUdp
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK.Net.Socket
{
	public class IUdp :
        Nreferenceable,
		CGDK.INameable,
		CGDK.IObjectStateable,
		CGDK.IInitializable,
		CGDK.IStartable,
		CGDK.IMessageable, 
		Io.ISenderDatagram,
        IDisposable
	{
	// constructors) 
		public IUdp()
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
		public IUdp(string _name) : this()
		{
			m_name = _name;
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
		public bool						SendTo(CGDK.buffer _buffer, IPEndPoint _to)
		{
			return this.ProcessRequestSendTo(_buffer, 1, _to);
		}
		public Io.Statistics.Ntraffic	Statistics
		{
			get { return m_statistics_traffic; }
		}
		public bool						CloseSocket(ulong _disconnect_reason = DISCONNECT_REASON.NONE)
		{
			// check) 
			Debug.Assert((_disconnect_reason & DISCONNECT_REASON.MASK) == 0);

			// 1) CloseSocket
			return ProcessCloseSocket(_disconnect_reason);
		}
		public Io.NSocket				Socket
		{
			get { return m_socket;}
		}

		public int						MinimumMesageBufferSize	{ get { return m_minimum_mesage_buffer_size; } set { m_minimum_mesage_buffer_size = value; } }
		public int						MaximumMesageBufferSize	{ get { return m_maximum_mesage_buffer_size; } set { m_maximum_mesage_buffer_size = value; } }
		public int						MaximumMesageBufferCount { get { return m_maximum_mesage_buffer_count; } set { m_maximum_mesage_buffer_count = value; } }

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
			return m_component_state.Start(_context);
		}
		public bool						Start()
		{
			// 1) create Context object
			Context context_setting = new();

			// 2) Start
			return this.Start(context_setting);
		}
		public bool						Start(IPEndPoint _remote_endpoint)
		{
			// 1) create Context object
			Context context_setting = new();

			// 2) set parameter
			context_setting["bind_endpoint"]["ip_endpoint"] = _remote_endpoint;

			// 3) Start
			return this.Start(context_setting);
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
			return this.m_component_state.GetEnumerator();
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

		protected virtual void			OnReceive(CGDK.buffer _buffer_received, SocketAsyncEventArgs _args) {}
		protected virtual int			OnMessage(object _source, sMESSAGE _msg) { return 0;}
		protected override void			OnFinalRelease()
		{
			// check) 
			Debug.Assert(this.ReferenceCount == 0);

			// check) 
			Debug.Assert(this.m_socket.SocketHandle == null);
		}

	// implementation)
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

			// 1) call 'OnStarting'
			this.OnStarting(_object, _context);

			// 2) read parameter
			Context	context_now = _context ?? throw new System.Exception("invalid parameter (it's not Context object)");

			// declare) 
			IPEndPoint ip_endpoint_bind = new(0,0);

			// 3) get parameter - 'bind address'
			{
				// - get bind_endpoint
				var context_bind_endpoint = context_now["bind_endpoint"];

				// - get bind_endpoint
				if(context_bind_endpoint.IsExist)
				{
					// - address
					{
						var	param = context_bind_endpoint["address"];
			
						if(param.IsExist)
						{
							try
							{
								ip_endpoint_bind = CGDK.Net.Dns.MakeEndpoint(param.Value);

								// log)
								LOG.INFO(null, "  + parameter['address']: '" + ip_endpoint_bind.ToString() + "'");
							}
							catch(FormatException _e)
							{
								// log) 
								LOG.ERROR(null, "  ! " + _e.ToString());
							}
							catch(System.Exception _e)
							{
								// log) 
								LOG.ERROR(null, "  ! " + _e.ToString());
							}
						}
					}

					// - port
					{
						var	param = context_bind_endpoint["port"];
			
						if(param.IsExist)
						{
							try
							{
								// - get port
								int port = param;

								// log)
								LOG.INFO(null, "  + parameter['port']: '" + port + "'");

								// check)
								if(port < 0 || port > 65536)
									throw new System.Exception("invalid port number! must be 'port over 0' and 'port under 65536'  (port: " + port + ")" );

								// - set port
								ip_endpoint_bind.Port = port;

							}
							catch(FormatException _e)
							{
								// log) 
								LOG.ERROR(null, "  ! " + _e.ToString());
							}
							catch(System.Exception _e)
							{
								// log) 
								LOG.ERROR(null, "  ! " + _e.ToString());
							}
						}
					}

					// - ip_endpoint_bind
					{
						var	param = context_bind_endpoint["ip_endpoint"];
			
						if(param.IsExist)
						{
							try
							{
								// - get ip_endpoint
								ip_endpoint_bind = param;

								// log)
								LOG.INFO(null, "  + parameter['address']: '" + ip_endpoint_bind.Address.ToString() + "'");
								LOG.INFO(null, "  + parameter['port']: '" + ip_endpoint_bind.Port.ToString() + "'");

								// check)
								if(ip_endpoint_bind.Port < 0 || ip_endpoint_bind.Port > 65536)
									throw new System.Exception("invalid port number! must be 'port over 0' and 'port under 65536'  (port: " + ip_endpoint_bind.Port + ")" );
							}
							catch(FormatException _e)
							{
								// log) 
								LOG.ERROR(null, "  ! " + _e.ToString());
							}
							catch(System.Exception _e)
							{
								// log) 
								LOG.ERROR(null, "  ! " + _e.ToString());
							}
						}
					}
				}
			}

			// 4) prepare Socket
			this.ProcessPrepareSocket(ip_endpoint_bind.AddressFamily);

			// 5) bind
			this.m_socket.RequestBind(ip_endpoint_bind);

			// 6) prepare receive
			this.PrepareReceive();
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
		}
		protected virtual void			_ProcessOnStop(object _object)
		{
			// 1) On Stop
			this.OnStop(_object);

			// log)
			LOG.PROGRESS(null, "@ <END> Stop CONNECTOR['" + Name + "']");
		}

		protected void					ProcessPrepareSocket(AddressFamily _address_family)
		{
			// 1) set Socket state as eSOCKET_STATE.ESTABLISHED only if Socket state is eSOCKET_STATE.CLOSED
			var	pre_socket_status = Socket.SetSocketStateIf(eSOCKET_STATE.CLOSED, eSOCKET_STATE.ESTABLISHED);

			// check) 
			if(pre_socket_status != eSOCKET_STATE.CLOSED)
				return;

			// 2) create Socket handle
			this.m_socket.CreateSocketHandle(_address_family, SocketType.Dgram, ProtocolType.Udp);

			// 3) set Socket options
			this.m_socket.SocketHandle.Blocking = false;
			this.m_socket.SetReceiveBufferSize(DEFAULT_UDP_RECEIVE_BUFFER_SIZE);
			this.m_socket.SetSendBufferSize(DEFAULT_UDP_SEND_BUFFER_SIZE);
		}
		protected bool					ProcessCloseSocket(ulong _disconnect_reason = DISCONNECT_REASON.NONE)
		{
			return this.m_socket.CloseSocketHandle();
		}

		public bool						ProcessRequestSendTo(CGDK.buffer _buffer, int _count_message, IPEndPoint _remote_endpoint)
		{
			// check)
			Debug.Assert(_buffer.Array != null);
			Debug.Assert((_buffer.Offset + _buffer.Count) <= _buffer.Capacity);
			Debug.Assert(_buffer.Count > 0);

			// check)
			if(_buffer.Count <= 0)
				return false;

			// 1) add ref 
			this.AddReference();

			// 2) Alloc buffer
			CGDK.buffer	buf_send = Memory.AllocBuffer(_buffer.Count);

			// 3) append buf
			buf_send.Append(_buffer.Array, _buffer.Offset, _buffer.Count);

			// 4) Alloc args object
			SocketAsyncEventArgs_send args_send = m_factroy_async_args_send.Alloc();

			args_send.BufferList = null;
			args_send.SetBuffer(buf_send.Array, buf_send.Offset, buf_send.Count);
			args_send.SocketFlags = SocketFlags.None;
			args_send.SocketError = SocketError.SocketError;
			args_send.RemoteEndPoint = _remote_endpoint;
			//args_send.SendPacketsFlags = TransmitFileOptions.UseDefaultWorkerThread;
			args_send.UserToken = this;

			// declare) 
			bool result_return = false;

			// 5) Send to
			var result = this.m_socket.SendToAsync(args_send);

			// 6) is succeded
			if(args_send.SocketError == SocketError.Success)
			{
				// Statistics) 
				this.Statistics.statistics_on_send_async();

				// - set as result
				result_return = true;
			}

			// check) 
			if (result)
				return true;

			// 7) call 'ProcessCompleteSend'
			this.ProcessCompleteSend(args_send);

			// 8) Release ref
			this.Release();

			// return) 
			return result_return;
		}
		private static void				CompleteProcessSend(object _source, SocketAsyncEventArgs _args)
		{
			// declare) 
			var	args_send = _args as SocketAsyncEventArgs_send;
			var socket_udp = _args.UserToken as IUdp;

			// check) 
			Debug.Assert(_args.UserToken != null);
			Debug.Assert(socket_udp != null);

			// 1) call 'ProcessCompleteSend'
			socket_udp.ProcessCompleteSend(args_send);

			// 2) Release ref
			socket_udp.Release();
		}
		protected virtual void			ProcessCompleteSend(SocketAsyncEventArgs_send _args)
		{
			// 1) Clear '_args'
			_args.Clear();

			// 2) args를 Free한다.
			m_factroy_async_args_send.Free(_args);
		}

		private void					PrepareReceive()
		{
			// check)
			if(this.m_socket.SocketState != eSOCKET_STATE.ESTABLISHED)
				return;

			// 1) get overlapped_receive
			var overlapped_receiving = Statistics.overlapped_receiving;

			// check) 
			if(this.m_maximum_mesage_buffer_count <= overlapped_receiving)
				return;

			// 1) get count
			var count_receive = this.m_maximum_mesage_buffer_count - overlapped_receiving;

			// 2) prepare receive
			while(count_receive != 0)
			{
				// - Alloc EventArgs object
				SocketAsyncEventArgs_recv args_receive = m_factory_async_args_receive.Alloc();

				// referece counting)
				this.AddReference();

				// - Alloc buffer
				var buf_recv = Factory.Memory.AllocBuffer(m_maximum_mesage_buffer_size);

				// - set receive parameter
				args_receive.UserToken = this;
				args_receive.BufferList = null;
				args_receive.SocketFlags = SocketFlags.None;
				args_receive.SocketError = SocketError.Success;
				args_receive.SetBuffer(buf_recv.Array, buf_recv.Offset, m_maximum_mesage_buffer_size);   // Buffer, Offset, Count

				// Statistics) 
				this.Statistics.statistics_on_increase_async_receiveing();

				// - call 'ReceiveFromAsync'
				var result = m_socket.ReceiveFromAsync(args_receive);

				// check) 
				if(result == false)
				{
					// Statistics) 
					this.Statistics.statistics_on_decrease_async_receiveing();

					// - 
					if(args_receive.SocketError == SocketError.Success)
					{
						// Statistics)
						this.Statistics.statistics_on_receive_bytes(args_receive.BytesTransferred);

						try
						{
							// - process receive
							this.ProcessReceive(args_receive);
						}
						catch(System.Exception)
						{
						}
					}

					// - Clear args_receive
					args_receive.Clear();

					// - Free args_receive
					m_factory_async_args_receive.Free(args_receive);

					// referece counting)
					this.Release();
				}
				else
				{
					--count_receive;
				}

				// Statistics) 
				this.Statistics.statistics_on_receive_async();
			}
		}
		private static void				CompleteProcessReceive(object _source, SocketAsyncEventArgs _args)
		{
			// declare) 
			var	args_receive = _args as SocketAsyncEventArgs_recv;
			var socket_udp = _args.UserToken as IUdp;

			// check) 
			Debug.Assert(_args.UserToken != null);
			Debug.Assert(socket_udp != null);

			// 1) call 'ProcessCompleteReceive'
			socket_udp.ProcessCompleteReceive(args_receive);

			// 2) Clear 'args_receive'
			args_receive.Clear();

			// 3) Free args_receive
			m_factory_async_args_receive.Free(args_receive);

			// Reference Counting) 
			socket_udp.Release();
		}
		protected virtual void			ProcessCompleteReceive(SocketAsyncEventArgs_recv _args)
		{
			// Statistics)
			this.Statistics.statistics_on_receive_bytes(_args.BytesTransferred);

			// Statistics) 
			this.Statistics.statistics_on_decrease_async_receiveing();

			// check) 
			if(_args.SocketError != SocketError.Success)
				return;

			try
			{
				this.ProcessReceive(_args);
			}
			catch(System.Exception)
			{
				// return)
				return;
			}
        
			// 6) receive
			this.PrepareReceive();
		}
		protected void					ProcessReceive(SocketAsyncEventArgs_recv _args)
		{
			// declare) 
			var	buf_message = new CGDK.buffer(_args.Buffer, _args.Offset, _args.BytesTransferred);

			// 1) call 'OnReceive'
			this.OnReceive(buf_message, _args);

			// 2) call 'ProcessPacket'
			this.ProcessPacket(ref buf_message, _args.RemoteEndPoint as IPEndPoint);
		}
		protected virtual int			ProcessPacket(ref CGDK.buffer _buffer, IPEndPoint _address)
		{
			// declare)
			sMESSAGE msg = new(eMESSAGE.SYSTEM.MESSAGE, this, _buffer, _address);

			// Statistics) 
			this.Statistics.statistics_on_receive_message(1);

			// 1) call 'ProcessMessage'
			this.ProcessMessage(this, msg);

			// return) 
			return 0;
		}
		public int						ProcessMessage(object _source, sMESSAGE _msg)
		{
			return this.OnMessage(_source, _msg);
		}

		public void						Dispose()
		{
			// 1) dispose
			this.Dispose(true);

			// 2) GC
			GC.SuppressFinalize(this);
		}
		protected virtual void			Dispose(bool _is_dispose)
		{
			// check)
			if (this.m_is_disposed)
				return;

			// 1) is disonsed
			if (_is_dispose) 
			{
				// check) 
				Debug.Assert(ReferenceCount==0);

				// - dispose Socket
				this.m_socket.Dispose();
			}

			// 2) set disposed
			this.m_is_disposed = true;
		}

		protected class SocketAsyncEventArgs_send : SocketAsyncEventArgs
		{
			public	SocketAsyncEventArgs_send()
			{
				this.Completed += new (IUdp.CompleteProcessSend);
			}

			public void Clear()
			{
				// 1) Free buffer
				if(Buffer != null)
				{
					// - Free buffer
					Memory.Free(Buffer);

					// - Reset buffer
					this.SetBuffer(null, 0, 0);
				}

				// 2) Reset UserToken
				UserToken = null;

				// check) 
				Debug.Assert(BufferList == null);
				Debug.Assert(AcceptSocket == null);
			}
		}
		protected class SocketAsyncEventArgs_recv : SocketAsyncEventArgs
		{
			public	SocketAsyncEventArgs_recv()
			{
				this.Completed += new (IUdp.CompleteProcessReceive);
				this.RemoteEndPoint = new IPEndPoint(IPAddress.IPv6Any, 0);
			}

			public void Clear()
			{
				// 1) Free array
				if(this.Buffer != null)
				{
					// - Free buffer
					Memory.Free(this.Buffer);

					// - Reset buffer
					this.SetBuffer(null, 0, 0);
				}

				// 2) Reset UserToken
				this.UserToken = null;

				// check) 
				Debug.Assert(this.BufferList == null);
				Debug.Assert(this.AcceptSocket == null);
			}
		}

	//  definitions)
		public const int				DEFAULT_UDP_SEND_BUFFER_SIZE			 = (   8 * 1024 * 1024);
		public const int				DEFAULT_UDP_RECEIVE_BUFFER_SIZE			 = (   8 * 1024 * 1024);
		public const int				DEFAULT_UDP_MESSAGE_BUFFER_SIZE			 = (         64 * 1024);
		public const int				DEFAULT_UDP_MESSAGE_BUFFER_COUNT		 = 32;
		public const int				MIN_UDP_MESSAGE_SIZE					 = 4;
		public const int				MAX_UDP_MESSAGE_SIZE					 = (         64 * 1024);

	// implementations)
		// 1) tunning parameters
		public static int				DefaultMinimumMesageBufferSize			 = DEFAULT_UDP_MESSAGE_BUFFER_SIZE;
		public static int				DefaultMaximumMesageBufferSize			 = DEFAULT_UDP_MESSAGE_BUFFER_SIZE;
		public static int				DefaultMaximumMessageBufferCount		 = DEFAULT_UDP_MESSAGE_BUFFER_COUNT;

		// 2) Socket status
		private Io.NSocket				m_socket								 = new();

		// 3) buffers
		private int						m_minimum_mesage_buffer_size			 = DefaultMinimumMesageBufferSize;
		private int						m_maximum_mesage_buffer_size			 = DefaultMaximumMesageBufferSize;
		private int						m_maximum_mesage_buffer_count			 = DefaultMaximumMessageBufferCount;

		// 4) ...
		private static Factory.Object<SocketAsyncEventArgs_send>    m_factroy_async_args_send = new("AsyncEventArgsSend");
		private static Factory.Object<SocketAsyncEventArgs_recv>	m_factory_async_args_receive = new("AsyncEventArgsReceive");

		// 5) Name & object state
		private	string					m_name;
		private ObjectState				m_component_state;

		// 6) Statistics
		private Io.Statistics.Ntraffic	m_statistics_traffic		 = new();

		// 7) 
		private	bool					m_is_disposed				 = false;
	}
}
