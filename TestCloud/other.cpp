#include"other.h"



//利用条件去除无效点
pcl::PointCloud<pcl::PointXYZ>::Ptr ConditionalRemoval(pcl::PointCloud<pcl::PointXYZ>::Ptr& cloud, bool b)
{
	clock_t start = clock();
	pcl::PointCloud<pcl::PointXYZ>::Ptr cloud_filtered(new pcl::PointCloud<pcl::PointXYZ>);

	//创建条件限定下的滤波器
	pcl::ConditionAnd<pcl::PointXYZ>::Ptr range_cond(new pcl::ConditionAnd<pcl::PointXYZ>);
	range_cond->addComparison(pcl::FieldComparison<pcl::PointXYZ>::ConstPtr(
		new pcl::FieldComparison<pcl::PointXYZ>("z", pcl::ComparisonOps::GT, -100)));//大于0的值   //设置作用域为z，取大于-0.1m且小于0.1m的位置，保留在点云中，其余进行移除
	range_cond->addComparison(pcl::FieldComparison<pcl::PointXYZ>::ConstPtr(
		new pcl::FieldComparison<pcl::PointXYZ>("z", pcl::ComparisonOps::LT, 100)));//小于100的值
																					//-0.05<内点<0.05
																					/*range_cond->addComparison(pcl::FieldComparison<pcl::PointXYZ>::ConstPtr(
																					new pcl::FieldComparison<pcl::PointXYZ>("y", pcl::ComparisonOps::GT, 0.10)));
																					range_cond->addComparison(pcl::FieldComparison<pcl::PointXYZ>::ConstPtr(
																					new pcl::FieldComparison<pcl::PointXYZ>("y", pcl::ComparisonOps::LT, 0.12)));*/

																					//创建滤波器并用条件定义对象初始化
	pcl::ConditionalRemoval<pcl::PointXYZ> condrem;
	condrem.setCondition(range_cond);
	condrem.setInputCloud(cloud);//设置输入点云
	condrem.setKeepOrganized(b);//设置保持点云的结构:为true时被剔除的点为NAN
								//condrem.setUserFilterValue(0.003f);//设置替换值
								//condrem.setKeepOrganized(false);
	condrem.filter(*cloud_filtered);//执行条件滤波，存储结果到cloud_filtered

	double t = (double)(clock() - start) / CLOCKS_PER_SEC;
	//std::cerr << "ConditionalRemoval滤波耗时:" << t << "s" << endl;

	return cloud_filtered;
}


