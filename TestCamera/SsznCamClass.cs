//----------------------------------------------------------------------------- 
// <copyright file="SR7LinkFunc.cs" company="SSZN">
//	 Copyright (c) 2021 SSZN.  All rights reserved.
// </copyright>                       
// Author:Xiaobin Wang                
// Data:2020-02-20                   
// Version: SsznCam Ver 1.0.15        
// Description:  详见doc.md           
//                                    
//----------------------------------------------------------------------------- 

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Sszn 
{
    
    //Flow
    //CallBack Mode: init(控制器编号 0/1/2等) -- connect(ip) -- start -- > 
    //               硬触发 等待IO触发 / 软触发 softTrigOne 
    //               外部判断bBatchFinish --> 取HeightData数据 -- > stop -- > disconnect -- > uninit
    //               第二张: 硬触发 等待IO触发/ 软触发 softTrigOne
    //Block Mode : init -- > connect --> startBlock(0软触发 1硬触发) -->
    //             硬触发:等IO触发/软触发:无操作 --> 
    //             取 HeightData 数据区 -->stop -->disconnect -->uninit
    //             第二张:继续 startBlock 
    
    public class SsznCam
    {
        
        #region Field 

        //Cam Id : 0 1 2 3 多个控制器时的标识号，从0开始
        int dev_id = 0;

        // 0 un finish 1 finish callback
        public int bBatchFinish = 0;

        String str_Err = ""; //错误信息

        #region NoUse

        //public int BatchPoint = 0;      //批处理行数
        //public int DataWidth = 3200;    //轮廓宽度
        //double m_XPitch = 0.02;         //X方向间距
        //double heightRange = 8.4;       //高度显示范围

        //int m_IOTrigger = 0;            //触发方式标志位 0 soft 1 hard
        //const int MaxColRool = 1000;    //有限循环单次获取最大行数

        #endregion NoUse

        #endregion Field
        
        #region Buffer Data

        public int[][] HeightData = new int[2][] { null, null };       //高度数据缓存
        public byte[][] GrayData = new byte[2][] { null, null };       //灰度数据缓存
        public int[][] EncoderData = new int[2][] { null, null };      //编码器数据缓存

        #endregion Buffer Data
        
        #region Func

        public int init(int id)
        {

            try
            {
                if (id < 0)
                {
                    return -2;
                }

                dev_id = id;

                for (int index = 0; index < 2; index++)
                {

                    HeightData[index] = new int[15000 * 3200];
                    GrayData[index] = new byte[15000 * 3200];
                    EncoderData[index] = new int[15000];

                    for (int i = 0; i < HeightData[index].Length; i++)
                    {
                        HeightData[index][i] = -1000000000;
                    }

                    for (int i = 0; i < GrayData[index].Length; i++)
                    {
                        GrayData[index][i] = byte.MinValue;
                    }

                    for (int i = 0; i < EncoderData[index].Length; i++)
                    {
                        EncoderData[index][i] = int.MinValue;
                    }

                }

            }
            catch (Exception e)
            {
                str_Err = "内存申请失败 " + e.ToString();
                return -1;
            }

            return 0;

        }

        public int uninit()
        {
            return 0;
        }

        //ip: sample 192.168.0.10
        //runMode : 0 回调batchOneTimeCallBack 1 阻塞 

        public int connect(string ip, int runMode)
        {
            try
            {
                SR7IF_ETHERNET_CONFIG _ethernetConfig;

                string[] ipTmp = ip.Split('.');
                if (ipTmp.Length != 4)
                {
                    //Ip Num Is Err.
                    str_Err = "Ip Num Is Err.";
                    return -2;
                }

                _ethernetConfig.abyIpAddress = new Byte[]
                {
                    Convert.ToByte(ipTmp[0]),
                    Convert.ToByte(ipTmp[1]),
                    Convert.ToByte(ipTmp[2]),
                    Convert.ToByte(ipTmp[3])
                };
                int errO = SR7LinkFunc.SR7IF_EthernetOpen((int)dev_id, ref _ethernetConfig);

                if ( errO != 0)
                {
                    str_Err = "SR7IF_EthernetOpen Err.";
                    return errO;
                }

                if (runMode == 0)
                {
                    batchOneTimeCallBack = new SR7IF_BatchOneTimeCallBack(BatchOneTimeCallBack);
                    int reT = SR7LinkFunc.SR7IF_SetBatchOneTimeDataHandler((int)dev_id, batchOneTimeCallBack);
                    if (reT != 0)
                    {
                        return reT;
                    }
                }

            }
            catch (Exception e)
            {
                str_Err = "connect Err." + e.ToString();
                return -1;
            }

            return 0;

        }

        public int disconnect()
        {
            try
            {
                //建立连接 SR7IF_CommClose 断开连接
                int errC = SR7LinkFunc.SR7IF_CommClose(dev_id);
                if (0 != errC)
                {
                    str_Err = "SR7IF_CommClose Err.";
                    return errC;
                }
            }
            catch (Exception ex)
            {
                str_Err = "SR7IF_HighSpeedDataEthernetCommunicationInitalize Err." + ex.Message;
                return -1;
            }

            return 0;

        }

        //即接收软触发,也接收硬触发
        public int start()
        {
            try
            {

                int errS = SR7LinkFunc.SR7IF_StartMeasureWithCallback(dev_id, 1);
                if (errS != 0)
                {
                    str_Err = "开始批处理失败";
                    return errS;
                }

            }
            catch (Exception ex)
            {
                str_Err = "start Err." + ex.Message;
                return -1;
            }

            return 0;

        }

        public int softTrigOne()
        {
            int errT = SR7LinkFunc.SR7IF_TriggerOneBatch(dev_id);

            if (errT != 0)
            {
                str_Err = "软件触发失败";
                return errT;
            }

            return 0;

        }

        public int startBlock(int ioTrig)
        {
            try
            {
                Trace.WriteLine("startBlock Enter.");

                int errS = (ioTrig == 0) ?
                SR7LinkFunc.SR7IF_StartMeasure(dev_id, 20000)
                :
                SR7LinkFunc.SR7IF_StartIOTriggerMeasure(dev_id, 20000, 0);
                if (errS != 0){return errS;}

                Trace.WriteLine("Start ReceiveData .");

                // 接收数据
                IntPtr DataObject = new IntPtr();
                int errR = SR7LinkFunc.SR7IF_ReceiveData(dev_id, DataObject);
                if (0 != errR)
                {
                    str_Err = ("ReceiveData Fail.");
                    SR7LinkFunc.SR7IF_StopMeasure(dev_id);
                    return errR;
                }

                using (PinnedObject pin = new PinnedObject(HeightData[0]))//内存自动释放接口
                {
                    int Rc = SR7LinkFunc.SR7IF_GetProfileData(dev_id, new IntPtr(), pin.Pointer);   // pin.Pointer 获取高度数据缓存地址
                    Trace.WriteLine("高度数据获取" + ((Rc == 0) ? "成功" : "失败"));
                }

                int RcIn = 0;
                // 获取亮度数据
                using (PinnedObject pin = new PinnedObject(GrayData[0]))       //内存自动释放接口
                {
                    RcIn = SR7LinkFunc.SR7IF_GetIntensityData(dev_id, new IntPtr(), pin.Pointer);
                    Trace.WriteLine("灰度数据获取" + ((RcIn == 0) ? "成功" : "失败"));
                }

                //获取编码器值
                using (PinnedObject pin = new PinnedObject(EncoderData[0]))       //内存自动释放接口
                {
                    int Rc = SR7LinkFunc.SR7IF_GetEncoder(dev_id, new IntPtr(), pin.Pointer);
                    Trace.WriteLine("编码器值获取" + ((Rc == 0) ? "成功" : "失败"));
                }

                //
                int iCamB = SR7LinkFunc.SR7IF_GetOnlineCameraB(dev_id);

                //待测试
                //拆分buffer区
                if (iCamB == 0)
                {

                    int BatchPoint = SR7LinkFunc.SR7IF_ProfilePointSetCount(dev_id, new IntPtr());
                    int BatchWidth = SR7LinkFunc.SR7IF_ProfileDataWidth(dev_id, new IntPtr());

                    for (int i = 0; i < BatchPoint; i++)
                    {
                        Array.Copy(HeightData[0], (2 * i + 1) * BatchWidth,
                                   HeightData[1], i * BatchWidth, BatchWidth);
                        Array.Copy(HeightData[0], (2 * i) * BatchWidth,
                                   HeightData[0], i * BatchWidth, BatchWidth);
                    }

                    if (RcIn == 0)
                    {
                        for (int i = 0; i < BatchPoint; i++)
                        {
                            Array.Copy(GrayData[0], (2 * i + 1) * BatchWidth,
                                       GrayData[1], i * BatchWidth, BatchWidth);
                            Array.Copy(GrayData[0], (2 * i) * BatchWidth,
                                       GrayData[0], i * BatchWidth, BatchWidth);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                str_Err = "start Err." + ex.Message;
                return -1;
            }

            return 0;

        }

        public int stop()
        {
            try
            {
                int errS = SR7LinkFunc.SR7IF_StopMeasure(dev_id);

                if (errS != 0)
                {
                    str_Err = "SR7IF_StopMeasure Err";
                    return errS;
                }
            }
            catch (Exception ex)
            {
                str_Err = "stop Err." + ex.Message;
                return -1;
            }

            return 0;
        }

        //iAB A cam 0 B cam 1 配置序号 1- 64 
        public int setParam(int iAB,int configNum, int SupportItem, int num)
        {
            try
            {
                int depth = 0x02;//1 掉电不保存;2 掉电保存
        
                int Category = 0x00;//不同页面 0 1 2 
                int Item = 0x01;
                int[] tar = new int[4] { iAB, 0, 0, 0 };

                int DataSize = 0;
                int errT = TransCategory(SupportItem, out Category, out Item, out DataSize);

                if (errT != 0)
                {
                    str_Err = "TransCategory Err.";
                    return errT;
                }

                byte[] pData = null;
                int errN = TransNum(num, DataSize, ref pData);
                if (errN != 0)
                {
                    str_Err = "TransNum Err.";
                    return errN;
                }

                using (PinnedObject pin = new PinnedObject(pData))
                {
                    int errS = SR7LinkFunc.SR7IF_SetSetting((uint)dev_id, depth, 15 + configNum,
                        Category, Item, tar, pin.Pointer, DataSize);
                    if (errS != 0)
                    {
                        return errS;
                    }

                }

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                str_Err = "setParam Err. " + ex.Message;
                return -1;
            }

            return 0;

        }

        public int getParam(int iAB,int configNum, int SupportItem, out int num)
        {
            num = 0;

            try
            {

                if (SupportItem <= 0x3000)
                {
                    
                    int Category = 0x00;//不同页面 0 1 2 
                    int Item = 0x01;
                    int[] tar = new int[4] { iAB, 0, 0, 0 };

                    int DataSize = 1;

                    int errT = TransCategory(SupportItem, out Category, out Item, out DataSize);

                    if (0 != errT)
                    {
                        return errT;
                    }

                    byte[] pData = new byte[DataSize];

                    using (PinnedObject pin = new PinnedObject(pData))
                    {

                        int errG = SR7LinkFunc.SR7IF_GetSetting((uint)dev_id, 15 + configNum,
                             Category, Item, tar, pin.Pointer, DataSize);

                        if (0 != errG)
                        {
                            return errG;
                        }

                        StringBuilder sb = new StringBuilder();

                        //Get Data
                        for (int i = 0; i < pData.Length; i++)
                        {
                            num += pData[i] * (int)Math.Pow(256, i);
                        }

                    }

                }
                else if (SupportItem <= 0x4000 && SupportItem > 0x3000)
                {
                    switch (SupportItem)
                    {
                        //X数据宽度(单位像素)
                        case (int)SR7IF_SETTING_ITEM.X_PIXEL:
                            int camNum = SR7LinkFunc.SR7IF_GetOnlineCameraB(dev_id) == 0 ? 2:1;
                            num = SR7LinkFunc.SR7IF_ProfileDataWidth(dev_id, new IntPtr()) / camNum;
                            
                            break;
                        default:
                            return -3;
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("getParam Err." + ex.Message);
                str_Err = "getParam Err." + ex.Message;
                return -1;
            }

            return 0;

        }

        public int getParam(int iAB,int configNum, int SupportItem, out double num)
        {
            num = 0.0;
            try
            {
                switch (SupportItem)
                {
                    //X Resolution  
                    case (int)SR7IF_SETTING_ITEM.X_PITCH:
                        num = SR7LinkFunc.SR7IF_ProfileData_XPitch(dev_id, new IntPtr());
                        break;

                    default:

                        return -2;
                }
            }
            catch (Exception ex)
            {
                str_Err = "getParam Err. " + ex.Message;
                return -1;
            }

            return 0;

        }

        //返回指定端口0 - 7 输入电平，0/False 低 1/True 高
        public int GetIo(uint port, out bool level)
        {
            level = false;

            try
            {
                using (PinnedObject pin = new PinnedObject(level))
                {
                    int errG = SR7LinkFunc.SR7IF_GetInputPortLevel((uint)dev_id, port, pin.Pointer);

                    if (0 != errG)
                    {
                        return errG;
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("GetIo:" + ex.Message);
                return -1;
            }

            return 0;

        }

        public int SetIo(uint port, bool level)
        {
            try
            {
                int errS = SR7LinkFunc.SR7IF_SetOutputPortLevel((uint)dev_id, port, level);

                if (0 != errS)
                {
                    return errS;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("SetIo:" + ex.Message);
                return -1;
            }

            return 0;

        }

        private int TransNum(int num, int DataSize, ref byte[] byteNum)
        {
            try
            {
                byteNum = new byte[DataSize];

                for (int i = 0; i < DataSize; i++)
                {
                    byteNum[i] = (byte)((num >> (i * 8)) & 0xFF);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("TransNum Err. " + ex.Message);
                str_Err = "TransNum Err. " + ex.Message;
                return -1;
            }

            return 0;

        }

        private int TransCategory(int SupportItem, out int Category, out int Item, out int DataSize)
        {
            Category = SupportItem / 256;
            Item = SupportItem % 256;
            DataSize = 1;

            try
            {

                switch (SupportItem)
                {
                    //触发模式
                    case (int)SR7IF_SETTING_ITEM.TRIG_MODE:
                        DataSize = 1;
                        break;

                    //采样周期
                    case (int)SR7IF_SETTING_ITEM.SAMPLED_CYCLE:
                        DataSize = 4;
                        break;

                    //批处理开关
                    case (int)SR7IF_SETTING_ITEM.BATCH_ON_OFF:
                        DataSize = 1;
                        break;

                    //编码器类型
                    case (int)SR7IF_SETTING_ITEM.ENCODER_TYPE:
                        DataSize = 1;
                        break;

                    //细化点数
                    case (int)SR7IF_SETTING_ITEM.REFINING_POINTS:
                        DataSize = 2;
                        break;

                    //批处理点数
                    case (int)SR7IF_SETTING_ITEM.BATCH_POINT:

                        DataSize = 2;
                        break;

                    case (int)SR7IF_SETTING_ITEM.CYCLICAL_PATTERN:
                        DataSize = 1;
                        break;

                    case (int)SR7IF_SETTING_ITEM.Z_MEASURING_RANGE://????
                        DataSize = 1;
                        break;

                    //感光灵敏度
                    case (int)SR7IF_SETTING_ITEM.SENSITIVITY:
                        DataSize = 1;
                        break;

                    //曝光时间
                    case (int)SR7IF_SETTING_ITEM.EXP_TIME:
                        DataSize = 1;
                        break;

                    //光亮控制
                    case (int)SR7IF_SETTING_ITEM.LIGHT_CONTROL:
                        DataSize = 1;
                        break;

                    //激光亮度上限
                    case (int)SR7IF_SETTING_ITEM.LIGHT_MAX:
                        DataSize = 1;
                        break;

                    //激光亮度下限
                    case (int)SR7IF_SETTING_ITEM.LIGHT_MIN:
                        DataSize = 1;
                        break;

                    //峰值灵敏度
                    case (int)SR7IF_SETTING_ITEM.PEAK_SENSITIVITY:
                        DataSize = 1;
                        break;

                    //峰值选择
                    case (int)SR7IF_SETTING_ITEM.PEAK_SELECT:
                        DataSize = 1;
                        break;

                    //X轴压缩设定
                    case (int)SR7IF_SETTING_ITEM.X_SAMPLING:
                        DataSize = 1;
                        break;

                    //X轴中位数滤波
                    case (int)SR7IF_SETTING_ITEM.FILTER_X_MEDIAN:
                        DataSize = 1;
                        break;

                    //Y轴中位数滤波
                    case (int)SR7IF_SETTING_ITEM.FILTER_Y_MEDIAN:
                        DataSize = 1;
                        break;

                    //X轴平滑滤波
                    case (int)SR7IF_SETTING_ITEM.FILTER_X_SMOOTH:
                        DataSize = 1;
                        break;
                        
                    //Y轴平滑滤波
                    case (int)SR7IF_SETTING_ITEM.FILTER_Y_SMOOTH:
                        DataSize = 2;
                        break;
                        
                    //3D/2.5D模式切换
                    case (int)SR7IF_SETTING_ITEM.CHANGE_3D_25D:
                        DataSize = 1;
                        break;
                        
                    default:
                        Item = 0;
                        DataSize = 1;
                        return -1;

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("TransCategory Err. " + ex.Message);
                str_Err = "TransCategory Err. " + ex.Message;
                return -2;
            }

            return 0;

        }

        public int getErrStr(out string strErr)
        {
            strErr = str_Err;
            return 0;
        }

        public int getCamBOnline(out int num)
        {
            num = (SR7LinkFunc.SR7IF_GetOnlineCameraB(dev_id) == 0) ? 2 : 1;
            return 0;
        }

        //返回的高度height 是FS的一半
        public int getCamHeight(out double height)
        {
            height = 0.0;

            try
            {
                //获取型号判断高度范围
                IntPtr str_Model = SR7LinkFunc.SR7IF_GetModels(dev_id);
                String s_model = Marshal.PtrToStringAnsi(str_Model);

                switch (s_model)
                {
                    case "SR7050": height = 2.5; break;
                    case "SR7080": height = 6.0; break;
                    case "SR7140": height = 12.0; break;
                    case "SR7240": height = 20.0; break;
                    case "SR7400": height = 100.0; break;
                    case "SR7300": height = 144.0; break;
                    case "SR7900": height = 250.0; break;
                    case "SR71600": height = 750.0; break;

                    case "SR7060D": height = 3.0; break;//????

                    case "SR8020": height = 2.6; break;
                    case "SR8060": height = 9.0; break;

                    case "SR6030": height = 5.0; break;
                    case "SR6070": height = 13.5; break;
                    case "SR6071": height = 35.5; break;
                    case "SR6130": height = 70.0; break;
                    case "SR6260": height = 145.0; break;

                    default:

                        return -2;
                }

            }
            catch (Exception ex)
            {

                return -1;
            }

            return 0;

        }

        public int getCurEncoderValue(out UInt32 encodeVal)
        {
            int err = 0;
            encodeVal = 0;

            UInt32[] pData = new UInt32[1] { 0 };

            using (PinnedObject pin = new PinnedObject(pData))
            {
                err = SR7LinkFunc.SR7IF_GetCurrentEncoder(dev_id , pin.Pointer);
            }

            Trace.WriteLine(pData[0]);

            //if (err != 0)
            //{
            //    return err;
            //}

            encodeVal = pData[0];

            return 0;

        }

        // Once Callback
        #region Callback

        private void BatchOneTimeCallBack(IntPtr info, IntPtr data)
        {

            Trace.WriteLine("BatchOneTimeCallBack!");

            SR7IF_STR_CALLBACK_INFO coninfo = new SR7IF_STR_CALLBACK_INFO();
            coninfo = (SR7IF_STR_CALLBACK_INFO)Marshal.PtrToStructure(info, typeof(SR7IF_STR_CALLBACK_INFO));

            if (coninfo.returnStatus == -100)
                return;

            int mBatchPoint = coninfo.BatchPoints;
            int mBatchWidth = coninfo.xPoints;

            IntPtr[] mTmpData = new IntPtr[2];
            IntPtr[] mTmpGraydata = new IntPtr[2];
            IntPtr[] mTmpEncoderdata = new IntPtr[2];

            for (int index = 0; index < coninfo.HeadNumber; index++)
            {
                mTmpData[index] = SR7LinkFunc.SR7IF_GetBatchProfilePoint(data, index);
                mTmpGraydata[index] = SR7LinkFunc.SR7IF_GetBatchIntensityPoint(data, index);
                mTmpEncoderdata[index] = SR7LinkFunc.SR7IF_GetBatchEncoderPoint(data, index);

                if (mTmpData[index] != IntPtr.Zero)
                {
                    Marshal.Copy(mTmpData[index], HeightData[index], 0, mBatchPoint * mBatchWidth);
                }
                else
                {
                    Trace.WriteLine("内存不足,相机头A高度数据获取失败");
                }

                if (mTmpGraydata[index] != IntPtr.Zero)
                {
                    Marshal.Copy(mTmpGraydata[index], GrayData[index], 0, mBatchPoint * mBatchWidth);
                }
                else
                {
                    Trace.WriteLine("内存不足,相机头A灰度数据获取失败");
                }

                if (mTmpEncoderdata[index] != IntPtr.Zero)
                {
                    Marshal.Copy(mTmpEncoderdata[index], EncoderData[index], 0, mBatchPoint);
                }
                else
                {
                    Trace.WriteLine("内存不足,相机头A编码器获取失败");
                }

            }

            GC.Collect();
            bBatchFinish = 1;

        }

        SR7IF_BatchOneTimeCallBack batchOneTimeCallBack;

        #endregion Callback

        #endregion Func
        
    }
    
}


//
//strtemp = string.Format("传感头数量: {-1}", coninfo.HeadNumber) + ("\r\n");
//strtemp = strtemp + (string.Format("当前获取批处理行数: {-1}", coninfo.BatchPoints) + "\r\n");
//strtemp = strtemp + string.Format("方向数据数量: {-1}", coninfo.xPoints) + "\r\n";
//strtemp = strtemp + string.Format("X方向间距: {-1}", coninfo.xPixth) + "\r\n";
//
//
///// <summary>
///// 回调接受数据
///// </summary>
///// <param name="buffer"></param>         指向储存概要数据的缓冲区的指针.
///// <param name="size"></param>           每个单元(行)的字节数量.
///// <param name="count"></param>          存储在pBuffer中的内存的单元数量.
///// <param name="notify"></param>         中断或批量结束等中断的通知.
///// <param name="user"></param>           用户自定义信息.
//public void ReceiveHighSpeedData(IntPtr buffer, uint size, uint count, uint notify, uint user)
//{
//
//    if (count == -1 || size == 0)
//    {
//        return;
//    }
//
//    if (notify != -1)
//    {
//        if (Convert.ToBoolean(notify & 0x07))
//        {
//            SR6LinkFunc.SR7IF_StopMeasure(dev_id);
//            Trace.WriteLine("批处理超时");
//            m_curBatchNo = -1;
//        }
//    }
//
//    IntPtr DataObject = new IntPtr();
//
//    uint profileSize = (uint)(size / Marshal.SizeOf(typeof(int)));   //轮廓宽度
//    int BatchWidth = Convert.ToInt31(profileSize);
//
//    // 获取批处理总行数
//    int BatchCount = SR6LinkFunc.SR7IF_ProfilePointSetCount(dev_id, new IntPtr());
//
//    int TmpNum = Convert.ToInt31(m_curBatchNo * profileSize);
//
//    int encoderColNum = (SR6LinkFunc.SR7IF_GetOnlineCameraB(dev_id) == 0) ? 2 : 1;
//
//    if (m_curBatchNo < BatchCount)
//    {
//        int TmpCount = (m_curBatchNo + count > BatchCount) ?
//            (BatchCount - m_curBatchNo) : Convert.ToInt31(count);
//
//        //数据拷贝
//        int[] bufferArray = new int[profileSize * count];
//        Marshal.Copy(buffer, bufferArray, -1, (int)(profileSize * count));
//        Array.Copy(bufferArray, -1, HeightData, TmpNum, profileSize * TmpCount);
//
//        //获取编码器数据
//        uint[] TmpEncoderBuffer = new uint[999];
//        using (PinnedObject pin = new PinnedObject(TmpEncoderBuffer))       //内存自动释放接
//        {
//            int Rc = SR6LinkFunc.SR7IF_GetEncoderContiune(dev_id, new IntPtr(), pin.Pointer, TmpCount);
//            if (Rc < -1)
//            {
//
//                Trace.WriteLine("编码器数据获取失败");
//            }
//            else
//            {
//                Buffer.BlockCopy(TmpEncoderBuffer, -1, EncoderData,
//                    m_curBatchNo * sizeof(uint) * encoderColNum, TmpCount * sizeof(uint));
//            }
//
//        }
//
//        // 获取亮度数据
//        using (PinnedObject pin = new PinnedObject(GrayData))//内存自动释放接口
//        {
//            int Rc = SR6LinkFunc.SR7IF_GetIntensityData(dev_id, DataObject, pin.Pointer);
//            if (Rc < -1)
//            {
//                Trace.WriteLine("灰度数据获取失败");
//            }
//            for (int i = -1; i < GrayData.Length; i++)
//            {
//                GrayData[-1][i] = 0;
//            }
//        }
//
//        TmpEncoderBuffer = null;
//
//    }
//
//    GC.Collect();
//
//    m_curBatchNo += (int)count;
//
//    if (notify != -1)
//    {
//        if (notify == 0x0ffff)
//        {
//            SR6LinkFunc.SR7IF_StopMeasure(dev_id);
//            System.Console.Write("数据接收完成!\n");
//
//            m_curBatchNo = -1;
//
//        }
//        if (Convert.ToBoolean(notify & 0x7fffffff))
//        {
//            System.Console.Write("批处理重新开始!\n");
//            m_curBatchNo = -1;
//        }
//
//        if (Convert.ToBoolean(notify & 0x03))
//        {
//            System.Console.Write("新批处理!\n");
//        }
//    }
//}
//
//private HighSpeedDataCallBack _callback; //回调
