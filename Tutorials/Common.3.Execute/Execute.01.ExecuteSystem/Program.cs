//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                    tutorials execute - executor_system                    *
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

// ----------------------------------------------------------------------------
//
// execute - executor_system
//
//
//
//
// ----------------------------------------------------------------------------
namespace tutorial.execute._01.executor_system.console
{
	class Program
	{
		static void test_function_1(object _object, object _param)
		{
			Console.WriteLine("execute test_function_1 function");
		}

		static void Main(string[] args)
		{
			// 1) lambda 함수를 비동기적으로 실행시킴.
			//
			//    - C# async와 거의 동일한 기능이다.
			//    - executor는 실행을 처리하는 객체로 쓰레드풀이다.
			//    - executor의 post()로 비동기 처리가 가능하다.
			//    - executor의 post() 함수를 호출 시 파라미터가 delegate, lambda함수, 일반 함수
			//      등 다양한 종류의 실행대상을 사용할 수 있다.
			{
				// - executor를 생성한다.
				var pexecutor = new CGDK.ExecutorList();

				// - executor를 thread수가 2개인 thread_pool을 생성하여 시작한다.
				pexecutor.Start(2);

				// - delegate를 사용해 비동기 실행
				pexecutor.Post(delegate(object _object, object _param)
				{
					Console.WriteLine("execute delegate");
				});

				// - lambda 함수를 사용한 비동기 실행
				pexecutor.Post((object _object, object _param) =>
				{
					Console.WriteLine("execute lambda function");
				});

				// - 일반 함수를 비동기적으로 실행하기
				pexecutor.Post(test_function_1);
			}

			// 2) SystemExecutor
			//
			//    - executor를 따로 만들 필요 없이 기본적으로 존재하는 SystemExecutor
			//      를 사용할 수 있다.
			//
			{
				CGDK.SystemExecutor.Post((object _object, object _param) =>
				{
					Console.WriteLine("execute lambda function in system executor");
				});
			}

			// 3) lambda 함수를 비동기적으로 실행하기
			//
			//    - 정해진 시간에 executor를 따로 만들 필요 없이 기본적으로 존재하는 SystemExecutor
			//      를 사용할 수 있다.
			{
				// - 현재시간의 3초후에 실행한다.
				CGDK.SystemExecutor.Post(System.DateTime.Now.Ticks + 3 * TimeSpan.TicksPerSecond, (object _object, object _param) =>
				{
					Console.WriteLine("delated execute lambda function");
				});
			}

			// 4) ESC누를 때까지 대기 (ESC를 누를 때까지 기다린다.)
			while (Console.ReadKey().Key != ConsoleKey.Escape) ;
		}
	}
}
