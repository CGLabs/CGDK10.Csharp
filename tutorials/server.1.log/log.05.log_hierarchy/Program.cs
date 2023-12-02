//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                   tutorials server.log - log_hierarchy                    *
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

namespace tutorial.server.log._05.log_hierarchy
{
	class Program
	{
		// *) log
		public static CGDK.server.log_hub plog;

		static void Main(string[] args)
		{
			// ------------------------------------------------------------------
			// 1. 한번의 LOG()함수 호출로 여러 개의 [로그 객체]에 로그를 전달하고 
			//    싶다면 계층적 로그 객체를 구성하면 가능하다. 
			//    
			//    예를 들면 로그를 파일로도 남기고 콘솔화면으로도 남기고 싶다면
			//    먼저 부모 [로그 객체]를 생성한 후 
			//    파일로 남기는 [로그 객체]와 콘솔창에 로그를 남기는 [로그 객체]를 
			//    이 부모 로그 객체의 자식 로그로 붙이게 되면 부모 [로그 객체]에 
			//    로그를 전달하는 것만으로 자식 로그 객체 모두에게 전달된다.
			//
			// ------------------------------------------------------------------

			// 1) 콘솔창용 [로그 객체]인 'CCGLogConsole'를 생성한다.
			var plog_console = new CGDK.server.log_console();

			// 2) 파일에 남기는 [로그 객체]를 생성한다.
			var plog_file = new CGDK.server.log_file();

			// 3) 파일에 남기는 [로그 객체]를 초기화한다.
			plog_file.initialize("./test_log.log");

			// 4) 부모 [로그 객체]를 로그 객체를 생성한다.
			plog = new CGDK.server.log_hub();

			// 5) 부모 [로그 객체]인 plog에 plog_console와 plog_file를 자식 로그 객체로 연결한다.
			plog.register_log_targetable(plog_console);
			plog.register_log_targetable(plog_file);


			// 6) 이제 부모 로그 객체인 plog에 로그를 전달하는 것만으로 
			//     plog_console과 plog_file 모두에게 log의 전달이 된다.
			LOG.INFO(plog, "This paragraph is log");

			// 2) Format String을 사용한 로그
			LOG.INFO(plog, "This paragraph is log with number [" + 10 + "]");
			LOG.INFO(plog, "This paragraph is log with string [" + "STRING" + "]");

			// view) 
			Console.WriteLine("complete output LOG\n");
		}
	}
}
