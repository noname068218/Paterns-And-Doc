# Dependency Injection (DI) in C#

## Introduzione

La **Dependency Injection (DI)** è un pattern di progettazione che implementa il principio di Inversione delle Dipendenze (Dependency Inversion Principle). Permette di creare codice più testabile, manutenibile e flessibile.

---

## 1. Cos'è la Dependency Injection?

### Definizione

La **Dependency Injection** è una tecnica in cui un oggetto riceve le sue dipendenze dall'esterno invece di crearle internamente. Questo disaccoppia le classi e facilita test e manutenzione.

### Problema: Accoppiamento Stretto

```csharp
// ❌ PROBLEMA: Accoppiamento stretto
public class OrderService {
    private EmailService _emailService;
    private DatabaseService _databaseService;
    
    public OrderService() {
        // Dipendenze create internamente - difficile da testare!
        _emailService = new EmailService();
        _databaseService = new DatabaseService();
    }
    
    public void ProcessOrder(Order order) {
        _databaseService.Save(order);
        _emailService.SendConfirmation(order);
    }
}
```

**Problemi:**
- Difficile testare (non posso mockare le dipendenze)
- Accoppiamento stretto
- Difficile cambiare implementazioni

### Soluzione: Dependency Injection

```csharp
// ✅ SOLUZIONE: Dipendenze iniettate
public class OrderService {
    private IEmailService _emailService;
    private IDatabaseService _databaseService;
    
    // Dipendenze iniettate tramite costruttore
    public OrderService(IEmailService emailService, IDatabaseService databaseService) {
        _emailService = emailService;
        _databaseService = databaseService;
    }
    
    public void ProcessOrder(Order order) {
        _databaseService.Save(order);
        _emailService.SendConfirmation(order);
    }
}
```

### Diagramma: Dependency Injection

```
┌─────────────────────────────────────────────┐
│  OrderService                               │
│  - _emailService: IEmailService            │
│  - _databaseService: IDatabaseService      │
│                                             │
│  Dipendenze INIETTATE dall'esterno         │
└─────────────────────────────────────────────┘
                    ▲
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│Email      │ │Database   │ │Mock       │
│Service    │ │Service    │ │Services   │
│(Production)│ │(Production)│ │(Testing)  │
└───────────┘ └───────────┘ └───────────┘
```

---

## 2. Tipi di Dependency Injection

### Constructor Injection (Raccomandato)

```csharp
public class OrderService {
    private IEmailService _emailService;
    
    // Dipendenze iniettate tramite costruttore
    public OrderService(IEmailService emailService) {
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    }
}
```

**Vantaggi:**
- ✅ Obbliga a fornire tutte le dipendenze
- ✅ Immutabile dopo costruzione
- ✅ Facile da testare

### Property Injection

```csharp
public class OrderService {
    // Dipendenza iniettata tramite proprietà
    public IEmailService EmailService { get; set; }
    
    public void ProcessOrder(Order order) {
        EmailService?.SendConfirmation(order);
    }
}

// Utilizzo
var service = new OrderService();
service.EmailService = new EmailService();
```

**Svantaggi:**
- ⚠️ Dipendenze opzionali (può essere null)
- ⚠️ Può essere modificata dopo costruzione

### Method Injection

```csharp
public class OrderService {
    public void ProcessOrder(Order order, IEmailService emailService) {
        emailService.SendConfirmation(order);
    }
}
```

**Quando usare:**
- Dipendenze che cambiano per ogni chiamata
- Dipendenze opzionali

---

## 3. Dependency Injection Container

### Cos'è un Container?

Un **DI Container** è una libreria che gestisce automaticamente la creazione e l'iniezione delle dipendenze.

### Microsoft.Extensions.DependencyInjection

```csharp
using Microsoft.Extensions.DependencyInjection;

// Configurazione del container
var services = new ServiceCollection();

// Registrazione servizi
services.AddScoped<IEmailService, EmailService>();
services.AddScoped<IDatabaseService, DatabaseService>();
services.AddScoped<OrderService>();

// Build del container
var serviceProvider = services.BuildServiceProvider();

// Risoluzione
var orderService = serviceProvider.GetService<OrderService>();
```

### Lifetime dei Servizi

#### Singleton

```csharp
// Una sola istanza per tutta l'applicazione
services.AddSingleton<IEmailService, EmailService>();
```

#### Scoped

