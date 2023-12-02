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
using CGDK.net;

public class socket_tcp : CGDK.net.socket.Itcp_client
{
    protected override void on_request_connect(IPEndPoint _remote_ep)
	{
		// trace) 
        Console.WriteLine(" @ request connect to "+_remote_ep.ToString());
	}

    protected override void on_connect()
	{
		// trace) 
        Console.WriteLine(" @ connected");

		// 1) Buffer를 할당받는다.
		var temp_buffer = new CGDK.buffer(CGDK.factory.memory.alloc_buffer(256));
			
		// 2) Message를 작성한다.
		temp_buffer.append<uint>();
		temp_buffer.append<uint>(0);
		temp_buffer.append<int>(10);
		temp_buffer.append<string>("test value");
		temp_buffer.set_front<int>(temp_buffer.size);
		
		// 3) 전송한다.
		send(temp_buffer);

		// trace) 
		Console.WriteLine(" @ message sended (size:" + temp_buffer.size + " message:" + temp_buffer.get_front<uint>(4) + ")");

	}
    protected override void on_fail_connect(ulong _disconnect_reason)
	{
		// trace) 
        Console.WriteLine(" @ fail to connect");
	}
    protected override void on_disconnect(ulong _disconnect_reason)
	{
		// trace) 
        Console.WriteLine(" @ disconnected");
	}

    protected override int on_message(object _source, sMESSAGE _msg)
	{
		// return) 
		return	0;
	}
}
