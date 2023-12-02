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

namespace CGDK
{
namespace CGServerAccount
{
namespace Repository
{
	public class CRemote<TACCOUNT> :
		CGServerAccount.NRepository<TACCOUNT> where TACCOUNT : IAccount, new()
	{
	// Constructors) 
		public CRemote() : this(null)
		{
		}
		public CRemote(string _Name) : base(_Name)
		{
			m_componentState						 = new CObjectState();
			m_componentState.Object					 = this;
			m_componentState.NotifyOnInitializing	 = _ProcessOnInitializing;
			m_componentState.NotifyOnInitialize		 = _ProcessOnInitialize;
			m_componentState.NotifyOnDestroying		 = _ProcessOnDestroying;
			m_componentState.NotifyOnDestroy		 = _ProcessOnDestroying;

			m_componentState.NotifyOnStarting		 = _ProcessOnStarting;
			m_componentState.NotifyOnStart			 = _ProcessOnStart;
			m_componentState.NotifyOnStopping		 = _ProcessOnStopping;
			m_componentState.NotifyOnStop			 = _ProcessOnStop;
		}

	// Publics) 
		public virtual	eOBJECT_RESULT	Checkin(IRepository _pRepository, CGD.buffer _bufRequest)	{ return eOBJECT_RESULT.SUCCESS;}
		public virtual	eOBJECT_RESULT	Checkout()									{ return eOBJECT_RESULT.SUCCESS;}
		public virtual	eOBJECT_RESULT	RequestTransaction()						{ return eOBJECT_RESULT.SUCCESS;}

		public override eOBJECT_STATE	ObjectState
		{
			get
			{
				return m_componentState.ObjectState;
			}
			set
			{
				m_componentState.ObjectState = value;
			}
		}
		public override bool			SetObjectStateIf(eOBJECT_STATE _Value, eOBJECT_STATE _compare)
		{
			return m_componentState.SetObjectStateIf(_Value, _compare);
		}

		public override bool			Initialize(CGMSG _Msg)						{ return m_componentState.Initialize(_Msg);}
		public override bool			Destroy()									{ return m_componentState.Destroy();}
		public override bool			Start(CGMSG _Msg)							{ return m_componentState.Start(_Msg);}
		public bool						Start(string _strAddress)					{ return true;}
		public override bool			Stop()										{ return m_componentState.Stop();}
		public override bool			Attach(ICGObjectStateable _Child)			{ return m_componentState.Attach(_Child);}
		public override int				Detach(ICGObjectStateable _Child)			{ return m_componentState.Detach(_Child);}
		public override IEnumerator		GetEnumerator()
		{
			return m_componentState.GetEnumerator();
		}

	// Frameworks) 
		protected virtual void			OnInitializing(CGMSG _Msg)					{}
		protected virtual void			OnInitialize(CGMSG _Msg)					{}
		protected virtual void			OnDestroying()								{}
		protected virtual void			OnDestroy()									{}

		protected virtual void			OnStarting(CGMSG _Msg)						{}
		protected virtual void			OnStart(CGMSG _Msg)							{}
		protected virtual void			OnStopping()								{}
		protected virtual void			OnStop()									{}

		protected void					OnConnect(object _Source)
		{
			// Log)
			//CGLOG.INFO(null, " > REPOSITORY_REMOTE['"+Name+"']: Socket Connected ("+m_socketRepository.Socket.LocalEndPoint.ToString()+")");
			//CGLOG.INFO(null, "     RID : "+m_ridRepository.keyDepot.ToString()+"/"+m_ridRepository.keyCertify.ToString()+"' ");
			//CGLOG.INFO(null, "     Remote RID : "+m_idRepositoryRemote.keyDepot.ToString()+"/"+m_idRepositoryRemote.keyCertify.ToString()+"' ");
			
			// 1) Certify를 요청하는 메시지를 전송한다.
			m_socketRepository.Send(Message.Message_ACCOUNT_REQUEST_CERTIFY(m_ridRepository, m_idRepositoryRemote));
		}
		protected void					OnFailConnect(object _source, uint _Reason)
		{
			//// Log)
			//CGLOG.INFO_LOW(null, " < REPOSITORY_REMOTE['"+Name+"']: Fail to connect");
		}
		protected void					OnDisconnect(object _source, uint _Reason)
		{
			//// Log)
			//CGLOG.INFO(null, " < REPOSITORY_REMOTE['"+Name+"']: Socket Disconnected ("+m_socketRepository.Socket.LocalEndPoint.ToString()+")");
		}
		protected int					OnMessage(object _Source, CGMSG _Msg)
		{
				// Declare) 
			int result = 0;

			// 1) Message를 얻는다.
			UInt32 msgCommand = _Msg.Buffer.get_front<UInt32>(4);

			// 2) Message 종류에 따른 처리
			switch (msgCommand)
			{
			case	CGMESSAGE.SERVER.ACCOUNT.RESPONSE_CERTIFY:
					result = OnMessage_ACCOUNT_RESPONSE_CERTIFY(_Source, _Msg);
					break;

			case	CGMESSAGE.SERVER.ACCOUNT.COMMAND_RESPONSE:
					result = OnMessage_ACCOUNT_COMMAND_RESPONSE(_Source, _Msg);
					break;

			case	CGMESSAGE.SERVER.ACCOUNT.COMMAND_REQUEST_CHECKOUT_REMOTE:
					result = OnMessage_ACCOUNT_COMMAND_REQUEST_CHECKOUT_REMOTE(_Source, _Msg);
					break;

			case	CGMESSAGE.SERVER.ACCOUNT.COMMAND_REQUEST_UPDATE:
					result = OnMessage_ACCOUNT_COMMAND_REQUEST_UPDATE(_Source, _Msg);
					break;

			default:
					break;
			}

			// Return) 
			return	result;
		}

		protected int					OnMessage_ACCOUNT_RESPONSE_CERTIFY(object _Source, CGDK.CGMSG _Msg)
		{
			// Declare) 
			int	MESSAGE_SIZE = 8 + Marshal.SizeOf(typeof(REPOSITORY_ID));	// sizeof(REPOSITORY_ID);	// Marshal.SizeOf(REPOSITORY_ID)

			// Check) 메시지 길이를 확인한다.
			Debug.Assert(_Msg.Buffer.len == MESSAGE_SIZE);

			// Check) 메시지 길이를 확인한다.
			if(_Msg.Buffer.len != MESSAGE_SIZE)
			{
				//CGLOG.ERROR(null, "Error) invalid Message size [value:"+_Msg.Buffer.len.ToString()+", Requried:"+MESSAGE_SIZE.ToString()+"] (CGServerAccount::Repository::CRemote<TACCOUNT>::OnMessage_ACCOUNT_RESPONSE_CERTIFY)");
				throw	new Exception();    /*CGException.CNetThrow_InvalidMessageLength();*/
			}

			// 1) Repository ID를 얻는다.
			var		ridRemote = _Msg.Buffer.get_front<REPOSITORY_ID>(8);

			//// Log)
			//CGLOG.INFO(null, " > REPOSITORY_REMOTE['"+Name+"']: Remote Repository CERTIFIED!! ("+m_socketRepository.Socket.LocalEndPoint.ToString()+")");

			// 2-1) 새로 RID를 전송했을 경우 Reset처리한다.
			if (ridRemote.exist())
			{
				//// Log) 
				//CGLOG.INFO(null, "     Initialize ACCOUNT info [Remote RID : "+ridRemote.keyDepot.ToString()+"/"+ridRemote.keyCertify.ToString()+"]' ");

				// - 초기화 한다.
				ProcessResetRepository(ridRemote);
			}
			// 2-2) RID를 empty를 전송해 왔을 경우 기존 복구한다.
			else
			{
				//// Log) 
				//CGLOG.INFO(null, "     Recover ACCOUNT info : "+m_ridRepository.keyDepot.ToString()+"/"+m_ridRepository.keyCertify.ToString()+"' ");
			}

			// Return) 
			return	1;
		}
		protected int					OnMessage_ACCOUNT_COMMAND_RESPONSE(object _Source, CGDK.CGMSG _Msg)
		{
			// Check)
			if(m_queueTransactable ==null)
				return 1;

			// 1) Buffer를 얻는다. 
			CGD.buffer	bufTemp = _Msg.Buffer + 8;

			// 2) 값들을 읽어온다.
			var	eCommand = bufTemp.extract<eTRANSACTION_COMMAND>();
			var	tid		 = bufTemp.extract<TRANSACTION_ID>();
			var	eResult	 = bufTemp.extract<eOBJECT_RESULT>();

			// Declare)
			ITransactable	pTransactable	 = null;

			// 3) tid에 해당하는 처리를 수행한다.
			pTransactable	 = m_queueTransactable .CompleteTransaction(tid, eResult, bufTemp);

			if(pTransactable==null)
			{
				if(eResult==eOBJECT_RESULT.SUCCESS)
				{
					// - Command에 따른 처리
					switch(eCommand)
					{
					case	eTRANSACTION_COMMAND.CHECKIN:
							{
								CGD.buffer	bufTemp2 = _Msg.Buffer+12+sizeof(UInt64)+sizeof(eOBJECT_RESULT);
								var	cidRemote		 = bufTemp2.extract<CHECKIN_ID>();
								m_socketRepository.Send(Message.Message_ACCOUNT_COMMAND_REQUEST_CHECKOUT(tid, cidRemote));
							}
							break;

					default:
							{
							}
							break;
					}
				}
			}
				
			// Return) 
			return	1;
		}
		protected int					OnMessage_ACCOUNT_COMMAND_REQUEST_CHECKOUT_REMOTE(object _Source, CGDK.CGMSG _Msg)
		{
			// Check)
			if(m_queueTransactable==null)
				return	1;

			// 1) Buffer를 얻는다. 
			CGD.buffer	bufTemp = _Msg.Buffer + 12;

			// 2) 값들을 읽어온다.
			var	rid		 = bufTemp.extract<REQUEST_ID>();
			var	cid		 = bufTemp.extract<CHECKIN_ID>();

			try
			{
				// 3) Checkout을 요청한다.
				var	result	 = ProcessRequestCheckout(rid, cid);

				// Check) 
				if(result>eOBJECT_RESULT.PENDING)
					throw	new Exception();
			}
			catch(Exception)
			{
			}

			// Return) 
			return	1;
		}
		protected int					OnMessage_ACCOUNT_COMMAND_REQUEST_UPDATE(object _Source, CGDK.CGMSG _Msg)
		{
			// 1) Buffer를 얻는다. 
			CGD.buffer	bufTemp = _Msg.Buffer + 8;

			// 2) 값들을 읽어온다.
			var		eCommand = bufTemp.extract<eTRANSACTION_COMMAND>();
			var		cid		 = bufTemp.extract<CHECKIN_ID>();
			var		buffer	 = bufTemp;

			// 3) Accout
			TACCOUNT	pAccount;

			// 3) 찾는다.
			lock(m_csAccount)
			{
				// 1) 찾는다.
				if (m_mapAccountByCID.TryGetValue(cid, out pAccount)==false)
				{
					return	1;
				}
			}

			// 4-A) 찾았을 경우 바로 처리한다.
			if (pAccount!=null)
			{
				pAccount.ProcessAccountUpdate(buffer);
			}

			// Return) 
			return	1;
		}

	// Implementations) 
		public void						ProcessResetRepository(REPOSITORY_ID _rid)
		{
			// 1) REPOSITORY_ID를 설정한다
			m_idRepositoryRemote = _rid;

			// 2) 모든 Checkin정보 Reset!
			lock(m_csAccount)
			{
				// - Clear한다.
				m_mapAccountByAID.Clear();
				m_mapAccountByCID.Clear();
			}
		}

		protected void					_ProcessOnInitializing(object _Source, CGDK.CGMSG _Msg)
		{
			OnInitializing(_Msg);
		}
		protected void					_ProcessOnInitialize(object _Source, CGDK.CGMSG _Msg)
		{
			// 1) Socket을 생성한다.
			var	socketRepository					 = new CGDK.Network.CSocketTCPClient("");
			socketRepository.NotifyOnConnect		 = OnConnect;
			socketRepository.NotifyOnDisconnect		 = OnDisconnect;
			socketRepository.NotifyOnFailConnect 	 = OnFailConnect;
			socketRepository.NotifyOnMessage		 = OnMessage;
			socketRepository.EnableReconnection		 = true;
			socketRepository.ReconnectionInterval	 = 10000;
			m_socketRepository						 = socketRepository;

			// 2) ....
			OnInitialize(_Msg);
		}
		protected void					_ProcessOnDestroying(object _Source)
		{
			// 1) 
			OnDestroying();

			// 2) 모든 객체를 Check-out한다.
			CheckoutAll();

			// 3) REPOSITORY_ID를 Reset한다.
			m_ridRepository.reset();
		}
		protected void					_ProcessOnDestroy(object _Source)
		{
			// 1) 
			OnDestroy();
		}
		protected void					_ProcessOnStarting(object _Source, CGDK.CGMSG _Msg)
		{
			// 1) Starting
			OnStarting(_Msg);
		}
		protected void					_ProcessOnStart(object _Source, CGDK.CGMSG _Msg)
		{
			// 1) 재접속 기능을 켠다.
			//m_socketRepository.EnableReconnect();

			// 2) 재접속 시간을 설정한다.
			//m_socketRepository.SetReconnectInterval(TICK.seconds(5));

			// 3) 
			m_socketRepository.Start(_Msg);

			// 4) ...
			OnStart(_Msg);
		}

		protected virtual void			_ProcessOnStopping(object _Source)
		{
			// 1) 
			OnStopping();

			// 2) 재접속 기능을 켠다.
			//m_socketRepository.DisableReconnect();

			// 3) Stop
			m_socketRepository.Stop();
		}
		protected virtual void			_ProcessOnStop(object _Source)
		{
			// 1) 
			OnStop();
		}

		//! @brief 계정의 '체크인'을 요청한다. @param _aid 계정 ID @return true 성공 @return false 실패
		public override eOBJECT_RESULT	ProcessRequestCheckin_Login				(REQUEST_ID _ridSource, CGD.buffer _bufRequest, IAccessable _pAccessable, TRESPONSE _fCompletion)
		{
			// Statistics)
			++m_statistics.m_statisticsLogin_Request;

			// Declare)
			eOBJECT_RESULT	eResult;

			try
			{
				// 4) Request 객체 생성
				//	  Request . Transaction_RequestCheckin_Login(this, m_pAuthority);
				//	  Complete. Transaction_CompleteCheckin_Login(this, _Result, _buffer);
				var	pTransactable = new ITransactableAccount<TACCOUNT>();

				// 5) Request 값 설정
				pTransactable.Reset();
				pTransactable.m_eCommand	 = eTRANSACTION_COMMAND.QUERY_AID_CHECKIN;
				pTransactable.m_ridSource	 = _ridSource;
				pTransactable.m_rid			 = _ridSource;
				pTransactable.m_pAccessable	 = _pAccessable;
				pTransactable.m_pRepository	 = this;
				pTransactable.m_pAccount	 = null;
				pTransactable.m_bufRequest	 = _bufRequest;
				pTransactable.m_fCompletion	 = _fCompletion;

				// 5) Request를 건다.
				eResult	 = m_queueTransactable .RequestTransaction(_ridSource.tid, pTransactable);
			}
			catch (Exception)
			{
				// Statistics)
				++m_statistics.m_statisticsLogin_Fail;

				// Reraise)
				eResult	 = eOBJECT_RESULT.EXCEPTION;
			}

			// Return)
			return	eResult;
		}
		//! @brief 계정의 '체크인'을 요청한다. @param _aid 계정 ID @return true 성공 @return false 실패
		public override eOBJECT_RESULT	ProcessRequestCheckin_Create			(REQUEST_ID _ridSource, CGD.buffer _bufRequest, IAccessable _pAccessable, TRESPONSE _fCompletion)
		{
			// Statistics)
			++m_statistics.m_statisticsCreate_Request;

			// Declare)
			eOBJECT_RESULT	eResult;

			try
			{
				// 1) Request 객체 생성
				var	pTransactable = new ITransactableAccount<TACCOUNT>();

				// 2) Request 값 설정
				pTransactable.Reset();
				pTransactable.m_eCommand	 = eTRANSACTION_COMMAND.QUERY_AID_CREATE;
				pTransactable.m_ridSource	 = _ridSource;
				pTransactable.m_rid			 = _ridSource;	// 이것이 내부적으로 Key로 사용됨.
				pTransactable.m_pAccessable	 = _pAccessable;
				pTransactable.m_pRepository	 = this;
				pTransactable.m_pAccount	 = null;
				pTransactable.m_bufRequest	 = _bufRequest;
				pTransactable.m_fCompletion	 = _fCompletion;

				// 3) Request를 건다.
				eResult	 = m_queueTransactable.RequestTransaction(_ridSource.tid, pTransactable);
			}
			catch (Exception)
			{
				// Statistics)
				++m_statistics.m_statisticsCreate_Fail;

				// - Result를 eOBJECT_RESULT.EXCEPTION로 설정
				eResult	 = eOBJECT_RESULT.EXCEPTION;
			}

			// Return)
			return	eResult;
		}
		//! @brief 계정의 '체크인'을 취소한다. @param _ridSource REQUET_ID @return 처리 결과 성공일 경우 eOBJECT_RESULT.SUCCESS를 리턴하며 이 외의 경우 에러
		public override bool			ProcessCancelCheckin_Login				(REQUEST_ID _ridSource)
		{
			// 1) TRANSACTION_ID를 얻는다.
			TRANSACTION_ID	tid		 = _ridSource.tid;

			// Check) tid는 반드시 존재해줘야 된다.
			Debug.Assert(tid.exist());

			// 1) Cancel한다.
			var		pTransaction	 = m_queueTransactable.CancelTransaction(tid);

			// Return) 결과 리턴...
			return	pTransaction!=null;
		}
		//! @brief 계정의 ACCOUNT_ID로 '체크인'을 요청한다. @param _ridSource 트렌젝션 ID @param _auth 인허가 정보 @param _pAccessable 억세스블 객체 ID @return 
		public override eOBJECT_RESULT	ProcessRequestCheckin_ACCOUNT_ID		(REQUEST_ID _ridSource, CGD.buffer _bufRequest, IAccessable _pAccessable, TRESPONSE _Function)
		{
			//// Check) 
			//Debug.Assert(_bufRequest.len == (sizeof(eCERTIFICATION_TYPE) + sizeof(ACCOUNT_ID)));

			// 1) ACCOUNT_ID를 얻는다.
			CGD.buffer	buffer	 = _bufRequest;

			// 2) 값을 읽어들인다.
			var		eCertificationType	 = buffer.extract<eCERTIFICATION_TYPE>();

			// Check) BY_ACCOUNT_ID이 아니면 Exception을 던진다.
			if(eCertificationType != eCERTIFICATION_TYPE.BY_ACCOUNT_ID)
			{
				CGLOG.EXCEPTION(null, "Exception) Certification type is invalid [CertiType:"+eCertificationType+"] (CGServerAccount::Repository::CRemote<TACCOUNT>::ProcessRequestCheckin_ACCOUNT_ID)");
				throw	new Exception();    /*CGException::CThrow_InvalidArgument()*/
			}

			// 3) ACCOUNT_ID를 얻는다.
			ACCOUNT_ID	aid = buffer.extract<ACCOUNT_ID>();

			// Check) AID의 유효성을 검사한다.
			if (aid.valid() == false)
			{
				CGLOG.EXCEPTION(null, "Exception) AID is invalid (CGServerAccount::Repository::CRemote<TACCOUNT>::ProcessRequestCheckin_ACCOUNT_ID)");
				throw	new Exception(); /*, CGException.CThrow_InvalidArgument(), );*/
			}

			// Statistics)
			++m_statistics.m_statisticsCheckin_Request;

			// Declare)
			TACCOUNT			pAccount	 = null;
			eOBJECT_RESULT		eResult;

			lock(m_csAccount)
			{
				// 2) AID에 맞는 Account 객체가 이미 존재하는지 찾는다.
				if(m_mapAccountByAID.TryGetValue(aid, out pAccount)==true)
				{
					// - Checin Count를 증가시킨다. (여기서 Checkin Count를 증가시켜주지 않으면 Transactable을 거는 사이에 LogOut되어 버릴 수 있다)
					pAccount.AddCheckinCount();
				}
			}

			if(pAccount!=null)
			{
				try
				{
					// 3) Transaction 객체를 생성한다.
					var	pTransaction = new ITransactableAccount<TACCOUNT>();

					// 4) Transaction 처리를 위한 값들을 설정한다.
					pTransaction.Reset();
					pTransaction.m_eCommand		 = eTRANSACTION_COMMAND.CHECKIN_AID;
					pTransaction.m_ridSource	 = _ridSource;
					pTransaction.m_pAccessable	 = _pAccessable;
					pTransaction.m_pRepository	 = this;
					pTransaction.m_pAccount		 = pAccount;
					pTransaction.m_aid			 = aid;
					pTransaction.m_bufRequest	 = _bufRequest;
					pTransaction.m_fCompletion	 = _Function;

					// 5) Transaction을 요청한다.
					eResult	 = pAccount.m_queueTransaction.RequestTransaction(pTransaction);
				}
				catch (Exception)
				{
					// Statistics)
					++m_statistics.m_statisticsCheckin_Fail;

					// - Result를 false로Exception
					eResult = eOBJECT_RESULT.EXCEPTION;
				}

				// 5) Release한다.
				pAccount.ReleaseCheckinCount();
			}
			else
			{
				try
				{
					// - Transactable 객체를 생성한다.
					var	pTransactable		 = new ITransactableAccount<TACCOUNT>();

					// - Transactable 처리를 위한 값들을 설정한다.
					pTransactable.Reset();
					pTransactable.m_eCommand	 = eTRANSACTION_COMMAND.CHECKIN_AID_REMOTE;
					pTransactable.m_ridSource	 = _ridSource;
					pTransactable.m_pAccessable	 = _pAccessable;
					pTransactable.m_pRepository	 = this;
					pTransactable.m_aid			 = aid;
					pTransactable.m_bufRequest	 = _bufRequest;
					pTransactable.m_fCompletion	 = _Function;

					// - Remote Transactable에 Transactable을 요청한다.
					eResult = m_queueTransactable.RequestTransaction(_ridSource.tid, pTransactable);
				}
				catch (Exception)
				{
					// Statistics)
					++m_statistics.m_statisticsCheckin_Fail;

					// - Result를 eOBJECT_RESULT.EXCEPTION로 설정
					eResult	 = eOBJECT_RESULT.EXCEPTION;
				}
			}

			// Return) 
			return	eResult;
		}
		//! @brief 계정의 CHECK_ID로 '체크인'을 요청한다. @param _ridSource 트렌젝션 ID @param _auth 인허가 정보 @param _pAccessable 억세스블 객체 ID @return 
		public override	eOBJECT_RESULT	ProcessRequestCheckin_CHECKIN_ID		(REQUEST_ID _ridSource, CGD.buffer _bufRequest, IAccessable _pAccessable, TRESPONSE _fCompletion)
		{
			//// Check)
			//Debug.Assert(_bufRequest.len == (sizeof(eCERTIFICATION_TYPE) + sizeof(CHECKIN_ID)));

			// 1) Buffer를 임시로 생성..
			CGD.buffer	buffer	 = _bufRequest;

			// 2) 값을 읽어들인다.
			var		eCertificationType = buffer.extract<eCERTIFICATION_TYPE>();

			// Check) 
			if(eCertificationType != eCERTIFICATION_TYPE.BY_CHEKIN_ID)
			{
				//CGLOG.EXCEPTION(null, "Exception) eCertificationType is invalid [Source:"+_Auth.eCertificationType.ToString()+", Compare:"+eCERTIFICATION_TYPE.BY_CHEKIN_ID.ToString()+"] (CGServerAccount::Repository::CRemote<TACCOUNT>::ProcessRequestCheckin_CHECKIN_ID)");
				throw	new Exception();	/*CGException::CThrow_InvalidArgument()*/
			}

			// 3) cid를 얻는다.
			var		cid = buffer.extract<CHECKIN_ID>();

			// 4) CID의 유효성을 검사한다.
			if(cid.valid() == false)
			{
				//CGLOG.EXCEPTION(null, "Exception) CID is invalid (CGServerAccount::Repository::CRemote<TACCOUNT>::ProcessRequestCheckin_CHECKIN_ID)");
				throw	new Exception();    /*CGException::CThrow_InvalidArgument()*/
			}

			// Statistics)
			++m_statistics.m_statisticsCheckin_Request;

			// Declare)
			TACCOUNT		pAccount	 = default(TACCOUNT);
			eOBJECT_RESULT	eResult		 = eOBJECT_RESULT.EXCEPTION;

			// 3) 찾는다.
			lock(m_csAccount)
			{
				// 1) 찾는다.
				if (m_mapAccountByCID.TryGetValue(cid, out pAccount)==true)
				{
					// - pAccount를 설정

					// - Reference Count를 증가시킨다.
					pAccount.AddCheckinCount();
				}
			}

			// 4-A) 찾았을 경우 바로 처리한다.
			if (pAccount!=null)
			{
				try
				{
					// 4) Transactable 객체를 생성한다.
					var	pTransactable		 = new ITransactableAccount<TACCOUNT>();

					// 5) Transactable 처리를 위한 값들을 설정한다.
					pTransactable.Reset();
					pTransactable.m_eCommand	 = eTRANSACTION_COMMAND.CHECKIN_CID;
					pTransactable.m_ridSource	 = _ridSource;
					pTransactable.m_pAccessable	 = _pAccessable;
					pTransactable.m_pRepository	 = this;
					pTransactable.m_pAccount	 = pAccount;
					pTransactable.m_cid			 = cid;
					pTransactable.m_fRequest	 = null;
					pTransactable.m_fCompletion	 = _fCompletion;

					// 6) Transactable을 요청한다.
					eResult = pAccount.RequestTransaction(pTransactable);
				}
				catch (Exception)
				{
					// Statistics)
					++m_statistics.m_statisticsCheckin_Fail;

					// - Result를 eOBJECT_RESULT.EXCEPTION로 설정
					eResult	 = eOBJECT_RESULT.EXCEPTION;

					// - Account의 Checkin Count를 1 줄인다.
					lock(m_csAccount)
					{
						pAccount.ReleaseCheckinCount();
					}
				}
			}
			// 4-B) 찾지 못했을 경우 Remote Checkin을 요청한다.
			else
			{
				try
				{
					// 4) Transactable 객체를 생성한다.
					var	pTransactable		 = new ITransactableAccount<TACCOUNT>();

					// 5) Transactable 처리를 위한 값들을 설정한다.
					pTransactable.Reset();
					pTransactable.m_eCommand	 = eTRANSACTION_COMMAND.CHECKIN_LOGIN;
					pTransactable.m_ridSource	 = _ridSource;
					pTransactable.m_pAccessable	 = _pAccessable;
					pTransactable.m_pRepository	 = this;
					pTransactable.m_cid			 = cid;
					pTransactable.m_fRequest	 = null;
					pTransactable.m_fCompletion	 = _fCompletion;

					// 6) Remote Transactable에 Transactable을 요청한다.
					eResult	 = m_queueTransactable.RequestTransaction(_ridSource.tid, pTransactable);
				}
				catch (Exception)
				{
					// Statistics)
					++m_statistics.m_statisticsCheckin_Fail;

					// - Result를 eOBJECT_RESULT.EXCEPTION로 설정
					eResult	 = eOBJECT_RESULT.EXCEPTION;
				}
			}

			// Return) 
			return	eResult;
		}
		//! @brief 계정의 '체크아웃'을 요청을 처리한다. @param _cid 대상 체크인한 ID @return true 성공 @return 결과eOBJECT_RESUTL::SUCCESS의 경우 성공
		public virtual	eOBJECT_RESULT	ProcessRequestCheckout					(REQUEST_ID _ridSource, CHECKIN_ID _cid)
		{
			// Declare) 
			IAccount	pAccount	 = null;

			// 1) 해당 Account를 찾는다.
			pAccount	 = FindAccount(_cid);

			// Check)
			if(pAccount==null)
				return	eOBJECT_RESULT.NOT_EXIST;

			// 2) CID로 Accessable을 찾는다.
			IAccessable	pAccessable	 = pAccount.FindAccessable(_cid);

			// Check)
			if(pAccessable==null)
			{
				return	eOBJECT_RESULT.NOT_EXIST;
			}

			// 3) Checkout을 요청한다.
			return	pAccessable.ProcessAccountCheckout(_ridSource, _cid, pAccount, null);
		}
		public override	eOBJECT_RESULT	ProcessRequestCheckin_CreateProcess		(ITransactableAccount<TACCOUNT> _pTransactable)
		{
			// Check)
			Debug.Assert(false);

			// Return) 정의되지 않았다.
			return	eOBJECT_RESULT.EXCEPTION;
		}

	// Implementations) 
		public override eOBJECT_RESULT	Transactable_Request					(ITransactableAccount<TACCOUNT> _pTransactable)
		{
			// Declare)
			eOBJECT_RESULT result = eOBJECT_RESULT.NOT_DEFINED;

			switch (_pTransactable.m_eCommand)
			{
			// CTransactableAccountCheckin_ACCOUNT_ID
			case	eTRANSACTION_COMMAND.CHECKIN_AID:
					result = Transactable_RequestCheckin_ACCOUNT_ID(_pTransactable);
					break;

			case	eTRANSACTION_COMMAND.CHECKIN_AID_REMOTE:
					result = Transactable_RequestCheckin_ACCOUNT_ID_Remote(_pTransactable);
					break;

			// CTransactableAccountCheckin_CHECKIN_ID
			case	eTRANSACTION_COMMAND.CHECKIN_CID:
					result = Transactable_RequestCheckin_CHECKIN_ID(_pTransactable);
					break;

			case	eTRANSACTION_COMMAND.CHECKIN_CID_REMOTE:
					result = Transactable_RequestCheckin_CHECKIN_ID_Remote(_pTransactable);
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
		public override	bool			Transactable_Complete					(ITransactableAccount<TACCOUNT> _pTransactable)
		{
			// Declare)
			bool result = false;

			switch (_pTransactable.m_eCommand)
			{
			// CTransactableAccountCheckin_ACCOUNT_ID
			case	eTRANSACTION_COMMAND.CHECKIN_AID:
					result = Transactable_CompleteCheckin_ACCOUNT_ID(_pTransactable);
					break;

			case	eTRANSACTION_COMMAND.CHECKIN_AID_REMOTE:
					result = Transactable_CompleteCheckin_ACCOUNT_ID_Remote(_pTransactable);
					break;

			// CTransactableAccountCheckin_CHECKIN_ID
			case	eTRANSACTION_COMMAND.CHECKIN_CID:
					result = Transactable_CompleteCheckin_CHECKIN_ID(_pTransactable);
					break;

			case	eTRANSACTION_COMMAND.CHECKIN_CID_REMOTE:
					result = Transactable_CompleteCheckin_CHECKIN_ID_Remote(_pTransactable);
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

		public override eOBJECT_RESULT	Transactable_RequestCheckin_QueryAID	(ITransactableAccount<TACCOUNT> _pTransactable)
		{
			// Declare)
			eOBJECT_RESULT	eResult;
			bool			bResult;

			// 1) Send한다.
			bResult = m_socketRepository.Send(Message.Message_ACCOUNT_COMMAND_REQUEST_CHECKIN(_pTransactable.m_rid.tid, _pTransactable.m_bufRequest));

			// 2-1) 전송에 성공했을 경우
			if(bResult)
			{
				eResult	 = eOBJECT_RESULT.PENDING;
			}
			// 2-2) 전송에  실패했을 경우...
			else
			{
				// - Result를 Fail로 설정한다.
				eResult	 = eOBJECT_RESULT.FAIL;

				// - 결과를 설정한다.
				_pTransactable.SetResult(eResult);
			}

			// Return) 
			return	eResult;
		}
		public override bool			Transactable_CompleteCheckin_QueryAID	(ITransactableAccount<TACCOUNT> _pTransactable)
		{
			// Check)
			Debug.Assert(_pTransactable != null);

			// 1) 임시로 복사
			IAccessable		pAccessable	 = _pTransactable.m_pAccessable;

			// Declare) 
			CHECKIN_ID		cidRemote;
			ACCOUNT_ID		aid	 = new ACCOUNT_ID();
			UInt64			iRevision;
			IAccount		pAccountOld	 = null;

			// Declare)
			bool			bResult		 = false;

			// 2) eResult값을 임시로 복사
			eOBJECT_RESULT	eResult		 = _pTransactable.m_eResult;

			// 3) _fCompletion함수를 호출한다.
			if (_pTransactable.m_fCompletion!=null)
			{
				eResult	 = _pTransactable.m_fCompletion(_pTransactable, eResult, _pTransactable.m_bufResult);
			}

			if (eResult == eOBJECT_RESULT.SUCCESS)
			{
				// 4) 버퍼포인터를 임시로 복사한다.
				var		bufTemp = _pTransactable.m_bufResult;

				// Declare) 
				TACCOUNT	pAccount	 = default(TACCOUNT);

				try
				{
					// Check) Accessable에서 이미 취소했으면 여기서 Exception을 던진다.
					if(_pTransactable.m_pAccessable==null)
						throw	new Exception();

					// Check) Cancel된 Request인가를 확인한다.

					// 5) Remote CHECKIN_ID를 얻는다.
					cidRemote	 = bufTemp.extract<CHECKIN_ID>();

					// Declare) 
					var	bufPassing	 = bufTemp;

					// Case) 일반적인 Login의 경우
					if(cidRemote.exist())
					{
						// 6) AID와 Revision을 읽어들인다.
						aid			 = bufTemp.extract<ACCOUNT_ID>();
						iRevision	 = bufTemp.extract<UInt64>();

						// 7) Parameter러 넘길 Buf를 갱신한다.
						bufPassing	 = bufTemp;

						// 8) Account 객체 생성
						pAccount	 = ProcessAllocAccountObject(new ACCOUNT_ID());

						// 9) Alloc를 설정한다.
						pAccount.ProcessAccountAlloc();

						// 10) 새로운 ACCOUNT_ID를 생성한다. Remote cid를 설정한다.
						pAccount.m_cid			 = cidRemote;
						pAccount.m_aid			 = aid;
						pAccount.m_iRevision	 = iRevision;

						// Statistics)
						++m_statistics.m_statisticsLogin;

						try
						{
							lock(m_csAccount)
							{
								// 11) 
								pAccount.AddCheckinCount();

								// 12) AID에 추가한다.
								m_mapAccountByAID.Add(aid, pAccount);

								// Check) 추가에 실패했을 경우 (이미 존재하고 있는 경우) - 의문: 추가하지 않고 기존의 ACCCOUNT_ID를 사용해야 하는가... 아니면 Exception을 던질 것인가...

								// 13) 새로운 ACCOUNT_ID를 생성한다.
								pAccount.RequestAccountSerializeIn(bufTemp);

								// Check) CID가 제대로 되었는 지 확인한다.
								Debug.Assert(cidRemote.valid()==true);

								// 14) 새로 할당한 추가한다.
								m_mapAccountByCID.Add(cidRemote, pAccount);

								// Check) 추가하지 못했다면 Exception을 던진다.

								try
								{
									// 15-Pre)
									ACCOUNT_ITEM	item = new ACCOUNT_ITEM();
									item.cid			 = cidRemote;
									item.pAccessable	 = pAccessable;

									// 15) CID를 Account에 등록한다.
									pAccountOld	 = pAccount.ProcessAddCID(_pTransactable.m_ridSource, item);

									try
									{
										// 16) Process Complete Checkin을 수행한다.
										pAccessable.ProcessAccountCompleteCheckin(_pTransactable.m_ridSource, cidRemote, eResult, _pTransactable.m_bufResult);
									}
									catch(Exception)
									{
										// - CID를 Remove한다.
										pAccount.ProcessRemoveCID(cidRemote);

										// Reraise) 
										throw;
									}
								}
								catch(Exception)
								{
									// - Account에 등록실패했으면 Checkin을 취소한다.
									m_mapAccountByCID.Remove(cidRemote);

									// Reraise) 
									throw;
								}
							}

							// Statistics)
							++m_statistics.m_statisticsCheckin;

							// Statistics)
							++m_statistics.m_statisticsCheckin_Success;
							++m_statistics.m_statisticsLogin_Success;

							// 17) Success
							bResult	 = true;
						}
						catch(Exception)
						{
							// - 다시 뺀다.
							lock(m_csAccount)
							{
								m_mapAccountByAID.Remove(aid);
							}

							// - Account의 Checkin Count를 줄인다.
							pAccount.ReleaseCheckinCount();

							// Reraise)
							throw;
						}
					}
					// Case) Create의 경우
					else
					{
						try
						{
							// 6) Process Complete Checkin을 수행한다.
							pAccessable.ProcessAccountCompleteCheckin(_pTransactable.m_ridSource, cidRemote, eResult, bufPassing);
						}
						catch(Exception)
						{
							// - CID를 Remove한다.
							pAccount.ProcessRemoveCID(cidRemote);

							// Reraise) 
							throw;
						}

						// Statistics)
						++m_statistics.m_statisticsCreate_Success;
					}
				}
				catch (Exception)
				{
					// - Result를 fail로 ...
					eResult	 = eOBJECT_RESULT.EXCEPTION;

					// - Accessable의 ProcessCompleteCheckin을 호출한다.
					if (_pTransactable.m_pAccessable != null)
					{
						_pTransactable.m_pAccessable.ProcessAccountCompleteCheckin(_pTransactable.m_ridSource, _pTransactable.m_cid, eResult, _pTransactable.m_bufResult);
					}
				}

				// 8) AccountOld가 있으면 Checkin Count를 감소시킨다..
				if(pAccountOld!=null)
				{
					pAccountOld.ReleaseCheckinCount();
				}
			}
			else
			{
				// Statistics)
				++m_statistics.m_statisticsLogin_Fail;

				if (_pTransactable.m_pAccessable != null)
				{
					// - Accesssable을 확인한다.
					_pTransactable.m_pAccessable.ProcessAccountCompleteCheckin(_pTransactable.m_ridSource, new CHECKIN_ID(), eResult, _pTransactable.m_bufResult);
				}

				// - Cancel일 경우 Cancel 메시지를 전송한다.
				if(eResult==eOBJECT_RESULT.CANCEL)
				{
					m_socketRepository.Send(Message.Message_ACCOUNT_COMMAND_CANCEL(_pTransactable.m_rid.tid, eTRANSACTION_COMMAND.CHECKOUT, new CHECKIN_ID()));
				}
			}

			// Return) 
			return	bResult;
		}

		public virtual eOBJECT_RESULT	Transactable_RequestCheckin_ACCOUNT_ID_Remote(ITransactableAccount<TACCOUNT> _pTransactable)
		{
			// Declare)
			eOBJECT_RESULT	eResult;
			bool			bResult;

			// 1) Send한다.
			bResult = m_socketRepository.Send(Message.Message_ACCOUNT_COMMAND_REQUEST_CHECKIN(_pTransactable.m_rid.tid, _pTransactable.m_bufRequest));

			// 2-1) 전송에 성공했을 경우
			if(bResult)
			{
				eResult	 = eOBJECT_RESULT.PENDING;
			}
			// 2-2) 전송에  실패했을 경우...
			else
			{
				// - Result를 Fail로 설정한다.
				eResult	 = eOBJECT_RESULT.FAIL;

				// - 결과를 설정한다.
				_pTransactable.SetResult(eResult);
			}

			// Return) 
			return	eResult;
		}
		public virtual bool				Transactable_CompleteCheckin_ACCOUNT_ID_Remote(ITransactableAccount<TACCOUNT> _pTransactable)
		{
			// Declare)
			bool			bResult		 = false;

			// Declare) 
			eOBJECT_RESULT	eResult		 = _pTransactable.m_eResult;

			// Declare) 
			TACCOUNT		pAccount	 = default(TACCOUNT);

			if (eResult == eOBJECT_RESULT.SUCCESS)
			{
				// 1) Checkin ID를 얻어낸다.
				CGD.buffer	bufTemp		 = _pTransactable.m_bufResult;

				// 2) CHECKIN_ID를 얻어낸다.
				var			cidRemote	 = bufTemp.extract<CHECKIN_ID>();

				// Declare)
				var			aid			 = _pTransactable.m_aid;
				var			aidRemote	 = bufTemp.extract<ACCOUNT_ID>();
				var			iRevision	 = bufTemp.extract<UInt64>();

				// Check)
				Debug.Assert(aid==aidRemote);

				try
				{
					lock(m_csAccount)
					{
						// 3) Checin Count를 증가시킨다. (여기서 Checkin Count를 증가시켜주지 않으면 Transactable을 거는 사이에 LogOut되어 버릴 수 있다)
						pAccount.AddCheckinCount();

						// 4) 기존에 이미 존재하는지 확인한다.
						var	result	= m_mapAccountByAID.TryGetValue(aid, out pAccount);

						// 4-1) 이미 존재하면 그걸 사용한다.
						if (result == true)
						{
							// Check) ACCOUNT_ID가 같아야 한다.
							Debug.Assert(pAccount.AccountID == aid);

							// Check) 만약 Account ID가 다르다면 심각한 오류다!
							if(pAccount.AccountID != aid)
							{
								//CGLOG.INFO_LOW(null, "Info) AID is different [Source:"+pAccount.GetAccountID().ToString()+", This:"+aid.keyAccount.ToString()+"] (CGServerAccount::Repository::CRemote<TACCOUNT>::Transactable_CompleteCheckin_ACCOUNT_ID_Remote)");
								throw	new Exception(); /*, CGException.CThrow_Error(), );*/
							}
						}
						// 4-2) 존재하지 않으면 신규로 생성한다.
						else
						{
							// - Account 객체를 신규로 생성한다.
							pAccount = ProcessAllocAccountObject(aidRemote);

							// - Alloc를 설정한다.
							pAccount.ProcessAccountAlloc();

							// - pAccount Serialize-in
							pAccount.RequestAccountSerializeIn(bufTemp);
						}

						// 5) cidRemote/iRevisionRemote로 값을 설정한다.
						pAccount.m_cid			 = cidRemote;
						pAccount.m_iRevision	 = iRevision;
					}

					// 2) 그 다음은 기존 Process Checkin과 동일Exception
					// 4) Transaction 객체를 생성한다.
					var	pTransactable = new ITransactableAccount<TACCOUNT>();

					// 5) Transaction 처리를 위한 값들을 설정한다.
					pTransactable.Reset();
					pTransactable.m_eCommand	 = eTRANSACTION_COMMAND.CHECKIN_AID;
					pTransactable.m_ridSource	 = _pTransactable.m_ridSource;
					pTransactable.m_pAccessable	 = _pTransactable.m_pAccessable;
					pTransactable.m_pRepository	 = this;
					pTransactable.m_pAccount	 = pAccount;
					pTransactable.m_aid			 = aid;

					// 6) Transaction을 요청한다.
					eResult	 = pAccount.m_queueTransaction.RequestTransaction(pTransactable);

					// Statistics)
					++m_statistics.m_statisticsCheckin_Success;

					// 7) Success
					bResult	 = true;
				}
				catch (Exception)
				{
					// Statistics)
					++m_statistics.m_statisticsCheckin_Fail;

					// - Result를 false로Exception
					eResult = eOBJECT_RESULT.EXCEPTION;

					// - Reference Count를 줄인다.
					pAccount.ReleaseCheckinCount();
				}
			}
			else
			{
				// Statistics)
				++m_statistics.m_statisticsCheckin_Fail;
			}

			// Return) 
			return	bResult;
		}

		public virtual eOBJECT_RESULT	Transactable_RequestCheckin_CHECKIN_ID_Remote(ITransactableAccount<TACCOUNT> _pTransactable)
		{
			// Declare)
			eOBJECT_RESULT	eResult;
			bool			bResult;

			// 1) Send한다.
			bResult = m_socketRepository.Send(Message.Message_ACCOUNT_COMMAND_REQUEST_CHECKIN(_pTransactable.m_rid.tid, _pTransactable.m_bufRequest));

			// 2-1) 전송에 성공했을 경우
			if(bResult)
			{
				eResult	 = eOBJECT_RESULT.PENDING;
			}
			// 2-2) 전송에  실패했을 경우...
			else
			{
				// - Result를 Fail로 설정한다.
				eResult	 = eOBJECT_RESULT.FAIL;

				// - 결과를 설정한다.
				_pTransactable.SetResult(eResult);
			}

			// Return) 
			return	eResult;
		}
		public virtual bool				Transactable_CompleteCheckin_CHECKIN_ID_Remote(ITransactableAccount<TACCOUNT> _pTransactable)
		{
			// Check)
			Debug.Assert(_pTransactable != null);

			// Declare) 
			eOBJECT_RESULT	eResult	 = _pTransactable.m_eResult;

			// 1) _fCompletion함수를 호출한다.
			if (_pTransactable.m_fCompletion!=null)
			{
				eResult	 = _pTransactable.m_fCompletion(_pTransactable, eResult, _pTransactable.m_bufResult);
			}

			// 1) 임시로 복사
			IAccessable		pAccessable	 = _pTransactable.m_pAccessable;

			// 2) Buffer...
			CGD.buffer		bufTemp		 = _pTransactable.m_bufResult;

			// 3) AID를 얻는다.
			var				iCommand	 = bufTemp.extract<UInt32>();
			CHECKIN_ID		cidRemote	 = bufTemp.extract<CHECKIN_ID>();

			// Declare) 
			TACCOUNT		pAccount	 = default(TACCOUNT);

			try
			{
				// 4) 새로운 Account 객체를 생성한다.
				if (iCommand != 0)
				{
					// - Account 정보를 생성한다.
					pAccount = ProcessAllocAccountObject(new ACCOUNT_ID());

					// - Alloc를 설정한다.
					pAccount.ProcessAccountAlloc();

					// - Account정보를 설정한다.
					pAccount.RequestAccountSerializeIn(bufTemp);

					// - ACCOUNT ID를 얻는다.
					ACCOUNT_ID	aid = pAccount.AccountID;

					lock(m_csAccount)
					{
						// - 추가한다.
						m_mapAccountByAID.Add(aid, pAccount);

						// Check) 존재하면 여기서 그냥 끝낸다. (m_mapAccountByAID.Add 처리중 중복된 키값이면 Exception을 던진다.)

						// - Checkin Count를 증가시킨다. (여기서 Checkin Count를 증가시켜주지 않으면 Transactable을 거는 사이에 LogOut되어 버릴 수 있다)
						pAccount.AddCheckinCount();
					}
				}
				else
				{
					// - ACCOUNT ID를 얻는다.
					var	aid = bufTemp.extract<ACCOUNT_ID>();

					lock(m_csAccount)
					{
						// - 추가한다.
						var	bResult	 = m_mapAccountByAID.TryGetValue(aid, out pAccount);

						// Check) 없다면 잘못된 것이므로 Exception을 던진다.
						if(bResult==false)
						{
							//CGLOG.INFO_LOW(null, "Info) AID is not exist [AID: "+aid.keyAccount.ToString()+"] (CGServerAccount::Repository::CRemote<TACCOUNT>::Transactable_CompleteCheckin_CHECKIN_ID_Remote)");
							throw	new Exception();
						}

						// - 찾은 Account 객체 설정

						// - Checin Count를 증가시킨다. (여기서 Checkin Count를 증가시켜주지 않으면 Transactable을 거는 사이에 LogOut되어 버릴 수 있다)
						pAccount.AddCheckinCount();
					}
				}

				// 5) 그 다음은 기존 Process Checkin과 동일...
				try
				{
					// 1) Process Complete Checkin을 호출
					pAccessable.ProcessAccountCompleteCheckin(_pTransactable.m_ridSource, cidRemote, eResult, _pTransactable.m_bufResult);
				}
				catch (Exception)
				{
					// - Rollback
					lock(m_csAccount)
					{
						pAccount.ReleaseCheckinCount();
					}

					// Reraise)
					throw;
				}
			}
			catch (Exception)
			{
				// - Result를 false로...
				eResult = eOBJECT_RESULT.EXCEPTION;
			}

			if (eResult == eOBJECT_RESULT.SUCCESS)
			{
				// Statistics) 
				++m_statistics.m_statisticsCheckin_Success;
			}
			else
			{
				// Statistics) 
				++m_statistics.m_statisticsCheckin_Fail;
			}

			// Return) 
			return	true;
		}

		public override eOBJECT_RESULT	Transactable_RequestLogout				(ITransactableAccount<TACCOUNT> _pTransactable)
		{
			// Check)
			Debug.Assert(_pTransactable != null);

			// 1) 임시로 복사
			IAccount		pAccount = _pTransactable.m_pAccount;
			REQUEST_ID		rid = _pTransactable.m_rid;
			ACCOUNT_ID		aid;
			CHECKIN_ID		cidRemote;

			lock(m_csAccount)
			{
				// 2) Account ID를 얻는다.
				aid			 = pAccount.AccountID;
				cidRemote	 = pAccount.m_cid;

				// 3) 삭제
				m_mapAccountByAID.Remove(aid);
			}

			// 4) Log Out 요청 메시지를 전송한다.
			var	bResult = m_socketRepository.Send(Message.Message_ACCOUNT_COMMAND_REQUEST_CHECKOUT(rid.tid, cidRemote));

			// 5) 전송에 성공했을 경우 eOBJECT_RESULT.SUCCESS 아니면 eOBJECT_RESULT.FAIL
			var	eResult	 = (bResult) ? eOBJECT_RESULT.SUCCESS : eOBJECT_RESULT.FAIL;

			// 6) Result를 설정한다.
			_pTransactable.SetResult(eResult);

			// Return) 완료
			return eResult;
		}
		public override bool			Transactable_CompleteLogout				(ITransactableAccount<TACCOUNT> _pTransactable)
		{
			// Check)
			Debug.Assert(_pTransactable.m_pAccount !=null);

			// 1) pAccount를 임시로 복사해놓는다.
			var	pAccount	 = _pTransactable.m_pAccount;

			// 2) Account의 ProcessFree함수를 호출한다.
			pAccount.ProcessAccountFree();

			// Statistics) 
			++m_statistics.m_statisticsLogout_Complete;

			// Statistics)
			--m_statistics.m_statisticsLogin;

			// Return) 성공을 Return한다.
			return	true;
		}

		public override eOBJECT_RESULT	Transactable_RequestTransaction			(ITransactableAccount<TACCOUNT> _pTransactable)
		{
			// Check) _pTransactable이 null이면 안됀다.
			Debug.Assert(_pTransactable != null);

			// Check) 현재 상태가 RUNNING이어야 한다.
			if(ObjectState<eOBJECT_STATE.RUNNING)
				return	eOBJECT_RESULT.NOT_READY;

			// Declare)
			var	eResult	 = eOBJECT_RESULT.EXCEPTION;

			try
			{
				// 1) Request 객체 생성
				var pTransactable = new CTransactableAccountTransactionProxy();

				// 2) Request 객체에 값 설정
				pTransactable.Reset();
				pTransactable.m_eCommand		 = eTRANSACTION_COMMAND.TRANSACTION_PROXY;
				pTransactable.m_ridSource		 = _pTransactable.m_ridSource;
				pTransactable.m_rid				 = _pTransactable.m_rid;
				pTransactable.m_aid				 = _pTransactable.m_aid;
				pTransactable.m_cid				 = _pTransactable.m_cid;
				pTransactable.m_bufRequest		 = _pTransactable.m_bufRequest;
				pTransactable.m_bufResult		 = _pTransactable.m_bufResult;
				pTransactable.m_pAccount		 = _pTransactable.m_pAccount;
				pTransactable.m_pAccessable		 = _pTransactable.m_pAccessable;
				pTransactable.m_pRepository		 = this;
				pTransactable.m_pTransactable	 = _pTransactable;

				// 3) Request를 건다.
				eResult	 = m_queueTransactable.RequestTransaction(_pTransactable.m_ridSource.tid, pTransactable);
			}
			catch (Exception)
			{
				// - Result를 Exception으로...
				eResult	 = eOBJECT_RESULT.EXCEPTION;
			}

			// 4) Pending아 아니면 Result를 설정한다.
			if(eResult!= eOBJECT_RESULT.PENDING)
			{
				_pTransactable.SetResult(eResult);
			}

			// Return)
			return	eResult;
		}
		public override bool			Transactable_CompleteTransaction		(ITransactableAccount<TACCOUNT> _pTransactable)
		{
			// Check)
			Debug.Assert(_pTransactable != null);

			// Declare)
			bool	bResult	 = false;

			// Declare)
			var		 eResult = _pTransactable.m_eResult;

			try
			{
				// 1) _Function함수를 호출한다.
				if(_pTransactable.m_fCompletion!=null)
				{
					eResult	 = _pTransactable.m_fCompletion(_pTransactable, eResult, _pTransactable.m_bufResult);
				}

				// 2) 임시로 복사
				IAccessable		pAccessable = _pTransactable.m_pAccessable;

				// 3) Commit...
				if (pAccessable != null)
				{
					pAccessable.ProcessAccountCompleteTransaction(_pTransactable.m_ridSource, _pTransactable.m_cid, eResult, _pTransactable.m_bufResult);
				}
			}
			catch (Exception)
			{
				// - ...
				eResult = eOBJECT_RESULT.EXCEPTION;
			}

			// Statistics) 
			if (eResult == eOBJECT_RESULT.SUCCESS)
			{
				++m_statistics.m_statisticsTransaction_Success;
			}
			else
			{
				++m_statistics.m_statisticsTransaction_Fail;
			}

			// Return) 
			return	bResult;
		}

		public class CTransactableAccountTransactionProxy : ITransactableAccount<TACCOUNT> /*, public NCGPoolable<CTransactionAccountCommit>*/
		{
			public	CTransactableAccountTransactionProxy() : base(eTRANSACTION_COMMAND.TRANSACTION_PROXY)
			{
			}

			public override	void				Reset()	{	base.Reset(); m_pTransactable=null;}

			public  override eOBJECT_RESULT		ProcessTransactionRequest()
			{
				// 1) Repository
				var	pRepository	 = m_pRepository as CRemote<TACCOUNT>;

				// Check) pRepository가 nullptr이면 안됀다.
				Debug.Assert(pRepository!=null);
		
				// 2) Send한다.
				var bResult = pRepository.m_socketRepository.Send(Message.Message_ACCOUNT_COMMAND_REQUEST_TRANSACTION(m_ridSource.tid, m_aid, m_cid, m_pTransactable.m_bufRequest));

				// Check)
				if(bResult == false)
					return eOBJECT_RESULT.FAIL;

				// Return) 성공~
				return eOBJECT_RESULT.PENDING;
			}
			public  override bool				ProcessTransactionComplete()
			{
				// Check) m_pAccount가 nullptr이면 안됀다.
				Debug.Assert(m_pAccount!=null);

				// Declare) 
				bool	bResult	 = false;

				// 1) Result를 설정한다.
				m_pTransactable.SetResult(m_eResult, m_bufResult);

				// 2) 찾아서 Completion처리를 한다.
				bResult	 = m_pAccount.m_queueTransaction.CompleteTransaction(m_pTransactable);

				// Return)
				return	bResult;
			}

			public ITransactable			m_pTransactable	 = null;
		};

	// 1) Remote Repository의 Repository ID
		protected REPOSITORY_ID				m_idRepositoryRemote;
		protected CGTransaction.CTransactorConcurrent	m_queueTransactable	 = new CGTransaction.CTransactorConcurrent();

	// 3) Transaction...
		private CObjectState				m_componentState;
		protected Network.CSocketTCPClient	m_socketRepository;
	}
}
}
}