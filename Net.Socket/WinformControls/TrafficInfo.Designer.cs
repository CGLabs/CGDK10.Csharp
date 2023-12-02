namespace CGSocketWinformControls
{
    partial class TrafficInfo
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.m_TEXT_Seperator_TotalMessage = new System.Windows.Forms.Label();
			this.m_TEXT_MessagesPerSec = new System.Windows.Forms.Label();
			this.m_TEXT_MessagesAccumulated = new System.Windows.Forms.Label();
			this.m_TEXT_ReceivedMessages = new System.Windows.Forms.Label();
			this.m_TEXT_SendedMessages = new System.Windows.Forms.Label();
			this.m_TEXT_TotalMessages = new System.Windows.Forms.Label();
			this.m_STATIC_MESSAGES_PERSEC_TOTAL = new System.Windows.Forms.Label();
			this.m_STATIC_RECEIVED_MESSAGES_PERSEC = new System.Windows.Forms.Label();
			this.m_STATIC_RECEIVED_MESSAGES = new System.Windows.Forms.Label();
			this.m_STATIC_SENDED_MESSAGES_PERSEC = new System.Windows.Forms.Label();
			this.m_STATIC_SENDED_MESSAGES = new System.Windows.Forms.Label();
			this.m_STATIC_SENDED_BYTES_PERSEC = new System.Windows.Forms.Label();
			this.m_STATIC_RECEIVED_BYTES_PERSEC = new System.Windows.Forms.Label();
			this.m_STATIC_BYTES_PERSEC_TOTAL = new System.Windows.Forms.Label();
			this.m_TEXT_Seperator_Statistics = new System.Windows.Forms.Label();
			this.m_TEXT_BytesPerSec = new System.Windows.Forms.Label();
			this.m_TEXT_Seperator_TotalBytes = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.m_pictureGraph = new System.Windows.Forms.PictureBox();
			this.m_trackBarScalre = new System.Windows.Forms.TrackBar();
			((System.ComponentModel.ISupportInitialize)(this.m_pictureGraph)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_trackBarScalre)).BeginInit();
			this.SuspendLayout();
			// 
			// m_TEXT_Seperator_TotalMessage
			// 
			this.m_TEXT_Seperator_TotalMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_TEXT_Seperator_TotalMessage.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.m_TEXT_Seperator_TotalMessage.Location = new System.Drawing.Point(57, 79);
			this.m_TEXT_Seperator_TotalMessage.Name = "m_TEXT_Seperator_TotalMessage";
			this.m_TEXT_Seperator_TotalMessage.Size = new System.Drawing.Size(196, 2);
			this.m_TEXT_Seperator_TotalMessage.TabIndex = 60;
			// 
			// m_TEXT_MessagesPerSec
			// 
			this.m_TEXT_MessagesPerSec.AutoSize = true;
			this.m_TEXT_MessagesPerSec.Location = new System.Drawing.Point(174, 14);
			this.m_TEXT_MessagesPerSec.Name = "m_TEXT_MessagesPerSec";
			this.m_TEXT_MessagesPerSec.Size = new System.Drawing.Size(79, 13);
			this.m_TEXT_MessagesPerSec.TabIndex = 58;
			this.m_TEXT_MessagesPerSec.Text = "Messages/Sec";
			// 
			// m_TEXT_MessagesAccumulated
			// 
			this.m_TEXT_MessagesAccumulated.AutoSize = true;
			this.m_TEXT_MessagesAccumulated.Location = new System.Drawing.Point(108, 14);
			this.m_TEXT_MessagesAccumulated.Name = "m_TEXT_MessagesAccumulated";
			this.m_TEXT_MessagesAccumulated.Size = new System.Drawing.Size(69, 13);
			this.m_TEXT_MessagesAccumulated.TabIndex = 57;
			this.m_TEXT_MessagesAccumulated.Text = "Accumulated";
			// 
			// m_TEXT_ReceivedMessages
			// 
			this.m_TEXT_ReceivedMessages.AutoSize = true;
			this.m_TEXT_ReceivedMessages.Location = new System.Drawing.Point(7, 57);
			this.m_TEXT_ReceivedMessages.Name = "m_TEXT_ReceivedMessages";
			this.m_TEXT_ReceivedMessages.Size = new System.Drawing.Size(53, 13);
			this.m_TEXT_ReceivedMessages.TabIndex = 54;
			this.m_TEXT_ReceivedMessages.Text = "Received";
			// 
			// m_TEXT_SendedMessages
			// 
			this.m_TEXT_SendedMessages.AutoSize = true;
			this.m_TEXT_SendedMessages.Location = new System.Drawing.Point(7, 35);
			this.m_TEXT_SendedMessages.Name = "m_TEXT_SendedMessages";
			this.m_TEXT_SendedMessages.Size = new System.Drawing.Size(44, 13);
			this.m_TEXT_SendedMessages.TabIndex = 53;
			this.m_TEXT_SendedMessages.Text = "Sended";
			// 
			// m_TEXT_TotalMessages
			// 
			this.m_TEXT_TotalMessages.AutoSize = true;
			this.m_TEXT_TotalMessages.Location = new System.Drawing.Point(134, 87);
			this.m_TEXT_TotalMessages.Name = "m_TEXT_TotalMessages";
			this.m_TEXT_TotalMessages.Size = new System.Drawing.Size(31, 13);
			this.m_TEXT_TotalMessages.TabIndex = 51;
			this.m_TEXT_TotalMessages.Text = "Total";
			// 
			// m_STATIC_MESSAGES_PERSEC_TOTAL
			// 
			this.m_STATIC_MESSAGES_PERSEC_TOTAL.Location = new System.Drawing.Point(161, 81);
			this.m_STATIC_MESSAGES_PERSEC_TOTAL.Name = "m_STATIC_MESSAGES_PERSEC_TOTAL";
			this.m_STATIC_MESSAGES_PERSEC_TOTAL.Size = new System.Drawing.Size(88, 24);
			this.m_STATIC_MESSAGES_PERSEC_TOTAL.TabIndex = 48;
			this.m_STATIC_MESSAGES_PERSEC_TOTAL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_RECEIVED_MESSAGES_PERSEC
			// 
			this.m_STATIC_RECEIVED_MESSAGES_PERSEC.Location = new System.Drawing.Point(176, 54);
			this.m_STATIC_RECEIVED_MESSAGES_PERSEC.Name = "m_STATIC_RECEIVED_MESSAGES_PERSEC";
			this.m_STATIC_RECEIVED_MESSAGES_PERSEC.Size = new System.Drawing.Size(75, 22);
			this.m_STATIC_RECEIVED_MESSAGES_PERSEC.TabIndex = 45;
			this.m_STATIC_RECEIVED_MESSAGES_PERSEC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_RECEIVED_MESSAGES
			// 
			this.m_STATIC_RECEIVED_MESSAGES.Location = new System.Drawing.Point(99, 54);
			this.m_STATIC_RECEIVED_MESSAGES.Name = "m_STATIC_RECEIVED_MESSAGES";
			this.m_STATIC_RECEIVED_MESSAGES.Size = new System.Drawing.Size(75, 22);
			this.m_STATIC_RECEIVED_MESSAGES.TabIndex = 44;
			this.m_STATIC_RECEIVED_MESSAGES.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_SENDED_MESSAGES_PERSEC
			// 
			this.m_STATIC_SENDED_MESSAGES_PERSEC.Location = new System.Drawing.Point(176, 30);
			this.m_STATIC_SENDED_MESSAGES_PERSEC.Name = "m_STATIC_SENDED_MESSAGES_PERSEC";
			this.m_STATIC_SENDED_MESSAGES_PERSEC.Size = new System.Drawing.Size(75, 22);
			this.m_STATIC_SENDED_MESSAGES_PERSEC.TabIndex = 43;
			this.m_STATIC_SENDED_MESSAGES_PERSEC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_SENDED_MESSAGES
			// 
			this.m_STATIC_SENDED_MESSAGES.Location = new System.Drawing.Point(99, 30);
			this.m_STATIC_SENDED_MESSAGES.Name = "m_STATIC_SENDED_MESSAGES";
			this.m_STATIC_SENDED_MESSAGES.Size = new System.Drawing.Size(75, 22);
			this.m_STATIC_SENDED_MESSAGES.TabIndex = 42;
			this.m_STATIC_SENDED_MESSAGES.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_SENDED_BYTES_PERSEC
			// 
			this.m_STATIC_SENDED_BYTES_PERSEC.Location = new System.Drawing.Point(297, 30);
			this.m_STATIC_SENDED_BYTES_PERSEC.Name = "m_STATIC_SENDED_BYTES_PERSEC";
			this.m_STATIC_SENDED_BYTES_PERSEC.Size = new System.Drawing.Size(74, 22);
			this.m_STATIC_SENDED_BYTES_PERSEC.TabIndex = 46;
			this.m_STATIC_SENDED_BYTES_PERSEC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_RECEIVED_BYTES_PERSEC
			// 
			this.m_STATIC_RECEIVED_BYTES_PERSEC.Location = new System.Drawing.Point(297, 54);
			this.m_STATIC_RECEIVED_BYTES_PERSEC.Name = "m_STATIC_RECEIVED_BYTES_PERSEC";
			this.m_STATIC_RECEIVED_BYTES_PERSEC.Size = new System.Drawing.Size(74, 22);
			this.m_STATIC_RECEIVED_BYTES_PERSEC.TabIndex = 47;
			this.m_STATIC_RECEIVED_BYTES_PERSEC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_STATIC_BYTES_PERSEC_TOTAL
			// 
			this.m_STATIC_BYTES_PERSEC_TOTAL.Location = new System.Drawing.Point(286, 81);
			this.m_STATIC_BYTES_PERSEC_TOTAL.Name = "m_STATIC_BYTES_PERSEC_TOTAL";
			this.m_STATIC_BYTES_PERSEC_TOTAL.Size = new System.Drawing.Size(82, 24);
			this.m_STATIC_BYTES_PERSEC_TOTAL.TabIndex = 49;
			this.m_STATIC_BYTES_PERSEC_TOTAL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_TEXT_Seperator_Statistics
			// 
			this.m_TEXT_Seperator_Statistics.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_TEXT_Seperator_Statistics.Location = new System.Drawing.Point(263, 15);
			this.m_TEXT_Seperator_Statistics.Name = "m_TEXT_Seperator_Statistics";
			this.m_TEXT_Seperator_Statistics.Size = new System.Drawing.Size(1, 70);
			this.m_TEXT_Seperator_Statistics.TabIndex = 50;
			// 
			// m_TEXT_BytesPerSec
			// 
			this.m_TEXT_BytesPerSec.AutoSize = true;
			this.m_TEXT_BytesPerSec.Location = new System.Drawing.Point(317, 14);
			this.m_TEXT_BytesPerSec.Name = "m_TEXT_BytesPerSec";
			this.m_TEXT_BytesPerSec.Size = new System.Drawing.Size(57, 13);
			this.m_TEXT_BytesPerSec.TabIndex = 59;
			this.m_TEXT_BytesPerSec.Text = "Bytes/Sec";
			// 
			// m_TEXT_Seperator_TotalBytes
			// 
			this.m_TEXT_Seperator_TotalBytes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_TEXT_Seperator_TotalBytes.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.m_TEXT_Seperator_TotalBytes.Location = new System.Drawing.Point(272, 79);
			this.m_TEXT_Seperator_TotalBytes.Name = "m_TEXT_Seperator_TotalBytes";
			this.m_TEXT_Seperator_TotalBytes.Size = new System.Drawing.Size(103, 1);
			this.m_TEXT_Seperator_TotalBytes.TabIndex = 61;
			// 
			// label1
			// 
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.label1.Location = new System.Drawing.Point(394, 80);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(63, 2);
			this.label1.TabIndex = 67;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(415, 15);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(45, 13);
			this.label2.TabIndex = 66;
			this.label2.Text = "Per Sec";
			// 
			// label3
			// 
			this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label3.Location = new System.Drawing.Point(385, 17);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(1, 70);
			this.label3.TabIndex = 65;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(394, 82);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(63, 24);
			this.label4.TabIndex = 64;
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(398, 55);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(60, 22);
			this.label5.TabIndex = 63;
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(398, 31);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(60, 22);
			this.label6.TabIndex = 62;
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_pictureGraph
			// 
			this.m_pictureGraph.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.m_pictureGraph.Location = new System.Drawing.Point(0, 113);
			this.m_pictureGraph.Name = "m_pictureGraph";
			this.m_pictureGraph.Size = new System.Drawing.Size(435, 112);
			this.m_pictureGraph.TabIndex = 68;
			this.m_pictureGraph.TabStop = false;
			// 
			// m_trackBarScalre
			// 
			this.m_trackBarScalre.Location = new System.Drawing.Point(440, 113);
			this.m_trackBarScalre.Name = "m_trackBarScalre";
			this.m_trackBarScalre.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.m_trackBarScalre.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.m_trackBarScalre.Size = new System.Drawing.Size(45, 112);
			this.m_trackBarScalre.TabIndex = 69;
			this.m_trackBarScalre.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			// 
			// TrafficInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.m_trackBarScalre);
			this.Controls.Add(this.m_pictureGraph);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.m_TEXT_Seperator_TotalBytes);
			this.Controls.Add(this.m_TEXT_Seperator_TotalMessage);
			this.Controls.Add(this.m_TEXT_BytesPerSec);
			this.Controls.Add(this.m_TEXT_MessagesPerSec);
			this.Controls.Add(this.m_TEXT_MessagesAccumulated);
			this.Controls.Add(this.m_TEXT_ReceivedMessages);
			this.Controls.Add(this.m_TEXT_SendedMessages);
			this.Controls.Add(this.m_TEXT_TotalMessages);
			this.Controls.Add(this.m_TEXT_Seperator_Statistics);
			this.Controls.Add(this.m_STATIC_BYTES_PERSEC_TOTAL);
			this.Controls.Add(this.m_STATIC_MESSAGES_PERSEC_TOTAL);
			this.Controls.Add(this.m_STATIC_RECEIVED_BYTES_PERSEC);
			this.Controls.Add(this.m_STATIC_SENDED_BYTES_PERSEC);
			this.Controls.Add(this.m_STATIC_RECEIVED_MESSAGES_PERSEC);
			this.Controls.Add(this.m_STATIC_RECEIVED_MESSAGES);
			this.Controls.Add(this.m_STATIC_SENDED_MESSAGES_PERSEC);
			this.Controls.Add(this.m_STATIC_SENDED_MESSAGES);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.Name = "TrafficInfo";
			this.Size = new System.Drawing.Size(471, 228);
			this.Load += new System.EventHandler(this.TrafficInfo_Load);
			((System.ComponentModel.ISupportInitialize)(this.m_pictureGraph)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_trackBarScalre)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

		#endregion
		private System.Windows.Forms.Label m_TEXT_Seperator_TotalMessage;
		private System.Windows.Forms.Label m_TEXT_MessagesPerSec;
		private System.Windows.Forms.Label m_TEXT_MessagesAccumulated;
		private System.Windows.Forms.Label m_TEXT_ReceivedMessages;
		private System.Windows.Forms.Label m_TEXT_SendedMessages;
		private System.Windows.Forms.Label m_TEXT_TotalMessages;
		private System.Windows.Forms.Label m_STATIC_MESSAGES_PERSEC_TOTAL;
		private System.Windows.Forms.Label m_STATIC_RECEIVED_MESSAGES_PERSEC;
		private System.Windows.Forms.Label m_STATIC_RECEIVED_MESSAGES;
		private System.Windows.Forms.Label m_STATIC_SENDED_MESSAGES_PERSEC;
		private System.Windows.Forms.Label m_STATIC_SENDED_MESSAGES;
		private System.Windows.Forms.Label m_STATIC_SENDED_BYTES_PERSEC;
		private System.Windows.Forms.Label m_STATIC_RECEIVED_BYTES_PERSEC;
		private System.Windows.Forms.Label m_STATIC_BYTES_PERSEC_TOTAL;
		private System.Windows.Forms.Label m_TEXT_Seperator_Statistics;
		private System.Windows.Forms.Label m_TEXT_BytesPerSec;
		private System.Windows.Forms.Label m_TEXT_Seperator_TotalBytes;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.PictureBox m_pictureGraph;
		private System.Windows.Forms.TrackBar m_trackBarScalre;
	}
}
