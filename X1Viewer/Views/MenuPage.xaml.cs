using X1Viewer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using X1Viewer.ViewModels;
using System.Threading.Tasks;
using System.Windows.Input;

namespace X1Viewer.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        List<DeviceItem> deviceList = new List<DeviceItem> {

                    new DeviceItem {Id = 1, Name = "Bunny" , Description = "mp4" , Url =  "http://commondatastorage.googleapis.com/gtv-videos-bucket/CastVideos/dash/BigBuckBunnyVideo.mp4"},
                    new DeviceItem {Id = 2, Name = "Steel" , Description = "mp4" , Url =  "http://commondatastorage.googleapis.com/gtv-videos-bucket/CastVideos/dash/TearsOfSteelVideo.mp4"}

        };
        public void RefreshData()
        {
            var Dev = new DeviceItem { Id = 1, Name = "test", Description = "mp4", Url = "http://commondatastorage.googleapis.com/gtv-videos-bucket/CastVideos/dash/BigBuckBunnyVideo.mp4" };
            deviceList.Add(Dev);
            DeviceListView.ItemsSource = null;
            DeviceListView.ItemsSource = deviceList;
        }

        public MenuPage()
        {
            InitializeComponent();
            DeviceListView.SeparatorVisibility = SeparatorVisibility.None;


            DeviceListView.ItemsSource = deviceList;
            //ListViewMenu.SelectedItem = deviceList[0];
            DeviceListView.ItemSelected += (sender, e) =>
            {
                Console.WriteLine("update: " + ((DeviceItem)e.SelectedItem).Url);
                if (Xamarin.Forms.Application.Current.MainPage is MasterDetailPage masterDetailPage)
                {
                    masterDetailPage.IsPresented = false;
                }
                else if (Xamarin.Forms.Application.Current.MainPage is NavigationPage navigationPage && navigationPage.CurrentPage is MasterDetailPage nestedMasterDetail)
                {
                    nestedMasterDetail.IsPresented = false;
                }

                //update video source
                VideoPlayerViewModel.Instance.PlayMedia(((DeviceItem)e.SelectedItem).Url);
            };

            DeviceListView.RefreshCommand = new Command(() => {
                //Do your stuff.    
                RefreshData();
                DeviceListView.IsRefreshing = false;
            });
        }
    }
}