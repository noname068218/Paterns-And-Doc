# Работа с CSV в C#: методы, парсинг, запись, краевые случаи

Отдельное руководство по созданию, чтению и разбору CSV в C#: какие методы использовать, зачем они нужны, как обрабатывать кавычки, запятые внутри полей и ошибки данных.

---

## 1. Что такое CSV и зачем он нужен

**CSV (Comma-Separated Values)** — текстовый формат, в котором:

- Строки — это записи (records).
- Поля внутри строки разделены разделителем (чаще всего запятая `,`).
- Первая строка часто — заголовок (имена колонок).

**Зачем нужен в коде:**

- Импорт/экспорт данных (полисы, клиенты, отчёты).
- Обмен с внешними системами и Excel.
- Простой дамп таблиц без СУБД.

**Особенности, которые нужно учитывать:**

- Разделитель может быть `,`, `;` или `\t` (TSV).
- Поля могут быть в кавычках; внутри кавычек запятая не разделяет поля.
- Переносы строк внутри кавычек — часть значения поля.
- Кодировка (часто UTF-8 или Windows-1252).

---

## 2. Базовые методы string для разбора CSV

Перед использованием специализированных библиотек полезно понимать, как разбирать простой CSV вручную.

### 2.1. Split — разбиение строки по разделителю

```csharp
string line = "P001,John Doe,100.50,2024-01-15";
string[] fields = line.Split(',');
// fields[0] = "P001"
// fields[1] = "John Doe"
// fields[2] = "100.50"
// fields[3] = "2024-01-15"
```

| Метод | Назначение |
|-------|------------|
| `string.Split(char)` | Разбивает строку по одному символу. Возвращает `string[]`. Пустые подстроки между двумя запятыми дают пустой элемент. |
| `string.Split(char[]?)` | Несколько разделителей (например `new[] { ',', ';' }`). |
| `string.Split(char, StringSplitOptions)` | Второй параметр — например `StringSplitOptions.RemoveEmptyEntries` убирает пустые элементы. |
| `string.Split(string?, StringSplitOptions)` | Разделитель — строка (например `"||"`). |

**Ограничение простого Split:** не понимает кавычки. Строка `"Doe, John",100` разобьётся на 3 части, а не на 2.

### 2.2. Trim — убирание пробелов по краям

```csharp
string raw = "  P001  ";
string cleaned = raw.Trim();  // "P001"
```

Зачем: в CSV часто бывают пробелы после запятой или в конце строки. `Trim()` убирает пробелы (и при необходимости другие символы) с начала и конца строки. Для каждого поля после Split обычно вызывают `field.Trim()`.

### 2.3. Substring и индексы — извлечение частей

Для фиксированного формата (например, номер полиса всегда в первых 10 символах):

```csharp
string line = "P001      ,John Doe";
string policyPart = line.Substring(0, 10).Trim();  // "P001"
// C# 8+ range:
string policyPart2 = line[..10].Trim();
```

В типичном CSV с разделителями используют Split, а не Substring.

### 2.4. Проверка на пустоту и null

```csharp
if (string.IsNullOrEmpty(csvContent))  return;
if (string.IsNullOrWhiteSpace(csvContent))  return;
```

| Метод | Назначение |
|-------|------------|
| `string.IsNullOrEmpty(s)` | `true`, если `s == null` или `s == ""`. |
| `string.IsNullOrWhiteSpace(s)` | `true`, если `s == null`, пустая или состоит только из пробелов/табуляций и т.п. |

Для входного CSV обычно используют `IsNullOrWhiteSpace`, чтобы отбросить и «пустые» строки из файла.

### 2.5. Замена символов

```csharp
string clean = input.Replace("-", "").Replace(" ", "");
```

Удаление лишних символов из одного поля (например, из номера полиса). Для целой строки CSV не заменяют запятые внутри кавычек — нужен посимвольный разбор или парсер.

---

## 3. Чтение CSV из строки (построчно)

Типичный поток: одна большая строка с переносами (например, считанная из файла) или построчное чтение.

### 3.1. Разбиение по строкам

```csharp
string csvContent = File.ReadAllText("policies.csv");
string[] lines = csvContent.Split('\n');

// Or with options:
string[] lines2 = csvContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
```

Почему два варианта:

- В Windows конец строки часто `\r\n`. Один `Split('\n')` оставит `\r` в конце каждой строки — тогда нужен `Trim()` или `Replace("\r", "")`.
- `StringSplitOptions.RemoveEmptyEntries` убирает пустые строки (например, от двойного `\n\n`).

### 3.2. Пропуск заголовка

```csharp
var dataLines = lines.Skip(1);
foreach (var line in dataLines)
{
    // process line
}
```

`Skip(1)` пропускает первую строку (заголовок). Итерация идёт только по строкам с данными.

