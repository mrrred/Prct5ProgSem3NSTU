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
        private MatrixService _matrixService;
        private List<MatrixDisplayItem> _matrices;

        public MainWindow()
        {
            InitializeComponent();
            InitializeApplication();
        }

        private void InitializeApplication()
        {
            try
            {
                _matrixService = new MatrixService("matrices.xml");
                _matrices = new List<MatrixDisplayItem>();
                RefreshMatrixList();
                UpdateStatus("Application initialized successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing application: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
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
                    _matrices.Add(new MatrixDisplayItem
                    {
                        Id = int.Parse(matrixEntry.Key),
                        Rows = matrix.RowsCount,
                        Columns = matrix.CollumnsCount,
                        MatrixString = MatrixToString(matrix),
                        Matrix = matrix
                    });
                }

                MatrixListView.ItemsSource = null;
                MatrixListView.ItemsSource = _matrices.OrderBy(m => m.Id).ToList();
                UpdateCount();
                UpdateStatus("Matrix list refreshed");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing list: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string MatrixToString(BoolMatrix matrix)
        {
            var result = "";
            for (int i = 0; i < matrix.RowsCount; i++)
            {
                for (int j = 0; j < matrix.CollumnsCount; j++)
                {
                    result += matrix[i, j] ? "1 " : "0 ";
                }
                if (i < matrix.RowsCount - 1) result += "| ";
            }
            return result;
        }

        private void UpdateCount()
        {
            CountTextBlock.Text = $"Total: {_matrices.Count} matrices";
        }

        private void UpdateStatus(string message)
        {
            StatusTextBlock.Text = message;
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
                MessageBox.Show($"Error adding matrix: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (MatrixListView.SelectedItem is MatrixDisplayItem selectedItem)
            {
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
                    MessageBox.Show($"Error editing matrix: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a matrix to edit", "Warning",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MatrixListView.SelectedItem is MatrixDisplayItem selectedItem)
            {
                try
                {
                    var result = MessageBox.Show($"Delete matrix ID {selectedItem.Id}?",
                        "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        _matrixService.Delete(selectedItem.Id);
                        RefreshMatrixList();
                        UpdateStatus($"Matrix ID {selectedItem.Id} deleted successfully");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting matrix: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a matrix to delete", "Warning",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
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
                var searchText = SearchTextBox.Text.ToLower();

                if (string.IsNullOrWhiteSpace(searchText) || searchText == "enter search terms...")
                {
                    RefreshMatrixList();
                    return;
                }

                var searchResults = _matrices
                    .Where(m => m.MatrixString.ToLower().Contains(searchText) ||
                               m.Id.ToString().Contains(searchText) ||
                               m.Rows.ToString().Contains(searchText) ||
                               m.Columns.ToString().Contains(searchText))
                    .ToList();

                MatrixListView.ItemsSource = searchResults;
                UpdateStatus($"Found {searchResults.Count} matrices matching '{searchText}'");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = "Enter search terms...";
            RefreshMatrixList();
        }

        private void MatrixListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MatrixListView.SelectedItem is MatrixDisplayItem selectedItem)
            {
                PreviewTextBlock.Text = FormatMatrixForDisplay(selectedItem.Matrix);

                DetailsTextBlock.Text = $"ID: {selectedItem.Id}\n" +
                                      $"Size: {selectedItem.Rows} × {selectedItem.Columns}\n" +
                                      $"Total Elements: {selectedItem.Rows * selectedItem.Columns}\n" +
                                      $"True Values: {CountTrueValues(selectedItem.Matrix)}";
            }
        }

        private string FormatMatrixForDisplay(BoolMatrix matrix)
        {
            var result = "";
            for (int i = 0; i < matrix.RowsCount; i++)
            {
                for (int j = 0; j < matrix.CollumnsCount; j++)
                {
                    result += matrix[i, j] ? "1 " : "0 ";
                }
                result += "\n";
            }
            return result;
        }

        private int CountTrueValues(BoolMatrix matrix)
        {
            int count = 0;
            for (int i = 0; i < matrix.RowsCount; i++)
                for (int j = 0; j < matrix.CollumnsCount; j++)
                    if (matrix[i, j]) count++;
            return count;
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text == "Enter search terms...")
                SearchTextBox.Text = "";
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
                SearchTextBox.Text = "Enter search terms...";
        }
    }
}