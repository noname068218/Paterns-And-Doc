# Costruttori in C#: Come Funzionano

## Introduzione

I **costruttori** sono metodi speciali in C# che vengono chiamati automaticamente quando viene creato un'istanza di una classe. Sono fondamentali per inizializzare gli oggetti e prepararli per l'uso.

---

## 1. Cos'è un Costruttore?

### Definizione

Un costruttore è un metodo speciale che:

- Ha lo **stesso nome** della classe
- **Non ha tipo di ritorno** (neanche `void`)
- Viene chiamato **automaticamente** quando si usa `new`
- Serve per **inizializzare** l'oggetto

### Sintassi Base

```csharp
public class Persona {
    public string Nome;
    public int Eta;

    // COSTRUTTORE
    public Persona() {
        Nome = "Sconosciuto";
        Eta = 0;
    }
}
```

### Diagramma: Chiamata del Costruttore

```
┌─────────────────────────────────────────┐
│  Persona persona = new Persona();      │
└─────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  new Persona()       │
        │  ↓                    │
        │  Alloca memoria       │
        │  nello HEAP           │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Costruttore Persona()│ ← Chiamato automaticamente
        │  {                     │
        │    Nome = "Sconosciuto"│
        │    Eta = 0;            │
        │  }                     │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Oggetto inizializzato│
        │  pronto per l'uso     │
        └───────────────────────┘
```

---

## 2. Tipi di Costruttori

### 2.1 Costruttore Predefinito (Default Constructor)

Se **non definisci** un costruttore, C# ne crea uno automaticamente (senza parametri).

```csharp
public class Auto {
    public string Marca;
    // Nessun costruttore definito
}

// C# crea automaticamente:
// public Auto() { }
```

### Diagramma: Costruttore Predefinito

```
┌──────────────────────────────────────┐
│  Classe senza costruttore            │
│  class Auto { }                      │
└──────────────────────────────────────┘
              │
              ▼
┌──────────────────────────────────────┐
│  C# crea automaticamente:            │
│  public Auto() { }                   │
└──────────────────────────────────────┘
              │
              ▼
┌──────────────────────────────────────┐
│  new Auto()  →  Costruttore vuoto    │
│                (tutti i campi = null) │
└──────────────────────────────────────┘
```

### 2.2 Costruttore con Parametri

Permette di passare valori durante la creazione dell'oggetto.

```csharp
public class Persona {
    public string Nome;
    public int Eta;

    // Costruttore con parametri
    public Persona(string nome, int eta) {
        Nome = nome;
        Eta = eta;
    }
}

// Utilizzo
Persona p = new Persona("Mario", 30);
```

### Diagramma: Costruttore con Parametri

```
┌─────────────────────────────────────────────┐
│  Persona p = new Persona("Mario", 30);     │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────────┐
        │  Parametri passati:       │
        │  nome = "Mario"           │
        │  eta = 30                 │
        └───────────────────────────┘
                    │
                    ▼
        ┌───────────────────────────┐
        │  Costruttore eseguito:    │
        │  Persona(string nome,     │
        │          int eta)         │
        │  {                         │
        │    Nome = nome;  ← "Mario"│
        │    Eta = eta;    ← 30     │
        │  }                         │
        └───────────────────────────┘
                    │
                    ▼
        ┌───────────────────────────┐
        │  Oggetto creato:          │
        │  Nome = "Mario"           │
        │  Eta = 30                 │
        └───────────────────────────┘
```

### 2.3 Costruttore Vuoto (Parameterless)

Costruttore senza parametri definito esplicitamente.

```csharp
public class Persona {
    public string Nome;
    public int Eta;

    // Costruttore vuoto esplicito
    public Persona() {
        Nome = "Anonimo";
        Eta = 0;
    }
}
```

---

## 3. Overloading dei Costruttori

Puoi definire **più costruttori** nella stessa classe con parametri diversi.

