//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                           Server Event Classes                            *
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
// ----------------------------------------------------------------------------
//
// CGDK.Server.system.TimeEvent.schedule.Iobject
//
// 1. ICGEventObject란?
//    1) EventObject 객체의 Interface Class이다.
//    2) ICGExecutalbe을 상속받은 클래스로 Executor에 물려 실행되도록 설계된다.
//
//
// ----------------------------------------------------------------------------
namespace CGDK.Server.TimeEvent
{
    public class IEvent : INameable
    {
    // publics)
		public ref sEVENT_SETTING   EventSetting { get { return ref this.m_EventSetting; } }
		public ref sEVENT_STATUS    EventStatus { get { return ref this.m_EventStatus; } }
        public string Name { get { return this.m_EventSetting.Name; } set { this.m_EventSetting.Name = value; } }

    // implementation) 
	    public virtual void		    ProcessReset() { }
        public virtual eRESULT	    ProcessEvent(DateTime _time_now) { return eRESULT.SUCCESS; }

    // implementation)
        public sEVENT_SETTING	    m_EventSetting;
        public sEVENT_STATUS		m_EventStatus;
	    protected IEntity          	m_entity;
    }
}
