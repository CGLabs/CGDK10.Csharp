//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                          Group Template Classes                           *
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
using System.Text;
using System.Diagnostics;

namespace CGDK.server.time_event
{
	public class Nevent : Ievent
	{
	// constructor/destructor)
		public Nevent() {}
		public Nevent(string _name) { name = _name; }
		public Nevent(sEVENT_SETTING _event_setting, CGDK.server.time_event.Ientity.delegate_notify_process_event _event_function)
		{
			base.event_setting = _event_setting;
			base.event_status.time_next = _event_setting.time_execute;
			event_function = _event_function;
		}

		// implementation) 
		public CGDK.server.time_event.Ientity.delegate_notify_process_event event_function;

		// implementation)
		public override eRESULT	process_event(DateTime _time_now)
		{
			// check) 
			if (event_function == null)
				return eRESULT.NOT_READY;

			// 1) call event_function
			return event_function(_time_now);
		}
	}
}
