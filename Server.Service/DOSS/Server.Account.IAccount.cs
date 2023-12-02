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
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace CGDK
{
namespace CGServerAccount
{
	public struct ACCOUNT_ITEM
	{
		public CHECKIN_ID	cid;
		public IAccessable	pAccessable;
	}

	public abstract class IAccount : ICGObjectStateable
	{
	// Constructors) 
		public IAccount()
		{
		}
		~IAccount()
		{
		}

	// Public) 
		// 1) Repository
		public IRepository				Repository
		{
			get {	return	m_pRepository; }
		}
		public ACCOUNT_ID				AccountID
		{
			get {	return m_aid; }
		}

		// 2) Account LockCount
		public void						AddCheckinCount()
		{
			lock(m_csAccount)
			{
				// 1) Reference Count를 증가시킨다.
				++m_countLock;
			}
		}
		public void						ReleaseCheckinCount()
		{
			lock(m_csAccount)
			{
				// Check)
				Debug.Assert(m_countLock>0);

				// 1) Reference Count를 깐다.
				var	iCount	 = --m_countLock;

				// Check)
				if(iCount==0)
				{
					if(m_pRepository!=null)
					{
						m_pRepository.ProcessCompleteCheckoutAll(this);
					}
				}
			}
		}
		public int						CheckinPoint
		{
			get { return m_countLock;}
		}

		// 3) Accessable
		public IAccessable				FindAccessable(CHECKIN_ID _cid)
		{
			// Declare) 
			IAccessable		pAccessable	 = null;

			lock(m_csAccount)
			{
				// - 찾는다.
				var	itemFind	 = m_vectorCID.Find(x=>x.cid==_cid);

				// - 찾아서 리턴~
				pAccessable	 = itemFind.pAccessable;
			}

			// Return)
			return	pAccessable;
		}

		// 4) Operation
		public virtual eOBJECT_RESULT	RequestAccountCheckout(REQUEST_ID _tidRequest, CHECKIN_ID _cid, TRESPONSE _fCompletion=null)
		{
			// 1) Repository를 얻는다.
			var	pRepository	 = Repository;

			// Check) pRepository가 없으면 eOBJECT_RESULT.NOT_READY에러를 되돌리며 끝낸다.
			if(pRepository==null)
				return	eOBJECT_RESULT.NOT_READY;

			// 2) Request한다.
			return	pRepository.RequestAccountCheckout(_tidRequest, _cid, this, _fCompletion);
		}
		public virtual eOBJECT_RESULT	RequestAccountTransaction(REQUEST_ID _tidRequest, CHECKIN_ID _cid, IAccessable _pAccessable, CGD.buffer _bufRequest, TREQUEST _fRequest=null, TRESPONSE _fCompletion=null)
		{
			// 1) Repository를 얻는다.
			var	pRepository	 = Repository;

			// Check) pRepository가 없으면 eOBJECT_RESULT.NOT_READY에러를 되돌리며 끝낸다.
			if(pRepository==null)
				return	eOBJECT_RESULT.NOT_READY;

			// 2) Request한다.
			return	pRepository.RequestAccountTransaction(_tidRequest, _cid, _pAccessable, _bufRequest, _fRequest, _fCompletion);
		}
		public virtual eOBJECT_RESULT	RequestAccountTransaction(CGD.buffer _bufRequest, TREQUEST _fRequest=null, TRESPONSE _fCompletion=null)
		{
			// 1) Repository를 얻는다.
			var	pRepository	 = Repository;

			// Check) pRepository가 없으면 eOBJECT_RESULT.NOT_READY에러를 되돌리며 끝낸다.
			if(pRepository==null)
				return	eOBJECT_RESULT.NOT_READY;

			// 2) Request한다.
			return	pRepository.RequestAccountTransaction(m_aid, _bufRequest, _fRequest, _fCompletion);
		}
		public virtual eOBJECT_RESULT	RequestAccountUpdate(REQUEST_ID _tidRequest, CGD.buffer _bufRequest, TREQUEST _fRequest=null, TRESPONSE _fCompletion=null)
		{
			// 1) Repository를 얻는다.
			var	pRepository	 = Repository;

			// Check) pRepository가 없으면 eOBJECT_RESULT.NOT_READY에러를 되돌리며 끝낸다.
			if(pRepository==null)
				return	eOBJECT_RESULT.NOT_READY;

			// 2) Request한다.
			return	pRepository.RequestAccountUpdate(_tidRequest, m_aid, null, _bufRequest, _fRequest, _fCompletion);
		}
		public virtual eOBJECT_RESULT	RequestAccountUpdate(CGD.buffer _bufRequest, TREQUEST _fRequest=null, TRESPONSE _fCompletion=null)
		{
			// 1) Repository를 얻는다.
			var	pRepository	 = Repository;

			// Check) pRepository가 없으면 eOBJECT_RESULT.NOT_READY에러를 되돌리며 끝낸다.
			if(pRepository==null)
				return	eOBJECT_RESULT.NOT_READY;

			// 2) Request한다.
			return	pRepository.RequestAccountUpdate(m_aid, _bufRequest, _fRequest, _fCompletion);
		}
		public virtual void				RequestAccountForceCheckoutAll()
		{
			// 1) Repository를 얻는다.
			var	pRepository	 = Repository;

			// Check) pRepository가 없으면 eOBJECT_RESULT.NOT_READY에러를 되돌리며 끝낸다.
			if(pRepository==null)
				return;

			lock(m_csAccount)
			{
				// Check)
				Debug.Assert(m_vectorCID.Count!=0);

				// 2) 모든 Checkin들을 제거한다.
				foreach(var iter in m_vectorCID)
				{
					try
					{
						// - 강제로 제거한다.
						pRepository.ProcessAccountForceCheckout(iter.cid);

						// - 강제로 제거한다.
						iter.pAccessable.ProcessAccountForceCheckout(iter.cid);
					}
					catch(Exception)
					{
					}
				}

				// 3) Checkin들을 모두 제거한다.
				m_vectorCID.Clear();
			}
		}
		public void						RequestAccountCheckoutAll()
		{
			// 1) Repository를 얻는다.
			var	pRepository = Repository;

			// Check) pRepository가 없으면 eOBJECT_RESULT.NOT_READY에러를 되돌리며 끝낸다.
			if (pRepository == null)
				return;

			// Declare) 
			List<CHECKIN_ID> vectorCID = new List<CHECKIN_ID>();

			lock(m_csAccount)
			{
				// Check)
				Debug.Assert(m_vectorCID.Count!=0);

				// 2) 모든 Checkin들을 제거한다.
				foreach(var iter in m_vectorCID)
				{
					vectorCID.Add(iter.cid);
				}
			}

			foreach(var iter in vectorCID)
			{
				pRepository.RequestAccountCheckout(new REQUEST_ID(), iter, this, null);
			}
		}

		// 5) Serialize In/Out/Update
		public virtual eOBJECT_RESULT	RequestAccountSerializeIn(CGD.buffer _Buffer)
		{
			return	ProcessAccountSerializeIn(_Buffer);
		}
		public virtual eOBJECT_RESULT	RequestAccountSerializeOut(CGD.buffer _Buffer)
		{
			return	ProcessAccountSerializeOut(_Buffer);
		}


	// Framework)
		// 1) Account Alloc/Free
		public abstract	void			ProcessAccountAlloc();
		public abstract void			ProcessAccountFree();

		// 2) Account Checkin/Checkout/Transaction/CheckoutAll
		public abstract	eOBJECT_RESULT	ProcessAccountCheckin();
		public abstract	eOBJECT_RESULT	ProcessAccountCheckout(CHECKIN_ID _cid);
		public abstract	eOBJECT_RESULT	ProcessAccountTransaction(CHECKIN_ID _cid);

		// 3) Serialize In/Out/Update
		public abstract	eOBJECT_RESULT	ProcessAccountSerializeIn(CGD.buffer _Buffer);
		public abstract	eOBJECT_RESULT	ProcessAccountSerializeOut(CGD.buffer _Buffer);
		public abstract	eOBJECT_RESULT	ProcessAccountUpdate(CGD.buffer _bufRequest);

		// 4) ProcessAccountRequestUpdate
		public virtual	eOBJECT_RESULT	ProcessAccountRequestUpdate(CGD.buffer _bufRequest)
		{
			// 1) Account의 ProcessAccountUpdate를 호출한다.
			ProcessAccountUpdate(_bufRequest);

			// 2) 모든 Checkin된 Accessable에 RequestAccountUpdate를 전달
			lock(m_csAccount)
			{
				foreach(var iter in m_vectorCID)
				{
					iter.pAccessable.ProcessAccountUpdate(iter.cid, this, _bufRequest);
				}
			}

			// Return) 
			return	eOBJECT_RESULT.SUCCESS;
		}


	// Implementations)
		public eOBJECT_STATE			ObjectState
		{
			get
			{
				return (eOBJECT_STATE)m_eStateAccount;
			}
			set
			{
				Interlocked.Exchange(ref m_eStateAccount, (int)value);
			}
		}
		public bool						SetObjectStateIf(eOBJECT_STATE _Value, eOBJECT_STATE _compare)
		{
			// 1) eOBJECT_STATE.NONE 상태일 때만 eOBJECT_STATE.INITIALIZING_PENDING 상태로 바꾼다.
			int statePre = Interlocked.CompareExchange(ref m_eStateAccount, (int)_Value, (int)_compare);

			// Return) 상태가 바뀌었으면 true 아니면 false를 리턴한다.
			return (statePre == (int)_compare);
		}

		public bool						Attach(ICGObjectStateable _Child) { return false;}
		public int						Detach(ICGObjectStateable _Child) { return 0;}
		public IEnumerator 				GetEnumerator()
		{
			return null;
		}


	// Implementations) 
		// 1) Lock object for Account
		public	object					m_csAccount			 = new object();

		// 2) 이 계정을 저장하는 Reposityr 객체 (Source)
		public	IRepository				m_pRepository		 = null;
		public	CHECKIN_ID				m_cid;

		// 3) Account ID
		public	ACCOUNT_ID				m_aid;

		// 4) Object State and Revision
		public	int						m_eStateAccount;
		public	UInt64					m_iRevision;

		// 5) Check out Info (이 Accoount를 Checkout해간 Accessable들)
		public	int						m_countLock;
		public	List<ACCOUNT_ITEM>		m_vectorCID			 = new List<ACCOUNT_ITEM>();

		// 6) Request Queue
		public	Transaction.CTransactor	m_queueTransaction	 = new Transaction.CTransactor();

	// Implementations) 
		public	IAccount				ProcessAddCID(REQUEST_ID _tid, ACCOUNT_ITEM _New)
		{
			// Declare) 
			IAccount	pAccountOld	 = null;

			lock(m_csAccount)
			{
				// Check) 이미 존재하고 있으면 안됀다.
				Debug.Assert(m_vectorCID.FindAll(x=>x.cid==_New.cid).Count==0);

				//// Check) 이미 존재하고 있으면 안됀다.
				//CGASSERT_ERROR(std::find_if(m_vectorCID.begin(), m_vectorCID.end(), [=](const std::pair<CHECKIN_ID, IAccessable*>& _iter) { return _iter.first==_New.first;})==m_vectorCID.end());

				// 1) 설정한다.
				pAccountOld	 = _New.pAccessable.ProcessAccountSet(_tid, this, _New.cid);

				// 2) 추가하기
				m_vectorCID.Add(_New);
			}

			// 3) 기존 Account는 Remove CID를 한다.
			if(pAccountOld != null)
			{
				pAccountOld.ProcessRemoveCID(_New.cid);
			}

			// Return)
			return	pAccountOld;
		}
		public	void					ProcessRemoveCID(CHECKIN_ID _cid)
		{
			lock(m_csAccount)
			{
				m_vectorCID.RemoveAll(x=>
				{
					bool	isThis	 = x.cid == _cid;

					if(isThis==true)
					{
						x.pAccessable.ProcessAccountSet(new REQUEST_ID(), null, _cid);
					}

					return isThis;
				});
			}
		}

		public virtual	eOBJECT_RESULT	RequestTransaction(ITransactable _pTransactable)
		{
			return m_queueTransaction.RequestTransaction(_pTransactable);
		}

	}

}
}
