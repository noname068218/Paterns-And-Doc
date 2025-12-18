# Dictionary (Словарь) в C#

## Введение

**Dictionary<TKey, TValue>** — это коллекция в C#, которая хранит пары ключ-значение. Это одна из самых полезных и часто используемых коллекций для быстрого поиска данных по ключу.

---

## 1. Что такое Dictionary?

### Определение

Dictionary — это структура данных, которая:
- Хранит элементы в виде пар **ключ-значение**
- Обеспечивает **быстрый поиск** по ключу (O(1) в среднем)
- Гарантирует **уникальность ключей**
- Позволяет эффективно добавлять, удалять и обновлять элементы

### Аналогия

```
┌─────────────────────────────────────────────┐
│  Dictionary = Телефонная книга              │
│                                              │
│  Ключ (Key)    →  Значение (Value)          │
│  "Иван"        →  "+7-999-123-45-67"        │
│  "Мария"       →  "+7-999-234-56-78"        │
│  "Петр"        →  "+7-999-345-67-89"        │
└─────────────────────────────────────────────┘
```

### Диаграмма: Структура Dictionary

```
┌─────────────────────────────────────────────┐
│  Dictionary<string, int>                    │
│                                             │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐ │
│  │ Key      │  │ Key      │  │ Key      │ │
│  │ "John"   │  │ "Jane"   │  │ "Bob"    │ │
│  │          │  │          │  │          │ │
│  │ Value    │  │ Value    │  │ Value    │ │
│  │ 30       │  │ 25       │  │ 28       │ │
│  └──────────┘  └──────────┘  └──────────┘ │
│                                             │
│  Быстрый поиск по ключу O(1)               │
└─────────────────────────────────────────────┘
```

---

## 2. Создание и инициализация Dictionary

### Базовое создание

```csharp
// Пустой словарь
var dict = new Dictionary<string, int>();

// С начальной ёмкостью
var dict2 = new Dictionary<string, int>(100);

// С инициализацией
var dict3 = new Dictionary<string, int> {
    { "John", 30 },
    { "Jane", 25 },
    { "Bob", 28 }
};

// Альтернативный синтаксис инициализации
var dict4 = new Dictionary<string, int> {
    ["John"] = 30,
    ["Jane"] = 25,
    ["Bob"] = 28
};
```

### Инициализация из коллекции

```csharp
// Из массива кортежей
var pairs = new[] {
    ("John", 30),
    ("Jane", 25),
    ("Bob", 28)
};
var dict = pairs.ToDictionary(p => p.Item1, p => p.Item2);

// Из списка объектов
var people = new List<Person> {
    new Person { Name = "John", Age = 30 },
    new Person { Name = "Jane", Age = 25 }
};
var dict = people.ToDictionary(p => p.Name, p => p.Age);
```

---

## 3. Основные операции

### Добавление элементов

```csharp
var dict = new Dictionary<string, int>();

// Метод Add
dict.Add("John", 30);
dict.Add("Jane", 25);

// Индексатор (если ключ существует, обновит значение)
dict["Bob"] = 28;
dict["John"] = 31; // Обновит значение для "John"

// AddRange (через цикл)
var newPairs = new[] { ("Alice", 22), ("Charlie", 35) };
foreach (var (key, value) in newPairs) {
    dict[key] = value;
}
```

### Получение значений

```csharp
var dict = new Dictionary<string, int> {
    { "John", 30 },
    { "Jane", 25 }
};

// Через индексатор (выбросит исключение, если ключ не найден)
int johnAge = dict["John"]; // 30
// int bobAge = dict["Bob"]; // ❌ KeyNotFoundException!

// Безопасное получение через TryGetValue
if (dict.TryGetValue("John", out int age)) {
    Console.WriteLine($"Возраст: {age}"); // Возраст: 30
}

// С проверкой существования
if (dict.ContainsKey("John")) {
    int age = dict["John"];
}

// Получение с значением по умолчанию
int age = dict.GetValueOrDefault("Bob", 0); // 0, если ключ не найден
```

### Обновление элементов

```csharp
var dict = new Dictionary<string, int> {
    { "John", 30 }
};

// Обновление через индексатор
dict["John"] = 31;

// Обновление с проверкой
if (dict.ContainsKey("John")) {
    dict["John"] = 31;
}

// TryGetValue + обновление
if (dict.TryGetValue("John", out int currentAge)) {
    dict["John"] = currentAge + 1;
}
```

### Удаление элементов

```csharp
var dict = new Dictionary<string, int> {
    { "John", 30 },
    { "Jane", 25 },
    { "Bob", 28 }
};

// Удаление по ключу
bool removed = dict.Remove("John"); // true, если удалён

// Удаление с проверкой
if (dict.ContainsKey("Jane")) {
    dict.Remove("Jane");
}

// Очистка всего словаря
dict.Clear();
```

---

## 4. Итерация по Dictionary

### Перебор всех элементов

