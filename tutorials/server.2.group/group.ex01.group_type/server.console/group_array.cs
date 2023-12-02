//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                  tutorials group - simple.server.console                  *
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
using System.Threading;
using CGDK;
using CGDK.net;
using CGDK.server.group;
using System.Collections.Generic;

// ----------------------------------------------------------------------------
//
// socket_tcpClient
//
// ----------------------------------------------------------------------------
public class group_array : CGDK.server.group.array<socket_tcp>
{
	// @ Group에 새로운 멤버가 입장했을 때 호출되는 함수.
	protected override void		on_member_entered(socket_tcp _member, sMESSAGE? _msg)
	{
		// trace) 
		Console.WriteLine("on_member_entered>> member entered (id:" + _member.m_groupable.member_serial + ")");

		// - 모든 group 멤버에 Message 전송
		//send(CCGBuffer(MEM_POOL_ALLOC(256))<<uint_t()<<uint_t(MESSAGE_ENTER_member)<<_pMember->m_member_serial<<"TestID"<<CGD::SET_LENGTH());
	}

	// @ Group에서 멤버가 나갈 때 호출되는 함수.
	protected override ulong	on_member_leaving(socket_tcp _member, sMESSAGE? _msg)
	{
		// 1) 모든 Group 멤버에 Message 전송
		//send(CCGBuffer(MEM_POOL_ALLOC(12))<<uint_t(12)<<uint_t(MESSAGE_LEAVE_member)<<_pMember->m_member_serial);

		// trace) 
		Console.WriteLine("on_member_leaving>> member entered (id:" + _member.m_groupable.member_serial + ")");

		// trace) 
		return 0;
	}
};
