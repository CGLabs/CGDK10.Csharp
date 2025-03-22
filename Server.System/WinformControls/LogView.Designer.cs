using System.ComponentModel;
using System.Windows.Forms;

using System.Drawing;

namespace CGDK.Server.System.WinformControls
{
	partial class LogView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(LogView));
			this.button1 = new();
			this.label_TOTAL = new();
			this.label_SYSTEM = new();
			this.label_PROGRESS = new();
			this.label_INFO = new();
			this.label_EXCEPTION = new();
			this.label_ERROR = new();
			this.label_DEBUG = new();
			this.label_ETC = new();
			this.lableLogTotal = new();
			this.lableLogSystem = new();
			this.lableLogProgress = new();
			this.lableLogInfo = new();
			this.lableLogException = new();
			this.lableLogError = new();
			this.lableLogDebug = new();
			this.lableLogEtc = new();
			this.label17 = new();
			this.labelFilter = new();
			this.buttonEditFilter = new();
			this.buttonLogClose = new();
			this.buttonLogSave = new();
			this.buttonLogSaveSelected = new();
			this.label19 = new();
			this.lableLogCount = new();
			this.listBoxLogView = new();
			this.tabLog = new();
			this.tabPage1 = new();
			this.tabPage2 = new();
			this.label21 = new();
			this.label22 = new();
			this.label23 = new();
			this.label24 = new();
			this.label25 = new();
			this.tabLog.SuspendLayout();
			this.SuspendLayout();

			// - create font
			var tempFont = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

			// 
			// button1
			// 
			this.button1.AutoSizeMode	 = AutoSizeMode.GrowAndShrink;
			this.button1.Font			 = tempFont;
			this.button1.Location		 = new(2, 0);
			this.button1.Margin			 = new(0);
			this.button1.Name			 = "button1";
			this.button1.Size			 = new(37, 28);
			this.button1.TabIndex		 = 0;
			this.button1.Text			 = "New";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// label_TOTAL
			// 
			this.label_TOTAL.Font		 = tempFont;
			this.label_TOTAL.Location	 = new(48, 0);
			this.label_TOTAL.Name		 = "label_TOTAL";
			this.label_TOTAL.Size		 = new(60, 15);
			this.label_TOTAL.TabIndex	 = 1;
			this.label_TOTAL.Text		 = "Total";
			this.label_TOTAL.TextAlign	 = ContentAlignment.MiddleCenter;
			// 
			// label_SYSTEM
			// 
			this.label_SYSTEM.Font		 = tempFont;
			this.label_SYSTEM.Location	 = new(118, 0);
			this.label_SYSTEM.Name		 = "label_SYSTEM";
			this.label_SYSTEM.Size		 = new(60, 15);
			this.label_SYSTEM.TabIndex	 = 2;
			this.label_SYSTEM.Text		 = "System";
			this.label_SYSTEM.TextAlign  = ContentAlignment.MiddleCenter;
			// 
			// label_PROGRESS
			// 
			this.label_PROGRESS.Font	  = tempFont;
			this.label_PROGRESS.Location  = new(181, 0);
			this.label_PROGRESS.Name	  = "label_PROGRESS";
			this.label_PROGRESS.Size	  = new(60, 15);
			this.label_PROGRESS.TabIndex  = 3;
			this.label_PROGRESS.Text	  = "Progress";
			this.label_PROGRESS.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// label_INFO
			// 
			this.label_INFO.Font		  = tempFont;
			this.label_INFO.Location	  = new(244, 0);
			this.label_INFO.Name		  = "label_INFO";
			this.label_INFO.Size		  = new(60, 15);
			this.label_INFO.TabIndex	  = 4;
			this.label_INFO.Text		  = "Info";
			this.label_INFO.TextAlign	  = ContentAlignment.MiddleCenter;
			// 
			// label_EXCEPTION
			// 
			this.label_EXCEPTION.Font	  = tempFont;
			this.label_EXCEPTION.Location = new(314, 0);
			this.label_EXCEPTION.Name	  = "label_EXCEPTION";
			this.label_EXCEPTION.Size	  = new(60, 15);
			this.label_EXCEPTION.TabIndex = 5;
			this.label_EXCEPTION.Text	  = "Exception";
			this.label_EXCEPTION.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// label_ERROR
			// 
			this.label_ERROR.Font		  = tempFont;
			this.label_ERROR.Location	  = new(377, 0);
			this.label_ERROR.Name		  = "label_ERROR";
			this.label_ERROR.Size		  = new(60, 15);
			this.label_ERROR.TabIndex	  = 6;
			this.label_ERROR.Text		  = "Error";
			this.label_ERROR.TextAlign	  = ContentAlignment.MiddleCenter;
			// 
			// label_DEBUG
			// 
			this.label_DEBUG.Font		  = tempFont;
			this.label_DEBUG.Location	  = new(446, 0);
			this.label_DEBUG.Name		  = "label_DEBUG";
			this.label_DEBUG.Size		  = new(60, 15);
			this.label_DEBUG.TabIndex	  = 7;
			this.label_DEBUG.Text		  = "Debug";
			this.label_DEBUG.TextAlign	  = ContentAlignment.MiddleCenter;
			// 
			// label_ETC
			// 
			this.label_ETC.Font			  = tempFont;
			this.label_ETC.Location		  = new(516, 0);
			this.label_ETC.Name			  = "label_ETC";
			this.label_ETC.Size			  = new(60, 15);
			this.label_ETC.TabIndex		  = 8;
			this.label_ETC.Text			  = "Etc";
			this.label_ETC.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// lableLogTotal
			// 
			this.lableLogTotal.Font		  = tempFont;
			this.lableLogTotal.Location	  = new(48, 16);
			this.lableLogTotal.Name		  = "lableLogTotal";
			this.lableLogTotal.Size		  = new(60, 16);
			this.lableLogTotal.TabIndex	  = 9;
			this.lableLogTotal.TextAlign  = ContentAlignment.MiddleCenter;
			// 
			// lableLogSystem
			// 
			this.lableLogSystem.Font	  = tempFont;
			this.lableLogSystem.Location  = new(118, 16);
			this.lableLogSystem.Name	  = "lableLogSystem";
			this.lableLogSystem.Size	  = new(60, 16);
			this.lableLogSystem.TabIndex  = 10;
			this.lableLogSystem.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// lableLogProgress
			// 
			this.lableLogProgress.Font		 = tempFont;
			this.lableLogProgress.Location	 = new(181, 16);
			this.lableLogProgress.Name		 = "lableLogProgress";
			this.lableLogProgress.Size		 = new(60, 16);
			this.lableLogProgress.TabIndex	 = 11;
			this.lableLogProgress.TextAlign	 = ContentAlignment.MiddleCenter;
			// 
			// lableLogInfo
			// 
			this.lableLogInfo.Font			 = tempFont;
			this.lableLogInfo.Location		 = new(244, 16);
			this.lableLogInfo.Name			 = "lableLogInfo";
			this.lableLogInfo.Size			 = new(60, 16);
			this.lableLogInfo.TabIndex		 = 12;
			this.lableLogInfo.TextAlign		 = ContentAlignment.MiddleCenter;
			// 
			// lableLogException
			// 
			this.lableLogException.Font		 = tempFont;
			this.lableLogException.Location	 = new(314, 16);
			this.lableLogException.Name		 = "lableLogException";
			this.lableLogException.Size		 = new(60, 16);
			this.lableLogException.TabIndex	 = 13;
			this.lableLogException.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// lableLogError
			// 
			this.lableLogError.Font			 = tempFont;
			this.lableLogError.Location		 = new(377, 16);
			this.lableLogError.Name			 = "lableLogError";
			this.lableLogError.Size			 = new(60, 16);
			this.lableLogError.TabIndex		 = 14;
			this.lableLogError.TextAlign	 = ContentAlignment.MiddleCenter;
			// 
			// lableLogDebug
			// 
			this.lableLogDebug.Font			 = tempFont;
			this.lableLogDebug.Location		 = new(446, 16);
			this.lableLogDebug.Name			 = "lableLogDebug";
			this.lableLogDebug.Size			 = new(60, 16);
			this.lableLogDebug.TabIndex		 = 15;
			this.lableLogDebug.TextAlign	 = ContentAlignment.MiddleCenter;
			// 
			// lableLogEtc
			// 
			this.lableLogEtc.Font			 = tempFont;
			this.lableLogEtc.Location		 = new(516, 16);
			this.lableLogEtc.Name			 = "lableLogEtc";
			this.lableLogEtc.Size			 = new(60, 16);
			this.lableLogEtc.TabIndex		 = 16;
			this.lableLogEtc.TextAlign		 = ContentAlignment.MiddleCenter;
			// 
			// label17
			// 
			this.label17.AutoSize			 = true;
			this.label17.Font				 = tempFont;
			this.label17.Location			 = new(7, 60);
			this.label17.Name				 = "label17";
			this.label17.Size				 = new(29, 13);
			this.label17.TabIndex			 = 17;
			this.label17.Text				 = "Filter";
			// 
			// labelFilter
			// 
			this.labelFilter.BorderStyle	 = BorderStyle.Fixed3D;
			this.labelFilter.Font			 = tempFont;
			this.labelFilter.Location		 = new(40, 57);
			this.labelFilter.Name			 = "labelFilter";
			this.labelFilter.Size			 = new(161, 19);
			this.labelFilter.TabIndex		 = 18;
			this.labelFilter.TextAlign		 = ContentAlignment.MiddleLeft;
			// 
			// buttonEditFilter
			// 
			this.buttonEditFilter.Font		 = tempFont;
			this.buttonEditFilter.Location	 = new(201, 57);
			this.buttonEditFilter.Margin	 = new(0);
			this.buttonEditFilter.Name		 = "buttonEditFilter";
			this.buttonEditFilter.Size		 = new(22, 19);
			this.buttonEditFilter.TabIndex	 = 19;
			this.buttonEditFilter.Text		 = "...";
			this.buttonEditFilter.UseVisualStyleBackColor = true;
			// 
			// buttonLogClose
			// 
			this.buttonLogClose.Font		 = tempFont;
			this.buttonLogClose.Location	 = new(231, 57);
			this.buttonLogClose.Margin		 = new(0);
			this.buttonLogClose.Name		 = "buttonLogClose";
			this.buttonLogClose.Size		 = new(42, 19);
			this.buttonLogClose.TabIndex	 = 20;
			this.buttonLogClose.Text		 = "Close";
			this.buttonLogClose.UseVisualStyleBackColor = true;
			// 
			// buttonLogSave
			// 
			this.buttonLogSave.Font			 = tempFont;
			this.buttonLogSave.Location		 = new(275, 57);
			this.buttonLogSave.Margin		 = new(0);
			this.buttonLogSave.Name			 = "buttonLogSave";
			this.buttonLogSave.Size			 = new(41, 19);
			this.buttonLogSave.TabIndex		 = 21;
			this.buttonLogSave.Text			 = "Save";
			this.buttonLogSave.UseVisualStyleBackColor = true;
			// 
			// buttonLogSaveSelected
			// 
			this.buttonLogSaveSelected.Location	 = new(316, 57);
			this.buttonLogSaveSelected.Margin	 = new(0);
			this.buttonLogSaveSelected.Name		 = "buttonLogSaveSelected";
			this.buttonLogSaveSelected.Size		 = new(86, 19);
			this.buttonLogSaveSelected.TabIndex	 = 22;
			this.buttonLogSaveSelected.Text		 = "Save Selected";
			this.buttonLogSaveSelected.UseVisualStyleBackColor = true;
			// 
			// label19
			// 
			this.label19.AutoSize	 = true;
			this.label19.Font		 = tempFont;
			this.label19.Location	 = new(462, 60);
			this.label19.Name		 = "label19";
			this.label19.Size		 = new(35, 13);
			this.label19.TabIndex	 = 23;
			this.label19.Text		 = "Count";
			// 
			// lableLogCount
			// 
			this.lableLogCount.BorderStyle	 = BorderStyle.Fixed3D;
			this.lableLogCount.Font			 = tempFont;
			this.lableLogCount.Location		 = new(502, 58);
			this.lableLogCount.Name			 = "lableLogCount";
			this.lableLogCount.Size			 = new(74, 18);
			this.lableLogCount.TabIndex		 = 24;
			this.lableLogCount.TextAlign	 = ContentAlignment.MiddleLeft;
			// 
			// listBoxLogView
			// 
			this.listBoxLogView.Anchor				 = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) 
														| AnchorStyles.Left) 
														| AnchorStyles.Right)));
			this.listBoxLogView.DrawMode			 = DrawMode.OwnerDrawFixed;
			this.listBoxLogView.Font				 = tempFont;
			this.listBoxLogView.FormattingEnabled	 = true;
			this.listBoxLogView.HorizontalScrollbar	 = true;
			this.listBoxLogView.ItemHeight			 = 15;
			this.listBoxLogView.Location			 = new(5, 79);
			this.listBoxLogView.MaxLogCount			 = 2048;
			this.listBoxLogView.Name				 = "listBoxLogView";
			this.listBoxLogView.Size				 = new(573, 199);
			this.listBoxLogView.TabIndex			 = 25;
			this.listBoxLogView.SelectedIndexChanged += new(this.ListBoxLogView_SelectedIndexChanged);
			// 
			// tabLog
			// 
			this.tabLog.Controls.Add(this.tabPage1);
			this.tabLog.Controls.Add(this.tabPage2);
			this.tabLog.Location		 = new(0, 31);
			this.tabLog.Name			 = "tabLog";
			this.tabLog.SelectedIndex	 = 0;
			this.tabLog.Size			 = new(582, 24);
			this.tabLog.TabIndex		 = 26;
			// 
			// tabPage1
			// 
			this.tabPage1.Location		 = new(4, 22);
			this.tabPage1.Name			 = "tabPage1";
			this.tabPage1.Padding		 = new(3);
			this.tabPage1.Size			 = new(574, 0);
			this.tabPage1.TabIndex		 = 0;
			this.tabPage1.Text			 = "tabPage1";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Location		 = new(4, 22);
			this.tabPage2.Name			 = "tabPage2";
			this.tabPage2.Padding		 = new(3);
			this.tabPage2.Size			 = new(574, 0);
			this.tabPage2.TabIndex		 = 1;
			this.tabPage2.Text			 = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// label21
			// 
			this.label21.BorderStyle	 = BorderStyle.Fixed3D;
			this.label21.Location		 = new(113, 2);
			this.label21.Name			 = "label21";
			this.label21.Size			 = new(2, 30);
			this.label21.TabIndex		 = 27;
			// 
			// label22
			// 
			this.label22.BorderStyle	 = BorderStyle.Fixed3D;
			this.label22.Location		 = new(308, -2);
			this.label22.Name			 = "label22";
			this.label22.Size			 = new(2, 30);
			this.label22.TabIndex		 = 28;
			// 
			// label23
			// 
			this.label23.BorderStyle	 = BorderStyle.Fixed3D;
			this.label23.Location		 = new(440, -5);
			this.label23.Name			 = "label23";
			this.label23.Size			 = new(2, 30);
			this.label23.TabIndex		 = 29;
			// 
			// label24
			// 
			this.label24.BorderStyle	 = BorderStyle.Fixed3D;
			this.label24.Location		 = new(510, 0);
			this.label24.Name			 = "label24";
			this.label24.Size			 = new(2, 30);
			this.label24.TabIndex		 = 30;
			// 
			// label25
			// 
			this.label25.BorderStyle	 = BorderStyle.Fixed3D;
			this.label25.Location		 = new(226, 60);
			this.label25.Name			 = "label25";
			this.label25.Size			 = new(2, 15);
			this.label25.TabIndex		 = 31;
			// 
			// LogView
			// 
			this.AutoScaleDimensions = new SizeF(6F, 13F);
			this.AutoScaleMode = AutoScaleMode.Font;
			this.Controls.Add(this.label25);
			this.Controls.Add(this.label24);
			this.Controls.Add(this.label23);
			this.Controls.Add(this.label22);
			this.Controls.Add(this.label21);
			this.Controls.Add(this.tabLog);
			this.Controls.Add(this.listBoxLogView);
			this.Controls.Add(this.lableLogCount);
			this.Controls.Add(this.label19);
			this.Controls.Add(this.buttonLogSaveSelected);
			this.Controls.Add(this.buttonLogSave);
			this.Controls.Add(this.buttonLogClose);
			this.Controls.Add(this.buttonEditFilter);
			this.Controls.Add(this.labelFilter);
			this.Controls.Add(this.label17);
			this.Controls.Add(this.lableLogEtc);
			this.Controls.Add(this.lableLogDebug);
			this.Controls.Add(this.lableLogError);
			this.Controls.Add(this.lableLogException);
			this.Controls.Add(this.lableLogInfo);
			this.Controls.Add(this.lableLogProgress);
			this.Controls.Add(this.lableLogSystem);
			this.Controls.Add(this.lableLogTotal);
			this.Controls.Add(this.label_ETC);
			this.Controls.Add(this.label_DEBUG);
			this.Controls.Add(this.label_ERROR);
			this.Controls.Add(this.label_EXCEPTION);
			this.Controls.Add(this.label_INFO);
			this.Controls.Add(this.label_PROGRESS);
			this.Controls.Add(this.label_SYSTEM);
			this.Controls.Add(this.label_TOTAL);
			this.Controls.Add(this.button1);
			this.Font = tempFont;
			this.Name = "LogView";
			this.Size = new(582, 296);
			this.Load += new(this.LogView_Load);
			this.tabLog.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

		private void SuspendLayout()
		{
			throw new NotImplementedException();
		}

		#endregion

		private Button button1;
		private Label label_TOTAL;
		private Label label_SYSTEM;
		private Label label_PROGRESS;
		private Label label_INFO;
		private Label label_EXCEPTION;
		private Label label_ERROR;
		private Label label_DEBUG;
		private Label label_ETC;
		private Label lableLogTotal;
		private Label lableLogSystem;
		private Label lableLogProgress;
		private Label lableLogInfo;
		private Label lableLogException;
		private Label lableLogError;
		private Label lableLogDebug;
		private Label lableLogEtc;
		private Label label17;
		private Label labelFilter;
		private Button buttonEditFilter;
		private Button buttonLogClose;
		private Button buttonLogSave;
		private Button buttonLogSaveSelected;
		private Label label19;
		private Label lableLogCount;
		private ListBoxLog listBoxLogView;
		private TabControl tabLog;
		private TabPage tabPage1;
		private TabPage tabPage2;
		private Label label21;
		private Label label22;
		private Label label23;
		private Label label24;
		private Label label25;
	}
}
