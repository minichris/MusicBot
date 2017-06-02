using System;
using System.Windows.Forms;
using System.IO;

namespace MusicBot
{
    public partial class MusicControl : UserControl
    {
        public Music LinkedMusic;
        public MusicControl(Music MusicObj)
        {
            InitializeComponent();
            LinkedMusic = MusicObj;

            NameLabel.Text = LinkedMusic.GetName();
            FilenameLabel.Text = Path.GetFileName(LinkedMusic.GetFilePath());
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
