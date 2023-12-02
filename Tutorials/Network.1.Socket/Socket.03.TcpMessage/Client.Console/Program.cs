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
using CGDK;

namespace tutorial_socket_03_tcp_message_client_console
{
	class Program
	{
		static void Main()
		{
			// trace) 
			Console.WriteLine("Start client [tut.socket.03.tcp_message.client]...");

			try
			{
				// 1) Socke을 생성한다.
				var socket_client = new SocketTcp();
			
				// 2) 접속을 시도한다.
				socket_client.Start("localhost", 20000);

				// 3) ESC누를 때까지 대기 (ESC를 누를 때까지 기다린다.)
				while(true)
				{
					var key = Console.ReadKey().Key;

					if(key == ConsoleKey.Escape)
					{
						socket_client.CloseSocket();
						break;
					}

					switch(key)
					{
					case	ConsoleKey.A:
							{

							}
							break;

					case	ConsoleKey.B:
							{

							}
							break;

					case	ConsoleKey.C:
							{

							}
							break;

					case	ConsoleKey.D:
							{

							}
							break;

					case	ConsoleKey.E:
							{

							}
							break;

					case	ConsoleKey.F:
							{

							}
							break;

					case	ConsoleKey.G:
							{

							}
							break;

					case	ConsoleKey.Z:
							socket_client.Disconnect();
							break;

					case	ConsoleKey.X:
							socket_client.CloseSocket();
							break;

						default:
							break;
					}
				}
			}
			catch (Exception _e)
			{
				Console.WriteLine(_e.ToString());
			}

			// trace) 
			Console.WriteLine("Stop client [tut.socket.02.tcp_message.client]...");
		}
	}
}
