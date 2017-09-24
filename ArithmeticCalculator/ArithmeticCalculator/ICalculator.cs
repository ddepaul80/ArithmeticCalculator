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
        public decimal Calculate(string expression)
        {
            string[] postfix = ConvertInfixToPostfix(expression);
            decimal calcValue = CalculatePostfix(postfix);
            return calcValue;
        }

        private string[] ConvertInfixToPostfix(string infix)
        {
            Queue<string> queue = new Queue<string>();
            Stack<string> stack = new Stack<string>();
            infix = AddSpaces(infix);
            string[] infixArray = infix.Split(' ');

            foreach (string elem in infixArray)
            {
                switch (elem)
                {
                    case "^":
                        stack.Push(elem);
                        break;
                    case "*":
                        if (stack.Count > 0)
                        {
                            string op = stack.Peek();
                            if (op == "^" || op == "/")
                                queue.Enqueue(stack.Pop());
                        }
                        stack.Push(elem);
                        break;
                    case "/":
                        if (stack.Count > 0)
                        {
                            string op = stack.Peek();
                            if (op == "^" || op == "*")
                                queue.Enqueue(stack.Pop());
                        }
                        stack.Push(elem);
                        break;
                    case "+":
                        if (stack.Count > 0)
                        {
                            string op = stack.Peek();
                            if (op == "^" || op == "*" || op == "/" || op == "-")
                                queue.Enqueue(stack.Pop());
                        }
                        stack.Push(elem);
                        break;
                    case "-":
                        if (stack.Count > 0)
                        {
                            string op = stack.Peek();
                            if (op == "^" || op == "*" || op == "/" || op == "+")
                                queue.Enqueue(stack.Pop());
                        }
                        stack.Push(elem);
                        break;
                    case "(":
                        stack.Push(elem);
                        break;
                    case ")":
                        while (stack.Peek() != "(")
                            queue.Enqueue(stack.Pop());
                        stack.Pop();
                        break;
                    default:
                        queue.Enqueue(elem);
                        break;
                }
            }
            int count1 = stack.Count;
            for (int i = 0; i < count1; i++)
                queue.Enqueue(stack.Pop());


            int count2 = queue.Count;
            string[] postfix = new string[count2];
            for (int j = 0; j < count2; j++)
                postfix[j] = queue.Dequeue();

            return postfix;
        }

        private decimal CalculatePostfix(string[] postfix)
        {
            Stack<decimal> stack = new Stack<decimal>();
            decimal number = decimal.Zero;

            foreach (string elem in postfix)
            {
                if (decimal.TryParse(elem, out number))
                    stack.Push(number);
                else
                {
                    switch (elem)
                    {
                        case "^":
                            number = stack.Pop();
                            stack.Push(Exponent(stack.Pop(), number));
                            break;
                        case "*":
                            stack.Push(Multiplication(stack.Pop(), stack.Pop()));
                            break;
                        case "/":
                            number = stack.Pop();
                            stack.Push(Division(stack.Pop(), number));
                            break;
                        case "+":
                            stack.Push(Addition(stack.Pop(), stack.Pop()));
                            break;
                        case "-":
                            number = stack.Pop();
                            stack.Push(Subtraction(stack.Pop(), number));
                            break;
                        default:
                            Console.WriteLine("I have an error");
                            break;
                    }
                }
            }
            return stack.Pop();
        }

        private string AddSpaces(string expression)
        {
            string exp = "";
            int counter = 0;
            int lastChar = expression.Length - 1;
            char previous = ' ';

            foreach (char c in expression)
            {
                if ((!Char.IsNumber(c)) && (c != ' '))
                {
                    if (counter == 0)
                    {
                        if (c != '-')
                            exp = exp + c + " ";
                        else
                            exp = exp + c;
                    }
                    else if (counter == lastChar)
                    {
                        if (!Char.IsNumber(previous))
                            exp = exp + c;
                        else
                            exp = exp + " " + c;
                    }
                    else
                    {
                        if (!Char.IsNumber(previous))
                            if ((c == '-') && (previous == '^' || previous == '*' || previous == '/' || previous == '+' || previous == '(' || previous == ')'))
                                exp = exp + c;
                            else
                                exp = exp + c + " ";
                        else
                            exp = exp + " " + c + " ";
                    }

                    previous = c;
                }
                else
                    if (c != ' ')
                    {
                        exp += c;
                        previous = c;
                    }

                counter++;
            }
            return exp;
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

        private decimal Exponent(decimal num1, decimal num2)
        {
            if (num2 == 0)
                return 1;
            else if (num2 == 1)
                return num1;
            else
            {
                decimal num = num1;
                for (int i = 2; i <= num2; i++)
                    num = num * num1;

                return num;
            }
        }
        //private decimal Sum { get; set; }
        //private decimal Operand { get; set; }

        //public decimal Calculate(string expression)
        //{
        //    string[] expArray = expression.Split(' ');

        //    //If there is only 1 element in the expression - return that value 
        //    if (expArray.Length == 1)
        //        return Convert.ToDecimal(expArray[0]);

        //    //Use a List<> to store values because I will need to manipulate the size of the List
        //    List<string> expList = new List<string>();
        //    foreach (string elem in expArray)
        //        expList.Add(elem);

        //    //Go through the List and perform all multiplications and divisions, resize List as necessary
        //    int counter = expList.Count;
        //    for (int x = 1; x < counter; x++)
        //    {
        //        string oper = expList[x];
        //        if (oper == "*" || oper == "/")
        //        {
        //            switch (oper)
        //            {
        //                case "*":
        //                    Operand = Multiplication(Convert.ToDecimal(expList[x - 1]), Convert.ToDecimal(expList[x + 1]));
        //                    expList.RemoveAt(x + 1);
        //                    expList.RemoveAt(x);
        //                    expList.RemoveAt(x - 1);
        //                    expList.Insert(x - 1, Operand.ToString());
        //                    x--;
        //                    counter = expList.Count;
        //                    break;
        //                case "/":
        //                    Operand = Division(System.Convert.ToDecimal(expList[x - 1]), System.Convert.ToDecimal(expList[x + 1]));
        //                    expList.RemoveAt(x + 1);
        //                    expList.RemoveAt(x);
        //                    expList.RemoveAt(x - 1);
        //                    expList.Insert(x - 1, Operand.ToString());
        //                    x--;
        //                    counter = expList.Count;
        //                    break;
        //            }
        //        }
        //    }

        //    //If the size of the List is 1, meaning all operators were multiplication and/or division, return the value
        //    Sum = Convert.ToDecimal(expList[0]);
        //    if (expList.Count == 1)
        //        return Sum;

        //    //Cycle through all elements of the List and perform addition/subtraction
        //    counter = expList.Count;
        //    for (int y = 1; y < counter; y++)
        //    {
        //        string oper = expList[y];
        //        if (oper == "+" || oper == "-")
        //        {
        //            switch (oper)
        //            {
        //                case "+":
        //                    Sum = Addition(Sum, Convert.ToDecimal(expList[y + 1]));
        //                    break;
        //                case "-":
        //                    Sum = Subtraction(Sum, Convert.ToDecimal(expList[y + 1]));
        //                    break;
        //            }
        //        }
        //    }

        //    return Sum;
        //}

    }
}