﻿#pragma checksum "..\..\Login.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8A3974FE2B00D7364D6CC73A4F90CC2A"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using ChatRoomClient;
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


namespace ChatRoomClient {
    
    
    /// <summary>
    /// Login
    /// </summary>
    public partial class Login : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblAccount;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbxAccount;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblPassWord;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox pbxPassWord;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSetIn;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSetUp;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbxRememberAccount;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbxRememberPassWord;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblNick;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\Login.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbxNick;
        
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
            System.Uri resourceLocater = new System.Uri("/ChatRoomClient;component/login.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Login.xaml"
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
            this.lblAccount = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.tbxAccount = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.lblPassWord = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.pbxPassWord = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 5:
            this.btnSetIn = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\Login.xaml"
            this.btnSetIn.Click += new System.Windows.RoutedEventHandler(this.btnSetIn_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnSetUp = ((System.Windows.Controls.Button)(target));
            
            #line 17 "..\..\Login.xaml"
            this.btnSetUp.Click += new System.Windows.RoutedEventHandler(this.btnSetUp_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.cbxRememberAccount = ((System.Windows.Controls.CheckBox)(target));
            
            #line 18 "..\..\Login.xaml"
            this.cbxRememberAccount.Checked += new System.Windows.RoutedEventHandler(this.cbxRememberAccount_Checked);
            
            #line default
            #line hidden
            
            #line 18 "..\..\Login.xaml"
            this.cbxRememberAccount.Unchecked += new System.Windows.RoutedEventHandler(this.cbxRememberAccount_Unchecked);
            
            #line default
            #line hidden
            return;
            case 8:
            this.cbxRememberPassWord = ((System.Windows.Controls.CheckBox)(target));
            
            #line 19 "..\..\Login.xaml"
            this.cbxRememberPassWord.Unchecked += new System.Windows.RoutedEventHandler(this.cbxRememberPassWord_Unchecked);
            
            #line default
            #line hidden
            
            #line 19 "..\..\Login.xaml"
            this.cbxRememberPassWord.Checked += new System.Windows.RoutedEventHandler(this.cbxRememberPassWord_Checked);
            
            #line default
            #line hidden
            return;
            case 9:
            this.lblNick = ((System.Windows.Controls.Label)(target));
            return;
            case 10:
            this.tbxNick = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

