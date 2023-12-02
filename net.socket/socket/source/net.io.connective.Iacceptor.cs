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
using CGDK;

//----------------------------------------------------------------------------
//
//  CGDK.net.io.connective.Iacceptor
//
//
//
//----------------------------------------------------------------------------
namespace CGDK.net.io.connective
{
	public interface Iacceptor : 
		io.Iconnective,
		Iobject_stateable,
		Istartable,
        Iinitializable,
		Inameable
	{
	// publics) 
		bool		start(int _port, int _accept_prepare_on_start = 0, int _accept_must_prepare = 0);
		bool		start(string _address, int _port, int _accept_prepare_on_start = 0, int _accept_must_prepare = 0);
		bool		start(string _address, string _port, int _accept_prepare_on_start = 0, int _accept_must_prepare = 0);
		bool		start(IPEndPoint _remote_endpoint, int _accept_prepare_on_start = 0, int _accept_must_prepare = 0);

		io.NSocket	accept_socket { get; }
	}
}
