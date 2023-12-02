//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                       tutorials execute - scheduler                       *
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
// execute - scheduler
//
//  - 일정시간마다 특정 함수를 실행하고 싶을 때 scheduler를 사용한다.
//  - system_executor에 Ischedulable 객체를 등록해 일정 시간마다 실행가능하다.
//  - 더 이상 실행하지 않기 위해서는 Ischedulabe 객체를 등록해제하면 된다.
//
// ----------------------------------------------------------------------------
namespace tutorial.execute._02.scheduler.console
{
	class Program
	{
		static void Main(string[] args)
		{
			// 1) 일반 함수 혹은 람다함수를 2초마다 실행하기
			var pschedulable = new CGDK.execute.schedulable.Cexecutable((object _object, object _param) =>
			{
				Console.WriteLine("execute 1");
			}, 
			2 * TimeSpan.TicksPerSecond);

			// - pschedulable 객체를 등록한다.
			CGDK.execute.system_executor.register_schedulable(pschedulable);


			// 2) foo 객체의 멤버 함수를 3초마다실행하기
			var pobject = new foo();

			// - pobject의 pschedulable 객체를 등록한다.
			CGDK.execute.system_executor.register_schedulable(pobject.pschedulable);


			// * ESC누를 때까지 대기 (ESC를 누를 때까지 기다린다.)
			while (Console.ReadKey().Key != ConsoleKey.Escape) ;

			// 3) 등록해제한다. (등록해제하기 위해서는 등록한 객체를 가지고 있어야 한다.)
			CGDK.execute.system_executor.unregister_schedulable(pschedulable);
			CGDK.execute.system_executor.unregister_schedulable(pobject.pschedulable);
		}
	}

	class foo
	{
		public foo()
		{
			// schedulable 객체를 생성한다. - on_execute()함수를 3초마다 실행하도록 설정한다.
			pschedulable = new CGDK.execute.schedulable.Cexecutable(on_execute, 3 * TimeSpan.TicksPerSecond);
		}
		public void on_execute(object _object, object _param)
		{
			Console.WriteLine("execute 2");
		}

		public CGDK.execute.schedulable.Cexecutable pschedulable;
	}

}
