# Примеры задач для Code Review на собеседовании (C# / .NET)

Подробный разбор типичных примеров кода с построчным объяснением: что делает каждая строка, параметр, метод, какие проблемы возможны и как улучшить. Формат ответа: «что делает → как работает → проблемы → как улучшить» (около 60 секунд на блок).

---

## 1. STRING OPERATIONS (очень вероятно ~90%)

### Пример 1.1: Парсинг CSV данных

**Исходный код (с проблемами):**

```csharp
public List<PolicyData> ParsePolicyFile(string csvContent)
{
    var policies = new List<PolicyData>();
    var lines = csvContent.Split('\n');

    foreach (var line in lines.Skip(1)) // Skip header
    {
        var fields = line.Split(',');
        var policy = new PolicyData
        {
            PolicyNumber = fields[0].Trim(),
            CustomerName = fields[1].Trim(),
            Premium = decimal.Parse(fields[2]),
            StartDate = DateTime.Parse(fields[3])
        };
        policies.Add(policy);
    }

    return policies;
}
```

#### Построчное объяснение

| Строка | Код | Что происходит |
|--------|-----|----------------|
| 1 | `public List<PolicyData> ParsePolicyFile(string csvContent)` | Метод возвращает список `PolicyData`. Параметр `csvContent` — сырой текст CSV (может быть `null`). |
| 2 | `var policies = new List<PolicyData>();` | Создаётся пустой список для накопления результатов. |
| 3 | `var lines = csvContent.Split('\n');` | `Split('\n')` разбивает строку по символу перевода строки. Возвращает `string[]`. Если `csvContent == null` → **NullReferenceException**. |
| 5 | `foreach (var line in lines.Skip(1))` | `Skip(1)` пропускает первую строку (заголовок). Итератор отдаёт строки начиная со второй. Пустые строки не отфильтрованы. |
| 6 | `var fields = line.Split(',');` | Каждая строка разбивается по запятой. Нет обработки запятых внутри кавычек (стандартная особенность CSV). |
| 7–12 | `var policy = new PolicyData { ... }` | Создаётся объект. `fields[0]`, `fields[1]` — при длине `fields` < 4 дадут **IndexOutOfRangeException**. `decimal.Parse` и `DateTime.Parse` при невалидных данных выбросят **FormatException**. |
| 13 | `policies.Add(policy);` | Объект добавляется в список. |
| 16 | `return policies;` | Возвращается заполненный список. |

#### Что спросят на собеседовании

- **«Объясни построчно, что происходит»** — см. таблицу выше.
- **«Какие проблемы здесь видишь?»**:
  - Нет проверки на `null` для `csvContent`.
  - **IndexOutOfRangeException**, если в строке меньше 4 полей.
  - **FormatException** при невалидном числе или дате в `Parse`.
  - Пустые строки не отбрасываются (могут дать пустые или неполные записи).
  - Нет обработки кавычек и запятых внутри полей (не полноценный CSV-парсер).

#### Улучшенная версия

```csharp
public List<PolicyData> ParsePolicyFileSafe(string csvContent)
{
    // Guard: reject null or whitespace input
    if (string.IsNullOrWhiteSpace(csvContent))
        throw new ArgumentException("CSV content is empty", nameof(csvContent));

    var policies = new List<PolicyData>();
    // RemoveEmptyEntries avoids empty lines from double newlines
    var lines = csvContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);

    foreach (var line in lines.Skip(1))
    {
        var fields = line.Split(',');
        // Skip lines that don't have enough columns
        if (fields.Length < 4) continue;

        // TryParse avoids FormatException; out vars hold parsed values
        if (decimal.TryParse(fields[2], out var premium) &&
            DateTime.TryParse(fields[3], out var startDate))
        {
            var policy = new PolicyData
            {
                PolicyNumber = fields[0]?.Trim() ?? string.Empty,
                CustomerName = fields[1]?.Trim() ?? string.Empty,
                Premium = premium,
                StartDate = startDate
            };
            policies.Add(policy);
        }
    }

    return policies;
}
```

Кратко: проверка входа, `RemoveEmptyEntries`, проверка `fields.Length`, `TryParse` вместо `Parse`, null-safe `Trim` с `?? string.Empty`.

---

### Пример 1.2: Форматирование номера полиса

**Исходный код (с проблемами):**

