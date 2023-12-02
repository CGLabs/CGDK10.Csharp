//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                   sample - tcp_echo.server.core.console                   *
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

// ----------------------------------------------------------------------------
//
// SocketTcp
//
// ----------------------------------------------------------------------------
public class SocketTcp : CGDK.Net.Socket.ITcp
{
	protected override int OnMessage(object _source, sMESSAGE _msg)
	{
		// 1) echo Send
		this.Send(_msg.bufMessage);

		// return) 
		return 1;
	}
}
