# Threading e Concorrenza in C#

## Introduzione

Il **threading** e la **concorrenza** permettono di eseguire più operazioni simultaneamente, migliorando le performance e la responsività delle applicazioni. In C#, ci sono diversi modi per gestire operazioni parallele e concorrenti.

---

## 1. Concetti Base

### Thread vs Processo

```
┌─────────────────────────────────────────────┐
│  PROCESSO                                  │
│  ┌──────────────────────────────────────┐ │
│  │  Memoria Isolata                      │ │
│  │  ┌──────────┐  ┌──────────┐         │ │
│  │  │ Thread 1 │  │ Thread 2 │         │ │
│  │  └──────────┘  └──────────┘         │ │
│  │  ┌──────────┐                        │ │
│  │  │ Thread 3 │                        │ │
│  │  └──────────┘                        │ │
│  └──────────────────────────────────────┘ │
└─────────────────────────────────────────────┘
```

**Processo:**
- Isolamento completo della memoria
- Più pesante da creare
- Comunicazione più complessa

**Thread:**
- Condivide memoria con altri thread dello stesso processo
- Più leggero
- Comunicazione più semplice

---

## 2. Thread Base

### Creazione Thread

```csharp
using System.Threading;

// Metodo da eseguire
void DoWork() {
    for (int i = 0; i < 5; i++) {
        Console.WriteLine($"Thread: {i}");
        Thread.Sleep(1000);
    }
}

// Creazione e avvio thread
Thread thread = new Thread(DoWork);
thread.Start();

// Attesa completamento
thread.Join();
Console.WriteLine("Thread completato");
```

### Thread con Parametri

```csharp
void DoWorkWithParam(object data) {
    int value = (int)data;
    Console.WriteLine($"Valore: {value}");
}

Thread thread = new Thread(DoWorkWithParam);
thread.Start(42);
```

### Thread Foreground vs Background

```csharp
Thread foregroundThread = new Thread(DoWork);
foregroundThread.IsBackground = false; // Foreground (default)
// L'applicazione non termina finché il thread è attivo

Thread backgroundThread = new Thread(DoWork);
backgroundThread.IsBackground = true; // Background
// L'applicazione può terminare anche se il thread è attivo
backgroundThread.Start();
```

---

## 3. Task (Raccomandato)

### Task vs Thread

**Task** è un'astrazione di livello superiore rispetto ai Thread. È più semplice da usare e gestisce automaticamente il thread pool.

### Creazione Task

```csharp
using System.Threading.Tasks;

// Task semplice
Task task = Task.Run(() => {
    Console.WriteLine("Task eseguito");
});

// Attesa completamento
await task; // oppure task.Wait();

// Task con ritorno
Task<int> taskWithResult = Task.Run(() => {
    return 42;
});

int result = await taskWithResult;
Console.WriteLine($"Risultato: {result}");
```

### Task Asincrono

```csharp
public async Task<int> CalculateAsync() {
    await Task.Delay(1000); // Simula operazione asincrona
    return 42;
}

// Utilizzo
int result = await CalculateAsync();
```

---

## 4. Parallel Processing

### Parallel.For

```csharp
using System.Threading.Tasks;

// Esecuzione parallela di loop
Parallel.For(0, 10, i => {
    Console.WriteLine($"Iterazione {i} su thread {Thread.CurrentThread.ManagedThreadId}");
});

// Con opzioni
var options = new ParallelOptions {
    MaxDegreeOfParallelism = 4 // Massimo 4 thread
};

Parallel.For(0, 100, options, i => {
    // Operazione parallela
});
```

### Parallel.ForEach

```csharp
var items = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

Parallel.ForEach(items, item => {
    Console.WriteLine($"Processando {item} su thread {Thread.CurrentThread.ManagedThreadId}");
    // Operazione su item
});
```

### Parallel.Invoke

```csharp
// Esegue più azioni in parallelo
Parallel.Invoke(
    () => DoWork1(),
    () => DoWork2(),
    () => DoWork3()
);
```

---

## 5. Race Conditions e Thread Safety

### Problema: Race Condition

```csharp
// ❌ PROBLEMA: Race condition
public class Counter {
    private int _count = 0;
    
    public void Increment() {
        _count++; // Non thread-safe!
    }
    
    public int Count => _count;
}

// Utilizzo con più thread
var counter = new Counter();
var tasks = new List<Task>();

for (int i = 0; i < 1000; i++) {
    tasks.Add(Task.Run(() => counter.Increment()));
}

await Task.WhenAll(tasks);
Console.WriteLine(counter.Count); // Potrebbe essere < 1000!
```

### Soluzione: lock

```csharp
// ✅ SOLUZIONE: lock per thread-safety
public class Counter {
    private int _count = 0;
    private readonly object _lock = new object();
    
    public void Increment() {
        lock (_lock) {
            _count++; // Thread-safe!
        }
    }
    
    public int Count {
        get {
            lock (_lock) {
                return _count;
            }
        }
    }
}
```

