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
using System.Net.Sockets;

//----------------------------------------------------------------------------
//
//  class CGDK.net.socket.tcp
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK.net.socket
{
	public class tcp : 
		Itcp 
	{
	// publics) 
		public delegate_notify			notify_on_connect;
		public delegate_notify_int64	notify_on_fail_connect;
		public delegate_notify_int64	notify_on_disconnect;
		public delegate_notify_io		notify_on_receive;
		public delegate_notify_message	notify_on_message;

	// frameworks)
		protected override void			on_connect()
		{
			if(notify_on_connect != null)
			{
				notify_on_connect(this);
			}
		}
		protected override void			on_fail_connect(ulong _disconnect_reason)
		{
			if(notify_on_fail_connect != null)
			{
				notify_on_fail_connect(this, _disconnect_reason);
			}
		}
		protected override void			on_disconnect(ulong _disconnect_reason)
		{
			if(notify_on_disconnect != null)
			{
				notify_on_disconnect(this, _disconnect_reason);
			}
		}
		protected override void			on_receive(CGDK.buffer _buffer_received, SocketAsyncEventArgs _args)
		{
			if (notify_on_receive != null)
			{
				notify_on_receive(this, _buffer_received, _args);
			}
		}
		protected override int			on_message(object _source, sMESSAGE _msg)
		{
			if(notify_on_message != null)
			{
				return	notify_on_message(_source, _msg);
			}
			else
			{
				return 0;
			}
		}
	}
}