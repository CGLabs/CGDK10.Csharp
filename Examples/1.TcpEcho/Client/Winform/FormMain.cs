//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                     sample - tcp_echo.client.winform                      *
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
using System.Net;
using System.Diagnostics;
using System.Windows.Forms;

namespace Sample.TcpEcho.Client.Winform
{
	public partial class Form_main : Form
	{
		public Form_main()
		{
			InitializeComponent();
		}

		// Definitions)
		private delegate void	delegateControl(object _sender, EventArgs _e);

		// Members) 
		private int				m_tick_last;
		private long			m_sended;
		private long			m_sended_byte;
		private long			m_received;
		private long			m_received_byte;
		private	static Timer	m_timer_update = new Timer();

		private void OnFormLoad(object _sender, EventArgs _e)
		{
			// 1) Socket Test 초기화
			TestTcpEchoClient.InitializeTest();

			// 2) [Update] Timer를 설정한다.
			m_timer_update.Tick			+= new EventHandler(on_timer_update);
			m_timer_update.Interval		 = 2000;

			// 3) Address와 Port의 초기값을 넣는다.
			m_EDIT_address.Text							 = "localhost";
			m_EDIT_Port.Text							 = "20000";
			m_EDIT_connection_times.Text					 = "2000";
			m_EDIT_connection_test_interval.Text		 = TestTcpEchoClient.connection_test_interval.ToString();
			m_EDIT_connection_test_range_start.Text		 = TestTcpEchoClient.connection_test_min_connect.ToString();
			m_EDIT_connection_test_range_end.Text			 = TestTcpEchoClient.connection_test_max_connect.ToString();
			m_EDIT_send_test_times.Text					 = TestTcpEchoClient.send_test_times.ToString();
			m_COMBO_send_test_message_size.SelectedIndex = TestTcpEchoClient.send_test_message_size;

			// 4) 마지막...
			var infoTraffic		 = CGDK.Net.Io.Statistics.Ntraffic.total;
			m_tick_last			 = Environment.TickCount;
			m_sended			 = infoTraffic.sended_message_total;
			m_sended_byte		 = infoTraffic.sended_bytes_total;
			m_received			 = infoTraffic.received_message_total;
			m_received_byte		 = infoTraffic.received_bytes_total;

			// 5) Update Timer를 시작한다.
			m_timer_update.Start();
		}

		private void OnFormClosing(object _sender, FormClosingEventArgs _e)
		{
			// 1) Update Timer를 중지한다.
			m_timer_update.Stop();

			// 2) Test를 Close
			TestTcpEchoClient.CloseTest();
		}

		private void read_connect_address()
		{
			// 1) Listen할 Address와 Port를 얻는다. (일반적으로 localhost)
			var	ip_host_info = Dns.GetHostEntry(m_EDIT_address.Text);
			var	ip_address	 = ip_host_info.AddressList[1];
			var	port		 = Int32.Parse(m_EDIT_Port.Text);
			TestTcpEchoClient.bind_ip_connect = new IPEndPoint(ip_address, port);

			// 2) Test Interval을 설정한다.
			TestTcpEchoClient.connection_test_interval = Int32.Parse(m_EDIT_connection_test_interval.Text);
		}
		private void update_controls()
		{
			try
			{
				// 1) 현재 Tick과 Tick Gap을 구한다.
				int	tick_now = Environment.TickCount;
				int tick_differ = tick_now-m_tick_last;
				m_tick_last = tick_now;

				// 2) Connective Info
				var info_conneceive						 = CGDK.Net.Io.Statistics.Nconnective.total;

				m_STATIC_connection.Text				 = Convert.ToString(info_conneceive.keep_now);
				m_STATIC_try.Text						 = Convert.ToString(info_conneceive.try_total);
				m_STATIC_success.Text					 = Convert.ToString(info_conneceive.success_connect_total);
				m_STATIC_failure.Text					 = Convert.ToString(info_conneceive.disconnect_total);
				m_STATIC_force_dis.Text					 = Convert.ToString(info_conneceive.fail_connect_total);
				m_STATIC_delay.Text						 = Convert.ToString(info_conneceive.error_disconnect_total);

				// 3) Traffice Info
				var infoTraffic							 = CGDK.Net.Io.Statistics.Ntraffic.total;

				long	iSended							 = infoTraffic.sended_message_total;
				long	iSendedByte						 = infoTraffic.sended_bytes_total;
				long	iReceived						 = infoTraffic.received_message_total;
				long	iReceivedByte					 = infoTraffic.received_bytes_total;

				float	total_sended_messages_per_sec		 = ((float)(iSended - m_sended))*1000 / tick_differ;
				float	total_sended_bytes_per_sec		 = ((float)(iSendedByte - m_sended_byte)) * 1000 / tick_differ;
				float	total_receive_messages_per_sec	 = ((float)(iReceived - m_received)) * 1000 / tick_differ;
				float	total_receive_bytes_per_sec		 = ((float)(iReceivedByte - m_received_byte)) * 1000 / tick_differ;

				m_sended								 = iSended;
				m_sended_byte							 = iSendedByte;
				m_received								 = iReceived;
				m_received_byte							 = iReceivedByte;

				m_STATIC_SENDED_MESSAGES.Text			 = CGDK.Util.ToString(iSended);
				m_STATIC_SENDED_MESSAGES_PERSEC.Text	 = String.Format("{0:F2}", total_sended_messages_per_sec);
				m_STATIC_RECEIVED_MESSAGES.Text			 = CGDK.Util.ToString(iReceived);
				m_STATIC_RECEIVED_MESSAGES_PERSEC.Text	 = String.Format("{0:F2}", total_receive_messages_per_sec);
				m_STATIC_MESSAGES_PERSEC_TOTAL.Text		 = String.Format("{0:F2}", total_sended_messages_per_sec + total_receive_messages_per_sec);

				m_STATIC_SENDED_BYTES_PERSEC.Text		 = CGDK.Util.ToString(total_sended_bytes_per_sec);
				m_STATIC_RECEIVED_BYTES_PERSEC.Text		 = CGDK.Util.ToString(total_receive_bytes_per_sec);
				m_STATIC_BYTES_PERSEC_TOTAL.Text		 = CGDK.Util.ToString(total_sended_bytes_per_sec + total_receive_bytes_per_sec);
			}
			catch(Exception e)
			{
				// Trace)
				Trace.WriteLine("(Excp) Program: update_controls");
				Trace.WriteLine("       "+e);
			}
		}

