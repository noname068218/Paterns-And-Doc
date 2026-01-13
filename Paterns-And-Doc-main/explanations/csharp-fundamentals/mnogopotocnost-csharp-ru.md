# Многопоточность в C#

## Введение

**Многопоточность** — это способность программы выполнять несколько операций одновременно, используя несколько потоков (threads) выполнения. В C# многопоточность позволяет улучшить производительность приложений, особенно при работе с I/O-операциями и вычислениями, которые можно распараллелить.

---

## 1. Основы многопоточности

### Что такое поток (Thread)?

**Поток (Thread)** — это наименьшая единица выполнения в программе. Каждый поток имеет свой собственный стек вызовов и может выполнять код независимо от других потоков. Все потоки в рамках одного процесса разделяют общую память.

```
┌─────────────────────────────────────────────┐
│  PROCESS (Процесс)                         │
│  ┌──────────────────────────────────────┐ │
│  │  Shared Memory (Общая память)         │ │
│  │  ┌──────────┐  ┌──────────┐         │ │
│  │  │ Thread 1 │  │ Thread 2 │         │ │
│  │  │ Stack    │  │ Stack    │         │ │
│  │  └──────────┘  └──────────┘         │ │
│  │  ┌──────────┐                        │ │
│  │  │ Thread 3 │                        │ │
│  │  │ Stack    │                        │ │
│  │  └──────────┘                        │ │
│  └──────────────────────────────────────┘ │
└─────────────────────────────────────────────┘
```

#### Пример создания потока

```csharp
using System;
using System.Threading;

// Метод, который будет выполняться в отдельном потоке
static void WorkerMethod()
{
    for (int i = 0; i < 5; i++)
    {
        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: {i}");
        Thread.Sleep(1000); // Имитация работы
    }
}

static void Main()
{
    // Создание нового потока
    Thread workerThread = new Thread(WorkerMethod);
    
    // Запуск потока
    workerThread.Start();
    
    // Главный поток продолжает работать параллельно
    for (int i = 0; i < 3; i++)
    {
        Console.WriteLine($"Main Thread {Thread.CurrentThread.ManagedThreadId}: {i}");
        Thread.Sleep(500);
    }
    
    // Ожидание завершения потока
    workerThread.Join();
    Console.WriteLine("All threads completed");
}
```

**Вывод:**
```
Main Thread 1: 0
Thread 3: 0
Main Thread 1: 1
Thread 3: 1
Main Thread 1: 2
Thread 3: 2
Thread 3: 3
Thread 3: 4
All threads completed
```

---

### Race Condition (Состояние гонки)

**Race Condition** возникает, когда результат выполнения программы зависит от относительного порядка выполнения потоков. Это происходит, когда несколько потоков обращаются к одной и той же переменной без синхронизации.

#### Пример Race Condition

```csharp
using System;
using System.Threading;

class Counter
{
    private int _count = 0;
    
    // Небезопасный метод инкремента
    public void Increment()
    {
        // Это не атомарная операция!
        // Состоит из трех шагов:
        // 1. Прочитать значение _count
        // 2. Увеличить его на 1
        // 3. Записать обратно
        _count++;
    }
    
    public int GetCount() => _count;
}

class Program
{
    static void Main()
    {
        Counter counter = new Counter();
        
        // Создаем 10 потоков, каждый инкрементирует счетчик 1000 раз
        Thread[] threads = new Thread[10];
        
        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(() =>
            {
                for (int j = 0; j < 1000; j++)
                {
                    counter.Increment();
                }
            });
            threads[i].Start();
        }
        
        // Ждем завершения всех потоков
        foreach (var thread in threads)
        {
            thread.Join();
        }
        
        // Ожидаем 10000, но получим меньше из-за race condition
        Console.WriteLine($"Expected: 10000, Actual: {counter.GetCount()}");
        // Вывод может быть: Expected: 10000, Actual: 8567 (или другое непредсказуемое значение)
    }
}
```

#### Почему происходит Race Condition?

```
Время →  Thread 1          Thread 2          Значение _count
─────────────────────────────────────────────────────────────
T1     │  Прочитать: 5                     │      5
T2     │  Прочитать: 5                     │      5
T3     │  Вычислить: 5 + 1 = 6             │      5
T4     │              Вычислить: 5 + 1 = 6 │      5
T5     │  Записать: 6                      │      6
T6     │              Записать: 6          │      6
─────────────────────────────────────────────────────────────
Результат: Оба потока инкрементировали, но значение увеличилось только на 1!
Ожидалось: 7, получили: 6
```

---

### Critical Section (Критическая секция)

**Critical Section (Критическая секция)** — это участок кода, который должен выполняться только одним потоком в данный момент времени. Для защиты критических секций используются механизмы синхронизации.

#### Использование lock для защиты критической секции

```csharp
using System;
using System.Threading;

class SafeCounter
{
    private int _count = 0;
    private readonly object _lockObject = new object(); // Объект для блокировки
    
    // Безопасный метод инкремента
    public void Increment()
    {
        // Критическая секция - только один поток может выполнить этот блок одновременно
        lock (_lockObject)
        {
            _count++;
        }
    }
    
    public int GetCount() => _count;
}

class Program
{
    static void Main()
    {
        SafeCounter counter = new SafeCounter();
        
        Thread[] threads = new Thread[10];
        
        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(() =>
            {
                for (int j = 0; j < 1000; j++)
                {
                    counter.Increment();
                }
            });
            threads[i].Start();
        }
        
        foreach (var thread in threads)
        {
            thread.Join();
        }
        
        // Теперь всегда получим правильное значение
        Console.WriteLine($"Expected: 10000, Actual: {counter.GetCount()}");
        // Вывод: Expected: 10000, Actual: 10000 ✅
    }
}
```

