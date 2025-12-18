# LINQ (Language Integrated Query) в C#

## Введение

**LINQ** (Language Integrated Query) — это функциональность C#, которая позволяет выполнять запросы к коллекциям данных в декларативном и type-safe стиле. LINQ интегрирует возможность запросов непосредственно в язык C#.

---

## 1. Что такое LINQ?

### Определение

LINQ позволяет писать запросы, похожие на SQL, непосредственно в C#, работая с:
- Коллекциями в памяти (List, Array, Dictionary)
- Базами данных (Entity Framework)
- XML
- Другими источниками данных

### Преимущества

✅ **Type-safe** - Проверка типов на этапе компиляции  
✅ **IntelliSense** - Полная поддержка IDE  
✅ **Декларативный** - Что вы хотите, а не как это получить  
✅ **Переиспользуемый** - Один и тот же синтаксис для разных источников  

### Диаграмма: Архитектура LINQ

```
┌─────────────────────────────────────────────┐
│              LINQ                           │
└─────────────────────────────────────────────┘
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│ LINQ to   │ │ LINQ to   │ │ LINQ to   │
│ Objects   │ │ SQL       │ │ XML       │
│           │ │ (EF)      │ │           │
└───────────┘ └───────────┘ └───────────┘
```

---

## 2. Синтаксис LINQ: Query Syntax vs Method Syntax

### Query Syntax (Похож на SQL)

```csharp
// Query Syntax
var result = from student in students
             where student.Age > 18
             orderby student.Name
             select student;
```

### Method Syntax (Fluent API)

```csharp
// Method Syntax (более распространённый)
var result = students
    .Where(s => s.Age > 18)
    .OrderBy(s => s.Name)
    .ToList();
```

### Сравнение

```csharp
public class Student {
    public string Name { get; set; }
    public int Age { get; set; }
    public int Grade { get; set; }
}

var students = new List<Student> {
    new Student { Name = "John", Age = 20, Grade = 85 },
    new Student { Name = "Jane", Age = 19, Grade = 90 },
    new Student { Name = "Bob", Age = 17, Grade = 95 }
};

// Query Syntax
var querySyntax = from s in students
                  where s.Age >= 18
                  orderby s.Grade descending
                  select s.Name;

// Method Syntax (эквивалент)
var methodSyntax = students
    .Where(s => s.Age >= 18)
    .OrderByDescending(s => s.Grade)
    .Select(s => s.Name);
```

---

## 3. Основные операторы LINQ

### Where - Фильтрация

```csharp
// Фильтрует элементы, удовлетворяющие условию
var adults = students.Where(s => s.Age >= 18);

// Несколько условий
var topStudents = students
    .Where(s => s.Age >= 18 && s.Grade > 90);
```

**Возвращает:** `IEnumerable<T>` - отложенное выполнение

### Select - Проекция

```csharp
// Преобразует каждый элемент
var names = students.Select(s => s.Name);

// Сложная проекция
var studentInfo = students.Select(s => new {
    Name = s.Name,
    IsAdult = s.Age >= 18
});
```

**Возвращает:** `IEnumerable<TResult>` - отложенное выполнение

### OrderBy / OrderByDescending - Сортировка

```csharp
// Сортировка по возрастанию
var sortedByName = students.OrderBy(s => s.Name);

// Сортировка по убыванию
var sortedByGrade = students.OrderByDescending(s => s.Grade);

// Множественная сортировка
var sorted = students
    .OrderBy(s => s.Age)
    .ThenByDescending(s => s.Grade);
```

**Возвращает:** `IOrderedEnumerable<T>` - отложенное выполнение

### First / FirstOrDefault

```csharp
// Первый элемент (выбрасывает исключение, если пусто)
var first = students.First();

// Первый элемент с условием
var firstAdult = students.First(s => s.Age >= 18);

// Первый элемент или значение по умолчанию (null, если пусто)
var firstOrDefault = students.FirstOrDefault();
var firstAdultOrNull = students.FirstOrDefault(s => s.Age >= 18);
```

**Возвращает:** `T` (First) или `T?` (FirstOrDefault) - немедленное выполнение

### Last / LastOrDefault

```csharp
// Последний элемент
var last = students.Last();
var lastOrDefault = students.LastOrDefault();
```

**Возвращает:** `T` (Last) или `T?` (LastOrDefault) - немедленное выполнение

