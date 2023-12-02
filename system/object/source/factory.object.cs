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
//  CGDK.Cobject<TOBJECT>
//
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK.factory
{
	public class Cobject<TOBJECT> : 
		Ifactory where TOBJECT: new()
	{
	// constructor/destructor) 
		public Cobject(string _name, Func<TOBJECT> _f_object_generator = null, eFACTORY_TYPE _factory_type = eFACTORY_TYPE.USER)
		{
			// 1) Concurrent Bag을 생성한다.
			m_stack_object = new ConcurrentBag<TOBJECT>();

			// 2) Object Generator를 저장한다.
			if (_f_object_generator != null)
			{
				m_f_object_generator = _f_object_generator;
			}
			else
			{
				m_f_object_generator = ()=>new TOBJECT();
			}

			// 3) 이름을 설정한다.
			name = _name;

			// 4) Pool Type을 설정힌다.
			m_statistics_factory.factory_type = _factory_type;

			// trace) 
			LOG.INFO_LOW(null, "(info) factory("+_name+") is created " + System.Reflection.MethodBase.GetCurrentMethod().Name + ")");
		}
		~Cobject()
		{
			// trace) 
			LOG.INFO_LOW(null, "(info) factory(" + name + ") is destroyed " + System.Reflection.MethodBase.GetCurrentMethod().Name + ")");
		}

	// public) 
		public TOBJECT					alloc()
		{
			// Declare) 
			TOBJECT	temp_object;

			// 1) 먼저 Concurent bag에 저장된 객체가 없으면 새로 만들어서 리턴한다.
			if(m_stack_object.try_take(out temp_object))
			{
				// Statistics) 재사용해서 할당한 수
				Interlocked.Increment(ref m_statistics_factory.alloc_stacked);

				// check) 
				Debug.Assert(temp_object != null);
			}
			else
			{
				// - 새로 할당받는다.
				temp_object = process_create_object();

				// Statistics) 
				Interlocked.Increment(ref m_statistics_factory.alloc_create);

				// Statistics) 생성해서 할당한 수
				Interlocked.Increment(ref m_statistics_factory.existing);

				// check) 
				Debug.Assert(temp_object != null);
			}

			// Statistics) 사용량을 증가시킨다.
			Interlocked.Increment(ref m_statistics_factory.in_using);

			// Return) 객체를 Return~
			return temp_object;
		}
		public void						free(TOBJECT _object)
		{
			// Statistics) 사용 중인 객체의 수를 줄인다.
			Interlocked.Decrement(ref m_statistics_factory.in_using);

			// Statistics) 스택해서 할당해제한 수
			Interlocked.Increment(ref m_statistics_factory.free_stacked);

			// 1) 사용이 끝나 회수된 객체를 저장한다.
			m_stack_object.add(_object);
		}

	// framework)
		protected virtual TOBJECT		process_create_object()
		{
			// Declare) 
			TOBJECT	temp_object;

			// 1) 새로 생성한다.
			temp_object = m_f_object_generator();

			// Return) 
			return	temp_object;
		}

	// implementations)
		private ConcurrentBag<TOBJECT>	m_stack_object;
		protected Func<TOBJECT>			m_f_object_generator;
	}
}