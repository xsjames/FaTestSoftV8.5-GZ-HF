using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FaTestSoft.CommonData;
using FaTestSoft.FUNCTIONCLASS;
using ZedGraph;
using System.IO;
using System.Runtime.InteropServices;

namespace FaTestSoft
{
    public partial class DeviceInfoAddr : Form
    {
        public DeviceInfoAddr()
        {
            InitializeComponent();
         
        }
        #region 本程序中用到的API函数声明

        [DllImport("kernel32.DLL")]

        private static extern int GetPrivateProfileString(string section, string key,
                                                          string def, StringBuilder retVal,
                                                          int size, string filePath);
        /*参数说明：section：INI文件中的段落名称；key：INI文件中的关键字；
          def：无法读取时候时候的缺省数值；retVal：读取数值；size：数值的大小；
          filePath：INI文件的完整路径和名称。*/

        [DllImport("kernel32.DLL")]
        private static extern long WritePrivateProfileString(string section, string key,
                                                             string val, string filePath);
        /*参数说明：section：INI文件中的段落；key：INI文件中的关键字；
          val：INI文件中关键字的数值；filePath：INI文件的完整的路径和名称。*/

        #endregion

        public static StringBuilder temp = new StringBuilder(255);       //初始化 一个StringBuilder的类型
        public static string str1;
        public static int inputnum;
        public static string[] name;
        public static string[] address;


        private Control[] Editors;
        public int num = 0;
        public static byte ItemId = 0;
        string addr = @"";
        string str  = @"";

        bool saveret = false;
        bool inputflag = false;
        string savename;

        private void DeviceInfoAddr_Load(object sender, EventArgs e)
        {
            byte i;
            
            Editors = new Control[] {
	                                combo,
									combo,			// for column 1
                                    combo,
                                    combo,
                                    combo,
                                    combo,
									};
                                                                              //隐藏组合框 不可见

            if (PublicDataClass.SaveText.devicenum == 0)
            {
                comboBox1.Text = "无信息";

            }
            else
            {
                for (i = 0; i < PublicDataClass.SaveText.devicenum; i++)
                {
                    comboBox1.Items.Add(PublicDataClass.SaveText.Device[i].PointName);
                }
                comboBox1.Text = PublicDataClass.SaveText.Device[0].PointName;

            }
            num = PublicDataClass.SaveText.devicenum;
           if(PublicDataClass.PrjType == 1)
           {
                PublicDataClass.SaveText.Cfg = new PublicDataClass.INFOADDRCFGINFO[PublicDataClass.SaveText.devicenum];//分配变量
                for (i = 0; i < PublicDataClass.SaveText.devicenum; i++)
                {
                    PublicDataClass.SaveText.Cfg[i].PointName = PublicDataClass.SaveText.Device[i].PointName;  //赋值
                }
                PublicDataClass.SaveText.cfgnum = PublicDataClass.SaveText.devicenum;
            }
            if (PublicDataClass.PrjType == 2)
            {

                for (i = 0; i < PublicDataClass.SaveText.cfgnum; i++)
                {
                    if (comboBox1.Text == PublicDataClass.SaveText.Device[i].PointName)
                    {
                        addr = PublicDataClass.SaveText.Device[i].Addr;
                        break;
                    }
                }

                
                
            }
            tabPage1.Controls.Clear();
            this.listView1.Columns.Clear();
            this.listView1.Columns.Add("序号", 60);
            this.listView1.Columns.Add("地址", 100);
            this.listView1.Columns.Add("名称", 100);
            this.listView1.Columns.Add("信息体", 100);
            this.listView1.Columns.Add("源码", 100);
            this.listView1.Columns.Add("接入标志", 120);
            this.listView1.Columns.Add("取反标志", 120);
            tabPage1.Controls.Add(listView1);
            //tabControl1.SelectedTab = tabPage1;
            ItemId = 1;  //默认的为1
            PublicDataClass.Menu = ItemId;
            CheckCfgState();
        }

        private void Tab_Selecting(object sender, TabControlCancelEventArgs e)
        {
            try
            {
                switch (e.TabPage.Name)
                {
                    case "tabPage1":                //yc
                        ItemId = 1;
                        PublicDataClass.Menu = ItemId;
                        tabPage1.Controls.Clear();   //zxl 0416
                        tabPage1.Controls.Add(listView1);
                        listView1.Items.Clear();
                        this.listView1.Columns.Clear();
                        this.listView1.Columns.Add("序号", 60);
                        this.listView1.Columns.Add("地址", 100);
                        this.listView1.Columns.Add("名称", 100);
                        this.listView1.Columns.Add("信息体", 100);
                        this.listView1.Columns.Add("源码", 100);
                        this.listView1.Columns.Add("接入标志", 120);
               //         this.listView1.Columns.Add("取反标志", 120);

                        listView1.Controls.Add(combo);
                        combo.Visible = false;
                        CheckCfgState();
                        
                        break;

                    case "tabPage2":                //yx
                        ItemId = 2;
                        PublicDataClass.Menu = ItemId;
                        tabPage1.Controls.Clear();  //zxl 0416

                        tabPage2.Controls.Add(listView1);
                        listView1.Items.Clear();
                        this.listView1.Columns.Clear();
                        this.listView1.Columns.Add("序号", 60);
                        this.listView1.Columns.Add("地址", 100);
                        this.listView1.Columns.Add("名称", 100);
                        this.listView1.Columns.Add("信息体",100);
                        this.listView1.Columns.Add("源码",100);
                        this.listView1.Columns.Add("接入标志",120);
                        this.listView1.Columns.Add("取反标志", 120);
         
                        listView1.Controls.Add(combo);
                        combo.Visible = false;
                     
                        CheckCfgState();
                        break;

                    case "tabPage3":                //ym
                        ItemId = 3;
                        PublicDataClass.Menu = ItemId;
                        tabPage1.Controls.Clear();  //zxl 0416
                        listView1.Items.Clear();
                        this.listView1.Columns.Clear();
                        this.listView1.Columns.Add("序号", 60);
                        this.listView1.Columns.Add("地址", 100);
                        this.listView1.Columns.Add("名称", 100);
                        this.listView1.Columns.Add("信息体", 100);
                        this.listView1.Columns.Add("源码", 100);
                        this.listView1.Columns.Add("接入标志", 120);
                       
          
                        tabPage3.Controls.Add(listView1);
                        listView1.Items.Clear();
                        listView1.Controls.Add(combo);
                        combo.Visible = false;
                        CheckCfgState();
                        break;
                    
                    default:
                        break;

                }

            }
            catch
            {


            }
        }

     
        /********************************************************************************************
       *  函数名：    OutPutbutton_Clic                                                            *
       *  功能  ：    导出按钮 处理函数                                                            *
       *  参数  ：    无                                                                           *
       *  返回值：    无                                                                           *
       *  修改日期：  2011-3-10                                                                    *
       *  作者    ：  cuibj                                                                        *
       * ******************************************************************************************/

