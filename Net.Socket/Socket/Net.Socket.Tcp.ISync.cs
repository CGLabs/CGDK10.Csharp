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
//  class CGDK.Net.Socket.ITcpSync
//
//----------------------------------------------------------------------------
namespace CGDK.Net.Socket
{
	public class ITcpSync :
		ITcp
	{
	// Definitions) 
		private class ExecutableSocketOnConnect : IExecutable
		{
			public ITcpSync pSocket;
			public SocketAsyncEventArgs	Args;

			public long ProcessExecute(ulong _return, ulong _param)
			{
				// 1) call 'ProcessCompleteConnectPost'
				try
				{
					pSocket.ProcessCompleteConnectPost(Args);
				}
				catch(System.Exception)
				{
				}

				// 2) AddReference for hold
				pSocket.Release();

				// return) 
				return 0;
			}
		}
		private class ExecutableSocketOnReceive : IExecutable
		{
			public ITcpSync pSocket;
			public SocketAsyncEventArgs	Args;

			public long ProcessExecute(ulong _return, ulong _param)
			{
				// 1) call 'ProcessCompleteReceivePost'
				try
				{
					pSocket.ProcessCompleteReceivePost(Args);
				}
				catch(System.Exception)
				{
				}

				// 2) AddReference for hold
				pSocket.Release();

				// return) 
				return 0;
			}
		}

	// constructors) 
		public ITcpSync()
		{
			m_io_queue = ExecutableQueue.DefaultQueue;
		}

	// implementation)
		public override bool	ProcessCompleteConnect(SocketAsyncEventArgs _args)
		{
			// 1) Alloc i/o object for queuing
			var pevent = new ExecutableSocketOnConnect
			{
				pSocket = this,
				Args = _args
			};

			// 2) AddReference for hold
			this.AddReference();

			// 3) queuing event
			try
			{
				lock (this.m_io_queue)
				{
					this.m_io_queue.Enqueue(pevent);
				}
			}
			catch(System.Exception)
			{
				// - Release for hold
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
			// 1) Alloc i/o object for queuing
			var pevent = new ExecutableSocketOnReceive
			{
				pSocket = this,
				Args = _args
			};

			// 2) AddReference for hold
			this.AddReference();

			// 3) queuing event
			try
			{
				lock (this.m_io_queue)
				{
					this.m_io_queue.Enqueue(pevent);
				}
			}
			catch(System.Exception)
			{
				// - Release for hold
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