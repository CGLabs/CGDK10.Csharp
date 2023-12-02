//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                      tutorials server.log - log_file                      *
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

namespace tutorial.server.log._01.log_file
{
	class Program
	{
		// *) log!
		public static CGDK.Server.LoggerFile log_file;

		static void Main()
		{
			// ------------------------------------------------------------------
			// 1. 먼저 로그를 남길 대상을 결정하는 대상객체를 생성한다.
			// ------------------------------------------------------------------
			// 1) CLogFile 객체를 생성한다.
			log_file = new();

			// View) 
			Console.WriteLine("create LOG file");

			// 2) CLogFile의 파일명을 파라메터로 주어 초기화한다.
			var result = log_file.Initialize("./test_log.log");

			// check) 만약 실패했다면 종료
			if(result == false)
			{
				// View) 
				Console.WriteLine("Fail to creating LOG file");

				// return) 
				return;
			}

			// View) 
			Console.WriteLine("Success initializing LOG file");

			// 3) 출력할 로그파일의 문자 인코딩을 결정한다.
			//    - Log의 문자인코딩과 파일의 출력 문자인코딩이 다르면 변환하여 출력한다.
			//    - 따라서 Log의 문자 인코딩과 출력할 파일의 문자 인코딩이 동일하다면 
			//      가장 효율이 좋을 것이다.
			//g_plog_file->set_character_encoding(eCHAR_ENCODING::WINDOWS_MBCS);		// Multibye character encoding
			//g_plog_file->set_character_encoding(eCHAR_ENCODING::WINDOWS_UNICODE);	// Default
			//g_plog_file->set_character_encoding(eCHAR_ENCODING::UTF8);			// UTF-8 character encodeing


			// ------------------------------------------------------------------
			// 2. LOG()함수를 사용하여 로그를 출력한다.
			//    (로그 대상 객체를 g_plog_file로 한다.)
			// ------------------------------------------------------------------

			// 1) 다음과 같이 Log를 출력할 수 있다.
			LOG.INFO(log_file, "[A] This paragraph is log");

			// 2) Format String을 사용한 로그
			LOG.INFO(log_file, "[A] This paragraph is log with number [" + 10 + "]");
			LOG.INFO(log_file, "[A] This paragraph is log with string [" + "STRING" + "]");
			LOG.INFO(log_file, "[A] 이 문장은 한글 문자열 로그 [" + "STRING" + "]");

			// view) 
			Console.WriteLine("Complete output LOG");
		}
	}
}
