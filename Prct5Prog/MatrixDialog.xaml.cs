using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BoolMatrixFramework;

namespace Prct5Prog
{
    public partial class MatrixDialog : Window
    {
        public class MatrixCell
        {
            public bool Value { get; set; }
        }

        private BoolMatrix _originalMatrix;
        private List<MatrixCell> _matrixCells;

        public MatrixDialog()
        {
            InitializeComponent();
            InitializeMatrix(3, 3);
        }

        public MatrixDialog(BoolMatrix existingMatrix)
        {
            InitializeComponent();
            _originalMatrix = existingMatrix;
            InitializeMatrix(existingMatrix.RowsCount, existingMatrix.CollumnsCount);
            LoadExistingMatrix(existingMatrix);
        }

        private void InitializeMatrix(int rows, int columns)
        {
            RowsTextBox.Text = rows.ToString();
            ColumnsTextBox.Text = columns.ToString();

            _matrixCells = new List<MatrixCell>();
            for (int i = 0; i < rows * columns; i++)
            {
                _matrixCells.Add(new MatrixCell { Value = false });
            }

            MatrixItemsControl.ItemsSource = _matrixCells;
        }

        private void LoadExistingMatrix(BoolMatrix matrix)
        {
            for (int i = 0; i < matrix.RowsCount; i++)
            {
                for (int j = 0; j < matrix.CollumnsCount; j++)
                {
                    int index = i * matrix.CollumnsCount + j;
                    if (index < _matrixCells.Count)
                    {
                        _matrixCells[index].Value = matrix[i, j];
                    }
                }
            }
            MatrixItemsControl.Items.Refresh();
        }

        public BoolMatrix GetMatrix()
        {
            int rows = int.Parse(RowsTextBox.Text);
            int columns = int.Parse(ColumnsTextBox.Text);

            var matrix = new BoolMatrix(rows, columns);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int index = i * columns + j;
                    if (index < _matrixCells.Count)
                    {
                        matrix[i, j] = _matrixCells[index].Value;
                    }
                }
            }
            return matrix;
        }

        private void ResizeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int rows = int.Parse(RowsTextBox.Text);
                int columns = int.Parse(ColumnsTextBox.Text);
                InitializeMatrix(rows, columns);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Invalid size: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int rows = int.Parse(RowsTextBox.Text);
                int columns = int.Parse(ColumnsTextBox.Text);

                if (rows <= 0 || columns <= 0)
                {
                    MessageBox.Show("Rows and columns must be positive numbers", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void SizeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void CheckBox_Changed(object sender, RoutedEventArgs e)
        {
            UpdateQuickInput();
        }

        private void QuickInputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (QuickInputTextBox.IsFocused)
            {
                ParseQuickInput();
            }
        }

        private void UpdateQuickInput()
        {
            int rows = int.Parse(RowsTextBox.Text);
            int columns = int.Parse(ColumnsTextBox.Text);

            var input = "";
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int index = i * columns + j;
                    if (index < _matrixCells.Count)
                    {
                        input += _matrixCells[index].Value ? "1 " : "0 ";
                    }
                }
                input += "\n";
            }

            QuickInputTextBox.TextChanged -= QuickInputTextBox_TextChanged;
            QuickInputTextBox.Text = input.Trim();
            QuickInputTextBox.TextChanged += QuickInputTextBox_TextChanged;
        }

        private void ParseQuickInput()
        {
            try
            {
                var input = QuickInputTextBox.Text;
                var rows = input.Split('\n');
                var values = new List<bool>();

                foreach (var row in rows)
                {
                    var cells = row.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var cell in cells)
                    {
                        values.Add(cell == "1");
                    }
                }

                int expectedCount = int.Parse(RowsTextBox.Text) * int.Parse(ColumnsTextBox.Text);
                if (values.Count == expectedCount)
                {
                    for (int i = 0; i < values.Count && i < _matrixCells.Count; i++)
                    {
                        _matrixCells[i].Value = values[i];
                    }
                    MatrixItemsControl.Items.Refresh();
                }
            }
            catch
            {
            }
        }
    }
}