# List e Collezioni in C#

## Introduzione

Le **collezioni** in C# sono strutture dati che permettono di memorizzare e gestire gruppi di oggetti. La classe `List<T>` è una delle collezioni più utilizzate e fornisce un'implementazione dinamica e type-safe di array.

---

## 1. List<T>: Collezione Dinamica

### Definizione

`List<T>` è una collezione generica che rappresenta un elenco di oggetti accessibili per indice. Supporta ricerca, ordinamento e manipolazione degli elementi.

### Caratteristiche Principali

- ✅ **Dinamica**: Cresce automaticamente
- ✅ **Type-safe**: Type-safe a compile-time
- ✅ **Accesso per indice**: Come un array
- ✅ **Metodi utili**: Add, Remove, Find, Sort, ecc.

### Sintassi Base

```csharp
// Creazione di una List
List<T> lista = new List<T>();

// Con inizializzazione
List<T> lista = new List<T> { elemento1, elemento2, ... };

// Con capacità iniziale
List<T> lista = new List<T>(capacita);
```

### Esempio Base

```csharp
// List di interi
List<int> numeri = new List<int>();
numeri.Add(10);
numeri.Add(20);
numeri.Add(30);

// List di stringhe
List<string> nomi = new List<string> { "Mario", "Luigi", "Peach" };

// List di oggetti personalizzati
List<Persona> persone = new List<Persona> {
    new Persona { Nome = "Mario", Eta = 30 },
    new Persona { Nome = "Luigi", Eta = 25 }
};

// Accesso per indice
int primo = numeri[0];  // ✅ 10
string primoNome = nomi[0];  // ✅ "Mario"
```

### Diagramma: Struttura List<T>

```
┌─────────────────────────────────────────────┐
│  List<int> numeri                           │
├─────────────────────────────────────────────┤
│  [0]  [1]  [2]  [3]  [4]  ...              │
│   10   20   30   ?   ?                     │
│                                             │
│  Count = 3                                  │
│  Capacity = 4 (cresce automaticamente)     │
└─────────────────────────────────────────────┘
```

---

## 2. Metodi Principali di List<T>

### 2.1 Aggiunta Elementi

```csharp
List<string> nomi = new List<string>();

// Add - aggiunge un elemento
nomi.Add("Mario");
nomi.Add("Luigi");

// AddRange - aggiunge più elementi
nomi.AddRange(new[] { "Peach", "Toad" });

// Insert - inserisce in una posizione specifica
nomi.Insert(1, "Yoshi");  // Inserisce "Yoshi" alla posizione 1

// Risultato: ["Mario", "Yoshi", "Luigi", "Peach", "Toad"]
```

### Diagramma: Aggiunta Elementi

```
┌─────────────────────────────────────────────┐
│  List prima: ["Mario", "Luigi"]              │
└─────────────────────────────────────────────┘
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│ Add()     │ │Insert()   │ │AddRange() │
│ Alla fine │ │ A posizione│ │Più elementi│
└───────────┘ └───────────┘ └───────────┘
        │           │           │
        └───────────┼───────────┘
                    │
                    ▼
┌─────────────────────────────────────────────┐
│  List dopo: ["Mario", "Yoshi", "Luigi",     │
│            "Peach", "Toad"]                 │
└─────────────────────────────────────────────┘
```

### 2.2 Rimozione Elementi

```csharp
List<string> nomi = new List<string> { "Mario", "Luigi", "Peach", "Mario" };

// Remove - rimuove la prima occorrenza
nomi.Remove("Mario");  // Rimuove il primo "Mario"
// Risultato: ["Luigi", "Peach", "Mario"]

// RemoveAt - rimuove per indice
nomi.RemoveAt(0);  // Rimuove "Luigi"
// Risultato: ["Peach", "Mario"]

// RemoveAll - rimuove tutti gli elementi che soddisfano una condizione
nomi.RemoveAll(n => n == "Mario");  // Rimuove tutti i "Mario"
// Risultato: ["Peach"]

// Clear - rimuove tutti gli elementi
nomi.Clear();  // List vuota
```