### 3.3. Полный пример: простой парсер без кавычек

```csharp
/// <summary>
/// Parses CSV content without quoted fields. Use only when data has no commas inside values.
/// </summary>
public static List<PolicyData> ParseSimpleCsv(string csvContent)
{
    if (string.IsNullOrWhiteSpace(csvContent))
        return new List<PolicyData>();

    var list = new List<PolicyData>();
    var lines = csvContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);

    foreach (var line in lines.Skip(1))
    {
        var fields = line.Split(',').Select(f => f.Trim()).ToArray();
        if (fields.Length < 4) continue;

        if (decimal.TryParse(fields[2], out var premium) &&
            DateTime.TryParse(fields[3], out var startDate))
        {
            list.Add(new PolicyData
            {
                PolicyNumber = fields[0],
                CustomerName = fields[1],
                Premium = premium,
                StartDate = startDate
            });
        }
    }
    return list;
}
```

Построчно:

- Проверка входа; при пустом — возврат пустого списка.
- Разбиение по `\n` с удалением пустых строк.
- Пропуск первой строки (заголовок).
- Разбиение каждой строки по `,`, Trim по каждому полю.
- Проверка количества полей; пропуск неполных строк.
- Безопасный разбор числа и даты через `TryParse`; только при успехе создаётся и добавляется объект.

---

## 4. Чтение CSV из файла (StreamReader)

Для больших файлов не загружают весь текст в память, а читают по строкам.

```csharp
using var reader = new StreamReader("policies.csv");
string? header = await reader.ReadLineAsync();  // optional: validate header

while (await reader.ReadLineAsync() is { } line)
{
    var fields = line.Split(',');
    // process fields
}
```

| Метод | Назначение |
|-------|------------|
| `StreamReader(path)` | Открывает файл для чтения в заданной кодировке (по умолчанию UTF-8). |
| `ReadLineAsync()` | Читает одну строку (до `\n` или конца потока). Возвращает `null` в конце файла. |
| `using` | Гарантирует вызов `Dispose` (закрытие файла) после выхода из блока. |

Так можно обрабатывать гигабайты CSV без загрузки всего в память.

---

## 5. Парсинг полей в кавычках (запятые внутри значения)

Если значение содержит запятую, в CSV его заключают в кавычки:

```text
PolicyNumber,CustomerName,Premium,StartDate
P001,"Doe, John",100.50,2024-01-15
```

Простой `Split(',')` разобьёт `"Doe, John"` на два поля. Нужен посимвольный разбор или готовая библиотека.

### 5.1. Ручной разбор одной строки с кавычками

Идея: идти по символам; внутри пары кавычек запятая не считается разделителем; кавычка внутри кавычек часто экранируется удвоением `""`.

```csharp
/// <summary>
/// Splits a CSV line respecting double-quoted fields. Handles "" as escaped quote inside field.
/// </summary>
public static List<string> SplitCsvLine(string line)
{
    var fields = new List<string>();
    var current = new StringBuilder();
    bool inQuotes = false;

    for (int i = 0; i < line.Length; i++)
    {
        char c = line[i];

        if (c == '"')
        {
            if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
            {
                current.Append('"');
                i++;
            }
            else
            {
                inQuotes = !inQuotes;
            }
        }
        else if (c == ',' && !inQuotes)
        {
            fields.Add(current.ToString().Trim());
            current.Clear();
        }
        else
        {
            current.Append(c);
        }
    }
    fields.Add(current.ToString().Trim());
    return fields;
}
```

Кратко по логике:

- `"` переключает режим «в кавычках» или закрывает поле; `""` внутри кавычек даёт одну кавычку в значении.
- Запятая считается разделителем только когда мы не внутри кавычек.
- В конце добавляется последнее накопленное поле.

Такой метод нужен, когда вы пишете парсер сами и хотите корректно обрабатывать кавычки и запятые внутри полей.

---

## 6. Запись CSV (формирование строки и запись в файл)

### 6.1. Экранирование поля (кавычки и переносы)

Правило: если поле содержит запятую, кавычки или перенос строки — всё поле оборачивают в кавычки; кавычка внутри поля удваивается.

```csharp
public static string EscapeCsvField(string? field)
{
    if (field == null) return "";
    if (field.Contains(',') || field.Contains('"') || field.Contains('\n') || field.Contains('\r'))
    {
        return "\"" + field.Replace("\"", "\"\"") + "\"";
    }
    return field;
}
```

Использование при сборке строки:

```csharp
string line = string.Join(",", values.Select(EscapeCsvField));
```

### 6.2. Формирование строки с заголовком и строками данных