### Interlocked (Per Operazioni Atomiche)

```csharp
using System.Threading;

public class Counter {
    private int _count = 0;
    
    public void Increment() {
        Interlocked.Increment(ref _count); // Operazione atomica
    }
    
    public int Count => _count;
}
```

---

## 6. Concurrent Collections

### ConcurrentDictionary

```csharp
using System.Collections.Concurrent;

var dict = new ConcurrentDictionary<string, int>();

// Operazioni thread-safe
dict.TryAdd("key1", 1);
dict.TryAdd("key2", 2);

// Aggiornamento thread-safe
dict.AddOrUpdate("key1", 0, (key, oldValue) => oldValue + 1);

// Lettura thread-safe
if (dict.TryGetValue("key1", out int value)) {
    Console.WriteLine(value);
}
```

### ConcurrentQueue

```csharp
var queue = new ConcurrentQueue<int>();

// Aggiunta thread-safe
queue.Enqueue(1);
queue.Enqueue(2);

// Rimozione thread-safe
if (queue.TryDequeue(out int item)) {
    Console.WriteLine(item);
}
```

### ConcurrentBag

```csharp
var bag = new ConcurrentBag<int>();

// Aggiunta thread-safe
bag.Add(1);
bag.Add(2);

// Rimozione thread-safe (ordine non garantito)
if (bag.TryTake(out int item)) {
    Console.WriteLine(item);
}
```

---

## 7. Async/Await Pattern

### Async/Await per Concorrenza

```csharp
// Operazioni asincrone sequenziali
public async Task ProcessSequentiallyAsync() {
    await DoWork1Async();
    await DoWork2Async();
    await DoWork3Async();
}

// Operazioni asincrone parallele
public async Task ProcessInParallelAsync() {
    var task1 = DoWork1Async();
    var task2 = DoWork2Async();
    var task3 = DoWork3Async();
    
    await Task.WhenAll(task1, task2, task3);
}

// Oppure
public async Task ProcessInParallelAsync() {
    await Task.WhenAll(
        DoWork1Async(),
        DoWork2Async(),
        DoWork3Async()
    );
}
```

### Task.WhenAny

```csharp
// Attende il primo task che completa
var tasks = new[] {
    DownloadFromServer1Async(),
    DownloadFromServer2Async(),
    DownloadFromServer3Async()
};

var completedTask = await Task.WhenAny(tasks);
var result = await completedTask;
Console.WriteLine($"Primo completato: {result}");
```

---

## 8. Semaphore e Mutex

### SemaphoreSlim

```csharp
using System.Threading;

// Limita il numero di thread concorrenti
var semaphore = new SemaphoreSlim(3); // Massimo 3 thread

async Task DoWorkAsync(int id) {
    await semaphore.WaitAsync(); // Aspetta se ci sono già 3 thread
    
    try {
        Console.WriteLine($"Thread {id} inizia");
        await Task.Delay(1000);
        Console.WriteLine($"Thread {id} completa");
    }
    finally {
        semaphore.Release(); // Rilascia il semaforo
    }
}

// Esegue 10 task, ma solo 3 alla volta
var tasks = Enumerable.Range(0, 10)
    .Select(i => DoWorkAsync(i))
    .ToArray();

await Task.WhenAll(tasks);
```

### Mutex

```csharp
using System.Threading;

// Mutex per sincronizzazione cross-process
var mutex = new Mutex(false, "MyAppMutex");

if (mutex.WaitOne(1000)) { // Attende max 1 secondo
    try {
        // Sezione critica
        Console.WriteLine("Sezione critica");
    }
    finally {
        mutex.ReleaseMutex();
    }
}
```

---

## 9. ReaderWriterLockSlim

### Lettura/Scrittura Concorrente

```csharp
using System.Threading;

public class ThreadSafeCache<TKey, TValue> {
    private readonly Dictionary<TKey, TValue> _cache = new();
    private readonly ReaderWriterLockSlim _lock = new();
    
    public TValue Get(TKey key) {
        _lock.EnterReadLock();
        try {
            return _cache.TryGetValue(key, out var value) ? value : default;
        }
        finally {
            _lock.ExitReadLock();
        }
    }
    
    public void Set(TKey key, TValue value) {
        _lock.EnterWriteLock();
        try {
            _cache[key] = value;
        }
        finally {
            _lock.ExitWriteLock();
        }
    }
}
```

**Vantaggi:**
- ✅ Multiple letture simultanee
- ✅ Scrittura esclusiva
- ✅ Performance migliori per letture frequenti

---

## 10. Deadlock

### Cos'è un Deadlock?

Un **deadlock** si verifica quando due o più thread si aspettano l'uno dall'altro, bloccando l'esecuzione.

### Esempio di Deadlock

