using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace RISI_Service_Desk
{
    public partial class AddEditRequestsPage : Page
    {
        private Request _currentRequest;
        private bool _isEditMode;

        public AddEditRequestsPage(Request selectedRequest)
        {
            InitializeComponent();

            _isEditMode = (selectedRequest != null);
            _currentRequest = selectedRequest ?? new Request();

            DataContext = _currentRequest;

            // Загружаем списки для ComboBox через локальный контекст
            using (var context = new RISI_ServiceDeskEntities1())
            {
                CmbClient.ItemsSource = context.Clients.ToList();
                CmbService.ItemsSource = context.Services.ToList();
                CmbEmployee.ItemsSource = context.Employees.ToList();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            // Валидация
            if (_currentRequest.ClientId == 0 || _currentRequest.ClientId == null)
                errors.AppendLine("Выберите клиента.");
            if (_currentRequest.ServiceId == 0 || _currentRequest.ServiceId == null)
                errors.AppendLine("Выберите услугу.");
            // Сотрудник может быть не назначен, поэтому проверяем только если обязательно
            // if (_currentRequest.EmployeeId == 0 || _currentRequest.EmployeeId == null)
            //     errors.AppendLine("Назначьте сотрудника.");

            if (string.IsNullOrWhiteSpace(txtPriority.Text))
                errors.AppendLine("Укажите приоритет.");
            if (string.IsNullOrWhiteSpace(txtStatus.Text))
                errors.AppendLine("Укажите статус.");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Обновляем остальные поля (кроме внешних ключей, которые уже привязаны)
            _currentRequest.Priority = txtPriority.Text.Trim();
            _currentRequest.Description = txtDescription.Text;
            _currentRequest.Status = txtStatus.Text.Trim();

            using (var context = new RISI_ServiceDeskEntities1())
            {
                try
                {
                    if (!_isEditMode)
                        context.Requests.Add(_currentRequest);
                    else
                    {
                        context.Requests.Attach(_currentRequest);
                        context.Entry(_currentRequest).State = EntityState.Modified;
                    }
                    context.SaveChanges();
                    MessageBox.Show("Сохранено успешно.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Возврат на предыдущую страницу
                    if (NavigationService != null && NavigationService.CanGoBack)
                        NavigationService.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}\n{ex.InnerException?.Message}",
                                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}