```csharp
var dict = new Dictionary<string, int> {
    { "John", 30 },
    { "Jane", 25 },
    { "Bob", 28 }
};

// Перебор пар ключ-значение
foreach (var kvp in dict) {
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
}

// Использование KeyValuePair явно
foreach (KeyValuePair<string, int> kvp in dict) {
    Console.WriteLine($"Ключ: {kvp.Key}, Значение: {kvp.Value}");
}

// Деструктуризация (C# 7+)
foreach (var (key, value) in dict) {
    Console.WriteLine($"{key}: {value}");
}
```

### Перебор только ключей или значений

```csharp
var dict = new Dictionary<string, int> {
    { "John", 30 },
    { "Jane", 25 },
    { "Bob", 28 }
};

// Только ключи
foreach (string key in dict.Keys) {
    Console.WriteLine($"Ключ: {key}");
}

// Только значения
foreach (int value in dict.Values) {
    Console.WriteLine($"Значение: {value}");
}

// Преобразование в списки
List<string> keys = dict.Keys.ToList();
List<int> values = dict.Values.ToList();
```

---

## 5. Проверка существования

### ContainsKey и ContainsValue

```csharp
var dict = new Dictionary<string, int> {
    { "John", 30 },
    { "Jane", 25 }
};

// Проверка ключа (быстро, O(1))
bool hasJohn = dict.ContainsKey("John"); // true
bool hasBob = dict.ContainsKey("Bob");   // false

// Проверка значения (медленно, O(n))
bool hasAge30 = dict.ContainsValue(30);  // true
bool hasAge40 = dict.ContainsValue(40);  // false
```

### TryGetValue (рекомендуемый способ)

```csharp
var dict = new Dictionary<string, int> {
    { "John", 30 }
};

// ✅ РЕКОМЕНДУЕТСЯ: TryGetValue
if (dict.TryGetValue("John", out int age)) {
    Console.WriteLine($"Возраст: {age}");
} else {
    Console.WriteLine("Ключ не найден");
}

// ❌ НЕ РЕКОМЕНДУЕТСЯ: ContainsKey + индексатор (двойной поиск)
if (dict.ContainsKey("John")) {
    int age = dict["John"]; // Второй поиск!
}
```

---

## 6. Практические примеры

### Пример 1: Подсчёт частоты элементов

```csharp
// Подсчёт частоты слов в тексте
string text = "hello world hello csharp world";
string[] words = text.Split(' ');

var wordCount = new Dictionary<string, int>();

foreach (string word in words) {
    if (wordCount.ContainsKey(word)) {
        wordCount[word]++;
    } else {
        wordCount[word] = 1;
    }
}

// Или более элегантно
var wordCount2 = new Dictionary<string, int>();
foreach (string word in words) {
    wordCount2[word] = wordCount2.GetValueOrDefault(word, 0) + 1;
}

// Или с LINQ
var wordCount3 = words
    .GroupBy(w => w)
    .ToDictionary(g => g.Key, g => g.Count());

// Вывод
foreach (var kvp in wordCount) {
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
}
// hello: 2
// world: 2
// csharp: 1
```

### Пример 2: Кэш с ограничением размера

```csharp
public class LRUCache<TKey, TValue> {
    private readonly Dictionary<TKey, TValue> _cache;
    private readonly Queue<TKey> _accessOrder;
    private readonly int _maxSize;

    public LRUCache(int maxSize) {
        _cache = new Dictionary<TKey, TValue>(maxSize);
        _accessOrder = new Queue<TKey>();
        _maxSize = maxSize;
    }

    public TValue Get(TKey key) {
        if (_cache.TryGetValue(key, out TValue value)) {
            // Обновляем порядок доступа
            _accessOrder.Enqueue(key);
            return value;
        }
        return default(TValue);
    }

    public void Set(TKey key, TValue value) {
        if (_cache.Count >= _maxSize && !_cache.ContainsKey(key)) {
            // Удаляем самый старый элемент
            TKey oldestKey = _accessOrder.Dequeue();
            _cache.Remove(oldestKey);
        }
        _cache[key] = value;
        _accessOrder.Enqueue(key);
    }
}
```

### Пример 3: Группировка данных

```csharp
public class Person {
    public string Name { get; set; }
    public string City { get; set; }
    public int Age { get; set; }
}

var people = new List<Person> {
    new Person { Name = "John", City = "Moscow", Age = 30 },
    new Person { Name = "Jane", City = "Moscow", Age = 25 },
    new Person { Name = "Bob", City = "SPB", Age = 28 },
    new Person { Name = "Alice", City = "SPB", Age = 22 }
};

// Группировка по городу
var groupedByCity = new Dictionary<string, List<Person>>();

foreach (var person in people) {
    if (!groupedByCity.ContainsKey(person.City)) {
        groupedByCity[person.City] = new List<Person>();
    }
    groupedByCity[person.City].Add(person);
}

// Или с LINQ
var groupedByCity2 = people
    .GroupBy(p => p.City)
    .ToDictionary(g => g.Key, g => g.ToList());

// Использование
foreach (var cityGroup in groupedByCity) {
    Console.WriteLine($"Город: {cityGroup.Key}");
    foreach (var person in cityGroup.Value) {
        Console.WriteLine($"  - {person.Name}, {person.Age} лет");
    }
}
```

