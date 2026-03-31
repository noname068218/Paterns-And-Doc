# Коллекции и структуры данных в C#

## Введение

**Коллекции** в C# — это структуры данных, позволяющие хранить и управлять группами объектов. Понимание различных коллекций и того, когда их использовать, необходимо для написания эффективного кода.

---

## 1. Иерархия коллекций

### Базовые интерфейсы

```
┌─────────────────────────────────────────────┐
│         IEnumerable<T>                      │
│         (iteration)                         │
└─────────────────────────────────────────────┘
                    ▲
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│ICollection│ │IReadOnly  │ │IQueryable │
│<T>        │ │Collection │ │<T>        │
│           │ │<T>        │ │           │
│Add/Remove │ │(read only)│ │(LINQ)     │
└───────────┘ └───────────┘ └───────────┘
        ▲
        │
┌───────┼───────┐
│       │       │
▼       ▼       ▼
┌───────┐ ┌───────┐ ┌───────┐
│IList  │ │ISet   │ │IDict  │
│<T>    │ │<T>    │ │<TKey, │
│       │ │       │ │TValue>│
│Index  │ │Unique │ │Key-   │
│access │ │values │ │Value  │
└───────┘ └───────┘ └───────┘
```

---

## 2. Big-O нотация: Понимание сложности алгоритмов

**Big-O нотация** описывает, как время выполнения или использование памяти алгоритма растёт с увеличением размера входных данных. Это критически важно для выбора правильной структуры данных.

### Основные типы сложности

#### O(1) - Константное время ⚡

**Описание:** Время выполнения не зависит от размера входных данных.

**Примеры:**
- Доступ к элементу массива по индексу: `arr[5]`
- Доступ к значению в Dictionary по ключу: `dict["key"]`
- Добавление элемента в конец List (амортизированно)

**Визуализация:**
```
Время
  │
  │────────────────────────────
  │
  └──────────────────────────── Размер данных
```

#### O(n) - Линейное время 📈

**Описание:** Время выполнения растёт пропорционально размеру входных данных.

**Примеры:**
- Поиск элемента в неотсортированном массиве/List
- Перебор всех элементов коллекции
- Удаление элемента из List по значению

**Визуализация:**
```
Время
  │     ╱
  │    ╱
  │   ╱
  │  ╱
  └─╱─────────────── Размер данных
```

#### O(log n) - Логарифмическое время 📊

**Описание:** Время выполнения растёт логарифмически с увеличением размера данных. Очень эффективно!

**Примеры:**
- Бинарный поиск в отсортированном массиве
- Поиск в сбалансированном бинарном дереве
- Поиск в SortedSet<T>

**Визуализация:**
```
Время
  │      ╱
  │     ╱
  │    ╱
  │   ╱
  └──╱─────────────── Размер данных
```

#### O(n²) - Квадратичное время 🐌

**Описание:** Время выполнения растёт пропорционально квадрату размера входных данных. Медленно для больших данных!

**Примеры:**
- Вложенные циклы (для каждого элемента проверяем все остальные)
- Пузырьковая сортировка (наивная версия)
- Проверка всех пар элементов в массиве

**Визуализация:**
```
Время
  │       ╱
  │      ╱
  │     ╱
  │    ╱
  │   ╱
  └──╱─────────────── Размер данных
```

### Сравнительная таблица сложности

| Сложность | Название           | Пример для 10 элементов | Пример для 1000 элементов | Когда использовать             |
| --------- | ------------------ | ----------------------- | ------------------------- | ------------------------------ |
| **O(1)**  | Константное        | 1 операция              | 1 операция                | Доступ по индексу/ключу        |
| **O(log n)** | Логарифмическое | 3-4 операции            | 10 операций               | Бинарный поиск, деревья        |
| **O(n)**  | Линейное           | 10 операций             | 1000 операций             | Перебор, поиск в списке        |
| **O(n log n)** | Линейно-логарифмическое | ~33 операции      | ~10,000 операций          | Эффективные сортировки         |
| **O(n²)** | Квадратичное       | 100 операций            | 1,000,000 операций        | Избегать! (вложенные циклы)    |

### Практические примеры

```csharp
// O(1) - Константное время
var dict = new Dictionary<string, int>();
dict["key"] = 42;  // O(1)
var value = dict["key"];  // O(1)

// O(n) - Линейное время
var list = new List<int> { 1, 2, 3, 4, 5 };
bool found = list.Contains(3);  // O(n) - нужно проверить все элементы

// O(log n) - Логарифмическое время
var sortedSet = new SortedSet<int> { 1, 2, 3, 4, 5 };
bool exists = sortedSet.Contains(3);  // O(log n) - бинарный поиск

// O(n²) - Квадратичное время
var arr = new[] { 1, 2, 3, 4, 5 };
for (int i = 0; i < arr.Length; i++) {
    for (int j = 0; j < arr.Length; j++) {
        // Обработка каждой пары - O(n²)
    }
}
```

### Почему это важно?

Понимание Big-O помогает:

- ✅ **Выбрать правильную структуру данных** для конкретной задачи
- ✅ **Оптимизировать производительность** кода
- ✅ **Предсказать поведение** алгоритма на больших данных
- ✅ **Избежать узких мест** (bottlenecks) в приложении

---

## 3. List<T> - Динамический список

### Характеристики

✅ Доступ по индексу  
✅ Динамическое добавление/удаление  
✅ Дубликаты разрешены  
✅ Поддерживается сортировка

### Базовое использование

```csharp
// Creation
var list = new List<int> { 1, 2, 3, 4, 5 };

// Add elements
list.Add(6);
list.AddRange(new[] { 7, 8, 9 });

// Access by index
int first = list[0]; // 1
int last = list[list.Count - 1]; // 9

// Remove
list.Remove(5);
list.RemoveAt(0); // Remove first element

// Search
bool contains = list.Contains(3); // true
int index = list.IndexOf(3); // 2

// Sorting
list.Sort(); // Sort in-place
var sorted = list.OrderBy(x => x).ToList(); // LINQ (new list)
```

### Распространённые операции

```csharp
var numbers = new List<int> { 3, 1, 4, 1, 5, 9, 2, 6 };

// Filtering
var evens = numbers.Where(x => x % 2 == 0).ToList();

// Transform
var doubled = numbers.Select(x => x * 2).ToList();

// Aggregation
int sum = numbers.Sum();
int max = numbers.Max();
double avg = numbers.Average();

// Conditions
bool allPositive = numbers.All(x => x > 0);
bool anyEven = numbers.Any(x => x % 2 == 0);
```

### Производительность

| Операция         | Сложность       |
| ------------------ | ---------------- |
| Доступ по индексу | O(1)             |
| Поиск элемента   | O(n)             |
| Вставка в конец   | O(1) амортизированно |
| Вставка в начало | O(n)             |
| Удаление          | O(n)             |

---

## 4. Dictionary<TKey, TValue> - Словарь ключ-значение

### Характеристики

✅ Быстрый доступ по ключу  
✅ Уникальные ключи  
✅ Пары ключ-значение  
✅ Поиск O(1) в среднем

### Базовое использование

```csharp
// Creation
var dict = new Dictionary<string, int> {
    { "John", 30 },
    { "Jane", 25 },
    { "Bob", 28 }
};

// Or
var dict2 = new Dictionary<string, int>();
dict2["John"] = 30;
dict2["Jane"] = 25;

// Access
int johnAge = dict["John"]; // 30

// Check existence
if (dict.ContainsKey("John")) {
    int age = dict["John"];
}

// Or with TryGetValue (more efficient)
if (dict.TryGetValue("John", out int age)) {
    Console.WriteLine($"Age: {age}");
}

// Iteration
foreach (var kvp in dict) {
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
}

// Only keys or values
var keys = dict.Keys;
var values = dict.Values;
```

### Практический пример

```csharp
// Count occurrences
var words = new[] { "apple", "banana", "apple", "cherry", "banana", "apple" };
var wordCount = new Dictionary<string, int>();

foreach (var word in words) {
    if (wordCount.ContainsKey(word)) {
        wordCount[word]++;
    }
    else {
        wordCount[word] = 1;
    }
}

// Or more elegant
var wordCount2 = words
    .GroupBy(w => w)
    .ToDictionary(g => g.Key, g => g.Count());
```

### Производительность

| Операция         | Сложность |
| ------------------ | ----------- |
| Доступ по ключу | O(1) средняя  |
| Вставка        | O(1) средняя  |
| Удаление          | O(1) средняя  |
| Поиск ключа     | O(1) средняя  |

