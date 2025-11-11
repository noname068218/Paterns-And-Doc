# Principi SOLID in C#

## Introduzione

I **principi SOLID** sono cinque principi di progettazione orientata agli oggetti che aiutano a scrivere codice più pulito, manutenibile e scalabile. Questi principi furono introdotti da Robert C. Martin (Uncle Bob) e sono considerati fondamentali per lo sviluppo software professionale.

---

## 1. Panoramica dei Principi SOLID

### Acronimo SOLID

```
┌─────────────────────────────────────────────────┐
│              PRINCIPI SOLID                     │
└─────────────────────────────────────────────────┘
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│    S      │ │     O     │ │     L     │
│ SINGLE    │ │  OPEN/    │ │ LISKOV    │
│ RESPONSIBILITY│  CLOSED  │ │ SUBSTITUTION│
└───────────┘ └───────────┘ └───────────┘
        │           │           │
        └───────────┼───────────┘
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐
│    I      │ │     D     │
│ INTERFACE │ │ DEPENDENCY│
│ SEGREGATION│  INVERSION │
└───────────┘ └───────────┘
```

---

## 2. S - Single Responsibility Principle (SRP)

### Definizione

**Una classe dovrebbe avere una sola ragione per cambiare.** Ogni classe dovrebbe avere una sola responsabilità.

### Problema: Violazione del SRP

```csharp
// ❌ SBAGLIATO - Troppe responsabilità
public class Persona {
    public string Nome { get; set; }
    public string Email { get; set; }
    
    // Responsabilità 1: Gestione dati
    public void Salva() {
        // Logica di salvataggio nel database
    }
    
    // Responsabilità 2: Validazione
    public bool Valida() {
        // Logica di validazione
        return true;
    }
    
    // Responsabilità 3: Invio email
    public void InviaEmail(string messaggio) {
        // Logica di invio email
    }
    
    // Responsabilità 4: Generazione report
    public void GeneraReport() {
        // Logica di generazione report
    }
}
```

### Diagramma: Violazione SRP

```
┌─────────────────────────────────────────────┐
│         Persona (Troppe responsabilità)      │
├─────────────────────────────────────────────┤
│  - Gestione dati                            │
│  - Validazione                              │
│  - Invio email                              │
│  - Generazione report                       │
│  - Logica business                          │
└─────────────────────────────────────────────┘
         │
         ▼
    Una modifica in una
    responsabilità può
    rompere le altre
```

### Soluzione: Rispetto del SRP

```csharp
// ✅ CORRETTO - Una responsabilità per classe

// Classe per i dati
public class Persona {
    public string Nome { get; set; }
    public string Email { get; set; }
}

// Classe per la validazione
public class PersonaValidator {
    public bool Valida(Persona persona) {
        if (string.IsNullOrEmpty(persona.Nome))
            return false;
        if (!persona.Email.Contains("@"))
            return false;
        return true;
    }
}

// Classe per il salvataggio
public class PersonaRepository {
    public void Salva(Persona persona) {
        // Logica di salvataggio nel database
    }
    
    public Persona Carica(int id) {
        // Logica di caricamento
        return new Persona();
    }
}

// Classe per l'invio email
public class EmailService {
    public void InviaEmail(Persona persona, string messaggio) {
        // Logica di invio email
    }
}

// Classe per i report
public class ReportGenerator {
    public void GeneraReport(Persona persona) {
        // Logica di generazione report
    }
}
```

### Diagramma: Rispetto SRP

```
┌──────────────┐  ┌──────────────┐  ┌──────────────┐
│   Persona    │  │  Validator   │  │  Repository  │
│  (Dati)      │  │  (Validazione)│ │  (Salvataggio)│
└──────────────┘  └──────────────┘  └──────────────┘

┌──────────────┐  ┌──────────────┐
│EmailService  │  │ReportGenerator│
│  (Email)     │  │  (Report)     │
└──────────────┘  └──────────────┘

Ogni classe ha UNA SOLA responsabilità
```

### Esempio Pratico Completo

