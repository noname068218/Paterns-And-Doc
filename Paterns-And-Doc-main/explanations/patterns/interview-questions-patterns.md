# Domande e Risposte per Colloqui - Design Patterns

## 1. Spiega il Singleton Pattern.

**Risposta:**
Garantisce una sola istanza di una classe.

```csharp
public class DatabaseConnection
{
    private static DatabaseConnection _instance;
    private static readonly object _lock = new object();
    
    private DatabaseConnection() { }
    
    public static DatabaseConnection GetInstance()
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new DatabaseConnection();
                }
            }
        }
        return _instance;
    }
}
```

**Quando usare:**
- Logger
- Database connections
- Configuration

---

## 2. Spiega il Factory Pattern.

**Risposta:**
Crea oggetti senza specificare la classe esatta.

```csharp
public interface IAnimal
{
    void MakeSound();
}

public class Dog : IAnimal
{
    public void MakeSound() => Console.WriteLine("Woof!");
}

public class Cat : IAnimal
{
    public void MakeSound() => Console.WriteLine("Meow!");
}

public class AnimalFactory
{
    public static IAnimal CreateAnimal(string type)
    {
        return type.ToLower() switch
        {
            "dog" => new Dog(),
            "cat" => new Cat(),
            _ => throw new ArgumentException("Unknown animal type")
        };
    }
}
```

---

## 3. Spiega il Repository Pattern.

**Risposta:**
Astrae l'accesso ai dati.

```csharp
public interface IRepository<T>
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
}

public class UserRepository : IRepository<User>
{
    private readonly DbContext _context;
    
    public UserRepository(DbContext context)
    {
        _context = context;
    }
    
    public User GetById(int id) => _context.Users.Find(id);
    // ... altri metodi
}
```

**Vantaggi:**
- Separazione delle responsabilità
- Testabilità
- Flessibilità (cambiare data source)

---

## 4. Spiega il Strategy Pattern.

**Risposta:**
Definisce una famiglia di algoritmi intercambiabili.

```csharp
public interface IPaymentStrategy
{
    void Pay(decimal amount);
}

public class CreditCardPayment : IPaymentStrategy
{
    public void Pay(decimal amount) => Console.WriteLine($"Paid {amount} with credit card");
}

public class PayPalPayment : IPaymentStrategy
{
    public void Pay(decimal amount) => Console.WriteLine($"Paid {amount} with PayPal");
}

public class PaymentProcessor
{
    private IPaymentStrategy _strategy;
    
    public void SetStrategy(IPaymentStrategy strategy) => _strategy = strategy;
    public void ProcessPayment(decimal amount) => _strategy.Pay(amount);
}
```

---

## 5. Spiega l'Observer Pattern.

**Risposta:**
Notifica multiple dipendenze quando un oggetto cambia stato.

```csharp
public interface IObserver
{
    void Update(string message);
}

public class Subject
{
    private List<IObserver> _observers = new();
    
    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Notify(string message) => _observers.ForEach(o => o.Update(message));
}

// In C#: Events
public class Button
{
    public event EventHandler Clicked;
    public void Click() => Clicked?.Invoke(this, EventArgs.Empty);
}
```

---

## 6. Spiega il Decorator Pattern.

**Risposta:**
Aggiunge funzionalità a oggetti dinamicamente.

```csharp
public interface ICoffee
{
    string GetDescription();
    decimal GetCost();
}

public class SimpleCoffee : ICoffee
{
    public string GetDescription() => "Simple Coffee";
    public decimal GetCost() => 2.0m;
}

public class MilkDecorator : ICoffee
{
    private readonly ICoffee _coffee;
    
    public MilkDecorator(ICoffee coffee) => _coffee = coffee;
    
    public string GetDescription() => _coffee.GetDescription() + ", Milk";
    public decimal GetCost() => _coffee.GetCost() + 0.5m;
}
```

---

## 7. Spiega il Adapter Pattern.

**Risposta:**
Permette a classi incompatibili di lavorare insieme.

```csharp
// Interfaccia target
public interface ITarget
{
    string Request();
}

// Classe da adattare
public class Adaptee
{
    public string SpecificRequest() => "Specific request";
}

// Adapter
public class Adapter : ITarget
{
    private readonly Adaptee _adaptee;
    
    public Adapter(Adaptee adaptee) => _adaptee = adaptee;
    
    public string Request() => _adaptee.SpecificRequest();
}
```

---

## 8. Spiega il Command Pattern.

**Risposta:**
Incapsula una richiesta come oggetto.

```csharp
public interface ICommand
{
    void Execute();
    void Undo();
}

public class LightOnCommand : ICommand
{
    private readonly Light _light;
    
    public LightOnCommand(Light light) => _light = light;
    
    public void Execute() => _light.TurnOn();
    public void Undo() => _light.TurnOff();
}

public class RemoteControl
{
    private ICommand _command;
    
    public void SetCommand(ICommand command) => _command = command;
    public void PressButton() => _command?.Execute();
}
```

