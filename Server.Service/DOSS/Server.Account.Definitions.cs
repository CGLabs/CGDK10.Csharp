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
using CGDK;
using CGDK.PoolClasses;
using System.Runtime.InteropServices;

namespace CGDK
{
	//-----------------------------------------------------------------------------
	// Class Definition
	//-----------------------------------------------------------------------------
	public enum eOS_TYPE : UInt32
	{
		NOT_DEFINED			 = 0,
		IOS					 = 1,
		ANDROID				 = 2,
		WINDOWS				 = 3,
		LINUX				 = 4,
	};

	public enum eLANGUAGE : UInt32
	{
		// 1) 한국어
		KOREAN,							// 한국어

		// 2) 아시아
		MANDARIN,						// 북경어(만다린)
		CANTONESE,						// 광동어(칸토니즈)
		JANANESE,						// 일본어
		TAPAI,							// 태국어

		// 3) 유럽
		ENGLISH,						// 영어
		SPAINESE,						// 스페인어
		PORTUGUESE,						// 포르투칼어
		FRANCE,							// 프랑스어
		DEUTSCHESE,						// 독일어
		DUTCH,							// 네델란드어
		RUSSIAN,						// 러시아어
	};

	public enum eCERTIFICATION_TYPE : UInt32
	{
		NOT_DEFINED			 = 0x00,		// 
		BY_ACCOUNT_ID		 = 0x01,		// New check-in and Add Lock
		BY_CHEKIN_ID		 = 0x02,		// Transfer
		LOGIN_BY_DEVICEID	 = 0x03,		// Login by Device ID
		LOGIN_BY_IDPASSWORD	 = 0x04,		// Login by ID/Passworkd
		CREATE_BY_IDPASSWORD = 0x05,		// Create by Device ID
		CREATE_BY_DEVICEID	 = 0x06,		// Create by ID/Passworkd
		RECONNECTION		 = 0x07,		// Recheckin
	};

	public enum eLOGIN_OPTION : UInt32
	{
		ALEADY_EXIST_DISPLACEMENT	 = 0,
		ALEADY_EXIST_REJECT,
		ALEADY_EXIST_CHECKIN,
	};

	public enum eTRANSACTION_COMMAND : UInt32
	{
		NONE					 = 0,

		QUERY_AID_CHECKIN		 = 1,
		QUERY_AID_CREATE		 = 2,
		CHECKIN					 = 3,
		CHECKIN_LOGIN			 = 4,
		CHECKIN_AID				 = 5,
		CHECKIN_CID				 = 6,
		TRANSACTION				 = 7,
		UPDATE					 = 8,
		CHECKIN_CUSTOM			 = 9,
		CHECKOUT				 = 10,
		CHECKOUT_ALL			 = 11,
		NEW_ACCOUNT				 = 12,
		DELETE_ACCOUNT			 = 13,
		CREATE					 = 14,
		CHECKIN_AID_REMOTE		 = 15,
		CHECKIN_CID_REMOTE		 = 16,
		TRANSACTION_PROXY		 = 17,
	}


	// ----------------------------------------------------------------------------
	// 2) Repository ID
	// ----------------------------------------------------------------------------
	[StructLayout(LayoutKind.Sequential, Pack =1)]
	public struct REPOSITORY_ID
	{
		public Int64					keyDepot;
		public Int64					keyCertify;

		// Statics) 
		public static Int64				TIndex	 = 1;
		public static Random			TRandom	 = new Random();

		public bool						empty() 											{ return keyDepot==-1;}
		public bool						exist()												{ return keyDepot != -1; }
		public void						reset(Int64 _keyDepot=-1)							{ keyDepot=_keyDepot; keyCertify=0; }
		public bool						valid()												{ bool bValid=true; bValid=exist(); return bValid;}
		public void						generate()											{ keyDepot=TIndex++; keyCertify=(TRandom.Next())<<32 | TRandom.Next();}

