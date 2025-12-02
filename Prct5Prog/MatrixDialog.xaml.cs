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
        private sealed class MatrixCell
        {
            public bool Value { get; set; }
        }

        private BoolMatrix _matrix;
        private List<MatrixCell> _matrixCells;
        private bool _isInitialized = false;

        public MatrixDialog() : this(new BoolMatrix(Constants.DEFAULT_MATRIX_ROWS, Constants.DEFAULT_MATRIX_COLUMNS))
        {
        }

        public MatrixDialog(BoolMatrix existingMatrix)
        {
            if (existingMatrix == null)
                throw new ArgumentNullException(nameof(existingMatrix));

            InitializeComponent();
            _isInitialized = true;
            _matrix = existingMatrix;

            InitializeUI();
        }

        public BoolMatrix GetMatrix()
        {
            int rows = GetValidatedRows();
            int columns = GetValidatedColumns();

            var resultMatrix = new BoolMatrix(rows, columns);

            for (int i = 0; i < Math.Min(rows, _matrix.RowsCount); i++)
            {
                for (int j = 0; j < Math.Min(columns, _matrix.ColumnsCount); j++)
                {
                    int index = i * columns + j;
                    if (index < _matrixCells.Count)
                    {
                        resultMatrix[i, j] = _matrixCells[index].Value;
                    }
                }
            }

            return resultMatrix;
        }

        private void InitializeUI()
        {
            RowsTextBox.Text = _matrix.RowsCount.ToString();
            ColumnsTextBox.Text = _matrix.ColumnsCount.ToString();

            LoadMatrixToUI();
            UpdateQuickInput();
        }

        private void LoadMatrixToUI()
        {
            int rows = _matrix.RowsCount;
            int columns = _matrix.ColumnsCount;

            _matrixCells = new List<MatrixCell>(rows * columns);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    _matrixCells.Add(new MatrixCell { Value = _matrix[i, j] });
                }
            }

            MatrixItemsControl.ItemsSource = _matrixCells;

            var uniformGrid = new System.Windows.Controls.Primitives.UniformGrid();
            uniformGrid.Columns = columns;

            var factory = new System.Windows.FrameworkElementFactory(typeof(System.Windows.Controls.Primitives.UniformGrid));
            factory.SetValue(System.Windows.Controls.Primitives.UniformGrid.ColumnsProperty, columns);

            MatrixItemsControl.ItemsPanel = new ItemsPanelTemplate(factory);
        }

        private void UpdateQuickInput()
        {
            if (!_isInitialized) return;

            int rows = GetValidatedRows();
            int columns = GetValidatedColumns();

            if (rows <= 0 || columns <= 0 || _matrixCells == null) return;

            var input = new System.Text.StringBuilder();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int index = i * columns + j;
                    if (index < _matrixCells.Count)
                    {
                        input.Append(_matrixCells[index].Value ? "1 " : "0 ");
                    }
                }
                if (i < rows - 1) input.AppendLine();
            }

            QuickInputTextBox.TextChanged -= QuickInputTextBox_TextChanged;
            QuickInputTextBox.Text = input.ToString().Trim();
            QuickInputTextBox.TextChanged += QuickInputTextBox_TextChanged;
        }

        private bool TryParseSize(string text, out int value)
        {
            return int.TryParse(text, out value) && value > 0;
        }

        private int GetValidatedRows()
        {
            if (TryParseSize(RowsTextBox.Text, out int rows) && rows <= Constants.MAX_MATRIX_ROWS)
                return rows;
            return Constants.DEFAULT_MATRIX_ROWS;
        }

        private int GetValidatedColumns()
        {
            if (TryParseSize(ColumnsTextBox.Text, out int columns) && columns <= Constants.MAX_MATRIX_COLUMNS)
                return columns;
            return Constants.DEFAULT_MATRIX_COLUMNS;
        }

        private bool ValidateMatrixSize(int rows, int columns, out string errorMessage)
        {
            errorMessage = null;

            if (rows <= 0 || columns <= 0)
            {
                errorMessage = "Rows and columns must be positive numbers.";
                return false;
            }

            if (rows > Constants.MAX_MATRIX_ROWS || columns > Constants.MAX_MATRIX_COLUMNS)
            {
                errorMessage = $"Maximum size is {Constants.MAX_MATRIX_ROWS}x{Constants.MAX_MATRIX_COLUMNS}.";
                return false;
            }

            if (rows * columns > Constants.MAX_MATRIX_ELEMENTS)
            {
                errorMessage = $"Maximum total elements is {Constants.MAX_MATRIX_ELEMENTS}.";
                return false;
            }

            return true;
        }

        private void ResizeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!TryParseSize(RowsTextBox.Text, out int rows) || !TryParseSize(ColumnsTextBox.Text, out int columns))
                {
                    MessageBox.Show("Please enter valid positive numbers for rows and columns.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!ValidateMatrixSize(rows, columns, out string errorMessage))
                {
                    MessageBox.Show(errorMessage, "Invalid Size", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newMatrix = new BoolMatrix(rows, columns);

                for (int i = 0; i < Math.Min(rows, _matrix.RowsCount); i++)
                {
                    for (int j = 0; j < Math.Min(columns, _matrix.ColumnsCount); j++)
                    {
                        newMatrix[i, j] = _matrix[i, j];
                    }
                }

                _matrix = newMatrix;
                LoadMatrixToUI();
                UpdateQuickInput();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error resizing matrix: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!TryParseSize(RowsTextBox.Text, out int rows) || !TryParseSize(ColumnsTextBox.Text, out int columns))
                {
                    MessageBox.Show("Please enter valid positive numbers for rows and columns.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!ValidateMatrixSize(rows, columns, out string errorMessage))
                {
                    MessageBox.Show(errorMessage, "Invalid Size", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void CheckBox_Changed(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is MatrixCell cell && _matrixCells != null)
            {
                int index = _matrixCells.IndexOf(cell);
                if (index >= 0)
                {
                    int rows = GetValidatedRows();
                    int columns = GetValidatedColumns();

                    int row = index / columns;
                    int col = index % columns;

                    if (row < _matrix.RowsCount && col < _matrix.ColumnsCount)
                    {
                        _matrix[row, col] = cell.Value;
                    }

                    UpdateQuickInput();
                }
            }
        }

        private void QuickInputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_isInitialized || !QuickInputTextBox.IsFocused) return;

            try
            {
                var input = QuickInputTextBox.Text;
                if (string.IsNullOrWhiteSpace(input)) return;

                var rows = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                var values = new List<bool>();

                foreach (var row in rows)
                {
                    var cells = row.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var cell in cells)
                    {
                        if (cell == "1")
                            values.Add(true);
                        else if (cell == "0")
                            values.Add(false);
                    }
                }

                int expectedRows = GetValidatedRows();
                int expectedColumns = GetValidatedColumns();
                int expectedCount = expectedRows * expectedColumns;

                if (values.Count == expectedCount && _matrixCells != null)
                {
                    int valueIndex = 0;
                    for (int i = 0; i < expectedRows && valueIndex < values.Count; i++)
                    {
                        for (int j = 0; j < expectedColumns && valueIndex < values.Count; j++)
                        {
                            if (i < _matrix.RowsCount && j < _matrix.ColumnsCount)
                            {
                                _matrix[i, j] = values[valueIndex];
                            }

                            int uiIndex = i * expectedColumns + j;
                            if (uiIndex < _matrixCells.Count)
                            {
                                _matrixCells[uiIndex].Value = values[valueIndex];
                            }

                            valueIndex++;
                        }
                    }

                    MatrixItemsControl.Items.Refresh();
                }
            }
            catch
            {
            }
        }

        private void SizeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}
