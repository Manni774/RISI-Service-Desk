using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RISI_Service_Desk
{
    public partial class ServicePage : Page
    {
        public ServicePage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (var context = new RISI_ServiceDeskEntities1())
            {
                var services = context.Services.ToList();
                DGridRISIService.ItemsSource = services;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddEditServicePage(null));
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selected = (sender as Button)?.DataContext as Service;
            if (selected == null)
            {
                MessageBox.Show("Выберите услугу для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            NavigationService?.Navigate(new AddEditServicePage(selected));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selected = DGridRISIService.SelectedItem as Service;
            if (selected == null)
            {
                MessageBox.Show("Выберите услугу для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Удалить услугу \"{selected.ServiceName}\"?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (var context = new RISI_ServiceDeskEntities1())
                {
                    var service = context.Services.Find(selected.Id);
                    if (service != null)
                    {
                        context.Services.Remove(service);
                        context.SaveChanges();
                    }
                }
                LoadData();
                MessageBox.Show("Услуга удалена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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