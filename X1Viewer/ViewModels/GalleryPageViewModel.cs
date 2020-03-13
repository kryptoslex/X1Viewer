using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using X1Viewer.Models;
using Xamarin.Forms;

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
                if (!p.Contains(".jpg"))
                {
                    //video
                    if(Device.RuntimePlatform == Device.iOS)
                    {
                       // VideoRenderClass.GenerateThumbImage(p, 1);
                    }
                    Debug.WriteLine("Video : " + p);
                    galleryImages.Add(new GalleryPageItem() { Path = p,
                                                              Type = GalleryType.Video });
                }
                else
                {
                    Debug.WriteLine("Image : " +p);
                    galleryImages.Add(new GalleryPageItem() { Path = p,
                                                              Type = GalleryType.Image});
                }
                
            }
        }

    }
}