### 2.3 Ricerca Elementi

```csharp
List<int> numeri = new List<int> { 10, 20, 30, 40, 50 };

// Contains - verifica se contiene un elemento
bool contiene = numeri.Contains(30);  // ✅ true

// IndexOf - trova l'indice della prima occorrenza
int indice = numeri.IndexOf(30);  // ✅ 2

// Find - trova il primo elemento che soddisfa una condizione
int primo = numeri.Find(n => n > 25);  // ✅ 30

// FindAll - trova tutti gli elementi che soddisfano una condizione
List<int> maggiori = numeri.FindAll(n => n > 25);
// Risultato: [30, 40, 50]

// Exists - verifica se esiste almeno un elemento che soddisfa una condizione
bool esiste = numeri.Exists(n => n > 100);  // ✅ false
```

### 2.4 Ordinamento

```csharp
List<int> numeri = new List<int> { 50, 20, 40, 10, 30 };

// Sort - ordina in-place
numeri.Sort();  // [10, 20, 30, 40, 50]

// Sort con comparatore
numeri.Sort((a, b) => b.CompareTo(a));  // Ordinamento decrescente
// Risultato: [50, 40, 30, 20, 10]

// Reverse - inverte l'ordine
numeri.Reverse();  // [10, 20, 30, 40, 50]
```

### Diagramma: Ordinamento

```
┌─────────────────────────────────────────────┐
│  Prima: [50, 20, 40, 10, 30]               │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Sort()               │
        └───────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────┐
│  Dopo: [10, 20, 30, 40, 50]                 │
└─────────────────────────────────────────────┘
```

---

## 3. Iterazione su List<T>

### 3.1 Foreach Loop

```csharp
List<string> nomi = new List<string> { "Mario", "Luigi", "Peach" };

// Foreach - iterazione semplice
foreach (string nome in nomi) {
    Console.WriteLine(nome);
}

// Foreach con indice (C# 8+)
foreach (var (nome, indice) in nomi.Select((n, i) => (n, i))) {
    Console.WriteLine($"{indice}: {nome}");
}
```

### 3.2 For Loop

```csharp
List<string> nomi = new List<string> { "Mario", "Luigi", "Peach" };

// For - iterazione con indice
for (int i = 0; i < nomi.Count; i++) {
    Console.WriteLine($"{i}: {nomi[i]}");
}

// For reverse - iterazione all'indietro
for (int i = nomi.Count - 1; i >= 0; i--) {
    Console.WriteLine(nomi[i]);
}
```

### 3.3 LINQ

```csharp
List<int> numeri = new List<int> { 1, 2, 3, 4, 5 };

// Select - trasforma elementi
var quadrati = numeri.Select(n => n * n).ToList();
// Risultato: [1, 4, 9, 16, 25]

// Where - filtra elementi
var pari = numeri.Where(n => n % 2 == 0).ToList();
// Risultato: [2, 4]

// Aggregate - riduce a un valore
int somma = numeri.Aggregate(0, (acc, n) => acc + n);
// Risultato: 15
```

### Diagramma: Iterazione

```
┌─────────────────────────────────────────────┐
│  List: ["Mario", "Luigi", "Peach"]         │
└─────────────────────────────────────────────┘
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│  foreach  │ │   for     │ │   LINQ    │
│           │ │           │ │           │
│  Semplice │ │  Con      │ │  Query    │
│           │ │  indice    │ │  syntax   │
└───────────┘ └───────────┘ └───────────┘
```

---

## 4. Altre Collezioni Generiche

### 4.1 Dictionary<TKey, TValue>

```csharp
// Dictionary - collezione chiave-valore
Dictionary<string, int> eta = new Dictionary<string, int>();

// Aggiunta
eta.Add("Mario", 30);
eta.Add("Luigi", 25);
eta["Peach"] = 28;  // Sintassi alternativa

// Accesso
int etaMario = eta["Mario"];  // ✅ 30

// Verifica esistenza
if (eta.ContainsKey("Mario")) {
    int eta = eta["Mario"];
}

// Iterazione
foreach (var kvp in eta) {
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
}
```

