using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XMLFramework.XMLConfigurations.Abstractions;
using XMLFramework.XMLFiles.Abstractions;

namespace XMLFramework.XMLFiles
{
    public class XMLFile : IXMLFile
    {
        public XDocument XML { get; }

        private string _fileName;

        public XMLFile(string fileName, string rootName)
        {
            _fileName = fileName;

            try
            {
                XML = XDocument.Load(fileName);
            }
            catch (FileNotFoundException)
            {
                XML = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement(rootName));
                Save();
            }

            if (XML?.Root == null)
                throw new InvalidOperationException("XML document root is missing");
        }

        public XMLFile(string fileName, IXMLConfiguration xMLConfiguration) : this(fileName, xMLConfiguration.XMLRootName) { }

        public void Save()
        {
            XML.Save(_fileName);
        }
    }
}
