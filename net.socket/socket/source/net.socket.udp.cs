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
//  class CGDK.net.socket.udp
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK.net.socket
{
	public class udp : Iudp
	{
	// publics) 
		public delegate_notify_io		notify_on_receive;
		public delegate_notify_message	notify_on_message;

	// frameworks)
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
				return	0;
			}
		}
	}
}