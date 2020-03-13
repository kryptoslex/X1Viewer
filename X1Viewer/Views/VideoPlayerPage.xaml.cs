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
            imageNameLabel.Text = videoSource;
            //if (!String.IsNullOrWhiteSpace(videoSource))
            //{
            //    videoPlayer.Source = new FileVideoSource
            //    {
            //        File = videoSource
            //    };
            //    videoPlayer.Play();
            //}

            Debug.WriteLine("Playing Video");
        }


        async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

    }
}