---

## 5. HashSet<T> - Уникальное множество

### Характеристики

✅ Уникальные элементы  
✅ Быстрый поиск O(1)  
✅ Операции над множествами  
✅ Порядок не гарантируется

### Базовое использование

```csharp
// Creation
var set = new HashSet<int> { 1, 2, 3, 4, 5 };

// Add (ignores duplicates)
set.Add(6);
set.Add(3); // Does not add (already present)

// Check existence
bool contains = set.Contains(3); // true

// Remove
set.Remove(3);

// Set operations
var set1 = new HashSet<int> { 1, 2, 3, 4 };
var set2 = new HashSet<int> { 3, 4, 5, 6 };

// Union
var union = new HashSet<int>(set1);
union.UnionWith(set2); // { 1, 2, 3, 4, 5, 6 }

// Intersection
var intersect = new HashSet<int>(set1);
intersect.IntersectWith(set2); // { 3, 4 }

// Difference
var except = new HashSet<int>(set1);
except.ExceptWith(set2); // { 1, 2 }

// Subset
bool isSubset = set1.IsSubsetOf(set2); // false
```

### Когда использовать HashSet

```csharp
// ✅ Perfect for: Fast uniqueness check
var uniqueEmails = new HashSet<string>();
foreach (var user in users) {
    uniqueEmails.Add(user.Email); // Automatically ignores duplicates
}

// ✅ Perfect for: Fast lookup
var allowedUsers = new HashSet<int> { 1, 2, 3, 5, 8 };
if (allowedUsers.Contains(userId)) {
    // Access granted
}
```

---

## 6. Queue<T> - Очередь FIFO

### Характеристики

✅ First In, First Out (FIFO)  
✅ Enqueue (добавить) / Dequeue (удалить)  
✅ Упорядоченная обработка

### Базовое использование

```csharp
// Creation
var queue = new Queue<string>();

// Add (to tail)
queue.Enqueue("First");
queue.Enqueue("Second");
queue.Enqueue("Third");

// Remove (from head)
string first = queue.Dequeue(); // "First"

// Peek without removing
string next = queue.Peek(); // "Second"

// Check
bool isEmpty = queue.Count == 0;

// Iteration (does not remove)
foreach (var item in queue) {
    Console.WriteLine(item);
}
```

### Диаграмма: Queue

```
┌─────────────────────────────────────────────┐
│  Queue (FIFO)                               │
│                                             │
│  Enqueue ──► [A] [B] [C] ──► Dequeue       │
│              ↑              ↑               │
│            Back          Front              │
└─────────────────────────────────────────────┘
```

### Пример: Обработка заказов

```csharp
var orderQueue = new Queue<Order>();

// Add orders
orderQueue.Enqueue(new Order { Id = 1 });
orderQueue.Enqueue(new Order { Id = 2 });
orderQueue.Enqueue(new Order { Id = 3 });

// Process in order
while (orderQueue.Count > 0) {
    var order = orderQueue.Dequeue();
    ProcessOrder(order);
}
```

---

## 7. Stack<T> - Стек LIFO

### Характеристики

✅ Last In, First Out (LIFO)  
✅ Push (добавить) / Pop (удалить)  
✅ Полезен для рекурсивных алгоритмов

### Базовое использование

```csharp
// Creation
var stack = new Stack<int>();

// Add (to top)
stack.Push(1);
stack.Push(2);
stack.Push(3);

// Remove (from top)
int top = stack.Pop(); // 3

// Peek without removing
int next = stack.Peek(); // 2

// Check
bool isEmpty = stack.Count == 0;
```

### Диаграмма: Stack

```
┌─────────────────────────────────────────────┐
│  Stack (LIFO)                               │
│                                             │
│  Push ──► [C] ──► Pop                       │
│            │                                │
│           [B]                               │
│            │                                │
│           [A]                               │
│            │                                │
│          Top                                │
└─────────────────────────────────────────────┘
```

### Пример: Валидация скобок

```csharp
public bool IsValidParentheses(string s) {
    var stack = new Stack<char>();

    foreach (char c in s) {
        if (c == '(' || c == '[' || c == '{') {
            stack.Push(c);
        }
        else if (c == ')' || c == ']' || c == '}') {
            if (stack.Count == 0) return false;

            char top = stack.Pop();
            if ((c == ')' && top != '(') ||
                (c == ']' && top != '[') ||
                (c == '}' && top != '{')) {
                return false;
            }
        }
    }

    return stack.Count == 0;
}
```

---

## 8. LinkedList<T> - Связанный список

### Характеристики

✅ Вставка/удаление O(1)  
✅ Последовательный доступ  
✅ Полезен для частых вставок

### Базовое использование

```csharp
// Creation
var linkedList = new LinkedList<int>();

// Add
linkedList.AddLast(1);
linkedList.AddLast(2);
linkedList.AddFirst(0); // Add to beginning

// Access
var first = linkedList.First; // LinkedListNode<int>
var last = linkedList.Last;

// Insert after node
var node = linkedList.Find(1);
if (node != null) {
    linkedList.AddAfter(node, 1.5);
}

// Remove
linkedList.Remove(1);
linkedList.RemoveFirst();
linkedList.RemoveLast();
```

---

## 9. Детальные сравнения коллекций

### 9.1. Array vs List<T> vs LinkedList<T>

#### Array (Массив)

**Характеристики:**
- Фиксированный размер (нельзя изменить после создания)
- Хранится в непрерывной области памяти
- Самый быстрый доступ по индексу
- Минимальный overhead (накладные расходы)

**Когда использовать:**
- ✅ Размер известен и не изменится
- ✅ Нужна максимальная производительность
- ✅ Много операций доступа по индексу
- ✅ Работа с байтами, числовыми данными

```csharp
// Array - фиксированный размер
int[] numbers = new int[10];  // Размер задаётся при создании
numbers[0] = 1;
numbers[1] = 2;

// Нельзя изменить размер!
// numbers.Add(3);  // ❌ Ошибка - такого метода нет
```

**Производительность Array:**
- Доступ по индексу: **O(1)** - самый быстрый
- Поиск элемента: **O(n)**
- Вставка: Невозможна (нужно создавать новый массив)
- Удаление: Невозможно

#### List<T>

**Характеристики:**
- Динамический размер (автоматически расширяется)
- Хранится в массиве (внутренне использует Array)
- Доступ по индексу O(1)
- Вставка в конец O(1) амортизированно

**Когда использовать:**
- ✅ Размер может изменяться
- ✅ Нужен доступ по индексу
- ✅ Частое добавление в конец
- ✅ Общая коллекция (используйте по умолчанию)

```csharp
// List - динамический размер
var list = new List<int>();  // Начальная ёмкость = 0
list.Add(1);  // Автоматически расширяется
list.Add(2);
list[0] = 10;  // Доступ по индексу
```

**Производительность List<T>:**
- Доступ по индексу: **O(1)**
- Поиск элемента: **O(n)**
- Вставка в конец: **O(1)** амортизированно
- Вставка в начало/середину: **O(n)**
- Удаление: **O(n)**

#### LinkedList<T>

**Характеристики:**
- Динамический размер
- Элементы связаны указателями (не в непрерывной памяти)
- Доступ по индексу O(n) - нужно переходить по ссылкам
- Вставка/удаление O(1) если есть узел

**Когда использовать:**
- ✅ Частые вставки/удаления в середине
- ✅ Частые вставки в начало
- ❌ НЕ нужен доступ по индексу
- ❌ НЕ нужен быстрый поиск

```csharp
// LinkedList - связанный список
var linkedList = new LinkedList<int>();
linkedList.AddLast(1);
linkedList.AddLast(2);
linkedList.AddFirst(0);  // O(1) - быстро!

// Доступ по индексу медленный
// var value = linkedList[1];  // ❌ Нет индексатора
```

**Производительность LinkedList<T>:**
- Доступ по индексу: **O(n)** - медленно!
- Поиск элемента: **O(n)**
- Вставка в начало/конец: **O(1)**
- Вставка в середину (если есть узел): **O(1)**
- Удаление (если есть узел): **O(1)**

#### Сравнительная таблица: Array vs List vs LinkedList

