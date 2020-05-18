using System;
using System.Collections.Generic;
using System.Drawing;
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
    public sealed partial class TrickerStarFunctionNode : UserControl
    {
        public String m_NodeName="MAIN";
        public delegate void Delegate_SlotClicked(Model.TrickerStarNodeSoltDetail slot_detaiil);
        public event Delegate_SlotClicked OnSlotClicked ;
        public delegate void Delegate_NodeClose(String name);
        public event Delegate_NodeClose OnNodeClose;
        List<Model.TrickerStarNodeSolt> m_InputSlot = new List<Model.TrickerStarNodeSolt>();
        List<Model.TrickerStarNodeSolt> m_OutputSlot = new List<Model.TrickerStarNodeSolt>();
        public TrickerStarFunctionNode()
        {
            this.InitializeComponent();

            C_NODE_NAME.Text = m_NodeName;
        }
        public void AddInpusStack(String type_str,String name_str, bool place_holder = false)
        {
            var slot = new Model.TrickerStarNodeSolt() { SlotName = name_str, SlotType = type_str, SlotSide = Model.TrickerStarSlotSide.INPUT, SlotIndex = m_InputSlot.Count };
            m_InputSlot.Add(slot);

            /*  <Grid MaxHeight="48" Margin="0,0,0,0" >
                    < Grid.RowDefinitions >
                        < RowDefinition Height = "12" />
 
                         < RowDefinition Height = "24" />
  
                      </ Grid.RowDefinitions >
  
                      < TextBlock FontSize = "6" HorizontalAlignment = "Stretch" VerticalAlignment = "Stretch" Height = "16" > jdfghjk </ TextBlock >
         
                             < TextBlock FontSize = "12" HorizontalAlignment = "Center" VerticalAlignment = "Center" Height = "24" Grid.Row = "1" FontWeight = "Bold" > jdfghjk </ TextBlock >
                    

                </ Grid >
         */

            Grid slot_grid = new Grid();
            slot_grid.MaxHeight = 48;
            slot_grid.MinHeight = 48;
            if (place_holder)
            {
                C_INPUT_STACK.Children.Add(slot_grid);
                return;
            }

            slot_grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            slot_grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            slot_grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(2, GridUnitType.Star) });

            Border border = new Border();
            border.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0));
            TextBlock var_type = new TextBlock();
            var_type.FontSize = 12;
            var_type.HorizontalAlignment = HorizontalAlignment.Left;
            var_type.VerticalAlignment = VerticalAlignment.Stretch;
            var_type.Text = type_str;
            var_type.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255));
            border.Child = var_type;
            border.HorizontalAlignment = HorizontalAlignment.Left;
            border.CornerRadius = new CornerRadius(4);
            border.Padding = new Thickness(4, 0, 4, 0);
            border.Margin = new Thickness(8, 0, 0, 0);
            border.BorderBrush = new SolidColorBrush(GetTypeColor(type_str));
            border.BorderThickness = new Thickness(2);

            TextBlock var_name = new TextBlock();

            var_name.FontSize = 12;
            var_name.FontWeight = new Windows.UI.Text.FontWeight() { Weight = 700 };
            var_name.HorizontalAlignment = HorizontalAlignment.Center;
            var_name.VerticalAlignment = VerticalAlignment.Stretch;
            var_name.Text = name_str;
            var_name.Margin = new Thickness(0, 0, 0, 0);
            var_name.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255));
            Border name_border = new Border();
            name_border.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0));
            name_border.BorderBrush = new SolidColorBrush(GetTypeColor(type_str));
            name_border.BorderThickness = new Thickness(2);
            name_border.Margin = new Thickness(4);
            name_border.CornerRadius = new CornerRadius(4);
            name_border.Child = var_name;

            Grid.SetRow(name_border, 1);
            Grid.SetRowSpan(name_border, 2);
            slot_grid.Children.Add(name_border);

            Grid.SetRow(border, 0);
            Grid.SetRowSpan(border, 2);
            slot_grid.Children.Add(border);
            C_INPUT_STACK.Children.Add(slot_grid);
            slot_grid.DataContext = slot;
            slot_grid.PointerPressed += OnSlotPressed;

        }
        public void AddOutpusStack(String type_str, String name_str,bool place_holder=false)
        {
            var slot = new Model.TrickerStarNodeSolt() { SlotName = name_str, SlotType = type_str,SlotSide=Model.TrickerStarSlotSide.OUTPUT,SlotIndex=m_OutputSlot.Count };
            m_OutputSlot.Add(slot);
            /*  <Grid MaxHeight="48" Margin="0,0,0,0" >
                    < Grid.RowDefinitions >
                        < RowDefinition Height = "12" />
 
                         < RowDefinition Height = "24" />
  
                      </ Grid.RowDefinitions >
  
                      < TextBlock FontSize = "6" HorizontalAlignment = "Stretch" VerticalAlignment = "Stretch" Height = "16" > jdfghjk </ TextBlock >
         
                             < TextBlock FontSize = "12" HorizontalAlignment = "Center" VerticalAlignment = "Center" Height = "24" Grid.Row = "1" FontWeight = "Bold" > jdfghjk </ TextBlock >
                    

                </ Grid >
         */

            Grid slot_grid = new Grid();
            slot_grid.MaxHeight = 48;
            slot_grid.MinHeight = 48;

            if(place_holder)
            {
                C_OUTPUT_STACK.Children.Add(slot_grid);
                return;
            }


            slot_grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            slot_grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            slot_grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(2, GridUnitType.Star) });

            Border border = new Border();
            border.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0));
            TextBlock var_type = new TextBlock();
            var_type.FontSize = 12;
            var_type.HorizontalAlignment = HorizontalAlignment.Right;
            var_type.VerticalAlignment = VerticalAlignment.Stretch;
            var_type.Text = type_str;
            var_type.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255));
            border.Child = var_type;
            border.HorizontalAlignment = HorizontalAlignment.Right;
            border.CornerRadius = new CornerRadius(4);
            border.Padding = new Thickness(4, 0, 4, 0);
            border.Margin = new Thickness(0, 0, 8, 0);
            border.BorderBrush = new SolidColorBrush(GetTypeColor(type_str));
            border.BorderThickness = new Thickness(2);

            TextBlock var_name = new TextBlock();
            
            var_name.FontSize = 12;
            var_name.FontWeight = new Windows.UI.Text.FontWeight() { Weight = 700 };
            var_name.HorizontalAlignment = HorizontalAlignment.Center;
            var_name.VerticalAlignment = VerticalAlignment.Stretch;
            var_name.Text = name_str;
            var_name.Margin = new Thickness(0, 0, 0, 0);
            var_name.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255));
            Border name_border = new Border();
            name_border.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0));
            name_border.BorderBrush = new SolidColorBrush(GetTypeColor(type_str));
            name_border.BorderThickness = new Thickness(2);
            name_border.Margin = new Thickness(4);
            name_border.CornerRadius = new CornerRadius(4);
            name_border.Child = var_name;

            Grid.SetRow(name_border, 1);
            Grid.SetRowSpan(name_border, 2);
            slot_grid.Children.Add(name_border);

            Grid.SetRow(border, 0);
            Grid.SetRowSpan(border, 2);
            slot_grid.Children.Add(border);

            C_OUTPUT_STACK.Children.Add(slot_grid);
            slot_grid.DataContext = slot;
            slot_grid.PointerPressed += OnSlotPressed;

        }
        public void AddInputLabel(bool place_holder = false)
        {
            /*
             <Grid MaxHeight="48" Margin="0,0,0,0" >
                <Grid.RowDefinitions>
                    <RowDefinition Height="12"/>
                    <RowDefinition Height="24"/>
                </Grid.RowDefinitions>
              <TextBlock FontSize="12" HorizontalAlignment="Left" VerticalAlignment="Center" Height="24" Grid.Row="1" FontWeight="Bold" Margin="13,0,0,0" ScrollViewer.HorizontalScrollBarVisibility="Visible" UseLayoutRounding="False" FocusVisualPrimaryBrush="#FFE60D0D" Foreground="Red">▶</TextBlock>

            </Grid>
             */

            Grid LabelGrid = new Grid();
            LabelGrid.MaxHeight = 48;
            LabelGrid.MinHeight = 48;
            if (place_holder)
            {
                C_INPUT_LABEL_STACK.Children.Add(LabelGrid);
                return;
            }
            LabelGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            LabelGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(2, GridUnitType.Star) });
            TextBlock SlotLabel = new TextBlock();
            Grid.SetRow(SlotLabel, 1);
            SlotLabel.FontSize = 24;
            SlotLabel.HorizontalAlignment = HorizontalAlignment.Left;
            SlotLabel.VerticalAlignment = VerticalAlignment.Top;
            SlotLabel.FontWeight = new Windows.UI.Text.FontWeight() { Weight = 700 };
            SlotLabel.Text = "▶";
            SlotLabel.Margin = new Thickness(4, 0, 0, 0);
            SlotLabel.Foreground = new SolidColorBrush( Windows.UI.Color.FromArgb(255,0,255,0));
            LabelGrid.Children.Add(SlotLabel);
            C_INPUT_LABEL_STACK.Children.Add(LabelGrid);
        }
        public void AddOutputLabel(bool place_holder = false)
        {
            /*
             <Grid MaxHeight="48" Margin="0,0,0,0" >
                <Grid.RowDefinitions>
                    <RowDefinition Height="12"/>
                    <RowDefinition Height="24"/>
                </Grid.RowDefinitions>
              <TextBlock FontSize="12" HorizontalAlignment="Left" VerticalAlignment="Center" Height="24" Grid.Row="1" FontWeight="Bold" Margin="13,0,0,0" ScrollViewer.HorizontalScrollBarVisibility="Visible" UseLayoutRounding="False" FocusVisualPrimaryBrush="#FFE60D0D" Foreground="Red">▶</TextBlock>

            </Grid>
             */

            Grid LabelGrid = new Grid();
            LabelGrid.MaxHeight = 48;
            LabelGrid.MinHeight = 48;
            if (place_holder)
            {
                C_OUTPUT_LABEL_STACK.Children.Add(LabelGrid);
                return;
            }
            LabelGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            LabelGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(2, GridUnitType.Star) });
            TextBlock SlotLabel = new TextBlock();
            Grid.SetRow(SlotLabel, 1);
            SlotLabel.FontSize = 24;
            SlotLabel.HorizontalAlignment = HorizontalAlignment.Right;
            SlotLabel.VerticalAlignment = VerticalAlignment.Top;
            SlotLabel.FontWeight = new Windows.UI.Text.FontWeight() { Weight = 700 };
            SlotLabel.Text = "▶";
            SlotLabel.Margin = new Thickness(0, 0, 4, 0);
            SlotLabel.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0));
            LabelGrid.Children.Add(SlotLabel);
            C_OUTPUT_LABEL_STACK.Children.Add(LabelGrid);
        }

        Windows.UI.Color GetTypeColor(String type_name)
        {
            switch (type_name)
            {
                case "string":
                    return Windows.UI.Color.FromArgb(255, 0, 255, 255);
                    break;
                case "int":
                    return Windows.UI.Color.FromArgb(255, 255, 128, 128);
                    break;
                case "double":
                    return Windows.UI.Color.FromArgb(255, 128, 128, 128);
                    break;
                case "EXECUTE":
                    return Windows.UI.Color.FromArgb(255, 240, 64, 220);
                    break;
            }

            return Windows.UI.Color.FromArgb(255, 128, 255, 128);

        }
        void RefreshNodeHeight()
        {

        }
        public  void OnSlotPressed(object sender, PointerRoutedEventArgs e)
        {
            Grid sender_grid = sender  as Grid;
            Model.TrickerStarNodeSolt slot = sender_grid.DataContext as Model.TrickerStarNodeSolt;
            Model.TrickerStarNodeSoltDetail detail = new Model.TrickerStarNodeSoltDetail()
            {
                SlotType = slot.SlotType,
                NodeName = m_NodeName,
                SlotName = slot.SlotName,
                SlotIndex = slot.SlotIndex,
                SlotSide = slot.SlotSide,

            };
            if(OnSlotClicked!=null)
            {
                OnSlotClicked.Invoke(detail);
            }
        }
        public void clear()
        {
            C_INPUT_LABEL_STACK.Children.Clear();
            C_OUTPUT_STACK.Children.Clear();
            C_INPUT_LABEL_STACK.Children.Clear();
            C_OUTPUT_LABEL_STACK.Children.Clear();
            m_InputSlot.Clear();
            m_OutputSlot.Clear();


        }
        public void SetNodeName(String name)
        {
            m_NodeName = name;
            C_NODE_NAME.Text = m_NodeName;
        }
        public void Select(bool selected)
        {
            if (selected)
            {
            C_CONTAINER_GRID.BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 0));
                C_SLOT_GRID.BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 0));
            }
            else
            {

            C_CONTAINER_GRID.BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 128, 128, 128));
                C_SLOT_GRID.BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 128, 128, 128));
            }
        }

        private void C_CLOSE_BUTTON_Click(object sender, RoutedEventArgs e)
        {
            if(OnNodeClose!=null)
            {

            OnNodeClose.Invoke(m_NodeName);
            }
        }
    }
}
