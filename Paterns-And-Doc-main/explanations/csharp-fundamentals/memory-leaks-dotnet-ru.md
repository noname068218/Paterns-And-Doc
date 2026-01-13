# Memory Leaks в .NET

## Введение

Несмотря на то, что .NET использует автоматическое управление памятью через Garbage Collector, **утечки памяти (Memory Leaks)** все еще возможны. Понимание причин утечек, их диагностики и решения критически важно для создания стабильных приложений.

---

## 1. Могут ли быть Memory Leaks в .NET?

**Да!** Хотя GC автоматически освобождает память, утечки возможны из-за:

1. **Удержания ссылок на объекты** (наиболее частая причина)
2. **События (Events) не отписаны**
3. **Статические коллекции**
4. **Кэши без ограничений**
5. **Неосвобожденные неуправляемые ресурсы**
6. **Циклические ссылки с событиями**

---

## 2. Типы Memory Leaks

### Утечка 1: События (Events) - Наиболее частая причина

```csharp
using System;

// ❌ Проблема: Publisher удерживает ссылки на Subscribers
public class EventPublisher
{
    public event EventHandler SomethingHappened;
    
    public void DoSomething()
    {
        SomethingHappened?.Invoke(this, EventArgs.Empty);
    }
}

public class Subscriber
{
    private string _data = new string('x', 1000000); // Большой объект
    
    public Subscriber(EventPublisher publisher)
    {
        // Подписка на событие
        publisher.SomethingHappened += OnSomethingHappened;
    }
    
    private void OnSomethingHappened(object sender, EventArgs e)
    {
        Console.WriteLine("Event received");
    }
    
    // Проблема: если Subscriber больше не нужен, но не отписан от события,
    // EventPublisher все еще держит ссылку на Subscriber через делегат!
}

// Использование
class Program
{
    static void Main()
    {
        var publisher = new EventPublisher();
        
        for (int i = 0; i < 1000; i++)
        {
            var subscriber = new Subscriber(publisher);
            // Subscriber создан, но не отписан!
            // Publisher держит ссылку на все 1000 Subscribers
            // GC не может удалить Subscribers, т.к. есть ссылка из Publisher
        }
        
        // Все 1000 Subscribers остаются в памяти!
    }
}

// ✅ Решение: Отписка от событий
public class GoodSubscriber : IDisposable
{
    private EventPublisher _publisher;
    
    public GoodSubscriber(EventPublisher publisher)
    {
        _publisher = publisher;
        _publisher.SomethingHappened += OnSomethingHappened;
    }
    
    private void OnSomethingHappened(object sender, EventArgs e)
    {
        Console.WriteLine("Event received");
    }
    
    public void Dispose()
    {
        // ВАЖНО: Отписка от события перед удалением
        if (_publisher != null)
        {
            _publisher.SomethingHappened -= OnSomethingHappened;
            _publisher = null;
        }
    }
}

// Использование с using
using (var subscriber = new GoodSubscriber(publisher))
{
    // работа
} // Отписка происходит автоматически
```

### Утечка 2: Статические коллекции

```csharp
using System;
using System.Collections.Generic;

// ❌ Проблема: Статическая коллекция удерживает все объекты
public class Cache
{
    // Статическая коллекция - живет все время жизни приложения
    private static List<LargeObject> _cache = new List<LargeObject>();
    
    public static void Add(LargeObject obj)
    {
        _cache.Add(obj);
        // Объект никогда не будет удален GC, т.к. есть ссылка из статической коллекции!
    }
    
    public static void Clear()
    {
        _cache.Clear(); // Нужно явно очищать
    }
}

public class LargeObject
{
    private byte[] _data = new byte[10_000_000]; // 10MB на объект
}

// Использование
for (int i = 0; i < 100; i++)
{
    var obj = new LargeObject();
    Cache.Add(obj);
    // 100 объектов * 10MB = 1GB памяти занято навсегда!
}

// ✅ Решение 1: Ограничение размера кэша
public class LimitedCache
{
    private static List<LargeObject> _cache = new List<LargeObject>();
    private const int MAX_SIZE = 10;
    
    public static void Add(LargeObject obj)
    {
        _cache.Add(obj);
        
        // Удаляем старые элементы при превышении лимита
        if (_cache.Count > MAX_SIZE)
        {
            _cache.RemoveAt(0); // Удаляем самый старый
        }
    }
}

// ✅ Решение 2: WeakReference (слабые ссылки)
public class WeakCache
{
    private static List<WeakReference<LargeObject>> _cache = new List<WeakReference<LargeObject>>();
    
    public static void Add(LargeObject obj)
    {
        _cache.Add(new WeakReference<LargeObject>(obj));
        // GC может удалить объект, но ссылка останется (IsAlive = false)
    }
    
    public static void Cleanup()
    {
        // Удаляем "мертвые" ссылки
        _cache.RemoveAll(wr => !wr.TryGetTarget(out _));
    }
}
```

### Утечка 3: Таймеры и Background Tasks

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

// ❌ Проблема: Timer держит ссылку на callback
public class DataProcessor
{
    private Timer _timer;
    private byte[] _largeData = new byte[1_000_000];
    
