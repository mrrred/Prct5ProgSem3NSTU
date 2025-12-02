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

        public BoolMatrix(bool[,] matrix)
        {
            _matrix = matrix;
            _rowsCount = matrix.GetLength(0);
            _columnsCount = matrix.GetLength(1);
        }

        public BoolMatrix(int rowsCount, int columnsCount, bool[,] matrix) : this(rowsCount, columnsCount)
        {
            _matrix = matrix;
        }

        public BoolMatrix(int[,] matrix)
        {
            _matrix = IntToBoolMatrix(matrix);
            _rowsCount = matrix.GetLength(0);
            _columnsCount = matrix.GetLength(1);
        }

        public BoolMatrix(int rowsCount, int columnsCount, int[,] matrix) : this(rowsCount, columnsCount)
        {
            _matrix = IntToBoolMatrix(matrix);
        }

        private bool[,] IntToBoolMatrix(int[,] matrix)
        {
            bool[,] boolMatrix = new bool[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    boolMatrix[i, j] = matrix[i, j] != 0;
                }
            }
            return boolMatrix;
        }

        public override string? ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < _rowsCount; i++)
            {
                for (int j = 0; j < _columnsCount; j++)
                {
                    sb.Append(_matrix[i, j] ? "1 " : "0 ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
