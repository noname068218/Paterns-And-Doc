# Principi della Programmazione Orientata agli Oggetti (OOP) in C#

## Introduzione

La **Programmazione Orientata agli Oggetti (OOP)** è un paradigma di programmazione basato sul concetto di "oggetti" che contengono dati (attributi) e codice (metodi). In C#, tutti i tipi sono ereditati da `object`, rendendo il linguaggio completamente orientato agli oggetti.

---

## 1. I Quattro Pilastri dell'OOP

La programmazione orientata agli oggetti si basa su quattro principi fondamentali:

### Diagramma: I Quattro Pilastri

```
┌─────────────────────────────────────────────────┐
│          PROGRAMMAZIONE OOP                    │
└─────────────────────────────────────────────────┘
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐ ┌───────────┐
│ INCAPSULA-│ │ EREDITA-  │ │ POLIMOR-  │ │ ASTRA-    │
│ MENTO     │ │ RITÀ      │ │ FISMO     │ │ ZIONE     │
└───────────┘ └───────────┘ └───────────┘ └───────────┘
```

---

## 2. Incapsulamento (Encapsulation)

### Definizione

L'**incapsulamento** è il meccanismo che nasconde i dettagli di implementazione di una classe e espone solo ciò che è necessario attraverso un'interfaccia pubblica.

### Caratteristiche

- ✅ Nasconde i dettagli interni
- ✅ Protegge i dati dall'accesso non autorizzato
- ✅ Fornisce controllo sull'accesso ai dati
- ✅ Migliora la manutenibilità del codice

### Modificatori di Accesso in C#

```csharp
public class ContoBancario {
    // Pubblico - accessibile ovunque
    public string NumeroConto { get; set; }

    // Privato - accessibile solo nella classe
    private decimal saldo;

    // Protetto - accessibile nella classe e nelle classi derivate
    protected string proprietario;

    // Interno - accessibile nello stesso assembly
    internal string filiale;

    // Protetto Interno - protected + internal
    protected internal DateTime dataCreazione;

    // Metodo pubblico per accedere al saldo privato
    public decimal GetSaldo() {
        return saldo;
    }

    // Metodo pubblico per modificare il saldo con controllo
    public void Deposita(decimal importo) {
        if (importo > 0) {
            saldo += importo;
        }
    }
}
```

### Diagramma: Incapsulamento

```
┌─────────────────────────────────────────────┐
│           CLASSE ContoBancario              │
├─────────────────────────────────────────────┤
│  ┌───────────────────────────────────────┐ │
│  │  INTERFACCIA PUBBLICA (Pubblico)      │ │
│  │  - NumeroConto                         │ │
│  │  - GetSaldo()                         │ │
│  │  - Deposita()                         │ │
│  └───────────────────────────────────────┘ │
│                    │                        │
│                    ▼                        │
│  ┌───────────────────────────────────────┐ │
│  │  IMPLEMENTAZIONE PRIVATA (Nascosta)   │ │
│  │  - saldo (privato)                    │ │
│  │  - Validazione                        │ │
│  │  - Logica interna                     │ │
│  └───────────────────────────────────────┘ │
└─────────────────────────────────────────────┘
```

### Esempio Completo: Incapsulamento

```csharp
public class Persona {
    // Campi privati - incapsulati
    private string nome;
    private int eta;
    private string email;

    // Proprietà pubbliche - interfaccia controllata
    public string Nome {
        get { return nome; }
        set {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Il nome non può essere vuoto");
            nome = value;
        }
    }

    public int Eta {
        get { return eta; }
        set {
            if (value < 0 || value > 150)
                throw new ArgumentException("Età non valida");
            eta = value;
        }
    }

    // Solo get - email non può essere modificata dall'esterno
    public string Email {
        get { return email; }
        private set { email = value; }
    }

    // Metodo pubblico per modificare l'email con validazione
    public void CambiaEmail(string nuovaEmail) {
        if (IsEmailValida(nuovaEmail)) {
            email = nuovaEmail;
        } else {
            throw new ArgumentException("Email non valida");
        }
    }

    // Metodo privato - solo per uso interno
    private bool IsEmailValida(string email) {
        return email.Contains("@") && email.Contains(".");
    }
}
```

### Diagramma: Accesso ai Dati

```
┌─────────────────────────────────────────────┐
│  Codice Esterno                              │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Accesso Pubblico    │
        │  - Nome              │
        │  - Eta               │
        │  - CambiaEmail()     │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Controllo e           │
        │  Validazione           │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Dati Privati         │
        │  - nome               │
        │  - eta                │
        │  - email              │
        └───────────────────────┘
```

