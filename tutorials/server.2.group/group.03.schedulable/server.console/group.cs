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
public class group_simple : 
	CGDK.server.group.list_sender<socket_tcp>
{
	public group_simple()
	{
		pschedulable = new CGDK.execute.schedulable.Cexecutable(on_execute, 2 * TimeSpan.TicksPerSecond );
	}

	protected override void		on_start(context _context)
	{
		CGDK.execute.system_executor.register_schedulable(pschedulable);
	}
	protected override void		on_stop()
	{
		CGDK.execute.system_executor.unregister_schedulable(pschedulable);
	}

	protected override void		on_member_entered(socket_tcp _member, sMESSAGE? _msg)
	{
		// trace) 
		Console.WriteLine("on_member_entered>> member entered (id:" + _member.m_groupable.member_serial + ")");

		// 1) member를 등록한다.
		_member.m_message_transmitter.register_messageable(this);
	}
	protected override ulong	on_member_leaving(socket_tcp _member, sMESSAGE? _msg)
	{
		// trace) 
		Console.WriteLine("on_member_leaving>> member entered (id:" + _member.m_groupable.member_serial + ")");

		// 1) member를 등록해제한다.
		_member.m_message_transmitter.unregister_messageable(this);

		// trace) 
		return 0;
	}

	protected override int		on_message(object _source, sMESSAGE _msg)
	{
		// return)
		return 0;
	}
	protected void				on_execute(object _object, object _param)
	{
		// - 전송할 메시지를 작성한다.
		var buf_send = CGDK.factory.memory.alloc_buffer(256);
		buf_send.append<UInt32>();
		buf_send.append<UInt32>();
		buf_send.set_front<UInt32>(buf_send.size);

		// - 모든 group 멤버에 Message 전송
		send(buf_send);
	}

	protected CGDK.execute.schedulable.Cexecutable	pschedulable;
};
