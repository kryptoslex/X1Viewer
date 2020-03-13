using System;

namespace X1Viewer.Models
{
    public enum GalleryType
    {
        Image,
        Video
    };

    public class GalleryPageItem
    {
        public GalleryType Type { get; set; }
        public string Path { get; set; }
    }
}