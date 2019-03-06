using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lesson2ModelleringEntity
{
    class AlbumActions
    {
        public static void AlbumMenu()
        {
            Action action = Menu.ShowMenu("Album Menu", new Option<Action>[]
            {
                new Option<Action>("List Albums", List),
                new Option<Action>("Add New Album", Add),
                new Option<Action>("List Albums Released After 2000", AlbumsReleasedAfter2000),
                new Option<Action>("List Albums Released In June", AlbumsReleasedInMonth),
                new Option<Action>("Return to Main", Menu.MainMenu)
            });
            action();
        }

        static void List()
        {
            if (Program.database.Album.Count() == 0)
            {
                Console.WriteLine("There are no albums in the database.");
            }
            else
            {
                Console.WriteLine("Albums in database:");
                Program.database.Album.ToList().ForEach(a => Console.WriteLine($"- {a.Title} ({a.ReleaseDate})"));
            }
        }

        static void Add()
        {
            ReadInput.WriteUnderlined("Add New Album");

            Album album = new Album
            {
                Title = ReadInput.Reader<string>("Title"),
                ReleaseDate = ReadInput.Reader<DateTime>("Release Date")
            };
            Program.database.Add(album);
            Program.database.SaveChanges();
        }

        static void AlbumsReleasedAfter2000()
        {
            Console.WriteLine("Albums released after year 2000");
            Program.database.Album.Where(a => a.ReleaseDate > new DateTime(2000, 1, 1))
                .ToList().ForEach(a => Console.WriteLine($"- {a.Title} {a.ReleaseDate}"));
        }

        static void AlbumsReleasedInMonth()
        {
            Console.WriteLine("Albums released in june");
            Program.database.Album.Where(a => a.ReleaseDate.Month == 6)
                .ToList().ForEach(a => Console.WriteLine($"- {a.Title} {a.ReleaseDate}"));
        }
    }
}