```csharp
// ✅ CORRETTO - Esempio completo
public class Ordine {
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public decimal Totale { get; set; }
    public List<Prodotto> Prodotti { get; set; }
}

// Responsabilità: Validazione ordini
public class OrdineValidator {
    public bool Valida(Ordine ordine) {
        if (ordine == null) return false;
        if (ordine.Prodotti == null || ordine.Prodotti.Count == 0)
            return false;
        if (ordine.Totale <= 0) return false;
        return true;
    }
}

// Responsabilità: Persistenza ordini
public class OrdineRepository {
    public void Salva(Ordine ordine) {
        // Salva nel database
    }
    
    public Ordine Carica(int id) {
        // Carica dal database
        return new Ordine();
    }
}

// Responsabilità: Calcolo totale
public class CalcolatoreTotale {
    public decimal Calcola(List<Prodotto> prodotti) {
        return prodotti.Sum(p => p.Prezzo * p.Quantita);
    }
}

// Utilizzo
var ordine = new Ordine {
    Prodotti = new List<Prodotto> { /* ... */ }
};

var validator = new OrdineValidator();
if (validator.Valida(ordine)) {
    var calculator = new CalcolatoreTotale();
    ordine.Totale = calculator.Calcola(ordine.Prodotti);
    
    var repository = new OrdineRepository();
    repository.Salva(ordine);
}
```

---

## 3. O - Open/Closed Principle (OCP)

### Definizione

**Le entità software dovrebbero essere aperte per l'estensione ma chiuse per la modifica.** Dovresti poter aggiungere nuove funzionalità senza modificare il codice esistente.

### Problema: Violazione dell'OCP

```csharp
// ❌ SBAGLIATO - Deve essere modificato per ogni nuovo tipo
public class CalcolatoreArea {
    public double CalcolaArea(object forma) {
        if (forma is Cerchio cerchio) {
            return Math.PI * cerchio.Raggio * cerchio.Raggio;
        }
        else if (forma is Rettangolo rettangolo) {
            return rettangolo.Larghezza * rettangolo.Altezza;
        }
        else if (forma is Triangolo triangolo) {
            return (triangolo.Base * triangolo.Altezza) / 2;
        }
        // ❌ Devo modificare questa classe per aggiungere nuove forme!
        return 0;
    }
}
```

### Diagramma: Violazione OCP

```
┌─────────────────────────────────────────────┐
│    CalcolatoreArea                          │
│  + CalcolaArea()                            │
│    - if Cerchio                             │
│    - else if Rettangolo                     │
│    - else if Triangolo                      │
│    - else if ... (continua a crescere)      │
└─────────────────────────────────────────────┘
         │
         ▼
    Ogni nuova forma
    richiede modifica
    del codice esistente
```

### Soluzione: Rispetto dell'OCP

```csharp
// ✅ CORRETTO - Estendibile senza modifiche

// Interfaccia base
public interface IForma {
    double CalcolaArea();
}

// Implementazioni concrete
public class Cerchio : IForma {
    public double Raggio { get; set; }
    
    public double CalcolaArea() {
        return Math.PI * Raggio * Raggio;
    }
}

public class Rettangolo : IForma {
    public double Larghezza { get; set; }
    public double Altezza { get; set; }
    
    public double CalcolaArea() {
        return Larghezza * Altezza;
    }
}

public class Triangolo : IForma {
    public double Base { get; set; }
    public double Altezza { get; set; }
    
    public double CalcolaArea() {
        return (Base * Altezza) / 2;
    }
}

// Calcolatore - CHIUSO per modifiche, APERTO per estensioni
public class CalcolatoreArea {
    public double CalcolaArea(IForma forma) {
        return forma.CalcolaArea();  // Non serve modificare questo codice!
    }
}

// Nuova forma - AGGIUNTA senza modificare codice esistente
public class Esagono : IForma {
    public double Lato { get; set; }
    
    public double CalcolaArea() {
        return (3 * Math.Sqrt(3) * Lato * Lato) / 2;
    }
}
```

### Diagramma: Rispetto OCP

