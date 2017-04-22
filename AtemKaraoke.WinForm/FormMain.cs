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

        const string _labelPreview = "Preview";
        const string _labelOnAir = "On Air";
        const string _labelOffAir = "Off Air";

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
            _lyrics.Selection.OnVerseSelected += new EventHandler(this.OnVerseSelected);
        }

        private void BindGrid()
        {
            grdSong.AutoGenerateColumns = false;
            grdSong.DataSource = Lyrics.VerseFiles;
            Lyrics.Selection.ToFirstVerse(); ;
        }

        private void SetLiveMode()
        {
            grdSong.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                CreateNewLyrics();
                if (_isRestart == false && chkExport.Checked == true)
                {
                    Upload();
                }
                BindGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ATEM Error");
            }

            ResizeSongControls();
            chkEditMode.Text = "Back To Edit Mode";
            toolTip.SetToolTip(chkEditMode, "Press Esc");
            btnOnAir.Text = _labelPreview;
            btnOnAir.Visible = true;
            chkExport.Checked = true; // keep it true for the next time
            toolStripStatusLabel.Text = _labelOffAir;
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
            try
            {
                Lyrics.Switcher.SetMediaOffAir();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ATEM Error");
            }
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
			if (path.Trim().Length > 0)
			{
				var process = new Process();
				process.StartInfo = new ProcessStartInfo(@"AtemKaraoke.Console.exe");
				process.StartInfo.Arguments = string.Format("\"{0}\"", path);
				if (sendSelected)
					process.StartInfo.Arguments += " sendSelected";
				process.Start();
			}
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
			string binaryFile = "";
			try
			{
				Lyrics.Selection.OnVerseSelected -= new EventHandler(this.OnVerseSelected); // to avoid {"Type 'System.Windows.Forms.Form' in Assembly 'System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' is not marked as serializable
				var tempSwitcher = Lyrics.Switcher;
				Lyrics.Switcher = null;
				binaryFile = new BinaryFileLyrics(Lyrics).Save();
				Lyrics.Selection.OnVerseSelected += new EventHandler(this.OnVerseSelected);
				Lyrics.Switcher = tempSwitcher;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "ATEM Error");
			}
			return binaryFile;
		}

		private void btnOnAir_Click(object sender, EventArgs e)
		{
			try
			{
				grdSong.Enabled = false;
				Cursor = Cursors.WaitCursor;

				if (btnOnAir.Text == _labelPreview)
				{
					Lyrics.Switcher.SetMediaToPreview();
					pnlSong.BackColor = Color.LightGreen;
					btnOnAir.Text = _labelOnAir; // declare the next action
					btnCancelPreview.Visible = true;
					toolStripStatusLabel.Text = _labelPreview;
					statusStrip1.Refresh();
				}
				else if (btnOnAir.Text == _labelOnAir)
				{
                    Lyrics.Switcher.SetMediaOnAir();
					pnlSong.BackColor = Color.Red;
					btnOnAir.Text = _labelOffAir; // declare the next action
					btnCancelPreview.Visible = false;
					toolStripStatusLabel.Text = _labelOnAir + "!";
					statusStrip1.Refresh();
				}
				else
				{
                    Lyrics.Switcher.SetMediaOffAir();
					pnlSong.BackColor = System.Drawing.SystemColors.Control;
					btnOnAir.Text = _labelPreview;
					btnCancelPreview.Visible = false;
					toolStripStatusLabel.Text = _labelOffAir;
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
            if (curVerse.IndexBasedOnZeroToSelect.HasValue)
            {
                grdSong.Rows[curVerse.IndexBasedOnZeroInGrid].Selected = true;
                Debug.Print("OnVerseSelected {0}", curVerse.IndexBasedOnZeroToSelect);
            }
        }

		private void grdSong_SelectionChanged(object sender, EventArgs e)
		{
            if (grdSong.CurrentRow != null && Lyrics.Selection.CurrentVerse != null && Lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect.HasValue)
            {
                var curVerseFile = grdSong.Rows[grdSong.CurrentRow.Index].DataBoundItem as VerseFile;
                if (curVerseFile.IndexBasedOnZeroToSelect != Lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect)
                {
                    //attempt to cancel the autoselection with arrows when it contradicts with Lyrics.SelectedVerse
                    grdSong.Rows[Lyrics.Selection.CurrentVerse.IndexBasedOnZeroInGrid].Selected = true;
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
                case Keys.Control | Keys.A:
                    if (txtSong.Visible) txtSong.SelectAll();
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
                    if (!IsCellBeingEdited())
                    {
                        chkEditMode.Checked = true;
                    }
                    else
                    {
                        if (grdSong.Visible) CancelEditingCell();
                    }
					result = true;
					break;
                case Keys.F2:
                    if (grdSong.Visible) StartEditingCell(grdSong.CurrentCell);
                    result = true;
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
                p.Left = Config.Default.RefrainPadding;
                e.CellStyle.Padding = p;
            }
            grdSong.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = "Double-click or F2 to edit a selected verse. Right-click to edit any verse";
        }

        private void grdSong_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            StartEditingCell(e.RowIndex, e.ColumnIndex);
            Debug.Print("grdSong_CellDoubleClick");
        }

        private DataGridViewCell _selectedBeforeEditingCell;
        private bool IsCellBeingEdited()
        {
            return _selectedBeforeEditingCell != null;
        }

        private void StartEditingCell(DataGridViewCell cell)
        {
            if (cell != null)
            {
                _selectedBeforeEditingCell = grdSong.CurrentCell;
                cell.ReadOnly = false;
                cell.Selected = true;
                grdSong.BeginEdit(false);
            }
        }

        private void StartEditingCell(int rowIndex, int columnIndex)
        {
            if (rowIndex >= 0 && columnIndex >= 0)
            {
                var cell = grdSong.Rows[rowIndex].Cells[columnIndex];
                StartEditingCell(cell);
            }
        }

        private void CancelEditingCell()
        {
            grdSong.CancelEdit();
            grdSong.CurrentCell.ReadOnly = true;
            _selectedBeforeEditingCell = null;
        }

        private void EndEditingCell(int rowIndex, int columnIndex)
        {
            if (rowIndex >= 0 && columnIndex >= 0)
            {
                var curCell = grdSong.Rows[rowIndex].Cells[columnIndex];
                var curVerseFile = grdSong.Rows[rowIndex].DataBoundItem as VerseFile;
                string newValue = curCell.EditedFormattedValue.ToString();
                if (curVerseFile.Verse.Update(newValue))
                {
                    string binaryFile = SaveLyrics();
                    SendViaConsole(binaryFile, true); // then the console is going to call Lyrics.SendSelected()

                    txtSong.Text = Lyrics.ToString();
                    grdSong.EndEdit();
                    curCell.ReadOnly = true;
                    if (_selectedBeforeEditingCell != null) _selectedBeforeEditingCell.Selected = true;
                }
            }
            _selectedBeforeEditingCell = null;
        }


        private void grdSong_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            EndEditingCell(e.RowIndex, e.ColumnIndex);
        }

        private void grdSong_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!grdSong.IsCurrentCellInEditMode)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var curVerseFile = grdSong.Rows[e.RowIndex].DataBoundItem as VerseFile;
                    Lyrics.Selection.ToVerse(curVerseFile);
                }
                else if (e.Button == MouseButtons.Right)
                {
                    StartEditingCell(e.RowIndex, e.ColumnIndex);
                }
            }
        }

        private void grdSong_KeyDown(object sender, KeyEventArgs e)
        {
            if (!grdSong.IsCurrentCellInEditMode)
            {
                switch (e.KeyCode)
                {
                    case Keys.Down:
                        Lyrics.Selection.ToNextVerse();
                        break;
                    case Keys.Up:
                        Lyrics.Selection.ToPrevVerse();
                        break;
                    case Keys.Right:
                        Lyrics.Selection.ToNextKeyVerse();
                        break;
                    case Keys.Left:
                        Lyrics.Selection.ToPrevKeyVerse();
                        break;
                    case Keys.Home:
                        Lyrics.Selection.ToFirstVerse();
                        break;
                    case Keys.End:
                        Lyrics.Selection.ToLastVerse();
                        break;
                    case Keys.PageUp:
                        Lyrics.Selection.ToFirstVerse();
                        break;
                    case Keys.PageDown:
                        Lyrics.Selection.ToLastVerse();
                        break;
                }
            }
        }

        #endregion


    }
}