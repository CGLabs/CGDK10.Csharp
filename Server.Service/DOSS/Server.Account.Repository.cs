
using System;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

namespace CGDK
{
namespace CGServerAccount
{

public abstract class NRepository<TACCOUNT> : IRepository where TACCOUNT : IAccount, new()
{
	public struct ITEM_TEMP
	{
		public CHECKIN_ID	cid;
		public TACCOUNT		pAccount;		
	}

	public NRepository()
	{
	}
	public NRepository(string _Name)
	{
		if(_Name!=null)
		{
			m_strName	 = _Name;
		}
	}

	public override string		Name	{	get	{	return m_strName;}	set	{	m_strName=value;}}

	//! @brief 계정의 '체크인'을 요청한다. @param _aid 계정 ID @return true 성공 @return false 실패
	public override	eOBJECT_RESULT		RequestAccountCheckin		(REQUEST_ID _ridSource, CGD.buffer _bufRequest, IAccessable _pAccessable, TRESPONSE _fCompletion)
	{
		//-----------------------------------------------------------------
		// 설명) 
		//    여기서 _ridSource는 말 그대로 전달받은 Source RID로 특별한
		//    역할을 하지 않고 저장하였다가 돌려준다.
		//
		//-----------------------------------------------------------------

		// Check)
		if(ObjectState<eOBJECT_STATE.RUNNING)
		{
			CGLOG.EXCEPTION(null, "Exception) Repository is not initialized ["+ Name + "] (CGServerAccount::Repository::NBase<TACCOUNT>::RequestAccountCheckin)");
			throw	new Exception();
		}

		// Check)
		Debug.Assert(_pAccessable!=null);

		// Check) _pAccessable가 null이다.
		if(_pAccessable==null)
		{
			CGLOG.EXCEPTION(null, "Exception) Invalid parameter (_pAccessiable is NULL) ["+ Name + "] (CGServerAccount::Repository::NBase<TACCOUNT>::RequestAccountCheckin)");
			throw	new Exception();
		}

		// Declare)
		eOBJECT_RESULT	eResult;

		// 1) CERTIFICATION_TYPE을 얻는다.
		eCERTIFICATION_TYPE	eCertificationType = _bufRequest.get_front<eCERTIFICATION_TYPE>();

		// 2) Certification Type에 따라 다른 Check-in을 처리한다.
		switch(eCertificationType)
		{
		// [Case] By Account ID (New check-in and Add Lock)
		case	eCERTIFICATION_TYPE.BY_ACCOUNT_ID:
				eResult = ProcessRequestCheckin_ACCOUNT_ID(_ridSource, _bufRequest, _pAccessable, _fCompletion);
				break;

			// [Case] By Check ID (Transfer or re-checkin)
		case	eCERTIFICATION_TYPE.BY_CHEKIN_ID:
				eResult = ProcessRequestCheckin_CHECKIN_ID(_ridSource, _bufRequest, _pAccessable, _fCompletion);
				break;

			// [Case] Login
		case	eCERTIFICATION_TYPE.LOGIN_BY_DEVICEID:
		case	eCERTIFICATION_TYPE.LOGIN_BY_IDPASSWORD:
				eResult = ProcessRequestCheckin_Login	(_ridSource, _bufRequest, _pAccessable, _fCompletion);
				break;

		// [Case] Create
		case	eCERTIFICATION_TYPE.CREATE_BY_DEVICEID:
		case	eCERTIFICATION_TYPE.CREATE_BY_IDPASSWORD:
				eResult = ProcessRequestCheckin_Create	(_ridSource, _bufRequest, _pAccessable, _fCompletion);
				break;

		// [Case] Others
		default:
				eResult = ProcessRequestCheckin_Custom	(_ridSource, _bufRequest, _pAccessable, _fCompletion);
				break;
		};

		// Return) 
		return	eResult;
	}
	//! @brief 계정의 '체크인'을 요청한다. @param _cid 대상 체크인 ID @return true 성공 @return false 실패
	public override	eOBJECT_RESULT		RequestAccountCheckout		(REQUEST_ID _ridSource, CHECKIN_ID _cid, IAccount _pAccount, TRESPONSE _fCompletion)
	{
		// Statistics)
		++m_statistics.m_statisticsCheckout_Request;

		// Declare)
		eOBJECT_RESULT	eResult;

		// 1-1) _pAccount가 nullptr이 아니면 이미 로그인이 완료된 상태를 말하는 것이며
		//      이럴 때는 Transaction을 건다.
		if(_pAccount!=null)
		{
			try
			{
				// 1) Request 객체 생성
				var		pTransaction		 = new ITransactableAccount<TACCOUNT>();

				// 2) Request 값 설정
				pTransaction.Reset();
				pTransaction.m_eCommand		 = eTRANSACTION_COMMAND.CHECKOUT;
				pTransaction.m_ridSource	 = _ridSource;
				pTransaction.m_pRepository	 = this;
				pTransaction.m_pAccount		 = _pAccount as TACCOUNT;
				pTransaction.m_cid			 = _cid;
				pTransaction.m_fCompletion	 = _fCompletion;

				// 3) Request를 건다.
				eResult	 = _pAccount.m_queueTransaction.RequestTransaction(pTransaction);
			}
			catch(Exception)
			{
				// Statistics) 
				++m_statistics.m_statisticsCheckout_Complete;

				// - Result를 Exception으로 설정
				eResult	 = eOBJECT_RESULT.EXCEPTION;
			}
		}
		// 1-2) _pAccount가 nullptr이면 아직 Checkin관련 Transaction이 완료된 상황이 아니거나
		//      Checkin이 된 상태가 아니란 의미로 Checkin 취소를 시도해 본다.
		else
		{
			// - Transaction을 취소한다.
			var	bResult	 = ProcessCancelCheckin_Login(_ridSource);

			// Statistics) 
			++m_statistics.m_statisticsCheckout_Complete;

			// - Result를 설정한다.
			eResult		 = (bResult) ? eOBJECT_RESULT.SUCCESS : eOBJECT_RESULT.FAIL;
		}

		// Return)
		return	eResult;
	}
	//! @brief 계정의 '커밋'을 요청한다. @param _cid 대상 체크인 ID @return true 성공 @return false 실패
	public override	eOBJECT_RESULT		RequestAccountTransaction	(REQUEST_ID _ridSource, CHECKIN_ID _cid, IAccessable _pAccessable, CGD.buffer _bufRequest, TREQUEST _fRequest=null, TRESPONSE _fCompletion=null)
	{
		// Check)_bufRequest
		Debug.Assert(_pAccessable!=null);

		// Check) _pAccessable가 null이면 안됀다.
		if(_pAccessable==null)
			throw	new Exception();

		// 1) Transaction을 생성한다.
		var		pTransaction		 = new ITransactableAccount<TACCOUNT>();

		// 2) Transaction 값을 설정한다.
		pTransaction.Reset();
		pTransaction.m_eCommand		 = eTRANSACTION_COMMAND.TRANSACTION;
		pTransaction.m_ridSource	 = _ridSource;
		pTransaction.m_rid			 = _ridSource;
		pTransaction.m_pAccessable	 = _pAccessable;
		pTransaction.m_aid			 = new ACCOUNT_ID();
		pTransaction.m_cid			 = _cid;
		pTransaction.m_bufRequest	 = _bufRequest;
		pTransaction.m_fRequest		 = _fRequest;
		pTransaction.m_fCompletion	 = _fCompletion;

		// 3) Transaction을 건다.
		return RequestAccountTransaction(_cid, pTransaction);
	}
	//! @brief 계정의 '커밋'을 요청한다. @param _cid 대상 체크인 ID @return true 성공 @return false 실패
	public override	eOBJECT_RESULT		RequestAccountTransaction	(REQUEST_ID _ridSource, ACCOUNT_ID _aid, IAccessable _pAccessable, CGD.buffer _bufRequest, TREQUEST _fRequest=null, TRESPONSE _fCompletion=null)
	{
		// 1) Transaction을 생성한다.
		var		pTransaction		 = new ITransactableAccount<TACCOUNT>();

		// 2) Transaction 값을 설정한다.
		pTransaction.Reset();
		pTransaction.m_eCommand		 = eTRANSACTION_COMMAND.TRANSACTION;
		pTransaction.m_ridSource	 = _ridSource;
		pTransaction.m_rid			 = _ridSource;
		pTransaction.m_pAccessable	 = _pAccessable;
		pTransaction.m_aid			 = _aid;
		pTransaction.m_cid			 = new CHECKIN_ID();
		pTransaction.m_bufRequest	 = _bufRequest;
		pTransaction.m_fRequest		 = _fRequest;
		pTransaction.m_fCompletion	 = _fCompletion;

		// 3) Transaction을 건다.
		return RequestAccountTransaction(_aid, pTransaction);
	}
	//! @brief 계정의 '커밋'을 요청한다. @param _cid 대상 체크인 ID @return true 성공 @return false 실패
	public override	eOBJECT_RESULT		RequestAccountUpdate		(REQUEST_ID _ridSource, ACCOUNT_ID _aid, IAccessable _pAccessable, CGD.buffer _bufRequest, TREQUEST _fRequest=null, TRESPONSE _fCompletion=null)
	{
		// 4) Transaction을 생성한다.
		var		pTransaction		 = new ITransactableAccount<TACCOUNT>();

		// 5) Transaction 값을 설정한다.
		pTransaction.Reset();
		pTransaction.m_eCommand		 = eTRANSACTION_COMMAND.TRANSACTION;
		pTransaction.m_ridSource	 = _ridSource;
		pTransaction.m_rid			 = _ridSource;
		pTransaction.m_pAccessable	 = _pAccessable;
		pTransaction.m_aid			 = _aid;
		pTransaction.m_cid			 = new CHECKIN_ID();
		pTransaction.m_bufRequest	 = _bufRequest;
		pTransaction.m_fRequest		 = _fRequest;
		pTransaction.m_fCompletion	 = _fCompletion;

		// 6) Transaction을 건다.
		return	RequestAccountTransaction(_aid, pTransaction);
	}