```csharp
public class Persona {
    public string Nome;
    public int Eta;
    public string Citta;

    // Costruttore 1: Nessun parametro
    public Persona() {
        Nome = "Anonimo";
        Eta = 0;
        Citta = "Sconosciuta";
    }

    // Costruttore 2: Solo nome
    public Persona(string nome) {
        Nome = nome;
        Eta = 0;
        Citta = "Sconosciuta";
    }

    // Costruttore 3: Nome e età
    public Persona(string nome, int eta) {
        Nome = nome;
        Eta = eta;
        Citta = "Sconosciuta";
    }

    // Costruttore 4: Tutti i parametri
    public Persona(string nome, int eta, string citta) {
        Nome = nome;
        Eta = eta;
        Citta = citta;
    }
}

// Utilizzo
Persona p1 = new Persona();                    // Costruttore 1
Persona p2 = new Persona("Mario");             // Costruttore 2
Persona p3 = new Persona("Mario", 30);         // Costruttore 3
Persona p4 = new Persona("Mario", 30, "Roma"); // Costruttore 4
```

### Diagramma: Selezione del Costruttore

```
┌─────────────────────────────────────────────┐
│  new Persona(...)                           │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────────┐
        │  Analisi parametri        │
        └───────────────────────────┘
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
    ┌───────┐  ┌───────┐  ┌───────┐
    │ 0 par │  │ 1 par │  │ 2 par │
    └───────┘  └───────┘  └───────┘
        │           │           │
        ▼           ▼           ▼
    Persona()  Persona(  Persona(
               string)   string, int)

    Compilatore sceglie il costruttore
    in base al numero e tipo di parametri
```

---

## 4. Costruttori a Catena (Constructor Chaining)

I costruttori possono chiamarsi a vicenda usando `this()`.

```csharp
public class Persona {
    public string Nome;
    public int Eta;
    public string Citta;

    // Costruttore principale
    public Persona(string nome, int eta, string citta) {
        Nome = nome;
        Eta = eta;
        Citta = citta;
    }

    // Chiama il costruttore principale
    public Persona(string nome, int eta)
        : this(nome, eta, "Sconosciuta") {
        // Codice aggiuntivo opzionale
    }

    // Chiama il costruttore sopra
    public Persona(string nome)
        : this(nome, 0) {
        // Codice aggiuntivo opzionale
    }

    // Chiama il costruttore sopra
    public Persona()
        : this("Anonimo") {
        // Codice aggiuntivo opzionale
    }
}
```

### Diagramma: Chaining dei Costruttori

```
┌─────────────────────────────────────────────┐
│  new Persona()                              │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────────┐
        │  Persona()                │
        │  : this("Anonimo")        │ ← Chiama
        └───────────────────────────┘    │
                    │                    │
                    ▼                    │
        ┌───────────────────────────┐    │
        │  Persona(string nome)     │ ←──┘
        │  : this(nome, 0)          │ ← Chiama
        └───────────────────────────┘    │
                    │                    │
                    ▼                    │
        ┌───────────────────────────┐    │
        │  Persona(string, int)     │ ←──┘
        │  : this(nome, eta, "Sconosciuta")│ ← Chiama
        └───────────────────────────┘    │
                    │                    │
                    ▼                    │
        ┌───────────────────────────┐    │
        │  Persona(string, int,     │ ←──┘
        │           string)          │
        │  {                         │
        │    // Inizializzazione     │
        │  }                         │
        └───────────────────────────┘
```

### Ordine di Esecuzione

```
Chiamata: new Persona()
    │
    ├─> Persona() esegue
    │   └─> Chiama this("Anonimo")
    │       │
    │       ├─> Persona(string) esegue
    │       │   └─> Chiama this("Anonimo", 0)
    │       │       │
    │       │       ├─> Persona(string, int) esegue
    │       │       │   └─> Chiama this("Anonimo", 0, "Sconosciuta")
    │       │       │       │
    │       │       │       └─> Persona(string, int, string) esegue
    │       │       │           └─> Inizializza tutti i campi
    │       │       │
    │       │       └─> Ritorna (body eseguito)
    │       │
    │       └─> Ritorna (body eseguito)
    │
    └─> Ritorna (body eseguito)
```

