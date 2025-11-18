# Extension Methods in C#

## Introduzione

Gli **Extension Methods** (Metodi di Estensione) in C# permettono di aggiungere nuovi metodi a tipi esistenti senza modificare il codice sorgente originale o creare una nuova classe derivata. Questo è particolarmente utile per estendere classi di librerie di terze parti o tipi built-in.

---

## 1. Cos'è un Extension Method?

### Definizione

Un Extension Method è un metodo statico definito in una classe statica che può essere chiamato come se fosse un metodo di istanza del tipo che estende.

### Problema: Non Possiamo Modificare Classi Esistenti

```csharp
// ❌ PROBLEMA: Non possiamo modificare la classe string
public class String {
    // Non possiamo aggiungere metodi qui!
}

// Cosa fare se vogliamo aggiungere funzionalità a string?
// ✅ SOLUZIONE: Extension Methods
```

---

## 2. Sintassi Base

### Struttura di un Extension Method

```csharp
// 1. Classe statica
public static class StringExtensions {
    
    // 2. Metodo statico
    // 3. Primo parametro con 'this'
    public static string Reverse(this string str) {
        char[] chars = str.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }
}

// Utilizzo: chiamato come metodo di istanza
string text = "Hello";
string reversed = text.Reverse(); // ✅ Come se fosse un metodo di string
Console.WriteLine(reversed); // Output: "olleH"
```

### Componenti Richiesti

1. **Classe statica**: La classe contenitore deve essere `static`
2. **Metodo statico**: Il metodo deve essere `static`
3. **Parametro `this`**: Il primo parametro deve essere preceduto da `this`
4. **Namespace**: Deve essere nello stesso namespace o importato con `using`

---

## 3. Esempi Pratici

### Esempio 1: Extension Method per String

```csharp
public static class StringExtensions {
    // Converte la prima lettera in maiuscola
    public static string Capitalize(this string str) {
        if (string.IsNullOrEmpty(str)) {
            return str;
        }
        return char.ToUpper(str[0]) + str.Substring(1).ToLower();
    }
    
    // Verifica se la stringa è un email valida (semplificato)
    public static bool IsValidEmail(this string email) {
        if (string.IsNullOrWhiteSpace(email)) {
            return false;
        }
        return email.Contains("@") && email.Contains(".");
    }
    
    // Rimuove tutti gli spazi
    public static string RemoveSpaces(this string str) {
        return str?.Replace(" ", "") ?? string.Empty;
    }
}

// Utilizzo
string name = "mario rossi";
string capitalized = name.Capitalize(); // "Mario rossi"
bool isValid = "test@example.com".IsValidEmail(); // true
string noSpaces = "hello world".RemoveSpaces(); // "helloworld"
```

### Esempio 2: Extension Method per Collections

```csharp
public static class CollectionExtensions {
    // Verifica se la collezione è vuota o null
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection) {
        return collection == null || !collection.Any();
    }
    
    // Stampa tutti gli elementi
    public static void PrintAll<T>(this IEnumerable<T> collection) {
        foreach (var item in collection) {
            Console.WriteLine(item);
        }
    }
    
    // Converte una lista in una stringa separata da virgole
    public static string ToCommaSeparatedString<T>(this IEnumerable<T> collection) {
        return string.Join(", ", collection);
    }
}

// Utilizzo
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
bool isEmpty = numbers.IsNullOrEmpty(); // false
numbers.PrintAll(); // Stampa tutti i numeri
string result = numbers.ToCommaSeparatedString(); // "1, 2, 3, 4, 5"
```

### Esempio 3: Extension Method per DateTime

```csharp
public static class DateTimeExtensions {
    // Verifica se la data è nel weekend
    public static bool IsWeekend(this DateTime date) {
        return date.DayOfWeek == DayOfWeek.Saturday || 
               date.DayOfWeek == DayOfWeek.Sunday;
    }
    
    // Verifica se la data è un giorno lavorativo
    public static bool IsWeekday(this DateTime date) {
        return !date.IsWeekend();
    }
    
    // Calcola l'età da una data di nascita
    public static int GetAge(this DateTime birthDate) {
        DateTime today = DateTime.Today;
        int age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age)) {
            age--;
        }
        return age;
    }
    
    // Formatta la data in formato italiano
    public static string ToItalianFormat(this DateTime date) {
        return date.ToString("dd/MM/yyyy");
    }
}

// Utilizzo
DateTime today = DateTime.Now;
bool isWeekend = today.IsWeekend();
DateTime birthDate = new DateTime(1990, 5, 15);
int age = birthDate.GetAge();
string italianDate = today.ToItalianFormat(); // "18/11/2025"
```

---

## 4. Extension Methods con Parametri Aggiuntivi

### Metodi con Più Parametri