	public eOBJECT_RESULT				RequestAccountTransaction	(CHECKIN_ID _cid, ITransactableAccount<TACCOUNT> _pTransactable)
	{
		// Declare)
		TACCOUNT		pAccount;

		lock(m_csAccount)
		{
			// 1) Account를 찾는다.
			var	bResult	 = m_mapAccountByCID.TryGetValue(_cid, out pAccount);

			// 2-1) 존재할 경우 그 Account에 Transaction을 건다.
			if(bResult==false)
				return	eOBJECT_RESULT.NOT_EXIST;

			// Check)
			Debug.Assert(pAccount !=null);

			// 3) Checkin Lock을 건다.
			pAccount.AddCheckinCount();
		}

		// Statistics)
		++m_statistics.m_statisticsTransaction_Request;

		// Declare) 
		eOBJECT_RESULT	eResult;

		try
		{
			// Check)
			Debug.Assert(_pTransactable.m_cid==_cid);

			// 4) m_pAccount를 복사한다.
			_pTransactable.m_pRepository	 = this;
			_pTransactable.m_pAccount		 = pAccount;

			// 5) Transactable을 요청한다.
			eResult	 = pAccount.m_queueTransaction.RequestTransaction(_pTransactable);
		}
		catch(Exception)
		{
			// Statistics)
			++m_statistics.m_statisticsTransaction_Fail;

			// - Result를 eOBJECT_RESULT.EXCEPTION로 설정
			eResult	 = eOBJECT_RESULT.EXCEPTION;
		}

		// 7) 무조건 Release Checkin 
		pAccount.ReleaseCheckinCount();

		// Return) 
		return	eResult;
	}
	//! @brief 계정의 '커밋'을 요청한다. @param _cid 대상 체크인 ID @return true 성공 @return false 실패
	public eOBJECT_RESULT				RequestAccountTransaction	(ACCOUNT_ID _aid, ITransactableAccount<TACCOUNT> _pTransactable)
	{
		// Declare)
		TACCOUNT		pAccount;

		lock(m_csAccount)
		{
			// 1) Account를 찾는다.
			var	bResult	 = m_mapAccountByAID.TryGetValue(_aid, out pAccount);

			// 2-1) 존재할 경우 그 Account에 Transaction을 건다.
			if(bResult!=true)
			{
				// 2-2-1) m_mapAccountCached에서 찾는다.
				bResult	 = m_mapAccountCached.TryGetValue(_aid, out pAccount);

				// 2-2-2) m_mapAccountCached에서 찾았을 경우 그것을 사용한다.

				// 2-2-3) m_mapAccountCached에서도 찾지 못했을 경우 임시로 신규 생성한다.
				if(bResult==false)
				{
					// - 신규로 생성한다.
					pAccount	 = ProcessAllocAccountObject(_aid);

					// - Cached에 등록한다.
					m_mapAccountCached.Add(_aid, pAccount);
				}
			}

			// Check)
			Debug.Assert(pAccount != null);

			// 3) Checkin Lock을 건다.
			pAccount.AddCheckinCount();
		}

		// Statistics)
		++m_statistics.m_statisticsTransaction_Request;

		// Declare) 
		eOBJECT_RESULT	eResult;

		try
		{
			// Check) 
			Debug.Assert(_pTransactable.m_aid == _aid);

			// 4) Transactable을 생성한다.
			_pTransactable.m_pRepository	 = this;
			_pTransactable.m_pAccount		 = pAccount;

			// 5) Transactable을 요청한다.
			eResult	 = pAccount.m_queueTransaction.RequestTransaction(_pTransactable);
		}
		catch(Exception)
		{
			// Statistics)
			++m_statistics.m_statisticsTransaction_Fail;

		// - Result를 eOBJECT_RESULT.EXCEPTION로 설정
			eResult	 = eOBJECT_RESULT.EXCEPTION;
		}

		// 7) 무조건 Release Checkin 
		pAccount.ReleaseCheckinCount();

		// Return) 
		return	eResult;
	}

	//! @brief 계정 객체를 찾는다. @param _aid 계정 ID @return 찾은 계정 객체의 포인터 실패하면 null을 리턴한다.
	public virtual	TACCOUNT			FindAccount					(ACCOUNT_ID _aid)
	{
		// Declare)
		TACCOUNT	pAccount	 = null;

		lock(m_csAccount)
		{
			m_mapAccountByAID.TryGetValue(_aid, out pAccount);
		}

		// Return) 
		return	pAccount;
	}
	//! @brief 계정 객체를 찾는다. @param _cid 체크인 ID @return 찾은 계정 객체의 포인터 실패하면 null을 리턴한다.
	public virtual	TACCOUNT			FindAccount					(CHECKIN_ID _cid)
	{
		// Declare)
		TACCOUNT	pAccount	 = null;

		lock(m_csAccount)
		{
			m_mapAccountByCID.TryGetValue(_cid, out pAccount);
		}

		// Return) 
		return	pAccount;
	}

	//! @brief 모든 계정 객체를 체크아웃한다.
	public void							CheckoutAll()
	{
		// 1) 계정 정보
		List<ITEM_TEMP>	vectorAccount	 = new List<ITEM_TEMP>();

		lock(m_csAccount)
		{
			// 2) Resize한다.(C#버전에서는 없음)

			// 3) 모두 추가한다.
			foreach (var iter in m_mapAccountByCID)
			{
				ITEM_TEMP	temp	 = new ITEM_TEMP();

				temp.cid			 = iter.Key;
				temp.pAccount		 = iter.Value;

				vectorAccount.Add(temp);
			}
		}

		// 4) 모두 Checkout한다.
		foreach (var iter in vectorAccount)
		{
			RequestAccountCheckout(new REQUEST_ID(), iter.cid, iter.pAccount, null);
		}
	}

	//! @brief 객체 외부에서 AccountOjbect 객체의 할당을 원할 때
	public TACCOUNT						AllocAccountObject(ACCOUNT_ID _aid) { return ProcessAllocAccountObject(_aid); }


