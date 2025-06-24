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
//  CGDK.ExecutorSingleExecute
//
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public class ExecutorSingleExecute : NExecutorThread
	{
		public ExecutorSingleExecute()
		{
			// 1) List Schedulable을 생성한다.
			this.m_priority_queue_executable = new List<EXECUTION_AT>();

			lock(this.m_priority_queue_executable)
			{
				// 2) Capacity를 16384로 설정한다.
				this.m_priority_queue_executable.Capacity = 16384;

				// 3) ...
				this.m_priority_queue_executable.Add(new EXECUTION_AT(0, null, 0));
			}

			// 4) default Executor를 설정한다.
			this.Executor = SystemExecutor.Executor;
		}
		~ExecutorSingleExecute()
		{
			this.CancelAllExecutable();
		}

		public bool PostAt(long _tick_time, IExecutable _executable, ulong _param = 0)
		{
			// check) _executable null이면 그냥 false를 리턴한다.
			if(_executable == null)
				return false;

			// 1) 추가한다.
			this.PushExecutable(new EXECUTION_AT(_tick_time, _executable, _param));

			// Trace) 
			LOG.INFO_LOW(null, "(info) CGExecute: Executable is posted["+_executable.GetHashCode().ToString()+"] (CGDK.ExecuteClasses.CExecutorSingleExecute.PostExecuteAt)");

			// return) 
			return	true;
		}
		public bool PostAfter(long _tick_duffer, IExecutable _executable, ulong _param = 0)
		{
			return this.PostAt(System.DateTime.Now.Ticks + _tick_duffer, _executable, _param);
		}

		public bool CancelExecutable(IExecutable _executable)
		{
			lock(this.m_priority_queue_executable)
			{
				// 1) 해당 객체를 찾는다.
				int	post_find = this.m_priority_queue_executable.FindIndex(x=>x.executable == _executable);

				// check) 찾지 못했으면 false를 리턴한다.
				if(post_find < 0)
					return false;

				// 2) 마지막 객체를 얻는다.
				var	iter_target = this.m_priority_queue_executable[this.m_priority_queue_executable.Count-1];
				var	qw_compare = iter_target.tick_execute;

				// 3) 객체의 Index를 얻는다.
				var	index = post_find;
		
				// 4) Target객체를 제거하고 제일 마지막 객체를 그 위치로 가져 온다.
				this.m_priority_queue_executable.RemoveAt(this.m_priority_queue_executable.Count-1);

				// check) 
				if(this.m_priority_queue_executable.Count < 2)
					return true;

				// CaseA) Heap Up
				if(this.m_priority_queue_executable[index/2].tick_execute>qw_compare)
				{
					// - Heap Up한다.
					int	pos_pre = post_find;

					while(index!=1)
					{
						// - 나누기 2한다.
						index /= 2;

						var	iter_now = this.m_priority_queue_executable[index];

						// check) 
						if(iter_now.tick_execute<=qw_compare)
							break;

						// Swap한다.
						this.m_priority_queue_executable[pos_pre] = iter_now;

						// - 교체...
						pos_pre = index;
					}

					this.m_priority_queue_executable[pos_pre] = iter_target;
				}
				// CaseB) Heap Down
				else
				{
					// 1) 
					// 3) Size & Parent
					var	size = this.m_priority_queue_executable.Count-1;
					var	pos_child = (index * 2);
					var	pos_parent = post_find;

					// 4) Heap down
					while(pos_child <= size)
					{
						// - Child를 얻는다.
						var	iter_child = m_priority_queue_executable[pos_child];

						// - Left가 더 크면 Right 선택한다.
						if(pos_child < size && iter_child.tick_execute > this.m_priority_queue_executable[pos_child+1].tick_execute)
						{
							++pos_child;
						}

						// check) Child가 더 크면 끝냄.
						if(qw_compare <= iter_child.tick_execute)
							break;

						// - 복사
						this.m_priority_queue_executable[pos_parent] = iter_child;

						// - Parent를 복사...
						pos_parent = pos_child;

						// pos_child = pos_child x 2
						pos_child *= 2;
					}

					this.m_priority_queue_executable[pos_parent] = iter_target;
				}
			}

			// Trace) 
			LOG.INFO_LOW(null, "(info) Execute: cancel executable object ["+_executable.GetHashCode().ToString()+"] (CGDK.ExecuteClasses.CExecutorSingleExecute.CancelExecutable)");

			// Return) 성공!!!
			return true;
		}
		public void CancelAllExecutable()
		{
			lock(m_priority_queue_executable)
			{
				this.m_priority_queue_executable.Clear();

				this.m_priority_queue_executable.Add(new EXECUTION_AT(0, null, 0));
			}

			// Trace) 
			LOG.INFO_LOW(null, "(info) Execute: all executables are canceled (CGDK.ExecuteClasses.CExecutorSingleExecute.CancelAllExecutable)");
		}

		public IExecutor Executor
		{
			get { return this.m_executor;}
			set { this.m_executor=value;}
		}
		public int Count
		{
			get
			{
				lock(this.m_priority_queue_executable)
				{
					return (this.m_priority_queue_executable.Count - 1);
				}
			}
		}

		public override bool Execute(int _tick = Timeout.Infinite)
		{
			// check) m_executor가 null이면 그냥 false를 리턴한다.
			if(this.m_executor == null)
				return false;

			// 1) 현재 tick을 구한다.
			var tick_now = System.DateTime.Now.Ticks;

			lock (this.m_priority_queue_executable)
			{
				while(this.m_priority_queue_executable.Count > 1)
				{
					// 2) 제일 앞의 Executable을 얻는다.
					var	pexecutable = this.m_priority_queue_executable[1];

					// check) 시간이 지나지 않았으면 끝낸다.
					if(pexecutable.tick_execute > tick_now)
						break;

					// 3) 실행한다.
					this.m_executor.ProcessPostExecute(pexecutable.executable, pexecutable.param);

					// 4) 제일 앞의 Executable을 제거한다.
					this.PopExecutable();
				}
			}

			// Return) 
			return	true;
		}
		private void PushExecutable(EXECUTION_AT _executable)
		{
			lock(m_priority_queue_executable)
			{
				// 1) 위치 저장해 놓음.
				var	position = this.m_priority_queue_executable.Count;

				// 2) 데이터를 Push한다.
				this.m_priority_queue_executable.Add(new EXECUTION_AT(0, null, 0));

				// 3) Heap Up한다.
				var	pos_pre = this.m_priority_queue_executable.Count-1;

				while(position!=1)
				{
					// - 나누기 2한다.
					position /= 2;

					var	iter_now = this.m_priority_queue_executable[position];

					// check) 
					if(iter_now.tick_execute<=_executable.tick_execute)
						break;

					// 값을 복사한다.
					this.m_priority_queue_executable[pos_pre] = iter_now;

					// - 교체...
					pos_pre = position;
				}

				this.m_priority_queue_executable[pos_pre] = _executable;
			}
		}
		private void PopExecutable()
		{
			lock(this.m_priority_queue_executable)
			{
				// 1) 
				var	ptarget = this.m_priority_queue_executable[this.m_priority_queue_executable.Count - 1];
				var	tick_compare = ptarget.tick_execute;

				// 2) Target객체를 제거하고 제일 마지막 객체를 그 위치로 가져 온다.
				this.m_priority_queue_executable.RemoveAt(this.m_priority_queue_executable.Count - 1);

				// check) 
				if(this.m_priority_queue_executable.Count<2)
					return;

				// 3) Size & Parent
				var	size = this.m_priority_queue_executable.Count - 1;
				var	pos_child = 2;
				int	pos_parent = this.m_priority_queue_executable.Count + 1;

				// 4) Heap down
				while(pos_child<=size)
				{
					// - Child를 얻는다.
					var	iterChild = this.m_priority_queue_executable[pos_child];

					// - Left가 더 크면 Right 선택한다.
					if(pos_child<size && iterChild.tick_execute > this.m_priority_queue_executable[pos_child+1].tick_execute)
					{
						++pos_child;
					}

					// check) Child가 더 크면 끝냄.
					if(tick_compare <= iterChild.tick_execute)
						break;

					// - 복사
					this.m_priority_queue_executable[pos_parent] = iterChild;

					// - Parent를 복사...
					pos_parent = pos_child;

					// iChild = iChild x 2
					pos_child *= 2;
				}

				this.m_priority_queue_executable[pos_parent] = ptarget;
			}
		}

		private	IExecutor				m_executor = null;
		private	List<EXECUTION_AT>		m_priority_queue_executable = null;
		
		private struct EXECUTION_AT
		{
			public EXECUTION_AT(long _tickExecute, IExecutable _executable, ulong _param)
			{
				this.tick_execute = _tickExecute;
				this.executable = _executable;
				this.param = _param;
			}

			public long				tick_execute;
			public IExecutable		executable;
			public ulong			param;

			public void Swap(EXECUTION_AT _right)	{ var t=_right; _right=this; this=t;}
		};
	}
}
