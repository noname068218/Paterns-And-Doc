# Методы работы со строками в C #

## Обзор

C# предоставляет множество встроенных методов для работы со строками. Все эти методы являются частью класса `string` и статического класса `String`.

---

## Основные методы манипуляции строками

### Concat - Объединение строк

**Описание:** Объединяет несколько строк в одну строку.

**Когда использовать:**

- Когда нужно соединить несколько строк
- При формировании сообщений из нескольких частей
- Для построения путей или URL

**Примеры:**

```csharp
// Статический метод Concat
string result = string.Concat("Hello", " ", "World"); // "Hello World"
string result2 = string.Concat("A", "B", "C"); // "ABC"

// С помощью оператора +
string result3 = "Hello" + " " + "World"; // "Hello World"

// С помощью string interpolation
string name = "John";
string greeting = $"Hello, {name}!"; // "Hello, John!"

// С несколькими строками
string[] words = { "The", "quick", "brown", "fox" };
string sentence = string.Concat(words); // "Thequickbrownfox"
string sentence2 = string.Join(" ", words); // "The quick brown fox"
```

---

### Repeat - Повторение строки

**Описание:** Повторяет строку указанное количество раз. (Доступно с C# 12)

**Когда использовать:**

- Для создания разделителей
- Для генерации повторяющихся паттернов
- Для заполнения строки символами

**Примеры:**

```csharp
// C# 12: String.Repeat
string repeated = "abc".Repeat(3); // "abcabcabc"
string separator = "-".Repeat(10); // "----------"
string padding = " ".Repeat(5); // "     "

// Альтернатива для старых версий C#
string repeated2 = new string('-', 10); // "----------"
string repeated3 = string.Concat(Enumerable.Repeat("abc", 3)); // "abcabcabc"
```

---

### Substring - Извлечение подстроки

**Описание:** Извлекает часть строки, начиная с указанного индекса.

**Когда использовать:**

- Для извлечения части строки
- Для парсинга данных
- Для обработки текста

**Примеры:**

```csharp
string text = "Hello World";

// Начиная с индекса до конца
string substring1 = text.Substring(6); // "World"

// Начиная с индекса, указанная длина
string substring2 = text.Substring(0, 5); // "Hello"
string substring3 = text.Substring(6, 5); // "World"

// С помощью Range (C# 8+)
string substring4 = text[6..]; // "World"
string substring5 = text[0..5]; // "Hello"
string substring6 = text[6..11]; // "World"
```

---

### Replace - Замена символов или подстрок

**Описание:** Заменяет все вхождения указанной подстроки на другую.

**Когда использовать:**

- Для замены текста в строке
- Для очистки данных
- Для форматирования строк

**Примеры:**

```csharp
string text = "Hello World";

// Замена подстроки
string replaced1 = text.Replace("World", "C#"); // "Hello C#"
string replaced2 = text.Replace("o", "0"); // "Hell0 W0rld"

// Замена всех вхождений
string text2 = "abc abc abc";
string replaced3 = text2.Replace("abc", "xyz"); // "xyz xyz xyz"

// Цепочка замен
string result = text.Replace("Hello", "Hi").Replace("World", "User"); // "Hi User"
```

---

### Split - Разделение строки на массив

**Описание:** Разделяет строку на массив подстрок по указанному разделителю.

**Когда использовать:**

- Для парсинга CSV данных
- Для разделения предложений на слова
- Для обработки входных данных

**Примеры:**

```csharp
string text = "apple,banana,cherry";

// Разделение по запятой
string[] fruits = text.Split(','); // ["apple", "banana", "cherry"]

// Разделение по пробелу
string sentence = "The quick brown fox";
string[] words = sentence.Split(' '); // ["The", "quick", "brown", "fox"]

// Разделение с удалением пустых записей
string text2 = "a,,b,c";
string[] parts = text2.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries); // ["a", "b", "c"]

// Разделение по нескольким разделителям
string text3 = "a,b;c.d";
string[] parts2 = text3.Split(new char[] { ',', ';', '.' }); // ["a", "b", "c", "d"]
```

---

### Join - Объединение массива в строку

**Описание:** Объединяет элементы массива или коллекции в одну строку с указанным разделителем.

**Когда использовать:**

- Для создания CSV строк
- Для форматирования списков
- Для построения путей или URL

**Примеры:**

```csharp
string[] fruits = { "apple", "banana", "cherry" };

// Объединение с разделителем
string result = string.Join(", ", fruits); // "apple, banana, cherry"

// Объединение без разделителя
string result2 = string.Join("", fruits); // "applebananacherry"

// Объединение с другим разделителем
string result3 = string.Join(" - ", fruits); // "apple - banana - cherry"

// Объединение List
List<int> numbers = new List<int> { 1, 2, 3 };
string numbersStr = string.Join("|", numbers); // "1|2|3"
```

---

### Trim - Удаление пробелов

**Описание:** Удаляет все пробельные символы в начале и конце строки.

**Когда использовать:**

- Для очистки пользовательского ввода
- Для обработки данных из файлов
- Для нормализации строк

**Примеры:**

```csharp
string text = "  Hello World  ";

// Удаление пробелов с обеих сторон
string trimmed = text.Trim(); // "Hello World"

// Удаление только слева
string trimmedLeft = text.TrimStart(); // "Hello World  "

// Удаление только справа
string trimmedRight = text.TrimEnd(); // "  Hello World"

// Удаление конкретных символов
string text2 = "###Hello###";
string trimmed2 = text2.Trim('#'); // "Hello"
```

---

### Contains - Проверка наличия подстроки

**Описание:** Проверяет, содержится ли указанная подстрока в строке.

**Когда использовать:**

- Для проверки наличия текста
- Для фильтрации данных
- Для валидации

**Примеры:**

```csharp
string text = "Hello World";

bool contains1 = text.Contains("World"); // true
bool contains2 = text.Contains("world"); // false (регистр важен)

// Без учета регистра
bool contains3 = text.Contains("world", StringComparison.OrdinalIgnoreCase); // true

// Проверка нескольких подстрок
bool containsAny = text.Contains("Hello") || text.Contains("Hi"); // true
```

---

### StartsWith - Проверка начала строки

**Описание:** Проверяет, начинается ли строка с указанной подстроки.

**Когда использовать:**

- Для проверки префиксов
- Для валидации форматов
- Для фильтрации по началу строки

**Примеры:**

```csharp
string text = "Hello World";

bool starts1 = text.StartsWith("Hello"); // true
bool starts2 = text.StartsWith("hello"); // false

// Без учета регистра
bool starts3 = text.StartsWith("hello", StringComparison.OrdinalIgnoreCase); // true

// Проверка с индексом начала поиска
bool starts4 = text.StartsWith("World", 6); // true (начинается с индекса 6)
```

---

### EndsWith - Проверка конца строки

**Описание:** Проверяет, заканчивается ли строка указанной подстрокой.

**Когда использовать:**

- Для проверки расширений файлов
- Для валидации суффиксов
- Для фильтрации по концу строки

**Примеры:**

```csharp
string filename = "document.pdf";

bool ends1 = filename.EndsWith(".pdf"); // true
bool ends2 = filename.EndsWith(".PDF"); // false

// Без учета регистра
bool ends3 = filename.EndsWith(".PDF", StringComparison.OrdinalIgnoreCase); // true

// Проверка расширений
string[] extensions = { ".pdf", ".doc", ".txt" };
bool hasValidExtension = extensions.Any(ext => filename.EndsWith(ext)); // true
```

---

### IndexOf - Поиск индекса подстроки

**Описание:** Возвращает индекс первого вхождения указанной подстроки.

**Когда использовать:**

- Для поиска позиции текста
- Для извлечения части строки
- Для парсинга данных

**Примеры:**

```csharp
string text = "Hello World";

int index1 = text.IndexOf("World"); // 6
int index2 = text.IndexOf("o"); // 4 (первое вхождение)
int index3 = text.IndexOf("xyz"); // -1 (не найдено)

// Поиск с указанного индекса
int index4 = text.IndexOf("o", 5); // 7 (второе вхождение "o")

// Последнее вхождение
int lastIndex = text.LastIndexOf("o"); // 7
```

---

### Insert - Вставка подстроки

**Описание:** Вставляет указанную строку в позицию с указанным индексом.

**Когда использовать:**

- Для модификации строк
- Для добавления текста в определенную позицию
- Для форматирования

**Примеры:**

```csharp
string text = "Hello World";

// Вставка в позицию
string inserted = text.Insert(5, ","); // "Hello, World"

// Вставка в начало
string inserted2 = text.Insert(0, "Say: "); // "Say: Hello World"

// Вставка в конец (аналогично конкатенации)
string inserted3 = text.Insert(text.Length, "!"); // "Hello World!"
```

---

### Remove - Удаление символов

**Описание:** Удаляет указанное количество символов, начиная с указанной позиции.

**Когда использовать:**

- Для удаления части строки
- Для очистки данных
- Для обрезки строк

**Примеры:**

```csharp
string text = "Hello World";

// Удаление от индекса до конца
string removed1 = text.Remove(5); // "Hello"

// Удаление указанного количества символов
string removed2 = text.Remove(5, 6); // "Hello" (удалили " World")
string removed3 = text.Remove(0, 6); // "World" (удалили "Hello ")
```

---

### ToUpper / ToLower - Изменение регистра

**Описание:** Преобразует все символы строки в верхний или нижний регистр.

**Когда использовать:**

- Для нормализации данных
- Для сравнения без учета регистра
- Для форматирования вывода

**Примеры:**

```csharp
string text = "Hello World";

string upper = text.ToUpper(); // "HELLO WORLD"
string lower = text.ToLower(); // "hello world"

// Для конкретной культуры
string upperInvariant = text.ToUpperInvariant(); // "HELLO WORLD"
string lowerInvariant = text.ToLowerInvariant(); // "hello world"

// Для сравнения
bool equalsIgnoreCase = text.ToUpper() == "HELLO WORLD"; // true
```

---

### PadLeft / PadRight - Заполнение строки

**Описание:** Заполняет строку указанным символом до указанной длины слева или справа.

**Когда использовать:**

- Для форматирования чисел
- Для выравнивания текста
- Для создания фиксированной ширины

**Примеры:**

```csharp
string number = "42";

// Заполнение слева (по умолчанию пробел)
string paddedLeft = number.PadLeft(5); // "   42"
string paddedLeft2 = number.PadLeft(5, '0'); // "00042"

// Заполнение справа
string paddedRight = number.PadRight(5); // "42   "
string paddedRight2 = number.PadRight(5, '0'); // "42000"

// Для форматирования чисел
int value = 42;
string formatted = value.ToString().PadLeft(4, '0'); // "0042"
```

---

### Compare - Сравнение строк

**Описание:** Сравнивает две строки и возвращает целое число, указывающее их относительное положение в порядке сортировки.

**Когда использовать:**

- Для сортировки строк
- Для сравнения без учета регистра
- Для кастомной логики сравнения

**Примеры:**
//
```csharp
string str1 = "apple";
string str2 = "banana";

// Сравнение (0 = равны, <0 = str1 меньше, >0 = str1 больше)
int result1 = string.Compare(str1, str2); // отрицательное число
int result2 = string.Compare(str2, str1); // положительное число
int result3 = string.Compare(str1, str1); // 0

// Без учета регистра
int result4 = string.Compare("Apple", "apple", StringComparison.OrdinalIgnoreCase); // 0

// Использование в сортировке
string[] fruits = { "banana", "apple", "cherry" };
Array.Sort(fruits, string.Compare); // ["apple", "banana", "cherry"]
```

---

### Format - Форматирование строк

**Описание:** Заменяет элементы формата в строке строковым представлением указанных объектов.

**Когда использовать:**

- Для создания форматированных строк
- Для подстановки значений
- Для вывода с форматированием

**Примеры:**

```csharp
string name = "John";
int age = 30;

// String.Format
string formatted1 = string.Format("Name: {0}, Age: {1}", name, age); // "Name: John, Age: 30"

// String interpolation (рекомендуется)
string formatted2 = $"Name: {name}, Age: {age}"; // "Name: John, Age: 30"

// С форматированием чисел
double price = 19.99;
string formatted3 = $"Price: {price:C2}"; // "Price: $19.99"
string formatted4 = $"Price: {price:F2}"; // "Price: 19.99"

// С выравниванием
string formatted5 = $"{name,-10} {age,5}"; // "John       30"
```

---

### IsNullOrEmpty / IsNullOrWhiteSpace - Проверка пустоты

**Описание:** Проверяет, является ли строка null, пустой или содержит только пробельные символы.

**Когда использовать:**

- Для валидации входных данных
- Для проверки перед обработкой
- Для безопасной работы со строками

**Примеры:**

```csharp
string str1 = null;
string str2 = "";
string str3 = "   ";
string str4 = "Hello";

bool check1 = string.IsNullOrEmpty(str1); // true
bool check2 = string.IsNullOrEmpty(str2); // true
bool check3 = string.IsNullOrEmpty(str4); // false

bool check4 = string.IsNullOrWhiteSpace(str1); // true
bool check5 = string.IsNullOrWhiteSpace(str2); // true
bool check6 = string.IsNullOrWhiteSpace(str3); // true
bool check7 = string.IsNullOrWhiteSpace(str4); // false

// Безопасная проверка перед обработкой
if (!string.IsNullOrWhiteSpace(str4))
{
    string upper = str4.ToUpper(); // Безопасно
}
```

---

## Дополнительные полезные методы

### ToCharArray - Преобразование в массив символов

```csharp
string text = "Hello";
char[] chars = text.ToCharArray(); // ['H', 'e', 'l', 'l', 'o']
```

### Equals - Сравнение строк

```csharp
string str1 = "Hello";
string str2 = "hello";

bool equals1 = str1.Equals(str2); // false
bool equals2 = str1.Equals(str2, StringComparison.OrdinalIgnoreCase); // true
```

### Copy - Копирование строки

```csharp
string original = "Hello";
string copy = string.Copy(original); // Создает копию
```

### Clone - Клонирование строки

```csharp
string original = "Hello";
string clone = (string)original.Clone(); // Возвращает ссылку на ту же строку
```

---

## Лучшие практики

1. **Используйте string interpolation** вместо `string.Format` для читаемости
2. **Используйте `string.IsNullOrWhiteSpace`** вместо проверки на null и пустоту отдельно
3. **Используйте `StringComparison.OrdinalIgnoreCase`** для сравнения без учета регистра
4. **Используйте `StringBuilder`** для множественных операций со строками
5. **Используйте `string.Join`** вместо циклов для объединения коллекций
6. **Избегайте множественных вызовов `Replace`** - используйте `Regex.Replace` для сложных замен
