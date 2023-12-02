using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace CGSocketWinformControls
{
    public partial class ConnectorControl : UserControl
    {
        public ConnectorControl()
        {
			// 1) Component를 초기화한다.
            InitializeComponent();
        }

		private void ConnectorControl_Load(object sender, EventArgs e)
		{
			// 1) Timer를 설정한다.
			m_timerUpdate.Tick		+= new EventHandler(OnTimer_Update);
			m_timerUpdate.Interval	 = 1000;

			// 2) Control설정
			EnableControls(m_Connector!=null);
		}

		public CGDK.Network.IO.IConnector Connector
		{
			get { return m_Connector;}
			set
			{
				lock(m_csConnector)
				{
					// Check) 기존 설정과 동일하면 그냥 끝낸다.
					if(m_Connector==value)
						return;

					// 1) 값을 설정한다.
					m_Connector = value;

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

		private void m_buttonConnect_Click(object sender, EventArgs e)
		{
			// Declare) 
			CGDK.Network.IO.IConnector	tempConnector	 = null;

			// 1) 현재 Acceptor를 얻는다.
			lock(m_csConnector)
			{
				tempConnector	 = m_Connector;
			}

			// Check) acceptor가 null이면 끝낸다.
			if(tempConnector==null)
			{
				EnableControls(false);
				return;
			}

			// 2) Address를 얻는다.
			var	ipHostInfo	 = Dns.GetHostEntry(m_textPeerAddress.Text);
			var	ipAddress	 = ipHostInfo.AddressList[ipHostInfo.AddressList.Length-1];
			var	ipPeer		 = new IPEndPoint(ipAddress, Int32.Parse(m_textPeerPort.Text));
			
			// 3) Connect를 시도한다.
			tempConnector.Connect(ipPeer);
		}

		private	void EnableControls(bool _Enable)
		{
			// Check) 설정이 동일하면 그냥 끝낸다.
			if(m_textPeerAddress.Enabled == _Enable)
				return;

			m_textPeerAddress.Enabled	 = _Enable;
			m_textPeerPort.Enabled		 = _Enable;
			m_buttonConnect.Enabled		 = _Enable;
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
			CGDK.Network.IO.IConnector	tempConnector	 = null;

			// 1) 현재 Acceptor를 얻는다.
			lock(m_csConnector)
			{
				tempConnector	 = m_Connector;
			}

			// Check) acceptor가 null이면 끝낸다.
			if(tempConnector==null)
			{
				EnableControls(false);
				return;
			}

			// 2) 내용을 Update한다.
			if(tempConnector.Socket.SocketState>=CGDK.Network.eSOCKET_STATE.ESTABLISHED)
			{
				m_buttonConnect.Text		 = "Disconnect";
				m_textPeerAddress.Enabled	 = false;
				m_textPeerPort.Enabled		 = false;
			}
			else
			{
				m_buttonConnect.Text		 = "Connect";
				m_textPeerAddress.Enabled	 = true;
				m_textPeerPort.Enabled		 = true;
			}
		}

		private	static Object							m_csConnector	 = new Object();
		private CGDK.Network.IO.IConnector				m_Connector		 = null;
		private	System.Windows.Forms.Timer				m_timerUpdate	 = new System.Windows.Forms.Timer();
		private delegate void delegateUpdate();
	}
}
