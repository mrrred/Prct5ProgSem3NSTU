using BoolMatrixFramework;
using System.Collections.Generic;
using XMLFramework.XMLConfigurations;
using XMLFramework.XMLDocumentManager;
using XMLFramework.XMLFiles;
using XMLFramework.XMLInteraction;
using XMLFramework.XMLIdManagers;
using XMLFramework.Serializators;
using XMLFramework.Deserializators;

namespace Prct5Prog.Services
{
    public class MatrixService
    {
        private readonly BoolMatrixDocumentManager _manager;

        public MatrixService(string filePath = "matrices.xml")
        {
            var config = new XMLBoolMatrixConfiguration();
            var xmlFile = new XMLFile(filePath, config);
            var serializator = new BoolMatrixSerializator(config);
            var deserializator = new BoolmatrixDeserializator(config);
            var idManager = new XMLIdManager(xmlFile.XML, config);
            var elementBuilder = new XMLBoolMatrixElementBuilder(xmlFile.XML, config, serializator);

            var editor = new XDocBoolMatrixEditor(
                xmlFile.XML, config, elementBuilder, idManager, serializator, deserializator);

            var searcher = new XDocSearcher(xmlFile.XML, deserializator, config);

            _manager = new BoolMatrixDocumentManager(xmlFile, editor, searcher);
        }

        public void Add(BoolMatrix matrix) => _manager.Add(matrix);
        public void Update(int id, BoolMatrix matrix) => _manager.EditElement(id, matrix);
        public void Delete(int id) => _manager.Pop(id);
        public BoolMatrix Get(int id) => _manager.GetElement(id);
        public Dictionary<string, BoolMatrix> GetAll() => _manager.SearchOnAttributes(new Dictionary<string, string>());
    }
}