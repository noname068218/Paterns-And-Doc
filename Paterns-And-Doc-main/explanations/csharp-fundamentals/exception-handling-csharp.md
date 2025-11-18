# Gestione delle Eccezioni (Exception Handling) in C#

## Introduzione

La **gestione delle eccezioni** è un meccanismo fondamentale per gestire errori e situazioni impreviste in modo controllato. In C#, le eccezioni permettono di separare la logica di business dalla gestione degli errori.

---

## 1. Cos'è un'Eccezione?

### Definizione

Un'**eccezione** è un evento che si verifica durante l'esecuzione di un programma e interrompe il normale flusso di istruzioni. Le eccezioni rappresentano errori o situazioni anomale che richiedono gestione speciale.

### Tipi di Errori

```
┌─────────────────────────────────────────────┐
│           TIPI DI ERRORI                     │
└─────────────────────────────────────────────┘
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│ COMPILE  │ │ RUNTIME   │ │ LOGIC     │
│ TIME     │ │ ERRORS    │ │ ERRORS    │
│          │ │           │ │           │
│ Syntax   │ │ NullRef   │ │ Wrong     │
│ Type     │ │ OutOfRange│ │ Algorithm │
└───────────┘ └───────────┘ └───────────┘
```

---

## 2. Struttura Base: try-catch-finally

### Sintassi Base

```csharp
try {
    // Codice che può generare eccezioni
    RiskyOperation();
}
catch (SpecificException ex) {
    // Gestisce eccezioni specifiche
    Console.WriteLine($"Errore: {ex.Message}");
}
catch (Exception ex) {
    // Gestisce tutte le altre eccezioni
    Console.WriteLine($"Errore generico: {ex.Message}");
}
finally {
    // Codice sempre eseguito (pulizia risorse)
    Cleanup();
}
```

### Esempio Pratico

```csharp
try {
    int result = Divide(10, 0);
    Console.WriteLine($"Risultato: {result}");
}
catch (DivideByZeroException ex) {
    Console.WriteLine($"Errore: Divisione per zero! {ex.Message}");
}
catch (Exception ex) {
    Console.WriteLine($"Errore generico: {ex.Message}");
}
finally {
    Console.WriteLine("Operazione completata");
}

int Divide(int a, int b) {
    if (b == 0) {
        throw new DivideByZeroException("Il divisore non può essere zero");
    }
    return a / b;
}
```

### Diagramma: Flusso try-catch-finally

```
┌─────────────────────────────────────────────┐
│  try { ... }                                 │
│    │                                         │
│    ├─► Esecuzione normale                    │
│    │                                         │
│    ├─► Eccezione generata?                   │
│    │   │                                     │
│    │   ├─► NO ────────────────┐              │
│    │   │                      │              │
│    │   └─► SÌ                 │              │
│    │       │                  │              │
│    │       ▼                  │              │
│    │   ┌──────────────┐       │              │
│    │   │ catch { ... }│       │              │
│    │   └──────────────┘       │              │
│    │                          │              │
│    └──────────────────────────┘              │
│                    │                          │
│                    ▼                          │
│            ┌──────────────┐                   │
│            │ finally { }  │                   │
│            │ (sempre)     │                   │
│            └──────────────┘                   │
└─────────────────────────────────────────────┘
```

---

## 3. Gerarchia delle Eccezioni

### Gerarchia Base

```csharp
System.Object
    └── System.Exception (classe base)
        ├── System.SystemException
        │   ├── ArgumentException
        │   │   ├── ArgumentNullException
        │   │   └── ArgumentOutOfRangeException
        │   ├── NullReferenceException
        │   ├── IndexOutOfRangeException
        │   ├── DivideByZeroException
        │   ├── InvalidOperationException
        │   └── NotImplementedException
        └── System.ApplicationException
            └── (Eccezioni personalizzate)
```

### Diagramma: Gerarchia

```
┌─────────────────────────────────────────────┐
│         System.Exception                    │
│         (classe base)                       │
└─────────────────────────────────────────────┘
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│System     │ │Application│ │Custom     │
│Exception  │ │Exception  │ │Exception  │
│           │ │           │ │           │
│- NullRef  │ │- User     │ │- Business │
│- DivideBy │ │  Defined  │ │  Logic    │
│- IndexOut │ │           │ │           │
└───────────┘ └───────────┘ └───────────┘
```