		public static bool				operator==(REPOSITORY_ID _lhs, REPOSITORY_ID _rhs) 	{ return _lhs.keyDepot == _rhs.keyDepot; }
		public static bool				operator!=(REPOSITORY_ID _lhs, REPOSITORY_ID _rhs) 	{ return _lhs.keyDepot != _rhs.keyDepot; }
		public static bool				operator>=(REPOSITORY_ID _lhs, REPOSITORY_ID _rhs) 	{ return _lhs.keyDepot >= _rhs.keyDepot; }
		public static bool				operator> (REPOSITORY_ID _lhs, REPOSITORY_ID _rhs) 	{ return _lhs.keyDepot >  _rhs.keyDepot; }
		public static bool				operator<=(REPOSITORY_ID _lhs, REPOSITORY_ID _rhs) 	{ return _lhs.keyDepot <= _rhs.keyDepot; }
		public static bool				operator< (REPOSITORY_ID _lhs, REPOSITORY_ID _rhs) 	{ return _lhs.keyDepot <  _rhs.keyDepot; }

		public override bool			Equals(System.Object _Obj)
								{
									if (_Obj == null)
									{
										return false;
									}

									var p = _Obj as REPOSITORY_ID?;

									if (p.Value == null)
									{
										return false;
									}

									return keyDepot == p.Value.keyDepot;
								}

		public override int				GetHashCode()
								{
									return (int)keyDepot;
								}
	};

	[StructLayout(LayoutKind.Sequential, Pack =1)]
	public struct ACCOUNT_ID
	{
		public UInt64					keyAccount;

		public bool						empty() 									{ return keyAccount == 0;}
		public bool						exist()										{ return keyAccount != 0; }
		public void						reset(UInt64 _keyAccount=0)					{ keyAccount=_keyAccount;}
		public bool						valid() 									{ bool bValid=true; bValid=exist(); return bValid;}

		public static bool				operator==(ACCOUNT_ID _lhs, ACCOUNT_ID _rhs){ return _lhs.keyAccount==_rhs.keyAccount;}
		public static bool				operator!=(ACCOUNT_ID _lhs, ACCOUNT_ID _rhs){ return _lhs.keyAccount!=_rhs.keyAccount;}
		public static bool				operator>=(ACCOUNT_ID _lhs, ACCOUNT_ID _rhs){ return _lhs.keyAccount>=_rhs.keyAccount;}
		public static bool				operator> (ACCOUNT_ID _lhs, ACCOUNT_ID _rhs){ return _lhs.keyAccount> _rhs.keyAccount;}
		public static bool				operator<=(ACCOUNT_ID _lhs, ACCOUNT_ID _rhs){ return _lhs.keyAccount<=_rhs.keyAccount;}
		public static bool				operator< (ACCOUNT_ID _lhs, ACCOUNT_ID _rhs){ return _lhs.keyAccount< _rhs.keyAccount;}
		public override bool			Equals(System.Object _Obj)
								{
									if (_Obj == null)
									{
										return false;
									}

									var p = _Obj as ACCOUNT_ID?;

									if (p.Value == null)
									{
										return false;
									}

									return keyAccount == p.Value.keyAccount;
								}

		public override int				GetHashCode()
								{
									return (int)keyAccount;
								}
	};



	[StructLayout(LayoutKind.Sequential, Pack =1)]
	public struct CHECKIN_ID
	{
		public ACCOUNT_ID				idAccount;
		public REPOSITORY_ID			didRepository;
		public UInt64					keyLogin;

		public bool						empty()										{ return keyLogin == 0;}
		public bool						exist()										{ return keyLogin != 0; }
		public bool						valid()										{ bool bValid=true; bValid=exist(); return bValid;}
		public void						reset()										{ idAccount.reset(); didRepository.reset(); keyLogin=0;}

		public static bool				operator==(CHECKIN_ID _lhs,CHECKIN_ID _rhs)	{ return _lhs.idAccount==_rhs.idAccount && _lhs.keyLogin==_rhs.keyLogin;}
		public static bool				operator!=(CHECKIN_ID _lhs,CHECKIN_ID _rhs)	{ return _lhs.idAccount!=_rhs.idAccount || _lhs.keyLogin!=_rhs.keyLogin;}
		public static bool				operator>=(CHECKIN_ID _lhs,CHECKIN_ID _rhs)	{ return (_lhs.idAccount>_rhs.idAccount) ? true : (_lhs.idAccount<_rhs.idAccount) ? false : _lhs.keyLogin >=_rhs.keyLogin; }
		public static bool				operator> (CHECKIN_ID _lhs,CHECKIN_ID _rhs)	{ return (_lhs.idAccount>_rhs.idAccount) ? true : (_lhs.idAccount<_rhs.idAccount) ? false : _lhs.keyLogin > _rhs.keyLogin; }
		public static bool				operator<=(CHECKIN_ID _lhs,CHECKIN_ID _rhs)	{ return (_lhs.idAccount<_rhs.idAccount) ? true : (_lhs.idAccount>_rhs.idAccount) ? false : _lhs.keyLogin <=_rhs.keyLogin; }
		public static bool				operator< (CHECKIN_ID _lhs,CHECKIN_ID _rhs)	{ return (_lhs.idAccount<_rhs.idAccount) ? true : (_lhs.idAccount>_rhs.idAccount) ? false : _lhs.keyLogin < _rhs.keyLogin; }
		public override bool			Equals(System.Object _Obj)
								{
									if (_Obj == null)
									{
										return false;
									}

									var p = _Obj as CHECKIN_ID?;

									if (p.Value == null)
									{
										return false;
									}

									return idAccount==p.Value.idAccount && keyLogin==p.Value.keyLogin;
								}

