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

            if (!String.IsNullOrWhiteSpace(videoSource))
            {
                //videoPlayer.Source = new FileVideoSource
                //{
                //    File = videoSource
                //};

                videoPlayer.Source= VideoSource.FromUri("https://archive.org/download/ElephantsDream/ed_hd_512kb.mp4");
            }

            Debug.WriteLine("Playing Video");
        }


        async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

    }
}
