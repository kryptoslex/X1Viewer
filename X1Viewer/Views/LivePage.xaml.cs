using System;
using System.Collections.Generic;
using X1Viewer.ViewModels;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace X1Viewer.Views
{
    public partial class LivePage : ContentPage
    {
        private VideoPlayerViewModel ViewModel { get; set; }

        public LivePage()
        {
            InitializeComponent();
            Title = "X1Viewer";

            ViewModel = VideoPlayerViewModel.Instance;
            BindingContext = ViewModel;
        }

        async void GalleryIconTapCommand(object sender, EventArgs e)
        {
            NavigationPage gallery = new NavigationPage(new GalleryPage())
            {
                BarBackgroundColor = Color.Black,
                BarTextColor = Color.White
            };
            await Navigation.PushModalAsync(gallery);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.PlayMedia();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }


        void PanUpdated(object sender, PanUpdatedEventArgs e) => ViewModel.OnGesture(e);
    }
}