### Diagramma: Dictionary

```
┌─────────────────────────────────────────────┐
│  Dictionary<string, int>                    │
├─────────────────────────────────────────────┤
│  "Mario"  →  30                             │
│  "Luigi"  →  25                             │
│  "Peach"  →  28                             │
└─────────────────────────────────────────────┘
        │
        ▼
    Accesso O(1)
    tramite chiave
```

### 4.2 HashSet<T>

```csharp
// HashSet - collezione di elementi unici
HashSet<int> numeri = new HashSet<int> { 1, 2, 3, 3, 4 };
// Risultato: {1, 2, 3, 4} - duplicati rimossi

// Aggiunta
numeri.Add(5);  // ✅ true
numeri.Add(3);  // ✅ false (già presente)

// Verifica esistenza
bool contiene = numeri.Contains(3);  // ✅ true

// Operazioni insiemistiche
HashSet<int> altri = new HashSet<int> { 3, 4, 5, 6 };

// Intersezione
numeri.IntersectWith(altri);  // {3, 4, 5}

// Unione
numeri.UnionWith(altri);  // {1, 2, 3, 4, 5, 6}
```

### 4.3 Queue<T> e Stack<T>

```csharp
// Queue - FIFO (First In First Out)
Queue<string> coda = new Queue<string>();
coda.Enqueue("Primo");
coda.Enqueue("Secondo");
coda.Enqueue("Terzo");

string primo = coda.Dequeue();  // ✅ "Primo"
string secondo = coda.Dequeue();  // ✅ "Secondo"

// Stack - LIFO (Last In First Out)
Stack<string> pila = new Stack<string>();
pila.Push("Primo");
pila.Push("Secondo");
pila.Push("Terzo");

string ultimo = pila.Pop();  // ✅ "Terzo"
string penultimo = pila.Pop();  // ✅ "Secondo"
```

### Diagramma: Queue e Stack

```
┌─────────────────────────────────────────────┐
│  QUEUE (FIFO)                               │
│  Enqueue → [Primo][Secondo][Terzo] → Dequeue│
│  Ordine: 1, 2, 3                            │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  STACK (LIFO)                               │
│  Push → [Terzo][Secondo][Primo] → Pop      │
│  Ordine: 3, 2, 1                            │
└─────────────────────────────────────────────┘
```

---

## 5. Interfacce delle Collezioni

### Gerarchia delle Interfacce

```csharp
// ICollection<T> - interfaccia base
public interface ICollection<T> : IEnumerable<T> {
    int Count { get; }
    bool IsReadOnly { get; }
    void Add(T item);
    void Clear();
    bool Contains(T item);
    void CopyTo(T[] array, int arrayIndex);
    bool Remove(T item);
}

// IList<T> - accesso per indice
public interface IList<T> : ICollection<T> {
    T this[int index] { get; set; }
    int IndexOf(T item);
    void Insert(int index, T item);
    void RemoveAt(int index);
}

// IDictionary<TKey, TValue> - chiave-valore
public interface IDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>> {
    TValue this[TKey key] { get; set; }
    ICollection<TKey> Keys { get; }
    ICollection<TValue> Values { get; }
    void Add(TKey key, TValue value);
    bool ContainsKey(TKey key);
    bool Remove(TKey key);
    bool TryGetValue(TKey key, out TValue value);
}
```

### Diagramma: Gerarchia Interfacce

```
┌─────────────────────────────────────────────┐
│      IEnumerable<T>                         │
│      (Iterazione)                           │
└─────────────────────────────────────────────┘
                    ▲
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│ICollection│ │  IList<T> │ │IDictionary│
│<T>        │ │           │ │<TKey, TVal>│
│           │ │+ Indexer  │ │+ Keys      │
│+ Add()    │ │+ IndexOf()│ │+ Values    │
│+ Remove() │ │+ Insert() │ │+ TryGetVal │
└───────────┘ └───────────┘ └───────────┘
```

---

## 6. Performance e Complessità