// 移除Nan点，因为经过滤波之后会有存在
void removeNan(pcl::PointCloud<pcl::PointXYZ>::Ptr& cloud)
{
	clock_t start = clock();

	std::vector<int> indices;
	pcl::PointCloud<pcl::PointXYZ>::Ptr Cloud_nan(new pcl::PointCloud<pcl::PointXYZ>);
	pcl::removeNaNFromPointCloud(*cloud, *Cloud_nan, indices); // indices为输出点云的索引

															   // 寻找nan点的索引值
	std::vector<int> NanIndex;
	int k = 0;
	for (size_t i = 0; i < cloud->size(); i++)
	{

		int j = i - k;
		//cout << "indices大小："  << indices.size() << "j值：" << j << endl;
		if (j >= indices.size())  // 如果结尾出现nan，直接将剩下的索引值写入数组索引中
		{

			for (size_t l = j + k; l < cloud->size(); l++)
			{
				NanIndex.push_back(l);
				//cout << "尾部移除nan值:" << l << endl;
				if (l % 300 == 0)
				{
					// 将左边的点云赋值给该nan点
					cloud->points[l].x = 0;
					cloud->points[l].y = (cloud->points[l - 300].y) + 1; // 此处+1是因为y与y之间的间隙为1
					cloud->points[l].z = (cloud->points[l - 300].z);
				}
				else
				{
					// 将前面的点云赋值给该nan点
					cloud->points[l].x = (cloud->points[l - 1].x) + 0.8; // 此处+0.8是因为x与x之间的间隙为0.8
					cloud->points[l].y = (cloud->points[l - 1].y);
					cloud->points[l].z = (cloud->points[l - 1].z);
				}


			}
			break;

		}
		else if (i == indices[j])
		{
			continue;
		}
		else // 将nan的索引写入数组
		{
			NanIndex.push_back(i);
			k++;
			//cout << "nan值索引：" << i << endl;
			// 点云替换
			if (i % 300 == 0)
			{
				if (i == 0)
				{
					// 将左边的点云赋值给该nan点
					cloud->points[i].x = 0;
					cloud->points[i].y = 0;
					int n = 1;
					//==============
					while (!(cloud->points[0].z > -1))//isnan(cloud->points[0].x)
					{
						if (cloud->points[0 + n].z > -1)//！isnan(cloud->points[0].x)
						{
							//index = (pcl::pointcloud<pcl::pointxyz>::iterator)idx;
							cloud->points[0].z = cloud->points[0 + n].z;
							//std::cout << cloud->points[0].z << endl;
						}
						else
						{
							n++;
						}
					}
				}
				else
				{
					// 将左边的点云赋值给该nan点
					cloud->points[i].x = 0;
					cloud->points[i].y = (cloud->points[i - 300].y) + 1;
					cloud->points[i].z = (cloud->points[i - 300].z);
				}

			}
			else
			{
				cloud->points[i].x = (cloud->points[i - 1].x) + 0.8;
				cloud->points[i].y = (cloud->points[i - 1].y);
				cloud->points[i].z = (cloud->points[i - 1].z);
			}


		}
	}
	cloud->width = 300;
	cloud->height = cloud->points.size() / 300;
	cloud->is_dense = false;
	//保存C:\Users\YSU_HYL\Desktop\Test_ShangHai\2AIR_WorkLine300_ConLine300\2021.12.24_300_300_test1.csv
	pcl::io::savePCDFileASCII("C:\\Users\\YSU_HYL\\Desktop\\Test_ShangHai\\2AIR_WorkLine300_ConLine300\\2021.12.24_300_300_test1_NoNan.txt", *cloud);

	//cout << "点云中nan值的索引：";
	//for (size_t i = 0; i < NanIndex.size(); i++)
	//{
	//	cout << NanIndex[i] << " ";
	//}

	//cout <<  "过滤后点云数量："  << indices.size() << "nan值数量：" << NanIndex.size() << endl;

	//int i = 0;
	//int Y = cloud->size() - 1 - indices[indices.size() - 1];
	// 将尾部的nan值替换掉
	//for (int j = 0; j < Y; j++)
	//{
	//	cloud->points[cloud->size() - Y + j] = cloud->points[cloud->size() - Y - 1 + j];
	//	//std::cout << cloud->points[cloud->size() - Y].z << endl;
	//}
	//// 有问题？？？？？？
	//for (int index = 0; index < indices[indices.size() - 1]; index++)
	//{
	//	if (index == 0 && indices[0] != 0)
	//	{
	//		int n = 1;
	//		while (!(cloud->points[0].x > -1))//isnan(cloud->points[0].x)
	//		{
	//			if (cloud->points[0 + n].x > -1)//！isnan(cloud->points[0].x)
	//			{
	//				i++;
	//				//index = (pcl::PointCloud<pcl::PointXYZ>::iterator)idx;
	//				cloud->points[0] = cloud->points[0 + n];
	//				//std::cout << cloud->points[0].z << endl;
	//			}
	//			else
	//			{
	//				n++;
	//			}
	//		}
	//	}
	//	else
	//	{
	//		//if (index > i && index - i > indices.size() - 1)
	//		//{
	//		//	for (int j = 0; j < Y; j++)
	//		//	{
	//		//		cloud->points[cloud->size() - Y + j] = cloud->points[cloud->size() - Y - 1 + j];
	//		//		//std::cout << cloud->points[cloud->size() - Y].z << endl;
	//		//	}
	//		//	break;
	//		//}
	//		if (index != indices[index - i] && index > 0)
	//		{
	//			if (index % 300 == 0)
	//			{
	//				i++;
	//				//index = (pcl::PointCloud<pcl::PointXYZ>::iterator)idx;
	//				cloud->points[index] = cloud->points[index - 300];
	//				//std::cout << cloud->points[index].z << endl;
	//			}
	//			else
	//			{
	//				i++;
	//				//index = (pcl::PointCloud<pcl::PointXYZ>::iterator)idx;
	//				cloud->points[index] = cloud->points[index - 1];
	//				//std::cout << cloud->points[index].z << endl;
	//			}
	//		}
	//	}

	double t = (double)(clock() - start) / CLOCKS_PER_SEC;
	//std::cerr << "removeNan耗时:" << t << "s" << endl;
}


// 可视化

