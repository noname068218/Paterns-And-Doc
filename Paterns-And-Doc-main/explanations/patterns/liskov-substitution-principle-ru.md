# Liskov Substitution Principle (LSP) - Принцип подстановки Барбары Лисков

## Введение

**Liskov Substitution Principle (LSP)** — это третий принцип из SOLID. Он гласит, что объекты подклассов должны быть заменяемы объектами их базовых классов без нарушения функциональности программы. Другими словами, подкласс должен быть заменяемым на свой родительский класс.

---

## Что такое Liskov Substitution Principle?

### Определение

**Принцип подстановки Лисков (LSP):** объекты в программе должны быть заменяемы экземплярами их подтипов без изменения правильности выполнения программы. То есть, если `S` является подтипом `T`, то объекты типа `T` могут быть заменены объектами типа `S` без нарушения программы.

### Формальное определение

Пусть `q(x)` — свойство, верное относительно объектов `x` некоторого типа `T`. Тогда `q(y)` также должно быть верным для объектов `y` типа `S`, где `S` является подтипом типа `T`.

### Простое объяснение

Если у вас есть базовый класс `Animal` и подкласс `Dog`, то везде, где используется `Animal`, вы должны иметь возможность использовать `Dog`, и программа должна работать корректно.

---

## Правило подстановки

### Ключевое правило

**Подкласс не должен требовать от вызывающего кода больше, чем базовый класс, и не должен давать меньше, чем базовый класс.**

Это означает:
- ✅ Подкласс должен выполнять все контракты базового класса
- ✅ Подкласс не должен выбрасывать новые исключения
- ✅ Подкласс не должен ослаблять предусловия (preconditions)
- ✅ Подкласс не должен усилять постусловия (postconditions)

---

## Пример нарушения LSP

### ❌ Классический пример: Rectangle и Square

```csharp
// ❌ ПЛОХО: Нарушение LSP
public class Rectangle {
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }
    
    public int GetArea() {
        return Width * Height;
    }
}

public class Square : Rectangle {
    public override int Width {
        get => base.Width;
        set {
            base.Width = value;
            base.Height = value; // Проблема: изменяет поведение
        }
    }
    
    public override int Height {
        get => base.Height;
        set {
            base.Width = value;  // Проблема: изменяет поведение
            base.Height = value;
        }
    }
}

// Проблема в использовании:
public void TestRectangle(Rectangle rectangle) {
    rectangle.Width = 5;
    rectangle.Height = 4;
    
    // Ожидаем площадь = 20 (5 * 4)
    // Но если передать Square, получим 16 (4 * 4)!
    Assert.AreEqual(20, rectangle.GetArea()); // ❌ Падает для Square
}

// Вызов
var rectangle = new Rectangle();
TestRectangle(rectangle); // ✅ Работает

var square = new Square();
TestRectangle(square); // ❌ Нарушает ожидания - нарушение LSP!
```

**Проблема:** `Square` изменяет поведение установки свойств, что нарушает ожидания клиентского кода.

### ✅ Правильное решение

```csharp
// ✅ ХОРОШО: Правильная иерархия
public abstract class Shape {
    public abstract int GetArea();
}

public class Rectangle : Shape {
    public int Width { get; set; }
    public int Height { get; set; }
    
    public override int GetArea() {
        return Width * Height;
    }
}

public class Square : Shape {
    public int Side { get; set; }
    
    public override int GetArea() {
        return Side * Side;
    }
}

// Теперь оба класса независимы и заменяемы через Shape
public void TestShape(Shape shape) {
    // Работает корректно для любого подкласса Shape
    var area = shape.GetArea();
}
```

---

## Ещё один пример нарушения LSP

### ❌ Пример: Птицы и пингвины

```csharp
// ❌ ПЛОХО: Нарушение LSP
public class Bird {
    public virtual void Fly() {
        Console.WriteLine("Bird is flying");
    }
    
    public virtual void Eat() {
        Console.WriteLine("Bird is eating");
    }
}

public class Sparrow : Bird {
    // Воробей умеет летать - всё хорошо
    public override void Fly() {
        Console.WriteLine("Sparrow is flying");
    }
}

public class Penguin : Bird {
    // ❌ ПРОБЛЕМА: Пингвин не умеет летать!
    public override void Fly() {
        throw new InvalidOperationException("Penguins cannot fly!");
    }
}

// Проблема в использовании:
public void MakeBirdFly(Bird bird) {
    bird.Fly(); // ❌ Выбросит исключение для Penguin!
}

// Вызов
var sparrow = new Sparrow();
MakeBirdFly(sparrow); // ✅ Работает

var penguin = new Penguin();
MakeBirdFly(penguin); // ❌ Выбросит исключение - нарушение LSP!
```

