namespace ComboImage
{
    partial class Window
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Window));
            this.panelMainImage = new System.Windows.Forms.Panel();
            this.panelFolderImages = new System.Windows.Forms.Panel();
            this.menuFolders = new System.Windows.Forms.MenuStrip();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // panelMainImage
            // 
            this.panelMainImage.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelMainImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.panelMainImage.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMainImage.Location = new System.Drawing.Point(0, 24);
            this.panelMainImage.Name = "panelMainImage";
            this.panelMainImage.Size = new System.Drawing.Size(668, 452);
            this.panelMainImage.TabIndex = 0;
            // 
            // panelFolderImages
            // 
            this.panelFolderImages.AutoScroll = true;
            this.panelFolderImages.AutoScrollMargin = new System.Drawing.Size(0, 20);
            this.panelFolderImages.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelFolderImages.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.panelFolderImages.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelFolderImages.Location = new System.Drawing.Point(682, 24);
            this.panelFolderImages.Name = "panelFolderImages";
            this.panelFolderImages.Size = new System.Drawing.Size(200, 452);
            this.panelFolderImages.TabIndex = 1;
            // 
            // menuFolders
            // 
            this.menuFolders.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.menuFolders.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuFolders.Location = new System.Drawing.Point(0, 0);
            this.menuFolders.Name = "menuFolders";
            this.menuFolders.Size = new System.Drawing.Size(882, 24);
            this.menuFolders.TabIndex = 2;
            this.menuFolders.Text = "menuStrip1";
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 476);
            this.Controls.Add(this.panelFolderImages);
            this.Controls.Add(this.panelMainImage);
            this.Controls.Add(this.menuFolders);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuFolders;
            this.Name = "Window";
            this.Text = "ComboImage";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Window_FormClosed);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Window_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelMainImage;
        private System.Windows.Forms.Panel panelFolderImages;
        private System.Windows.Forms.MenuStrip menuFolders;
        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
    }
}

