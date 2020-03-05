using System;
namespace X1Viewer.Models
{
    public class DeviceItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }

        public DeviceItem()
        {
        }

        public DeviceItem(string name, string url)
        {
            Name = name;
            Url = url;
        }
    }
}
