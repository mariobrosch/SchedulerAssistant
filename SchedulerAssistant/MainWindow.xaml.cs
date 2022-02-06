using SchedulerAssistant.Data.Helpers;
using SchedulerAssistant.Helpers;
using System.Collections.Generic;
using System.Windows;

namespace SchedulerAssistant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            _ = new Startup();
            Startup.Perform();

            SettingsHelper.CreateDefaultSettings();
            InitializeComponent();
        }

        private void SettingsToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Windows.Settings settingsForm = new();
            _ = settingsForm.ShowDialog();
            Show();
        }

        private void BtnContacts_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Windows.Contacts contactForm = new();
            _ = contactForm.ShowDialog();
            Show();
        }

        private void BtnEvents_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Windows.Events eventForm = new();
            _ = eventForm.ShowDialog();
            Show();

        }
    }
}
