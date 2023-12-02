//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*              tutorials socket - tcp_restorable.server.console             *
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
using System.Threading;
using CGDK;


// ----------------------------------------------------------------------------
//
// Tutorial 3. TCP Echo Test Client with Sync (1)
//
// Socket의 Connect/Disconnect/Receive 처리 등을 내부적인 쓰레드가 아니라 특정
// 쓰레드에서 실행해야 하는 경우가 있을 수 있다.
// 이럴 경우 처리해야할 Socket의 I/O 이벤트들을 Queuing하였다가 원하는 시점에 실행
// 할 수 있또록 해준다.
// 사용방법은 다음과 같다.
// 
//  1. CGDK.net.SocketTcp_client 대신 CGDK.net.SocketTcp_client_sync를
//     상속받아 소켓을 정의한다.
//  2. Socket I/O 이벤트를 Qeueuing할 ExecutableQueue를 설정한다.
//     이것은 io_queue 프라퍼티를 사용하여 값을 얻고 설정할 수 있다.
//     만약 아무것도 설정하지 않으면 전역 기본 ExecutableQueue가 설정된다.
//
//  3. 설정한 ExecutableQueue를 Socket의 I/O를 처리하는 시점에서 Execute()함수를
//     호출해 준다.
//     기본 ExecutableQueue를 사용한 경우 (즉 설정하지 않은 경우) 
//     ExecutableQueue.Default 의 Execute() 함수를 호출해 준다.
//
//  나머지 사용법은 CGDK.net.SocketTcp_client와 동일하다.
//  아래 예제는 전역인 기본 ExecutableQueue를 사용한 예제이다.
// 
// ----------------------------------------------------------------------------
namespace tutorial_socket_04_tcp_restorable_client_console
{
	class Program
	{
		public static void Main()
		{
			// trace) 
			Console.WriteLine("Start client [tut.socket.04.tcp_restorable.client]...");

			try
			{
				// 1) Socke을 생성한다.
				var socket_client = new SocketTcp();

				// 2) 접속을 시도한다.
				socket_client.Start("localhost", 20000);

				// 3) ESC누를 때까지 대기 (ESC를 누를 때까지 기다린다.)
				while(true)
				{
					// - ESC키를 누르면 빠져나간다.
					if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
					{
						break;
					}

					// - 1ms만큼 잔다.
					Thread.Sleep(1);
				}

				// 4) Socket을 닫는다.
				socket_client.Stop();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			// trace) 
			Console.WriteLine("Stop client [tut.socket.04.tcp_restorable.client]...");
		}
	}
}
