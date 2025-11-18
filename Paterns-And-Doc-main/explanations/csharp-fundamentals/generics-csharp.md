# Generics in C#

## Introduzione

I **Generics** (Generici) in C# permettono di definire classi, interfacce e metodi con placeholder per i tipi di dati. Questo consente di scrivere codice riutilizzabile, type-safe ed efficiente, evitando il boxing/unboxing e gli errori di tipo a runtime.

---

## 1. Cos'è un Generic?

### Definizione

Un **generic** è una classe, interfaccia o metodo che opera su un tipo parametrizzato. Il tipo viene specificato quando si usa il generic, permettendo di scrivere codice riutilizzabile per diversi tipi.

### Problema Senza Generics

```csharp
// ❌ PROBLEMA: Classe non type-safe, necessita boxing/unboxing
public class Stack {
    private object[] items;
    private int top;
    
    public void Push(object item) {
        items[top++] = item;  // Boxing per tipi valore
    }
    
    public object Pop() {
        return items[--top];  // Unboxing necessario
    }
}

// Utilizzo - errori possibili a runtime
Stack stack = new Stack();
stack.Push(10);  // int boxed in object
stack.Push("Ciao");  // string boxed in object
int numero = (int)stack.Pop();  // Unboxing - OK
string testo = (string)stack.Pop();  // Unboxing - OK
// Ma cosa succede se l'ordine è sbagliato? ❌
```

### Soluzione con Generics

```csharp
// ✅ SOLUZIONE: Type-safe, nessun boxing/unboxing
public class Stack<T> {
    private T[] items;
    private int top;
    
    public void Push(T item) {
        items[top++] = item;  // Nessun boxing!
    }
    
    public T Pop() {
        return items[--top];  // Nessun unboxing!
    }
}

// Utilizzo - type-safe a compile-time
Stack<int> intStack = new Stack<int>();
intStack.Push(10);  // ✅ OK
intStack.Push("Ciao");  // ❌ ERRORE a compile-time!
int numero = intStack.Pop();  // ✅ Type-safe

Stack<string> stringStack = new Stack<string>();
stringStack.Push("Ciao");  // ✅ OK
string testo = stringStack.Pop();  // ✅ Type-safe
```

### Diagramma: Generic vs Non-Generic

```
┌─────────────────────────────────────────────┐
│  Stack (Non-Generic)                        │
│  + Push(object)                             │
│  + Pop() : object                           │
│                                             │
│  Problemi:                                  │
│  - Boxing/Unboxing                         │
│  - Non type-safe                           │
│  - Errori a runtime                         │
└─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────┐
│  Stack<T> (Generic)                         │
│  + Push(T)                                  │
│  + Pop() : T                                │
│                                             │
│  Vantaggi:                                  │
│  - Type-safe                                │
│  - Nessun boxing                            │
│  - Errori a compile-time                    │
└─────────────────────────────────────────────┘
```

---

## 2. Generic Classes (Classi Generiche)

### Sintassi Base

```csharp
public class NomeClasse<T> {
    // T è il type parameter (parametro di tipo)
    private T campo;
    
    public T Proprieta { get; set; }
    
    public void Metodo(T parametro) {
        // Usa T come tipo
    }
    
    public T MetodoConRitorno() {
        return default(T);
    }
}
```

### Esempio Pratico: Coda Generica

```csharp
public class Coda<T> {
    private T[] elementi;
    private int front;
    private int rear;
    private int count;
    
    public Coda(int capacita) {
        elementi = new T[capacita];
        front = 0;
        rear = -1;
        count = 0;
    }
    
    public void Enqueue(T elemento) {
        if (count >= elementi.Length) {
            throw new InvalidOperationException("Coda piena");
        }
        rear = (rear + 1) % elementi.Length;
        elementi[rear] = elemento;
        count++;
    }
    
    public T Dequeue() {
        if (count == 0) {
            throw new InvalidOperationException("Coda vuota");
        }
        T elemento = elementi[front];
        front = (front + 1) % elementi.Length;
        count--;
        return elemento;
    }
    
    public bool IsEmpty => count == 0;
    public bool IsFull => count >= elementi.Length;
}

// Utilizzo
Coda<int> codaInteri = new Coda<int>(10);
codaInteri.Enqueue(1);
codaInteri.Enqueue(2);
int primo = codaInteri.Dequeue();  // ✅ Type-safe

Coda<string> codaStringhe = new Coda<string>(10);
codaStringhe.Enqueue("Primo");
codaStringhe.Enqueue("Secondo");
string primo = codaStringhe.Dequeue();  // ✅ Type-safe
```

