# Attributes и Reflection в C#

## Введение

**Attributes (Атрибуты)** — это способ добавления метаданных к типам, методам, свойствам и другим элементам кода. **Reflection (Рефлексия)** — это механизм для интроспекции (изучения) типов, методов и свойств во время выполнения. Эти концепции критически важны для понимания работы фреймворков и создания гибкого кода.

---

## 1. Attributes (Атрибуты)

### Что такое Attributes?

Attributes — это декларативные метки, которые добавляют метаданные к элементам кода. Они не влияют напрямую на выполнение кода, но могут использоваться через Reflection.

```csharp
using System;

// Применение атрибута к классу
[Serializable]
public class MyClass
{
    [Obsolete("Use NewMethod instead")]
    public void OldMethod() { }
    
    public void NewMethod() { }
}

// Атрибуты предоставляют информацию о коде
// [Serializable] - указывает, что класс можно сериализовать
// [Obsolete] - предупреждает, что метод устарел
```

### Встроенные Attributes

```csharp
using System;
using System.Diagnostics;

// 1. Obsolete - помечает элемент как устаревший
[Obsolete("This method is deprecated. Use NewMethod() instead.")]
public void OldMethod() { }

[Obsolete("This is an error", true)] // true = ошибка компиляции, false = предупреждение
public void VeryOldMethod() { }

// 2. Conditional - метод вызывается только при определенном условии
[Conditional("DEBUG")]
public void DebugMethod()
{
    Console.WriteLine("This only runs in DEBUG mode");
}

// 3. Serializable - класс можно сериализовать
[Serializable]
public class SerializableClass
{
    public int Value { get; set; }
}

// 4. NonSerialized - поле не сериализуется
[Serializable]
public class Person
{
    public string Name { get; set; }
    
    [NonSerialized]
    private int _age; // Не будет сериализовано
}

// 5. DllImport - импорт функции из DLL
[System.Runtime.InteropServices.DllImport("user32.dll")]
public static extern int MessageBox(int hWnd, string text, string caption, int type);
```

---

## 2. Создание собственных Attributes

### Базовый кастомный атрибут

```csharp
using System;

// Создание кастомного атрибута
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class AuthorAttribute : Attribute
{
    public string Name { get; }
    public string Version { get; set; }
    
    public AuthorAttribute(string name)
    {
        Name = name;
    }
}

// Использование
[Author("John Doe", Version = "1.0")]
public class MyClass
{
    [Author("Jane Smith")]
    public void MyMethod() { }
}
```

### AttributeUsage параметры

```csharp
using System;

[AttributeUsage(
    AttributeTargets.Class |      // Может применяться к классам
    AttributeTargets.Method |     // и методам
    AttributeTargets.Property,    // и свойствам
    AllowMultiple = true,          // Можно применять несколько раз
    Inherited = true              // Наследуется производными классами
)]
public class MyCustomAttribute : Attribute
{
    public string Description { get; set; }
}

// AllowMultiple = true позволяет:
[MyCustom(Description = "First")]
[MyCustom(Description = "Second")]
public class MyClass { }
```

### Практический пример: Атрибут для валидации

```csharp
using System;
using System.Reflection;

// Атрибут для валидации диапазона
[AttributeUsage(AttributeTargets.Property)]
public class RangeAttribute : Attribute
{
    public int Min { get; }
    public int Max { get; }
    
    public RangeAttribute(int min, int max)
    {
        Min = min;
        Max = max;
    }
}

// Класс с валидацией
public class Product
{
    [Range(1, 100)]
    public int Quantity { get; set; }
    
    [Range(0, 1000)]
    public decimal Price { get; set; }
}

// Валидатор использует Reflection для проверки атрибутов
public class Validator
{
    public static bool Validate(object obj)
    {
        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties();
        
        foreach (var property in properties)
        {
            var rangeAttr = property.GetCustomAttribute<RangeAttribute>();
            if (rangeAttr != null)
            {
                var value = Convert.ToInt32(property.GetValue(obj));
                if (value < rangeAttr.Min || value > rangeAttr.Max)
                {
                    Console.WriteLine($"{property.Name} is out of range [{rangeAttr.Min}, {rangeAttr.Max}]");
                    return false;
                }
            }
        }
        
        return true;
    }
}

// Использование
var product = new Product { Quantity = 150, Price = 50 };
bool isValid = Validator.Validate(product); // False: Quantity out of range
```

