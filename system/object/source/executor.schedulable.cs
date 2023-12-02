//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                               Pool Classes                                *
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
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;

//----------------------------------------------------------------------------
//
//  CGDK.executor_schedulable
//
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public class executor_schedulable : Nexecutor_thread
	{
	// Constructor/Destructor
		public executor_schedulable()
		{
			// 1) List Schedulable을 생성한다.
			m_list_schedulable = new List<Ischedulable>();
		}
		~executor_schedulable()
		{
			unregister_all_schedulable();
		}

	// public) 
		public bool						register_schedulable(Ischedulable _schedulable)
		{
			lock(m_list_schedulable)
			{
				// 1) 해당 객체를 찾는다.
				var	bFind	 = m_list_schedulable.Exists(x=>x==_schedulable);

				// check) 이미 존재하면 false를 리턴한다.
				if(bFind==true)
					return	false;

				// 2) 추가한다.
				m_list_schedulable.Add(_schedulable);
			}

			// 4) Hook
			_schedulable.process_on_register();

			// Trace) 
			LOG.INFO_LOW(null, "Prg) schedulable is registered["+_schedulable+"] (CGDK.ExecuteClasses.CExecutorSchedulable.RegisterSchedulable)");
	
			// Return) 성공~
			return	true;
		}
		public bool						unregister_schedulable(Ischedulable _schedulable)
		{
			lock(m_list_schedulable)
			{
				// 1) nullptr이면 모든 객체를 제거한다.
				if(_schedulable == null)
				{
					return	unregister_all_schedulable()!=0;
				}

				// 2) 지운다.
				var	bResult	 = m_list_schedulable.Remove(_schedulable);

				// check)
				if(bResult==false)
					return	false;
			}

			// 4) Hook함수(Processon_unregister)를 호출한다.
			_schedulable.process_on_unregister();

			// Trace) 
			LOG.INFO_LOW(null, "Prg) Schedulable is unregistered["+_schedulable+"] (CGDK.ExecuteClasses.CExecutorSchedulable.UnregisterSchedulable)");

			// Return) 성공!!!
			return	true;
		}
		public int						unregister_all_schedulable()
		{
			// Declare) 
			List<Ischedulable>	list_schedulable;

			// 1) Swap한다.
			lock(m_list_schedulable)
			{
				list_schedulable = m_list_schedulable;
				m_list_schedulable = null;
			}

			// Declare)
			int	result_count = (int)list_schedulable.Count;

			// 2) 모두 제거한다.
			foreach(var iter in list_schedulable)
			{
				// - Hook함수(Processon_unregister)를 호출한다.
				iter.process_on_unregister();

				// Trace) 
				LOG.INFO_LOW(null, "(info) CGExecute: Schedulable is registered["+iter+"] ()");
			}

			// Return)
			return	result_count;
		}

		public int						count					
		{
			get	{	return m_list_schedulable.Count;}
		}
		public override bool			execute(int _tick = Timeout.Infinite)
		{
			lock(m_list_schedulable)
			{
				foreach(var iter in m_list_schedulable)
				{
					iter.process_schedule();
				}
			}

			// Return) 
			return	true;
		}

	// implementations) 
		protected override void			_process_notify_starting(object _object, context _context)
		{
			// Declare) 
			base._process_notify_starting(_object, _context);
		}
		protected override void			_process_notify_stopping(object _object)
		{
			base._process_notify_stopping(_object);
		}

		private	List<Ischedulable>	    m_list_schedulable;
	}
}
