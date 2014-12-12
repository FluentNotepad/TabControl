using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Forms.Integration;
using System.Windows.Forms;
using Wpf.Controls;

namespace Test
{
    /// <summary>
    /// Interaction logic for WindowUsingItemProperty.xaml
    /// </summary>
    public partial class WindowUsingItemProperty : Window
    {
        public WindowUsingItemProperty()
        {
            InitializeComponent();

            tabControl.TabItemAdded += tabControl_TabItemAdded;
            tabControl.SelectionChanged += tabControl_SelectionChanged;

            // these 3 events ensure that all the contents of the textbox are selected on a mouseclick (as per IE)
            // code borrowed from the Windows Presentation Foundation Forum at http://forums.microsoft.com/MSDN/ShowPost.aspx?PostID=2199428&SiteID=1
            textBox.GotKeyboardFocus += delegate(object sender, KeyboardFocusChangedEventArgs e)
            {
                System.Windows.Controls.TextBox tb = (sender as System.Windows.Controls.TextBox);
                if (tb != null)
                    tb.SelectAll();
            };
            textBox.MouseDoubleClick += delegate(object sender, MouseButtonEventArgs e)
            {
                System.Windows.Controls.TextBox tb = (sender as System.Windows.Controls.TextBox);
                if (tb != null)
                    tb.SelectAll();
            };
            textBox.PreviewMouseLeftButtonDown += delegate(object sender, MouseButtonEventArgs e)
            {
                System.Windows.Controls.TextBox tb = (sender as System.Windows.Controls.TextBox);

                if (tb != null)
                {
                    if (!tb.IsKeyboardFocusWithin)
                    {
                        e.Handled = true;
                        tb.Focus();
                    }
                }
            };
        }


        private int count = 1;
        void tabControl_TabItemAdded(object sender, TabItemEventArgs e)
        {
            // Add an Icon to the tabItem
            BitmapImage image = new BitmapImage(new Uri("pack://application:,,,/Test;component/Images/ie.ico"));
            Image img = new Image();
            img.Source = image;
            img.Width = 16;
            img.Height = 16;
            img.Margin = new Thickness(2, 0, 2, 0);

            e.TabItem.Icon = img;

            // wrap the header in a textblock, this gives us the  character ellipsis (...) when trimmed
            TextBlock tb = new TextBlock();
            tb.Text = "New Tab " + count++;
            tb.TextTrimming = TextTrimming.CharacterEllipsis;
            tb.TextWrapping = TextWrapping.NoWrap;

            e.TabItem.Header = tb;

            // add a WebControl to the TAbItem
            WindowsFormsHost host = new WindowsFormsHost();
            host.Margin = new Thickness(2);
            System.Windows.Forms.WebBrowser browser = new System.Windows.Forms.WebBrowser();
            browser.DocumentTitleChanged += Browser_DocumentTitleChanged;
            browser.Navigated += Browser_Navigated;

            host.Child = browser;
            e.TabItem.Content = host;
        }


        void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Forms.WebBrowser browser = GetCurrentWebBrowser();
            if (browser == null) return;

            // set the Textbox to the Url of the selected tab
            if (browser.Url == null)
                textBox.Text = "about:blank";
            else
                textBox.Text = browser.Url.ToString();

            textBox.Focus();
        }

        void Browser_DocumentTitleChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.WebBrowser browser = sender as System.Windows.Forms.WebBrowser;
            if (browser == null) return;

            // update the TabItems's Header property
            Wpf.Controls.TabItem item = tabControl.SelectedItem as Wpf.Controls.TabItem;

            // Add an Icon to the tabItem
            BitmapImage image = new BitmapImage(new Uri("pack://application:,,,/Test;component/Images/ie.ico"));
            Image img = new Image();
            img.Source = image;
            img.Width = 16;
            img.Height = 16;
            img.Margin = new Thickness(2, 0, 2, 0);

            if (item != null) item.Icon = img;

            // wrap the header in a textblock, this gives us the  character ellipsis (...) when trimmed
            TextBlock tb = new TextBlock();
            tb.Text= browser.DocumentTitle;
            tb.TextTrimming = TextTrimming.CharacterEllipsis;
            tb.TextWrapping = TextWrapping.NoWrap;

            if (item != null) item.Header = tb;
        }

        void Browser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            System.Windows.Forms.WebBrowser browser = sender as System.Windows.Forms.WebBrowser;
            if (browser == null) return;

            textBox.Text = browser.Url.ToString();
        }

        private void textBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                try
                {
                    Cursor = System.Windows.Input.Cursors.Wait;
                    System.Windows.Forms.WebBrowser browser = GetCurrentWebBrowser();
                    if (browser == null) return;

                    // navigate to the selected web address
                    browser.Navigate(textBox.Text);
                }
                finally
                {
                    Cursor = System.Windows.Input.Cursors.Arrow;
                }
            }
        }

        private System.Windows.Forms.WebBrowser GetCurrentWebBrowser()
        {
            Wpf.Controls.TabItem item = tabControl.SelectedItem as Wpf.Controls.TabItem;
            if (item == null) return null;

            WindowsFormsHost host = item.Content as WindowsFormsHost;
            if (host == null) return null;

            System.Windows.Forms.WebBrowser browser = host.Child as System.Windows.Forms.WebBrowser;
            return browser;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.TextBox tb = (sender as System.Windows.Controls.TextBox);
            if (tb != null)
                tb.SelectAll();            
        }
    }
}
