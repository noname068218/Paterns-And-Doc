# Async и Await в C#

## Введение

**async** и **await** — это ключевые слова в C#, которые позволяют писать асинхронный код простым и читаемым способом. Они позволяют выполнять длительные операции без блокировки главного потока, улучшая отзывчивость приложений.

---

## 1. Зачем Async/Await?

### Проблема: Блокирующий синхронный код

```csharp
// ❌ PROBLEM: Blocks thread during operation
public string DownloadData() {
    // Simulate slow operation (database, API, file)
    Thread.Sleep(3000);  // Blocks for 3 seconds!
    return "Data downloaded";
}

// Usage
string data = DownloadData();  // Application freezes here!
Console.WriteLine(data);
```

### Решение: Асинхронный код

```csharp
// ✅ SOLUTION: Does not block thread
public async Task<string> DownloadDataAsync() {
    // Simulate async operation
    await Task.Delay(3000);  // Does not block!
    return "Data downloaded";
}

// Usage
string data = await DownloadDataAsync();  // Application remains responsive!
Console.WriteLine(data);
```

### Диаграмма: Синхронный vs Асинхронный

```
┌─────────────────────────────────────────────┐
│  SYNCHRONOUS CODE (Blocking)                │
├─────────────────────────────────────────────┤
│  Main thread                                │
│  │                                          │
│  │─── DownloadData() ────────────────────│  │
│  │     (blocked for 3 seconds)          │  │
│  │──────────────────────────────────────│  │
│  │─── Continue execution ───────────────│  │
│  │                                      │  │
│  ❌ Application not responsive!           │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  ASYNCHRONOUS CODE (Non-blocking)           │
├─────────────────────────────────────────────┤
│  Main thread                                │
│  │                                          │
│  │─── DownloadDataAsync() ───────────────│  │
│  │     (awaits, but does not block)      │  │
│  │─── Continue other operations ─────────│  │
│  │     (UI responsive!)                   │  │
│  │─── Receive result ────────────────────│  │
│  │                                      │  │
│  ✅ Application always responsive!         │
└─────────────────────────────────────────────┘
```

---

## 2. Базовый синтаксис: async и await

### async

Ключевое слово `async` модифицирует метод, указывая, что он содержит асинхронные операции.

```csharp
// Async method
public async Task<int> MethodAsync() {
    // Method body
    return 42;
}
```

### await

Ключевое слово `await` приостанавливает выполнение async метода до завершения асинхронной операции.

```csharp
public async Task<int> MethodAsync() {
    // await suspends execution here
    int result = await AsynchronousOperation();
    
    // This code runs after completion
    return result;
}
```

### Полный пример

```csharp
public class DataService {
    // Async method
    public async Task<string> DownloadDataAsync() {
        // Simulate HTTP call
        await Task.Delay(2000);  // Wait 2 seconds without blocking
        return "Data from server";
    }
    
    public async Task<int> CalculateAsync() {
        await Task.Delay(1000);
        return 100;
    }
}

// Usage
public async Task CallingMethod() {
    var service = new DataService();
    
    string data = await service.DownloadDataAsync();  // Wait here
    Console.WriteLine(data);
    
    int result = await service.CalculateAsync();  // Wait here
    Console.WriteLine(result);
}
```

### Диаграмма: Поток Async/Await

```
┌─────────────────────────────────────────────┐
│  CallingMethod()                            │
│  ↓                                          │
│  await DownloadDataAsync()                 │
│  ↓                                          │
│  [Suspend execution]                        │
│  ↓                                          │
│  Thread free for other operations           │
│  ↓                                          │
│  [Operation complete]                       │
│  ↓                                          │
│  Resume execution                           │
│  ↓                                          │
│  Console.WriteLine(data)                    │
└─────────────────────────────────────────────┘
```

---

## 3. Типы возвращаемых значений Async

### Task<T>

Возвращает значение типа `T`.

