using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CGDK.server.system.winform_controls
{
	public class ITEM_LOG : INotifyPropertyChanged
	{
		public ITEM_LOG()
		{
			Header		 = "";
			Log			 = "";
			TextColor	 = Color.Black;
			IsBold		 = false;
		}
		public ITEM_LOG(string _header, string _log)
		{
			Header		 = _header;
			Log			 = _log;
			TextColor	 = Color.Black;
			IsBold		 = false;
		}
		public ITEM_LOG(string _header, string _log, Color _color, bool _is_bold = false)
		{
			Header		 = _header;
			Log			 = _log;
			TextColor	 = _color;
			IsBold		 = _is_bold;
		}

		public string				Header
		{
			get
			{
				return m_Header;
			}
			set
			{
				if (m_Header != value)
				{
					m_Header = value;
					OnPropertyChanged("Header");
				}
			}
		}
		public string				Log
		{
			get
			{
				return m_Log;
			}
			set
			{
				if (m_Log != value)
				{
					m_Log = value;
					OnPropertyChanged("Log");
				}
			}
		}

		public Color				TextColor
		{
			get
			{
				return m_color_text;
			}
			set
			{
				if (m_color_text != value)
				{
					m_color_text = value;
					OnPropertyChanged("TextColor");
				}
			}
		}
		public bool					IsBold
		{
			get
			{
				return m_is_bold;
			}
			set
			{
				if (m_is_bold != value)
				{
					m_is_bold = value;
					OnPropertyChanged("IsBold");
				}
			}
		}

		public override string		ToString()
		{
			return Log;
		}

		#region INotifyPropertyChanged Members

		public event				PropertyChangedEventHandler PropertyChanged;

		private void				OnPropertyChanged(string propertyName)
		{
			var handler	 = PropertyChanged;

			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

		private string				m_Header;
		private string				m_Log;
		private	Color				m_color_text;
		private bool				m_is_bold;
	}

	public class ListBoxLog : ListBox
	{
		public ListBoxLog()
		{
			DrawMode	 = DrawMode.OwnerDrawFixed;
			ItemHeight	 = 15;

			m_cs_list	 = new object();

			m_font		 = new Font("Tahoma", 8.0f, FontStyle.Regular);
			m_font_bold	 = new Font("Calibri", 9.0f, FontStyle.Bold);
		}

		protected override void			OnDrawItem(DrawItemEventArgs _e)
		{
			const TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter;

			if (_e.Index >= 0 && _e.Index < Items.Count)
			{
				_e.DrawBackground();

				//_e.Graphics.DrawRectangle(Pens.Red, 2, _e.Bounds.Y + 2, 14, 14); // Simulate an icon.

				var item = Items[_e.Index] as ITEM_LOG;

				// Header
				var textRect		 = _e.Bounds;
				textRect.Width		 = 120;
				string textHeader	 = DesignMode ? "CGCII Log ListBox" : item.Header;
				TextRenderer.DrawText(_e.Graphics, textHeader, m_font, textRect, Color.Gray, flags);

				// Log
				textRect			 = _e.Bounds;
				textRect.X			 += 120;
				textRect.Width		 -= 120;
				string textLog		 = DesignMode ? "" : item.Log;
				TextRenderer.DrawText(_e.Graphics, textLog, item.IsBold ? m_font_bold : m_font, textRect, item.TextColor, flags);

				_e.DrawFocusRectangle();
			}
		}

		public	int						max_log_count
		{
			get	
			{
				return m_count_max_log;
			}
			set
			{
				m_count_max_log = value;
			}
		}

		public void						AddString(ITEM_LOG _item_log)
		{
			lock(m_cs_list)
			{
				// 1) 만약 최대 Log수 보다 많으면...
				if(Items.Count >= m_count_max_log)
				{
					Items.RemoveAt(0);
				}

				// 2) 추가한다.
				Items.Add(_item_log);
			}

			// 3) Scroll하기
			TopIndex = Items.Count - 1;
		}
		public void						AddString(DateTime _timeLog, string _str_message, Color _color_text, bool _is_bold)
		{
			ITEM_LOG item_log = new ITEM_LOG();

			item_log.Header = String.Format("[{0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}]",
				_timeLog.Year, 
				_timeLog.Month, 
				_timeLog.Day, 
				_timeLog.Hour, 
				_timeLog.Minute, 
				_timeLog.Second
				);
			item_log.Log = _str_message;
			item_log.TextColor = _color_text;
			item_log.IsBold = _is_bold;

			// return) 
			AddString(item_log);
		}
		public void						AddString(string _strHead, string _str_message, Color _color_text, bool _is_bold)
		{
			var item_log = new ITEM_LOG();
			item_log.Header = _strHead;
			item_log.Log = _str_message;
			item_log.TextColor = _color_text;
			item_log.IsBold = _is_bold;

			// return) 
			AddString(item_log);
		}

		private object					m_cs_list;

		private	Font					m_font;
		private	Font					m_font_bold;

		private	int						m_count_max_log	 = 2048;
	}
}