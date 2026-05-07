using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new ServicePage());
            Manager.MainFrame = MainFrame;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }

        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                btnBack.Visibility = Visibility.Visible;
            }
            else
            {
                btnBack.Visibility =Visibility.Hidden;
            }
        }

        private void BtnServices_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ServicePage());
        }

        private void BtnClients_Click(object sender, RoutedEventArgs e)
        {
            //MainFrame.Navigate(new ClientsPage());
        }

        private void BtnEmployees_Click(object sender, RoutedEventArgs e)
        {
            //MainFrame.Navigate(new EmployeesPage());
        }

        private void BtnRequests_Click(object sender, RoutedEventArgs e)
        {
            //MainFrame.Navigate(new RequestsPage());
        }
    }
}
