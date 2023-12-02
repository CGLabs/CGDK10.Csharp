//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*               tutorials socket - tcp_simple.server.winform                *
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

namespace tutorial_socket_03_tcp_message_client_console
{
	public class SocketTcp : CGDK.Net.Socket.ITcpClient
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
			// - buffer 객체를 임시로 복사한다.
			var buf_recv = _msg.bufMessage;

			// - Message Header를 읽는다.
			var message = buf_recv.GetFront<MESSAGE_struct.HEADER>();

			switch (message.message_type)
			{
				case eMESSAGE_TYPE.A:
					{
						var msg = buf_recv.Extract<MESSAGE_struct.A>();

						Console.WriteLine(" @ MessageType.A Received");
					}
					break;

				case eMESSAGE_TYPE.B:
					{
						var msg = buf_recv.Extract<MESSAGE_struct.B>();

						Console.WriteLine(" @ MessageType.B Received");
					}
					break;

				case eMESSAGE_TYPE.C:
					{
						var msg = buf_recv.Extract<MESSAGE_struct.C>();

						Console.WriteLine(" @ MessageType.C Received");
					}
					break;

				case eMESSAGE_TYPE.D:
					{
						var msg = buf_recv.Extract<MESSAGE_struct.D>();

						Console.WriteLine(" @ MessageType.D Received");
					}
					break;

				case eMESSAGE_TYPE.E:
					{
						var msg = buf_recv.Extract<MESSAGE_struct.E>();

						Console.WriteLine(" @ MessageType.E Received");
					}
					break;

				case eMESSAGE_TYPE.F:
					{
						var msg = buf_recv.Extract<MESSAGE_struct.F>();

						Console.WriteLine(" @ MessageType.F Received");
					}
					break;

				case eMESSAGE_TYPE.G:
					{
						var msg = buf_recv.Extract<MESSAGE_struct.G>();

						Console.WriteLine(" @ MessageType.G Received");
					}
					break;

				default:
					Console.WriteLine(" ! unhandled message");
					break;
			}

			// Return) 
			return 1;
		}
	}

}