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
	public class EntityStartIterationEnd : EntityStartEnd
	{
		// constructor/destructor)
		public EntityStartIterationEnd() { }
		public	EntityStartIterationEnd(string _name) : this() { this.Name = _name; }

		// publics)
		public	DateTime			event_time { get { return EntityStatus.timeNext; } set { EntityStatus.timeNext = value; } }
		public	TimeSpan			interval { get { return m_time_diff_interval; } set { m_time_diff_interval = value; } }
		public	int					times { get { return m_times; } set { m_times = value; } }

	// frameworks) 
		protected virtual eRESULT	ProcessEvent_iteration(DateTime _time) { return eRESULT.SUCCESS; }

	// implementation)
		public override long		ProcessExecute(ulong _return, ulong _param)
		{
			// 1) 현재시간을 구한다.
			var time_now = DateTime.UtcNow;

			// 2) Start시간이 지났다면 Start를 수행한다.
			if(this.m_is_event_start)
			{
				this.ProcessExecute_start(time_now);
			}
			// 2) run을 수행한다.
			else if(this.m_times_remained != 0)
			{
				this.ProcessExecute_iteration(time_now);
			}	
			// 4) Terminate Time이 지났다면 OnEventTerminate를 수행한다.
			else if(this.m_is_event_terminate)
			{
				this.ProcessExecute_end(time_now);
			}

			// 5) retry or Stop
			if (this.EntityStatus.State != eSTATE.DONE)
			{
				EntityManager.Instance._ProcessPushToHeap(this);
			}
			else
			{
				this.Stop();
			}

			// return)
			return 0;
		}
		protected void				ProcessExecute_start(DateTime _time_now)
		{
			// check) 
			if(this.Now != eOBJECT_STATE.RUNNING)
				return;

			// check) 
			Debug.Assert(this.m_time_event_start <= _time_now);

			// 1) Start했으므로 false로 설정한다.
			this.m_is_event_start = false;

			// 2) append result log
			var presult_event = _AppendResultLog("ProcessEvent_start");

			try
			{
				// statistics) 
				this.EntityStatus.StatisticsTry();

				// 3) (핵심) ProcessEvent_start함수를 호출한다.
				presult_event.Result = ProcessEvent_start(_time_now);

				// statistics) 
				if (presult_event.Result == eRESULT.SUCCESS)
				{
					this.EntityStatus.StatisticsSucceeded();
				}
			}
			catch(System.Exception)
			{
				// - 결과를 Exception으로 설정
				presult_event.Result = eRESULT.EXCEPTION;

				// statistics) 
				this.EntityStatus.StatisticsFailed();

				// event log)
				LOG_EVENT(null, eLOG_TYPE.EXCEPTION, eLOG_LEVEL.NORMAL, "Exception [" + this.Name + "]");
			}

			// 4) append result log
			_AppendResultLogPost(presult_event);

			// 5) transit messsage
			//EntityManager.Instance.TransmitMessage(new sMESSAGE(eMESSAGE.SERVER.EVENT.EXECUTE_EVENT, this as IEntity));

			// check) Interval이 모두 0이면 안됀다!
			Debug.Assert(m_time_diff_interval != TimeSpan.Zero);

			// 6) Interval만큼을 더해 다음 시간을 설정한다.
			var timeNext = m_time_event_start + m_time_diff_interval;

			if (timeNext < m_time_event_terminate)
			{
				EntityStatus.timeNext = timeNext;
			}
			else
			{
				EntityStatus.timeNext = m_time_event_terminate;
			}

			// 7) m_times_remained를 MAX값으로 리셋한다.
			m_times_remained = m_times;
		}
		protected void				ProcessExecute_iteration(DateTime _time_now)
		{
			// check) 
			if (this.Now != eOBJECT_STATE.RUNNING)
				return;

			// declare)
			var timeNext = EntityStatus.timeNext;

			// check) 
			Debug.Assert(timeNext <= _time_now);

			// 1) append result log
			var presult_event = _AppendResultLog("ProcessEvent_iteration");

			try
			{
				// statistics) 
				this.EntityStatus.StatisticsTry();

				// - call 'ProcessEvent_iteration'
				presult_event.Result = this.ProcessEvent_iteration(_time_now);

				// statistics) 
				if (presult_event.Result == eRESULT.SUCCESS)
				{
					this.EntityStatus.StatisticsSucceeded();
				}
			}
			catch(System.Exception)
			{
				// - 결과를 Exception으로 설정
				presult_event.Result = eRESULT.EXCEPTION;

				// statistics) 
				this.EntityStatus.StatisticsFailed();

				// event log)
				LOG_EVENT(null, eLOG_TYPE.EXCEPTION, eLOG_LEVEL.NORMAL, "Exception [" + EntitySetting.Name + "]");
			}

			// 3) append result log
			this._AppendResultLogPost(presult_event);

			// 4) Message 날리기..
			//EntityManager.Instance.TransmitMessage(new sMESSAGE(eMESSAGE.SERVER.EVENT.EXECUTE_EVENT, this as IEntity));

			// 5) m_times_remained을 1 줄인다.
			if(this.m_times_remained > 0)
			{
				this.m_times_remained--;
			}
			else
			{
				// - ProcessExecute_end
				this.ProcessExecute_end(_time_now);

				// - ...
				this.EntityStatus.State = eSTATE.DONE;
			}

			// check) Interval이 모두 0이면 안됀다!
			Debug.Assert(this.m_time_diff_interval != TimeSpan.Zero);

			// 6) Interval만큼을 더해 다음 시간을 설정한다.
			timeNext = timeNext + this.m_time_diff_interval;

			if (timeNext < this.m_time_event_terminate)
			{
				this.EntityStatus.timeNext = timeNext;
			}
			else
			{
				this.EntityStatus.timeNext = m_time_event_terminate;
			}
		}
		protected void				ProcessExecute_end(DateTime _time_now)
		{
			// check) 
			if (this.Now != eOBJECT_STATE.RUNNING)
				return;

			// 1) 종료처리했으므로 false로 설정한다.
			this.m_is_event_terminate = false;

			// 2) 새로운 Result를 추가한다.
			var presult_event = _AppendResultLog("ProcessEvent_end");

			try
			{
				// statistics) 
				this.EntityStatus.StatisticsTry();

				// 3) (핵심) ProcessEvent_end함수를 호출한다.
				presult_event.Result = ProcessEvent_end(_time_now);

				// statistics) 
				if (presult_event.Result == eRESULT.SUCCESS)
				{
					this.EntityStatus.StatisticsSucceeded();
				}
			}
			catch(System.Exception)
			{
				// - 결과를 Exception으로 설정
				presult_event.Result = eRESULT.EXCEPTION;

				// statistics) 
				this.EntityStatus.StatisticsFailed();

				// event log)
				LOG_EVENT(null, eLOG_TYPE.EXCEPTION, eLOG_LEVEL.NORMAL, "exception [" + Name + "]");
			}

			// 4) Result를 써넣는다.
			this._AppendResultLogPost(presult_event);

			// 5) Message 날리기..
			//EntityManager.Instance.TransmitMessage(new sMESSAGE(eMESSAGE.SERVER.EVENT.EXECUTE_EVENT, this as IEntity));

			// 6) Stop
			this.Stop();

			// 7) set event stste
			this.EntityStatus.State = eSTATE.DONE;

			// 8) Reset event time
			this.EntityStatus.timeNext = new DateTime(0);
		}

		private	int					m_times_remained = -1;	// 현재 남아 있는 반복 횟수
		private	int					m_times = -1;			// 목표 반복 횟수
		private	TimeSpan			m_time_diff_interval;	// 반복 간격
	}
}