| Критерий              | Array           | List<T>          | LinkedList<T>    |
| --------------------- | --------------- | ---------------- | ---------------- |
| **Размер**            | Фиксированный   | Динамический     | Динамический     |
| **Память**            | Непрерывная     | Непрерывная      | Связанная        |
| **Доступ по индексу** | O(1) ⚡         | O(1) ⚡          | O(n) 🐌          |
| **Поиск**             | O(n)            | O(n)             | O(n)             |
| **Вставка в начало**  | Невозможна      | O(n)             | O(1) ⚡          |
| **Вставка в конец**   | Невозможна      | O(1)* ⚡         | O(1) ⚡          |
| **Вставка в середину** | Невозможна     | O(n)             | O(1)** ⚡        |
| **Удаление**          | Невозможно      | O(n)             | O(1)** ⚡        |
| **Overhead**          | Минимальный     | Средний          | Высокий          |

\* O(1) амортизированно  
\** Если есть узел (node), иначе O(n) для поиска узла

#### Практические рекомендации

```csharp
// ✅ Используйте Array когда:
int[] fixedScores = new int[100];  // Известен размер
byte[] imageBytes = new byte[1024];  // Работа с байтами

// ✅ Используйте List<T> когда:
var items = new List<string>();  // Размер может изменяться
items.Add("item1");  // Добавление в конец
var item = items[0];  // Доступ по индексу

// ✅ Используйте LinkedList<T> когда:
var undoStack = new LinkedList<Action>();  // Частые вставки в начало
undoStack.AddFirst(UndoAction);  // O(1) - быстро!
```

---

### 9.2. Dictionary<TKey, TValue> vs HashSet<T>

#### Dictionary<TKey, TValue>

**Характеристики:**
- Хранит пары ключ-значение
- Уникальные ключи
- Быстрый доступ к значению по ключу O(1)
- Поддерживает итерацию по парам

**Когда использовать:**
- ✅ Нужно хранить и получать значения по ключу
- ✅ Логика типа "id -> объект", "email -> пользователь"
- ✅ Кэширование (ключ -> результат)
- ✅ Группировка данных (ключ -> список значений)

```csharp
// Dictionary - ключ -> значение
var users = new Dictionary<int, User>();
users[123] = new User { Id = 123, Name = "John" };
var user = users[123];  // O(1) - быстрый доступ

// Подсчёт вхождений
var wordCount = new Dictionary<string, int>();
wordCount["apple"] = 3;  // "apple" встречается 3 раза
```

**Производительность Dictionary:**
- Доступ по ключу: **O(1)** ⚡
- Добавление: **O(1)** ⚡
- Удаление: **O(1)** ⚡
- Поиск значения: **O(n)** 🐌 (нужно перебрать все значения)
- Проверка наличия ключа: **O(1)** ⚡

#### HashSet<T>

**Характеристики:**
- Хранит только уникальные значения (нет ключей)
- Быстрая проверка наличия O(1)
- Операции над множествами (Union, Intersect, Except)
- Порядок не гарантируется

**Когда использовать:**
- ✅ Нужно проверить, есть ли элемент в коллекции
- ✅ Нужны уникальные значения
- ✅ Операции над множествами (объединение, пересечение)
- ✅ Быстрая фильтрация дубликатов

```csharp
// HashSet - уникальные значения
var allowedIds = new HashSet<int> { 1, 2, 3, 5, 8 };
if (allowedIds.Contains(userId)) {  // O(1) - быстро!
    // Доступ разрешён
}

// Автоматическое удаление дубликатов
var uniqueEmails = new HashSet<string>();
foreach (var user in users) {
    uniqueEmails.Add(user.Email);  // Дубликаты игнорируются
}
```

**Производительность HashSet:**
- Проверка наличия (Contains): **O(1)** ⚡
- Добавление: **O(1)** ⚡
- Удаление: **O(1)** ⚡
- Операции множеств (Union, Intersect): **O(n)**

#### Сравнительная таблица: Dictionary vs HashSet

| Критерий                | Dictionary<TKey, TValue> | HashSet<T>        |
| ----------------------- | ------------------------ | ----------------- |
| **Хранение**            | Пары ключ-значение       | Только значения   |
| **Ключи**               | Уникальные               | Нет ключей        |
| **Значения**            | Могут дублироваться      | Уникальные        |
| **Доступ по ключу**     | O(1) ⚡                  | N/A               |
| **Проверка наличия**    | ContainsKey: O(1) ⚡     | Contains: O(1) ⚡  |
| **Поиск значения**      | O(1) ⚡                  | O(1) ⚡           |
| **Операции множеств**   | Нет                      | Да (Union, etc.)  |
| **Итерация**            | По парам KeyValuePair    | По значениям      |
| **Когда использовать**  | Ключ -> значение         | Уникальность      |

#### Практические примеры выбора

```csharp
// ✅ Dictionary - когда нужна связь ключ -> значение
var cache = new Dictionary<string, User>();
cache["user:123"] = user;  // Храним результат поиска
var cached = cache["user:123"];  // Быстро получаем

// ✅ HashSet - когда нужна только проверка наличия
var processedIds = new HashSet<int>();
if (!processedIds.Contains(orderId)) {  // Проверяем, не обработали ли уже
    ProcessOrder(orderId);
    processedIds.Add(orderId);
}

// ✅ Dictionary - подсчёт вхождений
var countByStatus = new Dictionary<string, int>();
foreach (var order in orders) {
    countByStatus[order.Status] = countByStatus.GetValueOrDefault(order.Status, 0) + 1;
}

// ✅ HashSet - удаление дубликатов
var uniqueWords = new HashSet<string>(words);  // Автоматически удаляет дубликаты

// ✅ HashSet - операции над множествами
var set1 = new HashSet<int> { 1, 2, 3, 4 };
var set2 = new HashSet<int> { 3, 4, 5, 6 };
set1.IntersectWith(set2);  // { 3, 4 } - пересечение
```

---

### 9.3. Queue<T> vs Stack<T>

#### Queue<T> (Очередь) - FIFO

**Характеристики:**
- First In, First Out (первым пришёл — первым ушёл)
- Добавление в конец (Enqueue), удаление из начала (Dequeue)
- Упорядоченная обработка элементов

**Когда использовать:**
- ✅ Обработка задач в порядке поступления
- ✅ Breadth-First Search (BFS) в графах
- ✅ Очереди сообщений/событий
- ✅ Планировщики задач

```csharp
// Queue - FIFO
var queue = new Queue<string>();
queue.Enqueue("First");   // Добавляем в конец
queue.Enqueue("Second");
queue.Enqueue("Third");

var first = queue.Dequeue();  // "First" - удаляем из начала
var second = queue.Dequeue(); // "Second"
```

**Производительность Queue:**
- Enqueue (добавление): **O(1)** ⚡
- Dequeue (удаление): **O(1)** ⚡
- Peek (просмотр): **O(1)** ⚡
- Поиск элемента: **O(n)** 🐌

#### Stack<T> (Стек) - LIFO

**Характеристики:**
- Last In, First Out (последним пришёл — первым ушёл)
- Добавление наверх (Push), удаление сверху (Pop)
- Полезен для отмены операций, рекурсии

**Когда использовать:**
- ✅ Undo/Redo операции
- ✅ Валидация скобок/выражений
- ✅ Depth-First Search (DFS) в графах
- ✅ Вызовы функций (call stack)

```csharp
// Stack - LIFO
var stack = new Stack<string>();
stack.Push("First");   // Добавляем наверх
stack.Push("Second");
stack.Push("Third");

var last = stack.Pop();   // "Third" - удаляем сверху
var next = stack.Pop();   // "Second"
```

**Производительность Stack:**
- Push (добавление): **O(1)** ⚡
- Pop (удаление): **O(1)** ⚡
- Peek (просмотр): **O(1)** ⚡
- Поиск элемента: **O(n)** 🐌

#### Сравнительная таблица: Queue vs Stack

| Критерий              | Queue<T> (FIFO)      | Stack<T> (LIFO)      |
| --------------------- | -------------------- | -------------------- |
| **Принцип**           | Первым пришёл — первым ушёл | Последним пришёл — первым ушёл |
| **Добавление**        | Enqueue (в конец)    | Push (наверх)        |
| **Удаление**          | Dequeue (из начала)  | Pop (сверху)         |
| **Просмотр**          | Peek (первый)        | Peek (верхний)       |
| **Производительность** | Все операции O(1) ⚡ | Все операции O(1) ⚡ |
| **Когда использовать** | Очереди, BFS        | Undo/Redo, DFS, скобки |

#### Визуальное сравнение

