#pragma once
#pragma once
#include <iostream>
#include <pcl/point_types.h>
#include <pcl/io/pcd_io.h>
#include <pcl/filters/conditional_removal.h> //条件滤波
#include <pcl/visualization/pcl_visualizer.h>
#include <boost/thread/thread.hpp>

#include <pcl/features/integral_image_normal.h>
#include <pcl/features/normal_3d.h>
#include <pcl/features/normal_3d_omp.h>
#include <pcl/surface/mls.h>
#include<pcl/features/principal_curvatures.h>
#include <pcl/features/boundary.h>

using namespace std;

// 条件滤波
pcl::PointCloud<pcl::PointXYZ>::Ptr ConditionalRemoval(pcl::PointCloud<pcl::PointXYZ>::Ptr& cloud, bool b);
// 移除Nan点
void removeNan(pcl::PointCloud<pcl::PointXYZ>::Ptr& cloud);
// 单一点云可视化
void VisualizeCloud(pcl::PointCloud<pcl::PointXYZ>::Ptr& cloud);
// 滤波点云对比可视化
void Visualize2Cloud(pcl::PointCloud<pcl::PointXYZ>::Ptr& cloud, pcl::PointCloud<pcl::PointXYZ>::Ptr& filter_cloud);
// 积分图法线估计
void IntegralImageNormalEstimation(pcl::PointCloud<pcl::PointXYZ>::Ptr& cloud, pcl::PointCloud<pcl::Normal>::Ptr& cloud_normals);
// 可视化法线估计
void VisualizeNormalCloud(pcl::PointCloud<pcl::PointXYZ>::Ptr& cloud, pcl::PointCloud<pcl::Normal>::Ptr& normals);
// 利用Z之间的高度差进行区域分割，检测单一轮廓线，点与点之间间距最大的那个点云坐标值！！！
void SegmentZ(pcl::PointCloud<pcl::PointXYZ>::Ptr& cloud, int R);