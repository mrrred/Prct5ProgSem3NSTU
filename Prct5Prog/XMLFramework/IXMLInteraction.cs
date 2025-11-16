using BoolMatrixFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Prct5Prog.XMLFramework
{
    interface IXMLInteraction
    {
        public XDocument XDocument { get; }

        void Add(BoolMatrix boolMatrix);

        BoolMatrix GetElement(int id);

        BoolMatrix Pop(int id);

        void EditElement(int id, BoolMatrix boolMatrix);

        int Search(BoolMatrix boolMatrix);
    }
}
