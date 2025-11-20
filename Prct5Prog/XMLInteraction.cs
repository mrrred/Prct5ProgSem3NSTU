using BoolMatrixFramework;
using System.Collections.Generic;

namespace Prct5Prog
{
    public class XMLInteraction
    {
        private XMLBoolMatrixDocManagerService _service;

        public XMLInteraction(string filePath)
        {
            _service = new XMLBoolMatrixDocManagerService(filePath);
        }

        public void Add(BoolMatrix matrix) => _service.Add(matrix);

        public void EditElement(int id, BoolMatrix matrix) => _service.EditElement(id, matrix);

        public BoolMatrix Pop(int id) => _service.Pop(id);

        public Dictionary<string, BoolMatrix> SearchOnAttributes(Dictionary<string, string> attributes)
            => _service.SearchOnAttributes(attributes);
    }
}