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
using System.Threading;
using System.Net;
using System.Net.Sockets;

//----------------------------------------------------------------------------
//
//  class CGDK.Net.Io.Nsocket
//
//
//----------------------------------------------------------------------------
namespace CGDK.Net.Io
{
	public class NSocket : IDisposable
	{
	// publics) 
		public	eSOCKET_STATE			ExchangeSocketState(eSOCKET_STATE _status)
		{
			return (eSOCKET_STATE)Interlocked.Exchange(ref this.m_status_socket, (int)_status);
		}
		public	eSOCKET_STATE			SetSocketStateIf(eSOCKET_STATE _status_comperand, eSOCKET_STATE _status_new)
		{
			return (eSOCKET_STATE)Interlocked.CompareExchange(ref this.m_status_socket, (int)_status_new, (int)_status_comperand);
		}
		public	eSOCKET_STATE			SocketState
		{
			get { return (eSOCKET_STATE)this.m_status_socket; }
            set { this.ExchangeSocketState(value); }
		}

		public	bool					CreateSocketHandle(AddressFamily addressFamily, SocketType _socket_type, ProtocolType _protocol_type)
		{
			lock(this.m_cs_socket)
			{
				// check) Listen Socket이 닫힌 상태라면 끝낸다.
				if(this.m_handle_socket != null)
					return false;

				// 1) create Socket handle
				this.m_handle_socket = new System.Net.Sockets.Socket(addressFamily, _socket_type, _protocol_type);

				// 2) set default socket options
				this.m_handle_socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);

				// return) 
				return this.m_handle_socket != null;
			}
		}
		public System.Net.Sockets.Socket SocketHandle
		{
			get
			{
				lock(this.m_cs_socket)
				{
					return this.m_handle_socket;
				}
			}
			set
			{
				lock(m_cs_socket)
				{
					this.m_handle_socket?.Close();
					this.m_handle_socket = value;
				}
			} 
		}
		public	bool					CloseSocketHandle()
		{
			lock(m_cs_socket)
			{
				// check)
				if(this.m_handle_socket == null)
					return	false;

				// check)
				if(this.m_handle_socket.Handle == 0)
					return	false;

				// 1) close handle
				this.m_handle_socket.Close();

				// 2) Reset Socket handle value
				this.m_handle_socket = null;

				// return)
				return	true;
			}
		}
		public	bool					Shutdown(SocketShutdown _how = SocketShutdown.Send)
		{
			lock(this.m_cs_socket)
			{
				// check)
				if(this.m_handle_socket == null)
					return false;

				// check)
				if(this.m_handle_socket.Handle == 0)
					return false;

				// 1) Send Shutdown
				this.m_handle_socket.Shutdown(_how);

				// return)
				return true;
			}
		}
		public	LingerOption			LingerOption
		{
			get
			{
				lock(m_cs_socket)
				{
					// check)
					if(this.m_handle_socket == null)
						return null;

					// return 
					return this.m_handle_socket.LingerState;
				}
			}

			set
			{
				lock(m_cs_socket)
				{
					// check)
					if(this.m_handle_socket == null)
						return;

					// check)
					if(this.m_handle_socket.Handle == 0)
						return;

					// 1) set linger option 
					this.m_handle_socket.LingerState = value;
				}
			}
		}
		public	void					SetLingerOptionGraceful()
		{
			LingerOption = new LingerOption(false, 0);
		}
		public	void					SetLingerOptionAbortive()
		{
			LingerOption = new LingerOption(true, 0);
		}
		public	int						GetAvailable()
		{
			lock(this.m_cs_socket)
			{
				// check) Listen Socket이 닫힌 상태라면 끝낸다.
				if(this.m_handle_socket == null)
					return	0;

				// check) NSocket Handle이 null인가?
				if(this.m_handle_socket.Handle == 0)
					return	0;

				// return) 
				return this.m_handle_socket.Available;
			}
		}

		public	int						IoControl(System.Net.Sockets.IOControlCode _io_control_code, byte[] _option_in_value, byte[] _option_out_value)
		{
			lock(m_cs_socket)
			{
				// check)
				if(this.m_handle_socket == null)
					return 0;

				// check)
				if(this.m_handle_socket.Handle == 0)
					return 0;

				// 1) call 'IOControl'
				return this.m_handle_socket.IOControl(_io_control_code, _option_in_value, _option_out_value);
			}
		}

