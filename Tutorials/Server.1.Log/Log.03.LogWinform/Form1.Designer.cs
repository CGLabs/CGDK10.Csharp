﻿namespace tutorial.server.log._03.log_winform.NET
{
	partial class Form1
	{
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		/// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form 디자이너에서 생성한 코드

		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
		/// </summary>
		private void InitializeComponent()
		{
			this.logview_main = new();
			this.SuspendLayout();
			// 
			// logview_main
			// 
			this.logview_main.Filter = null;
			this.logview_main.Location = new System.Drawing.Point(4, 29);
			this.logview_main.LogCount = ((ulong)(0ul));
			this.logview_main.Name = "logview_main";
			this.logview_main.Size = new System.Drawing.Size(791, 416);
			this.logview_main.TabIndex = 0;
			this.logview_main.TabStop = false;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.logview_main);
			this.Name = "Form1";
			this.Text = "tutorial.server.log.winform_controls";
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);

		}

		#endregion

		private CGDK.Server.System.WinformControls.ListBoxLog logview_main;
	}
}

