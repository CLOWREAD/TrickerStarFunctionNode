using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

using System.Collections.Generic;

using System.IO;

using System.Linq;

using System.Runtime.InteropServices.WindowsRuntime;

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
    public class SlotPathFactory
    {
        public static PathGeometry NewSlotPath()
        {
            var pathGeometry1 = new PathGeometry();
            pathGeometry1.FillRule = FillRule.EvenOdd;
            var pathFigureCollection1 = new PathFigureCollection();

            var pathFigure1 = new PathFigure();

            pathFigure1.IsClosed = false;

            pathFigure1.StartPoint = new Windows.Foundation.Point(0,0);

            pathFigureCollection1.Add(pathFigure1);

            var pathFigure2 = new PathFigure();

            pathFigure2.IsClosed = true;

            pathFigure2.StartPoint = new Windows.Foundation.Point(2,2);
            pathFigureCollection1.Add(pathFigure2);

            pathGeometry1.Figures = pathFigureCollection1;



            var pathSegmentCollection1 = new PathSegmentCollection();

            var pathSegment1 = new BezierSegment();

            pathSegment1.Point1 = new Point(100,0);

            pathSegment1.Point2 = new Point(100,100);

            pathSegment1.Point3 = new Point(200,100);

            pathSegmentCollection1.Add(pathSegment1);



            pathFigure1.Segments = pathSegmentCollection1;



            var pathSegmentCollection2 = new PathSegmentCollection();

            var pathSegment2 = new PolyLineSegment();
            pathFigure2.IsFilled = true;
          
            pathSegment2.Points.Add(new Point(2, 2));
            pathSegment2.Points.Add(new Point(200, 100));
            pathSegment2.Points.Add(new Point(100, 300));
            pathSegment2.Points.Add(new Point(300, 0));
            pathSegment2.Points.Add(new Point(200, 300));
            pathSegment2.Points.Add(new Point(2, 2));
            pathSegmentCollection1.Add(pathSegment2);



            pathFigure2.Segments = pathSegmentCollection2;


            return pathGeometry1;
        }
    }
}
