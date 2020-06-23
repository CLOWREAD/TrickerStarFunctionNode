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

        private async void C_GEN_BUTTON_Click(object sender, RoutedEventArgs e)
        {
            String generated_code = "";
            var nodes = C_MAIN_NODE_VIEW.TS_GetNodes();
            foreach (String nodename in nodes)
            {
                var node = C_MAIN_NODE_VIEW.TS_GetNode(nodename);
                String code = m_NodeCode[nodename] as String;
                code = code.Replace("@TRICKER_STAR_FUNCTIONNAME", node.NodeName);
                foreach (var slot_i in node.InputSlot)
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
                    if (slot_i.SlotType == Model.TrickerStarSlotType.UNDEFINED || 
                        slot_i.SlotType == Model.TrickerStarSlotType.STRING ||
                        slot_i.SlotType == Model.TrickerStarSlotType.INT || 
                        slot_i.SlotType == Model.TrickerStarSlotType.DOUBLE||
                        slot_i.SlotType == Model.TrickerStarSlotType.STRUCTURE
                        )
                    {

                        String var_to_be_replaced = String.Format("TRICKER_STAR_INPUT({0})", slot_i.SlotName);
                        if (line != null)
                        {
                            String var_to_replace = String.Format("object {0} = null; if (GLOBAL_OUTPUT.ContainsKey(\"{1}\")){{ try{{ {0}=((dynamic)GLOBAL_OUTPUT[\"{1}\"]).{2}; }}catch(Exception e){{}}   }}",
                            slot_i.SlotName, line.From.NodeName, line.From.SlotName);
                            code = code.Replace(var_to_be_replaced, var_to_replace);
                        }
                        else
                        {
                            String var_to_replace = String.Format("object {0} = null;", slot_i.SlotName);
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
                    if (slot_o.SlotType == Model.TrickerStarSlotType.EXECUTE)
                    {
                        String var_to_be_replaced = String.Format("TRICKER_STAR_INVOKE({0})", slot_o.SlotName);
                        if (line != null)
                        {
                            String var_to_replace = String.Format("{0}", line.To.NodeName);
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


    }
}
