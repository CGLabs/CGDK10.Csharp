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
//
//
//
//----------------------------------------------------------------------------
namespace CGDK.net.socket
{
	public class tcp_client : Itcp_client
	{
	// constructors) 
		public tcp_client()
		{
		}
		public tcp_client(string _name) : base(_name)
		{
		}

	// publics) 
		public delegate_notify_connect	notify_on_request_connect;
		public delegate_notify			notify_on_connect;
		public delegate_notify_int64	notify_on_fail_connect;
		public delegate_notify_int64	notify_on_disconnect;
		public delegate_notify_io		notify_on_receive;
		public delegate_notify_message	notify_on_message;

	// Frameworks
		protected override void			on_request_connect(IPEndPoint _remote_endpoint)
		{
			if (notify_on_request_connect != null)
			{
				notify_on_request_connect(this, _remote_endpoint);
			}
		}
		protected override void			on_connect()
		{
			if (notify_on_connect != null)
			{
				notify_on_connect(this);
			}
		}
		protected override void			on_fail_connect(ulong _disconnect_reason)
		{
			if (notify_on_fail_connect != null)
			{
				notify_on_fail_connect(this, _disconnect_reason);
			}
		}
		protected override void			on_disconnect(ulong _disconnect_reason)
		{
			if (notify_on_disconnect != null)
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
			if (notify_on_message != null)
			{
				return notify_on_message(_source, _msg);
			}
			else
			{
				return 0;
			}
		}
	}
}