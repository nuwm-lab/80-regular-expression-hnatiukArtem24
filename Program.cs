using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Console.WriteLine("Введіть текст:");
        string input = Console.ReadLine();

        // Регулярка: знайти число всередині (123) або [456]
        string pattern = @"[\(\[](\d+)[\)\]]";

        MatchCollection matches = Regex.Matches(input, pattern);

        Console.WriteLine("\nЗнайдені числа в дужках:");
        foreach (Match match in matches)
        {
            // match.Groups[1] — це саме число
            Console.WriteLine(match.Groups[1].Value);
        }

        Console.WriteLine($"\nВсього знайдено: {matches.Count}");
    }
}
