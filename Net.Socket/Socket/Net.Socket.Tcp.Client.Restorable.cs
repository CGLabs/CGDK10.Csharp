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

using System.Net;
using System.Net.Sockets;

//----------------------------------------------------------------------------
//
//  class CGDK.Net.Socket.TcpClientRestorable
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK.Net.Socket
{
	public class TcpClientRestorable : TcpClient
	{
	// constructors) 
		public TcpClientRestorable()
		{
		}
		public TcpClientRestorable(string _name) : base(_name)
		{
		}
	}
}