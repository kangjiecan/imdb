using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Data.Sqlite;

namespace imdb.ViewModels
{
    public class TitleView : INotifyPropertyChanged
    {
        private ObservableCollection<MovieTitle> _movies;
        private string _statusMessage;
        private MovieTitle _selectedMovie;

        public TitleView()
        {
            LoadMoviesCommand = new RelayCommand(LoadMovies);
            Movies = new ObservableCollection<MovieTitle>();
        }

        public ObservableCollection<MovieTitle> Movies
        {
            get => _movies;
            set
            {
                _movies = value;
                OnPropertyChanged();
            }
        }

        public MovieTitle SelectedMovie
        {
            get => _selectedMovie;
            set
            {
                _selectedMovie = value;
                OnPropertyChanged();
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadMoviesCommand { get; }

        public void LoadMovies()
        {
            try
            {
                // Clear existing items
                Movies.Clear();

                // Get database path
                string dbPath = "movie.sqlite";

                if (!File.Exists(dbPath))
                {
                    StatusMessage = $"Database file not found at: {Path.GetFullPath(dbPath)}";
                    return;
                }

                using (var connection = new SqliteConnection($"Data Source={dbPath}"))
                {
                    connection.Open();

                    // Query movies table
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT id, title FROM movies ORDER BY title";

                        using (var reader = command.ExecuteReader())
                        {
                            int count = 0;
                            while (reader.Read())
                            {
                                count++;
                                int id = reader.GetInt32(0);
                                string title = reader.IsDBNull(1) ? "(No Title)" : reader.GetString(1);

                                Movies.Add(new MovieTitle { Id = id, Title = title });
                            }

                            StatusMessage = $"Retrieved {count} movies";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading movies: {ex.Message}";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class MovieTitle
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        public void Execute(object parameter) => _execute();

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}