## Blockchain Messenger

This project demonstrates how a block chain can be used for sharing messages between different instances of a C# console application.

When the console application is run there are two choices:
- Listen
- Broadcast

Any running listeners will receive any (string) message that is broadcast and the data is added to a block:

```
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
```

and the block is added to the blockchain:

```
var lastBlock = blocks[^1];

var newBlock = BlockProcessor.MineBlock(lastBlock, data);

Chain.Blocks.Add(newBlock);
```

Each listener node validates the new chain:

```
public bool isValidChain(Blockchain chain)
{
    var genesisBlock = BlockFactory.Genesis();

    if (chain.Blocks[0].Timestamp != genesisBlock.Timestamp)
    {
        return false;
    }
    if (chain.Blocks[0].LastHash != genesisBlock.LastHash)
    {
        return false;
    }
    if (chain.Blocks[0].Hash != genesisBlock.Hash)
    {
        return false;
    }
    if (chain.Blocks[0].Data != genesisBlock.Data)
    {
        return false;
    }

    for (var i = 1; i < chain.Blocks.Count; i++)
    {
        var block = chain.Blocks[i];
        var lastBlock = chain.Blocks[i - 1];

        var blockHash = BlockProcessor.BlockHash(block);
        if (blockHash != lastBlock.Hash)
        {
            return false;
        }
    }

    return true;
}
```
 If the genesis block is invalid if:
- the genesis block is not equal to the block factory genesis block.
- if any of the hashes of subsequent blocks are not equal.