---

## 3. Ereditarietà (Inheritance)

### Definizione

L'**ereditarietà** permette a una classe di ereditare caratteristiche (campi e metodi) da un'altra classe, creando una relazione "è-un" (is-a).

### Sintassi Base

```csharp
// Classe base (superclasse)
public class Animale {
    public string Nome { get; set; }
    public int Eta { get; set; }

    public virtual void EmettiSuono() {
        Console.WriteLine("L'animale emette un suono");
    }

    public void Dormi() {
        Console.WriteLine($"{Nome} sta dormendo");
    }
}

// Classe derivata (sottoclasse)
public class Cane : Animale {
    public string Razza { get; set; }

    // Override del metodo virtuale
    public override void EmettiSuono() {
        Console.WriteLine("Bau! Bau!");
    }

    // Metodo aggiuntivo specifico del Cane
    public void Abbaia() {
        Console.WriteLine("Il cane abbaia");
    }
}

// Classe derivata
public class Gatto : Animale {
    public override void EmettiSuono() {
        Console.WriteLine("Miao! Miao!");
    }

    public void FaiLeFusa() {
        Console.WriteLine("Il gatto fa le fusa");
    }
}
```

### Diagramma: Ereditarietà

```
┌─────────────────────────────────────┐
│         Animale (Base)              │
│  - Nome                             │
│  - Eta                              │
│  + EmettiSuono()                    │
│  + Dormi()                          │
└─────────────────────────────────────┘
            │
    ┌───────┴───────┐
    │               │
    ▼               ▼
┌─────────┐    ┌─────────┐
│  Cane   │    │  Gatto  │
│  - Razza│    │         │
│  + Abbaia()   │  + FaiLeFusa()│
└─────────┘    └─────────┘

Ereditano: Nome, Eta, Dormi()
Override: EmettiSuono()
```

### Gerarchia di Ereditarietà

```csharp
public class Veicolo {
    public string Marca { get; set; }
    public int Anno { get; set; }

    public virtual void Accelera() {
        Console.WriteLine("Il veicolo accelera");
    }
}

public class Auto : Veicolo {
    public int NumeroPorte { get; set; }

    public override void Accelera() {
        Console.WriteLine("L'auto accelera velocemente");
    }
}

public class Moto : Veicolo {
    public int Cilindrata { get; set; }

    public override void Accelera() {
        Console.WriteLine("La moto accelera rapidamente");
    }
}

public class AutoElettrica : Auto {
    public int CapacitaBatteria { get; set; }

    public override void Accelera() {
        Console.WriteLine("L'auto elettrica accelera silenziosamente");
    }
}
```

### Diagramma: Gerarchia Multi-Livello

```
┌─────────────────────────┐
│      Veicolo            │
│  - Marca                │
│  - Anno                 │
│  + Accelera()           │
└─────────────────────────┘
         │
    ┌────┴────┐
    │         │
    ▼         ▼
┌───────┐ ┌───────┐
│  Auto │ │  Moto │
│  -    │ │  -    │
│  Num  │ │  Cil  │
│  Porte│ │  indr │
└───────┘ └───────┘
    │
    ▼
┌───────────────┐
│ AutoElettrica│
│ - Capacita    │
│   Batteria    │
└───────────────┘
```

### Utilizzo dell'Ereditarietà

```csharp
// Utilizzo
Cane cane = new Cane {
    Nome = "Fido",
    Eta = 5,
    Razza = "Labrador"
};

// Può usare metodi ereditati
cane.Dormi();  // Ereditato da Animale
cane.EmettiSuono();  // Override specifico
cane.Abbaia();  // Metodo proprio

// Polimorfismo
Animale animale = new Cane { Nome = "Rex" };
animale.EmettiSuono();  // Chiama il metodo di Cane
```

---

## 4. Polimorfismo (Polymorphism)

### Definizione

Il **polimorfismo** permette a oggetti di classi diverse di essere trattati attraverso un'interfaccia comune, rispondendo in modo diverso allo stesso messaggio.

### Tipi di Polimorfismo

#### 4.1 Polimorfismo a Tempo di Compilazione (Overloading)

```csharp
public class Calcolatrice {
    // Stesso metodo, parametri diversi
    public int Somma(int a, int b) {
        return a + b;
    }

    public double Somma(double a, double b) {
        return a + b;
    }

    public int Somma(int a, int b, int c) {
        return a + b + c;
    }
}

// Utilizzo
Calcolatrice calc = new Calcolatrice();
calc.Somma(5, 3);        // Chiama Somma(int, int)
calc.Somma(5.5, 3.2);    // Chiama Somma(double, double)
calc.Somma(1, 2, 3);     // Chiama Somma(int, int, int)
```

