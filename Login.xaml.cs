using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace browser
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Login : Window
    {
        //Window loginWindow = Application.Current.MainWindow;
        public static string username;
        private void toggleTheme(object sender, RoutedEventArgs e)
        {
            var theme = Resources.GetTheme();
            theme.SetBaseTheme(theme.GetBaseTheme() == BaseTheme.Dark ? Theme.Light : Theme.Dark);
            Resources.SetTheme(theme);
        }
        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            Database DB = new Database("localhost", "browser", "root", "");
            username = txtUsername.Text;
            // Le kell kezelni a szar adatokat
            if (loginBtn.Content.ToString() == "CREATE")
            {    
                if (!DB.userExists(username, "%%"))
                {
                    DB.insertData(username, SHA256Hash(txtPassword.Password));
                    MessageBox.Show("Sikeres regisztráció!", "Regisztráció", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Ilyen nevű felhasználó már létezik!", "Regisztráció", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                if (DB.userExists(username, SHA256Hash(txtPassword.Password)))
                {
                    Window main = new MainWindow();
                    Application.Current.MainWindow.Close();
                    Application.Current.MainWindow = main;
                    main.Show();
                }
                else
                {
                    MessageBox.Show("Ilyen felhasználónév ezzel a jelszóval nem létezik!", "Bejelentkezés", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            //Window main = new MainWindow();
            //Application.Current.MainWindow.Close();
            //Application.Current.MainWindow = main;
            //main.Show();
        }

        private void createAccount_Click(object sender, RoutedEventArgs e)
        {
            signupBtn.Content = signupBtn.Content.ToString() == "Create Account" ? "Sign in" : "Create Account";
            loginBtn.Content = loginBtn.Content.ToString() == "LOG IN" ? "CREATE" : "LOG IN";
            
        }

        private string SHA256Hash(string password)
        {
            using (SHA256 hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
