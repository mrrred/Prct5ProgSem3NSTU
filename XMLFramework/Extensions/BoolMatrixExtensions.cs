using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoolMatrixFramework;

namespace XMLFramework.Extensions
{
    static class BoolMatrixExtensions
    {
        public static string ConvertToString(this BoolMatrix boolMatrix)
        {
            StringBuilder stringBoolMatrix = new StringBuilder();

            for (int i = 0; i < boolMatrix.RowsCount; i++)
            {
                for (int j = 0; j < boolMatrix.CollumnsCount; j++)
                {
                    if (boolMatrix[i, j]) stringBoolMatrix.Append("1,");
                    else stringBoolMatrix.Append("0,");
                }

                stringBoolMatrix.Remove(stringBoolMatrix.Length - 1, 1);
                stringBoolMatrix.Append(';');
            }

            stringBoolMatrix.Remove(stringBoolMatrix.Length - 1, 1);

            return stringBoolMatrix.ToString();
        }
    }
}
