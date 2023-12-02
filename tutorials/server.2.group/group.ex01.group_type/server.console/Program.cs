//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*               tutorials group.ex - group_type.server.console              *
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
	static public group_array		 pgroup_a = new group_array();
	static public group_array_sender pgroup_b = new group_array_sender();
	static public group_list		 pgroup_c = new group_list();
	static public group_list_sender	 pgroup_d = new group_list_sender();

	public static int Main(String[] args)
	{
		// trace) 
		Console.WriteLine("CGCII tutorial.group.server.NET.ex01...");

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
