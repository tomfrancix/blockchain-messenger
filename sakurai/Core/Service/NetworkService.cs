using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using sakurai.Interface.IService;

namespace sakurai.Core.Service
{
    public class NetworkService : INetworkService
    {
        private readonly ILogger<NetworkService> Logger;

        public NetworkService(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<NetworkService>();
        }

        public void Connect()
        {
            Logger.LogInformation($"Doing the thing.");
        }
    }
}
