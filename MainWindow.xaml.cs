using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace browser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WebPage currentPage;
        List<WebPage> webPages = new List<WebPage>();
        Database DB = new Database("localhost", "browser", "root", "");
        string userID;
        double idleAnimationTimerSeconds = 20;
        bool animationStarted = false;

        List<FavoriteButton> favorites = new List<FavoriteButton>();
        FavoriteButton selectedFavorite;

        DispatcherTimer animationTimer;

        public MainWindow()
        {
            InitializeComponent();

            animationTimer = new DispatcherTimer();
            animationTimer.Interval = TimeSpan.FromSeconds(1);
            animationTimer.Tick += AnimationTimer_Tick;
            //animationTimer.Start();
            
            CreateNewTab();
            CreateAddTabButton();
            tabControl.SelectedIndex = 0;

            userID = DB.getUserID(Login.username);
            userName.Text = Login.username;
            usernameButton.ToolTip = Login.username;

            loadUsersFavorites();
        }
        
        #region animation
        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (currentPage.tabItem.Content.GetType() == typeof(Frame))
            {
                if (TimeSpan.FromMilliseconds(LastInputInfo.GetIdleTime()).Seconds >= idleAnimationTimerSeconds && !animationStarted)
                {
                    currentPage.frame.Navigate(new Pages.Animation());
                    animationStarted = true;
                }
                else if (TimeSpan.FromMilliseconds(LastInputInfo.GetIdleTime()).Seconds < idleAnimationTimerSeconds && currentPage.frame.Content.GetType() == typeof(Pages.Animation))
                {
                    currentPage.frame.Navigate(new Pages.HomePage(currentPage.frame));
                    animationStarted = false;
                }
            }
        }
        #endregion

        #region creating tabs
        private void CreateAddTabButton()
        {
            TabItem addTabItem = new TabItem
            {
                Width = 50,
                Header = "+",
                Cursor = Cursors.Hand
            };

            tabControl.Items.Add(addTabItem);
        }

        private void CreateNewTab(string headerText = "Új lap", string url = "https://google.com", int width = 200, bool ShowBrowser = false)
        {
            WebPage page = new WebPage(headerText, url, width, ShowBrowser);
            webPages.Add(page);
            if (currentPage != null)
            {
                ((Grid)webPages[webPages.IndexOf(currentPage)].closeButton.Parent).ColumnDefinitions[0].Width = new GridLength(20);
                webPages[tabControl.SelectedIndex].closeButton.Margin = new Thickness(0);
            }
            currentPage = page;

            if (tabControl.Items.Count >= 2)
            {
                tabControl.Items.Insert(tabControl.Items.Count - 1, page.tabItem);
                tabControl.SelectedIndex = tabControl.Items.Count - 2;
            }
            else
            {
                tabControl.Items.Add(page.tabItem);
                tabControl.SelectedIndex = tabControl.Items.Count - 1;
            }

            page.closeButton.Click += PageCloseButton_Click;
            page.frame.Navigate(new Pages.HomePage(page.frame));

            // browser events
            page.browser.TitleChanged += Browser_TitleChanged;
            page.browser.AddressChanged += Browser_AddressChanged;
            page.browser.FrameLoadStart += Browser_FrameLoadStart;
            page.browser.FrameLoadEnd += Browser_FrameLoadEnd;
            page.browser.LoadError += Browser_LoadError;

            ResizeTabs();

            AddressBar.Text = "";
        }
        #endregion

        #region browser events
        private void Browser_TitleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                WebPage page = webPages[tabControl.Items.IndexOf((TabItem)((ChromiumWebBrowser)sender).Parent)];
                ((TextBlock)((Grid)page.closeButton.Parent).Children[1]).Text = page.browser.Title;
            });
        }

        private void Browser_AddressChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                WebPage page = webPages[tabControl.Items.IndexOf((TabItem)((ChromiumWebBrowser)sender).Parent)];
                AddressBar.Text = page.browser.Address;
            });
        }

        private void Browser_FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                Dispatcher.Invoke(() =>
                {
                    ReloadButton.Content = (System.Windows.Controls.Image)FindResource("closeImage");
                });
            }
        }

        private void Browser_FrameLoadEnd(object sender, CefSharp.FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                Dispatcher.Invoke(() =>
                {
                    ReloadButton.Content = (System.Windows.Controls.Image)FindResource("reloadImage");

                    WebPage page = webPages[tabControl.Items.IndexOf((TabItem)((ChromiumWebBrowser)sender).Parent)];
                    SetFavicon(page, e);
                    favoritesButton.IsEnabled = true;
                });

            }
        }

        private void Browser_LoadError(object sender, LoadErrorEventArgs e)
        {
            WebPage page = webPages[tabControl.Items.IndexOf((TabItem)((ChromiumWebBrowser)sender).Parent)];
            if (e.Frame.IsMain)
            {
                string newURL = "";
                if (e.ErrorText == "ERR_NAME_NOT_RESOLVED")
                {
                    Dispatcher.Invoke(() =>
                    {
                        newURL = $"https://www.google.com/search?q={e.FailedUrl.Substring(7, e.FailedUrl.Length - 8)}";
                        AddressBar.Text = newURL;
                        page.browser.Load(newURL);
                    });
                }
                if (e.ErrorText == "ERR_CONNECTION_CLOSED")
                {
                    if (e.FailedUrl.StartsWith("https://"))
                    {
                        Dispatcher.Invoke(() =>
                        {
                            newURL = e.FailedUrl.Replace("https://", "http://");
                            newURL = newURL.Substring(0, newURL.Length - 1);
                            AddressBar.Text = newURL;
                            page.browser.Load(newURL);
                        });
                    }
                    else if (e.FailedUrl.StartsWith("http://"))
                    {
                        Dispatcher.Invoke(() =>
                        {
                            newURL = $"https://www.google.com/search?q={newURL.Substring(7, newURL.Length - 8)}";
                            AddressBar.Text = newURL;
                            page.browser.Load(newURL);
                        });
                    }
                    else
                    {
                        Dispatcher.Invoke(() =>
                        {
                            newURL = $"https://{e.FailedUrl}";
                            AddressBar.Text = newURL;
                            page.browser.Load(newURL);
                        });
                    }
                }
            }
        }


        #endregion

        #region close page
        private void PageCloseButton_Click(object sender, RoutedEventArgs e)
        {
            TabItem clicked = (TabItem)((Grid)((Button)sender).Parent).Parent;

            int pageIndex = tabControl.Items.IndexOf(clicked);
            WebPage clickedPage = webPages[pageIndex];

            if (tabControl.Items.Count > 2)
            {
                if (pageIndex > 0 && pageIndex <= tabControl.SelectedIndex && tabControl.SelectedIndex == tabControl.Items.Count - 2)
                {
                    tabControl.SelectedIndex -= 1;
                }

                clickedPage.closeButton.Click -= PageCloseButton_Click;

                clickedPage.browser.TitleChanged -= Browser_TitleChanged;
                clickedPage.browser.AddressChanged -= Browser_AddressChanged;
                clickedPage.browser.FrameLoadStart -= Browser_FrameLoadStart;
                clickedPage.browser.FrameLoadEnd -= Browser_FrameLoadEnd;
                clickedPage.browser.LoadError -= Browser_LoadError;
                clickedPage.browser.Dispose();

                clickedPage.icon.Dispose();

                webPages.Remove(clickedPage);
                tabControl.Items.Remove(clicked);

                ResizeTabs();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }
        #endregion

        #region AddressBar
        private void AddressBar_GotFocus(object sender, RoutedEventArgs e)
        {
            AddressBorder.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0,120,215));
        }

        private void AddressBar_LostFocus(object sender, RoutedEventArgs e)
        {
            AddressBorder.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(221, 221, 221));
        }

        private void AddressBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && AddressBar.Text.Trim() != "")
            {
                currentPage.tabItem.Content = currentPage.browser;
                currentPage.browser.Load(AddressBar.Text.Trim());
            }
        }
        #endregion

        #region navigation buttons
        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage.tabItem.Content.GetType() == typeof(ChromiumWebBrowser) && currentPage.browser.CanGoForward)
            {
                currentPage.browser.GetBrowser().GoForward();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage.tabItem.Content.GetType() == typeof(ChromiumWebBrowser) && currentPage.browser.CanGoBack)
            {
                currentPage.browser.GetBrowser().GoBack();
            }
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage.tabItem.Content.GetType() == typeof(ChromiumWebBrowser))
            {
                if (ReloadButton.Content == (System.Windows.Controls.Image)FindResource("reloadImage"))
                {
                    currentPage.browser.GetBrowser().Reload();
                    ReloadButton.Content = (System.Windows.Controls.Image)FindResource("closeImage");
                }
                else
                {
                    currentPage.browser.GetBrowser().Stop();
                    ReloadButton.Content = (System.Windows.Controls.Image)FindResource("reloadImage");
                }
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleWindowState();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region settings
        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            settingsWindow.Visibility = settingsWindow.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }

        #endregion

        #region switch tabs
        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControl.IsLoaded && tabControl.SelectedIndex >= 0)
            {
                if (tabControl.SelectedIndex == tabControl.Items.Count - 1)
                {
                    CreateNewTab();
                }
                else
                {
                    // set current pages header to show the close button and reset the last one
                    double pageWidth = ((TabItem)tabControl.Items[0]).Width;
                    Grid headerGrid = (Grid)webPages[tabControl.SelectedIndex].closeButton.Parent;
                    Grid lastHeaderGrid = (Grid)currentPage.closeButton.Parent;

                    if (pageWidth < 40)
                    {
                        lastHeaderGrid.ColumnDefinitions[0].Width = new GridLength(20);
                        headerGrid.ColumnDefinitions[0].Width = new GridLength(0);
                    }

                    currentPage = webPages[tabControl.SelectedIndex];
                    if (currentPage.tabItem.Content.GetType() == typeof(Frame))
                    {
                        AddressBar.Text = "";
                    }
                    else
                    {
                        AddressBar.Text = currentPage.browser.Address;
                    }
                    BackButton.IsEnabled = currentPage.browser.CanGoBack;
                    ForwardButton.IsEnabled = currentPage.browser.CanGoForward;
                }
            }
        }
        #endregion

        #region window drag + fullscreen
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point mousePosition = Mouse.GetPosition(this);
            if (mousePosition.Y <= 50)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    // hide all windows except the main one
                    if (favoritesWindow.Visibility == Visibility.Visible)
                    {
                        favoritesWindow.Visibility = Visibility.Collapsed;
                    }
                    if (settingsWindow.Visibility == Visibility.Visible)
                    {
                        settingsWindow.Visibility = Visibility.Collapsed;
                    }

                    // check if the mouse is over a header
                    bool canMoveWindow = true;
                    foreach (TabItem item in tabControl.Items)
                    {
                        if (item.IsMouseOver)
                        {
                            canMoveWindow = false;
                        }
                    }

                    if (canMoveWindow)
                    {
                        if (e.ClickCount >= 2 && e.LeftButton == MouseButtonState.Pressed)
                        {
                            ToggleWindowState();
                        }
                        DragMove();
                    }
                }
            }
        }
        #endregion

        #region keybinds
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (currentPage.tabItem.Content.GetType() == typeof(ChromiumWebBrowser))
            {
                if (e.Key == Key.F5)
                {
                    currentPage.browser.Reload();
                }
            }
            if (e.Key == Key.F11)
            {
                ToggleWindowState();
            }
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.E)
            {
                AddressBar.Focus();
            }
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.T)
            {
                CreateNewTab();
            }
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.W)
            {
                currentPage.closeButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Tab)
            {
                selectNextTabItem();
            }
            if (Keyboard.Modifiers.ToString() == "Control, Shift" && e.Key == Key.Tab)
            {
                selectPreviousTabItem();
            }
        }

        private void tabControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Tab)
            {
                selectNextTabItem();
                e.Handled = true;
            }
            if (Keyboard.Modifiers.ToString() == "Control, Shift" && e.Key == Key.Tab)
            {
                selectPreviousTabItem();
                e.Handled = true;
            }
        }
        #endregion

        #region helper functions
        private async void SetFavicon(WebPage page, FrameLoadEndEventArgs e)
        {
            var asd = await e.Frame.EvaluateScriptAsync("" +
                        "function getFavicon()" +
                        "{" +
                        "   var nodeList = document.getElementsByTagName('link');" +
                        "   for (var i = 0; i < nodeList.length; i++)" +
                        "   {" +
                        "       if ((nodeList[i].getAttribute('rel') == 'icon') || (nodeList[i].getAttribute('rel') == 'shortcut icon'))" +
                        "       {" +
                        "           return nodeList[i].getAttribute('href');" +
                        "       }" +
                        "   }" +
                        "   return undefined;" +
                        "}" +
                        "getFavicon();");

            if (asd.Result != null && asd.Result.ToString() != "undefined")
            {
                if (asd.Result.ToString().StartsWith("https://"))
                {
                    page.icon.Address = asd.Result.ToString();
                }
                else
                {
                    if (page.browser.Address != null && page.browser.Address.ToString().Contains("www.google.com"))
                    {
                        page.icon.Address = "https://www.gstatic.com/images/branding/searchlogo/ico/favicon.ico";
                    }
                    else
                    {
                        page.icon.Address = page.browser.Address.Substring(0, page.browser.Address.Length - 1) + asd.Result.ToString();
                    }
                }
            }
            else
            {
                page.icon.Address = System.AppDomain.CurrentDomain.BaseDirectory + $"../../Images/unknownIcon.png";
            }
        }

        private void ToggleWindowState()
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }


        private void selectNextTabItem()
        {
            if (tabControl.SelectedIndex < tabControl.Items.Count - 2)
            {
                tabControl.SelectedIndex++;
            }
            else
            {
                tabControl.SelectedIndex = 0;
            }
        }

        private void selectPreviousTabItem()
        {
            if (tabControl.SelectedIndex > 0)
            {
                tabControl.SelectedIndex--;
            }
            else
            {
                tabControl.SelectedIndex = tabControl.Items.Count - 2;
            }
        }
        #endregion

        #region favorites

        private void loadUsersFavorites()
        {
            List<Dictionary<string,string>> keyValuePairs = DB.selectUsersFavourites(userID);
            foreach (var favorite in keyValuePairs)
            {
                FavoriteButton button = new FavoriteButton(favorite["url"], favorite["icon_url"], favorite["title"]);
                button.Button.Click += favoritePageButton_Click;
                button.Button.MouseDown += favoriteButton_MouseDown;

                favorites.Add(button);
                favoritesContainer.Children.Add(button.Button);
            }
        }

        private void favoritesButton_Click(object sender, RoutedEventArgs e)
        {
            bool favoriteExists = false;
            foreach (FavoriteButton favorite in favorites)
            {
                if (favorite.URL == currentPage.browser.Address)
                {
                    favoriteExists = true;
                }
            }

            if (!favoriteExists && currentPage.tabItem.Content.GetType() == typeof(ChromiumWebBrowser))
            {
                FavoriteButton button = new FavoriteButton(currentPage.browser.Address, currentPage.icon.Address, currentPage.browser.Title);

                DB.insertFavoriteTab(userID, currentPage.browser.Address, currentPage.icon.Address, currentPage.browser.Title);
                button.Button.Click += favoritePageButton_Click;
                button.Button.MouseDown += favoriteButton_MouseDown;
            
                favorites.Add(button);
                favoritesContainer.Children.Add(button.Button);
            }
        }

        private void favoriteButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                selectedFavorite = favorites[favoritesContainer.Children.IndexOf((Button)sender)];
                favoritesWindow.Visibility = Visibility.Visible;
                System.Windows.Point pos = Mouse.GetPosition(this);
                favoritesWindow.Margin = new Thickness(pos.X, pos.Y, 0, 0);
            }
        }

        private void favoritePageButton_Click(object sender, RoutedEventArgs e)
        {
            FavoriteButton clicked = favorites[favoritesContainer.Children.IndexOf((Button)sender)];
            CreateNewTab(clicked.Text.Text, clicked.URL, ShowBrowser:true);
        }

        private void favoriteButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(selectedFavorite.URL);
            DB.deleteFavoriteTab(userID, selectedFavorite.URL);

            favoritesContainer.Children.Remove(selectedFavorite.Button);
            favorites.Remove(selectedFavorite);
            favoritesWindow.Visibility = Visibility.Collapsed;

        }
        #endregion

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int newAnimationInterval = 0;
                if (int.TryParse(AnimationTimerTextBox.Text.Trim(), out newAnimationInterval) && newAnimationInterval > 0 && newAnimationInterval < 300)
                {
                    idleAnimationTimerSeconds = newAnimationInterval;
                    settingsWindow.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void ResizeTabs()
        {
            double size = (Width - 152) / tabControl.Items.Count > 200 ? 200 : (double)(Width - 152) / tabControl.Items.Count;
            for (int i = 0; i < tabControl.Items.Count - 1; i++)
            {
                Grid headerGrid = (Grid)webPages[i].closeButton.Parent;
                TabItem tabItem = (TabItem)tabControl.Items[i];
                headerGrid.Width = size;
                tabItem.Width = size;
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeTabs();
        }
    }
}