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
    /// Логика взаимодействия для ApartmentEditWindow.xaml
    /// </summary>
    public partial class ApartmentEditWindow : Window
    {
        private Apartments _apartment;
        private bool _isEditMode;

        public ApartmentEditWindow()
        {
            InitializeComponent();
            _apartment = new Apartments();
            _isEditMode = false;
        }

        public ApartmentEditWindow(Apartments apartment)
        {
            InitializeComponent();
            _apartment = apartment;
            _isEditMode = true;
            LoadData();
        }

        private void LoadData()
        {
            txtBuildingNumber.Text = _apartment.BuildingNumber.ToString();
            txtApartmentNumber.Text = _apartment.ApartmentNumber.ToString();
            txtRooms.Text = _apartment.Rooms?.ToString();
            txtArea.Text = _apartment.Area?.ToString();
            txtFloor.Text = _apartment.Floor?.ToString();
            txtPrice.Text = _apartment.Price?.ToString();
        }

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(txtBuildingNumber.Text) ||
                string.IsNullOrWhiteSpace(txtApartmentNumber.Text))
            {
                MessageBox.Show("Номер дома и номер квартиры обязательны.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!int.TryParse(txtBuildingNumber.Text, out int buildingNumber))
            {
                MessageBox.Show("Номер дома должен быть целым числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!int.TryParse(txtApartmentNumber.Text, out int apartmentNumber))
            {
                MessageBox.Show("Номер квартиры должен быть целым числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var existing = DbConnection.db.Apartments.FirstOrDefault(a => a.BuildingNumber == buildingNumber && a.ApartmentNumber == apartmentNumber);
            if (existing != null && (_isEditMode == false || existing.Id != _apartment.Id))
            {
                MessageBox.Show("Квартира с таким номером дома и номером квартиры уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtRooms.Text) && !int.TryParse(txtRooms.Text, out _))
            {
                MessageBox.Show("Количество комнат должно быть целым числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!string.IsNullOrWhiteSpace(txtArea.Text) && !decimal.TryParse(txtArea.Text, out _))
            {
                MessageBox.Show("Площадь должна быть числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!string.IsNullOrWhiteSpace(txtFloor.Text) && !int.TryParse(txtFloor.Text, out _))
            {
                MessageBox.Show("Этаж должен быть целым числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!string.IsNullOrWhiteSpace(txtPrice.Text) && !decimal.TryParse(txtPrice.Text, out _))
            {
                MessageBox.Show("Цена должна быть числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate()) return;

            _apartment.BuildingNumber = int.Parse(txtBuildingNumber.Text);
            _apartment.ApartmentNumber = int.Parse(txtApartmentNumber.Text);
            _apartment.Rooms = string.IsNullOrWhiteSpace(txtRooms.Text) ? (int?)null : int.Parse(txtRooms.Text);
            _apartment.Area = string.IsNullOrWhiteSpace(txtArea.Text) ? (decimal?)null : decimal.Parse(txtArea.Text);
            _apartment.Floor = string.IsNullOrWhiteSpace(txtFloor.Text) ? (int?)null : int.Parse(txtFloor.Text);
            _apartment.Price = string.IsNullOrWhiteSpace(txtPrice.Text) ? (decimal?)null : decimal.Parse(txtPrice.Text);

            if (!_isEditMode)
            {
                DbConnection.db.Apartments.Add(_apartment);
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