### Tabella Complessità

| Operazione | List<T> | Dictionary<TKey, TValue> | HashSet<T> | Queue<T> | Stack<T> |
|------------|---------|--------------------------|------------|----------|----------|
| **Accesso per indice/chiave** | O(1) | O(1) | N/A | N/A | N/A |
| **Add/Insert** | O(1) amortizzato | O(1) | O(1) | O(1) | O(1) |
| **Remove** | O(n) | O(1) | O(1) | O(1) | O(1) |
| **Contains** | O(n) | O(1) | O(1) | O(n) | O(n) |
| **Find** | O(n) | N/A | O(1) | O(n) | O(n) |

### Diagramma: Performance

```
┌─────────────────────────────────────────────┐
│  LIST<T>                                     │
│  ✅ Accesso per indice: O(1)                 │
│  ⚠️  Ricerca: O(n)                           │
│  ⚠️  Inserimento in mezzo: O(n)             │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  DICTIONARY<TKey, TValue>                   │
│  ✅ Accesso per chiave: O(1)                 │
│  ✅ Inserimento: O(1)                       │
│  ✅ Rimozione: O(1)                         │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  HASHSET<T>                                  │
│  ✅ Verifica esistenza: O(1)                │
│  ✅ Inserimento: O(1)                       │
│  ✅ Rimozione: O(1)                         │
└─────────────────────────────────────────────┘
```

---

## 7. Esempi Pratici Completi

### Esempio 1: Gestione Ordini

```csharp
public class Ordine {
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public decimal Totale { get; set; }
}

public class GestoreOrdini {
    private List<Ordine> ordini = new List<Ordine>();
    
    public void AggiungiOrdine(Ordine ordine) {
        ordini.Add(ordine);
    }
    
    public List<Ordine> GetOrdiniPerData(DateTime data) {
        return ordini.Where(o => o.Data.Date == data.Date).ToList();
    }
    
    public List<Ordine> GetOrdiniPerTotale(decimal minimo) {
        return ordini.Where(o => o.Totale >= minimo)
                    .OrderByDescending(o => o.Totale)
                    .ToList();
    }
    
    public decimal GetTotaleComplessivo() {
        return ordini.Sum(o => o.Totale);
    }
}

// Utilizzo
var gestore = new GestoreOrdini();
gestore.AggiungiOrdine(new Ordine { Id = 1, Data = DateTime.Now, Totale = 100 });
gestore.AggiungiOrdine(new Ordine { Id = 2, Data = DateTime.Now, Totale = 200 });

var ordiniOggi = gestore.GetOrdiniPerData(DateTime.Now);
var totale = gestore.GetTotaleComplessivo();  // ✅ 300
```

### Esempio 2: Sistema di Cache

```csharp
public class Cache<TKey, TValue> {
    private Dictionary<TKey, TValue> cache = new Dictionary<TKey, TValue>();
    private Queue<TKey> ordineAccesso = new Queue<TKey>();
    private int capacitaMassima;
    
    public Cache(int capacitaMassima) {
        this.capacitaMassima = capacitaMassima;
    }
    
    public void Aggiungi(TKey chiave, TValue valore) {
        if (cache.Count >= capacitaMassima && !cache.ContainsKey(chiave)) {
            // Rimuovi il più vecchio
            TKey chiaveDaRimuovere = ordineAccesso.Dequeue();
            cache.Remove(chiaveDaRimuovere);
        }
        
        cache[chiave] = valore;
        ordineAccesso.Enqueue(chiave);
    }
    
    public bool ProvaOttieni(TKey chiave, out TValue valore) {
        return cache.TryGetValue(chiave, out valore);
    }
}

// Utilizzo
var cache = new Cache<string, string>(3);
cache.Aggiungi("chiave1", "valore1");
cache.Aggiungi("chiave2", "valore2");
cache.Aggiungi("chiave3", "valore3");

if (cache.ProvaOttieni("chiave1", out string valore)) {
    Console.WriteLine(valore);  // ✅ "valore1"
}
```

### Esempio 3: Sistema di Votazione

