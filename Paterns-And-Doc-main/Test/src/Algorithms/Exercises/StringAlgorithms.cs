using System;
using System.Collections.Generic;
using System.Text;

namespace Test.src.Algorithms.Exercises
{
    /// <summary>
    /// Содержит распространённые алгоритмы и упражнения по работе со строками.
    /// Эти задания часто используются при собеседованиях и в практических приложениях.
    /// </summary>
    public class StringAlgorithms
    {
        /// <summary>
        /// Переворачивает строку с помощью итеративного подхода.
        /// 
        /// Алгоритм:
        /// 1. Преобразовать строку в массив символов
        /// 2. Использовать два указателя: один в начале, другой в конце
        /// 3. Менять местами символы, двигая указатели навстречу друг другу
        /// 4. Преобразовать массив обратно в строку
        /// 
        /// Временная сложность: O(n), где n — длина строки
        /// Пространственная сложность: O(n) из-за массива символов
        /// </summary>
        /// <param name="input">Строка для переворота.</param>
        /// <returns>Перевёрнутая строка.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если input == null.</exception>
        public static string Reverse(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            // Преобразуем строку в массив символов для изменения "на месте"
            char[] chars = input.ToCharArray();

            // Два указателя: с обоих концов идём к центру
            int left = 0; // Указатель с начала
            int right = chars.Length - 1; // Указатель с конца

            // Меняем символы местами, пока указатели не встретятся или не пересекутся
            while (left < right)
            {
                // Меняем символы в позициях left и right
                char temp = chars[left];
                chars[left] = chars[right];
                chars[right] = temp;

                // Двигаем указатели к центру
                left++;
                right--;
            }

            // Преобразуем массив символов обратно в строку
            return new string(chars);
        }

        /// <summary>
        /// Проверяет, является ли строка палиндромом (читается одинаково слева и справа).
        /// 
        /// Примеры: "racecar", "level", "a" — палиндромы.
        /// "hello", "world" — не палиндромы.
        /// 
        /// Временная сложность: O(n)
        /// Пространственная сложность: O(1)
        /// </summary>
        /// <param name="input">Строка для проверки.</param>
        /// <returns>Истина, если строка — палиндром, иначе — ложь.</returns>
        public static bool IsPalindrome(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return true; // Пустая строка считается палиндромом
            }

            // Используем технику двух указателей
            int left = 0;
            int right = input.Length - 1;

            // Сравниваем символы с обоих концов, двигаясь к центру
            while (left < right)
            {
                // Если символы не совпадают — не палиндром
                if (char.ToLower(input[left]) != char.ToLower(input[right]))
                {
                    return false;
                }

                left++;
                right--;
            }

            // Все пары символов совпали — это палиндром
            return true;
        }

        /// <summary>
        /// Находит первый неповторяющийся символ в строке.
        /// 
        /// Алгоритм:
        /// 1. Подсчитать частоту каждого символа с помощью словаря
        /// 2. Ещё раз пройтись по строке, чтобы найти первый символ с количеством = 1
        /// 
        /// Временная сложность: O(n), где n — длина строки
        /// Пространственная сложность: O(k), где k — количество уникальных символов
        /// </summary>
        /// <param name="input">Строка для поиска.</param>
        /// <returns>Первый неповторяющийся символ или null, если все символы повторяются.</returns>
        public static char? FirstNonRepeatingChar(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            // Словарь для подсчёта частот символов
            // Ключ: символ, Значение: количество встречаний
            Dictionary<char, int> charCount = new Dictionary<char, int>();

            // Первый проход: подсчитываем частоты символов
            foreach (char c in input)
            {
                // Если символ уже есть в словаре — увеличиваем счетчик
                // Иначе добавляем с количеством = 1
                if (charCount.ContainsKey(c))
                {
                    charCount[c]++;
                }
                else
                {
                    charCount[c] = 1;
                }
            }

            // Второй проход: ищем первый символ с количеством = 1
            foreach (char c in input)
            {
                if (charCount[c] == 1)
                {
                    return c; // Найден первый неповторяющийся символ
                }
            }

            // Все символы встретились более одного раза
            return null;
        }

        /// <summary>
        /// Считает количество слов в строке.
        /// Слова разделяются пробельными символами.
        /// 
        /// Временная сложность: O(n)
        /// Пространственная сложность: O(1)
        /// </summary>
        /// <param name="input">Строка, в которой нужно посчитать слова.</param>
        /// <returns>Количество слов в строке.</returns>
        public static int CountWords(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return 0;
            }

            int wordCount = 0;
            bool inWord = false; // Флаг, чтобы отслеживать, находимся ли мы внутри слова