		private void on_timer_update(object _object, EventArgs _e)
		{
			// Check) Invoke가 필요하면 Invoke처리함.
			if(InvokeRequired)
			{
				// - Invoke
				Invoke(new delegateControl(on_timer_update));

				// Return) 
				return;
			}

			// 1) Update한다.
			update_controls();
		}
	
		private void on_click_reset(object _sender, EventArgs _e)
		{
			// 1) 접속된 모든 Socket들을 끊는다.
			TestTcpEchoClient.ResetStatistics();

			// 2) Update Info
			var infoTraffic		 = CGDK.Net.Io.Statistics.Ntraffic.total;
			m_tick_last			 = Environment.TickCount;
			m_sended			 = infoTraffic.sended_message_total;
			m_sended_byte		 = infoTraffic.sended_bytes_total;
			m_received			 = infoTraffic.received_message_total;
			m_received_byte		 = infoTraffic.received_bytes_total;
		}
		private void on_BUTTON_connect_1_click(object _sender, EventArgs _e)
		{
			// 1) Connect를 할 주소를 읽어들인다.
			read_connect_address();

			// 2) 1개의 Connect를 시도한다.
			TestTcpEchoClient.RequestConnect(1);
		}
		private void on_BUTTON_connect_10_click(object _sender, EventArgs _e)
		{
			// 1) Connect를 할 주소를 읽어들인다.
			read_connect_address();

			// 2) 10개의 Connect를 시도한다.
			TestTcpEchoClient.RequestConnect(10);
		}
		private void on_BUTTON_connect_100_click(object _sender, EventArgs _e)
		{
			// 1) Connect를 할 주소를 읽어들인다.
			read_connect_address();

			// 2) 100개의 Connect를 시도한다.
			TestTcpEchoClient.RequestConnect(100);
		}
		private void on_BUTTON_connect_Click(object _sender, EventArgs _e)
		{
			// 1) Connect를 할 주소를 읽어들인다.
			read_connect_address();

			// 2) 설정한 갯수만큼의 Connect를 시도한다.
			TestTcpEchoClient.RequestConnect(Int32.Parse(m_EDIT_connection_times.Text));
		}
		private void on_BUTTON_disconnect_click(object _sender, EventArgs _e)
		{
			// 1) 1개의 접속만 끊는다.
			TestTcpEchoClient.RequestDisconnectSingle();
		}
		private void on_BUTTON_request_disconnect_all_Click(object _sender, EventArgs _e)
		{
			// 1) 접속된 모든 Socket들을 끊는다.
			TestTcpEchoClient.RequestDisconnectAll();
		}

		private void on_CHECK_connection_test_start_stop_checked_changed(object _sender, EventArgs _e)
		{
			if (m_CHECK_connection_test_start_stop.Checked)
			{
				// 1) Connect를 할 주소를 읽어들인다.
				read_connect_address();

				// 2) 접속테스트를 시작한다.
				TestTcpEchoClient.RequestStartConnectionTest();
			}
			else
			{
				TestTcpEchoClient.RrequestStopConnectionTest();
			}
		}

		private void on_CHECK_RequestSend_start_stop_checked_changed(object _sender, EventArgs _e)
		{
			if (m_CHECK_RequestSend_start_stop.Checked)
			{
				TestTcpEchoClient.RequestStartSendTest();
			}
			else
			{
				TestTcpEchoClient.RequestStopSendTest();
			}
		}

		private void on_GROUP_traffic_info_enter(object _sender, EventArgs _e)
		{
		}

		private void on_COMBO_send_test_message_size_selected_index_changed(object _sender, EventArgs _e)
		{
			TestTcpEchoClient.send_test_message_size = m_COMBO_send_test_message_size.SelectedIndex;
		}

		private void on_BUTTON_single_send_click(object _sender, EventArgs _e)
		{
			TestTcpEchoClient.RequestSend(1);
		}

        private void on_BUTTON_error_send_zero_click(object _sender, EventArgs _e)
		{
			TestTcpEchoClient.RequestSendErrorZero();
		}

		private void on_EDIT_send_test_times_TextChanged(object _sender, EventArgs _e)
		{
			TestTcpEchoClient.send_test_times = Convert.ToInt32(m_EDIT_send_test_times.Text);
		}

		private void on_BUTTON_send_click(object _sender, EventArgs _e)
		{
			TestTcpEchoClient.RequestSend(TestTcpEchoClient.send_test_times);
		}

	}
}
