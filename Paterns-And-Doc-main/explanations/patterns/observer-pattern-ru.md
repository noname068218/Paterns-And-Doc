# Observer Pattern (Наблюдатель) - Паттерн проектирования

## Введение

**Observer Pattern (Паттерн Наблюдатель)** — это поведенческий паттерн проектирования, который определяет зависимость один-ко-многим между объектами. Когда один объект изменяет своё состояние, все зависимые от него объекты получают уведомление и обновляются автоматически.

---

## Что такое Observer Pattern?

### Определение

Observer Pattern определяет зависимость один-ко-многим между объектами так, что когда один объект (Subject) изменяет своё состояние, все зависящие от него объекты (Observers) автоматически уведомляются и обновляются.

### Основные компоненты

1. **Subject (Субъект)** — объект, состояние которого отслеживается
2. **Observer (Наблюдатель)** — объект, который должен быть уведомлён об изменениях
3. **Уведомление** — механизм, через который Subject уведомляет Observers

---

## Зачем нужен Observer Pattern?

### Проблема: Жёсткая связанность

```csharp
// ❌ ПЛОХО: Жёсткая связанность
public class Stock {
    private decimal _price;
    private readonly List<object> _subscribers = new();
    
    public void SetPrice(decimal price) {
        _price = price;
        
        // Проблема: нужно вручную уведомлять каждого подписчика
        // При добавлении нового подписчика нужно изменять этот код
        NotifyEmailSubscribers();
        NotifySmsSubscribers();
        UpdateUI();
        SaveToDatabase();
    }
    
    private void NotifyEmailSubscribers() {
        // Отправка email
    }
    
    private void NotifySmsSubscribers() {
        // Отправка SMS
    }
    
    private void UpdateUI() {
        // Обновление UI
    }
    
    private void SaveToDatabase() {
        // Сохранение в БД
    }
}

// Проблемы:
// 1. Stock знает о всех подписчиках
// 2. Сложно добавить нового подписчика
// 3. Нарушение Open/Closed Principle
// 4. Жёсткая связанность
```

### Решение: Observer Pattern

```csharp
// ✅ ХОРОШО: Использование Observer Pattern
public class Stock {
    private decimal _price;
    private readonly List<IObserver> _observers = new();
    
    public void Attach(IObserver observer) {
        _observers.Add(observer);
    }
    
    public void Detach(IObserver observer) {
        _observers.Remove(observer);
    }
    
    public void SetPrice(decimal price) {
        _price = price;
        Notify(); // Уведомляем всех наблюдателей
    }
    
    private void Notify() {
        foreach (var observer in _observers) {
            observer.Update(_price);
        }
    }
}

// Теперь легко добавлять новых наблюдателей без изменения класса Stock
```

---

## Базовая структура Observer Pattern

### Классическая реализация

```csharp
// 1. Интерфейс Observer
public interface IObserver {
    void Update(object data);
}

// 2. Интерфейс Subject
public interface ISubject {
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify();
}

// 3. Конкретный Subject
public class Stock : ISubject {
    private decimal _price;
    private readonly List<IObserver> _observers = new();
    
    public decimal Price {
        get => _price;
        set {
            _price = value;
            Notify(); // Уведомляем при изменении
        }
    }
    
    public void Attach(IObserver observer) {
        if (!_observers.Contains(observer)) {
            _observers.Add(observer);
        }
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

// 4. Конкретные Observers
public class EmailNotifier : IObserver {
    private readonly string _email;
    
    public EmailNotifier(string email) {
        _email = email;
    }
    
    public void Update(object data) {
        if (data is decimal price) {
            Console.WriteLine($"Sending email to {_email}: Stock price changed to {price}");
        }
    }
}

public class SmsNotifier : IObserver {
    private readonly string _phoneNumber;
    
    public SmsNotifier(string phoneNumber) {
        _phoneNumber = phoneNumber;
    }
    
    public void Update(object data) {
        if (data is decimal price) {
            Console.WriteLine($"Sending SMS to {_phoneNumber}: Stock price changed to {price}");
        }
    }
}

public class PriceDisplay : IObserver {
    public void Update(object data) {
        if (data is decimal price) {
            Console.WriteLine($"Price Display updated: {price}");
        }
    }
}

// Использование
var stock = new Stock();

var emailNotifier = new EmailNotifier("user@example.com");
var smsNotifier = new SmsNotifier("+1234567890");
var priceDisplay = new PriceDisplay();

// Подписка на уведомления
stock.Attach(emailNotifier);
stock.Attach(smsNotifier);
stock.Attach(priceDisplay);

// Изменение цены - все наблюдатели уведомлены автоматически
stock.Price = 100.50m;
// Output:
// Sending email to user@example.com: Stock price changed to 100.50
// Sending SMS to +1234567890: Stock price changed to 100.50
// Price Display updated: 100.50

// Отписка
stock.Detach(emailNotifier);

stock.Price = 105.00m;
// Output:
// Sending SMS to +1234567890: Stock price changed to 105.00
// Price Display updated: 105.00
```

