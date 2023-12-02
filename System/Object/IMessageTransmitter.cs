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
//  <<interface>> CGDK.IMessageTransmitter
// 
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public interface IMessageTransmitter
	{
		int				TransmitMessage(object _source, sMESSAGE _msg);

		bool			RegisterMessageable(IMessageable _pmessageable);
		bool			UnregisterMessageable(IMessageable _pmessageable);

	}
}
