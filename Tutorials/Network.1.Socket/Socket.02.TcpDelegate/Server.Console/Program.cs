//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*               tutorials socket - tcp_delegate.server.console              *
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

// ----------------------------------------------------------------------------
//
// tutorial socket. - tcp delegate
//
// OnConnect(), OnDisconnect(), OnMessage() 등과 같은 함수를 재정의하는 대신
// delegate를 사용해서 사용할 수 있다.
//
//  1) socket 클래스를 CGDK.Net.ISocketTcp대시 CGDK.Net.SocketTcp을 상속받는다.
//  2) delegate 변수인 notifyOnConnect, notifyOnDisconnect, notifyOnMessage를 
//     설정한다.
//  3) delegate를 사용할 경우 어떤 함수든 정의가 그낭하다는 장점이 있다.
//  4) delegate와 on_~()함수가 동시에 사용될 경우 on_~()함수가 사용된다.
//  5) 또 일부는 delegate를 다른 일부는 on_~()함수로 혼합해 사용할 수도 있다.
//  6) 일반적으로 client용으로 많이 사용되며 server의 경우 그냥 on~()함수를 사용하는 것을
//     권장한다.
// 
// ----------------------------------------------------------------------------
namespace tutorial.socket._02.tcp_delegate.server.console
{
	class Program
	{
		public static void Main(String[] args)
		{
			// trace) 
			Console.WriteLine("Start server [tut.socket.02.tcp_delegate.server]...");

			try
			{
				// 1) acceptor를 생성한다.
				var acceptor_test = new CGDK.Net.Acceptor<SocketTcp_client>();

				// 2) 20000번 포트로 Listen을 시작한다.
				acceptor_test.Start(20000);

				// trace) 
				Console.WriteLine("@ server Start... (listen: {0})", acceptor_test.AcceptSocket.LocalEndPoint);

				// 3) ESC누를 때까지 대기 (ESC를 누를 때까지 기다린다.)
				while (Console.ReadKey().Key != ConsoleKey.Escape) ;

				// 4) Acceptor를 닫는다.
				acceptor_test.Stop();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			// trace) 
			Console.WriteLine("Stop client [tut.socket.02.tcp_delegate.client]...");
		}

		public class SocketTcp_client : CGDK.Net.Socket.TcpClient
		{
			public SocketTcp_client()
			{
				this.NotifyOnConnect = new(socket_OnConnect);
				this.NotifyOnDisconnect = new(socket_OnDisconnect);
				this.NotifyOnMessage = new(socket_OnMessage);
			}
		}

		private static void socket_OnConnect(object _source)
		{
			Console.WriteLine(" @ connected");
		}
		private static void socket_OnDisconnect(object _source, ulong _disconnect_disconnect_reason)
		{
			// trace) 
			Console.WriteLine(" @ disconnected");
		}

		private static int socket_OnMessage(object _source, sMESSAGE _msg)
		{
			Console.WriteLine(" @ message received");

			// Return) 
			return 0;
		}
	}
}
