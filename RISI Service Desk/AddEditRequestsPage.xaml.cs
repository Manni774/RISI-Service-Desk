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
    /// Логика взаимодействия для AddEditRequestsPage.xaml
    /// </summary>
    public partial class AddEditRequestsPage : Page
    {
        private Request _currentRequests = new Request();
        private bool _isEditMode;

        public AddEditRequestsPage(Request selectedRequests)
        {
            InitializeComponent();
            if (selectedRequests != null)
                _currentRequests = selectedRequests;
            DataContext = _currentRequests;
            CmbClient.ItemsSource = RISI_ServiceDeskEntities1.GetContext().Clients.ToList();
            CmbService.ItemsSource = RISI_ServiceDeskEntities1.GetContext().Services.ToList();
            CmbEmployee.ItemsSource = RISI_ServiceDeskEntities1.GetContext().Employees.ToList();
            if (!_isEditMode) Title = "Добавление услуги";
            else Title = "Редактирование услуги";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (_currentRequests.ClientId == 0)
                errors.AppendLine("Укажите земельный участок.");
            if (_currentRequests.ServiceId == 0)
                errors.AppendLine("Укажите земельный участок.");
            if (_currentRequests.EmployeeId == 0)
                errors.AppendLine("Укажите земельный участок.");

            _currentRequests.Priority = txtPriority.Text;
            _currentRequests.Description = txtDescription.Text;
            _currentRequests.Status = txtStatus.Text;

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            using (var context = new RISI_ServiceDeskEntities1())
            {
                try
                {
                    if (!_isEditMode)
                        context.Requests.Add(_currentRequests);
                    else
                    {
                        context.Requests.Attach(_currentRequests);
                        context.Entry(_currentRequests).State = EntityState.Modified;
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
