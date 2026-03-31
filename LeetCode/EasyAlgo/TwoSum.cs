public class TwoSum
{
    /// <summary>
    /// Restituisce gli indici dei due elementi nell'array che sommati danno il target.
    /// Utilizza un dizionario per ottimizzare la ricerca (pattern: "hash map for complement").
    /// </summary>
    public int[] TwoSum(int[] nums, int target)
    {
        // Создаём словарь для хранения значения и его индекса при проходе по массиву
        // Это позволяет за O(1) проверить, встречался ли уже комплементарный элемент
        var result = new Dictionary<int, int>();

        // Проходим по всем элементам массива
        for (int i = 0; i < nums.Length; i++)
        {
            // Вычисляем комплемент, который в сумме с nums[i] даст целевое значение (target)
            int complement = target - nums[i];

            // Если комплемент уже есть в словаре, значит мы нашли нужную пару
            // Сразу возвращаем индексы найденных чисел
            if (result.ContainsKey(complement))
            {
                // Возвращаем массив с индексом найденного комплемента и текущим индексом
                return new int[] { result[complement], i };
            }

            // Если не нашли, добавляем текущий элемент в словарь с его индексом
            // Это пригодится для следующих шагов итерации
            result[nums[i]] = i;
        }

        // Если решения нет, возвращаем пустой массив
        return new int[0];
    }
}