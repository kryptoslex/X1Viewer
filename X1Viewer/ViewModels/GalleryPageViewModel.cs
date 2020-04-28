using System;
using System.Collections.Generic;
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

        public List<GalleryPageItemGroup> galleryPageItemsGroup = new List<GalleryPageItemGroup>();
        public List<GalleryPageItemGroup> GalleryPageItemsGroup
        {
            get => galleryPageItemsGroup;
            private set
            {
                galleryPageItemsGroup = value;
            }
        }

        public GalleryPageViewModel()
        {
            BuildImageGalleryModel();
        }

        public void BuildImageGalleryModel()
        {
            galleryPageItemsGroup.Clear();

            string[] imagePath = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Personal));

            foreach(var p in imagePath)
            {
                if (!p.Contains(".jpg"))
                {
                    //video
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        // VideoRenderClass.GenerateThumbImage(p, 1);
                    }

                    GalleryPageItem item = new GalleryPageItem()
                    {
                        Path = p,
                        Type = GalleryType.Video
                    };

                    AddGalleryPageItemToGroup(item);
                }
                else
                {
                    GalleryPageItem item = new GalleryPageItem()
                    {
                        Path = p,
                        Type = GalleryType.Image
                    };

                    AddGalleryPageItemToGroup(item);

                }
            }
        }

        private void AddGalleryPageItemToGroup (GalleryPageItem item)
        {
            string dateCreationString = Directory.GetCreationTime(item.Path).ToString("dd MMMM yyyy");
            bool isAdded = false;

            foreach (var i in galleryPageItemsGroup)
            {
                if(i.Date.Equals(dateCreationString))
                {
                    i.Add(item);
                    isAdded = true;
                }
            }

            if(!isAdded)
            {
                List<GalleryPageItem> newList = new List<GalleryPageItem>();
                newList.Add(item);
                galleryPageItemsGroup.Add(new GalleryPageItemGroup(dateCreationString, newList));
            }
        }

    }
}
