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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CarrierJumpNotification
{
    /// <summary>
    /// Interaction logic for Help.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ChangeUIColor(Color newColor)
        {
            SolidColorBrush newBrush = new SolidColorBrush(newColor);

            foreach (var ctr in this.GetChildren())
            {
                Label lab = ctr as Label;
                if (lab != null)
                {
                    lab.Foreground = newBrush;
                    continue;
                }

                TextBox txt = ctr as TextBox;
                if (txt != null)
                {
                    txt.Foreground = newBrush;
                    txt.BorderBrush = newBrush;
                    txt.SelectionBrush = new SolidColorBrush(ColorMischief.SelectionColor(newColor));
                    continue;
                }

                Button btn = ctr as Button;
                if (btn != null)
                {
                    btn.Background = newBrush;
                    btn.BorderBrush = newBrush;
                    continue;
                }

                RichTextBox rtf = ctr as RichTextBox;
                if (rtf != null)
                {
                    rtf.Foreground = newBrush;
                    rtf.BorderBrush = newBrush;
                    rtf.SelectionBrush = new SolidColorBrush(ColorMischief.SelectionColor(newColor));
                    continue;
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ChangeUIColor((Color)Application.Current.Resources["DynamicUIColor"]);
        }
    }
}
