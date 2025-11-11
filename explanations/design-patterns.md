# Design Patterns in C#

## Introduzione

I **Design Patterns** sono soluzioni riutilizzabili a problemi comuni nella progettazione software. Rappresentano best practices consolidate che aiutano a scrivere codice più pulito, manutenibile e scalabile.

---

## 1. Categorie di Design Patterns

### Classificazione

```
┌─────────────────────────────────────────────────┐
│           DESIGN PATTERNS                       │
└─────────────────────────────────────────────────┘
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│ CREAZIONALI│ │ STRUTTURALI│ │COMPORTAMENTALI│
│            │ │            │ │            │
│ Singleton  │ │ Adapter    │ │ Observer   │
│ Factory    │ │ Decorator  │ │ Strategy   │
│ Builder    │ │ Facade     │ │ Command    │
│ Prototype  │ │ Proxy      │ │ Chain of   │
│            │ │            │ │ Responsibility│
└───────────┘ └───────────┘ └───────────┘
```

---

## 2. Singleton Pattern

### Definizione

Il **Singleton Pattern** garantisce che una classe abbia una sola istanza e fornisce un punto di accesso globale a essa.

### Problema

```csharp
// ❌ PROBLEMA: Più istanze possono essere create
public class DatabaseConnection {
    public void Connect() {
        Console.WriteLine("Connesso al database");
    }
}

// Utilizzo
var db1 = new DatabaseConnection();
var db2 = new DatabaseConnection();
// Due connessioni separate - spreco di risorse!
```

### Soluzione: Singleton Pattern

```csharp
// ✅ SOLUZIONE: Una sola istanza
public class DatabaseConnection {
    // Istanza privata statica
    private static DatabaseConnection _instance;
    
    // Costruttore privato - previene creazione esterna
    private DatabaseConnection() {
        Console.WriteLine("Istanza DatabaseConnection creata");
    }
    
    // Metodo pubblico statico per ottenere l'istanza
    public static DatabaseConnection GetInstance() {
        if (_instance == null) {
            _instance = new DatabaseConnection();
        }
        return _instance;
    }
    
    public void Connect() {
        Console.WriteLine("Connesso al database");
    }
}

// Utilizzo
var db1 = DatabaseConnection.GetInstance();
var db2 = DatabaseConnection.GetInstance();
// db1 e db2 sono la STESSA istanza!
Console.WriteLine(ReferenceEquals(db1, db2)); // ✅ true
```

### Diagramma: Singleton

```
┌─────────────────────────────────────────────┐
│  DatabaseConnection                         │
├─────────────────────────────────────────────┤
│  - _instance: DatabaseConnection (static)   │
│  - DatabaseConnection() (private)           │
│  + GetInstance(): DatabaseConnection        │
│  + Connect()                                │
└─────────────────────────────────────────────┘
                    │
                    │ GetInstance()
                    ▼
        ┌───────────────────────┐
        │  Istanza Unica        │
        │  (condivisa)          │
        └───────────────────────┘
```

### Singleton Thread-Safe

```csharp
// ✅ Singleton thread-safe con lock
public class DatabaseConnection {
    private static DatabaseConnection _instance;
    private static readonly object _lock = new object();
    
    private DatabaseConnection() { }
    
    public static DatabaseConnection GetInstance() {
        // Double-check locking pattern
        if (_instance == null) {
            lock (_lock) {
                if (_instance == null) {
                    _instance = new DatabaseConnection();
                }
            }
        }
        return _instance;
    }
}
```

### Singleton con Lazy<T>

```csharp
// ✅ Singleton thread-safe con Lazy<T> (raccomandato)
public class DatabaseConnection {
    private static readonly Lazy<DatabaseConnection> _instance = 
        new Lazy<DatabaseConnection>(() => new DatabaseConnection());
    
    private DatabaseConnection() { }
    
    public static DatabaseConnection GetInstance() {
        return _instance.Value;
    }
}
```

### Quando Usare Singleton

✅ **Quando usare:**
- Una sola istanza necessaria (logger, configurazione, cache)
- Accesso globale controllato
- Risorse condivise costose da inizializzare

