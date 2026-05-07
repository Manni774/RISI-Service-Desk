using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RISI_Service_Desk
{
    /// <summary>
    /// Логика взаимодействия для AddEditClientsPage.xaml
    /// </summary>
    public partial class AddEditClientsPage : Page
    {
        private Client _currentClients = new Client();
        private bool _isEditMode;

        public AddEditClientsPage(Client selectedClients)
        {
            InitializeComponent();
            if (selectedClients != null)
                _currentClients = selectedClients;
            DataContext = _currentClients;
            if (!_isEditMode) Title = "Добавление услуги";
            else Title = "Редактирование услуги";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            // Валидация
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите наименование компании клиента.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtContact.Text))
            {
                MessageBox.Show("Введите контактное лицо.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Введите номер телефона.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Введите эл. почту.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _currentClients.Name = txtName.Text.Trim();
            _currentClients.ContactPerson = txtContact.Text;
            _currentClients.Phone = txtPhone.Text;
            _currentClients.Email = txtEmail.Text;

            using (var context = new RISI_ServiceDeskEntities1())
            {
                try
                {
                    if (!_isEditMode)
                        context.Clients.Add(_currentClients);
                    else
                    {
                        context.Clients.Attach(_currentClients);
                        context.Entry(_currentClients).State = EntityState.Modified;
                    }
                    context.SaveChanges();
                    MessageBox.Show("Сохранено успешно.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Возврат на предыдущую страницу
                    if (Manager.MainFrame.CanGoBack)
                        Manager.MainFrame.GoBack();
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
            if (Manager.MainFrame.CanGoBack)
                Manager.MainFrame.GoBack();
        }
    }
}