```
┌─────────────────────────────────────────────┐
│         IForma (Interfaccia)                 │
│         + CalcolaArea()                     │
└─────────────────────────────────────────────┘
                ▲
                │
    ┌───────────┼───────────┐
    │           │           │
    ▼           ▼           ▼
┌─────────┐ ┌─────────┐ ┌─────────┐
│ Cerchio │ │Rettangolo│ │Triangolo│
└─────────┘ └─────────┘ └─────────┘
                │
                ▼
        ┌───────────────┐
        │ Esagono (NUOVO)│
        │ (estende senza │
        │  modificare)   │
        └───────────────┘

CalcolatoreArea usa IForma
→ Non serve modificarlo!
```

### Esempio Pratico: Sistema di Sconto

```csharp
// ✅ CORRETTO - Sistema di sconto estendibile
public interface ISconto {
    decimal ApplicaSconto(decimal prezzo);
}

public class NessunoSconto : ISconto {
    public decimal ApplicaSconto(decimal prezzo) {
        return prezzo;
    }
}

public class ScontoPercentuale : ISconto {
    private decimal percentuale;
    
    public ScontoPercentuale(decimal percentuale) {
        this.percentuale = percentuale;
    }
    
    public decimal ApplicaSconto(decimal prezzo) {
        return prezzo * (1 - percentuale / 100);
    }
}

public class ScontoFisso : ISconto {
    private decimal importo;
    
    public ScontoFisso(decimal importo) {
        this.importo = importo;
    }
    
    public decimal ApplicaSconto(decimal prezzo) {
        return Math.Max(0, prezzo - importo);
    }
}

// Classe che usa gli sconti - CHIUSA per modifiche
public class CalcolatorePrezzo {
    public decimal CalcolaPrezzoFinale(decimal prezzoBase, ISconto sconto) {
        return sconto.ApplicaSconto(prezzoBase);
        // Non serve modificare questo codice per nuovi tipi di sconto!
    }
}

// Nuovo tipo di sconto - AGGIUNTO senza modificare codice esistente
public class ScontoProgressivo : ISconto {
    public decimal ApplicaSconto(decimal prezzo) {
        if (prezzo > 1000) return prezzo * 0.85m;
        if (prezzo > 500) return prezzo * 0.90m;
        return prezzo * 0.95m;
    }
}
```

---

## 4. L - Liskov Substitution Principle (LSP)

### Definizione

**Gli oggetti di una superclasse dovrebbero essere sostituibili con oggetti delle sue sottoclassi senza rompere l'applicazione.** Le classi derivate devono essere completamente sostituibili alle loro classi base.

### Problema: Violazione del LSP

```csharp
// ❌ SBAGLIATO - Violazione LSP
public class Rettangolo {
    public virtual int Larghezza { get; set; }
    public virtual int Altezza { get; set; }
    
    public int CalcolaArea() {
        return Larghezza * Altezza;
    }
}

public class Quadrato : Rettangolo {
    // Violazione: Quadrato modifica il comportamento di Rettangolo
    public override int Larghezza {
        set {
            base.Larghezza = value;
            base.Altezza = value;  // Forza altezza = larghezza
        }
    }
    
    public override int Altezza {
        set {
            base.Larghezza = value;  // Forza larghezza = altezza
            base.Altezza = value;
        }
    }
}

// Utilizzo - si rompe!
public void Test(Rettangolo rettangolo) {
    rettangolo.Larghezza = 5;
    rettangolo.Altezza = 10;
    // Si aspetta area = 50
    int area = rettangolo.CalcolaArea();
    // Ma se rettangolo è un Quadrato, area = 100! ❌
}
```

### Diagramma: Violazione LSP

```
┌─────────────────────────────────────────────┐
│  Codice che usa Rettangolo                  │
│  rettangolo.Larghezza = 5;                  │
│  rettangolo.Altezza = 10;                   │
│  int area = rettangolo.CalcolaArea();       │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Rettangolo (base)     │
        │  Area = 50 ✅          │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Quadrato (derivata)  │
        │  Area = 100 ❌        │
        │  (comportamento diverso)│
        └───────────────────────┘
```

