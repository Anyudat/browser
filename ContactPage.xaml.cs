using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace browser.Pages
{
    public partial class ContactPage : Page
    {
        private Button _selectedButton;

        public ContactPage()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            // Korábbi kiválasztott gomb underline eltüntetése
            if (_selectedButton != null && _selectedButton != btn)
            {
                var rect = _selectedButton.Template.FindName("Underline", _selectedButton) as Rectangle;
                if (rect != null)
                    rect.Width = 0;
            }

            _selectedButton = btn;

            // Kiválasztott gomb aláhúzása
            var underline = btn.Template.FindName("Underline", btn) as Rectangle;
            if (underline != null)
                underline.Width = 80;

            // Tartalom betöltése a ContentPanel-be
            ShowContent(btn.Tag.ToString());
        }

        private void ShowContent(string tag)
        {
            ContentPanel.Children.Clear();

            switch (tag)
            {
                case "Version":
                    // Verzió Header
                    ContentPanel.Children.Add(new TextBlock
                    {
                        Text = "Version 1.1.2",
                        FontSize = 20,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 0, 0, 12)
                    });

                    // Release date
                    ContentPanel.Children.Add(new TextBlock
                    {
                        Text = "Release Date: 2026-01-06",
                        FontSize = 16,
                        Margin = new Thickness(0, 0, 0, 8)
                    });

                    // Release type
                    ContentPanel.Children.Add(new TextBlock
                    {
                        Text = "Release Type: Stable",
                        FontSize = 16,
                        Margin = new Thickness(0, 0, 0, 12)
                    });

                    // Changelog / újdonságok
                    var changelog = new StackPanel();
                    changelog.Children.Add(new TextBlock { Text = "Changelog:", FontWeight = FontWeights.Bold, FontSize = 16, Margin = new Thickness(0, 0, 0, 8) });
                    changelog.Children.Add(new TextBlock { Text = "• Improved performance on homepage rendering", FontSize = 16, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(12, 0, 0, 4) });
                    changelog.Children.Add(new TextBlock { Text = "• Fixed login button color issue", FontSize = 16, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(12, 0, 0, 4) });
                    changelog.Children.Add(new TextBlock { Text = "• Added Contact Card page", FontSize = 16, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(12, 0, 0, 4) });
                    ContentPanel.Children.Add(changelog);

                    break;

                case "Developers":
                    ContentPanel.Children.Clear();

                    // Header
                    ContentPanel.Children.Add(new TextBlock
                    {
                        Text = "Developers",
                        FontSize = 20,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 0, 0, 12)
                    });

                    // Fejlesztő lista
                    var devStack = new StackPanel();

                    // ==================== Developer1 ====================
                    var developer1Border = new Border
                    {
                        CornerRadius = new CornerRadius(8),
                        Background = Brushes.WhiteSmoke,
                        Padding = new Thickness(16),
                        Margin = new Thickness(0, 0, 0, 16)
                    };
                    var developer1Panel = new StackPanel();

                    // 1. Név
                    developer1Panel.Children.Add(new TextBlock
                    {
                        Text = "György Zoltán Szilárd",
                        FontWeight = FontWeights.Bold,
                        FontSize = 20,
                        Margin = new Thickness(0, 0, 0, 8)
                    });

                    // 2. Pozíció / munkakör
                    developer1Panel.Children.Add(new TextBlock
                    {
                        Text = "Software Developer",
                        FontSize = 16,
                        FontStyle = FontStyles.Italic,
                        Margin = new Thickness(0, 0, 0, 4)
                    });

                    // 3. Konkrét szerep + rövid leírás
                    developer1Panel.Children.Add(new TextBlock
                    {
                        Text = "Responsible for front-end UI design, implementing interactive components, and optimizing user experience.",
                        FontSize = 14,
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 0, 0, 12)
                    });

                    // 4. Elérhetőségek
                    var contacts1 = new StackPanel();

                    // Email
                    var email1 = new TextBlock { FontSize = 14, Margin = new Thickness(0, 2, 0, 0) };
                    var emailLink1 = new Hyperlink(new Run("zoltan.szilard10@gmail.com")) { NavigateUri = new Uri("mailto:zoltan.szilard10@gmail.com") };
                    emailLink1.RequestNavigate += (s, e) => System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
                    email1.Inlines.Add(new Run("Email: ") { FontWeight = FontWeights.Bold });
                    email1.Inlines.Add(emailLink1);
                    contacts1.Children.Add(email1);

                    // Teams
                    var teams1 = new TextBlock { FontSize = 14, Margin = new Thickness(0, 2, 0, 0) };
                    var teamsLink1 = new Hyperlink(new Run("gyorgy.zoltan.szilard2007@tanulo.boronkay.hu")) { NavigateUri = new Uri("mailto:gyorgy.zoltan.szilard2007@tanulo.boronkay.hu") };
                    teamsLink1.RequestNavigate += (s, e) => System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
                    teams1.Inlines.Add(new Run("Teams: ") { FontWeight = FontWeights.Bold });
                    teams1.Inlines.Add(teamsLink1);
                    contacts1.Children.Add(teams1);

                    // GitHub
                    var github1 = new TextBlock { FontSize = 14, Margin = new Thickness(0, 2, 0, 0) };
                    var githubLink1 = new Hyperlink(new Run("https://github.com/Necebev")) { NavigateUri = new Uri("https://github.com/Necebev") };
                    githubLink1.RequestNavigate += (s, e) => System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
                    github1.Inlines.Add(new Run("GitHub: ") { FontWeight = FontWeights.Bold });
                    github1.Inlines.Add(githubLink1);
                    contacts1.Children.Add(github1);

                    // Hozzáadás
                    developer1Panel.Children.Add(contacts1);
                    developer1Border.Child = developer1Panel;
                    devStack.Children.Add(developer1Border);

                    // ==================== Developer2 ====================
                    var developer2Border = new Border
                    {
                        CornerRadius = new CornerRadius(8),
                        Background = Brushes.WhiteSmoke,
                        Padding = new Thickness(16),
                        Margin = new Thickness(0, 0, 0, 16)
                    };
                    var developer2Panel = new StackPanel();

                    // Név
                    developer2Panel.Children.Add(new TextBlock
                    {
                        Text = "Bayer Bálint",
                        FontWeight = FontWeights.Bold,
                        FontSize = 20,
                        Margin = new Thickness(0, 0, 0, 8)
                    });

                    // Pozíció / munkakör
                    developer2Panel.Children.Add(new TextBlock
                    {
                        Text = "Back-end Developer",
                        FontSize = 16,
                        FontStyle = FontStyles.Italic,
                        Margin = new Thickness(0, 0, 0, 4)
                    });

                    // Konkrét szerep + leírás
                    developer2Panel.Children.Add(new TextBlock
                    {
                        Text = "Handles database architecture, server-side logic, and API integration.",
                        FontSize = 14,
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 0, 0, 12)
                    });

                    // Elérhetőségek
                    var contacts2 = new StackPanel();

                    // Email
                    var email2 = new TextBlock { FontSize = 14, Margin = new Thickness(0, 2, 0, 0) };
                    var emailLink2 = new Hyperlink(new Run("balint22222@gmail.com")) { NavigateUri = new Uri("mailto:developer2@example.com") };
                    emailLink2.RequestNavigate += (s, e) => System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
                    email2.Inlines.Add(new Run("Email: ") { FontWeight = FontWeights.Bold });
                    email2.Inlines.Add(emailLink2);
                    contacts2.Children.Add(email2);

                    // Teams
                    var teams2 = new TextBlock { FontSize = 14, Margin = new Thickness(0, 2, 0, 0) };
                    var teamsLink2 = new Hyperlink(new Run("bayer.balint2008@tanulo.boronkay.hu")) { NavigateUri = new Uri("mailto:bayer.balint2008@tanulo.boronkay.hu") };
                    teamsLink2.RequestNavigate += (s, e) => System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
                    teams2.Inlines.Add(new Run("Teams: ") { FontWeight = FontWeights.Bold });
                    teams2.Inlines.Add(teamsLink2);
                    contacts2.Children.Add(teams2);

                    // GitHub
                    var github2 = new TextBlock { FontSize = 14, Margin = new Thickness(0, 2, 0, 0) };
                    var githubLink2 = new Hyperlink(new Run("https://github.com/bayerbalint")) { NavigateUri = new Uri("https://github.com/bayerbalint") };
                    githubLink2.RequestNavigate += (s, e) => System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
                    github2.Inlines.Add(new Run("GitHub: ") { FontWeight = FontWeights.Bold });
                    github2.Inlines.Add(githubLink2);
                    contacts2.Children.Add(github2);

                    developer2Panel.Children.Add(contacts2);
                    developer2Border.Child = developer2Panel;
                    devStack.Children.Add(developer2Border);

                    // ==================== Developer3 ====================
                    var developer3Border = new Border
                    {
                        CornerRadius = new CornerRadius(8),
                        Background = Brushes.WhiteSmoke,
                        Padding = new Thickness(16),
                        Margin = new Thickness(0, 0, 0, 16)
                    };
                    var developer3Panel = new StackPanel();

                    // Név
                    developer3Panel.Children.Add(new TextBlock
                    {
                        Text = "Hajtó Lili",
                        FontWeight = FontWeights.Bold,
                        FontSize = 20,
                        Margin = new Thickness(0, 0, 0, 8)
                    });

                    // Pozíció / munkakör
                    developer3Panel.Children.Add(new TextBlock
                    {
                        Text = "Project Manager & QA",
                        FontSize = 16,
                        FontStyle = FontStyles.Italic,
                        Margin = new Thickness(0, 0, 0, 4)
                    });

                    // Konkrét szerep + leírás
                    developer3Panel.Children.Add(new TextBlock
                    {
                        Text = "Coordinates the team, manages tasks, and ensures quality testing of the application.",
                        FontSize = 14,
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 0, 0, 12)
                    });

                    // Elérhetőségek
                    var contacts3 = new StackPanel();

                    // Email
                    var email3 = new TextBlock { FontSize = 14, Margin = new Thickness(0, 2, 0, 0) };
                    var emailLink3 = new Hyperlink(new Run("lilihajto@gmail.com")) { NavigateUri = new Uri("mailto:lilihajto@gmail.com") };
                    emailLink3.RequestNavigate += (s, e) => System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
                    email3.Inlines.Add(new Run("Email: ") { FontWeight = FontWeights.Bold });
                    email3.Inlines.Add(emailLink3);
                    contacts3.Children.Add(email3);

                    // Teams
                    var teams3 = new TextBlock { FontSize = 14, Margin = new Thickness(0, 2, 0, 0) };
                    var teamsLink3 = new Hyperlink(new Run("hajto.lili2007@tanulo.boronkay.hu")) { NavigateUri = new Uri("mailto:hajto.lili2007@tanulo.boronkay.hu") };
                    teamsLink3.RequestNavigate += (s, e) => System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
                    teams3.Inlines.Add(new Run("Teams: ") { FontWeight = FontWeights.Bold });
                    teams3.Inlines.Add(teamsLink3);
                    contacts3.Children.Add(teams3);

                    // GitHub
                    var github3 = new TextBlock { FontSize = 14, Margin = new Thickness(0, 2, 0, 0) };
                    var githubLink3 = new Hyperlink(new Run("https://github.com/lilih-coder")) { NavigateUri = new Uri("https://github.com/lilih-coder") };
                    githubLink3.RequestNavigate += (s, e) => System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
                    github3.Inlines.Add(new Run("GitHub: ") { FontWeight = FontWeights.Bold });
                    github3.Inlines.Add(githubLink3);
                    contacts3.Children.Add(github3);

                    developer3Panel.Children.Add(contacts3);
                    developer3Border.Child = developer3Panel;
                    devStack.Children.Add(developer3Border);

                    // Hozzáadás a ContentPanel-hez
                    ContentPanel.Children.Add(devStack);

                    break;


                case "Project":
                    ContentPanel.Children.Add(new TextBlock { Text = "Project Name: XY Browser 2025\nDescription: Modern lightweight browser", FontSize = 16, TextWrapping = TextWrapping.Wrap });
                    break;

                case "Tools":
                    var toolsBorder = new Border
                    {
                        CornerRadius = new CornerRadius(8),
                        Background = Brushes.WhiteSmoke,
                        Padding = new Thickness(16),
                        Margin = new Thickness(0, 0, 0, 16)
                    };
                    var toolsPanel = new StackPanel();

                    // Név
                    toolsPanel.Children.Add(new TextBlock
                    {
                        Text = "LANGUAGES",
                        FontWeight = FontWeights.Bold,
                        FontSize = 20,
                        Margin = new Thickness(0, 0, 0, 8)
                    });

                    // Pozíció / munkakör
                    toolsPanel.Children.Add(new TextBlock
                    {
                        Text = "C#",
                        FontSize = 16,
                        FontWeight= FontWeights.Bold,
                        Margin = new Thickness(0, 0, 0, 4)
                    });
                    // Név
                    toolsPanel.Children.Add(new TextBlock
                    {
                        Text = "DATABASE",
                        FontWeight = FontWeights.Bold,
                        FontSize = 20,
                        Margin = new Thickness(0, 0, 0, 8)
                    });

                    // Pozíció / munkakör
                    toolsPanel.Children.Add(new TextBlock
                    {
                        Text = "SQL",
                        FontSize = 16,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 0, 0, 4)
                    });


                    toolsBorder.Child = toolsPanel;
                    ContentPanel.Children.Add(toolsBorder);
                    break;
                case "About Us":
                    ContentPanel.Children.Clear();

                    // Header
                    ContentPanel.Children.Add(new TextBlock
                    {
                        Text = "About Us",
                        FontSize = 20,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 0, 0, 12)
                    });

                    // Paragraph 1
                    ContentPanel.Children.Add(new TextBlock
                    {
                        Text = "We are a team of passionate software development students, now in our fourth year of learning and growing together. Over the past few years, we have collaborated on multiple projects, and this is our third major joint endeavor.",
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 0, 0, 8)
                    });

                    // Paragraph 2
                    ContentPanel.Children.Add(new TextBlock
                    {
                        Text = "Our strength lies in the diversity of our skills. Each member brings unique expertise to the table, and together we complement each other perfectly. This synergy allows us to tackle challenges effectively and produce high-quality solutions.",
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 0, 0, 8)
                    });

                    // Paragraph 3
                    ContentPanel.Children.Add(new TextBlock
                    {
                        Text = "This project has been an exciting journey full of challenges and learning opportunities. Working on it has not only strengthened our technical skills but also reinforced our ability to collaborate, solve problems creatively, and adapt to new technologies.",
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 0, 0, 8)
                    });

                    // Paragraph 4
                    ContentPanel.Children.Add(new TextBlock
                    {
                        Text = "We are driven by curiosity and the desire to create software that matters. Every project we take on is a chance to push our limits, learn something new, and grow as a team.",
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 0, 0, 8)
                    });
                    break;

            }
        }
    }
}
