# Service Pattern (Сервис) - Паттерн проектирования

## Введение

**Service Pattern (Паттерн Сервис)** — это паттерн проектирования, который инкапсулирует бизнес-логику приложения в отдельные сервисные классы. Service классы координируют работу между различными компонентами и реализуют бизнес-правила приложения.

---

## Что такое Service Pattern?

### Определение

Service Pattern представляет бизнес-логику в виде сервисов — классов, которые содержат методы для выполнения бизнес-операций. Сервисы координируют работу между Repository, другими сервисами и внешними системами.

### Основная цель

Service Pattern позволяет:
- **Инкапсулировать бизнес-логику** в отдельных классах
- **Координировать работу** между различными компонентами
- **Упростить контроллеры** (в веб-приложениях)
- **Улучшить тестируемость** бизнес-логики
- **Переиспользовать** бизнес-логику

---

## Проблема: Бизнес-логика в контроллерах

### ❌ Плохой подход

```csharp
// ❌ ПЛОХО: Бизнес-логика в контроллере
public class UserController : Controller {
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;
    
    public UserController(ApplicationDbContext context, IEmailService emailService) {
        _context = context;
        _emailService = emailService;
    }
    
    [HttpPost]
    public IActionResult Register(RegisterRequest request) {
        // Бизнес-логика смешана с кодом контроллера
        if (string.IsNullOrWhiteSpace(request.Email)) {
            return BadRequest("Email is required");
        }
        
        // Проверка дубликатов
        if (_context.Users.Any(u => u.Email == request.Email)) {
            return BadRequest("Email already exists");
        }
        
        // Валидация
        if (request.Password.Length < 8) {
            return BadRequest("Password must be at least 8 characters");
        }
        
        // Создание пользователя
        var user = new User {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = HashPassword(request.Password)
        };
        
        _context.Users.Add(user);
        _context.SaveChanges();
        
        // Отправка email
        _emailService.SendWelcomeEmail(user.Email);
        
        // Логирование
        _logger.LogInformation($"User {user.Name} registered");
        
        return Ok(user);
    }
    
    private string HashPassword(string password) {
        // Логика хеширования
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}

// Проблемы:
// 1. Контроллер слишком "толстый" - содержит бизнес-логику
// 2. Сложно тестировать бизнес-логику отдельно
// 3. Невозможно переиспользовать логику регистрации
// 4. Нарушение Single Responsibility Principle
// 5. Сложно поддерживать и изменять
```

---

## Решение: Service Pattern

### Базовая структура

```csharp
// 1. Сервисный интерфейс
public interface IUserService {
    User RegisterUser(RegisterUserRequest request);
    void ChangePassword(int userId, string oldPassword, string newPassword);
    void SendWelcomeEmail(int userId);
    User GetUserById(int id);
}

// 2. Реализация сервиса
public class UserService : IUserService {
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly ILogger _logger;
    
    public UserService(
        IUserRepository userRepository,
        IEmailService emailService,
        ILogger<UserService> logger
    ) {
        _userRepository = userRepository;
        _emailService = emailService;
        _logger = logger;
    }
    
    public User RegisterUser(RegisterUserRequest request) {
        // Валидация
        if (string.IsNullOrWhiteSpace(request.Email)) {
            throw new ArgumentException("Email is required");
        }
        
        // Проверка дубликатов
        if (_userRepository.GetByEmail(request.Email) != null) {
            throw new InvalidOperationException("Email already exists");
        }
        
        // Валидация пароля
        if (request.Password.Length < 8) {
            throw new ArgumentException("Password must be at least 8 characters");
        }
        
        // Создание пользователя
        var user = new User {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow
        };
        
        // Сохранение через Repository
        _userRepository.Add(user);
        
        // Отправка email
        _emailService.SendWelcomeEmail(user.Email);
        
        // Логирование
        _logger.LogInformation($"User {user.Name} registered");
        
        return user;
    }
    
    public void ChangePassword(int userId, string oldPassword, string newPassword) {
        var user = _userRepository.GetById(userId);
        if (user == null) {
            throw new InvalidOperationException("User not found");
        }
        
        // Проверка старого пароля
        if (!VerifyPassword(oldPassword, user.PasswordHash)) {
            throw new UnauthorizedAccessException("Invalid password");
        }
        
        // Валидация нового пароля
        if (newPassword.Length < 8) {
            throw new ArgumentException("New password must be at least 8 characters");
        }
        
        // Обновление пароля
        user.PasswordHash = HashPassword(newPassword);
        user.UpdatedAt = DateTime.UtcNow;
        _userRepository.Update(user);
        
        _logger.LogInformation($"Password changed for user {userId}");
    }
    
    public void SendWelcomeEmail(int userId) {
        var user = _userRepository.GetById(userId);
        if (user == null) {
            throw new InvalidOperationException("User not found");
        }
        
        _emailService.SendWelcomeEmail(user.Email);
    }
    
    public User GetUserById(int id) {
        return _userRepository.GetById(id);
    }
    
    private string HashPassword(string password) {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    
    private bool VerifyPassword(string password, string hash) {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}

// 3. Контроллер использует сервис
public class UserController : Controller {
    private readonly IUserService _userService;
    
    public UserController(IUserService userService) {
        _userService = userService;
    }
    
    [HttpPost]
    public IActionResult Register(RegisterRequest request) {
        try {
            // Тонкий контроллер - только координация
            var user = _userService.RegisterUser(request);
            return Ok(user);
        }
        catch (ArgumentException ex) {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex) {
            return Conflict(ex.Message);
        }
    }
}
```

