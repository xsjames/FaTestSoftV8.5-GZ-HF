using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using ZedGraph;

namespace FaTestSoft
{
    namespace CommonData                        //定义它的命名空间
    {

        class PublicDataClass
        {

            #region My STRING
            /// <summary>
            /// 字符串型数据区
            /// </summary>
            public static string PrjPath;        //工程路径
            public static string PrjName;       //工程名字
            public static string IPADDR, ClientInfo;
            public static string PORT;
            public static string COMID;
            public static string[] FILENAME;
            public static string[] XIEFILENAME;
            public static string LinSPointName;
            public static string RevNetFrameMsg;        //帧报文信息
            public static string SedNetFrameMsg;        //帧报文信息
            public static string GprsFrameMsg;        //帧报文信息
            public static string ComFrameMsg;        //帧报文信息
            public static string UsbFrameMsg;        //帧报文信息
            public static string Manger;
            #endregion

            #region My INT
            /// <summary>
            /// int型数据区
            /// </summary>

            public static int LoginType;//登录用户类型 1：生产测试版
            public static int PrjType;      //打开的类型
            public static int TcpLinkType;
            public static int ParamInfoAddr;
            public static int YkStartPos;     //遥控起始点号 
            public static int DevAddr;
            public static int LinkAddr;
            public static ushort TxSeq = 0; //发送序列号
            public static ushort RxSeq = 0; //接收序列号
           
            public static int showtype = 0;//显示类型 0：浮点型  1： 整型
            public static int EquipmentType = 0;//设备类型: 1：2路 2:6路 3:6U  4:4U
            public static int SoftPageShowFlag;//软件显示标志，暂时为点号表用
            public static int DynOptCfgFlag = 0;//动态选项卡配置标志
            public static int DynOptHaveFlag = 0;//有动态参数文件标志
            public static int DynOptHaveNum = 0;//动态参数文件计数

            public static int XieDynOptCfgFlag = 0;//协处理器动态选项卡配置标志

            public static byte dataty;      //
            public static byte addselect = 2;//
            public static byte seqflag;      //
            public static byte seq;      //
            public static byte SQflag;      //
            public static byte ZDYtype;      //

            public static int linklen;      //
            public static int cotlen ;//
            public static int publen ;      //
            public static int inflen ;

            public static int Framelen = 800;
            //public static int Framelen = 794;
            //public static int CodeUpdatalen = 506;
            public static int CodeUpdatalen = 512;

            #endregion

            #region My BYTE
            /// <summary>
            /// 字节型数据区
            /// </summary>
            public static byte YkState;
            public static byte Menu;
            public static byte NetIndex;       //网络序号
            public static byte ChanelId;
            public static byte DataTy;

            #endregion

            #region My Structs

            public struct _ChangeFlag
            {
                public static bool ProductionVersionViewUpdate;
                public static bool FunlistViewTreeNeedUpdate;
                public static bool ToolBoxTreeNeedUpdate;
                public static bool ChannelClassUpdata;
                public static bool DeviceCalssUpdate;
                public static bool YcYxDeviceCalssUpdate;
                public static bool XtParamUpdate;
                public static bool XtParamUpdate1;   //zxl
                public static bool MoniterUpdate;
             
                public static bool CallDataUpdate;
                public static bool HistoryDataUpdate;
                public static bool AcquisitionParamUpdate;
                public static bool AcquisitionParamUpdate1;
                public static bool ControlViewUpdate;
                public static bool ControlViewUpdate1;
                public static bool OtherTypeViewUpdate;
                public static bool XieCPU;
             
                public static bool OtherTypeViewUpdate1;
                public static bool CePointClassUpdate;
                public static bool RoleMangerViewUpdate;
                public static bool ChangInfoViewUpdate;
                public static bool DocmentViewUpdate;
                public static bool Setflag;
                public static bool InitState;
                public static bool SoftMangerViewUpdate;
                public static bool Clearflag = false;
                public static bool Clearflag1 = false;
                public static bool Clearflag2 = false;
                public static bool YxkjCfg = false;//快捷配置
                public static bool YxzsCfg = false;//遥信置数快捷配置
                public static bool CodeUpdateFlag = false;

                //public static bool ContinueConnectFlag = false;//自动连接标志
                ///////////////////////生产测试版/////////////////////////
                public static bool CommunicationTestUpdate;
                public static bool AlongCall;