```csharp
var headers = new[] { "PolicyNumber", "CustomerName", "Premium", "StartDate" };
var rows = new List<string[]> { headers };

foreach (var p in policies)
{
    rows.Add(new[]
    {
        p.PolicyNumber,
        p.CustomerName,
        p.Premium.ToString(CultureInfo.InvariantCulture),
        p.StartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
    });
}

string csv = string.Join(Environment.NewLine,
    rows.Select(row => string.Join(",", row.Select(EscapeCsvField))));
```

Важно:

- Числа и даты лучше форматировать с `CultureInfo.InvariantCulture`, чтобы запятая/точка не зависели от локали.
- `Environment.NewLine` — перенос строки для текущей ОС (`\r\n` / `\n`).

### 6.3. Запись в файл

```csharp
await File.WriteAllTextAsync("output.csv", csv, Encoding.UTF8);
// Or line by line:
await using var writer = new StreamWriter("output.csv", append: false, Encoding.UTF8);
await writer.WriteLineAsync(string.Join(",", headers.Select(EscapeCsvField)));
foreach (var row in dataRows)
    await writer.WriteLineAsync(string.Join(",", row.Select(EscapeCsvField)));
```

Для очень больших объёмов предпочтительно писать по строкам через `StreamWriter`, чтобы не держать весь CSV в памяти.

---

## 7. Готовые библиотеки (кратко)

Для продакшена часто используют готовый парсер/писатель CSV.

| Библиотека | Назначение |
|------------|------------|
| **CsvHelper** (NuGet) | Чтение/запись через маппинг на классы, настройка разделителя, кавычек, культуры. |
| **Microsoft.VisualBasic.FileIO.TextFieldParser** | Встроено в .NET (из VB), умеет кавычки и разделители. |

Пример с CsvHelper (чтение):

```csharp
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

var config = new CsvConfiguration(CultureInfo.InvariantCulture)
{
    HasHeaderRecord = true,
    MissingFieldFound = null,
    BadDataFound = null,
};
using var reader = new StringReader(csvContent);
using var csv = new CsvReader(reader, config);
var records = csv.GetRecords<PolicyData>().ToList();
```

Класс `PolicyData` должен иметь свойства с именами как в заголовке (или маппинг через атрибуты/конфигурацию).

---

## 8. Типичные ошибки и как их избежать

| Ошибка | Причина | Решение |
|--------|--------|--------|
| NullReferenceException при разборе | `csvContent` или строка из файла null | В начале метода: `if (string.IsNullOrWhiteSpace(csvContent)) return ...;` или throw. |
| IndexOutOfRangeException | Обращение к `fields[i]` при малом числе полей | Проверять `fields.Length >= requiredColumns` перед доступом по индексу. |
| FormatException при Parse | Невалидное число или дата | Использовать `TryParse` и при `false` пропускать строку или логировать. |
| Неверные числа/даты из-за локали | `Parse` использует текущую культуру (запятая/точка) | Использовать `CultureInfo.InvariantCulture` в `Parse`/`ToString` или `TryParse(..., CultureInfo.InvariantCulture, ...)`. |
| Запятая внутри поля разбивает поле | Простой Split не учитывает кавычки | Парсить с учётом кавычек (ручной разбор или CsvHelper). |
| Лишний `\r` в полях | В Windows строки заканчиваются `\r\n` | После Split по `\n` вызывать `Trim()` у полей или у строки; или Split по `['\r','\n']`. |
| Огромное потребление памяти | Весь файл в строке | Читать построчно через `StreamReader` и обрабатывать по одной строке. |

---

## 9. Минимальный класс данных для примеров

```csharp
public class PolicyData
{
    public string PolicyNumber { get; set; } = "";
    public string CustomerName { get; set; } = "";
    public decimal Premium { get; set; }
    public DateTime StartDate { get; set; }
}
```

Используется в примерах парсинга выше; в реальном коде можно заменить на свой DTO или сущность.

---

## 10. Краткий чеклист: создание и работа с CSV

- **Чтение из строки:** `Split('\n'[, StringSplitOptions.RemoveEmptyEntries])`, затем по каждой строке `Split(',')` и при необходимости ручной разбор с кавычками.
- **Чтение из файла:** `StreamReader` + `ReadLineAsync()` для больших файлов.
- **Заголовок:** пропускать первой строкой (`Skip(1)`) или явно проверять имена колонок.
- **Поля:** после Split вызывать `Trim()`; для чисел и дат — `TryParse` с нужной культурой.
- **Запись:** экранировать поля с запятой/кавычками/переносами; числа и даты — InvariantCulture; писать по строкам при больших объёмах.
- **Надёжность:** проверка null/пустоты входа, проверка длины `fields`, обработка невалидных строк (пропуск или логирование).

Этого набора методов и правил достаточно, чтобы осознанно создавать и обрабатывать CSV в C# и уверенно отвечать на вопросы про строковые операции и парсинг на собеседовании. Для сложных форматов и производительности удобно подключать CsvHelper или аналог.