---

## Улучшенная реализация с типизацией

### Generic Observer Pattern

```csharp
// Generic интерфейсы
public interface IObserver<T> {
    void Update(T data);
}

public interface ISubject<T> {
    void Attach(IObserver<T> observer);
    void Detach(IObserver<T> observer);
    void Notify();
}

// Generic Subject
public class Stock : ISubject<decimal> {
    private decimal _price;
    private readonly List<IObserver<decimal>> _observers = new();
    
    public decimal Price {
        get => _price;
        set {
            if (_price != value) {
                _price = value;
                Notify();
            }
        }
    }
    
    public void Attach(IObserver<decimal> observer) {
        if (observer != null && !_observers.Contains(observer)) {
            _observers.Add(observer);
        }
    }
    
    public void Detach(IObserver<decimal> observer) {
        _observers.Remove(observer);
    }
    
    public void Notify() {
        foreach (var observer in _observers.ToList()) { // ToList() для безопасной итерации
            observer.Update(_price);
        }
    }
}

// Типизированные Observers
public class EmailNotifier : IObserver<decimal> {
    private readonly string _email;
    
    public EmailNotifier(string email) {
        _email = email;
    }
    
    public void Update(decimal price) {
        Console.WriteLine($"Email to {_email}: Stock price is now {price:C}");
        // Реальная отправка email
    }
}
```

---

## Практические примеры

### Пример 1: Система уведомлений о заказах

```csharp
// Event данные
public class OrderEventArgs : EventArgs {
    public int OrderId { get; set; }
    public string Status { get; set; }
    public decimal TotalAmount { get; set; }
}

// Subject - Заказ
public class Order {
    private string _status;
    private readonly List<IOrderObserver> _observers = new();
    
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }
    
    public string Status {
        get => _status;
        set {
            if (_status != value) {
                var oldStatus = _status;
                _status = value;
                NotifyObservers(oldStatus, _status);
            }
        }
    }
    
    public void Attach(IOrderObserver observer) {
        if (!_observers.Contains(observer)) {
            _observers.Add(observer);
        }
    }
    
    public void Detach(IOrderObserver observer) {
        _observers.Remove(observer);
    }
    
    private void NotifyObservers(string oldStatus, string newStatus) {
        var eventArgs = new OrderEventArgs {
            OrderId = Id,
            Status = newStatus,
            TotalAmount = TotalAmount
        };
        
        foreach (var observer in _observers.ToList()) {
            observer.OnOrderStatusChanged(this, oldStatus, newStatus, eventArgs);
        }
    }
}

// Observer интерфейс
public interface IOrderObserver {
    void OnOrderStatusChanged(Order order, string oldStatus, string newStatus, OrderEventArgs args);
}

// Конкретные Observers
public class EmailNotificationService : IOrderObserver {
    public void OnOrderStatusChanged(Order order, string oldStatus, string newStatus, OrderEventArgs args) {
        Console.WriteLine($"Sending email: Order {args.OrderId} changed from {oldStatus} to {newStatus}");
        // Реальная отправка email
    }
}

public class SmsNotificationService : IOrderObserver {
    public void OnOrderStatusChanged(Order order, string oldStatus, string newStatus, OrderEventArgs args) {
        if (newStatus == "Shipped") {
            Console.WriteLine($"Sending SMS: Your order {args.OrderId} has been shipped!");
            // Реальная отправка SMS
        }
    }
}

public class OrderHistoryService : IOrderObserver {
    public void OnOrderStatusChanged(Order order, string oldStatus, string newStatus, OrderEventArgs args) {
        Console.WriteLine($"Logging to history: Order {args.OrderId} status change: {oldStatus} → {newStatus}");
        // Сохранение в историю
    }
}

// Использование
var order = new Order {
    Id = 12345,
    TotalAmount = 299.99m,
    Status = "Pending"
};

var emailService = new EmailNotificationService();
var smsService = new SmsNotificationService();
var historyService = new OrderHistoryService();

order.Attach(emailService);
order.Attach(smsService);
order.Attach(historyService);

// Изменение статуса - все наблюдатели уведомлены
order.Status = "Processing";
// Output:
// Sending email: Order 12345 changed from Pending to Processing
// Logging to history: Order 12345 status change: Pending → Processing

order.Status = "Shipped";
// Output:
// Sending email: Order 12345 changed from Processing to Shipped
// Sending SMS: Your order 12345 has been shipped!
// Logging to history: Order 12345 status change: Processing → Shipped
```