---

## Типы Service классов

### 1. Domain Service (Доменный сервис)

Содержит бизнес-логику, которая не относится к одной сущности.

```csharp
// Доменный сервис для сложных бизнес-правил
public interface IOrderCalculationService {
    decimal CalculateOrderTotal(Order order);
    decimal CalculateDiscount(Order order, Customer customer);
    decimal CalculateTax(decimal amount, string country);
}

public class OrderCalculationService : IOrderCalculationService {
    public decimal CalculateOrderTotal(Order order) {
        var subtotal = order.Items.Sum(i => i.Price * i.Quantity);
        var discount = CalculateDiscount(order, order.Customer);
        var tax = CalculateTax(subtotal - discount, order.Customer.Country);
        
        return subtotal - discount + tax;
    }
    
    public decimal CalculateDiscount(Order order, Customer customer) {
        decimal discount = 0;
        
        // Скидка для VIP клиентов
        if (customer.IsVip) {
            discount += order.TotalAmount * 0.1m; // 10%
        }
        
        // Скидка за большой заказ
        if (order.TotalAmount > 1000) {
            discount += order.TotalAmount * 0.05m; // 5%
        }
        
        return discount;
    }
    
    public decimal CalculateTax(decimal amount, string country) {
        var taxRates = new Dictionary<string, decimal> {
            { "US", 0.08m },
            { "CA", 0.13m },
            { "UK", 0.20m }
        };
        
        var rate = taxRates.GetValueOrDefault(country, 0.10m);
        return amount * rate;
    }
}
```

### 2. Application Service (Сервис приложения)

Координирует работу между различными компонентами для выполнения пользовательских сценариев.

```csharp
// Сервис приложения для координации
public interface IOrderService {
    Order CreateOrder(CreateOrderRequest request);
    void CancelOrder(int orderId);
    void ShipOrder(int orderId);
}

public class OrderService : IOrderService {
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;
    private readonly IOrderCalculationService _calculationService;
    private readonly IEmailService _emailService;
    private readonly IPaymentService _paymentService;
    
    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IUserRepository userRepository,
        IOrderCalculationService calculationService,
        IEmailService emailService,
        IPaymentService paymentService
    ) {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _userRepository = userRepository;
        _calculationService = calculationService;
        _emailService = emailService;
        _paymentService = paymentService;
    }
    
    public Order CreateOrder(CreateOrderRequest request) {
        // Валидация пользователя
        var user = _userRepository.GetById(request.UserId);
        if (user == null) {
            throw new InvalidOperationException("User not found");
        }
        
        // Создание заказа
        var order = new Order {
            UserId = request.UserId,
            OrderDate = DateTime.UtcNow,
            Items = new List<OrderItem>()
        };
        
        // Обработка товаров
        foreach (var itemRequest in request.Items) {
            var product = _productRepository.GetById(itemRequest.ProductId);
            if (product == null) {
                throw new InvalidOperationException($"Product {itemRequest.ProductId} not found");
            }
            
            if (product.Stock < itemRequest.Quantity) {
                throw new InvalidOperationException($"Insufficient stock for {product.Name}");
            }
            
            order.Items.Add(new OrderItem {
                ProductId = product.Id,
                Quantity = itemRequest.Quantity,
                Price = product.Price
            });
            
            // Обновление остатков
            product.Stock -= itemRequest.Quantity;
            _productRepository.Update(product);
        }
        
        // Расчет суммы (используем доменный сервис)
        order.TotalAmount = _calculationService.CalculateOrderTotal(order);
        
        // Сохранение заказа
        _orderRepository.Add(order);
        
        // Обработка оплаты
        _paymentService.ProcessPayment(order.Id, order.TotalAmount);
        
        // Отправка подтверждения
        _emailService.SendOrderConfirmation(user.Email, order);
        
        return order;
    }
    
    public void CancelOrder(int orderId) {
        var order = _orderRepository.GetById(orderId);
        if (order == null) {
            throw new InvalidOperationException("Order not found");
        }
        
        if (order.Status == OrderStatus.Shipped) {
            throw new InvalidOperationException("Cannot cancel shipped order");
        }
        
        order.Status = OrderStatus.Cancelled;
        _orderRepository.Update(order);
        
        // Возврат товаров на склад
        foreach (var item in order.Items) {
            var product = _productRepository.GetById(item.ProductId);
            product.Stock += item.Quantity;
            _productRepository.Update(product);
        }
        
        // Возврат денег
        _paymentService.RefundPayment(order.Id);
        
        // Уведомление пользователя
        var user = _userRepository.GetById(order.UserId);
        _emailService.SendOrderCancellation(user.Email, order);
    }
}
```