		public override int				GetHashCode()
								{
									return (int)keyLogin;
								}
	};

	[StructLayout(LayoutKind.Sequential, Pack =1)]
	public struct PLATFORM
	{
		public	eOS_TYPE				OSType;
		public	eLANGUAGE				Language;
		public	int						Country;
		public	int						versionClient;
	};

	[StructLayout(LayoutKind.Sequential, Pack =1)]
	public struct AUTHORITY
	{
	// Publics) 
		public AUTHORITY(ACCOUNT_ID _aid)
		{
			iGroup				 = -1;
			flagAuthorith		 = 0;
			ClientVersion		 = 0;
			eCertificationType	 = eCERTIFICATION_TYPE.BY_ACCOUNT_ID;
			CertificationData	 = new byte[512];
			CGD.buffer temp		 = new CGD.buffer(CertificationData, 0);
			temp.append<ACCOUNT_ID>(_aid);
		}
		public AUTHORITY(CHECKIN_ID _cid)
		{
			iGroup				 = -1;
			flagAuthorith		 = 0;
			ClientVersion		 = 0;
			eCertificationType	 = eCERTIFICATION_TYPE.BY_CHEKIN_ID;
			CertificationData	 = new byte[512];
			CGD.buffer temp		 = new CGD.buffer(CertificationData, 0);
			temp.append<CHECKIN_ID>(_cid);
		}
		public AUTHORITY(string _ID, string _Password)
		{
			iGroup				 = -1;
			flagAuthorith		 = 0;
			ClientVersion		 = 0;
			eCertificationType	 = eCERTIFICATION_TYPE.LOGIN_BY_IDPASSWORD;
			CertificationData	 = new byte[512];
			CGD.buffer temp		 = new CGD.buffer(CertificationData, 0);
			temp.append<int>(0); temp.append(_ID); temp.append(_Password);
		}
		public AUTHORITY(string _idDevice)
		{
			iGroup				 =-1;
			flagAuthorith		 = 0;
			ClientVersion		 = 0;
			eCertificationType	 = eCERTIFICATION_TYPE.LOGIN_BY_DEVICEID;
			CertificationData	 = new byte[512];
			CGD.buffer temp		 = new CGD.buffer(CertificationData, 0);
			temp.append<int>(1); temp.append(_idDevice);
		}

	// Publics) 	
		public void						Set(ACCOUNT_ID _aid)				{ eCertificationType=eCERTIFICATION_TYPE.BY_ACCOUNT_ID;	CertificationData=new byte[512];CGD.buffer temp=new CGD.buffer(CertificationData, 0); temp.append<ACCOUNT_ID>(_aid); }
		public void						Set(CHECKIN_ID _cid)				{ eCertificationType=eCERTIFICATION_TYPE.BY_CHEKIN_ID; CertificationData=new byte[512];CGD.buffer temp=new CGD.buffer(CertificationData, 0); temp.append<CHECKIN_ID>(_cid); }
		public void						Set(string _ID, string _Password)	{ eCertificationType=eCERTIFICATION_TYPE.LOGIN_BY_IDPASSWORD; CertificationData=new byte[512];CGD.buffer temp=new CGD.buffer(CertificationData, 0); temp.append<int>(0); temp.append(_ID); temp.append(_Password); }
		public void						Set(string _idDevice)				{ eCertificationType=eCERTIFICATION_TYPE.LOGIN_BY_DEVICEID;	CertificationData=new byte[512];CGD.buffer temp=new CGD.buffer(CertificationData, 0); temp.append<int>(1); temp.append(_idDevice); }