                ////////////////////////////////////////////////

            }
            public struct _ProtocoltyFlag
            {
                public static int ACD;
                public static int FCB;
                public static int NetProFlag;
                public static int ComProFlag;
                public static int DelayTime;//重发时间
           
                public static int ZZTime;//总召循环时间
                public static bool Test101;
                public static bool Test104;
                public static bool ZZFlag;  //
                public static bool ZZFirstFlag;  //
                public static bool AloneCallYxYcFlag;

            
            
            }
            public struct _WindowsVisable      //------------------------窗体的可视状态
            {
                //public static bool FunlistViewTreeNeedVisable ;
                //public static bool ToolBoxTreeNeedVisable ;
                //public static bool ChannelClassUpdataVisable ;
                //public static bool DeviceCalssUpdateVisable ;
                public static bool XtParamUpdateVisable;
                public static bool RelayProtectVisable;//继保定值视图
                public static bool XietParamVisable;//协处理器视图
                //public static bool MoniterUpdate;
                public static bool RealTimeDataVisable;
                public static bool CallDataUpdatVisable;
                public static bool HistoryDataUpdateVisable;
                public static bool AcquisitionParamVisable;
                public static bool DateCodFormVisable;
                //public static bool ControlViewVisable;
                // public static bool OtherTypeViewVisable;
                // public static bool CePointClassVisable;
                // public static bool RoleMangerViewVisable;
            }
            public struct _ThreadIndex        //-----------------------召测数据的顺序
            {
                public static int NetThreadID;
                public static int ComThreadID;
                public static int GprsThreadID;
                public static int UsbThreadID;
                public static int RetryNetThreadID;

            }
            public struct _Time       //-----------------------召测数据的时间
            {
                public static int NetReTime;           //网络的帧重发时间
                public static int ComReTime;
                public static int NetDelayTime;        //网络的帧延时时间
                public static int ComDelayTime;        //串口的帧延时时间

            }
            public struct _Message
            {
                public static bool GprsShowSendTextData;
                public static bool GprsShowRecvTextData;
                public static bool ComShowSendTextData;
                public static bool ComShowRecvTextData;
                public static bool UsbShowSendTextData;
                public static bool UsbShowRecvTextData;
                public static bool NetShowSendTextData;
                public static bool NetShowSendOver;
                public static bool NetShowRecvTextData;
                public static bool NetShowDelayTimeMsg;
                public static bool TcpLinkState;
                public static bool GprsAcceptState;
                public static bool CallDataDocmentView;
                public static bool CallRelayProtectView;
                public static bool RealTimeDataDocmentView;
                public static bool CodeUpdateDoing;
                public static bool CodeUpdateJY;
                public static bool CodeUpdateARMJY;
                public static bool YkDocmentView;
                public static bool LinkInfo;
                public static bool ParamAck;
                public static bool YueXianEvent;
                public static bool YueXianEvent1;
                public static bool YxBianWeiOfNoTimeEvent;
                public static bool YxBianWeiOfTimeEvent;
                public static bool PowOffEvent;
                public static bool RaoDongEvent;
                public static bool FaultEvent;
                public static bool CallHisDataDocmentView;
                public static bool ReadParam;
                public static bool LinkUpdata;

            }
            //------------------------------------------------------------------------报文显示结构体
            public struct OutPutMessage                 
            {
                public static int type;//1.2.I帧            
                public static string length;//数据单元长度
                public static string txseq;//发送序号
                public static string rxseq;//接收序号
                public static string TypeID;
                public static string COT;//
                public static string VSQ;//

