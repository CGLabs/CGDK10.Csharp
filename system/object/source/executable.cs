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
using System.Collections.Generic;
using System.Linq;
using System.Text;

//----------------------------------------------------------------------------
//
//  CGDK.executable
//
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public abstract class executable : Iexecutable
	{
	// frameworks) 
		public abstract void			on_execute();

	// implementations) 
		public long	    				process_execute(ulong _return, ulong _param)
		{
			// 1) OnExecute함수를 호출한다.
			on_execute();

			// Return)
			return	0;
		}
	}
}