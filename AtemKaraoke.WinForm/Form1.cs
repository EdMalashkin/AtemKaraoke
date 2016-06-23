using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AtemKaraoke.Lib;

namespace AtemKaraoke.WinForm
{
    public partial class Form1 : Form
    {
        Song _song;

        public Form1()
        {
            InitializeComponent();
            chkEditMode_CheckedChanged(null, null);
            _song.VerseSelected += new VerseSelectedEventHandler(OnVerseSelected);
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            //_song.Number = 1;
            Convertor c = new Convertor();
            c.ConvertSongsToImages(_song);
        }

        public void OnVerseSelected(object sender, VerseSelectedEventArgs e)
        {
            Convertor c = new Convertor();
            c.SetSongToPlayer((uint)e.SelectionNumber);
        }

        private void MoveSelection()
        {
            if (txtSong.ReadOnly == false) return;

            int curPosition = txtSong.SelectionStart;
            _song.SelectVerse(curPosition);

            //string splitter = Environment.NewLine + Environment.NewLine;

            //int selectionStart = txtSong.Text.LastIndexOf(splitter, curPosition);
            //int selectionLength = txtSong.Text.IndexOf(splitter, curPosition);

            //if (curPosition < txtSong.Text.Length)
            //{
            //    int startIndex = curPosition - splitter.Length / 2;
            //    if (startIndex < 0) return;
            //    string curTextToCheckIfSplitter = txtSong.Text.Substring(startIndex, splitter.Length);
            //    if (splitter == curTextToCheckIfSplitter)  return;
            //}

            //if (selectionStart == -1)
            //    selectionStart = 0;
            //if (selectionLength == -1)
            //    selectionLength = txtSong.Text.Length;

            //selectionLength = selectionLength - selectionStart;

            //if (selectionStart >= 0 && selectionLength >= 0)
            //    txtSong.Select(selectionStart, selectionLength);
        }

        private void OnMousePressed(object sender, MouseEventArgs e)
        {
            MoveSelection();
        }

        private void chkEditMode_CheckedChanged(object sender, EventArgs e)
        {
            txtSong.ReadOnly = !chkEditMode.Checked;
            txtSong.Cursor = null;
            btnGenerate.Enabled = !chkEditMode.Checked;
            chkUpload.Enabled = !chkEditMode.Checked;

            if (!chkEditMode.Checked)
            {
                _song = new Song();
                _song.Text = txtSong.Text;
                txtSong.Text = _song.Text; // to see what is inside and select exactly what is inside
            }
        }

        private void OnMousePressed(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    //MoveSelection();
                    //MouseEventArgs a = new MouseEventArgs(MouseButtons.Left, 1, Cursor.Position.X, Cursor.Position.Y, 0);

                break;
            }

        }
    }
}
