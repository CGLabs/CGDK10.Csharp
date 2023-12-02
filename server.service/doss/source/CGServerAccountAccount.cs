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
using System.Collections.Generic;

namespace CGDK
{
namespace CGServerAccount
{

public class CAccount : IAccount
{
	public CAccount()
	{
	}
	~CAccount()
	{
	}
	
	public override eOBJECT_RESULT	RequestAccountCheckout(REQUEST_ID _tidRequest, CHECKIN_ID _cid, TRESPONSE _Function)
	{
		// 1) Repository를 얻는다.
		var	pRepository	 = Repository;

		// Check) pRepository가 없으면 eOBJECT_RESULT.NOT_READY에러를 되돌리며 끝낸다.
		if(pRepository==null)
			return	eOBJECT_RESULT.NOT_READY;

		// 2) Request한다.
		return	pRepository.RequestAccountCheckout(_tidRequest, _cid, this, _Function);
	}
	public override eOBJECT_RESULT	RequestAccountTransaction(REQUEST_ID _tidRequest, CHECKIN_ID _cid, IAccessable _pAccessable, CGD.buffer _bufRequest, TREQUEST _fRequest=null, TRESPONSE _fCompletion=null)
	{
		// 1) Repository를 얻는다.
		var	pRepository	 = Repository;

		// Check) pRepository가 없으면 eOBJECT_RESULT.NOT_READY에러를 되돌리며 끝낸다.
		if(pRepository==null)
			return	eOBJECT_RESULT.NOT_READY;

		// 2) Request한다.
		return	pRepository.RequestAccountTransaction(_tidRequest, _cid, _pAccessable, _bufRequest, _fRequest, _fCompletion);
	}

	public override eOBJECT_RESULT	RequestAccountSerializeIn(CGD.buffer _Buffer)
	{
		return	ProcessAccountSerializeIn(_Buffer);
	}
	public override eOBJECT_RESULT	RequestAccountSerializeOut(CGD.buffer _Buffer)
	{
		return	ProcessAccountSerializeOut(_Buffer);
	}


	public override void			ProcessAccountAlloc()
	{
		// Check) 
		Debug.Assert(m_vectorCID.Count==0);

		// Check) 
		Debug.Assert(m_countLock==0);
	}
	public override void			ProcessAccountFree()
	{
		// Check) 
		Debug.Assert(m_vectorCID.Count==0);

		// Check) 
		Debug.Assert(m_countLock==0);
	}

	public override eOBJECT_RESULT	ProcessAccountCheckin()
	{
		return	eOBJECT_RESULT.SUCCESS;
	}
	public override eOBJECT_RESULT	ProcessAccountCheckout(CHECKIN_ID _cid)
	{
		return	eOBJECT_RESULT.SUCCESS;
	}
	public override eOBJECT_RESULT	ProcessAccountTransaction(CHECKIN_ID _cid)
	{
		return	eOBJECT_RESULT.SUCCESS;
	}

	public override eOBJECT_RESULT	ProcessAccountSerializeIn(CGD.buffer _Buffer)
	{
		lock(m_csAccount)
		{
			// 1) ACCOUNT_ID
			m_aid		 = _Buffer.extract<ACCOUNT_ID>();

			// 2) Reversion
			m_iRevision	 = _Buffer.extract<UInt64>();

			// 3) ...

		}

		// Returne) 
		return	eOBJECT_RESULT.SUCCESS;
	}
	public override eOBJECT_RESULT	ProcessAccountSerializeOut(CGD.buffer _Buffer)
	{
		lock(m_csAccount)
		{
			// 1) ACCOUNT_ID
			_Buffer.append<ACCOUNT_ID>(m_aid);

			// 2) Reversion
			_Buffer.append<UInt64>(m_iRevision);

			// - ...
		}

		// Return) 
		return	eOBJECT_RESULT.SUCCESS;
	}
	public override	eOBJECT_RESULT	ProcessAccountUpdate(CGD.buffer _Buffer)
	{
		// Return) 
		return	eOBJECT_RESULT.SUCCESS;
	}
}

}
}
