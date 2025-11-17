# Tipi Riferimento e Valore: Stack e Heap

## Introduzione

In programmazione, è fondamentale comprendere la differenza tra **tipi riferimento** e **tipi valore**, e come vengono gestiti in memoria attraverso **stack** e **heap**. Questi concetti sono cruciali per comprendere il comportamento dei programmi e l'uso della memoria.

---

## 1. Tipi Valore (Value Types)

### Caratteristiche

I **tipi valore** sono tipi di dati che memorizzano direttamente il loro valore. Quando si assegna un tipo valore a una variabile, viene creata una **copia** del valore.

**Esempi di tipi valore:**

- `int`, `float`, `double`, `char`
- `bool`
- `struct` (in C#)
- `enum`

### Comportamento

```csharp
int a = 10;
int b = a;  // b riceve una COPIA del valore di a
a = 20;     // modifica solo a, b rimane 10

Console.WriteLine(a); // 20
Console.WriteLine(b); // 10
```

### Rappresentazione in Memoria

```
┌─────────────┐
│   STACK     │
├─────────────┤
│  a = 10     │ ← Variabile a contiene direttamente il valore
│  b = 10     │ ← Variabile b contiene una COPIA del valore
└─────────────┘
```

---

## 2. Tipi Riferimento (Reference Types)

### Caratteristiche

I **tipi riferimento** memorizzano un **riferimento** (indirizzo) all'oggetto in memoria, non il valore stesso. Più variabili possono riferirsi allo stesso oggetto.

**Esempi di tipi riferimento:**

- `class` (in C#)
- `string`
- `array`
- `object`

### Comportamento

```csharp
class Persona {
    public string Nome;
}

Persona p1 = new Persona { Nome = "Mario" };
Persona p2 = p1;  // p2 punta allo STESSO oggetto di p1
p1.Nome = "Luigi"; // modifica l'oggetto condiviso

Console.WriteLine(p1.Nome); // "Luigi"
Console.WriteLine(p2.Nome); // "Luigi" (stesso oggetto!)
```

### Rappresentazione in Memoria

```
┌─────────────┐         ┌─────────────┐
│   STACK     │         │    HEAP     │
├─────────────┤         ├─────────────┤
│  p1 ────────┼────────>│  Persona    │
│  (rifer.)   │         │  Nome="Luigi"│
│             │         └─────────────┘
│  p2 ────────┼─────────┘
│  (rifer.)   │         (p1 e p2 puntano allo stesso oggetto)
└─────────────┘
```

---

## 3. Stack (Pila)

### Definizione

Lo **stack** è una struttura dati **LIFO** (Last In, First Out) utilizzata per memorizzare:

- Variabili locali
- Parametri di funzioni
- Indirizzi di ritorno
- Riferimenti a oggetti (per tipi riferimento)

### Caratteristiche

✅ **Veloce** - Accesso molto rapido  
✅ **Dimensioni limitate** - Generalmente 1-2 MB  
✅ **Gestito automaticamente** - Allocazione/deallocazione automatica  
✅ **Memoria contigua** - Accesso sequenziale efficiente

### Diagramma Stack

```
┌─────────────────┐
│   Funzione 3    │ ← Ultima chiamata (prima a essere rimossa)
├─────────────────┤
│   Funzione 2    │
├─────────────────┤
│   Funzione 1    │
├─────────────────┤
│   Funzione main │ ← Prima chiamata (ultima a essere rimossa)
└─────────────────┘
     ↑
   TOP dello stack
```

### Esempio di Utilizzo

```csharp
void Metodo1() {
    int x = 10;        // Variabile locale nello stack
    Metodo2();
    // Quando Metodo1 termina, x viene rimossa dallo stack
}

void Metodo2() {
    int y = 20;        // Variabile locale nello stack
    // Quando Metodo2 termina, y viene rimossa dallo stack
}
```

**Sequenza di allocazione/deallocazione:**

1. `Metodo1` viene chiamato → `x` allocato nello stack
2. `Metodo2` viene chiamato → `y` allocato nello stack (sopra `x`)
3. `Metodo2` termina → `y` rimosso dallo stack
4. `Metodo1` termina → `x` rimosso dallo stack

---

## 4. Heap (Mucchio)

### Definizione

L'**heap** è una regione di memoria utilizzata per l'allocazione dinamica di oggetti. È più grande dello stack ma più lento nell'accesso.

### Caratteristiche

✅ **Dimensioni maggiori** - Generalmente diversi GB  
⚠️ **Più lento** - Accesso meno efficiente  
✅ **Flessibile** - Allocazione dinamica  
⚠️ **Richiede garbage collection** - Gestione automatica o manuale

### Diagramma Heap

```
┌─────────────────────────────────┐
│           HEAP                  │
├─────────────────────────────────┤
│  [Oggetto 1]  [Oggetto 2]       │ ← Oggetti allocati
│  [Libero]     [Oggetto 3]       │
│  [Oggetto 4]  [Libero]          │
│  [Libero]     [Oggetto 5]       │
└─────────────────────────────────┘
     ↑              ↑
  Frammentato, allocazione dinamica
```

### Esempio di Utilizzo

```csharp
void Metodo() {
    // Oggetto allocato nello HEAP
    Persona persona = new Persona { Nome = "Mario" };

    // La variabile 'persona' nello STACK contiene un riferimento
    // che punta all'oggetto nello HEAP

    // Quando il metodo termina:
    // - La variabile 'persona' viene rimossa dallo STACK
    // - L'oggetto nello HEAP rimane fino al Garbage Collection
}
```

---

## 5. Confronto Completo: Stack vs Heap

### Tabella Comparativa

| Caratteristica     | Stack                | Heap                   |
| ------------------ | -------------------- | ---------------------- |
| **Velocità**       | ⚡ Molto veloce      | 🐌 Più lento           |
| **Dimensione**     | 📏 Limitata (1-2 MB) | 📦 Grande (GB)         |
| **Gestione**       | 🤖 Automatica        | 🧹 Garbage Collection  |
| **Accesso**        | 📍 Diretto           | 🔗 Tramite riferimento |
| **Organizzazione** | 📚 LIFO (ordine)     | 🗂️ Frammentato         |
| **Uso tipico**     | Variabili locali     | Oggetti dinamici       |

---

## 6. Esempio Completo: Come Funziona in Memoria

### Codice di Esempio

```csharp
class Program {
    static void Main() {
        // TIPO VALORE nello STACK
        int numero = 42;

        // TIPO RIFERIMENTO: riferimento nello STACK, oggetto nello HEAP
        Persona persona = new Persona { Nome = "Mario", Eta = 30 };

        // Array: riferimento nello STACK, elementi nello HEAP
        int[] numeri = new int[] { 1, 2, 3 };

        Metodo(numero, persona);
    }

    static void Metodo(int x, Persona p) {
        int y = x * 2;  // Nuovo tipo valore nello stack
        p.Nome = "Luigi"; // Modifica oggetto nello heap
    }
}
```

### Rappresentazione Memoria Completa

```
┌─────────────────────────────────────────┐
│              STACK                      │
├─────────────────────────────────────────┤
│  Metodo:                                │
│    y = 84                               │ ← Tipo valore
│    p ──────┐                            │ ← Riferimento
│    x = 42  │                            │ ← Tipo valore (copia)
├─────────────────────────────────────────┤
│  Main:                                  │
│    numeri ──────┐                       │ ← Riferimento
│    persona ─────┼───┐                   │ ← Riferimento
│    numero = 42  │   │                   │ ← Tipo valore
└─────────────────┼───┼───────────────────┘
                  │   │
                  │   │
┌─────────────────┼───┼───────────────────┐
│              HEAP                       │
├─────────────────┼───┼───────────────────┤
│                 │   │                   │
│  [Array]        │   │                   │
│  [0]=1          │   │                   │
│  [1]=2          │   │                   │
│  [2]=3          │   │                   │
│                 │   │                   │
│  [Persona]      │   │                   │
│  Nome="Luigi"   │<──┘                   │
│  Eta=30         │                       │
│                 │                       │
└─────────────────┘                       ┘
```

---

## 7. Passaggio per Valore vs Passaggio per Riferimento

### Passaggio per Valore

```csharp
void ModificaValore(int x) {
    x = 100;  // Modifica solo la copia locale
}

int numero = 10;
ModificaValore(numero);
Console.WriteLine(numero); // Ancora 10! (non modificato)
```

**Spiegazione:**

- `numero` viene **copiato** nello stack del metodo
- Le modifiche interessano solo la copia, non l'originale

### Passaggio per Riferimento

```csharp
void ModificaRiferimento(Persona p) {
    p.Nome = "Modificato";  // Modifica l'oggetto nello heap
}

Persona persona = new Persona { Nome = "Originale" };
ModificaRiferimento(persona);
Console.WriteLine(persona.Nome); // "Modificato"! (oggetto modificato)
```

**Spiegazione:**

- `persona` contiene un riferimento (indirizzo)
- Il riferimento viene copiato nello stack del metodo
- Entrambi i riferimenti puntano allo **stesso oggetto** nello heap
- Le modifiche influenzano l'oggetto originale

---

## 8. Il Tipo String: Un Caso Speciale

### Introduzione

Il tipo `string` in C# è un **tipo riferimento**, ma ha caratteristiche particolari che lo rendono diverso dagli altri tipi riferimento. Comprendere come funziona `string` è fondamentale per evitare errori comuni.

### String è un Tipo Riferimento

```csharp
string s1 = "Ciao";
string s2 = s1;  // s2 punta allo stesso oggetto di s1
```

**Rappresentazione in Memoria:**

```
┌─────────────┐         ┌─────────────┐
│   STACK     │         │    HEAP     │
├─────────────┤         ├─────────────┤
│  s1 ────────┼────────>│  "Ciao"     │
│  (rifer.)   │         │             │
│             │         └─────────────┘
│  s2 ────────┼─────────┘
│  (rifer.)   │         (s1 e s2 puntano allo stesso oggetto)
└─────────────┘
```

### Immutabilità delle Stringhe

Le stringhe in C# sono **immutabili** (non modificabili). Quando modifichi una stringa, viene creato un **nuovo oggetto** nello heap.

```csharp
string s1 = "Ciao";
string s2 = s1;  // Entrambi puntano a "Ciao"

// Modifica apparente
s1 = "Arrivederci";  // Crea un NUOVO oggetto nello heap!

Console.WriteLine(s1); // "Arrivederci"
Console.WriteLine(s2); // "Ciao" (non modificato!)
```

**Rappresentazione Dopo la Modifica:**

```
┌─────────────┐         ┌─────────────┐
│   STACK     │         │    HEAP     │
├─────────────┤         ├─────────────┤
│  s1 ────────┼────────>│ "Arrivederci"│
│  (rifer.)   │         │             │
│             │         │  "Ciao"     │
│  s2 ────────┼────────>│  (ancora    │
│  (rifer.)   │         │   esistente)│
└─────────────┘         └─────────────┘
```

### String Interning (Pool di Stringhe)

C# mantiene un **string pool** (pool di stringhe) per le stringhe letterali. Stringhe identiche possono condividere lo stesso oggetto in memoria.

```csharp
string s1 = "Ciao";
string s2 = "Ciao";
string s3 = "Ciao";

// Con stringa letterale, potrebbero riferire lo stesso oggetto
bool stessoOggetto = ReferenceEquals(s1, s2);  // ✅ true (string interning)

// Con new, crea sempre un nuovo oggetto
string s4 = new string("Ciao".ToCharArray());
bool stessoOggetto2 = ReferenceEquals(s1, s4);  // ❌ false (nuovo oggetto)
```

**Diagramma: String Interning**

```
┌─────────────────────────────────────────────┐
│  String Literal Pool (String Interning)    │
├─────────────────────────────────────────────┤
│  "Ciao" (oggetto condiviso)                │
│    ↑          ↑          ↑                 │
│    │          │          │                 │
│   s1         s2         s3                 │
│  (tutti puntano allo stesso oggetto)      │
└─────────────────────────────────────────────┘
```

### Esempio Completo: Comportamento delle Stringhe

```csharp
// Assegnazione iniziale
string s1 = "Mario";
string s2 = s1;  // Entrambi puntano a "Mario"

Console.WriteLine(ReferenceEquals(s1, s2));  // ✅ true (stesso oggetto)

// Modifica di s1
s1 = "Luigi";  // Crea un NUOVO oggetto "Luigi"

Console.WriteLine(s1);  // "Luigi"
Console.WriteLine(s2);  // "Mario" (non modificato!)
Console.WriteLine(ReferenceEquals(s1, s2));  // ❌ false (oggetti diversi)

// Concatenazione crea un nuovo oggetto
string s3 = s2 + " Rossi";  // Crea un NUOVO oggetto "Mario Rossi"
Console.WriteLine(s2);  // "Mario" (non modificato)
Console.WriteLine(s3);  // "Mario Rossi" (nuovo oggetto)
```

### String vs Altri Tipi Riferimento

```csharp
// String - immutabile
string s1 = "Ciao";
string s2 = s1;
s1 = "Arrivederci";  // Crea nuovo oggetto
Console.WriteLine(s2);  // "Ciao" (non modificato)

// Altri tipi riferimento - mutabili
class Persona {
    public string Nome;
}

Persona p1 = new Persona { Nome = "Mario" };
Persona p2 = p1;
p1.Nome = "Luigi";  // Modifica l'oggetto esistente
Console.WriteLine(p2.Nome);  // "Luigi" (modificato!)
```

**Confronto:**

| Caratteristica       | String                | Altri Tipi Riferimento     |
| -------------------- | --------------------- | -------------------------- |
| **Tipo**             | Tipo riferimento      | Tipo riferimento           |
| **Immutabilità**     | ✅ Immutabile         | ❌ Mutabile                |
| **Modifica**         | Crea nuovo oggetto    | Modifica oggetto esistente |
| **Assegnazione**     | Riferimento copiato   | Riferimento copiato        |
| **String Interning** | ✅ Sì (per letterali) | ❌ No                      |

### Diagramma: Immutabilità delle Stringhe

```
┌─────────────────────────────────────────────┐
│  string s1 = "Ciao";                       │
│  string s2 = s1;                           │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  HEAP: "Ciao"         │
        │    ↑        ↑         │
        │    │        │         │
        │   s1       s2         │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  s1 = "Arrivederci";   │
        │  (NUOVO oggetto)        │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  HEAP:                │
        │  "Ciao" ← s2          │
        │  "Arrivederci" ← s1    │
        │  (2 oggetti separati)  │
        └───────────────────────┘
```

### Metodi che Modificano Stringhe

I metodi che "modificano" stringhe in realtà creano **nuovi oggetti**:

```csharp
string s1 = "Ciao Mondo";

// ToUpper() - crea nuovo oggetto
string s2 = s1.ToUpper();  // Nuovo oggetto "CIAO MONDO"
Console.WriteLine(s1);  // "Ciao Mondo" (non modificato)
Console.WriteLine(s2);  // "CIAO MONDO"

// Replace() - crea nuovo oggetto
string s3 = s1.Replace("Ciao", "Salve");  // Nuovo oggetto "Salve Mondo"
Console.WriteLine(s1);  // "Ciao Mondo" (non modificato)
Console.WriteLine(s3);  // "Salve Mondo"

// Substring() - crea nuovo oggetto
string s4 = s1.Substring(0, 4);  // Nuovo oggetto "Ciao"
Console.WriteLine(s1);  // "Ciao Mondo" (non modificato)
Console.WriteLine(s4);  // "Ciao"
```

### StringBuilder per Modifiche Frequenti

Per modifiche frequenti alle stringhe, usa `StringBuilder` per evitare di creare molti oggetti:

```csharp
// ❌ SBAGLIATO - Crea molti oggetti
string risultato = "";
for (int i = 0; i < 1000; i++) {
    risultato += i.ToString();  // Crea nuovo oggetto ogni volta!
}

// ✅ CORRETTO - Usa StringBuilder
StringBuilder sb = new StringBuilder();
for (int i = 0; i < 1000; i++) {
    sb.Append(i.ToString());  // Modifica lo stesso oggetto
}
string risultato = sb.ToString();  // Crea stringa finale una sola volta
```

### Diagramma: String vs StringBuilder

```
┌─────────────────────────────────────────────┐
│  String (Immutabile)                        │
│  risultato += "a"  →  Nuovo oggetto        │
│  risultato += "b"  →  Nuovo oggetto        │
│  risultato += "c"  →  Nuovo oggetto        │
│  (3 oggetti nello heap)                     │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  StringBuilder (Mutabile)                    │
│  sb.Append("a")  →  Modifica stesso oggetto│
│  sb.Append("b")  →  Modifica stesso oggetto  │
│  sb.Append("c")  →  Modifica stesso oggetto  │
│  sb.ToString()   →  Crea stringa finale     │
│  (1 oggetto StringBuilder + 1 stringa finale)│
└─────────────────────────────────────────────┘
```

### Confronto tra String e Altri Tipi: Tabella Riepilogativa

| Aspetto                     | String                  | Altri Tipi Riferimento (es. Persona) |
| --------------------------- | ----------------------- | ------------------------------------ |
| **Allocazione**             | Heap                    | Heap                                 |
| **Riferimento nello Stack** | ✅ Sì                   | ✅ Sì                                |
| **Immutabilità**            | ✅ Immutabile           | ❌ Mutabile                          |
| **Assegnazione**            | Copia riferimento       | Copia riferimento                    |
| **Modifica**                | Crea nuovo oggetto      | Modifica oggetto esistente           |
| **String Interning**        | ✅ Sì (letterali)       | ❌ No                                |
| **Performance modifiche**   | ⚠️ Lenta (crea oggetti) | ✅ Veloce (modifica diretta)         |

### Best Practices per le Stringhe

✅ **Cosa Fare:**

1. **Usa string per testi immutabili**

   ```csharp
   string nome = "Mario";  // ✅ OK
   ```

2. **Usa StringBuilder per concatenazioni frequenti**

   ```csharp
   StringBuilder sb = new StringBuilder();
   for (int i = 0; i < 100; i++) {
       sb.Append(i);  // ✅ OK
   }
   ```

3. **Usa string.Empty invece di ""**
   ```csharp
   string s = string.Empty;  // ✅ OK
   ```

❌ **Cosa Evitare:**

1. **Non concatenare stringhe in loop**

   ```csharp
   // ❌ SBAGLIATO
   string risultato = "";
   for (int i = 0; i < 1000; i++) {
       risultato += i;  // Crea molti oggetti!
   }
   ```

2. **Non aspettarti che le stringhe si comportino come altri tipi riferimento**
   ```csharp
   string s1 = "Ciao";
   string s2 = s1;
   s1 = "Arrivederci";
   // s2 è ancora "Ciao" (non "Arrivederci"!)
   ```

---

## 9. Garbage Collection

### Cos'è?

Il **Garbage Collector** (GC) è un meccanismo automatico che:

- Identifica oggetti nello heap non più referenziati
- Libera la memoria occupata da questi oggetti
- Compatta la memoria per ridurre la frammentazione

### Diagramma Garbage Collection

```
HEAP (Prima del GC):
┌──────┐ ┌──────┐ ┌──────┐ ┌──────┐
│ Ogg1 │ │ Ogg2 │ │ Ogg3 │ │ Ogg4 │
│ ✓    │ │ ✗    │ │ ✓    │ │ ✗    │
└──────┘ └──────┘ └──────┘ └──────┘
  │                │
  └────────────────┘ (ancora referenziati)

HEAP (Dopo il GC):
┌──────┐ ┌──────┐
│ Ogg1 │ │ Ogg3 │
│ ✓    │ │ ✓    │
└──────┘ └──────┘
  (Ogg2 e Ogg4 rimossi, memoria compattata)
```

### Quando Avviene?

Il GC viene eseguito automaticamente quando:

- La memoria heap è piena
- Il sistema è inattivo
- Viene chiamato manualmente (`GC.Collect()`)

---

## 10. Casi d'Uso e Best Practices

### Quando Usare Tipi Valore

✅ **Dati semplici e piccoli**

- Numeri, booleani, caratteri
- Strutture dati leggere (`struct`)

✅ **Quando serve performance**

- Accesso diretto più veloce
- Nessuna allocazione heap

✅ **Quando non serve condivisione**

- Ogni variabile ha la sua copia

### Quando Usare Tipi Riferimento

✅ **Oggetti complessi**

- Classi con molte proprietà
- Oggetti che cambiano nel tempo

✅ **Quando serve condivisione**

- Più variabili devono riferirsi allo stesso oggetto

✅ **Collezioni dinamiche**

- Array, liste, dizionari

---

## 11. Esempi Pratici in Diversi Linguaggi

### C# (C Sharp)

```csharp
// TIPO VALORE
int a = 10;
int b = a;  // Copia
a = 20;     // b rimane 10

// TIPO RIFERIMENTO
string s1 = "Ciao";
string s2 = s1;     // Entrambi riferiscono "Ciao"
s1 = "Arrivederci"; // s1 punta a nuovo oggetto, s2 rimane "Ciao"
```

---

## 12. Diagramma di Flusso: Allocazione in Memoria

```
┌─────────────────────────────────────────────────────────┐
│                    DICHIARAZIONE VARIABILE               │
└─────────────────────────────────────────────────────────┘
                           │
                           ▼
              ┌────────────────────────┐
              │  È un tipo valore?     │
              └────────────────────────┘
                    │              │
              SÌ    │              │    NO
                    ▼              ▼
        ┌──────────────┐    ┌──────────────┐
        │  STACK       │    │  STACK       │
        │  (valore)    │    │  (riferimento)│
        └──────────────┘    └──────┬───────┘
                                   │
                                   ▼
                          ┌─────────────────┐
                          │  new keyword?   │
                          └─────────────────┘
                                │
                          SÌ    │    NO
                                ▼
                        ┌──────────────┐
                        │  HEAP        │
                        │  (oggetto)   │
                        └──────────────┘
```

---

## 13. Riassunto e Conclusioni

### Punti Chiave

1. **Tipi Valore**:

   - Memorizzano il valore direttamente
   - Allocati nello **stack**
   - Copiati quando assegnati
   - Veloce accesso

2. **Tipi Riferimento**:

   - Memorizzano un riferimento (indirizzo)
   - Riferimento nello **stack**, oggetto nello **heap**
   - Condividono lo stesso oggetto
   - Più lento, ma più flessibile

3. **String (Tipo Riferimento Speciale)**:

   - È un tipo riferimento ma **immutabile**
   - Le modifiche creano nuovi oggetti nello heap
   - String interning per le stringhe letterali
   - Usa StringBuilder per modifiche frequenti

4. **Stack**:

   - Veloce, limitato, automatico
   - Per variabili locali e temporanee

5. **Heap**:
   - Grande, flessibile, gestito da GC
   - Per oggetti dinamici e di lunga durata

### Importanza

Comprendere questi concetti è essenziale per:

- 🎯 Scrivere codice efficiente
- 🐛 Evitare bug comuni
- 📊 Gestire correttamente la memoria
- ⚡ Ottimizzare le performance

---

## Riferimenti e Letture Consigliate

- Documentazione ufficiale del linguaggio di programmazione
- "Effective Java" di Joshua Bloch (per Java)
- "C# in Depth" di Jon Skeet (per C#)
- "The C++ Programming Language" di Bjarne Stroustrup (per C++)

---

_Documento creato per spiegare i concetti fondamentali di tipi riferimento, tipi valore, stack e heap in programmazione._
