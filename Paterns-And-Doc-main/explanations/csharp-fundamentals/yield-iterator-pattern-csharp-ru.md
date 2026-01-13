# Yield и Iterator Pattern в C#

## Введение

**yield** — это ключевое слово в C#, которое позволяет создавать итераторы (iterators) для коллекций. Понимание `yield` и Iterator Pattern критически важно для написания эффективного кода, особенно при работе с большими наборами данных и lazy evaluation.

---

## 1. Что такое Yield?

**yield** позволяет создавать методы, которые возвращают последовательность значений по требованию (lazy evaluation), без необходимости создавать всю коллекцию в памяти.

### Проблема без yield

```csharp
using System;
using System.Collections.Generic;

class Program
{
    // ❌ Плохо: создает всю коллекцию в памяти сразу
    static List<int> GetNumbersBad(int count)
    {
        var numbers = new List<int>();
        for (int i = 0; i < count; i++)
        {
            numbers.Add(i * i); // Все числа создаются и хранятся в памяти
        }
        return numbers; // Вся коллекция возвращается сразу
    }
    
    static void Main()
    {
        // Проблема: если count = 1,000,000, то создается список из 1M элементов
        var numbers = GetNumbersBad(1000000);
        
        // Используем только первые 10 элементов
        foreach (var num in numbers)
        {
            if (num > 100) break; // Остальные 999,990 элементов созданы зря!
            Console.WriteLine(num);
        }
    }
}
```

### Решение с yield

```csharp
using System;
using System.Collections.Generic;

class Program
{
    // ✅ Хорошо: yield возвращает значения по требованию
    static IEnumerable<int> GetNumbersGood(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return i * i; // Значение возвращается немедленно, выполнение приостанавливается
        }
        // Когда foreach запрашивает следующее значение, выполнение продолжается с этой точки
    }
    
    static void Main()
    {
        // Никакие значения еще не созданы!
        var numbers = GetNumbersGood(1000000);
        
        // Значения создаются только когда они запрашиваются
        foreach (var num in numbers)
        {
            if (num > 100) break; // Создано только несколько значений!
            Console.WriteLine(num);
        }
        // Остальные значения никогда не создавались - экономия памяти и времени
    }
}
```

---

## 2. Как работает Yield?

### Механизм работы

```csharp
using System;
using System.Collections.Generic;

class YieldMechanism
{
    static IEnumerable<int> SimpleYield()
    {
        Console.WriteLine("Method started");
        
        yield return 1;
        Console.WriteLine("After first yield");
        
        yield return 2;
        Console.WriteLine("After second yield");
        
        yield return 3;
        Console.WriteLine("After third yield");
        
        Console.WriteLine("Method ended");
    }
    
    static void Main()
    {
        // Метод еще не выполняется!
        var enumerable = SimpleYield();
        Console.WriteLine("Enumerable created");
        
        // Только теперь метод начинает выполняться
        foreach (var value in enumerable)
        {
            Console.WriteLine($"Got value: {value}");
        }
        
        // Вывод:
        // Enumerable created
        // Method started
        // Got value: 1
        // After first yield
        // Got value: 2
        // After second yield
        // Got value: 3
        // After third yield
        // Method ended
    }
}
```

### State Machine под капотом

Компилятор преобразует метод с `yield` в state machine (машину состояний):

```csharp
// Вы пишете:
static IEnumerable<int> GetNumbers()
{
    yield return 1;
    yield return 2;
    yield return 3;
}

// Компилятор создает примерно следующее (упрощенно):
class GetNumbersStateMachine : IEnumerable<int>, IEnumerator<int>
{
    private int _state = 0;
    private int _current;
    
    public int Current => _current;
    
    public bool MoveNext()
    {
        switch (_state)
        {
            case 0: _current = 1; _state = 1; return true;
            case 1: _current = 2; _state = 2; return true;
            case 2: _current = 3; _state = -1; return true;
            default: return false;
        }
    }
}
```

---

## 3. yield return

### Базовое использование yield return

