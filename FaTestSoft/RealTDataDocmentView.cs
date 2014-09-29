using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KD.WinFormsUI.Docking;
using FaTestSoft.CommonData;
using FaTestSoft.FUNCTIONCLASS;

namespace FaTestSoft
{
    public partial class RealTDataDocmentView : DockContent
    {
        public RealTDataDocmentView()
        {
            InitializeComponent();
        }

        //DataMoniterViewForm DataMviewfm = new DataMoniterViewForm();
        private int num = 0;
        static byte dex = 0;
        byte po = 0;
        static int datepos = 0;
        int delaytime;//循环召测间隔
        private int ty;
        int SelectType = 2;//选择类型  1：遥测   2：遥信
        int ShowType = 1;//选择类型  0：按线路   1：按序号
       

        private void RealTDataDocmentView_Load(object sender, EventArgs e)
        {
            for (int ch = 5; ch < 100; ch++)
            {
                domainUpDownb.Items.Add(ch);

            }
            domainUpDownb.SelectedIndex = 0;
            int i = 0;
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

            treeView1.ExpandAll();


            
            //CommonDatainterFace.SaveText.Cfg[0].YxcfgNum
            this.listView1.Columns.Add("序号");
            this.listView1.Columns.Add("名称");
            this.listView1.Columns.Add("地址");
            this.listView1.Columns.Add("值");
            this.listView1.Columns.Add("单位");
            this.listView1.Columns.Add("倍率");
            this.listView1.Columns[0].Width = 60;
            this.listView1.Columns[1].Width = 250;
            this.listView1.Columns[2].Width = 100;
            this.listView1.Columns[3].Width = 100;
            this.listView1.Columns[4].Width = 100;
            this.listView1.Columns[5].Width = 60;

            if (PublicDataClass.SaveText.cfgnum != 0)
            {
                for (i = 0; i < PublicDataClass.SaveText.cfgnum; i++)
                {
                    if (comboBox1.Text == PublicDataClass.SaveText.Device[i].PointName)
                    {
                        break;
                    }
                }

                for (int j = 0; j < PublicDataClass.SaveText.Cfg[i].YxcfgNum; j++)
                {

                    ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));
                    lv.SubItems.Add(PublicDataClass.SaveText.Cfg[i].YxcfgName[j]);
                    lv.SubItems.Add(PublicDataClass.SaveText.Cfg[i].YxcfgAddr[j]);
                    if (PublicDataClass.SaveText.Cfg[i].Yxdata[j] == null)
                        lv.SubItems.Add("null");
                    else
                        lv.SubItems.Add(PublicDataClass.SaveText.Cfg[i].Yxdata[j]);
                    lv.SubItems.Add("null");
                    lv.SubItems.Add("1");
                    listView1.Items.Add(lv);
                    lv.UseItemStyleForSubItems = false;
                    listView1.Items[j].SubItems[3].ForeColor = Color.Blue;
                }
            }
            dex = 1;
        }

       
        private void Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (treeView1.SelectedNode.Text == "遥 信")
                {
                    SelectType = 2;
                    dex = 1;
                    this.listView1.Items.Clear();
                    this.listView1.Columns.Clear();

                    this.listView1.Columns.Add("序号", 60);
                    this.listView1.Columns.Add("名称", 250);
                    this.listView1.Columns.Add("地址", 100);
                    this.listView1.Columns.Add("值", 100);
                    this.listView1.Columns.Add("单位", 100);
                    this.listView1.Columns.Add("倍率", 60);

                    if (PublicDataClass.SaveText.cfgnum != 0)
                    {
                       
                        po = FindCfgInfo();
                        for (int j = 0; j < PublicDataClass.SaveText.Cfg[po].YxcfgNum; j++)
                        {

                            ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));
                            lv.SubItems.Add(PublicDataClass.SaveText.Cfg[po].YxcfgName[j]);
                            lv.SubItems.Add(PublicDataClass.SaveText.Cfg[po].YxcfgAddr[j]);
                            if (PublicDataClass.SaveText.Cfg[po].Yxdata[j] == null)
                                lv.SubItems.Add("null");
                            else
                            {
                                lv.SubItems.Add(PublicDataClass.SaveText.Cfg[po].Yxdata[j]);
                                //if (PublicDataClass.SaveText.Cfg[po].Yxdataqf[j] == "0")

                                //    lv.SubItems.Add(PublicDataClass.SaveText.Cfg[po].Yxdata[j]);
                                //else
                                //{
                                //    if (PublicDataClass.SaveText.Cfg[po].Yxdata[j] == "合")
                                //        lv.SubItems.Add("分");
                                //    else
                                //        lv.SubItems.Add("合");

                                //}
                            }
                            lv.SubItems.Add("null");
                            lv.SubItems.Add("1");
                            listView1.Items.Add(lv);
                            lv.UseItemStyleForSubItems = false;
                            listView1.Items[j].SubItems[3].ForeColor = Color.Blue;
                        }
                    }

                }
                else if (treeView1.SelectedNode.Text == "遥 测")
                {
                  
                    dex = 2;
                    SelectType = 1;
                    this.listView1.Items.Clear();
                    this.listView1.Columns.Clear();
                    this.listView1.Columns.Add("序号",60);
                    this.listView1.Columns.Add("名称",250);
                    this.listView1.Columns.Add("地址",100);
                    this.listView1.Columns.Add("值",100);
                    this.listView1.Columns.Add("单位",100);
                    this.listView1.Columns.Add("倍率",60);
            
                    if (PublicDataClass.SaveText.cfgnum != 0)
                    {
                         po = FindCfgInfo();
                         if (ShowType == 1)//选择类型  0：按线路   1：按序号
                         {
                             for (int j = 0; j < PublicDataClass.SaveText.Cfg[po].YccfgNum; j++)
                             {

                                 ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));
                                 lv.SubItems.Add(PublicDataClass.SaveText.Cfg[po].YccfgName[j]);
                                 lv.SubItems.Add(PublicDataClass.SaveText.Cfg[po].YccfgAddr[j]);
                                 if (PublicDataClass.SaveText.Cfg[po].Ycdata[j] == null)
                                     lv.SubItems.Add("null");
                                 else
                                     lv.SubItems.Add(PublicDataClass.SaveText.Cfg[po].Ycdata[j]);
                                 lv.SubItems.Add("null");
                                 lv.SubItems.Add("1");

                                 listView1.Items.Add(lv);
                                 lv.UseItemStyleForSubItems = false;
                                 listView1.Items[j].SubItems[3].ForeColor = Color.Blue;
                             }
                         }
                        //////////////////////////////////////////////按显示序号显示//////////////////////////////////////////////////////////////////////
                         if (ShowType == 0)//选择类型  0：按线路   1：按序号
                         {
                             for (int i = 0; i < PublicDataClass.SaveText.Cfg[po].YccfgNum; i++)//循环遍历数据表中的每一行数据
                             {
                                 for (int j = 0; j < PublicDataClass.SaveText.Cfg[po].YccfgNum; j++)
                                 {
                                 //    if (Convert.ToInt16(PublicDataClass._YcInformationParam.IndexTable[j]) == i)
                                     if (Convert.ToInt16(PublicDataClass.SaveText.Cfg[po].YccfgIndex[j]) == i)
                                         
                                     {
                                         ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));
                                         lv.SubItems.Add(PublicDataClass.SaveText.Cfg[po].YccfgName[j]);
                                         lv.SubItems.Add(PublicDataClass.SaveText.Cfg[po].YccfgAddr[j]);
                                         if (PublicDataClass.SaveText.Cfg[po].Ycdata[j] == null)
                                             lv.SubItems.Add("0");
                                         else
                                             lv.SubItems.Add(PublicDataClass.SaveText.Cfg[po].Ycdata[j]);
                                         lv.SubItems.Add("null");
                                         lv.SubItems.Add("1");

                                         listView1.Items.Add(lv);
                                         lv.UseItemStyleForSubItems = false;
                                         listView1.Items[j].SubItems[3].ForeColor = Color.Blue;
                                     }
                                 }
                             }
                         }
                        /////////////////////////////////////////////按显示序号显示//////////////////////////////////////////////////////////////////////
                    }
                }
                
                else if (treeView1.SelectedNode.Text == "事 件")
                {
                    this.listView1.Items.Clear();
                    this.listView1.Columns.Clear();

                    this.listView1.Columns.Add("序号");
                    this.listView1.Columns.Add("名称");
                    this.listView1.Columns.Add("值");
                    this.listView1.Columns.Add("发生的时间");
                    this.listView1.Columns.Add("毫秒");
                    this.listView1.Columns[0].Width = 60;
                    this.listView1.Columns[1].Width = 250;
                    this.listView1.Columns[2].Width = 100;
                    this.listView1.Columns[3].Width = 250;
                    this.listView1.Columns[4].Width = 100;


                }
                else if (treeView1.SelectedNode.Text == "其 它")
                {
                    this.listView1.Items.Clear();
                    this.listView1.Columns.Clear();

                    this.listView1.Columns.Add("序号");
                    this.listView1.Columns.Add("名称");
                    this.listView1.Columns.Add("值");
                    this.listView1.Columns[0].Width = 60;
                    this.listView1.Columns[1].Width = 500;
                    this.listView1.Columns[2].Width = 500;
                }

            }
            catch
            {
                

            }
        }

        private void RealTDataDocmentView_Activated(object sender, EventArgs e)
        {
            timer3.Enabled = true;
            PublicDataClass._WindowsVisable.RealTimeDataVisable = true;    //可见
            timer1.Enabled = true;
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

        private void RealTDataDocmentView_Deactivate(object sender, EventArgs e)
        {
            PublicDataClass._WindowsVisable.RealTimeDataVisable = false;  //窗体不可见

            timer1.Enabled = false;
            timer3.Enabled = false;
         
        }
        /// <summary>
        /// 定时器的处理函数---负责显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            if (PublicDataClass._Message.RealTimeDataDocmentView == true)
            {
                PublicDataClass._Message.RealTimeDataDocmentView = false;
                if (PublicDataClass.ChanelId == 2)
                {
                    if (PublicDataClass.RevNetFrameMsg == "遥测")
                        GetYcValueInfo();
                    else if (PublicDataClass.RevNetFrameMsg == "遥信")
                        GetYxDataInfo();
                    else if (PublicDataClass.RevNetFrameMsg == "变位事件")
                        GetBWYxDataInfo();
                    else if (PublicDataClass.RevNetFrameMsg == "扰动事件")
                        GetRDYcValueInfo();
                }
                else if (PublicDataClass.ChanelId == 1)                   //zxl
                {
                    if ((PublicDataClass.ComFrameMsg == "遥测") || (PublicDataClass.ComFrameMsg == "浮点遥测"))
                        GetYcValueInfo();
                    else if (PublicDataClass.ComFrameMsg == "遥信")
                        GetYxDataInfo();
                    else if (PublicDataClass.ComFrameMsg == "变位事件")
                        GetBWYxDataInfo();
                    else if (PublicDataClass.ComFrameMsg == "扰动事件")
                        GetRDYcValueInfo();
                }
                RefreshValue();
            }
        }
        #region user-defined
        /// <summary>
        /// 更新值
        /// </summary>
        private void RefreshValue()
        {

            po = FindCfgInfo();
            if (dex == 1)  //遥信页面
            {

                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (PublicDataClass.SaveText.Cfg[po].Yxdata[i] == null)
                        listView1.Items[i].SubItems[3].Text = "null";
                    else
                    {
                        //if (PublicDataClass.SaveText.Cfg[po].Yxdataqf[i] == "0")
                            listView1.Items[i].SubItems[3].Text = PublicDataClass.SaveText.Cfg[po].Yxdata[i];
                        //else
                        //{
                        //    if (PublicDataClass.SaveText.Cfg[po].Yxdata[i] == "合")
                        //        listView1.Items[i].SubItems[3].Text = "分";
                        //    else
                        //        listView1.Items[i].SubItems[3].Text = "合";
                        //}
                    }
                }
                
            }
            else if (dex == 2)
            {
                if (ShowType == 1)//选择类型  0：按线路   1：按序号
                {
                    for (int i = 0; i < listView1.Items.Count; i++)
                        listView1.Items[i].SubItems[3].Text = PublicDataClass.SaveText.Cfg[po].Ycdata[i];
                }
               
                if (ShowType == 0)//选择类型  0：按线路   1：按序号
                {
                    for (int i = 0; i < PublicDataClass.SaveText.Cfg[po].YccfgNum; i++)//循环遍历数据表中的每一行数据
                    {
                        for (int j = 0; j < PublicDataClass.SaveText.Cfg[po].YccfgNum; j++)
                        {
                           // if (Convert.ToInt16(PublicDataClass._YcInformationParam.IndexTable[j]) == i)
                            if (Convert.ToInt16(PublicDataClass.SaveText.Cfg[po].YccfgIndex[j]) == i)
                            {
                                listView1.Items[i].SubItems[1].Text = PublicDataClass.SaveText.Cfg[po].YccfgName[j];
                                listView1.Items[i].SubItems[3].Text = PublicDataClass.SaveText.Cfg[po].Ycdata[j];
                            }
                        }
                    }
                }

            }// end  of  else if (dex == 2)
            
        }
        
        /// <summary>
        /// 查找测量点
        /// </summary>
        /// <returns></returns>
        private byte FindCfgInfo()
        {
            byte i = 0;
            for (i = 0; i < PublicDataClass.SaveText.cfgnum; i++)
            {
                if (comboBox1.Text == PublicDataClass.SaveText.Device[i].PointName)
                {
                    break;
                }
            }
            return i;
        }

        /// <summary>
        /// 获取遥测数值信息
        /// </summary>
        private void GetYcValueInfo()
        {
            if (PublicDataClass._ChangeFlag.Clearflag1 == true)
            {
                datepos = 0;
                PublicDataClass._ChangeFlag.Clearflag1 = false;
            }
            try
            {
                int StartPos = 0;
                int data = 0;
                float fdata = 0.0F;
                byte[] bytes = new byte[4];   //zxl
                int k = 0;
                byte dx = PublicFunction.FindPointNameCorrelativeIndex(comboBox1.Text);

                if (datepos >= PublicDataClass.SaveText.Cfg[dx].YccfgNum)
                {

                    return;

                }
                if (PublicDataClass.ChanelId == 2)
                {
                    for (int j = 0; j < PublicDataClass._DataField.FieldVSQ; j++)
                    {

                        if (PublicDataClass.SaveText.Cfg[dx].YccfgState[datepos] == "是")
                        //      
                        {
                            if ((PublicDataClass.DataTy == 35) || (PublicDataClass.DataTy == 40))//浮点型
                            {
                                bytes[0] = PublicDataClass._DataField.Buffer[k + 3];
                                bytes[1] = PublicDataClass._DataField.Buffer[k + 4];
                                bytes[2] = PublicDataClass._DataField.Buffer[k + 5];
                                bytes[3] = PublicDataClass._DataField.Buffer[k + 6];

                                fdata = BitConverter.ToSingle(bytes, 0);
                                k += 5;
                                PublicDataClass.SaveText.Cfg[dx].Ycdata[datepos] = String.Format("{0:f4}", fdata);
                            }
                            if ((PublicDataClass.DataTy == 36) || (PublicDataClass.DataTy == 38))//整型  带品质描述
                            {

                                data = PublicDataClass._DataField.Buffer[k + 3] + (PublicDataClass._DataField.Buffer[k + 4] << 8);

                                k += 3;
                                if (data > 0x8000)
                                    data = data - 65536;
                                PublicDataClass.SaveText.Cfg[dx].Ycdata[datepos] = Convert.ToString(data);


                            }
                            if (PublicDataClass.DataTy == 42)//整型 不带品质描述
                            {

                                data = PublicDataClass._DataField.Buffer[k + 3] + (PublicDataClass._DataField.Buffer[k + 4] << 8);

                                k += 2;
                                if (data > 0x8000)
                                    data = data - 65536;
                                PublicDataClass.SaveText.Cfg[dx].Ycdata[datepos] = Convert.ToString(data);

                            }

                        }
                        datepos++;

                        if (datepos >= PublicDataClass.SaveText.Cfg[dx].YccfgNum)
                        {
                            break;

                        }
                    }
                }
                else if (PublicDataClass.ChanelId == 1)
                {
                    for (int j = 0; j < PublicDataClass._DataField.FieldVSQ; j++)
                    {

                        if (PublicDataClass.SaveText.Cfg[dx].YccfgState[datepos] == "是")
                        //      
                        {
                            if ((PublicDataClass.DataTy == 35) || (PublicDataClass.DataTy == 40))//浮点型
                            {
                                if (PublicDataClass.inflen == 2)
                                {
                                    bytes[0] = PublicDataClass._DataField.Buffer[k + 2];
                                    bytes[1] = PublicDataClass._DataField.Buffer[k + 3];
                                    bytes[2] = PublicDataClass._DataField.Buffer[k + 4];
                                    bytes[3] = PublicDataClass._DataField.Buffer[k + 5];

                                    fdata = BitConverter.ToSingle(bytes, 0);
                                    k += 5;
                                }
                                else if (PublicDataClass.inflen == 3)
                                {
                                    bytes[0] = PublicDataClass._DataField.Buffer[k + 3];
                                    bytes[1] = PublicDataClass._DataField.Buffer[k + 4];
                                    bytes[2] = PublicDataClass._DataField.Buffer[k + 5];
                                    bytes[3] = PublicDataClass._DataField.Buffer[k + 6];

                                    fdata = BitConverter.ToSingle(bytes, 0);
                                    k += 5;
                                }
                                PublicDataClass.SaveText.Cfg[dx].Ycdata[datepos] = String.Format("{0:f4}", fdata);
                            }
                            if ((PublicDataClass.DataTy == 36) || (PublicDataClass.DataTy == 38))//整型  带品质描述
                            {
                                if (PublicDataClass.inflen == 2)
                                {
                                    data = PublicDataClass._DataField.Buffer[k + 2] + (PublicDataClass._DataField.Buffer[k + 3] << 8);
                                }
                                if (PublicDataClass.inflen == 3)
                                {
                                    data = PublicDataClass._DataField.Buffer[k + 3] + (PublicDataClass._DataField.Buffer[k + 4] << 8);
                                }
                                k += 3;
                                if (data > 0x8000)
                                    data = data - 65536;
                                PublicDataClass.SaveText.Cfg[dx].Ycdata[datepos] = Convert.ToString(data);


                            }
                            if (PublicDataClass.DataTy == 42)//整型 不带品质描述
                            {
                                if (PublicDataClass.inflen == 2)
                                {
                                    data = PublicDataClass._DataField.Buffer[k + 2] + (PublicDataClass._DataField.Buffer[k + 3] << 8);
                                }
                                else if (PublicDataClass.inflen == 3)
                                {
                                    data = PublicDataClass._DataField.Buffer[k + 3] + (PublicDataClass._DataField.Buffer[k + 4] << 8);
                                }

                                k += 2;
                                if (data > 0x8000)
                                    data = data - 65536;
                                PublicDataClass.SaveText.Cfg[dx].Ycdata[datepos] = Convert.ToString(data);

                            }

                        }
                        datepos++;

                        if (datepos >= PublicDataClass.SaveText.Cfg[dx].YccfgNum)
                        {
                            break;

                        }
                    }
                }
            }
            catch
            { }
        }
        /// <summary>
        /// 获取遥信数值信息
        /// </summary>
        private void GetYxDataInfo()
        {
            if (PublicDataClass._ChangeFlag.Clearflag2 == true)
            {
                datepos = 0;
                PublicDataClass._ChangeFlag.Clearflag2 = false;
            }
            try
            {
                byte dx = PublicFunction.FindPointNameCorrelativeIndex(comboBox1.Text);
                byte yxdata;
                if (datepos >= PublicDataClass.SaveText.Cfg[dx].YxcfgNum)
                {

                    return;

                }
                if (PublicDataClass.ChanelId == 2)
                {

                    int StartPos = 0;

                    StartPos = PublicDataClass._DataField.Buffer[2];
                    StartPos = StartPos << 16;
                    StartPos += PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] << 8);

                    for (int j = 0; j < PublicDataClass._DataField.FieldVSQ; j++)
                    {

                        if (PublicDataClass.SaveText.Cfg[dx].YxcfgState[datepos] == "是")
                        {
                                yxdata = PublicDataClass._DataField.Buffer[j + 3];
                                if (PublicDataClass.DataTy == 53)
                                {
                                    if (yxdata == 1)
                                        PublicDataClass.SaveText.Cfg[dx].Yxdata[datepos] = "分";
                                    else
                                        PublicDataClass.SaveText.Cfg[dx].Yxdata[datepos] = "合";
                                }
                                else
                                {
                                    if (yxdata == 0)
                                        PublicDataClass.SaveText.Cfg[dx].Yxdata[datepos] = "分";
                                    else
                                        PublicDataClass.SaveText.Cfg[dx].Yxdata[datepos] = "合";
                                }
                        }
                        datepos++;
                        if (datepos >= PublicDataClass.SaveText.Cfg[dx].YxcfgNum)
                        { 
                            break;

                        }

                    }
                }
                else if (PublicDataClass.ChanelId == 1)  //串口
                {
                    int addr = 0;
                    byte k = 0;
                    for (int j = 0; j < PublicDataClass._DataField.FieldVSQ; j++)
                    {
                        addr = PublicDataClass._DataField.Buffer[k] + (PublicDataClass._DataField.Buffer[k + 1] << 8);

                        if (PublicDataClass.SaveText.Cfg[dx].YxcfgState[addr - 1] == "是")
                        {
                            if (PublicDataClass.DataTy == 53)
                            {
                                if (PublicDataClass._DataField.Buffer[k + 2] == 1)
                                    PublicDataClass.SaveText.Cfg[dx].Yxdata[addr - 1] = "分";
                                else
                                    PublicDataClass.SaveText.Cfg[dx].Yxdata[addr - 1] = "合";
                            }
                            else
                            {
                                if (PublicDataClass._DataField.Buffer[k + 2] == 0)
                                    PublicDataClass.SaveText.Cfg[dx].Yxdata[addr - 1] = "分";
                                else
                                    PublicDataClass.SaveText.Cfg[dx].Yxdata[addr - 1] = "合";
                            }
                            k += 3;

                        }
                        datepos++;
                        if (datepos >= PublicDataClass.SaveText.Cfg[dx].YxcfgNum)
                        {

                            break;

                        }
                    }

                }
            }
            catch
            { }

        }

        //变位事件
        private void GetBWYxDataInfo()
        {
            try
            {
                byte dx = PublicFunction.FindPointNameCorrelativeIndex(comboBox1.Text);
                int dex = 0;
                int j = 0;
                if (PublicDataClass.DataTy == 52)//无时标的遥信变位
                {

                    for (int i = 0; i < PublicDataClass._DataField.FieldVSQ; i++)
                    {
                        int StartAddr = 0;
                        StartAddr = PublicDataClass._DataField.Buffer[dex + 2];
                        StartAddr = StartAddr << 16;
                        StartAddr += PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);

                        //for (j = 0; j < PublicDataClass.SaveText.Cfg[dx].YxcfgNum; j++)
                        //{
                        //    if (PublicDataClass.SaveText.Cfg[dx].YxcfgAddr[j] == Convert.ToString(StartAddr))
                        //        break;
                        //}
                        if (PublicDataClass._DataField.Buffer[dex + 3] == 0)
                            PublicDataClass.SaveText.Cfg[dx].Yxdata[StartAddr-1] = "分";
                        else
                            PublicDataClass.SaveText.Cfg[dx].Yxdata[StartAddr - 1] = "合";
                        dex += 4;
                    }
                }
                else if (PublicDataClass.DataTy == 54)//无时标的双点遥信变位
                {

                    for (int i = 0; i < PublicDataClass._DataField.FieldVSQ; i++)
                    {
                        int StartAddr = 0;
                        StartAddr = PublicDataClass._DataField.Buffer[dex + 2];
                        StartAddr = StartAddr << 16;
                        StartAddr += PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);

                        //for (j = 0; j < PublicDataClass.SaveText.Cfg[dx].YxcfgNum; j++)
                        //{
                        //    if (PublicDataClass.SaveText.Cfg[dx].YxcfgAddr[j] == Convert.ToString(StartAddr))
                        //        break;
                        //}
                        if (PublicDataClass._DataField.Buffer[dex + 3] == 1)
                            PublicDataClass.SaveText.Cfg[dx].Yxdata[StartAddr - 1] = "分";
                        else
                            PublicDataClass.SaveText.Cfg[dx].Yxdata[StartAddr - 1] = "合";
                        dex += 4;
                    }
                }
                else if (PublicDataClass.DataTy == 56) //有时标的遥信变位
                {

                    for (int i = 0; i < PublicDataClass._DataField.FieldVSQ; i++)
                    {
                        int StartAddr = 0; string DataInfo = @"";
                        StartAddr = PublicDataClass._DataField.Buffer[dex + 2];
                        StartAddr = StartAddr << 16;
                        StartAddr += PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);

                        //for (j = 0; j < PublicDataClass.SaveText.Cfg[dx].YxcfgNum; j++)
                        //{
                        //    if (PublicDataClass.SaveText.Cfg[dx].YxcfgAddr[j] == Convert.ToString(StartAddr))
                        //        break;
                        //}
                        if (PublicDataClass._DataField.Buffer[dex + 3] == 0)
                            //PublicDataClass.SaveText.Cfg[dx].Yxdata[j] = "分";
                            PublicDataClass.SaveText.Cfg[dx].Yxdata[StartAddr-1] = "分";
                        else
                            //PublicDataClass.SaveText.Cfg[dx].Yxdata[j] = "合";
                            PublicDataClass.SaveText.Cfg[dx].Yxdata[StartAddr-1] = "合";

                        dex += 11;
                    }
                }
                else if (PublicDataClass.DataTy == 58) //有时标的双点遥信变位
                {

                    for (int i = 0; i < PublicDataClass._DataField.FieldVSQ; i++)
                    {
                        int StartAddr = 0; string DataInfo = @"";
                        StartAddr = PublicDataClass._DataField.Buffer[dex + 2];
                        StartAddr = StartAddr << 16;
                        StartAddr += PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);

                        //for (j = 0; j < PublicDataClass.SaveText.Cfg[dx].YxcfgNum; j++)
                        //{
                        //    if (PublicDataClass.SaveText.Cfg[dx].YxcfgAddr[j] == Convert.ToString(StartAddr))
                        //        break;
                        //}
                        if (PublicDataClass._DataField.Buffer[dex + 3] == 1)
                            //PublicDataClass.SaveText.Cfg[dx].Yxdata[j] = "分";
                            PublicDataClass.SaveText.Cfg[dx].Yxdata[StartAddr - 1] = "分";
                        else
                            //PublicDataClass.SaveText.Cfg[dx].Yxdata[j] = "合";
                            PublicDataClass.SaveText.Cfg[dx].Yxdata[StartAddr - 1] = "合";

                        dex += 11;
                    }
                }
            }
            catch 
            { }


        }
        //扰动事件
        private void GetRDYcValueInfo()
        {
            try
            {
                /* for (int i = 0; i < PublicDataClass._DataField.FieldVSQ; i++)
                {
                   int StartAddr = 0; string DataInfo = @"";
                    StartAddr = PublicDataClass._DataField.Buffer[dex + 2];
                    StartAddr = StartAddr << 16;
                    StartAddr += PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);

                    lv = new ListViewItem(String.Format("{0:d}", listView1.Items.Count));
                    lv.SubItems.Add("<越限>");

                    lv.SubItems.Add(Convert.ToString(PublicDataClass.ThreeYNameTable.YcTable[StartAddr - 0x4001]));   //加入名称


                    lv.SubItems.Add(String.Format("(0-正常，1-越上限，2-越下限)：{0:d}", PublicDataClass._DataField.Buffer[3]));  //状态
                    DataInfo = "";
                    DataInfo = String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 10]);   //年
                    DataInfo += "年";
                    DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 9]);  //月
                    DataInfo += "月";
                    DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 8]);  //日+星期
                    DataInfo += "日";
                    DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 7]);  //时
                    DataInfo += "时";
                    DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 6]);  //分
                    DataInfo += "分";

                    lv.SubItems.Add(DataInfo);
                    lv.SubItems.Add(String.Format("{0:d}", (PublicDataClass._DataField.Buffer[dex + 5] << 8) + PublicDataClass._DataField.Buffer[dex + 4]));
                    listView1.Items.Add(lv);
                    dex += 11;
                }*/
                byte dx = PublicFunction.FindPointNameCorrelativeIndex(comboBox1.Text);
                byte[] bytes = new byte[4];
                int dex = 0;
                for (int i = 0; i < PublicDataClass._DataField.FieldVSQ; i++)
                {
                    int StartAddr = 0;
                    StartAddr = PublicDataClass._DataField.Buffer[dex + 2];
                    StartAddr = StartAddr << 16;
                    StartAddr += PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);

                    if (PublicDataClass.DataTy == 41)
                    {
                        bytes[0] = PublicDataClass._DataField.Buffer[dex + 3];
                        bytes[1] = PublicDataClass._DataField.Buffer[dex + 4];
                        bytes[2] = PublicDataClass._DataField.Buffer[dex + 5];
                        bytes[3] = PublicDataClass._DataField.Buffer[dex + 6];

                        float fdata = BitConverter.ToSingle(bytes, 0);  //转换为浮点
                        PublicDataClass.SaveText.Cfg[dx].Ycdata[StartAddr - 0x4001] = String.Format("{0:f4}", fdata);

                        //for (int j = 0; j < PublicDataClass.SaveText.Cfg[dx].YccfgNum; j++)
                        //{
                        //    if (PublicDataClass.SaveText.Cfg[dx].YccfgAddr[j] == Convert.ToString(StartAddr))
                        //        PublicDataClass.SaveText.Cfg[dx].Ycdata[j] = String.Format("{0:f4}", fdata);
                        //}
                        dex += 8;
                    }
                    if ((PublicDataClass.DataTy == 37) || (PublicDataClass.DataTy == 39))
                    {

                        int data = (PublicDataClass._DataField.Buffer[dex + 4] << 8) + PublicDataClass._DataField.Buffer[dex + 3];
                        if (data > 0x8000)
                            data = data - 65536;
                        PublicDataClass.SaveText.Cfg[dx].Ycdata[StartAddr - 0x4001] = Convert.ToString(data);
                        //for (int j = 0; j < PublicDataClass.SaveText.Cfg[dx].YccfgNum; j++)
                        //{
                        //    if (PublicDataClass.SaveText.Cfg[dx].YccfgAddr[j] == Convert.ToString(StartAddr))
                        //        PublicDataClass.SaveText.Cfg[dx].Ycdata[j] = Convert.ToString(data);
                        //}

                        dex += 6;
                    }
                    if (PublicDataClass.DataTy == 43)
                    {

                        int data = (PublicDataClass._DataField.Buffer[dex + 4] << 8) + PublicDataClass._DataField.Buffer[dex + 3];
                        if (data > 0x8000)
                            data = data - 65536;

                        PublicDataClass.SaveText.Cfg[dx].Ycdata[StartAddr - 0x4001] = Convert.ToString(data);
                        //for (int j = 0; j < PublicDataClass.SaveText.Cfg[dx].YccfgNum; j++)
                        //{
                        //    if (PublicDataClass.SaveText.Cfg[dx].YccfgAddr[j] == Convert.ToString(StartAddr))
                        //        PublicDataClass.SaveText.Cfg[dx].Ycdata[j] = Convert.ToString(data);
                        //}

                        dex += 5;
                    }
                }
            }
            catch
            { }
        }
        #endregion

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
               
                delaytime = Convert.ToInt16(domainUpDownb.Text);
                timer2.Enabled = true;

            }
            else
            {
               
                timer2.Enabled = false;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

            if (PublicDataClass._WindowsVisable.RealTimeDataVisable == true)//窗体可见
            {
                delaytime--;
                if (delaytime == 0)
                {

                    if (PublicDataClass.LinSPointName == "无信息")
                    {
                        MessageBox.Show("无测量点信息可操作", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    if (SelectType == 1)    //F0_遥测
                    {
                        datepos = 0;
                        //listView1.Items.Clear();
                        if (ty == 1)
                            PublicDataClass._ComTaskFlag.AloneCallYc_1 = true;

                        if (ty == 2)
                            PublicDataClass._NetTaskFlag.AloneCallYc_1 = true;
                        if (ty == 3)
                            PublicDataClass._GprsTaskFlag.AloneCallYc_1 = true;
                        if (ty == 4)
                            PublicDataClass._UsbTaskFlag.AloneCallYc_1 = true;

                    }

                    else if (SelectType == 2)  //F1__遥信
                    {
                        datepos = 0;
                        //listView1.Items.Clear();
                        if (ty == 1)
                            PublicDataClass._ComTaskFlag.AloneCallYx_1 = true;

                        if (ty == 2)
                            PublicDataClass._NetTaskFlag.AloneCallYx_1 = true;

                        if (ty == 3)
                            PublicDataClass._GprsTaskFlag.AloneCallYx_1 = true;

                        if (ty == 4)
                            PublicDataClass._UsbTaskFlag.AloneCallYx_1 = true;
                    }
                    delaytime = Convert.ToInt16(domainUpDownb.Text);

                }
            }
        }

        private void comboBoxselete_SelectedIndexChanged(object sender, EventArgs e)
        {
            

                if (comboBoxselete.SelectedIndex == 0)
                    ShowType = 0;//选择类型  0：按线路   1：按序号

                else if (comboBoxselete.SelectedIndex == 1)
                    ShowType = 1;//选择类型  0：按线路   1：按序号

                ///////////每选择一次相当于点击遥测页面一次/////////////////////////////////
                dex = 2;
                SelectType = 1;
                this.listView1.Items.Clear();
                this.listView1.Columns.Clear();
                this.listView1.Columns.Add("序号", 60);
                this.listView1.Columns.Add("名称", 250);
                this.listView1.Columns.Add("地址", 100);
                this.listView1.Columns.Add("值", 100);
                this.listView1.Columns.Add("单位", 100);
                this.listView1.Columns.Add("倍率", 60);

                if (PublicDataClass.SaveText.cfgnum != 0)
                {
                    po = FindCfgInfo();
                    if (ShowType == 1)//选择类型  0：按线路   1：按序号
                    {
                        for (int j = 0; j < PublicDataClass.SaveText.Cfg[po].YccfgNum; j++)
                        {

                            ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));
                            lv.SubItems.Add(PublicDataClass.SaveText.Cfg[po].YccfgName[j]);
                            lv.SubItems.Add(PublicDataClass.SaveText.Cfg[po].YccfgAddr[j]);
                            if (PublicDataClass.SaveText.Cfg[po].Ycdata[j] == null)
                                lv.SubItems.Add("0");
                            else
                                lv.SubItems.Add(PublicDataClass.SaveText.Cfg[po].Ycdata[j]);
                            lv.SubItems.Add("null");
                            lv.SubItems.Add("1");

                            listView1.Items.Add(lv);
                            lv.UseItemStyleForSubItems = false;
                            listView1.Items[j].SubItems[3].ForeColor = Color.Blue;
                        }
                    }
                    //////////////////////////////////////////////按显示序号显示//////////////////////////////////////////////////////////////////////
                    if (ShowType == 0)//选择类型  0：按线路   1：按序号
                    {
                        try
                        {
                            for (int i = 0; i < PublicDataClass.SaveText.Cfg[po].YccfgNum; i++)//循环遍历数据表中的每一行数据
                            {
                                for (int j = 0; j < PublicDataClass.SaveText.Cfg[po].YccfgNum; j++)
                                {
                                    //  if (Convert.ToInt16(PublicDataClass._YcInformationParam.IndexTable[j]) == i)
                                    if (Convert.ToInt16(PublicDataClass.SaveText.Cfg[po].YccfgIndex[j]) == i)
                                    {
                                        ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));
                                        lv.SubItems.Add(PublicDataClass.SaveText.Cfg[po].YccfgName[j]);
                                        lv.SubItems.Add(PublicDataClass.SaveText.Cfg[po].YccfgAddr[j]);
                                        if (PublicDataClass.SaveText.Cfg[po].Ycdata[j] == null)
                                            lv.SubItems.Add("0");
                                        else
                                            lv.SubItems.Add(PublicDataClass.SaveText.Cfg[po].Ycdata[j]);
                                        lv.SubItems.Add("null");
                                        lv.SubItems.Add("1");

                                        listView1.Items.Add(lv);
                                        lv.UseItemStyleForSubItems = false;
                                        // listView1.Items[j].SubItems[3].ForeColor = Color.Blue;
                                    }
                                }
                            }
                        }
                        catch { }
                    }
                    /////////////////////////////////////////////按显示序号显示//////////////////////////////////////////////////////////////////////
                }
            }

  

      

    
         
        
    }
}
