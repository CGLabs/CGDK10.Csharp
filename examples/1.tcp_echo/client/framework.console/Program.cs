//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                 sample - tcp_echo.client.framework.console                *
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

namespace sample.tcp_echo.client.console
{
	class Program
	{
		public static int Main(String[] args)
		{
			try
			{
				// declare)
				socket_tcp socket_client;

				// 1) create socket
				socket_client = new socket_tcp();

				// 2) request connect
				socket_client.start("localhost", 20000);

				// 3) ESC누를 때까지 대기 (ESC를 누를 때까지 기다린다.)
				while (Console.ReadKey().Key != ConsoleKey.Escape) ;

				// 4) close socket
				socket_client.closesocket();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			// return) 
			return 0;
		}
	}
}