### Soluzione: Rispetto del LSP

```csharp
// ✅ CORRETTO - Rispetto LSP

// Classe base astratta
public abstract class Forma {
    public abstract int CalcolaArea();
}

public class Rettangolo : Forma {
    public int Larghezza { get; set; }
    public int Altezza { get; set; }
    
    public override int CalcolaArea() {
        return Larghezza * Altezza;
    }
}

public class Quadrato : Forma {
    public int Lato { get; set; }
    
    public override int CalcolaArea() {
        return Lato * Lato;
    }
}

// Funzione che usa Forma - funziona con qualsiasi sottoclasse
public void Test(Forma forma) {
    int area = forma.CalcolaArea();  // ✅ Funziona sempre correttamente
}

// Utilizzo
Forma rettangolo = new Rettangolo { Larghezza = 5, Altezza = 10 };
Forma quadrato = new Quadrato { Lato = 5 };

Test(rettangolo);  // ✅ Funziona
Test(quadrato);    // ✅ Funziona - completamente sostituibile
```

### Diagramma: Rispetto LSP

```
┌─────────────────────────────────────────────┐
│  Codice che usa Forma                       │
│  forma.CalcolaArea();                       │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Forma (base)         │
        │  + CalcolaArea()      │
        └───────────────────────┘
                    ▲
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│Rettangolo │ │ Quadrato  │ │ Cerchio   │
│Area = 50  │ │Area = 25   │ │Area = ... │
└───────────┘ └───────────┘ └───────────┘

Tutte le sottoclassi sono
completamente sostituibili
```

### Esempio Pratico: Sistema di Pagamento

```csharp
// ✅ CORRETTO - Esempio pagamenti
public abstract class MetodoPagamento {
    public abstract void ProcessaPagamento(decimal importo);
    public abstract bool Valida();
}

public class CartaCredito : MetodoPagamento {
    public string NumeroCarta { get; set; }
    
    public override bool Valida() {
        return !string.IsNullOrEmpty(NumeroCarta) && NumeroCarta.Length == 16;
    }
    
    public override void ProcessaPagamento(decimal importo) {
        // Logica specifica carta di credito
        Console.WriteLine($"Pagamento di {importo} con carta di credito");
    }
}

public class PayPal : MetodoPagamento {
    public string Email { get; set; }
    
    public override bool Valida() {
        return !string.IsNullOrEmpty(Email) && Email.Contains("@");
    }
    
    public override void ProcessaPagamento(decimal importo) {
        // Logica specifica PayPal
        Console.WriteLine($"Pagamento di {importo} con PayPal");
    }
}

// Funzione che usa MetodoPagamento - funziona con qualsiasi implementazione
public void ProcessaOrdine(MetodoPagamento pagamento, decimal importo) {
    if (pagamento.Valida()) {
        pagamento.ProcessaPagamento(importo);
        // ✅ Funziona con qualsiasi sottoclasse
    }
}

// Utilizzo
ProcessaOrdine(new CartaCredito { NumeroCarta = "1234567890123456" }, 100);
ProcessaOrdine(new PayPal { Email = "user@example.com" }, 100);
// ✅ Entrambi completamente sostituibili
```

---

## 5. I - Interface Segregation Principle (ISP)

### Definizione

**I client non dovrebbero essere forzati a dipendere da interfacce che non usano.** È meglio avere molte interfacce specifiche che una interfaccia generale.

### Problema: Violazione dell'ISP

```csharp
// ❌ SBAGLIATO - Interfaccia troppo grande
public interface IWorker {
    void Work();
    void Eat();
    void Sleep();
}

// Classe umana - implementa tutto
public class Human : IWorker {
    public void Work() { Console.WriteLine("Lavora"); }
    public void Eat() { Console.WriteLine("Mangia"); }
    public void Sleep() { Console.WriteLine("Dorme"); }
}

// Robot - deve implementare metodi che non usa!
public class Robot : IWorker {
    public void Work() { Console.WriteLine("Lavora"); }
    
    // ❌ Robot non mangia!
    public void Eat() {
        throw new NotImplementedException("Robot non mangia!");
    }
    
    // ❌ Robot non dorme!
    public void Sleep() {
        throw new NotImplementedException("Robot non dorme!");
    }
}
```

