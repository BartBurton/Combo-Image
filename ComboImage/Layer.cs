using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace ComboImage
{
    class Layer
    {
        /// <summary>
        /// Цвет обозначения данного слоя.
        /// </summary>
        public Color Color;
        /// <summary>
        /// Ячейка на шкале слоев.
        /// </summary>
        public Panel Level { get; set; } = new Panel();
        /// <summary>
        /// Путь к папки, из которой зогружены картинки для данного слоя.
        /// </summary>
        public string Folder { get; set; } = "";
        /// <summary>
        /// Кнопка этого слоя на главном меню.
        /// </summary>
        public ToolStripItem Strip { get; set; } = new ToolStripMenuItem();

        /// <summary>
        /// Обозначение того, что слой уже имеет папку, и ее название на кнопке в главном меню.
        /// </summary>
        public bool IsInit = false;
        /// <summary>
        /// Обозначение того что работа в данный момент ведется с текущим слоем.
        /// </summary>
        public bool IsActive = false;

        /// <summary>
        /// Предыдущая выбранная картинка.
        /// </summary>
        int earlyChoosedPicture = -1;
        /// <summary>
        /// Выбранная картинка.
        /// </summary>
        int choosedPicture = -1;
        /// <summary>
        /// Кол-во загруженных из папки картинок.
        /// </summary>
        public int CountOfPictures { get; private set; } = 0;
        /// <summary>
        /// Выбранная картинка.
        /// </summary>
        public int ChoosedPicture
        {
            get => choosedPicture;
            set
            {
                if (value < 0) choosedPicture = CountOfPictures - 1;
                else if (value > CountOfPictures - 1) choosedPicture = 0;
                else choosedPicture = value;
            }
        }

        /// <summary>
        /// Поправка при расположении картинок на панели.
        /// </summary>
        const int COR = 10;



        public Layer(Color color)
        {
            Color = color;
            Level.BackColor = Color;
        }



        /// <summary>
        /// Метод инициализации слоя.
        /// </summary>
        /// <param name="folderName">Папка с картинками.</param>
        public void Init(string folderName)
        {
            if (folderName != "")
            {
                IsInit = true;

                Folder = folderName;
                Strip.Text = Folder.Substring(Folder.LastIndexOf('\\')); 
            }
        }

        /// <summary>
        /// Активация слоя.
        /// </summary>
        public void Active()
        {
            if (!IsActive)
            {
                Strip.BackColor = Color;
                Level.Width *= 2;
                IsActive = true;
            }
        }

        /// <summary>
        /// Очистка слоя от данных связанных с прошлой папкой.
        /// </summary>
        public void Clear()
        {
            IsInit = false;

            Folder = "";
            Strip.Text = "Add Folder";
            earlyChoosedPicture = -1;
            CountOfPictures = 0;
            ChoosedPicture = -1;

            Window.Mixed[Convert.ToInt32(Strip.Name)] = null;
        }

        /// <summary>
        /// Деактивация слоя.
        /// </summary>
        public void Deactive()
        {
            if (IsActive)
            {
                Strip.BackColor = Strip.Owner.BackColor;
                Level.Width /= 2;
                IsActive = false;
            }
        }


        /// <summary>
        /// Получить картинки, т.е. вывести их на необходимую панель.
        /// </summary>
        /// <param name="control"></param>
        public void GetPictures(ref Panel control)
        {
            if (Folder == "") return;

            control.AutoScroll = true;
            control.AutoScrollMargin = new Size(0, 2 * COR);
            control.Controls.Clear();

            int numberOfPictures = 0;

            try
            {
                foreach (string file in Directory.GetFiles(Folder))
                {
                    if (file.Contains(".png"))
                    {
                        PictureWithBorders picture = new PictureWithBorders(file, control.Width, ref Color);
                        picture.Active += PictureActive;
                        picture.Deactive += PictureDeactive;
                        control.Controls.Add(picture);

                        numberOfPictures++;
                        if (numberOfPictures > Window.NUMBER_OF_IMAGES)
                        {
                            throw new TooMoreImagesException("В папке слишком много изображений!");
                        }

                        if (control.Controls.Count != 1)
                        {
                            picture.Location =
                                new Point(control.Controls[control.Controls.Count - 2].Location.X,
                                control.Controls[control.Controls.Count - 2].Location.Y +
                                control.Controls[control.Controls.Count - 2].Width + COR);
                        }
                        else
                        {
                            picture.Location =
                                new Point((control.Width - picture.Width) / 3, COR * 2);
                        }
                    }
                }

                CountOfPictures = control.Controls.Count;

                if (ChoosedPicture != -1)
                {
                    control.Controls[ChoosedPicture].BackColor = Color.FromArgb(20, Color);
                    control.ScrollControlIntoView(control.Controls[ChoosedPicture]);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Деактивировать картинку.
        /// </summary>
        /// <param name="sender"></param>
        public void PictureDeactive(object sender)
        {
            if(ChoosedPicture != -1)
            {
                if (sender is PictureWithBorders pwb)
                {
                    Panel panel = pwb.Parent as Panel;

                    if (panel.Controls.IndexOf(pwb) == ChoosedPicture)
                    {
                        Window.Mixed[Convert.ToInt32(Strip.Name)] = null;
                        panel.Controls[ChoosedPicture].BackColor = panel.BackColor;

                        earlyChoosedPicture = -1;
                    }
                }
            }
        }


        /// <summary>
        /// Активировать картинку.
        /// </summary>
        /// <param name="sender">Объект картинки.</param>
        public void PictureActive(object sender)
        {
            if (sender is PictureWithBorders pwb)
            {
                Panel panel = pwb.Parent as Panel;
                ChoosedPicture = panel.Controls.GetChildIndex(pwb);

                if (ChoosedPicture != earlyChoosedPicture)
                {
                    panel.ScrollControlIntoView(pwb);

                    panel.Controls[ChoosedPicture].BackColor = Color.FromArgb(20, Color);

                    Window.Mixed[Convert.ToInt32(Strip.Name)] = pwb.Picture.Image;

                    if (earlyChoosedPicture != -1)
                    {
                        panel.Controls[earlyChoosedPicture].BackColor = panel.BackColor;
                    }
                    earlyChoosedPicture = ChoosedPicture;
                }
            }
        }
    }


    [Serializable]
    public class TooMoreImagesException : Exception
    {
        public TooMoreImagesException() { }
        public TooMoreImagesException(string message) : base(message) { }
        public TooMoreImagesException(string message, Exception inner) : base(message, inner) { }
        protected TooMoreImagesException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

