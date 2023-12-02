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
using System.Collections;
using System.Diagnostics;

namespace CGDK
{
namespace CGServerAccount
{

	public abstract class IRepository : ICGObjectStateable, ICGStartable, ICGInitializable, ICGNameable
	{
	// Publics)
		//! @brief 계정의 '체크아웃'을 요청한다. @param _aid 계정 ID @return true 성공 @return false 실패
		public abstract	eOBJECT_RESULT		RequestAccountCheckin		(REQUEST_ID _ridSource, CGD.buffer _bufRequest, IAccessable _pAccessable, TRESPONSE _Function);
		//! @brief 계정의 '체크인'을 요청한다. @param _cid 대상 체크인 ID @return true 성공 @return false 실패
		public abstract	eOBJECT_RESULT		RequestAccountCheckout		(REQUEST_ID _ridSource, CHECKIN_ID _cid, IAccount _pAccount, TRESPONSE _Function);
		//! @brief 계정의 '커밋'을 요청한다. @param _cid 대상 체크인 ID @return true 성공 @return false 실패
		public abstract	eOBJECT_RESULT		RequestAccountTransaction	(REQUEST_ID _ridSource, CHECKIN_ID _cid, IAccessable _pAccessable, CGD.buffer _bufRequest, TREQUEST _fRequest=null, TRESPONSE _fCompletion=null);
		public			eOBJECT_RESULT		RequestAccountTransaction	(ACCOUNT_ID _aid, CGD.buffer _bufRequest, TREQUEST _fRequest=null, TRESPONSE _fCompletion=null)
		{
			// 1) 임시 REQUEST_ID를 생성한다.
			REQUEST_ID	tidRequest	 = new REQUEST_ID();

			// 2) 임시 SourceID를 생성한다.
			SOURCE_ID	sidRequest	 = new SOURCE_ID();
			sidRequest.generate();

			// 2) REQUEST_ID를 Generate한다.
			tidRequest.generate(sidRequest);

			// 3) Request한다.
			return	RequestAccountUpdate(tidRequest, _aid, null, _bufRequest, _fRequest, _fCompletion);
		}
		//! @brief 계정의 '커밋'을 요청한다. @param _cid 대상 체크인 ID @return true 성공 @return false 실패
		public abstract	eOBJECT_RESULT		RequestAccountTransaction	(REQUEST_ID _ridSource, ACCOUNT_ID _aid, IAccessable _pAccessable, CGD.buffer _bufRequest, TREQUEST _fRequest=null, TRESPONSE _fCompletion=null);
		//! @brief 계정의 '커밋'을 요청한다. @param _cid 대상 체크인 ID @return true 성공 @return false 실패
		public abstract	eOBJECT_RESULT		RequestAccountUpdate		(REQUEST_ID _ridSource, ACCOUNT_ID _aid, IAccessable _pAccessable, CGD.buffer _bufRequest, TREQUEST _fRequest=null, TRESPONSE _fCompletion=null);
		public			eOBJECT_RESULT		RequestAccountUpdate		(ACCOUNT_ID _aid, CGD.buffer _bufRequest, TREQUEST _fRequest=null, TRESPONSE _fCompletion=null)
		{
			// 1) 임시 REQUEST_ID를 생성한다.
			REQUEST_ID	tidRequest	 = new REQUEST_ID();

			// 2) 임시 SourceID를 생성한다.
			SOURCE_ID	sidRequest	 = new SOURCE_ID();
			sidRequest.generate();

			// 3) REQUEST_ID를 Generate한다.
			tidRequest.generate(sidRequest);

			// 4) Request한다.
			return	RequestAccountUpdate(tidRequest, _aid, null, _bufRequest, _fRequest, _fCompletion);
		}

		//! @brief 데폿ID를 얻는다. @return 데폿ID
		public REPOSITORY_ID				RepositoryID
		{
			get { return m_ridRepository;}
			set { m_ridRepository=value; }
		}

		//! @brief 통계 정보를 얻는다. @return 통계 정보
		public Statistics.Repository		Statistics
		{
			get { return m_statistics;}
		}


	// Abstract) 
		abstract public string				Name
		{ 
			get;
			set;
		}

		public abstract eOBJECT_STATE		ObjectState
		{
			get;
			set;
		}
		public abstract bool				SetObjectStateIf(eOBJECT_STATE _Value, eOBJECT_STATE _compare);

		public abstract bool				Initialize(CGMSG _Msg = default(CGMSG));
		public abstract bool				Destroy();
		public abstract bool				Start(CGMSG _Msg = default(CGMSG));
		public abstract bool				Stop();
		public abstract bool				Attach(ICGObjectStateable _Child);
		public abstract int					Detach(ICGObjectStateable _Child);
		public abstract IEnumerator			GetEnumerator();


	// Implementations)
		//! @brief 계정의 '업데이트를'을 요청한다. @param _cid 대상 체크인 ID @return true 성공 @return false 실패
		public abstract	bool				ProcessCompleteCheckoutAll(IAccount _pAccount);

		//! @brief 해당 CHECKIN 정보를 강제로 Checkout시킨다. @param _cid 대상 체크인 ID @return 결과
		public abstract	bool				ProcessAccountForceCheckout(CHECKIN_ID _cid);

		// 1) Repository ID
		protected REPOSITORY_ID				m_ridRepository;

		// 2) Policy (정책)

		// 3) Statistics
		public Statistics.Repository		m_statistics;

	// Implementation)
		//protected void				RegisterRepository()					{	CGServerAccount::ManagerRepository::GetInstance()->Register(this);}
		//protected void				UnregisterRepository()					{	CGServerAccount::ManagerRepository::GetInstance()->Unregister(this);}

		protected void				RegisterRepository()					{}
		protected void				UnregisterRepository()					{}
	}

}
}
