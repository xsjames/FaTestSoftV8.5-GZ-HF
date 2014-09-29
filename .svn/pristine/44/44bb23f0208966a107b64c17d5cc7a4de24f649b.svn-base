using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FaTestSoft.CommonData;

namespace FaTestSoft
{
    public partial class AddRoleForm : Form
    {
        public AddRoleForm()
        {
            InitializeComponent();
        }

        private void buttonok_Click(object sender, EventArgs e)
        {
            if (textBoxuser.Text == "")
            {
                MessageBox.Show("用户名为空！", "提示",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBoxpassword1.Text == "" || textBoxpassword2.Text =="")
            {
                MessageBox.Show("密码为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBoxpassword1.Text != textBoxpassword2.Text)
            {
                MessageBox.Show("两次密码输入不对！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            PublicDataClass._AddRoleRecord.User = textBoxuser.Text;
            PublicDataClass._AddRoleRecord.password = textBoxpassword1.Text;

            this.DialogResult = DialogResult.OK;
        }
    }
}
