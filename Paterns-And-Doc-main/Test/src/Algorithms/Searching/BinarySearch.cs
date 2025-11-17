using System;

namespace Test.src.Algorithms.Searching
{
    /// <summary>
    /// Реализует алгоритм бинарного поиска для поиска элементов в отсортированном массиве.
    /// 
    /// <para>
    /// Бинарный поиск — это эффективный алгоритм для поиска элемента в отсортированном списке.
    /// Он работает путём многократного деления пополам той части списка, где может находиться элемент,
    /// пока не останется только одна возможная позиция.
    /// </para>
    /// 
    /// <para>
    /// Временная сложность: O(log n) — за каждую итерацию исключается половина оставшихся элементов.
    /// Пространственная сложность: O(1) для итеративной версии, O(log n) для рекурсивной версии (стек вызовов).
    /// </para>
    /// 
    /// <para>
    /// Важно: массив ДОЛЖЕН быть отсортирован для корректной работы бинарного поиска.
    /// Применение: Поиск в больших отсортированных данных, когда требуется быстрый поиск.
    /// </para>
    /// </summary>
    public class BinarySearch
    {
        /// <summary>
        /// Ищет целевое значение в отсортированном массиве с помощью итеративного бинарного поиска.
        /// Возвращает индекс найденного элемента, либо -1, если элемент не найден.
        /// </summary>
        /// <param name="array">Отсортированный массив для поиска.</param>
        /// <param name="target">Значение для поиска.</param>
        /// <returns>
        /// Индекс (с нуля) целевого элемента, если найден; иначе -1.
        /// </returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если массив равен null.</exception>
        public static int Search(int[] array, int target)
        {
            // Проверка входных данных
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array), "Массив не может быть null.");
            }

            // Пустой массив - нечего искать
            if (array.Length == 0)
            {
                return -1;
            }

            // Инициализируем границы поиска
            // 'left' — самый левый индекс текущего диапазона поиска
            // 'right' — самый правый индекс текущего диапазона поиска
            int left = 0;
            int right = array.Length - 1;

            // Продолжаем поиск, пока диапазон поиска не исчерпан
            // Когда left > right, перебраны все возможности
            while (left <= right)
            {
                // Вычисляем индекс середины
                // Эта формула предотвращает переполнение: left + (right - left) / 2
                // Прямая формула (left + right) / 2 может привести к переполнению при больших значениях
                int middle = left + (right - left) / 2;

                // Проверяем, совпадает ли элемент в середине с искомым
                if (array[middle] == target)
                {
                    // Найдено! Возвращаем индекс
                    return middle;
                }
                // Если целевое значение меньше элемента в середине, ищем в левой части
                // Отбрасываем правую часть (включая средний элемент), сдвигая правую границу
                else if (array[middle] > target)
                {
                    // Ищем в левом подмассиве: от left до (middle - 1)
                    right = middle - 1;
                }
                // Если целевое значение больше элемента в середине, ищем в правой части
                // Отбрасываем левую часть (включая средний элемент), сдвигая левую границу
                else
                {
                    // Ищем в правом подмассиве: от (middle + 1) до right
                    left = middle + 1;
                }
            }

            // Если вышли из цикла, целевой элемент не найден в массиве
            return -1;
        }

        /// <summary>
        /// Ищет целевое значение в отсортированном массиве с помощью рекурсивного бинарного поиска.
        /// Эта версия использует рекурсию вместо итерации.
        /// </summary>
        /// <param name="array">Отсортированный массив для поиска.</param>
        /// <param name="target">Значение для поиска.</param>
        /// <param name="left">Начальный индекс диапазона поиска (по умолчанию 0).</param>
        /// <param name="right">Конечный индекс диапазона поиска (по умолчанию array.Length - 1).</param>
        /// <returns>
        /// Индекс (с нуля) целевого элемента, если найден; иначе -1.
        /// </returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если массив равен null.</exception>
        public static int SearchRecursive(int[] array, int target, int left = 0, int? right = null)
        {
            // Проверка входных данных
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array), "Массив не может быть null.");
            }

            // Задаём правую границу по умолчанию, если она не указана
            if (right == null)
            {
                right = array.Length - 1;
            }

            // Базовый случай: если диапазон поиска некорректен, целевой элемент не найден
            if (left > right)
            {
                return -1;
            }

            // Вычисляем индекс середины
            int middle = left + (right.Value - left) / 2;

            // Базовый случай: если средний элемент — это искомый, возвращаем индекс
            if (array[middle] == target)
            {
                return middle;
            }

            // Рекурсивный случай: целевой элемент в левой части
            // Рекурсивно ищем в левом подмассиве
            if (array[middle] > target)
            {
                return SearchRecursive(array, target, left, middle - 1);
            }

            // Рекурсивный случай: целевой элемент в правой части
            // Рекурсивно ищем в правом подмассиве
            return SearchRecursive(array, target, middle + 1, right);
        }

        /// <summary>
        /// Находит первое вхождение целевого значения в отсортированном массиве (допускаются дубликаты).
        /// Возвращает индекс самого левого вхождения.
        /// </summary>
        /// <param name="array">Отсортированный массив для поиска.</param>
        /// <param name="target">Значение для поиска.</param>
        /// <returns>
        /// Индекс (с нуля) первого вхождения; -1 если не найдено.
        /// </returns>
        public static int FindFirstOccurrence(int[] array, int target)
        {
            if (array == null || array.Length == 0)
            {
                return -1;
            }

            int left = 0;
            int right = array.Length - 1;
            int result = -1; // Сохраняем наилучший найденный кандидат

            while (left <= right)
            {
                int middle = left + (right - left) / 2;

                if (array[middle] == target)
                {
                    // Найдено совпадение, но продолжаем искать влево для первого вхождения
                    result = middle; // Сохраняем как кандидата
                    right = middle - 1; // Ищем в левой части более раннее вхождение
                }
                else if (array[middle] > target)
                {
                    right = middle - 1;
                }
                else
                {
                    left = middle + 1;
                }
            }

            return result;
        }

        /// <summary>
        /// Демонстрирует работу алгоритма бинарного поиска на различных примерах.
        /// </summary>
        public static void Demonstrate()
        {
            Console.WriteLine("=== Демонстрация Бинарного Поиска ===\n");

            // Пример 1: Обычный поиск в отсортированном массиве
            int[] sortedArray = { 2, 5, 8, 12, 16, 23, 38, 45, 67, 78, 90 };
            Console.WriteLine("Отсортированный массив: [{0}]", string.Join(", ", sortedArray));
            Console.WriteLine();

            int[] targets = { 16, 45, 100, 5 };

            foreach (int target in targets)
            {
                int index = Search(sortedArray, target);
                if (index != -1)
                {
                    Console.WriteLine("Элемент {0} найден по индексу {1}", target, index);
                }
                else
                {
                    Console.WriteLine("Элемент {0} не найден в массиве", target);
                }
            }
            Console.WriteLine();

            // Пример 2: Рекурсивная версия
            Console.WriteLine("Используем рекурсивный поиск:");
            int indexRecursive = SearchRecursive(sortedArray, 23);
            Console.WriteLine("Элемент 23 найден по индексу {0}", indexRecursive);
            Console.WriteLine();

            // Пример 3: Первое вхождение с дубликатами
            int[] arrayWithDuplicates = { 2, 5, 5, 5, 12, 16, 16, 23, 23, 23, 45 };
            Console.WriteLine("Массив с дубликатами: [{0}]", string.Join(", ", arrayWithDuplicates));
            int firstIndex = FindFirstOccurrence(arrayWithDuplicates, 5);
            Console.WriteLine("Первое вхождение числа 5 по индексу {0}", firstIndex);
        }
    }
}
