# Nullable Types in C#

## Introduzione

I **Nullable Types** in C# permettono di assegnare il valore `null` a variabili di tipo valore (value types). Questo è essenziale per rappresentare l'assenza di un valore, specialmente quando si lavora con database o API che possono restituire valori null.

---

## 1. Il Problema: Value Types e Null

### Problema con Value Types Standard

```csharp
// ❌ PROBLEMA: int non può essere null
int age = null; // ERRORE DI COMPILAZIONE!

// Cosa fare quando un valore potrebbe non esistere?
// Soluzioni problematiche:
int age = -1; // Usare -1 come "valore sentinella" (cattiva pratica)
int? age = null; // ✅ SOLUZIONE: Nullable type
```

**Problemi senza Nullable Types:**
- Impossibile rappresentare "valore mancante" per value types
- Necessità di usare valori sentinella (magic numbers)
- Difficoltà nel lavorare con database (dove i campi possono essere NULL)

---

## 2. Sintassi dei Nullable Types

### Dichiarazione

```csharp
// Sintassi 1: Usando il modificatore ?
int? nullableInt = null;
double? nullableDouble = null;
bool? nullableBool = null;
DateTime? nullableDate = null;

// Sintassi 2: Usando Nullable<T> (equivalente)
Nullable<int> nullableInt2 = null;
Nullable<double> nullableDouble2 = null;

// Entrambe le sintassi sono equivalenti
int? value1 = null;
Nullable<int> value2 = null; // Stesso tipo
```

### Caratteristiche

1. **Sintassi abbreviata**: `T?` è equivalente a `Nullable<T>`
2. **Solo per value types**: Non puoi usare `string?` (string è già nullable)
3. **Valore di default**: `null`
4. **HasValue e Value**: Proprietà per verificare e accedere al valore

---

## 3. Verificare se un Nullable ha un Valore

### Proprietà HasValue

```csharp
int? number = null;

if (number.HasValue) {
    Console.WriteLine($"Il valore è: {number.Value}");
} else {
    Console.WriteLine("Il valore è null");
}
```

### Operatore Null-Coalescing (??)

```csharp
int? nullableNumber = null;
int result = nullableNumber ?? 0; // Se null, usa 0
Console.WriteLine(result); // Output: 0

int? number = 42;
int value = number ?? 0; // Se non null, usa il valore
Console.WriteLine(value); // Output: 42
```

### Null-Conditional Operator (?. e ?[])

```csharp
// Per oggetti
Person? person = GetPerson();

// ✅ Sicuro: non lancia NullReferenceException
string? name = person?.Name; // null se person è null
int? age = person?.Age; // null se person è null

// Per array/collezioni
int[]? numbers = GetNumbers();
int? first = numbers?[0]; // null se numbers è null
```

---

## 4. Accedere al Valore di un Nullable

### Proprietà Value (Sicura)

```csharp
int? number = 42;

if (number.HasValue) {
    int value = number.Value; // ✅ Sicuro
    Console.WriteLine(value);
}
```

### Conversione Implicita

```csharp
int? nullableInt = 10;
int normalInt = nullableInt ?? 0; // Conversione esplicita con default

// ⚠️ ATTENZIONE: Conversione implicita solo se non null
int? nullable = 5;
int value = (int)nullable; // ✅ Funziona se non null
int? nullValue = null;
int result = (int)nullValue; // ❌ ERRORE: InvalidOperationException
```

### Pattern Matching (C# 7.0+)

```csharp
int? number = 42;

// Pattern matching
if (number is int value) {
    Console.WriteLine($"Il valore è: {value}");
}

// Switch expression
string message = number switch {
    null => "Valore mancante",
    int n when n > 0 => $"Numero positivo: {n}",
    int n => $"Numero: {n}"
};
```

---

## 5. Operazioni con Nullable Types

### Operatori Aritmetici

