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
    public class BlockProcessor : IBlockProcessor
    {
        private readonly IBlockFactory BlockFactory;
        private readonly ITimestampHelper TimestampHelper;
        private readonly IHashFactory HashFactory;

        public BlockProcessor(IBlockFactory blockFactory, ITimestampHelper timestampHelper, IHashFactory hashFactory)
        {
            BlockFactory = blockFactory;
            TimestampHelper = timestampHelper;
            HashFactory = hashFactory;
        }

        public Block MineBlock(Block lastBlock, string data)
        {
            var nextBlock = new Block
            {
                Timestamp = TimestampHelper.GetTimestamp(DateTime.UtcNow),
                LastHash = lastBlock.Hash,
                Data = data
            };

            nextBlock.Hash = HashFactory.GenerateHash(nextBlock.Timestamp, nextBlock.LastHash, nextBlock.Data);

            return nextBlock;
        }

        public void ExecuteMining()
        {
            var lastBlock = MineBlock(BlockFactory.Genesis(), "foo");

            for (var i = 0; i < 100; i++)
            {
                var newBlock = MineBlock(lastBlock, "data" + i);

                BlockFactory.ToStringRepresentation(newBlock);

                lastBlock = newBlock;
            }

            throw new NotImplementedException();
        }
    }
}
