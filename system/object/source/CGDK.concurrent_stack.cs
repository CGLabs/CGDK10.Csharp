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
		public bool						try_take(out TOBJECT _item)
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
		public TOBJECT[]				try_take_all()
		{
			TOBJECT[] temp;

			lock (m_stack_object)
			{
				// 1) 모든 데이터들을 Array로 만든다.
				temp = m_stack_object.ToArray();

				// 2) 모든 데이터들 지우기
				m_stack_object.Clear();
			}

			// Return) 
			return temp;
		}
		public void						add(TOBJECT _item)
		{
			lock (m_stack_object)
			{
				m_stack_object.Push(_item);
			}
		}

	// Implementations) 
		private Stack<TOBJECT>			m_stack_object	 = new Stack<TOBJECT>();
	}

	public class ConcurrentQueue<TOBJECT>
	{
	// public) 
		public bool						try_dequeue(out TOBJECT _item)
		{
			lock (m_queue_object)
			{
				if (m_queue_object.Count == 0)
				{
					_item = default(TOBJECT);
					return false;
				}

				_item = m_queue_object.Dequeue();
			}

			return true;
		}
		public TOBJECT[]				try_dequeue_all()
		{
			TOBJECT[] temp;

			lock (m_queue_object)
			{
				// 1) 모든 데이터들을 Array로 만든다.
				temp = m_queue_object.ToArray();

				// 2) 모든 데이터들 지우기
				m_queue_object.Clear();
			}

			// Return) 
			return temp;
		}
		public void						enqueue(TOBJECT _item)
		{
			lock (m_queue_object)
			{
				m_queue_object.Enqueue(_item);
			}
		}

	// Implementations) 
		private Queue<TOBJECT>			m_queue_object	 = new Queue<TOBJECT>();
	}

	public class ConcurrentBlcokedQueue<TOBJECT>
	{
	// Constructor
		public ConcurrentBlcokedQueue()
		{
		}

	// PUblics) 
		public void						enqueue(TOBJECT item)
		{
			lock (m_queue)
			{
				// 1) Queuing한다.
				m_queue.Enqueue(item);

				// 2) 쉬고 있는 Thread가 있으면 깨운다.
				if (m_queue.Count == 1)
				{
					Monitor.Pulse(m_queue);
				}
			}
		}
		public TOBJECT dequeue(int _time_out = Timeout.Infinite)
		{
			lock (m_queue)
			{
				// - Queuing된 것이 하나도 없으면 기다린다.
				var result = Monitor.Wait(m_queue, _time_out);

				// check)
				if (result == false)
				{
					return default(TOBJECT);
				}

				// return) 
				return m_queue.Dequeue();
			}
		}
		public void						clear()
		{
			lock (m_queue)
			{
				// 1) Queue를 모두 비운다.
				m_queue.Clear();

				// 2) Wake up한다.
				Monitor.PulseAll(m_queue);
			}
		}

	// Implementations) 
		private Queue<TOBJECT>			m_queue          = new Queue<TOBJECT>();
		private object					m_csWait         = new object();
	}
}
#endif
