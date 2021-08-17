﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sakurai.Objects;

namespace sakurai.Interface.IProcessor
{
    public interface IBlockchainProcessor
    {
        Block AddBlock(string data);

        bool isValidChain(Blockchain chain);

        void ReplaceChain(Blockchain newChain);

        string ToFlatString(Blockchain blockchain);
    }
}
