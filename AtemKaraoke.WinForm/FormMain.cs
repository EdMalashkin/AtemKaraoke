﻿using System;
using System.Windows.Forms;
using AtemKaraoke.Lib;
using System.Diagnostics;
using System.Collections.Generic;
using SwitcherLib;
using System.Drawing;

namespace AtemKaraoke.WinForm
{
	public partial class FormMain : Form
	{
		Songs _songs;
		App _app;
		bool _isRestart;

		private App App
		{
			get
			{
				if (_app == null) _app = new App();
				return _app;
			}
		}

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

        private void grdSong_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            var curVerse = grdSong.Rows[e.RowIndex].DataBoundItem as Verse;
            if (curVerse == curVerse.Song.LastVerse)
            {
                using (Pen p = new Pen(Brushes.Black, 1))
                {
                    e.Graphics.DrawLine(p, new Point(e.CellBounds.Left, e.CellBounds.Bottom), new Point(e.CellBounds.Right, e.CellBounds.Bottom));
                }
                e.Handled = true;
            }
        }

        private void BindGrid()
        {
            grdSong.AutoGenerateColumns = false;
            _songs = new Songs(GetSelectedSongText);
            grdSong.DataSource = _songs.Verses;
            return;

            foreach (var v in _songs.Verses)
            {
                int idx = grdSong.Rows.Add(v.Text);
                var r = grdSong.Rows[idx];
                if (v.Number == 1)
                {
                    foreach (DataGridViewCell cell in r.Cells)
                    {
                        //cell.AdjustCellBorderStyle(newStyle, null, false, false, false, false);
                        cell.Style.ApplyStyle(RefrainStyle());
                        //
                    }
                }
                if (v.Number == 2)
                {
                    foreach (DataGridViewCell cell in r.Cells)
                    {
                        //cell.Style.Padding = ;
                    }
                }

               }
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
				App.SetSongOffAir();
				toolStripStatusLabel.Text = "Edit Mode";
				statusStrip1.Refresh();
			}

			txtSong.Visible = chkEditMode.Checked;
			chkExport.Visible = chkEditMode.Checked;
			grdSong.Visible = !chkEditMode.Checked;
			pnlSong.Visible = !chkEditMode.Checked;
			
		}

        private void Upload()
        {
            string newFolder = App.ConvertSongsToImages(_songs.Verses, _songs.Name);
            if (!App.UseConsoleToUploadFromWinForm)
            {
                App.UploadSongsToSwitcher(_songs.Verses);
            }
            else
            {
                Console.Write(newFolder);
                string MyBatchFile = @"AtemKaraoke.Console.exe";

                var process = new Process
                {
                    StartInfo = {
                                        Arguments = string.Format("\"{0}\"",  newFolder)
                                    }
                };
                process.StartInfo.FileName = MyBatchFile;
                bool b = process.Start();
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
					App.SetSongToPreview();
					pnlSong.BackColor = System.Drawing.Color.LightGreen;
					btnOnAir.Text = "On Air"; // declare the next action
					btnCancelPreview.Visible = true;
					toolStripStatusLabel.Text = "Preview";
					statusStrip1.Refresh();
				}
				else if (btnOnAir.Text == "On Air")
				{
					App.SetSongOnAir();
					pnlSong.BackColor = System.Drawing.Color.Red;
					btnOnAir.Text = "Off Air"; // declare the next action
					btnCancelPreview.Visible = false;
					toolStripStatusLabel.Text = "On Air!";
					statusStrip1.Refresh();
				}
				else
				{
					App.SetSongOffAir();
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
				App.SetSongToPlayer((uint)e.SelectionNumber - 1);
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
			//_songs.SelectVerse(_songs.Verses[grdSong.CurrentRow.Index]); 
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
			App.Configuration.curSongs = txtSong.Text;
			App.Configuration.curSelectedStart = txtSong.SelectionStart;
			App.Configuration.curSelectedLength = txtSong.SelectionLength;
			if (this.Location.X > 0 && this.Location.Y > 0)
			{
				App.Configuration.curWindowLocation = this.Location; // sometimes it saves negative values
			}
			App.Configuration.curWindowSize = this.Size;
			App.Configuration.Save();
		}

		private void LoadLastSettings()
		{
			try
			{
				if (txtSong.Text.Length == 0)
				{
					txtSong.Text = App.Configuration.curSongs;
					txtSong.SelectionStart = App.Configuration.curSelectedStart;
					txtSong.SelectionLength = App.Configuration.curSelectedLength;
				}

				this.Location = App.Configuration.curWindowLocation;
				this.Size = App.Configuration.curWindowSize;
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
				uint result = App.GetSongFromPlayer();
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


    }
}