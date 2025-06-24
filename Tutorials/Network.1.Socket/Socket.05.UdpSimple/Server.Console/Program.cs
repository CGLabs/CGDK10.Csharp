//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*            tutorials socket - udp.simple.server.server.console            *
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

// ----------------------------------------------------------------------------
//
// tutorial socket - udp simple.server.console
//
// UDP를 사용한 간단한 서버 프로그램이다.
//
// ----------------------------------------------------------------------------
namespace tutorial.socket._05.udp_simple.server.console
{
	class Program
	{
		static void Main(string[] args)
		{
			// trace) 
			Console.WriteLine("Start server [tut.socket.05.udp.server for C#]...");

			try
			{
				// 1) Acceptor를 생성한다.
				var temp_socket = new socket_udp();

				// 2) 20000번 포트로 bind한다.
				temp_socket.Start(new IPEndPoint(IPAddress.Any, 21000));

				// 3) ESC누를 때까지 대기 (ESC를 누를 때까지 기다린다.)
				while (Console.ReadKey().Key != ConsoleKey.Escape) ;

				// 4) Acceptor를 닫는다.
				temp_socket.Stop();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			// trace) 
			Console.WriteLine("Stop server [tut.socket.05.udp.server for C#]...");
		}
	}
}
