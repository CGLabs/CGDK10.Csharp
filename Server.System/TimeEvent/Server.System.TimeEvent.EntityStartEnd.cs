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

namespace CGDK.Server.TimeEvent
{
	public class EntityStartEnd : IEntity
	{
	// constructor/destructor)
		public	EntityStartEnd() : base((Int64)eTYPE.START_END) { m_time_event_start = new DateTime(0); m_is_event_start = true; m_time_event_start = new DateTime(0); m_is_event_terminate = true; }
		public	EntityStartEnd(string _name) : this()  { this.Name = _name; m_time_event_start = new DateTime(0); m_is_event_start = true; m_time_event_start = new DateTime(0); m_is_event_terminate = true; }

	// publics)
		public DateTime				StartTime { get { return m_time_event_start; } set { m_time_event_start = value; }}
		public DateTime				EndTime { get { return m_time_event_terminate; } set { m_time_event_terminate = value; }}

		protected virtual eRESULT	ProcessEvent_start(DateTime _time)
		{
			return eRESULT.SUCCESS;
		}
		protected virtual eRESULT	ProcessEvent_end(DateTime _time)
		{
			return eRESULT.SUCCESS;
		}

	// implementation) 
		public override long		ProcessExecute(ulong _return, ulong _param)
		{
			// check) 
			if(this.Now != eOBJECT_STATE.RUNNING)
				return 0;

			// 1) get now utc time
			var time_now = DateTime.UtcNow;

			if(this.m_is_event_start)
			{
				// check) 
				Debug.Assert(time_now >= m_time_event_start);

				// - set flag
				m_is_event_start = false;

				// - set next time as zero 'm_time_event_terminate'
				EntityStatus.timeNext = m_time_event_terminate;

				// - append result log
				var presult_event = _AppendResultLog("ProcessEvent_start");

				try
				{
					// statistics) 
					EntityStatus.StatisticsTry();

					// - call 'ProcessEvent_start'
					presult_event.Result = ProcessEvent_start(time_now);

					// statistics) 
					if (presult_event.Result == eRESULT.SUCCESS)
					{
						EntityStatus.StatisticsSucceeded();
					}

					// - transit message
					//EntityManager.Instance.TransmitMessage(new sMESSAGE(eMESSAGE.SERVER.EVENT.EXECUTE_EVENT, this as IEntity));
				}
				catch(System.Exception /*e*/)
				{
					// - set reault as 'eRESULT::EXCEPTION'
					presult_event.Result = eRESULT.EXCEPTION;

					// statistics) 
					EntityStatus.StatisticsFailed();

					// event log)
					LOG_EVENT(null, eLOG_TYPE.EXCEPTION, eLOG_LEVEL.NORMAL, "Exception [" + this.Name + "]");
				}

				// - append log
				_AppendResultLogPost(presult_event);

				// - Stop on complete
				if (EntityStatus.State != eSTATE.DONE)
				{
					EntityManager.Instance._ProcessPushToHeap(this);
				}
				else
				{
					// - Stop
					Stop();
				}
			}
			else if(m_is_event_terminate)
			{
				// check) 
				Debug.Assert(time_now >= m_time_event_terminate);

				// - set flag
				this.m_is_event_terminate = false;

				// - set next time as zero
				this.EntityStatus.timeNext = new DateTime(0);

				// - append result log
				var presult_event = _AppendResultLog("ProcessEvent_end");

				try
				{
					// statistics) 
					this.EntityStatus.StatisticsTry();

					// - call 'ProcessEvent_end'
					presult_event.Result = ProcessEvent_end(time_now);

					// statistics) 
					if (presult_event.Result == eRESULT.SUCCESS)
					{
						this.EntityStatus.StatisticsSucceeded();
					}

					// - transit message
					//EntityManager.Instance.TransmitMessage(new sMESSAGE(eMESSAGE.SERVER.EVENT.EXECUTE_EVENT, this as IEntity));
				}
				catch(System.Exception /*e*/)
				{
					// - set reault as 'eRESULT::EXCEPTION'
					presult_event.Result = eRESULT.EXCEPTION;

					// statistics) 
					this.EntityStatus.StatisticsFailed();

					// event log)
					LOG_EVENT(null, eLOG_TYPE.EXCEPTION, eLOG_LEVEL.NORMAL, "Exception [" + this.Name +"]");
				}

				// - append result log
				this._AppendResultLogPost(presult_event);

				// - change State to 'eSTATE.DONE'
				this.EntityStatus.State = eSTATE.DONE;

				// - Stop
				this.Stop();
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