```
Queue (FIFO):
Enqueue ──► [A] [B] [C] ──► Dequeue
            ↑              ↑
          Back          Front
         (конец)       (начало)

Stack (LIFO):
Push ──► [C] ──► Pop
         │
        [B]
         │
        [A]
         │
        Top
```

#### Практические примеры

```csharp
// ✅ Queue - обработка заказов по порядку
var orderQueue = new Queue<Order>();
orderQueue.Enqueue(order1);
orderQueue.Enqueue(order2);

while (orderQueue.Count > 0) {
    var order = orderQueue.Dequeue();  // Обрабатываем в порядке поступления
    ProcessOrder(order);
}

// ✅ Stack - Undo операций
var undoStack = new Stack<Action>();
undoStack.Push(() => document.UndoDelete());  // Последняя операция сверху
undoStack.Push(() => document.UndoFormat());

if (userWantsUndo) {
    var lastAction = undoStack.Pop();  // Отменяем последнюю операцию
    lastAction();
}

// ✅ Stack - валидация скобок
public bool IsValid(string s) {
    var stack = new Stack<char>();
    foreach (char c in s) {
        if (c == '(') stack.Push(')');
        else if (c == '[') stack.Push(']');
        else if (c == '{') stack.Push('}');
        else if (stack.Count == 0 || stack.Pop() != c) return false;
    }
    return stack.Count == 0;
}
```

---

## 10. Сравнение производительности

### Сравнительная таблица

| Коллекция          | Доступ по индексу | Поиск | Вставка | Удаление | Когда использовать            |
| ------------------- | -------------- | ------- | ----------- | --------- | ----------------------- |
| **List<T>**         | O(1)           | O(n)    | O(1)\*      | O(n)      | Общий список          |
| **Dictionary<K,V>** | O(1)           | O(1)    | O(1)        | O(1)      | Поиск по ключу       |
| **HashSet<T>**      | N/A            | O(1)    | O(1)        | O(1)      | Уникальность, быстрый поиск |
| **Queue<T>**        | N/A            | O(n)    | O(1)        | O(1)      | FIFO обработка         |
| **Stack<T>**        | N/A            | O(n)    | O(1)        | O(1)      | LIFO обработка         |
| **LinkedList<T>**   | O(n)           | O(n)    | O(1)        | O(1)      | Частые вставки   |

\*O(1) амортизированно для вставки в конец

---

## 11. Шпаргалка по производительности коллекций (Big-O Cheat Sheet)

Быстрая справочная таблица сложности операций для основных коллекций C#.

### List<T>

```csharp
var list = new List<int>();
```

| Операция              | Сложность | Описание                              |
| --------------------- | --------- | ------------------------------------- |
| `Add(item)`           | O(1)*     | Добавление в конец (амортизированно)  |
| `Insert(index, item)` | O(n)      | Вставка по индексу                    |
| `Remove(item)`        | O(n)      | Удаление по значению                  |
| `RemoveAt(index)`     | O(n)      | Удаление по индексу                   |
| `list[index]`         | O(1) ⚡   | Доступ по индексу (чтение/запись)     |
| `Contains(item)`      | O(n)      | Поиск элемента                        |
| `IndexOf(item)`       | O(n)      | Поиск индекса элемента                |
| `Sort()`              | O(n log n)| Сортировка                             |

\* O(1) амортизированно, O(n) при необходимости расширения

### Dictionary<TKey, TValue>

```csharp
var dict = new Dictionary<string, int>();
```

| Операция                    | Сложность | Описание                              |
| --------------------------- | --------- | ------------------------------------- |
| `Add(key, value)`           | O(1) ⚡   | Добавление пары ключ-значение         |
| `dict[key] = value`         | O(1) ⚡   | Установка значения (добавление/обновление) |
| `dict[key]`                 | O(1) ⚡   | Доступ к значению по ключу            |
| `Remove(key)`               | O(1) ⚡   | Удаление по ключу                     |
| `ContainsKey(key)`          | O(1) ⚡   | Проверка наличия ключа                |
| `TryGetValue(key, out val)` | O(1) ⚡   | Безопасное получение значения         |
| `ContainsValue(value)`      | O(n) 🐌  | Поиск по значению (перебор всех)      |

### HashSet<T>

```csharp
var set = new HashSet<int>();
```

| Операция        | Сложность | Описание                              |
| --------------- | --------- | ------------------------------------- |
| `Add(item)`     | O(1) ⚡   | Добавление элемента                   |
| `Remove(item)`  | O(1) ⚡   | Удаление элемента                     |
| `Contains(item)`| O(1) ⚡   | Проверка наличия элемента             |
| `UnionWith()`   | O(n)      | Объединение множеств                  |
| `IntersectWith()`| O(n)    | Пересечение множеств                  |
| `ExceptWith()`  | O(n)      | Разность множеств                     |

### LinkedList<T>

```csharp
var linkedList = new LinkedList<int>();
```

| Операция                    | Сложность | Описание                              |
| --------------------------- | --------- | ------------------------------------- |
| `AddFirst(item)`            | O(1) ⚡   | Добавление в начало                   |
| `AddLast(item)`             | O(1) ⚡   | Добавление в конец                    |
| `AddAfter(node, item)`      | O(1) ⚡   | Добавление после узла                 |
| `AddBefore(node, item)`     | O(1) ⚡   | Добавление перед узлом                |
| `Remove(item)`              | O(n)      | Удаление по значению (нужен поиск)    |
| `Remove(node)`              | O(1) ⚡   | Удаление узла (если узел известен)    |
| `Find(item)`                | O(n) 🐌  | Поиск узла по значению                |
| Доступ по индексу           | N/A       | Нет индексатора (только через First/Last) |

### Stack<T>

```csharp
var stack = new Stack<int>();
```

| Операция      | Сложность | Описание                              |
| ------------- | --------- | ------------------------------------- |
| `Push(item)`  | O(1) ⚡   | Добавление наверх                     |
| `Pop()`       | O(1) ⚡   | Удаление сверху                       |
| `Peek()`      | O(1) ⚡   | Просмотр верхнего элемента            |
| `Contains()`  | O(n) 🐌  | Поиск элемента                        |

### Queue<T>

```csharp
var queue = new Queue<int>();
```

| Операция         | Сложность | Описание                              |
| ---------------- | --------- | ------------------------------------- |
| `Enqueue(item)`  | O(1) ⚡   | Добавление в конец                    |
| `Dequeue()`      | O(1) ⚡   | Удаление из начала                    |
| `Peek()`         | O(1) ⚡   | Просмотр первого элемента             |
| `Contains()`     | O(n) 🐌  | Поиск элемента                        |

### Array

```csharp
int[] array = new int[10];
```

| Операция           | Сложность | Описание                              |
| ------------------ | --------- | ------------------------------------- |
| `array[index]`     | O(1) ⚡   | Доступ по индексу (самый быстрый)     |
| `Array.IndexOf()`  | O(n)      | Поиск индекса элемента                |
| `Array.Sort()`     | O(n log n)| Сортировка                             |
| Изменение размера  | N/A       | Невозможно (нужно создавать новый)    |

### Сводная таблица всех коллекций

| Коллекция          | Доступ по индексу | Поиск | Добавление | Удаление | Когда использовать            |
| ------------------ | ----------------- | ----- | ---------- | -------- | ---------------------------- |
| **Array**          | O(1) ⚡          | O(n)  | N/A        | N/A      | Фиксированный размер         |
| **List<T>**        | O(1) ⚡          | O(n)  | O(1)* ⚡   | O(n)     | Общая коллекция (по умолчанию) |
| **Dictionary<K,V>**| O(1) ⚡ (по ключу)| O(1) ⚡ (по ключу) | O(1) ⚡ | O(1) ⚡ | Поиск по ключу               |
| **HashSet<T>**     | N/A               | O(1) ⚡ | O(1) ⚡   | O(1) ⚡   | Уникальность, быстрый поиск  |
| **Queue<T>**       | N/A               | O(n)  | O(1) ⚡   | O(1) ⚡   | FIFO обработка               |
| **Stack<T>**       | N/A               | O(n)  | O(1) ⚡   | O(1) ⚡   | LIFO обработка               |
| **LinkedList<T>**  | O(n) 🐌          | O(n)  | O(1) ⚡** | O(1) ⚡** | Частые вставки в середину    |

\* O(1) амортизированно для добавления в конец List  
\** O(1) если узел известен, иначе O(n) для поиска узла

### Легенда