```csharp
public class SistemaVotazione {
    private Dictionary<string, HashSet<string>> voti = new Dictionary<string, HashSet<string>>();
    
    public void Vota(string candidato, string votante) {
        if (!voti.ContainsKey(candidato)) {
            voti[candidato] = new HashSet<string>();
        }
        
        voti[candidato].Add(votante);  // HashSet previene duplicati
    }
    
    public int GetNumeroVoti(string candidato) {
        return voti.ContainsKey(candidato) ? voti[candidato].Count : 0;
    }
    
    public string GetVincitore() {
        return voti.OrderByDescending(v => v.Value.Count)
                   .FirstOrDefault().Key;
    }
}

// Utilizzo
var sistema = new SistemaVotazione();
sistema.Vota("Mario", "Votante1");
sistema.Vota("Mario", "Votante2");
sistema.Vota("Luigi", "Votante3");
sistema.Vota("Mario", "Votante1");  // Duplicato ignorato

int votiMario = sistema.GetNumeroVoti("Mario");  // ✅ 2
string vincitore = sistema.GetVincitore();  // ✅ "Mario"
```

---

## 8. Best Practices

### ✅ Cosa Fare

1. **Usa List<T> per collezioni dinamiche**
   ```csharp
   // ✅ CORRETTO
   List<int> numeri = new List<int>();
   ```

2. **Usa Dictionary<TKey, TValue> per accesso rapido per chiave**
   ```csharp
   // ✅ CORRETTO
   Dictionary<string, Persona> persone = new Dictionary<string, Persona>();
   ```

3. **Usa HashSet<T> per elementi unici**
   ```csharp
   // ✅ CORRETTO
   HashSet<int> numeriUnici = new HashSet<int>();
   ```

4. **Specifica la capacità iniziale se conosci la dimensione**
   ```csharp
   // ✅ CORRETTO
   List<int> numeri = new List<int>(1000);  // Evita ridimensionamenti
   ```

### ❌ Cosa Evitare

1. **Non usare List<T> quando serve accesso per chiave**
   ```csharp
   // ❌ SBAGLIATO
   List<Persona> persone = new List<Persona>();
   // Cerca per nome - O(n)
   
   // ✅ CORRETTO
   Dictionary<string, Persona> persone = new Dictionary<string, Persona>();
   // Cerca per nome - O(1)
   ```

2. **Non modificare una collezione durante l'iterazione**
   ```csharp
   // ❌ SBAGLIATO
   foreach (var item in lista) {
       lista.Remove(item);  // ❌ Eccezione!
   }
   
   // ✅ CORRETTO
   for (int i = lista.Count - 1; i >= 0; i--) {
       lista.RemoveAt(i);
   }
   ```

---

## 9. Domande Frequenti (FAQ)

### Q: Qual è la differenza tra List<T> e Array?
**R:** List<T> è dinamica (cresce automaticamente), Array ha dimensione fissa. List<T> offre più metodi utili.

### Q: Quando usare Dictionary vs List?
**R:** Usa Dictionary quando devi accedere agli elementi per chiave. Usa List quando l'ordine e l'accesso per indice sono importanti.

### Q: Qual è la differenza tra HashSet e List?
**R:** HashSet garantisce elementi unici e ha accesso O(1) per Contains. List permette duplicati e ha accesso O(n) per Contains.

### Q: Come convertire una List in Array?
**R:** Usa `lista.ToArray()` o `lista.ToArray()`.

---

## Conclusioni

Le collezioni in C# sono fondamentali per:

- ✅ Gestire gruppi di dati in modo efficiente
- ✅ Organizzare e accedere ai dati rapidamente
- ✅ Implementare strutture dati comuni
- ✅ Scrivere codice type-safe e performante

`List<T>` è la collezione più utilizzata, ma Dictionary, HashSet, Queue e Stack hanno i loro casi d'uso specifici. Scegli la collezione giusta in base alle tue esigenze di accesso e performance.

---

*Documento creato per spiegare List e Collezioni in C# con esempi pratici e diagrammi.*