```csharp
public string FormatPolicyNumber(string input)
{
    var cleanNumber = input.Replace("-", "").Replace(" ", "");

    if (cleanNumber.Length != 10)
        return input;

    return $"{cleanNumber.Substring(0, 3)}-{cleanNumber.Substring(3, 3)}-{cleanNumber.Substring(6, 4)}";
}
```

#### Построчное объяснение

| Строка | Код | Что происходит |
|--------|-----|----------------|
| 1 | `public string FormatPolicyNumber(string input)` | Метод принимает строку, возвращает отформатированный номер. `input` может быть `null`. |
| 2 | `var cleanNumber = input.Replace("-", "").Replace(" ", "");` | Удаляются дефисы и пробелы. При `input == null` → **NullReferenceException** на первом же `Replace`. |
| 4–5 | `if (cleanNumber.Length != 10) return input;` | Если после очистки длина не 10, возвращается исходная строка. Не проверяется, что символы — цифры. |
| 6 | `return $"{cleanNumber.Substring(0, 3)}-...` | Строка форматируется как `XXX-XXX-XXXX`. `Substring(0,3)` — первые 3 символа, и т.д. |

#### Что спросят

- **«Что делает этот код?»** — убирает дефисы и пробелы и форматирует номер как `XXX-XXX-XXXX`, если длина 10.
- **«Что если input = null?»** — **NullReferenceException** при вызове `input.Replace(...)`.
- **«Как улучшить?»** — проверка на null/пустоту, проверка что все символы — цифры, можно использовать range syntax.

#### Улучшенная версия

```csharp
public string FormatPolicyNumber(string input)
{
    if (string.IsNullOrWhiteSpace(input))
        return string.Empty;

    var cleanNumber = input.Replace("-", "").Replace(" ", "").Trim();

    // Require exactly 10 digits
    if (cleanNumber.Length != 10 || !cleanNumber.All(char.IsDigit))
        return input;

    // C# 8+ range syntax: [..3] = from start to index 3 (exclusive)
    return $"{cleanNumber[..3]}-{cleanNumber[3..6]}-{cleanNumber[6..]}";
}
```

---

## 2. LINQ (очень вероятно ~85%)

### Пример 2.1: Фильтрация и группировка полисов

```csharp
public class PolicyService
{
    private List<Policy> _policies;

    public decimal GetTotalPremiumByCustomer(int customerId)
    {
        return _policies
            .Where(p => p.CustomerId == customerId)
            .Sum(p => p.Premium);
    }

    public List<CustomerPolicyStats> GetCustomerStats()
    {
        return _policies
            .GroupBy(p => p.CustomerId)
            .Select(g => new CustomerPolicyStats
            {
                CustomerId = g.Key,
                TotalPolicies = g.Count(),
                TotalPremium = g.Sum(p => p.Premium),
                AveragePremium = g.Average(p => p.Premium)
            })
            .ToList();
    }
}
```

#### Построчное объяснение

**GetTotalPremiumByCustomer:**

| Код | Назначение |
|-----|------------|
| `_policies` | Источник — список полисов (может быть `null` → NRE). |
| `.Where(p => p.CustomerId == customerId)` | Фильтр: только полисы с заданным `CustomerId`. Возвращает `IEnumerable<Policy>`, выполнение отложенное (deferred). |
| `.Sum(p => p.Premium)` | Суммирует `Premium` по отфильтрованной последовательности. **Материализует** запрос (проход по данным). |

**GetCustomerStats:**

| Код | Назначение |
|-----|------------|
| `.GroupBy(p => p.CustomerId)` | Группирует полисы по `CustomerId`. Тип результата — `IEnumerable<IGrouping<int, Policy>>`. `IGrouping<int, Policy>`: `Key` — это `CustomerId`, внутри — все `Policy` этой группы. |
| `.Select(g => new CustomerPolicyStats { ... })` | Для каждой группы создаётся один объект статистики. `g.Key` — id клиента, `g.Count()` — число полисов, `g.Sum(...)` / `g.Average(...)` — сумма и средняя премия. |
| `.ToList()` | Выполняет весь запрос и кладёт результат в `List<CustomerPolicyStats>`. |

#### Что спросят

