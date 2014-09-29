using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using FaTestSoft.CommonData;              //使用新增加的类所在的命名空间

namespace FaTestSoft
{
    public partial class CreateNewPrjForm : Form
    {
        public CreateNewPrjForm()
        {
            InitializeComponent();
        }

        private void CreateNewPrjForm_Load(object sender, EventArgs e)
        {
            textBoxPrjPath.Enabled = false;
            textBoxPrjPath.Text = System.Environment.CurrentDirectory;
            textBoxPrjName.Text = "新建工程";
            PublicDataClass.PrjName = "";   
        }

        private void button_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = "";
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                textBoxPrjPath.Text = folderBrowserDialog1.SelectedPath;
        }

        private void Okbutton_Click(object sender, EventArgs e)
        {
            textBoxPrjPath.Text += "\\";
            if (Directory.Exists(textBoxPrjPath.Text + textBoxPrjName.Text) == true)  //文件夹存在
            {
                MessageBox.Show("文件夹工程以存在,重新更名！", "提示",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            PublicDataClass.PrjPath = textBoxPrjPath.Text + textBoxPrjName.Text;
            PublicDataClass.PrjName = textBoxPrjName.Text;
            Directory.CreateDirectory(textBoxPrjPath.Text + textBoxPrjName.Text);    //建立文件夹
            this.DialogResult = DialogResult.OK;                                     //当文件夹不存在时 将按钮的DialogResult 的属性设置为OK
        }
    }
}
