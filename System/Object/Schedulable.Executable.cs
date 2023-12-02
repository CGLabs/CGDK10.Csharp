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
namespace CGDK.Schedulable
{
	public class Executable : ISchedulable
	{
	// Constructor)
		public Executable()
		{
			this.Target = null;
			this.Executor = null;
			this.Interval = 10 * TimeSpan.TicksPerSecond;
			this.NextExecute = 0;
		}
		public Executable(long _interval, IExecutor _executor = null)
		{
			this.Target = null;
			this.Executor = _executor;
			this.Interval = _interval;
			this.NextExecute = 0;
		}
		public Executable(IExecutable _executable, long _interval = 10, IExecutor _executor = null)
		{
			this.Target = _executable;
			this.Executor = _executor;
			this.Interval = _interval;
			this.NextExecute = 0;
		}
		public Executable(Common.DelegateExecute _function, long _interval = 10 * TimeSpan.TicksPerSecond, IExecutor _executor = null)
		{
			this.Target = new ExecutableDelegate(_function);
			this.Executor = _executor;
			this.Interval = _interval;
			this.NextExecute = 0;
		}

	// public) 
		public IExecutor		Executor
		{
			get { return this.m_executor;}
			set { this.m_executor = value;}
		}
		public IExecutable		Target
		{
			get { return this.m_executable;}
			set { this.m_executable = value;}
		}
		public long				Interval
		{
			get { return this.m_tick_execute_interval;}
			set { this.m_tick_execute_interval = value;}
		}
		public long				NextExecute
		{
			get { return this.m_tick_next_execute;}
			set { this.m_tick_next_execute = value;}
		}

	// implementations) 
		public void				ProcessOnRegister()
		{
		}
		public void				ProcessOnUnregister()
		{
		}
		public void				ProcessSchedule()
		{
			// check) m_executable null이면 그냥 끝낸다.
			if(this.m_executable == null)
				return;
		
			// check) m_executor null이면 기본 Executor로 설정한다.
			if(this.m_executor == null)
			{
				this.m_executor = SystemExecutor.Executor;
			}

			// check) Interval이 0이면 안됀다.
			Debug.Assert(this.m_tick_execute_interval != 0);

			// 1) 현재 시간을 구한다.
			var	tick_now = System.DateTime.Now.Ticks;

			// check) 시간이 되었는지 확인한다.
			if(this.m_tick_next_execute == 0)
			{
				this.m_tick_next_execute = tick_now;
				return;
			}

			// 2) 실행 시간이 초과했으면 실행한다.
			if(this.m_tick_next_execute <= tick_now)
			{
				// check) Interval이 0이면 안됀다.
				Debug.Assert(this.m_executable != null);

				// - 실행한다.
				this.m_executor.Post(this.m_executable);

				// - 다음 실행 시간을 계산한다.
				this.m_tick_next_execute += m_tick_execute_interval;
			}
		}

		private	IExecutable		m_executable;
		private	IExecutor		m_executor;
		private	long			m_tick_execute_interval;
		private	long			m_tick_next_execute;
	}
}