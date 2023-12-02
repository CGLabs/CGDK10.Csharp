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
using System.Runtime.InteropServices;
using CGDK.CGServerAccount;

namespace CGDK
{

namespace CGTransaction
{

public class CTransactorConcurrent : 
	ICGDestroyable
{
// Constructor/Destructor)
	public	CTransactorConcurrent()
			{

			}
	~CTransactorConcurrent()
			{
			}

	public	eOBJECT_RESULT		RequestTransaction(TRANSACTION_ID _Key, ITransactable _pTransactable)
	{
		// Check) _pTransactable이 null이면안됀다.
		Debug.Assert(_pTransactable != null);

		// Check) Transactable이 null이면 0을 리턴한다.
		if(_pTransactable==null)
			return	eOBJECT_RESULT.INVALID_ARGUMENT;

		lock(m_managerTransactable)
		{
			// Check) m_iterPending._Ptr이 null이어야만 한다.

			// Check) _pTransactable의 Transaction state는 반드시 eTRANSACTION_STATE.NONE상태여야 한다.
			Debug.Assert(_pTransactable.m_eResult==eOBJECT_RESULT.NOT_DEFINED);

			// Check)
			Debug.Assert(_Key.exist());

			// 1) 먼저 Register한다. (실패하면 Exception을 던진다.)
			m_managerTransactable.Register(_pTransactable, _Key);

			//// 2) tid를 설정한다.
			//_pTransactable.m_pTransactor	 = this;

			// 3) Transaction Register
			_pTransactable._TransactionRegister();

			// 4) 즉시 Request에 걸 것인지를 결정한다.(m_maxConcurrentTransactableCount보다 더 많이 걸려 있으면 그냥 Queueing한다.)
			var bRequest	 = m_managerTransactable.RegisteredCount<m_maxConcurrentTransactableCount;

			// 5) 만약 최대 동시 처리 갯수보다 더 많이 처리 중이라면 지연처리를 한다.(Queuing만하고 PENDING을 리턴한다.)
			if(bRequest==false)
			{
				// - 지연처리를 한다.
				_ProcessPendingRequest(_Key, _pTransactable);

				// Statistics) 
				++m_statistics.count_ProcessRequest_Pended;

				// Return) 바로 리턴한다.
				return	eOBJECT_RESULT.PENDING;
			}
		}

		// Declare)
		eOBJECT_RESULT	eResult;

		try
		{
			// Statistics) 
			++m_statistics.count_ProcessRequest_Immidiately;

			// 6) Request한다.
			eResult	 = ProcessRequestTransaction(_pTransactable);
		}
		catch(Exception)
		{
			// Declare) 
			ITransactable	pTransactable	 = null;

			lock(m_managerTransactable)
			{
				// - 먼저 Registerd Queue에서 빼낸다.
				pTransactable	 = m_managerTransactable.Unregister(_Key);

				// - 빼기에 성공했으면 그 다음을 진행...
				if(pTransactable!=null)
				{
					_ProcessCancelPendedRequest(pTransactable);
				}

				// - ProcessUnregister호출
				pTransactable._TransactionUnregister();
			}

			// - Request Count를 Decrease한다.
			_ProcessDecreaseRequestCount();

			// Reraise)
			throw;
		}

		// 6) 완료 처리한다.
		if(eResult != eOBJECT_RESULT.PENDING)
		{
			CompleteTransaction(_Key);
		}

		// Return) 
		return	eResult;
	}
	public	ITransactable		CompleteTransaction(TRANSACTION_ID _Key)
	{
		// Declare)
		ITransactable	pTransactable	 = null;

		lock(m_managerTransactable)
		{
			// 1) 먼저 Registerd Queue에서 빼낸다.
			pTransactable	 = m_managerTransactable.Unregister(_Key);

			// Check) 없으면 그냥 끝낸다.
			if(pTransactable==null)
				return	pTransactable;

			//// Check) 
			//Debug.Assert(pTransactable.m_iterPending.get<circular_list<PENDED_TRANSACTION>::iterator>()._Ptr==nullptr);

			// 2) Unregister
			pTransactable._TransactionUnregister();
		}

		try
		{
			// 4) Complete 처리를 한다.
			pTransactable._TransactionComplete();
		}
		//catch (CGException::CThrow_Critical& /*e*/)
		//{
		//	// - 실행이 되었으면 새로운 객체를 하나 더 Request건다.
		//	_ProcessDecreaseRequestCount();

		//	// Reraise);
		//	throw;
		//}
		catch (Exception)
		{

		}

		// 5) 실행이 되었으면 새로운 객체를 하나 더 Request건다.
		_ProcessDecreaseRequestCount();

		// Return)
		return	pTransactable;
	}
	public	ITransactable		CompleteTransaction(TRANSACTION_ID _Key, eOBJECT_RESULT _eResult, CGD.buffer _bufResult)
	{
		// Declare)
		ITransactable	pTransactable	 = null;

		lock(m_managerTransactable)
		{
			// 1) 먼저 Registerd Queue에서 빼낸다.
			pTransactable	 = m_managerTransactable.Unregister(_Key);

			// Check) 없으면 그냥 끝낸다.
			if(pTransactable==null)
				return	pTransactable;

			//// Check) 
			//Debug.Assert(pTransactable.m_iterPending.get<circular_list<PENDED_TRANSACTION>.iterator>()._Ptr==null);

			// 2) Unregister
			pTransactable._TransactionUnregister();
		}

		try
		{
			// 4) Complete 처리를 한다.
			pTransactable._TransactionComplete(_eResult, _bufResult);
		}
		//catch (CGException.CThrow_Critical& /*e*/)
		//{
		//	// - 실행이 되었으면 새로운 객체를 하나 더 Request건다.
		//	_ProcessDecreaseRequestCount();

		//	// Reraise);
		//	throw;
		//}
		catch (Exception)
		{
		}

		// 5) 실행이 되었으면 새로운 객체를 하나 더 Request건다.
		_ProcessDecreaseRequestCount();

		// Return)
		return	pTransactable;
	}
	public	ITransactable		CancelTransaction(TRANSACTION_ID _Key)
	{
		// Declare)
		ITransactable	pTransactable	 = null;

		lock(m_managerTransactable)
		{
			// 1) 먼저 Registerd Queue에서 빼낸다.
			pTransactable	 = m_managerTransactable.Unregister(_Key);

			// 2) Pending에서 찾는다.
			if(pTransactable==null)
			{
				foreach(var iter in m_listPendingTransactable)
				{
					if(iter.key==_Key)
					{
						// - Transactable을 복사해 놓는다.
						pTransactable	 = iter.pTransactable;

						// - PendedRequest를 Cancel한다.
						_ProcessCancelPendedRequest(pTransactable);

						break;
					}
				}
			}

			// Check) pTransactable이 null이면 여기서 끝낸다.
			if(pTransactable==null)
				return	pTransactable;

			// 4) Unregister
			pTransactable._TransactionUnregister();
		}

		// 5) Complete 처리를 한다.
		try
		{
			pTransactable._TransactionCancel();
		}
		catch (Exception)
		{

		}

		// 6) 실행이 되었으면 새로운 객체를 하나 더 Request건다.
		_ProcessDecreaseRequestCount();

		// Return)
		return	pTransactable;
	}

	public	int					PendingTransactableCount
	{
		get
		{
			lock(m_listPendingTransactable)
			{
				return m_listPendingTransactable.Count;
			}
		}
	}
	public	int					MaxConcurrentTransactableCount
	{
		get { return m_maxConcurrentTransactableCount;}
		set { m_maxConcurrentTransactableCount=value;}
	}

	public virtual bool			Destroy()
	{
		lock(m_managerTransactable)
		{
			// Decalre) 
			List<ITransactable>	vectorRegistered;

			// 1) 먼저 Registerd Queue에서 빼낸다.
			vectorRegistered	 = m_managerTransactable.UnregisterAll();

			// 2) 모든 객체를 Unregister처리
			foreach(var iter in vectorRegistered)
			{
				_ProcessCancelPendedRequest(iter);
			}

			// 3) 모든 객체에 Cancel처리를 요청한다.
			foreach(var iter in vectorRegistered)
			{
				// - ProcessComplete...
				iter._TransactionCancel();

				// - Unregister
				iter._TransactionUnregister();
			}
		}

		// Return) 
		return	true;
	}

// Implementation) 
	public virtual eOBJECT_RESULT ProcessRequestTransaction(ITransactable _pTransactable)
	{
		// 1) Request를 요청한다.
		var	eResult	 = _pTransactable._TransactionRequest();

		// Statistics) Request 수를 증가시킨다.
		++m_statistics.count_ProcessingRequest;

		// Return)
		return	eResult;
	}

	private bool				_ProcessPendedTransactable()
	{
		// Declare) 
		PENDED_TRANSACTION	pendingRequest;
		bool				bRetry;
		eOBJECT_RESULT		eResult	 = eOBJECT_RESULT.EXCEPTION;

		do
		{
			lock(m_managerTransactable)
			{
				// Check) Pended된 것이 없다면 그냥 끝낸다.
				if(m_listPendingTransactable.Count==0)
					return	false;

				// Check) 현재 진행중인 Request가 Max Concurrent Requests보다 크면 끝낸다.
				if(m_statistics.count_ProcessingRequest>=m_maxConcurrentTransactableCount)
					return	false;

				// 1) 맨 앞의 것을 하나 가져온다.
				pendingRequest	 = m_listPendingTransactable[0];

				// 2) Queuing된 Request를 Pop해낸다.
				m_listPendingTransactable.RemoveAt(0);

				//// 3) pendingRequest의 Iterator 정보를 Reset한다.
				//pendingRequest.pTransactable.m_iterPending.get<circular_list<PENDED_TRANSACTION>.iterator>()._Ptr = null;

				// 4) Transactor를 Reset한다.
				pendingRequest.pTransactable = null;

				// 4) Retry를 false로...
				bRetry	 = false;
			}

			try
			{
				// 5) 실행한다.
				eResult	 = ProcessRequestTransaction(pendingRequest.pTransactable);
			}
			catch(Exception)
			{
				// Statistics) 
				++m_statistics.count_Fail;

				// - 재시도하도록 설정한다.
				bRetry	 = true;
			}
		}
		while(bRetry);

		// 6) eOBJECT_RESULT.PENDING이 아닐 경우 바로 TransactionComplete를 호출한다.
		if(eResult != eOBJECT_RESULT.PENDING)
		{
			pendingRequest.pTransactable._TransactionComplete();
		}

		// Return)
		return	true;
	}

	private void				_ProcessPendingRequest(TRANSACTION_ID _Key, ITransactable _PendedTransactable)
	{
		// Declare)
		PENDED_TRANSACTION	item	 = new PENDED_TRANSACTION();
		item.key			 = _Key;
		item.pTransactable	 = _PendedTransactable;

		// 1) 지연 List에 추가한다.
		m_listPendingTransactable.Add(item);
	}
	private void				_ProcessCancelPendedRequest(ITransactable _pTransactable)
	{
		// Check) _pTransactable이 null이면 안됀다.
		Debug.Assert(_pTransactable != null);

		// Check) _pTransactable이 nullptr이면 안됀다.
		Debug.Assert(_pTransactable.m_pTransactor==null);

		// 1) 전체 갯수 구하기
		var	countList	 = m_listPendingTransactable.Count;

		// 2) 전체를 돌며 해당 객체 찾기
		for(int i=0;i<countList; ++i)
		{
			if(m_listPendingTransactable[i].pTransactable==_pTransactable)
			{
				m_listPendingTransactable.RemoveAt(i);
				break;
			}
		}
	}
	private void				_ProcessDecreaseRequestCount()
	{
		lock(m_managerTransactable)
		{
			// Statistics) Request count를 줄인다.
			--m_statistics.count_ProcessingRequest;

			// Check) Pending List에 추가되어 있는지 확인한다.
			if(m_listPendingTransactable.Count==0)
				return;
		}

		// 2) 새로운 Request를 건다.
		_ProcessPendedTransactable();
	}

	// [처리 중인 요청 맵]
	private	CGManagerRegister<ITransactable>	m_managerTransactable	 = new CGManagerRegister<ITransactable>();
	// [요청 한계 갯수]
	private	int					m_maxConcurrentTransactableCount	 = int.MaxValue;

	private struct PENDED_TRANSACTION
	{
		public	TRANSACTION_ID	key;
		public	ITransactable	pTransactable;
	};

	// [지연 요청 큐]
	private	List<PENDED_TRANSACTION>	m_listPendingTransactable	 = new List<PENDED_TRANSACTION>();

	// Statistics)
	private	CGServerAccount.Statistics.Transactable	m_statistics;
}


}
}