#### 4.2 Polimorfismo a Tempo di Esecuzione (Override)

```csharp
public class Forma {
    public virtual double CalcolaArea() {
        return 0;
    }

    public virtual void Disegna() {
        Console.WriteLine("Disegno una forma generica");
    }
}

public class Cerchio : Forma {
    public double Raggio { get; set; }

    public override double CalcolaArea() {
        return Math.PI * Raggio * Raggio;
    }

    public override void Disegna() {
        Console.WriteLine("Disegno un cerchio");
    }
}

public class Rettangolo : Forma {
    public double Larghezza { get; set; }
    public double Altezza { get; set; }

    public override double CalcolaArea() {
        return Larghezza * Altezza;
    }

    public override void Disegna() {
        Console.WriteLine("Disegno un rettangolo");
    }
}
```

### Diagramma: Polimorfismo

```
┌─────────────────────────────────────────────┐
│  Forma forma = new Cerchio();               │
│  forma.Disegna();  // Disegna un cerchio    │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Riferimento Forma    │
        │  (tipo base)          │
        └───────────────────────┘
                    │
        ┌───────────┴───────────┐
        │                       │
        ▼                       ▼
┌───────────────┐      ┌───────────────┐
│  Cerchio      │      │  Rettangolo   │
│  (oggetto)    │      │  (oggetto)    │
└───────────────┘      └───────────────┘
        │                       │
        └───────────┬───────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Metodo appropriato   │
        │  chiamato in base     │
        │  all'oggetto reale    │
        └───────────────────────┘
```

### Esempio: Polimorfismo con Lista

```csharp
// Lista polimorfica
List<Forma> forme = new List<Forma> {
    new Cerchio { Raggio = 5 },
    new Rettangolo { Larghezza = 10, Altezza = 5 },
    new Cerchio { Raggio = 3 }
};

// Ogni forma risponde in modo diverso
foreach (Forma forma in forme) {
    forma.Disegna();  // Polimorfismo!
    Console.WriteLine($"Area: {forma.CalcolaArea()}");
}

// Output:
// Disegno un cerchio
// Area: 78.5398163397448
// Disegno un rettangolo
// Area: 50
// Disegno un cerchio
// Area: 28.2743338823081
```

### Parole Chiave: virtual, override, new

```csharp
public class Base {
    public virtual void Metodo() {
        Console.WriteLine("Metodo Base");
    }
}

public class Derivata : Base {
    // Override - sostituisce il metodo base
    public override void Metodo() {
        Console.WriteLine("Metodo Derivata");
    }
}

public class AltraDerivata : Base {
    // new - nasconde il metodo base (non override)
    public new void Metodo() {
        Console.WriteLine("Metodo AltraDerivata");
    }
}

// Utilizzo
Base b1 = new Derivata();
b1.Metodo();  // "Metodo Derivata" (override)

Base b2 = new AltraDerivata();
b2.Metodo();  // "Metodo Base" (new nasconde, ma non override)
```

---

## 5. Astrazione (Abstraction)

### Definizione

L'**astrazione** è il processo di nascondere i dettagli di implementazione e mostrare solo le funzionalità essenziali all'utente.

### Astrazione tramite Classi Astratte

```csharp
// Classe astratta - non può essere istanziata
public abstract class Animale {
    public string Nome { get; set; }

    // Metodo astratto - deve essere implementato dalle classi derivate
    public abstract void EmettiSuono();

    // Metodo concreto - implementazione di default
    public void Dormi() {
        Console.WriteLine($"{Nome} sta dormendo");
    }
}

// Classe concreta - implementa i metodi astratti
public class Cane : Animale {
    public override void EmettiSuono() {
        Console.WriteLine("Bau!");
    }
}

public class Gatto : Animale {
    public override void EmettiSuono() {
        Console.WriteLine("Miao!");
    }
}

// Utilizzo
// Animale a = new Animale();  // ❌ ERRORE! Classe astratta
Animale cane = new Cane { Nome = "Fido" };
cane.EmettiSuono();  // "Bau!"
cane.Dormi();  // "Fido sta dormendo"
```

### Diagramma: Classe Astratta