### 3. Infrastructure Service (Инфраструктурный сервис)

Предоставляет техническую функциональность (email, логирование, кэширование).

```csharp
// Инфраструктурные сервисы
public interface IEmailService {
    void SendEmail(string to, string subject, string body);
    void SendWelcomeEmail(string email);
}

public interface IFileStorageService {
    string UploadFile(Stream file, string fileName);
    void DeleteFile(string filePath);
    Stream DownloadFile(string filePath);
}

public interface ICacheService {
    T Get<T>(string key);
    void Set<T>(string key, T value, TimeSpan expiration);
    void Remove(string key);
}
```

---

## Практический пример: Система управления заказами

```csharp
// Комплексный пример сервиса
public interface IOrderManagementService {
    Order CreateOrder(CreateOrderRequest request);
    void ProcessOrder(int orderId);
    void ShipOrder(int orderId);
    void CancelOrder(int orderId, string reason);
    Order GetOrder(int orderId);
    IEnumerable<Order> GetUserOrders(int userId);
}

public class OrderManagementService : IOrderManagementService {
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPaymentService _paymentService;
    private readonly IShippingService _shippingService;
    private readonly IEmailService _emailService;
    private readonly IOrderCalculationService _calculationService;
    private readonly ILogger<OrderManagementService> _logger;
    
    public OrderManagementService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IUserRepository userRepository,
        IPaymentService paymentService,
        IShippingService shippingService,
        IEmailService emailService,
        IOrderCalculationService calculationService,
        ILogger<OrderManagementService> logger
    ) {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _userRepository = userRepository;
        _paymentService = paymentService;
        _shippingService = shippingService;
        _emailService = emailService;
        _calculationService = calculationService;
        _logger = logger;
    }
    
    public Order CreateOrder(CreateOrderRequest request) {
        _logger.LogInformation($"Creating order for user {request.UserId}");
        
        // 1. Валидация пользователя
        var user = _userRepository.GetById(request.UserId);
        if (user == null) {
            throw new InvalidOperationException("User not found");
        }
        
        // 2. Создание заказа
        var order = new Order {
            UserId = request.UserId,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            Items = new List<OrderItem>()
        };
        
        // 3. Обработка товаров
        foreach (var itemRequest in request.Items) {
            var product = _productRepository.GetById(itemRequest.ProductId);
            if (product == null) {
                throw new InvalidOperationException($"Product {itemRequest.ProductId} not found");
            }
            
            if (product.Stock < itemRequest.Quantity) {
                throw new InvalidOperationException(
                    $"Insufficient stock for {product.Name}. Available: {product.Stock}, Requested: {itemRequest.Quantity}"
                );
            }
            
            order.Items.Add(new OrderItem {
                ProductId = product.Id,
                Quantity = itemRequest.Quantity,
                Price = product.Price
            });
            
            // Резервирование товара
            product.Stock -= itemRequest.Quantity;
            _productRepository.Update(product);
        }
        
        // 4. Расчет суммы
        order.TotalAmount = _calculationService.CalculateOrderTotal(order);
        
        // 5. Сохранение заказа
        _orderRepository.Add(order);
        
        _logger.LogInformation($"Order {order.Id} created with total {order.TotalAmount}");
        
        return order;
    }
    
    public void ProcessOrder(int orderId) {
        _logger.LogInformation($"Processing order {orderId}");
        
        var order = _orderRepository.GetById(orderId);
        if (order == null) {
            throw new InvalidOperationException("Order not found");
        }
        
        if (order.Status != OrderStatus.Pending) {
            throw new InvalidOperationException($"Cannot process order in status {order.Status}");
        }
        
        // Обработка оплаты
        var paymentResult = _paymentService.ProcessPayment(order.Id, order.TotalAmount);
        if (!paymentResult.Success) {
            throw new InvalidOperationException($"Payment failed: {paymentResult.ErrorMessage}");
        }
        
        order.Status = OrderStatus.Paid;
        order.PaymentDate = DateTime.UtcNow;
        _orderRepository.Update(order);
        
        // Отправка подтверждения
        var user = _userRepository.GetById(order.UserId);
        _emailService.SendOrderConfirmation(user.Email, order);
        
        _logger.LogInformation($"Order {orderId} processed successfully");
    }
    
    public void ShipOrder(int orderId) {
        var order = _orderRepository.GetById(orderId);
        if (order == null) {
            throw new InvalidOperationException("Order not found");
        }
        
        if (order.Status != OrderStatus.Paid) {
            throw new InvalidOperationException($"Cannot ship order in status {order.Status}");
        }
        
        // Создание отправки
        var trackingNumber = _shippingService.CreateShipment(order);
        
        order.Status = OrderStatus.Shipped;
        order.ShippingDate = DateTime.UtcNow;
        order.TrackingNumber = trackingNumber;
        _orderRepository.Update(order);
        
        // Уведомление пользователя
        var user = _userRepository.GetById(order.UserId);
        _emailService.SendShippingNotification(user.Email, order);
    }
    
    public void CancelOrder(int orderId, string reason) {
        var order = _orderRepository.GetById(orderId);
        if (order == null) {
            throw new InvalidOperationException("Order not found");
        }
        
        if (order.Status == OrderStatus.Shipped || order.Status == OrderStatus.Delivered) {
            throw new InvalidOperationException($"Cannot cancel order in status {order.Status}");
        }
        
        order.Status = OrderStatus.Cancelled;
        order.CancellationReason = reason;
        order.CancelledAt = DateTime.UtcNow;
        _orderRepository.Update(order);
        
        // Возврат товаров
        foreach (var item in order.Items) {
            var product = _productRepository.GetById(item.ProductId);
            product.Stock += item.Quantity;
            _productRepository.Update(product);
        }
        
        // Возврат денег (если оплачено)
        if (order.Status == OrderStatus.Paid) {
            _paymentService.RefundPayment(order.Id);
        }
        
        // Уведомление
        var user = _userRepository.GetById(order.UserId);
        _emailService.SendOrderCancellation(user.Email, order, reason);
    }
    
    public Order GetOrder(int orderId) {
        return _orderRepository.GetById(orderId);
    }
    
    public IEnumerable<Order> GetUserOrders(int userId) {
        return _orderRepository.GetByUserId(userId);
    }
}
```