		public int						iGroup;								// 포함 권한
		public UInt64					flagAuthorith;						// 권한: 각 권한을 OR연산으로 조합할 수 있다.
		public	int						ClientVersion;						// 클라이언트 버전

		public eCERTIFICATION_TYPE		eCertificationType;
		public byte[]					CertificationData;
	};


	public static class Message
	{
		public static  CGD.buffer		Message_ACCOUNT_REQUEST_CERTIFY(REPOSITORY_ID _rid, REPOSITORY_ID _ridRemote)
			{
				// 1) Buffer를 할당받는다.
				CGD.buffer	tempBuffer	 = new CGD.buffer(MEM_POOL.ALLOC(1024));

				// 2) Message를 작성한다.
				tempBuffer.append<UInt32>();
				tempBuffer.append<UInt32>(CGMESSAGE.SERVER.ACCOUNT.REQUEST_CERTIFY);
				tempBuffer.append<REPOSITORY_ID>(_rid);
				tempBuffer.append<REPOSITORY_ID>(_ridRemote);

				// 3) Message 길이를 써넣는다.
				tempBuffer.set_front<UInt32>(tempBuffer.len);

				// Return)
				return	tempBuffer;
			}

		public static CGD.buffer		Message_ACCOUNT_RESPONSE_CERTIFY(REPOSITORY_ID _rid)
			{
				// 1) Buffer를 할당받는다.
				CGD.buffer	tempBuffer	 = new CGD.buffer(MEM_POOL.ALLOC(1024));

				// 2) Message를 작성한다.
				tempBuffer.append<UInt32>();
				tempBuffer.append<UInt32>(CGMESSAGE.SERVER.ACCOUNT.RESPONSE_CERTIFY);
				tempBuffer.append<REPOSITORY_ID>(_rid);

				// 3) Message 길이를 써넣는다.
				tempBuffer.set_front<UInt32>(tempBuffer.len);

				// Return) 
				return	tempBuffer;
			}

		public static CGD.buffer		Message_ACCOUNT_REQUEST_RECOVER(TRANSACTION_ID _tid, CHECKIN_ID _cid)
			{
				// 1) 버퍼를 할당받는다.
				CGD.buffer	tempBuffer	 = new CGD.buffer(MEM_POOL.ALLOC(1024));

				// 2) Message를 작성한다.

				// 3) Message 길이를 설정한다.

				// Return) 
				return	tempBuffer;
			}

		public static CGD.buffer		Message_ACCOUNT_COMMAND_REQUEST_CHECKIN(TRANSACTION_ID _tid, CGD.buffer _bufRequest)
			{
				// 1) 버퍼를 할당받는다.
				//CGD.buffer	tempBuffer	 = new CGD.buffer(MEM_POOL.ALLOC(8+Marshal.SizeOf(_tid.GetType())+Marshal.SizeOf(_auth.GetType())));
				CGD.buffer	tempBuffer	 = new CGD.buffer(MEM_POOL.ALLOC(1024));

				// 2) Message를 작성한다.
				tempBuffer.append<UInt32>();
				tempBuffer.append<UInt32>(CGMESSAGE.SERVER.ACCOUNT.COMMAND_REQUEST_CHECKIN);
				tempBuffer.append<eTRANSACTION_COMMAND>(eTRANSACTION_COMMAND.CHECKIN);
				tempBuffer.append<TRANSACTION_ID>(_tid);
				tempBuffer.append(_bufRequest);

				// 3) Message 길이를 설정한다.
				tempBuffer.set_front<UInt32>(tempBuffer.len);

				// Return) 
				return	tempBuffer;
			}

