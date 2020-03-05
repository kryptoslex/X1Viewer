using System;
using System.Collections.Generic;
using System.Text;

namespace X1Viewer.Models
{
    public enum MenuItemType
    {
        Devices
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
