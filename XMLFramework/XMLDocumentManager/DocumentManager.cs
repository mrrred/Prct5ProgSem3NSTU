using BoolMatrixFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using XMLFramework.XMLConfigurations.Abstractions;
using XMLFramework.XMLDocumentManager.Abstractions;
using XMLFramework.XMLInteraction.Abstractions;

namespace XMLFramework.XMLDocumentManager
{
    public class DocumentManager : IDocumentManager<BoolMatrix>
    {
        public string XMLDocumentName { get; }

        public XDocument XDocument { get; }

        private IXMLBoolMatrixConfiguration _config;

        private IXDocEditor<BoolMatrix> _xDocEditor;

        private IXDocSearcher<BoolMatrix> _xDocSearcher;

        public DocumentManager(string xDocumentName,
            IXMLBoolMatrixConfiguration config,
            IXDocEditor<BoolMatrix> xMLEditor,
            IXDocSearcher<BoolMatrix> xDocSearcher)
        {
            XMLDocumentName = xDocumentName
                ?? throw new ArgumentNullException(nameof(xDocumentName));

            _config = config
                ?? throw new ArgumentNullException(nameof(config));

            _xDocEditor = xMLEditor;

            _xDocSearcher = xDocSearcher;

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
            return _xDocSearcher.SearchOnId(id);
        }

        public BoolMatrix Pop(int id)
        {
            var boolMatrix = _xDocEditor.Pop(id);

            XDocument.Save(XMLDocumentName);

            return boolMatrix;

        }

        public void EditElement(int id, BoolMatrix boolMatrix)
        {
            _xDocEditor.Edit(boolMatrix, id);
        }

        public Dictionary<string, BoolMatrix> SearchOnAttributes(Dictionary<string, string>? attributes = null)
        {
            return _xDocSearcher.SearchOnAttribute(attributes);
        }
    }
}
