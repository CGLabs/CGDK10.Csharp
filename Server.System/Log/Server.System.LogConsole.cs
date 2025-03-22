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
	public class LoggerConsole : NLogTargetable
	{
	// constructor)
		public override void		ProcessLog(LOG_RECORD _log_record)
		{
			// check) _log_record must be not null
			if(_log_record == null)
				return;

			// check) message must be not empty
			if(_log_record.Message.Length == 0)
				return;

			// check) 
			if(this.Now< eOBJECT_STATE.STOPPED && this.Now > eOBJECT_STATE.RUNNING)
				return;

			// 1) filtering
			if(this.Filter != null)
			{
				// - filtering result
				var filter_result = this.Filter.ProcessFiltering(_log_record);

				// check) 
				if(filter_result == false)
					return;
			}

			// 2) log Type
			var log_type = (eLOG_TYPE)(((int)_log_record.Type) & 0xffff);
			var log_not_continue = (((int)_log_record.Type) & ((int)eLOG_TYPE.CONTINUE)) == 0;

			// declare)
			ConsoleColor text_color = ConsoleColor.Gray;

			// 3) select text color
			switch (log_type)
			{
			case	eLOG_TYPE.INFO:
					text_color = (_log_record.Level < (int)eLOG_LEVEL.HIGHER) ? ConsoleColor.Gray : ConsoleColor.White;
					break;

			case	eLOG_TYPE.PROGRESS:
					text_color = (_log_record.Level < (int)eLOG_LEVEL.HIGHER) ? ConsoleColor.Green : ConsoleColor.Green;
					break;

			case	eLOG_TYPE.DEBUG:
					text_color = ConsoleColor.DarkGray;
					break;

			case	eLOG_TYPE.EXCEPTION:
					text_color = ConsoleColor.DarkRed;
					break;

			case	eLOG_TYPE.ERROR:
					text_color = (_log_record.Level < (int)eLOG_LEVEL.HIGHER) ? ConsoleColor.DarkRed : ConsoleColor.Red;
					break;

			case	eLOG_TYPE.WARNING:
					text_color = (_log_record.Level < (int)eLOG_LEVEL.HIGHER) ? ConsoleColor.DarkMagenta : ConsoleColor.Magenta;
					break;

			case	eLOG_TYPE.USER:
					text_color = (_log_record.Level < (int)eLOG_LEVEL.HIGHER) ? ConsoleColor.DarkYellow : ConsoleColor.Yellow;
					break;

			case	eLOG_TYPE.SYSTEM:
					text_color = (_log_record.Level < (int)eLOG_LEVEL.HIGHER) ? ConsoleColor.DarkBlue : ConsoleColor.Blue;
					break;

			default:
					break;
			}

			// 4) log count
			lock(m_cs_count_log)
			{
				// - log의 갯수를 계산한다.
				++m_count_log_total;

				// - log Type의 수를 증가시킨다.
				if ((int)log_type >= (int)eLOG_TYPE.INFO && (int)log_type < (int)eLOG_TYPE.MAX)
				{
					++m_count_log[(int)log_type];
				}
			}

			// 5) 출력한다.
			{
				// - line수를 읽어들인다.
				var line_count = 1;
					
				lock (m_cs_console)
				{
					for(var i = 0; i < line_count; ++i)
					{
						// - 날짜와 시간을 출력한다.
						if (log_not_continue && i == 0)
						{
							// - 출력한다.
							Console.ForegroundColor = ConsoleColor.DarkGray;
							Console.Write("[{0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}] ",
								_log_record.timeOccure.Year,
								_log_record.timeOccure.Month,
								_log_record.timeOccure.Day,
								_log_record.timeOccure.Hour,
								_log_record.timeOccure.Minute,
								_log_record.timeOccure.Second
							);
							Console.ForegroundColor = ConsoleColor.Gray;
						}
						else
						{
							// - 빈칸을 출력한다.
							//           "[0000/00/00 00:00:00] "
							Console.Write("                      ");
						}

						// - 로그 메시지를 출력한다.
						Console.ForegroundColor = text_color;
						Console.Write(_log_record.Message);
						Console.ForegroundColor = ConsoleColor.Gray;
						Console.Write("\n");
					}
				}
			}
		}

		protected override void			_ProcessNotifyInitializing (object _object, Context _Context)
		{
			base._ProcessNotifyInitializing(_object, _Context);
		}
		protected override void			_ProcessNotifyInitialize (object _object, Context _Context)
		{
			base._ProcessNotifyInitialize(_object, _Context);
		}
		protected override void			_ProcessNotifyDestroying (object _object)
		{
			base._ProcessNotifyDestroying(_object);
		}
		protected override void			_ProcessNotifyDestroy (object _object)
		{
			base._ProcessNotifyDestroy(_object);
		}

		protected	object				m_cs_console = new();
	}
}