		public	string					LocalEndPoint
		{
			get
			{
				return (this.m_handle_socket!=null) ? this.m_handle_socket.LocalEndPoint.ToString() : "-";
			}
		}
		public	void					RequestBind(EndPoint _e)
		{
			lock(this.m_cs_socket)
			{
				// check) Listen Socket이 닫힌 상태라면 끝낸다.
				if(this.m_handle_socket == null)
					throw new System.Exception();

				// check) NSocket Handle이 null인가?
				if(this.m_handle_socket.Handle == 0)
					throw new System.Exception();

				// 1) Bind를 수행한다.
				this.m_handle_socket.Bind(_e);
			}
		}
		public	void					RequestListen(int _e)
		{
			lock(this.m_cs_socket)
			{
				// check) Listen Socket이 닫힌 상태라면 끝낸다.
				if(this.m_handle_socket == null)
					throw new System.Exception();

				// check) NSocket Handle이 null인가?
				if(this.m_handle_socket.Handle == 0)
					throw new System.Exception();

				// 1) Listen을 수행한다.
				this.m_handle_socket.Listen(_e);
			}
		}
		public	bool					AcceptAsync(SocketAsyncEventArgs _e)
		{
			lock(this.m_cs_socket)
			{
				// check) Listen Socket이 닫힌 상태라면 끝낸다.
				if(this.m_handle_socket == null)
				{
					_e.SocketError = SocketError.NotSocket;
					return false;
				}

				// check) NSocket Handle이 null인가?
				if(this.m_handle_socket.Handle == 0)
				{
					_e.SocketError = SocketError.NotSocket;
					return false;
				}

				// 1) AcceptAsync를 수행한다.
				return this.m_handle_socket.AcceptAsync(_e);
			}
		}
		public	bool					ConnectAsync(SocketAsyncEventArgs _e)
		{
			lock(this.m_cs_socket)
			{
				// check) Listen Socket이 닫힌 상태라면 끝낸다.
				if(this.m_handle_socket == null)
				{
					_e.SocketError = SocketError.NotSocket;
					return false;
				}

				// check) NSocket Handle이 null인가?
				if(this.m_handle_socket.Handle == 0)
				{
					_e.SocketError = SocketError.NotSocket;
					return false;
				}

				// 1) SendAsync를 수행한다.
				return this.m_handle_socket.ConnectAsync(_e);
			}
		}
		public	bool					SendAsync(SocketAsyncEventArgs _e)
		{
			lock(this.m_cs_socket)
			{
				// check) Listen Socket이 닫힌 상태라면 끝낸다.
				if(this.m_handle_socket == null)
				{
					_e.SocketError = SocketError.NotSocket;
					return false;
				}

				// check) NSocket Handle이 null인가?
				if(this.m_handle_socket.Handle == 0)
				{
					_e.SocketError = SocketError.NotSocket;
					return false;
				}

				// 1) SendAsync를 수행한다.
				return this.m_handle_socket.SendAsync(_e);
			}
		}
		public	bool					ReceiveAsync(SocketAsyncEventArgs _e)
		{
			lock(this.m_cs_socket)
			{
				// check)
				if(this.m_handle_socket == null)
				{
					_e.SocketError = SocketError.NotSocket;
					return false;
				}

				// check)
				if(this.m_handle_socket.Handle == 0)
				{
					_e.SocketError = SocketError.NotSocket;
					return false;
				}

				// 1) call 'ReceiveAsync'
				return this.m_handle_socket.ReceiveAsync(_e);
			}
		}
		public	bool					SendToAsync(SocketAsyncEventArgs _e)
		{
			lock(this.m_cs_socket)
			{
				// check) Listen Socket이 닫힌 상태라면 끝낸다.
				if(this.m_handle_socket == null)
				{
					_e.SocketError = SocketError.NotSocket;
					return false;
				}

				// check) NSocket Handle이 null인가?
				if(this.m_handle_socket.Handle == 0)
				{
					_e.SocketError = SocketError.NotSocket;
					return false;
				}

				// 1) SendAsync를 수행한다.
				return this.m_handle_socket.SendToAsync(_e);
			}
		}
		public	bool					ReceiveFromAsync(SocketAsyncEventArgs _e)
		{
			lock(this.m_cs_socket)
			{
				// check) 
				if(this.m_handle_socket == null)
				{
					// - set Socket error
					_e.SocketError = SocketError.NotSocket;

					// return) 
					return false;
				}

				// check) 
				if(m_handle_socket.Handle == 0)
				{
					// - set Socket error
					_e.SocketError = SocketError.NotSocket;

					// return) 
					return false;
				}

				// 1) call 'ReceiveAsync'
				return this.m_handle_socket.ReceiveFromAsync(_e);
			}
		}

		public	void					SetReceiveBufferSize(int _size)
		{
			lock(this.m_cs_socket)
			{
				// check) Listen Socket이 닫힌 상태라면 끝낸다.
				if(this.m_handle_socket == null)
					return;

				// check) NSocket Handle이 null인가?
				if(this.m_handle_socket.Handle == 0)
					return;

				// 1) Set
				this.m_handle_socket.ReceiveBufferSize = _size;
			}
		}
		public	int						GetSendBufferSize()
		{
			return  (this.m_handle_socket != null) ? this.m_handle_socket.SendBufferSize : 0;
		}
		public	void					SetSendBufferSize(int _size)
		{
			lock(this.m_cs_socket)
			{
				// check) Listen Socket이 닫힌 상태라면 끝낸다.
				if(this.m_handle_socket == null)
					return;

				// check) NSocket Handle이 null인가?
				if(this.m_handle_socket.Handle == 0)
					return;

				// check) Buffer이 차이가 없으면 그냥 끝낸다.
				if(this.m_handle_socket.SendBufferSize == _size)
					return;

				// 1) Set
				this.m_handle_socket.SendBufferSize = _size;
			}
		}

		public	void					Dispose()
		{
			// 1) Dispose한다.
			this.Dispose(true);

			// 2) GC
			GC.SuppressFinalize(this);
		}
		protected virtual void			Dispose(bool _is_dispose)
		{
			// check) 이미 Disposed되어 있으면 그냥 끝낸다.
			if (this.m_is_disposed)
				return;

			// 1) 다른 managed object들을 Free한다.
			if (_is_dispose) 
			{
				lock(this.m_cs_socket)
				{
					//m_handle_socket.Dispose();
					this.m_handle_socket?.Close();
				}
			}

			// 3) dispose를 true로...
			this.m_is_disposed = true;
		}

	// implementations) 
		protected	bool				m_is_disposed = false;
		private		System.Net.Sockets.Socket m_handle_socket = null;
		protected	Object				m_cs_socket = new();
		private		int					m_status_socket = (int)eSOCKET_STATE.CLOSED;
	}
}