- ⚡ **O(1)** - Константное время (очень быстро, идеально)
- 📈 **O(log n)** - Логарифмическое время (быстро)
- 📊 **O(n)** - Линейное время (приемлемо)
- 🐌 **O(n²)** - Квадратичное время (медленно, избегать)

---

## 12. Потокобезопасные коллекции

### Concurrent Collections

```csharp
using System.Collections.Concurrent;

// ConcurrentDictionary
var concurrentDict = new ConcurrentDictionary<string, int>();
concurrentDict.TryAdd("key", 1);
concurrentDict.TryUpdate("key", 2, 1);

// ConcurrentQueue
var concurrentQueue = new ConcurrentQueue<int>();
concurrentQueue.Enqueue(1);
if (concurrentQueue.TryDequeue(out int value)) {
    Console.WriteLine(value);
}

// ConcurrentBag (unordered)
var concurrentBag = new ConcurrentBag<int>();
concurrentBag.Add(1);
if (concurrentBag.TryTake(out int item)) {
    Console.WriteLine(item);
}
```

---

## 13. Best Practices

### ✅ Что делать

1. **Используйте List<T> для общих коллекций**

   ```csharp
   var items = new List<string>();
   ```

2. **Используйте Dictionary<K,V> для быстрого поиска**

   ```csharp
   var lookup = new Dictionary<int, string>();
   ```

3. **Используйте HashSet<T> для уникальности**

   ```csharp
   var unique = new HashSet<int>();
   ```

4. **Инициализируйте с ёмкостью, если знаете размер**
   ```csharp
   var list = new List<int>(1000); // Avoid resize
   ```

### ❌ Чего избегать

1. **Не используйте List<T> для частого поиска**

   ```csharp
   // ❌ WRONG - O(n) search
   var list = new List<Person>();
   var person = list.FirstOrDefault(p => p.Id == 123);

   // ✅ CORRECT - O(1) search
   var dict = new Dictionary<int, Person>();
   var person = dict[123];
   ```

2. **Не изменяйте коллекцию во время итерации**

   ```csharp
   // ❌ WRONG
   foreach (var item in list) {
       list.Remove(item); // Exception!
   }

   // ✅ CORRECT
   for (int i = list.Count - 1; i >= 0; i--) {
       list.RemoveAt(i);
   }
   ```

---

## 14. Практические примеры

### Пример 1: Кэш с Dictionary

```csharp
public class Cache<TKey, TValue> {
    private readonly Dictionary<TKey, TValue> _cache = new();
    private readonly int _maxSize;

    public Cache(int maxSize = 100) {
        _maxSize = maxSize;
    }

    public TValue Get(TKey key) {
        return _cache.TryGetValue(key, out var value) ? value : default;
    }

    public void Set(TKey key, TValue value) {
        if (_cache.Count >= _maxSize) {
            var firstKey = _cache.Keys.First();
            _cache.Remove(firstKey);
        }
        _cache[key] = value;
    }
}
```

### Пример 2: Управление очередью задач

```csharp
public class JobProcessor {
    private readonly Queue<Job> _jobQueue = new();

    public void EnqueueJob(Job job) {
        _jobQueue.Enqueue(job);
    }

    public async Task ProcessJobsAsync() {
        while (_jobQueue.Count > 0) {
            var job = _jobQueue.Dequeue();
            await ProcessJobAsync(job);
        }
    }
}
```

---

## 15. Внутреннее устройство коллекций

### 15.1. Как работает Dictionary внутри?

Dictionary<TKey, TValue> в C# реализован как **хеш-таблица** (hash table), что позволяет достичь O(1) сложности для операций поиска, добавления и удаления.

#### Принцип работы хеш-таблицы

**Шаг 1: Хеширование ключа**

Когда вы добавляете элемент в Dictionary:

```csharp
var dict = new Dictionary<string, int>();
dict["John"] = 30;

// 1. Ключ "John" преобразуется в хеш-код через GetHashCode()
int hashCode = "John".GetHashCode(); // Например: 123456789

// 2. Хеш-код используется для вычисления индекса в массиве бакетов
int bucketIndex = Math.Abs(hashCode) % buckets.Length; // Например: 5
```

**Шаг 2: Хранение в бакете**

```csharp
// 3. Элемент сохраняется в "бакете" (bucket) по вычисленному индексу
// Внутренняя структура:
buckets[bucketIndex] = new Entry {
    Key = "John",
    Value = 30,
    HashCode = hashCode,
    Next = null  // Для обработки коллизий
};
```

**Шаг 3: Поиск элемента**

```csharp
// При поиске:
int age = dict["John"];

// 1. Вычисляется хеш-код ключа (та же операция)
int hashCode = "John".GetHashCode();

// 2. Вычисляется индекс бакета
int bucketIndex = Math.Abs(hashCode) % buckets.Length;

// 3. Прямой доступ к бакету по индексу (O(1))
Entry entry = buckets[bucketIndex];

// 4. Проверка ключа через Equals() (на случай коллизий)
if (entry.HashCode == hashCode && entry.Key.Equals("John")) {
    return entry.Value; // Найдено! O(1)
}
```

#### Диаграмма: Внутренняя структура Dictionary

```
Dictionary<string, int>:
┌──────────────────────────────────────────────────────┐
│  Массив бакетов (buckets array)                     │
│                                                       │
│  Index 0: [Entry] → Key: "Alice", Value: 25         │
│  Index 1: [Entry] → Key: "Bob", Value: 28           │
│  Index 2: [Entry] → Key: "Charlie", Value: 22       │
│  Index 3: null                                       │
│  Index 4: null                                       │
│  Index 5: [Entry] → Key: "John", Value: 30          │
│           │                                          │
│           └─→ [Entry] → Key: "Jane", Value: 27      │
│                         (коллизия, цепочка)          │
│  ...                                                 │
└──────────────────────────────────────────────────────┘

Операция: dict["John"]
  1. GetHashCode("John") → 123456789
  2. Math.Abs(123456789) % buckets.Length → 5
  3. Прямой доступ к buckets[5] → O(1) ⚡
```

#### Почему Dictionary.ContainsKey = O(1)?

Dictionary использует **хеш-таблицу**, где:

1. **Хеш-функция** преобразует ключ в числовой индекс: `O(1)`
2. **Прямой доступ** к элементу массива по индексу: `O(1)`
3. **Проверка ключа** через Equals() (в среднем 1 элемент в бакете): `O(1)`

**Итого: O(1) + O(1) + O(1) = O(1)** ⚡

В худшем случае (много коллизий) может быть O(n), но в среднем и при хорошей хеш-функции - **O(1)**.

---

### 15.2. Что такое коллизия (collision) в Dictionary?

**Коллизия** возникает, когда два разных ключа имеют одинаковый хеш-код или попадают в один бакет (после вычисления индекса).

#### Как возникают коллизии?

```csharp
var dict = new Dictionary<string, int>();

// Пример коллизии
string key1 = "John";
string key2 = "Jane";

int hash1 = key1.GetHashCode(); // Например: 123456789
int hash2 = key2.GetHashCode(); // Например: 987654321

// После вычисления индекса бакета:
int index1 = Math.Abs(hash1) % bucketCount; // Например: 5
int index2 = Math.Abs(hash2) % bucketCount; // Например: 5 (коллизия!)

// Оба ключа попадают в один бакет (bucket 5)
dict[key1] = 30;
dict[key2] = 25;
```

#### Как Dictionary обрабатывает коллизии?

Dictionary использует метод **цепочки (chaining)**: элементы с одинаковым индексом бакета хранятся в связанном списке.

```csharp
// Внутренняя структура при коллизии:
Bucket 5:
┌──────────┐
│ Entry 1  │ → Key: "John", Value: 30, HashCode: 123
└────┬─────┘
     │
     ▼
┌──────────┐
│ Entry 2  │ → Key: "Jane", Value: 25, HashCode: 456
└────┬─────┘
     │
     ▼
┌──────────┐
│ Entry 3  │ → Key: "Bob", Value: 28, HashCode: 789
└──────────┘

Цепочка (Chaining) для обработки коллизий
```

**При поиске:**
1. Вычисляется индекс бакета (O(1))
2. Перебирается цепочка элементов в бакете
3. Сравниваются хеш-коды и ключи через Equals()

**В худшем случае** (все ключи в одном бакете): **O(n)**  
**В среднем случае**: **O(1)** (цепочек мало или они короткие)

#### Как избежать коллизий?

1. **Хорошая хеш-функция** - равномерное распределение хеш-кодов
2. **Правильный размер** - Dictionary автоматически расширяется при необходимости
3. **Качественная реализация GetHashCode()** для кастомных типов ключей

