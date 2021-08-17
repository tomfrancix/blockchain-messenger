using System.Runtime.Serialization;

namespace sakurai.Objects
{
    [DataContract]
    public class Block
    {
        [DataMember]
        public string Timestamp { get; set; }

        [DataMember]
        public string LastHash { get; set; }

        [DataMember]
        public string Hash { get; set; }

        [DataMember]
        public string Data { get; set; }
    }
}
