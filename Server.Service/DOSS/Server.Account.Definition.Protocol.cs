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

//public class Definition
//{
//	public const UInt32 MESSAGE_HEAD		 = (CODE_TYPE.NETWORK | CODE_TYPE.TAIL_A);// (CODE_TYPE.NETWORK | CODE_TYPE.TAIL_A)
//}

//-----------------------------------------------------------------------------
//
// 설명)
//
//    - AccountID : 계정의 고유 번호:DB에 저장된 번호며 영원 불변
//    - LoginID   : 로긴 때마다 부여되는 ID로 해당 로긴에서만 유효하다.
//    - CheckinID : 체크인 할때 마다 부여되는 ID로 해당 서버에서만 유효한 ID이다.
//    - TransactionID : 요청때마다 할당되는 ID로 Transaction을 구분하기 위해 사용된다.
//
//-----------------------------------------------------------------------------
namespace CGMESSAGE
{
namespace SERVER
{
public static class ACCOUNT
{
	// Definitions

	// 1) Manager용 
	public const UInt32	REGISTER				 = CGMESSAGE.BASE.SERVER.ACCOUNT |   1;
	public const UInt32	UNREGISTER				 = CGMESSAGE.BASE.SERVER.ACCOUNT |   2;

	// 2) 처음 접속시 Certify를 요청하는 메시지
	//    - 확인 데이터
	//    - CERTIFY_ID:
	//      -1이면 Reset 및 재발급
	//      -1이 아니면 Recover 처리
	public const UInt32	REQUEST_CERTIFY			 = CGMESSAGE.BASE.SERVER.ACCOUNT |  10;

	// 3) 접속처리된 결과를 응답하는 메시지
	public const UInt32	RESPONSE_CERTIFY		 = CGMESSAGE.BASE.SERVER.ACCOUNT |  11;

	// 4) Depot이 Repository에 재접속 후 복구를 요청 함.
	public const UInt32	REQUEST_RECOVER			 = CGMESSAGE.BASE.SERVER.ACCOUNT |  12;

	// 5) Check-In/Check-Out을 요청한다.
	//    - Check-In 요청 시 LoginID를 가지고 있으면 그것을 발행한 쪽에서 강제 이행 받을 수 있다.
	//    - Check-In 요청 시 LoginID를 가지고 있지 않으면 중복 접속시 Reject된다.
	//    - Request-ID의 신규 발행은 Log-In시에만 된다.
	public const UInt32	COMMAND_RESPONSE				 = CGMESSAGE.BASE.SERVER.ACCOUNT |  30;
	public const UInt32	COMMAND_REQUEST_CHECKIN		   	 = CGMESSAGE.BASE.SERVER.ACCOUNT |  31;
	public const UInt32	COMMAND_REQUEST_CHECKOUT		 = CGMESSAGE.BASE.SERVER.ACCOUNT |  32;
	public const UInt32	COMMAND_REQUEST_CHECKOUT_REMOTE	 = CGMESSAGE.BASE.SERVER.ACCOUNT |  33;
	public const UInt32	COMMAND_REQUEST_UPDATE			 = CGMESSAGE.BASE.SERVER.ACCOUNT |  34;
	public const UInt32	COMMAND_REQUEST_TRANSACTION	     = CGMESSAGE.BASE.SERVER.ACCOUNT |  35;
	public const UInt32	COMMAND_CANCEL              	 = CGMESSAGE.BASE.SERVER.ACCOUNT |  36;
}

}
}