### Single / SingleOrDefault

```csharp
// Один элемент (выбрасывает исключение, если 0 или больше 1)
var single = students.Single(s => s.Name == "John");

// Один элемент или значение по умолчанию
var singleOrDefault = students.SingleOrDefault(s => s.Name == "John");
```

**Возвращает:** `T` (Single) или `T?` (SingleOrDefault) - немедленное выполнение

### Any / All

```csharp
// Проверяет, удовлетворяет ли хотя бы один элемент условию
bool hasAdults = students.Any(s => s.Age >= 18);

// Проверяет, удовлетворяют ли все элементы условию
bool allAdults = students.All(s => s.Age >= 18);
```

**Возвращает:** `bool` - немедленное выполнение

### Count / Count с условием

```csharp
// Подсчитывает все элементы
int total = students.Count();

// Подсчитывает элементы, удовлетворяющие условию
int adultsCount = students.Count(s => s.Age >= 18);
```

**Возвращает:** `int` - немедленное выполнение

### Sum / Average / Min / Max

```csharp
// Сумма
int totalAge = students.Sum(s => s.Age);

// Среднее значение
double averageAge = students.Average(s => s.Age);

// Минимальное значение
int minAge = students.Min(s => s.Age);

// Максимальное значение
int maxAge = students.Max(s => s.Age);
```

**Возвращает:** `TResult` (числовой тип) - немедленное выполнение

---

## 4. Операторы группировки

### GroupBy

```csharp
// Группирует по свойству
var groupedByAge = students.GroupBy(s => s.Age);

foreach (var group in groupedByAge) {
    Console.WriteLine($"Возраст: {group.Key}");
    foreach (var student in group) {
        Console.WriteLine($"  - {student.Name}");
    }
}

// Группировка с проекцией
var grouped = students.GroupBy(s => s.Age, s => s.Name);
```

**Возвращает:** `IEnumerable<IGrouping<TKey, TElement>>` - отложенное выполнение

### Diagramma: GroupBy

```
┌─────────────────────────────────────────────┐
│  Students:                                   │
│  [John, 20]                                 │
│  [Jane, 19]                                 │
│  [Bob, 20]                                  │
│  [Alice, 19]                                │
└─────────────────────────────────────────────┘
                    │
                    ▼ GroupBy(s => s.Age)
                    │
┌─────────────────────────────────────────────┐
│  Group Key: 20                              │
│    - John                                   │
│    - Bob                                    │
├─────────────────────────────────────────────┤
│  Group Key: 19                              │
│    - Jane                                   │
│    - Alice                                  │
└─────────────────────────────────────────────┘
```

---

## 5. Операторы Join

### Join

```csharp
public class Student {
    public int Id { get; set; }
    public string Name { get; set; }
    public int CourseId { get; set; }
}

public class Course {
    public int Id { get; set; }
    public string Name { get; set; }
}

var students = new List<Student> {
    new Student { Id = 1, Name = "John", CourseId = 1 },
    new Student { Id = 2, Name = "Jane", CourseId = 2 }
};

var courses = new List<Course> {
    new Course { Id = 1, Name = "C#" },
    new Course { Id = 2, Name = "Java" }
};

// Inner Join
var studentCourses = students.Join(
    courses,
    student => student.CourseId,
    course => course.Id,
    (student, course) => new {
        StudentName = student.Name,
        CourseName = course.Name
    }
);

// Query Syntax
var querySyntax = from s in students
                  join c in courses on s.CourseId equals c.Id
                  select new { s.Name, c.Name };
```

**Возвращает:** `IEnumerable<TResult>` - отложенное выполнение

### GroupJoin

```csharp
// Эквивалент Left Join
var studentCourses = students.GroupJoin(
    courses,
    student => student.CourseId,
    course => course.Id,
    (student, courseGroup) => new {
        Student = student.Name,
        Courses = courseGroup.Select(c => c.Name)
    }
);
```

**Возвращает:** `IEnumerable<TResult>` - отложенное выполнение

---

## 6. Операторы множеств

### Distinct

```csharp
var numbers = new[] { 1, 2, 2, 3, 3, 4 };
var unique = numbers.Distinct(); // { 1, 2, 3, 4 }

// С пользовательскими объектами
var uniqueStudents = students.Distinct(); // Требует Equals/GetHashCode
```

