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
using AudioVisualisation;
using Hurricane.ViewModels;

namespace Hurricane.AppMainWindow.WindowSkins
{
    /// <summary>
    /// Interaction logic for MiniPlayerView.xaml
    /// </summary>
    public partial class MiniPlayerView : IWindowSkin
    {
        public MiniPlayerView()
        {
            InitializeComponent();
            Configuration = new WindowSkinConfiguration
            {
                MaxHeight = double.PositiveInfinity,
                MaxWidth = double.PositiveInfinity,
                MinHeight = 300,
                MinWidth = 300,
                ShowSystemMenuOnRightClick = true,
                ShowTitleBar = false,
                ShowWindowControls = false,
                NeedsMovingHelp = true,
                ShowFullscreenDialogs = true,
                IsResizable = true,
                SupportsCustomBackground = false,
                SupportsMinimizingToTray = true
            };

            SettingsViewModel.Instance.Load();
        }

        void listview_Drop(object sender, DragEventArgs e)
        { }

        void listview_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
        }

        void listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TracksListView.ScrollIntoView(TracksListView.SelectedItem);
        }

        void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (CloseRequest != null)
                CloseRequest(this, EventArgs.Empty);
        }

        void Titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DragMoveStart != null)
                DragMoveStart(this, EventArgs.Empty);
        }

        void Titlebar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DragMoveStop != null)
                DragMoveStop(this, EventArgs.Empty);
        }

        public event EventHandler DragMoveStart;

        public event EventHandler DragMoveStop;

        public event EventHandler CloseRequest;

        public event EventHandler ToggleWindowState;

        public event EventHandler<MouseEventArgs> TitleBarMouseMove;

        public void EnableWindow()
        {
            var visulisation = AudioVisualisationContentControl.Tag as IAudioVisualisation;
            if (visulisation != null)
                visulisation.Enable();
        }

        public void DisableWindow()
        {
            var visulisation = AudioVisualisationContentControl.Tag as IAudioVisualisation;
            if (visulisation != null)
                visulisation.Disable();
        }

        public WindowSkinConfiguration Configuration { get; set; }
    }
}
