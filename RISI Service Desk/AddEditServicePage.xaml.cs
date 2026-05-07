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
    /// Логика взаимодействия для AddEditServicePage.xaml
    /// </summary>
    public partial class AddEditServicePage : Page
    {
        private Service _currentServices = new Service();

        public AddEditServicePage(Service selectedServices)
        {
            InitializeComponent();
            if (selectedServices != null)
                _currentServices = selectedServices;
            DataContext = _currentServices;
            CmbNameServise.ItemsSource = RISI_ServiceDeskEntities1.GetContext().Services.ToList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
