# Single Responsibility Principle (SRP) - Принцип единственной ответственности

## Введение

**Single Responsibility Principle (SRP)** — это первый принцип из SOLID. Он гласит, что **класс должен иметь только одну причину для изменения**, или, другими словами, **класс должен отвечать только за одну задачу или ответственность**.

---

## Что такое Single Responsibility Principle?

### Определение

Класс должен иметь **только одну ответственность** — только одну причину для изменения. Это означает, что все методы и свойства класса должны быть связаны с одной концепцией или бизнес-логикой.

### Простое объяснение своими словами

**Single Responsibility Principle** означает, что каждый класс должен делать только одну вещь и делать её хорошо.

Представьте, что вы нанимаете специалистов:
- 👨‍🍳 **Повар** готовит еду
- 🏥 **Врач** лечит людей  
- 🚗 **Водитель** управляет автомобилем

Вы бы не хотели, чтобы один человек делал всё сразу, потому что:
- Он не может быть экспертом во всём
- Изменения в одной области могут сломать другую
- Сложнее тестировать и поддерживать

То же самое с классами в программировании. Каждый класс должен иметь **одну четко определённую задачу**.

---

## Проблема: Нарушение SRP

### Пример: Класс с множественными ответственностями

```csharp
// ❌ ПЛОХО: Класс нарушает SRP - делает слишком много вещей
public class User {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    
    // Ответственность 1: Работа с данными пользователя
    public void Save() {
        // Сохранение в базу данных
        using (var connection = new SqlConnection(connectionString)) {
            connection.Open();
            var command = new SqlCommand($"INSERT INTO Users (Name, Email) VALUES ('{Name}', '{Email}')", connection);
            command.ExecuteNonQuery();
        }
    }
    
    // Ответственность 2: Валидация
    public bool ValidateEmail() {
        return Email.Contains("@") && Email.Contains(".");
    }
    
    // Ответственность 3: Отправка email
    public void SendEmail(string subject, string body) {
        var smtpClient = new SmtpClient("smtp.example.com");
        var mailMessage = new MailMessage("noreply@example.com", Email, subject, body);
        smtpClient.Send(mailMessage);
    }
    
    // Ответственность 4: Логирование
    public void LogActivity(string activity) {
        File.WriteAllText("log.txt", $"{DateTime.Now}: User {Name} - {activity}");
    }
    
    // Ответственность 5: Форматирование
    public string GetFormattedInfo() {
        return $"User: {Name} ({Email})";
    }
}
```

### Проблемы этого подхода

1. **Сложно тестировать** — нужно мокать базу данных, SMTP сервер, файловую систему
2. **Сложно изменять** — изменение логики отправки email может сломать сохранение
3. **Нарушение инкапсуляции** — класс знает слишком много о других системах
4. **Невозможно переиспользовать** — нельзя использовать валидацию отдельно от сохранения

---

## Решение: Применение SRP

### Разделение на отдельные классы

```csharp
// ✅ ХОРОШО: Класс User отвечает только за данные пользователя
public class User {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}

// ✅ ХОРОШО: Отдельный класс для работы с базой данных
public class UserRepository {
    private readonly string _connectionString;
    
    public UserRepository(string connectionString) {
        _connectionString = connectionString;
    }
    
    public void Save(User user) {
        using (var connection = new SqlConnection(_connectionString)) {
            connection.Open();
            var command = new SqlCommand(
                "INSERT INTO Users (Name, Email) VALUES (@Name, @Email)", 
                connection
            );
            command.Parameters.AddWithValue("@Name", user.Name);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.ExecuteNonQuery();
        }
    }
    
    public User GetById(int id) {
        // Получение пользователя из БД
        // ...
    }
}

// ✅ ХОРОШО: Отдельный класс для валидации
public class UserValidator {
    public bool ValidateEmail(string email) {
        if (string.IsNullOrWhiteSpace(email)) {
            return false;
        }
        
        return email.Contains("@") && 
               email.Contains(".") && 
               email.Length > 5;
    }
    
    public bool ValidateName(string name) {
        return !string.IsNullOrWhiteSpace(name) && name.Length >= 2;
    }
    
    public ValidationResult Validate(User user) {
        var result = new ValidationResult();
        
        if (!ValidateEmail(user.Email)) {
            result.AddError("Invalid email");
        }
        
        if (!ValidateName(user.Name)) {
            result.AddError("Invalid name");
        }
        
        return result;
    }
}

// ✅ ХОРОШО: Отдельный класс для отправки email
public class EmailService {
    private readonly SmtpClient _smtpClient;
    
    public EmailService(SmtpClient smtpClient) {
        _smtpClient = smtpClient;
    }
    
    public void SendEmail(string to, string subject, string body) {
        var mailMessage = new MailMessage("noreply@example.com", to, subject, body);
        _smtpClient.Send(mailMessage);
    }
}

// ✅ ХОРОШО: Отдельный класс для логирования
public class Logger {
    private readonly string _logPath;
    
    public Logger(string logPath) {
        _logPath = logPath;
    }
    
    public void Log(string message) {
        File.AppendAllText(_logPath, $"{DateTime.Now}: {message}\n");
    }
}

// ✅ ХОРОШО: Отдельный класс для форматирования
public class UserFormatter {
    public string FormatUserInfo(User user) {
        return $"User: {user.Name} ({user.Email})";
    }
    
    public string FormatUserSummary(User user) {
        return $"{user.Name} - {user.Email}";
    }
}
```