### Diagramma: Classe Generic

```
┌─────────────────────────────────────────────┐
│  Coda<T>                                    │
│  - elementi: T[]                            │
│  - front: int                               │
│  - rear: int                                │
│  + Enqueue(T elemento)                      │
│  + Dequeue() : T                            │
│  + IsEmpty : bool                           │
└─────────────────────────────────────────────┘
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│Coda<int>  │ │Coda<string>│ │Coda<Persona>│
│           │ │           │ │           │
│Type-safe  │ │Type-safe  │ │Type-safe  │
└───────────┘ └───────────┘ └───────────┘
```

### Multiple Type Parameters

```csharp
// Classe con più parametri di tipo
public class Dictionary<TKey, TValue> {
    private List<TKey> keys;
    private List<TValue> values;
    
    public void Add(TKey key, TValue value) {
        keys.Add(key);
        values.Add(value);
    }
    
    public TValue Get(TKey key) {
        int index = keys.IndexOf(key);
        return values[index];
    }
}

// Utilizzo
Dictionary<string, int> eta = new Dictionary<string, int>();
eta.Add("Mario", 30);
eta.Add("Luigi", 25);
int etaMario = eta.Get("Mario");  // ✅ Type-safe

Dictionary<int, string> nomi = new Dictionary<int, string>();
nomi.Add(1, "Mario");
nomi.Add(2, "Luigi");
string nome = nomi.Get(1);  // ✅ Type-safe
```

---

## 3. Generic Methods (Metodi Generici)

### Sintassi

```csharp
public T NomeMetodo<T>(T parametro) {
    // T è il type parameter del metodo
    return parametro;
}
```

### Esempio: Metodo di Scambio

```csharp
public class Utils {
    // Metodo generic per scambiare due valori
    public static void Swap<T>(ref T a, ref T b) {
        T temp = a;
        a = b;
        b = temp;
    }
    
    // Metodo generic per trovare il massimo
    public static T Max<T>(T a, T b) where T : IComparable<T> {
        return a.CompareTo(b) > 0 ? a : b;
    }
}

// Utilizzo
int x = 10, y = 20;
Utils.Swap(ref x, ref y);  // ✅ x = 20, y = 10

string s1 = "Ciao", s2 = "Mondo";
Utils.Swap(ref s1, ref s2);  // ✅ s1 = "Mondo", s2 = "Ciao"

int max = Utils.Max(10, 20);  // ✅ 20
string maxStr = Utils.Max("A", "B");  // ✅ "B"
```

### Diagramma: Metodo Generic

```
┌─────────────────────────────────────────────┐
│  Utils.Swap<T>(ref T a, ref T b)          │
└─────────────────────────────────────────────┘
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│Swap<int>  │ │Swap<string>│ │Swap<Persona>│
│           │ │           │ │           │
│x, y: int │ │s1, s2:    │ │p1, p2:    │
│           │ │  string   │ │  Persona  │
└───────────┘ └───────────┘ └───────────┘
```

---

## 4. Constraints (Vincoli)

### Definizione

I **constraints** (vincoli) limitano i tipi che possono essere usati come parametri di tipo. Questo permette di usare operazioni specifiche sui tipi.

### Tipi di Constraints

```csharp
// where T : struct        - T deve essere un tipo valore
// where T : class         - T deve essere un tipo riferimento
// where T : new()         - T deve avere costruttore senza parametri
// where T : BaseClass     - T deve derivare da BaseClass
// where T : IInterface     - T deve implementare IInterface
```

### Esempio: Constraint con Interfaccia

```csharp
public interface IComparable<T> {
    int CompareTo(T other);
}

// Constraint: T deve implementare IComparable<T>
public class SortedList<T> where T : IComparable<T> {
    private List<T> items;
    
    public void Add(T item) {
        items.Add(item);
        items.Sort((x, y) => x.CompareTo(y));  // ✅ OK grazie al constraint
    }
    
    public T GetMax() {
        if (items.Count == 0) throw new InvalidOperationException();
        return items[items.Count - 1];
    }
}

// Utilizzo
SortedList<int> numeri = new SortedList<int>();  // ✅ int implementa IComparable<int>
numeri.Add(5);
numeri.Add(2);
numeri.Add(8);
int max = numeri.GetMax();  // ✅ 8

SortedList<string> stringhe = new SortedList<string>();  // ✅ string implementa IComparable<string>
stringhe.Add("Zebra");
stringhe.Add("Apple");
string maxStr = stringhe.GetMax();  // ✅ "Zebra"
```

