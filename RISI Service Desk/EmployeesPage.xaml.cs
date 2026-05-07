using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RISI_Service_Desk
{
    public partial class EmployeesPage : Page
    {
        public EmployeesPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (var context = new RISI_ServiceDeskEntities1())
            {
                var employees = context.Employees.ToList();
                DGridRISIEmployees.ItemsSource = employees;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddEditEmployeesPage(null));
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selected = (sender as Button)?.DataContext as Employee;
            if (selected == null)
            {
                MessageBox.Show("Выберите сотрудника для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            NavigationService?.Navigate(new AddEditEmployeesPage(selected));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selected = DGridRISIEmployees.SelectedItem as Employee;
            if (selected == null)
            {
                MessageBox.Show("Выберите сотрудника для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Удалить сотрудника \"{selected.FullName}\"?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (var context = new RISI_ServiceDeskEntities1())
                {
                    var employee = context.Employees.Find(selected.Id);
                    if (employee != null)
                    {
                        context.Employees.Remove(employee);
                        context.SaveChanges();
                    }
                }
                LoadData();
                MessageBox.Show("Сотрудник удалён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
                LoadData();
        }
    }
}