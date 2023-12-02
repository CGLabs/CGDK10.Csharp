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
using CGDK;

// ----------------------------------------------------------------------------
//
// class CGDK.server.log_file
//
//
//
// ----------------------------------------------------------------------------
namespace CGDK.server
{
	public class log_file :	Nlog_targetable
	{
	// publics) 
		public bool						initialize(string _filename = null)
		{
			// 1) MSG객체와 Context객체를 생성한다.
			var context_now = new context();

			// 2) Filename을 설정한다.
			if(_filename != null)
			{
				context_now["filename"] = _filename;
			}

			// return) 
			return base.initialize(context_now);
		}
		public bool						start(string _filename = null)
		{
			// 1) MSG객체와 Context객체를 생성한다.
			var context_now = new context();

			// 2) Filename을 설정한다.
			if(_filename != null)
			{
				context_now["filename"] = _filename;
			}

			// return) 
			return base.start(context_now);
		}

	// Messages) 
		public override void			process_log(LOG_RECORD _log_record)
		{
			// check)
			Debug.Assert(_log_record != null);

			// check) 
			if(_log_record == null)
				return;

			// check) 
			if(object_state < eOBJECT_STATE.STOPPED && object_state > eOBJECT_STATE.RUNNING)
				return;

			var	file_log = m_file_log;

			// check) Log File이 설정되지 않았으면 그냥 끝낸다.
			if(file_log == null)
				return;

			// 2) Log Type을 얻는다.
			int	type = ((int)_log_record.type) & 0x00ff;

			// 3) filtering
			if(filter != null)
			{
				// - filtering result
				var filter_result = filter.process_filtering(_log_record);

				// check) 
				if(filter_result == false)
					return;
			}

			// Declare) 
			string str_write;

			// 3) Continue이냐 아니냐에 따라...
			if(((int)_log_record.type & (int)eLOG_TYPE.CONTINUE) == 0)
			{
				// - Type에 따라 View를 update한다.
				if(type >= (int)eLOG_TYPE.INFO && type < (int)eLOG_TYPE.MAX)
				{
					++m_count_log[type];
				}

				// - Message를 작성한다.
				str_write = String.Format("[{0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}] {6}",
					_log_record.time_occure.Year, 
					_log_record.time_occure.Month, 
					_log_record.time_occure.Day, 
					_log_record.time_occure.Hour, 
					_log_record.time_occure.Minute, 
					_log_record.time_occure.Second,
					_log_record.buf_message
					);
			}
			else
			{
				str_write = String.Format("                      {0}",
					_log_record.buf_message
					);
			}

			// 4) 써넣는다.
			file_log.write(str_write);

			// 5) Total Count를 더한다.
			++m_count_log_total;
		}

	// implementations) 
		protected override void			_process_notify_initializing	(object _object, context _context)
		{
			base._process_notify_initializing(_object, _context);
		}
		protected override void			_process_notify_initialize		(object _object, context _context)
		{
			// 1) File을 생성한다.
			var file_log = new archive_file();

			// Declare)
			var context_now = _context;

			// 2) Filename을 읽는다.
			string str_filename = context_now["filename"];

			// 3) Filename이 설정되어 있지 않으면 기본 filename을 설정한다.
			if(str_filename == null)
			{
				// - 현재 시간을 구한다.
				var	date_now = System.DateTime.Now;

				// - File이름을 설정한다.
				str_filename = String.Format("Log@{0:0000}-{1:00}-{2:00}@{3:00}_{4:00}_{5:00}.log", 
					date_now.Year,
					date_now.Month,
					date_now.Day,
					date_now.Hour,
					date_now.Minute,
					date_now.Second);
			}

			// 3) File을 Open한다.
			var result = file_log.start(str_filename);

			// check) file을 열지 못했으면 System.Exception을 던진다.
			if(result == false)
			{			
			    throw new System.Exception();
			}

			// 4) File을 Setting한다.
			m_file_log = file_log;

			// 5) ...
			base._process_notify_initialize(_object, _context);
		}
		protected override void			_process_notify_destroying		(object _object)
		{
			// 1) Base의 ProcessNotifyStopping을 호출한다.
			base._process_notify_destroying(_object);

			// 2) 현재의 File을 얻는다.
			var file_log = m_file_log;
			m_file_log = null;

			// check) 
			if(file_log == null)
			{
			    return;
			}

			// 3) File을 Close한다.
			file_log.destroy();
		}
		protected override void			_process_notify_destroy			(object _object)
		{
			base._process_notify_destroy(_object);
		}

		private	archive_file			m_file_log;
	}
}