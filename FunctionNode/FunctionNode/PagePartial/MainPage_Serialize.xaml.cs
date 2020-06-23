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

        private async void C_SAVE_BUTTON_Click(object sender, RoutedEventArgs e)
        {
            Model.TrickerStarNodeViewSerialize s = new Model.TrickerStarNodeViewSerialize();
            var nodes = C_MAIN_NODE_VIEW.TS_GetNodes();
            foreach (String nodename in nodes)
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
            foreach (var group in m_GroupList)
            {
                s.Groups.Add(group);
            }
            foreach (String node_name in m_NodeGroup.Keys)
            {
                Model.TrickerStarNodeGroupModel g = (Model.TrickerStarNodeGroupModel)m_NodeGroup[node_name];
                s.NodeGroups.Add(new TrickerStarNodeGroupModel() { NodeName = node_name, GroupName = g.GroupName, GroupTitle = g.GroupTitle });
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

            foreach (var node in serialize.Nodes)
            {
                C_MAIN_NODE_VIEW.TS_AddNode(node.NodeName);
                C_MAIN_NODE_VIEW.TS_SetNodePosition(node.NodeName, node.Pos);
                C_MAIN_NODE_VIEW.TS_SetNodeTitle(node.NodeName, node.NodeTitle);
                foreach (var inslot in node.InputSlot)
                {
                    if (inslot.SlotType == Model.TrickerStarSlotType.PLACEHOLDER)
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
                m_NodeGroup[group.NodeName] = new Model.TrickerStarGroupModel()
                {
                    GroupName = group.GroupName,
                    GroupTitle = group.GroupTitle,
                };
            }

        }



    }
}
