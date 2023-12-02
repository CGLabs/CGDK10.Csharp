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
using System.Threading;

//----------------------------------------------------------------------------
//
//  CGDK.Nexecutor_thread
//
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public abstract class Nexecutor_thread : Nobject_stateable, Iexecutor, Inameable
	{
	// Constructor) 
		public Nexecutor_thread()
		{
			interval = 10 * TimeSpan.TicksPerSecond;
		}
		public Nexecutor_thread(long _Interval)
		{
			interval = _Interval;
		}
		public Nexecutor_thread(string _name)
		{
			name = _name;
			interval = 10 * TimeSpan.TicksPerSecond;
		}
		public Nexecutor_thread(string _name, long _interval)
		{
			m_name = _name;
			interval = _interval;
		}
		~Nexecutor_thread()
		{
			destroy();
		}

	// public) 
		public virtual bool				post(Iexecutable _pexecutable, ulong _para = 0)
		{
			return process_post_execute(_pexecutable, _para);
		}
		public virtual bool				process_post_execute(Iexecutable _pexecutable, ulong _para = 0)
		{
			// Assert) 이 함수는 정의되어 있지 않다.
			Debug.Assert(false);

			// Return) 
			return	false;
		}
		public abstract bool			execute(int _tick = Timeout.Infinite);

		public long						interval
		{
			get { return m_tick_execute_interval; }
			set { m_tick_execute_interval = value; }
		}
		public string					name
		{
			get { return m_name; }
			set { m_name=value; }
		}

    // implementations) 
        protected override void			_process_notify_starting(object _object, context _context)
		{
			base._process_notify_starting(_object, _context);
		}
		protected override void			_process_notify_start(object _object, context _context)
		{
			// 1) m_bRun을 true로 설정
			m_is_run = true;

			// 2) Thread를 시작한다.
			var	thread = new Thread(_process_thread_run);

			thread.IsBackground = true;

			// 3) Thread를 시작한다.
				thread.Start(this);

			// 4) Thread Gap을 설정한다.
			m_thread = thread;

			// 5) 
			base._process_notify_starting(_object, _context);
		}
		protected override void			_process_notify_stopping(object _object)
		{
			// 1) Base의 ProcessNotifyStopping을 호출한다.
			base._process_notify_stopping(_object);
			
			// 2) Thread를 종료한다.
			m_is_run = false;
			
			// 3) 종료되길 기다린다.
			if(m_thread.ThreadState == System.Threading.ThreadState.Running)
			{
				m_thread.Join();
			}
		}
		protected override void			_process_notify_stop(object _object)
		{
			base._process_notify_stop(_object);
		}
		private void					_process_thread_run(object _param)
		{
			// 1) 현재 시간을 얻는다.
			var	time = System.DateTime.Now.Ticks;

			while(m_is_run)
			{
				try
				{
					// 1) process_execute함수를 호출한다.
					execute(0);

					while(m_is_run)
					{
						// 2) 다음 실행시간을 설정한다.
						time += m_tick_execute_interval;

						// declare)
						int tick_sleep = 0;

						// 3) sleep할 시간을 계산한다.
						var	time_now = System.DateTime.Now.Ticks;

						if(time > time_now)
						{
							tick_sleep = (int)((time - time_now) / 10000);
						}

						// 4) execute
						execute(tick_sleep);
					}
				}
				// On Exception) 
				catch(System.Exception)
				{
					// - System.Exception Handler를 실행한다.

					// - 다음 실행시간을 설정한다.
					time += m_tick_execute_interval;

					// - 남은 시간만큼은 Sleep를 함.
					var	time_now = System.DateTime.Now.Ticks;
					if(time > time_now)
					{
						Thread.Sleep((int)((time-time_now)/10000));
					}
				}
			}
		}

		// - ...
		private	bool					m_is_run = false;
		private Thread					m_thread = null;

		private long					m_tick_execute_interval;
		private string					m_name;
	}
}