---

## 7. Производительность и сложность

### Таблица сложности операций

| Операция | Сложность | Описание |
|----------|-----------|----------|
| **Доступ по ключу** | O(1) средняя | Быстрый поиск |
| **Добавление** | O(1) средняя | Быстрое добавление |
| **Удаление** | O(1) средняя | Быстрое удаление |
| **ContainsKey** | O(1) средняя | Быстрая проверка |
| **ContainsValue** | O(n) | Медленная проверка |
| **Итерация** | O(n) | Линейная по количеству элементов |

### Факторы, влияющие на производительность

```csharp
// ✅ Хорошо: Укажите начальную ёмкость, если знаете размер
var dict = new Dictionary<string, int>(1000);

// ✅ Хорошо: Используйте TryGetValue вместо ContainsKey + индексатор
if (dict.TryGetValue(key, out int value)) {
    // Используем value
}

// ❌ Плохо: Двойной поиск
if (dict.ContainsKey(key)) {
    int value = dict[key]; // Второй поиск!
}
```

---

## 8. Профилирование и сложность алгоритмов

### Что такое Big O нотация?

**Big O нотация** — это математический способ описания того, как время выполнения или использование памяти алгоритма растёт с увеличением размера входных данных.

### Основные типы сложности

#### O(1) — Константная сложность

```csharp
// Время выполнения не зависит от размера данных
var dict = new Dictionary<string, int> { /* 1000 элементов */ };
int value = dict["key"]; // Всегда одинаковое время, независимо от размера

// Примеры операций O(1) в Dictionary:
// - dict[key] = value (доступ по ключу)
// - dict.ContainsKey(key)
// - dict.Remove(key)
```

**Характеристики:**
- ✅ Время выполнения **постоянное**
- ✅ Не зависит от количества элементов
- ✅ **Идеальная** производительность

**Диаграмма:**
```
Время выполнения
    │
    │─────────────────────────────── O(1) - Константа
    │
    │
    └─────────────────────────────── Размер данных
```

#### O(n) — Линейная сложность

```csharp
// Время выполнения пропорционально размеру данных
var list = new List<Person> { /* 1000 элементов */ };
var person = list.FirstOrDefault(p => p.Id == 123); 
// В худшем случае нужно проверить все 1000 элементов

// Примеры операций O(n):
// - Поиск в List/Array
// - dict.ContainsValue(value) - поиск по значению
// - Итерация по всем элементам
```

**Характеристики:**
- ⚠️ Время выполнения **линейно** растёт
- ⚠️ Прямо пропорционально количеству элементов
- ⚠️ При удвоении данных — удваивается время

**Диаграмма:**
```
Время выполнения
    │     ╱
    │    ╱
    │   ╱  O(n) - Линейная
    │  ╱
    │ ╱
    └─────────────────────────────── Размер данных
```

#### O(n²) — Квадратичная сложность

```csharp
// Время выполнения пропорционально квадрату размера данных
var list1 = new List<Person> { /* 100 элементов */ };
var list2 = new List<Person> { /* 100 элементов */ };

// Поиск пересечений - O(n²)
foreach (var p1 in list1) {
    foreach (var p2 in list2) {
        if (p1.Id == p2.Id) {
            // Найдено совпадение
        }
    }
}
// 100 * 100 = 10,000 операций сравнения
```

**Характеристики:**
- ❌ Время выполнения **квадратично** растёт
- ❌ При удвоении данных — время увеличивается в 4 раза
- ❌ **Медленно** для больших данных

**Диаграмма:**
```
Время выполнения
    │    ╱
    │   ╱
    │  ╱  O(n²) - Квадратичная
    │ ╱
    │╱
    └─────────────────────────────── Размер данных
```

### Сравнительная таблица сложности

| Операция | Dictionary | List/Array | Описание |
|----------|------------|------------|----------|
| **Поиск по ключу/индексу** | O(1) | O(1) | Оба быстрые |
| **Поиск по значению** | O(n) | O(n) | Оба медленные |
| **Добавление в конец** | O(1) | O(1) | Оба быстрые |
| **Вставка в начало** | O(1) | O(n) | Dictionary быстрее |
| **Удаление** | O(1) | O(n) | Dictionary быстрее |
| **Проверка существования** | O(1) | O(n) | Dictionary быстрее |

### Как оценивать производительность кода?

#### 1. Анализ сложности