---

## 4. Eccezioni Comuni

### NullReferenceException

```csharp
// ❌ ERRORE: Oggetto null
string text = null;
int length = text.Length; // NullReferenceException!

// ✅ CORRETTO: Controllo null
string text = null;
int length = text?.Length ?? 0; // Usa null-conditional operator
```

### ArgumentException

```csharp
public void SetAge(int age) {
    if (age < 0 || age > 150) {
        throw new ArgumentOutOfRangeException(
            nameof(age), 
            "L'età deve essere tra 0 e 150"
        );
    }
    // ...
}
```

### IndexOutOfRangeException

```csharp
// ❌ ERRORE: Indice fuori range
int[] numbers = { 1, 2, 3 };
int value = numbers[10]; // IndexOutOfRangeException!

// ✅ CORRETTO: Controllo bounds
if (index >= 0 && index < numbers.Length) {
    int value = numbers[index];
}
```

### InvalidOperationException

```csharp
public class Stack<T> {
    private List<T> items = new List<T>();
    
    public T Pop() {
        if (items.Count == 0) {
            throw new InvalidOperationException("Lo stack è vuoto");
        }
        var item = items[items.Count - 1];
        items.RemoveAt(items.Count - 1);
        return item;
    }
}
```

---

## 5. Creare Eccezioni Personalizzate

### Eccezione Personalizzata Base

```csharp
// Eccezione personalizzata
public class InsufficientFundsException : Exception {
    public decimal Balance { get; }
    public decimal RequestedAmount { get; }
    
    public InsufficientFundsException(
        decimal balance, 
        decimal requestedAmount
    ) : base($"Fondi insufficienti. Saldo: {balance:C}, Richiesto: {requestedAmount:C}") {
        Balance = balance;
        RequestedAmount = requestedAmount;
    }
    
    // Costruttore con messaggio personalizzato
    public InsufficientFundsException(
        string message, 
        decimal balance, 
        decimal requestedAmount
    ) : base(message) {
        Balance = balance;
        RequestedAmount = requestedAmount;
    }
}
```

### Utilizzo

```csharp
public class BankAccount {
    private decimal balance;
    
    public void Withdraw(decimal amount) {
        if (amount > balance) {
            throw new InsufficientFundsException(
                balance, 
                amount
            );
        }
        balance -= amount;
    }
}

// Gestione
try {
    account.Withdraw(1000);
}
catch (InsufficientFundsException ex) {
    Console.WriteLine($"Errore: {ex.Message}");
    Console.WriteLine($"Saldo disponibile: {ex.Balance:C}");
}
```

---

## 6. Catch Multipli e Ordine

### Ordine Importante

```csharp
try {
    RiskyOperation();
}
catch (DivideByZeroException ex) {
    // Gestisce DivideByZeroException
    Console.WriteLine("Divisione per zero");
}
catch (ArgumentException ex) {
    // Gestisce ArgumentException
    Console.WriteLine("Argomento non valido");
}
catch (Exception ex) {
    // DEVE essere l'ultimo! Gestisce tutte le altre
    Console.WriteLine($"Errore generico: {ex.Message}");
}
```

### ⚠️ Errore Comune

```csharp
// ❌ SBAGLIATO: Exception prima delle specifiche
try {
    RiskyOperation();
}
catch (Exception ex) {
    // Questo cattura TUTTO, le altre catch non vengono mai raggiunte!
}
catch (DivideByZeroException ex) {
    // ⚠️ MAI RAGGIUNTO! Compile error
}
```

### Diagramma: Ordine Catch

```
┌─────────────────────────────────────────────┐
│  Eccezione generata                         │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  DivideByZeroException│
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  catch (DivideByZero) │ ← Controlla prima
        └───────────────────────┘
                    │
                    ▼ (se non matcha)
        ┌───────────────────────┐
        │  catch (ArgumentException)│ ← Poi questa
        └───────────────────────┘
                    │
                    ▼ (se non matcha)
        ┌───────────────────────┐
        │  catch (Exception)     │ ← Infine questa
        └───────────────────────┘
```

