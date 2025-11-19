using BoolMatrixFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using XMLFramework.Deserializators;
using XMLFramework.Extensions;
using XMLFramework.Serializators;
using XMLFramework.XMLConfigurations;
using XMLFramework.XMLIdManagers;

namespace XMLFramework
{
    public class XMLDocumentManager2 : IXMLDocumentManager2<BoolMatrix>
    {
        public string XMLDocumentName { get; }

        public XDocument XDocument { get; }

        private IXMLBoolMatrixConfiguration _config;

        private IXMLIdManager _idManager;

        private ISerializator<BoolMatrix> _boolMatrixSerializator;

        private IDeserializator<BoolMatrix> _boolMatrixDeserializator;

        public XMLDocumentManager2(string xDocumentName,
            IXMLBoolMatrixConfiguration config,
            IXMLIdManager idManager,
            ISerializator<BoolMatrix> boolMatrixSerializator,
            IDeserializator<BoolMatrix> boolMatrixDeserializator)
        {
            XMLDocumentName = xDocumentName
                ?? throw new ArgumentNullException(nameof(xDocumentName));

            _config = config
                ?? throw new ArgumentNullException(nameof(config));

            _idManager = idManager
                ?? throw new ArgumentNullException(nameof(idManager));

            _boolMatrixSerializator = boolMatrixSerializator
                ?? throw new ArgumentNullException(nameof(boolMatrixSerializator));

            _boolMatrixDeserializator = boolMatrixDeserializator
                ?? throw new ArgumentNullException(nameof(boolMatrixDeserializator));

            try
            {
                XDocument = XDocument.Load(xDocumentName);
            }
            catch (FileNotFoundException)
            {
                XDocument = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement(_config.XMLRootName));
                XDocument.Save(XMLDocumentName);
            }

            if (XDocument?.Root == null)
                throw new InvalidOperationException("XML document root is missing");
        }

        public void Add(BoolMatrix boolMatrix)
        {
            ArgumentNullException.ThrowIfNull(boolMatrix, nameof(boolMatrix));

            XElement xElement = new XElement(_config.XMLMatrixElementName,
                new XAttribute(_config.IDAttributeName, _idManager.NextId()),
                new XAttribute(_config.RowsCountAttributeName, boolMatrix.RowsCount),
                new XAttribute(_config.ColumnCountAttributeName, boolMatrix.CollumnsCount)
                );

            xElement.Value = _boolMatrixSerializator.Serialization(boolMatrix);

            XDocument.Root.Add(xElement);

            XDocument.Save(XMLDocumentName);
        }

        public BoolMatrix GetElement(int id)
        {
            throw new NotImplementedException();
        }

        public BoolMatrix Pop(int id)
        {
            throw new NotImplementedException();
        }

        public void EditElement(int id, BoolMatrix boolMatrix)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, BoolMatrix> SearchOnAttributes(Dictionary<string, string>? attributes = null)
        {
            throw new NotImplementedException();
        }
    }
}