### Esempio: Constraint con Classe Base

```csharp
public class Animal {
    public string Nome { get; set; }
    public virtual void EmettiSuono() {
        Console.WriteLine("Suono generico");
    }
}

// Constraint: T deve essere una sottoclasse di Animal
public class Zoo<T> where T : Animal {
    private List<T> animali;
    
    public void Aggiungi(T animale) {
        animali.Add(animale);
    }
    
    public void FaiRumore() {
        foreach (T animale in animali) {
            animale.EmettiSuono();  // ✅ OK grazie al constraint
        }
    }
}

public class Cane : Animal {
    public override void EmettiSuono() {
        Console.WriteLine("Bau!");
    }
}

// Utilizzo
Zoo<Cane> cani = new Zoo<Cane>();  // ✅ Cane deriva da Animal
cani.Aggiungi(new Cane { Nome = "Fido" });
cani.FaiRumore();  // ✅ "Bau!"

// Zoo<int> numeri = new Zoo<int>();  // ❌ ERRORE! int non deriva da Animal
```

### Esempio: Multiple Constraints

```csharp
// Multiple constraints
public class Repository<T> where T : class, new(), IEntity {
    public T Create() {
        return new T();  // ✅ new() constraint
    }
    
    public void Save(T entity) {
        entity.Id = GenerateId();  // ✅ IEntity constraint
    }
}

public interface IEntity {
    int Id { get; set; }
}

public class Persona : IEntity {
    public int Id { get; set; }
    public string Nome { get; set; }
}

// Utilizzo
Repository<Persona> repo = new Repository<Persona>();  // ✅ OK
// Repository<int> repo2 = new Repository<int>();  // ❌ ERRORE! int non è class
```

### Diagramma: Constraints

```
┌─────────────────────────────────────────────┐
│  SortedList<T> where T : IComparable<T>    │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  T deve implementare  │
        │  IComparable<T>        │
        └───────────────────────┘
                    │
        ┌───────────┴───────────┐
        │                       │
        ▼                       ▼
┌───────────┐          ┌───────────┐
│  int      │          │  string   │
│  ✅ OK    │          │  ✅ OK    │
│  (implementa│          │  (implementa│
│  IComparable)│          │  IComparable)│
└───────────┘          └───────────┘
```

---

## 5. Generic Interfaces (Interfacce Generiche)

### Sintassi

```csharp
public interface INomeInterfaccia<T> {
    T Metodo();
    void MetodoConParametro(T parametro);
}
```

### Esempio: Repository Pattern

```csharp
// Interfaccia generic
public interface IRepository<T> {
    T GetById(int id);
    void Save(T entity);
    void Delete(int id);
    List<T> GetAll();
}

// Implementazione per Persona
public class PersonaRepository : IRepository<Persona> {
    private List<Persona> persone = new List<Persona>();
    
    public Persona GetById(int id) {
        return persone.FirstOrDefault(p => p.Id == id);
    }
    
    public void Save(Persona persona) {
        var existing = persone.FirstOrDefault(p => p.Id == persona.Id);
        if (existing != null) {
            persone.Remove(existing);
        }
        persone.Add(persona);
    }
    
    public void Delete(int id) {
        persone.RemoveAll(p => p.Id == id);
    }
    
    public List<Persona> GetAll() {
        return persone;
    }
}

// Implementazione per Prodotto
public class ProdottoRepository : IRepository<Prodotto> {
    private List<Prodotto> prodotti = new List<Prodotto>();
    
    public Prodotto GetById(int id) {
        return prodotti.FirstOrDefault(p => p.Id == id);
    }
    
    public void Save(Prodotto prodotto) {
        var existing = prodotti.FirstOrDefault(p => p.Id == prodotto.Id);
        if (existing != null) {
            prodotti.Remove(existing);
        }
        prodotti.Add(prodotto);
    }
    
    public void Delete(int id) {
        prodotti.RemoveAll(p => p.Id == id);
    }
    
    public List<Prodotto> GetAll() {
        return prodotti;
    }
}

// Utilizzo
IRepository<Persona> personaRepo = new PersonaRepository();
personaRepo.Save(new Persona { Id = 1, Nome = "Mario" });

IRepository<Prodotto> prodottoRepo = new ProdottoRepository();
prodottoRepo.Save(new Prodotto { Id = 1, Nome = "Laptop" });
```