#### Как работает lock?

```csharp
// lock (obj) - это синтаксический сахар для следующего кода:

object obj = _lockObject;
bool lockTaken = false;

try
{
    // Попытка захватить блокировку
    Monitor.Enter(obj, ref lockTaken);
    
    // Критическая секция
    _count++;
}
finally
{
    // Освобождение блокировки в любом случае
    if (lockTaken)
    {
        Monitor.Exit(obj);
    }
}
```

#### Другие способы защиты критической секции

##### 1. Interlocked (для атомарных операций)

```csharp
using System.Threading;

class Counter
{
    private int _count = 0;
    
    // Атомарный инкремент (не требует lock)
    public void Increment()
    {
        Interlocked.Increment(ref _count);
    }
    
    public int GetCount() => _count;
}
```

**Interlocked операции:**
- `Interlocked.Increment(ref variable)` - атомарный инкремент
- `Interlocked.Decrement(ref variable)` - атомарный декремент
- `Interlocked.Add(ref variable, value)` - атомарное сложение
- `Interlocked.Exchange(ref variable, value)` - атомарный обмен
- `Interlocked.CompareExchange(ref variable, newValue, comparand)` - условный обмен

##### 2. Monitor

```csharp
private readonly object _lockObject = new object();

public void SafeMethod()
{
    Monitor.Enter(_lockObject);
    try
    {
        // Критическая секция
        _count++;
    }
    finally
    {
        Monitor.Exit(_lockObject);
    }
}
```

##### 3. Mutex

```csharp
using System.Threading;

private static Mutex _mutex = new Mutex();

public void SafeMethod()
{
    _mutex.WaitOne(); // Ожидание доступа
    try
    {
        // Критическая секция
        _count++;
    }
    finally
    {
        _mutex.ReleaseMutex(); // Освобождение
    }
}
```

##### 4. Semaphore / SemaphoreSlim

```csharp
using System.Threading;

// Semaphore позволяет N потокам одновременно войти в критическую секцию
private static SemaphoreSlim _semaphore = new SemaphoreSlim(3); // Максимум 3 потока

public async Task SafeMethodAsync()
{
    await _semaphore.WaitAsync(); // Ожидание доступа
    try
    {
        // Критическая секция (максимум 3 потока одновременно)
        await DoWorkAsync();
    }
    finally
    {
        _semaphore.Release(); // Освобождение
    }
}
```

---

### Deadlock (Взаимная блокировка)

**Deadlock** возникает, когда два или более потока заблокированы навсегда, ожидая друг друга. Это происходит, когда потоки удерживают блокировки и одновременно пытаются захватить блокировки, удерживаемые другими потоками.

#### Классический пример Deadlock

```csharp
using System;
using System.Threading;

class BankAccount
{
    private decimal _balance = 1000;
    private readonly object _lock1 = new object();
    
    public void Transfer(BankAccount target, decimal amount)
    {
        // Поток 1: захватывает _lock1, затем пытается захватить target._lock1
        // Поток 2: захватывает target._lock1, затем пытается захватить _lock1
        // → DEADLOCK!
        
        lock (_lock1)
        {
            Thread.Sleep(100); // Имитация задержки (увеличивает вероятность deadlock)
            
            lock (target._lock1)
            {
                if (_balance >= amount)
                {
                    _balance -= amount;
                    target._balance += amount;
                    Console.WriteLine($"Transferred {amount}");
                }
            }
        }
    }
    
    public decimal GetBalance() => _balance;
}

class Program
{
    static void Main()
    {
        BankAccount account1 = new BankAccount();
        BankAccount account2 = new BankAccount();
        
        // Поток 1: перевод с account1 на account2
        Thread thread1 = new Thread(() =>
        {
            for (int i = 0; i < 10; i++)
            {
                account1.Transfer(account2, 100);
            }
        });
        
        // Поток 2: перевод с account2 на account1
        Thread thread2 = new Thread(() =>
        {
            for (int i = 0; i < 10; i++)
            {
                account2.Transfer(account1, 100);
            }
        });
        
        thread1.Start();
        thread2.Start();
        
        thread1.Join(2000); // Ждем максимум 2 секунды
        thread2.Join(2000);
        
        if (thread1.IsAlive || thread2.IsAlive)
        {
            Console.WriteLine("DEADLOCK detected! Threads are stuck.");
        }
    }
}
```

#### Диаграмма Deadlock

```
Поток 1                          Поток 2
─────────────────────────────────────────────────
Захватывает lock1                Захватывает lock2
         │                                │
         │                                │
Пытается захватить lock2 ──────────────┐ │
         │                              │ │
         │                              │ │
         │                    Пытается захватить lock1
         │                              │ │
         │                              │ │
    ════════════════════════════════════════════
    DEADLOCK! Оба потока ждут друг друга
    ════════════════════════════════════════════
```

#### Решение: упорядоченная блокировка