	//! @brief 계정의 '체크인'을 요청한다. @param _aid 계정 ID @return true 성공 @return false 실패
	public abstract	eOBJECT_RESULT		ProcessRequestCheckin_Login				(REQUEST_ID _ridSource, CGD.buffer _bufRequest, IAccessable _pAccessable, TRESPONSE _fCompletion);
	public virtual	eOBJECT_RESULT		ProcessRequestCheckin_LoginProcess		(REQUEST_ID _ridSource, ACCOUNT_ID _aid, IAccessable _pAccessable, TRESPONSE _fCompletion)
	{
		// Check) ACCOUNT_ID가 제대로 된 값이어야 한다.
		Debug.Assert(_aid.exist());

		// Declare)
		eOBJECT_RESULT	eResult	 = eOBJECT_RESULT.EXCEPTION;

		// Declare) 
		TACCOUNT	pAccount	 = null;

		lock(m_csAccount)
		{
			// 1) 로긴된 ACCOUNT 중에서 중복확인
			var	bResult	 = m_mapAccountByAID.TryGetValue(_aid, out pAccount);

			// 2-1) 이미 등록된 ACCOUNT_ID가 없다면 신규로 등록한다.
			if (bResult == false)
			{
				bResult	 = m_mapAccountCached.TryGetValue(_aid, out pAccount);

				if(bResult==true)
				{
					// - Cached에서 제거한다.
					m_mapAccountCached.Remove(_aid);
				}
				else
				{
					// - Account객체를 생성한다.
					pAccount	 = ProcessAllocAccountObject(_aid);
				}

				// Check) 할당받은 객치는 ACCOUNT 객체의 상태는 반드시 초기화가 끝난 상태여야 한다.
				Debug.Assert(pAccount.ObjectState==eOBJECT_STATE.NONE || pAccount.ObjectState==eOBJECT_STATE.STOPPED);

				// - 진행중인 계정으로 등록한다.
				m_mapAccountByAID.Add(_aid, pAccount);

				// - 처음 생성된 객체일 경우
				if (pAccount.ObjectState==eOBJECT_STATE.NONE)
				{
					pAccount.ObjectState	 = eOBJECT_STATE.INITIALIZING_PENDING;
				}

				// Check)
				Debug.Assert(pAccount.ObjectState==eOBJECT_STATE.INITIALIZING_PENDING 
							|| pAccount.ObjectState==eOBJECT_STATE.START_PENDING);

				// - Checkin Count를 일단 1 증가시킨다.
				pAccount.AddCheckinCount();

				// Statistics)
				++m_statistics.m_statisticsLogin;

				// - 상태는 성공인 것으로
				eResult	 = eOBJECT_RESULT.SUCCESS;
			}
			// 2-2) 이미 등록된 ACCOUNT_ID라면 그것을 사용한다.
			else
			{
				// - Login option
				//eLOGIN_OPTION	optionLogin	 = eLOGIN_OPTION::ALEADY_EXIST_REJECT;
				eLOGIN_OPTION	optionLogin	 = eLOGIN_OPTION.ALEADY_EXIST_DISPLACEMENT;

				switch(optionLogin)
				{
				// Case A) 대체할 경우 - 기존 접속을 로그아웃시키고 대체한다.
				case	eLOGIN_OPTION.ALEADY_EXIST_DISPLACEMENT:
						{
							// - 등록되어 있는 계정을 사용한다

							// Check) pAccount는 nullptr이면 안됀다.
							Debug.Assert(pAccount!=null);

							// - Account 객체의 상태에 따라 처리한다.
							switch (pAccount.ObjectState)
							{
							// Case) 로그인이 진행중인 경우
							//       - 기존 Checkin을 취소시킨다.
							//       - 새로운 Checkin을 진행한다.
							case	eOBJECT_STATE.INITIALIZING_PENDING:
									{
										//// - 등록되어 있는 계정을 사용한다

										//// Check) pAccount는 nullptr이면 안됀다.
										//Debug.Assert(pAccount!=null);

										//// - Checkin Count를 일단 1 증가시킨다.
										//pAccount.AddCheckinCount();

										//// - Account에 물린 모든 Checkin을 강제 Checkout시킨다.
										//try
										//{
										//	// - Login 진행
										//	eResult	 = eOBJECT_RESULT.PENDING;
										//}
										//catch(Exception)
										//{
										//	// - Checkin Count를 Release한다.
										//	pAccount.ReleaseCheckinCount();

										//	// - Login 진행
										//	eResult	 = eOBJECT_RESULT.PENDING;
										//}
									
										CGLOG.ERROR(null, "(Error) eOBJECT_STATE.INITIALIZING_PENDING!");

										eResult		 = eOBJECT_RESULT.EXCEPTION;
										pAccount	 = null;
									}
									break;

							// Case) 완전히 로그인한 상태일 경우
							//       - 기존 Checkin을 강제로 Checkout시킨다.
							//       - 새로운 Checkin을 요청해 놓는다.
							//       - 강제로 Checkout한 요청이 완료되면 새로운 Checkin을 처리한다.
							case	eOBJECT_STATE.RUNNING:
									{
										// - Checkin Count를 일단 1 증가시킨다.
										pAccount.AddCheckinCount();

										// - 기존의 Checkin들을 모두 Checkout한다.
										pAccount.RequestAccountForceCheckoutAll();

										// - Account의 상태를 INITIALIZING_PENDING 상태로 변경해 주어야 한다.
										pAccount.ObjectState	 = eOBJECT_STATE.INITIALIZING_PENDING;

										// - Checkin처리를 진행한다.
										eResult		 = eOBJECT_RESULT.SUCCESS;
										//eResult	 = eOBJECT_RESULT.FAIL;
									}
									break;

							// Default) 
							default:
									{
										CGLOG.ERROR(null, "(Error) eOBJECT_STATE.OTHER_STATE! \n");

										// Error) 이런 상태가 있어서는 안됀다.
										Debug.Assert(false);
										eResult		 = eOBJECT_RESULT.EXCEPTION;
										pAccount	 = null;
									}
									break;
							}
						}
						break;

				// Case B) 금지일 경우 - 기존 접속이 있으면 새로운 접속은 금지한다.
				case	eLOGIN_OPTION.ALEADY_EXIST_REJECT:
						{
							pAccount	 = null;
							eResult		 = eOBJECT_RESULT.ALEADY_EXIST;
						}
						break;

				// Case C) 체크인할 경우 - 기존 접속이 있으면 그냥 체크인을 진행한다.
				case	eLOGIN_OPTION.ALEADY_EXIST_CHECKIN:
						{
							// - 등록되어 있는 계정을 사용한다

							// Check) pAccount는 nullptr이면 안됀다.
							Debug.Assert(pAccount!=null);

							// - 진행중인 계정으로 등록한다.
							m_mapAccountByAID.Add(_aid, pAccount);

							// - Checkin Count를 일단 1 증가시킨다.
							pAccount.AddCheckinCount();

							// - 상태는 성공인 것으로
							eResult	 = eOBJECT_RESULT.SUCCESS;

							// Error) 아직 정의되지 않았다.
							Debug.Assert(false);
							eResult	 = eOBJECT_RESULT.FAIL;
						}
						break;

				// Default) 
				default:
						{
							// Error) 이런 상태가 있어서는 안됀다.
							Debug.Assert(false);
							eResult	 = eOBJECT_RESULT.EXCEPTION;
						}
						break;
				}
			}
		}

		// 3) 만약 객체의 상태가 eOBJECT_STATE::INITIALIZING_PENDING 상태라면 로그인을 진행한다.
		if(eResult==eOBJECT_RESULT.SUCCESS)
		{
			try
			{
				// 1) Transactable을 생성한다.
				var	pTransactable	 = new ITransactableAccount<TACCOUNT>();

				// 2) Transaction값을 설정
				pTransactable.Reset();
				pTransactable.m_eCommand	 = eTRANSACTION_COMMAND.CHECKIN_LOGIN;
				pTransactable.m_ridSource	 = _ridSource;
				pTransactable.m_rid			 = _ridSource;		// 이것이 내부적으로 Key로 사용됨.
				pTransactable.m_aid			 = _aid;			// ACCOUNT_ID가 핵심!
				pTransactable.m_pAccessable	 = _pAccessable;
				pTransactable.m_pRepository	 = this;
				pTransactable.m_pAccount	 = pAccount;
				pTransactable.m_fCompletion	 = _fCompletion;

				// 3) Transaction을 요청한다.
				eResult	 = pAccount.m_queueTransaction.RequestTransaction(pTransactable);
			}
			catch (Exception)
			{
				eResult	 = eOBJECT_RESULT.EXCEPTION;
			}
		}
		else if(eResult>eOBJECT_RESULT.PENDING)
		{
			// Statistics)
			++m_statistics.m_statisticsLogin_Fail;

			// - _fCompletion함수를 호출한다.
			if (_fCompletion!=null)
			{
				_fCompletion(null, eResult, new CGD.buffer());
			}

			if (_pAccessable!=null)
			{
				_pAccessable.ProcessAccountCompleteCheckin(_ridSource, new CHECKIN_ID(), eResult, new CGD.buffer());
			}

			// - Checkin Count를 일단 1을 감소시킨다.
			if(pAccount!=null)
			{
				pAccount.ReleaseCheckinCount();
			}
		}
		else
		{
			Debug.Assert(true);
		}

		// Return) 
		return	eResult;
	}
	public abstract	eOBJECT_RESULT		ProcessRequestCheckin_Create			(REQUEST_ID _ridSource, CGD.buffer _bufRequest, IAccessable _pAccessable, TRESPONSE _fCompletion);
	public virtual	eOBJECT_RESULT		ProcessRequestCheckin_ACCOUNT_ID		(REQUEST_ID _ridSource, CGD.buffer _bufRequest, IAccessable _pAccessable, TRESPONSE _fCompletion)
	{
		// 1) buffer를 임시로...
		CGD.buffer	buffer = _bufRequest;

		// 2) CERTIFICATION_TYPE을 얻는다.
		var	eCertificationType = buffer.extract<eCERTIFICATION_TYPE>();

		// Check) 
		if(eCertificationType != eCERTIFICATION_TYPE.BY_ACCOUNT_ID)
		{
			CGLOG.EXCEPTION(null, "Exception) eCertificationType is invalid["+ eCertificationType + "] (CGServerAccount::Repository::NBase<TACCOUNT>::ProcessRequestCheckin_ACCOUNT_ID)");
			throw	new Exception();
		}

		// 2) aid를 얻는다.
		ACCOUNT_ID	aid	 = buffer.extract<ACCOUNT_ID>();

		// Check) AID의 유효성을 검사한다.
		if(aid.valid()==false)
		{
			CGLOG.EXCEPTION(null, "Exception) AID is invalid (CGServerAccount::Repository::NBase<TACCOUNT>::ProcessRequestCheckin_ACCOUNT_ID)");
			throw	new Exception();
		}

		// Declare)
		TACCOUNT		pAccount;
		eOBJECT_RESULT	eResult;

		lock(m_csAccount)
		{
			// 2) AID에 맞는 Account 객체가 이미 존재하는지 찾는다.
			if(m_mapAccountByAID.TryGetValue(aid, out pAccount)==false)
			{
				return	eOBJECT_RESULT.NOT_EXIST;
			}

			// 3) Checin Count를 증가시킨다. (여기서 Checkin Count를 증가시켜주지 않으면 Transaction을 거는 사이에 LogOut되어 버릴 수 있다)
			pAccount.AddCheckinCount();
		}

		// Statistics)
		++m_statistics.m_statisticsCheckin_Request;

		// * Checkin Transaction을 건다.
		try
		{
			// 4) Transaction 객체를 생성한다.
			var		pTransactable	 = new ITransactableAccount<TACCOUNT>();

			// 5) Transaction 처리를 위한 값들을 설정한다.
			pTransactable.Reset();
			pTransactable.m_eCommand	 = eTRANSACTION_COMMAND.CHECKIN_AID;
			pTransactable.m_ridSource	 = _ridSource;
			pTransactable.m_pAccessable	 = _pAccessable;
			pTransactable.m_pRepository	 = this;
			pTransactable.m_pAccount	 = pAccount;
			pTransactable.m_aid			 = aid;
			pTransactable.m_fRequest	 = null;
			pTransactable.m_fCompletion	 = _fCompletion;

			// 6) Transaction을 요청한다.
			eResult	 = pAccount.m_queueTransaction.RequestTransaction(pTransactable);
		}
		catch(Exception)
		{
			// Statistics)
			++m_statistics.m_statisticsCheckin_Fail;

		// - Result를 eOBJECT_RESULT.EXCEPTION로 설정
			eResult	 = eOBJECT_RESULT.EXCEPTION;
		}

	// 7) 무조건 Release Checkin 
		pAccount.ReleaseCheckinCount();

		// Return) 
		return	eResult;
	}
	public virtual	eOBJECT_RESULT		ProcessRequestCheckin_CHECKIN_ID		(REQUEST_ID _ridSource, CGD.buffer _bufRequest, IAccessable _pAccessable, TRESPONSE _fCompletion)
	{
		// 1) buffer를 임시로...
		CGD.buffer	buffer = _bufRequest;

		// 2) CERTIFICATION_TYPE을 얻는다.
		var	eCertificationType = buffer.extract<eCERTIFICATION_TYPE>();

		// Check) 
		if(eCertificationType != eCERTIFICATION_TYPE.BY_CHEKIN_ID)
		{
			CGLOG.EXCEPTION(null, "Exception) eCertificationType is invalid["+ eCertificationType + "] (CGServerAccount::Repository::NBase<TACCOUNT>::ProcessRequestCheckin_CHECKIN_ID)");
			throw	new Exception();
		}

		// 2) cid를 얻는다.
		CHECKIN_ID cid = buffer.extract<CHECKIN_ID>();

		// Check) CID의 유효성을 검사한다.
		if(cid.valid()==false)
		{
			CGLOG.EXCEPTION(null, "Exception) AID is invalid (CGServerAccount::Repository::NBase<TACCOUNT>::ProcessRequestCheckin_CHECKIN_ID)");
			throw	new Exception();
		}

		// Declare)
		TACCOUNT		pAccount;

		lock(m_csAccount)
		{
			// 2) 찾는다.
			if(m_mapAccountByCID.TryGetValue(cid, out pAccount)==false)
			{
				return	eOBJECT_RESULT.NOT_EXIST;
			}

			// Check)
			Debug.Assert(pAccount!=null);

			// 3) Reference Count를 증가시킨다.
			pAccount.AddCheckinCount();
		}

		// Statistics)
		++m_statistics.m_statisticsCheckin_Request;

		// Declare) 
		eOBJECT_RESULT	eResult;

		try
		{
			// 4) Transaction 객체를 생성한다.
			var pTransactable = new ITransactableAccount<TACCOUNT>();

			// 5) Transaction 처리를 위한 값들을 설정한다.
			pTransactable.Reset();
			pTransactable.m_eCommand	 = eTRANSACTION_COMMAND.CHECKIN_CID;
			pTransactable.m_ridSource	 = _ridSource;
			pTransactable.m_pAccessable	 = _pAccessable;
			pTransactable.m_pRepository	 = this;
			pTransactable.m_pAccount	 = pAccount;
			pTransactable.m_cid			 = cid;
			pTransactable.m_fRequest	 = null;
			pTransactable.m_fCompletion	 = _fCompletion;

			// 6) Transaction을 요청한다.
			eResult	 = pAccount.m_queueTransaction.RequestTransaction(pTransactable);
		}
		catch(Exception)
		{
			// Statistics)
			++m_statistics.m_statisticsCheckin_Fail;

			// - Result를 false로
			eResult	 = eOBJECT_RESULT.EXCEPTION;
		}

		// 7) 무조건 Release Checkin 
		pAccount.ReleaseCheckinCount();

		// Return) 
		return	eResult;
	}
	public virtual	eOBJECT_RESULT		ProcessRequestCheckin_Custom			(REQUEST_ID _ridSource, CGD.buffer _bufRequest, IAccessable _pAccessable, TRESPONSE _fCompletion)
	{
		// Error) 아직 정의가 되지 않았다.
		Debug.Assert(false);

		// Statistics)
		++m_statistics.m_statisticsCheckin_Request;

		// Return) 
		return	eOBJECT_RESULT.SUCCESS;
	}