❌ **Quando evitare:**
- Quando serve testabilità (difficile mockare)
- Quando serve multithreading complesso
- Quando viola Single Responsibility Principle

---

## 3. Factory Pattern

### Definizione

Il **Factory Pattern** fornisce un'interfaccia per creare oggetti senza specificare la loro classe esatta. Delega la creazione alle sottoclassi.

### Problema

```csharp
// ❌ PROBLEMA: Creazione diretta accoppiata
public class PaymentProcessor {
    public void ProcessPayment(string paymentType) {
        if (paymentType == "CreditCard") {
            var payment = new CreditCardPayment();
            payment.Process();
        }
        else if (paymentType == "PayPal") {
            var payment = new PayPalPayment();
            payment.Process();
        }
        // Difficile aggiungere nuovi tipi!
    }
}
```

### Soluzione: Factory Pattern

```csharp
// Interfaccia comune
public interface IPayment {
    void Process();
}

// Implementazioni concrete
public class CreditCardPayment : IPayment {
    public void Process() {
        Console.WriteLine("Processando pagamento con carta di credito");
    }
}

public class PayPalPayment : IPayment {
    public void Process() {
        Console.WriteLine("Processando pagamento con PayPal");
    }
}

// Factory
public class PaymentFactory {
    public static IPayment CreatePayment(string paymentType) {
        return paymentType.ToLower() switch {
            "creditcard" => new CreditCardPayment(),
            "paypal" => new PayPalPayment(),
            _ => throw new ArgumentException($"Tipo di pagamento non supportato: {paymentType}")
        };
    }
}

// Utilizzo
var payment = PaymentFactory.CreatePayment("CreditCard");
payment.Process();
```

### Diagramma: Factory Pattern

```
┌─────────────────────────────────────────────┐
│  PaymentFactory                             │
│  + CreatePayment(): IPayment                │
└─────────────────────────────────────────────┘
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│CreditCard │ │  PayPal   │ │  Bank     │
│ Payment   │ │  Payment  │ │  Transfer │
└───────────┘ └───────────┘ └───────────┘
        │           │           │
        └───────────┼───────────┘
                    │
                    ▼
            ┌───────────┐
            │ IPayment  │
            └───────────┘
```

### Factory Pattern con Enum

```csharp
public enum PaymentType {
    CreditCard,
    PayPal,
    BankTransfer
}

public class PaymentFactory {
    public static IPayment CreatePayment(PaymentType type) {
        return type switch {
            PaymentType.CreditCard => new CreditCardPayment(),
            PaymentType.PayPal => new PayPalPayment(),
            PaymentType.BankTransfer => new BankTransferPayment(),
            _ => throw new ArgumentException("Tipo non supportato")
        };
    }
}
```

---

## 4. Observer Pattern

### Definizione

Il **Observer Pattern** definisce una dipendenza uno-a-molti tra oggetti, in modo che quando un oggetto cambia stato, tutti i suoi dipendenti vengono notificati e aggiornati automaticamente.

### Problema

```csharp
// ❌ PROBLEMA: Accoppiamento stretto
public class Stock {
    private decimal _price;
    
    public void SetPrice(decimal price) {
        _price = price;
        // Deve notificare manualmente ogni subscriber
        // Difficile aggiungere nuovi subscriber!
    }
}
```

### Soluzione: Observer Pattern