```csharp
class BankAccount
{
    private decimal _balance = 1000;
    private readonly int _id; // Уникальный идентификатор для упорядочивания
    
    public BankAccount(int id)
    {
        _id = id;
    }
    
    public void Transfer(BankAccount target, decimal amount)
    {
        // Всегда захватываем блокировки в одном и том же порядке
        // (по id аккаунта, чтобы избежать deadlock)
        object firstLock = _id < target._id ? this : target;
        object secondLock = _id < target._id ? target : this;
        
        lock (firstLock)
        {
            lock (secondLock)
            {
                if (_balance >= amount)
                {
                    _balance -= amount;
                    target._balance += amount;
                    Console.WriteLine($"Transferred {amount}");
                }
            }
        }
    }
    
    public decimal GetBalance() => _balance;
}
```

#### Альтернативное решение: Monitor.TryEnter с таймаутом

```csharp
public void Transfer(BankAccount target, decimal amount)
{
    bool lock1Taken = false;
    bool lock2Taken = false;
    
    try
    {
        Monitor.TryEnter(_lock1, TimeSpan.FromSeconds(1), ref lock1Taken);
        if (!lock1Taken)
        {
            throw new TimeoutException("Could not acquire lock1");
        }
        
        Monitor.TryEnter(target._lock1, TimeSpan.FromSeconds(1), ref lock2Taken);
        if (!lock2Taken)
        {
            throw new TimeoutException("Could not acquire lock2");
        }
        
        if (_balance >= amount)
        {
            _balance -= amount;
            target._balance += amount;
        }
    }
    finally
    {
        if (lock2Taken) Monitor.Exit(target._lock1);
        if (lock1Taken) Monitor.Exit(_lock1);
    }
}
```

---

### Thread Safety (Потокобезопасность)

**Thread Safety** означает, что код безопасен для использования несколькими потоками одновременно, не приводя к race condition, deadlock или другим проблемам синхронизации.

#### Классификация коллекций по потокобезопасности

##### Непотокобезопасные коллекции

```csharp
using System.Collections.Generic;

// ❌ НЕ потокобезопасны
List<int> list = new List<int>(); // НЕ thread-safe
Dictionary<string, int> dict = new Dictionary<string, int>(); // НЕ thread-safe
Queue<int> queue = new Queue<int>(); // НЕ thread-safe

// Опасное использование
Thread thread1 = new Thread(() =>
{
    for (int i = 0; i < 1000; i++)
    {
        list.Add(i); // Race condition!
    }
});

Thread thread2 = new Thread(() =>
{
    for (int i = 0; i < 1000; i++)
    {
        list.Add(i); // Race condition!
    }
});

thread1.Start();
thread2.Start();
thread1.Join();
thread2.Join();
// Результат непредсказуем: может быть исключение или потеря данных
```

##### Потокобезопасные коллекции

```csharp
using System.Collections.Concurrent;

// ✅ Потокобезопасны (из пространства имен System.Collections.Concurrent)
ConcurrentBag<int> bag = new ConcurrentBag<int>();
ConcurrentDictionary<string, int> dict = new ConcurrentDictionary<string, int>();
ConcurrentQueue<int> queue = new ConcurrentQueue<int>();
ConcurrentStack<int> stack = new ConcurrentStack<int>();

// Безопасное использование
Thread thread1 = new Thread(() =>
{
    for (int i = 0; i < 1000; i++)
    {
        bag.Add(i); // Thread-safe!
    }
});

Thread thread2 = new Thread(() =>
{
    for (int i = 0; i < 1000; i++)
    {
        bag.Add(i); // Thread-safe!
    }
});

thread1.Start();
thread2.Start();
thread1.Join();
thread2.Join();
// Результат: 2000 элементов без проблем
```

#### Примеры потокобезопасных коллекций

##### ConcurrentDictionary

```csharp
using System.Collections.Concurrent;

ConcurrentDictionary<string, int> scores = new ConcurrentDictionary<string, int>();

// Потокобезопасные операции
Parallel.For(0, 1000, i =>
{
    string key = $"Player{i % 10}";
    
    // AddOrUpdate - атомарная операция
    scores.AddOrUpdate(key, 1, (key, oldValue) => oldValue + 1);
});

// GetOrAdd - атомарная операция
int value = scores.GetOrAdd("Player5", 0);

foreach (var kvp in scores)
{
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
}
```

##### ConcurrentQueue

```csharp
using System.Collections.Concurrent;

ConcurrentQueue<int> queue = new ConcurrentQueue<int>();

// Producer - добавляет элементы
Thread producer = new Thread(() =>
{
    for (int i = 0; i < 100; i++)
    {
        queue.Enqueue(i);
        Thread.Sleep(10);
    }
});

// Consumer - извлекает элементы
Thread consumer = new Thread(() =>
{
    while (true)
    {
        if (queue.TryDequeue(out int item))
        {
            Console.WriteLine($"Dequeued: {item}");
        }
        else
        {
            Thread.Sleep(10);
        }
    }
});

producer.Start();
consumer.Start();
producer.Join();
```

##### ConcurrentBag

```csharp
using System.Collections.Concurrent;

ConcurrentBag<int> bag = new ConcurrentBag<int>();

Parallel.For(0, 1000, i =>
{
    bag.Add(i);
});

Console.WriteLine($"Count: {bag.Count}"); // 1000

// TryTake - извлекает элемент
if (bag.TryTake(out int item))
{
    Console.WriteLine($"Took: {item}");
}
```

#### Thread-safe счетчик с использованием lock-free техники

