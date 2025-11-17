# LINQ методы в C# - Полное руководство

## Обзор

Language Integrated Query (LINQ) - это мощная функциональность C# для работы с коллекциями данных. LINQ предоставляет множество методов для фильтрации, преобразования, группировки и агрегации данных.

---

## Фильтрация (Filtering)

### Where - Фильтрация элементов

**Описание:** Фильтрует последовательность значений на основе предиката.

**Когда использовать:**

- Для выборки элементов по условию
- Для фильтрации коллекций
- Для поиска элементов с определенными свойствами

**Примеры:**

```csharp
int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

// Четные числа
var evenNumbers = numbers.Where(n => n % 2 == 0); // [2, 4, 6, 8, 10]

// Числа больше 5
var greaterThan5 = numbers.Where(n => n > 5); // [6, 7, 8, 9, 10]

// С индексом
var withIndex = numbers.Where((n, index) => n > index); // [1, 3, 5, 7, 9]

// Для строк
string[] names = { "Alice", "Bob", "Charlie", "David" };
var longNames = names.Where(name => name.Length > 4); // ["Alice", "Charlie", "David"]
```

### OfType - Фильтрация по типу

**Описание:** Фильтрует элементы указанного типа.

**Когда использовать:**

- Для работы с коллекциями разных типов
- Для извлечения элементов конкретного типа
- Для фильтрации объектов по типу

**Примеры:**

```csharp
object[] items = { 1, "hello", 2, "world", 3.5 };

var strings = items.OfType<string>(); // ["hello", "world"]
var integers = items.OfType<int>(); // [1, 2]
```

---

## Проекция (Projection)

### Select - Преобразование элементов

**Описание:** Проецирует каждый элемент последовательности в новую форму.

**Когда использовать:**

- Для преобразования элементов коллекции
- Для извлечения свойств объектов
- Для создания новых объектов из существующих

**Примеры:**

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

// Возведение в квадрат
var squares = numbers.Select(n => n * n); // [1, 4, 9, 16, 25]

// Преобразование в строки
var strings = numbers.Select(n => n.ToString()); // ["1", "2", "3", "4", "5"]

// С индексом
var indexed = numbers.Select((n, index) => $"Index {index}: {n}");

// Извлечение свойств
var people = new[] { new { Name = "John", Age = 30 }, new { Name = "Jane", Age = 25 } };
var names = people.Select(p => p.Name); // ["John", "Jane"]

// Преобразование в другой тип
var ages = people.Select(p => new { PersonName = p.Name, Years = p.Age });
```

### SelectMany - Выравнивание вложенных коллекций

**Описание:** Проецирует каждый элемент последовательности в IEnumerable<T> и объединяет результирующие последовательности в одну последовательность.

**Когда использовать:**

- Для "расплющивания" вложенных коллекций
- Для объединения коллекций из коллекций
- Для работы с матрицами

**Примеры:**

```csharp
string[] words = { "Hello", "World" };

// Разбить каждое слово на символы и объединить
var chars = words.SelectMany(word => word.ToCharArray()); 
// ['H', 'e', 'l', 'l', 'o', 'W', 'o', 'r', 'l', 'd']

// С индексом
var charsWithIndex = words.SelectMany((word, index) => word.Select(c => new { WordIndex = index, Char = c }));

// Вложенные коллекции
int[][] matrix = { new[] { 1, 2 }, new[] { 3, 4 }, new[] { 5, 6 } };
var flattened = matrix.SelectMany(row => row); // [1, 2, 3, 4, 5, 6]
```

---

## Сортировка (Sorting)

### OrderBy / OrderByDescending - Сортировка по возрастанию/убыванию

**Описание:** Сортирует элементы последовательности в порядке возрастания/убывания.

**Когда использовать:**

- Для сортировки коллекций
- Для упорядочивания данных перед выводом
- Для подготовки данных к дальнейшей обработке

**Примеры:**

```csharp
int[] numbers = { 3, 1, 4, 1, 5, 9, 2, 6 };

