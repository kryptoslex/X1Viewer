using System;
namespace X1Viewer.Models
{
    public class DeviceItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }

        public string Address { get; set; }
        public int Port { get; set; }
        public string Service { get; set; }

        public DeviceItem()
        {
        }

        public DeviceItem(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