```csharp
using System.Threading;

public class ThreadSafeCounter
{
    private int _count = 0;
    
    // Вариант 1: Использование Interlocked (lock-free)
    public void Increment()
    {
        Interlocked.Increment(ref _count);
    }
    
    // Вариант 2: Использование lock
    private readonly object _lock = new object();
    public void IncrementWithLock()
    {
        lock (_lock)
        {
            _count++;
        }
    }
    
    // Вариант 3: Использование ReaderWriterLockSlim (для чтения/записи)
    private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();
    
    public int ReadCount()
    {
        _rwLock.EnterReadLock();
        try
        {
            return _count; // Множественные чтения могут выполняться параллельно
        }
        finally
        {
            _rwLock.ExitReadLock();
        }
    }
    
    public void WriteIncrement()
    {
        _rwLock.EnterWriteLock();
        try
        {
            _count++; // Только одна запись в данный момент
        }
        finally
        {
            _rwLock.ExitWriteLock();
        }
    }
}
```

---

## 2. Task Parallel Library (TPL)

**Task Parallel Library (TPL)** — это современный способ работы с многопоточностью в .NET. TPL предоставляет высокоуровневую абстракцию над потоками и позволяет легко создавать параллельные и асинхронные операции.

### Task vs Thread: Основные отличия

| Аспект | Thread | Task |
|--------|--------|------|
| **Уровень абстракции** | Низкоуровневый | Высокоуровневый |
| **Управление ресурсами** | Ручное управление | Автоматическое через ThreadPool |
| **Результат выполнения** | Нет возвращаемого значения | Возвращает Task<T> с результатом |
| **Обработка исключений** | Сложная | Встроенная поддержка |
| **Отмена операций** | Нет встроенной поддержки | CancellationToken |
| **Продолжения** | Нет встроенной поддержки | ContinueWith, await |
| **Стоимость создания** | Высокая | Низкая (использует ThreadPool) |

#### Пример: Thread

```csharp
using System;
using System.Threading;

// Создание потока
Thread thread = new Thread(() =>
{
    Console.WriteLine("Thread started");
    Thread.Sleep(1000);
    Console.WriteLine("Thread completed");
});

thread.Start();
thread.Join(); // Ожидание завершения
```

#### Пример: Task

```csharp
using System;
using System.Threading.Tasks;

// Создание задачи
Task task = Task.Run(() =>
{
    Console.WriteLine("Task started");
    Thread.Sleep(1000);
    Console.WriteLine("Task completed");
});

await task; // Ожидание завершения

// Task с возвращаемым значением
Task<int> taskWithResult = Task.Run(() =>
{
    return 42;
});

int result = await taskWithResult;
Console.WriteLine($"Result: {result}"); // 42
```

---

### Task.Run

`Task.Run` — это основной способ выполнения кода на пуле потоков. Он помещает задачу в очередь ThreadPool и возвращает Task, представляющий эту операцию.

#### Базовое использование Task.Run

```csharp
using System;
using System.Threading.Tasks;

// Выполнение синхронного метода асинхронно
Task task = Task.Run(() =>
{
    // CPU-intensive операция
    int sum = 0;
    for (int i = 0; i < 1000000; i++)
    {
        sum += i;
    }
    Console.WriteLine($"Sum: {sum}");
});

await task;
```

#### Task.Run с возвращаемым значением

```csharp
// Task<T> - задача с возвращаемым значением
Task<int> calculateTask = Task.Run(() =>
{
    int result = 0;
    for (int i = 0; i < 1000000; i++)
    {
        result += i;
    }
    return result;
});

int result = await calculateTask;
Console.WriteLine($"Result: {result}");
```

#### Task.Run для параллельного выполнения

```csharp
using System;
using System.Linq;
using System.Threading.Tasks;

// Выполнение нескольких задач параллельно
Task<int> task1 = Task.Run(() => CalculateSum(1, 1000000));
Task<int> task2 = Task.Run(() => CalculateSum(1000001, 2000000));
Task<int> task3 = Task.Run(() => CalculateSum(2000001, 3000000));

// Ожидание всех задач
await Task.WhenAll(task1, task2, task3);

int total = task1.Result + task2.Result + task3.Result;
Console.WriteLine($"Total: {total}");

static int CalculateSum(int start, int end)
{
    int sum = 0;
    for (int i = start; i <= end; i++)
    {
        sum += i;
    }
    return sum;
}
```

#### Task.Run с обработкой исключений

```csharp
Task task = Task.Run(() =>
{
    throw new InvalidOperationException("Something went wrong!");
});

try
{
    await task;
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Exception caught: {ex.Message}");
}
```

---

### Task.Factory.StartNew (почему не надо использовать)

`Task.Factory.StartNew` — это старый способ создания задач. Хотя он более гибкий, чем `Task.Run`, он может привести к неожиданному поведению.

#### Проблемы с Task.Factory.StartNew

##### 1. Неправильное использование TaskCreationOptions

```csharp
// ❌ ПРОБЛЕМА: Task.Factory.StartNew может не использовать async/await правильно
Task task = Task.Factory.StartNew(async () =>
{
    await Task.Delay(1000);
    return 42;
});

// task - это Task<Task<int>>, а не Task<int>!
// Нужно разворачивать вложенную задачу
int result = await await task; // Неудобно!
```

##### 2. Task.Run vs Task.Factory.StartNew

```csharp
// ✅ ПРАВИЛЬНО: Task.Run автоматически разворачивает задачи
Task<int> task1 = Task.Run(async () =>
{
    await Task.Delay(1000);
    return 42;
});

int result = await task1; // Просто и понятно

// ❌ НЕПРАВИЛЬНО: Task.Factory.StartNew требует явного разворачивания
Task<Task<int>> task2 = Task.Factory.StartNew(async () =>
{
    await Task.Delay(1000);
    return 42;
});

int result2 = await await task2; // Двойной await
```

