using System;
using System.Collections.Generic;

namespace Arithmetic
{
    class Program
    {
        static void Main(string[] args)
        {
            ICalculator calculator = CreateCalculator();

            Dictionary<string, decimal> expressionsWithExpectedResults = new Dictionary<string, decimal>
				{
                    //Original expressions
					{"1", 1},

					{"1 + 1", 2},
					{"1 - 1", 0},
					{"1 * 1", 1},
					{"1 / 1", 1},

                    {"3 + 4 + 5", 12},
                    {"3 + 4 - 5", 2},
                    {"3 + 4 + -5", 2},
                    {"3 + 4 * 5", 23},
                    {"3 * 4 / 5", 2.4m},

                    {"3 + 4 + 5 * 0", 7},
                    {"0 * 3 + 4 - 5", -1},
					
                    {"1 + 1 - 1 * 1 / 1", 1},
                    {"1 / 1 * 1 - 1 + 1", 1},

                    {"1 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 1 + 1", 23},

                    //Added to test Exponentiation and Dijkstra's algorithm
                    {"3 + 4", 7},
                    {"5 + 2 ^ 3", 13},
                    {"( 4 + 8 ) * ( 6 - 5 ) / ( ( 3 - 2 ) * ( 2 + 2 ) )", 3},
                    {"2 + 1 - 12 / 3", -1},
                    {"2 * 3 - 4 / 5", 5.2m},
                    {"162 / ( 2 + 1 ) ^ 4", 2},
                    {"( 300 + 23 ) * ( 43 - 21 ) / ( 84 + 7 )", 78.087912087912087912087912088m},
                    {"3+4+5", 12},
                    {"3 +4 - 5", 2},
                    {"(9+57)/3*2^3", 176},
                    {"3+ 4 +-5", 2},
                    {"-5 + 90+ (24/4)", 91}
				};

            ConsoleColor originalForegroundColor = Console.ForegroundColor;

            bool happy = true;
            foreach (string expression in expressionsWithExpectedResults.Keys)
            {
                decimal actualResult = calculator.Calculate(expression);

                if (expressionsWithExpectedResults[expression] != actualResult) happy = false;
                string passOrFail = expressionsWithExpectedResults[expression] == actualResult ? "pass" : "fail";
                Console.ForegroundColor = expressionsWithExpectedResults[expression] == actualResult ? ConsoleColor.Green : ConsoleColor.Red;

                Console.WriteLine("{0} => expected {1}; actual {2}; {3}", expression, expressionsWithExpectedResults[expression], actualResult, passOrFail);
            }

            Console.WriteLine();
            Console.ForegroundColor = happy ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(happy ? "All tests passed.  Good job!" : "There is at leats one failed test.");
            Console.ForegroundColor = originalForegroundColor;
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }

        static ICalculator CreateCalculator()
        {
            try
            {
                ICalculator calc = new Calculator();
                return calc;
            }
            catch
            {
                throw new NotImplementedException();
            }

        }
    }
}