// По возрастанию
var ascending = numbers.OrderBy(n => n); // [1, 1, 2, 3, 4, 5, 6, 9]

// По убыванию
var descending = numbers.OrderByDescending(n => n); // [9, 6, 5, 4, 3, 2, 1, 1]

// Сортировка объектов
var people = new[] { new { Name = "John", Age = 30 }, new { Name = "Jane", Age = 25 } };
var sortedByAge = people.OrderBy(p => p.Age);
var sortedByName = people.OrderBy(p => p.Name);
```

### ThenBy / ThenByDescending - Вторичная сортировка

**Описание:** Выполняет дополнительную сортировку элементов последовательности в порядке возрастания/убывания.

**Когда использовать:**

- Для многоуровневой сортировки
- Когда первичная сортировка недостаточна
- Для сортировки по нескольким полям

**Примеры:**

```csharp
var people = new[] 
{ 
    new { Name = "John", Age = 30, City = "Moscow" },
    new { Name = "Jane", Age = 25, City = "Moscow" },
    new { Name = "Bob", Age = 30, City = "SPB" }
};

// Сортировка по возрасту, затем по имени
var sorted = people.OrderBy(p => p.Age).ThenBy(p => p.Name);

// Сортировка по городу, затем по возрасту (убывание)
var sorted2 = people.OrderBy(p => p.City).ThenByDescending(p => p.Age);
```

### Reverse - Обращение порядка

**Описание:** Изменяет порядок элементов последовательности на противоположный.

**Когда использовать:**

- Для изменения порядка элементов
- Для получения элементов в обратном порядке
- Для обработки данных с конца

**Примеры:**

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };
var reversed = numbers.Reverse(); // [5, 4, 3, 2, 1]

// Обращение строки через LINQ
string text = "Hello";
var reversedChars = text.Reverse().ToArray(); // ['o', 'l', 'l', 'e', 'H']
var reversedString = new string(reversedChars); // "olleH"
```

---

## Группировка (Grouping)

### GroupBy - Группировка элементов

**Описание:** Группирует элементы последовательности согласно указанной функции селектора ключа.

**Когда использовать:**

- Для группировки данных по ключу
- Для создания словарей из коллекций
- Для агрегации данных по группам

**Примеры:**

```csharp
var people = new[]
{
    new { Name = "John", Age = 30, City = "Moscow" },
    new { Name = "Jane", Age = 25, City = "Moscow" },
    new { Name = "Bob", Age = 30, City = "SPB" }
};

// Группировка по городу
var groupedByCity = people.GroupBy(p => p.City);
// Москва: [John, Jane]
// SPB: [Bob]

// Группировка по возрасту
var groupedByAge = people.GroupBy(p => p.Age);
// 30: [John, Bob]
// 25: [Jane]

// С преобразованием элементов группы
var grouped = people.GroupBy(p => p.City, p => p.Name);
// Москва: ["John", "Jane"]
// SPB: ["Bob"]

// С кастомным результатом
var result = people.GroupBy(
    p => p.City,
    (key, group) => new { City = key, Count = group.Count(), Names = group.Select(p => p.Name) }
);
```

---

## Агрегация (Aggregation)

### Count - Подсчет элементов

**Описание:** Возвращает количество элементов в последовательности.

**Когда использовать:**

- Для подсчета элементов
- Для проверки размера коллекции
- Для подсчета элементов, удовлетворяющих условию

**Примеры:**

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

int count = numbers.Count(); // 5

// С условием
int evenCount = numbers.Count(n => n % 2 == 0); // 2

// Для строк
string[] names = { "Alice", "Bob", "Charlie" };
int longNamesCount = names.Count(name => name.Length > 4); // 1
```

### Sum - Сумма элементов

**Описание:** Вычисляет сумму последовательности числовых значений.

**Когда использовать:**

- Для подсчета суммы чисел
- Для агрегации числовых данных
- Для расчетов

**Примеры:**

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

int sum = numbers.Sum(); // 15

// С преобразованием
var people = new[] { new { Name = "John", Salary = 50000 }, new { Name = "Jane", Salary = 60000 } };
int totalSalary = people.Sum(p => p.Salary); // 110000

// Для других типов
decimal[] prices = { 19.99m, 29.99m, 9.99m };
decimal totalPrice = prices.Sum(); // 59.97
```