---

## 5. Costruttori Statici

Un costruttore **statico** viene chiamato una sola volta, prima del primo utilizzo della classe.

```csharp
public class Configurazione {
    public static string Versione;

    // Costruttore statico
    static Configurazione() {
        Versione = "1.0.0";
        Console.WriteLine("Configurazione inizializzata");
    }
}
```

### Caratteristiche

- ✅ Viene chiamato **automaticamente** prima del primo accesso
- ✅ Viene eseguito **una sola volta**
- ✅ **Non può avere parametri**
- ✅ **Non può avere modificatori di accesso**

### Diagramma: Costruttore Statico

```
┌─────────────────────────────────────────────┐
│  Prima volta che si accede alla classe     │
│  Configurazione.Versione = "1.0.0";        │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────────┐
        │  Costruttore statico      │
        │  static Configurazione()  │
        │  {                         │
        │    Versione = "1.0.0";    │
        │  }                         │
        └───────────────────────────┘
                    │
                    ▼
        ┌───────────────────────────┐
        │  Classe inizializzata     │
        │  (una sola volta)         │
        └───────────────────────────┘
                    │
                    ▼
        ┌───────────────────────────┐
        │  Accessi successivi       │
        │  NON chiamano il          │
        │  costruttore statico      │
        └───────────────────────────┘
```

---

## 6. Costruttori di Copia

Un costruttore che crea una copia di un oggetto esistente.

```csharp
public class Persona {
    public string Nome;
    public int Eta;

    // Costruttore normale
    public Persona(string nome, int eta) {
        Nome = nome;
        Eta = eta;
    }

    // Costruttore di copia
    public Persona(Persona originale) {
        Nome = originale.Nome;
        Eta = originale.Eta;
    }
}

// Utilizzo
Persona p1 = new Persona("Mario", 30);
Persona p2 = new Persona(p1);  // Copia di p1
```

### Diagramma: Costruttore di Copia

```
┌─────────────────────────────────────────────┐
│  Persona p1 = new Persona("Mario", 30);    │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────────┐
        │  HEAP: Oggetto p1         │
        │  Nome = "Mario"           │
        │  Eta = 30                 │
        └───────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────┐
│  Persona p2 = new Persona(p1);             │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────────┐
        │  Costruttore di copia     │
        │  Persona(Persona orig)    │
        │  {                         │
        │    Nome = orig.Nome;      │
        │    Eta = orig.Eta;        │
        │  }                         │
        └───────────────────────────┘
                    │
                    ▼
        ┌───────────────────────────┐
        │  HEAP: Nuovo oggetto p2   │
        │  Nome = "Mario"           │
        │  Eta = 30                 │
        │                           │
        │  (Oggetto separato,       │
        │   non condiviso)          │
        └───────────────────────────┘
```

---

## 7. Costruttori Privati

I costruttori privati impediscono la creazione di istanze della classe dall'esterno.

```csharp
public class Singleton {
    private static Singleton istanza;

    // Costruttore privato
    private Singleton() {
        // Inizializzazione
    }

    // Metodo statico per ottenere l'istanza
    public static Singleton GetIstanza() {
        if (istanza == null) {
            istanza = new Singleton();
        }
        return istanza;
    }
}

// Utilizzo
// Singleton s = new Singleton();  // ❌ ERRORE! Non consentito
Singleton s = Singleton.GetIstanza();  // ✅ OK
```

### Pattern Singleton - Diagramma

```
┌─────────────────────────────────────────────┐
│  Singleton.GetIstanza()                     │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────────┐
        │  istanza è null?           │
        └───────────────────────────┘
                    │
        ┌───────────┴───────────┐
        │                       │
      SÌ                       NO
        │                       │
        ▼                       ▼
┌───────────────┐      ┌───────────────┐
│  new Singleton() │      │  Ritorna      │
│  (costruttore    │      │  istanza      │
│   privato)       │      │  esistente    │
└───────────────┘      └───────────────┘
        │
        ▼
┌───────────────┐
│  istanza =    │
│  nuovo oggetto│
└───────────────┘
```

