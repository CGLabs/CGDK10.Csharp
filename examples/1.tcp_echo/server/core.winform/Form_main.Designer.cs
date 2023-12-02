namespace sample.tcp_echo.server.winform
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
			this.m_EDIT_BIND_PORT = new System.Windows.Forms.TextBox();
			this.m_STATIC_ADDRESS = new System.Windows.Forms.Label();
			this.m_TEXT_bind_port = new System.Windows.Forms.Label();
			this.m_GROUP_acceptor = new System.Windows.Forms.GroupBox();
			this.m_CHECK_start_accept = new System.Windows.Forms.CheckBox();
			this.m_TEXT_connection = new System.Windows.Forms.Label();
			this.m_TEXT_try = new System.Windows.Forms.Label();
			this.m_TEXT_connected = new System.Windows.Forms.Label();
			this.m_TEXT_disconnected = new System.Windows.Forms.Label();
			this.m_TEXT_fail_to_connect = new System.Windows.Forms.Label();
			this.m_TEXT_error_close = new System.Windows.Forms.Label();
			this.m_TEXT_bottom = new System.Windows.Forms.Label();
			this.m_STATIC_KEEP_CONNECT = new System.Windows.Forms.Label();
			this.m_STATIC_TRY = new System.Windows.Forms.Label();
			this.m_STATIC_CONNECTED = new System.Windows.Forms.Label();
			this.m_STATIC_DISCONNECTED = new System.Windows.Forms.Label();
			this.m_STATIC_FAIL_CONNECT = new System.Windows.Forms.Label();
			this.m_STATIC_ERROR_CLOSE = new System.Windows.Forms.Label();
			this.m_TEXT_seperator_1 = new System.Windows.Forms.Label();
			this.m_TEXT_seperator_2 = new System.Windows.Forms.Label();
			this.m_BUTTON_DISCONNECT_ALL = new System.Windows.Forms.Button();
			this.m_STATIC_SENDED_MESSAGES = new System.Windows.Forms.Label();
			this.m_STATIC_SENDED_MESSAGES_PERSEC = new System.Windows.Forms.Label();
			this.m_STATIC_RECEIVED_MESSAGES = new System.Windows.Forms.Label();
			this.m_STATIC_RECEIVED_MESSAGES_PERSEC = new System.Windows.Forms.Label();
			this.m_STATIC_SENDED_BYTES_PERSEC = new System.Windows.Forms.Label();
			this.m_STATIC_RECEIVED_BYTES_PERSEC = new System.Windows.Forms.Label();
			this.m_STATIC_MESSAGES_PERSEC_TOTAL = new System.Windows.Forms.Label();
			this.m_STATIC_BYTES_PERSEC_TOTAL = new System.Windows.Forms.Label();
			this.m_TEXT_seperator_statistics = new System.Windows.Forms.Label();
			this.m_TEXT_total_messages = new System.Windows.Forms.Label();
			this.m_TEXT_total_bytes = new System.Windows.Forms.Label();
			this.m_TEXT_sended_messages = new System.Windows.Forms.Label();
			this.m_TEXT_received_messages = new System.Windows.Forms.Label();
			this.m_TEXT_sended_bytes = new System.Windows.Forms.Label();
			this.m_TEXT_ReceivedBytes = new System.Windows.Forms.Label();
			this.m_TEXT_messages_accumulated = new System.Windows.Forms.Label();
			this.m_TEXT_messages_per_sec = new System.Windows.Forms.Label();
			this.m_TEXT_bytes_per_sec = new System.Windows.Forms.Label();
			this.m_TEXT_seperator_total_message = new System.Windows.Forms.Label();
			this.m_TEXT_seperator_total_bytes = new System.Windows.Forms.Label();
			this.m_TEXT_seperator_bottom = new System.Windows.Forms.Label();
			this.m_GROUP_acceptor.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_EDIT_BIND_PORT
			// 
			this.m_EDIT_BIND_PORT.Location = new System.Drawing.Point(208, 27);
			this.m_EDIT_BIND_PORT.Name = "m_EDIT_BIND_PORT";
			this.m_EDIT_BIND_PORT.Size = new System.Drawing.Size(45, 20);
			this.m_EDIT_BIND_PORT.TabIndex = 3;
			// 
			// m_STATIC_ADDRESS
			// 
			this.m_STATIC_ADDRESS.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_STATIC_ADDRESS.Location = new System.Drawing.Point(98, 27);
			this.m_STATIC_ADDRESS.Name = "m_STATIC_ADDRESS";
			this.m_STATIC_ADDRESS.Size = new System.Drawing.Size(109, 20);
			this.m_STATIC_ADDRESS.TabIndex = 2;
			this.m_STATIC_ADDRESS.Text = "ADDR_ANY";
			this.m_STATIC_ADDRESS.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_TEXT_bind_port
			// 
			this.m_TEXT_bind_port.AutoSize = true;
			this.m_TEXT_bind_port.Location = new System.Drawing.Point(43, 30);
			this.m_TEXT_bind_port.Name = "m_TEXT_bind_port";
			this.m_TEXT_bind_port.Size = new System.Drawing.Size(50, 13);
			this.m_TEXT_bind_port.TabIndex = 0;
			this.m_TEXT_bind_port.Text = "Bind Port";
			// 
			// m_GROUP_acceptor
			// 
			this.m_GROUP_acceptor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.m_GROUP_acceptor.Controls.Add(this.m_CHECK_start_accept);
			this.m_GROUP_acceptor.Location = new System.Drawing.Point(10, 7);
			this.m_GROUP_acceptor.Name = "m_GROUP_acceptor";
			this.m_GROUP_acceptor.Size = new System.Drawing.Size(550, 51);
			this.m_GROUP_acceptor.TabIndex = 4;
			this.m_GROUP_acceptor.TabStop = false;
			// 
			// m_CHECK_start_accept
			// 
			this.m_CHECK_start_accept.Appearance = System.Windows.Forms.Appearance.Button;
			this.m_CHECK_start_accept.Location = new System.Drawing.Point(244, 19);
			this.m_CHECK_start_accept.Name = "m_CHECK_start_accept";
			this.m_CHECK_start_accept.Size = new System.Drawing.Size(39, 22);
			this.m_CHECK_start_accept.TabIndex = 0;
			this.m_CHECK_start_accept.Text = "start";
			this.m_CHECK_start_accept.UseVisualStyleBackColor = true;
			this.m_CHECK_start_accept.CheckedChanged += new System.EventHandler(this.m_CHECK_start_accept_CheckedChanged);
			// 
			// m_TEXT_connection
			// 
			this.m_TEXT_connection.AutoSize = true;
			this.m_TEXT_connection.Location = new System.Drawing.Point(38, 83);
			this.m_TEXT_connection.Name = "m_TEXT_connection";
			this.m_TEXT_connection.Size = new System.Drawing.Size(61, 13);
			this.m_TEXT_connection.TabIndex = 5;
			this.m_TEXT_connection.Text = "Connection";
			// 
			// m_TEXT_try
			// 
			this.m_TEXT_try.AutoSize = true;
			this.m_TEXT_try.Location = new System.Drawing.Point(137, 83);
			this.m_TEXT_try.Name = "m_TEXT_try";
			this.m_TEXT_try.Size = new System.Drawing.Size(23, 13);
			this.m_TEXT_try.TabIndex = 6;
			this.m_TEXT_try.Text = "Try";
			// 
			// m_TEXT_connected
			// 
			this.m_TEXT_connected.AutoSize = true;
			this.m_TEXT_connected.Location = new System.Drawing.Point(194, 83);
			this.m_TEXT_connected.Name = "m_TEXT_connected";
			this.m_TEXT_connected.Size = new System.Drawing.Size(59, 13);
			this.m_TEXT_connected.TabIndex = 7;
			this.m_TEXT_connected.Text = "Connected";
			// 
			// m_TEXT_disconnected
			// 
			this.m_TEXT_disconnected.AutoSize = true;
			this.m_TEXT_disconnected.Location = new System.Drawing.Point(263, 83);
			this.m_TEXT_disconnected.Name = "m_TEXT_disconnected";
			this.m_TEXT_disconnected.Size = new System.Drawing.Size(71, 13);
			this.m_TEXT_disconnected.TabIndex = 8;
			this.m_TEXT_disconnected.Text = "Disconnected";
			// 
			// m_TEXT_fail_to_connect
			// 
			this.m_TEXT_fail_to_connect.AutoSize = true;
			this.m_TEXT_fail_to_connect.Location = new System.Drawing.Point(348, 83);
			this.m_TEXT_fail_to_connect.Name = "m_TEXT_fail_to_connect";
			this.m_TEXT_fail_to_connect.Size = new System.Drawing.Size(66, 13);
			this.m_TEXT_fail_to_connect.TabIndex = 9;
			this.m_TEXT_fail_to_connect.Text = "Fail Connect";
			// 
			// m_TEXT_error_close
			// 
			this.m_TEXT_error_close.AutoSize = true;
			this.m_TEXT_error_close.Location = new System.Drawing.Point(427, 83);
			this.m_TEXT_error_close.Name = "m_TEXT_error_close";
			this.m_TEXT_error_close.Size = new System.Drawing.Size(60, 13);
			this.m_TEXT_error_close.TabIndex = 10;
			this.m_TEXT_error_close.Text = "Error Close";
			// 
			// m_TEXT_bottom
			// 
			this.m_TEXT_bottom.AutoSize = true;
			this.m_TEXT_bottom.Enabled = false;
			this.m_TEXT_bottom.Font = new System.Drawing.Font("Tahoma", 8F);
			this.m_TEXT_bottom.ForeColor = System.Drawing.SystemColors.ButtonShadow;
			this.m_TEXT_bottom.Location = new System.Drawing.Point(138, 268);
			this.m_TEXT_bottom.Name = "m_TEXT_bottom";
			this.m_TEXT_bottom.Size = new System.Drawing.Size(307, 13);
			this.m_TEXT_bottom.TabIndex = 12;
			this.m_TEXT_bottom.Text = "CGCII Network Classes for C#. (C) CGLabs. All right reserved.";
			// 
			// m_STATIC_KEEP_CONNECT
			// 
			this.m_STATIC_KEEP_CONNECT.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_STATIC_KEEP_CONNECT.Location = new System.Drawing.Point(33, 99);
			this.m_STATIC_KEEP_CONNECT.Name = "m_STATIC_KEEP_CONNECT";
			this.m_STATIC_KEEP_CONNECT.Size = new System.Drawing.Size(68, 20);
			this.m_STATIC_KEEP_CONNECT.TabIndex = 13;
			this.m_STATIC_KEEP_CONNECT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_TRY
			// 
			this.m_STATIC_TRY.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_STATIC_TRY.Location = new System.Drawing.Point(110, 99);
			this.m_STATIC_TRY.Name = "m_STATIC_TRY";
			this.m_STATIC_TRY.Size = new System.Drawing.Size(74, 20);
			this.m_STATIC_TRY.TabIndex = 14;
			this.m_STATIC_TRY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_CONNECTED
			// 
			this.m_STATIC_CONNECTED.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_STATIC_CONNECTED.Location = new System.Drawing.Point(185, 99);
			this.m_STATIC_CONNECTED.Name = "m_STATIC_CONNECTED";
			this.m_STATIC_CONNECTED.Size = new System.Drawing.Size(74, 20);
			this.m_STATIC_CONNECTED.TabIndex = 15;
			this.m_STATIC_CONNECTED.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_DISCONNECTED
			// 
			this.m_STATIC_DISCONNECTED.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_STATIC_DISCONNECTED.Location = new System.Drawing.Point(260, 99);
			this.m_STATIC_DISCONNECTED.Name = "m_STATIC_DISCONNECTED";
			this.m_STATIC_DISCONNECTED.Size = new System.Drawing.Size(74, 20);
			this.m_STATIC_DISCONNECTED.TabIndex = 16;
			this.m_STATIC_DISCONNECTED.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_FAIL_CONNECT
			// 
			this.m_STATIC_FAIL_CONNECT.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_STATIC_FAIL_CONNECT.Location = new System.Drawing.Point(343, 99);
			this.m_STATIC_FAIL_CONNECT.Name = "m_STATIC_FAIL_CONNECT";
			this.m_STATIC_FAIL_CONNECT.Size = new System.Drawing.Size(74, 20);
			this.m_STATIC_FAIL_CONNECT.TabIndex = 17;
			this.m_STATIC_FAIL_CONNECT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_ERROR_CLOSE
			// 
			this.m_STATIC_ERROR_CLOSE.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_STATIC_ERROR_CLOSE.Location = new System.Drawing.Point(419, 99);
			this.m_STATIC_ERROR_CLOSE.Name = "m_STATIC_ERROR_CLOSE";
			this.m_STATIC_ERROR_CLOSE.Size = new System.Drawing.Size(74, 20);
			this.m_STATIC_ERROR_CLOSE.TabIndex = 18;
			this.m_STATIC_ERROR_CLOSE.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_TEXT_seperator_1
			// 
			this.m_TEXT_seperator_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_TEXT_seperator_1.Location = new System.Drawing.Point(105, 87);
			this.m_TEXT_seperator_1.Name = "m_TEXT_seperator_1";
			this.m_TEXT_seperator_1.Size = new System.Drawing.Size(1, 32);
			this.m_TEXT_seperator_1.TabIndex = 19;
			// 
			// m_TEXT_seperator_2
			// 
			this.m_TEXT_seperator_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_TEXT_seperator_2.Location = new System.Drawing.Point(338, 87);
			this.m_TEXT_seperator_2.Name = "m_TEXT_seperator_2";
			this.m_TEXT_seperator_2.Size = new System.Drawing.Size(1, 32);
			this.m_TEXT_seperator_2.TabIndex = 20;
			// 
			// m_BUTTON_DISCONNECT_ALL
			// 
			this.m_BUTTON_DISCONNECT_ALL.Location = new System.Drawing.Point(494, 98);
			this.m_BUTTON_DISCONNECT_ALL.Margin = new System.Windows.Forms.Padding(0);
			this.m_BUTTON_DISCONNECT_ALL.Name = "m_BUTTON_DISCONNECT_ALL";
			this.m_BUTTON_DISCONNECT_ALL.Size = new System.Drawing.Size(40, 20);
			this.m_BUTTON_DISCONNECT_ALL.TabIndex = 21;
			this.m_BUTTON_DISCONNECT_ALL.Text = "Dis all";
			this.m_BUTTON_DISCONNECT_ALL.UseVisualStyleBackColor = true;
			this.m_BUTTON_DISCONNECT_ALL.Click += new System.EventHandler(this.OnClick_DISCONNECT_ALL);
			// 
			// m_STATIC_SENDED_MESSAGES
			// 
			this.m_STATIC_SENDED_MESSAGES.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_STATIC_SENDED_MESSAGES.Location = new System.Drawing.Point(136, 169);
			this.m_STATIC_SENDED_MESSAGES.Name = "m_STATIC_SENDED_MESSAGES";
			this.m_STATIC_SENDED_MESSAGES.Size = new System.Drawing.Size(88, 20);
			this.m_STATIC_SENDED_MESSAGES.TabIndex = 22;
			this.m_STATIC_SENDED_MESSAGES.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_SENDED_MESSAGES_PERSEC
			// 
			this.m_STATIC_SENDED_MESSAGES_PERSEC.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_STATIC_SENDED_MESSAGES_PERSEC.Location = new System.Drawing.Point(226, 169);
			this.m_STATIC_SENDED_MESSAGES_PERSEC.Name = "m_STATIC_SENDED_MESSAGES_PERSEC";
			this.m_STATIC_SENDED_MESSAGES_PERSEC.Size = new System.Drawing.Size(88, 20);
			this.m_STATIC_SENDED_MESSAGES_PERSEC.TabIndex = 23;
			this.m_STATIC_SENDED_MESSAGES_PERSEC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_RECEIVED_MESSAGES
			// 
			this.m_STATIC_RECEIVED_MESSAGES.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_STATIC_RECEIVED_MESSAGES.Location = new System.Drawing.Point(136, 191);
			this.m_STATIC_RECEIVED_MESSAGES.Name = "m_STATIC_RECEIVED_MESSAGES";
			this.m_STATIC_RECEIVED_MESSAGES.Size = new System.Drawing.Size(88, 20);
			this.m_STATIC_RECEIVED_MESSAGES.TabIndex = 24;
			this.m_STATIC_RECEIVED_MESSAGES.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_RECEIVED_MESSAGES_PERSEC
			// 
			this.m_STATIC_RECEIVED_MESSAGES_PERSEC.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_STATIC_RECEIVED_MESSAGES_PERSEC.Location = new System.Drawing.Point(226, 191);
			this.m_STATIC_RECEIVED_MESSAGES_PERSEC.Name = "m_STATIC_RECEIVED_MESSAGES_PERSEC";
			this.m_STATIC_RECEIVED_MESSAGES_PERSEC.Size = new System.Drawing.Size(88, 20);
			this.m_STATIC_RECEIVED_MESSAGES_PERSEC.TabIndex = 25;
			this.m_STATIC_RECEIVED_MESSAGES_PERSEC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_SENDED_BYTES_PERSEC
			// 
			this.m_STATIC_SENDED_BYTES_PERSEC.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_STATIC_SENDED_BYTES_PERSEC.Location = new System.Drawing.Point(434, 169);
			this.m_STATIC_SENDED_BYTES_PERSEC.Name = "m_STATIC_SENDED_BYTES_PERSEC";
			this.m_STATIC_SENDED_BYTES_PERSEC.Size = new System.Drawing.Size(86, 20);
			this.m_STATIC_SENDED_BYTES_PERSEC.TabIndex = 26;
			this.m_STATIC_SENDED_BYTES_PERSEC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_RECEIVED_BYTES_PERSEC
			// 
			this.m_STATIC_RECEIVED_BYTES_PERSEC.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_STATIC_RECEIVED_BYTES_PERSEC.Location = new System.Drawing.Point(434, 191);
			this.m_STATIC_RECEIVED_BYTES_PERSEC.Name = "m_STATIC_RECEIVED_BYTES_PERSEC";
			this.m_STATIC_RECEIVED_BYTES_PERSEC.Size = new System.Drawing.Size(86, 20);
			this.m_STATIC_RECEIVED_BYTES_PERSEC.TabIndex = 27;
			this.m_STATIC_RECEIVED_BYTES_PERSEC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_MESSAGES_PERSEC_TOTAL
			// 
			this.m_STATIC_MESSAGES_PERSEC_TOTAL.Location = new System.Drawing.Point(219, 216);
			this.m_STATIC_MESSAGES_PERSEC_TOTAL.Name = "m_STATIC_MESSAGES_PERSEC_TOTAL";
			this.m_STATIC_MESSAGES_PERSEC_TOTAL.Size = new System.Drawing.Size(93, 22);
			this.m_STATIC_MESSAGES_PERSEC_TOTAL.TabIndex = 28;
			this.m_STATIC_MESSAGES_PERSEC_TOTAL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_BYTES_PERSEC_TOTAL
			// 
			this.m_STATIC_BYTES_PERSEC_TOTAL.Location = new System.Drawing.Point(422, 216);
			this.m_STATIC_BYTES_PERSEC_TOTAL.Name = "m_STATIC_BYTES_PERSEC_TOTAL";
			this.m_STATIC_BYTES_PERSEC_TOTAL.Size = new System.Drawing.Size(96, 22);
			this.m_STATIC_BYTES_PERSEC_TOTAL.TabIndex = 29;
			this.m_STATIC_BYTES_PERSEC_TOTAL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_TEXT_seperator_statistics
			// 
			this.m_TEXT_seperator_statistics.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_TEXT_seperator_statistics.Location = new System.Drawing.Point(328, 155);
			this.m_TEXT_seperator_statistics.Name = "m_TEXT_seperator_statistics";
			this.m_TEXT_seperator_statistics.Size = new System.Drawing.Size(1, 65);
			this.m_TEXT_seperator_statistics.TabIndex = 30;
			// 
			// m_TEXT_total_messages
			// 
			this.m_TEXT_total_messages.AutoSize = true;
			this.m_TEXT_total_messages.Location = new System.Drawing.Point(191, 221);
			this.m_TEXT_total_messages.Name = "m_TEXT_total_messages";
			this.m_TEXT_total_messages.Size = new System.Drawing.Size(31, 13);
			this.m_TEXT_total_messages.TabIndex = 31;
			this.m_TEXT_total_messages.Text = "Total";
			// 
			// m_TEXT_total_bytes
			// 
			this.m_TEXT_total_bytes.AutoSize = true;
			this.m_TEXT_total_bytes.Location = new System.Drawing.Point(396, 221);
			this.m_TEXT_total_bytes.Name = "m_TEXT_total_bytes";
			this.m_TEXT_total_bytes.Size = new System.Drawing.Size(31, 13);
			this.m_TEXT_total_bytes.TabIndex = 32;
			this.m_TEXT_total_bytes.Text = "Total";
			// 
			// m_TEXT_sended_messages
			// 
			this.m_TEXT_sended_messages.AutoSize = true;
			this.m_TEXT_sended_messages.Location = new System.Drawing.Point(29, 173);
			this.m_TEXT_sended_messages.Name = "m_TEXT_sended_messages";
			this.m_TEXT_sended_messages.Size = new System.Drawing.Size(93, 13);
			this.m_TEXT_sended_messages.TabIndex = 33;
			this.m_TEXT_sended_messages.Text = "Sended Messages";
			// 
			// m_TEXT_received_messages
			// 
			this.m_TEXT_received_messages.AutoSize = true;
			this.m_TEXT_received_messages.Location = new System.Drawing.Point(29, 194);
			this.m_TEXT_received_messages.Name = "m_TEXT_received_messages";
			this.m_TEXT_received_messages.Size = new System.Drawing.Size(101, 13);
			this.m_TEXT_received_messages.TabIndex = 34;
			this.m_TEXT_received_messages.Text = "Received Messages";
			// 
			// m_TEXT_sended_bytes
			// 
			this.m_TEXT_sended_bytes.AutoSize = true;
			this.m_TEXT_sended_bytes.Location = new System.Drawing.Point(342, 173);
			this.m_TEXT_sended_bytes.Name = "m_TEXT_sended_bytes";
			this.m_TEXT_sended_bytes.Size = new System.Drawing.Size(73, 13);
			this.m_TEXT_sended_bytes.TabIndex = 35;
			this.m_TEXT_sended_bytes.Text = "Sended Bytes";
			// 
			// m_TEXT_ReceivedBytes
			// 
			this.m_TEXT_ReceivedBytes.AutoSize = true;
			this.m_TEXT_ReceivedBytes.Location = new System.Drawing.Point(342, 194);
			this.m_TEXT_ReceivedBytes.Name = "m_TEXT_ReceivedBytes";
			this.m_TEXT_ReceivedBytes.Size = new System.Drawing.Size(81, 13);
			this.m_TEXT_ReceivedBytes.TabIndex = 36;
			this.m_TEXT_ReceivedBytes.Text = "Received Bytes";
			// 
			// m_TEXT_messages_accumulated
			// 
			this.m_TEXT_messages_accumulated.AutoSize = true;
			this.m_TEXT_messages_accumulated.Location = new System.Drawing.Point(148, 154);
			this.m_TEXT_messages_accumulated.Name = "m_TEXT_messages_accumulated";
			this.m_TEXT_messages_accumulated.Size = new System.Drawing.Size(68, 13);
			this.m_TEXT_messages_accumulated.TabIndex = 37;
			this.m_TEXT_messages_accumulated.Text = "Accumulated";
			// 
			// m_TEXT_messages_per_sec
			// 
			this.m_TEXT_messages_per_sec.AutoSize = true;
			this.m_TEXT_messages_per_sec.Location = new System.Drawing.Point(251, 154);
			this.m_TEXT_messages_per_sec.Name = "m_TEXT_messages_per_sec";
			this.m_TEXT_messages_per_sec.Size = new System.Drawing.Size(43, 13);
			this.m_TEXT_messages_per_sec.TabIndex = 38;
			this.m_TEXT_messages_per_sec.Text = "Per Sec";
			// 
			// m_TEXT_bytes_per_sec
			// 
			this.m_TEXT_bytes_per_sec.AutoSize = true;
			this.m_TEXT_bytes_per_sec.Location = new System.Drawing.Point(459, 154);
			this.m_TEXT_bytes_per_sec.Name = "m_TEXT_bytes_per_sec";
			this.m_TEXT_bytes_per_sec.Size = new System.Drawing.Size(43, 13);
			this.m_TEXT_bytes_per_sec.TabIndex = 39;
			this.m_TEXT_bytes_per_sec.Text = "Per Sec";
			// 
			// m_TEXT_seperator_total_message
			// 
			this.m_TEXT_seperator_total_message.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_TEXT_seperator_total_message.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.m_TEXT_seperator_total_message.Location = new System.Drawing.Point(131, 214);
			this.m_TEXT_seperator_total_message.Name = "m_TEXT_seperator_total_message";
			this.m_TEXT_seperator_total_message.Size = new System.Drawing.Size(186, 1);
			this.m_TEXT_seperator_total_message.TabIndex = 40;
			// 
			// m_TEXT_seperator_total_bytes
			// 
			this.m_TEXT_seperator_total_bytes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_TEXT_seperator_total_bytes.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.m_TEXT_seperator_total_bytes.Location = new System.Drawing.Point(430, 214);
			this.m_TEXT_seperator_total_bytes.Name = "m_TEXT_seperator_total_bytes";
			this.m_TEXT_seperator_total_bytes.Size = new System.Drawing.Size(92, 1);
			this.m_TEXT_seperator_total_bytes.TabIndex = 41;
			// 
			// m_TEXT_seperator_bottom
			// 
			this.m_TEXT_seperator_bottom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_TEXT_seperator_bottom.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.m_TEXT_seperator_bottom.Location = new System.Drawing.Point(3, 263);
			this.m_TEXT_seperator_bottom.Name = "m_TEXT_seperator_bottom";
			this.m_TEXT_seperator_bottom.Size = new System.Drawing.Size(550, 1);
			this.m_TEXT_seperator_bottom.TabIndex = 43;
			// 
			// SampleServerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(555, 290);
			this.Controls.Add(this.m_TEXT_seperator_bottom);
			this.Controls.Add(this.m_TEXT_seperator_total_bytes);
			this.Controls.Add(this.m_TEXT_seperator_total_message);
			this.Controls.Add(this.m_TEXT_bytes_per_sec);
			this.Controls.Add(this.m_TEXT_messages_per_sec);
			this.Controls.Add(this.m_TEXT_messages_accumulated);
			this.Controls.Add(this.m_TEXT_ReceivedBytes);
			this.Controls.Add(this.m_TEXT_sended_bytes);
			this.Controls.Add(this.m_TEXT_received_messages);
			this.Controls.Add(this.m_TEXT_sended_messages);
			this.Controls.Add(this.m_TEXT_total_bytes);
			this.Controls.Add(this.m_TEXT_total_messages);
			this.Controls.Add(this.m_TEXT_seperator_statistics);
			this.Controls.Add(this.m_STATIC_BYTES_PERSEC_TOTAL);
			this.Controls.Add(this.m_STATIC_MESSAGES_PERSEC_TOTAL);
			this.Controls.Add(this.m_STATIC_RECEIVED_BYTES_PERSEC);
			this.Controls.Add(this.m_STATIC_SENDED_BYTES_PERSEC);
			this.Controls.Add(this.m_STATIC_RECEIVED_MESSAGES_PERSEC);
			this.Controls.Add(this.m_STATIC_RECEIVED_MESSAGES);
			this.Controls.Add(this.m_STATIC_SENDED_MESSAGES_PERSEC);
			this.Controls.Add(this.m_STATIC_SENDED_MESSAGES);
			this.Controls.Add(this.m_BUTTON_DISCONNECT_ALL);
			this.Controls.Add(this.m_TEXT_seperator_2);
			this.Controls.Add(this.m_TEXT_seperator_1);
			this.Controls.Add(this.m_STATIC_ERROR_CLOSE);
			this.Controls.Add(this.m_STATIC_FAIL_CONNECT);
			this.Controls.Add(this.m_STATIC_DISCONNECTED);
			this.Controls.Add(this.m_STATIC_CONNECTED);
			this.Controls.Add(this.m_STATIC_TRY);
			this.Controls.Add(this.m_STATIC_KEEP_CONNECT);
			this.Controls.Add(this.m_TEXT_bottom);
			this.Controls.Add(this.m_TEXT_error_close);
			this.Controls.Add(this.m_TEXT_fail_to_connect);
			this.Controls.Add(this.m_TEXT_disconnected);
			this.Controls.Add(this.m_TEXT_connected);
			this.Controls.Add(this.m_TEXT_try);
			this.Controls.Add(this.m_TEXT_connection);
			this.Controls.Add(this.m_EDIT_BIND_PORT);
			this.Controls.Add(this.m_STATIC_ADDRESS);
			this.Controls.Add(this.m_TEXT_bind_port);
			this.Controls.Add(this.m_GROUP_acceptor);
			this.Font = new System.Drawing.Font("Tahoma", 8F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "SampleServerForm";
			this.Text = "CGCII Network Sample TCP Echo Server for C#";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SampleServerForm_FormClosing);
			this.Load += new System.EventHandler(this.SampleServerForm_Load);
			this.m_GROUP_acceptor.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox m_EDIT_BIND_PORT;
		private System.Windows.Forms.Label m_STATIC_ADDRESS;
		private System.Windows.Forms.Label m_TEXT_bind_port;
		private System.Windows.Forms.GroupBox m_GROUP_acceptor;
		private System.Windows.Forms.Label m_TEXT_connection;
		private System.Windows.Forms.Label m_TEXT_try;
		private System.Windows.Forms.Label m_TEXT_connected;
		private System.Windows.Forms.Label m_TEXT_disconnected;
		private System.Windows.Forms.Label m_TEXT_fail_to_connect;
		private System.Windows.Forms.Label m_TEXT_error_close;
		private System.Windows.Forms.Label m_TEXT_bottom;
		private System.Windows.Forms.Label m_STATIC_KEEP_CONNECT;
		private System.Windows.Forms.Label m_STATIC_TRY;
		private System.Windows.Forms.Label m_STATIC_CONNECTED;
		private System.Windows.Forms.Label m_STATIC_DISCONNECTED;
		private System.Windows.Forms.Label m_STATIC_FAIL_CONNECT;
		private System.Windows.Forms.Label m_STATIC_ERROR_CLOSE;
		private System.Windows.Forms.Label m_TEXT_seperator_1;
		private System.Windows.Forms.Label m_TEXT_seperator_2;
		private System.Windows.Forms.Button m_BUTTON_DISCONNECT_ALL;
		private System.Windows.Forms.Label m_STATIC_SENDED_MESSAGES;
		private System.Windows.Forms.Label m_STATIC_SENDED_MESSAGES_PERSEC;
		private System.Windows.Forms.Label m_STATIC_RECEIVED_MESSAGES;
		private System.Windows.Forms.Label m_STATIC_RECEIVED_MESSAGES_PERSEC;
		private System.Windows.Forms.Label m_STATIC_SENDED_BYTES_PERSEC;
		private System.Windows.Forms.Label m_STATIC_RECEIVED_BYTES_PERSEC;
		private System.Windows.Forms.Label m_STATIC_MESSAGES_PERSEC_TOTAL;
		private System.Windows.Forms.Label m_STATIC_BYTES_PERSEC_TOTAL;
		private System.Windows.Forms.Label m_TEXT_seperator_statistics;
		private System.Windows.Forms.Label m_TEXT_total_messages;
		private System.Windows.Forms.Label m_TEXT_total_bytes;
		private System.Windows.Forms.Label m_TEXT_sended_messages;
		private System.Windows.Forms.Label m_TEXT_received_messages;
		private System.Windows.Forms.Label m_TEXT_sended_bytes;
		private System.Windows.Forms.Label m_TEXT_ReceivedBytes;
		private System.Windows.Forms.Label m_TEXT_messages_accumulated;
		private System.Windows.Forms.Label m_TEXT_messages_per_sec;
		private System.Windows.Forms.Label m_TEXT_bytes_per_sec;
		private System.Windows.Forms.Label m_TEXT_seperator_total_message;
		private System.Windows.Forms.Label m_TEXT_seperator_total_bytes;
		private System.Windows.Forms.Label m_TEXT_seperator_bottom;
		private System.Windows.Forms.CheckBox m_CHECK_start_accept;
	}
}

