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
        private readonly IBlockProcessor BlockProcessor;
        private readonly ITimestampHelper TimestampHelper;
        private readonly ILogger<Sakurai> Logger;

        public Sakurai(INetworkService networkService, ILoggerFactory loggerFactory, IBlockFactory blockFactory, ITimestampHelper timestampHelper, IBlockProcessor blockProcessor)
        {
            NetworkService = networkService;
            BlockFactory = blockFactory;
            TimestampHelper = timestampHelper;
            BlockProcessor = blockProcessor;
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
                    "-create block",
                    "-create genesis block",
                    "-mine block",
                    "-join network",
                    "-view history"
                };

                Console.WriteLine("Please choose one of the following commands:");

                foreach (var command in commands)
                {
                    Console.WriteLine(command);
                }

                var input = Console.ReadLine()?.Trim();

                switch (input)
                {
                    case "-create block":
                        var block = new Block
                        {
                            Timestamp = TimestampHelper.GetTimestamp(DateTime.Now),
                            LastHash = "last hash",
                            Hash = "hash",
                            Data = "the data"
                        };
                        Console.WriteLine(BlockFactory.Create(block));
                        break;

                    case "-create genesis block":
                        BlockFactory.ToStringRepresentation(BlockFactory.Genesis());
                        break;

                    case "-execute mining":
                        BlockProcessor.ExecuteMining();
                        break;

                    case "-join network":
                        //act
                        break;
                    case "-view history":
                        break;
                }

                break;
            }

            NetworkService.Connect();
        }
    }
}