---

## 8. Inizializzatori di Campo

I campi possono essere inizializzati direttamente nella dichiarazione.

```csharp
public class Persona {
    public string Nome = "Anonimo";  // Inizializzatore
    public int Eta = 0;              // Inizializzatore

    public Persona() {
        // Nome e Eta già inizializzati
    }

    public Persona(string nome) {
        Nome = nome;  // Sovrascrive l'inizializzatore
    }
}
```

### Ordine di Esecuzione

```
┌─────────────────────────────────────────────┐
│  new Persona("Mario")                       │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────────┐
        │  1. Inizializzatori campo │
        │     Nome = "Anonimo"      │
        │     Eta = 0               │
        └───────────────────────────┘
                    │
                    ▼
        ┌───────────────────────────┐
        │  2. Costruttore base      │
        │     (se presente)         │
        └───────────────────────────┘
                    │
                    ▼
        ┌───────────────────────────┐
        │  3. Corpo costruttore    │
        │     Nome = "Mario"        │
        │     (sovrascrive)         │
        └───────────────────────────┘
```

---

## 9. Ereditarietà e Costruttori

### Chiamata al Costruttore Base

Quando una classe deriva da un'altra, il costruttore della classe base viene chiamato automaticamente.

```csharp
public class Animale {
    public string Nome;

    public Animale(string nome) {
        Nome = nome;
        Console.WriteLine("Costruttore Animale");
    }
}

public class Cane : Animale {
    public string Razza;

    // Costruttore che chiama il base
    public Cane(string nome, string razza)
        : base(nome) {  // Chiama Animale(nome)
        Razza = razza;
        Console.WriteLine("Costruttore Cane");
    }
}

// Utilizzo
Cane cane = new Cane("Fido", "Labrador");
// Output:
// Costruttore Animale
// Costruttore Cane
```

### Diagramma: Ereditarietà e Costruttori

```
┌─────────────────────────────────────────────┐
│  new Cane("Fido", "Labrador")              │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────────┐
        │  Cane(string, string)     │
        │  : base(nome)             │ ← Chiama
        └───────────────────────────┘    │
                    │                    │
                    ▼                    │
        ┌───────────────────────────┐    │
        │  Animale(string nome)     │ ←──┘
        │  {                         │
        │    Nome = nome;            │
        │    // Esegue per primo     │
        │  }                         │
        └───────────────────────────┘
                    │
                    ▼
        ┌───────────────────────────┐
        │  Ritorna al costruttore  │
        │  Cane                     │
        └───────────────────────────┘
                    │
                    ▼
        ┌───────────────────────────┐
        │  Cane(string, string)     │
        │  {                         │
        │    Razza = razza;         │
        │    // Esegue dopo         │
        │  }                         │
        └───────────────────────────┘
```

### Ordine di Esecuzione nell'Ereditarietà

```
┌─────────────────────────────────────────────┐
│  new Cane("Fido", "Labrador")              │
└─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────┐
│  ORDINE DI ESECUZIONE:                      │
├─────────────────────────────────────────────┤
│  1. Inizializzatori campo (Cane)           │
│  2. Inizializzatori campo (Animale)        │
│  3. Costruttore Animale                     │
│  4. Costruttore Cane                        │
└─────────────────────────────────────────────┘
```

---

## 10. Costruttori e Proprietà

I costruttori possono inizializzare proprietà automatiche.

```csharp
public class Persona {
    public string Nome { get; set; }
    public int Eta { get; set; }

    // Costruttore con inizializzazione
    public Persona(string nome, int eta) {
        Nome = nome;
        Eta = eta;
    }

    // Oppure con inizializzatore
    public Persona(string nome) {
        Nome = nome;
    }

    public int Eta { get; set; } = 0;  // Valore predefinito
}
```

---

## 11. Record e Costruttori (C# 9+)

