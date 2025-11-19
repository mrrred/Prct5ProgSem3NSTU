using BoolMatrixFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XMLFramework.Deserializators.Abstractions;
using XMLFramework.XMLConfigurations.Abstractions;
using XMLFramework.XMLInteraction.Abstractions;

namespace XMLFramework.XMLInteraction
{
    public class XDocSearcher : IXDocSearcher<BoolMatrix>
    {
        private XDocument _xDoc;

        private IDeserializator<BoolMatrix> _deserializator;

        private IXMLBoolMatrixConfiguration _config;

        public XDocSearcher(XDocument xDoc, IDeserializator<BoolMatrix> deserializator, IXMLBoolMatrixConfiguration config)
        {
            _xDoc = xDoc;

            _deserializator = deserializator;

            _config = config;
        }

        public BoolMatrix SearchOnId(int id)
        {
            XElement searchElement = _xDoc!.Root!
                .Elements(_config.XMLMatrixElementName)
                .FirstOrDefault(el => Convert.ToInt32(el?.Attribute(_config.IDAttributeName)?.Value
                ?? throw new ArgumentException($"Attribute {_config.IDAttributeName} not found", nameof(_config.IDAttributeName))) == id)
                ?? throw new ArgumentException($"Element with ID {id} not found", nameof(id));

            return _deserializator.Deserialization(searchElement.Value);
        }

        public Dictionary<string, BoolMatrix> SearchOnAttribute(Dictionary<string, string>? attributes = null)
        {
            Dictionary<string, BoolMatrix> result = [];

            foreach (var a in _xDoc!.Root!.Elements().Where(el => AttributeMatching(el, attributes)))
            {
                result.Add(a?.Attribute(_config.IDAttributeName)?.Value
                    ?? throw new ArgumentException($"Attribute {_config.IDAttributeName} not found", nameof(_config.IDAttributeName)),
                    _deserializator.Deserialization(a.Value));
            }

            return result;
        }

        private bool AttributeMatching(XElement xElement, Dictionary<string, string>? attributes)
        {
            if (attributes == null) return true;

            foreach (var (attribute, value) in attributes)
            {
                string valueOnAttribute = xElement?.Attribute(attribute)?.Value
                    ?? throw new ArgumentException($"Attribute {attribute} not found", nameof(attribute));

                if (valueOnAttribute != value)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
