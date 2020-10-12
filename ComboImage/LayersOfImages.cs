using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;


namespace ComboImage
{
    public class LayersOfImages : PictureBox
    {
        /// <summary>
        /// Массив изображений содержащихся на полотне и размеры каждого изображения отмаштабированные до размеров полотна.
        /// </summary>
        (Image Image, float Width, float Height)[] images = new (Image, float, float)[Window.NUMBER_OF_FOLDERS];

        /// <summary>
        /// Перо для отрисовки слоев на полотне.
        /// </summary>
        Graphics graphics;
        /// <summary>
        /// Полотно выводимое на экран.
        /// </summary>
        Bitmap resultBitmap;

        /// <summary>
        /// Задать\получить изображение полотна на указанном слое.
        /// </summary>
        /// <param name="index">Номер слоя.</param>
        public Image this[int index]
        {
            get
            {
                if(index >= 0 && index < images.Length)
                {
                    return images[index].Image;
                }
                return null;
            }
            set
            {
                if (index >= 0 && index < images.Length)
                {
                    if(value == null)
                    {
                        images[index].Image = null;
                        images[index].Width = 0;
                        images[index].Height = 0;
                        drawing();
                        return;
                    }

                    images[index].Image = value;

                    images[index].Width = value.Width;
                    images[index].Height = value.Height;

                    float mod = (images[index].Width >= images[index].Height) ?
                        Width / images[index].Width :
                        Height / images[index].Height;

                    images[index].Width *= mod;
                    images[index].Height *= mod;

                    drawing();
                }
            }
        }



        public LayersOfImages(ref Panel parentControl)
        {
            parentControl.Controls.Add(this);
            SizeMode = PictureBoxSizeMode.Zoom;
            Height = parentControl.Height - (parentControl.Height / 10);
            Width = Height;
            Location = new Point((parentControl.Width - Width) / 2, (parentControl.Height - Height) / 2);
            BackColor = parentControl.BackColor;

            resultBitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

            graphics = Graphics.FromImage(resultBitmap);
            graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
        }


        /// <summary>
        /// Нанести слои на полотно.
        /// </summary>
        void drawing()
        {
            graphics.Clear(Parent.BackColor);

            for (int i = 0; i < images.Length; i++)
            {
                if (images[i].Image != null)
                {
                    graphics.DrawImage(images[i].Image, 
                        (Width - images[i].Width) / 2, (Height - images[i].Height) / 2, 
                        images[i].Width, images[i].Height);  
                }
            }

            Image = resultBitmap;
        }
    }
}
