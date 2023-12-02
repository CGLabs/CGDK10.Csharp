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
using CGDK;

namespace CGDK
{

namespace Transaction
{
	public class CTransactor
	{
		public CTransactor()
		{
		}
		~CTransactor()
		{
		}

		public eOBJECT_RESULT			RequestTransaction(ITransactable _pTransactable)
		{
			// Check) _pTransactable이 nullptr이면안됀다.
			Debug.Assert(_pTransactable != null);

			// Declare)
			bool	bEmpty	 = false;

			lock(m_listQueue)
			{
				// Check) _pTransactable의 Transaction state는 반드시 eOBJECT_RESULT.NOT_DEFINED상태여야 한다.
				Debug.Assert(_pTransactable.m_eResult == eOBJECT_RESULT.NOT_DEFINED);

				// Check) _pTransactable의 Transactor는 nullptr이어야 한다.
				Debug.Assert(_pTransactable.m_pTransactor == null);

				// 1) Queuing한다.
				m_listQueue.Add(_pTransactable);

				// 3) Transactor를 this로 설정한다.
				_pTransactable.m_pTransactor = this;

				// 3) Queued된 Size가 1이면
				bEmpty	 = m_listQueue.Count>1;

				// 4) TransactionRegister를 호출한다.
				_pTransactable._TransactionRegister();
			}

			// Check) 비어 있지 않을 경우 Queuing만 하고 끝낸다.
			if (bEmpty)
				return	eOBJECT_RESULT.PENDING;

			// 5) _pTransactable의 Request를 요청한다.
			return	_RequestTransaction(_pTransactable);
		}
		public bool						CompleteTransaction(ITransactable _pTransactable)
		{
			// Check)
			Debug.Assert(_pTransactable!=null);

			// Check)
			if(_pTransactable==null)
			{
				CGLOG.EXCEPTION(null, "Exception) _pTransactable is NULL (CTransactor::CompleteTransaction)");
				return	false;
			}

			// 1) _pTransactable을 Hold해준다. (m_listQueue에서 pop_front()해버리므로 할당해제되므로...)
			ITransactable	pTransactable		 = _pTransactable;

			// Check)
			ITransactable	pTransactableNext	 = null;

			lock(m_listQueue)
			{
				// Check) Queuing된 것이 없다면 그냥 끝낸다.
				if(m_listQueue.Count==0)
					return	true;

				// Check) 제일 앞의 것이 _pTransactable이여야만 한다.
				Debug.Assert(m_listQueue[0]==_pTransactable);

				// Check) _pTransactable의 m_pTransactor는 this여야만 한다.
				Debug.Assert(_pTransactable.m_pTransactor == this);

				// 1) Iterator를 Reset한다.

				// 2) _pTransactable의 m_pTransactor를 nullptr로 바꾼다.
				_pTransactable.m_pTransactor	 = null;

				// 3) 제일 앞에서 Queuing된 것을 하나 꺼낸다.
				m_listQueue.RemoveAt(0);

				// 4) 더이상 Queuing된 것이 없다면 그냥 끝낸다.
				if(m_listQueue.Count!=0)
				{
					// - 제일 앞의 Transactable을 얻는다.
					pTransactableNext	 = m_listQueue[0];

					// Check) pTransactable이 nullptr이면 안됀다.
					Debug.Assert(pTransactableNext!=null);
				}
			}

			try
			{
				// 5) Complete 처리를 한다.
				_pTransactable._TransactionComplete();
			}
			catch (Exception)
			{

			}

			// 5) TransactionUnregister를 호출한다.
			_pTransactable._TransactionUnregister();

			// 6) 다음 Transaction이 존재할 경우 Request한다.
			if(pTransactableNext!=null)
			{
				try
				{
					_RequestTransaction(pTransactableNext);
				}
				catch (Exception)
				{
				}
			}

			// Return)
			return	true;
		}
		public bool						CancelTransaction(ITransactable _pTransactable)
		{
			// 임시)
			Debug.Assert(false);

			// Return)
			return	false;
		}

		public bool						Destroy()
		{
			// Decalre) 
			List<ITransactable>	listQueue;

			// 1) 먼저 Registerd Queue에서 빼낸다.
			lock(m_listQueue)
			{
				listQueue	 = m_listQueue;
				m_listQueue	 = null;
			}

			// 2) 모든 객체에 Cancel처리를 요청한다.
			foreach(var iter in listQueue)
			{
				// - ProcessComplete...
				iter._TransactionCancel();

				// - Unregister
				iter._TransactionUnregister();
			}

			// Return) 
			return	true;
		}

		protected eOBJECT_RESULT		_RequestTransaction(ITransactable _pTransactable)
		{
			// Declare)
			eOBJECT_RESULT	eResult;

			try
			{
				// 1) 먼저 m_fRequest함수를 호출한다.
				if(_pTransactable.m_fRequest != null)
				{
					// m_fRequest를 호출한다. (여기서 실패시 바로 실패)
					eResult	 = _pTransactable.m_fRequest(_pTransactable);

					// Check) 여기서 Pending이어서는 안됀다.
					Debug.Assert(eResult != eOBJECT_RESULT.PENDING);

					// Check) 
					if(eResult!=eOBJECT_RESULT.SUCCESS)
						throw	new Exception();
				}

				// 2) 그런 다음 _pTransactable의 _TransactionRequest()함수를 호출한다.
				eResult	 = _pTransactable._TransactionRequest();
			}
			catch(Exception)
			{
				// Trace) 
				CGLOG.EXCEPTION(null, "Exception) TransactionRequest failed ");	//  _CGFUNCTION

				// - Result를 Exception으로 설정
				eResult		= eOBJECT_RESULT.EXCEPTION;
			}

			// 3) Pending이 아니라면 바로 완료 처리를 한다.(계속적 Recursion으로 불안정)
			if(eResult != eOBJECT_RESULT.PENDING)
			{
				CompleteTransaction(_pTransactable);
			}

			// Return)
			return	eResult;
		}

		protected List<ITransactable>	m_listQueue	 = new List<ITransactable>();
	}
}
}
