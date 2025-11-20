using BoolMatrixFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLFramework.Deserializators.Abstractions;
using XMLFramework.XMLConfigurations.Abstractions;

namespace XMLFramework.Deserializators
{
    public class BoolmatrixDeserializator : IDeserializator<BoolMatrix>
    {
        private readonly string _rowsSeparator;

        private readonly string _columnsSeparator;

        public BoolmatrixDeserializator(string rowsSeparator = ";", string columnsSeparator = ",")
        {
            ArgumentNullException.ThrowIfNullOrEmpty(rowsSeparator);
            ArgumentNullException.ThrowIfNullOrEmpty(columnsSeparator);

            _rowsSeparator = rowsSeparator;
            _columnsSeparator = columnsSeparator;
        }

        public BoolmatrixDeserializator(IXMLBoolMatrixConfiguration config) : this(config.RowsSeparator, config.ColumnsSeparator) { }

        public BoolMatrix Deserialization(string stringBoolMatrix)
        {
            if (string.IsNullOrEmpty(stringBoolMatrix))
                throw new ArgumentException("Input string cannot be null or empty");

            var rowsStrings = stringBoolMatrix.Split(_rowsSeparator, StringSplitOptions.RemoveEmptyEntries);

            int rows = rowsStrings.Length;

            int collumns = rowsStrings[0].Split(_columnsSeparator).Length;

            if (rows == 0 || collumns == 0)
                throw new ArgumentException("Matrix dimensions cannot be zero");

            BoolMatrix resultBoolMatrix = new(rows, collumns);

            for (int i = 0; i < rowsStrings.Length; i++)
            {
                var collumnsArray = rowsStrings[i].Split(_columnsSeparator, StringSplitOptions.RemoveEmptyEntries);

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
    }
}
