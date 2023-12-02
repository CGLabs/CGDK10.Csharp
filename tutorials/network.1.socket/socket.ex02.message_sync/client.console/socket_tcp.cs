//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*               tutorials socket - message_sync.server.console              *
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
using CGDK;

public class socket_tcp : CGDK.net.socket.Itcp_client_sync
{
	protected override void on_request_connect(IPEndPoint _remote_endpoint)
	{
		// trace) 
		Console.WriteLine(" @ request connect to " + _remote_endpoint.ToString());
	}
	protected override void on_connect()
	{
		// trace) 
		Console.WriteLine(" @ connected");
	}
	protected override void on_fail_connect(ulong _disconnect_reason)
	{
		// trace) 
		Console.WriteLine(" @ fail to connect");
	}
	protected override void on_disconnect(ulong _disconnect_reason)
	{
		// trace) 
		Console.WriteLine(" @ disconnected");
	}
	protected override int on_message(object _source, sMESSAGE _msg)
	{
		// trace) 
		Console.WriteLine(" @ message received!");

		// Return) 
		return 0;
	}
}
