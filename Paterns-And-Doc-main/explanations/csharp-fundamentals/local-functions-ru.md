# Локальные функции (Local Functions) в C#

## Введение

**Локальные функции** (Local Functions) — это функции, определённые внутри другого метода, конструктора или свойства. Они были введены в C# 7.0 и позволяют инкапсулировать логику внутри метода, делая код более читаемым и организованным.

---

## 1. Что такое локальные функции?

### Определение

Локальная функция — это метод, определённый внутри другого метода. Она доступна только внутри того метода, где определена, и может обращаться к переменным внешнего метода.

### Базовый синтаксис

```csharp
public void OuterMethod() {
    // Локальная функция
    void LocalFunction() {
        Console.WriteLine("Это локальная функция");
    }
    
    // Вызов локальной функции
    LocalFunction();
}
```

### Диаграмма: Область видимости

```
┌─────────────────────────────────────────────┐
│  OuterMethod()                              │
│  │                                          │
│  │  ┌──────────────────────────────────┐   │
│  │  │  LocalFunction()                 │   │
│  │  │  - Доступна только внутри       │   │
│  │  │    OuterMethod()                 │   │
│  │  │  - Может использовать переменные│   │
│  │  │    из OuterMethod()              │   │
│  │  └──────────────────────────────────┘   │
│  │                                          │
└─────────────────────────────────────────────┘
```

---

## 2. Зачем нужны локальные функции?

### Проблема: Вспомогательные методы в классе

```csharp
// ❌ ПРОБЛЕМА: Вспомогательный метод доступен всему классу
public class Calculator {
    public int Calculate(int a, int b) {
        return HelperMethod(a, b);
    }
    
    // Этот метод доступен везде, хотя нужен только в Calculate
    private int HelperMethod(int a, int b) {
        return a + b;
    }
}
```

### Решение: Локальная функция

```csharp
// ✅ РЕШЕНИЕ: Локальная функция инкапсулирована в методе
public class Calculator {
    public int Calculate(int a, int b) {
        // Локальная функция доступна только здесь
        int HelperMethod(int x, int y) {
            return x + y;
        }
        
        return HelperMethod(a, b);
    }
}
```

---

## 3. Базовые примеры

### Пример 1: Простая локальная функция

```csharp
public void ProcessData(int[] numbers) {
    // Локальная функция для вычисления суммы
    int CalculateSum(int[] nums) {
        int sum = 0;
        foreach (int num in nums) {
            sum += num;
        }
        return sum;
    }
    
    int total = CalculateSum(numbers);
    Console.WriteLine($"Сумма: {total}");
}
```

### Пример 2: Локальная функция с параметрами

```csharp
public void ValidateUser(string name, int age) {
    // Локальная функция с параметрами
    bool IsValidName(string n) {
        return !string.IsNullOrWhiteSpace(n) && n.Length >= 3;
    }
    
    bool IsValidAge(int a) {
        return a >= 0 && a <= 150;
    }
    
    if (!IsValidName(name)) {
        Console.WriteLine("Неверное имя");
        return;
    }
    
    if (!IsValidAge(age)) {
        Console.WriteLine("Неверный возраст");
        return;
    }
    
    Console.WriteLine("Пользователь валиден");
}
```

---

## 4. Доступ к переменным внешнего метода

### Захват переменных (Closure)

```csharp
public void ProcessItems(List<string> items) {
    int processedCount = 0; // Переменная внешнего метода
    
    // Локальная функция может использовать переменные внешнего метода
    void ProcessItem(string item) {
        Console.WriteLine($"Обработка: {item}");
        processedCount++; // Изменяет переменную внешнего метода
    }
    
    foreach (string item in items) {
        ProcessItem(item);
    }
    
    Console.WriteLine($"Обработано элементов: {processedCount}");
}
```

### Пример: Рекурсивная локальная функция