---

## 9. Spiega il Facade Pattern.

**Risposta:**
Fornisce un'interfaccia semplificata a un sistema complesso.

```csharp
public class OrderFacade
{
    private readonly InventoryService _inventory;
    private readonly PaymentService _payment;
    private readonly ShippingService _shipping;
    
    public void PlaceOrder(Order order)
    {
        _inventory.CheckStock(order);
        _payment.ProcessPayment(order);
        _shipping.ShipOrder(order);
    }
}
```

---

## 10. Spiega il Builder Pattern.

**Risposta:**
Costruisce oggetti complessi passo per passo.

```csharp
public class Pizza
{
    public string Dough { get; set; }
    public string Sauce { get; set; }
    public List<string> Toppings { get; set; } = new();
}

public class PizzaBuilder
{
    private Pizza _pizza = new();
    
    public PizzaBuilder WithDough(string dough)
    {
        _pizza.Dough = dough;
        return this;
    }
    
    public PizzaBuilder WithSauce(string sauce)
    {
        _pizza.Sauce = sauce;
        return this;
    }
    
    public PizzaBuilder AddTopping(string topping)
    {
        _pizza.Toppings.Add(topping);
        return this;
    }
    
    public Pizza Build() => _pizza;
}

// Utilizzo
var pizza = new PizzaBuilder()
    .WithDough("Thin")
    .WithSauce("Tomato")
    .AddTopping("Cheese")
    .AddTopping("Pepperoni")
    .Build();
```

---

## 11. Spiega il Template Method Pattern.

**Risposta:**
Definisce lo scheletro di un algoritmo, lasciando alcuni passi alle sottoclassi.

```csharp
public abstract class DataProcessor
{
    public void Process()
    {
        ReadData();
        ProcessData();
        SaveData();
    }
    
    protected abstract void ReadData();
    protected abstract void ProcessData();
    protected virtual void SaveData() => Console.WriteLine("Saving data...");
}

public class XmlDataProcessor : DataProcessor
{
    protected override void ReadData() => Console.WriteLine("Reading XML");
    protected override void ProcessData() => Console.WriteLine("Processing XML");
}
```

---

## 12. Spiega il Chain of Responsibility Pattern.

**Risposta:**
Passa richieste lungo una catena di handlers.

```csharp
public abstract class Handler
{
    protected Handler _next;
    
    public Handler SetNext(Handler next)
    {
        _next = next;
        return next;
    }
    
    public abstract void Handle(Request request);
}

public class AuthenticationHandler : Handler
{
    public override void Handle(Request request)
    {
        if (request.IsAuthenticated)
        {
            _next?.Handle(request);
        }
        else
        {
            Console.WriteLine("Not authenticated");
        }
    }
}
```

---

## 13. Spiega il Proxy Pattern.

**Risposta:**
Fornisce un placeholder per controllare l'accesso a un oggetto.

```csharp
public interface IImage
{
    void Display();
}

public class RealImage : IImage
{
    private string _filename;
    
    public RealImage(string filename)
    {
        _filename = filename;
        LoadFromDisk();
    }
    
    private void LoadFromDisk() => Console.WriteLine($"Loading {_filename}");
    public void Display() => Console.WriteLine($"Displaying {_filename}");
}

public class ProxyImage : IImage
{
    private RealImage _realImage;
    private string _filename;
    
    public ProxyImage(string filename) => _filename = filename;
    
    public void Display()
    {
        _realImage ??= new RealImage(_filename);
        _realImage.Display();
    }
}
```

---

## 14. Spiega il State Pattern.

**Risposta:**
Permette a un oggetto di cambiare comportamento quando il suo stato interno cambia.

```csharp
public interface IState
{
    void Handle(Context context);
}

public class ConcreteStateA : IState
{
    public void Handle(Context context)
    {
        Console.WriteLine("State A");
        context.State = new ConcreteStateB();
    }
}

public class Context
{
    public IState State { get; set; }
    
    public void Request() => State.Handle(this);
}
```

---

## 15. Spiega il MVC Pattern.

**Risposta:**
Separa l'applicazione in tre componenti:

**Model:**
Gestisce dati e logica business.

**View:**
Rappresenta i dati all'utente.

**Controller:**
Gestisce input utente e coordina Model e View.

```csharp
// Model
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
}

// View (in ASP.NET MVC)
@model User
<h1>@Model.Name</h1>

// Controller
public class UserController : Controller
{
    public IActionResult Index(int id)
    {
        var user = _userService.GetById(id);
        return View(user);
    }
}
```

---

*Documento creato per la preparazione ai colloqui tecnici - Design Patterns*

