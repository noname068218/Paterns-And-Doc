# Boxing и Unboxing в C#

## Введение

**Boxing** и **Unboxing** — это механизмы преобразования между value types (типами значений) и reference types (ссылочными типами) в C#. Понимание этих концепций критически важно для написания эффективного кода и избежания проблем с производительностью.

---

## 1. Что такое Boxing?

**Boxing** — это процесс преобразования value type в object (или любой другой reference type). При boxing создается объект-обертка в heap, который содержит копию value type.

### Простой пример Boxing

```csharp
using System;

// Value type
int number = 42;

// Boxing: int (value type) преобразуется в object (reference type)
object boxedNumber = number; // Boxing происходит здесь!

// Что происходит под капотом:
// 1. Создается объект в heap
// 2. Значение 42 копируется в этот объект
// 3. boxedNumber хранит ссылку на этот объект
```

### Визуализация Boxing

```
До Boxing:
┌─────────────┐
│   Stack     │
│  number = 42│  (value type, хранится в stack)
└─────────────┘

После Boxing:
┌─────────────┐         ┌──────────────┐
│   Stack     │         │    Heap      │
│ boxedNumber │────────▶│  ┌────────┐  │
│   (ref)     │         │  │  42    │  │
└─────────────┘         │  └────────┘  │
                        └──────────────┘
                        (object-обертка в heap)
```

### Примеры Boxing

```csharp
using System;
using System.Collections;

class Program
{
    static void Main()
    {
        // Пример 1: Прямое присваивание
        int value = 100;
        object boxed = value; // Boxing
        
        // Пример 2: Передача value type как object параметра
        int number = 42;
        PrintObject(number); // Boxing происходит при передаче
        
        // Пример 3: ArrayList и старые коллекции (до generics)
        ArrayList list = new ArrayList();
        list.Add(10);    // Boxing! int → object
        list.Add(20.5);  // Boxing! double → object
        list.Add(true);  // Boxing! bool → object
        
        // Пример 4: Вызов методов через object
        int x = 5;
        object obj = x; // Boxing
        Console.WriteLine(obj.ToString()); // Метод вызывается на boxed объекте
    }
    
    static void PrintObject(object obj)
    {
        Console.WriteLine(obj);
    }
}
```

---

## 2. Что такое Unboxing?

**Unboxing** — это процесс преобразования object обратно в value type. При unboxing значение извлекается из объекта-обертки и копируется в value type переменную.

### Простой пример Unboxing

```csharp
using System;

// Boxing
int originalValue = 42;
object boxed = originalValue; // Boxing

// Unboxing: object преобразуется обратно в int
int unboxedValue = (int)boxed; // Unboxing происходит здесь!

// Важно: Unboxing требует явного приведения типа
// int unboxedValue = boxed; // ❌ Ошибка компиляции!
```

### Визуализация Unboxing

```
Unboxing процесс:
┌─────────────┐         ┌──────────────┐
│   Stack     │         │    Heap      │
│   boxed     │────────▶│  ┌────────┐  │
│   (ref)     │         │  │  42    │  │
│             │         │  └────────┘  │
│ unboxedValue│◀────────┼──────────────┘
│   = 42      │         (копирование значения
│ (value type)│          из heap в stack)
└─────────────┘
```

### Примеры Unboxing

```csharp
using System;
using System.Collections;

class Program
{
    static void Main()
    {
        // Пример 1: Базовый unboxing
        int value = 100;
        object boxed = value;        // Boxing
        int unboxed = (int)boxed;    // Unboxing
        
        Console.WriteLine($"Original: {value}, Unboxed: {unboxed}");
        
        // Пример 2: Unboxing из ArrayList
        ArrayList list = new ArrayList();
        list.Add(42);           // Boxing
        list.Add(3.14);         // Boxing
        
        int intValue = (int)list[0];       // Unboxing
        double doubleValue = (double)list[1]; // Unboxing
        
        // Пример 3: Неправильный unboxing (InvalidCastException)
        object boxedInt = 42;
        // double wrong = (double)boxedInt; // ❌ Runtime error!
        
        // Правильный способ: сначала unbox в правильный тип
        int temp = (int)boxedInt;
        double correct = temp; // Implicit conversion
        
        // Или напрямую:
        double correct2 = (int)boxedInt; // Unbox to int, then convert
    }
}
```

