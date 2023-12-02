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
//
//  class CGDK.Net.Socket.tcp
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK.Net.Socket
{
	public class TcpClientSync : ITcpClientSync
	{
	// constructors) 
		public TcpClientSync()
		{
		}
		public TcpClientSync(string _name) : base(_name)
		{
		}

	// publics) 
		public delegateNotify_connect		NotifyOnRequestConnect;
		public delegateNotify				NotifyOnConnect;
		public delegateNotifyInt64		NotifyOnFailConnect;
		public delegateNotifyInt64		NotifyOnDisconnect;
		public delegateNotify_io			NotifyOnReceive;
		public delegateNotifyMessage		NotifyOnMessage;

	// Frameworks
		protected override void			OnRequestConnect(IPEndPoint _remote_endpoint)
		{
			this.NotifyOnRequestConnect?.Invoke(this, _remote_endpoint);
		}
		protected override void			OnConnect()
		{
			this.NotifyOnConnect?.Invoke(this);
		}
		protected override void			OnFailConnect(ulong _disconnect_reason)
		{
			this.NotifyOnFailConnect?.Invoke(this, _disconnect_reason);
		}
		protected override void			OnDisconnect(ulong _disconnect_reason)
		{
			this.NotifyOnDisconnect?.Invoke(this, _disconnect_reason);
		}
		protected override void			OnReceive(CGDK.buffer _buffer_received, SocketAsyncEventArgs _args)
		{
			this.NotifyOnReceive?.Invoke(this, _buffer_received, _args);
		}
		protected override int			OnMessage(object _source, sMESSAGE _msg)
		{
			if (this.NotifyOnMessage != null)
			{
				return this.NotifyOnMessage(_source, _msg);
			}
			else
			{
				return 0;
			}
		}
	}
}