---

## Repository vs Service: В чём разница?

### Repository Pattern

**Ответственность:** Работа с данными (CRUD операции)
- Получение данных из хранилища
- Сохранение данных
- Обновление данных
- Удаление данных
- Поиск и фильтрация

```csharp
public interface IUserRepository {
    User GetById(int id);
    User GetByEmail(string email);
    void Add(User user);
    void Update(User user);
    void Delete(int id);
    IEnumerable<User> Search(string query);
}
```

### Service Pattern

**Ответственность:** Бизнес-логика и координация
- Выполнение бизнес-правил
- Координация между Repository
- Валидация данных
- Трансформация данных
- Вызов внешних сервисов

```csharp
public interface IUserService {
    User RegisterUser(RegisterUserRequest request);
    void ChangePassword(int userId, string oldPassword, string newPassword);
    void SendWelcomeEmail(int userId);
    void ActivateUser(int userId);
    void DeactivateUser(int userId);
}
```

### Визуальное сравнение

```
Repository Layer (Данные):
┌─────────────────────┐
│  IUserRepository    │
│  - GetById()        │
│  - Add()            │
│  - Update()         │
│  - Delete()         │
└─────────────────────┘

Service Layer (Бизнес-логика):
┌─────────────────────┐
│  IUserService       │
│  - RegisterUser()   │  ──┐
│  - ChangePassword() │    │ Использует
│  - SendWelcome()    │  ──┘
└─────────────────────┘
         │
         ▼
┌─────────────────────┐
│  IUserRepository    │
└─────────────────────┘
```

