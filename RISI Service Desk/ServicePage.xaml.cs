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
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {

        public ServicePage()
        {
            InitializeComponent();
            DGridRISI.ItemsSource = RISI_ServiceDeskEntities1.GetContext().Services.ToList();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditServicePage((sender as Button).DataContext as Service));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var agreementRemove = DGridRISI.SelectedItems.Cast<Service>().ToList();

            if (agreementRemove.Any())
            {
                if (MessageBox.Show($"Вы точно хотите удалить следующие {agreementRemove.Count()} элементов?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var context = RISI_ServiceDeskEntities1.GetContext();
                        foreach (var item in agreementRemove)
                        {
                            context.Services.Remove(item);
                        }
                        context.SaveChanges();
                        MessageBox.Show("Данные удалены.");
                        DGridRISI.ItemsSource = new RISI_ServiceDeskEntities1().Services.ToList();
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
            Manager.MainFrame.Navigate(new AddEditServicePage(null));
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
            DGridRISI.ItemsSource = context.Services.ToList();
        }
    }
}
