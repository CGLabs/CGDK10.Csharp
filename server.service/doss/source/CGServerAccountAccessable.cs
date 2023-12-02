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

public class CAccessable<TACCOUNT> : 
// ****************************************************************************
// Inherited classes)
// ----------------------------------------------------------------------------
	IAccessable where TACCOUNT : IAccount, new()
{
// ****************************************************************************
// Constructor/Destructor)
// ----------------------------------------------------------------------------
	public CAccessable()
	{
	}


// ****************************************************************************
// Public)
// ----------------------------------------------------------------------------
	public virtual eOBJECT_RESULT	Checkin(IRepository _pRepository, CGD.buffer _bufRequest, TRESPONSE _fCompletion=null)
	{
		//-----------------------------------------------------------------
		// 설명)
		//   Checkin을 요청하면 다음과 같은 내용을 파라메터로 전달 받는다.
		//
		//   1) _pRepository	Checkin을 할 레포지터리다.
		//   2) _bufRequest		Checkin을 하기 위한 인증 정보를 가지고 있다.
		//	 3) _fCompletion	Checkin 처리가 완료되었을 때 호출할 콜백함수다.
		//
		//   일단 Checkin을 요청하면 m_pRepository를 저장한다.
		//   또 이 m_pRepository는 Checkin 처리가 완료되면 nullptr이 된다.
		//   혹은 Checkin 처리가 완료되지 않은 상태에서 Checkout을 호출하면 
		//   이 m_pRepository에 취소를 요청한다.
		//
		//-----------------------------------------------------------------
		// Declare)
		eOBJECT_RESULT	eResult	 = eOBJECT_RESULT.EXCEPTION;

		lock(m_csAccount)
		{
			// 1) tid를 새로 생성한다.
			m_ridSource.generate(m_sid);

			// 2) Repository를 설정한다.
			m_pRepository	 = _pRepository as NRepository<TACCOUNT>;

			// Check)
			Debug.Assert(m_pRepository!=null);

			// 2) Checkin을 Request한다.
			eResult	 = ProcessAccountCheckin(m_ridSource, _bufRequest, _pRepository, _fCompletion);

			// Check)
			if(eResult > eOBJECT_RESULT.PENDING)
			{
				m_ridSource.reset();
				m_pRepository	 = null;
			}
		}

		// Return) 
		return	eResult;
	}
	public virtual eOBJECT_RESULT	Checkout(TRESPONSE _fCompletion=null)
	{
		//-----------------------------------------------------------------
		// 설명)
		//   Checkout을 요청하면 현재 진행중인 Checkin은 일단 취소된다.
		//   중복된 Checkout처리를 하지 않기 위해 한번 Checkout을 하게 되면
		//   모든 Checkin 정보는 일단 제거한다.
		//   만약 비동기적으로 진행중인 Checkin이 있다면 진행중인 완료처리
		//   될 때 Exception 처리되어 Rollback되도록 처리된다.
		//
		//   Checkin인 된 상태는 다음과 같은 경우가 있을 수 있다.
		//
		//    Case 1) Checkin이 진행 중인 경우
		//     m_pAccount는 아직 nullptr이다.
		//     이럴 경우 m_Repository가 있을 것이므로 m_pRepository에 취소를
		//     요청한다.
		//
		//    Case 2) Checkin이 완료된 경우
		//     m_pAccount가 존재하므로 m_pAccount에 Checkout을 요청한다.
		//
		//-----------------------------------------------------------------
		// Declare)
		eOBJECT_RESULT	eResult	 = eOBJECT_RESULT.EXCEPTION;

		// 1) 파라메터는 아무것이나 보낸다. (어차피 이 Accessable은 내부 변수값을 읽어서 쓸테니 말이다.)
		eResult	 = ProcessAccountCheckout(new REQUEST_ID(), new CHECKIN_ID(), null, _fCompletion);

		// Return) 
		return	eResult;
	}
	public virtual eOBJECT_RESULT	RequestTransaction(CGD.buffer _bufRequest, TREQUEST _fRequest=null, TRESPONSE _fCompletion=null)
	{
		// 1) Account 객체를 얻는다.
		var		pAccount	 = Account;

		// Check) 
		if(pAccount==null)
			return	eOBJECT_RESULT.NOT_READY;

		// Declare)
		REQUEST_ID	ridSource	 = new REQUEST_ID();

		// 2) Request ID를 생성한다.(사실상 의미 없음...)
		ridSource.generate(m_sid);

		// 3) Request CheckOut
		return	pAccount.RequestAccountTransaction(ridSource, m_cid, this, _bufRequest, _fRequest, _fCompletion);
	}

	public CHECKIN_ID				CheckinID
	{
		get	{ lock(m_csAccount)	{	return	m_cid;} }
	}
	public IAccount					Account
	{
		get
		{
			return	m_pAccount;
		}
	}

	public object					Lockable
	{
		get { return m_csAccount; }
	}


// ****************************************************************************
// Framework) 
// ----------------------------------------------------------------------------
	protected virtual	void	OnAccountRequestCheckin		(CGD.buffer _bufRequest)					{ if(NotifyOnRequestCheckin!=null) NotifyOnRequestCheckin(_bufRequest);}
	protected virtual	void	OnAccountRequestCheckout	(CHECKIN_ID _cid)							{ if(NotifyOnRequestCheckout!=null) NotifyOnRequestCheckout(_cid);}
	protected virtual	void	OnAccountRequestTransaction	(CHECKIN_ID _cid, CGD.buffer _bufRequest)	{ if(NotifyOnRequestTransaction!=null) NotifyOnRequestTransaction(_cid, _bufRequest);}

	protected virtual	void	OnAccountCheckin			(eOBJECT_RESULT _eResult, CHECKIN_ID _cid, CGD.buffer _bufRequest){ if(NotifyOnCheckin!=null) NotifyOnCheckin(_eResult, _cid, _bufRequest);}
	protected virtual	void	OnAccountCheckout			(eOBJECT_RESULT _eResult, CHECKIN_ID _cid)	{ if(NotifyOnCheckout!=null) NotifyOnCheckout(_eResult, _cid);}
	protected virtual	void	OnAccountUpdate				(eOBJECT_RESULT _eResult, CHECKIN_ID _cid, CGD.buffer _bufRequest){ if(NotifyOnUpdate!=null) NotifyOnUpdate(_eResult, _cid, _bufRequest);}
	protected virtual	void	OnAccouontTransaction		(eOBJECT_RESULT _eResult, CHECKIN_ID _cid)	{ if(NotifyOnTransaction != null) NotifyOnTransaction(_eResult, _cid);}


// ****************************************************************************
// Implementation) 
// ----------------------------------------------------------------------------
	// 1) 
	public virtual IAccount		ProcessAccountSet					(REQUEST_ID _ridSource, IAccount _pAccount, CHECKIN_ID _cid)
	{
		IAccount	pAccountOld	 = null;
		CHECKIN_ID	cidOld;

		TACCOUNT	pAccount = _pAccount as TACCOUNT;

		lock(m_csAccount)
		{
			// 설명. pAccount가 nullptr이 아니면 추가다!
			if (pAccount!=null)
			{
				// Check) 현재 진행중인 Request와 Request ID가 다를 경우 Exception을 던진다.
				if(m_ridSource!=_ridSource)
				{
					//CGLOG.INFO_LOW(null, "Info) TID is different [Source:"+_rid.tid.value.ToString()+", This:"+m_ridSource.value().ToString()+"] (CGServerAccount::NAccessable<TACCOUNT>::ProcessSetAccount)");
					throw	new Exception();
				}

				// Check) pAccount가 TACCOUNT를 상속받은 객체가 아니면 Exception을 던진다.
				if(pAccount==null)
					throw	new Exception();

				// Check) 
				Debug.Assert(m_pAccount==null);
			}

			// 1) Old를 먼저 저장해 놓는다.
			pAccountOld		 = m_pAccount;
			cidOld			 = m_cid;

			// 2) 값을 설정한다.
			m_pAccount		 = pAccount;
			m_cid			 = _cid;

			// 3) Repository는 nullptr로 설정한다.
			m_pRepository	 = null;
		}

		// Return) 
		return	pAccountOld;
	}

	// 2) 
	public virtual eOBJECT_RESULT ProcessAccountCheckin				(REQUEST_ID _ridSource, CGD.buffer _bufRequest, IRepository _pRepository, TRESPONSE _fCompletion)
	{
		// Check) _pRepository가 nullptr이면 안됀다.
		Debug.Assert(_pRepository!=null);

		// Check) _ridSource가 설정된 값이어야 한다.
		Debug.Assert(_ridSource.exist());

		// Declare)
		eOBJECT_RESULT	eResult	 = eOBJECT_RESULT.EXCEPTION;

		try
		{
			// Check) _pRepository가 nullptr이면 Exception을 던진다.
			if(_pRepository==null)
			{
				//CGLOG.INFO_LOW(null, "Info) Repository is null");	//  _CGFUNCTION
				throw	new Exception();
			}

			// 1) OnRequestCheckin를 호출한다.
			OnAccountRequestCheckin(_bufRequest);

			// 2) Request Checkin
			eResult	 = _pRepository.RequestAccountCheckin(_ridSource, _bufRequest, this, _fCompletion);
		}
		catch(Exception)
		{
		}

		// Return)
		return	eResult;
	}
	public virtual eOBJECT_RESULT ProcessAccountCheckout			(REQUEST_ID _ridSource, CHECKIN_ID _cid, IAccount _pAccount, TRESPONSE _fCompletion)
	{
		// Declare) 
		IAccount		pAccount	 = null;
		IRepository		pRepository	 = null;
		CHECKIN_ID		cid;
		REQUEST_ID		ridSource;

		// 1) pAccount, pRepository, cid, rid를 얻어온다.
		lock(m_pAccount)
		{
			pAccount	 = m_pAccount;		m_pAccount		 = null;
			pRepository	 = m_pRepository;	m_pRepository	 = null;
			cid			 = m_cid;			m_cid.reset();
			ridSource	 = m_ridSource;		m_ridSource.reset();
		}

		// Declare)
		eOBJECT_RESULT	eResult	 = eOBJECT_RESULT.EXCEPTION;

		// 2-1) Account가 없으면 Error를 리턴한다.
		if (pAccount!=null)
		{
			// Check) cid가 설정되어 있지 않으면 안됀다. (이럴 수도 있긴 하다.)
			Debug.Assert(cid.exist());

			try
			{
				// 1) OnRequestCheckout을 호출한다.
				OnAccountRequestCheckout(cid);

				// 2) Account 객체에 Checkout을 요청한다.
				eResult	 = pAccount.RequestAccountCheckout(ridSource, cid, _fCompletion);
			}
			catch (Exception)
			{
				eResult	 = eOBJECT_RESULT.EXCEPTION;
			}
		}
		// 2-2) Account가 없지만 Repository와 CID 정보가 있으면 CID를 사용해서 Checkout처리한다.
		else if (pRepository!=null && cid.exist())
		{
			// - Repository에 바로 Checkout을 요청한다.(이때 ridSource가 핵심!)
			eResult	 = pRepository.RequestAccountCheckout(ridSource, cid, null, _fCompletion);
		}
		// 2-3) else면 eOBJECT_RESULT.NOT_READY를 리턴한다.
		else
		{
			eResult	 = eOBJECT_RESULT.NOT_READY;
		}

		// 3)   
		OnAccountCheckout(eResult, cid);

		// Return) 
		return	eResult;
	}
	public virtual eOBJECT_RESULT ProcessAccountTransaction			(REQUEST_ID _ridSource, CHECKIN_ID _cid, CGD.buffer _bufRequest, TREQUEST _fRequest, TRESPONSE _fCompletion)
	{
		// 1) Account를 얻는다.
		var	pAccount	 = m_pAccount;

		// Check) CheckIn ID가 제대로되어 있는지 확인한다.
		if(pAccount==null)
			return	eOBJECT_RESULT.FAIL;

		// Declare)
		eOBJECT_RESULT	eResult	 = eOBJECT_RESULT.EXCEPTION;

		try
		{
			// 2) RequestTransaction를 호출한다.
			OnAccountRequestTransaction(_cid, _bufRequest);

			// 3) 처리...
			eResult	 = pAccount.RequestAccountTransaction(_ridSource, _cid, this, _bufRequest, _fRequest, _fCompletion);
		}
		catch(Exception)
		{
		}

		// Return)
		return	eResult;
	}

	// 3) 
	public virtual void			ProcessAccountCompleteCheckin		(REQUEST_ID _ridSource, CHECKIN_ID _cid, eOBJECT_RESULT _Result, CGD.buffer _bufResult)
	{
		lock(m_csAccount)
		{
			// Check) Cancel되었을 경우가 있으므로... 
			if(_ridSource!=m_ridSource)
				return;
		}

		// 1) 시간을 설정.
		timeCheckin	 = System.DateTime.UtcNow;

		// 2) OnCheckin함수를 호출한다.
		OnAccountCheckin(_Result, _cid, _bufResult);
	}
	public virtual void			ProcessAccountCompleteTransaction	(REQUEST_ID _ridSource, CHECKIN_ID _cid, eOBJECT_RESULT _Result, CGD.buffer _bufResult)
	{
		// 1) OnTransaction함수를 호출한다.
		try
		{
			OnAccouontTransaction(_Result, _cid);
		}
		catch(Exception)
		{
		}
	}

	public virtual eOBJECT_RESULT ProcessAccountUpdate				(CHECKIN_ID _cid, IAccount _pAccount, CGD.buffer _bufRequest)
	{
		// 1) 먼저 호출한다.
		OnAccountUpdate(eOBJECT_RESULT.SUCCESS, _cid, _bufRequest);

		// Return) 
		return	eOBJECT_RESULT.SUCCESS;
	}

	// 4) 강제 Checkout
	public virtual bool			ProcessAccountForceCheckout			(CHECKIN_ID _cid)
	{
		// Declare) 
		IAccount			pAccount	 = null;
		IRepository			pRepository	 = null;
		CHECKIN_ID			cid;
		REQUEST_ID			ridSource;

		// 1) pAccount, pRepository, cid, rid를 얻어온다.
		lock(m_pAccount)
		{
			pAccount	 = m_pAccount;		m_pAccount		 = null;
			pRepository	 = m_pRepository;	m_pRepository	 = null;
			cid			 = m_cid;			m_cid.reset();
			ridSource	 = m_ridSource;		m_ridSource.reset();
		}

		// Check)
		if(pAccount==null)
			return	false;

		// 3) Force Close한다.
		OnAccountCheckout(eOBJECT_RESULT.FORCE_CLOSE, cid);

		// Return) 
		return	true;
	}


	public	DateTime timeCheckin 
	{
		get { return m_timeCheckin;}
		set { m_timeCheckin=value;}
	}
			DateTime			m_timeCheckin;

	// 1) Account Info (계정 정보)
	protected	SOURCE_ID		m_sid;
	protected 	REQUEST_ID		m_ridSource;
	protected	CHECKIN_ID		m_cid;
	protected	object			m_csAccount	 = new object();
	protected	TACCOUNT		m_pAccount;

	protected	NRepository<TACCOUNT>	m_pRepository;

	public delegate void		delegate_AccountNotifyRequestBuffer		(CGD.buffer _bufRequest);
	public delegate void		delegate_AccountNotifyRequest			(CHECKIN_ID _cid);
	public delegate void		delegate_AccountNotifyRequestTransaction(CHECKIN_ID _cid, CGD.buffer _bufRequest);
	public delegate void		delegate_AccountNotifyResultBuffer		(eOBJECT_RESULT _Result, CHECKIN_ID _cid, CGD.buffer _bufRequest);
	public delegate void		delegate_AccountNotifyResult			(eOBJECT_RESULT _Result, CHECKIN_ID _cid);

	// Hook Functions) 
	public delegate_AccountNotifyRequestBuffer		NotifyOnRequestCheckin;
    public delegate_AccountNotifyRequest			NotifyOnRequestCheckout;
    public delegate_AccountNotifyRequestTransaction	NotifyOnRequestTransaction;
    public delegate_AccountNotifyResultBuffer		NotifyOnCheckin;
    public delegate_AccountNotifyResult				NotifyOnCheckout;
    public delegate_AccountNotifyResultBuffer		NotifyOnUpdate;
    public delegate_AccountNotifyResult				NotifyOnTransaction;
}

}
}
