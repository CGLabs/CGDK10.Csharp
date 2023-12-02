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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

//----------------------------------------------------------------------------
//
//  CGDK.Factory.Array<TOBJECT>
//
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK.Factory
{
	public class Array<TOBJECT> : 
		IFactory
		where TOBJECT: new()
	{
	// constructor/destructor)
		public Array(string _name, int _size, Func<TOBJECT[]> _f_object_generator = null, eFACTORY_TYPE _factory_type = eFACTORY_TYPE.USER)
		{
			// 1) Concurrent Bag을 생성한다.
			m_list_object = new ConcurrentBag<TOBJECT[]>();

			// 2) 크기를 저장한다.
			m_size = _size;

			// 3) Object Generator를 저장한다.
			if (_f_object_generator!=null)
			{
				m_f_object_generator = _f_object_generator;
			}
			else
			{
				m_f_object_generator = ()=>new TOBJECT[_size];
			}

			// 4) 이름을 설정한다.
			Name = _name;

			// 5) Pool Type을 설정힌다.
			m_statistics_factory.factory_type = _factory_type;

			// Trace) 
			LOG.INFO_LOW(null, "pool: pool(" + _name + ") is created " + System.Reflection.MethodBase.GetCurrentMethod().Name + ")");
		}
		~Array()
		{
			// Trace) 
			LOG.INFO_LOW(null, "pool: pool(" + Name + ") is destroyed " + System.Reflection.MethodBase.GetCurrentMethod().Name + ")");
		}

	// public) 
		public TOBJECT[] Alloc()
		{
			// Declare) 
			TOBJECT[] temp_object;

			// 1) 먼저 Concurent bag에 저장된 객체가 없으면 새로 만들어서 리턴한다.
			if(m_list_object.TryTake(out temp_object))
			{
				// check) 
				Debug.Assert(m_size == temp_object.Length);

				// Statistics) 재사용해서 할당한 수
				Interlocked.Increment(ref m_statistics_factory.alloc_stacked);
			}
			else
			{
				// - 새로 할당받는다.
				temp_object	 = ProcessCreateObject();

				// check) 
				Debug.Assert(m_size==temp_object.Length);

				// Statistics) 
				Interlocked.Increment(ref m_statistics_factory.alloc_create);

				// Statistics) 생성해서 할당한 수
				Interlocked.Increment(ref m_statistics_factory.existing);
			}

			// Statistics) 사용량을 증가시킨다.
			Interlocked.Increment(ref m_statistics_factory.in_using);

			// Return) 객체를 Return~
			return temp_object;
		}
		public bool Free(TOBJECT[] _object)
		{
			// check) 크기가 다른 배열이라면 안된다!
			Debug.Assert(m_size == _object.Length);

			// check) 크기가 다른 배열이라면 그냥 return한다.
			if(_object.Length!=m_size)
				return	false;

			// Statistics) 사용 중인 객체의 수를 줄인다.
			Interlocked.Decrement(ref m_statistics_factory.in_using);

			// Statistics) 스택해서 할당해제한 수
			Interlocked.Increment(ref m_statistics_factory.free_stacked);

			// 1) 사용이 끝나 회수된 객체를 저장한다.
			m_list_object.Add(_object);

			// Return) 
			return	true;
		}

	// framework) 
		protected virtual TOBJECT[]	ProcessCreateObject()
		{
			// 1) 새로 생성한다.
			var temp_object = m_f_object_generator();

			// check) 
			Debug.Assert(m_size == temp_object.Length);

			// Return) 
			return	temp_object;
		}

	// implementations) 
		private		ConcurrentBag<TOBJECT[]>	m_list_object;
		protected	Func<TOBJECT[]>				m_f_object_generator;
		private		readonly int				m_size;
	}
}