using System;
using System.Threading.Tasks;
using Memstate.Host;
using Twitter.Core;

namespace Twitter.Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting memstate server, type exit to terminate");
            var host = new HostBuilder<TwitterModel>().Build();
            await host.Start();
            while(true)
            {
                if (Console.ReadLine().ToLower() == "exit") break;
            }
            await host.Stop();

        }
    }
}
