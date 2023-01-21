using System;
using System.Diagnostics;
using System.Timers;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
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

        private static string data_address = $"{Environment.CurrentDirectory[..Environment.CurrentDirectory.IndexOf("Search Engine")]}Search Engine-Data";
        private static List<string>[,] data = new List<string>[100000, 99];
        private static Dictionary<string, HashSet<string>> genres = new Dictionary<string, HashSet<string>>(18);
        private static Stopwatch watch = new Stopwatch();
        private static double beforeAllocation;

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
                    for (ConsoleKey inputKey = Console.ReadKey(true).Key; inputKey != ConsoleKey.Spacebar && inputKey != ConsoleKey.Enter; inputKey = Console.ReadKey(true).Key) ;
                }
                finally
                {
                    watch.Reset();
                    Console.Clear();
                }
            }
        }

        private static void CallFunction(int option)
        {
            try
            {
                switch (option)
                {
                    case 1:
                        PagesContaining(Get.String("Word: ", ConsoleColor.DarkCyan), Get.Int(new Range(1), "Number of Repetitions: ", ConsoleColor.DarkCyan)).ToString().Show(ConsoleColor.DarkYellow);
                        break;
                    case 2:
                        PagesWithMainGenre(Get.String("Genre: ", ConsoleColor.DarkCyan)).ToString().Show(ConsoleColor.DarkYellow);
                        break;
                    case 3:
                        int output = SimpleAdjacentPages(Get.String("Genre: ", ConsoleColor.DarkCyan), Get.Int(new Range(0, 99999), "Page Number: ", ConsoleColor.DarkCyan), Get.Int(new Range(1), "Number of Common Words: ", ConsoleColor.DarkCyan), new Range(20000, 30000), out List<int> pages);
                        "Pages: ".Show(ConsoleColor.DarkBlue);
                        pages.Show(ConsoleColor.DarkYellow);
                        "Total Number of Pages: ".Show(ConsoleColor.DarkBlue, false);
                        output.ToString().Show(ConsoleColor.DarkYellow);
                        break;
                    case 4:
                        output = AdjacentPages(Get.Int(new Range(0, 99999), "Page Number: ", ConsoleColor.DarkCyan), Get.Int(new Range(1), "Number of Common Words: ", ConsoleColor.DarkCyan), new Range(5000, 10000), out pages);
                        "Pages: ".Show(ConsoleColor.DarkBlue);
                        pages.Show(ConsoleColor.DarkYellow);
                        "Total Number of Pages: ".Show(ConsoleColor.DarkBlue, false);
                        output.ToString().Show(ConsoleColor.DarkYellow);
                        break;
                    case 5:
                        output = CompleteAdjacentPages(Get.Int(new Range(0, 99999), "Page Number: ", ConsoleColor.DarkCyan), Get.Int(new Range(1, 18), "Number of Common Genres: ", ConsoleColor.DarkCyan), Get.Int(new Range(1), "Number of Common Words: ", ConsoleColor.DarkCyan), new Range(2000, 3000), out pages);
                        "Pages: ".Show(ConsoleColor.DarkBlue);
                        pages.Show(ConsoleColor.DarkYellow);
                        "Total Number of Pages: ".Show(ConsoleColor.DarkBlue, false);
                        output.ToString().Show(ConsoleColor.DarkYellow);
                        break;
                    case 6:
                        output = BooksWithGenre(Get.String("Genre: ", ConsoleColor.DarkCyan), Get.Int(new Range(1), "Number of Common Words: ", ConsoleColor.DarkCyan), new Range(1000, 1500), out List<List<int>> books);
                        "Books: ".Show(ConsoleColor.DarkBlue);
                        books.Show("Book", ConsoleColor.Blue, ConsoleColor.DarkYellow);
                        "Total Number of Pages: ".Show(ConsoleColor.DarkBlue, false);
                        output.ToString().Show(ConsoleColor.DarkYellow);
                        break;
                    case 7:
                        //BooksContaining();
                        break;
                    case 8:
                        Function8();
                        break;
                    case 0:
                        Terminate();
                        Environment.Exit(0);
                        break;
                }

                "Time: ".Show(ConsoleColor.DarkBlue, false);
                $"{watch.Elapsed.ToString()[6..]}s".Show(ConsoleColor.DarkCyan);
                "Space: ".Show(ConsoleColor.DarkBlue, false);
                $"{(Process.GetCurrentProcess().PrivateMemorySize64 - beforeAllocation).InMegaBytes()}mb".Show(ConsoleColor.DarkCyan);
            }
            catch (Exception ex)
            {
                ex.Message.Show(ConsoleColor.DarkRed);
            }
        }

        private static int PagesContaining(string word, int repetitionNum)
        {
            watch.Start();
            beforeAllocation = Process.GetCurrentProcess().PrivateMemorySize64;

            int pagesNum = 0;
            for (int i = 0; i < 100000; i++)
            {
                int counter = 0;
                for (int j = 0; j < 99; j++)
                {
                    counter += data[i, j].Count(x => x == word);
                    if (counter >= repetitionNum)
                    {
                        pagesNum++;
                        break;
                    }
                }
            }

            watch.Stop();
            return pagesNum;
        }

        private static int PagesWithMainGenre(string genre)
        {
            watch.Start();
            beforeAllocation = GC.GetTotalMemory(false);

            if (!genres.ContainsKey(genre))
                throw new Exception($"{genre} isn't a valid genre");

            int pagesNum = 0;
            for (int i = 0; i < 100000; i++)
            {
                string word = data[i, 49].First();
                for (int k = 0; k <= 2; k++, word += $" {data[i, 49][k]}")
                {
                    if (genres[genre].Contains(word))
                    {
                        pagesNum++;
                        break;
                    }
                }
            }

            watch.Stop();
            return pagesNum;
        }

        private static int SimpleAdjacentPages(string genre, int pageNum, int commonWordsNum, Range range, out List<int> pages)
        {
            watch.Start();
            beforeAllocation = GC.GetTotalMemory(false);

            if (!genres.Keys.Contains(genre))
                throw new Exception($"{genre} isn't a valid genre");

            pages = new List<int>();
            HashSet<string> words = new HashSet<string>();
            for (int j = 0; j < 99; j += 11)
            {
                for (int k = 0; k < data[pageNum, j].Count; k++)
                {
                    string word = data[pageNum, j][k];
                    if (genres[genre].Contains(word))
                    {
                        if (!words.Contains(word))
                            words.Add(word);
                    }
                    else
                    {
                        k++;
                        for (int c = 1; k < data[pageNum, j].Count && c <= 2; k++, c++)
                        {
                            word += $" {data[pageNum, j][k]}";
                            if (genres[genre].Contains(word))
                            {
                                if (!words.Contains(word))
                                    words.Add(word);
                                break;
                            }
                        }
                    }
                }
            }

            for (int i = range.min; i < range.max; i++)
            {
                if (i == pageNum)
                    continue;

                int counter = 0;
                bool exit = false;
                for (int j = 0; !exit && j < 99; j++)
                {
                    for (int k = 0; k < data[i, j].Count; k++)
                    {
                        string word = data[i, j][k];
                        if (words.Contains(word))
                        {
                            counter++;
                        }
                        else
                        {
                            for (int c = 1; k + c < data[i, j].Count && c <= 2; c++)
                            {
                                word += $" {data[i, j][k + c]}";
                                if (words.Contains(word))
                                {
                                    counter++;
                                    k += c;
                                    break;
                                }
                            }
                        }

                        if (counter == commonWordsNum)
                        {
                            pages.Add(i);
                            exit = true;
                            break;
                        }
                    }
                }
            }

            watch.Stop();
            return pages.Count;
        }

        private static int AdjacentPages(int pageNum, int commonWordsNum, Range range, out List<int> pages)
        {
            watch.Start();
            beforeAllocation = GC.GetTotalMemory(false);

            pages = new List<int>();
            Dictionary<string, List<string>> genreWords = new Dictionary<string, List<string>>(9);
            for (int i = 0; i < 99;)
            {
                string word = data[pageNum, i].First();
                string genre = genres.FirstOrDefault(x => x.Value.Contains(word)).Key;
                for (int k = 1; genre == default; k++, word += $" {data[pageNum, i][k]}")
                    genre = genres.FirstOrDefault(x => x.Value.Contains(word)).Key;

                genreWords.Add(genre, new List<string>(330));
                i += 11;
                for (int j = i - 11; j < i; j++)
                    genreWords[genre].AddRange(data[i, j]);
            }

            for (int i = range.min; i < range.max; i++)
            {
                if (i == pageNum)
                    continue;

                int counter;
                bool isAdjacent = false;
                for (int _i = 0; !isAdjacent && _i < 99;)
                {
                    counter = 0;
                    string word = data[i, _i].First();
                    string genre = genre = genres.FirstOrDefault(x => x.Value.Contains(word)).Key;
                    for (int k = 0; genre == default; k++, word += $" {data[i, _i][k]}", genre = genres.FirstOrDefault(x => x.Value.Contains(word)).Key) ;

                    _i += 11;
                    if (!genreWords.ContainsKey(genre))
                        continue;

                    for (int j = _i - 11; !isAdjacent && j < _i; j++)
                    {
                        for (int k = 0; k < data[i, j].Count; k++)
                        {
                            if (genreWords[genre].Contains(data[i, j][k]))
                                counter++;
                            if (counter == commonWordsNum)
                            {
                                isAdjacent = true;
                                break;
                            }
                        }
                    }
                }
                if (isAdjacent)
                {
                    pages.Add(i);
                }
            }

            watch.Stop();
            return pages.Count;
        }

        private static int CompleteAdjacentPages(int pageNum, int commonGenresNum, int commonWordsNum, Range range, out List<int> pages)
        {
            watch.Start();
            beforeAllocation = GC.GetTotalMemory(false);

            pages = new List<int>();
            Dictionary<string, int> givenPageGeners = new Dictionary<string, int>(9);

            for (int j = 0; j < 99; j += 11)
            {
                string word = data[pageNum, j].First();
                string genre = genres.FirstOrDefault(x => x.Value.Contains(word) && !givenPageGeners.ContainsKey(x.Key)).Key;
                for (int k = 0; genre == default; k++, word += $" {data[pageNum, j][k]}", genre = genres.FirstOrDefault(x => x.Value.Contains(word) && !givenPageGeners.ContainsKey(x.Key)).Key) ;
                givenPageGeners.Add(genre, j);
            }

            for (int i = range.min; i < range.max; i++)
            {
                if (i == pageNum)
                    continue;

                int commonGenres = 0;
                for (int j = 0; commonGenres < commonGenresNum && j < 99;)
                {
                    string word = data[pageNum, j].First();
                    string genre = genres.FirstOrDefault(x => x.Value.Contains(word)).Key;
                    for (int k = 0; genre == default; k++, word += $" {data[pageNum, j][k]}", genre = genres.FirstOrDefault(x => x.Value.Contains(word)).Key) ;

                    j += 11;
                    if (givenPageGeners.ContainsKey(genre))
                    {
                        int paragraghIndex = givenPageGeners[genre];
                        int commonwords = 0;

                        for (int l1 = j - 11; commonwords < commonWordsNum && l1 < j; l1++)
                        {
                            for (int l2 = paragraghIndex; commonwords < commonWordsNum && l2 < paragraghIndex + 11; l2++)
                            {
                                for (int w1 = 0; commonwords < commonWordsNum && w1 < data[i, l1].Count; w1++)
                                {
                                    for (int w2 = 0; commonwords < commonWordsNum && w2 < data[pageNum, l2].Count; w2++)
                                    {
                                        if (data[i, l1][w1] == data[pageNum, l2][w2])
                                        {
                                            commonwords++;
                                        }
                                    }
                                }
                            }
                        }
                        if (commonwords >= commonWordsNum)
                            commonGenres++;
                    }
                }

                if (commonGenres >= commonGenresNum)
                    pages.Add(i);
            }

            watch.Stop();
            return pages.Count;
        }

        private static int BooksWithGenre(string genre, int commonWordsNum, Range range, out List<List<int>> books)
        {
            watch.Start();
            beforeAllocation = GC.GetTotalAllocatedBytes();

            books = new List<List<int>>();
            bool[] isChecked = new bool[range.max - range.min];
            for (int i = 0; i < range.max - range.min; i++)
            {
                bool iscont = false;
                for (int j = 0; j < 99; j += 11)
                {
                    string word = data[range.min + i, j].First();
                    string genree = genres.FirstOrDefault(x => x.Value.Contains(word)).Key;
                    if (genree == genre)
                    {
                        iscont = true;
                        break;
                    }
                }
                if (!iscont) isChecked[i] = true;
            }
            for (int i = 0; i < range.max - range.min; i++)
            {
                if (isChecked[i])
                {
                    Console.WriteLine("checked " + i);
                    continue;
                }
                List<int> newBook = new List<int>();
                SimpleAdjacentPages(genre, i, commonWordsNum, range, out newBook);
                newBook.Add(i + range.min);
                books.Add(newBook);
                for (int j = 0; j < newBook.Count; j++)
                {
                    isChecked[newBook[j] - range.min] = true;
                }
            }

            return books.Count();
        }

        //private static List<int> recursiveBFS(int root, string genre, Range range, List<int> bookPages, bool[] isChecked)
        //{
        //    int pagesnum = SimpleAdjacentPages(genre, root, 20, range, out List<int> pages);
        //    isChecked[root] = true;
        //    for (int i = 0; i < pagesnum; i++)
        //    {
        //        if (bookPages.Contains(pages[i])) continue;
        //        Console.WriteLine("adddddddddddddddddddddddddddddddddddddd  " + i);

        //        List<int> tmp = recursiveBFS(pages[i], genre, range, bookPages, isChecked);
        //        for (int j = 0; j < tmp.Count; j++)
        //        {
        //            if (bookPages.Contains(tmp[i])) continue;
        //            bookPages.Add(tmp[i]);
        //        }
        //    }

        //    return bookPages;
        //}

        private static void BooksContaining(string genre, string word, int N)
        {
            List<List<int>> books;
            List<List<int>> W_books = new List<List<int>>();

            int n = 0;
            BooksWithGenre(genre, N, new Range(1000, 1500), out books);

            for (int i = 0; i < books.Count; i++)
            {
                for (int j = 0; j < books[i].Count; j++) //list of pages = books[i]
                {
                    n += NTimes(word, books[i][j]);
                    if (N <= n) break;
                }
                if (N <= n)
                    W_books.Add(books[i]);
            }

            $"All valid Books : {W_books.Count}".Show(ConsoleColor.Yellow);

            for (int i = 0; i < W_books.Count; i++)
            {
                $"Book {i} pages ({W_books[i].Count}) : ".Show(ConsoleColor.Red);
                for (int j = 0; j < W_books[i].Count; j++)
                {
                    if (j != W_books.Count - 1)
                        (W_books[i][j] + " , ").Show(ConsoleColor.Blue, false);
                    else
                        (W_books[i][j]).ToString().Show(ConsoleColor.Blue, false);
                }
            }
        }

        private static int NTimes(string word, int pagenumber)
        {
            int n = 0;
            for (int i = 0; i < 99; i++)
            {
                try
                {
                    for (int k = 0; k < 30 && data[pagenumber, i][k] == word; k++)
                        n++;
                }
                catch (Exception e)
                {
                    break;
                }
            }
            return n;
        }

        private static void Function8()
        {

        }

        private static void Initialize()
        {
            "Initializing...".Show(ConsoleColor.DarkBlue);
            Stopwatch watch = Stopwatch.StartNew();
            double allocated = GC.GetTotalMemory(false);

            string txtFilesAddress = data_address + "/DataSet/Text_";
            string[] lines;

            for (int i = 0; i < 100000; i++)
            {
                lines = File.ReadAllLines(txtFilesAddress + i + ".txt");
                for (int j = 0; j < 99; j++)
                {
                    data[i, j] = lines[j].Split().ToList();
                }
            }

            string[] genreFiles = Directory.EnumerateFiles(data_address + "/Genres/", "*.txt").ToArray();
            string genre;
            int start = data_address.Length + "/Genres/".Length;
            for (int i = 0; i < 18; i++)
            {
                genre = genreFiles[i].Substring(start, genreFiles[i].IndexOf(".txt") - start);
                lines = File.ReadAllLines(genreFiles[i]);
                genres.Add(genre, new HashSet<string>(lines.Length));
                for (int j = 0; j < lines.Length; j++)
                    genres[genre].Add(lines[j]);
            }

            watch.Stop();
            "Time: ".Show(ConsoleColor.DarkBlue, false);
            $"{watch.Elapsed.ToString()[3..8]}".Show(ConsoleColor.DarkCyan);
            "Space: ".Show(ConsoleColor.DarkBlue, false);
            $"{(GC.GetTotalMemory(false) - allocated).InMegaBytes()}mb".Show(ConsoleColor.DarkCyan);
            for (ConsoleKey inputKey = Console.ReadKey(true).Key; inputKey != ConsoleKey.Spacebar && inputKey != ConsoleKey.Enter; inputKey = Console.ReadKey(true).Key) ;
            Console.Clear();
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

        public static void Show(this IEnumerable<string> list, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            for (int i = 0; i < list.Count(); i++)
                Console.WriteLine($"{i + 1}.{list.ElementAt(i)}");
            Console.WriteLine();
            Console.ResetColor();
        }

        public static void Show(this IEnumerable<int> list, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            for (int i = 0, wordCounter = 1; i < list.Count(); i++, wordCounter++)
            {
                Console.Write($"\"{list.ElementAt(i)}\"  ");
                if (wordCounter == 10)
                {
                    Console.WriteLine();
                    wordCounter = 0;
                }
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        public static void Show(this IEnumerable<IEnumerable<int>> list, string name = "", ConsoleColor name_color = ConsoleColor.White, ConsoleColor list_color = ConsoleColor.White)
        {
            for (int i = 0; i < list.Count(); i++)
            {
                $"{name}{i + 1}: ".Show(name_color);
                list.ElementAt(i).Show(list_color);
            }
            Console.WriteLine();
        }

        public static double InMegaBytes(this double bytes)
            => Math.Round(((bytes / 1024) / 1024), 2);
    }
}