### Использование разделённых классов

```csharp
// Теперь использование становится чище и понятнее
var user = new User {
    Id = 1,
    Name = "John Doe",
    Email = "john@example.com"
};

// Валидация
var validator = new UserValidator();
var validationResult = validator.Validate(user);
if (!validationResult.IsValid) {
    Console.WriteLine(string.Join(", ", validationResult.Errors));
    return;
}

// Сохранение
var repository = new UserRepository(connectionString);
repository.Save(user);

// Отправка email
var emailService = new EmailService(smtpClient);
emailService.SendEmail(user.Email, "Welcome", "Welcome to our service!");

// Логирование
var logger = new Logger("app.log");
logger.Log($"User {user.Name} registered");

// Форматирование
var formatter = new UserFormatter();
Console.WriteLine(formatter.FormatUserInfo(user));
```

---

## Практический пример: Система заказов

### Проблема: Монолитный класс Order

```csharp
// ❌ ПЛОХО: Класс Order делает слишком много
public class Order {
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string CustomerEmail { get; set; }
    
    // Расчет суммы заказа
    public void CalculateTotal(List<OrderItem> items) {
        TotalAmount = items.Sum(item => item.Price * item.Quantity);
    }
    
    // Сохранение в БД
    public void Save() {
        // SQL код для сохранения
    }
    
    // Отправка email
    public void SendConfirmationEmail() {
        // SMTP код
    }
    
    // Печать чека
    public void PrintReceipt() {
        // Код для печати
    }
    
    // Валидация
    public bool Validate() {
        // Код валидации
    }
}
```

### Решение: Разделение ответственностей

```csharp
// ✅ ХОРОШО: Класс Order - только данные
public class Order {
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string CustomerEmail { get; set; }
    public List<OrderItem> Items { get; set; }
}

// ✅ ХОРОШО: Калькулятор заказа
public class OrderCalculator {
    public decimal CalculateTotal(List<OrderItem> items) {
        return items.Sum(item => item.Price * item.Quantity);
    }
    
    public decimal CalculateTotalWithTax(List<OrderItem> items, decimal taxRate) {
        var subtotal = CalculateTotal(items);
        return subtotal * (1 + taxRate);
    }
}

// ✅ ХОРОШО: Репозиторий для работы с БД
public class OrderRepository {
    public void Save(Order order) {
        // Сохранение в БД
    }
    
    public Order GetById(int id) {
        // Получение из БД
    }
}

// ✅ ХОРОШО: Сервис отправки email
public class OrderEmailService {
    public void SendConfirmationEmail(Order order) {
        // Отправка email
    }
}

// ✅ ХОРОШО: Сервис печати
public class ReceiptPrinter {
    public void PrintReceipt(Order order) {
        // Печать чека
    }
}

// ✅ ХОРОШО: Валидатор заказа
public class OrderValidator {
    public ValidationResult Validate(Order order) {
        var result = new ValidationResult();
        
        if (order.Items == null || order.Items.Count == 0) {
            result.AddError("Order must have at least one item");
        }
        
        if (string.IsNullOrWhiteSpace(order.CustomerEmail)) {
            result.AddError("Customer email is required");
        }
        
        return result;
    }
}
```

---

## Как определить, нарушен ли SRP?

### Признаки нарушения SRP

1. **Класс имеет слишком много методов** (более 7-10 публичных методов)
2. **Класс знает о разных аспектах системы** (БД, файлы, сеть, UI)
3. **Изменения в одной части класса влияют на другие части**
4. **Сложно описать назначение класса одним предложением**
5. **Много зависимостей** (много using директив)
6. **Сложно тестировать** — нужно мокать много вещей

### Вопросы для проверки

- Можно ли описать класс одним предложением? ("Класс отвечает за...")
- Есть ли у класса только одна причина для изменения?
- Можно ли использовать части класса независимо друг от друга?
- Легко ли тестировать класс?

