using System;

namespace Test.src.Algorithms.Sorting
{
    /// <summary>
    /// Реализует алгоритм быстрой сортировки для массивов используя подход "разделяй и властвуй".
    /// 
    /// <para>
    /// Быстрая сортировка - один из самых эффективных алгоритмов сортировки. Он работает, выбирая 'опорный' элемент
    /// из массива и разделяя остальные элементы на два подмассива в зависимости от того, меньше они или больше опорного.
    /// Подмассивы затем сортируются рекурсивно.
    /// </para>
    /// 
    /// <para>
    /// Временная сложность: O(n log n) средний случай, O(n²) худший случай (редко, когда опорный элемент всегда наименьший/наибольший).
    /// Пространственная сложность: O(log n) средний случай (стек рекурсии), O(n) худший случай.
    /// </para>
    /// 
    /// <para>
    /// Применение: Большие наборы данных, когда важна средняя производительность. Очень эффективен на практике.
    /// Нестабильная сортировка (равные элементы могут изменить относительный порядок).
    /// </para>
    /// </summary>
    public class QuickSort
    {
        /// <summary>
        /// Sorts an array of integers in ascending order using the Quick Sort algorithm.
        /// This is the public entry point that initiates the recursive sorting process.
        /// </summary>
        /// <param name="array">The array to be sorted (will be modified in place).</param>
        /// <returns>The sorted array (same reference as input).</returns>
        /// <exception cref="ArgumentNullException">Thrown when array is null.</exception>
        public static int[] Sort(int[] array)
        {
            // Проверяем входные данные
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array), "Массив не может быть null.");
            }

            // Если массив содержит 0 или 1 элемент, он уже отсортирован
            if (array.Length <= 1)
            {
                return array;
            }

            // Запускаем рекурсивный процесс сортировки
            // Параметры: массив, начальный индекс (0), конечный индекс (последний элемент)
            SortRecursive(array, 0, array.Length - 1);

            return array;
        }

        /// <summary>
        /// Recursively sorts a portion of the array using Quick Sort.
        /// Uses divide-and-conquer: partition the array and recursively sort sub-arrays.
        /// </summary>
        /// <param name="array">The array to sort.</param>
        /// <param name="low">Starting index of the portion to sort (inclusive).</param>
        /// <param name="high">Ending index of the portion to sort (inclusive).</param>
        private static void SortRecursive(int[] array, int low, int high)
        {
            // Базовый случай: если остался один элемент или меньше, он уже отсортирован
            // Это останавливает рекурсию, когда подмассивы становятся достаточно маленькими
            if (low < high)
            {
                // Разделяем массив: переставляем элементы так, чтобы опорный элемент был в правильной позиции
                // Элементы меньше опорного идут влево, больше - вправо
                // Возвращает индекс опорного элемента после разделения
                int pivotIndex = Partition(array, low, high);

                // Рекурсивно сортируем левый подмассив (элементы меньше опорного)
                // Сортируем от 'low' до позиции перед опорным (опорный уже на правильном месте)
                SortRecursive(array, low, pivotIndex - 1);

                // Рекурсивно сортируем правый подмассив (элементы больше опорного)
                // Сортируем от позиции после опорного до 'high'
                SortRecursive(array, pivotIndex + 1, high);
            }
        }

        /// <summary>
        /// Partitions the array around a pivot element.
        /// Rearranges the array so that all elements less than pivot are to the left,
        /// and all elements greater than pivot are to the right. The pivot is placed in its final sorted position.
        /// </summary>
        /// <param name="array">The array to partition.</param>
        /// <param name="low">Starting index of the partition (inclusive).</param>
        /// <param name="high">Ending index of the partition (inclusive).</param>
        /// <returns>The final index of the pivot element after partitioning.</returns>
        private static int Partition(int[] array, int low, int high)
        {
            // Выбираем правый элемент как опорный
            // Это простая стратегия, но может привести к худшему случаю O(n²), если массив уже отсортирован
            // Лучшие стратегии: медиана трёх, случайный опорный элемент
            int pivot = array[high];

            // Индекс меньшего элемента (указывает правильную позицию опорного элемента пока что)
            // Элементы с индексами меньше 'i' уже меньше опорного
            int i = low - 1;

            // Проходим по всем элементам от 'low' до 'high - 1'
            // Пропускаем 'high', потому что это наш опорный элемент
            for (int j = low; j < high; j++)
            {
                // Если текущий элемент меньше или равен опорному
                // Он должен быть в левой части раздела
                if (array[j] <= pivot)
                {
                    // Увеличиваем индекс меньшего элемента
                    i++;

                    // Меняем текущий элемент с элементом на позиции 'i'
                    // Это перемещает меньшие элементы влево
                    int temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            }

            // После цикла: все элементы с индексами [low..i] <= опорного
            // Все элементы с индексами [i+1..high-1] > опорного
            // Размещаем опорный элемент в правильной позиции (сразу после всех меньших элементов)
            // Меняем опорный (на позиции 'high') с элементом на 'i + 1'
            int tempPivot = array[i + 1];
            array[i + 1] = array[high];
            array[high] = tempPivot;

            // Возвращаем индекс, где опорный элемент находится сейчас
            // Это будет использовано для разделения массива на рекурсивные вызовы
            return i + 1;
        }

        /// <summary>
        /// Демонстрирует алгоритм быстрой сортировки с примерами использования и сравнением производительности.
        /// </summary>
        public static void Demonstrate()
        {
            Console.WriteLine("=== Демонстрация быстрой сортировки ===\n");

            // Пример 1: Неотсортированный массив
            int[] numbers = { 64, 34, 25, 12, 22, 11, 90, 5 };
            Console.WriteLine("Исходный массив: [{0}]", string.Join(", ", numbers));

            int[] sorted = Sort((int[])numbers.Clone());
            Console.WriteLine("Отсортированный массив:   [{0}]", string.Join(", ", sorted));
            Console.WriteLine();

            // Пример 2: Демонстрация с большим массивом
            int[] largeArray = { 42, 13, 7, 99, 23, 56, 1, 88, 45, 33, 67, 89, 12, 78, 90 };
            Console.WriteLine("Большой массив ({0} элементов):", largeArray.Length);
            Console.WriteLine("Исходный: [{0}]", string.Join(", ", largeArray));

            int[] sortedLarge = Sort((int[])largeArray.Clone());
            Console.WriteLine("Отсортированный:   [{0}]", string.Join(", ", sortedLarge));
        }
    }
}
