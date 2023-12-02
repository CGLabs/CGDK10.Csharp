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
//  CGDK.system_executor
//
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public static class system_executor
	{
	// Public)
		public static bool					post(Iexecutable _pexecutable, ulong _param = 0)
		{
			return executor.post(_pexecutable, _param);
		}
		public static bool					post(long _tick_time, Iexecutable _pexecutable, ulong _param = 0)
		{
			return m_executor_single.post_at(_tick_time, _pexecutable, _param);
		}
		public static bool					post(common.delegate_execute _pexecutable, object _param = null)
		{
			return post(new executable_delegate(_pexecutable, _param));
		}
		public static bool					post(long _tick_time, common.delegate_execute _pexecutable, object _param = null)
		{
			return post(_tick_time, new executable_delegate(_pexecutable, _param));
		}

		public static bool					register_schedulable(Ischedulable _schedulable)
		{
			return	scheduler.register_schedulable(_schedulable);
		}
		public static bool					unregister_schedulable(Ischedulable _schedulable)
		{
			return	scheduler.unregister_schedulable(_schedulable);
		}

		public static executor_list			executor
		{
			get	{ if(m_executor == null) initialize_instance(); return m_executor;}
		}
		public static executor_single_execute single_execute
		{
			get	{ if(m_executor_single == null) initialize_instance(); return m_executor_single;}
		}
		public static executor_schedulable	scheduler
		{
			get	{ if(m_executor_schedulable == null) initialize_instance();  return m_executor_schedulable;}
			set	{ m_executor_schedulable = value;}
		}

		public static void					destroy()
		{
			lock (m_cs_executor)
			{
				if (m_executor != null)
					m_executor.destroy();

				if (m_executor_single != null)
					m_executor_single.destroy();

				if (m_executor_schedulable != null)
					m_executor_schedulable.destroy();
			}
		}

		// implementations) 
		private static object				m_cs_executor			 = new object();
		private static executor_list		m_executor				 = null;
		private static executor_single_execute m_executor_single	 = null;
		private static executor_schedulable	m_executor_schedulable	 = null;

		private	static	void				initialize_instance()
		{
			lock(m_cs_executor)
			{
				// check) 이미 초기화가 되었으면 여기서 종료한다.
				if(m_executor != null)
					return;

				// 1) 초기화를 진행한다.
				m_executor				 = new executor_list();
				m_executor_single		 = new executor_single_execute();
				m_executor_schedulable	 = new executor_schedulable();

				// 2) Executor들을 실행한다.
				context temp = new context();

				// 3) start
				m_executor.start(temp);
				m_executor_single.start(temp);
				m_executor_schedulable.start(temp);
			}
		}
	}
}