---

## 3. Reflection - Основы

### Получение информации о типах

```csharp
using System;
using System.Reflection;

class ReflectionBasics
{
    static void Main()
    {
        Type type = typeof(string); // Получение типа
        
        // Информация о типе
        Console.WriteLine($"Name: {type.Name}");
        Console.WriteLine($"FullName: {type.FullName}");
        Console.WriteLine($"Namespace: {type.Namespace}");
        Console.WriteLine($"IsClass: {type.IsClass}");
        Console.WriteLine($"IsValueType: {type.IsValueType}");
        
        // Методы
        MethodInfo[] methods = type.GetMethods();
        Console.WriteLine($"\nMethods count: {methods.Length}");
        foreach (var method in methods)
        {
            Console.WriteLine($"  - {method.Name}");
        }
        
        // Свойства
        PropertyInfo[] properties = type.GetProperties();
        Console.WriteLine($"\nProperties count: {properties.Length}");
        foreach (var prop in properties)
        {
            Console.WriteLine($"  - {prop.Name} ({prop.PropertyType.Name})");
        }
        
        // Поля
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
        Console.WriteLine($"\nPublic static fields: {fields.Length}");
    }
}
```

### Создание экземпляров через Reflection

```csharp
using System;
using System.Reflection;

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    public Person() { }
    
    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }
    
    public void Introduce()
    {
        Console.WriteLine($"Hi, I'm {Name}, {Age} years old");
    }
}

class ReflectionInstantiation
{
    static void Main()
    {
        Type personType = typeof(Person);
        
        // Создание экземпляра через конструктор без параметров
        object person1 = Activator.CreateInstance(personType);
        Console.WriteLine($"Created: {person1}");
        
        // Создание через конструктор с параметрами
        object person2 = Activator.CreateInstance(personType, "Alice", 30);
        Person p = (Person)person2;
        p.Introduce();
        
        // Альтернативный способ: получение конструктора
        ConstructorInfo constructor = personType.GetConstructor(new Type[] { typeof(string), typeof(int) });
        object person3 = constructor.Invoke(new object[] { "Bob", 25 });
        ((Person)person3).Introduce();
    }
}
```

### Вызов методов через Reflection

```csharp
using System;
using System.Reflection;

public class Calculator
{
    public int Add(int a, int b) => a + b;
    public int Multiply(int a, int b) => a * b;
    
    private int Subtract(int a, int b) => a - b;
    
    public static int Divide(int a, int b) => a / b;
}

class MethodInvocation
{
    static void Main()
    {
        Calculator calc = new Calculator();
        Type type = calc.GetType();
        
        // Вызов публичного метода
        MethodInfo addMethod = type.GetMethod("Add");
        object result = addMethod.Invoke(calc, new object[] { 10, 20 });
        Console.WriteLine($"Add result: {result}"); // 30
        
        // Вызов приватного метода
        MethodInfo subtractMethod = type.GetMethod("Subtract", BindingFlags.NonPublic | BindingFlags.Instance);
        object result2 = subtractMethod.Invoke(calc, new object[] { 20, 5 });
        Console.WriteLine($"Subtract result: {result2}"); // 15
        
        // Вызов статического метода
        MethodInfo divideMethod = type.GetMethod("Divide", BindingFlags.Public | BindingFlags.Static);
        object result3 = divideMethod.Invoke(null, new object[] { 20, 4 }); // null для статического
        Console.WriteLine($"Divide result: {result3}"); // 5
    }
}
```

### Работа со свойствами через Reflection

```csharp
using System;
using System.Reflection;

public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    private int Stock { get; set; }
    
    public Product(string name, decimal price)
    {
        Name = name;
        Price = price;
        Stock = 100;
    }
}

class PropertyReflection
{
    static void Main()
    {
        var product = new Product("Laptop", 999.99m);
        Type type = product.GetType();
        
        // Получение и установка публичных свойств
        PropertyInfo nameProperty = type.GetProperty("Name");
        string name = (string)nameProperty.GetValue(product);
        Console.WriteLine($"Name: {name}");
        
        nameProperty.SetValue(product, "Gaming Laptop");
        Console.WriteLine($"New Name: {product.Name}");
        
        // Получение приватных свойств
        PropertyInfo stockProperty = type.GetProperty("Stock", BindingFlags.NonPublic | BindingFlags.Instance);
        int stock = (int)stockProperty.GetValue(product);
        Console.WriteLine($"Stock: {stock}");
        
        // Получение всех свойств
        PropertyInfo[] allProperties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var prop in allProperties)
        {
            Console.WriteLine($"{prop.Name}: {prop.GetValue(product)}");
        }
    }
}
```