```csharp
// ✅ ХОРОШО: Правильная реализация GetHashCode()
public class PersonKey {
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public override int GetHashCode() {
        // Используйте HashCode.Combine для равномерного распределения
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

---

### 15.3. List.Contains vs HashSet.Contains: Что быстрее?

#### List.Contains: O(n)

```csharp
var list = new List<int> { 1, 2, 3, 4, 5, /* ... 1000 элементов */ };

bool exists = list.Contains(500);
// Нужно проверить ВСЕ элементы последовательно:
// Проверка 1, 2, 3, ..., 499, 500 = 500 операций сравнения
// В худшем случае: n операций = O(n) 🐌
```

**Диаграмма поиска в List:**
```
List: [1] [2] [3] ... [499] [500] ... [1000]
       ↓   ↓   ↓        ↓     ↓         ↓
      Проверка каждого элемента последовательно
      В худшем случае: n операций = O(n)
```

#### HashSet.Contains: O(1)

```csharp
var set = new HashSet<int> { 1, 2, 3, 4, 5, /* ... 1000 элементов */ };

bool exists = set.Contains(500);
// 1. Вычисляется хеш: GetHashCode(500) = 500
// 2. Вычисляется индекс бакета: 500 % bucketCount = индекс
// 3. Прямой доступ к бакету = 1 операция!
// В среднем: O(1) ⚡
```

**Диаграмма поиска в HashSet:**
```
HashSet:
  Hash(500) → Index → Bucket → Value
       ↓         ↓        ↓        ↓
     O(1)      O(1)     O(1)     O(1)
     Всего: O(1) операция!
```

#### Практическое сравнение

```csharp
// Тест производительности
var list = new List<int>();
var set = new HashSet<int>();

// Заполнение (100,000 элементов)
for (int i = 0; i < 100000; i++) {
    list.Add(i);
    set.Add(i);
}

// Поиск в List: O(n) - в среднем n/2 операций
var stopwatch = Stopwatch.StartNew();
bool found1 = list.Contains(50000);
stopwatch.Stop();
Console.WriteLine($"List.Contains: {stopwatch.ElapsedMilliseconds} мс");
// Результат: ~2-5 мс (50,000 сравнений в среднем)

// Поиск в HashSet: O(1) - 1 операция
stopwatch.Restart();
bool found2 = set.Contains(50000);
stopwatch.Stop();
Console.WriteLine($"HashSet.Contains: {stopwatch.ElapsedMilliseconds} мс");
// Результат: ~0.001 мс (1 операция)

// HashSet.Contains быстрее в 1000-5000 раз! ⚡
```

**Вывод:** HashSet.Contains **значительно быстрее** List.Contains для больших коллекций (O(1) vs O(n)).

---

### 15.4. Можно ли хранить дубликаты в HashSet?

**Нет, HashSet<T> автоматически удаляет дубликаты** - это его основная функция.

```csharp
var set = new HashSet<int>();

set.Add(1);
set.Add(2);
set.Add(1);  // Дубликат - игнорируется
set.Add(3);
set.Add(2);  // Дубликат - игнорируется

Console.WriteLine(set.Count); // 3 (не 5!)
// Содержит только: { 1, 2, 3 }
```

#### Как HashSet определяет дубликаты?

HashSet использует **GetHashCode()** и **Equals()** для определения уникальности:

```csharp
var set = new HashSet<string>();

set.Add("John");
set.Add("john");  // Разные строки (разный регистр)
set.Add("John");  // Дубликат - игнорируется

Console.WriteLine(set.Count); // 2: { "John", "john" }
```

#### Если нужны дубликаты - используйте List

```csharp
// ✅ List разрешает дубликаты
var list = new List<int> { 1, 2, 1, 3, 2 };
Console.WriteLine(list.Count); // 5 элементов

// ✅ HashSet удаляет дубликаты
var set = new HashSet<int> { 1, 2, 1, 3, 2 };
Console.WriteLine(set.Count); // 3 уникальных элемента
```

---

## 16. Шаблоны и техники алгоритмов (Algorithm Patterns)

Этот раздел описывает **типичные структуры данных и приёмы**, которые часто встречаются в задачах на алгоритмы и собеседованиях. Для каждого подхода указаны сложность, когда его использовать и готовые шаблоны кода на C#.

---

### 16.1. PriorityQueue&lt;TElement, TPriority&gt; — куча с приоритетами

**Сложность:** Enqueue / Dequeue — **O(log n)**. Peek — **O(1)**.

В .NET 6+ `PriorityQueue<TElement, TPriority>` реализован как **min-heap**: при извлечении через `Dequeue()` всегда получаем элемент с **минимальным** приоритетом. Для «максимального первым» можно использовать отрицательный приоритет или кастомный компаратор.

#### Когда использовать

| Задача | Зачем нужна куча |
|--------|-------------------|
| **Top-K наибольших/наименьших** | Храним только K элементов, лишние выкидываем через Dequeue — остаётся K-й по величине/минимум. |
| **Всегда быстро брать минимум** | Min-heap даёт минимум за O(1) (Peek), обновление кучи за O(log n). |
| **Merge K отсортированных списков** | В куче храним по одному текущему элементу от каждого списка, извлекаем минимум и подставляем следующий из того же списка. |
| **Алгоритм Дейкстры** | Обрабатываем вершины в порядке возрастания расстояния; куча по расстоянию даёт следующую вершину за O(log n). |

#### Базовое использование

```csharp
// .NET 6+ — min-heap по умолчанию (минимальный приоритет извлекается первым)
var pq = new PriorityQueue<int, int>();

pq.Enqueue(element: 10, priority: 10);  // add
pq.Enqueue(element: 5, priority: 5);
pq.Enqueue(element: 20, priority: 20);

int min = pq.Dequeue();   // 5  — элемент с минимальным приоритетом
int next = pq.Peek();    // 10 — посмотреть следующий минимум без извлечения
```

#### Пример: Top-K наибольших (K-й по величине элемент)

Идея: храним в min-heap ровно **K** наибольших элементов. Для каждого числа добавляем его в кучу; если размер стал больше K — извлекаем минимум (он точно не входит в Top-K). В конце на вершине кучи — K-й наибольший элемент.

```csharp
int FindKthLargest(int[] nums, int k) {
    var pq = new PriorityQueue<int, int>();  // min-heap by value

    foreach (int n in nums) {
        pq.Enqueue(n, n);
        if (pq.Count > k)
            pq.Dequeue();  // remove smallest, keep only k largest
    }
    return pq.Peek();  // k-th largest is at the top of min-heap of size k
}
```

**Сложность:** O(n log K) по времени, O(K) по памяти.

#### Связь с другими темами

- **Heap** — структура данных под капотом (двоичная куча).
- **Top-K** — классическая задача для приоритетной очереди.
- **Dijkstra** — кратчайший путь в графе с неотрицательными весами.
- **Merge K sorted lists** — слияние K отсортированных последовательностей за O(N log K).

---

### 16.2. Массив и List&lt;T&gt; для DP, prefix sum и счётчиков

**Сложность:** доступ по индексу **get/set — O(1)**.

Массив `int[]` и `List<T>` с доступом по индексу — основа для таблиц динамического программирования, префиксных сумм и счётчиков частот.

#### Когда использовать

| Приём | Назначение |
|-------|------------|
| **DP-таблица** | `dp[i]` — ответ для подзадачи размера i (например, число способов, минимальная стоимость). |
| **Prefix sum** | `pre[i]` — сумма элементов `arr[0..i-1]`; сумма на отрезке `[l..r]` = `pre[r+1] - pre[l]`. |
| **Счётчики частот** | `freq[c]` для символов/цифр (индекс — код символа или цифра). |
| **Two Pointers** | Два индекса `left`, `right` на одном отсортированном массиве — поиск пары, тройки и т.д. |

#### DP: пример Climbing Stairs

```csharp
// dp[i] = number of ways to reach step i
int ClimbStairs(int n) {
    if (n <= 1) return 1;
    int[] dp = new int[n + 1];
    dp[0] = 1;
    dp[1] = 1;
    for (int i = 2; i <= n; i++)
        dp[i] = dp[i - 1] + dp[i - 2];
    return dp[n];
}
```

#### Prefix Sum

```csharp
// pre[i] = sum of nums[0..i-1]; sum of [l..r] = pre[r+1] - pre[l]
int[] BuildPrefixSum(int[] nums) {
    int n = nums.Length;
    int[] pre = new int[n + 1];
    for (int i = 0; i < n; i++)
        pre[i + 1] = pre[i] + nums[i];
    return pre;
}

