using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using CGDK;
using CGDK.Server.System.WinformControls;

namespace CGDK.Server.System.WinformControls
{
	public partial class LogView : UserControl, IMessageable
    {
	// Constructor)
        public LogView()
        {
            InitializeComponent();

			m_colorTextLast = Color.DarkGray;
			m_bBoldLast = false;

			m_colorText = new Color[(int)eLOG_TYPE.MAX];
			m_bBold = new bool[(int)eLOG_TYPE.MAX];

			// 4) Default Color를 설정한다.
			m_colorText[(int)eLOG_TYPE.INFO]		 = Color.Black;
			m_bBold[(int)eLOG_TYPE.INFO]			 = false;

			m_colorText[(int)eLOG_TYPE.PROGRESS]	 = Color.FromArgb(0, 0x6f, 0);
			m_bBold[(int)eLOG_TYPE.PROGRESS]		 = false;

			m_colorText[(int)eLOG_TYPE.DEBUG]		 = Color.FromArgb(0x7f, 0x7f, 0x7f);
			m_bBold[(int)eLOG_TYPE.DEBUG]			 = false;

			m_colorText[(int)eLOG_TYPE.EXCEPTION]	 = Color.FromArgb(0x9f, 0, 0);
			m_bBold[(int)eLOG_TYPE.EXCEPTION]		 = true;

			m_colorText[(int)eLOG_TYPE.ERROR]		 = Color.FromArgb(0xff, 0, 0);
			m_bBold[(int)eLOG_TYPE.ERROR]			 = false;

			m_colorText[(int)eLOG_TYPE.USER]		 = Color.Black;
			m_bBold[(int)eLOG_TYPE.USER]			 = false;

			m_colorText[(int)eLOG_TYPE.SYSTEM]		 = Color.FromArgb(0, 0, 0xff);
			m_bBold[(int)eLOG_TYPE.SYSTEM]			 = true;

			// 5) 
			m_countTotal = 0;
			m_countLog	 = new int[(int)eLOG_TYPE.MAX+1];
			for(int i=0;i<m_countLog.Length; ++i)
			{
				m_countLog[i] = 0;
			}

			m_filterLog = null;
		}

	// Implementations)
		private void					LogView_Load(object sender, EventArgs e)
		{
		}
		private void					ListBoxLogView_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		// Definition) 
		delegate int	delegateProcessMessage(object _source, sMESSAGE _msg);

		public int						ProcessMessage(object _source, sMESSAGE _msg)
		{
			if(_msg.message == eMESSAGE.SYSTEM.LOG && _msg.param != null)
			{
				// Check) Invoke 
				if(InvokeRequired)
				{
					Invoke(new delegateProcessMessage(ProcessMessage), new object[] {_source, _msg});
					return	0;
				}

				// 1) Log Record를 얻는다.
				var	logRecord = _msg.param as LOG_RECORD;

				// Check)
				Debug.Assert(logRecord != null);

				// 2) Log 출력
				int	iType = ((int)logRecord.Type) & 0x00ff;

				// Check)
				if(m_filterLog != null)
				{

				}

				// 2) Log Time을 만든다.
				if(((int)logRecord.Type & (int)eLOG_TYPE.CONTINUE) == 0)
				{
					// Declare)
					Color TextColor	= Color.DarkGray;
					bool bBold = false;

					// - Type에 따라 View를 update한다.
					if(iType >= (int)eLOG_TYPE.INFO && iType < (int)eLOG_TYPE.MAX)
					{
						TextColor = m_colorText[iType];
						bBold = (logRecord.Level >= eLOG_LEVEL.HIGHER) || m_bBold[iType];

						++m_countLog[iType];
					}
					else
					{
						TextColor = Color.DarkGray;
						bBold = false;
					}

					// - 최후의 색상을 저장한다.
					m_colorTextLast = TextColor;
					m_bBoldLast = bBold;

					// - 추가한다.
					listBoxLogView.AddString(logRecord.timeOccure.ToLocalTime(), logRecord.Message, TextColor, bBold);

					UpdateStatistics(GetM_countLog(), GetM_countTotal());
				}
				else
				{
					listBoxLogView.AddString("", logRecord.Message, m_colorTextLast, m_bBoldLast);
				}

				// Total Count를 더한다.
				++m_countTotal;


				//// 3) Log Message
				//pItem.pLogRecord	 = _pLogRecord;

				// 4) PUSH!!
				//lock(m_csqueueString)
				//{
				//	// - Message를 Push한다.
				//	m_pqueueStringStack->pqueue.push_back(ITEM_LOG(_pLogRecord, pItem));
				//	++m_pqueueStringStack->count[iType];

				//	// Check) 1개이상 Queuing되었을 때만 PostMessage한다.
				//	RETURN_IF(m_pqueueStringStack->pqueue.size() > 1, );
				//}

				//// 5) Post Message
				//PostMessage(WM_ADD_STRING, 0, 0);
			}

			return	0;
		}

