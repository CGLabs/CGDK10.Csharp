//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                     sample - tcp_echo.client.console                      *
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

namespace Sample.TcpEcho.Client.Console
{
	class Program
	{
		public static int Main()
		{
			try
			{
				// declare)
				SocketTcp socket_client;

				// 1) create socket
				socket_client = new SocketTcp();

				// 2) request connect
				socket_client.Start("localhost", 20000);

				// 3) ESC누를 때까지 대기 (ESC를 누를 때까지 기다린다.)
				while (System.Console.ReadKey().Key != ConsoleKey.Escape) ;

				// 4) close socket
				socket_client.CloseSocket();
			}
			catch (Exception e)
			{
				System.Console.WriteLine(e.ToString());
			}

			// return) 
			return 0;
		}
	}
}
