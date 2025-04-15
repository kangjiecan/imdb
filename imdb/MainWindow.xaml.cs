using System;
using System.Windows;
using imdb.ViewModels;

namespace imdb
{
    public partial class MainWindow : Window
    {
        private TitleView _titleViewModel;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get ViewModel reference from resources
                _titleViewModel = (TitleView)FindResource("TitleVM");

                // Set window title with version information
                this.Title = $"IMDb Movie Database - {GetVersionInfo()}";

                // Load initial data
                _titleViewModel.LoadMovies();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing application: {ex.Message}",
                    "Initialization Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetVersionInfo()
        {
            try
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                return $"v{version.Major}.{version.Minor}.{version.Build}";
            }
            catch
            {
                return "v1.0";
            }
        }
    }
}