```csharp
// ❌ ПЛОХО: O(n²) - вложенные циклы
public bool ContainsDuplicate(List<int> numbers) {
    for (int i = 0; i < numbers.Count; i++) {
        for (int j = i + 1; j < numbers.Count; j++) {
            if (numbers[i] == numbers[j]) {
                return true;
            }
        }
    }
    return false;
}

// ✅ ХОРОШО: O(n) - использование Dictionary
public bool ContainsDuplicate(List<int> numbers) {
    var seen = new Dictionary<int, bool>();
    foreach (int num in numbers) {
        if (seen.ContainsKey(num)) {
            return true;
        }
        seen[num] = true;
    }
    return false;
}
```

#### 2. Профилирование с помощью Stopwatch

```csharp
using System.Diagnostics;

// Измерение времени выполнения
var stopwatch = Stopwatch.StartNew();

// Операция, которую нужно измерить
var dict = new Dictionary<int, string>();
for (int i = 0; i < 1000000; i++) {
    dict[i] = $"Value {i}";
}

stopwatch.Stop();
Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
```

#### 3. Анализ памяти

```csharp
// Dictionary использует больше памяти, чем List
// Но обеспечивает быстрый поиск

// List: ~24 байта на элемент (для int)
var list = new List<int>(1000000);

// Dictionary: ~24 байта на элемент + overhead для хеш-таблицы
var dict = new Dictionary<int, int>(1000000);
```

---

## 9. Структура данных и логика поиска

### Как работает Dictionary внутри?

Dictionary в C# реализован как **хеш-таблица** (hash table). Это позволяет достичь O(1) сложности для большинства операций.

### Хеш-таблица: Принцип работы

#### Шаг 1: Хеширование ключа

```csharp
// Когда вы добавляете элемент:
dict["John"] = 30;

// 1. Ключ "John" преобразуется в хеш-код
int hashCode = "John".GetHashCode(); // Например: 123456789

// 2. Хеш-код используется для вычисления индекса в массиве
int index = Math.Abs(hashCode) % bucketCount; // Например: 5
```

#### Шаг 2: Хранение в бакете

```csharp
// 3. Элемент сохраняется в "бакете" (bucket) по вычисленному индексу
buckets[index] = new Entry { Key = "John", Value = 30, HashCode = hashCode };
```

#### Шаг 3: Поиск элемента

```csharp
// При поиске:
int age = dict["John"];

// 1. Вычисляется хеш-код ключа
int hashCode = "John".GetHashCode();

// 2. Вычисляется индекс бакета
int index = Math.Abs(hashCode) % bucketCount;

// 3. Проверяется бакет по индексу
// 4. Если найдено - возвращается значение (O(1))
```

### Диаграмма: Внутренняя структура Dictionary

```
┌─────────────────────────────────────────────────────────┐
│  Dictionary<string, int>                                │
│                                                          │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐             │
│  │ Bucket 0 │  │ Bucket 1 │  │ Bucket 2 │  ...        │
│  │          │  │          │  │          │             │
│  │  null    │  │ Entry    │  │ Entry    │             │
│  │          │  │ "Jane"→25│  │ "Bob"→28 │             │
│  └──────────┘  └──────────┘  └──────────┘             │
│                                                          │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐             │
│  │ Bucket 3 │  │ Bucket 4 │  │ Bucket 5 │  ...        │
│  │          │  │          │  │          │             │
│  │ Entry    │  │  null    │  │ Entry    │             │
│  │ "John"→30│  │          │  │ "Alice"→22│            │
│  └──────────┘  └──────────┘  └──────────┘             │
│                                                          │
│  Хеш-функция: "John".GetHashCode() % bucketCount = 3   │
└─────────────────────────────────────────────────────────┘
```

### Процесс поиска: Пошагово

```csharp
// Пример: Поиск значения по ключу "John"

// 1. Вычисление хеш-кода
string key = "John";
int hashCode = key.GetHashCode(); // Например: 123456789

// 2. Вычисление индекса бакета
int bucketIndex = Math.Abs(hashCode) % buckets.Length; // Например: 3

// 3. Проверка бакета
Entry entry = buckets[bucketIndex];

// 4. Сравнение ключей (на случай коллизий)
while (entry != null) {
    if (entry.HashCode == hashCode && entry.Key.Equals(key)) {
        return entry.Value; // Найдено! O(1) в среднем
    }
    entry = entry.Next; // Переход к следующему в цепочке
}

// 5. Если не найдено - KeyNotFoundException
```

### Почему Dictionary быстрее List?

#### List: Линейный поиск O(n)

```csharp
// List хранит элементы последовательно
var list = new List<Person> {
    new Person { Id = 1, Name = "John" },
    new Person { Id = 2, Name = "Jane" },
    // ... 1000 элементов
    new Person { Id = 1000, Name = "Bob" }
};

// Поиск по Id = 1000
// Нужно проверить ВСЕ элементы до последнего
var person = list.FirstOrDefault(p => p.Id == 1000);
// Проверено: 1, 2, 3, ..., 999, 1000 = 1000 операций сравнения
```

**Диаграмма поиска в List:**
```
List: [1] [2] [3] ... [999] [1000]
       ↓   ↓   ↓        ↓     ↓
      Проверка каждого элемента последовательно
      В худшем случае: n операций
```

