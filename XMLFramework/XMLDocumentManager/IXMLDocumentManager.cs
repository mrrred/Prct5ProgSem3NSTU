using BoolMatrixFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XMLFramework
{
    public interface IXMLDocumentManager
    {
        public XDocument XDocument { get; }

        void Add(BoolMatrix boolMatrix);

        BoolMatrix GetElement(int id);

        BoolMatrix Pop(int id);

        void EditElement(int id, BoolMatrix boolMatrix);

        Dictionary<string, BoolMatrix> SearchOnAttributes(Dictionary<string, string> attributes);
    }
}