```csharp
public async Task<int> GetNumberAsync() {
    await Task.Delay(1000);
    return 42;
}

// Usage
int number = await GetNumberAsync();  // ✅ 42
```

### Task

Не возвращает значение (эквивалент `void`).

```csharp
public async Task SaveDataAsync() {
    await Task.Delay(1000);
    Console.WriteLine("Data saved");
    // No return
}

// Usage
await SaveDataAsync();  // ✅ Wait for completion
```

### void (Не рекомендуется)

```csharp
// ⚠️ AVOID: Cannot await void
public async void MethodAsync() {
    await Task.Delay(1000);
    // Problems: cannot await, uncaught errors
}
```

### Диаграмма: Типы возврата

```
┌─────────────────────────────────────────────┐
│  async Task<T>                               │
│  ✅ Returns value                           │
│  ✅ Can await                                │
│  ✅ Error handling                           │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  async Task                                 │
│  ✅ No return value                         │
│  ✅ Can await                                │
│  ✅ Error handling                           │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  async void                                 │
│  ❌ Cannot await                             │
│  ❌ Uncaught errors                         │
│  ⚠️  Use only for event handlers           │
└─────────────────────────────────────────────┘
```

---

## 4. Распространённые асинхронные операции

### 4.1 HTTP-запросы

```csharp
using System.Net.Http;

public class ApiService {
    private HttpClient httpClient = new HttpClient();
    
    public async Task<string> GetDataAsync(string url) {
        // Async HTTP call
        HttpResponseMessage response = await httpClient.GetAsync(url);
        string content = await response.Content.ReadAsStringAsync();
        return content;
    }
    
    public async Task<string> PostDataAsync(string url, string data) {
        var content = new StringContent(data);
        HttpResponseMessage response = await httpClient.PostAsync(url, content);
        return await response.Content.ReadAsStringAsync();
    }
}

// Usage
var service = new ApiService();
string data = await service.GetDataAsync("https://api.example.com/data");
```

### 4.2 Файловый I/O

```csharp
using System.IO;

public class FileService {
    public async Task<string> ReadFileAsync(string path) {
        // Async file reading
        using (var reader = new StreamReader(path)) {
            return await reader.ReadToEndAsync();
        }
    }
    
    public async Task WriteFileAsync(string path, string content) {
        // Async file writing
        using (var writer = new StreamWriter(path)) {
            await writer.WriteAsync(content);
        }
    }
}

// Usage
var fileService = new FileService();
string content = await fileService.ReadFileAsync("file.txt");
await fileService.WriteFileAsync("output.txt", content);
```

### 4.3 Операции с базой данных

```csharp
using System.Data.SqlClient;

public class DatabaseService {
    private string connectionString = "Server=...";
    
    public async Task<List<Person>> GetPeopleAsync() {
        var people = new List<Person>();
        
        using (var connection = new SqlConnection(connectionString)) {
            await connection.OpenAsync();  // Async connection
            
            using (var command = new SqlCommand("SELECT * FROM People", connection)) {
                using (var reader = await command.ExecuteReaderAsync()) {  // Async reading
                    while (await reader.ReadAsync()) {
                        people.Add(new Person {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }
        }
        
        return people;
    }
}
```

### Диаграмма: Асинхронные операции

```
┌─────────────────────────────────────────────┐
│  Async Operations                           │
├─────────────────────────────────────────────┤
│  HTTP Request                               │
│  ↓ await httpClient.GetAsync()             │
│  ↓ Does not block thread                   │
│  ↓ Receive response                        │
├─────────────────────────────────────────────┤
│  File I/O                                   │
│  ↓ await File.ReadAllTextAsync()           │
│  ↓ Does not block thread                   │
│  ↓ Read file                               │
├─────────────────────────────────────────────┤
│  Database                                   │
│  ↓ await connection.OpenAsync()            │
│  ↓ await command.ExecuteReaderAsync()      │
│  ↓ Does not block thread                   │
│  ↓ Receive data                            │
└─────────────────────────────────────────────┘
```

