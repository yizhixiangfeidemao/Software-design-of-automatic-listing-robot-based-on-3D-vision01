//----------------------------------------------------------------------------- 
// <copyright file="SR7LinkFunc.cs" company="SSZN">
//	 Copyright (c) 2017 SSZN.  All rights reserved.
// </copyright>
// Author:Xiaobin Wang
// Date:2020-11-06 
// Version: SsznCam Ver 1.0.6
// Description:1.增加一次回调功能
//             2.对getParam setParam 全部可读可写参数做测试，OK
//             3.增加亮度图支持
//             **4.当前只能对配置1设置和读取参数
//----------------------------------------------------------------------------- 

using System;
using System.Collections.Generic;
using System.Text;

namespace Sszn 
{

    /*******接口函数返回值**********/
    enum SR7IF_ERROR
    {
        SR7IF_ERROR_NOT_FOUND = (-999),                  // Item is not found.
        SR7IF_ERROR_COMMAND = (-998),                  // Command not recognized.
        SR7IF_ERROR_PARAMETER = (-997),                  // Parameter is invalid.
        SR7IF_ERROR_UNIMPLEMENTED = (-996),                  // Feature not implemented.
        SR7IF_ERROR_HANDLE = (-995),                  // Handle is invalid.
        SR7IF_ERROR_MEMORY = (-994),                  // Out of memory.
        SR7IF_ERROR_TIMEOUT = (-993),                  // Action timed out.
        SR7IF_ERROR_DATABUFFER = (-992),                  // Buffer not large enough for data.
        SR7IF_ERROR_STREAM = (-991),                  // Error in stream.
        SR7IF_ERROR_CLOSED = (-990),                  // Resource is no longer avaiable.
        SR7IF_ERROR_VERSION = (-989),                  // Invalid version number.
        SR7IF_ERROR_ABORT = (-988),                  // Operation aborted.
        SR7IF_ERROR_ALREADY_EXISTS = (-987),                  // Conflicts with existing item.
        SR7IF_ERROR_FRAME_LOSS = (-986),                  // Loss of frame.
        SR7IF_ERROR_ROLL_DATA_OVERFLOW = (-985),                  // Continue mode Data overflow.
        SR7IF_ERROR_ROLL_BUSY = (-984),                  // Read Busy.
        SR7IF_ERROR_MODE = (-983),                  // Err mode.
        SR7IF_ERROR_CAMERA_NOT_ONLINE = (-982),                  // Camera not online.
        SR7IF_ERROR = (-1),                    // General error.
        SR7IF_OK = (0),                     // Operation successful.
        SR7IF_NORMAL_STOP = (-100)                //A normal stop caused by external IO or other causes

    }

    //SetSetting GetSetting
    enum SR7IF_SETTING_ITEM
    {

        TRIG_MODE = 0x0001,//触发模式
        SAMPLED_CYCLE = 0x0002,//采样周期
        BATCH_ON_OFF = 0x0003,//批处理开关
        ENCODER_TYPE = 0x0007,//编码器类型

        REFINING_POINTS = 0x0009,//细化点数
        BATCH_POINT = 0x000A,//批处理点数

        CYCLICAL_PATTERN = 0x0010,//循环模式 0 关闭 1 打开
        Z_MEASURING_RANGE = 0x0103,//Z方向测量范围

        SENSITIVITY = 0x0105,//感光灵敏度
        EXP_TIME = 0x0106,//曝光时间
        LIGHT_CONTROL = 0x010B,//光亮控制 
        LIGHT_MAX = 0x010C,//激光亮度上限
        LIGHT_MIN = 0x010D,//激光亮度下限
        PEAK_SENSITIVITY = 0x010F,//峰值灵敏度
        PEAK_SELECT = 0x0111,  //峰值选择

        X_SAMPLING = 0x0202,   //X轴压缩设定

        FILTER_X_MEDIAN = 0x020A,  //X轴中位数滤波
        FILTER_X_SMOOTH = 0x020B,  //X轴平滑滤波
        FILTER_Y_MEDIAN = 0x020C,  //Y轴中位数滤波
        FILTER_Y_SMOOTH = 0x020D,  //Y轴平滑滤波

        CHANGE_3D_25D = 0x3000,    //3D/2.5D切换 2.5模式下X轴压缩设定为 自动变更默认值.

