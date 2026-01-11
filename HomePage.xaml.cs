using System.Windows;
using System.Windows.Controls;

namespace browser.Pages
{
    public partial class HomePage : Page
    {
        Frame frame;
        public HomePage(Frame Frame)
        {
            frame = Frame;
            InitializeComponent();
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new ContactPage());
        }

    }
}
