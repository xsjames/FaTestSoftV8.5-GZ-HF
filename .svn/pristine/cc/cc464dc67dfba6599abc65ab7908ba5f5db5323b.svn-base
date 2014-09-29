using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;              //使用线程

namespace FaTestSoft
{
    public partial class Origination : Form
    {
        public Origination()
        {
            InitializeComponent();
        }

        private void Origination_Load(object sender, EventArgs e)
        {
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            progressBar1.Minimum = 0;               //设定ProgressBar控件的最小值为0
            progressBar1.Maximum = 10;              //设定ProgressBar控件的最大值为10
            progressBar1.MarqueeAnimationSpeed = 100; //设定ProgressBar控件进度块在进度栏中移动的时间段
            timer1.Start();                       //启动计时器
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //this.Hide();                           //隐藏本窗体
            //Login MainForm = new Login();  //实例化一个MainForm对象
            //MainForm.Show(); 
            //显示窗体MainForm
            this.Hide(); 
            Form1 fm = new Form1();
            fm.Show();
            
           // Thread t = new Thread(new ThreadStart(delegate { Application.Run(new Form1()); }));
           // t.Start();
            
            timer1.Stop();     
        }

        Point pt;
        private void Origination_MouseDown(object sender, MouseEventArgs e)
        {
            pt = Cursor.Position;
        }

        private void Origination_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int px = Cursor.Position.X - pt.X;
                int py = Cursor.Position.Y - pt.Y;
                this.Location = new Point(this.Location.X + px, this.Location.Y + py);

                pt = Cursor.Position;
            }
           
        }
    }
}