---

## 3. Стоимость Boxing и Unboxing

Boxing и Unboxing имеют значительные затраты производительности:

### Проблемы производительности

```csharp
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        const int iterations = 10_000_000;
        
        // Тест 1: Boxing/Unboxing (медленно)
        var sw1 = Stopwatch.StartNew();
        ArrayList arrayList = new ArrayList();
        for (int i = 0; i < iterations; i++)
        {
            arrayList.Add(i);        // Boxing при каждом Add
            int value = (int)arrayList[i]; // Unboxing при каждом обращении
        }
        sw1.Stop();
        Console.WriteLine($"Boxing/Unboxing: {sw1.ElapsedMilliseconds} ms");
        
        // Тест 2: Generics (быстро)
        var sw2 = Stopwatch.StartNew();
        List<int> genericList = new List<int>();
        for (int i = 0; i < iterations; i++)
        {
            genericList.Add(i);      // Нет boxing!
            int value = genericList[i]; // Нет unboxing!
        }
        sw2.Stop();
        Console.WriteLine($"Generics: {sw2.ElapsedMilliseconds} ms");
        
        // Результат: Generics намного быстрее (примерно в 5-10 раз)
    }
}
```

### Почему Boxing/Unboxing медленные?

```csharp
// При каждом boxing:
// 1. Выделение памяти в heap (дорогая операция)
// 2. Копирование значения из stack в heap
// 3. Создание объекта-обертки
// 4. Управление этим объектом GC (дополнительная нагрузка)

// При каждом unboxing:
// 1. Проверка типа (может быть исключение)
// 2. Доступ к объекту в heap
// 3. Копирование значения обратно в stack
// 4. Cast операция
```

---

## 4. Где происходит Boxing и Unboxing?

### Неявный Boxing

```csharp
using System;

class Program
{
    static void Main()
    {
        // 1. Присваивание value type переменной типа object
        int x = 42;
        object obj = x; // Boxing
        
        // 2. Передача value type как параметра типа object
        PrintValue(100); // Boxing
        
        // 3. Вызов виртуальных методов на value type
        int number = 5;
        string str = number.ToString(); // Не boxing! ToString() не требует object
        object obj2 = number; // Boxing
        string str2 = obj2.ToString(); // Вызов на boxed объекте
        
        // 4. Использование value type в non-generic коллекциях
        System.Collections.ArrayList list = new System.Collections.ArrayList();
        list.Add(10); // Boxing
        list.Add(20.5); // Boxing
    }
    
    static void PrintValue(object value) // Принимает object
    {
        Console.WriteLine(value);
    }
}
```

### Неявный Unboxing

```csharp
using System;
using System.Collections;

class Program
{
    static void Main()
    {
        // 1. Извлечение из non-generic коллекций
        ArrayList list = new ArrayList();
        list.Add(42); // Boxing
        int value = (int)list[0]; // Unboxing (явный cast требуется)
        
        // 2. Приведение object к value type
        object boxed = 100;
        int unboxed = (int)boxed; // Unboxing
        
        // 3. Использование в выражениях
        object obj1 = 10;
        object obj2 = 20;
        // int sum = (int)obj1 + (int)obj2; // Unboxing обоих, затем сложение
    }
}
```

### Когда Boxing НЕ происходит

```csharp
using System;

class Program
{
    static void Main()
    {
        int value = 42;
        
        // 1. Вызов методов, определенных на самом value type
        string str = value.ToString(); // НЕ boxing! Метод определен в int
        int hashCode = value.GetHashCode(); // НЕ boxing!
        Type type = value.GetType(); // НЕ boxing! GetType() не виртуальный
        
        // 2. Использование generics
        List<int> numbers = new List<int>();
        numbers.Add(42); // НЕ boxing! Generic коллекция
        int x = numbers[0]; // НЕ unboxing!
        
        // 3. Вызов интерфейсных методов через интерфейс, реализованный value type
        IComparable<int> comparable = value; // НЕ boxing в .NET 2.0+ (constrained call)
        int result = comparable.CompareTo(100); // НЕ boxing!
    }
}
```

