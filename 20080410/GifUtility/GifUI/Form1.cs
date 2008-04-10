using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Jillzhang.GifUtility;
using System.IO;
using System.Threading;

namespace GifUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            folderBrowserDialog1.SelectedPath = AppDomain.CurrentDomain.BaseDirectory; ;
        }
        string outGifPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "monochrome.gif");
        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {                      
            GifHelper.Monochrome(pictureBox1.ImageLocation, outGifPath); 
        }

        private void 浏览ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                pictureBox1.ImageLocation = openFileDialog1.FileName;            
            }
        }

        private void 单色化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.RunWorkerAsync();
            toolStripStatusLabel1.Text = "正在对图像进行单色化......";      
        }

        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripStatusLabel1.Text = "完成";
            pictureBox2.ImageLocation = outGifPath;          
        }

        private void 缩略ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker2.DoWork += new DoWorkEventHandler(backgroundWorker2_DoWork);
            backgroundWorker2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker2_RunWorkerCompleted);
            backgroundWorker2.RunWorkerAsync();
            toolStripStatusLabel1.Text = "正在对图像缩略......";      
        }

        string thGif = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "thGif.gif");
        void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripStatusLabel1.Text = "完成";
            pictureBox2.ImageLocation = thGif;         
        }

        void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {           
            GifHelper.GetThumbnail(pictureBox1.ImageLocation, 0.5, thGif);
              
        }
        string dir;
        private void 合并ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = folderBrowserDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                dir = folderBrowserDialog1.SelectedPath;
                outGifPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "merge.gif");
                BackgroundWorker bg = new BackgroundWorker();
                bg.DoWork += new DoWorkEventHandler(bg_DoWork);
                bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);
                toolStripStatusLabel1.Text = "正在对图像进行合并......";
                bg.RunWorkerAsync();
            }
       
        }

        void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripStatusLabel1.Text = "完成";
            pictureBox1.ImageLocation = outGifPath;       
        }

        void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            List<string> sources = GetGifFiles(dir);          
            GifHelper.Merge(sources, outGifPath);
        }

        List<string> GetGifFiles(string dir)
        {
            List<string> list = new List<string>();
            DirectoryInfo di = new DirectoryInfo(dir);
            foreach (FileInfo fi in di.GetFiles("*.gif"))
            {
                list.Add(fi.FullName);
            }
            return list;
        }

        private void 向左旋转ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GifHelper.Rotate(pictureBox1.ImageLocation, RotateFlipType.Rotate90FlipXY, outGifPath);
            pictureBox2.ImageLocation = outGifPath;         
        }

        private void 向右旋转ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GifHelper.Rotate(pictureBox1.ImageLocation, RotateFlipType.Rotate270FlipXY, outGifPath);
            pictureBox2.ImageLocation = outGifPath;         
        }

        private void 水平翻转ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GifHelper.Rotate(pictureBox1.ImageLocation, RotateFlipType.RotateNoneFlipX, outGifPath);
            pictureBox2.ImageLocation = outGifPath;         
        }

        private void 垂直翻转ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GifHelper.Rotate(pictureBox1.ImageLocation, RotateFlipType.RotateNoneFlipY, outGifPath);
            pictureBox2.ImageLocation = outGifPath;         
        }
    }
}