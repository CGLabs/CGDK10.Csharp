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
// class CGDK.Server.log_file
//
//
//
// ----------------------------------------------------------------------------
namespace CGDK.Server
{
	public class LoggerFile : NLogTargetable
	{
	// publics) 
		public bool						Initialize(string _filename = null)
		{
			// 1) MSG객체와 Context객체를 생성한다.
			var Context_now = new Context();

			// 2) Filename을 설정한다.
			if(_filename != null)
				Context_now["filename"] = _filename;

			// return) 
			return base.Initialize(Context_now);
		}
		public bool						Start(string _filename = null)
		{
			// 1) MSG객체와 Context객체를 생성한다.
			var Context_now = new Context();

			// 2) Filename을 설정한다.
			if(_filename != null)
				Context_now["filename"] = _filename;

			// return) 
			return base.Start(Context_now);
		}

	// Messages) 
		public override void			ProcessLog(LOG_RECORD _log_record)
		{
			// check)
			Debug.Assert(_log_record != null);

			// check) 
			if(_log_record == null)
				return;

			// check) 
			if(this.Now < eOBJECT_STATE.STOPPED && this.Now > eOBJECT_STATE.RUNNING)
				return;

			var	file_log = this.m_file_log;

			// check) Log File이 설정되지 않았으면 그냥 끝낸다.
			if(file_log == null)
				return;

			// 2) Log Type을 얻는다.
			int	Type = ((int)_log_record.Type) & 0x00ff;

			// 3) filtering
			if(Filter != null)
			{
				// - filtering result
				var filter_result = Filter.ProcessFiltering(_log_record);

				// check) 
				if(filter_result == false)
					return;
			}

			// Declare) 
			string str_write;

			// 3) Continue이냐 아니냐에 따라...
			if(((int)_log_record.Type & (int)eLOG_TYPE.CONTINUE) == 0)
			{
				// - Type에 따라 View를 update한다.
				if(Type >= (int)eLOG_TYPE.INFO && Type < (int)eLOG_TYPE.MAX)
				{
					++this.m_count_log[Type];
				}

				// - Message를 작성한다.
				str_write = String.Format("[{0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}] {6}",
					_log_record.timeOccure.Year, 
					_log_record.timeOccure.Month, 
					_log_record.timeOccure.Day, 
					_log_record.timeOccure.Hour, 
					_log_record.timeOccure.Minute, 
					_log_record.timeOccure.Second,
					_log_record.Message
					);
			}
			else
			{
				str_write = String.Format("                      {0}",
					_log_record.Message
					);
			}

			// 4) 써넣는다.
			file_log.Write(str_write);

			// 5) Total Count를 더한다.
			++this.m_count_log_total;
		}

	// implementations) 
		protected override void			_ProcessNotifyInitializing	(object _object, Context _Context)
		{
			base._ProcessNotifyInitializing(_object, _Context);
		}
		protected override void			_ProcessNotifyInitialize (object _object, Context _Context)
		{
			// 1) File을 생성한다.
			var file_log = new archive_file();

			// Declare)
			var Context_now = _Context;

			// 2) Filename을 읽는다.
			string str_filename = Context_now["filename"];

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
			var result = file_log.Start(str_filename);

			// check) file을 열지 못했으면 System.Exception을 던진다.
			if(result == false)
			{			
			    throw new System.Exception();
			}

			// 4) File을 Setting한다.
			this.m_file_log = file_log;

			// 5) ...
			base._ProcessNotifyInitialize(_object, _Context);
		}
		protected override void			_ProcessNotifyDestroying		(object _object)
		{
			// 1) Base의 ProcessNotifyStopping을 호출한다.
			base._ProcessNotifyDestroying(_object);

			// 2) 현재의 File을 얻는다.
			var file_log = this.m_file_log;
			this.m_file_log = null;

			// check) 
			if(file_log == null)
			{
			    return;
			}

			// 3) File을 Close한다.
			file_log.Destroy();
		}
		protected override void			_ProcessNotifyDestroy			(object _object)
		{
			base._ProcessNotifyDestroy(_object);
		}

		private	archive_file			m_file_log;
	}
}