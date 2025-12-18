# Delegates и Events в C#

## Введение

**Delegates** и **Events** — это фундаментальные механизмы в C# для реализации паттерна Observer, callback-паттернов и слабо связанной коммуникации между компонентами.

---

## 1. Что такое Delegate?

### Определение

**Delegate** — это тип, представляющий ссылки на методы с определённой сигнатурой. Он позволяет обращаться с методами как с сущностями, которые можно присваивать переменным и передавать в качестве параметров.

### Аналогия

```
┌─────────────────────────────────────────────┐
│  Delegate = "Function pointer"              │
│                                              │
│  Like a variable that contains              │
│  a reference to a method                    │
└─────────────────────────────────────────────┘
```

### Базовый синтаксис

```csharp
// Delegate declaration
public delegate void MyDelegate(string message);

// Compatible method
public void PrintMessage(string msg) {
    Console.WriteLine(msg);
}

// Usage
MyDelegate del = PrintMessage;
del("Hello!"); // Calls PrintMessage("Hello!")
```

---

## 2. Типы Delegates

### Пользовательский Delegate

```csharp
// Delegate that accepts an int and returns an int
public delegate int MathOperation(int a, int b);

// Compatible methods
public int Add(int a, int b) => a + b;
public int Multiply(int a, int b) => a * b;

// Usage
MathOperation operation = Add;
int result = operation(5, 3); // 8

operation = Multiply;
result = operation(5, 3); // 15
```

### Action (Delegate без возврата)

```csharp
// Action<T> - accepts parameters, no return
Action<string> printAction = (msg) => Console.WriteLine(msg);
printAction("Hello");

// Action with multiple parameters
Action<int, int> addAction = (a, b) => Console.WriteLine(a + b);
addAction(5, 3); // 8
```

### Func (Delegate с возвратом)

```csharp
// Func<T, TResult> - accepts parameters and returns value
Func<int, int> square = (x) => x * x;
int result = square(5); // 25

// Func with multiple parameters
Func<int, int, int> add = (a, b) => a + b;
int sum = add(5, 3); // 8

// Func with multiple input parameters
Func<int, int, string> format = (a, b) => $"{a} + {b} = {a + b}";
string output = format(5, 3); // "5 + 3 = 8"
```

### Predicate (Delegate, возвращающий bool)

```csharp
// Predicate<T> - accepts one parameter, returns bool
Predicate<int> isEven = (x) => x % 2 == 0;
bool result = isEven(4); // true

// Usage with List
var numbers = new List<int> { 1, 2, 3, 4, 5 };
var evens = numbers.FindAll(isEven); // { 2, 4 }
```

### Диаграмма: Типы Delegates

```
┌─────────────────────────────────────────────┐
│           DELEGATES                         │
└─────────────────────────────────────────────┘
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│  Action   │ │   Func    │ │ Predicate │
│           │ │           │ │           │
│ void      │ │ TResult   │ │ bool      │
│ Method()  │ │ Method()  │ │ Method()  │
└───────────┘ └───────────┘ └───────────┘
```

---

## 3. Multicast Delegates

### Концепция

Delegate может содержать ссылки на **несколько методов**. При вызове все методы вызываются последовательно.

```csharp
public delegate void Notifier(string message);

public void SendEmail(string msg) {
    Console.WriteLine($"Email: {msg}");
}

public void SendSMS(string msg) {
    Console.WriteLine($"SMS: {msg}");
}

// Multicast delegate
Notifier notifier = SendEmail;
notifier += SendSMS; // Add another method
notifier += (msg) => Console.WriteLine($"Log: {msg}"); // Lambda

notifier("Important notification");
// Output:
// Email: Important notification
// SMS: Important notification
// Log: Important notification
```

### Удаление методов

```csharp
Notifier notifier = SendEmail;
notifier += SendSMS;

notifier -= SendEmail; // Remove SendEmail
notifier("Test"); // Calls only SendSMS
```

### Диаграмма: Multicast Delegate

```
┌─────────────────────────────────────────────┐
│  Notifier delegate                          │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐ │
│  │ SendEmail│→ │ SendSMS  │→ │   Log    │ │
│  └──────────┘  └──────────┘  └──────────┘ │
└─────────────────────────────────────────────┘
                    │
                    ▼ Invoke()
        ┌───────────────────────┐
        │  Execute all methods  │
        │  in sequence          │
        └───────────────────────┘
```

---

## 4. Lambda-выражения

### Синтаксис Lambda

```csharp
// Lambda expression - compact syntax
Func<int, int> square = x => x * x;

// Lambda with multiple parameters
Func<int, int, int> add = (a, b) => a + b;

// Lambda with multi-line body
Func<int, int> factorial = n => {
    int result = 1;
    for (int i = 1; i <= n; i++) {
        result *= i;
    }
    return result;
};
```

