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

public class socket_udp : CGDK.Net.Socket.IUdp
{
	protected override int OnMessage(object _source, sMESSAGE _msg)
	{
		Console.WriteLine(" @ datagram message received and echo sended!");

		// 1) echo
		this.SendTo(_msg.bufMessage, _msg.address);

		// return) 
		return 1;
	}
}
