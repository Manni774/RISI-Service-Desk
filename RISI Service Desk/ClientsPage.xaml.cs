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
    /// Логика взаимодействия для ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {
        public ClientsPage()
        {
            InitializeComponent();
            DGridRISIClients.ItemsSource = RISI_ServiceDeskEntities1.GetContext().Clients.ToList();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditClientsPage((sender as Button).DataContext as Client));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var clientRemove = DGridRISIClients.SelectedItems.Cast<Client>().ToList();

            if (clientRemove.Any())
            {
                if (MessageBox.Show($"Вы точно хотите удалить следующие {clientRemove.Count()} элементов?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var context = RISI_ServiceDeskEntities1.GetContext();
                        foreach (var item in clientRemove)
                        {
                            context.Clients.Remove(item);
                        }
                        context.SaveChanges();
                        MessageBox.Show("Данные удалены.");
                        DGridRISIClients.ItemsSource = new RISI_ServiceDeskEntities1().Clients.ToList();
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
            Manager.MainFrame.Navigate(new AddEditClientsPage(null));
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
            DGridRISIClients.ItemsSource = context.Clients.ToList();
        }
    }
}
