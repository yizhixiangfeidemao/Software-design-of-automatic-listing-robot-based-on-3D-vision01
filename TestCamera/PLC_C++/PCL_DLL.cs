using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace PCL
{
    class PCL_DLL
    {
        [DllImport(@"D:/GuaPai/PCL_test1.8.1/DLL_PointCloudLib/DLL_PointCloudLib/Debug/DLL_PointCloudLib.dll", EntryPoint = "CoordinatePoint",CallingConvention =CallingConvention.Cdecl)]
        public extern static void CoordinatePoint(float R);
    }
}
