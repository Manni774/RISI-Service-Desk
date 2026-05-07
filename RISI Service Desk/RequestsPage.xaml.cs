using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RISI_Service_Desk
{
    public partial class RequestsPage : Page
    {
        public RequestsPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (var context = new RISI_ServiceDeskEntities1())
            {
                // Для отображения названий клиента, услуги и сотрудника используем Include
                var requests = context.Requests
                    .Include("Client")
                    .Include("Service")
                    .Include("Employee")
                    .ToList();
                DGridRISIRequests.ItemsSource = requests;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddEditRequestsPage(null));
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selected = (sender as Button)?.DataContext as Request;
            if (selected == null)
            {
                MessageBox.Show("Выберите заявку для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            NavigationService?.Navigate(new AddEditRequestsPage(selected));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selected = DGridRISIRequests.SelectedItem as Request;
            if (selected == null)
            {
                MessageBox.Show("Выберите заявку для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Удалить заявку №{selected.Id}?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (var context = new RISI_ServiceDeskEntities1())
                {
                    var request = context.Requests.Find(selected.Id);
                    if (request != null)
                    {
                        context.Requests.Remove(request);
                        context.SaveChanges();
                    }
                }
                LoadData();
                MessageBox.Show("Заявка удалена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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