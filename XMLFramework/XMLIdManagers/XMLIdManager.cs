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
        private readonly XDocument _xDoc;

        private readonly IXMLConfiguration _config;

        public XMLIdManager(XDocument xDoc, IXMLConfiguration config)
        {
            _xDoc = xDoc
                ?? throw new ArgumentNullException(nameof(xDoc));

            if (xDoc.Root == null) throw new ArgumentNullException("Root is not found");

            _config = config
                ?? throw new ArgumentNullException(nameof(config));
        }

        private int GetElementId(XElement element)
        {
            return Convert.ToInt32(element.Attribute(_config.IDAttributeName)?.Value
                ?? throw new ArgumentException($"Attribute {_config.IDAttributeName} not found"));
        }

        public int NextId()
        {
            var existingIds = _xDoc!.Root!.Elements(_config.XMLMatrixElementName)
                .Select(el => Convert.ToInt32(el.Attribute(_config.IDAttributeName)?.Value ?? "0"))
                .DefaultIfEmpty(0)
                .Max();

            return existingIds + 1;
        }

        public void UpdateIdsAfterDeletion(int removedId)
        {
            var elementsToUpdate = _xDoc!.Root!.Elements(_config.XMLMatrixElementName)
                .Where(el => GetElementId(el) > removedId);

            foreach (var element in elementsToUpdate)
            {
                if (element != null)
                {
                    XAttribute attribute = element!.Attribute(_config.IDAttributeName) 
                        ?? throw new ArgumentException("Attribute not found");

                    attribute.Value = (GetElementId(element) - 1).ToString();
                }
            }
        }
    }
}