#### Dictionary: Хеш-поиск O(1)

```csharp
// Dictionary использует хеш-таблицу
var dict = new Dictionary<int, Person> {
    { 1, new Person { Id = 1, Name = "John" } },
    { 2, new Person { Id = 2, Name = "Jane" } },
    // ... 1000 элементов
    { 1000, new Person { Id = 1000, Name = "Bob" } }
};

// Поиск по Id = 1000
// 1. Вычисляется хеш: GetHashCode(1000) = 1000
// 2. Вычисляется индекс: 1000 % bucketCount = индекс
// 3. Прямой доступ к бакету = 1 операция!
var person = dict[1000];
// Всего: 1 операция (в среднем)
```

**Диаграмма поиска в Dictionary:**
```
Dictionary:
  Hash(1000) → Index → Bucket → Value
       ↓         ↓        ↓        ↓
     O(1)      O(1)     O(1)     O(1)
     Всего: O(1) операция!
```

### Сравнение производительности

```csharp
// Тест производительности
var list = new List<Person>();
var dict = new Dictionary<int, Person>();

// Заполнение (100,000 элементов)
for (int i = 0; i < 100000; i++) {
    var person = new Person { Id = i, Name = $"Person {i}" };
    list.Add(person);
    dict[i] = person;
}

// Поиск в List: O(n) - в среднем n/2 операций
var stopwatch = Stopwatch.StartNew();
var found = list.FirstOrDefault(p => p.Id == 50000);
stopwatch.Stop();
Console.WriteLine($"List поиск: {stopwatch.ElapsedMilliseconds} мс");
// Результат: ~5-10 мс (50,000 сравнений в среднем)

// Поиск в Dictionary: O(1) - 1 операция
stopwatch.Restart();
var found2 = dict[50000];
stopwatch.Stop();
Console.WriteLine($"Dictionary поиск: {stopwatch.ElapsedMilliseconds} мс");
// Результат: ~0.001 мс (1 операция)
```

---

## 10. Коллизии и дубликаты

### Что такое коллизия хешей?

**Коллизия** возникает, когда два разных ключа имеют одинаковый хеш-код. Dictionary обрабатывает коллизии с помощью метода **цепочки** (chaining).

### Как возникают коллизии?

```csharp
// Пример коллизии
string key1 = "John";
string key2 = "Jane";

int hash1 = key1.GetHashCode(); // Например: 123456789
int hash2 = key2.GetHashCode(); // Например: 987654321

// После вычисления индекса бакета:
int index1 = Math.Abs(hash1) % bucketCount; // Например: 5
int index2 = Math.Abs(hash2) % bucketCount; // Например: 5 (коллизия!)

// Оба ключа попадают в один бакет
```

### Диаграмма: Обработка коллизий

```
┌─────────────────────────────────────────────────────────┐
│  Bucket 5 (коллизия)                                    │
│                                                          │
│  ┌──────────┐                                           │
│  │ Entry 1  │ → Key: "John", Value: 30                 │
│  │ Hash: 123│                                           │
│  └────┬─────┘                                           │
│       │                                                  │
│       ▼                                                  │
│  ┌──────────┐                                           │
│  │ Entry 2  │ → Key: "Jane", Value: 25                 │
│  │ Hash: 456│                                           │
│  └────┬─────┘                                           │
│       │                                                  │
│       ▼                                                  │
│  ┌──────────┐                                           │
│  │ Entry 3  │ → Key: "Bob", Value: 28                  │
│  │ Hash: 789│                                           │
│  └──────────┘                                           │
│                                                          │
│  Цепочка (Chaining) для обработки коллизий             │
└─────────────────────────────────────────────────────────┘
```

### Как Dictionary обрабатывает коллизии?

```csharp
// Dictionary использует метод цепочки (chaining)
// Когда два ключа попадают в один бакет, они хранятся в связанном списке

// При поиске:
// 1. Вычисляется индекс бакета
// 2. Проверяется первый элемент в цепочке
// 3. Если хеш-коды совпадают, сравниваются ключи через Equals()
// 4. Если не совпадает - переход к следующему элементу в цепочке

// В худшем случае (все ключи в одном бакете): O(n)
// В среднем случае: O(1)
```

### Как избежать коллизий?

#### 1. Хорошая хеш-функция

```csharp
// ✅ ХОРОШО: Правильная реализация GetHashCode()
public class PersonKey {
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public override int GetHashCode() {
        // Используйте HashCode.Combine для комбинирования полей
        return HashCode.Combine(FirstName, LastName);
    }

    public override bool Equals(object obj) {
        if (obj is PersonKey other) {
            return FirstName == other.FirstName && LastName == other.LastName;
        }
        return false;
    }
}
```

#### 2. Равномерное распределение

```csharp
// ✅ ХОРОШО: Хеш-коды должны распределяться равномерно
// Плохая хеш-функция:
public override int GetHashCode() {
    return FirstName.Length; // ❌ Много коллизий для одинаковых длин
}

// Хорошая хеш-функция:
public override int GetHashCode() {
    return HashCode.Combine(FirstName, LastName); // ✅ Равномерное распределение
}
```

