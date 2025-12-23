# Repository Pattern (Репозиторий) - Паттерн проектирования

## Введение

**Repository Pattern (Паттерн Репозиторий)** — это паттерн проектирования, который инкапсулирует логику доступа к данным и предоставляет более объектно-ориентированный интерфейс. Repository действует как слой абстракции между бизнес-логикой и источником данных.

---

## Что такое Repository Pattern?

### Определение

Repository Pattern инкапсулирует набор объектов, хранящихся в хранилище данных, и операции, выполняемые над ними, предоставляя более объектно-ориентированный взгляд на уровень доступа к данным.

### Основная цель

Repository Pattern позволяет:
- **Абстрагироваться от источника данных** (БД, файлы, API)
- **Централизовать логику доступа к данным**
- **Упростить тестирование** бизнес-логики
- **Улучшить поддерживаемость** кода

---

## Проблема: Прямой доступ к данным

### ❌ Плохой подход

```csharp
// ❌ ПЛОХО: Бизнес-логика напрямую работает с БД
public class UserService {
    private readonly string _connectionString;
    
    public UserService(string connectionString) {
        _connectionString = connectionString;
    }
    
    public User GetUser(int id) {
        // Бизнес-логика знает о деталях работы с БД
        using (var connection = new SqlConnection(_connectionString)) {
            connection.Open();
            var command = new SqlCommand($"SELECT * FROM Users WHERE Id = {id}", connection);
            var reader = command.ExecuteReader();
            
            if (reader.Read()) {
                return new User {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2)
                };
            }
        }
        return null;
    }
    
    public void SaveUser(User user) {
        // SQL код напрямую в бизнес-логике
        using (var connection = new SqlConnection(_connectionString)) {
            connection.Open();
            var command = new SqlCommand(
                $"INSERT INTO Users (Name, Email) VALUES ('{user.Name}', '{user.Email}')",
                connection
            );
            command.ExecuteNonQuery();
        }
    }
    
    public List<User> GetAllUsers() {
        // Ещё больше SQL кода
        // ...
    }
}

// Проблемы:
// 1. Бизнес-логика смешана с логикой доступа к данным
// 2. Сложно тестировать - нужно реальная БД
// 3. Сложно изменить источник данных (БД → API → файлы)
// 4. Дублирование кода доступа к данным
// 5. SQL injection уязвимости
```

---

## Решение: Repository Pattern

### Базовая структура

```csharp
// 1. Доменная модель (Entity)
public class User {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}

// 2. Интерфейс Repository
public interface IUserRepository {
    User GetById(int id);
    IEnumerable<User> GetAll();
    void Add(User user);
    void Update(User user);
    void Delete(int id);
    bool Exists(int id);
}

// 3. Реализация Repository для БД
public class UserRepository : IUserRepository {
    private readonly DbContext _context;
    
    public UserRepository(DbContext context) {
        _context = context;
    }
    
    public User GetById(int id) {
        return _context.Users.Find(id);
    }
    
    public IEnumerable<User> GetAll() {
        return _context.Users.ToList();
    }
    
    public void Add(User user) {
        _context.Users.Add(user);
        _context.SaveChanges();
    }
    
    public void Update(User user) {
        _context.Users.Update(user);
        _context.SaveChanges();
    }
    
    public void Delete(int id) {
        var user = _context.Users.Find(id);
        if (user != null) {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
    
    public bool Exists(int id) {
        return _context.Users.Any(u => u.Id == id);
    }
}

// 4. Бизнес-логика использует Repository
public class UserService {
    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository userRepository) {
        _userRepository = userRepository;
    }
    
    public User GetUser(int id) {
        // Чистая бизнес-логика без деталей БД
        return _userRepository.GetById(id);
    }
    
    public void RegisterUser(User user) {
        if (_userRepository.Exists(user.Id)) {
            throw new InvalidOperationException("User already exists");
        }
        
        // Валидация и бизнес-правила
        if (string.IsNullOrWhiteSpace(user.Email)) {
            throw new ArgumentException("Email is required");
        }
        
        _userRepository.Add(user);
    }
}
```

---

## Типы Repository Pattern

### 1. Generic Repository

Общий репозиторий для всех сущностей.

