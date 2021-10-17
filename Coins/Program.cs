using System;
using System.Collections.Generic;
using System.Linq;

namespace Coins
{
    public static class Extentions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || !list.Any();
        }

        public static bool IsSame(this string val1, string val2, bool caseInsensitive = true)
        {
            return val1.Equals(val2, caseInsensitive ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture);
        }
    }

    public class Program
    {
        private static readonly CoinMaster _coinMaster = new();
        private static readonly string ALLOWED_CHANGE_CHARS = "1234567890";
        private static readonly string ALLOWED_DENOM_CHARS = $"{ALLOWED_CHANGE_CHARS},";
        private static readonly string CONFIG_MESSAGE = $"Please enter coin denominations available, only use the following characters '{ALLOWED_DENOM_CHARS}'.";

        public static void Configure()
        {
            Console.WriteLine(CONFIG_MESSAGE);
            string input;
            IEnumerable<string> denomsText;
            IEnumerable<int> denoms = null;
            bool inputValid = false;
            while (!inputValid && !(input = Console.ReadLine()).IsSame("cancel"))
            {
                if (
                    input.Any(d => !ALLOWED_DENOM_CHARS.Contains(d))
                    || (denomsText = input.Split(',')).Count() != denomsText.Distinct().Count()
                    || denomsText.Any(d => !int.TryParse(d, out _))
                    || (denoms = denomsText.Select(d => int.Parse(d))).Any(d => d == 0)
                    )
                {
                    denoms = null;
                    Console.Clear();
                    PrintInvalid();
                    Console.WriteLine(CONFIG_MESSAGE);
                }
                else { inputValid = true; }
            }
            if (!denoms.IsEmpty())
            {
                _coinMaster.SetDenominations(denoms);
                Console.WriteLine("Denominations Set");
                PrintContinue();
            }
        }

        public static void PrintChange(int value)
        {
            PrintLine();
            Console.WriteLine($"Providing Change to the value of {value}");

            foreach (var coin in _coinMaster.GetChange(value))
            {
                if (coin.Key == -1)
                {
                    Console.WriteLine($"Unable to provide change for remainder of {coin.Value}");
                }
                else
                {
                    Console.WriteLine($"{coin.Key} x {coin.Value} : {coin.Key * coin.Value}");
                }
            }
            PrintLine();
            PrintContinue();
        }

        public static void PrintContinue()
        {
            Console.WriteLine("Press enter key to continue");
            Console.ReadLine();
            Console.Clear();
        }

        public static void PrintInvalid() => Console.WriteLine("Invalid Input");

        public static void PrintLine() => Console.WriteLine("==========================================");

        public static void PrintWelcome()
        {
            Console.WriteLine("Welcome to Acme CoinMaster");
            PrintLine();
            Console.WriteLine("To reconfigure denominations please enter 'reset'.");
            Console.WriteLine("To cancel any action please enter 'cancel'.");
            PrintLine();
            Console.WriteLine("Press enter key to continue");
        }

        private static void Main()
        {


            PrintWelcome();
            string input;
            while (!(input = Console.ReadLine()).IsSame("exit") || _coinMaster.Denominations.IsEmpty())
            {
                Console.Clear();
                if (input.IsSame("reset") || _coinMaster.Denominations.IsEmpty())
                {
                    Configure();
                }
                else if (input.Any(d => !ALLOWED_CHANGE_CHARS.Contains(d)))
                {
                    PrintInvalid();
                }
                else if (input.All(d => ALLOWED_CHANGE_CHARS.Contains(d)))
                {
                    if (!int.TryParse(input, out int changeRequired))
                    {
                        PrintInvalid();
                    }
                    else
                    {
                        PrintChange(changeRequired);
                    }
                }

                if (!_coinMaster.Denominations.IsEmpty())
                {
                    Console.WriteLine("Please enter change amount required in cents");
                    Console.WriteLine($"Availible coins are : {string.Join(',', _coinMaster.Denominations)}");
                    PrintLine();
                }
            }
        }
    }
}