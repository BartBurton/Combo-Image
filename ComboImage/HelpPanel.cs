using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace ComboImage
{
    public class HelpPanel : Panel
    {
        PictureBox pictureBoxForIcon = new PictureBox();
        public Image Icon
        {
            get => pictureBoxForIcon.Image;
            set => pictureBoxForIcon.Image = value;
        }

        public string HelpText { get; set; }

        Color backColor;
        public new Color BackColor
        {
            get => backColor;
            set
            {
                backColor = value;
                ((Panel)this).BackColor = value;
            }
        }

        public HelpPanel(ref Panel parent)
        {
            parent.Controls.Add(this);
            Width = parent.Width / 30;
            Height = Width;
            Controls.Add(pictureBoxForIcon);
            MouseLeave += HelpPanel_MouseLeave;
            MouseMove += HelpPanel_MouseMove;


            pictureBoxForIcon.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxForIcon.Width = (Width * 8 / 10);
            pictureBoxForIcon.Height = pictureBoxForIcon.Width;
            pictureBoxForIcon.Location = new Point((Width - pictureBoxForIcon.Width) / 2, (Height - pictureBoxForIcon.Height) / 2);
            pictureBoxForIcon.Click += HelpPanel_Click;
            pictureBoxForIcon.MouseLeave += HelpPanel_MouseLeave;
            pictureBoxForIcon.MouseMove += HelpPanel_MouseMove;
        }

        private void HelpPanel_MouseMove(object sender, MouseEventArgs e)
        {
            ((Panel)this).BackColor = Color.FromArgb(50, BackColor);
        }

        private void HelpPanel_Click(object sender, EventArgs e)
        {
            MessageBox.Show(HelpText);
        }

        private void HelpPanel_MouseLeave(object sender, EventArgs e)
        {
            ((Panel)this).BackColor = backColor;
        }
    }
}
