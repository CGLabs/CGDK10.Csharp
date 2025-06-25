//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*             tutorials group - message_mediator.client.console             *
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

namespace tutorial.group._02.message_mediator.client.console
{	class Program
	{
		static void Main(string[] args)
		{
			// trace) 
			Console.WriteLine("[CGCII Network TCP Test Client for C#]");
			Console.WriteLine("@ Tutorial Group Client/ Simple Group");

			// trace) 
			Console.WriteLine(">> Client Start...");

			try
			{
				// 1) Socke을 생성한다.
				var socket_client = new SocketTcp();

				// 2) 접속을 시도한다.
				socket_client.Start("localhost", 20000);

				// 3) ESC누를 때까지 대기 (ESC를 누를 때까지 기다린다.)
				while (Console.ReadKey().Key != ConsoleKey.Escape) ;

				// 4) Socket을 닫는다.
				socket_client.CloseSocket();
			}
			catch (Exception _e)
			{
				Console.WriteLine(_e.ToString());
			}

			// trace) 
			Console.WriteLine("<< Client Closed...");
		}
	}
}
