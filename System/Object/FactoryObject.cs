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
//  CGDK.Object<TOBJECT>
//
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK.Factory
{
	public class Object<TOBJECT> : 
		IFactory where TOBJECT: new()
	{
	// constructor/destructor) 
		public Object(string _name, Func<TOBJECT> _f_object_generator = null, eFACTORY_TYPE _factory_type = eFACTORY_TYPE.USER)
		{
			// 1) Concurrent Bag을 생성한다.
			this.m_stack_object = new ConcurrentBag<TOBJECT>();

			// 2) Object Generator를 저장한다.
			if (_f_object_generator != null)
			{
				this.m_f_object_generator = _f_object_generator;
			}
			else
			{
				this.m_f_object_generator = ()=>new TOBJECT();
			}

			// 3) 이름을 설정한다.
			this.Name = _name;

			// 4) Pool Type을 설정힌다.
			this.m_statistics_factory.factory_type = _factory_type;

			// Trace) 
			LOG.INFO_LOW(null, "(info) Factory("+ _name + ") is created " + System.Reflection.MethodBase.GetCurrentMethod().Name + ")");
		}
		~Object()
		{
			// Trace) 
			LOG.INFO_LOW(null, "(info) Factory(" + this.Name + ") is destroyed " + System.Reflection.MethodBase.GetCurrentMethod().Name + ")");
		}

	// public) 
		public TOBJECT					Alloc()
		{
			// Declare) 
			TOBJECT	temp_object;

			// 1) 먼저 Concurent bag에 저장된 객체가 없으면 새로 만들어서 리턴한다.
			if(m_stack_object.TryTake(out temp_object))
			{
				// Statistics) 재사용해서 할당한 수
				Interlocked.Increment(ref this.m_statistics_factory.alloc_stacked);

				// check) 
				Debug.Assert(temp_object != null);
			}
			else
			{
				// - 새로 할당받는다.
				temp_object = ProcessCreateObject();

				// Statistics) 
				Interlocked.Increment(ref this.m_statistics_factory.alloc_create);

				// Statistics) 생성해서 할당한 수
				Interlocked.Increment(ref this.m_statistics_factory.existing);

				// check) 
				Debug.Assert(temp_object != null);
			}

			// Statistics) 사용량을 증가시킨다.
			Interlocked.Increment(ref this.m_statistics_factory.in_using);

			// Return) 객체를 Return~
			return temp_object;
		}
		public void						Free(TOBJECT _object)
		{
			// Statistics) 사용 중인 객체의 수를 줄인다.
			Interlocked.Decrement(ref this.m_statistics_factory.in_using);

			// Statistics) 스택해서 할당해제한 수
			Interlocked.Increment(ref this.m_statistics_factory.free_stacked);

			// 1) 사용이 끝나 회수된 객체를 저장한다.
			this.m_stack_object.Add(_object);
		}

	// framework)
		protected virtual TOBJECT		ProcessCreateObject()
		{
			return  this.m_f_object_generator();
		}

	// implementations)
		private ConcurrentBag<TOBJECT>	m_stack_object;
		protected Func<TOBJECT>			m_f_object_generator;
	}
}