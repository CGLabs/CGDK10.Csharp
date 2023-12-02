//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                   sample - tcp_echo.server.core.winform                   *
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
using CGDK.net.io;
using sample.tcp_echo.server.winform;

// ----------------------------------------------------------------------------
//
// test_tcp_echo_server
//
// ----------------------------------------------------------------------------
class test_tcp_echo_server
{
	// 1) Bind Port (Default:20000)
	public static int bind_port = 20000;

	// 2) Acceptor
	private	static CGDK.net.acceptor<socket_tcp> m_acceptor_test;

	public static void InitTest()
	{
		// 1) Acceptor를 생성한다.
		m_acceptor_test = new CGDK.net.acceptor<socket_tcp>("acceptor");

		// 2) Acceptor의 Delegator를 설정한다.
		m_acceptor_test.notify_on_starting = new delegate_notify_context(on_acceptor_start);
		m_acceptor_test.notify_on_stopping = new delegate_notify(on_acceptor_stop);
	}

	public static void CloseTest()
	{
		request_stop_test();
	}

	public static void RequestStartTest()
	{
		// 1) Acceptor를 처리할 Thread를 생성한다.
		Thread temp_thread = new Thread(process_test_start_server);

		// 2) Thread를 시작힌다.
		temp_thread.Start();
	}
	public static void request_stop_test()
	{
		m_acceptor_test.stop();
	}

	public static void request_disconnect_all()
	{
		m_acceptor_test.closesocket_all();
	}

	private static void process_test_start_server(object e)
	{
		m_acceptor_test.start(bind_port);
	}

	private static void on_acceptor_start(object _object, context _context)
	{
		Program.form.on_accept_start();
	}
	private static void on_acceptor_stop(object sender)
	{
		Program.form.on_accept_close();
	}
}
