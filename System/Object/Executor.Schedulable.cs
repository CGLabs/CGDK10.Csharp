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
//  CGDK.ExecutorSchedulable
//
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public class ExecutorSchedulable : NExecutorThread
	{
	// Constructor/Destructor
		public ExecutorSchedulable()
		{
			// 1) List Schedulable을 생성한다.
			this.m_list_schedulable = new List<ISchedulable>();
		}
		~ExecutorSchedulable()
		{
			this.unregister_all_schedulable();
		}

	// public) 
		public bool						RegisterSchedulable(ISchedulable _schedulable)
		{
			lock(m_list_schedulable)
			{
				// 1) 해당 객체를 찾는다.
				var	bFind = this.m_list_schedulable.Exists(x=>x==_schedulable);

				// check) 이미 존재하면 false를 리턴한다.
				if(bFind==true)
					return	false;

				// 2) 추가한다.
				this.m_list_schedulable.Add(_schedulable);
			}

			// 4) Hook
			_schedulable.ProcessOnRegister();

			// Trace) 
			LOG.INFO_LOW(null, "Prg) Schedulable is registered["+_schedulable+"] (CGDK.ExecuteClasses.CExecutorSchedulable.RegisterSchedulable)");
	
			// Return) 성공~
			return	true;
		}
		public bool						UnregisterSchedulable(ISchedulable _schedulable)
		{
			lock(this.m_list_schedulable)
			{
				// 1) nullptr이면 모든 객체를 제거한다.
				if(_schedulable == null)
				{
					return this.unregister_all_schedulable()!=0;
				}

				// 2) 지운다.
				var	Result = this.m_list_schedulable.Remove(_schedulable);

				// check)
				if(Result==false)
					return	false;
			}

			// 4) Hook함수(Processon_unregister)를 호출한다.
			_schedulable.ProcessOnUnregister();

			// Trace) 
			LOG.INFO_LOW(null, "Prg) Schedulable is unregistered["+_schedulable+"] (CGDK.ExecuteClasses.CExecutorSchedulable.UnregisterSchedulable)");

			// Return) 성공!!!
			return	true;
		}
		public int						unregister_all_schedulable()
		{
			// Declare) 
			List<ISchedulable> list_schedulable;

			// 1) Swap한다.
			lock(this.m_list_schedulable)
			{
				list_schedulable = this.m_list_schedulable;
				this.m_list_schedulable = null;
			}

			// Declare)
			int	Result_count = list_schedulable.Count;

			// 2) 모두 제거한다.
			foreach(var iter in list_schedulable)
			{
				// - Hook함수(Processon_unregister)를 호출한다.
				iter.ProcessOnUnregister();

				// Trace) 
				LOG.INFO_LOW(null, "(info) CGExecute: Schedulable is registered["+iter+"] ()");
			}

			// Return)
			return Result_count;
		}

		public int						Count					
		{
			get	{ return this.m_list_schedulable.Count;}
		}
		public override bool			Execute(int _tick = Timeout.Infinite)
		{
			lock(this.m_list_schedulable)
			{
				foreach(var iter in this.m_list_schedulable)
				{
					iter.ProcessSchedule();
				}
			}

			// Return) 
			return true;
		}

	// implementations) 
		protected override void			_ProcessNotifyStarting(object _object, Context _context)
		{
			// Declare) 
			base._ProcessNotifyStarting(_object, _context);
		}
		protected override void			_ProcessNotifyStopping(object _object)
		{
			base._ProcessNotifyStopping(_object);
		}

		private	List<ISchedulable>	    m_list_schedulable;
	}
}
