using BoolMatrixFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLFramework.Serializators.Abstractions;
using XMLFramework.XMLConfigurations.Abstractions;

namespace XMLFramework.Serializators
{
    public class BoolMatrixSerializator : ISerializator<BoolMatrix>
    {
        private string _rowsSeparator;

        private string _columnsSeparator;

        public BoolMatrixSerializator(string rowsSeparator = ";", string columnsSeparator = ",")
        {
            ArgumentNullException.ThrowIfNullOrEmpty(rowsSeparator);
            ArgumentNullException.ThrowIfNullOrEmpty(columnsSeparator);

            _rowsSeparator = rowsSeparator;
            _columnsSeparator = columnsSeparator;
        }

        public BoolMatrixSerializator(IXMLBoolMatrixConfiguration config) : this(config.RowsSeparator, config.ColumnsSeparator) { }

        public string Serialization(BoolMatrix boolMatrix)
        {
            StringBuilder stringBoolMatrix = new StringBuilder();

            for (int i = 0; i < boolMatrix.RowsCount; i++)
            {
                for (int j = 0; j < boolMatrix.CollumnsCount; j++)
                {
                    if (boolMatrix[i, j]) stringBoolMatrix.Append("1" + _columnsSeparator);
                    else stringBoolMatrix.Append("0" + _columnsSeparator);
                }

                stringBoolMatrix.Remove(stringBoolMatrix.Length - 1, 1);
                stringBoolMatrix.Append(_rowsSeparator);
            }

            stringBoolMatrix.Remove(stringBoolMatrix.Length - 1, 1);

            return stringBoolMatrix.ToString();
        }
    }
}