### Diagramma: Violazione ISP

```
┌─────────────────────────────────────────────┐
│         IWorker (Troppo grande)             │
│  + Work()                                   │
│  + Eat()                                    │
│  + Sleep()                                  │
└─────────────────────────────────────────────┘
         ▲                    ▲
         │                    │
    ┌─────────┐          ┌─────────┐
    │  Human  │          │  Robot  │
    │  ✅ Tutti│          │  ❌ Solo Work│
    │  i metodi│          │  Eat/Sleep│
    │         │          │  non usati│
    └─────────┘          └─────────┘
```

### Soluzione: Rispetto dell'ISP

```csharp
// ✅ CORRETTO - Interfacce separate e specifiche
public interface IWorkable {
    void Work();
}

public interface IEatable {
    void Eat();
}

public interface ISleepable {
    void Sleep();
}

// Human - implementa tutte le interfacce necessarie
public class Human : IWorkable, IEatable, ISleepable {
    public void Work() { Console.WriteLine("Lavora"); }
    public void Eat() { Console.WriteLine("Mangia"); }
    public void Sleep() { Console.WriteLine("Dorme"); }
}

// Robot - implementa solo ciò che serve
public class Robot : IWorkable {
    public void Work() { Console.WriteLine("Lavora"); }
    // ✅ Non deve implementare Eat() o Sleep()
}

// Utilizzo
public void ProcessWork(IWorkable worker) {
    worker.Work();  // ✅ Funziona con Human e Robot
}

public void ProcessMeal(IEatable eater) {
    eater.Eat();  // ✅ Solo per Human
}
```

### Diagramma: Rispetto ISP

```
┌──────────────┐  ┌──────────────┐  ┌──────────────┐
│  IWorkable   │  │  IEatable    │  │ ISleepable   │
│  + Work()    │  │  + Eat()     │  │  + Sleep()   │
└──────────────┘  └──────────────┘  └──────────────┘
       ▲                ▲                  ▲
       │                │                  │
       └────────┬───────┴──────────────────┘
                │
           ┌─────────┐
           │  Human  │
           │  (tutti)│
           └─────────┘
                │
                │
           ┌─────────┐
           │  Robot  │
           │  (solo  │
           │  Work)  │
           └─────────┘
```

### Esempio Pratico: Sistema di Documenti

```csharp
// ✅ CORRETTO - Esempio documenti
public interface IReadable {
    string Read();
}

public interface IWritable {
    void Write(string content);
}

public interface IPrintable {
    void Print();
}

public interface IScannable {
    void Scan();
}

// PDF - può essere letto e stampato
public class PdfDocument : IReadable, IPrintable {
    public string Read() {
        return "Contenuto PDF";
    }
    
    public void Print() {
        Console.WriteLine("Stampa PDF");
    }
}

// Word - può essere letto, scritto e stampato
public class WordDocument : IReadable, IWritable, IPrintable {
    public string Read() {
        return "Contenuto Word";
    }
    
    public void Write(string content) {
        Console.WriteLine($"Scrive: {content}");
    }
    
    public void Print() {
        Console.WriteLine("Stampa Word");
    }
}

// Scanner - può solo scansionare
public class Scanner : IScannable {
    public void Scan() {
        Console.WriteLine("Scansiona documento");
    }
}

// Utilizzo - ogni classe implementa solo ciò che serve
List<IReadable> leggibili = new List<IReadable> {
    new PdfDocument(),
    new WordDocument()
};

List<IWritable> scrivibili = new List<IWritable> {
    new WordDocument()
};
```

---

## 6. D - Dependency Inversion Principle (DIP)

### Definizione

**I moduli di alto livello non dovrebbero dipendere da moduli di basso livello. Entrambi dovrebbero dipendere da astrazioni.** Le dipendenze dovrebbero puntare verso astrazioni, non verso concrezioni.

### Problema: Violazione del DIP