// 单一可视化
void VisualizeCloud(pcl::PointCloud<pcl::PointXYZ>::Ptr& cloud)
{
	boost::shared_ptr<pcl::visualization::PCLVisualizer> viewer(new pcl::visualization::PCLVisualizer("3D viewer"));
	viewer->setBackgroundColor(255, 255, 255);
	pcl::visualization::PointCloudColorHandlerCustom<pcl::PointXYZ> single_color(cloud, 255, 0, 0); //设置点云颜色
	viewer->addPointCloud(cloud, single_color, "cloud");
	viewer->setPointCloudRenderingProperties(pcl::visualization::PCL_VISUALIZER_POINT_SIZE, 3, "cloud");
	viewer->addCoordinateSystem(1);  // 1.0是坐标系的大小比例
	viewer->initCameraParameters();  // 初始化相机设置？？？
	while (!viewer->wasStopped())
	{
		viewer->spinOnce(100);
		boost::this_thread::sleep(boost::posix_time::microseconds(100000));
	}

}

void Visualize2Cloud(pcl::PointCloud<pcl::PointXYZ>::Ptr & cloud, pcl::PointCloud<pcl::PointXYZ>::Ptr & filter_cloud)
{
	//-----------------------显示点云-----------------------
	boost::shared_ptr<pcl::visualization::PCLVisualizer> viewer(new pcl::visualization::PCLVisualizer("显示点云"));

	int v1(0), v2(0);
	viewer->createViewPort(0.0, 0.0, 0.5, 1.0, v1);
	viewer->setBackgroundColor(0, 0, 0, v1);
	viewer->addText("point clouds", 10, 10, "v1_text", v1);
	viewer->createViewPort(0.5, 0.0, 1, 1.0, v2);
	viewer->setBackgroundColor(0.1, 0.1, 0.1, v2);
	viewer->addText("filtered point clouds", 10, 10, "v2_text", v2);
	// 按照z字段进行渲染,将z改为x或y即为按照x或y字段渲染
	pcl::visualization::PointCloudColorHandlerGenericField<pcl::PointXYZ> fildColor(cloud, "x");
	viewer->addPointCloud<pcl::PointXYZ>(cloud, fildColor, "sample cloud", v1);

	viewer->addPointCloud<pcl::PointXYZ>(filter_cloud, "cloud_filtered", v2);
	viewer->setPointCloudRenderingProperties(pcl::visualization::PCL_VISUALIZER_COLOR, 0, 1, 0, "cloud_filtered", v2);
	//viewer->addCoordinateSystem(1.0);
	viewer->initCameraParameters();
	while (!viewer->wasStopped())
	{
		viewer->spinOnce(100);
		boost::this_thread::sleep(boost::posix_time::microseconds(100000));
	}
}



//对于width!=1的点云
void IntegralImageNormalEstimation(pcl::PointCloud<pcl::PointXYZ>::Ptr& cloud, pcl::PointCloud<pcl::Normal>::Ptr& cloud_normals)
{
	clock_t start = clock();

	// 法线估计对象
	pcl::IntegralImageNormalEstimation<pcl::PointXYZ, pcl::Normal> normalEstimation;

	// 法线估计方法: COVARIANCE_MATRIX, AVERAGE_DEPTH_CHANGE, SIMPLE_3D_GRADIENT.
	/****************************************************************************************
	三种法线估计方法
	COVARIANCE_MATRIX 模式从具体某个点的局部邻域的协方差矩阵创建9个积分，来计算这个点的法线
	AVERAGE_3D_GRADIENT   模式创建6个积分图来计算水平方向和垂直方向的平滑后的三维梯度并使用两个梯度间的向量
	积计算法线
	AVERAGE_DEPTH_CHANGE  模式只创建了一个单一的积分图，从而平局深度变化计算法线
	********************************************************************************************/
	normalEstimation.setNormalEstimationMethod(normalEstimation.AVERAGE_3D_GRADIENT);//
																					 //设置深度变化的阀值
	normalEstimation.setMaxDepthChangeFactor(0.02f);//0.02
													// 设置计算法线的区域
	normalEstimation.setNormalSmoothingSize(5.0f);//10//越大越圆滑
	normalEstimation.setViewPoint(0, 0, 100);//同一方向
	normalEstimation.setInputCloud(cloud);

	// 计算
	normalEstimation.compute(*cloud_normals);

	//for (int i = 0; i < cloud_normals->points.size(); i++)//同一方向
	//{
	//	if (cloud_normals->points[i].normal_z < 0)
	//	{
	//		cloud_normals->points[i].normal_x = -(cloud_normals->points[i].normal_x);
	//		cloud_normals->points[i].normal_y = -(cloud_normals->points[i].normal_y);
	//		cloud_normals->points[i].normal_z = -(cloud_normals->points[i].normal_z);
	//	}
	//}

	double t = (double)(clock() - start) / CLOCKS_PER_SEC;
	std::cerr << "IntegralImageNormalEstimation法线估计:" << t << "s" << endl;
}


