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
	public class EntityIteration : IEntity
	{
	// constructor/destructor)
		public		EntityIteration() : base((Int64)eTYPE.ITERATION)		{ this.m_times = 0; this.m_time_diff_interval = new TimeSpan(0);}
		public		EntityIteration(string _name) : base((Int64)eTYPE.ITERATION) { this.Name = _name; m_times = 0; m_time_diff_interval = new TimeSpan(0);}

		// publics)
		public int					Times									{ get { return m_times; } set { m_times = value; }}
		public TimeSpan				Interval								{ get { return m_time_diff_interval; } set { m_time_diff_interval = value; }}
																		  
	// frameworks)															  
		protected virtual eRESULT	ProcessEvent_iteration(DateTime _time)	{ return eRESULT.FAIL; }


	// implementation)
		public override long		ProcessExecute(ulong _return, ulong _param)
		{
			// check) 
			if(this.Now != eOBJECT_STATE.RUNNING)
				return 0;

			// 1) 현재시간과 다음 시간을 구한다. 
			var time_now = DateTime.UtcNow;
			var timeNext = EntityStatus.timeNext;

			// check) 현재 시간과 차이가 0보다 크면 실행한다. (S_FALSE)
			if (time_now < timeNext)
				return 1;

			// 2) 새로운 Result를 추가한다.
			var presult_event = ProcessAppendResultLog("ProcessEvent_iteration");

			// check)
			Debug.Assert(presult_event != null);

			try
			{
				// statistics) 
				m_entity_status.StatisticsTry();

				// 3) call 'ProcessEvent_iteration'
				presult_event.Result = ProcessEvent_iteration(time_now);

				// statistics) 
				if(presult_event.Result == eRESULT.SUCCESS)
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
				LOG_EVENT(null, eLOG_TYPE.EXCEPTION, eLOG_LEVEL.NORMAL, "Exception [" + Name + "]");
			}

			// 4) Result를 써넣는다.
			this._AppendResultLogPost(presult_event);

			// 5) transit messsage
			//EntityManager.Instance.TransmitMessage(new sMESSAGE(eMESSAGE.SERVER.EVENT.EXECUTE_EVENT, this as IEntity));

			// 6) m_dwInterval을 1 줄인다.
			if(this.m_times > 0)
			{
				--this.m_times;

				if(this.m_times == 0)
				{
					this.EntityStatus.State = eSTATE.DONE;
				}
			}

			// check) Interval이 모두 0이면 안됀다!
			Debug.Assert(this.m_time_diff_interval != TimeSpan.Zero);

			// 7) next time을 새로 설정한다. (interval만큼을 더한다.)
			timeNext += this.m_time_diff_interval;

			// 8) next time을 설정한다.
			this.EntityStatus.timeNext = timeNext;

			// 9) 완료되면 Stop를 한다.
			if (EntityStatus.State != eSTATE.DONE)
			{
				EntityManager.Instance._ProcessPushToHeap(this);
			}
			else
			{
				// - Stop
				this.Stop();
			}

			// return)
			return 0;
		}

		// 7) 횟수 관계
		private int					m_times; // 반복 횟수
		private TimeSpan			m_time_diff_interval; // 반복 간격
	}
}
