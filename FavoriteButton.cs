using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CefSharp.Wpf;

namespace browser
{
    internal class FavoriteButton
    {
        public Button Button { get; set; }
        public string URL { get; set; }
        public ChromiumWebBrowser Icon { get; set; }
        public TextBlock Text { get; set; }

        public FavoriteButton(string browserURL, string iconURL, string Title)
        {
            URL = browserURL;

            Grid containerGrid = new Grid
            {
                Width = 100,
                Height = 20,
                VerticalAlignment = VerticalAlignment.Center
            };
            containerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20) });
            containerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

            Icon = new ChromiumWebBrowser
            {
                Width = 15,
                Height = 15,
                Address = iconURL,
                IsEnabled = false
            };

            containerGrid.Children.Add(Icon);
            Grid.SetColumn(Icon, 0);

            Text = new TextBlock
            {
                Width = 85,
                Height = 20,
                Text = Title,
                VerticalAlignment = VerticalAlignment.Center,
            };

            containerGrid.Children.Add(Text);
            Grid.SetColumn(Text, 1);

            Button = new Button
            {
                Width = 100,
                Height = 20,
                Margin = new Thickness(5, 0, 5, 0)
            };

            Button.Content = containerGrid;
        }
    }
}
