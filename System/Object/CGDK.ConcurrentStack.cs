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

using System.Collections.Generic;
using System.Threading;

//----------------------------------------------------------------------------
//  CGDK.ConcurrentBag
//
//  class ConcurrentBag
//
//
//
//
//----------------------------------------------------------------------------
#if !_SUPPORT_NET40
namespace CGDK
{
	public class ConcurrentBag<TOBJECT>
	{
	// public) 
		public bool						TryTake(out TOBJECT _item)
		{
			lock (m_stack_object)
			{
				if (m_stack_object.Count == 0)
				{
					_item = default(TOBJECT);
					return false;
				}

				_item = m_stack_object.Pop();
			}

			return true;
		}
		public TOBJECT[]				TryTakeAll()
		{
			TOBJECT[] temp;

			lock (this.m_stack_object)
			{
				// 1) 모든 데이터들을 Array로 만든다.
				temp = this.m_stack_object.ToArray();

				// 2) 모든 데이터들 지우기
				this.m_stack_object.Clear();
			}

			// Return) 
			return temp;
		}
		public void						Add(TOBJECT _item)
		{
			lock (this.m_stack_object)
			{
				this.m_stack_object.Push(_item);
			}
		}

	// Implementations) 
		private Stack<TOBJECT>			m_stack_object = new Stack<TOBJECT>();
	}

	public class ConcurrentQueue<TOBJECT>
	{
	// public) 
		public bool						TryDequeue(out TOBJECT _item)
		{
			lock (this.m_queue_object)
			{
				if (this.m_queue_object.Count == 0)
				{
					_item = default(TOBJECT);
					return false;
				}

				_item = this.m_queue_object.Dequeue();
			}

			return true;
		}
		public TOBJECT[]				TryDequeueAll()
		{
			TOBJECT[] temp;

			lock (this.m_queue_object)
			{
				// 1) 모든 데이터들을 Array로 만든다.
				temp = this.m_queue_object.ToArray();

				// 2) 모든 데이터들 지우기
				this.m_queue_object.Clear();
			}

			// Return) 
			return temp;
		}
		public void						Enqueue(TOBJECT _item)
		{
			lock (this.m_queue_object)
			{
				this.m_queue_object.Enqueue(_item);
			}
		}

	// Implementations) 
		private Queue<TOBJECT>			m_queue_object = new Queue<TOBJECT>();
	}

	public class ConcurrentBlcokedQueue<TOBJECT>
	{
	// Constructor
		public ConcurrentBlcokedQueue()
		{
		}

	// PUblics) 
		public void						Enqueue(TOBJECT item)
		{
			lock (this.m_queue)
			{
				// 1) Queuing한다.
				this.m_queue.Enqueue(item);

				// 2) 쉬고 있는 Thread가 있으면 깨운다.
				if (this.m_queue.Count == 1)
				{
					Monitor.Pulse(this.m_queue);
				}
			}
		}
		public TOBJECT Dequeue(int _time_out = Timeout.Infinite)
		{
			lock (this.m_queue)
			{
				// - Queuing된 것이 하나도 없으면 기다린다.
				var Result = Monitor.Wait(this.m_queue, _time_out);

				// check)
				if (Result == false)
				{
					return default(TOBJECT);
				}

				// return) 
				return this.m_queue.Dequeue();
			}
		}
		public void						Clear()
		{
			lock (this.m_queue)
			{
				// 1) Queue를 모두 비운다.
				this.m_queue.Clear();

				// 2) Wake up한다.
				Monitor.PulseAll(this.m_queue);
			}
		}

	// Implementations) 
		private Queue<TOBJECT>			m_queue = new Queue<TOBJECT>();
		private object					m_csWait = new object();
	}
}
#endif
