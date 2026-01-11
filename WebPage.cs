using CefSharp.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace browser
{
    internal class WebPage
    {
        public ChromiumWebBrowser browser { get; set; }
        public TabItem tabItem { get; set; }
        public Button closeButton { get; set; }
        public Frame frame { get; set; }
        public ChromiumWebBrowser icon { get; set; }

        public WebPage(string HeaderText, string URL, int width, bool ShowBrowser)
        {
            browser = new ChromiumWebBrowser();
            browser.LoadUrl(URL);
            browser.Margin = new Thickness(0, 80, 0, 0);

            frame = new Frame
            {
                Margin = new Thickness(0,80,0,0),
                NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden
            };

            Grid headerGrid = new Grid 
            { 
                Width = width, 
                HorizontalAlignment = HorizontalAlignment.Left 
            };

            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20) });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });

            tabItem = new TabItem 
            {
                Width = width
            };

            icon = new ChromiumWebBrowser
            {
                Width = 14,
                Height = 14,
                Margin = new Thickness(5,0,0,0),
                BorderThickness = new Thickness(0),
                Address = System.AppDomain.CurrentDomain.BaseDirectory + $"../../Images/unknownIcon.png",
                IsEnabled = false,
            };

            RenderOptions.SetBitmapScalingMode(icon, BitmapScalingMode.HighQuality);

            headerGrid.Children.Add(icon);
            Grid.SetColumn(icon, 0);

            TextBlock headerText = new TextBlock
            {
                Text = HeaderText,
                Width = width - 40,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10,0,0,0)
            };

            headerGrid.Children.Add(headerText);
            Grid.SetColumn(headerText, 1);

            closeButton = new Button
            {
                Width = 20,
                Height = 20,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                FontWeight = FontWeights.Bold, // lehet változtatni
                FontSize = 12,
                Content = "x",
                Foreground = new SolidColorBrush(Colors.Red),
                Padding = new Thickness(0),
                Background = new SolidColorBrush(Colors.Transparent),
                BorderThickness = new Thickness(0),
                Cursor = Cursors.Hand
            };

            headerGrid.Children.Add(closeButton);
            Grid.SetColumn(closeButton, 2);

            tabItem.Header = headerGrid;
            if (ShowBrowser)
            {
                tabItem.Content = browser;
            }
            else
            {
                tabItem.Content = frame;
            }
        }
    }
}
