using System;
using System.Diagnostics;
using System.Timers;
using System.IO;

namespace @Text_Mining
{
    public static class Program
    {
        private static readonly string[] menu =
            { "Number of pages containing a word"
            , "Number of pages with a main genre"
            , "Number of pages with simple-adjacency with a page"
            , "Number of pages with adjacency with a page"
            , "Number of pages with complete-adjacency with a page"
            , "Books with a genre"
            , "Books with a genre containing a word"
            , "Books with complete-adjacency"};

        private const string data_address = @"";

        public static void Main()
        {
            while (true)
            {
                Welcome();
                try
                {
                    menu.Show(ConsoleColor.DarkYellow);
                    CallFunction(Get.Int(new Range(0, menu.Length), "Option: ", ConsoleColor.DarkCyan));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    "Program has crashed!\nRebooting...".Show(ConsoleColor.DarkRed);
                }
                finally
                {
                    for (ConsoleKey inputKey = Console.ReadKey().Key; inputKey != ConsoleKey.Spacebar && inputKey != ConsoleKey.Enter; inputKey = Console.ReadKey().Key) ;
                    Console.Clear();
                }
            }
        }

        private static void CallFunction(int option)
        {
            switch (option)
            {
                case 1:
                    TextsContatining(Get.String("word: ", ConsoleColor.DarkCyan), Get.Int(new Range(1), "number of repetitions: ", ConsoleColor.DarkCyan));
                    break;
                case 2:
                    Function2();
                    break;
                case 3:
                    Function3();
                    break;
                case 4:
                    Function4();
                    break;
                case 5:
                    Function5();
                    break;
                case 6:
                    Function6();
                    break;
                case 7:
                    Function7();
                    break;
                case 8:
                    Function8();
                    break;
                case 0:
                    Terminate();
                    Environment.Exit(0);
                    break;
            }
        }

        private static void TextsContatining(string word, int repetitionNum)
        {
            Stopwatch watch = Stopwatch.StartNew();
            watch.Stop();
            "Time: ".Show(ConsoleColor.DarkBlue, false);
            $"{watch.Elapsed.ToString()[6..]}s".Show(ConsoleColor.DarkCyan);
            "Space: ".Show(ConsoleColor.DarkBlue, false);
            $"mb".Show(ConsoleColor.DarkCyan);
        }

        private static void Function2()
        {
            Stopwatch watch = Stopwatch.StartNew();
            watch.Stop();
            "Time: ".Show(ConsoleColor.DarkBlue, false);
            $"{watch.Elapsed.ToString()[6..]}s".Show(ConsoleColor.DarkCyan);
            "Space: ".Show(ConsoleColor.DarkBlue, false);
            $"mb".Show(ConsoleColor.DarkCyan);
        }

        private static void Function3()
        {
            Stopwatch watch = Stopwatch.StartNew();
            watch.Stop();
            "Time: ".Show(ConsoleColor.DarkBlue, false);
            $"{watch.Elapsed.ToString()[6..]}s".Show(ConsoleColor.DarkCyan);
            "Space: ".Show(ConsoleColor.DarkBlue, false);
            $"mb".Show(ConsoleColor.DarkCyan);
        }

        private static void Function4()
        {
            Stopwatch watch = Stopwatch.StartNew();
            watch.Stop();
            "Time: ".Show(ConsoleColor.DarkBlue, false);
            $"{watch.Elapsed.ToString()[6..]}s".Show(ConsoleColor.DarkCyan);
            "Space: ".Show(ConsoleColor.DarkBlue, false);
            $"mb".Show(ConsoleColor.DarkCyan);
        }

        private static void Function5()
        {
            Stopwatch watch = Stopwatch.StartNew();
            watch.Stop();
            "Time: ".Show(ConsoleColor.DarkBlue, false);
            $"{watch.Elapsed.ToString()[6..]}s".Show(ConsoleColor.DarkCyan);
            "Space: ".Show(ConsoleColor.DarkBlue, false);
            $"mb".Show(ConsoleColor.DarkCyan);
        }

        private static void Function6()
        {
            Stopwatch watch = Stopwatch.StartNew();
            watch.Stop();
            "Time: ".Show(ConsoleColor.DarkBlue, false);
            $"{watch.Elapsed.ToString()[6..]}s".Show(ConsoleColor.DarkCyan);
            "Space: ".Show(ConsoleColor.DarkBlue, false);
            $"mb".Show(ConsoleColor.DarkCyan);
        }

        private static void Function7()
        {
            Stopwatch watch = Stopwatch.StartNew();
            watch.Stop();
            "Time: ".Show(ConsoleColor.DarkBlue, false);
            $"{watch.Elapsed.ToString()[6..]}s".Show(ConsoleColor.DarkCyan);
            "Space: ".Show(ConsoleColor.DarkBlue, false);
            $"mb".Show(ConsoleColor.DarkCyan);
        }

        private static void Function8()
        {
            Stopwatch watch = Stopwatch.StartNew();
            watch.Stop();
            "Time: ".Show(ConsoleColor.DarkBlue, false);
            $"{watch.Elapsed.ToString()[6..]}s".Show(ConsoleColor.DarkCyan);
            "Space: ".Show(ConsoleColor.DarkBlue, false);
            $"mb".Show(ConsoleColor.DarkCyan);
        }

        private static void Welcome()
        {
            string welcomeMessage = "Welcome To Text Mining";
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (welcomeMessage.Length / 2)) + "}", welcomeMessage));
            Console.ResetColor();
        }

        private static void Terminate()
            => "Thank you for using our program".Show(ConsoleColor.Magenta);
    }

    public static class Get
    {
        public static int Int(Range range, string message = "", ConsoleColor messageColor = ConsoleColor.White)
        {
            int int_input;
            message.Show(messageColor, false);
            while (!int.TryParse(Console.ReadLine(), out int_input) || int_input < range.min || int_input > range.max)
            {
                $"Input must be a number between {range.min} and {range.max}.\n".Show(ConsoleColor.Red);
                message.Show(messageColor, false);
            }
            return int_input;
        }

        public static string String(string message = "", ConsoleColor messageColor = ConsoleColor.White)
        {
            string? string_input;
            message.Show(messageColor, false);
            while ((string_input = Console.ReadLine()) == null)
            {
                "No word was inputed.\n".Show(ConsoleColor.Red);
                message.Show(messageColor, false);
            }
            return string_input;
        }
    }

    public readonly struct Range
    {
        public readonly int min;
        public readonly int max;

        public Range(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public Range(int min)
        {
            this.min = min;
            this.max = int.MaxValue;
        }
    }

    public static class Extensions
    {
        public static void Show(this string message, ConsoleColor color = ConsoleColor.White, bool inLine = true)
        {
            Console.ForegroundColor = color;
            if (inLine)
                Console.WriteLine(message);
            else
                Console.Write(message);
            Console.ResetColor();
        }

        public static void Show(this string[] menu, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            for (int i = 0; i < menu.Length; i++)
                Console.WriteLine($"{i + 1}.{menu[i]}");
            Console.WriteLine();
            Console.ResetColor();
        }
    }
}