using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace X1Viewer.Views
{
    public partial class GalleryPage : ContentPage
    {
        public GalleryPage()
        {
            InitializeComponent();
        }

        async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
