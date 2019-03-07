using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson2ModelleringEntity
{
    class Menu
    {
        public static void MainMenu()
        {
            Action action = ShowMenu("What do you want to do?", new Option<Action>[]
            {
                new Option<Action>("Show Artist Menu", ArtistActions.ArtistMenu),
                new Option<Action>("Show Album Menu", AlbumActions.AlbumMenu),
                new Option<Action>("Show Song Menu", SongActions.SongMenu),
                new Option<Action>("Quit", () => Environment.Exit(0))
            });
            action();
            Console.WriteLine();
        }

        // Use the Console class's features for moving the cursor in order to show a menu that can be controlled be selecting an option with the up and down arrows.
        public static T ShowMenu<T>(string prompt, Option<T>[] options)
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
            return options[selected].Value;
        }

        // Show a menu like with ShowMenu, but allow the user to select multiple options by toggling them with Space.
        // All of the selected options will be returned as an array of strings once the user has pressed Enter.
        public static T ShowMultiMenu<T>(string prompt, Option<T>[] options)
        {
            Console.WriteLine(prompt);

            var selected = new List<int>();
            int focused = 0;

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

                // Print all the options, highlighting the focused one and the selected ones.
                for (int i = 0; i < options.Length; i++)
                {
                    var option = options[i];
                    if (i == focused)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (selected.Contains(i))
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    if (selected.Contains(i)) Console.Write("+");
                    else Console.Write("-");
                    Console.WriteLine(" " + option.Name);

                    Console.ResetColor();
                }

                // Read another key and adjust the selected value before looping to repeat all of this.
                key = Console.ReadKey().Key;
                if (key == ConsoleKey.DownArrow)
                {
                    focused = Math.Min(focused + 1, options.Length - 1);
                }
                else if (key == ConsoleKey.UpArrow)
                {
                    focused = Math.Max(focused - 1, 0);
                }
                else if (key == ConsoleKey.Spacebar)
                {
                    if (selected.Contains(focused))
                    {
                        selected.Remove(focused);
                    }
                    else
                    {
                        selected.Add(focused);
                    }
                }
            }

            // Reset the cursor and return the selected options.
            Console.CursorVisible = true;

            // For consistency and predictability, sort selected indexes so that returned strings are in order shown in menu.
            selected.Sort();
            var selectedStrings = new List<Option<T>>();
            foreach (int i in selected)
            {
                selectedStrings.Add(options[i]);
            }
            return (T)Convert.ChangeType(selectedStrings, typeof(T));
        }
    }
}
