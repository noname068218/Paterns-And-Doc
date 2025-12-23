# Factory Pattern (Фабрика) - Паттерн проектирования

## Введение

**Factory Pattern (Паттерн Фабрика)** — это порождающий паттерн проектирования, который предоставляет интерфейс для создания объектов без указания их конкретных классов. Factory инкапсулирует логику создания объектов и делает код более гибким и расширяемым.

---

## Что такое Factory Pattern?

### Определение

Factory Pattern предоставляет способ создания объектов без указания точного класса объекта, который будет создан. Вместо использования оператора `new` напрямую, мы используем фабричный метод, который сам решает, какой класс создать.

### Зачем нужен Factory Pattern?

**Проблема:** Жёсткая связь кода с конкретными классами делает систему негибкой и сложной для расширения.

**Решение:** Factory инкапсулирует логику создания объектов, позволяя изменять и расширять её без изменения клиентского кода.

---

## Когда использовать Factory вместо new?

### ❌ Проблема: Прямое создание объектов

```csharp
// ❌ ПЛОХО: Жёсткая связь с конкретными классами
public class OrderProcessor {
    public void ProcessPayment(string paymentType, decimal amount) {
        IPayment payment;
        
        // Прямое создание объектов - сложно расширять
        if (paymentType == "CreditCard") {
            payment = new CreditCardPayment();
        }
        else if (paymentType == "PayPal") {
            payment = new PayPalPayment();
        }
        else if (paymentType == "BankTransfer") {
            payment = new BankTransferPayment();
        }
        else {
            throw new ArgumentException("Unknown payment type");
        }
        
        payment.Process(amount);
    }
}

// Проблемы:
// 1. Нарушение Open/Closed Principle - нужно изменять код для добавления новых типов
// 2. Дублирование логики создания в разных местах
// 3. Сложно тестировать - нужно мокать все классы
// 4. Жёсткая связь с конкретными реализациями
```

### ✅ Решение: Factory Pattern

```csharp
// ✅ ХОРОШО: Использование Factory
public class OrderProcessor {
    private readonly IPaymentFactory _paymentFactory;
    
    public OrderProcessor(IPaymentFactory paymentFactory) {
        _paymentFactory = paymentFactory;
    }
    
    public void ProcessPayment(string paymentType, decimal amount) {
        // Factory скрывает логику создания
        IPayment payment = _paymentFactory.CreatePayment(paymentType);
        payment.Process(amount);
    }
}

// Преимущества:
// 1. Легко добавить новый тип платежа - меняем только Factory
// 2. Логика создания в одном месте
// 3. Легко тестировать - мокаем только Factory
// 4. Слабая связь с конкретными классами
```

---

## Типы Factory Pattern

### 1. Simple Factory (Простая фабрика)

Самая простая форма Factory Pattern - один класс с методом создания.

```csharp
// Интерфейс для создаваемых объектов
public interface IPayment {
    void Process(decimal amount);
}

// Конкретные реализации
public class CreditCardPayment : IPayment {
    public void Process(decimal amount) {
        Console.WriteLine($"Processing credit card payment: {amount}");
    }
}

public class PayPalPayment : IPayment {
    public void Process(decimal amount) {
        Console.WriteLine($"Processing PayPal payment: {amount}");
    }
}

public class BankTransferPayment : IPayment {
    public void Process(decimal amount) {
        Console.WriteLine($"Processing bank transfer: {amount}");
    }
}

// ✅ Простая фабрика
public class PaymentFactory {
    public static IPayment CreatePayment(string paymentType) {
        return paymentType.ToLower() switch {
            "creditcard" => new CreditCardPayment(),
            "paypal" => new PayPalPayment(),
            "banktransfer" => new BankTransferPayment(),
            _ => throw new ArgumentException($"Unsupported payment type: {paymentType}")
        };
    }
}

// Использование
var payment = PaymentFactory.CreatePayment("CreditCard");
payment.Process(100.50m);
```

