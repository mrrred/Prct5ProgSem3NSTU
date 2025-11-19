using BoolMatrixFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using XMLFramework.Deserializators.Abstractions;
using XMLFramework.Serializators.Abstractions;
using XMLFramework.XMLConfigurations.Abstractions;
using XMLFramework.XMLIdManagers.Abstractions;
using XMLFramework.XMLInteraction.Abstractions;

namespace XMLFramework.XMLInteraction
{
    // ПОДРЕДАЧИТЬ СКОПИРОВАННЫЙ КОД
    public class XDocBoolMatrixEditor : IXDocEditor<BoolMatrix>
    {
        private XDocument _xDoc;

        private IXMLBoolMatrixConfiguration _config;

        private IXmlElementBuilder<BoolMatrix> _xmlElementBuilder;

        private IXMLIdManager _idManager;

        private ISerializator<BoolMatrix> _serializator;

        private IDeserializator<BoolMatrix> _deserializator;

        public XDocBoolMatrixEditor(XDocument xDocument, IXMLBoolMatrixConfiguration config, 
            IXmlElementBuilder<BoolMatrix> xmlElementBuilder, IXMLIdManager idManager,
            ISerializator<BoolMatrix> serializator, IDeserializator<BoolMatrix> deserializator)
        {
            _xDoc = xDocument;

            _config = config;

            _xmlElementBuilder = xmlElementBuilder;

            _idManager = idManager;

            _serializator = serializator;

            _deserializator = deserializator;
        }

        public void Add(BoolMatrix boolMatrix)
        {
            _xDoc.Root.Add(_xmlElementBuilder.BuildElement(boolMatrix, _idManager.NextId()));
        }

        public BoolMatrix Pop(int id)
        {
            var element = _xDoc!.Root!
                .Elements(_config.XMLMatrixElementName)
                .FirstOrDefault(el => Convert.ToInt32(el?.Attribute(_config.IDAttributeName)?.Value
                ?? throw new ArgumentException($"Attribute {_config.IDAttributeName} not found", nameof(_config.IDAttributeName))) == id)
                ?? throw new ArgumentException($"Element with ID {id} not found", nameof(id));

            var boolMatrix = _deserializator.Deserialization(element.Value);
            element.Remove();

            var xDocELement = _xDoc!.Root!
                .Elements(_config.XMLMatrixElementName)
                .Where(el => Convert.ToInt32(el?.Attribute(_config.IDAttributeName)?.Value
                ?? throw new ArgumentException($"Attribute {_config.IDAttributeName} not found", nameof(_config.IDAttributeName))) > id);

            foreach (var el in xDocELement)
            {
                if (el != null && el.Attribute(_config.IDAttributeName) != null)
                {
                    el!.Attribute(_config.IDAttributeName)!.Value
                        = (Convert.ToInt32(el.Attribute(_config.IDAttributeName)?.Value
                        ?? throw new ArgumentException($"Attribute {_config.IDAttributeName} not found", nameof(_config.IDAttributeName))) - 1).ToString();
                }
            }

            return boolMatrix;
        }

        public void Edit(BoolMatrix boolMatrix, int id)
        {
            XElement xElement = _xDoc!.Root!
                .Elements(_config.XMLMatrixElementName)
                .FirstOrDefault(el => Convert.ToInt32(el?.Attribute(_config.IDAttributeName)?.Value
                ?? throw new ArgumentException($"Attribute {_config.IDAttributeName} not found", nameof(_config.IDAttributeName))) == id)
                ?? throw new ArgumentException($"Element with ID {id} not found", nameof(id));

            xElement.Value = _serializator.Serialization(boolMatrix);

            xElement.SetAttributeValue(_config.RowsCountAttributeName, boolMatrix.RowsCount.ToString());

            xElement.SetAttributeValue(_config.ColumnCountAttributeName, boolMatrix.CollumnsCount.ToString());
        }
    }
}
