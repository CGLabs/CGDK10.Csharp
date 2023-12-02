//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                  tutorials group - simple.server.console                  *
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

//----------------------------------------------------------------------------
//  
//  simple group
//  
//  1. group이란?
//     
//
//  2. group을 사용하기 위해서
//     
//
//----------------------------------------------------------------------------
namespace TutorialGroupServer
{
	class Program
	{
		static public GroupSimple pgroup;

		public static int Main()
		{
			// trace) 
			Console.WriteLine("CGCII tutorial.group.server.NET 01...");

			// 1) group 객체를 생성한다 / create group object
			pgroup = new()
			{
				// - group에 입장 가능하게 설정한다 / enable entering of group object
				EnableEnter = true
			};

			// 2) acceptor를 생성한다. / create acceptor object
			var pacceptor = new CGDK.Net.Acceptor<SocketTcp>();

			// 3) 20000번 포트에 Listen을 시작한다. / listen on 20000 port
			pacceptor.Start(new IPEndPoint(IPAddress.Loopback, 20000));

			// 4) ESC누를 때까지 대기 (ESC를 누를 때까지 기다린다.)
			while (Console.ReadKey().Key != ConsoleKey.Escape) ;

			// 5) acceptor를 닫는다. / close acceptor
			pacceptor.Stop();

			// trace) 
			return 0;
		}
	}

}