using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLFramework.Serializators
{
    public interface ISerializator<T>
    {
        public string Deserialization(T obj);
    }
}
