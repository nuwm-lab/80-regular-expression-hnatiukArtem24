using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("Введіть текст:");
        string input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Текст порожній. Немає що шукати.");
            return;
        }

        // Регулярний вираз для пошуку грошових сум у форматі $123.45 або ₴567,89
        string pattern = @"\b(?<currency>\$|₴)(?<amount>\d+(?:[.,]\d{2})?)\b";

        Regex regex = new Regex(pattern);

        MatchCollection matches = regex.Matches(input);

        if (matches.Count == 0)
        {
            Console.WriteLine("Грошові суми не знайдені.");
        }
        else
        {
            Console.WriteLine("Знайдені грошові суми:");
            foreach (Match match in matches)
            {
                Console.WriteLine(match.Value);
            }
        }
    }
}
