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

## 2. List<T> - Динамический список

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

## 3. Dictionary<TKey, TValue> - Словарь ключ-значение

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

## 4. HashSet<T> - Уникальное множество

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

## 5. Queue<T> - Очередь FIFO

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

## 6. Stack<T> - Стек LIFO

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

## 7. LinkedList<T> - Связанный список

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

## 8. Сравнение производительности

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

## 9. Потокобезопасные коллекции

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

## 10. Best Practices

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

## 11. Практические примеры

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

## 12. Часто задаваемые вопросы (FAQ)

### Q: Когда использовать List vs Array?

**A:** Используйте `List<T>`, когда размер может изменяться. Используйте `Array`, когда размер фиксирован и известен.

### Q: В чём разница между Dictionary и Hashtable?

**A:** `Dictionary<TKey, TValue>` является generic и type-safe. `Hashtable` — это legacy и не generic.

### Q: Сохраняет ли HashSet порядок?

**A:** Нет, `HashSet<T>` не гарантирует порядок. Используйте `SortedSet<T>`, если нужна сортировка.

### Q: Когда использовать Queue vs Stack?

**A:** `Queue` для FIFO (первым пришёл — первым ушёл). `Stack` для LIFO (как undo/redo).

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

