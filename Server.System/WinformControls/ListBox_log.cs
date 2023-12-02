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

	public class ListBoxLog : ListBox
	{
		public ListBoxLog()
		{
			DrawMode	 = DrawMode.OwnerDrawFixed;
			ItemHeight	 = 15;

			m_csList	 = new object();

			m_Font		 = new Font("Tahoma", 8.0f, FontStyle.Regular);
			m_FontBold	 = new Font("Calibri", 9.0f, FontStyle.Bold);
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

		private readonly object			m_csList;

		private	readonly Font			m_Font;
		private	readonly Font			m_FontBold;

		private	int						m_MaxLogCount	 = 2048;
	}
}