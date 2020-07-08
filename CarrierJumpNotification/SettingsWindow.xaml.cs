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
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            txtPath.Text = GlobalSettings.EliteFolderPath;
            chkCutSystemName.IsChecked = GlobalSettings.CutColSystem;
            chkExtendedSearch.IsChecked = GlobalSettings.ExtendedSearch;
            chkAutoClipboard.IsChecked = GlobalSettings.AutoClipboard;

            ChangeUIColor((Color)Application.Current.Resources["DynamicUIColor"]);

            UiColorSlider.Maximum = ColorMischief.GradientSize - 1;
            UiColorSlider.Value = GlobalSettings.UiColorIndex;
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.SelectedPath = GlobalSettings.EliteFolderPath;

                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    txtPath.Text = dialog.SelectedPath;
                    GlobalSettings.EliteFolderPath = dialog.SelectedPath;
                }
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void chkCutSystemName_Click(object sender, RoutedEventArgs e)
        {
            GlobalSettings.CutColSystem = (bool)chkCutSystemName.IsChecked;
        }

        public void ChangeUIColor(Color newColor)
        {
            labPath.Foreground = new SolidColorBrush(newColor);

            txtPath.Foreground = new SolidColorBrush(newColor);
            txtPath.BorderBrush = new SolidColorBrush(newColor);
            txtPath.SelectionBrush = new SolidColorBrush(ColorMischief.SelectionColor(newColor));

            btnBrowse.Background = new SolidColorBrush(newColor);
            btnBrowse.BorderBrush = new SolidColorBrush(newColor);

            chkCutSystemName.Background = new SolidColorBrush(newColor);
            chkCutSystemName.BorderBrush = new SolidColorBrush(newColor);
            chkCutSystemName.Foreground = new SolidColorBrush(newColor);

            chkExtendedSearch.Background = new SolidColorBrush(newColor);
            chkExtendedSearch.BorderBrush = new SolidColorBrush(newColor);
            chkExtendedSearch.Foreground = new SolidColorBrush(newColor);

            chkAutoClipboard.Background = new SolidColorBrush(newColor);
            chkAutoClipboard.BorderBrush = new SolidColorBrush(newColor);
            chkAutoClipboard.Foreground = new SolidColorBrush(newColor);

            labUi.Foreground = new SolidColorBrush(newColor);

            UiColorSlider.Foreground = new SolidColorBrush(newColor);

            btnOK.Background = new SolidColorBrush(newColor);
            btnOK.BorderBrush = new SolidColorBrush(newColor);
        }

        private void UiColorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            GlobalSettings.UiColorIndex = UiColorSlider.Value;

            Application.Current.Resources["DynamicUIColor"] = ColorMischief.GetSmoothColor(GlobalSettings.UiColorIndex);

            ChangeUIColor(ColorMischief.GetSmoothColor(GlobalSettings.UiColorIndex));
        }

        private void chkExtendedSearch_Click(object sender, RoutedEventArgs e)
        {
            GlobalSettings.ExtendedSearch = (bool)chkExtendedSearch.IsChecked;
        }

        private void chkAutoClipboard_Click(object sender, RoutedEventArgs e)
        {
            GlobalSettings.AutoClipboard = (bool)chkAutoClipboard.IsChecked;
        }
    }
}