### Lambda vs Традиционные методы

```csharp
// Traditional method
public int Add(int a, int b) {
    return a + b;
}

// Equivalent lambda
Func<int, int, int> add = (a, b) => a + b;

// Identical usage
int result1 = Add(5, 3);
int result2 = add(5, 3);
```

---

## 5. Events (События)

### Определение

**Event** — это специальная обёртка вокруг delegate, добавляющая контроль доступа. События позволяют безопасно реализовать паттерн Observer.

### Проблема с публичным Delegate

```csharp
// ❌ PROBLEM: Public delegate can be overwritten
public class Button {
    public Action Click; // Anyone can do: button.Click = null;
}

// Dangerous usage
var button = new Button();
button.Click = null; // Removes all subscribers!
```

### Решение: Event

```csharp
// ✅ SOLUTION: Event protects delegate
public class Button {
    private Action _click;
    
    public event Action Click {
        add { _click += value; }
        remove { _click -= value; }
    }
    
    public void OnClick() {
        _click?.Invoke();
    }
}

// Safe usage
var button = new Button();
button.Click += HandleClick; // ✅ OK
// button.Click = null; // ❌ ERROR! Not allowed
```

### Упрощённый синтаксис

```csharp
// ✅ Simplified version (most common)
public class Button {
    public event Action Click;
    
    public void OnClick() {
        Click?.Invoke();
    }
}
```

### Диаграмма: Event Pattern

```
┌─────────────────────────────────────────────┐
│  Publisher (Button)                         │
│  - event Action Click                       │
│  + OnClick()                                │
└─────────────────────────────────────────────┘
                    │
                    │ Subscribe (+=)
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│ Handler 1 │ │ Handler 2 │ │ Handler 3 │
│           │ │           │ │           │
│ OnClick() │ │ OnClick() │ │ OnClick() │
└───────────┘ └───────────┘ └───────────┘
```

---

## 6. Стандартный паттерн Event Handler

### EventHandler<T>

```csharp
// Event data class
public class ButtonClickEventArgs : EventArgs {
    public DateTime ClickTime { get; set; }
    public string ButtonName { get; set; }
}

// Publisher
public class Button {
    public event EventHandler<ButtonClickEventArgs> Click;
    
    public void OnClick(string buttonName) {
        Click?.Invoke(this, new ButtonClickEventArgs {
            ClickTime = DateTime.Now,
            ButtonName = buttonName
        });
    }
}

// Subscriber
public class Form {
    private Button button;
    
    public Form() {
        button = new Button();
        button.Click += Button_Click; // Subscribe
    }
    
    private void Button_Click(object sender, ButtonClickEventArgs e) {
        Console.WriteLine($"Button {e.ButtonName} clicked at {e.ClickTime}");
    }
}
```

### Стандартный EventHandler (без пользовательских данных)

```csharp
public class Button {
    public event EventHandler Click;
    
    public void OnClick() {
        Click?.Invoke(this, EventArgs.Empty);
    }
}

// Usage
button.Click += (sender, e) => {
    Console.WriteLine("Button clicked!");
};
```

---

## 7. Полные практические примеры

### Пример 1: Система уведомлений

```csharp
public class NotificationService {
    public event Action<string> NotificationSent;
    
    public void SendNotification(string message) {
        // Sending logic
        Console.WriteLine($"Sending notification: {message}");
        
        // Notify subscribers
        NotificationSent?.Invoke(message);
    }
}

// Usage
var service = new NotificationService();
service.NotificationSent += (msg) => Console.WriteLine($"Email: {msg}");
service.NotificationSent += (msg) => Console.WriteLine($"SMS: {msg}");
service.NotificationSent += (msg) => Console.WriteLine($"Log: {msg}");

service.SendNotification("New message");
```

### Пример 2: Таймер с событиями

```csharp
public class Timer {
    public event Action<int> Tick;
    public event Action Completed;
    
    public async Task StartAsync(int seconds) {
        for (int i = seconds; i > 0; i--) {
            Tick?.Invoke(i);
            await Task.Delay(1000);
        }
        Completed?.Invoke();
    }
}

// Usage
var timer = new Timer();
timer.Tick += (remaining) => Console.WriteLine($"Time remaining: {remaining}s");
timer.Completed += () => Console.WriteLine("Timer completed!");

await timer.StartAsync(5);
```

### Пример 3: Observer Pattern с событиями

