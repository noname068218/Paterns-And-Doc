# Ref e Out Parameters in C#

## Introduzione

I parametri **ref** e **out** in C# permettono di passare argomenti per riferimento invece che per valore. Questi modificatori sono essenziali quando si vuole modificare il valore di una variabile all'interno di un metodo e mantenere quella modifica dopo l'esecuzione del metodo.

---

## 1. Differenza tra Passaggio per Valore e per Riferimento

### Passaggio per Valore (Default)

```csharp
// ❌ PROBLEMA: Il valore non viene modificato
public void Increment(int number) {
    number++; // Modifica solo la copia locale
}

int value = 5;
Increment(value);
Console.WriteLine(value); // Output: 5 (non modificato!)
```

**Spiegazione:**
- Quando passiamo un parametro per valore, viene creata una **copia** della variabile
- Le modifiche all'interno del metodo non influenzano la variabile originale
- Questo è il comportamento di default in C#

### Diagramma: Passaggio per Valore

```
┌─────────────────┐
│   Main Method   │
│                 │
│  int value = 5  │
│       │         │
│       ▼         │
│  Increment(5)   │───┐
└─────────────────┘   │
                      │ (copia del valore)
                      ▼
              ┌───────────────┐
              │ Increment()   │
              │               │
              │ number = 5    │
              │ number++      │
              │ number = 6    │ (solo locale)
              └───────────────┘
                      
┌─────────────────┐
│   Main Method   │
│  value = 5      │ (non modificato)
└─────────────────┘
```

---

## 2. Parametro `ref` (Reference)

### Definizione

Il modificatore **ref** passa l'argomento per riferimento, permettendo al metodo di modificare il valore della variabile originale.

### Sintassi

```csharp
// Metodo con parametro ref
public void Increment(ref int number) {
    number++; // Modifica la variabile originale
}

// Chiamata
int value = 5;
Increment(ref value); // Nota: 'ref' è obbligatorio nella chiamata
Console.WriteLine(value); // Output: 6 (modificato!)
```

### Caratteristiche di `ref`

1. **La variabile deve essere inizializzata** prima della chiamata
2. **Il modificatore `ref` è obbligatorio** sia nella dichiarazione che nella chiamata
3. **Il metodo può leggere e modificare** il valore
4. **Le modifiche persistono** dopo l'esecuzione del metodo

### Esempio Pratico: Swap di Valori

```csharp
// Metodo per scambiare due valori
public void Swap(ref int a, ref int b) {
    int temp = a;
    a = b;
    b = temp;
}

// Utilizzo
int x = 10;
int y = 20;
Console.WriteLine($"Prima: x = {x}, y = {y}"); // Prima: x = 10, y = 20

Swap(ref x, ref y);
Console.WriteLine($"Dopo: x = {x}, y = {y}"); // Dopo: x = 20, y = 10
```

### Esempio: Modifica di Oggetti

```csharp
public class Person {
    public string Name { get; set; }
    public int Age { get; set; }
}

// Modifica di un oggetto (già passato per riferimento)
public void UpdatePerson(Person person) {
    person.Name = "Mario"; // Funziona senza ref (oggetti sono reference types)
    person = new Person { Name = "Luigi" }; // Non modifica l'originale!
}

// Con ref possiamo sostituire l'intero oggetto
public void ReplacePerson(ref Person person) {
    person = new Person { Name = "Luigi", Age = 30 }; // Modifica l'originale
}

// Utilizzo
Person p = new Person { Name = "Giuseppe", Age = 25 };
UpdatePerson(p);
Console.WriteLine(p.Name); // Output: "Mario"

ReplacePerson(ref p);
Console.WriteLine(p.Name); // Output: "Luigi"
```

---

## 3. Parametro `out` (Output)

### Definizione

Il modificatore **out** è simile a `ref`, ma indica che il parametro viene usato solo per restituire un valore. La variabile non deve essere inizializzata prima della chiamata.

### Sintassi

```csharp
// Metodo con parametro out
public void Divide(int dividend, int divisor, out int quotient, out int remainder) {
    quotient = dividend / divisor;
    remainder = dividend % divisor;
}

// Chiamata
int result, remainder;
Divide(17, 5, out result, out remainder);
Console.WriteLine($"17 / 5 = {result}, resto = {remainder}"); 
// Output: 17 / 5 = 3, resto = 2
```

### Caratteristiche di `out`