```csharp
// Generic интерфейс
public interface IRepository<T> where T : class {
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
}

// Generic реализация
public class Repository<T> : IRepository<T> where T : class {
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;
    
    public Repository(DbContext context) {
        _context = context;
        _dbSet = context.Set<T>();
    }
    
    public T GetById(int id) {
        return _dbSet.Find(id);
    }
    
    public IEnumerable<T> GetAll() {
        return _dbSet.ToList();
    }
    
    public void Add(T entity) {
        _dbSet.Add(entity);
        _context.SaveChanges();
    }
    
    public void Update(T entity) {
        _dbSet.Update(entity);
        _context.SaveChanges();
    }
    
    public void Delete(int id) {
        var entity = _dbSet.Find(id);
        if (entity != null) {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }
    }
}

// Использование
public class UserRepository : Repository<User>, IUserRepository {
    public UserRepository(DbContext context) : base(context) { }
    
    // Дополнительные методы специфичные для User
    public User GetByEmail(string email) {
        return _dbSet.FirstOrDefault(u => u.Email == email);
    }
}
```

### 2. Специфичный Repository

Отдельный репозиторий для каждой сущности с её специфичными методами.

```csharp
public interface IUserRepository {
    User GetById(int id);
    User GetByEmail(string email);
    IEnumerable<User> GetActiveUsers();
    IEnumerable<User> SearchByName(string name);
    void Add(User user);
    void Update(User user);
    void Delete(int id);
}

public class UserRepository : IUserRepository {
    private readonly DbContext _context;
    
    public UserRepository(DbContext context) {
        _context = context;
    }
    
    public User GetById(int id) {
        return _context.Users.Find(id);
    }
    
    public User GetByEmail(string email) {
        return _context.Users.FirstOrDefault(u => u.Email == email);
    }
    
    public IEnumerable<User> GetActiveUsers() {
        return _context.Users
            .Where(u => u.IsActive)
            .ToList();
    }
    
    public IEnumerable<User> SearchByName(string name) {
        return _context.Users
            .Where(u => u.Name.Contains(name))
            .ToList();
    }
    
    public void Add(User user) {
        _context.Users.Add(user);
        _context.SaveChanges();
    }
    
    public void Update(User user) {
        _context.Users.Update(user);
        _context.SaveChanges();
    }
    
    public void Delete(int id) {
        var user = _context.Users.Find(id);
        if (user != null) {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
```

---

## Unit of Work Pattern

### Что такое Unit of Work?

Unit of Work координирует работу нескольких репозиториев и обеспечивает атомарность операций.

```csharp
// Unit of Work интерфейс
public interface IUnitOfWork : IDisposable {
    IUserRepository Users { get; }
    IOrderRepository Orders { get; }
    IProductRepository Products { get; }
    
    int SaveChanges();
    Task<int> SaveChangesAsync();
}

// Реализация Unit of Work
public class UnitOfWork : IUnitOfWork {
    private readonly DbContext _context;
    private IUserRepository _users;
    private IOrderRepository _orders;
    private IProductRepository _products;
    
    public UnitOfWork(DbContext context) {
        _context = context;
    }
    
    public IUserRepository Users {
        get {
            return _users ??= new UserRepository(_context);
        }
    }
    
    public IOrderRepository Orders {
        get {
            return _orders ??= new OrderRepository(_context);
        }
    }
    
    public IProductRepository Products {
        get {
            return _products ??= new ProductRepository(_context);
        }
    }
    
    public int SaveChanges() {
        return _context.SaveChanges();
    }
    
    public async Task<int> SaveChangesAsync() {
        return await _context.SaveChangesAsync();
    }
    
    public void Dispose() {
        _context?.Dispose();
    }
}

// Использование
public class OrderService {
    private readonly IUnitOfWork _unitOfWork;
    
    public OrderService(IUnitOfWork unitOfWork) {
        _unitOfWork = unitOfWork;
    }
    
    public void CreateOrder(Order order) {
        // Используем несколько репозиториев
        var user = _unitOfWork.Users.GetById(order.UserId);
        if (user == null) {
            throw new InvalidOperationException("User not found");
        }
        
        foreach (var item in order.Items) {
            var product = _unitOfWork.Products.GetById(item.ProductId);
            if (product == null) {
                throw new InvalidOperationException($"Product {item.ProductId} not found");
            }
        }
        
        _unitOfWork.Orders.Add(order);
        
        // Все изменения сохраняются атомарно
        _unitOfWork.SaveChanges();
    }
}
```

---

## Практический пример: Система заказов

