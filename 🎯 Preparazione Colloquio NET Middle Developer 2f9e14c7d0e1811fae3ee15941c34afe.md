# 🎯 Preparazione Colloquio .NET Middle Developer

# 🎯 Guida Completa al Colloquio .NET Middle Developer

# 🎯 Guida Completa al Colloquio .NET Middle Developer

## Documentazione Concettuale e Strategica

> **Filosofia**: Questa non è una lista di definizioni da memorizzare. Questa è una guida per **capire profondamente** i concetti e **ragionare** come un sviluppatore esperto.
> 

---

# 📚 INDICE STRATEGICO

## PARTE 1: FONDAMENTI CONCETTUALI

1. [Programmazione Object-Oriented - La Filosofia](#oop-filosofia)
2. [Sistema di Tipi in C# - Decisioni Architetturali](#tipi-csharp)
3. [Gestione della Memoria - Come Funziona Davvero](#memoria)
4. [LINQ - Il Potere della Composizione](#linq-potere)

## PARTE 2: PROGRAMMAZIONE ASINCRONA E CONCORRENZA

1. [Async/Await - La Rivoluzione del Non-Blocking](#async-rivoluzione)
2. [Multithreading - Coordinazione e Sincronizzazione](#multithreading)
3. [Patterns di Concorrenza - Quando e Perché](#concorrenza-patterns)

## PARTE 3: ARCHITETTURA WEB

1. [ASP.NET](http://ASP.NET) [Core - Architettura della Pipeline](#aspnet-architettura)
2. [Dependency Injection - Inversione del Controllo](#di-filosofia)
3. [REST API - Principi e Pragmatismo](#rest-principi)
4. [Autenticazione e Sicurezza - Proteggere le Applicazioni](#sicurezza)

## PARTE 4: PERSISTENZA DATI

1. [Entity Framework Core - ORM e Impedance Mismatch](#ef-core)
2. [SQL Server - Ottimizzazione e Performance](#sql-performance)
3. [Strategie di Accesso ai Dati](#data-access)

## PARTE 5: ARCHITETTURA SOFTWARE

1. [Clean Architecture - Separazione delle Responsabilità](#clean-arch)
2. [SOLID Principles - I Pilastri del Design](#solid)
3. [Design Patterns - Soluzioni a Problemi Ricorrenti](#patterns)

## PARTE 6: QUALITÀ E DEPLOYMENT

1. [Testing - Strategie e Filosofia](#testing)
2. [DevOps e CI/CD - Dalla Teoria alla Pratica](#devops)

## PARTE 7: PREPARAZIONE AL COLLOQUIO

1. [200 Domande Fondamentali con Risposte Approfondite](#domande)
2. [Come Rispondere - Strategie di Comunicazione](#strategie)
3. [Esempi dal Tuo CV - Preparazione Personalizzata](#esempi-cv)

---

# PARTE 1: FONDAMENTI CONCETTUALI

## 1. Programmazione Object-Oriented - La Filosofia {#oop-filosofia}

### Perché Esiste l'OOP?

La programmazione object-oriented non è nata per caso. È una **risposta evolutiva** a un problema fondamentale: **come organizziamo la complessità** in sistemi software che crescono?

Nei primi anni della programmazione, i programmi erano piccoli e procedurali. Ma quando i sistemi hanno iniziato a crescere (migliaia, poi milioni di righe di codice), i programmatori hanno capito che serviva un nuovo modo di pensare.

**Il problema centrale**: In un programma procedurale, quando cambi una parte, rischi di rompere tutto. I dati e le funzioni sono separati, e qualsiasi funzione può modificare qualsiasi dato.

**La soluzione OOP**: Raggruppa **dati e comportamenti insieme** in unità chiamate "oggetti". Ogni oggetto sa come gestire se stesso.

---

### I Quattro Pilastri - Spiegazione Profonda

#### **Encapsulation (Incapsulamento)**

**Concetto fondamentale**: L'incapsulamento è l'arte di **nascondere le decisioni**.

Quando scrivi una classe, prendi delle decisioni implementative: come memorizzi i dati, quali algoritmi usi, come gestisci i casi particolari. L'incapsulamento dice: "Queste decisioni sono **private**. Il mondo esterno vede solo l'interfaccia pubblica."

**Perché è cruciale**:

Immagina di avere una classe `Account` con un campo `Balance`. Se lo rendi pubblico, chiunque può scrivere:

```csharp
account.Balance = -1000; // Disaster! 
```

Ma se incapsuli:

```csharp
public class Account
{
    private decimal _balance; // Nascosto
    
    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive");
        if (amount > _balance)
            throw new InvalidOperationException("Insufficient funds");
        
        _balance -= amount;
        // Qui puoi anche loggare, notificare, validare regole business
    }
    
    public decimal GetBalance() => _balance;
}
```

**Cosa ottieni**:

1. **Controllo**: Solo tu decidi come il balance può cambiare
2. **Validazione**: Nessuno può creare stati invalidi
3. **Flessibilità**: Puoi cambiare l'implementazione interna senza rompere il codice che usa la classe
4. **Tracciabilità**: Tutti i cambiamenti passano attraverso punti controllati

**Nel mondo reale**: Nel tuo progetto Template Manager, quando hai creato le entità per gestire i template PDF, l'incapsulamento ti ha permesso di cambiare come memorizzi i placeholder senza rompere il codice che usa quelle entità.

---

#### **Inheritance (Ereditarietà)**

**Concetto fondamentale**: L'ereditarietà modella le **relazioni "è un tipo di"** (IS-A).

Non è semplicemente "riutilizzo del codice". È un modo di dire: "Questo concetto è una **specializzazione** di un concetto più generale."

**Il ragionamento**:

- Un `Manager` **è un tipo di** `Employee`
- Un `SavingsAccount` **è un tipo di** `BankAccount`
- Un `EmailNotification` **è un tipo di** `Notification`

Quando usi l'ereditarietà, stai dicendo: "Questa classe figlia può fare **tutto** ciò che fa la classe padre, **più** qualcosa di specifico."

**Esempio ragionato**:

```csharp
public abstract class Notification
{
    // Tutti i tipi di notifiche hanno un destinatario e un messaggio
    public string Recipient { get; protected set; }
    public string Message { get; protected set; }
    public DateTime SentAt { get; protected set; }
    
    // Tutti devono implementare Send, ma il COME è specifico
    public abstract Task SendAsync();
    
    // Logica comune: logging
    protected void LogNotification()
    {
        Console.WriteLine($"Notification sent to {Recipient} at {SentAt}");
    }
}

public class EmailNotification : Notification
{
    public string Subject { get; set; }
    
    public override async Task SendAsync()
    {
        // Logica specifica per email
        await _emailService.SendEmailAsync(Recipient, Subject, Message);
        SentAt = DateTime.UtcNow;
        LogNotification(); // Riuso logica comune
    }
}

public class SmsNotification : Notification
{
    public string PhoneNumber { get; set; }
    
    public override async Task SendAsync()
    {
        // Logica completamente diversa, ma stessa interfaccia
        await _smsService.SendSmsAsync(PhoneNumber, Message);
        SentAt = DateTime.UtcNow;
        LogNotification();
    }
}
```

**Cosa ottieni**:

1. **Riutilizzo intelligente**: La logica comune (logging, proprietà base) è scritta una volta
2. **Polimorfismo**: Puoi trattare tutte le notifiche allo stesso modo
3. **Estensibilità**: Aggiungere `PushNotification` è facile

**Attenzione - Il pericolo**:

L'ereditarietà crea **accoppiamento forte**. Se cambi la classe base, tutte le classi figlie sono impattate. Per questo, la regola moderna è: **"Composition over inheritance"** - preferisci comporre oggetti piuttosto che ereditare.

**Nel tuo lavoro**: Quando hai costruito il License Management System, probabilmente avevi diversi tipi di licenze (Trial, Professional, Enterprise). L'ereditarietà ti avrebbe permesso di modellare `TrialLicense : License` con validazioni specifiche.

---

#### **Polymorphism (Polimorfismo)**

**Concetto fondamentale**: Il polimorfismo è la capacità di **trattare oggetti diversi attraverso la stessa interfaccia**.

È il "superpotere" dell'OOP. Ti permette di scrivere codice che funziona con **famiglie di tipi**, non tipi specifici.

**Il valore pratico**:

Senza polimorfismo, per processare diverse forme di pagamento:

```csharp
// Codice BRUTTO - rigido, non scalabile
if (payment is CreditCardPayment)
{
    var cc = (CreditCardPayment)payment;
    ProcessCreditCard(cc.CardNumber, cc.Amount);
}
else if (payment is PayPalPayment)
{
    var pp = (PayPalPayment)payment;
    ProcessPayPal(pp.Email, pp.Amount);
}
else if (payment is BankTransferPayment)
{
    var bt = (BankTransferPayment)payment;
    ProcessBankTransfer(bt.IBAN, bt.Amount);
}
// Ogni nuovo metodo = nuovo if/else
```

Con polimorfismo:

```csharp
// Codice BELLO - flessibile, scalabile
public interface IPaymentMethod
{
    Task<PaymentResult> ProcessAsync(decimal amount);
}

public class PaymentProcessor
{
    public async Task ProcessPaymentAsync(IPaymentMethod payment, decimal amount)
    {
        var result = await payment.ProcessAsync(amount);
        // Stessa logica per TUTTI i metodi di pagamento
        LogTransaction(result);
        NotifyCustomer(result);
    }
}
```

**Cosa ottieni**:

1. **Estensibilità**: Aggiungere `CryptoPayment` non richiede modifiche a `PaymentProcessor`
2. **Testabilità**: Puoi creare `MockPayment` per i test
3. **Manutenibilità**: Logica centralizzata, non sparsa in if/else

**Virtual/Override - Il meccanismo**:

```csharp
public class Logger
{
    public virtual void Log(string message)
    {
        Console.WriteLine(message); // Comportamento default
    }
}

public class FileLogger : Logger
{
    public override void Log(string message)
    {
        File.AppendAllText("log.txt", message); // Comportamento specializzato
    }
}

public class DatabaseLogger : Logger
{
    public override void Log(string message)
    {
        _dbContext.Logs.Add(new LogEntry { Message = message });
        _dbContext.SaveChanges();
    }
}

// Uso polimorfico
Logger logger = GetLogger(); // Potrebbe essere qualsiasi implementazione
logger.Log("Something happened"); // Il comportamento dipende dal tipo runtime
```

**Nel colloquio**: Quando ti chiedono del polimorfismo, non dire solo "stesso metodo, comportamenti diversi". Spiega il **valore business**: "Mi permette di estendere il sistema senza modificare codice esistente, riducendo i rischi e facilitando i test."

---

#### **Abstraction (Astrazione)**

**Concetto fondamentale**: L'astrazione è l'arte di **modellare l'essenziale, ignorando i dettagli**.

Quando guidi un'auto, usi l'**astrazione**: sterzo, acceleratore, freno. Non pensi all'iniezione del carburante, alla trasmissione, all'ABS. Quei dettagli sono nascosti dietro un'interfaccia semplice.

Nel software, l'astrazione fa lo stesso: **separa il COSA dal COME**.

**Abstract Class vs Interface - La Decisione Architettonica**:

Questa è una delle domande più frequenti nei colloqui, e la risposta giusta non è tecnica, è **concettuale**.

**Abstract Class - Quando hai una FAMIGLIA con logica condivisa**:

```csharp
public abstract class DocumentGenerator
{
    // Template Method Pattern - la sequenza è fissa
    public async Task<byte[]> GenerateAsync()
    {
        ValidateData();        // Tutti devono validare
        var content = await CreateContentAsync(); // Ognuno crea diversamente
        var formatted = FormatDocument(content);   // Formatting comune
        return await SaveAsync(formatted);         // Salvataggio comune
    }
    
    protected abstract Task<string> CreateContentAsync(); // Specifico
    
    protected virtual void ValidateData() // Overridable, ma ha default
    {
        if (string.IsNullOrEmpty(Title))
            throw new ValidationException("Title required");
    }
    
    private byte[] FormatDocument(string content) // Non overridable
    {
        // Logica di formatting condivisa
    }
}

public class PdfGenerator : DocumentGenerator
{
    protected override async Task<string> CreateContentAsync()
    {
        // Logica specifica per PDF
    }
}

public class WordGenerator : DocumentGenerator
{
    protected override async Task<string> CreateContentAsync()
    {
        // Logica specifica per Word
    }
    
    protected override void ValidateData()
    {
        base.ValidateData(); // Usa validazione base
        // Aggiungi validazioni specifiche per Word
    }
}
```

**Quando usare Abstract Class**:

1. Hai logica condivisa concreta (metodi con implementazione)
2. Vuoi definire un "template" di processo
3. Hai stato condiviso (campi, proprietà)
4. Le classi hanno una forte relazione IS-A
5. Vuoi evoluzione controllata (aggiungi metodi protected senza rompere le classi figlie)

**Interface - Quando definisci CAPACITÀ**:

```csharp
public interface ILoggable
{
    void Log(string message);
}

public interface ICacheable
{
    string GetCacheKey();
    TimeSpan GetCacheDuration();
}

public interface IValidatable
{
    ValidationResult Validate();
}

// Una classe può avere MULTIPLE capacità
public class UserService : ILoggable, ICacheable, IValidatable
{
    public void Log(string message) { /* ... */ }
    public string GetCacheKey() { /* ... */ }
    public TimeSpan GetCacheDuration() { /* ... */ }
    public ValidationResult Validate() { /* ... */ }
}
```

**Quando usare Interface**:

1. Definisci contratti, non implementazioni
2. Vuoi implementazione multipla (C# non ha ereditarietà multipla)
3. Accoppiamento debole (Dependency Injection)
4. Testing (facile creare mock)
5. Le classi hanno capacità comuni ma non sono "parenti"

**La differenza filosofica**:

- **Abstract Class**: "Appartieni a questa famiglia, ecco il DNA condiviso"
- **Interface**: "Puoi fare questa cosa, non mi importa come"

**Esempio dal mondo reale**:

Nel tuo KPI Dashboard, probabilmente avevi diverse fonti di dati (Database, API esterna, Cache). Questi NON sono una famiglia (non hanno logica condivisa), ma hanno la stessa **capacità**: fornire dati. Quindi:

```csharp
public interface IDataSource
{
    Task<KpiData> GetDataAsync(DateTime from, DateTime to);
}

public class DatabaseDataSource : IDataSource { /* ... */ }
public class ApiDataSource : IDataSource { /* ... */ }
public class CachedDataSource : IDataSource { /* ... */ }

// Il Dashboard non sa (e non deve sapere) da dove vengono i dati
public class KpiDashboard
{
    private readonly IDataSource _dataSource;
    
    public KpiDashboard(IDataSource dataSource) // Dependency Injection
    {
        _dataSource = dataSource;
    }
}
```

**Nel colloquio - Risposta killer**:

"Uso Abstract Class quando ho una gerarchia con logica condivisa e forte relazione IS-A. Uso Interface per definire contratti e capacità, specialmente per Dependency Injection e testing. In pratica, tendo a preferire Interface per accoppiamento debole, e Abstract Class solo quando c'è veramente codice comune da condividere."

---

### Sealed - Il Controllo Finale

**Concetto**: `sealed` blocca l'ereditarietà. È l'opposto di `virtual` e `abstract`.

**Quando e perché**:

```csharp
public sealed class SecurityService
{
    // Nessuno può ereditare e sovrascrivere la logica di sicurezza
}
```

**Motivazioni**:

1. **Sicurezza**: Classi critiche (crittografia, autenticazione) non devono essere alterate
2. **Performance**: Il compilatore può fare ottimizzazioni (no virtual dispatch)
3. **Design intenzionale**: "Questa classe è completa, non estenderla"

**Sealed su metodi**:

```csharp
public class Base
{
    public virtual void Process() { }
}

public class Middle : Base
{
    public sealed override void Process() 
    { 
        // Questo override è FINALE
    }
}

public class Final : Middle
{
    // ERRORE: non puoi override un metodo sealed
    // public override void Process() { }
}
```

**Nel mondo reale**: Le classi del framework .NET come `String` sono sealed. Microsoft non vuole che tu crei `MyCustomString : String` e introduca comportamenti imprevedibili.

---

## 2. Sistema di Tipi in C# - Decisioni Architetturali {#tipi-csharp}

### Class vs Struct - La Decisione Fondamentale

Questa non è solo una differenza tecnica (reference vs value type). È una **decisione di design** che impatta performance, semantica e comportamento.

**La domanda chiave**: "Questo concetto rappresenta un'**entità** o un **valore**?"

#### **Struct (Value Type) - Semantica di Valore**

Un `struct` rappresenta un **valore immutabile e autonomo**, come un numero o una data.

**Caratteristiche**:

- Vive nello **Stack** (o inline in un oggetto)
- **Copiato** quando assegnato
- No overhead di Garbage Collection
- No inheritance (escluso da `object`)
- Meglio per dati **piccoli** (<16 bytes come guideline)

**Esempio perfetto - Point**:

```csharp
public struct Point
{
    public int X { get; }
    public int Y { get; }
    
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}

// Comportamento
var p1 = new Point(10, 20);
var p2 = p1; // COPIA - p2 è indipendente
p2 = new Point(30, 40);
Console.WriteLine(p1.X); // 10 - p1 non è cambiato
```

**Perché è giusto**:

Un punto (10, 20) **è** un valore, come il numero 42. Non ha "identità". Due punti (10, 20) sono **equivalenti**, non "lo stesso oggetto".

**Quando usare Struct**:

1. Dati piccoli e semplici
2. Semantica di valore (equality by value, non by reference)
3. Immutabilità (readonly struct in C# 7.2+)
4. Performance critica (no GC pressure)
5. Esempi: `DateTime`, `Decimal`, `Guid`, coordinate, colori RGB

#### **Class (Reference Type) - Semantica di Entità**

Una `class` rappresenta un'**entità** con identità e stato mutabile.

**Caratteristiche**:

- Vive nello **Heap**
- **Reference** quando assegnata (due variabili possono puntare allo stesso oggetto)
- Gestita dal Garbage Collector
- Supporta ereditarietà
- Può essere `null`

**Esempio - Customer**:

```csharp
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Order> Orders { get; set; }
}

// Comportamento
var customer1 = new Customer { Id = 1, Name = "John" };
var customer2 = customer1; // REFERENCE - stesso oggetto
customer2.Name = "Jane";
Console.WriteLine(customer1.Name); // "Jane" - stesso oggetto!
```

**Perché è giusto**:

Un customer **ha identità**. Customer #1 è diverso da Customer #2 anche se hanno lo stesso nome. Modificare `customer2` deve riflettere su `customer1` perché sono **la stessa persona**.

**Quando usare Class**:

1. Entità con identità
2. Dati grandi o complessi
3. Necessità di ereditarietà
4. Stato mutabile nel tempo
5. Esempi: `User`, `Order`, `Product`, servizi, controller

**La trappola comune**:

Molti sviluppatori junior pensano: "Struct è più veloce, uso sempre struct!"

**Sbagliato**. Struct è più veloce per dati piccoli, ma:

- Struct grandi rallentano (copia costosa)
- Struct mutabili creano bug sottili (copi senza volerlo)
- Struct con reference types dentro (List, String) perdono vantaggi

**Regola pratica**:

- Struct: <16 bytes, immutabile, semantica di valore
- Class: tutto il resto

---

### Record (C# 9+) - Value Semantics per Reference Types

**Il problema che risolve**:

Spesso vuoi:

- Reference type (per flessibilità)
- Value semantics (equality by value)
- Immutabilità (thread-safe, prevedibile)

I `record` danno esattamente questo.

**Esempio - DTO**:

```csharp
// Vecchio modo
public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    
    // Devi implementare manualmente
    public override bool Equals(object obj) { /* ... */ }
    public override int GetHashCode() { /* ... */ }
}

// Nuovo modo - Record
public record UserDto(int Id, string Name, string Email);

// Automaticamente hai:
var dto1 = new UserDto(1, "John", "john@test.com");
var dto2 = new UserDto(1, "John", "john@test.com");
Console.WriteLine(dto1 == dto2); // TRUE - value equality!

// Immutabilità con "with" expression
var dto3 = dto1 with { Name = "Jane" }; // Crea nuovo record
Console.WriteLine(dto1.Name); // "John" - originale immutato
```

**Quando usare Record**:

1. **DTO** (Data Transfer Objects) - perfetti!
2. **Value Objects** in Domain-Driven Design
3. Dati immutabili
4. Quando vuoi value equality senza boilerplate

**Nel tuo lavoro**:

Nel Template Manager, i DTO per i template PDF sarebbero perfetti come record:

```csharp
public record TemplateDto(int Id, string Name, List<PlaceholderDto> Placeholders);
public record PlaceholderDto(string Key, string DefaultValue, string Type);
```

---

### Readonly vs Const - Immutabilità a Livelli Diversi

Questa distinzione sembra piccola ma è **fondamentale** per capire come funziona C#.

#### **Const - Compile-Time Constant**

```csharp
public class Configuration
{
    public const int MaxRetries = 3;
    public const string ApiVersion = "v1";
}
```

**Cosa succede dietro le quinte**:

Quando compili, il compilatore **sostituisce** ogni uso di `MaxRetries` con il valore `3`. È come se avessi scritto direttamente `3` nel codice.

**Implicazioni**:

1. **Performance massima**: nessun lookup a runtime
2. **Pericolo di versioning**: Se Library A usa `const` da Library B, e Library B cambia il valore, Library A deve essere **ricompilata**
3. **Solo tipi primitivi**: string, int, bool, ecc.
4. **Implicitamente static**

#### **Readonly - Runtime Constant**

```csharp
public class Service
{
    private readonly DateTime _startTime;
    private readonly IConfiguration _config;
    
    public Service(IConfiguration config)
    {
        _startTime = DateTime.Now; // Valore runtime
        _config = config;          // Reference
    }
    
    public void DoWork()
    {
        // _startTime = DateTime.Now; // ERRORE - readonly
        // _config = null;             // ERRORE - readonly
        
        // Ma puoi modificare l'oggetto referenziato!
        var setting = _config["MySetting"];
    }
}
```

**Caratteristiche**:

1. Valore assegnato **a runtime** (costruttore)
2. Può essere qualsiasi tipo (anche reference types)
3. Può essere diverso per ogni istanza
4. **Attenzione**: readonly su reference type blocca la reference, non l'oggetto

**Quando usare cosa**:

- **Const**: Valori veramente costanti (matematica, costanti fisiche, version strings)
- **Readonly**: Dipendenze iniettate, configurazione, valori che cambiano tra ambienti

**Nel colloquio**:

"Const è compile-time e il valore è embedded nel codice. Readonly è runtime e può essere inizializzato nel costruttore. Per Dependency Injection uso sempre readonly perché le dipendenze sono iniettate a runtime."

---

### Boxing e Unboxing - Il Costo Nascosto

**Il problema**:

C# è un linguaggio che unifica value types e reference types sotto `object`. Ma questa unificazione ha un costo.

**Cosa succede**:

```csharp
int number = 42;              // Value type - Stack
object boxed = number;        // Boxing - copia nello Heap!
int unboxed = (int)boxed;     // Unboxing - copia dallo Heap!
```

**Dietro le quinte**:

1. **Boxing**: 
    - Alloca memoria nello Heap
    - Crea un wrapper object
    - Copia il valore dentro
    - Ritorna reference
2. **Unboxing**:
    - Verifica il tipo (runtime check)
    - Estrae il valore
    - Copia

**Perché è male**:

1. **Allocazione nello Heap** → Garbage Collector pressure
2. **Copia** → Lento per struct grandi
3. **Type check** a runtime → No type safety a compile-time

**Quando succede** (spesso senza che tu lo sappia):

```csharp
// Esempio 1 - Collections non-generic
ArrayList list = new ArrayList();
list.Add(42);        // Boxing!
int value = (int)list[0]; // Unboxing!

// Esempio 2 - String interpolation (prima di C# 10)
Console.WriteLine($"Number: {42}"); // Potenziale boxing

// Esempio 3 - Interface implementation
interface IComparable
{
    int CompareTo(object obj); // object parameter!
}

struct Point : IComparable
{
    public int X, Y;
    public int CompareTo(object obj) // obj è boxed Point!
    {
        Point other = (Point)obj; // Unboxing
        // ...
    }
}
```

**La soluzione - Generics**:

```csharp
// NO BOXING!
List<int> numbers = new List<int>();
numbers.Add(42);           // No boxing
int value = numbers[0];    // No unboxing

// Generic interface
interface IComparable<T>
{
    int CompareTo(T other); // T parameter - no boxing!
}

struct Point : IComparable<Point>
{
    public int X, Y;
    public int CompareTo(Point other) // No boxing!
    {
        // Confronto diretto
    }
}
```

**Nel mondo reale**:

In sistemi high-performance (gaming, real-time processing), il boxing può causare:

- GC pause (stuttering)
- Allocazioni eccessive
- Performance degradation

**Nel colloquio**:

"Boxing è la conversione implicita di value type a object, creando allocazione nello Heap. Lo evito usando generics invece di collections non-generic, e prestando attenzione a interface con parametri object. In applicazioni web normali non è critico, ma in high-performance code può causare GC pressure significativo."

---

## 3. Gestione della Memoria - Come Funziona Davvero {#memoria}

### Stack vs Heap - Architettura della Memoria

Questa è **fondamentale**. Non è solo teoria - impatta performance, concorrenza, e design.

#### **Lo Stack - LIFO e Thread-Local**

**Caratteristiche**:

- **LIFO** (Last In, First Out) - struttura a pila
- **Thread-local** - ogni thread ha il suo stack
- **Velocissimo** - solo incremento/decremento puntatore
- **Deterministico** - cleanup automatico quando esci da scope
- **Limitato** - ~1MB per thread (stack overflow se eccedi)

**Cosa ci va**:

- Variabili locali (value types)
- Parametri metodi
- Return addresses (chiamate metodi)
- Reference a oggetti Heap

**Esempio**:

```csharp
public void ProcessData()
{
    int count = 0;           // Stack
    DateTime now = DateTime.Now; // Stack (DateTime è struct)
    User user = new User();  // user (reference) → Stack
                             // oggetto User → Heap
    
    DoWork(count);
} // count, now, user reference: automaticamente rimossi!

private void DoWork(int value) // value copiato nello Stack
{
    int result = value * 2;  // Stack
} // result rimosso
```

**Perché è veloce**:

Lo stack cresce e decresce semplicemente muovendo un puntatore. Nessuna ricerca, nessuna frammentazione, nessun Garbage Collector.

#### **L'Heap - Memoria Gestita dal GC**

**Caratteristiche**:

- **Condiviso** tra tutti i thread
- **Gestito** dal Garbage Collector
- **Flessibile** - oggetti di qualsiasi dimensione
- **Più lento** - allocazione complessa, GC overhead
- **Non deterministico** - non sai quando un oggetto viene deallocato

**Cosa ci va**:

- Tutti i reference types (class instances)
- Array e stringhe
- Boxed value types

**Vita di un oggetto**:

```csharp
public void CreateUser()
{
    var user = new User { Name = "John" };
    // 1. new User alloca memoria nello Heap
    // 2. user (reference) salvata nello Stack
    // 3. L'oggetto vive nello Heap
    
    ProcessUser(user);
    // 4. user esce da scope, reference rimossa dallo Stack
    // 5. Oggetto Heap NON ancora deallocato
    // 6. GC dealloca quando decide (Gen 0 collection)
}
```

**Reference multiple**:

```csharp
var user1 = new User { Name = "John" };
var user2 = user1; // Stessa reference!
user2.Name = "Jane";
Console.WriteLine(user1.Name); // "Jane" - stesso oggetto Heap
```

---

### Garbage Collector - Il Gestore Automatico

**La sfida**:

In linguaggi come C/C++, tu devi manualmente `free()` la memoria. Dimentichi? Memory leak. Liberi due volte? Crash.

Il Garbage Collector (GC) di .NET **automatizza** questo, ma devi capire come funziona per scrivere codice efficiente.

#### **Generational GC - L'Intuizione Brillante**

**Osservazione empirica**:

La maggior parte degli oggetti muore giovane. Pochi oggetti vivono a lungo.

**Soluzione**:

Dividi gli oggetti in **generazioni** e scansiona frequentemente i giovani, raramente i vecchi.

**Gen 0 - Nursery**:

- Oggetti appena creati
- Raccolti **molto frequentemente**
- **Piccoli e veloci** (pochi millisecondi)
- ~90% degli oggetti muore qui

**Gen 1 - Middle Age**:

- Oggetti sopravvissuti a Gen 0
- Buffer tra Gen 0 e Gen 2
- Raccolti occasionalmente

**Gen 2 - Old Age**:

- Oggetti long-lived
- Raccolti **raramente**
- **Lenti e costosi** (possono bloccare applicazione)
- Esempi: singleton, cache, static data

**Large Object Heap (LOH)**:

- Oggetti >85KB
- **Non compattato** (per performance)
- Rischio di frammentazione
- Esempi: grandi array, immagini, buffer

**Esempio di vita di un oggetto**:

```csharp
public void ProcessRequest()
{
    // 1. DTO creato - Gen 0
    var dto = new RequestDto { /* ... */ };
    
    // 2. Processiamo
    var result = _service.Process(dto);
    
    // 3. Ritorniamo
    return result;
    
    // 4. dto esce da scope
    // 5. GC Gen 0 collection (dopo pochi ms)
    // 6. dto raccolto - mai promosso a Gen 1
}

public class CacheService
{
    // Oggetto long-lived - finirà in Gen 2
    private static Dictionary<string, Data> _cache = new();
}
```

**Perché è importante**:

Se crei troppi oggetti long-lived:

- Finiscono in Gen 2
- Gen 2 collection è **lenta**
- Applicazione rallenta (pause visibili)

**Best practices**:

1. **Oggetti short-lived**: Perfetti (muoiono in Gen 0)
2. **Oggetti long-lived**: OK se pochi (singleton, cache)
3. **Oggetti medium-lived**: **MALE** - promossi continuamente, GC pressure

**Nel tuo lavoro**:

Nel KPI Dashboard, i dati caricati per visualizzazione sono perfetti esempi di short-lived objects. Caricati, processati, mostrati, raccolti in Gen 0.

---

### IDisposable Pattern - Gestione Risorse Unmanaged

**Il problema**:

Il GC gestisce memoria managed (.NET objects), ma NON sa gestire:

- File handles
- Database connections
- Network sockets
- Window handles
- Unmanaged memory

Queste risorse devono essere **rilasciate esplicitamente**.

**La soluzione - IDisposable**:

```csharp
public interface IDisposable
{
    void Dispose();
}
```

**Pattern corretto (completo)**:

```csharp
public class DatabaseConnection : IDisposable
{
    private SqlConnection _connection;
    private bool _disposed = false;
    
    public DatabaseConnection(string connectionString)
    {
        _connection = new SqlConnection(connectionString);
    }
    
    public void ExecuteQuery(string sql)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(DatabaseConnection));
        
        // Usa _connection
    }
    
    // Public Dispose
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this); // Non chiamare finalizer
    }
    
    // Protected Dispose(bool)
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        
        if (disposing)
        {
            // Rilascia managed resources
            _connection?.Dispose();
        }
        
        // Rilascia unmanaged resources (se ce ne sono)
        
        _disposed = true;
    }
    
    // Finalizer (solo se hai unmanaged resources)
    ~DatabaseConnection()
    {
        Dispose(false);
    }
}
```

**Perché questo pattern complesso**:

1. `Dispose()` pubblico: chiamato manualmente
2. `Dispose(bool)` protetto: riutilizzabile in classi derivate
3. Finalizer: safety net se Dispose non viene chiamato
4. `GC.SuppressFinalize`: ottimizzazione (se Dispose è chiamato, finalizer inutile)

**Uso con using**:

```csharp
// Vecchio C#
using (var connection = new DatabaseConnection(connString))
{
    connection.ExecuteQuery("SELECT ...");
} // Dispose automatico, anche se exception!

// C# 8+
using var connection = new DatabaseConnection(connString);
connection.ExecuteQuery("SELECT ...");
// Dispose automatico alla fine dello scope
```

**Cosa fa using**:

È syntactic sugar per:

```csharp
var connection = new DatabaseConnection(connString);
try
{
    connection.ExecuteQuery("SELECT ...");
}
finally
{
    connection?.Dispose(); // SEMPRE chiamato
}
```

**Nel mondo reale**:

Quasi tutte le classi I/O del framework implementano IDisposable:

- `FileStream`
- `SqlConnection`
- `HttpClient` (anche se è speciale)
- `StreamReader`/`StreamWriter`

**Errore comune**:

```csharp
// MALE - connection non rilasciata se exception
var connection = new SqlConnection(connString);
connection.Open();
var result = connection.ExecuteQuery("SELECT ...");
connection.Dispose();

// BENE
using var connection = new SqlConnection(connString);
connection.Open();
var result = connection.ExecuteQuery("SELECT ...");
// Dispose garantito
```

**Nel colloquio**:

## 4. LINQ - Il Potere della Composizione {#linq-potere}

### La Filosofia di LINQ - Perché Esiste

LINQ (Language Integrated Query) non è solo "un modo per interrogare collezioni". È una **rivoluzione nel modo di pensare alla manipolazione dei dati**.

**Il problema che risolve**:

Prima di LINQ (C# 2.0 e precedenti), per filtrare e trasformare dati dovevi scrivere:

```csharp
// C# 1.0 - Imperativo, verboso, error-prone
List<User> adults = new List<User>();
foreach (User user in users)
{
    if (user.Age >= 18)
    {
        adults.Add(user);
    }
}

List<string> names = new List<string>();
foreach (User user in adults)
{
    names.Add(user.Name);
}

names.Sort();
```

**Problemi**:

1. Molto codice per operazioni semplici
2. Variabili temporanee (`adults`, `names`)
3. Logica mescolata (cosa fare + come farlo)
4. Difficile da comporre
5. Errori facili (dimenticare Add, ordinare la lista sbagliata)

**Con LINQ - Dichiarativo, Componibile, Espressivo**:

```csharp
var names = users
    .Where(u => u.Age >= 18)
    .Select(u => u.Name)
    .OrderBy(n => n)
    .ToList();
```

**Cosa hai guadagnato**:

1. **Chiarezza**: Leggi cosa fa, non come lo fa
2. **Composizione**: Concateni operazioni
3. **Immutabilità**: Non modifichi collezioni intermedie
4. **Riutilizzo**: Le lambda sono riutilizzabili
5. **Type safety**: Tutto verificato a compile-time

---

### Deferred Execution - Il Concetto Fondamentale

Questa è la caratteristica **più importante** e **più fraintesa** di LINQ.

**Concetto**: Le query LINQ **non eseguono immediatamente**. Creano un "piano di esecuzione" che viene valutato solo quando enumeri i risultati.

**Esempio**:

```csharp
var users = GetUsers(); // 1000 utenti dal DB

var query = users.Where(u => u.Age >= 18); // NON eseguito ancora!

Console.WriteLine("Query creata"); // Nessun filtraggio fatto

foreach (var user in query) // ORA esegue!
{
    Console.WriteLine(user.Name);
}
```

**Cosa succede dietro le quinte**:

1. `Where()` ritorna un `IEnumerable<User>` che **rappresenta** la query
2. Non itera ancora su `users`
3. Non alloca memoria per risultati
4. Quando fai `foreach`, **solo allora** inizia a iterare e filtrare

**Implicazioni pratiche**:

#### **1. La query "cattura" lo stato al momento dell'esecuzione**

```csharp
int minAge = 18;
var query = users.Where(u => u.Age >= minAge);

minAge = 21; // Cambiato!

foreach (var user in query) // Usa minAge = 21!
{
    // Solo utenti >= 21
}
```

#### **2. Multiple enumerazioni = Multiple esecuzioni**

```csharp
var expensiveQuery = users
    .Where(u => ExpensiveValidation(u)) // Chiamata costosa!
    .Select(u => Transform(u));         // Trasformazione pesante!

// Prima enumerazione
var count = expensiveQuery.Count(); // Esegue tutto!

// Seconda enumerazione
var list = expensiveQuery.ToList(); // Esegue ANCORA tutto!

// SOLUZIONE - Materializza una volta
var list = expensiveQuery.ToList();
var count = list.Count; // Usa la lista, no ri-esecuzione
```

**Questo è un bug comune in produzione!** Potresti accidentalmente interrogare il database 10 volte quando pensavi di farlo una volta.

#### **3. Operators che forzano esecuzione immediata**

**Deferred** (lazy):

- `Where`, `Select`, `SelectMany`
- `OrderBy`, `ThenBy`
- `Skip`, `Take`
- `Distinct`, `Union`

**Immediate** (eager):

- `ToList`, `ToArray`, `ToDictionary`
- `Count`, `Sum`, `Average`, `Max`, `Min`
- `First`, `Single`, `Last`
- `Any`, `All`

**Strategia**:

```csharp
// Costruisci query deferred
var query = users
    .Where(u => u.IsActive)
    .OrderBy(u => u.Name)
    .Select(u => new UserDto { Name = u.Name });

// Materializza UNA VOLTA quando serve
var result = query.ToList();

// Ora puoi usare result multiple volte senza ri-eseguire
var count = result.Count;
var first = result.First();
```

---

### IEnumerable vs IQueryable - La Differenza Cruciale

Questa distinzione è **fondamentale** per le performance delle applicazioni database.

#### **IEnumerable<T> - LINQ to Objects (In-Memory)**

**Cosa significa**:

- Opera su collezioni **già in memoria**
- Esecuzione tramite **delegates/lambda**
- Filtraggio/trasformazione **in C#**

**Esempio**:

```csharp
List<User> users = context.Users.ToList(); // Carica TUTTO in memoria

IEnumerable<User> adults = users.Where(u => u.Age >= 18); // Filtra in memoria

foreach (var user in adults) // Itera in memoria
{
    // ...
}
```

**Cosa succede**:

1. `ToList()` esegue `SELECT * FROM Users` → 10.000 righe caricate
2. `Where` itera in memoria e filtra → 5.000 adulti
3. Hai trasferito 10.000 righe per usarne 5.000

#### **IQueryable<T> - LINQ to SQL/EF (Database)**

**Cosa significa**:

- Opera su **sorgenti remote** (database)
- Esecuzione tramite **Expression Trees**
- Query tradotta in **SQL**

**Esempio**:

```csharp
IQueryable<User> query = context.Users; // Nessuna query ancora

var adults = query.Where(u => u.Age >= 18); // Ancora no query

var list = adults.ToList(); // ORA esegue: SELECT * FROM Users WHERE Age >= 18
```

**Cosa succede**:

1. `Where` costruisce un **Expression Tree**: `u => u.Age >= 18`
2. `ToList()` traduce in SQL: `WHERE Age >= 18`
3. Database filtra → Solo 5.000 righe trasferite

**La differenza è ENORME**:

- IEnumerable: 10.000 righe via rete
- IQueryable: 5.000 righe via rete

In sistemi con milioni di record, questa è la differenza tra **5 secondi e 0.1 secondi**.

#### **Expression Trees - Il Segreto di IQueryable**

**Concetto**: Le lambda in IQueryable non sono "codice da eseguire", ma **alberi di espressioni** che rappresentano il codice.

```csharp
// IEnumerable - Delegate (codice compilato)
Func<User, bool> predicate = u => u.Age >= 18;
var adults = users.Where(predicate); // Esegue il delegate su ogni elemento

// IQueryable - Expression Tree (rappresentazione del codice)
Expression<Func<User, bool>> expression = u => u.Age >= 18;
var adults = query.Where(expression); // Traduce in SQL
```

**L'Expression Tree è come un AST** (Abstract Syntax Tree):

```
BinaryExpression (>=)
├─ MemberAccess (u.Age)
└─ Constant (18)
```

EF Core "legge" questo albero e genera SQL.

#### **Quando IQueryable diventa IEnumerable (e perdi performance)**

```csharp
var users = context.Users // IQueryable
    .Where(u => u.Age >= 18) // Ancora IQueryable - WHERE in SQL
    .ToList()                // Materializza - diventa IEnumerable
    .Where(u => u.Name.StartsWith("J")); // IEnumerable - filtra in memoria!

// SQL eseguito: SELECT * FROM Users WHERE Age >= 18
// Poi filtra in C# per Nome
```

**Corretto**:

```csharp
var users = context.Users // IQueryable
    .Where(u => u.Age >= 18)              // IQueryable
    .Where(u => u.Name.StartsWith("J"))   // IQueryable
    .ToList();                            // Materializza alla fine

// SQL eseguito: SELECT * FROM Users WHERE Age >= 18 AND Name LIKE 'J%'
```

**Nel tuo KPI Dashboard**:

Quando hai ottimizzato le performance da 8s a 1.5s, probabilmente hai:

1. Rimosso `ToList()` prematuri
2. Spostato i filtri prima della materializzazione
3. Usato proiezioni (`Select`) per caricare solo campi necessari

---

### First vs FirstOrDefault vs Single vs SingleOrDefault - Semantica Precisa

Questi operatori sembrano simili ma hanno **semantica diversa** che riflette **intenzione diversa**.

#### **First() - "Dammi il primo, so che esiste"**

```csharp
var admin = users.First(u => u.Role == "Admin");
```

**Comportamento**:

- Ritorna il primo elemento che matcha
- **Exception** se nessun elemento
- **Uso**: Quando sei **certo** che esiste almeno uno

**Quando usarlo**:

```csharp
// OK - Sai che ogni utente ha almeno un ordine
var latestOrder = user.Orders.OrderByDescending(o => o.Date).First();

// OK - Configurazione che deve esistere
var setting = config.First(c => c.Key == "ApiUrl");
```

#### **FirstOrDefault() - "Dammi il primo, se esiste"**

```csharp
var admin = users.FirstOrDefault(u => u.Role == "Admin");
if (admin != null) // Controlla!
{
    // Usa admin
}
```

**Comportamento**:

- Ritorna il primo elemento o `null`/`default(T)`
- **No exception**
- **Uso**: Quando **potrebbe non esserci**

**Quando usarlo**:

```csharp
// OK - L'utente potrebbe non avere ordini
var latestOrder = user.Orders.OrderByDescending(o => o.Date).FirstOrDefault();

// OK - Il setting potrebbe non essere configurato
var setting = config.FirstOrDefault(c => c.Key == "OptionalFeature");
```

#### **Single() - "Dammi l'unico, deve essere esattamente uno"**

```csharp
var user = users.Single(u => u.Id == userId);
```

**Comportamento**:

- Ritorna l'elemento se **esattamente uno** matcha
- **Exception** se zero elementi
- **Exception** se più di un elemento
- **Uso**: Quando deve essere **esattamente uno** (unicità)

**Quando usarlo**:

```csharp
// OK - ID è unique
var user = context.Users.Single(u => u.Id == 123);

// OK - Email è unique
var user = context.Users.Single(u => u.Email == "test@test.com");

// MALE - Potrebbero esserci più admin!
var admin = users.Single(u => u.Role == "Admin"); // Exception se >1
```

#### **SingleOrDefault() - "Dammi l'unico, se esiste, deve essere al massimo uno"**

```csharp
var user = users.SingleOrDefault(u => u.Email == email);
```

**Comportamento**:

- Ritorna l'elemento se uno solo
- Ritorna `null` se zero
- **Exception** se più di uno
- **Uso**: Unicità opzionale

**Quando usarlo**:

```csharp
// OK - Cerco per unique key che potrebbe non esistere
var user = context.Users.SingleOrDefault(u => u.Email == email);

// OK - Relazione 1-to-0..1
var profile = user.Profile.SingleOrDefault();
```

#### **La scelta comunica intenzione**

**Domanda colloquio**: "Differenza tra First e Single?"

**Risposta SCARSA**: "First prende il primo, Single verifica che sia uno solo"

**Risposta OTTIMA**: "First e Single comunicano intenzione diversa. First dice 'dammi il primo di potenzialmente molti', Single dice 'deve esistere esattamente uno, è una constraint di unicità'. Se uso Single su una lista che potrebbe avere duplicati, voglio che l'applicazione fallisca perché indica un bug nei dati. First è per ordinamenti, Single è per unique keys."

**Nel tuo codice**:

```csharp
// License Management System
var license = context.Licenses.Single(l => l.Key == licenseKey); // Deve essere unique!

// Template Manager
var template = context.Templates.First(t => t.IsActive); // Primo attivo
```

---

### Any() vs Count() > 0 - Performance e Semantica

**Scenario**: Vuoi sapere se ci sono elementi.

```csharp
// Opzione 1
if (users.Count() > 0) { }

// Opzione 2
if (users.Any()) { }
```

**Quale usi?** `Any()` - sempre.

**Perché**:

#### **Count() - Conta TUTTI gli elementi**

```csharp
var count = users.Count();
// IEnumerable: Itera tutti gli elementi (O(n))
// IQueryable: SELECT COUNT(*) FROM Users
```

Se hai 1.000.000 di utenti e vuoi solo sapere se la tabella è vuota, `Count()` è **spreco enorme**.

#### **Any() - Si ferma al primo**

```csharp
var hasUsers = users.Any();
// IEnumerable: Si ferma al primo elemento (O(1) best case)
// IQueryable: SELECT TOP 1 ... (o equivalente)
```

**Differenza**:

- `Count() > 0`: Conta 1.000.000 di record
- `Any()`: Trova 1 record e si ferma

**Con filtro**:

```csharp
// MALE - Conta tutti gli adulti
if (users.Count(u => u.Age >= 18) > 0)

// BENE - Si ferma al primo adulto
if (users.Any(u => u.Age >= 18))
```

**SQL generato**:

```sql
-- Count()
SELECT COUNT(*) FROM Users WHERE Age >= 18 -- Scansiona tutti

-- Any()
SELECT CASE WHEN EXISTS(SELECT 1 FROM Users WHERE Age >= 18) 
       THEN 1 ELSE 0 END
-- O semplicemente
SELECT TOP 1 1 FROM Users WHERE Age >= 18
```

**Eccezione - List<T>.Count**:

```csharp
List<User> users = GetUsers();

if (users.Count > 0) // OK - O(1), accede a proprietà
if (users.Any())      // OK - ma meno idiomatico
```

Se è già una `List`, `Count` è una proprietà O(1). Ma `Any()` è ancora più chiaro semanticamente.

**Regola**: **Sempre `Any()` per esistenza, `Count()` solo se serve il numero.**

---

### SelectMany - Flatten e Proiezione

`SelectMany` è uno degli operatori **più potenti e più fraintesi** di LINQ.

**Il problema che risolve**: Hai collezioni di collezioni e vuoi "appiattirle".

#### **Scenario 1 - Flatten semplice**

```csharp
class Customer
{
    public List<Order> Orders { get; set; }
}

List<Customer> customers = GetCustomers();

// Vuoi TUTTI gli ordini di TUTTI i clienti

// MALE - Nested loops
var allOrders = new List<Order>();
foreach (var customer in customers)
{
    foreach (var order in customer.Orders)
    {
        allOrders.Add(order);
    }
}

// BENE - SelectMany
var allOrders = customers.SelectMany(c => c.Orders).ToList();
```

**Cosa fa SelectMany**:

1. Per ogni `customer`, prende `customer.Orders` (che è `List<Order>`)
2. "Appiattisce" tutte le liste in una singola sequenza

**Visualizzazione**:

```
Customers:
  Customer 1 → [Order A, Order B]
  Customer 2 → [Order C]
  Customer 3 → [Order D, Order E, Order F]

SelectMany:
  [Order A, Order B, Order C, Order D, Order E, Order F]
```

#### **Scenario 2 - Flatten con proiezione**

```csharp
// Vuoi tutti gli ordini con info cliente
var ordersWithCustomer = customers.SelectMany(
    customer => customer.Orders,
    (customer, order) => new
    {
        CustomerName = customer.Name,
        OrderId = order.Id,
        OrderTotal = order.Total
    }
);
```

**Questo è come un JOIN** in SQL.

#### **Scenario 3 - Combinazioni (Cartesian Product)**

```csharp
var sizes = new[] { "S", "M", "L" };
var colors = new[] { "Red", "Blue" };

// Tutte le combinazioni
var variants = sizes.SelectMany(
    size => colors,
    (size, color) => $"{size}-{color}"
);

// Risultato: ["S-Red", "S-Blue", "M-Red", "M-Blue", "L-Red", "L-Blue"]
```

#### **Nel mondo reale - Il tuo Template Manager**

```csharp
// Hai template con placeholder
class Template
{
    public List<Placeholder> Placeholders { get; set; }
}

// Vuoi tutti i placeholder di tutti i template attivi
var allPlaceholders = context.Templates
    .Where(t => t.IsActive)
    .SelectMany(t => t.Placeholders)
    .Distinct()
    .ToList();

// SQL:
// SELECT DISTINCT p.*
// FROM Templates t
// INNER JOIN Placeholders p ON t.Id = p.TemplateId
// WHERE t.IsActive = 1
```

**Differenza con Select**:

```csharp
// Select - Proiezione 1-to-1
var names = customers.Select(c => c.Name); // List<string>

// SelectMany - Proiezione 1-to-many
var orders = customers.SelectMany(c => c.Orders); // List<Order> (flatten)
```

---

### GroupBy - Aggregazione e Raggruppamento

`GroupBy` trasforma una sequenza flat in una **sequenza di gruppi**.

**Concetto**: Raggruppa elementi che condividono una caratteristica (key).

#### **Esempio base**

```csharp
var orders = GetOrders();

// Raggruppa per CustomerID
var grouped = orders.GroupBy(o => o.CustomerId);

// Tipo: IEnumerable<IGrouping<int, Order>>
// Ogni gruppo ha Key e collezione di elementi

foreach (var group in grouped)
{
    Console.WriteLine($"Customer {group.Key}:");
    foreach (var order in group)
    {
        Console.WriteLine($"  Order {order.Id}: {order.Total}");
    }
}
```

**Output**:

```
Customer 1:
  Order 101: 50.00
  Order 102: 30.00
Customer 2:
  Order 201: 100.00
```

#### **Con aggregazione**

```csharp
// Totale ordini per cliente
var customerTotals = orders
    .GroupBy(o => o.CustomerId)
    .Select(g => new
    {
        CustomerId = g.Key,
        OrderCount = g.Count(),
        TotalAmount = g.Sum(o => o.Total),
        AverageOrder = g.Average(o => o.Total)
    });

// SQL equivalente:
// SELECT CustomerId, 
//        COUNT(*) as OrderCount,
//        SUM(Total) as TotalAmount,
//        AVG(Total) as AverageOrder
// FROM Orders
// GROUP BY CustomerId
```

#### **GroupBy multipli (composite key)**

```csharp
// Raggruppa per Anno e Mese
var monthlyOrders = orders.GroupBy(o => new 
{ 
    Year = o.Date.Year, 
    Month = o.Date.Month 
});

foreach (var group in monthlyOrders)
{
    Console.WriteLine($"{group.Key.Year}-{group.Key.Month}: {group.Count()} orders");
}
```

#### **Nel tuo KPI Dashboard**

```csharp
// KPI per reparto e mese
var departmentKpis = metrics
    .GroupBy(m => new { m.DepartmentId, m.Month })
    .Select(g => new KpiResult
    {
        Department = g.Key.DepartmentId,
        Month = g.Key.Month,
        TotalSales = g.Sum(m => m.Sales),
        AverageEfficiency = g.Average(m => m.Efficiency),
        TopPerformer = g.OrderByDescending(m => m.Performance)
                        .First()
                        .EmployeeName
    });
```

---

### Il Problema N+1 - Il Killer delle Performance

Questo è **il bug di performance più comune** in applicazioni che usano ORM.

**Scenario**:

```csharp
// Carica clienti
var customers = context.Customers.ToList(); // 1 query

foreach (var customer in customers) // 100 clienti
{
    Console.WriteLine($"{customer.Name}:");
    
    // Per ogni cliente, carica gli ordini
    var orders = customer.Orders.ToList(); // 100 query! (N queries)
    
    foreach (var order in orders)
    {
        Console.WriteLine($"  {order.Total}");
    }
}

// Totale: 1 + 100 = 101 query!
```

**Cosa succede**:

```sql
-- Query 1
SELECT * FROM Customers

-- Query 2
SELECT * FROM Orders WHERE CustomerId = 1

-- Query 3
SELECT * FROM Orders WHERE CustomerId = 2

-- ...

-- Query 101
SELECT * FROM Orders WHERE CustomerId = 100
```

**Perché è disastroso**:

- 101 round-trips al database
- Network latency: 10ms × 101 = 1 secondo sprecato
- Database load inutile
- In produzione con 1000 clienti: **1001 query**

#### **Soluzione 1 - Eager Loading (Include)**

```csharp
var customers = context.Customers
    .Include(c => c.Orders) // JOIN!
    .ToList();

// 1 SOLA query!
// SELECT c.*, o.*
// FROM Customers c
// LEFT JOIN Orders o ON c.Id = o.CustomerId

foreach (var customer in customers)
{
    Console.WriteLine($"{customer.Name}:");
    
    // Orders già caricati - no query!
    foreach (var order in customer.Orders)
    {
        Console.WriteLine($"  {order.Total}");
    }
}
```

#### **Soluzione 2 - Projection (Select)**

```csharp
// Carica SOLO i dati che servono
var customerData = context.Customers
    .Select(c => new
    {
        c.Name,
        OrderCount = c.Orders.Count(),
        TotalSpent = c.Orders.Sum(o => o.Total)
    })
    .ToList();

// 1 query con aggregazioni
// SELECT c.Name, 
//        COUNT(o.Id) as OrderCount,
//        SUM(o.Total) as TotalSpent
// FROM Customers c
// LEFT JOIN Orders o ON c.Id = o.CustomerId
// GROUP BY c.Name
```

#### **Quando Include può peggiorare - Cartesian Explosion**

```csharp
// PERICOLO - Multiple Includes
var customers = context.Customers
    .Include(c => c.Orders)
    .Include(c => c.Addresses)
    .ToList();

// Se un cliente ha:
// - 10 Orders
// - 3 Addresses
// La query ritorna 10 × 3 = 30 righe per cliente!
```

**Soluzione - Split Queries (EF Core 5+)**:

```csharp
var customers = context.Customers
    .Include(c => c.Orders)
    .Include(c => c.Addresses)
    .AsSplitQuery() // Esegue query separate
    .ToList();

// Query 1: SELECT * FROM Customers
// Query 2: SELECT * FROM Orders WHERE CustomerId IN (...)
// Query 3: SELECT * FROM Addresses WHERE CustomerId IN (...)
```

**Nel tuo lavoro**:

Quando hai ottimizzato il KPI Dashboard da 8s a 1.5s, probabilmente hai risolto problemi N+1:

- Aggiunto `.Include()` dove serviva
- Usato proiezioni per caricare solo dati necessari
- Identificato loop con query dentro

---

### LINQ - Best Practices per il Colloquio

**Domanda**: "Come ottimizzi le query LINQ?"

**Risposta completa**:

1. **IQueryable fino alla fine**: Materializza (`ToList`) solo quando necessario
2. **Proiezioni**: Usa `Select` per caricare solo campi necessari
3. **Eager Loading**: `Include` per evitare N+1
4. **Any() non Count()**: Per verificare esistenza
5. **Where prima di Select**: Filtra prima di trasformare
6. **Evita multiple enumerazioni**: Materializza una volta se usi result più volte
7. **Attenzione a Include multipli**: Usa `AsSplitQuery` se necessario
8. **AsNoTracking**: Per query read-only

**Esempio completo ottimizzato**:

```csharp
// OTTIMIZZATO per performance
var result = context.Orders
    .Where(o => o.Date >= startDate)           // Filtra in SQL
    .Where(o => o.Status == "Completed")       // Filtra in SQL
    .Include(o => o.Customer)                  // Eager loading
    .AsNoTracking()                            // No change tracking
    .Select(o => new OrderDto                  // Proiezione
    {
        OrderId = o.Id,
        CustomerName = o.Customer.Name,
        Total = o.Total,
        Date = o.Date
    })
    .ToList();                                 // Materializza UNA volta

// 1 query SQL ottimizzata

# PARTE 2: PROGRAMMAZIONE ASINCRONA E CONCORRENZA

## 5. Async/Await - La Rivoluzione del Non-Blocking {#async-rivoluzione}

### La Nascita di Async/Await - Il Problema che Risolve

Per capire **perché** async/await è rivoluzionario, devi capire il problema che esisteva prima.

#### **Il Vecchio Mondo - Blocking I/O**

**Scenario**: Server web ASP.NET che gestisce richieste.

```

// [ASP.NET](http://ASP.NET) tradizionale (sincrono)

public ActionResult GetUserData(int userId)

{

// Thread bloccato qui - aspetta il database

var user = _database.GetUser(userId); // 100ms

// Thread bloccato qui - aspetta l'API

var orders = _apiClient.GetOrders(userId); // 200ms

return View(new { user, orders });

}

```

**Cosa succede al thread**:
1. Request arriva → Thread preso dal ThreadPool
2. Chiama database → **Thread bloccato** per 100ms (aspetta I/O)
3. Chiama API → **Thread bloccato** per 200ms (aspetta I/O)
4. Ritorna response → Thread rilasciato al pool

**Tempo totale**: 300ms
**Thread occupato**: 300ms
**Thread che fa lavoro utile**: ~5ms (creazione oggetti, logica)
**Thread sprecato in attesa**: 295ms (98%!)

#### **Il Problema - Scalabilità**

Il ThreadPool ha un **numero limitato di thread** (default: numero di CPU × alcuni fattori).

Immagina:
- ThreadPool size: 100 threads
- Request al secondo: 500
- Tempo medio per request: 300ms

**Matematica**:
- Ogni secondo arrivano 500 request
- Ogni request blocca un thread per 300ms
- Dopo 100 request, **ThreadPool esaurito**
- Request 101-500: **in coda, aspettano**
- Utenti vedono timeout, errori 503

**Questo è il problema della scalabilità sincrona**: I thread passano il 98% del tempo **aspettando I/O**, non facendo lavoro.

---

### Async/Await - La Soluzione Elegante

**Idea fondamentale**: Quando un'operazione deve aspettare (I/O), **libera il thread** invece di bloccarlo.

```

// [ASP.NET](http://ASP.NET) Core (asincrono)

public async Task<ActionResult> GetUserDataAsync(int userId)

{

// Thread liberato durante l'attesa

var user = await _database.GetUserAsync(userId); // 100ms

// Thread liberato durante l'attesa

var orders = await _apiClient.GetOrdersAsync(userId); // 200ms

return View(new { user, orders });

}

```

**Cosa succede al thread**:
1. Request arriva → Thread preso dal ThreadPool
2. Chiama database → `await` **libera il thread** (torna al pool)
3. *(thread serve altre request)*
4. Database risponde → ThreadPool riassegna un thread (potrebbe essere diverso!)
5. Chiama API → `await` **libera il thread**
6. *(thread serve altre request)*
7. API risponde → ThreadPool riassegna un thread
8. Ritorna response

**Tempo totale**: 300ms (stesso!)
**Thread occupato**: ~10ms (solo lavoro utile)
**Thread liberato**: 290ms (può servire altre request!)

#### **L'Impatto sulla Scalabilità**

Stesso scenario:
- ThreadPool size: 100 threads
- Request al secondo: 500
- Tempo medio per request: 300ms

**Con async/await**:
- Ogni thread è occupato solo 10ms per request
- In 1 secondo, un thread può gestire: 1000ms / 10ms = **100 request**
- 100 threads × 100 request = **10.000 request/secondo**

Da 100 request/sec a 10.000 request/sec - **100x throughput!**

**Questo è il superpotere di async/await**: Stessi thread, enormemente più request.

---

### Come Funziona Async/Await - Sotto il Cofano

**Concetto chiave**: `async/await` è **syntactic sugar** per una **state machine** generata dal compilatore.

#### **Il Codice che Scrivi**

```

public async Task<string> GetDataAsync()

{

Console.WriteLine("Start");

var result1 = await GetFromDatabaseAsync();

Console.WriteLine($"Got from DB: {result1}");

var result2 = await GetFromApiAsync(result1);

Console.WriteLine($"Got from API: {result2}");

return result2;

}

```

#### **Cosa Genera il Compilatore (concettualmente)**

Il compilatore trasforma il tuo metodo in una **state machine** (come un automa a stati):

```

// Generato dal compilatore (semplificato)

public Task<string> GetDataAsync()

{

var stateMachine = new StateMachine();

stateMachine.state = 0;

stateMachine.builder = AsyncTaskMethodBuilder<string>.Create();

stateMachine.builder.Start(ref stateMachine);

return stateMachine.builder.Task;

}

struct StateMachine : IAsyncStateMachine

{

public int state;

public AsyncTaskMethodBuilder<string> builder;

private TaskAwaiter<string> awaiter1;

private TaskAwaiter<string> awaiter2;

public void MoveNext()

{

try

{

switch (state)

{

case 0: // Inizio

Console.WriteLine("Start");

awaiter1 = GetFromDatabaseAsync().GetAwaiter();

if (!awaiter1.IsCompleted)

{

state = 1;

awaiter1.OnCompleted(MoveNext); // Continua quando completa

return; // ESCE - libera thread!

}

goto case 1;

case 1: // Dopo primo await

var result1 = awaiter1.GetResult();

Console.WriteLine($"Got from DB: {result1}");

awaiter2 = GetFromApiAsync(result1).GetAwaiter();

if (!awaiter2.IsCompleted)

{

state = 2;

awaiter2.OnCompleted(MoveNext);

return; // ESCE - libera thread!

}

goto case 2;

case 2: // Dopo secondo await

var result2 = awaiter2.GetResult();

Console.WriteLine($"Got from API: {result2}");

builder.SetResult(result2);

return;

}

}

catch (Exception ex)

{

builder.SetException(ex);

}

}

}

```

**Cosa succede**:
1. Metodo inizia → State = 0
2. Primo `await` → Se non completo, registra callback e **ritorna** (libera thread)
3. Quando operazione completa → Callback richiama `MoveNext()` → State = 1
4. Continua esecuzione → Secondo `await`
5. E così via

**Punti chiave**:
- `await` non blocca, **ritorna** e registra continuation
- Stato salvato nei campi della struct
- Quando I/O completa, ThreadPool riprende da dove si era fermato
- Potrebbe essere un **thread diverso** (thread-agnostic)

---

### Task<T> - La Promessa di un Valore Futuro

**Concetto**: Un `Task<T>` è una **promessa** che in futuro avrai un valore di tipo `T`.

```

Task<string> promise = GetDataAsync();

// In questo momento, il valore NON c'è ancora

// Il metodo sta eseguendo (o completato)

string value = await promise; // Aspetta che la promessa si realizzi

```

**Stati di un Task**:
- **Created**: Creato ma non avviato
- **WaitingForActivation**: Aspetta di iniziare
- **Running**: In esecuzione
- **RanToCompletion**: Completato con successo
- **Faulted**: Completato con errore
- **Canceled**: Cancellato

**Proprietà importanti**:
```

var task = GetDataAsync();

task.IsCompleted    // true se completato (successo/errore/cancellato)

task.IsFaulted      // true se exception

task.IsCanceled     // true se cancellato

task.Result         // ⚠️ BLOCCA finché non completa (PERICOLO!)

task.Exception      // Exception se faulted

task.Status         // Stato corrente

```

---

### CPU-Bound vs I/O-Bound - La Distinzione Fondamentale

Questa è **cruciale** per sapere quando usare async/await.

#### **I/O-Bound Operations - Aspettano Qualcosa**

**Caratteristiche**:
- Aspettano risorse esterne (database, file, network)
- CPU quasi inattiva durante l'attesa
- **Thread può essere liberato**

**Esempi**:
- Database query
- HTTP requests
- Lettura/scrittura file
- Chiamate a servizi esterni

**Soluzione**: `async/await`
```

public async Task<Data> LoadDataAsync()

{

// I/O-bound - usa async/await

return await _httpClient.GetFromJsonAsync<Data>("api/data");

}

```

**Perché funziona**: Durante `await`, il thread **torna al ThreadPool** e serve altre request.

#### **CPU-Bound Operations - Fanno Calcoli**

**Caratteristiche**:
- Calcoli intensivi (algoritmi, crittografia, elaborazione)
- CPU al 100%
- **Thread non può fare altro**

**Esempi**:
- Calcoli matematici complessi
- Compressione/decompressione
- Rendering immagini
- Parsing di grandi JSON

**Soluzione**: `Task.Run` (NON async/await diretto)
```

public async Task<int> CalculateAsync()

{

// CPU-bound - delega a ThreadPool thread

return await [Task.Run](http://Task.Run)(() => HeavyComputation());

}

private int HeavyComputation()

{

int result = 0;

for (int i = 0; i < 1000000000; i++)

{

result += i;

}

return result;

}

```

**Perché Task.Run**: Sposta il lavoro CPU-intensive su un **thread background**, liberando il thread corrente.

#### **Errore Comune - Async su CPU-Bound**

```

// ❌ SBAGLIATO

public async Task<int> CalculateAsync()

{

int result = 0;

for (int i = 0; i < 1000000000; i++)

{

result += i; // Blocca il thread!

}

return result; // No await, no benefit

}

```

Qui `async` non fa **nulla** perché non c'è `await`. Il thread è bloccato nei calcoli.

**Regola d'oro**:
- **I/O-bound** → `async/await`
- **CPU-bound** → `Task.Run` in async context, o sincrono se appropriato

---

### Deadlock - Il Killer Silenzioso

Il deadlock con async/await è uno dei bug più insidiosi e comuni, specialmente in applicazioni desktop e legacy.

#### **Scenario Classico - UI Thread o ASP.NET Context**

```

// ❌ CODICE CHE CAUSA DEADLOCK

public string GetData()

{

var task = GetDataAsync();

return task.Result; // BLOCCA e aspetta

}

public async Task<string> GetDataAsync()

{

await Task.Delay(1000);

return "data";

}

```

**Cosa succede (in UI o vecchio ASP.NET)**:

1. `GetData()` chiama `GetDataAsync()`
2. `GetDataAsync()` inizia su **UI thread** (o ASP.NET context thread)
3. `await Task.Delay(1000)` → Registra continuation sul **UI thread**
4. Ritorna al chiamante
5. `task.Result` **blocca il UI thread** aspettando il Task
6. Task.Delay completa, vuole eseguire continuation sul **UI thread**
7. MA il UI thread è **bloccato** su `task.Result`
8. **DEADLOCK**: UI thread aspetta Task, Task aspetta UI thread

**Visualizzazione**:
```

UI Thread: [GetData] → [task.Result BLOCCA] → aspetta Task

↑                                      |

|  |
| --- |

└─────── Task vuole UI thread ────────┘

DEADLOCK!

```

#### **Perché Succede - Synchronization Context**

In UI (WPF, WinForms) e vecchio ASP.NET, esiste un **SynchronizationContext** che garantisce che le continuation tornino sullo **stesso thread**.

Quando fai `await`, la continuation è schedulata sul **context catturato**.

Ma se quel thread è **bloccato** (da `.Result` o `.Wait()`), la continuation non può eseguire → deadlock.

#### **Soluzioni**

**Soluzione 1 - Async All The Way** (MIGLIORE)
```

// ✅ CORRETTO

public async Task<string> GetDataAsync() // async

{

return await GetDataInternalAsync();

}

public async Task<string> GetDataInternalAsync()

{

await Task.Delay(1000);

return "data";

}

// Chiamante

var data = await GetDataAsync(); // async tutto il percorso

```

**Principio**: **Mai mescolare sync e async**. Se usi async, vai async fino in cima allo stack.

**Soluzione 2 - ConfigureAwait(false)** (per librerie)
```

public async Task<string> GetDataAsync()

{

// Non cattura SynchronizationContext

await Task.Delay(1000).ConfigureAwait(false);

return "data";

}

```

`ConfigureAwait(false)` dice: "Non mi importa su quale thread continuo, usa qualsiasi thread del ThreadPool."

**Quando usarlo**:
- **Librerie**: Sempre `ConfigureAwait(false)` (non hai UI)
- **Applicazioni**: Di solito no (vuoi tornare su UI thread)

**Soluzione 3 - Task.Run** (workaround, non ideale)
```

public string GetData()

{

return [Task.Run](http://Task.Run)(() => GetDataAsync()).Result;

}

```

Questo funziona ma è **brutto**. Meglio riscrivere async.

#### **ASP.NET Core - Deadlock-Free**

Buona notizia: **ASP.NET Core non ha SynchronizationContext**.

Quindi anche se fai `.Result` (non dovresti!), probabilmente non deadlock.

Ma `.Result` è **ancora male** perché blocca thread.

---

### Task.WhenAll e Task.WhenAny - Composizione Parallela

#### **Task.WhenAll - Aspetta Tutti**

**Scenario**: Devi fare multiple operazioni I/O **in parallelo**.

```

// ❌ SEQUENZIALE - Lento

public async Task<Data> LoadAllDataAsync()

{

var users = await GetUsersAsync();      // 200ms

var products = await GetProductsAsync(); // 300ms

var orders = await GetOrdersAsync();    // 250ms

return new Data { users, products, orders };

}

// Tempo totale: 200 + 300 + 250 = 750ms

```

```

// ✅ PARALLELO - Veloce

public async Task<Data> LoadAllDataAsync()

{

// Avvia tutte le operazioni CONTEMPORANEAMENTE

var usersTask = GetUsersAsync();

var productsTask = GetProductsAsync();

var ordersTask = GetOrdersAsync();

// Aspetta che TUTTE completino

await Task.WhenAll(usersTask, productsTask, ordersTask);

// Ora tutte sono complete

var users = usersTask.Result;     // Già completato, no blocking

var products = productsTask.Result;

var orders = ordersTask.Result;

return new Data { users, products, orders };

}

// Tempo totale: Max(200, 300, 250) = 300ms

```

**Risparmio**: Da 750ms a 300ms - **2.5x più veloce!**

**Versione elegante con tuple**:
```

public async Task<Data> LoadAllDataAsync()

{

var (users, products, orders) = await 

(GetUsersAsync(), GetProductsAsync(), GetOrdersAsync());

return new Data { users, products, orders };

}

```

**Con collezioni**:
```

public async Task<List<User>> GetMultipleUsersAsync(int[] userIds)

{

var tasks = [userIds.Select](http://userIds.Select)(id => GetUserAsync(id));

var users = await Task.WhenAll(tasks);

return users.ToList();

}

```

**Gestione errori**:
```

try

{

await Task.WhenAll(task1, task2, task3);

}

catch (Exception ex)

{

// Solo la PRIMA exception è catturata

// Le altre sono in task.Exception

}

```

#### **Task.WhenAny - Aspetta il Primo**

**Scenario**: Vuoi il primo risultato che arriva, o implementare timeout.

```

// Timeout pattern

public async Task<Data> GetDataWithTimeoutAsync(TimeSpan timeout)

{

var dataTask = GetDataAsync();

var timeoutTask = Task.Delay(timeout);

var completedTask = await Task.WhenAny(dataTask, timeoutTask);

if (completedTask == timeoutTask)

{

throw new TimeoutException("Operation timed out");

}

return await dataTask; // Già completato

}

```

**Primo di multiple sorgenti**:
```

public async Task<Data> GetFromFastestSourceAsync()

{

var task1 = GetFromDatabase();

var task2 = GetFromCache();

var task3 = GetFromApi();

var completedTask = await Task.WhenAny(task1, task2, task3);

return await completedTask; // Ritorna il primo che completa

}

```

---

### CancellationToken - Cancellare Operazioni Async

**Concetto**: Permetti alle operazioni async di essere **cancellate** gracefully.

#### **Scenario Base**

```

public async Task<Data> LoadDataAsync(CancellationToken cancellationToken)

{

// Controlla se già cancellato

cancellationToken.ThrowIfCancellationRequested();

var data = await _httpClient.GetFromJsonAsync<Data>(

"api/data", 

cancellationToken); // Passa il token

// Lavoro CPU-bound - controlla periodicamente

for (int i = 0; i < 1000000; i++)

{

if (i % 10000 == 0)

{

cancellationToken.ThrowIfCancellationRequested();

}

// Lavoro...

}

return data;

}

```

**Uso**:
```

var cts = new CancellationTokenSource();

// Avvia operazione

var task = LoadDataAsync(cts.Token);

// Dopo 5 secondi, cancella

cts.CancelAfter(TimeSpan.FromSeconds(5));

try

{

var data = await task;

}

catch (OperationCanceledException)

{

Console.WriteLine("Operazione cancellata");

}

finally

{

cts.Dispose();

}

```

#### **Timeout Automatico**

```

public async Task<Data> LoadDataAsync()

{

using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

try

{

return await _api.GetDataAsync(cts.Token);

}

catch (OperationCanceledException)

{

throw new TimeoutException("API call timed out after 10 seconds");

}

}

```

#### **Cancellazione da UI**

```

// Button Click

private CancellationTokenSource _cts;

private async void LoadButton_Click(object sender, EventArgs e)

{

_cts = new CancellationTokenSource();

try

{

var data = await LoadDataAsync(_cts.Token);

DisplayData(data);

}

catch (OperationCanceledException)

{

[MessageBox.Show](http://MessageBox.Show)("Caricamento cancellato");

}

}

private void CancelButton_Click(object sender, EventArgs e)

{

_cts?.Cancel(); // Cancella operazione

}

```

#### **Nel tuo lavoro - Template Manager**

```

public async Task<byte[]> GeneratePdfAsync(

int templateId, 

CancellationToken cancellationToken = default)

{

var template = await _context.Templates

.FindAsync(new[] { templateId }, cancellationToken);

cancellationToken.ThrowIfCancellationRequested();

var content = await RenderContentAsync(template, cancellationToken);

cancellationToken.ThrowIfCancellationRequested();

return await _pdfGenerator.GenerateAsync(content, cancellationToken);

}

```

Se l'utente chiude la pagina, la generazione PDF viene cancellata invece di continuare inutilmente.

---

### ValueTask<T> - Ottimizzazione per Hot Path

**Problema con Task<T>**: Ogni Task è un'allocazione nello Heap.

Se hai un metodo chiamato **milioni di volte** e **spesso** il risultato è già disponibile (cache hit), allochi milioni di Task inutilmente.

**Esempio - Cache**:
```

public async Task<User> GetUserAsync(int id)

{

if (_cache.TryGetValue(id, out var user))

{

return user; // Già disponibile, ma abbiamo allocato Task!

}

return await _database.GetUserAsync(id);

}

```

Se 99% delle chiamate sono cache hit, allochiamo 99% di Task inutili.

**Soluzione - ValueTask<T>**:
```

public ValueTask<User> GetUserAsync(int id)

{

if (_cache.TryGetValue(id, out var user))

{

return new ValueTask<User>(user); // No allocazione!

}

return new ValueTask<User>(GetUserFromDatabaseAsync(id));

}

private async Task<User> GetUserFromDatabaseAsync(int id)

{

return await _database.GetUserAsync(id);

}

```

**Quando usare ValueTask**:
- Hot path (chiamato molto frequentemente)
- Spesso il risultato è sincrono (cache, validazione)
- Performance critica

**Quando NON usare**:
- Codice normale (Task è più semplice)
- Risultato quasi sempre asincrono
- Non hai misurato che sia un problema

**Restrizioni ValueTask** (importante!):
- Può essere awaited **solo una volta**
- Non può essere `.Result` senza await
- Non conservare in variabili per uso multiplo

**Regola**: Usa `Task<T>` per default, `ValueTask<T>` solo se profiling mostra benefici.

---

### Async Best Practices - Riepilogo per il Colloquio

**Domanda**: "Quali sono le best practices per async/await?"

**Risposta completa**:

1. **Async all the way**: Mai `.Result` o `.Wait()`, sempre `await`

2. **Suffisso Async**: Metodi async terminano con `Async` (es: `GetDataAsync`)

3. **CancellationToken**: Accetta sempre `CancellationToken` in metodi async pubblici

4. **ConfigureAwait(false)** in librerie: Evita capturing context

5. **Task.WhenAll** per parallelismo: Non await sequenziali se indipendenti

6. **Non async void**: Solo per event handlers, altrimenti `async Task`

7. **ValueTask** solo se necessario: Profila prima di ottimizzare

8. **I/O-bound** → async/await, **CPU-bound** → Task.Run

9. **Exception handling**: try/catch funziona normalmente con await

10. **Non creare thread**: async/await non crea thread, riutilizza ThreadPool

**Esempio applicato - Il tuo KPI Dashboard**:

```

public async Task<KpiResult> GetKpiDataAsync(

DateTime from, 

DateTime to,

CancellationToken cancellationToken = default)

{

// Carica dati in parallelo

var metricsTask = _context.Metrics

.Where(m => [m.Date](http://m.Date) >= from && [m.Date](http://m.Date) <= to)

.ToListAsync(cancellationToken);

var targetsTask = _context.Targets

.ToListAsync(cancellationToken);

await Task.WhenAll(metricsTask, targetsTask);

var metrics = await metricsTask;

var targets = await targetsTask;

// CPU-bound calculation

var result = await [Task.Run](http://Task.Run)(() => 

CalculateKpis(metrics, targets), 

cancellationToken);

return result;

}

```jsx

**Ottimizzazione da 8s a 1.5s**: Probabilmente hai:
1. Parallelizzato query con `Task.WhenAll`
2. Usato `AsNoTracking` per read-only
3. Aggiunto proiezioni per caricare solo dati necessari
4. Evitato N+1 con `Include`

## 6. Multithreading - Coordinazione e Sincronizzazione {#multithreading}

### Async vs Multithreading - La Distinzione Critica

Questa è una delle **confusioni più comuni** anche tra sviluppatori esperti.

**Frase killer per il colloquio**: "Asynchronous ≠ Multithreading"

#### **Asynchronous (Async/Await)**

**Cosa è**: Pattern per **non bloccare** thread durante I/O.

**Come funziona**: 
- Un thread inizia operazione
- Durante I/O, thread **liberato** (torna al pool)
- Quando I/O completa, **qualsiasi thread** continua
- Spesso **non crea nuovi thread**

**Scenario ideale**: I/O-bound (database, API, file)

**Quando usare SemaphoreSlim**:
- Limitare chiamate API
- Proteggere risorse condivise
- Rate limiting

**Risposta**: "SemaphoreSlim per limitare accessi concorrenti, ideale per async code"

---

Continuo con le altre sezioni del multithreading (lock, Interlocked, collections concurrent, Parallel)?

---

---

## 6b. Multithreading — lock, Interlocked, Concurrent Collections {#multithreading-sync}

### lock — Sezione Critica Più Comune

`lock` garantisce che **solo un thread alla volta** possa eseguire un blocco di codice.

```

public class OrderCounter

{

private int _count = 0;

private readonly object _lock = new object(); // Dedicated lock object

public void Increment()

{

lock (_lock)

{

_count++; // Solo un thread alla volta

}

}

public int GetCount() => Interlocked.CompactRead(ref _count);

}

```

**Regole fondamentali di `lock`:**
- Lock object deve essere `private readonly` — mai esporre l'oggetto di lock
- Mai fare `lock(this)` — se qualcuno ha reference esterna, può lockare la tua classe
- Mai fare `lock` su value types (struct) — boxing crea nuovo oggetto ogni volta
- Mantieni la sezione critica **minima** — meno codice dentro lock, meno chance di deadlock

```

// ❌ MALE

lock (this) { } // Espone lock

lock ("string") { } // String è interned — stesso oggetto ovunque!

lock (new object()) { } // Nuovo oggetto ogni volta — NON funziona!

// ✅ BENE

private readonly object _lock = new object();

lock (_lock) { / *codice minimo* / }

```

### Interlocked — Operazioni Atomiche Senza Lock

Per operazioni semplici (incremento, confronto-e-scambio), `Interlocked` è più veloce di `lock`.

```

public class AtomicCounter

{

private int _count = 0;

public void Increment()

{

Interlocked.Increment(ref _count); // Atomico, nessun lock

}

public int Decrement()

{

return Interlocked.Decrement(ref _count);

}

// Compare-and-Swap — cambia solo se il valore corrente è quello atteso

public bool TrySetValue(int expected, int newValue)

{

return Interlocked.CompareExchange(ref _count, newValue, expected) == expected;

}

}

```

**Quando usare Interlocked vs lock:**
- `Interlocked`: Operazioni singole su variabili primitives (++, --, swap)
- `lock`: Quando devi proteggere **sequenze** di operazioni che devono essere atomiche

### SemaphoreSlim — Limita Accessi Concorrenti

```

public class RateLimitedService

{

// Massimo 5 chiamate concurrent

private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(5, 5);

public async Task<Result> CallExternalApiAsync()

{

await _semaphore.WaitAsync(); // Aspetta se già 5 in corso

try

{

return await _httpClient.GetAsync("api/data");

}

finally

{

_semaphore.Release(); // Rilascia posto

}

}

}

```

**SemaphoreSlim vs Mutex vs lock:**
- `lock`: Più semplice, solo per sync code, un solo thread
- `SemaphoreSlim`: Async-friendly, permette N thread concorrenti
- `Mutex`: Cross-process (raramente necessario in web apps)

### Concurrent Collections — Thread-Safe senza Lock Manuale

```

using System.Collections.Concurrent;

public class CacheService

{

// Thread-safe dictionary — no lock necessario

private readonly ConcurrentDictionary<string, Data> _cache = new();

public Data GetOrAdd(string key)

}

{

return _cache.GetOrAdd(key, k => LoadFromDatabase(k));

// GetOrAdd è atomico: se key non esiste, chiama factory UNA volta

}

public bool TryRemove(string key)

{

return _cache.TryRemove(key, out _);

}

}

```

**Collections concurrent principali:**
- `ConcurrentDictionary<K,V>` — Dictionary thread-safe (usa il più)
- `ConcurrentQueue<T>` — Queue thread-safe (producer-consumer)
- `ConcurrentBag<T>` — Collezione unordered thread-safe
- `BlockingCollection<T>` — Queue con blocking (producer-consumer pattern)

### Parallel — Parallelismo Dati

```

// Parallel.ForEach — Elabora collection in parallelo

var items = GetItems(); // 1000 items

Parallel.ForEach(items, item =>

{

ProcessItem(item); // Eseguito in parallelo su thread pool

});

// Con opzioni di controllo

Parallel.ForEach(

items,

new ParallelOptions { MaxDegreeOfParallelism = 4 }, // Max 4 thread

item => ProcessItem(item)

);

```

**Quando usare Parallel vs async:**
- `Parallel.ForEach`: CPU-bound (calcoli pesanti, milioni di items)
- `async/await` con `Task.WhenAll`: I/O-bound (chiamate API, DB queries)

---
```

---

---

# PARTE 4: PERSISTENZA DATI

## 1. SQL Server — Queries, JOINs, Indexes, Transactions {#sql-performance}

### JOINs — Tipi e Quando Usarli

JOIN è il modo di combinare righe da tabelle diverse basandosi su una relazione.

```sql
-- Setup
CREATE TABLE Customers (Id INT PK, Name NVARCHAR(100));
CREATE TABLE Orders (Id INT PK, CustomerId INT FK, Amount DECIMAL, Status NVARCHAR(20));
```

#### **INNER JOIN — Solo dove c'è match in entrambe**

```sql
SELECT c.Name, o.Amount
FROM Customers c
INNER JOIN Orders o ON c.Id = o.CustomerId;
-- Ritorna solo clienti CHE HANNO ordini
-- Clienti senza ordini: NON compaiono
```

#### **LEFT JOIN — Tutto dalla sinistra + match dalla destra**

```sql
SELECT c.Name, o.Amount  -- o.Amount sara NULL se nessun ordine
FROM Customers c
LEFT JOIN Orders o ON c.Id = o.CustomerId;
-- Ritorna TUTTI i clienti, anche senza ordini
-- Se nessun ordine: Amount = NULL
```

**Uso tipico:** Quando vuoi TUTTI i record dalla tabella principale, anche se non hanno dati correlati.

#### **RIGHT JOIN — Tutto dalla destra + match dalla sinistra**

```sql
SELECT c.Name, o.Amount
FROM Customers c
RIGHT JOIN Orders o ON c.Id = o.CustomerId;
-- Ritorna tutti gli ordini, anche se il customer non esiste (edge case)
```

> **Nota:** RIGHT JOIN è raramente usato. Puoi sempre riscriversilo come LEFT JOIN invertendo le tabelle.
> 

#### **FULL OUTER JOIN — Tutto da entrambi i lati**

```sql
SELECT c.Name, o.Amount
FROM Customers c
FULL OUTER JOIN Orders o ON c.Id = o.CustomerId;
-- Tutti i clienti E tutti gli ordini
-- NULL dove non c'è corrispondenza
```

#### **CROSS JOIN — Prodotto cartesiano**

```sql
SELECT s.Name AS Size, c.Name AS Color
FROM Sizes s
CROSS JOIN Colors c;
-- Ogni size combinata con ogni color
-- 3 sizes x 4 colors = 12 righe
```

#### **Self JOIN — Tabella si unisce a se stessa**

```sql
-- Trova manager di ogni dipendente
SELECT e.Name AS Employee, m.Name AS Manager
FROM Employees e
INNER JOIN Employees m ON e.ManagerId = m.Id;
```

---

### Subqueries vs JOIN — Quale è Meglio?

```sql
-- Opzione 1: Subquery
SELECT Name FROM Customers
WHERE Id IN (
    SELECT CustomerId FROM Orders WHERE Amount > 100
);

-- Opzione 2: JOIN (generalmente più efficiente)
SELECT DISTINCT c.Name
FROM Customers c
INNER JOIN Orders o ON c.Id = o.CustomerId
WHERE o.Amount > 100;

-- Opzione 3: EXISTS (meglio di IN per grandi dataset)
SELECT Name FROM Customers c
WHERE EXISTS (
    SELECT 1 FROM Orders o 
    WHERE o.CustomerId = c.Id AND o.Amount > 100
);
```

**Regola:** `EXISTS` è spesso il migliore per "verifica esistenza", `JOIN` per aggregazioni, `IN` solo se la subquery ritorna pochi record.

---

### Indexes — Performance Fondamentale

**Cos'e un index?** Una struttura aggiuntiva (tipicamente B-Tree) che velocizza la ricerca di righe.

```sql
-- Senza index: Full Table Scan (legge ogni riga)
SELECT * FROM Orders WHERE CustomerId = 123;
-- Con 10 milioni di righe: lento!

-- Crea index
CREATE INDEX IX_Orders_CustomerId ON Orders (CustomerId);
-- Ora la ricerca è O(log n) invece di O(n)
```

#### **Tipi di Index:**

```sql
-- 1. Non-Clustered Index (default)
CREATE INDEX IX_Orders_CustomerId ON Orders (CustomerId);
-- Struttura separata dai dati, punta alle righe
-- Una tabella può avere MOLTI non-clustered indexes

-- 2. Clustered Index (organizza i dati fisicamente)
CREATE CLUSTERED INDEX IX_Orders_Date ON Orders (OrderDate);
-- I dati della tabella sono ordinati per OrderDate
-- Una tabella può avere UN SOLO clustered index
-- (spesso è la Primary Key)

-- 3. Composite Index (colonne multiple)
CREATE INDEX IX_Orders_Customer_Status ON Orders (CustomerId, Status);
-- Efficiente per query che filtrano su ENTRAMBE le colonne
-- L'ordine conta! CustomerId deve essere primo se filtri sempre su quello

-- 4. Unique Index
CREATE UNIQUE INDEX IX_Users_Email ON Users (Email);
-- Garantisce unicità + performance di ricerca

-- 5. Filtered Index
CREATE INDEX IX_Orders_Active ON Orders (CustomerId)
WHERE Status = 'Active';
-- Index solo per ordini attivi — più piccolo, più veloce
```

#### **Quando NON creare index:**

- Colonne con pochi valori distinti (es. IsActive con solo true/false)
- Tabelle piccole (<1000 righe) — full scan è già veloce
- Troppe colonne nello stesso index
- Ogni DML (INSERT/UPDATE/DELETE) deve aggiornare tutti gli index → overhead

---

### Execution Plan — Come SQL Server Esegue la Query

```sql
-- Vedi come SQL Server esegue la query
SET STATISTICS IO ON;
SET STATISTICS TIME ON;

SELECT c.Name, COUNT(o.Id) as OrderCount
FROM Customers c
LEFT JOIN Orders o ON c.Id = o.CustomerId
WHERE c.Status = 'Active'
GROUP BY c.Name;
```

**Cosa guardare nel Execution Plan:**

- **Table Scan** = Legge TUTTO (manca index)
- **Index Scan** = Legge tutto l'index (filtro non selective)
- **Index Seek** = Trova direttamente le righe (ottimo!)
- **Key Lookup** = Trova via index ma poi torna a prendere i dati (consider covering index)
- **Sort** = Ordinamento in memoria (consider index ordinato)

---

### Transactions e Isolation Levels

```sql
-- Transaction base
BEGIN TRANSACTION;

UPDATE Accounts SET Balance = Balance - 100 WHERE Id = 1;
UPDATE Accounts SET Balance = Balance + 100 WHERE Id = 2;

COMMIT; -- Se tutto ok
-- oppure
ROLLBACK; -- Se qualcosa fallisce
```

**ACID Properties:**

- **A**tomicity: Tutto o niente
- **C**onsistency: DB resta in stato valido
- **I**solation: Transazioni non si interferiscono
- **D**urability: Dopo COMMIT, i dati sono persisti

#### **Isolation Levels — Il trade-off Concorrenza/Consistenza:**

| Level | Dirty Read | Non-Repeatable | Phantom | Performance |
| --- | --- | --- | --- | --- |
| **Read Uncommitted** | ✅ | ✅ | ✅ | Massima |
| **Read Committed** (default SQL Server) | ❌ | ✅ | ✅ | Alta |
| **Repeatable Read** | ❌ | ❌ | ✅ | Media |
| **Serializable** | ❌ | ❌ | ❌ | Minima |
| **Snapshot** | ❌ | ❌ | ❌ | Alta (MVCC) |

**Definizioni dei problemi:**

- **Dirty Read:** Leggi dati non ancora committati
- **Non-Repeatable Read:** Leggi stesso dato 2 volte, risultati diversi
- **Phantom Read:** Leggi set di righe 2 volte, numero di righe cambia

```sql
-- Cambia isolation level
SET TRANSACTION ISOLATION LEVEL SNAPSHOT;
-- Snapshot: ogni transazione vede un "picture" consistente
-- Nessun blocking in lettura, eccellente per read-heavy workloads
```

---

### Aggregazioni e Window Functions

```sql
-- GROUP BY base
SELECT CustomerId, SUM(Amount) as Total
FROM Orders
GROUP BY CustomerId
HAVING SUM(Amount) > 1000; -- HAVING filtra i gruppi

-- Window Functions — Aggrega senza perdere le righe individuali
SELECT
    Id,
    CustomerId,
    Amount,
    SUM(Amount) OVER (PARTITION BY CustomerId) AS CustomerTotal,
    ROW_NUMBER() OVER (PARTITION BY CustomerId ORDER BY Amount DESC) AS Rank,
    AVG(Amount) OVER () AS GlobalAverage
FROM Orders;
```

**Window Functions principali:**

- `ROW_NUMBER()` — Numero sequenziale unico
- `RANK()` — Rank con gap per ties
- `DENSE_RANK()` — Rank senza gap
- `LAG/LEAD` — Accedi a righe precedenti/successive
- `SUM/AVG/COUNT OVER(...)` — Aggregazione senza GROUP BY

```sql
-- Esempio pratico: ogni ordine + running total per cliente
SELECT
    CustomerId,
    OrderDate,
    Amount,
    SUM(Amount) OVER (
        PARTITION BY CustomerId
        ORDER BY OrderDate
        ROWS UNBOUNDED PRECEDING
    ) AS RunningTotal
FROM Orders;
```

---

### CTEs (Common Table Expressions) — Queries Leggibili

```sql
-- Senza CTE — subquery annidata
SELECT Name, TopAmount
FROM Customers
WHERE Id IN (
    SELECT CustomerId FROM (
        SELECT CustomerId, MAX(Amount) as TopAmount
        FROM Orders GROUP BY CustomerId
    ) sub WHERE TopAmount > 500
);

-- Con CTE — leggibile come codice
WITH CustomerStats AS (
    SELECT 
        CustomerId,
        MAX(Amount) AS TopAmount,
        COUNT(*) AS OrderCount
    FROM Orders
    GROUP BY CustomerId
)
SELECT c.Name, cs.TopAmount, cs.OrderCount
FROM Customers c
INNER JOIN CustomerStats cs ON c.Id = cs.CustomerId
WHERE cs.TopAmount > 500;

-- CTE ricorsiva — gerarchie
WITH RECURSIVE OrgChart AS (
    -- Base: CEO
    SELECT Id, Name, ManagerId, 0 AS Level
    FROM Employees WHERE ManagerId IS NULL
    
    UNION ALL
    
    -- Ricorsione: dipendenti del livello precedente
    SELECT e.Id, e.Name, e.ManagerId, oc.Level + 1
    FROM Employees e
    INNER JOIN OrgChart oc ON e.ManagerId = oc.Id
)
SELECT * FROM OrgChart ORDER BY Level;
```

---

## 2. Entity Framework Core — ORM Deep Dive {#ef-core}

### DbContext — Il Centro di EF Core

```csharp
public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurazione relazioni
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configurazione indici
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}
```

### Change Tracking — Come EF Tiene Traccia

**Concetto fondamentale:** DbContext tiene traccia di tutte le entità "caricate" e delle loro modifiche.

```csharp
var user = await context.Users.FindAsync(1);
// Stato: Unchanged — EF ha una copia originale

user.Name = "New Name";
// Stato: Modified — EF rileva la differenza

await context.SaveChangesAsync();
// Genera: UPDATE Users SET Name = 'New Name' WHERE Id = 1
// Solo i campi CAMBIATI vengono aggiornati
```

**States:** Unchanged → Modified → Deleted | Detached | Added

```csharp
// AsNoTracking — disabilita tracking (per read-only queries)
var users = await context.Users
    .AsNoTracking() // No tracking overhead
    .Where(u => u.IsActive)
    .ToListAsync();
// Più veloce e meno memoria per query read-only

// AsTracking (default per entità tracciate)
var user = await context.Users.FindAsync(1); // Tracked
```

### Migrations — Schema Database

```bash
# Crea migration
dotnet ef migrations add "AddEmailToUser"

# Applica migration al database
dotnet ef database update

# Script SQL (per production/CI)
dotnet ef migrations script --from V1 --to V2 > migration.sql
```

**Best practices migrations:**

- Una migration per logical change
- Mai modificare migration già applicata — crea nuova
- In production: usa script SQL, non `dotnet ef database update`
- Reviora sempre il codice generato prima di applicare

### Relazioni — Configurazione

```csharp
// One-to-Many: Un Customer ha molti Orders
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Order> Orders { get; set; } = new();
}

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; } // FK
    public Customer Customer { get; set; } // Navigation property
    public decimal Amount { get; set; }
}

// Many-to-Many: Un Product ha molti Tags e viceversa
public class Product
{
    public int Id { get; set; }
    public List<Tag> Tags { get; set; } = new();
}

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Product> Products { get; set; } = new();
}
// EF Core 5+ crea junction table automaticamente
```

---

# PARTE 3: [ASP.NET](http://ASP.NET) CORE - ARCHITETTURA WEB ⚠️ MUST HAVE

> **ATTENZIONE**: Questa sezione è **CRITICA** per il colloquio .NET Middle. L'80% delle domande tecniche riguardano [ASP.NET](http://ASP.NET) Core. Senza questa conoscenza, il colloquio NON si passa.
> 

---

## 1. Request Pipeline - Come Funziona [ASP.NET](http://ASP.NET) Core {#aspnet-pipeline}

### La Pipeline HTTP - Il Cuore di [ASP.NET](http://ASP.NET) Core

**Concetto fondamentale**: Ogni HTTP request passa attraverso una **pipeline di middleware** prima di raggiungere il tuo codice (controller/endpoint).

#### **Cos'è un Middleware?**

Un middleware è un componente che:

- Riceve una `HttpRequest`
- Può modificarla o processarla
- **Decide**: passare al prossimo middleware o fermarsi (short-circuit)
- Può modificare la `HttpResponse`
- Ritorna al middleware precedente

**Struttura**:

```csharp
public class MyMiddleware
{
    private readonly RequestDelegate _next;
    
    public MyMiddleware(RequestDelegate next)
    {
        _next = next; // Il prossimo middleware nella pipeline
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        // PRIMA del prossimo middleware
        Console.WriteLine($"Request: {context.Request.Path}");
        
        // Chiama il prossimo
        await _next(context);
        
        // DOPO il prossimo middleware (response processing)
        Console.WriteLine($"Response: {context.Response.StatusCode}");
    }
}
```

#### **La Pipeline Completa - Program.cs ([ASP.NET](http://ASP.NET) Core 6+)**

```csharp
var builder = WebApplication.CreateBuilder(args);

// Configurazione servizi (PRIMA di Build)
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>();
// ...

var app = builder.Build();

// ⚠️ ORDINE CRITICO - LA PIPELINE
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // 1. Exception handling
}

app.UseHttpsRedirection();  // 2. Redirect HTTP → HTTPS
app.UseStaticFiles();       // 3. Serve static files (wwwroot)

app.UseRouting();           // 4. Routing (match URL → endpoint)

app.UseAuthentication();    // 5. Authentication (WHO are you?)
app.UseAuthorization();     // 6. Authorization (WHAT can you do?)

app.MapControllers();       // 7. Map endpoints

app.Run();
```

**Perché l'ordine è CRITICO**:

1. **Exception handling PRIMA**: Cattura errori da tutti i middleware successivi
2. **Static files PRIMA di routing**: Se è un file, non serve routing
3. **Authentication PRIMA di Authorization**: Devi sapere CHI sei prima di verificare permessi
4. **Routing PRIMA di endpoint**: Devi trovare l'endpoint prima di eseguirlo

**Errore comune**:

```csharp
app.UseAuthorization();  // ERRORE! Authorization prima di Authentication
app.UseAuthentication(); // Non funziona - non sai chi è l'utente
```

#### **Visualizzazione della Pipeline**

```
Request →
    [Exception Handler]
        ↓
    [HTTPS Redirect]
        ↓
    [Static Files] → Se file, STOP e ritorna file
        ↓
    [Routing] → Match URL
        ↓
    [Authentication] → Identifica utente
        ↓
    [Authorization] → Verifica permessi
        ↓
    [Endpoint] → Tuo Controller/Action
        ↓
    ← Response
```

---

## 2. Dependency Injection - Inversione del Controllo {#di-filosofia}

### Cos'è la Dependency Injection?

**Problema senza DI**:

```csharp
public class OrderService
{
    private readonly DatabaseContext _context;
    
    public OrderService()
    {
        // ❌ MALE - Tight coupling
        _context = new DatabaseContext("connection string");
    }
}
```

**Problemi**:

1. Hard-coded dependency
2. Impossibile testare (DB reale sempre)
3. Cambiare DB → Modificare OrderService
4. Configurazione duplicata ovunque

**Con DI**:

```csharp
public class OrderService
{
    private readonly IDbContext _context;
    
    // ✅ BENE - Dependency iniettata
    public OrderService(IDbContext context)
    {
        _context = context;
    }
}

// Configurazione in Program.cs
builder.Services.AddScoped<IDbContext, DatabaseContext>();
builder.Services.AddScoped<OrderService>();
```

**Vantaggi**:

1. **Loose coupling**: OrderService non sa quale implementazione usa
2. **Testabilità**: Puoi iniettare MockDbContext
3. **Configurazione centralizzata**: Cambio in UN posto
4. **Lifecycle gestito**: [ASP.NET](http://ASP.NET) Core crea/distrugge istanze

---

### I Tre Lifetime: Scoped, Singleton, Transient ⚠️ DOMANDA FREQUENTISSIMA

**Questa è LA domanda più comune su DI. Devi saperla perfettamente.**

#### **Transient** - Nuova istanza OGNI volta

```csharp
builder.Services.AddTransient<IEmailService, EmailService>();
```

**Comportamento**:

- **Nuova istanza** ogni volta che viene richiesta
- Anche nella **stessa request**, istanze diverse

**Quando usare**:

- Servizi **stateless** e **leggeri**
- Nessuno stato condiviso
- Esempi: Logger, Email sender, Validators

**Esempio**:

```csharp
public class MyController : Controller
{
    private readonly IEmailService _email1;
    private readonly IEmailService _email2;
    
    public MyController(IEmailService email1, IEmailService email2)
    {
        // email1 e email2 sono ISTANZE DIVERSE
        _email1 = email1;
        _email2 = email2;
    }
}
```

---

#### **Scoped** - Una istanza per REQUEST ⚠️ Più usato in Web

```csharp
builder.Services.AddScoped<IDbContext, AppDbContext>();
```

**Comportamento**:

- **Una istanza** per HTTP request
- **Condivisa** tra tutti i servizi nella stessa request
- **Distrutta** alla fine della request

**Quando usare**:

- **DbContext** (Entity Framework) ← Caso più comune!
- Servizi che mantengono stato durante la request
- Unit of Work pattern

**Esempio pratico**:

```csharp
// Request arriva
public class OrderController : Controller
{
    private readonly AppDbContext _context; // Istanza A
    private readonly OrderService _orderService;
    
    public OrderController(AppDbContext context, OrderService orderService)
    {
        _context = context; // Istanza A
        _orderService = orderService; // Usa STESSA istanza A
    }
}

public class OrderService
{
    private readonly AppDbContext _context; // STESSA Istanza A!
    
    public OrderService(AppDbContext context)
    {
        _context = context; // Stessa della request
    }
}
// Request finisce → Istanza A distrutta (Dispose chiamato)
```

**Perché DbContext è Scoped**:

- DbContext traccia modifiche (ChangeTracker)
- Non può essere condiviso tra request (thread-safety)
- Deve vivere per tutta la request (Unit of Work)
- Deve essere disposed (Dispose rilascia connessione DB)

---

#### **Singleton** - Una istanza per APPLICAZIONE

```csharp
builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();
```

**Comportamento**:

- **Una sola istanza** per tutta l'applicazione
- **Condivisa** tra tutte le request
- **Mai distrutta** (fino a shutdown applicazione)

**Quando usare**:

- Configurazioni read-only
- Cache in-memory
- Servizi costosi da creare
- Servizi **thread-safe** e **stateless** (importante!)

**⚠️ PERICOLO - Thread Safety**:

```csharp
// ❌ PERICOLO - Singleton con stato mutabile
public class CounterService // Singleton
{
    public int Counter { get; set; } // PERICOLO!
    
    public void Increment()
    {
        Counter++; // Race condition tra request!
    }
}
```

**Problema**: 100 request contemporanee incrementano `Counter` → Race condition, counter sbagliato.

**Soluzione - Thread-safe**:

```csharp
public class CounterService
{
    private int _counter;
    private readonly object _lock = new object();
    
    public void Increment()
    {
        lock (_lock)
        {
            _counter++;
        }
    }
}
// O usa Interlocked.Increment
```

---

### Tabella Riepilogativa - IMPARA A MEMORIA

| Lifetime | Quando creato | Quando distrutto | Uso tipico |
| --- | --- | --- | --- |
| **Transient** | Ogni richiesta | Subito dopo uso | Stateless, lightweight |
| **Scoped** | Inizio request | Fine request | DbContext, Unit of Work |
| **Singleton** | Startup app | Shutdown app | Config, Cache, Expensive |

---

### Domanda Trabocchetto - Scoped in Singleton ⚠️

**Scenario**:

```csharp
public class MySingleton
{
    private readonly AppDbContext _context; // Scoped!
    
    public MySingleton(AppDbContext context) // ❌ ERRORE!
    {
        _context = context;
    }
}

builder.Services.AddSingleton<MySingleton>();
builder.Services.AddScoped<AppDbContext>();
```

**Problema**:

- `MySingleton` vive per tutta l'app
- Tiene un'istanza di `AppDbContext`
- `AppDbContext` dovrebbe vivere solo per una request
- **Risultato**: DbContext condiviso tra request → Disastro!

[**ASP.NET](http://ASP.NET) Core ti protegge**: Lancia **eccezione** a runtime:

```
Cannot consume scoped service 'AppDbContext' from singleton 'MySingleton'.
```

**Soluzione - IServiceProvider**:

```csharp
public class MySingleton
{
    private readonly IServiceProvider _serviceProvider;
    
    public MySingleton(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public void DoWork()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        // Usa context
    }
}
```

---

## 3. REST API - Principi e Best Practices {#rest-api}

### Cosa Significa REST?

**REST** = **RE**presentational **S**tate **T**ransfer

Non è un protocollo, è uno **stile architetturale** basato su principi.

#### **I 6 Principi REST**

1. **Client-Server**: Separazione concerns
2. **Stateless**: Ogni request è indipendente
3. **Cacheable**: Response possono essere cachate
4. **Uniform Interface**: Interfaccia coerente
5. **Layered System**: Architettura a layer
6. **Code on Demand** (opzionale): Server può inviare codice

**In pratica, le regole importanti**:

---

### HTTP Verbs - CRUD Mapping ⚠️ DOMANDA COMUNE

**Domanda**: "Differenza tra PUT e PATCH?"

| Verb | Operazione | Idempotente? | Uso |
| --- | --- | --- | --- |
| **GET** | Read | ✅ Sì | Recupera risorsa |
| **POST** | Create | ❌ No | Crea nuova risorsa |
| **PUT** | Update (full) | ✅ Sì | Sostituisce intera risorsa |
| **PATCH** | Update (partial) | ❌ No* | Modifica parte risorsa |
| **DELETE** | Delete | ✅ Sì | Elimina risorsa |

**Idempotente** = Chiamare N volte ha stesso effetto di chiamare 1 volta.

#### **GET - Recuperare Dati**

```csharp
[HttpGet] // GET /api/users
public async Task<ActionResult<List<UserDto>>> GetUsers()
{
    var users = await _context.Users.ToListAsync();
    return Ok(users);
}

[HttpGet("{id}")] // GET /api/users/5
public async Task<ActionResult<UserDto>> GetUser(int id)
{
    var user = await _context.Users.FindAsync(id);
    if (user == null)
        return NotFound(); // 404
    
    return Ok(user); // 200
}
```

**Caratteristiche GET**:

- **Mai** modifica dati (safe)
- Può essere cachato
- Parametri in query string: `/api/users?role=admin&status=active`

---

#### **POST - Creare Risorsa**

```csharp
[HttpPost] // POST /api/users
public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto dto)
{
    var user = new User { Name = dto.Name, Email = dto.Email };
    _context.Users.Add(user);
    await _context.SaveChangesAsync();
    
    // 201 Created + Location header
    return CreatedAtAction(
        nameof(GetUser), 
        new { id = user.Id }, 
        user);
}
```

**Response**:

```
HTTP/1.1 201 Created
Location: /api/users/123
Content-Type: application/json

{"id": 123, "name": "John", "email": "john@test.com"}
```

**Caratteristiche POST**:

- **NON idempotente**: Chiamare 2 volte → 2 risorse create
- Status code: **201 Created**
- Header `Location`: URL della risorsa creata

---

#### **PUT vs PATCH - La Differenza Cruciale**

**PUT - Sostituzione COMPLETA**:

```csharp
[HttpPut("{id}")] // PUT /api/users/5
public async Task<IActionResult> UpdateUser(int id, UserDto dto)
{
    var user = await _context.Users.FindAsync(id);
    if (user == null)
        return NotFound();
    
    // Sostituisce TUTTI i campi
    user.Name = dto.Name;
    user.Email = dto.Email;
    user.Role = dto.Role;
    user.Status = dto.Status;
    // Se dto non ha un campo, va settato comunque (es. null)
    
    await _context.SaveChangesAsync();
    return NoContent(); // 204
}
```

**Body**:

```json
{
  "name": "John Updated",
  "email": "john.new@test.com",
  "role": "Admin",
  "status": "Active"
}
```

**PATCH - Modifica PARZIALE**:

```csharp
[HttpPatch("{id}")] // PATCH /api/users/5
public async Task<IActionResult> PatchUser(int id, JsonPatchDocument<User> patch)
{
    var user = await _context.Users.FindAsync(id);
    if (user == null)
        return NotFound();
    
    patch.ApplyTo(user);
    await _context.SaveChangesAsync();
    return NoContent();
}
```

**Body (JSON Patch)**:

```json
[
  { "op": "replace", "path": "/email", "value": "john.new@test.com" }
]
```

Modifica **solo** email, lascia gli altri campi invariati.

**Quando usare cosa**:

- **PUT**: Aggiornamento completo (form intero)
- **PATCH**: Aggiornamento parziale (singolo campo, es. toggle status)

---

#### **DELETE - Eliminare Risorsa**

```csharp
[HttpDelete("{id}")] // DELETE /api/users/5
public async Task<IActionResult> DeleteUser(int id)
{
    var user = await _context.Users.FindAsync(id);
    if (user == null)
        return NotFound(); // 404
    
    _context.Users.Remove(user);
    await _context.SaveChangesAsync();
    return NoContent(); // 204
}
```

**Caratteristiche DELETE**:

- **Idempotente**: Chiamare 2 volte → Stessa situazione (risorsa eliminata)
- Prima chiamata: 204 No Content
- Seconda chiamata: 404 Not Found (ma stato finale identico)

---

### Status Codes - Comunicare Risultati ⚠️ IMPORTANTE

**Devi conoscere questi a memoria**:

| Code | Significato | Quando usare |
| --- | --- | --- |
| **200 OK** | Success | GET, PUT, PATCH con body |
| **201 Created** | Created | POST riuscito |
| **204 No Content** | Success no body | DELETE, PUT senza ritorno |
| **400 Bad Request** | Input invalido | Validazione fallita |
| **401 Unauthorized** | Not authenticated | Token mancante/invalido |
| **403 Forbidden** | Not authorized | Autenticato ma no permessi |
| **404 Not Found** | Risorsa non esiste | GET/PUT/DELETE su ID inesistente |
| **409 Conflict** | Conflitto | Duplicate, concurrent update |
| **500 Internal Error** | Server error | Exception non gestita |

**Errore comune - 401 vs 403**:

```csharp
// User NON loggato
if (user == null)
    return Unauthorized(); // 401 "You must login"

// User loggato ma non è admin
if (user.Role != "Admin")
    return Forbid(); // 403 "You can't do this"
```

---

### Model Binding e Validation

#### **Model Binding Automatico**

```csharp
// ASP.NET Core lega automaticamente
[HttpGet]
public IActionResult Search(
    [FromQuery] string name,      // Da query string
    [FromQuery] int? age,          // Opzionale
    [FromHeader] string apiKey)    // Da header
{
    // ...
}

[HttpPost]
public IActionResult Create(
    [FromBody] CreateUserDto dto,  // Da JSON body
    [FromRoute] int id)            // Da URL route
{
    // ...
}
```

**Sources**:

- `[FromQuery]`: Query string (`?name=John&age=30`)
- `[FromRoute]`: URL template (`/api/users/{id}`)
- `[FromBody]`: Request body (JSON)
- `[FromHeader]`: HTTP headers
- `[FromForm]`: Form data

#### **Data Annotations Validation**

```csharp
public class CreateUserDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Range(18, 120)]
    public int Age { get; set; }
    
    [RegularExpression(@"^\+?[1-9]\d{1,14}$")]
    public string Phone { get; set; }
}

[HttpPost]
public IActionResult Create(CreateUserDto dto)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState); // 400 con errori
    }
    
    // Validazione OK, procedi
}
```

**Response su validazione fallita**:

```json
{
  "errors": {
    "Name": ["Name is required"],
    "Email": ["The Email field is not a valid email address."]
  }
}
```

---

### API Versioning - Evolvere senza Rompere

**Problema**: Devi cambiare API ma client vecchi devono funzionare.

**Soluzioni**:

#### **1. URL Versioning** (più comune)

```csharp
[ApiController]
[Route("api/v1/[controller]")]
public class UsersV1Controller : ControllerBase
{
    [HttpGet]
    public IActionResult GetUsers() => Ok("V1");
}

[ApiController]
[Route("api/v2/[controller]")]
public class UsersV2Controller : ControllerBase
{
    [HttpGet]
    public IActionResult GetUsers() => Ok("V2 with new fields");
}
```

#### **2. Header Versioning**

```csharp
// Request
GET /api/users
Api-Version: 2.0

// Package: Microsoft.AspNetCore.Mvc.Versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

[ApiVersion("1.0")]
[ApiVersion("2.0")]
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    [HttpGet]
    [MapToApiVersion("1.0")]
    public IActionResult GetUsersV1() => Ok("V1");
    
    [HttpGet]
    [MapToApiVersion("2.0")]
    public IActionResult GetUsersV2() => Ok("V2");
}
```

---

## 4. Authentication & Authorization {#autenticazione}

### Authentication vs Authorization - La Differenza

**Authentication** (Autenticazione): **CHI sei?**

- Login
- Verificare identità
- Token/Cookie

**Authorization** (Autorizzazione): **COSA puoi fare?**

- Permessi
- Ruoli
- Policies

**Analogia**: 

- Authentication = Mostrare ID all'ingresso edificio
- Authorization = Badge decide in quali stanze puoi entrare

---

### JWT (JSON Web Token) - Il Metodo Moderno ⚠️ IMPORTANTE

**Cos'è**: Token firmato digitalmente che contiene claims (informazioni utente).

#### **Struttura JWT**

```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c
```

**3 parti separate da `.`**:

1. **Header** (algoritmo):

```json
{"alg": "HS256", "typ": "JWT"}
```

1. **Payload** (claims):

```json
{
  "sub": "1234567890",
  "name": "John Doe",
  "role": "Admin",
  "exp": 1516239022
}
```

1. **Signature** (firma digitale):

```
HMAC-SHA256(
  base64(header) + "." + base64(payload),
  secret_key
)
```

#### **Implementazione in [ASP.NET](http://ASP.NET) Core**

**1. Configurazione (Program.cs)**:

```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configurazione JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// ⚠️ ORDINE CRITICO
app.UseAuthentication(); // PRIMA
app.UseAuthorization();  // DOPO

app.MapControllers();
app.Run();
```

**appsettings.json**:

```json
{
  "Jwt": {
    "Key": "SuperSecretKeyMinimum32CharactersLong!!",
    "Issuer": "MyApp",
    "Audience": "MyAppUsers",
    "ExpirationMinutes": 60
  }
}
```

**2. Generare Token (Login)**:

```csharp
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

[HttpPost("login")]
public IActionResult Login(LoginDto dto)
{
    // Valida credenziali
    var user = _context.Users
        .FirstOrDefault(u => u.Email == dto.Email);
    
    if (user == null || !VerifyPassword(dto.Password, user.PasswordHash))
        return Unauthorized(); // 401
    
    // Crea claims
    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role)
    };
    
    // Crea token
    var key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    
    var token = new JwtSecurityToken(
        issuer: _config["Jwt:Issuer"],
        audience: _config["Jwt:Audience"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(60),
        signingCredentials: creds);
    
    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
    
    return Ok(new { token = tokenString });
}
```

**3. Proteggere Endpoints**:

```csharp
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    // Richiede autenticazione (qualsiasi utente loggato)
    [Authorize]
    [HttpGet]
    public IActionResult GetMyOrders()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // ...
    }
    
    // Richiede ruolo specifico
    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public IActionResult GetAllOrders()
    {
        // Solo admin
    }
    
    // Multiple ruoli
    [Authorize(Roles = "Admin,Manager")]
    [HttpPost]
    public IActionResult CreateOrder()
    {
        // Admin O Manager
    }
}
```

**4. Client usa Token**:

```jsx
// JavaScript
fetch('/api/orders', {
  headers: {
    'Authorization': 'Bearer eyJhbGci...',
    'Content-Type': 'application/json'
  }
})
```

---

### Policy-Based Authorization - Più Flessibile

**Scenario**: Regole complesse (es. "Può modificare ordine solo se è suo O è admin").

**Configurazione**:

```csharp
builder.Services.AddAuthorization(options =>
{
    // Policy: Minimum Age
    options.AddPolicy("MinimumAge18", policy =>
        policy.RequireClaim("Age", "18", "19", "20" /* ... */));
    
    // Policy: Admin o Manager
    options.AddPolicy("AdminOrManager", policy =>
        policy.RequireRole("Admin", "Manager"));
    
    // Policy: Custom requirement
    options.AddPolicy("CanEditOrder", policy =>
        policy.Requirements.Add(new CanEditOrderRequirement()));
});
```

**Uso**:

```csharp
[Authorize(Policy = "AdminOrManager")]
[HttpGet("reports")]
public IActionResult GetReports() { }
```

**Custom Authorization Handler**:

```csharp
public class CanEditOrderRequirement : IAuthorizationRequirement { }

public class CanEditOrderHandler : AuthorizationHandler<CanEditOrderRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CanEditOrderRequirement requirement)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var isAdmin = context.User.IsInRole("Admin");
        
        var orderId = _httpContextAccessor.HttpContext?
            .Request.RouteValues["orderId"]?.ToString();
        
        // Logica custom: Admin può sempre, User solo se è suo ordine
        if (isAdmin || IsUserOrder(userId, orderId))
        {
            context.Succeed(requirement);
        }
        
        return Task.CompletedTask;
    }
}

// Registrazione
builder.Services.AddSingleton<IAuthorizationHandler, CanEditOrderHandler>();
```

---

# PARTE 4: CLEAN ARCHITECTURE & SOLID ⚠️ IMPORTANTE

## 1. SOLID Principles - I Pilastri del Design {#solid}

**SOLID** è un acronimo di 5 principi di design object-oriented.

### S - Single Responsibility Principle

**Principio**: "Una classe dovrebbe avere UNA sola ragione per cambiare."

**❌ MALE - Multiple Responsibilities**:

```csharp
public class UserService
{
    public void CreateUser(User user)
    {
        // 1. Validazione
        if (string.IsNullOrEmpty(user.Email))
            throw new Exception("Invalid email");
        
        // 2. Business logic
        user.CreatedAt = DateTime.Now;
        
        // 3. Database
        _context.Users.Add(user);
        _context.SaveChanges();
        
        // 4. Email
        var smtp = new SmtpClient("smtp.gmail.com");
        smtp.Send("Welcome!", user.Email);
        
        // 5. Logging
        File.AppendAllText("log.txt", $"User {user.Email} created");
    }
}
```

**Problemi**: 5 ragioni per cambiare questa classe!

**✅ BENE - Single Responsibility**:

```csharp
public class UserService
{
    private readonly IUserRepository _repository;
    private readonly IEmailService _emailService;
    private readonly ILogger<UserService> _logger;
    private readonly IValidator<User> _validator;
    
    public async Task CreateUserAsync(User user)
    {
        _validator.Validate(user); // 1 responsabilità
        user.CreatedAt = DateTime.Now;
        
        await _repository.AddAsync(user); // 1 responsabilità
        await _emailService.SendWelcomeEmail(user); // 1 responsabilità
        _logger.LogInformation($"User {user.Email} created"); // 1 responsabilità
    }
}
```

---

### O - Open/Closed Principle

**Principio**: "Aperto per estensione, chiuso per modifica."

**❌ MALE - Modifica classe esistente**:

```csharp
public class PaymentProcessor
{
    public void ProcessPayment(string type, decimal amount)
    {
        if (type == "CreditCard")
        {
            // Logica carta credito
        }
        else if (type == "PayPal")
        {
            // Logica PayPal
        }
        else if (type == "Crypto") // Nuovo metodo → MODIFICA classe
        {
            // Logica crypto
        }
    }
}
```

**✅ BENE - Estensione tramite interface**:

```csharp
public interface IPaymentMethod
{
    Task ProcessAsync(decimal amount);
}

public class CreditCardPayment : IPaymentMethod
{
    public async Task ProcessAsync(decimal amount) { }
}

public class PayPalPayment : IPaymentMethod
{
    public async Task ProcessAsync(decimal amount) { }
}

public class CryptoPayment : IPaymentMethod // NUOVA classe, NO modifica
{
    public async Task ProcessAsync(decimal amount) { }
}

public class PaymentProcessor
{
    public async Task ProcessAsync(IPaymentMethod method, decimal amount)
    {
        await method.ProcessAsync(amount);
    }
}
```

---

### L - Liskov Substitution Principle

**Principio**: "Le sottoclassi devono essere sostituibili alle classi base."

**❌ MALE - Viola LSP**:

```csharp
public class Rectangle
{
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }
    
    public int Area() => Width * Height;
}

public class Square : Rectangle
{
    public override int Width
    {
        set { base.Width = base.Height = value; } // PROBLEMA!
    }
    
    public override int Height
    {
        set { base.Width = base.Height = value; } // PROBLEMA!
    }
}

// Test
Rectangle rect = new Square();
rect.Width = 5;
rect.Height = 10;
Console.WriteLine(rect.Area()); // Aspetti 50, ottieni 100!
```

**✅ BENE - Interfaccia comune**:

```csharp
public interface IShape
{
    int Area();
}

public class Rectangle : IShape
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int Area() => Width * Height;
}

public class Square : IShape
{
    public int Side { get; set; }
    public int Area() => Side * Side;
}
```

---

### I - Interface Segregation Principle

**Principio**: "Client non dovrebbero dipendere da interface che non usano."

**❌ MALE - Interface "Grassa"**:

```csharp
public interface IWorker
{
    void Work();
    void Eat();
    void Sleep();
}

public class HumanWorker : IWorker
{
    public void Work() { }
    public void Eat() { }
    public void Sleep() { }
}

public class RobotWorker : IWorker
{
    public void Work() { }
    public void Eat() { throw new NotImplementedException(); } // !
    public void Sleep() { throw new NotImplementedException(); } // !
}
```

**✅ BENE - Interface Segregate**:

```csharp
public interface IWorkable
{
    void Work();
}

public interface IEatable
{
    void Eat();
}

public interface ISleepable
{
    void Sleep();
}

public class HumanWorker : IWorkable, IEatable, ISleepable
{
    public void Work() { }
    public void Eat() { }
    public void Sleep() { }
}

public class RobotWorker : IWorkable // Solo ciò che serve
{
    public void Work() { }
}
```

---

### D - Dependency Inversion Principle

**Principio**: "Dipendi da astrazioni, non da implementazioni."

**❌ MALE - Dipende da implementazione**:

```csharp
public class OrderService
{
    private readonly SqlServerRepository _repository; // Concrete!
    
    public OrderService()
    {
        _repository = new SqlServerRepository();
    }
}
```

**✅ BENE - Dipende da astrazione**:

```csharp
public interface IOrderRepository
{
    Task<Order> GetByIdAsync(int id);
}

public class OrderService
{
    private readonly IOrderRepository _repository; // Abstraction!
    
    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }
}

// Configurazione
builder.Services.AddScoped<IOrderRepository, SqlServerRepository>();
// Domani: AddScoped<IOrderRepository, MongoRepository>(); // Cambio facile
```

---

## 3. Design Patterns — Soluzioni a Problemi Ricorrenti {#patterns}

> **Regola d'oro del colloquio:** Non memorizzare i pattern. Capisci **il problema che risolvono** e **quando usarli**. Chi spiega solo il nome del pattern senza il problema che risolve, fallisce.
> 

---

### Repository Pattern — Separa Logica Dati dalla Business Logic

**Il problema senza Repository:**

```csharp
public class OrderService
{
    private readonly AppDbContext _context; // Dipende direttamente da EF

    public async Task<Order> GetOrderAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefault(o => o.Id == id);
        // Logica di accesso dati mescolata con business logic
    }
}
```

**Problemi:** Non testabile senza DB, logica di query sparsa ovunque, cambio ORM = cambia tutto.

**Con Repository:**

```csharp
// Interface (nel Domain/Application layer)
public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id);
    Task<List<Order>> GetByCustomerIdAsync(int customerId);
    Task<Order> AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(int id);
}

// Implementazione (nel Infrastructure layer)
public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Order>> GetByCustomerIdAsync(int customerId)
    {
        return await _context.Orders
            .Where(o => o.CustomerId == customerId)
            .ToListAsync();
    }

    // ... altri metodi
}

// Service usa INTERFACE, non implementazione
public class OrderService
{
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository)
    {
        _repository = repository; // Loose coupling!
    }

    public async Task<Order?> GetOrderAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
}

// Test: inietta mock
public class OrderServiceTests
{
    [Fact]
    async Task GetOrder_ReturnsOrder()
    {
        var mockRepo = new Mock<IOrderRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Order { Id = 1 });

        var service = new OrderService(mockRepo.Object);
        var order = await service.GetOrderAsync(1);

        Assert.NotNull(order);
        Assert.Equal(1, order.Id);
    }
}
```

**Generic Repository (più DRY):**

```csharp
public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly DbSet<T> _dbSet;

    public Repository<T>(AppDbContext context)
    {
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }
    // ... implementazione generica
}

// Uso specifico con metodi custom
public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email); // Metodo specifico
}
```

---

### Strategy Pattern — Algoritmi Intercambiabili

**Il problema senza Strategy:**

```csharp
public class ShippingCalculator
{
    public decimal CalculateShipping(string method, decimal weight)
    {
        if (method == "Standard")
            return weight * 1.5m;
        else if (method == "Express")
            return weight * 3.0m + 5.0m;
        else if (method == "Overnight")
            return weight * 5.0m + 15.0m;
        // Ogni nuovo metodo = modifica questa classe (viola Open/Closed)
    }
}
```

**Con Strategy:**

```csharp
// Strategy Interface
public interface IShippingStrategy
{
    decimal Calculate(decimal weight);
    string Name { get; }
}

// Strategie concrete
public class StandardShipping : IShippingStrategy
{
    public string Name => "Standard";
    public decimal Calculate(decimal weight) => weight * 1.5m;
}

public class ExpressShipping : IShippingStrategy
{
    public string Name => "Express";
    public decimal Calculate(decimal weight) => weight * 3.0m + 5.0m;
}

public class OvernightShipping : IShippingStrategy
{
    public string Name => "Overnight";
    public decimal Calculate(decimal weight) => weight * 5.0m + 15.0m;
}

// Context che usa la strategy
public class ShippingCalculator
{
    public decimal CalculateShipping(IShippingStrategy strategy, decimal weight)
    {
        return strategy.Calculate(weight);
    }
}

// Registrazione in DI
builder.Services.AddTransient<IShippingStrategy, StandardShipping>();
// oppure uso un factory per selezionare la strategy
```

**Quando usare Strategy:**

- Hai una famiglia di algoritmi che fanno la stessa cosa ma in modo diverso
- Vuoi cambiare l'algoritmo a runtime
- Hai if/else chains che crescono con nuovi casi

---

### Factory Pattern — Creazione Senza Conoscere la Classe Concreta

**Il problema:** Il codice client deve creare oggetti ma non dovrebbe conoscere le classi concrete.

```csharp
// Prodotti
public interface INotification
{
    Task SendAsync(string recipient, string message);
}

public class EmailNotification : INotification
{
    public async Task SendAsync(string recipient, string message)
    {
        // Logica invio email
    }
}

public class SmsNotification : INotification
{
    public async Task SendAsync(string recipient, string message)
    {
        // Logica invio SMS
    }
}

public class PushNotification : INotification
{
    public async Task SendAsync(string recipient, string message)
    {
        // Logica push notification
    }
}

// Factory
public class NotificationFactory
{
    public static INotification Create(string type)
    {
        return type switch
        {
            "email" => new EmailNotification(),
            "sms" => new SmsNotification(),
            "push" => new PushNotification(),
            _ => throw new ArgumentException($"Unknown type: {type}")
        };
    }
}

// Uso — non sa e non deve sapere concrete classes
public class NotificationService
{
    public async Task NotifyAsync(string type, string recipient, string message)
    {
        var notification = NotificationFactory.Create(type);
        await notification.SendAsync(recipient, message);
    }
}
```

**Abstract Factory — Factory di Factory (quando hai famiglie di oggetti correlati):**

```csharp
// Quando devi creare famiglie di oggetti correlati
public interface IButton { void Render(); }
public interface ICheckbox { void Render(); }

public interface IGUIFactory
{
    IButton CreateButton();
    ICheckbox CreateCheckbox();
}

// Windows Family
public class WindowsFactory : IGUIFactory
{
    public IButton CreateButton() => new WindowsButton();
    public ICheckbox CreateCheckbox() => new WindowsCheckbox();
}

// macOS Family
public class MacFactory : IGUIFactory
{
    public IButton CreateButton() => new MacButton();
    public ICheckbox CreateCheckbox() => new MacCheckbox();
}
```

---

### Observer Pattern — Notifica Automatica degli Eventi

**Il problema:** Quando algo cambia, diversi parti del sistema devono essere notificate. Non devono conoscersi.

```csharp
// Publisher (Observable)
public interface IEventPublisher
{
    void Subscribe<T>(Action<T> handler);
    void Publish<T>(T eventData);
}

public class EventPublisher : IEventPublisher
{
    private readonly Dictionary<Type, List<Delegate>> _handlers = new();

    public void Subscribe<T>(Action<T> handler)
    {
        if (!_handlers.ContainsKey(typeof(T)))
            _handlers[typeof(T)] = new List<Delegate>();
        _handlers[typeof(T)].Add(handler);
    }

    public void Publish<T>(T eventData)
    {
        if (_handlers.ContainsKey(typeof(T)))
        {
            foreach (var handler in _handlers[typeof(T)])
            {
                ((Action<T>)handler)(eventData);
            }
        }
    }
}

// Events
public record OrderCreatedEvent(int OrderId, string CustomerName, decimal Total);
public record UserRegisteredEvent(string Email, string Name);

// Subscribers — non sanno degli altri
public class EmailService
{
    public EmailService(IEventPublisher publisher)
    {
        publisher.Subscribe<OrderCreatedEvent>(OnOrderCreated);
        publisher.Subscribe<UserRegisteredEvent>(OnUserRegistered);
    }

    private void OnOrderCreated(OrderCreatedEvent e)
    {
        // Invia email di conferma ordine
    }

    private void OnUserRegistered(UserRegisteredEvent e)
    {
        // Invia email di benvenuto
    }
}

public class AnalyticsService
{
    public AnalyticsService(IEventPublisher publisher)
    {
        publisher.Subscribe<OrderCreatedEvent>(OnOrderCreated);
    }

    private void OnOrderCreated(OrderCreatedEvent e)
    {
        // Track analytics
    }
}

// Publisher non sa quanti subscriber ci sono
public class OrderService
{
    private readonly IEventPublisher _publisher;

    public async Task CreateOrderAsync(Order order)
    {
        await _repository.AddAsync(order);
        
        // Notifica tutti i subscriber
        _publisher.Publish(new OrderCreatedEvent(
            order.Id, order.CustomerName, order.Total));
    }
}
```

**Quando usare Observer:**

- Un evento deve notificare parti diverse del sistema
- Accoppiamento debole tra producer e consumers
- Numero di subscribers non è noto a compile-time

---

### Singleton Pattern — Una Sola Istanza

**Attenzione:** In [ASP.NET](http://ASP.NET) Core con DI, raramente devi implementare Singleton manualmente. Usa `AddSingleton<>()`. Ma devi capire il pattern.

```csharp
public class Configuration
{
    private static Configuration? _instance;
    private static readonly object _lock = new object();
    private readonly Dictionary<string, string> _settings;

    private Configuration() // Costruttore privato!
    {
        _settings = LoadSettings();
    }

    public static Configuration Instance
    {
        get
        {
            if (_instance == null) // Double-check locking
            {
                lock (_lock)
                {
                    _instance ??= new Configuration();
                }
            }
            return _instance;
        }
    }

    // C# moderno — più semplice e thread-safe
    // public static readonly Configuration Instance = new();
}
```

**In [ASP.NET](http://ASP.NET) Core — modo corretto:**

```csharp
// Non serve implementazione manuale!
builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();
// Il DI container gestisce la singola istanza
```

---

### Decorator Pattern — Aggiungi Comportamento senza Modificare

**Il problema:** Hai una classe e vuoi aggiungere funzionalità (logging, caching, auth) senza modificarla.

```csharp
// Component interface
public interface IDataService
{
    Task<List<Item>> GetItemsAsync();
}

// Implementazione base
public class DataService : IDataService
{
    public async Task<List<Item>> GetItemsAsync()
    {
        return await _repository.GetAllAsync();
    }
}

// Decorator: aggiunge caching
public class CachingDataService : IDataService
{
    private readonly IDataService _inner; // Wraps another IDataService
    private readonly IMemoryCache _cache;

    public CachingDataService(IDataService inner, IMemoryCache cache)
    {
        _inner = inner;
        _cache = cache;
    }

    public async Task<List<Item>> GetItemsAsync()
    {
        if (_cache.TryGetValue("items", out List<Item> cached))
            return cached;

        var items = await _inner.GetItemsAsync(); // Chiama il wrapped service
        _cache.Set("items", items, TimeSpan.FromMinutes(5));
        return items;
    }
}

// Decorator: aggiunge logging
public class LoggingDataService : IDataService
{
    private readonly IDataService _inner;
    private readonly ILogger _logger;

    public LoggingDataService(IDataService inner, ILogger logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<List<Item>> GetItemsAsync()
    {
        _logger.LogInformation("Getting items...");
        var items = await _inner.GetItemsAsync();
        _logger.LogInformation($"Got {items.Count} items");
        return items;
    }
}

// Composizione: LoggingDataService wraps CachingDataService wraps DataService
builder.Services
    .AddScoped<IDataService, DataService>()
    .Decorate<IDataService, CachingDataService>()
    .Decorate<IDataService, LoggingDataService>();
```

**Quando usare Decorator:**

- Aggiungi responsabilità dinamicamente
- Composizione vs ereditarietà
- Cross-cutting concerns (logging, caching, monitoring)

---

## 2. Clean Architecture - Separazione delle Responsabilità {#clean-arch}

### I Layer di Clean Architecture

```
┌─────────────────────────────────────┐
│       Presentation Layer            │  ← Controllers, Views
│    (ASP.NET Core API/MVC)           │
└─────────────────────────────────────┘
            ↓ depends on
┌─────────────────────────────────────┐
│      Application Layer              │  ← Use Cases, DTOs
│   (Business Logic, Services)        │
└─────────────────────────────────────┘
            ↓ depends on
┌─────────────────────────────────────┐
│        Domain Layer                 │  ← Entities, Interfaces
│     (Core Business Rules)           │
└─────────────────────────────────────┘
            ↑ implemented by
┌─────────────────────────────────────┐
│    Infrastructure Layer             │  ← DB, APIs, Email
│  (EF Core, External Services)       │
└─────────────────────────────────────┘
```

**Regola d'oro**: Le dipendenze puntano **verso l'interno** (verso Domain).

---

---

# PARTE 6: INFRASTRUTTURA E SISTEMI DISTRIBUITI

## 1. Redis — Caching e Performance {#redis}

### Cos'è Redis?

**Redis** = **R**emote **Di**ctionary **S**erver. Un data store **in-memory**, key-value, usato perché è **estremamente veloce** (operazioni sub-millisecond).

**Usi principali:**

- Caching (il caso più comune)
- Session management
- Rate limiting
- Pub/Sub
- Queue leggera

### Caching Strategies — Le Strategie Principali

#### **Cache-Aside (Lazy Loading) — Il Pattern Più Comune**

```
Client richiede dato
    ↓
  Controlla Cache (Redis)
    ↓ Hit? → Ritorna dato dalla cache
    ↓ Miss?
  Carica dal Database
    ↓
  Salva in Cache
    ↓
  Ritorna dato
```

```csharp
public class UserService
{
    private readonly IDistributedCache _cache;
    private readonly IUserRepository _repository;
    private const string CacheKeyPrefix = "user:";

    public async Task<UserDto> GetUserAsync(int id)
    {
        var cacheKey = $"{CacheKeyPrefix}{id}";

        // 1. Try cache first
        var cached = await _cache.GetStringAsync(cacheKey);
        if (cached != null)
        {
            return JsonSerializer.Deserialize<UserDto>(cached)!;
        }

        // 2. Cache miss → load from DB
        var user = await _repository.GetByIdAsync(id);
        if (user == null) return null;

        var dto = MapToDto(user);

        // 3. Store in cache with expiry
        await _cache.SetStringAsync(
            cacheKey,
            JsonSerializer.Serialize(dto),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });

        return dto;
    }

    // Invalida cache quando dato cambia
    public async Task UpdateUserAsync(UserDto dto)
    {
        await _repository.UpdateAsync(dto);
        await _cache.RemoveAsync($"{CacheKeyPrefix}{dto.Id}");
    }
}
```

#### **Write-Through — Scrivi Cache e DB Contemporaneamente**

```
Client scrive dato
    ↓
  Scrivi in Cache E nel Database
    ↓
  Cache sempre aggiornata (mai stale)
```

```csharp
public async Task CreateOrderAsync(Order order)
{
    // Scrivi nel DB
    await _repository.AddAsync(order);

    // Aggiortha la cache immeditamente
    var cacheKey = $"orders:customer:{order.CustomerId}";
    var orders = await GetOrdersFromCacheOrDbAsync(order.CustomerId);
    orders.Add(MapToDto(order));
    await _cache.SetStringAsync(cacheKey, Serialize(orders),
        new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        });
}
```

**Vantaggi:** Cache sempre coerente con DB.

**Svantaggio:** Overhead su ogni scrittura.

#### **Eviction Policies — Quando la Cache è Piena**

| Policy | Strategia | Quando usare |
| --- | --- | --- |
| `volatile-lru` | Rimuove key meno usata (con expiry) | **Default raccomandato** |
| `allkeys-lru` | Rimuove key meno usata (qualsiasi) | Se tutte le key sono uguali |
| `volatile-ttl` | Rimuove key con TTL più vicino alla scadenza | Priority-based caching |
| `noeviction` | Non rimuove, ritorna errore | Non usare in produzione |

### Redis in [ASP.NET](http://ASP.NET) Core

```csharp
// Program.cs
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
    options.InstanceName = "MyApp:";
});

// appsettings.json
// "Redis": { "ConnectionString": "localhost:6379" }

// Injection
public class MyService
{
    private readonly IDistributedCache _cache;

    public MyService(IDistributedCache cache)
    {
        _cache = cache;
    }
}
```

### Cache Key Strategy — Pattern Important

```csharp
// Pattern: "entità:identificatore"
// Consistente, leggibile, facile da invalidare

var userKey = $"user:{userId}";           // Singolo user
var ordersKey = $"orders:user:{userId}"; // Orders di un user
var dashboardKey = $"dashboard:{date:yyyy-MM-dd}"; // Dashboard del giorno

// Invalidazione per pattern (rimuovi tutto per un user)
await _cache.RemoveAsync($"user:{userId}");
await _cache.RemoveAsync($"orders:user:{userId}");
```

---

## 2. Messaging — RabbitMQ vs Kafka {#messaging}

### Message Broker — Perché Serve

**Problema senza messaging:**

```
OrderService → direttamente chiama → EmailService
                                    → InventoryService
                                    → AnalyticsService
```

Se EmailService cade, OrderService fallisce. Tight coupling tra tutti i servizi.

**Con Message Broker:**

```
OrderService → pubblica messaggio → [Message Broker]
                                          ↓    ↓    ↓
                                    EmailService  InventoryService  AnalyticsService
                                    (subscriber)  (subscriber)      (subscriber)
```

OrderService non sa e non cura quanti consumer ci sono. Se EmailService cade, il messaggio resta in coda.

### RabbitMQ — Traditional Message Broker

**Concetti fondamentali:**

```
Producer → [Exchange] → routing → [Queue] → Consumer
```

- **Exchange:** Riceve messaggi e li smistano alle code (routing logic)
- **Queue:** Memorizza i messaggi finché non vengono consumati
- **Binding:** Regola che collega Exchange a Queue

**Tipi di Exchange:**

| Tipo | Routing Logic | Uso |
| --- | --- | --- |
| **Direct** | Exact match sulla routing key | Task distribution |
| **Fanout** | Broadcast a tutte le code | Notifica tutti i subscriber |
| **Topic** | Pattern matching (`user.*`, `order.#`) | Event bus flessibile |
| **Headers** | Match su header del messaggio | Routing complesso |

**Patterns principali:**

```
// Work Queue (Task Queue)
// Multiple consumer, ogni messaggio processato da UN SOLO consumer
// Usato per: load balancing di task pesanti

Producer → [Queue] → Consumer 1
                   → Consumer 2  (round-robin)
                   → Consumer 3

// Pub/Sub
// Un messaggio va a TUTTI i subscriber
// Usato per: notifiche, event-driven architecture

Producer → [Exchange:Fanout] → Queue 1 → Consumer 1
                              → Queue 2 → Consumer 2
                              → Queue 3 → Consumer 3
```

**In [ASP.NET](http://ASP.NET) Core (RabbitMQ.Client):**

```csharp
// Publisher
public class OrderPublisher
{
    private readonly IConnection _connection;

    public async Task PublishOrderCreatedAsync(OrderCreatedEvent order)
    {
        using var channel = _connection.CreateModel();
        channel.ExchangeDeclare("orders", "topic", durable: true);

        var body = Encoding.UTF8.GetBytes(
            JsonSerializer.Serialize(order));

        channel.BasicPublish(
            exchange: "orders",
            routingKey: "order.created",
            properties: channel.CreateBasicProperties { Persistent = true },
            body: body);
    }
}

// Consumer (in Background Service)
public class OrderConsumer : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken ct)
    {
        using var channel = _connection.CreateModel();
        channel.QueueDeclare("order-email-queue", durable: true);
        channel.QueueBind("order-email-queue", "orders", "order.created");

        channel.BasicConsume(
            queue: "order-email-queue",
            autoAck: false, // Manual acknowledgement
            consumer: new EventingBasicConsumer(channel)
            {
                Received = (sender, ea) =>
                {
                    try
                    {
                        var order = JsonSerializer.Deserialize<OrderCreatedEvent>(
                            ea.Body.Span);
                        SendConfirmationEmail(order);
                        channel.BasicAck(ea.DeliveryTag, false); // Successo
                    }
                    catch
                    {
                        channel.BasicNack(ea.DeliveryTag, false, true); // Requeue
                    }
                }
            });

        return Task.Delay(Timeout.Infinite, ct);
    }
}
```

### Kafka — Distributed Event Streaming

**Differenza con RabbitMQ:** Kafka non è un traditional message broker, è un **event streaming platform**.

**Concetti fondamentali:**

```
Producer → [Topic] → [Partition 0] → Consumer Group
                   → [Partition 1] → Consumer Group
                   → [Partition 2] → Consumer Group
```

- **Topic:** Canale logico per tipo di evento ("orders", "users")
- **Partition:** Divisione di un topic per scalabilità e parallelismo
- **Offset:** Posizione di un messaggio in una partition (mai rimossi!)
- **Consumer Group:** Grupp di consumer che condividono il lavoro

### RabbitMQ vs Kafka — La Decisione

| Aspetto | RabbitMQ | Kafka |
| --- | --- | --- |
| **Tipo** | Message Broker | Event Streaming Platform |
| **Messaggi** | Rimossi dopo consumo | Ritenuti per tempo configurato |
| **Throughput** | Alto | Molto alto (milioni/sec) |
| **Uso principale** | Task distribution, RPC | Event sourcing, analytics, log aggregation |
| **Quando usare** | Comunicazione tra servizi, workflow | Streaming, audit trail, event replay |
| **Complessità** | Media | Alta |

**Regola pratica:**

- **RabbitMQ** se hai bisogno di message routing, workflow, task queue
- **Kafka** se hai bisogno di event streaming, alto throughput, replay degli eventi

---

## 3. Microservices — Architettura Distribuita {#microservices}

### Monolito vs Microservizi — Trade-offs

```
// MONOLITO
┌─────────────────────────────┐
│  OrderService               │
│  UserService                │
│  PaymentService             │  → Un solo deployment
│  NotificationService        │
│  [Shared Database]          │
└─────────────────────────────┘

// MICROSERVIZI
┌───────────┐  ┌───────────┐  ┌───────────┐
│OrderSvc   │  │UserSvc    │  │PaymentSvc │
│[Own DB]   │  │[Own DB]   │  │[Own DB]   │
└─────┬─────┘  └─────┬─────┘  └─────┬─────┘
      │               │               │
      └───────────────┼───────────────┘
              [Message Broker / API]
```

**Quando usare Microservizi (non sempre è la risposta!):**

- Equipaggi diversi lavorano su parti diverse
- Scaling indipendente dei servizi
- Deploy indipendente
- Team grandi (il monolito diventa "Big Ball of Mud")

**Pericoli microservizi:**

- Latenza di rete tra servizi
- Consistenza dati distribuita
- Complessità operativa
- Debugging più difficile

### Comunicazione tra Microservizi

#### **Sincrona — HTTP/gRPC**

```
OrderService → HTTP GET /users/{id} → UserService
```

- **HTTP/REST:** Semplice, universale
- **gRPC:** Più veloce (Protocol Buffers, HTTP/2), tipizzato

```csharp
// gRPC in ASP.NET Core
public class UserGrpcClient
{
    private readonly UserServiceGrpc.UserServiceClient _client;

    public async Task<UserResponse> GetUserAsync(int id)
    {
        return await _client.GetUserAsync(new UserRequest { Id = id });
    }
}
```

#### **Asincrona — Message Broker**

```
OrderService → pubblica "OrderCreated" → [Kafka/RabbitMQ]
                                                 ↓
                                          EmailService (subscriber)
                                          InventoryService (subscriber)
```

**Quando sincrona vs asincrona:**

- **Sincrona:** Serve il risultato subito ("Dammi i dati di questo user")
- **Asincrona:** Fire-and-forget, side effects ("L'ordine è stato creato, notifica")

### API Gateway — Punto d'ingresso Unico

```
Client → [API Gateway] → OrderService
                        → UserService
                        → PaymentService
```

**Responsabilità dell'API Gateway:**

- Routing delle request
- Authentication/Authorization centralizzata
- Rate limiting
- Load balancing
- Aggregazione di risposte da servizi multipli

### Service Discovery — Come Trovare i Servizi

**Il problema:** In un ambiente di microservizi, gli indirizzo dei servizi cambiano (scaling, restart).

```
// Senza service discovery
var url = "http://user-service:3000"; // IP/porta hard-coded?

// Con service discovery (es. Consul, etcd, Kubernetes DNS)
var url = await _serviceDiscovery.GetServiceUrlAsync("user-service");
// Il sistema sa sempre dove trovare il servizio
```

### Resilience Patterns — Gestire i Falluri

```csharp
// Circuit Breaker — Se un servizio fallisce, smetti di chiamarlo
public class UserServiceClient
{
    private readonly HttpClient _httpClient;

    // Se 5 chiamate falliscono di consecutive, apri il circuito
    // Dopo 30 secondi, prova di nuovo
    public async Task<User> GetUserAsync(int id)
    {
        // Con Polly (library raccomandata)
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .Retry(3, retryNumber =>
                Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, retryNumber))));

        var circuitBreakerPolicy = Policy
            .Handle<HttpRequestException>()
            .CircuitBreakerAsync(
                exceptionsBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30));

        var policyWrap = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);

        return await policyWrap.ExecuteAsync(async () =>
        {
            var response = await _httpClient.GetAsync($"api/users/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<User>();
        });
    }
}
```

**Patterns di resilience principali:**

- **Retry:** Riprova con backoff esponenziale
- **Circuit Breaker:** Smetti di chiamare servizi falliti
- **Timeout:** Non aspettare per sempre
- **Fallback:** Risultato di default quando tutto fallisce
- **Bulkhead:** Limita risorse per servizio (non un fallito prosciuga tutto)

---

# PARTE 7: QUALITA' E TESTING

## 1. Testing — Strategie e Filosofia {#testing}

### La Piramide dei Test

```
        /\             Unit Tests
       /  \            (veloce, isolato, tanti)
      /    \
     /      \          Integration Tests
    /        \         (medio, dipendenze reali)
   /          \
  /            \       E2E Tests
 /              \      (lento, sistema completo, pochi)
/________________\
```

### Unit Tests con xUnit e Moq

```csharp
// Il servizio da testare
public class OrderService
{
    private readonly IOrderRepository _repository;
    private readonly IPaymentService _paymentService;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IOrderRepository repository,
        IPaymentService paymentService,
        ILogger<OrderService> logger)
    {
        _repository = repository;
        _paymentService = paymentService;
        _logger = logger;
    }

    public async Task<Order> CreateOrderAsync(CreateOrderDto dto)
    {
        var order = new Order
        {
            CustomerId = dto.CustomerId,
            Items = dto.Items,
            Total = dto.Items.Sum(i => i.Price * i.Quantity),
            Status = OrderStatus.Pending
        };

        var isPaid = await _paymentService.ChargeAsync(order.Total);
        if (!isPaid)
            throw new PaymentFailedException("Payment failed");

        order.Status = OrderStatus.Confirmed;
        await _repository.AddAsync(order);

        _logger.LogInformation($"Order {order.Id} created");
        return order;
    }
}
```

```csharp
// Test con xUnit + Moq
public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _mockRepository;
    private readonly Mock<IPaymentService> _mockPayment;
    private readonly Mock<ILogger<OrderService>> _mockLogger;
    private readonly OrderService _sut; // System Under Test

    public OrderServiceTests()
    {
        _mockRepository = new Mock<IOrderRepository>();
        _mockPayment = new Mock<IPaymentService>();
        _mockLogger = new Mock<ILogger<OrderService>>();

        _sut = new OrderService(
            _mockRepository.Object,
            _mockPayment.Object,
            _mockLogger.Object);
    }

    // Test: Happy path
    [Fact]
    public async Task CreateOrder_PaymentSucceeds_ReturnsConfirmedOrder()
    {
        // Arrange
        var dto = new CreateOrderDto
        {
            CustomerId = 1,
            Items = new List<OrderItemDto>
            {
                new("Product A", 29.99m, 2)
            }
        };
        _mockPayment.Setup(p => p.ChargeAsync(It.IsAny<decimal>()))
            .ReturnsAsync(true); // Payment succeeds

        // Act
        var order = await _sut.CreateOrderAsync(dto);

        // Assert
        Assert.Equal(OrderStatus.Confirmed, order.Status);
        Assert.Equal(59.98m, order.Total);
        _mockRepository.Verify(r => r.AddAsync(order), Times.Once());
    }

    // Test: Payment failure
    [Fact]
    public async Task CreateOrder_PaymentFails_ThrowsException()
    {
        // Arrange
        var dto = new CreateOrderDto { /* ... */ };
        _mockPayment.Setup(p => p.ChargeAsync(It.IsAny<decimal>()))
            .ReturnsAsync(false); // Payment fails

        // Act & Assert
        await Assert.ThrowsAsync<PaymentFailedException>(
            () => _sut.CreateOrderAsync(dto));

        // Verifica che order non sia stato salvato
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Never());
    }

    // Test: Parametrizzato
    [Theory]
    [InlineData(1, 10.0, 10.0)]
    [InlineData(2, 10.0, 20.0)]
    [InlineData(3, 5.5, 16.5)]
    public void CalculateTotal_ReturnsCorrectTotal(
        int quantity, decimal price, decimal expectedTotal)
    {
        // Arrange & Act
        var total = price * quantity;

        // Assert
        Assert.Equal(expectedTotal, total);
    }
}
```

### Integration Tests con TestServer

```csharp
// Test che testa controller + service + DB (in-memory)
public class OrderControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public OrderControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateOrder_ValidData_Returns201()
    {
        // Arrange
        var order = new CreateOrderDto
        {
            CustomerId = 1,
            Items = new[] { new OrderItemDto("Product", 29.99m, 1) }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/orders", order);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<OrderDto>();
        Assert.NotNull(result);
        Assert.Equal(29.99m, result.Total);
    }

    [Fact]
    public async Task CreateOrder_InvalidData_Returns400()
    {
        var order = new CreateOrderDto { CustomerId = 0 }; // Invalid
        var response = await _client.PostAsJsonAsync("/api/orders", order);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
```

### Best Practices Testing

**Naming Convention: `MethodName_Condition_ExpectedResult`**

```
CreateOrder_PaymentSucceeds_ReturnsConfirmedOrder
GetUser_UserNotFound_ReturnsNull  
CalculateDiscount_Premium_Returns20Percent
```

**AAA Pattern: Arrange, Act, Assert**

- **Arrange:** Prepara stato e input
- **Act:** Esegue il codice sotto test
- **Assert:** Verifica il risultato

---

## 2. Error Handling — Global Exception Handling {#error-handling}

### ProblemDetails — Standard RFC 7807

```csharp
// Risposta errore standardizzata
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    "title": "Bad Request",
    "status": 400,
    "detail": "Email format is invalid",
    "instance": "/api/users"
}
```

### Global Exception Handler Middleware

```csharp
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Resource not found");
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "Not Found",
                Detail = ex.Message,
                Status = 404,
                Type = "NotFound"
            });
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation error");
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "Validation Error",
                Detail = string.Join("; ", ex.Errors),
                Status = 400
            });
        }
        catch (UnauthorizedException)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "Unauthorized",
                Detail = "Authentication required",
                Status = 401
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "Internal Server Error",
                Detail = "Something went wrong", // Mai esponi dettagli in production
                Status = 500
            });
        }
    }
}

// Registrazione in Program.cs
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
```

### Exception Hierarchy — Custom Exceptions

```csharp
// Base exception
public abstract class AppException : Exception
{
    public int StatusCode { get; }

    protected AppException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}

public class NotFoundException : AppException
{
    public NotFoundException(string entity, object id)
        : base($"{entity} with id {id} not found", 404) { }
}

public class ValidationException : AppException
{
    public List<string> Errors { get; }

    public ValidationException(List<string> errors)
        : base("Validation failed", 400)
    {
        Errors = errors;
    }
}

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message = "Unauthorized")
        : base(message, 401) { }
}

// Uso nel codice
public async Task<User> GetUserAsync(int id)
{
    var user = await _repository.GetByIdAsync(id);
    if (user == null)
        throw new NotFoundException("User", id); // Catturato dal middleware
    return user;
}
```

---

# PARTE 5: TOP DOMANDE & RISPOSTE RAPIDE ⚠️ MUST MEMORIZE

> **ATTENZIONE**: Queste sono le domande più frequenti. Devi saper rispondere in 30-60 secondi, chiaramente e con sicurezza.
> 

---

## Quick Q&A - Interface vs Abstract Class

**Q**: Differenza tra Interface e Abstract Class?

**A (30 sec)**:

"Un'**Interface** definisce un contratto (solo firme di metodi) e una classe può implementarne multiple. Un'**Abstract Class** può avere implementazioni concrete e campi, ma si può ereditare da una sola. Uso Interface per definire capacità (ILoggable, ICacheable) e per Dependency Injection. Uso Abstract Class quando ho logica condivisa tra classi con forte relazione IS-A, come nel Template Method pattern. In generale preferisco Interface per accoppiamento debole."

---

## Quick Q&A - Async/Await vs Multithreading

**Q**: Differenza tra async/await e multithreading?

**A (30 sec)**:

"**Async/await** è per operazioni I/O-bound: libera il thread durante l'attesa senza bloccarlo. Non crea necessariamente nuovi thread. **Multithreading** è per operazioni CPU-bound: esegue calcoli pesanti in parallelo su thread diversi. Esempio: per chiamate HTTP uso async/await, per elaborazioni matematiche intensive uso [Task.Run](http://Task.Run) o Parallel. Async è per scalabilità (più request con meno thread), multithreading è per performance (calcoli paralleli)."

---

## Quick Q&A - Entity Framework: Include vs Explicit Loading

**Q**: Differenza tra Include e Explicit Loading?

**A (30 sec)**:

"**Include** (Eager Loading) carica le entità correlate con una JOIN nella stessa query. **Explicit Loading** carica le entità correlate con query separate su richiesta. Include è meglio quando sai che userai i dati (evita N+1). Explicit Loading quando non sempre servono i dati correlati. Esempio: `.Include(u => u.Orders)` carica subito gli ordini, mentre `context.Entry(user).Collection(u => u.Orders).Load()` li carica solo se necessario."

---

## Quick Q&A - IEnumerable vs IQueryable

**Q**: Differenza tra IEnumerable e IQueryable?

**A (30 sec)**:

"**IEnumerable** esegue query in memoria (LINQ to Objects), **IQueryable** traduce in SQL (LINQ to SQL/EF). IQueryable usa Expression Trees per costruire query remote. Se chiamo `.ToList()` troppo presto, trasformo IQueryable in IEnumerable e perdo ottimizzazioni SQL. Esempio: con IQueryable il filtro `.Where(u => u.Age > 18)` diventa `WHERE Age > 18` in SQL, con IEnumerable filtra in C# dopo aver caricato tutti i record."

---

## Quick Q&A - Scoped vs Singleton vs Transient

**Q**: Quando uso Scoped, Singleton, Transient?

**A (30 sec)**:

"**Transient**: nuova istanza ogni volta (es. lightweight services). **Scoped**: una istanza per HTTP request, usato per DbContext perché deve vivere per la request ma essere disposed dopo. **Singleton**: una istanza per l'applicazione, usato per configurazioni o cache, ma ATTENZIONE: deve essere thread-safe. Regola: DbContext sempre Scoped, configurazioni Singleton, servizi stateless Transient."

---

## Quick Q&A - PUT vs PATCH

**Q**: Differenza tra PUT e PATCH?

**A (30 sec)**:

"**PUT** sostituisce l'intera risorsa (tutti i campi), è idempotente. **PATCH** modifica parzialmente la risorsa (solo campi specificati), tecnicamente non è idempotente. Esempio: PUT richiede tutti i campi nel body anche se modifico solo email. PATCH invia solo `{"email": "[new@test.com](mailto:new@test.com)"}`. Uso PUT per update completi (form intero), PATCH per modifiche singole (toggle status, cambia email)."

---

## Quick Q&A - 401 vs 403

**Q**: Differenza tra 401 Unauthorized e 403 Forbidden?

**A (30 sec)**:

"**401** significa 'non autenticato' - l'utente deve fare login (token mancante/invalido). **403** significa 'autenticato ma non autorizzato' - l'utente è loggato ma non ha i permessi per quell'azione. Esempio: se chiamo un endpoint protetto senza token → 401. Se sono loggato come User e cerco di accedere a endpoint riservato agli Admin → 403."

---

## Quick Q&A - FirstOrDefault vs SingleOrDefault

**Q**: Quando uso FirstOrDefault vs SingleOrDefault?

**A (30 sec)**:

"**FirstOrDefault** ritorna il primo elemento o null, va bene se ci possono essere più match. **SingleOrDefault** verifica che ci sia ESATTAMENTE un elemento, lancia exception se ce ne sono più di uno. Uso FirstOrDefault per ordinamenti o liste (`.OrderBy(x => [x.Date](http://x.Date)).FirstOrDefault()`). Uso SingleOrDefault per unique keys (`.SingleOrDefault(u => [u.Email](http://u.Email) == email)`) perché se ci sono duplicati, voglio saperlo."

---

## Quick Q&A - Problema N+1

**Q**: Cos'è il problema N+1 e come lo risolvi?

**A (30 sec)**:

"Il problema N+1 è quando carichi N entità e per ognuna fai una query separata per caricare dati correlati, risultando in 1+N query invece di 1. Esempio: carico 100 users, poi per ogni user carico i suoi orders → 101 query. Risolvo con **Eager Loading** usando `.Include(u => u.Orders)` che fa un JOIN e carica tutto in 1 query. Oppure uso proiezioni con `.Select()` per caricare solo i campi che servono."

---

## Quick Q&A - Middleware Pipeline

**Q**: Cos'è la Request Pipeline in [ASP.NET](http://ASP.NET) Core?

**A (30 sec)**:

"È una sequenza di middleware che processano ogni HTTP request. Ogni middleware può modificare request/response e decidere se passare al successivo o fermarsi (short-circuit). L'ordine è CRITICO: Exception handling deve essere primo per catturare errori, Authentication prima di Authorization, Routing prima degli endpoint. Esempio tipico: ExceptionHandler → HTTPS Redirect → Static Files → Routing → Authentication → Authorization → Endpoints."

---

## Quick Q&A - JWT

**Q**: Come funziona JWT authentication?

**A (30 sec)**:

"JWT è un token firmato digitalmente con 3 parti: Header (algoritmo), Payload (claims come user ID, role), Signature (firma con chiave segreta). Al login, server genera JWT e lo ritorna. Client lo include negli headers `Authorization: Bearer <token>`. Server valida firma e expiration, estrae claims. Vantaggi: stateless (no session server-side), scalabile, cross-domain. Svantaggio: non può essere revocato facilmente, devi aspettare expiration."

---

## Quick Q&A - SOLID

**Q**: Spiega SOLID brevemente.

**A (60 sec)**:

"**S**ingle Responsibility: una classe, una ragione per cambiare. **O**pen/Closed: aperto a estensione, chiuso a modifica (usa interface per estendere). **L**iskov Substitution: sottoclassi sostituibili alle classi base senza rompere funzionalità. **I**nterface Segregation: interface piccole e specifiche, non 'grasse'. **D**ependency Inversion: dipendi da astrazioni (interface), non da implementazioni concrete. In pratica: uso DI per Dependency Inversion, Interface Segregation per API chiare, Open/Closed con strategy pattern, Single Responsibility separando concerns."

---

---

## Quick Q&A — Repository Pattern

**Q**: Cos'è il Repository Pattern e perché lo usi?

**A (30 sec)**:

"Il Repository Pattern separa la logica di accesso ai dati dalla business logic. Definisco un'interface (IOrderRepository) nel domain/application layer con i metodi che mi servono. L'implementazione concreta usa EF Core nel infrastructure layer. Vantaggi: testabilità (inietto mock nel test), loose coupling (posso cambiare ORM), codice più leggibile. In Clean Architecture, il Repository è il punto di contatto tra Application e Infrastructure layer."

---

## Quick Q&A — SQL JOINs

**Q**: Differenza tra INNER JOIN e LEFT JOIN?

**A (30 sec)**:

"INNER JOIN ritorna solo righe dove c'è match in ENTRAMBE le tabelle. Se un cliente non ha ordini, non compare. LEFT JOIN ritorna TUTTE le righe dalla tabella sinistra, anche se non hanno match nella destra (in quel caso NULL). Uso INNER JOIN quando devo avere il match garantito, LEFT JOIN quando la relazione è opzionale. Esempio: per trovare clienti con ordini uso INNER, per trovare tutti i clienti inclusi quelli senza ordini uso LEFT."

---

## Quick Q&A — Indexes SQL Server

**Q**: Perché creare un index? Quando NON creare?

**A (30 sec)**:

"Un index velocizza le ricerca usando una struttura B-Tree, da O(n) a O(log n). Creo index sulle colonne usate frequentemente in WHERE, JOIN, ORDER BY. Non creo index quando: la colonna ha pochi valori distinti (IsActive true/false), la tabella è piccola, o ho troppi INSERT/UPDATE perché ogni DML deve aggiornare gli index. Il Clustered Index organizza fisicamente i dati, uno solo per tabella. Non-Clustered sono strutture separate, posso averne molti."

---

## Quick Q&A — Caching Strategy

**Q**: Cos'è Cache-Aside e come funziona?

**A (30 sec)**:

"Cache-Aside (o Lazy Loading) è il pattern più comune per caching. Prima controlla la cache: se hit, ritorno il dato. Se miss, carico dal database, salvo in cache con un TTL, e ritorno. Il vantaggio è che la cache è populata solo per i dati effettivamente richiesti. Svantaggio: primo accesso è sempre un cache miss. Quando aggiorno un dato nel DB, devo invalidare anche la cache per evitare dati stale."

---

## Quick Q&A — RabbitMQ vs Kafka

**Q**: Differenza tra RabbitMQ e Kafka?

**A (30 sec)**:

"RabbitMQ è un traditional message broker: i messaggi vengono rimossi dopo il consumo, focalizzato su routing e workflow tra servizi. Kafka è un event streaming platform: i messaggi vengono ritenuti per un periodo configurato e possono essere riprocessati (replay). Uso RabbitMQ per comunicazione tra microservizi e task distribution. Uso Kafka per alto throughput, event sourcing, audit trail, e quando devo reprocessare eventi storici."

---

## Quick Q&A — Microservizi vs Monolito

**Q**: Quando passare a microservizi?

**A (30 sec)**:

"Microservizi non è sempre la risposta migliore. Li uso quando: l'applicazione è grande e il monolito è difficile da gestire, ho equipaggi diversi che lavorano in parallelo, devo scalare parti del sistema indipendentemente. I rischi sono: latenza di rete, consistenza dati distribuita, complessità operativa. La raccomandazione comune è iniziare con un monolito ben strutturato e passare a microservizi solo quando hai un problema concreto di scalabilità o del team."

---

## Quick Q&A — Transactions e Isolation

**Q**: Cosa significa isolation level in SQL Server?

**A (30 sec)**:

"L'isolation level controlla come una transazione vede i dati delle altre transazioni in corso. Read Committed (default) previene dirty read ma permette non-repeatable reads. Repeatable Read garantisce che lo stesso dato letto 2 volte dia stesso risultato. Serializable è il più isolato ma il più lento. Snapshot usa versioning dei dati (MVCC) per isolamento senza blocking in lettura, eccellente per workload read-heavy."

---

## Quick Q&A — Design Patterns

**Q**: Qualé la differenza tra Strategy e Factory?

**A (30 sec)**:

"Strategy definisce una famiglia di algoritmi intercambiabili per fare la STESSA cosa in modi diversi – es. diversi metodi di pagamento. Il comportamento cambia a runtime. Factory è per la CREAZIONE di oggetti: quando il client deve creare un oggetto ma non deve conoscere la classe concreta. Es. NotificationFactory crea EmailNotification o SmsNotification basandosi su un parametro. Strategy = comportamento intercambiabile, Factory = creazione astratta."

---

## Quick Q&A — Unit vs Integration Tests

**Q**: Differenza tra Unit e Integration test?

**A (30 sec)**:

"Unit test testa un componente in ISOLAMENTO: mocks tutte le dipendenze, focalizzato su una singola funzione. Veloce e deterministic. Integration test testa l'interazione tra componenti con dipendenze reali (o vicine a reali, come DB in-memory). Più lento ma verifica che le parti funzionino insieme. Uso Unit test per business logic, Integration test per verificare che controller + service + DB funzionino correttamente."

---

## Quick Q&A — Decorator vs Inheritance

**Q**: Perché usare Decorator invece di ereditarietà?

**A (30 sec)**:

"Il Decorator aggiunge comportamento senza modificare la classe originale, usando composizione. Se uso ereditarietà per aggiungere logging + caching, creo una gerarchia profonda e inflessibile. Con Decorator, compongo i comportamenti dinamicamente: LoggingService wraps CachingService wraps DataService. Ogni decorator é indipendente e riutilizzabile. È l'applicazione del principio 'composition over inheritance'."

---

## Quick Q&A — AsNoTracking

**Q**: Quando uso AsNoTracking in EF Core?

**A (30 sec)**:

"AsNoTracking disabilita il change tracking di EF Core per quell'query. Quando carico dati solo per leggere (es. API GET, dashboard), non devo tracciare le modifiche. AsNoTracking è più veloce e usa meno memoria perché EF non crea una copia originale per il confronto. Uso sempre AsNoTracking per query read-only, mai quando devo poi modificare l'entità tramite DbContext."

---

## Quick Q&A — Circuit Breaker

**Q**: Cos'è il Circuit Breaker pattern?

**A (30 sec)**:

"Il Circuit Breaker è come un interruttore elettrico per chiamate esterne. Quando un servizio inizia a fallire, dopo N falliti consecutivi il circuito si 'apre' e le chiamate falliscono immediatamente senza aspettare timeout. Dopo un periodo, il circuito passa in stato 'half-open' e prova una chiamata: se ha successo si chiude, altrimenti resta aperto. Previene cascading failures tra microservizi."

---

## Quick Q&A — Window Functions SQL

**Q**: Differenza tra GROUP BY e Window Functions?

**A (30 sec)**:

"GROUP BY aggrega le righe e collassa il risultato: un row per gruppo. Window Functions aggregano MA mantengono tutte le righe individuali. Con GROUP BY perdo i dettagli singole righe. Con Window Functions ho sia il dettaglio sia l'aggregazione nella stessa riga. Esempio: GROUP BY per totale vendite per cliente, Window Function per ogni vendita + running total del cliente accanto."

---

## FINE AGGIORNAMENTO CRITICO

**✅ Cosa hai ora — Copertura Completa:**

| Area | Status | Livello |
| --- | --- | --- |
| OOP (4 Pillars) | ✅ Completo | Profondo |
| C# Type System | ✅ Completo | Profondo |
| Memory Management | ✅ Completo | Profondo |
| LINQ + N+1 | ✅ Completo | Profondo |
| Async/Await | ✅ Completo | Profondo |
| Multithreading | ✅ Completo | Medio-Alto |
| [ASP.NET](http://ASP.NET) Core Pipeline | ✅ Completo | Profondo |
| DI + Lifetimes | ✅ Completo | Profondo |
| REST API + HTTP Verbs | ✅ Completo | Profondo |
| JWT Auth | ✅ Completo | Profondo |
| SQL Server + JOINs + Indexes | ✅ Completo | Profondo |
| Transactions + Isolation | ✅ Completo | Medio |
| Window Functions + CTE | ✅ Completo | Medio |
| Entity Framework Core | ✅ Completo | Medio-Alto |
| SOLID Principles | ✅ Completo | Profondo |
| Clean Architecture | ✅ Completo | Medio |
| Design Patterns (7) | ✅ Completo | Profondo |
| Redis Caching | ✅ Completo | Medio |
| RabbitMQ / Kafka | ✅ Completo | Medio |
| Microservices | ✅ Completo | Medio |
| Resilience (Circuit Breaker) | ✅ Completo | Medio |
| Testing (Unit + Integration) | ✅ Completo | Medio |
| Error Handling | ✅ Completo | Medio |
| Quick Q&A | ✅ 22 risposte | Ready |

**🎟 Strategia per il Colloquio:**

---

# PARTE 8: TECNOLOGIE AGGIUNTIVE PER INSURANCE DOMAIN

## 1. [Quartz.NET](http://Quartz.NET) - Job Scheduling {#quartz-scheduling}

### Cos'è [Quartz.NET](http://Quartz.NET)?

[**Quartz.NET**](http://Quartz.NET) è un job scheduling framework per .NET. Permette di eseguire task automaticamente a orari specifici o intervalli regolari.

**Casi d'uso tipici:**

- Report generati ogni notte
- Pulizia dati obsoleti
- Sincronizzazione con sistemi esterni
- Invio email batch
- Calcolo premi assicurativi mensili

---

### I Tre Concetti Fondamentali

```jsx
┌──────────┐      schedula      ┌──────────┐
│ Trigger  │ ─────────────────→ │   Job    │
│ (QUANDO) │                     │  (COSA)  │
└──────────┘                     └──────────┘
           ↓                           ↓
        ┌────────────────────────────────┐
        │         Scheduler              │
        │  (orchestrator - coordina)     │
        └────────────────────────────────┘
```

#### **1. Job - COSA eseguire**

```csharp
public class GenerateMonthlyReportJob : IJob
{
    private readonly IReportService _reportService;
    private readonly ILogger<GenerateMonthlyReportJob> _logger;

    public GenerateMonthlyReportJob(
        IReportService reportService,
        ILogger<GenerateMonthlyReportJob> logger)
    {
        _reportService = reportService;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Starting monthly report generation");

        try
        {
            // Il tuo codice business
            await _reportService.GenerateMonthlyReportAsync();
            
            _logger.LogInformation("Monthly report completed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate monthly report");
            
            // JobExecutionException indica al scheduler che c'è stato un errore
            throw new JobExecutionException(ex);
        }
    }
}
```

#### **2. Trigger - QUANDO eseguire**

Ci sono due tipi principali:

**Simple Trigger** - Intervalli fissi:

```csharp
// Esegui ogni 30 minuti
var trigger = TriggerBuilder.Create()
    .WithIdentity("everyHalfHour", "reports")
    .StartNow()
    .WithSimpleSchedule(x => x
        .WithIntervalInMinutes(30)
        .RepeatForever())
    .Build();

// Esegui una volta dopo 5 minuti
var trigger = TriggerBuilder.Create()
    .WithIdentity("oneTime", "cleanup")
    .StartAt(DateTimeOffset.Now.AddMinutes(5))
    .Build();
```

**Cron Trigger** - Scheduling complesso:

```csharp
// Ogni giorno alle 3:00 AM
var trigger = TriggerBuilder.Create()
    .WithIdentity("nightlyReport", "reports")
    .WithCronSchedule("0 0 3 * * ?")
    .Build();

// Ogni lunedì alle 9:00 AM
var trigger = TriggerBuilder.Create()
    .WithIdentity("mondayMorning", "emails")
    .WithCronSchedule("0 0 9 ? * MON")
    .Build();
```

#### **3. Scheduler - Orchestrator**

```csharp
// Configurazione in ASP.NET Core
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Aggiungi Quartz
        builder.Services.AddQuartz(q =>
        {
            // Configurazione base
            q.UseMicrosoftDependencyInjectionJobFactory();

            // Job 1: Report mensile
            var monthlyReportKey = new JobKey("MonthlyReport");
            q.AddJob<GenerateMonthlyReportJob>(opts => opts.WithIdentity(monthlyReportKey));
            q.AddTrigger(opts => opts
                .ForJob(monthlyReportKey)
                .WithIdentity("MonthlyReportTrigger")
                .WithCronSchedule("0 0 2 1 * ?")); // Primo giorno del mese alle 2 AM

            // Job 2: Cleanup giornaliero
            var cleanupKey = new JobKey("DailyCleanup");
            q.AddJob<CleanupExpiredDataJob>(opts => opts.WithIdentity(cleanupKey));
            q.AddTrigger(opts => opts
                .ForJob(cleanupKey)
                .WithIdentity("DailyCleanupTrigger")
                .WithCronSchedule("0 0 3 * * ?")); // Ogni giorno alle 3 AM
        });

        // Hosted service per eseguire Quartz
        builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        var app = builder.Build();
        app.Run();
    }
}
```

---

### Cron Expressions - Sintassi Base

**Formato**: `sec min hour day month weekday [year]`

```jsx
┌───── secondi (0-59)
│ ┌───── minuti (0-59)
│ │ ┌───── ore (0-23)
│ │ │ ┌───── giorno del mese (1-31)
│ │ │ │ ┌───── mese (1-12 o JAN-DEC)
│ │ │ │ │ ┌───── giorno della settimana (0-7 o SUN-SAT, 0 e 7 = domenica)
│ │ │ │ │ │
* * * * * *
```

**Caratteri speciali:**

- `*` = ogni valore ("ogni ora", "ogni giorno")
- `?` = qualsiasi (usato per day o weekday quando l'altro è specifico)
- `-` = range (`1-5` = "da 1 a 5")
- `,` = lista (`MON,WED,FRI` = "lunedì, mercoledì, venerdì")
- `/` = incremento (`*/15` = "ogni 15")
- `#` = N-esimo giorno (`6#3` = "terzo venerdì")

**Esempi comuni:**

```csharp
// Ogni 15 minuti
"0 */15 * * * ?"

// Ogni giorno alle 14:30
"0 30 14 * * ?"

// Ogni lunedì alle 9:00
"0 0 9 ? * MON"

// Primo giorno di ogni mese alle 00:00
"0 0 0 1 * ?"

// Ogni giorno lavorativo (lun-ven) alle 8:00
"0 0 8 ? * MON-FRI"

// Ogni ultimo giorno del mese alle 23:59
"0 59 23 L * ?"

// Ogni 10 secondi
"*/10 * * * * ?"
```

---

### Esempio Completo - Insurance Domain

```csharp
// Job: Calcola premi mensili per tutte le polizze attive
public class CalculateMonthlyPremiumsJob : IJob
{
    private readonly IPolicyService _policyService;
    private readonly IEmailService _emailService;
    private readonly ILogger<CalculateMonthlyPremiumsJob> _logger;

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Starting monthly premium calculation");

        // Carica tutte le polizze attive
        var activePolicies = await _policyService.GetActivePoliciesAsync();

        int successCount = 0;
        int failCount = 0;

        foreach (var policy in activePolicies)
        {
            try
            {
                // Calcola premio
                var premium = await _policyService.CalculatePremiumAsync(policy.Id);
                
                // Invia notifica cliente
                await _emailService.SendPremiumNotificationAsync(
                    policy.CustomerEmail, 
                    policy.PolicyNumber, 
                    premium);
                
                successCount++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    $"Failed to calculate premium for policy {policy.PolicyNumber}");
                failCount++;
            }
        }

        _logger.LogInformation(
            $"Monthly premium calculation completed. Success: {successCount}, Failed: {failCount}");

        // Salva statistiche nel JobDataMap per monitoraggio
        context.Result = new { successCount, failCount };
    }
}

// Schedulazione
builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("CalculateMonthlyPremiums");
    
    q.AddJob<CalculateMonthlyPremiumsJob>(opts => opts
        .WithIdentity(jobKey)
        .StoreDurably()); // Job persiste anche senza trigger attivi

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("MonthlyPremiumTrigger")
        .WithCronSchedule("0 0 1 1 * ?") // Primo giorno di ogni mese all'1:00 AM
        .WithDescription("Calculate monthly insurance premiums"));
});
```

---

### Persistence - Salvare Job nel Database

```csharp
// Usa ADO.NET Job Store per persistere job nel database
builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    // Configurazione per SQL Server
    q.UsePersistentStore(store =>
    {
        store.UseSqlServer(connectionString);
        store.UseJsonSerializer();
    });
});
```

**Vantaggi persistence:**

- Job sopravvivono a restart dell'applicazione
- Clustering (multiple istanze della app condividono job)
- Storia esecuzioni

---

### Quick Q&A - [Quartz.NET](http://Quartz.NET)

**Q**: Differenza tra Job e Trigger?

**A (30 sec)**:

"Un **Job** definisce COSA fare (la logica business), implementando IJob con un metodo Execute. Un **Trigger** definisce QUANDO eseguire quel Job (ogni ora, ogni lunedì, cron expression). Un Job può avere multiple Trigger (es. esegui sia ogni notte CHE manualmente). Il **Scheduler** coordina tutto e gestisce l'esecuzione."

---

## 2. WCF (Windows Communication Foundation) {#wcf-basics}

### Cos'è WCF?

**WCF** è un framework Microsoft legacy (pre-2010) per costruire **servizi SOAP** (web services basati su XML).

**Perché lo vedi ancora nel 2025:**

- Sistemi legacy enterprise (banche, assicurazioni, pubblica amministrazione)
- Integrazioni con mainframe
- Contratti rigidi con XML Schema

**Perché è obsoleto:**

- Sostituito da REST API (più semplici, JSON, stateless)
- gRPC per performance (Protocol Buffers, HTTP/2)
- Complessità eccessiva per la maggior parte dei casi

---

### I Concetti Base WCF

**ABC di WCF:**

- **A**ddress: Dove trovare il servizio ([`http://server:8080/PolicyService`](http://server:8080/PolicyService))
- **B**inding: Come comunicare (HTTP, TCP, Named Pipes)
- **C**ontract: Cosa può fare il servizio (interfaccia)

```csharp
// Service Contract (interfaccia)
[ServiceContract]
public interface IPolicyService
{
    [OperationContract]
    Policy GetPolicy(string policyNumber);

    [OperationContract]
    void UpdatePolicy(Policy policy);
}

// Implementazione
public class PolicyService : IPolicyService
{
    public Policy GetPolicy(string policyNumber)
    {
        // Logica per recuperare polizza
        return new Policy { Number = policyNumber };
    }

    public void UpdatePolicy(Policy policy)
    {
        // Logica per aggiornare
    }
}

// Data Contract (oggetti scambiati)
[DataContract]
public class Policy
{
    [DataMember]
    public string Number { get; set; }

    [DataMember]
    public decimal Premium { get; set; }

    [DataMember]
    public DateTime ExpirationDate { get; set; }
}
```

---

### SOAP Message (cosa WCF invia)

**Request**:

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
  <soap:Body>
    <GetPolicy xmlns="http://tempuri.org/">
      <policyNumber>POL-12345</policyNumber>
    </GetPolicy>
  </soap:Body>
</soap:Envelope>
```

**Response**:

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
  <soap:Body>
    <GetPolicyResponse xmlns="http://tempuri.org/">
      <Policy>
        <Number>POL-12345</Number>
        <Premium>299.99</Premium>
        <ExpirationDate>2026-12-31</ExpirationDate>
      </Policy>
    </GetPolicyResponse>
  </soap:Body>
</soap:Envelope>
```

**Confronto con REST:**

```jsx
// REST API (JSON) - Molto più semplice!
GET /api/policies/POL-12345

Response:
{
  "number": "POL-12345",
  "premium": 299.99,
  "expirationDate": "2026-12-31"
}
```

---

### Perché WCF è Obsoleto

| Aspetto | WCF (SOAP) | REST API | gRPC |
| --- | --- | --- | --- |
| **Formato** | XML (verbose) | JSON (compatto) | Protobuf (binario) |
| **Overhead** | Alto | Basso | Molto basso |
| **Complessità** | Alta (WSDL, binding) | Bassa | Media |
| **Performance** | Lento | Veloce | Molto veloce |
| **Tooling** | Legacy | Moderno | Moderno |
| **Cross-platform** | No (principalmente Windows) | Sì | Sì |

---

### Migration Path (WCF → Modern)

**Strategia tipica in Insurance:**

1. **Fase 1**: Mantenere WCF per client legacy
2. **Fase 2**: Esporre REST API in parallelo (dual hosting)
3. **Fase 3**: Migrare client gradualmente a REST
4. **Fase 4**: Deprecare WCF

```csharp
// Dual hosting - WCF + REST API condividono business logic
public class PolicyBusinessLogic
{
    public Policy GetPolicy(string policyNumber)
    {
        // Shared logic
    }
}

// WCF Service
public class PolicyWcfService : IPolicyService
{
    private readonly PolicyBusinessLogic _logic;
    
    public Policy GetPolicy(string policyNumber)
    {
        return _logic.GetPolicy(policyNumber);
    }
}

// REST API Controller
[ApiController]
[Route("api/policies")]
public class PolicyController : ControllerBase
{
    private readonly PolicyBusinessLogic _logic;

    [HttpGet("{policyNumber}")]
    public ActionResult<Policy> GetPolicy(string policyNumber)
    {
        return Ok(_logic.GetPolicy(policyNumber));
    }
}
```

---

### Quick Q&A - WCF

**Q**: Cos'è WCF e perché è obsoleto?

**A (30 sec)**:

"WCF è un framework Microsoft per servizi SOAP basati su XML. Era lo standard negli anni 2000-2010 ma ora è legacy. Problemi: XML è verbose, configurazione complessa, performance inferiori a REST/JSON o gRPC. Lo trovi ancora in insurance e banking per sistemi legacy. La migration moderna è verso REST API per semplicità o gRPC per performance. Conoscenza base WCF è utile per integrazioni con sistemi vecchi, ma per nuovo codice si usa [ASP.NET](http://ASP.NET) Core Web API."

---

## 3. gRPC vs REST - Comunicazione tra Servizi {#grpc-rest}

### Cos'è gRPC?

**gRPC** = **g**oogle **R**emote **P**rocedure **C**all

Un framework moderno per comunicazione client-server:

- Usa **Protocol Buffers** (Protobuf) invece di JSON
- HTTP/2 invece di HTTP/1.1
- Tipizzato e generato dal codice
- Streaming bidirezionale

---

### REST vs gRPC - Confronto Dettagliato

| Aspetto | REST API | gRPC |
| --- | --- | --- |
| **Formato dati** | JSON (testo) | Protobuf (binario) |
| **Protocollo** | HTTP/1.1 | HTTP/2 |
| **Performance** | Buona | Eccellente (7-10x più veloce) |
| **Payload size** | Grande (JSON verbose) | Piccolo (binario compresso) |
| **Tipizzazione** | Debole (validazione runtime) | Forte (compile-time) |
| **Streaming** | No (solo request/response) | Sì (bidirezionale) |
| **Browser support** | Nativo | Limitato (serve grpc-web) |
| **Debugging** | Facile (leggibile) | Difficile (binario) |
| **Learning curve** | Bassa | Media |
| **Caso d'uso** | API pubbliche, web, mobile | Microservizi, server-to-server |

---

### Esempio Pratico - Stessa Funzionalità

#### **REST API**

```csharp
// Controller
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    [HttpGet("{id}")]
    public ActionResult<UserDto> GetUser(int id)
    {
        var user = _service.GetUser(id);
        return Ok(user);
    }
}

// Request/Response (HTTP)
GET /api/users/123 HTTP/1.1
Host: api.example.com

HTTP/1.1 200 OK
Content-Type: application/json
{
  "id": 123,
  "name": "John Doe",
  "email": "john@example.com"
}
```

#### **gRPC**

```protobuf
// users.proto - Definizione contratto
syntax = "proto3";

service UserService {
  rpc GetUser (UserRequest) returns (UserResponse);
}

message UserRequest {
  int32 id = 1;
}

message UserResponse {
  int32 id = 1;
  string name = 2;
  string email = 3;
}
```

```csharp
// Server - Implementazione
public class UserGrpcService : UserService.UserServiceBase
{
    public override Task<UserResponse> GetUser(
        UserRequest request, 
        ServerCallContext context)
    {
        var user = _service.GetUser(request.Id);
        
        return Task.FromResult(new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        });
    }
}

// Client
var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = new UserService.UserServiceClient(channel);

var response = await client.GetUserAsync(new UserRequest { Id = 123 });
Console.WriteLine($"{response.Name}: {response.Email}");
```

---

### Quando Usare Cosa?

#### **Usa REST quando:**

- ✅ Client è browser o mobile app
- ✅ API pubblica per third-party
- ✅ Debugging e ispezione manuale importanti
- ✅ Team non ha esperienza con gRPC
- ✅ Semplicità > Performance

#### **Usa gRPC quando:**

- ✅ Microservizi server-to-server
- ✅ Performance critica (real-time, alto throughput)
- ✅ Streaming necessario (video, log, chat)
- ✅ Contratti tipizzati importanti
- ✅ Polyglot environment (Java ↔ C# ↔ Go)

---

### gRPC Streaming - Superpotere

```csharp
// Server Streaming - Server invia stream di dati
service OrderService {
  rpc GetOrderUpdates (OrderRequest) returns (stream OrderUpdate);
}

public override async Task GetOrderUpdates(
    OrderRequest request,
    IServerStreamWriter<OrderUpdate> responseStream,
    ServerCallContext context)
{
    while (!context.CancellationToken.IsCancellationRequested)
    {
        var update = await _service.GetLatestUpdateAsync(request.OrderId);
        await responseStream.WriteAsync(update);
        await Task.Delay(1000); // Ogni secondo
    }
}

// Client riceve stream
var call = client.GetOrderUpdates(new OrderRequest { OrderId = 123 });

await foreach (var update in call.ResponseStream.ReadAllAsync())
{
    Console.WriteLine($"Order status: {update.Status}");
}
```

**Con REST** questo richiederebbe polling continuo o WebSocket.

---

### Quick Q&A - gRPC vs REST

**Q**: Quando useresti gRPC invece di REST?

**A (30 sec)**:

"gRPC è migliore per comunicazione **server-to-server** tra microservizi dove performance è critica. Usa HTTP/2 e Protocol Buffers binari che sono 7-10x più veloci di JSON. Supporta streaming bidirezionale nativamente. REST è migliore per **API pubbliche** e **client browser** perché JSON è leggibile e debugging è facile. In generale: gRPC per backend microservizi, REST per frontend e third-party API."

---

## 4. CSV - Lavorare con File CSV {#csv-processing}

### Cos'è un CSV?

**CSV** = **C**omma-**S**eparated **V**alues

Formato file testuale semplice per dati tabulari:

```
Name,Email,Age,City
John Doe,john@example.com,30,Milan
Jane Smith,jane@example.com,25,Rome
Mario Rossi,mario@example.com,35,Naples
```

**Caratteristiche:**

- Prima riga = header (nomi colonne)
- Ogni riga = un record
- Colonne separate da virgola (o `;` o `\t`)
- Usato per import/export dati tra sistemi

---

### Leggere CSV con CsvHelper (Library Migliore)

```csharp
// NuGet: CsvHelper
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

// Modello
public class Customer
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public string City { get; set; }
}

// Lettura
public async Task<List<Customer>> ReadCsvAsync(string filePath)
{
    using var reader = new StreamReader(filePath);
    using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
    
    // Mapping automatico dalle colonne alle proprietà
    var records = csv.GetRecords<Customer>();
    return records.ToList();
}

// Uso
var customers = await ReadCsvAsync("customers.csv");
foreach (var customer in customers)
{
    Console.WriteLine($"{customer.Name} - {customer.Email}");
}
```

---

### Scrivere CSV

```csharp
public async Task WriteCsvAsync(List<Customer> customers, string filePath)
{
    using var writer = new StreamWriter(filePath);
    using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
    
    // Scrive header + records
    await csv.WriteRecordsAsync(customers);
}

// Uso
var customers = new List<Customer>
{
    new Customer { Name = "John", Email = "john@test.com", Age = 30, City = "Milan" },
    new Customer { Name = "Jane", Email = "jane@test.com", Age = 25, City = "Rome" }
};

await WriteCsvAsync(customers, "output.csv");
```

---

### Mapping Personalizzato

```csharp
// CSV con colonne diverse dai nomi delle proprietà
public class CustomerMap : ClassMap<Customer>
{
    public CustomerMap()
    {
        Map(m => m.Name).Name("Full Name");     // Colonna "Full Name" → proprietà Name
        Map(m => m.Email).Name("E-mail");       // Colonna "E-mail" → proprietà Email
        Map(m => m.Age).Name("Years");          // Colonna "Years" → proprietà Age
        Map(m => m.City).Index(3);              // Colonna indice 3 → proprietà City
    }
}

// Uso mapping
var config = new CsvConfiguration(CultureInfo.InvariantCulture)
{
    HasHeaderRecord = true,
};

using var csv = new CsvReader(reader, config);
csv.Context.RegisterClassMap<CustomerMap>();
var records = csv.GetRecords<Customer>().ToList();
```

---

### Gestione Errori e Validazione

```csharp
public async Task<List<Customer>> ReadCsvWithValidationAsync(string filePath)
{
    var validRecords = new List<Customer>();
    var errors = new List<string>();

    using var reader = new StreamReader(filePath);
    using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

    // Configurazione: ignora header case-insensitive
    csv.Context.Configuration.PrepareHeaderForMatch = args => args.Header.ToLower();

    await foreach (var record in csv.GetRecordsAsync<Customer>())
    {
        try
        {
            // Validazione
            if (string.IsNullOrEmpty(record.Email) || !record.Email.Contains("@"))
            {
                errors.Add($"Row {csv.Context.Parser.Row}: Invalid email '{record.Email}'");
                continue;
            }

            if (record.Age < 0 || record.Age > 120)
            {
                errors.Add($"Row {csv.Context.Parser.Row}: Invalid age {record.Age}");
                continue;
            }

            validRecords.Add(record);
        }
        catch (Exception ex)
        {
            errors.Add($"Row {csv.Context.Parser.Row}: {ex.Message}");
        }
    }

    if (errors.Any())
    {
        _logger.LogWarning($"CSV import completed with {errors.Count} errors:\n" + 
            string.Join("\n", errors));
    }

    return validRecords;
}
```

---

### CSV per Insurance - Caso Reale

```csharp
// Export polizze per report mensile
public class PolicyExportDto
{
    public string PolicyNumber { get; set; }
    public string CustomerName { get; set; }
    public decimal Premium { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string Status { get; set; }
}

public async Task<byte[]> ExportPoliciesToCsvAsync(DateTime month)
{
    // Carica polizze del mese
    var policies = await _context.Policies
        .Where(p => p.StartDate.Month == month.Month && p.StartDate.Year == month.Year)
        .Select(p => new PolicyExportDto
        {
            PolicyNumber = p.Number,
            CustomerName = p.Customer.Name,
            Premium = p.Premium,
            StartDate = p.StartDate,
            ExpirationDate = p.ExpirationDate,
            Status = p.Status.ToString()
        })
        .ToListAsync();

    // Genera CSV in memoria
    using var memoryStream = new MemoryStream();
    using var writer = new StreamWriter(memoryStream);
    using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
    
    await csv.WriteRecordsAsync(policies);
    await writer.FlushAsync();
    
    return memoryStream.ToArray();
}

// Controller per download
[HttpGet("export/policies")]
public async Task<IActionResult> ExportPolicies([FromQuery] DateTime month)
{
    var csvBytes = await _service.ExportPoliciesToCsvAsync(month);
    
    return File(
        csvBytes, 
        "text/csv", 
        $"policies_{month:yyyy-MM}.csv");
}
```

---

### Import Bulk da CSV

```csharp
// Import massivo di polizze da CSV
public async Task<ImportResult> ImportPoliciesFromCsvAsync(Stream fileStream)
{
    var result = new ImportResult();

    using var reader = new StreamReader(fileStream);
    using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

    var records = csv.GetRecords<PolicyImportDto>();

    foreach (var record in records)
    {
        try
        {
            // Validazione
            if (await _context.Policies.AnyAsync(p => p.Number == record.PolicyNumber))
            {
                result.Skipped++;
                result.Errors.Add($"{record.PolicyNumber}: già esiste");
                continue;
            }

            // Crea entità
            var policy = new Policy
            {
                Number = record.PolicyNumber,
                CustomerId = await GetOrCreateCustomerAsync(record.CustomerEmail),
                Premium = record.Premium,
                StartDate = record.StartDate,
                ExpirationDate = record.ExpirationDate,
                Status = Enum.Parse<PolicyStatus>(record.Status)
            };

            _context.Policies.Add(policy);
            result.Imported++;

            // Batch save ogni 100 per performance
            if (result.Imported % 100 == 0)
            {
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            result.Failed++;
            result.Errors.Add($"{record.PolicyNumber}: {ex.Message}");
        }
    }

    // Save finale
    await _context.SaveChangesAsync();

    return result;
}

public class ImportResult
{
    public int Imported { get; set; }
    public int Failed { get; set; }
    public int Skipped { get; set; }
    public List<string> Errors { get; set; } = new();
}
```

---

### Quick Q&A - CSV Processing

**Q**: Come gestisci CSV in C#?

**A (30 sec)**:

"Uso la libreria **CsvHelper** perché gestisce automaticamente parsing, mapping, e edge cases. Per leggere: creo un modello C#, uso CsvReader con GetRecords<T>() e mappa automaticamente le colonne. Per scrivere: uso CsvWriter con WriteRecordsAsync(). Per import massivi faccio batch save ogni 100 record per performance. Gestisco sempre validazione ed errori con try-catch, loggando quali righe falliscono senza bloccare l'intero import."

---

## 5. Insurance Domain - Terminologia Base {#insurance-basics}

### Concetti Fondamentali

#### **Policy (Polizza)**

**Definizione**: Contratto tra assicurato e compagnia assicurativa.

**Componenti chiave:**

- **Policy Number**: Identificativo unico (es. POL-2024-001234)
- **Policyholder**: Intestatario della polizza
- **Insured**: Chi/cosa è assicurato
- **Coverage**: Cosa è coperto
- **Premium**: Quanto paga l'assicurato
- **Deductible**: Franchigia (quanto paga assicurato prima che assicurazione paghi)
- **Policy Period**: Durata (start date → expiration date)

```csharp
public class Policy
{
    public int Id { get; set; }
    public string PolicyNumber { get; set; }      // POL-2024-001234
    public int CustomerId { get; set; }
    public PolicyType Type { get; set; }          // Auto, Health, Life
    public decimal Premium { get; set; }          // €299.99/month
    public decimal CoverageAmount { get; set; }   // €50,000 max
    public decimal Deductible { get; set; }       // €500 franchigia
    public DateTime StartDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public PolicyStatus Status { get; set; }      // Active, Expired, Cancelled
}

public enum PolicyType
{
    Auto,
    Home,
    Health,
    Life,
    Travel
}

public enum PolicyStatus
{
    Draft,
    Active,
    Suspended,
    Expired,
    Cancelled
}
```

---

#### **Premium (Premio Assicurativo)**

**Definizione**: L'importo che l'assicurato paga periodicamente.

**Frequenze comuni:**

- Monthly (mensile)
- Quarterly (trimestrale)
- Semi-annually (semestrale)
- Annually (annuale)

**Calcolo (semplificato):**

```csharp
public decimal CalculatePremium(Policy policy)
{
    decimal basePremium = policy.CoverageAmount * 0.02m; // 2% del coverage
    
    // Fattori di rischio
    decimal riskMultiplier = 1.0m;
    
    if (policy.Type == PolicyType.Auto)
    {
        var driver = GetDriver(policy);
        if (driver.Age < 25) riskMultiplier += 0.5m;        // Giovani +50%
        if (driver.AccidentHistory > 0) riskMultiplier += 0.3m; // Incidenti +30%
    }
    
    // Sconto per franchigia alta
    if (policy.Deductible > 1000m)
        riskMultiplier -= 0.1m; // -10%
    
    return basePremium * riskMultiplier;
}
```

---

#### **Claim (Richiesta di Risarcimento)**

**Definizione**: Richiesta dell'assicurato per ottenere rimborso per un danno coperto.

**Flusso tipico:**

1. Assicurato subisce danno (incidente, furto, malattia)
2. Apre un Claim
3. Fornisce documentazione
4. Compagnia valuta (Underwriting review)
5. Approva/Rifiuta
6. Se approvato → Payout (pagamento)

```csharp
public class Claim
{
    public int Id { get; set; }
    public string ClaimNumber { get; set; }      // CLM-2024-005678
    public int PolicyId { get; set; }
    public Policy Policy { get; set; }
    
    public DateTime IncidentDate { get; set; }    // Quando è successo
    public DateTime FiledDate { get; set; }       // Quando è stato aperto claim
    public string Description { get; set; }       // Descrizione danno
    public decimal ClaimedAmount { get; set; }    // Quanto chiede assicurato
    public decimal ApprovedAmount { get; set; }   // Quanto approva assicurazione
    
    public ClaimStatus Status { get; set; }
    public List<ClaimDocument> Documents { get; set; }
}

public enum ClaimStatus
{
    Submitted,      // Appena inviato
    UnderReview,    // In valutazione
    Approved,       // Approvato
    Rejected,       // Rifiutato
    Paid            // Pagato
}
```

---

#### **Underwriting (Sottoscrizione/Valutazione Rischio)**

**Definizione**: Processo di valutazione del rischio per decidere se assicurare e a quale prezzo.

**Cosa valuta un underwriter:**

- Storia assicurativa dell'assicurato
- Fattori di rischio (età, salute, professione)
- Importo di coverage richiesto
- Statistiche attuariali

```csharp
public class UnderwritingDecision
{
    public int PolicyId { get; set; }
    public UnderwritingResult Result { get; set; }
    public decimal SuggestedPremium { get; set; }
    public string Notes { get; set; }
    public List<string> RiskFactors { get; set; }
}

public enum UnderwritingResult
{
    Approved,               // Approvato a condizioni standard
    ApprovedWithConditions, // Approvato ma con restrizioni
    Declined                // Rifiutato (rischio troppo alto)
}

// Esempio semplificato
public UnderwritingDecision EvaluatePolicy(PolicyApplication application)
{
    var decision = new UnderwritingDecision();
    var riskScore = 0;
    
    // Fattori di rischio
    if (application.ApplicantAge < 25 || application.ApplicantAge > 65)
    {
        riskScore += 2;
        decision.RiskFactors.Add("Age outside optimal range");
    }
    
    if (application.PreviousClaims > 2)
    {
        riskScore += 3;
        decision.RiskFactors.Add($"Multiple previous claims ({application.PreviousClaims})");
    }
    
    // Decisione
    if (riskScore <= 3)
    {
        decision.Result = UnderwritingResult.Approved;
        decision.SuggestedPremium = CalculateStandardPremium(application);
    }
    else if (riskScore <= 6)
    {
        decision.Result = UnderwritingResult.ApprovedWithConditions;
        decision.SuggestedPremium = CalculateStandardPremium(application) * 1.5m; // +50%
        decision.Notes = "Higher premium due to risk factors";
    }
    else
    {
        decision.Result = UnderwritingResult.Declined;
        decision.Notes = "Risk too high for standard coverage";
    }
    
    return decision;
}
```

---

### Terminologia Quick Reference

| Termine Inglese | Italiano | Significato |
| --- | --- | --- |
| **Policy** | Polizza | Contratto assicurativo |
| **Premium** | Premio | Pagamento periodico |
| **Claim** | Sinistro/Richiesta risarcimento | Richiesta di pagamento |
| **Underwriting** | Sottoscrizione | Valutazione rischio |
| **Deductible** | Franchigia | Quota a carico assicurato |
| **Coverage** | Copertura | Cosa è assicurato |
| **Policyholder** | Contraente | Chi paga la polizza |
| **Insured** | Assicurato | Chi/cosa è protetto |
| **Payout** | Indennizzo | Pagamento assicurazione |
| **Actuary** | Attuario | Esperto che calcola rischi |

---

### Quick Q&A - Insurance Basics

**Q**: Spiega Policy, Premium, Claim.

**A (30 sec)**:

"**Policy** (polizza) è il contratto assicurativo con coverage amount (quanto è coperto) e durata. **Premium** (premio) è il pagamento periodico dell'assicurato alla compagnia. **Claim** (sinistro) è quando l'assicurato richiede un rimborso per un danno coperto dalla polizza. Il processo è: l'assicurato paga premium regolarmente, se succede qualcosa apre un claim, underwriter valuta, se approvato c'è il payout meno la franchigia (deductible)."

---

## PARTE 9: EXTENDED QUICK Q&A - 20 DOMANDE AGGIUNTIVE

### Q21: Cosa sono le Migrations in EF Core?

**A (30 sec)**:

"Le Migrations sono il modo di EF Core per evolvere lo schema del database in modo versionato. Creo una migration con `dotnet ef migrations add NomeMigration` che genera codice Up/Down. Applico con `dotnet ef database update`. In produzione uso script SQL generati con `dotnet ef migrations script` invece di update diretto. Ogni migration è una classe con Up (applica cambiamenti) e Down (rollback). Mai modificare migration già applicate, sempre crearne una nuova."

---

### Q22: Differenza tra Lazy Loading e Eager Loading in EF?

**A (30 sec)**:

"**Eager Loading** carica le entità correlate subito con la query principale usando `.Include()` - una query con JOIN. **Lazy Loading** carica i dati correlati solo quando accedi alla proprietà navigation - query separate on-demand. Eager è meglio quando sai che userai i dati (evita N+1). Lazy può causare N+1 se non stai attento. In [ASP.NET](http://ASP.NET) Core API disabilito sempre Lazy Loading perché il DbContext viene disposed prima che le navigation properties vengano serializzate."

---

### Q23: Cos'è il Pattern Unit of Work?

**A (30 sec)**:

"Unit of Work coordina multiple operazioni su repository come una singola transazione. In EF Core, il DbContext è GIÀ un Unit of Work: traccia tutte le modifiche e `SaveChangesAsync()` le salva in una transazione atomica. Se implemento Unit of Work manualmente, creo un'interfaccia che espone repository e ha un metodo CommitAsync che chiama SaveChanges una volta sola. Utile quando ho logica complessa che modifica multiple entità e deve essere atomica."

---

### Q24: Come implementi Soft Delete?

**A (30 sec)**:

"Soft Delete significa marcare record come cancellati invece di eliminarli fisicamente. Aggiungo campo `IsDeleted` (bool) e `DeletedAt` (DateTime?). Invece di `DbSet.Remove()`, faccio `entity.IsDeleted = true`. Configuro Query Filter globale in OnModelCreating: `modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted)` così tutte le query automaticamente escludono i deleted. Per vedere anche i deleted: `.IgnoreQueryFilters()`. Vantaggi: audit trail, possibilità di restore."

---

### Q25: Cos'è CORS e come lo configuri?

**A (30 sec)**:

"**CORS** (Cross-Origin Resource Sharing) permette a un frontend su dominio diverso di chiamare la tua API. Senza CORS, browser blocca le chiamate cross-origin per sicurezza. Configuro in Program.cs con `AddCors` e specifico origini permesse. In sviluppo uso `AllowAnyOrigin()`, in produzione specifico domini esatti. Ordine nella pipeline: CORS deve essere PRIMA di Authorization. Esempio: [`builder.Services](http://builder.Services).AddCors(o => o.AddPolicy(\"AllowFrontend\", b => b.WithOrigins(\"[https://myapp.com\").AllowAnyMethod()](https://myapp.com\").AllowAnyMethod())))`"

---

### Q26: Differenza tra Authentication Middleware e Authorization Middleware?

**A (30 sec)**:

"**Authentication Middleware** (UseAuthentication) identifica CHI è l'utente - legge token JWT/cookie e popola `User` nel HttpContext. **Authorization Middleware** (UseAuthorization) verifica COSA può fare - controlla se l'utente ha i permessi per accedere all'endpoint (`[Authorize]`, policies, roles). Ordine CRITICO: Authentication PRIMA di Authorization. Se inverto, Authorization non sa chi è l'utente e blocca tutto."

---

### Q27: Cos'è Middleware e come ne creo uno custom?

**A (30 sec)**:

"Middleware è un componente nella request pipeline che processa request/response. Ha un `RequestDelegate _next` che è il middleware successivo. Implemento `InvokeAsync(HttpContext context)` dove faccio logica PRIMA di `await _next(context)` (request processing) e DOPO (response processing). Posso anche 'short-circuit' la pipeline non chiamando _next. Registro con `app.UseMiddleware<MyMiddleware>()`. Esempi: logging, exception handling, authentication."

---

### Q28: Cosa sono FluentValidation e DataAnnotations?

**A (30 sec)**:

"Entrambi validano input. **DataAnnotations** sono attributi sulle proprietà: `[Required]`, `[EmailAddress]`, `[Range(1,100)]`. Semplici ma limitati. **FluentValidation** è una libreria esterna più potente: validazione in classi separate, regole complesse, validazione asincrona, messaggi custom. FluentValidation ha miglior testabilità e separazione concerns. DataAnnotations per casi semplici, FluentValidation per validazione complessa o business rules."

---

### Q29: Cos'è il problema delle Circular Dependencies?

**A (30 sec)**:

"Circular dependency è quando Service A dipende da Service B che dipende da Service A. Il DI container non riesce a crearli (quale istanziare prima?). Soluzioni: 1) Refactor - spesso indica design smell, sposta logica comune in un terzo service. 2) Usa interfaccia + lazy loading. 3) Usa `IServiceProvider` per risolvere manualmente. Ma la soluzione migliore è ripensare il design - di solito una dipendenza circolare indica violazione di Single Responsibility."

---

### Q30: Differenza tra Controller e Minimal API?

**A (30 sec)**:

"**Controller** (MVC pattern) organizza endpoint in classi con metodi action. Più strutturato, supporta model binding, filters, convenzioni. **Minimal API** definisce endpoint direttamente con `app.MapGet/Post` - meno boilerplate, più funzionale. Minimal API è meglio per microservizi piccoli o API semplici. Controller è meglio per applicazioni grandi con molti endpoint e logica complessa. Performance quasi identica. Minimal API è trend moderno (.NET 6+)."

---

### Q31: Cos'è Caching in-memory vs Distributed?

**A (30 sec)**:

"**In-memory cache** (IMemoryCache) memorizza dati nella RAM del server. Velocissimo ma limitato a un'istanza - se hai multiple server, ogni server ha cache separata. **Distributed cache** (IDistributedCache con Redis) condivide cache tra tutti i server. Uso in-memory per dati che cambiano poco e l'inconsistenza tra server è accettabile. Uso distributed (Redis) per session management o quando scaling orizzontale con load balancer."

---

### Q32: Cosa sono Health Checks?

**A (30 sec)**:

"Health Checks sono endpoint che verificano se l'applicazione è 'sana' - database raggiungibile, servizi esterni up, memoria sufficiente. Aggiungo con `AddHealthChecks()` e `MapHealthChecks(\"/health\")`. Posso avere checks custom: database ping, Redis connection, API esterna. Kubernetes/Docker usa health checks per sapere se riavviare container. Liveness check = app viva? Readiness check = app pronta a ricevere traffico? Critici per produzione cloud."

---

### Q33: Differenza tra Stateless e Stateful services?

**A (30 sec)**:

"**Stateless**: ogni request è indipendente, il server non ricorda lo stato tra chiamate. REST API sono stateless per design. Vantaggi: facile da scalare (load balancer manda request a qualsiasi server), failure recovery semplice. **Stateful**: server mantiene stato tra request (es. session). Svantaggio: scaling complesso (serve sticky sessions o distributed session storage). In architetture moderne preferisco stateless + token JWT invece di session server-side."

---

### Q34: Cos'è Rate Limiting?

**A (30 sec)**:

"Rate Limiting limita quante request un client può fare in un periodo (es. 100 request/minuto). Previene abuse, DDoS, garantisce fair usage. In [ASP.NET](http://ASP.NET) Core 7+ uso middleware built-in `AddRateLimiter()`. Posso configurare per IP, per user, per endpoint. Strategie: Fixed Window (100/minuto), Sliding Window (più fair), Token Bucket. Quando limite superato ritorno 429 Too Many Requests. Essenziale per API pubbliche."

---

### Q35: Cosa sono ActionFilters?

**A (30 sec)**:

"ActionFilters sono attributi che eseguono logica PRIMA o DOPO un'action del controller. Implemento `IActionFilter` con `OnActionExecuting` (prima) e `OnActionExecuted` (dopo). Usi comuni: logging, caching, validazione, modifica response. Esempio: `[ValidateModelState]` controlla ModelState.IsValid automaticamente. Posso applicarli a singola action, intero controller, o globalmente. Utili per cross-cutting concerns senza duplicare codice."

---

### Q36: Differenza tra AddScoped e AddDbContext?

**A (30 sec)**:

"`AddDbContext<T>()` è shorthand per `AddScoped<T>()` con configurazione specifica per DbContext. DbContext È sempre scoped perché deve vivere per una request (Unit of Work) e poi essere disposed. AddDbContext accetta anche options per connection string e configurazione. In pratica sono equivalenti ma AddDbContext è più leggibile e idiomatico per EF Core. Uso `AddDbContext<AppDbContext>(opts => opts.UseSqlServer(connStr))`."

---

### Q37: Cos'è il Pattern Options in [ASP.NET](http://ASP.NET) Core?

**A (30 sec)**:

"Options Pattern carica configurazione da appsettings.json in classi strongly-typed. Creo una classe `MySettings`, la popolo con [`builder.Services](http://builder.Services).Configure<MySettings>(builder.Configuration.GetSection(\"MySettings\"))`, e la inietto con `IOptions<MySettings>`. Vantaggi: type-safety, intellisense, validazione. Uso `IOptions` per singleton, `IOptionsSnapshot` per scoped (refresh ogni request), `IOptionsMonitor` per hot reload (cambiamenti a runtime)."

---

### Q38: Cosa sono Hosted Services e Background Services?

**A (30 sec)**:

"**IHostedService** è un servizio che parte quando l'applicazione si avvia e si ferma quando si chiude. **BackgroundService** è una classe base che implementa IHostedService e fornisce `ExecuteAsync` per logica long-running. Usi: job scheduling (con Quartz), processing code, listener message queue. Registra con `AddHostedService<MyBackgroundService>()`. Attenzione: errori in ExecuteAsync crashano l'app, gestisci sempre le exception."

---

### Q39: Differenza tra EF Core e Dapper?

**A (30 sec)**:

"**EF Core** è un ORM full-featured: change tracking, migrations, lazy loading, LINQ to SQL. Astrazione completa dal database. **Dapper** è un micro-ORM: esegui SQL raw e mappa a oggetti C#. Nessun tracking, nessuna astrazione. Dapper è più veloce (10-50% meno overhead), ma devi scrivere SQL manualmente. Uso EF per CRUD standard e business logic. Uso Dapper per query complesse ottimizzate, report, bulk operations dove performance è critica."

---

### Q40: Come testi un API endpoint (integration test)?

**A (30 sec)**:

"Uso `WebApplicationFactory<Program>` che crea un test server in-memory. Inietto dipendenze mock o uso database in-memory. Chiamo endpoint con `HttpClient`, verifico status code e response body. Esempio: `var response = await client.PostAsJsonAsync(\"/api/users\", dto); Assert.Equal(HttpStatusCode.Created, response.StatusCode);`. Posso anche verificare che il dato sia stato salvato nel database di test. Più lenti di unit test ma testano l'intera pipeline."

---

## RIEPILOGO COMPLETO - CHECKLIST FINALE

### ✅ Fondamenti C# e .NET

- [x]  OOP (Encapsulation, Inheritance, Polymorphism, Abstraction)
- [x]  Class vs Struct vs Record
- [x]  Interface vs Abstract Class
- [x]  Stack vs Heap, Boxing/Unboxing
- [x]  Garbage Collector (Generational, Gen 0/1/2)
- [x]  IDisposable e using statement

### ✅ LINQ e Collections

- [x]  IEnumerable vs IQueryable
- [x]  Deferred vs Immediate execution
- [x]  First vs Single, Any vs Count
- [x]  SelectMany, GroupBy, Joins
- [x]  Problema N+1 e soluzioni

### ✅ Async e Concorrenza

- [x]  Async/Await (come funziona, state machine)
- [x]  Task<T>, ValueTask<T>
- [x]  Async vs Multithreading
- [x]  Deadlock e come evitarlo
- [x]  CancellationToken
- [x]  Task.WhenAll, Task.WhenAny
- [x]  lock, SemaphoreSlim, Interlocked
- [x]  Concurrent Collections

### ✅ [ASP.NET](http://ASP.NET) Core

- [x]  Request Pipeline e Middleware (ordine critico)
- [x]  Dependency Injection (Scoped/Singleton/Transient)
- [x]  REST API (GET/POST/PUT/PATCH/DELETE)
- [x]  Status Codes (200/201/400/401/403/404/500)
- [x]  Model Binding e Validation
- [x]  Authentication vs Authorization
- [x]  JWT Token
- [x]  CORS
- [x]  Filters e Action Filters

### ✅ Entity Framework Core

- [x]  DbContext, DbSet, Change Tracking
- [x]  Migrations
- [x]  Include (Eager Loading) vs Lazy Loading
- [x]  AsNoTracking (quando e perché)
- [x]  Repository Pattern
- [x]  Unit of Work
- [x]  Soft Delete

### ✅ SQL Server

- [x]  JOIN types (INNER, LEFT, RIGHT, FULL OUTER)
- [x]  Indexes (Clustered, Non-Clustered)
- [x]  Transactions e Isolation Levels
- [x]  Window Functions
- [x]  CTEs (Common Table Expressions)
- [x]  Execution Plans

### ✅ Design Patterns e Architecture

- [x]  SOLID Principles
- [x]  Repository Pattern
- [x]  Strategy Pattern
- [x]  Factory Pattern
- [x]  Observer Pattern
- [x]  Singleton Pattern
- [x]  Decorator Pattern
- [x]  Clean Architecture (Domain/Application/Infrastructure)

### ✅ Infrastruttura e Tools

- [x]  Redis Caching (Cache-Aside, Write-Through)
- [x]  Message Brokers (RabbitMQ vs Kafka)
- [x]  Microservices (comunicazione, resilience, API Gateway)
- [x]  gRPC vs REST
- [x]  Circuit Breaker Pattern

### ✅ Testing

- [x]  Unit Tests (xUnit, Moq)
- [x]  Integration Tests (WebApplicationFactory)
- [x]  AAA Pattern (Arrange, Act, Assert)
- [x]  Test Naming Conventions

### ✅ Tecnologie Aggiuntive (Insurance Domain)

- [x]  [**Quartz.NET**](http://Quartz.NET) - Job Scheduling (Job, Trigger, Scheduler, Cron)
- [x]  **WCF** - SOAP Services (legacy, ABC, perché obsoleto)
- [x]  **gRPC** - RPC moderno (Protobuf, streaming)
- [x]  **CSV Processing** - CsvHelper, import/export bulk
- [x]  **Insurance Basics** - Policy, Premium, Claim, Underwriting

### ✅ Quick Q&A - 40 Domande Preparate

- [x]  20 domande fondamentali (OOP, Async, DI, REST, SQL, Patterns)
- [x]  20 domande avanzate (EF, Middleware, Testing, Caching, Architecture)

---

## 🎯 STRATEGIA FINALE 7 GIORNI

### Giorno 1-2: Core Foundations

- OOP, C# Types, Memory Management
- LINQ deep dive
- Async/Await theory + practice
- **Pratica**: Scrivi codice async, esperimenta con Task.WhenAll

### Giorno 3-4: [ASP.NET](http://ASP.NET) Core

- Request Pipeline, Middleware (ordine!)
- DI Lifetimes (Scoped/Singleton/Transient)
- REST API, Status Codes
- JWT Authentication
- **Pratica**: Crea un'API semplice con auth

### Giorno 5: Database

- SQL JOINs, Indexes, Transactions
- EF Core (Include, AsNoTracking, N+1)
- Migrations
- **Pratica**: Ottimizza una query lenta

### Giorno 6: Patterns e Architecture

- SOLID principles (esempi concreti)
- Design Patterns (Repository, Strategy, Factory)
- Clean Architecture overview
- **Pratica**: Refactora codice per applicare pattern

### Giorno 7: Review e Quick Q&A

- Ripassa tutte le 40 Quick Q&A
- **PRATICA ORALE**: Rispondi a voce alta come in colloquio
- Rivedi i tuoi progetti CV e prepara spiegazioni
- Insurance domain basics + [Quartz.NET](http://Quartz.NET)

---

## 📚 RISORSE CONSIGLIATE

**Per approfondire:**

- Microsoft Docs: [https://docs.microsoft.com/aspnet/core](https://docs.microsoft.com/aspnet/core)
- Entity Framework Core Docs
- "C# in Depth" - Jon Skeet (libro)
- Pluralsight/Udemy courses on [ASP.NET](http://ASP.NET) Core

**Practice:**

- LeetCode/HackerRank per algoritmi
- Crea un progetto personale che usi: [ASP.NET](http://ASP.NET) Core + EF Core + JWT + Redis
- Contribuisci a progetto open source .NET

---

## ✅ SEI PRONTO QUANDO...

- ✅ Puoi spiegare ogni Quick Q&A in 30-60 secondi
- ✅ Puoi disegnare la Request Pipeline a memoria
- ✅ Sai quando usare Scoped vs Singleton vs Transient
- ✅ Capisci perché `async void` è pericoloso
- ✅ Puoi spiegare il problema N+1 con esempio SQL
- ✅ Conosci differenza tra authentication e authorization
- ✅ Sai implementare Repository Pattern da zero
- ✅ Puoi scrivere un Cron expression per "ogni lunedì alle 9:00"
- ✅ Capisci quando usare gRPC invece di REST

---

**ULTIMO CONSIGLIO**: Durante il colloquio, se non sai la risposta esatta, **ragiona ad alta voce**. Mostra il tuo processo di pensiero. È meglio dire "Non ho mai usato X, ma penso che funzioni così perché..." che stare in silenzio o inventare.

**IN BOCCA AL LUPO! 🚀**

---

# PARTE 10: STRING, STRINGBUILDER E COLLECTIONS {#string-collections}

## 1. String - Immutabilità e Performance

### String è Reference Type con Semantica di Valore

```csharp
// String è una CLASSE nello Heap
string name = "John";     // Allocazione Heap
name = name + " Doe";     // NUOVA allocazione (immutabile)
                          // "John" abbandonato (GC lo raccoglierà)
```

**Immutabilità**: Ogni operazione crea una NUOVA string.

```csharp
string original = "Hello";
string modified = original.ToUpper();  // NUOVA string "HELLO"
// original rimane "Hello" (invariato)
```

### String Interning - Ottimizzazione

```csharp
string a = "Hello";
string b = "Hello";
Console.WriteLine(ReferenceEquals(a, b)); // TRUE (stesso oggetto!)

// Ma runtime concat = nuovo oggetto
string c = "Hel" + "lo";  // Compile-time = interned
string d = "Hel";
string e = d + "lo";       // Runtime = nuovo oggetto
Console.WriteLine(ReferenceEquals(a, e)); // FALSE
```

### Metodi String Principali

```csharp
string text = "Hello World";

// Ricerca
text.Contains("World")         // true
text.StartsWith("Hello")       // true
text.IndexOf("o")              // 4

// Manipolazione (creano NUOVA string)
text.Substring(0, 5)           // "Hello"
text.Replace("World", "C#")    // "Hello C#"
text.ToUpper()                 // "HELLO WORLD"
"  text  ".Trim()             // "text"

// Split e Join
"a,b,c".Split(',')            // ["a", "b", "c"]
string.Join(", ", array)       // "a, b, c"

// Interpolation
string msg = $"Name: {name}, Age: {age}";
```

### ⚠️ Problema: Concatenazioni in Loop

```csharp
// ❌ DISASTER - 1000 allocazioni!
string result = "";
for (int i = 0; i < 1000; i++)
    result += i.ToString();  // O(n²) complexity

// ✅ SOLUTION - StringBuilder
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++)
    sb.Append(i);  // O(n)
string result = sb.ToString();
```

---

## 2. StringBuilder - Mutabile ed Efficiente

```csharp
using System.Text;

var sb = new StringBuilder();
sb.Append("Hello");
sb.Append(" ");
sb.Append("World");
sb.AppendLine("!");

string final = sb.ToString();  // "Hello World!\n"
```

### StringBuilder - Metodi

```csharp
var sb = new StringBuilder(100);  // Capacity iniziale

sb.Append("text");              // Aggiunge
sb.AppendLine("line");          // Aggiunge + newline
sb.Insert(0, "Start:");         // Inserisce
sb.Remove(0, 6);                // Rimuove
sb.Replace("old", "new");       // Replace
sb.Clear();                     // Svuota

Console.WriteLine(sb.Length);   // Caratteri attuali
Console.WriteLine(sb.Capacity); // Spazio allocato
```

### Quando Usare StringBuilder

- ✅ Loop con concatenazioni
- ✅ Costruzione incrementale
- ✅ Generazione HTML/SQL/JSON
- ❌ Poche operazioni (< 5)
- ❌ String immutabilità necessaria

---

## 3. Collections - Panoramica

### Array - Fixed Size

```csharp
int[] numbers = new int[5];                // Fixed
int[] scores = { 10, 20, 30, 40, 50 };

int first = scores[0];                     // O(1)
scores[2] = 99;

foreach (int score in scores)
    Console.WriteLine(score);
```

**Pro**: Performance O(1), memory contigua

**Contro**: Dimensione fissa, no insert/remove

---

### List<T> - Array Dinamico

```csharp
var list = new List<int>();

list.Add(1);                    // O(1) amortized
list.Insert(0, 0);              // O(n)
list.Remove(1);                 // O(n)
list.RemoveAt(0);               // O(n)

int value = list[0];            // O(1)
bool has = list.Contains(1);    // O(n)

list.Sort();                    // O(n log n)
list.Clear();
```

**Crescita dinamica**: Quando pieno, raddoppia capacity.

```csharp
var list = new List<int>(1000);  // Pre-alloca per performance
```

---

### Dictionary<K,V> - Hash Table

```csharp
var dict = new Dictionary<string, int>();

dict.Add("Alice", 25);          // O(1)
dict["Bob"] = 30;               // Add o update

if (dict.TryGetValue("Alice", out int age))
    Console.WriteLine(age);

bool has = dict.ContainsKey("Alice");  // O(1)
dict.Remove("Bob");                    // O(1)

// Iterazione
foreach (var (key, value) in dict)
    Console.WriteLine($"{key}: {value}");
```

**Quando usare**:

- Lookup veloce per chiave
- Relazioni key-value
- Cache, mapping

---

### HashSet<T> - Unique Values

```csharp
var set = new HashSet<int>();

set.Add(1);        // true
set.Add(1);        // false (già presente)

bool has = set.Contains(1);  // O(1)

// Operazioni insiemistiche
var set1 = new HashSet<int> { 1, 2, 3 };
var set2 = new HashSet<int> { 2, 3, 4 };

set1.UnionWith(set2);         // { 1, 2, 3, 4 }
set1.IntersectWith(set2);     // { 2, 3 }
set1.ExceptWith(set2);        // { 1 }
```

**Quando usare**:

- Rimuovere duplicati
- Verifiche esistenza veloci
- Operazioni matematiche su set

---

### Queue<T> - FIFO

```csharp
var queue = new Queue<string>();

queue.Enqueue("First");   // Aggiunge
queue.Enqueue("Second");

string first = queue.Dequeue();  // Rimuove "First"
string next = queue.Peek();      // Guarda senza rimuovere
```

### Stack<T> - LIFO

```csharp
var stack = new Stack<string>();

stack.Push("First");     // Aggiunge
stack.Push("Second");

string top = stack.Pop();   // Rimuove "Second"
string peek = stack.Peek(); // Guarda senza rimuovere
```

---

## Collections - Comparison Table

| Collection | Lookup | Insert | Remove | Ordered | Duplicates |
| --- | --- | --- | --- | --- | --- |
| **Array** | O(1) | N/A | N/A | ✅ | ✅ |
| **List<T>** | O(1) by index, O(n) by value | O(n) | O(n) | ✅ | ✅ |
| **Dictionary<K,V>** | O(1) | O(1) | O(1) | ❌ | Keys: ❌, Values: ✅ |
| **HashSet<T>** | O(1) | O(1) | O(1) | ❌ | ❌ |
| **Queue<T>** | O(n) | O(1) enqueue | O(1) dequeue | ✅ FIFO | ✅ |
| **Stack<T>** | O(n) | O(1) push | O(1) pop | ✅ LIFO | ✅ |

---

## Quick Q&A - String e Collections

**Q41**: Perché string è immutabile?

**A**: "String è immutabile per thread-safety e hash code consistency. Se modificabile, cambierebbe hash code rompendo Dictionary/HashSet. Immutabilità permette string interning per ottimizzare memoria. Ogni modifica crea nuovo oggetto - per questo StringBuilder per loop."

**Q42**: List vs Array?

**A**: "Array ha dimensione fissa, List cresce dinamicamente. Array è più veloce ma meno flessibile. List ha metodi utili (Add, Remove, Sort). Uso List per default, Array solo se dimensione fissa nota."

**Q43**: Dictionary vs List per lookup?

**A**: "Dictionary è O(1) lookup per chiave tramite hash table. List è O(n) scansione lineare. Uso Dictionary quando serve lookup veloce per chiave (cache, mapping). List quando ordine importante o no chiave naturale."

**Q44**: Quando uso HashSet?

**A**: "HashSet per valori unici con lookup O(1). Auto-ignora duplicati. Uso per: rimuovere duplicati, verifiche esistenza veloci, operazioni insiemistiche (Union/Intersect). È Dictionary senza valori."

# PARTE 12: GIT E VERSION CONTROL {#git-version-control}

## 1. Git - Fondamenti

### Cos'è Git?

**Git** è un sistema di version control **distribuito** per tracciare modifiche al codice.

**Concetti fondamentali:**

- **Repository**: Cartella che contiene tutto il progetto + storia
- **Commit**: Snapshot del progetto in un momento specifico
- **Branch**: Linea di sviluppo indipendente
- **Remote**: Repository su server (GitHub, GitLab, Azure DevOps)

---

### Workflow Base - I Comandi Essenziali

```bash
# 1. CLONARE un repository esistente
git clone https://github.com/company/project.git
cd project

# 2. VEDERE lo stato
git status  # Mostra file modificati, staged, untracked

# 3. AGGIUNGERE file al commit (staging)
git add Program.cs              # Singolo file
git add .                       # Tutti i file
git add *.cs                    # Tutti i .cs

# 4. COMMITTARE (salvare snapshot)
git commit -m "Fix: Risolto bug nel calcolo premio"

# 5. PUSH (inviare al server)
git push origin main            # Push su branch main

# 6. PULL (scaricare aggiornamenti)
git pull origin main            # Scarica + merge
```

---

### Branching - Sviluppo Parallelo

```bash
# Vedere i branch
git branch              # Branch locali
git branch -a           # Tutti i branch (locale + remote)

# Creare nuovo branch
git branch feature/add-policy-validation

# Switchare branch
git checkout feature/add-policy-validation
# O in Git moderno (2.23+)
git switch feature/add-policy-validation

# Creare E switchare in un comando
git checkout -b feature/fix-premium-calculation

# Eliminare branch
git branch -d feature/old-feature  # Solo se merged
git branch -D feature/old-feature  # Forza (anche se non merged)
```

---

### Merge - Unire Branch

```bash
# Scenario: Merge di feature branch in main

# 1. Switch a main
git checkout main

# 2. Pull per avere ultima versione
git pull origin main

# 3. Merge della feature
git merge feature/add-policy-validation

# 4. Push del merge
git push origin main
```

**Tipi di merge:**

- **Fast-forward**: Semplice spostamento del puntatore (no conflitti)
- **Three-way merge**: Crea un merge commit
- **Squash merge**: Comprime tutti i commit in uno solo

---

### Merge Conflicts - Risoluzione

```bash
# Durante merge, se c'è conflitto:
git merge feature/new-feature
# Auto-merging Policy.cs
# CONFLICT (content): Merge conflict in Policy.cs
# Automatic merge failed; fix conflicts and then commit.

# File con conflitto appare così:
```

```csharp
// Policy.cs
public decimal CalculatePremium()
{
<<<<<<< HEAD
    return BasePremium * 1.2m;  // Versione main
=======
    return BasePremium * 1.5m;  // Versione feature
>>>>>>> feature/new-feature
}
```

**Risoluzione:**

```bash
# 1. Apri file, risolvi manualmente scegliendo versione corretta
# 2. Rimuovi i marker (<<<<, ====, >>>>)
# 3. Testa il codice
# 4. Stage e commit

git add Policy.cs
git commit -m "Merge: Risolto conflitto in CalculatePremium"
git push
```

---

### Git Log - Storia Commit

```bash
# Log completo
git log

# Log compatto (una riga per commit)
git log --oneline

# Log con grafo branch
git log --oneline --graph --all

# Log di un file specifico
git log -- Policy.cs

# Vedere differenze tra commit
git diff HEAD~1 HEAD  # Ultimo commit vs precedente
git diff main feature/new-feature  # Tra branch
```

---

### .gitignore - Escludere File

```bash
# .gitignore per progetto .NET

# Build results
bin/
obj/
*.dll
*.exe

# User-specific files
*.suo
*.user
*.userosscache
.vs/

# NuGet
packages/
*.nupkg

# Secrets
appsettings.Development.json
appsettings.local.json
*.pfx

# OS files
.DS_Store
Thumbs.db
```

---

### GitFlow - Branching Strategy Aziendale

**Struttura tipica in azienda:**

```jsx
main (production)
  ↓
develop (sviluppo principale)
  ↓
feature/TICKET-123-add-policy-api
feature/TICKET-124-fix-premium-calc
bugfix/TICKET-125-claim-validation
hotfix/TICKET-126-critical-security
```

**Naming conventions:**

- `feature/` - Nuove funzionalità
- `bugfix/` - Fix di bug
- `hotfix/` - Fix urgenti in production
- `release/` - Preparazione release

**Esempio workflow:**

```bash
# 1. Crea feature da develop
git checkout develop
git pull
git checkout -b feature/INSURANCE-456-add-claim-api

# 2. Lavora e committa
git add .
git commit -m "feat(claims): Add ClaimController with POST endpoint"

# 3. Push feature
git push origin feature/INSURANCE-456-add-claim-api

# 4. Crea Pull Request su develop (via web interface)
# 5. Code review
# 6. Merge se approvato
```

---

### Commit Messages - Best Practices

**Formato Conventional Commits:**

```bash
<type>(<scope>): <subject>

<body>

<footer>
```

**Types:**

- `feat`: Nuova feature
- `fix`: Bug fix
- `refactor`: Refactoring (no new feature, no fix)
- `docs`: Documentazione
- `test`: Aggiunta test
- `chore`: Maintenance (dependencies, config)

**Esempi:**

```bash
git commit -m "feat(policy): Add premium calculation for health insurance"

git commit -m "fix(claims): Fix null reference in ClaimService.ProcessAsync"

git commit -m "refactor(auth): Extract JWT validation to separate service"

git commit -m "test(policy): Add unit tests for PolicyService"

git commit -m "chore: Update Entity Framework Core to 8.0.1"
```

---

### Pull Request (PR) - Code Review

**Processo tipico:**

1. **Crea feature branch**
2. **Sviluppa e committa**
3. **Push su remote**
4. **Apri Pull Request** (GitHub/GitLab/Azure DevOps)
5. **Descrivi le modifiche**:
    - Cosa fa?
    - Perché?
    - Come testare?
6. **Assegna reviewer**
7. **Attendi approvazione**
8. **Risolvi commenti**
9. **Merge** (squash, merge, rebase)

**Esempio descrizione PR:**

```markdown
## [INSURANCE-456] Add Claim Processing API

### What
Implementato endpoint POST /api/claims per sottomissione sinistri.

### Why
Permettere ai clienti di aprire claim direttamente dall'app mobile.

### How to test
1. POST /api/claims con body:
```

{

"policyId": 123,

"incidentDate": "2026-02-01",

"description": "Incidente auto",

"claimedAmount": 5000

}

```
2. Verificare che claim sia creato con status "Submitted"
3. Verificare email di conferma inviata

### Checklist
- [x] Unit tests aggiunti
- [x] Integration test aggiunto
- [x] Documentazione API aggiornata
- [x] Migration EF creata
```

---

### Comandi Avanzati Utili

```bash
# STASH - Salvare modifiche temporaneamente
git stash                    # Salva modifiche
git stash list               # Vedi stash salvati
git stash pop                # Ripristina ultimo stash
git stash apply stash@{1}    # Ripristina stash specifico

# RESET - Annullare commit
git reset --soft HEAD~1      # Annulla ultimo commit, mantiene modifiche staged
git reset --hard HEAD~1      # Annulla ultimo commit, CANCELLA modifiche

# REVERT - Annullare commit senza riscrivere storia
git revert abc123            # Crea nuovo commit che annulla abc123

# CHERRY-PICK - Copiare commit specifico
git cherry-pick abc123       # Applica commit abc123 sul branch corrente

# REBASE - Riscrivere storia (pulire commit)
git rebase -i HEAD~3         # Interactive rebase ultimi 3 commit
```

---

### Git in Team - Best Practices

#### ✅ DO:

- **Pull frequentemente** (almeno ogni mattina)
- **Commit piccoli e atomici** (una logica per commit)
- **Messaggi commit descrittivi**
- **Branch per ogni feature/bug**
- **Code review** prima di merge
- **Test prima di push**
- **Risolvi conflitti localmente** prima di push

#### ❌ DON'T:

- **Committare su main direttamente** (usa branch)
- **Commit giganti** (100 file modificati)
- **Messaggi vaghi** ("fix", "update", "changes")
- **Push codice che non compila**
- **Ignorare merge conflicts** (risolvi subito)
- **Force push su branch condivisi** (`git push --force`)

---

## Quick Q&A - Git

**Q45**: Come funziona Git flow?

**A (30 sec)**:

"GitFlow è un branching model. Branch principali: **main** (production) e **develop** (sviluppo). Per ogni feature creo branch `feature/` da develop, lavoro, committa, faccio pull request. Dopo code review, merge in develop. Per release, creo `release/` da develop, testo, merge in main E develop. Hotfix urgenti: `hotfix/` da main, fix, merge in main e develop. Naming: `feature/TICKET-123-description` per tracciabilità."

**Q46**: Cosa fai quando hai merge conflict?

**A (30 sec)**:

"Quando faccio merge e Git dice CONFLICT, apro i file con `<<<<<<<`, `=======`, `>>>>>>>` marker. Analizzo entrambe le versioni, decido quale tenere (o combino), rimuovo i marker. Testo che il codice funzioni. Poi `git add file.cs`, `git commit`, `git push`. Se il conflitto è complesso, chiedo al collega che ha fatto l'altro commit. Mai committare con i marker conflict non risolti."

**Q47**: Differenza tra merge e rebase?

**A (30 sec)**:

"**Merge** crea un merge commit unendo due branch, preserva storia completa ma può essere 'sporca'. **Rebase** riscrive la storia spostando i tuoi commit sopra il branch target, storia lineare pulita ma cambia hash commit. Uso merge per feature branch → develop (tracciabilità). Uso rebase per aggiornare la mia feature con develop prima di PR (storia pulita). Mai rebase su branch pubblici condivisi."

---

# PARTE 13: AGILE E SCRUM {#agile-scrum}

## 1. Metodologia Agile - Principi

### Cos'è Agile?

**Agile** è un approccio iterativo e incrementale allo sviluppo software.

**Manifesto Agile - 4 Valori:**

1. **Individui e interazioni** > processi e strumenti
2. **Software funzionante** > documentazione esaustiva
3. **Collaborazione col cliente** > negoziazione contratti
4. **Rispondere al cambiamento** > seguire un piano

**12 Principi chiave (selezionati):**

- Consegne frequenti di software funzionante (2-4 settimane)
- Accogliere cambiamenti anche tardivi
- Collaborazione quotidiana business/dev
- Team auto-organizzati
- Retrospettive regolari per miglioramento continuo

---

## 2. Scrum Framework

### Cos'è Scrum?

**Scrum** è il framework Agile più usato, basato su **sprint** iterativi.

```jsx
┌──────────────────────────────────────┐
│          PRODUCT BACKLOG             │
│  (lista prioritizzata funzionalità)  │
└────────────────┬─────────────────────┘
                 ↓
┌────────────────────────────────────┐
│      SPRINT PLANNING               │
│  (selezione items per sprint)      │
└────────────────┬───────────────────┘
                 ↓
┌────────────────────────────────────┐
│      SPRINT (2-4 settimane)        │
│  ┌──────────────────────────────┐  │
│  │   Daily Standup (15 min)     │  │
│  │   - Cosa ho fatto ieri?      │  │
│  │   - Cosa faccio oggi?        │  │
│  │   - Ho blocchi?              │  │
│  └──────────────────────────────┘  │
└────────────────┬───────────────────┘
                 ↓
┌────────────────────────────────────┐
│      SPRINT REVIEW                 │
│  (demo al cliente)                 │
└────────────────┬───────────────────┘
                 ↓
┌────────────────────────────────────┐
│      SPRINT RETROSPECTIVE          │
│  (cosa migliorare?)                │
└────────────────────────────────────┘
```

---

### Ruoli Scrum

#### **1. Product Owner (PO)**

- **Cosa fa**: Definisce COSA costruire
- Gestisce Product Backlog
- Prioritizza funzionalità per business value
- Accetta/rifiuta deliverable
- **Esempio**: "Vogliamo API per calcolo premio assicurativo entro prossimo sprint"

#### **2. Scrum Master**

- **Cosa fa**: Facilita il processo Scrum
- Rimuove impedimenti
- Protegge team da distrazioni esterne
- Organizza cerimonie
- **Esempio**: "Ho visto che il DB di test è down, lo escalato a infra"

#### **3. Development Team (tu!)**

- **Cosa fa**: Costruisce il software
- Auto-organizzato (decide COME)
- Cross-functional (dev, test, deploy)
- 5-9 persone idealmente
- **Esempio**: "Prendo lo user story 'Calcolo premio' e lo completo entro giovedì"

---

### Artefatti Scrum

#### **1. Product Backlog**

Lista prioritizzata di **User Stories** (funzionalità).

**Formato User Story:**

```
Come [ruolo]
Voglio [funzionalità]
Così che [beneficio]

Acceptance Criteria:
- Criterio 1
- Criterio 2
- Criterio 3
```

**Esempio Insurance:**

```
User Story: Calcolo Premio Polizza Auto

Come Cliente
Voglio ricevere un preventivo per polizza auto online
Così che posso decidere se acquistare senza chiamare agente

Acceptance Criteria:
- Sistema calcola premio basato su: età, tipo auto, storia sinistri
- Preventivo mostrato entro 3 secondi
- Cliente può salvare preventivo per 30 giorni
- Email di conferma inviata con riepilogo

Story Points: 8
Priority: High
```

#### **2. Sprint Backlog**

Subset del Product Backlog selezionato per lo sprint corrente + task.

**Esempio:**

```
[INSURANCE-456] Calcolo Premio Polizza Auto (8 SP)
  Tasks:
  ☐ Creare PremiumCalculationService
  ☐ Implementare logica calcolo per età
  ☐ Implementare logica per tipo auto
  ☐ Aggiungere fattore rischio per storia sinistri
  ☐ Unit test per tutte le casistiche
  ☐ Integration test con API
  ☐ Aggiornare documentazione API
```

#### **3. Increment**

Versione **potenzialmente rilasciabile** del prodotto alla fine dello sprint.

---

### Cerimonie Scrum

#### **1. Sprint Planning (inizio sprint)**

**Durata**: 2-4 ore per sprint di 2 settimane

**Cosa succede**:

1. PO presenta top priority items dal backlog
2. Team discute e fa domande
3. Team stima effort (story points)
4. Team commitment: "Prendiamo questi 5 story per lo sprint"
5. Break down in task

**Esempio dialogo:**

```
PO: "Prossimo sprint dobbiamo fare calcolo premio polizza auto"
Dev: "Abbiamo già API per dati cliente? Serve integrazione CRM?"
PO: "Sì, API cliente esiste. No CRM, tutto interno."
Dev: "Ok, stimiamo 8 story points. Fattibile in questo sprint."
```

#### **2. Daily Standup (ogni giorno)**

**Durata**: 15 minuti MAX

**Formato** (3 domande per ogni persona):

1. Cosa ho fatto ieri?
2. Cosa faccio oggi?
3. Ho blocchi/impedimenti?

**Esempio:**

```
Dev 1: "Ieri ho completato PremiumCalculationService. Oggi scrivo unit test. Nessun blocco."

Dev 2: "Ieri ho iniziato integration con API cliente. Oggi continuo. Blocco: non ho accesso al DB di test."
Scrum Master: "Ok, lo risolvo entro un'ora."

Dev 3: "Ieri code review. Oggi inizio nuovo task validazione claim. Nessun blocco."
```

**⚠️ NON è un meeting di status per il manager!** È per sincronizzazione del team.

#### **3. Sprint Review (fine sprint)**

**Durata**: 1-2 ore

**Cosa succede**:

1. Team **demo** il software funzionante
2. PO **accetta o rifiuta** story
3. Stakeholder danno feedback
4. Aggiornamento Product Backlog

**Esempio:**

```
Dev: [mostra demo] "Ecco il calcolo premio. Inserisco età 25, auto Fiat Panda, nessun sinistro. Premio calcolato: €650/anno."
PO: "Ottimo! Ma manca la franchigia selezionabile."
Dev: "Quella era nello story successivo, non in questo sprint."
PO: "Ok, accetto questo story. Franchigia la facciamo prossimo sprint."
```

#### **4. Sprint Retrospective (fine sprint)**

**Durata**: 1 ora

**Cosa succede** (il team riflette):

1. Cosa è andato bene?
2. Cosa è andato male?
3. Cosa migliorare prossimo sprint?

**Esempio:**

```
Andato bene:
✅ Code review veloci (< 4 ore)
✅ Zero bug in production

Andato male:
❌ Testing environment down 2 giorni
❌ Requirements poco chiari su storia claim

Azioni per prossimo sprint:
→ Setup backup test environment
→ Refinement session con PO prima di sprint planning
```

---

### Story Points - Stima Sforzo

**Story Points** = unità di **complessità relativa** (non ore!).

**Scala Fibonacci**: 1, 2, 3, 5, 8, 13, 21

**Esempi Insurance:**

- **1 SP**: Aggiungere campo email a form esistente
- **3 SP**: Implementare validazione email con regex
- **5 SP**: Creare API endpoint GET /policies/{id}
- **8 SP**: Implementare calcolo premio con 3 fattori rischio
- **13 SP**: Integrare con sistema CRM esterno
- **21 SP**: Troppo grande! Splittare in story più piccole

**Planning Poker**: Team stima insieme, discussione se stime diverse.

---

### Definition of Done (DoD)

Criteri che **ogni** story deve soddisfare per essere "Done".

**Esempio DoD:**

```
✅ Codice scritto e committato
✅ Unit test coverage ≥ 80%
✅ Integration test passano
✅ Code review approvato (almeno 1 reviewer)
✅ Nessun bug critico
✅ Documentazione API aggiornata
✅ Deployed su environment di test
✅ PO ha testato e accettato
```

Senza DoD completo → story **non è done** → non conta per velocity.

---

### Velocity - Misurare Produttività

**Velocity** = story points completati per sprint.

**Esempio:**

- Sprint 1: 23 SP completati
- Sprint 2: 28 SP completati
- Sprint 3: 25 SP completati
- **Velocity media**: ~25 SP/sprint

**Uso**: Pianificare quanti story prendere nel prossimo sprint.

"Il team ha velocity 25 SP, quindi possiamo prendere circa 25 SP di story per il prossimo sprint."

---

## Agile in Pratica - Scenario Reale

**Progetto**: Sistema di gestione polizze assicurative

**Sprint 1 (2 settimane):**

```
Day 1 (Lunedì):
- Sprint Planning: Selezione 5 user stories (totale 26 SP)
- Story principale: "Calcolo premio polizza auto"

Day 2-9 (Lun-Ven x2):
- Daily standup ogni mattina 9:00-9:15
- Sviluppo, test, code review
- Impedimenti risolti da Scrum Master

Day 10 (Venerdì week 2):
- Sprint Review: Demo al PO e stakeholder
- 4/5 stories accettate (21 SP), 1 story spostata a prossimo sprint
- Retrospective: Identificati 2 miglioramenti

Weekend: Pausa

Day 11 (Lunedì): Nuovo sprint planning
```

---

## Quick Q&A - Agile/Scrum

**Q48**: Cos'è Agile e Scrum?

**A (30 sec)**:

"**Agile** è una filosofia di sviluppo iterativo e incrementale. **Scrum** è un framework Agile specifico basato su sprint di 2-4 settimane. Ogni sprint: planning (selezioniamo user stories), daily standup (sincronizzazione 15 min), sviluppo, review (demo), retrospective (miglioramento). Ruoli: Product Owner (cosa fare), Scrum Master (facilita), Dev Team (costruisce). Stimiamo con story points, tracciamo velocity per pianificare sprint futuri."

**Q49**: Cosa fai in un daily standup?

**A (30 sec)**:

"Daily standup dura 15 minuti, ogni giorno alla stessa ora. Ogni persona risponde 3 domande: 1) Cosa ho fatto ieri? 2) Cosa faccio oggi? 3) Ho blocchi? Esempio: 'Ieri ho completato l'API claim, oggi scrivo unit test, nessun blocco'. Se ho blocco (es. DB down), lo dico e Scrum Master lo risolve. NON è status report per manager, è sincronizzazione team. Si sta in piedi per essere brevi."

**Q50**: Differenza tra story points e ore?

**A (30 sec)**:

"**Story points** misurano complessità relativa, **ore** misurano tempo. Story points considerano: complessità tecnica, incertezza, effort. Scala Fibonacci: 1, 2, 3, 5, 8, 13. Esempio: task semplice = 1 SP, task medio = 5 SP. Vantaggi: più accurati (le ore variano per persona), velocità di stima, focus su valore non tempo. Velocity del team (SP/sprint) migliora nel tempo quando team si calibra."

# PARTE 14: ANALISI FUNZIONALE E DOCUMENTAZIONE {#analisi-documentazione}

## 1. Analisi Funzionale - Il Processo

### Cos'è l'Analisi Funzionale?

**Analisi Funzionale** = comprendere **COSA** il sistema deve fare (requisiti) prima di decidere **COME** costruirlo (implementazione).

**Obiettivi:**

- Trasformare requisiti business in specifiche tecniche
- Identificare entità e relazioni
- Definire flussi e casi d'uso
- Stimare effort e rischi

---

### Il Processo Step-by-Step

#### **Step 1: Raccolta Requisiti**

**Meeting con Product Owner/Cliente:**

```
PO: "Vogliamo che i clienti possano aprire un sinistro online."

Analista (tu):
- "Quali tipi di polizza possono fare sinistri?"
- "Ci sono limiti di tempo? (es. sinistro entro X giorni dall'incidente)"
- "Quali documenti devono allegare?"
- "Chi approva i sinistri? Sistema automatico o underwriter?"
- "Cosa succede se il sinistro supera il massimale?"
```

#### **Step 2: Analisi e Modellazione**

**Output: Documento di Analisi Funzionale**

```markdown
# Analisi Funzionale: Gestione Sinistri Online

## 1. Scope
Permettere ai clienti con polizza attiva di aprire sinistri online.

## 2. Attori
- **Cliente**: Apre sinistro, carica documenti
- **Underwriter**: Valuta e approva/rifiuta sinistro
- **Sistema**: Valida dati, calcola importo approvabile, invia notifiche

## 3. Precondizioni
- Cliente autenticato
- Cliente ha almeno una polizza attiva
- Polizza copre il tipo di danno dichiarato

## 4. Flusso Principale

1. Cliente seleziona polizza da lista polizze attive
2. Sistema mostra form sinistro con:
   - Data incidente (obbligatorio, date picker)
   - Tipo danno (dropdown: Auto, Salute, Casa, altro)
   - Descrizione (textarea, max 1000 char)
   - Importo richiesto (number, €, > 0)
   - Upload documenti (PDF/JPG/PNG, max 5 file, max 10MB tot)
3. Cliente compila e submit
4. Sistema valida:
   - Data incidente ≤ oggi
   - Data incidente ≥ inizio polizza
   - Tipo danno coperto da polizza
   - Importo ≤ massimale polizza
5. Sistema crea sinistro con status "Submitted"
6. Sistema invia email a:
   - Cliente: "Sinistro #{id} ricevuto, in valutazione"
   - Underwriter: "Nuovo sinistro da valutare"
7. Sistema mostra conferma con numero sinistro

## 5. Flussi Alternativi

**5a. Data incidente fuori periodo polizza**
- Sistema mostra errore: "Data incidente non coperta da polizza"
- Cliente può correggere o annullare

**5b. Tipo danno non coperto**
- Sistema mostra warning: "Questo danno potrebbe non essere coperto"
- Cliente può proseguire (sinistro creato, underwriter decide)

**5c. Importo > massimale**
- Sistema mostra warning: "Importo richiesto (€10.000) supera massimale polizza (€5.000)"
- Sistema suggerisce: "Importo massimo approvabile: €5.000 - franchigia €500 = €4.500"
- Cliente può modificare o confermare

## 6. Regole Business

- **RB1**: Sinistro può essere aperto solo su polizza con status "Active"
- **RB2**: Data incidente deve essere nel periodo di validità polizza
- **RB3**: Franchigia viene sottratta dall'importo approvato (non dall'importo richiesto)
- **RB4**: Sinistri > €10.000 richiedono approvazione manager oltre underwriter
- **RB5**: Massimo 3 sinistri per polizza all'anno
- **RB6**: Documenti obbligatori: fattura/ricevuta + foto danno

## 7. Modello Dati

### Entità: Claim
```

CREATE TABLE Claims (

Id INT PRIMARY KEY IDENTITY,

ClaimNumber NVARCHAR(20) UNIQUE,  -- CLM-YYYY-######

PolicyId INT NOT NULL,

IncidentDate DATE NOT NULL,

DamageType NVARCHAR(50) NOT NULL,

Description NVARCHAR(1000) NOT NULL,

ClaimedAmount DECIMAL(10,2) NOT NULL,

ApprovedAmount DECIMAL(10,2) NULL,

Status NVARCHAR(20) NOT NULL,  -- Submitted, UnderReview, Approved, Rejected, Paid

SubmittedAt DATETIME2 DEFAULT GETUTCDATE(),

ReviewedBy INT NULL,  -- UserId underwriter

ReviewedAt DATETIME2 NULL,

RejectionReason NVARCHAR(500) NULL,

CONSTRAINT FK_Claims_Policies FOREIGN KEY (PolicyId) REFERENCES Policies(Id)

);

CREATE TABLE ClaimDocuments (

Id INT PRIMARY KEY IDENTITY,

ClaimId INT NOT NULL,

FileName NVARCHAR(255) NOT NULL,

FilePath NVARCHAR(500) NOT NULL,

FileSize INT NOT NULL,  -- bytes

MimeType NVARCHAR(100) NOT NULL,

UploadedAt DATETIME2 DEFAULT GETUTCDATE(),

CONSTRAINT FK_ClaimDocuments_Claims FOREIGN KEY (ClaimId) REFERENCES Claims(Id)

);

```

## 8. API Design

### Endpoints

**POST /api/claims**
- **Request**:
```

{

"policyId": 123,

"incidentDate": "2026-01-15",

"damageType": "Auto",

"description": "Incidente in autostrada",

"claimedAmount": 5000.00

}

```
- **Response 201**:
```

{

"id": 456,

"claimNumber": "CLM-2026-000456",

"status": "Submitted",

"submittedAt": "2026-02-07T10:30:00Z"

}

```
- **Response 400**: Validazione fallita
- **Response 404**: Polizza non trovata

**POST /api/claims/{id}/documents**
- **Request**: multipart/form-data
- **Response 201**: Documento caricato

**GET /api/claims/{id}**
- **Response**: Dettagli sinistro completi

**PUT /api/claims/{id}/status**
- **Request**: `{ "status": "Approved", "approvedAmount": 4500.00 }`
- **Autenticazione**: Solo underwriter

## 9. Validazioni

| Campo | Validazione |
|-------|-------------|
| policyId | Required, must exist, must be Active |
| incidentDate | Required, ≤ today, ≥ policy.StartDate, ≤ policy.ExpirationDate |
| damageType | Required, enum |
| description | Required, 10-1000 chars |
| claimedAmount | Required, > 0, ≤ 999999.99 |
| documents | Min 2, Max 5, Total ≤ 10MB, formats: PDF/JPG/PNG |

## 10. Notifiche

**Email a Cliente (template):**
```

Oggetto: Sinistro #{claimNumber} ricevuto

Gentile {customerName},

Il tuo sinistro #{claimNumber} è stato ricevuto correttamente.

Dettagli:

- Polizza: {policyNumber}
- Data incidente: {incidentDate}
- Importo richiesto: €{claimedAmount}

Il nostro team lo valuterà entro 3 giorni lavorativi.

Puoi monitorare lo stato su: [https://app.insurance.com/claims/{id}](https://app.insurance.com/claims/{id})

Grazie,

Team Assicurazioni

```

## 11. Stima

### Breakdown Tasks

| Task | Story Points |
|------|-------------|
| Backend: ClaimController + Service | 5 SP |
| Backend: Document upload logic | 3 SP |
| Backend: Email notifications | 2 SP |
| Frontend: Form sinistro | 5 SP |
| Frontend: Upload documenti | 3 SP |
| Database: Migrations | 1 SP |
| Unit tests (80% coverage) | 3 SP |
| Integration tests | 2 SP |
| **TOTALE** | **24 SP** |

**Effort stimato**: ~9-10 giorni (velocity team 25 SP/sprint)

## 12. Rischi

- ⚠️ **Storage documenti**: Serve S3/Azure Blob? O file system?
- ⚠️ **Email service**: SMTP configurato? Template pronti?
- ⚠️ **Approvazione manager**: Workflow complesso se > €10.000
- ⚠️ **Performance**: Se molti upload simultanei, serve queue?

## 13. Dipendenze

- EmailService (già implementato)
- FileStorageService (da implementare se non esiste)
- Policy API (già disponibile)
```

---

### Tecniche di Analisi

#### **Use Case Diagram** (UML)

```
        ┌──────────────────────┐
        │  Sistema Sinistri    │
        ├──────────────────────┤
        │                      │
 Cliente ───▶ Apri Sinistro      │
        │       │              │
        │       └───includes───▶ Carica Documenti
        │                      │
Underwriter─▶ Valuta Sinistro   │
        │       │              │
        │       └───extends───▶ Richiedi Info Aggiuntive
        │                      │
 Sistema ───▶ Invia Notifiche   │
        │                      │
        └──────────────────────┘
```

#### **Entity Relationship Diagram**

```
Customer ───< ha molte ─── Policy ───< ha molti ─── Claim
                                            |
                                            | ha molti
                                            ↓
                                      ClaimDocument
```

---

## 2. Documentazione Tecnica

### Tipi di Documentazione

#### **1. [README.md](http://README.md) - Setup Progetto**

```markdown
# Insurance Management System

## Descrizione
Sistema di gestione polizze e sinistri assicurativi.

## Prerequisiti
- .NET 8.0 SDK
- SQL Server 2019+
- Node.js 18+ (per frontend)
- Redis (opzionale, per caching)

## Setup Locale

### Backend

1. Clone repository:
```

git clone [https://github.com/kirey/insurance-api.git](https://github.com/kirey/insurance-api.git)

cd insurance-api

```

2. Configura `appsettings.Development.json`:
```

{

"ConnectionStrings": {

"Default": "Server=[localhost](http://localhost);Database=InsuranceDB;Trusted_Connection=true;"

},

"Jwt": {

"Secret": "your-secret-key-min-32-chars"

}

}

```

3. Applica migrations:
```

dotnet ef database update

```

4. Seed dati iniziali:
```

dotnet run --seed

```

5. Run:
```

dotnet run

```

6. Swagger UI: https://localhost:5001/swagger

### Test

```

# Unit tests

dotnet test --filter Category=Unit

# Integration tests

dotnet test --filter Category=Integration

# Code coverage

dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

```

## Struttura Progetto

```

InsuranceAPI/

├── Controllers/           # REST API endpoints

├── Services/              # Business logic

│   ├── Policies/

│   ├── Claims/

│   └── Premium/

├── Repositories/          # Data access layer

├── Models/                # Domain entities

├── DTOs/                  # Data transfer objects

├── Validators/            # FluentValidation rules

├── Middleware/            # Custom middleware

├── Infrastructure/        # External integrations

└── Tests/

├── Unit/

└── Integration/

```

## Environment Variables

| Variabile | Descrizione | Default |
|-----------|-------------|---------|
| `ASPNETCORE_ENVIRONMENT` | Environment (Development/Staging/Production) | Development |
| `DB_CONNECTION_STRING` | SQL Server connection | localhost |
| `REDIS_CONNECTION` | Redis cache | localhost:6379 |
| `SMTP_HOST` | Email server | smtp.gmail.com |

## Deploy

### Docker

```

docker build -t insurance-api .

docker run -p 5000:80 insurance-api

```

### Azure App Service

```

az webapp deploy --resource-group rg-insurance --name insurance-api

```

```

---

#### **2. API Documentation (Swagger)**

**XML Comments in Code:**

```csharp
/// <summary>
/// Crea un nuovo sinistro per una polizza attiva
/// </summary>
/// <param name="dto">Dati del sinistro da creare</param>
/// <returns>Sinistro creato con numero univoco</returns>
/// <response code="201">Sinistro creato con successo</response>
/// <response code="400">Dati di input non validi</response>
/// <response code="404">Polizza non trovata o non attiva</response>
/// <response code="409">Limite sinistri annuali raggiunto</response>
[HttpPost]
[ProducesResponseType(typeof(ClaimDto), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public async Task<ActionResult<ClaimDto>> CreateClaim(
    [FromBody] CreateClaimDto dto)
{
    // Valida policy attiva
    var policy = await _policyService.GetByIdAsync(dto.PolicyId);
    if (policy == null || policy.Status != PolicyStatus.Active)
        return NotFound("Polizza non trovata o non attiva");

    // Crea sinistro
    var claim = await _claimService.CreateAsync(dto);
    
    return CreatedAtAction(nameof(GetClaim), 
        new { id = claim.Id }, claim);
}
```

**Enable in Program.cs:**

```csharp
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Insurance API", 
        Version = "v1",
        Description = "API per gestione polizze e sinistri"
    });
    
    // Include XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
```

---

#### **3. Code Comments - Quando e Come**

**✅ BUONI commenti (spiegano PERCHÉ):**

```csharp
// Applichiamo franchigia solo al primo sinistro dell'anno per categoria.
// Regola business definita in documento BR-2024-Insurance v3.2
if (IsFirstClaimOfYearForCategory(policy, claim.DamageType))
    approvedAmount -= policy.Deductible;

// Cache 30 minuti perché premi ricalcolati solo durante batch notturno
await _cache.SetAsync(cacheKey, premium, TimeSpan.FromMinutes(30));

// Workaround temporaneo per race condition con legacy DB
// TODO: Rimuovere dopo migrazione completa a nuovo schema (JIRA-789)
await Task.Delay(50);
```

**❌ CATTIVI commenti (ovvi, inutili):**

```csharp
// Incrementa counter
counter++;

// Loop attraverso le policies
foreach (var policy in policies) { }

// Set status to active
policy.Status = PolicyStatus.Active;
```

**Regola d'oro**: Se il codice è chiaro, non serve commento. Commenta solo logica business complessa o workaround temporanei.

---

#### **4. Database Schema Documentation**

```sql
-- =====================================================
-- Table: Policies
-- Descrizione: Polizze assicurative (auto, casa, salute)
-- Owner: Insurance Team
-- Ultimo aggiornamento: 2026-02-07
-- =====================================================

CREATE TABLE Policies (
    Id INT PRIMARY KEY IDENTITY,
    
    -- Business Key (formato: POL-YYYY-######)
    PolicyNumber NVARCHAR(50) UNIQUE NOT NULL,
    
    -- Relazioni
    CustomerId INT NOT NULL,
    CONSTRAINT FK_Policies_Customers FOREIGN KEY (CustomerId) 
        REFERENCES Customers(Id),
    
    -- Dati polizza
    Type NVARCHAR(20) NOT NULL,  -- Auto, Health, Home, Life
    Premium DECIMAL(10,2) NOT NULL,  -- Importo mensile in EUR
    CoverageAmount DECIMAL(12,2) NOT NULL,  -- Massimale copertura
    Deductible DECIMAL(10,2) DEFAULT 0,  -- Franchigia
    
    -- Periodo validità
    StartDate DATE NOT NULL,
    ExpirationDate DATE NOT NULL,
    
    -- Status: Active, Expired, Cancelled, Suspended
    Status NVARCHAR(20) NOT NULL DEFAULT 'Active',
    
    -- Audit
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    CreatedBy INT NOT NULL,
    UpdatedAt DATETIME2,
    UpdatedBy INT,
    
    -- Soft delete
    IsDeleted BIT DEFAULT 0,
    DeletedAt DATETIME2,
    
    -- Check constraints
    CONSTRAINT CK_Policies_Dates CHECK (ExpirationDate > StartDate),
    CONSTRAINT CK_Policies_Premium CHECK (Premium > 0),
    CONSTRAINT CK_Policies_Coverage CHECK (CoverageAmount > 0)
);

-- Indici per performance
CREATE INDEX IX_Policies_CustomerId ON Policies(CustomerId) 
    WHERE IsDeleted = 0;

CREATE INDEX IX_Policies_Status_Expiration ON Policies(Status, ExpirationDate)
    WHERE IsDeleted = 0 AND Status = 'Active';

CREATE INDEX IX_Policies_Type ON Policies(Type)
    WHERE IsDeleted = 0;
```

---

## Quick Q&A - Analisi e Documentazione

**Q51**: Come fai analisi funzionale di un requisito?

**A (30 sec)**:

"Parto dal requisito business, faccio domande al PO per chiarire ambiguità: chi, cosa, quando, perché. Identifico attori (chi usa feature), precondizioni, flusso principale e flussi alternativi (cosa se errore?). Definisco regole business, validazioni, modello dati (entità e relazioni), API design. Stimo effort spezzando in task. Output: documento analisi con tutto questo + rischi e dipendenze. Esempio: per 'Apertura sinistro' definisco flusso cliente, validazioni (data/importo), entità Claim, endpoint POST /claims."

**Q52**: Che documentazione tecnica produci?

**A (30 sec)**:

"Produco 4 tipi: 1) **README** per setup progetto (prerequisiti, comandi run/test, struttura folder). 2) **API docs** con Swagger (XML comments su controller). 3) **Code comments** solo per business logic complessa o workaround (non codice ovvio). 4) **Schema DB** commentato (descrizione tabelle, constraints, indici). Uso Markdown per docs in repo. Filosofia: codice leggibile è miglior documentazione, documento solo PERCHÉ non COSA."

---

# CHECKLIST FINALE - PREPARAZIONE KIREY GROUP 🎯 {#checklist-kirey}

## ✅ Competenze Tecniche - COVERAGE COMPLETO

### Core .NET (OBBLIGATORIO)

- ✅ **C#** - OOP, types, async/await, LINQ, collections
- ✅ [**ASP.NET](http://ASP.NET) Core** - Pipeline, DI, REST API, middleware, JWT
- ✅ **Entity Framework Core** - DbContext, migrations, LINQ queries
- ✅ **SQL Server** - Queries, JOINs, indexes, transactions

### Scheduling e Legacy (RICHIESTO)

- ✅ [**Quartz.NET**](http://Quartz.NET) - Job, Trigger, Cron expressions, scheduling
- ✅ **WCF** - SOAP basics, differenze con REST, perché obsoleto

### Insurance Domain (PLUS)

- ✅ **Terminologia** - Policy, Premium, Claim, Underwriting, Deductible
- ✅ **Processi** - Calcolo premio, gestione sinistri, valutazione rischio
- ✅ **Esempi codice** - PremiumCalculator, ClaimService, Policy validation

### Version Control (PLUS → ESSENZIALE)

- ✅ **GIT** - clone, branch, commit, merge, push/pull
- ✅ **GitFlow** - feature/bugfix/hotfix branches
- ✅ **Pull Request** - Code review process
- ✅ **Conflict resolution** - Come risolvere merge conflicts

### Metodologia Agile (RICHIESTO)

- ✅ **Scrum** - Ruoli, artefatti, cerimonie
- ✅ **Sprint** - Planning, Daily Standup, Review, Retrospective
- ✅ **User Stories** - Formato, Acceptance Criteria, Story Points
- ✅ **Velocity** - Tracking e planning

### Analisi e Documentazione (RICHIESTO)

- ✅ **Analisi funzionale** - Raccolta requisiti, modellazione, stima
- ✅ **Documentazione** - README, Swagger, code comments, DB schema

---

## 📋 Piano di Studio Finale - 7 Giorni

### **Giorno -7 e -6**: Tecnologie Specifiche

- [ ]  Studia [**Quartz.NET**](http://Quartz.NET): Job/Trigger/Scheduler, Cron syntax
- [ ]  Ripassa **WCF**: SOAP, ABC model, perché obsoleto vs REST
- [ ]  Memorizza **Insurance terminology** (italiano-inglese)
- [ ]  Pratica: Scrivi Job [Quartz.NET](http://Quartz.NET) che calcola premi ogni giorno alle 3 AM

### **Giorno -5**: GIT e Version Control

- [ ]  Pratica comandi GIT: clone, branch, commit, merge, push/pull
- [ ]  Simula **merge conflict** e risolvilo
- [ ]  Scrivi commit messages con Conventional Commits
- [ ]  Pratica: Crea feature branch, fai modifiche, apri mock Pull Request

### **Giorno -4**: Agile/Scrum

- [ ]  Studia **ruoli Scrum**: PO, SM, Dev Team
- [ ]  Memorizza **cerimonie**: Planning, Standup, Review, Retro
- [ ]  Pratica: Scrivi 3 **User Stories** in formato corretto
- [ ]  Simula **Daily Standup**: prepara cosa diresti (3 domande)

### **Giorno -3**: Analisi Funzionale

- [ ]  Leggi esempio analisi funzionale "Gestione Sinistri"
- [ ]  Pratica: Fa analisi di un requisito semplice (es. "Reset password")
- [ ]  Identifica: attori, flussi, validazioni, API, stima
- [ ]  Disegna **Entity Relationship Diagram** per il tuo requisito

### **Giorno -2**: Review Fondamenti

- [ ]  Ripassa tutte le **52 Quick Q&A** (Q1-Q52)
- [ ]  Focus su: Async/Await, LINQ, DI, EF Core, Collections
- [ ]  Pratica orale: Rispondi a voce alta in 30-60 secondi

### **Giorno -1**: Mock Interview

- [ ]  **Mock colloquio** con amico/familiare
- [ ]  Spiega: GitFlow, Scrum Sprint, User Story, analisi funzionale
- [ ]  Prepara **domande per loro** (3-5 domande)
- [ ]  Rivedi progetti personali: prepara 2-3 esempi concreti
- [ ]  **Riposo**: vai a letto presto!

### **Giorno 0**: COLLOQUIO 🚀

- [ ]  Rilassati - SEI PREPARATO!
- [ ]  Porta CV aggiornato (PDF)
- [ ]  Esempi concreti pronti
- [ ]  Mindset: problem-solving, teamwork, voglia di imparare

---

## 💡 Domande da Fare a Kirey

### Tecnico

1. "Che versione di .NET usate? Siete su .NET 8 o ancora su .NET Framework?"
2. "Come gestite il deploy? Avete CI/CD con Azure DevOps?"
3. "[Quartz.NET](http://Quartz.NET) è usato per quali tipi di job? Batch notturni, report?"
4. "Avete ancora sistemi WCF da manutenere o è tutto REST?"

### Processo

1. "Come è strutturato il team? Quanti developer per team?"
2. "Seguite Scrum strict o Scrum adattato? Sprint di quanto?"
3. "Come funziona il code review? Tool usato? Tempistiche?"

### Progetto

1. "Su che progetti insurance specifici lavorerei inizialmente?"
2. "Quanto codice legacy vs nuovo sviluppo?"
3. "Quali assicurazioni sono i vostri clienti principali?"

### Crescita

1. "Che piano di formazione offrite? Certificazioni Azure/AWS?"
2. "Ci sono opportunità di crescita verso ruoli senior/lead?"
3. "Mentor/buddy system per onboarding?"

---

## ✅ SEI PRONTO QUANDO...

### Tecnico

- [ ]  Puoi spiegare **Job/Trigger/Scheduler** di [Quartz.NET](http://Quartz.NET)
- [ ]  Sai cos'è **WCF** e perché è obsoleto
- [ ]  Conosci terminologia **Insurance** (Policy, Premium, Claim, Underwriting)
- [ ]  Puoi spiegare **async/await**, **LINQ deferred execution**, **DI lifetimes**

### Git

- [ ]  Sai creare **branch**, fare **merge**, risolvere **conflicts**
- [ ]  Conosci **GitFlow** (feature/bugfix/hotfix)
- [ ]  Puoi descrivere processo **Pull Request** e code review

### Agile

- [ ]  Puoi descrivere uno **Sprint Scrum** completo
- [ ]  Sai cosa dire in un **Daily Standup**
- [ ]  Capisci differenza tra **User Story** e **Task**
- [ ]  Sai cos'è **Velocity** e come si usa

### Analisi

- [ ]  Puoi fare **analisi funzionale** di un requisito semplice
- [ ]  Identifichi: attori, flussi, validazioni, entità
- [ ]  Sai stimare con **Story Points**

### Q&A

- [ ]  Tutte le **52 Quick Q&A** in 30-60 secondi ciascuna

---

## 🎯 PUNTI CHIAVE PER KIREY

### Cosa Enfatizzare

1. **Teamwork e Collaborazione**
    - Kirey menziona molto "lavoro in team"
    - Dai esempi di pair programming, code review, aiuto colleghi
2. **Problem Solving**
    - "Capacità di problem solving" è requisito chiave
    - Prepara 2-3 esempi di bug complessi risolti
    - Spiega il processo: analisi, debug, soluzione, test
3. **Agile Mindset**
    - "Metodologia Agile" nell'offerta
    - Mostra familiarità con iterazioni, feedback, miglioramento continuo
4. **Interesse per Insurance Domain**
    - "Conoscenza mondo assicurativo" è PLUS
    - Mostra curiosità: "Ho studiato come funziona calcolo premium, underwriting..."
5. **Flessibilità e Adattabilità**
    - "Ambiente dinamico", "flessibilità nell'adattarsi"
    - Racconta esperienze dove hai imparato nuove tecnologie velocemente

### Red Flags da Evitare

- ❌ "Non ho mai lavorato in team" (cercano teamwork!)
- ❌ "Non conosco Agile" (metodologia richiesta)
- ❌ "Non so usare GIT" (version control essenziale)
- ❌ "Non mi interessa il dominio" (cercano interesse insurance)
- ❌ "Preferisco lavorare da solo" (collaborazione key)

---

## 🚀 ULTIMO MESSAGGIO

**HAI STUDIATO:**

- ✅ 52 domande tecniche con risposte 30-60 sec
- ✅ .NET, C#, [ASP.NET](http://ASP.NET) Core, EF Core, SQL Server
- ✅ [Quartz.NET](http://Quartz.NET), WCF, Insurance domain
- ✅ GIT, GitFlow, Pull Request
- ✅ Agile, Scrum, User Stories, Sprint
- ✅ Analisi funzionale, Documentazione
- ✅ String, StringBuilder, Collections

**SEI PREPARATO AL 100% PER:**

- Posizione: .NET Developer (2 anni esperienza)
- Azienda: Kirey Group - Digital Insurance
- Sede: Milano (hybrid/remote)

**MINDSET VINCENTE:**

- Problem solver
- Team player
- Voglia di imparare
- Interesse per insurance domain
- Agile mindset

---

# IN BOCCA AL LUPO! 🍀

**Ricorda**: Non devi sapere TUTTO al 100%. Kirey cerca:

- ✅ Solide basi .NET
- ✅ Voglia di crescere
- ✅ Capacità di lavorare in team
- ✅ Problem solving
- ✅ Flessibilità

Se non sai rispondere a qualcosa: **"Non l'ho usato ancora, ma sono molto interessato ad impararlo. Posso spiegarle come affronterei il problema..."**

**VAI E SPACCA IL COLLOQUIO! 🚀🚀🚀**

---

# PARTE 15: ARCHITETTURE ENTERPRISE AVANZATE {#enterprise-architectures}

## 1. CQRS - Command Query Responsibility Segregation {#cqrs}

### Cos'è CQRS?

**CQRS** = Separare **letture** (Query) da **scritture** (Command) usando modelli diversi.

**Problema tradizionale:**

```csharp
// Stesso modello per READ e WRITE
public class Policy
{
    public int Id { get; set; }
    public string Number { get; set; }
    public decimal Premium { get; set; }
    // ... 50 proprietà per coprire tutti i casi d'uso
}

// Read
var policy = await _db.Policies.FindAsync(id); // Carica TUTTO

// Write
policy.Premium = newPremium;
await _db.SaveChangesAsync();
```

**Problemi:**

- Modello troppo complesso (serve tutto per write, poco per read)
- Performance: Read carica dati inutili
- Scalabilità: Read e Write competono per stesso DB

---

### CQRS - Architettura

```jsx
                  APPLICATION
                       |
      +----------------+----------------+
      |                                 |
  COMMAND SIDE                      QUERY SIDE
  (Write Model)                     (Read Model)
      |                                 |
 Domain Logic                   Simplified DTOs
 Validazioni                    Denormalized
 Business Rules                 Optimized for Read
      |                                 |
  Write DB                          Read DB
(Normalized)                    (Denormalized)
      |                                 |
      +----------> SYNC <--------------+
              (Event/Message)
```

---

### CQRS - Implementazione Base

#### **Command Side (Write)**

```csharp
// Command - Azione che modifica stato
public record CreatePolicyCommand(
    string CustomerEmail,
    string Type,
    decimal Premium,
    decimal CoverageAmount
);

// Command Handler - Esegue business logic
public class CreatePolicyCommandHandler
{
    private readonly ApplicationDbContext _db;
    private readonly IEventBus _eventBus;

    public async Task<int> Handle(CreatePolicyCommand command)
    {
        // 1. Validazione business rules
        if (command.Premium <= 0)
            throw new ValidationException("Premium must be > 0");

        // 2. Crea entità domain
        var policy = new Policy
        {
            Number = GeneratePolicyNumber(),
            CustomerEmail = command.CustomerEmail,
            Type = command.Type,
            Premium = command.Premium,
            CoverageAmount = command.CoverageAmount,
            Status = PolicyStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        // 3. Salva in Write DB
        _db.Policies.Add(policy);
        await _db.SaveChangesAsync();

        // 4. Pubblica evento per sincronizzare Read Model
        await _eventBus.PublishAsync(new PolicyCreatedEvent
        {
            PolicyId = policy.Id,
            Number = policy.Number,
            CustomerEmail = policy.CustomerEmail,
            Premium = policy.Premium
        });

        return policy.Id;
    }
}
```

#### **Query Side (Read)**

```csharp
// Query - Richiesta dati
public record GetPolicyByIdQuery(int PolicyId);

// Read Model - Ottimizzato per UI
public class PolicyReadModel
{
    public int Id { get; set; }
    public string Number { get; set; }
    public string CustomerName { get; set; }  // Denormalized!
    public decimal Premium { get; set; }
    public string Status { get; set; }
    // Solo campi necessari per questa view
}

// Query Handler - Legge da Read DB
public class GetPolicyByIdQueryHandler
{
    private readonly IReadDbContext _readDb;

    public async Task<PolicyReadModel> Handle(GetPolicyByIdQuery query)
    {
        // Read DB già denormalizzato e ottimizzato
        return await _readDb.Policies
            .Where(p => p.Id == query.PolicyId)
            .Select(p => new PolicyReadModel
            {
                Id = p.Id,
                Number = p.Number,
                CustomerName = p.CustomerName, // Già joinato!
                Premium = p.Premium,
                Status = p.Status
            })
            .FirstOrDefaultAsync();
    }
}
```

---

### CQRS - Sincronizzazione Read Model

```csharp
// Event Handler - Aggiorna Read Model quando Write cambia
public class PolicyCreatedEventHandler
{
    private readonly IReadDbContext _readDb;

    public async Task Handle(PolicyCreatedEvent @event)
    {
        // Proietta evento su Read Model
        var readModel = new PolicyReadModel
        {
            Id = @event.PolicyId,
            Number = @event.Number,
            CustomerName = await GetCustomerName(@event.CustomerEmail),
            Premium = @event.Premium,
            Status = "Active"
        };

        await _readDb.Policies.AddAsync(readModel);
        await _readDb.SaveChangesAsync();
    }
}
```

---

### CQRS - Vantaggi e Quando Usare

**✅ Vantaggi:**

- **Performance**: Read ottimizzato (denormalized), Write focalizzato
- **Scalabilità**: Scaling indipendente (10x read, 1x write)
- **Flessibilità**: Modelli diversi per casi d'uso diversi
- **Query complesse**: Read DB può avere viste pre-calcolate

**❌ Svantaggi:**

- **Complessità**: Due modelli da mantenere
- **Eventual consistency**: Read Model non immediatamente aggiornato
- **Overhead**: Serve infrastruttura messaging

**Quando usare:**

- ✅ Sistemi con molte più read che write (90% read, 10% write)
- ✅ Query complesse vs operazioni write semplici
- ✅ Necessità di scalare read e write indipendentemente
- ✅ Domini complessi con molte business rules (Banking, Insurance)

**Quando NON usare:**

- ❌ CRUD semplice (overkill)
- ❌ Strong consistency richiesta (read deve essere sempre aggiornato)
- ❌ Team piccolo senza esperienza

---

## 2. Event Sourcing - Storia Immutabile {#event-sourcing}

### Cos'è Event Sourcing?

**Event Sourcing** = Salvare **tutti gli eventi** che cambiano lo stato, non solo lo stato finale.

**Tradizionale (State-based):**

```csharp
// Salviamo solo stato corrente
Policies table:
Id | Number | Premium | Status
1  | POL-001| 500     | Active

// Modifichiamo
policy.Premium = 600;
await _db.SaveChangesAsync();

// PERSO: Chi ha cambiato? Quando? Perché? Valore precedente?
```

**Event Sourcing:**

```csharp
// Salviamo TUTTI gli eventi
Events table:
Id | AggregateId | EventType           | Data                      | Timestamp
1  | POL-001     | PolicyCreated       | {Premium: 500}            | 2026-01-01
2  | POL-001     | PremiumChanged      | {Old: 500, New: 600}      | 2026-02-01
3  | POL-001     | PolicySuspended     | {Reason: "Non-payment"}   | 2026-03-01

// Stato corrente = REPLAY di tutti gli eventi
```

---

### Event Sourcing - Implementazione

#### **Definire Eventi**

```csharp
// Base event
public abstract record DomainEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
}

// Eventi specifici
public record PolicyCreatedEvent : DomainEvent
{
    public string PolicyNumber { get; init; }
    public string CustomerEmail { get; init; }
    public decimal Premium { get; init; }
    public decimal CoverageAmount { get; init; }
}

public record PremiumChangedEvent : DomainEvent
{
    public string PolicyNumber { get; init; }
    public decimal OldPremium { get; init; }
    public decimal NewPremium { get; init; }
    public string ChangedBy { get; init; }
    public string Reason { get; init; }
}

public record PolicySuspendedEvent : DomainEvent
{
    public string PolicyNumber { get; init; }
    public string Reason { get; init; }
}
```

---

#### **Aggregate - Ricostruisce Stato da Eventi**

```csharp
public class Policy
{
    // Stato corrente
    public string Number { get; private set; }
    public string CustomerEmail { get; private set; }
    public decimal Premium { get; private set; }
    public decimal CoverageAmount { get; private set; }
    public PolicyStatus Status { get; private set; }

    // Eventi non ancora salvati
    private readonly List<DomainEvent> _uncommittedEvents = new();
    public IReadOnlyList<DomainEvent> UncommittedEvents => _uncommittedEvents;

    // Costruttore per ricostruire da eventi
    public static Policy FromEvents(IEnumerable<DomainEvent> events)
    {
        var policy = new Policy();
        foreach (var @event in events)
            policy.Apply(@event);
        return policy;
    }

    // Comando: Cambia Premium
    public void ChangePremium(decimal newPremium, string changedBy, string reason)
    {
        // Business rules
        if (newPremium <= 0)
            throw new DomainException("Premium must be > 0");
        if (Status != PolicyStatus.Active)
            throw new DomainException("Cannot change premium of inactive policy");

        // Crea evento
        var @event = new PremiumChangedEvent
        {
            PolicyNumber = Number,
            OldPremium = Premium,
            NewPremium = newPremium,
            ChangedBy = changedBy,
            Reason = reason
        };

        // Applica evento (aggiorna stato)
        Apply(@event);
        _uncommittedEvents.Add(@event);
    }

    // Applica evento allo stato
    private void Apply(DomainEvent @event)
    {
        switch (@event)
        {
            case PolicyCreatedEvent e:
                Number = e.PolicyNumber;
                CustomerEmail = e.CustomerEmail;
                Premium = e.Premium;
                CoverageAmount = e.CoverageAmount;
                Status = PolicyStatus.Active;
                break;

            case PremiumChangedEvent e:
                Premium = e.NewPremium;
                break;

            case PolicySuspendedEvent e:
                Status = PolicyStatus.Suspended;
                break;
        }
    }
}
```

---

#### **Event Store - Persistenza**

```csharp
public interface IEventStore
{
    Task SaveEventsAsync(string aggregateId, IEnumerable<DomainEvent> events);
    Task<IEnumerable<DomainEvent>> GetEventsAsync(string aggregateId);
}

public class SqlEventStore : IEventStore
{
    private readonly DbContext _db;

    public async Task SaveEventsAsync(string aggregateId, IEnumerable<DomainEvent> events)
    {
        foreach (var @event in events)
        {
            var eventRecord = new EventRecord
            {
                AggregateId = aggregateId,
                EventType = @event.GetType().Name,
                EventData = JsonSerializer.Serialize(@event),
                OccurredAt = @event.OccurredAt
            };

            _db.Events.Add(eventRecord);
        }

        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<DomainEvent>> GetEventsAsync(string aggregateId)
    {
        var records = await _db.Events
            .Where(e => e.AggregateId == aggregateId)
            .OrderBy(e => e.OccurredAt)
            .ToListAsync();

        return records.Select(r => DeserializeEvent(r.EventType, r.EventData));
    }
}
```

---

#### **Repository con Event Sourcing**

```csharp
public class PolicyRepository
{
    private readonly IEventStore _eventStore;

    // Carica aggregate ricostruendo da eventi
    public async Task<Policy> GetByNumberAsync(string policyNumber)
    {
        var events = await _eventStore.GetEventsAsync(policyNumber);
        
        if (!events.Any())
            return null;

        return Policy.FromEvents(events);
    }

    // Salva solo i NUOVI eventi
    public async Task SaveAsync(Policy policy)
    {
        if (policy.UncommittedEvents.Any())
        {
            await _eventStore.SaveEventsAsync(
                policy.Number, 
                policy.UncommittedEvents
            );
        }
    }
}
```

---

### Event Sourcing - Vantaggi

**✅ Vantaggi:**

1. **Audit Trail Completo**
    - Chi, cosa, quando, perché per ogni cambiamento
    - Compliance (GDPR, SOX, regulatory)
    - Debug: "Cosa è successo il 15 gennaio?"
2. **Time Travel**
    - Ricostruisci stato a qualsiasi momento passato
    - "Mostra tutte le polizze attive al 31/12/2025"
3. **Analisi e BI**
    - Eventi = data warehouse naturale
    - Query analytics: "Quanti premium changes negli ultimi 6 mesi?"
4. **Undo/Replay**
    - Compensation events per annullare azioni
    - Replay eventi per testare nuova business logic

**❌ Svantaggi:**

- **Complessità**: Curva apprendimento alta
- **Performance**: Replay eventi può essere lento se molti eventi
- **Snapshot**: Serve snapshot periodici per aggregate con molti eventi
- **Schema evolution**: Gestire eventi vecchi quando cambia struttura

---

### CQRS + Event Sourcing - Il Matrimonio Perfetto

```jsx
        USER REQUEST
             |
     +-------+-------+
     |               |
 COMMAND         QUERY
     |               |
Command Handler   Query Handler
     |               |
 AGGREGATE           |
(Domain Logic)       |
     |               |
EVENTS STORE    READ MODEL DB
     |         (denormalized)
     |
 Event Handler
 (projections)
     |
     +-------> Aggiorna Read Model
```

**Workflow:**

1. Command → Genera eventi → Salva in Event Store
2. Eventi → Event Handler → Aggiorna Read Model (denormalized)
3. Query → Legge da Read Model (veloce!)

---

## 3. Mediator Pattern - Disaccoppiamento {#mediator-pattern}

### Cos'è Mediator?

**Mediator** = Centralizza comunicazione tra componenti, evitando dipendenze dirette.

**Problema senza Mediator:**

```csharp
public class PolicyController
{
    private readonly IPolicyService _policyService;
    private readonly IEmailService _emailService;
    private readonly INotificationService _notificationService;
    private readonly IAuditService _auditService;
    private readonly ICacheService _cacheService;
    // ... 10 dipendenze!

    public async Task<IActionResult> CreatePolicy(CreatePolicyDto dto)
    {
        var policy = await _policyService.CreateAsync(dto);
        await _emailService.SendAsync(...);
        await _notificationService.NotifyAsync(...);
        await _auditService.LogAsync(...);
        await _cacheService.InvalidateAsync(...);
        // Controller conosce TUTTI i servizi!
        return Ok(policy);
    }
}
```

---

### MediatR - Libreria Popular

**Install:**

```bash
dotnet add package MediatR
dotnet add package MediatR.Extensions.Microsoft.DependencyInjection
```

**Configurazione:**

```csharp
// Program.cs
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
```

---

### Mediator - Request/Response

```csharp
// Request (Command o Query)
public record CreatePolicyCommand : IRequest<PolicyDto>
{
    public string CustomerEmail { get; init; }
    public string Type { get; init; }
    public decimal Premium { get; init; }
}

// Handler
public class CreatePolicyCommandHandler 
    : IRequestHandler<CreatePolicyCommand, PolicyDto>
{
    private readonly ApplicationDbContext _db;
    private readonly IEmailService _emailService;

    public async Task<PolicyDto> Handle(
        CreatePolicyCommand request, 
        CancellationToken ct)
    {
        // 1. Business logic
        var policy = new Policy
        {
            CustomerEmail = request.CustomerEmail,
            Type = request.Type,
            Premium = request.Premium
        };

        _db.Policies.Add(policy);
        await _db.SaveChangesAsync(ct);

        // 2. Side effects
        await _emailService.SendWelcomeEmailAsync(policy.CustomerEmail);

        return new PolicyDto
        {
            Id = policy.Id,
            Number = policy.Number,
            Premium = policy.Premium
        };
    }
}
```

---

### Mediator - Nel Controller

```csharp
public class PolicyController : ControllerBase
{
    private readonly IMediator _mediator;  // UNA sola dipendenza!

    public PolicyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<PolicyDto>> CreatePolicy(
        CreatePolicyCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetPolicy), 
            new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PolicyDto>> GetPolicy(int id)
    {
        var query = new GetPolicyByIdQuery { PolicyId = id };
        var result = await _mediator.Send(query);
        
        if (result == null)
            return NotFound();
        
        return Ok(result);
    }
}
```

---

### Mediator - Pipeline Behaviors

**Behaviors** = Middleware per ogni request (logging, validation, caching).

```csharp
// Logging Behavior
public class LoggingBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("Handling {Request}", requestName);
        
        var sw = Stopwatch.StartNew();
        var response = await next();  // Esegue handler
        sw.Stop();

        _logger.LogInformation(
            "Handled {Request} in {ElapsedMs}ms",
            requestName, sw.ElapsedMilliseconds);

        return response;
    }
}

// Validation Behavior
public class ValidationBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var results = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, ct)));

            var failures = results
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
                throw new ValidationException(failures);
        }

        return await next();
    }
}

// Registrazione
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), 
    typeof(LoggingBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), 
    typeof(ValidationBehavior<,>));
```

---

### Mediator - Notifications (Pub/Sub)

```csharp
// Notification (evento interno)
public record PolicyCreatedNotification : INotification
{
    public int PolicyId { get; init; }
    public string CustomerEmail { get; init; }
}

// Handler 1: Invia email
public class SendWelcomeEmailHandler 
    : INotificationHandler<PolicyCreatedNotification>
{
    private readonly IEmailService _emailService;

    public async Task Handle(
        PolicyCreatedNotification notification, 
        CancellationToken ct)
    {
        await _emailService.SendWelcomeEmailAsync(
            notification.CustomerEmail);
    }
}

// Handler 2: Log audit
public class AuditPolicyCreationHandler 
    : INotificationHandler<PolicyCreatedNotification>
{
    private readonly IAuditService _auditService;

    public async Task Handle(
        PolicyCreatedNotification notification, 
        CancellationToken ct)
    {
        await _auditService.LogAsync(
            $"Policy {notification.PolicyId} created");
    }
}

// Pubblicazione
await _mediator.Publish(new PolicyCreatedNotification
{
    PolicyId = policy.Id,
    CustomerEmail = policy.CustomerEmail
});
// Entrambi gli handler vengono eseguiti!
```

---

## 4. Repository Pattern - Astrazione Data Access {#repository-pattern}

### Cos'è Repository?

**Repository** = Astrae l'accesso ai dati, nasconde dettagli persistenza.

**Vantaggi:**

- ✅ Testabilità (mock easy)
- ✅ Cambio DB senza toccare business logic
- ✅ Centralizza query complesse
- ✅ Domain-focused API

---

### Repository - Generic Implementation

```csharp
// Interface generica
public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}

// Implementazione EF Core
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public virtual async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
    }

    public virtual async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
    }
}
```

---

### Repository - Specific per Entità

```csharp
// Interface specifica
public interface IPolicyRepository : IRepository<Policy>
{
    Task<Policy> GetByNumberAsync(string policyNumber);
    Task<IEnumerable<Policy>> GetActivePoliciesAsync();
    Task<IEnumerable<Policy>> GetExpiringPoliciesAsync(int days);
    Task<decimal> GetTotalPremiumsAsync();
}

// Implementazione
public class PolicyRepository : Repository<Policy>, IPolicyRepository
{
    public PolicyRepository(ApplicationDbContext context) 
        : base(context) { }

    public async Task<Policy> GetByNumberAsync(string policyNumber)
    {
        return await _dbSet
            .Include(p => p.Customer)
            .Include(p => p.Claims)
            .FirstOrDefaultAsync(p => p.Number == policyNumber);
    }

    public async Task<IEnumerable<Policy>> GetActivePoliciesAsync()
    {
        return await _dbSet
            .Where(p => p.Status == PolicyStatus.Active)
            .Where(p => p.ExpirationDate > DateTime.UtcNow)
            .ToListAsync();
    }

    public async Task<IEnumerable<Policy>> GetExpiringPoliciesAsync(int days)
    {
        var expirationDate = DateTime.UtcNow.AddDays(days);
        
        return await _dbSet
            .Where(p => p.Status == PolicyStatus.Active)
            .Where(p => p.ExpirationDate <= expirationDate)
            .Where(p => p.ExpirationDate > DateTime.UtcNow)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalPremiumsAsync()
    {
        return await _dbSet
            .Where(p => p.Status == PolicyStatus.Active)
            .SumAsync(p => p.Premium);
    }
}
```

---

### Unit of Work Pattern

**Unit of Work** = Coordina transazioni tra repository multipli.

```csharp
public interface IUnitOfWork : IDisposable
{
    IPolicyRepository Policies { get; }
    IClaimRepository Claims { get; }
    ICustomerRepository Customers { get; }
    
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction _transaction;

    public IPolicyRepository Policies { get; }
    public IClaimRepository Claims { get; }
    public ICustomerRepository Customers { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Policies = new PolicyRepository(context);
        Claims = new ClaimRepository(context);
        Customers = new CustomerRepository(context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _transaction.CommitAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _transaction.RollbackAsync();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}

// Uso
public class ClaimService
{
    private readonly IUnitOfWork _unitOfWork;

    public async Task<Claim> ProcessClaimAsync(int claimId)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var claim = await _unitOfWork.Claims.GetByIdAsync(claimId);
            var policy = await _unitOfWork.Policies.GetByIdAsync(claim.PolicyId);

            // Business logic
            claim.Status = ClaimStatus.Approved;
            policy.ClaimsCount++;

            await _unitOfWork.Claims.UpdateAsync(claim);
            await _unitOfWork.Policies.UpdateAsync(policy);
            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitTransactionAsync();
            return claim;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
```

---

## 5. Patterns Usati in Banking/Finance {#banking-patterns}

### 1. Saga Pattern - Transazioni Distribuite

**Problema**: Come gestire transazioni su microservizi multipli?

**Esempio Banking**: Trasferimento soldi tra conti

```csharp
// Saga Orchestrator
public class TransferMoneySaga
{
    private readonly IMediator _mediator;

    public async Task<TransferResult> ExecuteAsync(
        string fromAccount, 
        string toAccount, 
        decimal amount)
    {
        var sagaId = Guid.NewGuid();

        try
        {
            // Step 1: Verifica fondi disponibili
            var checkFunds = new CheckFundsCommand
            {
                AccountId = fromAccount,
                Amount = amount
            };
            await _mediator.Send(checkFunds);

            // Step 2: Blocca fondi (reservation)
            var reserve = new ReserveFundsCommand
            {
                SagaId = sagaId,
                AccountId = fromAccount,
                Amount = amount
            };
            await _mediator.Send(reserve);

            // Step 3: Accredita destinazione
            var credit = new CreditAccountCommand
            {
                SagaId = sagaId,
                AccountId = toAccount,
                Amount = amount
            };
            await _mediator.Send(credit);

            // Step 4: Conferma prelievo
            var debit = new DebitAccountCommand
            {
                SagaId = sagaId,
                AccountId = fromAccount,
                Amount = amount
            };
            await _mediator.Send(debit);

            return TransferResult.Success(sagaId);
        }
        catch (Exception ex)
        {
            // COMPENSAZIONE: Annulla step già eseguiti
            await CompensateAsync(sagaId, fromAccount, toAccount, amount);
            return TransferResult.Failure(ex.Message);
        }
    }

    private async Task CompensateAsync(
        Guid sagaId, 
        string fromAccount, 
        string toAccount, 
        decimal amount)
    {
        // Rollback in ordine inverso
        await _mediator.Send(new ReleaseReservedFundsCommand
        {
            SagaId = sagaId,
            AccountId = fromAccount,
            Amount = amount
        });

        // Log per audit
        await _mediator.Publish(new SagaCompensatedEvent
        {
            SagaId = sagaId,
            Reason = "Transfer failed"
        });
    }
}
```

---

### 2. Idempotency - Operazioni Sicure

**Problema**: Request duplicata (retry, network) non deve creare duplicate transazioni.

```csharp
public class TransactionService
{
    private readonly IDistributedCache _cache;
    private readonly ApplicationDbContext _db;

    public async Task<Transaction> ProcessTransactionAsync(
        string idempotencyKey,  // Client-provided unique ID
        TransactionRequest request)
    {
        // 1. Check se già processata
        var cached = await _cache.GetStringAsync(idempotencyKey);
        if (cached != null)
        {
            // Già processata, ritorna risultato cached
            return JsonSerializer.Deserialize<Transaction>(cached);
        }

        // 2. Process transazione
        var transaction = new Transaction
        {
            IdempotencyKey = idempotencyKey,
            FromAccount = request.FromAccount,
            ToAccount = request.ToAccount,
            Amount = request.Amount,
            Status = TransactionStatus.Completed,
            ProcessedAt = DateTime.UtcNow
        };

        _db.Transactions.Add(transaction);
        await _db.SaveChangesAsync();

        // 3. Cache risultato (24h)
        await _cache.SetStringAsync(
            idempotencyKey,
            JsonSerializer.Serialize(transaction),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
            });

        return transaction;
    }
}

// Client usage
var idempotencyKey = Guid.NewGuid().ToString();
var result = await _transactionService.ProcessTransactionAsync(
    idempotencyKey, request);

// Se retry, stesso idempotencyKey → stesso risultato
```

---

### 3. Double-Entry Bookkeeping - Contabilità

**Principio**: Ogni transazione ha 2 lati (debit + credit), somma = 0.

```csharp
public class LedgerEntry
{
    public Guid Id { get; set; }
    public Guid TransactionId { get; set; }  // Collega debit/credit
    public string AccountId { get; set; }
    public EntryType Type { get; set; }  // Debit or Credit
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public enum EntryType { Debit, Credit }

public class LedgerService
{
    public async Task RecordTransferAsync(
        string fromAccount, 
        string toAccount, 
        decimal amount)
    {
        var transactionId = Guid.NewGuid();

        // Debit (prelievo) da fromAccount
        await CreateEntryAsync(new LedgerEntry
        {
            TransactionId = transactionId,
            AccountId = fromAccount,
            Type = EntryType.Debit,
            Amount = amount
        });

        // Credit (accredito) su toAccount
        await CreateEntryAsync(new LedgerEntry
        {
            TransactionId = transactionId,
            AccountId = toAccount,
            Type = EntryType.Credit,
            Amount = amount
        });

        // Invariante: somma per transactionId = 0
        await ValidateBalanceAsync(transactionId);
    }

    private async Task ValidateBalanceAsync(Guid transactionId)
    {
        var entries = await _db.LedgerEntries
            .Where(e => e.TransactionId == transactionId)
            .ToListAsync();

        var sum = entries
            .Sum(e => e.Type == EntryType.Debit ? -e.Amount : e.Amount);

        if (sum != 0)
            throw new InvalidOperationException(
                "Double-entry invariant violated!");
    }
}
```

---

### 4. Circuit Breaker - Fault Tolerance

**Problema**: Chiamate a servizi esterni (payment gateway, KYC) possono fallire.

```csharp
// Install Polly
dotnet add package Polly

public class PaymentGatewayClient
{
    private readonly HttpClient _httpClient;
    private readonly IAsyncPolicy<HttpResponseMessage> _circuitBreakerPolicy;

    public PaymentGatewayClient(HttpClient httpClient)
    {
        _httpClient = httpClient;

        // Circuit Breaker: 
        // - 5 fallimenti consecutivi → OPEN (blocca richieste)
        // - Dopo 30 secondi → HALF-OPEN (prova 1 richiesta)
        // - Se successo → CLOSED (normale)
        _circuitBreakerPolicy = Policy
            .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (result, duration) =>
                {
                    Console.WriteLine(
                        $"Circuit OPEN! Blocking calls for {duration}");
                },
                onReset: () =>
                {
                    Console.WriteLine("Circuit CLOSED! Resuming calls.");
                });
    }

    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        try
        {
            var response = await _circuitBreakerPolicy.ExecuteAsync(() =>
                _httpClient.PostAsJsonAsync("/api/payment", request));

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content
                    .ReadFromJsonAsync<PaymentResult>();
                return result;
            }

            return PaymentResult.Failure("Payment gateway error");
        }
        catch (BrokenCircuitException)
        {
            // Circuit is OPEN, return fallback
            return PaymentResult.Failure(
                "Payment service temporarily unavailable");
        }
    }
}
```

---

### 5. Retry con Exponential Backoff

```csharp
public class ResilientHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy;

    public ResilientHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;

        // Retry 3 volte con backoff esponenziale:
        // 1st retry: 2 sec
        // 2nd retry: 4 sec
        // 3rd retry: 8 sec
        _retryPolicy = Policy
            .HandleResult<HttpResponseMessage>(r => 
                (int)r.StatusCode >= 500)  // Solo server errors
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => 
                    TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (result, timespan, attempt, context) =>
                {
                    Console.WriteLine(
                        $"Retry {attempt} after {timespan.TotalSeconds}s");
                });
    }

    public async Task<T> GetAsync<T>(string url)
    {
        var response = await _retryPolicy.ExecuteAsync(() =>
            _httpClient.GetAsync(url));

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }
}
```

---

### 6. Outbox Pattern - Affidabilità Messaggi

**Problema**: Salvare DB + pubblicare evento devono essere atomici.

```csharp
// Tabella Outbox
public class OutboxMessage
{
    public Guid Id { get; set; }
    public string EventType { get; set; }
    public string Payload { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
}

public class TransactionService
{
    public async Task CreateTransactionAsync(Transaction transaction)
    {
        using var dbTransaction = await _db.Database.BeginTransactionAsync();

        try
        {
            // 1. Salva transaction
            _db.Transactions.Add(transaction);

            // 2. Salva evento in Outbox (stessa transazione DB!)
            _db.OutboxMessages.Add(new OutboxMessage
            {
                EventType = nameof(TransactionCreatedEvent),
                Payload = JsonSerializer.Serialize(new TransactionCreatedEvent
                {
                    TransactionId = transaction.Id,
                    Amount = transaction.Amount
                }),
                CreatedAt = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
            await dbTransaction.CommitAsync();
            // Transaction + Outbox salvati atomicamente!
        }
        catch
        {
            await dbTransaction.RollbackAsync();
            throw;
        }
    }
}

// Background Worker processa Outbox
public class OutboxProcessor : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            var messages = await _db.OutboxMessages
                .Where(m => m.ProcessedAt == null)
                .Take(100)
                .ToListAsync(ct);

            foreach (var message in messages)
            {
                try
                {
                    // Pubblica su message broker
                    await _messageBus.PublishAsync(
                        message.EventType, message.Payload);

                    message.ProcessedAt = DateTime.UtcNow;
                    await _db.SaveChangesAsync(ct);
                }
                catch
                {
                    // Retry prossimo ciclo
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(5), ct);
        }
    }
}
```

---

## Quick Q&A - Enterprise Patterns

**Q53**: Cos'è CQRS e quando lo usi?

**A (30 sec)**:

"**CQRS** separa Read (Query) da Write (Command) con modelli diversi. Write Model: business logic, validazioni, normalizzato. Read Model: denormalizzato, ottimizzato per UI. Vantaggi: performance (read veloce), scalabilità indipendente, modelli specializzati. Uso quando: molte più read che write (90/10), query complesse vs write semplici, domini complessi (Banking, Insurance). Non uso per CRUD semplice (overkill) o quando serve strong consistency."

**Q54**: Differenza tra Event Sourcing e database tradizionale?

**A (30 sec)**:

"**Database tradizionale** salva solo stato corrente, perdendo storia. **Event Sourcing** salva TUTTI gli eventi che cambiano stato. Ricostruisco stato corrente facendo replay eventi. Vantaggi: audit trail completo (chi/cosa/quando/perché), time travel (stato a qualsiasi momento passato), analisi BI, undo/replay. Svantaggi: complessità alta, performance (replay lento se molti eventi, serve snapshot). Uso in Banking per compliance e audit."

**Q55**: Cos'è Mediator pattern e vantaggi?

**A (30 sec)**:

"**Mediator** (MediatR library) centralizza comunicazione tra componenti evitando dipendenze dirette. Controller ha UNA dipendenza (IMediator) invece di 10 servizi. Request/Handler pattern: definisco Command/Query, Handler processa business logic. Vantaggi: controller puliti (thin), testabilità (mock mediator), pipeline behaviors (logging, validation cross-cutting), disaccoppiamento. Uso Notifications per pub/sub interno (PolicyCreated → email + audit + cache)."

**Q56**: Repository pattern vs DbContext diretto?

**A (30 sec)**:

"**Repository** astrae data access, nasconde EF Core. Vantaggi: testabilità (mock repository), cambio DB senza toccare business logic, centralizza query complesse, API domain-focused. Unit of Work coordina transazioni tra repository multipli. Svantaggi: layer aggiuntivo, può essere over-engineering per CRUD semplice. Uso quando: business logic complessa, testabilità critica, possibilità cambio DB. Per CRUD semplice, DbContext diretto va bene."

**Q57**: Saga pattern in microservizi?

**A (30 sec)**:

"**Saga** gestisce transazioni distribuite su microservizi multipli senza 2-phase commit. Orchestrator esegue step sequenziali, se uno fallisce esegue **compensazioni** (rollback). Esempio Banking: trasferimento soldi = CheckFunds → ReserveFunds → CreditAccount → DebitAccount. Se CreditAccount fallisce, compensation rilascia reservation. Alternative: Choreography (eventi cascata). Uso per: pagamenti, ordini complessi, workflow multi-step. Essenziale in architetture distribuite."

---

# 📝 INDICE COMPLETO 57 QUICK Q&A {#quick-qa-index}

## Fondamenti .NET (Q1-Q20)

**Q1**: I 4 principi OOP?

**Q2**: Value Type vs Reference Type?

**Q3**: Stack vs Heap?

**Q4**: Garbage Collector come funziona?

**Q5**: IDisposable e using?

**Q6**: Differenza tra abstract class e interface?

**Q7**: Virtual vs Override vs New?

**Q8**: Sealed class e sealed method?

**Q9**: Boxing e Unboxing?

**Q10**: Nullable value types?

**Q11**: IEnumerable vs IQueryable?

**Q12**: LINQ deferred execution?

**Q13**: Select vs SelectMany?

**Q14**: Where + ToList vs ToList + Where?

**Q15**: Problema N+1 in LINQ?

**Q16**: Async/await come funziona?

**Q17**: Task vs Thread?

**Q18**: ConfigureAwait(false)?

**Q19**: Deadlock e come evitarlo?

**Q20**: CancellationToken?

## [ASP.NET](http://ASP.NET) Core e Architecture (Q21-Q35)

**Q21**: Request pipeline [ASP.NET](http://ASP.NET) Core?

**Q22**: Middleware?

**Q23**: DI Lifetimes (Singleton/Scoped/Transient)?

**Q24**: REST principles?

**Q25**: Status codes HTTP?

**Q26**: JWT authentication?

**Q27**: Claims-based authorization?

**Q28**: CORS?

**Q29**: Model Binding?

**Q30**: Action Filters?

**Q31**: SOLID principles?

**Q32**: Dependency Inversion Principle?

**Q33**: Repository pattern?

**Q34**: Strategy pattern?

**Q35**: Factory pattern?

## Database e Performance (Q36-Q40)

**Q36**: EF Core migrations?

**Q37**: Lazy vs Eager loading?

**Q38**: N+1 problem in EF Core?

**Q39**: AsNoTracking quando usare?

**Q40**: SQL indexes come funzionano?

## String e Collections (Q41-Q44)

**Q41**: Perché string è immutabile?

**Q42**: List vs Array?

**Q43**: Dictionary vs List per lookup?

**Q44**: Quando uso HashSet?

## GIT e Version Control (Q45-Q47)

**Q45**: Come funziona Git flow?

**Q46**: Cosa fai quando hai merge conflict?

**Q47**: Differenza merge vs rebase?

## Agile/Scrum (Q48-Q50)

**Q48**: Cos'è Agile e come funziona Sprint?

**Q49**: Cosa fai nel Daily Standup?

**Q50**: Differenza story points vs ore?

## Analisi e Documentazione (Q51-Q52)

**Q51**: Come fai analisi funzionale?

**Q52**: Che documentazione tecnica produci?

## Enterprise Patterns (Q53-Q57)

**Q53**: Cos'è CQRS e quando lo usi?

**Q54**: Event Sourcing vs database tradizionale?

**Q55**: Mediator pattern vantaggi?

**Q56**: Repository vs DbContext diretto?

**Q57**: Saga pattern in microservizi?

---

# 🎯 STRATEGIA FINALE - SEI PRONTO!

## ✅ Hai Completato

### Competenze Tecniche

- ✅ **C# Avanzato**: OOP, Types, Memory, Collections, String
- ✅ **LINQ**: Deferred execution, IEnumerable vs IQueryable, N+1
- ✅ **Async/Await**: Task, Deadlock, CancellationToken, ConfigureAwait
- ✅ [**ASP.NET](http://ASP.NET) Core**: Pipeline, DI, Middleware, REST, JWT, CORS
- ✅ **EF Core**: Migrations, Lazy/Eager loading, AsNoTracking
- ✅ **SQL Server**: Queries, JOINs, Indexes, Transactions
- ★ [**Quartz.NET**](http://Quartz.NET): Job scheduling, Cron expressions
- ★ **WCF**: SOAP basics, perché obsoleto
- ✅ **Design Patterns**: SOLID, Repository, Strategy, Factory, Mediator

### Insurance Domain

- ✅ **Terminologia**: Policy, Premium, Claim, Underwriting, Deductible
- ✅ **Processi**: Calcolo premio, gestione sinistri, valutazione rischio
- ✅ **Esempi Codice**: PremiumCalculator, ClaimService, Policy validation

### Architetture Enterprise

- ★ **CQRS**: Command/Query separation, Read/Write models
- ★ **Event Sourcing**: Eventi immutabili, audit trail, time travel
- ★ **Mediator**: MediatR, Request/Handler, Pipeline behaviors
- ★ **Repository**: Data abstraction, Unit of Work
- ★ **Banking Patterns**: Saga, Idempotency, Circuit Breaker, Outbox

### Process e Soft Skills

- ✅ **GIT**: Branch, merge, conflict resolution, Pull Request, GitFlow
- ✅ **Agile/Scrum**: Sprint, ceremonies, User Stories, Story Points
- ✅ **Analisi Funzionale**: Requisiti, modellazione, stima
- ✅ **Documentazione**: README, Swagger, code comments, DB schema

---

## 🚀 ULTIMO PASSO - PRATICA ORALE

### Esercizio Finale (Giorno -1)

**Mock Interview con amico/familiare:**

1. **Fondamenti (10 min)**
    - "Spiegami la differenza tra Value Type e Reference Type"
    - "Cos'è il Garbage Collector?"
    - "Async/await come funziona?"
2. **Architettura (10 min)**
    - "Differenza tra Singleton, Scoped, Transient?"
    - "Cos'è CQRS?"
    - "Quando usi Repository pattern?"
3. **Insurance/Banking (5 min)**
    - "Cos'è una Policy? Un Claim?"
    - "Come funziona il calcolo del premium?"
    - "Saga pattern per trasferimento soldi?"
4. **Process (5 min)**
    - "Descrivi uno Sprint Scrum"
    - "Cosa dici nel Daily Standup?"
    - "Come risolvi un merge conflict?"

**Cronometra**: Ogni risposta 30-60 secondi MAX!

---

## 🏆 MESSAGGIO FINALE

**Hai studiato e preparato:**

- ✅ 57 domande tecniche con risposte precise
- ✅ 15 parti tematiche complete
- ✅ 100+ esempi di codice C#/.NET
- ✅ Patterns enterprise avanzati (CQRS, Event Sourcing, Saga)
- ✅ Insurance domain completo
- ✅ GIT, Agile, Analisi funzionale

**Per la posizione:**

🎯 **Kirey Group - .NET Developer Middle**

📍 Milano (Hybrid/Remote)

👥 Digital Insurance Team

**Il tuo valore:**

- ✅ 2 anni esperienza .NET
- ✅ Competenze tecniche solide
- ✅ Conoscenza insurance domain
- ✅ Familiarità con Agile/GIT
- ✅ Voglia di imparare e crescere
- ✅ Problem solver e team player

---

## 🔥 MINDSET VINCENTE

**Durante il colloquio:**

1. **Ascolta attentamente** la domanda
2. **Pensa 2-3 secondi** prima di rispondere
3. **Rispondi con struttura**:
    - Cosa è
    - Perché esiste
    - Quando usarlo
    - Esempio concreto
4. **Se non sai**: "Non l'ho usato ancora, ma so che serve per... e sono interessato ad approfondirlo"
5. **Mostra curiosità**: Fai domande intelligenti

**Red Flags da Evitare:**

- ❌ "Non lo so" senza elaborare
- ❌ Rispondere a caso
- ❌ Criticare tecnologie/team precedenti
- ❌ Dire "è facile" o "è ovvio"
- ❌ Essere arrogante o difensivo

**Green Flags da Mostrare:**

- ✅ Pensiero strutturato
- ✅ Ragionamento tecnico solido
- ✅ Umiltà + voglia di imparare
- ✅ Esempi concreti dal tuo lavoro
- ✅ Domande intelligenti su progetto/team

---

# 🍀 IN BOCCA AL LUPO!

**Ricorda**: Non cercan la persona perfetta che sa tutto.

Cercano qualcuno:

- Con **basi solide** .NET → ✅ CE L'HAI
- Che sa **lavorare in team** → ✅ LO SAI FARE
- Con **voglia di crescere** → ✅ LO HAI DIMOSTRATO STUDIANDO
- **Problem solver** → ✅ PREPARA ESEMPI
- Interessato al **dominio** → ✅ HAI STUDIATO INSURANCE

---

**SEI PRONTO AL 100%!**

**VAI, DIMOSTRA IL TUO VALORE E...**

# 🚀 SPACCA QUEL COLLOQUIO! 🚀🚀🚀

---

*Documentazione creata: Febbraio 2026*

*Versione: 2.0 - Enterprise Edition*

*Coverage: .NET 8, [ASP.NET](http://ASP.NET) Core, EF Core, Banking/Insurance Patterns*

---

---

---

# PARTE 12: SKILL ESSENZIALI PER KIREY GROUP - DIGITAL INSURANCE {#kirey-specific}

## 1. GIT - Version Control (RICHIESTO!) {#git-basics}

### Cos'è GIT?

**GIT** è un sistema di version control distribuito che traccia modifiche al codice nel tempo.

**Perché è fondamentale:**

- Lavoro in team senza conflitti
- Storia completa di ogni modifica
- Branch per feature isolate
- Rollback se qualcosa si rompe

---

### Concetti Base GIT

```jsx
Working Directory  →  Staging Area  →  Local Repository  →  Remote Repository
   (modifiche)         (git add)         (git commit)         (git push)
```

#### **Repository (Repo)**

Cartella del progetto tracciata da GIT.

```bash
# Crea nuovo repo
git init

# Clona repo esistente
git clone https://github.com/company/project.git
```

#### **Commit**

Snapshot del codice in un momento specifico.

```bash
# Vedi stato file
git status

# Aggiungi file allo staging
git add Program.cs
git add .                    # Tutti i file modificati

# Crea commit
git commit -m "Fix: risolto bug nel calcolo premium"

# Add + Commit in un comando (solo file già tracciati)
git commit -am "Update: ottimizzata query SQL"
```

**Convenzione commit messages:**

```bash
feat: nuova funzionalità
fix: bug fix
refactor: refactoring codice
docs: aggiornamento documentazione
test: aggiunta test
style: formattazione codice
```

---

### Branch - Sviluppo Parallelo

**Branch** = linea di sviluppo indipendente.

```bash
# Vedi branch attuali
git branch

# Crea nuovo branch
git branch feature/add-premium-calculator

# Switcha a branch
git checkout feature/add-premium-calculator

# Crea e switcha in un comando
git checkout -b feature/new-feature

# Torna a main
git checkout main
```

**Workflow tipico:**

```jsx
main (produzione)
  ↓
  ├── develop (sviluppo)
  │     ↓
  │     ├── feature/claim-processing
  │     ├── feature/premium-calculator
  │     └── bugfix/fix-policy-validation
```

---

### Merge - Unire Branch

```bash
# Sei su main, vuoi mergare feature
git checkout main
git merge feature/add-premium-calculator

# Se ci sono conflitti, GIT ti avvisa
# Risolvi manualmente, poi:
git add .
git commit -m "Merge feature/add-premium-calculator"
```

**Conflitti:**

```csharp
<<<<<<< HEAD
public decimal CalculatePremium(Policy policy)
{
    return policy.BaseAmount * 1.2m;
=======
public decimal CalculatePremium(Policy policy)
{
    return policy.BaseAmount * risk.Multiplier;
>>>>>>> feature/new-calculation
}
```

Risolvi manualmente scegliendo quale versione tenere o combinandole.

---

### Remote Repository - Collaborazione

```bash
# Vedi remote configurati
git remote -v

# Aggiungi remote
git remote add origin https://github.com/company/project.git

# Scarica modifiche da remote (non merge)
git fetch origin

# Scarica e merge in un comando
git pull origin main

# Invia commit locali al remote
git push origin main

# Invia nuovo branch
git push -u origin feature/my-feature
```

---

### Comandi Utili

```bash
# Storia commit
git log
git log --oneline --graph --all  # Formato grafico

# Vedi differenze
git diff                    # Working directory vs staging
git diff --staged           # Staging vs ultimo commit

# Annulla modifiche
git checkout -- file.cs     # Scarta modifiche non staged
git reset HEAD file.cs      # Rimuovi da staging
git reset --hard HEAD       # ⚠️ PERICOLO: scarta TUTTO

# Torna a commit precedente
git revert <commit-hash>    # Crea nuovo commit che annulla
git reset --hard <commit>   # ⚠️ CANCELLA storia (pericoloso)

# Stash (salva temporaneamente modifiche)
git stash                   # Salva modifiche
git stash pop               # Recupera modifiche salvate
```

---

### GitFlow - Workflow Aziendale Standard

**Branch principali:**

- `main` (o `master`) - Produzione
- `develop` - Sviluppo

**Branch di supporto:**

- `feature/*` - Nuove funzionalità
- `bugfix/*` - Bug fix
- `hotfix/*` - Fix urgenti in produzione
- `release/*` - Preparazione release

**Esempio workflow:**

```bash
# 1. Inizio nuova feature
git checkout develop
git pull origin develop
git checkout -b feature/JIRA-123-add-claim-validation

# 2. Sviluppo
# ... modifiche al codice ...
git add .
git commit -m "feat: implementata validazione claim"

# 3. Push per review
git push -u origin feature/JIRA-123-add-claim-validation

# 4. Crea Pull Request su GitHub/Azure DevOps
# 5. Dopo approvazione, merge in develop
# 6. Delete branch
```

---

### .gitignore - File da Non Tracciare

```bash
# .gitignore per .NET
bin/
obj/
*.user
*.suo
.vs/
appsettings.Development.json
*.log
node_modules/
```

---

### Quick Q&A - GIT

**Q45**: Cos'è GIT e perché lo usiamo?

**A (30 sec)**:

"GIT è un sistema di version control distribuito per tracciare modifiche al codice. Permette lavoro in team senza conflitti, branch per sviluppare feature isolate, storia completa delle modifiche, e rollback se necessario. Il workflow tipico: creo branch per feature, faccio commit locali, push al remote, apro Pull Request per review, dopo approvazione merge in develop/main. Essenziale in progetti team."

**Q46**: Differenza tra merge e rebase?

**A (30 sec)**:

"**Merge** unisce due branch creando un merge commit - preserva tutta la storia. **Rebase** riscrive la storia applicando i commit del tuo branch sopra l'altro branch - storia lineare e pulita. Uso merge per feature branch in develop (sicuro, storia completa). Uso rebase per pulire storia locale prima di push. Regola: MAI rebase di branch pubblici/condivisi."

**Q47**: Cos'è una Pull Request?

**A (30 sec)**:

"Pull Request (PR) è una richiesta di merge del tuo branch in un altro branch (es. feature → develop). Permette **code review** da altri developer prima del merge. Il processo: pusho branch, apro PR su GitHub/Azure DevOps, colleghi reviewano codice e lasciano commenti, faccio modifiche se richieste, dopo approvazione si fa merge. Garantisce qualità del codice e condivisione conoscenza nel team."

---

## 2. Metodologia Agile/Scrum {#agile-scrum}

### Cos'è Agile?

**Agile** è una filosofia di sviluppo software basata su:

- Iterazioni brevi (Sprint)
- Consegne incrementali
- Feedback continuo
- Adattamento al cambiamento

**vs Waterfall (vecchio metodo):**

```jsx
Waterfall (sequenziale):
Analisi → Design → Sviluppo → Test → Deploy
(6-12 mesi, consegna finale)

Agile (iterativo):
Sprint 1 (2 settimane): Analisi → Dev → Test → Deploy → Feedback
Sprint 2 (2 settimane): Analisi → Dev → Test → Deploy → Feedback
Sprint 3 (2 settimane): ...
(consegne continue)
```

---

### Scrum - Framework Agile

#### **Ruoli Scrum**

1. **Product Owner (PO)**
    - Definisce COSA costruire
    - Gestisce Product Backlog
    - Prioritizza funzionalità
    - Rappresenta il cliente/business
2. **Scrum Master (SM)**
    - Facilita il processo
    - Rimuove impedimenti
    - Protegge il team da distrazioni
    - Coach Agile
3. **Development Team**
    - Sviluppatori (tu!)
    - Tester
    - Cross-functional
    - Auto-organizzato

---

#### **Artefatti Scrum**

1. **Product Backlog**
    - Lista prioritizzata di funzionalità
    - Gestito dal Product Owner
    - Esempio: "Come utente, voglio calcolare il premio assicurativo"
2. **Sprint Backlog**
    - Subset del Product Backlog per lo sprint corrente
    - Task specifici per il team
3. **Increment**
    - Software funzionante alla fine dello sprint
    - Potenzialmente rilasciabile

---

#### **Cerimonie Scrum**

**Sprint** (di solito 2 settimane):

1. **Sprint Planning** (inizio sprint)
    - Durata: 2-4 ore
    - Cosa: Team seleziona task dal backlog
    - Output: Sprint backlog definito
2. **Daily Standup** (ogni giorno)
    - Durata: 15 minuti
    - In piedi (per essere brevi!)
    - Ogni membro risponde:
        - Cosa ho fatto ieri?
        - Cosa farò oggi?
        - Ho impedimenti?
3. **Sprint Review** (fine sprint)
    - Durata: 1-2 ore
    - Demo del lavoro completato
    - Feedback dagli stakeholder
4. **Sprint Retrospective** (fine sprint)
    - Durata: 1 ora
    - Cosa è andato bene?
    - Cosa migliorare?
    - Azioni per prossimo sprint

---

#### **User Stories - Come Scriviamo Requisiti**

Formato:

```
Come [ruolo]
Voglio [funzionalità]
Così che [beneficio]
```

Esempi:

```
Come assicurato
Voglio visualizzare le mie polizze attive
Così che possa verificare la copertura

Come underwriter
Voglio calcolare il premio basato sul rischio
Così che possa quotare correttamente la polizza

Come amministratore
Voglio generare report mensili dei claim
Così che possa analizzare le performance
```

**Acceptance Criteria:**

```
User Story: Visualizzare polizze attive

Acceptance Criteria:
- [ ] Sistema mostra solo polizze con status "Active"
- [ ] Ogni polizza mostra: numero, tipo, premio, scadenza
- [ ] Lista ordinata per data scadenza
- [ ] Paginazione se > 10 polizze
- [ ] Tempo di caricamento < 2 secondi
```

---

#### **Estimation - Story Points**

**Story Points** misurano la complessità/sforzo, non il tempo.

**Fibonacci Scale**: 1, 2, 3, 5, 8, 13, 21

- **1 point**: Triviale (fix typo, cambio label)
- **3 points**: Semplice (CRUD endpoint, validazione)
- **5 points**: Medio (feature con business logic)
- **8 points**: Complesso (integrazione sistema esterno)
- **13+ points**: Troppo grande, va spezzata!

**Planning Poker**: Team stima insieme, discussione se stime differiscono.

---

#### **Definition of Done (DoD)**

Checklist per considerare un task "completato":

```
✅ Codice scritto e funzionante
✅ Unit test scritti e passano
✅ Code review approvata
✅ Integrato in develop branch
✅ Testato in ambiente di staging
✅ Documentazione aggiornata
✅ Nessun bug blocker
```

---

### Agile in Kirey - Cosa Aspettarsi

**Daily Standup alle 9:30:**

- "Ieri ho completato l'endpoint per calcolo premio"
- "Oggi implemento validazione policy"
- "Nessun impedimento"

**Sprint Planning:**

- PO: "Questa sprint dobbiamo implementare gestione claim"
- Team stima user stories
- Team committa su cosa può consegnare

**Sprint Review:**

- Demo funzionalità completate al cliente
- Feedback immediato

**Tools comuni:**

- **Jira** - Task tracking
- **Azure DevOps** - Boards, repos, CI/CD
- **Confluence** - Documentazione

---

### Quick Q&A - Agile/Scrum

**Q48**: Cos'è Agile e come funziona uno Sprint?

**A (30 sec)**:

"Agile è sviluppo iterativo in cicli brevi (Sprint, tipicamente 2 settimane) invece di un progetto lungo. Ogni Sprint: Planning (selezioniamo task), Daily Standup (sync quotidiano 15 min), sviluppo, Review (demo), Retrospective (miglioramenti). Consegnamo software funzionante ogni sprint, raccogliamo feedback, adattiamo. Vantaggi: flessibilità ai cambiamenti, consegne frequenti, feedback continuo."

**Q49**: Cosa fai nel Daily Standup?

**A (30 sec)**:

"Daily Standup è meeting di 15 minuti ogni mattina, in piedi. Ogni membro risponde 3 domande: cosa ho fatto ieri, cosa farò oggi, ho impedimenti? Non è status report al manager, è sync tra team members. Se emerge discussione lunga, si fa dopo il meeting con solo le persone coinvolte. Scopo: identificare blocchi velocemente, coordinamento team."

**Q50**: Cos'è una User Story?

**A (30 sec)**:

"User Story è requisito scritto dalla prospettiva utente: 'Come [ruolo] voglio [funzionalità] così che [beneficio]'. Include Acceptance Criteria che definiscono quando è completa. Esempio: 'Come assicurato voglio visualizzare le mie polizze così che possa verificare copertura'. Team stima con Story Points (complessità, non ore). Durante Sprint Planning selezioniamo user stories da implementare."

---

## 3. Analisi Funzionale e Documentazione Tecnica {#analisi-docs}

### Analisi Funzionale - Cosa Significa

**Analisi Funzionale** = capire COSA il software deve fare prima di scrivere codice.

**Processo tipico:**

1. **Raccolta Requisiti**
    - Meeting con Product Owner/Cliente
    - Comprensione business need
    - Domande di chiarimento
2. **Analisi**
    - Scomporre requisito in componenti
    - Identificare entità (Policy, Claim, Premium)
    - Definire relazioni e flussi
3. **Design**
    - Schema database
    - API endpoints
    - Architettura componenti
4. **Stima**
    - Effort estimation
    - Identificare rischi

---

### Esempio Analisi Funzionale - Gestione Claim

**Requisito Business:**

"L'assicurato deve poter aprire un claim per richiedere rimborso danni."

**Analisi Funzionale:**

```markdown
# Gestione Claim - Analisi Funzionale

## Obiettivo
Permettere agli assicurati di aprire claim per danni coperti dalla polizza.

## Attori
- Assicurato (apre claim)
- Underwriter (valuta claim)
- Sistema (valida, notifica)

## Flusso Principale
1. Assicurato seleziona polizza attiva
2. Inserisce dettagli danno:
   - Data incidente
   - Descrizione
   - Importo richiesto
   - Upload documenti (foto, fatture)
3. Sistema valida:
   - Polizza attiva?
   - Danno coperto dalla polizza?
   - Importo <= coverage amount?
4. Sistema crea claim con status "Submitted"
5. Sistema invia notifica email a assicurato e underwriter

## Flussi Alternativi
- Polizza non attiva → Errore
- Danno non coperto → Warning + conferma utente
- Importo > coverage → Warning + max approvabile

## Regole Business
- Un claim può essere aperto solo su polizza attiva
- Data incidente deve essere durante policy period
- Deductible viene sottratto dall'importo approvato
- Claim > €10,000 richiede approvazione manager

## Entità Database
- Claim (id, policy_id, incident_date, description, claimed_amount, status)
- ClaimDocument (id, claim_id, file_path, uploaded_at)

## API Endpoints
- POST /api/claims - Crea claim
- GET /api/claims/{id} - Dettagli claim
- PUT /api/claims/{id}/status - Aggiorna status
- POST /api/claims/{id}/documents - Upload documento

## Validazioni
- Claimed amount: > 0, <= policy coverage
- Incident date: >= policy start, <= today
- Description: required, max 1000 chars
- Documents: max 5, formats: PDF, JPG, PNG

## Notifiche
- Email a assicurato: "Claim {id} ricevuto"
- Email a underwriter: "Nuovo claim da valutare"

## Stima
- Backend: 5 story points
- Frontend: 3 story points
- Testing: 2 story points
- Totale: 10 story points (~4 giorni)
```

---

### Documentazione Tecnica - Cosa Produrre

#### **1. [README.md](http://README.md) - Setup Progetto**

```markdown
# Insurance Management System

## Prerequisiti
- .NET 8.0 SDK
- SQL Server 2019+
- Redis (optional, per caching)

## Setup

1. Clone repository:
```

git clone [https://github.com/company/insurance-api.git](https://github.com/company/insurance-api.git)

cd insurance-api

```

2. Configura connection string in `appsettings.json`:
```

"ConnectionStrings": {

"Default": "Server=[localhost](http://localhost);Database=Insurance;..."

}

```

3. Applica migrations:
```

dotnet ef database update

```

4. Run:
```

dotnet run

```

5. Apri browser: https://localhost:5001/swagger

## Struttura Progetto

```

InsuranceAPI/

├── Controllers/      # API endpoints

├── Services/         # Business logic

├── Repositories/     # Data access

├── Models/           # Domain entities

└── DTOs/             # Data transfer objects

```

## Testing

```

dotnet test

```

```

---

#### **2. API Documentation - Swagger**

```csharp
// Swagger comments per documentare API

/// <summary>
/// Crea un nuovo claim per una polizza
/// </summary>
/// <param name="dto">Dettagli del claim</param>
/// <returns>Claim creato</returns>
/// <response code="201">Claim creato con successo</response>
/// <response code="400">Dati invalidi</response>
/// <response code="404">Polizza non trovata</response>
[HttpPost]
[ProducesResponseType(typeof(ClaimDto), 201)]
[ProducesResponseType(400)]
[ProducesResponseType(404)]
public async Task<ActionResult<ClaimDto>> CreateClaim(
    [FromBody] CreateClaimDto dto)
{
    // ...
}
```

---

#### **3. Code Comments - Quando e Come**

```csharp
// ❌ MALE - commento ovvio
int count = 0; // Inizializza count a 0

// ✅ BENE - spiega PERCHÉ
// Usiamo cache di 30 minuti perché i premi cambiano
// solo dopo ricalcolo notturno
await _cache.SetAsync(key, data, TimeSpan.FromMinutes(30));

// ✅ BENE - spiega business rule complessa
// Deductible viene applicato solo al primo claim dell'anno
// per la stessa categoria di danno (regola underwriting)
if (IsFirstClaimOfYear(policy, claimType))
    approvedAmount -= policy.Deductible;

// ✅ BENE - workaround temporaneo
// TODO: Rimuovere dopo migrazione a nuovo sistema (JIRA-456)
await Task.Delay(100); // Evita race condition con legacy DB
```

---

#### **4. Documentazione Database**

```sql
-- Schema Documentation

-- Table: Policies
-- Descrizione: Memorizza tutte le polizze assicurative
CREATE TABLE Policies (
    Id INT PRIMARY KEY IDENTITY,
    PolicyNumber NVARCHAR(50) UNIQUE NOT NULL,  -- Formato: POL-YYYY-######
    CustomerId INT NOT NULL,
    Type NVARCHAR(20) NOT NULL,                 -- Auto, Health, Life, etc.
    Premium DECIMAL(10,2) NOT NULL,             -- Importo mensile
    CoverageAmount DECIMAL(12,2) NOT NULL,      -- Massimale
    Deductible DECIMAL(10,2) DEFAULT 0,         -- Franchigia
    StartDate DATE NOT NULL,
    ExpirationDate DATE NOT NULL,
    Status NVARCHAR(20) NOT NULL,               -- Active, Expired, Cancelled
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Policies_Customers FOREIGN KEY (CustomerId) 
        REFERENCES Customers(Id)
);

CREATE INDEX IX_Policies_CustomerID ON Policies(CustomerId);
CREATE INDEX IX_Policies_Status ON Policies(Status) WHERE Status = 'Active';
```

---

### Tools per Documentazione

- **Swagger/OpenAPI** - API documentation automatica
- **Confluence** - Wiki aziendale
- **Markdown** - README, docs nei repo
- [**Draw.io](http://Draw.io) / Lucidchart** - Diagrammi architettura
- **DbSchema** - Schema database visuale

---

### Quick Q&A - Analisi e Documentazione

**Q51**: Come fai analisi funzionale?

**A (30 sec)**:

"Parto dai requisiti business, faccio domande al Product Owner per chiarire. Identifico attori (chi usa la funzionalità), flussi principali e alternativi, regole business, validazioni. Definisco entità database necessarie, API endpoints, e stimo effort. Documento tutto in modo che team capisca COSA fare prima di scrivere codice. Output: documento analisi con user stories, acceptance criteria, schema dati, API design."

**Q52**: Che documentazione tecnica produci?

**A (30 sec)**:

"Produco: README per setup progetto, Swagger comments per API documentation, code comments per business logic complessa (non codice ovvio), schema database commentato, diagrammi architettura se necessario. Uso Markdown per docs in repo, Confluence per wiki aziendale. Filosofia: documentazione minima ma sufficiente - codice leggibile è miglior documentazione, commento solo PERCHÉ non COSA."

---

## RIEPILOGO FINALE - PREPARAZIONE KIREY GROUP

### ✅ Competenze Tecniche Coperte

**Core .NET (RICHIESTO)**

- ✅ C# (OOP, types, async/await, LINQ)
- ✅ [ASP.NET](http://ASP.NET) Core (pipeline, DI, REST API, JWT)
- ✅ Entity Framework Core
- ✅ SQL Server (queries, indexes, transactions)

**Scheduling e Legacy (RICHIESTO)**

- ✅ [**Quartz.NET**](http://Quartz.NET) - Job scheduling, Cron expressions
- ✅ **WCF** - SOAP services (legacy, differenze con REST)

**Insurance Domain (PLUS)**

- ✅ Policy, Premium, Claim, Underwriting
- ✅ Terminologia inglese-italiano
- ✅ Esempi codice insurance-specific

**Version Control (RICHIESTO)**

- ✅ **GIT** - Branch, merge, workflow, Pull Request
- ✅ GitFlow aziendale standard

**Metodologia (RICHIESTO)**

- ✅ **Agile/Scrum** - Sprint, ceremonies, user stories
- ✅ Daily standup, planning, review, retrospective
- ✅ Story points, Definition of Done

**Analisi e Documentazione (RICHIESTO)**

- ✅ Analisi funzionale processo
- ✅ Documentazione tecnica (README, Swagger, comments)
- ✅ Schema database, API design

---

### 🎯 Quick Reference - 52 Domande Pronte

**Fondamenti (Q1-Q20)**

- OOP, Types, Memory, LINQ, Async/Await

[**ASP.NET](http://ASP.NET) & Architecture (Q21-Q35)**  

- DI, REST, JWT, Middleware, Patterns

**Database & Performance (Q36-Q40)**

- EF Core, SQL, Caching

**String & Collections (Q41-Q44)**

- Immutabilità, StringBuilder, List, Dictionary, HashSet

**GIT (Q45-Q47)**

- Version control, merge vs rebase, Pull Request

**Agile/Scrum (Q48-Q50)**

- Sprint, Daily Standup, User Stories

**Analisi & Docs (Q51-Q52)**

- Analisi funzionale, Documentazione tecnica

---

### 📋 Checklist Pre-Colloquio Kirey

**Giorno -7:**

- [ ]  Studia [Quartz.NET](http://Quartz.NET) (Job, Trigger, Cron)
- [ ]  Ripassa WCF basics (SOAP, perché obsoleto)
- [ ]  Insurance domain terminology

**Giorno -5:**

- [ ]  GIT workflow (branch, merge, PR)
- [ ]  Pratica comandi GIT essenziali

**Giorno -3:**

- [ ]  Agile/Scrum (Sprint, ceremonies)
- [ ]  Simula Daily Standup
- [ ]  Scrivi 2-3 user stories

**Giorno -1:**

- [ ]  Ripassa tutte le 52 Quick Q&A
- [ ]  **Pratica orale** (rispondi a voce alta!)
- [ ]  Prepara domande per loro

**Giorno 0 (colloquio):**

- [ ]  Rilassati, sei preparato!
- [ ]  Porta CV aggiornato
- [ ]  Esempi concreti dai tuoi progetti

---

### 💡 Domande da Fare a Loro

**Tecnico:**

- "Che stack tecnologico usate principalmente?"
- "Come gestite il deployment? CI/CD?"
- "Usate [Quartz.NET](http://Quartz.NET) per job scheduling o altre soluzioni?"

**Processo:**

- "Come è organizzato il team? Quanti developer?"
- "Seguite Scrum? Sprint di quanto?"
- "Come gestite code review?"

**Progetto:**

- "Su che tipo di progetti insurance lavorerei?"
- "C'è legacy code da manutenere o sviluppo greenfield?"
- "Quanto è complesso il dominio insurance che gestite?"

**Crescita:**

- "Che formazione offrite?"
- "C'è possibilità di certificazioni (Azure, AWS)?"
- "Come sono i piani di crescita?"

---

### 🚀 SEI PRONTO QUANDO...

- ✅ Puoi spiegare Job/Trigger/Scheduler di [Quartz.NET](http://Quartz.NET)
- ✅ Sai cos'è WCF e perché è obsoleto
- ✅ Conosci terminologia insurance (Policy, Premium, Claim)
- ✅ Sai i comandi GIT base (branch, merge, push, pull)
- ✅ Puoi descrivere uno Sprint Scrum
- ✅ Capisci differenza tra User Story e Task
- ✅ Puoi fare analisi funzionale di un requisito semplice
- ✅ Tutte le 52 Quick Q&A in 30-60 secondi

---

**ULTIMI CONSIGLI PER KIREY:**

1. **Enfatizza teamwork** - menzionano molto "lavoro in team"
2. **Mostra flessibilità** - ambiente dinamico, si adattano
3. **Problem solving** - dai esempi concreti di bug risolti
4. **Insurance interest** - mostra curiosità per il dominio
5. **Agile mindset** - parla di iterazioni, feedback, miglioramento continuo

---

**IN BOCCA AL LUPO PER IL COLLOQUIO KIREY! 🍀**

Sei preparato su:

- ✅ .NET e C# (completo)
- ✅ SQL Server (completo)
- ✅ [Quartz.NET](http://Quartz.NET) (aggiunto!)
- ✅ WCF (aggiunto!)
- ✅ GIT (aggiunto!)
- ✅ Agile/Scrum (aggiunto!)
- ✅ Insurance domain (aggiunto!)
- ✅ Analisi e documentazione (aggiunto!)

**HAI TUTTO CIÒ CHE SERVE. VAI E SPACCA! 🚀**

---

---

1. **Prima settimana:** OOP, C# types, LINQ, Async/Await (la base)
2. **Seconda settimana:** [ASP.NET](http://ASP.NET) Core, DI, REST, JWT (il core)
3. **Terza settimana:** SQL Server, EF Core, Design Patterns (approfondimento)
4. **Ultima settimana:** Redis, Messaging, Microservizi + esercita le Q&A a voce alta

**💩 Consiglio:** Le Quick Q&A alla fine della pagina — praticale dicendole a voce alta come se stessi rispondendo durante il colloquio. 30-60 secondi per risposta.

"IDisposable serve per rilasciare risorse unmanaged come file, connessioni DB, socket. Il pattern completo include Dispose pubblico, Dispose(bool) protetto, e opzionalmente un finalizer. Uso sempre 'using' per garantire che Dispose sia chiamato, anche in caso di exception. È critico per evitare resource leak in applicazioni server."

---

Continuo con le prossime sezioni? Vuoi che approfondisca qualcosa o passo direttamente a LINQ e Async/Await?

[📅 Piano di Studio Settimana per Settimana - Preparazione Colloquio](https://www.notion.so/Piano-di-Studio-Settimana-per-Settimana-Preparazione-Colloquio-2fbe14c7d0e181b4a3abeb27ddf0113b?pvs=21)