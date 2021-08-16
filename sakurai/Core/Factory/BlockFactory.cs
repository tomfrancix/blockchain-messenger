using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using sakurai.Interface.IFactory;
using sakurai.Interface.IHelper;
using sakurai.Objects;

namespace sakurai.Core.Factory
{
    public class BlockFactory : IBlockFactory
    {
        private readonly ITimestampHelper TimestampHelper;

        public BlockFactory(ITimestampHelper timestampHelper)
        {
            TimestampHelper = timestampHelper;
        }

        public Block Create(Block values)
        {
            var block = new Block
            {
                Timestamp = values.Timestamp,
                LastHash = values.LastHash,
                Hash = values.Hash,
                Data = values.Data
            };

            return block;
        }

        public Block Genesis()
        {
            var block = new Block
            {
                Timestamp = TimestampHelper.GetTimestamp(DateTime.UtcNow),
                LastHash = "--------",
                Hash = "54kur41-f1rs7-h45h",
                Data = ""
            };

            return block;
        }

        public void ToStringRepresentation(Block values)
        {
            Console.WriteLine(" > Block : |-| Timestamp : " + values.Timestamp);
            Console.WriteLine("           |-| Last Hash : " + values.LastHash);
            Console.WriteLine("           |-| This Hash : " + values.Hash);
            Console.WriteLine("           |-| This Data : " + values.Data);
            Console.WriteLine("");
        }
    }
}
