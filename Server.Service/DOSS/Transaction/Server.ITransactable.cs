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

public delegate CGDK.eOBJECT_RESULT TREQUEST(CGDK.ITransactable _Transactable);
public delegate CGDK.eOBJECT_RESULT TRESPONSE(CGDK.ITransactable _Transactable, CGDK.eOBJECT_RESULT _Result, CGD.buffer _bufResult);

namespace CGDK
{

public abstract class ITransactable : NCGReferenceCount
{
	public ITransactable() 										{	m_eCommand = eTRANSACTION_COMMAND.NONE; m_rid.reset(); m_fRequest=null; m_fCompletion=null; m_eResult=eOBJECT_RESULT.NOT_DEFINED; m_bufResult.clear(); m_pTransactor=null;}
	public ITransactable(eTRANSACTION_COMMAND _eCommand) 		{	m_eCommand = _eCommand; m_rid.reset(); m_fRequest=null; m_fCompletion=null; m_eResult=eOBJECT_RESULT.NOT_DEFINED; m_bufResult.clear(); m_pTransactor=null;}

	protected override void			OnFinalRelease()			{ Reset();}

	public virtual void				Reset()
	{
		lock(m_csState)
		{
			// 1) Transactor를 리셋한다.
			m_pTransactor		 = null;

			// 2) Command는 NONE으로
			m_eCommand			 = eTRANSACTION_COMMAND.NONE;

			// 3) Request 정보를 초기화한다.
			m_rid.reset();
			m_bufRequest.clear();
			m_fRequest			 = null;

			// 4) Completion 정보를 초기화한다.
			m_eResult			 = eOBJECT_RESULT.NOT_DEFINED;
			m_bufResult.clear();
			m_fCompletion		 = null;
		}
	}

	public void						ResetResult()
	{
		lock(m_csState)
		{
			m_eResult	 = eOBJECT_RESULT.NO_INITIALIZED;
			m_bufResult.clear();
		}
	}
	public void						ResetResult(eOBJECT_RESULT _Result)
	{
		lock(m_csState)
		{
			m_eResult	 = _Result;
			m_bufResult.clear();
		}
	}
	public void						ResetResult(eOBJECT_RESULT _Result, CGD.buffer _bufResult)
	{
		lock(m_csState)
		{
			m_eResult	 = _Result;
			m_bufResult	 = _bufResult;
		}
	}
	public void						SetResult(eOBJECT_RESULT _Result, CGD.buffer _bufResult)
	{
		lock(m_csState)
		{
			// Check)
			Debug.Assert(m_eResult==eOBJECT_RESULT.REQUEST);

			// Check) 이미 설정되어 있으면 Exception을 던진다.
			if(m_eResult!=eOBJECT_RESULT.REQUEST)
				throw	new Exception();

			// 1) Result를 써넣는다.
			m_eResult	 = _Result;

			// 2) Result를 써넣는다.
			m_bufResult	 = _bufResult;
		}
	}
	public void						SetResult(eOBJECT_RESULT _Result)
	{
		lock (m_csState)
		{
			// Check)
			Debug.Assert(m_eResult==eOBJECT_RESULT.REQUEST);

			// Check) 이미 설정되어 있으면 Exception을 던진다.
			if(m_eResult!=eOBJECT_RESULT.REQUEST)
				throw new Exception();

			// 1) Result를 써넣는다.
			m_eResult	 = _Result;

			// 2) 버퍼는 초기화 한다.
			m_bufResult.clear();
		}
	}

	public bool						CompleteTransaction()
	{
		// Declare) 
		Transaction.CTransactor pTransactor = null;

		lock (m_csState)
		{
			pTransactor = m_pTransactor;
		}

		// Check) pTransactor가 null이면 false를 리턴한다.
		if(pTransactor==null)
			return	false;

		// 1) Transaction을 Complete 처리한다.
		return	pTransactor.CompleteTransaction(this);
	}
	public bool						CompleteTransaction(eOBJECT_RESULT _eResult)
	{
		return	CompleteTransaction(_eResult, new CGD.buffer());
	}
	public bool						CompleteTransaction(eOBJECT_RESULT _eResult, CGD.buffer _bufResult)
	{
		// Check) Result가 REQUEST나 COMPLETE면 안됀다.
		Debug.Assert(_eResult != eOBJECT_RESULT.REQUEST);
		Debug.Assert(_eResult != eOBJECT_RESULT.COMPLETE);

		// Declare) 
		Transaction.CTransactor	pTransactor	 = null;

		lock(m_csState)
		{
			// Check) eOBJECT_RESULT.REQUEST 상태여야지만 한다.
			Debug.Assert(m_eResult == eOBJECT_RESULT.REQUEST);

			// Check) 이미 설정되어 있으면 Exception을 던진다.
			if(m_eResult != eOBJECT_RESULT.REQUEST)
				return	false;

			// 1) Transactor를 얻어 온다.
			pTransactor = m_pTransactor;

			// Check) pTransactor가 null이면 false를 리턴한다.
			if(pTransactor==null)
				return	false;

			// 2) Result를 써넣는다.
			m_eResult	 = _eResult;

			// 3) 버퍼는 초기화 한다.
			m_bufResult	 = _bufResult;
		}

		// 4) Transaction을 Complete 처리한다.
		return	pTransactor.CompleteTransaction(this);
	}

	public virtual void				_TransactionRegister()		{ ProcessTransactionRegister();}
	public virtual void				_TransactionUnregister()	{ ProcessTransactionUnregister();}