**Возвращает:** `IEnumerable<T>` - отложенное выполнение

### Union / Intersect / Except

```csharp
var set1 = new[] { 1, 2, 3, 4 };
var set2 = new[] { 3, 4, 5, 6 };

// Объединение (все уникальные элементы)
var union = set1.Union(set2); // { 1, 2, 3, 4, 5, 6 }

// Пересечение (общие элементы)
var intersect = set1.Intersect(set2); // { 3, 4 }

// Разность (элементы в set1, но не в set2)
var except = set1.Except(set2); // { 1, 2 }
```

**Возвращает:** `IEnumerable<T>` - отложенное выполнение

---

## 7. Операторы разбиения

### Take / Skip

```csharp
// Берёт первые N элементов
var firstThree = students.Take(3);

// Пропускает первые N элементов
var skipFirst = students.Skip(2);

// Пагинация
int pageSize = 10;
int pageNumber = 2;
var page = students
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize);
```

**Возвращает:** `IEnumerable<T>` - отложенное выполнение

### TakeWhile / SkipWhile

```csharp
// Берёт элементы, пока условие истинно
var takeWhile = students.TakeWhile(s => s.Age < 20);

// Пропускает элементы, пока условие истинно
var skipWhile = students.SkipWhile(s => s.Age < 18);
```

**Возвращает:** `IEnumerable<T>` - отложенное выполнение

---

## 8. Операторы преобразования

### ToList / ToArray

```csharp
// Преобразует в List
var list = students.Where(s => s.Age >= 18).ToList();

// Преобразует в Array
var array = students.Where(s => s.Age >= 18).ToArray();
```

**Возвращает:** `List<T>` или `T[]` - немедленное выполнение

### ToDictionary

```csharp
// Создаёт Dictionary из коллекции
var dict = students.ToDictionary(s => s.Id, s => s.Name);

// Использование
string name = dict[1]; // "John"
```

**Возвращает:** `Dictionary<TKey, TValue>` - немедленное выполнение

### ToLookup

```csharp
// Создаёт Lookup (похож на Dictionary, но с несколькими значениями на ключ)
var lookup = students.ToLookup(s => s.Age);

// Использование
var studentsAge20 = lookup[20]; // Все студенты 20 лет
```

**Возвращает:** `ILookup<TKey, TElement>` - немедленное выполнение

---

## 9. Операторы агрегации

### Aggregate

```csharp
// Пользовательская операция агрегации
var numbers = new[] { 1, 2, 3, 4, 5 };

// Пользовательская сумма
int sum = numbers.Aggregate((acc, num) => acc + num); // 15

// С начальным значением
int product = numbers.Aggregate(1, (acc, num) => acc * num); // 120

// С преобразованием конечного результата
string result = numbers.Aggregate(
    "Числа: ",
    (acc, num) => acc + num + ", ",
    acc => acc.TrimEnd(',', ' ')
);
```

**Возвращает:** `TResult` - немедленное выполнение

---

## 10. Отложенное выполнение (Deferred Execution)

### Lazy Evaluation

```csharp
var students = new List<Student> { /* ... */ };

// Запрос ЕЩЁ НЕ выполнен!
var query = students.Where(s => s.Age >= 18);

// Запрос выполняется только при итерации
foreach (var student in query) {
    Console.WriteLine(student.Name);
}

// Или при вызове ToList/ToArray
var result = query.ToList(); // Запрос выполнен здесь!
```

### Немедленное выполнение

```csharp
// Эти операторы выполняются немедленно:
var count = students.Count(); // Выполнено сразу
var first = students.First(); // Выполнено сразу
var list = students.ToList(); // Выполнено сразу
```

### Диаграмма: Отложенное выполнение

```
┌─────────────────────────────────────────────┐
│  var query = students.Where(...)           │
│  (Выполнение ещё не началось)               │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Запрос сохранён      │
        │  (lazy)               │
        └───────────────────────┘
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│ foreach   │ │ ToList()  │ │ Count()   │
│ (выполняет)│ │ (выполняет)│ │ (выполняет)│
└───────────┘ └───────────┘ └───────────┘
```

---

## 11. Все методы LINQ и их возвращаемые значения

### Методы фильтрации

