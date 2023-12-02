//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                     tutorials server.log - log_filter                     *
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

namespace tutorial.server.log._04.log_filter
{
	class Program
	{
		static void Main(string[] args)
		{
			// ------------------------------------------------------------------
			// 1. Log Filter
			//    1) [로그 필터 객체]는 특정 로그만 필터링해낼 수 있도록 해준다.
			//    2) [로그 필터 객체]를 생성하여 [로그 객체]에 설정해 줌으로써 동작한다.
			//    3) [로그 필터 객체]를 사용하기 위해서는 다음과 같은 작업을 수행해야 한다.
			//       - 먼저 [로그 필터 객체]를 생성한다.
			//       - 생성한 [로그 필터 객체]의 필터링 정보를 설정한다.
			//       - [로그 필터 객체]를 [로그 객체]에 설정한다.
			//
			//    4) [로그 필터 객체]의 필터링 정보는 [로그 형태]별로 가능하다.
			//       - enable_log() 멤버 함수를 사용하여 특정 [로그 형태]의 필터링 가능 여부를 설정한다.
			//       - enable_level_range()를 사용하여 레벨 정보로 필터링할 것임을 설정해야 한다.
			//       - set_level_range()함수를 사용하여 통과할 레벨 정보를 설정해야 한다.
			//
			//       주의) 필터링 정보는 Level, Source, Destination, Location 등이 있지만 현재는 Level만 사용한다.
			//
			//    5) set_level_range()함수를 사용해 필터링 레벨 범위를 설정할 수 있다.
			//       Level값은 int형이므로 -21억에서 21억 정도의 범위를 가진다.
			//       하지만 일반적으로 많이 사용하는 값을 enum으로 설정해 놓았다.
			//
			//			eLOG_LEVEL::LOWEST		 = INT_MIN,
			//			eLOG_LEVEL::LOWER		 = -256,
			//			eLOG_LEVEL::NORMAL		 = 0,
			//			eLOG_LEVEL::HIGHER		 = 256,
			//			eLOG_LEVEL::HIGHEST		 = INT_MAX,
			//
			//      Log Level 값은 꼭 enum으로 설정된 eLOG_LEVEL값이 아니라도 
			//      임의의 값을 설정할 수 있다.
			//       
			// ------------------------------------------------------------------
			// 1) Log Filter를 생성한다.
			var plog_filter = new CGDK.server.log_filter();

			// 2) INFO Log type에 대해서 filtering을 enable한다.
			//    또 INFO Log type 중 어떤 값에 filtering할지를 enable해 주어야 한다.
			//    INFO Log type은 eLOG_LEVEL::HIGH이상 eLOG_LEVEL::HIGHEST이하일 때만 출력한다.
			plog_filter.enable_log			(eLOG_TYPE.INFO);
			plog_filter.enable_level_range	(eLOG_TYPE.INFO);
			plog_filter.set_level_range		(eLOG_TYPE.INFO, eLOG_LEVEL.HIGHER, eLOG_LEVEL.HIGHEST);

			// 3) PROGRESS Log type에 대해서 filtering을 enable한다.
			//    PROGRESS Log type은 eLOG_LEVEL::NORMAL이상 eLOG_LEVEL::HIGHEST이하일 때만 출력한다.
			plog_filter.enable_log			(eLOG_TYPE.PROGRESS);
			plog_filter.enable_level_range	(eLOG_TYPE.PROGRESS);
			plog_filter.set_level_range		(eLOG_TYPE.PROGRESS, eLOG_LEVEL.NORMAL, eLOG_LEVEL.HIGHEST);

			// 4) Exception Log는 enable합니다.
			plog_filter.enable_log(eLOG_TYPE.DEBUG,		false);
			plog_filter.enable_log(eLOG_TYPE.EXCEPTION,	true);
			plog_filter.enable_log(eLOG_TYPE.ERROR,		true);
			plog_filter.enable_log(eLOG_TYPE.WARNING,	false);
			plog_filter.enable_log(eLOG_TYPE.USER,		false);
			plog_filter.enable_log(eLOG_TYPE.SYSTEM,	true);

			// 
			// 여기서 enable되지 않은 로그형(LOG TYPE)에 대해서는 아무런 Filtering을 하지 않게 된다.
			// 즉 eLOG_TYPE::INFO와 eLOG_TYPE::PROGRESS_만 filtering을 수행하며 나머지는 그냥 출력한다.
			//
			//	eLOG_TYPE.INFO          => 출력함. 하지만 LEVEL값이 eLOG_LEVEL::HIGH이상 eLOG_LEVEL::HIGHEST이하일 때만 출력한다.
			//	eLOG_TYPE.PROGRESS		=> 출력함. 하지만 LEVEL값이 eLOG_LEVEL::NORMAL이상 eLOG_LEVEL::HIGHEST이하일 때만 출력한다.
			//	eLOG_TYPE.DEBUG			=> 무조건 출력하지 않음.
			//	eLOG_TYPE.WARNING		=> 무조건 출력하지 않음.
			//	eLOG_TYPE.EXCEPTION 	=> 무조건 출력함.
			//	eLOG_TYPE.ERROR			=> 무조건 출력함.
			//	eLOG_TYPE.USER			=> 무조건 출력력하지 않음.
			//	eLOG_TYPE.SYSTEM		=> 무조건 출력함.
			// 


			// ------------------------------------------------------------------
			// 1. log_console을 open한다.
			// ------------------------------------------------------------------
			// 1) log_console 생성한다.
			var plog_console = new CGDK.server.log_console();

			// 2) log filter를 log객체에 설정한다.
			plog_console.filter = plog_filter;

			// view) 
			Console.WriteLine("Log Filter 예제를 시작합니다.");

			// 3) log를 write한다.
			LOG.write(plog_console, eLOG_TYPE.INFO,		 eLOG_LEVEL.LOWER,	 "Type:INFO, Level:eLOG_LEVEL.LOWER");
			LOG.write(plog_console, eLOG_TYPE.INFO,		 eLOG_LEVEL.NORMAL,	 "Type:INFO, Level:eLOG_LEVEL.NORMAL");
			LOG.write(plog_console, eLOG_TYPE.INFO,		 eLOG_LEVEL.HIGHER,	 "Type:INFO, Level:eLOG_LEVEL.HIGHER");
			LOG.write(plog_console, eLOG_TYPE.INFO,		 eLOG_LEVEL.HIGHEST, "Type:INFO, Level:eLOG_LEVEL.HIGHEST");
			Console.Write("\n");						 					 
														 					 
			LOG.write(plog_console, eLOG_TYPE.PROGRESS,	 eLOG_LEVEL.LOWER,	 "Type:PROGRESS, Level:eLOG_LEVEL.LOWER");
			LOG.write(plog_console, eLOG_TYPE.PROGRESS,	 eLOG_LEVEL.NORMAL,	 "Type:PROGRESS, Level:eLOG_LEVEL.NORMAL");
			LOG.write(plog_console, eLOG_TYPE.PROGRESS,	 eLOG_LEVEL.HIGHER,	 "Type:PROGRESS, Level:eLOG_LEVEL.HIGHER");
			LOG.write(plog_console, eLOG_TYPE.PROGRESS,	 eLOG_LEVEL.HIGHEST, "Type:PROGRESS, Level:eLOG_LEVEL.HIGHEST");
			Console.Write("\n");						 					 
														 					 
			LOG.write(plog_console, eLOG_TYPE.DEBUG,	 eLOG_LEVEL.LOWER,	 "Type:DEBUG, Level:eLOG_LEVEL.LOWER");
			LOG.write(plog_console, eLOG_TYPE.DEBUG,	 eLOG_LEVEL.NORMAL,	 "Type:DEBUG, Level:eLOG_LEVEL.NORMAL");
			LOG.write(plog_console, eLOG_TYPE.DEBUG,	 eLOG_LEVEL.HIGHER,	 "Type:DEBUG, Level:eLOG_LEVEL.HIGHER");
			LOG.write(plog_console, eLOG_TYPE.DEBUG,	 eLOG_LEVEL.HIGHEST, "Type:DEBUG, Level:eLOG_LEVEL.HIGHEST");
			Console.Write("\n");						 					 
														 					 
			LOG.write(plog_console, eLOG_TYPE.EXCEPTION, eLOG_LEVEL.LOWER,	 "Type:EXCEPTION, Level:eLOG_LEVEL.LOWER");
			LOG.write(plog_console, eLOG_TYPE.EXCEPTION, eLOG_LEVEL.NORMAL,	 "Type:EXCEPTION, Level:eLOG_LEVEL.NORMAL");
			LOG.write(plog_console, eLOG_TYPE.EXCEPTION, eLOG_LEVEL.HIGHER,	 "Type:EXCEPTION, Level:eLOG_LEVEL.HIGHER");
			LOG.write(plog_console, eLOG_TYPE.EXCEPTION, eLOG_LEVEL.HIGHEST, "Type:EXCEPTION, Level:eLOG_LEVEL.HIGHEST");
			Console.Write("\n");						 					 
														 					 
			LOG.write(plog_console, eLOG_TYPE.ERROR,	 eLOG_LEVEL.LOWER,	 "Type:ERROR, Level:eLOG_LEVEL.LOWER");
			LOG.write(plog_console, eLOG_TYPE.ERROR,	 eLOG_LEVEL.NORMAL,	 "Type:ERROR, Level:eLOG_LEVEL.NORMAL");
			LOG.write(plog_console, eLOG_TYPE.ERROR,	 eLOG_LEVEL.HIGHER,	 "Type:ERROR, Level:eLOG_LEVEL.HIGHER");
			LOG.write(plog_console, eLOG_TYPE.ERROR,	 eLOG_LEVEL.HIGHEST, "Type:ERROR, Level:eLOG_LEVEL.HIGHEST");
			Console.Write("\n");						 					 
												
			LOG.write(plog_console, eLOG_TYPE.WARNING,	 eLOG_LEVEL.LOWER,	 "Type:DEBUG, Level:eLOG_LEVEL.LOWER");
			LOG.write(plog_console, eLOG_TYPE.WARNING,	 eLOG_LEVEL.NORMAL,	 "Type:DEBUG, Level:eLOG_LEVEL.NORMAL");
			LOG.write(plog_console, eLOG_TYPE.WARNING,	 eLOG_LEVEL.HIGHER,	 "Type:DEBUG, Level:eLOG_LEVEL.HIGHER");
			LOG.write(plog_console, eLOG_TYPE.WARNING,	 eLOG_LEVEL.HIGHEST, "Type:DEBUG, Level:eLOG_LEVEL.HIGHEST");
			Console.Write("\n");						 					 
		
			LOG.write(plog_console, eLOG_TYPE.USER,		 eLOG_LEVEL.LOWER,	 "Type:USER, Level:eLOG_LEVEL.LOWER");
			LOG.write(plog_console, eLOG_TYPE.USER,		 eLOG_LEVEL.NORMAL,	 "Type:USER, Level:eLOG_LEVEL.NORMAL");
			LOG.write(plog_console, eLOG_TYPE.USER,		 eLOG_LEVEL.HIGHER,	 "Type:USER, Level:eLOG_LEVEL.HIGHER");
			LOG.write(plog_console, eLOG_TYPE.USER,		 eLOG_LEVEL.HIGHEST, "Type:USER, Level:eLOG_LEVEL.HIGHEST");
			Console.Write("\n");						 					 
														 					 
			LOG.write(plog_console, eLOG_TYPE.SYSTEM,	 eLOG_LEVEL.LOWER,	 "Type:SYSTEM, Level:eLOG_LEVEL.LOWER");
			LOG.write(plog_console, eLOG_TYPE.SYSTEM,	 eLOG_LEVEL.NORMAL,	 "Type:SYSTEM, Level:eLOG_LEVEL.NORMAL");
			LOG.write(plog_console, eLOG_TYPE.SYSTEM,	 eLOG_LEVEL.HIGHER,	 "Type:SYSTEM, Level:eLOG_LEVEL.HIGHER");
			LOG.write(plog_console, eLOG_TYPE.SYSTEM,	 eLOG_LEVEL.HIGHEST, "Type:SYSTEM, Level:eLOG_LEVEL.HIGHEST");
			Console.Write("\n");

			// return)
			return;
		}
	}
}