```csharp
using System;
using System.Collections.Generic;

class YieldReturn
{
    // Генерация чисел от 0 до n
    static IEnumerable<int> GenerateNumbers(int n)
    {
        for (int i = 0; i < n; i++)
        {
            yield return i;
        }
    }
    
    // Генерация только четных чисел
    static IEnumerable<int> GetEvenNumbers(int max)
    {
        for (int i = 0; i < max; i += 2)
        {
            yield return i;
        }
    }
    
    // Генерация с условием
    static IEnumerable<int> GetFilteredNumbers(int max, Func<int, bool> predicate)
    {
        for (int i = 0; i < max; i++)
        {
            if (predicate(i))
            {
                yield return i;
            }
        }
    }
    
    static void Main()
    {
        // Использование
        foreach (var num in GenerateNumbers(10))
        {
            Console.WriteLine(num);
        }
        
        // С условием
        foreach (var num in GetFilteredNumbers(20, x => x % 3 == 0))
        {
            Console.WriteLine(num); // Кратные 3: 0, 3, 6, 9, 12, 15, 18
        }
    }
}
```

### Множественные yield return

```csharp
using System;
using System.Collections.Generic;

class MultipleYield
{
    static IEnumerable<string> GetNames()
    {
        yield return "Alice";
        yield return "Bob";
        yield return "Charlie";
        yield return "David";
    }
    
    // Можно использовать в разных блоках
    static IEnumerable<int> GetNumbersWithCondition()
    {
        yield return 1;
        yield return 2;
        
        if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
        {
            yield return 3;
            yield return 4;
        }
        
        yield return 5;
    }
    
    static void Main()
    {
        foreach (var name in GetNames())
        {
            Console.WriteLine(name);
        }
    }
}
```

---

## 4. yield break

**yield break** завершает итерацию досрочно.

```csharp
using System;
using System.Collections.Generic;

class YieldBreak
{
    // Генерация до первого отрицательного числа
    static IEnumerable<int> GetNumbersUntilNegative(int[] numbers)
    {
        foreach (var num in numbers)
        {
            if (num < 0)
            {
                yield break; // Завершаем итерацию досрочно
            }
            yield return num;
        }
    }
    
    // Генерация до определенного условия
    static IEnumerable<int> GetNumbersUntilLimit(int max, int limit)
    {
        for (int i = 0; i < max; i++)
        {
            if (i >= limit)
            {
                yield break; // Завершаем когда достигли limit
            }
            yield return i;
        }
    }
    
    static void Main()
    {
        int[] numbers = { 1, 2, 3, -5, 6, 7 };
        
        foreach (var num in GetNumbersUntilNegative(numbers))
        {
            Console.WriteLine(num); // Выведет: 1, 2, 3 (до -5)
        }
    }
}
```

---

## 5. Практические примеры

### Пример 1: Чтение файла построчно

```csharp
using System;
using System.Collections.Generic;
using System.IO;

class FileReader
{
    // ✅ Эффективно: читает строки по требованию
    static IEnumerable<string> ReadLinesLazy(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line; // Возвращаем строку сразу, не ждем весь файл
            }
        }
    }
    
    // ❌ Неэффективно: читает весь файл в память
    static List<string> ReadLinesEager(string filePath)
    {
        var lines = new List<string>();
        using (var reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                lines.Add(line); // Все строки в памяти
            }
        }
        return lines;
    }
    
    static void Main()
    {
        // Использование lazy чтения
        foreach (var line in ReadLinesLazy("largefile.txt"))
        {
            if (line.StartsWith("STOP"))
                break; // Файл читается только до этой строки!
            
            Console.WriteLine(line);
        }
    }
}
```

### Пример 2: Фильтрация и трансформация

