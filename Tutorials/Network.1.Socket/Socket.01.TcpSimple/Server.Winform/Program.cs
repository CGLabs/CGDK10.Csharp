//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*               tutorials socket - tcp_simple.server.winform                *
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
// tutorial socket. - tcp simple.winform
//
// tutorial socket - tcp_simple과 동일한 튜터리얼이다. 다만 플랫폼과 UI를 winform으로
// 한 차이일 뿐다.
// 
// ----------------------------------------------------------------------------
namespace tutorial_socket_01_tcp_simple_server_winform
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			try
			{
				// 1) create acceptor
				var acceptor_test = new CGDK.Net.Acceptor<SocketTcp>();

				// 2) listen on 20000 port
				acceptor_test.Start(20000);

				// 3) wait until input 'ESC key'
				while (Console.ReadKey().Key != ConsoleKey.Escape) ;

				// 4) close acceptor
				acceptor_test.Stop();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
		}
	}
}