#### С использованием Enum

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
            _ => throw new ArgumentException($"Unsupported payment type: {type}")
        };
    }
}

// Использование
var payment = PaymentFactory.CreatePayment(PaymentType.CreditCard);
payment.Process(100.50m);
```

### 2. Factory Method Pattern (Фабричный метод)

Определяет интерфейс для создания объекта, но позволяет подклассам решать, какой класс инстанцировать.

```csharp
// Абстрактный класс Creator с фабричным методом
public abstract class PaymentProcessor {
    // Фабричный метод - создаёт объект
    protected abstract IPayment CreatePayment();
    
    // Бизнес-логика использует созданный объект
    public void ProcessPayment(decimal amount) {
        IPayment payment = CreatePayment();
        payment.Process(amount);
    }
}

// Конкретные создатели
public class CreditCardProcessor : PaymentProcessor {
    protected override IPayment CreatePayment() {
        return new CreditCardPayment();
    }
}

public class PayPalProcessor : PaymentProcessor {
    protected override IPayment CreatePayment() {
        return new PayPalPayment();
    }
}

// Использование
var processor = new CreditCardProcessor();
processor.ProcessPayment(100.50m);
```

### 3. Abstract Factory Pattern (Абстрактная фабрика)

Предоставляет интерфейс для создания семейств связанных объектов без указания их конкретных классов.

```csharp
// Абстрактная фабрика
public interface IPaymentFactory {
    IPayment CreatePayment();
    IReceipt CreateReceipt();
}

// Конкретные фабрики
public class CreditCardFactory : IPaymentFactory {
    public IPayment CreatePayment() {
        return new CreditCardPayment();
    }
    
    public IReceipt CreateReceipt() {
        return new CreditCardReceipt();
    }
}

public class PayPalFactory : IPaymentFactory {
    public IPayment CreatePayment() {
        return new PayPalPayment();
    }
    
    public IReceipt CreateReceipt() {
        return new PayPalReceipt();
    }
}

// Использование
IPaymentFactory factory = new CreditCardFactory();
var payment = factory.CreatePayment();
var receipt = factory.CreateReceipt();
```

---

## Практические примеры

### Пример 1: Factory для создания подключений к БД

```csharp
public interface IDatabaseConnection {
    void Connect();
    void ExecuteQuery(string query);
}

public class SqlServerConnection : IDatabaseConnection {
    private readonly string _connectionString;
    
    public SqlServerConnection(string connectionString) {
        _connectionString = connectionString;
    }
    
    public void Connect() {
        Console.WriteLine("Connected to SQL Server");
    }
    
    public void ExecuteQuery(string query) {
        Console.WriteLine($"Executing query on SQL Server: {query}");
    }
}

public class MySqlConnection : IDatabaseConnection {
    private readonly string _connectionString;
    
    public MySqlConnection(string connectionString) {
        _connectionString = connectionString;
    }
    
    public void Connect() {
        Console.WriteLine("Connected to MySQL");
    }
    
    public void ExecuteQuery(string query) {
        Console.WriteLine($"Executing query on MySQL: {query}");
    }
}

// Factory для создания подключений
public class DatabaseConnectionFactory {
    public static IDatabaseConnection CreateConnection(
        string dbType, 
        string connectionString
    ) {
        return dbType.ToLower() switch {
            "sqlserver" => new SqlServerConnection(connectionString),
            "mysql" => new MySqlConnection(connectionString),
            _ => throw new ArgumentException($"Unsupported database type: {dbType}")
        };
    }
}

// Использование
var connection = DatabaseConnectionFactory.CreateConnection(
    "SQLServer", 
    "Server=localhost;Database=MyDB;"
);
connection.Connect();
connection.ExecuteQuery("SELECT * FROM Users");
```

### Пример 2: Factory с конфигурацией

```csharp
public interface ILogger {
    void Log(string message);
}

public class FileLogger : ILogger {
    private readonly string _filePath;
    
    public FileLogger(string filePath) {
        _filePath = filePath;
    }
    
