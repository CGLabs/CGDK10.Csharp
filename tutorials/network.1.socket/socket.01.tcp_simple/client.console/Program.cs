//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*               tutorials socket - tcp_simple.client.console                *
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

// ----------------------------------------------------------------------------
//
// tutorial socket - tcp_simple.client
//
// tcp 클라이언트를 작성하는 방법을 설명한다.
// tcp 클라이언트란 접속을 요청하는 쪽을 의미한다.
// tcp 클라이언트는 접속을 하고자하는 ip와 port로 접속(connect)한다.
// 
//  1) CGDK.net.Isocket_tcp_client를 상속받아 socket 클래스를 정의한다.
//  2) CGDK.net.Isocket_tcp_client는 서버용 CGDK.net.Isocket_tcp에 connect()기능을
//     추가한 클래스이다.
//     (여기서는 'socket_tcp' 클래스가 T 객체에 해당한다.)
//  3) T 클래스는 반드시 
//  4) CGDK.net.acceptor<T> 객체를 생성하여 listen 포트를 전달해 start()함수를
//     호출하면 접속을 받기 시작한다.
//  3) 해당 포트로 접속이 들어오면 접속 마다 T 객체를 생성하며 그 T객체의
//     on_connect()함수를 호출해준다.
//  4) 메시지가 전송되어 올 경우 T 객체의 on_message()함수가 호출 된다.
//  5) 접속이 종료되면 되면 T객체의 on_disconnect()함수가 호출된다.
//  6) T 객체의 send()함수를 호출해 접속된 상대에게 데이터를 전송할 수 있다. 
//  7) T 객체의 closesocket() 혹은 disconnect()함수를 호출하면 접속이 종료된다.
//  8) 따라서 T객체는 CGDK.net.Isocket_tcp 클래스를 상속받아
//     on_connect(), on_disconnect(), on_message()함수를 재정의하는 방법으로
//     서버를 작성한다.
//
// ----------------------------------------------------------------------------

public class tutorial_socket_01_client
{
    public static void Main(String[] args)
    {
		// trace) 
		Console.WriteLine("start client [tut.socket.01.tcp_simple.client]...");

		try
		{
			// 1) Socke을 생성한다.
            var socket_client = new socket_tcp();

			var x = Dns.Resolve("localhost");

			// 2) 접속을 시도한다.
			//socket_client.start(new IPEndPoint(, 20000);

			// 3) ESC누를 때까지 대기 (ESC를 누를 때까지 기다린다.)
			while(Console.ReadKey().Key != ConsoleKey.Escape); 

			// 4) Socket을 닫는다.
			socket_client.stop();
        }
        catch (Exception _e)
        {
            Console.WriteLine(_e.ToString());
        }

		// trace) 
		Console.WriteLine("stop client [tut.socket.01.tcp_simple.client]...");
    }

	public class socket_tcp : CGDK.net.socket.Itcp_client
	{
		// 1) 접속이 요청되었을 때 불려지는 함수.
		protected override void on_request_connect(IPEndPoint _remote_endpoint)
		{
			// trace) 
			Console.WriteLine(" @ request connect to " + _remote_endpoint.ToString());
		}
		// 2) 접속이 되었을 때 불려지는 함수.
		protected override void on_connect()
		{
			// trace) 
			Console.WriteLine(" @ connected");

			// - 버퍼를 할당받는다.
			var buf_send = memory.alloc_buffer(256);

			// - Message를 작성한다.
			buf_send.append<ushort>(30);
			buf_send.append<ushort>(0);
			buf_send.append<int>(10);
			buf_send.append<string>("TestValue");

			// - 메시지 헤더(메시지 길이)를 메시지 제일 앞에 덩ㅄ어 써넣는다.(중요!)
			buf_send.set_front<int>(buf_send.size);

			// - 접속한 대상에게 전송한다.
			send(buf_send);

			// trace) 
			Console.WriteLine(" @ send message");
		}
		// 3) 접속이 실패했을 때 불려지는 함수.
		protected override void on_fail_connect(ulong _disconnect_reason)
		{
			// trace) 
			Console.WriteLine(" @ fail to connect");
		}
		// 4) 접속이 종료되었을 때 호출되는 함수.
		protected override void on_disconnect(ulong _disconnect_reason)
		{
			// trace) 
			Console.WriteLine(" @ disconnected");
		}
		// 5) message를 받았을 때 호출되는 함수.
		protected override int on_message(object _source, sMESSAGE _msg)
		{
			// - 데이터를 읽기 위해 임시 버퍼로 복사
			var buf_read = _msg.buf_message;

			// - 데이터를 읽어낸다.
			ushort size		 = buf_read.extract_ushort();
			ushort message	 = buf_read.extract_ushort();
			int value		 = buf_read.extract_int();
			string strValue	 = buf_read.extract_string();

			// trace) 
			Console.WriteLine(" @ receive message (" + "Size:" + size + " Message:" + message + " Value:" + value + " String:" + strValue + ")");

			// Return) 
			return 1;
		}
	}
}
