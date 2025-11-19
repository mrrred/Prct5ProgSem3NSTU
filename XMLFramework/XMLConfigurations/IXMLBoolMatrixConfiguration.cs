using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLFramework.XMLConfigurations
{
    public interface IXMLBoolMatrixConfiguration : IXMLConfiguration
    {
        public string RowsCountAttributeName { get; }

        public string ColumnCountAttributeName { get; }
    }
}
