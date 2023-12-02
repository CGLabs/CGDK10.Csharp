using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CGDK.Network.IO.Connective;

namespace CGSocketWinformControls
{
    public partial class CAcceptorControl : UserControl
    {
        public CAcceptorControl()
        {
			// 1) Component를 초기화한다.
            InitializeComponent();
        }

		private void CAcceptorControl_Load(object sender, EventArgs e)
		{
			// 1) Timer를 시작한다.
			m_timerUpdate.Tick += new EventHandler(OnTimer_Update);
			m_timerUpdate.Interval = 1000;

			// 2) Control설정
			EnableControls(m_acceptor!=null);
		}

		private	void EnableControls(bool _Enable)
		{
			// Check) 설정이 동일하면 그냥 끝낸다.
			if(m_comboBindAddress.Enabled == _Enable)
				return;

			m_comboBindAddress.Enabled			 = _Enable;
			m_textBindPort.Enabled				 = _Enable;
			m_checkAcceptorStart.Enabled		 = _Enable;
			m_buttonAcceptorStatus.Enabled		 = _Enable;
			m_buttonAcceptorCloseAll.Enabled	 = _Enable;
			label1.Enabled						 = _Enable;
		}

        private void BindAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void AcceptorStart_CheckedChanged(object sender, EventArgs e)
        {
			// Declare) 
			CGDK.Network.IO.Connective.IAcceptor	tempacceptor	 = null;

			// 1) 현재 Acceptor를 얻는다.
			lock(m_csAcceptor)
			{
				tempacceptor	 = m_acceptor;
			}

			// Check) acceptor가 null이면 끝낸다.
			if(tempacceptor==null)
				return;

			// Case Checked) Checked이면 시작한다.
			if(m_checkAcceptorStart.Checked)
			{
				// - Address와 Port를 읽어들인다.
				string	address	 = m_textBindPort.SelectedText;
				int		port	 = Int32.Parse(this.m_textBindPort.Text);

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
			CGDK.Network.IO.Connective.IAcceptor	tempacceptor	 = null;

			// 1) 현재 Acceptor를 얻는다.
			lock(m_csAcceptor)
			{
				tempacceptor	 = m_acceptor;
			}

			// Check) acceptor가 null이면 끝낸다.
			if(tempacceptor==null)
				return;

			// 2) ...
        }

        private void AcceptorCloseAll_Click(object sender, EventArgs e)
        {
			// Declare) 
			CGDK.Network.IO.Connective.IAcceptor	tempacceptor	 = null;

			// 1) 현재 Acceptor를 얻는다.
			lock(m_csAcceptor)
			{
				tempacceptor	 = m_acceptor;
			}

			// Check) acceptor가 null이면 끝낸다.
			if(tempacceptor==null)
				return;

			// 2) 모든 Socket을 닫는다.
			//tempacceptor.CloseAllSockets();
		}

		private void OnTimer_Update(object pObject, EventArgs e)
		{
			UpdateControls();
		}

		private void UpdateControls()
		{
			if(InvokeRequired)
			{
				// 1) Delegate 객체 만들기
				delegateUpdate	tempInvoke	 = new delegateUpdate(UpdateControls);

				// 2) Invoke
				Invoke(tempInvoke);

				// Return) 
				return;
			}

			// Declare) 
			CGDK.Network.IO.Connective.IAcceptor	tempacceptor	 = null;

			// 1) 현재 Acceptor를 얻는다.
			lock(m_csAcceptor)
			{
				tempacceptor	 = m_acceptor;
			}

			// Check) acceptor가 null이면 끝낸다.
			if(tempacceptor==null)
			{
				EnableControls(false);
				return;
			}

			// 2) 내용을 Update한다.
			if(tempacceptor.ObjectState>=CGDK.eOBJECT_STATE.RUNNING)
			{
				m_checkAcceptorStart.Text		 = "Stop";
				m_buttonAcceptorStatus.Enabled	 = false;
				m_buttonAcceptorCloseAll.Enabled = false;
			}
			else
			{
				m_checkAcceptorStart.Text		 = "Start";
				m_buttonAcceptorStatus.Enabled	 = true;
				m_buttonAcceptorCloseAll.Enabled = true;
			}
		}

		public CGDK.Network.IO.Connective.IAcceptor Acceptor
		{
			get { return m_acceptor;}
			set
			{
				lock(m_csAcceptor)
				{
					// Check) 기존 설정과 동일하면 그냥 끝낸다.
					if(m_acceptor==value)
						return;

					// 1) 값을 설정한다.
					m_acceptor = value;

					if(value!= null)
					{
						EnableControls(true);
						m_timerUpdate.Start();
					}
					else
					{
						EnableControls(false);
						m_timerUpdate.Stop(); 
					}
				}
			}
		}

		public int Port
		{
			get { return Int32.Parse(this.m_textBindPort.Text);}
			set { this.m_textBindPort.Text= (value<=65000) ? value.ToString() : "65000";}
		}

		private	static Object							m_csAcceptor	 = new Object();
		private CGDK.Network.IO.Connective.IAcceptor	m_acceptor		 = null;
		private	System.Windows.Forms.Timer				m_timerUpdate	 = new System.Windows.Forms.Timer();
		private delegate void delegateUpdate();
	}
}
