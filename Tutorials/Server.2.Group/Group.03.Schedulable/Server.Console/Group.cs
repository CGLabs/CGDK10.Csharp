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
	public class GroupSimple :
	CGDK.Server.Group.ListSender<SocketTcp>
	{
		public GroupSimple()
		{
			pschedulable = new CGDK.Schedulable.Executable(OnExecute, 2 * TimeSpan.TicksPerSecond);
		}

		protected override void OnStart(Context _Context)
		{
			SystemExecutor.RegisterSchedulable(pschedulable);
		}
		protected override void OnStop()
		{
			SystemExecutor.UnregisterSchedulable(pschedulable);
		}

		protected override void OnMemberEntered(SocketTcp _member, sMESSAGE? _msg)
		{
			// trace) 
			Console.WriteLine("OnMemberEntered>> member entered (id:" + _member.m_groupable.MemberSerial + ")");

			// 1) member를 등록한다.
			_member.m_MessageTransmitter.RegisterMessageable(this);
		}
		protected override ulong OnMemberLeaving(SocketTcp _member, sMESSAGE? _msg)
		{
			// trace) 
			Console.WriteLine("OnMemberLeaving>> member entered (id:" + _member.m_groupable.MemberSerial + ")");

			// 1) member를 등록해제한다.
			_member.m_MessageTransmitter.UnregisterMessageable(this);

			// trace) 
			return 0;
		}

		protected override int OnMessage(object _source, sMESSAGE _msg)
		{
			// return)
			return 0;
		}
		protected void OnExecute(object _object, object _param)
		{
			// - 전송할 메시지를 작성한다.
			var buf_Send = CGDK.Factory.Memory.AllocBuffer(256);
			buf_Send.Append<UInt32>(0);
			buf_Send.Append<UInt32>(0);
			buf_Send.SetFront<UInt32>((UInt32)buf_Send.Count, 0);

			// - 모든 group 멤버에 Message 전송
			this.Send(buf_Send);
		}

		protected CGDK.Schedulable.Executable pschedulable;
	};
}
