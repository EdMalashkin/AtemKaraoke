namespace AtemKaraoke.WinForm
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtSong = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.chkUpload = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtSong
            // 
            this.txtSong.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSong.Location = new System.Drawing.Point(12, 12);
            this.txtSong.Multiline = true;
            this.txtSong.Name = "txtSong";
            this.txtSong.ReadOnly = true;
            this.txtSong.Size = new System.Drawing.Size(511, 665);
            this.txtSong.TabIndex = 0;
            this.txtSong.Text = resources.GetString("txtSong.Text");
            this.txtSong.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMousePressed);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(529, 49);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(156, 34);
            this.btnGenerate.TabIndex = 1;
            this.btnGenerate.Text = "Generate PNG files";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // chkUpload
            // 
            this.chkUpload.AutoSize = true;
            this.chkUpload.Location = new System.Drawing.Point(530, 22);
            this.chkUpload.Name = "chkUpload";
            this.chkUpload.Size = new System.Drawing.Size(133, 21);
            this.chkUpload.TabIndex = 2;
            this.chkUpload.Text = "Upload to ATEM";
            this.chkUpload.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 689);
            this.Controls.Add(this.chkUpload);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.txtSong);
            this.Name = "Form1";
            this.Text = "AtemKaraoke";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSong;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.CheckBox chkUpload;
    }
}