        private void DeviceInfoAddr_VisibleChanged(object sender, EventArgs e)
        {

            if (num == PublicDataClass.SaveText.devicenum)
                return;
            comboBox1.Items.Clear();
            num = PublicDataClass.SaveText.devicenum;
            if (PublicDataClass.SaveText.devicenum == 0)
            {
                comboBox1.Text = "无信息";

            }
            else
            {
                for (byte i = 0; i < PublicDataClass.SaveText.devicenum; i++)
                {
                    comboBox1.Items.Add(PublicDataClass.SaveText.Device[i].PointName);//添加测量点名称信息
                }
                comboBox1.Text = PublicDataClass.SaveText.Device[0].PointName;

            }
            PublicDataClass.SaveText.Cfg = new PublicDataClass.INFOADDRCFGINFO[PublicDataClass.SaveText.devicenum];//分配变量
            for (byte i = 0; i < PublicDataClass.SaveText.devicenum; i++)
            {
                PublicDataClass.SaveText.Cfg[i].PointName = PublicDataClass.SaveText.Device[i].PointName;  //赋值
            }
            PublicDataClass.SaveText.cfgnum = PublicDataClass.SaveText.devicenum;


        }

        /********************************************************************************************
        *  函数名：    CheckCfgState                                                                *
        *  功能  ：    检查配置情况                                                                 *
        *  参数  ：    无                                                                           *
        *  返回值：    无                                                                           *
        *  修改日期：  2010-11-09                                                                   *
        *  作者    ：  cuibj                                                                        *
        * ******************************************************************************************/
        private void CheckCfgState()
        {
            try
            {

                if (comboBox1.Text == "无信息")
                {
                    MessageBox.Show("无测量点！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                for (byte j = 0; j < PublicDataClass.SaveText.devicenum; j++)
                {
                    if (comboBox1.Text == PublicDataClass.SaveText.Device[j].PointName)
                    {
                        addr = PublicDataClass.SaveText.Device[j].Addr;
                        break;
                    }
                }
                listView1.Items.Clear();

                int ch = 0;
                if (ItemId == 1)
                {
                    int ycadr = 16385;
                    for (int k = 0; k < PublicDataClass._YcConfigParam.num; k++)
                    {
                        for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                        {
                            try
                            {
                                if (Convert.ToInt32(PublicDataClass._YcConfigParam.AddrTable[j]) == ycadr)
                                {
                                    ListViewItem lv = new ListViewItem(String.Format("{0:d}", ch));
                                    lv.SubItems.Add(addr);
                                    lv.SubItems.Add(PublicDataClass._YcConfigParam.NameTable[j]);
                                    lv.SubItems.Add(PublicDataClass._YcConfigParam.AddrTable[j]);
                                    str = "0x" + String.Format("{0:x}", Convert.ToInt16(PublicDataClass._YcConfigParam.AddrTable[j]));
                                    lv.SubItems.Add(str);
                                    lv.SubItems.Add("是");
                                 //   lv.SubItems.Add(PublicDataClass._YcConfigParam.QufanTable[j]);
                                    listView1.Items.Add(lv);
                                    ch++;
                                    ycadr++;
                                    break;
                                }
                            }
                            catch
                            { }
                        }
                    }
                    ReNewCfgState();
                }
                else if (ItemId == 2)
                {
                    int yxadr = 1;
                    for (int k = 0; k < PublicDataClass._YxConfigParam.num; k++)
                    {
                        for (int j = 0; j < PublicDataClass._YxConfigParam.num; j++)
                        {
                            try
                            {
                                if (Convert.ToInt32(PublicDataClass._YxConfigParam.AddrTable[j]) == yxadr)
                                {
                                    ListViewItem lv = new ListViewItem(String.Format("{0:d}", ch));
                                    lv.SubItems.Add(addr);
                                    lv.SubItems.Add(PublicDataClass._YxConfigParam.NameTable[j]);
                                    lv.SubItems.Add(PublicDataClass._YxConfigParam.AddrTable[j]);
                                    str = "0x" + String.Format("{0:x}", Convert.ToInt16(PublicDataClass._YxConfigParam.AddrTable[j]));
                                    lv.SubItems.Add(str);
                                    lv.SubItems.Add("是");
                                    lv.SubItems.Add(PublicDataClass._YxConfigParam.QufanTable[j]);
                                    listView1.Items.Add(lv);
                                    ch++;
                                    yxadr++;
                                    break;
                                }
                            }
                            catch { }
                        }
                    }
                    ReNewCfgState();
                }
                else
                {
                    for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        if (PublicDataClass._YkConfigParam.AddrTable[j] != "65535")
                        {
                        ListViewItem lv = new ListViewItem(String.Format("{0:d}", ch));
                        lv.SubItems.Add(addr);
                        lv.SubItems.Add(PublicDataClass._YkConfigParam.NameTable[j]);
                        lv.SubItems.Add(Convert.ToString(24577 + ch));
                        str = "0x" + String.Format("{0:x}", Convert.ToInt32(24577 + ch));
                        lv.SubItems.Add(str);
                        lv.SubItems.Add("是");
                        listView1.Items.Add(lv);
                        ch++;
                        }
                    }
                    PublicDataClass._YkConfigParam.Ykinputflag = true;
                    ReNewCfgState();

                }
            }
            catch
            {
                ReNewCfgState();
                MessageBox.Show("配置文件有误，请确认！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //int i = 0;
            //for (i = 0; i < PublicDataClass.SaveText.cfgnum; i++)
            //{
            //    if (comboBox1.Text == PublicDataClass.SaveText.Device[i].PointName)
            //    {
            //        break;
            //    }
            //}

            //if (ItemId == 1)        //遥测的情况
            //{

               
            //    //=================================zxl 0416===========================================
            //    for (int j = 0; j < PublicDataClass.SaveText.Cfg[i].YccfgNum; j++)
            //    {
            //        //str = String.Format("{0:d}", j);
            //        ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));
            //        lv.SubItems.Add(PublicDataClass.SaveText.Device[i].Addr);

            //        lv.SubItems.Add(PublicDataClass.SaveText.Cfg[i].YccfgName[j]);
            //        lv.SubItems.Add(PublicDataClass.SaveText.Cfg[i].YccfgAddr[j]);
            //        str = "0x" + String.Format("{0:x}", Convert.ToInt16(PublicDataClass.SaveText.Cfg[i].YccfgAddr[j]));

            //        lv.SubItems.Add(str);
            //        lv.SubItems.Add(PublicDataClass.SaveText.Cfg[i].YccfgState[j]);
            //        lv.SubItems.Add(Convert.ToString(PublicDataClass.SaveText.Cfg[i].Ycdataqf[j]));              
            //        listView1.Items.Add(lv);

            //    }
            //    ReNewCfgState();
            //}
            //else if (ItemId == 2) //遥信的情况
            //{
            //    for (int j = 0; j < PublicDataClass.SaveText.Cfg[i].YxcfgNum; j++)
            //    {
            //        //str = String.Format("{0:d}", j);
            //        ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));
            //        lv.SubItems.Add(PublicDataClass.SaveText.Device[i].Addr);

            //        lv.SubItems.Add(PublicDataClass.SaveText.Cfg[i].YxcfgName[j]);
            //        lv.SubItems.Add(PublicDataClass.SaveText.Cfg[i].YxcfgAddr[j]);
            //        str = "0x" + String.Format("{0:x}", Convert.ToInt16(PublicDataClass.SaveText.Cfg[i].YxcfgAddr[j]));

            //        lv.SubItems.Add(str);
            //        lv.SubItems.Add(PublicDataClass.SaveText.Cfg[i].YxcfgState[j]);
            //        lv.SubItems.Add(PublicDataClass.SaveText.Cfg[i].Yxdataqf[j]);
            //        listView1.Items.Add(lv);

            //    }
            //    ReNewCfgState();
            //}
            //else//遥控的情况
            //{
            //    for (int j = 0; j < PublicDataClass.SaveText.Cfg[i].YkcfgNum; j++)
            //    {
            //        //str = String.Format("{0:d}", j);
            //        ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));
            //        lv.SubItems.Add(PublicDataClass.SaveText.Device[i].Addr);

            //        lv.SubItems.Add(PublicDataClass.SaveText.Cfg[i].YkcfgName[j]);
            //        lv.SubItems.Add(PublicDataClass.SaveText.Cfg[i].YkcfgAddr[j]);
            //        str = "0x" + String.Format("{0:x}", Convert.ToInt16(PublicDataClass.SaveText.Cfg[i].YkcfgAddr[j]));

            //        lv.SubItems.Add(str);
            //        lv.SubItems.Add(PublicDataClass.SaveText.Cfg[i].YkcfgState[j]);
            //        listView1.Items.Add(lv);

            //    }
            //    ReNewCfgState();
            //}
        }
        /********************************************************************************************
        *  函数名：    AddtoolStripMenu_Click                                                       *
        *  功能  ：    添加事件的响应                                                               *
        *  参数  ：    无                                                                           *
        *  返回值：    无                                                                           *
        *  修改日期：  2010-11-09                                                                   *
        *  作者    ：  cuibj                                                                        *
        * ******************************************************************************************/
        private void AddtoolStripMenu_Click(object sender, EventArgs e)
        {

            AddInfoAddrViewForm AddInfofm = new AddInfoAddrViewForm();
            AddInfofm.ShowDialog();
            if (AddInfofm.DialogResult == DialogResult.OK)               //判断是否按下-确定-按钮
            {
                ListViewItem lv = new ListViewItem(String.Format("{0:d}", this.listView1.Items.Count));
                lv.SubItems.Add("1");
                lv.SubItems.Add(PublicDataClass._AddInfoRecord.Name);
                lv.SubItems.Add(PublicDataClass._AddInfoRecord.Pos);
                lv.SubItems.Add("null");
                listView1.Items.Add(lv);
                ReNewCfgState();
            }
            
        }
        /********************************************************************************************
        *  函数名：    DeletetoolStripMenu_Click                                                    *
        *  功能  ：    删除事件的响应                                                               *
        *  参数  ：    无                                                                           *
        *  返回值：    无                                                                           *
        *  修改日期：  2010-11-09                                                                   *
        *  作者    ：  cuibj                                                                        *
        * ******************************************************************************************/
        private void DeletetoolStripMenu_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除此项吗?", "信  息",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
               ListView.SelectedListViewItemCollection SettleOnItem = new ListView.SelectedListViewItemCollection(this.listView1);
                for (byte i = 0; i < SettleOnItem.Count;)
                {

                    listView1.Items.Remove(SettleOnItem[i]);       //删除所选择的项
                }
                for (byte i = 0; i < listView1.Items.Count; i++)
                {
                    listView1.Items[i].SubItems[0].Text = String.Format("{0:d}", i);   //重新调整序号

                }
                ReNewCfgState();
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            /*foreach (ListViewItem item in this.listView1.Items)
            {
                item.ForeColor = SystemColors.WindowText;

            }
            this.listView1.SelectedItems[0].ForeColor = Color.Red;//设置当前选择项为红色
            if (e.X > (listView1.Columns[0].Width + listView1.Columns[1].Width + listView1.Columns[2].Width) && e.X < (listView1.Columns[0].Width + listView1.Columns[1].Width + listView1.Columns[2].Width + listView1.Columns[3].Width))
            {
                tbox.Text = this.listView1.SelectedItems[0].SubItems[3].Text;
                tbox.BackColor = Color.White;
                tbox.Font = this.Font;
                tbox.Multiline = true;
                tbox.Leave += new System.EventHandler(tbox_Leave);
                listView1.AddEmbeddedControl(tbox, 3, this.listView1.SelectedItems[0].Index);
                tbox.MouseLeave += new System.EventHandler(tbox_MouseLeave);
            }*/
            
            
        }
       
        /// <summary>
        /// 自定义的函数 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param funtion="更新配置情况 "></param>
        /// <param anthor="cuibj"></param>
        /// <param Time="11-04-28"></param>
        private void  ReNewCfgState()
        {

            byte i = PublicFunction.FindPointNameCorrelativeIndex(comboBox1.Text);
            if (ItemId == 1)
            {
                PublicDataClass.SaveText.Cfg[i].YccfgNum   = listView1.Items.Count;
                PublicDataClass.SaveText.Cfg[i].YccfgName  = new string[listView1.Items.Count];
                PublicDataClass.SaveText.Cfg[i].YccfgState = new string[listView1.Items.Count];
                PublicDataClass.SaveText.Cfg[i].YccfgAddr  = new string[listView1.Items.Count];
                PublicDataClass.SaveText.Cfg[i].Ycdata = new string[listView1.Items.Count];
             //   PublicDataClass.SaveText.Cfg[i].Ycdataqf = new string[listView1.Items.Count];
               
                PublicDataClass._Curve.listemp = new PointPairList[listView1.Items.Count];
                PublicDataClass._Curve.showcurve = new bool[listView1.Items.Count];
                PublicDataClass._Curve.ycdata = new double[listView1.Items.Count];
                for (int j = 0; j < PublicDataClass.SaveText.Cfg[i].YccfgNum; j++)
                {
                    PublicDataClass._Curve.listemp[j] = new PointPairList();
                }

                for (int j = 0; j < listView1.Items.Count; j++)
                {
                    PublicDataClass.SaveText.Cfg[i].YccfgName[j]  = listView1.Items[j].SubItems[2].Text;//取得listview某行某列的值

                    PublicDataClass.SaveText.Cfg[i].YccfgAddr[j]  = listView1.Items[j].SubItems[3].Text;

                    PublicDataClass.SaveText.Cfg[i].YccfgState[j] = listView1.Items[j].SubItems[5].Text;

              //      PublicDataClass.SaveText.Cfg[i].Ycdataqf[j] = listView1.Items[j].SubItems[6].Text;
                  
                }
            }
            else if (ItemId == 2)
            {
                PublicDataClass.SaveText.Cfg[i].YxcfgNum   = listView1.Items.Count;
                PublicDataClass.SaveText.Cfg[i].YxcfgName  = new string[listView1.Items.Count];
                PublicDataClass.SaveText.Cfg[i].YxcfgState = new string[listView1.Items.Count];
                PublicDataClass.SaveText.Cfg[i].YxcfgAddr  = new string[listView1.Items.Count];
                PublicDataClass.SaveText.Cfg[i].Yxdata = new string[listView1.Items.Count];
                PublicDataClass.SaveText.Cfg[i].Yxdataqf = new string[listView1.Items.Count];


                for (int j = 0; j < listView1.Items.Count; j++)
                {
                    PublicDataClass.SaveText.Cfg[i].YxcfgName[j]  = listView1.Items[j].SubItems[2].Text;//取得listview某行某列的值

                    PublicDataClass.SaveText.Cfg[i].YxcfgAddr[j]  = listView1.Items[j].SubItems[3].Text;

                    PublicDataClass.SaveText.Cfg[i].YxcfgState[j] = listView1.Items[j].SubItems[5].Text;
                    PublicDataClass.SaveText.Cfg[i].Yxdataqf[j] = listView1.Items[j].SubItems[6].Text;
                }
            }
            else
            {
                PublicDataClass.SaveText.Cfg[i].YkcfgNum = listView1.Items.Count;
                PublicDataClass.SaveText.Cfg[i].YkcfgName  = new string[listView1.Items.Count];
                PublicDataClass.SaveText.Cfg[i].YkcfgAddr = new string[listView1.Items.Count];
                PublicDataClass.SaveText.Cfg[i].YkcfgState = new string[listView1.Items.Count];

                for (int j = 0; j < listView1.Items.Count; j++)
                {
                    PublicDataClass.SaveText.Cfg[i].YkcfgName[j]  = listView1.Items[j].SubItems[2].Text;//取得listview某行某列的值
                    PublicDataClass.SaveText.Cfg[i].YkcfgAddr[j]  = listView1.Items[j].SubItems[3].Text;
                    PublicDataClass.SaveText.Cfg[i].YkcfgState[j] = listView1.Items[j].SubItems[5].Text;

                }
            }

        }
        /// <summary>
        /// listview控件的鼠标单击响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            
            
            foreach (ListViewItem item in this.listView1.Items)
            {
                item.ForeColor = SystemColors.WindowText;

            }
           
            this.listView1.SelectedItems[0].ForeColor = Color.Red;//设置当前选择项为红色

        }
       
       
     

        private void Inputbutton_Click_1(object sender, EventArgs e)
        {
            CheckCfgState();
            //try
            //{

            //    if (comboBox1.Text == "无信息")
            //    {
            //        MessageBox.Show("无测量点！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }
            //    for (byte j = 0; j < PublicDataClass.SaveText.devicenum; j++)
            //    {
            //        if (comboBox1.Text == PublicDataClass.SaveText.Device[j].PointName)
            //        {
            //            addr = PublicDataClass.SaveText.Device[j].Addr;
            //            break;
            //        }
            //    }
            //    listView1.Items.Clear();

            //    int ch = 0;
            //    if (ItemId == 1)
            //    {
            //        int ycadr = 16385;
            //        for (int k = 0; k < PublicDataClass._YcConfigParam.num;k++)
            //        {
            //            for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
            //            {
            //                try
            //                {
            //                    if (Convert.ToInt32(PublicDataClass._YcConfigParam.AddrTable[j]) == ycadr)
            //                    {
            //                        ListViewItem lv = new ListViewItem(String.Format("{0:d}", ch));
            //                        lv.SubItems.Add(addr);
            //                        lv.SubItems.Add(PublicDataClass._YcConfigParam.NameTable[j]);
            //                        lv.SubItems.Add(PublicDataClass._YcConfigParam.AddrTable[j]);
            //                        str = "0x" + String.Format("{0:x}", Convert.ToInt16(PublicDataClass._YcConfigParam.AddrTable[j]));
            //                        lv.SubItems.Add(str);
            //                        lv.SubItems.Add("是");
            //                        lv.SubItems.Add(PublicDataClass._YcConfigParam.QufanTable[j]);
            //                        listView1.Items.Add(lv);
            //                        ch++;
            //                        ycadr++;
            //                        break;
            //                    }
            //                }
            //                catch 
            //                { }
            //            }
            //        }                   
            //        ReNewCfgState();
            //    }
            //    else if (ItemId == 2)
            //    {
            //        int yxadr = 1;
            //        for (int k = 0; k < PublicDataClass._YxConfigParam.num; k++)
            //        {
            //            for (int j = 0; j < PublicDataClass._YxConfigParam.num; j++)
            //            {
            //                try
            //                {
            //                    if (Convert.ToInt32(PublicDataClass._YxConfigParam.AddrTable[j]) == yxadr)
            //                    {
            //                        ListViewItem lv = new ListViewItem(String.Format("{0:d}", ch));
            //                        lv.SubItems.Add(addr);
            //                        lv.SubItems.Add(PublicDataClass._YxConfigParam.NameTable[j]);
            //                        lv.SubItems.Add(PublicDataClass._YxConfigParam.AddrTable[j]);
            //                        str = "0x" + String.Format("{0:x}", Convert.ToInt16(PublicDataClass._YxConfigParam.AddrTable[j]));
            //                        lv.SubItems.Add(str);
            //                        lv.SubItems.Add("是");
            //                        lv.SubItems.Add(PublicDataClass._YxConfigParam.QufanTable[j]);
            //                        listView1.Items.Add(lv);
            //                        ch++;
            //                        yxadr++;
            //                        break;
            //                    }
            //                }
            //                catch { }
            //            }
            //        }
            //        ReNewCfgState();
            //    }
            //    else
            //    {
            //        for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
            //        {
            //            //if (PublicDataClass._YkConfigParam.AddrTable[j] != "65535")
            //            //{
            //                ListViewItem lv = new ListViewItem(String.Format("{0:d}", ch));
            //                lv.SubItems.Add(addr);
            //                lv.SubItems.Add(PublicDataClass._YkConfigParam.NameTable[j]);
            //                lv.SubItems.Add(Convert.ToString(24577 + j));
            //                str = "0x" + String.Format("{0:x}", Convert.ToInt32(24577 + j));
            //                lv.SubItems.Add(str);
            //                lv.SubItems.Add("是");
            //                listView1.Items.Add(lv);
            //                ch++;
            //            //}
            //        }
            //        PublicDataClass._YkConfigParam.Ykinputflag = true;
            //        ReNewCfgState();

            //    }
            //}
            //catch
            //{
            //    ReNewCfgState();
            //    MessageBox.Show("配置文件有误，请确认！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void outputbotton_Click(object sender, EventArgs e)
        {
            //string FileName = "";

            //string path = System.Environment.CurrentDirectory;
            //path += "\\ini";

            //if (ItemId == 1)      //遥测
            //{
            //    PublicDataClass.ThreeYNameTable.YcTable.Clear();                 //清空名称表
            //    PublicDataClass.InfoAddrTable.YcInfoATable.Clear();              //清空信息表
            //    PublicDataClass._ShowIndexTableParam.YcShowIndexTable.Clear();              //清空遥测显示序号表

            //    for (int j = 0; j < listView1.Items.Count; j++)
            //    {
            //        PublicDataClass.ThreeYNameTable.YcTable.Add(listView1.Items[j].SubItems[2].Text);
            //        PublicDataClass.InfoAddrTable.YcInfoATable.Add(listView1.Items[j].SubItems[3].Text);  //加入到表中
            //    //    PublicDataClass._ShowIndexTableParam.YcShowIndexTable.Add(listView2.Items[j].SubItems[6].Text);

            //    }

            //    FileName = path + "\\ycname.ini";
            //    PublicDataClass.ThreeYNameTable.Ycnum = listView1.Items.Count;


            //    WriteReadAllFile.WriteReadYxYkYmIniFile(FileName, 1, 1);

            //    FileName = path + "\\Ycinfoaddr.ini";

            //    PublicDataClass.InfoAddrTable.Ycnum = listView1.Items.Count;
            //    WriteReadAllFile.WriteReadYxYkYmIniFile(FileName, 1, 4);

            //}
            //else if (ItemId == 2)  //遥信
            //{
            //    PublicDataClass.ThreeYNameTable.YxTable.Clear();                 //清空名称表
            //    PublicDataClass.InfoAddrTable.YxInfoATable.Clear();              //清空信息表

            //    for (int j = 0; j < listView1.Items.Count; j++)
            //    {
            //        PublicDataClass.ThreeYNameTable.YxTable.Add(listView1.Items[j].SubItems[2].Text);
            //        PublicDataClass.InfoAddrTable.YxInfoATable.Add(listView1.Items[j].SubItems[3].Text);  //加入到表中

            //    }

            //    FileName = path + "\\yxname.ini";
            //    PublicDataClass.ThreeYNameTable.Yxnum = listView1.Items.Count;


            //    WriteReadAllFile.WriteReadYxYkYmIniFile(FileName, 1, 2);

            //    FileName = path + "\\Yxinfoaddr.ini";

            //    PublicDataClass.InfoAddrTable.Yxnum = listView1.Items.Count;
            //    WriteReadAllFile.WriteReadYxYkYmIniFile(FileName, 1, 5);

            //}
            //else         //遥脉
            //{
            //    PublicDataClass.ThreeYNameTable.YmTable.Clear();                 //清空名称表
            //    PublicDataClass.InfoAddrTable.YmInfoATable.Clear();              //清空信息表

            //    for (int j = 0; j < listView1.Items.Count; j++)
            //    {
            //        PublicDataClass.ThreeYNameTable.YmTable.Add(listView1.Items[j].SubItems[2].Text);
            //        PublicDataClass.InfoAddrTable.YmInfoATable.Add(listView1.Items[j].SubItems[3].Text);  //加入到表中

            //    }

            //    FileName = path + "\\ymname.ini";
            //    PublicDataClass.ThreeYNameTable.Ymnum = listView1.Items.Count;


            //    WriteReadAllFile.WriteReadYxYkYmIniFile(FileName, 1, 3);

            //    FileName = path + "\\Yminfoaddr.ini";

            //    PublicDataClass.InfoAddrTable.Ymnum = listView1.Items.Count;
            //    WriteReadAllFile.WriteReadYxYkYmIniFile(FileName, 1, 6);
            //}
        }

       

        private void listView1_SubItemClicked(object sender, SubItemEventArgs e)
        {
            if (e.SubItem <=4) // Password field
            {
                // the current value (text) of the subitem is ****, so we have to provide
                // the control with the actual text (that's been saved in the item's Tag property)
                return;
            }

          //  listView1.StartEditing(Editors[e.SubItem], e.Item, e.SubItem);
        }

        private void combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView1.EndEditing(true);
            ReNewCfgState();
        }

        private void ToolStripInsertMenuItem_Click(object sender, EventArgs e)
        {
            ////AddInfoAddrViewForm AddInfofm = new AddInfoAddrViewForm();
            ////AddInfofm.ShowDialog();
            ////if (AddInfofm.DialogResult == DialogResult.OK)               //判断是否按下-确定-按钮
            ////{
            //    //ListViewItem lv = new ListViewItem(String.Format("{0:d}", this.listView1.Items.Count));
            //    //lv.SubItems.Add("1");
            //    //lv.SubItems.Add(PublicDataClass._AddInfoRecord.Name);
            //    //lv.SubItems.Add(PublicDataClass._AddInfoRecord.Pos);
            //    //lv.SubItems.Add("null");
            //    //listView1.Items.Add(lv);
            //    //ListViewItem lv = new ListViewItem(String.Format("{0:d}", this.listView1.SelectedItems[0].Index));              
            //    ListViewItem lv = new ListViewItem(String.Format("{0:d}", 1));              
            //     lv.SubItems.Add("1");
            //    //lv.SubItems.Add(PublicDataClass._AddInfoRecord.Name);
            //    //lv.SubItems.Add(PublicDataClass._AddInfoRecord.Pos);
            //    lv.SubItems.Add("THD_U0[1][0]");
            //    lv.SubItems.Add("");
            //    lv.SubItems.Add("");
            //    //lv.SubItems.Add(PublicDataClass._AddParamRecord.Value);
            //    //lv.SubItems.Add(PublicDataClass._AddParamRecord.Beilv);
            //    lv.SubItems.Add("1");
            //    lv.SubItems.Add("1");
            //    listView1.Items.Insert(this.listView1.SelectedItems[0].Index, lv);
            //    for (int i = 0; i < listView1.Items.Count; i++)
            //    {
            //        listView1.Items[i].SubItems[0].Text = String.Format("{0:d}", i);   //重新调整序号

            //    }
            //    ReNewCfgState();
            ////}
        }

        private void buttonoutput_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("记录项内容为空", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                SaveFileDialog savefile = new SaveFileDialog();
                savefile.Filter = "System Files(*.ini)|*.ini|所有文件(*.*)|*.*";
                saveret = savefile.ShowDialog() == DialogResult.OK;
                if (saveret)
                {
                    savename = savefile.FileName;
                    //if (File.Exists(savename) == true)//判断文件是否存在
                    //{
                    //    MessageBox.Show("文件已存在");
                    //    return;
                    //}
                    //File.Create(savename);
                    FileStream afile;
                    StreamWriter sw;
                    afile = new FileStream(savename, FileMode.Create);
                    sw = new StreamWriter(afile);
                    sw.WriteLine("[NUM]");
                    sw.WriteLine("[NAMETABLE]");
                    sw.WriteLine("[ADDRESSTABLE]");
                    sw.Close();
                    afile.Close();
                    buttonoutput.Text = "导出中..";
                    savefile.Dispose();

                    //saveret = false;
                    //WriteReadIniFile(savename, 1);
                    //for (int i = 0; i < 10000; i++)
                    //{ }
                    //buttonoutput.Text = "导出";
                }
            }
        }

