using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using KD.WinFormsUI.Docking;
using FaTestSoft.CommonData;
using FaTestSoft.FUNCTIONCLASS;
using FaTestSoft.INIT;
using System.Xml;


namespace FaTestSoft
{
    public partial class AcquisitionDocmentView : DockContent
    {
        public AcquisitionDocmentView()
        {
            InitializeComponent();
        }
        private Control[] Editors;
        private Control[] Editors1;
        private Control[] Editors2;
        private Control[] Editors3;
        private Control[] Editors4;
        private Control[] Editors5;

        [DllImport("DataConVert.dll")]
        private static extern void InttoByte(int source, ref byte pdata);


        [DllImport("Operateprotocol.dll")]
        private static extern byte EncodeOneByte(byte BusNum, byte CardNum, byte UbusConnectionmode, byte IbusConnectionmode, ref byte pdata);
        [DllImport("Operateprotocol.dll")]
        private static extern byte EncodeThreeByte(int Index, byte Datatype, byte Magnification, ref byte pdata);

        public static int dataPos = 0;
        public static int Pos = 0;
        public int num = 0;
        public static byte ItemId = 0;
        private int ty;
        private int count = 0;
        private int zbpz = 0;//保存当前显示综保地址配置表位置号
        private int zbyx = 0;//保存当前显示综保遥信配置表位置号


        bool saveret = false;
        string savename;
        bool IniCfgDlgFirstShowFlag = false;
        //TabPage tp_dottable = new TabPage();//动态添加选项卡
        public static StringBuilder temp = new StringBuilder(255);       //初始化 一个StringBuilder的类型
        public static string str;


        public static int[] YxCfgIndex;
        public static int YxCfgNum = 0;





        /// <summary>
        /// ///////////////////////////////////////动态选项卡 /// ///////////////////////////////////////
        /// </summary>

        TabPage[] tp;

        //TabPage[] tp = new TabPage[50];//暂时开50个动态选项卡



        XmlDocument xmldoc;
        XmlElement xmlelem;


        private void AcquisitionDocmentView_Load(object sender, EventArgs e)
        {
            //if (PublicDataClass.DynOptCfgFlag == 1)
            //    tp = new TabPage[PublicDataClass.FILENAME.Length];
            //   PublicDataClass.FILENAME = new string[0];
            checkBox1.Checked = true;
            comboBoxaddr.SelectedIndex = 0;
            YxCfgNum = PublicDataClass.SaveText.Cfg[0].YxcfgNum;//第一次打开工程时
            Editors = new Control[] {
	                                textBoxvalue,
									textBoxvalue,			// for column 1
                                    textBoxvalue,
                                    textBoxvalue,           //
											// 
                                                                  
									};
            Editors1 = new Control[] {
	                                textBoxvalue,
									textBoxvalue,			// for column 1
                                    textBoxvalue,
                                    textBoxvalue,           //
									comboBoxvalue,			// 
                                    comboBoxvalue1,
                                    comboBoxvalue2,
                                    comboBoxvalue3,
                                    //comboBoxvalue3,
                                    //textBoxvalue,
                                    textBoxvalue,
									};
            Editors2 = new Control[] {
	                                textBoxvalue,
									textBoxvalue,			// for column 1
                                    textBoxvalue,
                                    textBoxvalue,
                                    comboBoxvalue3, 
                                    //comboBoxvalue3, 
                                    //textBoxvalue,
                                    textBoxvalue,//					                        
									};
            Editors3 = new Control[] {
                                    textBoxvalue,
	                                textBoxvalue,		
                                    textBoxvalue,           // for column 1
                                    comboBoxvalue,
                                    textBoxvalue,
                                    textBoxvalue,
                                    textBoxvalue,
                                    comboBoxvalue1,
                                    comboBoxvalue2,
                                    textBoxvalue,
                                    textBoxvalue,
                                    textBoxvalue,
                                    textBoxvalue,
                                    textBoxvalue,
									};
            Editors4 = new Control[] {
                                    textBoxvalue,
	                                textBoxvalue,		
                                    textBoxvalue,           // for column 1
                                    textBoxvalue,
                                    textBoxvalue,
                                    textBoxvalue,
                                    textBoxvalue,
                                    textBoxvalue,
                                    textBoxvalue,
                                    textBoxvalue,
                                    textBoxvalue,
                                    textBoxvalue,
                                    textBoxvalue,
                                    textBoxvalue,
                                    textBoxvalue,
                                    textBoxvalue,
                                     textBoxvalue,
                                    textBoxvalue,
                                    textBoxvalue,
                                    textBoxvalue,
                                    textBoxvalue,
                                    textBoxvalue,
                                    textBoxvalue,
                                    textBoxvalue,
                                    textBoxvalue
									};
            Editors5 = new Control[] {
                                    textBoxvalue,
	                                comboBoxvalue1,
                                    comboBoxvalue2,
                                    comboBoxvalue3,
                                    textBoxvalue,
                                    textBoxvalue,
                                    textBoxvalue,
                                    
									};
            //comboBoxaddr.Items.Add("从板1");
            //comboBoxaddr.Items.Add("从板2");
            //comboBoxaddr.Items.Add("从板3");
            //comboBoxaddr.Items.Add("广 播");
            //comboBoxaddr.SelectedIndex = 3;
            //comboBoxaddr.Enabled = false;


            if (PublicDataClass.SaveText.devicenum == 0)
            {
                comboBox1.Text = "无信息";

            }
            else
            {
                for (int i = 0; i < PublicDataClass.SaveText.devicenum; i++)
                {
                    comboBox1.Items.Add(PublicDataClass.SaveText.Device[i].PointName);
                }
                comboBox1.Text = PublicDataClass.SaveText.Device[0].PointName;

            }
            num = PublicDataClass.SaveText.devicenum;

            listView1.Items.Clear();
            listView1.Columns.Clear();
            listView1.Columns.Add("序号", 60);
            listView1.Columns.Add("名称", 200);
            listView1.Columns.Add("数值", 200);
            listView1.Columns.Add("字节", 60);
            for (int j = 0; j < PublicDataClass._ComParam.num; j++)
            {
                ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));
                lv.SubItems.Add(PublicDataClass._ComParam.NameTable[j]);
                lv.SubItems.Add(PublicDataClass._ComParam.ValueTable[j]);
                lv.SubItems.Add(PublicDataClass._ComParam.ByteTable[j]);
                listView1.Items.Add(lv);
            }
            tabPage1.Controls.Add(listView1);
            ItemId = 1;  //默认的为1
            downloadbutton.Enabled = true;
            textBoxpassword.Enabled = false;
            /// <summary>
            ////根据权限隐藏选项卡
            /// </summary>

            //tabPage2.Parent = null;
            //tabPage3.Parent = null;

            //if (PublicDataClass.DynOptCfgFlag == 1)//动态选项卡已配置
            //{
            //    DynOptProcess();//动态添加选项卡
            //    for (int k = 0; k < PublicDataClass.FILENAME.Length; k++)
            //    {  
            //            tp[k].Parent= null;

            //        }