```csharp
// ❌ SBAGLIATO - Dipendenza da concrezioni
public class EmailService {
    public void InviaEmail(string messaggio) {
        // Logica di invio email
        Console.WriteLine($"Email inviata: {messaggio}");
    }
}

public class NotificationService {
    // ❌ Dipende direttamente da EmailService (concrezione)
    private EmailService emailService;
    
    public NotificationService() {
        emailService = new EmailService();  // ❌ Accoppiamento stretto
    }
    
    public void Notifica(string messaggio) {
        emailService.InviaEmail(messaggio);
    }
}

// Problema: Se voglio cambiare a SMS, devo modificare NotificationService!
```

### Diagramma: Violazione DIP

```
┌─────────────────────────────────────────────┐
│  NotificationService (alto livello)          │
│  ↓ Dipende da                                │
│  EmailService (basso livello)                │
└─────────────────────────────────────────────┘
         │
         ▼
    Accoppiamento
    stretto
    Difficile testare
    Difficile cambiare
```

### Soluzione: Rispetto del DIP

```csharp
// ✅ CORRETTO - Dipendenza da astrazioni

// Astrazione (interfaccia)
public interface IMessageService {
    void Invia(string messaggio);
}

// Implementazione concreta 1
public class EmailService : IMessageService {
    public void Invia(string messaggio) {
        Console.WriteLine($"Email inviata: {messaggio}");
    }
}

// Implementazione concreta 2
public class SmsService : IMessageService {
    public void Invia(string messaggio) {
        Console.WriteLine($"SMS inviato: {messaggio}");
    }
}

// Implementazione concreta 3
public class PushNotificationService : IMessageService {
    public void Invia(string messaggio) {
        Console.WriteLine($"Push notification inviata: {messaggio}");
    }
}

// Classe di alto livello - dipende dall'astrazione
public class NotificationService {
    // ✅ Dipende dall'interfaccia, non dall'implementazione
    private readonly IMessageService messageService;
    
    // Dependency Injection tramite costruttore
    public NotificationService(IMessageService messageService) {
        this.messageService = messageService;  // ✅ Iniettato dall'esterno
    }
    
    public void Notifica(string messaggio) {
        messageService.Invia(messaggio);
    }
}

// Utilizzo - Dependency Injection
var emailService = new EmailService();
var notificationService = new NotificationService(emailService);
notificationService.Notifica("Ciao!");

// Facile cambiare implementazione
var smsService = new SmsService();
var notificationService2 = new NotificationService(smsService);
notificationService2.Notifica("Ciao!");

// Facile testare
public class FakeMessageService : IMessageService {
    public void Invia(string messaggio) {
        // Implementazione per test
    }
}

var fakeService = new FakeMessageService();
var testService = new NotificationService(fakeService);
// ✅ Facile da testare!
```

### Diagramma: Rispetto DIP

```
┌─────────────────────────────────────────────┐
│  NotificationService (alto livello)         │
│  ↓ Dipende da                               │
│  IMessageService (astrazione)               │
└─────────────────────────────────────────────┘
                    ▲
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌──────────────┐ ┌──────────────┐ ┌──────────────┐
│EmailService  │ │ SmsService   │ │PushService   │
│(concrezione) │ │(concrezione) │ │(concrezione) │
└──────────────┘ └──────────────┘ └──────────────┘

Alto livello → Astrazione
Basso livello → Astrazione
```

### Dependency Injection (DI)

