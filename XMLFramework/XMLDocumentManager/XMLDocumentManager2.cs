using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using XMLFramework.Extensions;
using BoolMatrixFramework;

namespace XMLFramework
{
    public class XMLDocumentManager2 : IXMLDocumentManager
    {
        public XDocument XDocument { get; }

        public string XMLDocumentName { get; }

        public string XMLRootName { get; }

        public string XMLMatrixElementName { get; }

        public string IDAttributeName { get; }

        public string RowsCountAttributeName { get; }

        public string ColumnCountAttributeName { get; }

        public XMLDocumentManager2(string xDocumentName,
            string xmlRootName = "Matrices",
            string xmlMatrixElementName = "Matrix",
            string idAttributeName = "Id",
            string rowsCountAttributeName = "Rows",
            string columnCountAttributeName = "Column")
        {
            XMLDocumentName = xDocumentName
                ?? throw new ArgumentNullException(nameof(xDocumentName));

            XMLRootName = xmlRootName
                ?? throw new ArgumentNullException(nameof(xDocumentName));

            XMLMatrixElementName = xmlMatrixElementName
                ?? throw new ArgumentNullException(nameof(xmlMatrixElementName));

            IDAttributeName = idAttributeName
                ?? throw new ArgumentNullException(nameof(idAttributeName));

            RowsCountAttributeName = rowsCountAttributeName
                ?? throw new ArgumentNullException(nameof(rowsCountAttributeName));

            ColumnCountAttributeName = columnCountAttributeName
                ?? throw new ArgumentNullException(nameof(columnCountAttributeName));

            try
            {
                XDocument = XDocument.Load(xDocumentName);
            }
            catch (FileNotFoundException)
            {
                XDocument = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement(XMLRootName));
                XDocument.Save(XMLDocumentName);
            }

            if (XDocument?.Root == null)
                throw new InvalidOperationException("XML document root is missing");
        }

        public void Add(BoolMatrix boolMatrix)
        {
            ArgumentNullException.ThrowIfNull(boolMatrix, nameof(boolMatrix));

            XElement xElement = new XElement(XMLMatrixElementName,
                new XAttribute(IDAttributeName, XDocument!.Root!.Elements(XMLMatrixElementName).Count()),
                new XAttribute(RowsCountAttributeName, boolMatrix.RowsCount),
                new XAttribute(ColumnCountAttributeName, boolMatrix.CollumnsCount)
                );

            xElement.Value = boolMatrix.ConvertToString();

            XDocument.Root.Add(xElement);

            XDocument.Save(XMLDocumentName);
        }

        public BoolMatrix GetElement(int id)
        {
            XElement searchElement = XDocument!.Root!
                .Elements(XMLMatrixElementName)
                .FirstOrDefault(el => Convert.ToInt32(el?.Attribute(IDAttributeName)?.Value
                ?? throw new ArgumentException($"Attribute {IDAttributeName} not found", nameof(IDAttributeName))) == id)
                ?? throw new ArgumentException($"Element with ID {id} not found", nameof(id));

            return searchElement.Value.ConvertToBoolMatrix();
        }

        public BoolMatrix Pop(int id)
        {
            var element = XDocument!.Root!
                .Elements(XMLMatrixElementName)
                .FirstOrDefault(el => Convert.ToInt32(el?.Attribute(IDAttributeName)?.Value
                ?? throw new ArgumentException($"Attribute {IDAttributeName} not found", nameof(IDAttributeName))) == id)
                ?? throw new ArgumentException($"Element with ID {id} not found", nameof(id));

            var boolMatrix = element.Value.ConvertToBoolMatrix();
            element.Remove();

            var xDocELement = XDocument!.Root!
                .Elements(XMLMatrixElementName)
                .Where(el => Convert.ToInt32(el?.Attribute(IDAttributeName)?.Value
                ?? throw new ArgumentException($"Attribute {IDAttributeName} not found", nameof(IDAttributeName))) > id);

            foreach (var el in xDocELement)
            {
                if (el != null && el.Attribute(IDAttributeName) != null)
                {
                    el!.Attribute(IDAttributeName)!.Value
                        = (Convert.ToInt32(el.Attribute(IDAttributeName)?.Value
                        ?? throw new ArgumentException($"Attribute {IDAttributeName} not found", nameof(IDAttributeName))) - 1).ToString();
                }
            }

            XDocument.Save(XMLDocumentName);

            return boolMatrix;
        }

        public void EditElement(int id, BoolMatrix boolMatrix)
        {
            XElement xElement = XDocument!.Root!
                .Elements(XMLMatrixElementName)
                .FirstOrDefault(el => Convert.ToInt32(el?.Attribute(IDAttributeName)?.Value
                ?? throw new ArgumentException($"Attribute {IDAttributeName} not found", nameof(IDAttributeName))) == id)
                ?? throw new ArgumentException($"Element with ID {id} not found", nameof(id));

            xElement.Value = boolMatrix.ConvertToString();

            xElement.SetAttributeValue(RowsCountAttributeName, boolMatrix.RowsCount.ToString());

            xElement.SetAttributeValue(ColumnCountAttributeName, boolMatrix.CollumnsCount.ToString());

            XDocument.Save(XMLDocumentName);
        }

        public Dictionary<string, BoolMatrix> SearchOnAttributes(Dictionary<string, string>? attributes = null)
        {
            Dictionary<string, BoolMatrix> result = [];

            foreach (var a in XDocument!.Root!.Elements().Where(el => AttributeMatching(el, attributes)))
            {
                result.Add(a?.Attribute(IDAttributeName)?.Value
                    ?? throw new ArgumentException($"Attribute {IDAttributeName} not found", nameof(IDAttributeName)),
                    a.Value.ConvertToBoolMatrix());
            }

            return result;
        }

        private bool AttributeMatching(XElement xElement, Dictionary<string, string>? attributes)
        {
            if (attributes == null) return true;

            foreach (var (attribute, value) in attributes)
            {
                string valueOnAttribute = xElement?.Attribute(attribute)?.Value
                    ?? throw new ArgumentException($"Attribute {attribute} not found", nameof(attribute));

                if (valueOnAttribute != value)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
