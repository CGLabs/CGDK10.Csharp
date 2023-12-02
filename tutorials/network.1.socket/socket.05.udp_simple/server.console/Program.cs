using System;
using System.Net;

namespace tutorial.socket._05.udp_simple.server.console
{
	class Program
	{
		static void Main(string[] args)
		{
			// trace) 
			Console.WriteLine("start server [tut.socket.05.udp.server for C#]...");

			try
			{
				// 1) Acceptor를 생성한다.
				var temp_socket = new socket_udp();

				// 2) 20000번 포트로 bind한다.
				temp_socket.start(new IPEndPoint(IPAddress.Any, 21000));

				// 3) ESC누를 때까지 대기 (ESC를 누를 때까지 기다린다.)
				while (Console.ReadKey().Key != ConsoleKey.Escape) ;

				// 4) Acceptor를 닫는다.
				temp_socket.stop();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			// trace) 
			Console.WriteLine("stop server [tut.socket.05.udp.server for C#]...");
		}
	}
}
