using System;
using System.Windows.Forms;
using AtemKaraoke.Core;
using System.Diagnostics;
using System.Drawing;

namespace AtemKaraoke.WinForm
{
    public partial class FormMain : Form
	{
		bool _isRestart;

        public FormMain()
		{
			InitializeComponent();
			LoadLastSettings();
			chkEditMode.Checked = true;
            Init();
        }

		public FormMain(bool isRestart)
		{
			_isRestart = isRestart;
			InitializeComponent();
			LoadLastSettings();
			chkEditMode.Checked = false;
            Init();
        }

        private void Init()
        {
            toolTip.SetToolTip(chkEditMode, "Press F5");
            toolTip.SetToolTip(btnOnAir, "Press F6");
            toolTip.SetToolTip(btnCancelPreview, "Press F7");
            toolStripStatusLabel.Text = "Edit Mode";
            statusStrip1.Refresh();
        }

        Lyrics _lyrics;
        private Lyrics Lyrics
        {
            get
            {
                if (_lyrics == null)
                {
                    //_lyrics = new Lyrics(GetSelectedSongText);
                    CreateNewLyrics();
                }
                return _lyrics;
            }
        }

        private string GetSelectedSongText
		{
			get
			{
				if (txtSong.SelectionLength > 0)
					return txtSong.SelectedText;
				else
					return txtSong.Text;
			}
		}

        private DataGridViewCellStyle RefrainStyle()
        {
            var s = new DataGridViewCellStyle();
            s.BackColor = Color.Yellow;
            s.SelectionBackColor = Color.Yellow;
            s.SelectionForeColor = Color.Black;
            return s;
        }

        private void CreateNewLyrics()
        {
            _lyrics = new Lyrics(GetSelectedSongText);
            _lyrics.OnVerseSelected += new EventHandler(this.OnVerseSelected);
        }

        private void BindGrid()
        {
            grdSong.AutoGenerateColumns = false;
            grdSong.DataSource = Lyrics.VerseFiles;
            Lyrics.SelectFirstVerse(); ;
        }

        private void SetLiveMode()
        {
            grdSong.Enabled = false;
            Cursor = Cursors.WaitCursor;

            CreateNewLyrics();
            if (_isRestart == false && chkExport.Checked == true)
            {
                Upload();
            }
            BindGrid();

            ResizeSongControls();
            chkEditMode.Text = "Back To Edit Mode";
            toolTip.SetToolTip(chkEditMode, "Press Esc");
            btnOnAir.Text = "Preview";
            btnOnAir.Visible = true;
            chkExport.Checked = true; // keep it true for the next time
            toolStripStatusLabel.Text = "Off Air";
            statusStrip1.Refresh();
            grdSong.Focus();
            txtSong.Visible = false;
            chkExport.Visible = false;
            grdSong.Visible = true;
            pnlSong.Visible = true;

            Cursor = Cursors.Default;
            grdSong.Enabled = true;
            //btnReconnect.Visible = true; commented as images are not get generated after reconnecting for some reason
        }

        private void SetEditMode()
        {
            chkEditMode.Text = "Go To Live Mode";
            toolTip.SetToolTip(chkEditMode, "Press F5");
            btnReconnect.Visible = false;
            btnOnAir.Text = "Anything";
            pnlSong.BackColor = SystemColors.Control;
            btnOnAir.Visible = false;
            btnCancelPreview.Visible = false;
            Lyrics.Switcher.SetMediaOffAir();
            toolStripStatusLabel.Text = "Edit Mode";
            statusStrip1.Refresh();
            txtSong.Visible = true;
            chkExport.Visible = true;
            grdSong.Visible = false;
            pnlSong.Visible = false;
        }

        private void chkEditMode_CheckedChanged(object sender, EventArgs e)
		{
            if (!chkEditMode.Checked) SetLiveMode(); else SetEditMode();
		}

        private void SendViaConsole(string path, bool sendSelected = false)
        {
            var process = new Process();
            process.StartInfo = new ProcessStartInfo(@"AtemKaraoke.Console.exe");
            process.StartInfo.Arguments = string.Format("\"{0}\"", path);
            if (sendSelected)
                process.StartInfo.Arguments += " sendSelected";
            process.Start();
        }