```csharp
public class Stock {
    private decimal _price;
    
    public event EventHandler<PriceChangedEventArgs> PriceChanged;
    
    public decimal Price {
        get => _price;
        set {
            if (_price != value) {
                decimal oldPrice = _price;
                _price = value;
                PriceChanged?.Invoke(this, new PriceChangedEventArgs {
                    OldPrice = oldPrice,
                    NewPrice = value
                });
            }
        }
    }
}

public class PriceChangedEventArgs : EventArgs {
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
}

// Usage
var stock = new Stock();
stock.PriceChanged += (sender, e) => {
    Console.WriteLine($"Price changed from {e.OldPrice:C} to {e.NewPrice:C}");
};

stock.Price = 100.50m; // Trigger event
```

---

## 8. Delegates как параметры

### Callback Pattern

```csharp
public class DataProcessor {
    // Accepts delegate as parameter
    public void ProcessData(int[] data, Func<int, int> operation) {
        for (int i = 0; i < data.Length; i++) {
            data[i] = operation(data[i]);
        }
    }
}

// Usage
var processor = new DataProcessor();
var numbers = new[] { 1, 2, 3, 4, 5 };

// Pass lambda as callback
processor.ProcessData(numbers, x => x * 2);
// numbers now: { 2, 4, 6, 8, 10 }

processor.ProcessData(numbers, x => x + 10);
// numbers now: { 12, 14, 16, 18, 20 }
```

### LINQ с Delegates

```csharp
var numbers = new[] { 1, 2, 3, 4, 5 };

// LINQ uses delegates internally
var evens = numbers.Where(x => x % 2 == 0);
var doubled = numbers.Select(x => x * 2);
var sum = numbers.Aggregate((acc, x) => acc + x);
```

---

## 9. Best Practices

### ✅ Что делать

1. **Используйте event вместо публичного delegate**
   ```csharp
   // ✅ CORRECT
   public event Action Click;
   ```

2. **Используйте EventHandler<T> для стандартных событий**
   ```csharp
   // ✅ CORRECT
   public event EventHandler<CustomEventArgs> SomethingHappened;
   ```

3. **Проверяйте на null перед вызовом**
   ```csharp
   // ✅ CORRECT
   Click?.Invoke();
   ```

4. **Отписывайтесь, когда больше не нужно**
   ```csharp
   // ✅ CORRECT
   button.Click -= HandleClick;
   ```

### ❌ Чего избегать

1. **Не выставляйте публичные delegates**
   ```csharp
   // ❌ WRONG
   public Action Click;
   
   // ✅ CORRECT
   public event Action Click;
   ```

2. **Не забывайте отписываться**
   ```csharp
   // ❌ WRONG - Potential memory leak
   button.Click += HandleClick;
   // Never removed!
   
   // ✅ CORRECT
   button.Click += HandleClick;
   // ...
   button.Click -= HandleClick; // Cleanup
   ```

3. **Не вызывайте события из конструктора**
   ```csharp
   // ❌ WRONG - Subscribers might not be ready
   public Button() {
       Click?.Invoke();
   }
   ```

---

## 10. Сравнение: Delegate vs Event

### Сравнительная таблица

| Характеристика | Delegate | Event |
|----------------|----------|-------|
| **Внешний доступ** | Публичный (может быть перезаписан) | Защищён (только += и -=) |
| **Основное использование** | Callback, параметры | Observer pattern |
| **Безопасность** | Менее безопасен | Более безопасен |
| **Multicast** | ✅ Да | ✅ Да |
| **Когда использовать** | Параметры методов | Коммуникация между компонентами |

---

## 11. Часто задаваемые вопросы (FAQ)

### Q: В чём разница между delegate и event?
**A:** `event` — это обёртка вокруг delegate, добавляющая контроль доступа. События можно подписать только с помощью `+=` и `-=`, их нельзя перезаписать.

### Q: Когда использовать Action vs Func vs Predicate?
**A:** 
- `Action` для методов void
- `Func` для методов с возвратом
- `Predicate` для методов, возвращающих bool (специфично для условий)

### Q: Как избежать утечек памяти с событиями?
**A:** Всегда отписывайтесь (`-=`), когда объект-подписчик больше не нужен, особенно если publisher живёт дольше.

### Q: Могу ли я передать несколько параметров в event?
**A:** Да, используя `EventHandler<CustomEventArgs>`, где `CustomEventArgs` содержит все необходимые данные.

---

## Заключение

Delegates и Events являются фундаментальными для:
- ✅ Реализации паттерна Observer
- ✅ Разделения компонентов
- ✅ Создания гибких callback-ов
- ✅ Управления коммуникацией между объектами

Используйте **delegates** для callback-ов и параметров, **events** для коммуникации между компонентами!

---

*Документ создан для объяснения Delegates и Events в C# с практическими примерами и best practices.*

