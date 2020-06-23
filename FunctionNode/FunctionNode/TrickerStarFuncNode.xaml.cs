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
        public String m_NodeTitle = "MAIN";
        public delegate void Delegate_SlotClicked(Model.TrickerStarNodeSoltDetail slot_detail);
        public delegate void Delegate_SlotValueChanged(Model.TrickerStarNodeSoltDetail slot_detail);
        public event Delegate_SlotClicked OnSlotClicked ;
        public delegate void Delegate_NodeClose(String name);
        public event Delegate_NodeClose OnNodeClose;

        public event Delegate_SlotValueChanged OnSlotValueChanged;

        List<Model.TrickerStarNodeSolt> m_InputSlot = new List<Model.TrickerStarNodeSolt>();
        List<Model.TrickerStarNodeSolt> m_OutputSlot = new List<Model.TrickerStarNodeSolt>();
        public TrickerStarFunctionNode()
        {
            this.InitializeComponent();

            C_NODE_NAME.Text = m_NodeName;
        }
        public void AddInpusStack(Model.TrickerStarSlotType type_str,String name_str)
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
            if (slot.SlotType == Model.TrickerStarSlotType.PLACEHOLDER)
            {
                C_INPUT_STACK.Children.Add(slot_grid);
                AddInputLabel(slot);
                return;
            }

            slot_grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            slot_grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            slot_grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(4, GridUnitType.Star) });

            Border border = new Border();
            border.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0));
            TextBlock var_type = new TextBlock();
            var_type.FontSize = 8;
            var_type.HorizontalAlignment = HorizontalAlignment.Left;
            var_type.VerticalAlignment = VerticalAlignment.Stretch;
            var_type.Text = type_str.ToString();
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

            TextBox var_instance_value = new TextBox();
            var_instance_value.Height = 32;
            var_instance_value.FontSize = 12;
            var_instance_value.AcceptsReturn = true;
            var_instance_value.FontWeight = new Windows.UI.Text.FontWeight() { Weight = 500 };
            var_instance_value.HorizontalAlignment = HorizontalAlignment.Stretch;
            var_instance_value.VerticalAlignment = VerticalAlignment.Stretch;
            var_instance_value.Margin = new Thickness(-1, -1, -1, -1);
            var_instance_value.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255));
            var_instance_value.Name = name_str;
            var_instance_value.TextWrapping = TextWrapping.Wrap;
            var_instance_value.MaxWidth = 400;
            var_instance_value.DataContext = slot;
            var_instance_value.TextChanged += SlotValueTextChanged;
            var_instance_value.PlaceholderText = name_str;

            Border name_border = new Border();
            name_border.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0));
            name_border.BorderBrush = new SolidColorBrush(GetTypeColor(type_str));
            name_border.BorderThickness = new Thickness(2);
            name_border.Margin = new Thickness(4);
            name_border.CornerRadius = new CornerRadius(4);
            name_border.MaxWidth = 200;
            

            if (type_str==Model.TrickerStarSlotType.INSTANCE_VALUE)
            {
                name_border.Child = var_instance_value;
            }
            else
            {
                name_border.Child = var_name;
            }


            Grid.SetRow(name_border, 1);
            Grid.SetRowSpan(name_border, 2);
            slot_grid.Children.Add(name_border);

            Grid.SetRow(border, 0);
            Grid.SetRowSpan(border, 2);
            slot_grid.Children.Add(border);
            C_INPUT_STACK.Children.Add(slot_grid);
            slot_grid.DataContext = slot;
            slot_grid.PointerPressed += OnSlotPressed;


         
                AddInputLabel(slot);
            
        }

        private void SlotValueTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            Model.TrickerStarNodeSolt slot = tb.DataContext as Model.TrickerStarNodeSolt;
            Model.TrickerStarNodeSoltDetail detail = new Model.TrickerStarNodeSoltDetail()
            {
                SlotType = slot.SlotType,
                NodeName = m_NodeName,
                SlotName = slot.SlotName,
                SlotIndex = slot.SlotIndex,
                SlotSide = slot.SlotSide,
                SlotValue = tb.Text,

            };
            if (OnSlotValueChanged != null)
            {
                OnSlotValueChanged.Invoke(detail);
            }
        }

        public String GetSlotValue(Model.TrickerStarSlotSide side,int slot_index)
        {
            String res="";
            try
            {

            if(side==Model.TrickerStarSlotSide.INPUT)
            {
                var grid=C_INPUT_STACK.Children[slot_index] as Grid;
                var border = grid.Children[0] as Border;
                var textbox = border.Child as TextBox;
                res = textbox.Text;
            }
            if (side == Model.TrickerStarSlotSide.OUTPUT)
            {
                var grid = C_OUTPUT_STACK.Children[slot_index] as Grid;
                var border = grid.Children[0] as Border;
                var textbox = border.Child as TextBox;
                res = textbox.Text;
            }
            }catch(Exception e)
            {

            }
            return res;
        }
        public void  SetSlotValue(Model.TrickerStarSlotSide side, int slot_index,String value)
        {
            if (side == Model.TrickerStarSlotSide.INPUT)
            {
                var grid = C_INPUT_STACK.Children[slot_index] as Grid;
                var border = grid.Children[0] as Border;
                var textbox = border.Child as TextBox;
                textbox.Text=value;
            }
            if (side == Model.TrickerStarSlotSide.OUTPUT)
            {
                var grid = C_OUTPUT_STACK.Children[slot_index] as Grid;
                var border = grid.Children[0] as Border;
                var textbox = border.Child as TextBox;
                textbox.Text = value;
            }
        
         }


        public void AddOutpusStack(Model.TrickerStarSlotType type_str, String name_str)
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

            if(slot.SlotType == Model.TrickerStarSlotType.PLACEHOLDER)
            {
                C_OUTPUT_STACK.Children.Add(slot_grid);
                AddOutputLabel(slot);

                return;
            }


            slot_grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            slot_grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            slot_grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(4, GridUnitType.Star) });

            Border border = new Border();
            border.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0));
            TextBlock var_type = new TextBlock();
            var_type.FontSize = 8;
            var_type.HorizontalAlignment = HorizontalAlignment.Right;
            var_type.VerticalAlignment = VerticalAlignment.Stretch;
            var_type.Text = type_str.ToString();
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

            TextBox var_instance_value = new TextBox();
            var_instance_value.AcceptsReturn = true;
            var_instance_value.Height = 32;
            var_instance_value.FontSize = 12;
            var_instance_value.FontWeight = new Windows.UI.Text.FontWeight() { Weight = 500 };
            var_instance_value.HorizontalAlignment = HorizontalAlignment.Stretch;
            var_instance_value.VerticalAlignment = VerticalAlignment.Stretch;
            var_instance_value.Margin = new Thickness(-1, -1, -1, -1);
            var_instance_value.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255));
            var_instance_value.Name = name_str;
            var_instance_value.TextWrapping = TextWrapping.Wrap;
            var_instance_value.MaxWidth = 400;
            var_instance_value.DataContext = slot;
            var_instance_value.TextChanged += SlotValueTextChanged;
            var_instance_value.PlaceholderText = name_str;


            Border name_border = new Border();
            name_border.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0));
            name_border.BorderBrush = new SolidColorBrush(GetTypeColor(type_str));
            name_border.BorderThickness = new Thickness(2);
            name_border.Margin = new Thickness(4);
            name_border.CornerRadius = new CornerRadius(4);
            name_border.MaxWidth = 200;
            if (type_str == Model.TrickerStarSlotType.INSTANCE_VALUE)
            {
                name_border.Child = var_instance_value;
            }
            else
            {
                name_border.Child = var_name;
                //AddOutputLabel(slot, place_holder);
            }

            Grid.SetRow(name_border, 1);
            Grid.SetRowSpan(name_border, 2);
            slot_grid.Children.Add(name_border);

            Grid.SetRow(border, 0);
            Grid.SetRowSpan(border, 2);
            slot_grid.Children.Add(border);

            C_OUTPUT_STACK.Children.Add(slot_grid);
            slot_grid.DataContext = slot;
            slot_grid.PointerPressed += OnSlotPressed;
           
            
            
                AddOutputLabel(slot);
            

        }
        public void AddInputLabel(Model.TrickerStarNodeSolt slot)
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
            if (slot.SlotType == Model.TrickerStarSlotType.PLACEHOLDER)
            {
                C_INPUT_LABEL_STACK.Children.Add(LabelGrid);
                return;
            }
            LabelGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            LabelGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(3, GridUnitType.Star) });
            TextBlock SlotLabel = new TextBlock();
            Grid.SetRow(SlotLabel, 1);
            SlotLabel.FontSize = 24;
            SlotLabel.HorizontalAlignment = HorizontalAlignment.Left;
            SlotLabel.VerticalAlignment = VerticalAlignment.Top;
            SlotLabel.FontWeight = new Windows.UI.Text.FontWeight() { Weight = 700 };
            SlotLabel.Text = GetTypeLabel(slot.SlotType);
            SlotLabel.Margin = new Thickness(8, 0, 0, 0);
            SlotLabel.Foreground = new SolidColorBrush( Windows.UI.Color.FromArgb(255,0,255,0));
            LabelGrid.Children.Add(SlotLabel);
            C_INPUT_LABEL_STACK.Children.Add(LabelGrid);

            LabelGrid.DataContext = slot;
            LabelGrid.PointerPressed += OnSlotPressed;


        }
        public void AddOutputLabel(Model.TrickerStarNodeSolt slot)
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
            if (slot.SlotType==Model.TrickerStarSlotType.PLACEHOLDER)
            {
                C_OUTPUT_LABEL_STACK.Children.Add(LabelGrid);
                return;
            }
            LabelGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            LabelGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(3, GridUnitType.Star) });
            TextBlock SlotLabel = new TextBlock();
            Grid.SetRow(SlotLabel, 1);
            SlotLabel.FontSize = 24;
            SlotLabel.HorizontalAlignment = HorizontalAlignment.Right;
            SlotLabel.VerticalAlignment = VerticalAlignment.Top;
            SlotLabel.FontWeight = new Windows.UI.Text.FontWeight() { Weight = 700 };
            SlotLabel.Text = GetTypeLabel(slot.SlotType);
            SlotLabel.Margin = new Thickness(0, 0, 8, 0);
            SlotLabel.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0));
            LabelGrid.Children.Add(SlotLabel);
            C_OUTPUT_LABEL_STACK.Children.Add(LabelGrid);

            LabelGrid.DataContext = slot;
            LabelGrid.PointerPressed += OnSlotPressed;
        }

        Windows.UI.Color GetTypeColor(Model.TrickerStarSlotType type_name)
        {
            switch (type_name)
            {
                case Model.TrickerStarSlotType.STRING:
                    return Windows.UI.Color.FromArgb(255, 0, 255, 255);
                    break;
                case Model.TrickerStarSlotType.INT:
                    return Windows.UI.Color.FromArgb(255, 255, 128, 128);
                    break;
                case Model.TrickerStarSlotType.STRUCTURE:
                    return Windows.UI.Color.FromArgb(255, 255, 128, 128);
                    break;
                case Model.TrickerStarSlotType.DOUBLE:
                    return Windows.UI.Color.FromArgb(255, 128, 128, 128);
                    break;
                case Model.TrickerStarSlotType.EXECUTE:
                    return Windows.UI.Color.FromArgb(255, 240, 64, 220);
                    break;
            }

            return Windows.UI.Color.FromArgb(255, 128, 255, 128);
            
        }
        String GetTypeLabel(Model.TrickerStarSlotType type_name)
        {
            switch (type_name)
            {
                case Model.TrickerStarSlotType.STRING:
                    return "◉";
                    break;
                case Model.TrickerStarSlotType.INT:
                    return "◉";
                    break;
                case Model.TrickerStarSlotType.DOUBLE:
                    return "◉";
                    break;
                case Model.TrickerStarSlotType.STRUCTURE:
                    return "◉";
                    break;
                case Model.TrickerStarSlotType.EXECUTE:
                    return "◈";
                    break;
                case Model.TrickerStarSlotType.INSTANCE_VALUE:
                    return "◈";
                    break;
            }

            return "◉";


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
        public void ClearSlot()
        {
            C_INPUT_LABEL_STACK.Children.Clear();
            C_OUTPUT_STACK.Children.Clear();
            C_INPUT_LABEL_STACK.Children.Clear();
            C_OUTPUT_LABEL_STACK.Children.Clear();
            m_InputSlot.Clear();
            m_OutputSlot.Clear();


        }
        public void SetNodeTitle(String name)
        {
            m_NodeTitle = name;
            C_NODE_NAME.Text = m_NodeTitle;
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

        private void UserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            Canvas.SetZIndex(this, 5);
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            Canvas.SetZIndex(this, 4);
        }
    }
}
