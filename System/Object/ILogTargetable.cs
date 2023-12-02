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

using System;

//----------------------------------------------------------------------------
//
//  <<interface>> CGDK.ILogTargetable
// 
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public interface ILogTargetable
	{
		void					Trace(LOG_RECORD _log_record);
		ILogFilter				Filter { get; set; }
		ulong					LogCount { get; set; }

		void					ProcessLog(LOG_RECORD _log_record);
	}
}