    public void Log(string message) {
        File.AppendAllText(_filePath, $"{DateTime.Now}: {message}\n");
    }
}

public class ConsoleLogger : ILogger {
    public void Log(string message) {
        Console.WriteLine($"{DateTime.Now}: {message}");
    }
}

public class DatabaseLogger : ILogger {
    private readonly string _connectionString;
    
    public DatabaseLogger(string connectionString) {
        _connectionString = connectionString;
    }
    
    public void Log(string message) {
        // Логирование в БД
        Console.WriteLine($"Logging to database: {message}");
    }
}

// Factory с конфигурацией
public class LoggerFactory {
    private readonly IConfiguration _configuration;
    
    public LoggerFactory(IConfiguration configuration) {
        _configuration = configuration;
    }
    
    public ILogger CreateLogger() {
        var loggerType = _configuration["Logging:Type"];
        var filePath = _configuration["Logging:FilePath"];
        var connectionString = _configuration["ConnectionStrings:Default"];
        
        return loggerType.ToLower() switch {
            "file" => new FileLogger(filePath),
            "console" => new ConsoleLogger(),
            "database" => new DatabaseLogger(connectionString),
            _ => throw new ArgumentException($"Unknown logger type: {loggerType}")
        };
    }
}
```

### Пример 3: Factory для создания UI компонентов

```csharp
public interface IButton {
    void Render();
    void Click();
}

public class WindowsButton : IButton {
    public void Render() {
        Console.WriteLine("Rendering Windows button");
    }
    
    public void Click() {
        Console.WriteLine("Windows button clicked");
    }
}

public class MacButton : IButton {
    public void Render() {
        Console.WriteLine("Rendering Mac button");
    }
    
    public void Click() {
        Console.WriteLine("Mac button clicked");
    }
}

public class LinuxButton : IButton {
    public void Render() {
        Console.WriteLine("Rendering Linux button");
    }
    
    public void Click() {
        Console.WriteLine("Linux button clicked");
    }
}

// Factory на основе операционной системы
public class ButtonFactory {
    public static IButton CreateButton() {
        var os = Environment.OSVersion.Platform;
        
        return os switch {
            PlatformID.Win32NT => new WindowsButton(),
            PlatformID.Unix => new LinuxButton(),
            PlatformID.MacOSX => new MacButton(),
            _ => new WindowsButton() // По умолчанию
        };
    }
}

// Использование
var button = ButtonFactory.CreateButton();
button.Render();
button.Click();
```

---

## Factory с Dependency Injection

### Использование Factory в ASP.NET Core

```csharp
// Интерфейс Factory
public interface IPaymentFactory {
    IPayment CreatePayment(string paymentType);
}

// Реализация Factory
public class PaymentFactory : IPaymentFactory {
    private readonly IServiceProvider _serviceProvider;
    
    public PaymentFactory(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }
    
    public IPayment CreatePayment(string paymentType) {
        return paymentType.ToLower() switch {
            "creditcard" => _serviceProvider.GetService<CreditCardPayment>(),
            "paypal" => _serviceProvider.GetService<PayPalPayment>(),
            _ => throw new ArgumentException($"Unsupported payment type: {paymentType}")
        };
    }
}

// Регистрация в Startup.cs или Program.cs
services.AddScoped<IPaymentFactory, PaymentFactory>();
services.AddScoped<CreditCardPayment>();
services.AddScoped<PayPalPayment>();

// Использование
public class OrderService {
    private readonly IPaymentFactory _paymentFactory;
    
    public OrderService(IPaymentFactory paymentFactory) {
        _paymentFactory = paymentFactory;
    }
    
