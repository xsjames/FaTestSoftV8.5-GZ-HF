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
using System.Runtime.InteropServices;

namespace FaTestSoft
{
    public partial class ParamAntoLoadForm : Form
    {
        public ParamAntoLoadForm()
        {
            InitializeComponent();
        }

        [DllImport("DataConVert.dll")]
        private static extern void InttoByte(int source, ref byte pdata);


        [DllImport("Operateprotocol.dll")]
        private static extern byte EncodeOneByte(byte BusNum, byte CardNum, byte UbusConnectionmode, byte IbusConnectionmode, ref byte pdata);
        [DllImport("Operateprotocol.dll")]
        private static extern byte EncodeThreeByte(int Index, byte Datatype, byte Magnification, ref byte pdata);
        public int num = 0;
        int ty = 0;
        static byte index = 0;

        public static int dataPos = 0;
        public static int Pos = 0;
        private int count = 0;

        private void ParamAntoLoadForm_Load(object sender, EventArgs e)
        {
            byte i = 0;
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

            listViewSoure.Items.Add("网络参数");//
            listViewSoure.Items.Add("GPRS参数");//
            listViewSoure.Items.Add("串口参数");//
            listViewSoure.Items.Add("系统参数");//
            listViewSoure.Items.Add("遥测参数");//
            listViewSoure.Items.Add("遥信参数");//
            listViewSoure.Items.Add("遥控参数");//
            listViewSoure.Items.Add("遥信配置参数");
            listViewSoure.Items.Add("遥测配置参数");
            listViewSoure.Items.Add("扰动参数");//
            DecideTrueOrFalse();//当listBox1中不存在选择项时，设定所有按钮为不可用状态
           
        }
        private void DecideTrueOrFalse()
        {
            if (listViewSoure.SelectedIndices.Count == 0)//当listBox1中不存在选择项时，设定所有按钮为不可用状态
            {
                bsomelefttoright.Enabled = false;
                bsomerighttoleft.Enabled = false;
                balonerighttoleft.Enabled = false;
                balonelefttoright.Enabled = false;

            }
        }

        private void balonelefttoright_Click(object sender, EventArgs e)
        {
            DecideTrueOrFalse();//当listBox1中不存在选择项时，设定所有按钮为不可用状态
            TransferRightTechnique();//
           
        }
        private void TransferRightTechnique()
        {
            ListView.SelectedListViewItemCollection SettleOnItem = new ListView.SelectedListViewItemCollection(this.listViewSoure);//定义一个选择项的集合
            for (int i = 0; i < SettleOnItem.Count; )//循环遍历选择的每一项
            {
                listView1.Items.Add(SettleOnItem[i].Text);//向listView2中添加选择项
                listViewSoure.Items.Remove(SettleOnItem[i]);//从listView1中移除选择项
            }
        }
        private void TransferLeftTechnique()
        {
            ListView.SelectedListViewItemCollection SettleOnItem = new ListView.SelectedListViewItemCollection(this.listView1);//定义一个选择项的集合
            for (int i = 0; i < SettleOnItem.Count; )//循环遍历选择的每一项
            {
                listViewSoure.Items.Add(SettleOnItem[i].Text);//向listView1中添加选择项
                listView1.Items.Remove(SettleOnItem[i]);//从listView2中移除选择项
            }
        }
        private void bsomelefttoright_Click(object sender, EventArgs e)
        {
         
            DecideTrueOrFalse();//当listBox1中不存在选择项时，设定所有按钮为不可用状态
            TransferRightTechnique();
        }

        private void balonerighttoleft_Click(object sender, EventArgs e)
        {
            DecideTrueOrFalse();//当listBox1中不存在选择项时，设定所有按钮为不可用状态
            TransferLeftTechnique();
        }

