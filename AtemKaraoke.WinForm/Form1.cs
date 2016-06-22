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
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            Song song = new Song();
            song.Text = txtSong.Text;
            song.Number = 1;
            Convertor c = new Convertor();
            c.ConvertSongsToImages(song);

        }

        private void OnMousePressed(object sender, MouseEventArgs e)
        {
            int curPosition = txtSong.SelectionStart;
            string splitter = Environment.NewLine + Environment.NewLine;

            int selectionStart = txtSong.Text.LastIndexOf(splitter, curPosition);
            int selectionLength = txtSong.Text.IndexOf(splitter, curPosition);

            string curTextToCheckIfSplitter = txtSong.Text.Substring(curPosition - splitter.Length/2, splitter.Length);
            if (splitter == curTextToCheckIfSplitter)
                return;

            if (selectionStart == -1)
                selectionStart = 0;
            if (selectionLength == -1)
                selectionLength = txtSong.Text.Length;

            selectionLength = selectionLength - selectionStart;

            if (selectionStart >= 0 && selectionLength >= 0)
                txtSong.Select(selectionStart, selectionLength);
        }

    }
}
