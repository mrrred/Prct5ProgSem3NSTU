using BoolMatrixFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using XMLFramework.Deserializators.Abstractions;
using XMLFramework.Extensions;
using XMLFramework.Serializators.Abstractions;
using XMLFramework.XMLConfigurations.Abstractions;
using XMLFramework.XMLDocumentManager.Abstractions;
using XMLFramework.XMLIdManagers.Abstractions;
using XMLFramework.XMLInteraction.Abstractions;

namespace XMLFramework.XMLDocumentManager
{
    public class DocumentManager : IDocumentManager<BoolMatrix>
    {
        public string XMLDocumentName { get; }

        public XDocument XDocument { get; }

        private IXMLBoolMatrixConfiguration _config;

        private IXMLIdManager _idManager;

        private ISerializator<BoolMatrix> _boolMatrixSerializator;

        private IDeserializator<BoolMatrix> _boolMatrixDeserializator;

        private IXDocEditor<BoolMatrix> _xDocEditor;

        public DocumentManager(string xDocumentName,
            IXMLBoolMatrixConfiguration config,
            IXMLIdManager idManager,
            ISerializator<BoolMatrix> boolMatrixSerializator,
            IDeserializator<BoolMatrix> boolMatrixDeserializator,
            IXDocEditor<BoolMatrix> xMLEditor)
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

            _xDocEditor = xMLEditor;

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

            _xDocEditor.Add(boolMatrix);

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