---

## Преимущества соблюдения SRP

### 1. Упрощение тестирования

```csharp
// ✅ Легко тестировать изолированные классы
[Test]
public void UserValidator_ShouldValidateEmail() {
    // Arrange
    var validator = new UserValidator();
    
    // Act
    var result = validator.ValidateEmail("test@example.com");
    
    // Assert
    Assert.IsTrue(result);
}

// Не нужно мокать БД, SMTP, файловую систему!
```

### 2. Упрощение изменений

```csharp
// ✅ Изменение логики email не затрагивает сохранение
public class EmailService {
    // Изменили реализацию отправки email
    // UserRepository не затронут!
}
```

### 3. Переиспользование кода

```csharp
// ✅ Валидатор можно использовать везде
var validator = new UserValidator();
var emailValidator = new EmailValidator();

// Можно использовать отдельно от сохранения
if (validator.ValidateEmail(email)) {
    // ...
}
```

### 4. Улучшение читаемости

```csharp
// ✅ Код становится самодокументируемым
var validator = new UserValidator();
var repository = new UserRepository(connectionString);
var emailService = new EmailService(smtpClient);

// Понятно, что делает каждый класс
```

---

## Когда применять SRP?

### ✅ Применяйте SRP когда:

1. **Класс становится слишком большим** (более 200-300 строк)
2. **Класс имеет несколько несвязанных методов**
3. **Сложно тестировать класс**
4. **Изменения в одной части ломают другую**
5. **Разные разработчики часто конфликтуют при изменении класса**

### ❌ Не переусердствуйте:

1. **Не создавайте класс на каждую функцию** — это приведёт к излишней фрагментации
2. **Не разделяйте связанную функциональность** — если методы работают вместе, они могут быть в одном классе
3. **Используйте здравый смысл** — SRP это инструмент, а не догма

---

## Практические рекомендации

### 1. Начните с малого

Не пытайтесь сразу разделить большой класс. Начните с выделения одной ответственности.

### 2. Используйте интерфейсы

```csharp
// ✅ Определите интерфейсы для каждой ответственности
public interface IUserRepository {
    void Save(User user);
    User GetById(int id);
}

public interface IEmailService {
    void SendEmail(string to, string subject, string body);
}

public interface ILogger {
    void Log(string message);
}
```

### 3. Используйте Dependency Injection

```csharp
// ✅ Внедряйте зависимости через конструктор
public class UserService {
    private readonly IUserRepository _repository;
    private readonly IEmailService _emailService;
    private readonly ILogger _logger;
    
    public UserService(
        IUserRepository repository,
        IEmailService emailService,
        ILogger logger
    ) {
        _repository = repository;
        _emailService = emailService;
        _logger = logger;
    }
    
    public void RegisterUser(User user) {
        _repository.Save(user);
        _emailService.SendEmail(user.Email, "Welcome", "Welcome!");
        _logger.Log($"User {user.Name} registered");
    }
}
```

### 4. Рефакторинг существующего кода

Когда встречаете большой класс:

1. Определите все его ответственности
2. Выделите каждую ответственность в отдельный класс
3. Используйте композицию вместо наследования
4. Тестируйте каждый класс отдельно

---

## Часто задаваемые вопросы

### Q: Как определить, что класс имеет слишком много ответственностей?

**A:** Если вы не можете описать класс одним простым предложением ("Этот класс отвечает за..."), вероятно, у него несколько ответственностей.

### Q: Всегда ли нужно разделять класс?

**A:** Нет. Если класс небольшой и все его методы тесно связаны, можно оставить их вместе. SRP — это руководство, а не строгое правило.

### Q: Что делать, если разделение приводит к слишком большому количеству классов?

**A:** Используйте папки и пространства имён для организации. Группируйте связанные классы вместе.

### Q: Как SRP связан с другими принципами SOLID?

**A:** SRP — основа остальных принципов. Если класс имеет одну ответственность, легче применять Open/Closed, Liskov Substitution, Interface Segregation и Dependency Inversion.

---

## Заключение

**Single Responsibility Principle** — это фундаментальный принцип проектирования, который помогает создавать чистый, поддерживаемый и тестируемый код.

**Ключевые моменты:**
- ✅ Один класс — одна ответственность
- ✅ Одна причина для изменения
- ✅ Упрощает тестирование и поддержку
- ✅ Улучшает переиспользование кода
- ✅ Используйте здравый смысл — не переусердствуйте

**Помните:** Цель SRP — улучшить качество кода, а не создать максимальное количество классов. Ищите баланс между разделением ответственностей и простотой использования.

---

*Документ создан для объяснения Single Responsibility Principle с практическими примерами на C#.*

