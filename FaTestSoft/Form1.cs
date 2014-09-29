using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics; 
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using KD.WinFormsUI.Docking;
using FaTestSoft.CommonData;
using FaTestSoft.ProtocolParam;
using FaTestSoft.PROTOCOL101;
using FaTestSoft.PROTOCOL104;//使用新增加的类所在的命名空间
using FaTestSoft.INIT;                    //使用新增加的类所在的命名空间
using ZedGraph;
using System.Data.OleDb;


public delegate bool mPCH375_NOTIFY_ROUTINE(uint iEventStatus);

namespace FaTestSoft
{
    public partial class Form1 : Form
    {

        [DllImport("Operateprotocol.dll")]

          private static extern int EncodeFrame(byte protocolty,byte dataty, ref byte pdata, ref byte DataField, int DataFieldLen,int VSQ,int DecAddr,int InfoAddr,int TxSeq,int RxSeq);
        [DllImport("Operateprotocol.dll")]
          private static extern byte DecodeFrame(byte protocolty, ref byte pdata, ref byte DataField, ref int DataLen,ref int VSQ,ref int InfoAdd,int FLen,ref int TxSeq,ref int RxSeq);
        
        private DeserializeDockContent m_deserializeDockContent;
        private ToolBox m_toolbox                        = new ToolBox();
        private CeLPointListViewForm m_celpointview      = new CeLPointListViewForm();
        private OutputWindows m_outputWindow             = new OutputWindows();
        private ProductionVersionFun m_productionfunview = new ProductionVersionFun();
        private DocumentView m_docview                   = new DocumentView();
        private RealTDataDocmentView m_realtdataview     = new RealTDataDocmentView();
        //private SysPDocmentView m_syspview               = new SysPDocmentView();
        private CallDataDocmentView m_calldview          = new CallDataDocmentView();      
        CreateNewPrjForm NewPrjform                      = new CreateNewPrjForm();           //构造一个新的对象
        private HistoryDataDocmemtView m_historydataview = new HistoryDataDocmemtView();
        private AcquisitionDocmentView m_acquisitionview = new AcquisitionDocmentView();
        private XMLCfg m_xmlcfgview = new XMLCfg();//XML配置视图
        private ControlDocmentView m_controlview         = new ControlDocmentView();
        private OtherTypeDocmentView m_otherview         = new OtherTypeDocmentView();
        private XCPUDocmentView m_xiecpuview = new XCPUDocmentView();
        private RoleManagerDocmentForm m_roleview        = new RoleManagerDocmentForm();
        private GraphicDocmentViewForm m_graphicview     = new GraphicDocmentViewForm();
        private ChangeInfoViewDocment m_changview        = new ChangeInfoViewDocment();
        private MainDocmentView m_mainview               = new MainDocmentView();
        private IniCfgDlg m_inicfgview = new IniCfgDlg();//动态配置选项
        private RelayProtectForm m_RPview = new RelayProtectForm();//继保定值视图
          
        /////////////生产测试版
        private FunctionListView m_flistview = new FunctionListView();
        private CommunicationTestView m_communicationTestView = new CommunicationTestView();
     


        //////////////////////////////////
        static int ProtoDelayTime = 0;//101规约测试延时重发计时
        static int ProtoDelayNum = 0;//101规约测试延时重发计数
        static int ProtoZZTime = 0;//101规约测试总召重发计时
        static int AlongCallTime = 0;//单招定时器

        //private TransmitDataForm Transmitfm = new TransmitDataForm();

        private PrjLoad loadprj = new PrjLoad();

        private static SerialPort[] comm =new SerialPort[10];

        
        OpenFileDialog OpenF      = new OpenFileDialog();

        private static Socket conn           = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static Socket acceptedSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private static Thread ComThread, NetSendThread, NetRecvThread, ConnectThread, ListenThread, GprsSendThread, GprsRecvThread, UsbThread;
        private static Thread WriteDataThread;
        private static Thread OpenProjectThread;
        private static Thread WriteIniThread;
        static bool WriteDataflag;
        public static bool WriteIniflag = false;
        public static string WriteIniFileName = @"";
        public static byte WriteIniFileType = 0;
        public static byte WriteIniFilek=0;

        static bool OpenPrjflag=false;
        static bool SavePrjflag = false;
        string Openpath = @"";
        string Openpathname = @"";
        FileInfo f;
        DirectoryInfo d;
          
        static int MRxSeq = 0;
        static int MTxSeq = 0;
        static int SRxSeq = 0;
        static int STxSeq = 0;
        static int Retime    = 0;
        static int DelayTime = 0;
        static int LinkType  = 0;
        private static bool NetLinkIsClose = false;
        static int Savetime = 0;
        static int ConConectTime = 0;
        static bool OpenPjOK = false;//打开工程成功


//usb
        USBCmd test = new USBCmd();    
        const uint CH375_DEVICE_ARRIVAL = 3;		    // 设备插入事件,已经插入
        const uint CH375_DEVICE_REMOVE_PEND = 1;		// 设备将要拔出
        const uint CH375_DEVICE_REMOVE = 0;		        // 设备拔出事件,已经拔出
        bool Open = false;
        ulong ID;
        ulong PID;
        ulong VID;
        ReadWriteReturnType wrt;
        ReadWriteReturnType rrt;
        private static  mPCH375_NOTIFY_ROUTINE myCallBack;

        public Form1()
        {
            USBCmd test = new USBCmd();    
            InitializeComponent();
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            //USB
            he = new HasEvent();
           he.SampleEvent +=
                       new EventHandler<MyEventArgs>(SampleEventHandler);

          
           myCallBack = new mPCH375_NOTIFY_ROUTINE(Form1.NotifyRoutine);
            byte[] iDeviceID = null;   // 这里监视任何CH372设备
           test.SetDeviceNotify(0, iDeviceID, myCallBack);
            
        }



 
               public static bool NotifyRoutine(uint iEventStatus)
        {
            USBCmd test = new USBCmd();
            Form1 form = new Form1();
            if (iEventStatus == CH375_DEVICE_ARRIVAL)
            {   //检测到设备插入事件
                MessageBox.Show("检测到有CH372/CH375设备已插入", "信息", MessageBoxButtons.OK);


                form.toolStripStatusLabel6.Text = "检测到有CH372/CH375设备已插入";        //这里也可以实现，但是在VB和VC环境里，在回调函数里直接操作窗体的话，会造成死机
                //  he.DemoEvent(form.toolStripStatusLabel6, "检测到有CH372/CH375设备已插入");  //本人水平有限,对C#不是很熟，这两种方法有什么不妥，或者还有其他方法，还请赐教，本人邮件 zyw@wch.cn

            }
            else if (iEventStatus == CH375_DEVICE_REMOVE)
            {   //检测到设备拔出事件
                form.toolStripStatusLabel6.Text = "检测到有CH372/CH375设备已拔出";
                MessageBox.Show("检测到有CH372/CH375设备已拔出", "信息", MessageBoxButtons.OK);
                test.Close(0);  //关闭设备
                //   he.DemoEvent(form.toolStripStatusLabel6, "检测到有CH372/CH375设备已拔出");


            }
            return true;
        }

        /// <summary>
        /// 窗体的初始化工作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = DateTime.Now.ToString();                 //将时间信息显示到 第2个状态栏上
            toolStripStatusLabel4.Text = PublicDataClass.Manger;
            //m_celpointview.Show(dockPanel, DockState.DockRightAutoHide);

            PublicDataClass._ProtocoltyFlag.NetProFlag = 1;//默认规约
            PublicDataClass._ProtocoltyFlag.ComProFlag = 1;//默认规约
              /// 生产版维护软件修改，开始全屏蔽 
            if (PublicDataClass.LoginType == 1)
            {
                m_productionfunview.Show(dockPanel);
            }
            else
            {
                m_toolbox.Show(dockPanel);
                m_flistview.Show(dockPanel);
               
            }
            
            m_outputWindow.Show(dockPanel);
            
            //m_docview.Show(dockPanel);
            m_mainview.Show(dockPanel);


          
            /// 生产版维护软件修改，开始全屏蔽 
            if (PublicDataClass.LoginType == 1)
            {
                TongXunStripButton.Enabled = false;
                CloseLinkStripButton.Enabled = false;
                JSButton.Enabled = false;
                CallTypeButton.Enabled = false;
                AddStripButton.Enabled = false;
                TransMitButton.Enabled = false;
                FrameButton.Enabled = false;
                NewProjectButton.Enabled = false;
                NewPrjMenuItem.Enabled = false;
            }
            else 
            {
             NewProjectButton.Enabled = true;
             NewPrjMenuItem.Enabled = true;
            }
           
            OpenProjectButton.Enabled = true;
            SaveProjectButton.Enabled = false;
            toolStripButton3.Enabled = false ;
            OpenPrjMenuItem.Enabled = true;          
            SavePrjMenuItem.Enabled = false;

            //string path=
            //if()
              
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            
            if (persistString == typeof(ToolBox).ToString())
                return m_toolbox;
            else if (persistString == typeof(OutputWindows).ToString())
                return m_outputWindow;
            else if (persistString == typeof(FunctionListView).ToString())
                return m_flistview;
         
                
            else if (persistString == typeof(DocumentView).ToString())
                return m_docview;
            else if (persistString == typeof(CeLPointListViewForm).ToString())
                return m_celpointview;
            else if (persistString == typeof(RealTDataDocmentView).ToString())
                return m_realtdataview;
            //else if (persistString == typeof(SysPDocmentView).ToString())
            //    return m_syspview;
            else if (persistString == typeof(CallDataDocmentView).ToString())
                return m_calldview;
            else if (persistString == typeof(HistoryDataDocmemtView).ToString())
                return m_historydataview;
            else if (persistString == typeof(AcquisitionDocmentView).ToString())
                return m_acquisitionview;
            else if (persistString == typeof(XMLCfg ).ToString())
                return m_xmlcfgview ;
            else if (persistString == typeof(ControlDocmentView).ToString())
                return m_controlview;
            else if (persistString == typeof(OtherTypeDocmentView).ToString())
                return m_otherview;
            else if (persistString == typeof(RoleManagerDocmentForm).ToString())
                return m_roleview;
            else if (persistString == typeof(GraphicDocmentViewForm).ToString())
                return m_graphicview;
            else if (persistString == typeof(ChangeInfoViewDocment).ToString())
                return m_changview;
            else if (persistString == typeof(MainDocmentView).ToString())
                return m_mainview;
                ///////////////生产测试版//////////////////
            else if (persistString == typeof(ProductionVersionFun).ToString())
                return m_productionfunview;
            else if (persistString == typeof(CommunicationTestView).ToString())
                return m_communicationTestView;
                 


                /////////////////////////////////////
            else
            {
                // DummyDoc overrides GetPersistString to add extra information into persistString.
                // Any DockContent may override this value to add any needed information for deserialization.

                /*string[] parsedStrings = persistString.Split(new char[] { ',' });
                if (parsedStrings.Length != 3)
                    return null;

                if (parsedStrings[0] != typeof(DummyDoc).ToString())
                    return null;

                DummyDoc dummyDoc = new DummyDoc();
                if (parsedStrings[1] != string.Empty)
                    dummyDoc.FileName = parsedStrings[1];
                if (parsedStrings[2] != string.Empty)
                    dummyDoc.Text = parsedStrings[2];

                return dummyDoc;*/
                return null;
            }
        }
        /********************************************************************************************
        *  函数名：    timer1_Tick                                                                  *
        *  功能  ：    定时器1的处理函数 主要更新时间到状态栏上                                     *
        *  参数  ：    无                                                                           *
        *  返回值：    无                                                                           *
        *  修改日期：  2010-12-07                                                                   *
        *  作者    ：  cuibj                                                                        *
        * ******************************************************************************************/
        private void timer1_Tick(object sender, EventArgs e)
        {

            toolStripStatusLabel2.Text = DateTime.Now.ToString();                 //将时间信息显示到 第2个状态栏上
            if (PublicDataClass._Message.LinkInfo == true)
            {
                PublicDataClass._Message.LinkInfo = false;
                if( LinkType ==1)
                    toolStripStatusLabel6.Text = "[" + PublicDataClass.IPADDR + "：" + PublicDataClass.PORT + "]" + "已连接";
                if (LinkType == 2)
                    toolStripStatusLabel6.Text = "[" + PublicDataClass.ClientInfo + "]" + "已连接";
                if (LinkType == 3)
                {
                    toolStripStatusLabel6.Text = "[" + PublicDataClass.IPADDR + "：" + PublicDataClass.PORT + "]" + "已断开";
                 
                }
                if (LinkType == 4)
                    toolStripStatusLabel6.Text = "[" + PublicDataClass.IPADDR + "：" + PublicDataClass.PORT + "]" + "连接中";
      
                //if (PublicDataClass._CallType.NetTy == 1)
                //{
                //    PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_F = true;
                //    PublicDataClass._FrameTime.LoopTime = 300;
                //    CloseLinkStripButton.Enabled = true;
                //    TongXunStripButton.Enabled = false;
                //}

            }
            if (NetLinkIsClose == true)
            {
                NetLinkIsClose = false;
                if (PublicDataClass._CallType.NetTy == 1) //循环
                {
                    DelayTime = 0;
                    if (ConnectThread != null)
                        ConnectThread.Abort();
                    conn = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    ConnectThread = new Thread(new ThreadStart(ConnThreadSendProc));
                    ConnectThread.Start();
                }
                else
                {
                    //if (ConnectThread != null)
                    //    ConnectThread.Abort();
                    ////conn = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    //ConnectThread = new Thread(new ThreadStart(ConnThreadSendProc));
                    //ConnectThread.Start();

                    //if (NetSendThread != null)
                    //    NetSendThread.Abort();
                    //if (NetRecvThread != null)
                    //    NetRecvThread.Abort();
                    //if (ConnectThread != null)
                    //    ConnectThread.Abort();
                    //timer4.Enabled = false;
                    //conn.Close();

             

                    //timer4.Enabled = true;
                    if (NetSendThread != null)
                        NetSendThread.Abort();
                    if (NetRecvThread != null)
                        NetRecvThread.Abort();
                    if (ConnectThread != null)
                        ConnectThread.Abort();
                    if (ComThread != null)
                    {
                        ComThread.Abort();
                        timer6fram.Enabled = false ;//关串口发定时器
                    }
                    if (ListenThread != null)
                        ListenThread.Abort();
                    if (GprsSendThread != null)
                        GprsSendThread.Abort();
                    if (GprsRecvThread != null)
                        GprsRecvThread.Abort();

                    conn.Close();
                    acceptedSocket.Close();



                    //PublicDataClass.TcpLinkType = 1;
                    //PublicDataClass._Message.TcpLinkState = true;
                    LinkType = 3;
                    PublicDataClass._Message.LinkInfo = true;

                    //CloseLinkStripButton.Enabled = false;
                    //TongXunStripButton.Enabled = true;
                    //断开后重新连接
                    conn = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    ConnectThread = new Thread(new ThreadStart(ConnThreadSendProc));
                    ConnectThread.Start();

                    NetSendThread = new Thread(new ThreadStart(NetThreadSendProc));
                    NetSendThread.Start();

                    NetRecvThread = new Thread(new ThreadStart(ReceiveNetMsg));
                    NetRecvThread.Start();
                }
            }
        }
       

        /********************************************************************************************
        *  函数名：    NewProjectButton_Click                                                       *
        *  功能  ：    新建工程事件处理函数                                                         *
        *  参数  ：    无                                                                           *
        *  返回值：    无                                                                           *
        *  修改日期：  2011-3-10                                                                    *
        *  作者    ：  cuibj                                                                        *
        * ******************************************************************************************/
        private void NewProjectButton_Click(object sender, EventArgs e)
        {
            NewPrjform.ShowDialog();
            if (NewPrjform.DialogResult == DialogResult.OK)               //判断是否按下-确定-按钮
            {
                PublicDataClass._ChangeFlag.ToolBoxTreeNeedUpdate     = true;
                PublicDataClass._ChangeFlag.FunlistViewTreeNeedUpdate = true;
                PublicDataClass.PrjType   = 1;
                NewProjectButton.Enabled  = false;
                OpenProjectButton.Enabled = false;
                SaveProjectButton.Enabled = true;
                //InitGlobalData.InitAllGlobalIniValue();                //初始化全局ini配置文件
                SaveProjectButton.Enabled = true;
            }
        }
        /// 获取系统串口的数目
        
      
        /*
        public static string[] GetCommKeys()
        {
            string[] values = null;
            try
            {
                RegistryKey hklm = Registry.LocalMachine;
                RegistryKey hs = hklm.OpenSubKey("HARDWARE\\DEVICEMAP\\SERIALCOMM",true);
                //Console.WriteLine(hs.ValueCount.ToString());
                values = new string[hs.ValueCount];
                for (int i = 0; i < hs.ValueCount; i++)
                {
                    values[i] = hs.GetValue(hs.GetValueNames()[i]).ToString();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            return values;
        }*/
        
        /// <summary>
        /// 配置视图-菜单的 事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CfgMenuItem_Click(object sender, EventArgs e)
        {
            
            m_toolbox.Show(dockPanel);
            
        }
        /// <summary>
        ///  功能视图-菜单的 事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FunctionMenuItem_Click(object sender, EventArgs e)
        {
            m_flistview.Show(dockPanel);
        }
        /// <summary>
        /// 输出视图-菜单的 事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutputMenuItem_Click(object sender, EventArgs e)
        {
            m_outputWindow.Show(dockPanel);
        }

        /********************************************************************************************
       *  函数名：    Form_SizeChanged                                                             *
       *  功能  ：    窗体大小改变事件处理函数                                                     *
       *  参数  ：    无                                                                           *
       *  返回值：    无                                                                           *
       *  修改日期：  2010-12-07                                                                   *
       *  作者    ：  cuibj                                                                        *
       * ******************************************************************************************/
        private void Form_SizeChanged(object sender, EventArgs e)           //主窗体尺寸大小发生改变的事件处理函数
        {
            if (this.WindowState == FormWindowState.Minimized)              //判断主窗体此时的状态
            {
                //this.notifyIcon1.Visible = true;                            //托盘图标可见
                //this.ShowInTaskbar = false;                           //状态栏不可见

            }
        }