##### 3. Различия в планировании

```csharp
// Task.Run всегда использует TaskScheduler.Default (ThreadPool)
Task task1 = Task.Run(() => Console.WriteLine("Uses ThreadPool"));

// Task.Factory.StartNew может использовать другой планировщик
Task task2 = Task.Factory.StartNew(() => Console.WriteLine("May use different scheduler"),
    TaskCreationOptions.None,
    TaskScheduler.FromCurrentSynchronizationContext()); // Может быть UI scheduler
```

#### Когда можно использовать Task.Factory.StartNew?

Только в очень специфических случаях:

```csharp
// 1. Когда нужен специфический TaskScheduler
Task task = Task.Factory.StartNew(
    () => DoWork(),
    CancellationToken.None,
    TaskCreationOptions.LongRunning, // Для длительных задач
    TaskScheduler.Default
);

// 2. Когда нужна привязка к родительской задаче (для структурированного параллелизма)
Task parentTask = Task.Factory.StartNew(() =>
{
    Task childTask = Task.Factory.StartNew(() =>
    {
        // Дочерняя задача
    }, TaskCreationOptions.AttachedToParent);
});
```

#### Рекомендация

**Всегда используйте `Task.Run` вместо `Task.Factory.StartNew`**, если только у вас нет очень специфической причины использовать последний.

---

### ThreadPool

**ThreadPool** — это пул рабочих потоков, управляемый .NET Runtime. Он автоматически управляет созданием, уничтожением и переиспользованием потоков, что делает его более эффективным, чем создание новых потоков вручную.

#### Как работает ThreadPool?

```
ThreadPool
├── Минимальное количество потоков (обычно = количество CPU ядер)
├── Максимальное количество потоков (зависит от конфигурации)
└── Очередь задач (work items queue)

Когда Task.Run вызывается:
1. Задача добавляется в очередь ThreadPool
2. Если есть свободный поток - он берет задачу
3. Если нет свободных потоков и не достигнут максимум - создается новый поток
4. После выполнения потока он возвращается в пул для переиспользования
```

#### Пример работы ThreadPool

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

// Получение информации о ThreadPool
int workerThreads, completionPortThreads;
ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
Console.WriteLine($"Available worker threads: {workerThreads}");

// Добавление задач в ThreadPool через Task.Run
for (int i = 0; i < 10; i++)
{
    int taskId = i;
    Task.Run(() =>
    {
        Console.WriteLine($"Task {taskId} executed on thread {Thread.CurrentThread.ManagedThreadId}");
        Thread.Sleep(1000);
    });
}

Thread.Sleep(2000);
```

#### Настройка ThreadPool

```csharp
// Установка минимального и максимального количества потоков
int minWorkerThreads, minCompletionPortThreads;
int maxWorkerThreads, maxCompletionPortThreads;

ThreadPool.GetMinThreads(out minWorkerThreads, out minCompletionPortThreads);
ThreadPool.GetMaxThreads(out maxWorkerThreads, out maxCompletionPortThreads);

Console.WriteLine($"Min threads: {minWorkerThreads}, Max threads: {maxWorkerThreads}");

// Установка новых значений (осторожно!)
bool success = ThreadPool.SetMinThreads(10, 10);
Console.WriteLine($"SetMinThreads success: {success}");
```

#### ThreadPool vs создание потоков вручную

```csharp
// ❌ Плохо: Создание потоков вручную для коротких задач
for (int i = 0; i < 1000; i++)
{
    Thread thread = new Thread(() =>
    {
        // Короткая задача
        Thread.Sleep(100);
    });
    thread.Start();
    // Создание 1000 потоков очень дорого!
}

// ✅ Хорошо: Использование ThreadPool через Task.Run
for (int i = 0; i < 1000; i++)
{
    Task.Run(() =>
    {
        // Короткая задача
        Thread.Sleep(100);
    });
    // Потоки переиспользуются из пула
}
```

---

### Жизненный цикл Task

Задача (Task) проходит через несколько состояний в течение своего жизненного цикла.

#### Состояния Task

```
Created → WaitingToRun → Running → RanToCompletion
                              ↓
                         Faulted (Exception)
                              ↓
                         Canceled (Cancellation)
```

#### Статусы Task

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

// 1. Created (создана, но не запущена)
Task<int> task = new Task<int>(() => 42);
Console.WriteLine($"Status: {task.Status}"); // Created

// 2. Запуск задачи
task.Start();
Console.WriteLine($"Status: {task.Status}"); // WaitingToRun или Running

// 3. Ожидание завершения
int result = await task;
Console.WriteLine($"Status: {task.Status}"); // RanToCompletion
Console.WriteLine($"Result: {result}");

// 4. Задача с исключением
Task faultedTask = Task.Run(() =>
{
    throw new Exception("Error!");
});

try
{
    await faultedTask;
}
catch (Exception ex)
{
    Console.WriteLine($"Status: {faultedTask.Status}"); // Faulted
    Console.WriteLine($"Exception: {ex.Message}");
}

// 5. Отмененная задача
CancellationTokenSource cts = new CancellationTokenSource();
Task canceledTask = Task.Run(() =>
{
    Thread.Sleep(5000);
    cts.Token.ThrowIfCancellationRequested();
}, cts.Token);

cts.CancelAfter(1000);

try
{
    await canceledTask;
}
catch (OperationCanceledException)
{
    Console.WriteLine($"Status: {canceledTask.Status}"); // Canceled
}
```

