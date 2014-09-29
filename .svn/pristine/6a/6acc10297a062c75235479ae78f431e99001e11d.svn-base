
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;




namespace FaTestSoft
{
   public struct ReadWriteReturnType
    {
        public bool FuncReturn;
        public uint LenReturn;

    }
    
    public class USBCmd
    {
        System.IntPtr handle;
        [System.Runtime.InteropServices.DllImport("CH375DLL", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern unsafe System.IntPtr CH375OpenDevice  // 打开CH375设备,返回句柄,出错则无效
        (
            uint iIndex                                     // 指定CH375设备序号,0对应第一个设备,-1则自动搜索一个可以被打开的设备并返回序号
        );
        [System.Runtime.InteropServices.DllImport("CH375DLL", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern unsafe void CH375CloseDevice          // 关闭CH375设备
        (
            uint iIndex                                     // 指定CH375设备序号
        );
        [System.Runtime.InteropServices.DllImport("CH375DLL", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern unsafe uint CH375SetDeviceNotify      // 打开CH375设备,返回句柄,出错则无效
        (
            uint iIndex,                                    // 指定CH375设备序号
            byte[] iDeviceID,
            mPCH375_NOTIFY_ROUTINE iNotifyRoutine
        );
        [System.Runtime.InteropServices.DllImport("CH375DLL", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern unsafe ulong CH375GetUsbID          // 获取VID和PID
        (
            uint iIndex                                     // 指定CH375设备序号
        );
        [System.Runtime.InteropServices.DllImport("CH375DLL", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern unsafe uint CH375SetTimeout
        (
            uint iIndex,                // 指定CH375设备序号
            uint iWriteTimeout,         // 指定USB写出数据块的超时时间,以毫秒mS为单位,0xFFFFFFFF指定不超时(默认值)
            uint iReadTimeout           // 指定USB读取数据块的超时时间,以毫秒mS为单位,0xFFFFFFFF指定不超时(默认值)

        );
        [System.Runtime.InteropServices.DllImport("CH375DLL", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern unsafe uint CH375ReadData
        (
            uint iIndex,                // 指定CH375设备序号
            void* oBuffer,              // 指向一个足够大的缓冲区,用于保存读取的数据
            uint* ioLength              // 指向长度单元,输入时为准备读取的长度,返回后为实际读取的长度

        );
        [System.Runtime.InteropServices.DllImport("CH375DLL", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern unsafe uint CH375WriteData
        (
            uint iIndex,                // 指定CH375设备序号
            void* iBuffer,              // 指向一个缓冲区,放置准备写出的数据
            uint* ioLength              // 指向长度单元,输入时为准备写出的长度,返回后为实际写出的长度

        );
        public bool Open(uint iIndex)
        {
            // open the existing file for reading       
            handle = CH375OpenDevice(iIndex);
            System.IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
            if (handle != INVALID_HANDLE_VALUE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public unsafe  bool SetDeviceNotify(uint iIndex, byte[] iDeviceID, mPCH375_NOTIFY_ROUTINE iNotifyRoutine)
        {
            //if (CH375SetDeviceNotify(iIndex, iDeviceID, iNotifyRoutine) == 0)    //*********************************************************
            {
                return false;
            }
            return true;
        }
        public void Close(uint iIndex)
        {
         //  CH375CloseDevice(iIndex);
        }
        public ulong GetID(uint iIndex)
        {
            return CH375GetUsbID(iIndex);
        }
        public bool SetTimeOut(uint iIndex, uint iWriteTimeout, uint iReadTimeout)
        {
            // open the existing file for reading 
            uint i;
            i = CH375SetTimeout(iIndex, iWriteTimeout, iReadTimeout);

            if (i != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public unsafe ReadWriteReturnType Read(uint iIndex, byte[] buffer, uint Length)
        {
            uint n = Length;
            ReadWriteReturnType rt;
            fixed (byte* p = buffer)
            {
                if (CH375ReadData(iIndex, p, &n) == 0)  //CH375ReadData返回，n是实际读到的数据,注意CH375ReadData会阻塞当前线程
                {
                    rt.LenReturn = 0;
                    rt.FuncReturn = false;
                    return rt;
                }
                else
                {
                    rt.LenReturn = n;
                    rt.FuncReturn = true;
                    return rt;
                }
            }

        }
        public unsafe ReadWriteReturnType Write(uint iIndex, byte[] buffer, uint Length)
        {
            
            uint n = Length;
            ReadWriteReturnType rt;
            fixed (void* p = buffer)
           {
                if (CH375WriteData(iIndex, p, &n) == 0)  //n 返回时，是实际读到的数据
                {
                    rt.LenReturn = 0;
                    rt.FuncReturn = false;
                    return rt;
                }
                else
                {
                    rt.LenReturn = n;
                    rt.FuncReturn = true;
                    return rt;
                }
            }

        }
    }
    

}
