using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FaTestSoft.CommonData;

namespace FaTestSoft
{
    namespace FUNCTIONCLASS
    {

        class PublicFunction
        {
            /*************************************************************************
             *  函数名：    CheckPointOfCommunicationEntrace                         *
             *  功能  ：    判断测量点所在的通讯口                                   *
             *  参数  ：    pointname ：测量点名                                     *
             *  返回值：    通道类型 1-串口  2-以太网 3-gprs  4-USB    0-无                 * 
             *  修改日期：  2010-11-09                                               *
             *  作者    ：  cuibj                                                    *
             * **********************************************************************/
            public static int CheckPointOfCommunicationEntrace(string pointname)
            {
                byte type =0;
                Regex cm = new Regex("COM"); Regex nt = new Regex("NET"); Regex gs = new Regex("GPRS"); Regex ub = new Regex("USB");
                for(int i=0;i<PublicDataClass.SaveText.devicenum;i++)
                {
                     if(pointname ==PublicDataClass.SaveText.Device[i].PointName)
                     {
                         Match m = cm.Match(PublicDataClass.SaveText.Device[i].ChannelID);
                         if (m.Success)
                         {
                             type = 1;
                             break;
                         }
                         m = nt.Match(PublicDataClass.SaveText.Device[i].ChannelID);
                         if (m.Success)
                         {
                             type = 2;
                             break;
                         }
                         m = gs.Match(PublicDataClass.SaveText.Device[i].ChannelID);
                         if (m.Success)
                         {
                             type = 3;
                             break;
                         }
                         m = ub.Match(PublicDataClass.SaveText.Device[i].ChannelID);
                         if (m.Success)
                         {
                             type = 4;
                             break;
                         }

                     }
                }

                return type;
            }

            /*************************************************************************
             *  函数名：    BCDtoHex                                                 *
             *  功能  ：    BCD码转16进制                                            *
             *  参数  ：    bcd ：值                                                 *
             *  返回值：    转换后的结果                                             *
             *  修改日期：  2010-11-09                                               *
             *  作者    ：  cuibj                                                    *
             * **********************************************************************/
            public static byte BCDtoHex(byte bcd)
            {
                byte result;
                result = Convert.ToByte((((bcd & 0xF0) >> 4) * 10) + (bcd & 0xF));
                return result;
            }
            /*************************************************************************
             *  函数名：    HextoBCD                                                 *
             *  功能  ：    16进制转BCD码                                            *
             *  参数  ：    bcd ：值                                                 *
             *  返回值：    转换后的结果                                             *
             *  修改日期：  2010-11-09                                               *
             *  作者    ：  cuibj                                                    *
             * **********************************************************************/
            public static byte HextoBCD(byte hex)
            {
                byte result;
                result = Convert.ToByte((hex / 10) * 16 + (hex % 10));
                return result;
            }
            /*************************************************************************
             *  函数名：    FindStartPosCorrelativeName                              *
             *  功能  ：    查找与起始点号相对应的名称                               *
             *  参数  ：    pos ：点号                                               *
             *  返回值：    转换后的结果string型                                     *
             *  修改日期：  2010-11-09                                               *
             *  作者    ：  cuibj                                                    *
             * **********************************************************************/
            public static string FindStartPosCorrelativeName(byte ch,int pos,int dex)
            {
                string name = @"";

                if (ch == 1)
                {
                    for (int i = 0; i < PublicDataClass.SaveText.Cfg[dex].YccfgNum; i++)
                    {
                       
                        if (pos == Convert.ToInt16(PublicDataClass.SaveText.Cfg[dex].YccfgAddr[i]))
                        {
                            name = PublicDataClass.SaveText.Cfg[dex].YccfgName[i];
                            break;

                        }

                    }

                }
                else if (ch == 2)
                {
                    for (int i = 0; i < PublicDataClass.SaveText.Cfg[dex].YxcfgNum; i++)
                    {
                            if (pos == Convert.ToInt16(PublicDataClass.SaveText.Cfg[dex].YxcfgAddr[i]))
                            {
                                name = PublicDataClass.SaveText.Cfg[dex].YxcfgName[i];
                                break;
                            }
                    }
                }
                return name;
            }

            public static int FindStartPos(byte ch, int pos, int dex)
            {
                string name = @"";
                int p = 0;

                if (ch == 1)
                {
                    for (int i = 0; i < PublicDataClass.SaveText.Cfg[dex].YccfgNum; i++)
                    {

                        if (pos == Convert.ToInt16(PublicDataClass.SaveText.Cfg[dex].YccfgAddr[i]))
                        {
                            name = PublicDataClass.SaveText.Cfg[dex].YccfgName[i];
                            p = i;
                            break;

                        }

                    }

                }
                else if (ch == 2)
                {
                    for (int i = 0; i < PublicDataClass.SaveText.Cfg[dex].YxcfgNum; i++)
                    {
                        if (pos == Convert.ToInt16(PublicDataClass.SaveText.Cfg[dex].YxcfgAddr[i]))
                        {
                            name = PublicDataClass.SaveText.Cfg[dex].YxcfgName[i];
                            p = i;
                            break;
                        }
                    }
                }
                return p;
            }
            public static byte FindPointNameCorrelativeIndex(string pointname)
            {
                   byte index = 0;
                   for (index = 0; index < PublicDataClass.SaveText.cfgnum; index++)
                   {
                       if (pointname == PublicDataClass.SaveText.Device[index].PointName)
                       {
                           break;
                       }
                   }
                   return index;
            }
            public static byte StringToByte(string str)
            {
                byte[] data = new byte[2];

                for (byte j = 0; j < str.Length; j++)
                {
                    if ((str[j] >= '0') && (str[j] <= '9'))
                        data[j] = Convert.ToByte(str[j] - '0');
                    if ((str[j] >= 'A') && (str[j] <= 'F'))
                        data[j] = Convert.ToByte(str[j] - 'A' + 10);
                    if ((str[j] >= 'a') && (str[j] <= 'f'))
                        data[j] = Convert.ToByte(str[j] - 'a' + 10);
                }
                return Convert.ToByte((data[0] << 4) + data[1]);

            }
        }
    }
}
