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
	public class Entity : IEntity
	{
	// constructor) 
		public							Entity() {}
		public							Entity(string _name, Int64 _type = 0) { base.Name = _name; EntitySetting.Type = _type; }

	// public) 
		public void						RegisterEvent(IEvent _event)
		{
			lock(this.m_vector_event)
			{
				// 1) find event
				var is_exist = this.m_vector_event.Exists( x => x == _event);

				// check) 
				if(is_exist == false)
					return;

				// 2) set next time
				_event.EventStatus.timeNext = _event.EventSetting.timeExecute;
				EntityStatus.timeNext = _event.EventStatus.timeNext;

				// 3) add to queue
				_push_heap(_event);

				// 4) add pevent
				try
				{
					this.m_vector_event.Add(_event);
				}
				catch (System.Exception)
				{
					this._ProcessEraseEventFromHeap(_event);

					// reraise) 
					throw;
				}
			}
		}
		public void						RegisterEvent(List<IEvent> _list_event)
		{
			lock (m_vector_event)
			{
				foreach(var iter in _list_event)
				{
					RegisterEvent(iter);
				}
			}
		}

		public void						UnregisterEvent(IEvent _event)
		{
			// check)
			Debug.Assert(_event != null);

			// check)
			if(_event == null)
				return;

			lock(m_vector_event)
			{
				// 1) find event
				var iter_find_set = this.m_vector_event.FindIndex(x => x == _event);

				// check) 
				if(iter_find_set == -1)
					return;

				// 2) erase entity
				this.m_vector_event.RemoveAt(iter_find_set);

				// 3) erase event from m_queue_event
				this._ProcessEraseEventFromHeap(_event);
			}
		}
		public void						UnRegisterEventAll()
		{
			lock(this.m_vector_event)
			{
				// 1) clear
				this.m_vector_event.Clear();
				this.m_queue_event.Clear();

				// 2) ...
				this.EntityStatus.ResetNextTime();
			}
		}
		public int						EventCount { get { return m_vector_event.Count; } }
		public DateTime					FrontNextTime { get { return (m_queue_event.Count != 0) ? m_queue_event[0].EventStatus.timeNext : new DateTime(0); } }

	// static publc) 
		public static List<IEvent>		MakeEventOnce(DateTime _time_execute, DelegateNotifyProcessEvent _event_function)
		{
			// 1) create List object
			var list_event = new List<IEvent>();

			// 2) once
			{
				// - create event object
				var pevent = new CGDK.Server.TimeEvent.NEvent();

				// - set event setting values
				var EventSetting = new sEVENT_SETTING();
				EventSetting.Name			 = "iteration";
				EventSetting.Type			 = TimeEvent.eTYPE.NONE;
				EventSetting.timeExecute	 = _time_execute;
				EventSetting.timeInterval	 = TimeSpan.Zero;
				EventSetting.countTimes	 = 1;

				// - set ...
				pevent.EventSetting		 = EventSetting;
				pevent.EventStatus.timeNext = _time_execute;
				pevent.EventFunction		 = _event_function;

				// - push
				list_event.Add(pevent);
			}

			// return) 
			return list_event;
		}
		public static List<IEvent>		MakeEventIteration(DateTime _time_first, TimeSpan _duraction, int _count, DelegateNotifyProcessEvent _event_function)
		{
			// 1) create List object
			var list_event = new List<IEvent>();

			// 2) iteeration
			{
				// - create event object
				var pevent = new CGDK.Server.TimeEvent.NEvent();

				// - set event setting values
				var EventSetting = new sEVENT_SETTING();
				EventSetting.Name			 = "iteration";
				EventSetting.Type			 = eTYPE.NONE;
				EventSetting.timeExecute	 = _time_first;
				EventSetting.timeInterval	 = _duraction;
				EventSetting.countTimes	 = _count;

				// - set ...
				pevent.EventSetting = EventSetting;
				pevent.EventStatus.timeNext = _time_first;
				pevent.EventFunction = _event_function;

				// - push
				list_event.Add(pevent);
			}

			// return) 
			return list_event;
		}
		public static List<IEvent>		MakeEventStartStop(DateTime _time_start, DateTime _time_end, int _count, DelegateNotifyProcessEvent _event_function)
		{
			// 1) create List object
			var list_event = new List<IEvent>();

			// 2) Start
			{
				// - create event object
				var pevent = new CGDK.Server.TimeEvent.NEvent();

				// declare)
				var EventSetting = new sEVENT_SETTING();
				EventSetting.Name			 = "Start";
				EventSetting.Type			 = eTYPE.NONE;
				EventSetting.timeExecute	 = _time_start;
				EventSetting.timeInterval	 = TimeSpan.Zero;
				EventSetting.countTimes	 = _count;

				// - set...
				pevent.EventSetting			 = EventSetting;
				pevent.EventStatus.timeNext = EventSetting.timeExecute;
				pevent.EventFunction		 = _event_function;

				// - push
				list_event.Add(pevent);
			}

			// 3) Stop
			{
				// - create event object
				var pevent = new CGDK.Server.TimeEvent.NEvent();

				// declare)
				var EventSetting = new sEVENT_SETTING();
				EventSetting.Name			 = "Stop";
				EventSetting.Type			 = eTYPE.NONE;
				EventSetting.timeExecute	 = _time_end;
				EventSetting.timeInterval	 = TimeSpan.Zero;
				EventSetting.countTimes	 = _count;

				// - set...
				pevent.EventSetting			 = EventSetting;
				pevent.EventStatus.timeNext = EventSetting.timeExecute;
				pevent.EventFunction		 = _event_function;

				// - push
				list_event.Add(pevent);
			}

			// return) 
			return list_event;
		}
		public static List<IEvent>		MakeEventStartIterationOnStop(DateTime _time_start, TimeSpan _duraction, int _count, DateTime _time_end, DelegateNotifyProcessEvent _event_function)
		{
			// 1) create List object
			var list_event = new List<IEvent>();

			// 2) Start
			{
				// - create event object
				var pevent = new CGDK.Server.TimeEvent.NEvent();

				// declare)
				var EventSetting = new sEVENT_SETTING();
				EventSetting.Name			 = "Start";
				EventSetting.Type			 = eTYPE.NONE;
				EventSetting.timeExecute	 = _time_start;
				EventSetting.timeInterval	 = TimeSpan.Zero;
				EventSetting.countTimes	 = _count;

				// - set...
				pevent.EventSetting			 = EventSetting;
				pevent.EventStatus.timeNext = EventSetting.timeExecute;
				pevent.EventFunction		 = _event_function;

				// - push
				list_event.Add(pevent);
			}

			// 3) iteeration
			{
				// - create event object
				var pevent = new CGDK.Server.TimeEvent.NEvent();

				// declare)
				var EventSetting = new sEVENT_SETTING();
				EventSetting.Name			 = "iteration";
				EventSetting.Type			 = eTYPE.NONE;
				EventSetting.timeExecute	 = _time_start + _duraction;
				EventSetting.timeInterval	 = TimeSpan.Zero;
				EventSetting.countTimes	 = _count;

				// - set...
				pevent.EventSetting			 = EventSetting;
				pevent.EventStatus.timeNext = EventSetting.timeExecute;
				pevent.EventFunction		 = _event_function;

				// - push
				list_event.Add(pevent);
			}

			// 4) Stop
			{
				// - create event object
				var pevent = new CGDK.Server.TimeEvent.NEvent();

				// declare)
				var EventSetting = new sEVENT_SETTING();
				EventSetting.Name			 = "Stop";
				EventSetting.Type			 = eTYPE.NONE;
				EventSetting.timeExecute	 = _time_end;
				EventSetting.timeInterval	 = TimeSpan.Zero;
				EventSetting.countTimes	 = _count;

				// - set...
				pevent.EventSetting			 = EventSetting;
				pevent.EventStatus.timeNext = EventSetting.timeExecute;
				pevent.EventFunction		 = _event_function;

				// - push
				list_event.Add(pevent);
			}

			// return) 
			return list_event;
		}

	// framework) 
		protected override void			_ProcessNotifyInitializing(object _object, Context _Context)
		{

			base._ProcessNotifyInitializing(_object, _Context);
		}
		protected override void			_ProcessNotifyInitialize(object _object, Context _Context)
		{
			base._ProcessNotifyInitialize(_object, _Context);
		}
		protected override void			_ProcessNotifyDestroying(object _object)
		{
			base._ProcessNotifyDestroying(_object);
		}
		protected override void			_ProcessNotifyDestroy(object _object)
		{
			base._ProcessNotifyDestroy(_object);
		}

		protected override void			_ProcessNotifyStarting(object _object, Context _Context)
		{
			// 1) create log
			lock(this.m_plog_result)
			{
				this.m_plog_result = new LOG_RECORD();
			}

			// 2) get param (entity setting)
			var entity_setting = _GetParameterEntitySetting(_Context);

			// 3) get param (event settings)
			var vector_EventSetting = _get_parameter_EventSetting_list(_Context);

			lock(this.m_vector_event)
			{
				// 4) set/add EventSetting

				// 5) clear
				this.m_queue_event.Clear();

				// 6) Initialize
				foreach (var iter in m_vector_event)
				{
					// - Reset..
					iter.EventStatus.countRemained = iter.EventSetting.countTimes;

					// - Initialize
					iter.ProcessReset();

					// - push_back
					this.m_queue_event.Add(iter);
				}

				// 7) remake priority queue
				this._MakeHeap();

				// 8) set timeNext
				EntityStatus.timeNext = m_queue_event[0].EventStatus.timeNext;
			}

			// 9) call parents's '_process_starting'
			base._ProcessNotifyStarting(_object, _Context);
		}
		protected override void			_ProcessNotifyStart(object _object, Context _Context)
		{
			// 1) call parents's '_process_start'
			base._ProcessNotifyStart(_object, _Context);

			// 2) register to...
			EntityManager.Instance.RegisterEntity(this);
		}
		protected override void			_ProcessNotifyStopping(object _object)
		{
			// 1) call parents's '_process_stopping'
			base._ProcessNotifyStopping(_object);

			// 2) register to...
			EntityManager.Instance.UnregisterEntity(this);
		}
		protected override void			_ProcessNotifyStop(object _object)
		{
			base._ProcessNotifyStop(_object);
		}

		public override long			ProcessExecute(ulong _return, ulong _param)
		{
			// check) 
			if(this.Now!= eOBJECT_STATE.RUNNING)
				return 0;

			// 1) get now utc time
			var time_now = DateTime.UtcNow;
			bool result = true;

			lock(this.m_vector_event)
			{
				// check) 
				Debug.Assert(m_queue_event.Count != 0);

				// check) 
				Debug.Assert(time_now >= m_queue_event[0].EventStatus.timeNext);
			}

			for(;;)
			{
				// declare) 
				IEvent pevent_front;

				lock(this.m_vector_event)
				{
					// check) 
					if (m_queue_event.Count == 0)
					{
						// - result is false
						EntityStatus.State = eSTATE.DONE;
						result = false;

						// break)
						break;
					}

					// 2) get front entity
					pevent_front = m_queue_event[0];

					// check) 
					if (time_now < pevent_front.EventStatus.timeNext)
						break;

					// 3) pop entity
					_pop_heap();
				}

				// 5) add log
				var presult_event = ProcessAppendResultLog("ProcessExecute_function");

				try
				{
					// statistics) 
					this.EntityStatus.StatisticsTry();

					// 6) run 'ProcessExecute'
					presult_event.Result = pevent_front.ProcessEvent(time_now);

					// statistics) 
					if (presult_event.Result == eRESULT.SUCCESS)
					{
						this.EntityStatus.StatisticsSucceeded();
					}

					// 7) set complete
					if (presult_event.Result == eRESULT.COMPLETE)
					{
						// statistics) 
						this.EntityStatus.StatisticsSucceeded();

						// - set State 
						this.EntityStatus.State = eSTATE.DONE;
					}
					else if (presult_event.Result == eRESULT.CANCEL)
					{
						// statistics) 
						this.EntityStatus.StatisticsFailed();

						// - set State 
						this.EntityStatus.State = eSTATE.DONE;
					}
				}
				catch (System.Exception)
				{
					// - 결과를 Exception으로 설정
					presult_event.Result = eRESULT.EXCEPTION;

					// statistics) 
					this.EntityStatus.StatisticsFailed();

					// event log)
					LOG_EVENT(null, eLOG_TYPE.EXCEPTION, eLOG_LEVEL.NORMAL, "exception [" + Name  + "]");
				}

				// 8) Result를 써넣는다.
				this._AppendResultLogPost(presult_event);

				// 9) Message 날리기..
				//EntityManager.Instance.TransmitMessage(sMESSAGE(eMESSAGE.SERVER.EVENT.EXECUTE_EVENT, this));

				// 10) [남은 실행 횟수]를 줄인다.
				if (pevent_front.EventStatus.countRemained > 0)
				{
					--pevent_front.EventStatus.countRemained;
				}

				// 11) [남은 실행 횟수]가 0이 아니면 다시 추가한다.
				if (pevent_front.EventSetting.countTimes == 0 
				 || pevent_front.EventStatus.countRemained != 0)
				{
					// - 실행한 Event에 대해 Interval만큼 증가시킨다.
					pevent_front.EventStatus.timeNext += pevent_front.EventSetting.timeInterval;
					pevent_front.EventStatus.timeLastExecuted = time_now;

					lock(m_vector_event)
					{
						// - push heap
						_push_heap(pevent_front);
					}
				}

				// 12) set...
				if (this.m_queue_event.Count != 0)
				{
					// - set entity next time
					this.EntityStatus.timeNext = this.m_queue_event[0].EventStatus.timeNext;
				}
			}

			// 9) 완료되면 Stop를 한다.
			if (this.EntityStatus.State == eSTATE.DONE)
			{
				// - Stop
				this.Stop();
			}

			// 10) push_back... if continue
			if (result)
			{
				EntityManager.Instance._ProcessPushToHeap(this);
			}

			// return)
			return 0;
		}

	// implementation)
		protected List<IEvent> m_vector_event;
		protected List<IEvent> m_queue_event;
		
		private class Comparer : IComparer<IEvent>
		{
			public int Compare(IEvent _lhs, IEvent _rhs)
			{
				if (_lhs.EventStatus.timeNext < _rhs.EventStatus.timeNext)
					return 1;
				else
					return (_lhs.EventStatus.timeNext < _rhs.EventStatus.timeNext) ? 0 : -1;
			}
		}

		protected void				_MakeHeap()
		{
			m_queue_event.Sort(1, m_queue_event.Count - 1, new Comparer());
		}
		protected void				_push_heap(IEvent _event)
		{
			lock (m_vector_event)
			{
				// 1) set pos
				var pos_now = m_queue_event.Count;

				// 2) push back item
				m_queue_event.Add(null);

				// 3) heap up
				var pos_child = m_queue_event.Count - 1;

				while (pos_now != 0)
				{
					// - 나누기 2한다.
					pos_now = ((pos_now - 1) >> 1);

					// check) 
					if (m_queue_event[pos_now].EventStatus.timeNext <= _event.EventStatus.timeNext)
						break;

					// - move to down
					m_queue_event[pos_child] = m_queue_event[pos_now];
					pos_child = pos_now;
				}

				// 4) ...
				m_queue_event[pos_child] = _event;
			}
		}
		protected void				_pop_heap()
		{
			lock (m_vector_event)
			{
				// 1) copy end item
				var item_target = m_queue_event[m_queue_event.Count - 1];

				// 2) remove last one
				m_queue_event.RemoveAt(m_queue_event.Count - 1);

				// check) 
				if (m_queue_event.Count == 0)
					return;

				// 3) Size & Parent
				int pos_max = m_queue_event.Count - 1;
				int pos_now = 1;
				var pos_parent = 0;

				// 4) Heap down
				while (pos_now < pos_max)
				{
					// - select left if left > right
					if (pos_now < pos_max && m_queue_event[pos_now].EventStatus.timeNext > m_queue_event[pos_now + 1].EventStatus.timeNext)
					{
						++pos_now;
					}

					// check)
					if (item_target.EventStatus.timeNext <= m_queue_event[pos_now].EventStatus.timeNext)
						break;

					// - move to up
					m_queue_event[pos_parent] = m_queue_event[pos_now];
					pos_parent = pos_now;

					// - set new child node (pos_now = pos_now x 2)
					pos_now = (pos_now << 1) + 1;
				}

				m_queue_event[pos_parent] = item_target;
			}
		}
		protected void				_ProcessEraseEventFromHeap(IEvent _event)
		{
			// check)
			Debug.Assert(_event != null);

			// check)
			if (_event == null)
				return;

			lock (m_vector_event)
			{
				// 1) find _event
				var pos_now = m_queue_event.FindIndex(x => x == _event);

				// check) 
				if (pos_now == -1)
					return;

				// 2) ...
				{
					// - get last node
					var ptarget = m_queue_event[m_queue_event.Count - 1];
					var tick_compare = ptarget.EventStatus.timeNext;

					// - erase event object from queue
					m_queue_event.RemoveAt(m_queue_event.Count - 1);

					// case A) Heap Up
					if (pos_now != 0 && m_queue_event[(pos_now - 1) >> 1].EventStatus.timeNext > tick_compare)
					{
						// - Heap Up한다.
						var pos_child = pos_now;

						do
						{
							// - 나누기 2한다.
							pos_now = ((pos_now - 1) >> 1);

							// check) 
							if (m_queue_event[pos_now].EventStatus.timeNext <= tick_compare)
								break;

							// Swap한다.
							m_queue_event[pos_child] = m_queue_event[pos_now];

							// - 교체...
							pos_child = pos_now;
						} while (pos_now != 0);

						m_queue_event[pos_child] = ptarget;
					}
					// case B) Heap Down
					else
					{
						pos_now = (pos_now << 1) + 1;

						// - Size & Parent
						var pos_max = m_queue_event.Count - 1;
						var pos_parent = pos_now;

						while (pos_now <= pos_max)
						{
							// - select right node if right node is larger then left node
							if (pos_now < pos_max && m_queue_event[pos_now].EventStatus.timeNext > m_queue_event[pos_now + 1].EventStatus.timeNext)
							{
								++pos_now;
							}

							// check) 
							if (tick_compare <= m_queue_event[pos_now].EventStatus.timeNext)
								break;

							// - move to parent node
							m_queue_event[pos_parent] = m_queue_event[pos_now];
							pos_parent = pos_now;

							// - set new pos_now ( pos_now x 2 + 1 )
							pos_now = (pos_now << 1) + 1;
						}

						m_queue_event[pos_parent] = ptarget;
					}
				}

				// 4) Reset next time
				if (m_queue_event.Count != 0)
				{
					EntityStatus.timeNext = m_queue_event[0].EventStatus.timeNext;
				}
				else
				{
					EntityStatus.ResetNextTime();
				}
			}
		}

		static public sENTITY_SETTING _GetParameterEntitySetting(Context _Context)
		{
			// declare) 
			var temp_entity_setting = new sENTITY_SETTING();

			foreach (var iter in _Context.map_node)
			{
				if (iter.Key == "Type")
				{
					temp_entity_setting.Type = (long)iter.Value;
				}
				else if (iter.Key == "timeExecute")
				{
					temp_entity_setting.Setter = (eSETTER)((int)iter.Value);
				}
				else if (iter.Key == "timeInterval")
				{
					temp_entity_setting.Level = (int)iter.Value;
				}
			}

			// return)
			return temp_entity_setting;
		}
		static public sEVENT_SETTING _get_parameter_EventSetting(string _name, Context _Context)
		{
			// declare) 
			var temp_EventSetting = new sEVENT_SETTING();

			// 1) set Name
			temp_EventSetting.Name = _name;

			// 2) ...
			foreach (var iter in _Context.map_node)
			{
				if (iter.Key == "Type")
				{
					temp_EventSetting.Type = (eTYPE)((int)iter.Value);
				}
				else if (iter.Key == "timeExecute")
				{
					temp_EventSetting.timeExecute = DateTime.Parse((string)iter.Value);
				}
				else if (iter.Key == "timeInterval")
				{
					temp_EventSetting.timeInterval = TimeSpan.Parse((string)iter.Value);
				}
				else if (iter.Key == "countTimes")
				{
					temp_EventSetting.countTimes = (int)iter.Value;
				}
			}

			// return)
			return temp_EventSetting;
		}
		List<sEVENT_SETTING> _get_parameter_EventSetting_list(Context _Context)
		{
			// declare) 
			var vector_EventSetting = new List<sEVENT_SETTING>();

			foreach (var iter in _Context.map_node)
			{
				// 1) parsing EventSetting
				var temp_EventSetting = _get_parameter_EventSetting(iter.Key, iter.Value);

				// 3) push_back EventSetting
				vector_EventSetting.Add(temp_EventSetting);
			}

			// return)
			return vector_EventSetting;
		}
	}
}