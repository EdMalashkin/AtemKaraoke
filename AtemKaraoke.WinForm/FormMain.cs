﻿using System;
using System.Windows.Forms;
using AtemKaraoke.Core;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace AtemKaraoke.WinForm
{
	public partial class FormMain : Form
	{
		
		bool _isRestart;

        //App _app;
        //private App App
        //{
        //    get
        //    {
        //        if (_app == null) _app = new App();
        //        return _app;
        //    }
        //}

        Lyrics _lyrics;
        private object True;

        private Lyrics Lyrics
        {
            get
            {
                if (_lyrics == null) _lyrics = new Lyrics(GetSelectedSongText);
                return _lyrics;
            }
        }

        //App _app;
        //private App App
        //{
        //    get
        //    {
        //        if (_app == null) _app = new App();
        //        return _app;
        //    }
        //}

        public FormMain()
		{
			InitializeComponent();
			LoadLastSettings();
			chkEditMode.Checked = true;
			toolTip.SetToolTip(chkEditMode, "Press F5");
			toolTip.SetToolTip(btnOnAir, "Press F6");
			toolTip.SetToolTip(btnCancelPreview, "Press F7");
			toolStripStatusLabel.Text = "Edit Mode";
			statusStrip1.Refresh();
		}

		public FormMain(bool isRestart)
		{
			_isRestart = isRestart;
			InitializeComponent();
			LoadLastSettings();
			chkEditMode.Checked = false;
			toolTip.SetToolTip(chkEditMode, "Press Esc");
			toolTip.SetToolTip(btnOnAir, "Press F6");
			toolTip.SetToolTip(btnCancelPreview, "Press F7");
			toolStripStatusLabel.Text = "Edit Mode";
			statusStrip1.Refresh();
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

        private void BindGrid()
        {
            grdSong.AutoGenerateColumns = false;
            _lyrics = new Lyrics(GetSelectedSongText);
            grdSong.DataSource = _lyrics.VerseFiles;
            //foreach (var v in _songs.Verses)
            //{
            //    int idx = grdSong.Rows.Add(v.Text);
            //    var r = grdSong.Rows[idx];
            //    if (v.Number == 1)
            //    {
            //        foreach (DataGridViewCell cell in r.Cells)
            //        {
            //            //cell.AdjustCellBorderStyle(newStyle, null, false, false, false, false);
            //            cell.Style.ApplyStyle(RefrainStyle());
            //            //
            //        }
            //    }
            //    if (v.Number == 2)
            //    {
            //        foreach (DataGridViewCell cell in r.Cells)
            //        {
            //            //cell.Style.Padding = ;
            //        }
            //    }

            //   }
        }

		private void chkEditMode_CheckedChanged(object sender, EventArgs e)
		{
			if (!chkEditMode.Checked)
			{
				//_songs.VerseSelected += new VerseSelectedEventHandler(OnVerseSelected); //is it necessary to have????
				
                BindGrid();
                ResizeSongControls();
				grdSong.Enabled = false;
				Cursor = Cursors.WaitCursor;

				try
				{
					if (_isRestart == false && chkExport.Checked == true)
					{
                        Upload();
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

					chkEditMode.Text = "Back To Edit Mode";
					toolTip.SetToolTip(chkEditMode, "Press Esc");
					btnOnAir.Text = "Preview";
					btnOnAir.Visible = true;
					chkExport.Checked = true; // keep it true for the next time
					toolStripStatusLabel.Text = "Off Air";
					statusStrip1.Refresh();
					grdSong.Focus();
					//btnReconnect.Visible = true; commented as images are not get generated after reconnecting for some reason
				}
			}
			else
			{
				chkEditMode.Text = "Go To Live Mode";
				toolTip.SetToolTip(chkEditMode, "Press F5");
				btnReconnect.Visible = false;
				btnOnAir.Text = "Anything";
				pnlSong.BackColor = System.Drawing.SystemColors.Control;
				btnOnAir.Visible = false;
				btnCancelPreview.Visible = false;
                Lyrics.Switcher.SetMediaOffAir();
				toolStripStatusLabel.Text = "Edit Mode";
				statusStrip1.Refresh();
			}

			txtSong.Visible = chkEditMode.Checked;
			chkExport.Visible = chkEditMode.Checked;
			grdSong.Visible = !chkEditMode.Checked;
			pnlSong.Visible = !chkEditMode.Checked;
			
		}

        private void SendViaConsole(string path)
        {
            string MyBatchFile = @"AtemKaraoke.Console.exe";
            var process = new Process
            {
                StartInfo = {
                                Arguments = string.Format("\"{0}\"",  path)
                            }
            };
            process.StartInfo.FileName = MyBatchFile;
            bool b = process.Start();
        }

        private void Upload()
        {
            if (!Lyrics.Configuration.UseConsoleToUploadFromWinForm)
            {
                Lyrics.Send();
            }
            else
            {   // this works today:
                string folder = Lyrics.Save();
                SendViaConsole(folder);
            }
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

		public void OnVerseSelected(object sender, VerseSelectedEventArgs e)
		{
			//grdSong.Enabled = false; // cannot do that because the grid looses focus
			grdSong.Cursor = Cursors.WaitCursor;

			try
			{
                Lyrics.Switcher.SetMediaToPlayer(e.SelectionNumber);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "ATEM Error");
			}
			finally
			{
				//grdSong.Enabled = true;
				grdSong.Cursor = Cursors.Default;
			}

			Debug.Print("OnVerseSelected " + e.SelectionNumber.ToString());
		}

		private void grdSong_SelectionChanged(object sender, EventArgs e)
		{
            var curVerseFile = grdSong.Rows[grdSong.CurrentRow.Index].DataBoundItem as VerseFile;
            Lyrics.Select(curVerseFile); 
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
				Debug.Print(string.Format("KeepConectionAlive: {0}", result));

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

		private void FormMain_Load(object sender, EventArgs e)
		{

		}

        #region GridEvents

        private void grdSong_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var curVerseFile = grdSong.Rows[e.RowIndex].DataBoundItem as VerseFile;
            if (curVerseFile.Verse == curVerseFile.Verse.Song.LastVerse)
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
            //var curVerseFile = grdSong.Rows[e.RowIndex].DataBoundItem as VerseFile;
            //DataGridViewCell cell = grdSong.Rows[e.RowIndex].Cells[e.ColumnIndex];
            //cell.ReadOnly = false;
            //grdSong.CurrentCell = cell;
            //grdSong.EditMode = DataGridViewEditMode.EditOnEnter;
            ////grdSong.CurrentCell.KeyEntersEditMode
            grdSong.CurrentCell.ReadOnly = false;
            //grdSong.BeginEdit(true);
        }

        private void grdSong_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            { 
                DataGridViewCell cell = grdSong.Rows[e.RowIndex].Cells[e.ColumnIndex];
                var curVerseFile = grdSong.Rows[e.RowIndex].DataBoundItem as VerseFile;
                string newVerseValue = cell.EditedFormattedValue.ToString().Trim();
                string oldVerseValue = curVerseFile.Verse.Text.Trim();
                if (newVerseValue != oldVerseValue)
                {
                    curVerseFile.Verse.Text = newVerseValue;
                    string filePath = curVerseFile.Save();
                    SendViaConsole(filePath);
                    txtSong.Text = Lyrics.ToString();
                    grdSong.CurrentCell.ReadOnly = true;
                }
            }
        }

        #endregion
    }
}