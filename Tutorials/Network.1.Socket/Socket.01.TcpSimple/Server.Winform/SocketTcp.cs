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

public class SocketTcp : CGDK.Net.Socket.ITcpClient
{
	// 1) 접속이 되었을 때 불려지는 함수.
	protected override void OnConnect()
	{
		// trace) 
		Console.WriteLine(" @ connected");

		// - 버퍼를 할당받는다.
		var buf_send = CGDK.Factory.Memory.AllocBuffer(256);

		// - Message를 작성한다.
		buf_send.Append<ushort>(30);
		buf_send.Append<ushort>(0);
		buf_send.Append<int>(10);
		buf_send.Append<string>("TestValue");

		// - 메시지 헤더(메시지 길이)를 메시지 제일 앞에 덩ㅄ어 써넣는다.(중요!)
		buf_send.SetFront<int>(buf_send.Count);

		// - 접속한 대상에게 전송한다.
		this.Send(buf_send);

		// trace) 
		Console.WriteLine("@ send message");
	}
	// 2) 접속이 종료되었을 때 호출되는 함수.
	protected override void OnDisconnect(ulong _disconnect_reason)
	{
		// trace) 
		Console.WriteLine(" @ disconnected");
	}

	// 3) message를 받았을 때 호출되는 함수.
	protected override int OnMessage(object _source, sMESSAGE _msg)
	{
		// - 데이터를 읽기 위해 임시 버퍼로 복사
		var buf_read = _msg.bufMessage;

		// - 데이터를 읽어낸다.
		var size	 = buf_read.Extract<ushort>();
		var message  = buf_read.Extract<ushort>();
		var value	 = buf_read.Extract<int>();
		var strValue = buf_read.Extract<string>();

		// trace) 
		Console.WriteLine(" @ receive message (" + "Size:" + size + " Message:" + message + " Value:" + value + " String:" + strValue + ")");

		// Return) 
		return 1;
	}
}