1. **La variabile NON deve essere inizializzata** prima della chiamata
2. **Il metodo DEVE assegnare un valore** al parametro out prima di terminare
3. **Il modificatore `out` è obbligatorio** nella chiamata (in C# 7.0+ può essere omesso in alcuni casi)
4. **Il metodo non può leggere** il valore prima di assegnarlo

### Esempio: TryParse Pattern

```csharp
// Pattern comune: TryParse
public bool TryParseNumber(string input, out int number) {
    number = 0; // Inizializzazione obbligatoria
    
    if (string.IsNullOrWhiteSpace(input)) {
        return false;
    }
    
    if (int.TryParse(input, out int parsedValue)) {
        number = parsedValue;
        return true;
    }
    
    return false;
}

// Utilizzo
string userInput = "123";
if (TryParseNumber(userInput, out int result)) {
    Console.WriteLine($"Numero valido: {result}");
} else {
    Console.WriteLine("Numero non valido");
}
```

### Esempio: Restituire Più Valori

```csharp
// Metodo che restituisce più valori usando out
public bool TryGetUserInfo(int userId, out string name, out string email, out int age) {
    // Simulazione di ricerca nel database
    if (userId == 1) {
        name = "Mario Rossi";
        email = "mario@example.com";
        age = 30;
        return true;
    }
    
    // Valori di default se non trovato
    name = null;
    email = null;
    age = 0;
    return false;
}

// Utilizzo
if (TryGetUserInfo(1, out string userName, out string userEmail, out int userAge)) {
    Console.WriteLine($"Nome: {userName}, Email: {userEmail}, Età: {userAge}");
}
```

---

## 4. Confronto: `ref` vs `out`

### Tabella Comparativa

| Caratteristica | `ref` | `out` |
|----------------|-------|-------|
| **Inizializzazione richiesta** | ✅ Sì, prima della chiamata | ❌ No |
| **Lettura prima dell'assegnazione** | ✅ Sì | ❌ No |
| **Assegnazione obbligatoria** | ❌ No | ✅ Sì, nel metodo |
| **Uso principale** | Modificare valore esistente | Restituire nuovo valore |
| **Sintassi chiamata** | `ref variable` | `out variable` |

### Esempio Comparativo

```csharp
public void ExampleRef(ref int value) {
    Console.WriteLine(value); // ✅ Può leggere
    value = 10; // Modifica opzionale
}

public void ExampleOut(out int value) {
    // Console.WriteLine(value); // ❌ ERRORE: non può leggere prima
    value = 10; // ✅ DEVE assegnare
}

// Utilizzo
int refValue = 5; // ✅ Deve essere inizializzato
ExampleRef(ref refValue);

int outValue; // ✅ Non deve essere inizializzato
ExampleOut(out outValue);
```

---

## 5. `in` Parameter (C# 7.2+)

### Definizione

Il modificatore **in** passa un parametro per riferimento ma in sola lettura. È utile per struct grandi per evitare copie costose.

### Sintassi

```csharp
public struct LargeStruct {
    public int Value1;
    public int Value2;
    // ... molti altri campi
}

// Con 'in': passa per riferimento ma in sola lettura
public void ProcessLargeStruct(in LargeStruct data) {
    // data.Value1 = 10; // ❌ ERRORE: non può modificare
    int value = data.Value1; // ✅ Può solo leggere
}

// Utilizzo
LargeStruct large = new LargeStruct { Value1 = 5, Value2 = 10 };
ProcessLargeStruct(in large); // 'in' è opzionale nella chiamata
ProcessLargeStruct(large); // Anche questo funziona
```

---

## 6. Esempi Avanzati

### Esempio 1: Metodo che Modifica Array

```csharp
// Metodo che modifica un array usando ref
public void ResizeArray(ref int[] array, int newSize) {
    int[] newArray = new int[newSize];
    int minSize = Math.Min(array.Length, newSize);
    
    for (int i = 0; i < minSize; i++) {
        newArray[i] = array[i];
    }
    
    array = newArray; // Sostituisce l'array originale
}

// Utilizzo
int[] numbers = { 1, 2, 3 };
ResizeArray(ref numbers, 5);
Console.WriteLine($"Lunghezza: {numbers.Length}"); // Output: 5
```

### Esempio 2: Metodo con Multiple Out Parameters

```csharp
// Calcolo statistiche di un array
public void CalculateStatistics(int[] numbers, out int min, out int max, out double average) {
    if (numbers == null || numbers.Length == 0) {
        min = max = 0;
        average = 0;
        return;
    }
    
    min = numbers[0];
    max = numbers[0];
    int sum = 0;
    
    foreach (int num in numbers) {
        if (num < min) min = num;
        if (num > max) max = num;
        sum += num;
    }
    
    average = (double)sum / numbers.Length;
}

// Utilizzo
int[] data = { 10, 20, 30, 40, 50 };
CalculateStatistics(data, out int minimum, out int maximum, out double avg);
Console.WriteLine($"Min: {minimum}, Max: {maximum}, Media: {avg}");
```

### Esempio 3: Pattern Matching con Out (C# 7.0+)

```csharp
// In C# 7.0+ possiamo dichiarare variabili out inline
string input = "42";

if (int.TryParse(input, out int number)) {
    Console.WriteLine($"Numero parsato: {number}");
}

// Anche in condizioni
if (int.TryParse(input, out int result) && result > 0) {
    Console.WriteLine($"Numero positivo: {result}");
}
```

---

## 7. Best Practices

### ✅ Quando Usare `ref`

1. **Quando devi modificare un valore esistente**
2. **Per ottimizzare performance con struct grandi** (evita copie)
3. **Per implementare algoritmi che richiedono swap o scambi**

```csharp
// ✅ Buon uso di ref
public void SortArray(ref int[] array) {
    Array.Sort(array);
}
```

### ✅ Quando Usare `out`

1. **Quando devi restituire più valori** (alternativa a Tuple)
2. **Per implementare pattern TryParse**
3. **Quando il valore non esiste prima della chiamata**

```csharp
// ✅ Buon uso di out
public bool TryGetValue(string key, out string value) {
    // Logica di ricerca
    value = foundValue;
    return found;
}
```

### ❌ Quando NON Usare

1. **Per valori semplici che possono essere restituiti normalmente**
2. **Quando puoi usare Tuple o record** (C# 7.0+)
3. **Per aumentare la complessità senza necessità**

```csharp
// ❌ Meglio evitare
public void BadExample(ref int simpleValue) {
    simpleValue = 10;
}

// ✅ Meglio così
public int GoodExample() {
    return 10;
}
```

---

## 8. Performance Considerations

### Struct Grandi

```csharp
public struct Point3D {
    public double X, Y, Z;
    // ... molti altri campi
}

// ❌ LENTO: copia l'intera struct
public double CalculateDistance(Point3D p1, Point3D p2) {
    // Copia costosa
}

// ✅ VELOCE: passa per riferimento
public double CalculateDistance(in Point3D p1, in Point3D p2) {
    // Nessuna copia, solo riferimento
}
```

---

## 9. Domande Frequenti (FAQ)

### Q: Posso usare ref/out con proprietà?
**R:** No, non puoi passare proprietà direttamente come ref/out. Devi usare una variabile locale:

```csharp
public class MyClass {
    public int Value { get; set; }
}

MyClass obj = new MyClass();
int temp = obj.Value;
Increment(ref temp);
obj.Value = temp;
```

### Q: Qual è la differenza tra ref e out in termini di performance?
**R:** Nessuna differenza significativa. Entrambi passano per riferimento. La differenza è semantica.

### Q: Posso usare ref/out con async methods?
**R:** No, i parametri ref/out non sono supportati nei metodi async.

### Q: Quando usare Tuple invece di out?
**R:** Usa Tuple quando:
- Hai bisogno di restituire più valori in modo più leggibile
- Il codice è più moderno (C# 7.0+)
- Non hai bisogno di pattern TryParse

```csharp
// Con Tuple
public (int quotient, int remainder) Divide(int a, int b) {
    return (a / b, a % b);
}

// Con out
public void Divide(int a, int b, out int quotient, out int remainder) {
    quotient = a / b;
    remainder = a % b;
}
```

---

## 10. Esercizi Pratici

### Esercizio 1: Implementa Swap Generico

```csharp
// Implementa un metodo Swap generico usando ref
public void Swap<T>(ref T a, ref T b) {
    // La tua implementazione qui
}
```

### Esercizio 2: TryParse Personalizzato

```csharp
// Crea un metodo TryParse per un tipo personalizzato
public class Person {
    public string Name { get; set; }
    public int Age { get; set; }
}

// Implementa: bool TryParsePerson(string input, out Person person)
```

### Esercizio 3: Calcolatrice con Multiple Outputs

```csharp
// Crea un metodo che calcola somma, differenza, prodotto e quoziente
// usando out parameters
public void CalculateAll(int a, int b, 
    out int sum, out int diff, out int product, out int quotient) {
    // La tua implementazione qui
}
```

---

## Conclusioni

I parametri `ref` e `out` sono strumenti potenti in C# che permettono:

- ✅ **Modificare valori** passati a metodi
- ✅ **Restituire più valori** da un singolo metodo
- ✅ **Ottimizzare performance** con struct grandi
- ✅ **Implementare pattern comuni** come TryParse

**Ricorda:**
- Usa `ref` quando devi modificare un valore esistente
- Usa `out` quando devi restituire un nuovo valore
- Usa `in` per struct grandi in sola lettura
- Considera Tuple come alternativa moderna a out

---

*Documento creato per spiegare ref e out parameters in C# con esempi pratici e best practices.*

