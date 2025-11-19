using BoolMatrixFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLFramework.XMLInteraction.Abstractions
{
    public interface IXDocSearcher<T>
    {
        T SearchOnId(int id);

        Dictionary<string, T> SearchOnAttribute(Dictionary<string, string>? attributes = null);
    }
}
