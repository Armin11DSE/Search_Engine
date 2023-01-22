using System;
using System.Diagnostics;
using System.Timers;
using System.IO;
using System.Collections.Generic;
using System.Text;
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
        private static double allocated;

        public static void Main()
        {
            Initialize();
            while (true)
            {
                AppIntro();
                menu.Show(ConsoleColor.DarkYellow);
                CallFunction(Get.Int(new Range(0, menu.Length), "Option: ", ConsoleColor.DarkCyan));
                for (ConsoleKey inputKey = Console.ReadKey(true).Key; inputKey != ConsoleKey.Spacebar && inputKey != ConsoleKey.Enter; inputKey = Console.ReadKey(true).Key) ;
                watch.Reset();
                Console.Clear();
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
                        output = BooksWithGenre(Get.String("Genre: ", ConsoleColor.DarkCyan), Get.Int(new Range(1), "Number of Common Words: ", ConsoleColor.DarkCyan), new Range(1000, 1500), out List<List<int>> books, out bool[,] adjacencyMatrix);
                        "Books: ".Show(ConsoleColor.DarkBlue);
                        books.Show("Book", ConsoleColor.Blue, ConsoleColor.DarkYellow);
                        "Total Number of Pages: ".Show(ConsoleColor.DarkBlue, false);
                        output.ToString().Show(ConsoleColor.DarkYellow);
                        break;
                    case 7:
                        output = BooksContaining(Get.String("Genre: ", ConsoleColor.DarkCyan), Get.String("Word: ", ConsoleColor.DarkCyan), Get.Int(new Range(1), "Number of Repetitions: ", ConsoleColor.DarkCyan), Get.Int(new Range(1), "Number of Common Words: ", ConsoleColor.DarkCyan), out books);
                        "Books: ".Show(ConsoleColor.DarkBlue);
                        books.Show("Book", ConsoleColor.Blue, ConsoleColor.DarkYellow);
                        "Total Number of Books: ".Show(ConsoleColor.DarkBlue, false);
                        output.ToString().Show(ConsoleColor.DarkYellow);
                        break;
                    case 8:
                        output = LargestCompleteBook(Get.Int(new Range(1), "Number of Common Words: ", ConsoleColor.DarkCyan), new Range(200, 300), out List<int> book);
                        "Pages: ".Show(ConsoleColor.DarkBlue);
                        book.Show(ConsoleColor.DarkYellow);
                        "Total Number of Pages: ".Show(ConsoleColor.DarkBlue, false);
                        output.ToString().Show(ConsoleColor.DarkYellow);
                        break;
                    case 0:
                        Terminate();
                        Environment.Exit(0);
                        break;
                }

                "Time: ".Show(ConsoleColor.DarkBlue, false);
                $"{watch.Elapsed.ToString()[6..]}s".Show(ConsoleColor.DarkCyan);
                "Space: ".Show(ConsoleColor.DarkBlue, false);
                $"{allocated.InMegaBytes()}mb".Show(ConsoleColor.DarkCyan);
            }
            catch (Exception ex)
            {
                ex.Message.Show(ConsoleColor.DarkRed);
            }
        }

        //Part_1
        private static int PagesContaining(string word, int repetitionNum)
        {
            watch.Start();
            allocated = Process.GetCurrentProcess().PrivateMemorySize64;

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
            allocated = Process.GetCurrentProcess().PrivateMemorySize64 - allocated;
            return pagesNum;
        }

        //Part_2
        private static int PagesWithMainGenre(string genre)
        {
            watch.Start();
            allocated = Process.GetCurrentProcess().PrivateMemorySize64;

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
            allocated = Process.GetCurrentProcess().PrivateMemorySize64 - allocated;
            return pagesNum;
        }

        //Part_3
        private static int SimpleAdjacentPages(string genre, int pageNum, int commonWordsNum, Range range, out List<int> pages)
        {
            watch.Start();
            allocated = Process.GetCurrentProcess().PrivateMemorySize64;

            if (!genres.ContainsKey(genre))
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
            allocated = Process.GetCurrentProcess().PrivateMemorySize64 - allocated;
            return pages.Count;
        }

        //Part_4
        private static int AdjacentPages(int pageNum, int commonWordsNum, Range range, out List<int> pages)
        {
            watch.Start();
            allocated = Process.GetCurrentProcess().PrivateMemorySize64;

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
            allocated = Process.GetCurrentProcess().PrivateMemorySize64 - allocated;
            return pages.Count;
        }

        //Part_5
        private static int CompleteAdjacentPages(int pageNum, int commonGenresNum, int commonWordsNum, Range range, out List<int> pages)
        {
            watch.Start();
            allocated = Process.GetCurrentProcess().PrivateMemorySize64;

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
            allocated = Process.GetCurrentProcess().PrivateMemorySize64 - allocated;
            return pages.Count;
        }

        //Part_6
        private static int BooksWithGenre(string genre, int commonWordsNum, Range range, out List<List<int>> books, out bool[,] adjacencyMatrix)
        {
            if (!genres.ContainsKey(genre))
                throw new Exception($"{genre} isn't a valid genre");

            watch.Start();
            allocated = Process.GetCurrentProcess().PrivateMemorySize64;

            books = new List<List<int>>();
            books.Add(new List<int>());
            adjacencyMatrix = new bool[range.max - range.min, range.max - range.min];

            for (int i = range.min; i < range.max; i++)
            {
                HashSet<string> words = new HashSet<string>();
                for (int j = 0; j < 99; j += 11)
                {
                    for (int k = 0; k < data[i, j].Count; k++)
                    {
                        string word = data[i, j][k];
                        if (genres[genre].Contains(word))
                        {
                            if (!words.Contains(word))
                                words.Add(word);
                        }
                        else
                        {
                            for (int c = 1; k + c < data[i, j].Count && c <= 2; c++)
                            {
                                word += $" {data[i, j][k + c]}";
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

                for (int _i = range.min; _i < i; _i++)
                {
                    if (adjacencyMatrix[i - range.min, _i - range.min] || adjacencyMatrix[_i - range.min, i - range.min])
                    {
                        books.Last().Add(_i);
                    }
                }

                if (books.Last().Count != 0)
                {
                    books.Last().Add(i);
                }

                for (int _i = i + 1; _i < range.max; _i++)
                {
                    int counter = 0;
                    bool exit = false;
                    for (int j = 0; !exit && j < 99; j++)
                    {
                        for (int k = 0; k < data[_i, j].Count; k++)
                        {
                            string word = data[_i, j][k];
                            if (words.Contains(word))
                            {
                                counter++;
                            }
                            else
                            {
                                for (int c = 1; k + c < data[_i, j].Count && c <= 2; c++)
                                {
                                    word += $" {data[_i, j][k + c]}";
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
                                adjacencyMatrix[i - range.min, _i - range.min] = true;
                                adjacencyMatrix[_i - range.min, i - range.min] = true;
                                books.Last().Add(_i);
                                exit = true;
                                break;
                            }
                        }
                    }
                }

                if (books.Last().Count != 0)
                {
                    if (books.Last().First() > i)
                    {
                        books.Last().Insert(0, i);
                    }
                    books.Add(new List<int>());
                }
            }

            if (books.Last().Count == 0)
                books.Remove(books.Last());

            watch.Stop();
            allocated = Process.GetCurrentProcess().PrivateMemorySize64 - allocated;
            return books.Count;
        }

        //Part_7
        private static int BooksContaining(string genre, string word, int repetitionsNum, int commonWordsNum, out List<List<int>> books)
        {
            watch.Start();
            allocated = Process.GetCurrentProcess().PrivateMemorySize64;

            books = new List<List<int>>();
            BooksWithGenre(genre, commonWordsNum, new Range(1000, 1500), out List<List<int>> _books, out bool[,] adjacencyMatrix);
            for (int book = 0; book < _books.Count; book++)
            {
                int counter = 0;
                for (int page = 0; page < _books[book].Count && counter < repetitionsNum; page++)
                {
                    for (int j = 0; j < 99 && counter < repetitionsNum; j++)
                    {
                        for (int k = 0; k < data[page, j].Count && counter < repetitionsNum; k++)
                        {
                            if (data[page, j][k] == word)
                            {
                                counter++;
                            }
                        }
                    }
                }
                if (counter >= repetitionsNum)
                    books.Add(_books[book]);
            }

            watch.Stop();
            allocated = Process.GetCurrentProcess().PrivateMemorySize64 - allocated;
            return books.Count;
        }

        //Part_8
        private static int LargestCompleteBook(int repetitionsNum, Range range, out List<int> largestBook)
        {
            watch.Start();
            allocated = Process.GetCurrentProcess().PrivateMemorySize64;

            largestBook = new List<int>();
            foreach (string genre in genres.Keys)
            {
                BooksWithGenre(genre, repetitionsNum, range, out List<List<int>> books, out bool[,] adjacencyMatrix);

                for (int book = 0; book < books.Count; book++)
                {
                    bool areAdjacent = true;
                    for (int i = 0; i < books[book].Count && areAdjacent; i++)
                    {
                        for (int j = i + 1; j < books[book].Count && areAdjacent; j++)
                        {
                            if (!adjacencyMatrix[books[book][i] - range.min, books[book][j] - range.min] && !adjacencyMatrix[books[book][j] - range.min, books[book][i] - range.min])
                            {
                                areAdjacent = false;
                            }
                        }
                    }
                    if (areAdjacent && books[book].Count > largestBook.Count)
                        largestBook = books[book];
                }
            }

            watch.Stop();
            allocated = Process.GetCurrentProcess().PrivateMemorySize64 - allocated;
            return largestBook.Count;
        }

        private static void Initialize()
        {
            "Initializing...".Show(ConsoleColor.Magenta);
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
            "Initializing Complete".Show(ConsoleColor.Magenta);
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