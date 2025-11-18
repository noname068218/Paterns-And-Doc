# Методы работы с массивами и коллекциями в C# #

## Обзор ##

C# предоставляет богатый набор методов для работы с массивами, списками и другими коллекциями. В этой документации описаны основные методы и их использование.

---

## Методы массивов (Array) ##

### Length / Count - Размер массива ###

**Описание:** Возвращает количество элементов в массиве.

**Когда использовать:**

- Для проверки размера коллекции
- Для циклов с условием размера
- Для валидации данных

**Примеры:**

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

int length = numbers.Length; // 5

// Для многомерных массивов
int[,] matrix = new int[3, 4];
int rows = matrix.GetLength(0); // 3
int cols = matrix.GetLength(1); // 4
```

### IndexOf - Поиск индекса элемента ###

**Описание:** Находит индекс первого вхождения указанного элемента.

**Когда использовать:**

- Для поиска позиции элемента
- Для проверки наличия элемента
- Для работы с индексами

**Примеры:**

```csharp
int[] numbers = { 10, 20, 30, 40, 50 };

int index = Array.IndexOf(numbers, 30); // 2
int notFound = Array.IndexOf(numbers, 100); // -1

// С указанием диапазона
int indexInRange = Array.IndexOf(numbers, 30, 0, 3); // 2
```

### LastIndexOf - Последний индекс элемента ###

**Описание:** Находит индекс последнего вхождения указанного элемента.

**Примеры:**

```csharp
int[] numbers = { 10, 20, 30, 20, 50 };

int lastIndex = Array.LastIndexOf(numbers, 20); // 3
```

### Contains - Проверка наличия элемента ###

**Описание:** Проверяет, содержит ли массив указанный элемент.

**Примеры:**

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

bool contains = numbers.Contains(3); // true (через LINQ)
// Для массивов обычно используется Array.IndexOf != -1
bool has = Array.IndexOf(numbers, 3) != -1; // true
```

### Sort - Сортировка массива ###

**Описание:** Сортирует элементы массива на месте.

**Когда использовать:**

- Для сортировки данных
- Для упорядочивания элементов
- Перед бинарным поиском

**Примеры:**

```csharp
int[] numbers = { 3, 1, 4, 1, 5, 9, 2, 6 };

Array.Sort(numbers); // [1, 1, 2, 3, 4, 5, 6, 9]

// С кастомным сравнением
string[] names = { "John", "Jane", "Bob" };
Array.Sort(names, StringComparer.OrdinalIgnoreCase);

// Частичная сортировка
int[] numbers2 = { 3, 1, 4, 1, 5, 9, 2, 6 };
Array.Sort(numbers2, 0, 3); // Сортирует только первые 3 элемента
```

### Reverse - Обращение массива ###

**Описание:** Обращает порядок элементов в массиве на месте.

**Примеры:**

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

Array.Reverse(numbers); // [5, 4, 3, 2, 1]

// Частичное обращение
int[] numbers2 = { 1, 2, 3, 4, 5 };
Array.Reverse(numbers2, 1, 3); // [1, 4, 3, 2, 5]
```

### Copy - Копирование массива ###

**Описание:** Копирует элементы из одного массива в другой.

**Примеры:**

```csharp
int[] source = { 1, 2, 3, 4, 5 };
int[] destination = new int[5];

Array.Copy(source, destination, source.Length); // destination = [1, 2, 3, 4, 5]

// Копирование с указанием позиций
int[] dest2 = new int[10];
Array.Copy(source, 0, dest2, 5, 3); // Копирует 3 элемента с позиции 0 в позицию 5
```

### Clone - Клонирование массива ###

**Описание:** Создает поверхностную копию массива.

**Примеры:**

```csharp
int[] original = { 1, 2, 3, 4, 5 };
int[] clone = (int[])original.Clone(); // Новый массив с теми же элементами
```

### Clear - Очистка массива ###

**Описание:** Устанавливает диапазон элементов массива в значения по умолчанию.

**Примеры:**

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

Array.Clear(numbers, 0, numbers.Length); // Все элементы = 0
Array.Clear(numbers, 1, 2); // Очищает элементы с индекса 1, длиной 2
```

