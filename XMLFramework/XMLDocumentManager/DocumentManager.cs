using BoolMatrixFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using XMLFramework.XMLConfigurations.Abstractions;
using XMLFramework.XMLDocumentManager.Abstractions;
using XMLFramework.XMLFiles.Abstractions;
using XMLFramework.XMLInteraction.Abstractions;

namespace XMLFramework.XMLDocumentManager
{
    public class DocumentManager : IDocumentManager<BoolMatrix>
    {
        private IXMLFile _xMLFile;

        private IXDocEditor<BoolMatrix> _xDocEditor;

        private IXDocSearcher<BoolMatrix> _xDocSearcher;

        public DocumentManager(IXMLFile xMLFile,
            IXDocEditor<BoolMatrix> xMLEditor,
            IXDocSearcher<BoolMatrix> xDocSearcher)
        {
            _xMLFile = xMLFile;

            _xDocEditor = xMLEditor;

            _xDocSearcher = xDocSearcher;
        }

        public void Add(BoolMatrix boolMatrix)
        {
            ArgumentNullException.ThrowIfNull(boolMatrix, nameof(boolMatrix));

            _xDocEditor.Add(boolMatrix);

            _xMLFile.Save();
        }

        public BoolMatrix GetElement(int id)
        {
            return _xDocSearcher.SearchOnId(id);
        }

        public BoolMatrix Pop(int id)
        {
            var boolMatrix = _xDocEditor.Pop(id);

            _xMLFile.Save();

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