        private void ExportToText(string path)
        {
            string s = "";
            int len = 0;
            string temp = "";
            try
            {
                //PageTable = LiShiTable.Copy;
                for (int i = 0; i < listView1.Columns.Count; i++)
                {
                    temp = listView1.Columns[i].Text;
                    len = 20 - Encoding.Default.GetByteCount(temp) + temp.Length; //考虑中英文的情况                  
                    temp = temp.PadRight(len, ' ');
                    s += temp;
                    //s += listView1.Columns[i].Text + "      ";
                }
                s += "\r\n";
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    for (int j = 0; j < listView1.Items[i].SubItems.Count; j++)
                    {
                        temp = listView1.Items[i].SubItems[j].Text;
                        len = 20 - Encoding.Default.GetByteCount(temp) + temp.Length; //考虑中英文的情况                  
                        temp = temp.PadRight(len, ' ');
                        s += temp;
                        //s += listView1.Items[i].SubItems[j].Text + "      ";
                    }
                    s += "\r\n";
                }
                FileStream afile;
                StreamWriter sw;
                path += ".txt";
                afile = new FileStream(path, FileMode.Create);
                sw = new StreamWriter(afile);
                sw.Write(s);

                sw.Close();
                afile.Close();
                MessageBox.Show("数据导出成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("数据导出失败！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WriteReadIniFile(string fname, byte Type)
        {
            byte i = PublicFunction.FindPointNameCorrelativeIndex(comboBox1.Text);
            if (Type == 0)    //读信息
            {
                GetPrivateProfileString("NUM", "num", "无法读取对应数值！",
                                                         temp, 255, fname);
                inputnum = int.Parse(temp.ToString());                    //转换为整型
                name = new string[inputnum];
                address = new string[inputnum];
                for (int j = 0; j < inputnum; j++)
                {
                    str1 = String.Format("name_{0:d}", j);

                    GetPrivateProfileString("NAMETABLE", str1, "无法读取对应数值！",
                                                 temp, 255, fname);
                    name[j] = temp.ToString();
                    
                }
                for (int j = 0; j < inputnum; j++)
                {
                    str1 = String.Format("addr_{0:d}", j);

                    GetPrivateProfileString("ADDRESSTABLE", str1, "无法读取对应数值！",
                                                 temp, 255, fname);
                    address[j] = temp.ToString();
                }
            }
            else if (Type == 1)    //写信息
            {
                //FileStream afile;
                //StreamWriter sw;
                //afile = new FileStream(fname, FileMode.Create);
                //sw = new StreamWriter(afile);
                if (ItemId == 1)
                {
                    //sw.WriteLine("[NUM]");
                    //sw.WriteLine("num=" + PublicDataClass.SaveText.Cfg[i].YccfgNum); 
                    str1 = Convert.ToString(PublicDataClass.SaveText.Cfg[i].YccfgNum);
                    WritePrivateProfileString("NUM", "num", str1, fname);

                    for (int j = 0; j < PublicDataClass.SaveText.Cfg[i].YccfgNum; j++)
                    {
                        WritePrivateProfileString("NAMETABLE", null, null, fname);
                        WritePrivateProfileString("ADDRESSTABLE", null, null, fname);     //先清除掉
                    }

                    //sw.WriteLine("[NAMETABLE]");
                    //for (int j = 0; j < PublicDataClass.SaveText.Cfg[i].YccfgNum; j++)
                    //{
                    //    str1 = String.Format("name_{0:d}", j);                    
                    //    sw.WriteLine(str1 + "=" + PublicDataClass.SaveText.Cfg[i].YccfgName[j]);
                    //}
                    //sw.WriteLine("[ADDRESSTABLE]");
                    //for (int j = 0; j < PublicDataClass.SaveText.Cfg[i].YccfgNum; j++)
                    //{
                    //    str1 = String.Format("addr_{0:d}", j);
                    //    sw.WriteLine(str1 + "=" + PublicDataClass.SaveText.Cfg[i].YccfgAddr[j]);
                    //}
                    for (int j = 0; j < PublicDataClass.SaveText.Cfg[i].YccfgNum; j++)
                    {
                        str1 = String.Format("name_{0:d}", j);

                        WritePrivateProfileString("NAMETABLE", str1, Convert.ToString(PublicDataClass.SaveText.Cfg[i].YccfgName[j]), fname);

                    }
                    for (int j = 0; j < PublicDataClass.SaveText.Cfg[i].YccfgNum; j++)
                    {
                        str1 = String.Format("addr_{0:d}", j);
                        WritePrivateProfileString("ADDRESSTABLE", str1, Convert.ToString(PublicDataClass.SaveText.Cfg[i].YccfgAddr[j]), fname);
                    }
                }
                else if (ItemId == 2)
                {
                    //sw.WriteLine("[NUM]");
                    //sw.WriteLine("num=" + PublicDataClass.SaveText.Cfg[i].YxcfgNum);
                    //sw.WriteLine("[NAMETABLE]");
                    //for (int j = 0; j < PublicDataClass.SaveText.Cfg[i].YxcfgNum; j++)
                    //{
                    //    str1 = String.Format("name_{0:d}", j);
                    //    sw.WriteLine(str1 + "=" + PublicDataClass.SaveText.Cfg[i].YxcfgName[j]);
                    //}
                    //sw.WriteLine("[ADDRESSTABLE]");
                    //for (int j = 0; j < PublicDataClass.SaveText.Cfg[i].YxcfgNum; j++)
                    //{
                    //    str1 = String.Format("addr_{0:d}", j);
                    //    sw.WriteLine(str1 + "=" + PublicDataClass.SaveText.Cfg[i].YxcfgAddr[j]);
                    //}
                    str1 = Convert.ToString(PublicDataClass.SaveText.Cfg[i].YxcfgNum);
                    WritePrivateProfileString("NUM", "num", str1, fname);

                    for (int j = 0; j < PublicDataClass.SaveText.Cfg[i].YxcfgNum; j++)
                    {
                        WritePrivateProfileString("NAMETABLE", null, null, fname);
                        WritePrivateProfileString("ADDRESSTABLE", null, null, fname);     //先清除掉
                    }

                    for (int j = 0; j < PublicDataClass.SaveText.Cfg[i].YxcfgNum; j++)
                    {
                        str1 = String.Format("name_{0:d}", j);

                        WritePrivateProfileString("NAMETABLE", str1, Convert.ToString(PublicDataClass.SaveText.Cfg[i].YxcfgName[j]), fname);

                    }
                    for (int j = 0; j < PublicDataClass.SaveText.Cfg[i].YxcfgNum; j++)
                    {
                        str1 = String.Format("addr_{0:d}", j);
                        WritePrivateProfileString("ADDRESSTABLE", str1, Convert.ToString(PublicDataClass.SaveText.Cfg[i].YxcfgAddr[j]), fname);
                    }
                }
                else if (ItemId == 3)
                {
                    //sw.WriteLine("[NUM]");
                    //sw.WriteLine("num=" + PublicDataClass.SaveText.Cfg[i].YkcfgNum);
                    //sw.WriteLine("[NAMETABLE]");
                    //for (int j = 0; j < PublicDataClass.SaveText.Cfg[i].YkcfgNum; j++)
                    //{
                    //    str1 = String.Format("name_{0:d}", j);
                    //    sw.WriteLine(str1 + "=" + PublicDataClass.SaveText.Cfg[i].YkcfgName[j]);
                    //}
                    //sw.WriteLine("[ADDRESSTABLE]");
                    //for (int j = 0; j < PublicDataClass.SaveText.Cfg[i].YccfgNum; j++)
                    //{
                    //    str1 = String.Format("addr_{0:d}", j);
                    //    sw.WriteLine(str1 + "=" + PublicDataClass.SaveText.Cfg[i].YkcfgAddr[j]);
                    //}
                    str1 = Convert.ToString(PublicDataClass.SaveText.Cfg[i].YkcfgNum);
                    WritePrivateProfileString("NUM", "num", str1, fname);

                    for (int j = 0; j < PublicDataClass.SaveText.Cfg[i].YkcfgNum; j++)
                    {
                        WritePrivateProfileString("NAMETABLE", null, null, fname);
                        WritePrivateProfileString("ADDRESSTABLE", null, null, fname);     //先清除掉
                    }

                    for (int j = 0; j < PublicDataClass.SaveText.Cfg[i].YkcfgNum; j++)
                    {
                        str1 = String.Format("name_{0:d}", j);

                        WritePrivateProfileString("NAMETABLE", str1, Convert.ToString(PublicDataClass.SaveText.Cfg[i].YkcfgName[j]), fname);

                    }
                    for (int j = 0; j < PublicDataClass.SaveText.Cfg[i].YkcfgNum; j++)
                    {
                        str1 = String.Format("addr_{0:d}", j);
                        WritePrivateProfileString("ADDRESSTABLE", str1, Convert.ToString(PublicDataClass.SaveText.Cfg[i].YkcfgAddr[j]), fname);
                    }
                }
                //sw.Close();
                //afile.Close();

            }
            return;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (saveret)
            {
                saveret = false;
                WriteReadIniFile(savename, 1);
                for (int i = 0; i < 10000; i++)
                { }
                buttonoutput.Text = "导出";
                MessageBox.Show("数据导出成功", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //if (inputflag)
            //{
            //    inputflag = false;
               
            //}
        }

        private void buttoninput_Click(object sender, EventArgs e)
        {
            for (byte j = 0; j < PublicDataClass.SaveText.devicenum; j++)
            {
                if (comboBox1.Text == PublicDataClass.SaveText.Device[j].PointName)
                {
                    addr = PublicDataClass.SaveText.Device[j].Addr;
                    break;
                }
            }
             OpenFileDialog ofile = new OpenFileDialog();
            ofile.Filter = "System Files(*.ini)|*.ini|所有文件(*.*)|*.*";
            ofile.InitialDirectory = System.Environment.CurrentDirectory;
            string sysfilepath = @"";
            inputflag = ofile.ShowDialog() == DialogResult.OK;
            if (inputflag)
            {
                buttoninput.Text = "导入中..";
                ofile.Dispose();
                try
                {
                    sysfilepath = ofile.FileName;
                    WriteReadIniFile(sysfilepath, 0);
                    listView1.Items.Clear();
                    for (int j = 0; j < inputnum; j++)
                    {
                        ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));
                        lv.SubItems.Add(addr);
                        lv.SubItems.Add(name[j]);
                        lv.SubItems.Add(address[j]);
                        str = "0x" + String.Format("{0:x}", Convert.ToInt16(address[j]));
                        lv.SubItems.Add(str);
                        lv.SubItems.Add("是");
                        lv.SubItems.Add("否");
                        listView1.Items.Add(lv);
                    }
                }
                catch
                {
                    MessageBox.Show("数据导入失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                buttoninput.Text = "导入";
                
            }
        }

        
       
    }
}
