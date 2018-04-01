using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerWCFFozzy
{
    public class Processing : IContract
    {
        public async Task<int> NumberOfFibonacci(int input)
        {
            return await Task<int>.Factory.StartNew(() => {
                DateTime dateTime = DateTime.Now;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Func<int> function = new Func<int>(() => {
                    if (input == 1 || input == 2)
                    {
                        return 1;
                    }
                    else
                    {
                        int first = 1, second = 1, element = 0;
                        input++;
                        Parallel.For(3, input, (int i) => {
                            element = first + second;
                            first = second;
                            second = element;
                        });
                        return second;
                    }
                });
                stopwatch.Stop();
                RequestInformation("Number of fibonacci", stopwatch, dateTime);
                return function();
            });
        }

        public async Task<double> CalculateTheExpression(string input)
        {
            return await Task<double>.Factory.StartNew(() => {
                DateTime dateTime = DateTime.Now;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Func<double> function = new Func<double>(() => {
                    Parse parse = new Parse();
                    double result = parse.Evaluate(input);
                    return result;
                });
                stopwatch.Stop();
                RequestInformation(input, stopwatch, dateTime);
                return function();
            });
        }

        private void RequestInformation(string requestName, Stopwatch stopwatch, DateTime dateTimeStart)
        {
            Console.WriteLine($"Request [{requestName}]:");
            Console.WriteLine($"     Time to receive a request: {dateTimeStart}");
            Console.WriteLine($"     Request processing time: {stopwatch.Elapsed}");
            Console.WriteLine($"     Response time: {DateTime.Now}");
            Console.WriteLine($"     State: Completed");
        }
    }

    public class Parse
    {
        private enum Types { NONE, DELIMITER, NUMBER };
        private string exp;
        private int expIdx;
        private string token;
        private Types tokType;

        public double Evaluate(string expstr)
        {
            double result;
            exp = expstr;
            expIdx = 0;
            GetToken();
            if (token == "")
                return 0.0;
            result = EvalExp1(out result);
            if (token != "")
                return 0.0;
            return result;

        }

        private double EvalExp1(out double result)
        {
            string op;
            double partialResult;
            result = EvalExp2(out result);
            while ((op = token) == "+" || op == "-")
            {
                GetToken();
                partialResult = EvalExp2(out partialResult);
                if (op == "-")
                    result -= partialResult;
                else if (op == "+")
                    result += partialResult;
            }
            return result;
        }

        private double EvalExp2(out double result)
        {
            string op;
            double partialResult = 0.0;
            result = EvalExp3(out result);
            while ((op = token) == "*" || op == "/" || op == "%")
            {
                GetToken();
                partialResult = EvalExp3(out partialResult);
                if (op == "*")
                    result *= partialResult;
                else if (op == "/")
                    result /= partialResult;
                else if (op == "%")
                    result %= partialResult;
            }
            return result;
        }

        private double EvalExp3(out double result)
        {
            double partialResult, ex;
            int t;
            result = EvalExp4(out result);
            if (token == "^")
            {
                GetToken();
                partialResult = EvalExp3(out partialResult);
                ex = result;
                if (partialResult == 0.0)
                    return 1.0;
                for (t = (int)partialResult - 1; t > 0; t--)
                    result *= ex;
            }
            return result;
        }

        private double EvalExp4(out double result)
        {
            string op = "";
            if ((tokType == Types.DELIMITER) && token == "+" || token == "-")
            {
                op = token;
                GetToken();
            }
            result = EvalExp5(out result);
            if (op == "-")
                result = -result;
            return result;
        }

        private double EvalExp5(out double result)
        {
            if (token == "(")
            {
                GetToken();
                result = EvalExp1(out result);
                GetToken();
                return result;
            }
            else
                return Atom(out result);
        }

        private double Atom(out double result)
        {
            switch (tokType)
            {
                case Types.NUMBER:
                    result = Convert.ToDouble(token);
                    GetToken();
                    return result;
                default:
                    result = 0.0;
                    return result;
            }
        }

        private void GetToken()
        {
            tokType = Types.NONE;
            token = "";
            if (expIdx == exp.Length)
                return;
            while (expIdx < exp.Length && exp[expIdx] == ' ')
                expIdx++;
            if (expIdx == exp.Length)
                return;
            if (IsDelim(exp[expIdx]))
            {
                token += exp[expIdx];
                expIdx++;
                tokType = Types.DELIMITER;
            }
            else if (Char.IsDigit(exp[expIdx]))
            {
                while (!IsDelim(exp[expIdx]))
                {
                    token += exp[expIdx];
                    expIdx++;
                    if (expIdx >= exp.Length)
                        break;
                }
                tokType = Types.NUMBER;
            }
        }

        private bool IsDelim(char c)
        {
            if ("+-/*%^=()".Contains(c))
                return true;
            return false;
        }
    }
}