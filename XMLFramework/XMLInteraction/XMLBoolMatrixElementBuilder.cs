using BoolMatrixFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using XMLFramework.Serializators.Abstractions;
using XMLFramework.XMLConfigurations.Abstractions;
using XMLFramework.XMLInteraction.Abstractions;

namespace XMLFramework.XMLInteraction
{
    public class XMLBoolMatrixElementBuilder : IXmlElementBuilder<BoolMatrix>
    {
        private XDocument _xDoc;

        private IXMLBoolMatrixConfiguration _config;

        private ISerializator<BoolMatrix> _serializator;

        public XMLBoolMatrixElementBuilder(XDocument xDoc, IXMLBoolMatrixConfiguration config, ISerializator<BoolMatrix> serializator)
        {
            _xDoc = xDoc
                ?? throw new ArgumentNullException(nameof(xDoc));

            _config = config
                ?? throw new ArgumentNullException(nameof(config));

            _serializator = serializator
                ?? throw new ArgumentNullException(nameof(serializator));
        }

        public XElement BuildElement(BoolMatrix boolMatrix, int id)
        {
            XElement xElement = new XElement(_config.XMLMatrixElementName,
                new XAttribute(_config.IDAttributeName, _xDoc!.Root!.Elements(_config.XMLMatrixElementName).Count()),
                new XAttribute(_config.RowsCountAttributeName, boolMatrix.RowsCount),
                new XAttribute(_config.ColumnCountAttributeName, boolMatrix.ColumnsCount)
                );

            xElement.Value = _serializator.Serialization(boolMatrix);

            return xElement;
        }
    }
}
