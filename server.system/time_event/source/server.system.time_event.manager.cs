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
	public class entity_manager :
		Nobject_stateable,
		Iexecutable
	{
	// publics) 
		// 1) add/remove
		public bool						register_entity(Ientity _entity)
		{
			// check) 
			if (_entity == null)
			{
				// - log
				LOG.ERROR(null, "(err) time_event: _pentity is nullptr");

				// return) 
				return false;
			}

			// check) 
			if(object_state != eOBJECT_STATE.RUNNING)
				return false;

			lock (m_vector_entity)
			{
				// 1) 먼저 같은 object가 있는지 확인한다.
				var iter_find = m_vector_entity.Find(iter => iter == _entity);

				// check) 이미 존재하는지 확인한다.
				if (iter_find == null)
					return false;

				//// log) 
				//_entity->_append_result_log("@register");

				// 2) set sevent setup time
				_entity.entity_status.state = eSTATE.RUN;

				// 3) add event object
				m_vector_entity.Add(_entity);

				// 4) push event object
				try
				{
					_process_push_to_heap(_entity);
				}
				catch (System.Exception)
				{
					// - rollback
					m_vector_entity.RemoveAt(m_vector_entity.Count);

					// reraise) 
					throw;
				}
			}

			// 6) transmit message
			//dispatch_message(new sMESSAGE(eMESSAGE.SERVER.EVENT.ADD_EVENT, _entity));

			// return)
			return true;
		}
		public bool						unregister_entity(Ientity _entity)
		{
			// declare) 
			Ientity pevent = null;

			// check)
			if(_entity == null)
			{
				// - log
				LOG.ERROR(null, "(err) time_event::Ientity: _entity is null");

				// return)
				return false;
			}

			lock(m_vector_entity)
			{
				// 1) find entity object
				var index_find = m_vector_entity.FindIndex(x => x==_entity);

				// check) 
				if(index_find == -1)
					return false;

				//try
				//{
				//	// log)
				//	_entity->_append_result_log("@unregister");
				//}
				//catch (...)
				//{
				//}

				// 3) remove from heap
				_process_remove_from_heap(pevent);

				// 4) erase event
				m_vector_entity.RemoveAt(index_find);
			}

			// 5) start _entity
			_entity.stop();

			// 6) transmit message
			//dispatch_message(new sMESSAGE(eMESSAGE.SERVER.EVENT.REMOVE_EVENT, _entity));

			// return)
			return true;
		}
		protected Ientity				unregister_entity(UInt64 _id_entity)
		{
			// declare) 
			time_event.Ientity pentity = null;

			lock(m_vector_entity)
			{
				// check)
				if(m_vector_entity.Count == 0)
					return pentity;

				// 1) find entity
				foreach(var iter in m_vector_entity)
				{
					if(iter.entity_status.id == _id_entity)
					{
						pentity = iter;
						break;
					}
				}

				// check)
				if(pentity == null)
					return null;

				// 2) erase event
				m_vector_entity.Remove(pentity);

				// 4) remove from heap
				_process_remove_from_heap(pentity);
			}
		
			// 5) start _pentity
			pentity.stop();

			// 6) transmit message
			//dispatch_message(new sMESSAGE(eMESSAGE.SERVER.EVENT.REMOVE_EVENT, pentity));

			// return)
			return pentity;
		}
		private void					unregister_entity_all()
		{
			// declare) 
			var vector_entity = new List<Ientity>();

			lock(m_vector_entity)
			{
				// 1) copy entity object
				foreach(var iter in m_vector_entity)
				{
					vector_entity.Add(iter);
				}
			}

			// 3) unregister all events
			foreach(var iter in vector_entity)
			{
				unregister_entity(iter);
			}





			//lock(m_vector_entity)
			//{
			//	while(m_vector_entity.Count!=0)
			//	{
			//		unregister_event(m_vector_entity[0]);
			//	}

			//	// check) 모두 Clear후이므로 m_vector_entity가 empty()여야지만 된다.
			//	Debug.Assert(m_vector_entity.Count==0);
			//}
		}
		public Ientity					find_entity(UInt64 _id_entity)
		{
			lock(m_vector_entity)
			{
				// check) 만약 SetEvent가 비어 있으면 여기서 끝낸다.
				if(m_vector_entity.Count == 0)
					return null;

				// 1) 해당 객체를 찾는다.
				foreach(var iter in m_vector_entity)
				{
					if(iter.entity_status.id == _id_entity)
					{
						return iter;
					}
				}
			}

			return	null;
		}
																				  
		// 2) ...																	  
		public static int				Count										{ get {return instance.process_get_count();}}
		public static bool				is_delete_event_done						{ get { return instance.m_is_delete_done_event;} set {instance.process_set_delete_done_event(value);}}
																				  
		// 3) Get instance															  
		public static entity_manager	instance									{ get {if(m_instance == null) process_init_instance(); return m_instance;}}

		// 4) start/stop
		protected override void			_process_notify_starting(object _object, context _context)
		{
			// 1) call base '_process_notify_starting'
			base._process_notify_starting(_object, _context);

			// 2) Schedulable을 생성한다.
			m_schedulable = new schedulable.Cexecutable();
			m_schedulable.interval = 10 * TimeSpan.TicksPerSecond;
			m_schedulable.executable = this;

			// 3) register
			system_executor.register_schedulable(m_schedulable);
		}
		protected override void			_process_notify_stopping(object _object)
		{
			// 1) unregister
			system_executor.unregister_schedulable(m_schedulable);

			// 2) call base '_process_notify_stopping'
			base._process_notify_stopping(_object);
		}
		public void						_process_push_to_heap(Ientity _entity)
		{

		}
		private void					_process_remove_from_heap(Ientity _entity)
		{

		}
		private Ientity					_process_pop_from_heap()
		{
			return null;
		}

	// implementation)
		private int						process_get_count()							{ return m_vector_entity.Count;}
		private void					process_set_delete_done_event(bool _status)	{ m_is_delete_done_event = _status; }
		private void					process_enable_delete_done_event()			{ m_is_delete_done_event = true;}
		private void					process_disable_delete_done_event()			{ m_is_delete_done_event = false;}
		private bool					process_is_delete_done_event()				{ return m_is_delete_done_event;}

		public long						process_execute(ulong _return, ulong _param)
		{
			// 1) get now time
			var time_now = DateTime.UtcNow;

			lock(m_vector_entity)
			{
				for (;;)
				{
					// check) 
					if (m_queue_entity.Count == 0)
						break;

					// check) 
					if (time_now < m_queue_entity[0].entity_status.time_next)
						break;

					// 2) get front entity
					var pentity = _process_pop_from_heap();

					// check) 
					Debug.Assert(pentity.entity_status.state != eSTATE.DONE);

					// check) continue if event state is not eSTATE::RUN
					if (pentity.entity_status.state != eSTATE.RUN)
						continue;

					// 3) post 'pentity' object;
					system_executor.post(pentity);
				}
			}

			// return) 
			return 0;
		}
		private static void				process_init_instance()
		{
			lock(m_cs_instance)
			{
				// check) 이미 생성되어 있다면 그냥 끝낸다.
				if(m_instance != null)
					return;

				// 1) 객체를 생성한다.
				m_instance = new entity_manager();
			
				// declare)
				context context_now = new context();

				// 2) 시작한다.
				m_instance.start(context_now);
			}
		}
		private static void				process_destroy_instance()
		{
			lock(m_cs_instance)
			{
				// check) 생성되어 있지 않으면 끝낸다.
				if(m_instance == null)
					return;

				// 1) 파괴한다.
				m_instance.stop();

				// 2) ...
				m_instance = null;
			}
		}

		// 1)
		private static object			m_cs_instance = new object();
		private static entity_manager	m_instance;

		// 2) List Event
		private List<Ientity>			m_vector_entity = new List<Ientity>();
		private List<Ientity>			m_queue_entity = new List<Ientity>();

		// 3) flags
		private bool					m_is_delete_done_event;

		private	schedulable.Cexecutable m_schedulable;
	}
}