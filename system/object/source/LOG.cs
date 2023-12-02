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
		public eLOG_TYPE	type;
		public int			level;
		public eRESULT		result;

		public int			char_encoding;
		public string		buf_message;

		public ulong		origin;
		public ulong		attribute;
		public ulong		source;
		public ulong		destination;

		public DateTime		time_occure;

		public LinkedList<LOG_RECORD>	sub_log;
	}

	public static class LOG
	{
		public static	void	INFO				(Ilog_targetable _log_targetable, string _string)	{ write(_log_targetable, eLOG_TYPE.INFO, eLOG_LEVEL.NORMAL, _string);}
		public static	void	INFO_LOW			(Ilog_targetable _log_targetable, string _string)	{ /*write(_log_targetable, eLOG_TYPE.INFO, eLOG_LEVEL.LOWER, _string);*/}
		public static	void	INFO_IMPORTANT		(Ilog_targetable _log_targetable, string _string)	{ write(_log_targetable, eLOG_TYPE.INFO, eLOG_LEVEL.HIGHER, _string);}
																							  
		public static	void	PROGRESS			(Ilog_targetable _log_targetable, string _string)	{ write(_log_targetable, eLOG_TYPE.PROGRESS, eLOG_LEVEL.NORMAL, _string);}
		public static	void	PROGRESS_LOW		(Ilog_targetable _log_targetable, string _string)	{ /*write(_log_targetable, eLOG_TYPE.PROGRESS, eLOG_LEVEL.LOWER, _string);*/}
		public static	void	PROGRESS_IMPORTANT	(Ilog_targetable _log_targetable, string _string)	{ write(_log_targetable, eLOG_TYPE.PROGRESS, eLOG_LEVEL.HIGHER, _string);}
																							  
		public static	void	DEBUG				(Ilog_targetable _log_targetable, string _string)	{ write(_log_targetable, eLOG_TYPE.DEBUG, eLOG_LEVEL.NORMAL, _string);}
		public static	void	DEBUG_LOW			(Ilog_targetable _log_targetable, string _string)	{ /*write(_log_targetable, eLOG_TYPE.DEBUG, eLOG_LEVEL.LOWER, _string);*/}
		public static	void	DEBUG_IMPORTANT		(Ilog_targetable _log_targetable, string _string)	{ write(_log_targetable, eLOG_TYPE.DEBUG, eLOG_LEVEL.HIGHER, _string);}
																							  
		public static	void	EXCEPTION			(Ilog_targetable _log_targetable, string _string)	{ write(_log_targetable, eLOG_TYPE.EXCEPTION, eLOG_LEVEL.NORMAL, _string);}
		public static	void	EXCEPTION_LOW		(Ilog_targetable _log_targetable, string _string)	{ write(_log_targetable, eLOG_TYPE.EXCEPTION, eLOG_LEVEL.LOWER, _string);}
		public static	void	EXCEPTION_IMPORTANT	(Ilog_targetable _log_targetable, string _string)	{ write(_log_targetable, eLOG_TYPE.EXCEPTION, eLOG_LEVEL.HIGHER, _string);}
																							  
		public static	void	ERROR				(Ilog_targetable _log_targetable, string _string)	{ write(_log_targetable, eLOG_TYPE.ERROR, eLOG_LEVEL.NORMAL, _string);}
		public static	void	ERROR_LOW			(Ilog_targetable _log_targetable, string _string)	{ write(_log_targetable, eLOG_TYPE.ERROR, eLOG_LEVEL.LOWER, _string);}
		public static	void	ERROR_IMPORTANT		(Ilog_targetable _log_targetable, string _string)	{ write(_log_targetable, eLOG_TYPE.ERROR, eLOG_LEVEL.HIGHER, _string);}
																							  
		public static	void	WARNING				(Ilog_targetable _log_targetable, string _string)	{ write(_log_targetable, eLOG_TYPE.WARNING, eLOG_LEVEL.NORMAL, _string);}
		public static	void	WARNING_LOW			(Ilog_targetable _log_targetable, string _string)	{ write(_log_targetable, eLOG_TYPE.WARNING, eLOG_LEVEL.LOWER, _string);}
		public static	void	WARNING_IMPORTANT	(Ilog_targetable _log_targetable, string _string)	{ write(_log_targetable, eLOG_TYPE.WARNING, eLOG_LEVEL.HIGHER, _string);}

		public static	void	USER				(Ilog_targetable _log_targetable, string _string)	{ write(_log_targetable, eLOG_TYPE.USER, eLOG_LEVEL.NORMAL, _string);}
		public static	void	USER_LOW			(Ilog_targetable _log_targetable, string _string)	{ /*write(_log_targetable, eLOG_TYPE.USER, eLOG_LEVEL.LOWER, _string);*/}
		public static	void	USER_IMPORTANT		(Ilog_targetable _log_targetable, string _string)	{ write(_log_targetable, eLOG_TYPE.USER, eLOG_LEVEL.HIGHER, _string);}
																							  
		public static	void	SYSTEM				(Ilog_targetable _log_targetable, string _string)	{ write(_log_targetable, eLOG_TYPE.SYSTEM, eLOG_LEVEL.NORMAL, _string);}
		public static	void	SYSTEM_LOW			(Ilog_targetable _log_targetable, string _string)	{ /*write(_log_targetable, eLOG_TYPE.SYSTEM, eLOG_LEVEL.LOWER, _string);*/}
		public static	void	SYSTEM_IMPORTANT	(Ilog_targetable _log_targetable, string _string)	{ write(_log_targetable, eLOG_TYPE.SYSTEM, eLOG_LEVEL.HIGHER, _string);}

		public static	void	write(Ilog_targetable _log_targetable, LOG_RECORD _log_record)
		{
			// check)
			if(_log_record == null)
				return;

			// 1) Log 출력
		#if _DEBUG
			Trace.WriteLine(_log_record.buf_message);
		#endif
			// 2) 전송할 Messageable
			Ilog_targetable log_targetable = (_log_targetable != null) ? _log_targetable : null;

			// 3) 전송
			if(log_targetable != null)
			{
				log_targetable.trace(_log_record);
			}
		}

		public static	void	write(Ilog_targetable _log_targetable, eLOG_TYPE _type, int _level, string _message)
		{
			// check)
			Debug.Assert(_message != null);

			// check)
			if(_message == null)
				return;

			// 1) Trace 처리
			Trace.WriteLine(_message);

			// 2) 전송할 Messageable
			Ilog_targetable log_targetable = (_log_targetable != null) ? _log_targetable : default_logger;

			// 3) 전송
			if(log_targetable != null)
			{
				// - Log Record
				var log_record			 = new LOG_RECORD();
				log_record.type			 = _type;
				log_record.level		 = _level;
				log_record.result		 = eRESULT.SUCCESS;
				log_record.char_encoding = 0;
				log_record.buf_message	 = _message;
				log_record.origin		 = 0;
				log_record.attribute	 = 0;
				log_record.source		 = 0;
				log_record.destination	 = 0;
				log_record.time_occure	 = DateTime.UtcNow;

				// - 전달하기
				log_targetable.trace(log_record);
			}
		}

		// Default Trace) 
		public	static Ilog_targetable	default_logger { get {	return g_default_tracer;}	set { g_default_tracer = value; } }
		private static Ilog_targetable	g_default_tracer = null;
	}																						  
}

