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
using System.Net.Sockets;
using CGDK;
using CGDK.Net.Io.Statistics;

//----------------------------------------------------------------------------
//
//  class CGDK.Net.Io.IConnectable
//
//
//----------------------------------------------------------------------------
namespace CGDK.Net.Io
{
	public interface IConnectable : IReferenceable
	{
	// publics) 
		bool			CloseSocket(ulong _disconnect_reason = DISCONNECT_REASON.NONE);
		bool			ProcessCompleteConnect(SocketAsyncEventArgs _args);
		void			ProcessCompleteDisconnect();

		Io.NSocket		Socket			{ get; }
										  
		IConnective		Connective		{ get; set;}
		Ntraffic		Statistics		{ get; }
	}
}
