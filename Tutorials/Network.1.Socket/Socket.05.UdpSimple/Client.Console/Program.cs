using System;
using System.Net;
using System.Threading;
using CGDK;


namespace tutorial_socket._05_udp_simple_client_console
{
	class Program
	{
		public static void Main()
		{
			// trace) 
			Console.WriteLine("Start client [tut.socket.05.udp_simple.client]...");

			try
			{
				// 1*) ExecutableQueue를 생성한다.
				var temp_queue = new ExecutableQueue();

				// 2) Socke을 생성한다.
				var udp_socket = new CGDK.Net.Socket.UdpSync
				{
					// - delegate 설정
					NotifyOnMessage = new(Udp_OnMessage),

					// *- io_queue를 샐정한다!
					io_queue = temp_queue
				};

				// 3) bind한다.
				udp_socket.Start();

				// 4) key 입력을 처리한다.
				while(true)
				{
					// (*) ExecutableQueue를 실행해준다.
					//     - 큐잉된 socket 관련 모든 i/o를 이 함수에서 처리해 준다.
					//     - 따라서 일정 시간마다 주기적으로 실행해 주어야 한다.
					temp_queue.Execute();

					// - 키입력을 확인한다.
					if(Console.KeyAvailable)
					{
						// - 입력받은 키를 얻는다.
						var key = Console.ReadKey().Key;

						// check) ESC키면 끝낸다.
						if(key == ConsoleKey.Escape)
						{
							udp_socket.CloseSocket();
							break;
						}

						// - 일반적 입력에 대한 처리
						switch(key)
						{
						case	ConsoleKey.A:
								{
									var buf_Send = CGDK.Factory.Memory.AllocBuffer(100);
									buf_Send.Append<int>(100);
									buf_Send.Append<int>(100);
									buf_Send.Append<string>("test_a");

									udp_socket.SendTo(buf_Send, new IPEndPoint(new IPAddress(new byte[]{ 127, 0, 0, 1 }), 21000));
								}
								break;

						case	ConsoleKey.B:
								{
									var buf_Send = CGDK.Factory.Memory.AllocBuffer(100);
									buf_Send.Append<int>(100);
									buf_Send.Append<int>(100);
									buf_Send.Append<string>("test_b");

									udp_socket.SendTo(buf_Send, new IPEndPoint(new IPAddress(new byte[]{ 127, 0, 0, 1 }), 21000));
								}
								break;

							default:
								break;
						}
					}

					// - 1ms만큼 잔다.
					Thread.Sleep(1);
				}

				// 5) Acceptor를 닫는다.
				udp_socket.Stop();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			// trace) 
			Console.WriteLine("Stop client [tut.socket.05.udp_simple.client]...");
		}

		public static int Udp_OnMessage(object _object, sMESSAGE _msg)
		{
			Console.WriteLine(" @ datagram message received!");

			// return) 
			return 1;
		}
	}
}
