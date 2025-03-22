using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace CGDK.Server.System.WinformControls
{
	public class ItemLog : INotifyPropertyChanged
	{
		public ItemLog()
		{
		}
		public ItemLog(string HeaderString, string log)
		{
			this.m_Header		 = HeaderString;
			this.m_Log			 = log;
		}
		public ItemLog(string HeaderString, string log, Color color, bool IsBold = false)
		{
			this.m_Header		 = HeaderString;
			this.m_Log			 = log;
			this.m_TextColor	 = color;
			this.m_IsBold		 = IsBold;
		}

		public string				Header
		{
			get
			{
				return this.m_Header;
			}
			set
			{
				if (this.m_Header != value)
				{
					this.m_Header = value;
					this.OnPropertyChanged(nameof(Header));
				}
			}
		}
		public string				Log
		{
			get
			{
				return this.m_Log;
			}
			set
			{
				if (this.m_Log != value)
				{
					this.m_Log = value;
					this.OnPropertyChanged(nameof(Log));
				}
			}
		}

		public Color				TextColor
		{
			get
			{
				return this.m_TextColor;
			}
			set
			{
				if (this.m_TextColor != value)
				{
					this.m_TextColor = value;
					OnPropertyChanged(nameof(TextColor));
				}
			}
		}
		public bool					IsBold
		{
			get
			{
				return this.m_IsBold;
			}
			set
			{
				if (this.m_IsBold != value)
				{
					this.m_IsBold = value;
					this.OnPropertyChanged(nameof(IsBold));
				}
			}
		}

		public override string		ToString()
		{
			return this.Log;
		}

		#region INotifyPropertyChanged Members

		public event				PropertyChangedEventHandler? PropertyChanged;

		private void				OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

		private string				m_Header = "";
		private string				m_Log = "";
		private	Color				m_TextColor = Color.Black;
		private bool				m_IsBold;
	}

	public class ListBoxLog : ListBox, ILogTargetable
	{
		public ListBoxLog()
		{
			DrawMode	 = DrawMode.OwnerDrawFixed;
			ItemHeight	 = 15;

			m_csList	 = new object();

			m_Font		 = new Font("Tahoma", 8.0f, FontStyle.Regular);
			m_FontBold	 = new Font("Calibri", 9.0f, FontStyle.Bold);

			m_count_log_total = 0;
			m_count_log = new int[(int)eLOG_TYPE.MAX + 1];
			for (int i = 0; i < m_count_log.Length; ++i)
			{
				this.m_count_log[i] = 0;
			}
		}
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (!disposing)
			{
				this.m_Font.Dispose();
				this.m_FontBold.Dispose();
			}
		}

		protected override void			OnDrawItem(DrawItemEventArgs e)
		{
			// check) 
			if (e == null)
				return;

			const TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter;

			if (e.Index >= 0 && e.Index < Items.Count)
			{
				e.DrawBackground();

				//e.Graphics.DrawRectangle(Pens.Red, 2, e.Bounds.Y + 2, 14, 14); // Simulate an icon.

				var item = Items[e.Index] as ItemLog;

				// check)
				Debug.Assert(item != null);

				// Header
				var textRect		 = e.Bounds;
				textRect.Width		 = 120;
				string textHeader	 = DesignMode ? "CGCII Log ListBox" : item.Header;
				TextRenderer.DrawText(e.Graphics, textHeader, m_Font, textRect, Color.Gray, flags);

				// Log
				textRect			 = e.Bounds;
				textRect.X			 += 120;
				textRect.Width		 -= 120;
				string textLog		 = DesignMode ? "" : item.Log;
				TextRenderer.DrawText(e.Graphics, textLog, item.IsBold ? m_FontBold : m_Font, textRect, item.TextColor, flags);

				e.DrawFocusRectangle();
			}
		}

		public	int						MaxLogCount
		{
			get	
			{
				return m_MaxLogCount;
			}
			set
			{
				m_MaxLogCount = value;
			}
		}

		public void						AddString(ItemLog Item)
		{
			lock(m_csList)
			{
				// 1) 만약 최대 Log수 보다 많으면...
				if(Items.Count >= m_MaxLogCount)
				{
					Items.RemoveAt(0);
				}

				// 2) 추가한다.
				Items.Add(Item);
			}

			// 3) Scroll하기
			TopIndex = Items.Count - 1;
		}
		public void						AddString(DateTime LogTime, string MessageString, Color TextColor, bool IsBold)
		{
			ItemLog ItemLog = new()
			{
				Header = string.Format("[{0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}]",
					LogTime.Year,
					LogTime.Month,
					LogTime.Day,
					LogTime.Hour,
					LogTime.Minute,
					LogTime.Second
					),
				Log = MessageString,
				TextColor = TextColor,
				IsBold = IsBold
			};

			// return) 
			AddString(ItemLog);
		}
		public void						AddString(string HeaderString, string MessageString, Color TextColor, bool IsBold)
		{
			var ItemLog = new ItemLog
			{
				Header = HeaderString,
				Log = MessageString,
				TextColor = TextColor,
				IsBold = IsBold
			};

			// return) 
			this.AddString(ItemLog);
		}

		public void						Trace(LOG_RECORD _log_record) { this.ProcessLog(_log_record); }
		public							ILogFilter Filter { get { return this.m_filter_log; } set { this.m_filter_log = value; } }
		public ulong					LogCount { get { return this.m_count_log_total; } set { this.m_count_log_total = value; } }
		public void						ProcessLog(LOG_RECORD _log_record)
		{
			// check) _log_record must be not null
			if (_log_record == null)
				return;

			// check) message must be not empty
			if (_log_record.Message.Length == 0)
				return;

			// check) 
			if (this.Now < eOBJECT_STATE.STOPPED && this.Now > eOBJECT_STATE.RUNNING)
				return;

			// 1) filtering
			if (this.Filter != null)
			{
				// - filtering result
				var filter_result = this.Filter.ProcessFiltering(_log_record);

				// check) 
				if (filter_result == false)
					return;
			}

			// 2) log Type
			var log_type = (eLOG_TYPE)(((int)_log_record.Type) & 0xffff);
			var log_not_continue = (((int)_log_record.Type) & ((int)eLOG_TYPE.CONTINUE)) == 0;

			// declare)
			ConsoleColor text_color = ConsoleColor.Gray;

			//// 3) select text color
			//switch (log_type)
			//{
			//	case eLOG_TYPE.INFO:
			//		text_color = (_log_record.Level < (int)eLOG_LEVEL.HIGHER) ? ConsoleColor.Gray : ConsoleColor.White;
			//		break;

			//	case eLOG_TYPE.PROGRESS:
			//		text_color = (_log_record.Level < (int)eLOG_LEVEL.HIGHER) ? ConsoleColor.Green : ConsoleColor.Green;
			//		break;

			//	case eLOG_TYPE.DEBUG:
			//		text_color = ConsoleColor.DarkGray;
			//		break;

			//	case eLOG_TYPE.EXCEPTION:
			//		text_color = ConsoleColor.DarkRed;
			//		break;

			//	case eLOG_TYPE.ERROR:
			//		text_color = (_log_record.Level < (int)eLOG_LEVEL.HIGHER) ? ConsoleColor.DarkRed : ConsoleColor.Red;
			//		break;

			//	case eLOG_TYPE.WARNING:
			//		text_color = (_log_record.Level < (int)eLOG_LEVEL.HIGHER) ? ConsoleColor.DarkMagenta : ConsoleColor.Magenta;
			//		break;

			//	case eLOG_TYPE.USER:
			//		text_color = (_log_record.Level < (int)eLOG_LEVEL.HIGHER) ? ConsoleColor.DarkYellow : ConsoleColor.Yellow;
			//		break;

			//	case eLOG_TYPE.SYSTEM:
			//		text_color = (_log_record.Level < (int)eLOG_LEVEL.HIGHER) ? ConsoleColor.DarkBlue : ConsoleColor.Blue;
			//		break;

			//	default:
			//		break;
			//}

			// 4) log count
			lock (m_cs_count_log)
			{
				// - log의 갯수를 계산한다.
				++m_count_log_total;

				// - log Type의 수를 증가시킨다.
				if ((int)log_type >= (int)eLOG_TYPE.INFO && (int)log_type < (int)eLOG_TYPE.MAX)
				{
					++m_count_log[(int)log_type];
				}
			}

			// 5) string 을 추기한다.


			//// 5) 출력한다.
			//{
			//	// - line수를 읽어들인다.
			//	var line_count = 1;

			//	lock (m_cs_console)
			//	{
			//		for (var i = 0; i < line_count; ++i)
			//		{
			//			// - 날짜와 시간을 출력한다.
			//			if (log_not_continue && i == 0)
			//			{
			//				// - 출력한다.
			//				Console.ForegroundColor = ConsoleColor.DarkGray;
			//				Console.Write("[{0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}] ",
			//					_log_record.timeOccure.Year,
			//					_log_record.timeOccure.Month,
			//					_log_record.timeOccure.Day,
			//					_log_record.timeOccure.Hour,
			//					_log_record.timeOccure.Minute,
			//					_log_record.timeOccure.Second
			//				);
			//				Console.ForegroundColor = ConsoleColor.Gray;
			//			}
			//			else
			//			{
			//				// - 빈칸을 출력한다.
			//				//           "[0000/00/00 00:00:00] "
			//				Console.Write("                      ");
			//			}

			//			// - 로그 메시지를 출력한다.
			//			Console.ForegroundColor = text_color;
			//			Console.Write(_log_record.Message);
			//			Console.ForegroundColor = ConsoleColor.Gray;
			//			Console.Write("\n");
			//		}
			//	}
			//}
		}

		private readonly object			m_csList;

		private	readonly Font			m_Font;
		private	readonly Font			m_FontBold;

		protected ulong					m_count_log_total = 0;
		protected int[]					m_count_log;
		protected object				m_cs_count_log = new();
		private ILogFilter				m_filter_log;
		private int						m_MaxLogCount = 2048;
	}
}