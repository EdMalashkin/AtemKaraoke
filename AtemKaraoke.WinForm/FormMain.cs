﻿using System;
using System.Windows.Forms;
using AtemKaraoke.Core;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using AtemKaraoke.Core.Tools;

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
			SetStatus("Edit Mode");
		}

		private void SetStatus(string message)
		{
			toolStripStatusLabel.Text = message;
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

		private void SelectSongOnCursor()
		{
			txtSong.HideSelection = false;

			var cursorPos = txtSong.SelectionStart;
			var lyr = new Lyrics(txtSong.Text);
			txtSong.Text = lyr.ToString(); //comment because it cuts if more than 19 selected // returned it back because of wrong selection of some songs with extra spaces, etc
			Song selectedSong = null;
			foreach (var song in lyr.Songs)
			{
				if (cursorPos >= song.GetFirstCharPosition() && cursorPos >= song.GetLastCharPosition())
				{
					selectedSong = song;
				}
			}
			if (selectedSong != null)
			{
				txtSong.Select(selectedSong.GetFirstCharPosition(), selectedSong.ToString().Length);
				SetStatusOfSelection(selectedSong);
			}
			else
			{
				SetStatusOfSelection(null);
			}
		}

		private void SetStatusOfSelection(Song selectedSong)
		{
			string msg = string.Empty;
			if (selectedSong != null)
			{
				if (selectedSong.VersesUnlimited.Count > selectedSong.VerseFiles.Count)
				{
					msg = String.Format("An attempt to select {0} verses - more than allowed ({1})", selectedSong.VersesUnlimited.Count, selectedSong.VerseFiles.Count);
				}
				else
				{
					msg = String.Format("{0} verses selected", selectedSong.VersesUnlimited.Count);
				}
			}
			SetStatus(msg);
		}

		private DataGridViewCellStyle RefrainStyle()
		{
			var s = new DataGridViewCellStyle();
			s.BackColor = Color.Yellow;
			s.SelectionBackColor = Color.Yellow;
			s.SelectionForeColor = Color.Black;
			return s;
		}

		string _lastNotSelectedText = "";
		int _lastSelectedTextPosition = 0;

		private void CreateNewLyrics()
		{
			string selectedText = GetSelectedSongText;
			if (selectedText.Length > 0)
			{
				_lastSelectedTextPosition = txtSong.Text.IndexOf(selectedText);
				_lastNotSelectedText = txtSong.Text.Replace(selectedText, "");
			}

			_lyrics = new Lyrics(selectedText);
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
			SetStatus(_labelOffAir);
			txtSong.Visible = false;
			chkExport.Visible = false;
			chkAutolist.Visible = false;
			grdSong.Visible = true;
			pnlSong.Visible = true;
			lstSongs.Visible = false;

			Cursor = Cursors.Default;
			grdSong.Enabled = true;
			grdSong.Focus();
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
			SetStatus("Edit Mode");
			txtSong.Visible = true;
			chkExport.Visible = true;
			chkAutolist.Visible = true;
			grdSong.Visible = false;
			pnlSong.Visible = false;
			chkAutolist_CheckedChanged(null, null);
		}

		private void chkEditMode_CheckedChanged(object sender, EventArgs e)
		{
			if (!chkEditMode.Checked) SetLiveMode(); else SetEditMode();
		}

		private void SendViaConsole(string path, bool sendSelected = false)
		{
			var r = new AtemKaraokeConsole(path, sendSelected);
			r.Run();
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
					SetStatus(_labelPreview);
				}
				else if (btnOnAir.Text == _labelOnAir)
				{
					Lyrics.Switcher.SetMediaOnAir();
					pnlSong.BackColor = Color.Red;
					btnOnAir.Text = _labelOffAir; // declare the next action
					btnCancelPreview.Visible = false;
					SetStatus(_labelOnAir + "!");
				}
				else
				{
					Lyrics.Switcher.SetMediaOffAir();
					pnlSong.BackColor = System.Drawing.SystemColors.Control;
					btnOnAir.Text = _labelPreview;
					btnCancelPreview.Visible = false;
					SetStatus(_labelOffAir);
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
			lstSongs.Width = txtSong.Width;
			if (chkAutolist.Checked)
			{
				txtSong.Top = lstSongs.Top + 10 + lstSongs.Height;
				txtSong.Height = this.Height - lstSongs.Height - 110;
			}
			else
			{
				txtSong.Top = lstSongs.Top;
				txtSong.Height = this.Height - 100;
			}

			pnlSong.Left = txtSong.Left;
			pnlSong.Top = lstSongs.Top;
			pnlSong.Width = txtSong.Width;
			pnlSong.Height = lstSongs.Height + 10 + txtSong.Height;

			// red border 3px
			grdSong.Left = pnlSong.Left + 3;
			grdSong.Top = lstSongs.Top + 3;
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
			Lyrics.Configuration.curAutolist = chkAutolist.Checked;
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
				chkAutolist.Checked = Lyrics.Configuration.curAutolist;
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
				SetStatus(ex.Message);
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
				case Keys.F4:
					SelectSongOnCursor();
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

					txtSong.Text = _lastNotSelectedText.Insert(_lastSelectedTextPosition, Lyrics.ToString());
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

		private void txtSong_TextChanged(object sender, EventArgs e)
		{
			BindList();
			SetStatus("Edit Mode");
		}

		private void BindList()
		{
			if (!chkAutolist.Checked) return;
			var lyr = new Lyrics(txtSong.Text);
			lstSongs.DataSource = new BindingSource(lyr.SongList, null);
			lstSongs.DisplayMember = "Value";
			lstSongs.ValueMember = "Key";
			if (lyr.SongList.Count > 0)
			{
				lstSongs.SelectedIndex = 0;
			}
		}

		private void lstSongs_SelectedIndexChanged(object sender, EventArgs e)
		{
			int songStart = 0;
			if (int.TryParse(lstSongs.SelectedValue.ToString(), out songStart))
			{
				txtSong.SelectionStart = songStart;
				txtSong.SelectionLength = 0;
				SelectSongOnCursor();
				txtSong.Focus();
				//txtSong.ScrollToCaret(); doesn't always go to the beginning of the selection
				SetScroll(txtSong, txtSong.GetLineFromCharIndex(songStart));
				lstSongs.Focus(); // focus back to be able to use keys
			}
		}

		[DllImport("user32.dll")]
		static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

		private void SetScroll(TextBox tbx, int lineIndex)
		{
			const int EM_GETFIRSTVISIBLELINE = 0x00CE;
			const int EM_LINESCROLL = 0x00B6;
			int currentLine = SendMessage(tbx.Handle, EM_GETFIRSTVISIBLELINE, 0, 0);
			SendMessage(tbx.Handle, EM_LINESCROLL, 0, lineIndex - currentLine);
		}

		private void chkAutolist_CheckedChanged(object sender, EventArgs e)
		{
			if (chkAutolist.Checked)
			{
				BindList();
				lstSongs.Height = 120;
				lstSongs.Visible = true;
				lstSongs.Focus();
			}
			else
			{
				lstSongs.Visible = false;
				lstSongs.Height = 0;
			}
			ResizeSongControls();
		}
	}
}