### Пример 2: Система мониторинга температуры

```csharp
// Subject - Датчик температуры
public class TemperatureSensor : ISubject<float> {
    private float _temperature;
    private readonly List<IObserver<float>> _observers = new();
    
    public float Temperature {
        get => _temperature;
        set {
            if (Math.Abs(_temperature - value) > 0.1f) { // Изменение больше 0.1°C
                _temperature = value;
                Notify();
            }
        }
    }
    
    public void Attach(IObserver<float> observer) {
        if (!_observers.Contains(observer)) {
            _observers.Add(observer);
        }
    }
    
    public void Detach(IObserver<float> observer) {
        _observers.Remove(observer);
    }
    
    public void Notify() {
        foreach (var observer in _observers.ToList()) {
            observer.Update(_temperature);
        }
    }
}

// Observers
public class TemperatureDisplay : IObserver<float> {
    public void Update(float temperature) {
        Console.WriteLine($"Display: Current temperature is {temperature:F1}°C");
    }
}

public class TemperatureAlarm : IObserver<float> {
    private readonly float _threshold;
    
    public TemperatureAlarm(float threshold) {
        _threshold = threshold;
    }
    
    public void Update(float temperature) {
        if (temperature > _threshold) {
            Console.WriteLine($"⚠️ ALARM: Temperature {temperature:F1}°C exceeds threshold {_threshold}°C!");
            // Запуск аварийной сигнализации
        }
    }
}

public class TemperatureLogger : IObserver<float> {
    private readonly string _logFile;
    
    public TemperatureLogger(string logFile) {
        _logFile = logFile;
    }
    
    public void Update(float temperature) {
        var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Temperature: {temperature:F1}°C\n";
        File.AppendAllText(_logFile, logEntry);
        Console.WriteLine($"Logged: {logEntry.Trim()}");
    }
}

// Использование
var sensor = new TemperatureSensor();

var display = new TemperatureDisplay();
var alarm = new TemperatureAlarm(threshold: 30.0f);
var logger = new TemperatureLogger("temperature.log");

sensor.Attach(display);
sensor.Attach(alarm);
sensor.Attach(logger);

// Изменение температуры - все наблюдатели уведомлены
sensor.Temperature = 25.5f;
// Output:
// Display: Current temperature is 25.5°C
// Logged: 2024-01-15 10:30:00 - Temperature: 25.5°C

sensor.Temperature = 31.0f;
// Output:
// Display: Current temperature is 31.0°C
// ⚠️ ALARM: Temperature 31.0°C exceeds threshold 30.0°C!
// Logged: 2024-01-15 10:31:00 - Temperature: 31.0°C
```

---

## Observer Pattern в .NET: Events и Delegates

### Использование встроенных Events в C#

