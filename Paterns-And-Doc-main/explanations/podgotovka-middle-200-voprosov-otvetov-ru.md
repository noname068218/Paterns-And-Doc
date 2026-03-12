# 🎯 Подготовка к собеседованию Middle Developer — 200+ вопросов и ответов

> Полный справочник вопросов и ответов для собеседования на позицию Middle .NET Developer.  
> Темы: C#, .NET, ASP.NET Web API, React, SQL Server, Entity Framework, тестирование.  
> Все ответы на **русском языке**.

---

## 🧩 OOP и SOLID — шпаргалка в начале

Краткая выжимка по четырём принципам ООП и пяти принципам SOLID с примерами на C#. На собеседовании часто просят назвать и привести примеры.

---

### OOP — 4 принципа

#### 1. Encapsulation (инкапсуляция) — прячем внутренности

**Идея:** Скрываем внутреннее состояние и детали реализации; доступ к данным только через публичный интерфейс (свойства, методы). Как в машине: крутишь руль и жмёшь газ, не зная, как устроен двигатель.

**Зачем:** Защита от некорректного использования, возможность менять реализацию без поломки клиентского кода, контроль валидации при записи.

```csharp
public class Employee
{
    // Внутреннее поле — снаружи недоступно
    private decimal _salary;

    // Доступ только через метод или свойство — можно добавить проверки
    public decimal GetSalary() => _salary;

    public void SetSalary(decimal value)
    {
        if (value < 0) throw new ArgumentException("Salary cannot be negative");
        _salary = value;
    }

    // Либо через свойство с логикой в set
    public decimal Salary
    {
        get => _salary;
        set => _salary = value >= 0 ? value : throw new ArgumentException(nameof(value));
    }
}
```

---

#### 2. Inheritance (наследование) — переиспользование и иерархия

**Идея:** Класс-потомок наследует поля и методы базового класса и может добавлять свои. «Собака — это животное»: у животного есть Eat(), у собаки дополнительно Bark().

**Зачем:** Избежание дублирования кода, выстраивание иерархии типов (IS-A отношение).

```csharp
public class Animal
{
    public void Eat() => Console.WriteLine("Eating");
}

public class Dog : Animal
{
    public void Bark() => Console.WriteLine("Woof!");
}

// Использование: Dog имеет и Eat(), и Bark()
var dog = new Dog();
dog.Eat();   // из Animal
dog.Bark();  // свой метод
```

---

#### 3. Polymorphism (полиморфизм) — один интерфейс, разное поведение

**Идея:** Один и тот же вызов метода ведёт к разному поведению в зависимости от реального типа объекта. Одна кнопка «Play» — в Spotify запускает трек, в YouTube — видео.

**Зачем:** Клиентский код работает с абстракцией (базовый класс/интерфейс), не зная конкретный тип; добавление новых типов не требует менять этот код.

```csharp
public class Animal
{
    public virtual void MakeSound() => Console.WriteLine("Some sound");
}

public class Dog : Animal
{
    public override void MakeSound() => Console.WriteLine("Woof");
}

public class Cat : Animal
{
    public override void MakeSound() => Console.WriteLine("Meow");
}

// Полиморфизм: переменная типа Animal, объекты — разные
Animal animal = new Dog();
animal.MakeSound();  // "Woof"

animal = new Cat();
animal.MakeSound();  // "Meow"
```

---

#### 4. Abstraction (абстракция) — показываем только нужное

**Идея:** Скрываем сложность системы, показывая только необходимый минимум для работы. Банкомат: видишь кнопки и экран, а не код и железо внутри.

**Зачем:** Упрощение использования, сокрытие деталей, возможность менять реализацию за абстракцией (интерфейс, абстрактный класс).

```csharp
// Абстракция: клиент знает только про IOrderRepository
public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id, CancellationToken ct);
    Task SaveAsync(Order order, CancellationToken ct);
}

// Реализация может быть на EF, SQL, in-memory — клиенту не важно
public class EfOrderRepository : IOrderRepository { ... }

// В коде зависаем от интерфейса, а не от конкретного класса
public class OrderService
{
    private readonly IOrderRepository _repo;
    public OrderService(IOrderRepository repo) => _repo = repo;
}
```

---

### SOLID — 5 принципов

#### S — Single Responsibility (единственная ответственность)

**Идея:** Один класс — одна причина для изменения. PdfGenerator генерирует PDF, а рассылку писем делает другой класс.

**Зачем:** Меньше связности, проще тесты и изменения. Если класс делает «всё подряд», любое изменение требований бьёт по нему.

```csharp
// Плохо: один класс и генерирует PDF, и шлёт email
public class ReportManager
{
    public byte[] GeneratePdf(Report data) { ... }
    public void SendEmail(string to, byte[] attachment) { ... }
}

// Хорошо: одна ответственность у каждого класса
public class PdfGenerator
{
    public byte[] Generate(Report data) { ... }
}

public class EmailSender
{
    public void Send(string to, byte[] attachment) { ... }
}
```

---

#### O — Open/Closed (открыт для расширения, закрыт для изменения)

**Идея:** Добавлять новое поведение через расширение (новые классы, реализация интерфейса), а не через правку существующего кода.

**Зачем:** Меньше риска сломать уже работающее; новые фичи — новые классы, старый код остаётся нетронутым.

```csharp
// Базовый тип — не меняем при добавлении новых форматов
public interface IReportExporter
{
    byte[] Export(Report report);
}

public class PdfExporter : IReportExporter { ... }
public class ExcelExporter : IReportExporter { ... }
// Новый формат — новый класс, старые не трогаем
public class CsvExporter : IReportExporter { ... }

// Использование: открыто для расширения новыми экспортерами
public class ReportService
{
    private readonly IEnumerable<IReportExporter> _exporters;
    public ReportService(IEnumerable<IReportExporter> exporters) => _exporters = exporters;
}
```

---

#### L — Liskov Substitution (подстановка Лисков)

**Идея:** Подкласс должен быть подставляем вместо базового класса без нарушения ожиданий клиента. Если код работает с Animal, подстановка Dog не должна ломать поведение.

**Зачем:** Полиморфизм без сюрпризов: контракт базового типа (поведение, инварианты) соблюдается и в наследниках.

```csharp
// Базовый класс задаёт контракт: SetWidth/SetHeight не должны менять другую сторону
public class Rectangle
{
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }
}

// Нарушение LSP: Square меняет обе стороны при установке одной — клиент, ожидающий Rectangle, получит неожиданное поведение
// public class Square : Rectangle { ... }  // плохо

// Правильно: наследник не нарушает контракт родителя
public class Dog : Animal
{
    public override void MakeSound() => Console.WriteLine("Woof");  // то же «семейство» поведения
}
```

---

#### I — Interface Segregation (разделение интерфейсов)

**Идея:** Интерфейсы маленькие и целевые. Клиент не должен реализовывать методы, которые ему не нужны. Лучше несколько узких интерфейсов, чем один огромный.

**Зачем:** Классы зависят только от того, что реально используют; нет «толстых» интерфейсов с пустыми реализациями.

```csharp
// Плохо: один большой интерфейс — принтер без сканера вынужден реализовать Scan()
public interface IMultiFunctionDevice
{
    void Print();
    void Scan();
    void Fax();
}

// Хорошо: маленькие интерфейсы под роли
public interface IPrinter { void Print(); }
public interface IScanner { void Scan(); }
public interface IFax { void Fax(); }

// Класс реализует только то, что умеет
public class SimplePrinter : IPrinter
{
    public void Print() { ... }
}
```

---

#### D — Dependency Inversion (инверсия зависимостей)

**Идея:** Зависеть от абстракций (интерфейсы, абстрактные классы), а не от конкретных реализаций. Сервис использует IRepository, а не SqlRepository напрямую.

**Зачем:** Подмена реализации (тесты — мок, прод — реальная БД) без изменения кода; слабая связность.

```csharp
// Плохо: зависимость от конкретного класса
public class OrderService
{
    private readonly SqlOrderRepository _repo = new SqlOrderRepository();
}

// Хорошо: зависимость от абстракции; конкретный тип внедряется снаружи (DI)
public class OrderService
{
    private readonly IOrderRepository _repo;
    public OrderService(IOrderRepository repo) => _repo = repo;
}

// В тестах подставляем Mock, в проде — EfOrderRepository
```

---

## 📚 СОДЕРЖАНИЕ