I record hanno costruttori primari incorporati.

```csharp
// Record con costruttore primario
public record Persona(string Nome, int Eta);

// Equivale a:
public record Persona {
    public string Nome { get; init; }
    public int Eta { get; init; }

    public Persona(string nome, int eta) {
        Nome = nome;
        Eta = eta;
    }
}

// Utilizzo
Persona p = new Persona("Mario", 30);
```

### Diagramma: Record con Costruttore Primario

```
┌─────────────────────────────────────────────┐
│  public record Persona(string Nome, int Eta)│
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────────┐
        │  C# genera automaticamente:│
        │  - Proprietà Nome          │
        │  - Proprietà Eta           │
        │  - Costruttore Persona(    │
        │      string Nome,          │
        │      int Eta)              │
        └───────────────────────────┘
```

---

## 12. Esempi Pratici Completi

### Esempio 1: Classe con Multiple Costruttori

```csharp
public class ContoBancario {
    private string numeroConto;
    private decimal saldo;
    private string proprietario;

    // Costruttore principale
    public ContoBancario(string numeroConto, string proprietario, decimal saldoIniziale) {
        this.numeroConto = numeroConto;
        this.proprietario = proprietario;
        this.saldo = saldoIniziale;
    }

    // Costruttore con saldo zero
    public ContoBancario(string numeroConto, string proprietario)
        : this(numeroConto, proprietario, 0) {
    }

    // Costruttore di copia
    public ContoBancario(ContoBancario altro)
        : this(altro.numeroConto, altro.proprietario, altro.saldo) {
    }

    public void StampaInfo() {
        Console.WriteLine($"Conto: {numeroConto}, Proprietario: {proprietario}, Saldo: {saldo}");
    }
}

// Utilizzo
ContoBancario conto1 = new ContoBancario("IT123", "Mario", 1000);
ContoBancario conto2 = new ContoBancario("IT456", "Luigi");  // Saldo = 0
ContoBancario conto3 = new ContoBancario(conto1);  // Copia
```

### Esempio 2: Classe con Costruttore Statico

```csharp
public class Database {
    private static string connectionString;
    private static bool inizializzato = false;

    // Costruttore statico
    static Database() {
        connectionString = "Server=localhost;Database=Test";
        inizializzato = true;
        Console.WriteLine("Database inizializzato");
    }

    public static string GetConnectionString() {
        return connectionString;
    }
}

// Utilizzo
string conn = Database.GetConnectionString();  // Chiama il costruttore statico la prima volta
```

---

## 13. Flusso Completo: Creazione di un Oggetto

```
┌─────────────────────────────────────────────────────────────┐
│  Persona p = new Persona("Mario", 30);                     │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
        ┌───────────────────────────────────────┐
        │  FASE 1: Allocazione Memoria         │
        │  - Alloca spazio nello HEAP           │
        │  - Tutti i campi = null/0             │
        └───────────────────────────────────────┘
                            │
                            ▼
        ┌───────────────────────────────────────┐
        │  FASE 2: Inizializzatori Campo        │
        │  - Esegue inizializzatori campo       │
        │  - Se presenti                        │
        └───────────────────────────────────────┘
                            │
                            ▼
        ┌───────────────────────────────────────┐
        │  FASE 3: Costruttore Base             │
        │  - Se classe deriva da altra           │
        │  - Chiama base() o this()              │
        └───────────────────────────────────────┘
                            │
                            ▼
        ┌───────────────────────────────────────┐
        │  FASE 4: Corpo Costruttore            │
        │  - Esegue il codice del costruttore   │
        │  - Inizializza campi con parametri     │
        └───────────────────────────────────────┘
                            │
                            ▼
        ┌───────────────────────────────────────┐
        │  FASE 5: Oggetto Pronto               │
        │  - Riferimento assegnato a p          │
        │  - Oggetto utilizzabile               │
        └───────────────────────────────────────┘
```

---

## 14. Best Practices

### ✅ Cosa Fare