---

## 5. Параллельное выполнение

### Task.WhenAll

Выполняет несколько асинхронных операций параллельно.

```csharp
public async Task<string[]> DownloadAllDataAsync() {
    var task1 = DownloadData1Async();
    var task2 = DownloadData2Async();
    var task3 = DownloadData3Async();
    
    // Wait for all operations to complete
    string[] results = await Task.WhenAll(task1, task2, task3);
    return results;
}

// Or with array
public async Task<string[]> DownloadAllDataAsync() {
    var tasks = new[] {
        DownloadData1Async(),
        DownloadData2Async(),
        DownloadData3Async()
    };
    
    return await Task.WhenAll(tasks);
}
```

### Диаграмма: Task.WhenAll

```
┌─────────────────────────────────────────────┐
│  Task 1 ────────────────┐                    │
│  Task 2 ────────────────┼─── Task.WhenAll() │
│  Task 3 ────────────────┘                    │
│                         │                    │
│  All in parallel        │                    │
│  ↓                      │                    │
│  Wait for completion    │                    │
│  ↓                      │                    │
│  Return all results                          │
└─────────────────────────────────────────────┘
```

### Task.WhenAny

Ждёт первую завершившуюся операцию.

```csharp
public async Task<string> DownloadFirstAvailableAsync() {
    var task1 = DownloadFromServer1Async();
    var task2 = DownloadFromServer2Async();
    var task3 = DownloadFromServer3Async();
    
    // Wait for first to complete
    Task<string> firstCompleted = await Task.WhenAny(task1, task2, task3);
    return await firstCompleted;
}
```

### Диаграмма: Task.WhenAny

```
┌─────────────────────────────────────────────┐
│  Task 1 ────────────────┐                    │
│  Task 2 ────────────✅───┼─── Task.WhenAny() │
│  Task 3 ────────────────┘                    │
│                         │                    │
│  All in parallel        │                    │
│  ↓                      │                    │
│  First complete (Task 2)│                    │
│  ↓                      │                    │
│  Return Task 2 result                        │
└─────────────────────────────────────────────┘
```

---

## 6. Обработка ошибок

### try-catch с async

```csharp
public async Task<string> DownloadDataAsync() {
    try {
        string data = await AsynchronousOperation();
        return data;
    }
    catch (HttpRequestException ex) {
        Console.WriteLine($"HTTP Error: {ex.Message}");
        throw;
    }
    catch (Exception ex) {
        Console.WriteLine($"Generic Error: {ex.Message}");
        throw;
    }
}

// Usage
try {
    string data = await DownloadDataAsync();
}
catch (Exception ex) {
    Console.WriteLine($"Error: {ex.Message}");
}
```

### Диаграмма: Обработка ошибок

```
┌─────────────────────────────────────────────┐
│  await AsynchronousOperation()              │
│  ↓                                          │
│  [Operation in progress]                    │
│  ↓                                          │
│  ┌───────────┐      ┌───────────┐          │
│  │ Success   │      │  Error    │          │
│  │           │      │           │          │
│  │ Continue  │      │  catch    │          │
│  │           │      │  handles  │          │
│  └───────────┘      └───────────┘          │
└─────────────────────────────────────────────┘
```

---

## 7. ConfigureAwait

### Проблема: Захват контекста

```csharp
// By default, await captures the SynchronizationContext
public async Task MethodAsync() {
    await OperationAsync();  // Captures context (UI thread)
    // Continuation on UI thread
}
```

### Решение: ConfigureAwait(false)

```csharp
// ConfigureAwait(false) - does not capture context
public async Task MethodAsync() {
    await OperationAsync().ConfigureAwait(false);
    // Continuation on thread pool (more efficient)
}
```

### Когда использовать