		private int[] GetM_countLog()
		{
			return m_countLog;
		}

		private int GetM_countTotal()
		{
			return m_countTotal;
		}

		private void					UpdateStatistics(int[] _countLog, int _countTotal)
		{
			//private System.Windows.Forms.Button button1;
			//private System.Windows.Forms.Label lableLogTotal;
			//private System.Windows.Forms.Label lableLogSystem;
			//private System.Windows.Forms.Label lableLogProgress;
			//private System.Windows.Forms.Label lableLogInfo;
			//private System.Windows.Forms.Label lableLogException;
			//private System.Windows.Forms.Label lableLogError;
			//private System.Windows.Forms.Label lableLogDebug;
			//private System.Windows.Forms.Label lableLogEtc;
			//private System.Windows.Forms.Label label17;
			//private System.Windows.Forms.Label labelFilter;
			//private System.Windows.Forms.Button buttonEditFilter;
			//private System.Windows.Forms.Button buttonLogClose;
			//private System.Windows.Forms.Button buttonLogSave;
			//private System.Windows.Forms.Button buttonLogSaveSelected;
			//private System.Windows.Forms.Label	label19;
			//private System.Windows.Forms.Label	lableLogCount;
			//private ListBoxLog					listBoxLogView;
			//private System.Windows.Forms.TabControl tabLog;
			//private System.Windows.Forms.TabPage tabPage1;
			//private System.Windows.Forms.TabPage tabPage2;
			//private System.Windows.Forms.Label label21;
			//private System.Windows.Forms.Label label22;
			//private System.Windows.Forms.Label label23;
			//private System.Windows.Forms.Label label24;
			//private System.Windows.Forms.Label label25;

			// check)
			Debug.Assert(_countLog != null);

			lableLogTotal.Text = _countTotal.ToString();
	
			if(_countLog[(int)eLOG_TYPE.SYSTEM]!=0)
				lableLogSystem.Text = _countLog[(int)eLOG_TYPE.SYSTEM].ToString();

			if(_countLog[(int)eLOG_TYPE.PROGRESS]!=0)
				lableLogProgress.Text = _countLog[(int)eLOG_TYPE.PROGRESS].ToString();

			if(_countLog[(int)eLOG_TYPE.INFO]!=0)
				lableLogInfo.Text = _countLog[(int)eLOG_TYPE.INFO].ToString();

			if(_countLog[(int)eLOG_TYPE.EXCEPTION]!=0)
				lableLogException.Text = _countLog[(int)eLOG_TYPE.EXCEPTION].ToString();

			if(_countLog[(int)eLOG_TYPE.ERROR]!=0)
				lableLogError.Text = _countLog[(int)eLOG_TYPE.ERROR].ToString();

			if(_countLog[(int)eLOG_TYPE.DEBUG]!=0)
				lableLogDebug.Text = _countLog[(int)eLOG_TYPE.DEBUG].ToString();

			//if(_countLog[(int)eLOG_TYPE.ETC]!=0)
			//	lableLogEtc.Text = _countLog[(int)eLOG_TYPE.ETC].ToString();
		}

		private	ILogFilter?				m_filterLog;
		private	int						m_countTotal;
		private readonly int[]			m_countLog;
		private	Color					m_colorTextLast;
		private	bool					m_bBoldLast;
		readonly private Color[]		m_colorText;
		readonly private bool[]			m_bBold;
	}
}
