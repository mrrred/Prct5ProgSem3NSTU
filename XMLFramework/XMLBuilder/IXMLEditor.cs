using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLFramework.XMLBuilder
{
    public interface IXMLEditor<T>
    {
        void Add(T item);

        void Remove(int id);

        void Edit(T item);
    }
}
