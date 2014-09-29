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
    public partial class ChangeInfoViewDocment : DockContent
    {
        public ChangeInfoViewDocment()
        {
            InitializeComponent();
        }
        ListViewItem lv;
        int dx = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                int dex = 0;
                //if (PublicDataClass._Message.RaoDongEvent == true)
                if (PublicDataClass.RevNetFrameMsg == "扰动事件" && (PublicDataClass._Message.RaoDongEvent == true))
                {
                    if (PublicDataClass.DataTy == 41)
                    {
                        byte[] bytes = new byte[4];
                        PublicDataClass._Message.RaoDongEvent = false;
                        for (int i = 0; i < PublicDataClass._DataField.FieldVSQ; i++)
                        {
                            int StartAddr = 0;
                            StartAddr = PublicDataClass._DataField.Buffer[dex + 2];
                            StartAddr = StartAddr << 16;
                            StartAddr += PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);

                            lv = new ListViewItem(String.Format("{0:d}", listView1.Items.Count));
                            lv.SubItems.Add("<死区>");
                            //lv.SubItems.Add(Convert.ToString(PublicDataClass.ThreeYNameTable.YcTable[StartAddr - 0x4001]));   //加入名称
                            //lv.SubItems.Add(Convert.ToString(PublicDataClass.SaveText.Cfg[dx].YccfgName[StartAddr - 0x4001]));   //加入名称 
                            if ((StartAddr - 0x4001) < PublicDataClass.SaveText.Cfg[dx].YccfgNum)//加入名称
                                lv.SubItems.Add(Convert.ToString(PublicDataClass.SaveText.Cfg[dx].YccfgName[StartAddr - 0x4001]));   //加入名称
                            else
                                lv.SubItems.Add("配置表无对应名称！！！");

                            bytes[0] = PublicDataClass._DataField.Buffer[dex + 3];
                            bytes[1] = PublicDataClass._DataField.Buffer[dex + 4];
                            bytes[2] = PublicDataClass._DataField.Buffer[dex + 5];
                            bytes[3] = PublicDataClass._DataField.Buffer[dex + 6];

                            float fdata = BitConverter.ToSingle(bytes, 0);  //转换为浮点

                            lv.SubItems.Add(Convert.ToString(String.Format("{0:f4}", fdata)));

                            lv.SubItems.Add("无");
                            lv.SubItems.Add("无");
                            listView1.Items.Add(lv);
                            dex += 8;
                        }
                    }
                    if ((PublicDataClass.DataTy == 37) || (PublicDataClass.DataTy == 39))
                    {

                        PublicDataClass._Message.RaoDongEvent = false;
                        for (int i = 0; i < PublicDataClass._DataField.FieldVSQ; i++)
                        {
                            int StartAddr = 0;
                            StartAddr = PublicDataClass._DataField.Buffer[dex + 2];
                            StartAddr = StartAddr << 16;
                            StartAddr += PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);

                            lv = new ListViewItem(String.Format("{0:d}", listView1.Items.Count));
                            lv.SubItems.Add("<死区>");
                            //lv.SubItems.Add(Convert.ToString(PublicDataClass.ThreeYNameTable.YcTable[StartAddr - 0x4001]));   //加入名称
                            //lv.SubItems.Add(Convert.ToString(PublicDataClass.SaveText.Cfg[dx].YccfgName[StartAddr - 0x4001]));   //加入名称 
                            if ((StartAddr - 0x4001) < PublicDataClass.SaveText.Cfg[dx].YccfgNum)//加入名称
                                lv.SubItems.Add(Convert.ToString(PublicDataClass.SaveText.Cfg[dx].YccfgName[StartAddr - 0x4001]));   //加入名称
                            else
                                lv.SubItems.Add("配置表无对应名称！！！");
                            int data = (PublicDataClass._DataField.Buffer[dex + 4] << 8) + PublicDataClass._DataField.Buffer[dex + 3];
                            if (data > 0x8000)
                                data = data - 65536;


                            lv.SubItems.Add(Convert.ToString(String.Format("{0:d}", data)));

                            lv.SubItems.Add("无");
                            lv.SubItems.Add("无");
                            listView1.Items.Add(lv);
                            dex += 6;
                        }
                    }
                    if (PublicDataClass.DataTy == 43)
                    {

                        PublicDataClass._Message.RaoDongEvent = false;
                        for (int i = 0; i < PublicDataClass._DataField.FieldVSQ; i++)
                        {
                            int StartAddr = 0;
                            StartAddr = PublicDataClass._DataField.Buffer[dex + 2];
                            StartAddr = StartAddr << 16;
                            StartAddr += PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);

                            lv = new ListViewItem(String.Format("{0:d}", listView1.Items.Count));
                            lv.SubItems.Add("<死区>");
                            //lv.SubItems.Add(Convert.ToString(PublicDataClass.ThreeYNameTable.YcTable[StartAddr - 0x4001]));   //加入名称
                            //lv.SubItems.Add(Convert.ToString(PublicDataClass.SaveText.Cfg[dx].YccfgName[StartAddr - 0x4001]));   //加入名称 
                            if ((StartAddr - 0x4001) < PublicDataClass.SaveText.Cfg[dx].YccfgNum)//加入名称
                                lv.SubItems.Add(Convert.ToString(PublicDataClass.SaveText.Cfg[dx].YccfgName[StartAddr - 0x4001]));   //加入名称
                            else
                                lv.SubItems.Add("配置表无对应名称！！！");
                            int data = (PublicDataClass._DataField.Buffer[dex + 4] << 8) + PublicDataClass._DataField.Buffer[dex + 3];
                            if (data > 0x8000)
                                data = data - 65536;


                            lv.SubItems.Add(Convert.ToString(String.Format("{0:d}", data)));

                            lv.SubItems.Add("无");
                            lv.SubItems.Add("无");
                            listView1.Items.Add(lv);
                            dex += 5;
                        }
                    }




                }
                
                //else if (PublicDataClass._Message.YxBianWeiOfNoTimeEvent == true)
                else if ((PublicDataClass.RevNetFrameMsg == "变位事件") && (PublicDataClass.DataTy == 52) && (PublicDataClass._Message.YxBianWeiOfNoTimeEvent == true))
                {
                    PublicDataClass._Message.YxBianWeiOfNoTimeEvent = false;
                    for (int i = 0; i < PublicDataClass._DataField.FieldVSQ; i++)
                    {
                        int StartAddr = 0;
                        StartAddr = PublicDataClass._DataField.Buffer[dex + 2];
                        StartAddr = StartAddr << 16;
                        StartAddr += PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);

                        lv = new ListViewItem(String.Format("{0:d}", listView1.Items.Count));
                        lv.SubItems.Add("<单点遥信变位>");
                        //lv.SubItems.Add(Convert.ToString(PublicDataClass.ThreeYNameTable.YxTable[StartAddr - 1]));
                        if ((StartAddr - 1 )< PublicDataClass.SaveText.Cfg[dx].YxcfgNum)//加入名称
                            lv.SubItems.Add(Convert.ToString(PublicDataClass.SaveText.Cfg[dx].YxcfgName[StartAddr - 1]));   //加入名称
                        else
                            lv.SubItems.Add("配置表无对应名称！！！");
                        lv.SubItems.Add(String.Format("(0-分，1-合)：{0:d}", PublicDataClass._DataField.Buffer[dex + 3]));  //状态

                        lv.SubItems.Add("无");
                        lv.SubItems.Add("无");
                        listView1.Items.Add(lv);
                        dex += 4;
                    }
                }
                else if ((PublicDataClass.RevNetFrameMsg == "变位事件") && (PublicDataClass.DataTy == 54) && (PublicDataClass._Message.YxBianWeiOfNoTimeEvent == true))
                {
                    PublicDataClass._Message.YxBianWeiOfNoTimeEvent = false;
                    for (int i = 0; i < PublicDataClass._DataField.FieldVSQ; i++)
                    {
                        int StartAddr = 0;
                        StartAddr = PublicDataClass._DataField.Buffer[dex + 2];
                        StartAddr = StartAddr << 16;
                        StartAddr += PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);

                        lv = new ListViewItem(String.Format("{0:d}", listView1.Items.Count));
                        lv.SubItems.Add("<双点遥信变位>");
                        //lv.SubItems.Add(Convert.ToString(PublicDataClass.ThreeYNameTable.YxTable[StartAddr - 1]));
                        if ((StartAddr - 1) < PublicDataClass.SaveText.Cfg[dx].YxcfgNum)//加入名称
                            lv.SubItems.Add(Convert.ToString(PublicDataClass.SaveText.Cfg[dx].YxcfgName[StartAddr - 1]));   //加入名称
                        else
                            lv.SubItems.Add("配置表无对应名称！！！");
                        lv.SubItems.Add(String.Format("(1-分，2-合)：{0:d}", PublicDataClass._DataField.Buffer[dex + 3]));  //状态

                        lv.SubItems.Add("无");
                        lv.SubItems.Add("无");
                        listView1.Items.Add(lv);
                        dex += 4;
                    }
                }
                //else if (PublicDataClass._Message.YxBianWeiOfTimeEvent == true)
                else if ((PublicDataClass.RevNetFrameMsg == "变位事件") && (PublicDataClass.DataTy == 56) && (PublicDataClass._Message.YxBianWeiOfTimeEvent == true))
                {
                    PublicDataClass._Message.YxBianWeiOfTimeEvent = false;
                    for (int i = 0; i < PublicDataClass._DataField.FieldVSQ; i++)
                    {
                        int StartAddr = 0; string DataInfo = @"";
                        StartAddr = PublicDataClass._DataField.Buffer[dex + 2];
                        StartAddr = StartAddr << 16;
                        StartAddr += PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);

                        lv = new ListViewItem(String.Format("{0:d}", listView1.Items.Count));

                        lv.SubItems.Add("<单点遥信变位>");
                        //lv.SubItems.Add(Convert.ToString(PublicDataClass.ThreeYNameTable.YxTable[StartAddr - 1]));   //加入名称
                        //lv.SubItems.Add(Convert.ToString(PublicDataClass.SaveText.Cfg[dx].YxcfgName[StartAddr - 1]));   //加入名称
                        if ((StartAddr - 1) < PublicDataClass.SaveText.Cfg[dx].YxcfgNum)//加入名称
                            lv.SubItems.Add(Convert.ToString(PublicDataClass.SaveText.Cfg[dx].YxcfgName[StartAddr - 1]));   //加入名称
                        else
                            lv.SubItems.Add("配置表无对应名称！！！");
                        lv.SubItems.Add(String.Format("(0-分，1-合)：{0:d}", PublicDataClass._DataField.Buffer[dex + 3]));  //状态
                        DataInfo = "";
                        DataInfo = String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 10]);   //年
                        DataInfo += "年";
                        DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 9]);  //月
                        DataInfo += "月";
                        DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 8]);  //日
                        DataInfo += "日";
                        DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 7]);  //时
                        DataInfo += "时";
                        DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 6]);  //分
                        DataInfo += "分";

                        lv.SubItems.Add(DataInfo);
                        lv.SubItems.Add(String.Format("{0:d}", (PublicDataClass._DataField.Buffer[dex + 5] << 8) + PublicDataClass._DataField.Buffer[dex + 4]));
                        listView1.Items.Add(lv);
                        dex += 11;
                    }
                }
                else if ((PublicDataClass.RevNetFrameMsg == "变位事件") && (PublicDataClass.DataTy == 58) && (PublicDataClass._Message.YxBianWeiOfTimeEvent == true))
                {
                    PublicDataClass._Message.YxBianWeiOfTimeEvent = false;
                    for (int i = 0; i < PublicDataClass._DataField.FieldVSQ; i++)
                    {
                        int StartAddr = 0; string DataInfo = @"";
                        StartAddr = PublicDataClass._DataField.Buffer[dex + 2];
                        StartAddr = StartAddr << 16;
                        StartAddr += PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);

                        lv = new ListViewItem(String.Format("{0:d}", listView1.Items.Count));

                        lv.SubItems.Add("<双点遥信变位>");
                        //lv.SubItems.Add(Convert.ToString(PublicDataClass.ThreeYNameTable.YxTable[StartAddr - 1]));   //加入名称
                        //lv.SubItems.Add(Convert.ToString(PublicDataClass.SaveText.Cfg[dx].YxcfgName[StartAddr - 1]));   //加入名称
                        if ((StartAddr - 1) < PublicDataClass.SaveText.Cfg[dx].YxcfgNum)//加入名称
                            lv.SubItems.Add(Convert.ToString(PublicDataClass.SaveText.Cfg[dx].YxcfgName[StartAddr - 1]));   //加入名称
                        else
                            lv.SubItems.Add("配置表无对应名称！！！");
                        lv.SubItems.Add(String.Format("(1-分，2-合)：{0:d}", PublicDataClass._DataField.Buffer[dex + 3]));  //状态
                        DataInfo = "";
                        DataInfo = String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 10]);   //年
                        DataInfo += "年";
                        DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 9]);  //月
                        DataInfo += "月";
                        DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 8]);  //日
                        DataInfo += "日";
                        DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 7]);  //时
                        DataInfo += "时";
                        DataInfo += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 6]);  //分
                        DataInfo += "分";

                        lv.SubItems.Add(DataInfo);
                        lv.SubItems.Add(String.Format("{0:d}", (PublicDataClass._DataField.Buffer[dex + 5] << 8) + PublicDataClass._DataField.Buffer[dex + 4]));
                        listView1.Items.Add(lv);
                        dex += 11;
                    }
                }
            }
            catch { }
        }

        private void buttonclear_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void ChangeInfoViewDocment_Activated(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void ChangeInfoViewDocment_Deactivate(object sender, EventArgs e)
        {
            //timer1.Enabled = false;
        }

        private void ChangeInfoViewDocment_Load(object sender, EventArgs e)
        {
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
            for (int j = 0; j < PublicDataClass.SaveText.cfgnum; j++)
            {
                if (comboBox1.Text == PublicDataClass.SaveText.Device[j].PointName)
                {
                    dx = j;
                    break;
                }
            }
        }

        
    }
}
