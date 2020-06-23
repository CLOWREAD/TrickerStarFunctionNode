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

namespace FunctionNode
{
    public sealed partial class MainPage : Page
    {

        private void C_CLEAR_BUTTON_Click(object sender, RoutedEventArgs e)
        {
            C_MAIN_NODE_VIEW.TS_Clear();
            m_GroupList.Clear();
            m_NodeCode.Clear();
            m_NodeGroup.Clear();
            C_NODE_GROUP_LIST.ItemsSource = null;
        }

        private void C_UNSET_GROUP_BUTTON_Click(object sender, RoutedEventArgs e)
        {
            var dc = ((Button)sender).DataContext;
            var item = (Model.TrickerStarGroupModel)dc;
            var nodes = C_MAIN_NODE_VIEW.TS_GetSelectedNodes();
            foreach (String node_name in nodes)
            {

                m_NodeGroup.Remove(node_name);
            }
        }
        /*
         
        */
        private void C_ADD_NODE_BUTTON_Click(object sender, RoutedEventArgs e)
        {
            var c_pos = C_MAIN_NODE_VIEW.TS_GetViewCenterPoint();
            String node_name = "NODE" + Model.TrickerStarDataModel.RandToken();
            C_MAIN_NODE_VIEW.TS_AddNode(node_name);
            C_MAIN_NODE_VIEW.TS_SetNodePosition(node_name, c_pos);
            String code = C_MAIN_CODE_TEXT.Text;
            int i = code.IndexOf("\r");
            while (i >= 0)

            {
                String param = code.Substring(0, i);
                code = code.Substring(i + 1);
                i = code.IndexOf("\r");
                if (param.Contains("----"))
                {
                    break;
                }
                if (param.StartsWith("@"))
                {
                    C_MAIN_NODE_VIEW.TS_SetNodeTitle(node_name, param.Substring(1));
                }
                if (param.Contains("INPUT ") && (!param.Contains(">>")))

                {

                    String slot_name = param.Split(" ", StringSplitOptions.RemoveEmptyEntries)[1];
                    String slot_type_str = param.Split(" ", StringSplitOptions.RemoveEmptyEntries)[3];
                    var slot_type = Model.TrickerStarDataModel.GetSlotTypeFromName(slot_type_str);

                    if (slot_type == Model.TrickerStarSlotType.PLACEHOLDER)
                    {
                        C_MAIN_NODE_VIEW.TS_AddSlot(node_name, Model.TrickerStarSlotType.PLACEHOLDER, "PLACE_HOLDER", Model.TrickerStarSlotSide.INPUT);
                    }
                    else
                    {
                        C_MAIN_NODE_VIEW.TS_AddSlot(node_name, slot_type, slot_name, Model.TrickerStarSlotSide.INPUT);
                    }


                }

                if (param.Contains("OUTPUT") && (!param.Contains("<<")))

                {

                    String slot_name = param.Split(" ", StringSplitOptions.RemoveEmptyEntries)[1];
                    String slot_type_str = param.Split(" ", StringSplitOptions.RemoveEmptyEntries)[3];
                    var slot_type = Model.TrickerStarDataModel.GetSlotTypeFromName(slot_type_str);
                    if (slot_type == Model.TrickerStarSlotType.PLACEHOLDER)
                    {
                        C_MAIN_NODE_VIEW.TS_AddSlot(node_name, Model.TrickerStarSlotType.PLACEHOLDER, "PLACE_HOLDER", Model.TrickerStarSlotSide.OUTPUT);
                    }
                    else
                    {
                        C_MAIN_NODE_VIEW.TS_AddSlot(node_name, slot_type, slot_name, Model.TrickerStarSlotSide.OUTPUT);
                    }

                }
            }
            m_NodeCode[node_name] = new String(code.ToCharArray());

        }

        private void C_NODE_GROUP_LIST_DropCompleted(UIElement sender, DropCompletedEventArgs args)
        {

        }

        private void C_NODE_GROUP_LIST_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Move;
        }

        private void C_NODE_GROUP_LIST_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
            C_NODE_GROUP_LIST.ItemsSource = null;
            C_NODE_GROUP_LIST.ItemsSource = m_GroupList;

        }

        private void C_NODE_GROUP_LIST_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            //
            e.Data.RequestedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Move;

        }

        private void C_SET_GROUP_BUTTON_Click(object sender, RoutedEventArgs e)
        {
            var dc = ((Button)sender).DataContext;
            var item = (Model.TrickerStarGroupModel)dc;
            var nodes = C_MAIN_NODE_VIEW.TS_GetSelectedNodes();
            foreach (String node_name in nodes)
            {

                m_NodeGroup[node_name] = new Model.TrickerStarGroupModel()
                {
                    GroupName = item.GroupName,
                    GroupTitle = item.GroupTitle,
                };
            }

        }

        private void C_DEL_GROUP_BUTTON_Click(object sender, RoutedEventArgs e)
        {
            var dc = ((Button)sender).DataContext;
            m_GroupList.Remove((Model.TrickerStarGroupModel)dc);
            C_NODE_GROUP_LIST.ItemsSource = null;
            C_NODE_GROUP_LIST.ItemsSource = m_GroupList;
        }

        private void C_ADD_GROUP_BUTTON_Click(object sender, RoutedEventArgs e)
        {
            m_GroupList.Add(new Model.TrickerStarGroupModel()
            {
                GroupTitle = C_GROUP_NAME_TEXTBOX.Text,
                GroupName = "GROUP" + Model.TrickerStarDataModel.RandToken()
            });
            C_NODE_GROUP_LIST.ItemsSource = null;
            C_NODE_GROUP_LIST.ItemsSource = m_GroupList;
        }

        private void C_NODE_GROUP_LIST_ItemClick(object sender, ItemClickEventArgs e)
        {
            var dc = ((ListView)sender);

            var item = (Model.TrickerStarGroupModel)e.ClickedItem;
            var nodes = m_NodeGroup.Keys;

            double top = Double.MaxValue, left = Double.MaxValue, right = Double.MinValue, down = Double.MinValue;
            bool SelectFlag = false;
            foreach (string node_name in nodes)
            {
                Model.TrickerStarGroupModel g = (Model.TrickerStarGroupModel)m_NodeGroup[node_name];
                if (g.GroupName.Equals(item.GroupName))
                {
                    C_MAIN_NODE_VIEW.TS_SelectNode(node_name);
                    var pos = C_MAIN_NODE_VIEW.TS_GetNodePosition(node_name);
                    left = Math.Min(left, pos.X);
                    top = Math.Min(top, pos.Y);

                    right = Math.Max(right, pos.X);
                    down = Math.Max(down, pos.Y);
                    SelectFlag = true;

                }
            }
            if (SelectFlag)
            {

                C_MAIN_NODE_VIEW.TS_FocusPos(new Point() { X = (right + left) / 2, Y = (top + down) / 2 });
            }


        }
    }
}
