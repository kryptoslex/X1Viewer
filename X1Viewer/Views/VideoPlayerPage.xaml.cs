using System;
using System.Diagnostics;
using FormsVideoLibrary;
using Xamarin.Forms;

namespace X1Viewer.Views
{
    public partial class VideoPlayerPage : ContentPage
    {
        public VideoPlayerPage(string videoSource)
        {
            InitializeComponent();

            if (!String.IsNullOrWhiteSpace(videoSource))
            {
                videoPlayer.Source = new FileVideoSource
                {
                    File = videoSource
                };
            }

            Debug.WriteLine("Playing Video");
        }


        async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

    }
}
