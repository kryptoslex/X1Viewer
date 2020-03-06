using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using X1Viewer.Models;
using Zeroconf;

namespace X1Viewer.Services
{
    public class DiscoveryHelper
    {
        static CancellationTokenSource source = new CancellationTokenSource();

        public static void StopSearch()
        {
            try
            {
                source?.Cancel();
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //X1LogHelper.Exception(e);
            }
            finally
            {
                source = null;
            }
        }
        public static async Task<IReadOnlyCollection<DeviceItem>> SearchService(string protocol)
        {
            Debug.Assert(protocol != null);
            var localProtocol = protocol;
            if (!localProtocol.EndsWith(".local."))
            {
                localProtocol += ".local.";
            }
            var list = new List<DeviceItem>();
            //var wifiManager = X1CoreFactory.Instance.WiFiManager;
            try
            {
                source.Cancel();
                source = new CancellationTokenSource();
                //wifiManager?.AcquireWiFiService();

                Debug.WriteLine("-------- localProtocol: " + localProtocol);
#if __ANDROID__
                var hosts =
 await ZeroconfResolver.ResolveAsync(localProtocol, default(TimeSpan),20, 2000, null, source.Token);
#else
                var hosts = await ZeroconfResolver.ResolveAsync(localProtocol, default(TimeSpan), 2, 2000, null,
                    source.Token);
#endif
                foreach (var host in hosts)
                {
                    Debug.WriteLine("-------- host.IPAddress: " + host.IPAddress);
                    list.AddRange(host.Services.Select(service => new DeviceItem
                    {
                        Name = host.DisplayName,
                        Address = host.IPAddress,
                        Url = "rtsp://" + host.IPAddress + "/stream1",
                        Id = host.Id,
                        Port = service.Value.Port,
                        Service = protocol,
                        Description = "Device"
                    })); 
                }
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                //wifiManager?.ReleaseWiFiService();
            }
            return list;
        }
    }
}
