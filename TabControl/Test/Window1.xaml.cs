using System;
using System.Windows;

namespace Test
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Items_Click(object sender, RoutedEventArgs e)
        {
            var win = new WindowUsingItemProperty();
            win.Show();
        }
        private void ItemsSource_Click(object sender, RoutedEventArgs e)
        {
            var win = new WindowUsingItemsSourceProperty();
            win.Show();
        }
    }
}
