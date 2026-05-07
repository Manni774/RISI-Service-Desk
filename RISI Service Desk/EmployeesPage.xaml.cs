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
    /// Логика взаимодействия для EmployeesPage.xaml
    /// </summary>
    public partial class EmployeesPage : Page
    {
        public EmployeesPage()
        {
            InitializeComponent();
            DGridRISIEmployees.ItemsSource = RISI_ServiceDeskEntities1.GetContext().Employees.ToList();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditEmployeesPage((sender as Button).DataContext as Employee));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var employeeRemove = DGridRISIEmployees.SelectedItems.Cast<Employee>().ToList();

            if (employeeRemove.Any())
            {
                if (MessageBox.Show($"Вы точно хотите удалить следующие {employeeRemove.Count()} элементов?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var context = RISI_ServiceDeskEntities1.GetContext();
                        foreach (var item in employeeRemove)
                        {
                            context.Employees.Remove(item);
                        }
                        context.SaveChanges();
                        MessageBox.Show("Данные удалены.");
                        DGridRISIEmployees.ItemsSource = new RISI_ServiceDeskEntities1().Employees.ToList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditEmployeesPage(null));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var context = RISI_ServiceDeskEntities1.GetContext();
            foreach (var entry in context.ChangeTracker.Entries().ToList())
            {
                if (entry.State != EntityState.Added)
                {
                    entry.Reload();
                }
            }
            DGridRISIEmployees.ItemsSource = context.Employees.ToList();
        }
    }
}
