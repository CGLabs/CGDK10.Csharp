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

using System.Net.Sockets;
using CGDK.net.io.statistics;

//----------------------------------------------------------------------------
//
//  class CGDK.net.io.Iconnective
//
//
//----------------------------------------------------------------------------
namespace CGDK.net.io
{
	public interface Iconnective : Ireferenceable
	{
		bool				enable		{ get; set; }
		Nconnective			statistics	{ get;}

		void				process_connective_connect(object _source, SocketAsyncEventArgs _args);
		void				process_connective_disconnect(Iconnectable _pconnectable);
	}
}
