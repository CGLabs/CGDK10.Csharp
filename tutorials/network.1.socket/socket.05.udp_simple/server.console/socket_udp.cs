//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*              tutorials socket - tcp_reconnect.server.console              *
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
using System.Threading;
using CGDK;
using CGDK.net;
using CGDK.net.io;

public class socket_udp : CGDK.net.socket.Iudp
{
	protected override int on_message(object _source, sMESSAGE _msg)
	{
		Console.WriteLine(" @ datagram message received and echo sended!");

		// 1) echo
		send_to(_msg.buf_message, _msg.address);

		// return) 
		return 1;
	}
}
