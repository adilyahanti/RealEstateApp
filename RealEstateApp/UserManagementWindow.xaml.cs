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
    /// Логика взаимодействия для UserManagementWindow.xaml
    /// </summary>
    public partial class UserManagementWindow : Window
    {
        public UserManagementWindow()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            dgUsers.ItemsSource = DbConnection.db.Users.ToList();
        }

        private void RefreshUsers()
        {
            DbConnection.db.SaveChanges();
            LoadUsers();
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var window = new UserEditWindow();
            if (window.ShowDialog() == true)
            {
                RefreshUsers();
            }
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgUsers.SelectedItem as Users;
            if (selected == null)
            {
                MessageBox.Show("Выберите пользователя для редактирования.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var window = new UserEditWindow(selected);
            if (window.ShowDialog() == true)
            {
                RefreshUsers();
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgUsers.SelectedItem as Users;
            if (selected == null)
            {
                MessageBox.Show("Выберите пользователя для удаления.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (selected.Role == "Admin" && DbConnection.db.Users.Count(u => u.Role == "Admin") == 1)
            {
                MessageBox.Show("Нельзя удалить последнего администратора.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show($"Удалить пользователя {selected.Username}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DbConnection.db.Users.Remove(selected);
                RefreshUsers();
            }
        }
    }
}
