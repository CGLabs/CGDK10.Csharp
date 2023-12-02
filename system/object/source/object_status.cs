//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                          Network Socket Classes                           *
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
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;

//----------------------------------------------------------------------------
//  <<interface>> CGDK.Cobject_state
//
// 
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public class Cobject_state : Iobject_stateable, Iinitializable, Istartable
	{
	// public)
		// - Object
		public object					target
		{
			get { return m_object; }
			set { m_object = value; }
		}

        // - Object State
        public eOBJECT_STATE			object_state
		{
			get { return (eOBJECT_STATE)m_state; }
			set { exchange_object_state(value);}
		}
		public bool						set_object_state_if(eOBJECT_STATE _value, eOBJECT_STATE _compare)
		{
			// 1) eOBJECT_STATE.NONE 상태일 때만 eOBJECT_STATE.INITIALIZING_PENDING 상태로 바꾼다.
			var statePre = Interlocked.CompareExchange(ref m_state, (int)_value, (int)_compare);

			// Return) 상태가 바뀌었으면 true 아니면 false를 리턴한다.
			return (statePre == (int)_compare);
		}

		// - Initialize/Destroy/Start/Stop
		public bool						initialize(CGDK.context _context)	{ return _initialize(_context); }
		public bool						destroy()							{ return _destroy(); }
		public bool						start(CGDK.context _context)		{ return _start(_context); }
		public bool						stop()								{ return _stop(); }
		public void						Reset()
		{
			target					 = null;
			notify_on_initializing	 = null;
			notify_on_initialize	 = null;
			notify_on_destroying	 = null;
			notify_on_destroy		 = null;
			notify_on_starting		 = null;
			notify_on_start			 = null;
			notify_on_stopping		 = null;
			notify_on_stop			 = null;
		}
		public bool						attach(CGDK.Iobject_stateable _child)
		{
			// 1) 이미 존재하는지 확인한다.
			var	temp = m_container.Find(x=>x==_child);

			// check) 이미 존재하는 객체이므로 그냥 끝낸다.
			if(temp != null)
				return false;

			// 2) 추가한다.
			m_container.Add(_child);

			// Return)
			return true;
		}
		public int						detach(CGDK.Iobject_stateable _child)
		{
			// 1) Remove한다.
			var	result = m_container.Remove(_child);

			// Return)
			return (result) ? 1: 0;
		}
		public IEnumerator 				GetEnumerator()
		{
			return m_container.GetEnumerator();
		}

		// - Delegations
		public delegate_notify_context	notify_on_initializing;	// !@brief 초기화 처리 전 내용을 정의하는 대리자
		public delegate_notify_context	notify_on_initialize;	// !@brief 초기화 내용을 정의하는 대리자
		public delegate_notify			notify_on_destroying;	// !@brief 파괴 전 내용을 정의하는 대리자
		public delegate_notify			notify_on_destroy;		// !@brief 파괴시 처리할 내용을 정의하는 대리자
		
		public delegate_notify_context	notify_on_starting;		//! @brief 시작 전 처리할 내용을 정의하는 대리자
		public delegate_notify_context	notify_on_start;		//! @brief 시작 시 처리할 내용을 정의하는 대리자
		public delegate_notify			notify_on_stopping;		// !@brief 정지 전 처리할 내용을 정의하는 대리자
		public delegate_notify			notify_on_stop;			// !@brief 징저 시 처리할 내용을 정의하는 대리자

	// implementations) 
		private bool					_initialize(CGDK.context _context)	
		{
			// 1) ECGSTATUS_NONE 상태일 때만 ECGSTATUS_INITIALIZING_PENDING 상태로 바꾼다.
			var is_changed = set_object_state_if(eOBJECT_STATE.INITIALIZING_PENDING, eOBJECT_STATE.NONE);

			// check) 상태가 바뀌지 않았으면 false를 리턴하고 끝낸다.
			if (is_changed == false)
				return false;

			try
			{
				// 2) process_initialize()함수를 호출한다.
				process_initialize(_context);
			}
			catch(System.Exception /*e*/)
			{
				try
				{
					// - 상태를 다시 ECGSTATUS_NONE으로 되돌린다.
					object_state = eOBJECT_STATE.NONE;
				}
				catch(System.Exception /*e*/)
				{
				}

				// return) 
				throw;
			}

			// 3) ECGSTATUS_STOPPED 상태로 변경한다.
			object_state	 = eOBJECT_STATE.STOPPED;

			// Return) 
			return	true;
		}
		protected bool					_destroy()
		{
			// 1) 일단 Stop 먼저 한다.
			_object_stop(this);

			// 2) ECGSTATUS_STOPPED 상태일 때만 ECGSTATUS_DESTROYING_PENDING로 상태를 바꾼다.
			var is_changed = set_object_state_if(eOBJECT_STATE.DESTROYING_PENDING, eOBJECT_STATE.STOPPED);

			// check) 상태가 바뀌지 않았을 경우에는 false를 리턴하며 끝낸다.
			if (is_changed == false)
				return false;

			// 3) process_destroy 함수를 호출한다.
			process_destroy();

			// 4) 상태를 ECGSTATUS_NONE로 변경한다.
			object_state = eOBJECT_STATE.NONE;

			// return) 
			return true;
		}
		protected bool					_start(CGDK.context _context)
		{
			// 1) 먼저 initialize한다.
			var is_initialize = _object_initialize(this, _context);

			// 2) 만약 이전 상태가 STOPPED상태면 START_PENDING상태로 변경한다.
			var is_changed = set_object_state_if(eOBJECT_STATE.START_PENDING, eOBJECT_STATE.STOPPED);

			// check) 상태가 바뀌지 않았을 경우에는 false를 리턴하며 끝낸다.
			if (is_changed == false)
				return false;

			try
			{
				// 3) 자신을 먼저 Start처리한다.
				process_start(_context);
			}
			catch(System.Exception)
			{
				// - Stop상태로 되돌린다.
				object_state = eOBJECT_STATE.STOPPED;

				try
				{
					// - Destroy한다.
					if (is_initialize)
					{
						_object_destroy(this);
					}
				}
				catch(System.Exception /*e*/)
				{
				}

				// reraise) 
				throw;
			}

			// 6) Service State를 변경한다.(RUNNING)
			object_state = eOBJECT_STATE.RUNNING;

			// Return) 
			return	true;
		}
		protected bool					_stop()
		{
			// 1) ECGSTATUS_RUNNING 상태일 때만 ECGSTATUS_STOP_PENDING 상태로 변경한다.
			var is_changed = set_object_state_if(eOBJECT_STATE.STOP_PENDING, eOBJECT_STATE.RUNNING);

			// check) 상태가 바뀌지 않았을 경우에는 false를 리턴하며 끝낸다.
			if (is_changed == false)
				return false;

			// 2) process_stop함수를 호출한다.
			process_stop();

			// 3) 상태를 ECGSTATUS_STOPPED로 변경한다.
			object_state = eOBJECT_STATE.STOPPED;

			// Return) 
			return	true;
		}

		protected virtual void			process_initialize(CGDK.context _context)
		{
			// 1) Nameable 객체 포인터를 얻고 현재의 Context를 저장해 놓는다.
			var	pnameable = m_object as Inameable;
			var	pname = (pnameable != null && pnameable.name != null) ? pnameable.name : null;

			// 2) 기존 Context 저장
			context	context_now	 = _context;

			// 3) Context를 변경한다.
			if (context_now != null && pname != null)
			{
				var	context_child = context_now[pname];

				if (context_child.exist)
				{
                    context_now = context_child;
				}
			}

			// 4) OnInitializing 함수를 호출한다.
			if(notify_on_initializing != null)
			{
				try
				{
					notify_on_initializing(this, context_now);
				}
				catch(System.Exception)
				{
					// log) 
					LOG.ERROR(null, "(Excp) System.Exception occure on '_ProcessInitializing' ['" + pname + "'] (" + System.Reflection.MethodBase.GetCurrentMethod().Name + ") ");

					// reraise)
					throw;
				}
			}

			// 5) Attribute를 확인해서 Child를 자동 생성한다.
			var tempMembers = m_object.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

			foreach(var iter_field in tempMembers)
			{
				var att_auto_object = (CGDK.Attribute.ChildObjbect)System.Attribute.GetCustomAttribute(iter_field, typeof(CGDK.Attribute.ChildObjbect));

				if(att_auto_object != null)
				{
					var	temp_object = iter_field.GetValue(m_object);

					if (temp_object == null)
					{
						var type_field = iter_field.FieldType;

						// - 객체를 생성한다.
						object obj_new = Activator.CreateInstance(type_field);

						// - 'Name' Field를 써넣는다.
						if (att_auto_object.Name != null)
						{
							Inameable obj_nameable = obj_new as Inameable;

							if (obj_nameable == null)
								throw new System.Exception("Exception) '" + iter_field.Name + "' field has Name attribute but has no Nameable interface");

							obj_nameable.name = att_auto_object.Name;
						}
						else
						{
							Inameable obj_nameable = obj_new as Inameable;

							if (obj_nameable != null)
							{
								obj_nameable.name = iter_field.Name;
							}
						}

						// - SetValue
						iter_field.SetValue(m_object, obj_new);

						// - 추가한다.
						m_container.Add(obj_new);
					}
					else
					{
						// - continue if not exist
						if(m_container.Exists( x => x == temp_object))
							continue;

						// - 추가한다.
						m_container.Add(temp_object);
					}
				}
			}

			// Declare) Rollback을 위한 List
			List<object> array_rollback = new List<object>();

			try
			{
				// Declare)
				var iter = m_container.GetEnumerator();

				// - 모든 자식 객체를 Initialize한다.
				while (iter.MoveNext())
				{
					_object_initialize(iter.Current as Iobject_stateable, context_now);
					array_rollback.Add(iter.Current);
				}

				// 4) OnInitialize 함수를 호출한다.
				if(notify_on_initialize!=null)
				{
					notify_on_initialize(this, context_now);
				}
			}
			catch (System.Exception /*e*/)
			{
				// - Initialize했던 자식 객체를 Roll-back한다.
				for (var i = array_rollback.Count; i>0;)
				{
					--i;
					_object_destroy(array_rollback[i] as Iobject_stateable);
				}

				// - OnDestroy 함수를 호출한다.(OnInitialize()함수와 짝함수이므로 호출된다.)
				if(notify_on_destroy!=null)
				{
					try
					{
						notify_on_destroy(this);
					}
					catch (System.Exception /*e2*/)
					{
					}
				}

				// reraise) 
				throw;
			}
		}
		protected virtual void			process_destroy()
		{
			// 1) OnDestroying 함수를 호출한다.
			if(notify_on_destroying != null)
			{
				try
				{
					notify_on_destroying(this);
				}
				catch (System.Exception /*e*/)
				{
				}
			}

			// 2) Enumable을 얻는다.
			var iter = m_container.GetEnumerator();

			// - Reverse를 위한 임시 List Container를 생성한다.
			var array_reverse = new List<object>();

			// - 모두 복사한다.
			while(iter.MoveNext())
			{
				array_reverse.Add(iter.Current);
			}

			// - 모든 자식 객체를 역순으로 Destroy한다.
			for (int i = array_reverse.Count; i > 0; )
			{
				--i;
				_object_destroy(array_reverse[i] as Iobject_stateable);
			}

			// 3) OnDestroy 함수를 호출한다.
			if(notify_on_destroy != null)
			{
				try
				{
					notify_on_destroy(this);
				}
				catch (System.Exception /*e*/)
				{
				}
			}
		}
		protected virtual void			process_start(CGDK.context _context)
		{
			// 1) get nameable object pointer and name
			var	pnameable = m_object as Inameable;
			var	pname = (pnameable != null && pnameable.name != null) ? pnameable.name : null;

			// 2) set context_now
			context	context_now = _context;

			// 3) find child context
			if (context_now != null && pname != null)
			{
				var	context_child = context_now[pname];

				if (context_child.exist)
				{
                    context_now = context_child;
				}
			}

			// 4) OnStarting 함수를 호출한다.
			if(notify_on_starting!=null)
			{
				try
				{
					notify_on_starting(this, context_now);
				}
				catch(System.Exception)
				{
					// log) 
					LOG.ERROR(null, "(Excp) System.Exception occure on '_ProcessStarting' [" + pname + "'] (" + System.Reflection.MethodBase.GetCurrentMethod().Name + ") ");

					// reraise)
					throw;
				}
			}

			// Declare) Rollback을 위한 List
			List<object> array_rollback = new List<object>();

			// 5) Enumbable이 있으면 ...
			try
			{
				// Declare)
				var iter = m_container.GetEnumerator();

				// - 모든 자식 객체를 Initialize한다.
				while (iter.MoveNext())
				{
					_object_start(iter.Current as Iobject_stateable, context_now);
					array_rollback.Add(iter.Current);
				}

				// 4) OnInitialize 함수를 호출한다.
				if(notify_on_start!=null)
				{
					notify_on_start(this, context_now);
				}
			}
			catch (System.Exception /*e*/)
			{
				// - Initialize했던 자식 객체를 Roll-back한다.
				for (var i = array_rollback.Count; i > 0; )
				{
					--i;
					_object_stop(array_rollback[i] as Iobject_stateable);
				}

				// - OnDestroy 함수를 호출한다.(OnInitialize()함수와 짝함수이므로 호출된다.)
				if(notify_on_stop!=null)
				{
					try
					{
						notify_on_stop(this);
					}
					catch (System.Exception /*e2*/)
					{
					}
				}

				// reraise) 
				throw;
			}
		}
		protected virtual void			process_stop()
		{
			// 1) on_stopping 함수를 호출한다.
			if(notify_on_stopping != null)
			{
				try
				{
					notify_on_stopping(this);
				}
				catch (System.Exception /*e*/)
				{
				}
			}

			// 2) Child를 Stop한다.
			for (var iter= m_container.Count; iter>0;)
			{
				--iter;
				_object_stop(m_container[iter] as Iobject_stateable);
			}

			// 3) on_stop 함수를 호출한다.
			if(notify_on_stop!=null)
			{
				try
				{
					notify_on_stop(this);
				}
				catch (System.Exception /*e*/)
				{
				}
			}
		}

		static protected bool			_object_initialize(Iobject_stateable _object_state, CGDK.context _context)
		{
			// check)
			if (_object_state == null)
				return false;

			// 2) Initializable 인터페이스가 있는지 확인한다.
			var object_initializable = _object_state as Iinitializable;

			// Declare)
			var result_return = false;

			// 2) 
			if(object_initializable != null)
			{
				result_return = object_initializable.initialize(_context);
			}
			// 3) Child node를 Initialize한다.
			else
			{
				// - ECGSTATUS_NONE 상태일 때만 ECGSTATUS_INITIALIZING_PENDING 상태로 바꾼다.
				var is_changed = _object_state.set_object_state_if(eOBJECT_STATE.NONE, eOBJECT_STATE.INITIALIZING_PENDING);

				// check) 상태가 바뀌지 않았으면 false를 리턴하며 끝낸다.
				if (is_changed == false)
					return false;

				// Declare) Rollback을 위한 List
				List<object> array_rollback = new List<object>();

				try
				{
					// - Enumable 인터페이스가 있는지 확인한다.
					var objectEnumable = _object_state as IEnumerable;

					// Declare)
					var iter = objectEnumable.GetEnumerator();

					// - 모든 자식 객체를 Initialize한다.
					while (iter.MoveNext())
					{
						_object_initialize(iter.Current as Iobject_stateable, _context);
						array_rollback.Add(iter.Current);
					}
				}
				catch(System.Exception /*e*/)
				{
					// - Initialize했던 자식 객체를 Roll-back한다.
					for (var i = array_rollback.Count; i>0;)
					{
						--i;
						_object_destroy(array_rollback[i] as Iobject_stateable);
					}

					// - 상태를 다시 ECGSTATUS_NONE으로 되돌린다.
					_object_state.object_state = eOBJECT_STATE.NONE;

					// reraise) 
					throw;
				}

				// - ECGSTATUS_STOPPED 상태로 변경한다.
				_object_state.object_state = eOBJECT_STATE.STOPPED;

				// - Return값은 true로...
				result_return = true;
			}

			// return)
			return result_return;
		}
		static protected bool			_object_destroy_child_only(Iobject_stateable _object_state)
		{
			// check)
			if (_object_state == null)
				return false;

			// 1) ECGOBJECT_STATE.STOPPED 상태일 때만 ECGOBJECT_STATE.DESTROYING_PENDING로 상태를 바꾼다.
			var is_changed = _object_state.set_object_state_if(eOBJECT_STATE.STOPPED, eOBJECT_STATE.DESTROYING_PENDING);

			// check) 상태가 바뀌지 않았을 경우에는 false를 리턴하며 끝낸다.
			if(is_changed == false)
				return	false;

			// 2) Enumable을 얻는다.
			var iter = _object_state.GetEnumerator();

			// - Reverse를 위한 임시 List Container를 생성한다.
			var array_reverse = new List<object>();

			// - 모두 복사한다.
			while(iter.MoveNext())
			{
				array_reverse.Add(iter.Current);
			}

			// - 모든 자식 객체를 역순으로 Destroy한다.
			for (var i = array_reverse.Count; i > 0; )
			{
				--i;
				_object_destroy(array_reverse[i] as Iobject_stateable);
			}

			// 3) 상태를 ECGSTATUS_NONE로 변경한다.
			_object_state.object_state = (int)eOBJECT_STATE.NONE;

			// return)
			return	true;
		}
		static protected bool			_object_destroy(Iobject_stateable _object_state)
		{
			// check)
			if (_object_state == null)
				return false;

			// 1) 먼저 Stop를 한다
			_object_stop(_object_state);

			// 2) 자신 Destroy
			var object_destroyable = _object_state as Idestroyable;

			// Declare)
			var result_return = false;

			// 2) Child node를 Destroy한다. (Destroy는 뒤에서 부터 앞으로 Destroy)
			if (object_destroyable != null)
			{
				result_return = object_destroyable.destroy();
			}
			else
			{
				result_return = _object_destroy_child_only(_object_state);
			}

			// return)
			return result_return;
		}
		static protected bool			_object_start(Iobject_stateable _object_state, context _context)
		{
			// check)
			if (_object_state == null)
				return false;

			// 1) 먼저 Initialize한다.
			_object_initialize(_object_state, _context);

			// 2) 자기 자신을 Start한다.
			var object_startable = _object_state as Istartable;

			// Declare)
			var result_return = false;

			// 3-A) ICGStratable을 상속받았으면 Start함수를 호출한다.
			if(object_startable !=null)
			{
				result_return = object_startable.start(_context);
			}
			// 3-B) Child node를 Start한다.
			else
			{
				// - 만약 이전 상태가 STOPPED상태면 START_PENDING상태로 변경한다.
				var is_changed = _object_state.set_object_state_if(eOBJECT_STATE.STOPPED, eOBJECT_STATE.START_PENDING);

				// check) 이전 상태가 STOPPED상태가 아니라면 false를 리턴한다.
				if(is_changed == false)
					return false;

				// Declare) Rollback을 위한 List
				List<object> array_rollback = new List<object>();

				try
				{
					// Declare)
					var iter = _object_state.GetEnumerator();

					// - 모든 자식 객체를 Initialize한다.
					while (iter.MoveNext())
					{
						_object_start(iter.Current as Iobject_stateable, _context);
						array_rollback.Add(iter.Current);
					}
				}
				catch(System.Exception /*e*/)
				{
					// - Initialize했던 자식 객체를 Roll-back한다.
					for (var i = array_rollback.Count; i>0;)
					{
						--i;
						_object_stop(array_rollback[i] as Iobject_stateable);
					}

					// - 상태를 다시 ECGSTATUS_NONE으로 되돌린다.
					_object_state.object_state = eOBJECT_STATE.NONE;

					// reraise) 
					throw;
				}
				// - Service State를 변경한다.(ECGOBJECT_STATE.RUNNING)
				_object_state.object_state = eOBJECT_STATE.RUNNING;

				// - Return값은 true로...
				result_return = true;
			}

			// return)
			return result_return;
		}
		static protected bool			_object_stop(Iobject_stateable _object_state)
		{
			// check)
			if (_object_state == null)
				return false;

			// 1) Child node를 Stop한다. (뒤에서 부터 앞으로 Stop)
			var pobject_startable = _object_state as Istartable;

			// Declare)
			var result_return = false;

			if (pobject_startable != null)
			{
				result_return = pobject_startable.stop();
			}
			else
			{
				result_return = _object_stop_child_only(_object_state);
			}

			// return)
			return result_return;
		}
		static protected bool			_object_stop_child_only(Iobject_stateable _object_state)
		{
			// check)
			if (_object_state == null)
				return false;

			// 1) ECGSTATUS_RUNNING상태일 때만 ECGSTATUS_STOP_PENDING상태로 바꾼다.
			var is_changed = _object_state.set_object_state_if(eOBJECT_STATE.RUNNING, eOBJECT_STATE.STOP_PENDING);

			// check) 상태가 변경되지 않았다면 그냥 끝낸다.
			if(is_changed == false)
				return	false;

			// 2) Enumable을 얻는다.
			var iter = _object_state.GetEnumerator();

			// - Reverse를 위한 임시 List Container를 생성한다.
			var array_reverse = new List<object>();

			// - 모두 복사한다.
			while (iter.MoveNext())
			{
				array_reverse.Add(iter.Current);
			}

			// - 모든 자식 객체를 역순으로 Destroy한다.
			for (var i = array_reverse.Count; i > 0; )
			{
				--i;
				_object_stop(array_reverse[i] as Iobject_stateable);
			}

			// 3) 상태를 ECGSTATUS_STOPPED로 변경한다.
			_object_state.object_state	 = eOBJECT_STATE.STOPPED;

			// return)
			return true;
		}

		private eOBJECT_STATE			exchange_object_state(eOBJECT_STATE _value)
		{
            return (eOBJECT_STATE)Interlocked.Exchange(ref m_state, (int)_value);
        }

        private object					m_object	 = null;
		private	int						m_state		 = (int)eOBJECT_STATE.NONE;
		protected List<object>			m_container	 = new List<object>();
	}
}
