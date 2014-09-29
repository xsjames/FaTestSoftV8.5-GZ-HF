using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FaTestSoft.CommonData;

namespace FaTestSoft
{
    namespace PROTOCOL101
    {
        class protocolty101
        {
            public static int EncodeFrame(byte dataty)
            {

                int linklen = PublicDataClass.linklen;   //
                int cotlen = PublicDataClass.cotlen;
                int publen = PublicDataClass.publen;
                int inflen = PublicDataClass.inflen;


                int Leng = 0;
                byte TypeID = 0;  //类型标识符
                ushort COT = 0; //传送原因
                byte COI = 0;   //可变结构限定词
                

                if (dataty == 1)     //请求链路状态
                {
                    PublicDataClass._ComStructData.TXBuffer[0] = 0x10;  //起始字节
                    PublicDataClass._ComStructData.TXBuffer[1] = 0x49;  //PRM=1，主站-》从站；功能码FC=9，召唤链路状态
                    PublicDataClass._ComStructData.TXBuffer[2] = 0x01;  // 
                    PublicDataClass._ComStructData.TXBuffer[3] = GetSumCheck(1, 1, 2);
                    PublicDataClass._ComStructData.TXBuffer[4] = 0x16;
                    Leng = 5;
                }
                else if (dataty == 2)  //复位远方链路
                {
                    PublicDataClass._ComStructData.TXBuffer[0] = 0x10;  //起始字节
                    PublicDataClass._ComStructData.TXBuffer[1] = 0x40;  //PRM=1，主站-》从站；功能码FC=0，复位远方链路
                    PublicDataClass._ComStructData.TXBuffer[2] = 0x01;  // 
                    PublicDataClass._ComStructData.TXBuffer[3] = GetSumCheck(1, 1, 2);
                    PublicDataClass._ComStructData.TXBuffer[4] = 0x16;
                    Leng = 5;
                }
                else if (dataty == 3)  //请求一级数据
                {
                    PublicDataClass._ComStructData.TXBuffer[0] = 0x10;  //起始字节
                    if (PublicDataClass._ComStructData.FCB == false)
                        PublicDataClass._ComStructData.TXBuffer[1] = 0x5A;  //PRM=1，主站-》从站；FCV=1；功能码FC=10，召唤一级数据
                    else
                        PublicDataClass._ComStructData.TXBuffer[1] = 0x7A;
                    PublicDataClass._ComStructData.TXBuffer[2] = 0x01;  // 
                    PublicDataClass._ComStructData.TXBuffer[3] = GetSumCheck(1, 1, 2);
                    PublicDataClass._ComStructData.TXBuffer[4] = 0x16;
                    Leng = 5;
                }
                else if (dataty == 4)  //请求二级数据
                {
                    PublicDataClass._ComStructData.TXBuffer[0] = 0x10;  //起始字节
                    if (PublicDataClass._ComStructData.FCB == false)
                        PublicDataClass._ComStructData.TXBuffer[1] = 0x5B;  //PRM=1，主站-》从站；FCV=1；功能码FC=11，召唤二级数据
                    else
                        PublicDataClass._ComStructData.TXBuffer[1] = 0x7B;
                    PublicDataClass._ComStructData.TXBuffer[2] = 0x01;  // 
                    PublicDataClass._ComStructData.TXBuffer[3] = GetSumCheck(1, 1, 2);
                    PublicDataClass._ComStructData.TXBuffer[4] = 0x16;
                    Leng = 5;
                }
                
                //==================在控制方向的系统命令===============================
                else if (dataty == 9)  //测试命令
                {
                    PublicDataClass._ComStructData.TXBuffer[4] = 0x53;  //  ????是0x51还是0x53????
                    TypeID = 104;
                    COT = 6;
                    
                   

                }
               
                else if (dataty == 10)  //总召唤
                {
                    if (PublicDataClass._ComStructData.FCB == false)
                        PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                    else
                        PublicDataClass._ComStructData.TXBuffer[4] = 0x73;
                    TypeID = 100;
                    COT = 6;
                   
                    
                }
                else if (dataty == 11)  //读命令
                {
                    if (PublicDataClass._ComStructData.FCB == false)
                        PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                    else
                        PublicDataClass._ComStructData.TXBuffer[4] = 0x73;
                    TypeID = 102;
                    COT = 6;
                    COI = 0x14;
                    
                }
                else if (dataty == 12)  //时钟同步命令
                {
                    if (PublicDataClass._ComStructData.FCB == false)
                        PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                    else
                        PublicDataClass._ComStructData.TXBuffer[4] = 0x73;
                    TypeID = 103;
                    COT = 6;
                }
                else if (dataty == 13)  //复位进程命令
                {
                    PublicDataClass._ComStructData.TXBuffer[4] = 0x41;
                    TypeID = 105;
                    COT = 6;
                }
                else if (dataty == 14)  //延时获得命令       
                {
                    PublicDataClass._ComStructData.TXBuffer[4] = 0x41;   //？？？
                    TypeID = 106;
                    COT = 6;
                }


             //===============================在控制方向的过程信息==================================
                else if (dataty == 15)  //不带时标的单点命令      
                {
                    if (PublicDataClass._ComStructData.FCB == false)
                        PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                    else
                        PublicDataClass._ComStructData.TXBuffer[4] = 0x73;
                    TypeID = 45;
                    COT = 6;
                }
                else if (dataty == 16)  //不带时标的单点命令撤销      
                {
                    if (PublicDataClass._ComStructData.FCB == false)
                        PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                    else
                        PublicDataClass._ComStructData.TXBuffer[4] = 0x73;
                    TypeID = 45;
                    COT = 8;
                }
                else if (dataty == 17)  //不带时标的双点命令      
                {
                    if (PublicDataClass._ComStructData.FCB == false)
                        PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                    else
                        PublicDataClass._ComStructData.TXBuffer[4] = 0x73;
                    TypeID = 46;
                    COT = 6;
                }
                else if (dataty == 18)  //不带时标的双点命令 撤销     
                {
                    if (PublicDataClass._ComStructData.FCB == false)
                        PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                    else
                        PublicDataClass._ComStructData.TXBuffer[4] = 0x73;
                    TypeID = 46;
                    COT = 8;
                }
                else if (dataty == 19)  //不带时标的升降命令
                {
                    if (PublicDataClass._ComStructData.FCB == false)
                        PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                    else
                        PublicDataClass._ComStructData.TXBuffer[4] = 0x73;
                    TypeID = 47;
                    COT = 6;
                }
                else if (dataty == 20)  //设定命令，归一化值      
                {
                    if (PublicDataClass._ComStructData.FCB == false)
                        PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                    else
                        PublicDataClass._ComStructData.TXBuffer[4] = 0x73;
                    TypeID = 48;
                    COT = 6;
                }
                else if (dataty == 21)  //设定命令，标度化值      
                {
                    if (PublicDataClass._ComStructData.FCB == false)
                        PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                    else
                        PublicDataClass._ComStructData.TXBuffer[4] = 0x73;
                    TypeID = 49;
                    COT = 6;
                }
                else if (dataty == 22)  //设定命令，短浮点值      
                {
                    if (PublicDataClass._ComStructData.FCB == false)
                        PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                    else
                        PublicDataClass._ComStructData.TXBuffer[4] = 0x73;
                    TypeID = 50;
                    COT = 6;
                }
                
                //=============自定义===========================================
                else if (dataty == 31)  //设置参数命令      
                {
                    PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                    TypeID = 110;
                    COT = 6;
                }
                else if (dataty == 32)  //读取设置参数命令      
                {
                    PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                    TypeID = 137;
                    COT = 6;
                }
                else if (dataty == 33)  //读版本号   
                {
                    PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                    TypeID = 144;
                    COT = 6;
                }
                else if (dataty == 34)  //读时间  
                {
                    PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                    TypeID = 143;
                    COT = 6;
                }
                else if (dataty == 35)  //读历史数据  ？？？
                {
                    PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                    TypeID = 102;
                    COT = 6;
                }
                else if (dataty == 36)  //读历史记录数据  
                {
                    PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                    TypeID = 145;
                    COT = 6;
                }
                else if (dataty == 37)  //读器件状态
                {
                    PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                    TypeID = 236;
                    COT = 6;
                }
                else if (dataty == 38)  //单独招遥测
                {
                    PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                    TypeID = 245;
                    COT = 6;
                }
                else if (dataty == 39)  //单独招遥信
                {
                    PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                    TypeID = 141;
                    COT = 6;
                }
                else if (dataty == 40)  //数据转发
                {
                    PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                    TypeID = 250;
                    COT = 6;
                }
                //else if (dataty == 41)  //升级文本校验
                //{
                //    PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                //    TypeID = 251;
                //    COT = 6;
                //}
                //else if (dataty == 42)  //终端升级代码
                //{
                //    PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                //    TypeID = 252;
                //    COT = 6;
                //}
                //else if (dataty == 43)  //ARM升级，包括文本传输、校验、代码升级
                //{
                //    PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                //    TypeID = 253;
                //    COT = 6;
                //}
                //else if (dataty == 44)  //4UDSP升级，包括文本传输、校验、代码升级
                //{
                //    PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                //    TypeID = 254;
                //    COT = 6;
                //}

                //else if (dataty == 45)  //软压板参数下载
                //{
                //    PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                //    TypeID = 180;
                //    COT = 6;
                //}
                //else if (dataty == 46)  //软压板参数读取
                //{
                //    PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                //    TypeID = 181;
                //    COT = 6;
                //}
                else if (dataty == 50)  //自定义转104
                {
                    TypeID = 200;
                    COT = 6;
                    PublicDataClass._ComStructData.TXBuffer[4] = 0x53;
                }

                if (dataty < 9)
                    goto END;
                else
                {
                    PublicDataClass._ComStructData.TXBuffer[0] = 0x68;
                    PublicDataClass._ComStructData.TXBuffer[3] = 0x68;
                    if (linklen==1)
                        PublicDataClass._ComStructData.TXBuffer[5] = (byte)((PublicDataClass.DevAddr) & 0x00ff);
                    else if (linklen == 2)
                    {
                        PublicDataClass._ComStructData.TXBuffer[5] = (byte)((PublicDataClass.DevAddr) & 0x00ff);
                        PublicDataClass._ComStructData.TXBuffer[6] = (byte)(((PublicDataClass.DevAddr) & 0xff00) >> 8);
                    }
                    PublicDataClass._ComStructData.TXBuffer[5+linklen] = TypeID;
                    PublicDataClass._ComStructData.TXBuffer[6 + linklen] = (byte)(PublicDataClass._DataField.FieldVSQ);

                    if (cotlen == 1)
                        PublicDataClass._ComStructData.TXBuffer[7 + linklen] = Convert.ToByte(COT & 0x00ff);
                    else if (cotlen==2)
                    {
                        PublicDataClass._ComStructData.TXBuffer[7 + linklen] = Convert.ToByte(COT & 0x00ff);
                        PublicDataClass._ComStructData.TXBuffer[8 + linklen] = Convert.ToByte((COT & 0xff00) >> 8);
                    }

                    if (publen == 1)
                        PublicDataClass._ComStructData.TXBuffer[7 + linklen + cotlen] = Convert.ToByte((PublicDataClass.DevAddr) & 0x00ff);
                    else if (publen == 2)
                    {
                        PublicDataClass._ComStructData.TXBuffer[7 + linklen + cotlen] = (byte)((PublicDataClass.DevAddr) & 0x00ff);
                        PublicDataClass._ComStructData.TXBuffer[8 + linklen + cotlen] = (byte)(((PublicDataClass.DevAddr) & 0xff00) >> 8);
                    }
                    Leng = linklen + cotlen + publen + 7;
   
                    switch (TypeID)
                    {
                        case 100:        //总召唤
                            if (inflen == 1)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x14;
                            }
                            else if (inflen == 2)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x14;
                            }
                            else if (inflen == 3)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x14;
                            }
                            break;
                        case 103:   //对时
                            if (inflen == 1)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;   
                            }
                            else if (inflen == 2)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;    
                            }
                            else if (inflen == 3)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            }         
                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                            break;
                        case 104:   //测试命令
                            if (inflen == 1)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x55;//限定词字节
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0xAA;//限定词字节
                            }
                            else if (inflen == 2)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x55;//限定词字节
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0xAA;//限定词字节
                            }
                            else if (inflen == 3)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x55;//限定词字节
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0xAA;//限定词字节
                            }
                            break;
                        case 105:   //复位进程
                            if (inflen == 1)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x01;//限定词字节
                            }
                            else if (inflen == 2)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x01;//限定词字节：1表示总复位
                            }
                            else if (inflen == 3)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x01;//限定词字节
                            }
                            
                            break;    
                        case 106:   //延时获得命令
                            if (inflen == 1)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;   
                            }
                            else if (inflen == 2)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;   
                            }
                            else if (inflen == 3)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            }
                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                            
                            break;
                        case 45:   //不带时标单点控制
                            if (inflen == 1)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.YkStartPos) & 0x00ff);
                            }
                            else if (inflen == 2)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.YkStartPos) & 0x00ff);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)(((PublicDataClass.YkStartPos) & 0xff00) >> 8);
                            }
                            else if (inflen == 3)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.YkStartPos) & 0x00ff);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)(((PublicDataClass.YkStartPos) & 0xff00) >> 8);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            }
                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                            
                            break;

                        case 46:   //不带时标双点控制
                            if (inflen == 1)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.YkStartPos) & 0x00ff);
                            }
                            else if (inflen == 2)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.YkStartPos) & 0x00ff);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)(((PublicDataClass.YkStartPos) & 0xff00) >> 8);
                            }
                            else if (inflen == 3)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.YkStartPos) & 0x00ff);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)(((PublicDataClass.YkStartPos) & 0xff00) >> 8);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            }
                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                           
                            break;
                        case 58:   //不带时标单点控制
                            if (inflen == 1)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.YkStartPos) & 0x00ff);
                            }
                            else if (inflen == 2)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.YkStartPos) & 0x00ff);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)(((PublicDataClass.YkStartPos) & 0xff00) >> 8);
                            }
                            else if (inflen == 3)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.YkStartPos) & 0x00ff);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)(((PublicDataClass.YkStartPos) & 0xff00) >> 8);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            }
                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                           
                            break;

                        case 59:   //不带时标双点控制
                            if (inflen == 1)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.ParamInfoAddr) & 0x00ff);
                            }
                            else if (inflen == 2)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.ParamInfoAddr) & 0x00ff);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)(((PublicDataClass.ParamInfoAddr) & 0xff00) >> 8);
                            }
                            else if (inflen == 3)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.ParamInfoAddr) & 0x00ff);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)(((PublicDataClass.ParamInfoAddr) & 0xff00) >> 8);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            }
                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                           
                            break;
                        //================以下为自定义类型=========================================
                        case 110:   //设置参数
                            if (inflen == 1)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.ParamInfoAddr) & 0x00ff);
                            }
                            else if (inflen == 2)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.ParamInfoAddr) & 0x00ff);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)(((PublicDataClass.ParamInfoAddr) & 0xff00) >> 8);
                            }
                            else if (inflen == 3)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.ParamInfoAddr) & 0x00ff);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)(((PublicDataClass.ParamInfoAddr) & 0xff00) >> 8);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            }
                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                           
                            break;
                        case 137:   //读参数
                            if (inflen == 1)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.ParamInfoAddr) & 0x00ff);
                            }
                            else if (inflen == 2)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.ParamInfoAddr) & 0x00ff);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)(((PublicDataClass.ParamInfoAddr) & 0xff00) >> 8);
                            }
                            else if (inflen == 3)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.ParamInfoAddr) & 0x00ff);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)(((PublicDataClass.ParamInfoAddr) & 0xff00) >> 8);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            }
                            
                            break;
                        case 144:        //读版本号
                            if (inflen == 1)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            }
                            else if (inflen == 2)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            }
                            else if (inflen == 3)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            }
                            PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x14;
                            
                            break;
                        case 143:        //读时间
                            if (inflen == 1)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            }
                            else if (inflen == 2)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            }
                            else if (inflen == 3)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            }
                            PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x14;
                           
                            break;
                        case 102:        //历史数据
                            if (inflen == 1)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.ParamInfoAddr) & 0x00ff);
                            }
                            else if (inflen == 2)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.ParamInfoAddr) & 0x00ff);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)(((PublicDataClass.ParamInfoAddr) & 0xff00) >> 8);
                            }
                            else if (inflen == 3)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.ParamInfoAddr) & 0x00ff);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)(((PublicDataClass.ParamInfoAddr) & 0xff00) >> 8);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            }
                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                            
                            break;
                        case 145:        //历史记录
                            if (inflen == 1)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.ParamInfoAddr) & 0x00ff);
                            }
                            else if (inflen == 2)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.ParamInfoAddr) & 0x00ff);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)(((PublicDataClass.ParamInfoAddr) & 0xff00) >> 8);
                            }
                            else if (inflen == 3)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.ParamInfoAddr) & 0x00ff);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)(((PublicDataClass.ParamInfoAddr) & 0xff00) >> 8);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            }
                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                            
                            break;
                        case 236:        //器件状态
                            if (inflen == 1)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            }
                            else if (inflen == 2)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            }
                            else if (inflen == 3)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            }
                            PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x14;

                            break;
                        case 141:        //单独招遥信
                            if (inflen == 1)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            }
                            else if (inflen == 2)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            }
                            else if (inflen == 3)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            }
                            PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x14;
                            
                            break;
                        case 245:        //单独招yaoce
                            if (inflen == 1)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            }
                            else if (inflen == 2)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            }
                            else if (inflen == 3)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            }
                            PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x14;
                           
                            break;
                        case 250:        //升级文本下载
                            if (inflen == 1)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.ParamInfoAddr) & 0x00ff);
                            }
                            else if (inflen == 2)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.ParamInfoAddr) & 0x00ff);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)(((PublicDataClass.ParamInfoAddr) & 0xff00) >> 8);
                            }
                            else if (inflen == 3)
                            {
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.ParamInfoAddr) & 0x00ff);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)(((PublicDataClass.ParamInfoAddr) & 0xff00) >> 8);
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            }
                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                            
                            break;

                        case 200:        //自定义转101
                            PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                            PublicDataClass._ComStructData.TXBuffer[Leng++] = PublicDataClass.ZDYtype;
                            PublicDataClass._ComStructData.TXBuffer[Leng++] = PublicDataClass.addselect;
                            PublicDataClass._ComStructData.TXBuffer[Leng++] = PublicDataClass.seqflag;
                            PublicDataClass._ComStructData.TXBuffer[Leng++] = PublicDataClass.seq;
                            PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)(PublicDataClass._DataField.FieldVSQ & 0x00ff); ;
                            PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass._DataField.FieldVSQ & 0xff00) >> 8);
                            PublicDataClass._ComStructData.TXBuffer[Leng++] = PublicDataClass.SQflag;
                            PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)(PublicDataClass.ParamInfoAddr & 0x00ff);
                            PublicDataClass._ComStructData.TXBuffer[Leng++] = (byte)((PublicDataClass.ParamInfoAddr & 0xff00) >> 8);
                            Leng = linklen + cotlen + publen + 7+11;
                   
                            for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                                PublicDataClass._ComStructData.TXBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];

                       
                            break;
                        //case 251:        //升级文本校验
                        //    if (inflen == 1)
                        //    {
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                        //    }
                        //    else if (inflen == 2)
                        //    {
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                        //    }
                        //    else if (inflen == 3)
                        //    {
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                        //    }
                        //    for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                           
                        //    break;
                        //case 252:        //终端升级
                        //    if (inflen == 1)
                        //    {
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                        //    }
                        //    else if (inflen == 2)
                        //    {
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                        //    }
                        //    else if (inflen == 3)
                        //    {
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                        //    }
                        //    PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x14;

                        //    break;
                        //case 253:        //ARM升级（包括文本下载、校验、升级）
                        //    if (inflen == 1)
                        //    {
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                        //    }
                        //    else if (inflen == 2)
                        //    {
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                        //    }
                        //    else if (inflen == 3)
                        //    {
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                        //    }
                        //    for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                            
                        //    break;
                        //case 254:         //4UDSP升级（包括文本下载、校验、升级）
                        //    if (inflen == 1)
                        //    {
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                        //    }
                        //    else if (inflen == 2)
                        //    {
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                        //    }
                        //    else if (inflen == 3)
                        //    {
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = 0x00;
                        //    }
                        //    for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                           
                        //    break;
                        //case 180:         //4UDSP升级（包括文本下载、校验、升级）  
                        //    for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                            
                        //    break;
                        //case 181:         //4UDSP升级（包括文本下载、校验、升级）  
                        //    for (int i = 0; i < PublicDataClass._DataField.FieldLen; i++)
                        //        PublicDataClass._ComStructData.TXBuffer[Leng++] = PublicDataClass._DataField.Buffer[i];
                           
                        //    break;
                        default:
                            break;
                    }
                    PublicDataClass._ComStructData.TXBuffer[1] = (byte)(Leng - 4);
                    PublicDataClass._ComStructData.TXBuffer[2] = (byte)(Leng - 4);
                    PublicDataClass._ComStructData.TXBuffer[Leng] = GetSumCheck(1,4, (Leng - 4));	//	校验和
                    PublicDataClass._ComStructData.TXBuffer[Leng +1] = 0x16;
                    Leng += 2;
                }
            END:
                return Leng;
            }
            public static byte DecodeFrame()
            {
                int linklen = PublicDataClass.linklen;   //
                int cotlen = PublicDataClass.cotlen;
                int publen = PublicDataClass.publen;
                int inflen = PublicDataClass.inflen;

                byte dataty = 0;
                byte Length = 0;
                byte TypeID = 0;  //类型标识符
                //byte VSQ=0;
                ushort COT = 0; //传送原因
                int Infaddr = 0;
                byte COI = 0;   //可变结构限定词
                byte AFN = 0;


                if ((PublicDataClass._ComStructData.RXBuffer[0] != 0x68) && (PublicDataClass._ComStructData.RXBuffer[0] != 0x10))  //非法帧
                {
                    if ((PublicDataClass._ComStructData.RXBuffer[0] == 0xE5) && PublicDataClass._ComStructData.RxLen==1)
                      dataty = 1;   //单个字符确认帧
                    else
                      dataty = 0;   //非法帧
                }
                else if(PublicDataClass._ComStructData.RXBuffer[0] == 0x10)
                {
                    if (GetSumCheck(2, 1, 2) != PublicDataClass._ComStructData.RXBuffer[3])
                        dataty = 0;   //非法帧
                    else
                    { 
                        if((PublicDataClass._ComStructData.RXBuffer[1]&0x0F)==0x0B)  //链路状态正常
                            dataty = 2;
                        else if ((PublicDataClass._ComStructData.RXBuffer[1] & 0x0F) == 0x00)  //复位链路确认
                            dataty = 3;
                      
                        else if ((PublicDataClass._ComStructData.RXBuffer[1] & 0xFF) == 0x80)  //确认帧
                            dataty = 3;
                        else if ((PublicDataClass._ComStructData.RXBuffer[1] & 0xFF) == 0x09)  //无所召唤的数据
                            dataty = 114;
                        else if ((PublicDataClass._ComStructData.RXBuffer[1] & 0xFF) == 0x88)  //以数据响应
                            dataty = 115;


                        if ((PublicDataClass._ComStructData.RXBuffer[1] & 0x20) == 0x20)
                            PublicDataClass._ProtocoltyFlag.ACD = 1;
                        else
                            PublicDataClass._ProtocoltyFlag.ACD = 2;

                    }
                }
                else if ((PublicDataClass._ComStructData.RXBuffer[0] == 0x68) && (PublicDataClass._ComStructData.RXBuffer[3] == 0x68))
                {
                    if ((GetSumCheck(2, 4, PublicDataClass._ComStructData.RxLen - 6) != PublicDataClass._ComStructData.RXBuffer[PublicDataClass._ComStructData.RxLen - 2]) || (PublicDataClass._ComStructData.RXBuffer[PublicDataClass._ComStructData.RxLen - 1] != 0x16))
                        dataty = 0;   //非法帧
                    else
                    {
                        if ((PublicDataClass._ComStructData.RXBuffer[4] & 0x20) == 0x20)
                            PublicDataClass._ProtocoltyFlag.ACD = 1;
                        else
                            PublicDataClass._ProtocoltyFlag.ACD = 2;
                        TypeID = PublicDataClass._ComStructData.RXBuffer[5+linklen];
                        PublicDataClass._DataField.FieldVSQ = PublicDataClass._ComStructData.RXBuffer[6 + linklen] & 0x7f;
                        if (cotlen == 1)
                            COT = PublicDataClass._ComStructData.RXBuffer[7 + linklen];
                        else if(cotlen == 2)
                            COT = Convert.ToUInt16((PublicDataClass._ComStructData.RXBuffer[8+ linklen] << 8) + PublicDataClass._ComStructData.RXBuffer[7 + linklen]);
                       
                        
                        if(inflen==1)
                            PublicDataClass.ParamInfoAddr = Convert.ToInt32(PublicDataClass._ComStructData.RXBuffer[7 + linklen + cotlen + publen]);
                        else if(inflen==2)
                            PublicDataClass.ParamInfoAddr = Convert.ToInt32(PublicDataClass._ComStructData.RXBuffer[7 + linklen + cotlen + publen] + (PublicDataClass._ComStructData.RXBuffer[8 + linklen + cotlen + publen])<<8);
                        else if (inflen == 3)
                            PublicDataClass.ParamInfoAddr = Convert.ToInt32(PublicDataClass._ComStructData.RXBuffer[7 + linklen + cotlen + publen] + (PublicDataClass._ComStructData.RXBuffer[8 + linklen + cotlen + publen] << 8 )+ (PublicDataClass._ComStructData.RXBuffer[9 + linklen + cotlen + publen] << 16));
                        COI = PublicDataClass._ComStructData.RXBuffer[7 + linklen + cotlen + publen + inflen];
                        switch (TypeID)
                        {
                            case 100:   //总召
                                if (COT == 7)     //总召唤激活；
                                    dataty = 4;
                                else if (COT == 10)
                                    dataty = 5;  //总召结束
                                break;
                            case 103:  //对时
                                for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + inflen); i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen + inflen];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + inflen);
                                dataty = 6;
                               
                                break;
                            case 104:  //测试命令

                                //dataty = 7;
                                break;
                            case 105:  //复位进程

                                dataty = 7;
                                break;
                            case 106:  //延时获得

                                dataty = 8;
                                break;
                            case 45:           //不带时标单点遥控
                                if (COT == 0x0007 && ((COI & 0x80) == 0x80))   //选择确认
                                    dataty = 10;
                                if (COT == 0x0007 && COI == 0x00)
                                    dataty = 11;                          //执行分成功
                                if (COT == 0x0007 && COI == 0x01)
                                    dataty = 11;                          //执行合成功
                                if (COT == 0x0009)
                                    dataty = 12;                            //遥控撤销确认
                                //PublicDataClass.RxSeq += 2;
                                //if (PublicDataClass.RxSeq >= 0x7fff)
                                //    PublicDataClass.RxSeq = 0;
                                break;
                            case 46:           //不带时标双点点遥控
                                if (COT == 0x0007 && ((COI & 0x80) == 0x80))   //选择确认
                                    dataty = 13;
                                if (COT == 0x0007 && COI == 0x01)
                                    dataty = 14;                          //执行分成功
                                if (COT == 0x0007 && COI == 0x02)
                                    dataty = 14;                          //执行合成功
                                if (COT == 0x0009)
                                    dataty = 15;                            //遥控撤销确认 
                                break;
                            case 58:           //带时标单点遥控
                                if (COT == 0x0007 && ((COI & 0x80) == 0x80))   //选择确认
                                    dataty = 16;
                                if (COT == 0x0007 && COI == 0x00)
                                    dataty = 17;                          //执行分成功
                                if (COT == 0x0007 && COI == 0x01)
                                    dataty = 17;                          //执行合成功
                                if (COT == 0x0009)
                                    dataty = 18;                            //遥控撤销确认
                                break;
                            case 59:           //带时标双点遥控
                                if (COT == 0x0007 && ((COI & 0x80) == 0x80))   //选择确认
                                    dataty = 19;
                                if (COT == 0x0007 && COI == 0x01)
                                    dataty = 20;                          //执行分成功
                                if (COT == 0x0007 && COI == 0x02)
                                    dataty = 20;                          //执行合成功
                                if (COT == 0x0009)
                                    dataty = 21;                            //遥控撤销确认 
                                break;
                            case 110:               //设置参数
                                if (COI == 1)
                                    dataty = 22;     //设置参数确认
                                else if (COI == 0)
                                    dataty = 23;     //设置参数否认 
                                break;
                            case 137:              //读参数
                                PublicDataClass._DataField.FieldVSQ = PublicDataClass._ComStructData.RXBuffer[6 + linklen];
                                for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + inflen); i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen + inflen];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + inflen);
                                dataty = 24;
                                break;
                            case 144:               //读版本号
                                for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + inflen); i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen + inflen];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + inflen);
                                dataty = 25;
                                
                                break;
                            case 236:               //读器件状态
                                for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + inflen); i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen + inflen];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + inflen);
                                dataty = 26;
                                
                                break;
                            case 102:               //历史数据
                                PublicDataClass._DataField.FieldVSQ = PublicDataClass._ComStructData.RXBuffer[6 + linklen];
                                for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + inflen); i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen + inflen];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + inflen);
                                dataty = 27;
                                
                                break;
                            case 145:               //历史记录
                                PublicDataClass._DataField.FieldVSQ = PublicDataClass._ComStructData.RXBuffer[6 + linklen];
                                for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + inflen); i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen + inflen];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + inflen);
                                dataty = 28;
                                
                                break;
                            case 250:               //数据转发回复
                                PublicDataClass._DataField.FieldVSQ = PublicDataClass._ComStructData.RXBuffer[6 + linklen];
                                for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + inflen); i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen + inflen];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + inflen);
                                dataty = 29;
                                
                                break;
                            //case 251:               //升级文本校验
                            //    for (int i = 0; i < 1; i++)
                            //        PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen + inflen];
                            //    PublicDataClass._DataField.FieldLen = 1;
                            //    dataty = 30;
                               
                            //    break;
                            //case 253:               //ARM升级
                            //    for (int i = 0; i < 3; i++)
                            //        PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen + inflen];
                            //    PublicDataClass._DataField.FieldLen = 3;
                            //    dataty = 31;
                                
                            //    break;
                            //case 254:               //4UDSP升级
                            //    for (int i = 0; i < 16; i++)
                            //        PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen + inflen];
                            //    PublicDataClass._DataField.FieldLen = 16;
                            //    dataty = 32;
                               
                            //    break;
                            //case 180:               //软压板参数下载回复
                            //    for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen); i++)
                            //        PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + linklen + cotlen + publen];
                            //    PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen);
                            //    dataty = 33;
                               
                            //    break;
                            //case 181:               //软压板参数读取
                            //    for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen); i++)
                            //        PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + linklen + cotlen + publen];
                            //    PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen);
                            //    dataty = 34;
                                
                            //    break;
                            case 245:               //自定义单独招遥测回复
                                for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen); i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i +7+ linklen + cotlen + publen];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen);
                                dataty = 35;
                               
                                break;
                            case 9:               //遥测 归一化值 2+1字节
                                if (COT == 20)  //正常响应站召唤
                                {
                                    for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen); i++)
                                        PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen];
                                    PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (9+ linklen + cotlen + publen);
                                    dataty = 36;
                                }
                                else if (COT == 3)  //突发，扰动
                                {
                                    for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen); i++)
                                        PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen];
                                    PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen);
                                    dataty = 37;
                                }

                               
                                break;
                            case 11:               //遥测 标度化值  2+1字节
                                if (COT == 20)  //正常响应站召唤
                                {
                                    for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen); i++)
                                        PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen];
                                    PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen);
                                    dataty = 38;
                                }
                                else if (COT == 3)  //突发，扰动
                                {
                                    for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen); i++)
                                        PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen];
                                    PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen);
                                    dataty = 39;
                                }
                                break;
                            case 13:               //遥测短浮点值  4+1字节
                                if (COT == 20)  //正常响应站召唤
                                {
                                    for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (9+ linklen + cotlen + publen); i++)
                                        PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen];
                                    PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen);
                                    dataty = 40;
                                }
                                else if (COT == 3)  //突发，扰动
                                {
                                    for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen); i++)
                                        PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen];
                                    PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen);
                                    dataty = 41;
                                }
                                break;
                            case 21:               //遥测 不带品质描述的归一化值 2字节
                                if (COT == 20)  //正常响应站召唤
                                {
                                    for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen); i++)
                                        PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen];
                                    PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen);
                                    dataty = 42;
                                }
                                else if (COT == 3)  //突发，扰动
                                {
                                    for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen); i++)
                                        PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen];
                                    PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen);
                                    dataty = 43;
                                }    
                                break;
                            //case 34:               //遥测 带56时标归一化值 2+1+7字节
                            //    if (COT == 20)  //正常响应站召唤
                            //    {
                            //        for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen); i++)
                            //            PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + linklen + cotlen + publen];
                            //        PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen);
                            //        dataty = 44;
                            //    }
                            //    else if (COT == 3)  //突发，扰动
                            //    {
                            //        for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen); i++)
                            //            PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + linklen + cotlen + publen];
                            //        PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen);
                            //        dataty = 45;
                            //    } 
                            //    break;
                            //case 35:               //遥测 带56时标的标度化值  2+1+7字节
                            //    if (COT == 20)  //正常响应站召唤
                            //    {
                            //        for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen); i++)
                            //            PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + linklen + cotlen + publen];
                            //        PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen);
                            //        dataty = 46;
                            //    }
                            //    else if (COT == 3)  //突发，扰动
                            //    {
                            //        for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen); i++)
                            //            PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + linklen + cotlen + publen];
                            //        PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen);
                            //        dataty = 47;
                            //    }                              
                            //    break;
                            //case 36:               //遥测 带56时标的短浮点值  4+1+7字节
                            //    if (COT == 20)  //正常响应站召唤
                            //    {
                            //        for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen); i++)
                            //            PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + linklen + cotlen + publen];
                            //        PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen);
                            //        dataty = 48;
                            //    }
                            //    else if (COT == 3)  //突发，扰动
                            //    {
                            //        for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen); i++)
                            //            PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + linklen + cotlen + publen];
                            //        PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen);
                            //        dataty = 49;
                            //    }   
                            //    break;
                            case 141:               //自定义单独招遥信回复
                                for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen); i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen);
                                dataty = 50;
                               
                                break;
                            case 1:               //单点遥信
                                if (COT == 20)  //正常响应站召唤
                                {
                                    for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (9+ linklen + cotlen + publen); i++)
                                        PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen];
                                    PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen);
                                    dataty = 51;
                                }
                                else if (COT == 3)  //突发，遥信变位
                                {
                                    for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (9+ linklen + cotlen + publen); i++)
                                        PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen];
                                    PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen);
                                    dataty = 52;
                                }         
                                break;
                            case 3:               //双点遥信
                                if (COT == 20)  //正常响应站召唤
                                {
                                    for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen); i++)
                                        PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen];
                                    PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen);
                                    dataty = 53;
                                }
                                else if (COT == 3)  //突发，双点遥信变位
                                {
                                    for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen); i++)
                                        PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen];
                                    PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen);
                                    dataty = 54;
                                }   
                                break;
                            case 30:               //带56时标的单点遥信
                                if (COT == 20)  //正常响应站召唤
                                {
                                    for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen); i++)
                                        PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen];
                                    PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (9+ linklen + cotlen + publen);
                                    dataty = 55;
                                }
                                else if (COT == 3)  //突发，遥信变位
                                {
                                    for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen); i++)
                                        PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen];
                                    PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen);
                                    dataty = 56;
                                }

                                break;
                            case 31:               //带56时标的双点遥信
                                if (COT == 20)  //正常响应站召唤
                                {
                                    for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen); i++)
                                        PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen];
                                    PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen);
                                    dataty = 57;
                                }
                                else if (COT == 3)  //突发，双点遥信变位
                                {
                                    for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen); i++)
                                        PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + 7 + linklen + cotlen + publen];
                                    PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (9 + linklen + cotlen + publen);
                                    dataty = 58;
                                }      
                                break;
                            case 22:               //停电事件
                                for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen); i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + linklen + cotlen + publen];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen);
                                dataty = 59;
                                break;
                            case 23:               //故障事件
                                for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen); i++)
                                    PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + linklen + cotlen + publen];
                                PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen);
                                dataty = 60;
                                break;
                            case 200:               //101转自定义
                              
                            
                                AFN = PublicDataClass._ComStructData.RXBuffer[7 + linklen + cotlen + publen+2];
                                PublicDataClass.addselect = PublicDataClass._ComStructData.RXBuffer[7 + linklen + cotlen + publen + 3];
                                PublicDataClass.seqflag = PublicDataClass._ComStructData.RXBuffer[7 + linklen + cotlen + publen + 4];
                                PublicDataClass.seq = PublicDataClass._ComStructData.RXBuffer[7 + linklen + cotlen + publen + 5];
                                PublicDataClass._DataField.FieldVSQ = (PublicDataClass._ComStructData.RXBuffer[7 + linklen + cotlen + publen + 7] << 8) + PublicDataClass._ComStructData.RXBuffer[7 + linklen + cotlen + publen + 6];
                                PublicDataClass.SQflag = PublicDataClass._ComStructData.RXBuffer[7 + linklen + cotlen + publen + 8];
                                PublicDataClass.ParamInfoAddr = (PublicDataClass._ComStructData.RXBuffer[7 + linklen + cotlen + publen + 10] << 8) + PublicDataClass._ComStructData.RXBuffer[7 + linklen + cotlen + publen + 9];
                                switch (AFN)
                                {
                                    case 1:   //参数下载
                                        if (PublicDataClass._ComStructData.RXBuffer[7 + linklen + cotlen + publen + 11] == 0x01)
                                            dataty = 100;    //确认
                                        else if (PublicDataClass._ComStructData.RXBuffer[7 + linklen + cotlen + publen + 11] == 0x00)
                                            dataty = 101;    //否认
                                        break;

                                    case 2:   //参数读取
                                        for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + 13); i++)
                                            PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + (7 + linklen + cotlen + publen + 11)];
                                        PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + 13);
                                        dataty = 102;    //

                                        break;
                                    case 3:   //dsp文本升级
                                        for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + 13); i++)
                                            PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + (7 + linklen + cotlen + publen + 11)];
                                        PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + 13);
                                        dataty = 103;    //

                                        break;
                                    case 4:   //历史记录
                                        for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + 13); i++)
                                            PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + (7 + linklen + cotlen + publen + 11)];
                                        PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + 13);
                                        dataty = 104;    //

                                        break;

                                    case 5:                         //版本号
                                        for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + 13); i++)
                                            PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + (7 + linklen + cotlen + publen + 11)];
                                        PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + 13);
                                        dataty = 105;    //版本号
                                        break;
                                    case 6:                         //时间
                                        for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + 13); i++)
                                            PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + (7 + linklen + cotlen + publen + 11)];
                                        PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + 13);
                                        dataty = 106;    //时间
                                        break;
                                    case 7:                         //对时回复
                                        for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + 13); i++)
                                            PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + (7 + linklen + cotlen + publen + 11)];
                                        PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + 13);
                                        dataty = 107;    //时间
                                        break;
                                    case 8:                         //器件状态
                                        for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + 13); i++)
                                            PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + (7 + linklen + cotlen + publen + 11)];
                                        PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + 13);
                                        dataty = 108;    //器件状态回复
                                        break;
                                    case 9:   //ARM文本升级
                                        for (int i = 0; i < PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + 13); i++)
                                            PublicDataClass._DataField.Buffer[i] = PublicDataClass._ComStructData.RXBuffer[i + (7 + linklen + cotlen + publen + 11)];
                                        PublicDataClass._DataField.FieldLen = PublicDataClass._ComStructData.RxLen - (7 + linklen + cotlen + publen + 13);
                                        dataty = 109;    //

                                        break;
                                }
                                break;
                            default:
                                dataty = 0;
                                break;




                        }
                    
                    }
                }
                else
                {
                    dataty = 0;   //非法帧
                }
   
               
                return dataty;
            }

            public static byte GetSumCheck(int type,int start, int len)
            {
                byte byTempSum = 0;
                if (type == 1)
                {
                    for (int j = 0; j < len; j++)
                    {
                        byTempSum += PublicDataClass._ComStructData.TXBuffer[start+j];
                    }
                }
                else if(type==2)
                {
                    for (int j = 0; j < len; j++)
                    {
                        byTempSum += PublicDataClass._ComStructData.RXBuffer[start + j];
                    }
                }
                return byTempSum;
            }


        }
    }
}
