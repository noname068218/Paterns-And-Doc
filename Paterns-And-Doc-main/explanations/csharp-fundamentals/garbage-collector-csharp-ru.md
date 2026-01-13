# Garbage Collector (GC) в C#

## Введение

**Garbage Collector (GC)** — это автоматический менеджер памяти в .NET, который освобождает объекты, которые больше не используются приложением. Понимание работы GC критически важно для написания эффективного кода и прохождения собеседований на Middle позицию.

---

## 1. Что такое Garbage Collector?

GC — это компонент CLR (Common Language Runtime), который автоматически управляет памятью, освобождая объекты, на которые больше нет ссылок.

### Проблема без GC

```csharp
// В языках без GC (например, C/C++) нужно вручную управлять памятью
void* ptr = malloc(100); // Выделение памяти
// ... использование ...
free(ptr); // Освобождение памяти (если забыть - утечка памяти!)
```

### Решение с GC

```csharp
// В C# GC автоматически освобождает память
public void Example()
{
    var obj = new MyClass(); // Выделение памяти
    // ... использование ...
    // Когда obj выходит из области видимости, GC автоматически освободит память
}
```

---

## 2. Как работает Garbage Collector?

### Основные шаги сборки мусора

```
1. Mark (Маркировка)
   └── GC находит все "живые" объекты (достижимые из корневых ссылок)

2. Plan (Планирование)
   └── GC решает, какие объекты удалить

3. Sweep (Очистка)
   └── GC освобождает память от "мертвых" объектов

4. Compact (Уплотнение)
   └── GC перемещает объекты, чтобы устранить фрагментацию
```

### Корневые ссылки (Root References)

Корневые ссылки — это точки входа, от которых GC начинает поиск "живых" объектов:

```csharp
class Program
{
    // 1. Статические поля - корневые ссылки
    private static MyClass _staticField = new MyClass();
    
    // 2. Локальные переменные в активных методах
    public void Method()
    {
        var localVar = new MyClass(); // Корневая ссылка пока метод активен
        
        // 3. Параметры методов - корневые ссылки
        ProcessObject(localVar);
    }
    
    // 4. Поля объектов, на которые есть ссылки
    private void ProcessObject(MyClass obj)
    {
        obj.Data = "test"; // obj - корневая ссылка
    }
}
```

### Пример работы GC

```csharp
using System;

class MyClass
{
    public int Value { get; set; }
    public MyClass(int value) => Value = value;
    
    ~MyClass() // Finalizer (деструктор)
    {
        Console.WriteLine($"Object {Value} is being finalized");
    }
}

class Program
{
    static void Main()
    {
        // Создаем объекты
        var obj1 = new MyClass(1);
        var obj2 = new MyClass(2);
        var obj3 = new MyClass(3);
        
        // obj1, obj2, obj3 - все корневые ссылки (достижимы)
        
        obj1 = null; // Удаляем ссылку на obj1
        // obj1 больше не корневая ссылка, но объект еще не удален
        
        // Принудительная сборка мусора (только для демонстрации, не использовать в продакшене!)
        GC.Collect();
        GC.WaitForPendingFinalizers();
        
        // obj1 будет удален, obj2 и obj3 останутся (пока они в области видимости)
        
        Console.WriteLine("End of Main");
        // obj2 и obj3 будут удалены когда Main завершится
    }
}
```

---

## 3. Поколения (Generations) в GC

.NET GC использует трехпоколенческую систему для оптимизации сборки мусора.

### Три поколения

```
┌─────────────────────────────────────────────────┐
│  Generation 0 (Gen 0) - Молодые объекты        │
│  └── Недавно созданные объекты                 │
│  └── Сборка происходит часто                   │
│  └── Размер ~16 MB (примерно)                  │
├─────────────────────────────────────────────────┤
│  Generation 1 (Gen 1) - Выжившие объекты       │
│  └── Объекты, пережившие сборку Gen 0          │
│  └── Сборка происходит реже                    │
│  └── Размер ~2-16 MB (примерно)                │
├─────────────────────────────────────────────────┤
│  Generation 2 (Gen 2) - Старые объекты         │
│  └── Объекты, пережившие сборки Gen 1          │
│  └── Сборка происходит очень редко             │
│  └── Размер может быть большим                 │
└─────────────────────────────────────────────────┘
```

