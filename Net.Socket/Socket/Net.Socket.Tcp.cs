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
	public class Tcp : 
		ITcp 
	{
	// publics) 
		public delegateNotify			NotifyOnConnect;
		public delegateNotifyInt64		NotifyOnFailConnect;
		public delegateNotifyInt64		NotifyOnDisconnect;
		public delegateNotify_io		NotifyOnReceive;
		public delegateNotifyMessage	NotifyOnMessage;

	// frameworks)
		protected override void			OnConnect()
		{
			if (this.NotifyOnConnect == null)
				return;

			this.NotifyOnConnect(this);
		}
		protected override void			OnFailConnect(ulong _disconnect_reason)
		{
			if (this.NotifyOnFailConnect == null)
				return;

			this.NotifyOnFailConnect(this, _disconnect_reason);
		}
		protected override void			OnDisconnect(ulong _disconnect_reason)
		{
			if (this.NotifyOnDisconnect == null)
				return;

			this.NotifyOnDisconnect(this, _disconnect_reason);
		}
		protected override void			OnReceive(CGDK.buffer _buffer_received, SocketAsyncEventArgs _args)
		{
			if (this.NotifyOnReceive == null)
				return;

			this.NotifyOnReceive(this, _buffer_received, _args);
		}
		protected override int			OnMessage(object _source, sMESSAGE _msg)
		{
			if (this.NotifyOnMessage == null)
				return 0;

			return this.NotifyOnMessage(_source, _msg);
		}
	}
}