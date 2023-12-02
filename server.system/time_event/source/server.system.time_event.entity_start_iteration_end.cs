//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                          Group Template Classes                           *
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
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace CGDK.server.time_event
{
	public class entity_start_iteration_end : entity_start_end
	{
		// constructor/destructor)
		public entity_start_iteration_end() { }
		public	entity_start_iteration_end(string _name) : this() { name = _name; }

		// publics)
		public	DateTime			event_time { get { return entity_status.time_next; } set { entity_status.time_next = value; } }
		public	TimeSpan			interval { get { return m_time_diff_interval; } set { m_time_diff_interval = value; } }
		public	int					times { get { return m_times; } set { m_times = value; } }

	// frameworks) 
		protected virtual eRESULT	process_event_iteration(DateTime _time) { return eRESULT.SUCCESS; }

	// implementation)
		public override long		process_execute(ulong _return, ulong _param)
		{
			// 1) 현재시간을 구한다.
			var time_now = DateTime.UtcNow;

			// 2) start시간이 지났다면 start를 수행한다.
			if(m_is_event_start)
			{
				process_execute_start(time_now);
			}
			// 2) run을 수행한다.
			else if(m_times_remained != 0)
			{
				process_execute_iteration(time_now);
			}	
			// 4) Terminate Time이 지났다면 OnEventTerminate를 수행한다.
			else if(m_is_event_terminate)
			{
				process_execute_end(time_now);
			}

			// 5) retry or stop
			if (entity_status.state != eSTATE.DONE)
			{
				entity_manager.instance._process_push_to_heap(this);
			}
			else
			{
				stop();
			}

			// return)
			return 0;
		}
		protected void				process_execute_start(DateTime _time_now)
		{
			// check) 
			if(object_state != eOBJECT_STATE.RUNNING)
				return;

			// check) 
			Debug.Assert(m_time_event_start <= _time_now);

			// 1) start했으므로 false로 설정한다.
			m_is_event_start = false;

			// 2) append result log
			var presult_event = _append_result_log("process_event_start");

			try
			{
				// statistics) 
				entity_status.statistics_try();

				// 3) (핵심) process_event_start함수를 호출한다.
				presult_event.result = process_event_start(_time_now);

				// statistics) 
				if (presult_event.result == eRESULT.SUCCESS)
				{
					entity_status.statistics_succeeded();
				}
			}
			catch(System.Exception)
			{
				// - 결과를 Exception으로 설정
				presult_event.result = eRESULT.EXCEPTION;

				// statistics) 
				entity_status.statistics_failed();

				// event log)
				LOG_EVENT(null, eLOG_TYPE.EXCEPTION, eLOG_LEVEL.NORMAL, "Exception [" + name + "]");
			}

			// 4) append result log
			_append_result_log_post(presult_event);

			// 5) transit messsage
			//entity_manager.instance.transmit_message(new sMESSAGE(eMESSAGE.SERVER.EVENT.EXECUTE_EVENT, this as Ientity));

			// check) Interval이 모두 0이면 안됀다!
			Debug.Assert(m_time_diff_interval != TimeSpan.Zero);

			// 6) Interval만큼을 더해 다음 시간을 설정한다.
			var time_next = m_time_event_start + m_time_diff_interval;

			if (time_next < m_time_event_terminate)
			{
				entity_status.time_next = time_next;
			}
			else
			{
				entity_status.time_next = m_time_event_terminate;
			}

			// 7) m_times_remained를 MAX값으로 리셋한다.
			m_times_remained = m_times;
		}
		protected void				process_execute_iteration(DateTime _time_now)
		{
			// check) 
			if (object_state != eOBJECT_STATE.RUNNING)
				return;

			// declare)
			var time_next = entity_status.time_next;

			// check) 
			Debug.Assert(time_next <= _time_now);

			// 1) append result log
			var presult_event = _append_result_log("process_event_iteration");

			try
			{
				// statistics) 
				entity_status.statistics_try();

				// - call 'process_event_iteration'
				presult_event.result = process_event_iteration(_time_now);

				// statistics) 
				if (presult_event.result == eRESULT.SUCCESS)
				{
					entity_status.statistics_succeeded();
				}
			}
			catch(System.Exception)
			{
				// - 결과를 Exception으로 설정
				presult_event.result = eRESULT.EXCEPTION;

				// statistics) 
				entity_status.statistics_failed();

				// event log)
				LOG_EVENT(null, eLOG_TYPE.EXCEPTION, eLOG_LEVEL.NORMAL, "Exception [" + entity_setting.name + "]");
			}

			// 3) append result log
			_append_result_log_post(presult_event);

			// 4) Message 날리기..
			//entity_manager.instance.transmit_message(new sMESSAGE(eMESSAGE.SERVER.EVENT.EXECUTE_EVENT, this as Ientity));

			// 5) m_times_remained을 1 줄인다.
			if(m_times_remained > 0)
			{
				m_times_remained--;
			}
			else
			{
				// - process_execute_end
				process_execute_end(_time_now);

				// - ...
				entity_status.state = eSTATE.DONE;
			}

			// check) Interval이 모두 0이면 안됀다!
			Debug.Assert(m_time_diff_interval != TimeSpan.Zero);

			// 6) Interval만큼을 더해 다음 시간을 설정한다.
			time_next = time_next + m_time_diff_interval;

			if (time_next < m_time_event_terminate)
			{
				entity_status.time_next = time_next;
			}
			else
			{
				entity_status.time_next = m_time_event_terminate;
			}
		}
		protected void				process_execute_end(DateTime _time_now)
		{
			// check) 
			if (object_state != eOBJECT_STATE.RUNNING)
				return;

			// 1) 종료처리했으므로 false로 설정한다.
			m_is_event_terminate = false;

			// 2) 새로운 Result를 추가한다.
			var presult_event = _append_result_log("process_event_end");

			try
			{
				// statistics) 
				entity_status.statistics_try();

				// 3) (핵심) process_event_end함수를 호출한다.
				presult_event.result = process_event_end(_time_now);

				// statistics) 
				if (presult_event.result == eRESULT.SUCCESS)
				{
					entity_status.statistics_succeeded();
				}
			}
			catch(System.Exception)
			{
				// - 결과를 Exception으로 설정
				presult_event.result = eRESULT.EXCEPTION;

				// statistics) 
				entity_status.statistics_failed();

				// event log)
				LOG_EVENT(null, eLOG_TYPE.EXCEPTION, eLOG_LEVEL.NORMAL, "exception [" + name + "]");
			}

			// 4) Result를 써넣는다.
			_append_result_log_post(presult_event);

			// 5) Message 날리기..
			//entity_manager.instance.transmit_message(new sMESSAGE(eMESSAGE.SERVER.EVENT.EXECUTE_EVENT, this as Ientity));

			// 6) stop
			stop();

			// 7) set event stste
			entity_status.state = eSTATE.DONE;

			// 8) reset event time
			entity_status.time_next = new DateTime(0);
		}

		private	int					m_times_remained = -1;	// 현재 남아 있는 반복 횟수
		private	int					m_times = -1;			// 목표 반복 횟수
		private	TimeSpan			m_time_diff_interval;	// 반복 간격
	}
}