### Жизненный цикл объекта

```csharp
class Program
{
    static void Main()
    {
        // 1. Создание объекта - попадает в Gen 0
        var obj = new MyClass();
        Console.WriteLine($"Generation after creation: {GC.GetGeneration(obj)}"); // 0
        
        // 2. Первая сборка Gen 0 - если объект выжил, переходит в Gen 1
        GC.Collect(0, GCCollectionMode.Forced);
        Console.WriteLine($"Generation after Gen 0 collection: {GC.GetGeneration(obj)}"); // 1
        
        // 3. Сборка Gen 1 - если объект выжил, переходит в Gen 2
        GC.Collect(1, GCCollectionMode.Forced);
        Console.WriteLine($"Generation after Gen 1 collection: {GC.GetGeneration(obj)}"); // 2
        
        // 4. Дальнейшие сборки - объект остается в Gen 2
        GC.Collect(2, GCCollectionMode.Forced);
        Console.WriteLine($"Generation after Gen 2 collection: {GC.GetGeneration(obj)}"); // 2
    }
}

class MyClass
{
    public int Value { get; set; }
}
```

### Large Object Heap (LOH) - Объекты больше 85KB

```csharp
// Объекты размером >= 85KB размещаются в специальной куче LOH
// LOH собирается только вместе с Gen 2

class Program
{
    static void Main()
    {
        // Массив > 85KB попадает в LOH
        byte[] largeArray = new byte[100_000]; // 100KB
        
        Console.WriteLine($"Is large object: {GC.GetGeneration(largeArray) == 2}");
        // LOH объекты всегда показывают Gen 2
        
        // Важно: LOH не уплотняется (не compact), только очищается
        // Это может привести к фрагментации памяти
    }
}
```

---

## 4. Типы сборок мусора

### Ephemeral GC (Быстрая сборка)

Собирает только Gen 0 и Gen 1. Быстрая операция, происходит часто.

```csharp
// Ephemeral GC происходит автоматически когда Gen 0 заполняется
// Обычно занимает < 1ms для небольших приложений

for (int i = 0; i < 1000; i++)
{
    var obj = new MyClass(i);
    // Когда Gen 0 заполнится, произойдет Ephemeral GC
}
```

### Full GC (Полная сборка)

Собирает все поколения (Gen 0, 1, 2) и LOH. Медленная операция, происходит редко.

```csharp
// Full GC происходит когда:
// 1. Gen 2 заполняется
// 2. Недостаточно памяти после Ephemeral GC
// 3. Вызов GC.Collect() без параметров

GC.Collect(); // Полная сборка всех поколений
GC.WaitForPendingFinalizers(); // Ждем завершения finalizers
GC.Collect(); // Еще одна сборка для очистки объектов с finalizers
```

### Background GC (Фоновая сборка)

В .NET 4.0+ Full GC может выполняться в фоновом потоке, не блокируя основные потоки.

```csharp
// Background GC работает параллельно с приложением
// Не блокирует потоки приложения во время большей части сборки

// Проверка, выполняется ли фоновая сборка
bool isBackgroundGC = GC.TryStartNoGCRegion(100_000_000); // Попытка создать регион без GC
if (isBackgroundGC)
{
    try
    {
        // Критический код, где GC нежелателен
        PerformCriticalOperation();
    }
    finally
    {
        GC.EndNoGCRegion(); // Завершаем регион без GC
    }
}
```

---

## 5. Когда происходит сборка мусора?

GC срабатывает автоматически в следующих случаях:

### Автоматические триггеры

