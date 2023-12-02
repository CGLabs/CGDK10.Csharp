//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*              tutorials socket - tcp_delegate.client.console               *
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
using CGDK;
using CGDK.net;
using CGDK.factory;

namespace tutorial.socket._02.tcp_delegate.client.console
{
	class Program
	{
		public static void Main(String[] args)
		{
			// trace) 
			Console.WriteLine("start client [tut.socket.02.tcp_delegate.client]...");

			try
			{
				// 1) Socke을 생성한다.
				var socket_client = new CGDK.net.socket.tcp_client();

				// 2) Delegator들을 설정
				socket_client.notify_on_request_connect  = new delegate_notify_connect(socket_on_request_connect);
				socket_client.notify_on_connect			 = new delegate_notify(socket_on_connect);
				socket_client.notify_on_fail_connect	 = new delegate_notify_int64(socket_on_fail_connect);
				socket_client.notify_on_disconnect		 = new delegate_notify_int64(socket_on_disconnect);
				socket_client.notify_on_message			 = new delegate_notify_message(socket_on_message);

				// 3) 접속을 시도한다.
				socket_client.start("localhost", 20000);

				// 4) ESC누를 때까지 대기 (ESC를 누를 때까지 기다린다.)
				while (Console.ReadKey().Key != ConsoleKey.Escape) ;

				// 5) Socket을 닫는다.
				socket_client.stop();
			}
			catch (Exception _e)
			{
				Console.WriteLine(_e.ToString());
			}

			// trace) 
			Console.WriteLine("stop client [tut.socket.02.tcp_delegate.client]...");
		}

		public static void socket_on_request_connect(object _source, IPEndPoint _remote_endpoint)
		{
			// trace) 
			Console.WriteLine(" @ request connect to " + _remote_endpoint.ToString());
		}

		private static void socket_on_connect(object _source)
		{
			// 1) 소켓을 얻는다.
			var psocket = _source as CGDK.net.socket.tcp_client;

			// trace) 
			Console.WriteLine(" @ connected");

			// 2) 버퍼를 할당받는다.
			var buf_send = memory.alloc_buffer(256);

			// 3) Message를 작성한다.
			buf_send.append<ushort>(30);
			buf_send.append<ushort>(0);
			buf_send.append<int>(10);
			buf_send.append<string>("TestValue");

			// 4) 메시지 헤더(메시지 길이)를 메시지 제일 앞에 덩ㅄ어 써넣는다.(중요!)
			buf_send.set_front<int>(buf_send.size);

			// 5) 접속한 대상에게 전송한다.
			psocket.send(buf_send);

			// trace) 
			Console.WriteLine(" @ send message");
		}
		private static void socket_on_fail_connect(object _source, ulong _disconnect_reason)
		{
			// trace) 
			Console.WriteLine(" @ fail to connect");
		}
		private static void socket_on_disconnect(object _source, ulong _disconnect_reason)
		{
			// trace) 
			Console.WriteLine(" @ disconnected");
		}

		private static int socket_on_message(object _source, sMESSAGE _msg)
		{
			// trace) 
			Console.WriteLine(" message received!");

			// return) 
			return 0;
		}
	}
}
