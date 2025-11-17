using System;
using System.Collections.Generic;
using System.Linq;

namespace Test.src.Helpers
{
    /// <summary>
    /// Утилитарный класс для работы с числами (int, long, double).
    /// Содержит полезные методы для решения различных задач с числами.
    /// </summary>
    public static class IntHelpers
    {
        /// <summary>
        /// Проверяет, является ли число простым (делится только на 1 и на себя).
        /// Временная сложность: O(√n).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="number">Число для проверки.</param>
        /// <returns>True, если число простое.</returns>
        public static bool IsPrime(int number)
        {
            // Числа меньше 2 не являются простыми
            if (number < 2)
                return false;

            // 2 - единственное чётное простое число
            if (number == 2)
                return true;

            // Чётные числа больше 2 не являются простыми
            if (number % 2 == 0)
                return false;

            // Проверяем делимость на нечётные числа до √n
            // Если число делится на какое-то число, большее √n, то частное будет меньше √n
            // и мы уже проверили это частное ранее
            int sqrt = (int)Math.Sqrt(number);
            for (int i = 3; i <= sqrt; i += 2)
            {
                if (number % i == 0)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Вычисляет факториал числа n (n! = 1 * 2 * 3 * ... * n).
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(1) для итеративного, O(n) для рекурсивного.
        /// </summary>
        /// <param name="n">Число для вычисления факториала.</param>
        /// <returns>Факториал числа n.</returns>
        /// <exception cref="ArgumentException">Выбрасывается если n отрицательное или слишком большое.</exception>
        public static long Factorial(int n)
        {
            if (n < 0)
                throw new ArgumentException("Факториал определён только для неотрицательных чисел.", nameof(n));

            if (n > 20)
                throw new ArgumentException("Результат слишком большой для типа long.", nameof(n));

            // Факториал 0 и 1 равен 1
            if (n <= 1)
                return 1;

            // Итеративное вычисление факториала
            long result = 1;
            for (int i = 2; i <= n; i++)
            {
                result *= i;
            }

            return result;
        }

        /// <summary>
        /// Вычисляет числа Фибоначчи до n-го числа.
        /// Последовательность Фибоначчи: 0, 1, 1, 2, 3, 5, 8, 13, ...
        /// Каждое число равно сумме двух предыдущих.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(n).
        /// </summary>
        /// <param name="n">Количество чисел Фибоначчи для генерации.</param>
        /// <returns>Массив первых n чисел Фибоначчи.</returns>
        public static long[] Fibonacci(int n)
        {
            if (n <= 0)
                return Array.Empty<long>();

            if (n == 1)
                return new long[] { 0 };

            long[] result = new long[n];
            result[0] = 0;
            result[1] = 1;

            // Вычисляем каждое следующее число как сумму двух предыдущих
            for (int i = 2; i < n; i++)
            {
                result[i] = result[i - 1] + result[i - 2];
            }

            return result;
        }

        /// <summary>
        /// Вычисляет n-е число Фибоначчи (оптимизированная версия без массива).
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="n">Индекс числа Фибоначчи (0-based).</param>
        /// <returns>n-е число Фибоначчи.</returns>
        public static long FibonacciNth(int n)
        {
            if (n < 0)
                throw new ArgumentException("Индекс должен быть неотрицательным.", nameof(n));

            if (n == 0)
                return 0;

            if (n == 1)
                return 1;

            long prev = 0;
            long current = 1;

            // Итеративно вычисляем n-е число, сохраняя только два предыдущих
            for (int i = 2; i <= n; i++)
            {
                long next = prev + current;
                prev = current;
                current = next;
            }

            return current;
        }

        /// <summary>
        /// Проверяет, является ли число чётным.
        /// </summary>
        /// <param name="number">Число для проверки.</param>
        /// <returns>True, если число чётное.</returns>
        public static bool IsEven(int number)
        {
            return number % 2 == 0;
        }

        /// <summary>
        /// Проверяет, является ли число нечётным.
        /// </summary>
        /// <param name="number">Число для проверки.</param>
        /// <returns>True, если число нечётное.</returns>
        public static bool IsOdd(int number)
        {
            return number % 2 != 0;
        }

        /// <summary>
        /// Вычисляет сумму цифр числа.
        /// Временная сложность: O(log₁₀(n)).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="number">Число для суммирования цифр.</param>
        /// <returns>Сумма всех цифр числа.</returns>
        public static int SumOfDigits(int number)
        {
            // Используем абсолютное значение для корректной работы с отрицательными числами
            number = Math.Abs(number);
            int sum = 0;

            // Извлекаем каждую цифру делением на 10
            while (number > 0)
            {
                sum += number % 10; // Получаем последнюю цифру
                number /= 10;       // Удаляем последнюю цифру
            }

            return sum;
        }

        /// <summary>
        /// Переворачивает число (123 -> 321).
        /// Временная сложность: O(log₁₀(n)).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="number">Число для переворота.</param>
        /// <returns>Перевёрнутое число.</returns>
        public static int Reverse(int number)
        {
            bool isNegative = number < 0;
            number = Math.Abs(number);
            int reversed = 0;

            // Строим перевёрнутое число, извлекая цифры справа налево
            while (number > 0)
            {
                reversed = reversed * 10 + number % 10; // Добавляем последнюю цифру
                number /= 10;                           // Удаляем последнюю цифру
            }

            return isNegative ? -reversed : reversed;
        }

        /// <summary>
        /// Проверяет, является ли число палиндромом (читается одинаково слева направо и справа налево).
        /// Временная сложность: O(log₁₀(n)).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="number">Число для проверки.</param>
        /// <returns>True, если число является палиндромом.</returns>
        public static bool IsPalindrome(int number)
        {
            return number == Reverse(number);
        }

        /// <summary>
        /// Вычисляет наибольший общий делитель (НОД) двух чисел используя алгоритм Евклида.
        /// Временная сложность: O(log(min(a, b))).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="a">Первое число.</param>
        /// <param name="b">Второе число.</param>
        /// <returns>НОД чисел a и b.</returns>
        public static int GreatestCommonDivisor(int a, int b)
        {
            // Используем абсолютные значения
            a = Math.Abs(a);
            b = Math.Abs(b);

            // Алгоритм Евклида: НОД(a, b) = НОД(b, a mod b)
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }

            return a;
        }

        /// <summary>
        /// Вычисляет наименьшее общее кратное (НОК) двух чисел.
        /// НОК(a, b) = (a * b) / НОД(a, b)
        /// </summary>
        /// <param name="a">Первое число.</param>
        /// <param name="b">Второе число.</param>
        /// <returns>НОК чисел a и b.</returns>
        public static long LeastCommonMultiple(int a, int b)
        {
            if (a == 0 || b == 0)
                return 0;

            int gcd = GreatestCommonDivisor(a, b);
            return (long)Math.Abs(a) * Math.Abs(b) / gcd;
        }

        /// <summary>
        /// Проверяет, является ли число степенью двойки.
        /// Временная сложность: O(1).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="number">Число для проверки.</param>
        /// <returns>True, если число является степенью двойки.</returns>
        public static bool IsPowerOfTwo(int number)
        {
            if (number <= 0)
                return false;

            // Число является степенью двойки, если в его двоичном представлении только один бит установлен
            // (number & (number - 1)) == 0 для степеней двойки
            return (number & (number - 1)) == 0;
        }

        /// <summary>
        /// Подсчитывает количество установленных битов в двоичном представлении числа (popcount).
        /// Временная сложность: O(log₂(n)).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="number">Число для подсчёта битов.</param>
        /// <returns>Количество установленных битов.</returns>
        public static int CountSetBits(int number)
        {
            int count = 0;

            // Проверяем каждый бит
            while (number != 0)
            {
                count += number & 1; // Добавляем 1, если последний бит установлен
                number >>= 1;        // Сдвигаем вправо на 1 бит
            }

            return count;
        }

        /// <summary>
        /// Проверяет, является ли число числом Армстронга (сумма цифр в степени количества цифр равна самому числу).
        /// Пример: 153 = 1³ + 5³ + 3³ = 1 + 125 + 27 = 153
        /// </summary>
        /// <param name="number">Число для проверки.</param>
        /// <returns>True, если число является числом Армстронга.</returns>
        public static bool IsArmstrongNumber(int number)
        {
            if (number < 0)
                return false;

            int original = number;
            int digitCount = 0;
            int temp = number;

            // Подсчитываем количество цифр
            while (temp > 0)
            {
                digitCount++;
                temp /= 10;
            }

            // Вычисляем сумму цифр в степени количества цифр
            int sum = 0;
            temp = number;
            while (temp > 0)
            {
                int digit = temp % 10;
                sum += (int)Math.Pow(digit, digitCount);
                temp /= 10;
            }

            return sum == original;
        }

        /// <summary>
        /// Генерирует список всех простых чисел до указанного предела (решето Эратосфена).
        /// Временная сложность: O(n log log n).
        /// Пространственная сложность: O(n).
        /// </summary>
        /// <param name="limit">Верхний предел для поиска простых чисел.</param>
        /// <returns>Список всех простых чисел до limit (включительно).</returns>
        public static List<int> GeneratePrimes(int limit)
        {
            if (limit < 2)
                return new List<int>();

            // Решето Эратосфена: изначально считаем все числа простыми
            bool[] isPrime = new bool[limit + 1];
            for (int i = 2; i <= limit; i++)
            {
                isPrime[i] = true;
            }

            // Помечаем составные числа
            for (int i = 2; i * i <= limit; i++)
            {
                if (isPrime[i])
                {
                    // Помечаем все кратные i как составные
                    for (int j = i * i; j <= limit; j += i)
                    {
                        isPrime[j] = false;
                    }
                }
            }

            // Собираем все простые числа в список
            List<int> primes = new List<int>();
            for (int i = 2; i <= limit; i++)
            {
                if (isPrime[i])
                    primes.Add(i);
            }

            return primes;
        }

        /// <summary>
        /// Демонстрирует работу всех методов класса IntHelpers.
        /// </summary>
        public static void Demonstrate()
        {
            Console.WriteLine("=== Демонстрация методов IntHelpers ===\n");

            // IsPrime
            Console.WriteLine($"IsPrime(17): {IsPrime(17)}");
            Console.WriteLine($"IsPrime(15): {IsPrime(15)}");

            // Factorial
            Console.WriteLine($"Factorial(5): {Factorial(5)}");

            // Fibonacci
            Console.WriteLine($"Fibonacci(10): [{string.Join(", ", Fibonacci(10))}]");
            Console.WriteLine($"FibonacciNth(10): {FibonacciNth(10)}");

            // IsEven/IsOdd
            Console.WriteLine($"IsEven(10): {IsEven(10)}");
            Console.WriteLine($"IsOdd(11): {IsOdd(11)}");

            // SumOfDigits
            Console.WriteLine($"SumOfDigits(12345): {SumOfDigits(12345)}");

            // Reverse
            Console.WriteLine($"Reverse(12345): {Reverse(12345)}");

            // IsPalindrome
            Console.WriteLine($"IsPalindrome(121): {IsPalindrome(121)}");
            Console.WriteLine($"IsPalindrome(123): {IsPalindrome(123)}");

            // GreatestCommonDivisor
            Console.WriteLine($"GreatestCommonDivisor(48, 18): {GreatestCommonDivisor(48, 18)}");

            // LeastCommonMultiple
            Console.WriteLine($"LeastCommonMultiple(12, 18): {LeastCommonMultiple(12, 18)}");

            // IsPowerOfTwo
            Console.WriteLine($"IsPowerOfTwo(16): {IsPowerOfTwo(16)}");
            Console.WriteLine($"IsPowerOfTwo(15): {IsPowerOfTwo(15)}");

            // CountSetBits
            Console.WriteLine($"CountSetBits(15): {CountSetBits(15)}"); // 1111 в двоичной = 4 бита

            // IsArmstrongNumber
            Console.WriteLine($"IsArmstrongNumber(153): {IsArmstrongNumber(153)}");

            // GeneratePrimes
            var primes = GeneratePrimes(50);
            Console.WriteLine($"GeneratePrimes(50): [{string.Join(", ", primes)}]");
        }
    }
}
