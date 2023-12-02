//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                          Network Socket Classes                           *
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

using System.Collections;

//----------------------------------------------------------------------------
//
//  <<interface>> CGDK.Iobject_stateable
//
// 
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public interface Iobject_stateable : 
		Iattachable<Iobject_stateable>,
		IEnumerable
	{
		eOBJECT_STATE		object_state { get; set;}

		bool				set_object_state_if(eOBJECT_STATE _state_compare, eOBJECT_STATE _new_states);
	}
}