```csharp
// Interfaccia Observer
public interface IObserver {
    void Update(decimal price);
}

// Interfaccia Subject
public interface ISubject {
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify();
}

// Subject (osservato)
public class Stock : ISubject {
    private decimal _price;
    private List<IObserver> _observers = new List<IObserver>();
    
    public decimal Price {
        get => _price;
        set {
            _price = value;
            Notify(); // Notifica tutti gli observer
        }
    }
    
    public void Attach(IObserver observer) {
        _observers.Add(observer);
    }
    
    public void Detach(IObserver observer) {
        _observers.Remove(observer);
    }
    
    public void Notify() {
        foreach (var observer in _observers) {
            observer.Update(_price);
        }
    }
}

// Observer concreti
public class EmailNotifier : IObserver {
    public void Update(decimal price) {
        Console.WriteLine($"Email: Prezzo aggiornato a {price:C}");
    }
}

public class SMSNotifier : IObserver {
    public void Update(decimal price) {
        Console.WriteLine($"SMS: Prezzo aggiornato a {price:C}");
    }
}

// Utilizzo
var stock = new Stock();
stock.Attach(new EmailNotifier());
stock.Attach(new SMSNotifier());

stock.Price = 100.50m; // Notifica automaticamente entrambi!
```

### Diagramma: Observer Pattern

```
┌─────────────────────────────────────────────┐
│  Stock (Subject)                            │
│  - Price                                    │
│  - _observers: List<IObserver>             │
│  + Attach(IObserver)                       │
│  + Detach(IObserver)                       │
│  + Notify()                                 │
└─────────────────────────────────────────────┘
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│Email      │ │   SMS     │ │  Logger   │
│Notifier   │ │ Notifier  │ │ Observer  │
│           │ │           │ │           │
│+ Update() │ │+ Update() │ │+ Update() │
└───────────┘ └───────────┘ └───────────┘
```

### Observer Pattern con Eventi C#

```csharp
// ✅ Versione moderna con eventi C#
public class Stock {
    private decimal _price;
    
    public event Action<decimal> PriceChanged;
    
    public decimal Price {
        get => _price;
        set {
            _price = value;
            PriceChanged?.Invoke(_price);
        }
    }
}

// Utilizzo
var stock = new Stock();
stock.PriceChanged += (price) => Console.WriteLine($"Prezzo: {price}");
stock.Price = 100.50m; // Trigger automatico!
```

---

## 5. Strategy Pattern

### Definizione

Il **Strategy Pattern** definisce una famiglia di algoritmi, li incapsula e li rende intercambiabili. Permette di variare l'algoritmo indipendentemente dai client che lo utilizzano.

### Problema

```csharp
// ❌ PROBLEMA: Logica condizionale complessa
public class OrderProcessor {
    public void ProcessOrder(Order order, string discountType) {
        decimal total = order.Total;
        
        if (discountType == "Student") {
            total *= 0.9m; // 10% sconto
        }
        else if (discountType == "Senior") {
            total *= 0.85m; // 15% sconto
        }
        else if (discountType == "VIP") {
            total *= 0.8m; // 20% sconto
        }
        // Difficile aggiungere nuovi sconti!
    }
}
```

### Soluzione: Strategy Pattern

```csharp
// Interfaccia Strategy
public interface IDiscountStrategy {
    decimal ApplyDiscount(decimal amount);
}

// Strategie concrete
public class StudentDiscount : IDiscountStrategy {
    public decimal ApplyDiscount(decimal amount) {
        return amount * 0.9m; // 10% sconto
    }
}

public class SeniorDiscount : IDiscountStrategy {
    public decimal ApplyDiscount(decimal amount) {
        return amount * 0.85m; // 15% sconto
    }
}

public class VIPDiscount : IDiscountStrategy {
    public decimal ApplyDiscount(decimal amount) {
        return amount * 0.8m; // 20% sconto
    }
}

public class NoDiscount : IDiscountStrategy {
    public decimal ApplyDiscount(decimal amount) {
        return amount; // Nessuno sconto
    }
}

// Context
public class OrderProcessor {
    private IDiscountStrategy _discountStrategy;
    
    public OrderProcessor(IDiscountStrategy discountStrategy) {
        _discountStrategy = discountStrategy;
    }
    
    public void SetDiscountStrategy(IDiscountStrategy strategy) {
        _discountStrategy = strategy;
    }
    
    public decimal ProcessOrder(Order order) {
        return _discountStrategy.ApplyDiscount(order.Total);
    }
}

// Utilizzo
var order = new Order { Total = 100 };
var processor = new OrderProcessor(new StudentDiscount());
decimal finalPrice = processor.ProcessOrder(order); // 90

// Cambio strategia a runtime
processor.SetDiscountStrategy(new VIPDiscount());
finalPrice = processor.ProcessOrder(order); // 80
```

