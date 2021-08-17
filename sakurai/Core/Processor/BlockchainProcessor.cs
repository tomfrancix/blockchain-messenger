using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sakurai.Interface.IFactory;
using sakurai.Interface.IHelper;
using sakurai.Interface.IProcessor;
using sakurai.Objects;

namespace sakurai.Core.Processor
{
    public class BlockchainProcessor : IBlockchainProcessor
    {
        private readonly IBlockFactory BlockFactory;
        private readonly IBlockProcessor BlockProcessor;
        private readonly IHashFactory HashFactory;

        private Blockchain Chain;

        public BlockchainProcessor(IBlockFactory blockFactory, IHashFactory hashFactory, IBlockProcessor blockProcessor)
        {
            BlockFactory = blockFactory;
            HashFactory = hashFactory;
            BlockProcessor = blockProcessor;

            Chain = new Blockchain()
            {
                Blocks = new List<Block>()
            };
        }

        public Block AddBlock(string data)
        {
            var blocks = this.Chain.Blocks;

            if (blocks.Count == 0)
            {
                var lastBlock = BlockProcessor.MineBlock(BlockFactory.Genesis(), "foo");

                Chain.Blocks.Add(lastBlock);
            }

            if (blocks.Count > 0)
            {
                var lastBlock = blocks[^1];

                var newBlock = BlockProcessor.MineBlock(lastBlock, data);

                BlockFactory.ToStringRepresentation(newBlock);

                Chain.Blocks.Add(newBlock);
            }

            return blocks[^1];
        }

        public bool isValidChain(Blockchain chain)
        {
            if (chain.Blocks[0] != BlockFactory.Genesis())
            {
                return false;
            }

            for (var i = 1; i < chain.Blocks.Count; i++)
            {
                var block = chain.Blocks[i];
                var lastBlock = chain.Blocks[i - 1];

                if (block.LastHash != lastBlock.Hash || block.Hash != BlockProcessor.BlockHash(block))
                {
                    return false;
                }
            }

            return true;
        }

        public void ReplaceChain(Blockchain newChain)
        {
            var blocks = this.Chain.Blocks;

            if (newChain.Blocks.Count <= blocks.Count)
            {
                Console.WriteLine("Received chain is not longer than chain in memory.");
            } else if (!isValidChain(newChain))
            {
                Console.WriteLine("Received chain is not valid.");
            }

            Console.WriteLine("Replacing blockchain with the new received chain...");

            this.Chain = newChain;
        }

        public string ToFlatString(Blockchain blockchain)
        {
            var chain = "";

            foreach (var block in blockchain.Blocks)
            {
                var blockString = BlockProcessor.ToFlatString(block);

                chain += blockString + "+";
            }

            return chain;
        }
    }
}
