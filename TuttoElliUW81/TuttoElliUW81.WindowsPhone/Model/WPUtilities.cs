using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TuttoElliUW81.SharedModel
{
    public static class WPUtilities
    {
        //visualizza una stringa in un ContentDialog
        public async static void visualizza(string parole, string title)
        {
            ContentDialog contentDialog;
            contentDialog = new ContentDialog();
            StackPanel panel = new StackPanel();
            panel.Children.Add(new TextBlock
            {
                Text = parole,
                TextWrapping = TextWrapping.Wrap,
            });
            contentDialog.Content = panel;
            contentDialog.IsPrimaryButtonEnabled = true;
            contentDialog.PrimaryButtonText = "OK";
            contentDialog.Title = title;
            await contentDialog.ShowAsync();
            await HideContentDialog(contentDialog);
        }

        public async static Task HideContentDialog(ContentDialog contentDialog)
        {
            await Task.Delay(10000);
            contentDialog.Hide();
        }

        public static string serializeListToJson(List<String> list)
        {
            
            return (list != null || list.Count > 0) ? JsonConvert.SerializeObject(list) : null;
        }

        public static List<String> deserializeJsonToList(String list)
        {
            return (list != null && list.Length > 0) ? JsonConvert.DeserializeObject<List<String>>(list) : null;
        }

        public static void updateLastViewedTracks(String title)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            List<String> savedTracks = WPUtilities.deserializeJsonToList((String)localSettings.Values["viewedLyrics"]);
            if (savedTracks == null) savedTracks = new List<String>();
            savedTracks.Add(title);
            localSettings.Values["viewedLyrics"] = WPUtilities.serializeListToJson(savedTracks);
        }


    }
}
