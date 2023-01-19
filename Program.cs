using System;
using System.Diagnostics;
using System.Timers;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace @SearchEngine
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

        private static string data_address = $"{Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.IndexOf("Search_Engine"))}Search Engine-Data";
        private static string[,,] data = new string[100000, 99, 10];
        private static Dictionary<string, List<string>> genres = new Dictionary<string, List<string>>(18);

        public static void Main()
        {
            Initialize();
            while (true)
            {
                AppIntro();
                try
                {
                    menu.Show(ConsoleColor.DarkYellow);
                    CallFunction(Get.Int(new Range(0, menu.Length), "Option: ", ConsoleColor.DarkCyan));
                    for (ConsoleKey inputKey = Console.ReadKey(true).Key; inputKey != ConsoleKey.Spacebar && inputKey != ConsoleKey.Enter; inputKey = Console.ReadKey(true).Key) ;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    "Program has crashed!\nRebooting...".Show(ConsoleColor.DarkRed);
                    System.Threading.Tasks.Task.Delay(2000);
                }
                finally
                {
                    Console.Clear();
                }
            }
        }

        private static void CallFunction(int option)
        {
            switch (option)
            {
                case 1:
                    PagesContaining(Get.String("word: ", ConsoleColor.DarkCyan), Get.Int(new Range(1), "number of repetitions: ", ConsoleColor.DarkCyan));
                    break;
                case 2:
                    PagesContaining(Get.String("genre: ", ConsoleColor.DarkCyan));
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

        private static void Initialize()
        {
            "Initializing...".Show(ConsoleColor.DarkBlue);
            Stopwatch watch = Stopwatch.StartNew();
            double allocated = GC.GetTotalMemory(false);

            string[] txtFiles = Directory.EnumerateFiles(data_address + "/DataSet/", "*.txt").ToArray();
            char[] chars = { ' ', '.', ',', ';', ':', '?' };
            string[] lines;
            string[] words;

            for (int i = 0; i < 100000; i++)
            {
                lines = File.ReadAllLines(txtFiles[i]);

                for (int j = 0; j < 99; j++)
                {
                    words = lines[j].Split(chars);
                    for (int k = 0; k < 10; k++)
                    {
                        data[i, j, k] = words[k];
                    }
                }
            }

            //generate genres dictionary
            string[] genreFiles = Directory.EnumerateFiles(data_address + "/Genres/", "*.txt").ToArray();
            string genre;

            int start = data_address.Length + "/Genres/".Length;
            for (int i = 0; i < 18; i++)
            {
                genre = genreFiles[i].Substring(start, genreFiles[i].IndexOf(".txt")-start);
                genres.Add(genre, new List<string>());
                lines = File.ReadAllLines(genreFiles[i]);
                for (int j = 0; j < lines.Length; j++)
                    genres[genre].Add(lines[j]);
            }


            watch.Stop();
            "Time: ".Show(ConsoleColor.DarkBlue, false);
            $"{watch.Elapsed.ToString().Substring(3, 5)}".Show(ConsoleColor.DarkCyan);
            "Space: ".Show(ConsoleColor.DarkBlue, false);
            $"{(GC.GetTotalMemory(false) - allocated).InMegaBytes()}mb".Show(ConsoleColor.DarkCyan);
        }

        private static void PagesContaining(string word, int repetitionNum)
        {
            Stopwatch watch = Stopwatch.StartNew();
            double allocated = GC.GetTotalMemory(false);


            watch.Stop();
            "Time: ".Show(ConsoleColor.DarkBlue, false);
            $"{watch.Elapsed.ToString().Substring(6, 5)}s".Show(ConsoleColor.DarkCyan);

            "Space: ".Show(ConsoleColor.DarkBlue, false);
            $"{(GC.GetTotalMemory(false) - allocated).InMegaBytes()}mb".Show(ConsoleColor.DarkCyan);
        }


        private static void PagesContaining(string genre)
        {
            Stopwatch watch = Stopwatch.StartNew();
            double allocated = GC.GetTotalMemory(false);

            int pagesNum = 0;
            for (int i = 0; i < 100000; i++) { 
                for (int j = 0; j < 10; j++)
                    if (genres.FirstOrDefault(x => x.Value.Contains(data[i, 50, j])).Key == genre)
                        pagesNum++;
            }

            pagesNum.ToString().Show(ConsoleColor.DarkYellow);
            watch.Stop();
            "Time: ".Show(ConsoleColor.DarkBlue, false);
            $"{(watch.Elapsed.ToString()).Substring(6, 5)}s".Show(ConsoleColor.DarkCyan);

            "Space: ".Show(ConsoleColor.DarkBlue, false);
            $"{(GC.GetTotalMemory(false) - allocated).InMegaBytes()}mb".Show(ConsoleColor.DarkCyan);
        }

        private static void Function3()
        {
            Stopwatch watch = Stopwatch.StartNew();
            double allocated = GC.GetTotalMemory(false);



            watch.Stop();
            "Time: ".Show(ConsoleColor.DarkBlue, false);
            $"{watch.Elapsed.ToString().Substring(6, 5)}s".Show(ConsoleColor.DarkCyan);
            "Space: ".Show(ConsoleColor.DarkBlue, false);
            $"{(GC.GetTotalMemory(false) - allocated).InMegaBytes()}mb".Show(ConsoleColor.DarkCyan);
        }

        private static void Function4()
        {
            Stopwatch watch = Stopwatch.StartNew();
            double allocated = GC.GetTotalMemory(false);



            watch.Stop();
            "Time: ".Show(ConsoleColor.DarkBlue, false);
            $"{watch.Elapsed.ToString().Substring(6, 5)}s".Show(ConsoleColor.DarkCyan);
            "Space: ".Show(ConsoleColor.DarkBlue, false);
            $"{(GC.GetTotalMemory(false) - allocated).InMegaBytes()}mb".Show(ConsoleColor.DarkCyan);
        }

        private static void Function5()
        {
            Stopwatch watch = Stopwatch.StartNew();
            double allocated = GC.GetTotalMemory(false);



            watch.Stop();
            "Time: ".Show(ConsoleColor.DarkBlue, false);
            $"{watch.Elapsed.ToString().Substring(6, 5)}s".Show(ConsoleColor.DarkCyan);
            "Space: ".Show(ConsoleColor.DarkBlue, false);
            $"{(GC.GetTotalMemory(false) - allocated).InMegaBytes()}mb".Show(ConsoleColor.DarkCyan);
        }

        private static void Function6()
        {
            Stopwatch watch = Stopwatch.StartNew();
            double allocated = GC.GetTotalMemory(false);



            watch.Stop();
            "Time: ".Show(ConsoleColor.DarkBlue, false);
            $"{watch.Elapsed.ToString().Substring(6, 5)}s".Show(ConsoleColor.DarkCyan);
            "Space: ".Show(ConsoleColor.DarkBlue, false);
            $"{(GC.GetTotalMemory(false) - allocated).InMegaBytes()}mb".Show(ConsoleColor.DarkCyan);
        }

        private static void Function7()
        {
            Stopwatch watch = Stopwatch.StartNew();
            double allocated = GC.GetTotalMemory(false);



            watch.Stop();
            "Time: ".Show(ConsoleColor.DarkBlue, false);
            $"{watch.Elapsed.ToString().Substring(6, 5)}s".Show(ConsoleColor.DarkCyan);
            "Space: ".Show(ConsoleColor.DarkBlue, false);
            $"{(GC.GetTotalMemory(false) - allocated).InMegaBytes()}mb".Show(ConsoleColor.DarkCyan);
        }

        private static void Function8()
        {
            Stopwatch watch = Stopwatch.StartNew();
            double allocated = GC.GetTotalMemory(false);



            watch.Stop();
            "Time: ".Show(ConsoleColor.DarkBlue, false);
            $"{watch.Elapsed.ToString().Substring(6, 5)}s".Show(ConsoleColor.DarkCyan);
            "Space: ".Show(ConsoleColor.DarkBlue, false);
            $"{(GC.GetTotalMemory(false) - allocated).InMegaBytes()}mb".Show(ConsoleColor.DarkCyan);
        }

        private static void AppIntro()
        {
            string welcomeMessage = "Search Engine";
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
            string string_input;
            message.Show(messageColor, false);
            while ((string_input = Console.ReadLine()) == null)
            {
                "No word was inputed.\n".Show(ConsoleColor.Red);
                message.Show(messageColor, false);
            }
            return string_input;
        }
    }

    public struct Range
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

        public static double InMegaBytes(this double bytes)
            => Math.Round(((bytes / 1024) / 1024), 2);
    }
}