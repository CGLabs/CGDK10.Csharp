namespace CGSocketWinformControls
{
    partial class ConnectorControl
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
			this.m_textPeerPort = new System.Windows.Forms.TextBox();
			this.m_textPeerAddress = new System.Windows.Forms.TextBox();
			this.m_buttonConnect = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// m_textPeerPort
			// 
			this.m_textPeerPort.Location = new System.Drawing.Point(134, 0);
			this.m_textPeerPort.MaxLength = 6;
			this.m_textPeerPort.Name = "m_textPeerPort";
			this.m_textPeerPort.Size = new System.Drawing.Size(45, 20);
			this.m_textPeerPort.TabIndex = 2;
			// 
			// m_textPeerAddress
			// 
			this.m_textPeerAddress.Location = new System.Drawing.Point(-1, 0);
			this.m_textPeerAddress.MaxLength = 6;
			this.m_textPeerAddress.Name = "m_textPeerAddress";
			this.m_textPeerAddress.Size = new System.Drawing.Size(133, 20);
			this.m_textPeerAddress.TabIndex = 3;
			// 
			// m_buttonConnect
			// 
			this.m_buttonConnect.Location = new System.Drawing.Point(180, 0);
			this.m_buttonConnect.Name = "m_buttonConnect";
			this.m_buttonConnect.Size = new System.Drawing.Size(59, 23);
			this.m_buttonConnect.TabIndex = 4;
			this.m_buttonConnect.Text = "Connect";
			this.m_buttonConnect.UseVisualStyleBackColor = true;
			this.m_buttonConnect.Click += new System.EventHandler(this.ButtonConnect_Clicked);
			// 
			// ConnectorControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.m_buttonConnect);
			this.Controls.Add(this.m_textPeerAddress);
			this.Controls.Add(this.m_textPeerPort);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.Name = "ConnectorControl";
			this.Size = new System.Drawing.Size(248, 24);
			this.Load += new System.EventHandler(this.ConnectorControl_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

		#endregion

		private System.Windows.Forms.TextBox m_textPeerPort;
		private System.Windows.Forms.TextBox m_textPeerAddress;
		private System.Windows.Forms.Button m_buttonConnect;
	}
}
