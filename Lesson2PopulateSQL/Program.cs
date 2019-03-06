using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson2PopulateSQL
{
    class Program
    {
        static SqlConnection connection;

        static void Main(string[] args)
        {
            // Before running this program, add the files Artists.csv, Albums.csv, and Songs.csv to this project. Also remember to set "Copy Always" on every file.
            connection = new SqlConnection(@"Data Source=(local)\SQLEXPRESS;Initial Catalog=EFMusic;Integrated Security=True");
            connection.Open();

            PopulateSongs();
            PopulateAlbums();
            PopulateArtists();
        }

        private static void PopulateSongs()
        {
            SqlCommand truncateCommand = new SqlCommand("TRUNCATE TABLE Song", connection);
            truncateCommand.ExecuteNonQuery();

            string[] lines = File.ReadAllLines("Songs.csv").Skip(1).ToArray();
            foreach (string  line in lines)
            {
                try
                {
                    string[] values = line.Split('|').Select(x => x.Trim()).ToArray();

                    int id = int.Parse(values[0]);
                    int trackNumber = int.Parse(values[1]);
                    string title = values[2];
                    var time = values[3].Split(':').ToArray();
                    var totalSeconds = (int.Parse(time[0]) * 60) + int.Parse(time[1]);
                    int length = totalSeconds;
                    bool hasMusicVideo;
                    if (values[4] == "Y")
                    {
                        hasMusicVideo = true;
                    }
                    else
                    {
                        hasMusicVideo = false;
                    }
                    int albumId = int.Parse(values[5]);
                    string lyrics = null;
                    if (values.Length == 7)
                    {
                        lyrics = values[6];
                    }

                    SqlCommand command = new SqlCommand(
                        "INSERT INTO Song (TrackNumber, Title, Length, HasMusicVideo, AlbumID, Lyrics)" + 
                        "VALUES (@TrackNumber, @Title, @Length, @HasMusicVideo, @AlbumID, @Lyrics)",
                        connection
                    );
                    command.Parameters.Add(new SqlParameter { ParameterName = "@TrackNumber", Value = trackNumber });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@Title", Value = title });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@Length", Value = length });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@HasMusicVideo", Value = hasMusicVideo });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@AlbumID", Value = albumId });
                    if (lyrics == null)
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@Lyrics", Value = DBNull.Value });
                    }
                    else
                    {
                        command.Parameters.Add(new SqlParameter { ParameterName = "@Lyrics", Value = lyrics });
                    }
                    command.ExecuteNonQuery();
                }
                catch
                {
                    Console.WriteLine("Could not read songs: " + line);
                }
            }
        }

        private static void PopulateAlbums()
        {
            SqlCommand truncateCommand = new SqlCommand("TRUNCATE TABLE Album", connection);
            truncateCommand.ExecuteNonQuery();

            string[] lines = File.ReadAllLines("Albums.csv").Skip(1).ToArray();
            foreach (string line in lines)
            {
                try
                {
                    string[] values = line.Split('|').Select(x => x.Trim()).ToArray();

                    int id = int.Parse(values[0]);
                    string title = values[1];
                    DateTime releaseDate = DateTime.Parse(values[2]);
                    int artistId = int.Parse(values[3]);

                    SqlCommand command = new SqlCommand(
                        "INSERT INTO Album (Title, ReleaseDate, ArtistID) " + 
                        "VALUES (@Title, @ReleaseDate, @ArtistID)",
                        connection
                    );
                    command.Parameters.Add(new SqlParameter { ParameterName = "@Title", Value = title });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@ReleaseDate", Value = releaseDate });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@ArtistID", Value = artistId });
                    command.ExecuteNonQuery();
                }
                catch
                {
                    Console.WriteLine("Could not read albums: " + line);
                }

            }
        }

        private static void PopulateArtists()
        {
            SqlCommand truncateCommand = new SqlCommand("TRUNCATE TABLE Artist", connection);
            truncateCommand.ExecuteNonQuery();

            string[] lines = File.ReadAllLines("Artists.csv").Skip(1).ToArray();
            foreach (string line in lines)
            {
                try
                {
                    string[] values = line.Split('|').Select(v => v.Trim()).ToArray();

                    int id = int.Parse(values[0]);
                    string name = values[1];
                    string country = values[2];
                    int yearStarted = int.Parse(values[3]);

                    SqlCommand command = new SqlCommand(
                        "INSERT INTO Artist (Name, Country, YearStarted) " +
                        "VALUES (@Name, @Country, @YearStarted)",
                        connection
                    );
                    command.Parameters.Add(new SqlParameter { ParameterName = "@Name", Value = name });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@Country", Value = country });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@YearStarted", Value = yearStarted });
                    command.ExecuteNonQuery();
                }
                catch
                {
                    Console.WriteLine("Could not read artist: " + line);
                }
            }
        }
    }
}