---

## 4. Практические примеры

### Пример 1: ORM-like mapping с атрибутами

```csharp
using System;
using System.Reflection;

// Атрибут для маппинга колонок БД
[AttributeUsage(AttributeTargets.Property)]
public class ColumnAttribute : Attribute
{
    public string Name { get; }
    public bool IsPrimaryKey { get; set; }
    
    public ColumnAttribute(string name)
    {
        Name = name;
    }
}

// Класс с маппингом
public class User
{
    [Column("user_id", IsPrimaryKey = true)]
    public int Id { get; set; }
    
    [Column("user_name")]
    public string Name { get; set; }
    
    [Column("email")]
    public string Email { get; set; }
    
    // Свойство без атрибута - не маппится
    public DateTime CreatedAt { get; set; }
}

// Генератор SQL через Reflection
public class SqlGenerator
{
    public static string GenerateSelect<T>() where T : class
    {
        Type type = typeof(T);
        var columns = new List<string>();
        
        foreach (var prop in type.GetProperties())
        {
            var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
            if (columnAttr != null)
            {
                columns.Add(columnAttr.Name);
            }
        }
        
        return $"SELECT {string.Join(", ", columns)} FROM {type.Name.ToLower()}s";
    }
    
    public static string GenerateInsert<T>(T entity) where T : class
    {
        Type type = typeof(T);
        var columns = new List<string>();
        var values = new List<string>();
        
        foreach (var prop in type.GetProperties())
        {
            var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
            if (columnAttr != null && !columnAttr.IsPrimaryKey)
            {
                columns.Add(columnAttr.Name);
                var value = prop.GetValue(entity);
                values.Add($"'{value}'");
            }
        }
        
        return $"INSERT INTO {type.Name.ToLower()}s ({string.Join(", ", columns)}) VALUES ({string.Join(", ", values)})";
    }
}

// Использование
var user = new User { Id = 1, Name = "Alice", Email = "alice@example.com" };
Console.WriteLine(SqlGenerator.GenerateSelect<User>());
Console.WriteLine(SqlGenerator.GenerateInsert(user));
```

### Пример 2: Dependency Injection через Reflection

```csharp
using System;
using System.Reflection;

// Атрибут для автоматической инъекции
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class InjectAttribute : Attribute { }

// Сервисы
public interface ILogger
{
    void Log(string message);
}

public class ConsoleLogger : ILogger
{
    public void Log(string message) => Console.WriteLine($"[LOG] {message}");
}

public interface IEmailService
{
    void SendEmail(string to, string message);
}

public class SmtpEmailService : IEmailService
{
    public void SendEmail(string to, string message)
    {
        Console.WriteLine($"Sending email to {to}: {message}");
    }
}

// Класс с зависимостями
public class UserService
{
    [Inject]
    private ILogger _logger;
    
    [Inject]
    public IEmailService EmailService { get; set; }
    
    public void RegisterUser(string email)
    {
        _logger?.Log($"Registering user: {email}");
        EmailService?.SendEmail(email, "Welcome!");
    }
}

// Простой DI контейнер через Reflection
public class SimpleContainer
{
    private readonly Dictionary<Type, object> _services = new();
    
    public void Register<TInterface, TImplementation>() where TImplementation : TInterface, new()
    {
        _services[typeof(TInterface)] = new TImplementation();
    }
    
    public T Resolve<T>() where T : new()
    {
        T instance = new T();
        Type type = typeof(T);
        
        // Инъекция в поля
        foreach (var field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public))
        {
            if (field.GetCustomAttribute<InjectAttribute>() != null)
            {
                if (_services.TryGetValue(field.FieldType, out object service))
                {
                    field.SetValue(instance, service);
                }
            }
        }
        
        // Инъекция в свойства
        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (prop.GetCustomAttribute<InjectAttribute>() != null && prop.CanWrite)
            {
                if (_services.TryGetValue(prop.PropertyType, out object service))
                {
                    prop.SetValue(instance, service);
                }
            }
        }
        
        return instance;
    }
}

// Использование
var container = new SimpleContainer();
container.Register<ILogger, ConsoleLogger>();
container.Register<IEmailService, SmtpEmailService>();

var userService = container.Resolve<UserService>();
userService.RegisterUser("user@example.com");
```

