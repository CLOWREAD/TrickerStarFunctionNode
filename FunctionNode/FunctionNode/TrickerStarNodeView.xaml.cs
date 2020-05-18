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
            AddNode("NODE_0x01");
            AddNode("NODE_0x02");
            AddNode("NODE_0x03");
            // C_MAIN_CANVAS.
            AddSlot("NODE_0x01", "EXECUTE", "Input1", TrickerStarSlotSide.INPUT);
            AddSlot("NODE_0x01", "int", "Input2", TrickerStarSlotSide.INPUT);
            AddSlot("NODE_0x01", "double", "Input3", TrickerStarSlotSide.INPUT);
            AddSlot("NODE_0x01", "String", "Input4", TrickerStarSlotSide.INPUT);
            AddSlot("NODE_0x01", "EXECUTE", "Output1", TrickerStarSlotSide.OUTPUT);
            AddSlot("NODE_0x01", "int", "Output2", TrickerStarSlotSide.OUTPUT);
            AddSlot("NODE_0x01", "double", "Output3", TrickerStarSlotSide.OUTPUT);
            AddSlot("NODE_0x01", "String", "Output4", TrickerStarSlotSide.OUTPUT);
            AddSlot("NODE_0x02", "EXECUTE", "Input1", TrickerStarSlotSide.INPUT);
            AddSlot("NODE_0x02", "int", "Input2", TrickerStarSlotSide.INPUT);
            AddSlot("NODE_0x02", "double", "Input3", TrickerStarSlotSide.INPUT);
            AddSlot("NODE_0x02", "String", "Input4", TrickerStarSlotSide.INPUT);
            AddSlot("NODE_0x02", "EXECUTE", "Output1", TrickerStarSlotSide.OUTPUT);
            AddSlot("NODE_0x02", "int", "Output2", TrickerStarSlotSide.OUTPUT);
            AddSlot("NODE_0x02", "double", "Output3", TrickerStarSlotSide.OUTPUT);
            AddSlot("NODE_0x02", "String", "Output4", TrickerStarSlotSide.OUTPUT);
            AddSlot("NODE_0x03", "EXECUTE", "Input1", TrickerStarSlotSide.INPUT);
            AddSlot("NODE_0x03", "int", "Input2", TrickerStarSlotSide.INPUT);
            AddSlot("NODE_0x03", "double", "Input3", TrickerStarSlotSide.INPUT);
            AddSlot("NODE_0x03", "String", "Input4", TrickerStarSlotSide.INPUT);
            AddSlot("NODE_0x03", "EXECUTE", "Output1", TrickerStarSlotSide.OUTPUT);
            AddSlot("NODE_0x03", "int", "Output2", TrickerStarSlotSide.OUTPUT);
            AddSlot("NODE_0x03", "double", "Output3", TrickerStarSlotSide.OUTPUT);
            AddSlot("NODE_0x03", "String", "Output4", TrickerStarSlotSide.OUTPUT);
        }
         public void AddNode(String NodeName)
        {
            Model.TrickerStarFunctionNodeModel node_m = new Model.TrickerStarFunctionNodeModel();
            node_m.NodeName = NodeName;

            TrickerStarFunctionNode node_v = new TrickerStarFunctionNode();
            Canvas.SetZIndex(node_v, 999);
            node_v.m_NodeName = NodeName;
            node_v.DataContext = node_m;
            node_v.SetNodeName(node_m.NodeName);
            C_MAIN_CANVAS.Children.Add(node_v);
            node_v.PointerPressed += Node_PointerPressed;
            node_v.PointerReleased += Node_PointerReleased;
            node_v.PointerMoved += Node_PointerMoved;
            node_v.OnSlotClicked += NODE_OnSlotClicked;
            node_v.OnNodeClose += NODE_OnNodeClose;
            m_FunctionNodeViews.Add(NodeName, node_v);
            m_FunctionNodeModels.Add(NodeName, node_m);

          
        }
        public void AddSlot(String NodeName,String typename,String slotname,Model.TrickerStarSlotSide side)
        {
            if (NodeName == null) return;
            if (!m_FunctionNodeModels.ContainsKey(NodeName)) return;
            Model.TrickerStarFunctionNodeModel node_m = (Model.TrickerStarFunctionNodeModel)m_FunctionNodeModels[NodeName];
            TrickerStarFunctionNode node_v = (TrickerStarFunctionNode)m_FunctionNodeViews[NodeName];
            if(side==TrickerStarSlotSide.INPUT)
            {
                node_v.AddInpusStack(typename, slotname);
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
                node_v.AddOutpusStack(typename, slotname);
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
                m_TempPath.Data = GenTempPathGeomentry(m_FromSlot, m_ToSlot, e.GetCurrentPoint(this).Position);
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
            }
        }
    }
}