```csharp
// ✅ Use ConfigureAwait(false) in libraries
public class LibraryClass {
    public async Task<string> GetDataAsync() {
        // Context is not important
        return await httpClient.GetStringAsync(url).ConfigureAwait(false);
    }
}

// ⚠️ Do not use in UI code if you need to update UI
public async Task UpdateUIAsync() {
    // Without ConfigureAwait - continue on UI thread
    string data = await GetDataAsync();
    textBox.Text = data;  // ✅ OK - we're on UI thread
}

public async Task UpdateUIAsync() {
    // With ConfigureAwait(false) - attention!
    string data = await GetDataAsync().ConfigureAwait(false);
    textBox.Text = data;  // ❌ ERROR! Not on UI thread!
}
```

---

## 8. Полные практические примеры

### Пример 1: Множественная загрузка файлов

```csharp
public class FileDownloader {
    private HttpClient httpClient = new HttpClient();
    
    public async Task<List<string>> DownloadFilesAsync(List<string> urls) {
        var tasks = urls.Select(url => DownloadSingleFileAsync(url));
        return (await Task.WhenAll(tasks)).ToList();
    }
    
    private async Task<string> DownloadSingleFileAsync(string url) {
        Console.WriteLine($"Start download: {url}");
        string content = await httpClient.GetStringAsync(url);
        Console.WriteLine($"Completed: {url}");
        return content;
    }
}

// Usage
var downloader = new FileDownloader();
var urls = new List<string> {
    "https://api.example.com/data1",
    "https://api.example.com/data2",
    "https://api.example.com/data3"
};

List<string> results = await downloader.DownloadFilesAsync(urls);
// All files download in parallel!
```

### Пример 2: Сервис с таймаутом

```csharp
public class ApiService {
    private HttpClient httpClient = new HttpClient();
    
    public async Task<string> GetDataWithTimeoutAsync(string url, int timeoutMs = 5000) {
        using (var cts = new CancellationTokenSource(timeoutMs)) {
            try {
                return await httpClient.GetStringAsync(url, cts.Token);
            }
            catch (TaskCanceledException) {
                throw new TimeoutException("Operation timeout");
            }
        }
    }
}

// Usage
var service = new ApiService();
try {
    string data = await service.GetDataWithTimeoutAsync("https://api.example.com", 3000);
}
catch (TimeoutException ex) {
    Console.WriteLine($"Timeout: {ex.Message}");
}
```

### Пример 3: Асинхронный конвейер обработки

```csharp
public class DataProcessor {
    public async Task<string> ProcessDataAsync(string input) {
        // Step 1: Validate
        string validated = await ValidateAsync(input);
        
        // Step 2: Transform
        string transformed = await TransformAsync(validated);
        
        // Step 3: Save
        await SaveAsync(transformed);
        
        return transformed;
    }
    
    private async Task<string> ValidateAsync(string data) {
        await Task.Delay(100);
        return data;
    }
    
    private async Task<string> TransformAsync(string data) {
        await Task.Delay(200);
        return data.ToUpper();
    }
    
    private async Task SaveAsync(string data) {
        await Task.Delay(150);
        // Save data
    }
}

// Usage
var processor = new DataProcessor();
string result = await processor.ProcessDataAsync("test");
// Executes all steps in sequence
```

---

## 9. Антипаттерны, которых следует избегать

### ❌ async void

```csharp
// ❌ WRONG
public async void MethodAsync() {
    await Task.Delay(1000);
}

// ✅ CORRECT
public async Task MethodAsync() {
    await Task.Delay(1000);
}
```

### ❌ Блокировка на Async

```csharp
// ❌ WRONG - Possible deadlock!
public string DownloadData() {
    return DownloadDataAsync().Result;  // ❌ Blocks!
}

// ❌ WRONG
public string DownloadData() {
    return DownloadDataAsync().GetAwaiter().GetResult();  // ❌ Blocks!
}

// ✅ CORRECT
public async Task<string> DownloadData() {
    return await DownloadDataAsync();
}
```

### ❌ Неправильный Fire-and-Forget