#### Все возможные статусы Task

```csharp
public enum TaskStatus
{
    Created = 0,              // Задача создана, но еще не запланирована
    WaitingForActivation = 1, // Задача ожидает активации (например, продолжение)
    WaitingToRun = 2,         // Задача запланирована, но еще не выполняется
    Running = 3,              // Задача выполняется
    WaitingForChildrenToComplete = 4, // Задача ожидает завершения дочерних задач
    RanToCompletion = 5,      // Задача успешно завершена
    Canceled = 6,             // Задача была отменена
    Faulted = 7               // Задача завершилась с исключением
}
```

---

### Где выполняется Task?

Понимание того, где выполняется Task, критически важно для правильного использования многопоточности.

#### Контекст выполнения Task

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

// 1. Task.Run - всегда выполняется на ThreadPool
Task.Run(() =>
{
    Console.WriteLine($"ThreadPool thread: {Thread.CurrentThread.IsThreadPoolThread}"); // True
    Console.WriteLine($"Thread ID: {Thread.CurrentThread.ManagedThreadId}");
});

// 2. Task без явного планирования - может выполняться синхронно
Task task = new Task(() =>
{
    Console.WriteLine($"ThreadPool thread: {Thread.CurrentThread.IsThreadPoolThread}");
});
task.RunSynchronously(); // Выполняется в текущем потоке
Console.WriteLine($"Synchronous execution: {task.Status}"); // RanToCompletion

// 3. async/await - продолжение на захваченном контексте
async Task ExampleAsync()
{
    Console.WriteLine($"Before await - Thread: {Thread.CurrentThread.ManagedThreadId}");
    
    await Task.Delay(1000); // После await может быть другой поток
    
    Console.WriteLine($"After await - Thread: {Thread.CurrentThread.ManagedThreadId}");
    Console.WriteLine($"ThreadPool: {Thread.CurrentThread.IsThreadPoolThread}");
}

await ExampleAsync();
```

#### ConfigureAwait и контекст синхронизации

```csharp
// В ASP.NET Core контекст синхронизации по умолчанию - null
// В WinForms/WPF есть UI контекст синхронизации

async Task ExampleWithConfigureAwait()
{
    // Без ConfigureAwait(false) - продолжение на исходном контексте
    await Task.Delay(1000);
    // Продолжение на том же контексте синхронизации (если он есть)
    
    // С ConfigureAwait(false) - продолжение на ThreadPool
    await Task.Delay(1000).ConfigureAwait(false);
    // Продолжение всегда на ThreadPool, независимо от контекста
}

// Рекомендация для библиотечного кода
public async Task<string> GetDataAsync()
{
    // Используйте ConfigureAwait(false) в библиотечном коде
    // Это избегает захвата контекста синхронизации
    using (var httpClient = new HttpClient())
    {
        return await httpClient.GetStringAsync("https://api.example.com/data")
            .ConfigureAwait(false);
    }
}
```

#### TaskScheduler и планирование задач

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

// 1. Default TaskScheduler (ThreadPool)
Task task1 = Task.Run(() =>
{
    Console.WriteLine("Executed on ThreadPool");
});

// 2. Current TaskScheduler (может быть UI scheduler в WinForms/WPF)
Task task2 = Task.Factory.StartNew(() =>
{
    Console.WriteLine("Executed on current scheduler");
}, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Current);

// 3. Кастомный TaskScheduler
public class CustomTaskScheduler : TaskScheduler
{
    protected override void QueueTask(Task task)
    {
        // Кастомная логика планирования
        ThreadPool.QueueUserWorkItem(_ => TryExecuteTask(task));
    }
    
    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {
        return TryExecuteTask(task);
    }
    
    protected override IEnumerable<Task> GetScheduledTasks()
    {
        return Enumerable.Empty<Task>();
    }
}

CustomTaskScheduler scheduler = new CustomTaskScheduler();
Task task3 = Task.Factory.StartNew(() =>
{
    Console.WriteLine("Executed on custom scheduler");
}, CancellationToken.None, TaskCreationOptions.None, scheduler);
```

---

### Почему Task.Run плохо в ASP.NET?

Использование `Task.Run` в ASP.NET Core может привести к серьезным проблемам с производительностью и масштабируемостью.

#### Проблема 1: Потеря асинхронности

```csharp
// ❌ ПЛОХО: Task.Run в контроллере ASP.NET
[ApiController]
[Route("api/[controller]")]
public class BadController : ControllerBase
{
    [HttpGet("bad")]
    public async Task<IActionResult> BadMethod()
    {
        // Задача выполняется на ThreadPool, но основной поток блокируется ожиданием
        var result = await Task.Run(() =>
        {
            // CPU-intensive операция
            int sum = 0;
            for (int i = 0; i < 1000000; i++)
            {
                sum += i;
            }
            return sum;
        });
        
        return Ok(result);
    }
}
```

**Проблема:**
- Запрос занимает поток из ThreadPool для ожидания
- ThreadPool поток используется для выполнения CPU-операции
- Два потока заняты вместо одного
- Снижается пропускная способность сервера

#### Проблема 2: Неправильное использование для I/O операций

