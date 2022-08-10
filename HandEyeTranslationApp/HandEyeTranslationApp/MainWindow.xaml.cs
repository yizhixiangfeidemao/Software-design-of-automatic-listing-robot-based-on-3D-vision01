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
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Solvers;
using HandEyeTranslationApp.View;

namespace HandEyeTranslationApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new CameraView();
            this.DataContext = new PointCloudView();
            this.DataContext = new RobotView();
            this.DataContext = new TranslationView();
            this.DataContext = new AboutView();

        }


        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            container.Content = new CameraView();
        }

        private void ListBoxItem_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            container.Content = new PointCloudView();
        }

        private void ListBoxItem_MouseDoubleClick_2(object sender, MouseButtonEventArgs e)
        {
            container.Content = new RobotView();
        }

        private void ListBoxItem_MouseDoubleClick_3(object sender, MouseButtonEventArgs e)
        {
            container.Content = new TranslationView();
        }

        private void ListBoxItem_MouseDoubleClick_4(object sender, MouseButtonEventArgs e)
        {
            container.Content = new AboutView();
        }

    }
}