---

## 7. Finally Block

### Utilizzo Finally

```csharp
FileStream file = null;
try {
    file = File.OpenRead("data.txt");
    // Operazioni sul file
}
catch (FileNotFoundException ex) {
    Console.WriteLine($"File non trovato: {ex.Message}");
}
finally {
    // SEMPRE eseguito, anche se c'è un'eccezione
    file?.Close(); // Chiude il file sempre
    Console.WriteLine("Risorse rilasciate");
}
```

### Using Statement (Alternativa a finally)

```csharp
// ✅ CORRETTO: Using statement (dispose automatico)
using (var file = File.OpenRead("data.txt")) {
    // Operazioni sul file
    // File chiuso automaticamente anche in caso di eccezione
}

// ✅ CORRETTO: Using declaration (C# 8+)
using var file = File.OpenRead("data.txt");
// File chiuso automaticamente alla fine dello scope
```

---

## 8. Throw e Re-throw

### Throw

```csharp
public void ProcessData(string data) {
    if (string.IsNullOrEmpty(data)) {
        throw new ArgumentNullException(nameof(data), "Data non può essere null o vuota");
    }
    // ...
}
```

### Re-throw (Rilanciare)

```csharp
try {
    RiskyOperation();
}
catch (Exception ex) {
    // Log dell'errore
    LogError(ex);
    
    // Rilancia l'eccezione originale
    throw; // ✅ Mantiene lo stack trace originale
    
    // ❌ SBAGLIATO: throw ex; (perde stack trace)
}
```

### Throw Nuova Eccezione

```csharp
try {
    RiskyOperation();
}
catch (SpecificException ex) {
    // Wrap in un'altra eccezione
    throw new CustomException("Errore durante operazione", ex);
    //                                    ↑ InnerException
}
```

---

## 9. Exception Properties

### Proprietà Utili

```csharp
try {
    RiskyOperation();
}
catch (Exception ex) {
    // Messaggio dell'eccezione
    Console.WriteLine(ex.Message);
    
    // Stack trace (dove è avvenuto l'errore)
    Console.WriteLine(ex.StackTrace);
    
    // Tipo dell'eccezione
    Console.WriteLine(ex.GetType().Name);
    
    // Eccezione interna (se presente)
    if (ex.InnerException != null) {
        Console.WriteLine($"Inner: {ex.InnerException.Message}");
    }
    
    // Source (assembly che ha generato l'eccezione)
    Console.WriteLine(ex.Source);
    
    // Help link
    Console.WriteLine(ex.HelpLink);
}
```

### Esempio Completo

```csharp
try {
    int result = Divide(10, 0);
}
catch (DivideByZeroException ex) {
    Console.WriteLine($"Message: {ex.Message}");
    Console.WriteLine($"Type: {ex.GetType().Name}");
    Console.WriteLine($"StackTrace:\n{ex.StackTrace}");
    Console.WriteLine($"Source: {ex.Source}");
}
```

---

## 10. Best Practices

### ✅ Cosa Fare

1. **Cattura eccezioni specifiche**
   ```csharp
   // ✅ CORRETTO
   catch (FileNotFoundException ex) {
       // Gestione specifica
   }
   ```

2. **Usa finally per pulizia risorse**
   ```csharp
   // ✅ CORRETTO
   finally {
       resource?.Dispose();
   }
   ```

3. **Fornisci messaggi informativi**
   ```csharp
   // ✅ CORRETTO
   throw new ArgumentException("Il valore deve essere positivo", nameof(value));
   ```

4. **Logga le eccezioni**
   ```csharp
   // ✅ CORRETTO
   catch (Exception ex) {
       logger.LogError(ex, "Errore durante operazione");
       throw;
   }
   ```

### ❌ Cosa Evitare

1. **Non catturare Exception generica senza motivo**
   ```csharp
   // ❌ SBAGLIATO
   try {
       // tutto il codice
   }
   catch (Exception ex) {
       // Ignora tutto
   }
   ```

2. **Non nascondere eccezioni**
   ```csharp
   // ❌ SBAGLIATO
   catch (Exception ex) {
       // Niente! L'errore è nascosto
   }
   ```