```csharp
// Использование встроенных механизмов C#
public class Stock {
    private decimal _price;
    
    // Event для уведомления наблюдателей
    public event EventHandler<PriceChangedEventArgs> PriceChanged;
    
    public decimal Price {
        get => _price;
        set {
            if (_price != value) {
                var oldPrice = _price;
                _price = value;
                OnPriceChanged(oldPrice, _price);
            }
        }
    }
    
    protected virtual void OnPriceChanged(decimal oldPrice, decimal newPrice) {
        PriceChanged?.Invoke(this, new PriceChangedEventArgs(oldPrice, newPrice));
    }
}

// EventArgs для передачи данных
public class PriceChangedEventArgs : EventArgs {
    public decimal OldPrice { get; }
    public decimal NewPrice { get; }
    
    public PriceChangedEventArgs(decimal oldPrice, decimal newPrice) {
        OldPrice = oldPrice;
        NewPrice = newPrice;
    }
}

// Использование
var stock = new Stock();

// Подписка на событие
stock.PriceChanged += (sender, args) => {
    Console.WriteLine($"Price changed from {args.OldPrice} to {args.NewPrice}");
};

stock.PriceChanged += (sender, args) => {
    if (args.NewPrice > 100) {
        Console.WriteLine("Price exceeded 100!");
    }
};

// Изменение цены - все подписчики уведомлены
stock.Price = 100.50m;
// Output:
// Price changed from 0 to 100.50
// Price exceeded 100!
```

### IObservable и IObserver интерфейсы

.NET предоставляет встроенные интерфейсы для Observer Pattern:

```csharp
using System;
using System.Collections.Generic;

// Использование IObservable<T> и IObserver<T>
public class Stock : IObservable<decimal> {
    private decimal _price;
    private readonly List<IObserver<decimal>> _observers = new();
    
    public decimal Price {
        get => _price;
        set {
            if (_price != value) {
                _price = value;
                NotifyObservers();
            }
        }
    }
    
    public IDisposable Subscribe(IObserver<decimal> observer) {
        if (!_observers.Contains(observer)) {
            _observers.Add(observer);
        }
        return new Unsubscriber(_observers, observer);
    }
    
    private void NotifyObservers() {
        foreach (var observer in _observers.ToList()) {
            observer.OnNext(_price);
        }
    }
    
    // Внутренний класс для отписки
    private class Unsubscriber : IDisposable {
        private readonly List<IObserver<decimal>> _observers;
        private readonly IObserver<decimal> _observer;
        
        public Unsubscriber(List<IObserver<decimal>> observers, IObserver<decimal> observer) {
            _observers = observers;
            _observer = observer;
        }
        
        public void Dispose() {
            if (_observers.Contains(_observer)) {
                _observers.Remove(_observer);
            }
        }
    }
}

// Реализация IObserver
public class PriceObserver : IObserver<decimal> {
    public void OnNext(decimal price) {
        Console.WriteLine($"Current price: {price}");
    }
    
    public void OnError(Exception error) {
        Console.WriteLine($"Error: {error.Message}");
    }
    
    public void OnCompleted() {
        Console.WriteLine("Price updates completed");
    }
}

// Использование
var stock = new Stock();
var observer = new PriceObserver();

var subscription = stock.Subscribe(observer);

stock.Price = 100.50m;
// Output: Current price: 100.50

stock.Price = 105.00m;
// Output: Current price: 105.00

// Отписка
subscription.Dispose();
```

---

## Преимущества Observer Pattern

### 1. Слабая связанность

Subject не знает конкретные классы Observers, только интерфейс.

### 2. Динамическая подписка

Можно добавлять и удалять Observers во время выполнения.

### 3. Открытость для расширения

Легко добавить новых Observers без изменения Subject.

### 4. Принцип единственной ответственности

Subject отвечает за управление состоянием, Observers — за реакцию на изменения.

---

## Недостатки Observer Pattern

### 1. Непредсказуемый порядок уведомлений

Порядок вызова Observers может быть непредсказуемым.

### 2. Проблемы с производительностью

Если Observers выполняют тяжёлые операции, это может замедлить Subject.

### 3. Утечки памяти

Если Observer не отписывается, может возникнуть утечка памяти (особенно в событиях .NET).

### 4. Сложность отладки

Цепочки уведомлений могут быть сложными для отладки.

---

## Когда использовать Observer Pattern?

### ✅ Используйте Observer когда:

