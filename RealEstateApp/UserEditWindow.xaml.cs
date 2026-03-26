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
    /// Логика взаимодействия для UserEditWindow.xaml
    /// </summary>
    public partial class UserEditWindow : Window
    {
        private Users _user;
        private bool _isEditMode;

        public UserEditWindow()
        {
            InitializeComponent();
            _user = new Users();
            _isEditMode = false;
        }

        public UserEditWindow(Users user)
        {
            InitializeComponent();
            _user = user;
            _isEditMode = true;
            LoadData();
        }

        private void LoadData()
        {
            txtUsername.Text = _user.Username;
            txtPassword.Password = "";
            cbRole.SelectedItem = cbRole.Items.Cast<ComboBoxItem>().FirstOrDefault(i => i.Content.ToString() == _user.Role);
        }

        private bool Validate()
        {
            string username = txtUsername.Text.Trim();
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Логин обязателен.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var existing = DbConnection.db.Users.FirstOrDefault(u => u.Username == username);
            if (existing != null && (_isEditMode == false || existing.Id != _user.Id))
            {
                MessageBox.Show("Пользователь с таким логином уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            string password = txtPassword.Password;
            if (string.IsNullOrWhiteSpace(password) && !_isEditMode)
            {
                MessageBox.Show("Пароль обязателен.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (cbRole.SelectedItem == null)
            {
                MessageBox.Show("Выберите роль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate()) return;

            _user.Username = txtUsername.Text.Trim();
            string newPassword = txtPassword.Password;
            if (!string.IsNullOrEmpty(newPassword))
                _user.Password = newPassword; 

            _user.Role = ((ComboBoxItem)cbRole.SelectedItem).Content.ToString();

            if (!_isEditMode)
            {
                DbConnection.db.Users.Add(_user);
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
