using Lesson2ModelleringEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopulateDatabase
{
    class Program
    {
        static AppDbContext database;

        static void Main(string[] args)
        {
            // Before running this program, add the files Artists.csv, Albums.csv, and Songs.csv to this project. Also remember to set "Copy Always" on every file.
            using (database = new AppDbContext())
            {
                ClearDatabase();
                PopulateDatabase();
            }
        }

        private static void ClearDatabase()
        {
            database.Song.RemoveRange(database.Song);
            database.Album.RemoveRange(database.Album);
            database.Artist.RemoveRange(database.Artist);
            database.SaveChanges();
        }

        private static void PopulateDatabase()
        {
            var artists = ReadArtists();
            var albums = ReadAlbums(artists);
            var songs = ReadSongs(albums);
            foreach (var song in songs.Values)
            {
                database.Add(song);
                database.SaveChanges();
            }
        }

        private static Dictionary<int, Song> ReadSongs(Dictionary<int, Album> albums)
        {
            var songs = new Dictionary<int, Song>();

            string[] lines = File.ReadAllLines("Songs.csv").Skip(1).ToArray();
            foreach (string line in lines)
            {
                try
                {
                    string[] values = line.Split('|').Select(v => v.Trim()).ToArray();

                    int id = int.Parse(values[0]);
                    byte trackNumber = byte.Parse(values[1]);
                    string title = values[2];

                    string[] lengthParts = values[3].Split(':');
                    int minutes = int.Parse(lengthParts[0]);
                    int seconds = int.Parse(lengthParts[1]);
                    Int16 length = Convert.ToInt16(minutes * 60 + seconds);

                    bool hasMusicVideo;
                    if (values[4].ToUpper() == "Y") hasMusicVideo = true;
                    else if (values[4].ToUpper() == "N") hasMusicVideo = false;
                    else throw new FormatException("Boolean string must be either Y or N.");

                    int albumId = int.Parse(values[5]);

                    // If there are lyrics, add them, otherwise let them be null.
                    string lyrics = null;
                    if (values.Length == 7)
                    {
                        lyrics = values[6];
                    }

                    songs[id] = new Song
                    {
                        TrackNumber = trackNumber,
                        Title = title,
                        Length = length,
                        HasMusicVideo = hasMusicVideo,
                        Lyrics = lyrics,
                        Album = albums[albumId]
                    };
                }
                catch
                {
                    Console.WriteLine("Could not read song: " + line);
                }
            }

            return songs;
        }

        private static Dictionary<int, Album> ReadAlbums(Dictionary<int, Artist> artists)
        {
            var albums = new Dictionary<int, Album>();

            string[] lines = File.ReadAllLines("Albums.csv").Skip(1).ToArray();
            foreach (string line in lines)
            {
                try
                {
                    string[] values = line.Split('|').Select(v => v.Trim()).ToArray();

                    int id = int.Parse(values[0]);
                    string title = values[1];
                    DateTime releaseDate = Convert.ToDateTime(values[2]);
                    int artistId = int.Parse(values[3]);

                    albums[id] = new Album
                    {
                        Title = title,
                        ReleaseDate = releaseDate,
                        Artist = artists[artistId]
                    };
                }
                catch
                {
                    Console.WriteLine("Could not read album: " + line);
                }
            }

            return albums;
        }

        private static Dictionary<int, Artist> ReadArtists()
        {
            var artists = new Dictionary<int, Artist>();

            string[] lines = File.ReadAllLines("Artists.csv").Skip(1).ToArray();
            foreach (string line in lines)
            {
                try
                {
                    string[] values = line.Split('|').Select(v => v.Trim()).ToArray();

                    int id = int.Parse(values[0]);
                    string name = values[1];
                    string country = values[2];
                    Int16 yearStarted = Int16.Parse(values[3]);

                    artists[id] = new Artist
                    {
                        Name = name,
                        Country = country,
                        YearStarted = yearStarted
                    };
                }
                catch
                {
                    Console.WriteLine("Could not read artist: " + line);
                }
            }

            return artists;
        }
    }
}