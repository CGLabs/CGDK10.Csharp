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
using System.Diagnostics;
using System.Collections.Generic;
using CGDK;

// ----------------------------------------------------------------------------
//
// CGDK.LOG
//
//
//
//
//
//
// ----------------------------------------------------------------------------
namespace CGDK
{
	public class LOG_RECORD
	{
		public eLOG_TYPE	Type;
		public int			Level;
		public eRESULT		Result;

		public int			CharEncoding;
		public string		bufMessage;

		public ulong		Origin;
		public ulong		Attribute;
		public ulong		Source;
		public ulong		Destination;

		public DateTime		timeOccure;

		public LinkedList<LOG_RECORD>	sub_log;
	}

	public static class LOG
	{
		public static	void	INFO				(ILogTargetable _log_targetable, string _string)	{ Write(_log_targetable, eLOG_TYPE.INFO, eLOG_LEVEL.NORMAL, _string);}
		public static	void	INFO_LOW			(ILogTargetable _log_targetable, string _string)	{ /*Write(_log_targetable, eLOG_TYPE.INFO, eLOG_LEVEL.LOWER, _string);*/}
		public static	void	INFO_IMPORTANT		(ILogTargetable _log_targetable, string _string)	{ Write(_log_targetable, eLOG_TYPE.INFO, eLOG_LEVEL.HIGHER, _string);}
																							  
		public static	void	PROGRESS			(ILogTargetable _log_targetable, string _string)	{ Write(_log_targetable, eLOG_TYPE.PROGRESS, eLOG_LEVEL.NORMAL, _string);}
		public static	void	PROGRESS_LOW		(ILogTargetable _log_targetable, string _string)	{ /*Write(_log_targetable, eLOG_TYPE.PROGRESS, eLOG_LEVEL.LOWER, _string);*/}
		public static	void	PROGRESS_IMPORTANT	(ILogTargetable _log_targetable, string _string)	{ Write(_log_targetable, eLOG_TYPE.PROGRESS, eLOG_LEVEL.HIGHER, _string);}
																							  
		public static	void	DEBUG				(ILogTargetable _log_targetable, string _string)	{ Write(_log_targetable, eLOG_TYPE.DEBUG, eLOG_LEVEL.NORMAL, _string);}
		public static	void	DEBUG_LOW			(ILogTargetable _log_targetable, string _string)	{ /*Write(_log_targetable, eLOG_TYPE.DEBUG, eLOG_LEVEL.LOWER, _string);*/}
		public static	void	DEBUG_IMPORTANT		(ILogTargetable _log_targetable, string _string)	{ Write(_log_targetable, eLOG_TYPE.DEBUG, eLOG_LEVEL.HIGHER, _string);}
																							  
		public static	void	EXCEPTION			(ILogTargetable _log_targetable, string _string)	{ Write(_log_targetable, eLOG_TYPE.EXCEPTION, eLOG_LEVEL.NORMAL, _string);}
		public static	void	EXCEPTION_LOW		(ILogTargetable _log_targetable, string _string)	{ Write(_log_targetable, eLOG_TYPE.EXCEPTION, eLOG_LEVEL.LOWER, _string);}
		public static	void	EXCEPTION_IMPORTANT	(ILogTargetable _log_targetable, string _string)	{ Write(_log_targetable, eLOG_TYPE.EXCEPTION, eLOG_LEVEL.HIGHER, _string);}
																							  
		public static	void	ERROR				(ILogTargetable _log_targetable, string _string)	{ Write(_log_targetable, eLOG_TYPE.ERROR, eLOG_LEVEL.NORMAL, _string);}
		public static	void	ERROR_LOW			(ILogTargetable _log_targetable, string _string)	{ Write(_log_targetable, eLOG_TYPE.ERROR, eLOG_LEVEL.LOWER, _string);}
		public static	void	ERROR_IMPORTANT		(ILogTargetable _log_targetable, string _string)	{ Write(_log_targetable, eLOG_TYPE.ERROR, eLOG_LEVEL.HIGHER, _string);}
																							  
		public static	void	WARNING				(ILogTargetable _log_targetable, string _string)	{ Write(_log_targetable, eLOG_TYPE.WARNING, eLOG_LEVEL.NORMAL, _string);}
		public static	void	WARNING_LOW			(ILogTargetable _log_targetable, string _string)	{ Write(_log_targetable, eLOG_TYPE.WARNING, eLOG_LEVEL.LOWER, _string);}
		public static	void	WARNING_IMPORTANT	(ILogTargetable _log_targetable, string _string)	{ Write(_log_targetable, eLOG_TYPE.WARNING, eLOG_LEVEL.HIGHER, _string);}

		public static	void	USER				(ILogTargetable _log_targetable, string _string)	{ Write(_log_targetable, eLOG_TYPE.USER, eLOG_LEVEL.NORMAL, _string);}
		public static	void	USER_LOW			(ILogTargetable _log_targetable, string _string)	{ /*Write(_log_targetable, eLOG_TYPE.USER, eLOG_LEVEL.LOWER, _string);*/}
		public static	void	USER_IMPORTANT		(ILogTargetable _log_targetable, string _string)	{ Write(_log_targetable, eLOG_TYPE.USER, eLOG_LEVEL.HIGHER, _string);}
																							  
		public static	void	SYSTEM				(ILogTargetable _log_targetable, string _string)	{ Write(_log_targetable, eLOG_TYPE.SYSTEM, eLOG_LEVEL.NORMAL, _string);}
		public static	void	SYSTEM_LOW			(ILogTargetable _log_targetable, string _string)	{ /*Write(_log_targetable, eLOG_TYPE.SYSTEM, eLOG_LEVEL.LOWER, _string);*/}
		public static	void	SYSTEM_IMPORTANT	(ILogTargetable _log_targetable, string _string)	{ Write(_log_targetable, eLOG_TYPE.SYSTEM, eLOG_LEVEL.HIGHER, _string);}

		public static	void	Write(ILogTargetable _log_targetable, LOG_RECORD _log_record)
		{
			// check)
			if(_log_record == null)
				return;

			// 1) Log 출력
		#if _DEBUG
			Trace.WriteLine(_log_record.bufMessage);
		#endif
			// 2) 전송할 Messageable
			ILogTargetable log_targetable = (_log_targetable != null) ? _log_targetable : null;

			// 3) 전송
			if(log_targetable != null)
			{
				log_targetable.Trace(_log_record);
			}
		}

		public static	void	Write(ILogTargetable _log_targetable, eLOG_TYPE _type, int _Level, string _message)
		{
			// check)
			Debug.Assert(_message != null);

			// check)
			if(_message == null)
				return;

			// 1) Trace 처리
			Trace.WriteLine(_message);

			// 2) 전송할 Messageable
			ILogTargetable log_targetable = (_log_targetable != null) ? _log_targetable : DefaultLogger;

			// 3) 전송
			if(log_targetable != null)
			{
				// - Log Record
				var log_record = new LOG_RECORD();
				log_record.Type = _type;
				log_record.Level = _Level;
				log_record.Result = eRESULT.SUCCESS;
				log_record.CharEncoding = 0;
				log_record.bufMessage = _message;
				log_record.Origin = 0;
				log_record.Attribute = 0;
				log_record.Source = 0;
				log_record.Destination = 0;
				log_record.timeOccure = DateTime.UtcNow;

				// - 전달하기
				log_targetable.Trace(log_record);
			}
		}

		// Default Trace) 
		public	static ILogTargetable	DefaultLogger { get {	return g_default_tracer;}	set { g_default_tracer = value; } }
		private static ILogTargetable	g_default_tracer = null;
	}																						  
}

