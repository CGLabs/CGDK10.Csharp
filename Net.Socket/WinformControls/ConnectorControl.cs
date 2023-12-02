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
            this.InitializeComponent();
        }

		private void ConnectorControl_Load(object sender, EventArgs e)
		{
			// 1) Timer를 설정한다.
			this.m_timerUpdate.Tick += new(OnTimer_Update);
			this.m_timerUpdate.Interval = 1000;

			// 2) Control설정
			this.EnableControls(this.m_Connector!=null);
		}

		public CGDK.Net.Io.IConnectRequestable? Connector
		{
			get { return this.m_Connector;}
			set
			{
				lock(m_csConnector)
				{
					// Check) 기존 설정과 동일하면 그냥 끝낸다.
					if(this.m_Connector == value)
						return;

					// 1) 값을 설정한다.
					this.m_Connector = value;

					if(value != null)
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

		private void ButtonConnect_Clicked(object sender, EventArgs e)
		{
			// Declare) 
			CGDK.Net.Io.IConnectRequestable? tempConnector = null;

			// 1) 현재 Acceptor를 얻는다.
			lock(m_csConnector)
			{
				tempConnector = this.m_Connector;
			}

			// Check) acceptor가 null이면 끝낸다.
			if(tempConnector == null)
			{
				this.EnableControls(false);
				return;
			}

			// 2) Address를 얻는다.
			var	ipHostInfo = Dns.GetHostEntry(m_textPeerAddress.Text);
			var	ipAddress = ipHostInfo.AddressList[^1];
			var	ipPeer = new IPEndPoint(ipAddress, Int32.Parse(m_textPeerPort.Text));
			
			// 3) Connect를 시도한다.
			tempConnector.ProcessRequestConnecting(ipPeer);
		}

		private	void EnableControls(bool _Enable)
		{
			// Check) 설정이 동일하면 그냥 끝낸다.
			if(this.m_textPeerAddress.Enabled == _Enable)
				return;

			this.m_textPeerAddress.Enabled = _Enable;
			this.m_textPeerPort.Enabled = _Enable;
			this.m_buttonConnect.Enabled = _Enable;
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
				delegateUpdate tempInvoke = new(UpdateControls);

				// 2) Invoke
				this.Invoke(tempInvoke);

				// Return) 
				return;
			}

			// Declare) 
			CGDK.Net.Io.IConnectRequestable? tempConnector = null;

			// 1) 현재 Connector 얻는다.
			lock(m_csConnector)
			{
				tempConnector = this.m_Connector;
			}

			// Check) Connector를 null이면 끝낸다.
			if(tempConnector == null)
			{
				this.EnableControls(false);
				return;
			}

			// 2) 내용을 Update한다.
			if(tempConnector.Socket.SocketState >= CGDK.Net.eSOCKET_STATE.ESTABLISHED)
			{
				this.m_buttonConnect.Text = "Disconnect";
				this.m_textPeerAddress.Enabled = false;
				this.m_textPeerPort.Enabled	= false;
			}
			else
			{
				this.m_buttonConnect.Text = "Connect";
				this.m_textPeerAddress.Enabled = true;
				this.m_textPeerPort.Enabled = true;
			}
		}

		private readonly static Object					m_csConnector = new();
		private CGDK.Net.Io.IConnectRequestable?		m_Connector = null;
		private	readonly System.Windows.Forms.Timer		m_timerUpdate = new();
		private delegate void delegateUpdate();
	}
}
