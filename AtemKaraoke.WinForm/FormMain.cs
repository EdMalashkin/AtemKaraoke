using System;
using System.Windows.Forms;
using AtemKaraoke.Lib;
using System.Diagnostics;
using System.Collections.Generic;
using SwitcherLib;

namespace AtemKaraoke.WinForm
{
    public partial class FormMain : Form
    {
        Song _song;
        Controller _controller;
        bool _isRestart;

        private Controller Controller
        {
            get
            {
                if (_controller == null) _controller = new Controller();
                return _controller;
            }
        }

        public FormMain()
        {
            InitializeComponent();
            LoadLastSettings();
            chkEditMode.Checked = true;
        }

        public FormMain(bool isRestart)
        {
            _isRestart = isRestart;
            InitializeComponent();
            LoadLastSettings();
            chkEditMode.Checked = false;
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

        private void chkEditMode_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkEditMode.Checked)
            {
                _song = new Song();
                _song.Text = GetSelectedSongText;
                _song.VerseSelected += new VerseSelectedEventHandler(OnVerseSelected);
                //txtSong.Text = _song.Text; // to see what is inside and select exactly what is inside

                ResizeSongControls();

                grdSong.AutoGenerateColumns = false;
                grdSong.DataSource = _song.Verses;

                grdSong.Enabled = false;
                Cursor = Cursors.WaitCursor;

                try
                {
                    if (_isRestart != true)
                    {
                        string newFolder = Controller.ConvertSongsToImages(_song);

                        if (!Controller.UseConsoleToUploadFromWinForm)
                        {
                            Controller.UploadSongsToSwitcher(_song);
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ATEM Error");
                }
                finally
                {
                    grdSong.Enabled = true;
                    Cursor = Cursors.Default;
                }
                chkEditMode.Text = "Back To Edit Mode";
                //btnReconnect.Visible = true; commented as images are not get generated after reconnecting for some reason
            }
            else
            {
                chkEditMode.Text = "Go Live";
                btnReconnect.Visible = false;
            }

            txtSong.Visible = chkEditMode.Checked;
            grdSong.Visible = !chkEditMode.Checked;
        }

        public void OnVerseSelected(object sender, VerseSelectedEventArgs e)
        {
            //grdSong.Enabled = false; // cannot do that because the grid looses focus
            grdSong.Cursor = Cursors.WaitCursor;

            try
            {
                Controller.SetSongToPlayer((uint)e.SelectionNumber - 1);
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
            _song.SelectVerse((Verse)grdSong.CurrentRow.DataBoundItem);
        }

        private void txtSong_Resized(object sender, EventArgs e)
        {
            ResizeSongControls();
        }

        private void ResizeSongControls()
        {
            grdSong.Left = txtSong.Left;
            grdSong.Top = txtSong.Top;
            grdSong.Width = txtSong.Width;
            grdSong.Height = txtSong.Height;
            grdSong.Columns[0].Width = grdSong.Width - 3;
        }

        private void FormMain_Closing(object sender, FormClosingEventArgs e)
        {
            RememberSettings();
        }

        private void RememberSettings()
        {
            Controller.Configuration.curSongs = txtSong.Text;
            Controller.Configuration.curSelectedStart = txtSong.SelectionStart;
            Controller.Configuration.curSelectedLength = txtSong.SelectionLength;
            if (this.Location.X > 0 && this.Location.Y > 0)
            {
                Controller.Configuration.curWindowLocation = this.Location; // sometimes it saves negative values
            }
            Controller.Configuration.curWindowSize = this.Size;
            Controller.Configuration.Save();
        }

        private void LoadLastSettings()
        {
            try
            {
                if (txtSong.Text.Length == 0)
                {
                    txtSong.Text = Controller.Configuration.curSongs;
                    txtSong.SelectionStart = Controller.Configuration.curSelectedStart;
                    txtSong.SelectionLength = Controller.Configuration.curSelectedLength;
                }
                    
                this.Location = Controller.Configuration.curWindowLocation;
                this.Size = Controller.Configuration.curWindowSize;
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
                uint result = Controller.GetSongFromPlayer();
                Debug.Print(string.Format("KeepConectionAlive: {0}", result));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ATEM Error");
            }

        }
    }
}
