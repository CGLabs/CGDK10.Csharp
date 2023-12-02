//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                          Group Template Classes                           *
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
using System.Diagnostics;

namespace CGDK
{
namespace CGServerAccount
{

	public interface IAccessable
	{
		// 1) 
		 IAccount				ProcessAccountSet			(REQUEST_ID _ridSource, IAccount _pAccount, CHECKIN_ID _cid);
		   
		// 2)  
		eOBJECT_RESULT			ProcessAccountCheckin		(REQUEST_ID _ridSource, CGD.buffer _bufRequest, IRepository _pRepository, TRESPONSE _fCompletion);
		eOBJECT_RESULT			ProcessAccountCheckout		(REQUEST_ID _ridSource, CHECKIN_ID _cid, IAccount _pAccount, TRESPONSE _fCompletion);
		eOBJECT_RESULT			ProcessAccountTransaction	(REQUEST_ID _ridSource, CHECKIN_ID _cid, CGD.buffer _bufRequest, TREQUEST _fRequest=null, TRESPONSE _fCompletion=null);
		   
		// 3)  
		void					ProcessAccountCompleteCheckin		(REQUEST_ID _ridSource, CHECKIN_ID _cid, eOBJECT_RESULT _eResult, CGD.buffer _bufResult);
		void					ProcessAccountCompleteTransaction	(REQUEST_ID _ridSource, CHECKIN_ID _cid, eOBJECT_RESULT _eResult, CGD.buffer _bufResult);

		// 4) Update
		eOBJECT_RESULT			ProcessAccountUpdate		(CHECKIN_ID _cid, IAccount _pAccount, CGD.buffer _bufRequest);

		// 5) 강제 Checkout
		bool					ProcessAccountForceCheckout		(CHECKIN_ID _cid);

		DateTime				timeCheckin	{ get; set;}
	}
}
}
