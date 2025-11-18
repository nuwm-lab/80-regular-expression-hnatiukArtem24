using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Console.WriteLine("Введіть текст:");
        string input = Console.ReadLine();

 
        string pattern = @"[\(\[](\d+)[\)\]]";

        MatchCollection matches = Regex.Matches(input, pattern);

        Console.WriteLine("\nЗнайдені числа в дужках:");
        foreach (Match match in matches)
        {
  
            Console.WriteLine(match.Groups[1].Value);
        }

        Console.WriteLine($"\nВсього знайдено: {matches.Count}");
    }
}
