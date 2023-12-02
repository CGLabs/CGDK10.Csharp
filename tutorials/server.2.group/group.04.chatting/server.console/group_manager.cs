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
public class group_manager
{
	public void register_group(group_room _pgroup)
	{
		list_group.Add(_pgroup);
	}
	public void unregister_group(group_room _pgroup)
	{
		list_group.Remove(_pgroup);
	}

	private List<group_room> list_group = new List<group_room>();
};