**Проблема:** `Penguin` не может заменить `Bird`, потому что выбрасывает исключение при вызове `Fly()`.

### ✅ Правильное решение

```csharp
// ✅ ХОРОШО: Правильная иерархия
public abstract class Bird {
    public abstract void Eat();
}

public interface IFlyable {
    void Fly();
}

public class Sparrow : Bird, IFlyable {
    public override void Eat() {
        Console.WriteLine("Sparrow is eating");
    }
    
    public void Fly() {
        Console.WriteLine("Sparrow is flying");
    }
}

public class Penguin : Bird {
    public override void Eat() {
        Console.WriteLine("Penguin is eating");
    }
    
    // Пингвин не реализует IFlyable - это нормально
}

// Использование
public void MakeBirdFly(IFlyable flyableBird) {
    flyableBird.Fly(); // Работает только для летающих птиц
}

public void FeedBird(Bird bird) {
    bird.Eat(); // Работает для всех птиц
}
```

---

## Пример из реального мира: Коллекции

### ❌ Нарушение LSP в коллекциях

```csharp
// ❌ ПЛОХО: Нарушение LSP
public class ReadOnlyList<T> : List<T> {
    public new void Add(T item) {
        throw new NotSupportedException("This list is read-only");
    }
    
    public new void Remove(T item) {
        throw new NotSupportedException("This list is read-only");
    }
}

// Проблема:
public void ProcessList(List<int> list) {
    list.Add(1);  // ❌ Выбросит исключение для ReadOnlyList!
    list.Add(2);
    list.Add(3);
}

// Вызов
var list = new List<int>();
ProcessList(list); // ✅ Работает

var readOnlyList = new ReadOnlyList<int>();
ProcessList(readOnlyList); // ❌ Выбросит исключение - нарушение LSP!
```

**Проблема:** `ReadOnlyList` не может заменить `List`, потому что выбрасывает исключения при вызове методов добавления/удаления.

### ✅ Правильное решение

```csharp
// ✅ ХОРОШО: Правильная иерархия
public interface IReadOnlyList<T> {
    T this[int index] { get; }
    int Count { get; }
    bool Contains(T item);
}

public class List<T> : IReadOnlyList<T> {
    private readonly System.Collections.Generic.List<T> _items = new();
    
    public T this[int index] => _items[index];
    public int Count => _items.Count;
    
    public void Add(T item) {
        _items.Add(item);
    }
    
    public bool Contains(T item) {
        return _items.Contains(item);
    }
}

public class ReadOnlyList<T> : IReadOnlyList<T> {
    private readonly System.Collections.Generic.List<T> _items;
    
    public ReadOnlyList(IEnumerable<T> items) {
        _items = new System.Collections.Generic.List<T>(items);
    }
    
    public T this[int index] => _items[index];
    public int Count => _items.Count;
    
    public bool Contains(T item) {
        return _items.Contains(item);
    }
}

// Использование
public void ProcessReadOnlyList(IReadOnlyList<int> list) {
    // Работает для любого IReadOnlyList
    var count = list.Count;
    var first = list[0];
}
```

---

## Пример: Работа с файлами

### ❌ Нарушение LSP

```csharp
// ❌ ПЛОХО: Нарушение LSP
public class File {
    public virtual void Delete() {
        // Удаление файла
        Console.WriteLine("File deleted");
    }
    
    public virtual void Read() {
        Console.WriteLine("Reading file");
    }
}

public class ReadOnlyFile : File {
    // ❌ ПРОБЛЕМА: Переопределение Delete выбрасывает исключение
    public override void Delete() {
        throw new UnauthorizedAccessException("Cannot delete read-only file");
    }
}

// Проблема:
public void DeleteFile(File file) {
    file.Delete(); // ❌ Выбросит исключение для ReadOnlyFile!
}

// Вызов
var file = new File();
DeleteFile(file); // ✅ Работает

var readOnlyFile = new ReadOnlyFile();
DeleteFile(readOnlyFile); // ❌ Выбросит исключение - нарушение LSP!
```

### ✅ Правильное решение

```csharp
// ✅ ХОРОШО: Правильная иерархия
public interface IReadableFile {
    void Read();
}

public interface IDeletableFile {
    void Delete();
}

public class File : IReadableFile, IDeletableFile {
    public void Read() {
        Console.WriteLine("Reading file");
    }
    
    public void Delete() {
        Console.WriteLine("File deleted");
    }
}

public class ReadOnlyFile : IReadableFile {
    public void Read() {
        Console.WriteLine("Reading read-only file");
    }
    
    // Не реализует IDeletableFile - это правильно
}

// Использование
public void DeleteFile(IDeletableFile file) {
    file.Delete(); // Работает только для удаляемых файлов
}

public void ReadFile(IReadableFile file) {
    file.Read(); // Работает для всех читаемых файлов
}
```

