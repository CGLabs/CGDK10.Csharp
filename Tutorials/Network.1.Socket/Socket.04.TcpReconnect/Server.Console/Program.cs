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

// ----------------------------------------------------------------------------
//
// tutorial socket - tcp reconnect
//
// 재접속 기능은 acceptor쪽의 socket은 사용할 수 없다.
// connect를 시도하는 client socket쪽에서만 사용할 수 있다.
// 이 프로젝트는 그냥 client쪽과 짝을 맞추기위한 프로젝트다.
//
// reconnect의 사용법은 client쪽을 참조하기 바란다.
//
// ----------------------------------------------------------------------------
namespace tutorial_socket_04_tcp_reconnect_server_console
{
	class Program
	{
		public static void Main(String[] args)
		{
			// trace) 
			Console.WriteLine("Start server [tut.socket.03.tcp_reconnect.server for C#]...");

			try
			{
				// 1) Acceptor를 생성한다.
				var acceptor_test = new CGDK.Net.Acceptor<SocketTcp>("");

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
			Console.WriteLine("Stop server [tut.socket.03.tcp_reconnect.server for C#]...");
		}
	}
}
