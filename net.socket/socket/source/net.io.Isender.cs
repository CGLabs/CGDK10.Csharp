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

using System.Net;

//----------------------------------------------------------------------------
//
//  class CGDK.net.io.Isender
//
//
//----------------------------------------------------------------------------
namespace CGDK.net.io
{
	public interface Isender_stream
	{
		bool	send(CGDK.buffer _buffer);
	}

	public interface Isender_datagram
	{
		bool	send_to(CGDK.buffer _buffer, IPEndPoint _to);
	}
}