    public DataProcessor()
    {
        // Timer держит ссылку на OnTimerCallback
        // OnTimerCallback - метод экземпляра, значит держит ссылку на DataProcessor
        _timer = new Timer(OnTimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
    }
    
    private void OnTimerCallback(object state)
    {
        // Обработка данных
    }
    
    // Проблема: даже если DataProcessor больше не нужен,
    // Timer продолжает работать и держать ссылку на него!
}

// ✅ Решение: Освобождение Timer
public class GoodDataProcessor : IDisposable
{
    private Timer _timer;
    private byte[] _largeData = new byte[1_000_000];
    
    public GoodDataProcessor()
    {
        _timer = new Timer(OnTimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
    }
    
    private void OnTimerCallback(object state)
    {
        // Обработка данных
    }
    
    public void Dispose()
    {
        // ВАЖНО: Останавливаем и освобождаем Timer
        _timer?.Dispose();
        _timer = null;
    }
}

// Аналогично для Task
public class TaskProcessor : IDisposable
{
    private CancellationTokenSource _cancellationTokenSource;
    private Task _backgroundTask;
    
    public TaskProcessor()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _backgroundTask = Task.Run(async () =>
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                // Фоновая работа
                await Task.Delay(1000, _cancellationTokenSource.Token);
            }
        });
    }
    
    public void Dispose()
    {
        _cancellationTokenSource?.Cancel();
        _backgroundTask?.Wait(TimeSpan.FromSeconds(5));
        _cancellationTokenSource?.Dispose();
    }
}
```

### Утечка 4: Циклические ссылки с событиями

```csharp
using System;

// ❌ Проблема: Циклическая ссылка через события
public class Parent
{
    public event EventHandler ChildAdded;
    private Child _child;
    
    public void SetChild(Child child)
    {
        _child = child;
        _child.Parent = this; // Parent ссылается на Child
        ChildAdded?.Invoke(this, EventArgs.Empty);
    }
}

public class Child
{
    private Parent _parent;
    private byte[] _largeData = new byte[1_000_000];
    
    public Parent Parent
    {
        get => _parent;
        set
        {
            _parent = value;
            // Если Parent имеет событие, на которое подписан Child,
            // возникает циклическая ссылка!
        }
    }
}

// ✅ Решение: WeakReference или явное управление ссылками
public class GoodChild
{
    private WeakReference<Parent> _parentRef; // Слабая ссылка
    private byte[] _largeData = new byte[1_000_000];
    
    public Parent Parent
    {
        get
        {
            if (_parentRef?.TryGetTarget(out Parent parent) == true)
                return parent;
            return null;
        }
        set => _parentRef = value != null ? new WeakReference<Parent>(value) : null;
    }
}
```

### Утечка 5: Неосвобожденные неуправляемые ресурсы

```csharp
using System;
using System.Runtime.InteropServices;

// ❌ Проблема: Неуправляемые ресурсы не освобождаются
public class NativeResourceWrapper
{
    private IntPtr _nativeHandle;
    
    public NativeResourceWrapper()
    {
        _nativeHandle = AllocateNativeMemory(1024);
    }
    
    [DllImport("kernel32.dll")]
    private static extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
    
    private IntPtr AllocateNativeMemory(int size)
    {
        return VirtualAlloc(IntPtr.Zero, (uint)size, 0x1000, 0x04); // MEM_COMMIT, PAGE_READWRITE
    }
    
    // Проблема: Если объект удаляется GC, но Dispose не вызван,
    // неуправляемая память не освободится!
}

// ✅ Решение: Правильная реализация IDisposable
public class GoodNativeResourceWrapper : IDisposable
{
    private IntPtr _nativeHandle;
    private bool _disposed = false;
    
    public GoodNativeResourceWrapper()
    {
        _nativeHandle = AllocateNativeMemory(1024);
    }
    
    [DllImport("kernel32.dll")]
    private static extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
    
    [DllImport("kernel32.dll")]
    private static extern bool VirtualFree(IntPtr lpAddress, uint dwSize, uint dwFreeType);
    
    private IntPtr AllocateNativeMemory(int size)
    {
        return VirtualAlloc(IntPtr.Zero, (uint)size, 0x1000, 0x04);
    }
    
    private void FreeNativeMemory(IntPtr handle)
    {
        if (handle != IntPtr.Zero)
        {
            VirtualFree(handle, 0, 0x8000); // MEM_RELEASE
        }
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            FreeNativeMemory(_nativeHandle);
            _nativeHandle = IntPtr.Zero;
            _disposed = true;
        }
    }
    
    ~GoodNativeResourceWrapper()
    {
        Dispose(false); // Finalizer освобождает неуправляемые ресурсы
    }
}
```

---

## 3. Диагностика Memory Leaks

### Использование Visual Studio Diagnostic Tools

```csharp
using System;
using System.Collections.Generic;
using System.Threading;