                public static int S_type;//1.2.I帧            
                public static string S_length;//数据单元长度
                public static string S_txseq;//发送序号
                public static string S_rxseq;//接收序号
                public static string S_TypeID;
                public static string S_COT;//
                public static string S_VSQ;//
             
            }
            //------------------------------------------------------------------------遥控信息结构体
            public struct YkIniInfo                    //遥控ini文件的数据结构
            {
                public static int num;
                public static string[] NameTable;
                public static string[] StartPosTable;
            }
            //------------------------------------------------------------------------协议信息结构体
            public struct ProtocolIniInfo             //协议ini文件的数据结构
            {
                public static int num;
                public static string[] IniTable;
            }
            //------------------------------------------------------------------------波特率信息结构体
            public struct BaudRateAJyIniInfo             //波特率与校验ini文件的数据结构
            {
                public static int Bnum;
                public static int JYnum;
                public static string[] BIniTable;
                public static string[] JIniTable;
            }
            //------------------------------------------------------------------------端口号信息结构体
            public struct PortIniInfo             //端口ini文件的数据结构
            {
                public static int num;
                public static string[] IniTable;
            }
            //------------------------------------------------------------------------遥测遥信遥脉名称结构体
            public struct ThreeYNameTable        //3遥名称表
            {
                public static int Ycnum;
                public static int Yxnum;
                public static int Ymnum;
                public static int YxOfinnum;
                public static ArrayList YcTable = new ArrayList();         //从System.Collections中继承  动态数组的声明
                public static ArrayList YxTable = new ArrayList();
                public static ArrayList YmTable = new ArrayList();
                public static ArrayList YxOfinTable = new ArrayList();
            }
            //------------------------------------------------------------------------遥测遥信遥脉信息体结构体
            public struct InfoAddrTable        //3遥信息体地址表
            {
                public static int Ycnum;
                public static int Yxnum;
                public static int Ymnum;
                public static ArrayList YcInfoATable = new ArrayList();         //从System.Collections中继承  动态数组的声明
                public static ArrayList YxInfoATable = new ArrayList();
                public static ArrayList YmInfoATable = new ArrayList();
            }

            //------------------------------------------------------------------------网络参数结构体
            public struct _NetParam
            {
                public static int num;
                public static string IP;
                public static string GwIP;
                public static string SubMask;
                public static string Port;
                public static string SrcHA;

                public static string[] NameTable;
                public static string[] ValueTable;
                public static string[] ByteTable;
            }
           
            //------------------------------------------------------------------------串口参数结构体
            public struct _ComParam
            {
                public static int num;
                public static string[] BaudRateTable;
                public static string[] JyTable;
                public static string[] DataBitTable;
                public static string[] NameTable;
                public static string[] ValueTable;
                public static string[] ByteTable;
            }
            //------------------------------------------------------------------------系统参数结构体
            public struct _SysParam
            {
                public static int num;
                public static string[] NameTable;
                public static string[] ValueTable;
                public static string[] ByteTable;
                public static int selindex;
            }
            //------------------------------------------------------------------------遥测配置结构体
            public struct _YcConfigParam
            {
                public static int num;
                public static int ZBStartYcNum;
                public static int ZBYcNum;
                public static string[] NameTable;
                public static string[] AddrTable;
                public static string[] IndexTable;
                public static string[] DatatypeTable;
                public static string[] MagnificationTable;
                public static string[] ConnectTable;
                public static string[] QufanTable;
                public static string[] setvalueTable;
                public static string[] ValueTable;
                public static string[] ByteTable;

            }
            //------------------------------------------------------------------------遥信配置结构体
            public struct _YxConfigParam
            {
                public static int num;
                public static int wyxnum;
                public static int ZBYxNum;
                public static string[] NameTable;
                public static string[] AddrTable;
                public static string[] IndexTable;
                public static string[] QufanTable;
                public static string[] setvalueTable;
                public static string[] ValueTable;
                public static string[] ByteTable;

            }
            //------------------------------------------------------------------------遥信配置结构体
            public struct _YkConfigParam
            {
                public static int num;
                public static string[] NameTable;
                public static string[] AddrTable;
                public static string[] triggermodeTable;
                public static string[] secltimeTable;
                public static string[] exetimeTable;
                public static string[] pulsewidthTable;
                public static string[] saveflagTable;
                public static string[] powerTable;
                public static string[] jdq1Table;
                public static string[] jdq2Table;
                public static string[] fjyx1Table;
                public static string[] fjyx2Table;
                public static string[] ByteTable;

                public static bool Ykinputflag;
                public static int selindex;
            }
            public struct _AIParam
            {
                public static int num;
                public static string[] quality;
                public static string[] phase;
                public static string[] line;
                public static string[] panel;
                public static string[] value;
                public static string[] ph;
                public static string[] zhishu;

                public static string remendquality;
                public static int remendtype;
                public static float remendvalue;
                public static int updown;
                
            }
            //------------------------------------------------------------------------反时限保护参数结构体
            public struct _FsxParam
            {
                public static string IdataValue;
                public static byte Checkeddata;

            }

            //------------------------------------------------------------------------运行参数结构体
            public struct _RunParam
            {
                public static int num;
                public static string[] NameTable;
                public static string[] ValueTable;
                public static string[] ByteTable;
            }

