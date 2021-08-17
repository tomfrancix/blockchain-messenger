using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sakurai.Objects;

namespace sakurai.Interface.IService
{
    public interface INetworkService
    {
        void ListenToPeers();

        void BroadcastToPeers(Blockchain blockchain);
    }
}