```csharp
// 1. Когда Gen 0 заполняется
for (int i = 0; i < 100_000; i++)
{
    var obj = new byte[1024]; // Когда Gen 0 заполнится, произойдет GC
}

// 2. Когда выделяется память и недостаточно места
var largeArray = new byte[50_000_000]; // Может вызвать GC если недостаточно памяти

// 3. Когда приложение выгружает AppDomain
// GC происходит автоматически

// 4. При вызове GC.Collect() вручную (не рекомендуется!)
GC.Collect();
```

### Мониторинг сборок мусора

```csharp
using System;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        // Получение информации о GC
        long gen0Collections = GC.CollectionCount(0);
        long gen1Collections = GC.CollectionCount(1);
        long gen2Collections = GC.CollectionCount(2);
        
        Console.WriteLine($"Gen 0 collections: {gen0Collections}");
        Console.WriteLine($"Gen 1 collections: {gen1Collections}");
        Console.WriteLine($"Gen 2 collections: {gen2Collections}");
        
        // Получение использованной памяти
        long memoryBefore = GC.GetTotalMemory(false); // false = без принудительной сборки
        Console.WriteLine($"Memory before: {memoryBefore / 1024} KB");
        
        // Создание объектов
        CreateObjects();
        
        long memoryAfter = GC.GetTotalMemory(false);
        Console.WriteLine($"Memory after creation: {memoryAfter / 1024} KB");
        
        // Принудительная сборка
        GC.Collect();
        long memoryAfterGC = GC.GetTotalMemory(true); // true = после сборки
        Console.WriteLine($"Memory after GC: {memoryAfterGC / 1024} KB");
    }
    
    static void CreateObjects()
    {
        for (int i = 0; i < 1000; i++)
        {
            var obj = new byte[1024];
        }
    }
}
```

---

## 6. Оптимизация производительности GC

### Минимизация количества объектов

```csharp
// ❌ Плохо: Создание множества временных объектов
public string ConcatenateStrings(string[] strings)
{
    string result = "";
    foreach (var s in strings)
    {
        result += s; // Создается новый объект string при каждой итерации!
    }
    return result;
}

// ✅ Хорошо: Использование StringBuilder
public string ConcatenateStrings(string[] strings)
{
    var sb = new StringBuilder();
    foreach (var s in strings)
    {
        sb.Append(s); // Переиспользует внутренний буфер
    }
    return sb.ToString();
}
```

### Object Pooling (Пул объектов)

```csharp
using System.Collections.Generic;

// Пул для переиспользования объектов
public class ObjectPool<T> where T : new()
{
    private readonly Stack<T> _pool = new Stack<T>();
    private readonly object _lock = new object();
    
    public T Rent()
    {
        lock (_lock)
        {
            if (_pool.Count > 0)
            {
                return _pool.Pop(); // Переиспользуем объект из пула
            }
        }
        return new T(); // Создаем новый если пул пуст
    }
    
    public void Return(T item)
    {
        // Сброс состояния объекта перед возвратом в пул
        if (item is IResettable resettable)
        {
            resettable.Reset();
        }
        
        lock (_lock)
        {
            _pool.Push(item); // Возвращаем объект в пул
        }
    }
}

// Использование пула
var pool = new ObjectPool<MyClass>();
var obj = pool.Rent(); // Берем из пула или создаем новый
try
{
    // Использование объекта
    obj.DoWork();
}
finally
{
    pool.Return(obj); // Возвращаем в пул для переиспользования
}
```

### ArrayPool<T> для массивов

```csharp
using System.Buffers;

// Использование ArrayPool для переиспользования массивов
public void ProcessData()
{
    var pool = ArrayPool<byte>.Shared;
    
    // Берем массив из пула
    byte[] buffer = pool.Rent(1024); // Минимальный размер 1024
    
    try
    {
        // Использование buffer
        ProcessBuffer(buffer);
    }
    finally
    {
        // Возвращаем массив в пул
        pool.Return(buffer, clearArray: false); // clearArray = очищать ли массив
    }
}

private void ProcessBuffer(byte[] buffer)
{
    // Обработка данных
}
```

### Избегание Large Object Heap

