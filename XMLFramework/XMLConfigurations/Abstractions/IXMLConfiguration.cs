using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLFramework.XMLConfigurations.Abstractions
{
    public interface IXMLConfiguration
    {
        public string XMLRootName { get; }

        public string XMLMatrixElementName { get; }

        public string IDAttributeName { get; }
    }
}
