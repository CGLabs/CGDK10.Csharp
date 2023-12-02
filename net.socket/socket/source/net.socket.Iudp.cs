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
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using CGDK;
using CGDK.factory;
using CGDK.net.io;


//----------------------------------------------------------------------------
//
//  class CGDK.net.socket.Iudp
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK.net.socket
{
	public class Iudp :
        Nreferenceable,
		CGDK.Inameable,
		CGDK.Iobject_stateable,
		CGDK.Istartable,
		CGDK.Iinitializable,
		CGDK.Imessageable, 
		io.Isender_datagram,
        IDisposable
	{
	// constructors) 
		public Iudp()
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
		public Iudp(string _name) : this()
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
		public bool						send_to(CGDK.buffer _buffer, IPEndPoint _to)
		{
			return	process_request_send_to(_buffer, 1, _to);
		}
		public io.statistics.Ntraffic	statistics
		{
			get { return m_statistics_traffic; }
		}
		public bool						closesocket(ulong _disconnect_reason = DISCONNECT_REASON.NONE)
		{
			// check) 
			Debug.Assert((_disconnect_reason & DISCONNECT_REASON.MASK) == 0);

			// 1) closesocket
			return process_closesocket(_disconnect_reason);
		}
		public io.NSocket				socket
		{
			get { return m_socket;}
		}

		public int						minimum_mesage_buffer_size	{ get { return m_minimum_mesage_buffer_size; } set { m_minimum_mesage_buffer_size = value; } }
		public int						maximum_mesage_buffer_size	{ get { return m_maximum_mesage_buffer_size; } set { m_maximum_mesage_buffer_size = value; } }
		public int						maximum_mesage_buffer_count	{ get { return m_maximum_mesage_buffer_count; } set { m_maximum_mesage_buffer_count = value; } }

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
		public bool						start()
		{
			// 1) create context object
			context context_setting = new context();

			// 2) start
			return start(context_setting);
		}
		public bool						start(IPEndPoint _remote_endpoint)
		{
			// 1) create context object
			context context_setting = new context();

			// 2) set parameter
			context_setting["bind_endpoint"]["ip_endpoint"] = _remote_endpoint;

			// 3) start
			return start(context_setting);
		}
		public bool						stop()
		{
			return closesocket();
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

		protected virtual void			on_receive(CGDK.buffer _buffer_received, SocketAsyncEventArgs _args) {}
		protected virtual int			on_message(object _source, sMESSAGE _msg) { return 0;}
		protected override void			on_final_release()
		{
			// check) 
			Debug.Assert(reference_count == 0);

			// check) 
			Debug.Assert(m_socket.socket_handle == null);
		}

	// implementation)
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

			// 1) call 'on_starting'
			on_starting(_object, _context);

			// 2) read parameter
			context	context_now = _context;

			// check)
			if(context_now == null)
				throw new System.Exception("invalid parameter (it's not context object)");

			// declare) 
			IPEndPoint ip_endpoint_bind = new IPEndPoint(0,0);

			// 3) get parameter - 'bind address'
			{
				// - get bind_endpoint
				var context_bind_endpoint = context_now["bind_endpoint"];

				// - get bind_endpoint
				if(context_bind_endpoint.exist)
				{
					// - address
					{
						var	param = context_bind_endpoint["address"];
			
						if(param.exist)
						{
							try
							{
								ip_endpoint_bind = CGDK.net.dns.make_endpoint(param.value);

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
			
						if(param.exist)
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
			
						if(param.exist)
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

			// 4) prepare socket
			process_prepare_socket(ip_endpoint_bind.AddressFamily);

			// 5) bind
			m_socket.request_bind(ip_endpoint_bind);

			// 6) prepare receive
			prepare_receive();
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
		}
		protected virtual void			_process_on_stop(object _object)
		{
			// 1) On stop
			on_stop(_object);

			// log)
			LOG.PROGRESS(null, "@ <END> stop CONNECTOR['" + name + "']");
		}

		protected void					process_prepare_socket(AddressFamily _address_family)
		{
			// 1) set socket state as eSOCKET_STATE.ESTABLISHED only if socket state is eSOCKET_STATE.CLOSED
			var	pre_socket_status = socket.set_socket_state_if(eSOCKET_STATE.CLOSED, eSOCKET_STATE.ESTABLISHED);

			// check) 
			if(pre_socket_status != eSOCKET_STATE.CLOSED)
				return;

			// 2) create socket handle
			m_socket.create_socket_handle(_address_family, SocketType.Dgram, ProtocolType.Udp);

			// 3) set socket options
			m_socket.socket_handle.Blocking = false;
			m_socket.set_receive_buffer_size(DEFAULT_UDP_RECEIVE_BUFFER_SIZE);
			m_socket.set_send_buffer_size(DEFAULT_UDP_SEND_BUFFER_SIZE);
		}
		protected bool					process_closesocket(ulong _disconnect_reason = DISCONNECT_REASON.NONE)
		{
			return m_socket.close_socket_handle();
		}

		public bool						process_request_send_to(CGDK.buffer _buffer, int _count_message, IPEndPoint _remote_endpoint)
		{
			// check)
			Debug.Assert(_buffer.Array != null);
			Debug.Assert((_buffer.Offset + _buffer.Count) <= _buffer.capacity);
			Debug.Assert(_buffer.Count > 0);

			// check)
			if(_buffer.Count <= 0)
				return false;

			// 1) add ref 
			add_ref();

			// 2) alloc buffer
			CGDK.buffer	buf_send = memory.alloc_buffer(_buffer.Count);

			// 3) append buf
			buf_send.append(_buffer.Array, _buffer.Offset, _buffer.Count);

			// 4) alloc args object
			SocketAsyncEventArgs_send args_send = m_factroy_async_args_send.alloc();

			args_send.BufferList = null;
			args_send.SetBuffer(buf_send.Array, buf_send.Offset, buf_send.Count);
			args_send.SocketFlags = SocketFlags.None;
			args_send.SocketError = SocketError.SocketError;
			args_send.RemoteEndPoint = _remote_endpoint;
			//args_send.SendPacketsFlags = TransmitFileOptions.UseDefaultWorkerThread;
			args_send.UserToken = this;

			// declare) 
			bool result_return = false;

			// 5) send to
			var result = m_socket.sendto_async(args_send);

			// 6) is succeded
			if(args_send.SocketError == SocketError.Success)
			{
				// statistics) 
				statistics.statistics_on_send_async();

				// - set as result
				result_return = true;
			}

			// check) 
			if (result)
				return true;

			// 7) call 'process_complete_send'
			process_complete_send(args_send);

			// 8) release ref
			release();

			// return) 
			return result_return;
		}
		private static void				complete_process_send(object _source, SocketAsyncEventArgs _args)
		{
			// declare) 
			var	args_send = _args as SocketAsyncEventArgs_send;
			var socket_udp = _args.UserToken as Iudp;

			// check) 
			Debug.Assert(_args.UserToken != null);
			Debug.Assert(socket_udp != null);

			// 1) call 'process_complete_send'
			socket_udp.process_complete_send(args_send);

			// 2) release ref
			socket_udp.release();
		}
		protected virtual void			process_complete_send(SocketAsyncEventArgs_send _args)
		{
			// 1) clear '_args'
			_args.clear();

			// 2) args를 Free한다.
			m_factroy_async_args_send.free(_args);
		}

		private void					prepare_receive()
		{
			// check)
			if(m_socket.socket_state != eSOCKET_STATE.ESTABLISHED)
				return;

			// 1) get overlapped_receive
			var overlapped_receiving = statistics.overlapped_receiving;

			// check) 
			if(m_maximum_mesage_buffer_count <= overlapped_receiving)
				return;

			// 1) get count
			var count_receive = m_maximum_mesage_buffer_count - overlapped_receiving;

			// 2) prepare receive
			while(count_receive != 0)
			{
				// - alloc EventArgs object
				SocketAsyncEventArgs_recv args_receive = m_factory_async_args_receive.alloc();

				// referece counting)
				add_ref();

				// - alloc buffer
				var buf_recv = factory.memory.alloc_buffer(m_maximum_mesage_buffer_size);

				// - set receive parameter
				args_receive.UserToken = this;
				args_receive.BufferList = null;
				args_receive.SocketFlags = SocketFlags.None;
				args_receive.SocketError = SocketError.Success;
				args_receive.SetBuffer(buf_recv.Array, buf_recv.Offset, m_maximum_mesage_buffer_size);   // Buffer, Offset, Count

				// statistics) 
				statistics.statistics_on_increase_async_receiveing();

				// - call 'receive_from_async'
				var result = m_socket.receive_from_async(args_receive);

				// check) 
				if(result == false)
				{
					// statistics) 
					statistics.statistics_on_decrease_async_receiveing();

					// - 
					if(args_receive.SocketError == SocketError.Success)
					{
						// statistics)
						statistics.statistics_on_receive_bytes(args_receive.BytesTransferred);

						try
						{
							// - process receive
							process_receive(args_receive);
						}
						catch(System.Exception)
						{
						}
					}

					// - clear args_receive
					args_receive.clear();

					// - free args_receive
					m_factory_async_args_receive.free(args_receive);

					// referece counting)
					release();
				}
				else
				{
					--count_receive;
				}

				// statistics) 
				statistics.statistics_on_receive_async();
			}
		}
		private static void				complete_process_receive(object _source, SocketAsyncEventArgs _args)
		{
			// declare) 
			var	args_receive = _args as SocketAsyncEventArgs_recv;
			var socket_udp = _args.UserToken as Iudp;

			// check) 
			Debug.Assert(_args.UserToken != null);
			Debug.Assert(socket_udp != null);

			// 1) call 'process_complete_receive'
			socket_udp.process_complete_receive(args_receive);

			// 2) clear 'args_receive'
			args_receive.clear();

			// 3) free args_receive
			m_factory_async_args_receive.free(args_receive);

			// Reference Counting) 
			socket_udp.release();
		}
		protected virtual void			process_complete_receive(SocketAsyncEventArgs_recv _args)
		{
			// statistics)
			statistics.statistics_on_receive_bytes(_args.BytesTransferred);

			// statistics) 
			statistics.statistics_on_decrease_async_receiveing();

			// check) 
			if(_args.SocketError != SocketError.Success)
				return;

			try
			{
				process_receive(_args);
			}
			catch(System.Exception)
			{
				// return)
				return;
			}
        
			// 6) receive
			prepare_receive();
		}
		protected void					process_receive(SocketAsyncEventArgs_recv _args)
		{
			// declare) 
			var	buf_message = new CGDK.buffer(_args.Buffer, _args.Offset, _args.BytesTransferred);

			// 1) call 'on_receive'
			on_receive(buf_message, _args);

			// 2) call 'process_packet'
			process_packet(ref buf_message, _args.RemoteEndPoint as IPEndPoint);
		}
		protected virtual int			process_packet(ref CGDK.buffer _buffer, IPEndPoint _address)
		{
			// declare)
			sMESSAGE msg = new sMESSAGE(eMESSAGE.SYSTEM.MESSAGE, this, _buffer, _address);

			// statistics) 
			statistics.statistics_on_receive_message(1);

			// 1) call 'process_message'
			process_message(this, msg);

			// return) 
			return 0;
		}
		public int						process_message(object _source, sMESSAGE _msg)
		{
			return on_message(_source, _msg);
		}

		public void						Dispose()
		{
			// 1) dispose
			Dispose(true);

			// 2) GC
			GC.SuppressFinalize(this);
		}
		protected virtual void			Dispose(bool _is_dispose)
		{
			// check)
			if (m_is_disposed)
				return;

			// 1) is disonsed
			if (_is_dispose) 
			{
				// check) 
				Debug.Assert(reference_count==0);

				// - dispose socket
				m_socket.Dispose();
			}

			// 2) set disposed
			m_is_disposed = true;
		}

		protected class SocketAsyncEventArgs_send : SocketAsyncEventArgs
		{
			public	SocketAsyncEventArgs_send()
			{
				Completed += new EventHandler<SocketAsyncEventArgs>(udp.complete_process_send);
			}

			public void clear()
			{
				// 1) free buffer
				if(Buffer != null)
				{
					// - free buffer
					memory.free(Buffer);

					// - reset buffer
					SetBuffer(null, 0, 0);
				}

				// 2) reset UserToken
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
				Completed += new EventHandler<SocketAsyncEventArgs>(udp.complete_process_receive);
				RemoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
			}

			public void clear()
			{
				// 1) free array
				if(Buffer != null)
				{
					// - free buffer
					memory.free(Buffer);

					// - reset buffer
					SetBuffer(null, 0, 0);
				}

				// 2) reset UserToken
				UserToken = null;

				// check) 
				Debug.Assert(BufferList == null);
				Debug.Assert(AcceptSocket == null);
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
		public static int				default_minimum_mesage_buffer_size		 = DEFAULT_UDP_MESSAGE_BUFFER_SIZE;
		public static int				default_maximum_mesage_buffer_size		 = DEFAULT_UDP_MESSAGE_BUFFER_SIZE;
		public static int				default_maximum_message_buffer_count	 = DEFAULT_UDP_MESSAGE_BUFFER_COUNT;

		// 2) socket status
		private io.NSocket				m_socket								 = new io.NSocket();

		// 3) buffers
		private int						m_minimum_mesage_buffer_size			 = default_minimum_mesage_buffer_size;
		private int						m_maximum_mesage_buffer_size			 = default_maximum_mesage_buffer_size;
		private int						m_maximum_mesage_buffer_count			 = default_maximum_message_buffer_count;

		// 4) ...
		private static factory.Cobject<SocketAsyncEventArgs_send>    m_factroy_async_args_send	 = new factory.Cobject<SocketAsyncEventArgs_send>("AsyncEventArgsSend");
		private static factory.Cobject<SocketAsyncEventArgs_recv>	m_factory_async_args_receive = new factory.Cobject<SocketAsyncEventArgs_recv>("AsyncEventArgsReceive");

		// 5) name & object state
		private	string					m_name;
		private Cobject_state			m_component_state;

		// 6) statistics
		private io.statistics.Ntraffic	m_statistics_traffic		 = new io.statistics.Ntraffic();

		// 7) 
		private	bool					m_is_disposed				 = false;
	}
}
