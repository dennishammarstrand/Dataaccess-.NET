using System;
using System.Collections.Generic;
using System.Text;

namespace Lesson2ModelleringEntity
{
    public class Option<T>
    {
        public string Name { get; }
        public T Value { get; }

        public Option(string name, T value)
        {
            Name = name;
            Value = value;
        }
    }
}
