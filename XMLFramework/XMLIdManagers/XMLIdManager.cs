using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XMLFramework.XMLConfigurations.Abstractions;
using XMLFramework.XMLIdManagers.Abstractions;

namespace XMLFramework.XMLIdManagers
{
    public class XMLIdManager : IXMLIdManager
    {
        private XDocument _xDoc;

        IXMLConfiguration _config;

        public XMLIdManager(XDocument xDoc, IXMLConfiguration config)
        {
            _xDoc = xDoc;
            _config = config;
        }

        public int NextId() => _xDoc!.Root!.Elements(_config.XMLMatrixElementName).Count();
    }
}
