using GlobusT.Models;
using GlobusT.Services;
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

namespace GlobusT
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private AuthService _authService;
        public LoginWindow()
        {
            InitializeComponent();
            _authService = new AuthService();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            if (Validate() == false)
            {
                return;
            }

            Role role = _authService.TryAuth(LogintextBox.Text, PasswordtextBox.Text);

            if (role != null)
            {
                MainWindow mainWindow = new MainWindow(role);
                mainWindow.Show();
                this.Close();
            }
        }

        private void GuestButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(null);
            mainWindow.Show();
            this.Close();
        }

        private bool Validate()
        {
            if (string.IsNullOrEmpty(LogintextBox.Text) || string.IsNullOrEmpty(PasswordtextBox.Text))
            {
                MessageBox.Show("Поле Логин или Пароль не могут быть пустыми!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}