| Метод | Возвращает | Выполнение | Описание |
|-------|------------|------------|----------|
| `Where<T>(predicate)` | `IEnumerable<T>` | Отложенное | Фильтрует элементы по условию |
| `OfType<TResult>()` | `IEnumerable<TResult>` | Отложенное | Фильтрует элементы определённого типа |
| `Cast<TResult>()` | `IEnumerable<TResult>` | Отложенное | Приводит все элементы к указанному типу |

### Методы проекции

| Метод | Возвращает | Выполнение | Описание |
|-------|------------|------------|----------|
| `Select<T, TResult>(selector)` | `IEnumerable<TResult>` | Отложенное | Преобразует каждый элемент |
| `SelectMany<T, TResult>(selector)` | `IEnumerable<TResult>` | Отложенное | Преобразует и разворачивает вложенные коллекции |

### Методы сортировки

| Метод | Возвращает | Выполнение | Описание |
|-------|------------|------------|----------|
| `OrderBy<T, TKey>(keySelector)` | `IOrderedEnumerable<T>` | Отложенное | Сортирует по возрастанию |
| `OrderByDescending<T, TKey>(keySelector)` | `IOrderedEnumerable<T>` | Отложенное | Сортирует по убыванию |
| `ThenBy<T, TKey>(keySelector)` | `IOrderedEnumerable<T>` | Отложенное | Дополнительная сортировка по возрастанию |
| `ThenByDescending<T, TKey>(keySelector)` | `IOrderedEnumerable<T>` | Отложенное | Дополнительная сортировка по убыванию |
| `Reverse<T>()` | `IEnumerable<T>` | Отложенное | Обращает порядок элементов |

### Методы получения элементов

| Метод | Возвращает | Выполнение | Описание |
|-------|------------|------------|----------|
| `First<T>()` | `T` | Немедленное | Первый элемент (исключение, если пусто) |
| `First<T>(predicate)` | `T` | Немедленное | Первый элемент, удовлетворяющий условию |
| `FirstOrDefault<T>()` | `T?` | Немедленное | Первый элемент или default(T) |
| `FirstOrDefault<T>(predicate)` | `T?` | Немедленное | Первый элемент, удовлетворяющий условию, или default |
| `Last<T>()` | `T` | Немедленное | Последний элемент |
| `Last<T>(predicate)` | `T` | Немедленное | Последний элемент, удовлетворяющий условию |
| `LastOrDefault<T>()` | `T?` | Немедленное | Последний элемент или default(T) |
| `LastOrDefault<T>(predicate)` | `T?` | Немедленное | Последний элемент, удовлетворяющий условию, или default |
| `Single<T>()` | `T` | Немедленное | Один элемент (исключение, если 0 или >1) |
| `Single<T>(predicate)` | `T` | Немедленное | Один элемент, удовлетворяющий условию |
| `SingleOrDefault<T>()` | `T?` | Немедленное | Один элемент или default(T) |
| `SingleOrDefault<T>(predicate)` | `T?` | Немедленное | Один элемент, удовлетворяющий условию, или default |
| `ElementAt<T>(index)` | `T` | Немедленное | Элемент по индексу |
| `ElementAtOrDefault<T>(index)` | `T?` | Немедленное | Элемент по индексу или default |

### Методы проверки условий

| Метод | Возвращает | Выполнение | Описание |
|-------|------------|------------|----------|
| `Any<T>()` | `bool` | Немедленное | Есть ли хотя бы один элемент |
| `Any<T>(predicate)` | `bool` | Немедленное | Есть ли хотя бы один элемент, удовлетворяющий условию |
| `All<T>(predicate)` | `bool` | Немедленное | Все ли элементы удовлетворяют условию |
| `Contains<T>(item)` | `bool` | Немедленное | Содержит ли коллекция элемент |
| `Contains<T>(item, comparer)` | `bool` | Немедленное | Содержит ли коллекция элемент (с компаратором) |

### Методы агрегации

| Метод | Возвращает | Выполнение | Описание |
|-------|------------|------------|----------|
| `Count<T>()` | `int` | Немедленное | Количество элементов |
| `Count<T>(predicate)` | `int` | Немедленное | Количество элементов, удовлетворяющих условию |
| `LongCount<T>()` | `long` | Немедленное | Количество элементов (long) |
| `Sum<T>(selector)` | `TResult` (числовой) | Немедленное | Сумма элементов |
| `Average<T>(selector)` | `double` | Немедленное | Среднее значение |
| `Min<T>()` | `T` | Немедленное | Минимальное значение |
| `Min<T>(selector)` | `TResult` | Немедленное | Минимальное значение по селектору |
| `Max<T>()` | `T` | Немедленное | Максимальное значение |
| `Max<T>(selector)` | `TResult` | Немедленное | Максимальное значение по селектору |
| `Aggregate<T>(func)` | `T` | Немедленное | Агрегация с пользовательской функцией |
| `Aggregate<T, TAccumulate>(seed, func)` | `TAccumulate` | Немедленное | Агрегация с начальным значением |

