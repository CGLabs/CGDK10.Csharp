//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                     tutorials server.log - log_console                    *
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

namespace tutorial.server.log._02.log_console
{
	class Program
	{
		// *) log
		public static CGDK.Server.LoggerConsole log_console;

		static void Main()
		{
			// ------------------------------------------------------------------
			// 설명)
			//
			//  Console창에 LOG를 남길 수도 있다.
			//  간단히 [로그 객체]를 콘솔창용으로 생성하면 된다.
			//  
			//    CCGLoggerConsole 이 콘솔창 출력용 [로그 객체]이다.
			//
			//  콘솔창 출력 용 [로그 객체]는 특별한 초기화가 필요 없다.
			//
			// ------------------------------------------------------------------
			// ------------------------------------------------------------------
			// 1. 먼저 로그를 남길 대상을 결정하는 대상객체를 생성한다.
			// ------------------------------------------------------------------
			// 1) 콘솔창용 [로그 객체]인 'CCGLoggerConsole'를 생성한다.
			log_console = new CGDK.Server.LoggerConsole();

			// view) 
			Console.WriteLine("create LOG file");

			// 2) initialize
			log_console.Initialize(new Context());


			// ------------------------------------------------------------------
			// 2. LOG()함수를 사용하여 로그를 출력한다.
			//    (로그 대상 객체를 g_plog로 한다.)
			// ------------------------------------------------------------------

			// 1) 다음과 같이 log를 출력할 수 있다.
			LOG.INFO				(log_console, "[A] This paragraph is log");

			// 2) Format String을 사용한 로그
			LOG.INFO				(log_console, "[A-1] INFO Important Log");
			LOG.INFO_IMPORTANT		(log_console, "[A-2] Important INFO Important Log");
			LOG.PROGRESS			(log_console, "[A-3] PROGRESS Log (1)");
			LOG.PROGRESS_IMPORTANT	(log_console, "[A-5] Important PROGRESS Log");
			LOG.ERROR				(log_console, "[A-6] ERROR Log");
			LOG.EXCEPTION			(log_console, "[A-7] EXCEPTION Log");
			LOG.WARNING				(log_console, "[A-8] WARNING Log");
			LOG.DEBUG				(log_console, "[A-9] DEBUG Log");
			LOG.USER				(log_console, "[A-10] USER Log");
			LOG.SYSTEM				(log_console, "[A-11] SYSTEM Log");
			LOG.INFO				(log_console, "[A-12] 이 문장은 멀티바이트 한글 문자열 로그 [" + "STRING" + "]");

			// return) 끝.
			return;
		}
	}
}
