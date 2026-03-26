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

namespace RealEstateApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _userRole;

        public MainWindow(string userRole)
        {
            InitializeComponent();
            _userRole = userRole;

            // Если роль Admin, показываем кнопку управления пользователями
            if (_userRole == "Admin")
                btnUserManagement.Visibility = Visibility.Visible;

            LoadData();
        }

        private void LoadData()
        {
            dgApartments.ItemsSource = DbConnection.db.Apartments.ToList();
            dgClients.ItemsSource = DbConnection.db.Clients.ToList();
            dgSales.ItemsSource = DbConnection.db.Sales.ToList();
        }

        private void RefreshData()
        {
            DbConnection.db.SaveChanges();
            LoadData();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void UserManagement_Click(object sender, RoutedEventArgs e)
        {
            var userMgmtWindow = new UserManagementWindow();
            userMgmtWindow.ShowDialog();
        }

        private void AddApartment_Click(object sender, RoutedEventArgs e)
        {
            var window = new ApartmentEditWindow();
            if (window.ShowDialog() == true)
            {
                RefreshData();
            }
        }

        private void EditApartment_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgApartments.SelectedItem as Apartments;
            if (selected == null)
            {
                MessageBox.Show("Выберите квартиру для редактирования.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var window = new ApartmentEditWindow(selected);
            if (window.ShowDialog() == true)
            {
                RefreshData();
            }
        }

        private void DeleteApartment_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgApartments.SelectedItem as Apartments;
            if (selected == null)
            {
                MessageBox.Show("Выберите квартиру для удаления.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("Удалить выбранную квартиру?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (DbConnection.db.Sales.Any(s => s.ApartmentId == selected.Id))
                {
                    MessageBox.Show("Невозможно удалить квартиру, так как она участвует в продажах.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                DbConnection.db.Apartments.Remove(selected);
                RefreshData();
            }
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            var window = new ClientEditWindow();
            if (window.ShowDialog() == true)
            {
                RefreshData();
            }
        }

        private void EditClient_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgClients.SelectedItem as Clients;
            if (selected == null)
            {
                MessageBox.Show("Выберите клиента для редактирования.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var window = new ClientEditWindow(selected);
            if (window.ShowDialog() == true)
            {
                RefreshData();
            }
        }

        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgClients.SelectedItem as Clients;
            if (selected == null)
            {
                MessageBox.Show("Выберите клиента для удаления.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("Удалить выбранного клиента?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (DbConnection.db.Sales.Any(s => s.ClientId == selected.Id))
                {
                    MessageBox.Show("Невозможно удалить клиента, так как он участвует в продажах.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                DbConnection.db.Clients.Remove(selected);
                RefreshData();
            }
        }

        private void AddSale_Click(object sender, RoutedEventArgs e)
        {
            var window = new SaleEditWindow();
            if (window.ShowDialog() == true)
            {
                RefreshData();
            }
        }

        private void EditSale_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgSales.SelectedItem as Sales;
            if (selected == null)
            {
                MessageBox.Show("Выберите продажу для редактирования.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var window = new SaleEditWindow(selected);
            if (window.ShowDialog() == true)
            {
                RefreshData();
            }
        }

        private void DeleteSale_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgSales.SelectedItem as Sales;
            if (selected == null)
            {
                MessageBox.Show("Выберите продажу для удаления.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("Удалить выбранную продажу?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DbConnection.db.Sales.Remove(selected);
                RefreshData();
            }
        }
    }
}