### Diagramma: Strategy Pattern

```
┌─────────────────────────────────────────────┐
│  OrderProcessor (Context)                    │
│  - _discountStrategy: IDiscountStrategy     │
│  + ProcessOrder(): decimal                  │
│  + SetDiscountStrategy()                    │
└─────────────────────────────────────────────┘
                    │
                    │ usa
                    ▼
        ┌───────────────────────┐
        │ IDiscountStrategy     │
        │ + ApplyDiscount()     │
        └───────────────────────┘
                    ▲
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│ Student   │ │  Senior   │ │   VIP     │
│ Discount  │ │ Discount  │ │ Discount  │
└───────────┘ └───────────┘ └───────────┘
```

---

## 6. Builder Pattern

### Definizione

Il **Builder Pattern** separa la costruzione di un oggetto complesso dalla sua rappresentazione, permettendo la stessa costruzione di creare rappresentazioni diverse.

### Problema

```csharp
// ❌ PROBLEMA: Costruttore con molti parametri
public class Pizza {
    public string Dough { get; set; }
    public string Sauce { get; set; }
    public string Cheese { get; set; }
    public List<string> Toppings { get; set; }
    
    public Pizza(string dough, string sauce, string cheese, List<string> toppings) {
        // Costruttore complesso e difficile da usare!
    }
}

// Utilizzo confuso
var pizza = new Pizza("thick", "tomato", "mozzarella", 
    new List<string> { "pepperoni", "mushrooms" });
```

### Soluzione: Builder Pattern

```csharp
public class Pizza {
    public string Dough { get; set; }
    public string Sauce { get; set; }
    public string Cheese { get; set; }
    public List<string> Toppings { get; set; } = new List<string>();
}

public class PizzaBuilder {
    private Pizza _pizza = new Pizza();
    
    public PizzaBuilder WithDough(string dough) {
        _pizza.Dough = dough;
        return this; // Fluent interface
    }
    
    public PizzaBuilder WithSauce(string sauce) {
        _pizza.Sauce = sauce;
        return this;
    }
    
    public PizzaBuilder WithCheese(string cheese) {
        _pizza.Cheese = cheese;
        return this;
    }
    
    public PizzaBuilder AddTopping(string topping) {
        _pizza.Toppings.Add(topping);
        return this;
    }
    
    public Pizza Build() {
        return _pizza;
    }
}

// Utilizzo (fluent e leggibile)
var pizza = new PizzaBuilder()
    .WithDough("thick")
    .WithSauce("tomato")
    .WithCheese("mozzarella")
    .AddTopping("pepperoni")
    .AddTopping("mushrooms")
    .Build();
```

### Diagramma: Builder Pattern

```
┌─────────────────────────────────────────────┐
│  PizzaBuilder                                │
│  - _pizza: Pizza                             │
│  + WithDough(): PizzaBuilder                 │
│  + WithSauce(): PizzaBuilder                 │
│  + WithCheese(): PizzaBuilder                │
│  + AddTopping(): PizzaBuilder                │
│  + Build(): Pizza                            │
└─────────────────────────────────────────────┘
                    │
                    │ Build()
                    ▼
        ┌───────────────────────┐
        │  Pizza                │
        │  - Dough              │
        │  - Sauce              │
        │  - Cheese             │
        │  - Toppings           │
        └───────────────────────┘
```

---

## 7. Adapter Pattern

### Definizione

Il **Adapter Pattern** permette a classi con interfacce incompatibili di lavorare insieme, convertendo l'interfaccia di una classe in un'interfaccia attesa dal client.

### Problema

```csharp
// Sistema esistente
public class LegacyPaymentSystem {
    public void ProcessPaymentLegacy(decimal amount, string currency) {
        Console.WriteLine($"Processing {amount} {currency} (Legacy)");
    }
}

// Nuovo sistema che si aspetta un'interfaccia diversa
public interface IPaymentProcessor {
    void ProcessPayment(PaymentRequest request);
}

public class PaymentRequest {
    public decimal Amount { get; set; }
    public string Currency { get; set; }
}
```

