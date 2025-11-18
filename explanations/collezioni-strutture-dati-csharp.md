# Collezioni e Strutture Dati in C#

## Introduzione

Le **collezioni** in C# sono strutture dati che permettono di memorizzare e gestire gruppi di oggetti. Comprendere le diverse collezioni e quando usarle è fondamentale per scrivere codice efficiente.

---

## 1. Gerarchia delle Collezioni

### Interfacce Base

```
┌─────────────────────────────────────────────┐
│         IEnumerable<T>                      │
│         (iterazione)                        │
└─────────────────────────────────────────────┘
                    ▲
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│ICollection│ │IReadOnly  │ │IQueryable │
│<T>        │ │Collection │ │<T>        │
│           │ │<T>        │ │           │
│Add/Remove │ │(sola lettura)│ │(LINQ)    │
└───────────┘ └───────────┘ └───────────┘
        ▲
        │
┌───────┼───────┐
│       │       │
▼       ▼       ▼
┌───────┐ ┌───────┐ ┌───────┐
│IList  │ │ISet   │ │IDict  │
│<T>    │ │<T>    │ │<TKey, │
│       │ │       │ │TValue>│
│Index  │ │Unique │ │Key-   │
│access │ │values │ │Value  │
└───────┘ └───────┘ └───────┘
```

---

## 2. List<T> - Lista Dinamica

### Caratteristiche

✅ Accesso per indice  
✅ Inserimento/rimozione dinamica  
✅ Duplicati permessi  
✅ Ordinamento supportato

### Utilizzo Base

```csharp
// Creazione
var list = new List<int> { 1, 2, 3, 4, 5 };

// Aggiunta elementi
list.Add(6);
list.AddRange(new[] { 7, 8, 9 });

// Accesso per indice
int first = list[0]; // 1
int last = list[list.Count - 1]; // 9

// Rimozione
list.Remove(5);
list.RemoveAt(0); // Rimuove primo elemento

// Ricerca
bool contains = list.Contains(3); // true
int index = list.IndexOf(3); // 2

// Ordinamento
list.Sort(); // Ordina in-place
var sorted = list.OrderBy(x => x).ToList(); // LINQ (nuova lista)
```

### Operazioni Comuni

```csharp
var numbers = new List<int> { 3, 1, 4, 1, 5, 9, 2, 6 };

// Filtraggio
var evens = numbers.Where(x => x % 2 == 0).ToList();

// Trasformazione
var doubled = numbers.Select(x => x * 2).ToList();

// Aggregazione
int sum = numbers.Sum();
int max = numbers.Max();
double avg = numbers.Average();

// Condizioni
bool allPositive = numbers.All(x => x > 0);
bool anyEven = numbers.Any(x => x % 2 == 0);
```

### Performance

| Operazione         | Complessità      |
| ------------------ | ---------------- |
| Accesso per indice | O(1)             |
| Ricerca elemento   | O(n)             |
| Inserimento fine   | O(1) amortizzato |
| Inserimento inizio | O(n)             |
| Rimozione          | O(n)             |

---

## 3. Dictionary<TKey, TValue> - Mappa Chiave-Valore

### Caratteristiche

✅ Accesso veloce per chiave  
✅ Chiavi uniche  
✅ Coppie chiave-valore  
✅ Lookup O(1) medio

### Utilizzo Base

```csharp
// Creazione
var dict = new Dictionary<string, int> {
    { "Mario", 30 },
    { "Luigi", 25 },
    { "Peach", 28 }
};

// Oppure
var dict2 = new Dictionary<string, int>();
dict2["Mario"] = 30;
dict2["Luigi"] = 25;

// Accesso
int marioAge = dict["Mario"]; // 30

// Controllo esistenza
if (dict.ContainsKey("Mario")) {
    int age = dict["Mario"];
}

// Oppure con TryGetValue (più efficiente)
if (dict.TryGetValue("Mario", out int age)) {
    Console.WriteLine($"Età: {age}");
}

// Iterazione
foreach (var kvp in dict) {
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
}

// Solo chiavi o valori
var keys = dict.Keys;
var values = dict.Values;
```

