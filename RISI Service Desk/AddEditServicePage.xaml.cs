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
    /// Логика взаимодействия для AddEditServicePage.xaml
    /// </summary>
    public partial class AddEditServicePage : Page
    {
        private Service _currentServices = new Service();
        private bool _isEditMode;

        public AddEditServicePage(Service selectedServices)
        {
            InitializeComponent();
            if (selectedServices != null)
                _currentServices = selectedServices;
            DataContext = _currentServices;
            CmbNameServise.ItemsSource = RISI_ServiceDeskEntities1.GetContext().Services.ToList();
            if (!_isEditMode) Title = "Добавление услуги";
            else Title = "Редактирование услуги";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            // Валидация
            if (string.IsNullOrWhiteSpace(CmbNameServise.Text))
            {
                MessageBox.Show("Выберите наименование услуги.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                CmbNameServise.Focus();
                return;
            }

            decimal price = 0;
            if (!string.IsNullOrWhiteSpace(txtBasePrice.Text) && !decimal.TryParse(txtBasePrice.Text, out price))
            {
                MessageBox.Show("Цена должна быть числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtBasePrice.Focus();
                return;
            }

            _currentServices.ServiceName = CmbNameServise.Text.Trim();
            _currentServices.Description = txtDescription.Text;
            _currentServices.BasePrice = price;

            using (var context = new RISI_ServiceDeskEntities1()) // ваш контекст
            {
                try
                {
                    if (!_isEditMode)
                        context.Services.Add(_currentServices);
                    else
                    {
                        context.Services.Attach(_currentServices);
                        context.Entry(_currentServices).State = EntityState.Modified;
                    }
                    context.SaveChanges();
                    MessageBox.Show("Сохранено успешно.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Возврат на предыдущую страницу (список услуг)
                    if (NavigationService.CanGoBack)
                        NavigationService.GoBack();
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
