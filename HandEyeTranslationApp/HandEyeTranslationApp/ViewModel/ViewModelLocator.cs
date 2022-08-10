using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;


namespace HandEyeTranslationApp.ViewModel
{

    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();

            //放入容器
            SimpleIoc.Default.Register<AboutViewModel>();
            SimpleIoc.Default.Register<CameraViewModel>();
            SimpleIoc.Default.Register<PointCloudViewModel>();
            SimpleIoc.Default.Register<RobotViewModel>();
            SimpleIoc.Default.Register<TranslationViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public AboutViewModel About
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AboutViewModel>();
            }
        }

        public CameraViewModel Camera
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CameraViewModel>();
            }
        }

        public PointCloudViewModel PointCloud
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PointCloudViewModel>();
            }
        }

        public RobotViewModel Robot
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RobotViewModel>();
            }
        }

        public TranslationViewModel Translation
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TranslationViewModel>();
            }
        }





        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}