### Esempio Pratico

```csharp
// Conta occorrenze
var words = new[] { "apple", "banana", "apple", "cherry", "banana", "apple" };
var wordCount = new Dictionary<string, int>();

foreach (var word in words) {
    if (wordCount.ContainsKey(word)) {
        wordCount[word]++;
    }
    else {
        wordCount[word] = 1;
    }
}

// Oppure più elegante
var wordCount2 = words
    .GroupBy(w => w)
    .ToDictionary(g => g.Key, g => g.Count());
```

### Performance

| Operazione         | Complessità |
| ------------------ | ----------- |
| Accesso per chiave | O(1) medio  |
| Inserimento        | O(1) medio  |
| Rimozione          | O(1) medio  |
| Ricerca chiave     | O(1) medio  |

---

## 4. HashSet<T> - Insieme Unico

### Caratteristiche

✅ Elementi unici  
✅ Ricerca veloce O(1)  
✅ Operazioni di insieme  
✅ Nessun ordinamento garantito

### Utilizzo Base

```csharp
// Creazione
var set = new HashSet<int> { 1, 2, 3, 4, 5 };

// Aggiunta (ignora duplicati)
set.Add(6);
set.Add(3); // Non aggiunge (già presente)

// Controllo esistenza
bool contains = set.Contains(3); // true

// Rimozione
set.Remove(3);

// Operazioni di insieme
var set1 = new HashSet<int> { 1, 2, 3, 4 };
var set2 = new HashSet<int> { 3, 4, 5, 6 };

// Unione
var union = new HashSet<int>(set1);
union.UnionWith(set2); // { 1, 2, 3, 4, 5, 6 }

// Intersezione
var intersect = new HashSet<int>(set1);
intersect.IntersectWith(set2); // { 3, 4 }

// Differenza
var except = new HashSet<int>(set1);
except.ExceptWith(set2); // { 1, 2 }

// Sottoset
bool isSubset = set1.IsSubsetOf(set2); // false
```

### Quando Usare HashSet

```csharp
// ✅ Perfetto per: Controllo unicità veloce
var uniqueEmails = new HashSet<string>();
foreach (var user in users) {
    uniqueEmails.Add(user.Email); // Ignora duplicati automaticamente
}

// ✅ Perfetto per: Ricerca veloce
var allowedUsers = new HashSet<int> { 1, 2, 3, 5, 8 };
if (allowedUsers.Contains(userId)) {
    // Accesso consentito
}
```

---

## 5. Queue<T> - Coda FIFO

### Caratteristiche

✅ First In, First Out (FIFO)  
✅ Enqueue (aggiungi) / Dequeue (rimuovi)  
✅ Processamento ordinato

### Utilizzo Base

```csharp
// Creazione
var queue = new Queue<string>();

// Aggiunta (in coda)
queue.Enqueue("Primo");
queue.Enqueue("Secondo");
queue.Enqueue("Terzo");

// Rimozione (dalla testa)
string first = queue.Dequeue(); // "Primo"

// Guardare senza rimuovere
string next = queue.Peek(); // "Secondo"

// Controllo
bool isEmpty = queue.Count == 0;

// Iterazione (non rimuove)
foreach (var item in queue) {
    Console.WriteLine(item);
}
```

### Diagramma: Queue

```
┌─────────────────────────────────────────────┐
│  Queue (FIFO)                               │
│                                             │
│  Enqueue ──► [A] [B] [C] ──► Dequeue      │
│              ↑              ↑               │
│            Back          Front              │
└─────────────────────────────────────────────┘
```

### Esempio: Processamento Ordini

```csharp
var orderQueue = new Queue<Order>();

// Aggiungi ordini
orderQueue.Enqueue(new Order { Id = 1 });
orderQueue.Enqueue(new Order { Id = 2 });
orderQueue.Enqueue(new Order { Id = 3 });

// Processa in ordine
while (orderQueue.Count > 0) {
    var order = orderQueue.Dequeue();
    ProcessOrder(order);
}
```

