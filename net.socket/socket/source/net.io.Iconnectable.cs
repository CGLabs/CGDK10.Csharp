//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                          Network socket Classes                           *
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
using System.Net.Sockets;
using CGDK;
using CGDK.net.io.statistics;

//----------------------------------------------------------------------------
//
//  class CGDK.net.io.Iconnectable
//
//
//----------------------------------------------------------------------------
namespace CGDK.net.io
{
	public interface Iconnectable : Ireferenceable
	{
	// publics) 
		bool			closesocket(ulong _disconnect_reason = DISCONNECT_REASON.NONE);
		bool			process_complete_connect(SocketAsyncEventArgs _args);
		void			process_complete_disconnect();

		io.NSocket		socket			{ get; }
										  
		Iconnective		connective		{ get; set;}
		Ntraffic		statistics		{ get; }
	}
}
