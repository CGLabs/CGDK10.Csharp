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
using CGDK;

//----------------------------------------------------------------------------
//
//  CGDK.Factory.auto<TOBJECT>
//
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK.Factory
{
	public class Auto<TOBJECT> : 
		Object<TOBJECT> where TOBJECT:Nreferenceable, new()
	{
	// Constructor) 
		public Auto(string _name, Func<TOBJECT> _f_object_generator = null, eFACTORY_TYPE _type = eFACTORY_TYPE.USER) : base(_name, _f_object_generator, _type)
		{
		}

	// framework) 
		protected override TOBJECT		ProcessCreateObject()
		{
			// 1) 새로 생성한다.
			var temp_object = m_f_object_generator();

			// 2) Free함수를 처리한다.
			temp_object.ProcessFree = new Nreferenceable.f_free(this.Withdraw);

			// Return) 
			return temp_object;
		}

	// implementations) 
		private void					Withdraw(IReferenceable _object)
		{
			// check) reference Count는 반드시 0이어여 한디ㅏ.
			Debug.Assert(_object.ReferenceCount == 0);

			// 1) 사용이 끝나 회수된 객체를 저장한다.
			Free(_object as TOBJECT);
		}
	}
}