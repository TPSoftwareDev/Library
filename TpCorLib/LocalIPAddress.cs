using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Teleperformance
{
    public static class LocalIPAddress
    {
        public static string GetLocalIPAddress()
        {
            try
            {
                NetworkInterface local = NetworkInterface.GetAllNetworkInterfaces().Where(i => !i.Description.ToLower().Contains("virtual") &&
                                                                                                !i.Description.ToLower().Contains("bluetooth") &&
                                                                                                !i.Description.ToLower().Contains("pseudo") &&
                                                                                                i.OperationalStatus == OperationalStatus.Up).FirstOrDefault();
                var addresses = local.GetIPProperties().UnicastAddresses;
                var preferred = (from a in addresses where a.Address.AddressFamily == AddressFamily.InterNetwork select a).FirstOrDefault();
                if (preferred != null)
                    return preferred.Address.ToString();
                return local.GetIPProperties().UnicastAddresses[0].Address.ToString();
            }
            catch (Exception)
            {
                throw new Exception("Local IP Address Not Found!");
            }
        }
    }
}
