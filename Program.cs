using System;
using System.Text.RegularExpressions;


namespace LabWork
{
    public class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("\nВведіть текст для пошуку грошових сум (або натисніть Enter для прикладу):");
                string input = Console.ReadLine();


                if (string.IsNullOrWhiteSpace(input))
                {
                    input = "Приклад сум: $1,234.56 та $50.00, також $123.45 і $1,000,000.00, неправильні формати: 123.456, 12.34";
                    Console.WriteLine($"\nВикористовуємо тестовий текст:\n{input}");
                }


                // Патерн для пошуку грошових сум
                string pattern = @"\$\d{1,3}(,\d{3})*\.\d{2}(?!\d)";
               
                try
                {
                    // Створюємо об'єкт Regex з опцією Compiled для кращої продуктивності
                    var regex = new Regex(pattern, RegexOptions.Compiled);


                    // Знаходимо всі збіги
                    MatchCollection matches = regex.Matches(input);


                    if (matches.Count == 0)
                    {
                        Console.WriteLine("\nУ тексті не знайдено коректних грошових сум.");
                        Console.WriteLine("Правильний формат: $XXX.XX або $X,XXX.XX\n");
                    }
                    else
                    {
                        Console.WriteLine("\nЗнайдені грошові суми:");
                        foreach (Match match in matches)
                        {
                            Console.WriteLine($"- {match.Value}");
                        }
                    }
                }
                catch (RegexMatchTimeoutException)
                {
                    Console.WriteLine("Помилка: Перевищено час виконання регулярного виразу.");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Помилка в патерні: {ex.Message}");
                }


                Console.WriteLine("\nНатисніть Enter для виходу або будь-яку іншу клавішу для продовження...");
                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    break;
                }
            }
        }
    }
}
