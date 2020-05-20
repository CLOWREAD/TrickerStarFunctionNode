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
        public void  AddLine(Model.TrickerStarNodeSoltDetail from,Model.TrickerStarNodeSoltDetail to)
        {
            Model.TrickerStarLineModel line = new TrickerStarLineModel() { LineName="LINE"+Model.TrickerStarDataModel.RandToken(),From=from,To=to};
            
            TrickerStarFunctionNodeModel node_from_m = (TrickerStarFunctionNodeModel)m_FunctionNodeModels[line.From.NodeName];
            TrickerStarFunctionNodeModel node_to_m = (TrickerStarFunctionNodeModel)m_FunctionNodeModels[line.To.NodeName];


            DeleteLine(node_from_m.OutputSlot[from.SlotIndex].LineName);
            DeleteLine(node_to_m.InputSlot[to.SlotIndex].LineName);
            node_from_m.OutputSlot[from.SlotIndex].LineName = line.LineName;
            node_to_m.InputSlot[to.SlotIndex].LineName = line.LineName;


            line.From = node_from_m.OutputSlot[from.SlotIndex];
            line.To = node_to_m.InputSlot[to.SlotIndex];

            var geo = GenPathGeomentry(from, to);
            var path = GenPath(line.LineName, geo);


            m_FunctionNodeModels[line.From.NodeName] = node_from_m;
            m_FunctionNodeModels[line.To.NodeName] = node_to_m;
            m_FunctionLineModels[line.LineName] = line;
            m_FunctionLineViews[line.LineName] = path;


            C_MAIN_CANVAS.Children.Add(path);

        }
        public void RefreshLine(String linename)
        {
            if (linename == null) return;
            if (m_FunctionLineModels.ContainsKey(linename))
            {
                Model.TrickerStarLineModel line = (TrickerStarLineModel)m_FunctionLineModels[linename];


                TrickerStarFunctionNodeModel node_from_m = (TrickerStarFunctionNodeModel)m_FunctionNodeModels[line.From.NodeName];
                TrickerStarFunctionNodeModel node_to_m = (TrickerStarFunctionNodeModel)m_FunctionNodeModels[line.To.NodeName];
                
                Windows.UI.Xaml.Shapes.Path t_path = m_FunctionLineViews[line.LineName] as Windows.UI.Xaml.Shapes.Path;
                var geo = GenPathGeomentry(line.From, line.To);
                t_path.Data = geo;


            }
            else
            {

            }
        }
        public void DeleteLine(String linename)
        {
            if (linename == null) return;
            if (m_FunctionLineModels.ContainsKey(linename))
            {
                Model.TrickerStarLineModel line = (TrickerStarLineModel)m_FunctionLineModels[linename];
            
           
                TrickerStarFunctionNodeModel node_from_m = (TrickerStarFunctionNodeModel)m_FunctionNodeModels[line.From.NodeName];
                TrickerStarFunctionNodeModel node_to_m = (TrickerStarFunctionNodeModel)m_FunctionNodeModels[line.To.NodeName];


                node_from_m.OutputSlot[line.From.SlotIndex].LineName = null;
                node_to_m.InputSlot[line.To.SlotIndex].LineName = null;
                m_FunctionNodeModels[line.From.NodeName] = node_from_m;
                m_FunctionNodeModels[line.To.NodeName] = node_to_m;

                Windows.UI.Xaml.Shapes.Path t_path= m_FunctionLineViews[line.LineName] as Windows.UI.Xaml.Shapes.Path;
                C_MAIN_CANVAS.Children.Remove(t_path);

                m_FunctionLineModels.Remove(linename);
                m_FunctionLineViews.Remove(linename);

            }
            else
            {

            }
            


        }
        public Windows.UI.Xaml.Shapes.Path GenPath(String LineName,  PathGeometry G)
        {
            Windows.UI.Xaml.Shapes.Path t_path = new Windows.UI.Xaml.Shapes.Path();
            DoubleCollection t_path_dash = new DoubleCollection();
            t_path_dash.Add(4);
            t_path_dash.Add(2);
            t_path.Name = LineName;
            t_path.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 204, 204, 255));
            t_path.Stroke = new SolidColorBrush(Windows.UI.Colors.White);
            t_path.StrokeStartLineCap = PenLineCap.Round;
            t_path.StrokeDashOffset = 40;
            t_path.StrokeDashArray = t_path_dash;
            t_path.StrokeDashCap = PenLineCap.Round;
            t_path.StrokeThickness = 4;
            t_path.Opacity = 0.5;
            t_path.Data = G;
            return t_path;
        }
        public PathGeometry  GenPathGeomentry(Model.TrickerStarNodeSoltDetail from, Model.TrickerStarNodeSoltDetail to)
        {

            TrickerStarFunctionNodeModel node_from_m = (TrickerStarFunctionNodeModel)m_FunctionNodeModels[from.NodeName];
            TrickerStarFunctionNodeModel node_to_m = (TrickerStarFunctionNodeModel)m_FunctionNodeModels[to.NodeName];
            TrickerStarFunctionNode node_from_v = (TrickerStarFunctionNode)m_FunctionNodeViews[from.NodeName];
            TrickerStarFunctionNode node_to_v = (TrickerStarFunctionNode)m_FunctionNodeViews[to.NodeName];

            Point Line_from= new Point(node_from_m.Pos.X, node_from_m.Pos.Y), Line_to=new Point(node_to_m.Pos.X, node_to_m.Pos.Y);
            node_from_v.InvalidateMeasure();
            Line_from.X += node_from_v.ActualWidth;
            Line_from.Y += from.SlotIndex * 48 + 96;
            Line_to.Y += to.SlotIndex * 48 + 96;
            Line_from.X -= 16;
            Line_to.X += 16;


            var pathGeometry1 = new PathGeometry();

            var pathFigureCollection1 = new PathFigureCollection();

            var pathFigure1 = new PathFigure();

            pathFigure1.IsClosed = false;

            pathFigure1.StartPoint = new Windows.Foundation.Point(Line_from.X, Line_from.Y);

            pathFigureCollection1.Add(pathFigure1);

            pathGeometry1.Figures = pathFigureCollection1;



            var pathSegmentCollection1 = new PathSegmentCollection();

            var pathSegment1 = new BezierSegment();

            pathSegment1.Point1 = new Point(Line_from.X + 100, Line_from.Y);

            pathSegment1.Point2 = new Point(Line_to.X - 100, Line_to.Y);

            pathSegment1.Point3 = new Point(Line_to.X, Line_to.Y);

            pathSegmentCollection1.Add(pathSegment1);



            pathFigure1.Segments = pathSegmentCollection1;







            return pathGeometry1;
        }
        public PathGeometry GenTempPathGeomentry(Model.TrickerStarNodeSoltDetail from, Model.TrickerStarNodeSoltDetail to,Point pointer_pos)
        {
            Point Line_from, Line_to ;
            if (from==null && to!=null)
            {
                TrickerStarFunctionNode node_to_v = (TrickerStarFunctionNode)m_FunctionNodeViews[to.NodeName];
                TrickerStarFunctionNodeModel node_to_m = (TrickerStarFunctionNodeModel)m_FunctionNodeModels[to.NodeName];
                Line_to = new Point(node_to_m.Pos.X, node_to_m.Pos.Y);
                Line_from = pointer_pos;
                Line_to.Y += to.SlotIndex * 48 + 96;
            }
            if (from != null && to == null)
            {
                TrickerStarFunctionNode node_from_v = (TrickerStarFunctionNode)m_FunctionNodeViews[from.NodeName];
                TrickerStarFunctionNodeModel node_from_m = (TrickerStarFunctionNodeModel)m_FunctionNodeModels[from.NodeName];
                Line_from = new Point(node_from_m.Pos.X, node_from_m.Pos.Y);
                Line_from.X += node_from_v.ActualWidth;
                Line_from.Y += from.SlotIndex * 48 + 96;
                Line_to = pointer_pos;
            }
         

            Line_from.X -= 16;
            Line_to.X += 16;


            var pathGeometry1 = new PathGeometry();

            var pathFigureCollection1 = new PathFigureCollection();

            var pathFigure1 = new PathFigure();

            pathFigure1.IsClosed = false;

            pathFigure1.StartPoint = new Windows.Foundation.Point(Line_from.X, Line_from.Y);

            pathFigureCollection1.Add(pathFigure1);

            pathGeometry1.Figures = pathFigureCollection1;



            var pathSegmentCollection1 = new PathSegmentCollection();

            var pathSegment1 = new BezierSegment();

            pathSegment1.Point1 = new Point(Line_from.X + 100, Line_from.Y);

            pathSegment1.Point2 = new Point(Line_to.X - 100, Line_to.Y);

            pathSegment1.Point3 = new Point(Line_to.X, Line_to.Y);

            pathSegmentCollection1.Add(pathSegment1);



            pathFigure1.Segments = pathSegmentCollection1;







            return pathGeometry1;
        }



    }
}
