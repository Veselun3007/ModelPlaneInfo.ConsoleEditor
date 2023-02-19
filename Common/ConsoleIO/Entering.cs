using System.Text.RegularExpressions;

namespace Common.ConsoleIO
{
    public static class Entering
    {
        static readonly string format = "{0,15}: ";
        public static int EnterInt(string prompt)
        {
            string str;
            while (true)
            {
                Console.Write(format, prompt);
                str = Console.ReadLine();
                int value;
                try
                {
                    value = Convert.ToInt32(str);
                }
                catch (FormatException)
                {
                    Console.WriteLine("\t" + "Not an integer entered ",
                        Console.ForegroundColor = ConsoleColor.DarkRed);
                    Console.ResetColor();
                    continue;
                }
                return value;
            }
        }
        public static int EnterInt(string prompt, int min, int max = int.MaxValue)
        {
            while (true)
            {
                int value = EnterInt(prompt);
                if (value >= min && value <= max)
                {
                    return value;
                }
                Console.WriteLine("\tThe value should be " +
                    "between {0} and {1}", min, max,
                        Console.ForegroundColor = ConsoleColor.DarkRed);
                Console.ResetColor();
            }
        }
        public static string EnterString(string prompt)
        {
            Console.Write(format, prompt);
            string s = Console.ReadLine();
            s = s.Trim();
            RemoveDuplicateSpaces(s);
            return s;
        }
        public static string EnterString(string prompt,int minLenght, int maxLenght = int.MaxValue)
        {
            while (true)
            {
                string s = EnterString(prompt);
                if (s.Length >= minLenght && s.Length <= maxLenght)
                {
                    return s;
                }
                Console.WriteLine("\tYou must enter the number of characters between {0} and {1}", 
                    minLenght, maxLenght, Console.ForegroundColor = ConsoleColor.DarkRed);
                Console.ResetColor();
            }
        }
        public static string EnterString(string prompt, string pattern, string errorMessage = null, RegexOptions options = RegexOptions.None)
        {
            while (true)
            {
                string s = EnterString(prompt);
                if (Regex.IsMatch(s, pattern, options))
                {
                    return s;
                }
                Console.WriteLine($"\t{errorMessage}", Console.ForegroundColor = ConsoleColor.DarkRed);
                Console.ResetColor();
            }
        }      
        public static int? EnterNullableInt32(string prompt)
        {
            Console.Write($"{prompt}: ");
            string s = Console.ReadLine();
            return int.TryParse(s, out int value) ? value : (int?)null;
        }
        public static string RemoveDuplicateSpaces(string s)
        {
            Regex regex = new(@"\s+");
            s = regex.Replace(s, " ");
            return s;
        }

    }
}

   

