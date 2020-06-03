using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace FunctionNode
{
    public sealed partial class TrickerStarPresetDialog : ContentDialog
    {
        public   delegate void PresetSelected(String Code);
        public event PresetSelected OnPresetSelected;
        public List<Model.TrickerStarPresetCodeModel> m_List=null;
        public TrickerStarPresetDialog()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void DEL_BUTTON_Click(object sender, RoutedEventArgs e)
        {
            var dc = ((Button)sender).DataContext;
            var item = (Model.TrickerStarPresetCodeModel)dc;
            m_List.Remove(item);
            C_NODE_PRESET_LIST.ItemsSource = null;
            C_NODE_PRESET_LIST.ItemsSource = m_List;


        }

        public void SetList(List< Model.TrickerStarPresetCodeModel> list)
        {
            m_List = list;
            C_NODE_PRESET_LIST.ItemsSource = null;
            C_NODE_PRESET_LIST.ItemsSource = m_List;
        }

        private void C_NODE_PRESET_LIST_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
            C_NODE_PRESET_LIST.ItemsSource = null;
            C_NODE_PRESET_LIST.ItemsSource = m_List;
        }

        private void C_NODE_PRESET_LIST_ItemClick(object sender, ItemClickEventArgs e)
        {
            if(OnPresetSelected!=null)
            {
                OnPresetSelected.Invoke(((Model.TrickerStarPresetCodeModel)e.ClickedItem).OriginalCode);
                this.Hide();
            }
        }

        private void C_CANCELBUTTON_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
