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
using sample.tcp_echo.client.winform;

// ----------------------------------------------------------------------------
//
// socket_tcp
//
// ----------------------------------------------------------------------------
public class socket_tcp : CGDK.net.socket.tcp_client
{
	public socket_tcp()
	{
		notify_on_connect		 = new delegate_notify			(test_tcp_echo_client.on_socket_connect);
		notify_on_fail_connect	 = new delegate_notify_int64	(test_tcp_echo_client.on_socket_fail_connect);
		notify_on_disconnect	 = new delegate_notify_int64	(test_tcp_echo_client.on_socket_disconnect);
		notify_on_message		 = new delegate_notify_message	(test_tcp_echo_client.on_socket_message);
	}
}
