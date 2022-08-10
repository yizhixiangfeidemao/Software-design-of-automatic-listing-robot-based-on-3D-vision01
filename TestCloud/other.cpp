#include"other.h"



//��������ȥ����Ч��
pcl::PointCloud<pcl::PointXYZ>::Ptr ConditionalRemoval(pcl::PointCloud<pcl::PointXYZ>::Ptr& cloud, bool b)
{
	clock_t start = clock();
	pcl::PointCloud<pcl::PointXYZ>::Ptr cloud_filtered(new pcl::PointCloud<pcl::PointXYZ>);

	//���������޶��µ��˲���
	pcl::ConditionAnd<pcl::PointXYZ>::Ptr range_cond(new pcl::ConditionAnd<pcl::PointXYZ>);
	range_cond->addComparison(pcl::FieldComparison<pcl::PointXYZ>::ConstPtr(
		new pcl::FieldComparison<pcl::PointXYZ>("z", pcl::ComparisonOps::GT, -100)));//����0��ֵ   //����������Ϊz��ȡ����-0.1m��С��0.1m��λ�ã������ڵ����У���������Ƴ�
	range_cond->addComparison(pcl::FieldComparison<pcl::PointXYZ>::ConstPtr(
		new pcl::FieldComparison<pcl::PointXYZ>("z", pcl::ComparisonOps::LT, 100)));//С��100��ֵ
																					//-0.05<�ڵ�<0.05
																					/*range_cond->addComparison(pcl::FieldComparison<pcl::PointXYZ>::ConstPtr(
																					new pcl::FieldComparison<pcl::PointXYZ>("y", pcl::ComparisonOps::GT, 0.10)));
																					range_cond->addComparison(pcl::FieldComparison<pcl::PointXYZ>::ConstPtr(
																					new pcl::FieldComparison<pcl::PointXYZ>("y", pcl::ComparisonOps::LT, 0.12)));*/

																					//�����˲�������������������ʼ��
	pcl::ConditionalRemoval<pcl::PointXYZ> condrem;
	condrem.setCondition(range_cond);
	condrem.setInputCloud(cloud);//�����������
	condrem.setKeepOrganized(b);//���ñ��ֵ��ƵĽṹ:Ϊtrueʱ���޳��ĵ�ΪNAN
								//condrem.setUserFilterValue(0.003f);//�����滻ֵ
								//condrem.setKeepOrganized(false);
	condrem.filter(*cloud_filtered);//ִ�������˲����洢�����cloud_filtered

	double t = (double)(clock() - start) / CLOCKS_PER_SEC;
	//std::cerr << "ConditionalRemoval�˲���ʱ:" << t << "s" << endl;

	return cloud_filtered;
}


