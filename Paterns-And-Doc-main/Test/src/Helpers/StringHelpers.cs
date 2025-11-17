using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Test.src.Helpers
{
    /// <summary>
    /// Утилитарный класс для работы со строками.
    /// Содержит полезные методы для решения различных задач со строками.
    /// </summary>
    public static class StringHelpers
    {
        /// <summary>
        /// Переворачивает строку.
        /// Временная сложность: O(n), где n - длина строки.
        /// Пространственная сложность: O(n).
        /// </summary>
        /// <param name="input">Строка для переворота.</param>
        /// <returns>Перевёрнутая строка.</returns>
        public static string Reverse(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Преобразуем строку в массив символов для манипуляций
            char[] chars = input.ToCharArray();
            int left = 0;
            int right = chars.Length - 1;

            // Используем технику двух указателей: меняем символы с обеих сторон к центру
            while (left < right)
            {
                char temp = chars[left];
                chars[left] = chars[right];
                chars[right] = temp;
                left++;
                right--;
            }

            return new string(chars);
        }

        /// <summary>
        /// Проверяет, является ли строка палиндромом (читается одинаково слева направо и справа налево).
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="input">Строка для проверки.</param>
        /// <param name="ignoreCase">Игнорировать регистр букв (по умолчанию true).</param>
        /// <returns>True, если строка является палиндромом.</returns>
        public static bool IsPalindrome(string input, bool ignoreCase = true)
        {
            if (string.IsNullOrEmpty(input))
                return true;

            int left = 0;
            int right = input.Length - 1;

            while (left < right)
            {
                char leftChar = ignoreCase ? char.ToLower(input[left]) : input[left];
                char rightChar = ignoreCase ? char.ToLower(input[right]) : input[right];

                if (leftChar != rightChar)
                    return false;

                left++;
                right--;
            }

            return true;
        }

        /// <summary>
        /// Находит первый неповторяющийся символ в строке.
        /// Временная сложность: O(n), где n - длина строки.
        /// Пространственная сложность: O(k), где k - количество уникальных символов.
        /// </summary>
        /// <param name="input">Строка для поиска.</param>
        /// <returns>Первый неповторяющийся символ или null, если такого нет.</returns>
        public static char? FirstNonRepeatingChar(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;

            // Словарь для подсчёта частоты каждого символа
            Dictionary<char, int> charCount = new Dictionary<char, int>();

            // Первый проход: считаем частоту каждого символа
            foreach (char c in input)
            {
                if (charCount.ContainsKey(c))
                    charCount[c]++;
                else
                    charCount[c] = 1;
            }

            // Второй проход: находим первый символ с частотой = 1
            foreach (char c in input)
            {
                if (charCount[c] == 1)
                    return c;
            }

            return null;
        }

        /// <summary>
        /// Подсчитывает количество слов в строке.
        /// Слова разделены пробелами, табуляциями или символами новой строки.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="input">Строка для подсчёта слов.</param>
        /// <returns>Количество слов в строке.</returns>
        public static int CountWords(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return 0;

            int wordCount = 0;
            bool inWord = false;

            foreach (char c in input)
            {
                if (char.IsWhiteSpace(c))
                {
                    if (inWord)
                    {
                        wordCount++;
                        inWord = false;
                    }
                }
                else
                {
                    inWord = true;
                }
            }

            // Не забываем последнее слово, если строка не заканчивается пробелом
            if (inWord)
                wordCount++;

            return wordCount;
        }

        /// <summary>
        /// Проверяет, являются ли две строки анаграммами (содержат одинаковые символы в разном порядке).
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(k), где k - количество уникальных символов.
        /// </summary>
        /// <param name="str1">Первая строка.</param>
        /// <param name="str2">Вторая строка.</param>
        /// <param name="ignoreCase">Игнорировать регистр (по умолчанию true).</param>
        /// <returns>True, если строки являются анаграммами.</returns>
        public static bool AreAnagrams(string str1, string str2, bool ignoreCase = true)
        {
            if (string.IsNullOrEmpty(str1) && string.IsNullOrEmpty(str2))
                return true;

            if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2))
                return false;

            if (str1.Length != str2.Length)
                return false;

            // Нормализуем строки для сравнения
            string normalized1 = ignoreCase ? str1.ToLower() : str1;
            string normalized2 = ignoreCase ? str2.ToLower() : str2;

            // Подсчитываем частоту символов в первой строке
            Dictionary<char, int> charCount = new Dictionary<char, int>();

            foreach (char c in normalized1)
            {
                if (charCount.ContainsKey(c))
                    charCount[c]++;
                else
                    charCount[c] = 1;
            }

            // Уменьшаем счётчик для символов во второй строке
            foreach (char c in normalized2)
            {
                if (!charCount.ContainsKey(c))
                    return false;

                charCount[c]--;

                if (charCount[c] < 0)
                    return false;
            }

            // Проверяем, все ли счётчики равны нулю
            return charCount.Values.All(count => count == 0);
        }

        /// <summary>
        /// Удаляет все пробельные символы из строки.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(n).
        /// </summary>
        /// <param name="input">Исходная строка.</param>
        /// <returns>Строка без пробельных символов.</returns>
        public static string RemoveWhitespace(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            StringBuilder result = new StringBuilder();

            foreach (char c in input)
            {
                if (!char.IsWhiteSpace(c))
                    result.Append(c);
            }

            return result.ToString();
        }

        /// <summary>
        /// Подсчитывает количество вхождений подстроки в строку.
        /// Временная сложность: O(n * m), где n - длина строки, m - длина подстроки.
        /// </summary>
        /// <param name="input">Исходная строка.</param>
        /// <param name="substring">Подстрока для поиска.</param>
        /// <param name="ignoreCase">Игнорировать регистр (по умолчанию false).</param>
        /// <returns>Количество вхождений подстроки.</returns>
        public static int CountSubstring(string input, string substring, bool ignoreCase = false)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(substring))
                return 0;

            if (substring.Length > input.Length)
                return 0;

            StringComparison comparison = ignoreCase 
                ? StringComparison.OrdinalIgnoreCase 
                : StringComparison.Ordinal;

            int count = 0;
            int index = 0;

            while ((index = input.IndexOf(substring, index, comparison)) != -1)
            {
                count++;
                index += substring.Length;
            }

            return count;
        }

        /// <summary>
        /// Проверяет, содержит ли строка только цифры.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="input">Строка для проверки.</param>
        /// <returns>True, если строка содержит только цифры.</returns>
        public static bool IsNumeric(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            foreach (char c in input)
            {
                if (!char.IsDigit(c))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Проверяет, содержит ли строка только буквы.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="input">Строка для проверки.</param>
        /// <returns>True, если строка содержит только буквы.</returns>
        public static bool IsAlpha(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            foreach (char c in input)
            {
                if (!char.IsLetter(c))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Проверяет, содержит ли строка только буквы и цифры.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="input">Строка для проверки.</param>
        /// <returns>True, если строка содержит только буквы и цифры.</returns>
        public static bool IsAlphaNumeric(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            foreach (char c in input)
            {
                if (!char.IsLetterOrDigit(c))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Преобразует первую букву каждого слова в заглавную (Title Case).
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(n).
        /// </summary>
        /// <param name="input">Исходная строка.</param>
        /// <returns>Строка с заглавными первыми буквами каждого слова.</returns>
        public static string ToTitleCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            StringBuilder result = new StringBuilder();
            bool isNewWord = true;

            foreach (char c in input)
            {
                if (char.IsWhiteSpace(c))
                {
                    result.Append(c);
                    isNewWord = true;
                }
                else
                {
                    if (isNewWord)
                    {
                        result.Append(char.ToUpper(c));
                        isNewWord = false;
                    }
                    else
                    {
                        result.Append(char.ToLower(c));
                    }
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Удаляет все дубликаты символов из строки, сохраняя первый вхождение каждого символа.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(k), где k - количество уникальных символов.
        /// </summary>
        /// <param name="input">Исходная строка.</param>
        /// <returns>Строка без дубликатов символов.</returns>
        public static string RemoveDuplicates(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            HashSet<char> seen = new HashSet<char>();
            StringBuilder result = new StringBuilder();

            foreach (char c in input)
            {
                if (seen.Add(c)) // Add возвращает true, если элемент был добавлен (не существовал)
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Извлекает все числа из строки.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(m), где m - количество найденных чисел.
        /// </summary>
        /// <param name="input">Исходная строка.</param>
        /// <returns>Список чисел, найденных в строке.</returns>
        public static List<int> ExtractNumbers(string input)
        {
            List<int> numbers = new List<int>();

            if (string.IsNullOrEmpty(input))
                return numbers;

            // Используем регулярное выражение для поиска чисел
            MatchCollection matches = Regex.Matches(input, @"-?\d+");

            foreach (Match match in matches)
            {
                if (int.TryParse(match.Value, out int number))
                {
                    numbers.Add(number);
                }
            }

            return numbers;
        }

        /// <summary>
        /// Проверяет, является ли строка валидным email адресом (простая проверка).
        /// Использует регулярное выражение для базовой валидации.
        /// </summary>
        /// <param name="email">Email адрес для проверки.</param>
        /// <returns>True, если строка выглядит как валидный email.</returns>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Простой паттерн для проверки email (не покрывает все случаи, но достаточно для базовой проверки)
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Подсчитывает количество символов каждого типа в строке.
        /// </summary>
        /// <param name="input">Исходная строка.</param>
        /// <returns>Словарь с количеством символов каждого типа.</returns>
        public static Dictionary<string, int> CountCharTypes(string input)
        {
            var result = new Dictionary<string, int>
            {
                ["letters"] = 0,
                ["digits"] = 0,
                ["whitespace"] = 0,
                ["punctuation"] = 0,
                ["other"] = 0
            };

            if (string.IsNullOrEmpty(input))
                return result;

            foreach (char c in input)
            {
                if (char.IsLetter(c))
                    result["letters"]++;
                else if (char.IsDigit(c))
                    result["digits"]++;
                else if (char.IsWhiteSpace(c))
                    result["whitespace"]++;
                else if (char.IsPunctuation(c))
                    result["punctuation"]++;
                else
                    result["other"]++;
            }

            return result;
        }

        /// <summary>
        /// Демонстрирует работу всех методов класса StringHelpers.
        /// </summary>
        public static void Demonstrate()
        {
            Console.WriteLine("=== Демонстрация методов StringHelpers ===\n");

            // Reverse
            string original = "Hello World";
            Console.WriteLine($"Reverse: '{original}' -> '{Reverse(original)}'");

            // IsPalindrome
            Console.WriteLine($"IsPalindrome('racecar'): {IsPalindrome("racecar")}");
            Console.WriteLine($"IsPalindrome('hello'): {IsPalindrome("hello")}");

            // FirstNonRepeatingChar
            Console.WriteLine($"FirstNonRepeatingChar('programming'): {FirstNonRepeatingChar("programming")}");

            // CountWords
            string sentence = "The quick brown fox jumps over the lazy dog";
            Console.WriteLine($"CountWords: '{sentence}' -> {CountWords(sentence)} words");

            // AreAnagrams
            Console.WriteLine($"AreAnagrams('listen', 'silent'): {AreAnagrams("listen", "silent")}");

            // RemoveWhitespace
            Console.WriteLine($"RemoveWhitespace('Hello World'): '{RemoveWhitespace("Hello World")}'");

            // CountSubstring
            Console.WriteLine($"CountSubstring('hello hello world', 'hello'): {CountSubstring("hello hello world", "hello")}");

            // IsNumeric
            Console.WriteLine($"IsNumeric('12345'): {IsNumeric("12345")}");

            // ToTitleCase
            Console.WriteLine($"ToTitleCase('hello world'): '{ToTitleCase("hello world")}'");

            // RemoveDuplicates
            Console.WriteLine($"RemoveDuplicates('hello'): '{RemoveDuplicates("hello")}'");

            // ExtractNumbers
            string withNumbers = "I have 5 apples and 3 oranges";
            Console.WriteLine($"ExtractNumbers('{withNumbers}'): [{string.Join(", ", ExtractNumbers(withNumbers))}]");

            // IsValidEmail
            Console.WriteLine($"IsValidEmail('test@example.com'): {IsValidEmail("test@example.com")}");

            // CountCharTypes
            var charTypes = CountCharTypes("Hello123 World!");
            Console.WriteLine($"CountCharTypes('Hello123 World!'):");
            foreach (var kvp in charTypes)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
        }
    }
}