### Resize - Изменение размера массива ###

**Описание:** Изменяет размер одномерного массива до указанного нового размера.

**Примеры:**

```csharp
int[] numbers = { 1, 2, 3 };

Array.Resize(ref numbers, 5); // Размер увеличен до 5, новые элементы = 0
Array.Resize(ref numbers, 2); // Размер уменьшен до 2
```

### Find / FindAll - Поиск элементов ###

**Описание:** Находит элементы, удовлетворяющие условию.

**Примеры:**

```csharp
int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

// Найти первый четный элемент
int firstEven = Array.Find(numbers, n => n % 2 == 0); // 2

// Найти последний элемент
int lastEven = Array.FindLast(numbers, n => n % 2 == 0); // 10

// Найти все четные элементы
int[] allEven = Array.FindAll(numbers, n => n % 2 == 0); // [2, 4, 6, 8, 10]

// Найти индекс элемента
int index = Array.FindIndex(numbers, n => n > 5); // 5 (индекс элемента 6)
int lastIndex = Array.FindLastIndex(numbers, n => n > 5); // 9
```

### Exists / TrueForAll - Проверка условий ###

**Описание:** Проверяет, существует ли элемент, удовлетворяющий условию, или все элементы удовлетворяют условию.

**Примеры:**

```csharp
int[] numbers = { 2, 4, 6, 8, 10 };

bool exists = Array.Exists(numbers, n => n % 2 == 0); // true
bool allEven = Array.TrueForAll(numbers, n => n % 2 == 0); // true
```

### ForEach - Перебор элементов ###

**Описание:** Выполняет указанное действие с каждым элементом массива.

**Примеры:**

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

Array.ForEach(numbers, n => Console.WriteLine(n * 2));

// Эквивалентно:
foreach (int n in numbers)
{
    Console.WriteLine(n * 2);
}
```

---

## Методы List<T> ##

### Add / AddRange - Добавление элементов ###

**Описание:** Добавляет элемент или коллекцию элементов в конец списка.

**Когда использовать:**

- Для добавления элементов
- Для объединения коллекций
- Для построения списков динамически

**Примеры:**

```csharp
List<int> numbers = new List<int>();

numbers.Add(1);
numbers.Add(2);
numbers.Add(3); // [1, 2, 3]

// Добавление нескольких элементов
numbers.AddRange(new[] { 4, 5, 6 }); // [1, 2, 3, 4, 5, 6]

// Добавление в начало
numbers.Insert(0, 0); // [0, 1, 2, 3, 4, 5, 6]
numbers.InsertRange(0, new[] { -2, -1 }); // [-2, -1, 0, 1, 2, 3, 4, 5, 6]
```

### Remove / RemoveAll - Удаление элементов ###

**Описание:** Удаляет первое вхождение указанного элемента или все элементы, удовлетворяющие условию.

**Примеры:**

```csharp
List<int> numbers = new List<int> { 1, 2, 2, 3, 4, 2, 5 };

numbers.Remove(2); // [1, 2, 3, 4, 2, 5] - удалил первое вхождение

numbers.RemoveAll(n => n % 2 == 0); // [1, 3, 5] - удалил все четные

numbers.RemoveAt(0); // [3, 5] - удалил элемент по индексу

numbers.RemoveRange(0, 1); // [5] - удалил 1 элемент с индекса 0
```

### Contains - Проверка наличия

**Описание:** Определяет, содержится ли элемент в списке.

**Примеры:**

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

bool contains = numbers.Contains(3); // true

// С кастомным сравнением
List<string> names = new List<string> { "John", "Jane", "Bob" };
bool hasJohn = names.Contains("john", StringComparer.OrdinalIgnoreCase); // true
```

### IndexOf / LastIndexOf - Поиск индекса ###