// �Ƴ�Nan�㣬��Ϊ�����˲�֮����д���
void removeNan(pcl::PointCloud<pcl::PointXYZ>::Ptr& cloud)
{
	clock_t start = clock();

	std::vector<int> indices;
	pcl::PointCloud<pcl::PointXYZ>::Ptr Cloud_nan(new pcl::PointCloud<pcl::PointXYZ>);
	pcl::removeNaNFromPointCloud(*cloud, *Cloud_nan, indices); // indicesΪ������Ƶ�����

															   // Ѱ��nan�������ֵ
	std::vector<int> NanIndex;
	int k = 0;
	for (size_t i = 0; i < cloud->size(); i++)
	{

		int j = i - k;
		//cout << "indices��С��"  << indices.size() << "jֵ��" << j << endl;
		if (j >= indices.size())  // �����β����nan��ֱ�ӽ�ʣ�µ�����ֵд������������
		{

			for (size_t l = j + k; l < cloud->size(); l++)
			{
				NanIndex.push_back(l);
				//cout << "β���Ƴ�nanֵ:" << l << endl;
				if (l % 300 == 0)
				{
					// ����ߵĵ��Ƹ�ֵ����nan��
					cloud->points[l].x = 0;
					cloud->points[l].y = (cloud->points[l - 300].y) + 1; // �˴�+1����Ϊy��y֮��ļ�϶Ϊ1
					cloud->points[l].z = (cloud->points[l - 300].z);
				}
				else
				{
					// ��ǰ��ĵ��Ƹ�ֵ����nan��
					cloud->points[l].x = (cloud->points[l - 1].x) + 0.8; // �˴�+0.8����Ϊx��x֮��ļ�϶Ϊ0.8
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
		else // ��nan������д������
		{
			NanIndex.push_back(i);
			k++;
			//cout << "nanֵ������" << i << endl;
			// �����滻
			if (i % 300 == 0)
			{
				if (i == 0)
				{
					// ����ߵĵ��Ƹ�ֵ����nan��
					cloud->points[i].x = 0;
					cloud->points[i].y = 0;
					int n = 1;
					//==============
					while (!(cloud->points[0].z > -1))//isnan(cloud->points[0].x)
					{
						if (cloud->points[0 + n].z > -1)//��isnan(cloud->points[0].x)
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
					// ����ߵĵ��Ƹ�ֵ����nan��
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
	//����C:\Users\YSU_HYL\Desktop\Test_ShangHai\2AIR_WorkLine300_ConLine300\2021.12.24_300_300_test1.csv
	pcl::io::savePCDFileASCII("C:\\Users\\YSU_HYL\\Desktop\\Test_ShangHai\\2AIR_WorkLine300_ConLine300\\2021.12.24_300_300_test1_NoNan.txt", *cloud);

	//cout << "������nanֵ��������";
	//for (size_t i = 0; i < NanIndex.size(); i++)
	//{
	//	cout << NanIndex[i] << " ";
	//}

	//cout <<  "���˺����������"  << indices.size() << "nanֵ������" << NanIndex.size() << endl;

	//int i = 0;
	//int Y = cloud->size() - 1 - indices[indices.size() - 1];
	// ��β����nanֵ�滻��
	//for (int j = 0; j < Y; j++)
	//{
	//	cloud->points[cloud->size() - Y + j] = cloud->points[cloud->size() - Y - 1 + j];
	//	//std::cout << cloud->points[cloud->size() - Y].z << endl;
	//}
	//// �����⣿����������
	//for (int index = 0; index < indices[indices.size() - 1]; index++)
	//{
	//	if (index == 0 && indices[0] != 0)
	//	{
	//		int n = 1;
	//		while (!(cloud->points[0].x > -1))//isnan(cloud->points[0].x)
	//		{
	//			if (cloud->points[0 + n].x > -1)//��isnan(cloud->points[0].x)
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
	//std::cerr << "removeNan��ʱ:" << t << "s" << endl;
}


// ���ӻ�

// ��һ���ӻ�
void VisualizeCloud(pcl::PointCloud<pcl::PointXYZ>::Ptr& cloud)
{
	boost::shared_ptr<pcl::visualization::PCLVisualizer> viewer(new pcl::visualization::PCLVisualizer("3D viewer"));
	viewer->setBackgroundColor(255, 255, 255);
	pcl::visualization::PointCloudColorHandlerCustom<pcl::PointXYZ> single_color(cloud, 255, 0, 0); //���õ�����ɫ
	viewer->addPointCloud(cloud, single_color, "cloud");
	viewer->setPointCloudRenderingProperties(pcl::visualization::PCL_VISUALIZER_POINT_SIZE, 3, "cloud");
	viewer->addCoordinateSystem(1);  // 1.0������ϵ�Ĵ�С����
	viewer->initCameraParameters();  // ��ʼ��������ã�����
	while (!viewer->wasStopped())
	{
		viewer->spinOnce(100);
		boost::this_thread::sleep(boost::posix_time::microseconds(100000));
	}

}

void Visualize2Cloud(pcl::PointCloud<pcl::PointXYZ>::Ptr & cloud, pcl::PointCloud<pcl::PointXYZ>::Ptr & filter_cloud)
{
	//-----------------------��ʾ����-----------------------
	boost::shared_ptr<pcl::visualization::PCLVisualizer> viewer(new pcl::visualization::PCLVisualizer("��ʾ����"));

	int v1(0), v2(0);
	viewer->createViewPort(0.0, 0.0, 0.5, 1.0, v1);
	viewer->setBackgroundColor(0, 0, 0, v1);
	viewer->addText("point clouds", 10, 10, "v1_text", v1);
	viewer->createViewPort(0.5, 0.0, 1, 1.0, v2);
	viewer->setBackgroundColor(0.1, 0.1, 0.1, v2);
	viewer->addText("filtered point clouds", 10, 10, "v2_text", v2);
	// ����z�ֶν�����Ⱦ,��z��Ϊx��y��Ϊ����x��y�ֶ���Ⱦ
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



//����width!=1�ĵ���
void IntegralImageNormalEstimation(pcl::PointCloud<pcl::PointXYZ>::Ptr& cloud, pcl::PointCloud<pcl::Normal>::Ptr& cloud_normals)
{
	clock_t start = clock();

	// ���߹��ƶ���
	pcl::IntegralImageNormalEstimation<pcl::PointXYZ, pcl::Normal> normalEstimation;

	// ���߹��Ʒ���: COVARIANCE_MATRIX, AVERAGE_DEPTH_CHANGE, SIMPLE_3D_GRADIENT.
	/****************************************************************************************
	���ַ��߹��Ʒ���
	COVARIANCE_MATRIX ģʽ�Ӿ���ĳ����ľֲ������Э������󴴽�9�����֣������������ķ���
	AVERAGE_3D_GRADIENT   ģʽ����6������ͼ������ˮƽ����ʹ�ֱ�����ƽ�������ά�ݶȲ�ʹ�������ݶȼ������
	�����㷨��
	AVERAGE_DEPTH_CHANGE  ģʽֻ������һ����һ�Ļ���ͼ���Ӷ�ƽ����ȱ仯���㷨��
	********************************************************************************************/
	normalEstimation.setNormalEstimationMethod(normalEstimation.AVERAGE_3D_GRADIENT);//
																					 //������ȱ仯�ķ�ֵ
	normalEstimation.setMaxDepthChangeFactor(0.02f);//0.02
													// ���ü��㷨�ߵ�����
	normalEstimation.setNormalSmoothingSize(5.0f);//10//Խ��ԽԲ��
	normalEstimation.setViewPoint(0, 0, 100);//ͬһ����
	normalEstimation.setInputCloud(cloud);

	// ����
	normalEstimation.compute(*cloud_normals);

	//for (int i = 0; i < cloud_normals->points.size(); i++)//ͬһ����
	//{
	//	if (cloud_normals->points[i].normal_z < 0)
	//	{
	//		cloud_normals->points[i].normal_x = -(cloud_normals->points[i].normal_x);
	//		cloud_normals->points[i].normal_y = -(cloud_normals->points[i].normal_y);
	//		cloud_normals->points[i].normal_z = -(cloud_normals->points[i].normal_z);
	//	}
	//}

	double t = (double)(clock() - start) / CLOCKS_PER_SEC;
	std::cerr << "IntegralImageNormalEstimation���߹���:" << t << "s" << endl;
}


// ���ӻ����߹���
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

// ����Z֮��ĸ߶Ȳ��������ָ��ⵥһ�����ߣ������֮���������Ǹ���������ֵ������
void SegmentZ(pcl::PointCloud<pcl::PointXYZ>::Ptr& cloud, int R)
{
	// ���ø߶Ȳ�, R = 8mm

	// Xֵ��С����������
	std::vector<int> xIndex;
	// ��ȥ��������������

	for (size_t i = 50; i < (cloud->width)-50; ++i)
	{
		//cout << "��ʱѭ��������Ϊ��" << i << "/һ������Ϊ��" << cloud->width << endl;
		//cout << "��i=" << i << "�Σ�����" << endl;
		if (i >= cloud->width) { cout << "1��Խ��" << endl; }
		// Zֵ�����������Ӧ������
		std::vector<float> zArray;
		std::vector<int> zIndex;
		// �ٱ����������еĵ�
		// ����0��ʼ��⣬���ǰ������ݺ����������
		// ȡ�м�160���㣬���ǰ20�����ݺͺ�20������
		for (size_t j = 20 + (cloud->height)*i; j < (cloud->height)*(i + 1) - 20; j++)
		{
			//cout << "��j=" << j << "�Σ�����" << endl;
			if ((j + 1) % 300 == 0)
			{
				cout << "�����ˣ���" << endl;
				continue;
			}
			else if (j == cloud->size() - 1)
			{
				cout << "����ˣ�����" << endl;
				break;
			}
			else
			{
				// �жϵ�����֮���Zֵ������ֱ�������¼��ֵ�͵�������ֵ
				if ((cloud->points[j + 1].z - cloud->points[j].z) >= R)
				{
					zArray.push_back(cloud->points[j + 1].z); // ��z���ֵ����
					zIndex.push_back(j + 1);               // �ѵ��Ƶ���������zIndex
				}
				else
				{
					continue;
				}
			}
		}
		// �ж�����ֵ���������ҳ�Z���������ֵ
		if (zIndex.size() < 1) // ���û���ҵ������㣬������
		{
			//cout << "������û���ҵ���϶�㣡" << endl;
			continue;
		}
		else if (1 <= zIndex.size() < 2)
		{
			xIndex.push_back(zIndex[0]);
		}
		else
		{
			// // ��Zֵ����ǰ�������Ƚ�zArray��x����Сֵ������xIndex�����С�
			float max[2] = { 0,0 };
			int maxIndex[2] = { 0,0 };
			// ����������Zֵ
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
			// �Ƚ�Xֵ,��¼����
			// points(maxIndex[0]).z��Z��ֵ�϶��Ǵ���points(maxIndex[0]).z��Z��ֵ
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
	// ����xIndex���Ƚ����ǵ�Z�ļ�϶���ֵ,��¼������
	// ע��Zֵ��󣬲�����Zֵ�ļ�϶���
	float zMax = 0;		// ��϶���
	int zCor = 0;		// ��϶�������
	//for (size_t i = 0; i < xIndex.size(); i++)
	//{
	//	// ������С100��������
	//	cout << "������С��" << xIndex.size() << endl;
	//	cout << "��" << floor(xIndex[i] / 300) << "��\t" << "��" << xIndex[i] % 300 << "����" << endl;
	//	cout << "ZֵΪ" << cloud->points[xIndex[i]].z << endl;
	//}
	// ������ʱ����������Z������ֵ
	double temp_z = 0;
	int zCor2 = 0; // ��϶�ڶ��������
	for (size_t i = 0; i < xIndex.size(); i++)
	{
		// �����϶���ֵ��������õ������ֵ
		double temp_z1 = cloud->points[xIndex[i]].z - cloud->points[xIndex[i] - 1].z; // ��϶���㣺��������������һ��Ĳ�ֵ
		if (temp_z1>temp_z)
		{
			
			// �Ѽ�϶���ֵ����temp_z
			temp_z = temp_z1;
			// ��¼����,��������
			zMax = cloud->points[xIndex[i]].z;		// ���¼�϶�������
			if (zCor>zCor2)	zCor2 = zCor;	// ����϶���ֵ���ڶ��󣬲����µ�һ��
			zCor = xIndex[i];						// ���¼�϶�������
		}
	}
	xIndex.clear();
	// ��ӡ��϶���ֵ�����
	cout << "���ҵ�����㣬������Ϊ��[" << cloud->points[zCor].x << ","
		<< cloud->points[zCor].y << ","
		<< cloud->points[zCor].z << "]" << endl;
	cout << "����϶��ֵΪ��" << temp_z << endl;
	cout << "����������һ�������Ϊ��[" << cloud->points[zCor-1].x << ","
		<< cloud->points[zCor-1].y << ","
		<< cloud->points[zCor-1].z << "]" << endl;


	// �����걣�浽IO�ļ�
	fstream ofs;	// �����ļ���
	ofs.open("C:/Users/YSU_HYL/Desktop/zb.txt", ios::out | ios::trunc);
	if (!ofs.is_open())
	{
		cout << "���ļ�����" << endl;
		exit(1);
	}

	// �� ��������д���ļ��У�����һ��Ϊ���Ƶ㣬һ��Ϊ���ҵ㡣
	// һ�����꣬������������㣬������������ӳɻ��ߣ�һ����������XYZ��
	// ÿһ�������֮�� x�ļ��Ϊ40mm����50����������Ϊÿ��x���0.8
	if (zCor%300 > 50)									// ��ֹ����Խ�磬���Ƶ���ܻ�С������50
	{
		ofs << cloud->points[zCor - 50].x << ","
			<< cloud->points[zCor - 50].y << ","
			<< cloud->points[zCor - 50].z << ",";
	}
	else
	{
		ofs << cloud->points[zCor].x-30 << ","			// x-30 ��ԭ�����Ƶ���Ϸ�
			<< cloud->points[zCor].y << ","
			<< cloud->points[zCor].z+10 << ",";			// z+10 ��ԭ�����Ƶ������
	}

	ofs << cloud->points[zCor].x << ","					// �����Ƶ�д��
		<< cloud->points[zCor].y << ","
		<< cloud->points[zCor].z << ",";				// z �Ӳ��Ӱ뾶�ĳߴ磬��Ҫ�����ж�

	if (zCor%300 < 250)										// ��ֹ����Խ�磬���Ƶ���ܻ��������250
	{
		ofs << cloud->points[zCor + 50].x << ","
			<< cloud->points[zCor + 50].y << ","
			<< cloud->points[zCor + 50].z << ",";
	}
	else
	{
		ofs << cloud->points[zCor].x + 30 << ","			// x-30 ��ԭ�����Ƶ���·�
			<< cloud->points[zCor].y << ","
			<< cloud->points[zCor].z - 10 << ",";			// z-10 ��ԭ�����Ƶ������
	}
	// �Դˣ���һ���������

	// �ڶ��飬�������꿪ʼ

	if (zCor2%300>50)									// ��ֹ����Խ�磬���Ƶ���ܻ�С������50
	{
		ofs << cloud->points[zCor2 - 50].x << ","
			<< cloud->points[zCor2 - 50].y << ","
			<< cloud->points[zCor2 - 50].z << ",";
	}
	else
	{
		ofs << cloud->points[zCor2].x - 30 << ","			// x-30 ��ԭ�����Ƶ���Ϸ�
			<< cloud->points[zCor2].y << ","
			<< cloud->points[zCor2].z + 10 << ",";			// z+10 ��ԭ�����Ƶ������
	}

	ofs << cloud->points[zCor2].x << ","
		<< cloud->points[zCor2].y << ","
		<< cloud->points[zCor2].z << ",";

	if (zCor2%300 < 250)										// ��ֹ����Խ�磬���Ƶ���ܻ��������250
	{
		ofs << cloud->points[zCor2 + 50].x << ","
			<< cloud->points[zCor2 + 50].y << ","
			<< cloud->points[zCor2 + 50].z << endl;
	}
	else
	{
		ofs << cloud->points[zCor2].x + 30 << ","			// x-30 ��ԭ�����Ƶ���·�
			<< cloud->points[zCor2].y << ","
			<< cloud->points[zCor2].z - 10 << endl;;			// z-10 ��ԭ�����Ƶ������
	}

	ofs.close();
}