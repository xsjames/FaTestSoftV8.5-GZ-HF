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
using System.Runtime.InteropServices;
using FaTestSoft.INIT;
using System.IO;



namespace FaTestSoft
{
    public partial class XCPUDocmentView : DockContent
    {
        public XCPUDocmentView()
        {
            InitializeComponent();
        }
        public static byte ItemId = 0;
        static byte index = 0;
        private int ty = 0;
        public int num = 0;
        //TabPage[] tp = new TabPage[50];
        TabPage[] tp ;
        private Control[] Editors;
        DateCodForm Codefm = new DateCodForm();
        [DllImport("DataConVert.dll")]
        private static extern void InttoByte(int source, ref byte pdata);
        private void textBoxpassword_TextChanged(object sender, EventArgs e)
        {

            if (textBoxpassword.Text == "abcd")
            {
                radioreset.Enabled = true;
                radioSPIreset.Enabled = true;
           
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                switch (e.TabPage.Name)
                {
                    case "tabPage1"://对时
                        ItemId = 1;
                        downloadbutton.Enabled = false;
                        readbutton.Enabled = false;
                        buttonsave.Enabled = false;
                        inputbutton.Enabled = false;
                        button1.Enabled = false;
                        button2.Enabled = false;
                        break;
                    case "tabPage2"://复位
                        ItemId = 2;
                        radioreset.Enabled = false;
                        radioSPIreset.Enabled = false;
                        downloadbutton.Enabled = false;
                        readbutton.Enabled = false;
                        buttonsave.Enabled = false;
                        inputbutton.Enabled = false;
                        button1.Enabled = false;
                        button2.Enabled = false;
                        break;
                    case "tabPage3"://读文件
                        ItemId = 3;
                        downloadbutton.Enabled = false;
                        readbutton.Enabled = false;
                        buttonsave.Enabled = false;
                        inputbutton.Enabled = false;
                        button1.Enabled = false;
                        button2.Enabled = false;
                        Codefm.TopLevel = false;
                        tabPage3.Controls.Add(Codefm);

                        Codefm.StartPosition = FormStartPosition.Manual;  //修改窗体的起始位置 后才能修改top和left
                        Codefm.Top = 0;
                        Codefm.Left = 0;
                        Codefm.Width = this.Width;
                        Codefm.Height = this.Height;
                    

                        Codefm.Show();
                        break;

                    default:
                        string str;
                        downloadbutton.Enabled = true ;
                        readbutton.Enabled = true;
                        buttonsave.Enabled = true;
                        inputbutton.Enabled = true;
                        button1.Enabled = true;
                        button2.Enabled = true;
                 
                        if (PublicDataClass.XieDynOptCfgFlag == 1)//协处理器动态选项卡已配置
                        {
                            for (int k = 0; k < PublicDataClass.XIEFILENAME.Length; k++)
                            {
                                downloadbutton.Enabled = true;
                                str = String.Format("tp_{0:d}", k);
                                if (e.TabPage.Name == str)
                                {
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
        private void CheckNowDynOptParamState(int k)//更新动态选项卡参数
        {
            try
            {
                listViewtest.Items.Clear();
                listViewtest.Columns.Clear();
                listViewtest.Controls.Add(textBoxvalue);
           
                for (int j = 0; j < PublicDataClass.XieTabCfg[k].ColumnNum; j++)
                {

                    if (j == 1)
                        listViewtest.Columns.Add(PublicDataClass.XieTabCfg[k].ColumnPageName[j], 400, HorizontalAlignment.Left);//第一个参数，表头名，第2个参数，表头大小，第3个参数，样式  
                    else
                        listViewtest.Columns.Add(PublicDataClass.XieTabCfg[k].ColumnPageName[j], 100, HorizontalAlignment.Left);//第一个参数，表头名，第2个参数，表头大小，第3个参数，样式    
                }

                for (int q = 0; q < PublicDataClass.XieTabCfg[k].LineNum; q++)
                {
                    ListViewItem lv = new ListViewItem(PublicDataClass.XieTabCfg[k].TabPageValue[0].ValueTable[q]);
                    for (int j = 0; j < PublicDataClass.XieTabCfg[k].ColumnNum - 1; j++)
                    {

                        lv.SubItems.Add(PublicDataClass.XieTabCfg[k].TabPageValue[j + 1].ValueTable[q]);
                    }
                    listViewtest.Items.Add(lv);
                }



            }
            catch
            { 
            }
        }
        private void buttonexe_Click(object sender, EventArgs e)
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

            if (index == 1)
            {
                PublicDataClass.ParamInfoAddr = 10;
            }
            else if (index == 2)
            {
             
                PublicDataClass.ParamInfoAddr = 11;

    
            }
            else
                return;

            PublicDataClass._DataField.FieldLen = 0;
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0x01;
            PublicDataClass._DataField.FieldLen++;
            PublicDataClass._DataField.FieldVSQ = 1;
            PublicDataClass.seqflag = 0;
            PublicDataClass.seq = 1;
            PublicDataClass.SQflag = 0;

            if (ty == 1)
                PublicDataClass._ComTaskFlag.SET_PARAM_CON = true;

            if (ty == 2)
                PublicDataClass._NetTaskFlag.SET_PARAM_CON = true;
            if (ty == 3)
                PublicDataClass._GprsTaskFlag.SET_PARAM_CON = true;


        }

        private void radioreset_CheckedChanged(object sender, EventArgs e)
        {
            index = 1;
        }

        private void radioSPIreset_CheckedChanged(object sender, EventArgs e)
        {
            index = 2;
        }

        private void XCPUDocmentView_Load(object sender, EventArgs e)
        {
            Editors = new Control[] {
	                                textBoxvalue,
									textBoxvalue,			// for column 1
                                    textBoxvalue,
                                    textBoxvalue,           //
											// 
                                                                  
									};
            if (PublicDataClass.XieDynOptCfgFlag == 1)//协处理器动态选项卡已配置
            {
                tp = new TabPage[PublicDataClass.XIEFILENAME.Length];

            }
         
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
            ItemId = 1;
            downloadbutton.Enabled = false;
            readbutton.Enabled = false;
            buttonsave.Enabled = false;
            inputbutton.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void buttonsystime_Click(object sender, EventArgs e)
        {
            textBoxyear.Text = Convert.ToString(DateTime.Now.Year);             //获取系统时间
            textBoxmon.Text = Convert.ToString(DateTime.Now.Month);
            textBoxdata.Text = Convert.ToString(DateTime.Now.Day);
            textBoxhour.Text = Convert.ToString(DateTime.Now.Hour);
            textBoxmin.Text = Convert.ToString(DateTime.Now.Minute);
            textBoxsec.Text = Convert.ToString(DateTime.Now.Second);
            textBoxms.Text = Convert.ToString(DateTime.Now.Millisecond);
        }

        private void buttonjs_Click(object sender, EventArgs e)
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

            byte data2 = 0;
            DateTime thedate = new DateTime();
            thedate = Convert.ToDateTime(textBoxyear.Text + "-" + textBoxmon.Text + "-" + textBoxdata.Text);
            switch (thedate.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    data2 = 1;
                    break;
                case DayOfWeek.Tuesday:
                    data2 = 2;
                    break;
                case DayOfWeek.Wednesday:
                    data2 = 3;
                    break;
                case DayOfWeek.Thursday:
                    data2 = 4;
                    break;
                case DayOfWeek.Friday:
                    data2 = 5;
                    break;
                case DayOfWeek.Saturday:
                    data2 = 6;
                    break;
                case DayOfWeek.Sunday:
                    data2 = 7;
                    break;
                default:
                    data2 = 0;
                    break;
            }
   
         
            PublicDataClass._DataField.FieldLen = 0;

            byte[] b = BitConverter.GetBytes(long.Parse(textBoxsec.Text));
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = b[0];
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = b[1];
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = b[2];
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 3] = b[3];
            PublicDataClass._DataField.FieldLen += 4;

            b = BitConverter.GetBytes(long.Parse(textBoxmin.Text));
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = b[0];
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = b[1];
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = b[2];
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 3] = b[3];
            PublicDataClass._DataField.FieldLen += 4;

            b = BitConverter.GetBytes(long.Parse(textBoxhour.Text));
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = b[0];
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = b[1];
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = b[2];
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 3] = b[3];
            PublicDataClass._DataField.FieldLen += 4;

            b = BitConverter.GetBytes(long.Parse(textBoxdata.Text));
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = b[0];
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = b[1];
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = b[2];
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 3] = b[3];
            PublicDataClass._DataField.FieldLen += 4;

            b = BitConverter.GetBytes(long.Parse(textBoxmon.Text));
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = b[0];
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = b[1];
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = b[2];
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 3] = b[3];
            PublicDataClass._DataField.FieldLen += 4;

            b = BitConverter.GetBytes(long.Parse(textBoxyear.Text));
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = b[0];
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = b[1];
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = b[2];
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 3] = b[3];
            PublicDataClass._DataField.FieldLen += 4;
            //b = BitConverter.GetBytes(long.Parse( thedate.DayOfWeek));
            b = BitConverter.GetBytes((long)data2 );
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = b[0];
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = b[1];
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = b[2];
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 3] = b[3];
            PublicDataClass._DataField.FieldLen += 4;
             b = BitConverter.GetBytes(long.Parse(textBoxms.Text));
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = b[0];
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = b[1];
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = b[2];
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 3] = b[3];
            PublicDataClass._DataField.FieldLen += 4;


     
            PublicDataClass.ParamInfoAddr = 200;
            PublicDataClass._DataField.FieldVSQ = 8;
            PublicDataClass.seqflag = 0;
            PublicDataClass.seq = 1;
            PublicDataClass.SQflag = 0;

           if (ty == 1)
                PublicDataClass._ComTaskFlag.SET_PARAM_CON = true;

            if (ty == 2)
                PublicDataClass._NetTaskFlag.SET_PARAM_CON = true;
            if (ty == 3)
                PublicDataClass._GprsTaskFlag.SET_PARAM_CON = true;
        }

        private void button3_Click(object sender, EventArgs e)
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
            PublicDataClass.seq = 1;
            PublicDataClass.seqflag = 0;
            PublicDataClass.SQflag = 0;
            PublicDataClass.ParamInfoAddr = 200;
            PublicDataClass._DataField.FieldLen = 0;
            if (ty == 1)
                PublicDataClass._ComTaskFlag.READ_P_1 = true;

            if (ty == 2)
                PublicDataClass._NetTaskFlag.READ_P_1 = true;
            timer2.Enabled = true;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (PublicDataClass._WindowsVisable.XietParamVisable == true)  //窗体可见
            {
                if (PublicDataClass._Message.ReadParam == true)
                {
                    PublicDataClass._Message.ReadParam = false;
                    timer2.Enabled = false;
                    int dex = 0;

                    if (ItemId == 1)   //对时
                    {
                        try
                        {
                            if (PublicDataClass.addselect == 3)//协处理器
                            {
                                if (PublicDataClass.ParamInfoAddr == 200)//读时间
                                {
                                    byte[] bytes = new byte[4];
                                    bytes[0] = PublicDataClass._DataField.Buffer[dex];
                                    bytes[1] = PublicDataClass._DataField.Buffer[dex + 1];
                                    bytes[2] = PublicDataClass._DataField.Buffer[dex + 2];
                                    bytes[3] = PublicDataClass._DataField.Buffer[dex + 3];
                                    long fdata = BitConverter.ToInt32(bytes, 0);
                                    textBoxsec.Text = String.Format("{0:d}", fdata);//秒
                                    dex += 4;

                                    bytes[0] = PublicDataClass._DataField.Buffer[dex];
                                    bytes[1] = PublicDataClass._DataField.Buffer[dex + 1];
                                    bytes[2] = PublicDataClass._DataField.Buffer[dex + 2];
                                    bytes[3] = PublicDataClass._DataField.Buffer[dex + 3];
                                    fdata = BitConverter.ToInt32(bytes, 0);
                                    textBoxmin.Text = String.Format("{0:d}", fdata);//分
                                    dex += 4;

                                    bytes[0] = PublicDataClass._DataField.Buffer[dex];
                                    bytes[1] = PublicDataClass._DataField.Buffer[dex + 1];
                                    bytes[2] = PublicDataClass._DataField.Buffer[dex + 2];
                                    bytes[3] = PublicDataClass._DataField.Buffer[dex + 3];
                                    fdata = BitConverter.ToInt32(bytes, 0);
                                    textBoxhour.Text = String.Format("{0:d}", fdata);//时
                                    dex += 4;

                                    bytes[0] = PublicDataClass._DataField.Buffer[dex];
                                    bytes[1] = PublicDataClass._DataField.Buffer[dex + 1];
                                    bytes[2] = PublicDataClass._DataField.Buffer[dex + 2];
                                    bytes[3] = PublicDataClass._DataField.Buffer[dex + 3];
                                    fdata = BitConverter.ToInt32(bytes, 0);
                                    textBoxdata.Text = String.Format("{0:d}", fdata);//日
                                    dex += 4;

                                    bytes[0] = PublicDataClass._DataField.Buffer[dex];
                                    bytes[1] = PublicDataClass._DataField.Buffer[dex + 1];
                                    bytes[2] = PublicDataClass._DataField.Buffer[dex + 2];
                                    bytes[3] = PublicDataClass._DataField.Buffer[dex + 3];
                                    fdata = BitConverter.ToInt32(bytes, 0);
                                    textBoxmon.Text = String.Format("{0:d}", fdata);//月
                                    dex += 4;

                                    bytes[0] = PublicDataClass._DataField.Buffer[dex];
                                    bytes[1] = PublicDataClass._DataField.Buffer[dex + 1];
                                    bytes[2] = PublicDataClass._DataField.Buffer[dex + 2];
                                    bytes[3] = PublicDataClass._DataField.Buffer[dex + 3];
                                    fdata = BitConverter.ToInt32(bytes, 0);
                                    textBoxyear.Text = String.Format("{0:d}", fdata);//年
                                    dex += 8;

                                    bytes[0] = PublicDataClass._DataField.Buffer[dex];
                                    bytes[1] = PublicDataClass._DataField.Buffer[dex + 1];
                                    bytes[2] = PublicDataClass._DataField.Buffer[dex + 2];
                                    bytes[3] = PublicDataClass._DataField.Buffer[dex + 3];
                                    fdata = BitConverter.ToInt32(bytes, 0);
                                    textBoxms.Text = String.Format("{0:d}", fdata);//毫秒

                                }
                                else if (PublicDataClass.ParamInfoAddr == 1)//读版本号
                                {
                                    string str = "";
                                    for (byte i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                        str += Convert.ToChar(PublicDataClass._DataField.Buffer[i]);
                                    label12.Text = str;

                                }

                            }
                        }

                        catch
                        {
                        }

                    }
                    else if (ItemId - 0x14 >= 0)  //动态选项卡
                    {
                        for (int q = 0; q < PublicDataClass._DataField.FieldVSQ; q++)
                        {
                            for (int col = 0; col < PublicDataClass.XieTabCfg[ItemId - 0x14].ColumnNum; col++)
                            {

                                if (PublicDataClass.XieTabCfg[ItemId - 0x14].ColumnDownByte[col] == "1")
                                {
                                    byte value = 0;
                                    string str = "";
                                    value = PublicDataClass._DataField.Buffer[dex];
                                    if (PublicDataClass.addselect == 3)//协处理器
                                    {  //公钥SID16进制显示
                                        if (PublicDataClass.ParamInfoAddr == 301 || PublicDataClass.ParamInfoAddr == 302 || PublicDataClass.ParamInfoAddr == 303 || PublicDataClass.ParamInfoAddr == 304 || PublicDataClass.ParamInfoAddr == 305)
                                        {

                                            if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.XieTabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text != String.Format("{0:d}", value))
                                            {
                                                listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.XieTabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                                listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.XieTabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].ForeColor = Color.Red;
                                            }
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.XieTabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text = String.Format("{0:d}", value);
                                        }

                                    }
                                    else
                                    {
                                        
                                    }
                                    dex += 1;
                                }
                                else if (PublicDataClass.XieTabCfg[ItemId - 0x14].ColumnDownByte[col] == "2")
                                {
                                    int value = 0;
                                    value += PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);
                                    if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.XieTabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text != Convert.ToString(value))
                                    {
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.XieTabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.XieTabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].ForeColor = Color.Red;
                                    }
                                    listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.XieTabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text = Convert.ToString(value);
                                    dex += 2;
                                }
                                else if (PublicDataClass.XieTabCfg[ItemId - 0x14].ColumnDownByte[col] == "4")
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
                                            if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.XieTabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text != String.Format("{0:d}", fdata))
                                            {
                                                listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.XieTabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                                listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.XieTabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].ForeColor = Color.Red;
                                            }
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.XieTabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text = String.Format("{0:d}", fdata);
                                        }
                                    }
                                    else
                                    {

                                    }
                                    dex += 4;
                                }
                                else if (PublicDataClass.XieTabCfg[ItemId - 0x14].ColumnDownByte[col] == "按字节数下载")
                                {
                                    int m;
                                    string zijie;
                                    for (m = 0; m < PublicDataClass.XieTabCfg[ItemId - 0x14].ColumnNum; m++)
                                    {
                                        if (PublicDataClass.XieTabCfg[ItemId - 0x14].ColumnDownByte[m] == "字节数")
                                            break;
                                    }
                                    zijie = PublicDataClass.XieTabCfg[ItemId - 0x14].TabPageValue[m].ValueTable[q];
                                    if (zijie == "1")
                                    {

                                        byte value = 0;
                                        value = PublicDataClass._DataField.Buffer[dex];



                                        if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.XieTabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text != Convert.ToString(value))
                                        {
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.XieTabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.XieTabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].ForeColor = Color.Red;
                                        }
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.XieTabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text = Convert.ToString(value);

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
                                        if (listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.XieTabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text != String.Format("{0:d}", fdata))
                                        {
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.XieTabCfg[ItemId - 0x14].DownAddr + q].UseItemStyleForSubItems = false;
                                            listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.XieTabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].ForeColor = Color.Red;
                                        }
                                        listViewtest.Items[PublicDataClass.ParamInfoAddr - PublicDataClass.XieTabCfg[ItemId - 0x14].DownAddr + q].SubItems[col].Text = String.Format("{0:d}", fdata);
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
                                        }
                                        listViewtest.Items[q].SubItems[2].Text = Convert.ToString(value);
                                        dex += 6;
                                    }


                                }
                            }

                        }
                        RefreshParamState();
                    }


                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //读版本号
            PublicDataClass.seq = 1;
            PublicDataClass.seqflag = 0;
            PublicDataClass.SQflag = 0;
            PublicDataClass.ParamInfoAddr = 1;
            PublicDataClass._DataField.FieldLen = 0;
            if (ty == 1)
                PublicDataClass._ComTaskFlag.READ_P_1 = true;

            if (ty == 2)
                PublicDataClass._NetTaskFlag.READ_P_1 = true;
            timer2.Enabled = true;
        }

        private void XCPUDocmentView_Activated(object sender, EventArgs e)
        {
            PublicDataClass._WindowsVisable.XietParamVisable = true;            //窗体可见
            PublicDataClass.addselect = 3;//协处理器


            if (PublicDataClass.XieDynOptCfgFlag == 1)
                DynOptProcess();//动态添加选项卡
        
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
        public void DynOptProcess()               //动态选项卡处理函数
        {
            try
            {
            
                for (int k = 0; k < PublicDataClass.XIEFILENAME.Length; k++)
                {
                    if (this.tabControl1.Controls.Contains(tp[k]))
                        this.tabControl1.Controls.Remove(tp[k]);

                }


                if (PublicDataClass.XIEFILENAME.Length > 0)
               
                {
                    for (int k = 0; k < PublicDataClass.XIEFILENAME.Length; k++)
                    {

                        //WriteReadAllFile.ReadDynOptFile(PublicDataClass.FILENAME[k], k, 1);
                        // 针对数据库的字段名称，建立与之适应显示表头
                        listViewtest.Items.Clear();
                        listViewtest.Columns.Clear();
                        for (int j = 0; j < PublicDataClass.XieTabCfg[k].ColumnNum; j++)
                        {
                            listViewtest.Columns.Add(PublicDataClass.XieTabCfg[k].ColumnPageName[j], 100, HorizontalAlignment.Left);//第一个参数，表头名，第2个参数，表头大小，第3个参数，样式    

                        }

                        for (int q = 0; q < PublicDataClass.XieTabCfg[k].LineNum; q++)
                        {
                            ListViewItem lv = new ListViewItem(PublicDataClass.XieTabCfg[k].TabPageValue[0].ValueTable[q]);
                            for (int j = 0; j < PublicDataClass.XieTabCfg[k].ColumnNum - 1; j++)
                            {

                                lv.SubItems.Add(PublicDataClass.XieTabCfg[k].TabPageValue[j + 1].ValueTable[q]);
                            }
                            listViewtest.Items.Add(lv);
                        }

                        tp[k] = new TabPage();
                        tp[k].Controls.Add(this.textBoxvalue);
                        tp[k].Controls.Add(this.listViewtest);
                        tp[k].Location = new System.Drawing.Point(4, 25);
                        string str = String.Format("tp_{0:d}", k);
                        tp[k].Name = str;
                        tp[k].Padding = new System.Windows.Forms.Padding(3);
                        tp[k].Size = new System.Drawing.Size(769, 375);
                        tp[k].TabIndex = 10 + k;//待定
                        tp[k].Text = PublicDataClass.XieTabCfg[k].PageName;
                        tp[k].UseVisualStyleBackColor = true;
                        tabControl1.Controls.Add(tp[k]);
                    }
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////


                }

            }
            catch
            { }

        }

        private void XCPUDocmentView_SizeChanged(object sender, EventArgs e)
        {
                  if ( ItemId == 3)
            {
                Codefm.Width = this.Width;
                Codefm.Height = this.Height;

            }
        }

        private void textBoxvalue_Leave(object sender, EventArgs e)
        {
            listViewtest.EndEditing(true);
            RefreshParamState();
        }
        private void RefreshParamState()
        {
                  if (ItemId - 0x14 >= 0)  //动态选项卡
            {
                PublicDataClass.XieTabCfg[ItemId - 0x14].LineNum = listViewtest.Items.Count;
       

                for  (int j = 0; j < PublicDataClass.XieTabCfg[ItemId - 0x14].ColumnNum; j++)
                {

                    PublicDataClass.XieTabCfg[ItemId - 0x14].TabPageValue[j].ValueTable = new string[PublicDataClass.XieTabCfg[ItemId - 0x14].LineNum];
                    for (int q = 0; q < PublicDataClass.XieTabCfg[ItemId - 0x14].LineNum; q++)
                    {
                        PublicDataClass.XieTabCfg[ItemId - 0x14].TabPageValue[j].ValueTable[q] = listViewtest.Items[q].SubItems[j].Text;
                    }
                }

            }
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

        private void listViewtest_SubItemClicked(object sender, SubItemEventArgs e)
        {
             if (this.listViewtest.SelectedItems.Count == 0)
                return;
             if (e.SubItem == 0) // Password field
             {
                 return;
             }

             else
             {
                
                     listViewtest.StartEditing(Editors[e.SubItem], e.Item, e.SubItem);
                 

             }
        }

        private void InsertMenuItem_Click(object sender, EventArgs e)
        {
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
        }

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
        }

        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
               if (MessageBox.Show("确定要删除此项吗?", "信  息",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                                 if (ItemId - 0x14 >= 0)  //动态选项卡
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

        private void ModifyMenuItem_Click(object sender, EventArgs e)
        {
            try
            {


                PublicDataClass._Mystruct.bpos = 0;
                PublicDataClass._Mystruct.epos = listViewtest.Items.Count;

                CModifyViewForm cfm = new CModifyViewForm();
                cfm.ShowDialog();
                if (cfm.DialogResult == DialogResult.OK)               //判断是否按下-确定-按钮
                {


                    for (int i = PublicDataClass._Mystruct.bpos; i <= PublicDataClass._Mystruct.epos; i++)
                    {
                        listViewtest.Items[i].SubItems[PublicDataClass._Mystruct.row].Text = Convert.ToString(PublicDataClass._Mystruct.value);   //重新调整序号

                    }


                    RefreshParamState();
                }

            }
            catch
            {
                MessageBox.Show("数值输入有误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void downloadbutton_Click(object sender, EventArgs e)
        {
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

                          if (ItemId - 0x14 >= 0)  //动态选项卡
                {
                    PublicDataClass.SQflag = 0;
        
                    int YCcount = 0;
                    PublicDataClass._DataField.FieldLen = 0;
                    PublicDataClass._DataField.FieldVSQ=0;
     
           
                        string thetemp;
                        for (int q = 0; q < PublicDataClass.XieTabCfg[ItemId - 0x14].LineNum; q++)
                        {
                            for (int col = 0; col < PublicDataClass.XieTabCfg[ItemId - 0x14].ColumnNum; col++)
                            {
                                try
                                {
                                    int a = PublicDataClass.XieTabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q].IndexOf("（");
                                    if (a < 0)
                                        a = PublicDataClass.XieTabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q].IndexOf("(");
                                    if (a > 0)
                                    {
                                        thetemp = PublicDataClass.XieTabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q].Substring(0, a);
                                    }
                                    else
                                        thetemp = PublicDataClass.XieTabCfg[ItemId - 0x14].TabPageValue[col].ValueTable[q];

                                    if (PublicDataClass.XieTabCfg[ItemId - 0x14].ColumnDownByte[col] == "1")
                                    {
                                        int value;
                                        bool flag = int.TryParse(thetemp, out value);
                                        if (flag == true)
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(thetemp);
                                        else
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)(Convert.ToChar(thetemp));
                                        PublicDataClass._DataField.FieldLen++;
                                    }
                                    else if (PublicDataClass.XieTabCfg[ItemId - 0x14].ColumnDownByte[col] == "2")
                                    {
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(thetemp)) & 0x00ff);
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(thetemp)) & 0xff00) >> 8);
                                     //   InttoByte(Convert.ToInt32(thetemp), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                                        PublicDataClass._DataField.FieldLen += 2;
                                    }
                                    else if (PublicDataClass.XieTabCfg[ItemId - 0x14].ColumnDownByte[col] == "4")
                                    {
                                  
                                        byte[] b = BitConverter.GetBytes(float.Parse(thetemp));
                                        if (PublicDataClass.addselect == 3&&PublicDataClass.ParamInfoAddr == 200)//协处理器long
                                          b = BitConverter.GetBytes(long.Parse(thetemp));

                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = b[0];
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = b[1];
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = b[2];
                                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 3] = b[3];


                                        //  InttoByte(Convert.ToInt16(PublicDataClass._RaoDong.ValueTable[i]), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
                                        PublicDataClass._DataField.FieldLen += 4;
                                    }
                                    else if (PublicDataClass.XieTabCfg[ItemId - 0x14].ColumnDownByte[col] == "按字节数下载")
                                    {
                                        int m;
                                        string zijie;
                                        for (m = 0; m < PublicDataClass.XieTabCfg[ItemId - 0x14].ColumnNum; m++)
                                        {
                                            if (PublicDataClass.XieTabCfg[ItemId - 0x14].ColumnDownByte[m] == "字节数")
                                                break;
                                        }
                                        zijie = PublicDataClass.XieTabCfg[ItemId - 0x14].TabPageValue[m].ValueTable[q];
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
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(thetemp)) & 0x00ff);
                                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(thetemp)) & 0xff00) >> 8);
                                       //     InttoByte(Convert.ToInt32(thetemp), ref PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen]);
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
                            //if ((PublicDataClass._DataField.FieldLen >= PublicDataClass.Framelen-10))      //一帧的长度,
                            //{
                            //    dataPos = q;
                            //    Pos = PublicDataClass._DataField.FieldVSQ;
                            //    timer1.Enabled = true;
                            //    PublicDataClass.seqflag = 1;
                            //    break;

                            //}
                            PublicDataClass.seqflag = 0;
                        }
                        
                    PublicDataClass.seq = 1;
                    PublicDataClass.ParamInfoAddr = PublicDataClass.XieTabCfg[ItemId - 0x14].DownAddr;//待定
                 

                }

                if (ty == 1)
                    PublicDataClass._ComTaskFlag.SET_PARAM_CON = true;

                if (ty == 2)
                    PublicDataClass._NetTaskFlag.SET_PARAM_CON = true;
                if (ty == 3)
                    PublicDataClass._GprsTaskFlag.SET_PARAM_CON = true;
            }
            catch
            { 
            
            }
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
            PublicDataClass._DataField.FieldLen = 0;
            PublicDataClass._DataField.FieldVSQ = 0;
            PublicDataClass.seq = 0;
                       if (ItemId - 0x14 >= 0)  //动态选项卡
            {
                PublicDataClass.seq = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.SQflag = 0;
                PublicDataClass.ParamInfoAddr = PublicDataClass.XieTabCfg[ItemId - 0x14].DownAddr;//待定
            }

            timer2.Enabled = true;
            if (ty == 1)
                PublicDataClass._ComTaskFlag.READ_P_1 = true;

            if (ty == 2)
                PublicDataClass._NetTaskFlag.READ_P_1 = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (ItemId - 0x14 >= 0)
            {
                WriteReadAllFile.ReadDynOptFile(PublicDataClass.XIEFILENAME[ItemId - 0x14], ItemId - 0x14 + PublicDataClass.FILENAME.Length, 1);
                for (int i = 0; i < PublicDataClass.XIEFILENAME.Length; i++)
                {
                    PublicDataClass.XieTabCfg[i] = PublicDataClass.TabCfg[i + PublicDataClass.FILENAME.Length];
                }
                listViewtest.Items.Clear();
                CheckNowDynOptParamState(ItemId - 0x14);//更新动态选项卡参数
            }
        }

        private void buttonsave_Click(object sender, EventArgs e)
        {
                       if (ItemId - 0x14 >= 0)  //动态选项卡
            {
                PublicDataClass.XieTabCfg[ItemId - 0x14].LineNum = listViewtest.Items.Count;
   

                WriteReadAllFile.ReadDynOptFile(PublicDataClass.XIEFILENAME[ItemId - 0x14], ItemId - 0x14+PublicDataClass.FILENAME.Length, 2);

            }


            MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void inputbutton_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void XCPUDocmentView_Deactivate(object sender, EventArgs e)
        {
            PublicDataClass._WindowsVisable.XietParamVisable = false ;            //窗体不可见
            PublicDataClass.addselect = 2;//主处理器
        }



    }
}
