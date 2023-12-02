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
//  <<interface>> CGDK.Imessage_transmitter
// 
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public interface Imessage_transmitter
	{
		int				transmit_message(object _source, sMESSAGE _msg);

		bool			register_messageable(Imessageable _pmessageable);
		bool			unregister_messageable(Imessageable _pmessageable);

	}
}
