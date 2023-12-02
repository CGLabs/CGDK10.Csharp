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
	private	static CGDK.Net.Acceptor<SocketTcp> m_acceptor_test;

	public static void InitTest()
	{
		// 1) Acceptor를 생성한다.
		m_acceptor_test = new("acceptor")
		{
			NotifyOnStarting = new(OnAcceptorStart),
			NotifyOnStopping = new(OnAcceptorStop)
		};
	}

	public static void CloseTest()
	{
		RequestStopTest();
	}

	public static void RequestStartTest()
	{
		// 1) Acceptor를 처리할 Thread를 생성한다.
		Thread temp_thread = new(ProcessTestStartServer);

		// 2) Thread를 시작힌다.
		temp_thread.Start();
	}
	public static void RequestStopTest()
	{
		m_acceptor_test.Stop();
	}

	public static void RequestDisconnectAll()
	{
		m_acceptor_test.CloseSocketAll();
	}

	private static void ProcessTestStartServer(object e)
	{
		m_acceptor_test.Start(bind_port);
	}

	private static void OnAcceptorStart(object _object, Context _Context)
	{
		Program.form.OnAcceptStart();
	}
	private static void OnAcceptorStop(object Sender)
	{
		Program.form.OnAcceptClose();
	}
}
