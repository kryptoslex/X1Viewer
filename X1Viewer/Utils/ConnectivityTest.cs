using System;
using Xamarin.Essentials;

namespace X1Viewer.Utils
{
    public class ConnectivityTest
    {
        public ConnectivityTest()
        {
            // Register for connectivity changes, be sure to unsubscribe when finished
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var access = e.NetworkAccess;
            var profiles = e.ConnectionProfiles;
        }
    }
}
