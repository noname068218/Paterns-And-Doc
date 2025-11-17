using System;
using System.Collections.Generic;
using System.Linq;

namespace Test.src.Helpers
{
    /// <summary>
    /// Утилитарный класс для работы с массивами.
    /// Содержит полезные методы для решения различных задач с массивами.
    /// </summary>
    public static class ArrayHelpers
    {
        /// <summary>
        /// Находит максимальный элемент в массиве.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="array">Массив для поиска.</param>
        /// <returns>Максимальный элемент.</returns>
        /// <exception cref="ArgumentException">Выбрасывается если массив пустой или null.</exception>
        public static int FindMax(int[] array)
        {
            if (array == null || array.Length == 0)
                throw new ArgumentException("Массив не может быть пустым или null.", nameof(array));

            int max = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] > max)
                    max = array[i];
            }

            return max;
        }

        /// <summary>
        /// Находит минимальный элемент в массиве.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="array">Массив для поиска.</param>
        /// <returns>Минимальный элемент.</returns>
        /// <exception cref="ArgumentException">Выбрасывается если массив пустой или null.</exception>
        public static int FindMin(int[] array)
        {
            if (array == null || array.Length == 0)
                throw new ArgumentException("Массив не может быть пустым или null.", nameof(array));

            int min = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] < min)
                    min = array[i];
            }

            return min;
        }

        /// <summary>
        /// Находит минимальный и максимальный элементы в массиве за один проход.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="array">Массив для поиска.</param>
        /// <returns>Кортеж (min, max).</returns>
        public static (int min, int max) FindMinMax(int[] array)
        {
            if (array == null || array.Length == 0)
                throw new ArgumentException("Массив не может быть пустым или null.", nameof(array));

            int min = array[0];
            int max = array[0];

            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] < min)
                    min = array[i];
                else if (array[i] > max)
                    max = array[i];
            }

            return (min, max);
        }

        /// <summary>
        /// Вычисляет сумму всех элементов массива.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="array">Массив для суммирования.</param>
        /// <returns>Сумма всех элементов.</returns>
        public static long Sum(int[] array)
        {
            if (array == null || array.Length == 0)
                return 0;

            long sum = 0;
            foreach (int item in array)
            {
                sum += item;
            }

            return sum;
        }

        /// <summary>
        /// Вычисляет среднее арифметическое элементов массива.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="array">Массив для вычисления среднего.</param>
        /// <returns>Среднее арифметическое.</returns>
        public static double Average(int[] array)
        {
            if (array == null || array.Length == 0)
                return 0;

            return (double)Sum(array) / array.Length;
        }

        /// <summary>
        /// Удаляет дубликаты из массива, сохраняя порядок первого вхождения.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(k), где k - количество уникальных элементов.
        /// </summary>
        /// <param name="array">Массив с возможными дубликатами.</param>
        /// <returns>Массив без дубликатов.</returns>
        public static int[] RemoveDuplicates(int[] array)
        {
            if (array == null || array.Length == 0)
                return Array.Empty<int>();

            HashSet<int> seen = new HashSet<int>();
            List<int> result = new List<int>();

            foreach (int item in array)
            {
                if (seen.Add(item)) // Добавляем только если элемент ещё не встречался
                {
                    result.Add(item);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Находит второй по величине элемент в массиве.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="array">Массив для поиска.</param>
        /// <returns>Второй по величине элемент или int.MinValue если не найден.</returns>
        public static int FindSecondLargest(int[] array)
        {
            if (array == null || array.Length < 2)
                return int.MinValue;

            int max = FindMax(array);
            int secondMax = int.MinValue;
            bool found = false;

            foreach (int item in array)
            {
                if (item < max && item > secondMax)
                {
                    secondMax = item;
                    found = true;
                }
            }

            return found ? secondMax : int.MinValue;
        }

        /// <summary>
        /// Переворачивает массив на месте (in-place).
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="array">Массив для переворота.</param>
        public static void Reverse(int[] array)
        {
            if (array == null || array.Length <= 1)
                return;

            int left = 0;
            int right = array.Length - 1;

            while (left < right)
            {
                int temp = array[left];
                array[left] = array[right];
                array[right] = temp;
                left++;
                right--;
            }
        }

        /// <summary>
        /// Создаёт новый перевёрнутый массив, не изменяя исходный.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(n).
        /// </summary>
        /// <param name="array">Исходный массив.</param>
        /// <returns>Новый перевёрнутый массив.</returns>
        public static int[] ReverseCopy(int[] array)
        {
            if (array == null || array.Length == 0)
                return Array.Empty<int>();

            int[] reversed = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                reversed[i] = array[array.Length - 1 - i];
            }

            return reversed;
        }

        /// <summary>
        /// Вращает массив вправо на k позиций.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="array">Массив для вращения.</param>
        /// <param name="k">Количество позиций для вращения вправо.</param>
        public static void RotateRight(int[] array, int k)
        {
            if (array == null || array.Length == 0 || k == 0)
                return;

            int n = array.Length;
            k = k % n; // Нормализуем k

            if (k == 0)
                return;

            // Используем трёхкратный переворот
            Reverse(array);                    // Переворачиваем весь массив
            ReverseSegment(array, 0, k - 1);   // Переворачиваем первые k элементов
            ReverseSegment(array, k, n - 1);   // Переворачиваем оставшиеся элементы
        }

        /// <summary>
        /// Вспомогательный метод для переворота сегмента массива.
        /// </summary>
        private static void ReverseSegment(int[] array, int start, int end)
        {
            while (start < end)
            {
                int temp = array[start];
                array[start] = array[end];
                array[end] = temp;
                start++;
                end--;
            }
        }

        /// <summary>
        /// Находит два числа в массиве, которые в сумме дают target (индексы).
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(n).
        /// </summary>
        /// <param name="array">Массив для поиска.</param>
        /// <param name="target">Целевая сумма.</param>
        /// <returns>Кортеж с индексами двух чисел или (-1, -1) если не найдено.</returns>
        public static (int index1, int index2) TwoSum(int[] array, int target)
        {
            if (array == null || array.Length < 2)
                return (-1, -1);

            Dictionary<int, int> seen = new Dictionary<int, int>();

            for (int i = 0; i < array.Length; i++)
            {
                int complement = target - array[i];

                if (seen.ContainsKey(complement))
                {
                    return (seen[complement], i);
                }

                seen[array[i]] = i;
            }

            return (-1, -1);
        }

        /// <summary>
        /// Проверяет, содержит ли массив указанное значение.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="array">Массив для поиска.</param>
        /// <param name="value">Значение для поиска.</param>
        /// <returns>True, если значение найдено.</returns>
        public static bool Contains(int[] array, int value)
        {
            if (array == null || array.Length == 0)
                return false;

            foreach (int item in array)
            {
                if (item == value)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Подсчитывает количество вхождений значения в массиве.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="array">Массив для подсчёта.</param>
        /// <param name="value">Значение для подсчёта.</param>
        /// <returns>Количество вхождений значения.</returns>
        public static int Count(int[] array, int value)
        {
            if (array == null || array.Length == 0)
                return 0;

            int count = 0;
            foreach (int item in array)
            {
                if (item == value)
                    count++;
            }

            return count;
        }

        /// <summary>
        /// Находит индекс первого вхождения значения в массиве.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="array">Массив для поиска.</param>
        /// <param name="value">Значение для поиска.</param>
        /// <returns>Индекс первого вхождения или -1 если не найдено.</returns>
        public static int IndexOf(int[] array, int value)
        {
            if (array == null || array.Length == 0)
                return -1;

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == value)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Находит все индексы вхождения значения в массиве.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(k), где k - количество вхождений.
        /// </summary>
        /// <param name="array">Массив для поиска.</param>
        /// <param name="value">Значение для поиска.</param>
        /// <returns>Массив индексов всех вхождений.</returns>
        public static int[] AllIndicesOf(int[] array, int value)
        {
            if (array == null || array.Length == 0)
                return Array.Empty<int>();

            List<int> indices = new List<int>();

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == value)
                    indices.Add(i);
            }

            return indices.ToArray();
        }

        /// <summary>
        /// Создаёт подмассив из указанного диапазона индексов.
        /// Временная сложность: O(length).
        /// Пространственная сложность: O(length).
        /// </summary>
        /// <param name="array">Исходный массив.</param>
        /// <param name="startIndex">Начальный индекс (включительно).</param>
        /// <param name="length">Длина подмассива.</param>
        /// <returns>Новый массив с элементами из указанного диапазона.</returns>
        public static int[] SubArray(int[] array, int startIndex, int length)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (startIndex < 0 || startIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex));

            if (length < 0 || startIndex + length > array.Length)
                throw new ArgumentOutOfRangeException(nameof(length));

            int[] result = new int[length];
            Array.Copy(array, startIndex, result, 0, length);

            return result;
        }

        /// <summary>
        /// Демонстрирует работу всех методов класса ArrayHelpers.
        /// </summary>
        public static void Demonstrate()
        {
            Console.WriteLine("=== Демонстрация методов ArrayHelpers ===\n");

            int[] numbers = { 3, 7, 2, 9, 1, 5, 9 };

            // FindMax/FindMin
            Console.WriteLine($"Массив: [{string.Join(", ", numbers)}]");
            Console.WriteLine($"Max: {FindMax(numbers)}");
            Console.WriteLine($"Min: {FindMin(numbers)}");
            var (min, max) = FindMinMax(numbers);
            Console.WriteLine($"MinMax: ({min}, {max})");

            // Sum/Average
            Console.WriteLine($"Sum: {Sum(numbers)}");
            Console.WriteLine($"Average: {Average(numbers):F2}");

            // RemoveDuplicates
            int[] withDuplicates = { 1, 2, 2, 3, 4, 4, 5 };
            Console.WriteLine($"RemoveDuplicates: [{string.Join(", ", RemoveDuplicates(withDuplicates))}]");

            // FindSecondLargest
            Console.WriteLine($"FindSecondLargest: {FindSecondLargest(numbers)}");

            // Reverse
            int[] toReverse = { 1, 2, 3, 4, 5 };
            Console.WriteLine($"Reverse: [{string.Join(", ", toReverse)}] -> ");
            Reverse(toReverse);
            Console.WriteLine($"  [{string.Join(", ", toReverse)}]");

            // RotateRight
            int[] toRotate = { 1, 2, 3, 4, 5 };
            Console.WriteLine($"RotateRight(2): [{string.Join(", ", toRotate)}] -> ");
            RotateRight(toRotate, 2);
            Console.WriteLine($"  [{string.Join(", ", toRotate)}]");

            // TwoSum
            int[] sumArray = { 2, 7, 11, 15 };
            var (i1, i2) = TwoSum(sumArray, 9);
            Console.WriteLine($"TwoSum(9): индексы [{i1}, {i2}], значения [{sumArray[i1]}, {sumArray[i2]}]");

            // Count/IndexOf
            Console.WriteLine($"Count(9): {Count(numbers, 9)}");
            Console.WriteLine($"IndexOf(9): {IndexOf(numbers, 9)}");
            Console.WriteLine($"AllIndicesOf(9): [{string.Join(", ", AllIndicesOf(numbers, 9))}]");

            // SubArray
            Console.WriteLine($"SubArray(2, 3): [{string.Join(", ", SubArray(numbers, 2, 3))}]");
        }
    }
}
