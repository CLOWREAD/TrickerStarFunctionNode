using FunctionNode.Model;
using System;
using System.Collections;
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

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FunctionNode
{
    public sealed partial class TrickerStarNodeView : UserControl
    {

        enum ACTIVE_ELEMENT_TYPE
        {
            NODE,LINE,CANVAS
        };

        ACTIVE_ELEMENT_TYPE m_ActiveElementType=ACTIVE_ELEMENT_TYPE.CANVAS ;
        bool m_Pressed = false;
        Point m_OldPoint = new Point(0, 0);
        Point m_OldTranslation = new Point( 0, 0);
        Pointer m_CapturedPointer;
        Model.TrickerStarNodeSoltDetail m_FromSlot=null, m_ToSlot=null;
        public bool m_ShiftPressed=false;
        public Windows.UI.Xaml.Shapes.Path m_TempPath;
        public System.Collections.Hashtable m_SelectedFunctionNodeModels = new System.Collections.Hashtable();
        public System.Collections.Hashtable m_FunctionNodeModels = new System.Collections.Hashtable();
        public System.Collections.Hashtable m_FunctionLineModels = new System.Collections.Hashtable();
        public System.Collections.Hashtable m_FunctionNodeViews = new System.Collections.Hashtable();
        public System.Collections.Hashtable m_FunctionLineViews = new System.Collections.Hashtable();
        public TrickerStarNodeView()
        {
            this.InitializeComponent();
            m_TempPath = GenPath("TEMP_PATH",null);
            C_MAIN_CANVAS.Children.Add(m_TempPath);
           
        }

        public void TS_SetCanvasSize(Size size)
        {
            C_MAIN_CANVAS.Width = size.Width;
            C_MAIN_CANVAS.Height = size.Height; 
        }
         public void TS_AddNode(String NodeName)
        {
            Model.TrickerStarFunctionNodeModel node_m = new Model.TrickerStarFunctionNodeModel();
            node_m.NodeName = NodeName;

            TrickerStarFunctionNode node_v = new TrickerStarFunctionNode();
            Canvas.SetZIndex(node_v, 999);
            node_v.m_NodeName = NodeName;
            node_v.DataContext = node_m;
            node_v.SetNodeTitle(node_m.NodeName);
            C_MAIN_CANVAS.Children.Add(node_v);
            node_v.PointerPressed += Node_PointerPressed;
            node_v.PointerReleased += Node_PointerReleased;
            node_v.PointerMoved += Node_PointerMoved;
            node_v.OnSlotClicked += NODE_OnSlotClicked; 
            node_v.OnNodeClose += NODE_OnNodeClose;
            m_FunctionNodeViews.Add(NodeName, node_v);
            m_FunctionNodeModels.Add(NodeName, node_m);

          
        }
        public Model.TrickerStarFunctionNodeModel TS_GetNode(String NodeName)
        {
            Model.TrickerStarFunctionNodeModel node_m = (Model.TrickerStarFunctionNodeModel)m_FunctionNodeModels[NodeName];
            return node_m;
        }
        public Model.TrickerStarLineModel TS_GetLine(String LineName)
        {
            Model.TrickerStarLineModel line_m = (Model.TrickerStarLineModel)m_FunctionNodeModels[LineName];
            return line_m;
        }
        public void TS_AddLine(String from_node,int from_slot_index,String to_node,int to_slot_index)
        {
            if (from_node == null) return;
            if (!m_FunctionNodeModels.ContainsKey(from_node)) return;
            Model.TrickerStarFunctionNodeModel from_node_m = (Model.TrickerStarFunctionNodeModel)m_FunctionNodeModels[from_node];

            if (to_node == null) return;
            if (!m_FunctionNodeModels.ContainsKey(to_node)) return;
            Model.TrickerStarFunctionNodeModel to_node_m = (Model.TrickerStarFunctionNodeModel)m_FunctionNodeModels[to_node];


            AddLine(from_node_m.OutputSlot[from_slot_index],to_node_m.InputSlot[to_slot_index] );
        }

        public void TS_DeleteLine(String LineName)
        {
            DeleteLine(LineName);
        }
        public void TS_DeleteNode(String NodeName)
        {
            if (NodeName == null) return;
            if (!m_FunctionNodeModels.ContainsKey(NodeName)) return;
            Model.TrickerStarFunctionNodeModel node_m = (Model.TrickerStarFunctionNodeModel)m_FunctionNodeModels[NodeName];
            TrickerStarFunctionNode node_v = (TrickerStarFunctionNode)m_FunctionNodeViews[NodeName];

            foreach (var item in node_m.InputSlot)
            {
                DeleteLine(item.LineName);
            }
            foreach (var item in node_m.OutputSlot)
            {
                DeleteLine(item.LineName);
            }
            m_FunctionNodeModels.Remove(NodeName);
            m_FunctionNodeViews.Remove(NodeName);
            C_MAIN_CANVAS.Children.Remove(node_v);
            m_SelectedFunctionNodeModels.Clear();
            m_FromSlot = null;
            m_ToSlot = null;


        }
        public void TS_ClearNode(String NodeName)
        {
            if (NodeName == null) return;
            if (!m_FunctionNodeModels.ContainsKey(NodeName)) return;
            Model.TrickerStarFunctionNodeModel node_m = (Model.TrickerStarFunctionNodeModel)m_FunctionNodeModels[NodeName];
            TrickerStarFunctionNode node_v = (TrickerStarFunctionNode)m_FunctionNodeViews[NodeName];

            foreach (var item in node_m.InputSlot)
            {
                DeleteLine(item.LineName);
            }
            foreach (var item in node_m.OutputSlot)
            {
                DeleteLine(item.LineName);
            }
            node_v.ClearSlot();
            node_m.InputSlot.Clear();
            node_m.OutputSlot.Clear();


        }
        public List<String> TS_GetNodes()
        {
            List<String> list = new List<string>();
            
            foreach( object item in m_FunctionNodeModels.Keys)
            {
                list.Add(item.ToString());
            }
            return list;

        }
        public List<String> TS_GetLines()
        {
            List<String> list = new List<string>();

            foreach (object item in m_FunctionLineModels.Keys)
            {
                list.Add(item.ToString());
            }
            return list;

        }
        public List<String> TS_GetSelectedNodes()
        {
            List<String> list = new List<string>();

            foreach (object item in m_SelectedFunctionNodeModels.Keys)
            {
                list.Add(item.ToString());
            }
            return list;

        }
        public void TS_SetNodePosition(String NodeName,Point pos)
        {
            if (NodeName == null) return;
            if (!m_FunctionNodeModels.ContainsKey(NodeName)) return;
            Model.TrickerStarFunctionNodeModel node_m = (Model.TrickerStarFunctionNodeModel)m_FunctionNodeModels[NodeName];
            TrickerStarFunctionNode node_v = (TrickerStarFunctionNode)m_FunctionNodeViews[NodeName];

            
            node_v.Translation = new System.Numerics.Vector3((float)(node_v.Translation.X + pos.X), (float)(node_v.Translation.Y + pos.Y), node_v.Translation.Z);
            node_m.Pos.X = node_v.Translation.X;
            node_m.Pos.Y = node_v.Translation.Y;
        }

        public void TS_AddSlot(String NodeName,String typename,String slotname,Model.TrickerStarSlotSide side,bool placeholder=false)
        {
            if (NodeName == null) return;
            if (!m_FunctionNodeModels.ContainsKey(NodeName)) return;
            Model.TrickerStarFunctionNodeModel node_m = (Model.TrickerStarFunctionNodeModel)m_FunctionNodeModels[NodeName];
            TrickerStarFunctionNode node_v = (TrickerStarFunctionNode)m_FunctionNodeViews[NodeName];
            if(side==TrickerStarSlotSide.INPUT)
            {
                node_v.AddInpusStack(typename, slotname, placeholder);
                //node_v.AddInputLabel(typename,placeholder);
                node_m.InputSlot.Add(new TrickerStarNodeSoltDetail()
                {
                    NodeName = node_v.m_NodeName,
                    SlotIndex = node_m.InputSlot.Count,
                    SlotSide = TrickerStarSlotSide.INPUT,
                    SlotName = slotname,
                    SlotType = typename,
                });

            }
            if (side == TrickerStarSlotSide.OUTPUT)
            {
                node_v.AddOutpusStack(typename, slotname, placeholder);
               // node_v.AddOutputLabel(typename,placeholder);
                node_m.OutputSlot.Add(new TrickerStarNodeSoltDetail()
                {
                    NodeName = node_v.m_NodeName,
                    SlotIndex = node_m.OutputSlot.Count,
                    SlotSide = TrickerStarSlotSide.OUTPUT,
                    SlotName = slotname,
                    SlotType = typename,
                });
            }
            m_FunctionNodeModels[NodeName]= node_m;
            m_FunctionNodeViews[NodeName]= node_v;

        }



        private void C_MAIN_CANVAS_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            m_ActiveElementType = ACTIVE_ELEMENT_TYPE.CANVAS;
            m_Pressed = true;

            m_OldPoint = e.GetCurrentPoint(this).Position;

            m_OldTranslation.X=C_MAINSCROLLVIEWER.HorizontalOffset;
            m_OldTranslation.Y = C_MAINSCROLLVIEWER.VerticalOffset;

            m_CapturedPointer = e.Pointer;

            UnselectNodes();
            m_SelectedFunctionNodeModels.Clear();

            m_FromSlot = null; m_ToSlot = null;
            m_TempPath.Visibility = Visibility.Collapsed;
            //C_MAIN_CANVAS.CapturePointer(e.Pointer);
        }

        private void C_MAIN_CANVAS_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (m_Pressed && m_ActiveElementType==ACTIVE_ELEMENT_TYPE.CANVAS)

            {

                var t = this.m_OldTranslation;



                t.X = (float)(e.GetCurrentPoint(this).Position.X - m_OldPoint.X );
                t.X /= C_MAINSCROLLVIEWER.ZoomFactor;
                t.X = -t.X + m_OldTranslation.X;

                t.Y = (float)(e.GetCurrentPoint(this).Position.Y - m_OldPoint.Y );
                t.Y /= C_MAINSCROLLVIEWER.ZoomFactor;
                t.Y = -t.Y + m_OldTranslation.Y;


                m_OldPoint = e.GetCurrentPoint(this).Position;




                m_OldTranslation = t;


               

                C_MAINSCROLLVIEWER.ChangeView(m_OldTranslation.X, m_OldTranslation.Y, C_MAINSCROLLVIEWER.ZoomFactor);


            }
            if ((m_FromSlot == null && m_ToSlot != null) || (m_FromSlot != null && m_ToSlot == null))
            {
                m_TempPath.Data = GenTempPathGeomentry(m_FromSlot, m_ToSlot, e.GetCurrentPoint(C_MAIN_CANVAS).Position);
                m_TempPath.Visibility = Visibility.Visible;
            }
            else
            {
                m_TempPath.Visibility = Visibility.Collapsed;
            }
        }

        private void C_MAIN_CANVAS_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            m_Pressed = false;
            m_ActiveElementType = ACTIVE_ELEMENT_TYPE.CANVAS;
            //C_MAIN_CANVAS.ReleasePointerCapture(m_CapturedPointer);
        }
        void UnselectNodes()
        {
            foreach (object node_str in m_SelectedFunctionNodeModels.Keys)
            {
                TrickerStarFunctionNodeModel node_m = (TrickerStarFunctionNodeModel)m_SelectedFunctionNodeModels[node_str];
                TrickerStarFunctionNode node_v = (TrickerStarFunctionNode)m_FunctionNodeViews[node_str];
                node_v.Select(false);
                Canvas.SetZIndex(node_v, 4);

            }
        }
    }
}
