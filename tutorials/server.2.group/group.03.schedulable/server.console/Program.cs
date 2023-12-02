//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*             tutorials group - message_mediator.server.console             *
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
using CGDK.net;

class Program
{
	static public group_simple pgroup = new group_simple();

	public static int Main(String[] args)
	{
		// trace) 
		Console.WriteLine("CGCII tutorial.group.server.NET 01...");

		// 1) group 객체를 생성한다 / create group object
		pgroup = new group_simple();

		// 2) group에 입장 가능하게 설정한다 / enable entering of group object
		pgroup.enable_enter = true;

		// 1) Acceptor를 생성한다.
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