**Описание:** Возвращает индекс первого/последнего вхождения элемента.

**Примеры:**

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 2, 5 };

int index = numbers.IndexOf(2); // 1
int lastIndex = numbers.LastIndexOf(2); // 3
int notFound = numbers.IndexOf(10); // -1

// С указанием диапазона поиска
int indexInRange = numbers.IndexOf(2, 2, 2); // 3
```

### Sort - Сортировка списка ###

**Описание:** Сортирует элементы списка.

**Примеры:**

```csharp
List<int> numbers = new List<int> { 3, 1, 4, 1, 5, 9, 2, 6 };

numbers.Sort(); // [1, 1, 2, 3, 4, 5, 6, 9]

// С кастомным сравнением
List<string> names = new List<string> { "John", "Jane", "Bob" };
names.Sort((x, y) => x.Length.CompareTo(y.Length)); // Сортировка по длине

// С использованием IComparer
names.Sort(StringComparer.OrdinalIgnoreCase);
```

### Reverse - Обращение списка ###

**Описание:** Обращает порядок элементов списка.

**Примеры:**

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

numbers.Reverse(); // [5, 4, 3, 2, 1]
```

### Find / FindAll - Поиск элементов ###

**Описание:** Находит элементы, удовлетворяющие условию.

**Примеры:**

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

int firstEven = numbers.Find(n => n % 2 == 0); // 2
int lastEven = numbers.FindLast(n => n % 2 == 0); // 10
List<int> allEven = numbers.FindAll(n => n % 2 == 0); // [2, 4, 6, 8, 10]

int index = numbers.FindIndex(n => n > 5); // 5
int lastIndex = numbers.FindLastIndex(n => n > 5); // 9
```

### Exists / TrueForAll - Проверка условий ###

**Описание:** Проверяет наличие элементов или выполнение условия для всех элементов.

**Примеры:**

```csharp
List<int> numbers = new List<int> { 2, 4, 6, 8, 10 };

bool exists = numbers.Exists(n => n % 2 == 0); // true
bool allEven = numbers.TrueForAll(n => n % 2 == 0); // true
```

### ConvertAll - Преобразование элементов

**Описание:** Преобразует все элементы списка в другой тип.

**Примеры:**

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

List<string> strings = numbers.ConvertAll(n => n.ToString()); // ["1", "2", "3", "4", "5"]
List<int> squares = numbers.ConvertAll(n => n * n); // [1, 4, 9, 16, 25]
```

### GetRange - Получение подсписка

**Описание:** Создает поверхностную копию диапазона элементов списка.

**Примеры:**

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

List<int> subList = numbers.GetRange(2, 4); // [3, 4, 5, 6]
```

### Clear - Очистка списка

**Описание:** Удаляет все элементы из списка.

**Примеры:**

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

numbers.Clear(); // []
```

### Capacity - Вместимость списка

**Описание:** Получает или задает общее число элементов, которое может содержать список без изменения размера.

**Примеры:**

```csharp
List<int> numbers = new List<int>();

Console.WriteLine(numbers.Capacity); // 0
numbers.Add(1);
Console.WriteLine(numbers.Capacity); // 4 (автоматически увеличивается)

// Установка вместимости заранее
numbers.Capacity = 100; // Резервирует место для 100 элементов
```

---

## Методы Dictionary<TKey, TValue>

### Add - Добавление элементов

**Описание:** Добавляет элемент с указанным ключом и значением.

**Примеры:**

```csharp
Dictionary<string, int> ages = new Dictionary<string, int>();

ages.Add("John", 30);
ages.Add("Jane", 25);
ages.Add("Bob", 35);

// Через индексатор (перезаписывает при существующем ключе)
ages["Alice"] = 28;

// Проверка перед добавлением
if (!ages.ContainsKey("Charlie"))
{
    ages.Add("Charlie", 40);
}
```

### Remove - Удаление элемента

**Описание:** Удаляет элемент с указанным ключом.

