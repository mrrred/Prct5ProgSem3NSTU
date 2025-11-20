using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLFramework.Serializators.Abstractions
{
    public interface ISerializator<T>
    {
        public string Serialization(T obj);
    }
}
