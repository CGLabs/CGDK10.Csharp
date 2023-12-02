//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 1.0 / Release 2015.01.05                    *
//*                                                                           *
//*                               Pool Classes                                *
//*                                                                           *
//*                                                                           *
//*                                                                           *
//*                                                                           *
//*  This Program is programmed by Cho SangHyun. sangducks@cgcii.co.kr        *
//*  Best for Game Developement and Optimized for Game Developement.          *
//*                                                                           *
//*                   (C) 2008 CGLabs All right reserved.                     *
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
	// Publics) 
		public bool						TryTake(out TOBJECT _Item)
		{
			lock (m_stackObject)
			{
				if (m_stackObject.Count == 0)
				{
					_Item = default(TOBJECT);
					return false;
				}

				_Item = m_stackObject.Pop();
			}

			return true;
		}
		public TOBJECT[]				TryTakeAll()
		{
			TOBJECT[] temp;

			lock (m_stackObject)
			{
				// 1) 모든 데이터들을 Array로 만든다.
				temp = m_stackObject.ToArray();

				// 2) 모든 데이터들 지우기
				m_stackObject.Clear();
			}

			// Return) 
			return temp;
		}
		public void						Add(TOBJECT _Item)
		{
			lock (m_stackObject)
			{
				m_stackObject.Push(_Item);
			}
		}

	// Implementations) 
		private Stack<TOBJECT>			m_stackObject	 = new Stack<TOBJECT>();
	}

	public class ConcurrentQueue<TOBJECT>
	{
	// Publics) 
		public bool						TryDequeue(out TOBJECT _Item)
		{
			lock (m_queueObject)
			{
				if (m_queueObject.Count == 0)
				{
					_Item = default(TOBJECT);
					return false;
				}

				_Item = m_queueObject.Dequeue();
			}

			return true;
		}
		public TOBJECT[]				TryDequeueAll()
		{
			TOBJECT[] temp;

			lock (m_queueObject)
			{
				// 1) 모든 데이터들을 Array로 만든다.
				temp = m_queueObject.ToArray();

				// 2) 모든 데이터들 지우기
				m_queueObject.Clear();
			}

			// Return) 
			return temp;
		}
		public void						Enqueue(TOBJECT _Item)
		{
			lock (m_queueObject)
			{
				m_queueObject.Enqueue(_Item);
			}
		}

	// Implementations) 
		private Queue<TOBJECT>			m_queueObject	 = new Queue<TOBJECT>();
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
		public bool						Dequeue(out TOBJECT _Out)
		{
			lock (m_queue)
			{
				// - Queuing된 것이 하나도 없으면 기다린다.
				while(m_queue.Count == 0)
				{
					Monitor.Wait(m_queue);
				}

				// - 값을 Deque한다.
				_Out = m_queue.Dequeue();

				// Return) 
				return	true;
			}
		}
		public void						Clear()
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
