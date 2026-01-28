using GlobusT.Data;
using GlobusT.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

namespace GlobusT
{
    /// <summary>
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {
        private Role _role;
        public ServicePage(Role role)
        {
            InitializeComponent();
            LoadData();
            _role = role;
            AdjustInterface();
        }

        private void LoadData()
        {
            using (var context = new GlobusTechnologyContext())
            {
                ServicesItemControl.ItemsSource = context.Services
                    .Include(r => r.IdTypeCommandNavigation)
                    .Include(r => r.IdDevelopmentAreaNavigation)
                    .ToList();

            }
        }

        private void AdjustInterface()
        {
            if (_role == null)
            {
                RequestButton.Visibility = Visibility.Collapsed;
                return;
            }

            switch (_role.Id)
            {
                case 1:
                    RequestButton.Visibility = Visibility.Visible;
                    break;
                case 2:
                    RequestButton.Visibility = Visibility.Visible;
                    break;
                case 3:
                    RequestButton.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void RequestPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RequestPage());
        }
    }
}
