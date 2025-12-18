# Dependency Injection (DI) в C#

## Введение

**Dependency Injection (DI)** — это паттерн проектирования, реализующий принцип инверсии зависимостей (Dependency Inversion Principle). Он позволяет создавать более тестируемый, поддерживаемый и гибкий код.

---

## 1. Что такое Dependency Injection?

### Определение

**Dependency Injection** — это техника, при которой объект получает свои зависимости извне, а не создаёт их внутри себя. Это разделяет классы и упрощает тестирование и поддержку.

### Проблема: Жёсткая связанность

```csharp
// ❌ PROBLEM: Tight coupling
public class OrderService {
    private EmailService _emailService;
    private DatabaseService _databaseService;
    
    public OrderService() {
        // Dependencies created internally - hard to test!
        _emailService = new EmailService();
        _databaseService = new DatabaseService();
    }
    
    public void ProcessOrder(Order order) {
        _databaseService.Save(order);
        _emailService.SendConfirmation(order);
    }
}
```

**Проблемы:**
- Сложно тестировать (не могу mockать зависимости)
- Жёсткая связанность
- Сложно менять реализации

### Решение: Dependency Injection

```csharp
// ✅ SOLUTION: Dependencies injected
public class OrderService {
    private IEmailService _emailService;
    private IDatabaseService _databaseService;
    
    // Dependencies injected via constructor
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

### Диаграмма: Dependency Injection

```
┌─────────────────────────────────────────────┐
│  OrderService                               │
│  - _emailService: IEmailService            │
│  - _databaseService: IDatabaseService      │
│                                             │
│  Dependencies INJECTED from outside        │
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

## 2. Типы Dependency Injection

### Constructor Injection (Рекомендуется)

```csharp
public class OrderService {
    private IEmailService _emailService;
    
    // Dependencies injected via constructor
    public OrderService(IEmailService emailService) {
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    }
}
```

**Преимущества:**
- ✅ Обязывает предоставить все зависимости
- ✅ Неизменяем после конструирования
- ✅ Легко тестировать

### Property Injection

```csharp
public class OrderService {
    // Dependency injected via property
    public IEmailService EmailService { get; set; }
    
    public void ProcessOrder(Order order) {
        EmailService?.SendConfirmation(order);
    }
}

// Usage
var service = new OrderService();
service.EmailService = new EmailService();
```

**Недостатки:**
- ⚠️ Опциональные зависимости (может быть null)
- ⚠️ Может быть изменено после конструирования

### Method Injection

```csharp
public class OrderService {
    public void ProcessOrder(Order order, IEmailService emailService) {
        emailService.SendConfirmation(order);
    }
}
```

**Когда использовать:**
- Зависимости, которые меняются для каждого вызова
- Опциональные зависимости

---

## 3. Dependency Injection Container

### Что такое Container?

**DI Container** — это библиотека, которая автоматически управляет созданием и инъекцией зависимостей.

### Microsoft.Extensions.DependencyInjection

```csharp
using Microsoft.Extensions.DependencyInjection;

// Container configuration
var services = new ServiceCollection();

// Service registration
services.AddScoped<IEmailService, EmailService>();
services.AddScoped<IDatabaseService, DatabaseService>();
services.AddScoped<OrderService>();

// Build container
var serviceProvider = services.BuildServiceProvider();

// Resolution
var orderService = serviceProvider.GetService<OrderService>();
```

### Время жизни сервисов

#### Singleton

```csharp
// One instance for the entire application
services.AddSingleton<IEmailService, EmailService>();
```

#### Scoped

```csharp
// One instance per scope (e.g. HTTP request)
services.AddScoped<IEmailService, EmailService>();
```

#### Transient

```csharp
// New instance every time
services.AddTransient<IEmailService, EmailService>();
```

### Диаграмма: Lifetime

```
┌─────────────────────────────────────────────┐
│  Singleton                                  │
│  ┌──────────┐                              │
│  │ Instance │ ← One for entire app         │
│  └──────────┘                              │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  Scoped                                     │
│  ┌──────────┐                              │
│  │ Instance │ ← One per scope (request)    │
│  └──────────┘                              │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  Transient                                  │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐│
│  │ Instance1│  │ Instance2│  │ Instance3││
│  └──────────┘  └──────────┘  └──────────┘│
│  ← New instance every time                 │
└─────────────────────────────────────────────┘
```

---

## 4. Конфигурация в ASP.NET Core

### Настройка в Program.cs

```csharp
var builder = WebApplication.CreateBuilder(args);

// Service registration
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddScoped<OrderService>();

// Repository pattern
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();
```

### Инъекция в контроллере

```csharp
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase {
    private readonly OrderService _orderService;
    
    // Dependencies injected automatically
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

## 5. Полные практические примеры

### Пример 1: Repository Pattern с DI

```csharp
// Interface
public interface IOrderRepository {
    Task<Order> GetByIdAsync(int id);
    Task SaveAsync(Order order);
}