            //------------------------------------------------------------------------功能参数结构体*********
            public struct _FuncParam
            {
                public static int num;
                public static string[] NameTable;
                public static string[] ValueTable;
                public static string[] ByteTable;

            }
            //------------------------------------------------------------------------功能配置参数结构体*********
            public struct _FuncConfigParam
            {
                public static int num;
                public static string[] NameTable;
                public static string[] ValueTable;
                public static string[] ByteTable;

            }
            //------------------------------------------------------------------------继保定值结构体*********
            public struct _RelayProtectParam
            {
                public static int num;
                public static string[] NameTable;
                public static string[] ValueTable;
                public static string[] AddrTable;
                public static string[] LineTable;

            }
            //------------------------------------------------------------------------遥测参数结构体
            public struct _YcParam
            {
                public static int num;
                public static string[] NameTable;
                public static string[] ValueTable;
                public static string[] ByteTable;
            }
            //------------------------------------------------------------------------遥信参数结构体
            public struct _YxParam
            {
                public static int num;
                public static string[] NameTable;
                public static string[] ValueTable;
                public static string[] ByteTable;
            }
            //------------------------------------------------------------------------遥控参数结构体
            public struct _YkParam
            {
                public static int num;
                public static string[] NameTable;
                public static string[] ValueTable;
                public static string[] ByteTable;
            }
            //------------------------------------------------------------------------遥控参数结构体
            public struct _GprsParam
            {
                public static int num;
                public static string IP;
                public static string BIP;
                public static string Port;
                public static string BPort;
                public static string Heart;
                public static string APN;
            }
            //------------------------------------------------------------------------遥信接入配置参数结构体
            public struct _YxLineCfgParam
            {
                public static int num;
                public static string[] NameTable;
                public static string[] ValueTable;
                public static string[] ByteTable;
            }
            //------------------------------------------------------------------------遥测接入配置参数结构体
            public struct _YcLineCfgParam
            {
                public static int num;
                public static string[] NameTable;
                public static string[] ValueTable;
                public static string[] ByteTable;
            }


            //------------------------------------------------------------遥测点号表配置参数结构体
            public struct _YcDotParam
            {
                public static int num;
                public static string[] NameTable;
                public static string[] BusNumTable;
                public static string[] CardNumTable;
                public static string[] UBusConnectionmodeTable;
                public static string[] IBusConnectionmodeTable;

            }

            //------------------------------------------------------------------------遥测信息表配置参数结构体

            public struct _YcInformationParam
            {
                public static int num;
                public static string[] NameTable;
                public static string[] IndexTable;
                public static string[] DatatypeTable;
                public static string[] MagnificationTable;

            }
            //------------------------------------------------------------------------遥测信息表配置参数结构体

            public struct _YxDataQFParam
            {
                public static int num;
                public static string[] DataQFTable;

                public static string[] ByteTable;



            }
            //------------------------------------------------------------------------显示序号结构体

            public struct _ShowIndexTableParam
            {

                public static int Ycnum;
                public static int Yxnum;
                public static int Ymnum;
                public static ArrayList YcShowIndexTable = new ArrayList();         //从System.Collections中继承  动态数组的声明
                public static ArrayList YxShowIndexTable = new ArrayList();
                public static ArrayList YmShowIndexTable = new ArrayList();

            }

            //------------------------------------------------------------------------串口数据缓存
            public struct _ComStructData
            {
                public static byte[] RXBuffer = new byte[1024];
                public static byte[] TXBuffer = new byte[1024];
                public static int TxLen;
                public static int RxLen;
                public static bool TX_TASK;
                public static bool FCB = false;

            }
            //------------------------------------------------------------------------串口任务处理
            public struct _ComTaskFlag
            {
                public static bool C_P1_NA_F;
                public static bool C_RQ_NA_LINKREQ_F;//请求链路状态
                public static bool C_RQ_NA_LINKREQ_F1;//复位链路
                public static bool FirstON_S;
                public static bool SET_PARAM_CON;
                public static bool C_CS_NA_1;             //对时
                public static bool C_IC_NA_1;
                public static bool CALL_1;//
                public static bool CALL_2;//
                public static bool YK_Sel_1;
                public static bool YK_Exe_1;
                public static bool YK_Cel_1;
                public static bool VERSION_1;
                public static bool CALLTIME_1;
                public static bool READ_P_1;
                public static bool AloneCallYc_1;
                public static bool AloneCallYx_1;
                public static bool AloneCallYx_2;
                public static bool UpdateCode_Start_1;
                public static bool UpdateCode_JY_1;
                public static bool UpdateCode_OK_1;
                public static bool UpdateCode_ARMStart_1;
                public static bool UpdateCode_ARMJY_1;
                public static bool UpdateCode_ARMOK_1;
                public static bool Reset_1;
                public static bool CallHisData;
                public static bool CallRecordData;
                public static bool FuGuiCmd;
                public static bool YK_Sel_1_D;
                public static bool YK_Exe_1_D;
                public static bool YK_Cel_1_D;
                public static bool READ_Hard_1;
                public static bool READ_RYB_1;
                public static bool SET_RYB_1;

