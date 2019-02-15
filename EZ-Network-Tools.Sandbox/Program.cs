using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace EZ_Network_Tools.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            // Process p = new Process();
            // ProcessStartInfo psi = new ProcessStartInfo("netsh", "interface ip set address \"Local Area Connection\" static 192.168.0.10 255.255.255.0 192.168.0.1 1");
            // p.StartInfo = psi;
            // p.Start();

            // get ALL network adapters and print
            Console.WriteLine("<----All Adapters---->");
            var allAdapters = NetworkInterface.GetAllNetworkInterfaces();
            PrintDNSAddresses(allAdapters);

            // get ONLY active network interfaces and print
            Console.WriteLine("<----Active Adapters---->");
            List<NetworkInterface> activeAdapters = (List<NetworkInterface>)GetActiveNetworkInterfaces();
            PrintDNSAddresses(activeAdapters);
        }

        /// <summary>
        /// Gets network adapters that are up and aren't loopback
        /// </summary>
        /// <returns>IEnumerable of NetworkInterfaces</returns>
        public static IEnumerable<NetworkInterface> GetActiveNetworkInterfaces()
        {
            NetworkInterface[] allAdapters = NetworkInterface.GetAllNetworkInterfaces();

            List<NetworkInterface> activeAdapters = new List<NetworkInterface>();

            foreach (NetworkInterface adapter in allAdapters)
            {
                if(adapter.OperationalStatus.HasFlag(OperationalStatus.Up) &&
                    !adapter.Name.ToLower().Contains("loopback") &&
                    !adapter.Name.ToLower().Contains("pseudo"))
                {
                    activeAdapters.Add(adapter);
                }
            }

            return activeAdapters;
        }

        /// <summary>
        /// Given a list of NetworkInterfaces, then print the DNS addresses to console
        /// </summary>
        /// <param name="networkInterfaces"></param>
        public static void PrintDNSAddresses(IList<NetworkInterface> networkInterfaces)
        {
            for(int i = 0; i < networkInterfaces.Count; i++)
            {
                IPInterfaceProperties properties = networkInterfaces[i].GetIPProperties();
                Console.WriteLine($"#{i+1}: Adapter name: {networkInterfaces[i].Name}");
                Console.WriteLine($"DNS Addresses:");
                foreach(var address in properties.DnsAddresses)
                {
                    Console.WriteLine(address.ToString());
                }
                Console.WriteLine();
            }
        }
    }
}
