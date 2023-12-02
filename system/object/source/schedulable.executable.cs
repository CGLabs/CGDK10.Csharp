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
using System.Collections.Generic;
using System.Linq;
using System.Text;

//----------------------------------------------------------------------------
//  CGDK.ExecuteClasses.
//
//  interface ICGSchedulable
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK.schedulable
{
	public class Cexecutable : Ischedulable
	{
	// Constructor)
		public Cexecutable()
		{
			executable	 = null;
			executor	 = null;
			interval	 = 10 * TimeSpan.TicksPerSecond;
			next_execute = 0;
		}
		public Cexecutable(long _interval, Iexecutor _executor = null)
		{
			executable	 = null;
			executor	 = _executor;
			interval	 = _interval;
			next_execute = 0;
		}
		public Cexecutable(Iexecutable _executable, long _interval = 10, Iexecutor _executor = null)
		{
			executable	 = _executable;
			executor	 = _executor;
			interval	 = _interval;
			next_execute = 0;
		}
		public Cexecutable(common.delegate_execute _function, long _interval = 10 * TimeSpan.TicksPerSecond, Iexecutor _executor = null)
		{
			executable	 = new executable_delegate(_function);
			executor	 = _executor;
			interval	 = _interval;
			next_execute = 0;
		}

	// public) 
		public Iexecutor		executor
		{
			get { return m_executor;}
			set { m_executor = value;}
		}
		public Iexecutable		executable
		{
			get { return m_executable;}
			set { m_executable = value;}
		}
		public long				interval
		{
			get { return m_tick_execute_interval;}
			set { m_tick_execute_interval = value;}
		}
		public long				next_execute
		{
			get { return m_tick_next_execute;}
			set { m_tick_next_execute = value;}
		}

	// implementations) 
		public void				process_on_register()
		{
		}
		public void				process_on_unregister()
		{
		}
		public void				process_schedule()
		{
			// check) m_executable null이면 그냥 끝낸다.
			if(m_executable == null)
				return;
		
			// check) m_executor null이면 기본 Executor로 설정한다.
			if(m_executor == null)
			{
				m_executor = system_executor.executor;
			}

			// check) Interval이 0이면 안됀다.
			Debug.Assert(m_tick_execute_interval != 0);

			// 1) 현재 시간을 구한다.
			var	tick_now = System.DateTime.Now.Ticks;

			// check) 시간이 되었는지 확인한다.
			if(m_tick_next_execute == 0)
			{
				m_tick_next_execute = tick_now;
				return;
			}

			// 2) 실행 시간이 초과했으면 실행한다.
			if(m_tick_next_execute <= tick_now)
			{
				// check) Interval이 0이면 안됀다.
				Debug.Assert(m_executable != null);

				// - 실행한다.
				m_executor.post(m_executable);

				// - 다음 실행 시간을 계산한다.
				m_tick_next_execute	+= m_tick_execute_interval;
			}
		}

		private	Iexecutable		m_executable;
		private	Iexecutor		m_executor;
		private	long			m_tick_execute_interval;
		private	long			m_tick_next_execute;
	}
}