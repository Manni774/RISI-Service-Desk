using System;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RISI_Service_Desk
{
    public partial class AddEditServicePage : Page
    {
        private Service _currentService;
        private bool _isEditMode;

        public AddEditServicePage(Service selectedService)
        {
            InitializeComponent();

            _isEditMode = (selectedService != null);
            _currentService = selectedService ?? new Service();
            CmbNameServise.ItemsSource = RISI_ServiceDeskEntities1.GetContext().Services.ToList();
            DataContext = _currentService;

            if (_isEditMode)
            {
                // Заполняем поля для редактирования (привязка уже есть, но можно задать явно)
                txtDescription.Text = _currentService.Description;
                txtBasePrice.Text = _currentService.BasePrice?.ToString("F2");
            }
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

            decimal? price = null;
            if (!string.IsNullOrWhiteSpace(txtBasePrice.Text))
            {
                string priceText = txtBasePrice.Text.Trim().Replace(',', '.');
                if (!decimal.TryParse(priceText, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal parsedPrice))
                {
                    MessageBox.Show("Цена должна быть числом (можно использовать точку или запятую).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtBasePrice.Focus();
                    return;
                }
                price = parsedPrice;
            }

            _currentService.Description = txtDescription.Text;
            _currentService.BasePrice = price;

            using (var context = new RISI_ServiceDeskEntities1())
            {
                try
                {
                    if (!_isEditMode)
                        context.Services.Add(_currentService);
                    else
                    {
                        context.Services.Attach(_currentService);
                        context.Entry(_currentService).State = EntityState.Modified;
                    }
                    context.SaveChanges();
                    MessageBox.Show("Сохранено успешно.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Возврат на предыдущую страницу
                    if (NavigationService != null && NavigationService.CanGoBack)
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
            if (NavigationService != null && NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}