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
using System.Net.Sockets;

//----------------------------------------------------------------------------
//
//  class CGDK.net.socket.tcp_client_restorable
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK.net.socket
{
	public class tcp_client_restorable : tcp_client
	{
	// constructors) 
		public tcp_client_restorable()
		{
		}
		public tcp_client_restorable(string _name) : base(_name)
		{
		}
	}
}