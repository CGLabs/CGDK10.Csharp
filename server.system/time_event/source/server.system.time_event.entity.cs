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
	public class entity : Ientity
	{
	// constructor) 
		public							entity() {}
		public							entity(string _name, Int64 _type = 0) { base.name = _name; entity_setting.type = _type; }

	// public) 
		public void						register_event(Ievent _event)
		{
			lock(m_vector_event)
			{
				// 1) find event
				var is_exist = m_vector_event.Exists( x => x == _event);

				// check) 
				if(is_exist == false)
					return;

				// 2) set next time
				_event.event_status.time_next = _event.event_setting.time_execute;
				entity_status.time_next = _event.event_status.time_next;

				// 3) add to queue
				_push_heap(_event);

				// 4) add pevent
				try
				{
					m_vector_event.Add(_event);
				}
				catch (System.Exception)
				{
					_process_erase_event_from_heap(_event);

					// reraise) 
					throw;
				}
			}
		}
		public void						register_event(List<Ievent> _list_event)
		{
			lock (m_vector_event)
			{
				foreach(var iter in _list_event)
				{
					register_event(iter);
				}
			}
		}

			public void						unregister_event(Ievent _event)
		{
			// check)
			Debug.Assert(_event != null);

			// check)
			if(_event == null)
				return;

			lock(m_vector_event)
			{
				// 1) find event
				var iter_find_set = m_vector_event.FindIndex(x => x == _event);

				// check) 
				if(iter_find_set == -1)
					return;

				// 2) erase entity
				m_vector_event.RemoveAt(iter_find_set);

				// 3) erase event from m_queue_event
				_process_erase_event_from_heap(_event);
			}
		}
		public void						unregister_event_all()
		{
			lock(m_vector_event)
			{
				// 1) clear
				m_vector_event.Clear();
				m_queue_event.Clear();

				// 2) ...
				entity_status.reset_next_time();
			}
		}
		public int						event_count { get { return m_vector_event.Count; } }
		public DateTime					front_next_time { get { return (m_queue_event.Count != 0) ? m_queue_event[0].event_status.time_next : new DateTime(0); } }

	// static publc) 
		public static List<Ievent>		make_event_once(DateTime _time_execute, delegate_notify_process_event _event_function)
		{
			// 1) create List object
			var list_event = new List<Ievent>();

			// 2) once
			{
				// - create event object
				var pevent = new CGDK.server.time_event.Nevent();

				// - set event setting values
				sEVENT_SETTING event_setting = new sEVENT_SETTING();
				event_setting.name			 = "iteration";
				event_setting.type			 = time_event.eTYPE.NONE;
				event_setting.time_execute	 = _time_execute;
				event_setting.time_interval	 = TimeSpan.Zero;
				event_setting.count_times	 = 1;

				// - set ...
				pevent.event_setting		 = event_setting;
				pevent.event_status.time_next = _time_execute;
				pevent.event_function		 = _event_function;

				// - push
				list_event.Add(pevent);
			}

			// return) 
			return list_event;
		}
		public static List<Ievent>		make_event_iteration(DateTime _time_first, TimeSpan _duraction, int _count, delegate_notify_process_event _event_function)
		{
			// 1) create List object
			var list_event = new List<Ievent>();

			// 2) iteeration
			{
				// - create event object
				var pevent = new CGDK.server.time_event.Nevent();

				// - set event setting values
				sEVENT_SETTING event_setting = new sEVENT_SETTING();
				event_setting.name			 = "iteration";
				event_setting.type			 = eTYPE.NONE;
				event_setting.time_execute	 = _time_first;
				event_setting.time_interval	 = _duraction;
				event_setting.count_times	 = _count;

				// - set ...
				pevent.event_setting		 = event_setting;
				pevent.event_status.time_next = _time_first;
				pevent.event_function		 = _event_function;

				// - push
				list_event.Add(pevent);
			}

			// return) 
			return list_event;
		}
		public static List<Ievent>		make_event_start_stop(DateTime _time_start, DateTime _time_end, int _count, delegate_notify_process_event _event_function)
		{
			// 1) create List object
			var list_event = new List<Ievent>();

			// 2) start
			{
				// - create event object
				var pevent = new CGDK.server.time_event.Nevent();

				// declare)
				sEVENT_SETTING event_setting = new sEVENT_SETTING();
				event_setting.name			 = "start";
				event_setting.type			 = eTYPE.NONE;
				event_setting.time_execute	 = _time_start;
				event_setting.time_interval	 = TimeSpan.Zero;
				event_setting.count_times	 = _count;

				// - set...
				pevent.event_setting		 = event_setting;
				pevent.event_status.time_next = event_setting.time_execute;
				pevent.event_function		 = _event_function;

				// - push
				list_event.Add(pevent);
			}

			// 3) stop
			{
				// - create event object
				var pevent = new CGDK.server.time_event.Nevent();

				// declare)
				sEVENT_SETTING event_setting = new sEVENT_SETTING();
				event_setting.name			 = "stop";
				event_setting.type			 = eTYPE.NONE;
				event_setting.time_execute	 = _time_end;
				event_setting.time_interval	 = TimeSpan.Zero;
				event_setting.count_times	 = _count;

				// - set...
				pevent.event_setting		 = event_setting;
				pevent.event_status.time_next = event_setting.time_execute;
				pevent.event_function		 = _event_function;

				// - push
				list_event.Add(pevent);
			}

			// return) 
			return list_event;
		}
		public static List<Ievent>		make_event_start_iteration_stop(DateTime _time_start, TimeSpan _duraction, int _count, DateTime _time_end, delegate_notify_process_event _event_function)
		{
			// 1) create List object
			var list_event = new List<Ievent>();

			// 2) start
			{
				// - create event object
				var pevent = new CGDK.server.time_event.Nevent();

				// declare)
				sEVENT_SETTING event_setting = new sEVENT_SETTING();
				event_setting.name			 = "start";
				event_setting.type			 = eTYPE.NONE;
				event_setting.time_execute	 = _time_start;
				event_setting.time_interval	 = TimeSpan.Zero;
				event_setting.count_times	 = _count;

				// - set...
				pevent.event_setting		 = event_setting;
				pevent.event_status.time_next = event_setting.time_execute;
				pevent.event_function		 = _event_function;

				// - push
				list_event.Add(pevent);
			}

			// 3) iteeration
			{
				// - create event object
				var pevent = new CGDK.server.time_event.Nevent();

				// declare)
				sEVENT_SETTING event_setting = new sEVENT_SETTING();
				event_setting.name			 = "iteration";
				event_setting.type			 = eTYPE.NONE;
				event_setting.time_execute	 = _time_start + _duraction;
				event_setting.time_interval	 = TimeSpan.Zero;
				event_setting.count_times	 = _count;

				// - set...
				pevent.event_setting		 = event_setting;
				pevent.event_status.time_next = event_setting.time_execute;
				pevent.event_function		 = _event_function;

				// - push
				list_event.Add(pevent);
			}

			// 4) stop
			{
				// - create event object
				var pevent = new CGDK.server.time_event.Nevent();

				// declare)
				sEVENT_SETTING event_setting = new sEVENT_SETTING();
				event_setting.name			 = "stop";
				event_setting.type			 = eTYPE.NONE;
				event_setting.time_execute	 = _time_end;
				event_setting.time_interval	 = TimeSpan.Zero;
				event_setting.count_times	 = _count;

				// - set...
				pevent.event_setting		 = event_setting;
				pevent.event_status.time_next = event_setting.time_execute;
				pevent.event_function		 = _event_function;

				// - push
				list_event.Add(pevent);
			}

			// return) 
			return list_event;
		}

	// framework) 
		protected override void			_process_notify_initializing(object _object, context _context)
		{

			base._process_notify_initializing(_object, _context);
		}
		protected override void			_process_notify_initialize(object _object, context _context)
		{
			base._process_notify_initialize(_object, _context);
		}
		protected override void			_process_notify_destroying(object _object)
		{
			base._process_notify_destroying(_object);
		}
		protected override void			_process_notify_destroy(object _object)
		{
			base._process_notify_destroy(_object);
		}

		protected override void			_process_notify_starting(object _object, context _context)
		{
			// 1) create log
			lock(m_plog_result)
			{
				m_plog_result = new LOG_RECORD();
			}

			// 2) get param (entity setting)
			var entity_setting = _get_parameter_entity_setting(_context);

			// 3) get param (event settings)
			var vector_event_setting = _get_parameter_event_setting_list(_context);

			lock(m_vector_event)
			{
				// 4) set/add event_setting

				// 5) clear
				m_queue_event.Clear();

				// 6) initialize
				foreach (var iter in m_vector_event)
				{
					// - reset..
					iter.event_status.count_remained = iter.event_setting.count_times;

					// - initialize
					iter.process_reset();

					// - push_back
					m_queue_event.Add(iter);
				}

				// 7) remake priority queue
				_make_heap();

				// 8) set time_next
				entity_status.time_next = m_queue_event[0].event_status.time_next;
			}

			// 9) call parents's '_process_starting'
			base._process_notify_starting(_object, _context);
		}
		protected override void			_process_notify_start(object _object, context _context)
		{
			// 1) call parents's '_process_start'
			base._process_notify_start(_object, _context);

			// 2) register to...
			entity_manager.instance.register_entity(this);
		}
		protected override void			_process_notify_stopping(object _object)
		{
			// 1) call parents's '_process_stopping'
			base._process_notify_stopping(_object);

			// 2) register to...
			entity_manager.instance.unregister_entity(this);
		}
		protected override void			_process_notify_stop(object _object)
		{
			base._process_notify_stop(_object);
		}

		public override long			process_execute(ulong _return, ulong _param)
		{
			// check) 
			if(object_state != eOBJECT_STATE.RUNNING)
				return 0;

			// 1) get now utc time
			var time_now = DateTime.UtcNow;
			bool result = true;

			lock(m_vector_event)
			{
				// check) 
				Debug.Assert(m_queue_event.Count != 0);

				// check) 
				Debug.Assert(time_now >= m_queue_event[0].event_status.time_next);
			}

			for(;;)
			{
				// declare) 
				Ievent pevent_front;

				lock(m_vector_event)
				{
					// check) 
					if (m_queue_event.Count == 0)
					{
						// - result is false
						entity_status.state = eSTATE.DONE;
						result = false;

						// break)
						break;
					}

					// 2) get front entity
					pevent_front = m_queue_event[0];

					// check) 
					if (time_now < pevent_front.event_status.time_next)
						break;

					// 3) pop entity
					_pop_heap();
				}

				// 5) add log
				var presult_event = _append_result_log("process_execute_function");

				try
				{
					// statistics) 
					entity_status.statistics_try();

					// 6) run 'process_execute'
					presult_event.result = pevent_front.process_event(time_now);

					// statistics) 
					if (presult_event.result == eRESULT.SUCCESS)
					{
						entity_status.statistics_succeeded();
					}

					// 7) set complete
					if (presult_event.result == eRESULT.COMPLETE)
					{
						// statistics) 
						entity_status.statistics_succeeded();

						// - set state 
						entity_status.state = eSTATE.DONE;
					}
					else if (presult_event.result == eRESULT.CANCEL)
					{
						// statistics) 
						entity_status.statistics_failed();

						// - set state 
						entity_status.state = eSTATE.DONE;
					}
				}
				catch (System.Exception)
				{
					// - 결과를 Exception으로 설정
					presult_event.result = eRESULT.EXCEPTION;

					// statistics) 
					entity_status.statistics_failed();

					// event log)
					LOG_EVENT(null, eLOG_TYPE.EXCEPTION, eLOG_LEVEL.NORMAL, "exception [" + name  + "]");
				}

				// 8) Result를 써넣는다.
				_append_result_log_post(presult_event);

				// 9) Message 날리기..
				//entity_manager.instance.transmit_message(sMESSAGE(eMESSAGE.SERVER.EVENT.EXECUTE_EVENT, this));

				// 10) [남은 실행 횟수]를 줄인다.
				if (pevent_front.event_status.count_remained > 0)
				{
					--pevent_front.event_status.count_remained;
				}

				// 11) [남은 실행 횟수]가 0이 아니면 다시 추가한다.
				if (pevent_front.event_setting.count_times == 0 
				 || pevent_front.event_status.count_remained != 0)
				{
					// - 실행한 Event에 대해 Interval만큼 증가시킨다.
					pevent_front.event_status.time_next += pevent_front.event_setting.time_interval;
					pevent_front.event_status.time_last_executed = time_now;

					lock(m_vector_event)
					{
						// - push heap
						_push_heap(pevent_front);
					}
				}

				// 12) set...
				if (m_queue_event.Count != 0)
				{
					// - set entity next time
					entity_status.time_next = m_queue_event[0].event_status.time_next;
				}
			}

			// 9) 완료되면 stop를 한다.
			if (entity_status.state == eSTATE.DONE)
			{
				// - stop
				stop();
			}

			// 10) push_back... if continue
			if (result)
			{
				entity_manager.instance._process_push_to_heap(this);
			}

			// return)
			return 0;
		}

	// implementation)
		protected List<Ievent> m_vector_event;
		protected List<Ievent> m_queue_event;
		
		private class comparer : IComparer<Ievent>
		{
			public int Compare(Ievent _lhs, Ievent _rhs)
			{
				if (_lhs.event_status.time_next < _rhs.event_status.time_next)
					return 1;
				else
					return (_lhs.event_status.time_next < _rhs.event_status.time_next) ? 0 : -1;
			}
		}

		protected void				_make_heap()
		{
			m_queue_event.Sort(1, m_queue_event.Count - 1, new comparer());
		}
		protected void				_push_heap(Ievent _event)
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
					if (m_queue_event[pos_now].event_status.time_next <= _event.event_status.time_next)
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
					if (pos_now < pos_max && m_queue_event[pos_now].event_status.time_next > m_queue_event[pos_now + 1].event_status.time_next)
					{
						++pos_now;
					}

					// check)
					if (item_target.event_status.time_next <= m_queue_event[pos_now].event_status.time_next)
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
		protected void				_process_erase_event_from_heap(Ievent _event)
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
					var tick_compare = ptarget.event_status.time_next;

					// - erase event object from queue
					m_queue_event.RemoveAt(m_queue_event.Count - 1);

					// case A) Heap Up
					if (pos_now != 0 && m_queue_event[(pos_now - 1) >> 1].event_status.time_next > tick_compare)
					{
						// - Heap Up한다.
						var pos_child = pos_now;

						do
						{
							// - 나누기 2한다.
							pos_now = ((pos_now - 1) >> 1);

							// check) 
							if (m_queue_event[pos_now].event_status.time_next <= tick_compare)
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
							if (pos_now < pos_max && m_queue_event[pos_now].event_status.time_next > m_queue_event[pos_now + 1].event_status.time_next)
							{
								++pos_now;
							}

							// check) 
							if (tick_compare <= m_queue_event[pos_now].event_status.time_next)
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

				// 4) reset next time
				if (m_queue_event.Count != 0)
				{
					entity_status.time_next = m_queue_event[0].event_status.time_next;
				}
				else
				{
					entity_status.reset_next_time();
				}
			}
		}

		static public sENTITY_SETTING _get_parameter_entity_setting(context _context)
		{
			// declare) 
			var temp_entity_setting = new sENTITY_SETTING();

			foreach (var iter in _context.map_node)
			{
				if (iter.Key == "type")
				{
					temp_entity_setting.type = (long)iter.Value;
				}
				else if (iter.Key == "time_execute")
				{
					temp_entity_setting.setter = (eSETTER)((int)iter.Value);
				}
				else if (iter.Key == "time_interval")
				{
					temp_entity_setting.level = (int)iter.Value;
				}
			}

			// return)
			return temp_entity_setting;
		}
		static public sEVENT_SETTING _get_parameter_event_setting(string _name, context _context)
		{
			// declare) 
			var temp_event_setting = new sEVENT_SETTING();

			// 1) set name
			temp_event_setting.name = _name;

			// 2) ...
			foreach (var iter in _context.map_node)
			{
				if (iter.Key == "type")
				{
					temp_event_setting.type = (eTYPE)((int)iter.Value);
				}
				else if (iter.Key == "time_execute")
				{
					temp_event_setting.time_execute = DateTime.Parse((string)iter.Value);
				}
				else if (iter.Key == "time_interval")
				{
					temp_event_setting.time_interval = TimeSpan.Parse((string)iter.Value);
				}
				else if (iter.Key == "count_times")
				{
					temp_event_setting.count_times = (int)iter.Value;
				}
			}

			// return)
			return temp_event_setting;
		}
		List<sEVENT_SETTING> _get_parameter_event_setting_list(context _context)
		{
			// declare) 
			var vector_event_setting = new List<sEVENT_SETTING>();

			foreach (var iter in _context.map_node)
			{
				// 1) parsing event_setting
				var temp_event_setting = _get_parameter_event_setting(iter.Key, iter.Value);

				// 3) push_back event_setting
				vector_event_setting.Add(temp_event_setting);
			}

			// return)
			return vector_event_setting;
		}
	}
}