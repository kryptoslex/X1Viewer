using X1Viewer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace X1Viewer.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        List<DeviceItem> deviceList;
        public MenuPage()
        {
            InitializeComponent();



            List<DeviceItem> deviceList = new List<DeviceItem> {
                
                    new DeviceItem {Id = 1, Name = "Bunny" , Description = "mp4" , Url =  "http://commondatastorage.googleapis.com/gtv-videos-bucket/CastVideos/dash/BigBuckBunnyVideo.mp4"},
                    new DeviceItem {Id = 2, Name = "Steel" , Description = "mp4" , Url =  "http://commondatastorage.googleapis.com/gtv-videos-bucket/CastVideos/dash/TearsOfSteelVideo.mp4"}
                
            };

            DeviceList.ItemsSource = deviceList;
            //ListViewMenu.SelectedItem = deviceList[0];
            //ListViewMenu.ItemSelected += async (sender, e) =>
            //{
            //    if (e.SelectedItem == null)
            //        return;

            //    var id = (int)((DeviceItem)e.SelectedItem).Id;
            //    await RootPage.NavigateFromMenu(id);
            //};
        }
    }
}