// 可视化法线估计
void VisualizeNormalCloud(pcl::PointCloud<pcl::PointXYZ>::Ptr& cloud, pcl::PointCloud<pcl::Normal>::Ptr& normals)
{
	pcl::visualization::PCLVisualizer viewer("Normal Viewer");
	viewer.setBackgroundColor(0, 0, 0.5);
	viewer.addPointCloudNormals<pcl::PointXYZ, pcl::Normal>(cloud, normals);
	viewer.initCameraParameters();
	while (!viewer.wasStopped())
	{
		viewer.spinOnce(100);
		boost::this_thread::sleep(boost::posix_time::microseconds(100000));
	}
}

// 利用Z之间的高度差进行区域分割，检测单一轮廓线，点与点之间间距最大的那个点云坐标值！！！
void SegmentZ(pcl::PointCloud<pcl::PointXYZ>::Ptr& cloud, int R)
{
	// 设置高度差, R = 8mm

	// X值最小的索引数组
	std::vector<int> xIndex;
	// 先去遍历各个轮廓线

	for (size_t i = 50; i < (cloud->width)-50; ++i)
	{
		//cout << "此时循环的行数为：" << i << "/一共行数为：" << cloud->width << endl;
		//cout << "第i=" << i << "次，报错！" << endl;
		if (i >= cloud->width) { cout << "1处越界" << endl; }
		// Z值最大的数组和相应的索引
		std::vector<float> zArray;
		std::vector<int> zIndex;
		// 再遍历轮廓线中的点
		// 不从0开始检测，跨过前面的数据和最后几组数据
		// 取中间160个点，跨过前20个数据和后20个数据
		for (size_t j = 20 + (cloud->height)*i; j < (cloud->height)*(i + 1) - 20; j++)
		{
			//cout << "第j=" << j << "次，报错！" << endl;
			if ((j + 1) % 300 == 0)
			{
				cout << "跳出了？？" << endl;
				continue;
			}
			else if (j == cloud->size() - 1)
			{
				cout << "打断了？？？" << endl;
				break;
			}
			else
			{
				// 判断当两点之间的Z值差距大于直径，则记录该值和点云索引值
				if ((cloud->points[j + 1].z - cloud->points[j].z) >= R)
				{
					zArray.push_back(cloud->points[j + 1].z); // 把z最大值存入
					zIndex.push_back(j + 1);               // 把点云的索引存入zIndex
				}
				else
				{
					continue;
				}
			}
		}
		// 判断索引值数量，并找出Z的两个最大值
		if (zIndex.size() < 1) // 如果没有找到两个点，则跳出
		{
			//cout << "该轮廓没有找到间隙点！" << endl;
			continue;
		}
		else if (1 <= zIndex.size() < 2)
		{
			xIndex.push_back(zIndex[0]);
		}
		else
		{
			// // 找Z值最大的前两个，比较zArray中x的最小值，存入xIndex索引中。
			float max[2] = { 0,0 };
			int maxIndex[2] = { 0,0 };
			// 找最大的两个Z值
			for (size_t k = 0; k < zIndex.size(); k++)
			{
				if (cloud->points[(zIndex[k])].z > max[0])
				{
					max[1] = max[0];
					maxIndex[1] = maxIndex[0];
					maxIndex[0] = zIndex[k];
					max[0] = cloud->points[(zIndex[k])].z;
				}
				else
				{
					continue;
				}
			}
			// 比较X值,记录索引
			// points(maxIndex[0]).z中Z的值肯定是大于points(maxIndex[0]).z中Z的值
			if (cloud->points[(maxIndex[0])].x <= cloud->points[(maxIndex[1])].x)
			{
				xIndex.push_back(maxIndex[0]);
			}
			else
			{
				xIndex.push_back(maxIndex[1]);
			}
		}
		zArray.clear();
		zIndex.clear();
	}
	// 利用xIndex，比较他们的Z的间隙最大值,记录索引。
	// 注意Z值最大，不代表Z值的间隙最大。
	float zMax = 0;		// 间隙最大
	int zCor = 0;		// 间隙最大坐标
	//for (size_t i = 0; i < xIndex.size(); i++)
	//{
	//	// 索引大小100？？？？
	//	cout << "索引大小：" << xIndex.size() << endl;
	//	cout << "第" << floor(xIndex[i] / 300) << "行\t" << "第" << xIndex[i] % 300 << "个数" << endl;
	//	cout << "Z值为" << cloud->points[xIndex[i]].z << endl;
	//}
	// 赋予临时变量，代表Z的最大差值
	double temp_z = 0;
	int zCor2 = 0; // 间隙第二大的坐标
	for (size_t i = 0; i < xIndex.size(); i++)
	{
		// 计算间隙最大值，并保存该点的索引值
		double temp_z1 = cloud->points[xIndex[i]].z - cloud->points[xIndex[i] - 1].z; // 间隙计算：根据索引点与上一点的差值
		if (temp_z1>temp_z)
		{
			
			// 把间隙最大值赋给temp_z
			temp_z = temp_z1;
			// 记录索引,更新索引
			zMax = cloud->points[xIndex[i]].z;		// 更新间隙最大索引
			if (zCor>zCor2)	zCor2 = zCor;	// 将间隙最大赋值给第二大，并更新第一大
			zCor = xIndex[i];						// 更新间隙最大坐标
		}
	}
	xIndex.clear();
	// 打印间隙最大值坐标点
	cout << "已找到坐标点，该坐标为：[" << cloud->points[zCor].x << ","
		<< cloud->points[zCor].y << ","
		<< cloud->points[zCor].z << "]" << endl;
	cout << "最大间隙差值为：" << temp_z << endl;
	cout << "该坐标点的上一点的坐标为：[" << cloud->points[zCor-1].x << ","
		<< cloud->points[zCor-1].y << ","
		<< cloud->points[zCor-1].z << "]" << endl;


	// 将坐标保存到IO文件
	fstream ofs;	// 创建文件流
	ofs.open("C:/Users/YSU_HYL/Desktop/zb.txt", ios::out | ios::trunc);
	if (!ofs.is_open())
	{
		cout << "打开文件错误" << endl;
		exit(1);
	}

	// 将 两组坐标写入文件中，其中一组为挂牌点，一组为补挂点。
	// 一组坐标，包括三个坐标点，三个坐标点连接成弧线，一个坐标点包括XYZ。
	// 每一个坐标点之间 x的间距为40mm，即50个索引，因为每个x间距0.8
	if (zCor%300 > 50)									// 防止数组越界，挂牌点可能会小于索引50
	{
		ofs << cloud->points[zCor - 50].x << ","
			<< cloud->points[zCor - 50].y << ","
			<< cloud->points[zCor - 50].z << ",";
	}
	else
	{
		ofs << cloud->points[zCor].x-30 << ","			// x-30 在原本挂牌点的上方
			<< cloud->points[zCor].y << ","
			<< cloud->points[zCor].z+10 << ",";			// z+10 在原本挂牌点的里面
	}

	ofs << cloud->points[zCor].x << ","					// 将挂牌点写入
		<< cloud->points[zCor].y << ","
		<< cloud->points[zCor].z << ",";				// z 加不加半径的尺寸，需要试验判断

	if (zCor%300 < 250)										// 防止数组越界，挂牌点可能会大于索引250
	{
		ofs << cloud->points[zCor + 50].x << ","
			<< cloud->points[zCor + 50].y << ","
			<< cloud->points[zCor + 50].z << ",";
	}
	else
	{
		ofs << cloud->points[zCor].x + 30 << ","			// x-30 在原本挂牌点的下方
			<< cloud->points[zCor].y << ","
			<< cloud->points[zCor].z - 10 << ",";			// z-10 在原本挂牌点的里面
	}
	// 自此，第一组坐标结束

	// 第二组，补挂坐标开始

	if (zCor2%300>50)									// 防止数组越界，挂牌点可能会小于索引50
	{
		ofs << cloud->points[zCor2 - 50].x << ","
			<< cloud->points[zCor2 - 50].y << ","
			<< cloud->points[zCor2 - 50].z << ",";
	}
	else
	{
		ofs << cloud->points[zCor2].x - 30 << ","			// x-30 在原本挂牌点的上方
			<< cloud->points[zCor2].y << ","
			<< cloud->points[zCor2].z + 10 << ",";			// z+10 在原本挂牌点的里面
	}

	ofs << cloud->points[zCor2].x << ","
		<< cloud->points[zCor2].y << ","
		<< cloud->points[zCor2].z << ",";

	if (zCor2%300 < 250)										// 防止数组越界，挂牌点可能会大于索引250
	{
		ofs << cloud->points[zCor2 + 50].x << ","
			<< cloud->points[zCor2 + 50].y << ","
			<< cloud->points[zCor2 + 50].z << endl;
	}
	else
	{
		ofs << cloud->points[zCor2].x + 30 << ","			// x-30 在原本挂牌点的下方
			<< cloud->points[zCor2].y << ","
			<< cloud->points[zCor2].z - 10 << endl;;			// z-10 在原本挂牌点的里面
	}

	ofs.close();
}