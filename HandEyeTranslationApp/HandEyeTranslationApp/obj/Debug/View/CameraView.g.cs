﻿#pragma checksum "..\..\..\View\CameraView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9D3E05E7CDC59D5EF7309E893D7E1F6B1C001555"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using HandEyeTranslationApp.View;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace HandEyeTranslationApp.View {
    
    
    /// <summary>
    /// CameraView
    /// </summary>
    public partial class CameraView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 21 "..\..\..\View\CameraView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCameraLine;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\View\CameraView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCameraOff;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\View\CameraView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnShoot;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\View\CameraView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSaveFile;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\View\CameraView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnPointCloudLoading;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\View\CameraView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txbBall;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\View\CameraView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnEnterBallRadius;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\View\CameraView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnFitting;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\View\CameraView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txbX;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\View\CameraView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txbY;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\View\CameraView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txbZ;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/HandEyeTranslationApp;component/view/cameraview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\View\CameraView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.btnCameraLine = ((System.Windows.Controls.Button)(target));
            
            #line 21 "..\..\..\View\CameraView.xaml"
            this.btnCameraLine.Click += new System.Windows.RoutedEventHandler(this.btnCameraLine_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btnCameraOff = ((System.Windows.Controls.Button)(target));
            return;
            case 3:
            this.btnShoot = ((System.Windows.Controls.Button)(target));
            return;
            case 4:
            this.btnSaveFile = ((System.Windows.Controls.Button)(target));
            return;
            case 5:
            this.btnPointCloudLoading = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\..\View\CameraView.xaml"
            this.btnPointCloudLoading.Click += new System.Windows.RoutedEventHandler(this.btnPointCloudLoading_Click_1);
            
            #line default
            #line hidden
            return;
            case 6:
            this.txbBall = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.btnEnterBallRadius = ((System.Windows.Controls.Button)(target));
            return;
            case 8:
            this.btnFitting = ((System.Windows.Controls.Button)(target));
            return;
            case 9:
            this.txbX = ((System.Windows.Controls.TextBox)(target));
            return;
            case 10:
            this.txbY = ((System.Windows.Controls.TextBox)(target));
            return;
            case 11:
            this.txbZ = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

