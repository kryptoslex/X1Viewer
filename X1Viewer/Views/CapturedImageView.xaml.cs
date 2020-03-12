using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace X1Viewer.Views
{
    public partial class CapturedImageView : ContentPage
    {
        public CapturedImageView(Image image)
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine(image.ToString()) ;
            imageNameLabel.Text = image.Source.ToString();
            CapturedImage.Source = image.Source;
            OnPropertyChanged();
        }

        async void BackButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }
    }
}
