//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                               Pool Classes                                *
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
using System.Diagnostics;

//----------------------------------------------------------------------------
//
//  CGDK.SystemExecutor
//
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public static class SystemExecutor
	{
		public static bool					Post(IExecutable _pexecutable, ulong _param = 0)
		{
			return Executor.Post(_pexecutable, _param);
		}
		public static bool					Post(long _tick_time, IExecutable _pexecutable, ulong _param = 0)
		{
			return m_executor_single.PostAt(_tick_time, _pexecutable, _param);
		}
		public static bool					Post(Common.DelegateExecute _pexecutable, object _param = null)
		{
			return Post(new ExecutableDelegate(_pexecutable, _param));
		}
		public static bool					Post(long _tick_time, Common.DelegateExecute _pexecutable, object _param = null)
		{
			return Post(_tick_time, new ExecutableDelegate(_pexecutable, _param));
		}

		public static bool					RegisterSchedulable(ISchedulable _schedulable)
		{
			return	Scheduler.RegisterSchedulable(_schedulable);
		}
		public static bool					UnregisterSchedulable(ISchedulable _schedulable)
		{
			return	Scheduler.UnregisterSchedulable(_schedulable);
		}

		public static ExecutorList			Executor
		{
			get	{ if(m_executor == null) InitializeInstance(); return m_executor;}
		}
		public static ExecutorSingleExecute SingleExecute
		{
			get	{ if(m_executor_single == null) InitializeInstance(); return m_executor_single;}
		}
		public static ExecutorSchedulable	Scheduler
		{
			get	{ if(m_executor_schedulable == null) InitializeInstance();  return m_executor_schedulable;}
			set	{ m_executor_schedulable = value;}
		}

		public static void					Destroy()
		{
			lock (m_cs_executor)
			{
				if (m_executor != null)
					m_executor.Destroy();

				if (m_executor_single != null)
					m_executor_single.Destroy();

				if (m_executor_schedulable != null)
					m_executor_schedulable.Destroy();
			}
		}

		private static object				m_cs_executor = new object();
		private static ExecutorList			m_executor = null;
		private static ExecutorSingleExecute m_executor_single = null;
		private static ExecutorSchedulable	m_executor_schedulable = null;

		private	static	void				InitializeInstance()
		{
			lock(m_cs_executor)
			{
				// check) 이미 초기화가 되었으면 여기서 종료한다.
				if(m_executor != null)
					return;

				// 1) 초기화를 진행한다.
				m_executor				 = new ExecutorList();
				m_executor_single		 = new ExecutorSingleExecute();
				m_executor_schedulable	 = new ExecutorSchedulable();

				// 2) Executor들을 실행한다.
				Context temp = new Context();

				// 3) Start
				m_executor.Start(temp);
				m_executor_single.Start(temp);
				m_executor_schedulable.Start(temp);
			}
		}
	}
}
