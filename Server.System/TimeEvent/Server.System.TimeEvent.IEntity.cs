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
namespace CGDK.Server.TimeEvent
{
	public abstract class IEntity : NObjectStateable, IPausable, IExecutable, INameable
	{
	// definitions) 
		public const int			EVENT_TIMES_INFINITE = 32;
		public delegate eRESULT DelegateNotifyProcessEvent(DateTime _time);


		// constructor/destructor) 
		public IEntity() { m_entity_setting.Type = 0; }
		public	IEntity(Int64 _type) { m_entity_setting.Type = _type; }

		// public) 
		public ref sENTITY_SETTING	EntitySetting	{ get { return ref this.m_entity_setting; } }
		public ref sENTITY_STATUS	EntityStatus	{ get { return ref this.m_entity_status; } }

		public bool					Pause() { return true; }
		public bool					Resume() { return true; }
		public string				Name { get { return this.m_entity_setting.Name; } set { this.m_entity_setting.Name = value; } }

	// implementation)  
		protected void				LOG_EVENT(ILogTargetable _plog_target, eLOG_TYPE _type, int _level, string _message)
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
					log_sub.Type = _type;
					log_sub.Level = _level;
					log_sub.bufMessage = _message;
					log_sub.Attribute = 0;
					log_sub.Source = 0;
					log_sub.Destination = 0;
					log_sub.timeOccure = presult_event.Value.timeOccure;

					// 3) add LOG_RECORD object
					presult_event.Value.sub_log.AddLast(log_sub);
				}

				// Log)
				LOG.Write(_plog_target, log_sub);
			}
			catch (System.Exception)
			{
			}
		}

		public abstract long		ProcessExecute(ulong _return, ulong _param);

		protected LOG_RECORD		_AppendResultLog(string _message)
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
					pevent_result.Type = eLOG_TYPE.UNDEFINED;
					pevent_result.Level = eLOG_LEVEL.NORMAL;
					pevent_result.bufMessage = _message;
					pevent_result.Attribute = 0;
					pevent_result.Source = 0;
					pevent_result.Destination = 0;
					pevent_result.timeOccure = DateTime.UtcNow;

					// 4) 새로운 Result 객체를 추가한다.
					this.m_plog_result.sub_log.AddLast(pevent_result);

					// check) 보관하는 Result의 객체가 한계를 넘을 경우 제일 앞의 Result를 하나 제거한다.
					if (this.m_plog_result.sub_log.Count > 256)
					{
						this.m_plog_result.sub_log.RemoveFirst();
					}
				}
			}
			catch (System.Exception)
			{
			}

			// return) 
			return pevent_result;
		}
		protected void				_AppendResultLogPost(LOG_RECORD _log_result)
		{
			// check) _log_result shouldn't be nullptr
			Debug.Assert(_log_result != null);

			// check) _log_result shouldn't be nullptr
			if (_log_result == null)
				return;

			lock(m_plog_result)
			{
				// 2) ...
				_log_result.bufMessage += " [" + _log_result.Result.ToString() + "]";
			}
		}

		// implementation)  
		protected sENTITY_SETTING	m_entity_setting;
		protected sENTITY_STATUS	m_entity_status;
		protected LOG_RECORD		m_plog_result;
	}
}