        X_PIXEL = 0x3001,          //X数据宽度(单位像素)
        X_PITCH = 0x3002      //X Resolution

    }

    //触发模式
    enum SR7IF_TRIG_MODE
    {
        CONTINUE = 0,
        EXT_TRIGGER = 1,
        ENCODER = 2,
    }

    //采集周期
    //100 200 400 600 1000 1500 2000 2500

    //批处理开关
    enum SR7IF_BATCH_ON_OFF
    {
        OFF = 0,
        ON = 1 
    }

    //编码器类型
    enum SR7IF_ENCODER_TYPE
    {
        E_1_1 = 0,//0：1 相 1 递增；
        E_2_1 = 1,//1：2 相 1 递增；
        E_2_2 = 2,//2：2 相 2 递增；
        E_2_4 = 3 //3：2 相 4 递增；
    }

    //细化点数   1 -- 
    //批处理点数 1 -- 

    //循环模式 0 关闭 1 打开
    enum SR7IF_CYCLICAL_PATTERN
    {
        CLOSE = 0,
        OPEN = 1
    }

    //Z方向测量范围 注：只支持SR8020/SR8060
	enum  SR7IF_Z_MEASURING_RANGE 
	{
		Z840 = 0,
		Z768 = 1,
		Z512 = 2,
		Z384 = 3,
		Z256 = 4,
		Z192 = 5,
		Z128 = 6,
		Z96 = 7,
		Z64 = 8,
		Z48 = 9,
		Z32 = 10
	};
        
    //感光灵敏度
    enum SR7IF_SENSITIVITY
    {
        HIGH = 0,
        HIGH_RANGE_1 = 1,
        HIGH_RANGE_2 = 2,
        HIGH_RANGE_3 = 3,
        HIGH_RANGE_4 = 4,
        CUSTOMIZATION = 5
    }

    //曝光时间
    enum SR7IF_EXP_TIME
    {
        T10US = 0,
        T15US = 1,
        T30US = 2,
        T60US = 3,
        T120US = 4,
        T240US = 5,
        T480US = 6,
        T960US = 7,
        T1920US = 8,
        T2400US = 9,
        T4900US = 10,
        T9800US = 11,

    }
    
    //光亮控制 
    enum SR7IF_LIGHT_CONTROL
    {
        AUTO = 0,
        MAN = 1
    }

    //激光亮度上限 0-99
    //激光亮度下限 0-99
    
    //峰值灵敏度
    enum SR7IF_PEAK_SENSITIVITY
    {
        N_1 = 1,
        N_2 = 2,
        N_3 = 3,
        N_4 = 4,
        N_5 = 5
    }

    //峰值选择
    enum SR7IF_PEAK_SELECT
    {
        STANDARD = 0,
        NEAR = 1,
        FAR = 2,
        BE_NULL = 3,
        CONTINUE = 4,
        GLUE = 5

    }
    
    //X轴压缩设定 注：2.5D 模式下不能设置
	enum SR7IF_X_SAMPLING
	{
		OFF = 1,
        X2 = 2,
		X4 = 4,
		X8 = 8,
		X16 = 16
	}

    //X轴中位数滤波
    enum SR7IF_FILTER_X_MEDIAN
    {
        OFF = 1,
        N3 = 3,
        N5 = 5,
        N7 = 7,
        N9 = 9
    }

    //Y轴中位数滤波
    enum SR7IF_FILTER_Y_MEDIAN
    {
        OFF = 1,
        N3 = 3,
        N5 = 5,
        N7 = 7,
        N9 = 9
    }

    //X轴平滑滤波
    enum SR7IF_FILTER_X_SMOOTH
    {

        N1 = 1,
        N2 = 2,
        N4 = 4,
        N8 = 8,
        N16 = 16,
        N32 = 32,
        N64 = 64
        
    }

    //Y轴平滑滤波
    enum SR7IF_FILTER_Y_SMOOTH
    {

        N1 = 1,
        N2 = 2,
        N4 = 4,
        N8 = 8,
        N16 = 16,
        N32 = 32,
        N64 = 64,
        N128 = 128,
        N256 = 256

    }

    //3D/2.5D切换 2.5模式下X轴压缩设定为 自动变更默认值.
	enum SR7IF_CHANGE_3D_25D 
	{
		T3D = 0,
		T25D = 1
	}



}







