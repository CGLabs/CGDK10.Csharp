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
	public class EntityManager :
		NObjectStateable,
		IExecutable
	{
	// publics) 
		// 1) add/remove
		public bool						RegisterEntity(IEntity _entity)
		{
			// check) 
			if (_entity == null)
			{
				// - log
				LOG.ERROR(null, "(err) TimeEvent: _pentity is nullptr");

				// return) 
				return false;
			}

			// check) 
			if(this.Now != eOBJECT_STATE.RUNNING)
				return false;

			lock (this.m_vector_entity)
			{
				// 1) 먼저 같은 object가 있는지 확인한다.
				var iter_find = this.m_vector_entity.Find(iter => iter == _entity);

				// check) 이미 존재하는지 확인한다.
				if (iter_find == null)
					return false;

				//// log) 
				//_entity->_AppendResultLog("@register");

				// 2) set sevent setup time
				_entity.EntityStatus.State = eSTATE.RUN;

				// 3) add event object
				this.m_vector_entity.Add(_entity);

				// 4) push event object
				try
				{
					_ProcessPushToHeap(_entity);
				}
				catch (System.Exception)
				{
					// - rollback
					this.m_vector_entity.RemoveAt(this.m_vector_entity.Count);

					// reraise) 
					throw;
				}
			}

			// 6) transmit message
			//dispatch_message(new sMESSAGE(eMESSAGE.SERVER.EVENT.ADD_EVENT, _entity));

			// return)
			return true;
		}
		public bool						UnregisterEntity(IEntity _entity)
		{
			// declare) 
			IEntity pevent = null;

			// check)
			if(_entity == null)
			{
				// - log
				LOG.ERROR(null, "(err) TimeEvent::IEntity: _entity is null");

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
				//	_entity->_AppendResultLog("@unregister");
				//}
				//catch (...)
				//{
				//}

				// 3) remove from heap
				_ProcessRemoveFromHeap(pevent);

				// 4) erase event
				m_vector_entity.RemoveAt(index_find);
			}

			// 5) Start _entity
			_entity.Stop();

			// 6) transmit message
			//dispatch_message(new sMESSAGE(eMESSAGE.SERVER.EVENT.REMOVE_EVENT, _entity));

			// return)
			return true;
		}
		protected IEntity				UnregisterEntity(UInt64 _id_entity)
		{
			// declare) 
			TimeEvent.IEntity pentity = null;

			lock(this.m_vector_entity)
			{
				// check)
				if(this.m_vector_entity.Count == 0)
					return pentity;

				// 1) find entity
				foreach(var iter in m_vector_entity)
				{
					if(iter.EntityStatus.Id == _id_entity)
					{
						pentity = iter;
						break;
					}
				}

				// check)
				if(pentity == null)
					return null;

				// 2) erase event
				this.m_vector_entity.Remove(pentity);

				// 4) remove from heap
				this._ProcessRemoveFromHeap(pentity);
			}
		
			// 5) Start _pentity
			pentity.Stop();

			// 6) transmit message
			//dispatch_message(new sMESSAGE(eMESSAGE.SERVER.EVENT.REMOVE_EVENT, pentity));

			// return)
			return pentity;
		}
		private void					UnregisterEntity_all()
		{
			// declare) 
			var vector_entity = new List<IEntity>();

			lock(this.m_vector_entity)
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
				this.UnregisterEntity(iter);
			}





			//lock(m_vector_entity)
			//{
			//	while(m_vector_entity.Count!=0)
			//	{
			//		UnregisterEvent(m_vector_entity[0]);
			//	}

			//	// check) 모두 Clear후이므로 m_vector_entity가 empty()여야지만 된다.
			//	Debug.Assert(m_vector_entity.Count==0);
			//}
		}
		public IEntity					FindEntity(UInt64 _id_entity)
		{
			lock(this.m_vector_entity)
			{
				// check) 만약 SetEvent가 비어 있으면 여기서 끝낸다.
				if(this.m_vector_entity.Count == 0)
					return null;

				// 1) 해당 객체를 찾는다.
				foreach(var iter in this.m_vector_entity)
				{
					if(iter.EntityStatus.Id == _id_entity)
					{
						return iter;
					}
				}
			}

			return	null;
		}
																				  
		// 2) ...																	  
		public static int				Count									{ get {return Instance.ProcessGetCount();}}
		public static bool				IsDeleteEventDone						{ get { return Instance.m_is_delete_done_event;} set {Instance.ProcessSetDeleteDoneEvent(value);}}
																				  
		// 3) Get Instance															  
		public static EntityManager	Instance									{ get {if(m_Instance == null) ProcessInitInstance(); return m_Instance;}}

		// 4) Start/Stop
		protected override void			_ProcessNotifyStarting(object _object, Context _Context)
		{
			// 1) call base '_ProcessNotifyStarting'
			base._ProcessNotifyStarting(_object, _Context);

			// 2) Schedulable을 생성한다.
			this.m_schedulable = new Schedulable.Executable
			{
				Interval = 10 * TimeSpan.TicksPerSecond,
				Target = this
			};

			// 3) register
			SystemExecutor.RegisterSchedulable(this.m_schedulable);
		}
		protected override void			_ProcessNotifyStopping(object _object)
		{
			// 1) unregister
			SystemExecutor.UnregisterSchedulable(this.m_schedulable);

			// 2) call base '_ProcessNotifyStopping'
			base._ProcessNotifyStopping(_object);
		}
		public void						_ProcessPushToHeap(IEntity _entity)
		{
		}
		private void					_ProcessRemoveFromHeap(IEntity _entity)
		{
		}
		private IEntity					_ProcessPopFromHeap()
		{
			return null;
		}

	// implementation)
		private int						ProcessGetCount() { return this.m_vector_entity.Count;}
		private void					ProcessSetDeleteDoneEvent(bool _status) { this.m_is_delete_done_event = _status; }
		private void					ProcessEnableDeleteDoneEvent() { this.m_is_delete_done_event = true;}
		private void					ProcessDisableDeleteDoneEvent() { this.m_is_delete_done_event = false;}
		private bool					ProcessIsDeleteDoneEvent() { return this.m_is_delete_done_event;}

		public long						ProcessExecute(ulong _return, ulong _param)
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
					if (time_now < m_queue_entity[0].EntityStatus.timeNext)
						break;

					// 2) get front entity
					var pentity = _ProcessPopFromHeap();

					// check) 
					Debug.Assert(pentity.EntityStatus.State != eSTATE.DONE);

					// check) continue if event State is not eSTATE::RUN
					if (pentity.EntityStatus.State != eSTATE.RUN)
						continue;

					// 3) post 'pentity' object;
					SystemExecutor.Post(pentity);
				}
			}

			// return) 
			return 0;
		}
		private static void				ProcessInitInstance()
		{
			lock(m_cs_Instance)
			{
				// check) 이미 생성되어 있다면 그냥 끝낸다.
				if(m_Instance != null)
					return;

				// 1) 객체를 생성한다.
				m_Instance = new EntityManager();
			
				// declare)
				var Context_now = new Context();

				// 2) 시작한다.
				m_Instance.Start(Context_now);
			}
		}
		private static void				ProcessDestroyInstance()
		{
			lock(m_cs_Instance)
			{
				// check) 생성되어 있지 않으면 끝낸다.
				if(m_Instance == null)
					return;

				// 1) 파괴한다.
				m_Instance.Stop();

				// 2) ...
				m_Instance = null;
			}
		}

		// 1)
		private static object			m_cs_Instance = new();
		private static EntityManager	m_Instance;

		// 2) List Event
		private List<IEntity>			m_vector_entity = new();
		private List<IEntity>			m_queue_entity = new();

		// 3) flags
		private bool					m_is_delete_done_event;

		private	Schedulable.Executable m_schedulable;
	}
}