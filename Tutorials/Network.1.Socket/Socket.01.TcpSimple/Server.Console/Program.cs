//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*               tutorials socket - tcp_simple.server.console                *
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

// ----------------------------------------------------------------------------
//
// tutorial socket. - tcp simple
//
// tcp 서버를 작성하는 방법을 설명한다.
// tcp 서버란 접속을 받아 쪽을 말한다.
// 즉 특정 ip와 port에 listen을 해 접속 요청을 기다린다.
// 접속 요청이 들어오면 접속을 받은 후 송수신 처리를 한다.
//
//  1) CGDK.Net.Acceptor<T> 클래스는 접속을 받기 위해 listen을 하는 클래스다.
//  2) CGDK.Net.Acceptor<T> 에서 T는 접속을 받았을 경우 생성할 socket 클래스다.
//     즉 새로운 접속이 들어오면 T 객체를 생성해 접속처리를 해주며 T 객체의 OnConnect()
//     함수를 호출한다.
//     (여기서는 'SocketTcp' 클래스가 T 객체에 해당한다.)
//  3) T 클래스는 반드시 CGDK.Net.ISocketTcp를 상속받은 클래스여야 한다.
//  4) CGDK.Net.Acceptor<T> 객체를 생성하여 listen 포트를 전달해 Start()함수를
//     호출하면 접속을 받기 시작한다.
//  3) 해당 포트로 접속이 들어오면 접속 마다 T 객체를 생성하며 그 T객체의
//     OnConnect()함수를 호출해준다.
//  4) 메시지가 전송되어 올 경우 T 객체의 OnMessage()함수가 호출 된다.
//  5) 접속이 종료되면 되면 T객체의 OnDisconnect()함수가 호출된다.
//  6) T 객체의 send()함수를 호출해 접속된 상대에게 데이터를 전송할 수 있다. 
//  7) T 객체의 closesocket() 혹은 disconnect()함수를 호출하면 접속이 종료된다.
//  8) 따라서 T객체는 CGDK.Net.ISocketTcp 클래스를 상속받아
//     OnConnect(), OnDisconnect(), OnMessage()함수를 재정의하는 방법으로
//     서버를 작성한다.
// 
// ----------------------------------------------------------------------------
namespace tutorial_socket_01_tcp_simple_server_console
{
	class Program
	{
		[STAThread]
		public static void Main(String[] args)
		{
			// trace) 
			Console.WriteLine("Start server [tut.socket.01.tcp_simple.server for C#]...");

			try
			{
				// 1) acceptor를 생성한다.
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
			Console.WriteLine("Stop server [tut.socket.01.tcp_simple.server for C#]...");
		}


		public class SocketTcp : CGDK.Net.Socket.ITcp
		{
			// 1) 접속이 되었을 때 불려지는 함수.
			protected override void OnConnect()
			{
				Console.WriteLine(" @ connected");
			}

			// 2) 접속이 종료되었을 때 호출되는 함수.
			protected override void OnDisconnect(ulong _disconnect_reason)
			{
				Console.WriteLine(" @ disconnected");
			}

			// 3) Message를 받았을 때 불려지는 함수.
			protected override int OnMessage(object _source, sMESSAGE _msg)
			{
				Console.WriteLine(" @ message received and echo sended!");

				// 섦명)
				//  전송받은 메시지는 _msg 에 전달된다.
				//  _msg.bufMessage에 선송받음 데이터들이 전달된다.

				// 1) echo 전송
				this.Send(_msg.bufMessage);

				// Return) 
				return 1;
			}
		}
	}
}