        /********************************************************************************************
        *  函数名：    NotifyIcon_MouseClick                                                        *
        *  功能  ：    图标单击事件处理函数                                                         *
        *  参数  ：    无                                                                           *
        *  返回值：    无                                                                           *
        *  修改日期：  2010-12-07                                                                   *
        *  作者    ：  cuibj                                                                        *
        * ******************************************************************************************/
        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e) //托盘图标的鼠标单击响应处理函数
        {
            if (this.WindowState == FormWindowState.Minimized)              //判断主窗体的状态
            {
                this.WindowState = FormWindowState.Maximized;

            }
            
            //this.Activate();                                               //激活主窗体
            m_docview.Show(dockPanel);
            this.notifyIcon1.Visible = false;                              //托盘图标显示属性=不可见
            this.ShowInTaskbar = true;                               //状态栏 =可见

        }
        /********************************************************************************************
        *  函数名：    ExitSystemButton_Click                                                       *
        *  功能  ：    退出系统事件处理函数                                                         *
        *  参数  ：    无                                                                           *
        *  返回值：    无                                                                           *
        *  修改日期：  2010-12-07                                                                   *
        *  作者    ：  cuibj                                                                        *
        * ******************************************************************************************/
        private void ExitSystemButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要退出配网自动化测试系统吗?", "系统信息",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {

                this.Close();

            }
        }
        /// <summary>
        ///  文档视图-菜单的 事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DocmentMenuItem_Click(object sender, EventArgs e)
        {
            m_docview.Show(dockPanel);
        }
        /********************************************************************************************
        *  函数名：    timer2_Tick                                                                  *
        *  功能  ：    定时器2的处理函数 主要负责窗体的切换显示                                     *
        *  参数  ：    无                                                                           *
        *  返回值：    无                                                                           *
        *  修改日期：  2010-12-07                                                                   *
        *  作者    ：  cuibj                                                                        *
        * ******************************************************************************************/
        private void Time2_Tick(object sender, EventArgs e)
        {
            if (PublicDataClass._ChangeFlag.MoniterUpdate == true)
            {
                PublicDataClass._ChangeFlag.MoniterUpdate = false;
                m_realtdataview.Show(dockPanel);

            }
            //else if (PublicDataClass._ChangeFlag.XtParamUpdate == true)
            //{

            //    PublicDataClass._ChangeFlag.XtParamUpdate = false;
            //    PublicDataClass._ChangeFlag.XtParamUpdate1 = false;  //zxl
            //    m_syspview.Show(dockPanel);
            //}
            else if (PublicDataClass._ChangeFlag.CallDataUpdate == true)
            {

                PublicDataClass._ChangeFlag.CallDataUpdate = false;
                m_calldview.Show(dockPanel);
            }
            else if (PublicDataClass._ChangeFlag.HistoryDataUpdate == true)
            {

                PublicDataClass._ChangeFlag.HistoryDataUpdate = false;
                m_historydataview.Show(dockPanel);
            }
            else if (PublicDataClass._ChangeFlag.AcquisitionParamUpdate == true)
            {

                PublicDataClass._ChangeFlag.AcquisitionParamUpdate = false;
                PublicDataClass._ChangeFlag.AcquisitionParamUpdate1 = false;  //zxl
                m_acquisitionview.Show(dockPanel); 
            }

            else if (PublicDataClass._ChangeFlag.ControlViewUpdate == true)
            {

                PublicDataClass._ChangeFlag.ControlViewUpdate = false;
                PublicDataClass._ChangeFlag.ControlViewUpdate1 = false;  //zxl
                m_controlview.Show(dockPanel);
            }
            else if (PublicDataClass._ChangeFlag.OtherTypeViewUpdate == true)
            {

                PublicDataClass._ChangeFlag.OtherTypeViewUpdate = false;
                PublicDataClass._ChangeFlag.OtherTypeViewUpdate = false;  //zxl
                m_otherview.Show(dockPanel);
            }
            else if (PublicDataClass._ChangeFlag.XieCPU == true)
            {

                PublicDataClass._ChangeFlag.XieCPU = false;

                m_xiecpuview.Show(dockPanel);
            }
            else if (PublicDataClass._ChangeFlag.RoleMangerViewUpdate == true)
            {
                PublicDataClass._ChangeFlag.RoleMangerViewUpdate = false;
                m_roleview.Show(dockPanel);

            }
            else if (PublicDataClass._ChangeFlag.ChangInfoViewUpdate == true)
            {
                PublicDataClass._ChangeFlag.ChangInfoViewUpdate = false;
                //m_changview.Show(dockPanel);
                

            }
            else if (PublicDataClass._ChangeFlag.DocmentViewUpdate == true)
            {
                PublicDataClass._ChangeFlag.DocmentViewUpdate = false;
                m_docview.Show(dockPanel);
            }

                ///////////////////////生产测试版/////////////////////////
            else if (PublicDataClass._ChangeFlag.CommunicationTestUpdate == true)
            {
                PublicDataClass._ChangeFlag.CommunicationTestUpdate = false;
                m_communicationTestView.Show(dockPanel);
            }



                ////////////////////////////////////////////////
            if (PublicDataClass._OpenLinkState.HaveNet == true)
            {
                if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)//当网口规约104时
                {
                    //远程升级时为防止升级被打断暂停测试确认帧
                    if (PublicDataClass._ChangeFlag.CodeUpdateFlag == true)
                        timer4.Enabled = false;
                    else
                        timer4.Enabled = true;
                }
            }

            ////连接断开时重新建立连接
            //if (PublicDataClass._ChangeFlag.ContinueConnectFlag == true)
            //{
            //    PublicDataClass._ChangeFlag.ContinueConnectFlag = false;

            //    //if (ConConectTime <= 4)
            //    {
            //        ConConectTime++;
            //        if (NetSendThread != null)
            //            NetSendThread.Abort();
            //        if (NetRecvThread != null)
            //            NetRecvThread.Abort();
            //        if (ConnectThread != null)
            //            ConnectThread.Abort();
            //        ConnectThread = new Thread(new ThreadStart(ConnThreadSendProc));
            //        ConnectThread.Start();

            //        NetSendThread = new Thread(new ThreadStart(NetThreadSendProc));
            //        NetSendThread.Start();

            //        NetRecvThread = new Thread(new ThreadStart(ReceiveNetMsg));
            //        NetRecvThread.Start();


            //    }




            //}

            if (PublicDataClass._ChangeFlag.AlongCall == true)
            {
                PublicDataClass._ChangeFlag.AlongCall = false;
                PublicDataClass._ProtocoltyFlag.AloneCallYxYcFlag = true;
                timer6AlongCall.Enabled = true;
                //if (PublicDataClass._ProtocoltyFlag .ComProFlag ==1)
                //timer6fram.Enabled = false;
            }

        }
        /********************************************************************************************
        *  函数名：    TongXunStripButton_Click                                                     *
        *  功能  ：    通讯连接事件处理函数                                                         *
        *  参数  ：    无                                                                           *
        *  返回值：    无                                                                           *
        *  修改日期：  2010-12-07                                                                   *
        *  作者    ：  cuibj                                                                        *
        * ******************************************************************************************/
        private void TongXunStripButton_Click(object sender, EventArgs e)
        {
      
            OpenLinkForm1 Openfm = new OpenLinkForm1();
            Openfm.ShowDialog();
            if (Openfm.DialogResult == DialogResult.OK)               //判断是否按下-确定-按钮
            {
                for(byte i=0;i<PublicDataClass._OpenLinkState.Linknum;i++)
                {
                    for (byte k = 0; k < PublicDataClass.SaveText.channelnum; k++)
                    {

                        if (PublicDataClass._OpenLinkState.LinkDevName[i] == PublicDataClass.SaveText.Channel[k].ChannelID)
                        {
                            Regex cm = new Regex("COM"); Regex nt = new Regex("NET"); Regex gs = new Regex("GPRS"); Regex ub = new Regex("USB");
                            Match m = cm.Match(PublicDataClass.SaveText.Channel[k].ChannelID);
                            if (m.Success)
                            {
                                ProcessSerial(PublicDataClass.SaveText.Channel[k].ChannelID,
                                           PublicDataClass.SaveText.Channel[k].baud, PublicDataClass.SaveText.Channel[k].jy);   //串口处理
                                PublicDataClass._OpenLinkState.HaveCom = true;
                                PublicDataClass._Time.NetDelayTime = Convert.ToInt16(PublicDataClass.SaveText.Channel[k].RelayTime);//串口走104时使用该值
                            }
                            m = nt.Match(PublicDataClass.SaveText.Channel[k].ChannelID);
                            if (m.Success)
                            {
                                //ProcessNet(PublicDataClass.SaveText.Channel[i].IP,PublicDataClass.SaveText.Channel[i].port);     //网口处理

                                PublicDataClass.IPADDR = PublicDataClass.SaveText.Channel[k].IP;
                                PublicDataClass.PORT = PublicDataClass.SaveText.Channel[k].port;
                                PublicDataClass._Time.NetDelayTime = Convert.ToInt16(PublicDataClass.SaveText.Channel[k].RelayTime);
                                PublicDataClass._OpenLinkState.HaveNet = true;
                            }
                            m = gs.Match(PublicDataClass.SaveText.Channel[k].ChannelID);
                            if (m.Success)
                            {
                                //ProcessGPRS();   //GPRS处理
                                PublicDataClass.PORT = PublicDataClass.SaveText.Channel[k].port;
                                PublicDataClass.IPADDR = PublicDataClass.SaveText.Channel[k].IP; 
                                PublicDataClass._OpenLinkState.HaveGprs = true;
                            }
                            m = ub.Match(PublicDataClass.SaveText.Channel[k].ChannelID);
                            if (m.Success)
                            {
                                ProcessUsb();   //Usb处理
                              
                                PublicDataClass._OpenLinkState.HaveUsb = true;
                            }
                        }
                    }
                }
                TongXunStripButton.Enabled = false;     //使按钮不可用
                CloseLinkStripButton.Enabled = true;
                PublicDataClass._NetTaskFlag.FirstON_S = false;
                PublicDataClass._ComTaskFlag.FirstON_S = false;
                PublicDataClass._Message.LinkUpdata = true;//通道更新
             }
            if (PublicDataClass._OpenLinkState.HaveCom == true)   //有串口的配置
            {
                ComThread = new Thread(new ThreadStart(SerialThreadProc));
                ComThread.Start();
                timer6fram.Enabled = true;//开串口发定时器
                if (PublicDataClass._ProtocoltyFlag.ComProFlag == 2)//当串口规约104时
                {
                    timer4.Enabled = true;
                    PublicDataClass._ComTaskFlag.C_RQ_NA_LINKREQ_F = true;
                    PublicDataClass.ChanelId = 2;
                }
                else 
                PublicDataClass.ChanelId = 1;
            }
            if (PublicDataClass._OpenLinkState.HaveNet == true)
            {

                //ProcessNet(PublicDataClass.IPADDR, PublicDataClass.PORT);
                conn = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ConnectThread = new Thread(new ThreadStart(ConnThreadSendProc));
                ConnectThread.Start();

                NetSendThread = new Thread(new ThreadStart(NetThreadSendProc));
                NetSendThread.Start();

                NetRecvThread = new Thread(new ThreadStart(ReceiveNetMsg));
                NetRecvThread.Start();
                if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)//当网口规约104时
                {
                    timer4.Enabled = true;
                    PublicDataClass.ChanelId = 2;
                }
                else 
                PublicDataClass.ChanelId = 1;//跟参数显示有关
            }
            if (PublicDataClass._OpenLinkState.HaveGprs == true)
            {
                conn = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPAddress HostIP = IPAddress.Parse(PublicDataClass.IPADDR);
                IPEndPoint point = new IPEndPoint(HostIP, Int32.Parse(PublicDataClass.PORT));
                conn.Bind(point);
                conn.Listen(8888);

                ListenThread = new Thread(new ThreadStart(ListenhreadSendProc));
                ListenThread.Start();

                GprsSendThread = new Thread(new ThreadStart(GprsThreadSendProc));
                GprsSendThread.Start();

                GprsRecvThread = new Thread(new ThreadStart(ReceiveGprsMsg));
                GprsRecvThread.Start();
       

            }
            if (PublicDataClass._OpenLinkState.HaveUsb == true)   //有USB的配置
            {
               
                UsbThread = new Thread(new ThreadStart(UsbThreadProc));
                UsbThread.Start();

                PublicDataClass.ChanelId = 3;
            }
        }
        /********************************************************************************************
        *  函数名：    CloseLinkStripButton_Click                                                   *
        *  功能  ：    设备关闭事件处理函数                                                         *
        *  参数  ：    无                                                                           *
        *  返回值：    无                                                                           *
        *  修改日期：  2010-12-07                                                                   *
        *  作者    ：  cuibj                                                                        *
        * ******************************************************************************************/
        private void CloseLinkStripButton_Click(object sender, EventArgs e)
        {
       
            if (PublicDataClass._OpenLinkState.Linknum == 0)
            {

                MessageBox.Show("没有任何通道处于连接状态！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (NetSendThread != null)
                NetSendThread.Abort();
            if (NetRecvThread != null)
                NetRecvThread.Abort();
             if (ConnectThread != null)
                ConnectThread.Abort();
             if (ComThread != null)
             {
                 ComThread.Abort();
                 timer6fram.Enabled = false ;//关串口发定时器
             }
            if (ListenThread != null)
                ListenThread.Abort();
            if (GprsSendThread != null)
                GprsSendThread.Abort();
            if (GprsRecvThread != null)
                GprsRecvThread.Abort();
            if (  UsbThread != null)
                  UsbThread.Abort();

            PublicDataClass.TcpLinkType = 0;
          
            for (byte i = 0; i < 10; i++)
            {
                if (PublicDataClass._ComState.IsUse[i] == true)
                {
                    PublicDataClass._ComState.IsUse[i] = false;
                    comm[i].Close();

                }
            }
            LinkType = 3;
            PublicDataClass._Message.LinkInfo = true;
            timer4.Enabled = false;
            conn.Close();
            acceptedSocket.Close();
            test.Close(0);  //关闭设备
            Open = false;
            CloseLinkStripButton.Enabled = false;
            TongXunStripButton.Enabled   = true;
     
            PublicDataClass._OpenLinkState.HaveNet  = false;
            PublicDataClass._OpenLinkState.HaveCom  = false;
            PublicDataClass._OpenLinkState.HaveGprs = false;
        }
        /*****************************************************************************************************************************
       *                                                          通讯口操作                                                        *
       *                                                                                                                            *
       *  功能  ：    通讯口处理                                                                                                    *
       *  参数  ：    无                                                                                                            *
       *  返回值：    无                                                                                                            *
       *  修改日期：  2011-04-07                                                                                                    *
       *  作者    ：  cuibj                                                                                                         *
       * ***************************************************************************************************************************/
        private void ProcessSerial(string ComID,string baud,string jy)
        {
            PublicDataClass.COMID = ComID;
            
            byte id = Convert.ToByte(ComID.Remove(0, 3));
            comm[id - 1] = new SerialPort();
            comm[id - 1].PortName = ComID;
            comm[id - 1].BaudRate = Convert.ToInt32(baud);
            if(jy =="奇")
               comm[id - 1].Parity = Parity.Odd;
            else if (jy == "偶")
                comm[id - 1].Parity = Parity.Even;
            else if (jy == "无")
                comm[id - 1].Parity = Parity.None;
            comm[id - 1].DataBits = 8;

            try
            {
                comm[id-1].Open();
                toolStripStatusLabel6.Text = PublicDataClass.COMID +"已打开";
                PublicDataClass._ComState.IsUse[id - 1] = true;
            }
            catch (IOException ee)                  //异常 计算机无此串口
            {
                PublicDataClass._ComState.IsUse[id - 1] = false;
                MessageBox.Show(ee.Message,"信息",MessageBoxButtons.OK, MessageBoxIcon.Information);
                    


            }
            catch (UnauthorizedAccessException ee)  //异常 串口被占用
            {
                PublicDataClass._ComState.IsUse[id - 1] = false;
                MessageBox.Show(ee.Message, "信息",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            

        }
        private static void ProcessNet(string ipaddr,string  port)
        {
            try
            {
                //PublicDataClass.IPADDR =ipaddr;
                //PublicDataClass.PORT = port;

                conn.Connect(IPAddress.Parse(ipaddr), Convert.ToInt16(port));
                PublicDataClass.TcpLinkType = 2;
                PublicDataClass._Message.TcpLinkState = true;
                ConnectThread.Abort();

            }
            catch
            {
               
                PublicDataClass.TcpLinkType = 1;
                PublicDataClass._Message.TcpLinkState = true;

            }
           
        }

        private void ProcessUsb()  //Usb处理
         {
         
                          
                Open = test.Open(0);  //打开设备
                if (Open == true)
                {              
                    toolStripStatusLabel6.Text =  "USB已打开";
              //      PublicDataClass._ComState.IsUse[id - 1] = true;
                   
                   ID = test.GetID(0);  //读取设备id和厂商id
                    VID = ID & 0xFFFF;
                    PID = (ID >> 16) & 0xFFFF;
                    toolStripStatusLabel6.Text = "USB设备PID为：" + PID.ToString("X") + "   " + "VID为：" + VID.ToString("X");
                    
                    
                }
                else
                {
                    toolStripStatusLabel6.Text = "设备打开失败";
                }
         
         }

        /*****************************************************************************************************************************
       *                                                          线程操作                                                          *
       *                                                                                                                            *
       *  功能  ：    线程任务处理                                                                                                  *
       *  参数  ：    无                                                                                                            *
       *  返回值：    无                                                                                                            *
       *  修改日期：  2010-12-07                                                                                                    *
       *  作者    ：  cuibj                                                                                                         *
       * ***************************************************************************************************************************/

        public static void SerialThreadProc()   //串口线程
        {
            int ReadLenNew = 0;
            int ReadLenOld = 0;
            byte tick = 0;
            while (true)       //处理事物
            {

                for (byte i = 0; i < 10; i++)
                {
                    if (PublicDataClass._ComState.IsUse[i] == true)
                    {
                          //  PtlComFrameProc(); // 是否有发送任务处理
                            if ((PublicDataClass._ComStructData.TX_TASK)&&(PublicDataClass._Message.ComShowRecvTextData==false))
                            {
                                PublicDataClass._ComStructData.TX_TASK = false;
                                comm[i].Write(PublicDataClass._ComStructData.TXBuffer, 0, PublicDataClass._ComStructData.TxLen);
                                PublicDataClass._Message.ComShowSendTextData = true;
                                PublicDataClass._Message.ParamAck = false;
                            }

                            if (PublicDataClass._Message.ComShowSendTextData == false)
                            {
                                ReadLenNew = comm[i].BytesToRead;
                                if (ReadLenNew > 0)
                                {
                                    if (ReadLenNew != ReadLenOld)
                                        tick = 0;
                                    else
                                    {
                                        tick++;
                                        if (tick > 10) //一侦接收完成
                                        {
                                            tick = 0;
                                            PublicDataClass._ComStructData.RxLen = comm[i].Read(PublicDataClass._ComStructData.RXBuffer, 0, ReadLenNew);
                                            ReadLenNew = 0;

                                            ParseComReceData();
                                            if (PublicDataClass._ProtocoltyFlag.ComProFlag == 2)//串口104规约
                                                PublicDataClass._FrameTime.T3 = 20;
                                            else if (PublicDataClass._ProtocoltyFlag.ComProFlag == 1)//串口101规约
                                                ProtoDelayTime = PublicDataClass._ProtocoltyFlag.DelayTime;
                                            PublicDataClass._Message.ComShowRecvTextData = true;
                                            
                                        }
                                    }
                                    ReadLenOld = ReadLenNew;
                                }
                            }

                    }
                }
             
                Thread.Sleep(1);
            }


        }
        //public static void NetThreadSendProc()   //网络发送数据线程
               private void  NetThreadSendProc()   //网络发送数据线程
        {
            //NetworkStream streamToClent = tclient.GetStream();
            while (true)    //处理事物
            {

                if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)//网口走101
                {
                    //网口任务转换为串口任务
                    TTaskNetToCom();
                    //采用串口组包函数
                    PtlComFrameProc();
                    //串口发送任务改为网口发送任务，发送buffer转换
                    TBufferComToNet();
                }
                else
                    PtlNetFrameProc(); // 是否有发送任务处理
                if (PublicDataClass._Message.NetShowRecvTextData == false)
                {
                    if (PublicDataClass._NetStructData.TX_TASK)
                    {

                        PublicDataClass._NetStructData.TX_TASK = false;

                        try
                        {

                            conn.Send(PublicDataClass._NetStructData.NetTBuffer, 0, PublicDataClass._NetStructData.NetTLen, 0);
                            PublicDataClass._Message.NetShowSendTextData = true;
                            PublicDataClass._Message.ParamAck = false;
                           //  PublicDataClass._FrameTime.T_NetSend = 2000;//
                            
                        }
                        catch
                        {
                            //if (PublicDataClass.TcpLinkType == 2)
                            //    NetLinkIsClose = true;
                            //MessageBox.Show("网络已断开连接");

                        }

                        PublicDataClass._Message.NetShowSendOver = true;
                    }

                    if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)//网口走101，设置帧间隔为2秒
                    {
 
                        Thread.Sleep(2000);
                    }
                    else
                        Thread.Sleep(1);

                }
            
            }


        }


        /*****************************************************************************************************************************
  *                                                          网络的打包解包 连接 操作                                          *
  *                                                                                                                            *
  *  功能  ：    数据帧类型处理                                                                                                *
  *  参数  ：    无                                                                                                            *
  *  返回值：    无                                                                                                            *
  *  修改日期：  2010-12-07                                                                                                    *
  *  作者    ：  cuibj                                                                                                         *
  * ***************************************************************************************************************************/
        public static void PtlNetFrameProc()  //---------------------------------------------网络组包
        {
     

            if (!PublicDataClass._NetTaskFlag.FirstON_S)
            {
                //	第一次开机处理
                PublicDataClass._NetTaskFlag.FirstON_S = true;		//	主机开机处理过标志(正常情况下该标志一直为1)
                PublicDataClass._NetTaskFlag.C_P1_NA_F = false;     //104U格式中STARTDT命令
                PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_F = false;
               if (PublicDataClass._ProtocoltyFlag.ComProFlag == 2)
                   PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_F = true ;//串口走104第一次开机复位
                return;
            }
            else if (PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_F == true)  //启动传输
            {
                PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_F = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                MTxSeq = 0;
                MRxSeq = 0;
                //PublicDataClass._NetStructData.NetTLen = EncodeFrame(1, 0, ref PublicDataClass._NetStructData.NetTBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                         0, 0, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);
 //               if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1 || PublicDataClass._ProtocoltyFlag.ComProFlag == 2)
                {
                    PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(1);
                    PublicDataClass.SedNetFrameMsg = "复位";

                }
                //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                //{
                //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(1);
                //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                //    PublicDataClass.SedNetFrameMsg = "请求链路状态";
                //    ProtoDelayTime = PublicDataClass._ProtocoltyFlag.DelayTime;
                //}
               
                PublicDataClass._NetStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.NetThreadID = 1;
                //DelayTime = PublicDataClass._Time.NetDelayTime;
                PublicDataClass._FrameTime.T1 = 15;
                PublicDataClass.TxSeq = 0;
                PublicDataClass.RxSeq = 0;

            }
            //else if (PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_F1 == true)  //复位链路101
            //{
            //    PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_F1 = false;
            //    PublicDataClass._NetStructData.NetTLen = 0;
            //    MTxSeq = 0;
            //    MRxSeq = 0;
    
            //   if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
            //    {
            //        PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(2);
            //        for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
            //            PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
            //        PublicDataClass.SedNetFrameMsg = "复位链路";
            //        ProtoDelayTime = PublicDataClass._ProtocoltyFlag.DelayTime;
            //    }

            //    PublicDataClass._NetStructData.TX_TASK = true;
            //    PublicDataClass._ThreadIndex.NetThreadID = 1;
            //    DelayTime = PublicDataClass._Time.NetDelayTime;
            //    PublicDataClass._FrameTime.T1 = 15;
            //    PublicDataClass.TxSeq = 0;
            //    PublicDataClass.RxSeq = 0;

            //}


            else if (PublicDataClass._NetTaskFlag.C_IC_NA_1 == true)  //总召唤
            {
                PublicDataClass._NetTaskFlag.C_IC_NA_1 = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                //PublicDataClass._NetStructData.NetTLen = EncodeFrame(1, 3, ref PublicDataClass._NetStructData.NetTBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                         0, 0, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;
                //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1 || PublicDataClass._ProtocoltyFlag.ComProFlag == 2)
                PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(10);
                //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                //{
                //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(10);
                //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                //    PublicDataClass._ComStructData.FCB = (!PublicDataClass._ComStructData.FCB);
                //}
                PublicDataClass.SedNetFrameMsg = "总召唤";
                //PublicDataClass._FrameTime.T2 = 0;
                PublicDataClass._NetStructData.TX_TASK = true;
                PublicDataClass._ProtocoltyFlag.ZZFlag = false ;//总召发结束
             
                PublicDataClass._ThreadIndex.NetThreadID = 2;
                DelayTime = PublicDataClass._Time.NetDelayTime;
         
                //ProtoDelayTime = PublicDataClass._ProtocoltyFlag.DelayTime;
                PublicDataClass._ChangeFlag.Clearflag = true;
                PublicDataClass._ChangeFlag.Clearflag1 = true;
                PublicDataClass._ChangeFlag.Clearflag2 = true;
            }

        
            else if (PublicDataClass._NetTaskFlag.SET_PARAM_CON == true)  //设置参数
            {
                PublicDataClass._NetTaskFlag.SET_PARAM_CON = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                PublicDataClass.ZDYtype = 1;

                if (PublicDataClass.Framelen == 231)
                {
                    //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)
                    PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(50);
                    //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                    //{
                    //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(50);
                    //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                    //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                    //}
                }
                else
                    PublicDataClass._NetStructData.NetTLen = protocoltyparam.EncodeFrame(1, PublicDataClass._NetStructData.NetTBuffer, PublicDataClass._DataField.Buffer,
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                          PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);

              /*  if (PublicDataClass.addselect == 5)
                    PublicDataClass._NetStructData.NetTBuffer[12] = 0x00;
                else if (PublicDataClass.addselect == 1)
                    PublicDataClass._NetStructData.NetTBuffer[12] = 0x01;
                else if (PublicDataClass.addselect == 2)
                    PublicDataClass._NetStructData.NetTBuffer[12] = 0x02;
                else if (PublicDataClass.addselect == 3)
                    PublicDataClass._NetStructData.NetTBuffer[12] = 0x03;
                else if (PublicDataClass.addselect == 4)
                    PublicDataClass._NetStructData.NetTBuffer[12] = 0xff;*/

                PublicDataClass.SedNetFrameMsg = "设置";
                PublicDataClass._NetStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.NetThreadID = 3;
                DelayTime = PublicDataClass._Time.NetDelayTime;

            }
            else if (PublicDataClass._NetTaskFlag.SET_RYB_1 == true)  //设置软压板
            {
                PublicDataClass._NetTaskFlag.SET_RYB_1 = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                //PublicDataClass._NetStructData.NetTLen = EncodeFrame(1, 32, ref PublicDataClass._NetStructData.NetTBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                         PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);
    
                PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(45);
      
                PublicDataClass.SedNetFrameMsg = "软压板参数设置";
                PublicDataClass._NetStructData.TX_TASK = true;
                //PublicDataClass._ThreadIndex.NetThreadID = 3;
                //DelayTime = PublicDataClass._Time.NetDelayTime;

            }
            else if (PublicDataClass._NetTaskFlag.READ_RYB_1== true)  //读软压板
            {
                PublicDataClass._NetTaskFlag.READ_RYB_1 = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                //PublicDataClass._NetStructData.NetTLen = EncodeFrame(1, 33, ref PublicDataClass._NetStructData.NetTBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                         PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);
                PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(46);
                PublicDataClass.SedNetFrameMsg = "软压板参数读取";
                PublicDataClass._NetStructData.TX_TASK = true;
                //PublicDataClass._ThreadIndex.NetThreadID = 3;
                //DelayTime = PublicDataClass._Time.NetDelayTime;

            }
            else if (PublicDataClass._NetTaskFlag.C_CS_NA_1 == true)
            {
                PublicDataClass._NetTaskFlag.C_CS_NA_1 = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                //PublicDataClass._NetStructData.NetTLen = EncodeFrame(1, 4, ref PublicDataClass._NetStructData.NetTBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);
                //PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;
                //PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(12);
                PublicDataClass.ZDYtype = 7;
                if (PublicDataClass.Framelen == 231)
                {
                    //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)
                        PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(50);
                    //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                    //{
                    //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(50);
                    //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                    //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                    //}
                }
                else
                    PublicDataClass._NetStructData.NetTLen = protocoltyparam.EncodeFrame(7, PublicDataClass._NetStructData.NetTBuffer, PublicDataClass._DataField.Buffer,
                                                         PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                         PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);
                PublicDataClass.SedNetFrameMsg = "校时";
                PublicDataClass._NetStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.NetThreadID = 4;
                DelayTime = PublicDataClass._Time.NetDelayTime;
            }
            
            else if (PublicDataClass._NetTaskFlag.YK_Sel_1_D == true)
            {
                PublicDataClass._NetTaskFlag.YK_Sel_1_D = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                //PublicDataClass._NetStructData.NetTLen = EncodeFrame(1, 22, ref PublicDataClass._NetStructData.NetTBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.YkStartPos, MTxSeq, MRxSeq);

                //PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;
                //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)
                PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(15);
                //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                //{
                //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(15);
                //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                //    PublicDataClass._ComStructData.FCB = (!PublicDataClass._ComStructData.FCB);
                //}

                if (PublicDataClass.YkState == 1)
                    PublicDataClass.SedNetFrameMsg = "选择<分>";
                else if (PublicDataClass.YkState == 2)
                    PublicDataClass.SedNetFrameMsg = "选择<合>";
                PublicDataClass._NetStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.NetThreadID = 22;
                DelayTime = PublicDataClass._Time.NetDelayTime;
            }
            else if (PublicDataClass._NetTaskFlag.YK_Exe_1_D == true)     //遥控执行
            {
                PublicDataClass._NetTaskFlag.YK_Exe_1_D = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                //PublicDataClass._NetStructData.NetTLen = EncodeFrame(1, 23, ref PublicDataClass._NetStructData.NetTBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.YkStartPos, MTxSeq, MRxSeq);

                //PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;
                //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)
                    PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(15);
                //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                //{
                //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(15);
                //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                //    PublicDataClass._ComStructData.FCB = (!PublicDataClass._ComStructData.FCB);
                //}

                if (PublicDataClass.YkState == 1)
                    PublicDataClass.SedNetFrameMsg = "执行<分>";
                else if (PublicDataClass.YkState == 2)
                    PublicDataClass.SedNetFrameMsg = "执行<合>";
                PublicDataClass._NetStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.NetThreadID = 23;
                DelayTime = PublicDataClass._Time.NetDelayTime;
            }
            else if (PublicDataClass._NetTaskFlag.YK_Cel_1_D == true)     //遥控撤销
            {
                PublicDataClass._NetTaskFlag.YK_Cel_1_D = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                //PublicDataClass._NetStructData.NetTLen = EncodeFrame(1, 23, ref PublicDataClass._NetStructData.NetTBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.YkStartPos, MTxSeq, MRxSeq);

                //PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;
                //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)
                    PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(16);
                //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                //{
                //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(16);
                //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                //    PublicDataClass._ComStructData.FCB = (!PublicDataClass._ComStructData.FCB);
                //}

                if (PublicDataClass.YkState == 1)
                    PublicDataClass.SedNetFrameMsg = "撤销<分>";
                else if (PublicDataClass.YkState == 2)
                    PublicDataClass.SedNetFrameMsg = "撤销<合>";
                PublicDataClass._NetStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.NetThreadID = 23;
                DelayTime = PublicDataClass._Time.NetDelayTime;
            }
            else if (PublicDataClass._NetTaskFlag.YK_Sel_1 == true)     //遥控选择
            {
                PublicDataClass._NetTaskFlag.YK_Sel_1 = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                //PublicDataClass._NetStructData.NetTLen = EncodeFrame(1, 5, ref PublicDataClass._NetStructData.NetTBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.YkStartPos, MTxSeq, MRxSeq);

                //PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;
                //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)
                    PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(17);
                //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                //{
                //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(17);
                //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                //    PublicDataClass._ComStructData.FCB = (!PublicDataClass._ComStructData.FCB);
                //}
                if (PublicDataClass.YkState == 1)
                    PublicDataClass.SedNetFrameMsg = "选择<分>";
                else if (PublicDataClass.YkState == 2)
                    PublicDataClass.SedNetFrameMsg = "选择<合>";
                PublicDataClass._NetStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.NetThreadID = 5;
                DelayTime = PublicDataClass._Time.NetDelayTime;
            }
            else if (PublicDataClass._NetTaskFlag.YK_Exe_1 == true)     //遥控执行
            {
                PublicDataClass._NetTaskFlag.YK_Exe_1 = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                //PublicDataClass._NetStructData.NetTLen = EncodeFrame(1, 6, ref PublicDataClass._NetStructData.NetTBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.YkStartPos, MTxSeq, MRxSeq);

                //PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;
                //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)
                    PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(17);
                //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                //{
                //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(17);
                //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                //    PublicDataClass._ComStructData.FCB = (!PublicDataClass._ComStructData.FCB);
                //}
                if (PublicDataClass.YkState == 1)
                    PublicDataClass.SedNetFrameMsg = "执行<分>";
                else if (PublicDataClass.YkState == 2)
                    PublicDataClass.SedNetFrameMsg = "执行<合>";
                PublicDataClass._NetStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.NetThreadID = 6;
                DelayTime = PublicDataClass._Time.NetDelayTime;
            }
            else if (PublicDataClass._NetTaskFlag.YK_Cel_1 == true)     //遥控撤销
            {
                PublicDataClass._NetTaskFlag.YK_Cel_1 = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                //PublicDataClass._NetStructData.NetTLen = EncodeFrame(1, 7, ref PublicDataClass._NetStructData.NetTBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.YkStartPos, MTxSeq, MRxSeq);

                //PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;
                //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)
                    PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(18);
                //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                //{
                //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(18);
                //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                //    PublicDataClass._ComStructData.FCB = (!PublicDataClass._ComStructData.FCB);
                //}

                if (PublicDataClass.YkState == 1)
                    PublicDataClass.SedNetFrameMsg = "撤销<分>";
                else if (PublicDataClass.YkState == 2)
                    PublicDataClass.SedNetFrameMsg = "撤销<合>";
                PublicDataClass._NetStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.NetThreadID = 7;
                DelayTime = PublicDataClass._Time.NetDelayTime;
            }
            else if (PublicDataClass._NetTaskFlag.VERSION_1 == true)    //读版本号
            {
                PublicDataClass._NetTaskFlag.VERSION_1 = false;
                PublicDataClass._NetStructData.NetTLen = 0;
              
                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.seq = 1;
                PublicDataClass.SQflag = 0;
                PublicDataClass.ZDYtype = 5;
                if (PublicDataClass.Framelen == 231)
                {
                    //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1 || PublicDataClass._ProtocoltyFlag.ComProFlag==2)
                        PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(50);
                    //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                    //{
                    //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(50);
                    //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                    //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                    //}
                }
                else
                    PublicDataClass._NetStructData.NetTLen = protocoltyparam.EncodeFrame(5, PublicDataClass._NetStructData.NetTBuffer, PublicDataClass._DataField.Buffer,
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                          PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);

                PublicDataClass.SedNetFrameMsg = "请求版本号";
                PublicDataClass._NetStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.NetThreadID = 8;
                DelayTime = PublicDataClass._Time.NetDelayTime;


            }
            else if (PublicDataClass._NetTaskFlag.CALLTIME_1 == true)  //请求时间
            {
                PublicDataClass._NetTaskFlag.CALLTIME_1 = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                //PublicDataClass._NetStructData.NetTLen = EncodeFrame(1, 11, ref PublicDataClass._NetStructData.NetTBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.seq = 1;
                PublicDataClass.SQflag = 0;
                //PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(34);
                PublicDataClass.ZDYtype = 6;
                if (PublicDataClass.Framelen == 231)
                {
                    //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)
                        PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(50);
                    //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                    //{
                    //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(50);
                    //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                    //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                    //}
                }
                else
                    PublicDataClass._NetStructData.NetTLen = protocoltyparam.EncodeFrame(6, PublicDataClass._NetStructData.NetTBuffer, PublicDataClass._DataField.Buffer,
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                          PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);

                PublicDataClass.SedNetFrameMsg = "请求时间";
                PublicDataClass._NetStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.NetThreadID = 9;
                DelayTime = PublicDataClass._Time.NetDelayTime;

            }
            else if (PublicDataClass._NetTaskFlag.READ_P_1 == true)   //读参数
            {

                PublicDataClass._NetTaskFlag.READ_P_1 = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                //PublicDataClass._NetStructData.NetTLen = EncodeFrame(1, 9, ref PublicDataClass._NetStructData.NetTBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                //PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass.ZDYtype = 2;
                PublicDataClass.seqflag = 0;
                if (PublicDataClass.Framelen == 231)
                {
                    //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)
                        PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(50);
                    //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                    //{
                    //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(50);
                    //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                    //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                    //}
                }
                else
                    PublicDataClass._NetStructData.NetTLen = protocoltyparam.EncodeFrame(2, PublicDataClass._NetStructData.NetTBuffer, PublicDataClass._DataField.Buffer,
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                          PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);
                PublicDataClass.SedNetFrameMsg = "读取参数";
                PublicDataClass._NetStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.NetThreadID = 10;
                DelayTime = PublicDataClass._Time.NetDelayTime;


            }
            else if (PublicDataClass._NetTaskFlag.AloneCallYx_1 == true)
            {
                PublicDataClass._NetTaskFlag.AloneCallYx_1 = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                //PublicDataClass._NetStructData.NetTLen = EncodeFrame(1, 12, ref PublicDataClass._NetStructData.NetTBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                //PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;
                //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)
                    PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(39);
                //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                //{
                //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(39);
                //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                //}

                PublicDataClass.SedNetFrameMsg = "请求遥信";
                PublicDataClass._NetStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.NetThreadID = 11;
                DelayTime = PublicDataClass._Time.NetDelayTime;
            }
            else if (PublicDataClass._NetTaskFlag.AloneCallYc_1 == true)
            {
                PublicDataClass._NetTaskFlag.AloneCallYc_1 = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                //PublicDataClass._NetStructData.NetTLen = EncodeFrame(1, 13, ref PublicDataClass._NetStructData.NetTBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                //PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;
                //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)
                    PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(38);
                //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                //{
                //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(38);
                //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                //}

                PublicDataClass.SedNetFrameMsg = "请求遥测";
                PublicDataClass._NetStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.NetThreadID = 12;
                DelayTime = PublicDataClass._Time.NetDelayTime;

            }
            else if (PublicDataClass._NetTaskFlag.UpdateCode_Start_1 == true)
            {
                PublicDataClass._NetTaskFlag.UpdateCode_Start_1 = false;
                PublicDataClass._NetStructData.NetTLen = 0;
               
                PublicDataClass._DataField.FieldVSQ = 1;
                //PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(40);
                PublicDataClass.ZDYtype = 3;
                if (PublicDataClass.Framelen == 231)
                {
                    //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)
                        PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(50);
                    //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                    //{
                    //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(50);
                    //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                    //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                    //}
                }
                else
                    PublicDataClass._NetStructData.NetTLen = protocoltyparam.EncodeFrame(3, PublicDataClass._NetStructData.NetTBuffer, PublicDataClass._DataField.Buffer,
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                          PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);

                PublicDataClass.SedNetFrameMsg = "升级";
                PublicDataClass._NetStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.NetThreadID = 13;
                DelayTime = PublicDataClass._Time.NetDelayTime;
            }
            else if (PublicDataClass._NetTaskFlag.UpdateCode_ARMStart_1 == true)
            {
                PublicDataClass._NetTaskFlag.UpdateCode_ARMStart_1 = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                //PublicDataClass._NetStructData.NetTLen = EncodeFrame(1, 24, ref PublicDataClass._NetStructData.NetTBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                          PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                //PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;
                //PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(43);
                PublicDataClass.ZDYtype = 9;
                if (PublicDataClass.Framelen == 231)
                {
                    //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)
                        PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(50);
                    //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                    //{
                    //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(50);
                    //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                    //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                    //}
                }
                else
                    PublicDataClass._NetStructData.NetTLen = protocoltyparam.EncodeFrame(9, PublicDataClass._NetStructData.NetTBuffer, PublicDataClass._DataField.Buffer,
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                          PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);


                PublicDataClass.SedNetFrameMsg = "ARM升级";
                PublicDataClass._NetStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.NetThreadID = 24;
                DelayTime = PublicDataClass._Time.NetDelayTime;
            }
            else if (PublicDataClass._NetTaskFlag.UpdateCode_JY_1 == true)
            {
                PublicDataClass._NetTaskFlag.UpdateCode_JY_1 = false;
                PublicDataClass._NetStructData.NetTLen = 0;
               
                PublicDataClass._DataField.FieldVSQ = 1;
                //PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(41);
                PublicDataClass.ZDYtype = 3;
                if (PublicDataClass.Framelen == 231)
                {
                    //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)
                        PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(50);
                    //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                    //{
                    //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(50);
                    //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                    //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                    //}
                }
                else
                    PublicDataClass._NetStructData.NetTLen = protocoltyparam.EncodeFrame(3, PublicDataClass._NetStructData.NetTBuffer, PublicDataClass._DataField.Buffer,
                                                         PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                         PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);


                PublicDataClass.SedNetFrameMsg = "校验";
                PublicDataClass._NetStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.NetThreadID = 14;
                DelayTime = PublicDataClass._Time.NetDelayTime;
            }
            else if (PublicDataClass._NetTaskFlag.UpdateCode_ARMJY_1 == true)
            {
                PublicDataClass._NetTaskFlag.UpdateCode_ARMJY_1 = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                //PublicDataClass._NetStructData.NetTLen = EncodeFrame(1, 24, ref PublicDataClass._NetStructData.NetTBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                //PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;
                //PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(43);
                PublicDataClass.ZDYtype = 9;
                if (PublicDataClass.Framelen == 231)
                {
                    //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)
                        PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(50);
                    //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                    //{
                    //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(50);
                    //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                    //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                    //}
                }
                else
                    PublicDataClass._NetStructData.NetTLen = protocoltyparam.EncodeFrame(9, PublicDataClass._NetStructData.NetTBuffer, PublicDataClass._DataField.Buffer,
                                                         PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                         PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);


                PublicDataClass.SedNetFrameMsg = "ARM校验";
                PublicDataClass._NetStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.NetThreadID = 24;
                DelayTime = PublicDataClass._Time.NetDelayTime;
            }
            else if (PublicDataClass._NetTaskFlag.UpdateCode_OK_1 == true)
            {
                PublicDataClass._NetTaskFlag.UpdateCode_OK_1 = false;
                PublicDataClass._NetStructData.NetTLen = 0;
               
                PublicDataClass._DataField.FieldVSQ = 0;
                //PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(42);
                   PublicDataClass.ZDYtype = 3;
                   if (PublicDataClass.Framelen == 231)
                   {
                       //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)
                           PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(50);
                       //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                       //{
                       //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(50);
                       //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                       //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                       //}
                   }
               else
                       PublicDataClass._NetStructData.NetTLen = protocoltyparam.EncodeFrame(3, PublicDataClass._NetStructData.NetTBuffer, PublicDataClass._DataField.Buffer,
                                                           PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                           PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);


                PublicDataClass.SedNetFrameMsg = "更新代码";
                PublicDataClass._ThreadIndex.NetThreadID = 15;
                PublicDataClass._NetStructData.TX_TASK = true;
            }
            else if (PublicDataClass._NetTaskFlag.UpdateCode_ARMOK_1 == true)
            {
                PublicDataClass._NetTaskFlag.UpdateCode_ARMOK_1 = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                //PublicDataClass._NetStructData.NetTLen = EncodeFrame(1, 24, ref PublicDataClass._NetStructData.NetTBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                //PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;
                //PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(43);
                PublicDataClass.ZDYtype = 9;
                if (PublicDataClass.Framelen == 231)
                {
                    //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)
                        PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(50);
                    //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                    //{
                    //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(50);
                    //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                    //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                    //}
                }
                else
                    PublicDataClass._NetStructData.NetTLen = protocoltyparam.EncodeFrame(9, PublicDataClass._NetStructData.NetTBuffer, PublicDataClass._DataField.Buffer,
                                                        PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                        PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);





                PublicDataClass.SedNetFrameMsg = "ARM更新代码";
                PublicDataClass._ThreadIndex.NetThreadID = 24;
                PublicDataClass._NetStructData.TX_TASK = true;
            }
            else if (PublicDataClass._NetTaskFlag.ARMX_DOWN  == true)//协处理器下载
            {
                PublicDataClass._NetTaskFlag.ARMX_DOWN = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                //PublicDataClass._NetStructData.NetTLen = DateCodForm.EncodeFrame(1);
                PublicDataClass.ZDYtype = 1;
                if (PublicDataClass.Framelen == 231)
                {
                    //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)
                        PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(50);
                    //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                    //{
                    //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(50);
                    //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                    //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                    //}
                }
                else
                    PublicDataClass._NetStructData.NetTLen = protocoltyparam.EncodeFrame(1, PublicDataClass._NetStructData.NetTBuffer, PublicDataClass._DataField.Buffer,
                                                        PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                        PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);



                PublicDataClass.SedNetFrameMsg = "协处理器下载参数";
       
                PublicDataClass._NetStructData.TX_TASK = true;

            }
            else if (PublicDataClass._NetTaskFlag.Do_OKTACT == true)   //S格式确认帧
            {

                PublicDataClass._NetTaskFlag.Do_OKTACT = false;
                //PublicDataClass._NetStructData.NetTLen = EncodeFrame(1, 1, ref PublicDataClass._NetStructData.NetTBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                        PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);
              
                PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(7);
           

                PublicDataClass.SedNetFrameMsg = "确认";
                //PublicDataClass._FrameTime.T1 = 6;
                PublicDataClass._ThreadIndex.NetThreadID = 16;
                PublicDataClass._NetStructData.TX_TASK = true;
            }
            else if (PublicDataClass._NetTaskFlag.Do_TESTACT == true)  //测试帧
            {
                PublicDataClass._NetTaskFlag.Do_TESTACT = false;
                //PublicDataClass._NetStructData.NetTLen = EncodeFrame(1, 2, ref PublicDataClass._NetStructData.NetTBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                        PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);
                PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(5);

                PublicDataClass.SedNetFrameMsg = "测试";
                PublicDataClass._ThreadIndex.NetThreadID = 17;
                PublicDataClass._NetStructData.TX_TASK = true;
                DelayTime = PublicDataClass._Time.NetDelayTime;
                PublicDataClass._FrameTime.T1 = 15;

            }
            else if (PublicDataClass._NetTaskFlag.Reset_1 == true)   //复位进程
            {
                PublicDataClass._NetTaskFlag.Reset_1 = false;
                //PublicDataClass._NetStructData.NetTLen = EncodeFrame(1, 17, ref PublicDataClass._NetStructData.NetTBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                        PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;
                //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)
                PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(13);
                // else  if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                //{
                //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(13);
                //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                //}

                PublicDataClass.SedNetFrameMsg = "复位";
                PublicDataClass._NetStructData.TX_TASK = true;
            }
            else if (PublicDataClass._NetTaskFlag.AloneCallYx_2 == true)
            {
                //PublicDataClass._NetTaskFlag.AloneCallYx_2 = false;
                //PublicDataClass._NetStructData.NetTLen = 0;
                //PublicDataClass._NetStructData.NetTLen = EncodeFrame(1, 18, ref PublicDataClass._NetStructData.NetTBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                //PublicDataClass.SedNetFrameMsg = "请求内遥信";
                //PublicDataClass._NetStructData.TX_TASK = true;
                //PublicDataClass._ThreadIndex.NetThreadID = 18;
                //DelayTime = PublicDataClass._Time.NetDelayTime;
            }
            else if (PublicDataClass._NetTaskFlag.CallHisData == true)
            {
                PublicDataClass._NetTaskFlag.CallHisData = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                //PublicDataClass._NetStructData.NetTLen = EncodeFrame(1, 19, ref PublicDataClass._NetStructData.NetTBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                //PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;
                //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)
                    PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(35);
                //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                //{
                //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(35);
                //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                //}

                PublicDataClass.SedNetFrameMsg = "请求历史数据";
                PublicDataClass._NetStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.NetThreadID = 19;
                DelayTime = PublicDataClass._Time.NetDelayTime;
            }
            else if (PublicDataClass._NetTaskFlag.CallRecordData == true)
            {
                PublicDataClass._NetTaskFlag.CallRecordData = false;
                //PublicDataClass._NetStructData.NetTLen = EncodeFrame(1, 20, ref PublicDataClass._NetStructData.NetTBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                ////PublicDataClass._DataField.FieldLen = 0;
                //PublicDataClass._DataField.FieldVSQ = 1;
                //PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(36);
                PublicDataClass.ZDYtype = 4;
                if (PublicDataClass.Framelen == 231)
                {
                    //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)
                        PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(50);
                    //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                    //{
                    //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(50);
                    //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                    //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                    //}
                }
                else
                    PublicDataClass._NetStructData.NetTLen = protocoltyparam.EncodeFrame(4, PublicDataClass._NetStructData.NetTBuffer, PublicDataClass._DataField.Buffer,
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                          PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);
                PublicDataClass.SedNetFrameMsg = "请求记录数据";
                PublicDataClass._NetStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.NetThreadID = 20;
                DelayTime = PublicDataClass._Time.NetDelayTime;
            }
            else if (PublicDataClass._NetTaskFlag.FuGuiCmd == true)
            {
                PublicDataClass._NetTaskFlag.FuGuiCmd = false;
                //PublicDataClass._NetStructData.NetTLen = EncodeFrame(1, 21, ref PublicDataClass._NetStructData.NetTBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                //PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;
                //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)
                    PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(47);
                //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                //{
                //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(47);
                //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                //}
                PublicDataClass.SedNetFrameMsg = "复归命令";
                PublicDataClass._NetStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.NetThreadID = 21;
                DelayTime = PublicDataClass._Time.NetDelayTime;
            }

            else if (PublicDataClass._NetTaskFlag.READ_Hard_1 == true)    //读器件状态
            {
                PublicDataClass._NetTaskFlag.READ_Hard_1 = false;
                PublicDataClass._NetStructData.NetTLen = 0;

                //PublicDataClass._DataField.FieldVSQ = 1;
                //PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(37);
                PublicDataClass.ZDYtype = 8;

                PublicDataClass._DataField.FieldLen = 0;

                PublicDataClass._DataField.FieldVSQ = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.seq = 1;
                PublicDataClass.SQflag = 0;
                if (PublicDataClass.Framelen == 231)
                {
                    //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)
                        PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(50);
                    //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                    //{
                    //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(50);
                    //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                    //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                    //}
                }
                else

                    PublicDataClass._NetStructData.NetTLen = protocoltyparam.EncodeFrame(8, PublicDataClass._NetStructData.NetTBuffer, PublicDataClass._DataField.Buffer,
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                          PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);
                PublicDataClass.SedNetFrameMsg = "请求器件状态";
                PublicDataClass._NetStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.NetThreadID = 24;
                DelayTime = PublicDataClass._Time.NetDelayTime;
            }

            else if (PublicDataClass._NetTaskFlag.Transmit == true)    //读器件状态
            {
                PublicDataClass._NetTaskFlag.Transmit = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;
                //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)
                    PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(40);
                //else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                //{
                //    PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(40);
                //    for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                //        PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                //}
                PublicDataClass.SedNetFrameMsg = "数据转发";
                PublicDataClass._NetStructData.TX_TASK = true;
            }
            else if (PublicDataClass._NetTaskFlag.SET_YC_1 == true)  //遥测置数
            {
                PublicDataClass._NetTaskFlag.SET_YC_1 = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(20);

                PublicDataClass.SedNetFrameMsg = "遥测置数";
                PublicDataClass._NetStructData.TX_TASK = true;
                //PublicDataClass._ThreadIndex.NetThreadID = 3;
                DelayTime = PublicDataClass._Time.NetDelayTime;

            }
            else if (PublicDataClass._NetTaskFlag.SET_YC_2 == true)  //遥测置数
            {
                PublicDataClass._NetTaskFlag.SET_YC_2 = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(21);


                PublicDataClass.SedNetFrameMsg = "遥测置数";
                PublicDataClass._NetStructData.TX_TASK = true;
                //PublicDataClass._ThreadIndex.NetThreadID = 3;
                DelayTime = PublicDataClass._Time.NetDelayTime;

            }
            else if (PublicDataClass._NetTaskFlag.SET_YC_3 == true)  //遥测置数
            {
                PublicDataClass._NetTaskFlag.SET_YC_3 = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(22);


                PublicDataClass.SedNetFrameMsg = "遥测置数";
                PublicDataClass._NetStructData.TX_TASK = true;
                //PublicDataClass._ThreadIndex.NetThreadID = 3;
                DelayTime = PublicDataClass._Time.NetDelayTime;

            }
            else if (PublicDataClass._NetTaskFlag.CEL_YC_1 == true)  //遥测置数撤销
            {
                PublicDataClass._NetTaskFlag.CEL_YC_1 = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(51);

                PublicDataClass.SedNetFrameMsg = "遥测置数撤销";
                PublicDataClass._NetStructData.TX_TASK = true;
                //PublicDataClass._ThreadIndex.NetThreadID = 3;
                DelayTime = PublicDataClass._Time.NetDelayTime;

            }
            else if (PublicDataClass._NetTaskFlag.CEL_YC_2 == true)  //遥测置数撤销
            {
                PublicDataClass._NetTaskFlag.CEL_YC_2 = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(52);


                PublicDataClass.SedNetFrameMsg = "遥测置数撤销";
                PublicDataClass._NetStructData.TX_TASK = true;
                //PublicDataClass._ThreadIndex.NetThreadID = 3;
                DelayTime = PublicDataClass._Time.NetDelayTime;

            }
            else if (PublicDataClass._NetTaskFlag.CEL_YC_3 == true)  //遥测置数撤销
            {
                PublicDataClass._NetTaskFlag.CEL_YC_3 = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(53);


                PublicDataClass.SedNetFrameMsg = "遥测置数撤销";
                PublicDataClass._NetStructData.TX_TASK = true;
                //PublicDataClass._ThreadIndex.NetThreadID = 3;
                DelayTime = PublicDataClass._Time.NetDelayTime;

            }
            else if (PublicDataClass._NetTaskFlag.READ_ReP_1 == true)   //读继保定值
            {

                PublicDataClass._NetTaskFlag.READ_ReP_1 = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(61);
                PublicDataClass.SedNetFrameMsg = "召唤继电保护定值";
                PublicDataClass._NetStructData.TX_TASK = true;
                DelayTime = PublicDataClass._Time.NetDelayTime;


            }
            else if (PublicDataClass._NetTaskFlag.SET_ReP== true)   //下装继保定值
            {

                PublicDataClass._NetTaskFlag.SET_ReP = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(62);
                PublicDataClass.SedNetFrameMsg = "下装继电保护定值";
                PublicDataClass._NetStructData.TX_TASK = true;
                DelayTime = PublicDataClass._Time.NetDelayTime;
            }
            else if (PublicDataClass._NetTaskFlag.ACT_ReP  == true)   //激活继保定值
            {

                PublicDataClass._NetTaskFlag.ACT_ReP = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(63);
                PublicDataClass.SedNetFrameMsg = "激活继保定值";
                PublicDataClass._NetStructData.TX_TASK = true;
                DelayTime = PublicDataClass._Time.NetDelayTime;
            }
            else if (PublicDataClass._NetTaskFlag.CancelACT_ReP  == true)   //撤销激活继保定值
            {

                PublicDataClass._NetTaskFlag.CancelACT_ReP = false;
                PublicDataClass._NetStructData.NetTLen = 0;
                PublicDataClass._NetStructData.NetTLen = protocolty104.EncodeFrame(64);
                PublicDataClass.SedNetFrameMsg = "撤销激活继保定值";
                PublicDataClass._NetStructData.TX_TASK = true;
                DelayTime = PublicDataClass._Time.NetDelayTime;
            }
            //else if (PublicDataClass._NetTaskFlag.CALL_1 == true)      //请求一级数据
            //{
            //    PublicDataClass._NetTaskFlag.CALL_1 = false;
            //    PublicDataClass._NetStructData.NetTLen = 0;
            //    if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
            //    {
            //        PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(3);
            //        for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
            //            PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
            //        PublicDataClass.SedNetFrameMsg = "请求一级数据";
            //        ProtoDelayTime = PublicDataClass._ProtocoltyFlag.DelayTime;
            //        DelayTime = PublicDataClass._Time.NetDelayTime;
            //        PublicDataClass._ComStructData.FCB = (!PublicDataClass._ComStructData.FCB);
            //    }

            //    PublicDataClass._NetStructData.TX_TASK = true;


            //}
            //else if (PublicDataClass._NetTaskFlag.CALL_2 == true)      //请求二级数据
            //{
            //    PublicDataClass._NetTaskFlag.CALL_2 = false;

            //    PublicDataClass._NetStructData.NetTLen = 0;
            //    if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
            //    {
            //        PublicDataClass._NetStructData.NetTLen = protocolty101.EncodeFrame(4);
            //        for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
            //            PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
            //        PublicDataClass.SedNetFrameMsg = "请求二级数据";
            //        ProtoDelayTime = PublicDataClass._ProtocoltyFlag.DelayTime;
            //        DelayTime = PublicDataClass._Time.NetDelayTime;
            //        PublicDataClass._ComStructData.FCB = (!PublicDataClass._ComStructData.FCB);
            //    }

            //    PublicDataClass._NetStructData.TX_TASK = true;
            //}

        }

        private void ReceiveNetMsg()   //---------------------------------------------网络解包线程
        {
            while (true)
            {
                try
                {
                    if (PublicDataClass._Message.NetShowSendOver == false)
                    {
                        PublicDataClass._NetStructData.NetRLen = conn.Receive(PublicDataClass._NetStructData.NetRBuffer);
                        if (PublicDataClass._NetStructData.NetRLen > 0)
                        {
                            ParseNetReceData();
                            PublicDataClass._Message.NetShowRecvTextData = true;
                            PublicDataClass._FrameTime.T3 = 20;
                            ProtoDelayTime = PublicDataClass._ProtocoltyFlag.DelayTime;
                            
                        }
                        else
                        {
                            if (PublicDataClass.TcpLinkType == 2)
                                NetLinkIsClose = true;
                            //if (PublicDataClass.TcpLinkType != 0)
                            //{
                            //    PublicDataClass.TcpLinkType = 0;
                            //    PublicDataClass._Message.TcpLinkState = true;
                            //    NetLinkIsClose = true;
                            //}

                        }

                    }

                }
                catch
                {
                    //if (PublicDataClass.TcpLinkType == 2)
                    //    NetLinkIsClose = true;
                    //if (PublicDataClass.TcpLinkType != 0)
                    //{
                    //    PublicDataClass.TcpLinkType = 0;
                    //    PublicDataClass._Message.TcpLinkState = true;
                    //    NetLinkIsClose = true;
                    //}

                }
                Thread.Sleep(1);

            }

        }

        /// <summary>
        /// 自定义的网络数据的解包处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param anthor="cuibj"></param>
        /// <param Time="11-04-28"></param>
        public static void ParseNetReceData()   //网络数据的解包
        {
        
            DelayTime = 0;
            PublicDataClass.DataTy = 0;
            PublicDataClass._DataField.FieldVSQ = 0;
            PublicDataClass.RevNetFrameMsg = "";
            //PublicDataClass.DataTy = DecodeFrame(1, ref PublicDataClass._NetStructData.NetRBuffer[0], ref PublicDataClass._DataField.Buffer[0],
            //       ref PublicDataClass._DataField.FieldLen, ref PublicDataClass._DataField.FieldVSQ, ref PublicDataClass.ParamInfoAddr, PublicDataClass._NetStructData.NetRLen, ref STxSeq, ref SRxSeq);
            
              int DevAddr=0;
              byte TargetBoard=0, seqflag=0,  seq=0,  SQflag=0;
            if (PublicDataClass._NetStructData.NetRBuffer[0] == 0x69)
                {
                    PublicDataClass.DataTy = protocoltyparam.DecodeFrame(PublicDataClass._NetStructData.NetRBuffer, PublicDataClass._NetStructData.NetRLen, PublicDataClass._DataField.Buffer, ref PublicDataClass._DataField.FieldLen, ref DevAddr, ref TargetBoard, ref PublicDataClass.seqflag, ref PublicDataClass.seq, ref PublicDataClass.SQflag, ref PublicDataClass._DataField.FieldVSQ, ref PublicDataClass.ParamInfoAddr);
                }
            else if(PublicDataClass._NetStructData.NetRBuffer[0] == 0x16)
                PublicDataClass.DataTy = DateCodForm.DecodeFrame();
                
            else
            {
                if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1 || PublicDataClass._ProtocoltyFlag.ComProFlag ==2)
                   PublicDataClass.DataTy = protocolty104.DecodeFrame();
                else if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                {
                    //PublicDataClass._ComStructData.RxLen = PublicDataClass._NetStructData.NetRLen;
                    //for (int i = 0; i < PublicDataClass._NetStructData.NetRLen; i++)
                    //    PublicDataClass._ComStructData.RXBuffer[i] = PublicDataClass._NetStructData.NetRBuffer[i];
                    //PublicDataClass.DataTy = protocolty101.DecodeFrame();
                        //思路：串口网口buffer和消息转换采用串口解包函数进行处理
                        RBufferNetToCom();
                        ParseComReceData();
                        PublicDataClass.RevNetFrameMsg = PublicDataClass.ComFrameMsg;
                        return;

                }
            }
           
            if (PublicDataClass.DataTy == 0)     //非法帧
            {
                PublicDataClass.RevNetFrameMsg = "无效";
            }

            else if (PublicDataClass.DataTy == 1)     
            {
                PublicDataClass.RevNetFrameMsg = "启动确认";
                            if (PublicDataClass._ProtocoltyFlag.Test104 == true)//104流程测试
                        {
                            if (PublicDataClass._ProtocoltyFlag.ZZFirstFlag == true )
                            {
                                PublicDataClass._ProtocoltyFlag.ZZFirstFlag = false;
                                PublicDataClass._ProtocoltyFlag.ZZFlag = true;
                                PublicDataClass._NetTaskFlag.C_IC_NA_1 = true;  //总召唤
                                ProtoZZTime = PublicDataClass._ProtocoltyFlag.ZZTime;
                            }

                        }
                            PublicDataClass._FrameTime.T1 = 0;
            }
            else if (PublicDataClass.DataTy == 2)   
            {
                PublicDataClass.RevNetFrameMsg = "测试确认";
                PublicDataClass._FrameTime.T1 = 0;
            }
            else if (PublicDataClass.DataTy == 3)    
            {
                PublicDataClass.RevNetFrameMsg = "停止确认";
                PublicDataClass._FrameTime.T1 = 0;
            }
            else if (PublicDataClass.DataTy == 9)     //S帧确认帧
            {
                PublicDataClass.RevNetFrameMsg = "应答";
                //PublicDataClass._FrameTime.T1 = 0;
                //PublicDataClass._FrameTime.T2 = 0;
            }
            else if (PublicDataClass.DataTy == 100)     //
            {
                PublicDataClass.RevNetFrameMsg = "参数设置（确认）";
                PublicDataClass._Message.ParamAck = true;

            }
            else if (PublicDataClass.DataTy == 101)     //
            {
                PublicDataClass.RevNetFrameMsg = "参数设置（否认）";

            }
            else if (PublicDataClass.DataTy == 102)   //
            {

                PublicDataClass.RevNetFrameMsg = "参数读取";
                PublicDataClass._Message.ReadParam = true;
            }
            else if (PublicDataClass.DataTy == 4)     //
            {
                PublicDataClass.RevNetFrameMsg = "总召激活";
            }
            else if (PublicDataClass.DataTy == 5)     //
            {
                PublicDataClass.RevNetFrameMsg = "总召结束";
                //TxSeq = TxSeq + 2;
                MTxSeq = SRxSeq;
                MRxSeq = STxSeq + 2;
                PublicDataClass._FrameTime.T2 = 10;
                if (PublicDataClass._ProtocoltyFlag.Test104 == true)//104流程测试
                {

            //        PublicDataClass._NetTaskFlag.C_CS_NA_1 = true;//发对时
                }

            }
            else if (PublicDataClass.DataTy == 35)     //  遥测（单独召测) 短浮点型，5字节
            {
                PublicDataClass.RevNetFrameMsg = "遥测";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (PublicDataClass.DataTy == 40)     //   短浮点型，5字节
            {
                PublicDataClass.RevNetFrameMsg = "遥测";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (PublicDataClass.DataTy == 36)     // 带品质描述归一化值 ，3字节
            {
                PublicDataClass.RevNetFrameMsg = "遥测";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (PublicDataClass.DataTy == 38)     // 带品质描述归一化值 ，3字节
            {
                PublicDataClass.RevNetFrameMsg = "遥测";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (PublicDataClass.DataTy == 42)     //不带品质描述归一化值，2字节
            {
                PublicDataClass.RevNetFrameMsg = "遥测";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (PublicDataClass.DataTy == 41)     //           8字节
            {
                PublicDataClass.RevNetFrameMsg = "扰动事件";
                PublicDataClass._ChangeFlag.ChangInfoViewUpdate = true;
                PublicDataClass._Message.RaoDongEvent = true;
                //PublicDataClass._Message.CallDataDocmentView = true;
                PublicDataClass._FrameTime.T2 = 10;
            }
            else if (PublicDataClass.DataTy == 37)     //          6字节
            {
                PublicDataClass.RevNetFrameMsg = "扰动事件";
                PublicDataClass._ChangeFlag.ChangInfoViewUpdate = true;
                PublicDataClass._Message.RaoDongEvent = true;
                //PublicDataClass._Message.CallDataDocmentView = true;
                PublicDataClass._FrameTime.T2 = 10;
            }
            else if (PublicDataClass.DataTy == 39)     //          6字节
            {
                PublicDataClass.RevNetFrameMsg = "扰动事件";
                PublicDataClass._ChangeFlag.ChangInfoViewUpdate = true;
                PublicDataClass._Message.RaoDongEvent = true;
                //PublicDataClass._Message.CallDataDocmentView = true;
                PublicDataClass._FrameTime.T2 = 10;
            }
            else if (PublicDataClass.DataTy == 43)  //         5字节
            {
                PublicDataClass.RevNetFrameMsg = "扰动事件";
                PublicDataClass._ChangeFlag.ChangInfoViewUpdate = true;
                PublicDataClass._Message.RaoDongEvent = true;
                PublicDataClass._FrameTime.T2 = 10;
            }

            //else if ((PublicDataClass.DataTy == 50)||(PublicDataClass.DataTy == 51))   //
            //{
            //    PublicDataClass.RevNetFrameMsg = "遥信";
            //}
            else if (PublicDataClass.DataTy == 50)    //遥信（单独召测)
            {
                PublicDataClass.RevNetFrameMsg = "遥信";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (PublicDataClass.DataTy == 51)    //正常遥信
            {
                PublicDataClass.RevNetFrameMsg = "遥信";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (PublicDataClass.DataTy == 52)     //
            {
                PublicDataClass.RevNetFrameMsg = "变位事件";
                PublicDataClass._FrameTime.T2 = 10;
                PublicDataClass._ChangeFlag.ChangInfoViewUpdate = true;
                PublicDataClass._Message.YxBianWeiOfNoTimeEvent = true;
            }
            else if (PublicDataClass.DataTy == 53)    //正常双点遥信
            {
                PublicDataClass.RevNetFrameMsg = "遥信";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (PublicDataClass.DataTy == 54)     //双点遥信变位
            {
                PublicDataClass.RevNetFrameMsg = "变位事件";
                PublicDataClass._FrameTime.T2 = 10;
                PublicDataClass._ChangeFlag.ChangInfoViewUpdate = true;
                PublicDataClass._Message.YxBianWeiOfNoTimeEvent = true;

            }
            else if (PublicDataClass.DataTy == 56)      //带56时标的遥信SOE
            {
                PublicDataClass.RevNetFrameMsg = "变位事件";
                PublicDataClass._ThreadIndex.NetThreadID = 0;
                PublicDataClass._FrameTime.T2 = 10;   //10S
                PublicDataClass._ChangeFlag.ChangInfoViewUpdate = true;
                PublicDataClass._Message.YxBianWeiOfTimeEvent = true;
            }
            else if (PublicDataClass.DataTy == 58)      //带56时标的双点遥信SOE
            {
                PublicDataClass.RevNetFrameMsg = "变位事件";
                PublicDataClass._ThreadIndex.NetThreadID = 0;
                PublicDataClass._FrameTime.T2 = 10;   //10S
                PublicDataClass._ChangeFlag.ChangInfoViewUpdate = true;
                PublicDataClass._Message.YxBianWeiOfTimeEvent = true;
            }
            else if (PublicDataClass.DataTy == 29)     //
            {
                PublicDataClass.RevNetFrameMsg = "转发回复";
            }
            else if (PublicDataClass.DataTy == 10||PublicDataClass.DataTy == 13)     //
            {
                PublicDataClass.RevNetFrameMsg = "选择应答";
                PublicDataClass._Message.YkDocmentView = true;
            }
            else if (PublicDataClass.DataTy == 11 || PublicDataClass.DataTy == 14)     //
            {
                PublicDataClass.RevNetFrameMsg = "执行成功";
                PublicDataClass._Message.YkDocmentView = true;
            }
            else if (PublicDataClass.DataTy == 12 || PublicDataClass.DataTy == 15)     //
            {
                PublicDataClass.RevNetFrameMsg = "遥控撤销(确认)";
                PublicDataClass._Message.YkDocmentView = true;
            }
            
            else if (PublicDataClass.DataTy == 7)
            {
                PublicDataClass.RevNetFrameMsg = "复位确认";

            }
           
            else if (PublicDataClass.DataTy == 59)  //停电
            {
                PublicDataClass.RevNetFrameMsg = "停电事件";
                PublicDataClass._ChangeFlag.ChangInfoViewUpdate = true;
                PublicDataClass._Message.PowOffEvent = true;
            }
           

            else if (PublicDataClass.DataTy == 60)  //故障类型
            {
                PublicDataClass.RevNetFrameMsg = "故障事件";
                PublicDataClass._ChangeFlag.ChangInfoViewUpdate = true;
                PublicDataClass._Message.FaultEvent = true;
            }

            else if (PublicDataClass.DataTy == 71 || PublicDataClass.DataTy == 73 || PublicDataClass.DataTy == 75 || PublicDataClass.DataTy == 113)     //
            {
                PublicDataClass.RevNetFrameMsg = "设置确认";
        
            }
            else if (PublicDataClass.DataTy == 72 || PublicDataClass.DataTy == 74 || PublicDataClass.DataTy == 76 || PublicDataClass.DataTy == 114)     //
            {
                PublicDataClass.RevNetFrameMsg = "撤销确认";
        
            }
            //else if (PublicDataClass.DataTy == 26)     //
            //{
            //    PublicDataClass.RevNetFrameMsg = "器件状态";
            //    PublicDataClass._Message.CallDataDocmentView = true;
            //}
           


            else if (PublicDataClass.DataTy == 103)
            {
                if (PublicDataClass.ParamInfoAddr < 1000)
                    PublicDataClass.RevNetFrameMsg = "升级(应答)";
                else if (PublicDataClass.ParamInfoAddr == 1000)
                {
                    PublicDataClass.RevNetFrameMsg = "校验(应答)";
                    PublicDataClass._Message.CodeUpdateJY = true;
                }
            }
            else if (PublicDataClass.DataTy == 104)  //记录
            {
                PublicDataClass.RevNetFrameMsg = "历史记录";
                PublicDataClass._Message.CallHisDataDocmentView = true;
            }
            else if (PublicDataClass.DataTy == 105)  //版本号
            {
                PublicDataClass.RevNetFrameMsg = "版本号";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (PublicDataClass.DataTy == 106)  //时间
            {
                PublicDataClass.RevNetFrameMsg = "时间";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (PublicDataClass.DataTy == 108)     //
            {
                PublicDataClass.RevNetFrameMsg = "器件状态";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (PublicDataClass.DataTy == 109)     ////ARM
            {
               
                if (PublicDataClass.ParamInfoAddr < 1000)
                    PublicDataClass.RevNetFrameMsg = "升级(应答)";
                else if (PublicDataClass.ParamInfoAddr == 1000)
                {
                    PublicDataClass.RevNetFrameMsg = "校验(应答)";
                    PublicDataClass._Message.CodeUpdateJY = true;
                }
             
            }
            else if (PublicDataClass.DataTy == 110)     //
            {
                PublicDataClass.RevNetFrameMsg = "响应召唤继电保护定值";
                PublicDataClass._Message.CallRelayProtectView = true;
            }
            else if (PublicDataClass.DataTy == 111)     //
            {
                PublicDataClass.RevNetFrameMsg = "设定继电保护定值确认";
                PublicDataClass._Message.CallRelayProtectView = true;
            }
            else if (PublicDataClass.DataTy == 112)     //
            {
                PublicDataClass.RevNetFrameMsg = "设定继电保护定值否认";
                PublicDataClass._Message.CallRelayProtectView = true;
            }
              //====================非法帧====================================
            else if (PublicDataClass.DataTy == 150)     //非法帧,启动字符及数据长度错误
            {
                PublicDataClass.RevNetFrameMsg = "非法帧-启动字符及数据长度错误";
            }
            else if (PublicDataClass.DataTy == 151)     //非法帧固定帧控制域无效
            {
                PublicDataClass.RevNetFrameMsg = "非法帧-固定帧控制域无效错误";
            }
            else if (PublicDataClass.DataTy == 152)     //非法帧，发送序列号错误
            {
                PublicDataClass.RevNetFrameMsg = "非法帧-发送序列号错误";
            }
            else if (PublicDataClass.DataTy == 169)     //总召传送原因错误
            {
                PublicDataClass.RevNetFrameMsg = "总召传送原因错误";
            }
            else if (PublicDataClass.DataTy == 153)     //不带时标单点遥控传送原因错误
            {
                PublicDataClass.RevNetFrameMsg = "单点遥控传送原因错误";
            }
            else if (PublicDataClass.DataTy == 154)     //不带时标双点遥控传送原因错误
            {
                PublicDataClass.RevNetFrameMsg = "双点遥控传送原因错误";
            }
            else if (PublicDataClass.DataTy == 156)     //带时标双点遥控传送原因错误
            {
                PublicDataClass.RevNetFrameMsg = "带时标双点遥控传送原因错误";
            }
            else if (PublicDataClass.DataTy == 157)     //遥测归一化值传送原因错误
            {
                PublicDataClass.RevNetFrameMsg = "遥测归一化值传送原因错误";
            }
            else if (PublicDataClass.DataTy == 158)     //带时标双点遥控传送原因错误
            {
                PublicDataClass.RevNetFrameMsg = "遥测标度化值传送原因错误";
            }
            else if (PublicDataClass.DataTy == 159)     //遥测短浮点值传送原因错误
            {
                PublicDataClass.RevNetFrameMsg = "遥测短浮点值传送原因错误";
            }
            else if (PublicDataClass.DataTy == 160)     //遥测不带品质描述的归一化值传送原因错误
            {
                PublicDataClass.RevNetFrameMsg = "遥测不带品质描述的归一化值传送原因错误";
            }
            else if (PublicDataClass.DataTy == 161)     //遥测带56时标归一化值传送原因错误
            {
                PublicDataClass.RevNetFrameMsg = "遥测带56时标归一化值传送原因错误";
            }
            else if (PublicDataClass.DataTy == 162)     //遥测带56时标标度化值传送原因错误
            {
                PublicDataClass.RevNetFrameMsg = "遥测带56时标标度化值传送原因错误";
            }
            else if (PublicDataClass.DataTy == 163)     //遥测带56时标的短浮点值传送原因错误
            {
                PublicDataClass.RevNetFrameMsg = "遥测带56时标的短浮点值传送原因错误";
            }
            else if (PublicDataClass.DataTy == 164)     //单点遥信传送原因错误
            {
                PublicDataClass.RevNetFrameMsg = "单点遥信传送原因错误";
            }
            else if (PublicDataClass.DataTy == 165)     //遥测带56时标的短浮点值传送原因错误
            {
                PublicDataClass.RevNetFrameMsg = "双点遥信传送原因错误";
            }
            else if (PublicDataClass.DataTy == 166)     //带56时标的单点遥信传送原因错误
            {
                PublicDataClass.RevNetFrameMsg = "带56时标的单点遥信传送原因错误";
            }
            else if (PublicDataClass.DataTy == 167)     //带56时标的双点遥信传送原因错误
            {
                PublicDataClass.RevNetFrameMsg = "带56时标的双点遥信传送原因错误";
            }
            else if (PublicDataClass.DataTy == 168)     //   类型标识符错误
            {
                PublicDataClass.RevNetFrameMsg = "类型标识符错误";
            }
            else if (PublicDataClass.DataTy == 172 || PublicDataClass.DataTy == 170||PublicDataClass.DataTy == 171)     //   类型标识符错误
            {
                PublicDataClass.RevNetFrameMsg = "遥测置数应答传送原因错误";
            }
            else if (PublicDataClass.DataTy == 173 )     //   类型标识符错误
            {
                PublicDataClass.RevNetFrameMsg = "遥控执行结束";
            }
            else if (PublicDataClass.DataTy == 174)     //   PN==1
            {
                PublicDataClass.RevNetFrameMsg = "遥控否认";
            }
            //if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)//网口走101
            //{
            //    if (PublicDataClass.DataTy == 114)
            //    {
            //        PublicDataClass.RevNetFrameMsg = "无所召唤的数据";

            //    }
            //    else if (PublicDataClass.DataTy == 115)
            //    {
            //        PublicDataClass.RevNetFrameMsg = "以数据响应";

            //    }
            //    if (PublicDataClass._ProtocoltyFlag.ZZFlag ==false )//总召过程中时停止召唤一级二级数据
            //    {
            //        if (PublicDataClass._ProtocoltyFlag.ACD == 1)
            //        {
            //            PublicDataClass._NetTaskFlag.CALL_1 = true;
            //        }
            //        else if (PublicDataClass._ProtocoltyFlag.ACD == 2)
            //        {
            //            PublicDataClass._NetTaskFlag.CALL_2 = true;
            //        }
            //    }
            //}
         
        }

        
        public static void ListenhreadSendProc()   //GPRS 监听线程
        {
            while (true)  //处理事物
            {
                acceptedSocket = conn.Accept();
                if (acceptedSocket.Connected)
                {
              
                    PublicDataClass._Message.GprsAcceptState = true;
                    PublicDataClass.ClientInfo = acceptedSocket.RemoteEndPoint.ToString();
                    PublicDataClass._Message.LinkInfo = true;
                    LinkType = 2;

                }
                Thread.Sleep(3);
            }
        }
        public static void GprsThreadSendProc()   //GPRS 发送线程
        {
            while (true)  //处理事物
            {
                PtlGprsFrameProc(); // 是否有发送任务处理
                try
                {
                   if (acceptedSocket.Connected)
                   {
                        if (PublicDataClass._GprsStructData.TX_TASK)
                        {
                            PublicDataClass._GprsStructData.TX_TASK = false;
                            acceptedSocket.Send(PublicDataClass._GprsStructData.TBuffer, PublicDataClass._GprsStructData.GprsTLen, 0);
                            PublicDataClass._Message.GprsShowSendTextData = true;
                        }

                     }
                }
                catch
                {

                  //NetLinkIsClose = true;
                }
                
                Thread.Sleep(3);
            }
        }
        public static void ReceiveGprsMsg()   //GPRS 接收线程
        {
            while (true)  //处理事物
            {
                try
                {
                    if (acceptedSocket.Connected)
                    {

                        PublicDataClass._GprsStructData.GprsRLen = acceptedSocket.Receive(PublicDataClass._GprsStructData.RBuffer, 1024, 0);
                        if (PublicDataClass._GprsStructData.GprsRLen > 0)
                        {
                            ParseGprsReceData();
                            PublicDataClass._Message.GprsShowRecvTextData = true;
                        }
                    }
                }
                catch
                {

                }
                Thread.Sleep(3);
            }
        }

        public void UsbThreadProc()   //USB线程
        {

            uint i, remain, rxtimes, txtimes;
            byte[] AckBuf = new byte[64];

            while (true)  //处理事物
            {
                PtlUsbFrameProc(); // 是否有发送任务处理

                if (Open == true)
                {
                    if (PublicDataClass._UsbStructData.TX_TASK)
                    {
                        PublicDataClass._UsbStructData.TX_TASK = false;

                      /*  if (PublicDataClass._UsbStructData.UsbTLen > 64)                 //长度大于64
                        {
                            txtimes = PublicDataClass._UsbStructData.UsbTLen / 64;
                            rxtimes = txtimes;
                            remain = PublicDataClass._UsbStructData.UsbTLen % 64;
                            if (remain > 0)
                            {
                                rxtimes++;
                            }
                            OutBuffer[0] = 0xFF;   //组命令包，0xFF代表向下位机发送连续数据
                            OutBuffer[1] = (byte)((rxtimes >> 8) & 0x00FF);
                            OutBuffer[2] = (byte)(rxtimes & 0x00FF);
                            OutBuffer[3] = (byte)remain;
                            wrt = test.Write(0, OutBuffer, 4);  //发送命令
                            PublicDataClass._Message.UsbShowSendTextData = true;
                            Thread.Sleep(200);
                            rrt = test.Read(0, AckBuf, 64);  //读取应答
                            PublicDataClass._Message.UsbShowRecvTextData = true;
                            Thread.Sleep(200);

                            for (i = 0; i < txtimes; i++)
                            {
                                wrt = test.Write(0, &PublicDataClass._UsbStructData.TBuffer[i * 64], 64);  //连续发送数据包，每个数据包发送后读取应答
                                PublicDataClass._Message.UsbShowSendTextData = true;
                                Thread.Sleep(200);
                                rrt = test.Read(0, AckBuf, 64);
                        //        PublicDataClass._Message.UsbShowRecvTextData = true;
                            }
                            if (remain > 0)
                            {
                                wrt = test.Write(0, &PublicDataClass._UsbStructData.TBuffer[txtimes * 64], remain);
                                PublicDataClass._Message.UsbShowSendTextData = true;
                                Thread.Sleep(200);
                                rrt = test.Read(0, AckBuf, 64);  //发送剩余数据
                          //      PublicDataClass._Message.UsbShowRecvTextData = true;
                            }
                        }*/
                         if (PublicDataClass._UsbStructData.UsbTLen <= 64)                //长度小于等于64
                        {
                         //   wrt = test.Write(0, PublicDataClass._UsbStructData.TBuffer, (uint)PublicDataClass._UsbStructData.UsbTLen);  //下传数据                         
                            PublicDataClass._UsbStructData.TBuffer[0] = 0x00;
                            PublicDataClass._UsbStructData.TBuffer[1] = 0x00;
                            PublicDataClass._UsbStructData.TBuffer[2] = 0x40;
                            PublicDataClass._UsbStructData.TBuffer[3] = 0x00;

                            wrt = test.Write(0, PublicDataClass._UsbStructData.TBuffer, 4);  //下传数据
                             PublicDataClass._Message.UsbShowSendTextData = true;
                        }

                        Thread.Sleep(400);

                        if (wrt.FuncReturn == true)
                        {
                            rrt = test.Read(0, PublicDataClass._UsbStructData.RBuffer, (uint)64);  //启动上传数据
                            if (rrt.FuncReturn == true)
                            {
                                ParseUsbReceData();
                                PublicDataClass._UsbStructData.UsbRLen = 64;
                                PublicDataClass._Message.UsbShowRecvTextData = true;
                            }
                        }
                    }//end of if (PublicDataClass._UsbStructData.TX_TASK)
                }//end of  if (Open == true)     
                Thread.Sleep(100);
            }//end of while (true)  //处理事物
        }

       
        /*****************************************************************************************************************************
       *                                                          串口的打包解包操作                                                *
       *                                                                                                                            *
       *  功能  ：    数据帧类型处理                                                                                                *
       *  参数  ：    无                                                                                                            *
       *  返回值：    无                                                                                                            *
       *  修改日期：  2010-12-07                                                                                                    *
       *  作者    ：  cuibj                                                                                                         *
       * ***************************************************************************************************************************/
        public static void PtlComFrameProc()  //---------------------------------------------串口组包
        {
            if (!PublicDataClass._ComTaskFlag.FirstON_S)
            {
                //	第一次开机处理
                PublicDataClass._ComTaskFlag.FirstON_S         = true;		//	主机开机处理过标志(正常情况下该标志一直为1)
                PublicDataClass._ComTaskFlag.C_P1_NA_F         = false;     //104U格式中STARTDT命令
                PublicDataClass._ComTaskFlag.C_RQ_NA_LINKREQ_F = false;
                return ;
            }

         
            if (PublicDataClass._ComTaskFlag.C_RQ_NA_LINKREQ_F == true)    //请求链路状态
            {
                PublicDataClass._ComTaskFlag.C_RQ_NA_LINKREQ_F = false;
                PublicDataClass._ComStructData.TxLen = 0;
                //PublicDataClass._ComStructData.TxLen = EncodeFrame(1, 0, 0, ref PublicDataClass._ComStructData.TXBuffer[0],ref PublicDataClass._DataField.Buffer[0]);
              
                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;

                PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(1);


                PublicDataClass.ComFrameMsg = "请求链路状态";
                ProtoDelayTime = PublicDataClass._ProtocoltyFlag.DelayTime;
                PublicDataClass._ComStructData.TX_TASK = true;

            }
            else if (PublicDataClass._ComTaskFlag.C_RQ_NA_LINKREQ_F1 == true)    //链路复位
            {
                PublicDataClass._ComTaskFlag.C_RQ_NA_LINKREQ_F1 = false;
                PublicDataClass._ComStructData.TxLen = 0;
                //PublicDataClass._ComStructData.TxLen = EncodeFrame(1, 0, 0, ref PublicDataClass._ComStructData.TXBuffer[0],ref PublicDataClass._DataField.Buffer[0]);
                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;

                PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(2);


                PublicDataClass.ComFrameMsg = "链路复位";
                ProtoDelayTime = PublicDataClass._ProtocoltyFlag.DelayTime;
                PublicDataClass._ComStructData.TX_TASK = true;

            }
            else if (PublicDataClass._ComTaskFlag.C_IC_NA_1 == true)  //总召唤
            {
                PublicDataClass._ComTaskFlag.C_IC_NA_1 = false;
                PublicDataClass._ComStructData.TxLen = 0;

                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;

                PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(10);


                PublicDataClass.ComFrameMsg = "总召唤";
                ProtoDelayTime = PublicDataClass._ProtocoltyFlag.DelayTime;
                PublicDataClass._ProtocoltyFlag.ZZFlag = false;
                PublicDataClass._ComStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.ComThreadID = 9;
                PublicDataClass._ComStructData.FCB = (!PublicDataClass._ComStructData.FCB);
            
            }

            else if (PublicDataClass._ComTaskFlag.C_CS_NA_1 == true)
            {

                PublicDataClass._ComTaskFlag.C_CS_NA_1 = false;
                PublicDataClass._ComStructData.TxLen = 0;
                PublicDataClass.ZDYtype = 7;
                if (PublicDataClass.Framelen == 231)
                    PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(50);
                else 
                PublicDataClass._ComStructData.TxLen = protocoltyparam.EncodeFrame(7, PublicDataClass._ComStructData.TXBuffer, PublicDataClass._DataField.Buffer,
                                                      PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                      PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);

                PublicDataClass.ComFrameMsg = "对时";
                PublicDataClass._ComStructData.TX_TASK = true;

            }
            else if (PublicDataClass._ComTaskFlag.YK_Sel_1_D == true)
            {
                PublicDataClass._ComTaskFlag.YK_Sel_1_D = false;
                PublicDataClass._ComStructData.TxLen = 0;
                
                PublicDataClass._DataField.FieldVSQ = 1;
                PublicDataClass.ParamInfoAddr = PublicDataClass.YkStartPos;
                PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(15);

                if (PublicDataClass.YkState == 1)
                    PublicDataClass.ComFrameMsg = "选择<分>";
                else if (PublicDataClass.YkState == 2)
                    PublicDataClass.ComFrameMsg = "选择<合>";
                PublicDataClass._ComStructData.TX_TASK = true;
                PublicDataClass._ComStructData.FCB = (!PublicDataClass._ComStructData.FCB);
            }
            else if (PublicDataClass._ComTaskFlag.YK_Exe_1_D == true)
            {
                PublicDataClass._ComTaskFlag.YK_Exe_1_D = false;
                PublicDataClass._ComStructData.TxLen = 0;

                PublicDataClass._DataField.FieldVSQ = 1;
                PublicDataClass.ParamInfoAddr = PublicDataClass.YkStartPos;
                PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(15);

                if (PublicDataClass.YkState == 1)
                    PublicDataClass.ComFrameMsg = "执行<分>";
                else if (PublicDataClass.YkState == 2)
                    PublicDataClass.ComFrameMsg = "执行<合>";
                PublicDataClass._ComStructData.TX_TASK = true;
                PublicDataClass._ComStructData.FCB = (!PublicDataClass._ComStructData.FCB);
            }
            else if (PublicDataClass._ComTaskFlag.YK_Cel_1_D == true)
            {
                PublicDataClass._ComTaskFlag.YK_Cel_1_D = false;
                PublicDataClass._ComStructData.TxLen = 0;

                PublicDataClass._DataField.FieldVSQ = 1;
                PublicDataClass.ParamInfoAddr = PublicDataClass.YkStartPos;
                PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(16);

                if (PublicDataClass.YkState == 1)
                    PublicDataClass.ComFrameMsg = "撤销<分>";
                else if (PublicDataClass.YkState == 2)
                    PublicDataClass.ComFrameMsg = "撤销<合>";
                PublicDataClass._ComStructData.TX_TASK = true;
                PublicDataClass._ComStructData.FCB = (!PublicDataClass._ComStructData.FCB);
            }
            else if (PublicDataClass._ComTaskFlag.YK_Sel_1 == true)
            {
                PublicDataClass._ComTaskFlag.YK_Sel_1 = false;
                PublicDataClass._ComStructData.TxLen = 0;
                
                PublicDataClass._DataField.FieldVSQ = 1;
                PublicDataClass.ParamInfoAddr = PublicDataClass.YkStartPos;
                PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(17);

                if (PublicDataClass.YkState == 1)
                    PublicDataClass.ComFrameMsg = "选择<分>";
                else if (PublicDataClass.YkState == 2)
                    PublicDataClass.ComFrameMsg = "选择<合>";
                
                PublicDataClass._ComStructData.TX_TASK = true;
                PublicDataClass._ComStructData.FCB = (!PublicDataClass._ComStructData.FCB);
            }
            else if (PublicDataClass._ComTaskFlag.YK_Exe_1 == true)
            {
                PublicDataClass._ComTaskFlag.YK_Exe_1 = false;
                PublicDataClass._ComStructData.TxLen = 0;

                PublicDataClass._DataField.FieldVSQ = 1;
                PublicDataClass.ParamInfoAddr = PublicDataClass.YkStartPos;
                PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(17);

                if (PublicDataClass.YkState == 1)
                    PublicDataClass.ComFrameMsg = "执行<分>";
                else if (PublicDataClass.YkState == 2)
                    PublicDataClass.ComFrameMsg = "执行<合>";
               
                PublicDataClass._ComStructData.TX_TASK = true;
                PublicDataClass._ComStructData.FCB = (!PublicDataClass._ComStructData.FCB);
            }
            else if (PublicDataClass._ComTaskFlag.YK_Cel_1 == true)
            {
                PublicDataClass._ComTaskFlag.YK_Cel_1 = false;
                PublicDataClass._ComStructData.TxLen = 0;

                PublicDataClass._DataField.FieldVSQ = 1;
                PublicDataClass.ParamInfoAddr = PublicDataClass.YkStartPos;
                PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(18);

                if (PublicDataClass.YkState == 1)
                    PublicDataClass.ComFrameMsg = "撤销<分>";
                else if (PublicDataClass.YkState == 2)
                    PublicDataClass.ComFrameMsg = "撤销<合>";
               
                PublicDataClass._ComStructData.TX_TASK = true;
                PublicDataClass._ComStructData.FCB = (!PublicDataClass._ComStructData.FCB);
            }
            else if (PublicDataClass._ComTaskFlag.VERSION_1 == true)    //读版本号
            {
                PublicDataClass._ComTaskFlag.VERSION_1 = false;
                PublicDataClass._ComStructData.TxLen = 0;
                //PublicDataClass._ComStructData.TxLen = EncodeFrame(2, 10, ref PublicDataClass._ComStructData.TXBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                         //PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);
                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.seq = 1;
                PublicDataClass.SQflag = 0;
                PublicDataClass.ZDYtype = 5;
                if (PublicDataClass.Framelen == 231)
                    PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(50);
                else 
                PublicDataClass._ComStructData.TxLen = protocoltyparam.EncodeFrame(5, PublicDataClass._ComStructData.TXBuffer, PublicDataClass._DataField.Buffer,
                                                      PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                      PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);
                PublicDataClass.ComFrameMsg = "请求版本号";
                PublicDataClass._ComStructData.TX_TASK = true;

            }
            else if (PublicDataClass._ComTaskFlag.CALLTIME_1 == true)
            {
                PublicDataClass._ComTaskFlag.CALLTIME_1 = false;
                PublicDataClass._ComStructData.TxLen = 0;
                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.seq = 1;
                PublicDataClass.SQflag = 0;
                PublicDataClass.ZDYtype = 6;
                if (PublicDataClass.Framelen == 231)
                    PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(50);
                else 
                PublicDataClass._ComStructData.TxLen = protocoltyparam.EncodeFrame(6, PublicDataClass._ComStructData.TXBuffer, PublicDataClass._DataField.Buffer,
                                                      PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                      PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);

                PublicDataClass.ComFrameMsg = "请求时间";
                PublicDataClass._ComStructData.TX_TASK = true;
            }
            else if (PublicDataClass._ComTaskFlag.READ_P_1 == true)
            {
                PublicDataClass._ComTaskFlag.READ_P_1 = false;
                PublicDataClass._ComStructData.TxLen = 0;
                PublicDataClass.ZDYtype = 2;
                if (PublicDataClass.Framelen == 231)
                    PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(50);
                else 
                PublicDataClass._ComStructData.TxLen = protocoltyparam.EncodeFrame(2, PublicDataClass._ComStructData.TXBuffer, PublicDataClass._DataField.Buffer,
                                                      PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                      PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);

                PublicDataClass.ComFrameMsg = "读参数";
                PublicDataClass._ComStructData.TX_TASK = true;
            }
            else if (PublicDataClass._ComTaskFlag.SET_PARAM_CON == true)  //设置参数
            {
                PublicDataClass._ComTaskFlag.SET_PARAM_CON = false;
                PublicDataClass._ComStructData.TxLen = 0;
                //PublicDataClass._ComStructData.TxLen = EncodeFrame(2, 8, ref PublicDataClass._ComStructData.TXBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                         PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);
                PublicDataClass.ZDYtype = 1;
                if (PublicDataClass.Framelen == 231)
                    PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(50);
                else 
                PublicDataClass._ComStructData.TxLen = protocoltyparam.EncodeFrame(1, PublicDataClass._ComStructData.TXBuffer, PublicDataClass._DataField.Buffer,
                                                      PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                      PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);


                PublicDataClass.ComFrameMsg = "设置";
                PublicDataClass._ComStructData.TX_TASK = true;

            }
           
            else if (PublicDataClass._ComTaskFlag.AloneCallYx_1 == true)
            {
                PublicDataClass._ComTaskFlag.AloneCallYx_1 = false;
                PublicDataClass._ComStructData.TxLen = 0;
                //PublicDataClass._ComStructData.TxLen = EncodeFrame(2, 12, ref PublicDataClass._ComStructData.TXBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                          PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);
                PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(39);
                PublicDataClass.ComFrameMsg = "请求遥信";
                //发送单招
                PublicDataClass._ChangeFlag.AlongCall = true;
                AlongCallTime = 0;
          
                PublicDataClass._ComStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.ComThreadID = 11;
            }
            else if (PublicDataClass._ComTaskFlag.AloneCallYc_1 == true)
            {
                PublicDataClass._ComTaskFlag.AloneCallYc_1 = false;
                PublicDataClass._ComStructData.TxLen = 0;
                //PublicDataClass._ComStructData.TxLen = EncodeFrame(2, 13, ref PublicDataClass._ComStructData.TXBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                          PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);
                PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(38);
                PublicDataClass.ComFrameMsg = "请求遥测";
                //发送单招
                PublicDataClass._ChangeFlag.AlongCall = true;
                AlongCallTime = 0;

                PublicDataClass._ComStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.ComThreadID = 12;

            }
            else if (PublicDataClass._ComTaskFlag.Reset_1 == true)
            {
                PublicDataClass._ComTaskFlag.Reset_1 = false;
                PublicDataClass._ComStructData.TxLen = 0;
                //PublicDataClass._ComStructData.TxLen = EncodeFrame(2, 17, ref PublicDataClass._ComStructData.TXBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                          PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);
                PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(13);

                PublicDataClass.ComFrameMsg = "复位";
                PublicDataClass._ComStructData.TX_TASK = true;
            }
            else if (PublicDataClass._ComTaskFlag.UpdateCode_Start_1 == true)     
            {
                PublicDataClass._ComTaskFlag.UpdateCode_Start_1 = false;
                PublicDataClass._ComStructData.TxLen = 0;

                //PublicDataClass._ComStructData.TxLen = EncodeFrame(2, 22, ref PublicDataClass._ComStructData.TXBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                              PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);
                PublicDataClass._DataField.FieldVSQ = 1;
                PublicDataClass.ZDYtype = 3;
                if (PublicDataClass.Framelen == 231)
                    PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(50);
                else 
                PublicDataClass._ComStructData.TxLen = protocoltyparam.EncodeFrame(3, PublicDataClass._ComStructData.TXBuffer, PublicDataClass._DataField.Buffer,
                                                      PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                      PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);


                PublicDataClass.ComFrameMsg = "升级";
                PublicDataClass._ComStructData.TX_TASK = true;
            }
            else if (PublicDataClass._ComTaskFlag.UpdateCode_ARMStart_1 == true)
            {
                PublicDataClass._ComTaskFlag.UpdateCode_ARMStart_1 = false;
                PublicDataClass._ComStructData.TxLen = 0;

                //PublicDataClass._ComStructData.TxLen = EncodeFrame(2, 22, ref PublicDataClass._ComStructData.TXBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                              PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);
                PublicDataClass._DataField.FieldVSQ = 1;
                PublicDataClass.ZDYtype = 9;
                if (PublicDataClass.Framelen == 231)
                    PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(50);
                else
                    PublicDataClass._ComStructData.TxLen = protocoltyparam.EncodeFrame(9, PublicDataClass._ComStructData.TXBuffer, PublicDataClass._DataField.Buffer,
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                          PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);


                PublicDataClass.ComFrameMsg = "ARM升级";
                PublicDataClass._ComStructData.TX_TASK = true;
            }
           
            else if (PublicDataClass._ComTaskFlag.UpdateCode_JY_1 ==true)
            {
                PublicDataClass._ComTaskFlag.UpdateCode_JY_1 = false;
                PublicDataClass._ComStructData.TxLen = 0;
              
                //{
                //    PublicDataClass._ComStructData.TxLen = EncodeFrame(2, 22, ref PublicDataClass._ComStructData.TXBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                              PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);
                //}
                PublicDataClass._DataField.FieldVSQ = 1;
                PublicDataClass.ZDYtype = 3;
                if (PublicDataClass.Framelen == 231)
                    PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(50);
                else 
                PublicDataClass._ComStructData.TxLen = protocoltyparam.EncodeFrame(3, PublicDataClass._ComStructData.TXBuffer, PublicDataClass._DataField.Buffer,
                                                     PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                     PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);

                PublicDataClass.ComFrameMsg = "校验";
                PublicDataClass._ComStructData.TX_TASK = true;

            }
            else if (PublicDataClass._ComTaskFlag.UpdateCode_ARMJY_1 == true)
            {
                PublicDataClass._ComTaskFlag.UpdateCode_ARMJY_1 = false;
                PublicDataClass._ComStructData.TxLen = 0;

                //{
                //    PublicDataClass._ComStructData.TxLen = EncodeFrame(2, 22, ref PublicDataClass._ComStructData.TXBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                              PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);
                //}
                PublicDataClass._DataField.FieldVSQ = 1;
                PublicDataClass.ZDYtype = 9;
                if (PublicDataClass.Framelen == 231)
                    PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(50);
                else
                    PublicDataClass._ComStructData.TxLen = protocoltyparam.EncodeFrame(9, PublicDataClass._ComStructData.TXBuffer, PublicDataClass._DataField.Buffer,
                                                         PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                         PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);

                PublicDataClass.ComFrameMsg = "ARM校验";
                PublicDataClass._ComStructData.TX_TASK = true;

            }
           
            else if (PublicDataClass._ComTaskFlag.UpdateCode_OK_1 == true)
            {
                PublicDataClass._ComTaskFlag.UpdateCode_OK_1 = false;
                PublicDataClass._ComStructData.TxLen = 0;
               
           
                 //PublicDataClass._ComStructData.TxLen = EncodeFrame(2, 22, ref PublicDataClass._ComStructData.TXBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                 //                                             PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);
                PublicDataClass._DataField.FieldVSQ = 1;
                PublicDataClass.ZDYtype = 3;
                if (PublicDataClass.Framelen == 231)
                    PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(50);
                else 
                PublicDataClass._ComStructData.TxLen = protocoltyparam.EncodeFrame(3, PublicDataClass._ComStructData.TXBuffer, PublicDataClass._DataField.Buffer,
                                                    PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                    PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);

        
                PublicDataClass.ComFrameMsg = "更新代码";
                PublicDataClass._ComStructData.TX_TASK = true;

            }
            else if (PublicDataClass._ComTaskFlag.UpdateCode_ARMOK_1 == true)
            {
                PublicDataClass._ComTaskFlag.UpdateCode_ARMOK_1 = false;
                PublicDataClass._ComStructData.TxLen = 0;


                //PublicDataClass._ComStructData.TxLen = EncodeFrame(2, 22, ref PublicDataClass._ComStructData.TXBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                             PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);
                PublicDataClass._DataField.FieldVSQ = 1;
                PublicDataClass.ZDYtype = 9;
                if (PublicDataClass.Framelen == 231)
                    PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(50);
                else
                    PublicDataClass._ComStructData.TxLen = protocoltyparam.EncodeFrame(9, PublicDataClass._ComStructData.TXBuffer, PublicDataClass._DataField.Buffer,
                                                        PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                        PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);


                PublicDataClass.ComFrameMsg = "ARM更新代码";
                PublicDataClass._ComStructData.TX_TASK = true;

            }
            
            
            else if (PublicDataClass._ComTaskFlag.CallRecordData == true)
            {
                PublicDataClass._ComTaskFlag.CallRecordData = false;
                PublicDataClass._ComStructData.TxLen = 0;
                //PublicDataClass._ComStructData.TxLen = EncodeFrame(2, 20, ref PublicDataClass._ComStructData.TXBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                //                                          PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);
                PublicDataClass.ZDYtype = 4;
                if (PublicDataClass.Framelen == 231)
                    PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(50);
                else 
                PublicDataClass._ComStructData.TxLen = protocoltyparam.EncodeFrame(4, PublicDataClass._ComStructData.TXBuffer, PublicDataClass._DataField.Buffer,
                                                      PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                      PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);
                PublicDataClass.ComFrameMsg = "请求记录数据";
                PublicDataClass._ComStructData.TX_TASK = true;
            }
           

            else if (PublicDataClass._ComTaskFlag.READ_Hard_1 == true)
            {

                PublicDataClass._ComTaskFlag.READ_Hard_1 = false;
                PublicDataClass._ComStructData.TxLen = 0;
                
                //PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(37);
                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.seq = 1;
                PublicDataClass.SQflag = 0;
                PublicDataClass.ZDYtype = 8;
                if (PublicDataClass.Framelen == 231)
                    PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(50);
                else 
                PublicDataClass._ComStructData.TxLen = protocoltyparam.EncodeFrame(8, PublicDataClass._ComStructData.TXBuffer, PublicDataClass._DataField.Buffer,
                                                      PublicDataClass._DataField.FieldLen, PublicDataClass.DevAddr, PublicDataClass.addselect, PublicDataClass.seqflag, PublicDataClass.seq,
                                                      PublicDataClass.SQflag, PublicDataClass._DataField.FieldVSQ, PublicDataClass.ParamInfoAddr);

                PublicDataClass.ComFrameMsg = "请求硬件状态";
                PublicDataClass._ComStructData.TX_TASK = true;

            }
            else if (PublicDataClass._ComTaskFlag.Transmit == true)
            {

                PublicDataClass._ComTaskFlag.Transmit = false;
                PublicDataClass._ComStructData.TxLen = 0;
                PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(40);

                PublicDataClass.ComFrameMsg = "数据转发";
                PublicDataClass._ComStructData.TX_TASK = true;

            }
            else if (PublicDataClass._ComTaskFlag.CALL_1 == true)  //
            {
              
                PublicDataClass._ComTaskFlag.CALL_1 = false;
                if (PublicDataClass._ProtocoltyFlag.AloneCallYxYcFlag == false)
                {
                    PublicDataClass._ComStructData.TxLen = 0;

                    PublicDataClass._DataField.FieldLen = 0;
                    PublicDataClass._DataField.FieldVSQ = 1;

                    PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(3);


                    PublicDataClass.ComFrameMsg = "召唤一级数据";
                    ProtoDelayTime = PublicDataClass._ProtocoltyFlag.DelayTime;

                    PublicDataClass._ComStructData.TX_TASK = true;
                    PublicDataClass._ComStructData.FCB = (!PublicDataClass._ComStructData.FCB);
                    //    PublicDataClass._ThreadIndex.ComThreadID = 9;
                }
            }
            else if (PublicDataClass._ComTaskFlag.CALL_2 == true)  //
            {
                if (PublicDataClass._ProtocoltyFlag.AloneCallYxYcFlag == false)
                {
                    PublicDataClass._ComTaskFlag.CALL_2 = false;
                    PublicDataClass._ComStructData.TxLen = 0;

                    PublicDataClass._DataField.FieldLen = 0;
                    PublicDataClass._DataField.FieldVSQ = 1;

                    PublicDataClass._ComStructData.TxLen = protocolty101.EncodeFrame(4);


                    PublicDataClass.ComFrameMsg = "召唤二级数据";
                    ProtoDelayTime = PublicDataClass._ProtocoltyFlag.DelayTime;
                    PublicDataClass._ComStructData.TX_TASK = true;
                    PublicDataClass._ComStructData.FCB = (!PublicDataClass._ComStructData.FCB);
                    //     PublicDataClass._ThreadIndex.ComThreadID = 9;
                }
            }
            
           
            
        }
        //-----------------------------------------------------------------解包
        /// <summary>
        /// 串口数据的解包
        /// </summary>
        public static void ParseComReceData()
        {
            byte dataty;
            PublicDataClass.DataTy = 0;
            int DevAddr = 0;
            byte TargetBoard = 0, seqflag = 0, seq = 0, SQflag = 0;

            if (PublicDataClass._ComStructData.RXBuffer[0] != 0x69)
            //PublicDataClass.DataTy = DecodeFrame(2, ref PublicDataClass._ComStructData.RXBuffer[0], ref PublicDataClass._DataField.Buffer[0],
            //       ref PublicDataClass._DataField.FieldLen, ref PublicDataClass._DataField.FieldVSQ, ref PublicDataClass.ParamInfoAddr, PublicDataClass._ComStructData.RxLen, ref STxSeq, ref SRxSeq);
            {
                if (PublicDataClass._ProtocoltyFlag.ComProFlag == 2)//串口走104规约
                {
                    //思路：串口网口buffer和消息转换采用网口解包函数进行处理
                    RBufferComToNet();
                    ParseNetReceData();
                    PublicDataClass.ComFrameMsg = PublicDataClass.RevNetFrameMsg;
                    return;
                }
                else 
                //串口走101规约
                PublicDataClass.DataTy = protocolty101.DecodeFrame();

            }
            else
            {
                PublicDataClass.DataTy = protocoltyparam.DecodeFrame(PublicDataClass._ComStructData.RXBuffer, PublicDataClass._ComStructData.RxLen, PublicDataClass._DataField.Buffer, ref PublicDataClass._DataField.FieldLen, ref DevAddr, ref TargetBoard, ref PublicDataClass.seqflag, ref PublicDataClass.seq, ref PublicDataClass.SQflag, ref PublicDataClass._DataField.FieldVSQ, ref PublicDataClass.ParamInfoAddr);
            }
            if (PublicDataClass.DataTy  == 0)     //非法帧
            {
                PublicDataClass.ComFrameMsg = "无效";
            }
            else if (PublicDataClass.DataTy == 2)  //链路状态正常
            {
                if (PublicDataClass._ProtocoltyFlag.Test101 == true)//101流程测试
           
                {
                    if (PublicDataClass._ProtocoltyFlag.ComProFlag == 1)

                        PublicDataClass._ComTaskFlag.C_RQ_NA_LINKREQ_F1 = true;  //链路复位
                    if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                        PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_F1 = true;  //链路复位
                }
                PublicDataClass.ComFrameMsg = "链路状态正常";
                return;
            }
            else if (PublicDataClass.DataTy == 3)  //复位链路确认
            {
    
                
                    if (PublicDataClass._ProtocoltyFlag.Test101 == true)//101流程测试
                    {
                        if (PublicDataClass._ProtocoltyFlag.ZZFirstFlag == true)
                        {
                            PublicDataClass._ProtocoltyFlag.ZZFirstFlag = false;//清第一次总召标志
                            PublicDataClass._ProtocoltyFlag.ZZFlag = true;//发送总召唤标志
                            if (PublicDataClass._ProtocoltyFlag.ComProFlag == 1)
                                PublicDataClass._ComTaskFlag.C_IC_NA_1 = true;  //发总召唤
                            if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)
                                PublicDataClass._NetTaskFlag .C_IC_NA_1 = true;  //发总召唤
                            ProtoZZTime = PublicDataClass._ProtocoltyFlag.ZZTime;//设置总召唤循环时间
                        }

                    }
                
                PublicDataClass.ComFrameMsg = "确认";
            }
            else if (PublicDataClass.DataTy == 4)     //  总召激活
            {
                PublicDataClass.ComFrameMsg = "总召激活";
            
            }
            else if (PublicDataClass.DataTy == 5)     //  总召结束
            {
                PublicDataClass.ComFrameMsg = "总召结束";
              
            }
            else if (PublicDataClass.DataTy == 35)     //  遥测（单独召测) 短浮点型，5字节
            {
                PublicDataClass.ComFrameMsg = "遥测";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (PublicDataClass.DataTy == 40)     //   短浮点型，5字节
            {
                PublicDataClass.ComFrameMsg = "遥测";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (PublicDataClass.DataTy == 36)     // 带品质描述归一化值 ，3字节
            {
                PublicDataClass.ComFrameMsg = "遥测";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (PublicDataClass.DataTy == 38)     // 带品质描述归一化值 ，3字节
            {
                PublicDataClass.ComFrameMsg = "遥测";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (PublicDataClass.DataTy == 42)     //不带品质描述归一化值，2字节
            {
                PublicDataClass.ComFrameMsg = "遥测";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (PublicDataClass.DataTy == 41)     //           8字节
            {
                PublicDataClass.ComFrameMsg = "扰动事件";
                PublicDataClass._ChangeFlag.ChangInfoViewUpdate = true;
                PublicDataClass._Message.RaoDongEvent = true;
                //PublicDataClass._FrameTime.T2 = 10;
            }
            else if (PublicDataClass.DataTy == 37)     //          6字节
            {
                PublicDataClass.ComFrameMsg = "扰动事件";
                PublicDataClass._ChangeFlag.ChangInfoViewUpdate = true;
                PublicDataClass._Message.RaoDongEvent = true;
                //PublicDataClass._FrameTime.T2 = 10;
            }
            else if (PublicDataClass.DataTy == 39)     //          6字节
            {
                PublicDataClass.ComFrameMsg = "扰动事件";
                PublicDataClass._ChangeFlag.ChangInfoViewUpdate = true;
                PublicDataClass._Message.RaoDongEvent = true;
                //PublicDataClass._FrameTime.T2 = 10;
            }
            else if (PublicDataClass.DataTy == 43)  //         5字节
            {
                PublicDataClass.ComFrameMsg = "扰动事件";
                PublicDataClass._ChangeFlag.ChangInfoViewUpdate = true;
                PublicDataClass._Message.RaoDongEvent = true;
                //PublicDataClass._FrameTime.T2 = 10;
            }

            else if (PublicDataClass.DataTy == 50)    //遥信（单独召测)
            {
                PublicDataClass.ComFrameMsg = "遥信";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (PublicDataClass.DataTy == 51)    //正常遥信
            {
                PublicDataClass.ComFrameMsg = "遥信";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (PublicDataClass.DataTy == 52)     //遥信变位
            {
                PublicDataClass.ComFrameMsg = "变位事件";
                PublicDataClass._ChangeFlag.ChangInfoViewUpdate = true;
                PublicDataClass._Message.YxBianWeiOfNoTimeEvent = true;

            }
            else if (PublicDataClass.DataTy == 53)    //正常双点遥信
            {
                PublicDataClass.ComFrameMsg = "遥信";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (PublicDataClass.DataTy == 54)     //双点变位事件
            {
                PublicDataClass.ComFrameMsg = "变位事件";
                PublicDataClass._ChangeFlag.ChangInfoViewUpdate = true;
                PublicDataClass._Message.YxBianWeiOfNoTimeEvent = true;

            }
            else if (PublicDataClass.DataTy == 56)
            {
                PublicDataClass.ComFrameMsg = "变位事件";
                PublicDataClass._ChangeFlag.ChangInfoViewUpdate = true;
                PublicDataClass._Message.YxBianWeiOfTimeEvent = true;
            }
            else if (PublicDataClass.DataTy == 58)
            {
                PublicDataClass.ComFrameMsg = "变位事件";
                PublicDataClass._ChangeFlag.ChangInfoViewUpdate = true;
                PublicDataClass._Message.YxBianWeiOfTimeEvent = true;
            }
            else if (PublicDataClass.DataTy  == 10)     //
            {
                PublicDataClass.ComFrameMsg = "选择应答";
                PublicDataClass._Message.YkDocmentView = true;
            }
            else if (PublicDataClass.DataTy  == 11)     //
            {
                PublicDataClass.ComFrameMsg = "执行成功";
                PublicDataClass._Message.YkDocmentView = true;
            }
            else if (PublicDataClass.DataTy  == 12)     //
            {
                PublicDataClass.ComFrameMsg = "遥控撤销（确认）";
                PublicDataClass._Message.YkDocmentView = true;
            }
            else if (PublicDataClass.DataTy  == 7)
            {
                PublicDataClass.ComFrameMsg = "复位确认";

            }   
            //else if (PublicDataClass.DataTy == 26)
            //{
            //    PublicDataClass.ComFrameMsg = "器件状态";
            //    PublicDataClass._Message.CallDataDocmentView = true;
            //}
           
            else if (PublicDataClass.DataTy == 100)     //
            {
                PublicDataClass.ComFrameMsg = "参数设置（确认）";
                PublicDataClass._Message.ParamAck = true;
                
            }
            else if (PublicDataClass.DataTy == 101)     //
            {
                PublicDataClass.ComFrameMsg = "参数设置（否认）";
               
            }
            else if (PublicDataClass.DataTy == 102)   //
            {
                PublicDataClass.ComFrameMsg = "参数读取";
                PublicDataClass._Message.ReadParam = true;
            }
            else if (PublicDataClass.DataTy == 103)  //升级
            {
                if (PublicDataClass.ParamInfoAddr < 1000)
                    PublicDataClass.ComFrameMsg = "升级(应答)";
                else if (PublicDataClass.ParamInfoAddr == 1000)
                {
                    PublicDataClass.ComFrameMsg = "校验(应答)";
                    PublicDataClass._Message.CodeUpdateJY = true;
                }
            }
            else if (PublicDataClass.DataTy == 104)  //记录
            {
                PublicDataClass.ComFrameMsg = "历史记录";
                PublicDataClass._Message.CallHisDataDocmentView = true;
            }
            else if (PublicDataClass.DataTy == 105)     //版本号
            {
                PublicDataClass.ComFrameMsg = "版本号";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (PublicDataClass.DataTy == 106)  
            { 
                PublicDataClass.ComFrameMsg = "时间";  //时间
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (PublicDataClass.DataTy == 108)   //器件状态
            {
                PublicDataClass.ComFrameMsg = "器件状态";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (PublicDataClass.DataTy == 109)  //升级
            {
                if (PublicDataClass.ParamInfoAddr < 1000)
                    PublicDataClass.ComFrameMsg = "升级(应答)";
                else if (PublicDataClass.ParamInfoAddr == 1000)
                {
                    PublicDataClass.ComFrameMsg = "校验(应答)";
                    PublicDataClass._Message.CodeUpdateJY = true;
                }
            }
            else if (PublicDataClass.DataTy == 29)     //
            {
                PublicDataClass.ComFrameMsg = "转发回复";

            }
            else if (PublicDataClass.DataTy == 114)
            {
                PublicDataClass.ComFrameMsg = "无所召唤的数据";

            }
            else if (PublicDataClass.DataTy == 115)
            {
                PublicDataClass.ComFrameMsg = "以数据响应";

            }
    //        if (PublicDataClass._ProtocoltyFlag.Test101 == true)
            if (PublicDataClass._ProtocoltyFlag.ZZFlag == false && PublicDataClass._ProtocoltyFlag.AloneCallYxYcFlag==false )//发总召过程中时停止召唤一级二级数据,单招遥信遥测时停止召唤一级二级数据
            {
                if (PublicDataClass._ProtocoltyFlag.ACD == 1)
                {
                   
                    PublicDataClass._ComTaskFlag.CALL_1 = true;
                }
                else if (PublicDataClass._ProtocoltyFlag.ACD == 2)
                {
                    
                    PublicDataClass._ComTaskFlag.CALL_2 = true;
                }
            }
          
        }
      /*****************************************************************************************************************************
      *                                                          GPRS的打包解包操作                                                *
      *                                                                                                                            *
      *  功能  ：    数据帧类型处理                                                                                                *
      *  参数  ：    无                                                                                                            *
      *  返回值：    无                                                                                                            *
      *  修改日期：  2010-12-07                                                                                                    *
      *  作者    ：  cuibj                                                                                                         *
      * ***************************************************************************************************************************/
        public static void PtlGprsFrameProc()
        {
            if (!PublicDataClass._GprsTaskFlag.FirstON_S)
            {
                //	第一次开机处理
                PublicDataClass._GprsTaskFlag.FirstON_S = true;		//	主机开机处理过标志(正常情况下该标志一直为1)
                PublicDataClass._GprsTaskFlag.C_P1_NA_F = false;     //104U格式中STARTDT命令
                PublicDataClass._GprsTaskFlag.C_RQ_NA_LINKREQ_F = false;
                return;
            }
            if (PublicDataClass._GprsTaskFlag.C_RQ_NA_LINKREQ_F == true)    //链路复位
            {
                PublicDataClass._GprsTaskFlag.C_RQ_NA_LINKREQ_F = false;
                PublicDataClass._GprsStructData.GprsTLen = 0;
                //PublicDataClass._ComStructData.TxLen = EncodeFrame(1, 0, 0, ref PublicDataClass._ComStructData.TXBuffer[0],ref PublicDataClass._DataField.Buffer[0]);
                PublicDataClass._GprsStructData.TX_TASK = true;

            }
            else if (PublicDataClass._GprsTaskFlag.C_CS_NA_1 == true)
            {

                PublicDataClass._GprsTaskFlag.C_CS_NA_1 = false;
                PublicDataClass._GprsStructData.GprsTLen = 0;
                PublicDataClass._GprsStructData.GprsTLen = EncodeFrame(2, 4, ref PublicDataClass._GprsStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.GprsFrameMsg = "对时";
                PublicDataClass._GprsStructData.TX_TASK = true;

            }
            else if (PublicDataClass._GprsTaskFlag.YK_Sel_1 == true)
            {
                PublicDataClass._GprsTaskFlag.YK_Sel_1 = false;
                PublicDataClass._GprsStructData.GprsTLen = 0;
                PublicDataClass._GprsStructData.GprsTLen = EncodeFrame(2, 5, ref PublicDataClass._GprsStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.YkStartPos, MTxSeq, MRxSeq);

                if (PublicDataClass.YkState == 1)
                    PublicDataClass.GprsFrameMsg = "选择<分>";
                else if (PublicDataClass.YkState == 2)
                    PublicDataClass.GprsFrameMsg = "选择<合>";
                PublicDataClass._GprsStructData.TX_TASK = true;
            }
            else if (PublicDataClass._GprsTaskFlag.YK_Exe_1 == true)
            {
                PublicDataClass._GprsTaskFlag.YK_Exe_1 = false;
                PublicDataClass._GprsStructData.GprsTLen = 0;
                PublicDataClass._GprsStructData.GprsTLen = EncodeFrame(2, 6, ref PublicDataClass._GprsStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.YkStartPos, MTxSeq, MRxSeq);

                if (PublicDataClass.YkState == 1)
                    PublicDataClass.GprsFrameMsg = "执行<分>";
                else if (PublicDataClass.YkState == 2)
                    PublicDataClass.GprsFrameMsg = "执行<合>";
                PublicDataClass._GprsStructData.TX_TASK = true;
            }
            else if (PublicDataClass._GprsTaskFlag.YK_Cel_1 == true)
            {
                PublicDataClass._GprsTaskFlag.YK_Cel_1 = false;
                PublicDataClass._GprsStructData.GprsTLen = 0;
                PublicDataClass._GprsStructData.GprsTLen = EncodeFrame(2, 7, ref PublicDataClass._GprsStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.YkStartPos, MTxSeq, MRxSeq);

                if (PublicDataClass.YkState == 1)
                    PublicDataClass.GprsFrameMsg = "撤销<分>";
                else if (PublicDataClass.YkState == 2)
                    PublicDataClass.GprsFrameMsg = "撤销<合>";
                PublicDataClass._GprsStructData.TX_TASK = true;
            }
            else if (PublicDataClass._GprsTaskFlag.VERSION_1 == true)    //读版本号
            {
                PublicDataClass._GprsTaskFlag.VERSION_1 = false;
                PublicDataClass._GprsStructData.GprsTLen = 0;
                PublicDataClass._GprsStructData.GprsTLen = EncodeFrame(2, 10, ref PublicDataClass._GprsStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.GprsFrameMsg = "请求版本号";
                PublicDataClass._GprsStructData.TX_TASK = true;

            }
            else if (PublicDataClass._GprsTaskFlag.CALLTIME_1 == true)
            {
                PublicDataClass._GprsTaskFlag.CALLTIME_1 = false;
                PublicDataClass._GprsStructData.GprsTLen = 0;
                PublicDataClass._GprsStructData.GprsTLen = EncodeFrame(2, 11, ref PublicDataClass._GprsStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.GprsFrameMsg = "请求时间";
                PublicDataClass._GprsStructData.TX_TASK = true;
            }
            else if (PublicDataClass._GprsTaskFlag.READ_P_1 == true)
            {
                PublicDataClass._GprsTaskFlag.READ_P_1 = false;
                PublicDataClass._GprsStructData.GprsTLen = 0;
                PublicDataClass._GprsStructData.GprsTLen = EncodeFrame(2, 9, ref PublicDataClass._GprsStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.GprsFrameMsg = "读参数";
                PublicDataClass._GprsStructData.TX_TASK = true;
            }
            else if (PublicDataClass._GprsTaskFlag.SET_PARAM_CON == true)  //设置参数
            {
                PublicDataClass._GprsTaskFlag.SET_PARAM_CON = false;
                PublicDataClass._GprsStructData.GprsTLen = 0;
                PublicDataClass._GprsStructData.GprsTLen = EncodeFrame(2, 8, ref PublicDataClass._GprsStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                         PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);
                PublicDataClass.GprsFrameMsg = "设置";
                PublicDataClass._GprsStructData.TX_TASK = true;

            }
            else if (PublicDataClass._GprsTaskFlag.AloneCallYx_1 == true)
            {
                PublicDataClass._GprsTaskFlag.AloneCallYx_1 = false;
                PublicDataClass._GprsStructData.GprsTLen = 0;
                PublicDataClass._GprsStructData.GprsTLen = EncodeFrame(2, 12, ref PublicDataClass._GprsStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.GprsFrameMsg = "请求遥信";
                PublicDataClass._GprsStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.GprsThreadID = 11;
            }
            else if (PublicDataClass._GprsTaskFlag.AloneCallYc_1 == true)
            {
                PublicDataClass._GprsTaskFlag.AloneCallYc_1 = false;
                PublicDataClass._GprsStructData.GprsTLen = 0;
                PublicDataClass._GprsStructData.GprsTLen = EncodeFrame(2, 13, ref PublicDataClass._GprsStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.GprsFrameMsg = "请求遥测";
                PublicDataClass._GprsStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.GprsThreadID = 12;

            }
            else if (PublicDataClass._GprsTaskFlag.Reset_1 == true)
            {
                PublicDataClass._GprsTaskFlag.Reset_1 = false;
                PublicDataClass._GprsStructData.GprsTLen = 0;
                PublicDataClass._GprsStructData.GprsTLen = EncodeFrame(2, 17, ref PublicDataClass._GprsStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.GprsFrameMsg = "复位";
                PublicDataClass._GprsStructData.TX_TASK = true;
            }
            else if (PublicDataClass._GprsTaskFlag.UpdateCode_Start_1 == true)
            {
                PublicDataClass._GprsTaskFlag.UpdateCode_Start_1 = false;
                PublicDataClass._GprsStructData.GprsTLen = 0;
                PublicDataClass._GprsStructData.GprsTLen = EncodeFrame(2, 14, ref PublicDataClass._GprsStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.GprsFrameMsg = "升级";
                PublicDataClass._GprsStructData.TX_TASK = true;
            }
            else if (PublicDataClass._GprsTaskFlag.UpdateCode_JY_1 == true)
            {
                PublicDataClass._GprsTaskFlag.UpdateCode_JY_1 = false;
                PublicDataClass._GprsStructData.GprsTLen = 0;
                PublicDataClass._GprsStructData.GprsTLen = EncodeFrame(2, 15, ref PublicDataClass._GprsStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.GprsFrameMsg = "校验";
                PublicDataClass._GprsStructData.TX_TASK = true;

            }
            else if (PublicDataClass._GprsTaskFlag.UpdateCode_OK_1 == true)
            {
                PublicDataClass._GprsTaskFlag.UpdateCode_OK_1 = false;
                PublicDataClass._GprsStructData.GprsTLen = 0;
                PublicDataClass._GprsStructData.GprsTLen = EncodeFrame(2, 16, ref PublicDataClass._GprsStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.GprsFrameMsg = "更新代码";
                PublicDataClass._GprsStructData.TX_TASK = true;

            }
            else if (PublicDataClass._GprsTaskFlag.AloneCallYx_2 == true)
            {
                PublicDataClass._GprsTaskFlag.AloneCallYx_2 = false;
                PublicDataClass._GprsStructData.GprsTLen = 0;
                PublicDataClass._GprsStructData.GprsTLen = EncodeFrame(2, 18, ref PublicDataClass._GprsStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.GprsFrameMsg = "内遥信";
                PublicDataClass._GprsStructData.TX_TASK = true;

            }
            else if (PublicDataClass._GprsTaskFlag.CallHisData == true)
            {

                PublicDataClass._GprsTaskFlag.CallHisData = false;
                PublicDataClass._GprsStructData.GprsTLen = 0;
                PublicDataClass._GprsStructData.GprsTLen = EncodeFrame(2, 19, ref PublicDataClass._GprsStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.GprsFrameMsg = "请求历史数据";
                PublicDataClass._GprsStructData.TX_TASK = true;

            }
            else if (PublicDataClass._GprsTaskFlag.CallRecordData == true)
            {

                PublicDataClass._GprsTaskFlag.CallRecordData = false;
                PublicDataClass._GprsStructData.GprsTLen = 0;
                PublicDataClass._GprsStructData.GprsTLen = EncodeFrame(2, 20, ref PublicDataClass._GprsStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.GprsFrameMsg = "请求记录数据";
                PublicDataClass._GprsStructData.TX_TASK = true;

            }
            else if (PublicDataClass._GprsTaskFlag.READ_Hard_1 == true)
            {

                PublicDataClass._GprsTaskFlag.READ_Hard_1 = false;
                PublicDataClass._GprsStructData.GprsTLen = 0;
                PublicDataClass._GprsStructData.GprsTLen = EncodeFrame(2, 30, ref PublicDataClass._GprsStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.GprsFrameMsg = "请求硬件状态";
                PublicDataClass._GprsStructData.TX_TASK = true;

            }
            else if (PublicDataClass._GprsTaskFlag.FuGuiCmd == true)
            {
                PublicDataClass._GprsTaskFlag.FuGuiCmd = false;
                PublicDataClass._GprsStructData.GprsTLen= 0;
                PublicDataClass._GprsStructData.GprsTLen = EncodeFrame(2, 31, ref PublicDataClass._ComStructData.TXBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                         PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.GprsFrameMsg = "复归命令";
                PublicDataClass._GprsStructData.TX_TASK = true;

            }


        }
        public static void ParseGprsReceData()  //-------------------------------------------gprs解包
        {
            byte dataty;
            dataty = DecodeFrame(2, ref PublicDataClass._GprsStructData.RBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                   ref PublicDataClass._DataField.FieldLen, ref PublicDataClass._DataField.FieldVSQ, ref PublicDataClass.ParamInfoAddr, PublicDataClass._GprsStructData.GprsRLen, ref STxSeq, ref SRxSeq);
            if (dataty == 0)     //非法帧
            {
                PublicDataClass.GprsFrameMsg = "无效";
            }
            else if (dataty == 9)
            {
                PublicDataClass.GprsFrameMsg = "时间";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (dataty == 10)     //
            {
                PublicDataClass.GprsFrameMsg = "选择应答";
                PublicDataClass._Message.YkDocmentView = true;
            }
            else if (dataty == 11)     //
            {
                PublicDataClass.GprsFrameMsg = "执行成功";
                PublicDataClass._Message.YkDocmentView = true;
            }
            else if (dataty == 12)     //
            {
                PublicDataClass.GprsFrameMsg = "遥控撤销";
                PublicDataClass._Message.YkDocmentView = true;
            }

            else if (dataty == 13)
            {
                PublicDataClass.GprsFrameMsg = "设置(确认)";
                PublicDataClass._Message.ParamAck = true;
            }
            else if (dataty == 14)
            {
                PublicDataClass.GprsFrameMsg = "设置(否认)";

            }
            else if (dataty == 15)
            {
                PublicDataClass.GprsFrameMsg = "参数读取";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (dataty == 16)     //
            {
                PublicDataClass.GprsFrameMsg = "版本号";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (dataty == 17)
            {
                PublicDataClass.GprsFrameMsg = "遥信";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (dataty == 18)
            {
                PublicDataClass.GprsFrameMsg = "遥测";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (dataty == 19)
            {
                PublicDataClass.GprsFrameMsg = "升级(应答)";
            }
            else if (dataty == 20)
            {
                PublicDataClass.GprsFrameMsg = "校验(应答)";
                PublicDataClass._Message.CodeUpdateJY = true;
            }
            else if (dataty == 22)
            {
                PublicDataClass.GprsFrameMsg = "复位否认";

            }
        }

   
       
        /// <summary>
        /// 自定义的连接socket线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param anthor="cuibj"></param>
        /// <param Time="11-04-28"></param>
        private void ConnThreadSendProc()          //连接socket线程
        {

            while (true)
            {
                try
                         {
                   
                    conn.Connect(IPAddress.Parse(PublicDataClass.IPADDR), Convert.ToInt16(PublicDataClass.PORT));
                    
                    PublicDataClass.TcpLinkType = 2;
                    ConConectTime=0;


                    PublicDataClass._Message.TcpLinkState = true;
                    LinkType = 1;
                    PublicDataClass._Message.LinkInfo     = true;
                    PublicDataClass._FrameTime.T3 = 20;
                 
                    break;

                }
                catch
                {

                    PublicDataClass.TcpLinkType = 1;
                    PublicDataClass._Message.TcpLinkState = true;
                    LinkType = 4;
                 
                    PublicDataClass._Message.LinkInfo = true ;


                }
                Thread.Sleep(20000);   //20s
            }

        }
     

        /*****************************************************************************************************************************
    *                                                          USB的打包解包操作                                                *
    *                                                                                                                            *
    *  功能  ：    数据帧类型处理                                                                                                *
    *  参数  ：    无                                                                                                            *
    *  返回值：    无                                                                                                            *
    *  修改日期：  2012-2-24                                                                                                    *
    *  作者    ：                                                                                                         *
    * ***************************************************************************************************************************/
        public static void PtlUsbFrameProc()
        {
            if (!PublicDataClass._UsbTaskFlag.FirstON_S)
            {
                //	第一次开机处理
                PublicDataClass._UsbTaskFlag.FirstON_S = true;		//	主机开机处理过标志(正常情况下该标志一直为1)
                PublicDataClass._UsbTaskFlag.C_P1_NA_F = false;     //104U格式中STARTDT命令
                PublicDataClass._UsbTaskFlag.C_RQ_NA_LINKREQ_F = false;
                return;
            }
            if (PublicDataClass._UsbTaskFlag.C_RQ_NA_LINKREQ_F == true)    //链路复位
            {
                PublicDataClass._UsbTaskFlag.C_RQ_NA_LINKREQ_F = false;
                PublicDataClass._UsbStructData.UsbTLen = 0;
                //PublicDataClass._ComStructData.TxLen = EncodeFrame(1, 0, 0, ref PublicDataClass._ComStructData.TXBuffer[0],ref PublicDataClass._DataField.Buffer[0]);
                PublicDataClass._UsbStructData.TX_TASK = true;

            }
            else if (PublicDataClass._UsbTaskFlag.C_CS_NA_1 == true)
            {

                PublicDataClass._UsbTaskFlag.C_CS_NA_1 = false;
                PublicDataClass._UsbStructData.UsbTLen = 0;
                PublicDataClass._UsbStructData.UsbTLen = EncodeFrame(2, 4, ref PublicDataClass._UsbStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.UsbFrameMsg = "对时";
                PublicDataClass._UsbStructData.TX_TASK = true;

            }
            else if (PublicDataClass._UsbTaskFlag.YK_Sel_1 == true)
            {
                PublicDataClass._UsbTaskFlag.YK_Sel_1 = false;
                PublicDataClass._UsbStructData.UsbTLen = 0;
                PublicDataClass._UsbStructData.UsbTLen = EncodeFrame(2, 5, ref PublicDataClass._UsbStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.YkStartPos, MTxSeq, MRxSeq);

                if (PublicDataClass.YkState == 1)
                    PublicDataClass.UsbFrameMsg = "选择<分>";
                else if (PublicDataClass.YkState == 2)
                    PublicDataClass.UsbFrameMsg = "选择<合>";
                PublicDataClass._UsbStructData.TX_TASK = true;
            }
            else if (PublicDataClass._UsbTaskFlag.YK_Exe_1 == true)
            {
                PublicDataClass._UsbTaskFlag.YK_Exe_1 = false;
                PublicDataClass._UsbStructData.UsbTLen = 0;
                PublicDataClass._UsbStructData.UsbTLen = EncodeFrame(2, 6, ref PublicDataClass._UsbStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.YkStartPos, MTxSeq, MRxSeq);

                if (PublicDataClass.YkState == 1)
                    PublicDataClass.UsbFrameMsg = "执行<分>";
                else if (PublicDataClass.YkState == 2)
                    PublicDataClass.UsbFrameMsg = "执行<合>";
                PublicDataClass._UsbStructData.TX_TASK = true;
            }
            else if (PublicDataClass._UsbTaskFlag.YK_Cel_1 == true)
            {
                PublicDataClass._UsbTaskFlag.YK_Cel_1 = false;
                PublicDataClass._UsbStructData.UsbTLen = 0;
                PublicDataClass._UsbStructData.UsbTLen = EncodeFrame(2, 7, ref PublicDataClass._UsbStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.YkStartPos, MTxSeq, MRxSeq);

                if (PublicDataClass.YkState == 1)
                    PublicDataClass.UsbFrameMsg = "撤销<分>";
                else if (PublicDataClass.YkState == 2)
                    PublicDataClass.UsbFrameMsg = "撤销<合>";
                PublicDataClass._UsbStructData.TX_TASK = true;
            }
            else if (PublicDataClass._UsbTaskFlag.VERSION_1 == true)    //读版本号
            {
                PublicDataClass._UsbTaskFlag.VERSION_1 = false;
                PublicDataClass._UsbStructData.UsbTLen = 0;
                PublicDataClass._UsbStructData.UsbTLen = EncodeFrame(2, 10, ref PublicDataClass._UsbStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.UsbFrameMsg = "请求版本号";
                PublicDataClass._UsbStructData.TX_TASK = true;

            }
            else if (PublicDataClass._UsbTaskFlag.CALLTIME_1 == true)
            {
                PublicDataClass._UsbTaskFlag.CALLTIME_1 = false;
                PublicDataClass._UsbStructData.UsbTLen = 0;
                PublicDataClass._UsbStructData.UsbTLen = EncodeFrame(2, 11, ref PublicDataClass._UsbStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.UsbFrameMsg = "请求时间";
                PublicDataClass._UsbStructData.TX_TASK = true;
            }
            else if (PublicDataClass._UsbTaskFlag.READ_P_1 == true)
            {
                PublicDataClass._UsbTaskFlag.READ_P_1 = false;
                PublicDataClass._UsbStructData.UsbTLen = 0;
                PublicDataClass._UsbStructData.UsbTLen = EncodeFrame(2, 9, ref PublicDataClass._UsbStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                         PublicDataClass._DataField.FieldLen, 1, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.UsbFrameMsg = "读参数";
                PublicDataClass._UsbStructData.TX_TASK = true;
            }
            else if (PublicDataClass._UsbTaskFlag.SET_PARAM_CON == true)  //设置参数
            {
                PublicDataClass._UsbTaskFlag.SET_PARAM_CON = false;
                PublicDataClass._UsbStructData.UsbTLen = 0;
                PublicDataClass._UsbStructData.UsbTLen = EncodeFrame(2, 8, ref PublicDataClass._UsbStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                         PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);
                PublicDataClass.UsbFrameMsg = "设置";
                PublicDataClass._UsbStructData.TX_TASK = true;

            }
            else if (PublicDataClass._UsbTaskFlag.AloneCallYx_1 == true)
            {
                PublicDataClass._UsbTaskFlag.AloneCallYx_1 = false;
                PublicDataClass._UsbStructData.UsbTLen = 0;
                PublicDataClass._UsbStructData.UsbTLen = EncodeFrame(2, 12, ref PublicDataClass._UsbStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.UsbFrameMsg = "请求遥信";
                PublicDataClass._UsbStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.UsbThreadID = 11;
            }
            else if (PublicDataClass._UsbTaskFlag.AloneCallYc_1 == true)
            {
                PublicDataClass._UsbTaskFlag.AloneCallYc_1 = false;
                PublicDataClass._UsbStructData.UsbTLen = 0;
                PublicDataClass._UsbStructData.UsbTLen = EncodeFrame(2, 13, ref PublicDataClass._UsbStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.UsbFrameMsg = "请求遥测";
                PublicDataClass._UsbStructData.TX_TASK = true;
                PublicDataClass._ThreadIndex.UsbThreadID = 12;

            }
            else if (PublicDataClass._UsbTaskFlag.Reset_1 == true)
            {
                PublicDataClass._UsbTaskFlag.Reset_1 = false;
                PublicDataClass._UsbStructData.UsbTLen = 0;
                PublicDataClass._UsbStructData.UsbTLen = EncodeFrame(2, 17, ref PublicDataClass._UsbStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.UsbFrameMsg = "复位";
                PublicDataClass._UsbStructData.TX_TASK = true;
            }
            else if (PublicDataClass._UsbTaskFlag.UpdateCode_Start_1 == true)
            {
                PublicDataClass._UsbTaskFlag.UpdateCode_Start_1 = false;
                PublicDataClass._UsbStructData.UsbTLen = 0;
                PublicDataClass._UsbStructData.UsbTLen = EncodeFrame(2, 14, ref PublicDataClass._UsbStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.UsbFrameMsg = "升级";
                PublicDataClass._UsbStructData.TX_TASK = true;
            }
            else if (PublicDataClass._UsbTaskFlag.UpdateCode_JY_1 == true)
            {
                PublicDataClass._UsbTaskFlag.UpdateCode_JY_1 = false;
                PublicDataClass._UsbStructData.UsbTLen = 0;
                PublicDataClass._UsbStructData.UsbTLen = EncodeFrame(2, 15, ref PublicDataClass._UsbStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.UsbFrameMsg = "校验";
                PublicDataClass._UsbStructData.TX_TASK = true;

            }
            else if (PublicDataClass._UsbTaskFlag.UpdateCode_OK_1 == true)
            {
                PublicDataClass._UsbTaskFlag.UpdateCode_OK_1 = false;
                PublicDataClass._UsbStructData.UsbTLen = 0;
                PublicDataClass._UsbStructData.UsbTLen = EncodeFrame(2, 16, ref PublicDataClass._UsbStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.UsbFrameMsg = "更新代码";
                PublicDataClass._UsbStructData.TX_TASK = true;

            }
            else if (PublicDataClass._UsbTaskFlag.AloneCallYx_2 == true)
            {
                PublicDataClass._UsbTaskFlag.AloneCallYx_2 = false;
                PublicDataClass._UsbStructData.UsbTLen = 0;
                PublicDataClass._UsbStructData.UsbTLen = EncodeFrame(2, 18, ref PublicDataClass._UsbStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.UsbFrameMsg = "内遥信";
                PublicDataClass._UsbStructData.TX_TASK = true;

            }
            else if (PublicDataClass._UsbTaskFlag.CallHisData == true)
            {

                PublicDataClass._UsbTaskFlag.CallHisData = false;
                PublicDataClass._UsbStructData.UsbTLen = 0;
                PublicDataClass._UsbStructData.UsbTLen = EncodeFrame(2, 19, ref PublicDataClass._UsbStructData.TBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                                                          PublicDataClass._DataField.FieldLen, PublicDataClass._DataField.FieldVSQ, PublicDataClass.DevAddr, PublicDataClass.ParamInfoAddr, MTxSeq, MRxSeq);

                PublicDataClass.UsbFrameMsg = "请求历史数据";
                PublicDataClass._UsbStructData.TX_TASK = true;

            }


        }
        public static void ParseUsbReceData()  //-------------------------------------------Usb解包
        {
            byte dataty;
            dataty = DecodeFrame(2, ref PublicDataClass._UsbStructData.RBuffer[0], ref PublicDataClass._DataField.Buffer[0],
                   ref PublicDataClass._DataField.FieldLen, ref PublicDataClass._DataField.FieldVSQ, ref PublicDataClass.ParamInfoAddr, PublicDataClass._UsbStructData.UsbRLen, ref STxSeq, ref SRxSeq);
            if (dataty == 0)     //非法帧
            {
                PublicDataClass.UsbFrameMsg = "无效";
            }
            else if (dataty == 9)
            {
                PublicDataClass.UsbFrameMsg = "时间";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (dataty == 10)     //
            {
                PublicDataClass.UsbFrameMsg = "选择应答";
                PublicDataClass._Message.YkDocmentView = true;
            }
            else if (dataty == 11)     //
            {
                PublicDataClass.UsbFrameMsg = "执行成功";
                PublicDataClass._Message.YkDocmentView = true;
            }
            else if (dataty == 12)     //
            {
                PublicDataClass.UsbFrameMsg = "遥控撤销";
                PublicDataClass._Message.YkDocmentView = true;
            }

            else if (dataty == 13)
            {
                PublicDataClass.UsbFrameMsg = "设置(确认)";
                PublicDataClass._Message.ParamAck = true;
            }
            else if (dataty == 14)
            {
                PublicDataClass.UsbFrameMsg = "设置(否认)";

            }
            else if (dataty == 15)
            {
                PublicDataClass.UsbFrameMsg = "参数读取";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (dataty == 16)     //
            {
                PublicDataClass.UsbFrameMsg = "版本号";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (dataty == 17)
            {
                PublicDataClass.UsbFrameMsg = "遥信";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (dataty == 18)
            {
                PublicDataClass.UsbFrameMsg = "遥测";
                PublicDataClass._Message.CallDataDocmentView = true;
            }
            else if (dataty == 19)
            {
                PublicDataClass.UsbFrameMsg = "升级(应答)";
            }
            else if (dataty == 20)
            {
                PublicDataClass.UsbFrameMsg = "校验(应答)";
                PublicDataClass._Message.CodeUpdateJY = true;
            }
            else if (dataty == 22)
            {
                PublicDataClass.UsbFrameMsg = "复位否认";

            }
        }
        /********************************************************************************************
        *  函数名：    Form1_FormClosing                                                            *
        *  功能  ：    窗体关闭事件                                                                 *
        *  参数  ：    无                                                                           *
        *  返回值：    无                                                                           *
        *  修改日期：  2010-12-07                                                                   *
        *  作者    ：  cuibj                                                                        *
        * ******************************************************************************************/
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("是否保存当前工程所有配置文件?", "提  示",
                   MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                SavePrjflag = true;
                timerOpenProj.Enabled = true;
                loadprj.ShowDialog();
                

            }
            else if (result == DialogResult.No)
            {
                System.Environment.Exit(0);//完全退出程序
            }
            else
                e.Cancel = true;
            //if (NetSendThread != null)
            //    NetSendThread.Abort();
            //if (NetRecvThread != null)
            //    NetRecvThread.Abort();
            //if (ConnectThread != null)
            //    ConnectThread.Abort();
            //if (ComThread != null)
            //    ComThread.Abort();
            //if (ListenThread!=null)
            //    ListenThread.Abort();
            //if (GprsSendThread !=null)
            //    GprsSendThread.Abort();
            //if (GprsRecvThread!=null)
            //    GprsRecvThread.Abort();
            //if (WriteDataThread != null)
            //    WriteDataThread.Abort();
            //conn.Close();
            //acceptedSocket.Close();
            //Application.Exit();
        }
        /********************************************************************************************
        *  函数名：    CallTypeButton_Click                                                         *
        *  功能  ：    召测方式事件处理函数                                                         *
        *  参数  ：    无                                                                           *
        *  返回值：    无                                                                           *
        *  修改日期：  2010-12-07                                                                   *
        *  作者    ：  cuibj                                                                        *
        * ******************************************************************************************/
        private void CallTypeButton_Click(object sender, EventArgs e)
        {
            if (CallTypeButton.Text == "重发命令")
            {

                RetryCmdViewForm Retryfm = new RetryCmdViewForm();
                Retryfm.ShowDialog();
                if (Retryfm.DialogResult == DialogResult.OK)               //判断是否按下-确定-按钮
                {

                    //PublicDataClass._CallType.NetTy = 2;
                    if (PublicDataClass._CallType.NetTy == 1)
                    {
                        Retime = PublicDataClass._Time.NetReTime;

                        timer3.Enabled = true;
                    }
                    CallTypeButton.Text = "关闭重发";

                }
            }
            else if (CallTypeButton.Text == "关闭重发")
            {
                timer3.Enabled = false;
                PublicDataClass._CallType.NetTy = 0;
                CallTypeButton.Text = "重发命令";

            }
            
            
            /*CallTypeViewForm CallTyfm = new CallTypeViewForm();
            CallTyfm.ShowDialog();
            if (CallTyfm.DialogResult == DialogResult.OK)               //判断是否按下-确定-按钮
            {
                for (int i = 0; i < PublicDataClass.SaveText.channelnum; i++)
                {
                    Regex cm = new Regex("COM"); Regex nt = new Regex("NET"); Regex gs = new Regex("GPRS");
                    Match m = cm.Match(PublicDataClass.SaveText.Channel[i].ChannelID);
                    if (m.Success)
                    {
                        if (PublicDataClass.SaveText.Channel[i].calltype == "循环")
                            PublicDataClass._CallType.ComTy = 1;
                        else if (PublicDataClass.SaveText.Channel[i].calltype == "重发")
                        {   
                            PublicDataClass._CallType.ComTy = 2;
                            Retime = PublicDataClass._Time.ComReTime;
                            timer3.Enabled = true;
                        }
                        else if (PublicDataClass.SaveText.Channel[i].calltype == "单召")
                            PublicDataClass._CallType.ComTy = 3;
                        else
                            PublicDataClass._CallType.ComTy = 0;
                    }
                    m = nt.Match(PublicDataClass.SaveText.Channel[i].ChannelID);
                    if (m.Success)
                    {
                        if (PublicDataClass.SaveText.Channel[i].calltype == "循环")
                        {
                            PublicDataClass._CallType.NetTy = 1;
                            PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_F =true;
                            PublicDataClass._FrameTime.LoopTime = 300;
                            timer3.Enabled = false;
                        }
                        else if (PublicDataClass.SaveText.Channel[i].calltype == "重发")
                        {
                            PublicDataClass._CallType.NetTy = 2;
                            Retime = PublicDataClass._Time.NetReTime;
                            timer3.Enabled = true;
                        }
                        else if (PublicDataClass.SaveText.Channel[i].calltype == "单召")
                        {
                            PublicDataClass._CallType.NetTy = 3;
                            timer3.Enabled = false;
                        }
                        else
                        {
                            PublicDataClass._CallType.NetTy = 0;
                            timer3.Enabled = false;
                        }
                    }

                }

            }*/

        }
        /********************************************************************************************
        *  函数名：    OpenProjectButton_Click                                                      *
        *  功能  ：    打开项目事件处理函数                                                         *
        *  参数  ：    无                                                                           *
        *  返回值：    无                                                                           *
        *  修改日期：  2010-12-07                                                                   *
        *  作者    ：  cuibj                                                                        *
        * ******************************************************************************************/
        private void OpenProjectButton_Click(object sender, EventArgs e)
        {
           
            //OpenPrjflag = true;
            //OpenProject();


            OpenF.Filter = "System Files(*.sys)|*.sys|所有文件(*.*)|*.*";
            OpenF.InitialDirectory = System.Environment.CurrentDirectory;
            //string savesysfilepath = @"";
            OpenPrjflag=OpenF.ShowDialog() == DialogResult.OK;
            if (OpenPrjflag)
            {
                 f = new FileInfo(OpenF.FileName);
                 d = new DirectoryInfo(f.DirectoryName);
                PublicDataClass.PrjName = d.Name;
                Openpath = f.DirectoryName;
                Openpathname = OpenF.FileName;
                loadprj.ShowDialog();
                timerOpenProj.Enabled = true;

            }
                
        }
        public void OpenProjectProc()
        {
               OpenProject();

               OpenProjectThread.Abort();
        }
        /********************************************************************************************
        *  函数名：    SaveProjectButton_Click                                                      *
        *  功能  ：    保存工程事件处理函数                                                         *
        *  参数  ：    无                                                                           *
        *  返回值：    无                                                                           *
        *  修改日期：  2010-12-07                                                                   *
        *  作者    ：  cuibj                                                                        *
        * ******************************************************************************************/
        private void SaveProjectButton_Click(object sender, EventArgs e)
        {
            //SavePrjflag=true; 
            //loadprj.ShowDialog();
            //timerOpenProj.Enabled = true;
            SaveProject();
        }
        /// <summary>
        /// 打开工程---自定义函数
        /// </summary>
        private void OpenProject()
        {
            string savesysfilepath = @"";
            FileStream afile;
            StreamReader sr;
            string strline;
            byte saveindex;
            try
            {
                string name = @"projectsysfig.sys";
                savesysfilepath = Openpath;
                if (Openpathname.Contains(name))          //进行文件有效性判断  是否包含此文件名
                {
                    PublicDataClass.PrjType = 2;   //打开的工程
                    PublicDataClass.PrjPath = savesysfilepath;
                    afile = new FileStream(OpenF.FileName, FileMode.Open);
                    sr = new StreamReader(afile);
                    strline = sr.ReadLine();
                    if (strline == null)
                    {
                        MessageBox.Show("文件保存记录为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        saveindex = Convert.ToByte(strline);
                        for (byte i = 0; i < saveindex; i++)
                        {
                            strline = sr.ReadLine();
                            if (i == 0)
                                PublicDataClass.SaveText.channelnum = Convert.ToByte(strline);
                            else if (i == 1)
                                PublicDataClass.SaveText.devicenum = Convert.ToByte(strline);
                            else
                                PublicDataClass.SaveText.cfgnum = Convert.ToByte(strline);

                        }


                    }
                    sr.Close();

          
                    if (PublicDataClass.LoginType == 1)
                    {
                        TongXunStripButton.Enabled = true;
                        CloseLinkStripButton.Enabled = true;
                        AddStripButton.Enabled = true;
                        PublicDataClass._ChangeFlag.ProductionVersionViewUpdate = true;
                    }
                    else
                    {
                        PublicDataClass._ChangeFlag.FunlistViewTreeNeedUpdate = true;
                        PublicDataClass._ChangeFlag.ToolBoxTreeNeedUpdate = true;
                    }
                    PublicDataClass.SaveText.Channel = new PublicDataClass.CHANNELINFO[PublicDataClass.SaveText.channelnum];
                    savesysfilepath = savesysfilepath + "\\channelfile.syss";                  //读通道保存文件
                    afile = new FileStream(savesysfilepath, FileMode.Open);
                    sr = new StreamReader(afile);
                    for (byte i = 0; i < PublicDataClass.SaveText.channelnum; i++)
                    {


                        for (byte j = 0; j < 10; j++)
                        {

                            strline = sr.ReadLine();
                            if (j == 0)
                                PublicDataClass.SaveText.Channel[i].ChannelID = Convert.ToString(strline);
                            else if (j == 1)
                                PublicDataClass.SaveText.Channel[i].potocol = Convert.ToString(strline);
                            else if (j == 2)
                                PublicDataClass.SaveText.Channel[i].baud = Convert.ToString(strline);
                            else if (j == 3)
                                PublicDataClass.SaveText.Channel[i].jy = Convert.ToString(strline);
                            else if (j == 4)
                                PublicDataClass.SaveText.Channel[i].port = Convert.ToString(strline);
                            else if (j == 5)
                                PublicDataClass.SaveText.Channel[i].IP = Convert.ToString(strline);
                            else if (j == 6)
                                PublicDataClass.SaveText.Channel[i].RelayTime = Convert.ToString(strline);
                            else if (j == 7)
                                PublicDataClass.cotlen = Convert.ToInt32(strline);
                            else if (j == 8)
                                PublicDataClass.publen = Convert.ToInt32(strline);
                            else if (j == 9)
                                PublicDataClass.inflen = Convert.ToInt32(strline);
                        }

                    }
                    sr.Close();

                    savesysfilepath = f.DirectoryName;
                    PublicDataClass.SaveText.Device = new PublicDataClass.DEVICEINFO[PublicDataClass.SaveText.devicenum];
                    savesysfilepath = savesysfilepath + "\\devicefile.syss";                  //读设备保存文件
                    afile = new FileStream(savesysfilepath, FileMode.Open);
                    sr = new StreamReader(afile);
                    for (byte i = 0; i < PublicDataClass.SaveText.devicenum; i++)
                    {


                        for (byte j = 0; j < 4; j++)
                        {
                            strline = sr.ReadLine();
                            if (j == 0)
                                PublicDataClass.SaveText.Device[i].PointName = Convert.ToString(strline);
                            else if (j == 1)
                                PublicDataClass.SaveText.Device[i].ChannelID = Convert.ToString(strline);
                            else if (j == 2)
                            {
                                PublicDataClass.SaveText.Device[i].Addr = Convert.ToString(strline);
                                //临时  打开工程时，通信地址等于设备0地址
                                PublicDataClass.DevAddr = Convert.ToInt32(PublicDataClass.SaveText.Device[0].Addr);


                            }
                            else if (j == 3)
                                PublicDataClass.SaveText.Device[i].State = Convert.ToString(strline);



                        }

                    }
                    sr.Close();

                   

                    savesysfilepath = f.DirectoryName;
                    PublicDataClass.SaveText.Cfg = new PublicDataClass.INFOADDRCFGINFO[PublicDataClass.SaveText.cfgnum];
                    savesysfilepath = savesysfilepath + "\\YcCfgfile.syss";                  //读设备保存文件
                    afile = new FileStream(savesysfilepath, FileMode.Open);
                    sr = new StreamReader(afile);
                    for (byte i = 0; i < PublicDataClass.SaveText.cfgnum; i++)
                    {

                        strline = sr.ReadLine();
                        PublicDataClass.SaveText.Device[i].PointName = Convert.ToString(strline);
                        strline = sr.ReadLine();
                        PublicDataClass.SaveText.Cfg[i].YccfgNum = Convert.ToInt16(strline);
                        PublicDataClass.SaveText.Cfg[i].YccfgName = new string[PublicDataClass.SaveText.Cfg[i].YccfgNum];
                        PublicDataClass.SaveText.Cfg[i].YccfgAddr = new string[PublicDataClass.SaveText.Cfg[i].YccfgNum];
                        PublicDataClass.SaveText.Cfg[i].YccfgState = new string[PublicDataClass.SaveText.Cfg[i].YccfgNum];
                        PublicDataClass.SaveText.Cfg[i].Ycdata = new string[PublicDataClass.SaveText.Cfg[i].YccfgNum];
                        PublicDataClass.SaveText.Cfg[i].Ycdataqf = new string[PublicDataClass.SaveText.Cfg[i].YccfgNum];

                        PublicDataClass._Curve.listemp = new PointPairList[PublicDataClass.SaveText.Cfg[i].YccfgNum];
                        PublicDataClass._Curve.showcurve = new bool[PublicDataClass.SaveText.Cfg[i].YccfgNum];
                        PublicDataClass._Curve.ycdata = new double[PublicDataClass.SaveText.Cfg[i].YccfgNum];
                        for (int j = 0; j < PublicDataClass.SaveText.Cfg[i].YccfgNum; j++)
                        {
                            PublicDataClass._Curve.listemp[j] = new PointPairList();
                        }

                        timer5.Enabled = true;
                        //WriteDataThread = new Thread(new ThreadStart(WriteDataThreadProc));
                        //WriteDataThread.Start();
                        for (int j = 0; j < PublicDataClass.SaveText.Cfg[i].YccfgNum; j++)
                        {
                            strline = sr.ReadLine();
                            PublicDataClass.SaveText.Device[i].Addr = Convert.ToString(strline);
                            strline = sr.ReadLine();
                            PublicDataClass.SaveText.Cfg[i].YccfgName[j] = Convert.ToString(strline);
                            strline = sr.ReadLine();
                            PublicDataClass.SaveText.Cfg[i].YccfgAddr[j] = Convert.ToString(strline);
                            strline = sr.ReadLine();
                            PublicDataClass.SaveText.Cfg[i].YccfgState[j] = Convert.ToString(strline);
                            strline = sr.ReadLine();
                            PublicDataClass.SaveText.Cfg[i].Ycdataqf[j] = Convert.ToString(strline);

                        }


                    }
                    sr.Close();

                    savesysfilepath = f.DirectoryName;
                    savesysfilepath = savesysfilepath + "\\YxCfgfile.syss";                  //读设备保存文件
                    afile = new FileStream(savesysfilepath, FileMode.Open);
                    sr = new StreamReader(afile);
                    for (byte i = 0; i < PublicDataClass.SaveText.cfgnum; i++)
                    {

                        strline = sr.ReadLine();
                        PublicDataClass.SaveText.Device[i].PointName = Convert.ToString(strline);
                        strline = sr.ReadLine();
                        PublicDataClass.SaveText.Cfg[i].YxcfgNum = Convert.ToInt16(strline);
                        PublicDataClass.SaveText.Cfg[i].YxcfgName = new string[PublicDataClass.SaveText.Cfg[i].YxcfgNum];
                        PublicDataClass.SaveText.Cfg[i].YxcfgAddr = new string[PublicDataClass.SaveText.Cfg[i].YxcfgNum];
                        PublicDataClass.SaveText.Cfg[i].YxcfgState = new string[PublicDataClass.SaveText.Cfg[i].YxcfgNum];
                        PublicDataClass.SaveText.Cfg[i].Yxdata = new string[PublicDataClass.SaveText.Cfg[i].YxcfgNum];
                        PublicDataClass.SaveText.Cfg[i].Yxdataqf = new string[PublicDataClass.SaveText.Cfg[i].YxcfgNum];

                        for (int j = 0; j < PublicDataClass.SaveText.Cfg[i].YxcfgNum; j++)
                        {
                            strline = sr.ReadLine();
                            PublicDataClass.SaveText.Device[i].Addr = Convert.ToString(strline);

                            strline = sr.ReadLine();
                            PublicDataClass.SaveText.Cfg[i].YxcfgName[j] = Convert.ToString(strline);
                            strline = sr.ReadLine();
                            PublicDataClass.SaveText.Cfg[i].YxcfgAddr[j] = Convert.ToString(strline);
                            strline = sr.ReadLine();
                            PublicDataClass.SaveText.Cfg[i].YxcfgState[j] = Convert.ToString(strline);
                            strline = sr.ReadLine();
                            PublicDataClass.SaveText.Cfg[i].Yxdataqf[j] = Convert.ToString(strline);
                        }


                    }

                    sr.Close();

                    savesysfilepath = f.DirectoryName;
                    savesysfilepath = savesysfilepath + "\\YkCfgfile.syss";                  //读设备保存文件
                    afile = new FileStream(savesysfilepath, FileMode.Open);
                    sr = new StreamReader(afile);
                    for (byte i = 0; i < PublicDataClass.SaveText.cfgnum; i++)
                    {

                        strline = sr.ReadLine();
                        PublicDataClass.SaveText.Device[i].PointName = Convert.ToString(strline);
                        strline = sr.ReadLine();
                        PublicDataClass.SaveText.Cfg[i].YkcfgNum = Convert.ToInt16(strline);
                        PublicDataClass.SaveText.Cfg[i].YkcfgName = new string[PublicDataClass.SaveText.Cfg[i].YkcfgNum];
                        PublicDataClass.SaveText.Cfg[i].YkcfgAddr = new string[PublicDataClass.SaveText.Cfg[i].YkcfgNum];
                        PublicDataClass.SaveText.Cfg[i].YkcfgState = new string[PublicDataClass.SaveText.Cfg[i].YkcfgNum];
                        for (int j = 0; j < PublicDataClass.SaveText.Cfg[i].YkcfgNum; j++)
                        {
                            strline = sr.ReadLine();
                            PublicDataClass.SaveText.Device[i].Addr = Convert.ToString(strline);
                            strline = sr.ReadLine();
                            PublicDataClass.SaveText.Cfg[i].YkcfgName[j] = Convert.ToString(strline);
                            strline = sr.ReadLine();
                            PublicDataClass.SaveText.Cfg[i].YkcfgAddr[j] = Convert.ToString(strline);
                            strline = sr.ReadLine();
                            PublicDataClass.SaveText.Cfg[i].YkcfgState[j] = Convert.ToString(strline);
                        }


                    }
                    sr.Close();
                    afile.Close();

                    //NewProjectButton.Enabled = false;
                    //OpenProjectButton.Enabled = false;
                    //SaveProjectButton.Enabled = true;

                    //OpenPrjMenuItem.Enabled = false;
                    //NewPrjMenuItem.Enabled = false;
                    //SavePrjMenuItem.Enabled = true;
                    InitGlobalData.InitAllGlobalIniValue();                //初始化全局ini配置文件


                   
                  
                    //存配置文件线程
                    //WriteIniThread = new Thread(new ThreadStart(WriteIniThreadProc));
                    //WriteIniThread.Start();

                   
                    //string datapath = PublicDataClass.PrjPath + "\\DATA" + "\\HistoryCurve" + "\\" + DateTime.Now.ToShortDateString() + ".cur";
                    //if (!File.Exists(datapath))
                    //{
                    //    File.Copy(PublicDataClass.PrjPath + "\\DATA" + "\\" + "curve.cur", datapath,true);
                    //}
                         OpenPjOK = true;
                }
                else
                {
                    MessageBox.Show("选择的工程文件无效", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            catch
            {
                MessageBox.Show("打开工程配置文件异常", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private static  void WriteIniThreadProc()
        {
            if (WriteIniflag)
            {
                WriteIniflag = false;
                WriteReadAllFile.WriteReadParamIniFile(WriteIniFileName, WriteIniFileType, WriteIniFilek );    
            }
        
        }
        /// <summary>
        /// 保存工程函数---自定义
        /// </summary>
        private void SaveProject()
        {
            string savesysfilepath = @"";
            byte cn = 0;
            FileStream afile;
            StreamWriter sw;

            try
            {
                //PublicDataClass.PrjPath += "\\";
                savesysfilepath = PublicDataClass.PrjPath + "\\projectsysfig.sys";
                afile = new FileStream(savesysfilepath, FileMode.Create);


                sw = new StreamWriter(afile);
                sw.WriteLine("3");                 //3行
                sw.WriteLine(PublicDataClass.SaveText.channelnum);
                sw.WriteLine(PublicDataClass.SaveText.devicenum);
                sw.WriteLine(PublicDataClass.SaveText.cfgnum);
                sw.Close();

                savesysfilepath = PublicDataClass.PrjPath + "\\channelfile.syss";
                afile = new FileStream(savesysfilepath, FileMode.Create);
                sw = new StreamWriter(afile);

                for (cn = 0; cn < PublicDataClass.SaveText.channelnum; cn++)
                {

                    for (byte j = 0; j < 10; j++)
                    {
                        if (j == 0)
                            sw.WriteLine(PublicDataClass.SaveText.Channel[cn].ChannelID);
                        else if (j == 1)
                            sw.WriteLine(PublicDataClass.SaveText.Channel[cn].potocol);
                        else if (j == 2)
                            sw.WriteLine(PublicDataClass.SaveText.Channel[cn].baud);
                        else if (j == 3)
                            sw.WriteLine(PublicDataClass.SaveText.Channel[cn].jy);
                        else if (j == 4)
                            sw.WriteLine(PublicDataClass.SaveText.Channel[cn].port);
                        else if (j == 5)
                            sw.WriteLine(PublicDataClass.SaveText.Channel[cn].IP);
                        else if (j == 6)
                            sw.WriteLine(PublicDataClass.SaveText.Channel[cn].RelayTime);
                        else if (j == 7)
                            sw.WriteLine(PublicDataClass.cotlen);
                        else if (j == 8)
                            sw.WriteLine(PublicDataClass.publen);
                        else if (j == 9)
                            sw.WriteLine(PublicDataClass.inflen);
                    }



                }
                sw.Close();

                savesysfilepath = PublicDataClass.PrjPath + "\\devicefile.syss";
                afile = new FileStream(savesysfilepath, FileMode.Create);
                sw = new StreamWriter(afile);
                for (cn = 0; cn < PublicDataClass.SaveText.devicenum; cn++)
                {

                    for (byte j = 0; j < 4; j++)
                    {
                        if (j == 0)
                            sw.WriteLine(PublicDataClass.SaveText.Device[cn].PointName);
                        else if (j == 1)
                            sw.WriteLine(PublicDataClass.SaveText.Device[cn].ChannelID);
                        else if (j == 2)
                            sw.WriteLine(PublicDataClass.SaveText.Device[cn].Addr);
                        else if (j == 3)
                            sw.WriteLine(PublicDataClass.SaveText.Device[cn].State);

                    }
                }
                sw.Close();

                savesysfilepath = PublicDataClass.PrjPath + "\\YcCfgfile.syss";
                afile = new FileStream(savesysfilepath, FileMode.Create);
                sw = new StreamWriter(afile);

                for (cn = 0; cn < PublicDataClass.SaveText.cfgnum; cn++)
                {
                    sw.WriteLine(PublicDataClass.SaveText.Device[cn].PointName);
                    sw.WriteLine(PublicDataClass.SaveText.Cfg[cn].YccfgNum);
                    for (int j = 0; j < PublicDataClass.SaveText.Cfg[cn].YccfgNum; j++)
                    {
                        sw.WriteLine(PublicDataClass.SaveText.Device[cn].Addr);
                        sw.Flush();
                        sw.WriteLine(PublicDataClass.SaveText.Cfg[cn].YccfgName[j]);
                        sw.Flush();
                        sw.WriteLine(PublicDataClass.SaveText.Cfg[cn].YccfgAddr[j]);
                        sw.Flush();
                        sw.WriteLine(PublicDataClass.SaveText.Cfg[cn].YccfgState[j]);
                        sw.Flush();
                        sw.WriteLine(PublicDataClass.SaveText.Cfg[cn].Ycdataqf[j]);
                        sw.Flush();
                    }
                }
                sw.Close();

                savesysfilepath = PublicDataClass.PrjPath + "\\YxCfgfile.syss";
                afile = new FileStream(savesysfilepath, FileMode.Create);
                sw = new StreamWriter(afile);

                for (cn = 0; cn < PublicDataClass.SaveText.cfgnum; cn++)
                {
                    sw.WriteLine(PublicDataClass.SaveText.Device[cn].PointName);
                    sw.WriteLine(PublicDataClass.SaveText.Cfg[cn].YxcfgNum);
                    for (int j = 0; j < PublicDataClass.SaveText.Cfg[cn].YxcfgNum; j++)
                    {
                        sw.WriteLine(PublicDataClass.SaveText.Device[cn].Addr);
                        sw.WriteLine(PublicDataClass.SaveText.Cfg[cn].YxcfgName[j]);
                        sw.WriteLine(PublicDataClass.SaveText.Cfg[cn].YxcfgAddr[j]);
                        sw.WriteLine(PublicDataClass.SaveText.Cfg[cn].YxcfgState[j]);
                       sw.WriteLine(PublicDataClass.SaveText.Cfg[cn].Yxdataqf[j]);
                       
    
                    }
                }
                sw.Close();

                savesysfilepath = PublicDataClass.PrjPath + "\\YkCfgfile.syss";
                afile = new FileStream(savesysfilepath, FileMode.Create);
                sw = new StreamWriter(afile);

                for (cn = 0; cn < PublicDataClass.SaveText.cfgnum; cn++)
                {
                    sw.WriteLine(PublicDataClass.SaveText.Device[cn].PointName);
                    sw.WriteLine(PublicDataClass.SaveText.Cfg[cn].YkcfgNum);
                    for (int j = 0; j < PublicDataClass.SaveText.Cfg[cn].YkcfgNum; j++)
                    {
                        sw.WriteLine(PublicDataClass.SaveText.Device[cn].Addr);
                        sw.WriteLine(PublicDataClass.SaveText.Cfg[cn].YkcfgName[j]);
                        sw.WriteLine(PublicDataClass.SaveText.Cfg[cn].YkcfgAddr[j]);
                        sw.WriteLine(PublicDataClass.SaveText.Cfg[cn].YkcfgState[j]);
                    }
                }
               
                sw.Close();
                afile.Close();

                MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "提示,保存失败!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void AddStripButton_Click(object sender, EventArgs e)
        {
            ModifyAddrForm modifyfm = new ModifyAddrForm();
            modifyfm.ShowDialog();
        }

        private void TransMitButton_Click(object sender, EventArgs e)
        {
            TransmitDataForm Transmitfm = new TransmitDataForm();
            Transmitfm.ShowDialog();

            //try
            //{
            //    Transmitfm.Show();
            //    Transmitfm.Activate();
            //}
            //catch
            //{
            //    TransmitDataForm Transmitfm = new TransmitDataForm();
            //    Transmitfm.Show();
            //    Transmitfm.Activate();
            //}
        }

     
        /********************************************************************************************
        *  函数名：    timer3_Tick                                                                  *
        *  功能  ：    定时器3的处理函数 主要负责数据的重发                                         *
        *  参数  ：    无                                                                           *
        *  返回值：    无                                                                           *
        *  修改日期：  2010-12-07                                                                   *
        *  作者    ：  cuibj                                                                        *
        * ******************************************************************************************/
        private void timer3_Tick(object sender, EventArgs e)
        {
            if (PublicDataClass._CallType.NetTy == 1)    //网口循环
            {

                Retime--;
                if (Retime == 0)
                {
                    Retime = PublicDataClass._Time.NetReTime;
                    switch (PublicDataClass._ThreadIndex.RetryNetThreadID)
                    {

                        case 1:
                            PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_F = true;
                            break;
                        case 2:
                            PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_OK = true;
                            break;
                        case 3:
                            PublicDataClass._NetTaskFlag.STOP_LINKREQ = true;
                            break;
                        case 4:
                            PublicDataClass._NetTaskFlag.STOP_LINKREQ_OK = true;
                            break;
                        case 5:
                            PublicDataClass._NetTaskFlag.Do_TESTACT = true;
                            break;
                        case 6:
                            PublicDataClass._NetTaskFlag.Do_TESTACT_OK = true;
                            break;
                        case 7:
                            PublicDataClass._NetTaskFlag.Do_OKTACT = true;
                            break;
                        case 8:
                            PublicDataClass._NetTaskFlag.C_CS_NA_1 = true; //对时
                            break;
                        case 9:
                            {
                                PublicDataClass._NetTaskFlag.C_IC_NA_1 = true; //总召唤
                                PublicDataClass._FrameTime.LoopTime = 15;
                            }
                            break;
                        case 10:
                            PublicDataClass._NetTaskFlag.Reset_1 = true;
                            break;
                        case 11:
                            PublicDataClass._NetTaskFlag.AloneCallYc_1 = true;
                            break;
                        case 12:
                            PublicDataClass._NetTaskFlag.AloneCallYx_1 = true;
                            break;

                    }
                }

                if (PublicDataClass._FrameTime.LoopTime > 0)   //总召唤无数据上报轮询时间
                {
                    PublicDataClass._FrameTime.LoopTime--;
                    if (PublicDataClass._FrameTime.LoopTime == 0)
                    {
                        PublicDataClass._FrameTime.LoopTime = 15;
                        PublicDataClass._NetTaskFlag.C_IC_NA_1 = true;
                        Retime = PublicDataClass._Time.NetReTime;
                        //PublicDataClass._NetTaskFlag.C_IC_NA_1  = true;
                        //PublicDataClass._NetTaskFlag.Do_OKTACT  = false;
                        //PublicDataClass._NetTaskFlag.Do_TESTACT = false;

                    }
                }
            }
            else if (PublicDataClass._CallType.ComTy == 1)    //串口循环
            {

                Retime--;
                if (Retime == 0)
                {
                    Retime = PublicDataClass._Time.ComReTime;
                    switch (PublicDataClass._ThreadIndex.ComThreadID)
                    {
                        case 1:
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                        case 8:
                            PublicDataClass._ComTaskFlag.VERSION_1 = true;
                            break;
                        case 9:
                            PublicDataClass._ComTaskFlag.C_IC_NA_1 = true;
                            break;


                        case 11:
                            PublicDataClass._ComTaskFlag.AloneCallYx_1 = true;
                            break;
                        case 12:
                            PublicDataClass._ComTaskFlag.AloneCallYc_1 = true;
                            break;

                    }
                }
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            //timer4.Enabled
            if (DelayTime > 0)
            {
                DelayTime--;
                if (DelayTime == 0)
                {
                    PublicDataClass._Message.NetShowDelayTimeMsg = true;
                    //if (PublicDataClass._ThreadIndex.NetThreadID == 1)
                    //    PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_F = true;
                    //if (PublicDataClass._ThreadIndex.NetThreadID == 17)
                    //    PublicDataClass._NetTaskFlag.Do_TESTACT = true;
                    //if (NetSendThread != null)
                    //    NetSendThread.Abort();
                    //if (NetRecvThread != null)
                    //    NetRecvThread.Abort();
                    //if (ConnectThread != null)
                    //    ConnectThread.Abort();

                    //conn.Close();

                    //conn = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    //ConnectThread = new Thread(new ThreadStart(ConnThreadSendProc));
                    //ConnectThread.Start();

                    //NetSendThread = new Thread(new ThreadStart(NetThreadSendProc));
                    //NetSendThread.Start();

                    //NetRecvThread = new Thread(new ThreadStart(ReceiveNetMsg));
                    //NetRecvThread.Start();
                }

            }
            
            if (PublicDataClass._FrameTime.T1 > 0)   //确认帧的
            {
                PublicDataClass._FrameTime.T1--;
                if (PublicDataClass._FrameTime.T1 == 0)
                {
                    //PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_F = true;
                    if (PublicDataClass.TcpLinkType == 2)
                        NetLinkIsClose = true;//建立连接后，测试复位后T1时间内未收到断开
                 
                }

            }
            if (PublicDataClass._FrameTime.T1_send > 0)   //确认帧的
            {
                PublicDataClass._FrameTime.T1_send--;
                if (PublicDataClass._FrameTime.T1_send == 0)
                {
                    PublicDataClass._NetTaskFlag.Do_OKTACT = true;
                }

            }
            if (PublicDataClass._FrameTime.T2 > 0)           //测试帧时间
            {
                PublicDataClass._FrameTime.T2--;
                if (PublicDataClass._FrameTime.T2 == 0)
                {
                    PublicDataClass._NetTaskFlag.Do_OKTACT = true;
                    //switch (PublicDataClass._ThreadIndex.NetThreadID)
                    //{
                    //    case 17:
                    //        PublicDataClass._NetTaskFlag.Do_TESTACT = true;   //发测试帧
                    //        break;
                    //    default:
                    //        PublicDataClass._NetTaskFlag.Do_OKTACT = true;    //发确认帧
                    //        break;
                    //}

                }
            }
            if (PublicDataClass._FrameTime.T3 > 0)   //测试帧
            {
                PublicDataClass._FrameTime.T3--;
                if (PublicDataClass._FrameTime.T3 == 0)
                {
                    //PublicDataClass._FrameTime.T3 = 20;
                 
                    PublicDataClass._NetTaskFlag.Do_TESTACT = true;
                }

            }

            
        }

        private void CaligtMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process Proc; //声明一个程序类

            try
            {
                Proc = System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "\\TFTPD32.EXE");//启动外部程序
            }

            catch
            {

                MessageBox.Show("系统找不到指定的程序文件", "错误提示！");
                return;

            }
        }

        private void PingMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.ProcessStartInfo Info = new System.Diagnostics.ProcessStartInfo();

            //设置外部程序名

            Info.FileName = "编程实现Ping操作.exe";    //计算器 calc.exe  info.WorkingDirectory = "c:/windows/";

            //设置外部程序的启动参数（命令行参数）为test.txt

            Info.Arguments = "hello word";

            Info.WindowStyle = ProcessWindowStyle.Normal;   //窗口的状态
            //设置外部程序工作目录为  C:\

            Info.WorkingDirectory = System.Environment.CurrentDirectory;
            //声明一个程序类

            System.Diagnostics.Process Proc;

            try
            {
                Proc = System.Diagnostics.Process.Start(Info);//启动外部程序
            }

            catch
            {

                Console.WriteLine("系统找不到指定的程序文件。\r{0}", e);

                return;

            }
        }

        private void MoniterMenuItem_Click(object sender, EventArgs e)
        {
            if (PublicDataClass.PrjType <= 0)
                return;
            MoniterMenuItem.Checked = true;
            m_realtdataview.Show(dockPanel);
        }

        private void CallMenuItem_Click(object sender, EventArgs e)
        {
            if (PublicDataClass.PrjType <= 0)
                return;
            CallMenuItem.Checked = true;
            m_calldview.Show(dockPanel);
        }

        private void ControlMenuItem_Click(object sender, EventArgs e)
        {
            if (PublicDataClass.PrjType <= 0)
                return;
            ControlMenuItem.Checked = true;
            m_controlview.Show(dockPanel);
        }

        private void OtherMenuItem_Click(object sender, EventArgs e)
        {
            if (PublicDataClass.PrjType <= 0)
                return;
            OtherMenuItem.Checked = true;
            m_otherview.Show(dockPanel);
        }

        private void HisdataMenuItem_Click(object sender, EventArgs e)
        {
            if (PublicDataClass.PrjType <= 0)
                return;
            HisdataMenuItem.Checked = true;
            m_historydataview.Show(dockPanel);
        }


        private void ThreeYPMenuItem_Click(object sender, EventArgs e)
        {
            if (PublicDataClass.PrjType <= 0)
                return;
            ThreeYPMenuItem.Checked = true;
            m_acquisitionview.Show(dockPanel);
        }

        private void OpenPrjMenuItem_Click(object sender, EventArgs e)
        {
            //OpenPrjflag = true;
            //OpenProject();


            OpenF.Filter = "System Files(*.sys)|*.sys|所有文件(*.*)|*.*";
            OpenF.InitialDirectory = System.Environment.CurrentDirectory;
            //string savesysfilepath = @"";
            OpenPrjflag = OpenF.ShowDialog() == DialogResult.OK;
            if (OpenPrjflag)
            {
                f = new FileInfo(OpenF.FileName);
                d = new DirectoryInfo(f.DirectoryName);
                PublicDataClass.PrjName = d.Name;
                Openpath = f.DirectoryName;
                Openpathname = OpenF.FileName;
                timerOpenProj.Enabled = true;
            }
        }

        private void NewPrjMenuItem_Click(object sender, EventArgs e)
        {
            NewPrjform.ShowDialog();

            if (NewPrjform.DialogResult == DialogResult.OK)               //判断是否按下-确定-按钮
            {
                PublicDataClass._ChangeFlag.ToolBoxTreeNeedUpdate     = true;
                PublicDataClass._ChangeFlag.FunlistViewTreeNeedUpdate = true;
                PublicDataClass.PrjType   = 1;
                NewProjectButton.Enabled  = false;
                OpenProjectButton.Enabled = false;
                SaveProjectButton.Enabled = true;
                OpenPrjMenuItem.Enabled   = false;
                NewPrjMenuItem.Enabled    = false;
            }
            OpenPrjMenuItem.Checked = false;
            NewPrjMenuItem.Checked  = true;
        }

        private void SavePrjMenuItem_Click(object sender, EventArgs e)
        {
            SaveProject();
            SavePrjMenuItem.Checked = true;
        }

        private void SoftUseMenuItem_Click(object sender, EventArgs e)
        {
            string helpfile = "配网维护测试软件.chm";

            Help.ShowHelp(this, helpfile);


        }

        private void MenuItemversion_Click(object sender, EventArgs e)
        {
            MessageBox.Show("版本日期：2014-7-21-维护软件综合版本 ", "版本",MessageBoxButtons.OK, MessageBoxIcon.Information);
          //  MessageBox.Show("版本日期：2013-10-12-维护软件综合版本 ", "版本", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   
        }

        private void GraphicMenuItem_Click(object sender, EventArgs e)
        {
            m_graphicview.Show(dockPanel);
        }

        private void ChangeInfoMenuItem_Click(object sender, EventArgs e)
        {
            m_changview.Show(dockPanel);
        }

     

        private static void SampleEventHandler(object sender, MyEventArgs mea)
        {
            System.Windows.Forms.TextBox tb = (System.Windows.Forms.TextBox)sender;
            tb.Text = mea.Message;

        }
        //---------------------------------------------------------
        public class MyEventArgs : EventArgs
        {
            private string msg;

            public MyEventArgs(string messageData)
            {
                msg = messageData;
            }
            public string Message
            {
                get { return msg; }
                set { msg = value; }
            }
        }
        //---------------------------------------------------------
        public class HasEvent
        {
            // Declare an event of delegate type EventHandler of 
            // MyEventArgs.

            public event EventHandler<MyEventArgs> SampleEvent;

            public void DemoEvent(object obj, string val)
            {
                // Copy to a temporary variable to be thread-safe.
                EventHandler<MyEventArgs> temp = SampleEvent;
                if (temp != null)
                    temp(obj, new MyEventArgs(val));
            }
        }

        private void JZMenuItem_Click(object sender, EventArgs e)
        {
            JZfrm jz = new JZfrm();
            jz.ShowDialog();
        }

        
        private void JSButton_Click(object sender, EventArgs e)
        {
            int mstime = Convert.ToInt16(DateTime.Now.Second) * 1000 + Convert.ToInt16(DateTime.Now.Millisecond);

            byte data = Convert.ToByte(Convert.ToInt16(DateTime.Now.Day) & 0x1f);
            byte data2 = Convert.ToByte((Convert.ToInt16(DateTime.Now.DayOfWeek) & 0x07) << 5);


            PublicDataClass._DataField.FieldLen = 0;
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((mstime) & 0x00ff);
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((mstime) & 0xff00) >> 8);
            PublicDataClass._DataField.FieldLen += 2;
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(DateTime.Now.Minute);
            PublicDataClass._DataField.FieldLen++;
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(DateTime.Now.Hour);
            PublicDataClass._DataField.FieldLen++;
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(data | data2);
            PublicDataClass._DataField.FieldLen++;
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(DateTime.Now.Month);
            PublicDataClass._DataField.FieldLen++;
            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(Convert.ToInt16(DateTime.Now.Year) - 2000);
            PublicDataClass._DataField.FieldLen++;

            PublicDataClass.ParamInfoAddr = 0;

            if (PublicDataClass.ChanelId == 1)
                PublicDataClass._ComTaskFlag.C_CS_NA_1 = true;

            if (PublicDataClass.ChanelId  == 2)
                PublicDataClass._NetTaskFlag.C_CS_NA_1 = true;
           
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            WriteDataflag = true; 
            //for (int i = 0; i < PublicDataClass.SaveText.Cfg[0].YccfgNum; i++)
            //{
            //    double x = (double)new XDate(DateTime.Now);
            //    double y = PublicDataClass._Curve.ycdata[i];
            //    if (PublicDataClass._Curve.listemp[i].Count >= 3600)
            //    {
            //        PublicDataClass._Curve.listemp[i].RemoveAt(0);

            //    }
            //    PublicDataClass._Curve.listemp[i].Add(x, y);
            //}

            //if ((DateTime.Now.Hour == 0) && (DateTime.Now.Minute == 0) && (DateTime.Now.Second == 0))
            //{
            //    string s = PublicDataClass.PrjPath + "\\DATA" + "\\HistoryCurve" + "\\" + DateTime.Now.ToShortDateString() + ".cur";
            //    if (!File.Exists(s))
            //    {
            //        File.Copy(PublicDataClass.PrjPath + "\\DATA" + "\\" + "curve.cur", s, true);
            //    }
            //}
            ////Savetime++;
            ////if (Savetime >= 5)
            ////{
            ////    Savetime = 0;
            //string datapath = PublicDataClass.PrjPath + "\\DATA" + "\\HistoryCurve" + "\\" + DateTime.Now.ToShortDateString() + ".cur";
            //////string ConStr = "Provider=Microsoft.Jet.OLEDB.4.0;Jet OLEDB:DataBase Password='csgcsg ';User Id=admin;Data source='" + datapath + "'";
            //string ConStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data source='" + datapath + "'";
            //OleDbConnection oleCon = new OleDbConnection(ConStr);
            //oleCon.Open();
            
            //using (OleDbCommand cmd = new OleDbCommand())
            //{
            //    string sqlstr;
                
            //    //sqlstr = "update 曲线 set YC1=10.00,YC2=10.00" + " where hour(时间)= '" + DateTime.Now.Hour + "'" + " and minute(时间)= '" + DateTime.Now.Minute + "'" + " and second(时间)= '" + DateTime.Now.Second + "'";
            //    //sqlstr = "update 曲线 set YC1='" + PublicDataClass._Curve.ycdata[0] + "',YC2=10.00" + " where hour(时间)= '" + DateTime.Now.Hour + "'" + " and minute(时间)= '" + DateTime.Now.Minute + "'" + " and second(时间)= '" + DateTime.Now.Second + "'";
            //    sqlstr = "update 曲线 set ";
            //    //int num= (PublicDataClass._Curve.savenum <= PublicDataClass.SaveText.Cfg[0].YccfgNum ? PublicDataClass._Curve.savenum : PublicDataClass.SaveText.Cfg[0].YccfgNum);
            //    int num = PublicDataClass._Curve.savenum;
            //    for (int i = 0; i < num-1; i++)
            //    {
            //        if ((PublicDataClass._Curve.SavePosTable[i] - 16385 >= 0) && (PublicDataClass._Curve.SavePosTable[i] - 16385 < PublicDataClass.SaveText.Cfg[0].YccfgNum))
            //        sqlstr += "YC" + (i+1) + "='" + PublicDataClass._Curve.ycdata[PublicDataClass._Curve.SavePosTable[i]-16385] + "'" + ",";

            //    }
            //    if ((PublicDataClass._Curve.SavePosTable[num - 1] - 16385 >= 0) && (PublicDataClass._Curve.SavePosTable[num - 1] - 16385 < PublicDataClass.SaveText.Cfg[0].YccfgNum))
            //        sqlstr += "YC" + num + "='" + PublicDataClass._Curve.ycdata[PublicDataClass._Curve.SavePosTable[num - 1] - 16385] + "'" + " " + " where hour(时间)= '" + DateTime.Now.Hour + "'" + " and minute(时间)= '" + DateTime.Now.Minute + "'" + " and second(时间)= '" + DateTime.Now.Second + "'";
            //    else
            //    {
            //        sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);
            //        sqlstr += " " + " where hour(时间)= '" + DateTime.Now.Hour + "'" + " and minute(时间)= '" + DateTime.Now.Minute + "'" + " and second(时间)= '" + DateTime.Now.Second + "'";
            //    }


            //      cmd.CommandText = sqlstr;
            //    //cmd.CommandText = "update 曲线 set YC1=10.00,YC2=10.00" + " where hour(时间)= '" + DateTime.Now.Hour + "'" + " and minute(时间)= '" + DateTime.Now.Minute + "'" + " and second(时间)= '" + DateTime.Now.Second + "'";
            //    cmd.Connection = oleCon;
            //    cmd.ExecuteNonQuery();
            //    oleCon.Close();
            //}
            ////}
            
        }
        public static void WriteDataThreadProc()   //写数据库线程
        {
            while (true)
            {
                try
                {
                    if (WriteDataflag == true)
                    {
                        WriteDataflag = false;
                        for (int i = 0; i < PublicDataClass.SaveText.Cfg[0].YccfgNum; i++)
                        {
                            double x = (double)new XDate(DateTime.Now);
                            double y = PublicDataClass._Curve.ycdata[i];
                            if (PublicDataClass._Curve.listemp[i].Count >= 3600)
                            {
                                PublicDataClass._Curve.listemp[i].RemoveAt(0);

                            }
                            PublicDataClass._Curve.listemp[i].Add(x, y);
                        }

                        if ((DateTime.Now.Hour == 0) && (DateTime.Now.Minute == 0) && (DateTime.Now.Second == 0))
                        {
                            string s = PublicDataClass.PrjPath + "\\DATA" + "\\HistoryCurve" + "\\" + DateTime.Now.ToShortDateString() + ".cur";
                            if (!File.Exists(s))
                            {
                                File.Copy(PublicDataClass.PrjPath + "\\DATA" + "\\" + "curve.cur", s, true);
                            }
                        }
                        //Savetime++;
                        //if (Savetime >= 5)
                        //{
                        //    Savetime = 0;
                        string datapath = PublicDataClass.PrjPath + "\\DATA" + "\\HistoryCurve" + "\\" + DateTime.Now.ToShortDateString() + ".cur";
                        ////string ConStr = "Provider=Microsoft.Jet.OLEDB.4.0;Jet OLEDB:DataBase Password='csgcsg ';User Id=admin;Data source='" + datapath + "'";
                        string ConStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data source='" + datapath + "'";
                        OleDbConnection oleCon = new OleDbConnection(ConStr);
                        oleCon.Open();

                        using (OleDbCommand cmd = new OleDbCommand())
                        {
                            string sqlstr;

                            //sqlstr = "update 曲线 set YC1=10.00,YC2=10.00" + " where hour(时间)= '" + DateTime.Now.Hour + "'" + " and minute(时间)= '" + DateTime.Now.Minute + "'" + " and second(时间)= '" + DateTime.Now.Second + "'";
                            //sqlstr = "update 曲线 set YC1='" + PublicDataClass._Curve.ycdata[0] + "',YC2=10.00" + " where hour(时间)= '" + DateTime.Now.Hour + "'" + " and minute(时间)= '" + DateTime.Now.Minute + "'" + " and second(时间)= '" + DateTime.Now.Second + "'";
                            sqlstr = "update 曲线 set ";
                            //int num= (PublicDataClass._Curve.savenum <= PublicDataClass.SaveText.Cfg[0].YccfgNum ? PublicDataClass._Curve.savenum : PublicDataClass.SaveText.Cfg[0].YccfgNum);
                            int num = PublicDataClass._Curve.savenum;
                            for (int i = 0; i < num - 1; i++)
                            {
                                if ((PublicDataClass._Curve.SavePosTable[i] - 16385 >= 0) && (PublicDataClass._Curve.SavePosTable[i] - 16385 < PublicDataClass.SaveText.Cfg[0].YccfgNum))
                                    sqlstr += "YC" + (i + 1) + "='" + PublicDataClass._Curve.ycdata[PublicDataClass._Curve.SavePosTable[i] - 16385] + "'" + ",";

                            }
                            if ((PublicDataClass._Curve.SavePosTable[num - 1] - 16385 >= 0) && (PublicDataClass._Curve.SavePosTable[num - 1] - 16385 < PublicDataClass.SaveText.Cfg[0].YccfgNum))
                                sqlstr += "YC" + num + "='" + PublicDataClass._Curve.ycdata[PublicDataClass._Curve.SavePosTable[num - 1] - 16385] + "'" + " " + " where hour(时间)= '" + DateTime.Now.Hour + "'" + " and minute(时间)= '" + DateTime.Now.Minute + "'" + " and second(时间)= '" + DateTime.Now.Second + "'";
                            else
                            {
                                sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);
                                sqlstr += " " + " where hour(时间)= '" + DateTime.Now.Hour + "'" + " and minute(时间)= '" + DateTime.Now.Minute + "'" + " and second(时间)= '" + DateTime.Now.Second + "'";
                            }


                            cmd.CommandText = sqlstr;
                            //cmd.CommandText = "update 曲线 set YC1=10.00,YC2=10.00" + " where hour(时间)= '" + DateTime.Now.Hour + "'" + " and minute(时间)= '" + DateTime.Now.Minute + "'" + " and second(时间)= '" + DateTime.Now.Second + "'";
                            cmd.Connection = oleCon;
                            cmd.ExecuteNonQuery();
                            oleCon.Close();
                        }
                        //}
                    }
                }
                catch
                { }
                Thread.Sleep(1);
            }

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ACJZfrm jz = new ACJZfrm();
            jz.ShowDialog();
        }

        private void timerOpenProj_Tick(object sender, EventArgs e)
        {
            if (OpenPrjflag)
            {
                OpenPrjflag = false;
                //   OpenProject();
                OpenProjectThread = new Thread(new ThreadStart(OpenProjectProc));
                OpenProjectThread.Start();
                //       if(OpenPjOK)
                loadprj.Close();
            }
            if (SavePrjflag)
            {
                SavePrjflag = false;
                InitGlobalData.SaveAllGlobalIniValue();
                timerOpenProj.Enabled = false;
                loadprj.Close();
                System.Environment.Exit(0);//完全退出程序
            }
            if (OpenPjOK)//打开工程成功
            {

                NewProjectButton.Enabled = false;
                OpenProjectButton.Enabled = false;
                SaveProjectButton.Enabled = true;

                OpenPrjMenuItem.Enabled = false;
                NewPrjMenuItem.Enabled = false;
                SavePrjMenuItem.Enabled = true;
                timerOpenProj.Enabled = false;
                toolStripButton3.Enabled = true;
             
            }
        }

        private void FrameButton_Click(object sender, EventArgs e)
        {
            FrameSet Framefm = new FrameSet();
            Framefm.ShowDialog();
        }

        private void 分模块测试视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_productionfunview.Show(dockPanel);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            YCSet YcSetDlg = new YCSet();
            YcSetDlg .ShowDialog();
            if (YcSetDlg.DialogResult == DialogResult.OK)               //判断是否按下-确定-按钮
            {
            }
        }

        private void timer6_send_Tick(object sender, EventArgs e)
        {
           
            //if (PublicDataClass._FrameTime.T_NetSend>0) 
            //    PublicDataClass._FrameTime.T_NetSend--;
     
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            YCSet YcSetDlg = new YCSet();
            YcSetDlg.ShowDialog();
            if (YcSetDlg.DialogResult == DialogResult.OK)               //判断是否按下-确定-按钮
            {
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            m_inicfgview.ShowDialog();
            if (m_inicfgview.DialogResult == DialogResult.OK)
            {
                 m_acquisitionview.DynOptProcess();//动态添加选项卡
              
              //  m_xmlcfgview.Init();  //根据当前文件夹文件个数初始化动态选项卡数目
              //  m_xmlcfgview.Show(dockPanel);//动态配置时自动显示xml配置视图
              //m_xmlcfgview.DynOptProcess();//在xml配置视图已经显示的情况下强制添加选项卡

            }
        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            ProtocolTest form = new ProtocolTest();
            form.ShowDialog();
            if (form.DialogResult == DialogResult.OK)
            {
                if (PublicDataClass._ProtocoltyFlag.Test101 == true)
                {

                    ProtoDelayTime = 0;
                    timerDelay.Enabled = true;//测试开始开 timerDelay定时器

                    if (PublicDataClass._ProtocoltyFlag.ComProFlag == 1)//串口走101
                        PublicDataClass._ComTaskFlag.C_RQ_NA_LINKREQ_F = true;
                    if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)//网口走101
                        PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_F = true;
                    PublicDataClass._ProtocoltyFlag.ZZFirstFlag = true;//测试开始发第一次总召标志
                    
                }
                else if (PublicDataClass._ProtocoltyFlag.Test104 == true)
                {
                    timerDelay.Enabled = true;//测试开始开 timerDelay定时器

                    if (PublicDataClass._ProtocoltyFlag.ComProFlag == 2)//串口走104
                        PublicDataClass._ComTaskFlag.C_RQ_NA_LINKREQ_F = true;
                    if (PublicDataClass._ProtocoltyFlag.NetProFlag == 1)//网口走104
                        PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_F = true;
                    PublicDataClass._ProtocoltyFlag.ZZFirstFlag = true;//测试开始发第一次总召标志
                }
                else timerDelay.Enabled = false;
            }
        }
        //定时器timerDelay101测试开始时启动，关闭测试停止。功能为超时重发和总召唤循环发送
        private void timerDelay_Tick(object sender, EventArgs e)
        {
            if (ProtoDelayTime > 0)
            {
                ProtoDelayTime--;
                if (ProtoDelayTime == 0)
                {
                    ProtoDelayNum++;
                    if (ProtoDelayNum > 5)
                    {
                        ProtoDelayNum = 0;
                        if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)//网口走101
                            PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_F = true;
                       if (PublicDataClass._ProtocoltyFlag.ComProFlag == 1)//串口走101
                            PublicDataClass._ComTaskFlag.C_RQ_NA_LINKREQ_F = true;
                        PublicDataClass._ProtocoltyFlag.ZZFirstFlag = true;//测试开始发第一次总召标志
                    }
                    if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2)//当前101测试走网口
                        PublicDataClass._NetStructData.TX_TASK = true;
                     if (PublicDataClass._ProtocoltyFlag.ComProFlag == 1)//当前101测试走串口
                        PublicDataClass._ComStructData.TX_TASK = true;
                    ProtoDelayTime = PublicDataClass._ProtocoltyFlag.DelayTime;
  
                }
            }
            if (ProtoZZTime > 0)
            {
                ProtoZZTime--;
                if (ProtoZZTime == 0)
                {
                    PublicDataClass._ProtocoltyFlag.ZZFlag = true;
                    if (PublicDataClass._ProtocoltyFlag.NetProFlag == 2 || PublicDataClass._ProtocoltyFlag.NetProFlag == 1)//当前101测试走网口
                        PublicDataClass._NetTaskFlag.C_IC_NA_1 = true;  //总召唤
                    if (PublicDataClass._ProtocoltyFlag.ComProFlag == 1 || PublicDataClass._ProtocoltyFlag.ComProFlag == 2)//当前101测试走串
                        PublicDataClass._ComTaskFlag.C_IC_NA_1 = true;  //总召唤
                    ProtoZZTime = PublicDataClass._ProtocoltyFlag.ZZTime;
  
                }
            }

           
              
 
                
        }
        //2013.10.28
        //定时器timer6fram作为串口发数据定时器，在打开串口线程和关闭串口线程时需要同时对定时器进行操作。此修改为了使发送变慢且不影响接收数据
        private void timer6fram_Tick(object sender, EventArgs e)
        {
            if (PublicDataClass._ProtocoltyFlag.ComProFlag == 1)//当串口规约101时
            PtlComFrameProc();
           else  if (PublicDataClass._ProtocoltyFlag.ComProFlag == 2)//当串口规约104时
            {
               //串口任务转换为网口任务
                TTaskComToNet();   
               //采用网口组包函数
                PtlNetFrameProc();
               //网口发送任务改为串口发送任务，发送buffer转换
                TBufferNetToCom();

            }
        }
    
        /********************************************************************************************
*  函数名：    TBufferNetToCom                                                                 *
*  功能  ：    串口任务转换为网口任务                                       *
*  参数  ：    无                                                                           *
*  返回值：    无                                                                           *
*  修改日期：  2013-10-28                                                                   *
*  作者    ：  liuhc                                                                       *
* ******************************************************************************************/
        private void TTaskComToNet()
        {


            if (PublicDataClass._ComTaskFlag.C_RQ_NA_LINKREQ_F == true)    //请求链路状态
            {
                PublicDataClass._ComTaskFlag.C_RQ_NA_LINKREQ_F = false;
                PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_F = true;//复位


            }
            else if (PublicDataClass._ComTaskFlag.C_RQ_NA_LINKREQ_F1 == true)    //链路复位
            {
                PublicDataClass._ComTaskFlag.C_RQ_NA_LINKREQ_F1 = false;

            }
            else if (PublicDataClass._ComTaskFlag.C_IC_NA_1 == true)  //总召唤
            {
                PublicDataClass._ComTaskFlag.C_IC_NA_1 = false;
                PublicDataClass._NetTaskFlag.C_IC_NA_1 = true;


            }

            else if (PublicDataClass._ComTaskFlag.C_CS_NA_1 == true)//校时
            {

                PublicDataClass._ComTaskFlag.C_CS_NA_1 = false;
                PublicDataClass._NetTaskFlag.C_CS_NA_1 = true;//


            }
            else if (PublicDataClass._ComTaskFlag.YK_Sel_1_D == true)
            {
                PublicDataClass._ComTaskFlag.YK_Sel_1_D = false;
                PublicDataClass._NetTaskFlag.YK_Sel_1_D = true;//

            }
            else if (PublicDataClass._ComTaskFlag.YK_Exe_1_D == true)
            {
                PublicDataClass._ComTaskFlag.YK_Exe_1_D = false;
                PublicDataClass._NetTaskFlag.YK_Exe_1_D = true;//

            }
            else if (PublicDataClass._ComTaskFlag.YK_Cel_1_D == true)
            {
                PublicDataClass._ComTaskFlag.YK_Cel_1_D = false;
                PublicDataClass._NetTaskFlag.YK_Cel_1_D = true;//

            }
            else if (PublicDataClass._ComTaskFlag.YK_Sel_1 == true)
            {
                PublicDataClass._ComTaskFlag.YK_Sel_1 = false;
                PublicDataClass._NetTaskFlag.YK_Sel_1 = true;//

            }
            else if (PublicDataClass._ComTaskFlag.YK_Exe_1 == true)
            {
                PublicDataClass._ComTaskFlag.YK_Exe_1 = false;
                PublicDataClass._NetTaskFlag.YK_Exe_1 = true;//

            }
            else if (PublicDataClass._ComTaskFlag.YK_Cel_1 == true)
            {
                PublicDataClass._ComTaskFlag.YK_Cel_1 = false;
                PublicDataClass._NetTaskFlag.YK_Cel_1 = true;//

            }
            else if (PublicDataClass._ComTaskFlag.VERSION_1 == true)    //读版本号
            {
                PublicDataClass._ComTaskFlag.VERSION_1 = false;
                PublicDataClass._NetTaskFlag.VERSION_1 = true;


            }
            else if (PublicDataClass._ComTaskFlag.CALLTIME_1 == true)
            {
                PublicDataClass._ComTaskFlag.CALLTIME_1 = false;
                PublicDataClass._NetTaskFlag.CALLTIME_1 = true;//

            }
            else if (PublicDataClass._ComTaskFlag.READ_P_1 == true)
            {
                PublicDataClass._ComTaskFlag.READ_P_1 = false;
                PublicDataClass._NetTaskFlag.READ_P_1 = true;//

            }
            else if (PublicDataClass._ComTaskFlag.SET_PARAM_CON == true)  //设置参数
            {
                PublicDataClass._ComTaskFlag.SET_PARAM_CON = false;
                PublicDataClass._NetTaskFlag.SET_PARAM_CON = true;//


            }

            else if (PublicDataClass._ComTaskFlag.AloneCallYx_1 == true)
            {
                PublicDataClass._ComTaskFlag.AloneCallYx_1 = false;
                PublicDataClass._NetTaskFlag.AloneCallYx_1 = true;//

            }
            else if (PublicDataClass._ComTaskFlag.AloneCallYc_1 == true)
            {
                PublicDataClass._ComTaskFlag.AloneCallYc_1 = false;
                PublicDataClass._NetTaskFlag.AloneCallYc_1 = true;//


            }
            else if (PublicDataClass._ComTaskFlag.Reset_1 == true)
            {
                PublicDataClass._ComTaskFlag.Reset_1 = false;
                PublicDataClass._NetTaskFlag.Reset_1 = true;//

            }
            else if (PublicDataClass._ComTaskFlag.UpdateCode_Start_1 == true)
            {
                PublicDataClass._ComTaskFlag.UpdateCode_Start_1 = false;
                PublicDataClass._NetTaskFlag.UpdateCode_Start_1 = true;//

            }
            else if (PublicDataClass._ComTaskFlag.UpdateCode_ARMStart_1 == true)
            {
                PublicDataClass._ComTaskFlag.UpdateCode_ARMStart_1 = false;
                PublicDataClass._NetTaskFlag.UpdateCode_ARMStart_1 = true;//

            }

            else if (PublicDataClass._ComTaskFlag.UpdateCode_JY_1 == true)
            {
                PublicDataClass._ComTaskFlag.UpdateCode_JY_1 = false;
                PublicDataClass._NetTaskFlag.UpdateCode_JY_1 = true;//


            }
            else if (PublicDataClass._ComTaskFlag.UpdateCode_ARMJY_1 == true)
            {
                PublicDataClass._ComTaskFlag.UpdateCode_ARMJY_1 = false;
                PublicDataClass._NetTaskFlag.UpdateCode_ARMJY_1 = true;//


            }

            else if (PublicDataClass._ComTaskFlag.UpdateCode_OK_1 == true)
            {
                PublicDataClass._ComTaskFlag.UpdateCode_OK_1 = false;
                PublicDataClass._NetTaskFlag.UpdateCode_OK_1 = true;//


            }
            else if (PublicDataClass._ComTaskFlag.UpdateCode_ARMOK_1 == true)
            {
                PublicDataClass._ComTaskFlag.UpdateCode_ARMOK_1 = false;
                PublicDataClass._NetTaskFlag.UpdateCode_ARMOK_1 = true;//


            }


            else if (PublicDataClass._ComTaskFlag.CallRecordData == true)
            {
                PublicDataClass._ComTaskFlag.CallRecordData = false;
                PublicDataClass._NetTaskFlag.CallRecordData = true;//

            }


            else if (PublicDataClass._ComTaskFlag.READ_Hard_1 == true)
            {

                PublicDataClass._ComTaskFlag.READ_Hard_1 = false;
                PublicDataClass._NetTaskFlag.READ_Hard_1 = true;//


            }
            else if (PublicDataClass._ComTaskFlag.Transmit == true)
            {


                PublicDataClass._ComTaskFlag.Transmit = false;
                PublicDataClass._NetTaskFlag.Transmit = true;//

            }
            else
            { }
        }
           /********************************************************************************************
*  函数名：    TBufferComToNet                                                                 *
*  功能  ：    网口任务转换为串口任务                                       *
*  参数  ：    无                                                                           *
*  返回值：    无                                                                           *
*  修改日期：  2013-10-28                                                                   *
*  作者    ：  liuhc                                                                       *
* ******************************************************************************************/
        private void TTaskNetToCom()
        {
            if (PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_F == true)
            {
                PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_F = false;
                PublicDataClass._ComTaskFlag.C_RQ_NA_LINKREQ_F = true;
            }
            else if (PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_F1 == true)
            {
                PublicDataClass._NetTaskFlag.C_RQ_NA_LINKREQ_F1 =false;
                PublicDataClass._ComTaskFlag.C_RQ_NA_LINKREQ_F1 = true;
            }
            else if (PublicDataClass._NetTaskFlag.C_IC_NA_1 == true)  //总召唤
            {
                PublicDataClass._NetTaskFlag.C_IC_NA_1 = false;
                PublicDataClass._ComTaskFlag.C_IC_NA_1 = true;

            }
            else if (PublicDataClass._NetTaskFlag.C_CS_NA_1 == true)//校时
            {

                PublicDataClass._NetTaskFlag.C_CS_NA_1 = false;
                PublicDataClass._ComTaskFlag.C_CS_NA_1 = true;//


            }
            else if (PublicDataClass._NetTaskFlag.YK_Sel_1_D == true)
            {
                PublicDataClass._NetTaskFlag.YK_Sel_1_D = false;
                PublicDataClass._ComTaskFlag.YK_Sel_1_D = true;//

            }
            else if (PublicDataClass._NetTaskFlag.YK_Exe_1_D == true)
            {
                PublicDataClass._NetTaskFlag.YK_Exe_1_D = false;
                PublicDataClass._ComTaskFlag.YK_Exe_1_D = true;//

            }
            else if (PublicDataClass._NetTaskFlag.YK_Cel_1_D == true)
            {
                PublicDataClass._NetTaskFlag.YK_Cel_1_D = false;
                PublicDataClass._ComTaskFlag.YK_Cel_1_D = true;//

            }
            else if (PublicDataClass._NetTaskFlag.YK_Sel_1 == true)
            {
                PublicDataClass._NetTaskFlag.YK_Sel_1 = false;
                PublicDataClass._ComTaskFlag.YK_Sel_1 = true;//

            }
            else if (PublicDataClass._NetTaskFlag.YK_Exe_1 == true)
            {
                PublicDataClass._NetTaskFlag.YK_Exe_1 = false;
                PublicDataClass._ComTaskFlag.YK_Exe_1 = true;//

            }
            else if (PublicDataClass._NetTaskFlag.YK_Cel_1 == true)
            {
                PublicDataClass._NetTaskFlag.YK_Cel_1 = false;
                PublicDataClass._ComTaskFlag.YK_Cel_1 = true;//

            }
            else if (PublicDataClass._NetTaskFlag.VERSION_1 == true)    //读版本号
            {
                PublicDataClass._NetTaskFlag.VERSION_1 = false;
                PublicDataClass._ComTaskFlag.VERSION_1 = true;


            }
            else if (PublicDataClass._NetTaskFlag.CALLTIME_1 == true)
            {
                PublicDataClass._NetTaskFlag.CALLTIME_1 = false;
                PublicDataClass._ComTaskFlag.CALLTIME_1 = true;//

            }
            else if (PublicDataClass._NetTaskFlag.READ_P_1 == true)
            {
                PublicDataClass._NetTaskFlag.READ_P_1 = false;
                PublicDataClass._ComTaskFlag.READ_P_1 = true;//

            }
            else if (PublicDataClass._NetTaskFlag.SET_PARAM_CON == true)  //设置参数
            {
                PublicDataClass._NetTaskFlag.SET_PARAM_CON = false;
                PublicDataClass._ComTaskFlag.SET_PARAM_CON = true;//


            }

            else if (PublicDataClass._NetTaskFlag.AloneCallYx_1 == true)
            {
                PublicDataClass._NetTaskFlag.AloneCallYx_1 = false;
                PublicDataClass._ComTaskFlag.AloneCallYx_1 = true;//

            }
            else if (PublicDataClass._NetTaskFlag.AloneCallYc_1 == true)
            {
                PublicDataClass._NetTaskFlag.AloneCallYc_1 = false;
                PublicDataClass._ComTaskFlag.AloneCallYc_1 = true;//


            }
            else if (PublicDataClass._NetTaskFlag.Reset_1 == true)
            {
                PublicDataClass._NetTaskFlag.Reset_1 = false;
                PublicDataClass._ComTaskFlag.Reset_1 = true;//

            }
            else if (PublicDataClass._NetTaskFlag.UpdateCode_Start_1 == true)
            {
                PublicDataClass._NetTaskFlag.UpdateCode_Start_1 = false;
                PublicDataClass._ComTaskFlag.UpdateCode_Start_1 = true;//

            }
            else if (PublicDataClass._NetTaskFlag.UpdateCode_ARMStart_1 == true)
            {
                PublicDataClass._NetTaskFlag.UpdateCode_ARMStart_1 = false;
                PublicDataClass._ComTaskFlag.UpdateCode_ARMStart_1 = true;//

            }

            else if (PublicDataClass._NetTaskFlag.UpdateCode_JY_1 == true)
            {
                PublicDataClass._NetTaskFlag.UpdateCode_JY_1 = false;
                PublicDataClass._ComTaskFlag.UpdateCode_JY_1 = true;//


            }
            else if (PublicDataClass._NetTaskFlag.UpdateCode_ARMJY_1 == true)
            {
                PublicDataClass._NetTaskFlag.UpdateCode_ARMJY_1 = false;
                PublicDataClass._ComTaskFlag.UpdateCode_ARMJY_1 = true;//


            }

            else if (PublicDataClass._NetTaskFlag.UpdateCode_OK_1 == true)
            {
                PublicDataClass._NetTaskFlag.UpdateCode_OK_1 = false;
                PublicDataClass._ComTaskFlag.UpdateCode_OK_1 = true;//


            }
            else if (PublicDataClass._NetTaskFlag.UpdateCode_ARMOK_1 == true)
            {
                PublicDataClass._NetTaskFlag.UpdateCode_ARMOK_1 = false;
                PublicDataClass._ComTaskFlag.UpdateCode_ARMOK_1 = true;//


            }


            else if (PublicDataClass._NetTaskFlag.CallRecordData == true)
            {
                PublicDataClass._NetTaskFlag.CallRecordData = false;
                PublicDataClass._ComTaskFlag.CallRecordData = true;//

            }


            else if (PublicDataClass._NetTaskFlag.READ_Hard_1 == true)
            {

                PublicDataClass._NetTaskFlag.READ_Hard_1 = false;
                PublicDataClass._ComTaskFlag.READ_Hard_1 = true;//


            }
            else
            { }
        }
        /********************************************************************************************
     *  函数名：    TBufferNetToCom                                                                 *
     *  功能  ：    网口发送Buffer转为串口发送Buffer                                        *
     *  参数  ：    无                                                                           *
     *  返回值：    无                                                                           *
     *  修改日期：  2013-10-28                                                                   *
     *  作者    ：  liuhc                                                                       *
     * ******************************************************************************************/
        private void TBufferNetToCom()
        {
            if (PublicDataClass._NetStructData.TX_TASK == true)
            {
                PublicDataClass._NetStructData.TX_TASK = false;
                PublicDataClass._ComStructData.TX_TASK = true;
                PublicDataClass._ComStructData.TxLen = PublicDataClass._NetStructData.NetTLen;
                for (int i = 0; i < PublicDataClass._ComStructData.TxLen; i++)
                    PublicDataClass._ComStructData.TXBuffer[i] = PublicDataClass._NetStructData.NetTBuffer[i];
                PublicDataClass.ComFrameMsg = PublicDataClass.SedNetFrameMsg;
            }
        
        }

        private void TBufferComToNet()
        {
            if (PublicDataClass._ComStructData.TX_TASK == true)
            {
                PublicDataClass._ComStructData.TX_TASK = false;
                PublicDataClass._NetStructData.TX_TASK = true;
                PublicDataClass._NetStructData.NetTLen = PublicDataClass._ComStructData.TxLen;
                for (int i = 0; i < PublicDataClass._NetStructData.NetTLen; i++)
                    PublicDataClass._NetStructData.NetTBuffer[i] = PublicDataClass._ComStructData.TXBuffer[i];
                PublicDataClass.SedNetFrameMsg = PublicDataClass.ComFrameMsg;
            }

        }
        //串口接收Buffer转为网口接收Buffer      
        public static void RBufferComToNet()
        {
            PublicDataClass._NetStructData.NetRLen = PublicDataClass._ComStructData.RxLen;
            for (int i = 0; i < PublicDataClass._NetStructData.NetRLen; i++)
                PublicDataClass._NetStructData.NetRBuffer[i] = PublicDataClass._ComStructData.RXBuffer[i];
          //  PublicDataClass.ComFrameMsg =PublicDataClass.RevNetFrameMsg ;
        }

        //网口接收Buffer转为串口接收Buffer      
        public static void RBufferNetToCom()
        {
             PublicDataClass._ComStructData.RxLen=PublicDataClass._NetStructData.NetRLen ;
             for (int i = 0; i < PublicDataClass._ComStructData.RxLen; i++)
              PublicDataClass._ComStructData.RXBuffer[i]=   PublicDataClass._NetStructData.NetRBuffer[i];
            //  PublicDataClass.ComFrameMsg =PublicDataClass.RevNetFrameMsg ;
        }
        //单招定时器此过程中停止发召唤一级二级数据
        private void timer6AlongCall_Tick(object sender, EventArgs e)
        {
            AlongCallTime++;
            if (AlongCallTime > 400)
            {
                AlongCallTime = 0;
                timer6AlongCall.Enabled = false;
                PublicDataClass._ProtocoltyFlag.AloneCallYxYcFlag = false;
                //timer6fram.Enabled = true ;
            }
        }

        private void timer6_Tick(object sender, EventArgs e)
        {

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            m_RPview.Show(dockPanel);

        }

        private void 继保定值视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PublicDataClass.PrjType <= 0)
                return;
            继保定值视图ToolStripMenuItem.Checked = true;
            m_RPview.Show(dockPanel);
        }

        private void xML配置视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_xmlcfgview.Show(dockPanel);
        }


    }
}