                public static bool Transmit; //数据转发
            }
            //------------------------------------------------------------------------网络数据缓存
            public struct _NetStructData
            {
                public static byte[] NetTBuffer = new byte[1024];
                public static byte[] NetRBuffer = new byte[1024];
                public static int NetTLen;
                public static int NetRLen;
                public static bool TX_TASK;

            }
            //------------------------------------------------------------------------网口任务处理
            public struct _NetTaskFlag
            {
                public static bool C_P1_NA_F;
                public static bool C_RQ_NA_LINKREQ_F;    //启动传输，复位帧
                public static bool C_RQ_NA_LINKREQ_OK;    //复位确认帧
                public static bool C_RQ_NA_LINKREQ_F1;    //启动传输，复位帧101
                public static bool C_RQ_NA_LINKREQ_OK1;    //复位确认帧101
                public static bool CALL_1;//网口走101标志召唤一级数据
                public static bool CALL_2;//网口走101标志召唤二级数据
                public static bool STOP_LINKREQ;    //
                public static bool STOP_LINKREQ_OK;    //
                public static bool Do_TESTACT;           //测试帧
                public static bool Do_TESTACT_OK;           //测试确认帧
                public static bool Do_OKTACT;            //S格式确认帧

                public static bool FirstON_S;
                public static bool SET_PARAM_CON;
                public static bool C_IC_NA_1;             //总召唤
                public static bool C_CS_NA_1;             //对时