### Average - Среднее значение

**Описание:** Вычисляет среднее значение последовательности числовых значений.

**Когда использовать:**

- Для расчета среднего значения
- Для статистических вычислений
- Для анализа данных

**Примеры:**

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

double average = numbers.Average(); // 3.0

// С преобразованием
var people = new[] { new { Name = "John", Age = 30 }, new { Name = "Jane", Age = 25 } };
double avgAge = people.Average(p => p.Age); // 27.5
```

### Min / Max - Минимальное/максимальное значение

**Описание:** Возвращает минимальное/максимальное значение в последовательности.

**Когда использовать:**

- Для поиска минимального/максимального значения
- Для определения границ данных
- Для анализа диапазонов

**Примеры:**

```csharp
int[] numbers = { 3, 1, 4, 1, 5, 9, 2, 6 };

int min = numbers.Min(); // 1
int max = numbers.Max(); // 9

// С преобразованием
var people = new[] { new { Name = "John", Age = 30 }, new { Name = "Jane", Age = 25 } };
int minAge = people.Min(p => p.Age); // 25
int maxAge = people.Max(p => p.Age); // 30

// Для строк (лексикографическое сравнение)
string[] names = { "Alice", "Bob", "Charlie" };
string minName = names.Min(); // "Alice"
string maxName = names.Max(); // "Charlie"
```

### Aggregate - Пользовательская агрегация

**Описание:** Применяет функцию аккумулятора к последовательности.

**Когда использовать:**

- Для сложных агрегаций
- Когда нужна кастомная логика
- Для цепочечных вычислений

**Примеры:**

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

// Произведение всех чисел
int product = numbers.Aggregate((acc, n) => acc * n); // 120

// С начальным значением
int sum = numbers.Aggregate(10, (acc, n) => acc + n); // 25 (10 + 1 + 2 + 3 + 4 + 5)

// Конкатенация строк
string[] words = { "Hello", " ", "World", "!" };
string sentence = words.Aggregate((acc, word) => acc + word); // "Hello World!"

// Построение строки из чисел
string numbersStr = numbers.Aggregate("Numbers: ", (acc, n) => acc + n + ", ");
```

---

## Работа с элементами

### First / FirstOrDefault - Первый элемент

**Описание:** Возвращает первый элемент последовательности или значение по умолчанию.

**Когда использовать:**

- Для получения первого элемента
- Когда нужен один элемент из коллекции
- Для безопасного доступа к первому элементу

**Примеры:**

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

int first = numbers.First(); // 1
int firstEven = numbers.First(n => n % 2 == 0); // 2

// Безопасная версия (возвращает 0 если не найдено)
int firstOrDefault = numbers.FirstOrDefault(n => n > 10); // 0

// Для пустой коллекции
int[] empty = { };
int firstFromEmpty = empty.FirstOrDefault(); // 0 (не выбросит исключение)
// empty.First(); // Выбросит InvalidOperationException
```

### Last / LastOrDefault - Последний элемент

**Описание:** Возвращает последний элемент последовательности или значение по умолчанию.

**Когда использовать:**

- Для получения последнего элемента
- Для доступа к хвосту коллекции
- Когда порядок важен

**Примеры:**

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

int last = numbers.Last(); // 5
int lastEven = numbers.Last(n => n % 2 == 0); // 4

int lastOrDefault = numbers.LastOrDefault(n => n > 10); // 0
```

### Single / SingleOrDefault - Единственный элемент

**Описание:** Возвращает единственный элемент последовательности или значение по умолчанию.

**Когда использовать:**

- Когда ожидается ровно один элемент
- Для валидации уникальности
- Когда множественные элементы - ошибка

**Примеры:**

