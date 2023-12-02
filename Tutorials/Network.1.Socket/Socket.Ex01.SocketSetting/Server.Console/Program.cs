//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*              tutorials socket - socket_setting.server.console             *
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
// tutorial socket - socket_setting
//
// context를 사용해 일관 초기화나 접속이 가능하다.
// 또 외부 JSON 파일을 사용해 일괄 초기화도 가능하다.
//
// ----------------------------------------------------------------------------
namespace tutorial_socket_ex01_socket_setting_server_console
{
	class Program
	{
		public static void Main(String[] args)
		{
			// trace) 
			Console.WriteLine("Start server [tut.socket.ex01.socket_setting.server for C#]...");

			try
			{
				// 1) 
				var context_setting					 = new Context();
				context_setting["acceptor"]["Port"]	 = 20000;
				context_setting["socket"]["Address"] = "127.0.0.1";
				context_setting["socket"]["Port"]	 = 20000;

				int	port = context_setting["socket"]["Port"];

				// 2) Acceptor를 생성한다.
				var	acceptor_test = new CGDK.Net.Acceptor<SocketTcp>("acceptor");

				// 3) 20000번 포트로 Listen을 시작한다.
				acceptor_test.Start(context_setting);
			
				// trace) 
				Console.WriteLine("@ server Start... (listen: {0})", acceptor_test.AcceptSocket.LocalEndPoint);

				// 4) ESC누를 때까지 대기 (ESC를 누를 때까지 기다린다.)
				while(Console.ReadKey().Key!=ConsoleKey.Escape); 

				// 5) acceptor를 닫는다.
				acceptor_test.Stop();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			// trace) 
			Console.WriteLine("Stop server [tut.socket.ex01.socket_setting.server for C#]...");
		}
	}
}
