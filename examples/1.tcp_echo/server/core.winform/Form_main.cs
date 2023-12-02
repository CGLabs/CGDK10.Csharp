//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                     sample - tcp_echo.server.winform                      *
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace sample.tcp_echo.server.winform
{
	public partial class Form_main : Form
	{
		public Form_main()
		{
			// 1) Init
			InitializeComponent();
		}

		private void SampleServerForm_Load(object sender, EventArgs e)
		{
			// 1) 초기화
			test_tcp_echo_server.InitTest();

			// 1) Timer를 시작한다.
			m_timer_update.Tick	+= new EventHandler(OnTimer_Update);
			m_timer_update.Interval = 2000;

			// 4) Address와 Port의 초기값을 넣는다.
			m_STATIC_ADDRESS.Text = "localhost";
			m_EDIT_BIND_PORT.Text = test_tcp_echo_server.bind_port.ToString();

			// 5) 일단을 Control들을 Disable시켜놓는다
			EnableControls(false);
		}
		private void SampleServerForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			test_tcp_echo_server.CloseTest();
		}
		private void m_CHECK_start_accept_CheckedChanged(object sender, EventArgs e)
		{
			// Case Checked) start가 눌려졌다면 Acceptor를 시작한다.
			if(m_CHECK_start_accept.Checked)
			{
				// - Port번호를 얻는다.
				test_tcp_echo_server.bind_port	 = Int32.Parse(m_EDIT_BIND_PORT.Text);
				
				// - Test를 start한다.
				test_tcp_echo_server.RequestStartTest();
			}
			// Case Unchecked) Acceptor를 닫는다.
			else
			{
				// - Test를 Stop한다.
				test_tcp_echo_server.request_stop_test();
			}
		}

		private void OnClick_DISCONNECT_ALL(object sender, EventArgs e)
		{
			test_tcp_echo_server.request_disconnect_all();
		}

		private void EnableControls(bool bEnable)
		{
			m_STATIC_ADDRESS.Enabled				 = !bEnable;
			m_EDIT_BIND_PORT.Enabled				 = !bEnable;

			m_BUTTON_DISCONNECT_ALL.Enabled			 = bEnable;
			m_STATIC_KEEP_CONNECT.Enabled			 = bEnable;
			m_STATIC_TRY.Enabled					 = bEnable;
			m_STATIC_CONNECTED.Enabled				 = bEnable;
			m_STATIC_DISCONNECTED.Enabled			 = bEnable;
			m_STATIC_FAIL_CONNECT.Enabled			 = bEnable;
			m_STATIC_ERROR_CLOSE.Enabled			 = bEnable;
			m_STATIC_SENDED_MESSAGES.Enabled		 = bEnable;
			m_STATIC_SENDED_MESSAGES_PERSEC.Enabled	 = bEnable;
			m_STATIC_RECEIVED_MESSAGES.Enabled		 = bEnable;
			m_STATIC_RECEIVED_MESSAGES_PERSEC.Enabled= bEnable;
			m_STATIC_SENDED_BYTES_PERSEC.Enabled	 = bEnable;
			m_STATIC_RECEIVED_BYTES_PERSEC.Enabled	 = bEnable;
			m_STATIC_MESSAGES_PERSEC_TOTAL.Enabled	 = bEnable;
			m_STATIC_BYTES_PERSEC_TOTAL.Enabled		 = bEnable;
		}

		private int		m_tickLast;
		private long	m_iSended;
		private long	m_iSendedByte;
		private long	m_iReceived;
		private long	m_iReceivedByte;

		private delegate void delegateUpdate();
		public void on_accept_start()
		{
			// Check) Invoke
			if(InvokeRequired)
			{
				// - Invoke
				Invoke(new delegateUpdate(on_accept_start));

				// Return) 
				return;
			}

			// 1) 마지막...
			var infoTraffic	 = CGDK.net.io.statistics.Ntraffic.total;
			m_tickLast		 = Environment.TickCount;
			m_iSended		 = infoTraffic.sended_message_total;
			m_iSendedByte	 = infoTraffic.sended_bytes_total;
			m_iReceived		 = infoTraffic.received_message_total;
			m_iReceivedByte	 = infoTraffic.received_bytes_total;

			// 2) Control을 true로 바꾼다.
			EnableControls(true);

			// 3) Timer를 시작한다.
			m_timer_update.Start();

			// 4) Timer를 중지한다.
			m_timer_update.Start();
		}
		public void on_accept_close()
		{
			// Check) Invoke
			if(InvokeRequired)
			{
				// - Invoke
				Invoke(new delegateUpdate(on_accept_close));

				// Return) 
				return;
			}

			// 1) Timer를 중지한다.
			m_timer_update.Stop();

			// 2) Control을 true로 바꾼다.
			EnableControls(false);
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

			lock(m_csTest)
			{
				try
				{
					// 1) 현재 Tick과 Tick Gap을 구한다.
					int		tickNow		 = Environment.TickCount;
					int		tickDiffer	 = tickNow-m_tickLast;

					// Check) 
					if(tickDiffer<100)
					{
						return;
					}
				
					// 2) Time Update
					m_tickLast			 = tickNow;
				
					// 3) Connective Info
					var	infoConneceive						 = CGDK.net.io.statistics.Nconnective.total;

					m_STATIC_KEEP_CONNECT.Text				 = Convert.ToString(infoConneceive.keep_now);
					m_STATIC_TRY.Text						 = Convert.ToString(infoConneceive.try_total);
					m_STATIC_CONNECTED.Text					 = Convert.ToString(infoConneceive.success_connect_total);
					m_STATIC_DISCONNECTED.Text				 = Convert.ToString(infoConneceive.disconnect_total);
					m_STATIC_FAIL_CONNECT.Text				 = Convert.ToString(infoConneceive.fail_connect_total);
					m_STATIC_ERROR_CLOSE.Text				 = Convert.ToString(infoConneceive.error_disconnect_total);

					// 4) Traffice Info
					var	infoTraffic							 = CGDK.net.io.statistics.Ntraffic.total;
					long	iSended							 = infoTraffic.sended_message_total;
					long	iSendedByte						 = infoTraffic.sended_bytes_total;
					long	iReceived						 = infoTraffic.received_message_total;
					long	iReceivedByte					 = infoTraffic.received_bytes_total;

					float	totalSended_Messages_PerSec		 = ((float)(iSended - m_iSended))*1000/tickDiffer;
					float	totalSended_Bytes_PerSec		 = ((float)(iSendedByte - m_iSendedByte))*1000/tickDiffer;
					float	totalReceive_Messages_PerSec	 = ((float)(iReceived - m_iReceived))*1000/tickDiffer;
					float	totalReceive_Bytes_PerSec		 = ((float)(iReceivedByte - m_iReceivedByte))*1000/tickDiffer;

					m_iSended								 = iSended;
					m_iSendedByte							 = iSendedByte;
					m_iReceived								 = iReceived;
					m_iReceivedByte							 = iReceivedByte;

					m_STATIC_SENDED_MESSAGES.Text			 = CGDK.util.ToString(iSended);
					m_STATIC_SENDED_MESSAGES_PERSEC.Text	 = String.Format("{0:F2}", totalSended_Messages_PerSec);
					m_STATIC_RECEIVED_MESSAGES.Text			 = CGDK.util.ToString(iReceived);
					m_STATIC_RECEIVED_MESSAGES_PERSEC.Text	 = String.Format("{0:F2}", totalReceive_Messages_PerSec);
					m_STATIC_MESSAGES_PERSEC_TOTAL.Text		 = String.Format("{0:F2}", totalSended_Messages_PerSec+totalReceive_Messages_PerSec);

					m_STATIC_SENDED_BYTES_PERSEC.Text		 = CGDK.util.ToString(totalSended_Bytes_PerSec);
					m_STATIC_RECEIVED_BYTES_PERSEC.Text		 = CGDK.util.ToString(totalReceive_Bytes_PerSec);
					m_STATIC_BYTES_PERSEC_TOTAL.Text		 = CGDK.util.ToString(totalSended_Bytes_PerSec+totalReceive_Bytes_PerSec);
				}
				catch(Exception e)
				{
					// Trace)
					Trace.WriteLine("(Excp) Program: UpdateControls");
					Trace.WriteLine("       "+e);
				}
			}
		}

		// Members) 
		private	System.Windows.Forms.Timer	m_timer_update	 = new System.Windows.Forms.Timer();
		private	static Object				m_csTest		 = new Object();
	}
}
