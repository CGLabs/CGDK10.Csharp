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
using CGDK;

//----------------------------------------------------------------------------
//
//  CGDK.Net.Io.Connective.IAcceptor
//
//
//
//----------------------------------------------------------------------------
namespace CGDK.Net.Io.Connective
{
	public interface IAcceptor : 
		Io.IConnective,
		IObjectStateable,
		IInitializable,
        IStartable,
		INameable
	{
	// publics) 
		bool		Start(int _port, int _accept_prepare_on_start = 0, int _accept_must_prepare = 0);
		bool		Start(string _address, int _port, int _accept_prepare_on_start = 0, int _accept_must_prepare = 0);
		bool		Start(string _address, string _port, int _accept_prepare_on_start = 0, int _accept_must_prepare = 0);
		bool		Start(IPEndPoint _remote_endpoint, int _accept_prepare_on_start = 0, int _accept_must_prepare = 0);

		Io.NSocket	AcceptSocket { get; }
	}
}