```csharp
// ✅ CORRETTO - Esempio completo con Dependency Injection

// Astrazione
public interface IRepository {
    void Save(object entity);
    object GetById(int id);
}

// Implementazione
public class DatabaseRepository : IRepository {
    public void Save(object entity) {
        // Salva nel database
    }
    
    public object GetById(int id) {
        // Carica dal database
        return new object();
    }
}

// Altre implementazioni possibili
public class FileRepository : IRepository {
    public void Save(object entity) {
        // Salva su file
    }
    
    public object GetById(int id) {
        // Carica da file
        return new object();
    }
}

// Classe di alto livello - usa l'astrazione
public class BusinessService {
    private readonly IRepository repository;
    
    // Dependency Injection tramite costruttore
    public BusinessService(IRepository repository) {
        this.repository = repository;
    }
    
    public void ProcessData(object data) {
        // Usa repository senza sapere quale implementazione
        repository.Save(data);
    }
}

// Utilizzo con Dependency Injection
var dbRepo = new DatabaseRepository();
var businessService = new BusinessService(dbRepo);

// Facile cambiare implementazione
var fileRepo = new FileRepository();
var businessService2 = new BusinessService(fileRepo);

// Facile testare
public class MockRepository : IRepository {
    public void Save(object entity) {
        // Mock per test
    }
    
    public object GetById(int id) {
        return new object();
    }
}

var mockRepo = new MockRepository();
var testService = new BusinessService(mockRepo);
// ✅ Facile da testare!
```

---

## 7. Riepilogo: I Cinque Principi SOLID

### Tabella Riepilogo

| Principio | Acronimo | Definizione | Beneficio |
|-----------|----------|-------------|-----------|
| **Single Responsibility** | S | Una classe, una responsabilità | Manutenibilità |
| **Open/Closed** | O | Aperto per estensione, chiuso per modifica | Estensibilità |
| **Liskov Substitution** | L | Sottoclassi sostituibili | Coerenza |
| **Interface Segregation** | I | Interfacce piccole e specifiche | Flessibilità |
| **Dependency Inversion** | D | Dipendi da astrazioni | Testabilità |

### Diagramma Completo: SOLID

```
┌─────────────────────────────────────────────────┐
│                  SOLID                         │
└─────────────────────────────────────────────────┘
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│     S     │ │     O     │ │     L     │
│  Una      │ │  Estendi   │ │ Sostituisci│
│  respons. │ │  non modif│ │  sempre   │
└───────────┘ └───────────┘ └───────────┘
        │           │           │
        └───────────┼───────────┘
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐
│     I     │ │     D     │
│  Interfacce│ │  Dipendi da │
│  piccole  │ │  astrazioni │
└───────────┘ └───────────┘
```

---

## 8. Esempio Pratico Completo: Sistema di E-Commerce