                public static bool YK_Sel_1;
                public static bool YK_Exe_1;
                public static bool YK_Cel_1;
                public static bool VERSION_1;
                public static bool CALLTIME_1;
                public static bool READ_P_1;
                public static bool READ_ReP_1;//召唤继保定值
                public static bool SET_ReP;//下装继保定值
                public static bool ACT_ReP;//激活继保定值
                public static bool CancelACT_ReP;//撤销激活继保定值
                public static bool ARMX_DOWN;
                public static bool UpdateCode_JY_1;
                public static bool UpdateCode_OK_1;
                public static bool UpdateCode_Start_1;
                public static bool UpdateCode_ARMStart_1;
                public static bool UpdateCode_ARMJY_1;
                public static bool UpdateCode_ARMOK_1;
                //public static bool UpdateCode_Stop_1;
                public static bool AloneCallYc_1;
                public static bool AloneCallYx_1;
                public static bool AloneCallYx_2;
                public static bool Reset_1;
                public static bool CallHisData;
                public static bool CallRecordData;
                public static bool FuGuiCmd;
                public static bool YK_Sel_1_D;
                public static bool YK_Exe_1_D;
                public static bool YK_Cel_1_D;
                public static bool READ_Hard_1;
                public static bool SET_RYB_1;
                public static bool READ_RYB_1;
                public static bool SET_YC_1;
                public static bool SET_YC_2;
                public static bool SET_YC_3;
                public static bool CEL_YC_1;
                public static bool CEL_YC_2;
                public static bool CEL_YC_3;
                public static bool Transmit; //数据转发
            }
            //------------------------------------------------------------------------gprs数据缓存
            public struct _GprsStructData
            {
                public static byte[] TBuffer = new byte[1024];
                public static byte[] RBuffer = new byte[1024];
                public static int GprsTLen;
                public static int GprsRLen;
                public static bool TX_TASK;

            }
            //------------------------------------------------------------------------Gprs任务处理
            public struct _GprsTaskFlag
            {
                public static bool C_P1_NA_F;
                public static bool C_RQ_NA_LINKREQ_F;
                //public static bool Do_OKTACT;            //确认帧
                //public static bool Do_TESTACT;           //测试帧
                public static bool FirstON_S;
                public static bool SET_PARAM_CON;
                public static bool C_IC_NA_1;             //总召唤
                public static bool C_CS_NA_1;             //对时
                public static bool YK_Sel_1;
                public static bool YK_Exe_1;
                public static bool YK_Cel_1;
                public static bool VERSION_1;
                public static bool CALLTIME_1;
                public static bool READ_P_1;
                public static bool UpdateCode_JY_1;
                public static bool UpdateCode_OK_1;
                public static bool UpdateCode_Start_1;
                public static bool UpdateCode_ARMStart_1;
                public static bool UpdateCode_ARMJY_1;
                public static bool UpdateCode_ARMOK_1;
                public static bool UpdateCode_Stop_1;
                public static bool AloneCallYc_1;
                public static bool AloneCallYx_1;
                public static bool AloneCallYx_2;
                public static bool Reset_1;
                public static bool CallHisData;
                public static bool CallRecordData;
                public static bool FuGuiCmd;
                public static bool YK_Sel_1_D;
                public static bool YK_Exe_1_D;
                public static bool YK_Cel_1_D;
                public static bool READ_Hard_1;
                public static bool READ_RYB_1;
            }
            //------------------------------------------------------------------------USB数据缓存
            public struct _UsbStructData
            {
                public static byte[] TBuffer = new byte[1024];
                public static byte[] RBuffer = new byte[1024];
                public static int UsbTLen;
                public static int UsbRLen;
                public static bool TX_TASK;

            }
            //------------------------------------------------------------------------USB任务处理
            public struct _UsbTaskFlag
            {
                public static bool C_P1_NA_F;
                public static bool C_RQ_NA_LINKREQ_F;
                //public static bool Do_OKTACT;            //确认帧
                //public static bool Do_TESTACT;           //测试帧
                public static bool FirstON_S;
                public static bool SET_PARAM_CON;
                public static bool C_IC_NA_1;             //总召唤
                public static bool C_CS_NA_1;             //对时
                public static bool YK_Sel_1;
                public static bool YK_Exe_1;
                public static bool YK_Cel_1;
                public static bool VERSION_1;
                public static bool CALLTIME_1;
                public static bool READ_P_1;
                public static bool UpdateCode_JY_1;
                public static bool UpdateCode_OK_1;
                public static bool UpdateCode_Start_1;
                public static bool UpdateCode_ARMStart_1;
                public static bool UpdateCode_ARMJY_1;
                public static bool UpdateCode_ARMOK_1;
                public static bool UpdateCode_Stop_1;
                public static bool AloneCallYc_1;
                public static bool AloneCallYx_1;
                public static bool AloneCallYx_2;
                public static bool Reset_1;
                public static bool CallHisData;
                public static bool CallRecordData;
                public static bool YK_Sel_1_D;
                public static bool YK_Exe_1_D;
                public static bool YK_Cel_1_D;
                public static bool READ_Hard_1;
                public static bool READ_RYB_1;
            }
            //-----------------------------------------------------------------------通道类结构体
            public struct CHANNELINFO
            {

                public string ChannelID;
                public string potocol;
                public string baud;
                public string jy;
                public string port;
                public string IP;
                public string RelayTime;
                public string calltype;

            }
            //-----------------------------------------------------------------------设备类结构体
            public struct DEVICEINFO
            {

                public string PointName;                 //测量点名称
                public string ChannelID;                 //测量点所在的通道
                public string Addr;
                public string State;

            }
            //-----------------------------------------------------------------------配置类结构体
            public struct INFOADDRCFGINFO
            {

                public string PointName;                 //测量点名称
                public int YxcfgNum;                     //yx配置了多少个
                public int YccfgNum;
                public int YkcfgNum;
                public string[] YxcfgName;               //yx配置了那些名称
                public string[] YxcfgAddr;               //yx配置了那些信息体
                public string[] YxcfgState;              //yx配置了那些接入状态 
                public string[] Yxdataqf;                  //遥信数据取反
                public string[] YxcfgIndex;
                public string[] Yxdata;                  //遥信数据

                public string[] YccfgName;
                public string[] YccfgAddr;
                public string[] YccfgState;
                public string[] Ycdataqf;                  //遥信数据取反
                public string[] YccfgIndex;
                public string[] Ycdata;

