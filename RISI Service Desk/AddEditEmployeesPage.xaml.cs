using System;
using System.Data;
using System.Data.Entity;
using System.Windows;
using System.Windows.Controls;

namespace RISI_Service_Desk
{
    public partial class AddEditEmployeesPage : Page
    {
        private Employee _currentEmployee;
        private bool _isEditMode;

        public AddEditEmployeesPage(Employee selectedEmployee)
        {
            InitializeComponent();

            _isEditMode = (selectedEmployee != null);
            _currentEmployee = selectedEmployee ?? new Employee();

            DataContext = _currentEmployee;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Введите ФИО сотрудника.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtFullName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPosition.Text))
            {
                MessageBox.Show("Введите должность.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPosition.Focus();
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

            _currentEmployee.FullName = txtFullName.Text.Trim();
            _currentEmployee.Position = txtPosition.Text.Trim();
            _currentEmployee.Phone = txtPhone.Text.Trim();
            _currentEmployee.Email = txtEmail.Text.Trim();

            using (var context = new RISI_ServiceDeskEntities1())
            {
                try
                {
                    if (!_isEditMode)
                        context.Employees.Add(_currentEmployee);
                    else
                    {
                        context.Employees.Attach(_currentEmployee);
                        context.Entry(_currentEmployee).State = EntityState.Modified;
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