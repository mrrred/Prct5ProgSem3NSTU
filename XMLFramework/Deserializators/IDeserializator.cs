using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLFramework.Deserializators
{
    public interface IDeserializator<T>
    {
        public T Deserialization(string obj);
    }
}
