using FunctionNode.Model;
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

namespace FunctionNode
{
    public sealed partial class TrickerStarNodeView : UserControl
    {
        private void Node_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            m_ActiveElementType = ACTIVE_ELEMENT_TYPE.NODE;

           
            if (m_Pressed && m_ActiveElementType == ACTIVE_ELEMENT_TYPE.NODE)
            {
                e.Handled = true;
                var t = this.m_OldTranslation;



                t.X = (float)(e.GetCurrentPoint(this).Position.X - m_OldPoint.X);
                t.X /= C_MAINSCROLLVIEWER.ZoomFactor;

                t.Y = (float)(e.GetCurrentPoint(this).Position.Y - m_OldPoint.Y);
                t.Y /= C_MAINSCROLLVIEWER.ZoomFactor;



                m_OldPoint = e.GetCurrentPoint(this).Position;




                m_OldTranslation = t;

                foreach (object node in m_SelectedFunctionNodeModels.Keys)
                {
                    TrickerStarFunctionNodeModel node_m = (TrickerStarFunctionNodeModel)m_SelectedFunctionNodeModels[node];
                    TrickerStarFunctionNode node_v = (TrickerStarFunctionNode)m_FunctionNodeViews[node];
                    node_v.Translation = new System.Numerics.Vector3((float)(node_v.Translation.X + t.X), (float)(node_v.Translation.Y + t.Y), node_v.Translation.Z);
                    node_m.Pos.X = node_v.Translation.X;
                    node_m.Pos.Y = node_v.Translation.Y;
                    m_FunctionNodeModels[node] = node_m;

                    foreach (var linename_i in node_m.InputSlot)
                    {
                        RefreshLine(linename_i.LineName);
                    }
                    foreach (var linename_o in node_m.OutputSlot)
                    {
                        RefreshLine(linename_o.LineName);
                    }

                }



            }
        }

        private void Node_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            m_ActiveElementType = ACTIVE_ELEMENT_TYPE.CANVAS;
            TrickerStarFunctionNode node = sender as TrickerStarFunctionNode;
            m_Pressed = false;
            e.Handled = true;
        }

        private void Node_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (m_ShiftPressed == false)
            {
                UnselectNodes();
                m_SelectedFunctionNodeModels.Clear();
            }
            m_Pressed = true;
            m_OldPoint = e.GetCurrentPoint(this).Position;

            m_OldTranslation.X = C_MAINSCROLLVIEWER.HorizontalOffset;
            m_OldTranslation.Y = C_MAINSCROLLVIEWER.VerticalOffset;

            m_ActiveElementType = ACTIVE_ELEMENT_TYPE.NODE;
            TrickerStarFunctionNode node = sender as TrickerStarFunctionNode;
            Model.TrickerStarFunctionNodeModel node_m = node.DataContext as TrickerStarFunctionNodeModel;
            m_SelectedFunctionNodeModels[node_m.NodeName]= node_m;
            node.Select(true);
            e.Handled = true;
        }
        private void NODE_OnSlotClicked(Model.TrickerStarNodeSoltDetail slot_detail)
        {

            DeleteLine(slot_detail.LineName);
            if(slot_detail.SlotSide==TrickerStarSlotSide.INPUT)
            {
                if (m_FromSlot != null)
                {
                    if (m_FromSlot.SlotType.Equals(slot_detail.SlotType))
                    {
                        m_ToSlot = slot_detail;
                    }
                }
                else
                {
                    m_ToSlot = slot_detail;
                }
               
            }
            if (slot_detail.SlotSide == TrickerStarSlotSide.OUTPUT)
            {
                if(m_ToSlot!=null)
                {
                    if(m_ToSlot.SlotType.Equals(slot_detail.SlotType))
                    {
                    m_FromSlot = slot_detail;
                    }
                }
                else
                {
                    m_FromSlot = slot_detail;
                }
            }
            ///////////////////////////////////////////////////////////
            if(m_FromSlot!=null && m_ToSlot!=null)
            {
                AddLine(m_FromSlot, m_ToSlot);
                m_FromSlot = null; m_ToSlot = null;
                m_TempPath.Visibility = Visibility.Collapsed;
            }
            if (m_FromSlot == null && m_ToSlot == null)
            {
                //m_TempPath.Visibility = Visibility.Collapsed;
            }
            if ((m_FromSlot==null && m_ToSlot!=null)|| (m_FromSlot != null && m_ToSlot == null))
            {
                //m_TempPath.Visibility = Visibility.Visible;
            }
        }
        private void NODE_OnNodeClose(String name)
        {
            if (name == null) return;
            if (!m_FunctionNodeModels.ContainsKey(name)) return;
            TrickerStarFunctionNodeModel node_m = (TrickerStarFunctionNodeModel)m_FunctionNodeModels[name];
            TrickerStarFunctionNode node_v = (TrickerStarFunctionNode)m_FunctionNodeViews[name];
            foreach(var item in node_m.InputSlot)
            {
                DeleteLine(item.LineName);
            }
            foreach (var item in node_m.OutputSlot)
            {
                DeleteLine(item.LineName);
            }
            m_FunctionNodeModels.Remove(name);
            m_FunctionNodeViews.Remove(name);
            C_MAIN_CANVAS.Children.Remove(node_v);
            m_SelectedFunctionNodeModels.Clear();
            m_FromSlot = null;
            m_ToSlot = null; 
        }
    }
}
