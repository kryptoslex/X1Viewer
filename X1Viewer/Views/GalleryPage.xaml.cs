using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace X1Viewer.Views
{
    public partial class GalleryPage : ContentPage
    {
        string[] imagePath = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Personal));

        public GalleryPage()
        {
            InitializeComponent();
            CreatePhotoCollection();
        }

        async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var current = e.CurrentSelection;
        }

        void CreatePhotoCollection()
        {
            foreach(var p in imagePath)
            {
                Grid grid = new Grid() { Padding = 10, RowSpacing = 2 };
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(140) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(140) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2) });

                Grid topBorder = new Grid { BackgroundColor = Color.Red };
                Grid bottomBorder = new Grid { BackgroundColor = Color.Red };
                Grid leftBorder = new Grid { BackgroundColor = Color.Red };
                Grid rightBorder = new Grid { BackgroundColor = Color.Red };

                grid.Children.Add(topBorder, 0, 0);
                Grid.SetColumnSpan(topBorder, 3);

                grid.Children.Add(bottomBorder, 0, 2);
                Grid.SetColumnSpan(bottomBorder, 3);

                grid.Children.Add(leftBorder, 0, 0);
                Grid.SetRowSpan(leftBorder, 3);

                grid.Children.Add(rightBorder, 2, 0);
                Grid.SetRowSpan(rightBorder, 3);

                var image = new Image { Source = p + ".jpg", WidthRequest = 60, HeightRequest = 60};
                grid.Children.Add(image, 1, 1);

                flexLayout.Children.Add(grid);
               
            }
            


        }

        //Image _imageCaptured;
        //public Image CapturedImage
        //{
        //    get
        //    {
        //        return getImagePath[0] + ".jpg";
        //    }
        //    set
        //    {
        //        if (_imageCaptured != value)
        //        {
        //            _imageCaptured = value;
        //        }
        //    }
        //}
    }
}