    public void ProcessOrder(Order order) {
        var payment = _paymentFactory.CreatePayment(order.PaymentType);
        payment.Process(order.Amount);
    }
}
```

---

## Преимущества Factory Pattern

### 1. Инкапсуляция логики создания

Логика создания объектов находится в одном месте, что упрощает поддержку и изменение.

### 2. Слабая связанность

Клиентский код не зависит от конкретных классов создаваемых объектов.

### 3. Расширяемость

Легко добавить новые типы объектов без изменения существующего кода (соблюдение Open/Closed Principle).

### 4. Тестируемость

Можно легко создать mock Factory для тестирования.

### 5. Единая точка управления

Все объекты создаются в одном месте, что упрощает контроль и логирование.

---

## Недостатки Factory Pattern

### 1. Увеличение сложности

Добавляет дополнительный уровень абстракции, что может усложнить код для простых случаев.

### 2. Необходимость в Factory

Нужно создавать и поддерживать Factory класс.

### 3. Возможное нарушение SRP

Если Factory становится слишком сложной, она может нарушать Single Responsibility Principle.

---

## Когда использовать Factory Pattern?

### ✅ Используйте Factory когда:

1. **Не знаете заранее точный тип создаваемого объекта**
2. **Нужна гибкость в создании объектов** (зависит от конфигурации, входных данных)
3. **Хотите инкапсулировать сложную логику создания**
4. **Нужно создавать семейства связанных объектов**
5. **Хотите уменьшить связанность между классами**

### ❌ Не используйте Factory когда:

1. **Создание объекта простое** — `new MyClass()` достаточно
2. **Тип объекта известен заранее** и не меняется
3. **Нет необходимости в гибкости** — усложняет код без пользы
4. **Объект создаётся один раз** в одном месте

---

## Factory vs new: Когда что использовать?

### Используйте `new` когда:

```csharp
// ✅ Простое создание известного объекта
var user = new User("John", "john@example.com");
var calculator = new Calculator();
var validator = new EmailValidator();
```

### Используйте Factory когда:

```csharp
// ✅ Тип определяется во время выполнения
var payment = PaymentFactory.CreatePayment(user.PaymentPreference);

// ✅ Сложная логика создания
var connection = DatabaseConnectionFactory.CreateConnection(config);

// ✅ Нужна гибкость и расширяемость
var logger = LoggerFactory.CreateLogger(settings.LogType);
```

---

## Лучшие практики

### 1. Используйте интерфейсы

Всегда возвращайте интерфейсы из Factory, а не конкретные классы.

### 2. Валидация параметров

Проверяйте входные параметры и выбрасывайте понятные исключения.

### 3. Документация

Документируйте, какие типы поддерживает Factory.

### 4. Тестирование

Тестируйте Factory отдельно, особенно логику выбора типа объекта.

### 5. Не переусердствуйте

Не создавайте Factory для простых случаев, где достаточно `new`.

---

## Часто задаваемые вопросы

### Q: В чём разница между Factory Method и Abstract Factory?

**A:** 
- **Factory Method** — один метод для создания одного объекта
- **Abstract Factory** — набор методов для создания семейства связанных объектов

### Q: Можно ли использовать Factory со статическими методами?

**A:** Да, для простых случаев (Simple Factory). Но для тестирования и гибкости лучше использовать инстанциируемый класс с Dependency Injection.

### Q: Как Factory связан с Dependency Injection?

**A:** Factory может использоваться вместе с DI. DI контейнер может сам выступать в роли Factory, или Factory может использовать DI для разрешения зависимостей создаваемых объектов.

---

## Заключение

**Factory Pattern** — мощный инструмент для создания гибкого и расширяемого кода. Он помогает инкапсулировать логику создания объектов и уменьшить связанность между классами.

**Ключевые моменты:**
- ✅ Используйте Factory для создания объектов, тип которых определяется во время выполнения
- ✅ Инкапсулируйте сложную логику создания в Factory
- ✅ Возвращайте интерфейсы, а не конкретные классы
- ✅ Не используйте Factory для простых случаев — `new` достаточно

**Помните:** Factory Pattern — это инструмент для решения конкретных проблем, а не обязательный элемент каждого проекта. Используйте его осознанно.

---

*Документ создан для объяснения Factory Pattern с практическими примерами на C#.*

