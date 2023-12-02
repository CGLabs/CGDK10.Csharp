//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*               tutorials socket - tcp_message.server.winform               *
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
		// 설명)
		// _msg의 bufMessage에 송신받은 메시지 데이터가 들어 있다.

		// - Message 읽기를 위해 buffer를 얕은 복사 한다.
		var bug_recv = _msg.bufMessage;

		// 설명)
		// _msge.bufMessage에 전달되는 데이터는 메시지 헤더까지 포함한 데이터이므로 
		// 그것을 감안하여 값을 읽어주어야 한다.
		//
		// CGD.buffer의 Extract<T> 함수
		//  -> 버퍼의 앞에서 T타입으로 값을 읽어오며 읽어낸 만큼 포인터를 옮기고 버퍼 길이를 줄인다.
		//
		// CGD.buffer의 get_front<T>() 함수
		//  -> 버퍼의 앞에서 T 타입으로 값을 읽어 온다. (이때 offset을 인자로 전달할 수도 있다.)
		//     읽어내더라도 포인터를 옮기거나 버퍼 길이를 변경하지 않는다. 말 그대로 읽어만 온다.

		// - get_front<T>()함수로 message header를 읽는다.
		var message = bug_recv.GetFront<MESSAGE_struct.HEADER>();

		// - message 종류에 따라 분기한다.
		switch (message.message_type)
		{
		case	eMESSAGE_TYPE.A:
				{
					// - eMESSAGE_TYPE.A에 대당하는 구조체인 MESSAGE_struct.A형으로 Extract한다.
					var msg = bug_recv.Extract<MESSAGE_struct.A>();

					Console.WriteLine(" @ MessageType.A Received");
				}
				break;

		case	eMESSAGE_TYPE.B:
				{
					// - eMESSAGE_TYPE.B에 대당하는 구조체인 MESSAGE_struct.B형으로 Extract한다.
					var msg = bug_recv.Extract<MESSAGE_struct.B>();

					Console.WriteLine(" @ MessageType.B Received");
				}
				break;

		case	eMESSAGE_TYPE.C:
				{
					// - eMESSAGE_TYPE.C에 대당하는 구조체인 MESSAGE_struct.C형으로 Extract한다.
					var msg = bug_recv.Extract<MESSAGE_struct.C>();

					Console.WriteLine(" @ MessageType.C Received");
				}
				break;

		case	eMESSAGE_TYPE.D:
				{
					// - eMESSAGE_TYPE.D에 대당하는 구조체인 MESSAGE_struct.D형으로 Extract한다.
					var msg = bug_recv.Extract<MESSAGE_struct.D>();

					Console.WriteLine(" @ MessageType.D Received");
				}
				break;

		case	eMESSAGE_TYPE.E:
				{
					// - eMESSAGE_TYPE.E에 대당하는 구조체인 MESSAGE_struct.E형으로 Extract한다.
					var msg = bug_recv.Extract<MESSAGE_struct.E>();

					Console.WriteLine(" @ MessageType.E Received");
				}
				break;

		case	eMESSAGE_TYPE.F:
				{
					// - eMESSAGE_TYPE.F에 대당하는 구조체인 MESSAGE_struct.F형으로 Extract한다.
					var msg = bug_recv.Extract<MESSAGE_struct.F>();

					Console.WriteLine(" @ MessageType.F Received");
				}
				break;

		case	eMESSAGE_TYPE.G:
				{
					// - eMESSAGE_TYPE.G에 대당하는 구조체인 MESSAGE_struct.G형으로 Extract한다.
					var msg = bug_recv.Extract<MESSAGE_struct.G>();

					Console.WriteLine(" @ MessageType.G Received");
				}
				break;

		default:
				Console.WriteLine(" ! unhandled message");
				break;
		}

		// 2) Echo 전송한다.
		this.Send(_msg.bufMessage);

		// Return) 
		return 1;
	}

	// 4) 전송받은 Message 수
	public static int	m_count_message	 = 0;
}
