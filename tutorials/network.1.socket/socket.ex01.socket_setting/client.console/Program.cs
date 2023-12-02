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
using CGDK.net;

namespace tutorial_socket_ex01_socket_setting_client_console
{
	class Program
	{
		public static void Main(String[] args)
		{
			// trace) 
			Console.WriteLine("start client [tut.socket.ex01.socket_setting.client]...");

			try
			{
				// 1) Socke을 생성한다.
				var socket_client = new socket_tcp();

				// 2) context를 작성한다
				var context_setting = new context();

				context_setting["address"] = "localhost";
				context_setting["port"]	   = 20000;

				// 3) context로 접속을 시도한다.
				socket_client.start(context_setting);

				// 4) ESC누를 때까지 대기 (ESC를 누를 때까지 기다린다.)
				while(Console.ReadKey().Key!=ConsoleKey.Escape); 

				// 5) closesocket을 한다.( closesocket과 stop은 동일한다.)
				socket_client.stop();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			// trace) 
			Console.WriteLine("stop client [tut.socket.ex01.socket_setting.client]...");
		}
	}
}
