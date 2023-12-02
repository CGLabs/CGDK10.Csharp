//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*               tutorials socket - message_sync.server.console              *
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
using CGDK.net;
using CGDK.execute;

// ----------------------------------------------------------------------------
//
// Tutorial 3. TCP Echo Test Client with Sync (2)
//
// Tutorial 3-(1)과 같은 내용이지만 기본 ExecutorQueue가 아닌 자체 생성한 Executable
// 을 사용한 예제이다.
// Tutorial 3-(1)과 아래와 같은 내용이 달라졌다.
//
//  50: CExecutableQueuer객체를 생성했다.
//  66: Socket에 io_queue에 생성한 CExecutableQueuer객체를 설정해 주었다.
//  75: executable_queue.Default대신 생성한  io_queue에 직접 생성해 설정한
//      CExecutableQueuer객체의 Execute()함수를 호추했다.
// 
// ----------------------------------------------------------------------------
namespace tutorial_socket_ex02_message_sync_client_console
{
	class Program
	{
	   public static void Main(String[] args)
		{
			// trace) 
			Console.WriteLine("start client [tut.socket.ex02.message_sync_client.client]...");

			try
			{
				// 1*) ExecutableQueue를 생성한다.
				var temp_queue = new executable_queue();

				// 2) Socke을 생성한다.
				var socket_client = new socket_tcp();

				// 3*) io_queue를 샐정한다!
				socket_client.io_queue = temp_queue;

				// 4) 접속을 시도한다.
				socket_client.start("localhost", 20000);

				// 5) ESC누를 때까지 대기 (ESC를 누를 때까지 기다린다.)
				while(true)
				{
					// (*) 생성한 ExecutableQueue를 실행해준다.
					temp_queue.execute();

					// - ESC키를 누르면 빠져나간다.
					if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
					{
						break;
					}

					// - 1ms만큼 잔다.
					Thread.Sleep(1);
				}

				// 6) Socket을 닫는다.
				socket_client.stop();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			// trace) 
			Console.WriteLine("stop client [tut.socket.ex02.message_sync_client.client]...");
		}
	}
}
