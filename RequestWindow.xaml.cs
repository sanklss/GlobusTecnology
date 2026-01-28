using GlobusT.Data;
using GlobusT.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;

namespace GlobusT
{
    public partial class RequestWindow : Window
    {
        private Request _request;
        private bool _isEditMode = false;
        private int _originalLicenseCount = 0;

        public RequestWindow(Request request)
        {
            InitializeComponent();
            _request = request;
            LoadData();
            LoadRequest();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateAvailableSlots();
        }

        private void LoadData()
        {
            try
            {
                using (var context = new GlobusTechnologyContext())
                {
                    var services = context.Services
                        .Include(s => s.IdTypeCommandNavigation)
                        .OrderBy(s => s.Name)
                        .ToList();
                    ServiceComboBox.ItemsSource = services;

                    var users = context.Users
                        .OrderBy(u => u.Login)
                        .ToList();
                    UserComboBox.ItemsSource = users;

                    var statuses = context.RequestStatuses
                        .OrderBy(s => s.Id)
                        .ToList();
                    StatusComboBox.ItemsSource = statuses;

                    if (_request.Id > 0)
                    {
                        if (_request.IdService > 0)
                        {
                            ServiceComboBox.SelectedItem = services.FirstOrDefault(s => s.Id == _request.IdService);
                        }
                        if (_request.IdUser > 0)
                        {
                            UserComboBox.SelectedItem = users.FirstOrDefault(u => u.Id == _request.IdUser);
                        }
                        if (_request.IdStatusRequest > 0)
                        {
                            StatusComboBox.SelectedItem = statuses.FirstOrDefault(s => s.Id == _request.IdStatusRequest);
                        }
                    }
                    else
                    {
                        StatusComboBox.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка");
                Close();
            }
        }

        private void LoadRequest()
        {
            if (_request.Id > 0)
            {
                _isEditMode = true;
                _originalLicenseCount = _request.CountNeedSpecialists;

                RequestIdText.Text = _request.Id.ToString();
                DatePicker.SelectedDate = _request.Date;
                LicenseQuantityTextBox.Text = _request.CountNeedSpecialists.ToString();
                CommentTextBox.Text = _request.Comment ?? "";

                DeleteButton.Visibility = Visibility.Visible;
            }
            else
            {
                RequestIdText.Text = "Новая заявка";
                DatePicker.SelectedDate = DateTime.Today;
                LicenseQuantityTextBox.Text = "1";
            }
        }

        private void UpdateAvailableSlots()
        {
            if (ServiceComboBox.SelectedItem is Service selectedService)
            {
                AvailableLicensesText.Text = $"Доступно: {selectedService.FreeSlot}";

                if (selectedService.FreeSlot < 10)
                {
                    AvailableLicensesText.Foreground = System.Windows.Media.Brushes.Red;
                    AvailableLicensesText.FontWeight = FontWeights.Bold;
                }
                else
                {
                    AvailableLicensesText.Foreground = System.Windows.Media.Brushes.Green;
                    AvailableLicensesText.FontWeight = FontWeights.Normal;
                }
            }
        }

        private void ServiceComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            UpdateAvailableSlots();
        }

        private void LicenseQuantityTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UpdateAvailableSlots();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ServiceComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите услугу!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (UserComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(LicenseQuantityTextBox.Text, out int licenseCount) || licenseCount <= 0)
            {
                MessageBox.Show("Введите корректное количество специалистов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (StatusComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите статус!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var selectedService = ServiceComboBox.SelectedItem as Service;
            var selectedUser = UserComboBox.SelectedItem as User;
            var selectedStatus = StatusComboBox.SelectedItem as RequestStatus;

            try
            {
                using (var context = new GlobusTechnologyContext())
                {
                    var serviceFromDb = context.Services.Find(selectedService.Id);

                    if (licenseCount > serviceFromDb.FreeSlot)
                    {
                        MessageBox.Show($"Недостаточно свободных слотов!\n" +
                                      $"Доступно: {serviceFromDb.FreeSlot}\n" +
                                      $"Требуется: {licenseCount}",
                                      "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    Request requestToSave;

                    if (_isEditMode)
                    {
                        requestToSave = context.Requests.Find(_request.Id);
                        if (requestToSave == null)
                        {
                            MessageBox.Show("Заявка не найдена!", "Ошибка");
                            return;
                        }

                        if (requestToSave.IdService == selectedService.Id)
                        {
                            serviceFromDb.FreeSlot += _originalLicenseCount;
                        }
                        else
                        {
                            var oldService = context.Services.Find(requestToSave.IdService);
                            if (oldService != null)
                            {
                                oldService.FreeSlot += _originalLicenseCount;
                            }
                        }
                    }
                    else
                    {
                        requestToSave = new Request();
                        context.Requests.Add(requestToSave);
                    }

                    requestToSave.IdService = selectedService.Id;
                    requestToSave.IdUser = selectedUser.Id;
                    requestToSave.IdStatusRequest = selectedStatus.Id;
                    requestToSave.Date = DatePicker.SelectedDate ?? DateTime.Today;
                    requestToSave.CountNeedSpecialists = licenseCount;
                    requestToSave.Comment = CommentTextBox.Text;

                    serviceFromDb.FreeSlot -= licenseCount;

                    context.SaveChanges();

                    if (!_isEditMode)
                    {
                        _request.Id = requestToSave.Id;
                    }

                    MessageBox.Show("Заявка успешно сохранена!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    DialogResult = true;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить заявку?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var context = new GlobusTechnologyContext())
                    {
                        var requestToDelete = context.Requests.Find(_request.Id);
                        if (requestToDelete != null)
                        {
                            var service = context.Services.Find(requestToDelete.IdService);
                            if (service != null)
                            {
                                service.FreeSlot += requestToDelete.CountNeedSpecialists;
                            }

                            context.Requests.Remove(requestToDelete);
                            context.SaveChanges();
                        }
                    }

                    MessageBox.Show("Заявка удалена!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    DialogResult = true;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка");
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}