        private void Upload()
        {
            try
            {
                Lyrics.Save();
                if (!Lyrics.Configuration.UseConsoleToUploadFromWinForm)
                {
                    Lyrics.Send();
                }
                else
                {   // this works today
                    string binaryFile = SaveLyrics();
                    SendViaConsole(binaryFile); // then the console is going to call Lyrics.Send()
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ATEM Error");
            }
        }

        private string SaveLyrics()
        {
            Lyrics.OnVerseSelected -= new EventHandler(this.OnVerseSelected); // to avoid {"Type 'System.Windows.Forms.Form' in Assembly 'System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' is not marked as serializable
            string binaryFile = new BinaryFileLyrics(Lyrics).Save();
            Lyrics.OnVerseSelected += new EventHandler(this.OnVerseSelected);
            return binaryFile;
        }
		private void btnOnAir_Click(object sender, EventArgs e)
		{
			try
			{
				grdSong.Enabled = false;
				Cursor = Cursors.WaitCursor;

				if (btnOnAir.Text == "Preview")
				{
					Lyrics.Switcher.SetMediaToPreview();
					pnlSong.BackColor = System.Drawing.Color.LightGreen;
					btnOnAir.Text = "On Air"; // declare the next action
					btnCancelPreview.Visible = true;
					toolStripStatusLabel.Text = "Preview";
					statusStrip1.Refresh();
				}
				else if (btnOnAir.Text == "On Air")
				{
                    Lyrics.Switcher.SetMediaOnAir();
					pnlSong.BackColor = System.Drawing.Color.Red;
					btnOnAir.Text = "Off Air"; // declare the next action
					btnCancelPreview.Visible = false;
					toolStripStatusLabel.Text = "On Air!";
					statusStrip1.Refresh();
				}
				else
				{
                    Lyrics.Switcher.SetMediaOffAir();
					pnlSong.BackColor = System.Drawing.SystemColors.Control;
					btnOnAir.Text = "Preview";
					btnCancelPreview.Visible = false;
					toolStripStatusLabel.Text = "Off Air";
					statusStrip1.Refresh();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "ATEM Error");
			}
			finally
			{
				grdSong.Enabled = true;
				Cursor = Cursors.Default;
				grdSong.Focus();
			}
		}

		private void btnOffAir_Click(object sender, EventArgs e)
		{
			btnOnAir.Text = "Cancel";
			btnOnAir_Click(null, null);
			btnCancelPreview.Visible = false;
		}

		public void OnVerseSelected(object sender, EventArgs e)
		{
            var curVerse = (VerseFile)sender;
            grdSong.Rows[curVerse.LyricsIndexBasedOnZero].Selected = true;
            Debug.Print("OnVerseSelected {0}", curVerse.LyricsIndexBasedOnZero);
        }

		private void grdSong_SelectionChanged(object sender, EventArgs e)
		{
            if (grdSong.CurrentRow != null && Lyrics.SelectedVerse != null)
            {
                var curVerseFile = grdSong.Rows[grdSong.CurrentRow.Index].DataBoundItem as VerseFile;
                if (curVerseFile.LyricsIndexBasedOnZero != Lyrics.SelectedVerse.LyricsIndexBasedOnZero)
                {
                    //attempt to cancel the autoselection with arrows when it contradicts with Lyrics.SelectedVerse
                    grdSong.Rows[Lyrics.SelectedVerse.LyricsIndexBasedOnZero].Selected = true;
                }
                //Debug.Print("grdSong_SelectionChanged {0}", curVerseFile.LyricsIndexBasedOnZero);
            }
        }

		private void txtSong_Resized(object sender, EventArgs e)
		{
			ResizeSongControls();
		}

		private void ResizeSongControls()
		{

			pnlSong.Left = txtSong.Left;
			pnlSong.Top = txtSong.Top;
			pnlSong.Width = txtSong.Width;
			pnlSong.Height = txtSong.Height;

			// red border 3px
			grdSong.Left = pnlSong.Left + 3;
			grdSong.Top = pnlSong.Top + 3;
			grdSong.Width = pnlSong.Width - 6;
			grdSong.Height = pnlSong.Height - 6;

			grdSong.Columns[0].Width = grdSong.Width;
		}

		private void FormMain_Closing(object sender, FormClosingEventArgs e)
		{
			RememberSettings();
		}

		private void RememberSettings()
		{
			Lyrics.Configuration.curSongs = txtSong.Text;
            Lyrics.Configuration.curSelectedStart = txtSong.SelectionStart;
            Lyrics.Configuration.curSelectedLength = txtSong.SelectionLength;
			if (this.Location.X > 0 && this.Location.Y > 0)
			{
                Lyrics.Configuration.curWindowLocation = this.Location; // sometimes it saves negative values
			}
            Lyrics.Configuration.curWindowSize = this.Size;
            Lyrics.Configuration.Save();
		}

		private void LoadLastSettings()
		{
			try
			{
				if (txtSong.Text.Length == 0)
				{
					txtSong.Text = Lyrics.Configuration.curSongs;
					txtSong.SelectionStart = Lyrics.Configuration.curSelectedStart;
					txtSong.SelectionLength = Lyrics.Configuration.curSelectedLength;
				}

				this.Location = Lyrics.Configuration.curWindowLocation;
				this.Size = Lyrics.Configuration.curWindowSize;
			}
			catch { } //suppress errors for the first time when there are no settings saved yets
		}

		private void btnReconnect_Click(object sender, EventArgs e)
		{
			RememberSettings();
			Process.Start(Application.ExecutablePath, "Restart");
			this.Close();
		}

		private void timerKeepConnectionAlive_Tick(object sender, EventArgs e)
		{
			try
			{
				// just to do sth with the switcher
				uint result = Lyrics.Switcher.GetMediaFromPlayer();
				Debug.Print("KeepConectionAlive: {0}", result);

				RememberSettings();
			}
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message, "ATEM Error");
				toolStripStatusLabel.Text = ex.Message;
				statusStrip1.Refresh();
			}

		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			bool result = false;
			switch (keyData)
			{
				case Keys.Control | Keys.S:
					RememberSettings();
					result = true;
					break;
				case Keys.F5:
					chkEditMode.Checked = false;
					result = true;
					break;
				case Keys.F6:
					btnOnAir_Click(null, null);
					result = true;
					break;
				case Keys.F7:
					btnOffAir_Click(null, null);
					result = true;
					break;
				case Keys.Escape:
					chkEditMode.Checked = true;
					result = true;
					break;
                case Keys.F2:
                    if (grdSong.Visible)
                    {
                        grdSong_CellDoubleClick(null, null);
                    }
                    break;
                default:
					result = base.ProcessCmdKey(ref msg, keyData);
					break;
			}
			return result;
		}

