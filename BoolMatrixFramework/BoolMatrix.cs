using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoolMatrixFramework
{
    public class BoolMatrix
    {
        private bool[,] _matrix;

        private void IsValidIndex(int row, int column)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(row, _rowsCount - 1);
            ArgumentOutOfRangeException.ThrowIfNegative(row);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(column, _columnsCount - 1);
            ArgumentOutOfRangeException.ThrowIfNegative(column);
        }

        public bool this[int row, int column]
        {
            get { 
                IsValidIndex(row, column);

                return _matrix[row, column]; 
            }
            set { 
                IsValidIndex(row, column);

                _matrix[row, column] = value; 
            }
        }

        private readonly int _rowsCount;
        public int RowsCount
        {
            get { return _rowsCount; }
        }

        private readonly int _columnsCount;

        public int ColumnsCount
        {
            get { return _columnsCount; }
        }

        public BoolMatrix(int rowsCount, int columnsCount)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rowsCount);
            _rowsCount = rowsCount;

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(columnsCount);
            _columnsCount = columnsCount;

            _matrix = new bool[rowsCount, columnsCount];
        }
    }
}
