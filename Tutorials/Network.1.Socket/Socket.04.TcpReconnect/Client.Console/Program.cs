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

namespace tutorial_socket_03_tcp_reconnect_client_console
{
	class Program
	{
		public static void Main()
		{
			// trace) 
			Console.WriteLine("Start client [tut.socket.03.tcp_reconnect.client]...");

			try
			{
				// 1) Socke을 생성한다.
				var socket_client = new SocketTcp
				{
					// 2) 접속을 하기 전에 재접속을 enable하고 재접속 시도 시간을 설정한다.
					EnableReconnection = true, // enable
					ReconnectionInterval = 10 // 10초
				};

				// 3) 접속을 시도한다.
				socket_client.Start("localhost", 20000);

				// 4) key 입력을 처리한다.
				while(true)
				{
					// - 입력받은 키를 얻는다.
					var key = Console.ReadKey().Key;

					// check) ESC키면 끝낸다.
					if(key == ConsoleKey.Escape)
					{
						socket_client.CloseSocket();
						break;
					}

					// - key 입력에 따라 처리한다.
					switch(key)
					{
					case	ConsoleKey.Z:
							{
								// - 접속을 종료하기전 재접속 기능을 끈다! (아니면 접속 종료 후 재접속을 시도한다.)
								socket_client.EnableReconnection = false; // enable

								// - 접속을 종료한다.
								socket_client.Disconnect();
							}
							break;

					case	ConsoleKey.X:
							{
								// - 접속을 종료하기전 재접속 기능을 끈다! (아니면 접속 종료 후 재접속을 시도한다.)
								socket_client.EnableReconnection = false; // enable

								// - 접속을 종료한다.
								socket_client.CloseSocket();
							}
							break;

					case	ConsoleKey.C:
							{
								// - 접속을 하기 전에 재접속을 enable하고 재접속 시도 시간을 설정한다.
								socket_client.EnableReconnection = true; // enable
								socket_client.ReconnectionInterval = 10; // 10초

								// - 접속한다.
								socket_client.Start("localhost", 20000);
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
			Console.WriteLine("Stop client [tut.socket.03.tcp_reconnect.client]...");
		}
	}
}