### Методы разбиения

| Метод | Возвращает | Выполнение | Описание |
|-------|------------|------------|----------|
| `Take<T>(count)` | `IEnumerable<T>` | Отложенное | Берёт первые N элементов |
| `TakeWhile<T>(predicate)` | `IEnumerable<T>` | Отложенное | Берёт элементы, пока условие истинно |
| `Skip<T>(count)` | `IEnumerable<T>` | Отложенное | Пропускает первые N элементов |
| `SkipWhile<T>(predicate)` | `IEnumerable<T>` | Отложенное | Пропускает элементы, пока условие истинно |

### Методы множеств

| Метод | Возвращает | Выполнение | Описание |
|-------|------------|------------|----------|
| `Distinct<T>()` | `IEnumerable<T>` | Отложенное | Уникальные элементы |
| `Distinct<T>(comparer)` | `IEnumerable<T>` | Отложенное | Уникальные элементы (с компаратором) |
| `Union<T>(second)` | `IEnumerable<T>` | Отложенное | Объединение двух последовательностей |
| `Intersect<T>(second)` | `IEnumerable<T>` | Отложенное | Пересечение двух последовательностей |
| `Except<T>(second)` | `IEnumerable<T>` | Отложенное | Разность двух последовательностей |

### Методы группировки

| Метод | Возвращает | Выполнение | Описание |
|-------|------------|------------|----------|
| `GroupBy<T, TKey>(keySelector)` | `IEnumerable<IGrouping<TKey, T>>` | Отложенное | Группирует по ключу |
| `GroupBy<T, TKey, TElement>(keySelector, elementSelector)` | `IEnumerable<IGrouping<TKey, TElement>>` | Отложенное | Группирует с проекцией элементов |
| `GroupBy<T, TKey, TResult>(keySelector, resultSelector)` | `IEnumerable<TResult>` | Отложенное | Группирует с преобразованием результата |

### Методы соединения

| Метод | Возвращает | Выполнение | Описание |
|-------|------------|------------|----------|
| `Join<TOuter, TInner, TKey, TResult>(inner, outerKeySelector, innerKeySelector, resultSelector)` | `IEnumerable<TResult>` | Отложенное | Inner Join двух последовательностей |
| `GroupJoin<TOuter, TInner, TKey, TResult>(inner, outerKeySelector, innerKeySelector, resultSelector)` | `IEnumerable<TResult>` | Отложенное | Групповое соединение (Left Join) |

### Методы преобразования

| Метод | Возвращает | Выполнение | Описание |
|-------|------------|------------|----------|
| `ToList<T>()` | `List<T>` | Немедленное | Преобразует в List |
| `ToArray<T>()` | `T[]` | Немедленное | Преобразует в Array |
| `ToDictionary<T, TKey>(keySelector)` | `Dictionary<TKey, T>` | Немедленное | Преобразует в Dictionary |
| `ToDictionary<T, TKey, TValue>(keySelector, elementSelector)` | `Dictionary<TKey, TValue>` | Немедленное | Преобразует в Dictionary с проекцией |
| `ToLookup<T, TKey>(keySelector)` | `ILookup<TKey, T>` | Немедленное | Преобразует в Lookup |
| `ToHashSet<T>()` | `HashSet<T>` | Немедленное | Преобразует в HashSet |
| `AsEnumerable<T>()` | `IEnumerable<T>` | Отложенное | Возвращает как IEnumerable |
| `AsQueryable<T>()` | `IQueryable<T>` | Отложенное | Преобразует в IQueryable |

### Примеры использования всех методов

