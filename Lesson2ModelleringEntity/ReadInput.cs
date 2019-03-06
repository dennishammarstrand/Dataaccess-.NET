using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lesson2ModelleringEntity
{
    public class ReadInput
    {
        public static void WriteUnderlined(string line)
        {
            Console.WriteLine(line);
            Console.WriteLine(new String('-', line.Length));
        }

        public static T Reader<T>(string prompt)
        {
            Console.Write(prompt + ": ");
            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(Console.ReadLine(), typeof(T));
            }
            else if (typeof(T) == typeof(int))
            {
                string input = Console.ReadLine();
                int? number = null;
                while (number == null)
                {
                    try
                    {
                        number = int.Parse(input);
                    }
                    catch
                    {
                        Console.Write("Please enter a valid integer: ");
                    }
                }
                return (T)Convert.ChangeType(number, typeof(T));
            }
            else if (typeof(T) == typeof(DateTime))
            {
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
                return (T)Convert.ChangeType(date, typeof(T));
            }
            else if (typeof(T) == typeof(bool))
            {
                bool? value = null;
                while (value == null)
                {
                    string input = Console.ReadLine();
                    if (input.ToUpper() == "Y")
                    {
                        value = true;
                    }
                    else if (input.ToUpper() == "N")
                    {
                        value = false;
                    }
                    else
                    {
                        Console.Write("Please enter either Y or N: ");
                    }
                }
                return (T)Convert.ChangeType(value, typeof(T));
            }
            else throw new InvalidDataException();
        }
    }
}
