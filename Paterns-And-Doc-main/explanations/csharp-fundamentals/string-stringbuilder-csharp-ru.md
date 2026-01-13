# String vs StringBuilder в C#

## Введение

**String** и **StringBuilder** — это два способа работы со строками в C#. Понимание различий между ними критически важно для написания эффективного кода, особенно при работе с большими объемами текстовых данных.

---

## 1. Особенности String в C#

### String - Immutable (Неизменяемый)

**String** в C# является неизменяемым (immutable) типом. Это означает, что после создания строки её нельзя изменить.

```csharp
using System;

string str = "Hello";
str = "World"; // Это НЕ изменяет строку, а создает новую!

// Что происходит на самом деле:
// 1. Создается объект "Hello" в heap
// 2. str указывает на "Hello"
// 3. При присваивании "World" создается НОВЫЙ объект "World" в heap
// 4. str теперь указывает на "World"
// 5. Объект "Hello" остается в памяти до сборки мусора
```

### Визуализация Immutability

```
Изначально:
┌─────────────┐
│  str ──────▶│ "Hello" (heap)
└─────────────┘

После str = "World":
┌─────────────┐         ┌─────────────┐
│  str ──────▶│ "World" │ "Hello"     │ (ожидает GC)
└─────────────┘         └─────────────┘
```

### Проблема конкатенации строк

```csharp
using System;
using System.Diagnostics;

class Program
{
    static void BadExample()
    {
        // ❌ Плохо: множественная конкатенация строк
        string result = "";
        
        for (int i = 0; i < 10000; i++)
        {
            result += "a"; // Создается новая строка при каждой итерации!
        }
        
        // Что происходит:
        // Итерация 1: создается "a"
        // Итерация 2: создается "aa" (новая строка!)
        // Итерация 3: создается "aaa" (новая строка!)
        // ...
        // Итерация 10000: создается строка из 10000 символов
        // 
        // Итого: создано 10000 объектов строк в heap!
        // Старые строки остаются до GC - огромная трата памяти
    }
    
    static void MeasurePerformance()
    {
        const int iterations = 10000;
        
        // Измерение конкатенации
        var sw1 = Stopwatch.StartNew();
        string concatResult = "";
        for (int i = 0; i < iterations; i++)
        {
            concatResult += "a";
        }
        sw1.Stop();
        Console.WriteLine($"String concatenation: {sw1.ElapsedMilliseconds}ms");
        
        // Измерение StringBuilder
        var sw2 = Stopwatch.StartNew();
        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < iterations; i++)
        {
            sb.Append("a");
        }
        string sbResult = sb.ToString();
        sw2.Stop();
        Console.WriteLine($"StringBuilder: {sw2.ElapsedMilliseconds}ms");
        
        // Результат: StringBuilder значительно быстрее для множественных операций
    }
}
```

---

## 2. StringBuilder - Mutable (Изменяемый)

**StringBuilder** — это изменяемый класс для работы со строками. Он использует внутренний буфер и позволяет эффективно модифицировать строки без создания новых объектов.

### Основные особенности StringBuilder

```csharp
using System.Text;

// Создание StringBuilder
StringBuilder sb = new StringBuilder();

// Добавление строк
sb.Append("Hello");
sb.Append(" ");
sb.Append("World");

// Получение результата
string result = sb.ToString(); // "Hello World"

// StringBuilder использует внутренний буфер
// При необходимости буфер автоматически увеличивается
```

### Визуализация работы StringBuilder

```
StringBuilder внутренняя структура:
┌─────────────────────────────────────┐
│  StringBuilder                      │
│  ┌───────────────────────────────┐ │
│  │ Internal Buffer (char[])      │ │
│  │ [H][e][l][l][o][ ][W][o]...  │ │
│  │        ↑                      │ │
│  │    Position                   │ │
│  └───────────────────────────────┘ │
│  Capacity: 16 (по умолчанию)      │
│  Length: текущая длина            │
└─────────────────────────────────────┘

При Append() значение добавляется в буфер
Если буфер заполнен - создается новый больший буфер
Старый буфер копируется в новый
```

### Примеры использования StringBuilder

