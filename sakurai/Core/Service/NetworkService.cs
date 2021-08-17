using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Extensions.Logging;
using sakurai.Core.Factory;
using sakurai.Core.Processor;
using sakurai.Interface.IFactory;
using sakurai.Interface.IHelper;
using sakurai.Interface.IProcessor;
using sakurai.Interface.IService;
using sakurai.Objects;

namespace sakurai.Core.Service
{
    public class NetworkService : INetworkService
    {
        private readonly ILogger<NetworkService> Logger;
        private readonly IObjectBytesHelper ObjectBytesHelper;
        private readonly IBlockchainProcessor BlockchainProcessor;
        private readonly IBlockFactory BlockFactory;

        private Blockchain Chain;

        public NetworkService(ILoggerFactory loggerFactory, IBlockchainProcessor blockchainProcessor, IBlockFactory blockFactory)
        {
            BlockchainProcessor = blockchainProcessor;
            BlockFactory = blockFactory;
            Logger = loggerFactory.CreateLogger<NetworkService>();
            Chain = new Blockchain()
            {
                Blocks = new List<Block>()
            };
        }

        public void ListenToPeers()
        {
            Chain.Blocks = new List<Block> {BlockFactory.Genesis()};

            Console.WriteLine("\nInitializing server...");

            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            Console.WriteLine("   -| Host : " + host);
            Console.WriteLine("   -| IP Address : " + ipAddress);
            Console.WriteLine("   -| Endpoint : " + localEndPoint);

            try
            {

                // Create a Socket that will use Tcp protocol      
                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                listener.Bind(localEndPoint);

               

                Console.WriteLine("\n   Node is ACTIVE.");

                var listening = true;

                while (listening)
                {
                    // Specify how many requests a Socket can listen before it gives Server busy response.  
                    // We will listen 10 requests at a time  
                    listener.Listen(10);
                    Console.WriteLine("\n   Waiting for a connection...");

                    Socket handler = listener.Accept();

                    // Incoming data from the client.    
                    string data = null;
                    byte[] bytes = null;
                    var newBlockchain = new Blockchain()
                        {
                            Blocks = new List<Block>()
                        };
                    while (true)
                    {
                        
                        bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                        var dataBlocks = data.Split("+++");
                        foreach (var dataBlock in dataBlocks)
                        {
                            var dataBlockValues = dataBlock.Split("::");

                            if (dataBlockValues.Length == 4)
                            {
                                var newBlock = new Block
                                {
                                    Timestamp = dataBlockValues[0],
                                    LastHash = dataBlockValues[1],
                                    Hash = dataBlockValues[2],
                                    Data = dataBlockValues[3]
                                };

                                newBlockchain.Blocks.Add(newBlock);
                            }
                        }

                        break;
                    }

                    Console.WriteLine("\nReceived blockchain:\n");

                    if (newBlockchain.Blocks.Count > Chain.Blocks.Count)
                    {
                        if (BlockchainProcessor.isValidChain(newBlockchain))
                        {
                            Chain = BlockchainProcessor.ReplaceChain(newBlockchain);
                        }
                    }
                    else
                    {
                        for (var i = 0; i < Chain.Blocks.Count; i++)
                        {
                            var knownData = Chain.Blocks[i];

                            if (i < newBlockchain.Blocks.Count)
                            {
                                var newData = newBlockchain.Blocks[i];
                                if (knownData.Timestamp == newData.Timestamp)
                                {
                                    continue;
                                }
                                else
                                {
                                    var newInfo = newData.Data;
                                    BlockchainProcessor.AddBlock(Chain, newInfo);
                                }
                            }
                        }
                    }

                    foreach (var block in Chain.Blocks)
                    {
                        BlockFactory.ToStringRepresentation(block);
                    }

                    byte[] msg = Encoding.ASCII.GetBytes(BlockchainProcessor.ToFlatString(Chain));
                    handler.Send(msg);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void BroadcastToPeers(Blockchain blockchain)
        {
            Console.WriteLine("\n  Connecting to node...");

            byte[] bytes = new byte[1024];

            try
            {
                // Connect to a Remote server  
                // Get Host IP Address that is used to establish a connection  
                // In this case, we get one IP address of localhost that is IP : 127.0.0.1  
                // If a host has multiple addresses, you will get a list of addresses  
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);


                Console.WriteLine("   -| Host : " + host);
                Console.WriteLine("   -| IP Address : " + ipAddress);
                Console.WriteLine("   -| Endpoint : " + remoteEP);

                // Create a TCP/IP socket.    
                Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
 
                try
                {
                    // Connect to Remote EndPoint  
                    sender.Connect(remoteEP);

                    Console.WriteLine("\n  Socket connected to {0}", sender.RemoteEndPoint);

                    // Encode the data string into a byte array.    
                    byte[] msg = Encoding.ASCII.GetBytes(BlockchainProcessor.ToFlatString(blockchain));

                    // Send the data through the socket.    
                    int bytesSent = sender.Send(msg);

                    // Receive the response from the remote device.    
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine("\n  Received chain.");

                    string data = null;
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    var newBlockchain = new Blockchain()
                    {
                        Blocks = new List<Block>()
                    };

                    var dataBlocks = data.Split("+++");
                    foreach (var dataBlock in dataBlocks)
                    {
                        var dataBlockValues = dataBlock.Split("::");

                        if (dataBlockValues.Length == 4)
                        {
                            var newBlock = new Block
                            {
                                Timestamp = dataBlockValues[0],
                                LastHash = dataBlockValues[1],
                                Hash = dataBlockValues[2],
                                Data = dataBlockValues[3]
                            };

                            newBlockchain.Blocks.Add(newBlock);
                        }
                    }

                    if (BlockchainProcessor.isValidChain(newBlockchain))
                    {
                        if (newBlockchain.Blocks.Count > Chain.Blocks.Count)
                        {
                            Console.WriteLine("\n  Found longer chain.\n");
                            Chain = BlockchainProcessor.ReplaceChain(newBlockchain);
                        }
                    }
                    

                    // Release the socket.    
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                    foreach (var block in Chain.Blocks)
                    {
                        BlockFactory.ToStringRepresentation(block);
                    }

                    Console.WriteLine("\n  Chain Length : " + Chain.Blocks.Count);

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("\nArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("\nSocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nUnexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void StartNode()
        {
            Console.WriteLine("\nStarting node...");
            ListenToPeers();
        }

        public void BroadcastData()
        {
            while (true)
            {
                Console.WriteLine("\nEnter a message...");
                var message = Console.ReadLine();

                if (Chain.Blocks.Count < 1)
                {
                    Chain.Blocks.Add(BlockFactory.Genesis());
                }

                BlockchainProcessor.AddBlock(Chain, message);

                Console.WriteLine("\nSharing data with network...");

                BroadcastToPeers(Chain);
            }
        }
    }
}