```
┌─────────────────────────────────────┐
│    Animale (Astratta)                │
│  - Nome (concreto)                  │
│  + Dormi() (concreto)               │
│  + EmettiSuono() (astratto) ◄──────┼─── Deve essere implementato
└─────────────────────────────────────┘
            ▲              ▲
            │              │
            │              │
    ┌───────┘              └───────┐
    │                              │
┌─────────┐                  ┌─────────┐
│  Cane   │                  │  Gatto  │
│  + EmettiSuono()           │  + EmettiSuono()│
│    "Bau!"                   │    "Miao!"     │
└─────────┘                  └─────────┘
```

### Astrazione tramite Interfacce

```csharp
// Interfaccia - definisce solo il contratto
public interface IVolatile {
    void Vola();
    int VelocitaMassima { get; set; }
}

public interface IAcquatico {
    void Nuota();
    int ProfonditaMassima { get; set; }
}

// Una classe può implementare più interfacce
public class Anatra : IVolatile, IAcquatico {
    public int VelocitaMassima { get; set; } = 50;
    public int ProfonditaMassima { get; set; } = 5;

    public void Vola() {
        Console.WriteLine("L'anatra vola");
    }

    public void Nuota() {
        Console.WriteLine("L'anatra nuota");
    }
}

// Utilizzo tramite interfaccia
IVolatile volatile = new Anatra();
volatile.Vola();  // Usa solo i metodi di IVolatile

IAcquatico acquatico = new Anatra();
acquatico.Nuota();  // Usa solo i metodi di IAcquatico
```

### Diagramma: Interfacce

```
┌──────────────────┐         ┌──────────────────┐
│   IVolatile       │         │   IAcquatico      │
│  + Vola()         │         │  + Nuota()       │
│  + VelocitaMax    │         │  + ProfonditaMax  │
└──────────────────┘         └──────────────────┘
        │                              │
        └──────────┬───────────────────┘
                   │
                   ▼
            ┌───────────┐
            │  Anatra   │
            │  (implementa entrambe)│
            └───────────┘
```

### Confronto: Classe Astratta vs Interfaccia

