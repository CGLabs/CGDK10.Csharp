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
//  <<interface>> CGDK.ObjectState
//
// 
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public class ObjectState : IObjectStateable, IInitializable, IStartable
	{
	// public)
		// - Object
		public object					Target
		{
			get { return this.m_object; }
			set { this.m_object = value; }
		}

        // - Object State
        public eOBJECT_STATE			Now
		{
			get { return (eOBJECT_STATE)this.m_state; }
			set { this.ExchangeObjectState(value);}
		}
		public bool						SetObjectStateIf(eOBJECT_STATE _value, eOBJECT_STATE _compare)
		{
			// 1) eOBJECT_STATE.NONE 상태일 때만 eOBJECT_STATE.INITIALIZING_PENDING 상태로 바꾼다.
			var statePre = Interlocked.CompareExchange(ref m_state, (int)_value, (int)_compare);

			// Return) 상태가 바뀌었으면 true 아니면 false를 리턴한다.
			return (statePre == (int)_compare);
		}

		// - Initialize/Destroy/Start/Stop
		public bool						Initialize(CGDK.Context _context)	{ return this._Initialize(_context); }
		public bool						Destroy()							{ return this._Destroy(); }
		public bool						Start(CGDK.Context _context)		{ return this._Start(_context); }
		public bool						Stop()								{ return this._Stop(); }
		public void						Reset()
		{
			this.Target = null;
			this.notifyOnInitializing = null;
			this.notifyOnInitialize = null;
			this.notifyOnDestroying = null;
			this.notifyOnDestroy = null;
			this.notifyOnStarting = null;
			this.notifyOnStart = null;
			this.notifyOnStopping = null;
			this.notifyOnStop = null;
		}
		public bool						Attach(CGDK.IObjectStateable _child)
		{
			// 1) 이미 존재하는지 확인한다.
			var	temp = this.m_container.Find(x=>x==_child);

			// check) 이미 존재하는 객체이므로 그냥 끝낸다.
			if(temp != null)
				return false;

			// 2) 추가한다.
			this.m_container.Add(_child);

			// Return)
			return true;
		}
		public int						Detach(CGDK.IObjectStateable _child)
		{
			// 1) Remove한다.
			var	Result = this.m_container.Remove(_child);

			// Return)
			return (Result) ? 1: 0;
		}
		public IEnumerator 				GetEnumerator()
		{
			return this.m_container.GetEnumerator();
		}

		// - Delegations
		public delegateNotifyContext	notifyOnInitializing;	// !@brief 초기화 처리 전 내용을 정의하는 대리자
		public delegateNotifyContext	notifyOnInitialize;		// !@brief 초기화 내용을 정의하는 대리자
		public delegateNotify			notifyOnDestroying;		// !@brief 파괴 전 내용을 정의하는 대리자
		public delegateNotify			notifyOnDestroy;		// !@brief 파괴시 처리할 내용을 정의하는 대리자
		
		public delegateNotifyContext	notifyOnStarting;		//! @brief 시작 전 처리할 내용을 정의하는 대리자
		public delegateNotifyContext	notifyOnStart;			//! @brief 시작 시 처리할 내용을 정의하는 대리자
		public delegateNotify			notifyOnStopping;		// !@brief 정지 전 처리할 내용을 정의하는 대리자
		public delegateNotify			notifyOnStop;			// !@brief 징저 시 처리할 내용을 정의하는 대리자

	// implementations) 
		private bool					_Initialize(CGDK.Context _context)	
		{
			// 1) ECGSTATUS_NONE 상태일 때만 ECGSTATUS_INITIALIZING_PENDING 상태로 바꾼다.
			var is_changed = this.SetObjectStateIf(eOBJECT_STATE.INITIALIZING_PENDING, eOBJECT_STATE.NONE);

			// check) 상태가 바뀌지 않았으면 false를 리턴하고 끝낸다.
			if (is_changed == false)
				return false;

			try
			{
				// 2) ProcessInitialize()함수를 호출한다.
				 this.ProcessInitialize(_context);
			}
			catch(System.Exception /*e*/)
			{
				try
				{
					// - 상태를 다시 ECGSTATUS_NONE으로 되돌린다.
					this.Now = eOBJECT_STATE.NONE;
				}
				catch(System.Exception /*e*/)
				{
				}

				// return) 
				throw;
			}

			// 3) ECGSTATUS_STOPPED 상태로 변경한다.
			this.Now = eOBJECT_STATE.STOPPED;

			// Return) 
			return	true;
		}
		protected bool					_Destroy()
		{
			// 1) 일단 Stop 먼저 한다.
			_ObjectStop(this);

			// 2) ECGSTATUS_STOPPED 상태일 때만 ECGSTATUS_DESTROYING_PENDING로 상태를 바꾼다.
			var is_changed = this.SetObjectStateIf(eOBJECT_STATE.DESTROYING_PENDING, eOBJECT_STATE.STOPPED);

			// check) 상태가 바뀌지 않았을 경우에는 false를 리턴하며 끝낸다.
			if (is_changed == false)
				return false;

			// 3) ProcessDestroy 함수를 호출한다.
			this.ProcessDestroy();

			// 4) 상태를 ECGSTATUS_NONE로 변경한다.
			this.Now = eOBJECT_STATE.NONE;

			// return) 
			return true;
		}
		protected bool					_Start(CGDK.Context _context)
		{
			// 1) 먼저 Initialize한다.
			var is_initialize = _ObjectInitialize(this, _context);

			// 2) 만약 이전 상태가 STOPPED상태면 START_PENDING상태로 변경한다.
			var is_changed = this.SetObjectStateIf(eOBJECT_STATE.START_PENDING, eOBJECT_STATE.STOPPED);

			// check) 상태가 바뀌지 않았을 경우에는 false를 리턴하며 끝낸다.
			if (is_changed == false)
				return false;

			try
			{
				// 3) 자신을 먼저 Start처리한다.
				this.ProcessStart(_context);
			}
			catch(System.Exception)
			{
				// - Stop상태로 되돌린다.
				this.Now = eOBJECT_STATE.STOPPED;

				try
				{
					// - Destroy한다.
					if (is_initialize)
					{
						_ObjectDestroy(this);
					}
				}
				catch(System.Exception /*e*/)
				{
				}

				// reraise) 
				throw;
			}

			// 6) Service State를 변경한다.(RUNNING)
			this.Now = eOBJECT_STATE.RUNNING;

			// Return) 
			return true;
		}
		protected bool					_Stop()
		{
			// 1) ECGSTATUS_RUNNING 상태일 때만 ECGSTATUS_STOP_PENDING 상태로 변경한다.
			var is_changed = this.SetObjectStateIf(eOBJECT_STATE.STOP_PENDING, eOBJECT_STATE.RUNNING);

			// check) 상태가 바뀌지 않았을 경우에는 false를 리턴하며 끝낸다.
			if (is_changed == false)
				return false;

			// 2) ProcessStop함수를 호출한다.
			this.ProcessStop();

			// 3) 상태를 ECGSTATUS_STOPPED로 변경한다.
			this.Now = eOBJECT_STATE.STOPPED;

			// Return) 
			return true;
		}

		protected virtual void			ProcessInitialize(CGDK.Context _context)
		{
			// 1) Nameable 객체 포인터를 얻고 현재의 Context를 저장해 놓는다.
			var	pnameable = m_object as INameable;
			var	pname = (pnameable != null && pnameable.Name != null) ? pnameable.Name : null;

			// 2) 기존 Context 저장
			Context	context_now	 = _context;

			// 3) Context를 변경한다.
			if (context_now != null && pname != null)
			{
				var	context_child = context_now[pname];

				if (context_child.IsExist)
				{
                    context_now = context_child;
				}
			}

			// 4) OnInitializing 함수를 호출한다.
			if(notifyOnInitializing != null)
			{
				try
				{
					notifyOnInitializing(this, context_now);
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
							INameable obj_nameable = obj_new as INameable;

							if (obj_nameable == null)
								throw new System.Exception("Exception) '" + iter_field.Name + "' field has Name Attribute but has no Nameable interface");

							obj_nameable.Name = att_auto_object.Name;
						}
						else
						{
							INameable obj_nameable = obj_new as INameable;

							if (obj_nameable != null)
							{
								obj_nameable.Name = iter_field.Name;
							}
						}

						// - SetValue
						iter_field.SetValue(m_object, obj_new);

						// - 추가한다.
						this.m_container.Add(obj_new);
					}
					else
					{
						// - continue if not IsExist
						if(this.m_container.Exists( x => x == temp_object))
							continue;

						// - 추가한다.
						this.m_container.Add(temp_object);
					}
				}
			}

			// Declare) Rollback을 위한 List
			List<object> array_rollback = new List<object>();

			try
			{
				// Declare)
				var iter = this.m_container.GetEnumerator();

				// - 모든 자식 객체를 Initialize한다.
				while (iter.MoveNext())
				{
					_ObjectInitialize(iter.Current as IObjectStateable, context_now);
					array_rollback.Add(iter.Current);
				}

				// 4) OnInitialize 함수를 호출한다.
				if(this.notifyOnInitialize != null)
				{
					this.notifyOnInitialize(this, context_now);
				}
			}
			catch (System.Exception /*e*/)
			{
				// - Initialize했던 자식 객체를 Roll-back한다.
				for (var i = array_rollback.Count; i>0;)
				{
					--i;
					_ObjectDestroy(array_rollback[i] as IObjectStateable);
				}

				// - OnDestroy 함수를 호출한다.(OnInitialize()함수와 짝함수이므로 호출된다.)
				if(notifyOnDestroy!=null)
				{
					try
					{
						notifyOnDestroy(this);
					}
					catch (System.Exception /*e2*/)
					{
					}
				}

				// reraise) 
				throw;
			}
		}
		protected virtual void			ProcessDestroy()
		{
			// 1) OnDestroying 함수를 호출한다.
			if(notifyOnDestroying != null)
			{
				try
				{
					notifyOnDestroying(this);
				}
				catch (System.Exception /*e*/)
				{
				}
			}

			// 2) Enumable을 얻는다.
			var iter = this.m_container.GetEnumerator();

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
				_ObjectDestroy(array_reverse[i] as IObjectStateable);
			}

			// 3) OnDestroy 함수를 호출한다.
			if(this.notifyOnDestroy != null)
			{
				try
				{
					this.notifyOnDestroy(this);
				}
				catch (System.Exception /*e*/)
				{
				}
			}
		}
		protected virtual void			ProcessStart(CGDK.Context _context)
		{
			// 1) get nameable object pointer and Name
			var	pnameable = this.m_object as INameable;
			var	pname = (pnameable != null && pnameable.Name != null) ? pnameable.Name : null;

			// 2) set context_now
			Context	context_now = _context;

			// 3) find child Context
			if (context_now != null && pname != null)
			{
				var	context_child = context_now[pname];

				if (context_child.IsExist)
				{
                    context_now = context_child;
				}
			}

			// 4) OnStarting 함수를 호출한다.
			if(notifyOnStarting != null)
			{
				try
				{
					notifyOnStarting(this, context_now);
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
				var iter = this.m_container.GetEnumerator();

				// - 모든 자식 객체를 Initialize한다.
				while (iter.MoveNext())
				{
					_ObjectStart(iter.Current as IObjectStateable, context_now);
					array_rollback.Add(iter.Current);
				}

				// 4) OnInitialize 함수를 호출한다.
				if(notifyOnStart!=null)
				{
					notifyOnStart(this, context_now);
				}
			}
			catch (System.Exception /*e*/)
			{
				// - Initialize했던 자식 객체를 Roll-back한다.
				for (var i = array_rollback.Count; i > 0; )
				{
					--i;
					_ObjectStop(array_rollback[i] as IObjectStateable);
				}

				// - OnDestroy 함수를 호출한다.(OnInitialize()함수와 짝함수이므로 호출된다.)
				if(notifyOnStop!=null)
				{
					try
					{
						notifyOnStop(this);
					}
					catch (System.Exception /*e2*/)
					{
					}
				}

				// reraise) 
				throw;
			}
		}
		protected virtual void			ProcessStop()
		{
			// 1) OnStopping 함수를 호출한다.
			if(notifyOnStopping != null)
			{
				try
				{
					notifyOnStopping(this);
				}
				catch (System.Exception /*e*/)
				{
				}
			}

			// 2) Child를 Stop한다.
			for (var iter= m_container.Count; iter>0;)
			{
				--iter;
				_ObjectStop(m_container[iter] as IObjectStateable);
			}

			// 3) OnStop 함수를 호출한다.
			if(notifyOnStop!=null)
			{
				try
				{
					notifyOnStop(this);
				}
				catch (System.Exception /*e*/)
				{
				}
			}
		}

		static protected bool			_ObjectInitialize(IObjectStateable _object_state, CGDK.Context _context)
		{
			// check)
			if (_object_state == null)
				return false;

			// 2) Initializable 인터페이스가 있는지 확인한다.
			var object_initializable = _object_state as IInitializable;

			// Declare)
			var Result_return = false;

			// 2) 
			if(object_initializable != null)
			{
				Result_return = object_initializable.Initialize(_context);
			}
			// 3) Child node를 Initialize한다.
			else
			{
				// - ECGSTATUS_NONE 상태일 때만 ECGSTATUS_INITIALIZING_PENDING 상태로 바꾼다.
				var is_changed = _object_state.SetObjectStateIf(eOBJECT_STATE.NONE, eOBJECT_STATE.INITIALIZING_PENDING);

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
						_ObjectInitialize(iter.Current as IObjectStateable, _context);
						array_rollback.Add(iter.Current);
					}
				}
				catch(System.Exception /*e*/)
				{
					// - Initialize했던 자식 객체를 Roll-back한다.
					for (var i = array_rollback.Count; i>0;)
					{
						--i;
						_ObjectDestroy(array_rollback[i] as IObjectStateable);
					}

					// - 상태를 다시 ECGSTATUS_NONE으로 되돌린다.
					_object_state.Now = eOBJECT_STATE.NONE;

					// reraise) 
					throw;
				}

				// - ECGSTATUS_STOPPED 상태로 변경한다.
				_object_state.Now = eOBJECT_STATE.STOPPED;

				// - Return값은 true로...
				Result_return = true;
			}

			// return)
			return Result_return;
		}
		static protected bool			_ObjectDestroyChildOnly(IObjectStateable _object_state)
		{
			// check)
			if (_object_state == null)
				return false;

			// 1) ECGOBJECT_STATE.STOPPED 상태일 때만 ECGOBJECT_STATE.DESTROYING_PENDING로 상태를 바꾼다.
			var is_changed = _object_state.SetObjectStateIf(eOBJECT_STATE.STOPPED, eOBJECT_STATE.DESTROYING_PENDING);

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
				_ObjectDestroy(array_reverse[i] as IObjectStateable);
			}

			// 3) 상태를 ECGSTATUS_NONE로 변경한다.
			_object_state.Now = (int)eOBJECT_STATE.NONE;

			// return)
			return	true;
		}
		static protected bool			_ObjectDestroy(IObjectStateable _object_state)
		{
			// check)
			if (_object_state == null)
				return false;

			// 1) 먼저 Stop를 한다
			_ObjectStop(_object_state);

			// 2) 자신 Destroy
			var object_destroyable = _object_state as IDestroyable;

			// Declare)
			var Result_return = false;

			// 2) Child node를 Destroy한다. (Destroy는 뒤에서 부터 앞으로 Destroy)
			if (object_destroyable != null)
			{
				Result_return = object_destroyable.Destroy();
			}
			else
			{
				Result_return = _ObjectDestroyChildOnly(_object_state);
			}

			// return)
			return Result_return;
		}
		static protected bool			_ObjectStart(IObjectStateable _object_state, Context _context)
		{
			// check)
			if (_object_state == null)
				return false;

			// 1) 먼저 Initialize한다.
			_ObjectInitialize(_object_state, _context);

			// 2) 자기 자신을 Start한다.
			var object_startable = _object_state as IStartable;

			// Declare)
			var Result_return = false;

			// 3-A) ICGStratable을 상속받았으면 Start함수를 호출한다.
			if(object_startable !=null)
			{
				Result_return = object_startable.Start(_context);
			}
			// 3-B) Child node를 Start한다.
			else
			{
				// - 만약 이전 상태가 STOPPED상태면 START_PENDING상태로 변경한다.
				var is_changed = _object_state.SetObjectStateIf(eOBJECT_STATE.STOPPED, eOBJECT_STATE.START_PENDING);

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
						_ObjectStart(iter.Current as IObjectStateable, _context);
						array_rollback.Add(iter.Current);
					}
				}
				catch(System.Exception /*e*/)
				{
					// - Initialize했던 자식 객체를 Roll-back한다.
					for (var i = array_rollback.Count; i>0;)
					{
						--i;
						_ObjectStop(array_rollback[i] as IObjectStateable);
					}

					// - 상태를 다시 ECGSTATUS_NONE으로 되돌린다.
					_object_state.Now = eOBJECT_STATE.NONE;

					// reraise) 
					throw;
				}
				// - Service State를 변경한다.(ECGOBJECT_STATE.RUNNING)
				_object_state.Now = eOBJECT_STATE.RUNNING;

				// - Return값은 true로...
				Result_return = true;
			}

			// return)
			return Result_return;
		}
		static protected bool			_ObjectStop(IObjectStateable _object_state)
		{
			// check)
			if (_object_state == null)
				return false;

			// 1) Child node를 Stop한다. (뒤에서 부터 앞으로 Stop)
			var pobject_startable = _object_state as IStartable;

			// Declare)
			var Result_return = false;

			if (pobject_startable != null)
			{
				Result_return = pobject_startable.Stop();
			}
			else
			{
				Result_return = _ObjectStopChildOnly(_object_state);
			}

			// return)
			return Result_return;
		}
		static protected bool			_ObjectStopChildOnly(IObjectStateable _object_state)
		{
			// check)
			if (_object_state == null)
				return false;

			// 1) ECGSTATUS_RUNNING상태일 때만 ECGSTATUS_STOP_PENDING상태로 바꾼다.
			var is_changed = _object_state.SetObjectStateIf(eOBJECT_STATE.RUNNING, eOBJECT_STATE.STOP_PENDING);

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
				_ObjectStop(array_reverse[i] as IObjectStateable);
			}

			// 3) 상태를 ECGSTATUS_STOPPED로 변경한다.
			_object_state.Now = eOBJECT_STATE.STOPPED;

			// return)
			return true;
		}

		private eOBJECT_STATE			ExchangeObjectState(eOBJECT_STATE _value)
		{
            return (eOBJECT_STATE)Interlocked.Exchange(ref m_state, (int)_value);
        }

        private object					m_object = null;
		private	int						m_state = (int)eOBJECT_STATE.NONE;
		protected List<object>			m_container = new List<object>();
	}
}
