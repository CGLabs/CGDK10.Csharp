//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*              tutorials socket - tcp_reconnect.server.console              *
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
using CGDK;
using CGDK.net;

namespace tutorial_socket_03_tcp_reconnect_client_console
{
	class Program
	{
		public static void Main(String[] args)
		{
			// trace) 
			Console.WriteLine("start client [tut.socket.03.tcp_reconnect.client]...");

			try
			{
				// 1) Socke을 생성한다.
				var socket_client = new socket_tcp();

				// 2) 접속을 하기 전에 재접속을 enable하고 재접속 시도 시간을 설정한다.
				socket_client.enable_reconnection = true; // enable
				socket_client.reconnection_interval = 10; // 10초

				// 3) 접속을 시도한다.
				socket_client.start("localhost", 20000);

				// 4) key 입력을 처리한다.
				while(true)
				{
					// - 입력받은 키를 얻는다.
					var key = Console.ReadKey().Key;

					// check) ESC키면 끝낸다.
					if(key == ConsoleKey.Escape)
					{
						socket_client.closesocket();
						break;
					}

					// - key 입력에 따라 처리한다.
					switch(key)
					{
					case	ConsoleKey.Z:
							{
								// - 접속을 종료하기전 재접속 기능을 끈다! (아니면 접속 종료 후 재접속을 시도한다.)
								socket_client.enable_reconnection = false; // enable

								// - 접속을 종료한다.
								socket_client.disconnect();
							}
							break;

					case	ConsoleKey.X:
							{
								// - 접속을 종료하기전 재접속 기능을 끈다! (아니면 접속 종료 후 재접속을 시도한다.)
								socket_client.enable_reconnection = false; // enable

								// - 접속을 종료한다.
								socket_client.closesocket();
							}
							break;

					case	ConsoleKey.C:
							{
								// - 접속을 하기 전에 재접속을 enable하고 재접속 시도 시간을 설정한다.
								socket_client.enable_reconnection = true; // enable
								socket_client.reconnection_interval = 10; // 10초

								// - 접속한다.
								socket_client.start("localhost", 20000);
							}
							break;

					default:
							break;
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			// trace) 
			Console.WriteLine("stop client [tut.socket.03.tcp_reconnect.client]...");
		}
	}
}
