using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfFunction
{

    class ProfileFunction
    {
        // 替换算法：替换-10000的点
        public void ReplaceFunction(List<double> list)
        {
            // 如果初始位置有-10000，必须用后面的替换
            int temp_i = 0;
            while (list[temp_i] == -10000)
            {
                temp_i++;
            }
            for (int j = 0; j < temp_i; j++)
            {
                list[j] = list[temp_i];
            }
            // 如果前面没有-10000，则用前面的将-10000进行替换
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i] == -10000)
                {
                    list[i] = list[i - 1];
                }
            }

        }

        // 算法1：获取单条轮廓后，对单条轮廓进行判断峰值以及计算出需要移动的距离
        public void MoveFunction(List<double> list,out double moveNumber)
        {
            moveNumber = 0;//需要移动的距离值
            double MaxInter = 8; //最大间隙值
            int TimeIndex = 0; //最大间隙值的索引
            for (int i = 0; i < list.Count()-1; i++)
            {
                double TimeMax = list[i + 1] - list[i];

                if (TimeMax>MaxInter)
                {
                    MaxInter = TimeMax;
                    TimeIndex = i+1;
                }
            }

            // 寻找峰值，根据最大间隙处，进行查找
            int TimeIndexMin = 0;//最大间隙值的相反数索引
            for (int i = TimeIndex; i < list.Count()-1; i++)
            {
                double MinInter = -8;//最大间隙值相反数，即当从峰值减小时
                double TimeMin = list[i + 1] - list[i];
                if (TimeMin<MinInter)
                {
                    TimeIndexMin = i+1;//记录索引
                    break;
                }
            }
            //查找峰值
            double MaxZ = list[TimeIndex]; //峰值
            int MaxZindex = TimeIndex;//峰值索引
            for (int i = TimeIndex; i < TimeIndexMin; i++)
            {
                if (list[i+1]>list[i])
                {
                    MaxZ = list[i + 1];
                    MaxZindex = i + 1;
                }
            }

            //Console.WriteLine("峰值下降处的索引：{0},{1}", TimeIndexMin, list[TimeIndexMin]);
            int Index = list.Count()/2;//索引的中间值
            //Console.WriteLine("峰值{0},峰值索引{1}，原点索引{2},原点值{3}", MaxZ, MaxZindex, Index,list[Index]);
            if (MaxZindex < Index)
            {
                moveNumber = -(Math.Abs(MaxZindex - Index) * 0.1);//x的间隙值为0.1
            }
            else if (MaxZindex > Index)
            {
                moveNumber = Math.Abs(MaxZindex - Index) * 0.1;
            }
            else
            {
                moveNumber = 0;
            }
            //Console.WriteLine("移动的距离为{0}:",moveNumber);
        }

        // 算法2：获取单条轮廓后，对单条轮廓进行判断峰值以及计算出需要转动的角度
        public void AngleFunction(List<double> list, out double angleNumber)
        {
            angleNumber = 0;//需要转动的角度值
            double MaxInter = 8; //最大间隙值
            int TimeIndex = 0; //最大间隙值的索引
            for (int i = 0; i < list.Count() - 1; i++)
            {
                double TimeMax = list[i + 1] - list[i];

                if (TimeMax > MaxInter)
                {
                    MaxInter = TimeMax;
                    TimeIndex = i + 1;
                }
            }

            // 寻找峰值，根据最大间隙处，进行查找
            int TimeIndexMin = 0;//最大间隙值的相反数索引
            for (int i = TimeIndex; i < list.Count() - 1; i++)
            {
                double MinInter = -8;//最大间隙值相反数，即当从峰值减小时
                double TimeMin = list[i + 1] - list[i];
                if (TimeMin < MinInter)
                {
                    TimeIndexMin = i;//记录索引
                    break;
                }
            }
            //峰值
            double MaxZ = list[TimeIndex]; //峰值
            int MaxZindex = TimeIndex;//峰值索引
            for (int i = TimeIndex; i < TimeIndexMin; i++)
            {
                if (list[i + 1] > list[i])
                {
                    MaxZ = list[i + 1];
                    MaxZindex = i + 1;
                }
            }

            //Console.WriteLine("峰值下降处的索引：{0},{1}", TimeIndexMin, list[TimeIndexMin]);
            int Index = list.Count() / 2;//索引的中间值
            double z = list[TimeIndexMin];  // 最高点的z值，即距离线激光轮廓仪的值d2，tanα=d1/d2;
            //Console.WriteLine("峰值{0},峰值索引{1}，{2}", MaxZ, MaxZindex, Index);
            double d2 = Math.Abs(z);//因为基准坐标系距离镜头的距离300mm处。
            if (MaxZindex < Index)
            {
                double d1 = -(MaxZindex - Index) * 0.1;//d1表示，中心点距离最高点的x值，d2表示最高点到镜头的值
                angleNumber = (Math.Atan(d1 / d2) * 180) / (Math.PI);
            }
            else if (MaxZindex > Index)
            {
                double d1 = (MaxZindex - Index) * 0.1;
                angleNumber = (Math.Atan(d1 / d2) * 180) / (Math.PI);
            }
            else
            {
                angleNumber = 0;
            }
            //Console.WriteLine("转动的角度值:{0}°", angleNumber);
        }


        // 算法3 移动+转动
        public void MoveAngleFunction(List<double> list,out double moveNumber,out double Angle)
        {
            moveNumber = 0;
            Angle = 0;
            double MaxInter = 8; //最大间隙值
            int TimeIndex = 0; //最大间隙值的索引
            for (int i = 0; i < list.Count() - 1; i++)
            {
                double TimeMax = list[i + 1] - list[i];

                if (TimeMax > MaxInter)
                {
                    MaxInter = TimeMax;
                    TimeIndex = i + 1;
                }
            }

            // 寻找峰值，根据最大间隙处，进行查找
            int TimeIndexMin = 0;//最大间隙值的相反数索引
            for (int i = TimeIndex; i < list.Count() - 1; i++)
            {
                double MinInter = -8;//最大间隙值相反数，即当从峰值减小时
                double TimeMin = list[i + 1] - list[i];
                if (TimeMin < MinInter)
                {
                    TimeIndexMin = i + 1;//记录索引
                    break;
                }
            }
            // TimeIndex与TimeIndexMin是索引峰值的区间
            //峰值MaxZ
            double MaxZ = list[TimeIndex]; //峰值
            int MaxZindex = TimeIndex;//峰值索引
            for (int i = TimeIndex; i < TimeIndexMin; i++)
            {
                if (list[i + 1] > list[i])
                {
                    MaxZ = list[i + 1];
                    MaxZindex = i + 1;
                }
            }

            //根据区间索引两点拟合直线或者说，求直线方程
            int Index = list.Count() / 2;//索引的中间值
            double X1 = (Index - TimeIndex - 1) * 0.1;//x1的值，减1是因为该点必须在直线上，而不是在弧线上
            double X2 = (Index - TimeIndexMin) * 0.1;//x2的值
            double k = (list[TimeIndexMin] - list[TimeIndex]) / (X2 - X1);//斜率
            double c = list[TimeIndex] - k * X1;//常数c

            //求峰值点到直线方程的距离h1
            double MaxX = (Index - MaxZindex) * 0.1;//峰值点的x值
            double h1 = Math.Abs(k * MaxX - MaxZ + c) / (Math.Sqrt(k*k + 1.0));
            double h2 = MaxZ - (k * MaxX + c);//将峰值点x带入直线方程求得同一点x，直线方程的z值，两值相减为距离值h2

            //求得角度值α
            Angle = Math.Abs(Math.Acos(h1 / h2)*180/(Math.PI));

            //求移动值d2
            double z0 = Math.Abs(list[Index]);// 原点处的z值
            double d1 = z0 * Math.Sin((Angle*Math.PI)/180);//求转动后，镜头的中心线在点云上的交点值与原点中心的值
            moveNumber = Math.Abs(d1 - MaxX);//moveNumber为所求移动值

            if (list[TimeIndex-1]<list[TimeIndexMin])//比较z值判定角度的大小
            {
                //如果z1小于z2，则向下转动
                Angle = -Angle;
            }

            if (MaxZindex>Index)
            {
                moveNumber = -moveNumber; //如果峰值的索引大于原点的索引则向下移动
            }

        }
    }
}
