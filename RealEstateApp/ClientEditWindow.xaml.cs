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
    /// Логика взаимодействия для ClientEditWindow.xaml
    /// </summary>
    public partial class ClientEditWindow : Window
    {
        private Clients _client;
        private bool _isEditMode;

        public ClientEditWindow()
        {
            InitializeComponent();
            _client = new Clients();
            _isEditMode = false;
        }

        public ClientEditWindow(Clients client)
        {
            InitializeComponent();
            _client = client;
            _isEditMode = true;
            LoadData();
        }

        private void LoadData()
        {
            txtFullName.Text = _client.FullName;
            txtPhone.Text = _client.Phone;
            txtPassport.Text = _client.PassportData;
        }

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("ФИО обязательно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate()) return;

            _client.FullName = txtFullName.Text;
            _client.Phone = txtPhone.Text;
            _client.PassportData = txtPassport.Text;

            if (!_isEditMode)
            {
                DbConnection.db.Clients.Add(_client);
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
