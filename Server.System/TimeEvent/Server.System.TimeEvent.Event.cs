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

namespace CGDK.Server.TimeEvent
{
	public class NEvent : IEvent
	{
	// constructor/destructor)
		public NEvent() {}
		public NEvent(string _name) { this.Name = _name; }
		public NEvent(sEVENT_SETTING _EventSetting, CGDK.Server.TimeEvent.IEntity.DelegateNotifyProcessEvent _event_function)
		{
			base.EventSetting = _EventSetting;
			base.EventStatus.timeNext = _EventSetting.timeExecute;
			EventFunction = _event_function;
		}

		// implementation) 
		public CGDK.Server.TimeEvent.IEntity.DelegateNotifyProcessEvent EventFunction;

		// implementation)
		public override eRESULT	ProcessEvent(DateTime _time_now)
		{
			// check) 
			if (EventFunction == null)
				return eRESULT.NOT_READY;

			// 1) call EventFunction
			return EventFunction(_time_now);
		}
	}
}
