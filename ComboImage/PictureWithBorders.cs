using System;
using System.Windows.Forms;
using System.Drawing;

namespace ComboImage
{
    /// <summary>
    /// Класс необходимый для представления картинок загруженных из папки.
    /// </summary>
    class PictureWithBorders : Panel
    {
        /// <summary>
        /// Загруженное изображение.
        /// </summary>
        public PictureBox Picture { get; set; } = new PictureBox();

        /// <summary>
        /// Событие активации картинки.
        /// </summary>
        public event Action<Panel> Active;
        /// <summary>
        /// Событие деактивации картинки.
        /// </summary>
        public event Action<Panel> Deactive;


        /// <summary>
        /// Граница необходимая для выделения картинки.
        /// </summary>
        Panel bottomBorder = new Panel();

        /// <summary>
        /// Цвет границы.
        /// </summary>
        Color BorderColor;



        public PictureWithBorders(string nameFile, int width, ref Color color)
        {
            Picture.SizeMode = PictureBoxSizeMode.Zoom;
            Picture.Image = Image.FromFile(nameFile);
            Picture.Width = (8 * width) / 10;
            Picture.Height = (8 * Picture.Width) / 10;

            Width = Picture.Width;
            Height = Picture.Height + (Picture.Height / 20);

            BorderColor = color;
            bottomBorder.BackColor = BorderColor;

            bottomBorder.Width = Width;
            bottomBorder.Height = Picture.Height / 20;
            bottomBorder.Location = new Point(0, Picture.Height);

            Controls.Add(Picture);
            Controls.Add(bottomBorder);

            Picture.MouseMove += PictureWithBorders_MouseMove;
            Picture.MouseLeave += PictureWithBorders_MouseLeave;
            Picture.MouseUp += Picture_Click;
        }



        private void Picture_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Active(this); 
            }
            else if(e.Button == MouseButtons.Right)
            {
                Deactive(this);
            }
        }


        /// <summary>
        /// Изменить цвет границы, при наведении на картинку.
        /// </summary>
        private void PictureWithBorders_MouseMove(object sender, MouseEventArgs e)
        {
            bottomBorder.BackColor = Color.FromArgb(80, BorderColor);
        }

        /// <summary>
        /// Вернуть цвет границы, при уведении курсора с картинки.
        /// </summary>
        private void PictureWithBorders_MouseLeave(object sender, EventArgs e)
        {
            bottomBorder.BackColor = BorderColor;
        }
    }
}
