//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                 tutorials group - chatting.server.console                 *
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
			this.m_groupable = new(this);
		}

		// @ 접속이 되었을 때 불려지는 함수.
		protected override void OnConnect()
		{
			// trace) 
			Console.WriteLine(" @ connected");
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
			return this.m_MessageTransmitter.TransmitMessage(this, _msg);
		}

		// 4) Igroupable
		public CGDK.Server.IGroupable<SocketTcp> m_groupable;

		// 5) message transmitter
		public CGDK.MessageTransmitter m_MessageTransmitter = new();

		// 6) Member Serial
		protected int m_MemberSerial;
	}
}