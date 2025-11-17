using System;
using System.Collections.Generic;
using System.Linq;

namespace Test.src.Helpers
{
    /// <summary>
    /// Утилитарный класс для работы со списками (List&lt;T&gt;).
    /// Содержит полезные методы для решения различных задач со списками.
    /// </summary>
    public static class ListHelpers
    {
        /// <summary>
        /// Находит максимальный элемент в списке.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="list">Список для поиска.</param>
        /// <returns>Максимальный элемент.</returns>
        /// <exception cref="ArgumentException">Выбрасывается если список пустой или null.</exception>
        public static T FindMax<T>(List<T> list) where T : IComparable<T>
        {
            if (list == null || list.Count == 0)
                throw new ArgumentException("Список не может быть пустым или null.", nameof(list));

            T max = list[0];
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i].CompareTo(max) > 0)
                    max = list[i];
            }

            return max;
        }

        /// <summary>
        /// Находит минимальный элемент в списке.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="list">Список для поиска.</param>
        /// <returns>Минимальный элемент.</returns>
        public static T FindMin<T>(List<T> list) where T : IComparable<T>
        {
            if (list == null || list.Count == 0)
                throw new ArgumentException("Список не может быть пустым или null.", nameof(list));

            T min = list[0];
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i].CompareTo(min) < 0)
                    min = list[i];
            }

            return min;
        }

        /// <summary>
        /// Удаляет дубликаты из списка, сохраняя порядок первого вхождения.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(k), где k - количество уникальных элементов.
        /// </summary>
        /// <param name="list">Список с возможными дубликатами.</param>
        /// <returns>Новый список без дубликатов.</returns>
        public static List<T> RemoveDuplicates<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
                return new List<T>();

            HashSet<T> seen = new HashSet<T>();
            List<T> result = new List<T>();

            foreach (T item in list)
            {
                if (seen.Add(item))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Переворачивает список на месте (in-place).
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="list">Список для переворота.</param>
        public static void Reverse<T>(List<T> list)
        {
            if (list == null || list.Count <= 1)
                return;

            int left = 0;
            int right = list.Count - 1;

            while (left < right)
            {
                T temp = list[left];
                list[left] = list[right];
                list[right] = temp;
                left++;
                right--;
            }
        }

        /// <summary>
        /// Создаёт новый перевёрнутый список, не изменяя исходный.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(n).
        /// </summary>
        /// <param name="list">Исходный список.</param>
        /// <returns>Новый перевёрнутый список.</returns>
        public static List<T> ReverseCopy<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
                return new List<T>();

            List<T> reversed = new List<T>(list.Count);
            for (int i = list.Count - 1; i >= 0; i--)
            {
                reversed.Add(list[i]);
            }

            return reversed;
        }

        /// <summary>
        /// Разделяет список на части указанного размера.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(n).
        /// </summary>
        /// <param name="list">Исходный список.</param>
        /// <param name="chunkSize">Размер каждой части.</param>
        /// <returns>Список списков, каждый размером chunkSize (кроме последнего).</returns>
        public static List<List<T>> Chunk<T>(List<T> list, int chunkSize)
        {
            if (list == null || list.Count == 0 || chunkSize <= 0)
                return new List<List<T>>();

            List<List<T>> chunks = new List<List<T>>();

            for (int i = 0; i < list.Count; i += chunkSize)
            {
                int size = Math.Min(chunkSize, list.Count - i);
                chunks.Add(list.GetRange(i, size));
            }

            return chunks;
        }

        /// <summary>
        /// Перемешивает элементы списка случайным образом (Fisher-Yates shuffle).
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="list">Список для перемешивания.</param>
        public static void Shuffle<T>(List<T> list)
        {
            if (list == null || list.Count <= 1)
                return;

            Random random = new Random();
            int n = list.Count;

            // Алгоритм Fisher-Yates: идём от конца к началу
            for (int i = n - 1; i > 0; i--)
            {
                // Выбираем случайный индекс от 0 до i (включительно)
                int j = random.Next(i + 1);

                // Меняем элементы местами
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        /// <summary>
        /// Находит все индексы вхождения элемента в списке.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(k), где k - количество вхождений.
        /// </summary>
        /// <param name="list">Список для поиска.</param>
        /// <param name="item">Элемент для поиска.</param>
        /// <returns>Список индексов всех вхождений.</returns>
        public static List<int> AllIndicesOf<T>(List<T> list, T item)
        {
            List<int> indices = new List<int>();

            if (list == null || list.Count == 0)
                return indices;

            for (int i = 0; i < list.Count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(list[i], item))
                {
                    indices.Add(i);
                }
            }

            return indices;
        }

        /// <summary>
        /// Фильтрует список, возвращая только элементы, удовлетворяющие условию.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(k), где k - количество элементов, удовлетворяющих условию.
        /// </summary>
        /// <param name="list">Исходный список.</param>
        /// <param name="predicate">Условие для фильтрации.</param>
        /// <returns>Новый список с отфильтрованными элементами.</returns>
        public static List<T> Filter<T>(List<T> list, Func<T, bool> predicate)
        {
            if (list == null || predicate == null)
                return new List<T>();

            List<T> result = new List<T>();
            foreach (T item in list)
            {
                if (predicate(item))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Группирует элементы списка по указанному ключу.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(n).
        /// </summary>
        /// <param name="list">Исходный список.</param>
        /// <param name="keySelector">Функция для извлечения ключа группировки.</param>
        /// <returns>Словарь, где ключ - значение keySelector, значение - список элементов.</returns>
        public static Dictionary<TKey, List<T>> GroupBy<T, TKey>(List<T> list, Func<T, TKey> keySelector)
        {
            Dictionary<TKey, List<T>> groups = new Dictionary<TKey, List<T>>();

            if (list == null || keySelector == null)
                return groups;

            foreach (T item in list)
            {
                TKey key = keySelector(item);

                if (!groups.ContainsKey(key))
                {
                    groups[key] = new List<T>();
                }

                groups[key].Add(item);
            }

            return groups;
        }

        /// <summary>
        /// Проверяет, содержат ли два списка одинаковые элементы (в любом порядке).
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(n).
        /// </summary>
        /// <param name="list1">Первый список.</param>
        /// <param name="list2">Второй список.</param>
        /// <returns>True, если списки содержат одинаковые элементы.</returns>
        public static bool AreEquivalent<T>(List<T> list1, List<T> list2)
        {
            if (list1 == null && list2 == null)
                return true;

            if (list1 == null || list2 == null)
                return false;

            if (list1.Count != list2.Count)
                return false;

            // Подсчитываем частоту каждого элемента в первом списке
            Dictionary<T, int> count1 = new Dictionary<T, int>();
            foreach (T item in list1)
            {
                if (count1.ContainsKey(item))
                    count1[item]++;
                else
                    count1[item] = 1;
            }

            // Подсчитываем частоту каждого элемента во втором списке
            Dictionary<T, int> count2 = new Dictionary<T, int>();
            foreach (T item in list2)
            {
                if (count2.ContainsKey(item))
                    count2[item]++;
                else
                    count2[item] = 1;
            }

            // Проверяем, что частоты совпадают
            if (count1.Count != count2.Count)
                return false;

            foreach (var kvp in count1)
            {
                if (!count2.ContainsKey(kvp.Key) || count2[kvp.Key] != kvp.Value)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Находит пересечение двух списков (элементы, присутствующие в обоих списках).
        /// Временная сложность: O(n + m), где n и m - длины списков.
        /// Пространственная сложность: O(min(n, m)).
        /// </summary>
        /// <param name="list1">Первый список.</param>
        /// <param name="list2">Второй список.</param>
        /// <returns>Новый список с общими элементами.</returns>
        public static List<T> Intersect<T>(List<T> list1, List<T> list2)
        {
            if (list1 == null || list2 == null || list1.Count == 0 || list2.Count == 0)
                return new List<T>();

            HashSet<T> set2 = new HashSet<T>(list2);
            List<T> result = new List<T>();
            HashSet<T> added = new HashSet<T>(); // Для избежания дубликатов в результате

            foreach (T item in list1)
            {
                if (set2.Contains(item) && added.Add(item))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Находит объединение двух списков (все уникальные элементы из обоих списков).
        /// Временная сложность: O(n + m).
        /// Пространственная сложность: O(n + m).
        /// </summary>
        /// <param name="list1">Первый список.</param>
        /// <param name="list2">Второй список.</param>
        /// <returns>Новый список со всеми уникальными элементами.</returns>
        public static List<T> Union<T>(List<T> list1, List<T> list2)
        {
            HashSet<T> result = new HashSet<T>();

            if (list1 != null)
            {
                foreach (T item in list1)
                {
                    result.Add(item);
                }
            }

            if (list2 != null)
            {
                foreach (T item in list2)
                {
                    result.Add(item);
                }
            }

            return result.ToList();
        }

        /// <summary>
        /// Демонстрирует работу всех методов класса ListHelpers.
        /// </summary>
        public static void Demonstrate()
        {
            Console.WriteLine("=== Демонстрация методов ListHelpers ===\n");

            List<int> numbers = new List<int> { 3, 7, 2, 9, 1, 5, 9, 2 };

            // FindMax/FindMin
            Console.WriteLine($"Список: [{string.Join(", ", numbers)}]");
            Console.WriteLine($"Max: {FindMax(numbers)}");
            Console.WriteLine($"Min: {FindMin(numbers)}");

            // RemoveDuplicates
            Console.WriteLine($"RemoveDuplicates: [{string.Join(", ", RemoveDuplicates(numbers))}]");

            // Reverse
            List<int> toReverse = new List<int> { 1, 2, 3, 4, 5 };
            Console.WriteLine($"Reverse: [{string.Join(", ", toReverse)}] -> ");
            Reverse(toReverse);
            Console.WriteLine($"  [{string.Join(", ", toReverse)}]");

            // Chunk
            List<int> toChunk = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
            var chunks = Chunk(toChunk, 3);
            Console.WriteLine($"Chunk(3):");
            foreach (var chunk in chunks)
            {
                Console.WriteLine($"  [{string.Join(", ", chunk)}]");
            }

            // Shuffle
            List<int> toShuffle = new List<int> { 1, 2, 3, 4, 5 };
            Console.WriteLine($"Shuffle: [{string.Join(", ", toShuffle)}] -> ");
            Shuffle(toShuffle);
            Console.WriteLine($"  [{string.Join(", ", toShuffle)}]");

            // AllIndicesOf
            Console.WriteLine($"AllIndicesOf(9): [{string.Join(", ", AllIndicesOf(numbers, 9))}]");

            // Filter
            var evenNumbers = Filter(numbers, x => x % 2 == 0);
            Console.WriteLine($"Filter (even): [{string.Join(", ", evenNumbers)}]");

            // GroupBy
            var groups = GroupBy(numbers, x => x % 2 == 0 ? "even" : "odd");
            Console.WriteLine($"GroupBy (even/odd):");
            foreach (var group in groups)
            {
                Console.WriteLine($"  {group.Key}: [{string.Join(", ", group.Value)}]");
            }

            // Intersect/Union
            List<int> list1 = new List<int> { 1, 2, 3, 4 };
            List<int> list2 = new List<int> { 3, 4, 5, 6 };
            Console.WriteLine($"Intersect: [{string.Join(", ", Intersect(list1, list2))}]");
            Console.WriteLine($"Union: [{string.Join(", ", Union(list1, list2))}]");
        }
    }
}
