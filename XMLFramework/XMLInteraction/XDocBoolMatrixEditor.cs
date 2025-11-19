using BoolMatrixFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XMLFramework.XMLConfigurations.Abstractions;
using XMLFramework.XMLIdManagers.Abstractions;
using XMLFramework.XMLInteraction.Abstractions;

namespace XMLFramework.XMLInteraction
{
    public class XDocBoolMatrixEditor : IXDocEditor<BoolMatrix>
    {
        private XDocument _xDoc;

        private IXMLBoolMatrixConfiguration _config;

        private IXmlElementBuilder<BoolMatrix> _xmlElementBuilder;

        private IXMLIdManager _idManager;

        public XDocBoolMatrixEditor(XDocument xDocument, IXMLBoolMatrixConfiguration config, 
            IXmlElementBuilder<BoolMatrix> xmlElementBuilder, IXMLIdManager idManager)
        {
            _xDoc = xDocument;

            _config = config;

            _xmlElementBuilder = xmlElementBuilder;

            _idManager = idManager;
        }

        public void Add(BoolMatrix boolMatrix)
        {
            _xDoc.Root.Add(_xmlElementBuilder.BuildElement(boolMatrix, _idManager.NextId()));
        }

        public void Remove(int id)
        {

        }

        public void Edit(BoolMatrix item)
        {
            throw new NotImplementedException();
        }
    }
}