```csharp
public static class StringExtensions {
    // Ripete una stringa n volte
    public static string Repeat(this string str, int count) {
        if (count <= 0) {
            return string.Empty;
        }
        return string.Concat(Enumerable.Repeat(str, count));
    }
    
    // Sostituisce caratteri multipli
    public static string ReplaceMultiple(this string str, 
        Dictionary<char, char> replacements) {
        foreach (var replacement in replacements) {
            str = str.Replace(replacement.Key, replacement.Value);
        }
        return str;
    }
    
    // Tronca una stringa a una lunghezza specifica
    public static string Truncate(this string str, int maxLength, 
        string suffix = "...") {
        if (string.IsNullOrEmpty(str) || str.Length <= maxLength) {
            return str;
        }
        return str.Substring(0, maxLength - suffix.Length) + suffix;
    }
}

// Utilizzo
string text = "Hello";
string repeated = text.Repeat(3); // "HelloHelloHello"

string longText = "This is a very long text";
string truncated = longText.Truncate(10); // "This is a..."
```

---

## 5. Extension Methods Generici

### Metodi Generici per Qualsiasi Tipo

```csharp
public static class GenericExtensions {
    // Verifica se un valore è compreso tra due valori
    public static bool IsBetween<T>(this T value, T min, T max) 
        where T : IComparable<T> {
        return value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
    }
    
    // Converte un valore in un altro tipo (con fallback)
    public static TResult ConvertTo<TResult>(this object value, 
        TResult defaultValue = default(TResult)) {
        try {
            return (TResult)Convert.ChangeType(value, typeof(TResult));
        } catch {
            return defaultValue;
        }
    }
    
    // Verifica se un valore è null o default
    public static bool IsNullOrDefault<T>(this T value) {
        return EqualityComparer<T>.Default.Equals(value, default(T));
    }
}

// Utilizzo
int number = 5;
bool isBetween = number.IsBetween(1, 10); // true

string str = "123";
int converted = str.ConvertTo<int>(0); // 123
string invalid = "abc";
int defaultVal = invalid.ConvertTo<int>(0); // 0 (default)
```

---

## 6. Extension Methods per LINQ

### Estendere LINQ con Metodi Personalizzati

```csharp
public static class LinqExtensions {
    // Filtra elementi duplicati basati su una chiave
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector) {
        HashSet<TKey> seenKeys = new HashSet<TKey>();
        foreach (TSource element in source) {
            if (seenKeys.Add(keySelector(element))) {
                yield return element;
            }
        }
    }
    
    // Applica un'azione a ogni elemento
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, 
        Action<T> action) {
        foreach (var item in source) {
            action(item);
            yield return item;
        }
    }
    
    // Raggruppa elementi consecutivi
    public static IEnumerable<IEnumerable<T>> GroupConsecutive<T>(
        this IEnumerable<T> source, 
        Func<T, T, bool> predicate) {
        using (var enumerator = source.GetEnumerator()) {
            if (!enumerator.MoveNext()) {
                yield break;
            }
            
            var currentGroup = new List<T> { enumerator.Current };
            
            while (enumerator.MoveNext()) {
                if (predicate(currentGroup.Last(), enumerator.Current)) {
                    currentGroup.Add(enumerator.Current);
                } else {
                    yield return currentGroup;
                    currentGroup = new List<T> { enumerator.Current };
                }
            }
            
            yield return currentGroup;
        }
    }
}

// Utilizzo
var people = new[] {
    new { Name = "Mario", Age = 30 },
    new { Name = "Luigi", Age = 30 },
    new { Name = "Peach", Age = 25 }
};

// Distinct per età
var distinctAges = people.DistinctBy(p => p.Age);

// ForEach
numbers.ForEach(n => Console.WriteLine(n * 2));
```

---

## 7. Best Practices

### ✅ Quando Usare Extension Methods

1. **Estendere classi di librerie di terze parti**
2. **Aggiungere funzionalità a tipi built-in** (string, int, DateTime, ecc.)
3. **Creare API più fluide e leggibili**
4. **Aggiungere metodi di utilità senza modificare classi esistenti**

```csharp
// ✅ Buon uso: estendere funzionalità esistenti
public static class StringExtensions {
    public static bool IsNullOrWhiteSpace(this string str) {
        // Aggiunge funzionalità a string
    }
}
```

### ❌ Quando NON Usare

1. **Quando puoi modificare direttamente la classe originale**
2. **Per metodi che dovrebbero essere parte della classe**
3. **Quando crea confusione o ambiguità**
4. **Per metodi che cambiano lo stato interno** (meglio metodi di istanza)

```csharp
// ❌ Cattivo uso: meglio aggiungere direttamente alla classe
public class User {
    // Meglio aggiungere qui invece di extension method
    public void UpdateProfile() {
        // ...
    }
}
```

### Convenzioni di Naming

```csharp
// ✅ Buona convenzione: NomeClasse + Extensions
public static class StringExtensions { }
public static class DateTimeExtensions { }
public static class CollectionExtensions { }

// ✅ Buona convenzione: namespace dedicato
namespace MyProject.Extensions {
    public static class StringExtensions { }
}
```

---

## 8. Namespace e Organizzazione

### Organizzazione Consigliata

