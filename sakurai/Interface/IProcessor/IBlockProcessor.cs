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
        void ExecuteMining();

        Block MineBlock(Block lastBlock, string data);
    }
}