```csharp
int[] numbers = { 5 };

int single = numbers.Single(); // 5

int[] multiple = { 1, 2, 3 };
// multiple.Single(); // Выбросит исключение (больше одного элемента)

int[] empty = { };
int singleOrDefault = empty.SingleOrDefault(); // 0

// С условием
int singleEven = numbers.Single(n => n == 5); // 5
```

### ElementAt / ElementAtOrDefault - Элемент по индексу

**Описание:** Возвращает элемент по указанному индексу в последовательности.

**Когда использовать:**

- Для доступа к элементу по индексу
- Когда нужен элемент на определенной позиции
- Для безопасного доступа к индексу

**Примеры:**

```csharp
int[] numbers = { 10, 20, 30, 40, 50 };

int element = numbers.ElementAt(2); // 30

int elementOrDefault = numbers.ElementAtOrDefault(10); // 0 (не выбросит исключение)
// numbers.ElementAt(10); // Выбросит ArgumentOutOfRangeException
```

---

## Удаление дубликатов и объединение

### Distinct - Уникальные элементы

**Описание:** Возвращает различающиеся элементы последовательности.

**Когда использовать:**

- Для удаления дубликатов
- Для получения уникальных значений
- Для очистки данных

**Примеры:**

```csharp
int[] numbers = { 1, 2, 2, 3, 3, 3, 4, 5 };

var distinct = numbers.Distinct(); // [1, 2, 3, 4, 5]

// Для объектов (нужен IEqualityComparer или переопределенный Equals)
var people = new[] { new { Name = "John" }, new { Name = "Jane" }, new { Name = "John" } };
var distinctPeople = people.Distinct(); // Может не работать без сравнения

// С кастомным сравнением
var distinctByLength = people.DistinctBy(p => p.Name); // C# 10+
```

### Union - Объединение множеств

**Описание:** Находит объединение двух множеств (все уникальные элементы из обеих последовательностей).

**Когда использовать:**

- Для объединения коллекций
- Для получения всех уникальных элементов
- Для слияния данных

**Примеры:**

```csharp
int[] numbers1 = { 1, 2, 3 };
int[] numbers2 = { 3, 4, 5 };

var union = numbers1.Union(numbers2); // [1, 2, 3, 4, 5]
```

### Intersect - Пересечение множеств

**Описание:** Находит пересечение двух множеств (элементы, присутствующие в обеих последовательностях).

**Когда использовать:**

- Для поиска общих элементов
- Для сравнения коллекций
- Для фильтрации по наличию в другой коллекции

**Примеры:**

```csharp
int[] numbers1 = { 1, 2, 3, 4 };
int[] numbers2 = { 3, 4, 5, 6 };

var intersect = numbers1.Intersect(numbers2); // [3, 4]
```

### Except - Разность множеств

**Описание:** Находит разность двух множеств (элементы первой последовательности, отсутствующие во второй).

**Когда использовать:**

- Для поиска элементов, которых нет в другой коллекции
- Для удаления элементов по списку
- Для фильтрации исключений

**Примеры:**

```csharp
int[] numbers1 = { 1, 2, 3, 4 };
int[] numbers2 = { 3, 4, 5, 6 };

var except = numbers1.Except(numbers2); // [1, 2]
```

### Concat - Объединение последовательностей

**Описание:** Объединяет две последовательности (сохраняет дубликаты).

**Когда использовать:**

- Для объединения коллекций с сохранением всех элементов
- Для добавления элементов к коллекции
- Когда нужны все элементы из обеих коллекций

**Примеры:**

```csharp
int[] numbers1 = { 1, 2, 3 };
int[] numbers2 = { 4, 5, 6 };

var concat = numbers1.Concat(numbers2); // [1, 2, 3, 4, 5, 6]
```

---

## Проверки (Checking)

### Any - Проверка наличия элементов

**Описание:** Определяет, содержит ли последовательность какие-либо элементы или элементы, удовлетворяющие условию.

**Когда использовать:**

- Для проверки наличия элементов
- Для валидации данных
- Для быстрой проверки условий

**Примеры:**

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

