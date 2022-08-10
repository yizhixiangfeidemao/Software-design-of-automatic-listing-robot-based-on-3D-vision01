using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S7.Net;

namespace PLC_Test
{
    class Program
    {

        public static ErrorCode lastErrorCode;     // 全局错误处理
        public static string lastErrorString;      // 公共字符串
        public static bool IsAvailable;            // 检查PLC的可用性



        static void Main(string[] args)
        {
            //连接PLC，与PLC通讯。s7 - 1200,默认为rack = 0和slot = 0；外部以太网需要slot>0
            Plc plc = new Plc(CpuType.S71200, "192.168.0.2", 0, 1);   // 位于0号机架，用于硬件配置槽1的cpu,DB块号为40。
            //if ((int)ErrorCode.WrongCPU_Type == 1) Console.WriteLine("错误的CPU类型");
            plc.Open();         // 打开连接,返回void类型         
            if (ErrorCode.NoError == 0) Console.WriteLine("功能已正确执行");
            if (plc.IsConnected) Console.WriteLine("连接成功！");
            if (IsAvailable) Console.WriteLine("plc可用");

            // 连接激光轮廓仪，与激光轮廓仪通讯


            while (true)
            {
                // 将程序号改为0，提前进入单轮廓扫描模式。

                /*
                // 判定机器人是否到达位置,如果没有则一直循环读取
                bool robot_Reach_Location = false;
                while (!robot_Reach_Location)
                {
                    robot_Reach_Location = (bool)plc.Read("DB40.DBX0.0");        // 读取  到达位置信号
                }

                // 到达位置，开启激光轮廓仪,扫描单条轮廓数据。该行写激光轮廓仪控制程序

                
                // 机器人是否需要调整角度
                bool robot_Adjust_Angle_TF = false;
                // 利用获取的单条激光轮廓仪数据进行计算。判断是否有峰值，如果有峰值，则不调整角度，否则调整角度。写入计算调整角度
                // 根据计算判断是否需要false
                List<double> list_profile = new List<double>();
                int index_Angle = 0;          // 记录 峰值索引
                for (int i = 1; i < list_profile.Count()-1; i++)
                {
                    if (list_profile[i]>list_profile[i-1]&&list_profile[i]<list_profile[i+1])
                    {
                        index_Angle = i;
                        break;
                    }             
                }
                // 判断，将轮廓线分为4段，如果峰值索引在 第一段和第四段范围内，则进行调整。
                if (index_Angle < 0.25 * list_profile.Count() || index_Angle > 0.75 * list_profile.Count()) robot_Adjust_Angle_TF = true;
                
                // 调整角度为true,开始计算调整角度
                if (robot_Adjust_Angle_TF)      // 如果需要调整角度，那么发送调整角度大小
                {
                    // 发送 机器人调整角度大小
                    float robot_Adjust_Angle = (float)(((list_profile.Count() / 2 - index_Angle) / list_profile[index_Angle])* 180 / Math.PI);

                    plc.Write("DB40.DBX0.1", robot_Adjust_Angle_TF);      // 发送 调整角度 True
                    plc.Write("DB40.DBD2.0", robot_Adjust_Angle.ConvertToUInt());  // 机器人调整角度大小

                    // 接收角度调整完毕,如果完毕则跳出循环，否则一直读取
                    bool robot_Adjust_Angle_OK = false;
                    while (!robot_Adjust_Angle_OK)
                    {
                        robot_Adjust_Angle_OK = (bool)plc.Read("DB40.DBX6.0");     // 接收角度调整完毕
                    }
                }
                

                // 调整完毕，同时发送 控制线激光轮廓仪扫描命令  和  机器人直线运动命令
                plc.Write("DB40.DBX6.1", true);          // 发送直线运动命令
                
                */

                // 发送，激光轮廓仪开始扫描命令


                // 测得坐标值，进行计算


                // 坐标值为,y值需要通过读取当前机器人坐标获取.且此时y值传给第七轴。且需要移动一个108，减去108。如果是另一个夹爪则是+45
                float Camera_x1 = -20.3f;
                float Camera_z1 = 35.94f;
                float robot_x1 = Camera_x1 * (-3.6248f) + Camera_z1 * (6.4144f) + 0.1772f;
                float robot_y1 = -646.172f;
                float robot_z1 = Camera_x1 * (-1.3376f) + Camera_z1 * 2.3628f + 0.0635f;

                // 补挂点
                float Camera_x2 = -12.0f;
                float Camera_z2 = 28.88f;
                float robot_x2 = Camera_x2 * 3.2397f + Camera_z2 * (-7.7869f) + (-0.2675f);
                float robot_y2 = -505.082f;
                float robot_z2 = Camera_x2 * (-0.7076f) + Camera_z2 * 1.7206f + 0.0633f;

                // 发送挂牌坐标点,一共发送3个坐标点  
                float robot_Axis_One_X = robot_x1+20;               // 发送 给机器人第1个坐标的X
                plc.Write("DB40.DBD8.0", robot_Axis_One_X.ConvertToUInt());
                float robot_Axis_One_Y = robot_y1;               // 发送 给机器人第1个坐标的Y  - 186.1833;该y值为读取当前机器人y的坐标值
                plc.Write("DB40.DBD12.0", robot_Axis_One_Y.ConvertToUInt());
                float robot_Axis_One_Z = robot_z1+50;               // 发送 给机器人第1个坐标的Z
                plc.Write("DB40.DBD16.0", robot_Axis_One_Z.ConvertToUInt());
                float robot_Axis_Two_X = robot_x1;              // 发送 给机器人第2个坐标的X
                plc.Write("DB40.DBD20.0", robot_Axis_Two_X.ConvertToUInt());
                float robot_Axis_Two_Y = robot_y1;               // 发送 给机器人第2个坐标的Y
                plc.Write("DB40.DBD24.0", robot_Axis_Two_Y.ConvertToUInt());
                float robot_Axis_Two_Z = robot_z1;               // 发送 给机器人第2个坐标的Z
                plc.Write("DB40.DBD28.0", robot_Axis_Two_Z.ConvertToUInt());
                float robot_Axis_Three_X = robot_x1-40;               // 发送 给机器人第3个坐标的X
                plc.Write("DB40.DBD32.0", robot_Axis_Three_X.ConvertToUInt());
                float robot_Axis_Three_Y = robot_y1;               // 发送 给机器人第3个坐标的Y
                plc.Write("DB40.DBD36.0", robot_Axis_Three_Y.ConvertToUInt());
                float robot_Axis_Three_Z = robot_z1-50;               // 发送 给机器人第3个坐标的Z
                plc.Write("DB40.DBD40.0", robot_Axis_Three_Z.ConvertToUInt());

                // 补充 挂牌点
                float robot_Axis_Patch_One_X = robot_x2 - 20;               // 发送 给机器人第1个坐标的X,反向
                plc.Write("DB40.DBD44.0", robot_Axis_Patch_One_X.ConvertToUInt());
                float robot_Axis_Patch_One_Y = robot_y2;               // 发送 给机器人第1个坐标的Y
                plc.Write("DB40.DBD48.0", robot_Axis_Patch_One_Y.ConvertToUInt());
                float robot_Axis_Patch_One_Z = robot_z2 + 50;               // 发送 给机器人第1个坐标的Z
                plc.Write("DB40.DBD52.0", robot_Axis_Patch_One_Z.ConvertToUInt());
                float robot_Axis_Patch_Two_X = robot_x2;               // 发送 给机器人第2个坐标的X
                plc.Write("DB40.DBD56.0", robot_Axis_Patch_Two_X.ConvertToUInt());
                float robot_Axis_Patch_Two_Y = robot_y2;               // 发送 给机器人第2个坐标的Y
                plc.Write("DB40.DBD60.0", robot_Axis_Patch_Two_Y.ConvertToUInt());
                float robot_Axis_Patch_Two_Z = robot_z2;               // 发送 给机器人第2个坐标的Z
                plc.Write("DB40.DBD64.0", robot_Axis_Patch_Two_Z.ConvertToUInt());
                float robot_Axis_Patch_Three_X = robot_x2+40;                // 发送 给机器人第3个坐标的X
                plc.Write("DB40.DBD68.0", robot_Axis_Patch_Three_X.ConvertToUInt());
                float robot_Axis_Patch_Three_Y = robot_y2;               // 发送 给机器人第3个坐标的Y
                plc.Write("DB40.DBD72.0", robot_Axis_Patch_Three_Y.ConvertToUInt());
                float robot_Axis_Patch_Three_Z = robot_z2 - 50;               // 发送 给机器人第3个坐标的Z
                plc.Write("DB40.DBD76.0", robot_Axis_Patch_Three_Z.ConvertToUInt());


                // 读取->机器人挂牌完毕信号 , 没有读取到就要反复读取
                bool robot_Work_OK = false;
                while (!robot_Work_OK)
                {
                    robot_Work_OK = (bool)plc.Read("DB40.DBX412.0");        // 读取  到达位置信号
                }

                // 读取-> 机器人到达拍照位置信号
                bool robot_Reach_2D_Cr = false;
                while (!robot_Work_OK)
                {
                    robot_Reach_2D_Cr = (bool)plc.Read("DB40.DBX412.1");        // 读取  到达位置信号
                }

                // 读取-> 机器人光源亮灭
                bool robot_Light = false;
                while (!robot_Light)
                {
                    robot_Light = (bool)plc.Read("DB40.DBX412.2");        // 读取  机器人光源亮灭
                }

                // 灯亮了，开始拍照
                Console.Read();

                // 发送挂牌成功
                plc.Write("DB40.DBX412.3", true);          // 发送直线运动命令
            }   // 大循环结束
            
            plc.Close();            // 关闭连接

        }
    }
}
