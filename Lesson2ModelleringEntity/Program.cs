using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lesson2ModelleringEntity
{
    class Program
    {
        public static AppDbContext database;

        static void Main(string[] args)
        {
            using (database = new AppDbContext())
            {
                Task startup = AppDbContext.LoadDbOnStart();
                while (true)
                {
                    Menu.MainMenu();
                }
            }
        }
    }
}