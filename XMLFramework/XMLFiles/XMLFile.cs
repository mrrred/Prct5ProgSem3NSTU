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

        private string _filePath;

        public XMLFile(string filePath, string rootName)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(filePath, "File path cannot be empty or null");

            _filePath = filePath;

            try
            {
                XML = XDocument.Load(filePath);
            }
            catch (FileNotFoundException)
            {
                XML = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement(rootName));
                Save();
            }
            catch (Exception ex) when (ex is DirectoryNotFoundException or UnauthorizedAccessException)
            {
                throw new InvalidOperationException($"Cannot access file: {filePath}", ex);
            }

            if (XML?.Root == null)
                throw new InvalidOperationException("XML document root is missing");
        }

        public XMLFile(string filePath, IXMLConfiguration xMLConfiguration) : this(filePath, xMLConfiguration.XMLRootName) { }

        public void Save()
        {
            XML.Save(_filePath);
        }
    }
}
