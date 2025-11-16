using BoolMatrixFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
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

            stringBoolMatrix.Remove(stringBoolMatrix.Length - 1, 1);

            return stringBoolMatrix.ToString();
        }

        private BoolMatrix stringToBoolMatrix(string stringBoolMatrix, string rowsSeparator = ";", string collumnsSeparator = ",")
        {
            if (string.IsNullOrEmpty(stringBoolMatrix))
                throw new ArgumentException("Input string cannot be null or empty");

            var rowsStrings = stringBoolMatrix.Split(rowsSeparator, StringSplitOptions.RemoveEmptyEntries);

            int rows = rowsStrings.Length;

            int collumns = rowsStrings[0].Split(collumnsSeparator).Length;

            // Проверки на 0 добавить

            BoolMatrix resultBoolMatrix = new(rows, collumns);

            for (int i = 0; i < rowsStrings.Length; i++)
            {
                var collumnsArray = rowsStrings[i].Split(collumnsSeparator, StringSplitOptions.RemoveEmptyEntries);

                if (collumnsArray.Length != collumns)
                {
                    throw new ArgumentException("The length of the lines is not uniform");
                }

                for (int j = 0; j < collumnsArray.Length; j++)
                {
                    resultBoolMatrix[i, j] = collumnsArray[j] == "1" ? true : false;
                }
            }

            return resultBoolMatrix;
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
            XElement searchElement = _xDocument.Root.Elements().FirstOrDefault(el => Convert.ToInt32(el.Attribute("Id").Value) == id);

            return stringToBoolMatrix(searchElement.Value);
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
