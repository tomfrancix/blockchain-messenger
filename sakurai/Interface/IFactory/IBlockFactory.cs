using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sakurai.Objects;

namespace sakurai.Interface.IFactory
{
    public interface IBlockFactory : IFactory<Block>
    {
        Block Genesis();
    }
}
