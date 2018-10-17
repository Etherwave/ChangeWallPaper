using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChangeWallPaper
{
    public partial class Form1 : Form
    {
        public int refresh_seconds = 3*1000;
        public string filePath = "C:\\Users\\Amazing\\Desktop\\20171127_IMG_0003.JPG";
        public string[] images;
        public int cnt = 0;
        public bool working = false;

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(
                int uAction,
                int uParam,
                string lpvParam,
                int fuWinIni
                );

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 判断字符串是否是数字
        /// </summary>
        public static bool IsNumber(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            const string pattern = "^[0-9]*$";
            Regex rx = new Regex(pattern);
            return rx.IsMatch(s);
        }

        public void update_location()
        {
            label1.Left = this.Width / 2 - label1.Width / 2;
            textBox1.Left = this.Width / 2 - textBox1.Width / 2;
            button1.Left = this.Width / 2 - button1.Width / 2;
            label_seconds.Left = this.Width / 2 - label_seconds.Width / 2;
            textBox_seconds.Left = label_seconds.Left- textBox_seconds.Width;
            button_refresh.Left = label_seconds.Left + label_seconds.Width;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            update_location();
        }

        //利用系统的用户接口设置壁纸
        
        public static void setWallpaperApi(string strSavePath)
        {
           SystemParametersInfo(20, 1, strSavePath, 1);
        }

        private void button_refresh_Click(object sender, EventArgs e)
        {
            refresh_seconds = 3;
            if (IsNumber(textBox_seconds.Text))
            {
                refresh_seconds = Convert.ToInt32(textBox_seconds.Text);
            }
            else
            {
                textBox_seconds.Text = "3";
            }

            if(textBox1.Text!="")
            {
                filePath = textBox1.Text;
            }
            else
            {
                filePath= "C:\\Users\\Amazing\\Desktop\\20171127_IMG_0003.JPG";
            }

            timer2.Interval = refresh_seconds * 1000;

            get_images_path();

            working = true;
        }

        public void get_images_path()
        {
            var a = Directory.GetFiles(filePath, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".bmp") || s.EndsWith(".jpg"));
            images = a.ToArray();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if(working)
            {
                setWallpaperApi(images[cnt++]);
                cnt %= images.Length;
            }
            else
            {
                setWallpaperApi(filePath);
            }
        }
    }
}