```csharp
// ❌ Плохо: Создание больших массивов в LOH
byte[] largeArray = new byte[100_000]; // Попадает в LOH

// ✅ Хорошо: Разбиение на меньшие массивы
const int chunkSize = 80_000; // Меньше 85KB
byte[] chunk1 = new byte[chunkSize];
byte[] chunk2 = new byte[chunkSize];
// Объекты попадают в обычную кучу, быстрее собираются
```

---

## 7. Finalizers (Финализаторы) и Dispose Pattern

### Проблема с Finalizers

```csharp
// ❌ Проблема: Finalizers замедляют GC
class BadResource
{
    private IntPtr _handle;
    
    public BadResource()
    {
        _handle = AllocateHandle();
    }
    
    ~BadResource() // Finalizer - выполняется медленно
    {
        // Объекты с finalizers не удаляются сразу
        // Переходят в очередь finalization
        // Удаляются только при следующей Full GC
        ReleaseHandle(_handle);
    }
    
    private IntPtr AllocateHandle() => new IntPtr(1);
    private void ReleaseHandle(IntPtr handle) { }
}
```

### Правильный Dispose Pattern

```csharp
using System;

// ✅ Правильная реализация IDisposable
public class ManagedResource : IDisposable
{
    private bool _disposed = false;
    private IntPtr _handle;
    private SomeManagedObject _managedObject;
    
    public ManagedResource()
    {
        _handle = AllocateHandle();
        _managedObject = new SomeManagedObject();
    }
    
    // Публичный метод Dispose
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this); // Указываем GC, что finalizer не нужен
    }
    
    // Защищенный виртуальный метод Dispose
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Освобождение управляемых ресурсов
                _managedObject?.Dispose();
                _managedObject = null;
            }
            
            // Освобождение неуправляемых ресурсов
            if (_handle != IntPtr.Zero)
            {
                ReleaseHandle(_handle);
                _handle = IntPtr.Zero;
            }
            
            _disposed = true;
        }
    }
    
    // Finalizer - только для освобождения неуправляемых ресурсов
    ~ManagedResource()
    {
        Dispose(false); // Не освобождаем управляемые ресурсы в finalizer!
    }
    
    // Методы для работы с ресурсом
    public void DoWork()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(ManagedResource));
        
        // Работа с ресурсом
    }
    
    private IntPtr AllocateHandle() => new IntPtr(1);
    private void ReleaseHandle(IntPtr handle) { }
}

class SomeManagedObject : IDisposable
{
    public void Dispose() { }
}
```

### Using Statement (Автоматический вызов Dispose)

```csharp
// ✅ Правильное использование using
using (var resource = new ManagedResource())
{
    resource.DoWork();
    // Dispose() вызывается автоматически при выходе из блока
}

// Эквивалентно:
var resource = new ManagedResource();
try
{
    resource.DoWork();
}
finally
{
    resource.Dispose();
}

// C# 8.0+ using declaration
using var resource = new ManagedResource();
resource.DoWork();
// Dispose вызывается когда переменная выходит из области видимости
```

---

## 8. Weak References (Слабые ссылки)

Weak Reference позволяет GC собирать объект, даже если на него есть ссылка.

### Когда использовать Weak References

```csharp
using System;

// Обычная ссылка - объект не будет собран
var strongRef = new MyClass();
GC.Collect();
GC.WaitForPendingFinalizers();
Console.WriteLine($"Strong reference alive: {strongRef != null}"); // True

// Weak Reference - объект может быть собран
var obj = new MyClass();
WeakReference weakRef = new WeakReference(obj);
obj = null; // Удаляем сильную ссылку

GC.Collect();
GC.WaitForPendingFinalizers();

if (weakRef.IsAlive)
{
    var target = weakRef.Target as MyClass;
    Console.WriteLine("Object is still alive");
}
else
{
    Console.WriteLine("Object has been collected");
}

class MyClass
{
    public int Value { get; set; }
}
```

### Практический пример: Кэш с автоматической очисткой

