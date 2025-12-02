﻿using BoolMatrixFramework;
using System;

namespace Prct5Prog.Models
{
    public sealed class MatrixDisplayItem : IEquatable<MatrixDisplayItem>
    {
        private readonly int _id;
        private readonly BoolMatrix _matrix;

        public int Id => _id;
        public int Rows => _matrix?.RowsCount ?? 0;
        public int Columns => _matrix?.ColumnsCount ?? 0; 
        public BoolMatrix Matrix => _matrix;

        public string MatrixString => FormatMatrix("| ");

        public MatrixDisplayItem(int id, BoolMatrix matrix)
        {
            if (id < Constants.MIN_MATRIX_ID)
                throw new ArgumentException($"ID must be >= {Constants.MIN_MATRIX_ID}", nameof(id));

            _id = id;
            _matrix = matrix ?? throw new ArgumentNullException(nameof(matrix));
        }

        public string GetFormattedMatrix(bool withNewLines = true)
        {
            return FormatMatrix(withNewLines ? Environment.NewLine : "| ");
        }

        private string FormatMatrix(string rowSeparator)
        {
            if (_matrix == null) return string.Empty;

            var result = new System.Text.StringBuilder();
            for (int i = 0; i < _matrix.RowsCount; i++)
            {
                for (int j = 0; j < _matrix.ColumnsCount; j++) 
                {
                    result.Append(_matrix[i, j] ? "1 " : "0 ");
                }

                if (i < _matrix.RowsCount - 1)
                    result.Append(rowSeparator);
            }
            return result.ToString().Trim();
        }

        public override bool Equals(object obj) => Equals(obj as MatrixDisplayItem);

        public bool Equals(MatrixDisplayItem other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return _id == other._id && _matrix == other._matrix;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_id * Constants.HASHCODE_PRIME) ^ (_matrix?.GetHashCode() ?? 0);
            }
        }

        public static bool operator ==(MatrixDisplayItem left, MatrixDisplayItem right) =>
            Equals(left, right);

        public static bool operator !=(MatrixDisplayItem left, MatrixDisplayItem right) =>
            !Equals(left, right);

        public override string ToString() =>
            $"Matrix ID: {Id}, Size: {Rows}x{Columns}";
    }
}