```csharp
using System;
using System.Collections.Generic;

class FilterTransform
{
    // Фильтрация с yield
    static IEnumerable<int> Filter(IEnumerable<int> source, Func<int, bool> predicate)
    {
        foreach (var item in source)
        {
            if (predicate(item))
            {
                yield return item;
            }
        }
    }
    
    // Трансформация с yield
    static IEnumerable<TResult> Map<TSource, TResult>(
        IEnumerable<TSource> source, 
        Func<TSource, TResult> selector)
    {
        foreach (var item in source)
        {
            yield return selector(item);
        }
    }
    
    // Комбинация фильтрации и трансформации
    static IEnumerable<string> GetProcessedNumbers(IEnumerable<int> numbers)
    {
        foreach (var num in numbers)
        {
            if (num > 0 && num % 2 == 0)
            {
                yield return $"Number: {num * 2}";
            }
        }
    }
    
    static void Main()
    {
        var numbers = new[] { -5, -2, 0, 1, 2, 3, 4, 5, 6 };
        
        // Все операции выполняются lazy (по требованию)
        var result = GetProcessedNumbers(numbers);
        
        // Обработка только первых 3 элементов
        int count = 0;
        foreach (var item in result)
        {
            Console.WriteLine(item);
            if (++count >= 3) break; // Остальные элементы не обрабатывались
        }
    }
}
```

### Пример 3: Генерация бесконечных последовательностей

```csharp
using System;
using System.Collections.Generic;

class InfiniteSequences
{
    // Генерация бесконечной последовательности чисел
    static IEnumerable<int> NaturalNumbers()
    {
        int n = 0;
        while (true) // Бесконечный цикл - но это OK с yield!
        {
            yield return n++;
        }
    }
    
    // Числа Фибоначчи
    static IEnumerable<long> Fibonacci()
    {
        long a = 0, b = 1;
        yield return a;
        yield return b;
        
        while (true)
        {
            long next = a + b;
            yield return next;
            a = b;
            b = next;
        }
    }
    
    // Простые числа (простой алгоритм)
    static IEnumerable<int> PrimeNumbers()
    {
        yield return 2;
        
        int candidate = 3;
        while (true)
        {
            bool isPrime = true;
            for (int i = 2; i * i <= candidate; i++)
            {
                if (candidate % i == 0)
                {
                    isPrime = false;
                    break;
                }
            }
            
            if (isPrime)
            {
                yield return candidate;
            }
            
            candidate += 2; // Только нечетные числа
        }
    }
    
    static void Main()
    {
        // Бесконечная последовательность, но обрабатываем только первые 10
        foreach (var num in NaturalNumbers())
        {
            Console.WriteLine(num);
            if (num >= 9) break; // Важно: нужен break для бесконечных последовательностей!
        }
        
        // Первые 10 чисел Фибоначчи
        int count = 0;
        foreach (var fib in Fibonacci())
        {
            Console.WriteLine($"F({count}) = {fib}");
            if (++count >= 10) break;
        }
    }
}
```

### Пример 4: Разбиение на батчи (Chunking)

```csharp
using System;
using System.Collections.Generic;

class Chunking
{
    // Разбиение последовательности на батчи заданного размера
    static IEnumerable<T[]> Chunk<T>(IEnumerable<T> source, int chunkSize)
    {
        var chunk = new List<T>();
        
        foreach (var item in source)
        {
            chunk.Add(item);
            
            if (chunk.Count == chunkSize)
            {
                yield return chunk.ToArray();
                chunk.Clear();
            }
        }
        
        // Возвращаем остаток если есть
        if (chunk.Count > 0)
        {
            yield return chunk.ToArray();
        }
    }
    
    static void Main()
    {
        var numbers = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        
        // Разбиение на батчи по 3 элемента
        foreach (var chunk in Chunk(numbers, 3))
        {
            Console.WriteLine($"Chunk: [{string.Join(", ", chunk)}]");
        }
        
        // Вывод:
        // Chunk: [1, 2, 3]
        // Chunk: [4, 5, 6]
        // Chunk: [7, 8, 9]
        // Chunk: [10]
    }
}
```

---

## 6. Yield в async методах

### IAsyncEnumerable<T> (C# 8.0+)

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

class AsyncYield
{
    // Асинхронная генерация с yield
    static async IAsyncEnumerable<int> GetNumbersAsync(int count)
    {
        for (int i = 0; i < count; i++)
        {
            await Task.Delay(100); // Асинхронная операция
            yield return i;
        }
    }
    