```csharp
// Una istanza per scope (es. richiesta HTTP)
services.AddScoped<IEmailService, EmailService>();
```

#### Transient

```csharp
// Nuova istanza ogni volta
services.AddTransient<IEmailService, EmailService>();
```

### Diagramma: Lifetime

```
┌─────────────────────────────────────────────┐
│  Singleton                                  │
│  ┌──────────┐                              │
│  │ Instance │ ← Una sola per tutta l'app   │
│  └──────────┘                              │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  Scoped                                     │
│  ┌──────────┐                              │
│  │ Instance │ ← Una per scope (richiesta)  │
│  └──────────┘                              │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  Transient                                  │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐│
│  │ Instance1│  │ Instance2│  │ Instance3││
│  └──────────┘  └──────────┘  └──────────┘│
│  ← Nuova istanza ogni volta                │
└─────────────────────────────────────────────┘
```

---

## 4. Configurazione in ASP.NET Core

### Setup in Program.cs

```csharp
var builder = WebApplication.CreateBuilder(args);

// Registrazione servizi
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddScoped<OrderService>();

// Repository pattern
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();
```

### Iniezione in Controller

```csharp
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase {
    private readonly OrderService _orderService;
    
    // Dipendenze iniettate automaticamente
    public OrdersController(OrderService orderService) {
        _orderService = orderService;
    }
    
    [HttpPost]
    public IActionResult CreateOrder(Order order) {
        _orderService.ProcessOrder(order);
        return Ok();
    }
}
```

---

## 5. Esempi Pratici Completi

### Esempio 1: Repository Pattern con DI

```csharp
// Interfaccia
public interface IOrderRepository {
    Task<Order> GetByIdAsync(int id);
    Task SaveAsync(Order order);
}

// Implementazione
public class OrderRepository : IOrderRepository {
    private readonly DbContext _context;
    
    public OrderRepository(DbContext context) {
        _context = context;
    }
    
    public async Task<Order> GetByIdAsync(int id) {
        return await _context.Orders.FindAsync(id);
    }
    
    public async Task SaveAsync(Order order) {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }
}

// Service che usa repository
public class OrderService {
    private readonly IOrderRepository _repository;
    
    public OrderService(IOrderRepository repository) {
        _repository = repository;
    }
    
    public async Task<Order> GetOrderAsync(int id) {
        return await _repository.GetByIdAsync(id);
    }
}

// Registrazione
services.AddScoped<IOrderRepository, OrderRepository>();
services.AddScoped<OrderService>();
```

### Esempio 2: Logging con DI

```csharp
public class OrderService {
    private readonly ILogger<OrderService> _logger;
    private readonly IOrderRepository _repository;
    
    public OrderService(
        ILogger<OrderService> logger,
        IOrderRepository repository) {
        _logger = logger;
        _repository = repository;
    }
    
    public async Task ProcessOrderAsync(Order order) {
        _logger.LogInformation("Processing order {OrderId}", order.Id);
        
        try {
            await _repository.SaveAsync(order);
            _logger.LogInformation("Order {OrderId} processed successfully", order.Id);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error processing order {OrderId}", order.Id);
            throw;
        }
    }
}

// Registrazione (ILogger è già registrato in ASP.NET Core)
services.AddScoped<IOrderRepository, OrderRepository>();
services.AddScoped<OrderService>();
```

---

## 6. Testing con Dependency Injection

### Mock delle Dipendenze

```csharp
// Test con mock
[Test]
public void ProcessOrder_Should_Save_And_Send_Email() {
    // Arrange
    var mockEmailService = new Mock<IEmailService>();
    var mockDatabaseService = new Mock<IDatabaseService>();
    var orderService = new OrderService(
        mockEmailService.Object,
        mockDatabaseService.Object
    );
    var order = new Order { Id = 1 };
    
    // Act
    orderService.ProcessOrder(order);
    
    // Assert
    mockDatabaseService.Verify(s => s.Save(order), Times.Once);
    mockEmailService.Verify(s => s.SendConfirmation(order), Times.Once);
}
```

### Vantaggi per il Testing

✅ **Isolamento** - Testa una classe alla volta  
✅ **Mock facile** - Sostituisci dipendenze con mock  
✅ **Controllo** - Controlla comportamento delle dipendenze  

---

## 7. Pattern Avanzati

### Factory Pattern con DI

