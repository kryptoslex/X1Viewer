using System;
using System.Collections.ObjectModel;
using System.IO;
using X1Viewer.Models;

namespace X1Viewer.ViewModels
{
    public class GalleryPageViewModel : BaseViewModel
    {
        private static GalleryPageViewModel _instance;
        public static GalleryPageViewModel Instance => _instance ?? (_instance = new GalleryPageViewModel());

        private ObservableCollection<GalleryPageItem> galleryImages = new ObservableCollection<GalleryPageItem>();
        public ObservableCollection<GalleryPageItem> GalleryImages
        {
            get => galleryImages;
            private set
            {
                galleryImages = value;
            }
        }


        public GalleryPageViewModel()
        {
            BuildImageGalleryModel();
        }

        public void BuildImageGalleryModel()
        {
            galleryImages.Clear();

            string[] imagePath = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Personal));

            foreach (var p in imagePath)
            {
                System.Diagnostics.Debug.WriteLine(p);
                galleryImages.Add(new GalleryPageItem() { Path = p + ".jpg" });
            }
        }
    }
}
