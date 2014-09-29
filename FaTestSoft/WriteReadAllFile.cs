using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using FaTestSoft.CommonData;              //使用新增加的类所在的命名空间


namespace FaTestSoft
{
    class WriteReadAllFile
    {
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
        public static string str;

        /*************************************************************************
         *  函数名：    WriteReadParamIniFile                                      *
         *  功能  ：    系统可写可读ini文件                                      *
         *  参数  ：    fname ：路径名                                           *
         *              Type  ：0--读,1--写                                      *
         *              k     ：那一种文件                                       *
         *  返回值：    无                                                       *
         *  修改日期：  2012-09-03                                               *
         *  作者    ：  ZXL                                                   *
         * **********************************************************************/
        public static void WriteReadParamIniFile(string fname, byte Type, byte k)
        {

       //     IniFiles Ini = new IniFiles(fname);//ini文件的绝对路径
            //bool running = Ini.ReadValue("RunTime Control", "Running", true);
            //Ini.WriteValue("RunTime Control", "Running", "mystring");
            if (Type == 0)          //读
            {
                if (k == 0)              //读串口参数
                {
                    GetPrivateProfileString("NUM", "PARAMNUM", "无法读取对应数值！",
                                                         temp, 255, fname);
                    PublicDataClass._ComParam.num = int.Parse(temp.ToString());                    //转换为整型
                    PublicDataClass._ComParam.NameTable = new string[PublicDataClass._ComParam.num];
                    PublicDataClass._ComParam.ValueTable = new string[PublicDataClass._ComParam.num ];
                    PublicDataClass._ComParam.ByteTable = new string[PublicDataClass._ComParam.num ];
                    for (int j = 0; j < PublicDataClass._ComParam.num; j++)
                    {
                        str = String.Format("name_{0:d}", j);

                        GetPrivateProfileString("NAMETABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._ComParam.NameTable[j] = temp.ToString();
                    }
                    for (int j = 0; j < PublicDataClass._ComParam.num; j++)
                    {
                        str = String.Format("value_{0:d}", j);

                        GetPrivateProfileString("VALUETABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._ComParam.ValueTable[j] = temp.ToString();
                    }
                    for (int j = 0; j < PublicDataClass._ComParam.num; j++)
                    {
                        str = String.Format("beilv_{0:d}", j);

                        GetPrivateProfileString("BEILVTABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._ComParam.ByteTable[j] = temp.ToString();
                    }

                }
                else if (k == 1)              //读网络参数
                {
                    GetPrivateProfileString("NUM", "PARAMNUM", "无法读取对应数值！",
                                                         temp, 255, fname);
                    PublicDataClass._NetParam.num = int.Parse(temp.ToString());                    //转换为整型
                    PublicDataClass._NetParam.NameTable = new string[PublicDataClass._NetParam.num];
                    PublicDataClass._NetParam.ValueTable = new string[PublicDataClass._NetParam.num];
                    PublicDataClass._NetParam.ByteTable = new string[PublicDataClass._NetParam.num];
                    for (int j = 0; j < PublicDataClass._NetParam.num; j++)
                    {
                        str = String.Format("name_{0:d}", j);

                        GetPrivateProfileString("NAMETABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._NetParam.NameTable[j] = temp.ToString();
                    }
                    for (int j = 0; j < PublicDataClass._NetParam.num; j++)
                    {
                        str = String.Format("value_{0:d}", j);

                        GetPrivateProfileString("VALUETABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._NetParam.ValueTable[j] = temp.ToString();
                    }
                    for (int j = 0; j < PublicDataClass._NetParam.num; j++)
                    {
                        str = String.Format("beilv_{0:d}", j);

                        GetPrivateProfileString("BEILVTABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._NetParam.ByteTable[j] = temp.ToString();
                    }
                }
                else if (k == 2)              //读系统参数
                {
                    GetPrivateProfileString("NUM", "SYSPARAMNUM", "无法读取对应数值！",
                                                         temp, 255, fname);
                    PublicDataClass._SysParam.num = int.Parse(temp.ToString());                    //转换为整型
                    PublicDataClass._SysParam.NameTable = new string[PublicDataClass._SysParam.num];
                    PublicDataClass._SysParam.ValueTable = new string[PublicDataClass._SysParam.num];
                    PublicDataClass._SysParam.ByteTable = new string[PublicDataClass._SysParam.num];

                    for (int j = 0; j < PublicDataClass._SysParam.num; j++)
                    {
                        str = String.Format("name_{0:d}", j);

                        GetPrivateProfileString("NAMETABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._SysParam.NameTable[j] = temp.ToString();
                    }
                    for (int j = 0; j < PublicDataClass._SysParam.num; j++)
                    {
                        str = String.Format("value_{0:d}", j);

                        GetPrivateProfileString("VALUETABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._SysParam.ValueTable[j] = temp.ToString();
                    }
                    for (int j = 0; j < PublicDataClass._SysParam.num; j++)
                    {
                        str = String.Format("beilv_{0:d}", j);

                        GetPrivateProfileString("BEILVTABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._SysParam.ByteTable[j] = temp.ToString();
                    }

                }



                else if (k == 3)              //读遥测配置参数
                {
                    GetPrivateProfileString("NUM", "PARAMNUM", "无法读取对应数值！",
                                                         temp, 255, fname);
                    PublicDataClass._YcConfigParam.num = int.Parse(temp.ToString());
                    PublicDataClass._YcConfigParam.NameTable = new string[PublicDataClass._YcConfigParam.num];
                    PublicDataClass._YcConfigParam.AddrTable = new string[PublicDataClass._YcConfigParam.num];
                    PublicDataClass._YcConfigParam.IndexTable = new string[PublicDataClass._YcConfigParam.num];
                 //   PublicDataClass._YcConfigParam.DatatypeTable = new string[PublicDataClass._YcConfigParam.num];
                 //   PublicDataClass._YcConfigParam.MagnificationTable = new string[PublicDataClass._YcConfigParam.num];
                  //  PublicDataClass._YcConfigParam.ConnectTable = new string[PublicDataClass._YcConfigParam.num];
                 //   PublicDataClass._YcConfigParam.QufanTable = new string[PublicDataClass._YcConfigParam.num];
                    //PublicDataClass._YcConfigParam.setvalueTable = new string[PublicDataClass._YcConfigParam.num];
                    //PublicDataClass._YcConfigParam.ValueTable = new string[PublicDataClass._YcConfigParam.num];
                  //  PublicDataClass._YcConfigParam.ByteTable = new string[PublicDataClass._YcConfigParam.num];

                    GetPrivateProfileString("ZBSTARTYCNUM", "ZBSTARTYCNUM", "0",
                                                       temp, 255, fname);
                    PublicDataClass._YcConfigParam.ZBStartYcNum= int.Parse(temp.ToString());
                    GetPrivateProfileString("ZBYCNUM", "ZBYCNUM", "0",
                                                    temp, 255, fname);
                    PublicDataClass._YcConfigParam.ZBYcNum = int.Parse(temp.ToString());
                    for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                    {
                        str = String.Format("name_{0:d}", j);

                        GetPrivateProfileString("NAMETABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._YcConfigParam.NameTable[j] = temp.ToString();

                    }
                    for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                    {
                        str = String.Format("infoaddr_{0:d}", j);

                        GetPrivateProfileString("INFOTABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._YcConfigParam.AddrTable[j] = temp.ToString();

                    }
                    for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                    {
                        str = String.Format("index_{0:d}", j);

                        GetPrivateProfileString("INDEXTABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._YcConfigParam.IndexTable[j] = temp.ToString();

                    }
                    //for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                    //{
                    //    str = String.Format("datatyple_{0:d}", j);

                    //    GetPrivateProfileString("DATATYPLETABLE", str, "无法读取对应数值！",
                    //                                 temp, 255, fname);
                    //    PublicDataClass._YcConfigParam.DatatypeTable[j] = temp.ToString();

                    //}
                    //for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                    //{
                    //    str = String.Format("beishu_{0:d}", j);

                    //    GetPrivateProfileString("BEISHUTABLE", str, "无法读取对应数值！",
                    //                                 temp, 255, fname);
                    //    PublicDataClass._YcConfigParam.MagnificationTable[j] = temp.ToString();

                    //}
                    //for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                    //{
                    //    str = String.Format("connect_{0:d}", j);

                    //    GetPrivateProfileString("CONNECTTABLE", str, "无法读取对应数值！",
                    //                                 temp, 255, fname);
                    //    PublicDataClass._YcConfigParam.ConnectTable[j] = temp.ToString();

                    //}
                    //for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                    //{
                    //    str = String.Format("qufan_{0:d}", j);

                    //    GetPrivateProfileString("QUFANTABLE", str, "无法读取对应数值！",
                    //                                 temp, 255, fname);
                    //    PublicDataClass._YcConfigParam.QufanTable[j] = temp.ToString();

                    //}
                    //for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                    //{
                    //    str = String.Format("setflag_{0:d}", j);

                    //    GetPrivateProfileString("SETVALUETABLE", str, "无法读取对应数值！",
                    //                                 temp, 255, fname);
                    //    PublicDataClass._YcConfigParam.setvalueTable[j] = temp.ToString();

                    //}
                    //for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                    //{
                    //    str = String.Format("value_{0:d}", j);

                    //    GetPrivateProfileString("VALUETABLE", str, "无法读取对应数值！",
                    //                                 temp, 255, fname);
                    //    PublicDataClass._YcConfigParam.ValueTable[j] = temp.ToString();

                    //}
                    //for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                    //{
                    //    str = String.Format("byte_{0:d}", j);

                    //    GetPrivateProfileString("BYTETABLE", str, "无法读取对应数值！",
                    //                                 temp, 255, fname);
                    //    PublicDataClass._YcConfigParam.ByteTable[j] = temp.ToString();

                    //}

                }
                else if (k == 4)              //读遥信配置参数
                {
                    GetPrivateProfileString("NUM", "PARAMNUM", "无法读取对应数值！",
                                                         temp, 255, fname);
                    PublicDataClass._YxConfigParam.num = int.Parse(temp.ToString());
                    PublicDataClass._YxConfigParam.NameTable = new string[PublicDataClass._YxConfigParam.num];
                    PublicDataClass._YxConfigParam.AddrTable = new string[PublicDataClass._YxConfigParam.num];
                    PublicDataClass._YxConfigParam.IndexTable = new string[PublicDataClass._YxConfigParam.num];
                    PublicDataClass._YxConfigParam.QufanTable = new string[PublicDataClass._YxConfigParam.num];
                    //PublicDataClass._YxConfigParam.setvalueTable = new string[PublicDataClass._YxConfigParam.num];
                    //PublicDataClass._YxConfigParam.ValueTable = new string[PublicDataClass._YxConfigParam.num];
                    PublicDataClass._YxConfigParam.ByteTable = new string[PublicDataClass._YxConfigParam.num];

                    GetPrivateProfileString("WYXNUM", "YXNUM", PublicDataClass._YxConfigParam.num.ToString(),
                                                         temp, 255, fname);
                    PublicDataClass._YxConfigParam.wyxnum = int.Parse(temp.ToString());
                    GetPrivateProfileString("ZBYXNUM", "ZBYXNUM", "0",
                                                    temp, 255, fname);
                    PublicDataClass._YxConfigParam.ZBYxNum = int.Parse(temp.ToString());

                    for (int j = 0; j < PublicDataClass._YxConfigParam.num; j++)
                    {
                        str = String.Format("name_{0:d}", j);

                        GetPrivateProfileString("NAMETABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._YxConfigParam.NameTable[j] = temp.ToString();

                    }
                    for (int j = 0; j < PublicDataClass._YxConfigParam.num; j++)
                    {
                        str = String.Format("infoaddr_{0:d}", j);

                        GetPrivateProfileString("INFOTABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._YxConfigParam.AddrTable[j] = temp.ToString();

                    }
                    for (int j = 0; j < PublicDataClass._YxConfigParam.num; j++)
                    {
                        str = String.Format("index_{0:d}", j);

                        GetPrivateProfileString("INDEXTABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._YxConfigParam.IndexTable[j] = temp.ToString();

                    }
                    
                    for (int j = 0; j < PublicDataClass._YxConfigParam.num; j++)
                    {
                        str = String.Format("qufan_{0:d}", j);

                        GetPrivateProfileString("QUFANTABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._YxConfigParam.QufanTable[j] = temp.ToString();

                    }
                    //for (int j = 0; j < PublicDataClass._YxConfigParam.num; j++)
                    //{
                    //    str = String.Format("setflag_{0:d}", j);

                    //    GetPrivateProfileString("SETVALUETABLE", str, "无法读取对应数值！",
                    //                                 temp, 255, fname);
                    //    PublicDataClass._YxConfigParam.setvalueTable[j] = temp.ToString();

                    //}
                    //for (int j = 0; j < PublicDataClass._YxConfigParam.num; j++)
                    //{
                    //    str = String.Format("value_{0:d}", j);

                    //    GetPrivateProfileString("VALUETABLE", str, "无法读取对应数值！",
                    //                                 temp, 255, fname);
                    //    PublicDataClass._YxConfigParam.ValueTable[j] = temp.ToString();

                    //}
                    for (int j = 0; j < PublicDataClass._YxConfigParam.num; j++)
                    {
                        str = String.Format("byte_{0:d}", j);

                        GetPrivateProfileString("BYTETABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._YxConfigParam.ByteTable[j] = temp.ToString();

                    }

                }
                else if (k == 5)      //读遥控配置参数
                {
                    GetPrivateProfileString("NUM", "PARAMNUM", "无法读取对应数值！",
                                                         temp, 255, fname);
                    PublicDataClass._YkConfigParam.num = int.Parse(temp.ToString());
                    PublicDataClass._YkConfigParam.NameTable = new string[PublicDataClass._YkConfigParam.num];
                    PublicDataClass._YkConfigParam.AddrTable = new string[PublicDataClass._YkConfigParam.num];
                    PublicDataClass._YkConfigParam.triggermodeTable= new string[PublicDataClass._YkConfigParam.num];
                    PublicDataClass._YkConfigParam.secltimeTable = new string[PublicDataClass._YkConfigParam.num];
                    PublicDataClass._YkConfigParam.exetimeTable = new string[PublicDataClass._YkConfigParam.num];
                    PublicDataClass._YkConfigParam.pulsewidthTable= new string[PublicDataClass._YkConfigParam.num];
                    PublicDataClass._YkConfigParam.saveflagTable = new string[PublicDataClass._YkConfigParam.num];

                    PublicDataClass._YkConfigParam.powerTable= new string[PublicDataClass._YkConfigParam.num];
                    PublicDataClass._YkConfigParam.jdq1Table = new string[PublicDataClass._YkConfigParam.num];
                    PublicDataClass._YkConfigParam.jdq2Table = new string[PublicDataClass._YkConfigParam.num];
                    PublicDataClass._YkConfigParam.fjyx1Table = new string[PublicDataClass._YkConfigParam.num];
                    PublicDataClass._YkConfigParam.fjyx2Table = new string[PublicDataClass._YkConfigParam.num];

                    PublicDataClass._YkConfigParam.ByteTable = new string[PublicDataClass._YkConfigParam.num];
                    for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        str = String.Format("name_{0:d}", j);

                        GetPrivateProfileString("NAMETABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._YkConfigParam.NameTable[j] = temp.ToString();

                    }
                    for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        str = String.Format("infoaddr_{0:d}", j);

                        GetPrivateProfileString("INFOTABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._YkConfigParam.AddrTable[j] = temp.ToString();

                    }
                    for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        str = String.Format("triggermode_{0:d}", j);

                        GetPrivateProfileString("TRIGGERMODE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._YkConfigParam.triggermodeTable[j] = temp.ToString();

                    }
                    for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        str = String.Format("secltime_{0:d}", j);

                        GetPrivateProfileString("SECLTIME", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._YkConfigParam.secltimeTable[j] = temp.ToString();

                    } for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        str = String.Format("exetime_{0:d}", j);

                        GetPrivateProfileString("EXETIME", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._YkConfigParam.exetimeTable[j] = temp.ToString();

                    }
                    for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        str = String.Format("pulsewidth_{0:d}", j);

                        GetPrivateProfileString("PULSEWIDTH", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._YkConfigParam.pulsewidthTable[j] = temp.ToString();

                    }
                    for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        str = String.Format("saveflag_{0:d}", j);

                        GetPrivateProfileString("SAVEFLAG", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._YkConfigParam.saveflagTable[j] = temp.ToString();

                    }


                    for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        str = String.Format("power_{0:d}", j);

                        GetPrivateProfileString("POWER", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._YkConfigParam.powerTable[j] = temp.ToString();

                    }
                    for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        str = String.Format("jdq1_{0:d}", j);

                        GetPrivateProfileString("JDQ1", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._YkConfigParam.jdq1Table[j] = temp.ToString();

                    } for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        str = String.Format("jdq2_{0:d}", j);

                        GetPrivateProfileString("JDQ2", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._YkConfigParam.jdq2Table[j] = temp.ToString();

                    }
                    for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        str = String.Format("fjyx1_{0:d}", j);

                        GetPrivateProfileString("FJYX1", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._YkConfigParam.fjyx1Table[j] = temp.ToString();

                    }
                    for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        str = String.Format("fjyx2_{0:d}", j);

                        GetPrivateProfileString("FJYX2", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._YkConfigParam.fjyx2Table[j] = temp.ToString();

                    }

                    for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        str = String.Format("byte_{0:d}", j);

                        GetPrivateProfileString("BYTETABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._YkConfigParam.ByteTable[j] = temp.ToString();

                    }

                }
                else if (k == 6)     //读历史数据名称表
                {
                    GetPrivateProfileString("NUM", "YCHISNUM", "无法读取对应数值！",
                                                        temp, 255, fname);
                    PublicDataClass._HisData.ycnum = int.Parse(temp.ToString());            //转换为整型
                    PublicDataClass._HisData.YcNameTable = new string[PublicDataClass._HisData.ycnum];
                    for (int j = 0; j < PublicDataClass._HisData.ycnum; j++)
                    {
                        str = String.Format("ychisname_{0:d}", j);

                        GetPrivateProfileString("YCHISNAME", str, "无法读取对应数值！",
                                                     temp, 255, fname);

                        PublicDataClass._HisData.YcNameTable[j] = temp.ToString();
                    }
                }
                else if (k == 7)     //读统计数据名称表
                {
                    GetPrivateProfileString("NUM", "TJHISNUM", "无法读取对应数值！",
                                                        temp, 255, fname);
                    PublicDataClass._HisData.tjnum = int.Parse(temp.ToString());            //转换为整型
                    PublicDataClass._HisData.TjNameTable = new string[PublicDataClass._HisData.tjnum];
                    for (int j = 0; j < PublicDataClass._HisData.tjnum; j++)
                    {
                        str = String.Format("tjhisname_{0:d}", j);

                        GetPrivateProfileString("TJHISNAME", str, "无法读取对应数值！",
                                                     temp, 255, fname);

                        PublicDataClass._HisData.TjNameTable[j] = temp.ToString();
                    }
                }
                else if (k == 8)          //读协议ini
                {

                    GetPrivateProfileString("NUM", "PROTOCOLNUM", "无法读取对应数值！",
                                                        temp, 255, fname);
                    PublicDataClass.ProtocolIniInfo.num = int.Parse(temp.ToString());             //转换为整型
                    PublicDataClass.ProtocolIniInfo.IniTable = new string[PublicDataClass.ProtocolIniInfo.num];
                    for (int j = 0; j < PublicDataClass.ProtocolIniInfo.num; j++)
                    {
                        str = String.Format("protocol_{0:d}", j);

                        GetPrivateProfileString("TABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);

                        PublicDataClass.ProtocolIniInfo.IniTable[j] = temp.ToString();
                    }

                }
                else if (k == 9)          //
                {

                    GetPrivateProfileString("NUM", "PARAMNUM", "无法读取对应数值！",
                                                        temp, 255, fname);
                    PublicDataClass._Curve.savenum = int.Parse(temp.ToString());             //转换为整型
                    PublicDataClass._Curve.SavePosTable = new int[PublicDataClass._Curve.savenum];
                    for (int j = 0; j < PublicDataClass._Curve.savenum; j++)
                    {
                        str = String.Format("pos_{0:d}", j);

                        GetPrivateProfileString("POSTABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._Curve.SavePosTable[j] = Convert.ToInt32( temp.ToString());
                    }

                }
                else if (k == 10)              //模拟量参数
                {
                    GetPrivateProfileString("NUM", "PARAMNUM", "无法读取对应数值！",
                                                         temp, 255, fname);
                    PublicDataClass._AIParam.num = int.Parse(temp.ToString());                    //转换为整型
                    PublicDataClass._AIParam.quality = new string[PublicDataClass._AIParam.num];
                    PublicDataClass._AIParam.phase = new string[PublicDataClass._AIParam.num];
                    PublicDataClass._AIParam.line = new string[PublicDataClass._AIParam.num];
                    PublicDataClass._AIParam.panel = new string[PublicDataClass._AIParam.num];
                    PublicDataClass._AIParam.value = new string[PublicDataClass._AIParam.num];
                    PublicDataClass._AIParam.ph = new string[PublicDataClass._AIParam.num];
                    PublicDataClass._AIParam.zhishu = new string[PublicDataClass._AIParam.num];

                    for (int j = 0; j < PublicDataClass._AIParam.num; j++)
                    {
                        str = String.Format("quality_{0:d}", j);

                        GetPrivateProfileString("QUALITYTABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._AIParam.quality[j] = temp.ToString();
                    }
                    for (int j = 0; j < PublicDataClass._AIParam.num; j++)
                    {
                        str = String.Format("phase_{0:d}", j);

                        GetPrivateProfileString("PHASETABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._AIParam.phase[j] = temp.ToString();
                    }
                    for (int j = 0; j < PublicDataClass._AIParam.num; j++)
                    {
                        str = String.Format("line_{0:d}", j);

                        GetPrivateProfileString("LINETABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._AIParam.line[j] = temp.ToString();
                    }
                    for (int j = 0; j < PublicDataClass._AIParam.num; j++)
                    {
                        str = String.Format("panel_{0:d}", j);

                        GetPrivateProfileString("PANELTABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._AIParam.panel[j] = temp.ToString();
                    }
                    for (int j = 0; j < PublicDataClass._AIParam.num; j++)
                    {
                        str = String.Format("value_{0:d}", j);

                        GetPrivateProfileString("VALUETABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._AIParam.value[j] = temp.ToString();
                    }
                    for (int j = 0; j < PublicDataClass._AIParam.num; j++)
                    {
                        str = String.Format("ph_{0:d}", j);

                        GetPrivateProfileString("PHTABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._AIParam.ph[j] = temp.ToString();
                    }
                    for (int j = 0; j < PublicDataClass._AIParam.num; j++)
                    {
                        str = String.Format("zhishu_{0:d}", j);

                        GetPrivateProfileString("ZHISHUTABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);
                        PublicDataClass._AIParam.zhishu[j] = temp.ToString();
                    }
                }

                else if (k == 11)          //读继保定值
                {

                    GetPrivateProfileString("NUM", "PARAMNUM", "无法读取对应数值！",
                                                        temp, 255, fname);
                    PublicDataClass._RelayProtectParam .num = int.Parse(temp.ToString());             //转换为整型
                    PublicDataClass._RelayProtectParam.NameTable = new string[PublicDataClass._RelayProtectParam.num];
                    PublicDataClass._RelayProtectParam.ValueTable = new string[PublicDataClass._RelayProtectParam.num];
                    PublicDataClass._RelayProtectParam.AddrTable = new string[PublicDataClass._RelayProtectParam.num];
                    PublicDataClass._RelayProtectParam.LineTable = new string[PublicDataClass._RelayProtectParam.num];
                    for (int j = 0; j < PublicDataClass._RelayProtectParam.num; j++)
                    {
                        str = String.Format("name_{0:d}", j);

                        GetPrivateProfileString("NAMETABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);

                        PublicDataClass._RelayProtectParam.NameTable[j] = temp.ToString();
                    }
                    for (int j = 0; j < PublicDataClass._RelayProtectParam.num; j++)
                    {
                        str = String.Format("value_{0:d}", j);

                        GetPrivateProfileString("VALUETABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);

                        PublicDataClass._RelayProtectParam.ValueTable[j] = temp.ToString();
                    }
                    for (int j = 0; j < PublicDataClass._RelayProtectParam.num; j++)
                    {
                        str = String.Format("add_{0:d}", j);

                        GetPrivateProfileString("ADDTABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);

                        PublicDataClass._RelayProtectParam.AddrTable[j] = temp.ToString();
                    }
                    for (int j = 0; j < PublicDataClass._RelayProtectParam.num; j++)
                    {
                        str = String.Format("line_{0:d}", j);

                        GetPrivateProfileString("LINETABLE", str, "无法读取对应数值！",
                                                     temp, 255, fname);

                        PublicDataClass._RelayProtectParam.LineTable[j] = temp.ToString();
                    }

                }


            }
            else                    //写
            {
                if (k == 0)   //串口参数
                {
                    str = Convert.ToString(PublicDataClass._ComParam.num);

                    WritePrivateProfileString("NUM", "PARAMNUM", str, fname);


                    for (int j = 0; j < PublicDataClass._ComParam.num; j++)
                    {
                        WritePrivateProfileString("NAMETABLE", null, null, fname);     //先清除掉
                        WritePrivateProfileString("VALUETABLE", null, null, fname);
                        WritePrivateProfileString("BEILVTABLE", null, null, fname);
                    }
                    for (int j = 0; j < PublicDataClass._ComParam.num; j++)
                    {
                        str = String.Format("name_{0:d}", j);

                        WritePrivateProfileString("NAMETABLE", str, Convert.ToString(PublicDataClass._ComParam.NameTable[j]), fname);


                    }
                    for (int j = 0; j < PublicDataClass._ComParam.num; j++)
                    {
                        str = String.Format("value_{0:d}", j);

                        WritePrivateProfileString("VALUETABLE", str, Convert.ToString(PublicDataClass._ComParam.ValueTable[j]), fname);

                    }
                    for (int j = 0; j < PublicDataClass._ComParam.num; j++)
                    {
                        str = String.Format("beilv_{0:d}", j);
                        WritePrivateProfileString("BEILVTABLE", str, Convert.ToString(PublicDataClass._ComParam.ByteTable[j]), fname);
                    }
                    
                }
                else if (k == 1)   //网口参数
                {
                    str = Convert.ToString(PublicDataClass._NetParam.num);

                    WritePrivateProfileString("NUM", "PARAMNUM", str, fname);


                    for (int j = 0; j < PublicDataClass._NetParam.num; j++)
                    {
                        WritePrivateProfileString("NAMETABLE", null, null, fname);     //先清除掉
                        WritePrivateProfileString("VALUETABLE", null, null, fname);
                        WritePrivateProfileString("BEILVTABLE", null, null, fname);
                    }
                    for (int j = 0; j < PublicDataClass._NetParam.num; j++)
                    {
                        str = String.Format("name_{0:d}", j);

                        WritePrivateProfileString("NAMETABLE", str, Convert.ToString(PublicDataClass._NetParam.NameTable[j]), fname);


                    }
                    for (int j = 0; j < PublicDataClass._NetParam.num; j++)
                    {
                        str = String.Format("value_{0:d}", j);

                        WritePrivateProfileString("VALUETABLE", str, Convert.ToString(PublicDataClass._NetParam.ValueTable[j]), fname);

                    }
                    for (int j = 0; j < PublicDataClass._NetParam.num; j++)
                    {
                        str = String.Format("beilv_{0:d}", j);
                        WritePrivateProfileString("BEILVTABLE", str, Convert.ToString(PublicDataClass._NetParam.ByteTable[j]), fname);
                    }
                   

                }
                else if (k == 2)   //系统参数
                {
                    str = Convert.ToString(PublicDataClass._SysParam.num);

                    WritePrivateProfileString("NUM", "SYSPARAMNUM", str, fname);


                    for (int j = 0; j < PublicDataClass._SysParam.num ; j++)
                    {
                        WritePrivateProfileString("NAMETABLE", null, null, fname);     //先清除掉
                        WritePrivateProfileString("VALUETABLE", null, null, fname);
                        WritePrivateProfileString("BEILVTABLE", null, null, fname);
                    }
                    for (int j = 0; j < PublicDataClass._SysParam.num; j++)
                    {
                        str = String.Format("name_{0:d}", j);

                        WritePrivateProfileString("NAMETABLE", str, Convert.ToString(PublicDataClass._SysParam.NameTable[j]), fname);


                    }
                    for (int j = 0; j < PublicDataClass._SysParam.num; j++)
                    {
                        str = String.Format("value_{0:d}", j);

                        WritePrivateProfileString("VALUETABLE", str, Convert.ToString(PublicDataClass._SysParam.ValueTable[j]), fname);

                    }
                    for (int j = 0; j < PublicDataClass._SysParam.num ; j++)
                    {
                        str = String.Format("beilv_{0:d}", j);
                        WritePrivateProfileString("BEILVTABLE", str, Convert.ToString(PublicDataClass._SysParam.ByteTable[j]), fname);
                    }
                }
                else if (k == 3)   //遥测配置
                {
                    str = Convert.ToString(PublicDataClass._YcConfigParam.num);
          
                    WritePrivateProfileString("NUM", "PARAMNUM", str, fname);


                    for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                    {
                        WritePrivateProfileString("NAMETABLE", null, null, fname);     //先清除掉
                        WritePrivateProfileString("INFOTABLE", null, null, fname);
                        WritePrivateProfileString("INDEXTABLE", null, null, fname);
                        //    WritePrivateProfileString("DATATYPLETABLE", null, null, fname);     //先清除掉
                        //   WritePrivateProfileString("BEISHUTABLE", null, null, fname);
                        //  WritePrivateProfileString("CONNECTTABLE", null, null, fname);
                        //  WritePrivateProfileString("QUFANTABLE", null, null, fname);
                        //WritePrivateProfileString("SETVALUETABLE", null, null, fname);
                        //WritePrivateProfileString("VALUETABLE", null, null, fname);
                        //    WritePrivateProfileString("BYTETABLE", null, null, fname);
                    }

                    for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                    {
                        str = String.Format("name_{0:d}", j);

                        WritePrivateProfileString("NAMETABLE", str, Convert.ToString(PublicDataClass._YcConfigParam.NameTable[j]), fname);
                        //      Ini.WriteValue("NAMETABLE", str, Convert.ToString(PublicDataClass._YcConfigParam.NameTable[j]));

                    }
                    for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                    {
                        str = String.Format("infoaddr_{0:d}", j);

                        WritePrivateProfileString("INFOTABLE", str, Convert.ToString(PublicDataClass._YcConfigParam.AddrTable[j]), fname);

                    }
                    for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                    {
                        str = String.Format("index_{0:d}", j);
                        WritePrivateProfileString("INDEXTABLE", str, Convert.ToString(PublicDataClass._YcConfigParam.IndexTable[j]), fname);
                    }
                    //for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                    //{
                    //    str = String.Format("datatyple_{0:d}", j);

                    //    WritePrivateProfileString("DATATYPLETABLE", str, Convert.ToString(PublicDataClass._YcConfigParam.DatatypeTable[j]), fname);


                    //}
                    //for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                    //{
                    //    str = String.Format("beishu_{0:d}", j);

                    //    WritePrivateProfileString("BEISHUTABLE", str, Convert.ToString(PublicDataClass._YcConfigParam.MagnificationTable[j]), fname);

                    //}
                    //for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                    //{
                    //    str = String.Format("connect_{0:d}", j);
                    //    WritePrivateProfileString("CONNECTTABLE", str, Convert.ToString(PublicDataClass._YcConfigParam.ConnectTable[j]), fname);
                    //}
                    //for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                    //{
                    //    str = String.Format("qufan_{0:d}", j);

                    //    WritePrivateProfileString("QUFANTABLE", str, Convert.ToString(PublicDataClass._YcConfigParam.QufanTable[j]), fname);

                    //}
                    //for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                    //{
                    //    str = String.Format("setflag_{0:d}", j);
                    //    WritePrivateProfileString("SETVALUETABLE", str, Convert.ToString(PublicDataClass._YcConfigParam.setvalueTable[j]), fname);
                    //}
                    //for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                    //{
                    //    str = String.Format("value_{0:d}", j);

                    //    WritePrivateProfileString("VALUETABLE", str, Convert.ToString(PublicDataClass._YcConfigParam.ValueTable[j]), fname);

                    //}
                    //for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                    //{
                    //    str = String.Format("byte_{0:d}", j);
                    //    WritePrivateProfileString("BYTETABLE", str, Convert.ToString(PublicDataClass._YcConfigParam.ByteTable[j]), fname);
                    //}
                }
                else if (k == 4)   //遥信配置
                {
                    str = Convert.ToString(PublicDataClass._YxConfigParam.num);
                    WritePrivateProfileString("NUM", "PARAMNUM", str, fname);

                    str = Convert.ToString(PublicDataClass._YxConfigParam.wyxnum);
                    WritePrivateProfileString("WYXNUM", "YXNUM", str, fname);

                    for (int j = 0; j < PublicDataClass._YxConfigParam.num; j++)
                    {
                        WritePrivateProfileString("NAMETABLE", null, null, fname);     //先清除掉
                        WritePrivateProfileString("INFOTABLE", null, null, fname);
                        WritePrivateProfileString("INDEXTABLE", null, null, fname);
                        WritePrivateProfileString("QUFANTABLE", null, null, fname);
                        WritePrivateProfileString("SETVALUETABLE", null, null, fname);
                        WritePrivateProfileString("VALUETABLE", null, null, fname);
                        WritePrivateProfileString("BYTETABLE", null, null, fname);
                    }
                    for (int j = 0; j < PublicDataClass._YxConfigParam.num; j++)
                    {
                        str = String.Format("name_{0:d}", j);

                        WritePrivateProfileString("NAMETABLE", str, Convert.ToString(PublicDataClass._YxConfigParam.NameTable[j]), fname);


                    }
                    for (int j = 0; j < PublicDataClass._YxConfigParam.num; j++)
                    {
                        str = String.Format("infoaddr_{0:d}", j);

                        WritePrivateProfileString("INFOTABLE", str, Convert.ToString(PublicDataClass._YxConfigParam.AddrTable[j]), fname);

                    }
                    for (int j = 0; j < PublicDataClass._YxConfigParam.num; j++)
                    {
                        str = String.Format("index_{0:d}", j);
                        WritePrivateProfileString("INDEXTABLE", str, Convert.ToString(PublicDataClass._YxConfigParam.IndexTable[j]), fname);
                    }

                    for (int j = 0; j < PublicDataClass._YxConfigParam.num; j++)
                    {
                        str = String.Format("qufan_{0:d}", j);

                        WritePrivateProfileString("QUFANTABLE", str, Convert.ToString(PublicDataClass._YxConfigParam.QufanTable[j]), fname);

                    }
                    //for (int j = 0; j < PublicDataClass._YxConfigParam.num; j++)
                    //{
                    //    str = String.Format("setflag_{0:d}", j);
                    //    WritePrivateProfileString("SETVALUETABLE", str, Convert.ToString(PublicDataClass._YxConfigParam.setvalueTable[j]), fname);
                    //}
                    //for (int j = 0; j < PublicDataClass._YxConfigParam.num; j++)
                    //{
                    //    str = String.Format("value_{0:d}", j);

                    //    WritePrivateProfileString("VALUETABLE", str, Convert.ToString(PublicDataClass._YxConfigParam.ValueTable[j]), fname);

                    //}
                    for (int j = 0; j < PublicDataClass._YxConfigParam.num; j++)
                    {
                        str = String.Format("byte_{0:d}", j);
                        WritePrivateProfileString("BYTETABLE", str, Convert.ToString(PublicDataClass._YxConfigParam.ByteTable[j]), fname);
                    }
                }
                else if (k == 5)   //遥控配置
                {
                    str = Convert.ToString(PublicDataClass._YkConfigParam.num);

                    WritePrivateProfileString("NUM", "PARAMNUM", str, fname);


                    for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        WritePrivateProfileString("NAMETABLE", null, null, fname);     //先清除掉
                        WritePrivateProfileString("INFOTABLE", null, null, fname);
                        WritePrivateProfileString("TRIGGERMODE", null, null, fname);

                        WritePrivateProfileString("SECLTIME", null, null, fname);     //先清除掉
                        WritePrivateProfileString("EXETIME", null, null, fname);
                        WritePrivateProfileString("PULSEWIDTH", null, null, fname);

                        WritePrivateProfileString("SAVEFLAG", null, null, fname);     //先清除掉
                        WritePrivateProfileString("POWER", null, null, fname);
                        WritePrivateProfileString("JDQ1", null, null, fname);

                        WritePrivateProfileString("JDQ2", null, null, fname);     //先清除掉
                        WritePrivateProfileString("FJYX1", null, null, fname);
                        WritePrivateProfileString("FJYX2", null, null, fname);

                        WritePrivateProfileString("BYTETABLE", null, null, fname);
                    }
                    for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        str = String.Format("name_{0:d}", j);

                        WritePrivateProfileString("NAMETABLE", str, Convert.ToString(PublicDataClass._YkConfigParam.NameTable[j]), fname);


                    }
                    for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        str = String.Format("infoaddr_{0:d}", j);

                        WritePrivateProfileString("INFOTABLE", str, Convert.ToString(PublicDataClass._YkConfigParam.AddrTable[j]), fname);

                    }
                    for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        str = String.Format("triggermode_{0:d}", j);
                        WritePrivateProfileString("TRIGGERMODE", str, Convert.ToString(PublicDataClass._YkConfigParam.triggermodeTable[j]), fname);
                    }


                    for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        str = String.Format("secltime_{0:d}", j);

                        WritePrivateProfileString("SECLTIME", str, Convert.ToString(PublicDataClass._YkConfigParam.secltimeTable[j]), fname);

                    }
                    for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        str = String.Format("exetime_{0:d}", j);
                        WritePrivateProfileString("EXETIME", str, Convert.ToString(PublicDataClass._YkConfigParam.exetimeTable[j]), fname);
                    }

                    for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        str = String.Format("pulsewidth_{0:d}", j);

                        WritePrivateProfileString("PULSEWIDTH", str, Convert.ToString(PublicDataClass._YkConfigParam.pulsewidthTable[j]), fname);

                    }
                    for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        str = String.Format("saveflag_{0:d}", j);
                        WritePrivateProfileString("SAVEFLAG", str, Convert.ToString(PublicDataClass._YkConfigParam.saveflagTable[j]), fname);
                    }



                    for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        str = String.Format("power_{0:d}", j);
                        WritePrivateProfileString("POWER", str, Convert.ToString(PublicDataClass._YkConfigParam.powerTable[j]), fname);
                    }


                    for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        str = String.Format("jdq1_{0:d}", j);

                        WritePrivateProfileString("JDQ1", str, Convert.ToString(PublicDataClass._YkConfigParam.jdq1Table[j]), fname);

                    }
                    for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        str = String.Format("jdq2_{0:d}", j);
                        WritePrivateProfileString("JDQ2", str, Convert.ToString(PublicDataClass._YkConfigParam.jdq2Table[j]), fname);
                    }

                    for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        str = String.Format("fjyx1_{0:d}", j);

                        WritePrivateProfileString("FJYX1", str, Convert.ToString(PublicDataClass._YkConfigParam.fjyx1Table[j]), fname);

                    }
                    for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        str = String.Format("fjyx2_{0:d}", j);
                        WritePrivateProfileString("FJYX2", str, Convert.ToString(PublicDataClass._YkConfigParam.fjyx2Table[j]), fname);
                    }

                    for (int j = 0; j < PublicDataClass._YkConfigParam.num; j++)
                    {
                        str = String.Format("byte_{0:d}", j);
                        WritePrivateProfileString("BYTETABLE", str, Convert.ToString(PublicDataClass._YkConfigParam.ByteTable[j]), fname);
                    }

                }
                else if (k == 10)   //模拟量参数
                {
                    str = Convert.ToString(PublicDataClass._AIParam.num);

                    WritePrivateProfileString("NUM", "PARAMNUM", str, fname);


                    for (int j = 0; j < PublicDataClass._AIParam.num; j++)
                    {
                        WritePrivateProfileString("QUALITYTABLE", null, null, fname);     //先清除掉
                        WritePrivateProfileString("PHASETABLE", null, null, fname);
                        WritePrivateProfileString("LINETABLE", null, null, fname);
                        WritePrivateProfileString("PANELTABLE", null, null, fname);     //先清除掉
                        WritePrivateProfileString("VALUETABLE", null, null, fname);
                        WritePrivateProfileString("PHTABLE", null, null, fname);
                        WritePrivateProfileString("ZHISHUTABLE", null, null, fname);
                    }
                    for (int j = 0; j < PublicDataClass._AIParam.num; j++)
                    {
                        str = String.Format("quality_{0:d}", j);

                        WritePrivateProfileString("QUALITYTABLE", str, Convert.ToString(PublicDataClass._AIParam.quality[j]), fname);
                    }
                    for (int j = 0; j < PublicDataClass._AIParam.num; j++)
                    {
                        str = String.Format("phase_{0:d}", j);

                        WritePrivateProfileString("PHASETABLE", str, Convert.ToString(PublicDataClass._AIParam.phase[j]), fname);
                    }
                    for (int j = 0; j < PublicDataClass._AIParam.num; j++)
                    {
                        str = String.Format("line_{0:d}", j);

                        WritePrivateProfileString("LINETABLE", str, Convert.ToString(PublicDataClass._AIParam.line[j]), fname);
                    }
                    for (int j = 0; j < PublicDataClass._AIParam.num; j++)
                    {
                        str = String.Format("panel_{0:d}", j);

                        WritePrivateProfileString("PANELTABLE", str, Convert.ToString(PublicDataClass._AIParam.panel[j]), fname);
                    }
                    for (int j = 0; j < PublicDataClass._AIParam.num; j++)
                    {
                        str = String.Format("value_{0:d}", j);

                        WritePrivateProfileString("VALUETABLE", str, Convert.ToString(PublicDataClass._AIParam.value[j]), fname);
                    }
                    for (int j = 0; j < PublicDataClass._AIParam.num; j++)
                    {
                        str = String.Format("ph_{0:d}", j);

                        WritePrivateProfileString("PHTABLE", str, Convert.ToString(PublicDataClass._AIParam.ph[j]), fname);
                    }
                    for (int j = 0; j < PublicDataClass._AIParam.num; j++)
                    {
                        str = String.Format("zhishu_{0:d}", j);

                        WritePrivateProfileString("ZHISHUTABLE", str, Convert.ToString(PublicDataClass._AIParam.zhishu[j]), fname);
                    }
                }
                else if (k == 11)          //继保定值
                {
                    str = Convert.ToString(PublicDataClass._RelayProtectParam.num);

                    WritePrivateProfileString("NUM", "PARAMNUM", str, fname);


                    for (int j = 0; j < PublicDataClass._RelayProtectParam.num; j++)
                    {
                        WritePrivateProfileString("NAMETABLE", null, null, fname);     //先清除掉
                        WritePrivateProfileString("VALUETABLE", null, null, fname);
                        WritePrivateProfileString("ADDTABLE", null, null, fname);
                        WritePrivateProfileString("LINETABLE", null, null, fname);     //先清除掉
            
                    }
                    for (int j = 0; j < PublicDataClass._RelayProtectParam.num; j++)
                    {
                        str = String.Format("name_{0:d}", j);

                        WritePrivateProfileString("NAMETABLE", str, Convert.ToString(PublicDataClass._RelayProtectParam.NameTable[j]), fname);
                    }

                    for (int j = 0; j < PublicDataClass._RelayProtectParam.num; j++)
                    {

                        str = String.Format("value_{0:d}", j);
                        WritePrivateProfileString("VALUETABLE", str, Convert.ToString(PublicDataClass._RelayProtectParam.ValueTable[j]), fname);

                    }
                    for (int j = 0; j < PublicDataClass._RelayProtectParam.num; j++)
                    {
                        str = String.Format("add_{0:d}", j);
                        WritePrivateProfileString("ADDTABLE", str, Convert.ToString(PublicDataClass._RelayProtectParam.AddrTable[j]), fname);

                    }
                    for (int j = 0; j < PublicDataClass._RelayProtectParam.num; j++)
                    {
                        str = String.Format("line_{0:d}", j);
                        WritePrivateProfileString("LINETABLE", str, Convert.ToString(PublicDataClass._RelayProtectParam.LineTable[j]), fname);

                    }

                }
            }
        }

        /*************************************************************************
       *  函数名：    ReadDynOptFile                                    *
       *  功能  ：    读动态选项卡配置文件                                     *
       *  参数  ：    fname ：路径名  
       *                                 Type=1  读
       *                                 Type=2   写*
       *                                                   *
       *  返回值：    无                                                       *
       *  修改日期：  2012-5-18                                              *
       *  作者    ：  liuhch                                                  *
       * **********************************************************************/
        public static void ReadDynOptFile(string fname, int k, byte Type)
        {
            string namestr, valuestr;
            if (Type == 1)//动态添加选项卡，读
            {


                GetPrivateProfileString("PAGENAME", "PageName", "无法读取对应数值！",
                                                     temp, 255, fname);

                PublicDataClass.TabCfg[k].PageName = temp.ToString();//选项卡名称
                GetPrivateProfileString("LINENUM", "LineNum", "无法读取对应数值！",
                                                temp, 255, fname);
                PublicDataClass.TabCfg[k].LineNum = int.Parse(temp.ToString());       //行数     
                GetPrivateProfileString("COLLUMNNUM", "ColumnNum", "无法读取对应数值！",
                                         temp, 255, fname);
                PublicDataClass.TabCfg[k].ColumnNum = int.Parse(temp.ToString());       //列数
                PublicDataClass.TabCfg[k].ColumnPageName = new string[PublicDataClass.TabCfg[k].ColumnNum];
                PublicDataClass.TabCfg[k].ColumnDownByte = new string[PublicDataClass.TabCfg[k].ColumnNum];
                PublicDataClass.TabCfg[k].TabPageValue = new PublicDataClass.TabPageValueTable[PublicDataClass.TabCfg[k].ColumnNum];

                for (int j = 0; j < PublicDataClass.TabCfg[k].ColumnNum; j++)   //列名称
                {
                    str = String.Format("ColumnPageName_{0:d}", j);

                    GetPrivateProfileString("COLLUMNPAGENAME", str, "无法读取对应数值！",
                                                 temp, 255, fname);
                    PublicDataClass.TabCfg[k].ColumnPageName[j] = temp.ToString();
                }
                for (int j = 0; j < PublicDataClass.TabCfg[k].ColumnNum; j++)   //列名称
                {
                    str = String.Format("Columndownbyte_{0:d}", j);

                    GetPrivateProfileString("COLLUMDOWNBYTE", str, "无法读取对应数值！",
                                                 temp, 255, fname);
                    PublicDataClass.TabCfg[k].ColumnDownByte[j] = temp.ToString();
                }

                for (int j = 0; j < PublicDataClass.TabCfg[k].ColumnNum; j++)
                {
                    namestr = String.Format("ColumnValueTable_{0:d}", j);
                    PublicDataClass.TabCfg[k].TabPageValue[j].ValueTable = new string[PublicDataClass.TabCfg[k].LineNum];
                    for (int q = 0; q < PublicDataClass.TabCfg[k].LineNum; q++)
                    {

                        valuestr = String.Format("ValueTable_{0:d}", q);

                        GetPrivateProfileString(namestr, valuestr, "无法读取对应数值！",
                                                      temp, 255, fname);
                        PublicDataClass.TabCfg[k].TabPageValue[j].ValueTable[q] = temp.ToString();


                    }
                }
                GetPrivateProfileString("PARAMINFOADDR", "ParamInfoAddr", "无法读取对应数值！",
                                                   temp, 255, fname);

                PublicDataClass.TabCfg[k].DownAddr = int.Parse(temp.ToString());   //下载地址

            }
            else//动态添加选项卡，写
            {
                str = Convert.ToString(PublicDataClass.TabCfg[k].LineNum);  //行数
                WritePrivateProfileString("LINENUM", "LineNum", str, fname);


                for (int q = 0; q < PublicDataClass.TabCfg[k].LineNum; q++)
                {

                    for (int j = 0; j < PublicDataClass.TabCfg[k].ColumnNum; j++)
                    {
                        namestr = String.Format("ColumnValueTable_{0:d}", j);
                        WritePrivateProfileString(namestr, null, null, fname);     //先清除掉
                    }
                }

                for (int j = 0; j < PublicDataClass.TabCfg[k].ColumnNum; j++)
                {
                    namestr = String.Format("ColumnValueTable_{0:d}", j);

                    for (int q = 0; q < PublicDataClass.TabCfg[k].LineNum; q++)
                    {
                        valuestr = String.Format("ValueTable_{0:d}", q);
                        WritePrivateProfileString(namestr, valuestr, Convert.ToString(PublicDataClass.TabCfg[k].TabPageValue[j].ValueTable[q]), fname);
                    }
                }


            }
        }
    }
}