        private void btnSave_Click(object sender, EventArgs e)
		{
			RememberSettings();
		}

        #region GridEvents
        private void grdSong_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var curVerseFile = grdSong.Rows[e.RowIndex].DataBoundItem as VerseFile;
            if (curVerseFile == curVerseFile.Verse.Song.LastVerseFile)
            {
                e.Value += Lyrics.GetSongSplitter();
            }
            if (curVerseFile.Verse.IsRefrain == true)
            {
                Padding p = e.CellStyle.Padding;
                p.Left = Config.Default.RefrainePadding;
                e.CellStyle.Padding = p;
            }
        }

        private void grdSong_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            grdSong.CurrentCell.ReadOnly = false;
            grdSong.BeginEdit(false);
            Debug.Print("grdSong_CellDoubleClick");
        }

        private void grdSong_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            { 
                var curVerseFile = grdSong.Rows[e.RowIndex].DataBoundItem as VerseFile;
                string newValue = grdSong.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString();
                if (curVerseFile.Verse.Update(newValue))
                {
                    Lyrics.Select(curVerseFile); // be sure to remember what verse is selected
                    string binaryFile = SaveLyrics();
                    SendViaConsole(binaryFile, true); // then the console is going to call Lyrics.SendSelected()
                    
                    txtSong.Text = Lyrics.ToString();
                    grdSong.CurrentCell.ReadOnly = true;
                }
            }
        }

        private void grdSong_CellClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var curVerseFile = grdSong.Rows[e.RowIndex].DataBoundItem as VerseFile;
            Lyrics.Select(curVerseFile);
        }

        private void grdSong_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    Lyrics.SelectNextVerse();
                    break;
                case Keys.Up:
                    Lyrics.SelectPrevVerse();
                    break;
                case Keys.Right:
                    Lyrics.SelectNextKeyVerse();
                    break;
                case Keys.Left:
                    Lyrics.SelectPrevKeyVerse();
                    break;
                case Keys.Home:
                    Lyrics.SelectFirstVerse();
                    break;
                case Keys.End:
                    Lyrics.SelectLastVerse();
                    break;
                case Keys.PageUp:
                    Lyrics.SelectFirstVerse();
                    break;
                case Keys.PageDown:
                    Lyrics.SelectLastVerse();
                    break;
            }
        }

        #endregion


    }
}