namespace AtemKaraoke.WinForm
{
    partial class FormMain
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			this.txtSong = new System.Windows.Forms.TextBox();
			this.chkEditMode = new System.Windows.Forms.CheckBox();
			this.grdSong = new System.Windows.Forms.DataGridView();
			this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnReconnect = new System.Windows.Forms.Button();
			this.timerKeepConnectionAlive = new System.Windows.Forms.Timer(this.components);
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.btnOnAir = new System.Windows.Forms.Button();
			this.pnlSong = new System.Windows.Forms.Panel();
			this.btnCancelPreview = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.grdSong)).BeginInit();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtSong
			// 
			this.txtSong.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtSong.BackColor = System.Drawing.SystemColors.Window;
			this.txtSong.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.txtSong.Location = new System.Drawing.Point(13, 38);
			this.txtSong.Multiline = true;
			this.txtSong.Name = "txtSong";
			this.txtSong.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtSong.Size = new System.Drawing.Size(471, 743);
			this.txtSong.TabIndex = 0;
			this.txtSong.Resize += new System.EventHandler(this.txtSong_Resized);
			// 
			// chkEditMode
			// 
			this.chkEditMode.Appearance = System.Windows.Forms.Appearance.Button;
			this.chkEditMode.AutoSize = true;
			this.chkEditMode.Checked = true;
			this.chkEditMode.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkEditMode.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.chkEditMode.Location = new System.Drawing.Point(12, 5);
			this.chkEditMode.Name = "chkEditMode";
			this.chkEditMode.Size = new System.Drawing.Size(122, 27);
			this.chkEditMode.TabIndex = 3;
			this.chkEditMode.Text = "Go to Live Mode";
			this.toolTip.SetToolTip(this.chkEditMode, "F5");
			this.chkEditMode.UseVisualStyleBackColor = true;
			this.chkEditMode.CheckedChanged += new System.EventHandler(this.chkEditMode_CheckedChanged);
			// 
			// grdSong
			// 
			this.grdSong.AllowUserToAddRows = false;
			this.grdSong.AllowUserToDeleteRows = false;
			this.grdSong.AllowUserToResizeColumns = false;
			this.grdSong.AllowUserToResizeRows = false;
			this.grdSong.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grdSong.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.grdSong.BackgroundColor = System.Drawing.SystemColors.Control;
			this.grdSong.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.grdSong.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			this.grdSong.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grdSong.ColumnHeadersVisible = false;
			this.grdSong.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(10);
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.grdSong.DefaultCellStyle = dataGridViewCellStyle1;
			this.grdSong.Location = new System.Drawing.Point(287, 73);
			this.grdSong.MultiSelect = false;
			this.grdSong.Name = "grdSong";
			this.grdSong.ReadOnly = true;
			this.grdSong.RowHeadersVisible = false;
			this.grdSong.RowTemplate.Height = 24;
			this.grdSong.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.grdSong.Size = new System.Drawing.Size(138, 549);
			this.grdSong.TabIndex = 0;
			this.grdSong.Visible = false;
			this.grdSong.SelectionChanged += new System.EventHandler(this.grdSong_SelectionChanged);
			// 
			// Column1
			// 
			this.Column1.DataPropertyName = "Text";
			this.Column1.HeaderText = "Text";
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// btnReconnect
			// 
			this.btnReconnect.Location = new System.Drawing.Point(12, 5);
			this.btnReconnect.Name = "btnReconnect";
			this.btnReconnect.Size = new System.Drawing.Size(90, 27);
			this.btnReconnect.TabIndex = 1;
			this.btnReconnect.Text = "Reconnect";
			this.btnReconnect.UseVisualStyleBackColor = true;
			this.btnReconnect.Visible = false;
			this.btnReconnect.Click += new System.EventHandler(this.btnReconnect_Click);
			// 
			// timerKeepConnectionAlive
			// 
			this.timerKeepConnectionAlive.Enabled = true;
			this.timerKeepConnectionAlive.Interval = 30000;
			this.timerKeepConnectionAlive.Tick += new System.EventHandler(this.timerKeepConnectionAlive_Tick);
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
			this.statusStrip1.Location = new System.Drawing.Point(0, 771);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(496, 22);
			this.statusStrip1.TabIndex = 4;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel
			// 
			this.toolStripStatusLabel.Name = "toolStripStatusLabel";
			this.toolStripStatusLabel.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
			this.toolStripStatusLabel.Size = new System.Drawing.Size(10, 17);
			// 
			// btnOnAir
			// 
			this.btnOnAir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOnAir.BackColor = System.Drawing.SystemColors.Control;
			this.btnOnAir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnOnAir.Location = new System.Drawing.Point(307, 5);
			this.btnOnAir.Name = "btnOnAir";
			this.btnOnAir.Size = new System.Drawing.Size(177, 27);
			this.btnOnAir.TabIndex = 5;
			this.btnOnAir.Text = "Preview";
			this.btnOnAir.UseVisualStyleBackColor = true;
			this.btnOnAir.Visible = false;
			this.btnOnAir.Click += new System.EventHandler(this.btnOnAir_Click);
			// 
			// pnlSong
			// 
			this.pnlSong.BackColor = System.Drawing.SystemColors.Control;
			this.pnlSong.ForeColor = System.Drawing.SystemColors.Control;
			this.pnlSong.Location = new System.Drawing.Point(81, 82);
			this.pnlSong.Name = "pnlSong";
			this.pnlSong.Padding = new System.Windows.Forms.Padding(1);
			this.pnlSong.Size = new System.Drawing.Size(200, 100);
			this.pnlSong.TabIndex = 6;
			this.pnlSong.Visible = false;
			// 
			// btnGoOffAir
			// 
			this.btnCancelPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancelPreview.Location = new System.Drawing.Point(175, 5);
			this.btnCancelPreview.Name = "btnGoOffAir";
			this.btnCancelPreview.Size = new System.Drawing.Size(126, 27);
			this.btnCancelPreview.TabIndex = 7;
			this.btnCancelPreview.Text = "Cancel Preview";
			this.btnCancelPreview.UseVisualStyleBackColor = true;
			this.btnCancelPreview.Visible = false;
			this.btnCancelPreview.Click += new System.EventHandler(this.btnOffAir_Click);
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(496, 793);
			this.Controls.Add(this.btnCancelPreview);
			this.Controls.Add(this.grdSong);
			this.Controls.Add(this.pnlSong);
			this.Controls.Add(this.chkEditMode);
			this.Controls.Add(this.btnOnAir);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.btnReconnect);
			this.Controls.Add(this.txtSong);
			this.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "AtemKaraoke";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_Closing);
			this.Load += new System.EventHandler(this.FormMain_Load);
			((System.ComponentModel.ISupportInitialize)(this.grdSong)).EndInit();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSong;
        private System.Windows.Forms.CheckBox chkEditMode;
        private System.Windows.Forms.DataGridView grdSong;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.Button btnReconnect;
        private System.Windows.Forms.Timer timerKeepConnectionAlive;
        private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
		private System.Windows.Forms.Button btnOnAir;
		private System.Windows.Forms.Panel pnlSong;
		private System.Windows.Forms.Button btnCancelPreview;
	}
}

