using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using sakurai.Core.Processor;
using sakurai.Interface.IFactory;
using sakurai.Interface.IHelper;
using sakurai.Interface.IProcessor;
using sakurai.Interface.IService;
using sakurai.Objects;

namespace sakurai
{
    public class Sakurai
    {
        private readonly INetworkService NetworkService;
        private readonly IBlockFactory BlockFactory;
        private readonly IBlockchainProcessor BlockchainProcessor;
        private readonly ITimestampHelper TimestampHelper;
        private readonly ILogger<Sakurai> Logger;

        public Sakurai(INetworkService networkService, ILoggerFactory loggerFactory, IBlockFactory blockFactory, ITimestampHelper timestampHelper, IBlockchainProcessor blockchainProcessor)
        {
            NetworkService = networkService;
            BlockFactory = blockFactory;
            TimestampHelper = timestampHelper;
            BlockchainProcessor = blockchainProcessor;
            Logger = loggerFactory.CreateLogger<Sakurai>();
        }

        // Application starting point
        public void Run()
        {
            Logger.LogInformation("Application running...");

            while (true)
            {
                var commands = new List<string>
                {
                    "-join network",
                    "-broadcast"
                };

                Console.WriteLine("Please choose one of the following commands:");

                foreach (var command in commands)
                {
                    Console.WriteLine(command);
                }

                var input = Console.ReadLine()?.Trim();

                var blo = new Blockchain
                {
                    Blocks = new List<Block>()
                };

                blo.Blocks.Add(BlockFactory.Genesis());
                BlockchainProcessor.AddBlock("new block");
                switch (input)
                {
                    case "-join network":
                        NetworkService.ListenToPeers();
                        break;
                    case "-broadcast":
                        NetworkService.BroadcastToPeers(blo);
                        break;
                }

                break;
            }
        }
    }
}