```csharp
// File: Extensions/StringExtensions.cs
namespace MyProject.Extensions {
    public static class StringExtensions {
        public static string Reverse(this string str) {
            // ...
        }
    }
}

// File: Extensions/DateTimeExtensions.cs
namespace MyProject.Extensions {
    public static class DateTimeExtensions {
        public static bool IsWeekend(this DateTime date) {
            // ...
        }
    }
}

// File: Program.cs
using MyProject.Extensions; // Importa tutti gli extension methods

string text = "Hello";
string reversed = text.Reverse(); // ✅ Disponibile dopo using
```

---

## 9. Limitazioni degli Extension Methods

### Cosa NON Possono Fare

```csharp
// ❌ Non possono accedere a membri privati/protetti
public static class StringExtensions {
    public static void AccessPrivate(this string str) {
        // Non possiamo accedere a campi privati di string
    }
}

// ❌ Non possono essere virtuali o astratti
public static class BaseExtensions {
    // public virtual void Method() { } // ERRORE
}

// ❌ Non possono essere definiti in classi non statiche
public class NonStaticClass {
    // public static void Method(this string str) { } // ERRORE
}

// ❌ Non possono sovrascrivere metodi esistenti
public static class StringExtensions {
    // public static int Length(this string str) { } // ERRORE: Length esiste già
}
```

---

## 10. Esempi Avanzati

### Esempio: Fluent API con Extension Methods

```csharp
public static class FluentExtensions {
    public static StringBuilder AppendLineIf(this StringBuilder sb, 
        bool condition, string value) {
        if (condition) {
            sb.AppendLine(value);
        }
        return sb;
    }
    
    public static StringBuilder AppendFormatted(this StringBuilder sb, 
        string format, params object[] args) {
        sb.AppendFormat(format, args);
        return sb;
    }
}

// Utilizzo: API fluida
var sb = new StringBuilder();
sb.AppendLine("Header")
  .AppendLineIf(condition, "Conditional line")
  .AppendFormatted("Value: {0}", 42);
```

### Esempio: Extension Methods per Nullable

```csharp
public static class NullableExtensions {
    // Valore di default personalizzato
    public static T ValueOr<T>(this T? nullable, T defaultValue) 
        where T : struct {
        return nullable ?? defaultValue;
    }
    
    // Esegue un'azione se non null
    public static void IfNotNull<T>(this T? nullable, Action<T> action) 
        where T : struct {
        if (nullable.HasValue) {
            action(nullable.Value);
        }
    }
}

// Utilizzo
int? number = null;
int value = number.ValueOr(10); // 10

number = 5;
number.IfNotNull(n => Console.WriteLine($"Value: {n}"));
```

---

## 11. Domande Frequenti (FAQ)

### Q: Gli extension methods sono più lenti dei metodi normali?
**R:** No, le performance sono identiche. Il compilatore converte gli extension methods in chiamate statiche normali.

### Q: Posso creare extension methods per interfacce?
**R:** Sì! Questo è molto utile:

```csharp
public static class IEnumerableExtensions {
    public static void PrintAll<T>(this IEnumerable<T> source) {
        foreach (var item in source) {
            Console.WriteLine(item);
        }
    }
}

// Funziona con List, Array, HashSet, ecc.
List<int> list = new List<int>();
int[] array = new int[10];
list.PrintAll(); // ✅
array.PrintAll(); // ✅
```

### Q: Cosa succede se due extension methods hanno lo stesso nome?
**R:** Se sono nello stesso namespace, c'è ambiguità. Devi specificare esplicitamente quale usare o rimuovere uno dei `using`.

### Q: Posso creare extension properties?
**R:** No, solo metodi. Ma puoi creare metodi che sembrano proprietà:

```csharp
public static string Reversed(this string str) {
    return new string(str.Reverse().ToArray());
}

string text = "Hello";
string reversed = text.Reversed(); // Sembra una proprietà
```

---

## 12. Esercizi Pratici

### Esercizio 1: Extension Method per String

```csharp
// Crea un extension method che conta le parole in una stringa
public static int WordCount(this string str) {
    // La tua implementazione qui
}
```

### Esercizio 2: Extension Method per List

```csharp
// Crea un extension method che rimuove duplicati mantenendo l'ordine
public static List<T> RemoveDuplicates<T>(this List<T> list) {
    // La tua implementazione qui
}
```

### Esercizio 3: Extension Method per DateTime

```csharp
// Crea un extension method che calcola i giorni lavorativi tra due date
public static int WorkingDaysBetween(this DateTime start, DateTime end) {
    // La tua implementazione qui
}
```

---

## Conclusioni

Gli Extension Methods in C# sono potenti strumenti che permettono di:

- ✅ **Estendere tipi esistenti** senza modificarli
- ✅ **Creare API più fluide** e leggibili
- ✅ **Aggiungere funzionalità** a librerie di terze parti
- ✅ **Organizzare codice** in modo modulare

**Ricorda:**
- La classe deve essere `static`
- Il metodo deve essere `static`
- Il primo parametro deve avere `this`
- Importa il namespace con `using` per usarli
- Non possono accedere a membri privati

---

*Documento creato per spiegare gli Extension Methods in C# con esempi pratici e best practices.*

