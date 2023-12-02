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

namespace tutorial_socket_ex02_message_sync_client_console
{

	public class SocketTcp : CGDK.Net.Socket.ITcpClientSync
	{
		protected override void OnRequestConnect(IPEndPoint _remote_endpoint)
		{
			// trace) 
			Console.WriteLine(" @ request connect to " + _remote_endpoint.ToString());
		}
		protected override void OnConnect()
		{
			// trace) 
			Console.WriteLine(" @ connected");
		}
		protected override void OnFailConnect(ulong _Disconnect_reason)
		{
			// trace) 
			Console.WriteLine(" @ fail to connect");
		}
		protected override void OnDisconnect(ulong _Disconnect_reason)
		{
			// trace) 
			Console.WriteLine(" @ Disconnected");
		}
		protected override int OnMessage(object _source, sMESSAGE _msg)
		{
			// trace) 
			Console.WriteLine(" @ message received!");

			// Return) 
			return 0;
		}
	}

}