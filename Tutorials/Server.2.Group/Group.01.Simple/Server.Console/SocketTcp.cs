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
	public class SocketTcp : CGDK.Net.Socket.Tcp
	{
		public SocketTcp()
		{
			m_groupable = new CGDK.Server.IGroupable<SocketTcp>(this);
		}

		// @ 접속이 되었을 때 불려지는 함수.
		protected override void OnConnect()
		{
			// trace) 
			Console.WriteLine(" @ connected");

			// 1) Group에 Enter시킨다.
			Program.pgroup.EnterMember(m_groupable);
		}
		// @ 접속이 종료되었을 때 호출되는 함수.
		protected override void OnDisconnect(ulong _disconnect_reason)
		{
			// 1) 접속이 종료되면 Group에서 나간다.
			this.m_groupable.LeaveGroup();

			// trace) 
			Console.WriteLine(" @ disconnected");
		}
		// @ Message를 받았을 때 불려지는 함수.
		protected override int OnMessage(object _source, sMESSAGE _msg)
		{
			if (_msg.message == eMESSAGE.SYSTEM.MESSAGE)
			{
				// trace) 
				Console.WriteLine(" @ message received (" + "Size:" + _msg.bufMessage.Count + " Message:" + _msg.bufMessage.GetFront<uint>(4) + ")");

				// 1) 네트워크로 전달된 메시지면 Group 전체에게 전송한다.
				Program.pgroup.Send(_msg.bufMessage);
			}

			// return)
			return 0;
		}

		// 4) Igroupable
		public CGDK.Server.IGroupable<SocketTcp> m_groupable;

		// 5) Member Serial
		protected int m_MemberSerial;
	}

}