```csharp
// Доменные модели
public class User {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}

public class Product {
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
}

public class Order {
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderItem> Items { get; set; }
}

public class OrderItem {
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

// Репозитории
public interface IUserRepository {
    User GetById(int id);
    User GetByEmail(string email);
    void Add(User user);
}

public interface IProductRepository {
    Product GetById(int id);
    IEnumerable<Product> GetAvailableProducts();
    void Update(Product product);
}

public interface IOrderRepository {
    Order GetById(int id);
    IEnumerable<Order> GetByUserId(int userId);
    void Add(Order order);
}

// Реализации (с Entity Framework)
public class UserRepository : IUserRepository {
    private readonly DbContext _context;
    
    public UserRepository(DbContext context) {
        _context = context;
    }
    
    public User GetById(int id) {
        return _context.Set<User>().Find(id);
    }
    
    public User GetByEmail(string email) {
        return _context.Set<User>()
            .FirstOrDefault(u => u.Email == email);
    }
    
    public void Add(User user) {
        _context.Set<User>().Add(user);
        _context.SaveChanges();
    }
}

// Сервис использует репозитории
public class OrderService {
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;
    
    public OrderService(
        IOrderRepository orderRepository,
        IUserRepository userRepository,
        IProductRepository productRepository
    ) {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _productRepository = productRepository;
    }
    
    public void CreateOrder(Order order) {
        // Бизнес-логика
        var user = _userRepository.GetById(order.UserId);
        if (user == null) {
            throw new InvalidOperationException("User not found");
        }
        
        foreach (var item in order.Items) {
            var product = _productRepository.GetById(item.ProductId);
            if (product == null) {
                throw new InvalidOperationException($"Product {item.ProductId} not found");
            }
            
            if (product.Stock < item.Quantity) {
                throw new InvalidOperationException($"Insufficient stock for product {product.Name}");
            }
            
            // Обновляем остаток
            product.Stock -= item.Quantity;
            _productRepository.Update(product);
        }
        
        order.OrderDate = DateTime.Now;
        order.TotalAmount = order.Items.Sum(i => i.Price * i.Quantity);
        
        _orderRepository.Add(order);
    }
}
```

---

## Тестирование Repository

### Mock Repository для тестов

```csharp
// In-memory реализация для тестов
public class InMemoryUserRepository : IUserRepository {
    private readonly List<User> _users = new List<User>();
    private int _nextId = 1;
    
    public User GetById(int id) {
        return _users.FirstOrDefault(u => u.Id == id);
    }
    
    public IEnumerable<User> GetAll() {
        return _users.ToList();
    }
    
    public void Add(User user) {
        user.Id = _nextId++;
        _users.Add(user);
    }
    
    public void Update(User user) {
        var existing = _users.FirstOrDefault(u => u.Id == user.Id);
        if (existing != null) {
            var index = _users.IndexOf(existing);
            _users[index] = user;
        }
    }
    
    public void Delete(int id) {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user != null) {
            _users.Remove(user);
        }
    }
    
    public bool Exists(int id) {
        return _users.Any(u => u.Id == id);
    }
}

// Unit тест
[Test]
public void UserService_RegisterUser_ShouldAddUser() {
    // Arrange
    var repository = new InMemoryUserRepository();
    var service = new UserService(repository);
    var user = new User { Name = "John", Email = "john@example.com" };
    
    // Act
    service.RegisterUser(user);
    
    // Assert
    var savedUser = repository.GetByEmail("john@example.com");
    Assert.IsNotNull(savedUser);
    Assert.AreEqual("John", savedUser.Name);
}
```

---

## Repository с Entity Framework Core

```csharp
// DbContext
public class ApplicationDbContext : DbContext {
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlServer(connectionString);
    }
}

// Repository с EF Core
public class UserRepository : IUserRepository {
    private readonly ApplicationDbContext _context;
    
    public UserRepository(ApplicationDbContext context) {
        _context = context;
    }
    
    public User GetById(int id) {
        return _context.Users.Find(id);
    }
    
    public async Task<User> GetByIdAsync(int id) {
        return await _context.Users.FindAsync(id);
    }
    
    public IEnumerable<User> GetAll() {
        return _context.Users.ToList();
    }
    
    public IEnumerable<User> GetByEmail(string email) {
        return _context.Users
            .Where(u => u.Email.Contains(email))
            .ToList();
    }
    
    public void Add(User user) {
        _context.Users.Add(user);
        _context.SaveChanges();
    }
    
    public async Task AddAsync(User user) {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
    
    public void Update(User user) {
        _context.Users.Update(user);
        _context.SaveChanges();
    }
    
    public void Delete(int id) {
        var user = _context.Users.Find(id);
        if (user != null) {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
```

