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

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace FunctionNode
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            DoubleCollection t_path_dash = new DoubleCollection();

            t_path_dash.Add(4);

            t_path_dash.Add(2);


            T_Path.Data = SlotPathFactory.NewSlotPath();
            
            T_Path.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(240, 204, 204, 255));

            T_Path.Stroke = new SolidColorBrush(Windows.UI.Colors.Black);

            T_Path.StrokeStartLineCap = PenLineCap.Round;

            T_Path.StrokeDashCap = PenLineCap.Round;

            T_Path.StrokeThickness = 4;

            //C_NODE1.OnSlotClicked += C_NODE1_OnSlotClicked;
            Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += Dispatcher_AcceleratorKeyActivated;

        }
        private async void Dispatcher_AcceleratorKeyActivated(Windows.UI.Core.CoreDispatcher sender, Windows.UI.Core.AcceleratorKeyEventArgs args)

        {

            if (args.EventType.ToString().Equals("KeyDown"))

            {

               if(args.VirtualKey==Windows.System.VirtualKey.Shift)
                {
                    C_MAIN_NODE_VIEW.m_ShiftPressed = true;
                }

            }

            if (args.EventType.ToString().Equals("KeyUp"))
            {


                C_MAIN_NODE_VIEW.m_ShiftPressed = false;


            }

        }
        private void C_NODE1_OnSlotClicked(Model.TrickerStarNodeSoltDetail slot_detaiil)
        {
            
        }
    }
}