```csharp
public int CalculateFactorial(int n) {
    // Рекурсивная локальная функция
    int Factorial(int num) {
        if (num <= 1) {
            return 1;
        }
        return num * Factorial(num - 1);
    }
    
    return Factorial(n);
}

// Использование
int result = CalculateFactorial(5); // 120
```

---

## 5. Локальные функции vs Лямбда-выражения

### Сравнение

```csharp
public void CompareApproaches() {
    int multiplier = 10;
    
    // Лямбда-выражение
    Func<int, int> lambda = x => x * multiplier;
    
    // Локальная функция
    int LocalFunction(int x) {
        return x * multiplier;
    }
    
    // Оба работают одинаково
    int result1 = lambda(5);        // 50
    int result2 = LocalFunction(5); // 50
}
```

### Когда использовать локальные функции

```csharp
// ✅ Локальные функции лучше для:
// 1. Рекурсии
public int Fibonacci(int n) {
    int Fib(int num) {
        if (num <= 1) return num;
        return Fib(num - 1) + Fib(num - 2);
    }
    return Fib(n);
}

// 2. Iterator methods (yield)
public IEnumerable<int> GetNumbers() {
    int GetNext() {
        yield return 1;
        yield return 2;
        yield return 3;
    }
    return GetNext();
}

// 3. Async methods
public async Task ProcessAsync() {
    async Task<string> FetchDataAsync() {
        await Task.Delay(1000);
        return "Data";
    }
    return await FetchDataAsync();
}
```

---

## 6. Практические примеры

### Пример 1: Валидация с несколькими правилами

```csharp
public bool ValidateOrder(Order order) {
    // Локальные функции для каждого правила валидации
    bool IsValidAmount(decimal amount) {
        return amount > 0 && amount <= 1000000;
    }
    
    bool IsValidDate(DateTime date) {
        return date >= DateTime.Today;
    }
    
    bool IsValidCustomer(string customerId) {
        return !string.IsNullOrWhiteSpace(customerId) && 
               customerId.Length >= 5;
    }
    
    // Использование локальных функций
    if (!IsValidAmount(order.Amount)) {
        Console.WriteLine("Неверная сумма");
        return false;
    }
    
    if (!IsValidDate(order.DeliveryDate)) {
        Console.WriteLine("Неверная дата");
        return false;
    }
    
    if (!IsValidCustomer(order.CustomerId)) {
        Console.WriteLine("Неверный ID клиента");
        return false;
    }
    
    return true;
}
```

### Пример 2: Обработка данных с вложенными функциями

```csharp
public void ProcessUserData(List<User> users) {
    // Внешняя локальная функция
    void ProcessUser(User user) {
        // Внутренняя локальная функция
        string FormatName(string firstName, string lastName) {
            return $"{firstName} {lastName}".Trim();
        }
        
        // Ещё одна внутренняя функция
        bool IsActiveUser(User u) {
            return u.IsActive && u.LastLoginDate > DateTime.Now.AddDays(-30);
        }
        
        string fullName = FormatName(user.FirstName, user.LastName);
        bool isActive = IsActiveUser(user);
        
        Console.WriteLine($"{fullName}: {(isActive ? "Активен" : "Неактивен")}");
    }
    
    foreach (var user in users) {
        ProcessUser(user);
    }
}
```

### Пример 3: Рекурсивный обход дерева

```csharp
public class TreeNode {
    public int Value { get; set; }
    public List<TreeNode> Children { get; set; } = new List<TreeNode>();
}

public void TraverseTree(TreeNode root) {
    // Рекурсивная локальная функция
    void Traverse(TreeNode node, int depth) {
        string indent = new string(' ', depth * 2);
        Console.WriteLine($"{indent}{node.Value}");
        
        foreach (var child in node.Children) {
            Traverse(child, depth + 1);
        }
    }
    
    Traverse(root, 0);
}
```

---

## 7. Локальные функции с async/await

### Асинхронные локальные функции

