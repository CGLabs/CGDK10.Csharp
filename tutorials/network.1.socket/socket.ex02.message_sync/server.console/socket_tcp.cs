//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*               tutorials socket - message_sync.server.console              *
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
using CGDK.net;

public class socket_tcp : CGDK.net.socket.tcp_sync
{
	public socket_tcp()
	{
		m_count_message		 = 0;
		notify_on_connect	 = new delegate_notify(socket_on_connect);
		notify_on_disconnect = new delegate_notify_int64(socket_on_disconnect);
		notify_on_message	 = new delegate_notify_message(socket_on_message);
	}

	// 1) 접속이 되었을 때 불려지는 함수.
	public void socket_on_connect(object _source)
	{
		Console.WriteLine(" @ connected");
	}
	// 2) 접속이 종료되었을 때 호출되는 함수.
	public void socket_on_disconnect(object _source, ulong _disconnect_reason)
	{
		Console.WriteLine(" @ disconnected");
	}
	// 3) Message를 받았을 때 불려지는 함수.
	public int socket_on_message(object _source, sMESSAGE _msg)
	{
		Console.WriteLine(" @ message received and echo sended!");

		// 1) 받은 Message 갯수 증가.
		Interlocked.Increment(ref m_count_message);

		// 2) Echo 전송
		send(_msg.buf_message);

		// Return) 
		return	1;
	}

	// 4) 전송받은 Message 수
	public static int	m_count_message	 = 0;
}
