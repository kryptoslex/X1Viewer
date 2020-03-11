using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using X1Viewer.ViewModels;
using Xamarin.Forms;

namespace X1Viewer.Views
{
    public partial class GalleryPage : ContentPage
    {
        private GalleryPageViewModel ViewModel { get; set; }

        public GalleryPage()
        {
            InitializeComponent();

            ViewModel = GalleryPageViewModel.Instance;
            BindingContext = ViewModel;
        }

        async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var current = e.CurrentSelection;
        }

       
    }
}
