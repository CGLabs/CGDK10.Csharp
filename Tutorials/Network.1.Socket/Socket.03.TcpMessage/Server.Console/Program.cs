//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*               tutorials socket - tcp_message.server.winform               *
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
// tutorial socket. - tcp message
//
// 메시지를 송신(send)하고 수신(receive)하는 다양한 방법들에 대해서 설명한다.
//
//  1) 메시지의 송신과 수신은 기본적으로 CGD.buffer를 사용한다.
//  2) 메시지가 전송되어 오면 OnMessage()함수에 전달되어 온다.
//  3) OnMessage()에서 전달되는 인자인 sMESSAGE의 bufMessage에 들어 있다.
//  4) 메시지를 전송하기 위해서는 send()함수를 사용한다.
//  5) send시 전달할 메시지 헤드 메시지 길이를 정확히 해주어야 한다!
//     (기본 메시지 헤더은 처음 4byte에 메시지 길이가 들어가며 헤더크기를 포함한
//      메시지 길이를 넣어주어야 한다.)
// 
// ----------------------------------------------------------------------------
namespace tutorial_socket_03_tcp_message_server_console
{
	class Program
	{
		public static void Main(String[] args)
		{
			// trace) 
			Console.WriteLine("Start server [tut.socket.02.tcp_message.server for C#]...");

			try
			{
				// 1) Acceptor를 생성한다.
				var acceptor_test = new CGDK.Net.Acceptor<SocketTcp>();

				// 2) 20000번 포트로 Listen을 시작한다.
				acceptor_test.Start(20000);

				// trace) 
				Console.WriteLine("@ server Start... (listen: {0})", acceptor_test.AcceptSocket.LocalEndPoint);

				// 3) ESC누를 때까지 대기 (ESC를 누를 때까지 기다린다.)
				while (Console.ReadKey().Key != ConsoleKey.Escape) ;

				// 4) Acceptor를 닫는다.
				acceptor_test.Stop();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			// trace) 
			Console.WriteLine("Stop server [tut.socket.02.tcp_message.server for C#]...");
		}
	}
}
