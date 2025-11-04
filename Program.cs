using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        string text = "Мої витрати сьогодні: $123.45 на продукти і ₴567,89 на техніку.";

        string pattern = @"(\$|₴)\d+([.,]\d{2})?";
        MatchCollection matches = Regex.Matches(text, pattern);

        Console.WriteLine("Знайдені грошові суми:");
        foreach (Match match in matches)
        {
            Console.WriteLine(match.Value);
        }

        Console.ReadLine();
    }
}
