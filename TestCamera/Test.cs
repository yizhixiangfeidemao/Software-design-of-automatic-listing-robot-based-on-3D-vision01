using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sszn;
using System.Runtime.InteropServices;
using PCL;
using System.Threading;
using SRDLLDemo;
using SelfFunction;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Diagnostics;
using S7.Net;

namespace TestCamera
{
    class Test
    {
        static Socket socketSend;           // 用于Socket通信

        static private int mBatchPoint = 0; //批处理行数
        static private int mBatchWidth = 3200; //轮廓宽度
        static private int mYscale = 0;
        static private int mXscale = 0;
        static int mBatchTimes = 0;
        static int[][] HeightData = new int[2][]; //高度数据缓存
        static byte[][] GrayData = new byte[2][]; //灰度数据缓存
        static int[][] EncoderData = new int[2][]; //编码器数据缓存
        static private bool bBatchFinish = false;       // 建立控制是否 批处理完成 的锁

        public static ErrorCode lastErrorCode;     // 全局错误处理
        public static string lastErrorString;      // 公共字符串
        public static bool IsAvailable;            // 检查PLC的可用性
        static public void BatchOneTimeCallBack(IntPtr info, IntPtr data)
        {
            string strtemp = "";
            SR7IF_STR_CALLBACK_INFO coninfo = new SR7IF_STR_CALLBACK_INFO();
            coninfo = (SR7IF_STR_CALLBACK_INFO)Marshal.PtrToStructure(info, typeof(SR7IF_STR_CALLBACK_INFO));
            if (coninfo.returnStatus == -100)
                return;
            strtemp = string.Format("传感头数量: {0}", coninfo.HeadNumber) + ("\r\n");
            strtemp = strtemp + (string.Format("当前获取批处理行数: {0}", coninfo.BatchPoints) + "\r\n");
            strtemp = strtemp + string.Format("方向数据数量: {0}", coninfo.xPoints) + "\r\n";
            strtemp = strtemp + string.Format("X方向间距: {0}", coninfo.xPixth) + "\r\n";

            if (coninfo.BatchPoints != 0)
                mBatchTimes++;
            strtemp = strtemp + string.Format("批处理次数: {0}", mBatchTimes) + "\r\n";
            Console.WriteLine(strtemp);
            mBatchPoint = coninfo.BatchPoints;
            mBatchWidth = coninfo.xPoints;
            mYscale = mBatchPoint / 560;
            mXscale = mBatchWidth / 800;
            if (mBatchPoint < 560)
                mYscale = 1;
            if (mBatchWidth < 800)
                mXscale = 1;

            IntPtr mTmpData = SR7LinkFunc.SR7IF_GetBatchProfilePoint(data, 0);
            int mNumP = (mBatchPoint * mBatchWidth);

            if (mTmpData != IntPtr.Zero)
            {
                Marshal.Copy(mTmpData, HeightData[0], 0, mNumP);
            }
            else
            {
                Console.WriteLine("内存不足,相机头A高度数据获取失败");
            }
            bBatchFinish = true;
            GC.Collect();
        }