---

## 5. Проблемы с Boxing/Unboxing

### Проблема 1: InvalidCastException

```csharp
using System;

class Program
{
    static void Main()
    {
        // Boxing int
        int value = 42;
        object boxed = value; // Boxing как int
        
        try
        {
            // Попытка unboxing в другой тип
            double wrong = (double)boxed; // ❌ InvalidCastException!
        }
        catch (InvalidCastException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        
        // Правильный способ
        int correct = (int)boxed; // Сначала unbox в правильный тип
        double converted = correct; // Затем конвертация
    }
}
```

### Проблема 2: Потеря производительности

```csharp
using System;
using System.Text;

class Program
{
    static void BadExample()
    {
        // ❌ Плохо: множественный boxing в строковой интерполяции (старые версии)
        int x = 10;
        int y = 20;
        // В старых версиях C#: string result = $"{x} + {y} = {x + y}"; // 3 boxing операции
        // В C# 10+: оптимизировано, boxing не происходит для value types
    }
    
    static void GoodExample()
    {
        // ✅ Хорошо: использование generics
        var numbers = new System.Collections.Generic.List<int>();
        for (int i = 0; i < 1000; i++)
        {
            numbers.Add(i); // Нет boxing!
        }
    }
}
```

### Проблема 3: Изменение значения

```csharp
using System;

struct Point
{
    public int X;
    public int Y;
    
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}

class Program
{
    static void Main()
    {
        Point point = new Point(10, 20);
        object boxed = point; // Boxing: копия point попадает в heap
        
        // Изменение оригинального value type
        point.X = 100;
        
        // Unboxing вернет исходное значение (до изменения)
        Point unboxed = (Point)boxed;
        Console.WriteLine($"Original: X={point.X}, Y={point.Y}"); // X=100, Y=20
        Console.WriteLine($"Unboxed: X={unboxed.X}, Y={unboxed.Y}"); // X=10, Y=20
        
        // Важно: изменения в boxed объекте невозможны (immutable после boxing)
        // Point p = (Point)boxed;
        // p.X = 200; // Это изменяет только локальную копию, не boxed объект!
    }
}
```

---

## 6. Как избежать Boxing/Unboxing?

### Использование Generics

```csharp
using System.Collections.Generic;

// ❌ Плохо: non-generic коллекции (boxing/unboxing)
System.Collections.ArrayList badList = new System.Collections.ArrayList();
badList.Add(42);           // Boxing
int value = (int)badList[0]; // Unboxing

// ✅ Хорошо: generic коллекции (нет boxing/unboxing)
List<int> goodList = new List<int>();
goodList.Add(42);    // Нет boxing!
int value2 = goodList[0]; // Нет unboxing!
```

### Использование конкретных типов вместо object

```csharp
// ❌ Плохо: метод принимает object
void ProcessValue(object value)
{
    int intValue = (int)value; // Unboxing
    // обработка
}

// ✅ Хорошо: generic метод
void ProcessValue<T>(T value) where T : struct
{
    // Обработка без boxing/unboxing
    // T остается value type
}

// ✅ Или перегрузка для конкретных типов
void ProcessValue(int value)
{
    // Обработка без boxing
}
```

### Использование интерфейсов с constraint

```csharp
using System;

// В .NET 2.0+ constrained call предотвращает boxing
public void CompareValues<T>(T value1, T value2) where T : IComparable<T>
{
    int result = value1.CompareTo(value2); // Нет boxing!
    // Compiler генерирует constrained call
}

// Использование
int x = 10;
int y = 20;
CompareValues(x, y); // Нет boxing! Constrained call оптимизирует это
```