- **«Что делает каждый метод?»** — первый: сумма премий по клиенту; второй: по каждому клиенту — количество полисов, сумма и средняя премия.
- **«Что такое GroupBy? Что возвращает?»** — группирует элементы по ключу; возвращает последовательность групп `IGrouping<TKey, TElement>`.
- **«Когда выполняется LINQ?»** — в момент материализации: `Sum()` и `ToList()` вызывают выполнение; до этого — отложенное выполнение (deferred execution).
- **«Что если _policies очень большой?»** — один проход по данным, но `ToList()` держит весь результат в памяти; при необходимости можно возвращать `IEnumerable<>` и не вызывать `ToList()` до использования.

---

## 3. ASYNC / AWAIT (вероятно ~75%)

### Пример 3.1: Асинхронная загрузка данных (последовательно)

```csharp
public async Task<ClaimDetails> GetClaimDetailsAsync(int claimId)
{
    var claim = await _repository.GetClaimAsync(claimId);
    var policy = await _repository.GetPolicyAsync(claim.PolicyId);
    var customer = await _repository.GetCustomerAsync(policy.CustomerId);

    return new ClaimDetails
    {
        Claim = claim,
        Policy = policy,
        Customer = customer
    };
}
```

#### Построчное объяснение

| Строка | Код | Что происходит |
|--------|-----|----------------|
| 1 | `public async Task<ClaimDetails> GetClaimDetailsAsync(...)` | Асинхронный метод: возвращает `Task<ClaimDetails>`, внутри можно использовать `await`. |
| 2 | `var claim = await _repository.GetClaimAsync(claimId);` | Запускается запрос за claim. `await` приостанавливает метод до завершения задачи, поток не блокируется. Результат — объект claim. |
| 3 | `var policy = await _repository.GetPolicyAsync(claim.PolicyId);` | После получения claim выполняется запрос за policy. Выполнение **последовательное**: policy только после claim. |
| 4 | `var customer = await _repository.GetCustomerAsync(policy.CustomerId);` | Затем запрос за customer. Тоже последовательно. |
| 6–11 | `return new ClaimDetails { ... }` | Собирается DTO и возвращается (уже как результат завершённой задачи). |

#### Что спросят

- **«Что делает await?»** — ждёт завершения `Task` без блокировки потока; управление возвращается вызывающему коду до завершения задачи.
- **«Запросы параллельно или последовательно?»** — **последовательно**: policy после claim, customer после policy.
- **«Как оптимизировать?»** — загружать policy и customer параллельно после получения claim (см. ниже).

#### Улучшенная версия (параллельная загрузка)

```csharp
public async Task<ClaimDetails> GetClaimDetailsOptimizedAsync(int claimId)
{
    var claim = await _repository.GetClaimAsync(claimId);

    // Start both requests without awaiting - they run in parallel
    var policyTask = _repository.GetPolicyAsync(claim.PolicyId);
    var customerTask = _repository.GetCustomerAsync(claim.CustomerId);

    await Task.WhenAll(policyTask, customerTask);

    return new ClaimDetails
    {
        Claim = claim,
        Policy = policyTask.Result,
        Customer = customerTask.Result
    };
}
```

Кратко: после claim запускаются две задачи без `await`, затем `Task.WhenAll` ждёт обе. Для доступа к результатам после `WhenAll` безопасно использовать `.Result` (задачи уже завершены). Альтернатива: `var (policy, customer) = (await policyTask, await customerTask);`.

---

## 4. NULL HANDLING (вероятно ~80%)

### Пример 4.1: Расчёт скидки с nullable

**Исходный код (с проблемами):**

```csharp
public decimal CalculateDiscount(Policy policy)
{
    decimal discount = 0;

    if (policy.DiscountPercentage != null)
    {
        discount = policy.Premium * (policy.DiscountPercentage.Value / 100);
    }

    return policy.Premium - discount;
}
```

#### Построчное объяснение

| Код | Назначение |
|-----|------------|
| `Policy policy` | Может быть `null` → при обращении к `policy.Premium` или `policy.DiscountPercentage` будет **NullReferenceException**. |
| `policy.DiscountPercentage != null` | Проверка, задан ли процент скидки (nullable тип, например `decimal?`). |
| `policy.DiscountPercentage.Value` | `.Value` у `Nullable<T>` даёт значение; если бы было `null`, вызов `.Value` выбросил бы `InvalidOperationException` (здесь мы под защитой `if`). |
| `policy.Premium - discount` | Итоговая сумма после скидки. |