	public virtual eOBJECT_RESULT	_TransactionRequest()
	{
		// Declare) 
		eOBJECT_RESULT	eResult	 = eOBJECT_RESULT.EXCEPTION;

		lock(m_csState)
		{
			// Check) m_stateTransaction의 상태는 반드시 eOBJECT_RESULT.NOT_DEFINED이어야 한다.
			//        eOBJECT_RESULT.NOT_DEFINED상태가 아니라면 초기화가 되지 않거나 
			//        진행중인 Transactable 객체임을 의미하기 때문이다.
			if(m_eResult!=eOBJECT_RESULT.NOT_DEFINED)
			{
				CGLOG.EXCEPTION(null, "Exception) Transactable is aleady initialized  (ICGTransactable::TransactionRequest)");
				throw	new Exception();
			}

			// 1) 실행상태를 1로 변경한다.
			m_eResult			 = eOBJECT_RESULT.REQUEST;

			// 2) OnComplete 수행 
			eResult	 = ProcessTransactionRequest();

			// 3) 만약 PENDING 상황도 아닌데 여전히 REQUEST라면 
			if(eResult!=eOBJECT_RESULT.PENDING && m_eResult==eOBJECT_RESULT.REQUEST)
			{
				// Check) 기본적으로 이런 상황이 되어서는 안된다.
				//        쉽게말해서 PENDING이 아닌데 Request에서 결과값을 설정하지 않은 것이다.
				Debug.Assert(false);

				SetResult(eOBJECT_RESULT.FAIL);
			}
		}

		// Return)
		return	eResult;
	}
	public virtual bool				_TransactionComplete()
	{
		// Declare) 
		bool	bResult	 = false;

		lock(m_csState)
		{
			// Check) 현재 상태가 Complete이면 여기서 그냥 끝낸다.
			Debug.Assert(m_eResult!=eOBJECT_RESULT.COMPLETE);

			// Check) 현재 상태가 Complete이면 여기서 그냥 끝낸다.
			//        (이미 Complete가 실행된 상태를 의미하므로)
			if(m_eResult==eOBJECT_RESULT.COMPLETE)
				return	false;

			// Check) 상태가 반드시 eOBJECT_RESULT.NOT_DEFINED 이어야 한다.
			//        그것이 아니라면 이지 처리해 결과값이 반영된 Transaction임을 의미한다.
			Debug.Assert(m_eResult!=eOBJECT_RESULT.NOT_DEFINED && m_eResult!=eOBJECT_RESULT.REQUEST);

			// Check) 상태가 반드시 eOBJECT_RESULT.NOT_DEFINED 이어야 한다.
			if(m_eResult==eOBJECT_RESULT.NOT_DEFINED || m_eResult==eOBJECT_RESULT.REQUEST)
				return	false;

			// 1) OnComplete 수행
			bResult		 = ProcessTransactionComplete();

			// 2) 상태를 완료로 변경한다.
			m_eResult	 = eOBJECT_RESULT.COMPLETE;
		}

		// Return)
		return	bResult;
	}
	public virtual bool				_TransactionComplete(eOBJECT_RESULT _eResult, CGD.buffer _bufResult)
	{
		// Declare) 
		bool	bResult	 = false;

		lock(m_csState)
		{
			// Check) 여전히 eOBJECT_RESULT.REQUEST 상태여야만 한다.
			//        그것이 아니라면 이미 처리한 Transaction임을 의미한다.
			Debug.Assert(m_eResult==eOBJECT_RESULT.REQUEST);

			// Check) 상태가 반드시 eOBJECT_RESULT.REQUEST 이어야 한다.
			//        (그것이 아니라면 누가 먼저 결과 값을 설정했다는 의미다.)
			if(m_eResult!=eOBJECT_RESULT.REQUEST)
				return	false;

			// 1) 결과값을 설정한다.
			m_eResult	 = _eResult;
			m_bufResult	 = _bufResult;

			// 2) OnComplete 수행
			bResult		 = ProcessTransactionComplete();

			// 3) 상태를 완료로 변경한다.
			m_eResult	 = eOBJECT_RESULT.COMPLETE;
		}

		// Return)
		return	bResult;
	}
	public virtual bool				_TransactionCancel()
	{
		// Declare) 
		bool	bResult	 = false;

		lock(m_csState)
		{
			// Check) 현재 결과 상태가 REQUEST 상태인가 확인한다.
			//        만약 REQUEST가 아니면 Request 자체가 아에 요청되지 않았거나
			//        이미 처리가 끝난 경우를 의미한다.
			if(m_eResult!=eOBJECT_RESULT.REQUEST)
				return	false;

			// 1) Cancel을 처리한다.
			bResult	 = _TransactionCancel();
		}

		// Return)
		return	bResult;
	}

	public abstract void			ProcessTransactionRegister();
	public abstract void			ProcessTransactionUnregister();
	public abstract eOBJECT_RESULT	ProcessTransactionRequest();
	public abstract bool			ProcessTransactionComplete();

	// 1) Transactor
	public	Transaction.CTransactor	m_pTransactor;

	// 2) Command & State
	public	eTRANSACTION_COMMAND	m_eCommand;
	protected	object				m_csState		 = new object();

	// 3) Request info
	public	REQUEST_ID				m_rid;
	public	CGD.buffer				m_bufRequest;
	public	TREQUEST				m_fRequest;

	// 4) Result info
	public	eOBJECT_RESULT			m_eResult;
	public	CGD.buffer				m_bufResult;
	public	TRESPONSE				m_fCompletion;
}

}
