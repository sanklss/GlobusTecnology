using GlobusT.Models;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Role _role;
        public MainWindow(Role role)
        {
            InitializeComponent();
            _role = role;
            MainFrame.Navigated += MainFrame_Navigated;
            MainFrame.Navigate(new ServicePage(role));
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is Page page)
            {
                if (_role == null)
                {
                    this.Title = $"Глобус Т. - Гость - {page.Title}";
                }
                else
                {
                    this.Title = $"Глобус Т. - {page.Title}"; ;
                }
            }
            else
            {
                this.Title = "Глобус Т.";
            }
        }
    }
}