1. **Inizializza sempre tutti i campi**

   ```csharp
   public class Persona {
       public string Nome { get; set; }

       public Persona(string nome) {
           Nome = nome ?? "Anonimo";  // Gestione null
       }
   }
   ```

2. **Usa costruttori a catena per evitare duplicazione**

   ```csharp
   public Persona(string nome, int eta) {
       Nome = nome;
       Eta = eta;
   }

   public Persona(string nome) : this(nome, 0) { }
   ```

3. **Valida i parametri nel costruttore**

   ```csharp
   public Persona(string nome, int eta) {
       if (string.IsNullOrEmpty(nome))
           throw new ArgumentException("Nome non può essere vuoto");
       if (eta < 0)
           throw new ArgumentException("Età non può essere negativa");

       Nome = nome;
       Eta = eta;
   }
   ```

### ❌ Cosa Evitare

1. **Non fare lavoro pesante nel costruttore**

   ```csharp
   // ❌ SBAGLIATO
   public Persona() {
       // Operazioni pesanti (database, file, rete)
       CaricaDaDatabase();  // Troppo lento!
   }
   ```

2. **Non chiamare metodi virtuali nel costruttore**

   ```csharp
   // ⚠️ PERICOLOSO
   public class Base {
       public Base() {
           MetodoVirtuale();  // Può chiamare override non ancora inizializzato
       }

       public virtual void MetodoVirtuale() { }
   }
   ```

3. **Non creare dipendenze circolari**

   ```csharp
   // ❌ SBAGLIATO
   public class A {
       public A(B b) { }
   }

   public class B {
       public B(A a) { }  // Dipendenza circolare!
   }
   ```

---

## 15. Riepilogo e Tabella Comparativa

### Tabella: Tipi di Costruttori

| Tipo              | Sintassi                    | Quando Usare                 | Esempio                    |
| ----------------- | --------------------------- | ---------------------------- | -------------------------- |
| **Predefinito**   | `public Class() { }`        | Automatico se non definito   | C# lo crea automaticamente |
| **Con parametri** | `public Class(int x) { }`   | Inizializzazione con valori  | `new Persona("Mario", 30)` |
| **Vuoto**         | `public Class() { }`        | Inizializzazione predefinita | `new Persona()`            |
| **Statico**       | `static Class() { }`        | Inizializzazione classe      | Prima accesso alla classe  |
| **Di copia**      | `public Class(Class c) { }` | Copia oggetto esistente      | `new Persona(p1)`          |
| **Privato**       | `private Class() { }`       | Pattern Singleton            | `Singleton.GetIstanza()`   |

### Ordine di Esecuzione (Riepilogo)

```
1. Allocazione memoria (HEAP)
2. Inizializzatori campo (classe derivata)
3. Inizializzatori campo (classe base)
4. Costruttore classe base
5. Corpo costruttore classe derivata
```

---

## 16. Domande Frequenti (FAQ)

### Q: Un costruttore può avere un tipo di ritorno?

**R:** No, mai. Un costruttore non ha tipo di ritorno (neanche `void`).

### Q: Posso chiamare un costruttore manualmente?

**R:** No, i costruttori vengono chiamati solo quando si usa `new` o `base()`/`this()`.

### Q: Quanti costruttori posso avere?

**R:** Quanti vuoi, purché abbiano firme diverse (overloading).

### Q: Cosa succede se non definisco un costruttore?

**R:** C# crea automaticamente un costruttore pubblico senza parametri.

### Q: Un costruttore può essere asincrono?

**R:** No, i costruttori non possono essere `async`.

---

## Conclusioni

I costruttori sono fondamentali in C# per:

- ✅ Inizializzare oggetti correttamente
- ✅ Validare i dati in ingresso
- ✅ Gestire la memoria in modo efficiente
- ✅ Implementare pattern di design (Singleton, Factory, ecc.)

Comprendere come funzionano i costruttori è essenziale per scrivere codice C# robusto e ben strutturato.

---

_Documento creato per spiegare i costruttori in C# con esempi pratici e diagrammi._