                public string[] YkcfgName;
                public string[] YkcfgAddr;
                public string[] YkcfgState;
                public string[] Ykdata;
               


            }
            public struct SaveText
            {
                public static int channelnum;               //通道有多少个
                public static int devicenum;                //测量点有多少个
                public static int cfgnum;                   //配置多少个
                public static CHANNELINFO[] Channel;
                public static DEVICEINFO[] Device;
                public static INFOADDRCFGINFO[] Cfg;
                public static bool YCcfgflag;


            }
            //------------------------------------------------------------------------动态TabPage信息体结构体
            public struct TabPageValueTable
            {

                //   public  ArrayList ValueTable = new ArrayList();
                public string[] ValueTable;
            }
            public struct TabPageCfgParam
            {

                public int LineNum;                          //行数
                public int ColumnNum;                     //列数
                public string PageName;                    //标签名称
                public int DownAddr;                         //下载地址
                public string[] ColumnPageName;   //列名称
                public string[] ColumnDownByte;   //列名称
                public TabPageValueTable[] TabPageValue;         //从System.Collections中继承  动态数组的声明

            }

            public static TabPageCfgParam[] TabCfg;
            public static TabPageCfgParam[] XieTabCfg;


            public struct _OpenLinkState
            {

                public static int Linknum;                  //需要连接的多少个
                public static string[] LinkDevName;
                public static bool HaveCom;
                public static bool HaveNet;
                public static bool HaveGprs;
                public static bool HaveUsb;

            }
            public struct _CallType
            {
                public static int NetTy;
                public static int ComTy;

            }

            //-----------------------------------------------------------------------串口类的状态
            public struct _ComState
            {

                public static bool[] IsUse = new bool[10];                  //
            }
            //-------------------
            public struct _DataField
            {
                public static byte[] Buffer = new byte[1024];

                public static int FieldLen;
                public static int FieldVSQ;

            }
            public struct _AddYkRecord
            {
                public static string Name;
                public static string Pos;

            }
            public struct _AddInfoRecord
            {
                public static string Name;
                public static string Pos;

            }
            public struct _AddParamRecord
            {
                public static string Name;
                public static string Value;
                public static string Beilv;

            }
            public struct _FastParamRecord
            {
                public static byte ItemId;
                public static int num;
                public static int[] index;
                //public static string[] yxzs;
                public static int[] alarmX;
                public static int[] alarmY;
                public static string[] addr;

                public static int num1=0;
                public static string[] CDTindex1;
                public static string[] CDTname1;

                public static int num2=0;
                public static string[] CDTindex2;
                public static string[] CDTname2;

                public static int num3=0;
                public static string[] CDTindex3;
                public static string[] CDTname3;
            }

            public struct _AddYcDotParamRecord
            {
                public static string Name;
                public static string BusNum;
                public static string CardNum;
                public static string UBusConnectionmode;
                public static string IBusConnectionmode;


            }
            public struct _AddYcInformationRecord
            {
                public static string Name;
                public static string Index;
                public static string Datatype;
                public static string Magnification;

            }

            public struct _AddRoleRecord
            {
                public static string User;
                public static string password;
               

            }
            public struct _FileHeadInfo
            {
                //public static int FileID;
                public static int SegNum;
                public static byte CheckSum;
                public static short  ARMCheckSum;
                public static byte DataBlockNum;
                //public static int RES;
                public static int FileLen;
            }
            public struct _ROLEMANEGER
            {
                public string user;
                public string Password;
                public string state;
                public bool rightCfgState;
                public bool [] AcqParamFlag;//参数配置页面显示标志
                public bool [] DynOpt;//动态选项卡
                public bool MoniterFlag;//实时监视
                public bool CallDataFlag;//实时召测
                public bool HistoryDataFlag;//历史数据
                public bool [] ControlViewFlag;//控制命令
                public bool [] OtherTypeFlag;//其它操作
                public bool RoleMangerFlag;//用户管理
                
            }
            public struct CURRNTRIGHTCFG
            {
                public static bool[] AcqParamFlag;//参数配置页面显示标志
                public static bool[] DynOpt;//动态选项卡
                public static bool MoniterFlag;//实时监视
                public static bool CallDataFlag;//实时召测
                public static bool HistoryDataFlag;//历史数据
                public static bool[] ControlViewFlag;//控制命令
                public static bool[] OtherTypeFlag;//其它操作
                public static bool RoleMangerFlag;//用户管理
                public static bool rightCfgState;

            }