**Примеры:**

```csharp
Dictionary<string, int> ages = new Dictionary<string, int>
{
    ["John"] = 30,
    ["Jane"] = 25,
    ["Bob"] = 35
};

bool removed = ages.Remove("John"); // true
bool notRemoved = ages.Remove("Charlie"); // false (ключа не было)

ages.Clear(); // Удаляет все элементы
```

### ContainsKey / ContainsValue - Проверка наличия

**Описание:** Определяет, содержит ли словарь указанный ключ или значение.

**Примеры:**

```csharp
Dictionary<string, int> ages = new Dictionary<string, int>
{
    ["John"] = 30,
    ["Jane"] = 25
};

bool hasKey = ages.ContainsKey("John"); // true
bool hasValue = ages.ContainsValue(30); // true
```

### TryGetValue - Безопасное получение значения

**Описание:** Получает значение, связанное с указанным ключом, без выбрасывания исключения.

**Когда использовать:**

- Для безопасного доступа к значениям
- Вместо проверки ContainsKey + индексатора
- Для оптимизации производительности

**Примеры:**

```csharp
Dictionary<string, int> ages = new Dictionary<string, int>
{
    ["John"] = 30,
    ["Jane"] = 25
};

if (ages.TryGetValue("John", out int age))
{
    Console.WriteLine($"John is {age} years old"); // John is 30 years old
}

// Безопасно обрабатывает отсутствующий ключ
if (ages.TryGetValue("Charlie", out int charlieAge))
{
    // Этот блок не выполнится
}
else
{
    Console.WriteLine("Charlie not found");
}
```

### Keys / Values - Получение ключей и значений

**Описание:** Получает коллекцию, содержащую ключи или значения словаря.

**Примеры:**

```csharp
Dictionary<string, int> ages = new Dictionary<string, int>
{
    ["John"] = 30,
    ["Jane"] = 25,
    ["Bob"] = 35
};

foreach (string name in ages.Keys)
{
    Console.WriteLine(name); // John, Jane, Bob
}

foreach (int age in ages.Values)
{
    Console.WriteLine(age); // 30, 25, 35
}
```

---

## Полезные комбинации методов

### Фильтрация и преобразование

```csharp
int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

// Найти все четные числа, возвести в квадрат, преобразовать в список
List<int> result = numbers
    .Where(n => n % 2 == 0)
    .Select(n => n * n)
    .ToList(); // [4, 16, 36, 64, 100]
```

### Группировка и агрегация

```csharp
var people = new[]
{
    new { Name = "John", Age = 30, City = "Moscow" },
    new { Name = "Jane", Age = 25, City = "Moscow" },
    new { Name = "Bob", Age = 35, City = "SPB" }
};

// Группировка по городу и средний возраст
var avgAgeByCity = people
    .GroupBy(p => p.City)
    .Select(g => new { City = g.Key, AvgAge = g.Average(p => p.Age) });
```

### Сортировка и выборка

```csharp
int[] numbers = { 3, 1, 4, 1, 5, 9, 2, 6, 5, 3, 5 };

// Топ-3 самых частых числа
var topNumbers = numbers
    .GroupBy(n => n)
    .OrderByDescending(g => g.Count())
    .Take(3)
    .Select(g => g.Key); // [5, 3, 1]
```

---

## Лучшие практики

1. **Используйте `TryGetValue`** вместо `ContainsKey` + индексатора для словарей
2. **Используйте LINQ** для сложных запросов вместо циклов
3. **Используйте `ToList()`** когда нужна материализация коллекции
4. **Используйте `FirstOrDefault`** вместо `First` для безопасного доступа
5. **Используйте `Any()`** вместо `Count() > 0` для проверки наличия элементов
6. **Избегайте множественных вызовов `Count()`** на IEnumerable - материализуйте коллекцию
7. **Используйте `Capacity`** для List когда известен размер заранее
8. **Используйте `RemoveAll`** вместо цикла с `Remove` для удаления по условию
