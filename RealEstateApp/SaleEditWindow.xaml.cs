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
using System.Windows.Shapes;

namespace RealEstateApp
{
    /// <summary>
    /// Логика взаимодействия для SaleEditWindow.xaml
    /// </summary>
    public partial class SaleEditWindow : Window
    {
        private Sales _sale;
        private bool _isEditMode;

        public SaleEditWindow()
        {
            InitializeComponent();
            _sale = new Sales();
            _isEditMode = false;
            LoadComboBoxes();
            dpSaleDate.SelectedDate = DateTime.Today;
            cbStatus.SelectedIndex = 0;
        }

        public SaleEditWindow(Sales sale)
        {
            InitializeComponent();
            _sale = sale;
            _isEditMode = true;
            LoadComboBoxes();
            LoadData();
        }

        private void LoadComboBoxes()
        {
            var apartments = DbConnection.db.Apartments.ToList();
            var apartmentsWithDisplay = apartments.Select(a => new
            {
                a.Id,
                DisplayText = $"Дом {a.BuildingNumber}, кв. {a.ApartmentNumber}",
                a.Price
            }).ToList();
            cbApartments.ItemsSource = apartmentsWithDisplay;
            cbApartments.DisplayMemberPath = "DisplayText";
            cbApartments.SelectedValuePath = "Id";

            cbClients.ItemsSource = DbConnection.db.Clients.ToList();
            cbClients.DisplayMemberPath = "FullName";
            cbClients.SelectedValuePath = "Id";
        }

        private void LoadData()
        {
            cbApartments.SelectedValue = _sale.ApartmentId;
            cbClients.SelectedValue = _sale.ClientId;
            dpSaleDate.SelectedDate = _sale.SaleDate;
            txtAmount.Text = _sale.Amount.ToString();
            cbStatus.SelectedItem = cbStatus.Items.Cast<ComboBoxItem>().FirstOrDefault(i => i.Content.ToString() == _sale.Status);
        }

        private void CbApartments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isEditMode && cbApartments.SelectedItem != null)
            {
                dynamic selected = cbApartments.SelectedItem;
                if (selected != null && string.IsNullOrWhiteSpace(txtAmount.Text))
                {
                    txtAmount.Text = selected.Price?.ToString() ?? "";
                }
            }
        }

        private bool Validate()
        {
            if (cbApartments.SelectedValue == null)
            {
                MessageBox.Show("Выберите квартиру.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (cbClients.SelectedValue == null)
            {
                MessageBox.Show("Выберите клиента.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (dpSaleDate.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату продажи.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAmount.Text) || !decimal.TryParse(txtAmount.Text, out _))
            {
                MessageBox.Show("Сумма должна быть числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (cbStatus.SelectedItem == null)
            {
                MessageBox.Show("Выберите статус.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            int apartmentId = (int)cbApartments.SelectedValue;

            if (!_isEditMode || (_sale.ApartmentId != apartmentId))
            {
                if (DbConnection.db.Sales.Any(s => s.ApartmentId == apartmentId))
                {
                    MessageBox.Show("Данная квартира уже участвует в продаже.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            return true;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate()) return;

            _sale.ApartmentId = (int)cbApartments.SelectedValue;
            _sale.ClientId = (int)cbClients.SelectedValue;
            _sale.SaleDate = dpSaleDate.SelectedDate.Value;
            _sale.Amount = decimal.Parse(txtAmount.Text);
            _sale.Status = ((ComboBoxItem)cbStatus.SelectedItem).Content.ToString();

            if (!_isEditMode)
            {
                DbConnection.db.Sales.Add(_sale);
            }

            DbConnection.db.SaveChanges();
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
