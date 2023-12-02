//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*              tutorials socket - socket_setting.server.console             *
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
using System.Threading;
using CGDK;
using CGDK.net;

public class socket_tcp : CGDK.net.socket.tcp_client
{
	protected override void on_connect()
	{
		Console.WriteLine(" @ connected");
	}

	protected override void on_disconnect(ulong _reason)
	{
		Console.WriteLine(" @ disconnected");
	}

	protected override int on_message(object _source, sMESSAGE _msg)
	{
		Console.WriteLine(" @ message received and echo sended!");

		// 1) echo 전송
		send(_msg.buf_message);

		// Return) 
		return 1;
	}
}
