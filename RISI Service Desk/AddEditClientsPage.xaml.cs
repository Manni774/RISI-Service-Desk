using System;
using System.Data;
using System.Data.Entity;
using System.Windows;
using System.Windows.Controls;

namespace RISI_Service_Desk
{
    public partial class AddEditClientsPage : Page
    {
        private Client _currentClient;
        private bool _isEditMode;

        public AddEditClientsPage(Client selectedClient)
        {
            InitializeComponent();

            _isEditMode = (selectedClient != null);
            _currentClient = selectedClient ?? new Client();

            DataContext = _currentClient;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите наименование компании клиента.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtContact.Text))
            {
                MessageBox.Show("Введите контактное лицо.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtContact.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Введите номер телефона.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPhone.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Введите эл. почту.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmail.Focus();
                return;
            }

            // Обновляем объект
            _currentClient.Name = txtName.Text.Trim();
            _currentClient.ContactPerson = txtContact.Text.Trim();
            _currentClient.Phone = txtPhone.Text.Trim();
            _currentClient.Email = txtEmail.Text.Trim();

            // Сохраняем в новом контексте
            using (var context = new RISI_ServiceDeskEntities1())
            {
                try
                {
                    if (!_isEditMode)
                        context.Clients.Add(_currentClient);
                    else
                    {
                        context.Clients.Attach(_currentClient);
                        context.Entry(_currentClient).State = EntityState.Modified;
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