### Diagramma: Interfaccia Generic

```
┌─────────────────────────────────────────────┐
│  IRepository<T>                             │
│  + GetById(int) : T                         │
│  + Save(T)                                   │
│  + Delete(int)                              │
│  + GetAll() : List<T>                       │
└─────────────────────────────────────────────┘
                    ▲
        ┌───────────┼───────────┐
        │           │           │
        │           │           │
┌───────────────┐ ┌───────────────┐
│PersonaRepository│ │ProdottoRepository│
│: IRepository<   │ │: IRepository<    │
│  Persona>      │ │  Prodotto>       │
└───────────────┘ └───────────────┘
```

---

## 6. Covarianza e Controvarianza

### Covarianza (Covariance)

La **covarianza** permette di usare un tipo più derivato al posto di un tipo base.

```csharp
// Covarianza con IEnumerable<T>
IEnumerable<string> stringhe = new List<string> { "A", "B", "C" };
IEnumerable<object> oggetti = stringhe;  // ✅ Covarianza

// Covarianza con IReadOnlyList<T>
IReadOnlyList<string> stringList = new List<string>();
IReadOnlyList<object> objectList = stringList;  // ✅ Covarianza
```

### Controvarianza (Contravariance)

La **controvarianza** permette di usare un tipo più base al posto di un tipo derivato.

```csharp
// Controvarianza con Action<T>
Action<object> actionObject = (obj) => Console.WriteLine(obj);
Action<string> actionString = actionObject;  // ✅ Controvarianza

// Utilizzo
actionString("Ciao");  // ✅ OK
```

### Diagramma: Varianza

```
┌─────────────────────────────────────────────┐
│  COVARIANZA (più specifico → più generale) │
│                                             │
│  IEnumerable<string>                        │
│        ↓                                    │
│  IEnumerable<object>                        │
│                                             │
│  ✅ OK - Leggendo, string è object         │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  CONTROVARIANZA (più generale → più specifico)│
│                                             │
│  Action<object>                            │
│        ↓                                    │
│  Action<string>                            │
│                                             │
│  ✅ OK - Scrivendo, object contiene string│
└─────────────────────────────────────────────┘
```

---

## 7. Esempi Pratici Completi

### Esempio 1: Classe Generic per Pair

```csharp
public class Pair<TFirst, TSecond> {
    public TFirst First { get; set; }
    public TSecond Second { get; set; }
    
    public Pair(TFirst first, TSecond second) {
        First = first;
        Second = second;
    }
    
    public override string ToString() {
        return $"({First}, {Second})";
    }
}

// Utilizzo
Pair<string, int> nomeEta = new Pair<string, int>("Mario", 30);
Pair<int, int> coordinate = new Pair<int, int>(10, 20);
Pair<string, string> nomeCognome = new Pair<string, string>("Mario", "Rossi");
```

### Esempio 2: Classe Generic per Binary Tree

```csharp
public class BinaryTree<T> where T : IComparable<T> {
    private class Node {
        public T Value { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
    }
    
    private Node root;
    
    public void Insert(T value) {
        root = InsertRecursive(root, value);
    }
    
    private Node InsertRecursive(Node node, T value) {
        if (node == null) {
            return new Node { Value = value };
        }
        
        if (value.CompareTo(node.Value) < 0) {
            node.Left = InsertRecursive(node.Left, value);
        } else if (value.CompareTo(node.Value) > 0) {
            node.Right = InsertRecursive(node.Right, value);
        }
        
        return node;
    }
    
    public bool Contains(T value) {
        return ContainsRecursive(root, value);
    }
    
    private bool ContainsRecursive(Node node, T value) {
        if (node == null) return false;
        
        int comparison = value.CompareTo(node.Value);
        if (comparison == 0) return true;
        
        return comparison < 0 
            ? ContainsRecursive(node.Left, value)
            : ContainsRecursive(node.Right, value);
    }
}

// Utilizzo
BinaryTree<int> albero = new BinaryTree<int>();
albero.Insert(5);
albero.Insert(3);
albero.Insert(7);
bool contiene = albero.Contains(5);  // ✅ true
```