            //    }



        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            try
            {
                button5.Visible = false;//综保配置按钮隐藏
                button6.Visible = false;//综保配置按钮显示
                button7.Visible = false;//综保配置按钮显示
                label3.Text = "";
                switch (e.TabPage.Name)
                {
                    case "tabPage1":                //串口参数
                        ItemId = 1;
                        downloadbutton.Enabled = true;    //12-06-28应吉林要求变为不使能  
                        textBoxpassword.Enabled = false;

                        快捷配置ToolStripMenuItem.Enabled = false;
                        cDT点号配置ToolStripMenuItem.Enabled = false;
                        参数修正ToolStripMenuItem.Enabled = false;
                        遥信刷新ToolStripMenuItem.Enabled = false;
                        遥信取反ToolStripMenuItem.Enabled = false;
                        //comboBoxaddr.Items.Clear();
                        //comboBoxaddr.Items.Add("从板1");
                        //comboBoxaddr.Items.Add("从板2");
                        //comboBoxaddr.Items.Add("从板3");
                        //comboBoxaddr.Items.Add("广 播");
                        //comboBoxaddr.SelectedIndex = 3;
                        tabPage1.Controls.Add(listView1);
                        listView1.Items.Clear();
                        listView1.Columns.Clear();
                        listView1.Columns.Add("序号", 60);
                        listView1.Columns.Add("名称", 200);
                        listView1.Columns.Add("数值", 200);
                        listView1.Columns.Add("字节", 60);
                        //listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                        listView1.Controls.Add(textBoxvalue);
                        textBoxvalue.Visible = false;
                        CheckNowParamState();

                        break;

                    case "tabPage2":                //网络参数
                        ItemId = 2;
                        downloadbutton.Enabled = true;
                        textBoxpassword.Enabled = false;
                        快捷配置ToolStripMenuItem.Enabled = false;
                        cDT点号配置ToolStripMenuItem.Enabled = false;
                        参数修正ToolStripMenuItem.Enabled = false;
                        遥信刷新ToolStripMenuItem.Enabled = false;
                        遥信取反ToolStripMenuItem.Enabled = false;
                        //comboBoxaddr.Items.Clear();
                        //comboBoxaddr.Items.Add("主 板");
                        //comboBoxaddr.Items.Add("从板1");
                        //comboBoxaddr.Items.Add("从板2");
                        //comboBoxaddr.Items.Add("从板3");
                        //comboBoxaddr.Items.Add("广 播");
                        //comboBoxaddr.SelectedIndex = 0;
                        tabPage2.Controls.Add(listView1);
                        listView1.Items.Clear();
                        listView1.Columns.Clear();
                        listView1.Columns.Add("序号", 60);
                        listView1.Columns.Add("名称", 200);
                        listView1.Columns.Add("数值", 200);
                        listView1.Columns.Add("字节", 60);
                        //listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                        listView1.Controls.Add(textBoxvalue);
                        textBoxvalue.Visible = false;
                        CheckNowParamState();
                        break;

                    case "tabPage3":                //系统参数
                        ItemId = 3;
                        downloadbutton.Enabled = true;
                        textBoxpassword.Enabled = false;
                        快捷配置ToolStripMenuItem.Enabled = false;
                        cDT点号配置ToolStripMenuItem.Enabled = false;
                        参数修正ToolStripMenuItem.Enabled = false;
                        遥信刷新ToolStripMenuItem.Enabled = false;
                        遥信取反ToolStripMenuItem.Enabled = false;
                        //comboBoxaddr.Items.Clear();
                        //comboBoxaddr.Items.Add("主 板");
                        //comboBoxaddr.Items.Add("从板1");
                        //comboBoxaddr.Items.Add("从板2");
                        //comboBoxaddr.Items.Add("从板3");
                        //comboBoxaddr.Items.Add("广 播");
                        //comboBoxaddr.SelectedIndex = 0;
                        tabPage3.Controls.Add(listView1);
                        listView1.Items.Clear();
                        listView1.Columns.Clear();
                        listView1.Columns.Add("序号", 60);
                        listView1.Columns.Add("名称", 200);
                        listView1.Columns.Add("数值", 200);
                        listView1.Columns.Add("字节", 60);
                        //listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                        listView1.Controls.Add(textBoxvalue);
                        textBoxvalue.Visible = false;
                        CheckNowParamState();
                        break;
                    case "tabPage4":                //遥测配置
                        ItemId = 4;
                        downloadbutton.Enabled = true;
                        textBoxpassword.Enabled = false;
                        快捷配置ToolStripMenuItem.Enabled = true;
                        cDT点号配置ToolStripMenuItem.Enabled = true;
                        参数修正ToolStripMenuItem.Enabled = false;
                        遥信刷新ToolStripMenuItem.Enabled = false;
                        遥信取反ToolStripMenuItem.Enabled = false;
                        //comboBoxaddr.Items.Clear();
                        //comboBoxaddr.Items.Add("主 板");

                        //comboBoxaddr.SelectedIndex = 0;
                        tabPage4.Controls.Clear();
                        tabPage4.Controls.Add(listView1);
                        listView1.Items.Clear();
                        listView1.Columns.Clear();
                        listView1.Columns.Clear();
                        listView1.Columns.Add("序号", 60);
                        listView1.Columns.Add("名称", 200);
                        listView1.Columns.Add("索引号", 60);
                        listView1.Columns.Add("信息体地址", 100);
                        //    listView1.Columns.Add("数据类型", 60);
                        //  listView1.Columns.Add("放大倍数", 60);
                        //  listView1.Columns.Add("母线接法", 60);
                        //  listView1.Columns.Add("取反标志", 60);
                        //listView1.Columns.Add("置数标志");
                        //listView1.Columns.Add("置数数值");
                        //  listView1.Columns.Add("字节", 60);
                        //listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                        //listView1.Controls.Add(textBoxvalue);
                        //textBoxvalue.Visible = false;

                        //comboBoxvalue.Items.Clear();
                        //comboBoxvalue.Items.Add("整型");
                        //comboBoxvalue.Items.Add("浮点型");
                        //listView1.Controls.Add(comboBoxvalue);
                        //comboBoxvalue.Visible = false;

                        //comboBoxvalue1.Items.Clear();
                        //comboBoxvalue1.Items.Add("不放大");
                        //comboBoxvalue1.Items.Add("10倍");
                        //comboBoxvalue1.Items.Add("100倍");
                        //comboBoxvalue1.Items.Add("1000倍");
                        //listView1.Controls.Add(comboBoxvalue1);
                        //comboBoxvalue1.Visible = false;

                        //comboBoxvalue2.Items.Clear();
                        //comboBoxvalue2.Items.Add("NULL");
                        //comboBoxvalue2.Items.Add("Y型");
                        //comboBoxvalue2.Items.Add("V型");
                        //listView1.Controls.Add(comboBoxvalue2);
                        //comboBoxvalue2.Visible = false;

                        //comboBoxvalue3.Items.Clear();
                        //comboBoxvalue3.Items.Add("是");
                        //comboBoxvalue3.Items.Add("否");
                        //listView1.Controls.Add(comboBoxvalue3);
                        //comboBoxvalue3.Visible = false;

                        CheckNowParamState();
                        break;
                    case "tabPage5":                //遥信配置
                        ItemId = 5;
                        downloadbutton.Enabled = true;
                        textBoxpassword.Enabled = false;
                        快捷配置ToolStripMenuItem.Enabled = true;
                        cDT点号配置ToolStripMenuItem.Enabled = false;
                        参数修正ToolStripMenuItem.Enabled = false;
                        遥信刷新ToolStripMenuItem.Enabled = false;
                        遥信取反ToolStripMenuItem.Enabled = false;
                        //comboBoxaddr.Items.Clear();
                        //comboBoxaddr.Items.Add("主 板");

                        //comboBoxaddr.SelectedIndex = 0;
                        tabPage5.Controls.Add(listView1);
                        listView1.Items.Clear();
                        listView1.Columns.Clear();
                        listView1.Columns.Add("序号", 60);
                        listView1.Columns.Add("名称", 200);
                        listView1.Columns.Add("索引号", 60);
                        listView1.Columns.Add("信息体地址", 100);
                        listView1.Columns.Add("取反标志", 60);
                        //listView1.Columns.Add("置数标志", 60);
                        //listView1.Columns.Add("置数数值", 60);
                        listView1.Columns.Add("字节", 60);

                        //listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                        listView1.Controls.Add(textBoxvalue);
                        textBoxvalue.Visible = false;

                        comboBoxvalue3.Items.Clear();
                        comboBoxvalue3.Items.Add("是");
                        comboBoxvalue3.Items.Add("否");
                        listView1.Controls.Add(comboBoxvalue3);
                        comboBoxvalue3.Visible = false;
                        CheckNowParamState();
                        break;
                    case "tabPage6":                //遥控配置
                        ItemId = 6;
                        downloadbutton.Enabled = true;
                        textBoxpassword.Enabled = false;
                        快捷配置ToolStripMenuItem.Enabled = false;
                        cDT点号配置ToolStripMenuItem.Enabled = false;
                        参数修正ToolStripMenuItem.Enabled = false;
                        遥信刷新ToolStripMenuItem.Enabled = false;
                        遥信取反ToolStripMenuItem.Enabled = false;
                        //comboBoxaddr.Items.Clear();
                        //comboBoxaddr.Items.Add("从板1");
                        //comboBoxaddr.Items.Add("从板2");
                        //comboBoxaddr.Items.Add("从板3");
                        //comboBoxaddr.Items.Add("广 播");
                        //comboBoxaddr.SelectedIndex = 3;
                        tabPage6.Controls.Add(listView1);
                        listView1.Items.Clear();
                        listView1.Columns.Clear();
                        listView1.Columns.Add("序号", 60);
                        listView1.Columns.Add("名称", 200);
                        listView1.Columns.Add("属性数值", 100);
                        listView1.Columns.Add("触发方式", 100);
                        listView1.Columns.Add("选择时间(ms)", 100);
                        listView1.Columns.Add("执行时间(ms)", 100);
                        listView1.Columns.Add("脉冲宽度(ms)", 100);
                        listView1.Columns.Add("记录保存", 60);
                        listView1.Columns.Add("电源属性", 100);
                        listView1.Columns.Add("并联继电器点号（合）", 100);
                        listView1.Columns.Add("并联继电器点号（分）", 100);
                        listView1.Columns.Add("反校遥信点号（合）", 100);
                        listView1.Columns.Add("反校遥信点号（分）", 100);
                        listView1.Columns.Add("字节", 60);
                        //listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                        listView1.Controls.Add(textBoxvalue);
                        textBoxvalue.Visible = false;

                        comboBoxvalue.Items.Clear();
                        comboBoxvalue.Items.Add("电平");
                        comboBoxvalue.Items.Add("脉冲");
                        listView1.Controls.Add(comboBoxvalue);
                        comboBoxvalue.Visible = false;

                        comboBoxvalue1.Items.Clear();
                        comboBoxvalue1.Items.Add("是");
                        comboBoxvalue1.Items.Add("否");
                        listView1.Controls.Add(comboBoxvalue1);
                        comboBoxvalue1.Visible = false;

                        comboBoxvalue2.Items.Clear();
                        comboBoxvalue2.Items.Add("无");
                        comboBoxvalue2.Items.Add("24V");
                        comboBoxvalue2.Items.Add("电操机构");
                        comboBoxvalue2.Items.Add("电源+电操");
                        listView1.Controls.Add(comboBoxvalue2);
                        comboBoxvalue2.Visible = false;
                        CheckNowParamState();
                        break;
                    case "tabPage7":                //模拟量接入配置
                        ItemId = 7;
                        downloadbutton.Enabled = false;

                        textBoxpassword.Enabled = true;
                        快捷配置ToolStripMenuItem.Enabled = false;
                        cDT点号配置ToolStripMenuItem.Enabled = false;
                        参数修正ToolStripMenuItem.Enabled = false;
                        遥信刷新ToolStripMenuItem.Enabled = false;
                        遥信取反ToolStripMenuItem.Enabled = false;
                        tabPage7.Controls.Add(listView1);
                        listView1.Items.Clear();
                        listView1.Columns.Clear();
                        listView1.Columns.Add("序号");
                        listView1.Columns.Add("属性");
                        listView1.Columns.Add("相别");
                        listView1.Columns.Add("线路");
                        listView1.Columns.Add("采集通道");
                        listView1.Columns.Add("幅值");
                        listView1.Columns.Add("相角");
                        listView1.Columns.Add("置数");
                        listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                        listView1.Controls.Add(textBoxvalue);
                        textBoxvalue.Visible = false;
                        CheckNowParamState();
                        for (int j = 0; j < listView1.Items.Count; j++)
                        {
                            listView1.Items[j].UseItemStyleForSubItems = false;
                            listView1.Items[j].SubItems[5].ForeColor = Color.Gray;
                            listView1.Items[j].SubItems[6].ForeColor = Color.Gray;
                            listView1.Items[j].SubItems[7].ForeColor = Color.Gray;
                        }
                        break;
                    case "tabPage8":                //校准参数配置
                        ItemId = 8;
                        downloadbutton.Enabled = false;

                        textBoxpassword.Enabled = true;
                        快捷配置ToolStripMenuItem.Enabled = false;
                        cDT点号配置ToolStripMenuItem.Enabled = false;
                        参数修正ToolStripMenuItem.Enabled = true;
                        遥信刷新ToolStripMenuItem.Enabled = false;
                        遥信取反ToolStripMenuItem.Enabled = false;
                        tabPage8.Controls.Add(listView1);
                        listView1.Items.Clear();
                        listView1.Columns.Clear();
                        listView1.Columns.Add("序号");
                        listView1.Columns.Add("属性");
                        listView1.Columns.Add("相别");
                        listView1.Columns.Add("线路");
                        listView1.Columns.Add("采集通道");
                        listView1.Columns.Add("幅值");
                        listView1.Columns.Add("相角");
                        listView1.Columns.Add("置数");
                        listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                        listView1.Controls.Add(textBoxvalue);
                        textBoxvalue.Visible = false;
                        CheckNowParamState();
                        for (int j = 0; j < listView1.Items.Count; j++)
                        {
                            listView1.Items[j].UseItemStyleForSubItems = false;
                            listView1.Items[j].SubItems[1].ForeColor = Color.Gray;
                            listView1.Items[j].SubItems[2].ForeColor = Color.Gray;
                            listView1.Items[j].SubItems[3].ForeColor = Color.Gray;
                            listView1.Items[j].SubItems[4].ForeColor = Color.Gray;
                            listView1.Items[j].SubItems[7].ForeColor = Color.Gray;
                        }
                        break;
                    case "tabPage9":                //置数配置
                        ItemId = 9;
                        downloadbutton.Enabled = true;
                        快捷配置ToolStripMenuItem.Enabled = false;
                        cDT点号配置ToolStripMenuItem.Enabled = false;
                        参数修正ToolStripMenuItem.Enabled = false;
                        遥信刷新ToolStripMenuItem.Enabled = false;
                        遥信取反ToolStripMenuItem.Enabled = false;
                        tabPage9.Controls.Add(listView1);
                        listView1.Items.Clear();
                        listView1.Columns.Clear();
                        listView1.Columns.Add("序号");
                        listView1.Columns.Add("属性");
                        listView1.Columns.Add("相别");
                        listView1.Columns.Add("线路");
                        listView1.Columns.Add("采集通道");
                        listView1.Columns.Add("幅值");
                        listView1.Columns.Add("相角");
                        listView1.Columns.Add("置数");
                        listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                        listView1.Controls.Add(textBoxvalue);
                        textBoxvalue.Visible = false;
                        CheckNowParamState();
                        for (int j = 0; j < listView1.Items.Count; j++)
                        {
                            listView1.Items[j].UseItemStyleForSubItems = false;
                            listView1.Items[j].SubItems[1].ForeColor = Color.Gray;
                            listView1.Items[j].SubItems[2].ForeColor = Color.Gray;
                            listView1.Items[j].SubItems[3].ForeColor = Color.Gray;
                            listView1.Items[j].SubItems[4].ForeColor = Color.Gray;
                            listView1.Items[j].SubItems[5].ForeColor = Color.Gray;
                            listView1.Items[j].SubItems[6].ForeColor = Color.Gray;
                        }
                        break;

                    default:
                        string str;
                        快捷配置ToolStripMenuItem.Enabled = false;
                        cDT点号配置ToolStripMenuItem.Enabled = false;
                        参数修正ToolStripMenuItem.Enabled = false;
                        遥信刷新ToolStripMenuItem.Enabled = true;
                        遥信取反ToolStripMenuItem.Enabled = true;

                        if (PublicDataClass.DynOptCfgFlag == 1)//动态选项卡已配置
                        {
                            for (int k = 0; k < PublicDataClass.FILENAME.Length; k++)
                            {

                                downloadbutton.Enabled = true;
                                str = String.Format("tp_{0:d}", k);
                                if (e.TabPage.Name == str)
                                {
                                    if (PublicDataClass.FILENAME[k].IndexOf("报警灯关联参数") > 0 || PublicDataClass.FILENAME[k].IndexOf("CDT上传遥信合并点号参数") > 0 || PublicDataClass.FILENAME[k].IndexOf("电量配置") > 0)
                                        快捷配置ToolStripMenuItem.Enabled = true;
                                    else
                                        快捷配置ToolStripMenuItem.Enabled = false;
                                    if (PublicDataClass.FILENAME[k].IndexOf("综保遥信信息点配置") > 0)
                                    {
                                        button5.Visible = true;//综保配置按钮显示
                                        button6.Visible = true;//综保配置按钮显示
                                        button7.Visible = true;//综保配置按钮显示
                                        zbyx = k;
                                    }
                                    if (PublicDataClass.FILENAME[k].IndexOf("综保地址") > 0)
                                    {
                                        button5.Visible = true;//综保配置按钮显示
                                        button6.Visible = true;//综保配置按钮显示
                                        button7.Visible = true;//综保配置按钮显示
                                        zbpz = k;
                                    }
                                    byte[] b = new byte[2];
                                    b[0] = (byte)k;
                                    ItemId = 0x14;
                                    ItemId += b[0];
                                    tp[k].Controls.Add(listViewtest);
                                    CheckNowDynOptParamState(k);//更新动态选项卡参数
                                }

                            }
                        }
                        break;

                }

            }
            catch
            {


            }
        }

        /********************************************************************************************
       *  函数名：    CheckCfgState                                                                *
       *  功能  ：    CheckNowParamState                                                           *
       *  参数  ：    无                                                                           *
       *  返回值：    无                                                                           *
       *  修改日期：  2010-11-09                                                                   *
       *  作者    ：  cuibj                                                                        *
       * ******************************************************************************************/
        private void CheckNowParamState()
        {

            if (ItemId == 1)        //串口参数
            {

                for (int j = 0; j < PublicDataClass._ComParam.num; j++)
                {
                    ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));
                    lv.SubItems.Add(PublicDataClass._ComParam.NameTable[j]);
                    lv.SubItems.Add(PublicDataClass._ComParam.ValueTable[j]);
                    lv.SubItems.Add(PublicDataClass._ComParam.ByteTable[j]);
                    listView1.Items.Add(lv);
                }
            }
            else if (ItemId == 2) //网口参数
            {
                for (int j = 0; j < PublicDataClass._NetParam.num; j++)
                {
                    ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));
                    lv.SubItems.Add(PublicDataClass._NetParam.NameTable[j]);
                    lv.SubItems.Add(PublicDataClass._NetParam.ValueTable[j]);
                    lv.SubItems.Add(PublicDataClass._NetParam.ByteTable[j]);
                    listView1.Items.Add(lv);
                }
            }
            else if (ItemId == 3) //系统参数
            {
                for (int j = 0; j < PublicDataClass._SysParam.num; j++)
                {
                    ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));
                    lv.SubItems.Add(PublicDataClass._SysParam.NameTable[j]);
                    lv.SubItems.Add(PublicDataClass._SysParam.ValueTable[j]);
                    lv.SubItems.Add(PublicDataClass._SysParam.ByteTable[j]);
                    listView1.Items.Add(lv);
                }

            }
            else if (ItemId == 4) //遥测配置
            {
                for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                {
                    ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));
                    lv.SubItems.Add(PublicDataClass._YcConfigParam.NameTable[j]);
                    lv.SubItems.Add(PublicDataClass._YcConfigParam.IndexTable[j]);
                    lv.SubItems.Add(PublicDataClass._YcConfigParam.AddrTable[j]);
                    //   lv.SubItems.Add(PublicDataClass._YcConfigParam.DatatypeTable[j]);
                    //   lv.SubItems.Add(PublicDataClass._YcConfigParam.MagnificationTable[j]);
                    //   lv.SubItems.Add(PublicDataClass._YcConfigParam.ConnectTable[j]);
                    //   lv.SubItems.Add(PublicDataClass._YcConfigParam.QufanTable[j]);
                    //lv.SubItems.Add(PublicDataClass._YcConfigParam.setvalueTable[j]);
                    //lv.SubItems.Add(PublicDataClass._YcConfigParam.ValueTable[j]);
                    //      lv.SubItems.Add(PublicDataClass._YcConfigParam.ByteTable[j]);

                    listView1.Items.Add(lv);
                }

            }
            else if (ItemId == 5) //遥信配置
            {
                int s = 0;
                for (int j = 0; j < PublicDataClass._YxConfigParam.wyxnum; j++)
                {
                    ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));
                    lv.SubItems.Add(PublicDataClass._YxConfigParam.NameTable[j]);
                    lv.SubItems.Add(PublicDataClass._YxConfigParam.IndexTable[j]);
                    lv.SubItems.Add(PublicDataClass._YxConfigParam.AddrTable[j]);

                    lv.SubItems.Add(PublicDataClass._YxConfigParam.QufanTable[j]);
                    //lv.SubItems.Add(PublicDataClass._YxConfigParam.setvalueTable[j]);
                    //lv.SubItems.Add(PublicDataClass._YxConfigParam.ValueTable[j]);
                    lv.SubItems.Add(PublicDataClass._YxConfigParam.ByteTable[j]);
                    listView1.Items.Add(lv);
                }
                for (s = 0; s < PublicDataClass.FILENAME.Length; s++)
                {
                    if (PublicDataClass.TabCfg[s].PageName == "内遥信配置参数")

                        break;

                }

                if (s == PublicDataClass.FILENAME.Length)//查到最后一页没有内遥信配置参数时
                    s = 0;

                for (int j = PublicDataClass._YxConfigParam.wyxnum; j < PublicDataClass._YxConfigParam.num; j++)
                {
                    ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));
                    try
                    {
                        if (PublicDataClass.TabCfg[s].PageName == "内遥信配置参数")
                        {


                            if ((j < PublicDataClass.TabCfg[s].LineNum) && (PublicDataClass.TabCfg[s].TabPageValue[1].ValueTable[j - PublicDataClass._YxConfigParam.wyxnum] != "255"))
                                lv.SubItems.Add("线路" + PublicDataClass.TabCfg[s].TabPageValue[4].ValueTable[j - PublicDataClass._YxConfigParam.wyxnum]
                                                + "-" + PublicDataClass.TabCfg[s].TabPageValue[1].ValueTable[j - PublicDataClass._YxConfigParam.wyxnum]
                                                + "-" + PublicDataClass.TabCfg[s].TabPageValue[3].ValueTable[j - PublicDataClass._YxConfigParam.wyxnum]);


                            else lv.SubItems.Add(PublicDataClass._YxConfigParam.NameTable[j]);//9.9 既不是内遥信又不是外遥信

                        }
                        else
                            //lv.SubItems.Add("YX" + (j + 1));//8.20应延安项目修改
                            lv.SubItems.Add(PublicDataClass._YxConfigParam.NameTable[j]);//8.20应延安项目修改
                    }
                    catch
                    {
                        lv.SubItems.Add("YX" + (j + 1));
                    }
                    lv.SubItems.Add(PublicDataClass._YxConfigParam.IndexTable[j]);
                    lv.SubItems.Add(PublicDataClass._YxConfigParam.AddrTable[j]);
                    lv.SubItems.Add(PublicDataClass._YxConfigParam.QufanTable[j]);
                    //lv.SubItems.Add(PublicDataClass._YxConfigParam.setvalueTable[j]);
                    //lv.SubItems.Add(PublicDataClass._YxConfigParam.ValueTable[j]);
                    lv.SubItems.Add(PublicDataClass._YxConfigParam.ByteTable[j]);
                    listView1.Items.Add(lv);
                }
                RefreshParamState();
            }
            else if (ItemId == 6) //遥控配置
            {
                for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                {
                    ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));
                    lv.SubItems.Add(PublicDataClass._YkConfigParam.NameTable[j]);
                    lv.SubItems.Add(PublicDataClass._YkConfigParam.AddrTable[j]);

                    lv.SubItems.Add(PublicDataClass._YkConfigParam.triggermodeTable[j]);
                    lv.SubItems.Add(PublicDataClass._YkConfigParam.secltimeTable[j]);
                    lv.SubItems.Add(PublicDataClass._YkConfigParam.exetimeTable[j]);
                    lv.SubItems.Add(PublicDataClass._YkConfigParam.pulsewidthTable[j]);
                    lv.SubItems.Add(PublicDataClass._YkConfigParam.saveflagTable[j]);
                    lv.SubItems.Add(PublicDataClass._YkConfigParam.powerTable[j]);
                    lv.SubItems.Add(PublicDataClass._YkConfigParam.jdq1Table[j]);
                    lv.SubItems.Add(PublicDataClass._YkConfigParam.jdq2Table[j]);
                    lv.SubItems.Add(PublicDataClass._YkConfigParam.fjyx1Table[j]);
                    lv.SubItems.Add(PublicDataClass._YkConfigParam.fjyx2Table[j]);

                    lv.SubItems.Add(PublicDataClass._YkConfigParam.ByteTable[j]);



                    lv.SubItems.Add(PublicDataClass._YkConfigParam.ByteTable[j]);

                    listView1.Items.Add(lv);
                }

            }
            else if ((ItemId == 7) || (ItemId == 8) || (ItemId == 9))//模拟量配置
            {
                for (int j = 0; j < PublicDataClass._AIParam.num; j++)
                {
                    ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));
                    lv.SubItems.Add(PublicDataClass._AIParam.quality[j]);
                    lv.SubItems.Add(PublicDataClass._AIParam.phase[j]);
                    lv.SubItems.Add(PublicDataClass._AIParam.line[j]);
                    lv.SubItems.Add(PublicDataClass._AIParam.panel[j]);
                    lv.SubItems.Add(PublicDataClass._AIParam.value[j]);
                    lv.SubItems.Add(PublicDataClass._AIParam.ph[j]);
                    lv.SubItems.Add(PublicDataClass._AIParam.zhishu[j]);

                    listView1.Items.Add(lv);
                }

            }


            //
        }

        private void AcquisitionDocmentView_Activated(object sender, EventArgs e)
        {
            PublicDataClass._WindowsVisable.XtParamUpdateVisable = true;            //窗体可见
            PublicDataClass.addselect = 2;//主处理器
            //timer2.Enabled = true;


            //  if (PublicDataClass.DynOptCfgFlag == 1)
            //     DynOptProcess();//动态添加选项卡
            /////////////////////////////////////////////////
            //for (int k = 0; k < PublicDataClass.FILENAME.Length; k++)
            //{
            //    tp[k].Parent = null;

            //}
            /////////////////////////////////////////////////



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
                    comboBox1.Items.Add(PublicDataClass.SaveText.Device[i].PointName);
                }
                comboBox1.Text = PublicDataClass.SaveText.Device[0].PointName;

            }
        }
        /// <summary>
        /// 添加菜单的 处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddMenuItem_Click(object sender, EventArgs e)
        {

            if (ItemId - 0x14 >= 0)  //动态选项卡
            {
                ListViewItem lv = new ListViewItem(String.Format("{0:d}", this.listViewtest.Items.Count));
                for (int i = 0; i < listViewtest.Columns.Count - 1; i++)
                {
                    lv.SubItems.Add("");
                }
                listViewtest.Items.Add(lv);
                RefreshParamState();
            }
            else
            {
                ListViewItem lv = new ListViewItem(String.Format("{0:d}", this.listView1.Items.Count));
                for (int i = 0; i < listView1.Columns.Count - 1; i++)
                {
                    lv.SubItems.Add("");
                }
                listView1.Items.Add(lv);
                RefreshParamState();
            }

        }

        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除此项吗?", "信  息",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                if (ItemId - 0x14 < 0)
                {
                    ListView.SelectedListViewItemCollection SettleOnItem = new ListView.SelectedListViewItemCollection(this.listView1);

                    if (SettleOnItem.Count <= 0)
                    {
                        MessageBox.Show("记录项选择为空", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    for (int i = 0; i < SettleOnItem.Count; )
                    {

                        listView1.Items.Remove(SettleOnItem[i]);       //删除所选择的项
                    }
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        listView1.Items[i].SubItems[0].Text = String.Format("{0:d}", i);   //重新调整序号

                    }
                    RefreshParamState();
                }


                else if (ItemId - 0x14 >= 0)  //动态选项卡
                {

                    ListView.SelectedListViewItemCollection SettleOnItem = new ListView.SelectedListViewItemCollection(this.listViewtest);

                    if (SettleOnItem.Count <= 0)
                    {
                        MessageBox.Show("记录项选择为空", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    for (int i = 0; i < SettleOnItem.Count; )
                    {

                        listViewtest.Items.Remove(SettleOnItem[i]);       //删除所选择的项
                    }
                    for (int i = 0; i < listViewtest.Items.Count; i++)
                    {
                        listViewtest.Items[i].SubItems[0].Text = String.Format("{0:d}", i);   //重新调整序号
                    }
                    RefreshParamState();
                }

        }
        private void RefreshYcYxCfgParam()
        {
            if (ItemId == 4)//遥测配置
            {
                //PublicDataClass.SaveText.Cfg[i].YccfgNum = listView1.Items.Count;
                //PublicDataClass.SaveText.Cfg[i].YccfgName = new string[listView1.Items.Count];
                //PublicDataClass.SaveText.Cfg[i].YccfgState = new string[listView1.Items.Count];
                //PublicDataClass.SaveText.Cfg[i].YccfgAddr = new string[listView1.Items.Count];
                //PublicDataClass.SaveText.Cfg[i].Ycdata = new string[listView1.Items.Count];
                //PublicDataClass.SaveText.Cfg[i].Ycdataqf = new string[listView1.Items.Count];

                //PublicDataClass._Curve.listemp = new PointPairList[listView1.Items.Count];
                //PublicDataClass._Curve.showcurve = new bool[listView1.Items.Count];
                //PublicDataClass._Curve.ycdata = new double[listView1.Items.Count];
                //for (int j = 0; j < PublicDataClass.SaveText.Cfg[i].YccfgNum; j++)
                //{
                //    PublicDataClass._Curve.listemp[j] = new PointPairList();
                //}

                //for (int j = 0; j < listView1.Items.Count; j++)
                //{
                //    PublicDataClass.SaveText.Cfg[i].YccfgName[j] = listView1.Items[j].SubItems[2].Text;//取得listview某行某列的值

                //    PublicDataClass.SaveText.Cfg[i].YccfgAddr[j] = listView1.Items[j].SubItems[3].Text;

                //    PublicDataClass.SaveText.Cfg[i].YccfgState[j] = listView1.Items[j].SubItems[5].Text;

                //    PublicDataClass.SaveText.Cfg[i].Ycdataqf[j] = listView1.Items[j].SubItems[6].Text;

                //}

            }
            else if (ItemId == 5)//遥信配置
            {
                //int i = 0;
                //PublicDataClass.SaveText.Cfg[i].YxcfgNum = YxCfgNum;
                //PublicDataClass.SaveText.Cfg[i].YxcfgName = new string[YxCfgNum];
                //PublicDataClass.SaveText.Cfg[i].YxcfgState = new string[YxCfgNum];
                //PublicDataClass.SaveText.Cfg[i].YxcfgAddr = new string[YxCfgNum];
                //PublicDataClass.SaveText.Cfg[i].YccfgIndex = new string[YxCfgNum];
                //PublicDataClass.SaveText.Cfg[i].Yxdataqf = new string[YxCfgNum];


                //for (int j = 0; j < YxCfgNum; j++)
                //{
                //    PublicDataClass.SaveText.Cfg[i].YxcfgName[j] = PublicDataClass._YxConfigParam.NameTable[YxCfgIndex[j] - 1];

                //    PublicDataClass.SaveText.Cfg[i].YxcfgAddr[j] = PublicDataClass._YxConfigParam.AddrTable[YxCfgIndex[j] - 1];

                //    PublicDataClass.SaveText.Cfg[i].YxcfgState[j] = "是";
                //    PublicDataClass.SaveText.Cfg[i].Yxdataqf[j] = PublicDataClass._YxConfigParam.QufanTable[YxCfgIndex[j] - 1];

                //}

            }

        }
        private void RefreshParamState()
        {
            if (ItemId == 1)
            {
                PublicDataClass._ComParam.num = listView1.Items.Count;
                PublicDataClass._ComParam.NameTable = new string[listView1.Items.Count];
                PublicDataClass._ComParam.ValueTable = new string[listView1.Items.Count];
                PublicDataClass._ComParam.ByteTable = new string[listView1.Items.Count];


                for (int j = 0; j < listView1.Items.Count; j++)
                {
                    PublicDataClass._ComParam.NameTable[j] = listView1.Items[j].SubItems[1].Text;//取得listview某行某列的值

                    PublicDataClass._ComParam.ValueTable[j] = listView1.Items[j].SubItems[2].Text;

                    PublicDataClass._ComParam.ByteTable[j] = listView1.Items[j].SubItems[3].Text;
                }
            }
            else if (ItemId == 2)
            {
                PublicDataClass._NetParam.num = listView1.Items.Count;
                PublicDataClass._NetParam.NameTable = new string[PublicDataClass._NetParam.num];
                PublicDataClass._NetParam.ValueTable = new string[PublicDataClass._NetParam.num];
                PublicDataClass._NetParam.ByteTable = new string[PublicDataClass._NetParam.num];

                for (int j = 0; j < listView1.Items.Count; j++)
                {
                    PublicDataClass._NetParam.NameTable[j] = listView1.Items[j].SubItems[1].Text;//取得listview某行某列的值

                    PublicDataClass._NetParam.ValueTable[j] = listView1.Items[j].SubItems[2].Text;

                    PublicDataClass._NetParam.ByteTable[j] = listView1.Items[j].SubItems[3].Text;
                }
            }
            else if (ItemId == 3)
            {
                PublicDataClass._SysParam.num = listView1.Items.Count;
                PublicDataClass._SysParam.NameTable = new string[PublicDataClass._SysParam.num];
                PublicDataClass._SysParam.ValueTable = new string[PublicDataClass._SysParam.num];
                PublicDataClass._SysParam.ByteTable = new string[PublicDataClass._SysParam.num];

                for (int j = 0; j < listView1.Items.Count; j++)
                {
                    PublicDataClass._SysParam.NameTable[j] = listView1.Items[j].SubItems[1].Text;//取得listview某行某列的值

                    PublicDataClass._SysParam.ValueTable[j] = listView1.Items[j].SubItems[2].Text;

                    PublicDataClass._SysParam.ByteTable[j] = listView1.Items[j].SubItems[3].Text;
                }
            }
            else if (ItemId == 4)
            {
                YxCfgNum = 0;
                PublicDataClass._YcConfigParam.num = listView1.Items.Count;
                PublicDataClass._YcConfigParam.NameTable = new string[PublicDataClass._YcConfigParam.num];
                PublicDataClass._YcConfigParam.AddrTable = new string[PublicDataClass._YcConfigParam.num];
                PublicDataClass._YcConfigParam.IndexTable = new string[PublicDataClass._YcConfigParam.num];
                //    PublicDataClass._YcConfigParam.DatatypeTable = new string[PublicDataClass._YcConfigParam.num];
                //    PublicDataClass._YcConfigParam.MagnificationTable = new string[PublicDataClass._YcConfigParam.num];
                //   PublicDataClass._YcConfigParam.ConnectTable = new string[PublicDataClass._YcConfigParam.num];
                //   PublicDataClass._YcConfigParam.QufanTable = new string[PublicDataClass._YcConfigParam.num];
                //PublicDataClass._YcConfigParam.setvalueTable = new string[PublicDataClass._YcConfigParam.num];
                //PublicDataClass._YcConfigParam.ValueTable = new string[PublicDataClass._YcConfigParam.num];
                //   PublicDataClass._YcConfigParam.ByteTable = new string[PublicDataClass._YcConfigParam.num];

                for (int j = 0; j < listView1.Items.Count; j++)
                {

                    PublicDataClass._YcConfigParam.NameTable[j] = listView1.Items[j].SubItems[1].Text;//取得listview某行某列的值
                    PublicDataClass._YcConfigParam.AddrTable[j] = listView1.Items[j].SubItems[3].Text;

                    PublicDataClass._YcConfigParam.IndexTable[j] = listView1.Items[j].SubItems[2].Text;
                    //   PublicDataClass._YcConfigParam.DatatypeTable[j] = listView1.Items[j].SubItems[4].Text;//取得listview某行某列的值
                    //   PublicDataClass._YcConfigParam.MagnificationTable[j] = listView1.Items[j].SubItems[5].Text;
                    //   PublicDataClass._YcConfigParam.ConnectTable[j] = listView1.Items[j].SubItems[6].Text;
                    //   PublicDataClass._YcConfigParam.QufanTable[j] = listView1.Items[j].SubItems[7].Text;//取得listview某行某列的值
                    //PublicDataClass._YcConfigParam.setvalueTable[j] = listView1.Items[j].SubItems[8].Text;//取得listview某行某列的值
                    //PublicDataClass._YcConfigParam.ValueTable[j] = listView1.Items[j].SubItems[9].Text;//取得listview某行某列的值
                    //   PublicDataClass._YcConfigParam.ByteTable[j] = listView1.Items[j].SubItems[8].Text;
                    listView1.Items[j].UseItemStyleForSubItems = false;
                    if (listView1.Items[j].SubItems[3].Text == "65535")
                        listView1.Items[j].SubItems[3].ForeColor = Color.Gray;
                    //else
                    //    listView1.Items[j].SubItems[3].ForeColor = Color.Black ;
                }
            }
            else if (ItemId == 5)
            {
                YxCfgNum = 0;

                PublicDataClass._YxConfigParam.num = listView1.Items.Count;
                PublicDataClass._YxConfigParam.NameTable = new string[PublicDataClass._YxConfigParam.num];
                PublicDataClass._YxConfigParam.AddrTable = new string[PublicDataClass._YxConfigParam.num];
                PublicDataClass._YxConfigParam.IndexTable = new string[PublicDataClass._YxConfigParam.num];
                PublicDataClass._YxConfigParam.QufanTable = new string[PublicDataClass._YxConfigParam.num];
                //PublicDataClass._YxConfigParam.setvalueTable = new string[PublicDataClass._YxConfigParam.num];
                //PublicDataClass._YxConfigParam.ValueTable = new string[PublicDataClass._YxConfigParam.num];
                PublicDataClass._YxConfigParam.ByteTable = new string[PublicDataClass._YxConfigParam.num];
                for (int j = 0; j < listView1.Items.Count; j++)
                {
                    PublicDataClass._YxConfigParam.NameTable[j] = listView1.Items[j].SubItems[1].Text;//取得listview某行某列的值
                    PublicDataClass._YxConfigParam.AddrTable[j] = listView1.Items[j].SubItems[3].Text;
                    if (PublicDataClass._YxConfigParam.AddrTable[j] != "65535")
                        YxCfgNum++;//实际配置的遥信数--2013.8.16
                    PublicDataClass._YxConfigParam.IndexTable[j] = listView1.Items[j].SubItems[2].Text;

                    PublicDataClass._YxConfigParam.QufanTable[j] = listView1.Items[j].SubItems[4].Text;//取得listview某行某列的值
                    //PublicDataClass._YxConfigParam.setvalueTable[j] = listView1.Items[j].SubItems[5].Text;
                    //PublicDataClass._YxConfigParam.ValueTable[j] = listView1.Items[j].SubItems[6].Text;
                    PublicDataClass._YxConfigParam.ByteTable[j] = listView1.Items[j].SubItems[5].Text;


                }

            }
            else if (ItemId == 6)
            {
                PublicDataClass._YkConfigParam.num = listView1.Items.Count;
                PublicDataClass._YkConfigParam.NameTable = new string[PublicDataClass._YkConfigParam.num];
                PublicDataClass._YkConfigParam.AddrTable = new string[PublicDataClass._YkConfigParam.num];
                PublicDataClass._YkConfigParam.triggermodeTable = new string[PublicDataClass._YkConfigParam.num];
                PublicDataClass._YkConfigParam.secltimeTable = new string[PublicDataClass._YkConfigParam.num];
                PublicDataClass._YkConfigParam.exetimeTable = new string[PublicDataClass._YkConfigParam.num];
                PublicDataClass._YkConfigParam.pulsewidthTable = new string[PublicDataClass._YkConfigParam.num];
                PublicDataClass._YkConfigParam.saveflagTable = new string[PublicDataClass._YkConfigParam.num];

                PublicDataClass._YkConfigParam.powerTable = new string[PublicDataClass._YkConfigParam.num];
                PublicDataClass._YkConfigParam.jdq1Table = new string[PublicDataClass._YkConfigParam.num];
                PublicDataClass._YkConfigParam.jdq2Table = new string[PublicDataClass._YkConfigParam.num];
                PublicDataClass._YkConfigParam.fjyx1Table = new string[PublicDataClass._YkConfigParam.num];
                PublicDataClass._YkConfigParam.fjyx2Table = new string[PublicDataClass._YkConfigParam.num];

                PublicDataClass._YkConfigParam.ByteTable = new string[PublicDataClass._YkConfigParam.num];

                for (int j = 0; j < listView1.Items.Count; j++)
                {
                    PublicDataClass._YkConfigParam.NameTable[j] = listView1.Items[j].SubItems[1].Text;//取得listview某行某列的值
                    PublicDataClass._YkConfigParam.AddrTable[j] = listView1.Items[j].SubItems[2].Text;

                    PublicDataClass._YkConfigParam.triggermodeTable[j] = listView1.Items[j].SubItems[3].Text;
                    PublicDataClass._YkConfigParam.secltimeTable[j] = listView1.Items[j].SubItems[4].Text;//取得listview某行某列的值
                    PublicDataClass._YkConfigParam.exetimeTable[j] = listView1.Items[j].SubItems[5].Text;
                    PublicDataClass._YkConfigParam.pulsewidthTable[j] = listView1.Items[j].SubItems[6].Text;
                    PublicDataClass._YkConfigParam.saveflagTable[j] = listView1.Items[j].SubItems[7].Text;

                    PublicDataClass._YkConfigParam.powerTable[j] = listView1.Items[j].SubItems[8].Text;
                    PublicDataClass._YkConfigParam.jdq1Table[j] = listView1.Items[j].SubItems[9].Text;//取得listview某行某列的值
                    PublicDataClass._YkConfigParam.jdq2Table[j] = listView1.Items[j].SubItems[10].Text;
                    PublicDataClass._YkConfigParam.fjyx1Table[j] = listView1.Items[j].SubItems[11].Text;
                    PublicDataClass._YkConfigParam.fjyx2Table[j] = listView1.Items[j].SubItems[12].Text;

                    PublicDataClass._YkConfigParam.ByteTable[j] = listView1.Items[j].SubItems[13].Text;
                }
            }
            else if ((ItemId == 7) || (ItemId == 8) || (ItemId == 9))
            {
                PublicDataClass._AIParam.num = listView1.Items.Count;
                PublicDataClass._AIParam.quality = new string[PublicDataClass._AIParam.num];
                PublicDataClass._AIParam.phase = new string[PublicDataClass._AIParam.num];
                PublicDataClass._AIParam.line = new string[PublicDataClass._AIParam.num];
                PublicDataClass._AIParam.panel = new string[PublicDataClass._AIParam.num];
                PublicDataClass._AIParam.value = new string[PublicDataClass._AIParam.num];
                PublicDataClass._AIParam.ph = new string[PublicDataClass._AIParam.num];
                PublicDataClass._AIParam.zhishu = new string[PublicDataClass._AIParam.num];

                for (int j = 0; j < listView1.Items.Count; j++)
                {
                    PublicDataClass._AIParam.quality[j] = listView1.Items[j].SubItems[1].Text;//取得listview某行某列的值
                    PublicDataClass._AIParam.phase[j] = listView1.Items[j].SubItems[2].Text;
                    PublicDataClass._AIParam.line[j] = listView1.Items[j].SubItems[3].Text;
                    PublicDataClass._AIParam.panel[j] = listView1.Items[j].SubItems[4].Text;//取得listview某行某列的值
                    PublicDataClass._AIParam.value[j] = listView1.Items[j].SubItems[5].Text;
                    PublicDataClass._AIParam.ph[j] = listView1.Items[j].SubItems[6].Text;
                    PublicDataClass._AIParam.zhishu[j] = listView1.Items[j].SubItems[7].Text;
                }

            }

            else if (ItemId - 0x14 >= 0)  //动态选项卡
            {
                PublicDataClass.TabCfg[ItemId - 0x14].LineNum = listViewtest.Items.Count;
                if (PublicDataClass.TabCfg[ItemId - 0x14].PageName == "遥信置数")
                {
                    PublicDataClass.TabCfg[ItemId - 0x14].LineNum = PublicDataClass._YxConfigParam.wyxnum;
                    //if (PublicDataClass._ChangeFlag.YxzsCfg == true)
                    {
                        for (int j = 0; j < YxCfgNum; j++)
                        {
                            if (YxCfgIndex[j] <= PublicDataClass._YxConfigParam.wyxnum)
                                PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[2].ValueTable[YxCfgIndex[j] - 1] = listViewtest.Items[j].SubItems[2].Text;


                        }

                        return;
                    }

                }

                for (int j = 0; j < PublicDataClass.TabCfg[ItemId - 0x14].ColumnNum; j++)
                {

                    PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[j].ValueTable = new string[PublicDataClass.TabCfg[ItemId - 0x14].LineNum];
                    for (int q = 0; q < PublicDataClass.TabCfg[ItemId - 0x14].LineNum; q++)
                    {
                        PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[j].ValueTable[q] = listViewtest.Items[q].SubItems[j].Text;
                    }
                }

            }

        }






        /// <summary>
        /// 下载参数按钮的事件响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 返回类型-void
        private void downloadbutton_Click(object sender, EventArgs e)
        {
            PublicDataClass._Message.ParamAck = false;
            try
            {
                if (comboBox1.Text == "无信息")
                {
                    MessageBox.Show("无测量点信息可操作", "信息",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
                else
                {
                    ty = PublicFunction.CheckPointOfCommunicationEntrace(comboBox1.Text);
                    if (ty == 0)
                    {
                        MessageBox.Show("无测量点信息可操作", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;

                    }

                }
                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 0;
                PublicDataClass.seq = 0;
                if (ItemId == 1)  //下载串口参数
                {
                    PublicDataClass.SQflag = 0;
                    dataPos = 0;
                    for (int i = 0; i < PublicDataClass._ComParam.num; i++)
                    {
                        if (PublicDataClass._ComParam.ByteTable[i] == "1")
                        {
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(PublicDataClass._ComParam.ValueTable[i]);
                            PublicDataClass._DataField.FieldLen++;

                        }
                        else if (PublicDataClass._ComParam.ByteTable[i] == "2")
                        {
                            //InttoByte(Convert.ToInt32(PublicDataClass._ComParam.ValueTable[i]), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._ComParam.ValueTable[i])) & 0x00ff);
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(PublicDataClass._ComParam.ValueTable[i])) & 0xff00) >> 8);
                            PublicDataClass._DataField.FieldLen += 2;
                        }
                        else if (PublicDataClass._ComParam.ByteTable[i] == "4")
                        {

                            byte[] b = BitConverter.GetBytes(float.Parse(PublicDataClass._ComParam.ValueTable[i]));

                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = b[0];
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = b[1];
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = b[2];
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 3] = b[3];

                            PublicDataClass._DataField.FieldLen += 4;
                        }
                        PublicDataClass._DataField.FieldVSQ++;
                        if ((PublicDataClass._DataField.FieldLen >= PublicDataClass.Framelen) && (i < PublicDataClass._ComParam.num - 1))      //一帧的长度
                        {
                            dataPos = i;
                            Pos = PublicDataClass._DataField.FieldVSQ;
                            timer1.Enabled = true;
                            PublicDataClass.seqflag = 1;
                            break;

                        }
                        PublicDataClass.seqflag = 0;
                    }
                    PublicDataClass.seq = 1;
                    PublicDataClass.ParamInfoAddr = 0x0100;
                }
                else if (ItemId == 2) //网口参数
                {
                    try
                    {
                        PublicDataClass.SQflag = 0;
                        for (int i = 0; i < PublicDataClass._NetParam.num; i++)
                        {
                            if (PublicDataClass._NetParam.ByteTable[i] == "1")
                            {
                                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(PublicDataClass._NetParam.ValueTable[i]);
                                PublicDataClass._DataField.FieldLen++;

                            }
                            else if (PublicDataClass._NetParam.ByteTable[i] == "2")
                            {
                                //InttoByte(Convert.ToInt32(PublicDataClass._NetParam.ValueTable[i]), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._NetParam.ValueTable[i])) & 0x00ff);
                                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(PublicDataClass._NetParam.ValueTable[i])) & 0xff00) >> 8);
                                PublicDataClass._DataField.FieldLen += 2;
                            }
                            else if (PublicDataClass._NetParam.ByteTable[i] == "4")
                            {
                                string text = @"";
                                text = PublicDataClass._NetParam.ValueTable[i];
                                for (int k = 0; k < 4; k++)
                                {
                                    if (k < 3)
                                    {
                                        int a = text.IndexOf(".");
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(text.Substring(0, a));
                                        text = text.Remove(0, a + 1);
                                    }
                                    else
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(text);
                                    PublicDataClass._DataField.FieldLen++;

                                }
                            }
                            else if (PublicDataClass._NetParam.ByteTable[i] == "6")
                            {
                                string text = @"";
                                text = PublicDataClass._NetParam.ValueTable[i];
                                for (int k = 0; k < 6; k++)
                                {
                                    if (k < 5)
                                    {
                                        //int a = text.IndexOf("-");
                                        //string cs = text.Substring(0, a);
                                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = PublicFunction.StringToByte(cs);

                                        int a = text.IndexOf("-");
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(text.Substring(0, a));
                                        text = text.Remove(0, a + 1);

                                    }
                                    else
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(text);
                                    PublicDataClass._DataField.FieldLen++;

                                }
                            }
                            PublicDataClass._DataField.FieldVSQ++;
                        }
                        PublicDataClass.seqflag = 0;
                        PublicDataClass.seq = 1;
                        PublicDataClass.ParamInfoAddr = 0x0200;
                    }
                    catch (System.ArgumentOutOfRangeException)
                    {
                        MessageBox.Show("参数输入格式不对！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else if (ItemId == 3)//系统参数
                {
                    PublicDataClass.SQflag = 0;
                    dataPos = 0;
                    for (int i = 0; i < PublicDataClass._SysParam.num; i++)
                    {
                        if (PublicDataClass._SysParam.ByteTable[i] == "1")
                        {
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(PublicDataClass._SysParam.ValueTable[i]);
                            PublicDataClass._DataField.FieldLen++;

                        }
                        else if (PublicDataClass._SysParam.ByteTable[i] == "2")
                        {
                            //InttoByte(Convert.ToInt32(PublicDataClass._SysParam.ValueTable[i]), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._SysParam.ValueTable[i])) & 0x00ff);
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(PublicDataClass._SysParam.ValueTable[i])) & 0xff00) >> 8);
                            PublicDataClass._DataField.FieldLen += 2;
                        }
                        else if (PublicDataClass._SysParam.ByteTable[i] == "4")
                        {

                            byte[] b = BitConverter.GetBytes(float.Parse(PublicDataClass._SysParam.ValueTable[i]));

                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = b[0];
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = b[1];
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = b[2];
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 3] = b[3];

                            PublicDataClass._DataField.FieldLen += 4;
                        }
                        PublicDataClass._DataField.FieldVSQ++;
                        if ((PublicDataClass._DataField.FieldLen >= PublicDataClass.Framelen - 3) && (i < PublicDataClass._SysParam.num - 1))      //一帧的长度
                        {
                            dataPos = i;
                            Pos = PublicDataClass._DataField.FieldVSQ;
                            timer1.Enabled = true;
                            PublicDataClass.seqflag = 1;
                            break;

                        }
                        PublicDataClass.seqflag = 0;
                    }
                    PublicDataClass.seq = 1;
                    PublicDataClass.ParamInfoAddr = 0x0300;


                }
                else if (ItemId == 4) //遥测配置
                {
                    PublicDataClass.SQflag = 0;
                    dataPos = 0;
                    int index = 0, infadr = 0;
                    byte datatype = 0, magnify = 0, linemode = 0, qufanflag = 0;
                    for (int i = 0; i < PublicDataClass._YcConfigParam.num; i++)
                    {
                        index = Convert.ToInt32(PublicDataClass._YcConfigParam.IndexTable[i]);
                        infadr = Convert.ToInt32(PublicDataClass._YcConfigParam.AddrTable[i]);

                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((index) & 0x00ff);
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((index) & 0xff00) >> 8);
                        //PublicDataClass._DataField.FieldLen += 2;

                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((infadr) & 0x00ff);
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((infadr) & 0xff00) >> 8);
                        PublicDataClass._DataField.FieldLen += 2;

                        PublicDataClass._DataField.FieldVSQ++;
                        if ((PublicDataClass._DataField.FieldLen >= PublicDataClass.Framelen - 1) && (i < PublicDataClass._YcConfigParam.num - 1))      //一帧的长度
                        {
                            dataPos = i;
                            Pos = PublicDataClass._DataField.FieldVSQ;
                            timer1.Enabled = true;
                            PublicDataClass.seqflag = 1;
                            break;

                        }
                        PublicDataClass.seqflag = 0;
                    }
                    //int ycadr = 16385;
                    //for (int k = 0; k < PublicDataClass._YcConfigParam.num; k++)
                    //{
                    //    for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                    //    {
                    //        try
                    //        {
                    //            if (Convert.ToInt32(PublicDataClass._YcConfigParam.AddrTable[j]) == ycadr)
                    //            {
                    //                infadr = j;
                    //                ycadr++;
                    //                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((infadr) & 0x00ff);
                    //                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((infadr) & 0xff00) >> 8);
                    //                PublicDataClass._DataField.FieldLen += 2;

                    //                PublicDataClass._DataField.FieldVSQ++;
                    //                if (PublicDataClass._DataField.FieldLen >= 800)      //一帧的长度
                    //                {
                    //                    dataPos = ycadr;
                    //                    Pos = PublicDataClass._DataField.FieldVSQ;
                    //                    timer1.Enabled = true;
                    //                    PublicDataClass.seqflag = 1;
                    //                    break;

                    //                }
                    //                PublicDataClass.seqflag = 0;
                    //            }
                    //        }
                    //        catch
                    //        { }
                    //    }
                    //} 
                    PublicDataClass.seq = 1;
                    PublicDataClass.ParamInfoAddr = 0x5000;

                }
                else if (ItemId == 5) //遥信配置参数
                {
                    PublicDataClass.SQflag = 0;
                    dataPos = 0;
                    int index = 0, infadr = 0;
                    byte qufanflag = 0, setflag = 0, value = 0;
                    for (int i = 0; i < PublicDataClass._YxConfigParam.num; i++)
                    {
                        index = Convert.ToInt32(PublicDataClass._YxConfigParam.IndexTable[i]);
                        infadr = Convert.ToInt32(PublicDataClass._YxConfigParam.AddrTable[i]);

                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((index) & 0x00ff);
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((index) & 0xff00) >> 8);
                        //PublicDataClass._DataField.FieldLen += 2;

                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((infadr) & 0x00ff);
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((infadr) & 0xff00) >> 8);
                        PublicDataClass._DataField.FieldLen += 2;

                        if (PublicDataClass._YxConfigParam.QufanTable[i] == "否")
                            qufanflag = 0;
                        else if (PublicDataClass._YxConfigParam.QufanTable[i] == "是")
                            qufanflag = 1;

                        //if (PublicDataClass._YxConfigParam.setvalueTable[i] == "否")
                        //    setflag = 0;
                        //else if (PublicDataClass._YxConfigParam.setvalueTable[i] == "是")
                        //    setflag = 1;

                        //value = Convert.ToByte(PublicDataClass._YxConfigParam.ValueTable[i]);

                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((value << 2) + (setflag << 1) + (qufanflag));
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)(qufanflag);
                        PublicDataClass._DataField.FieldLen += 1;

                        PublicDataClass._DataField.FieldVSQ++;
                        if ((PublicDataClass._DataField.FieldLen >= PublicDataClass.Framelen - 2) && (i < PublicDataClass._YxConfigParam.num - 1))      //一帧的长度
                        {
                            dataPos = i;
                            Pos = PublicDataClass._DataField.FieldVSQ;
                            timer1.Enabled = true;
                            PublicDataClass.seqflag = 1;
                            break;

                        }
                        PublicDataClass.seqflag = 0;
                    }
                    PublicDataClass.seq = 1;
                    PublicDataClass.ParamInfoAddr = 0x6000;
                }
                else if (ItemId == 6) //遥控配置
                {
                    PublicDataClass.SQflag = 0;
                    dataPos = 0;
                    int triggermodeTable, secltimeTable, exetimeTable, pulsewidthTable, saveflagTable, powerTable, jdq1Table, jdq2Table, fjyx1Table, fjyx2Table;


                    for (int i = 0; i < PublicDataClass._YkConfigParam.num; i++)
                    {
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._YkConfigParam.AddrTable[i])) & 0x00ff);
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(PublicDataClass._YkConfigParam.AddrTable[i])) & 0xff00) >> 8);
                        PublicDataClass._DataField.FieldLen += 2;

                        if (PublicDataClass._YkConfigParam.triggermodeTable[i] == "电平")
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0;
                        else if (PublicDataClass._YkConfigParam.triggermodeTable[i] == "脉冲")
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 1;
                        PublicDataClass._DataField.FieldLen += 1;

                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._YkConfigParam.secltimeTable[i])) & 0x00ff);
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(PublicDataClass._YkConfigParam.secltimeTable[i])) & 0xff00) >> 8);
                        PublicDataClass._DataField.FieldLen += 2;

                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._YkConfigParam.exetimeTable[i])) & 0x00ff);
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(PublicDataClass._YkConfigParam.exetimeTable[i])) & 0xff00) >> 8);
                        PublicDataClass._DataField.FieldLen += 2;

                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._YkConfigParam.pulsewidthTable[i])) & 0x00ff);
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(PublicDataClass._YkConfigParam.pulsewidthTable[i])) & 0xff00) >> 8);
                        PublicDataClass._DataField.FieldLen += 2;

                        if (PublicDataClass._YkConfigParam.saveflagTable[i] == "否")
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0;
                        else if (PublicDataClass._YkConfigParam.saveflagTable[i] == "是")
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 1;
                        PublicDataClass._DataField.FieldLen += 1;

                        if (PublicDataClass._YkConfigParam.powerTable[i] == "无")
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0;
                        else if (PublicDataClass._YkConfigParam.powerTable[i] == "24V")
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 1;
                        else if (PublicDataClass._YkConfigParam.powerTable[i] == "电操机构")
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 2;
                        else if (PublicDataClass._YkConfigParam.powerTable[i] == "电源+电操")
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 3;
                        PublicDataClass._DataField.FieldLen += 1;

                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._YkConfigParam.jdq1Table[i])) & 0x00ff);
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(PublicDataClass._YkConfigParam.jdq1Table[i])) & 0xff00) >> 8);
                        //PublicDataClass._DataField.FieldLen += 2;

                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._YkConfigParam.jdq2Table[i])) & 0x00ff);
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(PublicDataClass._YkConfigParam.jdq2Table[i])) & 0xff00) >> 8);
                        //PublicDataClass._DataField.FieldLen += 2;

                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._YkConfigParam.fjyx1Table[i])) & 0x00ff);
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(PublicDataClass._YkConfigParam.fjyx1Table[i])) & 0xff00) >> 8);
                        //PublicDataClass._DataField.FieldLen += 2;

                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._YkConfigParam.fjyx2Table[i])) & 0x00ff);
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(PublicDataClass._YkConfigParam.fjyx2Table[i])) & 0xff00) >> 8);
                        //PublicDataClass._DataField.FieldLen += 2;
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._YkConfigParam.jdq1Table[i])) & 0x00ff);
                        PublicDataClass._DataField.FieldLen += 1;

                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._YkConfigParam.jdq2Table[i])) & 0x00ff);
                        PublicDataClass._DataField.FieldLen += 1;

                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._YkConfigParam.fjyx1Table[i])) & 0x00ff);
                        PublicDataClass._DataField.FieldLen += 1;

                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._YkConfigParam.fjyx2Table[i])) & 0x00ff);
                        PublicDataClass._DataField.FieldLen += 1;


                        PublicDataClass._DataField.FieldVSQ++;
                        if ((PublicDataClass._DataField.FieldLen >= PublicDataClass.Framelen - 14) && (i < PublicDataClass._YkConfigParam.num - 1))      //一帧的长度
                        {
                            dataPos = i;
                            Pos = PublicDataClass._DataField.FieldVSQ;
                            timer1.Enabled = true;
                            PublicDataClass.seqflag = 1;
                            break;

                        }
                        PublicDataClass.seqflag = 0;
                    }
                    PublicDataClass.seq = 1;
                    PublicDataClass.ParamInfoAddr = 0x7000;

                }
                if (ItemId == 7)  //下载模拟量配置参数
                {
                    PublicDataClass.SQflag = 0;
                    dataPos = 0;
                    for (int i = 0; i < PublicDataClass._AIParam.num; i++)
                    {
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)(Convert.ToChar(PublicDataClass._AIParam.quality[i]));
                        PublicDataClass._DataField.FieldLen++;

                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)(Convert.ToChar(PublicDataClass._AIParam.phase[i]));
                        PublicDataClass._DataField.FieldLen++;

                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (Convert.ToByte(PublicDataClass._AIParam.line[i]));
                        PublicDataClass._DataField.FieldLen++;

                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (Convert.ToByte(PublicDataClass._AIParam.panel[i]));
                        PublicDataClass._DataField.FieldLen++;

                        PublicDataClass._DataField.FieldVSQ++;
                        if ((PublicDataClass._DataField.FieldLen >= PublicDataClass.Framelen - 4) && (i < PublicDataClass._AIParam.num - 1))      //一帧的长度
                        {
                            dataPos = i;
                            Pos = PublicDataClass._DataField.FieldVSQ;
                            timer1.Enabled = true;
                            PublicDataClass.seqflag = 1;
                            break;

                        }
                        PublicDataClass.seqflag = 0;
                    }
                    PublicDataClass.seq = 1;
                    PublicDataClass.ParamInfoAddr = 0x400;
                }
                if (ItemId == 8)  //下载模拟量校准参数
                {
                    PublicDataClass.SQflag = 0;
                    dataPos = 0;
                    for (int i = 0; i < PublicDataClass._AIParam.num; i++)
                    {
                        byte[] b = BitConverter.GetBytes(float.Parse(PublicDataClass._AIParam.value[i]));
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = b[0];
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = b[1];
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = b[2];
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 3] = b[3];
                        PublicDataClass._DataField.FieldLen += 4;

                        b = BitConverter.GetBytes(float.Parse(PublicDataClass._AIParam.ph[i]));
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = b[0];
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = b[1];
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = b[2];
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 3] = b[3];
                        PublicDataClass._DataField.FieldLen += 4;
                        PublicDataClass._DataField.FieldVSQ++;

                        if ((PublicDataClass._DataField.FieldLen >= PublicDataClass.Framelen - 7) && (i < PublicDataClass._AIParam.num - 1))      //一帧的长度
                        //if ((PublicDataClass._DataField.FieldLen >= 800 - 7 - 6) && (i < PublicDataClass._AIParam.num - 1))      //一帧的长度
                        {
                            dataPos = i;
                            Pos = PublicDataClass._DataField.FieldVSQ;
                            timer1.Enabled = true;
                            PublicDataClass.seqflag = 1;
                            break;

                        }
                        PublicDataClass.seqflag = 0;
                    }
                    PublicDataClass.seq = 1;
                    if (checkBox2.Checked == true)
                        PublicDataClass.ParamInfoAddr = 0xd000;
                    else if (checkBox3.Checked == true)
                        PublicDataClass.ParamInfoAddr = 0xe000;
                    else if (checkBox4.Checked == true)
                        PublicDataClass.ParamInfoAddr = 0xf000;


                }
                if (ItemId == 9)  //下载模拟量置数参数
                {
                    PublicDataClass.SQflag = 0;
                    dataPos = 0;
                    for (int i = 0; i < PublicDataClass._AIParam.num; i++)
                    {

                        //byte[] b = BitConverter.GetBytes(float.Parse(PublicDataClass._AIParam.zhishu[i]));
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = b[0];
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = b[1];
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = b[2];
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 3] = b[3];
                        //PublicDataClass._DataField.FieldLen += 4;
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._AIParam.zhishu[i]))& 0x00ff);
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(PublicDataClass._AIParam.zhishu[i])) & 0xff00) >> 8);
                       // InttoByte(Convert.ToInt32(PublicDataClass._AIParam.zhishu[i]), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                        PublicDataClass._DataField.FieldLen += 2;

                     

                        PublicDataClass._DataField.FieldVSQ++;
                        if ((PublicDataClass._DataField.FieldLen >= PublicDataClass.Framelen - 3) && (i < PublicDataClass._AIParam.num - 1))      //一帧的长度
                        //if ((PublicDataClass._DataField.FieldLen >= 800 - 3) && (i < PublicDataClass._AIParam.num - 1))      //一帧的长度
                        {
                            dataPos = i;
                            Pos = PublicDataClass._DataField.FieldVSQ;
                            timer1.Enabled = true;
                            PublicDataClass.seqflag = 1;
                            break;

                        }
                        PublicDataClass.seqflag = 0;
                    }
                    PublicDataClass.seq = 1;
                    PublicDataClass.ParamInfoAddr = 0x800;
                }
                else if (ItemId - 0x14 >= 0)  //动态选项卡
                {
                    PublicDataClass.SQflag = 0;
                    dataPos = 0;
                    int YCcount = 0;
                    if (PublicDataClass.TabCfg[ItemId - 0x14].PageName == "内遥信配置参数")
                    {
                        for (int q = 0; q < PublicDataClass.TabCfg[ItemId - 0x14].LineNum; q++)
                        {
                            for (int col = 0; col < PublicDataClass.TabCfg[ItemId - 0x14].ColumnNum; col++)
                            {

                                try
                                {
                                    if (PublicDataClass.TabCfg[ItemId - 0x14].ColumnDownByte[col] == "1")
                                    {
                                        //int value;
                                        //bool flag = int.TryParse(PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q], out value);
                                        //if (flag == true)
                                        //    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q]);
                                        //else
                                        //    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)(Convert.ToChar(PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q]));
                                        if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "过流II段")
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 1;
                                        else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "过流I段")
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 2;
                                        else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "零序告警")
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 3;
                                        else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "电流过负荷")
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 4;
                                        else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "过压告警")
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 5;
                                        else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "欠压告警")
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 6;
                                        else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "断线")
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 7;
                                        else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "单相接地")
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 8;
                                        else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "相间短路")
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 9;
                                        else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "事故总")
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 10;
                                        else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "过流II段线路总信号")
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 11;
                                        else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "过流I段线路总信号")
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 12;
                                        else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "相间短路总信号")
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 13;
                                        else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "AB")
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)(Convert.ToChar("C"));
                                        else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "BC")
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)(Convert.ToChar("A"));
                                        else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "AC")
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)(Convert.ToChar("B"));
                                        else
                                        {
                                            int value;
                                            bool flag = int.TryParse(PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q], out value);
                                            if (flag == true)
                                                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q]);
                                            else

                                                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)(Convert.ToChar(PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q]));
                                        }
                                        PublicDataClass._DataField.FieldLen++;
                                    }
                                    else if (PublicDataClass.TabCfg[ItemId - 0x14].ColumnDownByte[col] == "2")
                                    {
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q]) )& 0x00ff);
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q])) & 0xff00) >> 8);
                                        //InttoByte(Convert.ToInt32(PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q]), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                                        PublicDataClass._DataField.FieldLen += 2;
  
                                    }
                                    else if (PublicDataClass.TabCfg[ItemId - 0x14].ColumnDownByte[col] == "4")
                                    {

                                        byte[] b = BitConverter.GetBytes(float.Parse(PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q]));

                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = b[0];
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = b[1];
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = b[2];
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 3] = b[3];


                                        //  InttoByte(Convert.ToInt16(PublicDataClass._RaoDong.ValueTable[i]), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                                        PublicDataClass._DataField.FieldLen += 4;
                                    }
                                }
                                catch
                                {
                                    YCcount++;
                                    if (YCcount == 1)
                                        //MessageBox.Show("第" + q + "行" + "第" + col + "列参数配置有误！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        MessageBox.Show("蓝色标注处参数配置有误！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    listViewtest.Items[q].UseItemStyleForSubItems = false;
                                    listViewtest.Items[q].SubItems[col].ForeColor = Color.Blue;

                                }
                            }
                            PublicDataClass._DataField.FieldVSQ++;
                            if ((PublicDataClass._DataField.FieldLen >= PublicDataClass.Framelen - 6))      //一帧的长度
                            {
                                dataPos = q;
                                Pos = PublicDataClass._DataField.FieldVSQ;
                                timer1.Enabled = true;
                                PublicDataClass.seqflag = 1;
                                break;

                            }
                            PublicDataClass.seqflag = 0;
                        }
                    }
                    else if (PublicDataClass.TabCfg[ItemId - 0x14].PageName == "GPRS参数")
                    {
                        string text;
                        for (int q = 0; q < PublicDataClass.TabCfg[ItemId - 0x14].LineNum; q++)
                        {
                            try
                            {
                                if (q <= 1)
                                {
                                    text = PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[2].ValueTable[q];
                                    for (int i = 0; i < 4; i++)
                                    {
                                        if (i < 3)
                                        {
                                            int a = text.IndexOf(".");
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(text.Substring(0, a));
                                            text = text.Remove(0, a + 1);
                                        }
                                        else
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(text);
                                        PublicDataClass._DataField.FieldLen++;

                                    }
                                }
                                else if (q <= 3)
                                {
                                    text = PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[2].ValueTable[q];
                                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(text)) & 0x00ff);
                                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(text)) & 0xff00) >> 8);
                                    PublicDataClass._DataField.FieldLen += 2;
                                }
                                else if (q == 4)
                                {
                                    text = PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[2].ValueTable[q];
                                    for (int i = 0; i < text.Length; i++)
                                    {
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(text[i]);
                                        PublicDataClass._DataField.FieldLen++;
                                    }
                                    if (text.Length < 16)
                                    {
                                        for (int i = text.Length; i < 16; i++)
                                        {
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(' ');
                                            PublicDataClass._DataField.FieldLen++;
                                        }
                                    }
                                }

                            }
                            catch
                            {
                                YCcount++;
                                if (YCcount == 1)
                                    //MessageBox.Show("第" + q + "行" + "第2列参数配置有误！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    MessageBox.Show("蓝色标注处参数配置有误！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                listViewtest.Items[q].UseItemStyleForSubItems = false;
                                listViewtest.Items[q].SubItems[2].ForeColor = Color.Blue;

                            }

                            PublicDataClass._DataField.FieldVSQ++;
                            PublicDataClass.seqflag = 0;
                        }
                    }
                    else
                    {
                        string thetemp;
                        for (int q = 0; q < PublicDataClass.TabCfg[ItemId - 0x14].LineNum; q++)
                        {
                            for (int col = 0; col < PublicDataClass.TabCfg[ItemId - 0x14].ColumnNum; col++)
                            {
                                try
                                {
                                    int a = PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q].IndexOf("（");
                                    if (a < 0)
                                        a = PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q].IndexOf("(");
                                    if (a > 0)
                                    {
                                        thetemp = PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q].Substring(0, a);
                                    }
                                    else
                                        thetemp = PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q];

                                    if (PublicDataClass.TabCfg[ItemId - 0x14].ColumnDownByte[col] == "1")
                                    {
                                        int value;
                                        bool flag = int.TryParse(thetemp, out value);
                                        if (flag == true)
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(thetemp);
                                        else
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)(Convert.ToChar(thetemp));
                                        PublicDataClass._DataField.FieldLen++;
                                    }
                                    else if (PublicDataClass.TabCfg[ItemId - 0x14].ColumnDownByte[col] == "2")
                                    {
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(thetemp)) & 0x00ff);
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(thetemp)) & 0xff00) >> 8);
                                        //InttoByte(Convert.ToInt32(thetemp), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                                        PublicDataClass._DataField.FieldLen += 2;
       
                                    }
                                    else if (PublicDataClass.TabCfg[ItemId - 0x14].ColumnDownByte[col] == "4")
                                    {

                                        byte[] b = BitConverter.GetBytes(float.Parse(thetemp));
                                        if (PublicDataClass.addselect == 3 && PublicDataClass.ParamInfoAddr == 200)//协处理器long
                                            b = BitConverter.GetBytes(long.Parse(thetemp));

                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = b[0];
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = b[1];
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = b[2];
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 3] = b[3];


                                        //  InttoByte(Convert.ToInt16(PublicDataClass._RaoDong.ValueTable[i]), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                                        PublicDataClass._DataField.FieldLen += 4;
                                    }
                                    else if (PublicDataClass.TabCfg[ItemId - 0x14].ColumnDownByte[col] == "按字节数下载")
                                    {
                                        int m;
                                        string zijie;
                                        for (m = 0; m < PublicDataClass.TabCfg[ItemId - 0x14].ColumnNum; m++)
                                        {
                                            if (PublicDataClass.TabCfg[ItemId - 0x14].ColumnDownByte[m] == "字节数")
                                                break;
                                        }
                                        zijie = PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[m].ValueTable[q];
                                        if (zijie == "1")
                                        {
                                            //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(thetemp);
                                            //PublicDataClass._DataField.FieldLen++;
                                            int value;
                                            bool flag = int.TryParse(thetemp, out value);
                                            if (flag == true)
                                                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(thetemp);
                                            else
                                                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)(Convert.ToChar(thetemp));
                                            PublicDataClass._DataField.FieldLen++;

                                        }
                                        else if (zijie == "2")
                                        {

                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(thetemp)) & 0x00ff);
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(thetemp)) & 0xff00) >> 8);
                                        
                                            //InttoByte(Convert.ToInt32(thetemp), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                                            PublicDataClass._DataField.FieldLen += 2;
          
                                        }
                                        else if (zijie == "4")
                                        {

                                            //for (int k = 0; k < 4; k++)
                                            //{
                                            //    if (k < 3)
                                            //    {
                                            //        int b = thetemp.IndexOf(".");
                                            //        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(thetemp.Substring(0, b));
                                            //        thetemp = thetemp.Remove(0, b + 1);
                                            //    }
                                            //    else
                                            //        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(thetemp);
                                            //    PublicDataClass._DataField.FieldLen++;

                                            //}
                                            //byte[] b = BitConverter.GetBytes(float.Parse(thetemp));
                                            byte[] b = BitConverter.GetBytes(long.Parse(thetemp));

                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = b[0];
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = b[1];
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = b[2];
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 3] = b[3];


                                            //  InttoByte(Convert.ToInt16(PublicDataClass._RaoDong.ValueTable[i]), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                                            PublicDataClass._DataField.FieldLen += 4;
                                        }
                                        else if (zijie == "6")
                                        {

                                            for (int k = 0; k < 6; k++)
                                            {
                                                if (k < 5)
                                                {
                                                    //int a = text.IndexOf("-");
                                                    //string cs = text.Substring(0, a);
                                                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = PublicFunction.StringToByte(cs);

                                                    int b = thetemp.IndexOf("-");
                                                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(thetemp.Substring(0, b));
                                                    thetemp = thetemp.Remove(0, b + 1);

                                                }
                                                else
                                                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(thetemp);
                                                PublicDataClass._DataField.FieldLen++;

                                            }
                                        }


                                    }
                                }
                                catch
                                {
                                    YCcount++;
                                    if (YCcount == 1)
                                        MessageBox.Show("蓝色标注处参数配置有误！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    //MessageBox.Show("第" + q + "行" + "第" + col + "列参数配置有误！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    listViewtest.Items[q].UseItemStyleForSubItems = false;
                                    listViewtest.Items[q].SubItems[col].ForeColor = Color.Blue;

                                }
                            }
                            PublicDataClass._DataField.FieldVSQ++;
                            if ((PublicDataClass._DataField.FieldLen >= PublicDataClass.Framelen - 10))      //一帧的长度,
                            {
                                dataPos = q;
                                Pos = PublicDataClass._DataField.FieldVSQ;
                                timer1.Enabled = true;
                                PublicDataClass.seqflag = 1;
                                break;

                            }
                            PublicDataClass.seqflag = 0;
                        }
                    }
                    PublicDataClass.seq = 1;
                    PublicDataClass.ParamInfoAddr = PublicDataClass.TabCfg[ItemId - 0x14].DownAddr;//待定

                }

                if (ty == 1)
                    PublicDataClass._ComTaskFlag.SET_PARAM_CON = true;

                if (ty == 2)
                    PublicDataClass._NetTaskFlag.SET_PARAM_CON = true;
                if (ty == 3)
                    PublicDataClass._GprsTaskFlag.SET_PARAM_CON = true;
                //}
            }
            catch (OverflowException ee)                  //异常 
            {
                MessageBox.Show(ee.Message, "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (FormatException ee)                  //异常 
            {
                MessageBox.Show(ee.Message, "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {

            }
        }
        /// <summary>
        /// 按键--按下的处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        private void listView1_SubItemClicked(object sender, SubItemEventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
            if (e.SubItem == 0) // Password field
            {
                return;
            }

            else
            {
                if (ItemId < 3)
                {
                    if (e.SubItem != 2) // Password field

                        return;

                    else
                        listView1.StartEditing(Editors[e.SubItem], e.Item, e.SubItem);
                }
                else if (ItemId == 3)
                {
                    if (e.SubItem == 2)
                    {
                        if (listView1.Items[e.Item.Index].SubItems[1].Text.IndexOf("功率与电压配对号") >= 0)
                        {
                            SysParSet cfm = new SysParSet();
                            PublicDataClass._SysParam.selindex = e.Item.Index;
                            cfm.ShowDialog();
                            if (cfm.DialogResult == DialogResult.OK)               //判断是否按下-确定-按钮
                            {
                                listView1.Items[e.Item.Index].SubItems[2].Text = PublicDataClass._SysParam.ValueTable[e.Item.Index];
                                RefreshParamState();
                            }
                        }
                        else
                            listView1.StartEditing(Editors[e.SubItem], e.Item, e.SubItem);

                    }
                }
                else if (ItemId == 4)
                    listView1.StartEditing(Editors1[e.SubItem], e.Item, e.SubItem);
                else if (ItemId == 5)
                    listView1.StartEditing(Editors2[e.SubItem], e.Item, e.SubItem);
                else if (ItemId == 6)
                {
                    if (e.SubItem == 2)
                    {
                        YkqualityConfig cfm = new YkqualityConfig();
                        PublicDataClass._YkConfigParam.selindex = e.Item.Index;
                        cfm.ShowDialog();
                        if (cfm.DialogResult == DialogResult.OK)               //判断是否按下-确定-按钮
                        {
                            listView1.Items[e.Item.Index].SubItems[2].Text = PublicDataClass._YkConfigParam.AddrTable[e.Item.Index];
                            RefreshParamState();
                        }
                    }
                    else
                        listView1.StartEditing(Editors3[e.SubItem], e.Item, e.SubItem);
                }
                else if ((ItemId == 7) && (downloadbutton.Enabled == true))
                {
                    if (e.SubItem > 4) // Password field

                        return;

                    else
                        listView1.StartEditing(Editors4[e.SubItem], e.Item, e.SubItem);
                }
                else if ((ItemId == 8) && (downloadbutton.Enabled == true))
                {
                    if ((e.SubItem <= 4) || (e.SubItem > 6))// Password field

                        return;

                    else
                        listView1.StartEditing(Editors4[e.SubItem], e.Item, e.SubItem);
                }
                else if ((ItemId == 9) && (downloadbutton.Enabled == true))
                {
                    if ((e.SubItem < 7))// Password field

                        return;

                    else
                        listView1.StartEditing(Editors4[e.SubItem], e.Item, e.SubItem);
                }

            }
        }


        /// <summary>
        /// listview的鼠标单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
            foreach (ListViewItem item in this.listView1.Items)
            {
                item.ForeColor = SystemColors.WindowText;

            }
            this.listView1.SelectedItems[0].ForeColor = Color.Red;//设置当前选择项为红色
        }
        /// <summary>
        /// textbox离开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxvalue_Leave(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 多帧传送
        /// </summary>
        /// <param name="mid"></param>
        private void StartMangFrameTransmit(byte mid)     //等待应答后再发
        {
            if (mid == 1)
            {
                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 0;
                PublicDataClass.ParamInfoAddr += Pos;
                PublicDataClass.seq++;
                for (int i = dataPos + 1; i < PublicDataClass._ComParam.num; i++)
                {
                    if (PublicDataClass._ComParam.ByteTable[i] == "1")
                    {
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(PublicDataClass._ComParam.ValueTable[i]);
                        PublicDataClass._DataField.FieldLen++;

                    }
                    else if (PublicDataClass._ComParam.ByteTable[i] == "2")
                    {
                        //InttoByte(Convert.ToInt32(PublicDataClass._ComParam.ValueTable[i]), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._ComParam.ValueTable[i])) & 0x00ff);
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(PublicDataClass._ComParam.ValueTable[i])) & 0xff00) >> 8);
                        PublicDataClass._DataField.FieldLen += 2;
                    }
                    else if (PublicDataClass._ComParam.ByteTable[i] == "4")
                    {

                        byte[] b = BitConverter.GetBytes(float.Parse(PublicDataClass._ComParam.ValueTable[i]));

                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = b[0];
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = b[1];
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = b[2];
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 3] = b[3];

                        PublicDataClass._DataField.FieldLen += 4;
                    }

                    PublicDataClass._DataField.FieldVSQ++;
                    if ((PublicDataClass._DataField.FieldLen >= PublicDataClass.Framelen) && (i < PublicDataClass._ComParam.num - 1))      //一帧的长度
                    {
                        dataPos = i;
                        Pos = PublicDataClass._DataField.FieldVSQ;
                        timer1.Enabled = true;
                        PublicDataClass.seqflag = 1;
                        break;

                    }
                    PublicDataClass.seqflag = 0;

                }

            }
            else if (mid == 4)
            {
                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 0;
                PublicDataClass.ParamInfoAddr += Pos;
                PublicDataClass.seq++;
                int index = 0, infadr = 0;
                byte datatype = 0, magnify = 0, linemode = 0, qufanflag = 0;
                for (int i = dataPos + 1; i < PublicDataClass._YcConfigParam.num; i++)
                {
                    index = Convert.ToInt32(PublicDataClass._YcConfigParam.IndexTable[i]);
                    infadr = Convert.ToInt32(PublicDataClass._YcConfigParam.AddrTable[i]);

                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((infadr) & 0x00ff);
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((infadr) & 0xff00) >> 8);
                    PublicDataClass._DataField.FieldLen += 2;

                    PublicDataClass._DataField.FieldVSQ++;
                    if ((PublicDataClass._DataField.FieldLen >= PublicDataClass.Framelen - 1) && (i < PublicDataClass._YcConfigParam.num - 1))      //一帧的长度
                    {
                        dataPos = i;
                        Pos = PublicDataClass._DataField.FieldVSQ;
                        timer1.Enabled = true;
                        PublicDataClass.seqflag = 1;
                        break;

                    }
                    PublicDataClass.seqflag = 0;
                }
                //int ycadr = dataPos;
                //for (int k = 0; k < PublicDataClass._YcConfigParam.num; k++)
                //{
                //    for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                //    {
                //        try
                //        {
                //            if (Convert.ToInt32(PublicDataClass._YcConfigParam.AddrTable[j]) == ycadr)
                //            {
                //                infadr = j;
                //                ycadr++;
                //                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((infadr) & 0x00ff);
                //                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((infadr) & 0xff00) >> 8);
                //                PublicDataClass._DataField.FieldLen += 2;

                //                PublicDataClass._DataField.FieldVSQ++;
                //                if (PublicDataClass._DataField.FieldLen >= 800)      //一帧的长度
                //                {
                //                    dataPos = ycadr;
                //                    Pos = PublicDataClass._DataField.FieldVSQ;
                //                    timer1.Enabled = true;
                //                    PublicDataClass.seqflag = 1;
                //                    break;

                //                }
                //                PublicDataClass.seqflag = 0;
                //            }
                //        }
                //        catch
                //        { }
                //    }
                //} 

            }
            else if (mid == 5)
            {
                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 0;
                PublicDataClass.ParamInfoAddr += Pos;
                PublicDataClass.seq++;
                int index = 0, infadr = 0;
                byte qufanflag = 0, setflag = 0, value = 0;
                for (int i = dataPos + 1; i < PublicDataClass._YxConfigParam.num; i++)
                {
                    index = Convert.ToInt32(PublicDataClass._YxConfigParam.IndexTable[i]);
                    infadr = Convert.ToInt32(PublicDataClass._YxConfigParam.AddrTable[i]);

                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((index) & 0x00ff);
                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((index) & 0xff00) >> 8);
                    //PublicDataClass._DataField.FieldLen += 2;

                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((infadr) & 0x00ff);
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((infadr) & 0xff00) >> 8);
                    PublicDataClass._DataField.FieldLen += 2;



                    if (PublicDataClass._YxConfigParam.QufanTable[i] == "否")
                        qufanflag = 0;
                    else if (PublicDataClass._YxConfigParam.QufanTable[i] == "是")
                        qufanflag = 1;

                    //if (PublicDataClass._YxConfigParam.setvalueTable[i] == "否")
                    //    setflag = 0;
                    //else if (PublicDataClass._YxConfigParam.setvalueTable[i] == "是")
                    //    setflag = 1;

                    //value = Convert.ToByte(PublicDataClass._YxConfigParam.ValueTable[i]);

                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((value << 2) + (setflag << 1) + (qufanflag));
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((qufanflag));
                    PublicDataClass._DataField.FieldLen += 1;

                    PublicDataClass._DataField.FieldVSQ++;
                    if ((PublicDataClass._DataField.FieldLen >= PublicDataClass.Framelen - 2) && (i < PublicDataClass._YxConfigParam.num - 1))      //一帧的长度
                    {
                        dataPos = i;
                        Pos = PublicDataClass._DataField.FieldVSQ;
                        timer1.Enabled = true;
                        PublicDataClass.seqflag = 1;
                        break;

                    }
                    PublicDataClass.seqflag = 0;

                }

            }
            else if (mid == 6)
            {
                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 0;
                PublicDataClass.ParamInfoAddr += Pos;
                PublicDataClass.seq++;
                for (int i = dataPos + 1; i < PublicDataClass._YkConfigParam.num; i++)
                {
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._YkConfigParam.AddrTable[i])) & 0x00ff);
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(PublicDataClass._YkConfigParam.AddrTable[i])) & 0xff00) >> 8);
                    PublicDataClass._DataField.FieldLen += 2;
                    if (PublicDataClass._YkConfigParam.triggermodeTable[i] == "电平")
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0;
                    else if (PublicDataClass._YkConfigParam.triggermodeTable[i] == "脉冲")
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 1;
                    PublicDataClass._DataField.FieldLen += 1;

                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._YkConfigParam.secltimeTable[i])) & 0x00ff);
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(PublicDataClass._YkConfigParam.secltimeTable[i])) & 0xff00) >> 8);
                    PublicDataClass._DataField.FieldLen += 2;

                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._YkConfigParam.exetimeTable[i])) & 0x00ff);
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(PublicDataClass._YkConfigParam.exetimeTable[i]) & 0xff00)) >> 8);
                    PublicDataClass._DataField.FieldLen += 2;

                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._YkConfigParam.pulsewidthTable[i])) & 0x00ff);
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(PublicDataClass._YkConfigParam.pulsewidthTable[i]) & 0xff00)) >> 8);
                    PublicDataClass._DataField.FieldLen += 2;

                    if (PublicDataClass._YkConfigParam.saveflagTable[i] == "否")
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0;
                    else if (PublicDataClass._YkConfigParam.saveflagTable[i] == "是")
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 1;
                    PublicDataClass._DataField.FieldLen += 1;

                    if (PublicDataClass._YkConfigParam.powerTable[i] == "无")
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0;
                    else if (PublicDataClass._YkConfigParam.powerTable[i] == "24V")
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 1;
                    else if (PublicDataClass._YkConfigParam.powerTable[i] == "电操机构")
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 2;
                    else if (PublicDataClass._YkConfigParam.powerTable[i] == "电源+电操")
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 3;
                    PublicDataClass._DataField.FieldLen += 1;

                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._YkConfigParam.jdq1Table[i])) & 0x00ff);
                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(PublicDataClass._YkConfigParam.jdq1Table[i]) & 0xff00)) >> 8);
                    //PublicDataClass._DataField.FieldLen += 2;

                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._YkConfigParam.jdq2Table[i])) & 0x00ff);
                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(PublicDataClass._YkConfigParam.jdq2Table[i]) & 0xff00)) >> 8);
                    //PublicDataClass._DataField.FieldLen += 2;

                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._YkConfigParam.fjyx1Table[i])) & 0x00ff);
                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(PublicDataClass._YkConfigParam.fjyx1Table[i]) & 0xff00)) >> 8);
                    //PublicDataClass._DataField.FieldLen += 2;

                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._YkConfigParam.fjyx2Table[i])) & 0x00ff);
                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(PublicDataClass._YkConfigParam.fjyx2Table[i]) & 0xff00)) >> 8);
                    //PublicDataClass._DataField.FieldLen += 2;
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._YkConfigParam.jdq1Table[i])) & 0x00ff);
                    PublicDataClass._DataField.FieldLen += 1;

                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._YkConfigParam.jdq2Table[i])) & 0x00ff);
                    PublicDataClass._DataField.FieldLen += 1;

                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._YkConfigParam.fjyx1Table[i])) & 0x00ff);
                    PublicDataClass._DataField.FieldLen += 1;

                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._YkConfigParam.fjyx2Table[i])) & 0x00ff);
                    PublicDataClass._DataField.FieldLen += 1;


                    PublicDataClass._DataField.FieldVSQ++;
                    if ((PublicDataClass._DataField.FieldLen >= PublicDataClass.Framelen - 14) && (i < PublicDataClass._YkConfigParam.num - 1))      //一帧的长度
                    {
                        dataPos = i;
                        Pos = PublicDataClass._DataField.FieldVSQ;
                        timer1.Enabled = true;
                        PublicDataClass.seqflag = 1;
                        break;

                    }
                    PublicDataClass.seqflag = 0;

                }

            }
            else if (mid == 7)
            {
                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 0;
                PublicDataClass.ParamInfoAddr += Pos;
                PublicDataClass.seq++;
                for (int i = dataPos + 1; i < PublicDataClass._AIParam.num; i++)
                {
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)(Convert.ToChar(PublicDataClass._AIParam.quality[i]));
                    PublicDataClass._DataField.FieldLen++;

                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)(Convert.ToChar(PublicDataClass._AIParam.phase[i]));
                    PublicDataClass._DataField.FieldLen++;

                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (Convert.ToByte(PublicDataClass._AIParam.line[i]));
                    PublicDataClass._DataField.FieldLen++;

                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (Convert.ToByte(PublicDataClass._AIParam.panel[i]));
                    PublicDataClass._DataField.FieldLen++;

                    PublicDataClass._DataField.FieldVSQ++;
                    if ((PublicDataClass._DataField.FieldLen >= PublicDataClass.Framelen - 4) && (i < PublicDataClass._AIParam.num - 1))      //一帧的长度
                    {
                        dataPos = i;
                        Pos = PublicDataClass._DataField.FieldVSQ;
                        timer1.Enabled = true;
                        PublicDataClass.seqflag = 1;
                        break;

                    }
                    PublicDataClass.seqflag = 0;
                }
            }
            else if (mid == 8)
            {
                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 0;
                PublicDataClass.ParamInfoAddr += Pos;
                PublicDataClass.seq++;
                for (int i = dataPos + 1; i < PublicDataClass._AIParam.num; i++)
                {
                    byte[] b = BitConverter.GetBytes(float.Parse(PublicDataClass._AIParam.value[i]));
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = b[0];
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = b[1];
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = b[2];
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 3] = b[3];
                    PublicDataClass._DataField.FieldLen += 4;

                    b = BitConverter.GetBytes(float.Parse(PublicDataClass._AIParam.ph[i]));
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = b[0];
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = b[1];
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = b[2];
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 3] = b[3];
                    PublicDataClass._DataField.FieldLen += 4;
                    PublicDataClass._DataField.FieldVSQ++;
                    //if ((PublicDataClass._DataField.FieldLen >= 800 - 7-6) && (i < PublicDataClass._AIParam.num - 1))      //一帧的长度
                    if ((PublicDataClass._DataField.FieldLen >= PublicDataClass.Framelen - 7) && (i < PublicDataClass._AIParam.num - 1))      //一帧的长度
                    {
                        dataPos = i;
                        Pos = PublicDataClass._DataField.FieldVSQ;
                        timer1.Enabled = true;
                        PublicDataClass.seqflag = 1;
                        break;

                    }
                    PublicDataClass.seqflag = 0;
                }
            }
            else if (mid == 9)//模拟量置数
            {
                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 0;
                PublicDataClass.ParamInfoAddr += Pos;
                PublicDataClass.seq++;
                for (int i = dataPos + 1; i < PublicDataClass._AIParam.num; i++)
                {
                    //byte[] b = BitConverter.GetBytes(float.Parse(PublicDataClass._AIParam.zhishu[i]));
                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = b[0];
                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = b[1];
                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = b[2];
                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 3] = b[3];
                    //PublicDataClass._DataField.FieldLen += 4;

                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._AIParam.zhishu[i])) & 0x00ff);
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(PublicDataClass._AIParam.zhishu[i])) & 0xff00) >> 8);
                    PublicDataClass._DataField.FieldLen += 2;
                    PublicDataClass._DataField.FieldVSQ++;

                    if ((PublicDataClass._DataField.FieldLen >= PublicDataClass.Framelen - 3) && (i < PublicDataClass._AIParam.num - 1))      //一帧的长度
                    //if ((PublicDataClass._DataField.FieldLen >= 800- 3) && (i < PublicDataClass._AIParam.num - 1))      //一帧的长度
                    {
                        dataPos = i;
                        Pos = PublicDataClass._DataField.FieldVSQ;
                        timer1.Enabled = true;
                        PublicDataClass.seqflag = 1;
                        break;

                    }
                    PublicDataClass.seqflag = 0;
                }
            }

            else if (ItemId - 0x14 >= 0)  //动态选项卡
            {
                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 0;
                PublicDataClass.ParamInfoAddr += Pos;
                PublicDataClass.seq++;
                int YCcount = 0;
                if (PublicDataClass.TabCfg[ItemId - 0x14].PageName == "内遥信配置参数")
                {
                    for (int q = dataPos + 1; q < PublicDataClass.TabCfg[ItemId - 0x14].LineNum; q++)
                    {
                        for (int col = 0; col < PublicDataClass.TabCfg[ItemId - 0x14].ColumnNum; col++)
                        {
                            try
                            {
                                if (PublicDataClass.TabCfg[ItemId - 0x14].ColumnDownByte[col] == "1")
                                {
                                    if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "过流II段")
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 1;
                                    else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "过流I段")
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 2;
                                    else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "零序告警")
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 3;
                                    else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "电流过负荷")
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 4;
                                    else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "过压告警")
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 5;
                                    else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "欠压告警")
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 6;
                                    else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "断线")
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 7;
                                    else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "单相接地")
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 8;
                                    else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "相间短路")
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 9;
                                    else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "事故总")
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 10;
                                    else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "过流II段线路总信号")
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 11;
                                    else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "过流I段线路总信号")
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 12;
                                    else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "相间短路总信号")
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 13;
                                    else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "AB")
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)(Convert.ToChar("C"));
                                    else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "BC")
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)(Convert.ToChar("A"));
                                    else if (PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q] == "AC")
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)(Convert.ToChar("B"));
                                    else
                                    {
                                        int value;
                                        bool flag = int.TryParse(PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q], out value);
                                        if (flag == true)
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q]);
                                        else
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)(Convert.ToChar(PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q]));
                                    }
                                    PublicDataClass._DataField.FieldLen++;
                                }
                                else if (PublicDataClass.TabCfg[ItemId - 0x14].ColumnDownByte[col] == "2")
                                {
                                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q]))  & 0x00ff);
                                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q])) & 0xff00) >> 8);
                                    //InttoByte(Convert.ToInt32(PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q]), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                                    PublicDataClass._DataField.FieldLen += 2;
                                }
                                else if (PublicDataClass.TabCfg[ItemId - 0x14].ColumnDownByte[col] == "4")
                                {

                                    byte[] b = BitConverter.GetBytes(float.Parse(PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q]));

                                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = b[0];
                                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = b[1];
                                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = b[2];
                                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 3] = b[3];


                                    //  InttoByte(Convert.ToInt16(PublicDataClass._RaoDong.ValueTable[i]), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                                    PublicDataClass._DataField.FieldLen += 4;
                                }
                            }
                            catch
                            {
                                YCcount++;
                                if (YCcount == 1)
                                    MessageBox.Show("蓝色标注处参数配置有误！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //MessageBox.Show("第" + q + "行" + "第" + col + "列参数配置有误！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                listViewtest.Items[q].UseItemStyleForSubItems = false;
                                listViewtest.Items[q].SubItems[col].ForeColor = Color.Blue;

                            }
                        }
                        PublicDataClass._DataField.FieldVSQ++;
                        if ((PublicDataClass._DataField.FieldLen >= PublicDataClass.Framelen - 6))      //一帧的长度
                        {
                            dataPos = q;
                            Pos = PublicDataClass._DataField.FieldVSQ;
                            timer1.Enabled = true;
                            PublicDataClass.seqflag = 1;
                            break;

                        }
                        PublicDataClass.seqflag = 0;
                    }
                }
                else
                {
                    string thetemp;
                    for (int q = dataPos + 1; q < PublicDataClass.TabCfg[ItemId - 0x14].LineNum; q++)
                    {
                        for (int col = 0; col < PublicDataClass.TabCfg[ItemId - 0x14].ColumnNum; col++)
                        {
                            try
                            {
                                int a = PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q].IndexOf("（");
                                if (a < 0)
                                    a = PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q].IndexOf("(");
                                if (a > 0)
                                {
                                    thetemp = PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q].Substring(0, a);
                                }
                                else
                                    thetemp = PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q];
                                if (PublicDataClass.TabCfg[ItemId - 0x14].ColumnDownByte[col] == "1")
                                {
                                    int value;
                                    bool flag = int.TryParse(thetemp, out value);
                                    if (flag == true)
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(thetemp);
                                    else
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)(Convert.ToChar(thetemp));
                                    PublicDataClass._DataField.FieldLen++;
                                }
                                else if (PublicDataClass.TabCfg[ItemId - 0x14].ColumnDownByte[col] == "2")
                                {
                                    //InttoByte(Convert.ToInt32(thetemp), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(thetemp)) & 0x00ff);
                                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(thetemp)) & 0xff00) >> 8);
                                    PublicDataClass._DataField.FieldLen += 2;
                                }
                                else if (PublicDataClass.TabCfg[ItemId - 0x14].ColumnDownByte[col] == "4")
                                {

                                    byte[] b = BitConverter.GetBytes(float.Parse(thetemp));

                                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = b[0];
                                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = b[1];
                                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = b[2];
                                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 3] = b[3];


                                    //  InttoByte(Convert.ToInt16(PublicDataClass._RaoDong.ValueTable[i]), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                                    PublicDataClass._DataField.FieldLen += 4;
                                }
                                else if (PublicDataClass.TabCfg[ItemId - 0x14].ColumnDownByte[col] == "按字节数下载")
                                {
                                    int m;
                                    string zijie;
                                    for (m = 0; m < PublicDataClass.TabCfg[ItemId - 0x14].ColumnNum; m++)
                                    {
                                        if (PublicDataClass.TabCfg[ItemId - 0x14].ColumnDownByte[m] == "字节数")
                                            break;
                                    }
                                    zijie = PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[m].ValueTable[q];
                                    if (zijie == "1")
                                    {
                                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(thetemp);
                                        //PublicDataClass._DataField.FieldLen++;
                                        int value;
                                        bool flag = int.TryParse(thetemp, out value);
                                        if (flag == true)
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(thetemp);
                                        else
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)(Convert.ToChar(thetemp));
                                        PublicDataClass._DataField.FieldLen++;

                                    }
                                    else if (zijie == "2")
                                    {

                                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(PublicDataClass._NetParam.ValueTable[i])) & 0x00ff);
                                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(PublicDataClass._NetParam.ValueTable[i])) & 0xff00) >> 8);
                                        //PublicDataClass._DataField.FieldLen += 2;
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(thetemp))  & 0x00ff);
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(thetemp)) & 0xff00) >> 8);
                                        //InttoByte(Convert.ToInt32(thetemp), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                                        PublicDataClass._DataField.FieldLen += 2;
                                    }
                                    else if (zijie == "4")
                                    {

                                        //for (int k = 0; k < 4; k++)
                                        //{
                                        //    if (k < 3)
                                        //    {
                                        //        int b = thetemp.IndexOf(".");
                                        //        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(thetemp.Substring(0, b));
                                        //        thetemp = thetemp.Remove(0, b + 1);
                                        //    }
                                        //    else
                                        //        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(thetemp);
                                        //    PublicDataClass._DataField.FieldLen++;

                                        //}
                                        //byte[] b = BitConverter.GetBytes(float.Parse(thetemp));
                                        byte[] b = BitConverter.GetBytes(long.Parse(thetemp));

                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = b[0];
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = b[1];
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = b[2];
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 3] = b[3];


                                        //  InttoByte(Convert.ToInt16(PublicDataClass._RaoDong.ValueTable[i]), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                                        PublicDataClass._DataField.FieldLen += 4;
                                    }
                                    else if (zijie == "6")
                                    {

                                        for (int k = 0; k < 6; k++)
                                        {
                                            if (k < 5)
                                            {
                                                //int a = text.IndexOf("-");
                                                //string cs = text.Substring(0, a);
                                                //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = PublicFunction.StringToByte(cs);

                                                int b = thetemp.IndexOf("-");
                                                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(thetemp.Substring(0, b));
                                                thetemp = thetemp.Remove(0, b + 1);

                                            }
                                            else
                                                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(thetemp);
                                            PublicDataClass._DataField.FieldLen++;

                                        }
                                    }


                                }
                            }
                            catch
                            {
                                YCcount++;
                                if (YCcount == 1)
                                    MessageBox.Show("蓝色标注处参数配置有误！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //MessageBox.Show("第" + q + "行" + "第" + col + "列参数配置有误！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                listViewtest.Items[q].UseItemStyleForSubItems = false;
                                listViewtest.Items[q].SubItems[col].ForeColor = Color.Blue;

                            }
                        }
                        PublicDataClass._DataField.FieldVSQ++;
                        if ((PublicDataClass._DataField.FieldLen >= PublicDataClass.Framelen - 10))      //一帧的长度
                        {
                            dataPos = q;
                            Pos = PublicDataClass._DataField.FieldVSQ;
                            timer1.Enabled = true;
                            PublicDataClass.seqflag = 1;
                            break;

                        }
                        PublicDataClass.seqflag = 0;
                    }
                }
            }
            if (ty == 1)
                PublicDataClass._ComTaskFlag.SET_PARAM_CON = true;

            if (ty == 2)
                PublicDataClass._NetTaskFlag.SET_PARAM_CON = true;
            if (ty == 3)
                PublicDataClass._GprsTaskFlag.SET_PARAM_CON = true;


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (PublicDataClass._Message.ParamAck == true)
            {
                PublicDataClass._Message.ParamAck = false;
                timer1.Enabled = false;
                StartMangFrameTransmit(ItemId);

            }
        }
        /// <summary>
        /// 保存--按钮的处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonsave_Click(object sender, EventArgs e)
        {
            string FileName = "";
            //string path =System.AppDomain.CurrentDomain.BaseDirectory;
            //path += "ini";
            string path = PublicDataClass.PrjPath + "\\ini";

            if (ItemId == 1)   //串口参数
            {
                for (int j = 0; j < listView1.Items.Count; j++)
                {

                    PublicDataClass._ComParam.NameTable[j] = listView1.Items[j].SubItems[1].Text;//取得listview某行某列的值

                    PublicDataClass._ComParam.ValueTable[j] = listView1.Items[j].SubItems[2].Text;

                    PublicDataClass._ComParam.ByteTable[j] = listView1.Items[j].SubItems[3].Text;
                }
                FileName = path + "\\comparam.ini";
                PublicDataClass._ComParam.num = listView1.Items.Count;

                WriteReadAllFile.WriteReadParamIniFile(FileName, 1, 0);
                //Form1.WriteIniFileName = FileName;
                //Form1.WriteIniFileType = 1;
                //Form1.WriteIniFilek = 0;
                //Form1.WriteIniflag = true;


            }
            else if (ItemId == 2)   //网口参数
            {
                for (int j = 0; j < listView1.Items.Count; j++)
                {

                    PublicDataClass._NetParam.NameTable[j] = listView1.Items[j].SubItems[1].Text;//取得listview某行某列的值

                    PublicDataClass._NetParam.ValueTable[j] = listView1.Items[j].SubItems[2].Text;

                    PublicDataClass._NetParam.ByteTable[j] = listView1.Items[j].SubItems[3].Text;
                }
                FileName = path + "\\netparam.ini";
                PublicDataClass._NetParam.num = listView1.Items.Count;

                WriteReadAllFile.WriteReadParamIniFile(FileName, 1, 1);
                //Form1.WriteIniFileName = FileName;
                //Form1.WriteIniFileType = 1;
                //Form1.WriteIniFilek = 1;
                //Form1.WriteIniflag = true;

            }
            else if (ItemId == 3)  //系统参数
            {

                for (int j = 0; j < listView1.Items.Count; j++)
                {

                    PublicDataClass._SysParam.NameTable[j] = listView1.Items[j].SubItems[1].Text;//取得listview某行某列的值

                    PublicDataClass._SysParam.ValueTable[j] = listView1.Items[j].SubItems[2].Text;

                    PublicDataClass._SysParam.ByteTable[j] = listView1.Items[j].SubItems[3].Text;
                }
                FileName = path + "\\sysparam.ini";
                PublicDataClass._SysParam.num = listView1.Items.Count;

                WriteReadAllFile.WriteReadParamIniFile(FileName, 1, 2);
                //Form1.WriteIniFileName = FileName;
                //Form1.WriteIniFileType = 1;
                //Form1.WriteIniFilek = 2;
                //Form1.WriteIniflag = true;
            }
            else if (ItemId == 4)  //遥测配置
            {
                for (int j = 0; j < listView1.Items.Count; j++)
                {
                    PublicDataClass._YcConfigParam.NameTable[j] = listView1.Items[j].SubItems[1].Text;//取得listview某行某列的值
                    PublicDataClass._YcConfigParam.AddrTable[j] = listView1.Items[j].SubItems[3].Text;
                    PublicDataClass._YcConfigParam.IndexTable[j] = listView1.Items[j].SubItems[2].Text;
                    //    PublicDataClass._YcConfigParam.DatatypeTable[j] = listView1.Items[j].SubItems[4].Text;//取得listview某行某列的值
                    //    PublicDataClass._YcConfigParam.MagnificationTable[j] = listView1.Items[j].SubItems[5].Text;
                    //    PublicDataClass._YcConfigParam.ConnectTable[j] = listView1.Items[j].SubItems[6].Text;
                    //     PublicDataClass._YcConfigParam.QufanTable[j] = listView1.Items[j].SubItems[7].Text;//取得listview某行某列的值
                    //PublicDataClass._YcConfigParam.setvalueTable[j] = listView1.Items[j].SubItems[8].Text;
                    //PublicDataClass._YcConfigParam.ValueTable[j] = listView1.Items[j].SubItems[9].Text;
                    //  PublicDataClass._YcConfigParam.ByteTable[j] = listView1.Items[j].SubItems[8].Text;

                }

                FileName = path + "\\YCconfig.ini";
                PublicDataClass._YcConfigParam.num = listView1.Items.Count;

                WriteReadAllFile.WriteReadParamIniFile(FileName, 1, 3);
                //Form1.WriteIniFileName = FileName;
                //Form1.WriteIniFileType = 1;
                //Form1.WriteIniFilek = 3;
                //Form1.WriteIniflag = true;

            }
            else if (ItemId == 5)  //遥信配置
            {
                for (int j = 0; j < listView1.Items.Count; j++)
                {
                    PublicDataClass._YxConfigParam.NameTable[j] = listView1.Items[j].SubItems[1].Text;//取得listview某行某列的值
                    PublicDataClass._YxConfigParam.AddrTable[j] = listView1.Items[j].SubItems[3].Text;
                    PublicDataClass._YxConfigParam.IndexTable[j] = listView1.Items[j].SubItems[2].Text;
                    PublicDataClass._YxConfigParam.QufanTable[j] = listView1.Items[j].SubItems[4].Text;//取得listview某行某列的值
                    //PublicDataClass._YxConfigParam.setvalueTable[j] = listView1.Items[j].SubItems[5].Text;
                    //PublicDataClass._YxConfigParam.ValueTable[j] = listView1.Items[j].SubItems[6].Text;
                    PublicDataClass._YxConfigParam.ByteTable[j] = listView1.Items[j].SubItems[5].Text;

                }

                FileName = path + "\\YXconfig.ini";
                PublicDataClass._YxConfigParam.num = listView1.Items.Count;

                WriteReadAllFile.WriteReadParamIniFile(FileName, 1, 4);
                //Form1.WriteIniFileName = FileName;
                //Form1.WriteIniFileType = 1;
                //Form1.WriteIniFilek = 4;
                //Form1.WriteIniflag = true;
            }
            else if (ItemId == 6)  //遥控配置
            {


                for (int j = 0; j < listView1.Items.Count; j++)
                {
                    PublicDataClass._YkConfigParam.NameTable[j] = listView1.Items[j].SubItems[1].Text;//取得listview某行某列的值
                    PublicDataClass._YkConfigParam.AddrTable[j] = listView1.Items[j].SubItems[2].Text;

                    PublicDataClass._YkConfigParam.triggermodeTable[j] = listView1.Items[j].SubItems[3].Text;
                    PublicDataClass._YkConfigParam.secltimeTable[j] = listView1.Items[j].SubItems[4].Text;//取得listview某行某列的值
                    PublicDataClass._YkConfigParam.exetimeTable[j] = listView1.Items[j].SubItems[5].Text;
                    PublicDataClass._YkConfigParam.pulsewidthTable[j] = listView1.Items[j].SubItems[6].Text;
                    PublicDataClass._YkConfigParam.saveflagTable[j] = listView1.Items[j].SubItems[7].Text;

                    PublicDataClass._YkConfigParam.powerTable[j] = listView1.Items[j].SubItems[8].Text;
                    PublicDataClass._YkConfigParam.jdq1Table[j] = listView1.Items[j].SubItems[9].Text;//取得listview某行某列的值
                    PublicDataClass._YkConfigParam.jdq2Table[j] = listView1.Items[j].SubItems[10].Text;
                    PublicDataClass._YkConfigParam.fjyx1Table[j] = listView1.Items[j].SubItems[11].Text;
                    PublicDataClass._YkConfigParam.fjyx2Table[j] = listView1.Items[j].SubItems[12].Text;

                    PublicDataClass._YkConfigParam.ByteTable[j] = listView1.Items[j].SubItems[13].Text;
                }

                FileName = path + "\\YKconfig.ini";
                PublicDataClass._YkConfigParam.num = listView1.Items.Count;

                WriteReadAllFile.WriteReadParamIniFile(FileName, 1, 5);
                //Form1.WriteIniFileName = FileName;
                //Form1.WriteIniFileType = 1;
                //Form1.WriteIniFilek = 5;
                //Form1.WriteIniflag = true;
            }
            else if ((ItemId == 7) || (ItemId == 8) || (ItemId == 9))  //模拟量配置
            {

                for (int j = 0; j < listView1.Items.Count; j++)
                {
                    PublicDataClass._AIParam.quality[j] = listView1.Items[j].SubItems[1].Text;//取得listview某行某列的值
                    PublicDataClass._AIParam.phase[j] = listView1.Items[j].SubItems[2].Text;
                    PublicDataClass._AIParam.line[j] = listView1.Items[j].SubItems[3].Text;
                    PublicDataClass._AIParam.panel[j] = listView1.Items[j].SubItems[4].Text;//取得listview某行某列的值
                    PublicDataClass._AIParam.value[j] = listView1.Items[j].SubItems[5].Text;
                    PublicDataClass._AIParam.ph[j] = listView1.Items[j].SubItems[6].Text;
                    PublicDataClass._AIParam.zhishu[j] = listView1.Items[j].SubItems[7].Text;
                }

                FileName = path + "\\模拟量接入配置.ini";
                PublicDataClass._AIParam.num = listView1.Items.Count;

                WriteReadAllFile.WriteReadParamIniFile(FileName, 1, 10);
                //Form1.WriteIniFileName = FileName;
                //Form1.WriteIniFileType = 1;
                //Form1.WriteIniFilek = 10;
                //Form1.WriteIniflag = true;
            }


            else if (ItemId - 0x14 >= 0)  //动态选项卡
            {
                PublicDataClass.TabCfg[ItemId - 0x14].LineNum = listViewtest.Items.Count;
                if (PublicDataClass.TabCfg[ItemId - 0x14].PageName == "遥信置数")
                {

                    PublicDataClass.TabCfg[ItemId - 0x14].LineNum = PublicDataClass._YxConfigParam.wyxnum;
                }

                WriteReadAllFile.ReadDynOptFile(PublicDataClass.FILENAME[ItemId - 0x14], ItemId - 0x14, 2);

            }


            MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void InsertMenuItem_Click(object sender, EventArgs e)
        {
            //if (ItemId < 9 || ItemId == 11)
            // {
            //     AddParamRecordViewForm AddPfm = new AddParamRecordViewForm();
            //     AddPfm.ShowDialog();
            //     if (AddPfm.DialogResult == DialogResult.OK)               //判断是否按下-确定-按钮
            //     {
            //         ListViewItem lv = new ListViewItem(String.Format("{0:d}", this.listView1.SelectedItems[0].Index));
            //         lv.SubItems.Add(PublicDataClass._AddParamRecord.Name);

            //         lv.SubItems.Add(PublicDataClass._AddParamRecord.Value);
            //         lv.SubItems.Add(PublicDataClass._AddParamRecord.Beilv);
            //         listView1.Items.Insert(this.listView1.SelectedItems[0].Index, lv);
            //         for (int i = 0; i < listView1.Items.Count; i++)
            //         {
            //             listView1.Items[i].SubItems[0].Text = String.Format("{0:d}", i);   //重新调整序号

            //         }
            //         RefreshParamState();
            //     }
            // }
            //else if (ItemId == 9)
            //{
            //    AddParamRecordViewForm1 AddPfm = new AddParamRecordViewForm1();
            //    AddPfm.ShowDialog();
            //    if (AddPfm.DialogResult == DialogResult.OK)               //判断是否按下-确定-按钮
            //    {
            //        ListViewItem lv = new ListViewItem(String.Format("{0:d}", this.listView2.SelectedItems[0].Index));
            //        lv.SubItems.Add(PublicDataClass._AddYcDotParamRecord.Name);

            //        lv.SubItems.Add(PublicDataClass._AddYcDotParamRecord.BusNum);
            //        lv.SubItems.Add(PublicDataClass._AddYcDotParamRecord.CardNum);
            //        lv.SubItems.Add(PublicDataClass._AddYcDotParamRecord.UBusConnectionmode);
            //        lv.SubItems.Add(PublicDataClass._AddYcDotParamRecord.IBusConnectionmode);
            //        listView2.Items.Insert(this.listView2.SelectedItems[0].Index, lv);
            //        for (int i = 0; i < listView2.Items.Count; i++)
            //        {
            //            listView2.Items[i].SubItems[0].Text = String.Format("{0:d}", i);   //重新调整序号

            //        }
            //        RefreshParamState();
            //    }
            //}
            //else if (ItemId == 10)
            //{
            //    AddParamRecordViewForm2 AddPfm = new AddParamRecordViewForm2();
            //    AddPfm.ShowDialog();
            //    if (AddPfm.DialogResult == DialogResult.OK)               //判断是否按下-确定-按钮
            //    {
            //        ListViewItem lv = new ListViewItem(String.Format("{0:d}", this.listView3.SelectedItems[0].Index));

            //        lv.SubItems.Add(PublicDataClass._AddYcInformationRecord.Name);
            //        lv.SubItems.Add(PublicDataClass._AddYcInformationRecord.Index);
            //        lv.SubItems.Add(PublicDataClass._AddYcInformationRecord.Datatype);
            //        lv.SubItems.Add(PublicDataClass._AddYcInformationRecord.Magnification);

            //        listView3.Items.Insert(this.listView3.SelectedItems[0].Index, lv);
            //        for (int i = 0; i < listView3.Items.Count; i++)
            //        {
            //            listView3.Items[i].SubItems[0].Text = String.Format("{0:d}", i);   //重新调整序号

            //        }
            //        RefreshParamState();
            //    }
            //}
            if (ItemId - 0x14 >= 0)  //动态选项卡
            {

                ListViewItem lv = new ListViewItem(String.Format("{0:d}", this.listViewtest.SelectedItems[0].Index));
                for (int i = 0; i < listViewtest.Columns.Count - 1; i++)
                {
                    lv.SubItems.Add("");
                }
                listViewtest.Items.Insert(this.listViewtest.SelectedItems[0].Index, lv);

                for (int i = 0; i < listViewtest.Items.Count; i++)
                {
                    listViewtest.Items[i].SubItems[0].Text = String.Format("{0:d}", i);   //重新调整序号
                }
                RefreshParamState();
            }
            else
            {
                ListViewItem lv = new ListViewItem(String.Format("{0:d}", this.listView1.SelectedItems[0].Index));
                for (int i = 0; i < listView1.Columns.Count - 1; i++)
                {
                    lv.SubItems.Add("");
                }
                listView1.Items.Insert(this.listView1.SelectedItems[0].Index, lv);

                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    listView1.Items[i].SubItems[0].Text = String.Format("{0:d}", i);   //重新调整序号
                }
                RefreshParamState();
            }

        }
        /// <summary>
        /// 参数导入--按钮的消息响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inputbutton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofile = new OpenFileDialog();
            ofile.Filter = "System Files(*.ini)|*.ini|所有文件(*.*)|*.*";
            ofile.InitialDirectory = System.Environment.CurrentDirectory;
            string savesysfilepath = @"";
            if (ofile.ShowDialog() == DialogResult.OK)
            {
                if (ItemId <= 9)
                {
                    listView1.Items.Clear();
                    FileInfo f = new FileInfo(ofile.FileName);
                    InputParam(f.FullName);
                }
            }
            ofile.Dispose();
        }
        private void InputParam(string path)
        {
            if (ItemId == 1)
                WriteReadAllFile.WriteReadParamIniFile(path, 0, 0);
            else if (ItemId == 2)
                WriteReadAllFile.WriteReadParamIniFile(path, 0, 1);
            else if (ItemId == 3)
                WriteReadAllFile.WriteReadParamIniFile(path, 0, 2);
            else if (ItemId == 4)
                WriteReadAllFile.WriteReadParamIniFile(path, 0, 3);
            else if (ItemId == 5)
                WriteReadAllFile.WriteReadParamIniFile(path, 0, 4);
            else if (ItemId == 6)
                WriteReadAllFile.WriteReadParamIniFile(path, 0, 5);
            else if (ItemId <= 9)
                WriteReadAllFile.WriteReadParamIniFile(path, 0, 10);
            //if ( ItemId <7)
            //    Form1.WriteIniFilek = (byte)(ItemId - 1);
            //else
            //    Form1.WriteIniFilek = 10;
            //Form1.WriteIniFileName = path;
            //Form1.WriteIniFileType = 0;
            //Form1.WriteIniflag = true;
            CheckNowParamState();

        }

        private void ModifyMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                if (ItemId <= 9)
                {
                    PublicDataClass._Mystruct.bpos = 0;
                    PublicDataClass._Mystruct.epos = listView1.Items.Count;
                }
                else
                {
                    PublicDataClass._Mystruct.bpos = 0;
                    PublicDataClass._Mystruct.epos = listViewtest.Items.Count;
                }
                CModifyViewForm cfm = new CModifyViewForm();
                cfm.ShowDialog();
                if (cfm.DialogResult == DialogResult.OK)               //判断是否按下-确定-按钮
                {
                    if (ItemId <= 9)
                    {
                        for (int i = PublicDataClass._Mystruct.bpos; i <= PublicDataClass._Mystruct.epos; i++)
                        {
                            listView1.Items[i].SubItems[PublicDataClass._Mystruct.row].Text = Convert.ToString(PublicDataClass._Mystruct.value);   //重新调整序号

                        }
                    }
                    else
                    {
                        for (int i = PublicDataClass._Mystruct.bpos; i <= PublicDataClass._Mystruct.epos; i++)
                        {
                            listViewtest.Items[i].SubItems[PublicDataClass._Mystruct.row].Text = Convert.ToString(PublicDataClass._Mystruct.value);   //重新调整序号

                        }
                    }

                    RefreshParamState();
                }

            }
            catch
            {
                MessageBox.Show("数值输入有误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
                comboBoxaddr.Enabled = true;
            else
                comboBoxaddr.Enabled = false;
            PublicDataClass.addselect = 2;//默认主处理器
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* if (comboBoxaddr.SelectedIndex == 0)
                 PublicDataClass._DataField.Buffer[0] = 0x00;
             else if (comboBoxaddr.SelectedIndex == 1)
                 PublicDataClass._DataField.Buffer[0] = 0x01;
             else if (comboBoxaddr.SelectedIndex == 2)
                 PublicDataClass._DataField.Buffer[0] = 0x02;
             else if (comboBoxaddr.SelectedIndex == 3)
                 PublicDataClass._DataField.Buffer[0] = 0x03;
             else if (comboBoxaddr.SelectedIndex == 4)
                 PublicDataClass._DataField.Buffer[0] = 0xff;*/

            if (comboBoxaddr.SelectedIndex == 0)//dsp
                PublicDataClass.addselect = 2;
            else if (comboBoxaddr.SelectedIndex == 1)//协处理器
                PublicDataClass.addselect = 3;
            //else if (comboBoxaddr.SelectedIndex == 2)//
            //    PublicDataClass.addselect = 2;
            //else if (comboBoxaddr.SelectedIndex == 3)
            //    PublicDataClass.addselect = 3;
            //else if (comboBoxaddr.SelectedIndex == 4)
            //    PublicDataClass.addselect = 4;

        }

        public void DynOptProcess()               //动态选项卡处理函数
        {
            try
            {



                if (PublicDataClass.FILENAME.Length > 0)
                {
                    PublicDataClass.SoftPageShowFlag = 2;
                    //    PublicDataClass.TabCfg = new PublicDataClass.TabPageCfgParam[PublicDataClass.FILENAME.Length];//分配变量

                }
                else
                    PublicDataClass.SoftPageShowFlag = 0;
                //for (int k = 0; k < PublicDataClass.FILENAME.Length; k++)
                //{
                //    if (this.tabControl1.Controls.Contains(tp[k]))
                //        this.tabControl1.Controls.Remove(tp[k]);
                //}


                for (int k = 0; k < tp.Length; k++)
                {
                    if (this.tabControl1.Controls.Contains(tp[k]))
                        this.tabControl1.Controls.Remove(tp[k]);
                }




                if (PublicDataClass.SoftPageShowFlag == 2)
                {
                    for (int k = 0; k < PublicDataClass.FILENAME.Length; k++)
                    {

                        WriteReadAllFile.ReadDynOptFile(PublicDataClass.FILENAME[k], k, 1);
                        // 针对数据库的字段名称，建立与之适应显示表头
                        listViewtest.Items.Clear();
                        listViewtest.Columns.Clear();
                        for (int j = 0; j < PublicDataClass.TabCfg[k].ColumnNum; j++)
                        {
                            listViewtest.Columns.Add(PublicDataClass.TabCfg[k].ColumnPageName[j], 100, HorizontalAlignment.Left);//第一个参数，表头名，第2个参数，表头大小，第3个参数，样式    

                        }

                        for (int q = 0; q < PublicDataClass.TabCfg[k].LineNum; q++)
                        {
                            ListViewItem lv = new ListViewItem(PublicDataClass.TabCfg[k].TabPageValue[0].ValueTable[q]);
                            for (int j = 0; j < PublicDataClass.TabCfg[k].ColumnNum - 1; j++)
                            {

                                lv.SubItems.Add(PublicDataClass.TabCfg[k].TabPageValue[j + 1].ValueTable[q]);
                            }
                            listViewtest.Items.Add(lv);
                        }

                        tp[k] = new TabPage();
                        tp[k].Controls.Add(this.comboBoxvalue3);
                        tp[k].Controls.Add(this.listViewtest);
                        tp[k].Location = new System.Drawing.Point(4, 25);
                        string str = String.Format("tp_{0:d}", k);
                        tp[k].Name = str;
                        tp[k].Padding = new System.Windows.Forms.Padding(3);
                        tp[k].Size = new System.Drawing.Size(769, 375);
                        tp[k].TabIndex = 10 + k;//待定
                        tp[k].Text = PublicDataClass.TabCfg[k].PageName;
                        tp[k].UseVisualStyleBackColor = true;
                        tabControl1.Controls.Add(tp[k]);
                    }
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////


                }

            }
            catch
            { }

        }

        private void CheckNowDynOptParamState(int k)//更新动态选项卡参数
        {
            try
            {
                listViewtest.Items.Clear();
                listViewtest.Columns.Clear();
                listViewtest.Controls.Add(textBoxvalue);
                listViewtest.Controls.Add(comboBoxvalue);

                comboBoxvalue1.Items.Clear();
                comboBoxvalue1.Items.Add("过流II段");
                comboBoxvalue1.Items.Add("过流I段");
                comboBoxvalue1.Items.Add("零序告警");
                comboBoxvalue1.Items.Add("电流过负荷");
                comboBoxvalue1.Items.Add("过压告警");
                comboBoxvalue1.Items.Add("欠压告警");
                comboBoxvalue1.Items.Add("断线");
                comboBoxvalue1.Items.Add("单相接地");
                comboBoxvalue1.Items.Add("相间短路");
                comboBoxvalue1.Items.Add("事故总");
                comboBoxvalue1.Items.Add("过流II段线路总信号");
                comboBoxvalue1.Items.Add("过流I段线路总信号");
                comboBoxvalue1.Items.Add("相间短路总信号");
                listViewtest.Controls.Add(comboBoxvalue1);
                comboBoxvalue1.Visible = false;

                comboBoxvalue2.Items.Clear();
                comboBoxvalue2.Items.Add("U");
                comboBoxvalue2.Items.Add("I");
                listViewtest.Controls.Add(comboBoxvalue2);
                comboBoxvalue2.Visible = false;

                comboBoxvalue3.Items.Clear();
                comboBoxvalue3.Items.Add("A");
                comboBoxvalue3.Items.Add("B");
                comboBoxvalue3.Items.Add("C");
                comboBoxvalue3.Items.Add("O");
                comboBoxvalue3.Items.Add("P");
                comboBoxvalue3.Items.Add("Q");
                comboBoxvalue3.Items.Add("N");
                listViewtest.Controls.Add(comboBoxvalue3);
                comboBoxvalue3.Visible = false;


                if (PublicDataClass.TabCfg[k].PageName == "遥信置数")
                {

                    for (int j = 0; j < PublicDataClass._YxConfigParam.wyxnum; j++)
                    {
                        PublicDataClass.TabCfg[k].TabPageValue[1].ValueTable[j] = PublicDataClass._YxConfigParam.NameTable[j];
                    }

                    if (PublicDataClass._ChangeFlag.YxkjCfg == true)//已点击快捷配置
                    {
                        PublicDataClass._ChangeFlag.YxkjCfg = false;

                        YxCfgNum = PublicDataClass._FastParamRecord.num;
                        YxCfgIndex = new int[YxCfgNum];


                        for (int j = 0; j < YxCfgNum; j++)
                        {
                            YxCfgIndex[j] = PublicDataClass._FastParamRecord.index[j];
                        }
                        //PublicDataClass._ChangeFlag.YxzsCfg = true;
                    }
                    else //未点击快捷配置
                    {
                        YxCfgIndex = new int[YxCfgNum];
                        int ycadr = 1;


                        for (int m = 0; m < PublicDataClass._YxConfigParam.num; m++)
                        {
                            for (int j = 0; j < PublicDataClass._YxConfigParam.num; j++)
                            {
                                try
                                {
                                    if (Convert.ToInt32(PublicDataClass._YxConfigParam.AddrTable[j]) == ycadr)
                                    {
                                        YxCfgIndex[ycadr - 1] = Convert.ToInt32(PublicDataClass._YxConfigParam.IndexTable[j]);

                                        ycadr++;
                                        break;
                                    }
                                }
                                catch
                                { }
                            }
                        }
                        //PublicDataClass._ChangeFlag.YxzsCfg = true;
                    }
                    //if (PublicDataClass._ChangeFlag.YxzsCfg == true)
                    {
                        listViewtest.Columns.Add("序号");
                        listViewtest.Columns.Add("名称");
                        listViewtest.Columns[1].Width = 200;
                        listViewtest.Columns.Add("数值");
                        listViewtest.Columns.Add("索引号");


                        for (int j = 0; j < YxCfgNum; j++)
                        {

                            ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));

                            lv.SubItems.Add(PublicDataClass._YxConfigParam.NameTable[YxCfgIndex[j] - 1]);
                            if (YxCfgIndex[j] <= PublicDataClass._YxConfigParam.wyxnum)
                                lv.SubItems.Add(PublicDataClass.TabCfg[k].TabPageValue[2].ValueTable[YxCfgIndex[j] - 1]);
                            else
                                lv.SubItems.Add("0");
                            lv.SubItems.Add(Convert.ToString(YxCfgIndex[j]));
                            listViewtest.Items.Add(lv);

                        }
                    }
                    return;


                }

                for (int j = 0; j < PublicDataClass.TabCfg[k].ColumnNum; j++)
                {

                    if (j == 1)
                        listViewtest.Columns.Add(PublicDataClass.TabCfg[k].ColumnPageName[j], 400, HorizontalAlignment.Left);//第一个参数，表头名，第2个参数，表头大小，第3个参数，样式  
                    else
                        listViewtest.Columns.Add(PublicDataClass.TabCfg[k].ColumnPageName[j], 100, HorizontalAlignment.Left);//第一个参数，表头名，第2个参数，表头大小，第3个参数，样式    
                }

                for (int q = 0; q < PublicDataClass.TabCfg[k].LineNum; q++)
                {
                    ListViewItem lv = new ListViewItem(PublicDataClass.TabCfg[k].TabPageValue[0].ValueTable[q]);
                    for (int j = 0; j < PublicDataClass.TabCfg[k].ColumnNum - 1; j++)
                    {

                        lv.SubItems.Add(PublicDataClass.TabCfg[k].TabPageValue[j + 1].ValueTable[q]);
                    }
                    listViewtest.Items.Add(lv);
                }


            }
            catch { }

        }

        private void listViewtest_SubItemClicked(object sender, SubItemEventArgs e)
        {
            try
            {
                if (this.listViewtest.SelectedItems.Count == 0)
                    return;
                if (e.SubItem == 0) // Password field
                {
                    return;
                }
                if (PublicDataClass.TabCfg[ItemId - 0x14].PageName == "模拟量接入配置参数")
                {
                    return;
                }
                if (PublicDataClass.TabCfg[ItemId - 0x14].PageName == "内遥信配置参数")
                {
                    if (e.SubItem == 2) // Password field
                    {
                        if ((listViewtest.Items[e.Item.Index].SubItems[1].Text == "过流II段") || (listViewtest.Items[e.Item.Index].SubItems[1].Text == "过流I段")
                            || (listViewtest.Items[e.Item.Index].SubItems[1].Text == "零序告警") || (listViewtest.Items[e.Item.Index].SubItems[1].Text == "电流过负荷")
                            || (listViewtest.Items[e.Item.Index].SubItems[1].Text == "过流II段线路总信号") || (listViewtest.Items[e.Item.Index].SubItems[1].Text == "过流I段线路总信号")
                            || (listViewtest.Items[e.Item.Index].SubItems[1].Text == "相间短路") || (listViewtest.Items[e.Item.Index].SubItems[1].Text == "相间短路总信号"))
                        {
                            comboBoxvalue2.Items.Clear();
                            comboBoxvalue2.Items.Add("I");
                        }
                        else if ((listViewtest.Items[e.Item.Index].SubItems[1].Text == "过压告警") || (listViewtest.Items[e.Item.Index].SubItems[1].Text == "欠压告警"))
                        {
                            comboBoxvalue2.Items.Clear();
                            comboBoxvalue2.Items.Add("U");
                        }
                        else if ((listViewtest.Items[e.Item.Index].SubItems[1].Text == "断线") || (listViewtest.Items[e.Item.Index].SubItems[1].Text == "单相接地")
                             || (listViewtest.Items[e.Item.Index].SubItems[1].Text == "事故总"))
                        {
                            comboBoxvalue2.Items.Clear();
                            comboBoxvalue2.Items.Add("U");
                            comboBoxvalue2.Items.Add("I");
                        }
                        else
                        {
                            comboBoxvalue2.Items.Clear();
                        }


                    }
                    if (e.SubItem == 3) // Password field
                    {
                        if ((listViewtest.Items[e.Item.Index].SubItems[1].Text == "过流II段") || (listViewtest.Items[e.Item.Index].SubItems[1].Text == "过流I段")
                            || (listViewtest.Items[e.Item.Index].SubItems[1].Text == "电流过负荷"))
                        {
                            comboBoxvalue3.Items.Clear();
                            comboBoxvalue3.Items.Add("A");
                            comboBoxvalue3.Items.Add("B");
                            comboBoxvalue3.Items.Add("C");
                            comboBoxvalue3.Items.Add("O");
                            comboBoxvalue3.Items.Add("P");
                            comboBoxvalue3.Items.Add("Q");
                        }
                        else if ((listViewtest.Items[e.Item.Index].SubItems[1].Text == "零序告警"))
                        {
                            comboBoxvalue3.Items.Clear();
                            comboBoxvalue3.Items.Add("O");
                        }
                        else if ((listViewtest.Items[e.Item.Index].SubItems[1].Text == "相间短路"))
                        {
                            comboBoxvalue3.Items.Clear();
                            comboBoxvalue3.Items.Add("AB");
                            comboBoxvalue3.Items.Add("BC");
                            comboBoxvalue3.Items.Add("AC");
                        }
                        else if ((listViewtest.Items[e.Item.Index].SubItems[1].Text == "过压告警") || (listViewtest.Items[e.Item.Index].SubItems[1].Text == "欠压告警"))
                        {
                            comboBoxvalue3.Items.Clear();
                            comboBoxvalue3.Items.Add("A");
                            comboBoxvalue3.Items.Add("B");
                            comboBoxvalue3.Items.Add("C");
                            comboBoxvalue3.Items.Add("O");
                        }
                        else if ((listViewtest.Items[e.Item.Index].SubItems[1].Text == "断线")
                              || (listViewtest.Items[e.Item.Index].SubItems[1].Text == "事故总") || (listViewtest.Items[e.Item.Index].SubItems[1].Text == "过流II段线路总信号")
                              || (listViewtest.Items[e.Item.Index].SubItems[1].Text == "过流I段线路总信号") || (listViewtest.Items[e.Item.Index].SubItems[1].Text == "相间短路总信号"))
                        {
                            comboBoxvalue3.Items.Clear();
                            comboBoxvalue3.Items.Add("N");
                        }
                        else if ((listViewtest.Items[e.Item.Index].SubItems[1].Text == "单相接地"))
                        {
                            comboBoxvalue3.Items.Clear();
                            comboBoxvalue3.Items.Add("S");
                            comboBoxvalue3.Items.Add("C");
                        }
                        else
                        {
                            comboBoxvalue3.Items.Clear();
                        }
                    }
                    listViewtest.StartEditing(Editors5[e.SubItem], e.Item, e.SubItem);
                }
                else
                    listViewtest.StartEditing(Editors4[e.SubItem], e.Item, e.SubItem);
            }
            catch
            { }
        }

        private void listViewtest_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.listViewtest.SelectedItems.Count == 0)
                return;
            foreach (ListViewItem item in this.listViewtest.Items)
            {
                item.ForeColor = SystemColors.WindowText;

            }
            this.listViewtest.SelectedItems[0].ForeColor = Color.Red;//设置当前选择项为红色
        }

        private void comboBoxvalue_Leave(object sender, EventArgs e)
        {
            listViewtest.EndEditing(true);
            RefreshParamState();
        }


        private void textBoxvalue_Leave_1(object sender, EventArgs e)
        {
            listView1.EndEditing(true);
            listViewtest.EndEditing(true);
            RefreshParamState();
        }

        private void readbutton_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "无信息")
            {
                MessageBox.Show("无测量点信息可操作", "信息",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            else
            {
                ty = PublicFunction.CheckPointOfCommunicationEntrace(comboBox1.Text);
                if (ty == 0)
                {
                    MessageBox.Show("无测量点信息可操作", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }

            }
            //全黑色显示，清标注状态
            for (int p = 0; p < listView1.Items.Count; p++)
            {
                for (int q = 0; q < listView1.Columns.Count; q++)
                {
                    listView1.Items[p].UseItemStyleForSubItems = false;
                    listView1.Items[p].SubItems[q].ForeColor = Color.Black;
                }
            }
            for (int p = 0; p < listViewtest.Items.Count; p++)
            {
                for (int q = 0; q < listViewtest.Columns.Count; q++)
                {
                    listViewtest.Items[p].UseItemStyleForSubItems = false;
                    listViewtest.Items[p].SubItems[q].ForeColor = Color.Black;
                }
            }
            label3.Text = "";
            PublicDataClass._DataField.FieldLen = 0;
         //   PublicDataClass._DataField.FieldVSQ = 0;
            PublicDataClass.seq = 0;
            dataPos = 0;
            if (ItemId == 1)  //读取串口参数
            {
                PublicDataClass.seq = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.SQflag = 0;
                PublicDataClass.ParamInfoAddr = 0x0100;
            }
            else if (ItemId == 2)  //读取网口参数
            {
                PublicDataClass.seq = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.SQflag = 0;
                PublicDataClass.ParamInfoAddr = 0x0200;
            }
            else if (ItemId == 3)  //读取系统参数
            {
                PublicDataClass.seq = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.SQflag = 0;
                PublicDataClass.ParamInfoAddr = 0x0300;
            }
            else if (ItemId == 4)  //读取遥测配置参数
            {
                PublicDataClass.seq = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.SQflag = 0;
                PublicDataClass.ParamInfoAddr = 0x5000;
            }
            else if (ItemId == 5)  //读取遥信配置参数
            {
                PublicDataClass.seq = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.SQflag = 0;
                PublicDataClass.ParamInfoAddr = 0x6000;
            }
            else if (ItemId == 6)  //读取遥控配置参数
            {
                PublicDataClass.seq = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.SQflag = 0;
                PublicDataClass.ParamInfoAddr = 0x7000;
            }
            else if (ItemId == 7)  //读取模拟量接入参数
            {
                PublicDataClass.seq = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.SQflag = 0;
                PublicDataClass.ParamInfoAddr = 0x0400;
            }
            else if (ItemId == 8)  //读取模拟量校准参数
            {
                PublicDataClass.seq = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.SQflag = 0;
                if (checkBox2.Checked == true)
                    PublicDataClass.ParamInfoAddr = 0xd000;
                else if (checkBox3.Checked == true)
                    PublicDataClass.ParamInfoAddr = 0xe000;
                else if (checkBox4.Checked == true)
                    PublicDataClass.ParamInfoAddr = 0xf000;
            }
            else if (ItemId == 9)  //读取模拟量置数参数
            {
                PublicDataClass.seq = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.SQflag = 0;
                PublicDataClass.ParamInfoAddr = 0x0800;
            }
            else if (ItemId - 0x14 >= 0)  //动态选项卡
            {
                PublicDataClass.seq = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.SQflag = 0;
                PublicDataClass.ParamInfoAddr = PublicDataClass.TabCfg[ItemId - 0x14].DownAddr;//待定
            }

            
            PublicDataClass._Message.ReadParam = false;
            timer2.Enabled = true;
            if (ty == 1)
                PublicDataClass._ComTaskFlag.READ_P_1 = true;

            if (ty == 2)
                PublicDataClass._NetTaskFlag.READ_P_1 = true;


        }

        private void comboBoxvalue_Leave_1(object sender, EventArgs e)
        {
            listView1.EndEditing(true);
            listViewtest.EndEditing(true);
            RefreshParamState();
        }

        private void comboBoxvalue1_Leave(object sender, EventArgs e)
        {
            listView1.EndEditing(true);
            listViewtest.EndEditing(true);
            RefreshParamState();
        }

        private void comboBoxvalue2_Leave(object sender, EventArgs e)
        {
            listView1.EndEditing(true);
            listViewtest.EndEditing(true);
            RefreshParamState();
        }

        private void comboBoxvalue3_Leave(object sender, EventArgs e)
        {
            listView1.EndEditing(true);
            listViewtest.EndEditing(true);
            RefreshParamState();
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                if (ItemId <= 9)
                {
                    PublicDataClass._Mystruct.bpos = 0;
                    PublicDataClass._Mystruct.epos = listView1.Items.Count;
                }
                else
                {
                    PublicDataClass._Mystruct.bpos = 0;
                    PublicDataClass._Mystruct.epos = listViewtest.Items.Count;
                }
                CModifyViewForm2 cfm = new CModifyViewForm2();
                cfm.ShowDialog();
                if (cfm.DialogResult == DialogResult.OK)               //判断是否按下-确定-按钮
                {
                    int startpos = Convert.ToInt32(PublicDataClass._Mystruct.value);
                    if (ItemId <= 9)
                    {
                        if (PublicDataClass._Mystruct.bl == true)
                        {

                            for (int i = PublicDataClass._Mystruct.bpos; i <= PublicDataClass._Mystruct.epos; i++)
                            {
                                listView1.Items[i].SubItems[PublicDataClass._Mystruct.row].Text = Convert.ToString(startpos++);   //重新调整序号

                            }
                        }
                        else
                        {
                            for (int i = PublicDataClass._Mystruct.bpos; i <= PublicDataClass._Mystruct.epos; i++)
                            {
                                listView1.Items[i].SubItems[PublicDataClass._Mystruct.row].Text = Convert.ToString(startpos--);   //重新调整序号

                            }
                        }

                    }
                    else
                    {
                        if (PublicDataClass._Mystruct.bl == true)
                        {

                            for (int i = PublicDataClass._Mystruct.bpos; i <= PublicDataClass._Mystruct.epos; i++)
                            {
                                listViewtest.Items[i].SubItems[PublicDataClass._Mystruct.row].Text = Convert.ToString(startpos++);   //重新调整序号

                            }
                        }
                        else
                        {
                            for (int i = PublicDataClass._Mystruct.bpos; i <= PublicDataClass._Mystruct.epos; i++)
                            {
                                listViewtest.Items[i].SubItems[PublicDataClass._Mystruct.row].Text = Convert.ToString(startpos--);   //重新调整序号

                            }
                        }
                    }

                    RefreshParamState();
                }

            }
            catch
            {
                MessageBox.Show("数值输入有误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (PublicDataClass._WindowsVisable.XtParamUpdateVisable == true)  //窗体可见
            {

                if (PublicDataClass._Message.ReadParam == true)
                {
                    PublicDataClass._Message.ReadParam = false;
                    timer2.Enabled = false;

                  
                    int dex = 0;
                    ListViewItem lv;
                    int count = 0;
                    if (ItemId == 1)   //串口参数
                    {
                        for (int i = 0; i < PublicDataClass._ComParam.num; i++)
                        {
                            if (PublicDataClass._ComParam.ByteTable[i] == "2")
                            {
                                int value = 0;
                                value += PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);

                                if (Convert.ToString(value) != listView1.Items[i].SubItems[2].Text)
                                {
                                    listView1.Items[i].UseItemStyleForSubItems = false;
                                    listView1.Items[i].SubItems[2].ForeColor = Color.Red;
                                    count++;
                                }
                                listView1.Items[i].SubItems[2].Text = Convert.ToString(value);
                                dex += 2;
                            }
                        }
                   
                     

                    }
                    else if (ItemId == 2)
                    {
                        for (int i = 0; i < PublicDataClass._NetParam.num; i++)
                        {
                            if (PublicDataClass._NetParam.ByteTable[i] == "2")
                            {
                                int value = 0;
                                value += PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);
                                if (Convert.ToString(value) != listView1.Items[i].SubItems[2].Text)
                                {
                                    listView1.Items[i].UseItemStyleForSubItems = false;
                                    listView1.Items[i].SubItems[2].ForeColor = Color.Red;
                                    count++;
                                }
                                listView1.Items[i].SubItems[2].Text = Convert.ToString(value);
                                dex += 2;
                            }
                            else if (PublicDataClass._NetParam.ByteTable[i] == "4")
                            {
                                string value = @"";
                                value += PublicDataClass._DataField.Buffer[dex] + "." + PublicDataClass._DataField.Buffer[dex + 1] + "." + PublicDataClass._DataField.Buffer[dex + 2] + "." + PublicDataClass._DataField.Buffer[dex + 3];
                                if (Convert.ToString(value) != listView1.Items[i].SubItems[2].Text)
                                {
                                    listView1.Items[i].UseItemStyleForSubItems = false;
                                    listView1.Items[i].SubItems[2].ForeColor = Color.Red;
                                    count++;
                                }
                                listView1.Items[i].SubItems[2].Text = Convert.ToString(value);
                                dex += 4;
                            }
                            else if (PublicDataClass._NetParam.ByteTable[i] == "6")
                            {
                                string value = @"";
                                value += PublicDataClass._DataField.Buffer[dex] + "-" + PublicDataClass._DataField.Buffer[dex + 1] + "-" + PublicDataClass._DataField.Buffer[dex + 2]
                                          + "-" + PublicDataClass._DataField.Buffer[dex + 3] + "-" + PublicDataClass._DataField.Buffer[dex + 4] + "-" + PublicDataClass._DataField.Buffer[dex + 5];

                                if (Convert.ToString(value) != listView1.Items[i].SubItems[2].Text)
                                {
                                    listView1.Items[i].UseItemStyleForSubItems = false;
                                    listView1.Items[i].SubItems[2].ForeColor = Color.Red;
                                    count++;
                                }
                                listView1.Items[i].SubItems[2].Text = Convert.ToString(value);
                                dex += 6;
                            }

                        }

                    }
                    else if (ItemId == 3)
                    {
                        for (int i = 0; i < PublicDataClass._SysParam.num; i++)
                        {
                            if (PublicDataClass._SysParam.ByteTable[i] == "2")
                            {
                                int value = 0;
                                value += PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);
                                if (Convert.ToString(value) != listView1.Items[i].SubItems[2].Text)
                                {
                                    listView1.Items[i].UseItemStyleForSubItems = false;
                                    listView1.Items[i].SubItems[2].ForeColor = Color.Red;
                                    count++;
                                }
                                listView1.Items[i].SubItems[2].Text = Convert.ToString(value);
                                dex += 2;
                            }
                        }

                    }
                    else if (ItemId == 4)
                    {
                        try
                        {
                            for (int i = 0; i < PublicDataClass._DataField.FieldVSQ; i++)
                            {
                                int index = 0, infadr = 0;
                                byte datatype = 0, magnify = 0, linemode = 0, qufanflag = 0, setfalg;
                                int intyc = 0;
                                float fyc = 0;
                                infadr = PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);
                                //if (Convert.ToString(infadr) != listView1.Items[PublicDataClass.ParamInfoAddr - 0x5000 + i].SubItems[3].Text)
                                //{
                                //    listView1.Items[PublicDataClass.ParamInfoAddr - 0x5000 + i].UseItemStyleForSubItems = false;
                                //    listView1.Items[PublicDataClass.ParamInfoAddr - 0x5000 + i].SubItems[3].ForeColor = Color.Red;
                                //    count++;
                                //}
                                listView1.Items[PublicDataClass.ParamInfoAddr - 0x5000 + i].SubItems[3].Text = Convert.ToString(infadr);
                                dex += 2;
                            }
                            if (PublicDataClass.seqflag == 1)
                            {
                                PublicDataClass.seq = 1;
                                PublicDataClass.seqflag = 0;
                                PublicDataClass.SQflag = 0;

                                PublicDataClass.ParamInfoAddr += PublicDataClass._DataField.FieldVSQ;
                                PublicDataClass._DataField.FieldLen = 0;
                                PublicDataClass._DataField.FieldVSQ = 0;
                                if (ty == 1)
                                    PublicDataClass._ComTaskFlag.READ_P_1 = true;
                                if (ty == 2)
                                    PublicDataClass._NetTaskFlag.READ_P_1 = true;
                                timer2.Enabled = true;
                            }
                        }
                        catch
                        {
                            if (PublicDataClass.seqflag == 1)
                            {
                                PublicDataClass.seq = 1;
                                PublicDataClass.seqflag = 0;
                                PublicDataClass.SQflag = 0;

                                PublicDataClass.ParamInfoAddr += PublicDataClass._DataField.FieldVSQ;
                                PublicDataClass._DataField.FieldLen = 0;
                                PublicDataClass._DataField.FieldVSQ = 0;
                                if (ty == 1)
                                    PublicDataClass._ComTaskFlag.READ_P_1 = true;

                                if (ty == 2)
                                    PublicDataClass._NetTaskFlag.READ_P_1 = true;
                            }
                             MessageBox.Show("异常！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        //try
                        //{
                        //    for (int i = 0; i < PublicDataClass._DataField.FieldVSQ; i++)
                        //    {
                        //        int index = 0, infadr = 0;
                        //        byte datatype = 0, magnify = 0, linemode = 0, qufanflag = 0, setfalg;
                        //        int intyc = 0;
                        //        float fyc = 0;

                        //        infadr = PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);
                        //        if (Convert.ToString(PublicDataClass.ParamInfoAddr - 0x5000 +16385+i) != listView1.Items[infadr].SubItems[3].Text)
                        //        {
                        //            listView1.Items[infadr].UseItemStyleForSubItems = false;
                        //            listView1.Items[infadr].SubItems[3].ForeColor = Color.Red;
                        //        }
                        //        listView1.Items[infadr].SubItems[3].Text = Convert.ToString(PublicDataClass.ParamInfoAddr - 0x5000 + 16385 + i);
                        //        dex += 2;
                        //    }
                        //    if (PublicDataClass.seqflag == 1)
                        //    {
                        //        PublicDataClass.seq = 1;
                        //        PublicDataClass.seqflag = 0;
                        //        PublicDataClass.SQflag = 0;

                        //        PublicDataClass.ParamInfoAddr += PublicDataClass._DataField.FieldVSQ;
                        //        PublicDataClass._DataField.FieldLen = 0;
                        //        PublicDataClass._DataField.FieldVSQ = 0;
                        //        if (ty == 1)
                        //            PublicDataClass._ComTaskFlag.READ_P_1 = true;
                        //        if (ty == 2)
                        //            PublicDataClass._NetTaskFlag.READ_P_1 = true;
                        //        timer2.Enabled = true;
                        //    }
                        //}
                        //catch 
                        //{
                        //    if (PublicDataClass.seqflag == 1)
                        //    {
                        //        PublicDataClass.seq = 1;
                        //        PublicDataClass.seqflag = 0;
                        //        PublicDataClass.SQflag = 0;

                        //        PublicDataClass.ParamInfoAddr+= PublicDataClass._DataField.FieldVSQ;
                        //        PublicDataClass._DataField.FieldLen = 0;
                        //        PublicDataClass._DataField.FieldVSQ = 0;
                        //        if (ty == 1)
                        //            PublicDataClass._ComTaskFlag.READ_P_1 = true;

                        //        if (ty == 2)
                        //            PublicDataClass._NetTaskFlag.READ_P_1 = true;
                        //    }
                        //}

                    }
                    else if (ItemId == 5)  //遥信配置
                    {
                        try
                        {
                            for (int i = 0; i < PublicDataClass._DataField.FieldVSQ; i++)
                            {
                                int index = 0, infadr = 0;
                                byte qufanflag = 0, setfalg;
                                int yx = 0;


                                //index = PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);
                                //if (Convert.ToString(index) != listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[2].Text)
                                //{
                                //    listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].UseItemStyleForSubItems = false;
                                //    listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[2].ForeColor = Color.Red;
                                //}
                                //listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[2].Text = Convert.ToString(index);
                                //dex += 2;

                                infadr = PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);
                                if (Convert.ToString(infadr) != listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[3].Text)
                                {
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].UseItemStyleForSubItems = false;
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[3].ForeColor = Color.Red;
                                    count++;
                                }
                                listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[3].Text = Convert.ToString(infadr);
                                dex += 2;

                                qufanflag = (byte)(PublicDataClass._DataField.Buffer[dex] & 0x01);
                                if (qufanflag == 0)
                                {
                                    if (listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[4].Text != "否")
                                    {
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].UseItemStyleForSubItems = false;
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[4].ForeColor = Color.Red;
                                        count++;
                                    }
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[4].Text = "否";
                                }
                                else if (qufanflag == 1)
                                {
                                    if (listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[4].Text != "是")
                                    {
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].UseItemStyleForSubItems = false;
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[4].ForeColor = Color.Red;
                                        count++;
                                    }
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[4].Text = "是";
                                }
                                else
                                {
                                    if (listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[4].Text != "==")
                                    {
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].UseItemStyleForSubItems = false;
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[4].ForeColor = Color.Red;
                                        count++;
                                    }
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[4].Text = "==";
                                }


                                //setfalg = (byte)((PublicDataClass._DataField.Buffer[dex] & 0x02) >> 1);
                                //if (setfalg == 0)
                                //{
                                //    if (listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[5].Text != "否")
                                //    {
                                //        listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].UseItemStyleForSubItems = false;
                                //        listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[5].ForeColor = Color.Red;
                                //    }
                                //    listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[5].Text = "否";
                                //}
                                //else if (setfalg == 1)
                                //{
                                //    if (listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[5].Text != "是")
                                //    {
                                //        listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].UseItemStyleForSubItems = false;
                                //        listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[5].ForeColor = Color.Red;
                                //    }
                                //    listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[5].Text = "是";
                                //}

                                //else
                                //{
                                //    if (listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[5].Text != "==")
                                //    {
                                //        listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].UseItemStyleForSubItems = false;
                                //        listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[5].ForeColor = Color.Red;
                                //    }
                                //    listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[5].Text = "==";
                                //}

                                //yx = (PublicDataClass._DataField.Buffer[dex] & 0x04) >> 2;
                                //if (listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[6].Text != String.Format("{0:d}", yx))
                                //{
                                //    listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].UseItemStyleForSubItems = false;
                                //    listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[6].ForeColor = Color.Red;
                                //}
                                //listView1.Items[PublicDataClass.ParamInfoAddr - 0x6000 + i].SubItems[6].Text = String.Format("{0:d}", yx);

                                dex += 1;


                            }
                            if (PublicDataClass.seqflag == 1)
                            {
                                PublicDataClass.seq = 1;
                                PublicDataClass.seqflag = 0;
                                PublicDataClass.SQflag = 0;

                                PublicDataClass.ParamInfoAddr += PublicDataClass._DataField.FieldVSQ;
                                PublicDataClass._DataField.FieldLen = 0;
                                PublicDataClass._DataField.FieldVSQ = 0;
                                if (ty == 1)
                                    PublicDataClass._ComTaskFlag.READ_P_1 = true;
                                if (ty == 2)
                                    PublicDataClass._NetTaskFlag.READ_P_1 = true;
                                timer2.Enabled = true;
                            }
                        }
                        catch
                        {
                            if (PublicDataClass.seqflag == 1)
                            {
                                PublicDataClass.seq = 1;
                                PublicDataClass.seqflag = 0;
                                PublicDataClass.SQflag = 0;

                                PublicDataClass.ParamInfoAddr += PublicDataClass._DataField.FieldVSQ;
                                PublicDataClass._DataField.FieldLen = 0;
                                PublicDataClass._DataField.FieldVSQ = 0;
                                if (ty == 1)
                                    PublicDataClass._ComTaskFlag.READ_P_1 = true;

                                if (ty == 2)
                                    PublicDataClass._NetTaskFlag.READ_P_1 = true;
                                timer2.Enabled = true;
                            }
                            MessageBox.Show("异常！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                    }
                    else if (ItemId == 6)  //遥控配置
                    {
                        try
                        {
                            for (int i = 0; i < PublicDataClass._DataField.FieldVSQ; i++)
                            {
                                int qual, triggermodeTable, secltimeTable, exetimeTable, pulsewidthTable, saveflagTable, powerTable, jdq1Table, jdq2Table, fjyx1Table, fjyx2Table;

                                qual = PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);
                                if (listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[2].Text != Convert.ToString(qual))
                                {
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].UseItemStyleForSubItems = false;
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[2].ForeColor = Color.Red;
                                    count++;
                                }
                                listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[2].Text = Convert.ToString(qual);
                                dex += 2;

                                triggermodeTable = PublicDataClass._DataField.Buffer[dex];
                                if (triggermodeTable == 0)
                                {
                                    if (listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[3].Text != "电平")
                                    {
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].UseItemStyleForSubItems = false;
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[3].ForeColor = Color.Red;
                                        count++;
                                    }
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[3].Text = "电平";
                                }
                                else if (triggermodeTable == 1)
                                {
                                    if (listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[3].Text != "脉冲")
                                    {
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].UseItemStyleForSubItems = false;
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[3].ForeColor = Color.Red;
                                        count++;
                                    }
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[3].Text = "脉冲";
                                }
                                else
                                {
                                    if (listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[3].Text != "==")
                                    {
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].UseItemStyleForSubItems = false;
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[3].ForeColor = Color.Red;
                                        count++;
                                    }
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[3].Text = "==";
                                }
                                dex += 1;

                                secltimeTable = PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);
                                if (listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[4].Text != Convert.ToString(secltimeTable))
                                {
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].UseItemStyleForSubItems = false;
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[4].ForeColor = Color.Red;
                                    count++;
                                }
                                listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[4].Text = Convert.ToString(secltimeTable);
                                dex += 2;

                                exetimeTable = PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);
                                if (listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[5].Text != Convert.ToString(exetimeTable))
                                {
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].UseItemStyleForSubItems = false;
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[5].ForeColor = Color.Red;
                                    count++;
                                }
                                listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[5].Text = Convert.ToString(exetimeTable);
                                dex += 2;

                                pulsewidthTable = PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);
                                if (listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[6].Text != Convert.ToString(pulsewidthTable))
                                {
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].UseItemStyleForSubItems = false;
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[6].ForeColor = Color.Red;
                                    count++;
                                }
                                listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[6].Text = Convert.ToString(pulsewidthTable);
                                dex += 2;

                                saveflagTable = PublicDataClass._DataField.Buffer[dex];
                                if (saveflagTable == 0)
                                {
                                    if (listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[7].Text != "否")
                                    {
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].UseItemStyleForSubItems = false;
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[7].ForeColor = Color.Red;
                                        count++;
                                    }
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[7].Text = "否";
                                }
                                else if (saveflagTable == 1)
                                {
                                    if (listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[7].Text != "是")
                                    {
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].UseItemStyleForSubItems = false;
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[7].ForeColor = Color.Red;
                                        count++;
                                    }
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[7].Text = "是";
                                }
                                else
                                {
                                    if (listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[7].Text != "==")
                                    {
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].UseItemStyleForSubItems = false;
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[7].ForeColor = Color.Red;
                                        count++;
                                    }
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[7].Text = "==";
                                }
                                dex += 1;

                                powerTable = PublicDataClass._DataField.Buffer[dex];
                                if (powerTable == 0)
                                {
                                    if (listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[8].Text != "无")
                                    {
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].UseItemStyleForSubItems = false;
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[8].ForeColor = Color.Red;
                                        count++;
                                    }
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[8].Text = "无";
                                }
                                else if (powerTable == 1)
                                {
                                    if (listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[8].Text != "24V")
                                    {
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].UseItemStyleForSubItems = false;
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[8].ForeColor = Color.Red;
                                        count++;
                                    }
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[8].Text = "24V";
                                }
                                else if (powerTable == 2)
                                {
                                    if (listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[8].Text != "电操机构")
                                    {
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].UseItemStyleForSubItems = false;
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[8].ForeColor = Color.Red;
                                        count++;
                                    }
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[8].Text = "电操机构";
                                }
                                else if (powerTable == 3)
                                {
                                    if (listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[8].Text != "电源+电操")
                                    {
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].UseItemStyleForSubItems = false;
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[8].ForeColor = Color.Red;
                                        count++;
                                    }
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[8].Text = "电源+电操";
                                }
                                else
                                {
                                    if (listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[8].Text != "==")
                                    {
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].UseItemStyleForSubItems = false;
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[8].ForeColor = Color.Red;
                                        count++;
                                    }
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[8].Text = "==";
                                }
                                dex += 1;

                                jdq1Table = PublicDataClass._DataField.Buffer[dex];
                                if (listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[9].Text != Convert.ToString(jdq1Table))
                                {
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].UseItemStyleForSubItems = false;
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[9].ForeColor = Color.Red;
                                    count++;
                                }
                                listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[9].Text = Convert.ToString(jdq1Table);
                                dex += 1;


                                jdq2Table = PublicDataClass._DataField.Buffer[dex];
                                if (listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[10].Text != Convert.ToString(jdq2Table))
                                {
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].UseItemStyleForSubItems = false;
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[10].ForeColor = Color.Red;
                                    count++;
                                }
                                listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[10].Text = Convert.ToString(jdq2Table);
                                dex += 1;

                                fjyx1Table = PublicDataClass._DataField.Buffer[dex];
                                if (listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[11].Text != Convert.ToString(fjyx1Table))
                                {
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].UseItemStyleForSubItems = false;
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[11].ForeColor = Color.Red;
                                    count++;
                                }
                                listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[11].Text = Convert.ToString(fjyx1Table);
                                dex += 1;

                                fjyx2Table = PublicDataClass._DataField.Buffer[dex];
                                if (listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[12].Text != Convert.ToString(fjyx2Table))
                                {
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].UseItemStyleForSubItems = false;
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[12].ForeColor = Color.Red;
                                    count++;
                                }
                                listView1.Items[PublicDataClass.ParamInfoAddr - 0x7000 + i].SubItems[12].Text = Convert.ToString(fjyx2Table);
                                dex += 1;
                            }
                            if (PublicDataClass.seqflag == 1)
                            {
                                PublicDataClass.seq = 1;
                                PublicDataClass.seqflag = 0;
                                PublicDataClass.SQflag = 0;

                                PublicDataClass.ParamInfoAddr += PublicDataClass._DataField.FieldVSQ;
                                PublicDataClass._DataField.FieldLen = 0;
                                PublicDataClass._DataField.FieldVSQ = 0;
                                if (ty == 1)
                                    PublicDataClass._ComTaskFlag.READ_P_1 = true;

                                if (ty == 2)
                                    PublicDataClass._NetTaskFlag.READ_P_1 = true;
                                timer2.Enabled = true;

                            }
                        }
                        catch
                        {
                            if (PublicDataClass.seqflag == 1)
                            {
                                PublicDataClass.seq = 1;
                                PublicDataClass.seqflag = 0;
                                PublicDataClass.SQflag = 0;

                                PublicDataClass.ParamInfoAddr += PublicDataClass._DataField.FieldVSQ;
                                PublicDataClass._DataField.FieldLen = 0;
                                PublicDataClass._DataField.FieldVSQ = 0;
                                if (ty == 1)
                                    PublicDataClass._ComTaskFlag.READ_P_1 = true;

                                if (ty == 2)
                                    PublicDataClass._NetTaskFlag.READ_P_1 = true;
                                timer2.Enabled = true;
                            }
                            MessageBox.Show("异常！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                    }
                    else if (ItemId == 7)
                    {
                        try
                        {
                            for (int i = 0; i < PublicDataClass._DataField.FieldVSQ; i++)
                            {
                                byte value = 0;
                                value = PublicDataClass._DataField.Buffer[dex];
                                if (Convert.ToChar(value).ToString() != listView1.Items[PublicDataClass.ParamInfoAddr - 0x400 + i].SubItems[1].Text)
                                {
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x400 + i].UseItemStyleForSubItems = false;
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x400 + i].SubItems[1].ForeColor = Color.Red;
                                    count++;
                                }
                                listView1.Items[PublicDataClass.ParamInfoAddr - 0x400 + i].SubItems[1].Text = Convert.ToChar(value).ToString();
                                dex += 1;

                                value = PublicDataClass._DataField.Buffer[dex];
                                if (Convert.ToChar(value).ToString() != listView1.Items[PublicDataClass.ParamInfoAddr - 0x400 + i].SubItems[2].Text)
                                {
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x400 + i].UseItemStyleForSubItems = false;
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x400 + i].SubItems[2].ForeColor = Color.Red;
                                    count++;
                                }
                                listView1.Items[PublicDataClass.ParamInfoAddr - 0x400 + i].SubItems[2].Text = Convert.ToChar(value).ToString();
                                dex += 1;

                                value = PublicDataClass._DataField.Buffer[dex];
                                if (Convert.ToChar(value).ToString() != listView1.Items[PublicDataClass.ParamInfoAddr - 0x400 + i].SubItems[3].Text)
                                {
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x400 + i].UseItemStyleForSubItems = false;
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x400 + i].SubItems[3].ForeColor = Color.Red;
                                    count++;
                                }
                                listView1.Items[PublicDataClass.ParamInfoAddr - 0x400 + i].SubItems[3].Text = Convert.ToString(value);
                                dex += 1;

                                value = PublicDataClass._DataField.Buffer[dex];
                                if (Convert.ToChar(value).ToString() != listView1.Items[PublicDataClass.ParamInfoAddr - 0x400 + i].SubItems[4].Text)
                                {
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x400 + i].UseItemStyleForSubItems = false;
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x400 + i].SubItems[4].ForeColor = Color.Red;
                                    count++;
                                }
                                listView1.Items[PublicDataClass.ParamInfoAddr - 0x400 + i].SubItems[4].Text = Convert.ToString(value);
                                dex += 1;
                            }
                            if (PublicDataClass.seqflag == 1)
                            {
                                PublicDataClass.seq = 1;
                                PublicDataClass.seqflag = 0;
                                PublicDataClass.SQflag = 0;

                                PublicDataClass.ParamInfoAddr += PublicDataClass._DataField.FieldVSQ;
                                PublicDataClass._DataField.FieldLen = 0;
                                PublicDataClass._DataField.FieldVSQ = 0;
                                if (ty == 1)
                                    PublicDataClass._ComTaskFlag.READ_P_1 = true;

                                if (ty == 2)
                                    PublicDataClass._NetTaskFlag.READ_P_1 = true;
                                timer2.Enabled = true;
                            }
                        }
                        catch
                        {
                            if (PublicDataClass.seqflag == 1)
                            {
                                PublicDataClass.seq = 1;
                                PublicDataClass.seqflag = 0;
                                PublicDataClass.SQflag = 0;

                                PublicDataClass.ParamInfoAddr += PublicDataClass._DataField.FieldVSQ;
                                PublicDataClass._DataField.FieldLen = 0;
                                PublicDataClass._DataField.FieldVSQ = 0;
                                if (ty == 1)
                                    PublicDataClass._ComTaskFlag.READ_P_1 = true;

                                if (ty == 2)
                                    PublicDataClass._NetTaskFlag.READ_P_1 = true;
                                timer2.Enabled = true;
                            }
                            MessageBox.Show("异常！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else if (ItemId == 8)
                    {
                        float value1 = 0;
                        float value2 = 0;
                        try
                        {
                            for (int i = 0; i < PublicDataClass._DataField.FieldVSQ; i++)
                            {
                                byte[] bytes = new byte[4];
                                bytes[0] = PublicDataClass._DataField.Buffer[dex];
                                bytes[1] = PublicDataClass._DataField.Buffer[dex + 1];
                                bytes[2] = PublicDataClass._DataField.Buffer[dex + 2];
                                bytes[3] = PublicDataClass._DataField.Buffer[dex + 3];
                                value1 = BitConverter.ToSingle(bytes, 0);
                                dex += 4;

                                bytes[0] = PublicDataClass._DataField.Buffer[dex];
                                bytes[1] = PublicDataClass._DataField.Buffer[dex + 1];
                                bytes[2] = PublicDataClass._DataField.Buffer[dex + 2];
                                bytes[3] = PublicDataClass._DataField.Buffer[dex + 3];
                                value2 = BitConverter.ToSingle(bytes, 0);
                                dex += 4;

                                if (PublicDataClass.ParamInfoAddr >= 0xd000 && PublicDataClass.ParamInfoAddr <= 0xdfff)
                                {
                                    checkBox2.Checked = true;
                                    if (String.Format("{0:f4}", value1) != listView1.Items[PublicDataClass.ParamInfoAddr - 0xd000 + i].SubItems[5].Text)
                                    {
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0xd000 + i].UseItemStyleForSubItems = false;
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0xd000 + i].SubItems[5].ForeColor = Color.Red;
                                        count++;
                                    }
                                    if (String.Format("{0:f4}", value2) != listView1.Items[PublicDataClass.ParamInfoAddr - 0xd000 + i].SubItems[6].Text)
                                    {
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0xd000 + i].UseItemStyleForSubItems = false;
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0xd000 + i].SubItems[6].ForeColor = Color.Red;
                                        count++;
                                    }
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0xd000 + i].SubItems[5].Text = String.Format("{0:f4}", value1);
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0xd000 + i].SubItems[6].Text = String.Format("{0:f4}", value2);
                                }
                                else if (PublicDataClass.ParamInfoAddr >= 0xe000 && PublicDataClass.ParamInfoAddr <= 0xefff)
                                {
                                    checkBox3.Checked = true;
                                    if (String.Format("{0:f4}", value1) != listView1.Items[PublicDataClass.ParamInfoAddr - 0xe000 + i].SubItems[5].Text)
                                    {
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0xe000 + i].UseItemStyleForSubItems = false;
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0xe000 + i].SubItems[5].ForeColor = Color.Red;
                                        count++;
                                    }
                                    if (String.Format("{0:f4}", value2) != listView1.Items[PublicDataClass.ParamInfoAddr - 0xe000 + i].SubItems[6].Text)
                                    {
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0xe000 + i].UseItemStyleForSubItems = false;
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0xe000 + i].SubItems[6].ForeColor = Color.Red;
                                        count++;
                                    }
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0xe000 + i].SubItems[5].Text = String.Format("{0:f4}", value1);
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0xe000 + i].SubItems[6].Text = String.Format("{0:f4}", value2);
                                }
                                else if (PublicDataClass.ParamInfoAddr >= 0xf000 && PublicDataClass.ParamInfoAddr <= 0xffff)
                                {
                                    checkBox4.Checked = true;
                                    if (String.Format("{0:f4}", value1) != listView1.Items[PublicDataClass.ParamInfoAddr - 0xf000 + i].SubItems[5].Text)
                                    {
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0xf000 + i].UseItemStyleForSubItems = false;
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0xf000 + i].SubItems[5].ForeColor = Color.Red;
                                        count++;
                                    }
                                    if (String.Format("{0:f4}", value2) != listView1.Items[PublicDataClass.ParamInfoAddr - 0xf000 + i].SubItems[6].Text)
                                    {
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0xf000 + i].UseItemStyleForSubItems = false;
                                        listView1.Items[PublicDataClass.ParamInfoAddr - 0xf000 + i].SubItems[6].ForeColor = Color.Red;
                                        count++;
                                    }
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0xf000 + i].SubItems[5].Text = String.Format("{0:f4}", value1);
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0xf000 + i].SubItems[6].Text = String.Format("{0:f4}", value2);
                                }



                            }
                            if (PublicDataClass.seqflag == 1)
                            {
                                PublicDataClass.seq = 1;
                                PublicDataClass.seqflag = 0;
                                PublicDataClass.SQflag = 0;

                                PublicDataClass.ParamInfoAddr += PublicDataClass._DataField.FieldVSQ;
                                PublicDataClass._DataField.FieldLen = 0;
                                PublicDataClass._DataField.FieldVSQ = 0;
                                if (ty == 1)
                                    PublicDataClass._ComTaskFlag.READ_P_1 = true;

                                if (ty == 2)
                                    PublicDataClass._NetTaskFlag.READ_P_1 = true;
                                timer2.Enabled = true;
                            }
                        }
                        catch
                        {
                            if (PublicDataClass.seqflag == 1)
                            {
                                PublicDataClass.seq = 1;
                                PublicDataClass.seqflag = 0;
                                PublicDataClass.SQflag = 0;

                                PublicDataClass.ParamInfoAddr += PublicDataClass._DataField.FieldVSQ;
                                PublicDataClass._DataField.FieldLen = 0;
                                PublicDataClass._DataField.FieldVSQ = 0;
                                if (ty == 1)
                                    PublicDataClass._ComTaskFlag.READ_P_1 = true;

                                if (ty == 2)
                                    PublicDataClass._NetTaskFlag.READ_P_1 = true;
                                timer2.Enabled = true;
                            }
                            MessageBox.Show("异常！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                    }
                    else if (ItemId == 9)
                    {
                        float value1 = 0;
                        try
                        {
                            for (int i = 0; i < PublicDataClass._DataField.FieldVSQ; i++)
                            {
                                byte[] bytes = new byte[4];
                                bytes[0] = PublicDataClass._DataField.Buffer[dex];
                                bytes[1] = PublicDataClass._DataField.Buffer[dex + 1];
                                bytes[2] = PublicDataClass._DataField.Buffer[dex + 2];
                                bytes[3] = PublicDataClass._DataField.Buffer[dex + 3];
                                value1 = BitConverter.ToSingle(bytes, 0);
                                dex += 4;
                                if (String.Format("{0:f4}", value1) != listView1.Items[PublicDataClass.ParamInfoAddr - 0x800 + i].SubItems[7].Text)
                                {
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x800 + i].UseItemStyleForSubItems = false;
                                    listView1.Items[PublicDataClass.ParamInfoAddr - 0x800 + i].SubItems[7].ForeColor = Color.Red;
                                    count++;
                                }
                                listView1.Items[PublicDataClass.ParamInfoAddr - 0x800 + i].SubItems[7].Text = String.Format("{0:f4}", value1);
                            }
                            if (PublicDataClass.seqflag == 1)
                            {
                                PublicDataClass.seq = 1;
                                PublicDataClass.seqflag = 0;
                                PublicDataClass.SQflag = 0;

                                PublicDataClass.ParamInfoAddr += PublicDataClass._DataField.FieldVSQ;
                                PublicDataClass._DataField.FieldLen = 0;
                                PublicDataClass._DataField.FieldVSQ = 0;
                                if (ty == 1)
                                    PublicDataClass._ComTaskFlag.READ_P_1 = true;

                                if (ty == 2)
                                    PublicDataClass._NetTaskFlag.READ_P_1 = true;
                                timer2.Enabled = true;
                            }
                        }
                        catch
                        {
                            if (PublicDataClass.seqflag == 1)
                            {
                                PublicDataClass.seq = 1;
                                PublicDataClass.seqflag = 0;
                                PublicDataClass.SQflag = 0;

                                PublicDataClass.ParamInfoAddr += PublicDataClass._DataField.FieldVSQ;
                                PublicDataClass._DataField.FieldLen = 0;
                                PublicDataClass._DataField.FieldVSQ = 0;
                                if (ty == 1)
                                    PublicDataClass._ComTaskFlag.READ_P_1 = true;

                                if (ty == 2)
                                    PublicDataClass._NetTaskFlag.READ_P_1 = true;
                                timer2.Enabled = true;
                            }
                            MessageBox.Show("异常！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                    }
                    else if (ItemId - 0x14 >= 0)  //动态选项卡
                    {
                        if (PublicDataClass.TabCfg[ItemId - 0x14].PageName == "内遥信配置参数")
                        {
                            try
                            { 
                                for (int q = 0; q < PublicDataClass._DataField.FieldVSQ; q++)
                                {
                                    byte value = 0;
                                    value = PublicDataClass._DataField.Buffer[dex];

                                    if (value == 1)
                                    {
                                        if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text != "过流II段")
                                        {
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].ForeColor = Color.Red;
                                            count++;
                                        }
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text = "过流II段";

                                    }
                                    else if (value == 2)
                                    {
                                        if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text != "过流I段")
                                        {
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].ForeColor = Color.Red;
                                            count++;
                                        }
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text = "过流I段";
                                    }
                                    else if (value == 3)
                                    {
                                        if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text != "零序告警")
                                        {
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].ForeColor = Color.Red;
                                            count++;
                                        }
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text = "零序告警";
                                    }
                                    else if (value == 4)
                                    {
                                        if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text != "电流过负荷")
                                        {
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].ForeColor = Color.Red;
                                            count++;
                                        }
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text = "电流过负荷";
                                    }
                                    else if (value == 5)
                                    {
                                        if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text != "过压告警")
                                        {
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].ForeColor = Color.Red;
                                            count++;
                                        }
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text = "过压告警";
                                    }
                                    else if (value == 6)
                                    {
                                        if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text != "欠压告警")
                                        {
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].ForeColor = Color.Red;
                                            count++;
                                        }
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text = "欠压告警";
                                    }
                                    else if (value == 7)
                                    {
                                        if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text != "断线")
                                        {
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].ForeColor = Color.Red;
                                            count++;
                                        }
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text = "断线";
                                    }
                                    else if (value == 8)
                                    {
                                        if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text != "单相接地")
                                        {
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].ForeColor = Color.Red;
                                            count++;
                                        }
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text = "单相接地";
                                    }
                                    else if (value == 9)
                                    {
                                        if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text != "相间短路")
                                        {
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].ForeColor = Color.Red;
                                            count++;
                                        }
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text = "相间短路";
                                    }
                                    else if (value == 10)
                                    {
                                        if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text != "事故总")
                                        {
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].ForeColor = Color.Red;
                                            count++;
                                        }
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text = "事故总";
                                    }
                                    else if (value == 11)
                                    {
                                        if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text != "过流II段线路总信号")
                                        {
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].ForeColor = Color.Red;
                                            count++;
                                        }
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text = "过流II段线路总信号";
                                    }
                                    else if (value == 12)
                                    {
                                        if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text != "过流I段线路总信号")
                                        {
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].ForeColor = Color.Red;
                                            count++;
                                        }
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text = "过流I段线路总信号";
                                    }
                                    else if (value == 13)
                                    {
                                        if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text != "相间短路总信号")
                                        {
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].ForeColor = Color.Red;
                                            count++;
                                        }
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text = "相间短路总信号";
                                    }
                                    else if (value == 255)
                                    {
                                        if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text != "255")
                                        {
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].ForeColor = Color.Red;
                                            count++;
                                        }
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text = "255";
                                    }
                                    dex += 1;

                                    value = PublicDataClass._DataField.Buffer[dex];
                                    if (Convert.ToChar(value).ToString() != listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[2].Text)
                                    {
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[2].ForeColor = Color.Red;
                                        count++;
                                    }
                                    listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[2].Text = Convert.ToChar(value).ToString();
                                    dex += 1;

                                    value = PublicDataClass._DataField.Buffer[dex];
                                    if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[1].Text == "相间短路")
                                    {
                                        if (value == 65)
                                        {
                                            if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[3].Text != "BC")
                                            {
                                                listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                                listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[3].ForeColor = Color.Red;
                                                count++;
                                            }
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[3].Text = "BC";

                                        }
                                        else if (value == 66)
                                        {
                                            if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[3].Text != "AC")
                                            {
                                                listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                                listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[3].ForeColor = Color.Red;
                                                count++;
                                            }
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[3].Text = "AC";

                                        }
                                        else if (value == 67)
                                        {
                                            if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[3].Text != "AB")
                                            {
                                                listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                                listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[3].ForeColor = Color.Red;
                                                count++;
                                            }
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[3].Text = "AB";

                                        }
                                    }
                                    else
                                    {
                                        if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[3].Text != "BC")
                                        {
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[3].ForeColor = Color.Red;
                                            count++;
                                        }
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[3].Text = Convert.ToChar(value).ToString();

                                    } dex += 1;

                                    value = PublicDataClass._DataField.Buffer[dex];
                                    if (Convert.ToString(value) != listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[4].Text)
                                    {
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[4].ForeColor = Color.Red;
                                        count++;
                                    }
                                    listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[4].Text = Convert.ToString(value);
                                    dex += 1;

                                    byte[] bytes = new byte[4];
                                    bytes[0] = PublicDataClass._DataField.Buffer[dex];
                                    bytes[1] = PublicDataClass._DataField.Buffer[dex + 1];
                                    bytes[2] = PublicDataClass._DataField.Buffer[dex + 2];
                                    bytes[3] = PublicDataClass._DataField.Buffer[dex + 3];

                                    float fdata = BitConverter.ToSingle(bytes, 0);
                                    if (String.Format("{0:f4}", fdata) != listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[5].Text)
                                    {
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[5].ForeColor = Color.Red;
                                        count++;
                                    }
                                    listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[5].Text = String.Format("{0:f4}", fdata);
                                    dex += 4;

                                    bytes = new byte[4];
                                    bytes[0] = PublicDataClass._DataField.Buffer[dex];
                                    bytes[1] = PublicDataClass._DataField.Buffer[dex + 1];
                                    bytes[2] = PublicDataClass._DataField.Buffer[dex + 2];
                                    bytes[3] = PublicDataClass._DataField.Buffer[dex + 3];

                                    fdata = BitConverter.ToSingle(bytes, 0);
                                    if (String.Format("{0:f4}", fdata) != listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[6].Text)
                                    {
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[6].ForeColor = Color.Red;
                                        count++;
                                    }
                                    listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[6].Text = String.Format("{0:f4}", fdata);
                                    dex += 4;
                                }
                                if (PublicDataClass.seqflag == 1)
                                {
                                    PublicDataClass.seq = 1;
                                    PublicDataClass.seqflag = 0;
                                    PublicDataClass.SQflag = 0;
                                    PublicDataClass.ParamInfoAddr += PublicDataClass._DataField.FieldVSQ;
                                    PublicDataClass._DataField.FieldLen = 0;
                                    PublicDataClass._DataField.FieldVSQ = 0;
                                    if (ty == 1)
                                        PublicDataClass._ComTaskFlag.READ_P_1 = true;

                                    if (ty == 2)
                                        PublicDataClass._NetTaskFlag.READ_P_1 = true;
                                    timer2.Enabled = true;
                                }
                            }
                            catch
                            {
                                if (PublicDataClass.seqflag == 1)
                                {
                                    PublicDataClass.seq = 1;
                                    PublicDataClass.seqflag = 0;
                                    PublicDataClass.SQflag = 0;
                                    PublicDataClass.ParamInfoAddr += PublicDataClass._DataField.FieldVSQ;
                                    PublicDataClass._DataField.FieldLen = 0;
                                    PublicDataClass._DataField.FieldVSQ = 0;
                                    if (ty == 1)
                                        PublicDataClass._ComTaskFlag.READ_P_1 = true;

                                    if (ty == 2)
                                        PublicDataClass._NetTaskFlag.READ_P_1 = true;
                                    timer2.Enabled = true;
                                }
                                MessageBox.Show("异常！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else if (PublicDataClass.TabCfg[ItemId - 0x14].PageName == "GPRS参数")
                        {
                            try
                            {
                                for (int q = 0; q < PublicDataClass._DataField.FieldVSQ; q++)
                                {
                                    if (q <= 1)
                                    {
                                        string value = @"";
                                        value += PublicDataClass._DataField.Buffer[dex] + "." + PublicDataClass._DataField.Buffer[dex + 1] + "." + PublicDataClass._DataField.Buffer[dex + 2] + "." + PublicDataClass._DataField.Buffer[dex + 3];
                                        if (Convert.ToString(value) != listViewtest.Items[q].SubItems[2].Text)
                                        {
                                            listViewtest.Items[q].UseItemStyleForSubItems = false;
                                            listViewtest.Items[q].SubItems[2].ForeColor = Color.Red;
                                        }
                                        listViewtest.Items[q].SubItems[2].Text = Convert.ToString(value);
                                        dex += 4;
                                    }
                                    else if (q <= 3)
                                    {
                                        int value = 0;
                                        value += PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);
                                        if (Convert.ToString(value) != listViewtest.Items[q].SubItems[2].Text)
                                        {
                                            listViewtest.Items[q].UseItemStyleForSubItems = false;
                                            listViewtest.Items[q].SubItems[2].ForeColor = Color.Red;
                                        }
                                        listViewtest.Items[q].SubItems[2].Text = Convert.ToString(value);
                                        dex += 2;
                                    }
                                    else if (q == 4)
                                    {
                                        string value = @"";
                                        for (int i = 0; i < 16; i++)
                                        {
                                            value += Convert.ToChar(PublicDataClass._DataField.Buffer[dex]);
                                            dex += 1;
                                        }
                                        if (value != listViewtest.Items[q].SubItems[2].Text)
                                        {
                                            listViewtest.Items[q].UseItemStyleForSubItems = false;
                                            listViewtest.Items[q].SubItems[2].ForeColor = Color.Red;
                                        }
                                        listViewtest.Items[q].SubItems[2].Text = Convert.ToString(value);
                                    }
                                }
                            }
                            catch
                            {
                                MessageBox.Show("异常！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                        }
                        else if (PublicDataClass.TabCfg[ItemId - 0x14].PageName == "模拟量接入配置参数")
                        {
                            try
                            {

                                for (int q = 0; q < PublicDataClass._DataField.FieldVSQ; q++)
                                {
                                    for (int col = 0; col < PublicDataClass.TabCfg[ItemId - 0x14].ColumnNum; col++)
                                    {

                                        if (PublicDataClass.TabCfg[ItemId - 0x14].ColumnDownByte[col] == "1")
                                        {
                                            byte value = 0;
                                            value = PublicDataClass._DataField.Buffer[dex];
                                            if ((Convert.ToChar(value) >= 'A') && (Convert.ToChar(value) <= 'Z'))
                                            {
                                                if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text != Convert.ToChar(value).ToString())
                                                {
                                                    listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                                    listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].ForeColor = Color.Red;
                                                    count++;
                                                }
                                                listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text = Convert.ToChar(value).ToString();
                                            }
                                            else
                                            {
                                                if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text != Convert.ToString(value))
                                                {
                                                    listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                                    listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].ForeColor = Color.Red;
                                                    count++;
                                                }
                                                listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text = Convert.ToString(value);
                                            }
                                            dex += 1;
                                        }
                                        else if (PublicDataClass.TabCfg[ItemId - 0x14].ColumnDownByte[col] == "2")
                                        {
                                            int value = 0;
                                            value += PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);
                                            if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text != Convert.ToString(value))
                                            {
                                                listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                                listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].ForeColor = Color.Red;
                                                count++;
                                            }
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text = Convert.ToString(value);
                                            dex += 2;
                                        }
                                        else if (PublicDataClass.TabCfg[ItemId - 0x14].ColumnDownByte[col] == "4")
                                        {
                                            byte[] bytes = new byte[4];
                                            bytes[0] = PublicDataClass._DataField.Buffer[dex];
                                            bytes[1] = PublicDataClass._DataField.Buffer[dex + 1];
                                            bytes[2] = PublicDataClass._DataField.Buffer[dex + 2];
                                            bytes[3] = PublicDataClass._DataField.Buffer[dex + 3];

                                            float fdata = BitConverter.ToSingle(bytes, 0);
                                            if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text != String.Format("{0:f4}", fdata))
                                            {
                                                listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                                listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].ForeColor = Color.Red;
                                                count++;
                                            }
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text = String.Format("{0:f4}", fdata);
                                            dex += 4;
                                        }
                                    }

                                }
                                if (PublicDataClass.seqflag == 1)
                                {
                                    PublicDataClass.seq = 1;
                                    PublicDataClass.seqflag = 0;
                                    PublicDataClass.SQflag = 0;

                                    PublicDataClass.ParamInfoAddr += PublicDataClass._DataField.FieldVSQ;
                                    PublicDataClass._DataField.FieldLen = 0;
                                    PublicDataClass._DataField.FieldVSQ = 0;
                                    if (ty == 1)
                                        PublicDataClass._ComTaskFlag.READ_P_1 = true;

                                    if (ty == 2)
                                        PublicDataClass._NetTaskFlag.READ_P_1 = true;
                                    timer2.Enabled = true;
                                }
                            }
                            catch
                            {
                                if (PublicDataClass.seqflag == 1)
                                {
                                    PublicDataClass.seq = 1;
                                    PublicDataClass.seqflag = 0;
                                    PublicDataClass.SQflag = 0;

                                    PublicDataClass.ParamInfoAddr += PublicDataClass._DataField.FieldVSQ;
                                    PublicDataClass._DataField.FieldLen = 0;
                                    PublicDataClass._DataField.FieldVSQ = 0;
                                    if (ty == 1)
                                        PublicDataClass._ComTaskFlag.READ_P_1 = true;

                                    if (ty == 2)
                                        PublicDataClass._NetTaskFlag.READ_P_1 = true;
                                    timer2.Enabled = true;
                                }
                                MessageBox.Show("异常！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            try
                            {

                                for (int q = 0; q < PublicDataClass._DataField.FieldVSQ; q++)
                                {
                                    for (int col = 0; col < PublicDataClass.TabCfg[ItemId - 0x14].ColumnNum; col++)
                                    {

                                        if (PublicDataClass.TabCfg[ItemId - 0x14].ColumnDownByte[col] == "1")
                                        {
                                            byte value = 0;
                                            string str = "";
                                            value = PublicDataClass._DataField.Buffer[dex];
                                            if (PublicDataClass.addselect == 3)//协处理器
                                            {  //公钥SID16进制显示
                                                if (PublicDataClass.ParamInfoAddr == 301 || PublicDataClass.ParamInfoAddr == 302 || PublicDataClass.ParamInfoAddr == 303 || PublicDataClass.ParamInfoAddr == 304 || PublicDataClass.ParamInfoAddr == 305)
                                                {

                                                    if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text != String.Format("{0:d}", value))
                                                    {
                                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].ForeColor = Color.Red;
                                                        count++;
                                                    }
                                                    listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text = String.Format("{0:d}", value);
                                                }

                                                else if (PublicDataClass.ParamInfoAddr == 1)//版本号
                                                {
                                                    for (byte i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                                        str += Convert.ToChar(PublicDataClass._DataField.Buffer[i]);
                                                    if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text != str)
                                                    {
                                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].ForeColor = Color.Red;
                                                        count++;
                                                    }
                                                    listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text = str;

                                                }
                                            }
                                            else
                                            {
                                                if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text != Convert.ToString(value))
                                                {
                                                    listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                                    listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].ForeColor = Color.Red;
                                                    count++;
                                                }
                                                listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text = Convert.ToString(value);
                                            }
                                            dex += 1;
                                        }
                                        else if (PublicDataClass.TabCfg[ItemId - 0x14].ColumnDownByte[col] == "2")
                                        {
                                            int value = 0;
                                            value += PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);
                                            if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text != Convert.ToString(value))
                                            {
                                                listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                                listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].ForeColor = Color.Red;
                                                count++;
                                            }
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text = Convert.ToString(value);
                                            dex += 2;
                                        }
                                        else if (PublicDataClass.TabCfg[ItemId - 0x14].ColumnDownByte[col] == "4")
                                        {
                                            byte[] bytes = new byte[4];
                                            bytes[0] = PublicDataClass._DataField.Buffer[dex];
                                            bytes[1] = PublicDataClass._DataField.Buffer[dex + 1];
                                            bytes[2] = PublicDataClass._DataField.Buffer[dex + 2];
                                            bytes[3] = PublicDataClass._DataField.Buffer[dex + 3];
                                            if (PublicDataClass.addselect == 3)//协处理器
                                            {
                                                long fdata = BitConverter.ToInt32(bytes, 0);
                                                if (PublicDataClass.ParamInfoAddr == 200)
                                                {
                                                    if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text != String.Format("{0:d}", fdata))
                                                    {
                                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].ForeColor = Color.Red;
                                                        count++;
                                                    }
                                                    listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text = String.Format("{0:d}", fdata);
                                                }
                                            }
                                            else
                                            {

                                                float fdata = BitConverter.ToSingle(bytes, 0);
                                                if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text != String.Format("{0:f4}", fdata))
                                                {
                                                    listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                                    listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].ForeColor = Color.Red;
                                                    count++;
                                                }
                                                listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text = String.Format("{0:f4}", fdata);
                                            }
                                            dex += 4;
                                        }
                                        else if (PublicDataClass.TabCfg[ItemId - 0x14].ColumnDownByte[col] == "按字节数下载")
                                        {
                                            int m;
                                            string zijie;
                                            for (m = 0; m < PublicDataClass.TabCfg[ItemId - 0x14].ColumnNum; m++)
                                            {
                                                if (PublicDataClass.TabCfg[ItemId - 0x14].ColumnDownByte[m] == "字节数")
                                                    break;
                                            }
                                            zijie = PublicDataClass.TabCfg[ItemId - 0x14].TabPageValue[m].ValueTable[q];
                                            if (zijie == "1")
                                            {

                                                byte value = 0;
                                                value = PublicDataClass._DataField.Buffer[dex];



                                                if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text != Convert.ToString(value))
                                                {
                                                    listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                                    listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].ForeColor = Color.Red;
                                                    count++;
                                                }
                                                listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text = Convert.ToString(value);

                                                dex += 1;

                                            }
                                            else if (zijie == "2")
                                            {

                                                int value = 0;
                                                value += PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);
                                                if (Convert.ToString(value) != listViewtest.Items[q].SubItems[2].Text)
                                                {
                                                    listViewtest.Items[q].UseItemStyleForSubItems = false;
                                                    listViewtest.Items[q].SubItems[2].ForeColor = Color.Red;
                                                    count++;
                                                }
                                                listViewtest.Items[q].SubItems[2].Text = Convert.ToString(value);
                                                dex += 2;
                                            }
                                            else if (zijie == "4")
                                            {

                                                //string value = @"";
                                                //value += PublicDataClass._DataField.Buffer[dex] + "." + PublicDataClass._DataField.Buffer[dex + 1] + "." + PublicDataClass._DataField.Buffer[dex + 2] + "." + PublicDataClass._DataField.Buffer[dex + 3];
                                                //if (Convert.ToString(value) != listViewtest.Items[q].SubItems[2].Text)
                                                //{
                                                //    listViewtest.Items[q].UseItemStyleForSubItems = false;
                                                //    listViewtest.Items[q].SubItems[2].ForeColor = Color.Red;
                                                //}
                                                //listViewtest.Items[q].SubItems[2].Text = Convert.ToString(value);
                                                //dex += 4;
                                                byte[] bytes = new byte[4];
                                                bytes[0] = PublicDataClass._DataField.Buffer[dex];
                                                bytes[1] = PublicDataClass._DataField.Buffer[dex + 1];
                                                bytes[2] = PublicDataClass._DataField.Buffer[dex + 2];
                                                bytes[3] = PublicDataClass._DataField.Buffer[dex + 3];

                                                //  float fdata = BitConverter.ToSingle(bytes, 0);
                                                long fdata = BitConverter.ToInt32(bytes, 0);
                                                if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text != String.Format("{0:d}", fdata))
                                                {
                                                    listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                                    listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].ForeColor = Color.Red;
                                                    count++;
                                                }
                                                listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.TabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text = String.Format("{0:d}", fdata);
                                                dex += 4;
                                            }
                                            else if (zijie == "6")
                                            {

                                                string value = @"";
                                                value += PublicDataClass._DataField.Buffer[dex] + "-" + PublicDataClass._DataField.Buffer[dex + 1] + "-" + PublicDataClass._DataField.Buffer[dex + 2]
                                                          + "-" + PublicDataClass._DataField.Buffer[dex + 3] + "-" + PublicDataClass._DataField.Buffer[dex + 4] + "-" + PublicDataClass._DataField.Buffer[dex + 5];

                                                if (Convert.ToString(value) != listViewtest.Items[q].SubItems[2].Text)
                                                {
                                                    listViewtest.Items[q].UseItemStyleForSubItems = false;
                                                    listViewtest.Items[q].SubItems[2].ForeColor = Color.Red;
                                                    count++;
                                                }
                                                listViewtest.Items[q].SubItems[2].Text = Convert.ToString(value);
                                                dex += 6;
                                            }


                                        }
                                    }

                                }
                                if (PublicDataClass.seqflag == 1)
                                {
                                    PublicDataClass.seq = 1;
                                    PublicDataClass.seqflag = 0;
                                    PublicDataClass.SQflag = 0;

                                    PublicDataClass.ParamInfoAddr += PublicDataClass._DataField.FieldVSQ;
                                    PublicDataClass._DataField.FieldLen = 0;
                                    PublicDataClass._DataField.FieldVSQ = 0;
                                    if (ty == 1)
                                        PublicDataClass._ComTaskFlag.READ_P_1 = true;

                                    if (ty == 2)
                                        PublicDataClass._NetTaskFlag.READ_P_1 = true;
                                    timer2.Enabled = true;
                                }
                            }
                            catch
                            {
                                if (PublicDataClass.seqflag == 1)
                                {
                                    PublicDataClass.seq = 1;
                                    PublicDataClass.seqflag = 0;
                                    PublicDataClass.SQflag = 0;

                                    PublicDataClass.ParamInfoAddr += PublicDataClass._DataField.FieldVSQ;
                                    PublicDataClass._DataField.FieldLen = 0;
                                    PublicDataClass._DataField.FieldVSQ = 0;
                                    if (ty == 1)
                                        PublicDataClass._ComTaskFlag.READ_P_1 = true;

                                    if (ty == 2)
                                        PublicDataClass._NetTaskFlag.READ_P_1 = true;
                                    timer2.Enabled = true;
                                }
                                MessageBox.Show("异常！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }

                    }
                    if (count > 0)
                        label3.Text = "红色标注处参数变化！";

                      //  MessageBox.Show("红色标注处参数变化！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshParamState();
                }
            }
        }

        private void 快捷配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ItemId == 4)
            {
                PublicDataClass._FastParamRecord.ItemId = 4;
                AddParamRecordViewForm cfm = new AddParamRecordViewForm();
                cfm.ShowDialog();
                if (cfm.DialogResult == DialogResult.OK)               //判断是否按下-确定-按钮
                {
                    for (int j = 0; j < listView1.Items.Count; j++)
                    {
                        listView1.Items[j].SubItems[3].Text = "65535";
                        listView1.Items[j].UseItemStyleForSubItems = false;
                        listView1.Items[j].SubItems[3].ForeColor = Color.Gray;
                    }
                    for (int j = 0; j < PublicDataClass._FastParamRecord.num; j++)
                    {
                        listView1.Items[PublicDataClass._FastParamRecord.index[j] - 1].SubItems[3].Text = PublicDataClass._FastParamRecord.addr[j];
                        listView1.Items[PublicDataClass._FastParamRecord.index[j] - 1].SubItems[3].ForeColor = Color.Red;
                    
                    }
                    RefreshParamState();
                    //RefreshYcYxCfgParam();
                }
            }
            else if (ItemId == 5)
            {
                PublicDataClass._FastParamRecord.ItemId = 5;
                AddParamRecordViewForm cfm = new AddParamRecordViewForm();
                cfm.ShowDialog();
                if (cfm.DialogResult == DialogResult.OK)               //判断是否按下-确定-按钮
                {
                    for (int j = 0; j < listView1.Items.Count; j++)
                    {
                        listView1.Items[j].SubItems[3].Text = "65535";
                        listView1.Items[j].UseItemStyleForSubItems = false;
                        listView1.Items[j].SubItems[3].ForeColor = Color.Gray;
                    }
                    for (int j = 0; j < PublicDataClass._FastParamRecord.num; j++)
                    {
                        listView1.Items[PublicDataClass._FastParamRecord.index[j] - 1].SubItems[3].Text = PublicDataClass._FastParamRecord.addr[j];
                        listView1.Items[PublicDataClass._FastParamRecord.index[j] - 1].SubItems[3].ForeColor = Color.Red;
                        count++;
                    }
                    RefreshParamState();
                    //RefreshYcYxCfgParam();
                }
            }
            else if (PublicDataClass.FILENAME[ItemId - 0x14].IndexOf("报警灯关联参数") > 0 || PublicDataClass.FILENAME[ItemId - 0x14].IndexOf("CDT上传遥信合并点号参数") > 0)
            {
                PublicDataClass._FastParamRecord.ItemId = (byte)(ItemId - 0x14);
                AlarmLampYXCfg alarmcfg = new AlarmLampYXCfg();
                alarmcfg.ShowDialog();
                if (alarmcfg.DialogResult == DialogResult.OK)               //判断是否按下-确定-按钮
                {
                    for (int q = 0; q < listViewtest.Items.Count; q++)
                    {
                        for (int p = 1; p < 25; p++)
                        {
                            listViewtest.Items[q].SubItems[p].Text = "65535";
                            listViewtest.Items[q].UseItemStyleForSubItems = false;
                            listViewtest.Items[q].SubItems[p].ForeColor = Color.Gray;
                        }
                    }
                    for (int j = 0; j < PublicDataClass._FastParamRecord.num; j++)
                    {
                        listViewtest.Items[PublicDataClass._FastParamRecord.alarmY[j]].SubItems[PublicDataClass._FastParamRecord.alarmX[j]].Text = Convert.ToString(PublicDataClass._FastParamRecord.index[j] - 1);
                        listViewtest.Items[PublicDataClass._FastParamRecord.alarmY[j]].SubItems[PublicDataClass._FastParamRecord.alarmX[j]].ForeColor = Color.Red;
                        count++;
                    }
                    RefreshParamState();
                }
            }
            else if (PublicDataClass.FILENAME[ItemId - 0x14].IndexOf("电量配置") > 0)
            {
                PublicDataClass._FastParamRecord.ItemId = (byte)(ItemId - 0x14);
                AddParamRecordViewForm cfm = new AddParamRecordViewForm();
                cfm.ShowDialog();
                if (cfm.DialogResult == DialogResult.OK)               //判断是否按下-确定-按钮
                {
                    for (int j = 0; j < listViewtest.Items.Count; j++)
                    {
                        listViewtest.Items[j].SubItems[3].Text = "65535";
                        listViewtest.Items[j].UseItemStyleForSubItems = false;
                        listViewtest.Items[j].SubItems[3].ForeColor = Color.Gray;
                    }
                    for (int j = 0; j < PublicDataClass._FastParamRecord.num; j++)
                    {
                        listViewtest.Items[PublicDataClass._FastParamRecord.index[j] - 1].SubItems[3].Text = PublicDataClass._FastParamRecord.addr[j];
                        listViewtest.Items[PublicDataClass._FastParamRecord.index[j] - 1].SubItems[3].ForeColor = Color.Red;
                        count++;
                    }
                    RefreshParamState();
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                checkBox3.Checked = false;
                checkBox4.Checked = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                checkBox2.Checked = false;
                checkBox4.Checked = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                checkBox2.Checked = false;
                checkBox3.Checked = false;
            }
        }

        private void textBoxpassword_TextChanged(object sender, EventArgs e)
        {
            if (textBoxpassword.Text == "abcdef")
            {
                downloadbutton.Enabled = true;
            }
        }

        private void cDT点号配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ItemId == 4)
            {
                PublicDataClass._FastParamRecord.ItemId = 104;
                AddParamRecordViewForm cfm = new AddParamRecordViewForm();
                cfm.ShowDialog();
                if (cfm.DialogResult == DialogResult.OK)               //判断是否按下-确定-按钮
                {
                    //for (int j = 0; j < listView1.Items.Count; j++)
                    //{
                    //    listView1.Items[j].SubItems[3].Text = "65535";
                    //    listView1.Items[j].UseItemStyleForSubItems = false;
                    //    listView1.Items[j].SubItems[3].ForeColor = Color.Gray;
                    //}
                    //for (int j = 0; j < PublicDataClass._FastParamRecord.num; j++)
                    //{
                    //    listView1.Items[PublicDataClass._FastParamRecord.index[j] - 1].SubItems[3].Text = PublicDataClass._FastParamRecord.addr[j];
                    //    listView1.Items[PublicDataClass._FastParamRecord.index[j] - 1].SubItems[3].ForeColor = Color.Red;
                    //}
                    //RefreshParamState();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "System Files(*.ini)|*.ini|所有文件(*.*)|*.*";
            saveret = savefile.ShowDialog() == DialogResult.OK;
            if (saveret)
            {
                savename = savefile.FileName;
                FileStream afile;
                StreamWriter sw;
                afile = new FileStream(savename, FileMode.Create);
                if (ItemId <= 3)
                {
                    sw = new StreamWriter(afile);
                    sw.WriteLine("[NUM]");
                    sw.WriteLine("[NAMETABLE]");
                    sw.WriteLine("[VALUETABLE]");
                    sw.WriteLine("[BEILVTABLE]");
                    sw.Close();
                    afile.Close();
                }
                else if (ItemId == 4)
                {
                    sw = new StreamWriter(afile);
                    sw.WriteLine("[NUM]");
                    sw.WriteLine("[NAMETABLE]");
                    sw.WriteLine("[INFOTABLE]");
                    sw.WriteLine("INDEXTABLE");
                    sw.WriteLine("DATATYPLETABLE");
                    sw.WriteLine("BEISHUTABLE");
                    sw.WriteLine("CONNECTTABLE");
                    sw.WriteLine("QUFANTABLE");
                    sw.WriteLine("BYTETABLE");
                    sw.Close();
                    afile.Close();
                }
                else if (ItemId == 5)
                {
                    sw = new StreamWriter(afile);
                    sw.WriteLine("[NUM]");
                    sw.WriteLine("WYXNUM");
                    sw.WriteLine("[NAMETABLE]");
                    sw.WriteLine("[INFOTABLE]");
                    sw.WriteLine("INDEXTABLE");
                    sw.WriteLine("QUFANTABLE");
                    sw.WriteLine("SETVALUETABLE");
                    sw.WriteLine("VALUETABLE");
                    sw.WriteLine("BYTETABLE");
                    sw.Close();
                    afile.Close();
                }
                else if (ItemId == 6)
                {
                    sw = new StreamWriter(afile);
                    sw.WriteLine("[NUM]");
                    sw.WriteLine("[NAMETABLE]");
                    sw.WriteLine("[INFOTABLE]");
                    sw.WriteLine("[TRIGGERMODE]");
                    sw.WriteLine("[SECLTIME]");
                    sw.WriteLine("[EXETIME]");
                    sw.WriteLine("[PULSEWIDTH]");
                    sw.WriteLine("[SAVEFLAG]");
                    sw.WriteLine("[POWER]");
                    sw.WriteLine("[JDQ1]");
                    sw.WriteLine("[JDQ2]");
                    sw.WriteLine("[FJYX1]");
                    sw.WriteLine("[FJYX2]");
                    sw.WriteLine("[BYTETABLE]");
                    sw.Close();
                    afile.Close();
                }
                else if ((ItemId == 7) || (ItemId == 8) || (ItemId == 9))
                {
                    sw = new StreamWriter(afile);
                    sw.WriteLine("[NUM]");
                    sw.WriteLine("[QUALITYTABLE]");
                    sw.WriteLine("[PHASETABLE]");
                    sw.WriteLine("[LINETABLE]");
                    sw.WriteLine("[PANELTABLE]");
                    sw.WriteLine("[VALUETABLE]");
                    sw.WriteLine("[PHTABLE]");
                    sw.WriteLine("[ZHISHUTABLE]");
                    sw.Close();
                    afile.Close();
                }
                //else if (ItemId == 8)
                //{
                //    sw = new StreamWriter(afile);
                //    sw.WriteLine("[NUM]");
                //    sw.WriteLine("[VALUETABLE]");
                //    sw.WriteLine("[PHTABLE]");
                //}
                //else if (ItemId == 9)
                //{
                //    sw = new StreamWriter(afile);
                //    sw.WriteLine("[NUM]");
                //    sw.WriteLine("[ZHISHUTABLE]");
                //}



                button1.Text = "导出中..";
                savefile.Dispose();
            }
        }

        private void 参数修正ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JZremend cfm = new JZremend();
            cfm.ShowDialog();
            if (cfm.DialogResult == DialogResult.OK)               //判断是否按下-确定-按钮
            {
                for (int j = 0; j < listView1.Items.Count; j++)
                {
                    if (listView1.Items[j].SubItems[1].Text == PublicDataClass._AIParam.remendquality)
                    {
                        if ((PublicDataClass._AIParam.remendtype == 1) && (PublicDataClass._AIParam.updown == 1))
                        {
                            float temovalue = Convert.ToSingle(listView1.Items[j].SubItems[5].Text) + PublicDataClass._AIParam.remendvalue;
                            listView1.Items[j].SubItems[5].Text = temovalue.ToString();
                        }
                        else if ((PublicDataClass._AIParam.remendtype == 1) && (PublicDataClass._AIParam.updown == 2))
                        {
                            float temovalue = Convert.ToSingle(listView1.Items[j].SubItems[5].Text) - PublicDataClass._AIParam.remendvalue;
                            listView1.Items[j].SubItems[5].Text = temovalue.ToString();
                        }
                        else if ((PublicDataClass._AIParam.remendtype == 2) && (PublicDataClass._AIParam.updown == 1))
                        {
                            float temovalue = Convert.ToSingle(listView1.Items[j].SubItems[6].Text) + PublicDataClass._AIParam.remendvalue;
                            listView1.Items[j].SubItems[6].Text = temovalue.ToString();
                        }
                        else if ((PublicDataClass._AIParam.remendtype == 2) && (PublicDataClass._AIParam.updown == 2))
                        {
                            float temovalue = Convert.ToSingle(listView1.Items[j].SubItems[6].Text) - PublicDataClass._AIParam.remendvalue;
                            listView1.Items[j].SubItems[6].Text = temovalue.ToString();
                        }

                    }
                }
                RefreshParamState();
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (saveret)
            {
                saveret = false;
                if (ItemId <= 6)
                {
                    WriteReadAllFile.WriteReadParamIniFile(savename, 1, (byte)(ItemId - 1));
                    //  Form1.WriteIniFilek = (byte)(ItemId - 1);
                }
                else if (ItemId <= 9)
                {
                    WriteReadAllFile.WriteReadParamIniFile(savename, 1, 10);
                    //  Form1.WriteIniFilek = 10;
                }

                //Form1.WriteIniFileType = 1;
                //Form1.WriteIniFileName = savename;
                //Form1.WriteIniflag = true;
                for (int i = 0; i < 10000; i++)
                { }
                button1.Text = "导出";
                MessageBox.Show("数据导出成功", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (!IniCfgDlgFirstShowFlag)//只初始化一次
            {

                try
                {
                    if (PublicDataClass.DynOptHaveFlag == 1)
                    {
                        tp = new TabPage[PublicDataClass.DynOptHaveNum];
                        IniCfgDlgFirstShowFlag = true;
                    }
                }
                catch//异常说明工程初始化未完成
                { }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string FileName = "";
            string path = PublicDataClass.PrjPath + "\\ini";

            switch (ItemId)
            {
                case 1:
                    FileName = path + "\\comparam.ini";
                    break;
                case 2:
                    FileName = path + "\\netparam.ini";
                    break;
                case 3:
                    FileName = path + "\\sysparam.ini";
                    break;
                case 4:
                    FileName = path + "\\YCconfig.ini";
                    break;
                case 5:
                    FileName = path + "\\YXconfig.ini";
                    break;
                case 6:
                    FileName = path + "\\YKconfig.ini";
                    break;

                case 7:
                    FileName = path + "\\模拟量接入配置.ini";
                    break;

                case 8:
                    FileName = path + "\\模拟量接入配置.ini";
                    break;
                case 9:
                    FileName = path + "\\模拟量接入配置.ini";
                    break;


            }
            if (ItemId < 7)
            {
                WriteReadAllFile.WriteReadParamIniFile(FileName, 0, (byte)(ItemId - 1));
                listView1.Items.Clear();
                CheckNowParamState();
            }
            else if (ItemId >= 7 && ItemId < 0x14)
            {
                WriteReadAllFile.WriteReadParamIniFile(FileName, 0, 10);
                //if (ItemId < 7)
                //    Form1.WriteIniFilek = (byte)(ItemId - 1);
                //else
                //    Form1.WriteIniFilek = 10;
                //Form1.WriteIniFileName = FileName;
                //Form1.WriteIniFileType = 0;
                //Form1.WriteIniflag = true;

                listView1.Items.Clear();
                CheckNowParamState();
            }
            if (ItemId - 0x14 >= 0)
            {
                WriteReadAllFile.ReadDynOptFile(PublicDataClass.FILENAME[ItemId - 0x14], ItemId - 0x14, 1);
                listViewtest.Items.Clear();
                CheckNowDynOptParamState(ItemId - 0x14);//更新动态选项卡参数
            }

        }

        private void 遥信刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listViewtest.Items.Clear();
            listViewtest.Columns.Clear();
            for (int k = 0; k < PublicDataClass.FILENAME.Length; k++)
            {

                if (PublicDataClass.TabCfg[k].PageName == "遥信置数")
                {

                    //if (PublicDataClass._ChangeFlag.YxzsCfg == true)
                    //{


                    listViewtest.Columns.Add("序号");
                    listViewtest.Columns.Add("名称");
                    listViewtest.Columns[1].Width = 200;
                    listViewtest.Columns.Add("数值");
                    listViewtest.Columns.Add("索引号");


                    for (int j = 0; j < YxCfgNum; j++)
                    {

                        ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));

                        lv.SubItems.Add(PublicDataClass._YxConfigParam.NameTable[YxCfgIndex[j] - 1]);
                        if (YxCfgIndex[j] <= PublicDataClass._YxConfigParam.wyxnum)
                        {
                            if (PublicDataClass.SaveText.Cfg[0].Yxdata[j] == "合")

                                PublicDataClass.TabCfg[k].TabPageValue[2].ValueTable[YxCfgIndex[j] - 1] = "1";
                            else if (PublicDataClass.SaveText.Cfg[0].Yxdata[j] == "分")

                                PublicDataClass.TabCfg[k].TabPageValue[2].ValueTable[YxCfgIndex[j] - 1] = "0";
                            lv.SubItems.Add(PublicDataClass.TabCfg[k].TabPageValue[2].ValueTable[YxCfgIndex[j] - 1]);


                        }
                        else
                            lv.SubItems.Add("0");
                        lv.SubItems.Add(Convert.ToString(YxCfgIndex[j]));
                        listViewtest.Items.Add(lv);

                    }
                    return;
                    //}

                }


            }

        }

        private void 遥信取反ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listViewtest.Items.Clear();
            listViewtest.Columns.Clear();

            for (int k = 0; k < PublicDataClass.FILENAME.Length; k++)
            {

                if (PublicDataClass.TabCfg[k].PageName == "遥信置数")
                {

                    //if (PublicDataClass._ChangeFlag.YxzsCfg == true)
                    {


                        listViewtest.Columns.Add("序号");
                        listViewtest.Columns.Add("名称");
                        listViewtest.Columns[1].Width = 200;
                        listViewtest.Columns.Add("数值");
                        listViewtest.Columns.Add("索引号");


                        for (int j = 0; j < YxCfgNum; j++)
                        {

                            ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));

                            lv.SubItems.Add(PublicDataClass._YxConfigParam.NameTable[YxCfgIndex[j] - 1]);
                            if (YxCfgIndex[j] <= PublicDataClass._YxConfigParam.wyxnum)
                            {
                                if (PublicDataClass.TabCfg[k].TabPageValue[2].ValueTable[YxCfgIndex[j] - 1] == "0")

                                    PublicDataClass.TabCfg[k].TabPageValue[2].ValueTable[YxCfgIndex[j] - 1] = "1";
                                else if (PublicDataClass.TabCfg[k].TabPageValue[2].ValueTable[YxCfgIndex[j] - 1] == "1")

                                    PublicDataClass.TabCfg[k].TabPageValue[2].ValueTable[YxCfgIndex[j] - 1] = "0";
                                lv.SubItems.Add(PublicDataClass.TabCfg[k].TabPageValue[2].ValueTable[YxCfgIndex[j] - 1]);


                            }
                            else
                                lv.SubItems.Add("0");
                            lv.SubItems.Add(Convert.ToString(YxCfgIndex[j]));
                            listViewtest.Items.Add(lv);

                        }
                        return;
                    }

                }


            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            int k = 0;
            xmldoc = new XmlDocument();
            //加入XML的声明段落,<?xml version="1.0" encoding="gb2312"?>
            XmlDeclaration xmldecl;
            xmldecl = xmldoc.CreateXmlDeclaration("1.0", "gb2312", null);
            xmldoc.AppendChild(xmldecl);

            //加入一个根元素
            xmlelem = xmldoc.CreateElement("", "File", "");
            xmldoc.AppendChild(xmlelem);
            //加入另外一个元素

            XmlNode root = xmldoc.SelectSingleNode("File");//查找<Employees> 
            XmlElement xe1 = xmldoc.CreateElement("Node");//创建一个<Node>节点 
            xe1.SetAttribute("genre", "李赞红");//设置该节点genre属性 
            xe1.SetAttribute("ISBN", "2-3631-4");//设置该节点ISBN属性 

            XmlElement xesub1 = xmldoc.CreateElement("title");
            xesub1.InnerText = PublicDataClass.FILENAME[ItemId - 0x14];//设置文本节点 
            xe1.AppendChild(xesub1);//添加到<Node>节点中 
            XmlElement xesub2 = xmldoc.CreateElement("LineNum");
            xesub2.InnerText = Convert.ToString(PublicDataClass.TabCfg[k].LineNum);
            xe1.AppendChild(xesub2);
            XmlElement xesub3 = xmldoc.CreateElement("ColumnNum");
            xesub3.InnerText = Convert.ToString(PublicDataClass.TabCfg[k].ColumnNum);
            xe1.AppendChild(xesub3);
            XmlElement xesub4 = xmldoc.CreateElement("ParamInfoAddr");
            xesub4.InnerText = "1792";
            xe1.AppendChild(xesub4);
            XmlElement xesub5 = xmldoc.CreateElement("COLLUMNPAGENAME");
            xe1.AppendChild(xesub5);
            XmlElement xesub6 = xmldoc.CreateElement("ColumnValueTable");
            xe1.AppendChild(xesub6);
            //每列值名称
            XmlElement[] colname = new XmlElement[PublicDataClass.TabCfg[k].ColumnNum];
            XmlElement[] value = new XmlElement[PublicDataClass.TabCfg[k].LineNum];
            for (int j = 0; j < PublicDataClass.TabCfg[k].ColumnNum; j++)
            {
                colname[j] = xmldoc.CreateElement(String.Format("ColumnPageName_{0:d}", j));
                colname[j].InnerText = PublicDataClass.TabCfg[k].ColumnPageName[j];
                xesub5.AppendChild(colname[j]);
            }

            for (int j = 0; j < PublicDataClass.TabCfg[k].ColumnNum; j++)
            {
                colname[j] = xmldoc.CreateElement(String.Format("ColumnValueTable_{0:d}", j));

                for (int q = 0; q < PublicDataClass.TabCfg[k].LineNum; q++)
                {
                    value[j] = xmldoc.CreateElement(String.Format("ValueTable_{0:d}", q));
                    value[j].InnerText = PublicDataClass.TabCfg[k].TabPageValue[j].ValueTable[q];
                    colname[j].AppendChild(value[j]);
                }
                xesub6.AppendChild(colname[j]);
            }

            root.AppendChild(xe1);//添加到<Employees>节点中 

            string path = PublicDataClass.PrjPath + "\\ini";
            //保存创建好的XML文档
            xmldoc.Save(path + "\\data.xml");


        }

        private void button3_Click(object sender, EventArgs e)
        {


            xmldoc = new XmlDocument();
            string filepath = PublicDataClass.PrjPath + "\\ini\\data.xml";
            xmldoc.Load(filepath);

            XmlNodeList nodeList = xmldoc.SelectSingleNode("File").ChildNodes;//获取Employees节点的所有子节点 


            foreach (XmlNode xn in nodeList)//遍历所有子节点 
            {
                int k = 0;
                XmlElement xe = (XmlElement)xn;//将子节点类型转换为XmlElement类型 
                //   if (xe.GetAttribute("genre") == "张三")//如果genre属性值为“张三” 
                {
                    //     xe.SetAttribute("genre", "update张三");//则修改该属性为“update张三” 

                    XmlNodeList nls = xe.ChildNodes;//继续获取xe子节点的所有子节点 
                    foreach (XmlNode xn1 in nls)//遍历 
                    {
                        XmlElement xe1 = (XmlElement)xn1;//转换类型 
                        if (xe1.Name == "title")//如果找到 
                        {
                            PublicDataClass.FILENAME[ItemId - 0x14] = xe1.InnerText;
                        }
                        if (xe1.Name == "LineNum")//如果找到 
                        {
                            PublicDataClass.TabCfg[k].LineNum = Convert.ToInt16(xe1.InnerText);
                        }
                        if (xe1.Name == "ColumnNum")//如果找到 
                        {
                            PublicDataClass.TabCfg[k].ColumnNum = Convert.ToInt16(xe1.InnerText);
                        }
                        if (xe1.Name == "ParamInfoAddr")//如果找到 
                        {
                            //     PublicDataClass.FILENAME[ItemId - 0x14] = xe1.InnerText;
                        }
                        if (xe1.Name == "COLLUMNPAGENAME")//如果找到 
                        {
                            XmlNodeList nls1 = xe1.ChildNodes;//继续获取xe子节点的所有子节点 
                            foreach (XmlNode xn2 in nls1)//遍历 
                            {
                                XmlElement xe2 = (XmlElement)xn2;//转换类型 

                                for (int j = 0; j < PublicDataClass.TabCfg[k].ColumnNum; j++)
                                {
                                    if (xe2.Name == String.Format("ColumnPageName_{0:d}", j))//如果找到 
                                    {
                                        PublicDataClass.TabCfg[k].ColumnPageName[j] = xe2.InnerText;
                                    }
                                }
                            }
                        }
                        if (xe1.Name == "ColumnValueTable")//如果找到 
                        {
                            XmlNodeList nls1 = xe1.ChildNodes;//继续获取xe子节点的所有子节点 
                            foreach (XmlNode xn2 in nls1)//遍历 
                            {
                                XmlElement xe2 = (XmlElement)xn2;//转换类型 

                                for (int j = 0; j < PublicDataClass.TabCfg[k].ColumnNum; j++)
                                {
                                    if (xe2.Name == String.Format("ColumnValueTable_{0:d}", j))//如果找到 
                                    {
                                        XmlNodeList nls2 = xe2.ChildNodes;//继续获取xe子节点的所有子节点 
                                        foreach (XmlNode xn3 in nls2)//遍历 
                                        {
                                            XmlElement xe3 = (XmlElement)xn3;//转换类型 

                                            for (int q = 0; q < PublicDataClass.TabCfg[k].LineNum; q++)
                                            {
                                                if (xe3.Name == String.Format("ValueTable_{0:d}", q))//如果找到 
                                                {
                                                    PublicDataClass.TabCfg[k].TabPageValue[j].ValueTable[q] = xe3.InnerText;

                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }

                    }
                }
            }



        }
        /// <summary>
        /// 关联到遥信配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            if (PublicDataClass.TabCfg[zbpz].TabPageValue.Length < 7)
            {
                MessageBox.Show("综保配置文件错误！", "信息",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //显示遥信配置选项卡
            tabControl1.SelectedIndex = 4;

            listView1.Items.Clear();
            listView1.Columns.Clear();
            listView1.Columns.Add("序号", 60);
            listView1.Columns.Add("名称", 200);
            listView1.Columns.Add("索引号", 60);
            listView1.Columns.Add("信息体地址", 100);
            listView1.Columns.Add("取反标志", 60);
            listView1.Columns.Add("字节", 60);
            listView1.Controls.Add(textBoxvalue);
            textBoxvalue.Visible = false;

            comboBoxvalue3.Items.Clear();
            comboBoxvalue3.Items.Add("是");
            comboBoxvalue3.Items.Add("否");
            listView1.Controls.Add(comboBoxvalue3);
            comboBoxvalue3.Visible = false;
            try
            {
                for (int j = 0; j < PublicDataClass._YxConfigParam.wyxnum; j++)
                {
                    ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));
                    lv.SubItems.Add(PublicDataClass._YxConfigParam.NameTable[j]);
                    lv.SubItems.Add(PublicDataClass._YxConfigParam.IndexTable[j]);
                    lv.SubItems.Add(PublicDataClass._YxConfigParam.AddrTable[j]);
                    lv.SubItems.Add(PublicDataClass._YxConfigParam.QufanTable[j]);
                    lv.SubItems.Add(PublicDataClass._YxConfigParam.ByteTable[j]);
                    listView1.Items.Add(lv);
                }
                int linenum = PublicDataClass._YxConfigParam.wyxnum;
                for (int q = 0; q < PublicDataClass.TabCfg[zbpz].LineNum; q++)
                {
                    if (Convert.ToInt32(PublicDataClass.TabCfg[zbpz].TabPageValue[2].ValueTable[q]) > 0)
                    {
                        for (int j = 0; j < PublicDataClass._YxConfigParam.ZBYxNum; j++)
                        {
                            ListViewItem lv = new ListViewItem(String.Format("{0:d}", j + linenum));
                            int t = 0;
                            if (Convert.ToInt32(PublicDataClass.TabCfg[zbpz].TabPageValue[2].ValueTable[q]) % 4 > 0)
                                t = Convert.ToInt32(PublicDataClass.TabCfg[zbpz].TabPageValue[2].ValueTable[q]) % 4 - 1;
                            else
                                t = 3;
                            lv.SubItems.Add("(" + PublicDataClass.TabCfg[zbpz].TabPageValue[6].ValueTable[q] + ") " + PublicDataClass.TabCfg[zbyx].TabPageValue[1].ValueTable[t * 64 + j]);
                            lv.SubItems.Add(PublicDataClass._YxConfigParam.IndexTable[j + linenum]);
                            lv.SubItems.Add(PublicDataClass._YxConfigParam.AddrTable[j + linenum]);
                            lv.SubItems.Add(PublicDataClass._YxConfigParam.QufanTable[j + linenum]);
                            lv.SubItems.Add(PublicDataClass._YxConfigParam.ByteTable[j + linenum]);
                            listView1.Items.Add(lv);

                        }
                        linenum += PublicDataClass._YxConfigParam.ZBYxNum;
                    }
                }
                for (int j = linenum; j < PublicDataClass._YxConfigParam.num; j++)
                {
                    ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));
                    if (j < PublicDataClass._YxConfigParam.wyxnum + PublicDataClass.TabCfg[zbpz].LineNum * PublicDataClass._YxConfigParam.ZBYxNum)
                    {
                        lv.SubItems.Add("综保[YX" + (j + 1).ToString() + "]");
                    }
                    else
                        lv.SubItems.Add(PublicDataClass._YxConfigParam.NameTable[j]);
                    lv.SubItems.Add(PublicDataClass._YxConfigParam.IndexTable[j]);
                    lv.SubItems.Add(PublicDataClass._YxConfigParam.AddrTable[j]);
                    lv.SubItems.Add(PublicDataClass._YxConfigParam.QufanTable[j]);
                    lv.SubItems.Add(PublicDataClass._YxConfigParam.ByteTable[j]);
                    listView1.Items.Add(lv);
                }
                RefreshParamState();
            }
            catch
            { }
        }
        /// <summary>
        /// 关联到遥测配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            if (PublicDataClass.TabCfg[zbpz].TabPageValue.Length < 7)
            {
                MessageBox.Show("综保配置文件错误！", "信息",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string name = @"";
            tabControl1.SelectedIndex = 3;

            listView1.Items.Clear();
            listView1.Columns.Clear();
            listView1.Columns.Add("序号", 60);
            listView1.Columns.Add("名称", 200);
            listView1.Columns.Add("索引号", 60);
            listView1.Columns.Add("信息体地址", 100);
            //
            try
            {
                for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                {
                    ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));
                    int  startnum= PublicDataClass._YcConfigParam.ZBStartYcNum - 1;
                    int endnum=PublicDataClass.TabCfg[zbpz].LineNum * PublicDataClass._YcConfigParam.ZBYcNum + PublicDataClass._YcConfigParam.ZBStartYcNum;
                    if (j > startnum && j < endnum)
                    {

                        int index = PublicDataClass._YcConfigParam.NameTable[j].IndexOf(")");
                        if (index > 0)
                        {
                            string tname = @"";
                            if ((j - PublicDataClass._YcConfigParam.ZBStartYcNum) % PublicDataClass.TabCfg[zbpz].LineNum == 0)
                                tname = "(" + PublicDataClass.TabCfg[zbpz].TabPageValue[6].ValueTable[0];

                            else
                                tname = "(" + PublicDataClass.TabCfg[zbpz].TabPageValue[6].ValueTable[(j - PublicDataClass._YcConfigParam.ZBStartYcNum) % PublicDataClass.TabCfg[zbpz].LineNum];
                            name = tname + PublicDataClass._YcConfigParam.NameTable[j].Substring(index);
                            lv.SubItems.Add(name);
                        }
                        else lv.SubItems.Add(PublicDataClass._YcConfigParam.NameTable[j]);
                    }
                    else
                        lv.SubItems.Add(PublicDataClass._YcConfigParam.NameTable[j]);
                    lv.SubItems.Add(PublicDataClass._YcConfigParam.IndexTable[j]);
                    lv.SubItems.Add(PublicDataClass._YcConfigParam.AddrTable[j]);


                    listView1.Items.Add(lv);
                }
                RefreshParamState();
            }
            catch
            { }
        }
        /// <summary>
        /// 关联到遥控配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            if (PublicDataClass.TabCfg[zbpz].TabPageValue.Length < 7)
            {
                MessageBox.Show("综保配置文件错误！", "信息",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            tabControl1.SelectedIndex = 5;

            listView1.Items.Clear();
            listView1.Columns.Clear();
            listView1.Columns.Add("序号", 60);
            listView1.Columns.Add("名称", 200);
            listView1.Columns.Add("属性数值", 100);
            listView1.Columns.Add("触发方式", 100);
            listView1.Columns.Add("选择时间(ms)", 100);
            listView1.Columns.Add("执行时间(ms)", 100);
            listView1.Columns.Add("脉冲宽度(ms)", 100);
            listView1.Columns.Add("记录保存", 60);
            listView1.Columns.Add("电源属性", 100);
            listView1.Columns.Add("并联继电器点号（合）", 100);
            listView1.Columns.Add("并联继电器点号（分）", 100);
            listView1.Columns.Add("反校遥信点号（合）", 100);
            listView1.Columns.Add("反校遥信点号（分）", 100);
            listView1.Columns.Add("字节", 60);

            listView1.Controls.Add(textBoxvalue);
            textBoxvalue.Visible = false;

            comboBoxvalue.Items.Clear();
            comboBoxvalue.Items.Add("电平");
            comboBoxvalue.Items.Add("脉冲");
            listView1.Controls.Add(comboBoxvalue);
            comboBoxvalue.Visible = false;

            comboBoxvalue1.Items.Clear();
            comboBoxvalue1.Items.Add("是");
            comboBoxvalue1.Items.Add("否");
            listView1.Controls.Add(comboBoxvalue1);
            comboBoxvalue1.Visible = false;

            comboBoxvalue2.Items.Clear();
            comboBoxvalue2.Items.Add("无");
            comboBoxvalue2.Items.Add("24V");
            comboBoxvalue2.Items.Add("电操机构");
            comboBoxvalue2.Items.Add("电源+电操");
            listView1.Controls.Add(comboBoxvalue2);
            comboBoxvalue2.Visible = false;
            try
            {
                for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                {
                    ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));
                    if (j < 24)
                    {
                        int index = PublicDataClass._YkConfigParam.NameTable[j].IndexOf("(");
                        if (index > 0)
                        {
                            string tname = "(" + PublicDataClass.TabCfg[zbpz].TabPageValue[6].ValueTable[j] + ")";
                            lv.SubItems.Add(PublicDataClass._YkConfigParam.NameTable[j].Substring(0, index) + tname);
                        }
                        else
                            lv.SubItems.Add(PublicDataClass._YkConfigParam.NameTable[j]);
                    }
                    else
                        lv.SubItems.Add(PublicDataClass._YkConfigParam.NameTable[j]);
                    lv.SubItems.Add(PublicDataClass._YkConfigParam.AddrTable[j]);

                    lv.SubItems.Add(PublicDataClass._YkConfigParam.triggermodeTable[j]);
                    lv.SubItems.Add(PublicDataClass._YkConfigParam.secltimeTable[j]);
                    lv.SubItems.Add(PublicDataClass._YkConfigParam.exetimeTable[j]);
                    lv.SubItems.Add(PublicDataClass._YkConfigParam.pulsewidthTable[j]);
                    lv.SubItems.Add(PublicDataClass._YkConfigParam.saveflagTable[j]);
                    lv.SubItems.Add(PublicDataClass._YkConfigParam.powerTable[j]);
                    lv.SubItems.Add(PublicDataClass._YkConfigParam.jdq1Table[j]);
                    lv.SubItems.Add(PublicDataClass._YkConfigParam.jdq2Table[j]);
                    lv.SubItems.Add(PublicDataClass._YkConfigParam.fjyx1Table[j]);
                    lv.SubItems.Add(PublicDataClass._YkConfigParam.fjyx2Table[j]);

                    lv.SubItems.Add(PublicDataClass._YkConfigParam.ByteTable[j]);



                    lv.SubItems.Add(PublicDataClass._YkConfigParam.ByteTable[j]);

                    listView1.Items.Add(lv);
                }
                RefreshParamState();
            }
            catch
            { }
        }
        private byte[] ConVertToByte(int soure, byte pos)
        {
            byte[] data = new byte[pos];
            if (pos == 2)   //int型
            {
                data[0] = Convert.ToByte(soure & 0x00ff);
                data[1] = Convert.ToByte((soure & 0xff00) >> 8);

            }
            if (pos == 4)
            {
                data[3] = Convert.ToByte((soure & 0xff000000) >> 24);
                data[2] = Convert.ToByte((soure & 0x00ff0000) >> 16);
                data[1] = Convert.ToByte((soure & 0x0000ff00) >> 8);
                data[0] = Convert.ToByte((soure & 0x000000ff));
            }
            return data;

        }
    }
}