---

## 6. Stack<T> - Pila LIFO

### Caratteristiche

✅ Last In, First Out (LIFO)  
✅ Push (aggiungi) / Pop (rimuovi)  
✅ Utile per algoritmi ricorsivi

### Utilizzo Base

```csharp
// Creazione
var stack = new Stack<int>();

// Aggiunta (in cima)
stack.Push(1);
stack.Push(2);
stack.Push(3);

// Rimozione (dalla cima)
int top = stack.Pop(); // 3

// Guardare senza rimuovere
int next = stack.Peek(); // 2

// Controllo
bool isEmpty = stack.Count == 0;
```

### Diagramma: Stack

```
┌─────────────────────────────────────────────┐
│  Stack (LIFO)                               │
│                                             │
│  Push ──► [C] ──► Pop                      │
│            │                                │
│           [B]                               │
│            │                                │
│           [A]                               │
│            │                                │
│          Top                                │
└─────────────────────────────────────────────┘
```

### Esempio: Validazione Parentesi

```csharp
public bool IsValidParentheses(string s) {
    var stack = new Stack<char>();

    foreach (char c in s) {
        if (c == '(' || c == '[' || c == '{') {
            stack.Push(c);
        }
        else if (c == ')' || c == ']' || c == '}') {
            if (stack.Count == 0) return false;

            char top = stack.Pop();
            if ((c == ')' && top != '(') ||
                (c == ']' && top != '[') ||
                (c == '}' && top != '{')) {
                return false;
            }
        }
    }

    return stack.Count == 0;
}
```

---

## 7. LinkedList<T> - Lista Collegata

### Caratteristiche

✅ Inserimento/rimozione O(1)  
✅ Accesso sequenziale  
✅ Utile per inserimenti frequenti

### Utilizzo Base

```csharp
// Creazione
var linkedList = new LinkedList<int>();

// Aggiunta
linkedList.AddLast(1);
linkedList.AddLast(2);
linkedList.AddFirst(0); // Aggiunge all'inizio

// Accesso
var first = linkedList.First; // LinkedListNode<int>
var last = linkedList.Last;

// Inserimento dopo un nodo
var node = linkedList.Find(1);
if (node != null) {
    linkedList.AddAfter(node, 1.5);
}

// Rimozione
linkedList.Remove(1);
linkedList.RemoveFirst();
linkedList.RemoveLast();
```

---

## 8. Confronto Performance

### Tabella Comparativa

| Collezione          | Accesso Indice | Ricerca | Inserimento | Rimozione | Quando Usare            |
| ------------------- | -------------- | ------- | ----------- | --------- | ----------------------- |
| **List<T>**         | O(1)           | O(n)    | O(1)\*      | O(n)      | Lista generale          |
| **Dictionary<K,V>** | O(1)           | O(1)    | O(1)        | O(1)      | Lookup per chiave       |
| **HashSet<T>**      | N/A            | O(1)    | O(1)        | O(1)      | Unicità, ricerca veloce |
| **Queue<T>**        | N/A            | O(n)    | O(1)        | O(1)      | FIFO processing         |
| **Stack<T>**        | N/A            | O(n)    | O(1)        | O(1)      | LIFO processing         |
| **LinkedList<T>**   | O(n)           | O(n)    | O(1)        | O(1)      | Inserimenti frequenti   |

\*O(1) amortizzato per inserimento alla fine

---

## 9. Collezioni Thread-Safe

### Concurrent Collections

```csharp
using System.Collections.Concurrent;

// ConcurrentDictionary
var concurrentDict = new ConcurrentDictionary<string, int>();
concurrentDict.TryAdd("key", 1);
concurrentDict.TryUpdate("key", 2, 1);

// ConcurrentQueue
var concurrentQueue = new ConcurrentQueue<int>();
concurrentQueue.Enqueue(1);
if (concurrentQueue.TryDequeue(out int value)) {
    Console.WriteLine(value);
}

// ConcurrentBag (non ordinato)
var concurrentBag = new ConcurrentBag<int>();
concurrentBag.Add(1);
if (concurrentBag.TryTake(out int item)) {
    Console.WriteLine(item);
}
```