### Soluzione: Adapter Pattern

```csharp
// Adapter che adatta LegacyPaymentSystem a IPaymentProcessor
public class LegacyPaymentAdapter : IPaymentProcessor {
    private LegacyPaymentSystem _legacySystem;
    
    public LegacyPaymentAdapter(LegacyPaymentSystem legacySystem) {
        _legacySystem = legacySystem;
    }
    
    public void ProcessPayment(PaymentRequest request) {
        // Adatta la nuova interfaccia alla vecchia
        _legacySystem.ProcessPaymentLegacy(request.Amount, request.Currency);
    }
}

// Utilizzo
var legacySystem = new LegacyPaymentSystem();
var adapter = new LegacyPaymentAdapter(legacySystem);
var request = new PaymentRequest { Amount = 100, Currency = "EUR" };
adapter.ProcessPayment(request); // Funziona con il nuovo sistema!
```

### Diagramma: Adapter Pattern

```
┌─────────────────────────────────────────────┐
│  Client                                     │
│  usa IPaymentProcessor                      │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │ IPaymentProcessor     │
        │ + ProcessPayment()    │
        └───────────────────────┘
                    ▲
                    │ implementa
                    │
        ┌───────────────────────┐
        │ LegacyPaymentAdapter  │
        │ - _legacySystem       │
        │ + ProcessPayment()    │
        └───────────────────────┘
                    │
                    │ usa
                    ▼
        ┌───────────────────────┐
        │ LegacyPaymentSystem   │
        │ + ProcessPaymentLegacy()│
        └───────────────────────┘
```

---

## 8. Decorator Pattern

### Definizione

Il **Decorator Pattern** permette di aggiungere comportamenti a oggetti dinamicamente, senza alterare la loro struttura. Fornisce un'alternativa flessibile all'ereditarietà.

### Problema

```csharp
// ❌ PROBLEMA: Ereditarietà rigida
public abstract class Coffee {
    public abstract decimal Cost();
}

public class SimpleCoffee : Coffee {
    public override decimal Cost() => 2.0m;
}

public class CoffeeWithMilk : SimpleCoffee {
    public override decimal Cost() => base.Cost() + 0.5m;
}

// Cosa succede se voglio caffè con latte E zucchero?
// Devo creare CoffeeWithMilkAndSugar...
```

### Soluzione: Decorator Pattern

```csharp
// Component
public interface ICoffee {
    decimal Cost();
    string Description { get; }
}

// Concrete Component
public class SimpleCoffee : ICoffee {
    public decimal Cost() => 2.0m;
    public string Description => "Caffè semplice";
}

// Base Decorator
public abstract class CoffeeDecorator : ICoffee {
    protected ICoffee _coffee;
    
    public CoffeeDecorator(ICoffee coffee) {
        _coffee = coffee;
    }
    
    public virtual decimal Cost() => _coffee.Cost();
    public virtual string Description => _coffee.Description;
}

// Concrete Decorators
public class MilkDecorator : CoffeeDecorator {
    public MilkDecorator(ICoffee coffee) : base(coffee) { }
    
    public override decimal Cost() => base.Cost() + 0.5m;
    public override string Description => base.Description + ", Latte";
}

public class SugarDecorator : CoffeeDecorator {
    public SugarDecorator(ICoffee coffee) : base(coffee) { }
    
    public override decimal Cost() => base.Cost() + 0.2m;
    public override string Description => base.Description + ", Zucchero";
}

// Utilizzo (composizione flessibile)
ICoffee coffee = new SimpleCoffee();
coffee = new MilkDecorator(coffee);
coffee = new SugarDecorator(coffee);

Console.WriteLine($"{coffee.Description}: {coffee.Cost():C}");
// Output: Caffè semplice, Latte, Zucchero: €2.70
```

### Diagramma: Decorator Pattern

