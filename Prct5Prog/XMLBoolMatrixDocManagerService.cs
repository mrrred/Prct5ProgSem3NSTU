using BoolMatrixFramework;
using XMLFramework.Deserializators;
using XMLFramework.Serializators;
using XMLFramework.XMLConfigurations;
using XMLFramework.XMLDocumentManager;
using XMLFramework.XMLDocumentManager.Abstractions;
using XMLFramework.XMLFiles;
using XMLFramework.XMLIdManagers;
using XMLFramework.XMLInteraction;

namespace Prct5Prog
{
    class XMLBoolMatrixDocManagerService
    {
        private IDocumentManager<BoolMatrix> _documentManager;

        public XMLBoolMatrixDocManagerService(string filePath)
        {
            var configuration = new XMLBoolMatrixConfiguration();

            var xMLFile = new XMLFile(filePath, configuration);

            var serial = new BoolMatrixSerializator(configuration);

            var deSerial = new BoolmatrixDeserializator(configuration);

            var xDocEditor = new XDocBoolMatrixEditor(xMLFile.XML, configuration,
                new XMLBoolMatrixElementBuilder(xMLFile.XML, configuration, serial),
                new XMLIdManager(xMLFile.XML, configuration), serial, deSerial);

            var xDocSerch = new XDocSearcher(xMLFile.XML, deSerial, configuration);

            _documentManager = new BoolMatrixDocumentManager(xMLFile, xDocEditor, xDocSerch);
        }

        public void Add(BoolMatrix obj) => _documentManager.Add(obj);

        public void EditElement(int id, BoolMatrix obj) => _documentManager.EditElement(id, obj);

        public BoolMatrix GetElement(int id) => _documentManager.GetElement(id);

        public BoolMatrix Pop(int id) => _documentManager.Pop(id);

        public Dictionary<string, BoolMatrix> SearchOnAttributes(Dictionary<string, string> attributes) => _documentManager.SearchOnAttributes(attributes);
    }
}
