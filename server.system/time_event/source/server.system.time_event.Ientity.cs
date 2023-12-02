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
using System.Collections.Generic;
using System.Diagnostics;


// ----------------------------------------------------------------------------
//
// ICGEventObject
//
// 1. ICGEventObject란?
//    1) EventObject 객체의 Interface Class이다.
//    2) ICGExecutalbe을 상속받은 클래스로 Executor에 물려 실행되도록 설계된다.
//
//
// ----------------------------------------------------------------------------
namespace CGDK.server.time_event
{
	public abstract class Ientity : Nobject_stateable, Ipausable, Iexecutable, Inameable
	{
	// definitions) 
		public const int			EVENT_TIMES_INFINITE = 32;
		public delegate eRESULT delegate_notify_process_event(DateTime _time);


		// constructor/destructor) 
		public Ientity() { m_entity_setting.type = 0; }
		public	Ientity(Int64 _type) { m_entity_setting.type = _type; }

		// public) 
		public ref sENTITY_SETTING	entity_setting	{ get { return ref m_entity_setting; } }
		public ref sENTITY_STATUS	entity_status	{ get { return ref m_entity_status; } }

		public bool					pause() { return true; }
		public bool					resume() { return true; }
		public string				name { get { return m_entity_setting.name; } set { m_entity_setting.name = value; } }

	// implementation)  
		protected void				LOG_EVENT(Ilog_targetable _plog_target, eLOG_TYPE _type, int _level, string _message)
		{
			// check) _format shouldn't be nullptr
			Debug.Assert(_message.Length != 0);

			// check) _format shouldn't be nullptr
			if (_message.Length == 0)
				return;

			try
			{
				//  declare) 
				LOG_RECORD log_sub;

				lock(m_plog_result)
				{
					// check) 
					if (m_plog_result == null)
						return;

					// check) 
					if (m_plog_result.sub_log == null)
						return;

					// 1) 제일 마지막의 SubEvent객체를 얻어 온다.
					var presult_event = m_plog_result.sub_log.Last;

					// 2) alloc LOG_RECORD object
					log_sub = new LOG_RECORD();

					// 3) set log info
					log_sub.type		 = _type;
					log_sub.level		 = _level;
					log_sub.buf_message  = _message;
					log_sub.attribute	 = 0;
					log_sub.source		 = 0;
					log_sub.destination	 = 0;
					log_sub.time_occure	 = presult_event.Value.time_occure;

					// 3) add LOG_RECORD object
					presult_event.Value.sub_log.AddLast(log_sub);
				}

				// Log)
				LOG.write(_plog_target, log_sub);
			}
			catch (System.Exception)
			{
			}
		}

		public abstract long		process_execute(ulong _return, ulong _param);

		protected LOG_RECORD		_append_result_log(string _message)
		{
			// check) _message shouldn't be nullptr
			Debug.Assert(_message.Length != 0);

			// check) _message shouldn't be nullptr
			if(_message.Length == 0)
				return null;

			// declare)
			LOG_RECORD pevent_result = null;

			try
			{
				lock(m_plog_result)
				{
					// 1) allocate new LOG_RECORD object
					pevent_result = new LOG_RECORD();

					// 2) set log info
					pevent_result.type			 = eLOG_TYPE.UNDEFINED;
					pevent_result.level			 = eLOG_LEVEL.NORMAL;
					pevent_result.buf_message	 = _message;
					pevent_result.attribute		 = 0;
					pevent_result.source		 = 0;
					pevent_result.destination	 = 0;
					pevent_result.time_occure	 = DateTime.UtcNow;

					// 4) 새로운 Result 객체를 추가한다.
					m_plog_result.sub_log.AddLast(pevent_result);

					// check) 보관하는 Result의 객체가 한계를 넘을 경우 제일 앞의 Result를 하나 제거한다.
					if (m_plog_result.sub_log.Count > 256)
					{
						m_plog_result.sub_log.RemoveFirst();
					}
				}
			}
			catch (System.Exception)
			{
			}

			// return) 
			return pevent_result;
		}
		protected void				_append_result_log_post(LOG_RECORD _log_result)
		{
			// check) _log_result shouldn't be nullptr
			Debug.Assert(_log_result != null);

			// check) _log_result shouldn't be nullptr
			if (_log_result == null)
				return;

			lock(m_plog_result)
			{
				// 2) ...
				_log_result.buf_message += " [" + _log_result.result.ToString() + "]";
			}
		}

		// implementation)  
		protected sENTITY_SETTING	m_entity_setting;
		protected sENTITY_STATUS	m_entity_status;
		protected LOG_RECORD		m_plog_result;
	}
}