int RangeSum(int[] pre, int left, int right) {
    return pre[right + 1] - pre[left];
}
```

#### Счётчики частот (буквы/цифры)

```csharp
// Frequency of each character in string
int[] freq = new int[128];  // or 256 for extended ASCII
foreach (char c in s)
    freq[c]++;

// Frequency of digits 0..9
int[] digitCount = new int[10];
foreach (char c in numString)
    digitCount[c - '0']++;
```

---

### 16.3. Two Pointers (два указателя)

**Сложность:** один проход — **O(n)**.

Два индекса `left` и `right` двигаются по массиву (часто по отсортированному). Используется для поиска пары с заданной суммой, удаления дубликатов, проверки палиндрома и т.д.

#### Когда использовать

- Отсортированный массив: найти пару с суммой K.
- In-place операции: сдвинуть нули в конец, удалить дубликаты.
- Палиндром: два указателя с концов к центру.

#### Шаблон: пара с суммой K (массив отсортирован)

```csharp
bool HasPairWithSum(int[] nums, int target) {
    int left = 0, right = nums.Length - 1;
    while (left < right) {
        int sum = nums[left] + nums[right];
        if (sum == target) return true;
        if (sum < target) left++;
        else right--;
    }
    return false;
}
```

---

### 16.4. Sliding Window (скользящее окно) + Dictionary

**Сложность:** один проход — **O(n)**.

Окно — подмассив `[left..right]`. Словарь хранит состояние окна (частоты символов, счётчики). Правый указатель расширяет окно, левый сужает, когда условие нарушено.

#### Когда использовать

- Самая длинная подстрока без повторяющихся символов.
- Минимальное окно, содержащее все символы из набора.
- Подмассив с максимальной суммой длины K (или с фиксированным K — без словаря, просто окно фиксированного размера).

#### Шаблон: подстрока без повторов

```csharp
int LengthOfLongestSubstring(string s) {
    int left = 0, maxLen = 0;
    var window = new Dictionary<char, int>();

    for (int right = 0; right < s.Length; right++) {
        char c = s[right];
        window[c] = window.GetValueOrDefault(c, 0) + 1;

        // Shrink window until no duplicate in window
        while (window[c] > 1) {
            window[s[left]]--;
            left++;
        }
        maxLen = Math.Max(maxLen, right - left + 1);
    }
    return maxLen;
}
```

---

### 16.5. Рекурсия: шаблон для деревьев и графов

**Сложность:** зависит от задачи (обычно O(n) по числу узлов/вершин).

Для обхода дерева и графа рекурсия — естественный способ: базовый случай (null или лист), рекурсивные вызовы для поддеревьев/соседей, затем объединение результатов.

#### Когда использовать

- Задачи на дереве: высота, сумма, проверка свойств.
- DFS на графе.
- Divide and Conquer (merge sort, quick sort).
- Перебор: перестановки, комбинации (backtracking).

#### Шаблон: DFS по дереву

```csharp
// Example: max depth of binary tree
int Dfs(TreeNode node) {
    // 1. Base case — always first
    if (node == null) return 0;

    // 2. Recurse into children
    int left  = Dfs(node.left);
    int right = Dfs(node.right);

    // 3. Compute result and return
    return Math.Max(left, right) + 1;
}
```

Тот же подход применим к обходу графа (посещённые вершины хранить в `HashSet` или массиве).

---

### 16.6. Binary Search (бинарный поиск)

**Сложность:** **O(log n)**.

Ищем в отсортированном массиве или по монотонному условию (binary search по ответу). Важно корректно вычислять `mid`, чтобы не было переполнения.

#### Когда использовать

- Отсортированный массив: найти элемент или границу (первый ≥ X, последний ≤ X).
- Задачи вида «минимизировать максимум» или «максимизировать минимум» — бинарный поиск по ответу.

#### Классический бинарный поиск

```csharp
int BinarySearch(int[] nums, int target) {
    int lo = 0, hi = nums.Length - 1;
    while (lo <= hi) {
        // Avoid overflow: do not use (lo + hi) / 2
        int mid = lo + (hi - lo) / 2;
        if (nums[mid] == target) return mid;
        if (nums[mid] < target) lo = mid + 1;
        else hi = mid - 1;
    }
    return -1;
}
```

**Правило:** `mid = lo + (hi - lo) / 2` — всегда так, чтобы избежать переполнения при больших `lo` и `hi`.

#### Сводная таблица техник

| Техника | Сложность | Типичные задачи |
|---------|-----------|------------------|
| **PriorityQueue** | Enqueue/Dequeue O(log n) | Top-K, Dijkstra, Merge K lists |
| **int[] / List для DP** | get/set O(1) | DP, prefix sum, counting |
| **Two Pointers** | O(n) | Пара с суммой, палиндром, in-place |
| **Sliding Window + Dict** | O(n) | Подстрока без повторов, минимальное окно |
| **Рекурсия (DFS)** | O(n) по узлам | Деревья, графы, backtracking |
| **Binary Search** | O(log n) | Поиск в отсортированном, поиск по ответу |

---

## 17. Часто задаваемые вопросы (FAQ)

### Q1: Когда использовать List, а когда LinkedList?

**A:** 

**Используйте List<T> когда:**
- ✅ Нужен доступ по индексу: `list[5]` (O(1))
- ✅ Часто добавляете элементы в конец (O(1) амортизированно)
- ✅ Общая коллекция для большинства случаев
- ✅ Нужна сортировка, поиск, фильтрация через LINQ

```csharp
// ✅ Идеально для List
var items = new List<string>();
items.Add("item1");
var first = items[0];  // Доступ по индексу - O(1)
```

**Используйте LinkedList<T> когда:**
- ✅ Частые вставки/удаления в середине списка (O(1) если узел известен)
- ✅ Частые вставки в начало (O(1))
- ❌ НЕ нужен доступ по индексу (иначе O(n) - медленно!)
- ❌ НЕ нужен быстрый поиск

```csharp
// ✅ Идеально для LinkedList
var undoStack = new LinkedList<Action>();
var node = undoStack.AddFirst(UndoAction);  // O(1)
undoStack.AddAfter(node, AnotherAction);    // O(1) - быстро!
```

**Сравнение:**
| Операция | List<T> | LinkedList<T> |
|----------|---------|---------------|
| Доступ по индексу | O(1) ⚡ | O(n) 🐌 |
| Вставка в начало | O(n) | O(1) ⚡ |
| Вставка в конец | O(1)* ⚡ | O(1) ⚡ |
| Вставка в середину | O(n) | O(1)** ⚡ |

\* O(1) амортизированно  
\** O(1) если узел известен

---

### Q2: В чем разница Dictionary и HashSet?

**A:** 

| Критерий | Dictionary<TKey, TValue> | HashSet<T> |
|----------|-------------------------|------------|
| **Хранение** | Пары ключ-значение | Только значения |
| **Ключи** | Уникальные | Нет ключей (только значения) |
| **Доступ** | По ключу: `dict["key"]` | По значению: `set.Contains(value)` |
| **Использование** | Ключ → значение (маппинг) | Уникальность, проверка наличия |
| **Производительность** | O(1) доступ по ключу | O(1) проверка наличия |

```csharp
// Dictionary - хранит пары ключ-значение
var dict = new Dictionary<string, int>();
dict["John"] = 30;  // Ключ "John" → значение 30
int age = dict["John"];  // Получаем значение по ключу

// HashSet - хранит только уникальные значения
var set = new HashSet<string>();
set.Add("John");  // Только значение, нет ключей
bool exists = set.Contains("John");  // Проверка наличия
```

**Когда использовать:**
- **Dictionary**: когда нужна связь ключ → значение (кэш, маппинг, группировка)
- **HashSet**: когда нужна только проверка наличия или уникальность

---

### Q3: Что такое O(1) и O(n)?

**A:** Это **Big-O нотация** - способ описания сложности алгоритмов.

**O(1) - Константное время** ⚡
- Время выполнения **не зависит** от размера данных
- Всегда выполняется за постоянное время
- Примеры: доступ к элементу массива по индексу, доступ к Dictionary по ключу

```csharp
// O(1) - всегда 1 операция, независимо от размера
int[] arr = new int[1000000];
int value = arr[500000];  // O(1) - прямой доступ

