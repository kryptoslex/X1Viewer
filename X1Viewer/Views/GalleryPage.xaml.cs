using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using com.leicams.X1.popup;
using X1Viewer.Models;
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

        private void GalleryTapGestureRecognizer_OnDoubleTapped(object sender, EventArgs e)
        {
            if (e is TappedEventArgs tappedEventArgs)
            {
                var tapArgs = e as TappedEventArgs;
                var img = tapArgs.Parameter as GalleryPageItem;
                var selectedImage = new Image { Source = img.Path };

                System.Diagnostics.Debug.WriteLine(selectedImage);


                if (selectedImage != null)
                {
                    NavigationHelper.PushModalSingletonAsync(new CapturedImageView(selectedImage));
                }

            }


        }
    }
}
