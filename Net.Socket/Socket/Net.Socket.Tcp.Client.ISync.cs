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
using System.Net;
using System.Net.Sockets;

//----------------------------------------------------------------------------
//  CGDK.Network.SocketClasses
//
//  class socket_tcp
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
	public class ITcpClientSync : ITcpClient
	{
	// Definitions) 
		private class ExecutableSocketOnConnect : IExecutable
		{
			public ITcpClientSync pSocket;
			public SocketAsyncEventArgs	Args;

			public long ProcessExecute(ulong _return, ulong _param)
			{
				// 1) Socket의 Connect처리를 한다.
				try
				{
					pSocket.ProcessCompleteConnectPost(Args);
				}
				catch(System.Exception)
				{
				}

				// 2) Hold를 위해 AddReference한 것을 Release한다.
				pSocket.Release();

				// return) 
				return 0;
			}
		}
		private class ExecutableSocketOnReceive : IExecutable
		{
			public ITcpClientSync pSocket;
			public SocketAsyncEventArgs	Args;

			public long ProcessExecute(ulong _return, ulong _param)
			{
				// 1) Socket의 Receive처리를 한다.
				try
				{
					pSocket.ProcessCompleteReceivePost(Args);
				}
				catch(System.Exception)
				{
				}

				// 2) Hold를 위해 AddReference한 것을 Release한다.
				pSocket.Release();

				// return) 
				return 0;
			}
		}

	// constructors) 
		public ITcpClientSync()
		{
			this.m_io_queue = ExecutableQueue.DefaultQueue;
		}
		public ITcpClientSync(string _name) : base(_name)
		{
			this.m_io_queue = ExecutableQueue.DefaultQueue;
        }

	// implementation)
		public override bool	ProcessCompleteConnect(SocketAsyncEventArgs _args)
		{
			// 1) Queuing할 I/O 객체를 만든다.
			var pevent = new ExecutableSocketOnConnect
			{
				pSocket = this,
				Args = (_args as SocketAsyncEventArgs_connect).Clone()
			};

			// 2) Socket의 Hold를 위해 AddReference
			this.AddReference();

			// 3) Queuing한다.
			try
			{
				lock (this.m_io_queue)
				{
					this.m_io_queue.Enqueue(pevent);
				}
			}
			catch(System.Exception)
			{
				// - Hold를 위해 AddReference한 것을 다시 Release
				this.Release();

				// reraise)
				throw;
			}

			//return)
			return true;
		}
		public void				ProcessCompleteConnectPost(SocketAsyncEventArgs _args)
		{
			base.ProcessCompleteConnect(_args);
		}

		protected override void ProcessCompleteReceive(SocketAsyncEventArgs _args)
		{
			// 1) Queuing할 I/O 객체를 만든다.
			var pevent = new ExecutableSocketOnReceive
			{
				pSocket = this,
				Args = _args
			};

			// 2) Socket의 Hold를 위해 AddReference
			this.AddReference();

			// 3) Queuing한다.
			try
			{
				lock (this.m_io_queue)
				{
					this.m_io_queue.Enqueue(pevent);
				}
			}
			catch(System.Exception)
			{
				// - Hold를 위해 AddReference한 것을 다시 Release()
				this.Release();

				// reraise)
				throw;
			}
		}
		protected void			ProcessCompleteReceivePost(SocketAsyncEventArgs _args)
		{
			base.ProcessCompleteReceive(_args);
		}

		public ExecutableQueue io_queue
		{
			get { lock(this.m_io_queue) { return this.m_io_queue;} }
			set { lock(this.m_io_queue) { this.m_io_queue = value;} }
		}

		private ExecutableQueue m_io_queue;
	}
}