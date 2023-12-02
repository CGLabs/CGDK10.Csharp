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
using System.Threading;
using System.Net;
using System.Net.Sockets;

//----------------------------------------------------------------------------
//
//  class CGDK.net.io.Nsocket
//
//
//----------------------------------------------------------------------------
namespace CGDK.net.io
{
	public class NSocket : IDisposable
	{
	// publics) 
		public	eSOCKET_STATE			exchange_socket_state(eSOCKET_STATE _status)
		{
			return (eSOCKET_STATE)Interlocked.Exchange(ref m_status_socket, (int)_status);
		}
		public	eSOCKET_STATE			set_socket_state_if(eSOCKET_STATE _status_comperand, eSOCKET_STATE _status_new)
		{
			return (eSOCKET_STATE)Interlocked.CompareExchange(ref m_status_socket, (int)_status_new, (int)_status_comperand);
		}
		public	eSOCKET_STATE			socket_state
		{
			get { return (eSOCKET_STATE)m_status_socket; }
            set { exchange_socket_state(value); }
		}

		public	bool					create_socket_handle(AddressFamily addressFamily, SocketType _socket_type, ProtocolType _protocol_type)
		{
			lock(m_cs_socket)
			{
				// check) Listen socket이 닫힌 상태라면 끝낸다.
				if(m_handle_socket != null)
					return false;

				// 1) create socket handle
				m_handle_socket = new System.Net.Sockets.Socket(addressFamily, _socket_type, _protocol_type);

				// return) 
				return	m_handle_socket!=null;
			}
		}
		public Socket					socket_handle
		{
			get
			{
				lock(m_cs_socket)
				{
					return	m_handle_socket;
				}
			}
			set
			{
				lock(m_cs_socket)
				{
					if(m_handle_socket != null)
						m_handle_socket.Close();

					m_handle_socket = value;
				}
			} 
		}
		public	bool					close_socket_handle()
		{
			lock(m_cs_socket)
			{
				// check)
				if(m_handle_socket == null)
					return	false;

				// check)
				if(m_handle_socket.Handle == null)
					return	false;

				// 1) close handle
				m_handle_socket.Close();

				// 2) reset socket handle value
				m_handle_socket	 = null;

				// return)
				return	true;
			}
		}
		public	bool					shutdown(SocketShutdown _how = SocketShutdown.Send)
		{
			lock(m_cs_socket)
			{
				// check)
				if(m_handle_socket == null)
					return	false;

				// check)
				if(m_handle_socket.Handle == null)
					return	false;

				// 1) send shutdown
				m_handle_socket.Shutdown(_how);

				// return)
				return	true;
			}
		}
		public	LingerOption			linger_option
		{
			get
			{
				lock(m_cs_socket)
				{
					// check)
					if(m_handle_socket == null)
						return null;

					// return 
					return m_handle_socket.LingerState;
				}
			}

			set
			{
				lock(m_cs_socket)
				{
					// check)
					if(m_handle_socket == null)
						return;

					// check)
					if(m_handle_socket.Handle == null)
						return;

					// 1) set linger option 
					m_handle_socket.LingerState = value;
				}
			}
		}
		public	void					set_linger_option_graceful()
		{
			linger_option = new LingerOption(false, 0);
		}
		public	void					set_linger_option_abortive()
		{
			linger_option = new LingerOption(true, 0);
		}
		public	int						get_available()
		{
			lock(m_cs_socket)
			{
				// check) Listen socket이 닫힌 상태라면 끝낸다.
				if(m_handle_socket == null)
					return	0;

				// check) NSocket Handle이 null인가?
				if(m_handle_socket.Handle == null)
					return	0;

				// return) 
				return	m_handle_socket.Available;
			}
		}

		public	int						io_control(System.Net.Sockets.IOControlCode _io_control_code, byte[] _option_in_value, byte[] _option_out_value)
		{
			lock(m_cs_socket)
			{
				// check)
				if(m_handle_socket == null)
					return 0;

				// check)
				if(m_handle_socket.Handle == null)
					return 0;

				// 1) call 'IOControl'
				return m_handle_socket.IOControl(_io_control_code, _option_in_value, _option_out_value);
			}
		}