var dict = new Dictionary<string, int>();
int age = dict["John"];  // O(1) - прямой доступ по ключу
```

**O(n) - Линейное время** 📈
- Время выполнения **пропорционально** размеру данных
- Если данных в 2 раза больше - времени нужно в 2 раза больше
- Примеры: поиск в неотсортированном списке, перебор всех элементов

```csharp
// O(n) - нужно проверить все элементы
var list = new List<int> { 1, 2, 3, /* ... 1000 элементов */ };
bool found = list.Contains(500);  // O(n) - проверка всех элементов

// Для 1000 элементов: ~500 проверок в среднем
// Для 10000 элементов: ~5000 проверок в среднем
```

**Визуализация:**
```
O(1):        O(n):
Время        Время
  │            │     ╱
  │            │    ╱
  │            │   ╱
  │            │  ╱
  └────────    └─╱─────
   Размер        Размер
```

См. также раздел **2. Big-O нотация** для подробного объяснения.

---

### Q4: Когда Stack лучше чем Queue?

**A:** Stack и Queue используются для разных задач:

**Stack (LIFO - Last In, First Out):**
- ✅ **Undo/Redo** операции
- ✅ **Валидация скобок** в выражениях
- ✅ **Depth-First Search (DFS)** в графах
- ✅ **Вызовы функций** (call stack)
- ✅ **Обработка в обратном порядке**

```csharp
// ✅ Stack для Undo операций
var undoStack = new Stack<Action>();
undoStack.Push(() => document.DeleteText());
undoStack.Push(() => document.FormatBold());

// Отменяем последнюю операцию первой
undoStack.Pop()();  // Отменяет FormatBold
undoStack.Pop()();  // Отменяет DeleteText
```

**Queue (FIFO - First In, First Out):**
- ✅ **Обработка заказов** в порядке поступления
- ✅ **Breadth-First Search (BFS)** в графах
- ✅ **Очереди сообщений/событий**
- ✅ **Планировщики задач**
- ✅ **Обработка в порядке поступления**

```csharp
// ✅ Queue для обработки заказов
var orderQueue = new Queue<Order>();
orderQueue.Enqueue(order1);  // Первый заказ
orderQueue.Enqueue(order2);  // Второй заказ

var first = orderQueue.Dequeue();  // Обрабатываем первый
var second = orderQueue.Dequeue(); // Обрабатываем второй
```

**Сравнение:**
- **Stack**: Последний пришёл - первый ушёл (как стопка тарелок)
- **Queue**: Первый пришёл - первый ушёл (как очередь в магазине)

---

### Q5: Как работает Dictionary внутри?

**A:** Dictionary реализован как **хеш-таблица (hash table)**. Подробное объяснение см. в разделе **15.1. Как работает Dictionary внутри?**.

**Краткое объяснение:**
1. Ключ преобразуется в хеш-код через `GetHashCode()`
2. Хеш-код используется для вычисления индекса в массиве бакетов
3. Элемент хранится в бакете по этому индексу
4. При поиске - та же операция: ключ → хеш → индекс → прямой доступ (O(1))

```csharp
dict["John"] = 30;

// Внутри:
// 1. "John".GetHashCode() → 123456789
// 2. Math.Abs(123456789) % buckets.Length → 5
// 3. buckets[5] = { Key: "John", Value: 30 }
// 4. Поиск: та же операция → buckets[5] → O(1)
```

---

### Q6: Почему Dictionary.ContainsKey = O(1)?

**A:** Dictionary использует хеш-таблицу, которая позволяет напрямую вычислять индекс элемента по ключу:

1. Хеш-функция: `GetHashCode(key)` → O(1)
2. Вычисление индекса: `hashCode % bucketCount` → O(1)
3. Прямой доступ к массиву: `buckets[index]` → O(1)
4. Проверка ключа (в среднем 1 элемент в бакете) → O(1)

**Итого: O(1) + O(1) + O(1) + O(1) = O(1)** ⚡

В худшем случае (много коллизий) может быть O(n), но в среднем при хорошей хеш-функции - **O(1)**.

Подробнее см. раздел **15.1. Как работает Dictionary внутри?**.

---

### Q7: Можно ли хранить дубликаты в HashSet?

**A:** **Нет**, HashSet автоматически удаляет дубликаты - это его основная функция.

```csharp
var set = new HashSet<int>();
set.Add(1);
set.Add(2);
set.Add(1);  // Дубликат - игнорируется!
set.Add(3);
set.Add(2);  // Дубликат - игнорируется!

Console.WriteLine(set.Count); // 3 (не 5!)
// Содержит только: { 1, 2, 3 }
```

Если нужны дубликаты - используйте **List<T>**.

Подробнее см. раздел **15.4. Можно ли хранить дубликаты в HashSet?**.

---

### Q8: Что быстрее: List.Contains или HashSet.Contains?

**A:** **HashSet.Contains значительно быстрее** для больших коллекций.

- **List.Contains**: O(n) - нужно проверить все элементы последовательно
- **HashSet.Contains**: O(1) - прямой доступ через хеш-таблицу

**На практике:**
- Для 100,000 элементов: HashSet.Contains быстрее в **1000-5000 раз**
- List.Contains: ~2-5 мс (50,000 сравнений в среднем)
- HashSet.Contains: ~0.001 мс (1 операция)

Подробное сравнение см. в разделе **15.3. List.Contains vs HashSet.Contains**.

---

### Q9: Когда использовать Array вместо List?

**A:** 

**Используйте Array когда:**
- ✅ Размер **известен и не изменится**
- ✅ Нужна **максимальная производительность**
- ✅ Много операций **доступа по индексу**
- ✅ Работа с **байтами, числовыми данными**

```csharp
// ✅ Идеально для Array
int[] scores = new int[100];  // Фиксированный размер
scores[0] = 95;
int firstScore = scores[0];  // O(1) - самый быстрый доступ
```

**Используйте List<T> когда:**
- ✅ Размер может изменяться
- ✅ Нужно добавлять/удалять элементы
- ✅ Общая коллекция (по умолчанию)

```csharp
// ✅ Идеально для List
var items = new List<string>();
items.Add("item1");  // Размер автоматически увеличивается
items.Remove("item1");
```

**Сравнение:**
| Критерий | Array | List<T> |
|----------|-------|---------|
| Размер | Фиксированный | Динамический |
| Добавление элементов | Невозможно | O(1)* |
| Доступ по индексу | O(1) ⚡ | O(1) ⚡ |
| Производительность | Максимальная | Немного медленнее |

Подробнее см. раздел **9.1. Array vs List<T> vs LinkedList<T>**.

---

### Q10: Что такое collision (коллизия) в Dictionary?

**A:** **Коллизия** возникает, когда два разных ключа имеют одинаковый хеш-код или попадают в один бакет (после вычисления индекса).

```csharp
// Пример коллизии
var dict = new Dictionary<string, int>();

string key1 = "John";
string key2 = "Jane";

// Оба ключа могут попасть в один бакет (после вычисления индекса)
dict[key1] = 30;  // Может попасть в bucket 5
dict[key2] = 25;  // Может попасть в bucket 5 (коллизия!)
```

**Как Dictionary обрабатывает коллизии:**
Dictionary использует метод **цепочки (chaining)**: элементы с одинаковым индексом бакета хранятся в связанном списке. При поиске перебирается цепочка элементов.

- **В среднем случае**: O(1) (цепочек мало)
- **В худшем случае**: O(n) (все ключи в одном бакете)

Подробное объяснение см. в разделе **15.2. Что такое коллизия (collision) в Dictionary?**.

---

### Дополнительные вопросы

### Q: В чём разница между Dictionary и Hashtable?

**A:** `Dictionary<TKey, TValue>` является generic и type-safe. `Hashtable` — это legacy класс и не generic (работает с object).

### Q: Сохраняет ли HashSet порядок?

**A:** Нет, `HashSet<T>` не гарантирует порядок элементов. Используйте `SortedSet<T>`, если нужна сортировка.

---

## Заключение

Выбирайте правильную коллекцию для:

- ✅ Оптимальной производительности
- ✅ Более читаемого кода
- ✅ Эффективных операций
- ✅ Правильного поведения

**Общее правило:**

- **List<T>**: Общая коллекция
- **Dictionary<K,V>**: Поиск по ключу
- **HashSet<T>**: Уникальность и быстрый поиск
- **Queue<T>**: FIFO обработка
- **Stack<T>**: LIFO обработка

---

*Документ создан для объяснения коллекций в C# с практическими примерами и best practices.*

