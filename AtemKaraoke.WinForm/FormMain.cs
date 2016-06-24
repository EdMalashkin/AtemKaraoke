﻿using System;
using System.Windows.Forms;
using AtemKaraoke.Lib;
using System.Diagnostics;

namespace AtemKaraoke.WinForm
{
    public partial class FormMain : Form
    {
        Song _song;
        Controller _controller;

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
            chkEditMode_CheckedChanged(null, null);
        }

        private void chkEditMode_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkEditMode.Checked)
            {
                _song = new Song();
                _song.Text = txtSong.Text;
                _song.VerseSelected += new VerseSelectedEventHandler(OnVerseSelected);
                //txtSong.Text = _song.Text; // to see what is inside and select exactly what is inside

                ResizeSongControls();

                grdSong.AutoGenerateColumns = false;
                grdSong.DataSource = _song.Verses;

                grdSong.Enabled = false;
                Cursor = Cursors.WaitCursor;

                try
                {
                    Controller.ConvertSongsToImages(_song);
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
            }

            txtSong.Visible = chkEditMode.Checked;
            grdSong.Visible = !chkEditMode.Checked;
        }

        public void OnVerseSelected(object sender, VerseSelectedEventArgs e)
        {
            grdSong.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                Controller.SetSongToPlayer((uint)e.SelectionNumber);
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
    }
}
