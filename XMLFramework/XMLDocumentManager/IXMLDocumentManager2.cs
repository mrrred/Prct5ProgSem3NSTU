using BoolMatrixFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XMLFramework
{
    public interface IXMLDocumentManager2<T>
    {
        public XDocument XDocument { get; }

        void Add(T obj);

        BoolMatrix GetElement(int id);

        BoolMatrix Pop(int id);

        void EditElement(int id, T obj);

        Dictionary<string, T> SearchOnAttributes(Dictionary<string, string> attributes);
    }
}
