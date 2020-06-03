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
        public bool m_ControlPressed = false;
        private async void Dispatcher_AcceleratorKeyActivated(Windows.UI.Core.CoreDispatcher sender, Windows.UI.Core.AcceleratorKeyEventArgs args)
        {
            if (args.EventType.ToString().Equals("KeyDown"))
            {
                if (args.VirtualKey == Windows.System.VirtualKey.Shift)
                {
                    C_MAIN_NODE_VIEW.m_ShiftPressed = true;
                }
                if (args.VirtualKey == Windows.System.VirtualKey.Control)
                {
                    m_ControlPressed = true;
                }
                if (args.VirtualKey == Windows.System.VirtualKey.Tab)
                {
                    if(!m_PresetDlgFlag && m_ControlPressed==true)
                    {
                        m_PresetDlgFlag = true;
                        m_PresetDlg.SetList(m_PresetList);
                        try
                        {
                        await m_PresetDlg.ShowAsync();
                        }catch (Exception e)
                        {
                           // m_PresetDlg.Hide();
                        }
                        
                    }
                    else
                    {
                        
                    }
                }
            }

            if (args.EventType.ToString().Equals("KeyUp"))
            {
                
                if (args.VirtualKey == Windows.System.VirtualKey.Shift)
                {
                    C_MAIN_NODE_VIEW.m_ShiftPressed = false;
                }
                if (args.VirtualKey == Windows.System.VirtualKey.Control)
                {
                    m_ControlPressed = false;
                }
                if (args.VirtualKey == Windows.System.VirtualKey.Escape)
                {
                   
                }

            }

        }

    }
}