	public abstract	eOBJECT_RESULT		ProcessRequestCheckin_CreateProcess		(ITransactableAccount<TACCOUNT> _pTransactable);

	public abstract	bool				ProcessCancelCheckin_Login				(REQUEST_ID _ridSource);
	public override	bool				ProcessAccountForceCheckout				(CHECKIN_ID _cid)
	{
		// Check) Running 상태가 아니면 처리하지 않는다.
		if(ObjectState<eOBJECT_STATE.RUNNING)
		{
			//CGLOG.EXCEPTION(null, "Error) Repository is not ready (NOT RUNNING, State: {0}) [{1}] " + System.Reflection.MethodBase.GetCurrentMethod().Name, ObjectState, Name);
			return	false;
		}

		// Declar)
		TACCOUNT	pAccount	 = default(TACCOUNT);

		lock(m_csAccount)
		{
			// 1) 찾는다
			if(m_mapAccountByCID.TryGetValue(_cid, out pAccount)==false)
			{
				return false;
			}

			// 2) 제거한다.
			m_mapAccountByCID.Remove(_cid);

			// Check)
			Debug.Assert(pAccount!=null);
		}

		// 4) Check Count를 Release한다.
		pAccount.ReleaseCheckinCount();

		// Statistics)
		++m_statistics.m_statisticsCheckout_Request;
		++m_statistics.m_statisticsCheckout_Complete;

		// Statistics)
		--m_statistics.m_statisticsCheckin;

		// Return) 
		return	true;
	}


	//! @brief 계정 객체를 생성한다. @return 생성한 계정 객체의 포인터
	public TACCOUNT						ProcessAllocAccountObject				(ACCOUNT_ID _aid)
	{
		// Declare) 
		TACCOUNT	pAccount	 = default(TACCOUNT);

		// 1) Caching되어 있는지 확인한다.
		lock(m_csAccount)
		{
			// - Cache되어 있는 곳에서 찾는다.
			var		bResult	 = m_mapAccountCached.TryGetValue(_aid, out pAccount);

			// - 찾았다면 그 pAccount를 쓴다.
			if(bResult==true)
			{
				// Check) pAccount가 nullptr이면 안됀다.
				Debug.Assert(pAccount!=null);

				// Check) pAccount의 Repository는 this여야지만 한다.
				Debug.Assert(pAccount.Repository==this);

				// Check) pAccount의 ACCOUNT_ID가 동일해야만 한다.
				Debug.Assert(pAccount.m_aid==_aid);

				// Check) pAccount의 상태는 반드시 eOBJECT_STATE::STOPPED여야만 한다.
				Debug.Assert(pAccount.ObjectState==eOBJECT_STATE.STOPPED);

				// Check) pAccount의 CHECKIN_COUNT는 반드시 0이어야 한다.
				Debug.Assert(pAccount.CheckinPoint==0);

				// - 제거한다.
				m_mapAccountCached.Remove(_aid);
			}
		}

		if(pAccount==null)
		{
			// 1) 객체를 생성한다.
			pAccount	 = new TACCOUNT();

			// 2) Repository를 this로 설정한다.
			pAccount.m_pRepository	 = this;

			// 3) 새로운 ACCOUNT_ID를 생성한다.(임시발급)
			pAccount.m_aid			 = _aid;

			// 4) 초기화를 한다.
			pAccount.ObjectState	 = eOBJECT_STATE.NONE;

			// 5) ProcessAlloc()함수를 호출한다.
			pAccount.ProcessAccountAlloc();
		}

		// Return)
		return	pAccount;
	}
	//! @brief 계정의 '업데이트를'을 요청한다. @param _pAccount 계정 객체 ID @return true 성공 @return false 실패
	public override bool				ProcessCompleteCheckoutAll				(IAccount _pAccount)
	{
		// Check)
		if(ObjectState<eOBJECT_STATE.RUNNING)
			return	false;

		// Declare)
		bool	bResult	 = false;

		try
		{
			// 1) Request를 생성한다.
			var	pTransactable	 = new ITransactableAccount<TACCOUNT>();

			// 2) Request를 설정
			pTransactable.Reset();
			pTransactable.m_eCommand	 = eTRANSACTION_COMMAND.CHECKOUT_ALL;
			pTransactable.m_pAccount	 = _pAccount as TACCOUNT;
			pTransactable.m_pRepository	 = this;

			// 3) Transactable을 요청한다.
			var	eResult	 = _pAccount.m_queueTransaction.RequestTransaction(pTransactable);

			// Check) Request가 성공했거나 Pending했을 경우에만 Request를 증가시킨다.
			if(eResult<=eOBJECT_RESULT.PENDING)
			{ 
				++m_statistics.m_statisticsLogout_Request;
				bResult	 = true;
			}
		}
		catch(Exception)
		{
			// Statistics)
			++m_statistics.m_statisticsLogout_Complete;
		}

		// Return) 
		return	bResult;
	}
	public virtual	eOBJECT_RESULT		ProcessAccountTransaction				(ITransactableAccount<TACCOUNT> _pTransactable)
	{
		// Declare)
		var	result	 = eOBJECT_RESULT.SUCCESS;

		// 1) Result를 써넣는다.
		_pTransactable.SetResult(result);

		// Return) 결과 리턴
		return	result;
	}

