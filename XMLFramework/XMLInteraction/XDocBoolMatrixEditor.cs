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
            _xDoc = xDocument
                ?? throw new ArgumentNullException(nameof(xDocument));

            if (_xDoc.Root == null) throw new ArgumentNullException("Root is not found");

            _config = config
                ?? throw new ArgumentNullException(nameof(config));

            _xmlElementBuilder = xmlElementBuilder
                ?? throw new ArgumentNullException(nameof(xmlElementBuilder));

            _idManager = idManager
                ?? throw new ArgumentNullException(nameof(idManager));

            _serializator = serializator
                ?? throw new ArgumentNullException(nameof(serializator));

            _deserializator = deserializator
                ?? throw new ArgumentNullException(nameof(deserializator));
        }

        public void Add(BoolMatrix boolMatrix)
        {
            _xDoc!.Root!.Add(_xmlElementBuilder.BuildElement(boolMatrix, _idManager.NextId()));
        }

        public BoolMatrix Pop(int id)
        {
            var element = FindElementById(id);

            var boolMatrix = _deserializator.Deserialization(element.Value);
            element.Remove();

            _idManager.UpdateIdsAfterDeletion(id);

            return boolMatrix;
        }

        public void Edit(BoolMatrix boolMatrix, int id)
        {
            var element = FindElementById(id);

            UpdateElement(element, boolMatrix);
        }

        private XElement FindElementById(int id)
        {
            return _xDoc.Root!
                .Elements(_config.XMLMatrixElementName)
                .FirstOrDefault(IsElementWithId(id))
                ?? throw new ArgumentException($"Element with ID {id} not found", nameof(id));
        }

        private Func<XElement, bool> IsElementWithId(int id)
        {
            return el =>
            {
                var elementId = GetElementId(el);
                return elementId == id;
            };
        }

        private int GetElementId(XElement element)
        {
            var idAttribute = element.Attribute(_config.IDAttributeName)
                ?? throw new ArgumentException($"Attribute {_config.IDAttributeName} not found",
                nameof(_config.IDAttributeName));

            return Convert.ToInt32(idAttribute.Value);
        }

        private void UpdateElement(XElement element, BoolMatrix boolMatrix)
        {
            element.Value = _serializator.Serialization(boolMatrix);

            element.SetAttributeValue(_config.RowsCountAttributeName, boolMatrix.RowsCount.ToString());

            element.SetAttributeValue(_config.ColumnCountAttributeName, boolMatrix.CollumnsCount.ToString());
        }
    }
}