---

## Регистрация в DI Container (ASP.NET Core)

```csharp
// Program.cs или Startup.cs
public void ConfigureServices(IServiceCollection services) {
    // Регистрация DbContext
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
    
    // Регистрация Repository
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IOrderRepository, OrderRepository>();
    services.AddScoped<IProductRepository, ProductRepository>();
    
    // Или Unit of Work
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    
    // Регистрация сервисов
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IOrderService, OrderService>();
}
```

---

## Преимущества Repository Pattern

### 1. Абстракция источника данных

Можно легко изменить источник данных (БД → API → файлы) без изменения бизнес-логики.

### 2. Тестируемость

Легко создать mock или in-memory реализацию для тестирования.

### 3. Централизация логики доступа к данным

Вся логика работы с данными в одном месте.

### 4. Упрощение бизнес-логики

Бизнес-логика не знает деталей работы с БД.

### 5. Переиспользование

Repository можно переиспользовать в разных сервисах.

---

## Недостатки Repository Pattern

### 1. Дополнительный слой абстракции

Может быть избыточным для простых CRUD операций.

### 2. Overhead

Дополнительный код и сложность.

### 3. Проблема с IQueryable

Если возвращать IQueryable из Repository, теряется абстракция.

---

## Когда использовать Repository Pattern?

### ✅ Используйте Repository когда:

1. **Нужна абстракция от источника данных**
2. **Хотите упростить тестирование**
3. **Нужно централизовать логику доступа к данным**
4. **Работаете с несколькими источниками данных**
5. **Нужна сложная бизнес-логика** работы с данными

### ❌ Не используйте Repository когда:

1. **Простые CRUD операции** — Entity Framework уже предоставляет Repository
2. **Маленький проект** — может быть избыточным
3. **Только один источник данных** и он не меняется

---

## Repository vs Service: В чём разница?

### Repository Pattern

**Ответственность:** Работа с данными (CRUD операции)
- Получение данных из хранилища
- Сохранение данных
- Обновление данных
- Удаление данных
- Поиск и фильтрация данных

```csharp
public interface IUserRepository {
    User GetById(int id);
    void Add(User user);
    void Update(User user);
    void Delete(int id);
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
    void RegisterUser(RegisterUserRequest request);
    void ChangePassword(int userId, string oldPassword, string newPassword);
    void SendWelcomeEmail(int userId);
}
```

### Сравнение

| Критерий | Repository | Service |
|----------|------------|---------|
| **Ответственность** | Доступ к данным | Бизнес-логика |
| **Операции** | CRUD | Бизнес-операции |
| **Зависимости** | DbContext, источники данных | Repository, другие сервисы |
| **Примеры методов** | GetById, Add, Update | RegisterUser, ProcessOrder |

### Пример взаимодействия

```csharp
// Repository - только работа с данными
public class UserRepository : IUserRepository {
    public User GetById(int id) { /* ... */ }
    public void Add(User user) { /* ... */ }
}

// Service - бизнес-логика, использует Repository
public class UserService {
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    
    public void RegisterUser(RegisterUserRequest request) {
        // Бизнес-логика
        if (_userRepository.GetByEmail(request.Email) != null) {
            throw new InvalidOperationException("Email already exists");
        }
        
        var user = new User {
            Name = request.Name,
            Email = request.Email
        };
        
        // Использует Repository для сохранения
        _userRepository.Add(user);
        
        // Вызывает другой сервис
        _emailService.SendWelcomeEmail(user.Email);
    }
}
```

**Вывод:** Repository работает с данными, Service реализует бизнес-логику и использует Repository.

---

## Заключение

**Repository Pattern** — мощный паттерн для абстракции доступа к данным, который улучшает тестируемость, поддерживаемость и гибкость кода.

**Ключевые моменты:**
- ✅ Инкапсулирует логику доступа к данным
- ✅ Абстрагирует источник данных
- ✅ Упрощает тестирование
- ✅ Разделяет ответственности: Repository = данные, Service = бизнес-логика
- ✅ Используйте осознанно — не переусердствуйте

**Помните:** Repository отвечает за работу с данными, Service — за бизнес-логику. Они дополняют друг друга.

---

*Документ создан для объяснения Repository Pattern с практическими примерами на C#.*

