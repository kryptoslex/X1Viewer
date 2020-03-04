using System;
using System.Collections.Generic;
using X1Viewer.ViewModels;
using Xamarin.Forms;

namespace X1Viewer.Views
{
    public partial class LivePage : ContentPage
    {
        VideoPlayerViewModel _vm;
        public LivePage()
        {
            InitializeComponent();
            Title = "X1Viewer";
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            videoView.MediaPlayerChanged += MediaPlayerChanged;

            _vm = BindingContext as VideoPlayerViewModel;
            _vm.Initialize();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            //videoView.MediaPlayerChanged -= MediaPlayerChanged;

            //_vm = BindingContext as VideoPlayerViewModel;
            //_vm.Stop();
        }

        private void MediaPlayerChanged(object sender, System.EventArgs e)
        {
            _vm.MediaPlayer.Play();
        }

        void PanUpdated(object sender, PanUpdatedEventArgs e) => _vm.OnGesture(e);
    }
}
