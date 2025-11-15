using BoolMatrixFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Prct5Prog.XMLFramework
{
    class XMLInteraction : IXMLInteraction
    {
        private readonly XDocument _xDocument;

        public XDocument XDocument
        {
            get { return _xDocument; }
        }

        private readonly string _xDocumentName;
        public XMLInteraction(string xDocumentName)
        {
            _xDocumentName = xDocumentName;

            try
            {
                _xDocument = XDocument.Load(xDocumentName);
            }
            catch (FileNotFoundException)
            {
                _xDocument = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement("Matrices"));
                _xDocument.Save(xDocumentName);
            }
        }

        private string BoolMAtrixToString(BoolMatrix boolMatrix)
        {
            StringBuilder stringBoolMatrix = new StringBuilder();

            for (int i = 0; i < boolMatrix.RowsCount; i++)
            {
                for (int j = 0; j < boolMatrix.CollumnsCount; j++)
                {
                    if (boolMatrix[i, j]) stringBoolMatrix.Append("1,");
                    else stringBoolMatrix.Append("0,");
                }

                stringBoolMatrix.Remove(stringBoolMatrix.Length - 1, 1);
                stringBoolMatrix.Append(';');
            }

            return stringBoolMatrix.ToString();
        }

        public void Add(BoolMatrix boolMatrix)
        {
            XElement xElement = new XElement("Matrix",
                new XAttribute("Id", _xDocument.Root.Elements().Count()),
                new XAttribute("Rows", boolMatrix.RowsCount),
                new XAttribute("Columns", boolMatrix.CollumnsCount),
                new XElement("Data", BoolMAtrixToString(boolMatrix))
                );


            _xDocument.Root.Add(xElement);

            _xDocument.Save(_xDocumentName);
        }

        public BoolMatrix GetEmelent(int id)
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

        public int Search(BoolMatrix boolMatrix)
        {
            throw new NotImplementedException();
        }
    }
}
