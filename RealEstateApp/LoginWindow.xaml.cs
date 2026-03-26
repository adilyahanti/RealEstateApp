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
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                tbError.Text = "Введите логин и пароль.";
                return;
            }

            var user = DbConnection.db.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user == null)
            {
                tbError.Text = "Неверный логин или пароль.";
                return;
            }

            MainWindow mainWindow = new MainWindow(user.Role);
            mainWindow.Show();
            this.Close();
        }
    }
}