```csharp
// ✅ CORRETTO - Esempio completo che rispetta tutti i principi SOLID

// ========== SRP: Classi con una sola responsabilità ==========

// Dati
public class Ordine {
    public int Id { get; set; }
    public decimal Totale { get; set; }
    public List<Prodotto> Prodotti { get; set; }
}

// Validazione (SRP)
public class OrdineValidator {
    public bool Valida(Ordine ordine) {
        return ordine != null && 
               ordine.Prodotti != null && 
               ordine.Prodotti.Any();
    }
}

// Calcolo (SRP)
public class CalcolatoreTotale {
    public decimal Calcola(List<Prodotto> prodotti) {
        return prodotti.Sum(p => p.Prezzo * p.Quantita);
    }
}

// ========== OCP: Estendibile senza modifiche ==========

// Astrazione per sconti (OCP)
public interface ISconto {
    decimal Applica(decimal prezzo);
}

public class NessunoSconto : ISconto {
    public decimal Applica(decimal prezzo) => prezzo;
}

public class ScontoPercentuale : ISconto {
    private decimal percentuale;
    public ScontoPercentuale(decimal percentuale) {
        this.percentuale = percentuale;
    }
    public decimal Applica(decimal prezzo) => prezzo * (1 - percentuale / 100);
}

// Nuovo sconto aggiunto senza modificare codice esistente
public class ScontoFisso : ISconto {
    private decimal importo;
    public ScontoFisso(decimal importo) => this.importo = importo;
    public decimal Applica(decimal prezzo) => Math.Max(0, prezzo - importo);
}

// ========== LSP: Sottoclassi sostituibili ==========

// Astrazione pagamento (LSP)
public interface IMetodoPagamento {
    void Processa(decimal importo);
}

public class CartaCredito : IMetodoPagamento {
    public void Processa(decimal importo) {
        Console.WriteLine($"Pagamento carta: {importo}");
    }
}

public class PayPal : IMetodoPagamento {
    public void Processa(decimal importo) {
        Console.WriteLine($"Pagamento PayPal: {importo}");
    }
}

// ========== ISP: Interfacce specifiche ==========

public interface IReadable {
    string Read();
}

public interface IWritable {
    void Write(string content);
}

public class ReportGenerator : IReadable, IWritable {
    public string Read() => "Contenuto report";
    public void Write(string content) => Console.WriteLine($"Scrive: {content}");
}

// ========== DIP: Dipendenza da astrazioni ==========

// Astrazione repository (DIP)
public interface IOrdineRepository {
    void Salva(Ordine ordine);
    Ordine Carica(int id);
}

public class DatabaseRepository : IOrdineRepository {
    public void Salva(Ordine ordine) {
        Console.WriteLine("Salva nel database");
    }
    public Ordine Carica(int id) => new Ordine();
}

// Service di alto livello (DIP)
public class OrdineService {
    private readonly IOrdineRepository repository;
    private readonly OrdineValidator validator;
    private readonly CalcolatoreTotale calculator;
    
    // Dependency Injection
    public OrdineService(
        IOrdineRepository repository,
        OrdineValidator validator,
        CalcolatoreTotale calculator) {
        this.repository = repository;
        this.validator = validator;
        this.calculator = calculator;
    }
    
    public void CreaOrdine(Ordine ordine, ISconto sconto, IMetodoPagamento pagamento) {
        if (validator.Valida(ordine)) {
            ordine.Totale = calculator.Calcola(ordine.Prodotti);
            ordine.Totale = sconto.Applica(ordine.Totale);
            repository.Salva(ordine);
            pagamento.Processa(ordine.Totale);
        }
    }
}

// Utilizzo
var repository = new DatabaseRepository();
var validator = new OrdineValidator();
var calculator = new CalcolatoreTotale();
var service = new OrdineService(repository, validator, calculator);

var ordine = new Ordine {
    Prodotti = new List<Prodotto> { /* ... */ }
};

var sconto = new ScontoPercentuale(10);
var pagamento = new CartaCredito();

service.CreaOrdine(ordine, sconto, pagamento);
```

---

## 9. Best Practices SOLID

### ✅ Cosa Fare

1. **Mantieni le classi piccole e focalizzate** (SRP)
2. **Usa interfacce per estendere funzionalità** (OCP)
3. **Assicurati che le sottoclassi siano sostituibili** (LSP)
4. **Crea interfacce piccole e specifiche** (ISP)
5. **Dipendi da astrazioni, non da concrezioni** (DIP)

### ❌ Cosa Evitare

1. **Non creare classi "God Object"** (SRP)
2. **Non usare if/else per ogni nuovo tipo** (OCP)
3. **Non cambiare il comportamento previsto** (LSP)
4. **Non creare interfacce troppo grandi** (ISP)
5. **Non dipendere direttamente da classi concrete** (DIP)

---

## 10. Domande Frequenti (FAQ)

### Q: I principi SOLID sono sempre applicabili?
**R:** Sì, ma con giudizio. A volte per progetti piccoli può essere over-engineering. Per progetti grandi sono essenziali.

### Q: Come posso applicare SOLID in progetti esistenti?
**R:** Refactoring graduale. Identifica violazioni, estrai interfacce, applica Dependency Injection.

### Q: SOLID è solo per C#?
**R:** No, SOLID è applicabile a qualsiasi linguaggio orientato agli oggetti.

### Q: Qual è il principio più importante?
**R:** Tutti sono importanti, ma SRP è spesso il punto di partenza, e DIP è cruciale per la testabilità.

---

## Conclusioni

I principi SOLID sono fondamentali per:

- ✅ Scrivere codice manutenibile
- ✅ Creare sistemi estendibili
- ✅ Facilitare i test
- ✅ Ridurre l'accoppiamento
- ✅ Aumentare la coesione

Applicare SOLID rende il codice più pulito, professionale e facile da mantenere nel tempo.

---

*Documento creato per spiegare i principi SOLID in C# con esempi pratici e diagrammi.*