```csharp
using System.Text;

class Program
{
    static void Main()
    {
        // Пример 1: Базовая конкатенация
        var sb = new StringBuilder();
        sb.Append("Hello");
        sb.Append(" ");
        sb.Append("World");
        Console.WriteLine(sb.ToString()); // "Hello World"
        
        // Пример 2: Циклическое добавление
        var sb2 = new StringBuilder();
        for (int i = 0; i < 1000; i++)
        {
            sb2.Append(i.ToString());
            sb2.Append(", ");
        }
        string numbers = sb2.ToString();
        
        // Пример 3: Добавление строк с переносами
        var sb3 = new StringBuilder();
        sb3.AppendLine("First line");
        sb3.AppendLine("Second line");
        sb3.AppendLine("Third line");
        Console.WriteLine(sb3.ToString());
        
        // Пример 4: Замена и удаление
        var sb4 = new StringBuilder("Hello World");
        sb4.Replace("World", "C#");
        sb4.Remove(5, 1); // Удалить 1 символ начиная с индекса 5
        Console.WriteLine(sb4.ToString()); // "Hello C#"
        
        // Пример 5: Вставка
        var sb5 = new StringBuilder("Hello World");
        sb5.Insert(6, "Beautiful ");
        Console.WriteLine(sb5.ToString()); // "Hello Beautiful World"
    }
}
```

---

## 3. Сравнение производительности

### Тест 1: Множественная конкатенация

```csharp
using System;
using System.Diagnostics;
using System.Text;

class PerformanceTest
{
    static void Main()
    {
        const int iterations = 100000;
        
        // Test 1: String конкатенация
        var sw1 = Stopwatch.StartNew();
        string str = "";
        for (int i = 0; i < iterations; i++)
        {
            str += "a";
        }
        sw1.Stop();
        Console.WriteLine($"String concatenation ({iterations} iterations): {sw1.ElapsedMilliseconds}ms");
        Console.WriteLine($"Memory allocated: ~{iterations * iterations / 2} characters (estimated)");
        
        // Test 2: StringBuilder
        var sw2 = Stopwatch.StartNew();
        var sb = new StringBuilder();
        for (int i = 0; i < iterations; i++)
        {
            sb.Append("a");
        }
        string result = sb.ToString();
        sw2.Stop();
        Console.WriteLine($"StringBuilder ({iterations} iterations): {sw2.ElapsedMilliseconds}ms");
        Console.WriteLine($"Memory allocated: ~{result.Length} characters");
        
        // Результат: StringBuilder в десятки раз быстрее для больших объемов
    }
}
```

### Тест 2: Разные сценарии использования

```csharp
using System;
using System.Diagnostics;
using System.Text;

class ScenarioComparison
{
    static void Main()
    {
        // Сценарий 1: Малая конкатенация (1-5 операций)
        Console.WriteLine("=== Small concatenation (5 operations) ===");
        TestSmallConcatenation();
        
        // Сценарий 2: Средняя конкатенация (100 операций)
        Console.WriteLine("\n=== Medium concatenation (100 operations) ===");
        TestMediumConcatenation();
        
        // Сценарий 3: Большая конкатенация (10000 операций)
        Console.WriteLine("\n=== Large concatenation (10000 operations) ===");
        TestLargeConcatenation();
    }
    
    static void TestSmallConcatenation()
    {
        // String может быть быстрее для малых операций
        var sw1 = Stopwatch.StartNew();
        string str = "a" + "b" + "c" + "d" + "e";
        sw1.Stop();
        
        var sw2 = Stopwatch.StartNew();
        var sb = new StringBuilder();
        sb.Append("a").Append("b").Append("c").Append("d").Append("e");
        string result = sb.ToString();
        sw2.Stop();
        
        Console.WriteLine($"String: {sw1.ElapsedTicks} ticks");
        Console.WriteLine($"StringBuilder: {sw2.ElapsedTicks} ticks");
    }
    
    static void TestMediumConcatenation()
    {
        const int count = 100;
        
        var sw1 = Stopwatch.StartNew();
        string str = "";
        for (int i = 0; i < count; i++)
        {
            str += i.ToString();
        }
        sw1.Stop();
        
        var sw2 = Stopwatch.StartNew();
        var sb = new StringBuilder();
        for (int i = 0; i < count; i++)
        {
            sb.Append(i);
        }
        string result = sb.ToString();
        sw2.Stop();
        
        Console.WriteLine($"String: {sw1.ElapsedMilliseconds}ms");
        Console.WriteLine($"StringBuilder: {sw2.ElapsedMilliseconds}ms");
    }
    
    static void TestLargeConcatenation()
    {
        const int count = 10000;
        
        var sw1 = Stopwatch.StartNew();
        string str = "";
        for (int i = 0; i < count; i++)
        {
            str += i.ToString();
        }
        sw1.Stop();
        
        var sw2 = Stopwatch.StartNew();
        var sb = new StringBuilder();
        for (int i = 0; i < count; i++)
        {
            sb.Append(i);
        }
        string result = sb.ToString();
        sw2.Stop();
        
        Console.WriteLine($"String: {sw1.ElapsedMilliseconds}ms");
        Console.WriteLine($"StringBuilder: {sw2.ElapsedMilliseconds}ms");
        Console.WriteLine($"Speedup: {(double)sw1.ElapsedMilliseconds / sw2.ElapsedMilliseconds:F2}x");
    }
}
```