#### 3. Указание начальной ёмкости

```csharp
// ✅ ХОРОШО: Укажите начальную ёмкость, если знаете размер
// Это уменьшает количество рехеширований и коллизий
var dict = new Dictionary<string, int>(1000); // Начальная ёмкость

// Dictionary автоматически увеличивает размер при необходимости
// Но лучше указать заранее, чтобы избежать рехеширования
```

### Дубликаты ключей

#### Dictionary предотвращает дубликаты автоматически

```csharp
var dict = new Dictionary<string, int>();

// ✅ Добавление нового ключа
dict.Add("John", 30);

// ❌ Попытка добавить дубликат - исключение
try {
    dict.Add("John", 31); // ArgumentException: "An item with the same key has already been added."
} catch (ArgumentException ex) {
    Console.WriteLine(ex.Message);
}

// ✅ Обновление существующего ключа через индексатор
dict["John"] = 31; // Обновит значение, не выбросит исключение
```

#### Проверка перед добавлением

```csharp
var dict = new Dictionary<string, int>();

// Способ 1: Проверка перед добавлением
if (!dict.ContainsKey("John")) {
    dict.Add("John", 30);
} else {
    dict["John"] = 31; // Обновление
}

// Способ 2: Использование индексатора (автоматически обновит)
dict["John"] = 30; // Добавит или обновит

// Способ 3: TryAdd (C# 8.0+)
if (dict.TryAdd("John", 30)) {
    Console.WriteLine("Добавлено");
} else {
    Console.WriteLine("Ключ уже существует");
    dict["John"] = 31; // Обновление
}
```

### Best Practices для избежания коллизий

#### ✅ Что делать

1. **Используйте HashCode.Combine() для составных ключей**
   ```csharp
   public override int GetHashCode() {
       return HashCode.Combine(Field1, Field2, Field3);
   }
   ```

2. **Указывайте начальную ёмкость**
   ```csharp
   var dict = new Dictionary<string, int>(expectedSize);
   ```

3. **Используйте простые типы в качестве ключей, когда возможно**
   ```csharp
   // ✅ ХОРОШО: int, string - хорошие ключи
   var dict = new Dictionary<int, Person>();
   
   // ⚠️ ВНИМАНИЕ: Сложные объекты требуют правильной реализации GetHashCode()
   var dict2 = new Dictionary<PersonKey, int>();
   ```

#### ❌ Чего избегать

1. **Не используйте изменяемые объекты в качестве ключей**
   ```csharp
   // ❌ ПЛОХО: Изменение ключа после добавления
   var dict = new Dictionary<Person, int>();
   var person = new Person { Id = 1, Name = "John" };
   dict[person] = 30;
   
   person.Name = "Jane"; // Изменение ключа!
   // Теперь поиск по person может не работать корректно
   ```

2. **Не возвращайте константу из GetHashCode()**
   ```csharp
   // ❌ ПЛОХО: Все объекты будут иметь одинаковый хеш
   public override int GetHashCode() {
       return 42; // Всегда одно и то же!
   }
   ```

---

## 11. Сравнение с другими коллекциями

### Почему Dictionary лучше, чем List или Array?

#### Сравнение производительности поиска

```csharp
// ❌ List/Array: Линейный поиск O(n)
var list = new List<Person> {
    new Person { Id = 1, Name = "John" },
    new Person { Id = 2, Name = "Jane" },
    // ... 1,000,000 элементов
    new Person { Id = 1000000, Name = "Bob" }
};

// Поиск элемента с Id = 1000000
// Нужно проверить ВСЕ элементы последовательно
var person = list.FirstOrDefault(p => p.Id == 1000000);
// В худшем случае: 1,000,000 операций сравнения
// В среднем случае: 500,000 операций сравнения

// ✅ Dictionary: Хеш-поиск O(1)
var dict = new Dictionary<int, Person>();
for (int i = 1; i <= 1000000; i++) {
    dict[i] = new Person { Id = i, Name = $"Person {i}" };
}

// Поиск элемента с Id = 1000000
var person2 = dict[1000000];
// Всего: 1 операция (в среднем)!
```

#### Практический пример: Поиск пользователя

```csharp
// Сценарий: Поиск пользователя по ID в базе из 100,000 пользователей

// ❌ ПЛОХО: Использование List
public class UserServiceList {
    private List<User> _users = new List<User>();
    
    public User FindUser(int userId) {
        // O(n) - нужно проверить все элементы
        return _users.FirstOrDefault(u => u.Id == userId);
        // В среднем: 50,000 операций сравнения
        // В худшем случае: 100,000 операций сравнения
    }
}

// ✅ ХОРОШО: Использование Dictionary
public class UserServiceDict {
    private Dictionary<int, User> _users = new Dictionary<int, User>();
    
    public User FindUser(int userId) {
        // O(1) - прямой доступ по ключу
        return _users.TryGetValue(userId, out User user) ? user : null;
        // Всего: 1 операция (в среднем)!
    }
}
```

