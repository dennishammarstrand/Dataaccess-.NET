using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lesson2ModelleringEntity
{
    class ArtistActions
    {
        public static void ArtistMenu()
        {
            Action action = Menu.ShowMenu("Artist Menu", new Option<Action>[]
            {
                new Option<Action>("List Artists", List),
                new Option<Action>("Add New Artist", Add),
                new Option<Action>("List Artists From Sweden", ArtistsFromSweden),
                new Option<Action>("List Artists Not From Sweden", ArtistsNotFromSweden),
                new Option<Action>("List Artists From USA Or Canada", AristsFromUSAOrCanada),
                new Option<Action>("List Artists Years As Active", AgeOfAnArtist),
                new Option<Action>("List Artists By Country", ListArtistsByCountry),
                new Option<Action>("List Artists By Country By List", ListArtistsByCountrySelectList),
                new Option<Action>("Return to Main", Menu.MainMenu)
            });
            action();
        }

        static void List()
        {
            if (Program.database.Artist.Count() == 0)
            {
                Console.WriteLine("There are no artists in the database.");
            }
            else
            {
                Console.WriteLine("Artists in database:");
                Program.database.Artist.ToList().ForEach(a => Console.WriteLine($"- {a.Name} ({a.Country}, {a.YearStarted})"));
            }
        }

        static void Add()
        {
            ReadInput.WriteUnderlined("Add New Artist");

            Artist artist = new Artist
            {
                Name = ReadInput.Reader<string>("Name"),
                Country = ReadInput.Reader<string>("Country"),
                YearStarted = (Int16)ReadInput.Reader<int>("Year Started")
            };
            Program.database.Add(artist);
            Program.database.SaveChanges();
        }

        static void ArtistsFromSweden()
        {
            Console.WriteLine("Artists from sweden");
            Program.database.Artist.Where(a => a.Country == "Sweden")
                .ToList().ForEach(a => Console.WriteLine($"- {a.Name}"));
        }

        static void ArtistsNotFromSweden()
        {
            Console.WriteLine("Artists not from sweden");
            Program.database.Artist.Where(a => a.Country != "Sweden")
                .ToList().ForEach(a => Console.WriteLine($"- {a.Name} : {a.Country}"));
        }

        static void AristsFromUSAOrCanada()
        {
            Console.WriteLine("Artists from USA or Canada");
            Program.database.Artist.Where(a => a.Country == "USA" || a.Country == "Canada")
                .ToList().ForEach(a => Console.WriteLine($"- {a.Name} : {a.Country}"));
        }

        static void AgeOfAnArtist()
        {
            Console.WriteLine("Artists have been active for");
            Program.database.Artist.ToList().ForEach(a => Console.WriteLine($"- {a.Name} : {DateTime.Today.Year - a.YearStarted} Years"));
        }

        static void ListArtistsByCountry()
        {
            Console.WriteLine("List Artists By Country");
            string country = ReadInput.Reader<string>("Please enter a country: ");
            Console.WriteLine($"Artist from {country}");
            Program.database.Artist.Where(a => a.Country == country).ToList().ForEach(a => Console.WriteLine($"- {a.Name}"));
        }

        static void ListArtistsByCountrySelectList()
        {
            string[] countrys = Program.database.Artist.Select(a => a.Country).Distinct().ToArray();
            Option<string>[] list = countrys.Select(x => new Option<string>(x, x)).ToArray();
            string option = Menu.ShowMenu("Please select country", list);
            Console.WriteLine($"Artist from {option}");
            Program.database.Artist.Where(a => a.Country == option).ToList().ForEach(a => Console.WriteLine($"- {a.Name}"));
        }
    }
}
