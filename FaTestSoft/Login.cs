using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;              //使用线程
using System.IO;                       //读写文件所在的命名空间
using FaTestSoft.CommonData;

namespace FaTestSoft
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        string struser           = @"";
        string strpassword       = @"";
        private byte OpenYesOrOn = 0;
        //static bool InitState    = false;

        private void buttonOk_Click(object sender, EventArgs e)
        {
            byte i = 0;
            if (comboBoxusename.Text == "")
            {
                MessageBox.Show("用户名为空！", "提示",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            if (textBox1password.Text == "")
            {
                MessageBox.Show("密码为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (OpenYesOrOn == 2)   //系统文件存在
            {
                for (i = 0; i < PublicDataClass._SaveRoleInfo.num; i++)
                {
                    if ( (comboBoxusename.Text == PublicDataClass._SaveRoleInfo.RoleInfo[i].user )&&

                        (textBox1password.Text == PublicDataClass._SaveRoleInfo.RoleInfo[i].Password))
                    {
                        PublicDataClass.Manger = PublicDataClass._SaveRoleInfo.RoleInfo[i].user;
                        ////////////用户类型///////////////////
                        if (PublicDataClass.Manger != "admin")
                            PublicDataClass.LoginType = 1;
                        ////////////////////////////////////////
                        break;
                    }
                }
                if (i < PublicDataClass._SaveRoleInfo.num)
                {
                    PublicDataClass._ChangeFlag.InitState = true;//好像没用到？
                    if (PublicDataClass._ChangeFlag.XtParamUpdate1 == true)
                    {
                        //this.Hide();
                        this.Visible = false;
                        PublicDataClass._ChangeFlag.XtParamUpdate = true;
                        PublicDataClass._ChangeFlag.Setflag = true;
                    }
                    else if (PublicDataClass._ChangeFlag.AcquisitionParamUpdate1 == true)
                    {
                        //this.Hide();
                        this.Visible = false;
                        PublicDataClass._ChangeFlag.AcquisitionParamUpdate = true;
                        PublicDataClass._ChangeFlag.Setflag = true;
                    }
                    else if (PublicDataClass._ChangeFlag.ControlViewUpdate1 == true)
                    {
                        //this.Hide();
                        this.Visible = false;
                        PublicDataClass._ChangeFlag.ControlViewUpdate = true;
                        PublicDataClass._ChangeFlag.Setflag = true;
                    }
                    else if (PublicDataClass._ChangeFlag.OtherTypeViewUpdate1 == true)
                    {
                        //this.Hide();
                        this.Visible = false;
                        PublicDataClass._ChangeFlag.OtherTypeViewUpdate = true;
                        PublicDataClass._ChangeFlag.Setflag = true;
                    }
                    else
                    {
                        //this.Hide();
                        this.Visible = false;
                        Form1 fm1 = new Form1();
                        fm1.Show();
                    }
                }
                else
                {
                    MessageBox.Show("密码输入错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            else if (OpenYesOrOn == 1)
            {
                if ((comboBoxusename.Text == struser) && (textBox1password.Text == strpassword))
                {
                    if (PublicDataClass._ChangeFlag.XtParamUpdate1 == true)
                    {
                        this.Close();
                        //this.Visible = false;
                        PublicDataClass._ChangeFlag.XtParamUpdate = true;
                        PublicDataClass._ChangeFlag.Setflag = true;
                       
                    }
                     else if (PublicDataClass._ChangeFlag.AcquisitionParamUpdate1 == true)
                    {
                        this.Close();
                        //this.Visible = false;
                        PublicDataClass._ChangeFlag.AcquisitionParamUpdate = true;
                        PublicDataClass._ChangeFlag.Setflag = true;
                        
                    }
                    else if (PublicDataClass._ChangeFlag.ControlViewUpdate1 == true)
                    {
                        this.Close();
                        //this.Visible = false;
                        PublicDataClass._ChangeFlag.ControlViewUpdate = true;
                        PublicDataClass._ChangeFlag.Setflag = true;
                        
                    }
                     else if (PublicDataClass._ChangeFlag.OtherTypeViewUpdate1 == true)
                    {
                        this.Close();
                        //this.Visible = false;
                        PublicDataClass._ChangeFlag.OtherTypeViewUpdate = true;
                        PublicDataClass._ChangeFlag.Setflag = true;
                       
                    }
                    else
                    {
                        PublicDataClass.Manger = struser;
                        this.Close();
                        //this.Visible = false;
                        Form1 fm1 = new Form1();
                        fm1.Show();
                    }
                }
                else
                    MessageBox.Show("密码或用户名输入错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
           
          
        }

        private void textBox1password_Validating(object sender, CancelEventArgs e)
        {
            /*if (textBox1password.Text != "123456")//判断输入是否正确
            {
                errorPassword.SetError(textBox1password, "密码错误"); //在红色旁边提示
            }
            else
            {
                errorPassword.SetError(textBox1password, "");
                //strB = txtPasword.Text;
            }*/
        }

        private void Login_Load(object sender, EventArgs e)
        {
            string FileName = @"";
            string strline = @"";
            FileStream afile;
            //if(PublicDataClass._ChangeFlag.InitState==false)
            //    FileName = System.Environment.CurrentDirectory + "\\新建工程" + "\\rolecfg.sys";
            //else
            FileName = System.Environment.CurrentDirectory + "\\User.txt";

            try
            {
                afile = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                OpenYesOrOn = 1;
                struser = "admin";
                strpassword = "123456";
                return;
            }
            StreamReader sr =new StreamReader(afile);
            strline = sr.ReadLine();
            if (strline == null)
            {
                OpenYesOrOn = 1;
                struser = "admin";
                strpassword = "123456";
                sr.Close();
                return;
            }
            PublicDataClass._SaveRoleInfo.num =Convert.ToByte(strline);
            PublicDataClass._SaveRoleInfo.RoleInfo =new PublicDataClass._ROLEMANEGER[PublicDataClass._SaveRoleInfo.num];
            for (byte i = 0; i < PublicDataClass._SaveRoleInfo.num; i++)
            {
                    strline = sr.ReadLine();
                    PublicDataClass._SaveRoleInfo.RoleInfo[i].user    = Convert.ToString(strline);
                    strline = sr.ReadLine();
                    PublicDataClass._SaveRoleInfo.RoleInfo[i].Password = Convert.ToString(strline);
                    strline = sr.ReadLine();
                    PublicDataClass._SaveRoleInfo.RoleInfo[i].state = Convert.ToString(strline);

            }
            for (byte i = 0; i < PublicDataClass._SaveRoleInfo.num; i++)
            {
                comboBoxusename.Items.Add(PublicDataClass._SaveRoleInfo.RoleInfo[i].user);
            }
            comboBoxusename.Text = PublicDataClass._SaveRoleInfo.RoleInfo[0].user;
            
            OpenYesOrOn = 2;
            sr.Close();
            if (PublicDataClass._SaveRoleInfo.RoleInfo[0].state == "1")
            {

                textBox1password.Text = PublicDataClass._SaveRoleInfo.RoleInfo[0].Password;
                checkBox1.Checked = true;
                
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true )
            {

                if (comboBoxusename.Text == "")
                {
                    MessageBox.Show("用户名为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    checkBox1.Checked = false;
                    return;

                }
                if (textBox1password.Text == "")
                {
                    MessageBox.Show("密码为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    checkBox1.Checked = false;
                    return;
                }

                //PublicDataClass._SaveRoleInfo.num = 1;
                //PublicDataClass._SaveRoleInfo.RoleInfo = new PublicDataClass._ROLEMANEGER[PublicDataClass._SaveRoleInfo.num];


                //PublicDataClass._SaveRoleInfo.RoleInfo[0].user = comboBoxusename.Text;
                //PublicDataClass._SaveRoleInfo.RoleInfo[0].Password = textBox1password.Text;
                //PublicDataClass._SaveRoleInfo.RoleInfo[0].state = @"1";



                string FileName = @"";
                FileStream afile;

                //if (PublicDataClass._ChangeFlag.InitState == false)
                //    FileName = System.Environment.CurrentDirectory + "\\新建工程" + "\\rolecfg.sys";
                //else
                //    FileName = System.Environment.CurrentDirectory + "\\rolecfg.sys";
                FileName = System.Environment.CurrentDirectory + "\\User.txt";
            

                afile = new FileStream(FileName, FileMode.Create);


                StreamWriter sw = new StreamWriter(afile);


                sw.WriteLine(PublicDataClass._SaveRoleInfo.num);

                for (byte i = 0; i < PublicDataClass._SaveRoleInfo.num; i++)
                {
                    sw.WriteLine(PublicDataClass._SaveRoleInfo.RoleInfo[i].user);
                    sw.WriteLine(PublicDataClass._SaveRoleInfo.RoleInfo[i].Password);
                    PublicDataClass._SaveRoleInfo.RoleInfo[i].state = @"1";
                    sw.WriteLine(PublicDataClass._SaveRoleInfo.RoleInfo[i].state);
                }
                sw.Close();

            }
            else if (checkBox1.Checked == false )
            {

                if (comboBoxusename.Text == "")
                {
                    MessageBox.Show("用户名为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    checkBox1.Checked = false;
                    return;

                }
                if (textBox1password.Text == "")
                {
                    MessageBox.Show("密码为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    checkBox1.Checked = false;
                    return;
                }

                //PublicDataClass._SaveRoleInfo.num = 1;
                //PublicDataClass._SaveRoleInfo.RoleInfo = new PublicDataClass._ROLEMANEGER[PublicDataClass._SaveRoleInfo.num];


                //PublicDataClass._SaveRoleInfo.RoleInfo[0].user = comboBoxusename.Text;
                //PublicDataClass._SaveRoleInfo.RoleInfo[0].Password = textBox1password.Text;
                //PublicDataClass._SaveRoleInfo.RoleInfo[0].state = @"1";



                string FileName = @"";
                FileStream afile;

                //if (PublicDataClass._ChangeFlag.InitState == false)
                //    FileName = System.Environment.CurrentDirectory + "\\新建工程" + "\\rolecfg.sys";
                //else
                //    FileName = System.Environment.CurrentDirectory + "\\rolecfg.sys";
                FileName = System.Environment.CurrentDirectory + "\\User.txt";


                afile = new FileStream(FileName, FileMode.Create);


                StreamWriter sw = new StreamWriter(afile);


                sw.WriteLine(PublicDataClass._SaveRoleInfo.num);

                for (byte i = 0; i < PublicDataClass._SaveRoleInfo.num; i++)
                {
                    sw.WriteLine(PublicDataClass._SaveRoleInfo.RoleInfo[i].user);
                    sw.WriteLine(PublicDataClass._SaveRoleInfo.RoleInfo[i].Password);
                    PublicDataClass._SaveRoleInfo.RoleInfo[i].state = @"0";
                    sw.WriteLine(PublicDataClass._SaveRoleInfo.RoleInfo[i].state);
                }
                sw.Close();

            }
            
        }

     

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      
    }
}
