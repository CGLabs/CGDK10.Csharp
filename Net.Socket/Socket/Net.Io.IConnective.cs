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

using System.Net.Sockets;
using CGDK.Net.Io.Statistics;

//----------------------------------------------------------------------------
//
//  class CGDK.Net.Io.IConnective
//
//
//----------------------------------------------------------------------------
namespace CGDK.Net.Io
{
	public interface IConnective : IReferenceable
	{
		bool				Enable  { get; set; }
		Nconnective			Statistics { get;}

		void				ProcessConnectiveConnect(object _source, SocketAsyncEventArgs _args);
		void				ProcessConnectiveDisconnect(IConnectable _pconnectable);
	}
}
