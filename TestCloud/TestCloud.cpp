/*
将csv格式转化为pcd，并显示
*/
#define _SCL_SECURE_NO_WARNINGS

#include <iostream>
#include <fstream>
#include <vector>
#include <sstream> // 用于读写存储在内存中的string对象
#include <pcl/io/pcd_io.h>
#include <pcl/point_types.h>
#include <pcl/visualization/pcl_visualizer.h>
#include <boost/thread/thread.hpp>
#include <pcl/point_types.h>
#include "other.h"
#include <stdlib.h>
#include<chrono>
using namespace std;
using namespace chrono;


int main(int argc, char** argv)
{
	//clock_t start = clock();
	auto start_c = system_clock::now();
	//==============================
	// c++ 删除csv文件前两行或读取特定行C:\\Users\\YSU_HYL\\Desktop\\Test_ShangHai\\3AIR_WorkLine300_ConLine300\\2021.12.24_300_300_test1.csv
	// C# 中保存时，去掉csv前两行。‪‪   C:/Users/YSU_HYL/Desktop/2022_7_13_03.txt   Test.txt  D:/GuaPai/PCL_test1.8.1/TestCamera/bin/x64/Debug/Test.txt
	// 读取CSV文件     C:\\Users\\YSU_HYL\\Desktop\\Test_ShangHai\\NO4AIR_WorkLine300_ConLine300\\2021.12.25_300_300_test5.csv
	fstream inFile("C:/Users/YSU_HYL/Desktop/Test_ShangHai/3AIR_WorkLine300_ConLine300/2021.12.24_300_300_test1.csv", ios::in);
	std::string lineStr;
	std::string str;

	pcl::PointCloud<pcl::PointXYZ>::Ptr cloud(new pcl::PointCloud<pcl::PointXYZ>);
	pcl::PointXYZ point;

	int yStep = 0;		// y的初值
	//getline(inFile, lineStr);		// 省略第一行
	while (std::getline(inFile, lineStr))
	{
		// 未经过TCP手眼标定转换
		std::stringstream ss(lineStr); // string数据流化
		float xStep = -120;		// x的初值,从 -120~120
		float xjStep = 0.8;
		while (std::getline(ss, str, ',')) // 读取行中每一个元素
		{
			point.x = xStep;
			// 因为以工作距离300为基准，所以需要用300去减去一个测量的高度值，才是距离镜头末端的值。
			//point.z = 300 - (std::stoi(str));
			/*cout << str;*/
			point.z = std::stoi(str);
			point.y = yStep;
			cloud->points.push_back(point);
			//xStep = xStep + xjStep;
			xStep = round((xStep + xjStep) * 10) / 10;  // x步长为0.8，共160行,有效保存一位小数
														//cout << "xstep" << xStep << endl;
		}
		yStep = yStep + 1;
		//cout << "ystep" << yStep << endl;
	}
	inFile.close();
	auto start_middle = system_clock::now();
	cout << "点云大小：" << cloud->points.size() << endl;
	chrono::duration<double> runtime_middle{ start_middle - start_c };
	cout << "加载耗时:" << runtime_middle.count() << "s" << endl;
	int mwidth = 300;
	cloud->width = mwidth;
	cloud->height = cloud->points.size() / mwidth;
	cloud->is_dense = false;
	cloud->points.resize(cloud->width * cloud->height);
	//cloud->width = cloud->points.size();
	//cloud->height = 1;


	// 保存查看点云形状是否和扫描一致，用于调试。
	//pcl::io::savePCDFileASCII("C:\\Users\\YSU_HYL\\Desktop\\test_PCD.pcd", *cloud);


	pcl::PointCloud<pcl::PointXYZ>::Ptr cloud_filtered(new pcl::PointCloud<pcl::PointXYZ>);//滤波后点云定义
	cloud_filtered = ConditionalRemoval(cloud, true);//条件滤波设置为:-100<z<100;

	// 是否保存查看具体nan点位置？
	//pcl::io::savePCDFileASCII("C:\\Users\\YSU_HYL\\Desktop\\Nan_PCD.pcd", *cloud_filtered);

	// 滤波之后，替换nan点
	removeNan(cloud_filtered); //移除、替换nan

	//pcl::PointCloud<pcl::PointXYZ>::Ptr no_nan_cloud(new pcl::PointCloud<pcl::PointXYZ>);
	//pcl::io::loadPCDFile<pcl::PointXYZ>("C:\\Users\\YSU_HYL\\Desktop\\no_Nan_PCD.pcd", *no_nan_cloud);
	//Visualize2Cloud(cloud, no_nan_cloud);

	// 区域分割,检测单一轮廓线，点与点之间间距最大的那个点云坐标值！！！
	// 利用Z之间的高度差进行区域分割，检测单一轮廓线，点与点之间间距最大的那个点云坐标值！！！
	// 设置区分高度
	cout << "开始寻找坐标点......" << endl;
	int R = 8;
	SegmentZ(cloud_filtered, R);
	cout << "结束" << endl;
	//double t = (double)(clock() - start) / CLOCKS_PER_SEC;
	//std::cerr << "总耗时:" << t << "s" << endl;

	auto end_c = system_clock::now();
	chrono::duration<double> runtime{ end_c - start_c };
	cout << "总耗时:" << runtime.count() << "s" << endl;

	// 进行体素滤波

	// 积分图法线有序
	//pcl::PointCloud<pcl::Normal>::Ptr cloud_normals(new pcl::PointCloud<pcl::Normal>);
	//IntegralImageNormalEstimation(no_nan_cloud, cloud_normals);//积分图法线，有序

	// 可视化法线估计
	//VisualizeNormalCloud(no_nan_cloud, cloud_normals);
	system("PAUSE");
	return (0);
}