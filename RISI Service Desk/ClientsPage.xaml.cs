using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RISI_Service_Desk
{
    public partial class ClientsPage : Page
    {
        public ClientsPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (var context = new RISI_ServiceDeskEntities1())
            {
                var clients = context.Clients.ToList();
                DGridRISIClients.ItemsSource = clients;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddEditClientsPage(null));
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selected = DGridRISIClients.SelectedItem as Client;
            if (selected == null)
            {
                MessageBox.Show("Выберите клиента для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            NavigationService?.Navigate(new AddEditClientsPage(selected));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selected = DGridRISIClients.SelectedItem as Client;
            if (selected == null)
            {
                MessageBox.Show("Выберите клиента для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Удалить клиента \"{selected.Name}\"?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (var context = new RISI_ServiceDeskEntities1())
                {
                    var client = context.Clients.Find(selected.Id);
                    if (client != null)
                    {
                        context.Clients.Remove(client);
                        context.SaveChanges();
                    }
                }
                LoadData();
                MessageBox.Show("Клиент удалён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
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