0. [OOP и SOLID — шпаргалка в начале](#-oop-и-solid--шпаргалка-в-начале)  
1. [БЛОК 1: HR и поведенческие вопросы](#блок-1-hr-и-поведенческие-вопросы)
2. [БЛОК 2: C# — основы и ООП](#блок-2-c--основы-и-ооп)
3. [БЛОК 3: C# — продвинутые темы](#блок-3-c--продвинутые-темы)
4. [БЛОК 4: ASP.NET Core / Web API](#блок-4-aspnet-core--web-api)
5. [БЛОК 5: SQL Server](#блок-5-sql-server)
6. [БЛОК 6: React и Frontend](#блок-6-react-и-frontend)
7. [БЛОК 7: Entity Framework Core](#блок-7-entity-framework-core)
8. [БЛОК 8: Тестирование](#блок-8-тестирование)
9. [БЛОК 9: Git и DevOps](#блок-9-git-и-devops)
10. [БЛОК 10: Soft Skills](#блок-10-soft-skills)
11. [БЛОК 11: Практические примеры кода](#блок-11-практические-примеры-кода)
12. [БЛОК 12: C# CORE — уровень профи (ООП, память, SOLID, Generics, Async, LINQ)](#блок-12-c-core--уровень-профи)
13. [БЛОК 13: ASP.NET Core — сердце backend (Middleware, DI, Validation, Error Handling)](#блок-13-aspnet-core--сердце-backend)
14. [БЛОК 14: Database + EF Core — выше CRUD](#блок-14-database--ef-core--выше-crud)
15. [БЛОК 15: Архитектура (Clean Architecture, CQRS, DTO)](#блок-15-архитектура)
16. [БЛОК 16: Security (JWT, Authorization)](#блок-16-security)
17. [БЛОК 17: Production Skills (Logging, тесты, CI/CD, Docker)](#блок-17-production-skills)
18. [БЛОК 18: Чеклист резюме и проекта Middle](#блок-18-чеклист-резюме-и-проекта-middle)
19. [БЛОК 19: На что обратить внимание на собеседовании](#блок-19-на-что-обратить-внимание-на-собеседовании)
20. [БЛОК 20: Follow-up глубина и темы Middle+](#блок-20-follow-up-глубина-и-темы-middle)
21. [БЛОК 21: SQL — полный разбор для собеседования](#блок-21-sql--полный-разбор-для-собеседования)
22. [БЛОК 22: RabbitMQ — полный разбор](#блок-22-rabbitmq--полный-разбор)
23. [БЛОК 23: Apache Kafka — полный гайд](#блок-23-apache-kafka--полный-гайд)
24. [БЛОК 24: Entity Framework Core — глобальный разбор ORM](#блок-24-entity-framework-core--глобальный-разбор-orm)
25. [БЛОК 25: HTTP и REST — глубже](#блок-25-http-и-rest--глубже)
26. [БЛОК 26: Caching (IMemoryCache, Redis, cache-aside)](#блок-26-caching)
27. [БЛОК 27: Design Patterns — подробнее](#блок-27-design-patterns--подробнее)
28. [БЛОК 28: Mapping и Pagination](#блок-28-mapping-и-pagination)
29. [БЛОК 29: API Design — версионирование и response](#блок-29-api-design)
30. [БЛОК 30: Resilience (Polly: Timeout, Bulkhead, Fallback)](#блок-30-resilience)
31. [БЛОК 31: Observability](#блок-31-observability)
32. [БЛОК 32: Тестирование — глубже](#блок-32-тестирование--глубже)
33. [БЛОК 33: C# — новые фичи](#блок-33-c--новые-фичи)
34. [БЛОК 34: Опционально (HttpClient, микросервисы, C#, Result)](#блок-34-опционально)

---

## БЛОК 1: HR и поведенческие вопросы

### 1. Расскажи о себе и своём опыте

Я backend‑разработчик с примерно 2 годами опыта в продакшене. Работаю в основном с C# и .NET, включая ASP.NET Core и Web API. Для работы с базой данных использую SQL Server. На фронтенде есть опыт с React, TypeScript и JavaScript. В текущей работе занимаюсь разработкой масштабируемых веб‑приложений и поддержкой существующих систем. Интересуют критические системы, например аэропортовые.

---

### 2. Почему хочешь сменить работу?

Ищу новые профессиональные задачи. Интересует работа с критичными системами, где важна надёжность и непрерывность сервиса. Хочу развиваться в более сложном и интересном контексте.

---

### 3. Что знаешь о нашей компании?

Подготовься заранее: изучи сайт, продукты, проекты компании. Упомяни конкретные проекты, технологии, миссию — покажи, что ты целенаправленно заинтересован.

---

### 4. Какие твои сильные стороны как разработчика?

Аналитическое мышление, умение разбирать сложные задачи, ответственность за результат, готовность учиться новым технологиям, аккуратная работа с кодом и документацией.

---

### 5. Какие слабые стороны?

Честно укажи 1–2 области развития. Например: «Иногда уделяю слишком много времени деталям, но сейчас лучше планирую задачи и приоритеты».

---

### 6. Опиши конфликтную ситуацию в команде и как ты её решил

Опиши по STAR: Ситуация → Задача → Действия → Результат. Покажи, что умеешь слушать, находить компромисс и фокусироваться на общем результате.

---

### 7. Где видишь себя через 5 лет?

Например: «Вижу себя сильным senior‑разработчиком, который не только пишет код, но и помогает проектировать архитектуру и менторит junior‑разработчиков».

---

### 8. Какую зарплату ожидаешь?

Проверь рыночные ставки и назови диапазон, ориентируясь на свой опыт, навыки и локацию. Будь готов обосновать цифры.

---

### 9. Есть ли у тебя вопросы к нам?

Задавай вопросы о проектах, стеке, культуре, процессах разработки, возможностях роста. Это показывает интерес к месту.

---

### 10. Почему именно .NET?

.NET даёт стабильную экосистему, производительность, современный C#, отличную поддержку Microsoft, широкий выбор библиотек и востребованность в enterprise‑проектах.

---

## БЛОК 2: C# — основы и ООП

### 11. Что такое Class и Object?

**Class** — шаблон (тип), описывающий структуру и поведение. Определяет поля, свойства, методы.

**Object** — конкретный экземпляр класса, созданный оператором `new`. Объект существует в памяти и использует поведение, заданное классом.

```csharp
// Класс — это определение
public class User
{
    public string Name { get; set; }
}

// Объект — это экземпляр класса
var user = new User { Name = "Ivan" };
```

---

### 12. Что такое static и для чего нужен?

**static** — модификатор, означающий, что член принадлежит классу, а не конкретному объекту. Один экземпляр на всё приложение.

- Статические поля/свойства — общие данные для всех экземпляров
- Статические методы — вызываются без создания объекта
- Статический конструктор — вызывается один раз при первом обращении к классу

```csharp
public class Counter
{
    public static int Count { get; set; }  // Общий для всех
    public static void Reset() => Count = 0;  // Вызов: Counter.Reset();
}
```

---

### 13. Разница между var и явным типом?

**var** — вывод типа на этапе компиляции. Тип остаётся явным в IL, IntelliSense работает. Обязательна инициализация.

```csharp
var name = "Test";  // string
var list = new List<int>();  // List<int>
```

**Явный тип** — полезен, когда тип неочевиден, в публичных API и для читаемости. `var` лучше для локальных переменных, где тип очевиден.

---

### 14. Разница между классом и структурой (class vs struct)?

| | **class** | **struct** |
|---|---|---|
| Хранение | reference type (heap) | value type (stack/value) |
| null | может быть null | не может (без nullable) |
| Наследование | поддерживает | не поддерживает |
| Конструктор по умолчанию | есть | нельзя определить вручную |
| Копирование | по ссылке | по значению |

Структуры лучше для небольших неизменяемых значений (Point, DateTime и т.п.).

---

### 15. Что такое reference type и value type?

**Value type** — хранится в стеке (или inline), при присваивании копируется полностью. Примеры: int, bool, struct, enum.

**Reference type** — хранится в куче, переменная содержит ссылку. При присваивании копируется только ссылка. Примеры: class, interface, delegate, string.

---

### 16. Что такое nullable и когда его использовать?

`T?` — сокращение для `Nullable<T>`. Позволяет value type хранить null (например, `int?`, `DateTime?`).

Используют, когда значение может отсутствовать: опциональные поля, отсутствующие данные из БД.

```csharp
int? age = null;
if (age.HasValue)
    Console.WriteLine(age.Value);
```

---

### 17. Что такое наследование? virtual и override?

**Наследование** — механизм, при котором класс получает члены другого класса. Используется для иерархии и переиспользования кода.

**virtual** — метод в базовом классе, который можно переопределить.

**override** — переопределение виртуального метода в производном классе. Это основа полиморфизма во время выполнения.

```csharp
public class Animal
{
    public virtual void Speak() => Console.WriteLine("...");
}
public class Dog : Animal
{
    public override void Speak() => Console.WriteLine("Woof");
}
```

---

### 18. abstract class vs interface — в чём разница?

| | **abstract class** | **interface** |
|---|---|---|
| Реализация | частичная или полная | только сигнатуры (до C# 8) |
| Конструкторы | может быть | нет |
| Поля | может иметь | нет (кроме static) |
| Множественное наследование | нет | да |
| Назначение | общая база для иерархии | контракт, абстракция поведения |

---

### 19. Что такое полиморфизм?

Полиморфизм — возможность использовать разные реализации через общий интерфейс или базовый тип.

**Compile-time polymorphism (статический)** — компилятор выбирает метод до запуска программы. Главный пример — **перегрузка методов (overloading)**: несколько методов с одним именем, но разными параметрами (количество, типы, порядок). Нельзя перегрузить только по return type.

```csharp
class Calculator
{
    public int Add(int a, int b) => a + b;
    public double Add(double a, double b) => a + b;
    public int Add(int a, int b, int c) => a + b + c;
}
// calc.Add(2, 3) → int; calc.Add(2.5, 3.1) → double
```

**Runtime polymorphism (динамический)** — выбор метода во время выполнения. Через virtual/override.

```csharp
Animal a = new Dog();
a.Speak();  // Woof — вызывается метод Dog, несмотря на тип Animal
```

---

### 20. Что такое инкапсуляция?

Сокрытие деталей реализации и предоставление только нужного API. Достигается через модификаторы доступа: `private`, `protected`, `internal`, `public`. Поля обычно private, доступ — через свойства и методы.

---

### 21. Что такое ref и out?

**ref** — аргумент передаётся по ссылке, должен быть инициализирован до вызова. Метод может читать и менять его.

**out** — аргумент передаётся по ссылке, метод обязан присвоить ему значение. Не требуется инициализация до вызова.

```csharp
void Swap(ref int a, ref int b) { (a, b) = (b, a); }
bool TryParse(string s, out int result);
```

---

### 22. Что такое params?

Ключевое слово, позволяющее передавать **переменное количество аргументов** в метод. Компилятор упаковывает их в массив: `Sum(1, 2, 3)` превращается в `Sum(new int[] { 1, 2, 3 })`. Можно передать массив напрямую.

**Правила:** только один params; params всегда последний параметр; можно комбинировать с обычными параметрами.

```csharp
public int Sum(params int[] numbers)
{
    int total = 0;
    foreach (int n in numbers) total += n;
    return total;
}
Sum(1, 2); Sum(1, 2, 3);  // работает
void Print(string name, params int[] nums);  // params последний
```

**Где используется:** `Console.WriteLine("Hello {0}", name)` — через params object[]; `string.Format`, логирование.

---

### 23. Разница между const и readonly?

**const** — константа на этапе компиляции. Должна быть инициализирована при объявлении. Нельзя использовать с объектными типами (кроме string).

**readonly** — поле, которое можно задать при объявлении или в конструкторе. Подходит для полей объекта и reference types.

---

### 24. Что такое boxing и unboxing?

**Boxing** — преобразование value type в object: значение копируется из stack в heap, создаётся объект-обёртка.

**Unboxing** — обратное: object → value type, нужен явный cast. Ошибка при неверном типе: `(double)(object)10` → InvalidCastException.

```csharp
int i = 42;
object o = i;    // boxing — создаётся объект в heap
int j = (int)o;  // unboxing — копирование обратно
```

**Зачем:** object — базовый тип; старые коллекции (ArrayList) работали через object; интерфейсы (struct → interface даёт boxing). **Почему плохо:** аллокация в heap, копирование, нагрузка на GC. `List<int>` вместо `List<object>` избегает boxing.

---

### 25. Что такое string immutability?

Строки в C# **неизменяемы** — после создания строку нельзя изменить. Операция `s = s + "!"` создаёт **новую** строку; старая остаётся. Если `b = a`, потом `a += "x"`, то `b` не изменится.

**Зачем:** безопасность, string pool (одинаковые литералы могут указывать на один объект), потокобезопасность. **Проблема:** конкатенация в цикле создаёт много объектов → использовать `StringBuilder`.

---

### 26. StringBuilder — когда использовать?

Класс для **эффективного построения строк**. string immutable — каждое `+=` создаёт новый объект; StringBuilder изменяет один буфер. **Методы:** Append, AppendLine, Insert, Replace, Remove.

**Когда использовать:** сбор строки в цикле, много конкатенаций, большие тексты (лог, отчёт). **Когда не нужен:** малая конкатенация (`first + last`). `for(...) sb.Append("a")` быстрее, чем `for(...) text += "a"`.

---

### 27. Что такое enum?

Набор именованных констант одного целочисленного типа. Используется для ограниченного набора значений (статусы, флаги).

```csharp
public enum Status { Pending, Active, Completed }
```

---

### 28. Что такое delegate?

Тип, хранящий **ссылку на метод**. Можно передавать методы как параметры, сохранять в переменную, вызывать через неё. Основа для событий, LINQ, callbacks.

```csharp
public delegate int Operation(int a, int b);
Operation op = (a, b) => a + b;
Console.WriteLine(op(2, 3));  // 5
```

**Action** — void; **Func&lt;T&gt;** — возвращает значение. **Multicast delegate:** `a += Method2` — при вызове выполняются оба; если возвращает значение — вернётся результат последнего.

---

### 29. Что такое event?

Механизм уведомления подписчиков. По сути — инкапсулированный делегат с добавлением/удалением подписчиков (`+=`, `-=`). Позволяет избежать прямого вызова методов извне класса.

---

### 30. Разница между == и Equals()?

**==** — оператор. Для value types сравнивает значения; для reference types по умолчанию — ссылки (если не перегружен).

**Equals()** — метод, определяет логическое равенство. По умолчанию у классов тоже сравнивает ссылки; можно переопределить.

**string — особый случай:** и ==, и Equals сравнивают содержимое (перегрузка и override). **Важно:** `object a = "hi"; object b = "hi";` — `a == b` может быть false (сравнение ссылок), `a.Equals(b)` — true (содержимое).

---

### 31. Зачем переопределять GetHashCode() при Equals()?

Контракт: если `a.Equals(b)`, то `a.GetHashCode() == b.GetHashCode()`. Иначе `Dictionary`, `HashSet` работают некорректно (равные объекты считаются разными). Хэш используется для быстрого поиска; коллизии разрешаются через Equals. **HashCode.Combine(Name, Age)** — современный способ. **Опасность:** если объект как ключ Dictionary и его поле изменилось после добавления — хэш не совпадает, ключ «потеряется».

---

### 32. Что такое record в C#?

Тип (C# 9+), ориентированный на неизменяемые данные. Автоматически генерирует `Equals`, `GetHashCode`, `ToString`, позволяет удобный синтаксис для копирования с изменениями.

```csharp
public record User(string Name, string Email);
var u2 = u1 with { Name = "New" };
```

---

### 33. Что такое init-only property?

Свойство, доступное для инициализации только в конструкторе или object initializer. После создания объекта его менять нельзя.

```csharp
public string Name { get; init; }
```

---

### 34. Что такое pattern matching?

Проверка типа, структуры и значений с удобным извлечением данных. **Type pattern:** `if (obj is Person p) Console.WriteLine(p.Name)`. **Property pattern:** `if (p is { Age: > 18 })`. **Switch expression:** `level switch { 1 => "User", _ => "Unknown" }`. **Tuple pattern:** `(0, _) => "On Y axis"`. Для Result-типов: `return result switch { Success s => Ok(s.Data), NotFound => NotFound(), _ => StatusCode(500) }`.

---

### 35. Что такое expression-bodied members?

Короткий синтаксис для методов, свойств, индексаторов и конструкторов в одну строку через `=>`.

```csharp
public string FullName => $"{FirstName} {LastName}";
public int Double(int x) => x * 2;
```

---

### 36. Что такое null-conditional operator (?.)?

Вызов члена только если объект не null. Если объект null, выражение возвращает null без исключения.

```csharp
var length = user?.Address?.City?.Length;  // null если любой из них null
```

---

### 37. Что такое null-coalescing (?? и ??=)?

**??** — возвращает левый операнд, если он не null, иначе правый.

**??=** — присваивает правый операнд только если левый null.

```csharp
var name = user?.Name ?? "Unknown";
list ??= new List<int>();
```

---

### 38. Что такое nameof?

Возвращает строковое имя переменной, типа, члена. Полезно для сообщений об ошибках, логов, рефлексии — не ломается при переименовании.

```csharp
throw new ArgumentNullException(nameof(user));
```

---

### 39. Разница между throw и throw ex?

**throw** — сохраняет оригинальный stack trace.

**throw ex** — перезаписывает stack trace, теряется информация о месте возникновения.

Всегда используй **throw** для повторной передачи исключения.

---

### 40. Что такое using (IDisposable)?

Гарантирует вызов `Dispose()` при выходе из блока (в т.ч. при исключении). Компилятор превращает в try/finally. **C# 8+:** `using var fs = File.OpenRead(...)` — Dispose в конце метода.

```csharp
using (SqlConnection conn = new SqlConnection(cs))
{
    conn.Open();
    // запросы...
}
```

**IDisposable** — интерфейс для ресурсов (файлы, потоки, SqlConnection, DbContext, HttpResponseMessage). Dispose освобождает сразу; GC ждёт дольше. **Если забыть Dispose у SqlConnection** — connection leak, пул переполняется, «max pool size reached».

---

### 40a. Extension Methods — глубоко

**Extension Method (метод расширения)** — статический метод, который вызывается как метод экземпляра на первом параметре. Синтаксически выглядит так, будто тип «приобрёл» новый метод, хотя класс типа менять не нужно.

**Синтаксис:** метод объявляется в **статическом** классе, первый параметр с модификатором **this** — тип, который «расширяем».

```csharp
public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string? value)
    {
        return string.IsNullOrEmpty(value);
    }
}

// Вызов как у экземпляра
bool empty = "".IsNullOrEmpty();
```

Компилятор превращает вызов `x.SomeExtMethod(a, b)` в вызов статического метода `StaticClass.SomeExtMethod(x, a, b)`. Никакой «магии» в рантайме — только синтаксис.

---

**Зачем нужны**

- Добавлять поведение к типам, которые нельзя изменить (например, интерфейсы, типы из библиотек).
- Писать цепочки вызовов в стиле fluent API: `items.Where(...).OrderBy(...).Select(...)`.
- Не засорять наследников: логика в отдельном статическом классе, а не в подклассах.

**Где живут**

- В **статическом** классе (имя класса может быть любым, часто суффикс `Extensions`).
- Сам метод обязательно **static**, первый параметр — `this TypeName parameterName`.

**Разрешение (resolution)**

- Компилятор ищет extension methods только в **статических классах**, видимых в текущей области (текущий файл + открытые через `using` пространства имён). Если два расширения с одинаковой сигнатурой видны в одной области — ошибка неоднозначности.
- Extension methods **не переопределяют** реальные методы экземпляра. Если у типа уже есть метод с такой сигнатурой — вызывается он, а не расширение.
- Для интерфейсов (например, `IEnumerable<T>`) расширение пишется для интерфейса — тогда метод доступен всем реализациям. Так устроен LINQ: методы объявлены для `IEnumerable<T>` и `IQueryable<T>` в статических классах `Enumerable` и `Queryable`.

**Ограничения**

- Расширять можно только типы: классы, структуры, интерфейсы. Нельзя расширять, например, делегаты или перечисления тем же синтаксисом (для enum нужен отдельный приём).
- Первый параметр — только один, и только он помечается `this`.
- Extension method не имеет доступа к **private** и **protected** членам типа — только к публичной поверхности. Это не «подмена» класса, а вызов статической функции с первым аргументом.

**Nullable и value type**

- Для `string?` первый параметр можно объявить как `this string? value` — тогда метод виден и для null, внутри нужно проверять на null.
- Для value type (например, `this int value`) расширение получает копию; менять «исходное» значение по ссылке нельзя, если не использовать `ref` (но для extension с `ref` есть отдельный синтаксис — `ref extension` в новых версиях C# для ref struct).

**LINQ и Extension Methods**

- Почти все методы LINQ (`Where`, `Select`, `OrderBy`, `GroupBy`, `First`, `ToList` и т.д.) — это extension methods для `IEnumerable<T>` и/или `IQueryable<T>`. Поэтому пишется `source.Where(x => x.Id > 0)`, а не `Enumerable.Where(source, x => x.Id > 0)`.
- Разница между LINQ to Objects и LINQ to SQL/EF: для `IEnumerable<T>` методы определены в `Enumerable`, для `IQueryable<T>` — в `Queryable`; последние принимают `Expression<...>`, чтобы провайдер (EF) строил дерево выражений и SQL.

**Fluent API и цепочки**

- Extension methods позволяют возвращать тот же или связанный тип и вызывать следующий метод: `builder.UseX().UseY().UseZ()`. В ASP.NET Core так сделаны многие `Use*` для middleware — это extension-методы для `IApplicationBuilder`.

**Именование и организация**

- Класс с расширениями часто называют `{Type}Extensions` (например, `StringExtensions`, `CollectionExtensions`). В одном классе группируют расширения для одного типа (или одной темы).
- Чтобы extension был виден, нужен `using` на пространство имён, где объявлен статический класс. В больших решениях выносят расширения в отдельные папки/проекты и подключают через using.

**На собеседовании**

- «Что такое extension method?» — статический метод с первым параметром `this Type`, вызывается как метод экземпляра; компилятор подставляет вызов статического метода.
- «Как компилятор находит extension methods?» — только в статических классах в видимых namespace’ах; при конфликте сигнатур — неоднозначность.
- «Почему LINQ можно вызывать как .Where(), .Select()?» — это extension methods для `IEnumerable<T>` и `IQueryable<T>`.

---

## БЛОК 3: C# — продвинутые темы

### 41. Что такое Garbage Collector (GC)?

Компонент CLR, который управляет памятью в .NET. Освобождает объекты, на которые больше нет ссылок. Работает по поколениям: Gen 0 (короткоживущие), Gen 1, Gen 2 (долгоживущие).

---

### 42. Как работает сборка мусора по поколениям?

- Gen 0 — новые объекты, сборка происходит чаще
- Gen 1 — объекты, пережившие сборку Gen 0
- Gen 2 — долгоживущие объекты, сборка реже и дольше

Идея: большинство объектов быстро становятся мусором, поэтому Gen 0 компактна и быстро очищается.

---

### 43. Что такое IDisposable и зачем нужен?

Интерфейс для явного освобождения неуправляемых ресурсов (файлы, сеть, дескрипторы). Метод `Dispose()` вызывается вручную или через `using`. GC не управляет такими ресурсами напрямую.

---

### 44. Что такое finalizer (деструктор)?

Метод `~ClassName()` — вызывается GC при сборке объекта. Ненадёжен по времени, создаёт нагрузку. Используется редко, в основном для fallback при утечке вызова `Dispose()`.

---

### 45. Что такое deadlock (взаимная блокировка)?

Ситуация, когда два и более потока ждут друг друга, и ни один не может продолжить. Например, поток A держит lock1 и ждёт lock2, а поток B держит lock2 и ждёт lock1.

---

### 46. Как избежать deadlock?

- Одинаковый порядок захвата блокировок во всех потоках
- Использование таймаутов (`Monitor.TryEnter`)
- `lock` только на короткое время
- Избегать вложенных блокировок
- Использовать `SemaphoreSlim`, `ConcurrentDictionary` и т.п. вместо ручных lock

---

### 47. Что такое lock и Monitor?

**lock** — синтаксический сахар для `Monitor.Enter`/`Exit` с `try-finally`. Обеспечивает эксклюзивный доступ к объекту.

```csharp
lock (_syncObject)
{
    // critical section
}
```

---

### 48. Что такое async/await?

**async** — помечает метод как асинхронный.  
**await** — приостанавливает выполнение до завершения задачи, не блокируя поток.

Асинхронность освобождает потоки во время ожидания I/O и повышает масштабируемость.

```csharp
public async Task<User> GetUserAsync(int id)
{
    var user = await _repository.GetByIdAsync(id);
    return user;
}
```

---

### 49. Task vs Thread?

**Thread** — поток ОС. Создание и переключение дорогие.

**Task** — абстракция над асинхронной работой. Может выполняться в thread pool, не обязательно в отдельном потоке. Подходит для I/O и CPU-bound задач.

---

### 50. Task.Run — когда использовать?

Для выноса CPU-bound или синхронного блокирующего кода в фоновый поток из thread pool. Не стоит использовать только ради «сделать метод async» — для I/O лучше нативный async API.

---

### 51. Разница между IEnumerable и IQueryable?

**IEnumerable** — коллекция в памяти. LINQ выполняется локально.

**IQueryable** — запрос к провайдеру (например, БД). LINQ транслируется в SQL и выполняется на сервере.

IQueryable эффективнее для БД — фильтрация и сортировка выполняются на стороне сервера.

---

### 52. Deferred execution (отложенное выполнение) в LINQ?

Многие операторы LINQ не выполняются сразу при создании запроса, а только при перечислении (foreach, ToList, Count и т.д.). Это позволяет строить цепочки запросов и выполнять их один раз.

---

### 53. Разница между First, FirstOrDefault, Single, SingleOrDefault?

- **First** — первый элемент; исключение, если пусто
- **FirstOrDefault** — первый элемент или default
- **Single** — ровно один элемент; исключение при 0 или >1
- **SingleOrDefault** — ровно один или default; исключение при >1

---

### 54. Select vs Where?

**Where** — фильтрует элементы по условию.

**Select** — проецирует элементы в другой тип/форму.

---

### 55. GroupBy — как работает?

Группирует элементы по ключу. Результат — последовательность групп, каждая с ключом и коллекцией элементов.

```csharp
var byDepartment = users.GroupBy(u => u.DepartmentId);
```

---

### 56. Dependency Injection — что это?

Паттерн, при котором зависимости передаются извне, а не создаются внутри класса. В ASP.NET Core используется встроенный контейнер: `AddSingleton`, `AddScoped`, `AddTransient`.

Плюсы: слабая связанность, лёгкое тестирование, гибкая конфигурация.

#### Как было БЕЗ DI (жёсткая связанность):

```csharp
// OrderService сам создаёт EmailService — нельзя подменить, сложно тестировать
public class OrderService
{
    private readonly EmailService _emailService = new EmailService(); // hardcoded dependency!

    public void CreateOrder(Order order)
    {
        // ... business logic ...
        _emailService.Send(order.CustomerEmail, "Order confirmed");
    }
}

// Контроллер тоже создаёт сервис внутри — цепочка зависимостей
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService = new OrderService(); // tight coupling!

    [HttpPost]
    public IActionResult Create(OrderDto dto)
    {
        _orderService.CreateOrder(MapToOrder(dto));
        return Ok();
    }
}
```

**Проблемы:** нельзя подменить `EmailService` на mock в тестах, нельзя сменить реализацию (SMS вместо email), нарушение DIP (Dependency Inversion Principle).

#### Как стало С DI (слабая связанность):

```csharp
// Interface — абстракция, от которой зависит OrderService
public interface IEmailService
{
    void Send(string to, string message);
}

public class EmailService : IEmailService
{
    public void Send(string to, string message) { /* ... */ }
}

// Зависимость приходит через конструктор — внедряется извне
public class OrderService
{
    private readonly IEmailService _emailService;

    public OrderService(IEmailService emailService)
    {
        _emailService = emailService; // injected from DI container
    }

    public void CreateOrder(Order order)
    {
        // ... business logic ...
        _emailService.Send(order.CustomerEmail, "Order confirmed");
    }
}

// Контроллер получает OrderService через конструктор
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService; // injected by ASP.NET Core
    }

    [HttpPost]
    public IActionResult Create(OrderDto dto)
    {
        _orderService.CreateOrder(MapToOrder(dto));
        return Ok();
    }
}

// Program.cs — регистрация в контейнере
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<OrderService>();
```

**Плюсы:** в тестах подставляем `MockEmailService`, можно менять реализацию без изменения `OrderService`, соблюдение SOLID.

---

### 57. DI & IoC Container — Transient, Scoped, Singleton и регистрация (детально)

**IoC (Inversion of Control)** — управление созданием и жизненным циклом объектов передано контейнеру. **DI (Dependency Injection)** — способ передать зависимости в класс извне (через конструктор, свойство, метод). В ASP.NET Core встроен IoC-контейнер и DI «из коробки».

---

#### 57.1. Transient, Scoped, Singleton — в чём разница

| Lifetime | Когда создаётся экземпляр | Сколько живёт | Типичное использование |
|----------|---------------------------|---------------|--------------------------|
| **Transient** | При каждом вызове `GetService<T>()` / инжекте | До сборки мусора | Валидаторы, мапперы, лёгкие stateless |
| **Scoped** | Один раз на scope (в ASP.NET — на один HTTP-запрос) | До завершения scope (конец запроса) | DbContext, Unit of Work, репозитории |
| **Singleton** | Один раз при первом запросе | До завершения приложения | Кэш, конфигурация, логгер-фабрика |

**Как это работает при resolve:**

- **Transient:** у контроллера есть `IValidator` и `IMapper`. За один запрос контроллер создаётся один раз, но если внутри одного объекта дважды запросить Transient (или два разных сервиса оба зависят от одного Transient) — контейнер создаст **два** экземпляра (каждый resolve = новый экземпляр).
- **Scoped:** в рамках одного HTTP-запроса все инжекты `IOrderRepository` и `AppDbContext` получают **один и тот же** экземпляр. Другой запрос — другой экземпляр.
- **Singleton:** один экземпляр на всё приложение, потокобезопасность — ответственность разработчика.

```csharp
// Program.cs — базовая регистрация
builder.Services.AddSingleton<ICacheService, MemoryCacheService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IEmailValidator, EmailValidator>();
```

**Порядок создания при одном HTTP-запросе (упрощённо):**

```
Request → Create Scope
    → Resolve Controller (need IOrderRepository, IEmailValidator)
        → Resolve OrderRepository (need DbContext) → Resolve DbContext (Scoped) — один на scope
        → Resolve EmailValidator (Transient) — новый экземпляр
    → Controller получил один Scoped Repository и один Transient Validator
→ End of request → Scope disposed → Scoped instances (DbContext, Repository) disposed
```

---

#### 57.2. Когда какой lifetime использовать

**Singleton:**

- Stateless сервисы: кэш в памяти, `IOptions<T>`, клиенты (если потокобезопасны и переиспользуются).
- Не использовать для: DbContext, чего-либо с состоянием на запрос, чего-либо не потокобезопасного.

```csharp
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddSingleton<IConfiguration>(sp => sp.GetRequiredService<IConfiguration>());
```

**Scoped:**

- Всё, что привязано к одному запросу или одной единице работы: `DbContext`, Unit of Work, репозитории, сервисы, которые используют DbContext.

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)); // по умолчанию Scoped
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
```

**Transient:**

- Лёгкие объекты без состояния, создание дёшево: валидаторы, мапперы, мелкие хелперы. Если создание тяжёлое или объект держит ресурсы — предпочтительнее Scoped.

```csharp
builder.Services.AddTransient<IValidator<OrderDto>, OrderValidator>();
builder.Services.AddTransient<IMapper, MyMapper>();
```

**Важно: DbContext — только Scoped.** Singleton с DbContext приведёт к утечкам, гонкам и порче данных. Всегда Scoped (или явный scope в фоне через `IServiceScopeFactory`).

---

#### 57.3. Captive dependency — проблема и решение

**Captive dependency** — когда сервис с **коротким** временем жизни (Transient или Scoped) внедрён в сервис с **длинным** (Singleton). Singleton создаётся один раз и держит ссылку на зависимость — та живёт столько же, сколько и он, т.е. фактически становится «пленником» Singleton и ведёт себя как Singleton. Для Scoped (например, DbContext) это недопустимо.

**Пример нарушения:**

```csharp
// BAD: Singleton holds Scoped dependency — captive dependency
builder.Services.AddSingleton<ICacheWarmer, CacheWarmer>();      // Singleton
builder.Services.AddScoped<AppDbContext>(/* ... */);            // Scoped

public class CacheWarmer : ICacheWarmer
{
    private readonly AppDbContext _db;  // Injected once when CacheWarmer is created

    public CacheWarmer(AppDbContext db) => _db = db;  // DbContext captured forever!

    public void Warm() => _db.Products.Load();  // Same DbContext for all requests — stale data, not thread-safe
}
```

Контейнер при создании Singleton разрешает зависимости. Scoped в момент создания Singleton берётся из «текущего» scope (или создаётся специальный), и этот один экземпляр DbContext навсегда остаётся в `CacheWarmer`. Дальнейшие запросы не получают новый DbContext внутри `CacheWarmer` — нарушается ожидаемое время жизни Scoped.

**Правильный подход — не инжектить Scoped/Transient в Singleton.** Если Singleton должен выполнять работу, требующую Scoped-сервисов, создавать scope вручную через `IServiceScopeFactory`:

```csharp
builder.Services.AddSingleton<ICacheWarmer, CacheWarmer>();
builder.Services.AddScoped<AppDbContext>(/* ... */);

public class CacheWarmer : ICacheWarmer
{
    private readonly IServiceScopeFactory _scopeFactory;

    public CacheWarmer(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

    public void Warm()
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Products.Load();
    }
}
```

Каждый вызов `Warm()` получает свой scope и свой экземпляр `AppDbContext`, который корректно освобождается после `using`.

**Правило:** не внедрять Scoped/Transient в конструктор Singleton. Для фоновых задач и воркеров — использовать `IServiceScopeFactory.CreateScope()` и разрешать Scoped-сервисы внутри созданного scope.

---

#### 57.4. Регистрация в ASP.NET Core — детально

**Базовые методы:**

```csharp
// Интерфейс → реализация (контейнер создаёт реализацию)
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddSingleton<ICacheService, MemoryCacheService>();

// Конкретный класс (если нет абстракции)
builder.Services.AddScoped<OrderService>();

// Существующий экземпляр (Singleton по смыслу)
var config = new MyConfig();
builder.Services.AddSingleton(config);
builder.Services.AddSingleton<IMyConfig>(config);
```

**Регистрация через фабрику:**

```csharp
builder.Services.AddScoped<IOrderRepository>(sp =>
{
    var db = sp.GetRequiredService<AppDbContext>();
    var logger = sp.GetRequiredService<ILogger<OrderRepository>>();
    return new OrderRepository(db, logger);
});

builder.Services.AddSingleton<IMyService>(sp =>
{
    var options = sp.GetRequiredService<IOptions<MyOptions>>().Value;
    return new MyService(options);
});
```

**Open Generic (универсальная регистрация для любых T):**

```csharp
builder.Services.AddTransient(typeof(IValidator<>), typeof(GenericValidator<>));
// IValidator<OrderDto> → GenericValidator<OrderDto>, IValidator<UserDto> → GenericValidator<UserDto>
```

**Несколько реализаций и выбор при resolve:**

```csharp
builder.Services.AddScoped<IReportExporter, PdfExporter>();
builder.Services.AddScoped<IReportExporter, ExcelExporter>();
// GetService<IEnumerable<IReportExporter>>() — все; иначе контейнер по умолчанию вернёт последнюю зарегистрированную

// Именованные / ключевые сервисы (например, .NET 8)
builder.Services.AddKeyedScoped<IReportExporter, PdfExporter>("pdf");
builder.Services.AddKeyedScoped<IReportExporter, ExcelExporter>("excel");
// В конструкторе: [FromKeyedServices("pdf")] IReportExporter exporter
```

**TryAdd — регистрировать только если ещё не зарегистрировано:**

```csharp
builder.Services.TryAddSingleton<ICacheService, MemoryCacheService>();
builder.Services.TryAddSingleton<ICacheService, RedisCacheService>(); // не подменит первую
```

**Replace / Remove (подмена или удаление):**

```csharp
builder.Services.AddScoped<IEmailService, EmailService>();
// В тестах или при условной конфигурации:
builder.Services.Replace(ServiceDescriptor.Scoped<IEmailService, MockEmailService>());
// или
var descriptor = builder.Services.First(d => d.ServiceType == typeof(IEmailService));
builder.Services.Remove(descriptor);
builder.Services.AddScoped<IEmailService, MockEmailService>();
```

**Групповая регистрация (например, все IValidator из сборки):**

```csharp
builder.Services.AddValidatorsFromAssemblyContaining<OrderValidator>();
// Обычно это расширение из FluentValidation; идея та же — сканирование и регистрация по интерфейсу
```

**Регистрация DbContext:**

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.EnableSensitiveDataLogging(env.IsDevelopment());
});
// По умолчанию Scoped; для пула:
builder.Services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(connectionString));
```

---

#### Краткая сводка

| Вопрос | Ответ |
|--------|--------|
| Transient | Новый экземпляр при каждом resolve. Для лёгких stateless. |
| Scoped | Один экземпляр на scope (запрос). DbContext, репозитории, Unit of Work. |
| Singleton | Один на приложение. Кэш, конфигурация. Не держать Scoped внутри. |
| Captive dependency | Scoped/Transient в конструкторе Singleton. Решение: IServiceScopeFactory в Singleton и создание scope при вызове. |
| Регистрация | Add*/TryAdd*, фабрика, Open Generic, Keyed (.NET 8), Replace/Remove. |

---

### 57.5. IServiceProvider: GetService vs GetRequiredService

`IServiceProvider` — интерфейс контейнера DI. Позволяет **вручную** получить сервис по типу. Доступен через инжект `IServiceProvider` в конструктор или через `HttpContext.RequestServices` в ASP.NET Core.

#### GetService vs GetRequiredService

```csharp
// GetService — возвращает T? (null, если сервис не зарегистрирован)
public T? GetService<T>() where T : class;

// GetRequiredService — возвращает T, бросает InvalidOperationException, если не найден
public T GetRequiredService<T>() where T : class;
```

#### Примеры использования

```csharp
// 1. В контроллере или сервисе — когда сервис опционален
public class ReportController : ControllerBase
{
    private readonly IServiceProvider _services;

    public ReportController(IServiceProvider services) => _services = services;

    [HttpGet("export")]
    public IActionResult Export(string format)
    {
        // Экспортёр может быть не зарегистрирован (опциональная фича)
        var exporter = _services.GetService<IReportExporter>();
        if (exporter == null)
            return BadRequest("Export feature is not configured");
        return Ok(exporter.Export());
    }
}

// 2. В Worker / IHostedService — для Scoped-сервисов (обязательно через scope!)
using var scope = _serviceProvider.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
// GetRequiredService — сервис обязан быть; если нет — сразу исключение при старте
```

#### Когда использовать GetService, когда GetRequiredService

| Ситуация | Метод |
|----------|-------|
| Сервис обязателен (DbContext, репозиторий) | `GetRequiredService<T>()` |
| Сервис опционален (плагин, feature flag) | `GetService<T>()` + проверка на null |
| В scope внутри Worker | `scope.ServiceProvider.GetRequiredService<T>()` |

#### Extension-методы (Microsoft.Extensions.DependencyInjection)

```csharp
// Добавить: using Microsoft.Extensions.DependencyInjection;

var service = serviceProvider.GetService<IMyService>();      // T?
var required = serviceProvider.GetRequiredService<IMyService>(); // T, throws if null
```

Эти методы — extension'ы для `IServiceProvider`, определённые в пакете `Microsoft.Extensions.DependencyInjection.Abstractions`.

---

### 58. Принципы SOLID — подробно с примерами

SOLID — пять принципов объектно-ориентированного проектирования, помогающих писать поддерживаемый, расширяемый и тестируемый код.

---

#### SRP — Single Responsibility Principle (Принцип единственной ответственности)

**Суть:** Один класс должен иметь только одну причину для изменения. Одна ответственность = одна зона изменений.

**Почему важно:** Если класс делает несколько несвязанных вещей, любое изменение в одной области затрагивает весь класс. Это усложняет тестирование, увеличивает риск регрессий и затрудняет понимание кода.

**Пример нарушения:**

```csharp
/// <summary>
/// BAD: Class violates SRP — handles order logic, persistence, and email notification.
/// Any change in order rules, DB, or email format forces modification of this class.
/// </summary>
public class OrderService
{
    public void ProcessOrder(Order order)
    {
        // Responsibility 1: Business logic
        if (order.Total < 0)
            throw new ArgumentException("Invalid order total");
        order.Status = OrderStatus.Processing;

        // Responsibility 2: Persistence (DB access)
        using var connection = new SqlConnection("ConnectionString");
        connection.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = "INSERT INTO Orders (...) VALUES (...)";
        cmd.ExecuteNonQuery();

        // Responsibility 3: Email notification
        var smtp = new SmtpClient("smtp.server.com");
        smtp.Send("orders@shop.com", order.CustomerEmail,
            "Order confirmed", $"Your order #{order.Id} is processing");
    }
}
```

**Правильный вариант:**

```csharp
/// <summary>
/// Handles only order business logic. Delegates persistence and notifications.
/// </summary>
public class OrderProcessor
{
    private readonly IOrderRepository _repository;
    private readonly INotificationService _notification;

    public OrderProcessor(IOrderRepository repository, INotificationService notification)
    {
        _repository = repository;
        _notification = notification;
    }

    public void ProcessOrder(Order order)
    {
        if (order.Total < 0)
            throw new ArgumentException("Invalid order total");
        order.Status = OrderStatus.Processing;

        _repository.Save(order);
        _notification.SendOrderConfirmation(order);
    }
}

/// <summary>
/// Single responsibility: data persistence.
/// </summary>
public interface IOrderRepository
{
    void Save(Order order);
}

/// <summary>
/// Single responsibility: sending notifications.
/// </summary>
public interface INotificationService
{
    void SendOrderConfirmation(Order order);
}
```

---

#### OCP — Open/Closed Principle (Принцип открытости/закрытости)

**Суть:** Классы должны быть открыты для расширения, но закрыты для модификации. Добавлять новое поведение нужно через новые классы/наследование, а не через правку существующего кода.

**Почему важно:** Модификация работающего кода — источник багов. Расширение через новые сущности снижает риск сломать уже работающую логику.

**Пример нарушения:**

```csharp
/// <summary>
/// BAD: Adding new discount type requires modifying this class (switch/case).
/// Violates OCP — class is not closed for modification.
/// </summary>
public class PriceCalculator
{
    public decimal Calculate(decimal price, string discountType)
    {
        switch (discountType)
        {
            case "Percentage":
                return price * 0.9m;
            case "Fixed":
                return price - 50;
            case "Seasonal":  // New type — had to MODIFY existing code
                return price * 0.85m;
            default:
                return price;
        }
    }
}
```

**Правильный вариант:**

```csharp
/// <summary>
/// Strategy interface — new discount types extend via new classes, no modification.
/// </summary>
public interface IDiscountStrategy
{
    decimal Apply(decimal price);
}

public class PercentageDiscount : IDiscountStrategy
{
    private readonly decimal _percent;
    public PercentageDiscount(decimal percent) => _percent = percent;
    public decimal Apply(decimal price) => price * (1 - _percent / 100);
}

public class FixedAmountDiscount : IDiscountStrategy
{
    private readonly decimal _amount;
    public FixedAmountDiscount(decimal amount) => _amount = amount;
    public decimal Apply(decimal price) => Math.Max(0, price - _amount);
}

public class SeasonalDiscount : IDiscountStrategy
{
    public decimal Apply(decimal price) => price * 0.85m;
}

/// <summary>
/// Open for extension (new strategies), closed for modification.
/// </summary>
public class PriceCalculator
{
    private readonly IDiscountStrategy _discount;

    public PriceCalculator(IDiscountStrategy discount) => _discount = discount;

    public decimal Calculate(decimal price) => _discount.Apply(price);
}
```

---

#### LSP — Liskov Substitution Principle (Принцип подстановки Барбары Лисков)

**Суть:** Подтипы должны быть заменяемы своими базовыми типами без нарушения корректности программы. Поведение наследника не должно противоречить контракту базового типа.

**Почему важно:** Нарушение LSP приводит к неожиданным ошибкам при подстановке реализаций. Код, рассчитанный на базовый тип, ломается при работе с наследником.

**Пример нарушения:**

```csharp
/// <summary>
/// BAD: Square "is-a" Rectangle in math, but substitutability breaks.
/// Setting Width on Square also changes Height — caller expects Rectangle behavior.
/// </summary>
public class Rectangle
{
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }
    public int Area => Width * Height;
}

public class Square : Rectangle
{
    public override int Width
    {
        set { base.Width = base.Height = value; }  // Violates LSP!
    }
    public override int Height
    {
        set { base.Width = base.Height = value; }  // Caller expects independent Width/Height
    }
}

// Usage breaks expectations:
// Rectangle r = new Square();
// r.Width = 4; r.Height = 5;  // Expect Area = 20, but get 25 (Square overrides both)
```

**Правильный вариант:**

```csharp
/// <summary>
/// Base abstraction — both Rectangle and Square implement it correctly.
/// No substitutability violation: each type has consistent behavior.
/// </summary>
public interface IShape
{
    int Area { get; }
}

public class Rectangle : IShape
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int Area => Width * Height;
}

public class Square : IShape
{
    public int Side { get; set; }
    public int Area => Side * Side;
}

// Usage: work with IShape, no unexpected behavior
public int ComputeTotalArea(IEnumerable<IShape> shapes) =>
    shapes.Sum(s => s.Area);
```

---

#### ISP — Interface Segregation Principle (Принцип разделения интерфейса)

**Суть:** Клиенты не должны зависеть от интерфейсов, которые они не используют. Лучше много маленьких специализированных интерфейсов, чем один «толстый».

**Почему важно:** «Толстый» интерфейс заставляет реализации предоставлять методы-заглушки или выбрасывать исключения. Клиенты получают зависимости от ненужного API.

**Пример нарушения:**

```csharp
/// <summary>
/// BAD: Fat interface — printer must implement scan/fax even if it only prints.
/// Violates ISP — clients forced to depend on unused methods.
/// </summary>
public interface IMultiFunctionDevice
{
    void Print(Document doc);
    void Scan(Document doc);
    void Fax(Document doc);
}

public class SimplePrinter : IMultiFunctionDevice
{
    public void Print(Document doc) { /* OK */ }
    public void Scan(Document doc) => throw new NotSupportedException();  // Stub!
    public void Fax(Document doc) => throw new NotSupportedException();   // Stub!
}

// Client that only needs printing still depends on Scan/Fax
public class ReportGenerator
{
    private readonly IMultiFunctionDevice _device;
    public ReportGenerator(IMultiFunctionDevice device) => _device = device;
    public void PrintReport() => _device.Print(new Document());  // Only uses Print
}
```

**Правильный вариант:**

```csharp
/// <summary>
/// Segregated interfaces — clients depend only on what they use.
/// </summary>
public interface IPrinter
{
    void Print(Document doc);
}

public interface IScanner
{
    void Scan(Document doc);
}

public interface IFax
{
    void Fax(Document doc);
}

public class SimplePrinter : IPrinter
{
    public void Print(Document doc) { /* Only what it can do */ }
}

public class MultiFunctionMachine : IPrinter, IScanner, IFax
{
    public void Print(Document doc) { }
    public void Scan(Document doc) { }
    public void Fax(Document doc) { }
}

// Client depends only on IPrinter
public class ReportGenerator
{
    private readonly IPrinter _printer;
    public ReportGenerator(IPrinter printer) => _printer = printer;
    public void PrintReport() => _printer.Print(new Document());
}
```

---

#### DIP — Dependency Inversion Principle (Принцип инверсии зависимостей)

**Суть:** Модули верхнего уровня не должны зависеть от модулей нижнего уровня. Оба должны зависеть от абстракций. Абстракции не должны зависеть от деталей — детали зависят от абстракций.

**Почему важно:** Прямая зависимость от конкретных классов (SqlConnection, SmtpClient, FileSystem) делает код жёстко связанным, сложным для тестирования и замены реализаций.

**Пример нарушения:**

```csharp
/// <summary>
/// BAD: High-level service directly depends on low-level concrete classes.
/// Cannot unit test without real DB and email server.
/// </summary>
public class UserRegistrationService
{
    public void Register(string email, string password)
    {
        var validator = new EmailValidator();  // Concrete dependency
        if (!validator.IsValid(email))
            throw new ArgumentException("Invalid email");

        using var db = new SqlConnection("Server=...");  // Concrete DB
        db.Open();
        // ... insert user

        var smtp = new SmtpClient("smtp.gmail.com");   // Concrete email
        smtp.Send(/* ... */);
    }
}
```

**Правильный вариант:**

```csharp
/// <summary>
/// Abstractions — high-level module depends on these, not on concrete implementations.
/// </summary>
public interface IEmailValidator
{
    bool IsValid(string email);
}

public interface IUserRepository
{
    void Save(User user);
}

public interface IEmailSender
{
    void Send(string to, string subject, string body);
}

/// <summary>
/// High-level logic depends on abstractions. Concrete implementations injected from outside.
/// Easy to test with mocks, easy to swap DB/email provider.
/// </summary>
public class UserRegistrationService
{
    private readonly IEmailValidator _validator;
    private readonly IUserRepository _repository;
    private readonly IEmailSender _emailSender;

    public UserRegistrationService(
        IEmailValidator validator,
        IUserRepository repository,
        IEmailSender emailSender)
    {
        _validator = validator;
        _repository = repository;
        _emailSender = emailSender;
    }

    public void Register(string email, string password)
    {
        if (!_validator.IsValid(email))
            throw new ArgumentException("Invalid email");

        var user = new User { Email = email, PasswordHash = Hash(password) };
        _repository.Save(user);
        _emailSender.Send(email, "Welcome", "Your account has been created.");
    }

    private static string Hash(string password) => /* ... */;
}
```

---

#### Краткая сводка SOLID

| Принцип | Суть | Ключевая идея |
|---------|------|----------------|
| **SRP** | Один класс — одна ответственность | Разделяй зоны изменений |
| **OCP** | Открыт для расширения, закрыт для модификации | Расширяй через новые классы |
| **LSP** | Подтипы заменяют базовый тип без сюрпризов | Поведение наследника согласовано с контрактом |
| **ISP** | Маленькие специализированные интерфейсы | Не зависеть от неиспользуемого API |
| **DIP** | Зависимость от абстракций | Инжектируй интерфейсы, не конкретные классы |

---

### 59. Repository Pattern — зачем?

Абстракция доступа к данным. Скрывает детали БД, даёт единый интерфейс. Упрощает тестирование (можно подменить mock‑репозиторием) и смену источника данных.

---

### 60. Unit of Work — что это?

Паттерн, группирующий несколько операций с репозиториями в одну транзакцию. Commit выполняется один раз для всей единицы работы.

---

### 61. Что такое Generics?

Механизм параметризации типов. Позволяет писать общий код для разных типов с сохранением типобезопасности.

```csharp
public class Repository<T> where T : class
```

---

### 62. Ограничения where в Generics?

- `where T : class` — reference type
- `where T : struct` — value type
- `where T : SomeBase` — наследование
- `where T : IInterface` — реализация интерфейса
- `where T : new()` — есть публичный конструктор без параметров

---

### 63. Разница между List и Array?

**Array** — фиксированный размер, создаётся при инициализации.

**List<T>** — динамический размер, растёт при добавлении. Внутри использует массив.

---

### 64. Dictionary — как работает?

Хеш‑таблица: ключ → хеш → индекс. Обычно O(1) для добавления и поиска. Ключи должны быть уникальными, нужна корректная реализация `GetHashCode` и `Equals`.

---

### 65. ConcurrentDictionary — когда использовать?

Когда несколько потоков параллельно читают и пишут в словарь. Методы типа `AddOrUpdate`, `GetOrAdd` атомарны.

---

### 66. yield return — что делает?

Итератор: возвращает элементы по одному, не загружая всю коллекцию в память. Ленивое вычисление, удобно для больших последовательностей.

```csharp
foreach (var item in GetItems())
```

---

### 67. Что такое Reflection?

Механизм инспекции и вызова типов, методов, свойств во время выполнения. Используется в сериализаторах, ORM, IoC. Дорого по производительности.

---

### 68. Что такое Attribute?

Метаданные, прикрепляемые к типам и членам. Используются для сериализации, валидации, маршрутизации, тестов и т.д.

```csharp
[Required, MaxLength(100)]
public string Name { get; set; }
```

---

### 69. Разница между readonly и const в контексте полей?

`readonly` — поле можно задать в конструкторе, потом менять нельзя.  
`const` — константа времени компиляции, задаётся при объявлении.

---

### 70. Что такое volatile?

Модификатор для полей: чтения и записи не кэшируются процессором, что важно при многопоточности. Но для сложной синхронизации лучше `Interlocked`, `lock` или специализированные примитивы.

---

## БЛОК 4: ASP.NET Core / Web API

### 71. Что такое MVC?

Model-View-Controller. Model — данные и бизнес‑логика, View — отображение, Controller — обработка запросов и связь Model и View.

---

### 72. ViewBag vs ViewData vs TempData?

**ViewData** — словарь, требуется приведение типов.

**ViewBag** — обёртка над ViewData с dynamic, без IntelliSense.

**TempData** — живёт между redirect’ами, хранится в сессии.

В API обычно используются strongly-typed модели, а не эти механизмы.

---

### 73. Методы HTTP — когда что использовать?

- **GET** — чтение, идемпотентный
- **POST** — создание ресурса
- **PUT** — полная замена ресурса
- **PATCH** — частичное обновление
- **DELETE** — удаление, идемпотентный

---

### 74. Основные HTTP Status Codes?

Успех: 200 OK, 201 Created, 204 No Content.  
Клиент: 400 Bad Request, 401 Unauthorized, 403 Forbidden, 404 Not Found.  
Сервер: 500 Internal Server Error.

---

### 75. Что такое Middleware?

Компоненты в конвейере обработки HTTP. Каждый middleware обрабатывает запрос/ответ и либо вызывает следующий, либо завершает цепочку. Порядок важен: Routing, Authentication, Authorization, Endpoints.

---

#### 75.1. Request Pipeline — Use / Run / Map (детально)

**Request Pipeline** — цепочка компонентов (middleware), через которые проходит каждый HTTP-запрос. Каждый компонент получает `HttpContext`, выполняет свою логику и решает: вызвать следующий (`next`) или завершить обработку.

**Типы делегатов:**

- `RequestDelegate` — `Task Invoke(HttpContext context)` — один следующий обработчик.
- В pipeline каждый middleware получает `next` — ссылку на следующий в цепочке.

---

##### Use — добавляет middleware в цепочку (может вызвать next)

`app.Use(Func<HttpContext, RequestDelegate, Task> middleware)` — добавляет компонент, который **может** вызвать `next()` и передать управление дальше.

**Порядок выполнения:** сначала код до `await next(context)` (request), затем — после возврата из `next` (response, в обратном порядке).

```csharp
app.Use(async (context, next) =>
{
    // 1. BEFORE next — runs on REQUEST (incoming)
    var sw = Stopwatch.StartNew();
    context.Response.Headers["X-Request-Id"] = Guid.NewGuid().ToString();

    await next(context);  // Pass to next middleware, wait for completion

    // 2. AFTER next — runs on RESPONSE (outgoing, reverse order)
    sw.Stop();
    context.Response.Headers["X-Elapsed-Ms"] = sw.ElapsedMilliseconds.ToString();
});
```

**Важно:** Если не вызвать `next(context)`, pipeline обрывается — последующие middleware не выполнятся, ответ уже может быть отправлен.

---

##### Run — терминальный middleware (никогда не вызывает next)

`app.Run(RequestDelegate handler)` — добавляет **последний** компонент в цепочку. У него нет `next`, он всегда завершает pipeline.

```csharp
app.Run(async context =>
{
    context.Response.StatusCode = 404;
    await context.Response.WriteAsync("Not Found");
});
// Nothing after Run() will execute for this branch
```

**Особенности:**

- `Run` — это по сути `Use` с пустым `next` (handler не получает следующий делегат).
- Всё, что зарегистрировано после `Run`, не выполнится — Run «закрывает» ветку.

---

##### Map — ветвление pipeline по пути (path)

`app.Map(path, configuration)` — создаёт **отдельную ветку** pipeline для запросов, у которых путь начинается с `path`. Для совпадающих запросов выполняется только конфигурация внутри `Map`, а не основной pipeline.

```csharp
// Main pipeline
app.UseRouting();
app.UseAuthentication();

// Branch: only for /api/admin/*
app.Map("/api/admin", adminApp =>
{
    adminApp.UseAuthorization();  // Extra auth for admin
    adminApp.UseEndpoints(e => e.MapControllers());
});

// Branch: only for /health — simple terminal
app.Map("/health", healthApp =>
{
    healthApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { status = "OK" });
    });
});

// Main pipeline continues for non-matching paths
app.MapControllers();
```

**Поведение:**

- `Map` сопоставляет по **префиксу** пути. `/api/admin/users` попадает в ветку `/api/admin`.
- `HttpContext.Request.Path` внутри ветки **обрезается**: остаётся `/users` (PathBase = `/api/admin`).
- Если путь не совпал — запрос идёт по основному pipeline дальше.

---

##### MapWhen — условное ветвление

`app.MapWhen(Func<HttpContext, bool> predicate, configuration)` — ветвление по произвольному условию, не только по пути.

```csharp
app.MapWhen(ctx => ctx.Request.Headers["X-Use-V2"] == "true", v2App =>
{
    v2App.UseMiddleware<V2ApiMiddleware>();
    v2App.MapControllers();
});
```

---

##### UseWhen — условный middleware (с возвратом в основной pipeline)

`app.UseWhen(predicate, configuration)` — добавляет middleware **только при выполнении условия**, но после их выполнения управление возвращается в основной pipeline (в отличие от Map, который создаёт отдельную ветку).

```csharp
app.UseWhen(
    ctx => ctx.Request.Path.StartsWithSegments("/api"),
    appBuilder => appBuilder.UseMiddleware<ApiRequestLogger>()
);
// After ApiRequestLogger, request continues in main pipeline
```

---

##### Сводка Use / Run / Map

| Метод | Назначение | Вызывает next? | Ветвление |
|-------|------------|----------------|-----------|
| **Use** | Добавить middleware в цепочку | Да (опционально) | Нет |
| **Run** | Терминальный handler | Нет (завершает) | Нет |
| **Map** | Ветка по path | Внутри ветки — своя цепочка | Да (по пути) |
| **MapWhen** | Ветка по условию | Внутри ветки | Да (по predicate) |
| **UseWhen** | Условный middleware | Да, возврат в основной pipeline | Условное включение |

---

##### Порядок выполнения (схема)

```
Request
    │
    ▼
┌─────────────────────┐
│ ExceptionHandler     │  Use
└──────────┬──────────┘
           │ next()
           ▼
┌─────────────────────┐
│ HTTPS Redirect      │  Use
└──────────┬──────────┘
           │ next()
           ▼
┌─────────────────────┐
│ Routing             │  Use
└──────────┬──────────┘
           │ next()
           ▼
     ┌─────┴─────┐
     │ Map?      │  /health → Run (terminal)
     │           │  /api/admin → branch
     └─────┬─────┘
           │
           ▼
┌─────────────────────┐
│ Authentication      │  Use
└──────────┬──────────┘
           │ next()
           ▼
┌─────────────────────┐
│ Authorization       │  Use
└──────────┬──────────┘
           │ next()
           ▼
┌─────────────────────┐
│ Endpoints           │  Run (MapControllers, etc.)
└─────────────────────┘
           │
           ▼
       Response
```

---

### 76. Жизненный цикл запроса в ASP.NET Core?

Request → Middleware (Routing, Auth, …) → Endpoint (Controller/Action) → Model Binding → Validation → Action → Response.

---

### 77. Как организована аутентификация?

Через Authentication Middleware. Варианты: JWT, Cookie, OAuth, OIDC. В `Program.cs` настраивают `AddAuthentication`, `AddJwtBearer` и т.д. На контроллерах/действиях — `[Authorize]`.

---

### 78. JWT — как работает?

Токен из трёх частей (header.payload.signature). Подписывается секретом. Сервер проверяет подпись и claims. Stateless, удобен для API и микросервисов.

---

### 79. [Authorize] и роли?

`[Authorize(Roles = "Admin")]` — доступ только для указанных ролей. Роли могут браться из claims в JWT или из провайдера (Identity и т.п.).

---

### 80. Model Binding — что это?

Автоматическое связывание данных запроса (query, body, route, headers) с параметрами action. Поддерживаются примитивы, сложные типы, коллекции.

---

### 81. Model Validation — как реализуется?

Data Annotations (`[Required]`, `[MaxLength]` и т.д.) или FluentValidation. Проверка через `ModelState.IsValid` в action. Невалидные данные → 400 Bad Request.

---

### 82. Что такое Action Filter?

Атрибут, выполняемый до или после action. Используется для логирования, проверки прав, трансформации результата, кэширования.

---

### 83. Разница между IActionResult и ActionResult<T>?

**IActionResult** — абстракция результата (Ok, NotFound, BadRequest и т.д.).

**ActionResult<T>** — добавляет типизацию для документирования в Swagger и удобства возврата `T` или обёрнутого результата.

---

### 84. Что такое Program.cs и минимальный хостинг?

С C# 10 / .NET 6 точка входа и конфигурация могут быть в одном `Program.cs` без явного `Main` и `Startup`. Хостинг настраивается через `WebApplication.CreateBuilder()` и `app.Run()`.

---

### 85. appsettings.json vs переменные окружения?

`appsettings.json` — базовая конфигурация. Переменные окружения переопределяют её (обычно для секретов и окружений). Иерархия: appsettings → appsettings.{Environment} → переменные окружения.

---

### 86. Что такое Options pattern?

Паттерн для strongly-typed конфигурации. Секция из `IConfiguration` биндится в класс, регистрируется через `AddOptions<T>()` и внедряется как `IOptions<T>`.

---

### 87. Как логировать в ASP.NET Core?

Через `ILogger<T>`. Провайдеры: Console, Debug, файл (NLog, Serilog). Уровни: Trace, Debug, Information, Warning, Error, Critical.

---

### 88. Global Exception Handler?

Middleware, перехватывающий необработанные исключения. Логирует, возвращает корректный статус и JSON. Не раскрывает детали клиенту в продакшене.

---

### 89. CORS — что это и зачем?

Cross-Origin Resource Sharing. Браузер блокирует запросы с другого origin, если сервер не разрешает их через CORS. Настраивается в `AddCors` и `UseCors`.

---

### 90. Что такое Swagger/OpenAPI?

Стандарт описания REST API. Swashbuckle генерирует UI из контроллеров и атрибутов. Используется для документации и тестирования API.

---

### 91. Health Checks?

Эндпоинты для проверки состояния приложения и зависимостей (БД, внешние сервисы). Используются оркестраторами и балансировщиками.

---

### 92. Rate Limiting?

Ограничение частоты запросов с клиента. Защита от DDoS и злоупотреблений. В ASP.NET Core — встроенный middleware.

---

### 93. Что такое Response Caching?

Кэширование ответа по ключу (URL, заголовки). Следующие запросы могут обслуживаться из кэша без выполнения action.

---

### 94. Minimal API vs Controllers?

Minimal API — маршруты и логика прямо в `Program.cs` или extension-методах. Меньше шаблонного кода. Controllers — классический подход с `[ApiController]`, удобнее для крупных API.

---

### 95. Что такое API Versioning?

Поддержка нескольких версий API (URL, заголовок, query). Позволяет развивать API без ломания старых клиентов.

---

### 96. Background Services — как создать? Worker Service, IHostedService

Класс, наследующий `BackgroundService`, переопределяющий `ExecuteAsync`. Регистрируется как `AddHostedService<T>()`. Используется для фоновых задач, очередей, периодической работы.

#### Worker Service — что это?

**Worker Service** — шаблон проекта в .NET для создания долгоживущих фоновых приложений без веб-сервера. Подходит для: обработки очередей, периодических задач (синхронизация, отчёты), мониторинга, работы с сообщениями (RabbitMQ, Kafka).

Создание: `dotnet new worker -n MyWorker` или через Visual Studio «Worker Service».

#### Интерфейс IHostedService

Базовый контракт для фоновых сервисов. Два метода:

```csharp
public interface IHostedService
{
    // Вызывается при запуске приложения
    Task StartAsync(CancellationToken cancellationToken);
    // Вызывается при остановке (graceful shutdown)
    Task StopAsync(CancellationToken cancellationToken);
}
```

**BackgroundService** — абстрактный класс, реализующий `IHostedService` и добавляющий `ExecuteAsync(CancellationToken)` — основной цикл работы. Хост вызывает `StartAsync` → запускает `ExecuteAsync` в фоне → при остановке отменяет `CancellationToken` и вызывает `StopAsync`.

#### Создание Worker Service — полный пример

```csharp
// Worker.cs — фоновый сервис
public class DataSyncWorker : BackgroundService
{
    private readonly ILogger<DataSyncWorker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public DataSyncWorker(ILogger<DataSyncWorker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("DataSyncWorker started at {Time}", DateTimeOffset.UtcNow);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Создаём scope для Scoped-сервисов (DbContext и т.д.)
                using var scope = _serviceProvider.CreateScope();
                var syncService = scope.ServiceProvider.GetRequiredService<IDataSyncService>();

                await syncService.SyncAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during sync");
            }

            // Периодичность: каждые 5 минут
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }

        _logger.LogInformation("DataSyncWorker stopped at {Time}", DateTimeOffset.UtcNow);
    }
}
```

#### Program.cs для Worker Service

```csharp
// Program.cs
var builder = Host.CreateApplicationBuilder(args);

// Регистрация зависимостей (как в Web API)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IDataSyncService, DataSyncService>();

// Регистрация hosted service
builder.Services.AddHostedService<DataSyncWorker>();

var host = builder.Build();
await host.RunAsync();
```

#### Важно: Scoped-сервисы в Worker

Worker живёт как **Singleton**. Нельзя инжектить **Scoped** (DbContext, репозитории) напрямую в конструктор — получится captive dependency. Решение: `IServiceProvider.CreateScope()` + `GetRequiredService<T>()` внутри цикла — каждый цикл получает свой scope и свой DbContext.

#### Тот же сервис в ASP.NET Core Web API

В Web API Worker регистрируется так же: `builder.Services.AddHostedService<DataSyncWorker>()`. Хост ASP.NET Core поддерживает `IHostedService` — фоновые задачи работают параллельно с обработкой HTTP-запросов.

---

### 97. SignalR — что это?

**SignalR** — библиотека ASP.NET Core для **двусторонней real-time коммуникации** между сервером и клиентами. Сервер может в любой момент отправить данные подключённым клиентам без ожидания запроса (push-модель). Клиенты подключаются к **Hub** и вызывают методы на сервере; сервер вызывает методы на клиентах по соединению, группе или всем.

**Транспорты (в порядке предпочтения при согласовании):**

- **WebSockets** — полноценный двусторонний канал, один постоянный TCP-соединение.
- **Server-Sent Events (SSE)** — сервер → клиент; клиент использует обычный HTTP для отправки.
- **Long Polling** — клиент периодически держит открытый запрос; когда серверу есть что отправить, запрос завершается с данными и клиент сразу делает следующий. Работает везде, но менее эффективен.

SignalR сам выбирает лучший доступный транспорт; при необходимости можно ограничить транспорт в конфигурации.

**Основные концепции:**

- **Hub** — класс на сервере, наследник `Hub`. Клиенты вызывают методы Hub; методы Hub могут вызывать методы на клиентах (`Clients.Caller`, `Clients.All`, `Clients.Group(...)`, `Clients.User(...)`).
- **ConnectionId** — уникальный идентификатор соединения. Можно привязывать пользователя к connectionId при входе (например, через `Context.User` при использовании аутентификации).
- **Groups** — клиенты подписываются на группы (`Groups.AddToGroupAsync`); рассылка по группе: `Clients.Group("Room1").SendAsync(...)`.

**Пример Hub на сервере (ASP.NET Core):**

```csharp
// NuGet: Microsoft.AspNetCore.SignalR
public class ChatHub : Hub
{
    // Клиент вызывает SendMessage → сервер шлёт сообщение всем в группе
    public async Task SendMessage(string groupName, string user, string message)
    {
        await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
    }

    // Подключение клиента к группе (комнате чата)
    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}
```

**Регистрация в Program.cs:**

```csharp
builder.Services.AddSignalR();
// ...
app.MapHub<ChatHub>("/hubs/chat");
```

**Пример клиента (JavaScript в браузере):**

```javascript
const connection = new signalR.HubConnectionBuilder()
  .withUrl("/hubs/chat")
  .withAutomaticReconnect()
  .build();

// Слушаем метод, который вызывает сервер
connection.on("ReceiveMessage", (user, message) => {
  console.log(`${user}: ${message}`);
});

await connection.start();
await connection.invoke("JoinGroup", "Room1");
await connection.invoke("SendMessage", "Room1", "Alice", "Hello!");
```

**Типичные сценарии:**

- Чаты, коллаборативные редакторы.
- Уведомления (новый заказ, статус задачи).
- Дашборды с обновлением графиков в реальном времени.
- Игры, голосования, онлайн-статусы.

**Важно на собеседовании:**

- Отличие от обычного REST: клиент не опрашивает сервер; сервер сам шлёт данные при событиях.
- Масштабирование: при нескольких серверах нужен **backplane** (например, Redis или Azure SignalR Service), чтобы сообщения доходили до клиентов, подключённых к другому узлу.
- Аутентификация: обычно через cookie/JWT при установке соединения; на Hub доступен `Context.User`.
- `WithAutomaticReconnect()` на клиенте — встроенное переподключение при обрыве.

---

### 98. gRPC vs REST?

**REST** — JSON/HTTP, универсальный, удобен для браузеров.

**gRPC** — бинарный протокол поверх HTTP/2, контракты через protobuf. Лучше для внутренних сервисов, микросервисов, стриминга.

---

### 99. Что такое Kestrel?

Веб‑сервер по умолчанию в ASP.NET Core. Кросс-платформенный, асинхронный, настраиваемый. Может работать за обратным прокси (IIS, nginx).

---

### 100. Разница между AddDbContext и AddDbContextPool?

`AddDbContext` создаёт новый контекст на scope. `AddDbContextPool` переиспользует экземпляры контекста из пула, снижая накладные расходы на создание.

---

## БЛОК 5: SQL Server

Блок посвящён SQL Server и общим темам SQL на собеседованиях: ключи, JOIN, индексы, транзакции, оптимизация, а также **частым практическим задачам** (дубликаты, топ-N, вторая максимальная зарплата, ROW_NUMBER/RANK, разница WHERE/HAVING и т.д.) с примерами кода.

---

### Частые темы на собеседовании (с примерами SQL)

Ниже — темы, которые встречаются в ~80% SQL-вопросов, с готовыми примерами запросов.

---

#### WHERE vs HAVING

| | WHERE | HAVING |
|---|--------|--------|
| **Когда выполняется** | До группировки (фильтрация строк) | После группировки (фильтрация групп) |
| **С чем можно использовать** | Любые столбцы и выражения | Только агрегаты (COUNT, SUM, AVG…) и столбцы из GROUP BY |
| **Типичное использование** | Отсечь строки до группировки | Отсечь группы по условию на агрегат |

**Пример:** вывести отделы, в которых средняя зарплата больше 50 000.

```sql
-- HAVING: фильтр по результату агрегации (после GROUP BY)
SELECT DepartmentId, AVG(Salary) AS AvgSalary
FROM Employees
GROUP BY DepartmentId
HAVING AVG(Salary) > 50000;

-- WHERE: отфильтровать строки до группировки (например, только активные сотрудники)
SELECT DepartmentId, AVG(Salary) AS AvgSalary
FROM Employees
WHERE IsActive = 1
GROUP BY DepartmentId
HAVING AVG(Salary) > 50000;
```

---

#### Что делает GROUP BY?

`GROUP BY` объединяет строки с одинаковыми значениями указанных столбцов и позволяет считать по ним агрегаты (COUNT, SUM, AVG, MIN, MAX). В SELECT можно указывать только столбцы из GROUP BY и агрегатные функции.

**Пример:** количество сотрудников и сумма зарплат по отделам.

```sql
SELECT DepartmentId,
       COUNT(*) AS EmployeeCount,
       SUM(Salary) AS TotalSalary,
       AVG(Salary) AS AvgSalary
FROM Employees
GROUP BY DepartmentId;
```

---

#### ROW_NUMBER, RANK, DENSE_RANK (с примерами)

Окно задаётся через `OVER (PARTITION BY ... ORDER BY ...)`.

- **ROW_NUMBER()** — уникальный порядковый номер строки в рамках партиции (при равенстве ORDER BY номера всё равно разные).
- **RANK()** — ранг с «дырами»: при равных значениях один ранг, следующий ранг пропускается (1, 2, 2, 4).
- **DENSE_RANK()** — ранг без пропусков (1, 2, 2, 3).

**Пример:** нумерация и ранги по зарплате внутри отдела.

```sql
SELECT Id, Name, DepartmentId, Salary,
       ROW_NUMBER() OVER (PARTITION BY DepartmentId ORDER BY Salary DESC) AS RowNum,
       RANK()       OVER (PARTITION BY DepartmentId ORDER BY Salary DESC) AS RankVal,
       DENSE_RANK() OVER (PARTITION BY DepartmentId ORDER BY Salary DESC) AS DenseRankVal
FROM Employees;
```

---

#### Разница между DELETE и TRUNCATE (с примерами)

| | DELETE | TRUNCATE |
|---|--------|----------|
| **Условие** | Можно с WHERE (выборочное удаление) | Только вся таблица |
| **Логирование** | Каждая строка в логе транзакций | Минимальное (освобождение страниц) |
| **IDENTITY** | Не сбрасывается | Сбрасывается до seed |
| **Триггеры** | Вызываются | Не вызываются |
| **FK** | Можно при наличии FK на другие таблицы | Часто не допускается при FK |

```sql
-- DELETE: удалить только строки, удовлетворяющие условию; можно откатить в транзакции
BEGIN TRANSACTION;
DELETE FROM Employees WHERE DepartmentId = 5;
-- ROLLBACK; -- откат
COMMIT;

-- TRUNCATE: удалить все строки таблицы, сброс IDENTITY, быстрее
TRUNCATE TABLE LogArchive;
```

---

#### Subquery (подзапрос)

Подзапрос — запрос внутри другого запроса. Может использоваться в SELECT, FROM, WHERE, HAVING. Должен возвращать скаляр (одно значение) или таблицу в зависимости от контекста.

```sql
-- Скалярный подзапрос в WHERE: сотрудники с зарплатой выше средней
SELECT * FROM Employees
WHERE Salary > (SELECT AVG(Salary) FROM Employees);

-- Подзапрос в FROM (производная таблица)
SELECT DeptId, Cnt
FROM (SELECT DepartmentId AS DeptId, COUNT(*) AS Cnt FROM Employees GROUP BY DepartmentId) AS T
WHERE Cnt > 10;

-- Подзапрос с EXISTS (часто эффективнее IN на больших наборах)
SELECT * FROM Orders o
WHERE EXISTS (SELECT 1 FROM OrderDetails d WHERE d.OrderId = o.Id AND d.Quantity > 5);
```

---

#### Transaction и ACID

**Транзакция** — группа операций, выполняемая как одно целое: либо все изменения применяются (COMMIT), либо все откатываются (ROLLBACK).

**ACID:**

- **Atomicity** — все операции в транзакции выполняются или ни одна.
- **Consistency** — БД переходит из одного согласованного состояния в другое.
- **Isolation** — параллельные транзакции не «видят» незакоммиченные изменения друг друга так, чтобы нарушить согласованность.
- **Durability** — после COMMIT изменения сохраняются даже при сбое.

```sql
BEGIN TRANSACTION;
  UPDATE Accounts SET Balance = Balance - 100 WHERE Id = 1;
  UPDATE Accounts SET Balance = Balance + 100 WHERE Id = 2;
  -- Если что-то пошло не так:
  -- ROLLBACK;
COMMIT;
```

---

#### View (представление)

**View** — сохранённый запрос, который ведёт себя как виртуальная таблица. Удобно для упрощения доступа и сокрытия сложности. Индексированное представление (materialized view) в SQL Server хранит данные физически.

```sql
CREATE VIEW v_EmployeeSummary AS
SELECT DepartmentId,
       COUNT(*) AS EmployeeCount,
       AVG(Salary) AS AvgSalary
FROM Employees
GROUP BY DepartmentId;

-- Использование
SELECT * FROM v_EmployeeSummary WHERE AvgSalary > 50000;
```

---

#### Trigger (триггер)

**Триггер** — код, выполняемый автоматически при INSERT/UPDATE/DELETE. Используется для аудита, каскадных изменений, бизнес-правил. Может усложнять отладку и влиять на производительность.

```sql
-- Пример: запись в таблицу аудита при обновлении зарплаты
CREATE TRIGGER tr_Employees_AuditSalary
ON Employees
AFTER UPDATE
AS
  IF UPDATE(Salary)
  INSERT INTO SalaryAudit (EmployeeId, OldSalary, NewSalary, ChangedAt)
  SELECT d.Id, d.Salary, i.Salary, GETUTCDATE()
  FROM deleted d
  INNER JOIN inserted i ON d.Id = i.Id;
```

---

#### Нормализация

**Нормализация** — разбиение таблиц для уменьшения дублирования и аномалий обновления/удаления. Степени: 1NF (атомарность, уникальность строк), 2NF (зависимость неключевых атрибутов от полного ключа), 3NF (нет транзитивных зависимостей), BCNF и далее. Для производительности иногда применяют денормализацию.

---

### Практические задачи с примерами SQL

Ниже — типовые задачи на собеседованиях с готовыми решениями.

---

#### 1. Найти дубликаты

Найти значения колонки (или комбинации колонок), которые встречаются больше одного раза.

```sql
-- Дубликаты по одному полю (например, Email)
SELECT Email, COUNT(*) AS Cnt
FROM Users
GROUP BY Email
HAVING COUNT(*) > 1;

-- Получить все строки-дубликаты (все строки с повторяющимся Email)
SELECT * FROM Users u
WHERE u.Email IN (
  SELECT Email FROM Users GROUP BY Email HAVING COUNT(*) > 1
);

-- Вариант через ROW_NUMBER: оставить один «образец», остальные считать дубликатами
SELECT * FROM (
  SELECT *, ROW_NUMBER() OVER (PARTITION BY Email ORDER BY Id) AS rn
  FROM Users
) t
WHERE rn > 1;
```

---

#### 2. Найти вторую максимальную зарплату

```sql
-- Через подзапрос и MAX
SELECT MAX(Salary) AS SecondMaxSalary
FROM Employees
WHERE Salary < (SELECT MAX(Salary) FROM Employees);

-- Через DENSE_RANK (вторая по величине с учётом возможных равенств)
SELECT Salary AS SecondMaxSalary
FROM (
  SELECT Salary, DENSE_RANK() OVER (ORDER BY Salary DESC) AS dr
  FROM (SELECT DISTINCT Salary FROM Employees) s
) t
WHERE dr = 2;

-- Через OFFSET/FETCH (SQL Server 2012+): вторая строка после сортировки по убыванию
SELECT DISTINCT Salary
FROM Employees
ORDER BY Salary DESC
OFFSET 1 ROWS FETCH NEXT 1 ROWS ONLY;
```

---

#### 3. Найти последнюю запись

«Последняя» обычно по дате или по максимальному Id.

```sql
-- Последняя по Id
SELECT TOP 1 * FROM Orders ORDER BY Id DESC;

-- Последняя по дате по каждой группе (например, последний заказ клиента)
SELECT * FROM (
  SELECT *, ROW_NUMBER() OVER (PARTITION BY CustomerId ORDER BY OrderDate DESC) AS rn
  FROM Orders
) t
WHERE rn = 1;

-- Через подзапрос: строки, у которых Id равен максимуму в своей группе
SELECT o.* FROM Orders o
INNER JOIN (
  SELECT CustomerId, MAX(OrderDate) AS LastDate FROM Orders GROUP BY CustomerId
) last ON o.CustomerId = last.CustomerId AND o.OrderDate = last.LastDate;
```

---

#### 4. Найти топ-N записей (например, топ-3)

```sql
-- Топ-3 сотрудника по зарплате по всей компании
SELECT TOP 3 * FROM Employees ORDER BY Salary DESC;

-- Топ-3 по зарплате в каждом отделе (через ROW_NUMBER)
SELECT * FROM (
  SELECT *,
         ROW_NUMBER() OVER (PARTITION BY DepartmentId ORDER BY Salary DESC) AS rn
  FROM Employees
) t
WHERE rn <= 3;

-- Топ-3 через OFFSET/FETCH
SELECT * FROM Employees
ORDER BY Salary DESC
OFFSET 0 ROWS FETCH NEXT 3 ROWS ONLY;
```

---

#### 5. Использовать ROW_NUMBER

Типичные сценарии: дедупликация (оставить одну запись на группу), пагинация, нумерация.

```sql
-- Оставить по одной записи на пользователя (с наибольшим Id)
DELETE FROM DuplicateLog
WHERE Id NOT IN (
  SELECT Id FROM (
    SELECT Id, ROW_NUMBER() OVER (PARTITION BY UserId ORDER BY Id DESC) AS rn
    FROM DuplicateLog
  ) t
  WHERE rn = 1
);

-- Пагинация: вторая страница по 10 записей
SELECT * FROM (
  SELECT *, ROW_NUMBER() OVER (ORDER BY CreatedAt DESC) AS rn
  FROM Products
) t
WHERE rn > 10 AND rn <= 20;
```

---

#### 6. JOIN — все основные типы с примерами

Пусть есть таблицы `Employees (Id, Name, DepartmentId)` и `Departments (Id, Name)`.

```sql
-- INNER JOIN — только сотрудники, у которых есть отдел (совпадение с обеих сторон)
SELECT e.Name, d.Name AS DepartmentName
FROM Employees e
INNER JOIN Departments d ON e.DepartmentId = d.Id;

-- LEFT JOIN — все сотрудники; если отдела нет, справа NULL
SELECT e.Name, d.Name AS DepartmentName
FROM Employees e
LEFT JOIN Departments d ON e.DepartmentId = d.Id;

-- RIGHT JOIN — все отделы; сотрудники без отдела не попадут, отделы без сотрудников — попадут
SELECT e.Name, d.Name AS DepartmentName
FROM Employees e
RIGHT JOIN Departments d ON e.DepartmentId = d.Id;

-- FULL OUTER JOIN — все сотрудники и все отделы; при отсутствии связи — NULL
SELECT e.Name, d.Name AS DepartmentName
FROM Employees e
FULL OUTER JOIN Departments d ON e.DepartmentId = d.Id;

-- CROSS JOIN — каждая строка слева с каждой справа (декартово произведение)
SELECT e.Name, d.Name AS DepartmentName
FROM Employees e
CROSS JOIN Departments d;
```

---

### 101. Primary Key и Foreign Key?

**Primary Key** — уникальный идентификатор строки, NOT NULL, обычно один на таблицу.

**Foreign Key** — ссылка на Primary Key другой таблицы. Обеспечивает ссылочную целостность.

---

### 102. Типы JOIN?

- **INNER JOIN** — только совпадающие строки с обеих таблиц.
- **LEFT JOIN** — все строки слева + совпадения справа (справа NULL при отсутствии совпадения).
- **RIGHT JOIN** — все строки справа + совпадения слева.
- **FULL OUTER JOIN** — все строки с обеих сторон (NULL там, где нет пары).
- **CROSS JOIN** — декартово произведение (каждая строка первой таблицы с каждой второй).

Готовые примеры запросов по всем типам JOIN см. в разделе **«Практические задачи» → «6. JOIN»** выше в этом блоке.

---

### 103. Что такое Index? Clustered vs Non-Clustered?

**Index** — структура для ускорения поиска (аналог индекса в книге).

**Clustered** — определяет физический порядок данных. Один на таблицу, часто на Primary Key.

**Non-Clustered** — отдельная структура с указателями на строки. Несколько индексов на таблицу. Ускоряет SELECT, замедляет INSERT/UPDATE.

---

### 104. Что такое транзакция?

Группа операций, выполняемых как единое целое (ACID). Либо все применяются (COMMIT), либо все откатываются (ROLLBACK).

---

### 105. ACID — что значит?

- **Atomicity** — либо все операции, либо ни одна
- **Consistency** — данные остаются в допустимом состоянии
- **Isolation** — транзакции не мешают друг другу
- **Durability** — зафиксированные изменения сохраняются

---

### 106. Уровни изоляции транзакций?

Read Uncommitted, Read Committed, Repeatable Read, Serializable, Snapshot. Влияют на видимость изменений других транзакций и риск блокировок/аномалий.

---

### 107. Как оптимизировать медленный запрос?

1. Смотреть execution plan  
2. Добавлять индексы по полям в WHERE/JOIN  
3. Избегать `SELECT *`, выбирать нужные колонки  
4. Использовать пагинацию  
5. Рассматривать материализацию, денормализацию, переписывание запроса

---

#### Query Performance — практические правила

- **Избегать `SELECT *`** — выбирать только нужные столбцы. Меньше данных по сети и по памяти, оптимизатор может лучше использовать индексы (covering index).
- **JOIN вместо подзапроса, где уместно** — часто один запрос с JOIN эффективнее нескольких подзапросов или запросов в цикле (в приложении). Для сложной логики подзапрос и JOIN сравнивать по execution plan.
- **Анализировать Execution Plan** — смотреть на Index Seek/Scan, количество прочитанных строк, сортировки, тип JOIN. Искать тяжёлые операции (Table Scan на больших таблицах, избыточные сортировки).
- **Мониторить медленные запросы** — расширенные события (Extended Events), логирование запросов дольше N секунд, Application Insights / мониторинг БД. Находить узкие места по факту, а не «на глаз».

**Пример: только нужные колонки вместо SELECT \***

```sql
-- Хуже: тянет все колонки
SELECT * FROM Orders WHERE CustomerId = 1;

-- Лучше: только то, что нужно для ответа
SELECT Id, OrderDate, TotalAmount, Status FROM Orders WHERE CustomerId = 1;
```

---

### 108. Stored Procedure vs динамический SQL?

**Stored Procedure** — предкомпилированная, переиспользуемая логика на сервере.  
**Динамический SQL** — гибкость, но выше риск SQL Injection и сложнее поддержка. Для сложной повторяющейся логики чаще выбирают SP.

---

### 109. N+1 проблема?

Когда для каждой строки результата выполняется отдельный запрос (например, подгрузка связанных сущностей). Решение: JOIN, Include в EF, batch-запросы.

---

### 110. Разница между UNION и UNION ALL?

**UNION** — объединяет результаты, убирает дубликаты.  
**UNION ALL** — объединяет без удаления дубликатов, быстрее.

---

### 111. Что такое CTE (Common Table Expression)?

Временный именованный результат запроса в рамках одного выражения. Удобен для иерархий и разбиения сложного запроса.

```sql
WITH Sales AS (SELECT * FROM Orders WHERE Year = 2024)
SELECT * FROM Sales;
```

---

### 112. Что такое подзапрос (subquery)?

Запрос внутри другого запроса. Может использоваться в SELECT, FROM, WHERE, HAVING. Возвращает скаляр (одно значение) или таблицу в зависимости от контекста. Примеры скалярного подзапроса, производной таблицы и EXISTS см. в разделе **«Subquery (подзапрос)»** выше в этом блоке.

---

### 113. EXISTS vs IN?

**EXISTS** — проверяет наличие строк, может остановиться на первой. Часто эффективнее для больших таблиц.  
**IN** — проверка вхождения в набор значений. Подзапрос может материализоваться полностью.

---

### 114. Что такое нормализация?

Разбиение таблиц для уменьшения дублирования и аномалий. НФ: 1NF, 2NF, 3NF, BCNF и т.д. Иногда для производительности применяют денормализацию.

---

### 115. Что такое триггер?

Код, выполняемый при INSERT/UPDATE/DELETE. Используется для аудита, каскадов, бизнес‑правил. Может усложнять отладку и производительность.

---

### 116. Что такое View?

Сохранённый запрос, работающий как виртуальная таблица. Упрощает доступ к данным, может скрывать сложность. Materialized View (индексированное представление) хранит данные физически.

---

### 117. Разница между DELETE и TRUNCATE?

**DELETE** — удаляет строки по условию (WHERE), пишет каждую строку в лог транзакций, можно откатить в транзакции, вызываются триггеры.  
**TRUNCATE** — удаляет все строки таблицы, сбрасывает IDENTITY, быстрее, почти не логирует построчно, триггеры не вызываются. Сравнительная таблица и примеры кода см. в разделе **«Разница между DELETE и TRUNCATE»** выше в этом блоке.

---

### 118. Что такое Execution Plan?

План выполнения запроса, показывающий, как оптимизатор обрабатывает запрос: индексы, JOIN, сортировки. Помогает находить узкие места.

---

### 119. Deadlock в SQL — как избежать?

Одинаковый порядок доступа к объектам во всех транзакциях, короткие транзакции, правильный уровень изоляции, использование `TRY/CATCH` и retry при deadlock.

---

### 120. Что такое ROW_NUMBER, RANK, DENSE_RANK?

Ранжирующие оконные функции (задают окно через `OVER (PARTITION BY ... ORDER BY ...)`):

- **ROW_NUMBER()** — уникальный порядковый номер строки в партиции; при равных значениях ORDER BY номера всё равно разные (1, 2, 3, 4).
- **RANK()** — ранг с пропусками: при равенстве один ранг, следующий пропускается (1, 2, 2, 4).
- **DENSE_RANK()** — ранг без пропусков (1, 2, 2, 3).

Используются для топ-N по группе, дедупликации, «вторая максимальная зарплата» и т.п. Примеры синтаксиса и запросов см. в разделах **«ROW_NUMBER, RANK, DENSE_RANK»** и **«Практические задачи» (п. 4, 5)** выше в этом блоке.

---

## БЛОК 6: React и Frontend

### 121. React — библиотека или фреймворк?

Библиотека для построения UI. Роутинг, state management и др. выбираются отдельно. Фреймворки (например, Angular) предоставляют всё «из коробки».

---

### 122. Что такое Virtual DOM?

Абстракция над реальным DOM. React хранит lightweight-копию в памяти, сравнивает при изменениях и вносит только необходимые правки в DOM. Это уменьшает число операций с DOM.

---

### 123. useState — как работает?

Хук для локального состояния. Возвращает текущее значение и функцию обновления. При обновлении компонент перерисовывается. Состояние сохраняется между рендерами.

---

### 124. useEffect — когда использовать?

Для побочных эффектов: подписки, запросы, работа с DOM. Зависимости указываются во втором аргументе. Пустой массив — выполнить один раз при монтировании.

---

### 125. useCallback и useMemo — зачем?

**useCallback** — мемоизирует функцию, чтобы не создавать её при каждом рендере. Полезно для зависимостей в `useEffect` и пропсов в дочерних компонентах.

**useMemo** — мемоизирует результат вычислений. Уменьшает лишние вычисления при одинаковых входных данных.

---

### 126. useContext — что это?

Позволяет передавать данные вглубь дерева компонентов без prop drilling. Создаётся контекст, Provider оборачивает часть дерева, потребители вызывают `useContext`.

---

### 127. Разница между controlled и uncontrolled компонентами?

**Controlled** — значение управляется state, onChange обновляет state.  
**Uncontrolled** — значение берётся из DOM (ref). Controlled предпочтительнее для React.

---

### 128. Жизненный цикл компонента?

Монтирование (mount) → обновление (update) → размонтирование (unmount). В функциональных компонентах аналог — `useEffect` с зависимостями и cleanup.

---

### 129. Что такое React.memo?

HOC для мемоизации компонента. Повторный рендер только при изменении props. Снижает лишние перерисовки при частых обновлениях родителя.

---

### 130. Что такое TypeScript?

Надмножество JavaScript со статической типизацией. Улучшает надёжность, даёт автодополнение и ранний поиск ошибок. Компилируется в JavaScript.

---

### 131. interface vs type в TypeScript?

**interface** — для описания формы объекта, может расширяться.  
**type** — для unions, примитивов, сложных типов. В большинстве случаев можно использовать оба, интерфейсы удобнее для объектов.

---

### 132. Что такое Generics в TypeScript?

Параметризация типов. Позволяет писать универсальный код с сохранением типов.

```typescript
function identity<T>(arg: T): T { return arg; }
```

---

### 133. Что такое React Router?

Библиотека для маршрутизации в SPA. Определяет соответствие URL и компонентов, поддерживает вложенные маршруты, параметры, программную навигацию.

---

### 134. Axios vs Fetch?

**Fetch** — встроенный API. **Axios** — обёртка с интерцепторами, отменами, преобразованием JSON, удобной обработкой ошибок.

---

### 135. React Query (TanStack Query) — зачем?

Управление состоянием сервера: кэширование, повторные запросы, инвалидация, оптимистичные обновления. Упрощает работу с асинхронными данными.

---

### 136. Redux — когда нужен?

Когда нужен глобальный стейт и предсказуемое обновление через actions и reducers. Для простых приложений часто хватает Context, React Query и локального state.

---

### 137. Что такое CSS Modules?

CSS-файлы, в которых имена классов локализуются (уникальны). Устраняют конфликты имён, комбинация с React/TypeScript удобна.

---

### 138. Что такое CORS на стороне клиента?

Браузер при cross-origin запросе отправляет предзапрос (preflight) OPTIONS. Сервер должен вернуть подходящие CORS-заголовки. Клиент сам CORS не обходит — это ограничение браузера.

---

### 139. Что такое SPA?

Single Page Application. Одна HTML-страница, навигация через JavaScript без перезагрузки. Контент подгружается динамически. React, Vue, Angular — типичные инструменты для SPA.

---

### 140. SEO и SPA — в чём сложность?

Контент генерируется на клиенте, поисковики могут не выполнить JavaScript. Решения: SSR (Next.js, Nuxt), предрендер, prerender.io.

---

### 141. Что такое Lazy Loading компонентов?

Загрузка компонента только при необходимости (например, при переходе на маршрут). `React.lazy` + `Suspense` разбивают бандл и ускоряют начальную загрузку.

---

### 142. Что такое Custom Hook?

Функция, использующая хуки и инкапсулирующая логику. Позволяет переиспользовать состояние и побочные эффекты.

---

### 143. Key в списках React — зачем?

Помогает React сопоставлять элементы при обновлении. Должна быть стабильной и уникальной в рамках списка. Нельзя использовать индекс, если порядок меняется.

---

### 144. Разница между React и Angular?

React — библиотека, гибкость, большое сообщество. Angular — полноценный фреймворк, свои правила, RxJS, DI. Оба используют компонентный подход и TypeScript.

---

### 145. Что такое Strict Mode?

Режим, помогающий находить потенциальные проблемы (устаревшие API, побочные эффекты). В development может вызывать двойной рендер для проверки чистоты компонентов.

---

## БЛОК 7: Entity Framework Core

### 146. Code First vs Database First?

**Code First** — модель в коде, миграции создают/обновляют БД.  
**Database First** — схема уже в БД, scaffolding генерирует классы. Code First чаще в новых проектах.

---

### 147. Что такое DbContext?

Центральный класс EF Core. Управляет подключением, наборами сущностей (DbSet), кэшем, трекингом. Обычно живёт в рамках одного scope (например, запроса).

---

### 148. Eager Loading vs Lazy Loading?

**Eager** — связанные данные загружаются явно через `Include`.  
**Lazy** — загрузка при обращении к навигационному свойству (требует настройки). Eager даёт контроль и позволяет избежать N+1.

---

### 149. Что такое Change Tracking?

EF отслеживает изменения сущностей, загруженных через DbContext. При `SaveChanges` формируются нужные INSERT/UPDATE/DELETE. `AsNoTracking` отключает трекинг для read-only запросов.

---

### 150. AsNoTracking — когда использовать?

Для запросов только на чтение, когда не планируется сохранять изменения. Снижает потребление памяти и ускоряет выполнение.

---

### 151. Миграции — как работают?

Инструмент для version control схемы БД. Генерируются классы миграций с методами `Up`/`Down`. `dotnet ef migrations add`, `Update-Database` применяют изменения.

---

### 152. Fluent API vs Data Annotations?

**Data Annotations** — атрибуты на свойствах (`[Required]`, `[MaxLength]`).  
**Fluent API** — конфигурация в `OnModelCreating`. Fluent API гибче и не засоряет модель.

---

### 153. Что такое Shadow Properties?

Свойства, не объявленные в классе, но существующие в БД. Добавляются через Fluent API. Используются для служебных полей (например, CreatedAt, мягкое удаление).

---

### 154. Soft Delete в EF Core?

Вместо физического DELETE — обновление флага `IsDeleted`. Реализуется через глобальный query filter в DbContext, чтобы скрывать «удалённые» записи по умолчанию.

---

### 155. Разница между Find и запросом по Id?

`Find` сначала ищет в кэше, затем в БД. Подходит для загрузки по ключу. LINQ-запрос всегда идёт в БД (если нет кэша), но позволяет сложные условия.

---

### 156. ExecuteUpdate / ExecuteDelete (EF Core 7+)

Массовое обновление/удаление без загрузки сущностей в память. Генерирует один UPDATE/DELETE на стороне БД. Эффективнее для bulk-операций.

---

### 157. Raw SQL в EF Core?

`FromSqlRaw`, `ExecuteSqlRaw` для выполнения произвольного SQL. Риски: SQL Injection, обход маппинга. Параметризация обязательна.

---

### 158. Раздельные DbContext для чтения и записи (CQRS)?

Отдельные контексты или модели для команд и запросов. Write — полноценный трекинг, миграции. Read — AsNoTracking, проекции, возможно отдельная БД/схема.

---

### 159. Обработка конкурентности (Concurrency)?

Поле `RowVersion` (timestamp) или проверка других полей. При конфликте EF выбрасывает `DbUpdateConcurrencyException`. Нужна стратегия повторной попытки или слияния изменений.

---

### 160. Лучшие практики EF Core?

- Использовать `async` для всех I/O
- `AsNoTracking` для read-only
- Проекции вместо полной загрузки сущностей
- Пагинация для больших выборок
- Избегать N+1 через Include
- Не держать DbContext долго открытым

---

## БЛОК 8: Тестирование

### 161. Unit Test — что это?

Тест одной единицы (метод, класс) в изоляции. Зависимости заменяются mock’ами. Проверяется корректность логики.

---

### 162. Arrange-Act-Assert (AAA)?

Структура теста:  
- **Arrange** — подготовка данных и зависимостей (моки, тестовые объекты).  
- **Act** — вызов тестируемого кода (один метод или сценарий).  
- **Assert** — проверка результата (и при необходимости вызовов моков через Verify).

**Пример unit-теста с AAA (xUnit):**

```csharp
[Fact]
public void CreateUser_ValidData_ReturnsSuccess()
{
    // Arrange
    var service = new UserService(_userRepositoryMock.Object);
    var email = "test@example.com";

    // Act
    var result = service.CreateUser(email);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.Equal(email, result.Email);
}
```

---

### 163. xUnit, NUnit, MSTest — в чём разница?

Разные фреймворки для .NET. xUnit популярен в экосистеме .NET Core, одна инициализация на класс, атрибуты `[Fact]`, `[Theory]`. NUnit и MSTest тоже широко используются.

---

### 164. Moq — как использовать?

Библиотека для создания mock-объектов. **Setup** — настройка поведения (что вернуть при вызове метода); **Verify** — проверка, что метод был вызван нужное число раз с нужными аргументами. Подменяет зависимости в unit-тестах, чтобы тестировать один класс в изоляции.

**Пример мокирования с Moq:**

```csharp
[Fact]
public async Task GetOrderById_ExistingId_ReturnsOrder()
{
    // Arrange
    var order = new Order { Id = 1, CustomerName = "Alice", Total = 100m };
    var repoMock = new Mock<IOrderRepository>();
    repoMock
        .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
        .ReturnsAsync(order);
    var service = new OrderService(repoMock.Object);

    // Act
    var result = await service.GetOrderById(1, CancellationToken.None);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(1, result.Id);
    Assert.Equal("Alice", result.CustomerName);
    repoMock.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
}

[Fact]
public async Task CreateOrder_ValidData_CallsSaveAsync()
{
    var repoMock = new Mock<IOrderRepository>();
    var service = new OrderService(repoMock.Object);

    await service.CreateOrderAsync("Bob", new List<OrderItemDto>(), CancellationToken.None);

    repoMock.Verify(
        r => r.SaveAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()),
        Times.Once);
}
```

Для async-методов: `ReturnsAsync(value)`, для исключений — `ThrowsAsync(new InvalidOperationException())`. `It.IsAny<T>()` — любой аргумент типа T; `It.Is<T>(x => ...)` — проверка по условию.

---

#### 164.1. Unit-тесты и TDD (Must): xUnit, Moq, FluentAssertions, AAA, что тестировать, code coverage

**xUnit / NUnit:** xUnit — по умолчанию в шаблонах .NET Core: `[Fact]` для одного теста, `[Theory]` + `[InlineData]` для параметризованных. Один экземпляр тестового класса на тест (изоляция). NUnit — `[Test]`, `[TestCase]`; общая инициализация через `[SetUp]`. Оба подходят; xUnit часто встречается в .NET-проектах.

**Moq** — моки: `var mock = new Mock<IOrderRepository>(); mock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(order);` — при вызове `GetByIdAsync(1, ...)` вернётся `order`. `mock.Verify(r => r.SaveAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);` — проверить, что метод вызван ровно один раз. Для async: `ReturnsAsync`, `ThrowsAsync`.

**FluentAssertions** — читаемые утверждения: `result.Should().NotBeNull(); result.Name.Should().Be("Expected"); list.Should().HaveCount(3).And.Contain(x => x.Id == 1); exception.Should().Throw<ArgumentException>().WithMessage("*invalid*");` Удобно для коллекций, исключений, вложенных объектов.

**AAA (Arrange / Act / Assert):** Arrange — подготовка данных и моков; Act — вызов тестируемого метода; Assert — проверка результата и при необходимости вызовов моков. Держать тест коротким и по одному сценарию на тест.

**Что тестировать:** бизнес-логику (сервисы, домен, валидаторы), краевые случаи и ошибки. **Что не тестировать unit-тестами:** тонкие контроллеры (достаточно интеграционных проверок маршрута и статуса), тривиальные геттеры/сеттеры, код без логики. Инфраструктуру (БД, HTTP) — через интеграционные тесты или моки.

**Code coverage** — доля кода, выполняемого тестами. Метрика качества, но не цель сама по себе: 100% покрытие не гарантирует отсутствие багов. Важно покрывать критические пути и граничные случаи. В .NET: coverlet + `dotnet test --collect:"XPlat Code Coverage"` или встроенные отчёты IDE.

**TDD (Test-Driven Development):** сначала пишется падающий тест (красный), затем минимальный код для прохождения (зелёный), затем рефакторинг. Помогает дизайну API и покрытию; на собеседовании достаточно знать идею и что «тест сначала».

---

### 165. Integration Test — что это?

Тест взаимодействия нескольких компонентов (API, БД, сервисы). Проверяет реальные сценарии. Используется тестовый контекст, часто in-memory БД или test containers.

---

### 166. Mock vs Stub vs Fake?

**Mock** — проверяет, как объект вызывался.  
**Stub** — предоставляет предопределённые ответы.  
**Fake** — рабочая, но упрощённая реализация (например, in-memory репозиторий).

---

### 167. Test Coverage — что означает?

Доля кода, выполняемого тестами. Метрика качества, но не гарантия. Важно покрывать критические пути и граничные случаи.

---

### 168. Что такое TDD?

Test-Driven Development. Сначала пишется падающий тест, затем код, чтобы тест прошёл, затем рефакторинг. Помогает улучшать дизайн и покрытие.

---

### 169. Как тестировать асинхронный код?

Тесты должны быть async и возвращать `Task`. Использовать `await` для асинхронных вызовов. Moq настраивает `ReturnsAsync` для async-методов.

---

### 170. Как тестировать контроллеры?

Через `WebApplicationFactory` (integration) или создание экземпляра контроллера с подставленными зависимостями (unit). Проверять статус-коды, тело ответа, вызовы сервисов.

---

## БЛОК 9: Git и DevOps

### 171. Как используешь Git в команде?

Ветки под фичи (`feature/`, `bugfix/`), частые коммиты с осмысленными сообщениями, Pull Request и code review, rebase для аккуратной истории. Следуем Git Flow или trunk-based.

---

### 172. Git Flow vs Trunk-Based?

**Git Flow** — ветки `develop`, `release`, `hotfix`, `feature`.  
**Trunk-Based** — одна основная ветка, короткоживущие фичи. Trunk-Based проще, чаще в CI/CD.

---

### 173. merge vs rebase?

**merge** — создаёт merge commit, сохраняет историю веток.  
**rebase** — переносит коммиты на верх другой ветки, история линейная. Rebase перед merge в main для «чистой» истории.

---

### 174. Что такое CI/CD?

**CI** — Continuous Integration: автоматическая сборка и тесты при каждом коммите.  
**CD** — Continuous Deployment/Delivery: автоматический деплой в тест/прод после успешного прохождения пайплайна.

---

#### 174.1. Azure DevOps Pipelines — стадии, environments, approvals. Рассказ про свой CI/CD

**Стадии типичного пайплайна:**

1. **Build** — восстановление зависимостей (`dotnet restore`), сборка (`dotnet build` или `dotnet publish`), артефакт (например, папка publish или Docker-образ). Триггер: push в ветку или PR.
2. **Test** — запуск unit-тестов (`dotnet test`), при необходимости интеграционных. Сборка не продвигается дальше при падении тестов.
3. **Deploy** — развёртывание артефакта в окружение (test, staging, production). Может быть отдельным job или пайплайном, подтягивающим артефакт из build.

**Environments** в Azure DevOps — логические цели деплоя (Dev, Test, Prod). К environment можно привязать approvals and checks: перед деплоем в Prod требуется подтверждение (approval) или проверка. Защита веток (branch policies) дополняет: в main можно мержить только после успешного пайплайна и ревью.

**Преимущества CI/CD:** автоматизированное тестирование при каждом изменении; одинаковые и предсказуемые деплои; быстрый откат (rollback) к предыдущему артефакту; меньше ручных ошибок при выкатке.

**Пример pipeline YAML (Azure DevOps):**

```yaml
trigger:
  - main

pool:
  vmImage: 'ubuntu-latest'

stages:
  - stage: Build
    jobs:
      - job: BuildJob
        steps:
          - task: UseDotNet@2
            inputs:
              packageType: 'sdk'
              version: '8.x'
          - script: dotnet restore
            displayName: 'Restore'
          - script: dotnet build --no-restore -c Release
            displayName: 'Build'
          - script: dotnet publish -c Release -o $(Build.ArtifactStagingDirectory)
            displayName: 'Publish'
          - publish: $(Build.ArtifactStagingDirectory)
            artifact: drop
            displayName: 'Publish artifact'

  - stage: Test
    dependsOn: Build
    jobs:
      - job: TestJob
        steps:
          - download: current
            artifact: drop
          - script: dotnet test --no-build -c Release --logger trx
            displayName: 'Unit tests'

  - stage: Deploy
    dependsOn: Test
    jobs:
      - deployment: DeployTest
        environment: 'Test'
        strategy:
          runOnce:
            deploy:
              steps:
                - download: current
                  artifact: drop
                # шаги деплоя (Azure Web App, VM, Docker и т.д.)
```

**Пример формулировки для собеседования:** «Внедрил CI/CD в Azure DevOps: время деплоя сократилось с ~2 часов до ~15 минут (улучшение порядка 87%). Сборка, тесты и выкат в Test автоматические; в Prod — после ручного approval. Секреты в переменных пайплайна, не в коде.»

**Пример рассказа про свой опыт (подставь свой проект):** «На проекте мы используем Azure DevOps Pipelines. При push в main запускается сборка: restore, build, publish в артефакт. Параллельно прогоняются unit-тесты; при падении сборка помечается как failed. Успешный артефакт деплоится в окружение Test автоматически. В Production деплой идёт после ручного approval: в Azure DevOps настроен environment Prod с проверкой «approval». Секреты и строки подключения хранятся в переменных пайплайна (secret variables), не в коде.»

---

### 175. Docker — зачем в разработке?

Одинаковая среда на всех машинах, изоляция зависимостей, воспроизводимость. Контейнеризация приложения и БД упрощает локальную разработку и деплой.

---

#### 175.1. Docker базово: Dockerfile для ASP.NET Core, docker-compose, контейнеры vs VM

**Контейнеры vs VM:** VM — полная виртуальная машина с гостевой ОС. Контейнер — изоляция процессов на общем ядре хоста; образ содержит приложение и зависимости, без отдельной ОС. Контейнеры легче, быстрее стартуют, меньше ресурсов. Docker — движок для запуска контейнеров по образу.

**Dockerfile для ASP.NET Core (multi-stage):** первый этап — сборка (SDK), второй — runtime (только runtime, без SDK), чтобы образ был меньше.

```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MyApi/MyApi.csproj", "MyApi/"]
RUN dotnet restore "MyApi/MyApi.csproj"
COPY . .
WORKDIR /src/MyApi
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MyApi.dll"]
```

**docker-compose для local dev** — поднять API и БД одной командой:

```yaml
version: '3.8'
services:
  api:
    build: .
    ports:
      - "5000:8080"
    environment:
      - ConnectionStrings__DefaultConnection=Server=db;Database=AppDb;User=sa;Password=YourPassword123;
    depends_on:
      - db
  db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      ACCEPT_EULA: Y
      SA_PASSWORD: YourPassword123
    ports:
      - "1433:1433"
```

Запуск: `docker-compose up --build`. Остановка: `docker-compose down`. На собеседовании часто спрашивают — стоит потратить 1–2 дня на практику, если не знаком.

---

### 176. Что такое Kubernetes?

Оркестратор контейнеров. Управление масштабированием, балансировкой, обновлениями, self-healing. Используется для продакшена микросервисов.

---

### 177. Разница между Azure DevOps и GitHub Actions?

Оба — платформы CI/CD. Azure DevOps — часть экосистемы Microsoft, интеграция с Azure. GitHub Actions — встроен в GitHub, marketplace actions. Выбор часто зависит от хостинга репозитория и облака.

---

### 178. Что такое Blue-Green Deployment?

Два идентичных окружения: Blue (текущее) и Green (новое). Переключение трафика на Green после проверки. Быстрый откат — переключение обратно на Blue.

---

### 179. Что такое Feature Flags?

Механизм включения/выключения функциональности без деплоя. Используется для постепенного раската, A/B тестов, быстрого отключения проблемного кода.

---

### 180. Что такое Logging и Monitoring?

**Logging** — запись событий (Serilog, NLog) в файлы, БД, Elasticsearch.  
**Monitoring** — метрики (Application Insights, Prometheus), алерты, дашборды для отслеживания здоровья системы.

---

## БЛОК 10: Soft Skills

### 181. Опиши сложную техническую проблему, которую решил

Структура STAR: ситуация, твоя задача, действия (анализ, гипотезы, решение), результат. Подчеркни анализ, сотрудничество и измеримый эффект.

---

### 182. Как работаешь в команде?

Открытая коммуникация, участие в code review, готовность делиться знаниями и спрашивать, когда нужно. Учитываю приоритеты и дедлайны команды.

---

### 183. Как управляешь приоритетами?

Разделяю задачи по критичности и срочности. Обсуждаю конфликты с тимлидом. Стараюсь сначала закрывать блокеры для других. Гибко реагирую на смену приоритетов.

---

### 184. Как учишься новым технологиям?

Документация, курсы, pet-проекты, чтение чужого кода. Пробую применить новое в рабочих или учебных задачах.

---

### 185. Как даёшь и принимаешь обратную связь?

Даю конструктивно, с примерами. Принимаю без защиты, уточняю, как улучшить результат. Считаю feedback инструментом роста.

---

### 186. Как справляешься с дедлайнами?

Оцениваю задачи, выделяю приоритеты, предупреждаю о рисках. При перегрузке — сразу коммуникация с менеджером/тимлидом.

---

### 187. Как подходишь к Code Review?

Проверяю логику, читаемость, соответствие стандартам, граничные случаи, тесты. Комментарии — конкретные и дружелюбные, предлагаю альтернативы, а не только критику.

---

### 188. Как объясняешь технические вещи нетехническим людям?

Аналогии из жизни, без жаргона. Фокус на результате и ограничениях, а не на деталях реализации.

---

### 189. Как остаёшься в курсе индустрии?

Подкасты, блоги, конференции, Twitter/X, GitHub, хабра/Medium. Выбираю 2–3 направления для более глубокого изучения.

---

### 190. Опиши ситуацию, когда не согласился с решением команды

Опиши, как выразил несогласие аргументированно, обсудил альтернативы, но в итоге поддержал общее решение. Покажи баланс между своей позицией и командной дисциплиной.

---

## БЛОК 11: Практические примеры кода

### 191. LINQ: найти активных пользователей с заказами за последние 30 дней

```csharp
var activeUsers = users
    .Where(u => u.IsActive)
    .Where(u => u.Orders.Any(o => o.Date > DateTime.UtcNow.AddDays(-30)))
    .Select(u => new { u.Name, OrderCount = u.Orders.Count() })
    .ToList();
```

---

### 192. SQL: топ-5 самых продаваемых товаров

```sql
SELECT TOP 5 p.ProductName, SUM(oi.Quantity) AS TotalSold
FROM Products p
JOIN OrderItems oi ON p.ProductId = oi.ProductId
GROUP BY p.ProductName
ORDER BY TotalSold DESC;
```

---

### 193. Controller: создание пользователя (POST)

```csharp
[HttpPost]
public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    var user = new User { Name = dto.Name, Email = dto.Email };
    await _userService.CreateAsync(user);

    return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
}
```

---

### 194. Unit Test: xUnit + Moq

```csharp
[Fact]
public async Task GetUser_ReturnsUser_WhenExists()
{
    // Arrange
    var mockRepo = new Mock<IUserRepository>();
    mockRepo.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new User { Id = 1, Name = "Test" });
    var service = new UserService(mockRepo.Object);

    // Act
    var result = await service.GetUserAsync(1);

    // Assert
    Assert.NotNull(result);
    Assert.Equal("Test", result.Name);
}
```

---

### 195. Транзакция в SQL

```sql
BEGIN TRANSACTION;
UPDATE Accounts SET Balance = Balance - 100 WHERE Id = 1;
UPDATE Accounts SET Balance = Balance + 100 WHERE Id = 2;
COMMIT;
```

---

### 196. Async метод с обработкой ошибок

```csharp
public async Task<Result<User>> GetUserSafeAsync(int id)
{
    try
    {
        var user = await _repository.GetByIdAsync(id);
        return user is null ? Result<User>.NotFound() : Result<User>.Ok(user);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error getting user {Id}", id);
        return Result<User>.Failure("Failed to load user");
    }
}
```

---

### 197. React: fetch данных с useEffect

```tsx
useEffect(() => {
  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await fetch('/api/users');
      const data = await res.json();
      setUsers(data);
    } catch (err) {
      setError(err);
    } finally {
      setLoading(false);
    }
  };
  fetchData();
}, []);
```

---

### 198. TypeScript: интерфейс и типизированный fetch

```typescript
interface User {
  id: number;
  name: string;
  email: string;
}

async function getUser(id: number): Promise<User> {
  const res = await fetch(`/api/users/${id}`);
  return res.json();
}
```

---

### 199. Пагинация в LINQ

```csharp
var page = users
    .OrderBy(u => u.Name)
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToList();
```

---

### 200. Middleware: глобальная обработка исключений

```csharp
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var ex = context.Features.Get<IExceptionHandlerFeature>();
        await context.Response.WriteAsJsonAsync(new
        {
            error = "An error occurred",
            message = ex?.Error.Message
        });
    });
});
```

---

### 201. Circuit Breaker — что это?

Паттерн для устойчивости: при многократных ошибках вызова внешнего сервиса «размыкается цепь» и запросы не отправляются. Через время — попытка снова. Реализуется в Polly, например.

---

### 202. Retry Pattern — когда применять?

При временных сбоях (сеть, таймауты). Настраиваются количество попыток, задержка, экспоненциальный backoff. Polly даёт готовые политики Retry и Retry with backoff.

---

## БЛОК 12: C# CORE — уровень профи

> **Цель блока:** Уровень Middle — не теория, а практика: расширяемый код, без God Object, SOLID в действии, глубокое понимание памяти и типов.

---

### 203. Память в C#: Stack и Heap — как это устроено детально?

**Stack (стек)** — область памяти для локальных переменных и вызовов методов. Хранит value types и ссылки (указатели) на объекты в heap. Работает по LIFO. Быстрый доступ, ограниченный размер. При выходе из метода кадр стека освобождается.

**Heap (куча)** — область для reference types (объекты классов, массивы). Размер динамический. Объекты живут, пока на них есть ссылки. Управляется Garbage Collector.

**Практика:** Локальная переменная `int x` — значение в стеке. `var obj = new MyClass()` — ссылка в стеке, сам объект в куче. Ниже — погружение на уровень Senior: как всё работает под капотом.

---

#### Память под капотом — уровень Senior

**203-S1. Стек вызовов (call stack) и stack frame — как устроено**

У каждого потока свой **стек**. При вызове метода CLR выделяет **stack frame** (кадр): в нём хранятся локальные переменные, параметры, адрес возврата (куда прыгнуть после return), сохранённые регистры. Указатель стека (SP) двигается вниз при вызове и вверх при возврате. Размер стека по умолчанию ограничен (обычно 1 МБ для потока в .NET; можно задать при создании потока). **StackOverflowException** возникает, когда кадров слишком много (глубокая рекурсия без выхода) или один кадр слишком большой (например, огромный локальный массив через stackalloc).

**Зачем это знать:** Понимаешь, почему рекурсия без базы съедает стек и почему большие структуры в локальных переменных копируются в каждый кадр.

---

**203-S2. Где именно хранятся value types и reference types**

- **Value type в локальной переменной или параметре** — лежит **в стеке** (в кадре метода). Исключение: value type как **поле класса** — тогда он внутри объекта в **куче**.
- **Reference type** — **в куче** всегда (объект). В стеке только **ссылка** (указатель, 4/8 байт).
- **Boxing:** value type упаковывается в объект в куче — в стеке ссылка на этот объект. Unboxing — из объекта в куче значение копируется обратно (часто в стек или в другой объект).

Итого: «value type не всегда в стеке» — только когда он локальная переменная или параметр; внутри class он в heap.

---

**203-S3. Структура объекта в куче (memory layout)**

Каждый объект в managed heap имеет заголовок:
- **Sync block index** (для lock, GetHashCode, мониторинга) — скрытое поле.
- **Method table pointer** (указатель на тип) — по нему CLR знает тип объекта и виртуальную таблицу методов.

Дальше идут поля объекта в порядке объявления (с учётом выравнивания). Размер объекта = заголовок + размер всех полей (+ padding). Узнать приблизительно: `sizeof` для структур; для классов — через сырые средства (например, Unsafe, или профилировщики памяти).

**Зачем:** Понимание layout помогает при interop, отладке дампов, объяснении разницы размеров struct vs class.

---

**203-S4. Как куча выделяет память (managed heap)**

Управляемая куча разбита на **сегменты** (segments). Внутри сегмента есть **next object pointer** — следующее свободное место. При `new` объект размещается по этому указателю, указатель сдвигается на размер объекта. Выделение — очень быстрая операция (почти как аллокация в стеке), пока есть место. Когда сегмент заполняется или нужен новый — вызывается GC или запрашивается новый сегмент у ОС. Поэтому **частые мелкие аллокации быстрые**, но **частая сборка мусора** может давать паузы.

---

**203-S5. Корни (roots) — откуда GC считает объекты «живыми»**

**Корень** — любая ссылка, по которой можно дойти до объекта без участия других объектов. Типичные корни:
- Локальные переменные и параметры в активных методах (в стеке потока).
- Статические поля.
- Поля объектов, на которые уже есть ссылка из корня (обход графа от корней).
- Ref-переменные (ref, out).
- Ссылки в финализируемых объектах (f-reachable queue, см. ниже).

GC **не** собирает объект, если от какого-либо корня по цепочке ссылок до него можно дойти. Всё остальное — мусор.

---

**203-S6. Алгоритм GC: Mark, Sweep, Compact (кратко)**

1. **Mark (маркировка):** От всех корней обход в глубину/ширину, все достижимые объекты помечаются как живые.
2. **Sweep (очистка):** Мёртвые объекты считаются свободной памятью.
3. **Compact (уплотнение):** Живые объекты сдвигаются к началу сегмента, устраняется фрагментация, обновляются все ссылки на перемещённые объекты. **LOH по умолчанию не уплотняется** (из соображений производительности и размера).

После compact «next object pointer» указывает на первое свободное место — аллокации снова быстрые.

---

**203-S7. Поколения (Gen 0, 1, 2) — зачем и как**

- **Gen 0** — недавно созданные объекты. Большинство объектов умирают молодыми, поэтому Gen 0 маленький и собирается часто. Сборка быстрая.
- **Gen 1** — буфер между 0 и 2: объекты, пережившие одну сборку Gen 0. Собирается реже.
- **Gen 2** — долгоживущие объекты. Полная сборка Gen 2 дорогая; происходит реже.

При сборке Gen 0 выжившие объекты продвигаются в Gen 1; при сборке Gen 1 выжившие — в Gen 2. **Гипотеза:** большинство объектов краткоживущие, поэтому оптимизация под маленькую Gen 0 даёт выигрыш.

**Large Object Heap (LOH):** объекты **≥ 85 000 байт** (для примитивных типов; для ссылочных типов считается размер без вложенных объектов). Размещаются в отдельной куче LOH. LOH собирается только при сборке Gen 2. LOH **не уплотняется** (по умолчанию) — возможна **фрагментация**: много свободных «дыр», но нет одного непрерывного блока под большой объект.

---

**203-S8. Очередь финализации (finalization queue и f-reachable)**

Если у объекта есть финализатор (деструктор `~Class()`), он при создании регистрируется в **finalization queue**. Когда GC помечает объект как мёртвый, он не удаляется сразу: ссылка на него попадает в **f-reachable queue** (очередь «достижимых для финализации»). Отдельный поток вызывает финализаторы; после этого объект снова мёртв и будет собран при следующей сборке. Поэтому объекты с финализатором переживают минимум две сборки и создают нагрузку на GC. **Правило:** не полагаться на финализатор для освобождения ресурсов; использовать IDisposable и using.

---

**203-S9. Pinning (закрепление) и GCHandle**

Если managed-объект нужно передать в нативный код, который держит указатель дольше одного вызова, объект нельзя перемещать при compact. **Закрепление (pinning)** — объект помечается как неперемещаемый; GC не сдвигает его при compact. Закреплённые объекты усиливают **фрагментацию кучи**. `GCHandle.Alloc(obj, GCHandleType.Pinned)` возвращает handle; нужно вызывать `Free()`, иначе утечка и фрагментация. Используется в P/Invoke, когда нативная сторона хранит указатель.

---

**203-S10. Weak Reference (слабая ссылка)**

**WeakReference** — ссылка, которая не «удерживает» объект живым для GC. Если на объект ссылаются только слабые ссылки, GC может его собрать. После сборки `WeakReference.Target` станет null. Используется для кэшей: держать тяжёлые объекты, но позволять GC освободить их при нехватке памяти. **WeakReference&lt;T&gt;** — типизированный вариант.

---

**203-S11. Boxing и unboxing — что происходит в памяти**

**Boxing:** value type копируется в объект в куче; в стеке (или в поле) остаётся ссылка на этот объект. Аллокация + копия. **Unboxing:** из объекта в куче значение копируется в value type (в стек или в другое место). Не «превращение» ссылки в значение, а копирование данных. Повторный boxing создаёт **новый** объект каждый раз — например, в цикле `list.Add(42)` для `List&lt;object&gt;` создаётся много боксов.

---

**203-S12. Span&lt;T&gt;, stackalloc, ref struct — память без heap**

- **stackalloc** — массив выделяется **в стеке** текущего кадра (не в куче). Живёт до выхода из метода. Очень быстро, без нагрузки на GC. Ограничен размером стека.
- **Span&lt;T&gt;** — представление непрерывного участка памяти (массив, stackalloc, или unmanaged). Не владеет памятью, только указывает. **ref struct** — не может быть в heap (не поле класса, не в массиве), только в стеке. Гарантирует, что Span не переживёт область, где память ещё валидна.
- **ref struct** — структура, которая может существовать только на стеке (или в другом ref struct). Компилятор запрещает боксинг, поле в обычном классе, async state machine и т.д. Нужна для Span, ReadOnlySpan и безопасной работы с буферами без копирования.

**Зачем:** Нулевые аллокации в горячих путях (парсинг, протоколы), меньше давление на GC.

---

**203-S13. ArrayPool&lt;T&gt; и переиспользование буферов**

Вместо `new byte[1024]` каждый раз — **аренда** буфера из пула: `ArrayPool&lt;byte&gt;.Shared.Rent(minLength)`. После использования — `Return(array, clearArray: false)`. Буфер возвращается в пул и переиспользуется. Меньше аллокаций и сборок мусора при частых операциях с массивами (I/O, сериализация). **Правило:** всегда Return в finally, и не использовать массив после Return.

---

**203-S14. GC.Collect() — когда вызывать и почему обычно не надо**

Принудительная сборка прерывает работу приложения (в зависимости от режима GC). Обычно **не вызывать**: GC сам подстраивается под аллокации. Исключения: краткие окна, где пауза допустима (например, между уровнями игры); тесты; особые сценарии с нехваткой памяти. **GC.WaitForPendingFinalizers()** после Collect ждёт завершения финализаторов; иногда затем вызывают Collect ещё раз, чтобы собрать объекты, которые стали мёртвыми после финализации.

---

**203-S15. Конкурентная и фоновая сборка (Workstation vs Server GC)**

**Workstation GC** — по умолчанию для клиентских приложений. Может быть **concurrent** (Gen 2 собирается в фоне, не блокируя надолго). **Server GC** — для серверов: несколько куч (по одной на логический процессор), отдельные потоки GC, приоритет на пропускную способность. Настраивается в csproj/runtime. **Background GC** — Gen 2 собирается в фоновом потоке, пока приложение продолжает работать; блокировки короче.

---

**203-S16. Дампы памяти и базовый анализ**

При утечках или высоком потреблении памяти делают **dump процесса** (dotnet-dump, Procdump, Visual Studio). В дампе смотрят: какие типы занимают больше всего; кто держит ссылки на большие объекты (путь от корня). Понимание корней, поколений и того, что «живо» только достижимое из корней, нужно для интерпретации графов объектов в анализаторах.

---

### 204. Что такое static, когда использовать? И зачем static method / static class?

**static** — член принадлежит типу, а не экземпляру. Один экземпляр на домен приложения.

**Когда использовать:**
- Вспомогательные методы без состояния (например, `Math.Sqrt`, утилиты).
- Общие настройки или кэш (осторожно с потокобезопасностью).
- Статический конструктор — инициализация типа один раз.

**Static class** — только статические члены, нельзя создавать экземпляры. Для утилитных классов (например, константы, хелперы).

**Когда не использовать:** Когда нужны тестируемость и подмена зависимостей — статика усложняет моки.

---

### 205. var, явный тип и class — когда что применять?

**var** — вывод типа при компиляции. Удобен для локальных переменных, когда тип очевиден справа (`var list = new List<int>()`). Не скрывает тип, IntelliSense работает.

**Явный тип** — когда тип неочевиден, в публичных API (возвращаемый тип метода), для читаемости.

**class** — описание ссылочного типа (объекта). Определяет поля, свойства, методы. Экземпляр создаётся через `new`, живёт в heap.

---

### 206. Abstract class — когда использовать и в чём разница с обычным классом и интерфейсом?

**Abstract class** — класс, от которого нельзя создать экземпляр. Может содержать абстрактные члены (без реализации) и уже реализованные. Подходит для общей базы иерархии с общей логикой.

**Отличие от обычного class:** Обычный класс можно инстанцировать; абстрактный — только через наследников.

**Отличие от interface:** Интерфейс — только контракт (сигнатуры). Абстрактный класс — контракт + реализованная логика + конструкторы, поля. В C# одно наследование класса, много реализаций интерфейсов.

**Когда использовать abstract class:** Когда есть общая реализация для всех наследников и не нужна множественная «наследовательная» иерархия.

---

### 207. Interface vs Abstract class — краткая шпаргалка

| | **Interface** | **Abstract class** |
|---|---|---|
| Реализация | Только сигнатуры (до C# 8 — default impl опционально) | Может быть частичная и полная |
| Наследование | Много интерфейсов | Один класс |
| Конструктор | Нет | Есть |
| Поля | Нет (кроме static) | Есть |
| Когда | Контракт, полиморфизм, слабая связь | Общая база с общей логикой |

---

### 208. Почему лучше внедрять интерфейс, а не конкретный класс? ⚠️

- **Тестируемость:** Легко подменить реализацию mock’ом в тестах.
- **Расширяемость:** Можно подставить другую реализацию без изменения зависимого кода (Open/Closed).
- **Слабая связь (DIP):** Зависимость от абстракции, а не от деталей. Меняется реализация — клиентский код стабилен.
- **Контракт:** Интерфейс явно описывает, что нужно классу, а не «какой именно класс».

Middle проектирует расширяемый код и избегает God Object — для этого и нужны интерфейсы.

---

### 209. Composition > Inheritance — что это значит на практике?

Наследование жёстко связывает класс с одним предком. Композиция — «включать» поведение через поля/зависимости (другие классы или интерфейсы). Легче менять поведение, тестировать, избегать раздутых иерархий.

**Пример:** Вместо `class Dog : Animal` с переопределением всего — класс `Dog` с полем `IBehavior behavior` и делегированием. Поведение можно менять без новых подклассов.

---

### 210. SRP — почему сервис не должен делать всё?

**Single Responsibility:** Один класс — одна причина для изменения. Если сервис и шлёт письма, и пишет в БД, и считает налоги — при изменении правил налогов меняется один модуль, но задевается и почта, и БД. Тестировать и переиспользовать такой сервис сложно. Middle дробит на отдельные сервисы (EmailService, TaxCalculator, Repository).

---

### 211. OCP — как добавлять функционал без переписывания?

**Open/Closed:** Открыто для расширения (новые классы, стратегии), закрыто для модификации (не лезем в уже работающий код). Через интерфейсы, стратегии, плагины — новая логика = новый класс, а не правки в старом.

---

### 212. DIP — зачем DI вообще существует?

**Dependency Inversion:** Зависимости должны быть от абстракций (интерфейсов), а не от конкретных классов. DI-контейнер даёт конкретную реализацию в рантайме. Так достигаются тестируемость, замена реализаций и слабая связь. «Зависимость от абстракции» = DIP в действии.

---

### 213. Зачем нужны ограничения в Generics (where T : …)? ⚠️

Чтобы компилятор знал, что с `T` можно делать. Без ограничений для `T` доступны только операции типа `object`. С `where T : class` — T может быть null, с `where T : IRepository` — у T есть методы интерфейса, с `where T : new()` — можно писать `new T()`. Ограничения дают типобезопасность и понятный API.

---

### 214. IEnumerable&lt;T&gt; vs List&lt;T&gt; — когда что?

**IEnumerable&lt;T&gt;** — абстракция «последовательность», отложенное выполнение. Не хранит всё в памяти, подходит для стриминга и LINQ-цепочек.

**List&lt;T&gt;** — конкретная коллекция с индексатором, Count, Add. Когда нужен доступ по индексу или многократный проход без пересчёта — List. В API лучше возвращать `IEnumerable<T>` или `IReadOnlyList<T>`, если не хотите раскрывать мутабельность.

---

### 215. async/await pipeline — что происходит под капотом?

Метод помечается `async`, внутри используется `await`. При `await` выполнение приостанавливается, поток не блокируется (может обслуживать другие запросы). По завершении задачи выполнение продолжается (часто на другом потоке из pool). Состояние сохраняется в state machine, генерируемом компилятором.

---

### 216. CancellationToken — зачем и как использовать?

Передача сигнала отмены в асинхронные операции (отмена запроса пользователем, таймаут, завершение приложения). Передаётся в async-методы; они периодически проверяют `token.ThrowIfCancellationRequested()` или передают токен в API (например, EF, HttpClient). В ASP.NET Core токен запроса автоматически отменяется при отмене запроса.

---

### 217. Почему IQueryable может быть опасным? ⚠️

- **Множественное выполнение:** Каждое перечисление (ToList, Count, foreach) может выполнить запрос к БД. Если сначала Count(), потом ToList() — два запроса.
- **Закрытие контекста:** После `using` по DbContext IQueryable уже не может выполниться — «закрытый контекст».
- **Сложность запроса:** Длинные цепочки могут породить неочевидный или тяжёлый SQL. Нужно смотреть сгенерированный SQL и план выполнения.

Понимание deferred execution и момента материализации (ToList, ToArray, foreach) обязательно.

---

### 218. Expression Trees — зачем в LINQ?

Выражение (лямбда) может компилироваться не в делегат, а в дерево выражений (Expression&lt;T&gt;). EF Core и другие провайдеры разбирают это дерево и строят SQL (или другой запрос). Поэтому `Where(x => x.Age > 18)` на IQueryable превращается в `WHERE Age > 18` в БД, а не выполняется в памяти.

---

### 219. LINQ performance traps — какие бывают?

- N+1 запросов (подгрузка связей в цикле).
- Материализация большого результата в память (ToList() на миллионах строк).
- Многократное выполнение одного IQueryable.
- Сложные группировки и джойны в памяти вместо БД (когда уже вызван ToList() и дальше LINQ to Objects).

---

### 220. Паттерны: Repository, Unit of Work, Factory — когда что применять?

**Repository** — абстракция доступа к данным (GetById, Add, Delete). Скрывает EF или SQL. Нужен для тестов и смены источника данных.

**Unit of Work** — группа репозиториев в одной транзакции; один Commit для всей операции. Когда несколько сущностей нужно сохранять атомарно.

**Factory** — создание сложных объектов без прямого `new` в бизнес-логике. Когда конструкция объекта нетривиальная или нужно выбирать реализацию по условию.

---

### 221. Паттерн Strategy — зачем?

Выбор алгоритма во время выполнения. Вместо switch/if по типу — набор классов, реализующих общий интерфейс, и подстановка нужной реализации. Соответствует OCP: новые стратегии добавляются без изменения клиентского кода.

---

### 222. Паттерн Observer / Events — когда использовать?

Когда один объект должен уведомлять многих подписчиков об изменениях (события, доменные события). В C# — события (event + delegate). В микросервисах — шины сообщений (RabbitMQ, Kafka) как масштабируемый вариант observer.

---

## БЛОК 13: ASP.NET Core — сердце backend

### 223. Порядок Middleware — почему он важен?

Middleware выполняются в порядке регистрации. Каждый решает: вызвать `next()` или завершить конвейер. Неправильный порядок: например, Authorization до Authentication — авторизация не сможет прочитать идентичность. Типичный порядок: Exception Handling → HTTPS → Routing → CORS → Authentication → Authorization → Endpoints.

---

### 224. UseAuthentication vs UseAuthorization — в чём разница?

**UseAuthentication** — определяет «кто ты» (проверка токена/cookie, заполнение `User` и `Claims`).

**UseAuthorization** — проверяет «имеешь ли право» на действие (политики, роли). Должен идти после Authentication, иначе нечего авторизовывать.

---

### 225. Как написать свой Middleware?

Класс с методом `InvokeAsync(HttpContext context, RequestDelegate next)`. Регистрация: `app.UseMiddleware<MyMiddleware>()` или extension с `UseMyMiddleware()`. В конструкторе — инжект зависимостей. Перед вызовом `next` — логика до запроса, после — после.

---

### 226. Scoped / Singleton / Transient — когда что регистрировать?

- **Singleton** — один на приложение. Для stateless сервисов, кэша, конфигурации.
- **Scoped** — один на scope (в ASP.NET — на запрос). DbContext, Unit of Work, сервисы, привязанные к запросу.
- **Transient** — новый экземпляр при каждом запросе из контейнера. Лёгкие, без состояния.

---

### 227. Почему нельзя DbContext как Singleton? ⚠️

DbContext не потокобезопасен и хранит состояние (кэш, трекинг). В Singleton один экземпляр обслуживает все запросы → гонки и порча данных. Плюс контекст должен жить не дольше одного запроса (правило «один контекст на единицу работы»). Всегда **Scoped**.

---

### 228. Captive dependency problem — что это?

Когда сервис с коротким временем жизни (Transient/Scoped) внедрён в сервис с длинным (Singleton). Singleton создаётся один раз и «держит» зависимость навсегда — она ведёт себя как Singleton и может хранить устаревшее состояние (например, старый DbContext). Решение: не инжектить Scoped/Transient в Singleton; при необходимости использовать IServiceScopeFactory и создавать scope вручную.

---

### 229. Тонкие контроллеры — что это значит?

Контроллер только принимает HTTP, валидирует вход, вызывает сервис и возвращает результат. Вся бизнес-логика — в сервисах (Application/Domain). Контроллер не содержит if/циклов по бизнес-правилам, только оркестрация вызова.

---

### 230. Правильные HTTP статусы для API (кратко)

- **200 OK** — успешное чтение.
- **201 Created** — ресурс создан, в теле или заголовке Location — ссылка.
- **400 Bad Request** — ошибка валидации (например, ModelState).
- **401 Unauthorized** — не аутентифицирован.
- **403 Forbidden** — аутентифицирован, но нет прав.
- **404 Not Found** — ресурс не найден.
- **500 Internal Server Error** — неожиданная ошибка сервера (в продакшене без деталей).

---

### 231. FluentValidation — зачем и как встроить?

Валидация правил в отдельных классах-валидаторах, а не в атрибутах на модели. Удобно для сложных правил и единого слоя валидации. Регистрируется в DI, вызывается в pipeline (filter или middleware) до контроллера. Ошибки маппятся в 400 и единый формат (например, ProblemDetails).

---

### 232. Единый validation layer — что делает Middle?

Один механизм валидации для всего API (FluentValidation + filter/middleware), единый формат ответов об ошибках (RFC 7807 ProblemDetails или свой контракт), повторное использование правил. Контроллеры не дублируют проверки.

---

### 233. Global Error Handling — что обязательно реализовать?

Exception Middleware перехватывает необработанные исключения, логирует (ILogger), возвращает единый формат (ProblemDetails), не пробрасывает стек и внутренние детали клиенту в продакшене. Разные типы исключений можно маппить на разные статус-коды (404 для NotFound, 400 для ValidationException).

---

### 234. ProblemDetails (RFC) — зачем?

Стандартный формат ответа об ошибке (тип, заголовок, статус, detail, extensions). Клиенты и фронтенд могут единообразно парсить ошибки. ASP.NET Core поддерживает через `ProblemDetails` и `ProducesErrorResponseType`.

---

## БЛОК 14: Database + EF Core — выше CRUD

### 235. Когда использовать AsNoTracking? ⚠️

Когда запрос только на чтение и не будет изменений через этот контекст. AsNoTracking не создаёт трекинг — меньше памяти, быстрее. Обязательно для read-only сценариев (отчёты, API GET). Для команд (create/update/delete) нужен трекинг.

---

### 236. Tracking vs NoTracking — кратко

**Tracking** — EF отслеживает изменения сущностей и при SaveChanges генерирует UPDATE/DELETE. Нужен для сценариев изменения.

**NoTracking** — сущности не отслеживаются, только чтение. Меньше накладных расходов и риска «случайно» изменить и сохранить.

---

### 237. Includes и JOIN — как не делать N+1?

Явно подгружать связи через `Include()` / `ThenInclude()` для нужных навигационных свойств, чтобы один запрос с JOIN’ами получил все данные. Или проекции (Select в DTO) без загрузки целых сущностей. Избегать обращения к навигационным свойствам в цикле без предзагрузки.

---

### 238. Lazy loading — подводные камни

Включённый lazy loading при обращении к навигационному свойству выполняет запрос к БД. В цикле — N+1. Плюс контекст должен быть жив — при закрытом контексте будет исключение. Middle предпочитает явный Eager loading (Include) или проекции.

---

### 239. Repository — когда нужен, когда EF уже достаточно?

**Repository нужен:** когда нужна абстракция над источником данных (тесты, смена БД, сложная доменная логика доступа). **EF уже как repository:** DbSet + DbContext дают GetById, Add, SaveChanges. В простых CRUD-приложениях отдельный слой репозитория может быть избыточен. Middle умеет обосновать: «Repository для абстракции и тестируемости; в простом CRUD можно обойтись DbContext».

---

### 240. Понимание Execution Plan — зачем Middle?

Чтобы объяснить, почему запрос медленный: какие индексы используются, есть ли сканы таблиц, тяжёлые сортировки. Умение прочитать план и предложить индекс или переписать запрос отличает Middle от Junior.

---

## БЛОК 15: Архитектура

### 241. Clean Architecture — Dependency Rule

Зависимости направлены внутрь: Domain не зависит ни от чего; Application зависит от Domain; Infrastructure и Web зависят от Application (и Domain). Внешние детали (БД, API, UI) не влияют на ядро. Domain и Application — без ссылок на EF, ASP.NET и т.д.

---

#### 241.1. Clean Architecture — слои и рассказ на 3–5 минут

**Идея:** Ядро приложения (бизнес-правила) не зависит от БД, фреймворков и UI. Внешние слои подключаются через интерфейсы (абстракции), определённые во внутренних слоях.

**Слои (изнутри наружу):**

1. **Domain (ядро)** — сущности, value objects, доменные исключения, интерфейсы репозиториев (если есть). Без ссылок на EF, ASP.NET, файлы, сеть. Только бизнес-правила и язык домена.
2. **Application (Use Cases)** — сценарии использования: сервисы приложения, DTO, интерфейсы портов (например, `IOrderRepository`). Вызывает Domain, оркестрирует репозитории и внешние сервисы через интерфейсы. Без ссылок на EF, HTTP — только на абстракции.
3. **Infrastructure** — реализация портов: репозитории на EF, отправка email, вызов внешних API. Зависит от Application (реализует его интерфейсы) и от конкретных библиотек (EF, SmtpClient).
4. **Presentation (Web API)** — контроллеры, middleware, маппинг HTTP → команды/запросы. Зависит от Application (вызывает handlers или application-сервисы), не от Infrastructure напрямую (инжект через интерфейсы).

**Dependency Rule:** Зависимости направлены только **внутрь**. Domain не ссылается ни на что. Application ссылается только на Domain. Infrastructure и Presentation ссылаются на Application (и Domain). Никогда не тащить EF, HttpClient, конфиг файлы в Domain или Application — только интерфейсы.

**Сравнение с Onion и Hexagonal:**

- **Onion Architecture** — та же идея: ядро в центре, зависимости внутрь. «Слой» Domain в центре, вокруг — Application, затем адаптеры (инфра, UI). По смыслу Clean Architecture и Onion очень близки; Clean — это уточнённая и популяризированная (Uncle Bob) версия с явными слоями и правилами.
- **Hexagonal (Ports and Adapters)** — ядро (домен + application) в центре; снаружи — «адаптеры»: левые (входящие — API, CLI, события) и правые (исходящие — БД, почта, внешние API). «Порты» — интерфейсы, которые ядро объявляет; адаптеры их реализуют. Без «слоёв» Domain/Application как таковых — акцент на портах и адаптерах. Итог: та же изоляция ядра, разная терминология и визуализация.

**Краткий рассказ на собесе (3–5 минут):** «Мы используем Clean Architecture. В центре — Domain: сущности и правила без зависимостей от БД и фреймворков. Дальше — Application: сценарии использования и интерфейсы репозиториев/внешних сервисов. Infrastructure реализует эти интерфейсы — EF, почта, внешние API. Presentation — Web API, только вызывает application-сервисы или MediatR. Все зависимости направлены внутрь: Domain ни от чего не зависит, Application — только от Domain. Так мы можем тестировать домен и сценарии изолированно и менять БД или API без затрагивания ядра.»

---

#### 241.2. Domain-Driven Design (DDD)

**DDD** — подход к моделированию сложной предметной области через язык домена (ubiquitous language), сущности, value objects, агрегаты и ограниченные контексты (bounded context).

- **Entity** — объект с идентичностью (Id), жизнь которого отслеживается (например, `Order`, `User`).
- **Value Object** — объект без идентичности, определяется только значениями полей (Address, Money). Неизменяемый.
- **Aggregate** — кластер сущностей и value objects с одной корневой сущностью (Aggregate Root). Граница консистентности: изменения снаружи только через корень.
- **Bounded Context** — граница модели: внутри один общий язык и согласованная модель; между контекстами — явные контракты (API, события).
- **Repository** — абстракция доступа к агрегатам как к коллекциям: `GetById`, `Add`, `Remove`. Скрывает персистенцию.

**Пример (упрощённо):**

```csharp
// Value Object — нет Id, сравнивается по полям
public record Money(decimal Amount, string Currency);

// Entity — есть Id
public class Order
{
    public Guid Id { get; private set; }
    public Money Total { get; private set; }
    private readonly List<OrderLine> _lines = new();

    public void AddLine(Product product, int quantity)
    {
        _lines.Add(new OrderLine(product.Id, product.Price, quantity));
        Total = new Money(_lines.Sum(l => l.Subtotal.Amount), Total.Currency);
    }
}
```

На собеседовании: DDD уместен при сложной доменной логике; для простого CRUD часто избыточен.

---

#### 241.3. RESTful API — принципы и пример

**REST** — архитектурный стиль: ресурсы идентифицируются URI, операции через HTTP-методы, статус-коды передают результат. Клиент не хранит состояние сервера (stateless).

- **Ресурсы** — существительные в URL: `/api/orders`, `/api/orders/1`.
- **HTTP-методы:** GET (чтение), POST (создание), PUT (полная замена), PATCH (частичное обновление), DELETE (удаление).
- **Коды:** 200 OK, 201 Created, 204 No Content, 400 Bad Request, 401 Unauthorized, 404 Not Found, 500 Internal Server Error.
- **Версионирование:** в URL (`/api/v1/orders`) или заголовке.

**Пример эндпоинтов:**

```csharp
[ApiController]
[Route("api/v1/[controller]")]
public class OrdersController : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OrderDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<OrderDto>> Get(int id, CancellationToken ct) { ... }

    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderRequest dto, CancellationToken ct) { ... }

    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateOrderRequest dto, CancellationToken ct) { ... }

    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct) { ... }
}
```

---

### 242. Domain layer независим — что это значит?

Domain содержит сущности, value objects, доменные исключения и интерфейсы репозиториев (если есть). Без зависимостей от БД, фреймворков, HTTP. Только бизнес-правила и язык домена. Так домен можно тестировать и переносить между проектами.

---

### 243. Когда CQRS будет overengineering? ⚠️

Когда приложение простое CRUD, одна модель чтения/записи, нет разных моделей для чтения и записи и нет требований к масштабированию чтения. CQRS оправдан при разной модели чтения и записи, event sourcing, сложных отчётах. Для маленького API — лишняя сложность.

---

### 244. Command vs Query (CQRS) — как выглядит pipeline?

Command — изменение состояния (CreateOrder). Query — чтение (GetOrderById). В pipeline: валидация → обработчик команды/запроса → сохранение/возврат. MediatR реализует такой pipeline: одна команда/запрос — один handler, плюс общие поведения (validation, logging).

---

#### 244.1. CQRS & MediatR — детально: handlers, pipeline behaviors, read/write

**CQRS (Command Query Responsibility Segregation)** — разделение моделей и путей для чтения (Query) и записи (Command). Не обязательно разные БД: достаточно раздельных DTO, handlers и при необходимости разных моделей (например, read-модели оптимизированы под отчёты).

**MediatR в ASP.NET Core:** библиотека-посредник. Контроллер отправляет команду или запрос через `IMediator.Send(command)` или `Send(query)`; один handler на тип команды/запроса. Плюс **pipeline behaviors** — цепочка обработки до и после handler (валидация, логирование, транзакции).

**Handlers:** один класс — один тип команды или запроса. Реализует `IRequestHandler<TRequest, TResponse>`.

```csharp
public record CreateOrderCommand(string CustomerName, List<OrderItemDto> Items) : IRequest<OrderResult>;
public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderResult>
{
    private readonly IOrderRepository _repo;
    public CreateOrderHandler(IOrderRepository repo) => _repo = repo;
    public async Task<OrderResult> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        var order = new Order { CustomerName = request.CustomerName, ... };
        await _repo.AddAsync(order, ct);
        return new OrderResult(order.Id);
    }
}

public record GetOrderQuery(int Id) : IRequest<OrderDto?>;
public class GetOrderHandler : IRequestHandler<GetOrderQuery, OrderDto?>
{
    private readonly IOrderRepository _repo;
    public async Task<OrderDto?> Handle(GetOrderQuery request, CancellationToken ct) =>
        await _repo.GetByIdAsync(request.Id, ct);
}
```

**Pipeline behaviors:** выполняются до и после handler. Порядок регистрации задаёт порядок обхода (как middleware). Типичные применения: валидация (FluentValidation), логирование, транзакция (открыть транзакцию до handler, commit после).

```csharp
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var context = new ValidationContext<TRequest>(request);
        var failures = _validators.Select(v => v.Validate(context)).SelectMany(r => r.Errors).Where(f => f != null).ToList();
        if (failures.Count != 0) throw new ValidationException(failures);
        return await next();
    }
}
// Registration: builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
```

**Разделение read/write:** для записи — команды, доменная модель, репозитории, транзакции. Для чтения — запросы, DTO/проекции, при необходимости отдельные read-модели (например, денормализованные представления или отдельный read-контекст в EF). Это не обязательно две БД — достаточно раздельных контрактов и handlers.

---

### 245. DTO ≠ Entity — почему Entity не отдавать наружу?

Entity привязана к БД (навигационные свойства, трекинг, внутренние поля). Отдавая её в API, завязываем контракт на структуре БД и раскрываем лишнее. DTO — контракт API: только нужные поля, стабильная версионность. Middle всегда маппит Entity → DTO на границе.

#### 245.1. DTO Pattern — пример кода

**Request DTO** — что приходит от клиента. **Response DTO** — что возвращаем. Маппинг в сервисе или через AutoMapper/Mapster.

```csharp
// Entity (Domain/Infrastructure) — не отдаём наружу
public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<OrderItem> Items { get; set; }  // навигация, трекинг
}

// Response DTO — контракт API
public record OrderDto(int Id, string CustomerName, DateTime CreatedAt, List<OrderItemDto> Items);
public record OrderItemDto(int ProductId, string ProductName, int Quantity, decimal UnitPrice);

// Request DTO — создание заказа
public record CreateOrderRequest(string CustomerName, List<CreateOrderItemRequest> Items);
public record CreateOrderItemRequest(int ProductId, int Quantity);

// В сервисе или handler: Entity → DTO
public static OrderDto ToDto(Order order) =>
    new OrderDto(
        order.Id,
        order.CustomerName,
        order.CreatedAt,
        order.Items.Select(i => new OrderItemDto(i.ProductId, i.Product.Name, i.Quantity, i.UnitPrice)).ToList());
```

---

### 246. Domain Services vs Application Services

**Domain Service** — логика, которая не естественно лежит в одной сущности (например, расчёт скидки по нескольким сущностям). Работает с доменными типами.

**Application Service** — оркестрация: вызывает репозитории, доменные сервисы, транзакции, маппинг в DTO. Не содержит доменных правил, только сценарии использования.

---

## БЛОК 16: Security

### 16.0. JWT & Auth (Must) — структура токена, access/refresh, хранение, OAuth/OIDC, Identity

#### Структура JWT токена

JWT (JSON Web Token) — три части в Base64Url, разделённые точкой: **header.payload.signature**.

- **Header** — тип токена (`alg`, `typ`), например HS256, JWT.
- **Payload** — claims (утверждения): `sub` (subject, id пользователя), `email`, `role`, `exp` (expiration), `iat` (issued at). Не шифруется — только Base64; не класть секреты.
- **Signature** — подпись: `HMACSHA256(base64(header).base64(payload), secret)`. Сервер проверяет подпись секретом; подделка без секрета невозможна.

Клиент отправляет токен в заголовке: `Authorization: Bearer <token>`.

#### JWT Flow (пошагово)

1. **Клиент** отправляет учётные данные (username/password) на эндпоинт входа (например, `POST /api/auth/login`).
2. **Сервер** проверяет учётные данные (например, по БД или Identity), генерирует JWT (access + опционально refresh) и возвращает их в ответе.
3. **Клиент** сохраняет токен (в памяти или в httpOnly cookie для refresh) и при каждом запросе к API передаёт его в заголовке `Authorization: Bearer <token>`.
4. **Сервер** проверяет подпись и claims (в т.ч. `exp`) без обращения к БД; при успехе выполняет запрос, при истечении или невалидном токене возвращает 401.

**Пример формулировки для собеседования:** «Реализовал JWT-аутентификацию в платформе управления лицензиями: access и refresh токены, проверка по ролям (RBAC). Access — короткоживущий, refresh — в httpOnly cookie; при истечении access клиент обновляет пару через refresh.»

#### OAuth 2.0 — Grant Types

- **Authorization Code** — самый безопасный для веб-приложений: пользователь перенаправляется на страницу входа провайдера, после входа провайдер редиректит обратно с кодом; приложение обменивает код на токен на сервере (секрет не в браузере). Для SPA часто **Authorization Code + PKCE** (proof key for code exchange), чтобы обойти необходимость в client secret в браузере.
- **Client Credentials** — machine-to-machine: приложение аутентифицируется само (client_id + client_secret), получает токен для доступа к API от имени приложения, а не пользователя.
- **Refresh Token** — долгоживущий токен для получения новой пары access/refresh без повторного ввода пароля; хранится безопасно (httpOnly cookie), может быть отозван.

#### Access Token и Refresh Token

- **Access Token** — короткоживущий (минуты–часы), для доступа к API. Содержит claims (sub, roles). При истечении — 401; клиент использует refresh для получения новой пары.
- **Refresh Token** — длинноживущий, хранится безопаснее (httpOnly cookie), используется только для эндпоинта «обмен refresh → новая пара access + refresh». Компрометация access ограничена по времени; refresh можно отзывать (blacklist в БД или хранилище).

#### Хранение на клиенте

- **localStorage/sessionStorage** — доступны из JavaScript → уязвимы к XSS (скрипт может украсть токен). Для access токена не рекомендуется.
- **Память (переменная)** — безопасно от XSS на других доменах, но теряется при перезагрузке страницы; нужен refresh в httpOnly cookie для возобновления сессии.
- **httpOnly cookie** — JavaScript не имеет доступа; cookie автоматически отправляется с запросами (same-site). Идеально для refresh token. Для cross-domain API с cookie нужна настройка CORS и credentials.

**Рекомендация:** Access — в памяти или short-lived; Refresh — в httpOnly, secure, SameSite cookie. Не хранить access в localStorage.

#### OpenID Connect (OIDC)

**OpenID Connect** — слой поверх OAuth 2.0: идентификация пользователя. ID Token (JWT) с claims о пользователе; UserInfo endpoint. Используется для «логин через Google/GitHub/Microsoft». В ASP.NET Core: `AddAuthentication().AddJwtBearer()` для своего API; для входа через провайдера — `AddOpenIdConnect()` или готовые схемы (Microsoft, Google).

#### Role-Based Access Control (RBAC)

Доступ к действиям и ресурсам определяется **ролями** пользователя. Роли могут храниться в БД (AspNetUserRoles) и/или в claims JWT. На контроллерах и действиях — `[Authorize(Roles = "Admin")]` или политики на основе ролей.

**Пример:**

```csharp
// Только пользователи в роли Admin
[Authorize(Roles = "Admin")]
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteOrder(int id) { ... }

// Несколько ролей — достаточно одной
[Authorize(Roles = "Admin,Manager")]
[HttpGet("reports")]
public async Task<IActionResult> GetReports() { ... }

// Регистрация политики по роли (Program.cs)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ManageOrders", policy => policy.RequireRole("Admin", "Manager"));
});
// Использование: [Authorize(Policy = "AdminOnly")]
```

#### ASP.NET Core Identity

Готовый фреймворк для аутентификации и управления пользователями: регистрация, логин, роли, хэширование паролей, двухфакторная аутентификация, внешние провайдеры. Хранит пользователей в БД (таблицы `AspNetUsers`, `AspNetRoles` и др.). Для API часто комбинируют Identity (хранение пользователей, пароли) с выдачей JWT (например, после проверки пароля генерируем access/refresh и отдаём клиенту). Для «чистого» API без UI Identity не обязателен — можно свой репозиторий пользователей и JWT.

---

### 247. Почему нельзя хранить JWT в localStorage? ⚠️

Любой скрипт на странице (XSS) может прочитать localStorage и украсть токен. С cookie (httpOnly, secure) JavaScript не имеет доступа — токен не похищается через XSS. Лучше: Access Token в памяти или short-lived + Refresh Token в httpOnly cookie.

---

### 248. Access Token и Refresh Token — зачем два?

Access — короткоживущий, для доступа к API. Refresh — длинноживущий, хранится безопаснее (httpOnly cookie), используется только для получения новой пары токенов. Компрометация access токена ограничена по времени; refresh вращается и может быть отозван.

---

### 249. Claims — что хранить?

Идентификатор пользователя (sub), роли, email, права (claims для policy). Не хранить чувствительные данные и лишнее — только необходимое для авторизации и отображения.

---

### 250. Policy-based security — что это?

Вместо проверки ролей в коде (`if (User.IsInRole("Admin"))`) — объявление политик (например, `RequireRole("Admin")` или кастомные требования). Политики задаются при конфигурации и применяются через `[Authorize(Policy = "AdminOnly")]`. Удобно расширять и тестировать.

---

### 250a. Безопасность API — частые вопросы на собеседовании

- **Как защищаешь API?** — Аутентификация (JWT Bearer или cookie), авторизация (роли/политики), HTTPS, валидация ввода, лимиты запросов (rate limiting), не раскрывать внутренние ошибки в прод (ProblemDetails без стека).
- **SQL Injection?** — Параметризованные запросы (EF Core, `SqlParameter`); никогда не склеивать SQL из строки с пользовательским вводом.
- **XSS?** — На API обычно не храним HTML от клиента без санитизации; если API отдаёт контент в браузер — правильные заголовки (Content-Type, при необходимости CSP) и экранирование на фронте.
- **CORS?** — Настраивать явно разрешённые origins, не `AllowAnyOrigin()` в проде.
- **Секреты?** — Не в коде; переменные окружения, Azure Key Vault, User Secrets в разработке.
- **Как хранишь пароли?** — Хэш с солью (ASP.NET Core Identity использует PBKDF2); никогда в открытом виде.

---

## БЛОК 17: Production Skills

### 251. Structured logging (Serilog) — зачем?

Структурированные поля (не только строка сообщения): свойства, корреляционные id, уровни. Удобно искать в ELK, Application Insights. Формулировка для резюме: «Implemented structured logging with Serilog for observability».

---

### 252. Тестировать бизнес-логику, а не контроллеры — что имеется в виду?

Unit-тесты на сервисах, домене, валидаторах — там, где реальная логика. Контроллеры тонкие; их достаточно покрывать интеграционными тестами (проверка маршрутов и статусов). Так тесты стабильнее и быстрее, а покрытие — там, где баги дороже.

---

### 253. Чем Unit тест отличается от Integration? ⚠️

**Unit** — один класс/метод, зависимости подменены mock’ами, быстрый, изолированный. **Integration** — несколько компонентов вместе (API + БД или in-memory), реальные или тестовые зависимости, проверка сценария целиком. Оба нужны: unit для логики, integration для контрактов и сценариев.

---

### 254. WebApplicationFactory — зачем?

Создаёт тестовый хост приложения для интеграционных тестов без поднятия реального сервера. Можно вызывать HTTP-эндпоинты, подменять сервисы (например, БД на in-memory). Стандартный способ интеграционных тестов в ASP.NET Core.

---

### 255. CI/CD минимум: Build → Test → Deploy

Pipeline (например, GitHub Actions): на push/PR — сборка, запуск unit- и при необходимости integration-тестов, линтеры. На main/release — деплой в тест/прод (артефакт + развёртывание). Middle должен понимать этапы и уметь настроить простой pipeline.

---

### 256. Docker: Dockerfile и docker-compose для API и БД

**Dockerfile** — образ приложения (runtime, публикация, точка входа). **docker-compose** — описание сервисов (api, db), сети, переменные окружения, зависимости. Запуск одной командой: приложение + БД для локальной разработки и тестов. Конкретные примеры для ASP.NET Core и сравнение контейнеров с VM — см. пункт 175.1. На собеседовании это сильный плюс.

---

## БЛОК 18: Чеклист резюме и проекта Middle

### Что показать в резюме (идеально)

Один сильный проект может содержать:

- **Clean Architecture** — слои Domain, Application, Infrastructure, Web.
- **Auth:** JWT + Refresh Token (без хранения access в localStorage).
- **CQRS + MediatR** — команды/запросы и pipeline.
- **FluentValidation** — единый слой валидации.
- **EF Core + Migrations** — работа с БД и версионирование схемы.
- **Global Error Middleware** — единый формат ошибок (например, ProblemDetails).
- **Unit Tests** — xUnit, Moq, FluentAssertions на логике.
- **Docker Compose** — API + БД одной командой.
- **Swagger** — документирование API.

**Формулировки для резюме:**

- *Designed services using SOLID principles and Clean Architecture.*
- *Implemented global exception handling and standardized error responses.*
- *Built modular backend using Clean Architecture and separation of concerns.*
- *Implemented structured logging with Serilog for observability.*

Если в проекте есть этот набор — уровень Middle демонстрируется наглядно.

---

## БЛОК 19: На что обратить внимание на собеседовании

### Практика важнее теории

Интервьюер часто копает не «что такое SOLID», а **«покажи, как ты применил SRP в своём проекте»** или **«почему выбрал Scoped, а не Singleton для этого сервиса»**.

**Что готовить:**
- По каждой ключевой теме — 1–2 примера из реальных задач.
- SRP: «Вынес отправку писем в отдельный EmailService, потому что…»
- DI: «Для DbContext использовал Scoped, потому что контекст не потокобезопасен и привязан к запросу».
- Async: «В API все I/O-операции асинхронные — вызовы к БД и внешним API через await».
- Ошибки: «Добавил global exception middleware, который логирует и возвращает ProblemDetails».

Заранее продумай 2–3 проекта/модуля и вытащи из них конкретные решения под типичные вопросы.

---

### Живое кодирование (Live Coding)

Часто на собеседовании дают:
- Написать LINQ-запрос (группировка, фильтрация, Join через коллекции).
- Спроектировать эндпоинт (сигнатура, статусы, валидация).
- Найти баг в коде (null reference, забытый await, неправильный scope).

**Как готовиться:**
- Блок 11 в этом документе — база. Решай похожие задачи на время (15–20 минут).
- Тренируйся писать код вслух: «сначала отфильтрую, потом сгруппирую, верну топ-5».
- Если не успеваешь — озвучь план и что бы дописал; иногда смотрят на ход мыслей.

---

### Глубина follow-up вопросов

Ты говоришь «использую async/await» → следующий вопрос может быть:
- **«Что будет, если забыть await?»**
- **«Зачем ConfigureAwait(false)?»**

Такие цепочки типичны для тем, помеченных в документе. Стоит заранее копнуть глубже по ним. Ниже (Блок 20) — ответы на частые follow-up и темы уровня Middle+.

---

### Что может не хватить (Middle+ территория)

Для части компаний это уже Middle+ или Senior, но **базово знать полезно**:

| Область | Что могут спросить | Где подтянуть |
|--------|---------------------|----------------|
| **Конкурентность** | Semaphore, SemaphoreSlim, Channel, Interlocked | Блок 20 ниже |
| **Микросервисы** | Outbox (надёжная доставка сообщений), Saga (распределённые транзакции) | Паттерны интеграции |
| **SQL** | Оконные функции (ROW_NUMBER, SUM OVER), рекурсивные CTE, разбор execution plan на практике | Продвинутый SQL |

Имеет смысл хотя бы одним предложением объяснить: «Outbox — это сохранение исходящего сообщения в БД в той же транзакции, чтобы потом гарантированно отправить»; «Saga — последовательность локальных транзакций с компенсирующими действиями при сбое».

---

## БЛОК 20: Follow-up глубина и темы Middle+

> Ответы на типичные углублённые вопросы и краткий обзор тем, которые могут выйти за рамки «классического» Middle.

---

### Что будет, если забыть await?

Метод вернёт **Task или Task&lt;T&gt;** вместо результата. Вызывающий код получит «горящую» задачу, а не данные. Возможные последствия:
- **Исключение внутри задачи** не будет поймано твоим try-catch — оно «всплывёт» при ожидании задачи или при сборке мусора (в .NET при неожиданной задаче может быть UnobservedTaskException).
- **Логика выполняется «в фоне»** — следующий код может выполниться до завершения операции (race condition).
- В ASP.NET запрос может завершиться до того, как асинхронная работа закончится — ответ уйдёт без результата или с пустым телом.

**Вывод:** Всегда await для Task-возвращающих методов в async-методах; в синхронном коде — .GetAwaiter().GetResult() только если осознанно (риск deadlock в UI).

---

### Зачем ConfigureAwait(false)?

**ConfigureAwait(true)** (по умолчанию) — после await продолжение выполняется в том же **контексте синхронизации** (например, UI-поток в WinForms/WPF или контекст в ASP.NET до Core). Это нужно, когда после await обращаешься к UI или к HttpContext.

**ConfigureAwait(false)** — «не захватывать контекст». Продолжение может выполниться на любом потоке из thread pool. Это уменьшает риск deadlock в библиотечном коде (когда библиотека вызывается из UI и делает .Result на Task) и немного снижает накладные расходы.

**В ASP.NET Core** контекста синхронизации по умолчанию нет, поэтому в коде приложения ConfigureAwait(false) менее критичен, но в **переиспользуемых библиотеках** его по-прежнему рекомендуют: библиотека не знает, откуда её вызвали (UI, ASP.NET Core, консоль).

---

### Semaphore / SemaphoreSlim — зачем?

**Семафор** ограничивает число одновременных входов в участок кода. Например: не более 5 параллельных запросов к внешнему API.

- **Semaphore** — для межпроцессной синхронизации.
- **SemaphoreSlim** — легковесный, для внутрипроцессной. В async-коде используют `WaitAsync` с таймаутом/отменой.

```csharp
await _semaphore.WaitAsync(cancellationToken);
try
{
    await CallExternalApi();
}
finally { _semaphore.Release(); }
```

---

### Channel&lt;T&gt; — когда использовать?

**System.Threading.Channels** — очередь producer-consumer для асинхронной работы. Писатель(и) кладут элементы, читатель(и) забирают. Один канал — несколько производителей/потребителей, backpressure (писатель ждёт при переполнении).

Используют для очередей задач между потоками/асинхронными методами, внутренних буферов, ограничения нагрузки. Альтернатива BlockingCollection в async-мире.

---

### Interlocked — что это и когда применять?

Класс для **атомарных** операций над простыми типами без lock: `Increment`, `Decrement`, `Add`, `CompareExchange`, `Exchange`. Нужен для счётчиков, флагов, lock-free структур при многопоточности.

Когда достаточно простой операции (счётчик, флаг) — Interlocked быстрее и проще, чем lock. Сложную логику лучше оборачивать в lock или другие примитивы.

---

### Outbox (Transactional Outbox) — в двух словах

Паттерн для **надёжной доставки сообщений** при переходе к событийной/микросервисной архитектуре. Идея: исходящее сообщение (событие) записывается в таблицу Outbox **в той же транзакции**, что и изменение бизнес-данных. Отдельный процесс/воркер периодически читает Outbox и отправляет сообщения в шину (Kafka, RabbitMQ). Так не теряем события при падении после commit бизнес-транзакции, но до отправки в шину.

---

### Saga — в двух словах

Паттерн для **распределённых сценариев** без одной общей транзакции. Сценарий разбит на шаги в разных сервисах. Каждый шаг — локальная транзакция. При сбое на каком-то шаге выполняются **компенсирующие транзакции** (откат предыдущих шагов). Бывают хореография (события между сервисами) и оркестрация (центральный координатор). Нужно знать, что откат в распределённой системе — через компенсации, а не через 2PC в общем случае.

---

### SQL: оконные функции (OVER) — зачем?

**Оконные функции** считают значение в «окне» строк (например, по партиции и порядку), не схлопывая строки в одну, в отличие от GROUP BY. Примеры: ROW_NUMBER(), RANK(), SUM() OVER (PARTITION BY … ORDER BY …), LAG/LEAD.

Используют для: нумерации строк внутри группы, скользящих сумм, сравнения с предыдущей/следующей строкой без самоджойнов. Вопросы «топ N в каждой категории», «разница с предыдущим периодом» часто решаются через OVER.

---

### Рекурсивные CTE — когда нужны?

CTE может ссылаться на себя — получается рекурсия. В первом запросе — «якорь» (базовые строки), во втором — рекурсивная часть, обращающаяся к результату CTE. Подходит для **иерархий**: дерево сотрудников, категорий, путей в графе (с ограничением глубины).

Пример: «все подчинённые (прямые и косвенные) данного менеджера».

---

### Execution plan на практике — на что смотреть?

После выполнения запроса в SSMS можно включить «Include Actual Execution Plan». В плане смотришь:
- **Table Scan** vs **Index Seek/Scan** — скан таблицы часто плох на больших данных; seek по индексу — хорошо.
- **Cost** — доля стоимости оператора; самые дорогие узлы — кандидаты на оптимизацию.
- **Warnings** (жёлтые значки) — неявные приведения типов, большие оценки строк и т.п.

Middle должен уметь сказать: «здесь полный скан таблицы, добавлю индекс по колонке из WHERE» или «здесь тяжёлая сортировка, посмотрю, можно ли сузить выборку или покрыть индексом».

---

## БЛОК 21: SQL — полный разбор для собеседования

> Системный разбор SQL от основ до продвинутых тем для .NET/Backend собеседований.

---

### 21.1. Типы команд SQL (DDL, DML, DCL, TCL)

- **DDL (Data Definition Language)** — определение структуры: `CREATE`, `ALTER`, `DROP`, `TRUNCATE`. Меняют схему БД.
- **DML (Data Manipulation Language)** — работа с данными: `SELECT`, `INSERT`, `UPDATE`, `DELETE`. То, что пишешь каждый день.
- **DCL (Data Control Language)** — управление доступом: `GRANT`, `REVOKE`.
- **TCL (Transaction Control Language)** — управление транзакциями: `BEGIN TRANSACTION`, `COMMIT`, `ROLLBACK`, `SAVEPOINT`.

**Чем DELETE отличается от TRUNCATE?**  
**DELETE** — DML. Удаляет строки по условию, логирует каждую строку в transaction log, можно откатить в транзакции, срабатывают триггеры.  
**TRUNCATE** — DDL. Удаляет все строки разом, сбрасывает identity, быстрее (логирует только освобождение страниц), триггеры не срабатывают. Нельзя использовать с таблицами, на которые ссылается foreign key.

---

### 21.2. JOIN'ы — примеры и anti-join

```sql
-- INNER JOIN — только совпадения
SELECT e.Name, d.Name AS Department
FROM Employees e
INNER JOIN Departments d ON e.DepartmentId = d.Id;

-- LEFT JOIN — все из левой + совпадения (NULL если нет)
SELECT e.Name, d.Name AS Department
FROM Employees e
LEFT JOIN Departments d ON e.DepartmentId = d.Id;

-- RIGHT JOIN — зеркало LEFT; FULL OUTER — все из обеих; CROSS JOIN — декартово произведение
```

**Записи в одной таблице, но нет в другой (anti-join):**

```sql
SELECT e.Name
FROM Employees e
LEFT JOIN Departments d ON e.DepartmentId = d.Id
WHERE d.Id IS NULL;
```

Эффективнее, чем `NOT IN` с подзапросом (особенно при возможных NULL в подзапросе).

---

### 21.3. Агрегация, GROUP BY, WHERE vs HAVING

```sql
SELECT d.Name, COUNT(*) AS EmployeeCount
FROM Employees e
JOIN Departments d ON e.DepartmentId = d.Id
GROUP BY d.Name
HAVING COUNT(*) > 5
ORDER BY EmployeeCount DESC;
```

**WHERE** фильтрует строки до группировки, **HAVING** — после. Агрегатные функции в `WHERE` использовать нельзя.

**Порядок выполнения запроса:**  
`FROM` → `JOIN` → `WHERE` → `GROUP BY` → `HAVING` → `SELECT` → `DISTINCT` → `ORDER BY` → `TOP`/`OFFSET`.  
Поэтому алиас из `SELECT` нельзя использовать в `WHERE` — `WHERE` выполняется раньше.

---

### 21.4. Подзапросы и CTE (в т.ч. рекурсивные)

```sql
-- Подзапрос: зарплата выше средней
SELECT Name, Salary FROM Employees
WHERE Salary > (SELECT AVG(Salary) FROM Employees);

-- CTE
WITH AvgSalary AS (SELECT AVG(Salary) AS Value FROM Employees)
SELECT e.Name, e.Salary FROM Employees e
CROSS JOIN AvgSalary a WHERE e.Salary > a.Value;
```

CTE в SQL Server не даёт выигрыша по плану (тот же план, что у подзапроса), но улучшает читаемость. **Рекурсивные CTE** — для иерархий (дерево категорий, оргструктура):

```sql
WITH OrgChart AS (
    SELECT Id, Name, ManagerId, 0 AS Level FROM Employees WHERE ManagerId IS NULL
    UNION ALL
    SELECT e.Id, e.Name, e.ManagerId, oc.Level + 1
    FROM Employees e INNER JOIN OrgChart oc ON e.ManagerId = oc.Id
)
SELECT * FROM OrgChart;
```

---

### 21.5. Оконные функции (Window Functions)

Не сворачивают строки, в отличие от GROUP BY. ROW_NUMBER — уникальный номер; RANK — с пропуском при равенстве; DENSE_RANK — без пропуска.

```sql
SELECT Name, Department, Salary,
    ROW_NUMBER() OVER (PARTITION BY Department ORDER BY Salary DESC) AS RowNum,
    RANK()       OVER (PARTITION BY Department ORDER BY Salary DESC) AS Rnk,
    DENSE_RANK() OVER (PARTITION BY Department ORDER BY Salary DESC) AS DenseRnk
FROM Employees;
```

Второй по окладу в каждом отделе:

```sql
WITH Ranked AS (
    SELECT *, DENSE_RANK() OVER (PARTITION BY DepartmentId ORDER BY Salary DESC) AS Rnk
    FROM Employees
)
SELECT * FROM Ranked WHERE Rnk = 2;
```

Другие: `SUM(...) OVER (ORDER BY ...)` — нарастающий итог; `LAG`, `LEAD` — предыдущее/следующее значение.

---

### 21.6. Индексы: Clustered, Non-Clustered, Covering

- **Clustered** — физический порядок данных, один на таблицу, обычно на PK.
- **Non-Clustered** — отдельная B-tree с указателями на строки, можно много.
- **Covering index** — включает все нужные колонки (INCLUDE), чтобы избежать key lookup.

```sql
CREATE NONCLUSTERED INDEX IX_Employee_Dept_Covering
ON Employees(DepartmentId) INCLUDE (Name, Salary);
```

Каждый индекс замедляет INSERT/UPDATE/DELETE. Почему запрос не использует индекс: функции по колонке (`YEAR(CreatedDate)`), неявное приведение типов, низкая селективность, оптимизатор выбрал table scan.

---

### 21.7. Транзакции и уровни изоляции (кратко)

ACID: Atomicity, Consistency, Isolation, Durability.  
Уровни в SQL Server: **READ UNCOMMITTED** (грязное чтение), **READ COMMITTED** (по умолчанию), **REPEATABLE READ**, **SERIALIZABLE**, **SNAPSHOT** (row versioning).

---

### 21.8. Связь с .NET / EF Core

**N+1:** избегать lazy load в цикле; использовать `Include` или проекцию с `Count()`.  
**Raw SQL:** `FromSqlRaw` когда LINQ даёт неоптимальный запрос.  
**Хранимые процедуры** — для сложных отчётов и batch-операций; для CRUD обычно достаточно EF.

---

### 21.9. Классические задачи с собеседований

Найти дубликаты: `GROUP BY Email HAVING COUNT(*) > 1`.  
Удалить дубликаты, оставив одну запись: CTE с `ROW_NUMBER() OVER (PARTITION BY Email ORDER BY Id)`, затем `DELETE FROM CTE WHERE Rn > 1`.  
N-я по величине зарплата (без оконных функций): подзапрос с `COUNT(DISTINCT Salary)`.  
**Pivot** — строки в столбцы: `PIVOT (SUM(Salary) FOR Mon IN ([1]..[12]))`.

---

### 21.10. Execution Plan, Deadlocks, Temp Table vs Table Variable

**Execution Plan:** Table Scan (плохо на больших таблицах), Key Lookup (кандидат на covering index), Sort с высокой стоимостью.  
**Deadlocks:** доступ к таблицам в одном порядке, короткие транзакции.  
**#TempTable** — в tempdb, индексы, статистика, большие объёмы. **@TableVariable** — оптимизатор часто считает 1 строку, подходит для малых наборов.

---

### 21.11. Connection Pool и Connection Leak

**Connection Pool** — пул уже открытых соединений с БД. Вместо создания нового подключения каждый раз (дорого) .NET переиспользует соединения. После `Dispose()` соединение не уничтожается, а возвращается в pool — следующий запрос берёт его оттуда.

**Connection Leak** — утечка соединений: открываются, но не закрываются и не возвращаются в pool. Причина: забыли `using` или `Dispose`. Соединения накапливаются → пул переполняется → **«Timeout expired. The maximum pool size was reached»** → API тормозит, таймауты, возможный crash под нагрузкой. **Как избежать:** всегда `using SqlConnection` или EF Core с правильным scope.

---

### 21.12. String.Format (кратко)

`string.Format("Hello, {0}!", "Alex")` — подстановка значений по плейсхолдерам {0}, {1}. Сигнатура: `Format(string format, params object[] args)`. Форматирование: `{0:C}` — валюта, `{0:F2}` — 2 знака после запятой, `{0:yyyy-MM-dd}` — дата. Современная альтернатива — интерполяция `$"Hello, {name}!"`.

---

## БЛОК 22: RabbitMQ — полный разбор

### 22.1. Что такое RabbitMQ и зачем

RabbitMQ — message broker по протоколу AMQP. Принимает сообщение от producer и доставляет consumer, развязывая сервисы во времени и пространстве. Решает: снижение связанности, сглаживание пиковых нагрузок, асинхронная обработка (HTTP 200 сразу, тяжёлая работа в фоне).

---

### 22.2. Архитектура: Producer → Exchange → Binding → Queue → Consumer

Producer отправляет в **Exchange**, не в очередь. Exchange по типу и **routing key** решает, в какую очередь (или очереди) отправить. **Binding** — правило связи Exchange–Queue. **Queue** — буфер FIFO. **Consumer** подписывается на очередь и обрабатывает сообщения.

---

### 22.3. Типы Exchange

| Тип | Логика | Сценарий |
|-----|--------|----------|
| **direct** | Точное совпадение routing key | Задача конкретному сервису |
| **fanout** | Broadcast во все привязанные очереди | Уведомления «событие произошло» |
| **topic** | * = одно слово, # = ноль или больше | order.*.shipped, log.# |
| **headers** | По заголовкам сообщения | Редко |

---

### 22.4. Гарантии доставки (обязательная тема)

- **Publisher Confirms** — продюсер ждёт подтверждения от брокера (`ConfirmSelect`, `WaitForConfirms`).
- **Durable Queue** — очередь переживает рестарт. **Persistent message** (`delivery mode = 2`) — сообщение пишется на диск.
- **Manual ack** — сообщение удаляется из очереди только после `BasicAck`. При `autoAck: true` при падении consumer сообщение теряется. При ошибке — `BasicNack(requeue: true/false)`.

Цепочка: Producer [confirm] → Exchange → Durable Queue (persistent) → Consumer [manual ack].

---

### 22.5. Prefetch Count (QoS)

Сколько сообщений брокер отдаёт consumer до получения ack. Маленький prefetch — лучше балансировка (round-robin); большой — один consumer может забрать всё.

---

### 22.6. Dead Letter Exchange (DLX)

Куда попадают сообщения при Nack(requeue: false), истечении TTL, переполнении очереди. Настройка через аргументы очереди: `x-dead-letter-exchange`, `x-dead-letter-routing-key`. Используется для retry с задержкой.

---

### 22.7. MassTransit в .NET

В проде часто используют абстракцию поверх RabbitMQ. Регистрация: `AddMassTransit`, `AddConsumer<>`, `UsingRabbitMq`. Публикация: `IPublishEndpoint.Publish(event)`. Обработка: `IConsumer<TMessage>`. MassTransit даёт retry, DLX, сериализацию, Outbox.

---

### 22.8. Идемпотентность

RabbitMQ при правильной настройке даёт **at-least-once** — сообщение может обработаться дважды. Consumer должен быть идемпотентным: проверка по MessageId в таблице обработанных или бизнес-ключу, затем обработка и запись факта.

---

### 22.9. Типичные вопросы

- **RabbitMQ vs Kafka:** RabbitMQ — классическая очередь (push, сообщение удаляется после доставки). Kafka — распределённый лог (pull, сообщения хранятся, replay).
- **Consumer упал:** при manual ack unacked сообщения вернутся в очередь и перераспределятся.
- **Порядок сообщений:** одна очередь + один consumer + prefetch 1; при масштабировании порядок не гарантирован.
- **Outbox Pattern:** запись сообщения в таблицу Outbox в той же транзакции с бизнес-данными; фоновый процесс публикует в RabbitMQ. Консистентность БД и брокера.

---

## БЛОК 23: Apache Kafka — полный гайд

### 23.1. Что такое Kafka

Распределённая платформа потоковой передачи событий (distributed event streaming). По сути — быстрый, отказоустойчивый брокер: **распределённый лог**. Сообщения пишутся в конец лога, имеют **offset**, не удаляются после чтения (retention period).

---

### 23.2. Основные концепции

- **Topic** — именованный канал (append-only).
- **Partition** — лог внутри топика; даёт параллелизм. Сообщения с одним ключом попадают в одну партицию (порядок по ключу).
- **Producer** — отправляет в топик (можно указать key).
- **Consumer / Consumer Group** — группа делит партиции: одна партиция — один consumer в группе. Consumer'ов в группе не больше, чем партиций.
- **Broker** — сервер Kafka. **Replication factor** — копии партиции (leader + followers, ISR).
- **Offset** — порядковый номер в партиции; consumer хранит свой offset (в т.ч. в `__consumer_offsets`).

---

### 23.3. Гарантии доставки

| Гарантия | Описание | Параметр |
|----------|----------|----------|
| At most once | Может потеряться | acks=0 |
| At least once | Может продублироваться | acks=all + retry |
| Exactly once | Без потерь и дублей | Idempotent producer + transactions |

**acks:** 0 — не ждёт; 1 — от leader; all (-1) — от всех ISR.

---

### 23.4. Zookeeper vs KRaft

Раньше Kafka использовала ZooKeeper для метаданных кластера. В новых версиях (Kafka 3.3+) — **KRaft** (встроенный консенсус на Raft) без ZooKeeper.

---

### 23.5. Kafka vs RabbitMQ (кратко)

Kafka — log, pull, хранение, replay, высокая пропускная способность, event streaming. RabbitMQ — queue, push, удаление после подтверждения, task queue, RPC, pub/sub.

---

### 23.6. Паттерны: Event-Driven, Event Sourcing, CQRS, CDC

Event-driven — микросервисы через события. Event Sourcing — состояние как последовательность событий. CQRS — Kafka между write и read моделью. CDC — Kafka Connect стримит изменения из БД в Kafka.

---

### 23.7. Что спросят на собеседовании

- Порядок сообщений — только внутри партиции; ключ гарантирует порядок по сущности.
- Consumer упал — rebalance, партиции перераспределяются; новый consumer продолжает с закоммиченного offset.
- **Idempotent producer** — дедупликация по producer ID + sequence number.
- Количество партиций — от параллелизма и числа consumer'ов.
- **Retention** — удаление по времени/размеру; **compaction** — по ключу хранится последнее значение.
- **ISR** — реплики в синхронизации с leader; **min.insync.replicas** для подтверждения записи.
- **Schema Registry** — хранение схем (Avro/Protobuf) для совместимости версий.

В .NET: Confluent.Kafka (ProducerBuilder, ConsumerBuilder, Commit), а также MassTransit/CAP как абстракции.

---

## БЛОК 24: Entity Framework Core — глобальный разбор ORM

### 24.1. Что такое ORM и зачем EF Core

ORM (Object-Relational Mapping) отображает объекты C# на таблицы БД. EF Core — реализация для .NET: работа с БД через объекты и LINQ, миграции, трекинг, связь с ASP.NET Core (DI, async). Плюсы: меньше ручного SQL, типобезопасность, переносимость между СУБД. Минусы: нужно понимать, какой SQL генерируется, и избегать N+1 и лишних аллокаций.

---

### 24.2. Жизненный цикл DbContext

DbContext не потокобезопасен, должен быть короткоживущим (один запрос — один контекст при Scoped). Создание — при инжекте или `new`; при Dispose освобождаются соединение и ресурсы. **DbContext pooling** (`AddDbContextPool`) переиспользует экземпляры контекста, снижая накладные расходы на создание.

---

### 24.3. Трекинг и материализация

**Change Tracking** — контекст отслеживает сущности, загруженные через него (без AsNoTracking). При `SaveChanges` формируются INSERT/UPDATE/DELETE по изменениям. **Материализация** — момент, когда запрос выполняется (ToList, ToArray, foreach, First и т.д.). До материализации — только построение дерева выражений (IQueryable).

---

### 24.4. Загрузка связей: Eager, Explicit, Lazy

- **Eager:** `Include`/`ThenInclude` — в одном запросе с JOIN. Контролируемо, предсказуемо.
- **Explicit:** `context.Entry(entity).Collection(x => x.Items).Load()` — по требованию.
- **Lazy:** навигационное свойство при обращении выполняет запрос (нужна настройка и прокси). Риск N+1; для API чаще предпочитают Eager или проекции.

---

### 24.5. Миграции и схема БД

Миграции — версионирование схемы. `Add-Migration Name` создаёт класс с `Up`/`Down`. `Update-Database` применяет к БД. В CI/CD миграции часто применяют при деплое. Конфликты решают вручную (merge миграций или откат).

---

### 24.6. Raw SQL, хранимые процедуры, FromSql

Когда LINQ не подходит или нужен полный контроль: `FromSqlRaw`, `ExecuteSqlRaw` с параметрами (никогда не конкатенация строк — SQL injection). Хранимые процедуры — через `FromSqlRaw` или специальные методы. Маппинг результата на сущности или DTO (в т.ч. Keyless entity types).

---

### 24.7. Конкурентность и Concurrency Token

Оптимистичная конкурентность: поле `RowVersion` (timestamp) или проверка старых значений. При конфликте EF выбрасывает `DbUpdateConcurrencyException`. Обработка: перезагрузить сущность, показать пользователю или автоматически перезаписать.

---

### 24.8. Фильтры запросов (Query Filters)

Глобальный фильтр в `OnModelCreating`: `modelBuilder.Entity<Entity>().HasQueryFilter(e => !e.IsDeleted)`. Применяется ко всем запросам по этой сущности (в т.ч. Include). Используется для soft delete и мультитенантности. Можно отключить через `IgnoreQueryFilters()`.

---

### 24.9. Когда EF недостаточно

Тяжёлые batch-операции (тысячи строк) — лучше bulk-библиотеки (EfCore.BulkExtensions, SqlBulkCopy) или ExecuteUpdate/ExecuteDelete (EF Core 7+). Сложные отчёты — иногда проще raw SQL или хранимые процедуры. Высоконагруженные сценарии — внимание к трекингу, AsNoTracking, проекция в DTO без загрузки сущностей.

---

### 24.10. Value Objects и Owned Types (DDD)

**Value Object** в DDD — объект без идентичности, определяемый значениями полей (например, Address, Money). В EF Core маппится через **Owned Entity Types**: `OwnsOne` (один объект) или `OwnsMany` (коллекция). Хранятся в той же таблице (вложенные колонки) или в отдельной с FK.

```csharp
modelBuilder.Entity<Order>().OwnsOne(o => o.ShippingAddress, a =>
{
    a.Property(p => p.Street).HasColumnName("ShippingStreet");
    a.Property(p => p.City).HasColumnName("ShippingCity");
});
```

Используется для DDD-ориентированных проектов: доменная модель с value objects, без лишних таблиц под каждое «вложение».

---

### 24.11. Interceptors и SaveChanges override (аудит, soft delete)

**SaveChanges override** в DbContext — перехват перед вызовом `base.SaveChanges()`. Типичное применение: проставить `CreatedAt`/`UpdatedAt` для всех сущностей с такими полями, выставить `IsDeleted` при «удалении». Обход `ChangeTracker.Entries()`, проверка `EntityState.Added`/`Modified`/`Deleted` и установка полей.

**Interceptors** (реализация `IInterceptor`) — более общий механизм: перехват команд (например, `DbCommandInterceptor` подменяет SQL). Используются для: аудита (логирование запросов), автоматической подстановки tenant ID, soft delete на уровне SQL (подмена DELETE на UPDATE). Регистрация: `AddInterceptors(new MyInterceptor())`.

На собеседовании часто спрашивают: «Как автоматически проставлять дату создания?» — через override `SaveChanges` и проверку `Entry(entity).State == EntityState.Added`.

---

### 24.12. Резюме для собеседования

EF Core — ORM для .NET: DbContext, трекинг, миграции, LINQ to SQL. Понимать: когда материалется запрос, N+1, AsNoTracking, Include, транзакции, конкурентность, Owned Types, Interceptors. Знать границы: когда использовать raw SQL, bulk, хранимые процедуры.

---

## БЛОК 25: HTTP и REST — глубже

### 25.0. REST API Design — Must (сводка для собеседования)

#### HTTP-методы — когда какой

| Метод | Назначение | Идемпотентность | Безопасность (не меняет данные) |
|-------|------------|------------------|----------------------------------|
| **GET** | Получить ресурс(ы) | Да | Да |
| **POST** | Создать ресурс | Нет | Нет |
| **PUT** | Полная замена ресурса по URI | Да | Нет |
| **PATCH** | Частичное обновление | Зависит от реализации | Нет |
| **DELETE** | Удалить ресурс | Да | Нет |

**Практика:** GET для чтения, POST для создания (возврат 201 Created + Location), PUT для «положи сюда это» по известному ID, PATCH для частичных правок, DELETE для удаления.

#### HTTP Status Codes — обязательные для API

| Код | Значение | Когда возвращать |
|-----|----------|-------------------|
| **200 OK** | Успешное чтение (GET) или обновление | GET успешен, PUT/PATCH успешен |
| **201 Created** | Ресурс создан | POST создал сущность; в заголовке Location — URI нового ресурса |
| **204 No Content** | Успех без тела ответа | DELETE успешен, иногда PUT/PATCH |
| **400 Bad Request** | Неверный запрос (синтаксис, формат) | Невалидный JSON, неизвестное поле |
| **401 Unauthorized** | Не аутентифицирован | Нет или невалидный токен |
| **403 Forbidden** | Аутентифицирован, но нет прав | Нет доступа к ресурсу |
| **404 Not Found** | Ресурс не найден | Нет сущности по ID |
| **422 Unprocessable Entity** | Семантическая ошибка (валидация бизнес-правил) | Данные синтаксически верны, но не проходят валидацию (дубликат email, нарушение правил) |
| **500 Internal Server Error** | Ошибка сервера | Необработанное исключение; в проде без деталей клиенту |

**422 vs 400:** 400 — «запрос кривой» (не JSON, не та схема). 422 — запрос корректен, но бизнес-правила не выполнены (например, валидация FluentValidation не прошла). Многие API отдают 400 для обоих; разделение 400/422 улучшает контракт.

#### Версионирование API

- **URL path:** `/api/v1/users`, `/api/v2/users` — понятно, кэшируется.
- **Query:** `/api/users?api-version=1.0` — не меняет путь.
- **Header:** `Accept: application/json; api-version=1.0` или `X-Api-Version`.

В ASP.NET Core: `Microsoft.AspNetCore.Mvc.Versioning` — маппинг версии на контроллеры/действия.

#### Идемпотентность

GET, PUT, DELETE по спецификации идемпотентны (повторный вызов даёт тот же эффект). POST — нет (каждый вызов может создать новую сущность). Для безопасного повтора POST используют **Idempotency-Key** (заголовок): сервер запоминает ключ и результат; при повторном запросе с тем же ключом возвращает сохранённый ответ без повторного выполнения.

#### Пагинация и фильтрация

- **Пагинация:** `?page=1&pageSize=20` (offset) или cursor `?cursor=xyz&limit=20`. Cursor лучше для больших объёмов (см. блок 28.2).
- **Фильтрация:** `?status=Active&from=2024-01-01` — query-параметры. Сортировка: `?sort=createdAt&order=desc`.
- В ответе списка — метаданные: `totalCount`, `page`, `pageSize` или `nextPageToken`.

#### HATEOAS

Ответ содержит ссылки на связанные ресурсы и действия (`links: [{ "rel": "orders", "href": "/users/1/orders" }]`). В большинстве API не реализуют: клиент (SPA, мобильное приложение) знает контракт. Знать термин и что это «гипермедиа-уровень» REST достаточно.

---

### 25.1. Идемпотентность методов

**Идемпотентность** — повторный вызов с теми же данными даёт тот же результат и не меняет состояние сверх первого вызова.

- **GET, PUT, DELETE** — идемпотентны. GET не меняет состояние; PUT «положи сюда это» — результат один и тот же; DELETE «удали это» — после первого удаления ресурса уже нет.
- **POST** — не идемпотентен. Каждый вызов обычно создаёт новую сущность (новый ID). Поэтому повторная отправка формы может создать дубликат.

На собеседовании: «Почему PUT идемпотентен, а POST нет?» — PUT заменяет ресурс по известному идентификатору; POST создаёт новый ресурс, каждый вызов — новая сущность.

---

### 25.2. PATCH: JSON Patch vs JSON Merge Patch

**PATCH** — частичное обновление ресурса. Два распространённых формата:

- **JSON Patch (RFC 6902)** — массив операций: `[{ "op": "replace", "path": "/name", "value": "New" }]`. Точный контроль, можно выразить добавление/удаление элементов массива. В ASP.NET Core — `JsonPatchDocument<T>`.
- **JSON Merge Patch (RFC 7396)** — отправляешь только изменённые поля как фрагмент JSON. Сервер мержит с текущим объектом. Проще, но нельзя явно «обнулить» поле или удалить элемент массива (зависит от интерпретации).

Когда что: Merge Patch — простые сценарии. JSON Patch — когда нужны явные операции (remove, add в массив) и однозначная семантика.

---

### 25.3. HATEOAS (знать что это и почему обычно не делают)

**HATEOAS** (Hypermedia as the Engine of Application State) — ответ API содержит ссылки на связанные действия и ресурсы (например, `links: [{ "rel": "orders", "href": "/users/1/orders" }]`). Клиент «переходит» по ссылкам, а не собирает URL сам.

**Почему обычно не делают:** большинство API проектируют под конкретный клиент (фронтенд или мобильное приложение), который и так знает контракт. HATEOAS усложняет контракт и клиентскую логику; выгода есть в динамических или неизвестных заранее клиентах. Достаточно знать термин и что это «гипермедиа-уровень» REST.

---

### 25.4. Content Negotiation

Клиент запрашивает формат ответа через заголовок **Accept** (например, `application/json`, `application/xml`). Сервер при поддержке нескольких форматов выбирает подходящий и возвращает **Content-Type**. В ASP.NET Core по умолчанию в основном JSON; форматтеры для XML и других типов добавляются в опции. Важно для API, которые обслуживают разных клиентов с разными предпочтениями.

---

## БЛОК 26: Caching

### 26.0. Caching стратегии (Must) — in-memory vs distributed, invalidation, IMemoryCache/IDistributedCache, cache-aside

**In-memory cache** — кэш в памяти одного экземпляра приложения. Плюсы: быстро, без сети. Минусы: при нескольких инстансах (scale-out) кэш не общий — возможна рассинхронизация; теряется при рестарте. **IDistributedCache / Redis** — общий кэш для всех инстансов, переживает рестарт приложения. Плюсы: консистентность между серверами, возможность TTL и структуры данных в Redis. Минусы: сетевая задержка.

**Cache invalidation:** когда данные в источнике (БД) изменились, запись в кэше должна быть обновлена или удалена. Подходы: TTL (время жизни — просто, но возможны устаревшие данные); явная инвалидация при изменении (Remove/Set при update в коде); cache-aside (см. ниже).

**IMemoryCache** (ASP.NET Core) — `AddMemoryCache()`, методы `Get`, `Set`, `GetOrCreateAsync`. Ключ — произвольный объект (часто строка). **IDistributedCache** — абстракция распределённого кэша; реализации: Redis (`AddStackExchangeRedisCache`), SQL Server, NCache. Методы `GetAsync`, `SetAsync` работают с `byte[]`; часто оборачивают в extension с сериализацией (JSON).

**Cache-aside:** приложение само управляет кэшем. Чтение: проверить кэш → при промахе загрузить из БД → записать в кэш. Запись: обновить БД → удалить или обновить запись в кэше. Кэш не знает об источнике — только get/set по ключу. На собеседовании: «Как инвалидируешь?» — при изменении данных явно удаляю ключ или обновляю значение в кэше.

---

### 26.1. IMemoryCache (in-memory)

Кэш в памяти одного экземпляра приложения. Быстрый, без сети. Подходит для данных, общих для всех запросов и не критичных к актуальности. Регистрация: `AddMemoryCache()`. Использование: `GetOrCreateAsync`, `Get`, `Set`, `Remove`. Учитывать: при нескольких инстансах приложения кэш не общий — возможна рассинхронизация.

---

### 26.2. IDistributedCache и Redis

**IDistributedCache** — абстракция распределённого кэша. Реализации: Redis, SQL Server, NCache. Один кэш для нескольких инстансов приложения. Redis — частый выбор: быстрый, поддержка TTL, структуры данных (строки, хеши, списки). Регистрация: `AddStackExchangeRedisCache()`, настройка connection string и опционально instance name.

---

### 26.3. Cache invalidation стратегии

- **TTL (Time To Live)** — запись устаревает через заданное время. Просто, но возможны устаревшие данные до истечения TTL.
- **Explicit invalidation** — при изменении данных вызываем `Remove` или инвалидируем по ключу. Актуальнее, но нужно не забывать инвалидировать везде, где данные меняются.
- **Cache-aside** — при чтении: сначала кэш, при промахе — БД, затем запись в кэш. При записи — обновить/удалить БД и инвалидировать кэш (или обновить значение в кэше).

---

### 26.4. Cache-aside pattern

Клиент (приложение) сам управляет кэшем: проверяет кэш → при промахе идёт в источник (БД, API) → кладёт результат в кэш. При обновлении данных — обновляет источник и удаляет/обновляет запись в кэше. Брокер (Redis) не знает об источнике — только get/set по ключу. На собеседовании: «Как инвалидируешь кэш?» — при изменении данных явно удаляю ключ или обновляю значение.

---

### 26.5. Write-Through и Write-Behind

- **Write-Through:** при записи приложение обновляет и источник (БД), и кэш одновременно. Чтение всегда идёт из кэша (пока не истёк TTL). Консистентность выше, каждая запись даёт запись в БД и в кэш.
- **Write-Behind (Write-Back):** приложение пишет в кэш, а запись в БД откладывается (очередь, фоновая задача). Очень быстрый отклик на запись, но риск потери данных при сбое до сброса в БД; сложнее консистентность.

**Когда обновлять кэш (invalidation):** при любом изменении данных, от которых зависит ключ: после INSERT/UPDATE/DELETE явно вызывать `Remove(cacheKey)` или `Set(cacheKey, newValue)`. Альтернатива — короткий TTL, чтобы устаревшие данные жили недолго.

---

### 26.6. Redis vs Memory Cache — когда что

| | **Memory Cache (IMemoryCache)** | **Redis (IDistributedCache)** |
|---|--------------------------------|------------------------------|
| **Где хранится** | В памяти одного экземпляра приложения | Отдельный сервер/кластер, общий для всех инстансов |
| **Масштабирование** | При нескольких серверах кэш не общий | Один кэш для всех серверов |
| **Скорость** | Очень быстро (локальная память) | Сетевая задержка, но всё ещё быстро |
| **Когда использовать** | Один инстанс; данные локальные, не критично к рассинхрону | Несколько инстансов; общие сессии, данные, блокировки |

---

### 26.7. Cache-Aside — пример кода (IMemoryCache)

```csharp
public class ProductService
{
    private readonly IProductRepository _repo;
    private readonly IMemoryCache _cache;
    private const string CacheKeyPrefix = "product:";

    public ProductService(IProductRepository repo, IMemoryCache cache)
    {
        _repo = repo;
        _cache = cache;
    }

    public async Task<ProductDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var key = CacheKeyPrefix + id;
        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            return await _repo.GetByIdAsync(id, ct);
        });
    }

    public async Task UpdateAsync(int id, ProductDto dto, CancellationToken ct)
    {
        await _repo.UpdateAsync(id, dto, ct);
        _cache.Remove(CacheKeyPrefix + id);  // invalidation
    }
}
```

---

## БЛОК 27: Design Patterns — подробнее

### 27.0. GoF паттерны — краткая шпаргалка для собеседования

#### Порождающие (Creational) — как создавать объекты

| Паттерн | Что сказать на собесе |
|---------|------------------------|
| **Абстрактная фабрика (Abstract Factory)** | Семейство связанных объектов создаётся через интерфейс фабрики. Клиент не знает конкретные классы. Пример: UI-элементы для Windows/Mac — одна фабрика даёт кнопки и поля под свою ОС. |
| **Строитель (Builder)** | Пошаговое создание сложного объекта. Builder с методами `WithX()`, в конце `Build()`. Читаемый код, валидация при сборке. Примеры: FluentValidation, настройка HttpClient, `StringBuilder`. |
| **Фабричный метод (Factory Method)** | Класс делегирует создание объекта подклассам. Виртуальный метод `CreateProduct()` — каждая фабрика создаёт свой тип. Ослабляет связь с конкретным классом. |
| **Прототип (Prototype)** | Создание копии объекта через клонирование (`ICloneable`, `MemberwiseClone`). Когда создание «с нуля» дорого или объект сложно инициализировать. |
| **Одиночка (Singleton)** | Один экземпляр на приложение. В .NET — через DI `AddSingleton<T>()`. Использовать осторожно: тестируемость, глобальное состояние. |

#### Структурные (Structural) — как компоновать классы и объекты

| Паттерн | Что сказать на собесе |
|---------|------------------------|
| **Адаптер (Adapter)** | Адаптирует интерфейс одного класса под ожидаемый другим. «Обёртка» над чужим API. Пример: класс-адаптер для внешней библиотеки с неудобным API. |
| **Мост (Bridge)** | Разделяет абстракцию и реализацию, чтобы они менялись независимо. Вместо иерархии «круг-красный, круг-синий» — абстракция (форма) + реализация (цвет). |
| **Компоновщик (Composite)** | Древовидная структура: узлы и листья обрабатываются одинаково. Пример: папки и файлы, UI-дерево (контейнеры и элементы). |
| **Декоратор (Decorator)** | Оборачивает объект, добавляя поведение до/после вызова. Тот же интерфейс. Примеры: логирование `IUserService`, кэширование, Middleware в ASP.NET Core. |
| **Фасад (Facade)** | Простой интерфейс к сложной подсистеме. Скрывает множество классов за одним. Пример: `HttpClient` — фасад над `HttpClientHandler`, пулами и т.д. |
| **Приспособленец (Flyweight)** | Разделение общего состояния между множеством объектов. Экономия памяти: общие данные (например, символы шрифта) хранятся один раз. |
| **Заместитель (Proxy)** | Обёртка, контролирующая доступ к объекту. Ленивая загрузка, кэш, проверка прав. Пример: `Lazy<T>`, прокси для удалённого сервиса. |

#### Поведенческие (Behavioral) — алгоритмы и взаимодействие

| Паттерн | Что сказать на собесе |
|---------|------------------------|
| **Цепочка обязанностей (Chain of Responsibility)** | Запрос идёт по цепочке обработчиков; каждый решает обработать или передать дальше. **Middleware в ASP.NET Core** — классический пример. |
| **Команда (Command)** | Инкапсулирует действие как объект. Можно ставить в очередь, отменять, логировать. Примеры: Undo/Redo, очереди задач, CQRS-команды. |
| **Интерпретатор (Interpreter)** | Представление грамматики языка и интерпретация выражений. Дерево узлов, каждый узел обрабатывает свою часть. Редко в бизнес-коде. |
| **Итератор (Iterator)** | Доступ к элементам коллекции без раскрытия её структуры. `IEnumerable<T>`, `foreach`. В .NET — встроен повсеместно. |
| **Посредник (Mediator)** | Объекты общаются через посредника, а не напрямую. **MediatR** — команда/запрос → handler. Снижает связанность. |
| **Хранитель (Memento)** | Сохранение и восстановление состояния объекта без нарушения инкапсуляции. Snapshot для Undo. |
| **Наблюдатель (Observer)** | Один объект уведомляет подписчиков об изменениях. В C# — `event` + delegate. В микросервисах — RabbitMQ, Kafka. |
| **Состояние (State)** | Поведение объекта зависит от внутреннего состояния. Вместо множества `if` — отдельный класс на каждое состояние. Конечный автомат. |
| **Стратегия (Strategy)** | Семейство алгоритмов, взаимозаменяемых. Инжект `IStrategy` в конструктор. Пример: разные способы расчёта скидки. |
| **Шаблонный метод (Template Method)** | Базовый класс задаёт скелет алгоритма, подклассы переопределяют шаги. `Execute()` вызывает `Step1()`, `Step2()` — виртуальные. |
| **Посетитель (Visitor)** | Добавление операций к иерархии классов без их изменения. Отдельный Visitor обходит структуру и выполняет действия. Двойная диспетчеризация. |

#### Паттерны классов vs объектов

**Паттерны классов** — отношения через наследование, определяются на этапе компиляции: Factory Method, Interpreter, Template Method, Adapter (классовый).

**Паттерны объектов** — отношения между объектами, возникают в runtime, гибче: остальные из списка выше.

---

#### 27.0a. Creational — Singleton, Factory, Abstract Factory, Builder (примеры на C#)

**Singleton — один экземпляр на приложение.** В .NET предпочтительно через DI: `builder.Services.AddSingleton<ICacheService, MemoryCacheService>()`. Проблемы классического Singleton (статическое поле + приватный конструктор): сложно тестировать (глобальное состояние), нельзя подменить в тестах, скрытая зависимость. Поэтому в ASP.NET Core Singleton через контейнер — явная регистрация и инжект.

```csharp
// BAD: classic Singleton — hard to test, hidden dependency
public sealed class ConfigSingleton
{
    private static readonly Lazy<ConfigSingleton> _instance = new(() => new ConfigSingleton());
    public static ConfigSingleton Instance => _instance.Value;
    private ConfigSingleton() { }
}

// GOOD: Singleton via DI — testable, explicit
builder.Services.AddSingleton<IConfigService, ConfigService>();
```

**Factory Method** — создание объекта делегируется методу (часто виртуальному), подклассы переопределяют и возвращают нужный тип. Ослабляет связь с конкретным классом.

```csharp
public abstract class DocumentExporter
{
    public byte[] Export(Document doc)
    {
        var writer = CreateWriter();  // Factory method
        return writer.Write(doc);
    }
    protected abstract IDocumentWriter CreateWriter();
}
public class PdfExporter : DocumentExporter
{
    protected override IDocumentWriter CreateWriter() => new PdfWriter();
}
```

**Abstract Factory** — создание семейства связанных объектов (например, UI-элементы для темы). Клиент работает с интерфейсом фабрики, не зная конкретных классов.

```csharp
public interface IUiFactory
{
    IButton CreateButton();
    ITextBox CreateTextBox();
}
public class DarkThemeFactory : IUiFactory
{
    public IButton CreateButton() => new DarkButton();
    public ITextBox CreateTextBox() => new DarkTextBox();
}
// Client receives IUiFactory (e.g. from DI) and creates consistent theme
```

**Builder** — пошаговое создание сложного объекта. Fluent-интерфейс, в конце `Build()`. Когда много опциональных параметров или сложная валидация при сборке.

```csharp
public class OrderBuilder
{
    private string _customer;
    private readonly List<OrderItem> _items = new();
    public OrderBuilder WithCustomer(string name) { _customer = name; return this; }
    public OrderBuilder AddItem(int productId, int qty) { _items.Add(new OrderItem(productId, qty)); return this; }
    public Order Build()
    {
        if (string.IsNullOrEmpty(_customer)) throw new InvalidOperationException("Customer required");
        return new Order { CustomerName = _customer, Items = _items.ToList() };
    }
}
// Usage: new OrderBuilder().WithCustomer("Alice").AddItem(1, 2).Build();
```

**Когда применять:** Singleton — один общий сервис (кэш, конфиг) через DI. Factory Method — иерархия классов, у каждого свой тип создаваемого объекта. Abstract Factory — семейства продуктов (темы, провайдеры). Builder — сложная конфигурация объекта, много опций, валидация при сборке.

---

#### 27.0b. Behavioral — Strategy, Observer, Command, Repository, Unit of Work (примеры)

**Strategy** — взаимозаменяемые алгоритмы за общим интерфейсом. Клиент получает стратегию извне (DI). Пример: расчёт скидки (см. SOLID OCP в документе) — `IDiscountStrategy`, реализации PercentageDiscount, FixedDiscount.

```csharp
public interface IPricingStrategy
{
    decimal Calculate(Order order);
}
public class StandardPricing : IPricingStrategy { ... }
public class VipPricing : IPricingStrategy { ... }
// Injected into OrderService — easy to test and extend
```

**Observer** — один объект уведомляет подписчиков об изменениях. В C# — `event` + delegate; в микросервисах — сообщения (RabbitMQ, Kafka).

```csharp
public class OrderService
{
    public event EventHandler<OrderCreatedEventArgs> OrderCreated;
    public void Create(Order order)
    {
        // ... save order ...
        OrderCreated?.Invoke(this, new OrderCreatedEventArgs(order));
    }
}
// Subscribers: email sender, analytics, inventory — subscribe to OrderCreated
```

**Command** — действие инкапсулировано в объект. Можно ставить в очередь, отменять, логировать. CQRS-команды в MediatR — типичный пример.

```csharp
public record CreateOrderCommand(string CustomerName, List<OrderItemDto> Items) : IRequest<OrderResult>;
public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderResult>
{
    public async Task<OrderResult> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        // Create order, save, return result
    }
}
// Controller: await _mediator.Send(new CreateOrderCommand(...));
```

**Repository** — абстракция доступа к данным. Интерфейс в Application/Domain, реализация в Infrastructure. Скрывает EF/SQL, даёт тестируемость (mock). См. вопросы 59, 239.

```csharp
public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id, CancellationToken ct = default);
    Task AddAsync(Order order, CancellationToken ct = default);
}
public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _db;
    public OrderRepository(AppDbContext db) => _db = db;
    public async Task<Order?> GetByIdAsync(int id, CancellationToken ct) =>
        await _db.Orders.FindAsync(new object[] { id }, ct);
    // ...
}
```

**Unit of Work** — группа операций в одной транзакции. Один Commit для нескольких репозиториев. См. вопрос 60.

```csharp
public interface IUnitOfWork
{
    IOrderRepository Orders { get; }
    IProductRepository Products { get; }
    Task SaveChangesAsync(CancellationToken ct = default);
}
// In handler: _uow.Orders.Add(order); _uow.Products.UpdateStock(...); await _uow.SaveChangesAsync(ct);
```

---

### 27.1. Decorator (декоратор)

Оборачивает объект другим объектом того же типа (или интерфейса), добавляя поведение до/после вызова. В DI — регистрация «декорируемого» сервиса и обёртка поверх него. Примеры: логирование вызовов `IUserService`, кэширование ответов, проверки прав. Middleware в ASP.NET Core — цепочка обработчиков, по смыслу близко к декоратору/chain of responsibility.

---

### 27.2. Builder (строитель)

Пошаговое создание сложного объекта. Отдельный класс Builder с методами типа `WithName()`, `WithItems()`, в конце `Build()`. Используется для сложных конфигураций (FluentValidation, настройка HttpClient, построение запросов/фильтров). Улучшает читаемость и даёт валидацию на этапе сборки.

---

### 27.3. Mediator (посредник)

Объекты не вызывают друг друга напрямую, а отправляют запрос/команду через **медиатор**. Медиатор определяет, кто обрабатывает запрос. **MediatR** в .NET — реализация: одна команда/запрос — один handler, плюс pipeline (validation, logging). Снижает связанность между слоями и упрощает добавление сквозной логики.

---

### 27.4. Chain of Responsibility (цепочка обязанностей)

Запрос передаётся по цепочке обработчиков; каждый решает обработать или передать дальше. **Middleware в ASP.NET Core** — это и есть цепочка: каждый компонент вызывает `next()` или завершает обработку. Связь на собеседовании: «Middleware — реализация Chain of Responsibility».

---

## БЛОК 28: Mapping и Pagination

### 28.1. Ручной маппинг vs AutoMapper vs Mapster

- **Ручной маппинг** — явные присвоения в коде или отдельный класс-маппер. Полный контроль, без «магии», легко отлаживать. Подходит для небольших моделей и критичных к производительности мест.
- **AutoMapper** — конвенции по именам свойств, настройка через `CreateMap`. Удобно для больших моделей; нужно знать правила и ограничения (проекции в IQueryable, вложенные объекты). Нагрузка на рефлексию и конфигурацию при старте.
- **Mapster** — быстрее AutoMapper, меньше конфигурации, генерация кода на лету. Часто выбирают как компромисс между удобством и скоростью.

Когда что: ручной — когда мало полей или важна производительность; AutoMapper/Mapster — когда много DTO и сущностей, и хочется уменьшить шаблонный код.

---

### 28.2. Pagination: offset vs cursor (keyset)

- **Offset (Skip/Take):** `ORDER BY Id OFFSET 1000 ROWS FETCH NEXT 20 ROWS ONLY`. На больших смещениях БД всё равно «проходит» по пропущенным строкам — при росте offset запрос замедляется. Подходит для небольших объёмов и простых списков.
- **Cursor (keyset):** «Дай 20 записей после ключа X». Например, `WHERE Id > @lastSeenId ORDER BY Id FETCH NEXT 20`. Масштабируется лучше: используется индекс, не зависит от глубины «страницы». Нельзя прыгнуть на произвольную страницу (только «вперёд»). Идеально для бесконечной ленты и API с `nextPageToken`.

На собеседовании: «Почему offset плохо на больших таблицах?» — при большом offset сервер по сути сканирует и отбрасывает много строк; cursor использует индекс и стабилен по времени.

---

## БЛОК 29: API Design

### 29.1. Версионирование API: подходы

- **URL path:** `/api/v1/users`, `/api/v2/users`. Понятно, кэшируется, но дублирует путь.
- **Query string:** `/api/users?api-version=1.0`. Не меняет путь, но версия не в ресурсе.
- **Header:** `Accept: application/json; api-version=1.0` или кастомный `X-Api-Version`. Не загрязняет URL, но сложнее кэширования и тестирования.

В ASP.NET Core: пакет `Microsoft.AspNetCore.Mvc.Versioning` или встроенные возможности; выбор стратегии (URL, query, header) и маппинг версии на контроллеры.

---

### 29.2. Идемпотентность ключей для POST

Чтобы повторная отправка одного и того же запроса не создавала дубликат, клиент передаёт **Idempotency-Key** (заголовок или поле): сервер сохраняет ключ и результат; при повторном запросе с тем же ключом возвращает сохранённый ответ без повторного выполнения. Важно для платежей и создания заказов.

---

### 29.3. Response envelope patterns

Обёртка ответа: `{ "data": { ... }, "meta": { "total": 100 }, "errors": [] }`. Плюсы: единый формат, место для метаданных (пагинация, версия). Минусы: лишний уровень вложенности. Часто делают опционально (заголовки для метаданных) или только для списков с пагинацией. На собеседовании — знать про оба подхода и trade-offs.

---

## БЛОК 30: Resilience (Polly: Timeout, Bulkhead, Fallback)

### 30.1. Timeout

Ограничение времени ожидания вызова. Без таймаута зависший внешний сервис может держать поток/запрос бесконечно. В Polly: `Policy.TimeoutAsync(TimeSpan.FromSeconds(5))`. Часто комбинируют с Retry: сначала retry с таймаутом на каждую попытку.

---

### 30.2. Bulkhead (изоляция)

Ограничение числа параллельных вызовов к одному ресурсу (отдельная «переборка»). Если один тип вызовов исчерпал лимит, остальные типы не блокируются. В Polly: `Policy.BulkheadAsync(maxParallelization, maxQueuingActions)`. Снижает каскадные сбои.

---

### 30.3. Fallback

При сбое выполнить запасной вариант (вернуть кэш, значение по умолчанию, другой источник). В Polly: `Policy.Handle<T>().FallbackAsync(fallbackAction)`. Улучшает доступность при деградации внешних зависимостей.

---

### 30.4. Комбинирование (Polly / Microsoft.Extensions.Resilience)

Retry + Timeout + Circuit Breaker + Fallback — типичный набор для вызовов к внешним API. В .NET 8+ — `Microsoft.Extensions.Resilience` и `AddStandardResilienceHandler()` с настройкой retry, timeout, circuit breaker. На Middle+ спрашивают: зачем каждый паттерн и как их совмещать.

---

## БЛОК 31: Observability

### 31.1. Correlation ID

Уникальный идентификатор запроса (например, `X-Correlation-Id` или из `Activity`), прокидывается через все логи и при необходимости во все вложенные вызовы (другие сервисы, очереди). Позволяет собрать все логи одного запроса в одну трассировку. Реализация: middleware, который читает/генерирует ID и кладёт в `HttpContext.Items` и в scope логгера (Serilog: `LogContext.PushProperty("CorrelationId", id)`).

---

### 31.2. Health Checks — детали реализации

`AddHealthChecks()` — регистрация проверок; `MapHealthChecks("/health")` — эндпоинт. Проверки: БД (`AddDbContextCheck`), внешний API (HttpClient), Redis, диск. Разделение на **liveness** (жив ли процесс) и **readiness** (готов ли принимать трафик — БД доступна, кэш доступен). В Kubernetes: liveness — перезапуск пода; readiness — исключение из балансировки.

---

### 31.3. Metrics (Prometheus / OpenTelemetry)

**Метрики** — счётчики, гистограммы, gauges (например, количество запросов, задержка, размер очереди). **Prometheus** — pull-модель, скрейпит эндпоинт `/metrics`. **OpenTelemetry** — единый стандарт для трасс, метрик и логов; экспорт в Prometheus, Jaeger и др. В ASP.NET Core: `UseOpenTelemetry()`, метрики по HTTP-запросам из коробки. Middle должен хотя бы знать: зачем метрики, что такое counter/histogram и как посмотреть их в дашборде.

---

## БЛОК 32: Тестирование — глубже

### 32.1. FluentAssertions

Библиотека утверждений с цепочками в стиле «читаемого» кода: `result.Should().NotBeNull(); result.Name.Should().Be("Expected"); list.Should().HaveCount(3).And.Contain(x => x.Id == 1)`. Удобно для коллекций, исключений, дат. В резюме и на собеседовании — признак внимания к качеству тестов.

---

### 32.2. Testcontainers

Запуск реальных сервисов (БД, Redis, RabbitMQ) в Docker-контейнерах во время тестов. Интеграционные тесты против настоящей БД без ручного поднятия окружения. В .NET: `Testcontainers.MsSql`, настройка в конструкторе теста или fixture; получение connection string из контейнера и подстановка в конфиг. На собеседовании: «Как тестируешь с БД?» — in-memory провайдер для скорости или Testcontainers для реалистичности.

---

### 32.3. WebApplicationFactory подробнее

Создаёт тестовый хост приложения без реального HTTP-сервера. Подмена сервисов: `WithWebHostBuilder(b => b.ConfigureTestServices(services => { services.Replace<IService, MockService>(); }))`. Подмена конфигурации, добавление тестовой аутентификации. Вызов `CreateClient()` и `client.GetAsync("/api/...")` для проверки эндпоинтов. Используется для интеграционных тестов API с реальной конфигурацией и middleware.

---

## БЛОК 33: C# — новые фичи

### 33.1. global using

`global using System;` (и другие пространства имён или алиасы) в одном файле — эти using применяются ко всему проекту. Уменьшает повторения в начале файлов. Обычно в одном месте (например, `GlobalUsings.cs`) или в начале основного файла.

---

### 33.2. File-scoped namespaces

`namespace MyProject.Services;` без фигурных скобок — всё содержимое файла в этом namespace. Меньше отступов, чище код. C# 10+.

---

### 33.3. required (C# 11)

Модификатор для свойств и полей: инициализация обязательна при создании объекта (в конструкторе или object initializer). Компилятор проверяет, что значение задано. Удобно для DTO и конфигурационных объектов.

---

### 33.4. Primary constructors (C# 12)

Параметры конструктора прямо в объявлении класса: `public class UserService(IUserRepository repo) { }` — параметр становится полем, доступным в классе. Меньше шаблона для инжекта зависимостей. Для record типов primary constructor задаёт свойства.

---

### 33.5. Collection expressions (C# 12)

Единый синтаксис для коллекций: `int[] a = [1, 2, 3];` или `List<int> b = [1, 2, 3];`. Работает с span, массивом, списком в зависимости от контекста. Удобно для инициализации и передачи литералов.

---

### 33.6. Source Generators — введение

**Source Generators** — механизм компилятора Roslyn (C# 9+), позволяющий генерировать дополнительный C#-код **во время компиляции**. Результат — обычные файлы `.cs`, которые становятся частью сборки. В отличие от Reflection — генерация на этапе компиляции, без runtime-накладных расходов.

#### Зачем нужны

- **Производительность:** генерация кода вместо Reflection (сериализаторы, мапперы, DI).
- **Метаданные на этапе компиляции:** доступ к синтаксическим деревьям и семантической модели.
- **Автогенерация:** DTO, builders, partial-методы, валидаторы по атрибутам.

#### Как устроены

1. Создаётся проект типа **Analyzer** (C# 9+).
2. Класс реализует интерфейс `ISourceGenerator` и атрибут `[Generator]`.
3. Метод `Execute` получает `GeneratorExecutionContext` и добавляет исходники через `context.AddSource`.

```csharp
[Generator]
public class MySourceGenerator : IIncrementalGenerator  // или ISourceGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Находим все классы с атрибутом [GenerateDto]
        var provider = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: (node, _) => node is ClassDeclarationSyntax c && c.AttributeLists.Count > 0,
                transform: (ctx, _) => GetClassInfo(ctx))
            .Where(static m => m != null);

        context.RegisterSourceOutput(provider, (spc, source) =>
        {
            // Генерируем код
            var generated = GenerateDto(source);
            spc.AddSource($"{source.Name}.g.cs", generated);
        });
    }
}
```

#### Где применяются

- **System.Text.Json** — сериализация без Reflection (AOT, производительность).
- **ASP.NET Core** — минимальные API, endpoint-генерация.
- **MediatR** — генерация handlers.
- **Entity Framework** — оптимизация запросов.
- **Custom:** DTO из интерфейсов, маппинг по конвенциям, валидаторы.

#### IIncrementalGenerator vs ISourceGenerator

**IIncrementalGenerator** (C# 10+) — инкрементальная генерация: при изменении одного файла пересчитывается только затронутая часть. Быстрее для больших решений. **ISourceGenerator** — полный проход при каждой компиляции.

#### На собеседовании

«Source Generators — что это?» — генерация C#-кода во время компиляции. Используются для производительности (без Reflection), автогенерации DTO, мапперов, валидаторов. Примеры: System.Text.Json, EF Core, MediatR.

---

## БЛОК 34: Опционально (не обязательно для Middle)

> Темы, которые часто спрашивают на production-вакансиях или при микросервисном стеке. Полезно знать, но не критично для базового уровня Middle.

---

### 34.1. HttpClient — best practices (часто спрашивают)

**Почему нельзя создавать `new HttpClient()` в каждом методе или запросе:** под капотом каждый HttpClient при Dispose не сразу освобождает сокеты; они переходят в состояние TIME_WAIT. При большом числе запросов накапливаются сокеты → **socket exhaustion** (исчерпание портов/дескрипторов), ошибки типа «Unable to connect».

**Решение: IHttpClientFactory.** Фабрика управляет жизненным циклом HttpClient'ов, переиспользует соединения и освобождает их корректно. Регистрация: `AddHttpClient()`.

- **Typed client** — класс, принимающий `HttpClient` в конструкторе; регистрируется как `AddHttpClient<IMyApiClient, MyApiClient>()`. Удобно для вызовов одного внешнего API с базовым URL и настройками.
- **Named client** — `AddHttpClient("ExternalApi", client => { ... })`; получение через `IHttpClientFactory.CreateClient("ExternalApi")`. Когда нужно несколько разных конфигураций без отдельного типа.

Рекомендация Microsoft: не создавать HttpClient вручную в долгоживущем коде; использовать IHttpClientFactory. Это production-тема для Middle.

---

### 34.2. Микросервисы и монолит — когда что, API Gateway, Service Discovery, Strangler Fig, Distributed Tracing

**Когда монолит:** маленькая команда, один продукт, простые границы, быстрый старт. Один деплой, одна БД, проще транзакции и отладка. Подходит для многих проектов на этапе роста.

**Когда микросервисы:** большие команды (независимые команды по сервисам), разные технологии/масштабирование по частям, чёткие границы контекстов (DDD bounded context). Цена: распределённые транзакции, сетевая задержка, операционная сложность (деплой, мониторинг, отказоустойчивость).

**API Gateway** — единая точка входа для клиентов. Маршрутизирует запросы к нужному сервису, аутентификация, rate limiting, агрегация ответов. В .NET: **Ocelot**, **YARP**. Клиент знает только шлюз; адреса сервисов скрыты.

**Service Discovery** — сервисы находят друг друга по имени. Сервис при старте регистрируется (Consul, Eureka); при вызове запрашивается актуальный адрес. Решает смену портов и масштабирование инстансов.

**Strangler Fig (паттерн «удушающая фига»)** — постепенная миграция с монолита на микросервисы. Новый функционал делается в новых сервисах; старый монолит остаётся, но трафик к отдельным сценариям переводится на новые сервисы (через роутинг в шлюзе или прокси). Со временем «обёртка» монолита «удушается» — всё больше запросов идут в микросервисы, монолит уменьшается или выключается. Без «большого взрыва» переписывания.

**Distributed tracing** — сквозной идентификатор запроса (trace id) и цепочка спанов (span) по всем сервисам и вызовам. Позволяет увидеть полный путь запроса: API → OrderService → PaymentService → DB. Реализации: **OpenTelemetry**, **Jaeger**, **Application Insights**. В каждом сервисе прокидывать trace id (заголовок `traceparent` / W3C или `X-Trace-Id`) и логировать в span; в дашборде собирать трассировку по trace id. На собеседовании: «Как ищешь причину медленного запроса в микросервисах?» — distributed tracing по trace id, смотрю, в каком сервисе или вызове ушло время.

**Sidecar pattern** — рядом с сервисом контейнер (sidecar), который берёт на себя сеть: прокси, TLS, метрики, service mesh (Envoy). Сервис не знает деталей сети.

---

### 34.3. Record struct (C# 10)

**record** по умолчанию — ссылочный тип (class). **record struct** — value type: хранится в стеке (или inline), копируется по значению, не может быть null (без nullable). Синтаксис: `public record struct Point(int X, int Y);`. Подходит для небольших неизменяемых данных, где важна производительность и отсутствие аллокаций в куче. Разница с обычным record: record = reference type, record struct = value type; record struct может быть readonly struct для полной неизменяемости.

---

### 34.4. Nullable reference types (NRT)

В C# 8+ с **nullable context** (`#nullable enable` или в csproj `<Nullable>enable</Nullable>`) ссылочные типы по умолчанию считаются non-nullable. `string name` — компилятор ожидает, что null не присвоят; `string? optionalName` — явно «может быть null». Компилятор выдаёт предупреждения при присвоении null в non-nullable и при разыменовании без проверки. Цель — ловить NullReferenceException на этапе компиляции. Зачем включать: меньше сюрпризов в рантайме, явный контракт в сигнатурах методов. На собеседовании: «Что такое string?» — nullable reference type в включённом nullable context.

---

### 34.5. Result pattern

Вместо выброса исключения метод может возвращать **Result&lt;T&gt;** (или Result без значения): успех с данными или неудача с кодом/сообщением ошибки. Вызывающий код проверяет `result.IsSuccess` и обрабатывает ошибку без try-catch. Плюсы: явная обработка ошибок в сигнатуре, исключения только для действительно исключительных ситуаций, удобно для валидации и бизнес-ошибок (например, «пользователь не найден»). Минусы: больше кода на проверки, нужна дисциплина у команды. Оправдано в сервисном слое и CQRS-handlers, где много ожидаемых «ошибок» (not found, validation). В документе в примере кода используется `Result<User>` — это и есть Result pattern: возвращать результат или ошибку объектом, а не исключением.

---

## 📊 Статистика документа

- **Всего вопросов / тем:** 295+ (включая чеклист и блоки 19–34)
- **Блоки:** 34
- **Темы:** C#, .NET, ASP.NET Core, SQL Server, React, EF Core, тестирование (xUnit, Moq, FluentAssertions, AAA, TDD, code coverage), Git, DevOps, Azure DevOps (CI/CD pipelines, build/test/deploy, environments, approvals), Docker (Dockerfile ASP.NET Core, docker-compose, контейнеры vs VM), soft skills, память, SOLID, Clean Architecture (слои, Onion, Hexagonal, рассказ 3–5 мин), CQRS, MediatR (handlers, pipeline behaviors), Security (JWT структура, access/refresh, хранение, OAuth/OIDC, Identity, безопасность API), Production, HTTP/REST, REST API Design, Caching (IMemoryCache, IDistributedCache, Redis, cache-aside, invalidation), Design Patterns, Mapping, Pagination, API Design, Resilience, Observability, C# новые фичи (в т.ч. Source Generators), Worker Service, IHostedService, IServiceProvider, EF (Value Objects, Interceptors), HttpClient, микросервисы и монолит, record struct, NRT, Result pattern

---

> **Совет:** Не учи ответы наизусть. Понимай суть, уметь объяснить своими словами и привести примеры из своего опыта.