---

## Признаки нарушения LSP

### 1. Подкласс выбрасывает новые исключения

```csharp
// ❌ ПЛОХО
public class Base {
    public virtual void Method() {
        // Не выбрасывает исключений
    }
}

public class Derived : Base {
    public override void Method() {
        throw new Exception(); // ❌ Новое исключение
    }
}
```

### 2. Подкласс ослабляет предусловия

```csharp
// ❌ ПЛОХО
public class Base {
    public virtual void Method(int value) {
        if (value <= 0) {
            throw new ArgumentException("Value must be positive");
        }
        // Обработка
    }
}

public class Derived : Base {
    public override void Method(int value) {
        // ❌ Убрали проверку - ослабили предусловие
        // Обработка без проверки
    }
}
```

### 3. Подкласс усиляет постусловия

```csharp
// ❌ ПЛОХО
public class Base {
    public virtual int Calculate(int a, int b) {
        return a + b; // Простое сложение
    }
}

public class Derived : Base {
    public override int Calculate(int a, int b) {
        var result = base.Calculate(a, b);
        // ❌ Добавили дополнительную логику - усиление постусловия
        if (result < 0) {
            throw new Exception("Result cannot be negative");
        }
        return result;
    }
}
```

### 4. Подкласс возвращает значения, несовместимые с базовым классом

```csharp
// ❌ ПЛОХО
public class Base {
    public virtual List<string> GetItems() {
        return new List<string> { "A", "B", "C" };
    }
}

public class Derived : Base {
    public override List<string> GetItems() {
        return null; // ❌ Возвращает null, хотя базовый класс не возвращает null
    }
}
```

---

## Правила соблюдения LSP

### 1. Контракт не должен ослабляться

Подкласс должен выполнять все контракты базового класса. Нельзя:
- Удалять проверки (ослаблять предусловия)
- Требовать больше параметров
- Выбрасывать новые исключения

### 2. Контракт не должен усиливаться излишне

Подкласс может:
- Добавлять дополнительные проверки (но не менять существующие)
- Возвращать более специфичные типы (ковариантность возвращаемого типа)
- Принимать более общие типы (контравариантность параметров)

### 3. Инварианты должны сохраняться

Инварианты (условия, которые всегда истинны) базового класса должны сохраняться в подклассе.

### 4. История не должна нарушаться

Подкласс не должен менять историю изменений объекта так, что это нарушает ожидания клиентского кода.

---

## Правильные примеры LSP

### ✅ Пример 1: Правильная иерархия коллекций

```csharp
public interface ICollection<T> {
    void Add(T item);
    bool Contains(T item);
    int Count { get; }
}

public class List<T> : ICollection<T> {
    private readonly System.Collections.Generic.List<T> _items = new();
    
    public void Add(T item) {
        _items.Add(item);
    }
    
    public bool Contains(T item) {
        return _items.Contains(item);
    }
    
    public int Count => _items.Count;
}

public class Set<T> : ICollection<T> {
    private readonly HashSet<T> _items = new();
    
    public void Add(T item) {
        _items.Add(item); // Set автоматически игнорирует дубликаты
    }
    
    public bool Contains(T item) {
        return _items.Contains(item);
    }
    
    public int Count => _items.Count;
}

// Использование - оба класса заменяемы
public void ProcessCollection(ICollection<int> collection) {
    collection.Add(1);
    collection.Add(2);
    collection.Add(3);
    
    // Работает для любого ICollection
    Console.WriteLine($"Count: {collection.Count}");
}
```

### ✅ Пример 2: Правильная иерархия платежей

```csharp
public abstract class Payment {
    public decimal Amount { get; set; }
    
    public abstract void Process();
    public abstract bool Validate();
}

public class CreditCardPayment : Payment {
    public string CardNumber { get; set; }
    
    public override void Process() {
        // Обработка кредитной карты
        Console.WriteLine($"Processing credit card payment: {Amount}");
    }
    
    public override bool Validate() {
        return !string.IsNullOrWhiteSpace(CardNumber) && 
               CardNumber.Length == 16;
    }
}

public class PayPalPayment : Payment {
    public string Email { get; set; }
    
    public override void Process() {
        // Обработка PayPal
        Console.WriteLine($"Processing PayPal payment: {Amount}");
    }
    
    public override bool Validate() {
        return !string.IsNullOrWhiteSpace(Email) && 
               Email.Contains("@");
    }
}

// Использование - оба класса заменяемы
public void ProcessPayment(Payment payment) {
    if (payment.Validate()) {
        payment.Process();
    }
}

// Вызов
var creditCard = new CreditCardPayment { Amount = 100, CardNumber = "1234567890123456" };
ProcessPayment(creditCard); // ✅ Работает

var paypal = new PayPalPayment { Amount = 100, Email = "user@example.com" };
ProcessPayment(paypal); // ✅ Работает
```

