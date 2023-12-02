//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                 tutorials group - chatting.client.console                 *
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

public class SocketTcp : CGDK.Net.Socket.ITcpClient
{
    protected override void OnRequestConnect(IPEndPoint _remote_ep)
	{
		// trace) 
        Console.WriteLine(" @ request connect to "+_remote_ep.ToString());
	}

    protected override void OnConnect()
	{
		// trace) 
        Console.WriteLine(" @ connected");

		// 1) Buffer를 할당받는다.
		var temp_buffer = new CGDK.buffer(CGDK.Factory.Memory.AllocBuffer(256));
			
		// 2) Message를 작성한다.
		temp_buffer.Append<uint>();
		temp_buffer.Append<uint>(0);
		temp_buffer.Append<int>(10);
		temp_buffer.Append<string>("test value");
		temp_buffer.SetFront<int>(temp_buffer.Count);
		
		// 3) 전송한다.
		this.Send(temp_buffer);

		// trace) 
		Console.WriteLine(" @ message Sended (size:" + temp_buffer.Count + " message:" + temp_buffer.GetFront<uint>(4) + ")");

	}
    protected override void OnFailConnect(ulong _disconnect_reason)
	{
		// trace) 
        Console.WriteLine(" @ fail to connect");
	}
    protected override void OnDisconnect(ulong _disconnect_reason)
	{
		// trace) 
        Console.WriteLine(" @ disconnected");
	}

    protected override int OnMessage(object _source, sMESSAGE _msg)
	{
		// return) 
		return 0;
	}
}