	//! @brief Transaction...
	public virtual	eOBJECT_RESULT		Transactable_Request					(ITransactableAccount<TACCOUNT> _pTransactable)
	{
		// Declare)
		eOBJECT_RESULT result = eOBJECT_RESULT.NOT_DEFINED;

		switch (_pTransactable.m_eCommand)
		{
		// CTransactableAccountCheckin_ACCOUNT_ID
		case	eTRANSACTION_COMMAND.CHECKIN_AID:
				result = Transactable_RequestCheckin_ACCOUNT_ID(_pTransactable);
				break;

		// CTransactableAccountCheckin_CHECKIN_ID
		case	eTRANSACTION_COMMAND.CHECKIN_CID:
				result = Transactable_RequestCheckin_CHECKIN_ID(_pTransactable);
				break;

		// CTransactableAccountCheckin_QueryAID
		case	eTRANSACTION_COMMAND.QUERY_AID_CHECKIN:
		case	eTRANSACTION_COMMAND.QUERY_AID_CREATE:
				result = Transactable_RequestCheckin_QueryAID(_pTransactable);
				break;

		// CTransactableAccountCheckin_ProcessLogin
		case	eTRANSACTION_COMMAND.CHECKIN_LOGIN:
				result = Transactable_RequestCheckin_Login(_pTransactable);
				break;

		// CTransactableAccountTransaction
		case	eTRANSACTION_COMMAND.TRANSACTION:
				result = Transactable_RequestTransaction(_pTransactable);
				break;

		// CTransactableAccountUpdate
		case	eTRANSACTION_COMMAND.UPDATE:
				//result = Transactable_RequestUpdate(_pTransactable);
				break;

		// CTransactableAccountCheckout
		case	eTRANSACTION_COMMAND.CHECKOUT:
				result = Transactable_RequestCheckout(_pTransactable);
				break;

		// CTransactableAccountAllCheckedout
		case	eTRANSACTION_COMMAND.CHECKOUT_ALL:
				result = Transactable_RequestLogout(_pTransactable);
				break;

		// CTransactableAccountNewAccount
		case	eTRANSACTION_COMMAND.NEW_ACCOUNT:
				result = Transactable_RequestCheckin_QueryAID(_pTransactable);
				break;

		default:
				Debug.Assert(false);
				break;
		}

		return result;
	}
	public virtual	bool				Transactable_Complete					(ITransactableAccount<TACCOUNT> _pTransactable)
	{
		// Declare)
		bool result = false;

		switch (_pTransactable.m_eCommand)
		{
		// CTransactableAccountCheckin_ACCOUNT_ID
		case	eTRANSACTION_COMMAND.CHECKIN_AID:
				result = Transactable_CompleteCheckin_ACCOUNT_ID(_pTransactable);
				break;

		// CTransactableAccountCheckin_CHECKIN_ID
		case	eTRANSACTION_COMMAND.CHECKIN_CID:
				result = Transactable_CompleteCheckin_CHECKIN_ID(_pTransactable);
				break;

		// CTransactableAccountCheckin_QueryAID
		case	eTRANSACTION_COMMAND.QUERY_AID_CHECKIN:
		case	eTRANSACTION_COMMAND.QUERY_AID_CREATE:
				result = Transactable_CompleteCheckin_QueryAID(_pTransactable);
				break;

		// CTransactableAccountCheckin_ProcessLogin
		case	eTRANSACTION_COMMAND.CHECKIN_LOGIN:
				result = Transactable_CompleteCheckin_Login(_pTransactable);
				break;

		// CTransactableAccountTransaction
		case	eTRANSACTION_COMMAND.TRANSACTION:
				result = Transactable_CompleteTransaction(_pTransactable);
				break;

		// CTransactableAccountUpdate
		case	eTRANSACTION_COMMAND.UPDATE:
				//result = Transactable_CompleteUpdate(_pTransactable);
				break;

		// CTransactableAccountCheckout
		case	eTRANSACTION_COMMAND.CHECKOUT:
				result = Transactable_CompleteCheckout(_pTransactable);
				break;

		// CTransactableAccountAllCheckedout
		case	eTRANSACTION_COMMAND.CHECKOUT_ALL:
				result = Transactable_CompleteLogout(_pTransactable);
				break;

		// CTransactableAccountNewAccount
		case	eTRANSACTION_COMMAND.NEW_ACCOUNT:
				result = Transactable_CompleteCheckin_QueryAID(_pTransactable);
				break;

		// Default) 
		default:
				Debug.Assert(false);
				break;
		}

		// Return) 
		return result;
	}

