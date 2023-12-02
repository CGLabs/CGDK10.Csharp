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
using System.Net;
using System.Net.Sockets;

//----------------------------------------------------------------------------
//  CGDK.Network.SocketClasses
//
//  class socket_tcp
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
	public class Itcp_client_sync : Itcp_client
	{
	// Definitions) 
		private class executable_socket_on_connect : Iexecutable
		{
			public Itcp_client_sync pSocket;
			public SocketAsyncEventArgs	Args;

			public long process_execute(ulong _return, ulong _param)
			{
				// 1) socket의 Connect처리를 한다.
				try
				{
					pSocket.process_complete_connect_post(Args);
				}
				catch(System.Exception)
				{
				}

				// 2) Hold를 위해 add_ref한 것을 Release한다.
				pSocket.release();

				// return) 
				return 0;
			}
		}
		private class executable_socket_on_receive : Iexecutable
		{
			public Itcp_client_sync pSocket;
			public SocketAsyncEventArgs	Args;

			public long process_execute(ulong _return, ulong _param)
			{
				// 1) socket의 Receive처리를 한다.
				try
				{
					pSocket.process_complete_receive_post(Args);
				}
				catch(System.Exception)
				{
				}

				// 2) Hold를 위해 add_ref한 것을 Release한다.
				pSocket.release();

				// return) 
				return 0;
			}
		}

	// constructors) 
		public Itcp_client_sync()
		{
			m_io_queue = executable_queue.default_queue;
		}
		public Itcp_client_sync(string _name) : base(_name)
		{
			m_io_queue = executable_queue.default_queue;
        }

	// implementation)
		public override bool	process_complete_connect(SocketAsyncEventArgs _args)
		{
			// 1) Queuing할 I/O 객체를 만든다.
			var pevent = new executable_socket_on_connect();
			pevent.pSocket = this;
			pevent.Args = (_args as SocketAsyncEventArgs_connect).Clone();

			// 2) socket의 Hold를 위해 add_ref
			add_ref();

			// 3) Queuing한다.
			try
			{
				lock (m_io_queue)
				{
					m_io_queue.enqueue(pevent);
				}
			}
			catch(System.Exception)
			{
				// - Hold를 위해 add_ref한 것을 다시 Release
				release();

				// reraise)
				throw;
			}

			//return)
			return true;
		}
		public void				process_complete_connect_post(SocketAsyncEventArgs _args)
		{
			base.process_complete_connect(_args);
		}

		protected override void process_complete_receive(SocketAsyncEventArgs _args)
		{
			// 1) Queuing할 I/O 객체를 만든다.
			var	pevent = new executable_socket_on_receive();
			pevent.pSocket = this;
			pevent.Args = _args;

			// 2) socket의 Hold를 위해 add_ref
			add_ref();

			// 3) Queuing한다.
			try
			{
				lock (m_io_queue)
				{
					m_io_queue.enqueue(pevent);
				}
			}
			catch(System.Exception)
			{
				// - Hold를 위해 add_ref한 것을 다시 Release()
				release();

				// reraise)
				throw;
			}
		}
		protected void			process_complete_receive_post(SocketAsyncEventArgs _args)
		{
			base.process_complete_receive(_args);
		}

		public executable_queue io_queue
		{
			get { lock(m_io_queue) { return m_io_queue;} }
			set { lock(m_io_queue) { m_io_queue = value;} }
		}

		private executable_queue m_io_queue;
	}
}