### Пример взаимодействия

```csharp
// Repository - только данные
public class UserRepository : IUserRepository {
    public User GetById(int id) {
        return _context.Users.Find(id);
    }
    
    public void Add(User user) {
        _context.Users.Add(user);
        _context.SaveChanges();
    }
}

// Service - бизнес-логика + использует Repository
public class UserService {
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    
    public void RegisterUser(RegisterUserRequest request) {
        // Бизнес-логика
        if (_userRepository.GetByEmail(request.Email) != null) {
            throw new InvalidOperationException("Email exists");
        }
        
        var user = new User { /* ... */ };
        
        // Использует Repository для сохранения
        _userRepository.Add(user);
        
        // Вызывает другой сервис
        _emailService.SendWelcomeEmail(user.Email);
    }
}
```

**Вывод:**
- **Repository** = работа с данными (CRUD)
- **Service** = бизнес-логика + координация
- **Service использует Repository**, а не наоборот

---

## Регистрация в DI Container

```csharp
// Program.cs или Startup.cs
public void ConfigureServices(IServiceCollection services) {
    // Repository
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IOrderRepository, OrderRepository>();
    
    // Domain Services
    services.AddScoped<IOrderCalculationService, OrderCalculationService>();
    
    // Application Services
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IOrderService, OrderService>();
    services.AddScoped<IOrderManagementService, OrderManagementService>();
    
    // Infrastructure Services
    services.AddScoped<IEmailService, EmailService>();
    services.AddScoped<IPaymentService, PaymentService>();
}
```

---

## Тестирование Service

```csharp
// Unit тест для сервиса
[Test]
public void UserService_RegisterUser_ShouldCreateUserAndSendEmail() {
    // Arrange
    var mockRepository = new Mock<IUserRepository>();
    var mockEmailService = new Mock<IEmailService>();
    var mockLogger = new Mock<ILogger<UserService>>();
    
    mockRepository.Setup(r => r.GetByEmail(It.IsAny<string>())).Returns((User)null);
    mockRepository.Setup(r => r.Add(It.IsAny<User>()));
    
    var service = new UserService(
        mockRepository.Object,
        mockEmailService.Object,
        mockLogger.Object
    );
    
    var request = new RegisterUserRequest {
        Name = "John",
        Email = "john@example.com",
        Password = "password123"
    };
    
    // Act
    var user = service.RegisterUser(request);
    
    // Assert
    Assert.IsNotNull(user);
    Assert.AreEqual("John", user.Name);
    mockRepository.Verify(r => r.Add(It.IsAny<User>()), Times.Once);
    mockEmailService.Verify(e => e.SendWelcomeEmail("john@example.com"), Times.Once);
}
```

---

## Преимущества Service Pattern

### 1. Разделение ответственностей

Бизнес-логика отделена от контроллеров и Repository.

### 2. Переиспользование

Бизнес-логику можно использовать в разных местах (API, консольное приложение, фоновая задача).

### 3. Тестируемость

Легко тестировать бизнес-логику изолированно через моки.

### 4. Упрощение контроллеров

Контроллеры становятся тонкими — только координация запросов/ответов.

### 5. Централизация бизнес-правил

Все бизнес-правила в одном месте.

---

## Когда использовать Service Pattern?

### ✅ Используйте Service когда:

1. **Есть сложная бизнес-логика**
2. **Нужна координация между несколькими Repository**
3. **Требуется валидация и бизнес-правила**
4. **Нужно вызывать внешние сервисы**
5. **Хотите упростить контроллеры**

### ❌ Не создавайте Service для:

1. **Простых CRUD операций** — достаточно Repository
2. **Простых делегирований** — не нужен дополнительный слой

---

## Заключение

**Service Pattern** — важный паттерн для организации бизнес-логики приложения. Он помогает создать чистую архитектуру с разделением ответственностей.

**Ключевые моменты:**
- ✅ Service инкапсулирует бизнес-логику
- ✅ Service координирует работу между компонентами
- ✅ Service использует Repository для работы с данными
- ✅ Service ≠ Repository: разные ответственности
- ✅ Используйте осознанно — не переусердствуйте

**Помните:** Repository работает с данными, Service реализует бизнес-логику. Они дополняют друг друга в правильной архитектуре.

---

*Документ создан для объяснения Service Pattern с практическими примерами на C#.*

