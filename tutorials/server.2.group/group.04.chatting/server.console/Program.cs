//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                 tutorials group - chatting.server.console                 *
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

class Program
{
	static public group_manager pgroup_manager = new group_manager();

	public static int Main(String[] args)
	{
		// trace) 
		Console.WriteLine("CGCII tutorial.group.server.NET 04...");

		// 1) acceptor를 생성한다.
		var pacceptor = new CGDK.net.acceptor<socket_tcp>();

		// 2) 20000번 포트로 Listen을 시작한다.
		pacceptor.start(new IPEndPoint(IPAddress.Loopback, 20000));

		// 3) ESC누를 때까지 대기 (ESC를 누를 때까지 기다린다.)
		while (Console.ReadKey().Key != ConsoleKey.Escape) ;

		// 4) Acceptor를 닫는다.
		pacceptor.stop();

		// trace) 
		return 0;
	}
}
