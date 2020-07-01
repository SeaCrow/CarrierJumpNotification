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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Text.RegularExpressions;

using Microsoft.Win32;

namespace CarrierJumpNotification
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string configPath = "config.dat";

        public MainWindow()
        {
            InitializeComponent();

            creditsLabel.Content += "  v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, 4);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Style style = new Style { TargetType = typeof(Paragraph) };
            style.Setters.Add(new Setter(Paragraph.MarginProperty, new Thickness(0)));
            Resources.Add(typeof(Paragraph), style);

            ExampleData();

            if (File.Exists(configPath))
            {
                GlobalSettings.InitFromFile(configPath);
            }
            else
            {
                GlobalSettings.InitDefault();
            }

            NotificationPattern.Document.Blocks.Clear();
            NotificationPattern.Document.Blocks.Add(new Paragraph(new Run(GlobalSettings.NotificationPattern)));

            GenerateNotification();
        }

        private void NotificationResult_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string notification = new TextRange(NotificationResult.Document.ContentStart, NotificationResult.Document.ContentEnd).Text;
            Clipboard.SetText(notification);
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow hw = new HelpWindow();
            hw.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string pattern = new TextRange(NotificationPattern.Document.ContentStart, NotificationPattern.Document.ContentEnd).Text;

            GlobalSettings.NotificationPattern = pattern;

            GlobalSettings.SaveSettings(configPath);
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            GenerateNotification();
        }

        private void GenerateNotification()
        {
            string pattern = new TextRange(NotificationPattern.Document.ContentStart, NotificationPattern.Document.ContentEnd).Text;

            pattern = pattern.Replace("<fc_name>", txtCarrierName.Text);

            pattern = pattern.Replace("<fc_id>", txtCarrierID.Text);

            if (!GlobalSettings.CutColSystem)
            {
                pattern = pattern.Replace("<target_system>", txtTargetSystem.Text);

                pattern = pattern.Replace("<current_system>", txtSourceSystem.Text);
            }else
            {
                Match Col = Regex.Match(txtTargetSystem.Text, @"COL\s\d{1,}\sSector\s");
                if(Col.Success)
                {
                    string tmpSystem = txtTargetSystem.Text.Replace(Col.Value, "");
                    pattern = pattern.Replace("<target_system>", tmpSystem);
                }

                Col = Regex.Match(txtSourceSystem.Text, @"COL\s\d{1,}\sSector\s");
                if (Col.Success)
                {
                    string tmpSystem = txtSourceSystem.Text.Replace(Col.Value, "");
                    pattern = pattern.Replace("<current_system>", tmpSystem);
                }
            }

            string toJump = string.Empty;
            string toLockdown = string.Empty;


            DateTime StartTime;

            if (DateTime.TryParse(txtStartTime.Text, out StartTime))
            {
                DateTime JumpTime = StartTime.AddMinutes(15);
                DateTime LockdownTime = JumpTime.AddSeconds(-200);

                int jumpMinutes = (int)(JumpTime - DateTime.UtcNow).TotalMinutes;
                int lockdownMinutes = (int)(LockdownTime - DateTime.UtcNow).TotalMinutes;

                if (jumpMinutes < 0)
                    jumpMinutes = 0;

                if (lockdownMinutes < 0)
                    lockdownMinutes = 0;

                toJump = jumpMinutes.ToString();
                toLockdown = lockdownMinutes.ToString();
            }

            pattern = pattern.Replace("<jump_time>", toJump);

            pattern = pattern.Replace("<lockdown_time>", toLockdown);

            NotificationResult.Document.Blocks.Clear();
            NotificationResult.Document.Blocks.Add(new Paragraph(new Run(pattern)));

        }

        private void ExampleData()
        {
            txtCarrierName.Text = "Fleet Carrier";
            txtCarrierID.Text = "AA-001";
            txtTargetSystem.Text = "Sol";
            txtSourceSystem.Text = "Shinrarta Dezhra";
            txtStartTime.Text = DateTime.UtcNow.ToString("s");
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow config = new SettingsWindow();
            config.ShowDialog();
        }

        private void PullData_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(GlobalSettings.EliteFolderPath))
                return;

            var directory = new DirectoryInfo(GlobalSettings.EliteFolderPath);
            var filename = (from f in directory.GetFiles("*.log")
                            orderby f.LastWriteTime descending
                            select f 
                            ).First().FullName;

            CarrierJumpData latestJump = EliteLogParser.PullFromLog(filename);

            if (latestJump == null)
                return;

            txtStartTime.Text = latestJump.JumpSequenceStart.ToString("s");
            txtCarrierID.Text = latestJump.Callsign;
            txtCarrierName.Text = latestJump.Name;
            txtTargetSystem.Text = latestJump.TargetSystem;
            if(latestJump.CurrentSystem != null && latestJump.CurrentSystem != string.Empty)
            {
                txtSourceSystem.Text = latestJump.CurrentSystem;
                
            }

            GenerateNotification();
        }
    }
}
