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
	public class entity_start_end : Ientity
	{
	// constructor/destructor)
		public	entity_start_end() : base((Int64)eTYPE.START_END) { m_time_event_start = new DateTime(0); m_is_event_start = true; m_time_event_start = new DateTime(0); m_is_event_terminate = true; }
		public	entity_start_end(string _name) : this()  { name = _name; m_time_event_start = new DateTime(0); m_is_event_start = true; m_time_event_start = new DateTime(0); m_is_event_terminate = true; }

	// publics)
		public DateTime				start_time { get { return m_time_event_start; } set { m_time_event_start = value; }}
		public DateTime				end_time { get { return m_time_event_terminate; } set { m_time_event_terminate = value; }}

		protected virtual eRESULT	process_event_start(DateTime _time)
		{
			return eRESULT.SUCCESS;
		}
		protected virtual eRESULT	process_event_end(DateTime _time)
		{
			return eRESULT.SUCCESS;
		}

	// implementation) 
		public override long		process_execute(ulong _return, ulong _param)
		{
			// check) 
			if(object_state != eOBJECT_STATE.RUNNING)
				return 0;

			// 1) get now utc time
			var time_now = DateTime.UtcNow;

			if(m_is_event_start)
			{
				// check) 
				Debug.Assert(time_now >= m_time_event_start);

				// - set flag
				m_is_event_start = false;

				// - set next time as zero 'm_time_event_terminate'
				entity_status.time_next = m_time_event_terminate;

				// - append result log
				var presult_event = _append_result_log("process_event_start");

				try
				{
					// statistics) 
					entity_status.statistics_try();

					// - call 'process_event_start'
					presult_event.result = process_event_start(time_now);

					// statistics) 
					if (presult_event.result == eRESULT.SUCCESS)
					{
						entity_status.statistics_succeeded();
					}

					// - transit message
					//entity_manager.instance.transmit_message(new sMESSAGE(eMESSAGE.SERVER.EVENT.EXECUTE_EVENT, this as Ientity));
				}
				catch(System.Exception /*e*/)
				{
					// - set reault as 'eRESULT::EXCEPTION'
					presult_event.result = eRESULT.EXCEPTION;

					// statistics) 
					entity_status.statistics_failed();

					// event log)
					LOG_EVENT(null, eLOG_TYPE.EXCEPTION, eLOG_LEVEL.NORMAL, "Exception [" + name + "]");
				}

				// - append log
				_append_result_log_post(presult_event);

				// - stop on complete
				if (entity_status.state != eSTATE.DONE)
				{
					entity_manager.instance._process_push_to_heap(this);
				}
				else
				{
					// - stop
					stop();
				}
			}
			else if(m_is_event_terminate)
			{
				// check) 
				Debug.Assert(time_now >= m_time_event_terminate);

				// - set flag
				m_is_event_terminate = false;

				// - set next time as zero
				entity_status.time_next = new DateTime(0);

				// - append result log
				var presult_event = _append_result_log("process_event_end");

				try
				{
					// statistics) 
					entity_status.statistics_try();

					// - call 'process_event_end'
					presult_event.result = process_event_end(time_now);

					// statistics) 
					if (presult_event.result == eRESULT.SUCCESS)
					{
						entity_status.statistics_succeeded();
					}

					// - transit message
					//entity_manager.instance.transmit_message(new sMESSAGE(eMESSAGE.SERVER.EVENT.EXECUTE_EVENT, this as Ientity));
				}
				catch(System.Exception /*e*/)
				{
					// - set reault as 'eRESULT::EXCEPTION'
					presult_event.result = eRESULT.EXCEPTION;

					// statistics) 
					entity_status.statistics_failed();

					// event log)
					LOG_EVENT(null, eLOG_TYPE.EXCEPTION, eLOG_LEVEL.NORMAL, "Exception [" + name +"]");
				}

				// - append result log
				_append_result_log_post(presult_event);

				// - change state to 'eSTATE.DONE'
				entity_status.state = eSTATE.DONE;

				// - stop
				stop();
			}

			// return)
			return 0;
		}

		// 6) 시작 시간.
		protected DateTime			m_time_event_start;
		protected bool				m_is_event_start;

		// 7) 끝나는 시간
		protected DateTime			m_time_event_terminate;
		protected bool				m_is_event_terminate;
	}
}
