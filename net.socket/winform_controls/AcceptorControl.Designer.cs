namespace CGSocketWinformControls
{
    partial class CAcceptorControl
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
			this.m_comboBindAddress = new System.Windows.Forms.ComboBox();
			this.m_textBindPort = new System.Windows.Forms.TextBox();
			this.m_checkAcceptorStart = new System.Windows.Forms.CheckBox();
			this.m_buttonAcceptorStatus = new System.Windows.Forms.Button();
			this.m_buttonAcceptorCloseAll = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_comboBindAddress
			// 
			this.m_comboBindAddress.AllowDrop = true;
			this.m_comboBindAddress.DropDownHeight = 50;
			this.m_comboBindAddress.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_comboBindAddress.FormattingEnabled = true;
			this.m_comboBindAddress.IntegralHeight = false;
			this.m_comboBindAddress.Items.AddRange(new object[] {
            "localhost"});
			this.m_comboBindAddress.Location = new System.Drawing.Point(0, 0);
			this.m_comboBindAddress.Name = "m_comboBindAddress";
			this.m_comboBindAddress.Size = new System.Drawing.Size(147, 21);
			this.m_comboBindAddress.TabIndex = 0;
			this.m_comboBindAddress.SelectedIndexChanged += new System.EventHandler(this.BindAddress_SelectedIndexChanged);
			// 
			// m_textBindPort
			// 
			this.m_textBindPort.Location = new System.Drawing.Point(148, 0);
			this.m_textBindPort.MaxLength = 6;
			this.m_textBindPort.Name = "m_textBindPort";
			this.m_textBindPort.Size = new System.Drawing.Size(45, 20);
			this.m_textBindPort.TabIndex = 1;
			// 
			// m_checkAcceptorStart
			// 
			this.m_checkAcceptorStart.Appearance = System.Windows.Forms.Appearance.Button;
			this.m_checkAcceptorStart.Location = new System.Drawing.Point(195, -1);
			this.m_checkAcceptorStart.Name = "m_checkAcceptorStart";
			this.m_checkAcceptorStart.Size = new System.Drawing.Size(34, 24);
			this.m_checkAcceptorStart.TabIndex = 2;
			this.m_checkAcceptorStart.Text = "Start";
			this.m_checkAcceptorStart.UseVisualStyleBackColor = true;
			this.m_checkAcceptorStart.CheckedChanged += new System.EventHandler(this.AcceptorStart_CheckedChanged);
			// 
			// m_buttonAcceptorStatus
			// 
			this.m_buttonAcceptorStatus.Location = new System.Drawing.Point(233, -1);
			this.m_buttonAcceptorStatus.Name = "m_buttonAcceptorStatus";
			this.m_buttonAcceptorStatus.Size = new System.Drawing.Size(51, 24);
			this.m_buttonAcceptorStatus.TabIndex = 3;
			this.m_buttonAcceptorStatus.Text = "Status";
			this.m_buttonAcceptorStatus.UseVisualStyleBackColor = true;
			this.m_buttonAcceptorStatus.Click += new System.EventHandler(this.AcceptorStatus_Click);
			// 
			// m_buttonAcceptorCloseAll
			// 
			this.m_buttonAcceptorCloseAll.Location = new System.Drawing.Point(284, -1);
			this.m_buttonAcceptorCloseAll.Name = "m_buttonAcceptorCloseAll";
			this.m_buttonAcceptorCloseAll.Size = new System.Drawing.Size(64, 24);
			this.m_buttonAcceptorCloseAll.TabIndex = 4;
			this.m_buttonAcceptorCloseAll.Text = "Close All";
			this.m_buttonAcceptorCloseAll.UseVisualStyleBackColor = true;
			this.m_buttonAcceptorCloseAll.Click += new System.EventHandler(this.AcceptorCloseAll_Click);
			// 
			// label1
			// 
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label1.Enabled = false;
			this.label1.Location = new System.Drawing.Point(231, 2);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(2, 20);
			this.label1.TabIndex = 7;
			// 
			// CAcceptorControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.m_buttonAcceptorCloseAll);
			this.Controls.Add(this.m_buttonAcceptorStatus);
			this.Controls.Add(this.m_checkAcceptorStart);
			this.Controls.Add(this.m_textBindPort);
			this.Controls.Add(this.m_comboBindAddress);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.Name = "CAcceptorControl";
			this.Size = new System.Drawing.Size(351, 23);
			this.Load += new System.EventHandler(this.CAcceptorControl_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox m_comboBindAddress;
        private System.Windows.Forms.TextBox m_textBindPort;
        private System.Windows.Forms.CheckBox m_checkAcceptorStart;
        private System.Windows.Forms.Button m_buttonAcceptorStatus;
        private System.Windows.Forms.Button m_buttonAcceptorCloseAll;
        private System.Windows.Forms.Label label1;
    }
}