```csharp
int? a = 10;
int? b = 20;
int? c = null;

int? sum = a + b; // 30
int? sumWithNull = a + c; // null (qualsiasi operazione con null = null)
int? product = a * b; // 200
int? division = a / b; // 0

// Confronti
bool isEqual = (a == b); // false
bool isNull = (c == null); // true
bool isNotNull = (a != null); // true
```

### Operatori di Confronto

```csharp
int? a = 10;
int? b = null;
int? c = 5;

// Confronti con null
bool result1 = a > b; // false (qualsiasi confronto con null = false)
bool result2 = a < b; // false
bool result3 = a == b; // false
bool result4 = a != b; // true

// Confronti tra nullable
bool result5 = a > c; // true
bool result6 = a < c; // false
```

---

## 6. Nullable Reference Types (C# 8.0+)

### Introduzione

A partire da C# 8.0, è possibile abilitare i **Nullable Reference Types** per rendere i reference types non-nullable di default.

### Abilitazione

```csharp
// Nel file .csproj
<PropertyGroup>
  <Nullable>enable</Nullable>
</PropertyGroup>

// Oppure nel file .cs
#nullable enable
```

### Esempio

```csharp
#nullable enable

// string è non-nullable di default
string name = null; // ⚠️ WARNING: possibile null

// string? è esplicitamente nullable
string? nullableName = null; // ✅ OK

// Metodi
public string GetName() {
    return "Mario"; // ✅ Non può essere null
}

public string? GetOptionalName() {
    return null; // ✅ Può essere null
}

// Utilizzo
string name1 = GetName(); // ✅ OK
string? name2 = GetOptionalName(); // ✅ OK
string name3 = GetOptionalName(); // ⚠️ WARNING: possibile null
string name4 = GetOptionalName() ?? "Default"; // ✅ OK
```

---

## 7. Esempi Pratici

### Esempio 1: Database e Nullable

```csharp
public class User {
    public int Id { get; set; }
    public string Name { get; set; }
    public int? Age { get; set; } // Può essere null nel database
    public DateTime? LastLogin { get; set; } // Può essere null
}

// Lettura dal database
User user = GetUserFromDatabase(1);

if (user.Age.HasValue) {
    Console.WriteLine($"Età: {user.Age.Value}");
} else {
    Console.WriteLine("Età non specificata");
}

// Oppure con null-coalescing
int age = user.Age ?? 0;
Console.WriteLine($"Età: {age}");
```

### Esempio 2: Calcolo con Nullable

```csharp
public double? CalculateAverage(int?[] numbers) {
    if (numbers == null || numbers.Length == 0) {
        return null;
    }
    
    int sum = 0;
    int count = 0;
    
    foreach (int? number in numbers) {
        if (number.HasValue) {
            sum += number.Value;
            count++;
        }
    }
    
    return count > 0 ? (double?)sum / count : null;
}

// Utilizzo
int?[] values = { 10, 20, null, 30, null };
double? average = CalculateAverage(values);
Console.WriteLine(average?.ToString() ?? "Nessun valore valido");
```

### Esempio 3: Parsing con Nullable

```csharp
public int? ParseIntOrNull(string input) {
    if (int.TryParse(input, out int result)) {
        return result;
    }
    return null;
}

// Utilizzo
string userInput = Console.ReadLine();
int? number = ParseIntOrNull(userInput);

if (number.HasValue) {
    Console.WriteLine($"Hai inserito: {number.Value}");
} else {
    Console.WriteLine("Input non valido");
}
```

---

## 8. Best Practices

### ✅ Quando Usare Nullable Types

1. **Campi opzionali nel database**
2. **Valori che potrebbero non essere disponibili**
3. **API che possono restituire null**
4. **Configurazioni opzionali**

```csharp
// ✅ Buon uso
public class Product {
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; } // Opzionale
    public DateTime? ExpiryDate { get; set; } // Opzionale
}
```

### ❌ Quando NON Usare