#### Что спросят

- **«Что если policy = null?»** — **NullReferenceException** при первом обращении к `policy`.
- **«Объясни .Value»** — доступ к значению внутри `Nullable<T>`; при `null` — исключение.
- **«Как упростить?»** — проверка `policy == null`, использование null-coalescing `??` для процента.

#### Улучшенная версия

```csharp
public decimal CalculateDiscount(Policy? policy)
{
    ArgumentNullException.ThrowIfNull(policy);

    var discountPercentage = policy.DiscountPercentage ?? 0;
    var discount = policy.Premium * discountPercentage / 100;
    return policy.Premium - discount;
}
```

Или с явной проверкой: `if (policy == null) throw new ArgumentNullException(nameof(policy));`.

---

## 5. EXCEPTION HANDLING (вероятно ~70%)

### Пример 5.1: Обработка ошибок при обработке претензии

**Исходный код (с проблемами):**

```csharp
public void ProcessClaim(int claimId)
{
    try
    {
        var claim = _repository.GetClaim(claimId);
        ValidateClaim(claim);
        _paymentService.ProcessPayment(claim);
        _repository.UpdateClaimStatus(claimId, "Processed");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
```

#### Проблемы

- Ловит **все** исключения (`catch (Exception)`), в том числе критические.
- Только вывод в консоль — нет структурированного логирования.
- Нет rethrow — вызывающий код не узнает об ошибке.
- Нет отката состояния при сбое (например, откат транзакции или обновление статуса на "Failed").

#### Улучшенная версия

```csharp
public async Task ProcessClaimAsync(int claimId)
{
    try
    {
        var claim = await _repository.GetClaimAsync(claimId);

        if (claim == null)
            throw new ClaimNotFoundException($"Claim {claimId} not found");

        ValidateClaim(claim);
        await _paymentService.ProcessPaymentAsync(claim);
        await _repository.UpdateClaimStatusAsync(claimId, "Processed");

        _logger.LogInformation("Claim {ClaimId} processed successfully", claimId);
    }
    catch (ClaimNotFoundException ex)
    {
        _logger.LogWarning(ex, "Claim not found: {ClaimId}", claimId);
        throw;
    }
    catch (PaymentException ex)
    {
        _logger.LogError(ex, "Payment failed for claim {ClaimId}", claimId);
        await _repository.UpdateClaimStatusAsync(claimId, "PaymentFailed");
        throw;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Unexpected error processing claim {ClaimId}", claimId);
        throw;
    }
}
```

Идея: обрабатывать известные типы исключений (логирование + при необходимости обновление статуса), остальное логировать и пробрасывать дальше (`throw`).

---

## 6. SQL (вероятно ~80%)

### Пример 6.1: JOIN с подводными камнями

```sql
SELECT c.CustomerName, p.PolicyNumber, p.Premium, cl.ClaimAmount
FROM Customers c
INNER JOIN Policies p ON c.CustomerId = p.CustomerId
INNER JOIN Claims cl ON p.PolicyId = cl.PolicyId
WHERE c.City = 'Milan'
ORDER BY p.Premium DESC
```

#### Объяснение

- **SELECT** — выбираются имя клиента, номер полиса, премия, сумма претензии.
- **FROM Customers c** — таблица клиентов с алиасом `c`.
- **INNER JOIN Policies p** — только клиенты, у которых есть хотя бы один полис; полисы без клиента не попадут (по смыслу FK обычно не бывает).
- **INNER JOIN Claims cl** — только те полисы, по которым есть хотя бы один claim. Полисы **без** претензий **не попадут** в результат.
- **WHERE c.City = 'Milan'** — только клиенты из Милана.
- **ORDER BY p.Premium DESC** — сортировка по премии по убыванию.

#### Что спросят

- **«Что возвращает запрос?»** — клиенты из Милана, у которых есть полис и хотя бы один claim; в выводе — имя, номер полиса, премия, сумма претензии.
- **«Что если у клиента нет claims?»** — такой клиент/полис **не появится** из-за второго INNER JOIN.
- **«Как оптимизировать?»** — индексы: по `Customers(City, CustomerId)`, по FK `Policies(CustomerId)`, `Claims(PolicyId)`; при необходимости «все полисы, даже без claims» — заменить последний JOIN на **LEFT JOIN** и обрабатывать `NULL` в `cl.ClaimAmount`.

