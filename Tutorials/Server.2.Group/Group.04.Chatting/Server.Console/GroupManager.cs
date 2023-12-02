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
using System.Collections.Generic;

// ----------------------------------------------------------------------------
//
// SocketTcpClient
//
// ----------------------------------------------------------------------------
namespace TutorialGroupServer
{
	public class GroupManager
	{
		public void RegisterGroup(GroupRoom _pgroup)
		{
			this.list_group.Add(_pgroup);
		}
		public void UnregisterGroup(GroupRoom _pgroup)
		{
			this.list_group.Remove(_pgroup);
		}

		private List<GroupRoom> list_group = new();
	};
}