		public	string					local_end_point
		{
			get
			{
				return (m_handle_socket!=null) ? m_handle_socket.LocalEndPoint.ToString() : "-";
			}
		}
		public	void					request_bind(EndPoint _e)
		{
			lock(m_cs_socket)
			{
				// check) Listen socket이 닫힌 상태라면 끝낸다.
				if(m_handle_socket == null)
					throw new System.Exception();

				// check) NSocket Handle이 null인가?
				if(m_handle_socket.Handle == null)
					throw new System.Exception();

				// 1) Bind를 수행한다.
				m_handle_socket.Bind(_e);
			}
		}
		public	void					request_listen(int _e)
		{
			lock(m_cs_socket)
			{
				// check) Listen socket이 닫힌 상태라면 끝낸다.
				if(m_handle_socket == null)
					throw new System.Exception();

				// check) NSocket Handle이 null인가?
				if(m_handle_socket.Handle == null)
					throw new System.Exception();

				// 1) Listen을 수행한다.
				m_handle_socket.Listen(_e);
			}
		}
		public	bool					accept_async(SocketAsyncEventArgs _e)
		{
			lock(m_cs_socket)
			{
				// check) Listen socket이 닫힌 상태라면 끝낸다.
				if(m_handle_socket == null)
				{
					_e.SocketError = SocketError.NotSocket;
					return false;
				}

				// check) NSocket Handle이 null인가?
				if(m_handle_socket.Handle == null)
				{
					_e.SocketError = SocketError.NotSocket;
					return false;
				}

				// 1) AcceptAsync를 수행한다.
				return	m_handle_socket.AcceptAsync(_e);
			}
		}
		public	bool					connect_async(SocketAsyncEventArgs _e)
		{
			lock(m_cs_socket)
			{
				// check) Listen socket이 닫힌 상태라면 끝낸다.
				if(m_handle_socket == null)
				{
					_e.SocketError = SocketError.NotSocket;
					return false;
				}

				// check) NSocket Handle이 null인가?
				if(m_handle_socket.Handle == null)
				{
					_e.SocketError = SocketError.NotSocket;
					return false;
				}

				// 1) SendAsync를 수행한다.
				return	m_handle_socket.ConnectAsync(_e);
			}
		}
		public	bool					send_async(SocketAsyncEventArgs _e)
		{
			lock(m_cs_socket)
			{
				// check) Listen socket이 닫힌 상태라면 끝낸다.
				if(m_handle_socket == null)
				{
					_e.SocketError = SocketError.NotSocket;
					return false;
				}

				// check) NSocket Handle이 null인가?
				if(m_handle_socket.Handle == null)
				{
					_e.SocketError = SocketError.NotSocket;
					return false;
				}

				// 1) SendAsync를 수행한다.
				return	m_handle_socket.SendAsync(_e);
			}
		}
		public	bool					receive_async(SocketAsyncEventArgs _e)
		{
			lock(m_cs_socket)
			{
				// check)
				if(m_handle_socket == null)
				{
					_e.SocketError = SocketError.NotSocket;
					return false;
				}

				// check)
				if(m_handle_socket.Handle == null)
				{
					_e.SocketError = SocketError.NotSocket;
					return false;
				}

				// 1) call 'ReceiveAsync'
				return	m_handle_socket.ReceiveAsync(_e);
			}
		}
		public	bool					sendto_async(SocketAsyncEventArgs _e)
		{
			lock(m_cs_socket)
			{
				// check) Listen socket이 닫힌 상태라면 끝낸다.
				if(m_handle_socket == null)
				{
					_e.SocketError = SocketError.NotSocket;
					return false;
				}

				// check) NSocket Handle이 null인가?
				if(m_handle_socket.Handle == null)
				{
					_e.SocketError = SocketError.NotSocket;
					return false;
				}

				// 1) SendAsync를 수행한다.
				return	m_handle_socket.SendToAsync(_e);
			}
		}
		public	bool					receive_from_async(SocketAsyncEventArgs _e)
		{
			lock(m_cs_socket)
			{
				// check) 
				if(m_handle_socket == null)
				{
					// - set socket error
					_e.SocketError = SocketError.NotSocket;

					// return) 
					return false;
				}

				// check) 
				if(m_handle_socket.Handle == null)
				{
					// - set socket error
					_e.SocketError = SocketError.NotSocket;

					// return) 
					return false;
				}

				// 1) call 'ReceiveAsync'
				return	m_handle_socket.ReceiveFromAsync(_e);
			}
		}

		public	void					set_receive_buffer_size(int _size)
		{
			lock(m_cs_socket)
			{
				// check) Listen socket이 닫힌 상태라면 끝낸다.
				if(m_handle_socket == null)
					return;

				// check) NSocket Handle이 null인가?
				if(m_handle_socket.Handle == null)
					return;

				// 1) Set
				m_handle_socket.ReceiveBufferSize = _size;
			}
		}
		public	int						get_send_buffer_size()
		{
			return  (m_handle_socket != null) ? m_handle_socket.SendBufferSize : 0;
		}
		public	void					set_send_buffer_size(int _size)
		{
			lock(m_cs_socket)
			{
				// check) Listen socket이 닫힌 상태라면 끝낸다.
				if(m_handle_socket == null)
					return;

				// check) NSocket Handle이 null인가?
				if(m_handle_socket.Handle == null)
					return;

				// check) Buffer이 차이가 없으면 그냥 끝낸다.
				if(m_handle_socket.SendBufferSize == _size)
					return;

				// 1) Set
				m_handle_socket.SendBufferSize = _size;
			}
		}

		public	void					Dispose()
		{
			// 1) Dispose한다.
			Dispose(true);

			// 2) GC
			GC.SuppressFinalize(this);
		}
		protected virtual void			Dispose(bool _is_dispose)
		{
			// check) 이미 Disposed되어 있으면 그냥 끝낸다.
			if (m_is_disposed)
				return;

			// 1) 다른 managed object들을 Free한다.
			if (_is_dispose) 
			{
				lock(m_cs_socket)
				{
					if(m_handle_socket != null)
					{
						//m_handle_socket.Dispose();
						m_handle_socket.Close();
					}
				}
			}

			// 3) dispose를 true로...
			m_is_disposed = true;
		}

	// implementations) 
		protected	bool				m_is_disposed	 = false;
		private		Socket				m_handle_socket	 = null;
		protected	Object				m_cs_socket		 = new Object();
		private		int					m_status_socket	 = (int)eSOCKET_STATE.CLOSED;
	}
}
