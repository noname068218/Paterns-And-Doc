using System;
using System.Collections.Generic;
using System.Linq;

namespace Test.src.Helpers
{
    /// <summary>
    /// Утилитарный класс для работы со словарями (Dictionary&lt;TKey, TValue&gt;).
    /// Содержит полезные методы для решения различных задач со словарями.
    /// </summary>
    public static class DictionaryHelpers
    {
        /// <summary>
        /// Инвертирует словарь (меняет ключи и значения местами).
        /// Предупреждение: если в словаре есть дублирующиеся значения, они будут перезаписаны.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(n).
        /// </summary>
        /// <param name="dictionary">Исходный словарь.</param>
        /// <returns>Новый словарь с инвертированными ключами и значениями.</returns>
        public static Dictionary<TValue, TKey> Invert<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
                return new Dictionary<TValue, TKey>();

            Dictionary<TValue, TKey> inverted = new Dictionary<TValue, TKey>();

            foreach (var kvp in dictionary)
            {
                // Если значение уже существует, последнее вхождение перезапишет предыдущее
                inverted[kvp.Value] = kvp.Key;
            }

            return inverted;
        }

        /// <summary>
        /// Объединяет два словаря. При конфликте ключей используется значение из второго словаря.
        /// Временная сложность: O(n + m), где n и m - размеры словарей.
        /// Пространственная сложность: O(n + m).
        /// </summary>
        /// <param name="dict1">Первый словарь.</param>
        /// <param name="dict2">Второй словарь (приоритетный при конфликте ключей).</param>
        /// <returns>Новый объединённый словарь.</returns>
        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
            Dictionary<TKey, TValue> dict1, 
            Dictionary<TKey, TValue> dict2)
        {
            Dictionary<TKey, TValue> merged = new Dictionary<TKey, TValue>();

            if (dict1 != null)
            {
                foreach (var kvp in dict1)
                {
                    merged[kvp.Key] = kvp.Value;
                }
            }

            if (dict2 != null)
            {
                foreach (var kvp in dict2)
                {
                    merged[kvp.Key] = kvp.Value; // Перезаписывает значения из dict1 при конфликте
                }
            }

            return merged;
        }

        /// <summary>
        /// Получает значение по ключу, возвращая значение по умолчанию, если ключ не найден.
        /// Временная сложность: O(1) в среднем случае.
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="dictionary">Словарь для поиска.</param>
        /// <param name="key">Ключ для поиска.</param>
        /// <param name="defaultValue">Значение по умолчанию, если ключ не найден.</param>
        /// <returns>Значение по ключу или defaultValue.</returns>
        public static TValue GetOrDefault<TKey, TValue>(
            Dictionary<TKey, TValue> dictionary, 
            TKey key, 
            TValue defaultValue = default(TValue))
        {
            if (dictionary == null)
                return defaultValue;

            return dictionary.TryGetValue(key, out TValue value) ? value : defaultValue;
        }

        /// <summary>
        /// Подсчитывает частоту значений в словаре (сколько раз каждое значение встречается).
        /// Временная сложность: O(n), где n - количество элементов в словаре.
        /// Пространственная сложность: O(k), где k - количество уникальных значений.
        /// </summary>
        /// <param name="dictionary">Словарь для анализа.</param>
        /// <returns>Словарь, где ключ - значение из исходного словаря, значение - частота.</returns>
        public static Dictionary<TValue, int> CountValueFrequency<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            Dictionary<TValue, int> frequency = new Dictionary<TValue, int>();

            if (dictionary == null)
                return frequency;

            foreach (var value in dictionary.Values)
            {
                if (frequency.ContainsKey(value))
                    frequency[value]++;
                else
                    frequency[value] = 1;
            }

            return frequency;
        }

        /// <summary>
        /// Находит ключ с максимальным значением в словаре.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="dictionary">Словарь для поиска.</param>
        /// <returns>Ключ с максимальным значением.</returns>
        /// <exception cref="ArgumentException">Выбрасывается если словарь пустой.</exception>
        public static TKey KeyWithMaxValue<TKey, TValue>(Dictionary<TKey, TValue> dictionary) 
            where TValue : IComparable<TValue>
        {
            if (dictionary == null || dictionary.Count == 0)
                throw new ArgumentException("Словарь не может быть пустым.", nameof(dictionary));

            TKey maxKey = default(TKey);
            TValue maxValue = default(TValue);
            bool first = true;

            foreach (var kvp in dictionary)
            {
                if (first || kvp.Value.CompareTo(maxValue) > 0)
                {
                    maxKey = kvp.Key;
                    maxValue = kvp.Value;
                    first = false;
                }
            }

            return maxKey;
        }

        /// <summary>
        /// Находит ключ с минимальным значением в словаре.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="dictionary">Словарь для поиска.</param>
        /// <returns>Ключ с минимальным значением.</returns>
        /// <exception cref="ArgumentException">Выбрасывается если словарь пустой.</exception>
        public static TKey KeyWithMinValue<TKey, TValue>(Dictionary<TKey, TValue> dictionary) 
            where TValue : IComparable<TValue>
        {
            if (dictionary == null || dictionary.Count == 0)
                throw new ArgumentException("Словарь не может быть пустым.", nameof(dictionary));

            TKey minKey = default(TKey);
            TValue minValue = default(TValue);
            bool first = true;

            foreach (var kvp in dictionary)
            {
                if (first || kvp.Value.CompareTo(minValue) < 0)
                {
                    minKey = kvp.Key;
                    minValue = kvp.Value;
                    first = false;
                }
            }

            return minKey;
        }

        /// <summary>
        /// Фильтрует словарь, возвращая только элементы, удовлетворяющие условию.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(k), где k - количество элементов, удовлетворяющих условию.
        /// </summary>
        /// <param name="dictionary">Исходный словарь.</param>
        /// <param name="predicate">Условие для фильтрации.</param>
        /// <returns>Новый словарь с отфильтрованными элементами.</returns>
        public static Dictionary<TKey, TValue> Filter<TKey, TValue>(
            Dictionary<TKey, TValue> dictionary, 
            Func<KeyValuePair<TKey, TValue>, bool> predicate)
        {
            if (dictionary == null || predicate == null)
                return new Dictionary<TKey, TValue>();

            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();

            foreach (var kvp in dictionary)
            {
                if (predicate(kvp))
                {
                    result[kvp.Key] = kvp.Value;
                }
            }

            return result;
        }

        /// <summary>
        /// Преобразует словарь в список пар ключ-значение, отсортированный по значениям.
        /// Временная сложность: O(n log n).
        /// Пространственная сложность: O(n).
        /// </summary>
        /// <param name="dictionary">Исходный словарь.</param>
        /// <param name="ascending">True для сортировки по возрастанию, false - по убыванию.</param>
        /// <returns>Список пар ключ-значение, отсортированный по значениям.</returns>
        public static List<KeyValuePair<TKey, TValue>> SortByValue<TKey, TValue>(
            Dictionary<TKey, TValue> dictionary, 
            bool ascending = true) 
            where TValue : IComparable<TValue>
        {
            if (dictionary == null || dictionary.Count == 0)
                return new List<KeyValuePair<TKey, TValue>>();

            List<KeyValuePair<TKey, TValue>> sorted = dictionary.ToList();

            sorted.Sort((x, y) => 
            {
                int comparison = x.Value.CompareTo(y.Value);
                return ascending ? comparison : -comparison;
            });

            return sorted;
        }

        /// <summary>
        /// Проверяет, содержит ли словарь все ключи из указанного списка.
        /// Временная сложность: O(m), где m - количество ключей для проверки.
        /// Пространственная сложность: O(1).
        /// </summary>
        /// <param name="dictionary">Словарь для проверки.</param>
        /// <param name="keys">Список ключей для проверки.</param>
        /// <returns>True, если все ключи присутствуют в словаре.</returns>
        public static bool ContainsAllKeys<TKey, TValue>(Dictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys)
        {
            if (dictionary == null || keys == null)
                return false;

            foreach (TKey key in keys)
            {
                if (!dictionary.ContainsKey(key))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Подсчитывает количество вхождений каждого значения в словаре.
        /// Временная сложность: O(n).
        /// Пространственная сложность: O(k), где k - количество уникальных значений.
        /// </summary>
        /// <param name="dictionary">Словарь для анализа.</param>
        /// <returns>Словарь частот значений.</returns>
        public static Dictionary<TValue, int> GetValueCounts<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            return CountValueFrequency(dictionary);
        }

        /// <summary>
        /// Демонстрирует работу всех методов класса DictionaryHelpers.
        /// </summary>
        public static void Demonstrate()
        {
            Console.WriteLine("=== Демонстрация методов DictionaryHelpers ===\n");

            Dictionary<string, int> scores = new Dictionary<string, int>
            {
                ["Alice"] = 95,
                ["Bob"] = 87,
                ["Charlie"] = 92,
                ["David"] = 87
            };

            Console.WriteLine("Исходный словарь:");
            foreach (var kvp in scores)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
            Console.WriteLine();

            // Invert
            var inverted = Invert(scores);
            Console.WriteLine("Invert:");
            foreach (var kvp in inverted)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
            Console.WriteLine();

            // GetOrDefault
            Console.WriteLine($"GetOrDefault('Alice', 0): {GetOrDefault(scores, "Alice", 0)}");
            Console.WriteLine($"GetOrDefault('Eve', 0): {GetOrDefault(scores, "Eve", 0)}");
            Console.WriteLine();

            // CountValueFrequency
            var frequencies = CountValueFrequency(scores);
            Console.WriteLine("CountValueFrequency:");
            foreach (var kvp in frequencies)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value} раз(а)");
            }
            Console.WriteLine();

            // KeyWithMaxValue/KeyWithMinValue
            Console.WriteLine($"KeyWithMaxValue: {KeyWithMaxValue(scores)}");
            Console.WriteLine($"KeyWithMinValue: {KeyWithMinValue(scores)}");
            Console.WriteLine();

            // Filter
            var highScores = Filter(scores, kvp => kvp.Value >= 90);
            Console.WriteLine("Filter (>= 90):");
            foreach (var kvp in highScores)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
            Console.WriteLine();

            // SortByValue
            var sorted = SortByValue(scores, ascending: false);
            Console.WriteLine("SortByValue (descending):");
            foreach (var kvp in sorted)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
            Console.WriteLine();

            // ContainsAllKeys
            Console.WriteLine($"ContainsAllKeys(['Alice', 'Bob']): {ContainsAllKeys(scores, new[] { "Alice", "Bob" })}");
            Console.WriteLine($"ContainsAllKeys(['Alice', 'Eve']): {ContainsAllKeys(scores, new[] { "Alice", "Eve" })}");

            // Merge
            Dictionary<string, int> additionalScores = new Dictionary<string, int>
            {
                ["Eve"] = 88,
                ["Bob"] = 90 // Перезапишет значение из scores
            };
            var merged = Merge(scores, additionalScores);
            Console.WriteLine("\nMerge:");
            foreach (var kvp in merged)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
        }
    }
}
