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
using CGDK;

// ----------------------------------------------------------------------------
//
// SocketTcpClient
//
// ----------------------------------------------------------------------------
namespace TutorialGroupServer
{
	public class GroupArray : CGDK.Server.Group.Array<SocketTcp>
	{
		// @ Group에 새로운 멤버가 입장했을 때 호출되는 함수.
		protected override void OnMemberEntered(SocketTcp _member, sMESSAGE? _msg)
		{
			// trace) 
			Console.WriteLine("OnMemberEntered>> member entered (id:" + _member.m_groupable.MemberSerial + ")");

			// - 모든 group 멤버에 Message 전송
			//Send(CCGBuffer(MEM_POOL_ALLOC(256))<<uint_t()<<uint_t(MESSAGE_EnterMember)<<_pMember->m_MemberSerial<<"TestID"<<CGD::SET_LENGTH());
		}

		// @ Group에서 멤버가 나갈 때 호출되는 함수.
		protected override ulong OnMemberLeaving(SocketTcp _member, sMESSAGE? _msg)
		{
			// 1) 모든 Group 멤버에 Message 전송
			//Send(CCGBuffer(MEM_POOL_ALLOC(12))<<uint_t(12)<<uint_t(MESSAGE_LEAVE_member)<<_pMember->m_MemberSerial);

			// trace) 
			Console.WriteLine("OnMemberLeaving>> member entered (id:" + _member.m_groupable.MemberSerial + ")");

			// trace) 
			return 0;
		}
	};
}
