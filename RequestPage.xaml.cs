using GlobusT.Data;
using GlobusT.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GlobusT
{
    public partial class RequestPage : Page
    {
        private bool _isAscendingSort = false;

        public RequestPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                LoadStatuses();
                LoadRequests(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка");
            }
        }

        private void LoadRequests()
        {
            try
            {
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заявок: {ex.Message}", "Ошибка");
            }
        }

        private void LoadStatuses()
        {
            try
            {
                using (var context = new GlobusTechnologyContext())
                {
                    var statuses = context.RequestStatuses.ToList();
                    statuses.Insert(0, new RequestStatus { Id = 0, Name = "Все статусы" });
                    StatusFilterComboBox.ItemsSource = statuses;
                    StatusFilterComboBox.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки статусов: {ex.Message}", "Ошибка");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void StatusFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            _isAscendingSort = !_isAscendingSort;
            SortButton.Content = _isAscendingSort ? "По дате ▲" : "По дате ▼";
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            try
            {
                using (var context = new GlobusTechnologyContext())
                {
                    var query = context.Requests
                        .Include(r => r.IdServiceNavigation)
                        .Include(r => r.IdStatusRequestNavigation)
                        .Include(r => r.IdUserNavigation)
                        .AsQueryable();

                    string searchText = SearchTextBox.Text.Trim();
                    if (!string.IsNullOrWhiteSpace(searchText))
                    {
                        query = query.Where(r =>
                            (r.IdUserNavigation != null &&
                             r.IdUserNavigation.FullName.Contains(searchText)) ||
                            r.Id.ToString().Contains(searchText));
                    }

                    var selectedStatus = StatusFilterComboBox.SelectedItem as RequestStatus;
                    if (selectedStatus != null && selectedStatus.Id != 0)
                    {
                        query = query.Where(r => r.IdStatusRequest == selectedStatus.Id);
                    }

                    query = _isAscendingSort
                        ? query.OrderBy(r => r.Date)
                        : query.OrderByDescending(r => r.Date);

                    RequestItemControl.ItemsSource = query.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка фильтрации: {ex.Message}", "Ошибка");
            }
        }
    }
}