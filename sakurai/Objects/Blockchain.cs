using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace sakurai.Objects
{
    [DataContract]
    public class Blockchain
    {
        [DataMember]
        public List<Block> Blocks { get; set; }
    }
}
