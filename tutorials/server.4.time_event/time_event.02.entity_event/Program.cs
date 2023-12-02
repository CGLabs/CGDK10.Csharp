//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                   tutorials time_event - basic.console                    *
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
// 1. time_event.entity란?
//
//    1) 이전의 다른 event 객체들은 시작시간과 종료시간 혹은 일정시간마다 반복되는
//       경우에만 사용가능한 Event 객체였다.
//       하지만 정해진 시간에 실행되어야 하는 Event가 있을 경우 그 사용이 힘들었다.
//       이 schedule Event 객체는 임의의 시간을 등록하여 등록된 시간에 스케쥴에
//       따라 실행될 수 있도록 하는 Event 객체이다.
//
//    2) time_event.entity의 주요 사항
//       - 실행시간은 EVENT_SCHEDULE 구조체를 사용하여 등록한다.
//       - EVENT_SCHEDULE을 사용하여 다음 실행될 시간 (설정될 때 기준으로는 최초
//         실행 시간)과 반복실행할 경우 반복할 간격과 횟수를 설정해 줄수 있다.
//       - 1회만 실행될 경우 반복횟수를 1로 설정해 주면 된다.
//       - 설정된 EVENT_SCHEDULE를 add_schedule()함수를 사용하여 설정할 수 있다.
//
//
// ----------------------------------------------------------------------------
namespace tutorial.time_event._02.entity_event.console
{
	class Program
	{
		static void Main(string[] args)
		{
			// trace) 
			Console.WriteLine("start tutorial...");


			// ------------------------------------------------------------------
			// 1. entity event (1)
			//
			//    - 정해진 시간에 한번만 실행되는 Event객체이다.
			//    - set_event_time()으로 설정된 시간에 process_event()함수를 호출한다.
			//    - 따라서 Event의 동작 시 처리해야할 내용을 process_event()에
			//      작성하면 된다.
			//    - 시작후 7초후 실행되도록 설정했다.
			//
			// ------------------------------------------------------------------
			{
				// 1) create event entity
				var pentity = new CGDK.server.time_event.entity ("test_event_1");

				// 2) make event - start
				{
					// - event setting
					var event_setting = new CGDK.server.time_event.sEVENT_SETTING();
					event_setting.name = "start";
					event_setting.type = CGDK.server.time_event.eTYPE.NONE;
					event_setting.time_execute = DateTime.UtcNow;
					event_setting.time_interval = TimeSpan.Zero;
					event_setting.count_times = 1;

					// - create entity object
					var event_object = new CGDK.server.time_event.Nevent (event_setting, (DateTime _time_now) =>
					{
						Console.WriteLine("test_event_1." + "executed (start)");

						return eRESULT.SUCCESS;
					});

					// - register event object
					pentity.register_event(event_object);
				}

				// 3) make event - end
				{
					// - event setting
					var event_setting = new CGDK.server.time_event.sEVENT_SETTING();
					event_setting.name = "end";
					event_setting.type = CGDK.server.time_event.eTYPE.NONE;
					event_setting.time_execute = DateTime.UtcNow + TimeSpan.FromSeconds(5);
					event_setting.time_interval = TimeSpan.Zero;
					event_setting.count_times = 1;

					// - create entity object
					var event_object = new CGDK.server.time_event.Nevent (event_setting, (DateTime _time_now) =>
					{
						Console.WriteLine("test_event_1." + "executed (end)");

						return eRESULT.COMPLETE;
					});

					// - register event object
					pentity.register_event(event_object);
				}

				// 4) start entity
				pentity.start();

				// 5) ESC를 누를 때까지 기다린다.
				while (Console.ReadKey().Key != ConsoleKey.Escape);
			}


			// ------------------------------------------------------------------
			// 2. entity event (2)
			//
			//    - 정해진 시작 시간과 종료시간에 각각 한번씩 process_event_start()
			//      와 process_event_end()를 호출하는 Event 객체이다.
			//    - 정해진 시작 시간에 process_event_start()를 호출하고 
			//    - 정해진 종료 시간에 process_event_end()를 호출한다.
			//    - 시작후 5초 후 process_event_start()가 호출되며
			//      시작후 20초후 process_event_end()가 호출된다.
			//
			// ------------------------------------------------------------------
			{
				// 1) create event entity
				var pentity = new CGDK.server.time_event.entity("test_event_2");

				// 2) 시작한 후 3초 실행하는 event 객체를 추가한다.
				pentity.register_event(CGDK.server.time_event.entity.make_event_once(DateTime.UtcNow + TimeSpan.FromSeconds(3), (DateTime _time_now) =>
				{
					Console.WriteLine("test_event_2." + "executed (once)");

					return eRESULT.SUCCESS;
				}));

				// 3) 시작한 직후 5초마다 5번 식행하는 event 객체를 추가한다.
				pentity.register_event(CGDK.server.time_event.entity.make_event_iteration(DateTime.UtcNow, TimeSpan.FromSeconds(5), 5, (DateTime _time_now) =>
				{
					Console.WriteLine("test_event_2." + "executed (iteration)");

					return eRESULT.SUCCESS;
				}));

				pentity.start();

				// 4) ESC를 누를 때까지 기다린다.
				while (Console.ReadKey().Key != ConsoleKey.Escape);
			}


			// ------------------------------------------------------------------
			// 4. 뒷마무리 & 끝내기
			// ------------------------------------------------------------------
			// view) 
			Console.WriteLine("end tutorial...");
		}
	}
}