class MemoryLeakDiagnostics
{
    static void Main()
    {
        var list = new List<byte[]>();
        
        // Симуляция утечки памяти
        for (int i = 0; i < 1000; i++)
        {
            list.Add(new byte[1_000_000]); // 1MB каждый
            Thread.Sleep(10);
            
            // В Visual Studio Diagnostic Tools можно увидеть:
            // - Постоянный рост памяти
            // - Объекты, которые не собираются GC
        }
    }
}
```

### Использование dotMemory, PerfView, или dotnet-counters

```csharp
// Установка dotnet-counters
// dotnet tool install -g dotnet-counters

// Мониторинг в реальном времени
// dotnet-counters monitor --process-id <pid> System.Runtime

// Проверка конкретных метрик
// - GC Heap Size - должен стабилизироваться
// - Gen 0/1/2 Collections - частота сборок
// - Exception Count - не должно расти постоянно
```

### Ручная диагностика через код

```csharp
using System;
using System.Diagnostics;
using System.Threading;

class ManualDiagnostics
{
    static void CheckMemory()
    {
        // Принудительная сборка мусора
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        
        // Получение использованной памяти
        long memory = GC.GetTotalMemory(false);
        Console.WriteLine($"Memory after GC: {memory / 1024 / 1024} MB");
        
        // Количество сборок по поколениям
        Console.WriteLine($"Gen 0: {GC.CollectionCount(0)}");
        Console.WriteLine($"Gen 1: {GC.CollectionCount(1)}");
        Console.WriteLine($"Gen 2: {GC.CollectionCount(2)}");
    }
    
    static void Main()
    {
        CheckMemory();
        
        // Выполнение операций
        CreateObjects();
        
        CheckMemory();
        
        // Если память не освободилась - возможна утечка
    }
    
    static void CreateObjects()
    {
        var list = new List<byte[]>();
        for (int i = 0; i < 100; i++)
        {
            list.Add(new byte[1_000_000]);
        }
        // list выходит из области видимости, но если есть ссылки - не освободится
    }
}
```

---

## 4. Best Practices для предотвращения утечек

### ✅ Что делать

1. **Всегда отписывайтесь от событий**
   ```csharp
   public void Dispose()
   {
       _publisher.Event -= Handler;
   }
   ```

2. **Ограничивайте размер кэшей**
   ```csharp
   if (_cache.Count > MAX_SIZE) _cache.RemoveAt(0);
   ```

3. **Используйте WeakReference для кэшей**
   ```csharp
   var weakRef = new WeakReference<MyObject>(obj);
   ```

4. **Освобождайте Timer, CancellationTokenSource**
   ```csharp
   _timer?.Dispose();
   _cancellationTokenSource?.Dispose();
   ```

5. **Реализуйте IDisposable для классов с ресурсами**
   ```csharp
   public class MyClass : IDisposable { }
   ```

6. **Используйте using для IDisposable**
   ```csharp
   using (var resource = new MyResource()) { }
   ```

### ❌ Чего избегать

1. **Не храните ссылки на объекты в статических коллекциях без ограничений**
   ```csharp
   // ❌ Плохо
   private static List<object> _cache = new List<object>();
   ```

2. **Не забывайте отписываться от событий**
   ```csharp
   // ❌ Плохо
   publisher.Event += Handler;
   // Забыли: publisher.Event -= Handler;
   ```

3. **Не создавайте циклические ссылки**
   ```csharp
   // Избегайте Parent <-> Child с событиями
   ```

4. **Не держите ссылки на большие объекты дольше необходимо**
   ```csharp
   // Освобождайте ссылки явно: obj = null;
   ```

---

## 5. Вопросы для собеседований

### Типичные вопросы и ответы

**Q1: Возможны ли утечки памяти в .NET?**
- Да, несмотря на GC
- Основные причины: события, статические коллекции, таймеры, циклические ссылки

**Q2: Какова самая частая причина утечек памяти?**
- Неотписанные события (Event Handlers)
- Publisher держит ссылку на Subscriber через делегат

**Q3: Как диагностировать утечки памяти?**
- Visual Studio Diagnostic Tools
- dotMemory, PerfView, dotnet-counters
- Мониторинг GC.GetTotalMemory()
- Анализ частоты сборок мусора

**Q4: Как предотвратить утечки памяти?**
- Отписываться от событий в Dispose()
- Ограничивать размер кэшей
- Использовать WeakReference
- Правильно реализовывать IDisposable
- Освобождать Timer, CancellationTokenSource

**Q5: Что такое WeakReference и когда использовать?**
- Слабая ссылка, позволяющая GC собирать объект
- Полезно для кэшей, где объекты могут быть собраны при нехватке памяти

**Q6: Почему статические коллекции могут вызывать утечки?**
- Статические переменные живут все время жизни приложения
- Объекты в статических коллекциях никогда не удаляются GC
- Решение: ограничение размера или использование WeakReference

---

## Заключение

Memory Leaks в .NET возможны, несмотря на автоматическое управление памятью:
- **Основная причина**: удержание ссылок (события, статические коллекции)
- **Диагностика**: инструменты профилирования, мониторинг памяти
- **Решение**: правильное управление жизненным циклом объектов, отписка от событий, ограничение кэшей

Помните: **всегда отписывайтесь от событий и освобождайте ресурсы в Dispose()!**
