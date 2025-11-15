using BoolMatrixFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prct5Prog.XMLFramework
{
    interface IXMLInteraction
    {
        void Add(BoolMatrix boolMatrix);

        BoolMatrix Pop(int id);

        int Search(BoolMatrix boolMatrix);

        void EditElement(int id, BoolMatrix boolMatrix);

    }
}
