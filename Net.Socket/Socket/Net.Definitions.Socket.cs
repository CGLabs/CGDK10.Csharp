//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                          Network Socket Classes                           *
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
using System.Net;
using System.Net.Sockets;
using CGDK;

//----------------------------------------------------------------------------
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
namespace Net
{

public enum eSOCKET_STATE : int
{
	CLOSED						 = 0,	//  0 : 접속이 종료된 상태
	BINDED						 = 1,	//  1 : Binded
	SYN							 = 2,	//  2 : 접속진행 중인 상태
	SEND_DISCONNECTED			 = 3,	//  3 : (Active) 접속종료 진행상태
	FIN_WAIT_1					 = 4,	//  4 : (Active) 접속종료 진행상태
	FIN_WAIT_2					 = 5,	//  5 : (Active) 접속종료 진행상태
	TIME_WAIT					 = 6,	//  6 : (Active) 접속종료 진행상태
	CLOSE_WAIT					 = 7,	//  7 : (Passive) 접속종료 진행상태
	LAST_ACK					 = 8,	//  8 : (Passive) 접속종료 진행상태
	ESTABLISHED					 = 9,	//  9 : 접속만 된 상태
	LISTEN						 = 10,	// 10 : Listen상태
	CERTIFIED					 = 11,	// 11 : 접속이 되고 CGNet의 Client검증이 끝난 상태.
	LOGINED						 = 12,	// 12 : ID와 Password를 넣고 Log-In이 된 상태.
	MAX									// MAX
};

public class DISCONNECT_REASON
{
	public const ulong	NODISCONNECTEX               = 0x1000000000000000;	// Mask
	
	public const ulong	NONE						 = 0x0000000000000000;	// 일상적인 처리 과정일 경우
	public const ulong	FAIL						 = 0x0000000100000000;	// 오류로 인한 접속 종료인 경우(송수신 중 오류, 상대방에 의한 강제접속 종료)
	public const ulong	ACTIVE						 = 0x0000000200000000;	// 접속 종료를 요청한 측일 경우(설정되어 있지 않으면 Passive)
	public const ulong	ABORTIVE					 = 0x0000000400000000;	// 강제 접속종료일 경우(설정되어 있지 않으면 Graceful)
	public const ulong	EXCEPTION					 = 0x0000000800000000;	// 예외로 인해 것일 경우
	public const ulong	MASK						 = 0x0fffffff00000000;  // Mask
	public const ulong MASK_USER					 = 0x00000000ffffffff;	// User 정의
	
	public const ulong	OVERFLOW_MAX_MESSAGES		 = 0x0000000000000001;	// Send시 Message 수의 Overflow로 접속을 종료했다.
	public const ulong	OVERFLOW_MAX_BYTES			 = 0x0000000000000002;	// Send시 Bytes 수의 Overflow로 접속을 종료했다.
	public const ulong	MESSAGE_LENGTH_TOO_LONG		 = 0x0000000000000003;	// Message의 길이가 너무 길다
	public const ulong	MESSAGE_LENGTH_TOO_SHORT	 = 0x0000000000000004;	// Message의 길이가 너무 짧다
	public const ulong	MESSAGE_UNHANDLED_EXCEPTION	 = 0x0000000000000005;	// Message처리 중 핸들링 되지 않은 Exception
	public const ulong	BUFFER_IS_NULL				 = 0x0000000000000006;	// shared_buffer의 buf값이 nullptr이다.
	public const ulong	INVALID_STATE				 = 0x0000000000000007;	// 처리 과정상 있을 수 없는 상태이다.
	public const ulong	INVALID_SOCKET_HANDLE		 = 0x0000000000000008;	// Socket handle이 INVALID_SOCKET_HANDLE이다.
	public const ulong	FAIL_TO_SOCKET_PREPARE		 = 0x0000000000000009;	// Socket의 prepare단계에서 실패했다.
	public const ulong	FAIL_TO_SOCKET_ACCEPT		 = 0x000000000000000a;	// Socket의 Accept 과정에서실패했다.
	public const ulong	FAIL_TO_SOCKET_CONNECT		 = 0x000000000000000b;  // Socket의 connect 단계에서 실패했다.
	public const ulong KEEP_ALIVE_TIME_OUT			 = 0x000000000000000c;	// Socket의 prepare단계에서 실패했다.
	
	public const ulong	SUCCESS						 = 0;
	public const ulong	GRACEFUL					 = 0;
	public const ulong	FAIL_TIMEOUT				 = (DISCONNECT_REASON.FAIL | DISCONNECT_REASON.ACTIVE);
	public const ulong	FAIL_ABORTIVE				 = (DISCONNECT_REASON.FAIL | DISCONNECT_REASON.ABORTIVE | DISCONNECT_REASON.ACTIVE);
	public const ulong FAIL_EXCEPTION				 = (DISCONNECT_REASON.FAIL | DISCONNECT_REASON.EXCEPTION);
}

// definitions) 
public delegate void	delegateNotify_io		(object _source, CGDK.buffer _buffer_received, SocketAsyncEventArgs _args);
public delegate void	delegateNotify_connect	(object _source, IPEndPoint _remote_endpoint);
public delegate void	delegateNotifyAccept	(object _acceptor, Io.IConnectable _socket);


}
}
