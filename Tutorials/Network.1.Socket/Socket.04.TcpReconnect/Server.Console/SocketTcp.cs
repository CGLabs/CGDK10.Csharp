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
using CGDK;

public class SocketTcp : CGDK.Net.Socket.TcpClient
{
	protected override void OnConnect()
	{
		Console.WriteLine(" @ connected");
	}

	protected override void OnDisconnect(ulong _reason)
	{
		Console.WriteLine(" @ disconnected");
	}

	protected override int OnMessage(object _source, sMESSAGE _msg)
	{
		Console.WriteLine(" @ message received and echo sended!");

		// 1) echo 전송
		this.Send(_msg.bufMessage);

		// Return) 
		return 1;
	}
}
