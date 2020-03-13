using X1Viewer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using X1Viewer.ViewModels;
using System.Threading.Tasks;
using X1Viewer.Services;
using System.Diagnostics;
using X1Viewer.Utils;

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
        DeviceItem test1 = new DeviceItem { Id = "1", Name = "Sample1", Description = "mp4", Url = "https://archive.org/download/ElephantsDream/ed_hd_512kb.mp4" };
        DeviceItem test2 = new DeviceItem { Id = "2", Name = "Sample2", Description = "mp4", Url = "https://archive.org/download/BigBuckBunny_328/BigBuckBunny_512kb.mp4" };
        DeviceItem test3 = new DeviceItem { Id = "3", Name = "Sample3", Description = "mp4", Url = "https://archive.org/download/Sintel/sintel-2048-stereo_512kb.mp4" };
        
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

        void OnAddClicked(object sender, EventArgs args)
        {
            Device.BeginInvokeOnMainThread(async () => {
                string[] myinput = await PopUpInput.InputBox(this.Navigation);
                if(string.IsNullOrEmpty(myinput[0]) || string.IsNullOrEmpty(myinput[1]))
                {
                    await DisplayAlert("Error", "Invalid Input.", "OK");
                }
                else
                {
                    DeviceItem newDevice = new DeviceItem { Id = deviceList.Count.ToString(), Name = myinput[1], Description = "Goldfinch", Url = myinput[0] };
                    deviceList.Add(newDevice);
                    _ = RefreshDataAsync();
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