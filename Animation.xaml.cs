using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace browser.Pages
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Animation : Page
    {
        bool movingRight = true;
        Random rnd = new Random();
        double distance;

        public Animation()
        {
            InitializeComponent();

            distance = ((MainWindow)Application.Current.MainWindow).Width - SharkImage.Width - 20;
            StartSwimming();
            StartVerticalSwimming();
        }

        void StartSwimming()
        {
            DoubleAnimation xAnim = new DoubleAnimation
            {
                From = movingRight ? 0 : distance,
                To = movingRight ? distance : 0,
                Duration = TimeSpan.FromSeconds(5),
                FillBehavior = FillBehavior.Stop
            };

            xAnim.Completed += (s, e) =>
            {
                movingRight = !movingRight;

                // tükrözés (kép alapból balra néz)
                FlipTransform.ScaleX = movingRight ? -1 : 1;

                StartSwimming();
            };

            MoveTransform.BeginAnimation(TranslateTransform.XProperty, xAnim);
        }

        void StartVerticalSwimming()
        {
            double newY = rnd.Next(50, 180);
            double angle = rnd.Next(-15, 15);

            DoubleAnimation yAnim = new DoubleAnimation
            {
                To = newY,
                Duration = TimeSpan.FromSeconds(3),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };

            DoubleAnimation rotateAnim = new DoubleAnimation
            {
                To = angle,
                Duration = TimeSpan.FromSeconds(1),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };

            MoveTransform.BeginAnimation(TranslateTransform.YProperty, yAnim);
            RotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotateAnim);
        }
    }
}