---

## 4. Capacity и оптимизация StringBuilder

### Capacity (Емкость) StringBuilder

```csharp
using System.Text;

class Program
{
    static void Main()
    {
        // По умолчанию Capacity = 16
        var sb1 = new StringBuilder();
        Console.WriteLine($"Default capacity: {sb1.Capacity}"); // 16
        
        // Указание начальной capacity
        var sb2 = new StringBuilder(capacity: 1000);
        Console.WriteLine($"Custom capacity: {sb2.Capacity}"); // 1000
        
        // Указание начальной строки и capacity
        var sb3 = new StringBuilder("Hello", capacity: 100);
        Console.WriteLine($"Initial string: {sb3.ToString()}, Capacity: {sb3.Capacity}");
        
        // Автоматическое увеличение capacity
        var sb4 = new StringBuilder(capacity: 16);
        for (int i = 0; i < 100; i++)
        {
            sb4.Append("a");
            if (i % 10 == 0)
            {
                Console.WriteLine($"Length: {sb4.Length}, Capacity: {sb4.Capacity}");
            }
        }
        
        // Capacity увеличивается автоматически при необходимости
        // Обычно удваивается: 16 → 32 → 64 → 128 ...
    }
}
```

### Оптимизация: Установка Capacity заранее

```csharp
using System.Text;
using System.Diagnostics;

class OptimizationExample
{
    static void Main()
    {
        const int expectedSize = 10000;
        
        // ❌ Плохо: StringBuilder с capacity по умолчанию
        var sw1 = Stopwatch.StartNew();
        var sb1 = new StringBuilder(); // Capacity = 16
        for (int i = 0; i < expectedSize; i++)
        {
            sb1.Append("a");
            // Capacity будет увеличиваться несколько раз: 16→32→64→128→...
        }
        sw1.Stop();
        Console.WriteLine($"Without pre-allocation: {sw1.ElapsedMilliseconds}ms");
        
        // ✅ Хорошо: Установка capacity заранее
        var sw2 = Stopwatch.StartNew();
        var sb2 = new StringBuilder(capacity: expectedSize);
        for (int i = 0; i < expectedSize; i++)
        {
            sb2.Append("a");
            // Capacity не увеличивается - уже достаточно места
        }
        sw2.Stop();
        Console.WriteLine($"With pre-allocation: {sw2.ElapsedMilliseconds}ms");
        
        // Результат: Предварительная установка capacity быстрее
    }
}
```

### Clear() и повторное использование

```csharp
using System.Text;

class ReuseExample
{
    private static StringBuilder _sharedBuilder = new StringBuilder(1000);
    
    static string ProcessItems(int[] items)
    {
        // Переиспользование StringBuilder
        _sharedBuilder.Clear(); // Очистка без создания нового объекта
        
        foreach (var item in items)
        {
            _sharedBuilder.Append(item);
            _sharedBuilder.Append(", ");
        }
        
        // Удаление последней запятой и пробела
        if (_sharedBuilder.Length > 0)
        {
            _sharedBuilder.Length -= 2;
        }
        
        return _sharedBuilder.ToString();
    }
    
    static void Main()
    {
        int[] items1 = { 1, 2, 3, 4, 5 };
        string result1 = ProcessItems(items1);
        Console.WriteLine(result1); // "1, 2, 3, 4, 5"
        
        int[] items2 = { 10, 20, 30 };
        string result2 = ProcessItems(items2);
        Console.WriteLine(result2); // "10, 20, 30"
    }
}
```

---

## 5. Когда использовать String, а когда StringBuilder?

### ✅ Используйте String когда:

1. **Малое количество операций (< 5-10 конкатенаций)**
   ```csharp
   // ✅ Хорошо для String
   string fullName = firstName + " " + lastName;
   string message = "Hello, " + name + "!";
   ```

