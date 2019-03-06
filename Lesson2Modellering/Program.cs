using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson2Modellering
{
    // This class is intended to be used with the following database table:
    // ID: int
    // Name: nvarchar(50)
    // Country: nvarchar(50)
    // YearStarted: smallint
    class Artist
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public int YearStarted { get; set; }
    }

    class Album
    {
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ArtistID { get; set; }
    }

    class Song
    {
        public int TrackNumber { get; set; }
        public string Title { get; set; }
        public int Length { get; set; }
        public bool HasMusicVideo { get; set; }
        public int AlbumID { get; set; }
    }

    class Option
    {
        public string Name { get; }
        public Action Action { get; }

        public Option(string name, Action action)
        {
            Name = name;
            Action = action;
        }
    }

    class Program
    {
        static SqlConnection connection;
        static List<Artist> artists;
        static List<Album> albums;
        static List<Song> songs;

        static void Main(string[] args)
        {
            connection = new SqlConnection(@"Data Source=(local)\SQLEXPRESS;Initial Catalog=Music;Integrated Security=True");
            connection.Open();

            artists = LoadArtistsFromDatabase();
            albums = LoadAlbumsFromDatabase();
            songs = LoadSongsFromDatabase();

            while (true)
            {
                ShowMenu("What do you want to do?", new Option[]
                {
                new Option("List Artists", ListArtists),
                new Option("List Albums", ListAlbums),
                new Option("List Songs", ListSongs),
                new Option("Add New Artist", AddNewArtist),
                new Option("Add New Album", AddNewAlbum),
                new Option("Add New Song", AddNewSong),
                new Option("Quit", () => Environment.Exit(0))
                });
                Console.WriteLine();
            }
        }

        // Use the Console class's features for moving the cursor in order to show a menu that can be controlled be selecting an option with the up and down arrows.
        static void ShowMenu(string prompt, Option[] options)
        {
            Console.WriteLine(prompt);

            int selected = 0;

            // Hide the cursor that will blink after calling ReadKey.
            Console.CursorVisible = false;

            ConsoleKey? key = null;
            while (key != ConsoleKey.Enter)
            {
                // If this is not the first iteration, move the cursor to the first line of the menu.
                if (key != null)
                {
                    Console.CursorLeft = 0;
                    Console.CursorTop = Console.CursorTop - options.Length;
                }

                // Print all the options, highlighting the selected one.
                for (int i = 0; i < options.Length; i++)
                {
                    var option = options[i];
                    if (i == selected)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.WriteLine("- " + option.Name);
                    Console.ResetColor();
                }

                // Read another key and adjust the selected value before looping to repeat all of this.
                key = Console.ReadKey().Key;
                if (key == ConsoleKey.DownArrow)
                {
                    selected = Math.Min(selected + 1, options.Length - 1);
                }
                else if (key == ConsoleKey.UpArrow)
                {
                    selected = Math.Max(selected - 1, 0);
                }
            }

            // Reset the cursor and perform the selected action.
            Console.CursorVisible = true;
            Console.Clear();
            Action action = options[selected].Action;
            action();
        }

        static void WriteUnderlined(string line)
        {
            Console.WriteLine(line);
            Console.WriteLine(new String('-', line.Length));
        }

        static string ReadString(string prompt)
        {
            Console.Write(prompt + ": ");
            return Console.ReadLine();
        }

        static int ReadInt(string prompt)
        {
            Console.Write(prompt + ": ");
            int? number = null;
            while (number == null)
            {
                string input = Console.ReadLine();
                try
                {
                    number = int.Parse(input);
                }
                catch
                {
                    Console.Write("Please enter a valid integer: ");
                }
            }

            return (int)number;
        }

        static DateTime ReadDate(string prompt)
        {
            Console.Write(prompt + ": ");
            DateTime? date = null;
            while (date == null)
            {
                string input = Console.ReadLine();
                try
                {
                    date = DateTime.Parse(input);
                }
                catch
                {
                    Console.Write("Please enter a valid date: ");
                }
            }

            return (DateTime)date;
        }

        static bool ReadBool(string prompt)
        {
            Console.Write(prompt + ": ");
            bool? statement = null;
            while (statement == null)
            {
                string input = Console.ReadLine();
                try
                {
                    statement = bool.Parse(input);
                }
                catch
                {
                    Console.Write("Please enter a valid bool: ");
                }
            }

            return (bool)statement;
        }

        static void ListArtists()
        {
            if (artists.Count == 0)
            {
                Console.WriteLine("There are no artists in the database.");
            }
            else
            {
                Console.WriteLine("Artists in database:");
                foreach (var artist in artists)
                {
                    Console.WriteLine("- " + artist.Name + " (" + artist.Country + ", " + artist.YearStarted + ")");
                }
            }
        }

        static void AddNewArtist()
        {
            WriteUnderlined("Add New Artist");

            Artist artist = new Artist
            {
                Name = ReadString("Name"),
                Country = ReadString("Country"),
                YearStarted = ReadInt("Year Started")
            };
            artists.Add(artist);

            SaveArtistToDatabase(artist);
        }

        static List<Artist> LoadArtistsFromDatabase()
        {
            SqlCommand command = new SqlCommand(
                "SELECT Name, Country, YearStarted FROM Artist",
                connection
            );
            SqlDataReader reader = command.ExecuteReader();

            List<Artist> dbArtists = new List<Artist>();
            while (reader.Read())
            {
                Artist dbArtist = new Artist
                {
                    Name = Convert.ToString(reader["Name"]),
                    Country = Convert.ToString(reader["Country"]),
                    YearStarted = Convert.ToInt32(reader["YearStarted"])
                };
                dbArtists.Add(dbArtist);
            }
            reader.Close();
            return dbArtists;
        }

        static void SaveArtistToDatabase(Artist artist)
        {
            SqlCommand command = new SqlCommand(
                "INSERT INTO Artist (Name, Country, YearStarted) VALUES (@Name, @Country, @YearStarted)",
                connection
            );
            command.Parameters.Add(new SqlParameter { ParameterName = "@Name", Value = artist.Name });
            command.Parameters.Add(new SqlParameter { ParameterName = "@Country", Value = artist.Country });
            command.Parameters.Add(new SqlParameter { ParameterName = "@YearStarted", Value = artist.YearStarted });
            command.ExecuteNonQuery();
        }

        static void ListAlbums()
        {
            if (albums.Count == 0)
            {
                Console.WriteLine("There are no albums in the database.");
            }
            else
            {
                Console.WriteLine("Albums in database:");
                foreach (var album in albums)
                {
                    Console.WriteLine("- " + album.Title + " (" + album.ReleaseDate + ")");
                }
            }
        }

        static void AddNewAlbum()
        {
            WriteUnderlined("Add New Album");

            Album album = new Album
            {
                Title = ReadString("Title"),
                ReleaseDate = ReadDate("Release Date"),
                ArtistID = ReadInt("ArtistID")
            };
            albums.Add(album);

            SaveAlbumToDatabase(album);
        }

        static List<Album> LoadAlbumsFromDatabase()
        {
            SqlCommand command = new SqlCommand(
                "SELECT Title, ReleaseDate, ArtistID FROM Album",
                connection
            );
            SqlDataReader reader = command.ExecuteReader();

            List<Album> dbAlbums = new List<Album>();
            while (reader.Read())
            {
                Album dbAlbum = new Album
                {
                    Title = Convert.ToString(reader["Title"]),
                    ReleaseDate = Convert.ToDateTime(reader["ReleaseDate"]),
                    ArtistID = Convert.ToInt32(reader["ArtistID"])
                };
                dbAlbums.Add(dbAlbum);
            }
            reader.Close();
            return dbAlbums;
        }

        static void SaveAlbumToDatabase(Album album)
        {
            SqlCommand command = new SqlCommand(
                "INSERT INTO Album (Title, ReleaseDate, ArtistID) VALUES (@Title, @ReleaseDate, @ArtistID)",
                connection
            );
            command.Parameters.Add(new SqlParameter { ParameterName = "@Title", Value = album.Title });
            command.Parameters.Add(new SqlParameter { ParameterName = "@ReleaseDate", Value = album.ReleaseDate });
            command.Parameters.Add(new SqlParameter { ParameterName = "@ArtistID", Value = album.ArtistID });
            command.ExecuteNonQuery();
        }

        static void ListSongs()
        {
            if (songs.Count == 0)
            {
                Console.WriteLine("There are no songs in the database.");
            }
            else
            {
                Console.WriteLine("Songs in database:");
                foreach (var song in songs)
                {
                    Console.WriteLine($"- {song.TrackNumber} {song.Title} (Length: {song.Length}) Has music video: {song.HasMusicVideo} ");
                }
            }
        }

        static void AddNewSong()
        {
            WriteUnderlined("Add New Song");

            var song = new Song();
            song.TrackNumber = ReadInt("Track Number");
            song.Title = ReadString("Title");
            Console.WriteLine("Length:");
            int minutes = ReadInt("- Minutes");
            int seconds = ReadInt("- Seconds");
            int totalSeconds = minutes * 60 + seconds;
            song.Length = (Int16)totalSeconds;
            song.HasMusicVideo = ReadBool("Has Music Video (Y/N)");
            //Song song = new Song
            //{
            //    Title = ReadString("Title"),
            //    Length = ReadInt("Length"),
            //    Lyrics = ReadString("Lyrics"),
            //    HasMusicVideo = ReadBool("Has Music Video")
            //};
            songs.Add(song);

            SaveSongToDatabase(song);
        }

        static List<Song> LoadSongsFromDatabase()
        {
            SqlCommand command = new SqlCommand(
                "SELECT TrackNumber, Title, Length, HasMusicVideo FROM Song",
                connection
            );
            SqlDataReader reader = command.ExecuteReader();

            List<Song> dbSongs = new List<Song>();
            while (reader.Read())
            {
                Song dbSong = new Song
                {
                    TrackNumber = Convert.ToInt32(reader["TrackNumber"]),
                    Title = Convert.ToString(reader["Title"]),
                    Length = Convert.ToInt32(reader["Length"]),
                    HasMusicVideo = Convert.ToBoolean(reader["HasMusicVideo"])
                };
                dbSongs.Add(dbSong);
            }
            reader.Close();
            return dbSongs;
        }

        static void SaveSongToDatabase(Song song)
        {
            SqlCommand command = new SqlCommand(
                "INSERT INTO Song (TrackNumber, Title, Length, HasMusicVideo) VALUES (@TrackNumber, @Title, @Length, @HasMusicVideo)",
                connection
            );
            command.Parameters.Add(new SqlParameter { ParameterName = "@TrackNumber", Value = song.TrackNumber });
            command.Parameters.Add(new SqlParameter { ParameterName = "@Title", Value = song.Title });
            command.Parameters.Add(new SqlParameter { ParameterName = "@Length", Value = song.Length });
            command.Parameters.Add(new SqlParameter { ParameterName = "@HasMusicVideo", Value = song.HasMusicVideo });
            command.ExecuteNonQuery();
        }
    }
}
