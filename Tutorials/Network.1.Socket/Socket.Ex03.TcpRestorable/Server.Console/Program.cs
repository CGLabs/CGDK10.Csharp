//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*              tutorials socket - tcp_restorable.server.console             *
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
// tutorial socket - synchronous i/o
//
// 일부 상황에서 동기식 처리가 필요한 경우가 있다.
// 이를 위해 동기식 socket i/o를 queue를 사용해 지원한다.
//
// ----------------------------------------------------------------------------
namespace tutorial_socket_ex03_tcp_restorable_server_console
{
	class Program
	{
	   public static void Main(String[] args)
		{
			// trace) 
			Console.WriteLine("Start server [tut.socket.04.tcp_restorable.server for C#]...");

			try
			{
				// 1) Acceptor를 생성한다.
				var acceptor_test = new CGDK.Net.Acceptor<SocketTcp>("");

				// 2) 20000번 포트로 Listen을 시작한다.
				acceptor_test.Start(20000);

				// trace) 
				Console.WriteLine("@ server Start... (listen: {0})", acceptor_test.AcceptSocket.LocalEndPoint);
 
				// 3) ESC누를 때까지 대기 (ESC를 누를 때까지 기다린다.)
				while(Console.ReadKey().Key != ConsoleKey.Escape); 

				// 4) Acceptor를 닫는다.
				acceptor_test.Stop();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			// trace) 
			Console.WriteLine("Stop server [tut.socket.04.tcp_restorable.server for C#]...");
		}
	}
}