        private void bsomerighttoleft_Click(object sender, EventArgs e)
        {
            /*for (int i = 0; i < listBoxaim.SelectedItems.Count; )//循环遍历listBox2中的所有选定项
            {
                listBoxsoure.Items.Add(listBoxaim.SelectedItems[i]);//向listBox1中添加listBox2中选定的项
                listBoxaim.Items.Remove(listBoxaim.SelectedItems[i]);//移除listBox2中的选定项
            }*/
            DecideTrueOrFalse();//当listBox1中不存在选择项时，设定所有按钮为不可用状态
            TransferLeftTechnique();
        }

        private void listViewSoure_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewSoure.SelectedIndices.Count == 0)//当listBoxsoure中的选定项为空时
            {
                bsomelefttoright.Enabled  = false;
                bsomerighttoleft.Enabled  = false;
                balonerighttoleft.Enabled = false;
                balonelefttoright.Enabled = false;
            }
            else if (listViewSoure.SelectedIndices.Count == 1)//当listBoxsoure中的选定项为1时
            {
                bsomelefttoright.Enabled  = false;
                balonelefttoright.Enabled = true;
                bsomerighttoleft.Enabled  = false;
                balonerighttoleft.Enabled = false;
            }
            else if (listViewSoure.SelectedIndices.Count > 1)//当listBoxsoure中的选定项大于1时
            {
                bsomelefttoright.Enabled  = true;
                balonelefttoright.Enabled = false;
                bsomerighttoleft.Enabled  = false;
                balonerighttoleft.Enabled = false;
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)//当listBoxsoure中的选定项为空时
            {
                bsomelefttoright.Enabled  = false;
                bsomerighttoleft.Enabled  = false;
                balonerighttoleft.Enabled = false;
                balonelefttoright.Enabled = false;
            }
            else if (listView1.SelectedIndices.Count == 1)//当listBoxsoure中的选定项为1时
            {
                bsomelefttoright.Enabled  = false;
                balonelefttoright.Enabled = false;
                bsomerighttoleft.Enabled  = false;
                balonerighttoleft.Enabled = true;
            }
            else if (listView1.SelectedIndices.Count > 1)//当listBoxsoure中的选定项大于1时
            {
                bsomelefttoright.Enabled  = false;
                balonelefttoright.Enabled = false;
                bsomerighttoleft.Enabled  = true;
                balonerighttoleft.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            index = 0;
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
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("当前项为空", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }
            else
            {
                ProcessSeleteTask(index);

            }
        }
        #region user-defined
        private void ProcessSeleteTask(byte ix)
        {
            string text = @"";
            PublicDataClass._DataField.FieldLen = 0;
            PublicDataClass._DataField.FieldVSQ = 0;
            
            switch (listView1.Items[ix].SubItems[0].Text)
            {
                case "网络参数":
                    //准备数据包 ix++ 启动多帧发送定时器 定时器事件里先关闭该定时器 调用ProcessSeleteTask(byte ix)函数
                    index++;
                    timer1.Enabled = true;
                    

                    text = PublicDataClass._NetParam.IP;
                    for (byte i = 0; i < 4; i++)
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
                    text = PublicDataClass._NetParam.GwIP;
                    for (byte i = 0; i < 4; i++)
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
                    text = PublicDataClass._NetParam.SubMask;
                    for (byte i = 0; i < 4; i++)
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

                    InttoByte(Convert.ToInt16(PublicDataClass._NetParam.Port), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                    PublicDataClass._DataField.FieldLen += 2;

                    text = PublicDataClass._NetParam.SrcHA;
                    for (byte i = 0; i < 6; i++)
                    {
                        if (i < 5)
                        {
                            int a = text.IndexOf("-");
                            string cs = text.Substring(0, a);
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = PublicFunction.StringToByte(cs);

                            //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = PublicFunction.HextoBCD(Convert.ToByte(text.Substring(0, a)));
                            text = text.Remove(0, a + 1);
                        }
                        else
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = PublicFunction.StringToByte(text);
                        PublicDataClass._DataField.FieldLen++;

                    }
                    PublicDataClass._DataField.FieldVSQ = 5;
                    PublicDataClass.ParamInfoAddr = 0x5001 + PublicDataClass.NetIndex;
                    if (ty == 1)
                        PublicDataClass._ComTaskFlag.SET_PARAM_CON = true;

                    if (ty == 2)
                        PublicDataClass._NetTaskFlag.SET_PARAM_CON = true;
                    if (ty == 3)
                        PublicDataClass._GprsTaskFlag.SET_PARAM_CON = true;
                  
                    break;

                case "GPRS参数":
                    //准备数据包 ix++ 启动多帧发送定时器 定时器事件里先关闭该定时器 调用ProcessSeleteTask(byte ix)函数
                    index++;
                    timer1.Enabled = true;
                    text = PublicDataClass._GprsParam.IP;
                    for (byte i = 0; i < 4; i++)
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
                    text = PublicDataClass._GprsParam.BIP;
                    for (byte i = 0; i < 4; i++)
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
                    InttoByte(Convert.ToInt16(PublicDataClass._GprsParam.Port), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                    PublicDataClass._DataField.FieldLen += 2;

                    InttoByte(Convert.ToInt16(PublicDataClass._GprsParam.BPort), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                    PublicDataClass._DataField.FieldLen += 2;

                    InttoByte(Convert.ToInt16(PublicDataClass._GprsParam.Heart), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                    PublicDataClass._DataField.FieldLen += 2;

                    for (byte i = 0; i < PublicDataClass._GprsParam.APN.Length; i++)
                    {

                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(PublicDataClass._GprsParam.APN[i]);
                        PublicDataClass._DataField.FieldLen++;
                    }

                    PublicDataClass._DataField.FieldVSQ = 6;
                    //PublicDataClass.ParamInfoAddr = 0x5002;
                    if (ty == 1)
                        PublicDataClass._ComTaskFlag.SET_PARAM_CON = true;

                    if (ty == 2)
                        PublicDataClass._NetTaskFlag.SET_PARAM_CON = true;
                    if (ty == 3)
                        PublicDataClass._GprsTaskFlag.SET_PARAM_CON = true;
                    break;

                case "串口参数":  
                    index++;
                    timer1.Enabled = true;
                    for (byte i = 0; i < 4; i++)
                    {
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(PublicDataClass._ComParam.BaudRateTable[i]);
                        PublicDataClass._DataField.FieldLen++;
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(PublicDataClass._ComParam.JyTable[i]);
                        PublicDataClass._DataField.FieldLen++;
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(PublicDataClass._ComParam.DataBitTable[i]);
                        PublicDataClass._DataField.FieldLen++;
                    }
                    PublicDataClass._DataField.FieldVSQ = 4;
                    PublicDataClass.ParamInfoAddr = 0x5200;
                    if (ty == 1)
                        PublicDataClass._ComTaskFlag.SET_PARAM_CON = true;

                    if (ty == 2)
                        PublicDataClass._NetTaskFlag.SET_PARAM_CON = true;
                    if (ty == 3)
                        PublicDataClass._GprsTaskFlag.SET_PARAM_CON = true;
                    break;

                case "系统参数":
                    index++;
                    timer1.Enabled = true;
                    for (byte i = 0; i < PublicDataClass._SysParam.num; i++)
                    {
                        if (PublicDataClass._SysParam.ByteTable[i] == "1")
                        {
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(PublicDataClass._SysParam.ValueTable[i]);
                            PublicDataClass._DataField.FieldLen++;

                        }
                        else if (PublicDataClass._SysParam.ByteTable[i] == "2")
                        {
                            InttoByte(Convert.ToInt32(PublicDataClass._SysParam.ValueTable[i]), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                            PublicDataClass._DataField.FieldLen += 2;
                        }
                        PublicDataClass._DataField.FieldVSQ++;
                    }
                    PublicDataClass.ParamInfoAddr = 0x5100;
                    if (ty == 1)
                        PublicDataClass._ComTaskFlag.SET_PARAM_CON = true;

                    if (ty == 2)
                        PublicDataClass._NetTaskFlag.SET_PARAM_CON = true;
                    if (ty == 3)
                        PublicDataClass._GprsTaskFlag.SET_PARAM_CON = true;
                    break;

                case "遥控参数":
                    index++;
                    timer1.Enabled = true;

                    for (int i = 0; i < PublicDataClass._YkParam.num / 3; i++)
                    {
                        if (PublicDataClass._YkParam.ByteTable[i] == "1")
                        {
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(PublicDataClass._YkParam.ValueTable[i]);
                            PublicDataClass._DataField.FieldLen++;

                        }
                        else if (PublicDataClass._YkParam.ByteTable[i] == "2")
                        {
                            InttoByte(Convert.ToInt16(PublicDataClass._YkParam.ValueTable[i]), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                            PublicDataClass._DataField.FieldLen += 2;
                        }
                        PublicDataClass._DataField.FieldVSQ++;
                    }
                    PublicDataClass.ParamInfoAddr = 0x9001;

                    if (ty == 1)
                        PublicDataClass._ComTaskFlag.SET_PARAM_CON = true;

                    if (ty == 2)
                        PublicDataClass._NetTaskFlag.SET_PARAM_CON = true;
                    if (ty == 3)
                        PublicDataClass._GprsTaskFlag.SET_PARAM_CON = true;

                    break;

                case "遥信参数":
                    index++;
                    timer1.Enabled = true;

                    for (int i = 0; i < PublicDataClass._YxParam.num / 3; i++)
                    {
                        if (PublicDataClass._YxParam.ByteTable[i] == "1")
                        {
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(PublicDataClass._YxParam.ValueTable[i]);
                            PublicDataClass._DataField.FieldLen++;

                        }
                        else if (PublicDataClass._YxParam.ByteTable[i] == "2")
                        {
                            InttoByte(Convert.ToInt16(PublicDataClass._YxParam.ValueTable[i]), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                            PublicDataClass._DataField.FieldLen += 2;
                        }
                        PublicDataClass._DataField.FieldVSQ++;
                    }
                    PublicDataClass.ParamInfoAddr = 0x8001;
                    if (ty == 1)
                        PublicDataClass._ComTaskFlag.SET_PARAM_CON = true;

                    if (ty == 2)
                        PublicDataClass._NetTaskFlag.SET_PARAM_CON = true;
                    if (ty == 3)
                        PublicDataClass._GprsTaskFlag.SET_PARAM_CON = true;
                    break;

                case "遥测参数":
                    index++;
                    timer1.Enabled = true;

                    for (int i = 0; i < PublicDataClass._YcParam.num / 3; i++)
                    {
                        if (PublicDataClass._YcParam.ByteTable[i] == "1")
                        {
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(PublicDataClass._YcParam.ValueTable[i]);
                            PublicDataClass._DataField.FieldLen++;

                        }
                        else if (PublicDataClass._YcParam.ByteTable[i] == "2")
                        {
                            InttoByte(Convert.ToInt16(PublicDataClass._YcParam.ValueTable[i]), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                            PublicDataClass._DataField.FieldLen += 2;
                        }
                        PublicDataClass._DataField.FieldVSQ++;
                    }
                    PublicDataClass.ParamInfoAddr = 0x7001;
                    if (ty == 1)
                        PublicDataClass._ComTaskFlag.SET_PARAM_CON = true;

                    if (ty == 2)
                        PublicDataClass._NetTaskFlag.SET_PARAM_CON = true;
                    if (ty == 3)
                        PublicDataClass._GprsTaskFlag.SET_PARAM_CON = true;
                    break;

                case "遥信配置参数":
                    index++;
                    timer1.Enabled = true;
                    for (int i = 0; i < PublicDataClass._YxLineCfgParam.num / 3; i++)
                    {
                        if (PublicDataClass._YxLineCfgParam.ByteTable[i] == "1")
                        {
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(PublicDataClass._YxLineCfgParam.ValueTable[i]);
                            PublicDataClass._DataField.FieldLen++;

                        }
                        else if (PublicDataClass._YxLineCfgParam.ByteTable[i] == "2")
                        {
                            InttoByte(Convert.ToInt16(PublicDataClass._YxLineCfgParam.ValueTable[i]), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                            PublicDataClass._DataField.FieldLen += 2;
                        }
                        PublicDataClass._DataField.FieldVSQ++;
                    }
                    PublicDataClass.ParamInfoAddr = 0xa001;

                    if (ty == 1)
                        PublicDataClass._ComTaskFlag.SET_PARAM_CON = true;

                    if (ty == 2)
                        PublicDataClass._NetTaskFlag.SET_PARAM_CON = true;
                    if (ty == 3)
                        PublicDataClass._GprsTaskFlag.SET_PARAM_CON = true;
                    break;

                case "遥测配置参数":
                    
                    dataPos = 0;
                    count = 0;
                    for (int i = 0; i < PublicDataClass._YcLineCfgParam.num / 3; i++)
                    {

                        count++;
                        if (PublicDataClass._YcLineCfgParam.ByteTable[i] == "1")
                        {
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(PublicDataClass._YcLineCfgParam.ValueTable[i]);
                            PublicDataClass._DataField.FieldLen++;

                        }
                        else if (PublicDataClass._YcLineCfgParam.ByteTable[i] == "2")
                        {
                            InttoByte(Convert.ToInt16(PublicDataClass._YcLineCfgParam.ValueTable[i]), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                            PublicDataClass._DataField.FieldLen += 2;
                        }

                        PublicDataClass._DataField.FieldVSQ++;
                        if (PublicDataClass._DataField.FieldLen >= 240)       //一帧的长度
                        {
                            //StartMangFrameTransmit(ItemId);
                            dataPos = i;
                            Pos = 240;
                            timer2.Enabled = true;
                            break;

                        }
                        else
                        {
                            index++;
                            timer1.Enabled = true;
                        }
                    }
                    PublicDataClass.ParamInfoAddr = 0xb001;
                    if (ty == 1)
                        PublicDataClass._ComTaskFlag.SET_PARAM_CON = true;

                    if (ty == 2)
                        PublicDataClass._NetTaskFlag.SET_PARAM_CON = true;
                    if (ty == 3)
                        PublicDataClass._GprsTaskFlag.SET_PARAM_CON = true;
                    break;

                case "扰动参数":
                    index++;
                    timer1.Enabled = true;
                    for (int i = 0; i < PublicDataClass._RaoDong.num / 3; i++)
                    {
                        if (PublicDataClass._RaoDong.ByteTable[i] == "1")
                        {
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(PublicDataClass._RaoDong.ValueTable[i]);
                            PublicDataClass._DataField.FieldLen++;

                        }
                        else if (PublicDataClass._RaoDong.ByteTable[i] == "2")
                        {
                            InttoByte(Convert.ToInt16(PublicDataClass._RaoDong.ValueTable[i]), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                            PublicDataClass._DataField.FieldLen += 2;
                        }
                        PublicDataClass._DataField.FieldVSQ++;

                    }
                    PublicDataClass.ParamInfoAddr = 0x5300;
                    if (ty == 1)
                        PublicDataClass._ComTaskFlag.SET_PARAM_CON = true;

                    if (ty == 2)
                        PublicDataClass._NetTaskFlag.SET_PARAM_CON = true;
                    if (ty == 3)
                        PublicDataClass._GprsTaskFlag.SET_PARAM_CON = true;
                    break;
                    
            }
        }
       #endregion 

        private void timer1_Tick(object sender, EventArgs e)
        {
           
            if (PublicDataClass._Message.ParamAck == true)
            {
                PublicDataClass._Message.ParamAck = false;
                timer1.Enabled = false;
                if (index < listView1.Items.Count)
                ProcessSeleteTask(index);

            }
        }

        private void ParamAntoLoadForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            index = 0;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (PublicDataClass._Message.ParamAck == true)
            {
                PublicDataClass._Message.ParamAck = false;
                timer2.Enabled = false;
                StartMangFrameTransmit(1);

            }
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
                for (int i = dataPos + 1; i < PublicDataClass._YcLineCfgParam.num / 3; i++)
                {
                    count++;
                    if (PublicDataClass._YcLineCfgParam.ByteTable[i] == "1")
                    {
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(PublicDataClass._YcLineCfgParam.ValueTable[i]);
                        if (count >= 688 && count <= 705)
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen];
                        PublicDataClass._DataField.FieldLen++;

                    }
                    else if (PublicDataClass._YcLineCfgParam.ByteTable[i] == "2")
                    {
                        InttoByte(Convert.ToInt16(PublicDataClass._YcLineCfgParam.ValueTable[i]), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                        PublicDataClass._DataField.FieldLen += 2;
                    }
                    PublicDataClass._DataField.FieldVSQ++;
                    if (PublicDataClass._DataField.FieldLen >= 240)       //一帧的长度
                    {
                        //StartMangFrameTransmit(ItemId);
                        dataPos = 0;
                        dataPos = i;
                        Pos = 240;
                        timer2.Enabled = true;
                        break;

                    }
                    else
                    {
                        index++;
                        timer1.Enabled = true;
                    }
                }

            }
            else if (mid == 2) //遥测信息表
            {
                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 0;
                PublicDataClass.ParamInfoAddr += Pos;
                byte[] p2 = new byte[3];
                byte DatatypeTable, MagnificationTable;
                int IndexTable;
                for (int i = dataPos + 1; i < PublicDataClass._YcInformationParam.num / 4; i++)
                {
                    count++;
                    IndexTable = Convert.ToInt16(PublicDataClass._YcInformationParam.IndexTable[i]);
                    DatatypeTable = Convert.ToByte(PublicDataClass._YcInformationParam.DatatypeTable[i]);
                    MagnificationTable = Convert.ToByte(PublicDataClass._YcInformationParam.MagnificationTable[i]);
                    /* if (PublicDataClass._YcInformationParam.MagnificationTable[i]== "不放大")
                        MagnificationTable= 0;
                     else if (PublicDataClass._YcInformationParam.MagnificationTable[i]== "10倍")
                        MagnificationTable= 1;
                     else  if (PublicDataClass._YcInformationParam.MagnificationTable[i]== "100倍")
                        MagnificationTable= 2;
                     else  if (PublicDataClass._YcInformationParam.MagnificationTable[i]== "1000倍")
                         MagnificationTable= 3;*/

                    EncodeThreeByte(IndexTable, DatatypeTable, MagnificationTable, ref p2[0]);


                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = p2[0];
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = p2[1];
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = p2[2];
                    PublicDataClass._DataField.FieldLen += 3;


                    PublicDataClass._DataField.FieldVSQ++;
                    if (PublicDataClass._DataField.FieldLen >= 240)       //一帧的长度
                    {
                        //StartMangFrameTransmit(ItemId);
                        dataPos = i;
                        Pos = 240;
                        timer2.Enabled = true;
                        break;

                    }
                }

            }
            if (ty == 1)
                PublicDataClass._ComTaskFlag.SET_PARAM_CON = true;

            if (ty == 2)
                PublicDataClass._NetTaskFlag.SET_PARAM_CON = true;
        }



    }
}