2. **String interpolation (C# 6.0+)**
   ```csharp
   // ✅ String interpolation эффективен
   string message = $"Hello, {name}! You are {age} years old.";
   ```

3. **Неизменяемые строки (read-only операции)**
   ```csharp
   // ✅ String идеален для чтения
   string config = "appsettings.json";
   string path = Path.Combine(basePath, config);
   ```

4. **Постоянные строки (compile-time)**
   ```csharp
   // ✅ Компилятор оптимизирует
   const string GREETING = "Hello";
   string message = GREETING + " World"; // Может быть оптимизировано
   ```

### ✅ Используйте StringBuilder когда:

1. **Множественная конкатенация в цикле**
   ```csharp
   // ✅ StringBuilder обязателен
   var sb = new StringBuilder();
   for (int i = 0; i < 1000; i++)
   {
       sb.Append(items[i]);
   }
   ```

2. **Динамическое построение строки**
   ```csharp
   // ✅ StringBuilder для динамического построения
   var sb = new StringBuilder();
   if (includeHeader) sb.AppendLine("Header");
   foreach (var item in items) sb.AppendLine(item);
   if (includeFooter) sb.AppendLine("Footer");
   ```

3. **Большие объемы данных**
   ```csharp
   // ✅ StringBuilder для больших строк
   var sb = new StringBuilder(capacity: 100000);
   // ... добавление большого количества данных
   ```

4. **Многократные операции модификации**
   ```csharp
   // ✅ StringBuilder для множественных операций
   var sb = new StringBuilder(text);
   sb.Replace("old", "new");
   sb.Insert(0, "Prefix: ");
   sb.AppendLine("Suffix");
   ```

---

## 6. Продвинутые техники

### ChunkedAppend для очень больших данных

```csharp
using System.Text;

class ChunkedAppend
{
    static string BuildLargeString(int chunkSize, int chunks)
    {
        var sb = new StringBuilder(capacity: chunkSize * chunks);
        
        // Добавление больших чанков
        string chunk = new string('a', chunkSize);
        for (int i = 0; i < chunks; i++)
        {
            sb.Append(chunk);
        }
        
        return sb.ToString();
    }
    
    static void Main()
    {
        string result = BuildLargeString(chunkSize: 1000, chunks: 100);
        Console.WriteLine($"Result length: {result.Length}");
    }
}
```

### Форматирование с StringBuilder

```csharp
using System.Text;

class FormattingExample
{
    static void Main()
    {
        var sb = new StringBuilder();
        
        // AppendFormat для форматирования
        sb.AppendFormat("Name: {0}, Age: {1}, Salary: {2:C}", 
            "John", 30, 50000);
        Console.WriteLine(sb.ToString());
        
        sb.Clear();
        
        // Использование с индексами
        sb.AppendFormat("{0} + {1} = {2}", 10, 20, 30);
        Console.WriteLine(sb.ToString());
    }
}
```

### Условное добавление

```csharp
using System.Text;

class ConditionalAppend
{
    static string BuildConditionalString(bool includeDetails, int count)
    {
        var sb = new StringBuilder();
        sb.Append("Items: ");
        sb.Append(count);
        
        if (includeDetails)
        {
            sb.AppendLine();
            sb.AppendLine("Details:");
            for (int i = 0; i < count; i++)
            {
                sb.AppendFormat("  Item {0}\n", i + 1);
            }
        }
        
        return sb.ToString();
    }
    
    static void Main()
    {
        string result1 = BuildConditionalString(includeDetails: false, count: 5);
        Console.WriteLine(result1);
        
        string result2 = BuildConditionalString(includeDetails: true, count: 3);
        Console.WriteLine(result2);
    }
}
```

---

## 7. String Interpolation vs StringBuilder

### String Interpolation (C# 6.0+)

```csharp
// String interpolation - удобен и эффективен для простых случаев
string name = "John";
int age = 30;
string message = $"Hello, {name}! You are {age} years old.";
// Компилятор оптимизирует это эффективно

// Но НЕ используйте в циклах для конкатенации!
// ❌ Плохо:
string result = "";
for (int i = 0; i < 1000; i++)
{
    result += $"Item {i}"; // Все равно создает новые строки!
}

// ✅ Хорошо:
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++)
{
    sb.AppendFormat("Item {0}", i); // Или sb.Append($"Item {i}");
}
string result = sb.ToString();
```

### Сравнение подходов

```csharp
using System.Text;

class Comparison
{
    static void Main()
    {
        string name = "John";
        int age = 30;
        string city = "New York";
        
        // Вариант 1: String concatenation
        string result1 = "Name: " + name + ", Age: " + age + ", City: " + city;
        
        // Вариант 2: String.Format
        string result2 = string.Format("Name: {0}, Age: {1}, City: {2}", name, age, city);
        
        // Вариант 3: String interpolation (рекомендуется для простых случаев)
        string result3 = $"Name: {name}, Age: {age}, City: {city}";
        
        // Вариант 4: StringBuilder (для сложных/динамических случаев)
        var sb = new StringBuilder();
        sb.Append("Name: ").Append(name);
        sb.Append(", Age: ").Append(age);
        sb.Append(", City: ").Append(city);
        string result4 = sb.ToString();
        
        // Все варианты дают одинаковый результат
        // Выбор зависит от контекста
    }
}
```

---

## 8. Best Practices

### ✅ Что делать

1. **Используйте StringBuilder для циклов**
   ```csharp
   var sb = new StringBuilder();
   foreach (var item in items)
   {
       sb.AppendLine(item);
   }
   ```

2. **Устанавливайте Capacity заранее если известен размер**
   ```csharp
   var sb = new StringBuilder(capacity: expectedSize);
   ```

3. **Используйте Clear() для переиспользования**
   ```csharp
   sb.Clear(); // Вместо создания нового StringBuilder
   ```

4. **Используйте AppendFormat для форматирования**
   ```csharp
   sb.AppendFormat("Value: {0:C}", amount);
   ```

5. **Используйте String для простых случаев**
   ```csharp
   string message = $"Hello, {name}!";
   ```

### ❌ Чего избегать

1. **Не используйте String конкатенацию в циклах**
   ```csharp
   // ❌ Плохо
   string result = "";
   for (int i = 0; i < 1000; i++)
   {
       result += i.ToString();
   }
   ```

2. **Не создавайте новый StringBuilder в каждой итерации**
   ```csharp
   // ❌ Плохо
   foreach (var item in items)
   {
       var sb = new StringBuilder(); // Создание в каждой итерации!
       sb.Append(item);
   }
   ```

3. **Не игнорируйте Capacity для больших строк**
   ```csharp
   // ❌ Плохо (если известно что будет ~10000 символов)
   var sb = new StringBuilder(); // Capacity = 16, будет перераспределение
   
   // ✅ Хорошо
   var sb = new StringBuilder(capacity: 10000);
   ```

4. **Не используйте StringBuilder для одной операции**
   ```csharp
   // ❌ Избыточно
   var sb = new StringBuilder();
   sb.Append("Hello");
   string result = sb.ToString();
   
   // ✅ Проще
   string result = "Hello";
   ```

---

## 9. Вопросы для собеседований

### Типичные вопросы и ответы

**Q1: В чем разница между String и StringBuilder?**
- String: immutable, создает новые объекты при каждой операции
- StringBuilder: mutable, использует внутренний буфер, эффективен для множественных операций

**Q2: Когда использовать String, а когда StringBuilder?**
- String: малые операции, string interpolation, неизменяемые строки
- StringBuilder: циклы, множественная конкатенация, динамическое построение

**Q3: Почему String конкатенация в цикле медленная?**
- Каждая операция создает новый объект строки
- Старые объекты остаются в памяти до GC
- O(n²) сложность по памяти и времени

**Q4: Как работает Capacity в StringBuilder?**
- Capacity - размер внутреннего буфера
- По умолчанию 16 символов
- Автоматически увеличивается при необходимости (обычно удваивается)
- Можно установить заранее для оптимизации

**Q5: Что такое String Interpolation и когда его использовать?**
- Синтаксис $"text {variable}" в C# 6.0+
- Удобен для простых случаев форматирования
- Компилятор оптимизирует эффективно
- Не подходит для множественной конкатенации в циклах

**Q6: Можно ли переиспользовать StringBuilder?**
- Да, используя метод Clear()
- Это эффективнее создания нового объекта
- Полезно в циклах или при повторных операциях

**Q7: Какой Complexity у операций?**
- String конкатенация: O(n) для каждой операции, O(n²) для n операций
- StringBuilder.Append: O(1) амортизированно, O(n) при расширении буфера

---

## Заключение

Выбор между String и StringBuilder зависит от сценария использования:
- **String**: простые операции, небольшое количество конкатенаций, string interpolation
- **StringBuilder**: циклы, множественные операции, динамическое построение строк

Помните: **для циклов и множественных операций всегда используйте StringBuilder!**
