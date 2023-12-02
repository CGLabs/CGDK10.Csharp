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
	public class GroupSimple : CGDK.Server.Group.ListSender<SocketTcp>
	{
		protected override void OnMemberEntered(SocketTcp _member, sMESSAGE? _msg)
		{
			// trace) 
			Console.WriteLine("OnMemberEntered>> member entered (id:" + _member.m_groupable.MemberSerial + ")");

			// * socket객체의 메시지 중재자에 등록한다.
			_member.m_MessageTransmitter.RegisterMessageable(this);

			// - 모든 group 멤버에 Message 전송
			//Send(CCGBuffer(MEM_POOL_ALLOC(256))<<uint_t()<<uint_t(MESSAGE_EnterMember)<<_pMember->m_MemberSerial<<"TestID"<<CGD::SET_LENGTH());
		}
		protected override ulong OnMemberLeaving(SocketTcp _member, sMESSAGE? _msg)
		{
			// - 모든 Group 멤버에 Message 전송
			//Send(CCGBuffer(MEM_POOL_ALLOC(12))<<uint_t(12)<<uint_t(MESSAGE_LEAVE_member)<<_pMember->m_MemberSerial);

			// trace) 
			Console.WriteLine("OnMemberLeaving>> member entered (id:" + _member.m_groupable.MemberSerial + ")");

			// * socket객체의 메시지 중재자에서 등록해제한다.
			_member.m_MessageTransmitter.UnregisterMessageable(this);

			// trace) 
			return 0;
		}
	};
}