### Избежание виртуальных методов на value types

```csharp
// Когда возможно, используйте методы напрямую на value type
int value = 42;

// ✅ Хорошо: прямой вызов метода
string str = value.ToString(); // Нет boxing, метод определен в int

// ❌ Избегайте: через object (хотя в некоторых случаях компилятор оптимизирует)
object obj = value;
string str2 = obj.ToString(); // Boxing произошел при присваивании
```

---

## 7. Constrained Execution Regions (CER)

В .NET есть механизм constrained calls, который предотвращает boxing для вызова методов интерфейсов на value types.

```csharp
using System;

// Generic метод с constraint предотвращает boxing
public static int Compare<T>(T x, T y) where T : IComparable<T>
{
    return x.CompareTo(y); // Constrained call - нет boxing!
}

// Использование
int a = 10;
int b = 20;
int result = Compare(a, b); // Нет boxing! Compiler оптимизирует вызов
```

### Как это работает

```csharp
// Без constraint (старый способ):
public static int CompareWithoutConstraint(IComparable x, IComparable y)
{
    return x.CompareTo(y); // Может потребовать boxing для value types
}

// С constraint (современный способ):
public static int CompareWithConstraint<T>(T x, T y) where T : IComparable<T>
{
    // Compiler генерирует IL-код с constrained prefix
    // Это позволяет вызывать метод напрямую на value type без boxing
    return x.CompareTo(y);
}

// Использование
int x = 10, y = 20;
CompareWithConstraint(x, y); // Нет boxing благодаря constrained call
```

---

## 8. Практические примеры

### Пример 1: Сравнение производительности

```csharp
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

class PerformanceComparison
{
    static void Main()
    {
        const int count = 10_000_000;
        
        // Test 1: ArrayList (boxing/unboxing)
        var sw1 = Stopwatch.StartNew();
        ArrayList arrayList = new ArrayList();
        for (int i = 0; i < count; i++)
        {
            arrayList.Add(i);
        }
        long sum1 = 0;
        for (int i = 0; i < count; i++)
        {
            sum1 += (int)arrayList[i]; // Unboxing
        }
        sw1.Stop();
        Console.WriteLine($"ArrayList: {sw1.ElapsedMilliseconds}ms, Sum: {sum1}");
        
        // Test 2: List<int> (generics, нет boxing)
        var sw2 = Stopwatch.StartNew();
        List<int> genericList = new List<int>();
        for (int i = 0; i < count; i++)
        {
            genericList.Add(i); // Нет boxing
        }
        long sum2 = 0;
        for (int i = 0; i < count; i++)
        {
            sum2 += genericList[i]; // Нет unboxing
        }
        sw2.Stop();
        Console.WriteLine($"List<int>: {sw2.ElapsedMilliseconds}ms, Sum: {sum2}");
        
        // Результат: List<int> примерно в 5-10 раз быстрее
    }
}
```

### Пример 2: Правильное использование интерфейсов

```csharp
using System;

// ❌ Плохо: метод принимает object
public class BadCalculator
{
    public static int Add(object a, object b)
    {
        return (int)a + (int)b; // Два unboxing
    }
}

// ✅ Хорошо: generic метод с constraint
public class GoodCalculator
{
    public static T Add<T>(T a, T b) where T : struct
    {
        // Для чисел нужно использовать более сложную логику
        // Но это предотвращает boxing/unboxing
        dynamic da = a;
        dynamic db = b;
        return da + db; // Использует dynamic dispatch, но без boxing
    }
    
    // Лучше: перегрузки для конкретных типов
    public static int Add(int a, int b) => a + b;
    public static double Add(double a, double b) => a + b;
    public static long Add(long a, long b) => a + b;
}
```

---

## 9. Best Practices

### ✅ Что делать

1. **Используйте generic коллекции вместо non-generic**
   ```csharp
   // ✅ Хорошо
   List<int> numbers = new List<int>();
   Dictionary<string, int> dict = new Dictionary<string, int>();
   ```