---

## LSP и полиморфизм

### Правильный полиморфизм

LSP обеспечивает правильный полиморфизм:

```csharp
public abstract class Animal {
    public abstract void MakeSound();
    public abstract void Eat();
}

public class Dog : Animal {
    public override void MakeSound() {
        Console.WriteLine("Woof!");
    }
    
    public override void Eat() {
        Console.WriteLine("Dog is eating");
    }
}

public class Cat : Animal {
    public override void MakeSound() {
        Console.WriteLine("Meow!");
    }
    
    public override void Eat() {
        Console.WriteLine("Cat is eating");
    }
}

// Использование полиморфизма - все подклассы заменяемы
public void InteractWithAnimal(Animal animal) {
    animal.MakeSound();
    animal.Eat();
}

// Вызов
InteractWithAnimal(new Dog()); // ✅ Работает
InteractWithAnimal(new Cat()); // ✅ Работает
```

---

## Преимущества соблюдения LSP

### 1. Корректный полиморфизм

Подклассы могут быть использованы везде, где используется базовый класс.

### 2. Упрощение тестирования

Легче тестировать код, работающий с базовым классом — он будет работать и с подклассами.

### 3. Расширяемость

Можно добавлять новые подклассы без изменения существующего кода.

### 4. Надёжность

Код более предсказуем и надёжен, так как подклассы не нарушают ожидания.

---

## Когда нарушается LSP?

### Типичные ситуации нарушения

1. **Подкласс выбрасывает исключения**, которые не выбрасывает базовый класс
2. **Подкласс возвращает null**, когда базовый класс не возвращает null
3. **Подкласс требует дополнительных проверок** перед вызовом методов
4. **Подкласс изменяет инварианты** базового класса
5. **Подкласс не реализует все методы** базового класса (или выбрасывает NotImplementedException)

---

## Как избежать нарушения LSP?

### 1. Используйте интерфейсы

Вместо наследования используйте композицию и интерфейсы:

```csharp
// ✅ ХОРОШО: Использование интерфейсов
public interface IFlyable {
    void Fly();
}

public interface ISwimmable {
    void Swim();
}

public class Sparrow : IFlyable {
    public void Fly() { /* ... */ }
}

public class Penguin : ISwimmable {
    public void Swim() { /* ... */ }
}
```

### 2. Используйте композицию вместо наследования

```csharp
// ✅ ХОРОШО: Композиция
public class Bird {
    public void Eat() { /* ... */ }
}

public class FlyingBird {
    private readonly Bird _bird;
    
    public FlyingBird(Bird bird) {
        _bird = bird;
    }
    
    public void Eat() => _bird.Eat();
    public void Fly() { /* ... */ }
}
```

### 3. Соблюдайте контракты

Убедитесь, что подкласс выполняет все контракты базового класса.

---

## Часто задаваемые вопросы

### Q: Всегда ли наследование нарушает LSP?

**A:** Нет, наследование не обязательно нарушает LSP. Нарушение происходит, когда подкласс не может заменить базовый класс без изменения поведения программы.

### Q: Как проверить, соблюдается ли LSP?

**A:** Попробуйте заменить все использования базового класса на подкласс и убедитесь, что программа работает корректно. Если возникают ошибки или неожиданное поведение — LSP нарушен.

### Q: LSP относится только к наследованию?

**A:** LSP относится к любым отношениям подтипизации, включая наследование классов, реализацию интерфейсов и полиморфизм.

---

## Заключение

**Liskov Substitution Principle** — важный принцип объектно-ориентированного программирования, который обеспечивает корректность полиморфизма.

**Ключевые моменты:**
- ✅ Подклассы должны быть заменяемы базовыми классами
- ✅ Подклассы не должны нарушать контракты базовых классов
- ✅ Подклассы не должны выбрасывать новые исключения
- ✅ Используйте интерфейсы и композицию для избежания нарушений
- ✅ Тестируйте заменяемость классов

**Помните:** Если подкласс не может заменить базовый класс везде, где он используется, значит LSP нарушен. Пересмотрите иерархию классов или используйте интерфейсы и композицию.

---

*Документ создан для объяснения Liskov Substitution Principle с практическими примерами на C#.*