---

## 10. Best Practices

### ✅ Cosa Fare

1. **Usa List<T> per collezioni generali**

   ```csharp
   var items = new List<string>();
   ```

2. **Usa Dictionary<K,V> per lookup veloce**

   ```csharp
   var lookup = new Dictionary<int, string>();
   ```

3. **Usa HashSet<T> per unicità**

   ```csharp
   var unique = new HashSet<int>();
   ```

4. **Inizializza con capacità se conosci la dimensione**
   ```csharp
   var list = new List<int>(1000); // Evita resize
   ```

### ❌ Cosa Evitare

1. **Non usare List<T> per lookup frequente**

   ```csharp
   // ❌ SBAGLIATO - O(n) per ricerca
   var list = new List<Person>();
   var person = list.FirstOrDefault(p => p.Id == 123);

   // ✅ CORRETTO - O(1) per ricerca
   var dict = new Dictionary<int, Person>();
   var person = dict[123];
   ```

2. **Non modificare collezione durante iterazione**

   ```csharp
   // ❌ SBAGLIATO
   foreach (var item in list) {
       list.Remove(item); // Exception!
   }

   // ✅ CORRETTO
   for (int i = list.Count - 1; i >= 0; i--) {
       list.RemoveAt(i);
   }
   ```

---

## 11. Esempi Pratici

### Esempio 1: Cache con Dictionary

```csharp
public class Cache<TKey, TValue> {
    private readonly Dictionary<TKey, TValue> _cache = new();
    private readonly int _maxSize;

    public Cache(int maxSize = 100) {
        _maxSize = maxSize;
    }

    public TValue Get(TKey key) {
        return _cache.TryGetValue(key, out var value) ? value : default;
    }

    public void Set(TKey key, TValue value) {
        if (_cache.Count >= _maxSize) {
            var firstKey = _cache.Keys.First();
            _cache.Remove(firstKey);
        }
        _cache[key] = value;
    }
}
```

### Esempio 2: Gestione Coda Processi

```csharp
public class JobProcessor {
    private readonly Queue<Job> _jobQueue = new();

    public void EnqueueJob(Job job) {
        _jobQueue.Enqueue(job);
    }

    public async Task ProcessJobsAsync() {
        while (_jobQueue.Count > 0) {
            var job = _jobQueue.Dequeue();
            await ProcessJobAsync(job);
        }
    }
}
```

---

## 12. Domande Frequenti (FAQ)

### Q: Quando usare List vs Array?

**R:** Usa `List<T>` quando la dimensione può cambiare. Usa `Array` quando la dimensione è fissa e conosciuta.

### Q: Qual è la differenza tra Dictionary e Hashtable?

**R:** `Dictionary<TKey, TValue>` è generico e type-safe. `Hashtable` è legacy e non generico.

### Q: HashSet mantiene l'ordine?

**R:** No, `HashSet<T>` non garantisce ordine. Usa `SortedSet<T>` se serve ordinamento.

### Q: Quando usare Queue vs Stack?

**R:** `Queue` per FIFO (first-come-first-served). `Stack` per LIFO (come undo/redo).

---

## Conclusioni

Scegli la collezione giusta per:

- ✅ Performance ottimali
- ✅ Codice più leggibile
- ✅ Operazioni efficienti
- ✅ Comportamento corretto

**Regola generale:**

- **List<T>**: Collezione generale
- **Dictionary<K,V>**: Lookup per chiave
- **HashSet<T>**: Unicità e ricerca veloce
- **Queue<T>**: Processamento FIFO
- **Stack<T>**: Processamento LIFO

---

_Documento creato per spiegare le collezioni in C# con esempi pratici e best practices._
