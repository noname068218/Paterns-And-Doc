# Domande e Risposte per Colloqui - .NET e C#

## 1. Cos'è .NET e qual è la differenza tra .NET Framework e .NET Core/.NET 5+?

**Risposta:**
.NET è una piattaforma di sviluppo creata da Microsoft per costruire applicazioni cross-platform.

**Differenze principali:**

- **.NET Framework**: Solo Windows, closed-source, versione legacy
- **.NET Core/.NET 5+**: Cross-platform (Windows, Linux, macOS), open-source, moderno, modulare

**.NET 5+** è l'evoluzione che unifica .NET Framework e .NET Core in un'unica piattaforma.

---

## 2. Spiega la differenza tra value types e reference types.

**Risposta:**

**Value Types:**
- Memorizzati nello stack
- Contengono direttamente il valore
- Esempi: `int`, `double`, `bool`, `struct`, `enum`
- Assegnazione crea una copia

```csharp
int a = 10;
int b = a; // Copia il valore
b = 20;    // a rimane 10
```

**Reference Types:**
- Memorizzati nello heap
- Contengono un riferimento (puntatore) al valore
- Esempi: `class`, `interface`, `delegate`, `array`, `string`
- Assegnazione copia il riferimento

```csharp
Person p1 = new Person { Name = "Mario" };
Person p2 = p1; // Copia il riferimento
p2.Name = "Luigi"; // p1.Name diventa anche "Luigi"
```

---

## 3. Cos'è il Garbage Collector (GC) e come funziona?

**Risposta:**
Il Garbage Collector è un meccanismo automatico di gestione della memoria che libera oggetti non più referenziati.

**Come funziona:**
1. **Mark**: Identifica oggetti raggiungibili (root references)
2. **Sweep**: Rimuove oggetti non marcati
3. **Compact**: Compatta la memoria per ridurre frammentazione

**Generazioni:**
- **Gen 0**: Oggetti appena creati (raccolti più frequentemente)
- **Gen 1**: Oggetti sopravvissuti a una raccolta
- **Gen 2**: Oggetti long-lived (raccolti meno frequentemente)

**Best Practices:**
- Evita chiamate esplicite a `GC.Collect()`
- Usa `using` per `IDisposable`
- Evita finalizers quando possibile

---

## 4. Spiega async/await in C#.

**Risposta:**
`async/await` permette di scrivere codice asincrono in modo sincrono, migliorando la responsiveness delle applicazioni.

```csharp
// Metodo asincrono
public async Task<string> FetchDataAsync()
{
    using var client = new HttpClient();
    return await client.GetStringAsync("https://api.example.com/data");
}

// Utilizzo
var data = await FetchDataAsync();
```

**Punti chiave:**
- `async` rende il metodo asincrono
- `await` attende il completamento senza bloccare il thread
- Restituisce `Task` o `Task<T>`
- Non blocca il thread UI in applicazioni desktop/web

**Best Practices:**
- Usa `ConfigureAwait(false)` in librerie
- Evita `async void` (solo per event handlers)
- Usa `Task.Run` solo per CPU-bound operations

---

## 5. Cos'è LINQ e quali sono i suoi vantaggi?

**Risposta:**
LINQ (Language Integrated Query) permette di interrogare collezioni usando una sintassi simile a SQL.

**Vantaggi:**
- Sintassi dichiarativa e leggibile
- Type-safe (controllo a compile-time)
- Supporta query su collezioni, database, XML

**Esempi:**

```csharp
// Query syntax
var result = from user in users
             where user.Age > 18
             orderby user.Name
             select user.Name;

// Method syntax
var result = users
    .Where(u => u.Age > 18)
    .OrderBy(u => u.Name)
    .Select(u => u.Name);
```

**Operatori principali:**
- `Where`, `Select`, `OrderBy`, `GroupBy`
- `First`, `FirstOrDefault`, `Single`, `Any`, `All`
- `Join`, `GroupJoin`, `Aggregate`

---

## 6. Spiega i Generics in C#.

**Risposta:**
I Generics permettono di definire classi, interfacce e metodi con placeholder di tipo, garantendo type safety e riusabilità.

```csharp
// Classe generica
public class Repository<T>
{
    private List<T> items = new List<T>();
    
    public void Add(T item) => items.Add(item);
    public T GetById(int id) => items[id];
}

// Utilizzo
var userRepo = new Repository<User>();
var productRepo = new Repository<Product>();
```

**Vantaggi:**
- Type safety a compile-time
- Elimina boxing/unboxing per value types
- Codice riutilizzabile
- Performance migliori

**Constraints:**
```csharp
public class Repository<T> where T : class, new()
{
    // T deve essere una classe con costruttore senza parametri
}
```

---

## 7. Cos'è Dependency Injection (DI) e perché è importante?

**Risposta:**
Dependency Injection è un pattern che inverte il controllo delle dipendenze, rendendo il codice più testabile e manutenibile.

**Problema senza DI:**
```csharp
// ❌ Accoppiamento stretto
public class OrderService
{
    private EmailService emailService = new EmailService(); // Dipendenza hardcoded
}
```

**Soluzione con DI:**
```csharp
// ✅ Iniezione di dipendenza
public class OrderService
{
    private readonly IEmailService _emailService;
    
    public OrderService(IEmailService emailService)
    {
        _emailService = emailService; // Dipendenza iniettata
    }
}
```

**Vantaggi:**
- Testabilità (mock delle dipendenze)
- Basso accoppiamento
- Flessibilità (facile cambiare implementazioni)
- Manutenibilità