1. **Изменение одного объекта требует изменения других объектов**
2. **Количество зависимых объектов заранее неизвестно**
3. **Нужна слабая связанность между объектами**
4. **Нужна система уведомлений или событий**
5. **Реализуете модель MVC/MVP/MVVM**

### ❌ Не используйте Observer когда:

1. **Простая связь один-к-одному** — прямой вызов метода проще
2. **Критична производительность** — уведомления могут быть медленными
3. **Порядок обработки важен** — Observer не гарантирует порядок

---

## Практические сценарии использования

### 1. GUI приложения (Model-View)

```csharp
// Model
public class DataModel {
    private string _data;
    public event EventHandler<string> DataChanged;
    
    public string Data {
        get => _data;
        set {
            _data = value;
            DataChanged?.Invoke(this, _data);
        }
    }
}

// View
public class DataView {
    public void DisplayData(object sender, string data) {
        Console.WriteLine($"View updated: {data}");
    }
}

// Использование
var model = new DataModel();
var view = new DataView();

model.DataChanged += view.DisplayData;
model.Data = "New Data";
// Output: View updated: New Data
```

### 2. Система логирования

```csharp
public class Logger {
    private readonly List<ILogObserver> _observers = new();
    
    public void Attach(ILogObserver observer) {
        _observers.Add(observer);
    }
    
    public void Log(string message, LogLevel level) {
        foreach (var observer in _observers) {
            observer.OnLog(message, level);
        }
    }
}

public interface ILogObserver {
    void OnLog(string message, LogLevel level);
}

public class ConsoleLogger : ILogObserver {
    public void OnLog(string message, LogLevel level) {
        Console.WriteLine($"[{level}] {message}");
    }
}

public class FileLogger : ILogObserver {
    public void OnLog(string message, LogLevel level) {
        File.AppendAllText("app.log", $"[{level}] {message}\n");
    }
}
```

---

## Лучшие практики

### 1. Используйте встроенные Events в C#

Для простых случаев используйте события C#:

```csharp
public event EventHandler<EventArgs> SomethingHappened;
```

### 2. Правильно обрабатывайте отписки

Всегда предоставляйте способ отписки (IDisposable или метод Detach).

### 3. Защита от исключений

Обрабатывайте исключения в Observers, чтобы не сломать другие уведомления:

```csharp
public void Notify() {
    foreach (var observer in _observers.ToList()) {
        try {
            observer.Update(_price);
        }
        catch (Exception ex) {
            // Логируем, но продолжаем уведомлять остальных
            _logger.LogError(ex, "Error notifying observer");
        }
    }
}
```

### 4. Уведомляйте только при реальных изменениях

Проверяйте, действительно ли значение изменилось:

```csharp
public decimal Price {
    set {
        if (_price != value) { // Проверка изменения
            _price = value;
            Notify();
        }
    }
}
```

---

## Часто задаваемые вопросы

### Q: В чём разница между Observer Pattern и Events в C#?

**A:** Events в C# — это встроенная реализация Observer Pattern. Events проще использовать для простых случаев, классический Observer Pattern даёт больше контроля.

### Q: Можно ли использовать Observer для синхронных операций?

**A:** Да, но будьте осторожны. Если Observer выполняет долгие операции, это блокирует Subject. Используйте асинхронные паттерны для долгих операций.

### Q: Как избежать утечек памяти при использовании Events?

**A:** Всегда отписывайтесь от событий, когда объект больше не нужен. Используйте слабые ссылки (WeakEvent) для долгоживущих Subject.

---

## Заключение

**Observer Pattern** — мощный паттерн для реализации системы уведомлений и событий. Он позволяет создавать слабо связанные системы, которые легко расширять.

**Ключевые моменты:**
- ✅ Определяет зависимость один-ко-многим
- ✅ Автоматическое уведомление наблюдателей
- ✅ Слабая связанность между компонентами
- ✅ Используйте Events в C# для простых случаев
- ✅ Правильно обрабатывайте отписки и исключения

**Помните:** Observer Pattern — основа для событийных систем, MVC паттернов и реактивного программирования. Используйте его осознанно, когда нужна гибкая система уведомлений.

---

*Документ создан для объяснения Observer Pattern с практическими примерами на C#.*

