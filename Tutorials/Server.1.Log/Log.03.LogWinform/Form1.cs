using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CGDK;

namespace tutorial.server.log._03.log_winform.NET
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void OnLoad(object sender, EventArgs e)
		{
			// 1) 다음과 같이 log를 출력할 수 있다.
			LOG.INFO				(logview_main, "[A] This paragraph is log");
			LOG.INFO				(logview_main, "[A-1] INFO Important Log");
			LOG.INFO_IMPORTANT		(logview_main, "[A-2] Important INFO Important Log");
			LOG.PROGRESS			(logview_main, "[A-3] PROGRESS Log (1)");
			LOG.PROGRESS_IMPORTANT	(logview_main, "[A-5] Important PROGRESS Log");
			LOG.ERROR				(logview_main, "[A-6] ERROR Log");
			LOG.EXCEPTION			(logview_main, "[A-7] EXCEPTION Log");
			LOG.WARNING				(logview_main, "[A-8] WARNING Log");
			LOG.DEBUG				(logview_main, "[A-9] DEBUG Log");
			LOG.USER				(logview_main, "[A-10] USER Log");
			LOG.SYSTEM				(logview_main, "[A-11] SYSTEM Log");
			LOG.INFO				(logview_main, "[A-12] 이 문장은 멀티바이트 한글 문자열 로그 [" + "STRING" + "]");
		}
	}
}
