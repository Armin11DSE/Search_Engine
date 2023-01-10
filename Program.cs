using System;
using System.Timers;

namespace @Text_Mining
{
    public static class Program
    {
        private static string[] menu =
            { ""
            , ""
            , ""
            , ""
            , ""
            , ""
            , ""
            , ""};

        public static void Main()
        {
            while (true)
            {
                try
                {
                    Run();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    "Program has crashed!\nRebooting...".Show(ConsoleColor.DarkRed);

                    Console.Clear();
                }
            }
        }

        private static void Run()
        {
            while (true)
            {
                menu.Show(ConsoleColor.DarkYellow);
                CallFunction(Get.Int(new range(0, menu.Length)));
            }
        }

        private static void CallFunction(int option)
        {
            switch (option)
            {
                case 1:
                    TSI_Count();
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

        private static void TSI_Count()
        {

        }

        private static void Function2()
        {

        }

        private static void Function3()
        {

        }

        private static void Function4()
        {

        }

        private static void Function5()
        {

        }

        private static void Function6()
        {

        }

        private static void Function7()
        {

        }

        private static void Function8()
        {

        }

        private static void Terminate()
            => "Thank you for using our program".Show(ConsoleColor.Green);

        private static void CallTimer()
        {

        }
    }

    public static class Get
    {
        public static int Int(range range)
        {
            int int_input;
            while (!int.TryParse(Console.ReadLine(), out int_input) || int_input < range.min || int_input > range.max)
                $"Input must be a number between {range.min} and {range.max}!".Show(ConsoleColor.Red);
            return int_input;
        }

        public static string String()
        {
            string? string_input;
            while ((string_input = Console.ReadLine()) == null)
                "No word was inputed!".Show(ConsoleColor.Red);
            return string_input;
        }
    }

    public struct range
    {
        public readonly int min;
        public readonly int max;

        public range(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
    }

    public static class Extensions
    {
        public static void Show(this string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message + '\n');
            Console.ResetColor();
        }

        public static void Show(this string[] menu, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            for (int i = 0; i < menu.Length; i++)
                Console.WriteLine($"{i + 1}. {menu[i]}");
            Console.WriteLine();
            Console.ResetColor();
        }
    }
}