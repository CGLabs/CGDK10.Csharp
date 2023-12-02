namespace sample.tcp_echo.client.winform
{
	partial class Form_main
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.m_EDIT_Port = new System.Windows.Forms.TextBox();
			this.m_TEXT_address = new System.Windows.Forms.Label();
			this.m_GROUP_connection_test = new System.Windows.Forms.GroupBox();
			this.m_CHECK_connection_test_start_stop = new System.Windows.Forms.CheckBox();
			this.m_TEXT_connection_test_range = new System.Windows.Forms.Label();
			this.m_TEXT_connection_test_interval = new System.Windows.Forms.Label();
			this.m_EDIT_connection_test_range_end = new System.Windows.Forms.TextBox();
			this.m_EDIT_connection_test_range_start = new System.Windows.Forms.TextBox();
			this.m_EDIT_connection_test_interval = new System.Windows.Forms.TextBox();
			this.m_TEXT_connection_test_rangeWave = new System.Windows.Forms.Label();
			this.m_TEXT_explanation_1 = new System.Windows.Forms.Label();
			this.m_TEXT_connection = new System.Windows.Forms.Label();
			this.m_TEXT_try = new System.Windows.Forms.Label();
			this.m_TEXT_success = new System.Windows.Forms.Label();
			this.m_TEXT_failure = new System.Windows.Forms.Label();
			this.m_TEXT_force_dis = new System.Windows.Forms.Label();
			this.m_TEXT_delay = new System.Windows.Forms.Label();
			this.m_TEXT_bottom = new System.Windows.Forms.Label();
			this.m_STATIC_connection = new System.Windows.Forms.Label();
			this.m_STATIC_try = new System.Windows.Forms.Label();
			this.m_STATIC_success = new System.Windows.Forms.Label();
			this.m_STATIC_failure = new System.Windows.Forms.Label();
			this.m_STATIC_force_dis = new System.Windows.Forms.Label();
			this.m_STATIC_delay = new System.Windows.Forms.Label();
			this.m_TEXT_seperator_2 = new System.Windows.Forms.Label();
			this.m_BUTTON_reset = new System.Windows.Forms.Button();
			this.m_STATIC_SENDED_MESSAGES = new System.Windows.Forms.Label();
			this.m_STATIC_SENDED_MESSAGES_PERSEC = new System.Windows.Forms.Label();
			this.m_STATIC_RECEIVED_MESSAGES = new System.Windows.Forms.Label();
			this.m_STATIC_RECEIVED_MESSAGES_PERSEC = new System.Windows.Forms.Label();
			this.m_STATIC_SENDED_BYTES_PERSEC = new System.Windows.Forms.Label();
			this.m_STATIC_RECEIVED_BYTES_PERSEC = new System.Windows.Forms.Label();
			this.m_STATIC_MESSAGES_PERSEC_TOTAL = new System.Windows.Forms.Label();
			this.m_STATIC_BYTES_PERSEC_TOTAL = new System.Windows.Forms.Label();
			this.m_TEXT_total_messages = new System.Windows.Forms.Label();
			this.m_TEXT_total_bytes = new System.Windows.Forms.Label();
			this.m_TEXT_sended_messages = new System.Windows.Forms.Label();
			this.m_TEXT_received_messages = new System.Windows.Forms.Label();
			this.m_TEXT_sended_bytes = new System.Windows.Forms.Label();
			this.m_TEXT_received_bytes = new System.Windows.Forms.Label();
			this.m_TEXT_messages_accumulated = new System.Windows.Forms.Label();
			this.m_TEXT_messages_per_sec = new System.Windows.Forms.Label();
			this.m_TEXT_bytes_per_sec = new System.Windows.Forms.Label();
			this.m_TEXT_tcp_connection_info_seperator_1 = new System.Windows.Forms.Label();
			this.m_TEXT_tcp_connection_info_seperator_3 = new System.Windows.Forms.Label();
			this.m_TEXT_tcp_connection_info_seperator_4 = new System.Windows.Forms.Label();
			this.m_TEXT_tcp_connection_info_seperator_5 = new System.Windows.Forms.Label();
			this.m_TEXT_tcp_connection_info_seperator_6 = new System.Windows.Forms.Label();
			this.m_TEXT_traffic_info_seperator_1 = new System.Windows.Forms.Label();
			this.m_TEXT_traffic_info_seperator_2 = new System.Windows.Forms.Label();
			this.m_TEXT_traffic_info_seperator_3 = new System.Windows.Forms.Label();
			this.m_TEXT_traffic_info_seperator_4 = new System.Windows.Forms.Label();
			this.m_TEXT_traffic_info_seperator_5 = new System.Windows.Forms.Label();
			this.m_TEXT_seperator_bottom = new System.Windows.Forms.Label();
			this.m_GROUP_tcp_connection_info = new System.Windows.Forms.GroupBox();
			this.m_EDIT_address = new System.Windows.Forms.TextBox();
			this.m_BUTTON_request_disconnect_all = new System.Windows.Forms.Button();
			this.m_BUTTON_disconnect = new System.Windows.Forms.Button();
			this.m_BUTTON_connect = new System.Windows.Forms.Button();
			this.m_EDIT_connection_times = new System.Windows.Forms.TextBox();
			this.m_BUTTON_connect_1 = new System.Windows.Forms.Button();
			this.m_BUTTON_connect_10 = new System.Windows.Forms.Button();
			this.m_BUTTON_connect_100 = new System.Windows.Forms.Button();
			this.m_TEXT_ms = new System.Windows.Forms.Label();
			this.m_GROUP_traffic_info = new System.Windows.Forms.GroupBox();
			this.m_GROUP_request_send = new System.Windows.Forms.GroupBox();
			this.m_BUTTON_send = new System.Windows.Forms.Button();
			this.m_CHECK_request_send_start_stop = new System.Windows.Forms.CheckBox();
			this.m_COMBO_send_test_message_size = new System.Windows.Forms.ComboBox();
			this.m_TEXT_error_send_zero = new System.Windows.Forms.Label();
			this.m_TEXT_single_send = new System.Windows.Forms.Label();
			this.m_BUTTON_error_send_zero = new System.Windows.Forms.Button();
			this.m_BUTTON_single_send = new System.Windows.Forms.Button();
			this.m_TEXT_send_message_size = new System.Windows.Forms.Label();
			this.m_TEXT_request_send_times = new System.Windows.Forms.Label();
			this.m_EDIT_send_test_times = new System.Windows.Forms.TextBox();
			this.m_TEXT_explanation_2 = new System.Windows.Forms.Label();
			this.m_GROUP_connection_test.SuspendLayout();
			this.m_GROUP_tcp_connection_info.SuspendLayout();
			this.m_GROUP_traffic_info.SuspendLayout();
			this.m_GROUP_request_send.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_EDIT_Port
			// 
			this.m_EDIT_Port.Location = new System.Drawing.Point(190, 72);
			this.m_EDIT_Port.Name = "m_EDIT_Port";
			this.m_EDIT_Port.Size = new System.Drawing.Size(45, 20);
			this.m_EDIT_Port.TabIndex = 3;
			// 
			// m_TEXT_address
			// 
			this.m_TEXT_address.AutoSize = true;
			this.m_TEXT_address.Location = new System.Drawing.Point(23, 75);
			this.m_TEXT_address.Name = "m_TEXT_address";
			this.m_TEXT_address.Size = new System.Drawing.Size(46, 13);
			this.m_TEXT_address.TabIndex = 0;
			this.m_TEXT_address.Text = "Address";
			// 
			// m_GROUP_connection_test
			// 
			this.m_GROUP_connection_test.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.m_GROUP_connection_test.Controls.Add(this.m_CHECK_connection_test_start_stop);
			this.m_GROUP_connection_test.Controls.Add(this.m_TEXT_connection_test_range);
			this.m_GROUP_connection_test.Controls.Add(this.m_TEXT_connection_test_interval);
			this.m_GROUP_connection_test.Controls.Add(this.m_EDIT_connection_test_range_end);
			this.m_GROUP_connection_test.Controls.Add(this.m_EDIT_connection_test_range_start);
			this.m_GROUP_connection_test.Controls.Add(this.m_EDIT_connection_test_interval);
			this.m_GROUP_connection_test.Controls.Add(this.m_TEXT_connection_test_rangeWave);
			this.m_GROUP_connection_test.Controls.Add(this.m_TEXT_explanation_1);
			this.m_GROUP_connection_test.Location = new System.Drawing.Point(20, 106);
			this.m_GROUP_connection_test.Name = "m_GROUP_connection_test";
			this.m_GROUP_connection_test.Size = new System.Drawing.Size(599, 64);
			this.m_GROUP_connection_test.TabIndex = 4;
			this.m_GROUP_connection_test.TabStop = false;
			this.m_GROUP_connection_test.Text = "Connection Test";
			// 
			// m_CHECK_connection_test_start_stop
			// 
			this.m_CHECK_connection_test_start_stop.Appearance = System.Windows.Forms.Appearance.Button;
			this.m_CHECK_connection_test_start_stop.Location = new System.Drawing.Point(504, 31);
			this.m_CHECK_connection_test_start_stop.Name = "m_CHECK_connection_test_start_stop";
			this.m_CHECK_connection_test_start_stop.Size = new System.Drawing.Size(84, 24);
			this.m_CHECK_connection_test_start_stop.TabIndex = 0;
			this.m_CHECK_connection_test_start_stop.Text = "Start/Stop";
			this.m_CHECK_connection_test_start_stop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.m_CHECK_connection_test_start_stop.CheckedChanged += new System.EventHandler(this.on_CHECK_connection_test_start_stop_checked_changed);
			// 
			// m_TEXT_connection_test_range
			// 
			this.m_TEXT_connection_test_range.AutoSize = true;
			this.m_TEXT_connection_test_range.Location = new System.Drawing.Point(420, 18);
			this.m_TEXT_connection_test_range.Name = "m_TEXT_connection_test_range";
			this.m_TEXT_connection_test_range.Size = new System.Drawing.Size(38, 13);
			this.m_TEXT_connection_test_range.TabIndex = 49;
			this.m_TEXT_connection_test_range.Text = "Range";
			// 
			// m_TEXT_connection_test_interval
			// 
			this.m_TEXT_connection_test_interval.AutoSize = true;
			this.m_TEXT_connection_test_interval.Location = new System.Drawing.Point(323, 18);
			this.m_TEXT_connection_test_interval.Name = "m_TEXT_connection_test_interval";
			this.m_TEXT_connection_test_interval.Size = new System.Drawing.Size(69, 13);
			this.m_TEXT_connection_test_interval.TabIndex = 45;
			this.m_TEXT_connection_test_interval.Text = "Interval (ms)";
			// 
			// m_EDIT_connection_test_range_end
			// 
			this.m_EDIT_connection_test_range_end.Location = new System.Drawing.Point(447, 34);
			this.m_EDIT_connection_test_range_end.Name = "m_EDIT_connection_test_range_end";
			this.m_EDIT_connection_test_range_end.Size = new System.Drawing.Size(53, 20);
			this.m_EDIT_connection_test_range_end.TabIndex = 48;
			this.m_EDIT_connection_test_range_end.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// m_EDIT_connection_test_range_start
			// 
			this.m_EDIT_connection_test_range_start.Location = new System.Drawing.Point(382, 34);
			this.m_EDIT_connection_test_range_start.Name = "m_EDIT_connection_test_range_start";
			this.m_EDIT_connection_test_range_start.Size = new System.Drawing.Size(53, 20);
			this.m_EDIT_connection_test_range_start.TabIndex = 47;
			this.m_EDIT_connection_test_range_start.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// m_EDIT_connection_test_interval
			// 
			this.m_EDIT_connection_test_interval.Location = new System.Drawing.Point(331, 34);
			this.m_EDIT_connection_test_interval.Name = "m_EDIT_connection_test_interval";
			this.m_EDIT_connection_test_interval.Size = new System.Drawing.Size(45, 20);
			this.m_EDIT_connection_test_interval.TabIndex = 46;
			this.m_EDIT_connection_test_interval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// m_TEXT_connection_test_rangeWave
			// 
			this.m_TEXT_connection_test_rangeWave.AutoSize = true;
			this.m_TEXT_connection_test_rangeWave.Location = new System.Drawing.Point(433, 38);
			this.m_TEXT_connection_test_rangeWave.Name = "m_TEXT_connection_test_rangeWave";
			this.m_TEXT_connection_test_rangeWave.Size = new System.Drawing.Size(15, 13);
			this.m_TEXT_connection_test_rangeWave.TabIndex = 46;
			this.m_TEXT_connection_test_rangeWave.Text = "~";
			// 
			// m_TEXT_explanation_1
			// 
			this.m_TEXT_explanation_1.Location = new System.Drawing.Point(15, 27);
			this.m_TEXT_explanation_1.Name = "m_TEXT_explanation_1";
			this.m_TEXT_explanation_1.Size = new System.Drawing.Size(307, 35);
			this.m_TEXT_explanation_1.TabIndex = 45;
			this.m_TEXT_explanation_1.Text = "테스트를 시작하면 Interval마다 Range에 설정된 접속수를 유지하는 범위내에서 접속과 접속해제를 반복합니다.";
			// 
			// m_TEXT_connection
			// 
			this.m_TEXT_connection.AutoSize = true;
			this.m_TEXT_connection.Location = new System.Drawing.Point(64, 25);
			this.m_TEXT_connection.Name = "m_TEXT_connection";
			this.m_TEXT_connection.Size = new System.Drawing.Size(61, 13);
			this.m_TEXT_connection.TabIndex = 5;
			this.m_TEXT_connection.Text = "Connection";
			// 
			// m_TEXT_try
			// 
			this.m_TEXT_try.AutoSize = true;
			this.m_TEXT_try.Location = new System.Drawing.Point(172, 25);
			this.m_TEXT_try.Name = "m_TEXT_try";
			this.m_TEXT_try.Size = new System.Drawing.Size(23, 13);
			this.m_TEXT_try.TabIndex = 6;
			this.m_TEXT_try.Text = "Try";
			// 
			// m_TEXT_success
			// 
			this.m_TEXT_success.AutoSize = true;
			this.m_TEXT_success.Location = new System.Drawing.Point(235, 25);
			this.m_TEXT_success.Name = "m_TEXT_success";
			this.m_TEXT_success.Size = new System.Drawing.Size(45, 13);
			this.m_TEXT_success.TabIndex = 7;
			this.m_TEXT_success.Text = "Success";
			// 
			// m_TEXT_failure
			// 
			this.m_TEXT_failure.AutoSize = true;
			this.m_TEXT_failure.Location = new System.Drawing.Point(312, 25);
			this.m_TEXT_failure.Name = "m_TEXT_failure";
			this.m_TEXT_failure.Size = new System.Drawing.Size(39, 13);
			this.m_TEXT_failure.TabIndex = 8;
			this.m_TEXT_failure.Text = "Failure";
			// 
			// m_TEXT_force_dis
			// 
			this.m_TEXT_force_dis.AutoSize = true;
			this.m_TEXT_force_dis.Location = new System.Drawing.Point(390, 25);
			this.m_TEXT_force_dis.Name = "m_TEXT_force_dis";
			this.m_TEXT_force_dis.Size = new System.Drawing.Size(51, 13);
			this.m_TEXT_force_dis.TabIndex = 9;
			this.m_TEXT_force_dis.Text = "Force Dis";
			// 
			// m_TEXT_delay
			// 
			this.m_TEXT_delay.AutoSize = true;
			this.m_TEXT_delay.Location = new System.Drawing.Point(490, 25);
			this.m_TEXT_delay.Name = "m_TEXT_delay";
			this.m_TEXT_delay.Size = new System.Drawing.Size(34, 13);
			this.m_TEXT_delay.TabIndex = 10;
			this.m_TEXT_delay.Text = "Delay";
			// 
			// m_TEXT_bottom
			// 
			this.m_TEXT_bottom.AutoSize = true;
			this.m_TEXT_bottom.Enabled = false;
			this.m_TEXT_bottom.Font = new System.Drawing.Font("Tahoma", 8F);
			this.m_TEXT_bottom.ForeColor = System.Drawing.SystemColors.ButtonShadow;
			this.m_TEXT_bottom.Location = new System.Drawing.Point(193, 441);
			this.m_TEXT_bottom.Name = "m_TEXT_bottom";
			this.m_TEXT_bottom.Size = new System.Drawing.Size(241, 13);
			this.m_TEXT_bottom.TabIndex = 12;
			this.m_TEXT_bottom.Text = "CGCII. ⓒ 2016 Cho sanghyun All right reserved.";
			// 
			// m_STATIC_connection
			// 
			this.m_STATIC_connection.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_STATIC_connection.Location = new System.Drawing.Point(60, 41);
			this.m_STATIC_connection.Name = "m_STATIC_connection";
			this.m_STATIC_connection.Size = new System.Drawing.Size(69, 20);
			this.m_STATIC_connection.TabIndex = 13;
			this.m_STATIC_connection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_try
			// 
			this.m_STATIC_try.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_STATIC_try.Location = new System.Drawing.Point(149, 41);
			this.m_STATIC_try.Name = "m_STATIC_try";
			this.m_STATIC_try.Size = new System.Drawing.Size(69, 20);
			this.m_STATIC_try.TabIndex = 14;
			this.m_STATIC_try.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_success
			// 
			this.m_STATIC_success.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_STATIC_success.Location = new System.Drawing.Point(223, 41);
			this.m_STATIC_success.Name = "m_STATIC_success";
			this.m_STATIC_success.Size = new System.Drawing.Size(69, 20);
			this.m_STATIC_success.TabIndex = 15;
			this.m_STATIC_success.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_failure
			// 
			this.m_STATIC_failure.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_STATIC_failure.Location = new System.Drawing.Point(299, 41);
			this.m_STATIC_failure.Name = "m_STATIC_failure";
			this.m_STATIC_failure.Size = new System.Drawing.Size(60, 20);
			this.m_STATIC_failure.TabIndex = 16;
			this.m_STATIC_failure.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_force_dis
			// 
			this.m_STATIC_force_dis.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_STATIC_force_dis.Location = new System.Drawing.Point(381, 41);
			this.m_STATIC_force_dis.Name = "m_STATIC_force_dis";
			this.m_STATIC_force_dis.Size = new System.Drawing.Size(69, 20);
			this.m_STATIC_force_dis.TabIndex = 17;
			this.m_STATIC_force_dis.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_delay
			// 
			this.m_STATIC_delay.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_STATIC_delay.Location = new System.Drawing.Point(478, 41);
			this.m_STATIC_delay.Name = "m_STATIC_delay";
			this.m_STATIC_delay.Size = new System.Drawing.Size(64, 20);
			this.m_STATIC_delay.TabIndex = 18;
			this.m_STATIC_delay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_TEXT_tcp_connection_info_seperator_1
			// 
			this.m_TEXT_tcp_connection_info_seperator_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_TEXT_tcp_connection_info_seperator_1.Location = new System.Drawing.Point(139, 29);
			this.m_TEXT_tcp_connection_info_seperator_1.Name = "m_TEXT_tcp_connection_info_seperator_1";
			this.m_TEXT_tcp_connection_info_seperator_1.Size = new System.Drawing.Size(1, 32);
			this.m_TEXT_tcp_connection_info_seperator_1.TabIndex = 19;
			// 
			// m_TEXT_seperator_2
			// 
			this.m_TEXT_seperator_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_TEXT_seperator_2.Location = new System.Drawing.Point(370, 29);
			this.m_TEXT_seperator_2.Name = "m_TEXT_seperator_2";
			this.m_TEXT_seperator_2.Size = new System.Drawing.Size(1, 32);
			this.m_TEXT_seperator_2.TabIndex = 20;
			this.m_TEXT_seperator_2.Text = "m_TEXT_TCPConnectionInfo_Seperator_2";
			// 
			// m_BUTTON_reset
			// 
			this.m_BUTTON_reset.Location = new System.Drawing.Point(565, 44);
			this.m_BUTTON_reset.Margin = new System.Windows.Forms.Padding(0);
			this.m_BUTTON_reset.Name = "m_BUTTON_reset";
			this.m_BUTTON_reset.Size = new System.Drawing.Size(31, 19);
			this.m_BUTTON_reset.TabIndex = 21;
			this.m_BUTTON_reset.Text = "Rst";
			this.m_BUTTON_reset.UseVisualStyleBackColor = true;
			this.m_BUTTON_reset.Click += new System.EventHandler(this.on_click_reset);
			// 
			// m_STATIC_SENDED_MESSAGES
			// 
			this.m_STATIC_SENDED_MESSAGES.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_STATIC_SENDED_MESSAGES.Location = new System.Drawing.Point(177, 230);
			this.m_STATIC_SENDED_MESSAGES.Name = "m_STATIC_SENDED_MESSAGES";
			this.m_STATIC_SENDED_MESSAGES.Size = new System.Drawing.Size(88, 20);
			this.m_STATIC_SENDED_MESSAGES.TabIndex = 22;
			this.m_STATIC_SENDED_MESSAGES.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_SENDED_MESSAGES_PERSEC
			// 
			this.m_STATIC_SENDED_MESSAGES_PERSEC.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_STATIC_SENDED_MESSAGES_PERSEC.Location = new System.Drawing.Point(267, 230);
			this.m_STATIC_SENDED_MESSAGES_PERSEC.Name = "m_STATIC_SENDED_MESSAGES_PERSEC";
			this.m_STATIC_SENDED_MESSAGES_PERSEC.Size = new System.Drawing.Size(88, 20);
			this.m_STATIC_SENDED_MESSAGES_PERSEC.TabIndex = 23;
			this.m_STATIC_SENDED_MESSAGES_PERSEC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_RECEIVED_MESSAGES
			// 
			this.m_STATIC_RECEIVED_MESSAGES.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_STATIC_RECEIVED_MESSAGES.Location = new System.Drawing.Point(177, 252);
			this.m_STATIC_RECEIVED_MESSAGES.Name = "m_STATIC_RECEIVED_MESSAGES";
			this.m_STATIC_RECEIVED_MESSAGES.Size = new System.Drawing.Size(88, 20);
			this.m_STATIC_RECEIVED_MESSAGES.TabIndex = 24;
			this.m_STATIC_RECEIVED_MESSAGES.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_RECEIVED_MESSAGES_PERSEC
			// 
			this.m_STATIC_RECEIVED_MESSAGES_PERSEC.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_STATIC_RECEIVED_MESSAGES_PERSEC.Location = new System.Drawing.Point(267, 252);
			this.m_STATIC_RECEIVED_MESSAGES_PERSEC.Name = "m_STATIC_RECEIVED_MESSAGES_PERSEC";
			this.m_STATIC_RECEIVED_MESSAGES_PERSEC.Size = new System.Drawing.Size(88, 20);
			this.m_STATIC_RECEIVED_MESSAGES_PERSEC.TabIndex = 25;
			this.m_STATIC_RECEIVED_MESSAGES_PERSEC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_SENDED_BYTES_PERSEC
			// 
			this.m_STATIC_SENDED_BYTES_PERSEC.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_STATIC_SENDED_BYTES_PERSEC.Location = new System.Drawing.Point(475, 230);
			this.m_STATIC_SENDED_BYTES_PERSEC.Name = "m_STATIC_SENDED_BYTES_PERSEC";
			this.m_STATIC_SENDED_BYTES_PERSEC.Size = new System.Drawing.Size(86, 20);
			this.m_STATIC_SENDED_BYTES_PERSEC.TabIndex = 26;
			this.m_STATIC_SENDED_BYTES_PERSEC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_RECEIVED_BYTES_PERSEC
			// 
			this.m_STATIC_RECEIVED_BYTES_PERSEC.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_STATIC_RECEIVED_BYTES_PERSEC.Location = new System.Drawing.Point(475, 252);
			this.m_STATIC_RECEIVED_BYTES_PERSEC.Name = "m_STATIC_RECEIVED_BYTES_PERSEC";
			this.m_STATIC_RECEIVED_BYTES_PERSEC.Size = new System.Drawing.Size(86, 20);
			this.m_STATIC_RECEIVED_BYTES_PERSEC.TabIndex = 27;
			this.m_STATIC_RECEIVED_BYTES_PERSEC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_MESSAGES_PERSEC_TOTAL
			// 
			this.m_STATIC_MESSAGES_PERSEC_TOTAL.Location = new System.Drawing.Point(259, 277);
			this.m_STATIC_MESSAGES_PERSEC_TOTAL.Name = "m_STATIC_MESSAGES_PERSEC_TOTAL";
			this.m_STATIC_MESSAGES_PERSEC_TOTAL.Size = new System.Drawing.Size(95, 22);
			this.m_STATIC_MESSAGES_PERSEC_TOTAL.TabIndex = 28;
			this.m_STATIC_MESSAGES_PERSEC_TOTAL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_BYTES_PERSEC_TOTAL
			// 
			this.m_STATIC_BYTES_PERSEC_TOTAL.Location = new System.Drawing.Point(464, 277);
			this.m_STATIC_BYTES_PERSEC_TOTAL.Name = "m_STATIC_BYTES_PERSEC_TOTAL";
			this.m_STATIC_BYTES_PERSEC_TOTAL.Size = new System.Drawing.Size(96, 22);
			this.m_STATIC_BYTES_PERSEC_TOTAL.TabIndex = 29;
			this.m_STATIC_BYTES_PERSEC_TOTAL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_TEXT_traffic_info_seperator_2
			// 
			this.m_TEXT_traffic_info_seperator_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_TEXT_traffic_info_seperator_2.Location = new System.Drawing.Point(369, 216);
			this.m_TEXT_traffic_info_seperator_2.Name = "m_TEXT_traffic_info_seperator_2";
			this.m_TEXT_traffic_info_seperator_2.Size = new System.Drawing.Size(1, 65);
			this.m_TEXT_traffic_info_seperator_2.TabIndex = 30;
			// 
			// m_TEXT_total_messages
			// 
			this.m_TEXT_total_messages.AutoSize = true;
			this.m_TEXT_total_messages.Location = new System.Drawing.Point(225, 282);
			this.m_TEXT_total_messages.Name = "m_TEXT_total_messages";
			this.m_TEXT_total_messages.Size = new System.Drawing.Size(31, 13);
			this.m_TEXT_total_messages.TabIndex = 31;
			this.m_TEXT_total_messages.Text = "Total";
			// 
			// m_TEXT_total_bytes
			// 
			this.m_TEXT_total_bytes.AutoSize = true;
			this.m_TEXT_total_bytes.Location = new System.Drawing.Point(430, 282);
			this.m_TEXT_total_bytes.Name = "m_TEXT_total_bytes";
			this.m_TEXT_total_bytes.Size = new System.Drawing.Size(31, 13);
			this.m_TEXT_total_bytes.TabIndex = 32;
			this.m_TEXT_total_bytes.Text = "Total";
			// 
			// m_TEXT_sended_messages
			// 
			this.m_TEXT_sended_messages.AutoSize = true;
			this.m_TEXT_sended_messages.Location = new System.Drawing.Point(70, 234);
			this.m_TEXT_sended_messages.Name = "m_TEXT_sended_messages";
			this.m_TEXT_sended_messages.Size = new System.Drawing.Size(93, 13);
			this.m_TEXT_sended_messages.TabIndex = 33;
			this.m_TEXT_sended_messages.Text = "Sended Messages";
			// 
			// m_TEXT_received_messages
			// 
			this.m_TEXT_received_messages.AutoSize = true;
			this.m_TEXT_received_messages.Location = new System.Drawing.Point(70, 255);
			this.m_TEXT_received_messages.Name = "m_TEXT_received_messages";
			this.m_TEXT_received_messages.Size = new System.Drawing.Size(101, 13);
			this.m_TEXT_received_messages.TabIndex = 34;
			this.m_TEXT_received_messages.Text = "Received Messages";
			// 
			// m_TEXT_sended_bytes
			// 
			this.m_TEXT_sended_bytes.AutoSize = true;
			this.m_TEXT_sended_bytes.Location = new System.Drawing.Point(383, 234);
			this.m_TEXT_sended_bytes.Name = "m_TEXT_sended_bytes";
			this.m_TEXT_sended_bytes.Size = new System.Drawing.Size(73, 13);
			this.m_TEXT_sended_bytes.TabIndex = 35;
			this.m_TEXT_sended_bytes.Text = "Sended Bytes";
			// 
			// m_TEXT_received_bytes
			// 
			this.m_TEXT_received_bytes.AutoSize = true;
			this.m_TEXT_received_bytes.Location = new System.Drawing.Point(383, 255);
			this.m_TEXT_received_bytes.Name = "m_TEXT_received_bytes";
			this.m_TEXT_received_bytes.Size = new System.Drawing.Size(81, 13);
			this.m_TEXT_received_bytes.TabIndex = 36;
			this.m_TEXT_received_bytes.Text = "Received Bytes";
			// 
			// m_TEXT_messages_accumulated
			// 
			this.m_TEXT_messages_accumulated.AutoSize = true;
			this.m_TEXT_messages_accumulated.Location = new System.Drawing.Point(189, 215);
			this.m_TEXT_messages_accumulated.Name = "m_TEXT_messages_accumulated";
			this.m_TEXT_messages_accumulated.Size = new System.Drawing.Size(68, 13);
			this.m_TEXT_messages_accumulated.TabIndex = 37;
			this.m_TEXT_messages_accumulated.Text = "Accumulated";
			// 
			// m_TEXT_messages_per_sec
			// 
			this.m_TEXT_messages_per_sec.AutoSize = true;
			this.m_TEXT_messages_per_sec.Location = new System.Drawing.Point(292, 215);
			this.m_TEXT_messages_per_sec.Name = "m_TEXT_messages_per_sec";
			this.m_TEXT_messages_per_sec.Size = new System.Drawing.Size(43, 13);
			this.m_TEXT_messages_per_sec.TabIndex = 38;
			this.m_TEXT_messages_per_sec.Text = "Per Sec";
			// 
			// m_TEXT_bytes_per_sec
			// 
			this.m_TEXT_bytes_per_sec.AutoSize = true;
			this.m_TEXT_bytes_per_sec.Location = new System.Drawing.Point(500, 215);
			this.m_TEXT_bytes_per_sec.Name = "m_TEXT_bytes_per_sec";
			this.m_TEXT_bytes_per_sec.Size = new System.Drawing.Size(43, 13);
			this.m_TEXT_bytes_per_sec.TabIndex = 39;
			this.m_TEXT_bytes_per_sec.Text = "Per Sec";
			// 
			// m_TEXT_traffic_info_seperator_1
			// 
			this.m_TEXT_traffic_info_seperator_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_TEXT_traffic_info_seperator_1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.m_TEXT_traffic_info_seperator_1.Location = new System.Drawing.Point(172, 275);
			this.m_TEXT_traffic_info_seperator_1.Name = "m_TEXT_traffic_info_seperator_1";
			this.m_TEXT_traffic_info_seperator_1.Size = new System.Drawing.Size(186, 1);
			this.m_TEXT_traffic_info_seperator_1.TabIndex = 40;
			// 
			// m_TEXT_traffic_info_seperator_3
			// 
			this.m_TEXT_traffic_info_seperator_3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_TEXT_traffic_info_seperator_3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.m_TEXT_traffic_info_seperator_3.Location = new System.Drawing.Point(471, 275);
			this.m_TEXT_traffic_info_seperator_3.Name = "m_TEXT_traffic_info_seperator_3";
			this.m_TEXT_traffic_info_seperator_3.Size = new System.Drawing.Size(92, 1);
			this.m_TEXT_traffic_info_seperator_3.TabIndex = 41;
			// 
			// m_TEXT_seperator_bottom
			// 
			this.m_TEXT_seperator_bottom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_TEXT_seperator_bottom.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.m_TEXT_seperator_bottom.Location = new System.Drawing.Point(2, 432);
			this.m_TEXT_seperator_bottom.Name = "m_TEXT_seperator_bottom";
			this.m_TEXT_seperator_bottom.Size = new System.Drawing.Size(630, 1);
			this.m_TEXT_seperator_bottom.TabIndex = 43;
			// 
			// m_GROUP_tcp_connection_info
			// 
			this.m_GROUP_tcp_connection_info.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.m_GROUP_tcp_connection_info.Controls.Add(this.m_EDIT_address);
			this.m_GROUP_tcp_connection_info.Controls.Add(this.m_BUTTON_request_disconnect_all);
			this.m_GROUP_tcp_connection_info.Controls.Add(this.m_BUTTON_disconnect);
			this.m_GROUP_tcp_connection_info.Controls.Add(this.m_BUTTON_connect);
			this.m_GROUP_tcp_connection_info.Controls.Add(this.m_EDIT_connection_times);
			this.m_GROUP_tcp_connection_info.Controls.Add(this.m_BUTTON_connect_100);
			this.m_GROUP_tcp_connection_info.Controls.Add(this.m_BUTTON_connect_10);
			this.m_GROUP_tcp_connection_info.Controls.Add(this.m_BUTTON_connect_1);
			this.m_GROUP_tcp_connection_info.Controls.Add(this.m_TEXT_ms);
			this.m_GROUP_tcp_connection_info.Controls.Add(this.m_TEXT_address);
			this.m_GROUP_tcp_connection_info.Controls.Add(this.m_EDIT_Port);
			this.m_GROUP_tcp_connection_info.Location = new System.Drawing.Point(12, 6);
			this.m_GROUP_tcp_connection_info.Name = "m_GROUP_tcp_connection_info";
			this.m_GROUP_tcp_connection_info.Size = new System.Drawing.Size(615, 174);
			this.m_GROUP_tcp_connection_info.TabIndex = 5;
			this.m_GROUP_tcp_connection_info.TabStop = false;
			this.m_GROUP_tcp_connection_info.Text = "TCP Connection Info";
			// 
			// m_EDIT_address
			// 
			this.m_EDIT_address.Location = new System.Drawing.Point(73, 72);
			this.m_EDIT_address.Name = "m_EDIT_address";
			this.m_EDIT_address.Size = new System.Drawing.Size(115, 20);
			this.m_EDIT_address.TabIndex = 56;
			// 
			// m_BUTTON_request_disconnect_all
			// 
			this.m_BUTTON_request_disconnect_all.Location = new System.Drawing.Point(558, 72);
			this.m_BUTTON_request_disconnect_all.Margin = new System.Windows.Forms.Padding(0);
			this.m_BUTTON_request_disconnect_all.Name = "m_BUTTON_request_disconnect_all";
			this.m_BUTTON_request_disconnect_all.Size = new System.Drawing.Size(45, 23);
			this.m_BUTTON_request_disconnect_all.TabIndex = 55;
			this.m_BUTTON_request_disconnect_all.Text = "Dis All";
			this.m_BUTTON_request_disconnect_all.UseVisualStyleBackColor = true;
			this.m_BUTTON_request_disconnect_all.Click += new System.EventHandler(this.on_BUTTON_request_disconnect_all_Click);
			// 
			// m_BUTTON_disconnect
			// 
			this.m_BUTTON_disconnect.Location = new System.Drawing.Point(524, 72);
			this.m_BUTTON_disconnect.Margin = new System.Windows.Forms.Padding(0);
			this.m_BUTTON_disconnect.Name = "m_BUTTON_disconnect";
			this.m_BUTTON_disconnect.Size = new System.Drawing.Size(34, 23);
			this.m_BUTTON_disconnect.TabIndex = 54;
			this.m_BUTTON_disconnect.Text = "Dis";
			this.m_BUTTON_disconnect.UseVisualStyleBackColor = true;
			this.m_BUTTON_disconnect.Click += new System.EventHandler(this.on_BUTTON_disconnect_click);
			// 
			// m_BUTTON_connect
			// 
			this.m_BUTTON_connect.Location = new System.Drawing.Point(477, 72);
			this.m_BUTTON_connect.Margin = new System.Windows.Forms.Padding(0);
			this.m_BUTTON_connect.Name = "m_BUTTON_connect";
			this.m_BUTTON_connect.Size = new System.Drawing.Size(34, 23);
			this.m_BUTTON_connect.TabIndex = 53;
			this.m_BUTTON_connect.Text = "Con";
			this.m_BUTTON_connect.UseVisualStyleBackColor = true;
			this.m_BUTTON_connect.Click += new System.EventHandler(this.on_BUTTON_connect_Click);
			// 
			// m_EDIT_connection_times
			// 
			this.m_EDIT_connection_times.Location = new System.Drawing.Point(406, 73);
			this.m_EDIT_connection_times.Name = "m_EDIT_connection_times";
			this.m_EDIT_connection_times.Size = new System.Drawing.Size(68, 20);
			this.m_EDIT_connection_times.TabIndex = 50;
			this.m_EDIT_connection_times.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// m_BUTTON_connect_100
			// 
			this.m_BUTTON_connect_100.Location = new System.Drawing.Point(341, 72);
			this.m_BUTTON_connect_100.Margin = new System.Windows.Forms.Padding(0);
			this.m_BUTTON_connect_100.Name = "m_BUTTON_connect_100";
			this.m_BUTTON_connect_100.Size = new System.Drawing.Size(53, 23);
			this.m_BUTTON_connect_100.TabIndex = 52;
			this.m_BUTTON_connect_100.Text = "Con100";
			this.m_BUTTON_connect_100.UseVisualStyleBackColor = true;
			this.m_BUTTON_connect_100.Click += new System.EventHandler(this.on_BUTTON_connect_100_click);
			// 
			// m_BUTTON_connect_10
			// 
			this.m_BUTTON_connect_10.Location = new System.Drawing.Point(295, 72);
			this.m_BUTTON_connect_10.Margin = new System.Windows.Forms.Padding(0);
			this.m_BUTTON_connect_10.Name = "m_BUTTON_connect_10";
			this.m_BUTTON_connect_10.Size = new System.Drawing.Size(46, 23);
			this.m_BUTTON_connect_10.TabIndex = 51;
			this.m_BUTTON_connect_10.Text = "Con10";
			this.m_BUTTON_connect_10.UseVisualStyleBackColor = true;
			this.m_BUTTON_connect_10.Click += new System.EventHandler(this.on_BUTTON_connect_10_click);
			// 
			// m_BUTTON_connect_1
			// 
			this.m_BUTTON_connect_1.Location = new System.Drawing.Point(249, 72);
			this.m_BUTTON_connect_1.Margin = new System.Windows.Forms.Padding(0);
			this.m_BUTTON_connect_1.Name = "m_BUTTON_connect_1";
			this.m_BUTTON_connect_1.Size = new System.Drawing.Size(46, 23);
			this.m_BUTTON_connect_1.TabIndex = 50;
			this.m_BUTTON_connect_1.Text = "Con1";
			this.m_BUTTON_connect_1.UseVisualStyleBackColor = true;
			this.m_BUTTON_connect_1.Click += new System.EventHandler(this.on_BUTTON_connect_1_click);
			// 
			// m_TEXT_ms
			// 
			this.m_TEXT_ms.AutoSize = true;
			this.m_TEXT_ms.Location = new System.Drawing.Point(534, 43);
			this.m_TEXT_ms.Name = "m_TEXT_ms";
			this.m_TEXT_ms.Size = new System.Drawing.Size(20, 13);
			this.m_TEXT_ms.TabIndex = 45;
			this.m_TEXT_ms.Text = "ms";
			// 
			// m_GROUP_traffic_info
			// 
			this.m_GROUP_traffic_info.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.m_GROUP_traffic_info.Controls.Add(this.m_GROUP_request_send);
			this.m_GROUP_traffic_info.Location = new System.Drawing.Point(13, 190);
			this.m_GROUP_traffic_info.Name = "m_GROUP_traffic_info";
			this.m_GROUP_traffic_info.Size = new System.Drawing.Size(611, 234);
			this.m_GROUP_traffic_info.TabIndex = 44;
			this.m_GROUP_traffic_info.TabStop = false;
			this.m_GROUP_traffic_info.Text = "Traffic Info";
			this.m_GROUP_traffic_info.Enter += new System.EventHandler(this.on_GROUP_traffic_info_enter);
			// 
			// m_GROUP_request_send
			// 
			this.m_GROUP_request_send.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.m_GROUP_request_send.Controls.Add(this.m_BUTTON_send);
			this.m_GROUP_request_send.Controls.Add(this.m_CHECK_request_send_start_stop);
			this.m_GROUP_request_send.Controls.Add(this.m_COMBO_send_test_message_size);
			this.m_GROUP_request_send.Controls.Add(this.m_TEXT_error_send_zero);
			this.m_GROUP_request_send.Controls.Add(this.m_TEXT_single_send);
			this.m_GROUP_request_send.Controls.Add(this.m_BUTTON_error_send_zero);
			this.m_GROUP_request_send.Controls.Add(this.m_BUTTON_single_send);
			this.m_GROUP_request_send.Controls.Add(this.m_TEXT_send_message_size);
			this.m_GROUP_request_send.Controls.Add(this.m_TEXT_request_send_times);
			this.m_GROUP_request_send.Controls.Add(this.m_EDIT_send_test_times);
			this.m_GROUP_request_send.Controls.Add(this.m_TEXT_explanation_2);
			this.m_GROUP_request_send.Location = new System.Drawing.Point(8, 129);
			this.m_GROUP_request_send.Name = "m_GROUP_request_send";
			this.m_GROUP_request_send.Size = new System.Drawing.Size(594, 97);
			this.m_GROUP_request_send.TabIndex = 5;
			this.m_GROUP_request_send.TabStop = false;
			this.m_GROUP_request_send.Text = "Send Test";
			// 
			// m_BUTTON_send
			// 
			this.m_BUTTON_send.Location = new System.Drawing.Point(302, 62);
			this.m_BUTTON_send.Name = "m_BUTTON_send";
			this.m_BUTTON_send.Size = new System.Drawing.Size(60, 24);
			this.m_BUTTON_send.TabIndex = 60;
			this.m_BUTTON_send.Text = "Send";
			this.m_BUTTON_send.Click += new System.EventHandler(this.on_BUTTON_send_click);
			// 
			// m_CHECK_request_send_start_stop
			// 
			this.m_CHECK_request_send_start_stop.Appearance = System.Windows.Forms.Appearance.Button;
			this.m_CHECK_request_send_start_stop.Location = new System.Drawing.Point(237, 62);
			this.m_CHECK_request_send_start_stop.Name = "m_CHECK_request_send_start_stop";
			this.m_CHECK_request_send_start_stop.Size = new System.Drawing.Size(66, 24);
			this.m_CHECK_request_send_start_stop.TabIndex = 0;
			this.m_CHECK_request_send_start_stop.Text = "Start/Stop";
			this.m_CHECK_request_send_start_stop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.m_CHECK_request_send_start_stop.CheckedChanged += new System.EventHandler(this.on_CHECK_request_send_start_stop_checked_changed);
			// 
			// m_COMBO_send_test_message_size
			// 
			this.m_COMBO_send_test_message_size.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_COMBO_send_test_message_size.Items.AddRange(new object[] {
            "8Byte",
            "64Byte",
            "256Byte",
            "1KByte",
            "4KByte",
            "16KByte",
            "64KByte"});
			this.m_COMBO_send_test_message_size.Location = new System.Drawing.Point(137, 63);
			this.m_COMBO_send_test_message_size.Name = "m_COMBO_send_test_message_size";
			this.m_COMBO_send_test_message_size.Size = new System.Drawing.Size(100, 21);
			this.m_COMBO_send_test_message_size.TabIndex = 0;
			this.m_COMBO_send_test_message_size.SelectedIndexChanged += new System.EventHandler(this.on_COMBO_send_test_message_size_selected_index_changed);
			// 
			// m_TEXT_error_send_zero
			// 
			this.m_TEXT_error_send_zero.AutoSize = true;
			this.m_TEXT_error_send_zero.Location = new System.Drawing.Point(470, 48);
			this.m_TEXT_error_send_zero.Name = "m_TEXT_error_send_zero";
			this.m_TEXT_error_send_zero.Size = new System.Drawing.Size(58, 13);
			this.m_TEXT_error_send_zero.TabIndex = 56;
			this.m_TEXT_error_send_zero.Text = "Error Send";
			// 
			// m_TEXT_single_send
			// 
			this.m_TEXT_single_send.AutoSize = true;
			this.m_TEXT_single_send.Location = new System.Drawing.Point(388, 48);
			this.m_TEXT_single_send.Name = "m_TEXT_single_send";
			this.m_TEXT_single_send.Size = new System.Drawing.Size(62, 13);
			this.m_TEXT_single_send.TabIndex = 55;
			this.m_TEXT_single_send.Text = "Single Send";
			// 
			// m_BUTTON_error_send_zero
			// 
			this.m_BUTTON_error_send_zero.Location = new System.Drawing.Point(469, 63);
			this.m_BUTTON_error_send_zero.Name = "m_BUTTON_error_send_zero";
			this.m_BUTTON_error_send_zero.Size = new System.Drawing.Size(75, 23);
			this.m_BUTTON_error_send_zero.TabIndex = 57;
			this.m_BUTTON_error_send_zero.Text = "(E)Zero";
			this.m_BUTTON_error_send_zero.Click += new System.EventHandler(this.on_BUTTON_error_send_zero_click);
			// 
			// m_BUTTON_single_send
			// 
			this.m_BUTTON_single_send.Location = new System.Drawing.Point(380, 63);
			this.m_BUTTON_single_send.Name = "m_BUTTON_single_send";
			this.m_BUTTON_single_send.Size = new System.Drawing.Size(75, 23);
			this.m_BUTTON_single_send.TabIndex = 58;
			this.m_BUTTON_single_send.Text = "Send";
			this.m_BUTTON_single_send.Click += new System.EventHandler(this.on_BUTTON_single_send_click);
			// 
			// m_TEXT_send_message_size
			// 
			this.m_TEXT_send_message_size.AutoSize = true;
			this.m_TEXT_send_message_size.Location = new System.Drawing.Point(163, 48);
			this.m_TEXT_send_message_size.Name = "m_TEXT_send_message_size";
			this.m_TEXT_send_message_size.Size = new System.Drawing.Size(26, 13);
			this.m_TEXT_send_message_size.TabIndex = 51;
			this.m_TEXT_send_message_size.Text = "Size";
			// 
			// m_TEXT_request_send_times
			// 
			this.m_TEXT_request_send_times.AutoSize = true;
			this.m_TEXT_request_send_times.Location = new System.Drawing.Point(92, 48);
			this.m_TEXT_request_send_times.Name = "m_TEXT_request_send_times";
			this.m_TEXT_request_send_times.Size = new System.Drawing.Size(34, 13);
			this.m_TEXT_request_send_times.TabIndex = 50;
			this.m_TEXT_request_send_times.Text = "Times";
			// 
			// m_EDIT_send_test_times
			// 
			this.m_EDIT_send_test_times.Location = new System.Drawing.Point(46, 64);
			this.m_EDIT_send_test_times.Name = "m_EDIT_send_test_times";
			this.m_EDIT_send_test_times.Size = new System.Drawing.Size(89, 20);
			this.m_EDIT_send_test_times.TabIndex = 59;
			this.m_EDIT_send_test_times.TextChanged += new System.EventHandler(this.on_EDIT_send_test_times_TextChanged);
			// 
			// m_TEXT_explanation_2
			// 
			this.m_TEXT_explanation_2.AutoSize = true;
			this.m_TEXT_explanation_2.Location = new System.Drawing.Point(16, 25);
			this.m_TEXT_explanation_2.Name = "m_TEXT_explanation_2";
			this.m_TEXT_explanation_2.Size = new System.Drawing.Size(422, 13);
			this.m_TEXT_explanation_2.TabIndex = 46;
			this.m_TEXT_explanation_2.Text = "접속된 모든 Socket에 100ms마다 설정된 Size의 메시지를 설정된 Times번 전송합니다.";
			// 
			// m_TEXT_tcp_connection_info_seperator_3
			// 
			this.m_TEXT_tcp_connection_info_seperator_3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_TEXT_tcp_connection_info_seperator_3.Location = new System.Drawing.Point(463, 29);
			this.m_TEXT_tcp_connection_info_seperator_3.Name = "m_TEXT_tcp_connection_info_seperator_3";
			this.m_TEXT_tcp_connection_info_seperator_3.Size = new System.Drawing.Size(1, 32);
			this.m_TEXT_tcp_connection_info_seperator_3.TabIndex = 45;
			// 
			// m_TEXT_tcp_connection_info_seperator_4
			// 
			this.m_TEXT_tcp_connection_info_seperator_4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_TEXT_tcp_connection_info_seperator_4.Location = new System.Drawing.Point(254, 81);
			this.m_TEXT_tcp_connection_info_seperator_4.Name = "m_TEXT_tcp_connection_info_seperator_4";
			this.m_TEXT_tcp_connection_info_seperator_4.Size = new System.Drawing.Size(1, 16);
			this.m_TEXT_tcp_connection_info_seperator_4.TabIndex = 46;
			// 
			// m_TEXT_tcp_connection_info_seperator_5
			// 
			this.m_TEXT_tcp_connection_info_seperator_5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_TEXT_tcp_connection_info_seperator_5.Location = new System.Drawing.Point(411, 81);
			this.m_TEXT_tcp_connection_info_seperator_5.Name = "m_TEXT_tcp_connection_info_seperator_5";
			this.m_TEXT_tcp_connection_info_seperator_5.Size = new System.Drawing.Size(1, 16);
			this.m_TEXT_tcp_connection_info_seperator_5.TabIndex = 47;
			// 
			// m_TEXT_traffic_info_seperator_4
			// 
			this.m_TEXT_traffic_info_seperator_4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_TEXT_traffic_info_seperator_4.Location = new System.Drawing.Point(393, 385);
			this.m_TEXT_traffic_info_seperator_4.Name = "m_TEXT_traffic_info_seperator_4";
			this.m_TEXT_traffic_info_seperator_4.Size = new System.Drawing.Size(1, 16);
			this.m_TEXT_traffic_info_seperator_4.TabIndex = 48;
			// 
			// m_TEXT_traffic_info_seperator_5
			// 
			this.m_TEXT_traffic_info_seperator_5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_TEXT_traffic_info_seperator_5.Location = new System.Drawing.Point(482, 386);
			this.m_TEXT_traffic_info_seperator_5.Name = "m_TEXT_traffic_info_seperator_5";
			this.m_TEXT_traffic_info_seperator_5.Size = new System.Drawing.Size(1, 16);
			this.m_TEXT_traffic_info_seperator_5.TabIndex = 49;
			// 
			// m_TEXT_tcp_connection_info_seperator_6
			// 
			this.m_TEXT_tcp_connection_info_seperator_6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_TEXT_tcp_connection_info_seperator_6.Location = new System.Drawing.Point(529, 80);
			this.m_TEXT_tcp_connection_info_seperator_6.Name = "m_TEXT_tcp_connection_info_seperator_6";
			this.m_TEXT_tcp_connection_info_seperator_6.Size = new System.Drawing.Size(1, 16);
			this.m_TEXT_tcp_connection_info_seperator_6.TabIndex = 50;
			// 
			// SampleClientForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(636, 464);
			this.Controls.Add(this.m_TEXT_tcp_connection_info_seperator_6);
			this.Controls.Add(this.m_TEXT_traffic_info_seperator_5);
			this.Controls.Add(this.m_TEXT_traffic_info_seperator_4);
			this.Controls.Add(this.m_TEXT_tcp_connection_info_seperator_5);
			this.Controls.Add(this.m_TEXT_tcp_connection_info_seperator_4);
			this.Controls.Add(this.m_TEXT_tcp_connection_info_seperator_3);
			this.Controls.Add(this.m_TEXT_seperator_bottom);
			this.Controls.Add(this.m_TEXT_traffic_info_seperator_3);
			this.Controls.Add(this.m_TEXT_traffic_info_seperator_1);
			this.Controls.Add(this.m_TEXT_bytes_per_sec);
			this.Controls.Add(this.m_TEXT_messages_per_sec);
			this.Controls.Add(this.m_TEXT_messages_accumulated);
			this.Controls.Add(this.m_TEXT_received_bytes);
			this.Controls.Add(this.m_TEXT_sended_bytes);
			this.Controls.Add(this.m_TEXT_received_messages);
			this.Controls.Add(this.m_TEXT_sended_messages);
			this.Controls.Add(this.m_TEXT_total_bytes);
			this.Controls.Add(this.m_TEXT_total_messages);
			this.Controls.Add(this.m_TEXT_traffic_info_seperator_2);
			this.Controls.Add(this.m_STATIC_BYTES_PERSEC_TOTAL);
			this.Controls.Add(this.m_STATIC_MESSAGES_PERSEC_TOTAL);
			this.Controls.Add(this.m_STATIC_RECEIVED_BYTES_PERSEC);
			this.Controls.Add(this.m_STATIC_SENDED_BYTES_PERSEC);
			this.Controls.Add(this.m_STATIC_RECEIVED_MESSAGES_PERSEC);
			this.Controls.Add(this.m_STATIC_RECEIVED_MESSAGES);
			this.Controls.Add(this.m_STATIC_SENDED_MESSAGES_PERSEC);
			this.Controls.Add(this.m_STATIC_SENDED_MESSAGES);
			this.Controls.Add(this.m_BUTTON_reset);
			this.Controls.Add(this.m_TEXT_seperator_2);
			this.Controls.Add(this.m_TEXT_tcp_connection_info_seperator_1);
			this.Controls.Add(this.m_STATIC_delay);
			this.Controls.Add(this.m_STATIC_force_dis);
			this.Controls.Add(this.m_STATIC_failure);
			this.Controls.Add(this.m_STATIC_success);
			this.Controls.Add(this.m_STATIC_try);
			this.Controls.Add(this.m_STATIC_connection);
			this.Controls.Add(this.m_TEXT_bottom);
			this.Controls.Add(this.m_TEXT_delay);
			this.Controls.Add(this.m_TEXT_force_dis);
			this.Controls.Add(this.m_TEXT_failure);
			this.Controls.Add(this.m_TEXT_success);
			this.Controls.Add(this.m_TEXT_try);
			this.Controls.Add(this.m_TEXT_connection);
			this.Controls.Add(this.m_GROUP_traffic_info);
			this.Controls.Add(this.m_GROUP_connection_test);
			this.Controls.Add(this.m_GROUP_tcp_connection_info);
			this.Font = new System.Drawing.Font("Tahoma", 8F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "SampleClientForm";
			this.Text = "CGCII Network Sample TCP Echo Test Client for C#";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
			this.Load += new System.EventHandler(this.OnFormLoad);
			this.m_GROUP_connection_test.ResumeLayout(false);
			this.m_GROUP_connection_test.PerformLayout();
			this.m_GROUP_tcp_connection_info.ResumeLayout(false);
			this.m_GROUP_tcp_connection_info.PerformLayout();
			this.m_GROUP_traffic_info.ResumeLayout(false);
			this.m_GROUP_request_send.ResumeLayout(false);
			this.m_GROUP_request_send.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion

		private System.Windows.Forms.TextBox m_EDIT_Port;
		private System.Windows.Forms.Label m_TEXT_address;
		private System.Windows.Forms.GroupBox m_GROUP_connection_test;
		private System.Windows.Forms.Label m_TEXT_connection;
		private System.Windows.Forms.Label m_TEXT_try;
		private System.Windows.Forms.Label m_TEXT_success;
		private System.Windows.Forms.Label m_TEXT_failure;
		private System.Windows.Forms.Label m_TEXT_force_dis;
		private System.Windows.Forms.Label m_TEXT_delay;
		private System.Windows.Forms.Label m_TEXT_bottom;
		private System.Windows.Forms.Label m_STATIC_connection;
		private System.Windows.Forms.Label m_STATIC_try;
		private System.Windows.Forms.Label m_STATIC_success;
		private System.Windows.Forms.Label m_STATIC_failure;
		private System.Windows.Forms.Label m_STATIC_force_dis;
		private System.Windows.Forms.Label m_STATIC_delay;
		private System.Windows.Forms.Label m_TEXT_tcp_connection_info_seperator_1;
		private System.Windows.Forms.Label m_TEXT_seperator_2;
		private System.Windows.Forms.Button m_BUTTON_reset;
		private System.Windows.Forms.Label m_STATIC_SENDED_MESSAGES;
		private System.Windows.Forms.Label m_STATIC_SENDED_MESSAGES_PERSEC;
		private System.Windows.Forms.Label m_STATIC_RECEIVED_MESSAGES;
		private System.Windows.Forms.Label m_STATIC_RECEIVED_MESSAGES_PERSEC;
		private System.Windows.Forms.Label m_STATIC_SENDED_BYTES_PERSEC;
		private System.Windows.Forms.Label m_STATIC_RECEIVED_BYTES_PERSEC;
		private System.Windows.Forms.Label m_STATIC_MESSAGES_PERSEC_TOTAL;
		private System.Windows.Forms.Label m_STATIC_BYTES_PERSEC_TOTAL;
		private System.Windows.Forms.Label m_TEXT_traffic_info_seperator_2;
		private System.Windows.Forms.Label m_TEXT_total_messages;
		private System.Windows.Forms.Label m_TEXT_total_bytes;
		private System.Windows.Forms.Label m_TEXT_sended_messages;
		private System.Windows.Forms.Label m_TEXT_received_messages;
		private System.Windows.Forms.Label m_TEXT_sended_bytes;
		private System.Windows.Forms.Label m_TEXT_received_bytes;
		private System.Windows.Forms.Label m_TEXT_messages_accumulated;
		private System.Windows.Forms.Label m_TEXT_messages_per_sec;
		private System.Windows.Forms.Label m_TEXT_bytes_per_sec;
		private System.Windows.Forms.Label m_TEXT_traffic_info_seperator_1;
		private System.Windows.Forms.Label m_TEXT_traffic_info_seperator_3;
		private System.Windows.Forms.Label m_TEXT_seperator_bottom;
		private System.Windows.Forms.GroupBox m_GROUP_tcp_connection_info;
		private System.Windows.Forms.GroupBox m_GROUP_traffic_info;
		private System.Windows.Forms.GroupBox m_GROUP_request_send;
		private System.Windows.Forms.Label m_TEXT_explanation_1;
		private System.Windows.Forms.Label m_TEXT_explanation_2;
		private System.Windows.Forms.Label m_TEXT_ms;
		private System.Windows.Forms.Label m_TEXT_connection_test_range;
		private System.Windows.Forms.Label m_TEXT_connection_test_interval;
		private System.Windows.Forms.TextBox m_EDIT_connection_test_range_end;
		private System.Windows.Forms.TextBox m_EDIT_connection_test_range_start;
		private System.Windows.Forms.TextBox m_EDIT_connection_test_interval;
		private System.Windows.Forms.Label m_TEXT_connection_test_rangeWave;
		private System.Windows.Forms.Button m_BUTTON_error_send_zero;
		private System.Windows.Forms.Button m_BUTTON_single_send;
		private System.Windows.Forms.Label m_TEXT_send_message_size;
		private System.Windows.Forms.Label m_TEXT_request_send_times;
		private System.Windows.Forms.TextBox m_EDIT_send_test_times;
		private System.Windows.Forms.ComboBox m_COMBO_send_test_message_size;
		private System.Windows.Forms.Label m_TEXT_error_send_zero;
		private System.Windows.Forms.Label m_TEXT_single_send;
		private System.Windows.Forms.Button m_BUTTON_connect_100;
		private System.Windows.Forms.Button m_BUTTON_connect_10;
		private System.Windows.Forms.Button m_BUTTON_connect_1;
		private System.Windows.Forms.Button m_BUTTON_request_disconnect_all;
		private System.Windows.Forms.Button m_BUTTON_disconnect;
		private System.Windows.Forms.Button m_BUTTON_connect;
		private System.Windows.Forms.TextBox m_EDIT_connection_times;
		private System.Windows.Forms.TextBox m_EDIT_address;
		private System.Windows.Forms.Label m_TEXT_tcp_connection_info_seperator_3;
		private System.Windows.Forms.Label m_TEXT_tcp_connection_info_seperator_4;
		private System.Windows.Forms.Label m_TEXT_tcp_connection_info_seperator_5;
		private System.Windows.Forms.Label m_TEXT_traffic_info_seperator_4;
		private System.Windows.Forms.Label m_TEXT_traffic_info_seperator_5;
		private System.Windows.Forms.Label m_TEXT_tcp_connection_info_seperator_6;
		private System.Windows.Forms.CheckBox m_CHECK_connection_test_start_stop;
		private System.Windows.Forms.CheckBox m_CHECK_request_send_start_stop;
		private System.Windows.Forms.Button m_BUTTON_send;
	}
}