	//! @brief 계정의 [체크인]을 요청한다.
	public virtual eOBJECT_RESULT		Transactable_RequestCheckin_QueryAID	(ITransactableAccount<TACCOUNT> _pTransactable)
	{
		// 임ㅅ) 
		// 1) ACCOUNT_ID를 Generation한다.
		ACCOUNT_ID	aid	 = new ACCOUNT_ID();
		aid.keyAccount	 = GenerateUID();

		// 2) Transactable에 ACCOUNT_ID를 설정한다.
		_pTransactable.m_aid	 = aid;

		// Declare) 
		var	result		 = eOBJECT_RESULT.SUCCESS;

		// 3) 결과를 설정한다.
		_pTransactable.SetResult(result);

		// Return)
		return	result;
	}
	public virtual bool					Transactable_CompleteCheckin_QueryAID	(ITransactableAccount<TACCOUNT> _pTransactable)
	{
		// Check)
		Debug.Assert(_pTransactable.m_eResult!=eOBJECT_RESULT.REQUEST && _pTransactable.m_eResult!=eOBJECT_RESULT.NOT_DEFINED);

		// Declare)
		var	eResult		 = _pTransactable.m_eResult;
		var	ridSource	 = _pTransactable.m_ridSource;
		var	aid			 = _pTransactable.m_aid;
		var	pAccessable	 = _pTransactable.m_pAccessable;
		var	fFunction	 = _pTransactable.m_fCompletion;
		var	bufResult	 = _pTransactable.m_bufResult;

		if(_pTransactable.m_eResult==eOBJECT_RESULT.SUCCESS)
		{
			// 1) 
			switch(_pTransactable.m_eCommand)
			{
			case	eTRANSACTION_COMMAND.CHECKIN_LOGIN:
					{
						eResult	 = ProcessRequestCheckin_LoginProcess(ridSource, aid, pAccessable, fFunction);
					}
					break;

			case	eTRANSACTION_COMMAND.NEW_ACCOUNT:
					{
						eResult	 = ProcessRequestCheckin_CreateProcess(_pTransactable);
					}
					break;

			default:
					Debug.Assert(false);
					return	false;
			}
		}
		else
		{
			// Statistics)
			++m_statistics.m_statisticsLogin_Fail;

			// - fFunction함수를 호출한다.
			if (fFunction!=null)
			{
				fFunction(null, eResult, bufResult);
			}

			if (pAccessable!=null)
			{
				pAccessable.ProcessAccountCompleteCheckin(ridSource, new CHECKIN_ID(), eResult, bufResult);
			}
		}

		// Return)
		return	true;
	}
	public virtual eOBJECT_RESULT		Transactable_RequestCheckin_Login		(ITransactableAccount<TACCOUNT> _pTransactable)
	{
		// 1) pAccount 객체를 얻어 놓는다.
		TACCOUNT	pAccount	 = _pTransactable.m_pAccount as TACCOUNT;

		// Check) pAccount가 nullptr이어서는 안됀다.
		Debug.Assert(pAccount!=null);

		// Check) pAccount의 상태는 반드시 eOBJECT_STATE::INITIALIZING_PENDING이여야만 한다.
		Debug.Assert(pAccount.ObjectState==eOBJECT_STATE.INITIALIZING_PENDING);

		// 2) Result를 설정한다.
		var	result	 = eOBJECT_RESULT.SUCCESS;

		// 3) Result 설정
		_pTransactable.SetResult(result);
	
		// Return) 
		return	result;
	}
	public virtual bool					Transactable_CompleteCheckin_Login		(ITransactableAccount<TACCOUNT> _pTransactable)
	{
		// Check)
		Debug.Assert(_pTransactable != null);

		// Check)
		Debug.Assert(_pTransactable.m_eResult!=eOBJECT_RESULT.REQUEST && _pTransactable.m_eResult!=eOBJECT_RESULT.NOT_DEFINED);

		// Declare)
		bool	bResult		 = false;

		// 1) Result를 임시 복사
		var		eResult		 = _pTransactable.m_eResult;

		// 2) _fCompletion함수를 호출한다.
		if (_pTransactable.m_fCompletion!=null)
		{
			eResult	 = _pTransactable.m_fCompletion(_pTransactable, eResult, _pTransactable.m_bufResult);
		}

		// 3) pAccount를 임시로 저장해 놓는다.
		TACCOUNT	pAccount	 = _pTransactable.m_pAccount as TACCOUNT;

		if (eResult==eOBJECT_RESULT.SUCCESS)
		{
			// 4) 임시로 복사
			IAccessable pAccessable = _pTransactable.m_pAccessable;

			// Declare) 
			CHECKIN_ID	cidNew;
			IAccount	pAccountOld	 = null;

			// Check) pAccount가 nullptr이면 안됀다.
			Debug.Assert(pAccount!=null);

			// 5) Account와 aid를 임시로 저장해 놓는다.
			ACCOUNT_ID	aid			 = pAccount.AccountID;

			try
			{
				lock(m_csAccount)
				{
					try
					{
						// 6) CHECKIN_COUNT를 증가시킨다.(Checkin에 해당)
						pAccount.AddCheckinCount();

						// 7) Checkin_id를 새로 발급받는다.
						cidNew	 = GenerateNewCID(aid);

						// Check) CID가 제대로 되었는 지 확인한다.
						Debug.Assert(cidNew.valid()==true);

						// Statistics) 
						++m_statistics.m_statisticsCheckin_Request;

						// 8) 새로 할당한 추가한다.
						m_mapAccountByCID.Add(cidNew, pAccount);

						// Check) 추가하지 못했다면 Exception을 던진다. (Add에서 내부적으로 던진다.)
					}
					catch(Exception)
					{
						// - Accessable의 ProcessCompleteCheckin을 호출한다.
						try
						{
							if (_pTransactable.m_pAccessable != null)
							{
								_pTransactable.m_pAccessable.ProcessAccountCompleteCheckin(_pTransactable.m_ridSource, _pTransactable.m_cid, eOBJECT_RESULT.EXCEPTION, _pTransactable.m_bufResult);
							}
						}
						catch(Exception)
						{
							throw	new Exception();
						}

						// Reraise)
						throw;
					}

					try
					{
						// 9-Pre)
						var	item	 = new ACCOUNT_ITEM();
						item.cid		 = cidNew;
						item.pAccessable = pAccessable;

						// 9) CID를 Account에 등록한다.
						pAccountOld	 = pAccount.ProcessAddCID(_pTransactable.m_ridSource, item);

						try
						{
							// 10) pAccount의 상태를 eOBJECT_STATE::RUNNING로 한다.
							pAccount.ObjectState	 = eOBJECT_STATE.RUNNING;

							// 11) Process Complete Checkin을 수행한다.
							pAccessable.ProcessAccountCompleteCheckin(_pTransactable.m_ridSource, cidNew, eResult, _pTransactable.m_bufResult);

							// Test)
							//throw	CGException::CThrow_Failure();
							//return	eOBJECT_RESULT.NOT_EXIST;
						}
						catch(Exception)
						{
							// - CID를 Remove한다.
							pAccount.ProcessRemoveCID(cidNew);

							//// 10) pAccount의 상태를 eOBJECT_STATE::RUNNING로 한다.
							//pAccount->SetObjectState(eOBJECT_STATE::INITIALIZING_PENDING);

							// Reraise) 
							throw	new Exception();
						}
					}
					catch(Exception)
					{
							// - Account에 등록실패했으면 Checkin을 취소한다.
						m_mapAccountByCID.Remove(cidNew);

						// Reraise) 
						throw;
					}
				}

				// Statistics)
				++m_statistics.m_statisticsCheckin;

				// Statistics)
				++m_statistics.m_statisticsCheckin_Success;
				++m_statistics.m_statisticsLogin_Success;
			}
			//catch(ExceptionCritical)
			//{
			//	// Statistics)
			//	++m_statistics.m_statisticsCheckin_Fail;
			//	++m_statistics.m_statisticsLogin_Fail;

			//	// Check)
			//	Debug.Assert(false);
			//}
			catch(Exception)
			{
				// Statistics)
				++m_statistics.m_statisticsCheckin_Fail;
				++m_statistics.m_statisticsLogin_Fail;

				// Check)
				Debug.Assert(false);
			}

			// 8) AccountOld가 있으면 Checkin Count를 감소시킨다..
			if(pAccountOld != null)
			{
				pAccountOld.ReleaseCheckinCount();
			}
		}
		else
		{
			// Statistics)
			++m_statistics.m_statisticsCheckin_Fail;

			if (_pTransactable.m_pAccessable != null)
			{
				_pTransactable.m_pAccessable.ProcessAccountCompleteCheckin(_pTransactable.m_ridSource, _pTransactable.m_cid, eResult, _pTransactable.m_bufResult);
			}
		}

		// 9) Checkin Count를 일단 1을 감소시킨다.
		if(pAccount!=null)
		{
			pAccount.ReleaseCheckinCount();
		}

		// Return) 
		return	bResult;
	}

	//! @brief 계정의 [체크인]을 요청한다.
	public virtual eOBJECT_RESULT		Transactable_RequestCheckin_ACCOUNT_ID	(ITransactableAccount<TACCOUNT> _pTransactable)
	{
		// * 기존에 등록된 AID가 필요하기 때문에 이미 Login한 ID를 다른 계정이 접속하고자할 때 사용된다.
		//
		// 1) AID에서 해당 계정을 찾는다.
		// CASE A) 찾지 못했으면 Fail을 리턴한다.
		// CASE B) 찾았으면 Check-in을 한번 더 수행해 Checkin ID를 발급해 처리한다. - 이때 Checkin ID를 새로 등록하여 발급한다.
		//         새로운 Checkin ID를 발급받아 Check-in처리를 한다.

		// Check) 
		if(_pTransactable == null)
			return	eOBJECT_RESULT.INVALID_ARGUMENT;

		// Check)
		Debug.Assert(_pTransactable != null);

		// 1) 임시로 복사
		TACCOUNT		pAccount	 = _pTransactable.m_pAccount as TACCOUNT;
		IAccessable		pAccessable	 = _pTransactable.m_pAccessable;
		IAccount		pAccountOld	 = null;

		// Check)
		Debug.Assert(pAccount != null);

		// Declare)
		eOBJECT_RESULT	eResult		 = eOBJECT_RESULT.SUCCESS;

		try
		{
			// Declare)
			CHECKIN_ID	cidNew;

			lock(m_csAccount)
			{
				// 1) Checkin Count를 증가시킨다.
				pAccount.AddCheckinCount();

				// 2) Checkin_id를 새로 발급받는다.
				cidNew	 = GenerateNewCID(_pTransactable.m_aid);

				// Check) CID가 제대로 되었는 지 확인한다.
				Debug.Assert(cidNew.valid()==true);

				// 3) 새로 할당한 추가한다.
				m_mapAccountByCID.Add(cidNew, pAccount);

				try
				{
					// 4-Pre) 
					ACCOUNT_ITEM	item	 = new ACCOUNT_ITEM();

					item.cid		 = cidNew;
					item.pAccessable = pAccessable;

					// 4) CID를 Account에 등록한다.
					pAccountOld	 = pAccount.ProcessAddCID(_pTransactable.m_ridSource, item);
				}
				catch(Exception)
				{
					// - Account에 등록실패했으면 Checkin을 취소한다.
					m_mapAccountByCID.Remove(cidNew);

					// Reraise) 
					throw;
				}
			}

			// 5) 
			_pTransactable.m_cid	 = cidNew;
		}
		catch(Exception)
		{
			// - Fail로 설정한다.
			eResult	 = eOBJECT_RESULT.FAIL;

			// Statistics)
			++m_statistics.m_statisticsCheckin_Fail;

			// - Release Checin count
			pAccount.ReleaseCheckinCount();

			// - Reset
			_pTransactable.m_cid.reset();
			_pTransactable.m_eResult	 = eResult;
        }

		// 6) AccountOld가 있으면 Checkin Count를 감소시킨다..
		if (pAccountOld != null)
		{
			pAccountOld.ReleaseCheckinCount();
		}

		// Return) 
		return	eResult;
	}
	public virtual bool					Transactable_CompleteCheckin_ACCOUNT_ID (ITransactableAccount<TACCOUNT> _pTransactable)
	{
		// Check)
		Debug.Assert(_pTransactable != null);

		// Check)
		Debug.Assert(_pTransactable.m_eResult!=eOBJECT_RESULT.REQUEST && _pTransactable.m_eResult!=eOBJECT_RESULT.NOT_DEFINED);

		// Declare)
		var	eResult	 = _pTransactable.m_eResult;

		// 1) _fCompletion함수를 호출한다.
		if (_pTransactable.m_fCompletion!=null)
		{
			eResult	 = _pTransactable.m_fCompletion(_pTransactable, eResult, _pTransactable.m_bufResult);
		}

		if(eResult == eOBJECT_RESULT.SUCCESS)
		{
			// Statistics)
			++m_statistics.m_statisticsCheckin;

			try
			{
				// 1) 임시로 복사
				IAccessable		pAccessable	 = _pTransactable.m_pAccessable;

				// Check) pAccessable이 null이면 안됀다.
				if(pAccessable==null)
				{
					//CGLOG.INFO_LOW(null, "Info) pAccessable is NULL (CGServerAccount::Repository::NBase<TACCOUNT>::Transactable_CompleteCheckin_ACCOUNT_ID)");
					throw	new Exception();	// CGException::CThrow_Error()
				}

				// 2) Process Complete Checkin을 수행한다.
				pAccessable.ProcessAccountCompleteCheckin(_pTransactable.m_ridSource, _pTransactable.m_cid, eResult, _pTransactable.m_bufResult);

				// Statistics)
				++m_statistics.m_statisticsCheckin_Success;
			}
			catch(Exception)
			{
				// Statistics)
				--m_statistics.m_statisticsCheckin;

				// Statistics)
				++m_statistics.m_statisticsCheckin_Fail;
			}
		}
		else
		{
			// Statistics)
			++m_statistics.m_statisticsCheckin_Fail;

			// - Process Complete Checkin을 수행한다.
			if (_pTransactable.m_pAccessable != null)
			{
				_pTransactable.m_pAccessable.ProcessAccountCompleteCheckin(_pTransactable.m_ridSource, _pTransactable.m_cid, eResult, _pTransactable.m_bufResult);
			}
		}

		// Return) 
		return	true;
	}

