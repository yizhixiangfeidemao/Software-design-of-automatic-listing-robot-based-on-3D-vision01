#pragma once
#pragma once
#include <iostream>
#include <pcl/point_types.h>
#include <pcl/io/pcd_io.h>
#include <pcl/filters/conditional_removal.h> //�����˲�
#include <pcl/visualization/pcl_visualizer.h>
#include <boost/thread/thread.hpp>

#include <pcl/features/integral_image_normal.h>
#include <pcl/features/normal_3d.h>
#include <pcl/features/normal_3d_omp.h>
#include <pcl/surface/mls.h>
#include<pcl/features/principal_curvatures.h>
#include <pcl/features/boundary.h>

using namespace std;

// �����˲�
pcl::PointCloud<pcl::PointXYZ>::Ptr ConditionalRemoval(pcl::PointCloud<pcl::PointXYZ>::Ptr& cloud, bool b);
// �Ƴ�Nan��
void removeNan(pcl::PointCloud<pcl::PointXYZ>::Ptr& cloud);
// ��һ���ƿ��ӻ�
void VisualizeCloud(pcl::PointCloud<pcl::PointXYZ>::Ptr& cloud);
// �˲����ƶԱȿ��ӻ�
void Visualize2Cloud(pcl::PointCloud<pcl::PointXYZ>::Ptr& cloud, pcl::PointCloud<pcl::PointXYZ>::Ptr& filter_cloud);
// ����ͼ���߹���
void IntegralImageNormalEstimation(pcl::PointCloud<pcl::PointXYZ>::Ptr& cloud, pcl::PointCloud<pcl::Normal>::Ptr& cloud_normals);
// ���ӻ����߹���
void VisualizeNormalCloud(pcl::PointCloud<pcl::PointXYZ>::Ptr& cloud, pcl::PointCloud<pcl::Normal>::Ptr& normals);
// ����Z֮��ĸ߶Ȳ��������ָ��ⵥһ�����ߣ������֮���������Ǹ���������ֵ������
void SegmentZ(pcl::PointCloud<pcl::PointXYZ>::Ptr& cloud, int R);