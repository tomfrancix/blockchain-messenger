using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sakurai.Objects;

namespace sakurai.Interface.IProcessor
{
    public interface IBlockProcessor
    {
        Block MineBlock(Block lastBlock, string data);

        string BlockHash(Block block);
        string ToFlatString(Block block);
    }
}