#### Сравнительная таблица операций

| Операция | Array | List | Dictionary | Когда использовать |
|----------|-------|------|------------|-------------------|
| **Доступ по индексу** | O(1) | O(1) | O(1) | Все одинаковые |
| **Поиск по значению** | O(n) | O(n) | O(n) | Все одинаковые |
| **Поиск по ключу** | N/A | O(n) | **O(1)** | **Dictionary** |
| **Добавление в конец** | N/A | O(1) | O(1) | List/Dictionary |
| **Вставка в начало** | N/A | O(n) | O(1) | **Dictionary** |
| **Удаление** | N/A | O(n) | **O(1)** | **Dictionary** |
| **Проверка существования** | O(n) | O(n) | **O(1)** | **Dictionary** |

#### Когда использовать Dictionary вместо List?

```csharp
// ✅ ИСПОЛЬЗУЙТЕ Dictionary, когда:
// 1. Нужен частый поиск по уникальному ключу
var userLookup = new Dictionary<int, User>(); // Поиск по ID

// 2. Нужна быстрая проверка существования
var allowedUsers = new Dictionary<int, bool>(); // Проверка разрешений

// 3. Нужна быстрая вставка/удаление
var cache = new Dictionary<string, object>(); // Кэш данных

// 4. Нужна группировка данных
var usersByCity = new Dictionary<string, List<User>>(); // Группировка

// ❌ ИСПОЛЬЗУЙТЕ List, когда:
// 1. Нужен доступ по индексу (порядок важен)
var sortedList = new List<User>(); // Сортированный список

// 2. Нужна итерация по всем элементам
foreach (var user in users) { } // Проход по всем

// 3. Нужен небольшой набор данных (< 100 элементов)
var smallList = new List<int>(); // Маленький список
```

#### Реальный пример: Система кэширования

```csharp
// ❌ ПЛОХО: Кэш на основе List
public class CacheList<T> {
    private List<CacheItem<T>> _items = new List<CacheItem<T>>();
    
    public T Get(string key) {
        // O(n) - медленно для больших кэшей
        var item = _items.FirstOrDefault(i => i.Key == key);
        return item != null ? item.Value : default(T);
    }
    
    public void Set(string key, T value) {
        var item = _items.FirstOrDefault(i => i.Key == key);
        if (item != null) {
            item.Value = value; // O(n) для поиска
        } else {
            _items.Add(new CacheItem<T> { Key = key, Value = value });
        }
    }
}

// ✅ ХОРОШО: Кэш на основе Dictionary
public class CacheDict<T> {
    private Dictionary<string, T> _items = new Dictionary<string, T>();
    
    public T Get(string key) {
        // O(1) - быстро всегда!
        return _items.TryGetValue(key, out T value) ? value : default(T);
    }
    
    public void Set(string key, T value) {
        // O(1) - быстро всегда!
        _items[key] = value;
    }
}
```

#### Измерение производительности

```csharp
using System.Diagnostics;

// Тест: Поиск в коллекции из 1,000,000 элементов

// List
var list = new List<Person>();
for (int i = 0; i < 1000000; i++) {
    list.Add(new Person { Id = i, Name = $"Person {i}" });
}

var sw = Stopwatch.StartNew();
var found = list.FirstOrDefault(p => p.Id == 999999);
sw.Stop();
Console.WriteLine($"List поиск: {sw.ElapsedMilliseconds} мс");
// Результат: ~50-100 мс (500,000 сравнений в среднем)

// Dictionary
var dict = new Dictionary<int, Person>();
for (int i = 0; i < 1000000; i++) {
    dict[i] = new Person { Id = i, Name = $"Person {i}" };
}

sw.Restart();
var found2 = dict[999999];
sw.Stop();
Console.WriteLine($"Dictionary поиск: {sw.ElapsedMilliseconds} мс");
// Результат: ~0.001 мс (1 операция)

// Dictionary в 50,000-100,000 раз быстрее!
```

### Dictionary vs List

```csharp
// List - поиск O(n)
var list = new List<Person>();
var person = list.FirstOrDefault(p => p.Id == 123); // Медленно для больших списков

// Dictionary - поиск O(1)
var dict = new Dictionary<int, Person>();
var person = dict[123]; // Быстро всегда
```

### Dictionary vs Hashtable

```csharp
// ❌ Hashtable (legacy, не generic)
Hashtable hashtable = new Hashtable();
hashtable["key"] = "value";
string value = (string)hashtable["key"]; // Требуется приведение типа

// ✅ Dictionary (generic, type-safe)
Dictionary<string, string> dict = new Dictionary<string, string>();
dict["key"] = "value";
string value = dict["key"]; // Type-safe, без приведения
```

---

## 12. Пользовательские ключи

### Ключи-объекты

