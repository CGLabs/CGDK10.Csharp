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
//  <<interface>> CGDK.IObjectStateable
//
// 
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public interface IObjectStateable : 
		Iattachable<IObjectStateable>,
		IEnumerable
	{
		eOBJECT_STATE		Now { get; set;}

		bool				SetObjectStateIf(eOBJECT_STATE _state_compare, eOBJECT_STATE _new_states);
	}
}
