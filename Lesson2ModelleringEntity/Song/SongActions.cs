using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lesson2ModelleringEntity
{
    class SongActions
    {
        public static void SongMenu()
        {
            Action action = Menu.ShowMenu("Song Options", new Option<Action>[]
            {
                new Option<Action>("List Songs", List),
                new Option<Action>("Add New Song", Add),
                new Option<Action>("List Songs A-Z", Alphabetically),
                new Option<Action>("List Songs By Length, Longest First", ByLength),
                new Option<Action>("List Songs Longer Than Four Minutes", LongerThanFourMinutes),
                new Option<Action>("List Songs Between Three And Four Minutes", BetweenThreeAndFour),
                new Option<Action>("List Songs Not Between Three And Four Minutes", NotBetweenThreeAndFour),
                new Option<Action>("List Songs With More Than 1000 Char Lyric", TextLongerThan1000Chars),
                new Option<Action>("List Songs That Starts With A 'T'", StartsWithT),
                new Option<Action>("List Songs Containing Word Gun", LyricsContainsWordGun),
                new Option<Action>("Return to Main", Menu.MainMenu)
            });
            action();
        }

        static void List()
        {
            if (Program.database.Song.Count() == 0)
            {
                Console.WriteLine("There are no songs in the database.");
            }
            else
            {
                Console.WriteLine("Songs in database:");
                Program.database.Song.ToList().ForEach(x => Console.WriteLine($"- {x.TrackNumber} {x.Title}, " +
                    $"Length: {x.Length / 60:00}:{x.Length % 60:00}," +$" has music video: {(x.HasMusicVideo == true ? "Yes" : "No")}"));
            }
        }

        static void Add()
        {
            ReadInput.WriteUnderlined("Add New Song");
            Album[] albums = Program.database.Album.ToArray();
            Option<Album>[] list = albums.Select(x => new Option<Album>(x.Title, x)).ToArray();

            Song song = new Song
            {
                TrackNumber = ReadInput.Reader<int>("TrackNumber"),
                Title = ReadInput.Reader<string>("Title"),
                Length = (Int16)ReadInput.Reader<int>("Length"),
                HasMusicVideo = ReadInput.Reader<bool>("Has Music Video (Y/N)"),
                Album = Menu.ShowMenu("Album: ", list)
            };
            Program.database.Add(song);
            Program.database.SaveChanges();
        }

        static void Alphabetically()
        {
            Console.WriteLine("Songs A-Z");
            Program.database.Song.OrderBy(s => s.Title)
                .ToList().ForEach(s => Console.WriteLine($"- {s.Title}"));
        }

        static void ByLength()
        {
            Console.WriteLine("By length. Longest first");
            Program.database.Song.OrderByDescending(s => s.Length)
                .ToList().ForEach(s => Console.WriteLine($"- {s.Length} {s.Title}"));
        }

        static void LongerThanFourMinutes()
        {
            Console.WriteLine("Songs longer than four minutes");
            Program.database.Song.Where(s => s.Length > 240).OrderBy(s => s.Length)
                .ToList().ForEach(s => Console.WriteLine($"- {s.Length} {s.Title}"));
        }

        static void BetweenThreeAndFour()
        {
            Console.WriteLine("Songs between three and four minutes");
            Program.database.Song.Where(s => s.Length > 180 && s.Length < 240).OrderBy(s => s.Length)
                .ToList().ForEach(s => Console.WriteLine($"- {s.Length} {s.Title}"));
        }

        static void NotBetweenThreeAndFour()
        {
            Console.WriteLine("Songs not between three and four minutes");
            Program.database.Song.Where(s => s.Length < 180 || s.Length > 240).OrderBy(s => s.Length)
                .ToList().ForEach(s => Console.WriteLine($"- {s.Length} {s.Title}"));
        }

        static void TextLongerThan1000Chars()
        {
            Console.WriteLine("Songs longer than 1000 characters");
            Program.database.Song.Where(s => s.Lyrics.Length > 40)
                .ToList().ForEach(s => Console.WriteLine($"- Title: {s.Title} Lyrics: {s.Lyrics}"));
        }

        static void StartsWithT()
        {
            Console.WriteLine("Songs that start with T");
            Program.database.Song.Where(s => s.Title.StartsWith('T'))
                .ToList().ForEach(s => Console.WriteLine($"- {s.Title}"));
        }

        static void LyricsContainsWordGun()
        {
            Console.WriteLine("Songs with the word gun in them");
            Program.database.Song.Where(s => s.Lyrics.Contains("gun"))
                .ToList().ForEach(s => Console.WriteLine($"- {s.Title}"));
        }
    }
}