```csharp
// ❌ ПЛОХО: Task.Run для I/O операций
[HttpGet("bad-io")]
public async Task<IActionResult> BadIO()
{
    // НЕ НУЖНО оборачивать асинхронные I/O операции в Task.Run!
    var result = await Task.Run(async () =>
    {
        using (var client = new HttpClient())
        {
            return await client.GetStringAsync("https://api.example.com/data");
        }
    });
    
    return Ok(result);
}

// ✅ ХОРОШО: Прямое использование async I/O
[HttpGet("good-io")]
public async Task<IActionResult> GoodIO()
{
    using (var client = new HttpClient())
    {
        var result = await client.GetStringAsync("https://api.example.com/data");
        return Ok(result);
    }
}
```

#### Проблема 3: Исчерпание ThreadPool

```csharp
// ❌ ПЛОХО: Много Task.Run может исчерпать ThreadPool
[HttpGet("bad-batch")]
public async Task<IActionResult> BadBatch()
{
    var tasks = new List<Task<int>>();
    
    // Создание множества задач, каждая занимает поток
    for (int i = 0; i < 100; i++)
    {
        tasks.Add(Task.Run(() =>
        {
            Thread.Sleep(1000); // Имитация работы
            return i;
        }));
    }
    
    var results = await Task.WhenAll(tasks);
    return Ok(results);
}

// ✅ ХОРОШО: Использование настоящего асинхронного кода
[HttpGet("good-batch")]
public async Task<IActionResult> GoodBatch()
{
    var tasks = new List<Task<string>>();
    
    using (var client = new HttpClient())
    {
        // Настоящие асинхронные операции не занимают потоки
        for (int i = 0; i < 100; i++)
        {
            tasks.Add(client.GetStringAsync($"https://api.example.com/data/{i}"));
        }
        
        var results = await Task.WhenAll(tasks);
        return Ok(results);
    }
}
```

#### Когда Task.Run допустим в ASP.NET?

Только для CPU-intensive операций, которые нельзя сделать асинхронными:

```csharp
// ✅ ПРИЕМЛЕМО: CPU-intensive операция, которую нельзя избежать
[HttpPost("process")]
public async Task<IActionResult> ProcessData([FromBody] int[] data)
{
    // Если это действительно тяжелая CPU-операция
    var result = await Task.Run(() =>
    {
        // Сложные вычисления, обработка изображений, шифрование и т.д.
        return ProcessHeavyComputation(data);
    });
    
    return Ok(result);
}

private int ProcessHeavyComputation(int[] data)
{
    // CPU-intensive операция
    int sum = 0;
    foreach (var item in data)
    {
        sum += item * item;
    }
    return sum;
}
```

**Но лучше:**
- Использовать фоновые задачи (BackgroundService, Hangfire)
- Очереди задач для тяжелых операций
- Отдельные воркеры для обработки

#### Правильный подход: Background Services

```csharp
// ✅ ХОРОШО: Вынос тяжелых операций в фоновые сервисы
public class HeavyProcessingService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Обработка задач из очереди
            await ProcessQueueAsync(stoppingToken);
            await Task.Delay(1000, stoppingToken);
        }
    }
    
    private async Task ProcessQueueAsync(CancellationToken cancellationToken)
    {
        // CPU-intensive операции выполняются в фоне
        // Не блокируют HTTP запросы
    }
}

// Регистрация в Program.cs
builder.Services.AddHostedService<HeavyProcessingService>();
```

#### Резюме: Task.Run в ASP.NET

| Сценарий | Использование Task.Run | Альтернатива |
|----------|------------------------|--------------|
| I/O операции (БД, HTTP, файлы) | ❌ Никогда | async/await напрямую |
| CPU-intensive операции | ⚠️ Избегать | BackgroundService, очереди |
| Синхронные библиотеки | ⚠️ Иногда необходимо | Найти async версию библиотеки |

**Правило:** В ASP.NET используйте `Task.Run` только если у вас нет другого выбора, и всегда предпочитайте настоящий асинхронный код.

---

## 3. Практические примеры

### Пример 1: Параллельная обработка данных

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class DataProcessor
{
    // Обработка большого массива данных параллельно
    public async Task<int[]> ProcessDataParallelAsync(int[] data)
    {
        // Разбиваем данные на чанки
        int chunkSize = data.Length / Environment.ProcessorCount;
        var chunks = data
            .Select((value, index) => new { value, index })
            .GroupBy(x => x.index / chunkSize)
            .Select(g => g.Select(x => x.value).ToArray())
            .ToArray();
        
        // Обрабатываем каждый чанк параллельно
        var tasks = chunks.Select(chunk => Task.Run(() => ProcessChunk(chunk)));
        var results = await Task.WhenAll(tasks);
        
        // Объединяем результаты
        return results.SelectMany(r => r).ToArray();
    }
    
    private int[] ProcessChunk(int[] chunk)
    {
        // CPU-intensive обработка
        return chunk.Select(x => x * x).ToArray();
    }
}
```

### Пример 2: Потокобезопасный кэш

```csharp
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

public class ThreadSafeCache<TKey, TValue>
{
    private readonly ConcurrentDictionary<TKey, Lazy<Task<TValue>>> _cache = new();
    
    // Потокобезопасное получение или создание значения
    public async Task<TValue> GetOrAddAsync(TKey key, Func<TKey, Task<TValue>> valueFactory)
    {
        var lazyTask = _cache.GetOrAdd(key, k => 
            new Lazy<Task<TValue>>(() => valueFactory(k), 
                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication));
        
        return await lazyTask.Value;
    }
    