```csharp
public interface IEmailServiceFactory {
    IEmailService Create(string type);
}

public class EmailServiceFactory : IEmailServiceFactory {
    private readonly IServiceProvider _serviceProvider;
    
    public EmailServiceFactory(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }
    
    public IEmailService Create(string type) {
        return type switch {
            "smtp" => _serviceProvider.GetService<SmptEmailService>(),
            "sendgrid" => _serviceProvider.GetService<SendGridEmailService>(),
            _ => throw new ArgumentException("Tipo non supportato")
        };
    }
}
```

### Options Pattern

```csharp
// Classe di configurazione
public class EmailSettings {
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
}

// Registrazione
services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

// Utilizzo
public class EmailService {
    private readonly EmailSettings _settings;
    
    public EmailService(IOptions<EmailSettings> options) {
        _settings = options.Value;
    }
}
```

---

## 8. Best Practices

### ✅ Cosa Fare

1. **Usa interfacce per dipendenze**
   ```csharp
   // ✅ CORRETTO
   public class OrderService {
       private readonly IOrderRepository _repository;
   }
   ```

2. **Usa Constructor Injection**
   ```csharp
   // ✅ CORRETTO
   public OrderService(IOrderRepository repository) {
       _repository = repository;
   }
   ```

3. **Registra servizi per interfaccia**
   ```csharp
   // ✅ CORRETTO
   services.AddScoped<IOrderRepository, OrderRepository>();
   ```

4. **Valida dipendenze nel costruttore**
   ```csharp
   // ✅ CORRETTO
   public OrderService(IOrderRepository repository) {
       _repository = repository ?? throw new ArgumentNullException(nameof(repository));
   }
   ```

### ❌ Cosa Evitare

1. **Non creare dipendenze internamente**
   ```csharp
   // ❌ SBAGLIATO
   public class OrderService {
       private IOrderRepository _repository = new OrderRepository();
   }
   ```

2. **Non usare Service Locator pattern**
   ```csharp
   // ❌ SBAGLIATO
   public class OrderService {
       public void Process() {
           var repo = ServiceLocator.GetService<IOrderRepository>();
       }
   }
   ```

3. **Non iniettare troppe dipendenze**
   ```csharp
   // ❌ SBAGLIATO - Troppe dipendenze (code smell)
   public OrderService(
       IRepo1 r1, IRepo2 r2, IRepo3 r3, IRepo4 r4, IRepo5 r5) {
   }
   ```

---

## 9. Dependency Injection vs Service Locator

### Dependency Injection (Raccomandato)

```csharp
// ✅ Dipendenze esplicite nel costruttore
public class OrderService {
    private readonly IOrderRepository _repository;
    
    public OrderService(IOrderRepository repository) {
        _repository = repository;
    }
}
```

**Vantaggi:**
- Dipendenze esplicite
- Facile da testare
- Type-safe

### Service Locator (Anti-pattern)

```csharp
// ❌ Dipendenze nascoste
public class OrderService {
    public void Process() {
        var repo = ServiceLocator.GetService<IOrderRepository>();
    }
}
```

**Svantaggi:**
- Dipendenze nascoste
- Difficile da testare
- Accoppiamento con container

---

## 10. Domande Frequenti (FAQ)

### Q: Qual è la differenza tra AddSingleton, AddScoped e AddTransient?
**R:** 
- **Singleton**: Una istanza per tutta l'applicazione
- **Scoped**: Una istanza per scope (es. richiesta HTTP)
- **Transient**: Nuova istanza ogni volta

### Q: Quando usare Property Injection invece di Constructor Injection?
**R:** Usa Property Injection solo per dipendenze opzionali. Constructor Injection è preferibile nella maggior parte dei casi.

### Q: Come gestisco dipendenze circolari?
**R:** Evita dipendenze circolari riprogettando. Se necessario, usa `Lazy<T>` o eventi.

### Q: DI aumenta le performance?
**R:** C'è un overhead minimo, ma i benefici (testabilità, manutenibilità) compensano ampiamente.

---

## Conclusioni

Dependency Injection è essenziale per:
- ✅ Scrivere codice testabile
- ✅ Disaccoppiare componenti
- ✅ Migliorare manutenibilità
- ✅ Facilitare cambiamenti di implementazione

Usa DI per creare applicazioni più robuste e manutenibili!

---

_Documento creato per spiegare Dependency Injection in C# con esempi pratici e best practices._

