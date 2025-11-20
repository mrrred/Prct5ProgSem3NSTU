using BoolMatrixFramework;

namespace Prct5Prog.Models
{
    public class MatrixDisplayItem
    {
        public int Id { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public string MatrixString { get; set; }
        public BoolMatrix Matrix { get; set; }
    }
}