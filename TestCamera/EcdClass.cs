//----------------------------------------------------------------------------- 
// <copyright file="SR7LinkFunc.cs" company="SSZN">
//	 Copyright (c) 2020 SSZN.  All rights reserved.
// </copyright>                        
// Author:Xiaobin Wang                 
// Data:2020-12-29                     
// Version: SsznTrans Ver 1.0.1        
// Description:  详见doc.md            
//                                     
//----------------------------------------------------------------------------- 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sszn
{
    //New
    //struct ECD
    //{
    //    unsigned int version;   //赋值 0x00000002
    //    int width;
    //    int height;
    //    double xInternel;
    //    double yInternel;
    //    char info[32];      //赋值 "SSZN2021 V00000002";
    //    int reserve[2545];
    //};

    //Old
    //struct ECD
    //{
    //    unsigned int version;   
    //    int width;
    //    int height;
    //    int reserveU;
    //    double xInternel;
    //    double yInternel;
    //    int reserve[2552];
    //};

    public struct BATCH_INFO
    {
        public uint version;
        public int width;
        public int height;
        public double xInterval;
        public double yInterval;
    };

    public struct PointCloudHead
    {
        public int _height; //点云行数
        public int _width; //点云列数
        public double _xInterval; //点云列间距
        public double _yInterval; //点云行间距
    };



    public class EcdClass
    {
        
        public static void readEcd(string file, ref Int32[] data, ref PointCloudHead pcHead)
        {
            if (!File.Exists(file))
            {
                return;
            }

            FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            BATCH_INFO head = new BATCH_INFO();
            Int32 reserveU = 0;

            head.version = br.ReadUInt32();
            head.width = br.ReadInt32();
            head.height = br.ReadInt32();
            if (head.version == 2)
            {   
                head.xInterval = Math.Round(br.ReadDouble(), 3);
                head.yInterval = Math.Round(br.ReadDouble(), 3);
                reserveU = br.ReadInt32();
            }
            else
            {
                reserveU = br.ReadInt32();  //无效位，为了数据对齐
                head.xInterval = Math.Round(br.ReadDouble(), 3);
                head.yInterval = Math.Round(br.ReadDouble(), 3);
            }         
            br.ReadBytes(2552 * 4);   //文件头共10240字节
            data = new int[head.width * head.height];
            for (int i = 0; i < head.width * head.height ; i++)
            {
                data[i] = br.ReadInt32();
            }
            pcHead._width = head.width;
            pcHead._height = head.height;
            pcHead._xInterval = head.xInterval;
            pcHead._yInterval = head.yInterval;

            Trace.WriteLine( head.version);
            Trace.WriteLine( pcHead._width );
            Trace.WriteLine( pcHead._height );
            Trace.WriteLine( pcHead._xInterval );
            Trace.WriteLine( pcHead._yInterval );

        }
        
        public static void writeEcd(string file, Int32[] data, PointCloudHead pcHead)
        {

            FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);
            BATCH_INFO head = new BATCH_INFO();

            head.version = 2;
            head.width = pcHead._width;
            head.height = pcHead._height;
            head.xInterval = pcHead._xInterval;
            head.yInterval = pcHead._yInterval;

            bw.Write(head.version);
            bw.Write(head.width);
            bw.Write(head.height);
            bw.Write(head.xInterval);
            bw.Write(head.yInterval);
 
            byte[] buff = new byte[2553 * 4];
            
            bw.Write(buff, 0, buff.Length);         
            for (int i = 0; i < data.Length; i++)
            {
                bw.Write(data[i]);
            }
            fs.Flush();
            fs.Close();
            bw.Close();

        }
        
        public static int WritePcd(string file, Int32[] batchData, PointCloudHead pcHead )
        {
            using (PinnedObject data = new PinnedObject(batchData))
            {
                IntPtr ptr = Marshal.StringToHGlobalAnsi(file);
                 savePcd(ptr, data.Pointer, pcHead);
                Marshal.FreeHGlobal(ptr);
            }
            return 0;
        }
        public static int writePly(string file, Int32[] batchData, PointCloudHead pcHead)
        {
            using (PinnedObject data = new PinnedObject(batchData))
            {
                IntPtr ptr = Marshal.StringToHGlobalAnsi(file);
                savePly(ptr, data.Pointer, pcHead);
                Marshal.FreeHGlobal(ptr);
            }
            return 0;
        }
        public static int writeTif(string file, Int32[] batchData, PointCloudHead pcHead)
        {
            using (PinnedObject data = new PinnedObject(batchData))
            {
                IntPtr ptr = Marshal.StringToHGlobalAnsi(file);
                saveTif(ptr, data.Pointer, pcHead);
                Marshal.FreeHGlobal(ptr);
            }
            return 0;
        }

        /// <summary>
        /// 保存PCD文件
        /// </summary>
        /// <param name="path"></param>            保存路径
        /// <param name="data"></param>            高度数据 int数组
        /// <param name="head"></param>            头文件图像长宽 xy间距
        /// <returns></returns>                    0：成功; 小于0：失败
        [DllImport("ImageConvert.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int savePcd(IntPtr path, IntPtr data, PointCloudHead head);

        /// <summary>
        /// 保存PCD文件
        /// </summary>
        /// <param name="path"></param>            保存路径
        /// <param name="data"></param>            高度数据 int数组
        /// <param name="head"></param>            头文件图像长宽 xy间距
        /// <returns></returns>                    0：成功; 小于0：失败
        [DllImport("ImageConvert.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int savePly(IntPtr path, IntPtr data, PointCloudHead head);

        /// <summary>
        /// 保存PCD文件
        /// </summary>
        /// <param name="path"></param>            保存路径
        /// <param name="data"></param>            高度数据 int数组
        /// <param name="head"></param>            头文件图像长宽 xy间距
        /// <returns></returns>                    0：成功; 小于0：失败
        [DllImport("ImageConvert.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int saveTif(IntPtr path, IntPtr data, PointCloudHead head);
    }
    
}



