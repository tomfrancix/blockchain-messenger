using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sakurai.Interface.IFactory
{
    public interface IFactory<T>
    {
        T Create(T values);

        void ToStringRepresentation(T values);
    }
}
