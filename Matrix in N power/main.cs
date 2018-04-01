using System;

namespace Fozzy
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] matrix = { 
                { 0, 1, 0, 0 }, 
                { 0, 0, 1, 0 }, 
                { 0, 0, 0, 1 }, 
                { 0, 0, 0, 0 }
            }; // матриця яку підносимо до степеня
            int[,] matrix_started = matrix; // початкова матриця яка не змінюватиметься
            Console.WriteLine("Start matrix: ");
            for (int i = 0; i <= 3; i++) // проходи по рядках отриманої матриці
            {
                for (int j = 0; j <= 3; j++) // проходи по стовбцях отриманої матриці
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("*******");
            Actions steps = new Actions();
            for (int i = 2; i <= 3; i++)
            {
                Console.WriteLine("Step: {0}", i);
                steps.Power(ref matrix, matrix_started);
                Console.WriteLine("*******");
            }
            Console.ReadLine();
        }
    }

    class Actions
    {
        #region Бізнес-логіка
        public void Power(ref int[,] matrix, int[,] matrix_started)
        {
            int[,] matrix_copy = new int[4, 4]; // оголошення результуючої матриці
            // піднесення матриці з кешу до квадрату
            for (int i = 0; i <= 3; i++) // проходи по рядках лівої матриці
            {
                for (int j = 0; j <= 3; j++) // проходи по стовпцях правої матриці
                {
                    for (int t = 0; t <= 3; t++) // проходи по елементах поточного стовпця правої матриці
                    {
                        matrix_copy[i, j] += matrix[i, t] * matrix_started[t, j]; // Сумування добутків елементів матриці
                    }
                }
            }
            matrix = matrix_copy;
            Output(matrix);
        }
        #endregion

        #region Вивід результату
        private void Output(int[,] matrix_output)
        {
            for (int i = 0; i <= 3; i++) // проходи по рядках отриманої матриці
            {
                for (int j = 0; j <= 3; j++) // проходи по стовбцях отриманої матриці
                {
                    Console.Write(matrix_output[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
        #endregion
    }
}
