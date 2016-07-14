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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.txtSong = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.chkEditMode = new System.Windows.Forms.CheckBox();
            this.grdSong = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnReconnect = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdSong)).BeginInit();
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
            this.txtSong.Size = new System.Drawing.Size(471, 743);
            this.txtSong.TabIndex = 0;
            this.txtSong.Resize += new System.EventHandler(this.txtSong_Resized);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // chkEditMode
            // 
            this.chkEditMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkEditMode.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkEditMode.AutoSize = true;
            this.chkEditMode.Checked = true;
            this.chkEditMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEditMode.Location = new System.Drawing.Point(417, 5);
            this.chkEditMode.Name = "chkEditMode";
            this.chkEditMode.Size = new System.Drawing.Size(67, 27);
            this.chkEditMode.TabIndex = 0;
            this.chkEditMode.Text = "Go Live";
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
            this.grdSong.Location = new System.Drawing.Point(254, 5);
            this.grdSong.MultiSelect = false;
            this.grdSong.Name = "grdSong";
            this.grdSong.ReadOnly = true;
            this.grdSong.RowHeadersVisible = false;
            this.grdSong.RowTemplate.Height = 24;
            this.grdSong.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdSong.Size = new System.Drawing.Size(138, 629);
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
            this.btnReconnect.Location = new System.Drawing.Point(13, 5);
            this.btnReconnect.Name = "btnReconnect";
            this.btnReconnect.Size = new System.Drawing.Size(90, 27);
            this.btnReconnect.TabIndex = 1;
            this.btnReconnect.Text = "Reconnect";
            this.btnReconnect.UseVisualStyleBackColor = true;
            this.btnReconnect.Visible = false;
            this.btnReconnect.Click += new System.EventHandler(this.btnReconnect_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 793);
            this.Controls.Add(this.btnReconnect);
            this.Controls.Add(this.grdSong);
            this.Controls.Add(this.chkEditMode);
            this.Controls.Add(this.txtSong);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "AtemKaraoke";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_Closing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdSong)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSong;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox chkEditMode;
        private System.Windows.Forms.DataGridView grdSong;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.Button btnReconnect;
    }
}

