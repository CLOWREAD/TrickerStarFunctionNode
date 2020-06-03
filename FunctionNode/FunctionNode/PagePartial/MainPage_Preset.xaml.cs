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
        public bool m_PresetDlgFlag = false;
        private void M_PresetDlg_OnPresetSelected(string Code)
        {
            C_MAIN_CODE_TEXT.Text = Code;
        }
        private void M_PresetDlg_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            m_PresetDlgFlag = false;
        }
        private void C_ADD_PRESET_BUTTON_Click(object sender, RoutedEventArgs e)
        {
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
                if (param.StartsWith("@"))
                {
                    m_PresetList.Add(new TrickerStarPresetCodeModel()
                    {
                        NodeName = param.Substring(1),
                        OriginalCode = new String(C_MAIN_CODE_TEXT.Text.ToCharArray()),
                    });
                }
            }
        }

        private async void C_SAVE_PRESET_BUTTON_Click(object sender, RoutedEventArgs e)
        {

            Model.TrickerStarPresetSerialize s = new Model.TrickerStarPresetSerialize();

            foreach(var item in m_PresetList)
            {
                s.PresetCodes.Add(item);
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

        private async void C_LOAD_PRESET_BUTTON_Click(object sender, RoutedEventArgs e)
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

            var serialize = (Model.TrickerStarPresetSerialize)Model.JsonHelper.FromJson(json_str, typeof(Model.TrickerStarPresetSerialize));

            foreach (var item in serialize.PresetCodes)
            {
                m_PresetList.Add(item);
            }



        }
    }
}
