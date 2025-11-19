using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLFramework.XMLConfigurations.Abstractions;

namespace XMLFramework.XMLConfigurations
{
    public class XMLBoolMatrixConfiguration : IXMLBoolMatrixConfiguration
    {
        public string XMLRootName { get; }

        public string XMLMatrixElementName { get; }

        public string IDAttributeName { get; }

        public string RowsCountAttributeName { get; }

        public string ColumnCountAttributeName { get; }

        public XMLBoolMatrixConfiguration(string xmlRootName = "Matrices",
            string xmlMatrixElementName = "Matrix",
            string idAttributeName = "Id",
            string rowsCountAttributeName = "Rows",
            string columnCountAttributeName = "Column")
        {
            XMLRootName = xmlRootName
                ?? throw new ArgumentNullException(nameof(xmlRootName));

            XMLMatrixElementName = xmlMatrixElementName
                ?? throw new ArgumentNullException(nameof(xmlMatrixElementName));

            IDAttributeName = idAttributeName
                ?? throw new ArgumentNullException(nameof(idAttributeName));

            RowsCountAttributeName = rowsCountAttributeName
                ?? throw new ArgumentNullException(nameof(rowsCountAttributeName));

            ColumnCountAttributeName = columnCountAttributeName
                ?? throw new ArgumentNullException(nameof(columnCountAttributeName));
        }
    }
}