### Esempio 3: Factory Pattern con Generics

```csharp
public interface IFactory<T> {
    T Create();
}

public class PersonaFactory : IFactory<Persona> {
    public Persona Create() {
        return new Persona {
            Nome = "Nuovo",
            Eta = 0
        };
    }
}

public class GenericFactory<T> where T : new() {
    public T Create() {
        return new T();  // ✅ new() constraint
    }
}

// Utilizzo
var factory = new GenericFactory<Persona>();
Persona p = factory.Create();  // ✅ OK se Persona ha costruttore senza parametri
```

---

## 8. Vantaggi dei Generics

### Tabella Comparativa

| Aspetto | Senza Generics | Con Generics |
|---------|----------------|--------------|
| **Type Safety** | ❌ Runtime errors | ✅ Compile-time checking |
| **Performance** | ⚠️ Boxing/Unboxing | ✅ Nessun boxing |
| **Code Reuse** | ❌ Duplicazione codice | ✅ Codice riutilizzabile |
| **IntelliSense** | ⚠️ Limitato | ✅ Completo |
| **Leggibilità** | ⚠️ Cast necessari | ✅ Codice chiaro |

### Diagramma: Vantaggi

```
┌─────────────────────────────────────────────┐
│         VANTAGGI DEI GENERICS               │
├─────────────────────────────────────────────┤
│  ✅ Type Safety                              │
│     - Errori a compile-time                 │
│     - Non errori a runtime                   │
├─────────────────────────────────────────────┤
│  ✅ Performance                              │
│     - Nessun boxing/unboxing                │
│     - Codice più efficiente                 │
├─────────────────────────────────────────────┤
│  ✅ Code Reuse                               │
│     - Una classe per tutti i tipi           │
│     - Meno duplicazione                     │
├─────────────────────────────────────────────┤
│  ✅ IntelliSense                             │
│     - Supporto completo IDE                 │
│     - Autocompletamento                     │
└─────────────────────────────────────────────┘
```

---

## 9. Best Practices

### ✅ Cosa Fare

1. **Usa nomi descrittivi per i parametri di tipo**
   ```csharp
   // ✅ CORRETTO
   public class Dictionary<TKey, TValue> { }
   
   // ❌ SBAGLIATO
   public class Dictionary<T, U> { }
   ```

2. **Usa constraints quando necessario**
   ```csharp
   // ✅ CORRETTO
   public class SortedList<T> where T : IComparable<T> { }
   ```

3. **Considera le interfacce generiche per maggiore flessibilità**
   ```csharp
   // ✅ CORRETTO
   IList<T> lista = new List<T>();
   ```

### ❌ Cosa Evitare

1. **Non usare generics quando non servono**
   ```csharp
   // ❌ OVER-ENGINEERING
   public class Calculator<T> where T : int { }
   ```

2. **Non usare troppi parametri di tipo**
   ```csharp
   // ⚠️ TROPPO COMPLESSO
   public class SuperGeneric<T1, T2, T3, T4, T5> { }
   ```

---

## 10. Domande Frequenti (FAQ)

### Q: Qual è la differenza tra generics e object?
**R:** I generics sono type-safe a compile-time e non richiedono boxing/unboxing. Object è type-safe solo a runtime e richiede casting.

### Q: Posso usare generics con tipi nullable?
**R:** Sì, puoi usare `T?` quando T è un tipo valore, o `T` direttamente quando T è un tipo riferimento.

### Q: I generics sono più lenti di classi non-generic?
**R:** No, anzi sono più veloci perché evitano boxing/unboxing e casting.

### Q: Posso avere metodi generici in classi non-generic?
**R:** Sì, i metodi possono essere generici anche se la classe non lo è.

---

## Conclusioni

I generics in C# sono fondamentali per:

- ✅ Scrivere codice type-safe
- ✅ Migliorare le performance
- ✅ Ridurre la duplicazione del codice
- ✅ Creare API più flessibili e riutilizzabili

I generics sono utilizzati ampiamente in .NET Framework (List<T>, Dictionary<TKey, TValue>, ecc.) e sono essenziali per scrivere codice C# moderno e professionale.

---

*Documento creato per spiegare i Generics in C# con esempi pratici e diagrammi.*