bool hasElements = numbers.Any(); // true
bool hasEven = numbers.Any(n => n % 2 == 0); // true
bool hasNegative = numbers.Any(n => n < 0); // false

int[] empty = { };
bool hasAny = empty.Any(); // false
```

### All - Проверка всех элементов

**Описание:** Определяет, все ли элементы последовательности удовлетворяют условию.

**Когда использовать:**

- Для валидации всех элементов
- Для проверки условий на всю коллекцию
- Для утверждений

**Примеры:**

```csharp
int[] numbers = { 2, 4, 6, 8 };

bool allEven = numbers.All(n => n % 2 == 0); // true
bool allPositive = numbers.All(n => n > 0); // true
bool allGreaterThan5 = numbers.All(n => n > 5); // false
```

### Contains - Проверка наличия элемента

**Описание:** Определяет, содержит ли последовательность указанный элемент.

**Когда использовать:**

- Для проверки наличия конкретного элемента
- Для поиска значения
- Для валидации членства

**Примеры:**

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

bool contains3 = numbers.Contains(3); // true
bool contains10 = numbers.Contains(10); // false

// С кастомным сравнением
string[] names = { "John", "Jane", "Bob" };
bool containsJohn = names.Contains("john", StringComparer.OrdinalIgnoreCase); // true
```

---

## Ограничение и пропуск

### Take - Взять первые N элементов

**Описание:** Возвращает указанное число подряд идущих элементов с начала последовательности.

**Когда использовать:**

- Для ограничения количества элементов
- Для пагинации
- Для получения первых элементов

**Примеры:**

```csharp
int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

var first5 = numbers.Take(5); // [1, 2, 3, 4, 5]
var first3 = numbers.Take(3); // [1, 2, 3]

// С условием
var firstEven = numbers.TakeWhile(n => n % 2 == 0); // [] (первый элемент нечетный)
var firstLessThan5 = numbers.TakeWhile(n => n < 5); // [1, 2, 3, 4]
```

### Skip - Пропустить первые N элементов

**Описание:** Пропускает указанное число элементов в последовательности и возвращает остальные элементы.

**Когда использовать:**

- Для пропуска элементов
- Для пагинации
- Для получения элементов после определенной позиции

**Примеры:**

```csharp
int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

var skip5 = numbers.Skip(5); // [6, 7, 8, 9, 10]
var skip3 = numbers.Skip(3); // [4, 5, 6, 7, 8, 9, 10]

// С условием
var skipWhileLessThan5 = numbers.SkipWhile(n => n < 5); // [5, 6, 7, 8, 9, 10]
```

### TakeWhile / SkipWhile - Условное ограничение/пропуск

**Описание:** Возвращает/пропускает элементы последовательности, пока условие истинно.

**Примеры:**

```csharp
int[] numbers = { 2, 4, 6, 7, 8, 9, 10 };

var takeWhileEven = numbers.TakeWhile(n => n % 2 == 0); // [2, 4, 6]
var skipWhileEven = numbers.SkipWhile(n => n % 2 == 0); // [7, 8, 9, 10]
```

---

## Преобразование типов

### ToList - Преобразование в List

**Описание:** Создает List<T> из IEnumerable<T>.

**Когда использовать:**

- Когда нужен List вместо IEnumerable
- Для множественного доступа к коллекции
- Для методов, требующих List

**Примеры:**

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

List<int> list = numbers.ToList();
List<int> evenList = numbers.Where(n => n % 2 == 0).ToList();
```

### ToArray - Преобразование в массив

**Описание:** Создает массив из IEnumerable<T>.

**Когда использовать:**

- Когда нужен массив вместо IEnumerable
- Для методов, требующих массив
- Для фиксированного размера

**Примеры:**

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

int[] array = numbers.ToArray();
int[] evenArray = numbers.Where(n => n % 2 == 0).ToArray();
```

### ToDictionary - Преобразование в Dictionary

**Описание:** Создает Dictionary<TKey, TValue> из IEnumerable<T>.

**Когда использовать:**

- Для быстрого поиска по ключу
- Для преобразования в словарь
- Когда нужен доступ по ключу

