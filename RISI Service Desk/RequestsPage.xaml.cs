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
    /// Логика взаимодействия для RequestsPage.xaml
    /// </summary>
    public partial class RequestsPage : Page
    {
        public RequestsPage()
        {
            InitializeComponent();
            DGridRISIRequests.ItemsSource = RISI_ServiceDeskEntities1.GetContext().Requests.ToList();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditRequestsPage((sender as Button).DataContext as Request));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var requestRemove = DGridRISIRequests.SelectedItems.Cast<Request>().ToList();

            if (requestRemove.Any())
            {
                if (MessageBox.Show($"Вы точно хотите удалить следующие {requestRemove.Count()} элементов?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var context = RISI_ServiceDeskEntities1.GetContext();
                        foreach (var item in requestRemove)
                        {
                            context.Requests.Remove(item);
                        }
                        context.SaveChanges();
                        MessageBox.Show("Данные удалены.");
                        DGridRISIRequests.ItemsSource = new RISI_ServiceDeskEntities1().Requests.ToList();
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
            Manager.MainFrame.Navigate(new AddEditRequestsPage(null));
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
            DGridRISIRequests.ItemsSource = context.Requests.ToList();
        }
    }
}
