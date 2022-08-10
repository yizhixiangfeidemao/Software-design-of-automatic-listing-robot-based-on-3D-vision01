using System.IO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HandEyeTranslationApp.View
{
    /// <summary>
    /// CameraView.xaml 的交互逻辑
    /// </summary>
    public partial class CameraView : UserControl
    {
        public CameraView()
        {
            InitializeComponent();
        }

        private void btnCameraLine_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPointCloudLoading_Click(object sender, RoutedEventArgs e)
        {

            Console.WriteLine("你点击的按钮！");
            List<float> PointData = new List<float>();//实例化单条轮廓的点云数据
            OpenFileDialog op = new OpenFileDialog();//实例化打开对画框
            //if (op.ShowDialog() == DialogResult.OK)//选择的文件名有效
            //{
            //    //创建一个文件流
            //    FileStream fs = new FileStream(op.FileName, FileMode.Open, FileAccess.Read, FileShare.None);
            //    //创建读这个流的对象，第一个参数是文件流，第二个参数是编码（其实里面的值是多少对我们这个读没有什么问题）
            //    StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding(936));

            //    string str = "";
            //    //string fileName = op 
            //    //System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite);
            //}
        }




        private void btnPointCloudLoading_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("你点击了按钮！");
        }
    }
}
