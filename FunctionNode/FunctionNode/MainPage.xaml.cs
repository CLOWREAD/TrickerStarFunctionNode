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

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace FunctionNode
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        List<Model.TrickerStarGroupModel> m_GroupList = new List<Model.TrickerStarGroupModel>();
        //List<Model.TrickerStarNodeGroupModel> m_NodeGroupList = new List<Model.TrickerStarNodeGroupModel>();
        public System.Collections.Hashtable m_NodeCode = new System.Collections.Hashtable();
        public System.Collections.Hashtable m_NodeGroupList = new System.Collections.Hashtable();
        public MainPage()
        {
            this.InitializeComponent();
            this.Dispatcher.AcceleratorKeyActivated += Dispatcher_AcceleratorKeyActivated;

            C_NODE_GROUP_LIST.ItemsSource = m_GroupList;

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

        /*
         
        */
        private void C_ADD_NODE_BUTTON_Click(object sender, RoutedEventArgs e)
        {


            var c_pos = C_MAIN_NODE_VIEW.TS_GetViewCenterPoint();



            String node_name ="NODE"+ Model.TrickerStarDataModel.RandToken();
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
                if (param.Contains("@"))
                {
                    C_MAIN_NODE_VIEW.TS_SetNodeTitle(node_name, param.Substring(1));
                }
                if (param.Contains("INPUT ") && (!param.Contains(">>")))

                {

                    String slot_name= param.Split(" ",StringSplitOptions.RemoveEmptyEntries)[1];
                    String slot_type_str = param.Split(" ", StringSplitOptions.RemoveEmptyEntries)[3];
                    var slot_type = Model.TrickerStarDataModel.GetSlotTypeFromName(slot_type_str);

                    if (slot_type== Model.TrickerStarSlotType.PLACEHOLDER)
                    {
                        C_MAIN_NODE_VIEW.TS_AddSlot(node_name, Model.TrickerStarSlotType.PLACEHOLDER, "PLACE_HOLDER", Model.TrickerStarSlotSide.INPUT );
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
            m_NodeCode[node_name] =  new String(code.ToCharArray());

        }

        private async void C_SAVE_BUTTON_Click(object sender, RoutedEventArgs e)
        {
            Model.TrickerStarNodeViewSerialize s = new Model.TrickerStarNodeViewSerialize();
            var nodes = C_MAIN_NODE_VIEW.TS_GetNodes();
            foreach(String nodename in nodes)
            {
                s.Nodes.Add(C_MAIN_NODE_VIEW.TS_GetNode(nodename));
                s.Codes.Add(
                    new Model.TrickerStarFunctionNodeCodeModel() 
                    { Code = m_NodeCode[nodename] as String, NodeName = nodename }
                );

            }
            var lines = C_MAIN_NODE_VIEW.TS_GetLines();
            foreach (String linename in lines)
            {
                s.Lines.Add(C_MAIN_NODE_VIEW.TS_GetLine(linename));
            }
            foreach(var group in m_GroupList)
            {
                s.Groups.Add(group);
            }
            foreach (String node_name in m_NodeGroupList.Keys)
            {
                Model.TrickerStarNodeGroupModel g = (Model.TrickerStarNodeGroupModel)m_NodeGroupList[node_name];
                s.NodeGroups.Add(new TrickerStarNodeGroupModel() { NodeName= node_name ,GroupName=g.GroupName,GroupTitle=g.GroupTitle});
            }



            String json_str = Model.JsonHelper.ToJson(s, s.GetType());
            var picker = new Windows.Storage.Pickers.FileSavePicker();

            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;

            // Dropdown of file types the user can save the file as

            picker.FileTypeChoices.Add("JSON", new List<string>() { ".json" });

            // Default file name if the user does not type one in or select a file to replace

            picker.SuggestedFileName = "NewDocument";

            var t_storagefile = await picker.PickSaveFileAsync();

            if (t_storagefile == null)

            {

                return;

            }

            using (StorageStreamTransaction transaction = await t_storagefile.OpenTransactedWriteAsync())

            {

                using (Windows.Storage.Streams.DataWriter dataWriter = new Windows.Storage.Streams.DataWriter(transaction.Stream))

                {



                    dataWriter.WriteString(json_str);

                    transaction.Stream.Size = await dataWriter.StoreAsync();

                    await transaction.Stream.FlushAsync();

                    await transaction.CommitAsync();

                }

            }
        }

        private async void C_LOAD_BUTTON_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();

            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;

            // Dropdown of file types the user can save the file as

            picker.FileTypeFilter.Add(".json");

            // Default file name if the user does not type one in or select a file to replace

            var t_storagefile = await picker.PickSingleFileAsync();

            if (t_storagefile == null) return;

            String json_str;

            using (StorageStreamTransaction transaction = await t_storagefile.OpenTransactedWriteAsync())

            {

                using (Windows.Storage.Streams.DataReader dataReader = new Windows.Storage.Streams.DataReader(transaction.Stream))

                {

                    uint numBytesLoaded = await dataReader.LoadAsync((uint)transaction.Stream.Size);

                    json_str = dataReader.ReadString(numBytesLoaded);



                    await transaction.Stream.FlushAsync();

                    await transaction.CommitAsync();

                }

            }

            var serialize = (Model.TrickerStarNodeViewSerialize)Model.JsonHelper.FromJson(json_str, typeof(Model.TrickerStarNodeViewSerialize));

            foreach( var node in serialize.Nodes)
            {
                C_MAIN_NODE_VIEW.TS_AddNode(node.NodeName);
                C_MAIN_NODE_VIEW.TS_SetNodePosition(node.NodeName, node.Pos);
                C_MAIN_NODE_VIEW.TS_SetNodeTitle(node.NodeName, node.NodeTitle);
                foreach(var inslot in node.InputSlot)
                {
                    if(inslot.SlotType==Model.TrickerStarSlotType.PLACEHOLDER)
                    {
                    C_MAIN_NODE_VIEW.TS_AddSlot(node.NodeName, inslot.SlotType, inslot.SlotName, inslot.SlotSide);

                    }
                    else
                    {
                        C_MAIN_NODE_VIEW.TS_AddSlot(node.NodeName, inslot.SlotType, inslot.SlotName, inslot.SlotSide);

                    }
                    if (inslot.SlotType == Model.TrickerStarSlotType.INSTANCE_VALUE)
                    {
                        C_MAIN_NODE_VIEW.TS_SetSlotValue(node.NodeName, inslot.SlotIndex, inslot.SlotSide, inslot.SlotValue);
                    }
                }
                foreach (var outslot in node.OutputSlot)
                {
                    if (outslot.SlotType == Model.TrickerStarSlotType.PLACEHOLDER)
                    {
                        C_MAIN_NODE_VIEW.TS_AddSlot(node.NodeName, outslot.SlotType, outslot.SlotName, outslot.SlotSide);

                    }
                    else
                    {
                        C_MAIN_NODE_VIEW.TS_AddSlot(node.NodeName, outslot.SlotType, outslot.SlotName, outslot.SlotSide);

                    }
                    if (outslot.SlotType == Model.TrickerStarSlotType.INSTANCE_VALUE)
                    {
                        C_MAIN_NODE_VIEW.TS_SetSlotValue(node.NodeName, outslot.SlotIndex, outslot.SlotSide, outslot.SlotValue);
                    }
                }
            }
            foreach (var line in serialize.Lines)
            {
                C_MAIN_NODE_VIEW.TS_AddLine(line.LineName, line.From.NodeName, line.From.SlotIndex, line.To.NodeName, line.To.SlotIndex);
            }

            foreach (var code in serialize.Codes)
            {
                m_NodeCode[code.NodeName] = code.Code;
            }
            foreach (var group in serialize.Groups)
            {
                m_GroupList.Add(group);
            }
            foreach (var group in serialize.NodeGroups)
            {
                m_NodeGroupList[group.NodeName] = new Model.TrickerStarGroupModel()
                {
                    GroupName = group.GroupName,
                    GroupTitle = group.GroupTitle,
                };
            }

        }

        private async void C_GEN_BUTTON_Click(object sender, RoutedEventArgs e)
        {
             String generated_code = "";
            var nodes = C_MAIN_NODE_VIEW.TS_GetNodes();
            foreach (String nodename in nodes)
            {
                var node= C_MAIN_NODE_VIEW.TS_GetNode(nodename);
                String code = m_NodeCode[nodename] as String;
                code = code.Replace("@TRICKER_STAR_FUNCTIONNAME", node.NodeName);
                foreach ( var slot_i in node.InputSlot )
                {
                    var line = C_MAIN_NODE_VIEW.TS_GetLine(slot_i.LineName);
                    
                   
                    if (slot_i.SlotType == Model.TrickerStarSlotType.INSTANCE_VALUE)
                    {
                        String var_to_be_replaced = String.Format("TRICKER_STAR_INSTANCE_VALUE({0})", slot_i.SlotName);
                        String var_to_replace = String.Format("{0}",
                            slot_i.SlotValue);
                        // object { 0} = null; if (GLOBAL_OUTPUT.ContainsKey(\"{1}\")){{ try{{ {0}=((dynamic)GLOBAL_OUTPUT[\"{1}\"]).{2}; }}catch(Exception e){{}}   }}", i_name, o_label + "_" + o_name, o_slot);
                        code = code.Replace(var_to_be_replaced, var_to_replace);
                    }
                    if (slot_i.SlotType == Model.TrickerStarSlotType.PLACEHOLDER)
                    {

                    }
                    if(slot_i.SlotType == Model.TrickerStarSlotType.STRING || slot_i.SlotType == Model.TrickerStarSlotType .INT|| slot_i.SlotType == Model.TrickerStarSlotType.DOUBLE)
                    {

                        String var_to_be_replaced = String.Format("TRICKER_STAR_INPUT({0})",slot_i.SlotName);
                        if (line != null)
                        {
                            String var_to_replace = String.Format("object {0} = null; if (GLOBAL_OUTPUT.ContainsKey(\"{1}\")){{ try{{ {0}=((dynamic)GLOBAL_OUTPUT[\"{1}\"]).{2}; }}catch(Exception e){{}}   }}",
                            slot_i.SlotName, line.From.NodeName, line.From.SlotName);
                            code = code.Replace(var_to_be_replaced, var_to_replace);
                        }
                        else
                        {
                            String var_to_replace = String.Format("object {0} = null;",
                           slot_i.SlotName);
                            code = code.Replace(var_to_be_replaced, var_to_replace);
                        }
                              
                    }

                    if (slot_i.SlotType == Model.TrickerStarSlotType.EXECUTE)
                    {
                        if (line == null) return;
                     


                    }
                }

                foreach (var slot_o in node.OutputSlot)
                {
                    var line = C_MAIN_NODE_VIEW.TS_GetLine(slot_o.LineName);
                    if (slot_o.SlotType== Model.TrickerStarSlotType.EXECUTE)
                    {
                        String var_to_be_replaced = String.Format("TRICKER_STAR_INVOKE({0})", slot_o.SlotName);
                        if (line != null)
                        {
                        String var_to_replace = String.Format("\"{0}\";", line.To.NodeName);
                        code = code.Replace(var_to_be_replaced, var_to_replace);

                        }
                        else
                        {
                            String var_to_replace = "null";
                            code = code.Replace(var_to_be_replaced, var_to_replace);
                        }
                        // object { 0} = null; if (GLOBAL_OUTPUT.ContainsKey(\"{1}\")){{ try{{ {0}=((dynamic)GLOBAL_OUTPUT[\"{1}\"]).{2}; }}catch(Exception e){{}}   }}", i_name, o_label + "_" + o_name, o_slot);


                    }
                }
                generated_code += code;
             

            }
            var picker = new Windows.Storage.Pickers.FileSavePicker();

            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;

            // Dropdown of file types the user can save the file as

            picker.FileTypeChoices.Add("cs", new List<string>() { ".cs" });

            // Default file name if the user does not type one in or select a file to replace

            picker.SuggestedFileName = "gencode";

            var t_storagefile = await picker.PickSaveFileAsync();

            if (t_storagefile == null)

            {

                return;

            }

            using (StorageStreamTransaction transaction = await t_storagefile.OpenTransactedWriteAsync())

            {

                using (Windows.Storage.Streams.DataWriter dataWriter = new Windows.Storage.Streams.DataWriter(transaction.Stream))

                {



                    dataWriter.WriteString(generated_code);

                    transaction.Stream.Size = await dataWriter.StoreAsync();

                    await transaction.Stream.FlushAsync();

                    await transaction.CommitAsync();

                }

            }

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
            var nodes=C_MAIN_NODE_VIEW.TS_GetSelectedNodes();
            foreach(String node_name in nodes)
            {

                m_NodeGroupList[node_name] = new Model.TrickerStarGroupModel()
                {
                GroupName= item.GroupName,
                GroupTitle=item.GroupTitle,
                };
            }

        }

        private void C_DEL_GROUP_BUTTON_Click(object sender, RoutedEventArgs e)
        {
            var dc=((Button)sender).DataContext;
            m_GroupList.Remove((Model.TrickerStarGroupModel)dc);
            C_NODE_GROUP_LIST.ItemsSource = null;
            C_NODE_GROUP_LIST.ItemsSource = m_GroupList;
        }

        private void C_ADD_GROUP_BUTTON_Click(object sender, RoutedEventArgs e)
        {
            m_GroupList.Add(new Model.TrickerStarGroupModel()
            {
                GroupTitle = C_GROUP_NAME_TEXTBOX.Text,
                GroupName= "GROUP"+Model.TrickerStarDataModel.RandToken()
            });
            C_NODE_GROUP_LIST.ItemsSource = null;
            C_NODE_GROUP_LIST.ItemsSource = m_GroupList;
        }

        private void C_NODE_GROUP_LIST_ItemClick(object sender, ItemClickEventArgs e)
        {
            var dc = ((ListView)sender);

            var item = (Model.TrickerStarGroupModel)e.ClickedItem;
            var nodes = m_NodeGroupList.Keys;
            
            double top= Double.MaxValue ,left = Double.MaxValue, right=Double.MinValue, down=Double.MinValue;
            bool SelectFlag = false;
            foreach (string node_name in nodes)
            {
                Model.TrickerStarGroupModel g = ( Model.TrickerStarGroupModel)m_NodeGroupList[node_name];
                if (g.GroupName.Equals(item.GroupName))
                {
                    C_MAIN_NODE_VIEW.TS_SelectNode(node_name);
                    var pos = C_MAIN_NODE_VIEW.TS_GetNodePosition(node_name);
                    left = Math.Min(left, pos.X);
                    top = Math.Min(top, pos.Y);

                    right = Math.Max(right, pos.X );
                    down = Math.Max(down, pos.Y);
                    SelectFlag = true;

                }
            }
            if(SelectFlag)
            {

            C_MAIN_NODE_VIEW.TS_FocusPos(new Point() { X = (right + left) / 2, Y = (top + down) / 2 });
            } 


        }
    }
    
}
