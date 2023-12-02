//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*              tutorials socket - tcp_delegate.client.console               *
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

namespace tutorial_socket_02_tcp_delegate_client_console
{
	class Program
	{
		public static void Main()
		{
			// trace) 
			Console.WriteLine("Start client [tut.socket.02.tcp_delegate.client]...");

			try
			{
				// 1) Socke을 생성한다.
				var socket_client = new CGDK.Net.Socket.TcpClient
				{
					// 2) Delegator들을 설정
					NotifyOnRequestConnect = new(Socket_OnRequestConnect),
					NotifyOnConnect = new(Socket_OnConnect),
					NotifyOnFailConnect = new(Socket_OnFailConnect),
					NotifyOnDisconnect = new(Socket_OnDisconnect),
					NotifyOnMessage = new(Socket_OnMessage)
				};

				// 3) 접속을 시도한다.
				socket_client.Start("localhost", 20000);

				// 4) ESC누를 때까지 대기 (ESC를 누를 때까지 기다린다.)
				while (Console.ReadKey().Key != ConsoleKey.Escape) ;

				// 5) Socket을 닫는다.
				socket_client.Stop();
			}
			catch (Exception _e)
			{
				Console.WriteLine(_e.ToString());
			}

			// trace) 
			Console.WriteLine("Stop client [tut.socket.02.tcp_delegate.client]...");
		}

		public static void Socket_OnRequestConnect(object _source, IPEndPoint _remote_endpoint)
		{
			// trace) 
			Console.WriteLine(" @ request connect to " + _remote_endpoint.ToString());
		}

		private static void Socket_OnConnect(object _source)
		{
			// 1) 소켓을 얻는다.
			var psocket = _source as CGDK.Net.Socket.TcpClient;

			// trace) 
			Console.WriteLine(" @ connected");

			// 2) 버퍼를 할당받는다.
			var buf_Send = CGDK.Factory.Memory.AllocBuffer(256);

			// 3) Message를 작성한다.
			buf_Send.Append<ushort>(30);
			buf_Send.Append<ushort>(0);
			buf_Send.Append<int>(10);
			buf_Send.Append<string>("TestValue");

			// 4) 메시지 헤더(메시지 길이)를 메시지 제일 앞에 덩ㅄ어 써넣는다.(중요!)
			buf_Send.SetFront<int>(buf_Send.Count);

			// 5) 접속한 대상에게 전송한다.
			psocket.Send(buf_Send);

			// trace) 
			Console.WriteLine(" @ Send message");
		}
		private static void Socket_OnFailConnect(object _source, ulong _Disconnect_reason)
		{
			// trace) 
			Console.WriteLine(" @ fail to connect");
		}
		private static void Socket_OnDisconnect(object _source, ulong _Disconnect_reason)
		{
			// trace) 
			Console.WriteLine(" @ Disconnected");
		}

		private static int Socket_OnMessage(object _source, sMESSAGE _msg)
		{
			// trace) 
			Console.WriteLine(" message received!");

			// return) 
			return 0;
		}
	}
}
