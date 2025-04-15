using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace imdb.Database
{
    public static class DbCreator
    {
        public static bool CreateNewDatabase(string path)
        {
            try
            {
                // Delete the file if it already exists
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                // Create a new database
                using (var connection = new SqliteConnection($"Data Source={path}"))
                {
                    connection.Open();

                    // Create directors table
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                            CREATE TABLE directors (
                                id INTEGER PRIMARY KEY,
                                name TEXT,
                                gender INTEGER,
                                department TEXT,
                                uid INTEGER
                            )";
                        command.ExecuteNonQuery();
                    }

                    // Create movies table
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                            CREATE TABLE movies (
                                id INTEGER PRIMARY KEY,
                                original_title VARCHAR,
                                budget INTEGER,
                                popularity INTEGER,
                                release_date TEXT,
                                revenue INTEGER,
                                title TEXT,
                                vote_average REAL,
                                vote_count INTEGER,
                                overview TEXT,
                                tagline TEXT,
                                uid INTEGER,
                                director_id INTEGER
                            )";
                        command.ExecuteNonQuery();
                    }

                    // Add sample data
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                            INSERT INTO movies (id, title, original_title, release_date, vote_average, vote_count, budget, revenue, overview, tagline)
                            VALUES 
                            (1, 'The Shawshank Redemption', 'The Shawshank Redemption', '1994-09-23', 9.3, 2400000, 25000000, 58300000, 'Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.', 'Fear can hold you prisoner. Hope can set you free.'),
                            (2, 'The Godfather', 'The Godfather', '1972-03-24', 9.2, 1800000, 6000000, 245000000, 'The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.', 'An offer you can''t refuse.'),
                            (3, 'The Dark Knight', 'The Dark Knight', '2008-07-18', 9.0, 2500000, 185000000, 1005000000, 'When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman must accept one of the greatest psychological and physical tests of his ability to fight injustice.', 'Why so serious?')";
                        command.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating database: {ex.Message}");
                return false;
            }
        }
    }
}