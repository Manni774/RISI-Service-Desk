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
using System.Xml.Linq;

namespace RISI_Service_Desk
{
    /// <summary>
    /// Логика взаимодействия для AddEditEmployeesPage.xaml
    /// </summary>
    public partial class AddEditEmployeesPage : Page
    {
        private Employee _currentEmployees = new Employee();
        private bool _isEditMode;

        public AddEditEmployeesPage(Employee selectedEmployees)
        {
            InitializeComponent();
            if (selectedEmployees != null)
                _currentEmployees = selectedEmployees;
            DataContext = _currentEmployees;
            if (!_isEditMode) Title = "Добавление услуги";
            else Title = "Редактирование услуги";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            // Валидация
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Введите наименование компании клиента.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPosition.Text))
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

            _currentEmployees.FullName = txtFullName.Text.Trim();
            _currentEmployees.Position = txtPosition.Text;
            _currentEmployees.Phone = txtPhone.Text;
            _currentEmployees.Email = txtEmail.Text;

            using (var context = new RISI_ServiceDeskEntities1())
            {
                try
                {
                    if (!_isEditMode)
                        context.Employees.Add(_currentEmployees);
                    else
                    {
                        context.Employees.Attach(_currentEmployees);
                        context.Entry(_currentEmployees).State = EntityState.Modified;
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