---

## 7. DEPENDENCY INJECTION (вероятно ~65%)

### Пример 7.1: Создание зависимости внутри контроллера

**Плохо:**

```csharp
public class PolicyController : ControllerBase
{
    private readonly PolicyService _policyService;

    public PolicyController()
    {
        _policyService = new PolicyService(); // Tight coupling
    }

    [HttpGet("{id}")]
    public ActionResult<Policy> GetPolicy(int id)
    {
        var policy = _policyService.GetPolicy(id);
        return Ok(policy);
    }
}
```

Проблемы: жёсткая привязка к конкретному классу, нельзя подменить реализацию в тестах, жизненный цикл не управляется контейнером, нарушение Dependency Inversion (зависимость от конкретики).

**Лучше:**

```csharp
public class PolicyController : ControllerBase
{
    private readonly IPolicyService _policyService;
    private readonly ILogger<PolicyController> _logger;

    public PolicyController(
        IPolicyService policyService,
        ILogger<PolicyController> logger)
    {
        _policyService = policyService ?? throw new ArgumentNullException(nameof(policyService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Policy>> GetPolicyAsync(int id)
    {
        try
        {
            var policy = await _policyService.GetPolicyAsync(id);
            if (policy == null)
                return NotFound();
            return Ok(policy);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting policy {PolicyId}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}
```

Контроллер зависит от абстракций (`IPolicyService`, `ILogger`), зависимости приходят извне (DI), их можно мокать в тестах.

---

## 8. N+1 PROBLEM (вероятно ~60%)

### Пример 8.1: N+1 запросов к БД

**Плохо:**

```csharp
public List<PolicyWithCustomer> GetAllPoliciesWithCustomers()
{
    var policies = _context.Policies.ToList();           // 1 query
    var result = new List<PolicyWithCustomer>();

    foreach (var policy in policies)
    {
        var customer = _context.Customers
            .FirstOrDefault(c => c.CustomerId == policy.CustomerId); // N queries!
        result.Add(new PolicyWithCustomer { Policy = policy, Customer = customer });
    }
    return result;
}
```

Сначала один запрос за все полисы, затем в цикле для каждого полиса — отдельный запрос за клиента. Итого: **1 + N** запросов (N+1 problem).

**Исправление (Eager Loading):**

```csharp
public async Task<List<PolicyWithCustomer>> GetAllPoliciesWithCustomersAsync()
{
    return await _context.Policies
        .Include(p => p.Customer)   // One query with JOIN
        .Select(p => new PolicyWithCustomer
        {
            Policy = p,
            Customer = p.Customer
        })
        .AsNoTracking()
        .ToListAsync();
}
```

`.Include(p => p.Customer)` подтягивает связанных клиентов одним запросом (JOIN или отдельный batch). `.AsNoTracking()` — только чтение, без отслеживания сущностей, меньше памяти и быстрее.

---

## Чеклист для подготовки

- [ ] Парсинг CSV (string operations, Split, Trim, TryParse).
- [ ] LINQ: GroupBy, Where, Sum, deferred vs immediate execution.
- [ ] Async/await: последовательно vs параллельно, `Task.WhenAll`.
- [ ] Null: null-coalescing `??`, `ArgumentNullException.ThrowIfNull`, nullable reference types.
- [ ] Exception handling: конкретные типы исключений, логирование, rethrow.
- [ ] SQL: INNER vs LEFT JOIN, индексы по FK и фильтрам.
- [ ] DI: зависимость от абстракций, конструктор, не `new` внутри контроллера.
- [ ] N+1: `Include`, Eager Loading, `AsNoTracking`.

**Стратегия ответа (около 60 секунд на блок):**

1. **Что делает код** (≈10 сек).
2. **Как работает** (≈20 сек) — ключевые методы и поток данных.
3. **Проблемы** (≈15 сек) — null, исключения, производительность, поддерживаемость.
4. **Как улучшить** (≈15 сек) — конкретные приёмы (guard clauses, TryParse, DI, Include и т.д.).

Для детальной работы именно с CSV (методы, парсинг, запись, краевые случаи) см. отдельный файл **csv-operations-csharp-ru.md**.