**Container DI (.NET):**
```csharp
services.AddScoped<IEmailService, EmailService>();
services.AddTransient<IOrderService, OrderService>();
```

---

## 8. Spiega i Delegates e gli Events.

**Risposta:**

**Delegates:**
Un delegate è un tipo che rappresenta riferimenti a metodi con una firma specifica.

```csharp
// Definizione delegate
public delegate int MathOperation(int a, int b);

// Utilizzo
MathOperation add = (a, b) => a + b;
MathOperation multiply = (a, b) => a * b;

int result = add(5, 3); // 8
```

**Events:**
Gli eventi sono un pattern basato su delegates per implementare il pattern Observer.

```csharp
public class Button
{
    public event EventHandler Clicked;
    
    public void Click()
    {
        Clicked?.Invoke(this, EventArgs.Empty);
    }
}

// Sottoscrizione
button.Clicked += (sender, e) => Console.WriteLine("Button clicked!");
```

**Action e Func:**
```csharp
Action<string> log = message => Console.WriteLine(message);
Func<int, int, int> add = (a, b) => a + b;
```

---

## 9. Cos'è la Reflection e quando usarla?

**Risposta:**
La Reflection permette di ispezionare e manipolare metadati di tipi a runtime.

**Esempi:**

```csharp
// Ottenere informazioni su un tipo
Type type = typeof(Person);
PropertyInfo[] properties = type.GetProperties();

// Creare istanza dinamicamente
object instance = Activator.CreateInstance(type);

// Invocare metodi
MethodInfo method = type.GetMethod("GetName");
string result = (string)method.Invoke(instance, null);
```

**Quando usarla:**
- Serializzazione/Deserializzazione
- ORM (Entity Framework)
- Dependency Injection containers
- Attribute-based programming

**Svantaggi:**
- Performance più lente
- Nessun type safety a compile-time
- Codice più complesso

---

## 10. Spiega i Nullable Types.

**Risposta:**
I Nullable Types permettono di assegnare `null` a value types.

```csharp
int? nullableInt = null;
bool? nullableBool = null;
DateTime? nullableDate = null;
```

**Verifica e accesso:**
```csharp
int? number = 42;

if (number.HasValue)
{
    int value = number.Value;
}

// Null-coalescing operator
int result = number ?? 0;

// Null-conditional operator
string? name = person?.Name;
```

**Nullable Reference Types (C# 8.0+):**
```csharp
#nullable enable

string? nullableString = null; // Esplicitamente nullable
string nonNullableString = "Hello"; // Non può essere null
```

---

## 11. Cos'è il Boxing e Unboxing?

**Risposta:**

**Boxing:**
Conversione di un value type in `object` (reference type), copiando il valore nello heap.

```csharp
int value = 42;
object boxed = value; // Boxing
```

**Unboxing:**
Conversione di un `object` in un value type, copiando il valore dallo heap.

```csharp
object boxed = 42;
int unboxed = (int)boxed; // Unboxing
```

**Problemi:**
- Performance overhead (allocazione memoria)
- Possibili `InvalidCastException` durante unboxing

**Soluzione:**
Usa Generics per evitare boxing/unboxing:
```csharp
List<int> numbers = new List<int>(); // Nessun boxing
```

---

## 12. Spiega i Modificatori di Accesso.

**Risposta:**

- **public**: Accessibile ovunque
- **private**: Solo dentro la classe
- **protected**: Classe e sottoclassi
- **internal**: Solo dentro lo stesso assembly
- **protected internal**: Protected O internal
- **private protected**: Protected E internal (C# 7.2+)

```csharp
public class Example
{
    public int PublicField;
    private int PrivateField;
    protected int ProtectedField;
    internal int InternalField;
}
```

---

## 13. Cos'è il Pattern Matching?

**Risposta:**
Il Pattern Matching permette di confrontare valori con pattern specifici (C# 7.0+).

```csharp
// Switch expression
string result = value switch
{
    int i when i > 0 => "Positive number",
    int i when i < 0 => "Negative number",
    int => "Zero",
    string s => $"String: {s}",
    null => "Null",
    _ => "Unknown"
};

// Type pattern
if (obj is Person p)
{
    Console.WriteLine(p.Name);
}
```

---

## 14. Spiega i Records (C# 9.0+).

**Risposta:**
I Records sono tipi immutabili per rappresentare dati.

```csharp
// Record semplice
public record Person(string Name, int Age);

// Utilizzo
var person = new Person("Mario", 30);
var person2 = person with { Age = 31 }; // Crea nuova istanza

// Record con metodi
public record Person(string Name, int Age)
{
    public string Greet() => $"Hello, I'm {Name}";
}
```

**Caratteristiche:**
- Immutabilità
- Value-based equality
- `with` expressions per creare copie modificate
- Sintassi concisa

---

## 15. Cos'è la Memoria Stack e Heap?

**Risposta:**

**Stack:**
- Memoria veloce e limitata
- Gestita automaticamente (LIFO)
- Contiene value types e riferimenti
- Allocazione/deallocazione veloce

**Heap:**
- Memoria più lenta ma più grande
- Gestita dal Garbage Collector
- Contiene oggetti (reference types)
- Frammentazione possibile

**Esempio:**
```csharp
int number = 10;        // Stack
Person person = new Person(); // Heap (oggetto), Stack (riferimento)
```

---

*Documento creato per la preparazione ai colloqui tecnici - .NET e C#*