        static void Main(string[] args)
        {

            //连接PLC，与PLC通讯。s7 - 1200,默认为rack = 0和slot = 0；外部以太网需要slot>0
            Plc plc = new Plc(CpuType.S71200, "192.168.0.2", 0, 1);   // 位于0号机架，用于硬件配置槽1的cpu,DB块号为40。
            //if ((int)ErrorCode.WrongCPU_Type == 1) Console.WriteLine("错误的CPU类型");
            plc.Open();         // 打开连接,返回void类型         
            if (ErrorCode.NoError == 0) Console.WriteLine("功能已正确执行");
            if (plc.IsConnected) Console.WriteLine("连接成功！");
            if (IsAvailable) Console.WriteLine("plc可用");

            // 打开轮廓仪通讯
            SR7IF_ETHERNET_CONFIG _ethernetConfig;
            int _currentDeviceId = 0;
            _ethernetConfig.abyIpAddress = new Byte[]
            {
                Convert.ToByte(192),
                Convert.ToByte(168),
                Convert.ToByte(0),
                Convert.ToByte(10)
            };
            // 进行连接 0号线扫
            int Rconnect = SR7LinkFunc.SR7IF_EthernetOpen(_currentDeviceId, ref _ethernetConfig);
            if (Rconnect == 0) Console.WriteLine("连接成功！");
            // 首先内存申请
            HeightData[0] = new int[15000 * 6400];
            HeightData[1] = new int[15000 * 6400];




            while (true)
            {

                for (int i = 0; i < HeightData[0].Length; i++)
                {
                    HeightData[0][i] = -1000000;
                    HeightData[1][i] = -1000000;
                }
                // 将模式更换为 2.5D模式
                try
                {
                    int PRGNo = 0;// 程序号

                    int reT25 = SR7LinkFunc.SR7IF_SwitchProgram(0, PRGNo);

                    if (reT25 != 0)
                    {
                        Console.WriteLine("程序切换失败！");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("PRG Switch Exception." + ex.Message);
                    return;
                }



                // 扫描单条轮廓线，并进行检测是否处于中心线位置
                SsznCam m_Cam = new SsznCam();
                int dev_id = 0;
                IntPtr DataObject = new IntPtr();
                int m_EncoderNum = 1; //双相机
                m_Cam.getCamBOnline(out m_EncoderNum);
                uint[] EncoderData = new uint[m_EncoderNum];
                PinnedObject pin_Encoder = new PinnedObject(EncoderData);
                int DataWidth = SR7LinkFunc.SR7IF_ProfileDataWidth(0, DataObject);
                //int[] ProfData = new int[DataWidth];
                int[] ProfData = new int[3200];         // 自己手动修改，将其改为3200行数据
                using (PinnedObject pin_Profile = new PinnedObject(ProfData))
                {
                    using (PinnedObject pin_Encoder1 = new PinnedObject(EncoderData))
                    {
                        Thread.Sleep(1000);
                        int Rc = SR7LinkFunc.SR7IF_GetSingleProfile((uint)dev_id,
                                pin_Profile.Pointer, pin_Encoder1.Pointer);

                        if (Rc != 0)
                        {
                            Console.WriteLine("轮廓获取失败,返回值：" + Rc.ToString());
                            return;
                        }
                    }
                }
                Console.WriteLine("单条轮廓仪扫描完成");
                // 将每个z值都必须除以100000，将获取的轮廓线放入list数组中
                List<double> list_profile = new List<double>();

                foreach (var i in ProfData)
                {
                    double j = i / 100000;
                    list_profile.Add(j);
                }
                Console.WriteLine(list_profile.Count());

                // 其中有-10000这种特殊点，并且一共3200个点，x间距为0.1，从[-160,159.9]。
                // 替换-10000函数
                SelfFunction.ProfileFunction fun = new ProfileFunction();
                fun.ReplaceFunction(list_profile);

                //移动函数
                double angle;
                fun.AngleFunction(list_profile, out angle);
                Console.WriteLine("转动的角度为：{0}°", angle);

                // 将模式更换为 3D模式
                try
                {
                    int PRGNo = 1;// 程序号
                    int reT12 = SR7LinkFunc.SR7IF_SwitchProgram(0, PRGNo);
                    if (reT12 != 0)
                    {
                        Console.WriteLine("程序切换失败！");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("PRG Switch Exception." + ex.Message);
                    return;
                }

                // 开始进行批处理回调
                SR7IF_BatchOneTimeCallBack batchOneTimeCallBack;
                int m_BatchWait = 0;
                batchOneTimeCallBack = new SR7IF_BatchOneTimeCallBack(BatchOneTimeCallBack);
                int reT = SR7LinkFunc.SR7IF_SetBatchOneTimeDataHandler(0, batchOneTimeCallBack);
                if (reT == 0)
                {
                    reT = SR7LinkFunc.SR7IF_StartMeasureWithCallback(0, m_BatchWait);
                    if (reT < 0)
                    {
                        Console.WriteLine("开始批处理失败");
                        return;
                    }
                }
                bBatchFinish = false;
                Stopwatch sw = new Stopwatch();         // 计算时间
                sw.Start();
                while (!bBatchFinish)
                {

                }
                while (bBatchFinish)
                {
                    Console.WriteLine("开始写入文件......");
                    // 读取完成，开始测试写入文件
                    string name = @"D:/GuaPai/PCL_test1.8.1/TestCamera/bin/Debug/Test.txt";
                    FileStream wfs = new FileStream(name, FileMode.Create, FileAccess.Write);
                    StreamWriter csvfile = new StreamWriter(wfs, Encoding.Default);
                    for (int i = 0; i < 300; i++)
                    {
                        for (int j = 0; j < 300; j++)
                        {
                            csvfile.Write(HeightData[0][j] / 100000 + " ,");
                        }
                        csvfile.Write("\n");
                    }
                    csvfile.Close();
                    wfs.Close();
                    break;
                }

                    //try
                    //{
                    //    int PRGNo = 1;// 程序号

                    //    int reT1 = SR7LinkFunc.SR7IF_SwitchProgram(0, PRGNo);

                    //    if (reT1 != 0)
                    //    {
                    //        Console.WriteLine("程序切换失败！");
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine("PRG Switch Exception." + ex.Message);
                    //    return;
                    //}



                    // 点云计算
                    try
                    {
                        float KunX_R = 8;       // 捆线半径设置为8
                        PCL_DLL.CoordinatePoint(KunX_R);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("点云坐标获取失败：" + ex.Message);
                    }
                    // 根据生成的zb.txt文件进行输出
                    Console.WriteLine("点云坐标为：");
                    string zbpath = @"D:/GuaPai/PCL_test1.8.1/TestCamera/bin/Debug/zb.txt";
                    StreamReader zbsr = new StreamReader(zbpath, Encoding.Default);
                    string content;
                    while ((content = zbsr.ReadLine()) != null)
                    {
                        char[] chs = { ' ' };
                        string[] res = content.ToString().Split(chs, options: StringSplitOptions.RemoveEmptyEntries);
                        Console.WriteLine("X = {0}", res[0]);
                        Console.WriteLine("Y = {0}", res[1]);
                        Console.WriteLine("Z = {0}", res[2]);
                }
                    zbsr.Close();
                    // 写入完成，点云结束时间计算
                    sw.Stop();
                    TimeSpan ts1 = sw.Elapsed;
                    Console.WriteLine("生成文件与点云计算总共花费 {0}ms，换算单位秒，为{1}s", ts1.TotalMilliseconds, ts1.TotalMilliseconds / 1000);

                
                // 与Python通信
                try
                {
                    //创建负责通信的Socket
                    socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    // 写入远程 服务器 的IP和端口号
                    IPAddress ip = IPAddress.Parse("192.168.0.155");
                    IPEndPoint endPoint = new IPEndPoint(ip, Convert.ToInt32("50000"));
                    //获取要远程 服务器 连接的IP和端口号
                    socketSend.Connect(endPoint);
                    Console.WriteLine("与服务端连接成功");
                    int judeg_value = 5;
                    //// 开启新线程
                    //Thread th = new Thread(() => Recive(ref judeg_value));
                    //th.IsBackground = true;
                    //th.Start();
                    //发送字段
                    string str = "121";
                    // 将发送的字段转化为字节数组
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
                    socketSend.Send(buffer);

                    // 接受数据
                    byte[] buffer2 = new byte[1024 * 1024 * 2];
                    // 实际接受到的有效字节数
                    int r = socketSend.Receive(buffer2);
                    if (r == 0)
                    {
                        break;
                    }
                    string str2 = Encoding.UTF8.GetString(buffer2, 0, r);
                    Console.WriteLine(socketSend.RemoteEndPoint + ":" + str2);
                    Console.WriteLine("我接收到的数据{0}", str2);
                    judeg_value = int.Parse(str2);

                    //Thread.Sleep(5000);
                    // 接受到信号开始判断挂牌成功与否
                    Console.WriteLine("打印Python返回的值:{0}", judeg_value);
                    if (judeg_value == 1)
                    {
                        DateTime dt = DateTime.Now;
                        string strTime = dt.ToString();
                        Console.WriteLine("时间:{0}", strTime);
                        Console.WriteLine("挂牌成功!");
                    }
                    else
                    {
                        DateTime dt = DateTime.Now;
                        string strTime = dt.ToString();
                        Console.WriteLine("时间:{0}", strTime);
                        Console.WriteLine("挂牌失败！重新挂牌！");
                        // 开始执行重新挂牌
                    }
                }
                catch (Exception)
                {
                    //throw;
                }
                

            } // while循环
            int Rt = Sszn.SR7LinkFunc.SR7IF_CommClose(0);
            if (Rt == 0)
            {
                Console.WriteLine("断开连接成功！");
            }
        } // 主函数结束==========================================================

    }
}