1. **Per valori che devono sempre esistere**
2. **Come alternativa a valori di default appropriati**
3. **Per evitare la gestione degli errori**

```csharp
// ❌ Cattivo uso
public int? CalculateSum(int a, int b) {
    return a + b; // Perché nullable? La somma esiste sempre
}

// ✅ Meglio
public int CalculateSum(int a, int b) {
    return a + b;
}
```

### Pattern Consigliati

```csharp
// Pattern 1: Null-coalescing per valori di default
int age = user.Age ?? 0;

// Pattern 2: Null-conditional per chiamate sicure
string? name = person?.Name;

// Pattern 3: HasValue per verifiche esplicite
if (value.HasValue) {
    ProcessValue(value.Value);
}

// Pattern 4: Pattern matching (C# 7.0+)
if (value is int number) {
    ProcessValue(number);
}
```

---

## 9. Nullable e LINQ

### Esempi con LINQ

```csharp
List<int?> numbers = new List<int?> { 1, 2, null, 4, null, 6 };

// Filtrare valori null
var validNumbers = numbers.Where(n => n.HasValue).Select(n => n.Value);

// Oppure
var validNumbers2 = numbers.OfType<int>();

// Calcolare somma ignorando null
int sum = numbers.Where(n => n.HasValue).Sum(n => n.Value);
// Oppure
int sum2 = numbers.Sum(n => n ?? 0);

// Valore massimo/minimo
int? max = numbers.Max(); // Restituisce null se tutti sono null
int? min = numbers.Min();
```

---

## 10. Domande Frequenti (FAQ)

### Q: Qual è la differenza tra `int?` e `Nullable<int>`?
**R:** Nessuna differenza. `int?` è solo una sintassi abbreviata per `Nullable<int>`. Il compilatore li tratta allo stesso modo.

### Q: Posso usare nullable con qualsiasi tipo valore?
**R:** Sì, puoi usare nullable con qualsiasi value type (int, double, bool, DateTime, struct personalizzate, ecc.).

### Q: Cosa succede se accedo a `.Value` quando HasValue è false?
**R:** Viene lanciata un'eccezione `InvalidOperationException`. Sempre verificare `HasValue` prima di accedere a `.Value`.

### Q: I nullable types influenzano le performance?
**R:** C'è un overhead minimo (un bool per HasValue), ma è trascurabile nella maggior parte dei casi.

### Q: Quando usare nullable reference types (C# 8.0+)?
**R:** Usali per:
- Nuovi progetti per prevenire NullReferenceException
- Migliorare la documentazione del codice
- Rendere esplicito quando un valore può essere null

---

## 11. Esercizi Pratici

### Esercizio 1: Calcolatrice Nullable

```csharp
// Crea un metodo che calcola la divisione, restituendo null se il divisore è 0
public double? SafeDivide(double dividend, double divisor) {
    // La tua implementazione qui
}
```

### Esercizio 2: Validazione Nullable

```csharp
// Crea un metodo che valida se un nullable int è positivo
public bool IsPositive(int? number) {
    // La tua implementazione qui
}
```

### Esercizio 3: Media con Nullable

```csharp
// Calcola la media di una lista di nullable int, ignorando i null
public double? CalculateAverage(List<int?> numbers) {
    // La tua implementazione qui
}
```

---

## Conclusioni

I Nullable Types in C# sono essenziali per:

- ✅ **Rappresentare valori mancanti** per value types
- ✅ **Lavorare con database** che possono avere campi NULL
- ✅ **Gestire API** che possono restituire null
- ✅ **Scrivere codice più sicuro** con null-conditional operators

**Ricorda:**
- Usa `HasValue` per verificare se un nullable ha un valore
- Usa `??` (null-coalescing) per valori di default
- Usa `?.` (null-conditional) per chiamate sicure
- Considera nullable reference types per nuovi progetti

---

*Documento creato per spiegare i Nullable Types in C# con esempi pratici e best practices.*

