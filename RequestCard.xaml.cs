using GlobusT.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GlobusT
{
    public partial class RequestCard : UserControl
    {
        public RequestCard()
        {
            InitializeComponent();
            this.Loaded += RequestCard_Loaded;
        }

        private void RequestCard_Loaded(object sender, RoutedEventArgs e)
        {
            var itemsControl = FindParentItemsControl(this);
            var role = itemsControl?.Tag as Role;

            if (role?.Id != 2)
            {
                var editButton = this.FindName("EditButton") as Button;
                if (editButton != null)
                {
                    editButton.Visibility = Visibility.Collapsed;
                }
            }
        }

        private ItemsControl FindParentItemsControl(DependencyObject child)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            while (parent != null && !(parent is ItemsControl))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as ItemsControl;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var itemsControl = FindParentItemsControl(this);
                var role = itemsControl?.Tag as Role;

                if (role?.Id != 2)
                {
                    MessageBox.Show("Только менеджеры могут редактировать заявки!",
                        "Доступ запрещен", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (DataContext is Request request)
                {
                    var requestWindow = new RequestWindow(request);
                    requestWindow.ShowDialog();

                    if (requestWindow.DialogResult == true)
                    {
                        var page = FindParentPage(this);
                        if (page is RequestPage requestPage)
                        {
                            requestPage.ApplyFilters();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия заявки: {ex.Message}", "Ошибка");
            }
        }

        private Page FindParentPage(DependencyObject child)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            while (parent != null && !(parent is Page))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as Page;
        }
    }
}