```csharp
public async Task<string> ProcessDataAsync(string input) {
    // Асинхронная локальная функция
    async Task<string> ValidateAndProcessAsync(string data) {
        await Task.Delay(100); // Симуляция асинхронной операции
        
        if (string.IsNullOrEmpty(data)) {
            throw new ArgumentException("Данные не могут быть пустыми");
        }
        
        return data.ToUpper();
    }
    
    // Ещё одна асинхронная локальная функция
    async Task<string> SaveAsync(string processedData) {
        await Task.Delay(200);
        // Сохранение данных
        return $"Сохранено: {processedData}";
    }
    
    string validated = await ValidateAndProcessAsync(input);
    return await SaveAsync(validated);
}
```

### Пример: Параллельная обработка

```csharp
public async Task<List<string>> ProcessItemsAsync(List<string> items) {
    // Локальная функция для обработки одного элемента
    async Task<string> ProcessItemAsync(string item) {
        await Task.Delay(100);
        return $"Обработано: {item}";
    }
    
    var tasks = items.Select(item => ProcessItemAsync(item));
    return (await Task.WhenAll(tasks)).ToList();
}
```

---

## 8. Локальные функции с yield

### Iterator methods

```csharp
public IEnumerable<int> GetFilteredNumbers(int[] numbers, int threshold) {
    // Локальная функция-итератор
    IEnumerable<int> FilterNumbers() {
        foreach (int num in numbers) {
            if (num > threshold) {
                yield return num;
            }
        }
    }
    
    return FilterNumbers();
}

// Использование
int[] numbers = { 1, 5, 10, 15, 20 };
var filtered = GetFilteredNumbers(numbers, 10);
foreach (int num in filtered) {
    Console.WriteLine(num); // 15, 20
}
```

---

## 9. Статические локальные функции (C# 8.0+)

### Статические локальные функции

```csharp
public void ProcessData(int[] numbers) {
    int multiplier = 10; // Переменная внешнего метода
    
    // Обычная локальная функция (может использовать multiplier)
    int Multiply(int x) {
        return x * multiplier; // ✅ Может использовать multiplier
    }
    
    // Статическая локальная функция (НЕ может использовать multiplier)
    static int Add(int x, int y) {
        // return x * multiplier; // ❌ ОШИБКА! Не может использовать multiplier
        return x + y;
    }
    
    // Статическая функция не захватывает переменные из внешнего метода
    // Это может улучшить производительность
}
```

### Когда использовать static

```csharp
public void Calculate(int[] numbers) {
    // ✅ Используйте static, если не нужен доступ к переменным внешнего метода
    static int Square(int x) {
        return x * x;
    }
    
    // ✅ Это предотвращает захват переменных и может улучшить производительность
    var squared = numbers.Select(Square).ToArray();
}
```

---

## 10. Best Practices

### ✅ Что делать

1. **Используйте локальные функции для инкапсуляции логики**
   ```csharp
   public void ComplexMethod() {
       // Локальная функция скрывает сложность
       void HelperMethod() {
           // Сложная логика
       }
       HelperMethod();
   }
   ```

2. **Используйте для рекурсии**
   ```csharp
   public int Calculate(int n) {
       int Recursive(int num) {
           // Рекурсивная логика
       }
       return Recursive(n);
   }
   ```

3. **Используйте static, если не нужен доступ к переменным**
   ```csharp
   static int Helper(int x) {
       // Не захватывает переменные
   }
   ```

### ❌ Чего избегать

1. **Не создавайте слишком сложные локальные функции**
   ```csharp
   // ❌ ПЛОХО: Слишком сложная локальная функция
   public void Method() {
       void ComplexFunction() {
           // 100+ строк кода - лучше вынести в отдельный метод
       }
   }
   ```