		public static CGD.buffer		Message_ACCOUNT_COMMAND_CANCEL(TRANSACTION_ID _tid, eTRANSACTION_COMMAND _TransactionCommand, CHECKIN_ID _cid)
			{
				// 1) 버퍼를 할당받는다.
				//CGD.buffer tempBuffer = new CGD.buffer(MEM_POOL.ALLOC(8 + sizeof(eTRANSACTION_COMMAND) + sizeof(TRANSACTION_ID) + sizeof(CHECKIN_ID)));
				CGD.buffer tempBuffer = new CGD.buffer(MEM_POOL.ALLOC(1024));

				// 2) Message를 작성한다.
				tempBuffer.append<UInt32>();
				tempBuffer.append<UInt32>(CGMESSAGE.SERVER.ACCOUNT.COMMAND_CANCEL);
				tempBuffer.append<eTRANSACTION_COMMAND>(_TransactionCommand);
				tempBuffer.append<TRANSACTION_ID>(_tid);
				tempBuffer.append<CHECKIN_ID>(_cid);

				// 3) Message 길이를 설정한다.
				tempBuffer.set_front<UInt32>(tempBuffer.len);

				// Return) 
				return tempBuffer;
			}

		public static CGD.buffer		Message_ACCOUNT_COMMAND_REQUEST_CHECKOUT(TRANSACTION_ID _tid, CHECKIN_ID _cid)
			{
				// 1) 버퍼를 할당받는다.
				//CGD.buffer	tempBuffer	 = new CGD.buffer(MEM_POOL.ALLOC(8+Marshal.SizeOf(_tid.GetType())+Marshal.SizeOf(_cid.GetType())));
				CGD.buffer	tempBuffer	 = new CGD.buffer(MEM_POOL.ALLOC(1024));

				// 2) Message를 작성한다.
				tempBuffer.append<UInt32>();
				tempBuffer.append<UInt32>(CGMESSAGE.SERVER.ACCOUNT.COMMAND_REQUEST_CHECKOUT);
				tempBuffer.append<eTRANSACTION_COMMAND>(eTRANSACTION_COMMAND.CHECKOUT);
				tempBuffer.append<TRANSACTION_ID>(_tid);
				tempBuffer.append<CHECKIN_ID>(_cid);

				// 3) Message 길이를 설정한다.
				tempBuffer.set_front<UInt32>(tempBuffer.len);

				// Return) 
				return	tempBuffer;
			}

		public static CGD.buffer		Message_ACCOUNT_COMMAND_REQUEST_TRANSACTION(TRANSACTION_ID _tid, ACCOUNT_ID _aid, CHECKIN_ID _cid, CGD.buffer _bufRequest)
			{
				// 1) 버퍼를 할당받는다.
				//CGD.buffer	tempBuffer	 = new CGD.buffer(MEM_POOL.ALLOC(8+Marshal.SizeOf(_tid.GetType())+Marshal.SizeOf(_cid.GetType())));
				CGD.buffer	tempBuffer	 = new CGD.buffer(MEM_POOL.ALLOC(1024+ _bufRequest.len));

				// 2) Message를 작성한다.
				tempBuffer.append<UInt32>();
				tempBuffer.append<UInt32>(CGMESSAGE.SERVER.ACCOUNT.COMMAND_REQUEST_TRANSACTION);
				tempBuffer.append<eTRANSACTION_COMMAND>(eTRANSACTION_COMMAND.UPDATE);
				tempBuffer.append<TRANSACTION_ID>(_tid);
				tempBuffer.append<ACCOUNT_ID>(_aid);
				tempBuffer.append<CHECKIN_ID>(_cid);
				tempBuffer.append(_bufRequest);

				// 3) Message 길이를 설정한다.
				tempBuffer.set_front<UInt32>(tempBuffer.len);

				// Return) 
				return	tempBuffer;
			}

		public static CGD.buffer		Message_ACCOUNT_COMMAND_RESPONSE(TRANSACTION_ID _tid, eOBJECT_RESULT _eResult, CGD.buffer _bufferAppend)
			{
				//CGD.buffer	tempBuffer	 = new CGD.buffer(MEM_POOL.ALLOC(8+Marshal.SizeOf(_tid.GetType())+sizeof(eOBJECT_RESULT)+_bufferAppend.len));
				CGD.buffer	tempBuffer	 = new CGD.buffer(MEM_POOL.ALLOC(1024));

				tempBuffer.append<UInt32>();
				tempBuffer.append<UInt32>(CGMESSAGE.SERVER.ACCOUNT.COMMAND_RESPONSE);
				tempBuffer.append<TRANSACTION_ID>(_tid);
				tempBuffer.append<eOBJECT_RESULT>(_eResult);
				tempBuffer.append(_bufferAppend);

				tempBuffer.set_front<UInt32>(tempBuffer.len);

				return	tempBuffer;
			}
	}
}
