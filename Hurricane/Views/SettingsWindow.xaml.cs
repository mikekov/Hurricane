using System.Windows;
using Hurricane.ViewModels;

namespace Hurricane.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : MahApps.Metro.Controls.MetroWindow
    {
        public SettingsWindow()
        {
            InitializeComponent();
            Loaded += SettingsWindow_Loaded;
        }

        void SettingsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SettingsViewModel.Instance.Load();
        }
    }
}