            public struct _SaveRoleInfo
            {
                public static byte num;               //保存的用户名信息个数
                public static _ROLEMANEGER[] RoleInfo;
            }
            public struct _FrameTime
            {
                public static byte T0;
                public static byte T1;
                public static byte T1_send;
                public static byte T2;
                public static byte T2_CloseTCP;
                public static byte T3;
                public static int LoopTime;
                public static int K = 0;
                public static int W = 0;
                public static int T_NetSend = 0;
            }
            public struct _RaoDong
            {
                public static int num;
                public static string[] NameTable;
                public static string[] ValueTable;
                public static string[] ByteTable;
            }
            public struct _YkLogicInfo
            {
                public static int num;
                public static string[] NameTable;
                public static string[] ValueTable;
                public static string[] ByteTable;
            }
            public struct _HisData
            {
                public static int ycnum;
                public static int tjnum;
                public static string[] YcNameTable;
                public static string[] TjNameTable;
            }

            public struct _Mystruct
            {
                public static int bpos;
                public static int epos;
                public static string value;
                public static bool bl;

                public static int row;

                public static int linenum;
                public static int paramnum;
                public static string[] paramname;

            }

            public struct _MyYcDotstruct
            {
                public static int bpos;
                public static int epos;
                public static string BusNum;
                public static string CardNum;
                public static string UBusmode;
                public static string IBusmode;
                public static bool BusNum_notchange;
                public static bool CardNum_notchange;
                public static bool UBusmode_notchange;
                public static bool IBusmode_notchange;


                //public static bool bl;
            }

            public struct _MyYcInformationstruct
            {
                public static int bpos;
                public static int epos;
                public static int index;
                public static string datatype;
                public static string magnification;
                public static bool index_notchange;
                public static bool datatype_notchange;
                public static bool magnification_notchange;

                public static bool bl;
            }


            public struct _Collector
            {
                public static int num;
                public static string[] NameTable;
                public static string[] ValueTable;
                public static string[] ByteTable;
            }
            public struct _DeviceInfo
            {
                public string NameTable;
                public string ValueTable;
                public string ByteTable;
            }
            public struct _Indicatorstruct
            {
                public static byte INDICATOR_Num;
                public static _DeviceInfo[] protocoltypeInfo;
                public static _DeviceInfo[] baudInfo;
                public static _DeviceInfo[] AddrInfo;
            }
            public struct _Huijiqistruct
            {
                public static byte Huijiqi_Num;
                public static _DeviceInfo[] protocoltypeInfo;
                public static _DeviceInfo[] baudInfo;
                public static _DeviceInfo[] AddrInfo;
            }
            public struct _Sensorstruct
            {
                public static byte Sensor_Num;
                public static _DeviceInfo[] protocoltypeInfo;
                public static _DeviceInfo[] baudInfo;
                public static _DeviceInfo[] AddrInfo;
            }
            public struct _Q2struct
            {
                public static byte Q2_Num;
                public static _DeviceInfo[] protocoltypeInfo;
                public static _DeviceInfo[] baudInfo;
                public static _DeviceInfo[] AddrInfo;
            }

            public struct _MnitorParam
            {
                public static string MnitorIP;
                public static string MnitorPort;
                public static string MnitorComPort;
                public static string MnitorCombaud;
                public static string MnitorComjy;
                public static byte[] NetTBuffer = new byte[1024];
                public static byte[] NetRBuffer = new byte[1024];
                public static int NetTLen;
                public static int NetRLen;
                public static bool TX_TASK;
                public static bool TcpLinkState;
                public static int TcpLinkType;
                public static bool NetShowRecvMonitorData=false;
                public static bool NetShowSendMonitorData=false;
                public static bool start;
                public static bool stop;
                public static bool test;
                public static bool monitotclose=true;
            }
            public struct JZPARAM
            {

                public static string vaule1;
                public static string vaule2;
                public static string vaule3;
                public static string vaule4;
                public static string vaule5;
                public static string vaule6;
                public static string vaule7;
                public static string vaule8;
                public static string vaule9;
                public static string vaule10;
                public static string vaule11;
                public static string vaule12;

                public static string vaule13;
                public static string vaule14;
                public static string vaule15;
                public static string vaule16;
                public static string vaule17;
                public static string vaule18;
                public static string vaule19;
                public static string vaule20;
            }
            public struct _Curve
            {
                //public static PointPairList listemp = new PointPairList();
                public static double[] ycdata;
                public static PointPairList[] listemp;
                public static bool[] showcurve;
                public static int savenum;
                public static int[] SavePosTable;

            }

            #endregion

        }
    }
}
