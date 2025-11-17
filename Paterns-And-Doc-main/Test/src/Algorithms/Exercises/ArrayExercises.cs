using System;
using System.Collections.Generic;
using System.Linq;

namespace Test.src.Algorithms.Exercises
{
    /// <summary>
    /// Содержит распространённые алгоритмы и упражнения для работы с массивами.
    /// Эти упражнения помогают понять работу с массивами и паттерны решения задач.
    /// </summary>
    public class ArrayExercises
    {
        /// <summary>
        /// Находит максимальный элемент в массиве.
        /// 
        /// Алгоритм: Линейный поиск — пройти по массиву один раз, отслеживая максимум.
        /// 
        /// Временная сложность: O(n), где n — длина массива
        /// Пространственная сложность: O(1)
        /// </summary>
        /// <param name="array">Массив для поиска.</param>
        /// <returns>Максимальное значение в массиве.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если массив null или пустой.</exception>
        public static int FindMax(int[] array)
        {
            if (array == null || array.Length == 0)
            {
                throw new ArgumentException("Array cannot be null or empty.", nameof(array));
            }

            // Инициализируем max первым элементом
            // Это предполагает, что в массиве минимум один элемент (гарантируется проверкой выше)
            int max = array[0];

            // Проходим по оставшимся элементам
            // Начинаем с индекса 1, так как индекс 0 уже в max
            for (int i = 1; i < array.Length; i++)
            {
                // Если текущий элемент больше max, обновляем max
                if (array[i] > max)
                {
                    max = array[i];
                }
            }

            return max;
        }

        /// <summary>
        /// Находит минимальный и максимальный элементы в массиве за один проход.
        /// Более эффективно, чем вызывать FindMax и FindMin по отдельности.
        /// 
        /// Временная сложность: O(n) — один проход по массиву
        /// Пространственная сложность: O(1)
        /// </summary>
        /// <param name="array">Массив для поиска.</param>
        /// <returns>Кортеж из значений (min, max).</returns>
        /// <exception cref="ArgumentException">Выбрасывается, если массив null или пустой.</exception>
        public static (int min, int max) FindMinMax(int[] array)
        {
            if (array == null || array.Length == 0)
            {
                throw new ArgumentException("Array cannot be null or empty.", nameof(array));
            }

            // Инициализируем и min, и max первым элементом
            int min = array[0];
            int max = array[0];

            // Проходим по остальным элементам, обновляя одновременно min и max
            for (int i = 1; i < array.Length; i++)
            {
                // Обновляем min, если текущий элемент меньше
                if (array[i] < min)
                {
                    min = array[i];
                }
                // Обновляем max, если текущий элемент больше
                else if (array[i] > max)
                {
                    max = array[i];
                }
            }

            return (min, max);
        }

        /// <summary>
        /// Находит второй по величине элемент в массиве.
        /// 
        /// Алгоритм:
        /// 1. Найти максимальный элемент
        /// 2. Найти максимальный среди элементов, меньших максимального
        /// 
        /// Временная сложность: O(n) — два прохода по массиву
        /// Пространственная сложность: O(1)
        /// </summary>
        /// <param name="array">Массив для поиска.</param>
        /// <returns>Второе по величине значение, либо int.MinValue если не найдено.</returns>
        public static int FindSecondLargest(int[] array)
        {
            if (array == null || array.Length < 2)
            {
                return int.MinValue; // Недостаточно элементов для второго по величине
            }

            // Находим максимальный элемент
            int max = FindMax(array);
            
            // Находим максимум среди элементов, которые меньше max
            int secondMax = int.MinValue;
            bool found = false;

            foreach (int value in array)
            {
                // Элемент должен быть меньше max и больше текущего secondMax
                if (value < max && value > secondMax)
                {
                    secondMax = value;
                    found = true;
                }
            }

            return found ? secondMax : int.MinValue;
        }

        /// <summary>
        /// Убирает дубликаты из массива и возвращает только уникальные элементы.
        /// 
        /// Алгоритм: использовать HashSet для отслеживания встреченных элементов, добавлять только если не было.
        /// 
        /// Временная сложность: O(n), где n — длина массива
        /// Пространственная сложность: O(k), где k — количество уникальных элементов
        /// </summary>
        /// <param name="array">Массив с возможными дубликатами.</param>
        /// <returns>Массив только из уникальных элементов.</returns>
        public static int[] RemoveDuplicates(int[] array)
        {
            if (array == null || array.Length == 0)
            {
                return Array.Empty<int>();
            }

            // HashSet сам обрабатывает уникальность
            // Быстро проверяет наличие элемента (O(1) в среднем случае)
            HashSet<int> seen = new HashSet<int>();
            List<int> result = new List<int>();

            // Проходим по массиву и добавляем в результат только те элементы, которых ещё не было
            foreach (int value in array)
            {
                // Add возвращает true, если элемент был добавлен (ещё не был в множестве)
                // Возвращает false, если элемент уже был
                if (seen.Add(value))
                {
                    result.Add(value);
                }
            }

            // Переводим список в массив
            return result.ToArray();
        }

