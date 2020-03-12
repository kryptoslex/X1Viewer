using X1Viewer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using X1Viewer.ViewModels;
using System.Threading.Tasks;
using X1Viewer.Services;
using System.Diagnostics;

namespace X1Viewer.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        public const string CameraHttpServiceType = @"_leicahardwarehub._tcp";
        public const string X1MessagingServiceType = @"_lasx1messaging._tcp";


        bool isContinous = true;

        MainPage RootPage { get => Application.Current.MainPage as MainPage; }

        List<DeviceItem> deviceList = new List<DeviceItem>();
        DeviceItem test1 = new DeviceItem { Id = "1", Name = "Bunny", Description = "mp4", Url = "http://commondatastorage.googleapis.com/gtv-videos-bucket/CastVideos/dash/BigBuckBunnyVideo.mp4" };
        DeviceItem test2 = new DeviceItem { Id = "2", Name = "Steel", Description = "mp4", Url = "http://commondatastorage.googleapis.com/gtv-videos-bucket/CastVideos/dash/TearsOfSteelVideo.mp4" };
        DeviceItem test3 = new DeviceItem { Id = "3", Name = "Chrome", Description = "mp4", Url = "http://commondatastorage.googleapis.com/gtv-videos-bucket/CastVideos/dash/ForBiggerEscapesVideo.mp4" };
        
        public async Task RefreshDataAsync()
        {

            await DiscoveryHelper.SearchService(CameraHttpServiceType).ContinueWith(o =>
            {
                if (o.Result.Count > 0)
                {
                    deviceList.Clear();
                    foreach (var deviceItem in o.Result)
                    {
                        deviceList.Add(deviceItem);
                    }
                    deviceList.Add(test1);
                    deviceList.Add(test2);
                    deviceList.Add(test3);

                }
                Console.WriteLine("Search results: " + o.Result);

            });

            DeviceListView.ItemsSource = null;
            DeviceListView.ItemsSource = deviceList;
            DeviceListView.IsRefreshing = false;
        }

        void OnResetClicked(object sender, EventArgs args)
        {
            Device.BeginInvokeOnMainThread(async () => {
                bool reset = await DisplayAlert("Clear Devices", "Are you sure you want to clear the device list?", "Yes", "No");
                if (reset)
                {
                    Debug.WriteLine("Reset: " + reset);
                    deviceList.Clear();
                    deviceList.Add(test1);
                    deviceList.Add(test2);
                    deviceList.Add(test3);
                }
            });
        }

        public MenuPage()
        {
            InitializeComponent();
            DeviceListView.SeparatorVisibility = SeparatorVisibility.None;


            deviceList.Add(test1);
            deviceList.Add(test2);
            deviceList.Add(test3);


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
                _ = RefreshDataAsync();
                
            });
        }
    }
}