```csharp
using System;
using System.Collections.Generic;

public class WeakReferenceCache<TKey, TValue> where TValue : class
{
    private readonly Dictionary<TKey, WeakReference<TValue>> _cache = new();
    private readonly object _lock = new object();
    
    public void Add(TKey key, TValue value)
    {
        lock (_lock)
        {
            _cache[key] = new WeakReference<TValue>(value);
        }
    }
    
    public bool TryGetValue(TKey key, out TValue value)
    {
        lock (_lock)
        {
            if (_cache.TryGetValue(key, out var weakRef))
            {
                if (weakRef.TryGetTarget(out value))
                {
                    return true; // Объект еще в памяти
                }
                else
                {
                    // Объект был собран GC - удаляем из кэша
                    _cache.Remove(key);
                }
            }
            
            value = null;
            return false;
        }
    }
    
    // Очистка "мертвых" ссылок
    public void Cleanup()
    {
        lock (_lock)
        {
            var keysToRemove = new List<TKey>();
            
            foreach (var kvp in _cache)
            {
                if (!kvp.Value.TryGetTarget(out _))
                {
                    keysToRemove.Add(kvp.Key);
                }
            }
            
            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
            }
        }
    }
}
```

---

## 9. Метрики и диагностика GC

### Использование GC класса для диагностики

```csharp
using System;

class Program
{
    static void Main()
    {
        // Количество сборок по поколениям
        Console.WriteLine($"Gen 0: {GC.CollectionCount(0)}");
        Console.WriteLine($"Gen 1: {GC.CollectionCount(1)}");
        Console.WriteLine($"Gen 2: {GC.CollectionCount(2)}");
        
        // Использованная память
        long memory = GC.GetTotalMemory(forceFullCollection: false);
        Console.WriteLine($"Total memory: {memory / 1024} KB");
        
        // Максимальное поколение (обычно 2)
        Console.WriteLine($"Max generation: {GC.MaxGeneration}");
        
        // Проверка, выполняется ли сборка
        bool isCollecting = GC.TryStartNoGCRegion(100_000);
        if (isCollecting)
        {
            Console.WriteLine("No GC region started");
            // Критический код
            GC.EndNoGCRegion();
        }
    }
}
```

### Performance Counters для мониторинга GC

```csharp
using System.Diagnostics;

// Использование Performance Counters для мониторинга GC
public class GCMonitor
{
    private readonly PerformanceCounter _gen0Counter;
    private readonly PerformanceCounter _gen1Counter;
    private readonly PerformanceCounter _gen2Counter;
    private readonly PerformanceCounter _memoryCounter;
    
    public GCMonitor()
    {
        _gen0Counter = new PerformanceCounter(".NET CLR Memory", "# Gen 0 Collections", "_Global_");
        _gen1Counter = new PerformanceCounter(".NET CLR Memory", "# Gen 1 Collections", "_Global_");
        _gen2Counter = new PerformanceCounter(".NET CLR Memory", "# Gen 2 Collections", "_Global_");
        _memoryCounter = new PerformanceCounter(".NET CLR Memory", "# Bytes in all Heaps", "_Global_");
    }
    
    public void PrintStats()
    {
        Console.WriteLine($"Gen 0 Collections: {_gen0Counter.NextValue()}");
        Console.WriteLine($"Gen 1 Collections: {_gen1Counter.NextValue()}");
        Console.WriteLine($"Gen 2 Collections: {_gen2Counter.NextValue()}");
        Console.WriteLine($"Memory in Heaps: {_memoryCounter.NextValue() / 1024 / 1024} MB");
    }
}
```

---

## 10. Best Practices

### ✅ Что делать

1. **Используйте using для IDisposable объектов**
   ```csharp
   using (var stream = new FileStream("file.txt", FileMode.Open))
   {
       // Работа со stream
   }
   ```

2. **Минимизируйте количество временных объектов**
   ```csharp
   // Используйте StringBuilder вместо конкатенации строк
   // Используйте ArrayPool для массивов
   // Используйте Object Pooling для часто создаваемых объектов
   ```

3. **Избегайте finalizers если возможно**
   ```csharp
   // Реализуйте IDisposable вместо finalizers
   // Finalizers только для неуправляемых ресурсов
   ```