        /// <summary>
        /// Сдвигает массив вправо на k позиций.
        /// 
        /// Пример: [1, 2, 3, 4, 5] после сдвига вправо на 2 станет [4, 5, 1, 2, 3]
        /// 
        /// Алгоритм: перевернуть весь массив, потом перевернуть первые k элементов, затем оставшиеся элементы.
        /// 
        /// Временная сложность: O(n), где n — длина массива
        /// Пространственная сложность: O(1) — сдвиг на месте
        /// </summary>
        /// <param name="array">Массив для сдвига.</param>
        /// <param name="k">Количество позиций для сдвига (будет приведено к диапазону массива).</param>
        public static void RotateRight(int[] array, int k)
        {
            if (array == null || array.Length == 0 || k == 0)
            {
                return; // Нечего сдвигать
            }

            int n = array.Length;
            // Привести k к границам массива
            // Если k > n, сдвиг на k то же, что сдвиг на k % n
            k = k % n;

            // Если после приведения k к границам массива получилось 0, не нужно ничего делать
            if (k == 0)
            {
                return;
            }

            // Сначала разворачиваем весь массив
            // Пример: [1,2,3,4,5] -> [5,4,3,2,1]
            ReverseArray(array, 0, n - 1);

            // Потом разворачиваем первые k элементов
            // Пример: [5,4,3,2,1] -> [4,5,3,2,1]
            ReverseArray(array, 0, k - 1);

            // Затем оставшиеся элементы
            // Пример: [4,5,3,2,1] -> [4,5,1,2,3]
            ReverseArray(array, k, n - 1);
        }

        /// <summary>
        /// Вспомогательный метод для переворота части массива на месте.
        /// </summary>
        /// <param name="array">Массив для частичного переворота.</param>
        /// <param name="start">Стартовый индекс (включительно).</param>
        /// <param name="end">Конечный индекс (включительно).</param>
        private static void ReverseArray(int[] array, int start, int end)
        {
            // Два указателя: меняем элементы с концов, двигаясь навстречу к центру
            while (start < end)
            {
                // Меняем элементы местами на позициях start и end
                int temp = array[start];
                array[start] = array[end];
                array[end] = temp;

                // Сдвигаем указатели к центру
                start++;
                end--;
            }
        }

        /// <summary>
        /// Находит две цифры в массиве, сумма которых равна целевому значению.
        /// Предполагается, что в массиве ровно одно решение, и один и тот же элемент нельзя использовать дважды.
        /// 
        /// Алгоритм: использовать словарь для хранения встреченных чисел и их индексов.
        /// Для каждого числа проверять, встречалась ли ранее (target - number).
        /// 
        /// Временная сложность: O(n) — один проход по массиву
        /// Пространственная сложность: O(n) — хранение в словаре
        /// </summary>
        /// <param name="array">Массив для поиска.</param>
        /// <param name="target">Целевая сумма.</param>
        /// <returns>Кортеж с индексами двух чисел или (-1, -1) если не найдено.</returns>
        public static (int index1, int index2) TwoSum(int[] array, int target)
        {
            if (array == null || array.Length < 2)
            {
                return (-1, -1);
            }

            // Словарь: ключ = значение массива, значение = индекс этого значения
            // Позволяет за O(1) проверить, есть ли нужное число
            Dictionary<int, int> seen = new Dictionary<int, int>();

            // Проходим по массиву один раз
            for (int i = 0; i < array.Length; i++)
            {
                int current = array[i];
                int complement = target - current; // То, что нужно найти

                // Проверяем, встречалось ли ранее нужное число
                if (seen.ContainsKey(complement))
                {
                    // Нашли пару! Возвращаем индексы
                    return (seen[complement], i);
                }

                // Запоминаем текущее число и его индекс для будущих проверок
                seen[current] = i;
            }

            // Пара не найдена (не должно быть, если задача гарантирует решение)
            return (-1, -1);
        }

        /// <summary>
        /// Демонстрирует различные упражнения с массивами на примерах.
        /// </summary>
        public static void Demonstrate()
        {
            Console.WriteLine("=== Демонстрация упражнений с массивами ===\n");

            // Поиск максимального элемента
            int[] numbers = { 3, 7, 2, 9, 1, 5 };
            Console.WriteLine("Массив: [{0}]", string.Join(", ", numbers));
            Console.WriteLine("Максимум: {0}\n", FindMax(numbers));

            // Поиск минимального и максимального элементов
            var (min, max) = FindMinMax(numbers);
            Console.WriteLine("Минимум: {0}, Максимум: {1}\n", min, max);

            // Второй по величине элемент
            int secondLargest = FindSecondLargest(numbers);
            Console.WriteLine("Второй по величине: {0}\n", secondLargest);

            // Удаление дубликатов
            int[] withDuplicates = { 1, 2, 2, 3, 4, 4, 5, 5, 5 };
            Console.WriteLine("Массив с дубликатами: [{0}]", string.Join(", ", withDuplicates));
            int[] unique = RemoveDuplicates(withDuplicates);
            Console.WriteLine("Уникальные элементы: [{0}]\n", string.Join(", ", unique));

            // Сдвиг массива
            int[] toRotate = { 1, 2, 3, 4, 5 };
            Console.WriteLine("Оригинал: [{0}]", string.Join(", ", toRotate));
            RotateRight((int[])toRotate.Clone(), 2);
            Console.WriteLine("Сдвиг вправо на 2: [{0}]\n", string.Join(", ", toRotate));

            // Two sum
            int[] sumArray = { 2, 7, 11, 15 };
            int target = 9;
            var (index1, index2) = TwoSum(sumArray, target);
            Console.WriteLine("Массив: [{0}], Цель: {1}", string.Join(", ", sumArray), target);
            Console.WriteLine("Индексы, сумма которых равна {0}: [{1}, {2}]", target, index1, index2);
            Console.WriteLine("Значения: {0} + {1} = {2}", sumArray[index1], sumArray[index2], target);
        }
    }
}
