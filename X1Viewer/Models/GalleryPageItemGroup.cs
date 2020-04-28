using System;
using System.Collections.Generic;

namespace X1Viewer.Models
{
    public class GalleryPageItemGroup : List<GalleryPageItem>
    {
        public string Date { get; private set; }
        public GalleryPageItemGroup(string date, List<GalleryPageItem> galleryPageItems) : base(galleryPageItems)
        {
            Date = date;
        }
    }
}
