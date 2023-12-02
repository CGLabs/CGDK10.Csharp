//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*              tutorials socket - tcp_restorable.server.console             *
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

public class SocketTcp : CGDK.Net.Socket.TcpClient
{
	public SocketTcp()
	{
		this.NotifyOnConnect = new(socket_OnConnect);
		this.NotifyOnDisconnect = new(socket_OnDisconnect);
		this.NotifyOnMessage = new(socket_OnMessage);
	}

	// 1) 접속이 되었을 때 불려지는 함수.
	public void socket_OnConnect(object _source)
	{
		Console.WriteLine(" @ connected");
	}
	// 2) 접속이 종료되었을 때 호출되는 함수.
	public void socket_OnDisconnect(object _source, ulong _disconnect_reason)
	{
		Console.WriteLine(" @ disconnected");
	}
	// 3) Message를 받았을 때 불려지는 함수.
	public int socket_OnMessage(object _source, sMESSAGE _msg)
	{
		Console.WriteLine(" @ message received and echo sended!");

		// 1) 받은 Message 갯수 증가.
		Interlocked.Increment(ref m_count_message);

		// 2) Echo 전송
		this.Send(_msg.bufMessage);

		// Return) 
		return 1;
	}

	// 4) 전송받은 Message 수
	public static int	m_count_message	 = 0;
}
