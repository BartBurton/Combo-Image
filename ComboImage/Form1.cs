using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComboImage
{
    public partial class Window : Form
    {
        public const int NUMBER_OF_FOLDERS = 10;
        public const int NUMBER_OF_IMAGES = 50;

        const string CONFIG_FILE = "Directories.ini";

        Layer[] Layers = new Layer[NUMBER_OF_FOLDERS]
        {
            new Layer(Color.FromArgb(239, 228, 176)),
            new Layer(Color.FromArgb(208, 239, 116)),
            new Layer(Color.FromArgb(34, 189, 255)),
            new Layer(Color.FromArgb(255, 108, 156)),
            new Layer(Color.FromArgb(255, 166, 106)),
            new Layer(Color.FromArgb(188, 105, 188)),
            new Layer(Color.FromArgb(153, 217, 234)),
            new Layer(Color.FromArgb(71, 143, 143)),
            new Layer(Color.FromArgb(105, 0, 210)),
            new Layer(Color.FromArgb(255, 74, 74)),
        };

        static public LayersOfImages Mixed;

        int earlyActiveIndex = -1;
        int index = 0;
        int ActiveIndex
        {
            get => index;
            set
            {
                if (value > NUMBER_OF_FOLDERS - 1) index = 0;
                else if (value < 0) index = NUMBER_OF_FOLDERS - 1;
                else index = value;
            }
        }

        HelpPanel Reference;
        HelpPanel Specification;
        HelpPanel HotKey;


        public Window()
        {
            InitializeComponent();

            MinimumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, 
                Screen.PrimaryScreen.WorkingArea.Height);
            MaximumSize = MinimumSize;

            panelMainImage.Width = (8 * Width) / 10;
            panelFolderImages.Location = new Point(panelMainImage.Width, 0);
            panelFolderImages.Width = Width - panelMainImage.Width;

            initLevels();
            initStrips();

            Mixed = new LayersOfImages(ref panelMainImage);

            ToReadConfig();

            initHelpPanels();
        }


        /// <summary>
        /// Создать шкалу, показывающую уровни.
        /// </summary>
        void initLevels()
        {
            int h = (panelMainImage.Height / NUMBER_OF_FOLDERS) + 1;
            int w = panelMainImage.Width / 100;

            Layers[NUMBER_OF_FOLDERS - 1].Level.Width = w;
            Layers[NUMBER_OF_FOLDERS - 1].Level.Height = h;
            Layers[NUMBER_OF_FOLDERS - 1].Level.Location = new Point(0, 0);
            panelMainImage.Controls.Add(Layers[NUMBER_OF_FOLDERS - 1].Level);

            for (int i = NUMBER_OF_FOLDERS - 2; i >= 0; i--)
            {
                Layers[i].Level.Width = w;
                Layers[i].Level.Height = h;
                Layers[i].Level.Location =
                    new Point(0, Layers[i + 1].Level.Location.Y + Layers[i + 1].Level.Height);
                panelMainImage.Controls.Add(Layers[i].Level);
            }
        }

        /// <summary>
        /// Создать главное меню.
        /// </summary>
        void initStrips()
        {
            for (int i = 0; i < NUMBER_OF_FOLDERS; i++)
            {
                Layers[i].Strip.Name = $"{i}";
                Layers[i].Strip.AutoSize = true;
                Layers[i].Strip.Text = "Add Folder";
                Layers[i].Strip.MouseUp += new MouseEventHandler(ActionMenu);
                menuFolders.Items.Add(Layers[i].Strip);
            }
        }


        void initHelpPanels()
        {
            Reference = new HelpPanel(ref panelMainImage);
            Reference.Location = new Point(panelFolderImages.Location.X - Reference.Width, 0);
            Reference.Anchor = AnchorStyles.Right;
            Reference.BackColor = Color.FromArgb(208, 239, 116);
            Reference.Icon = Image.FromFile(@"C:\Users\MAXXXYMIRON\Desktop\ComboImage\ComboImage\Icons\reference.png");
            Reference.HelpText = 
                "Программа ComboImage\n" +
                "Разработчик - Никитин Максим.\n" +
                "Версия - 1.0 (04.09.2020).\n" +
                "Все права защищены.";


            Specification = new HelpPanel(ref panelMainImage);
            Specification.Location = new Point(panelFolderImages.Location.X - Specification.Width, Reference.Location.Y + Reference.Height);
            Specification.Anchor = AnchorStyles.Right;
            Specification.BackColor = Color.FromArgb(105, 0, 210);
            Specification.Icon = Image.FromFile(@"C:\Users\MAXXXYMIRON\Desktop\ComboImage\ComboImage\Icons\specification.png");
            Specification.HelpText = 
                "Программа предназначена для слоевого наложения изображений на единый холст.\n" +
                "Данная функциональность реализованна для бастрой проверки сочетаемости\n" +
                "новых спрайтов со старыми наработками.\n" +
                "Программа автоматически сохраняет свое состояние перед закрытием,\n" +
                "в частости сохраняются пути к папкам с изображениями, выбранные в данных папках\n" +
                "изображения и изображение на холсте.\n" +
                "Разноцветная шкала слева обозначает, на каком уровне в текущий момент происходит работа,\n" +
                "слои находящиеся ниже текущего будут отрисовываться под ним, те, что выше над ним.\n" +
                "Выбор папки происходит нажатием ЛКМ на один из пунктов главного меню, если пункт пустой,\n" +
                "нужно будет выбрать папку. Выбранную папку можно исключить СКМ или изменить ПКМ.\n" +
                "Исключать или изменять можно только выбранную в текущий момент папку.\n" +
                "Изображение выбирается нажатием ЛКМ и стирается с холста ПКМ.";


            HotKey = new HelpPanel(ref panelMainImage);
            HotKey.Location = new Point(panelFolderImages.Location.X - HotKey.Width, Specification.Location.Y + Specification.Height);
            HotKey.Anchor = AnchorStyles.Right;
            HotKey.BackColor = Color.FromArgb(255, 74, 74);
            HotKey.Icon = Image.FromFile(@"C:\Users\MAXXXYMIRON\Desktop\ComboImage\ComboImage\Icons\hot_key.png");
            HotKey.HelpText = 
                "ГОРЯЧИЕ КЛАВИШИ\n" +
                "\"A\" - Переместиться в папку слева.\n" +
                "\"D\" - Переместиться в папку справа.\n" +
                "\"Q\" - Исключить папку.\n" +
                "\"E\" - Изменить папку.\n" +
                "\"W\" - Выбрать изображение выше.\n" +
                "\"S\" - Выбрать изображение ниже.\n" +
                "\"Space\" - Стереть изображение с холста.\n";
        }

        void ToReadConfig()
        {
            using (StreamReader sr = new StreamReader(new FileStream(CONFIG_FILE, FileMode.OpenOrCreate)))
            {
                if (sr.BaseStream.Length > 0)
                {
                    for (int i = 0; i < Layers.Length; i++)
                    {
                        Layers[i].Init(sr.ReadLine());
                        Layers[i].GetPictures(ref panelFolderImages);
                        Layers[i].ChoosedPicture = Convert.ToInt32(sr.ReadLine());
                        if(Layers[i].ChoosedPicture != -1 && Layers[i].CountOfPictures != 0)
                        {
                            Layers[i].PictureActive(panelFolderImages.Controls[Layers[i].ChoosedPicture]);
                        }
                    }

                    for (int i = Layers.Length- 1; i >= 0; i--)
                    {
                        if (Layers[i].CountOfPictures != 0)
                        {
                            ActionMenu(Layers[i].Strip, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
                            break;
                        }
                    }
                }
            }
        }

        void ToWriteConfig()
        {
            using (StreamWriter sw = new StreamWriter(new FileStream(CONFIG_FILE, FileMode.Open)))
            {
                for (int i = 0; i < Layers.Length; i++)
                {
                    sw.WriteLine(Layers[i].Folder);
                    sw.WriteLine(Layers[i].ChoosedPicture);
                }
            }
        }


        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D)
            {
                ActiveIndex++;
                ActionMenu(Layers[ActiveIndex].Strip, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
            }
            else if (e.KeyCode == Keys.A)
            {
                ActiveIndex--;
                ActionMenu(Layers[ActiveIndex].Strip, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
            }
            else if (e.KeyCode == Keys.Q)
            {
                ActionMenu(Layers[ActiveIndex].Strip, new MouseEventArgs(MouseButtons.Middle, 1, 0, 0, 0));
            }
            else if (e.KeyCode == Keys.E)
            {
                ActionMenu(Layers[ActiveIndex].Strip, new MouseEventArgs(MouseButtons.Right, 1, 0, 0, 0));
            }
            else if (e.KeyCode == Keys.W)
            {
                Layers[ActiveIndex].ChoosedPicture--;
                Layers[ActiveIndex].PictureActive(panelFolderImages.Controls[Layers[ActiveIndex].ChoosedPicture]);
            }
            else if (e.KeyCode == Keys.S)
            {
                Layers[ActiveIndex].ChoosedPicture++;
                Layers[ActiveIndex].PictureActive(panelFolderImages.Controls[Layers[ActiveIndex].ChoosedPicture]);
            }
            else if (e.KeyCode == Keys.Space)
            {
                Layers[ActiveIndex].PictureDeactive(panelFolderImages.Controls[Layers[ActiveIndex].ChoosedPicture]);
            }
        }

        private void ActionMenu(object sender, MouseEventArgs e)
        {
            ToolStripItem item = sender as ToolStripItem;
            int tempIndex = Convert.ToInt32(item.Name);

            if (e.Button == MouseButtons.Left)
            {
                ActiveIndex = tempIndex;

                if (!Layers[tempIndex].IsInit)
                {
                    folderBrowser.ShowDialog();
                    Layers[ActiveIndex].Folder = folderBrowser.SelectedPath;
                    if (Layers[ActiveIndex].Folder == "") return;

                    Layers[ActiveIndex].Init(Layers[ActiveIndex].Folder);
                }

                if (!Layers[ActiveIndex].IsActive)
                {
                    Layers[ActiveIndex].Active();
                    Layers[ActiveIndex].GetPictures(ref panelFolderImages);
                    if (earlyActiveIndex != -1)
                    {
                        Layers[earlyActiveIndex].Deactive();
                    }
                    earlyActiveIndex = ActiveIndex;
                }
            }


            else if (e.Button == MouseButtons.Right)
            {
                if (Layers[tempIndex].IsActive)
                {
                    Layers[ActiveIndex].Clear();

                    folderBrowser.ShowDialog();
                    Layers[ActiveIndex].Folder = folderBrowser.SelectedPath;
                    if (Layers[ActiveIndex].Folder == "") return;

                    Layers[ActiveIndex].Init(Layers[ActiveIndex].Folder);
                    Layers[ActiveIndex].GetPictures(ref panelFolderImages);
                }
            }


            else if (e.Button == MouseButtons.Middle)
            {
                if (Layers[tempIndex].IsActive)
                {
                    Layers[ActiveIndex].Deactive();
                    Layers[ActiveIndex].Clear();
                    panelFolderImages.Controls.Clear();

                    earlyActiveIndex = -1;
                }
            }
        }

        private void Window_FormClosed(object sender, FormClosedEventArgs e)
        {
            ToWriteConfig();
        }
    }
}