---

## 5. Performance Considerations

### Reflection медленный - кеширование помогает

```csharp
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

class ReflectionPerformance
{
    // ❌ Медленно: получение метода каждый раз
    static void SlowApproach(object obj, string methodName, object[] args)
    {
        Type type = obj.GetType();
        MethodInfo method = type.GetMethod(methodName);
        method.Invoke(obj, args);
    }
    
    // ✅ Быстро: кеширование MethodInfo
    private static readonly Dictionary<string, MethodInfo> _methodCache = new();
    
    static void FastApproach(object obj, string methodName, object[] args)
    {
        string key = $"{obj.GetType().FullName}.{methodName}";
        
        if (!_methodCache.TryGetValue(key, out MethodInfo method))
        {
            method = obj.GetType().GetMethod(methodName);
            _methodCache[key] = method;
        }
        
        method.Invoke(obj, args);
    }
    
    // ✅ Еще быстрее: делегаты
    private static readonly Dictionary<string, Delegate> _delegateCache = new();
    
    static void FastestApproach(object obj, string methodName, object[] args)
    {
        string key = $"{obj.GetType().FullName}.{methodName}";
        
        if (!_delegateCache.TryGetValue(key, out Delegate del))
        {
            MethodInfo method = obj.GetType().GetMethod(methodName);
            del = Delegate.CreateDelegate(typeof(Action<int, int>), obj, method);
            _delegateCache[key] = del;
        }
        
        del.DynamicInvoke(args);
    }
}
```

---

## 6. Best Practices

### ✅ Что делать

1. **Кешируйте результаты Reflection**
   ```csharp
   // Кешируйте Type, MethodInfo, PropertyInfo и т.д.
   ```

2. **Используйте BindingFlags правильно**
   ```csharp
   // Указывайте конкретные флаги вместо получения всего
   type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
   ```

3. **Используйте GetCustomAttribute<T> для одного атрибута**
   ```csharp
   var attr = prop.GetCustomAttribute<MyAttribute>();
   ```

4. **Используйте делегаты для часто вызываемых методов**
   ```csharp
   // Создавайте делегаты один раз, вызывайте много раз
   ```

### ❌ Чего избегать

1. **Не используйте Reflection для часто вызываемого кода**
   ```csharp
   // Reflection медленный, избегайте в hot paths
   ```

2. **Не забывайте обрабатывать исключения**
   ```csharp
   // MethodInfo.Invoke может выбросить исключение
   ```

3. **Не используйте GetMethods() без BindingFlags если нужны конкретные методы**
   ```csharp
   // Это возвращает только публичные методы по умолчанию
   ```

---

## 7. Вопросы для собеседований

**Q1: Что такое Attributes?**
- Метаданные, добавляемые к элементам кода
- Не влияют на выполнение напрямую
- Используются через Reflection

**Q2: Что такое Reflection?**
- Механизм интроспекции типов во время выполнения
- Позволяет создавать экземпляры, вызывать методы, получать свойства
- Медленный, но гибкий

**Q3: Какой атрибут используется для сериализации?**
- [Serializable] - для классов
- [NonSerialized] - для исключения полей

**Q4: Как создать экземпляр класса через Reflection?**
- Activator.CreateInstance(type)
- ConstructorInfo.Invoke()

**Q5: Почему Reflection медленный?**
- Динамический поиск типов и методов
- Нет оптимизаций компилятора
- Решение: кеширование результатов

---

## Заключение

Attributes и Reflection - мощные инструменты для:
- Добавления метаданных к коду
- Создания гибких фреймворков
- Автоматизации задач (ORM, DI, валидация)

Помните: **Reflection медленный - используйте его осторожно и кешируйте результаты!**
