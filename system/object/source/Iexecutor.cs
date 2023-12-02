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
using System.Threading;

//----------------------------------------------------------------------------
//
//  <<interface>> CGDK.Iexecutor
//
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public interface Iexecutor : Istartable
	{
	// public) 
		bool			post(Iexecutable _pexecutable, ulong _para = 0);
		bool			process_post_execute(Iexecutable _pexecutable, ulong _para = 0);
	}
}
