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
using System.Threading;
using System.Collections.Generic;

//----------------------------------------------------------------------------
//
//  CGDK.executor_single_execute
//
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public class executor_single_execute : Nexecutor_thread
	{
	// Constructor/Destructor
		public executor_single_execute()
		{
			// 1) List Schedulable을 생성한다.
			m_priority_queue_executable	 = new List<EXECUTION_AT>();

			lock(m_priority_queue_executable)
			{
				// 2) Capacity를 16384로 설정한다.
				m_priority_queue_executable.Capacity = 16384;

				// 3) ...
				m_priority_queue_executable.Add(new EXECUTION_AT(0, null, 0));
			}

			// 4) default executor를 설정한다.
			executor = system_executor.executor;
		}
		~executor_single_execute()
		{
			cancel_all_executable();
		}

	// public) 
		public bool						post_at(long _tick_time, Iexecutable _executable, ulong _param = 0)
		{
			// check) _executable null이면 그냥 false를 리턴한다.
			if(_executable == null)
				return false;

			// 1) 추가한다.
			push_executable(new EXECUTION_AT(_tick_time, _executable, _param));

			// trace) 
			LOG.INFO_LOW(null, "(info) CGExecute: Executable is posted["+_executable.GetHashCode().ToString()+"] (CGDK.ExecuteClasses.CExecutorSingleExecute.PostExecuteAt)");

			// return) 
			return	true;
		}
		public bool						post_after(long _tick_duffer, Iexecutable _executable, ulong _param = 0)
		{
			return	post_at(System.DateTime.Now.Ticks + _tick_duffer, _executable, _param);
		}

		public bool						cancel_executable(Iexecutable _executable)
		{
			lock(m_priority_queue_executable)
			{
				// 1) 해당 객체를 찾는다.
				int	post_find = m_priority_queue_executable.FindIndex(x=>x.executable == _executable);

				// check) 찾지 못했으면 false를 리턴한다.
				if(post_find < 0)
					return false;

				// 2) 마지막 객체를 얻는다.
				var	iter_target = m_priority_queue_executable[m_priority_queue_executable.Count-1];
				var	qw_compare = iter_target.tick_execute;

				// 3) 객체의 Index를 얻는다.
				var	index = post_find;
		
				// 4) Target객체를 제거하고 제일 마지막 객체를 그 위치로 가져 온다.
				m_priority_queue_executable.RemoveAt(m_priority_queue_executable.Count-1);

				// check) 
				if(m_priority_queue_executable.Count < 2)
					return true;

				// CaseA) Heap Up
				if(m_priority_queue_executable[index/2].tick_execute>qw_compare)
				{
					// - Heap Up한다.
					int	pos_pre = post_find;

					while(index!=1)
					{
						// - 나누기 2한다.
						index /= 2;

						var	iter_now = m_priority_queue_executable[index];

						// check) 
						if(iter_now.tick_execute<=qw_compare)
							break;

						// Swap한다.
						m_priority_queue_executable[pos_pre] = iter_now;

						// - 교체...
						pos_pre = index;
					}

					m_priority_queue_executable[pos_pre] = iter_target;
				}
				// CaseB) Heap Down
				else
				{
					// 1) 
					// 3) Size & Parent
					var	size = m_priority_queue_executable.Count-1;
					var	pos_child = (index * 2);
					var	pos_parent = post_find;

					// 4) Heap down
					while(pos_child <= size)
					{
						// - Child를 얻는다.
						var	iter_child = m_priority_queue_executable[pos_child];

						// - Left가 더 크면 Right 선택한다.
						if(pos_child < size && iter_child.tick_execute > m_priority_queue_executable[pos_child+1].tick_execute)
						{
							++pos_child;
						}

						// check) Child가 더 크면 끝냄.
						if(qw_compare <= iter_child.tick_execute)
							break;

						// - 복사
						m_priority_queue_executable[pos_parent] = iter_child;

						// - Parent를 복사...
						pos_parent = pos_child;

						// pos_child = pos_child x 2
						pos_child *= 2;
					}

					m_priority_queue_executable[pos_parent] = iter_target;
				}
			}

			// Trace) 
			LOG.INFO_LOW(null, "(info) execute: cancel executable object ["+_executable.GetHashCode().ToString()+"] (CGDK.ExecuteClasses.CExecutorSingleExecute.CancelExecutable)");

			// Return) 성공!!!
			return true;
		}
		public void						cancel_all_executable()
		{
			lock(m_priority_queue_executable)
			{
				m_priority_queue_executable.Clear();

				m_priority_queue_executable.Add(new EXECUTION_AT(0, null, 0));
			}

			// Trace) 
			LOG.INFO_LOW(null, "(info) execute: all executables are canceled (CGDK.ExecuteClasses.CExecutorSingleExecute.CancelAllExecutable)");
		}

		public Iexecutor				executor
		{
			get { return m_executor;}
			set { m_executor=value;}
		}
		public int						count
		{
			get
			{
				lock(m_priority_queue_executable)
				{
					return (m_priority_queue_executable.Count - 1);
				}
			}
		}

	// implementations) 
		public override bool			execute(int _tick = Timeout.Infinite)
		{
			// check) m_executor가 null이면 그냥 false를 리턴한다.
			if(m_executor == null)
				return false;

			// 1) 현재 tick을 구한다.
			var tick_now = System.DateTime.Now.Ticks;

			lock (m_priority_queue_executable)
			{
				while(m_priority_queue_executable.Count > 1)
				{
					// 2) 제일 앞의 Executable을 얻는다.
					var	pexecutable = m_priority_queue_executable[1];

					// check) 시간이 지나지 않았으면 끝낸다.
					if(pexecutable.tick_execute > tick_now)
						break;

					// 3) 실행한다.
					m_executor.process_post_execute(pexecutable.executable, pexecutable.param);

					// 4) 제일 앞의 Executable을 제거한다.
					pop_executable();
				}
			}

			// Return) 
			return	true;
		}
		private void					push_executable(EXECUTION_AT _executable)
		{
			lock(m_priority_queue_executable)
			{
				// 1) 위치 저장해 놓음.
				var	position = m_priority_queue_executable.Count;

				// 2) 데이터를 Push한다.
				m_priority_queue_executable.Add(new EXECUTION_AT(0, null, 0));

				// 3) Heap Up한다.
				var	pos_pre = m_priority_queue_executable.Count-1;

				while(position!=1)
				{
					// - 나누기 2한다.
					position /= 2;

					var	iter_now = m_priority_queue_executable[position];

					// check) 
					if(iter_now.tick_execute<=_executable.tick_execute)
						break;

					// 값을 복사한다.
					m_priority_queue_executable[pos_pre] = iter_now;

					// - 교체...
					pos_pre = position;
				}

				m_priority_queue_executable[pos_pre] = _executable;
			}
		}
		private void					pop_executable()
		{
			lock(m_priority_queue_executable)
			{
				// 1) 
				var	ptarget = m_priority_queue_executable[m_priority_queue_executable.Count - 1];
				var	tick_compare = ptarget.tick_execute;

				// 2) Target객체를 제거하고 제일 마지막 객체를 그 위치로 가져 온다.
				m_priority_queue_executable.RemoveAt(m_priority_queue_executable.Count - 1);

				// check) 
				if(m_priority_queue_executable.Count<2)
					return;

				// 3) Size & Parent
				var	size = m_priority_queue_executable.Count - 1;
				var	pos_child = 2;
				int	pos_parent = m_priority_queue_executable.Count + 1;

				// 4) Heap down
				while(pos_child<=size)
				{
					// - Child를 얻는다.
					var	iterChild = m_priority_queue_executable[pos_child];

					// - Left가 더 크면 Right 선택한다.
					if(pos_child<size && iterChild.tick_execute>m_priority_queue_executable[pos_child+1].tick_execute)
					{
						++pos_child;
					}

					// check) Child가 더 크면 끝냄.
					if(tick_compare<=iterChild.tick_execute)
						break;

					// - 복사
					m_priority_queue_executable[pos_parent] = iterChild;

					// - Parent를 복사...
					pos_parent = pos_child;

					// iChild = iChild x 2
					pos_child *= 2;
				}

				m_priority_queue_executable[pos_parent] = ptarget;
			}
		}

		private	Iexecutor				m_executor = null;
		private	List<EXECUTION_AT>		m_priority_queue_executable = null;
		
		private struct EXECUTION_AT
		{
			public EXECUTION_AT(long _tickExecute, Iexecutable _executable, ulong _param)
			{
				tick_execute = _tickExecute;
				executable = _executable;
				param = _param;
			}

			public long				tick_execute;
			public Iexecutable		executable;
			public ulong			param;

			public void				swap(EXECUTION_AT _right)	{ var t=_right; _right=this; this=t;}
		};
	}
}