**Примеры:**

```csharp
var people = new[]
{
    new { Id = 1, Name = "John" },
    new { Id = 2, Name = "Jane" }
};

// По ключу
Dictionary<int, string> dict = people.ToDictionary(p => p.Id, p => p.Name);
// { 1: "John", 2: "Jane" }

// Без селектора значения (весь объект)
Dictionary<int, object> dict2 = people.ToDictionary(p => p.Id);
```

### ToLookup - Преобразование в Lookup

**Описание:** Создает Lookup<TKey, TElement> из IEnumerable<T>.

**Когда использовать:**

- Для группировки с множественными значениями
- Для быстрого поиска по ключу с несколькими значениями
- Когда ключи могут повторяться

**Примеры:**

```csharp
var people = new[]
{
    new { Name = "John", City = "Moscow" },
    new { Name = "Jane", City = "Moscow" },
    new { Name = "Bob", City = "SPB" }
};

var lookup = people.ToLookup(p => p.City);
var moscowPeople = lookup["Moscow"]; // [John, Jane]
var spbPeople = lookup["SPB"]; // [Bob]
```

### Cast - Преобразование типа элементов

**Описание:** Приводит элементы последовательности к указанному типу.

**Когда использовать:**

- Для преобразования типа элементов
- При работе с необобщенными коллекциями
- Для приведения базового типа к производному

**Примеры:**

```csharp
ArrayList list = new ArrayList { 1, 2, 3, 4, 5 };

IEnumerable<int> numbers = list.Cast<int>();

// Для объектов
object[] objects = { 1, "hello", 2, "world" };
IEnumerable<string> strings = objects.OfType<string>(); // Безопаснее чем Cast
```

---

## Создание последовательностей

### Range - Создание диапазона чисел

**Описание:** Генерирует последовательность целых чисел в указанном диапазоне.

**Когда использовать:**

- Для генерации чисел
- Для создания последовательностей
- Для циклов в LINQ стиле

**Примеры:**

```csharp
// Числа от 1 до 10
var numbers = Enumerable.Range(1, 10); // [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]

// Квадраты чисел от 1 до 5
var squares = Enumerable.Range(1, 5).Select(n => n * n); // [1, 4, 9, 16, 25]

// Четные числа от 0 до 20
var evens = Enumerable.Range(0, 21).Where(n => n % 2 == 0); // [0, 2, 4, ..., 20]
```

### Repeat - Повторение элемента

**Описание:** Генерирует последовательность, содержащую одно повторяющееся значение.

**Когда использовать:**

- Для создания повторяющихся значений
- Для инициализации коллекций
- Для тестовых данных

**Примеры:**

```csharp
// Пять нулей
var zeros = Enumerable.Repeat(0, 5); // [0, 0, 0, 0, 0]

// Три строки "Hello"
var hellos = Enumerable.Repeat("Hello", 3); // ["Hello", "Hello", "Hello"]
```

### Empty - Пустая последовательность

**Описание:** Возвращает пустую последовательность указанного типа.

**Когда использовать:**

- Для возврата пустой коллекции
- Для инициализации
- Для обработки отсутствующих данных

**Примеры:**

```csharp
var empty = Enumerable.Empty<int>(); // []
var emptyStrings = Enumerable.Empty<string>(); // []
```

---

## Полезные комбинации

### Пагинация

```csharp
int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
int pageSize = 3;
int pageNumber = 2;

var page = numbers.Skip((pageNumber - 1) * pageSize).Take(pageSize); // [4, 5, 6]
```

### Поиск дубликатов

```csharp
int[] numbers = { 1, 2, 2, 3, 3, 3, 4, 5 };
var duplicates = numbers.GroupBy(n => n).Where(g => g.Count() > 1).Select(g => g.Key); // [2, 3]
```

### Статистика

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };
var stats = new
{
    Count = numbers.Count(),
    Sum = numbers.Sum(),
    Average = numbers.Average(),
    Min = numbers.Min(),
    Max = numbers.Max()
};
```