```csharp
var numbers = new[] { 1, 2, 3, 4, 5, 2, 3 };
var students = new List<Student> { /* ... */ };

// Фильтрация
var evens = numbers.Where(n => n % 2 == 0); // IEnumerable<int>
var strings = new object[] { 1, "hello", 2, "world" };
var onlyStrings = strings.OfType<string>(); // IEnumerable<string>

// Проекция
var names = students.Select(s => s.Name); // IEnumerable<string>
var nested = new[] { new[] { 1, 2 }, new[] { 3, 4 } };
var flattened = nested.SelectMany(arr => arr); // IEnumerable<int> { 1, 2, 3, 4 }

// Сортировка
var sorted = students.OrderBy(s => s.Age); // IOrderedEnumerable<Student>
var multiSorted = students
    .OrderBy(s => s.Age)
    .ThenByDescending(s => s.Grade); // IOrderedEnumerable<Student>

// Получение элементов
var first = students.First(); // Student
var firstAdult = students.FirstOrDefault(s => s.Age >= 18); // Student?
var third = students.ElementAt(2); // Student

// Проверка условий
var hasAny = students.Any(); // bool
var hasAdults = students.Any(s => s.Age >= 18); // bool
var allAdults = students.All(s => s.Age >= 18); // bool

// Агрегация
var count = students.Count(); // int
var sum = numbers.Sum(); // int
var avg = students.Average(s => s.Age); // double
var min = students.Min(s => s.Grade); // int
var max = students.Max(s => s.Grade); // int

// Разбиение
var first3 = students.Take(3); // IEnumerable<Student>
var skip2 = students.Skip(2); // IEnumerable<Student>
var whileYoung = students.TakeWhile(s => s.Age < 20); // IEnumerable<Student>

// Множества
var unique = numbers.Distinct(); // IEnumerable<int>
var set1 = new[] { 1, 2, 3 };
var set2 = new[] { 3, 4, 5 };
var union = set1.Union(set2); // IEnumerable<int> { 1, 2, 3, 4, 5 }
var intersect = set1.Intersect(set2); // IEnumerable<int> { 3 }
var except = set1.Except(set2); // IEnumerable<int> { 1, 2 }

// Группировка
var byAge = students.GroupBy(s => s.Age); // IEnumerable<IGrouping<int, Student>>
var byAgeNames = students.GroupBy(s => s.Age, s => s.Name); // IEnumerable<IGrouping<int, string>>

// Соединение
var courses = new List<Course> { /* ... */ };
var joined = students.Join(
    courses,
    s => s.CourseId,
    c => c.Id,
    (s, c) => new { s.Name, c.Name }
); // IEnumerable<anonymous type>

// Преобразование
var list = students.ToList(); // List<Student>
var array = students.ToArray(); // Student[]
var dict = students.ToDictionary(s => s.Id, s => s.Name); // Dictionary<int, string>
var lookup = students.ToLookup(s => s.Age); // ILookup<int, Student>
var hashSet = numbers.ToHashSet(); // HashSet<int>
```

---

## 12. Практические примеры

### Пример 1: Фильтрация и сортировка

```csharp
var result = students
    .Where(s => s.Age >= 18 && s.Grade >= 80)
    .OrderByDescending(s => s.Grade)
    .ThenBy(s => s.Name)
    .Select(s => new {
        s.Name,
        s.Grade,
        Status = s.Grade >= 90 ? "Отлично" : "Хорошо"
    })
    .ToList();
```

### Пример 2: Группировка и агрегация

```csharp
var statistics = students
    .GroupBy(s => s.Age)
    .Select(g => new {
        Age = g.Key,
        Count = g.Count(),
        AverageGrade = g.Average(s => s.Grade),
        TopStudent = g.OrderByDescending(s => s.Grade).First().Name
    })
    .OrderBy(s => s.Age)
    .ToList();
```

### Пример 3: Сложное соединение

```csharp
var studentCourseInfo = from s in students
                        join c in courses on s.CourseId equals c.Id
                        join e in enrollments on s.Id equals e.StudentId
                        where e.EnrollmentDate.Year == 2024
                        select new {
                            StudentName = s.Name,
                            CourseName = c.Name,
                            EnrollmentDate = e.EnrollmentDate
                        };
```

### Пример 4: Сложные операции

```csharp
// Находит студентов с оценками выше среднего
var averageGrade = students.Average(s => s.Grade);
var aboveAverage = students
    .Where(s => s.Grade > averageGrade)
    .OrderByDescending(s => s.Grade)
    .ToList();

// Топ-3 студента для каждого возраста
var top3ByAge = students
    .GroupBy(s => s.Age)
    .SelectMany(g => g.OrderByDescending(s => s.Grade).Take(3))
    .ToList();
```