    static async Task Main()
    {
        // await foreach для IAsyncEnumerable
        await foreach (var num in GetNumbersAsync(10))
        {
            Console.WriteLine(num);
        }
    }
}
```

---

## 7. Производительность и Lazy Evaluation

### Преимущества yield

```csharp
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

class PerformanceComparison
{
    // ❌ Eager evaluation: все элементы создаются сразу
    static List<int> GetNumbersEager(int count)
    {
        var numbers = new List<int>();
        for (int i = 0; i < count; i++)
        {
            numbers.Add(i * i);
        }
        return numbers;
    }
    
    // ✅ Lazy evaluation: элементы создаются по требованию
    static IEnumerable<int> GetNumbersLazy(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return i * i;
        }
    }
    
    static void Main()
    {
        const int count = 10_000_000;
        
        // Eager: создает все 10M элементов
        var sw1 = Stopwatch.StartNew();
        var eager = GetNumbersEager(count);
        var result1 = eager.Take(10).ToList(); // Используем только 10!
        sw1.Stop();
        Console.WriteLine($"Eager: {sw1.ElapsedMilliseconds}ms, Memory: большой");
        
        // Lazy: создает только 10 элементов
        var sw2 = Stopwatch.StartNew();
        var lazy = GetNumbersLazy(count);
        var result2 = lazy.Take(10).ToList(); // Создано только 10!
        sw2.Stop();
        Console.WriteLine($"Lazy: {sw2.ElapsedMilliseconds}ms, Memory: маленький");
        
        // Lazy значительно быстрее и использует меньше памяти!
    }
}
```

---

## 8. Best Practices

### ✅ Что делать

1. **Используйте yield для больших последовательностей**
   ```csharp
   static IEnumerable<int> GetLargeSequence() { yield return ...; }
   ```

2. **Используйте yield для lazy evaluation**
   ```csharp
   // Элементы обрабатываются только при запросе
   ```

3. **Комбинируйте yield с LINQ для эффективности**
   ```csharp
   var result = GetNumbers().Where(x => x > 10).Take(5);
   ```

4. **Используйте yield break для раннего завершения**
   ```csharp
   if (condition) yield break;
   ```

### ❌ Чего избегать

1. **Не используйте yield в методах с out/ref параметрами**
   ```csharp
   // ❌ Нельзя
   static IEnumerable<int> BadMethod(out int value) { yield return 1; }
   ```

2. **Не используйте yield в try-catch блоках с yield return**
   ```csharp
   // ❌ Проблематично
   try { yield return 1; } catch { }
   // Можно использовать try-finally
   ```

3. **Не забывайте про break для бесконечных последовательностей**
   ```csharp
   foreach (var item in InfiniteSequence())
   {
       if (condition) break; // Важно!
   }
   ```

---

## 9. Вопросы для собеседований

### Типичные вопросы и ответы

**Q1: Что такое yield и зачем он нужен?**
- Ключевое слово для создания итераторов
- Позволяет lazy evaluation (вычисление по требованию)
- Экономит память и время для больших последовательностей

**Q2: В чем разница между yield return и обычным return?**
- return: завершает метод и возвращает значение
- yield return: приостанавливает выполнение, возвращает значение, продолжает при следующем запросе

**Q3: Когда использовать yield?**
- Большие последовательности данных
- Lazy evaluation требуется
- Экономия памяти важна
- Данные генерируются или читаются по требованию

**Q4: Что такое yield break?**
- Досрочное завершение итерации
- Эквивалентно return в обычном методе, но для итератора

**Q5: Как компилятор реализует yield?**
- Создает state machine (машину состояний)
- Сохраняет состояние между вызовами MoveNext()
- Позволяет возобновлять выполнение с точки yield return

**Q6: Можно ли использовать yield в async методах?**
- Да, с IAsyncEnumerable<T> (C# 8.0+)
- Используется await yield return
- Обрабатывается через await foreach

---

## Заключение

Yield и Iterator Pattern — мощные инструменты для:
- Lazy evaluation и экономии памяти
- Работы с большими последовательностями данных
- Создания эффективных итераторов
- Улучшения производительности приложений

Помните: **используйте yield для последовательностей, которые не нужно создавать целиком в памяти!**
