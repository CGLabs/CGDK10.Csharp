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

public class ITransactableAccount<TACCOUNT> : 
		ITransactable
		where TACCOUNT : CGServerAccount.IAccount, new()
	{
	public ITransactableAccount() 									{}
	public ITransactableAccount(eTRANSACTION_COMMAND _eCommand) : base(_eCommand) 	{}

	public override void			Reset()
	{
		lock(m_csState)
		{
			m_ridSource.reset();
			m_aid.reset();
			m_cid.reset();
			m_pAccount		 = null;
			m_pAccessable	 = null;
			m_pRepository	 = null;

			base.Reset();
		}
	}
	public eOBJECT_RESULT			RequestAccountUpdate(CGD.buffer _bufRequest)
	{
		// 1) Account를 얻는다.
		var	pAccount	 = m_pAccount;

		// Check) m_pAccount가 nullptr이면 안됀다.
		if(pAccount==null)
			return	eOBJECT_RESULT.NOT_EXIST;

		// Check) 처리 중(eOBJECT_RESULT.REQUEST)일 때만 가능하다.
		Debug.Assert(m_eResult==eOBJECT_RESULT.REQUEST);

		// 2) Update를 요청한다.(ProcessAccountRequestUpdate는 여기서만 호출가능하다.)
		return	pAccount.ProcessAccountRequestUpdate(_bufRequest);
	}

	public override void			ProcessTransactionRegister()	{ Debug.Assert(m_pRepository != null); ++(m_pRepository.m_statistics.m_statisticsCheckin_Request); }
	public override void			ProcessTransactionUnregister()	{ Debug.Assert(m_pRepository != null); ++(m_pRepository.m_statistics.m_statisticsTransactable_Complete); }

	public override eOBJECT_RESULT	ProcessTransactionRequest()		{ Debug.Assert(m_pRepository != null); return m_pRepository.Transactable_Request(this); }
	public override bool			ProcessTransactionComplete()	{ Debug.Assert(m_pRepository != null); return m_pRepository.Transactable_Complete(this); }

	protected override void			OnFinalRelease()				{ Reset();}

	public	REQUEST_ID				m_ridSource;
	public	ACCOUNT_ID				m_aid;
	public	CHECKIN_ID				m_cid;
	public	TACCOUNT				m_pAccount;
	public CGServerAccount.IAccessable m_pAccessable;

	public CGServerAccount.NRepository<TACCOUNT> m_pRepository;
}

}