---

## 13. LINQ с Entity Framework

### Запросы к базе данных

```csharp
// LINQ to Entities (Entity Framework)
using (var context = new SchoolContext()) {
    // Запрос выполняется в базе данных
    var students = context.Students
        .Where(s => s.Age >= 18)
        .OrderBy(s => s.Name)
        .ToList();
    
    // Запрос с join
    var result = from s in context.Students
                 join c in context.Courses on s.CourseId equals c.Id
                 select new { s.Name, c.Name };
}
```

### Отложенное выполнение в EF

```csharp
// Запрос ЕЩЁ НЕ выполнен
var query = context.Students.Where(s => s.Age >= 18);

// Запрос выполняется при вызове ToList/First/etc
var result = query.ToList(); // SQL выполнен здесь!
```

---

## 14. Производительность и Best Practices

### ✅ Что делать

1. **Используйте ToList/ToArray только когда необходимо**
   ```csharp
   // ✅ OK - Отложенное выполнение
   var query = students.Where(s => s.Age >= 18);
   
   // ✅ OK - Если нужен List
   var list = students.Where(s => s.Age >= 18).ToList();
   ```

2. **Используйте FirstOrDefault вместо Where().First()**
   ```csharp
   // ✅ ПРАВИЛЬНО
   var student = students.FirstOrDefault(s => s.Age >= 18);
   
   // ⚠️ МЕНЕЕ ЭФФЕКТИВНО
   var student = students.Where(s => s.Age >= 18).FirstOrDefault();
   ```

3. **Фильтруйте перед сортировкой**
   ```csharp
   // ✅ ПРАВИЛЬНО
   var result = students
       .Where(s => s.Age >= 18)  // Фильтруем сначала
       .OrderBy(s => s.Name);     // Потом сортируем
   ```

### ❌ Чего избегать

1. **Не итерируйте несколько раз по LINQ-запросу**
   ```csharp
   // ❌ НЕПРАВИЛЬНО - Запрос выполняется дважды
   var query = students.Where(s => s.Age >= 18);
   var count = query.Count();
   var list = query.ToList();
   
   // ✅ ПРАВИЛЬНО - Сохраните результат
   var list = students.Where(s => s.Age >= 18).ToList();
   var count = list.Count;
   ```

2. **Не используйте ToList() без необходимости**
   ```csharp
   // ❌ НЕПРАВИЛЬНО
   var result = students.ToList().Where(s => s.Age >= 18);
   
   // ✅ ПРАВИЛЬНО
   var result = students.Where(s => s.Age >= 18);
   ```

---

## 15. Часто задаваемые вопросы (FAQ)

### Q: В чём разница между First() и FirstOrDefault()?
**A:** `First()` выбрасывает исключение, если коллекция пуста. `FirstOrDefault()` возвращает значение по умолчанию (null для reference types).

### Q: Когда выполняется LINQ-запрос?
**A:** При итерации (foreach) или при вызове операторов немедленного выполнения (ToList, ToArray, Count, First и т.д.).

### Q: В чём разница между Select и SelectMany?
**A:** `Select` проецирует каждый элемент. `SelectMany` разворачивает вложенные коллекции.

### Q: LINQ медленнее традиционных циклов?
**A:** Для простых операций может быть немного медленнее, но разница минимальна, а читаемость компенсирует это.

### Q: В чём разница между ToDictionary и ToLookup?
**A:** `ToDictionary` создаёт словарь (один ключ → одно значение). `ToLookup` создаёт lookup (один ключ → несколько значений).

### Q: Когда использовать GroupBy vs ToLookup?
**A:** `GroupBy` возвращает отложенную последовательность. `ToLookup` выполняет группировку немедленно и создаёт структуру для быстрого доступа по ключу.

---

## Заключение

LINQ — это мощный инструмент, который:
- ✅ Улучшает читаемость кода
- ✅ Обеспечивает type-safety
- ✅ Унифицирует синтаксис для разных источников данных
- ✅ Поддерживает отложенное выполнение для производительности

Используйте LINQ для написания более декларативного и поддерживаемого кода!

---

*Документ создан для объяснения LINQ в C# с практическими примерами, всеми методами LINQ и их возвращаемыми значениями, а также best practices.*