3. **Non usare eccezioni per controllo di flusso**
   ```csharp
   // ❌ SBAGLIATO
   try {
       return array[index];
   }
   catch (IndexOutOfRangeException) {
       return default;
   }
   
   // ✅ CORRETTO
   if (index >= 0 && index < array.Length) {
       return array[index];
   }
   return default;
   ```

---

## 11. Exception Handling Patterns

### Pattern: Try-Parse

```csharp
// Pattern comune per conversioni
if (int.TryParse(input, out int result)) {
    // Conversione riuscita
    Console.WriteLine($"Numero: {result}");
}
else {
    // Conversione fallita (senza eccezione!)
    Console.WriteLine("Input non valido");
}
```

### Pattern: Guard Clauses

```csharp
public void ProcessOrder(Order order) {
    // Guard clauses all'inizio
    if (order == null) {
        throw new ArgumentNullException(nameof(order));
    }
    
    if (order.Items.Count == 0) {
        throw new ArgumentException("L'ordine deve contenere almeno un item", nameof(order));
    }
    
    // Logica principale
    // ...
}
```

### Pattern: Result Object

```csharp
public class Result<T> {
    public bool Success { get; set; }
    public T Data { get; set; }
    public string ErrorMessage { get; set; }
    
    public static Result<T> Ok(T data) => new Result<T> { Success = true, Data = data };
    public static Result<T> Fail(string error) => new Result<T> { Success = false, ErrorMessage = error };
}

// Utilizzo (senza eccezioni)
public Result<int> Divide(int a, int b) {
    if (b == 0) {
        return Result<int>.Fail("Divisione per zero");
    }
    return Result<int>.Ok(a / b);
}
```

---

## 12. Exception Handling in Async/Await

### Gestione Eccezioni Async

```csharp
public async Task<string> DownloadDataAsync() {
    try {
        var data = await httpClient.GetStringAsync("url");
        return data;
    }
    catch (HttpRequestException ex) {
        // Gestisce errori HTTP
        Console.WriteLine($"Errore HTTP: {ex.Message}");
        throw;
    }
    catch (TaskCanceledException ex) {
        // Gestisce timeout/cancellazione
        Console.WriteLine("Operazione cancellata");
        throw;
    }
}

// Utilizzo
try {
    var data = await DownloadDataAsync();
}
catch (Exception ex) {
    Console.WriteLine($"Errore: {ex.Message}");
}
```

### AggregateException

```csharp
try {
    var tasks = new[] {
        Task.Run(() => Operation1()),
        Task.Run(() => Operation2()),
        Task.Run(() => Operation3())
    };
    
    await Task.WhenAll(tasks);
}
catch (AggregateException ex) {
    // Gestisce multiple eccezioni
    foreach (var innerEx in ex.InnerExceptions) {
        Console.WriteLine($"Errore: {innerEx.Message}");
    }
}
```

---

## 13. Domande Frequenti (FAQ)

### Q: Qual è la differenza tra throw e throw ex?
**R:** `throw` mantiene lo stack trace originale. `throw ex` resetta lo stack trace, perdendo informazioni utili per il debug.

### Q: Quando usare finally vs using?
**R:** `finally` per qualsiasi pulizia. `using` specificamente per oggetti che implementano `IDisposable`.

### Q: È meglio usare eccezioni o valori di ritorno per errori?
**R:** Eccezioni per errori eccezionali. Valori di ritorno (Result pattern) per errori previsti nel flusso normale.

### Q: Come gestire eccezioni in applicazioni multi-thread?
**R:** Ogni thread gestisce le proprie eccezioni. Usa `AggregateException` per operazioni parallele.

---

## Conclusioni

La gestione delle eccezioni è essenziale per:
- ✅ Scrivere codice robusto e affidabile
- ✅ Separare logica di business da gestione errori
- ✅ Fornire informazioni utili per il debug
- ✅ Gestire situazioni impreviste in modo controllato

Ricorda: usa eccezioni per situazioni eccezionali, non per controllo di flusso normale!

---

_Documento creato per spiegare la gestione delle eccezioni in C# con esempi pratici e best practices._

