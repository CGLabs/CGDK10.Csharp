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
//  class CGDK.Net.Socket.IUdpSync
//
//----------------------------------------------------------------------------
namespace CGDK.Net.Socket
{
	public class IUdpSync :
		IUdp
	{
	// definitions) 
		private class ExecutableSocketOnReceive : IExecutable
		{
			public IUdpSync pSocket;
			public SocketAsyncEventArgs_recv Args;

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
		public IUdpSync()
		{
			this.m_io_queue = ExecutableQueue.DefaultQueue;
		}

	// implementation)
		protected override void ProcessCompleteReceive(SocketAsyncEventArgs_recv _args)
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
		protected void			ProcessCompleteReceivePost(SocketAsyncEventArgs_recv _args)
		{
			base.ProcessCompleteReceive(_args);
		}

		public ExecutableQueue io_queue
		{
			get { lock(m_io_queue) { return m_io_queue;} }
			set { lock(m_io_queue) { m_io_queue = value;} }
		}

		private ExecutableQueue m_io_queue;
	}
}