            // Проходим по строке посимвольно
            foreach (char c in input)
            {
                // Если текущий символ — пробел/разделитель
                if (char.IsWhiteSpace(c))
                {
                    // Если были внутри слова — дошли до конца слова
                    if (inWord)
                    {
                        wordCount++;
                        inWord = false;
                    }
                }
                // Текущий символ — не пробел, мы внутри слова
                else
                {
                    inWord = true;
                }
            }

            // Не забываем про последнее слово, если строка не заканчивается пробелом
            if (inWord)
            {
                wordCount++;
            }

            return wordCount;
        }

        /// <summary>
        /// Проверяет, являются ли две строки анаграммами (содержат одинаковые символы в разном порядке).
        /// 
        /// Примеры: "listen" и "silent" — анаграммы.
        /// "hello" и "world" — не анаграммы.
        /// 
        /// Алгоритм:
        /// 1. Если длины различаются — это не анаграммы
        /// 2. Подсчитать частоты символов первой строки
        /// 3. Вычесть по символу из второй строки
        /// 4. Если после этого все частоты равны нулю — строки анаграммы
        /// 
        /// Временная сложность: O(n), где n — длина строк
        /// Пространственная сложность: O(k), где k — количество уникальных символов
        /// </summary>
        /// <param name="str1">Первая строка.</param>
        /// <param name="str2">Вторая строка.</param>
        /// <returns>Истина, если строки являются анаграммами; иначе — ложь.</returns>
        public static bool AreAnagrams(string str1, string str2)
        {
            // Пустые или null-строки считаются анаграммами
            if (string.IsNullOrEmpty(str1) && string.IsNullOrEmpty(str2))
            {
                return true;
            }

            // Если одна из строк null/пустая, а другая — нет, это не анаграммы
            if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2))
            {
                return false;
            }

            // Если длины различаются — это не анаграммы
            if (str1.Length != str2.Length)
            {
                return false;
            }

            // Подсчитываем частоты символов в первой строке
            Dictionary<char, int> charCount = new Dictionary<char, int>();

            // Считаем символы первой строки (без учёта регистра)
            foreach (char c in str1.ToLower())
            {
                if (charCount.ContainsKey(c))
                {
                    charCount[c]++;
                }
                else
                {
                    charCount[c] = 1;
                }
            }

            // Вычитаем символы второй строки
            foreach (char c in str2.ToLower())
            {
                // Если символа нет в первой строке — не анаграммы
                if (!charCount.ContainsKey(c))
                {
                    return false;
                }

                // Уменьшаем счётчик
                charCount[c]--;

                // Если счетчик становится отрицательным — символа во второй строке больше
                if (charCount[c] < 0)
                {
                    return false;
                }
            }

            // Проверяем, что у всех символов счетчик стал равен нулю
            foreach (var count in charCount.Values)
            {
                if (count != 0)
                {
                    return false;
                }
            }

            // Все проверки пройдены — это анаграммы
            return true;
        }

        /// <summary>
        /// Демонстрирует работу различных алгоритмов со строками на примерах.
        /// </summary>
        public static void Demonstrate()
        {
            Console.WriteLine("=== Демонстрация алгоритмов со строками ===\n");

            // Переворот строки
            string original = "Hello World";
            string reversed = Reverse(original);
            Console.WriteLine("Переворот строки:");
            Console.WriteLine("Исходная: {0}", original);
            Console.WriteLine("Перевёрнутая: {0}\n", reversed);

            // Проверка на палиндром
            string[] palindromes = { "racecar", "level", "hello", "A", "" };
            Console.WriteLine("Проверка на палиндром:");
            foreach (string str in palindromes)
            {
                Console.WriteLine("'{0}' — палиндром: {1}", str, IsPalindrome(str));
            }
            Console.WriteLine();

            // Первый неповторяющийся символ
            string testString = "programming";
            char? firstNonRepeat = FirstNonRepeatingChar(testString);
            Console.WriteLine("Первый неповторяющийся символ:");
            Console.WriteLine("Строка: {0}", testString);
            Console.WriteLine("Первый уникальный: {0}\n", firstNonRepeat?.ToString() ?? "Нет");

            // Подсчёт слов
            string sentence = "The quick brown fox jumps over the lazy dog";
            int wordCount = CountWords(sentence);
            Console.WriteLine("Подсчёт количества слов:");
            Console.WriteLine("Предложение: {0}", sentence);
            Console.WriteLine("Слов: {0}\n", wordCount);

            // Проверка на анаграммы
            var anagramPairs = new[]
            {
                ("listen", "silent"),
                ("hello", "world"),
                ("rail safety", "fairy tales"),
                ("Tom Marvolo Riddle", "I am Lord Voldemort")
            };

            Console.WriteLine("Проверка анаграмм:");
            foreach (var (str1, str2) in anagramPairs)
            {
                Console.WriteLine("'{0}' и '{1}': {2}", 
                    str1, str2, AreAnagrams(str1, str2));
            }
        }
    }
}