```csharp
// Для использования объектов в качестве ключей, они должны:
// 1. Реализовывать GetHashCode() и Equals()
// 2. Или использовать IEqualityComparer<T>

public class PersonKey {
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public override bool Equals(object obj) {
        if (obj is PersonKey other) {
            return FirstName == other.FirstName && LastName == other.LastName;
        }
        return false;
    }

    public override int GetHashCode() {
        return HashCode.Combine(FirstName, LastName);
    }
}

var dict = new Dictionary<PersonKey, int> {
    { new PersonKey { FirstName = "John", LastName = "Doe" }, 30 }
};
```

### Пользовательский IEqualityComparer

```csharp
public class CaseInsensitiveComparer : IEqualityComparer<string> {
    public bool Equals(string x, string y) {
        return string.Equals(x, y, StringComparison.OrdinalIgnoreCase);
    }

    public int GetHashCode(string obj) {
        return obj?.ToLower().GetHashCode() ?? 0;
    }
}

// Использование
var dict = new Dictionary<string, int>(new CaseInsensitiveComparer());
dict["John"] = 30;
int age = dict["JOHN"]; // Найдёт "John" (без учёта регистра)
```

---

## 13. Best Practices

### ✅ Что делать

1. **Используйте TryGetValue вместо ContainsKey + индексатор**
   ```csharp
   // ✅ ХОРОШО
   if (dict.TryGetValue(key, out int value)) {
       // Используем value
   }
   ```

2. **Указывайте начальную ёмкость, если знаете размер**
   ```csharp
   // ✅ ХОРОШО
   var dict = new Dictionary<string, int>(expectedSize);
   ```

3. **Используйте Dictionary для быстрого поиска по ключу**
   ```csharp
   // ✅ ХОРОШО - O(1) поиск
   var lookup = new Dictionary<int, Person>();
   var person = lookup[123];
   ```

### ❌ Чего избегать

1. **Не используйте ContainsValue для частых проверок**
   ```csharp
   // ❌ ПЛОХО - O(n) операция
   if (dict.ContainsValue(30)) {
       // Медленно для больших словарей
   }
   ```

2. **Не используйте Dictionary, если нужен порядок элементов**
   ```csharp
   // ❌ ПЛОХО - Dictionary не гарантирует порядок
   // Используйте OrderedDictionary или SortedDictionary
   ```

3. **Не используйте индексатор без проверки**
   ```csharp
   // ❌ ПЛОХО - выбросит исключение, если ключ не найден
   int value = dict["nonexistent"];
   
   // ✅ ХОРОШО
   if (dict.TryGetValue("nonexistent", out int value)) {
       // Используем value
   }
   ```

---

## 14. Часто задаваемые вопросы (FAQ)

### Q: В чём разница между Dictionary и Hashtable?
**A:** Dictionary является generic и type-safe, Hashtable — legacy класс, не generic. Всегда используйте Dictionary.

### Q: Сохраняет ли Dictionary порядок элементов?
**A:** В .NET Core/.NET 5+ Dictionary сохраняет порядок вставки. В .NET Framework порядок не гарантируется.

### Q: Что произойдёт, если добавить дубликат ключа?
**A:** Метод `Add()` выбросит `ArgumentException`. Индексатор обновит значение.

### Q: Как получить значение по умолчанию, если ключ не найден?
**A:** Используйте `GetValueOrDefault(key, defaultValue)` или `TryGetValue()`.

### Q: Можно ли использовать null в качестве ключа?
**A:** Нет, ключ не может быть null. Используйте `Dictionary<TKey, TValue>` где `TKey` не может быть null, или обработайте null отдельно.

---

## 15. Продвинутые техники

### Словарь словарей

```csharp
// Вложенные словари
var nestedDict = new Dictionary<string, Dictionary<string, int>>();

nestedDict["Moscow"] = new Dictionary<string, int> {
    { "John", 30 },
    { "Jane", 25 }
};

nestedDict["SPB"] = new Dictionary<string, int> {
    { "Bob", 28 }
};

// Доступ
int age = nestedDict["Moscow"]["John"]; // 30
```

### ConcurrentDictionary (потокобезопасный)

```csharp
using System.Collections.Concurrent;

// Потокобезопасный словарь
var concurrentDict = new ConcurrentDictionary<string, int>();

// Безопасные операции из разных потоков
concurrentDict.TryAdd("John", 30);
concurrentDict.TryUpdate("John", 31, 30); // Обновить, если текущее значение = 30
concurrentDict.AddOrUpdate("Jane", 25, (key, oldValue) => oldValue + 1);
```

---

## Заключение

Dictionary<TKey, TValue> — это мощная коллекция для:

- ✅ Быстрого поиска по ключу (O(1))
- ✅ Хранения пар ключ-значение
- ✅ Эффективного добавления и удаления
- ✅ Реализации кэшей и lookup-таблиц

Используйте Dictionary, когда нужен быстрый поиск по уникальному ключу!

---

*Документ создан для объяснения Dictionary в C# с практическими примерами и best practices.*

