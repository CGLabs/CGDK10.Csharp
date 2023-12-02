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
	public class entity_iteration : Ientity
	{
	// constructor/destructor)
		public		entity_iteration() : base((Int64)eTYPE.ITERATION)		{m_times = 0; m_time_diff_interval = new TimeSpan(0);}
		public		entity_iteration(string _name) : base((Int64)eTYPE.ITERATION) { name = _name; m_times = 0; m_time_diff_interval = new TimeSpan(0);}

		// publics)
		public int					times									{ get { return m_times; } set { m_times = value; }}
		public TimeSpan				interval								{ get { return m_time_diff_interval; } set { m_time_diff_interval = value; }}
																		  
	// frameworks)															  
		protected virtual eRESULT	process_event_iteration(DateTime _time)	{ return eRESULT.FAIL; }


	// implementation)
		public override long		process_execute(ulong _return, ulong _param)
		{
			// check) 
			if(object_state != eOBJECT_STATE.RUNNING)
				return 0;

			// 1) 현재시간과 다음 시간을 구한다. 
			var time_now = DateTime.UtcNow;
			var time_next = entity_status.time_next;

			// check) 현재 시간과 차이가 0보다 크면 실행한다. (S_FALSE)
			if (time_now < time_next)
				return 1;

			// 2) 새로운 Result를 추가한다.
			var presult_event = _append_result_log("process_event_iteration");

			// check)
			Debug.Assert(presult_event != null);

			try
			{
				// statistics) 
				m_entity_status.statistics_try();

				// 3) call 'process_event_iteration'
				presult_event.result = process_event_iteration(time_now);

				// statistics) 
				if(presult_event.result == eRESULT.SUCCESS)
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

			// 4) Result를 써넣는다.
			_append_result_log_post(presult_event);

			// 5) transit messsage
			//entity_manager.instance.transmit_message(new sMESSAGE(eMESSAGE.SERVER.EVENT.EXECUTE_EVENT, this as Ientity));

			// 6) m_dwInterval을 1 줄인다.
			if(m_times > 0)
			{
				--m_times;

				if(m_times == 0)
				{
					entity_status.state = eSTATE.DONE;
				}
			}

			// check) Interval이 모두 0이면 안됀다!
			Debug.Assert(m_time_diff_interval != TimeSpan.Zero);

			// 7) next time을 새로 설정한다. (interval만큼을 더한다.)
			time_next += m_time_diff_interval;

			// 8) next time을 설정한다.
			entity_status.time_next = time_next;

			// 9) 완료되면 stop를 한다.
			if (entity_status.state != eSTATE.DONE)
			{
				entity_manager.instance._process_push_to_heap(this);
			}
			else
			{
				// - stop
				stop();
			}

			// return)
			return 0;
		}

		// 7) 횟수 관계
		private int					m_times; // 반복 횟수
		private TimeSpan			m_time_diff_interval; // 반복 간격
	}
}
