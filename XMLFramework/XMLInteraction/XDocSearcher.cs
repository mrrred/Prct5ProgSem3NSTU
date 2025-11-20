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
            _xDoc = xDoc ?? throw new ArgumentNullException(nameof(xDoc));

            if (xDoc.Root == null) throw new ArgumentNullException("Root is missing");

            _deserializator = deserializator ?? throw new ArgumentNullException(nameof(deserializator));

            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        private int GetElementId(XElement element)
        {
            return Convert.ToInt32(element.Attribute(_config.IDAttributeName)?.Value
                ?? throw new ArgumentException($"Attribute {_config.IDAttributeName} not found"));
        }

        public BoolMatrix SearchOnId(int id)
        {
            var element = _xDoc!.Root!.Elements(_config.XMLMatrixElementName)
                .FirstOrDefault(el => GetElementId(el) == id)
                ?? throw new ArgumentException($"Element with ID {id} not found");

            return _deserializator.Deserialization(element.Value);
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