2. **Используйте generic методы с constraints**
   ```csharp
   // ✅ Хорошо
   public void Process<T>(T value) where T : IComparable<T>
   {
       // Constrained call предотвращает boxing
   }
   ```

3. **Используйте конкретные типы когда возможно**
   ```csharp
   // ✅ Хорошо
   void Method(int value) { }
   void Method(double value) { }
   ```

4. **Избегайте object когда тип известен**
   ```csharp
   // ✅ Хорошо
   void ProcessInt(int value) { }
   
   // ❌ Плохо
   void ProcessObject(object value) { }
   ```

### ❌ Чего избегать

1. **Не используйте ArrayList и другие non-generic коллекции**
   ```csharp
   // ❌ Плохо
   ArrayList list = new ArrayList();
   list.Add(42); // Boxing
   ```

2. **Не передавайте value types как object без необходимости**
   ```csharp
   // ❌ Плохо
   void Method(object value) { }
   Method(42); // Boxing
   
   // ✅ Хорошо
   void Method(int value) { }
   Method(42); // Нет boxing
   ```

3. **Не используйте string interpolation со старыми компиляторами**
   ```csharp
   // В старых версиях C# (< 10):
   // int x = 10;
   // string s = $"{x}"; // Boxing
   
   // В C# 10+: оптимизировано, boxing не происходит
   ```

4. **Не делайте лишних unboxing операций**
   ```csharp
   // ❌ Плохо
   object obj = 42;
   int x = (int)obj; // Unboxing
   int y = (int)obj; // Еще один unboxing (тот же объект)
   
   // ✅ Хорошо
   object obj = 42;
   int x = (int)obj; // Unboxing один раз
   int y = x; // Копирование значения
   ```

---

## 10. Вопросы для собеседований

### Типичные вопросы и ответы

**Q1: Что такое Boxing и Unboxing?**
- Boxing: преобразование value type в reference type (object)
- Unboxing: преобразование object обратно в value type
- Boxing создает объект-обертку в heap, unboxing извлекает значение

**Q2: В чем разница между Boxing и Casting?**
- Boxing: value type → object (создание объекта в heap)
- Casting: изменение типа ссылки (например, object → string)
- Unboxing требует явного cast, но это разные операции

**Q3: Почему Boxing/Unboxing медленные?**
- Boxing: выделение памяти в heap, копирование значения, создание объекта
- Unboxing: проверка типа, доступ к heap, копирование обратно в stack
- Дополнительная нагрузка на GC

**Q4: Как избежать Boxing/Unboxing?**
- Использовать generic коллекции (List<T> вместо ArrayList)
- Использовать generic методы с constraints
- Использовать конкретные типы вместо object
- Использовать constrained calls для интерфейсных методов

**Q5: Происходит ли Boxing при вызове ToString() на int?**
- Нет! ToString() определен в самом типе int, boxing не требуется
- Boxing происходит только при присваивании value type переменной типа object

**Q6: Что такое Constrained Call?**
- Механизм в .NET, который позволяет вызывать методы интерфейсов на value types без boxing
- Используется в generic методах с constraint (where T : IComparable<T>)
- Compiler генерирует специальный IL-код с constrained prefix

**Q7: Что произойдет при неправильном Unboxing?**
- InvalidCastException во время выполнения
- Например: int boxed = 42; double wrong = (double)boxed; // Exception!

**Q8: Происходит ли Boxing в современном C# (10+) при интерполяции строк?**
- В большинстве случаев нет, компилятор оптимизирует
- Для value types используется специальная оптимизация
- Но лучше использовать generics для гарантии отсутствия boxing

---

## Заключение

Boxing и Unboxing - важные концепции C#, которые влияют на производительность:
- **Boxing** создает объекты в heap и замедляет выполнение
- **Unboxing** извлекает значения и требует проверки типов
- **Generics** - лучшее решение для избежания boxing/unboxing
- **Constrained calls** предотвращают boxing при вызове интерфейсных методов

Помните: **всегда предпочитайте generics над non-generic коллекциями и методами!**
