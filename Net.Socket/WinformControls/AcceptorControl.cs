using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CGDK.Net.Io.Connective;

namespace CGSocketWinformControls
{
    public partial class CAcceptorControl : UserControl
    {
        public CAcceptorControl()
        {
			// 1) Component를 초기화한다.
            this.InitializeComponent();
        }

		private void CAcceptorControl_Load(object sender, EventArgs e)
		{
			// 1) Timer를 시작한다.
			this.m_timerUpdate.Tick += new(OnTimer_Update);
			this.m_timerUpdate.Interval = 1000;

			// 2) Control설정
			this.EnableControls(this.m_acceptor != null);
		}

		private	void EnableControls(bool _Enable)
		{
			// Check) 설정이 동일하면 그냥 끝낸다.
			if(this.m_comboBindAddress.Enabled == _Enable)
				return;

			this.m_comboBindAddress.Enabled			 = _Enable;
			this.m_textBindPort.Enabled				 = _Enable;
			this.m_checkAcceptorStart.Enabled		 = _Enable;
			this.m_buttonAcceptorStatus.Enabled		 = _Enable;
			this.m_buttonAcceptorCloseAll.Enabled	 = _Enable;
			label1.Enabled						 = _Enable;
		}

        private void BindAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void AcceptorStart_CheckedChanged(object sender, EventArgs e)
        {
			// Declare) 
			CGDK.Net.Io.Connective.IAcceptor? tempacceptor = null;

			// 1) 현재 Acceptor를 얻는다.
			lock(m_csAcceptor)
			{
				tempacceptor = this.m_acceptor;
			}

			// Check) acceptor가 null이면 끝낸다.
			if(tempacceptor == null)
				return;

			// Case Checked) Checked이면 시작한다.
			if(this.m_checkAcceptorStart.Checked)
			{
				// - Address와 Port를 읽어들인다.
				string address = m_textBindPort.SelectedText;
				int	port = Int32.Parse(this.m_textBindPort.Text);

				// - 시작한다.
				tempacceptor.Start(address, port);
			}
			// Case Unchecked) Unchecked이면 Acceptor를 중지한다.
			else
			{
				tempacceptor.Stop();
			}
        }

        private void AcceptorStatus_Click(object sender, EventArgs e)
        {
			// Declare) 
			CGDK.Net.Io.Connective.IAcceptor? tempacceptor = null;

			// 1) 현재 Acceptor를 얻는다.
			lock(m_csAcceptor)
			{
				tempacceptor = this.m_acceptor;
			}

			// Check) acceptor가 null이면 끝낸다.
			if(tempacceptor == null)
				return;

			// 2) ...
        }

        private void AcceptorCloseAll_Click(object sender, EventArgs e)
        {
			// Declare) 
			CGDK.Net.Io.Connective.IAcceptor? tempacceptor = null;

			// 1) 현재 Acceptor를 얻는다.
			lock(m_csAcceptor)
			{
				tempacceptor = this.m_acceptor;
			}

			// Check) acceptor가 null이면 끝낸다.
			if(tempacceptor == null)
				return;

			// 2) 모든 Socket을 닫는다.
			//tempacceptor.CloseAllSockets();
		}

		private void OnTimer_Update(object? pObject, EventArgs e)
		{
			this.UpdateControls();
		}

		private void UpdateControls()
		{
			if(InvokeRequired)
			{
				// 1) Delegate 객체 만들기
				var tempInvoke = new delegateUpdate(UpdateControls);

				// 2) Invoke
				this.Invoke(tempInvoke);

				// Return) 
				return;
			}

			// Declare) 
			CGDK.Net.Io.Connective.IAcceptor? tempacceptor = null;

			// 1) 현재 Acceptor를 얻는다.
			lock(m_csAcceptor)
			{
				tempacceptor = this.m_acceptor;
			}

			// Check) acceptor가 null이면 끝낸다.
			if(tempacceptor == null)
			{
				this.EnableControls(false);
				return;
			}

			// 2) 내용을 Update한다.
			if(tempacceptor.Now >= CGDK.eOBJECT_STATE.RUNNING)
			{
				this.m_checkAcceptorStart.Text = "Stop";
				this.m_buttonAcceptorStatus.Enabled = false;
				this.m_buttonAcceptorCloseAll.Enabled = false;
			}
			else
			{
				this.m_checkAcceptorStart.Text = "Start";
				this.m_buttonAcceptorStatus.Enabled = true;
				this.m_buttonAcceptorCloseAll.Enabled = true;
			}
		}

		public CGDK.Net.Io.Connective.IAcceptor? Acceptor
		{
			get { return m_acceptor;}
			set
			{
				lock(m_csAcceptor)
				{
					// Check) 기존 설정과 동일하면 그냥 끝낸다.
					if(this.m_acceptor == value)
						return;

					// 1) 값을 설정한다.
					this.m_acceptor = value;

					if(value!= null)
					{
						this.EnableControls(true);
						this.m_timerUpdate.Start();
					}
					else
					{
						this.EnableControls(false);
						this.m_timerUpdate.Stop(); 
					}
				}
			}
		}

		public int Port
		{
			get { return Int32.Parse(this.m_textBindPort.Text);}
			set { this.m_textBindPort.Text= (value<=65000) ? value.ToString() : "65000";}
		}

		private	readonly static Object					m_csAcceptor = new();
		private CGDK.Net.Io.Connective.IAcceptor?		m_acceptor = null;
		private	readonly System.Windows.Forms.Timer		m_timerUpdate = new();
		private delegate void delegateUpdate();
	}
}
