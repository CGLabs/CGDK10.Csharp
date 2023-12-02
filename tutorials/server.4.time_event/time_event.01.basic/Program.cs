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
// 1. Event System이란?
//
//    1) 각종 System을 제작하다보면 일정시간마다 실행을 하거나 특정한 시간에 실행
//       해야하는 것들을 설정할 필요가 있다.
//       이때 특정한 시간이나  특정한 시간마다 실행하도록 하는 시스템이 Event 시스템이다.
//
//    2) Event System은 Event object와 Event Manager로 나뉘어진다.
//       - Event Manager는 등록된 EventObject를 등록하였다가 정해진 시간마다 실행해주는
//         관리자를 의미한다.
//       - Event object는 Event Manager에 등록되어 동작하는 객체로 필요에 따라
//         다양한 실행형태로 제작되어져 있다. 
//         선언된 Event object는 아래와 같은 것들이 있다.
//
//         @ time_event::object::COnce
//           예약된 시간에 딱 1회만 실행되는 객체
//
//         @ time_event::object::start_end
//           예약된 시작시간과 종료시간에 각각 정해진 함수를 호출하는 객체
//
//         @ time_event::object::start_iteration_end
//           예약된 시작시간에 정해진 함수가 호출되며 이후에 정해진 간격에 따라
//           정해진 함수를 정해진 실행횟수만큼 호출해주는 Event 객체이다.
//           실행횟수만큼 호출된 후 process_event_end()함수가 호출된다.
//
//         @ CCGEventIteration
//           정해진 시간 간격 마다 정해진 함수를 무한반복적으로 실행해하는
//           Event객체이다.
//
//    3) 각 Event객체들은 특정한 시간에 호출되어지는 virtual함수들이 정의되어 있다.
//       이 virtual함수를 재정의하여 실행하고자하는 내용을 작성하면 된다.
//
//    4) Event객체의 정밀도는 초단위 수준만을 보장하므로 더욱 정밀한 실행이 필요할
//       경우 Executable이나 다른 방법을 사용하기를 권장한다.
//
//    5) Event System에서의 시간은 모두 System Time을 사용하고 있다. System Time은
//       Local Time과 동일하지 않기 때문에 실제 지역 시간과는 다를 수 있으므로 
//       설정을 할때는 반드시 System Time으로 설정할 필요가 있다.
//
//
// ----------------------------------------------------------------------------
namespace tutorial.time_event._01.basic.console
{
	class Program
	{
		static void Main(string[] args)
		{
			// trace) 
			Console.WriteLine("start tutorial...");

			// ------------------------------------------------------------------
			// 1. CGDK.server.time_event.entity_iteration
			//
			//    - 정해진 시간에 한번만 실행되는 Event객체이다.
			//    - set_event_time()으로 설정된 시간에 process_event()함수를 호출한다.
			//    - process_event()에 처리할 내용을 재정의합니다.
			//
			//    - 시작후 1초 후 최초 실행하며 3초마다 10회 재실행합니다.
			// ------------------------------------------------------------------
			// 1) time_event 객체를 생성한다.
			var pentity_a = new CGDK.server.time_event.entity_iteration();

			// 2) setting
			pentity_a.entity_status.time_next = DateTime.UtcNow + TimeSpan.FromSeconds(1);
			pentity_a.interval = TimeSpan.FromSeconds(3);
			pentity_a.times    = 10;
			//pentity_a.event_iteration = new 
			//{

			//};

			// 3) start
			pentity_a.start();


			// ------------------------------------------------------------------
			// 2. time_event::entity_start_end
			//
			//    - 시작 시간과 process_event_start()를 호출하고
			//      종료시간에 process_event_end()를 호출하는 time_event 객체이다.
			//
			//    - 시작후 5초 후 process_event_start()가 호출되며
			//      시작후 15초후 process_event_end()가 호출된다.
			// ------------------------------------------------------------------
			// 1) time_event 객체를 생성한다.
			var pentity_b = new CGDK.server.time_event.entity_start_end();

			// 2) setting
			pentity_b.start_time = DateTime.UtcNow + TimeSpan.FromSeconds(5);
			pentity_b.end_time   = DateTime.UtcNow + TimeSpan.FromSeconds(15);

			// 3) start
			pentity_b.start();


			// ------------------------------------------------------------------
			// 3. time_event::entity_start_iteration_end
			//
			//    - 시작 시간과 process_event_start()를 호출하고
			//      time_interval 간격마다 process_event_iteration()함수를 호출한 후 
			//      종료시간에 process_event_end()를 호출하는 time_event 객체이다.
			//
			//    - 시작후 2초 후 process_event_start()가 호출된다.
			//      시작후 3초 간격으로 10번 process_event_iteration()가 호출되며
			//      그 5초 후 process_event_end()가 호출된다.
			// ------------------------------------------------------------------
			// 1) time_event 객체를 생성한다.
			var pentity_c = new CGDK.server.time_event.entity_start_iteration_end();

			// 2) setting
			pentity_c.start_time = DateTime.UtcNow + TimeSpan.FromSeconds(10);
			pentity_c.interval   = TimeSpan.FromSeconds(5);
			pentity_c.end_time   = DateTime.UtcNow + TimeSpan.FromSeconds(1);

			// 3) start
			pentity_c.start();


			// ------------------------------------------------------------------
			// 4. 종료대기...
			// ------------------------------------------------------------------
			// *1) ESC를 누를 때까지 기다린다.
			while (Console.ReadKey().Key != ConsoleKey.Escape);

			// trace) 
			Console.WriteLine("end tutorial...");
		}
	}
}
