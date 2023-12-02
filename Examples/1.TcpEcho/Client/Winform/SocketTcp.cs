//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                     sample - tcp_echo.client.winform                      *
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

using CGDK;
using Sample.TcpEcho.Client.Winform;

// ----------------------------------------------------------------------------
//
// SocketTcp
//
// ----------------------------------------------------------------------------
namespace Sample.TcpEcho.Client.Winform
{
	public class SocketTcp : CGDK.Net.Socket.TcpClient
	{
		public SocketTcp()
		{
			this.NotifyOnConnect = new(TestTcpEchoClient.OnSocketConnect);
			this.NotifyOnFailConnect = new(TestTcpEchoClient.OnSocketFailConnect);
			this.NotifyOnDisconnect = new(TestTcpEchoClient.OnSocketDisconnect);
			this.NotifyOnMessage = new(TestTcpEchoClient.OnSocketMessage);
		}
	}
}