```csharp
// ❌ PROBLEMA: Deadlock potenziale
object lock1 = new object();
object lock2 = new object();

// Thread 1
Task.Run(() => {
    lock (lock1) {
        Thread.Sleep(100);
        lock (lock2) { // Aspetta lock2
            // ...
        }
    }
});

// Thread 2
Task.Run(() => {
    lock (lock2) {
        Thread.Sleep(100);
        lock (lock1) { // Aspetta lock1 - DEADLOCK!
            // ...
        }
    }
});
```

### Prevenzione Deadlock

```csharp
// ✅ SOLUZIONE: Stesso ordine di lock
object lock1 = new object();
object lock2 = new object();

Task.Run(() => {
    lock (lock1) {
        lock (lock2) { // Stesso ordine
            // ...
        }
    }
});

Task.Run(() => {
    lock (lock1) { // Stesso ordine
        lock (lock2) {
            // ...
        }
    }
});
```

---

## 11. Best Practices

### ✅ Cosa Fare

1. **Usa Task invece di Thread quando possibile**
   ```csharp
   // ✅ CORRETTO
   await Task.Run(() => DoWork());
   ```

2. **Usa async/await per operazioni I/O**
   ```csharp
   // ✅ CORRETTO
   var data = await httpClient.GetStringAsync("url");
   ```

3. **Usa Concurrent Collections per thread-safety**
   ```csharp
   // ✅ CORRETTO
   var dict = new ConcurrentDictionary<string, int>();
   ```

4. **Rilascia sempre i lock**
   ```csharp
   // ✅ CORRETTO
   lock (_lock) {
       // operazioni
   } // Lock rilasciato automaticamente
   ```

### ❌ Cosa Evitare

1. **Non usare lock su oggetti pubblici**
   ```csharp
   // ❌ SBAGLIATO
   lock (this) { }
   
   // ✅ CORRETTO
   private readonly object _lock = new object();
   lock (_lock) { }
   ```

2. **Non fare operazioni sincrone in async**
   ```csharp
   // ❌ SBAGLIATO
   public async Task DoWorkAsync() {
       Thread.Sleep(1000); // Blocca il thread!
   }
   
   // ✅ CORRETTO
   public async Task DoWorkAsync() {
       await Task.Delay(1000); // Non blocca
   }
   ```

3. **Non catturare SynchronizationContext inutile**
   ```csharp
   // ✅ CORRETTO
   await Task.Run(() => DoWork()).ConfigureAwait(false);
   ```

---

## 12. Esempi Pratici

### Esempio 1: Download Parallelo

```csharp
public async Task<List<string>> DownloadUrlsAsync(string[] urls) {
    var tasks = urls.Select(url => DownloadAsync(url));
    var results = await Task.WhenAll(tasks);
    return results.ToList();
}

private async Task<string> DownloadAsync(string url) {
    using var client = new HttpClient();
    return await client.GetStringAsync(url);
}
```

### Esempio 2: Processamento Parallelo con Limite

```csharp
public async Task ProcessItemsAsync<T>(IEnumerable<T> items, Func<T, Task> processor, int maxConcurrency = 4) {
    var semaphore = new SemaphoreSlim(maxConcurrency);
    
    var tasks = items.Select(async item => {
        await semaphore.WaitAsync();
        try {
            await processor(item);
        }
        finally {
            semaphore.Release();
        }
    });
    
    await Task.WhenAll(tasks);
}
```

### Esempio 3: Cache Thread-Safe

```csharp
public class ThreadSafeCache<TKey, TValue> {
    private readonly ConcurrentDictionary<TKey, Lazy<TValue>> _cache = new();
    
    public TValue GetOrAdd(TKey key, Func<TValue> factory) {
        var lazy = _cache.GetOrAdd(key, k => new Lazy<TValue>(factory));
        return lazy.Value;
    }
}
```

---

## 13. Domande Frequenti (FAQ)

### Q: Qual è la differenza tra Task e Thread?
**R:** `Task` è un'astrazione di livello superiore che usa il thread pool. `Thread` è più basso livello e crea un nuovo thread del sistema operativo.

### Q: Quando usare Parallel.For vs Task.Run?
**R:** `Parallel.For` per loop paralleli su collezioni. `Task.Run` per operazioni asincrone singole.

### Q: Come evitare deadlock?
**R:** Usa sempre lo stesso ordine per acquisire lock multipli, oppure usa `Monitor.TryEnter` con timeout.

### Q: Async/Await crea nuovi thread?
**R:** No, `async/await` non crea thread. Usa il thread pool esistente e non blocca thread durante attesa I/O.

---

## Conclusioni

Threading e concorrenza sono essenziali per:
- ✅ Migliorare performance
- ✅ Migliorare responsività
- ✅ Utilizzare efficacemente risorse
- ✅ Gestire operazioni parallele

Ricorda: usa `Task` e `async/await` per la maggior parte dei casi, e assicurati sempre la thread-safety!

---

_Documento creato per spiegare Threading e Concorrenza in C# con esempi pratici e best practices._