4. **Правильно реализуйте Dispose Pattern**
   ```csharp
   // Всегда вызывайте GC.SuppressFinalize(this) в Dispose()
   // Различайте управляемые и неуправляемые ресурсы
   ```

### ❌ Чего избегать

1. **Не вызывайте GC.Collect() вручную**
   ```csharp
   // ❌ Плохо
   GC.Collect();
   
   // ✅ Хорошо - позвольте GC работать автоматически
   ```

2. **Не держите ссылки на объекты дольше необходимо**
   ```csharp
   // ❌ Плохо
   var largeObject = LoadLargeObject();
   // ... много кода ...
   // largeObject все еще в памяти
   
   // ✅ Хорошо
   using (var largeObject = LoadLargeObject())
   {
       // Использование
   }
   // largeObject освобожден
   ```

3. **Не создавайте объекты с finalizers без необходимости**
   ```csharp
   // ❌ Плохо - замедляет GC
   class MyClass
   {
       ~MyClass() { } // Не нужен если нет неуправляемых ресурсов
   }
   ```

4. **Не используйте большие объекты (>85KB) без необходимости**
   ```csharp
   // ❌ Плохо - попадает в LOH
   byte[] hugeArray = new byte[100_000];
   
   // ✅ Хорошо - разбить на меньшие части
   byte[] chunk1 = new byte[80_000];
   byte[] chunk2 = new byte[20_000];
   ```

---

## 11. Вопросы для собеседований

### Типичные вопросы и ответы

**Q1: Что такое Garbage Collector и как он работает?**
- GC - автоматический менеджер памяти в .NET
- Работает по принципу Mark & Sweep
- Использует трехпоколенческую систему (Gen 0, 1, 2)
- Освобождает объекты, на которые нет ссылок

**Q2: Что такое поколения (Generations) в GC?**
- Gen 0: молодые объекты, сборка часто
- Gen 1: выжившие после Gen 0, сборка реже
- Gen 2: долгоживущие объекты, сборка редко
- Объекты продвигаются в следующее поколение после выживания в сборке

**Q3: В чем разница между Ephemeral GC и Full GC?**
- Ephemeral GC: собирает Gen 0 и Gen 1, быстрая операция
- Full GC: собирает все поколения включая Gen 2 и LOH, медленная операция

**Q4: Что такое Large Object Heap (LOH)?**
- Специальная область для объектов >= 85KB
- Собирается только вместе с Gen 2
- Не уплотняется (no compaction), только очищается
- Может привести к фрагментации памяти

**Q5: Когда нужно вызывать GC.Collect()?**
- Почти никогда! GC работает автоматически и эффективно
- Только в очень специфических случаях (тестирование, профилирование)
- В продакшене лучше избегать

**Q6: Что такое Finalizer и когда его использовать?**
- Finalizer (~деструктор) вызывается GC перед удалением объекта
- Используется только для освобождения неуправляемых ресурсов
- Замедляет GC, объекты с finalizers удаляются медленнее
- Всегда реализуйте IDisposable вместо finalizers, когда возможно

**Q7: Что такое Weak Reference?**
- Слабая ссылка позволяет GC собирать объект даже при наличии ссылки
- Полезно для кэшей, где объекты могут быть собраны при нехватке памяти
- Используется для избежания циклических ссылок

**Q8: Как оптимизировать работу с GC?**
- Минимизировать количество временных объектов
- Использовать Object Pooling для часто создаваемых объектов
- Использовать ArrayPool<T> для массивов
- Избегать создания больших объектов (>85KB)
- Правильно реализовывать Dispose Pattern
- Избегать finalizers когда возможно

---

## Заключение

Garbage Collector - критически важный компонент .NET, который автоматически управляет памятью. Понимание его работы необходимо для:
- Написания эффективного кода
- Избежания проблем с производительностью
- Правильного управления ресурсами
- Успешного прохождения собеседований

Помните: **GC работает автоматически и эффективно. Доверьтесь ему и не вызывайте GC.Collect() без крайней необходимости!**