    // Синхронная версия
    public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
    {
        var lazyValue = _cache.GetOrAdd(key, k => 
            new Lazy<TValue>(() => valueFactory(k), 
                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication));
        
        return lazyValue.Value;
    }
    
    public void Clear() => _cache.Clear();
    public bool TryRemove(TKey key, out TValue value)
    {
        if (_cache.TryRemove(key, out var lazy))
        {
            value = lazy.Value.Result;
            return true;
        }
        value = default(TValue);
        return false;
    }
}
```

### Пример 3: Producer-Consumer Pattern

```csharp
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

public class ProducerConsumer<T>
{
    private readonly BlockingCollection<T> _queue = new BlockingCollection<T>();
    private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    
    // Producer - добавляет элементы
    public void Produce(T item)
    {
        _queue.Add(item);
    }
    
    // Consumer - обрабатывает элементы
    public async Task StartConsumingAsync(Func<T, Task> processor, int numberOfConsumers = 1)
    {
        var tasks = new List<Task>();
        
        for (int i = 0; i < numberOfConsumers; i++)
        {
            int consumerId = i;
            tasks.Add(Task.Run(async () =>
            {
                foreach (var item in _queue.GetConsumingEnumerable(_cancellationTokenSource.Token))
                {
                    try
                    {
                        await processor(item);
                        Console.WriteLine($"Consumer {consumerId} processed: {item}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing {item}: {ex.Message}");
                    }
                }
            }, _cancellationTokenSource.Token));
        }
        
        await Task.WhenAll(tasks);
    }
    
    public void CompleteAdding() => _queue.CompleteAdding();
    public void Stop() => _cancellationTokenSource.Cancel();
}
```

---

## 4. Best Practices и рекомендации

### ✅ Что делать

1. **Используйте async/await для I/O операций**
   ```csharp
   // ✅ Хорошо
   var data = await httpClient.GetStringAsync(url);
   ```

2. **Используйте Task.Run только для CPU-intensive операций**
   ```csharp
   // ✅ Хорошо
   var result = await Task.Run(() => HeavyComputation());
   ```

3. **Используйте потокобезопасные коллекции**
   ```csharp
   // ✅ Хорошо
   ConcurrentDictionary<string, int> dict = new ConcurrentDictionary<string, int>();
   ```

4. **Используйте ConfigureAwait(false) в библиотечном коде**
   ```csharp
   // ✅ Хорошо
   var data = await GetDataAsync().ConfigureAwait(false);
   ```

5. **Всегда обрабатывайте исключения в задачах**
   ```csharp
   // ✅ Хорошо
   try
   {
       await task;
   }
   catch (Exception ex)
   {
       // Обработка исключения
   }
   ```

### ❌ Чего избегать

1. **Не используйте Task.Run в ASP.NET для I/O операций**
   ```csharp
   // ❌ Плохо
   var data = await Task.Run(async () => await httpClient.GetStringAsync(url));
   ```

2. **Не используйте lock на публичных объектах**
   ```csharp
   // ❌ Плохо
   lock (this) { }
   
   // ✅ Хорошо
   private readonly object _lock = new object();
   lock (_lock) { }
   ```

3. **Не используйте Thread.Sleep в async методах**
   ```csharp
   // ❌ Плохо
   Thread.Sleep(1000);
   
   // ✅ Хорошо
   await Task.Delay(1000);
   ```

4. **Не игнорируйте Task исключения**
   ```csharp
   // ❌ Плохо
   Task.Run(() => { throw new Exception(); });
   
   // ✅ Хорошо
   try
   {
       await Task.Run(() => { throw new Exception(); });
   }
   catch { }
   ```

5. **Не создавайте слишком много потоков вручную**
   ```csharp
   // ❌ Плохо
   for (int i = 0; i < 1000; i++)
   {
       new Thread(DoWork).Start();
   }
   
   // ✅ Хорошо
   Parallel.For(0, 1000, i => DoWork());
   ```

---

## 5. Проверка знаний

### Вопросы для самопроверки

1. **Что такое Race Condition и как его избежать?**
   - Race Condition возникает, когда несколько потоков обращаются к общим данным без синхронизации
   - Решение: использование lock, Interlocked, потокобезопасных коллекций

2. **В чем разница между Task и Thread?**
   - Task - высокоуровневая абстракция, использует ThreadPool
   - Thread - низкоуровневый объект, требует ручного управления

3. **Почему Task.Run плохо в ASP.NET?**
   - Занимает потоки из ThreadPool без необходимости
   - Для I/O операций есть настоящий async/await
   - Снижает пропускную способность сервера

4. **Что такое Deadlock и как его предотвратить?**
   - Deadlock - взаимная блокировка потоков
   - Решение: упорядоченная блокировка, таймауты, избежание циклических зависимостей

5. **Где выполняется Task?**
   - Task.Run - всегда на ThreadPool
   - async/await - продолжение на захваченном контексте синхронизации (или ThreadPool с ConfigureAwait(false))

---

## Заключение

Многопоточность в C# - мощный инструмент, но требующий понимания основ:
- Правильное использование Task и Thread
- Понимание проблем синхронизации (Race Condition, Deadlock)
- Знание когда использовать Task.Run, а когда нет
- Понимание контекста выполнения задач

Помните: **в ASP.NET предпочитайте настоящий async/await над Task.Run**, используйте потокобезопасные коллекции и всегда обрабатывайте исключения.