```csharp
// ❌ WRONG - uncaught errors
public void Method() {
    OperationAsync();  // Error lost!
}

// ✅ CORRECT
public void Method() {
    _ = OperationAsync().ContinueWith(task => {
        if (task.IsFaulted) {
            // Handle error
        }
    }, TaskContinuationOptions.OnlyOnFaulted);
}
```

### Диаграмма: Антипаттерны

```
┌─────────────────────────────────────────────┐
│  ❌ async void                               │
│  - Uncaught errors                          │
│  - Cannot await                             │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  ❌ .Result or .Wait()                       │
│  - Possible deadlock                        │
│  - Blocks thread                            │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  ❌ Fire-and-forget without error handling  │
│  - Lost errors                              │
│  - Difficult debug                          │
└─────────────────────────────────────────────┘
```

---

## 10. Best Practices

### ✅ Что делать

1. **Используйте async/await для I/O операций**
   ```csharp
   // ✅ CORRECT
   public async Task<string> GetDataAsync() {
       return await httpClient.GetStringAsync(url);
   }
   ```

2. **Распространяйте async через всю цепочку вызовов**
   ```csharp
   // ✅ CORRECT
   public async Task Method1Async() {
       await Method2Async();
   }
   
   public async Task Method2Async() {
       await Method3Async();
   }
   ```

3. **Используйте Task.WhenAll для параллельных операций**
   ```csharp
   // ✅ CORRECT
   var results = await Task.WhenAll(task1, task2, task3);
   ```

4. **Используйте ConfigureAwait(false) в библиотеках**
   ```csharp
   // ✅ CORRECT
   return await httpClient.GetStringAsync(url).ConfigureAwait(false);
   ```

### ❌ Чего избегать

1. **Не используйте async для CPU-интенсивных операций**
   ```csharp
   // ❌ WRONG
   public async Task<int> CalculateAsync() {
       return await Task.Run(() => HeavyCalculation());  // ⚠️ Use Task.Run only if necessary
   }
   ```

2. **Не создавайте async void (кроме обработчиков событий)**
   ```csharp
   // ❌ WRONG
   public async void MethodAsync() { }
   ```

3. **Не блокируйте async код**
   ```csharp
   // ❌ WRONG
   var result = MethodAsync().Result;
   ```

---

## 11. Производительность и соображения

### Преимущества

- ✅ **Не блокирует поток** - отзывчивый UI
- ✅ **Масштабируемость** - обрабатывает больше параллельных операций
- ✅ **Эффективность** - лучшее использование ресурсов

### Когда использовать

- ✅ **I/O операции**: HTTP, File, Database
- ✅ **Длительные операции**: неблокирующие
- ✅ **UI приложения**: поддержка отзывчивости

### Когда НЕ использовать

- ❌ **CPU-интенсивные операции**: используйте Task.Run() или отдельные потоки
- ❌ **Простые синхронные операции**: async не нужен
- ❌ **Критичные hot paths**: минимальный, но присутствующий overhead

---

## 12. Часто задаваемые вопросы (FAQ)

### Q: Делает ли async код быстрее?
**A:** Нет, async не делает код быстрее, но улучшает масштабируемость и отзывчивость приложения.

### Q: Сколько потоков создаёт async?
**A:** Async не создаёт дополнительные потоки. Он эффективно использует существующий thread pool.

### Q: Могу ли я использовать await без async?
**A:** Нет, await может использоваться только в методах, помеченных async.

### Q: Как управлять таймаутами?
**A:** Используйте `CancellationTokenSource` с таймаутом или `Task.WaitAsync()`.

---

## Заключение

async/await в C# — это основа для:

- ✅ Написания читаемого асинхронного кода
- ✅ Улучшения отзывчивости приложений
- ✅ Эффективной обработки I/O операций
- ✅ Масштабирования приложений, обрабатывающих множество параллельных операций

Async/await — это современный и рекомендуемый способ обработки асинхронных операций в C#.

---

*Документ создан для объяснения async/await в C# с практическими примерами и диаграммами.*