2. **Не используйте, если функция нужна в нескольких местах**
   ```csharp
   // ❌ ПЛОХО: Дублирование кода
   public void Method1() {
       void Helper() { /* ... */ }
   }
   
   public void Method2() {
       void Helper() { /* ... */ } // Дублирование!
   }
   
   // ✅ ХОРОШО: Вынести в приватный метод класса
   private void Helper() { /* ... */ }
   ```

---

## 11. Сравнение с альтернативами

### Локальные функции vs Приватные методы

```csharp
public class Example {
    // Приватный метод - доступен всему классу
    private void HelperMethod() {
        // ...
    }
    
    public void PublicMethod() {
        // Локальная функция - доступна только здесь
        void LocalHelper() {
            // ...
        }
        LocalHelper();
    }
}
```

### Локальные функции vs Лямбда-выражения

| Характеристика | Локальные функции | Лямбда-выражения |
|----------------|-------------------|------------------|
| **Рекурсия** | ✅ Поддерживается | ❌ Сложно |
| **Iterator (yield)** | ✅ Поддерживается | ❌ Не поддерживается |
| **async/await** | ✅ Поддерживается | ✅ Поддерживается |
| **Производительность** | ✅ Лучше | ⚠️ Может быть хуже |
| **Читаемость** | ✅ Лучше для сложной логики | ✅ Лучше для простых операций |

---

## 12. Продвинутые примеры

### Пример: Кэширование с локальной функцией

```csharp
public int CalculateWithCache(int n) {
    var cache = new Dictionary<int, int>();
    
    // Рекурсивная локальная функция с кэшированием
    int Fibonacci(int num) {
        if (cache.TryGetValue(num, out int cached)) {
            return cached;
        }
        
        int result;
        if (num <= 1) {
            result = num;
        } else {
            result = Fibonacci(num - 1) + Fibonacci(num - 2);
        }
        
        cache[num] = result;
        return result;
    }
    
    return Fibonacci(n);
}
```

### Пример: Валидация с накоплением ошибок

```csharp
public ValidationResult ValidateOrder(Order order) {
    var errors = new List<string>();
    
    // Локальные функции для валидации
    void ValidateAmount(decimal amount) {
        if (amount <= 0) {
            errors.Add("Сумма должна быть больше нуля");
        }
    }
    
    void ValidateDate(DateTime date) {
        if (date < DateTime.Today) {
            errors.Add("Дата не может быть в прошлом");
        }
    }
    
    void ValidateCustomer(string customerId) {
        if (string.IsNullOrWhiteSpace(customerId)) {
            errors.Add("ID клиента обязателен");
        }
    }
    
    // Выполнение валидаций
    ValidateAmount(order.Amount);
    ValidateDate(order.DeliveryDate);
    ValidateCustomer(order.CustomerId);
    
    return new ValidationResult {
        IsValid = errors.Count == 0,
        Errors = errors
    };
}
```

---

## 13. Часто задаваемые вопросы (FAQ)

### Q: Могут ли локальные функции быть виртуальными или абстрактными?
**A:** Нет, локальные функции не могут быть виртуальными, абстрактными или переопределёнными.

### Q: Можно ли использовать локальные функции в свойствах?
**A:** Да, локальные функции можно использовать в get/set аксессорах свойств.

### Q: Влияют ли локальные функции на производительность?
**A:** Локальные функции обычно имеют такую же производительность, как обычные методы. Статические локальные функции могут быть немного быстрее, так как не захватывают переменные.

### Q: Можно ли использовать локальные функции в конструкторах?
**A:** Да, локальные функции можно использовать в конструкторах, методах, свойствах и других членах класса.

---

## Заключение

Локальные функции полезны для:

- ✅ Инкапсуляции логики внутри метода
- ✅ Реализации рекурсивных алгоритмов
- ✅ Создания iterator methods
- ✅ Улучшения читаемости кода
- ✅ Избежания загрязнения пространства имён класса

Используйте локальные функции, когда логика специфична для одного метода и не нужна в других местах!

---

*Документ создан для объяснения локальных функций в C# с практическими примерами и best practices.*

