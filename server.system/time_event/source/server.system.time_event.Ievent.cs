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
// CGDK.server.system.time_event.schedule.Iobject
//
// 1. ICGEventObject란?
//    1) EventObject 객체의 Interface Class이다.
//    2) ICGExecutalbe을 상속받은 클래스로 Executor에 물려 실행되도록 설계된다.
//
//
// ----------------------------------------------------------------------------
namespace CGDK.server.time_event
{
    public class Ievent : Inameable
    {
    // publics)
		public ref sEVENT_SETTING   event_setting { get { return ref m_event_setting; } }
		public ref sEVENT_STATUS    event_status { get { return ref m_event_status; } }
        public string name { get { return m_event_setting.name; } set { m_event_setting.name = value; } }

    // implementation) 
	    public virtual void		    process_reset() { }
        public virtual eRESULT	    process_event(DateTime _time_now) { return eRESULT.SUCCESS; }

    // implementation)
        public sEVENT_SETTING	    m_event_setting;
        public sEVENT_STATUS		m_event_status;
	    protected Ientity          	m_entity;
    }
}
