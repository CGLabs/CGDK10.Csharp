using System;
using System.Net;
using System.Threading;
using CGDK;
using CGDK.execute;


namespace tutorial.socket._05.udp_simple.client.console
{
	class Program
	{
		public static void Main(String[] args)
		{
			// trace) 
			Console.WriteLine("start client [tut.socket.05.udp_simple.client]...");

			try
			{
				// 1*) ExecutableQueue를 생성한다.
				var temp_queue = new executable_queue();

				// 2) Socke을 생성한다.
				var udp_socket = new CGDK.net.socket.udp_sync();

				// 3) delegate 설정
				udp_socket.notify_on_message = new delegate_notify_message(udp_on_message);

				// 4*) io_queue를 샐정한다!
				udp_socket.io_queue = temp_queue;

				// 5) bind한다.
				udp_socket.start();

				// 6) key 입력을 처리한다.
				while(true)
				{
					// (*) executable_queue를 실행해준다.
					//     - 큐잉된 socket 관련 모든 i/o를 이 함수에서 처리해 준다.
					//     - 따라서 일정 시간마다 주기적으로 실행해 주어야 한다.
					temp_queue.execute();

					// - 키입력을 확인한다.
					if(Console.KeyAvailable)
					{
						// - 입력받은 키를 얻는다.
						var key = Console.ReadKey().Key;

						// check) ESC키면 끝낸다.
						if(key == ConsoleKey.Escape)
						{
							udp_socket.closesocket();
							break;
						}

						// - 일반적 입력에 대한 처리
						switch(key)
						{
						case	ConsoleKey.A:
								{
									var buf_send = CGDK.factory.memory.alloc_buffer(100);
									buf_send.append<int>(100);
									buf_send.append<int>(100);
									buf_send.append<string>("test_a");

									udp_socket.send_to(buf_send, new IPEndPoint(new IPAddress(new byte[]{ 127, 0, 0, 1 }), 21000));
								}
								break;

						case	ConsoleKey.B:
								{
									var buf_send = CGDK.factory.memory.alloc_buffer(100);
									buf_send.append<int>(100);
									buf_send.append<int>(100);
									buf_send.append<string>("test_b");

									udp_socket.send_to(buf_send, new IPEndPoint(new IPAddress(new byte[]{ 127, 0, 0, 1 }), 21000));
								}
								break;

							default:
								break;
						}
					}

					// - 1ms만큼 잔다.
					Thread.Sleep(1);
				}

				// 7) Acceptor를 닫는다.
				udp_socket.stop();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			// trace) 
			Console.WriteLine("stop client [tut.socket.05.udp_simple.client]...");
		}

		public static int udp_on_message(object _object, sMESSAGE _msg)
		{
			Console.WriteLine(" @ datagram message received!");

			// return) 
			return 1;
		}
	}
}