	//! @brief 계정의 [체크인]을 요청한다.
	public virtual eOBJECT_RESULT		Transactable_RequestCheckin_CHECKIN_ID	(ITransactableAccount<TACCOUNT> _pTransactable)
	{
		// Declare) 
		CHECKIN_ID	cid = _pTransactable.m_cid;

		// * 기존에 등록된 AID와 CID가 필요하기 때문이 일반적으로 기존의 접속을 계승해 사용된다.
		// 1) CID에서 해당 계정을 찾는다.
		// CASE A) 찾지 못했으면 Fail을 리턴한다.
		// CASE B) 찾았으면 기존의 Check-in 정보를 그대로 계승하고 이전의 Checkin Accessor는 제거한다.
		//         기존의 Checkin-id를 그대로 사용한다.

		// Check) AID의 유효성을 검사한다.
		if(cid.valid()==false)
		{
			throw	new Exception();	// CGException::CThrow_InvalidArgument()
		}

		// Check) Repository가 동일한지 확인한다.
		if(cid.didRepository!=RepositoryID)
		{
			throw	new Exception();	// ExceGException::CThrow_InvalidArgument(), );
		}

		// Check)
		Debug.Assert(_pTransactable != null);

		// 1) 임시로 복사
		TACCOUNT		pAccount	 = _pTransactable.m_pAccount as TACCOUNT;
		IAccessable		pAccessable	 = _pTransactable.m_pAccessable;
		IAccount		pAccountOld	 = null;

		// Check)
		Debug.Assert(pAccount != null);

		// Declare)
		eOBJECT_RESULT	eResult		 = eOBJECT_RESULT.SUCCESS;

		try
		{
			lock(m_csAccount)
			{
				// 1) Checkin Count를 증가시킨다.
				pAccount.AddCheckinCount();

				// Declare) 
				TACCOUNT	iterCID;

				// 2) 먼저 기존 CID 정보를 찾는다.
				if(m_mapAccountByCID.TryGetValue(cid, out iterCID)==false)
				{
					//CGLOG.INFO(null, "Info) CID is not exist [CID:"+ cid.keyLogin + "] (CGServerAccount::Repository::NBase<TACCOUNT>::Transactable_RequestCheckin_CHECKIN_ID)");
					throw	new Exception();
				}

				// Declare)
				CHECKIN_ID	cidNew	 = GenerateNewCID(cid.idAccount);

				// Check) CID가 제대로 되었는 지 확인한다.
				Debug.Assert(cidNew.valid()==true);

				// 4) 새로 할당한 추가한다. (실패할 일도 없지만 실패하면 Exception을 던짐.)
				m_mapAccountByCID.Add(cidNew, pAccount);

				try
				{
					ACCOUNT_ITEM	item;
					item.cid			 = cidNew;
					item.pAccessable	 = pAccessable;

					// 5) CID를 Account에 등록한다.
					pAccountOld	 = pAccount.ProcessAddCID(_pTransactable.m_ridSource, item);
				}
				catch(Exception)
				{
					// - Account에 등록실패했으면 Checkin을 취소한다.
					m_mapAccountByCID.Remove(cidNew);

					// Reraise) 
					throw;
				}

				// 6) 기존 Accessable은 제거한다.
				iterCID.ProcessRemoveCID(cid);

				// 7) 기존 Accessable을 Release한다.
				iterCID.ReleaseCheckinCount();

				// 8) 기존 CID 정보를 제거한다.
				m_mapAccountByCID.Remove(cid);
			}

			// 9) AccountOld가 있으면 Checkin Count를 감소시킨다..
			if (pAccountOld != null)
			{
				pAccountOld.ReleaseCheckinCount();
			}
		}
		catch(Exception)
		{
			// - Error Code
			eResult	 = eOBJECT_RESULT.FAIL;

			// Statistics)
			++m_statistics.m_statisticsCheckin_Fail;

			// - Release Checin count
			pAccount.ReleaseCheckinCount();

			// - Result를 설정한다.
			_pTransactable.m_eResult	 = eResult;
		}

		// Return) 
		return	eResult;
	}
	public virtual bool					Transactable_CompleteCheckin_CHECKIN_ID (ITransactableAccount<TACCOUNT> _pTransactable)
	{
		// Check)
		Debug.Assert(_pTransactable != null);

		// Check)
		Debug.Assert(_pTransactable.m_eResult!=eOBJECT_RESULT.REQUEST && _pTransactable.m_eResult!=eOBJECT_RESULT.NOT_DEFINED);

		// Declare)
		var	eResult	 = _pTransactable.m_eResult;

		// 1) _fCompletion함수를 호출한다.
		if (_pTransactable.m_fCompletion!=null)
		{
			eResult	 = _pTransactable.m_fCompletion(_pTransactable, eResult, _pTransactable.m_bufResult);
		}

		if(eResult==eOBJECT_RESULT.SUCCESS)
		{
			// Statistics)
			++m_statistics.m_statisticsCheckin;

			try
			{
				// 1) 임시로
				IAccessable		pAccessable	 = _pTransactable.m_pAccessable;

				// Check) pAccessable이 null이면 안됀다.
				if(pAccessable==null)
				{
					CGLOG.INFO_LOW(null, "Info) pAccessable is NULL (CGServerAccount::Repository::NBase<TACCOUNT>::Transactable_CompleteCheckin_CHECKIN_ID)");
					throw	new Exception();        // CGException::CThrow_Error()
				}

				// 2) Process Complete Checkin을 수행한다.
				pAccessable.ProcessAccountCompleteCheckin(_pTransactable.m_ridSource, new CHECKIN_ID(), eResult, _pTransactable.m_bufResult);
			}
			catch(Exception)
			{
				// Statistics)
				--m_statistics.m_statisticsCheckin;

				// Statistics)
				++m_statistics.m_statisticsCheckin_Fail;
			}
		}
		else
		{
			// Statistics)
			++m_statistics.m_statisticsCheckin_Fail;

			// - Process Complete Checkin을 수행한다.
			if (_pTransactable.m_pAccessable == null)
			{
				_pTransactable.m_pAccessable.ProcessAccountCompleteCheckin(_pTransactable.m_ridSource, new CHECKIN_ID(), eResult, _pTransactable.m_bufResult);
			}
		}

		// Return) 
		return	true;
	}

	//! @brief 계정의 [체크인]을 요청한다.
	public virtual eOBJECT_RESULT		Transactable_RequestCheckin_Custom		(ITransactableAccount<TACCOUNT> _pTransactable)
	{
		// 1) Result
		var	result	 = eOBJECT_RESULT.SUCCESS;

		// 2) Logout요청 처리를 한다.
		_pTransactable.SetResult(result);

		// Return) 
		return	result;
	}
	public virtual bool					Transactable_CompleteCheckin_Custom		(ITransactableAccount<TACCOUNT> _pTransactable)
	{
		// Check)
		Debug.Assert(_pTransactable != null);

		// Check)
		Debug.Assert(_pTransactable.m_eResult!=eOBJECT_RESULT.REQUEST && _pTransactable.m_eResult!=eOBJECT_RESULT.NOT_DEFINED);

		// Declare)
		bool	bResult	 = false;

		// Declare)
		var		eResult	 = _pTransactable.m_eResult;

		// 1) _fCompletion함수를 호출한다.
		if (_pTransactable.m_fCompletion!=null)
		{
			eResult	 = _pTransactable.m_fCompletion(_pTransactable, eResult, _pTransactable.m_bufResult);
		}


		if(eResult==eOBJECT_RESULT.SUCCESS)
		{
			// Statistics)
			++m_statistics.m_statisticsCheckin;

			try
			{
				// 1) 임시로
				IAccessable		pAccessable	 = _pTransactable.m_pAccessable;

				// Check) pAccessable이 null이면 안됀다.
				if(pAccessable==null)
				{
					CGLOG.INFO_LOW(null, "Info) pAccessable is NULL (CGServerAccount::Repository::NBase<TACCOUNT>::Transactable_CompleteCheckin_Custom)");
					throw	new Exception();    // CGException::CThrow_Error()
				}

				// 2) Process Complete Checkin을 수행한다.
				pAccessable.ProcessAccountCompleteCheckin(_pTransactable.m_ridSource, new CHECKIN_ID(), eResult, _pTransactable.m_bufResult);

				// Statistics)
				++m_statistics.m_statisticsCheckin_Success;

				// 3) 성공!
				bResult	 = true;
			}
			catch(Exception)
			{
				// Statistics)
				--m_statistics.m_statisticsCheckin;

				// Statistics)
				++m_statistics.m_statisticsCheckin_Fail;
			}
		}
		else
		{
			// Statistics)
			++m_statistics.m_statisticsCheckin_Fail;

			// - Process Complete Checkin을 수행한다.
			if(_pTransactable.m_pAccessable!=null)
			{
				_pTransactable.m_pAccessable.ProcessAccountCompleteCheckin(_pTransactable.m_ridSource, new CHECKIN_ID(), eResult, _pTransactable.m_bufResult);
			}
		}

		// Return) 
		return	bResult;
	}

