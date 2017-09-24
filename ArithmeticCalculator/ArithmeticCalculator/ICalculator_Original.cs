using System;
using System.Collections.Generic;

namespace Arithmetic
{
    public interface ICalculator
    {
        decimal Calculate(string expression);
    }

    public class Calculator : ICalculator
    {
        private decimal Sum { get; set; }
        private decimal Operand { get; set; }

        public decimal Calculate(string expression)
        {
            string[] expArray = expression.Split(' ');

            //If there is only 1 element in the expression - return that value 
            if (expArray.Length == 1)
                return Convert.ToDecimal(expArray[0]);

            //Use a List<> to store values because I will need to manipulate the size of the List
            List<string> expList = new List<string>();
            foreach (string elem in expArray)
                expList.Add(elem);

            //Go through the List and perform all multiplications and divisions, resize List as necessary
            int counter = expList.Count;
            for (int x = 1; x < counter; x++)
            {
                string oper = expList[x];
                if (oper == "*" || oper == "/")
                {
                    switch (oper)
                    {
                        case "*":
                            Operand = Multiplication(Convert.ToDecimal(expList[x - 1]), Convert.ToDecimal(expList[x + 1]));
                            expList.RemoveAt(x + 1);
                            expList.RemoveAt(x);
                            expList.RemoveAt(x - 1);
                            expList.Insert(x - 1, Operand.ToString());
                            x--;
                            counter = expList.Count;
                            break;
                        case "/":
                            Operand = Division(System.Convert.ToDecimal(expList[x - 1]), System.Convert.ToDecimal(expList[x + 1]));
                            expList.RemoveAt(x + 1);
                            expList.RemoveAt(x);
                            expList.RemoveAt(x - 1);
                            expList.Insert(x - 1, Operand.ToString());
                            x--;
                            counter = expList.Count;
                            break;
                    }
                }
            }

            //If the size of the List is 1, meaning all operators were multiplication and/or division, return the value
            Sum = Convert.ToDecimal(expList[0]);
            if (expList.Count == 1)
                return Sum;

            //Cycle through all elements of the List and perform addition/subtraction
            counter = expList.Count;
            for (int y = 1; y < counter; y++)
            {
                string oper = expList[y];
                if (oper == "+" || oper == "-")
                {
                    switch (oper)
                    {
                        case "+":
                            Sum = Addition(Sum, Convert.ToDecimal(expList[y + 1]));
                            break;
                        case "-":
                            Sum = Subtraction(Sum, Convert.ToDecimal(expList[y + 1]));
                            break;
                    }
                }
            }

            return Sum;
        }

        private decimal Addition(decimal num1, decimal num2)
        {
            return num1 + num2;
        }

        private decimal Subtraction(decimal num1, decimal num2)
        {
            return num1 - num2;
        }

        private decimal Multiplication(decimal num1, decimal num2)
        {
            return num1 * num2;
        }

        private decimal Division(decimal num1, decimal num2)
        {
            return num1 / num2;
        }
    }
}