| Caratteristica      | Classe Astratta              | Interfaccia                                         |
| ------------------- | ---------------------------- | --------------------------------------------------- |
| **Istanziazione**   | ❌ Non può essere istanziata | ❌ Non può essere istanziata                        |
| **Implementazione** | ✅ Può avere metodi concreti | ❌ Solo firme (C# 8+ ha implementazione di default) |
| **Campi**           | ✅ Può avere campi           | ❌ No (solo proprietà)                              |
| **Ereditarietà**    | ✅ Una sola classe           | ✅ Multiple interfacce                              |
| **Modificatori**    | ✅ Può avere membri privati  | ❌ Tutto pubblico                                   |
| **Quando usare**    | Condivisione codice comune   | Contratto di comportamento                          |

---

## 6. Esempi Pratici Completi

### Esempio 1: Sistema di Gestione Veicoli

```csharp
// Classe astratta base
public abstract class Veicolo {
    public string Marca { get; set; }
    public string Modello { get; set; }
    public int Anno { get; set; }

    // Metodo astratto - ogni veicolo accelera diversamente
    public abstract void Accelera();

    // Metodo concreto - comune a tutti
    public virtual void Frena() {
        Console.WriteLine($"{Marca} {Modello} sta frenando");
    }
}

// Interfaccia per veicoli elettrici
public interface IElettrico {
    int CapacitaBatteria { get; set; }
    void Ricarica();
}

// Interfaccia per veicoli con combustibile
public interface ICombustibile {
    string TipoCarburante { get; set; }
    void Rifornisci();
}

// Auto - implementa ICombustibile
public class Auto : Veicolo, ICombustibile {
    public int NumeroPorte { get; set; }
    public string TipoCarburante { get; set; }

    public override void Accelera() {
        Console.WriteLine("L'auto accelera con il motore");
    }

    public void Rifornisci() {
        Console.WriteLine($"Rifornimento di {TipoCarburante}");
    }
}

// Auto Elettrica - implementa IElettrico
public class AutoElettrica : Veicolo, IElettrico {
    public int CapacitaBatteria { get; set; }

    public override void Accelera() {
        Console.WriteLine("L'auto elettrica accelera silenziosamente");
    }

    public void Ricarica() {
        Console.WriteLine($"Ricaricando batteria da {CapacitaBatteria} kWh");
    }
}

// Utilizzo
List<Veicolo> veicoli = new List<Veicolo> {
    new Auto { Marca = "Toyota", Modello = "Corolla", TipoCarburante = "Benzina" },
    new AutoElettrica { Marca = "Tesla", Modello = "Model 3", CapacitaBatteria = 75 }
};

foreach (Veicolo v in veicoli) {
    v.Accelera();  // Polimorfismo!
}
```

---

## 7. Riepilogo: I Quattro Pilastri

### Tabella Riepilogo

| Pilastro           | Scopo                              | Meccanismo C#                     | Esempio                       |
| ------------------ | ---------------------------------- | --------------------------------- | ----------------------------- |
| **Incapsulamento** | Nascondere dettagli                | `private`, `protected`, proprietà | `private decimal saldo;`      |
| **Ereditarietà**   | Riutilizzare codice                | `class Derivata : Base`           | `class Cane : Animale`        |
| **Polimorfismo**   | Stesso nome, comportamento diverso | `virtual`, `override`             | `Forma.Disegna()`             |
| **Astrazione**     | Modellare concetti                 | `abstract`, `interface`           | `abstract class`, `interface` |

### Diagramma Completo: Relazione tra i Pilastri

```
┌─────────────────────────────────────────────────┐
│                  OOP                            │
└─────────────────────────────────────────────────┘
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌──────────────┐ ┌──────────────┐ ┌──────────────┐
│ INCAPSULA-   │ │ EREDITA-     │ │ POLIMOR-     │
│ MENTO        │ │ RITÀ         │ │ FISMO        │
│              │ │              │ │              │
│ Nasconde     │ │ Riutilizza   │ │ Stesso nome, │
│ dettagli     │ │ codice       │ │ diversi      │
│              │ │              │ │ comportamenti│
└──────────────┘ └──────────────┘ └──────────────┘
        │               │               │
        └───────────────┼───────────────┘
                        │
                        ▼
                ┌──────────────┐
                │ ASTRAZIONE   │
                │              │
                │ Modella      │
                │ concetti     │
                └──────────────┘
```

---

## 8. Best Practices OOP

### ✅ Cosa Fare

1. **Usa l'incapsulamento per proteggere i dati**

   ```csharp
   // ✅ CORRETTO
   private decimal saldo;
   public decimal GetSaldo() { return saldo; }
   ```

2. **Preferisci composizione a ereditarietà**

   ```csharp
   // ✅ CORRETTO - Composizione
   public class Auto {
       private Motore motore;  // Composizione
   }
   ```

3. **Usa interfacce per flessibilità**

   ```csharp
   // ✅ CORRETTO
   public interface IRepository {
       void Save();
   }
   ```

4. **Rendi le classi responsabili di una sola cosa**
   ```csharp
   // ✅ CORRETTO - Single Responsibility
   public class Persona { }
   public class PersonaValidator { }
   public class PersonaRepository { }
   ```

### ❌ Cosa Evitare

1. **Non esporre campi pubblici**

   ```csharp
   // ❌ SBAGLIATO
   public class Persona {
       public string nome;  // Non protetto!
   }

   // ✅ CORRETTO
   public class Persona {
       private string nome;
       public string Nome { get; set; }
   }
   ```

2. **Non creare gerarchie troppo profonde**

   ```csharp
   // ⚠️ EVITARE troppe livelli
   A -> B -> C -> D -> E -> F
   ```

3. **Non usare ereditarietà per riutilizzare codice se non c'è relazione "è-un"**

   ```csharp
   // ❌ SBAGLIATO
   class Auto : Motore { }  // Auto NON è un Motore

   // ✅ CORRETTO
   class Auto {
       private Motore motore;  // Auto HA un Motore
   }
   ```

---

## 9. Domande Frequenti (FAQ)

### Q: Quando usare una classe astratta vs un'interfaccia?

**R:** Usa una classe astratta quando hai codice comune da condividere. Usa un'interfaccia quando definisci solo un contratto.

### Q: Posso avere più ereditarietà in C#?

**R:** No, una classe può ereditare da una sola classe base, ma può implementare multiple interfacce.

### Q: Qual è la differenza tra override e new?

**R:** `override` sostituisce il metodo base. `new` nasconde il metodo base ma non lo sostituisce.

### Q: Quando usare virtual vs abstract?

**R:** `virtual` per metodi con implementazione di default che possono essere sovrascritti. `abstract` per metodi che devono essere implementati.

---

## Conclusioni

I quattro pilastri dell'OOP in C# sono:

- ✅ **Incapsulamento**: Protegge i dati e nasconde l'implementazione
- ✅ **Ereditarietà**: Riutilizza codice e crea relazioni gerarchiche
- ✅ **Polimorfismo**: Permette flessibilità e comportamento dinamico
- ✅ **Astrazione**: Modella concetti e semplifica la complessità

Comprendere questi principi è fondamentale per scrivere codice C# ben strutturato, manutenibile e scalabile.

---

_Documento creato per spiegare i principi OOP in C# con esempi pratici e diagrammi._
