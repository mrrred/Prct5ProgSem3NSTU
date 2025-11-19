using BoolMatrixFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLFramework.XMLInteraction.Abstractions
{
    public interface IXDocEditor<T>
    {
        void Add(T item);

        BoolMatrix Pop(int id);

        void Edit(T item, int id);
    }
}