```
┌─────────────────────────────────────────────┐
│  ICoffee                                    │
│  + Cost(): decimal                         │
│  + Description: string                      │
└─────────────────────────────────────────────┘
        ▲                    ▲
        │                    │
        │                    │
┌───────────────┐    ┌───────────────────────┐
│ SimpleCoffee  │    │ CoffeeDecorator       │
│ (concrete)    │    │ (abstract)            │
└───────────────┘    │ - _coffee: ICoffee    │
                     └───────────────────────┘
                                    ▲
                    ┌───────────────┼───────────────┐
                    │               │               │
                    ▼               ▼               ▼
            ┌───────────┐   ┌───────────┐   ┌───────────┐
            │   Milk    │   │  Sugar    │   │  Cream    │
            │ Decorator │   │ Decorator │   │ Decorator │
            └───────────┘   └───────────┘   └───────────┘
```

---

## 9. Confronto Pattern: Quando Usare Quale?

### Tabella Riepilogativa

| Pattern | Scopo | Quando Usare | Esempio |
|---------|-------|--------------|---------|
| **Singleton** | Una sola istanza | Logger, Configurazione, Cache | `DatabaseConnection.GetInstance()` |
| **Factory** | Creazione oggetti | Creazione complessa, dipendenza da tipo | `PaymentFactory.CreatePayment()` |
| **Observer** | Notifiche uno-a-molti | Eventi, notifiche, MVC | `Stock.PriceChanged` event |
| **Strategy** | Algoritmi intercambiabili | Multiple varianti di algoritmo | `DiscountStrategy` |
| **Builder** | Costruzione complessa | Oggetti con molti parametri | `PizzaBuilder` |
| **Adapter** | Compatibilità interfacce | Integrazione sistemi legacy | `LegacyPaymentAdapter` |
| **Decorator** | Aggiunta comportamenti | Estensioni dinamiche | `CoffeeDecorator` |

---

## 10. Best Practices

### ✅ Cosa Fare

1. **Usa pattern quando necessario**
   ```csharp
   // ✅ Usa Singleton per logger
   var logger = Logger.GetInstance();
   ```

2. **Preferisci composizione a ereditarietà**
   ```csharp
   // ✅ Decorator Pattern
   ICoffee coffee = new MilkDecorator(new SimpleCoffee());
   ```

3. **Usa interfacce per flessibilità**
   ```csharp
   // ✅ Strategy Pattern con interfaccia
   IDiscountStrategy strategy = new StudentDiscount();
   ```

### ❌ Cosa Evitare

1. **Non sovra-ingegnerizzare**
   ```csharp
   // ❌ Pattern non necessario per casi semplici
   var simple = new SimpleFactory().CreateSimpleObject();
   ```

2. **Non usare Singleton per tutto**
   ```csharp
   // ❌ Singleton non necessario qui
   var user = User.GetInstance(); // SBAGLIATO!
   ```

---

## 11. Domande Frequenti (FAQ)

### Q: Qual è la differenza tra Factory e Abstract Factory?
**R:** Factory crea un tipo di oggetto. Abstract Factory crea famiglie di oggetti correlati.

### Q: Singleton è thread-safe?
**R:** La versione base no. Usa `Lazy<T>` o `lock` per thread-safety.

### Q: Quando usare Strategy vs Template Method?
**R:** Strategy usa composizione (intercambiabile a runtime). Template Method usa ereditarietà (definito a compile-time).

### Q: Observer vs Eventi C#?
**R:** Gli eventi C# implementano il pattern Observer in modo nativo e più semplice.

---

## Conclusioni

I Design Patterns sono strumenti potenti per:
- ✅ Scrivere codice più pulito e manutenibile
- ✅ Risolvere problemi comuni in modo standardizzato
- ✅ Migliorare la comunicazione tra sviluppatori
- ✅ Facilitare testabilità e manutenzione

Ricorda: i pattern sono strumenti, non obiettivi. Usali quando aggiungono valore, non per complessità inutile.

---

_Documento creato per spiegare i Design Patterns più importanti con esempi pratici in C#._