	//! @brief 계정의 [체크아웃]을 요청한다. @param _cid 대상 체크인 ID @return eOBJECT_RESULT
	public virtual eOBJECT_RESULT		Transactable_RequestCheckout			(ITransactableAccount<TACCOUNT> _pTransactable)
	{
		// 1) Result
		var	result	 = eOBJECT_RESULT.SUCCESS;

		// 2) Logout요청 처리를 한다.
		_pTransactable.SetResult(result);

		// Return) 
		return	result;
	}
	public virtual bool					Transactable_CompleteCheckout			(ITransactableAccount<TACCOUNT> _pTransactable)
	{
		// Check)
		if(ObjectState<eOBJECT_STATE.RUNNING)
		{
			//CGLOG.EXCEPTION(null, "Error) Repository is not ready (NOT RUNNING, State: %d) [%s] ", ObjectState, Name);
			return	false;
		}

		// Check)
		Debug.Assert(_pTransactable != null);

		// Check) _pAccount가 null이다.
		if(_pTransactable == null)
			return	false;

		// 1) Result를 임시 복사
		var	eResult	 = _pTransactable.m_eResult;

		// 2) _fCompletion함수를 호출한다.
		if (_pTransactable.m_fCompletion!=null)
		{
			eResult	 = _pTransactable.m_fCompletion(_pTransactable, eResult, _pTransactable.m_bufResult);
		}

		if(eResult==eOBJECT_RESULT.SUCCESS)
		{
			// 3) 임시로
			var		pAccount	 = _pTransactable.m_pAccount;

			// Check)
			Debug.Assert(pAccount!=null);

			// Check) _pAccount가 null이다.
			if(pAccount==null)
				return	false;

			lock(m_csAccount)
			{
				// 4) 제거한다.
				var	result	 = m_mapAccountByCID.Remove(_pTransactable.m_cid);

			// Check) 찾지 못했으면 eOBJECT_RESULT.NOT_READY를 리턴한다.
				if(result==false)
				{
					return	false;
				}

				// 6) Account에서 CID를 제거한다.
				pAccount.ProcessRemoveCID(_pTransactable.m_cid);
			}

			// 7) Check Count를 Release한다.
			pAccount.ReleaseCheckinCount();
		}
		else
		{
			// Not Ready
			Debug.Assert(false);
		}

		// Statistics)
		++m_statistics.m_statisticsCheckout_Complete;

		// Statistics)
		--m_statistics.m_statisticsCheckin;

		// Return) 
		return	true;
	}

	//! @brief 계정의 [체크아웃]을 요청한다. @param _aid 계정 ID @return true eOBJECT_RESULT
	public virtual eOBJECT_RESULT		Transactable_RequestLogout				(ITransactableAccount<TACCOUNT> _pTransactable)
	{
		// 1) Result
		var	result	 = eOBJECT_RESULT.SUCCESS;

		// 2) Logout요청 처리를 한다.
		_pTransactable.SetResult(result);

		// Return) 
		return	result;
	}
	public virtual bool					Transactable_CompleteLogout				(ITransactableAccount<TACCOUNT> _pTransactable)
	{
		// Check)
		Debug.Assert(_pTransactable != null);

		// Check)
		Debug.Assert(_pTransactable.m_pAccount!=null);

		// Declare)
		ACCOUNT_ID	aid;

		// 1) 임시로 복사
		var		pAccount	 = _pTransactable.m_pAccount;

		lock(m_csAccount)
		{
			// 1) Account ID를 얻는다.
			aid	 = pAccount.AccountID;

			// 2) 찾는다.
			var	result = m_mapAccountByAID.Remove(aid);

			// Check) 찾지 못했으면 false를 리턴한다.
			if(result==false)
				return	false;
		}

		// 5) _fCompletion함수를 호출한다.
		if(_pTransactable.m_fCompletion!=null)
		{
			_pTransactable.m_fCompletion(_pTransactable, _pTransactable.m_eResult, _pTransactable.m_bufResult);
		}

		// 6) Caching처리한다.
		try
		{
			lock(m_csAccount)
			{
				// Check) 이미 같은 ACCOUNT_ID를 가진 객체가 있는지 확인한다.
				Debug.Assert(m_mapAccountCached.ContainsKey(aid)==false);

				// - Account의 상태를 eOBJECT_STATE::STOPPED상태로 한다.
				pAccount.ObjectState	 = eOBJECT_STATE.STOPPED;

				// - 추가한다.
				m_mapAccountCached.Add(aid, pAccount);
			}
		}
		catch(Exception /*e*/)
		{
			// - 소멸시킨다.
			pAccount.ProcessAccountFree();
		}

		// Statistics)
		++m_statistics.m_statisticsLogout_Complete;

		// Statistics)
		--m_statistics.m_statisticsLogin;

		// Return) 
		return	true;
	}

	//! @brief 계정의 [트랜젝션]을 요청한다. @param _cid 대상 체크인 ID @return eOBJECT_RESULT
	public virtual eOBJECT_RESULT		Transactable_RequestTransaction			(ITransactableAccount<TACCOUNT> _pTransactable)
	{
		// Check)
		Debug.Assert(_pTransactable != null);

		// Check)
		Debug.Assert(_pTransactable.m_pAccount!=null);

		// Check)
		if(ObjectState<eOBJECT_STATE.RUNNING)
		{
			_pTransactable.SetResult(eOBJECT_RESULT.NOT_READY);
			return	eOBJECT_RESULT.NOT_READY;
		}

		// 1) Request를 진행한다.
		return _pTransactable.m_pAccount.ProcessAccountTransaction(_pTransactable.m_cid);
	}
	public virtual bool					Transactable_CompleteTransaction		(ITransactableAccount<TACCOUNT> _pTransactable)
	{
		// Check)
		Debug.Assert(_pTransactable != null);

		// Check)
		Debug.Assert(_pTransactable.m_eResult!=eOBJECT_RESULT.REQUEST && _pTransactable.m_eResult!=eOBJECT_RESULT.NOT_DEFINED);

		// Declare)
		bool	bResult	 = false;

		// 1) Result를 복사해서 사용한다.
		var		eResult	 = _pTransactable.m_eResult;

		try
		{
			// 2) _fCompletion함수를 호출한다.
			if(_pTransactable.m_fCompletion!=null)
			{
				eResult	 = _pTransactable.m_fCompletion(_pTransactable, eResult, _pTransactable.m_bufResult);
			}

			// 3) 임시로 복사
			var		pAccessable	 = _pTransactable.m_pAccessable;

			// 4) Accessable의 ProcessAccountCompleteTransaction을 호출한다.
			if(pAccessable!=null)
			{
				pAccessable.ProcessAccountCompleteTransaction(_pTransactable.m_ridSource, _pTransactable.m_cid, _pTransactable.m_eResult, _pTransactable.m_bufResult);
			}

			// 5) 성공!
			bResult	 = true;
		}
		catch(Exception)
		{
			// - 
			eResult	 = eOBJECT_RESULT.EXCEPTION;
		}

		// Statistics) 
		if(eResult==eOBJECT_RESULT.SUCCESS)
		{
			++m_statistics.m_statisticsCreate_Success;
		}
		else
		{
			++m_statistics.m_statisticsCreate_Fail;
		}

		// Return) 
		return	bResult;
	}

	// 1) Name
	private	string								m_strName					 = "";

	// 2) 계정 정보
	protected object							m_csAccount			 = new object();
	protected Dictionary<ACCOUNT_ID, TACCOUNT>	m_mapAccountByAID	 = new Dictionary<ACCOUNT_ID, TACCOUNT>();
	protected Dictionary<CHECKIN_ID, TACCOUNT>	m_mapAccountByCID	 = new Dictionary<CHECKIN_ID, TACCOUNT>();
	protected Dictionary<ACCOUNT_ID, TACCOUNT>	m_mapAccountCached	 = new Dictionary<ACCOUNT_ID, TACCOUNT>();

	// 임시)  ---------------------
	private static long sAID	= 0;
	private static UInt64			GenerateUID()
	{
		UInt64 temp = (UInt64)Interlocked.Increment(ref sAID);

		if (temp == 0)
			temp = (UInt64)Interlocked.Increment(ref sAID);

		return	temp;
	}
	// ---------------------------

	public CHECKIN_ID			GenerateNewCID(ACCOUNT_ID _aid)
	{
		// Declare) 
		CHECKIN_ID	cid;

		// 1) ACCOUNT_ID를 설정한다.
		cid.idAccount		 = _aid;

		// 2) Repository ID를 설정한다.
		cid.didRepository	 = RepositoryID;

		// 3) Login ID를 발급한다.
		cid.keyLogin		 = _ProcessGenerateNewCID();

		// Return) 
		return cid;
	}

	// Static)
	protected static UInt64		idCheckinGenerate	 = 0;

	public	UInt64				_ProcessGenerateNewCID()
	{
		return	++idCheckinGenerate;
	}

}

}
}
