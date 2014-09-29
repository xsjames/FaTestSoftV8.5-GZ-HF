using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FaTestSoft.CommonData;              //使用新增加的类所在的命名空间
using System.IO;

namespace FaTestSoft
{
    namespace INIT
    {
        class InitGlobalData
        {
            /*************************************************************************
                 *  函数名：    InitAllGlobalVariables                                   *
                 *  功能  ：    初始化所有的全局变量                                     *
                 *  参数  ：    无                                                       *
                 *  返回值：    无                                                       *
                 *  修改日期：  2010-11-09                                               *
                 *  作者    ：  cuibj                                                    *
                 * **********************************************************************/
            public static void InitAllGlobalVariables()
            {

                PublicDataClass.PrjPath = @"";
                PublicDataClass.PrjName = @"";

            }
            /*************************************************************************
            *  函数名：    InitAllGlobalIniValue                                    *
            *  功能  ：    初始化所有的ini变量                                      *
            *  参数  ：    无                                                       *
            *  返回值：    无                                                       *
            *  修改日期：  2010-11-09                                               *
            *  作者    ：  cuibj                                                    *
            * **********************************************************************/
            public static void InitAllGlobalIniValue()
            {
                string FileName = "";
                //string path = System.Environment.CurrentDirectory;
                //path += "\\ini";
             
                string path = PublicDataClass.PrjPath + "\\ini";
                for (byte i = 0; i < 11; i++)
                {
                    switch (i)
                    {
                        case 0:
                            FileName = path + "\\comparam.ini";
                            break;
                        case 1:
                            FileName = path + "\\netparam.ini";
                            break;
                        case 2:
                            FileName = path + "\\sysparam.ini";
                            break;
                        case 3:
                            FileName = path + "\\YCconfig.ini";
                            break;
                        case 4:
                            FileName = path + "\\YXconfig.ini";
                            break;
                        case 5:
                            FileName = path + "\\YKconfig.ini";
                            break;

                        case 6:
                            FileName = path + "\\ychistroydataname.ini";
                            break;
                        case 7:
                            FileName = path + "\\Tjhistroydataname.ini";
                            break;
                        case 8:
                            FileName = path + "\\protocol.ini";
                            break;
                        case 9:
                            FileName = path + "\\CurvePos.ini";
                            break;
                        case 10:
                            FileName = path + "\\模拟量接入配置.ini";
                            break;

                    }
                    WriteReadAllFile.WriteReadParamIniFile(FileName, 0, i);
                   //Form1.WriteIniFilek = i;
                   //Form1.WriteIniFileName = FileName;
                   //Form1.WriteIniFileType = 0;
                   //Form1.WriteIniflag = true;
                }


                string path1 = PublicDataClass.PrjPath + "\\ini\\动态配置";
                //   string path1 = PublicDataClass.PrjPath + "\\ini\\XML";
                string[] s = Directory.GetFiles(path1);

                PublicDataClass.FILENAME = new string[s.Length];

                if (s.Length > 0)
                {
                    PublicDataClass.DynOptHaveFlag = 1;
                    PublicDataClass.DynOptHaveNum = s.Length;
                }

                //协处理器
                string[] s1;
                string path2 = PublicDataClass.PrjPath + "\\ini\\协处理器动态配置";
                try
                {
                    s1 = Directory.GetFiles(path2);
                }
                catch
                {
                    s1 = new string[0];

                }

                PublicDataClass.XIEFILENAME = new string[s1.Length];
                PublicDataClass.XieTabCfg = new PublicDataClass.TabPageCfgParam[PublicDataClass.XIEFILENAME.Length];//分配变量

                PublicDataClass.TabCfg = new PublicDataClass.TabPageCfgParam[PublicDataClass.FILENAME.Length + PublicDataClass.XIEFILENAME.Length];//分配变量
                for (byte i = 0; i < s.Length; i++)
                {
                    PublicDataClass.FILENAME[i] = s[i];
               //     WriteReadAllFile.ReadDynOptFile(PublicDataClass.FILENAME[i], i, 1);
                }
                for (byte i = 0; i < s1.Length; i++)
                {
                    PublicDataClass.XIEFILENAME[i] = s1[i];
                    WriteReadAllFile.ReadDynOptFile(PublicDataClass.XIEFILENAME[i], i + PublicDataClass.FILENAME.Length, 1);
                }
           
                //协处理器的动态选项卡接着主处理器动态选项卡存储，为名称方便转存 PublicDataClass.XieTabCfg
                for (int i = 0; i < s1.Length; i++)
                {


                    PublicDataClass.XieTabCfg[i] = PublicDataClass.TabCfg[i + PublicDataClass.FILENAME.Length];


                }
                if (s1.Length > 0)
                    PublicDataClass.XieDynOptCfgFlag = 1; 


            }

            public static void SaveAllGlobalIniValue()
            {
                string FileName = "";
                string path = PublicDataClass.PrjPath + "\\ini";
                for (byte i = 0; i < 11; i++)
                {
                    switch (i)
                    {
                        case 0:
                            FileName = path + "\\comparam.ini";
                            break;
                        case 1:
                            FileName = path + "\\netparam.ini";
                            break;
                        case 2:
                            FileName = path + "\\sysparam.ini";
                            break;
                        case 3:
                            FileName = path + "\\YCconfig.ini";
                            break;
                        case 4:
                            FileName = path + "\\YXconfig.ini";
                            break;
                        case 5:
                            FileName = path + "\\YKconfig.ini";
                            break;

                        case 6:
                            FileName = path + "\\ychistroydataname.ini";
                            break;
                        case 7:
                            FileName = path + "\\Tjhistroydataname.ini";
                            break;
                        case 8:
                            FileName = path + "\\protocol.ini";
                            break;
                        case 9:
                            FileName = path + "\\CurvePos.ini";
                            break;
                        case 10:
                            FileName = path + "\\模拟量接入配置.ini";
                            break;

                    }
                       WriteReadAllFile.WriteReadParamIniFile(FileName, 1, i);
                    //Form1.WriteIniFilek = i;
                    //Form1.WriteIniFileName = FileName;
                    //Form1.WriteIniFileType = 1;
                    //Form1.WriteIniflag = true;
                }
                if (PublicDataClass.DynOptCfgFlag == 1)//动态选项卡已配置
                {
                    for (int k = 0; k < PublicDataClass.FILENAME.Length; k++)
                    {
                        WriteReadAllFile.ReadDynOptFile(PublicDataClass.FILENAME[k], k, 2);
                    }
                }
            }

        }
    }
}
