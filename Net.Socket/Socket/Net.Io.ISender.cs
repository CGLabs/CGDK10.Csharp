﻿//*****************************************************************************
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

using System.Net;

//----------------------------------------------------------------------------
//
//  class CGDK.Net.Io.Isender
//
//
//----------------------------------------------------------------------------
namespace CGDK.Net.Io
{
	public interface ISenderStream
	{
		bool	Send(CGDK.buffer _buffer);
	}

	public interface ISenderDatagram
	{
		bool	SendTo(CGDK.buffer _buffer, IPEndPoint _to);
	}
}
