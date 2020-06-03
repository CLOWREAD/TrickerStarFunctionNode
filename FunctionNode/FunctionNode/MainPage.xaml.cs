using FunctionNode.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace FunctionNode
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        TrickerStarPresetDialog m_PresetDlg = new TrickerStarPresetDialog();
        List<Model.TrickerStarPresetCodeModel> m_PresetList = new List<Model.TrickerStarPresetCodeModel>();
        List<Model.TrickerStarGroupModel> m_GroupList = new List<Model.TrickerStarGroupModel>();
        //List<Model.TrickerStarNodeGroupModel> m_NodeGroupList = new List<Model.TrickerStarNodeGroupModel>();
        public System.Collections.Hashtable m_NodeCode = new System.Collections.Hashtable();

        //Key :String Value: Model.TrickerStarNodeGroupModel
        public System.Collections.Hashtable m_NodeGroup = new System.Collections.Hashtable();
        public MainPage()
        {
            this.InitializeComponent();
            this.Dispatcher.AcceleratorKeyActivated += Dispatcher_AcceleratorKeyActivated;
            

            C_NODE_GROUP_LIST.ItemsSource = m_GroupList;
            m_PresetDlg.OnPresetSelected += M_PresetDlg_OnPresetSelected;
            m_PresetDlg.Closed += M_PresetDlg_Closed;
        }


    }
    
}