// Implementation
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

// Service that uses repository
public class OrderService {
    private readonly IOrderRepository _repository;
    
    public OrderService(IOrderRepository repository) {
        _repository = repository;
    }
    
    public async Task<Order> GetOrderAsync(int id) {
        return await _repository.GetByIdAsync(id);
    }
}

// Registration
services.AddScoped<IOrderRepository, OrderRepository>();
services.AddScoped<OrderService>();
```

### Пример 2: Логирование с DI

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

// Registration (ILogger is already registered in ASP.NET Core)
services.AddScoped<IOrderRepository, OrderRepository>();
services.AddScoped<OrderService>();
```

---

## 6. Тестирование с Dependency Injection

### Mock зависимостей

```csharp
// Test with mocks
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

### Преимущества для тестирования

✅ **Изоляция** - Тестируйте один класс за раз  
✅ **Лёгкий мокинг** - Заменяйте зависимости моками  
✅ **Контроль** - Контролируйте поведение зависимостей  

---

## 7. Продвинутые паттерны

### Factory Pattern с DI

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
            _ => throw new ArgumentException("Type not supported")
        };
    }
}
```

### Options Pattern

```csharp
// Configuration class
public class EmailSettings {
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
}

// Registration
services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

// Usage
public class EmailService {
    private readonly EmailSettings _settings;
    
    public EmailService(IOptions<EmailSettings> options) {
        _settings = options.Value;
    }
}
```

---

## 8. Best Practices

### ✅ Что делать

1. **Используйте интерфейсы для зависимостей**
   ```csharp
   // ✅ CORRECT
   public class OrderService {
       private readonly IOrderRepository _repository;
   }
   ```

2. **Используйте Constructor Injection**
   ```csharp
   // ✅ CORRECT
   public OrderService(IOrderRepository repository) {
       _repository = repository;
   }
   ```

3. **Регистрируйте сервисы по интерфейсу**
   ```csharp
   // ✅ CORRECT
   services.AddScoped<IOrderRepository, OrderRepository>();
   ```

4. **Валидируйте зависимости в конструкторе**
   ```csharp
   // ✅ CORRECT
   public OrderService(IOrderRepository repository) {
       _repository = repository ?? throw new ArgumentNullException(nameof(repository));
   }
   ```

### ❌ Чего избегать

1. **Не создавайте зависимости внутри**
   ```csharp
   // ❌ WRONG
   public class OrderService {
       private IOrderRepository _repository = new OrderRepository();
   }
   ```

2. **Не используйте Service Locator pattern**
   ```csharp
   // ❌ WRONG
   public class OrderService {
       public void Process() {
           var repo = ServiceLocator.GetService<IOrderRepository>();
       }
   }
   ```

3. **Не инъектируйте слишком много зависимостей**
   ```csharp
   // ❌ WRONG - Too many dependencies (code smell)
   public OrderService(
       IRepo1 r1, IRepo2 r2, IRepo3 r3, IRepo4 r4, IRepo5 r5) {
   }
   ```

---

## 9. Dependency Injection vs Service Locator

### Dependency Injection (Рекомендуется)

```csharp
// ✅ Explicit dependencies in constructor
public class OrderService {
    private readonly IOrderRepository _repository;
    
    public OrderService(IOrderRepository repository) {
        _repository = repository;
    }
}
```

**Преимущества:**
- Явные зависимости
- Легко тестировать
- Type-safe

### Service Locator (Антипаттерн)

```csharp
// ❌ Hidden dependencies
public class OrderService {
    public void Process() {
        var repo = ServiceLocator.GetService<IOrderRepository>();
    }
}
```

**Недостатки:**
- Скрытые зависимости
- Сложно тестировать
- Связанность с контейнером

---

## 10. Часто задаваемые вопросы (FAQ)

### Q: В чём разница между AddSingleton, AddScoped и AddTransient?
**A:** 
- **Singleton**: Один экземпляр для всего приложения
- **Scoped**: Один экземпляр на область (например, HTTP-запрос)
- **Transient**: Новый экземпляр каждый раз

### Q: Когда использовать Property Injection вместо Constructor Injection?
**A:** Используйте Property Injection только для опциональных зависимостей. Constructor Injection предпочтительнее в большинстве случаев.

### Q: Как управлять циклическими зависимостями?
**A:** Избегайте циклических зависимостей через рефакторинг. Если необходимо, используйте `Lazy<T>` или события.

### Q: Увеличивает ли DI производительность?
**A:** Есть минимальный overhead, но преимущества (тестируемость, поддерживаемость) значительно перевешивают.

---

## Заключение

Dependency Injection необходима для:
- ✅ Написания тестируемого кода
- ✅ Разделения компонентов
- ✅ Улучшения поддерживаемости
- ✅ Упрощения изменения реализаций

Используйте DI для создания более надёжных и поддерживаемых приложений!

---

*Документ создан для объяснения Dependency Injection в C# с практическими примерами и best practices.*

