using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BoolMatrixFramework;
using Prct5Prog.Models;
using Prct5Prog.Services;

namespace Prct5Prog
{
    public partial class MainWindow : Window
    {
        private readonly MatrixService _matrixService;
        private readonly List<MatrixDisplayItem> _matrices;

        public MainWindow()
        {
            InitializeComponent();

            _matrices = new List<MatrixDisplayItem>();
            _matrixService = new MatrixService("matrices.xml");

            InitializeApplication();
        }

        private void InitializeApplication()
        {
            try
            {
                RefreshMatrixList();
                UpdateStatus("Application initialized successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing application: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshMatrixList()
        {
            try
            {
                _matrices.Clear();
                var allMatrices = _matrixService.GetAll();

                foreach (var matrixEntry in allMatrices)
                {
                    var matrix = matrixEntry.Value;

                    if (!int.TryParse(matrixEntry.Key, out int id) || id <= 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"Skipping matrix with invalid ID: {matrixEntry.Key}");
                        continue;
                    }

                    try
                    {
                        var displayItem = new MatrixDisplayItem(id, matrix);
                        _matrices.Add(displayItem);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error creating MatrixDisplayItem: {ex.Message}");
                    }
                }

                MatrixListView.ItemsSource = null;
                MatrixListView.ItemsSource = _matrices
                    .OrderBy(m => m.Id)
                    .ToList();

                UpdateCount();
                UpdateStatus($"Loaded {_matrices.Count} matrices");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing list: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateCount()
        {
            CountTextBlock.Text = $"Total: {_matrices.Count} matrices";
        }

        private void UpdateStatus(string message)
        {
            StatusTextBlock.Text = $"{DateTime.Now:HH:mm:ss}: {message}";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new MatrixDialog();

                if (dialog.ShowDialog() == true)
                {
                    var newMatrix = dialog.GetMatrix();
                    _matrixService.Add(newMatrix);
                    RefreshMatrixList();
                    UpdateStatus("Matrix added successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding matrix: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = MatrixListView.SelectedItem as MatrixDisplayItem;

            if (selectedItem == null)
            {
                MessageBox.Show("Please select a matrix to edit.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var dialog = new MatrixDialog(selectedItem.Matrix);

                if (dialog.ShowDialog() == true)
                {
                    var updatedMatrix = dialog.GetMatrix();
                    _matrixService.Update(selectedItem.Id, updatedMatrix);
                    RefreshMatrixList();
                    UpdateStatus($"Matrix ID {selectedItem.Id} updated successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing matrix: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = MatrixListView.SelectedItem as MatrixDisplayItem;

            if (selectedItem == null)
            {
                MessageBox.Show("Please select a matrix to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete matrix ID {selectedItem.Id}?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _matrixService.Delete(selectedItem.Id);
                    RefreshMatrixList();
                    UpdateStatus($"Matrix ID {selectedItem.Id} deleted successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting matrix: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshMatrixList();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var searchText = SearchTextBox.Text?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(searchText) || searchText == "Enter search terms...")
                {
                    RefreshMatrixList();
                    return;
                }

                searchText = searchText.ToLower();

                var searchResults = _matrices
                    .Where(m =>
                        m.Id.ToString().Contains(searchText) ||
                        m.Rows.ToString().Contains(searchText) ||
                        m.Columns.ToString().Contains(searchText) ||
                        m.MatrixString.ToLower().Contains(searchText) ||
                        m.MatrixString.Replace(" ", "").Contains(searchText))
                    .ToList();

                MatrixListView.ItemsSource = searchResults;
                UpdateStatus($"Found {searchResults.Count} matrices matching '{searchText}'");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = "Enter search terms...";
            RefreshMatrixList();
        }

        private void MatrixListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = MatrixListView.SelectedItem as MatrixDisplayItem;

            if (selectedItem != null)
            {
                PreviewTextBlock.Text = selectedItem.GetFormattedMatrix();

                int trueCount = 0;
                var matrix = selectedItem.Matrix;
                for (int i = 0; i < matrix.RowsCount; i++)
                    for (int j = 0; j < matrix.ColumnsCount; j++)
                        if (matrix[i, j]) trueCount++;

                int totalElements = selectedItem.Rows * selectedItem.Columns;
                double truePercentage = totalElements > 0 ? (trueCount * 100.0 / totalElements) : 0;

                DetailsTextBlock.Text =
                    $"ID: {selectedItem.Id}\n" +
                    $"Size: {selectedItem.Rows} × {selectedItem.Columns}\n" +
                    $"Total Elements: {totalElements}\n" +
                    $"True Values: {trueCount} ({truePercentage:F1}%)\n" +
                    $"False Values: {totalElements - trueCount}";
            }
            else
            {
                PreviewTextBlock.Text = string.Empty;
                DetailsTextBlock.Text = "Select a matrix to view details";
            }
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text == "Enter search terms...")
            {
                SearchTextBox.Text = string.Empty;
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SearchTextBox.Text = "Enter search terms...";
            }
        }
    }
}
