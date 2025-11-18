# LINQ (Language Integrated Query) in C#

## Introduzione

**LINQ** (Language Integrated Query) è una funzionalità di C# che permette di eseguire query su collezioni di dati in modo dichiarativo e type-safe. LINQ integra la capacità di query direttamente nel linguaggio C#.

---

## 1. Cos'è LINQ?

### Definizione

LINQ permette di scrivere query simili a SQL direttamente in C#, lavorando con:
- Collezioni in memoria (List, Array, Dictionary)
- Database (Entity Framework)
- XML
- Altri sorgenti dati

### Vantaggi

✅ **Type-safe** - Controllo dei tipi a compile-time  
✅ **IntelliSense** - Supporto IDE completo  
✅ **Dichiarativo** - Cosa vuoi, non come ottenerlo  
✅ **Riutilizzabile** - Stessa sintassi per diverse sorgenti  

### Diagramma: LINQ Architecture

```
┌─────────────────────────────────────────────┐
│              LINQ                           │
└─────────────────────────────────────────────┘
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│ LINQ to   │ │ LINQ to   │ │ LINQ to   │
│ Objects   │ │ SQL       │ │ XML       │
│           │ │ (EF)      │ │           │
└───────────┘ └───────────┘ └───────────┘
```

---

## 2. Sintassi LINQ: Query Syntax vs Method Syntax

### Query Syntax (Simile a SQL)

```csharp
// Query Syntax
var result = from student in students
             where student.Age > 18
             orderby student.Name
             select student;
```

### Method Syntax (Fluent API)

```csharp
// Method Syntax (più comune)
var result = students
    .Where(s => s.Age > 18)
    .OrderBy(s => s.Name)
    .ToList();
```

### Confronto

```csharp
public class Student {
    public string Name { get; set; }
    public int Age { get; set; }
    public int Grade { get; set; }
}

var students = new List<Student> {
    new Student { Name = "Mario", Age = 20, Grade = 85 },
    new Student { Name = "Luigi", Age = 19, Grade = 90 },
    new Student { Name = "Peach", Age = 17, Grade = 95 }
};

// Query Syntax
var querySyntax = from s in students
                  where s.Age >= 18
                  orderby s.Grade descending
                  select s.Name;

// Method Syntax (equivalente)
var methodSyntax = students
    .Where(s => s.Age >= 18)
    .OrderByDescending(s => s.Grade)
    .Select(s => s.Name);
```

---

## 3. Operatori LINQ Fondamentali

### Where - Filtrare

```csharp
// Filtra elementi che soddisfano una condizione
var adults = students.Where(s => s.Age >= 18);

// Multiple condizioni
var topStudents = students
    .Where(s => s.Age >= 18 && s.Grade > 90);
```

### Select - Proiezione

```csharp
// Trasforma ogni elemento
var names = students.Select(s => s.Name);

// Proiezione complessa
var studentInfo = students.Select(s => new {
    Name = s.Name,
    IsAdult = s.Age >= 18
});
```

### OrderBy / OrderByDescending - Ordinamento

```csharp
// Ordinamento ascendente
var sortedByName = students.OrderBy(s => s.Name);

// Ordinamento discendente
var sortedByGrade = students.OrderByDescending(s => s.Grade);

// Ordinamento multiplo
var sorted = students
    .OrderBy(s => s.Age)
    .ThenByDescending(s => s.Grade);
```

### First / FirstOrDefault

```csharp
// Primo elemento (lancia eccezione se vuoto)
var first = students.First();

// Primo elemento con condizione
var firstAdult = students.First(s => s.Age >= 18);

// Primo elemento o default (null se vuoto)
var firstOrDefault = students.FirstOrDefault();
var firstAdultOrNull = students.FirstOrDefault(s => s.Age >= 18);
```

### Last / LastOrDefault

```csharp
// Ultimo elemento
var last = students.Last();
var lastOrDefault = students.LastOrDefault();
```

### Single / SingleOrDefault

```csharp
// Un solo elemento (lancia eccezione se 0 o più di 1)
var single = students.Single(s => s.Name == "Mario");

// Un solo elemento o default
var singleOrDefault = students.SingleOrDefault(s => s.Name == "Mario");
```

### Any / All

```csharp
// Verifica se almeno un elemento soddisfa la condizione
bool hasAdults = students.Any(s => s.Age >= 18);

// Verifica se tutti gli elementi soddisfano la condizione
bool allAdults = students.All(s => s.Age >= 18);
```

### Count / Count con condizione

```csharp
// Conta tutti gli elementi
int total = students.Count();

// Conta elementi che soddisfano condizione
int adultsCount = students.Count(s => s.Age >= 18);
```

### Sum / Average / Min / Max

```csharp
// Somma
int totalAge = students.Sum(s => s.Age);

// Media
double averageAge = students.Average(s => s.Age);

// Minimo
int minAge = students.Min(s => s.Age);

// Massimo
int maxAge = students.Max(s => s.Age);
```

---

## 4. Operatori di Raggruppamento

### GroupBy

```csharp
// Raggruppa per proprietà
var groupedByAge = students.GroupBy(s => s.Age);

foreach (var group in groupedByAge) {
    Console.WriteLine($"Età: {group.Key}");
    foreach (var student in group) {
        Console.WriteLine($"  - {student.Name}");
    }
}

// Raggruppa con proiezione
var grouped = students.GroupBy(s => s.Age, s => s.Name);
```

### Diagramma: GroupBy

```
┌─────────────────────────────────────────────┐
│  Students:                                   │
│  [Mario, 20]                                │
│  [Luigi, 19]                                │
│  [Peach, 20]                                │
│  [Toad, 19]                                 │
└─────────────────────────────────────────────┘
                    │
                    ▼ GroupBy(s => s.Age)
                    │
┌─────────────────────────────────────────────┐
│  Group Key: 20                              │
│    - Mario                                  │
│    - Peach                                  │
├─────────────────────────────────────────────┤
│  Group Key: 19                              │
│    - Luigi                                  │
│    - Toad                                   │
└─────────────────────────────────────────────┘
```

---

## 5. Operatori di Join

### Join

```csharp
public class Student {
    public int Id { get; set; }
    public string Name { get; set; }
    public int CourseId { get; set; }
}

public class Course {
    public int Id { get; set; }
    public string Name { get; set; }
}

var students = new List<Student> {
    new Student { Id = 1, Name = "Mario", CourseId = 1 },
    new Student { Id = 2, Name = "Luigi", CourseId = 2 }
};

var courses = new List<Course> {
    new Course { Id = 1, Name = "C#" },
    new Course { Id = 2, Name = "Java" }
};

// Inner Join
var studentCourses = students.Join(
    courses,
    student => student.CourseId,
    course => course.Id,
    (student, course) => new {
        StudentName = student.Name,
        CourseName = course.Name
    }
);

// Query Syntax
var querySyntax = from s in students
                  join c in courses on s.CourseId equals c.Id
                  select new { s.Name, c.Name };
```

### GroupJoin

```csharp
// Left Join equivalente
var studentCourses = students.GroupJoin(
    courses,
    student => student.CourseId,
    course => course.Id,
    (student, courseGroup) => new {
        Student = student.Name,
        Courses = courseGroup.Select(c => c.Name)
    }
);
```

---

## 6. Operatori di Set

### Distinct

```csharp
var numbers = new[] { 1, 2, 2, 3, 3, 4 };
var unique = numbers.Distinct(); // { 1, 2, 3, 4 }

// Con oggetti personalizzati
var uniqueStudents = students.Distinct(); // Richiede Equals/GetHashCode
```

### Union / Intersect / Except

```csharp
var set1 = new[] { 1, 2, 3, 4 };
var set2 = new[] { 3, 4, 5, 6 };

// Unione (tutti gli elementi unici)
var union = set1.Union(set2); // { 1, 2, 3, 4, 5, 6 }

// Intersezione (elementi comuni)
var intersect = set1.Intersect(set2); // { 3, 4 }

// Differenza (elementi in set1 ma non in set2)
var except = set1.Except(set2); // { 1, 2 }
```

---

## 7. Operatori di Partizionamento

### Take / Skip

```csharp
// Prendi i primi N elementi
var firstThree = students.Take(3);

// Salta i primi N elementi
var skipFirst = students.Skip(2);

// Paginazione
int pageSize = 10;
int pageNumber = 2;
var page = students
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize);
```

### TakeWhile / SkipWhile

```csharp
// Prendi elementi finché la condizione è vera
var takeWhile = students.TakeWhile(s => s.Age < 20);

// Salta elementi finché la condizione è vera
var skipWhile = students.SkipWhile(s => s.Age < 18);
```

---

## 8. Operatori di Conversione

### ToList / ToArray

```csharp
// Converte in List
var list = students.Where(s => s.Age >= 18).ToList();

// Converte in Array
var array = students.Where(s => s.Age >= 18).ToArray();
```

### ToDictionary

```csharp
// Crea Dictionary da collezione
var dict = students.ToDictionary(s => s.Id, s => s.Name);

// Utilizzo
string name = dict[1]; // "Mario"
```

### ToLookup

```csharp
// Crea Lookup (simile a Dictionary ma con più valori per chiave)
var lookup = students.ToLookup(s => s.Age);

// Utilizzo
var studentsAge20 = lookup[20]; // Tutti gli studenti di 20 anni
```

---

## 9. Operatori di Aggregazione

### Aggregate

```csharp
// Operazione personalizzata di aggregazione
var numbers = new[] { 1, 2, 3, 4, 5 };

// Somma personalizzata
int sum = numbers.Aggregate((acc, num) => acc + num); // 15

// Con seed iniziale
int product = numbers.Aggregate(1, (acc, num) => acc * num); // 120

// Con risultato finale trasformato
string result = numbers.Aggregate(
    "Numbers: ",
    (acc, num) => acc + num + ", ",
    acc => acc.TrimEnd(',', ' ')
);
```

---

## 10. Esecuzione Differita (Deferred Execution)

### Lazy Evaluation

```csharp
var students = new List<Student> { /* ... */ };

// Query NON eseguita ancora!
var query = students.Where(s => s.Age >= 18);

// Query eseguita solo quando iteriamo
foreach (var student in query) {
    Console.WriteLine(student.Name);
}

// Oppure quando chiamiamo ToList/ToArray
var result = query.ToList(); // Query eseguita qui!
```

### Esecuzione Immediata

```csharp
// Questi operatori eseguono immediatamente:
var count = students.Count(); // Eseguito subito
var first = students.First(); // Eseguito subito
var list = students.ToList(); // Eseguito subito
```

### Diagramma: Esecuzione Differita

```
┌─────────────────────────────────────────────┐
│  var query = students.Where(...)           │
│  (Nessuna esecuzione ancora)                │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Query memorizzata    │
        │  (lazy)               │
        └───────────────────────┘
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│ foreach   │ │ ToList()  │ │ Count()   │
│ (esegue)  │ │ (esegue)  │ │ (esegue)  │
└───────────┘ └───────────┘ └───────────┘
```

---

## 11. Esempi Pratici Completi

### Esempio 1: Filtrare e Ordinare

```csharp
var result = students
    .Where(s => s.Age >= 18 && s.Grade >= 80)
    .OrderByDescending(s => s.Grade)
    .ThenBy(s => s.Name)
    .Select(s => new {
        s.Name,
        s.Grade,
        Status = s.Grade >= 90 ? "Excellent" : "Good"
    })
    .ToList();
```

### Esempio 2: Raggruppamento e Aggregazione

```csharp
var statistics = students
    .GroupBy(s => s.Age)
    .Select(g => new {
        Age = g.Key,
        Count = g.Count(),
        AverageGrade = g.Average(s => s.Grade),
        TopStudent = g.OrderByDescending(s => s.Grade).First().Name
    })
    .OrderBy(s => s.Age)
    .ToList();
```

### Esempio 3: Join Complesso

```csharp
var studentCourseInfo = from s in students
                        join c in courses on s.CourseId equals c.Id
                        join e in enrollments on s.Id equals e.StudentId
                        where e.EnrollmentDate.Year == 2024
                        select new {
                            StudentName = s.Name,
                            CourseName = c.Name,
                            EnrollmentDate = e.EnrollmentDate
                        };
```

### Esempio 4: Operazioni Complesse

```csharp
// Trova studenti con voti sopra la media
var averageGrade = students.Average(s => s.Grade);
var aboveAverage = students
    .Where(s => s.Grade > averageGrade)
    .OrderByDescending(s => s.Grade)
    .ToList();

// Top 3 studenti per ogni età
var top3ByAge = students
    .GroupBy(s => s.Age)
    .SelectMany(g => g.OrderByDescending(s => s.Grade).Take(3))
    .ToList();
```

---

## 12. LINQ con Entity Framework

### Query su Database

```csharp
// LINQ to Entities (Entity Framework)
using (var context = new SchoolContext()) {
    // Query eseguita sul database
    var students = context.Students
        .Where(s => s.Age >= 18)
        .OrderBy(s => s.Name)
        .ToList();
    
    // Query con join
    var result = from s in context.Students
                 join c in context.Courses on s.CourseId equals c.Id
                 select new { s.Name, c.Name };
}
```

### Esecuzione Differita in EF

```csharp
// Query NON eseguita ancora
var query = context.Students.Where(s => s.Age >= 18);

// Query eseguita quando chiamiamo ToList/First/etc
var result = query.ToList(); // SQL eseguito qui!
```

---

## 13. Performance e Best Practices

### ✅ Cosa Fare

1. **Usa ToList/ToArray solo quando necessario**
   ```csharp
   // ✅ OK - Esecuzione differita
   var query = students.Where(s => s.Age >= 18);
   
   // ✅ OK - Se serve List
   var list = students.Where(s => s.Age >= 18).ToList();
   ```

2. **Usa FirstOrDefault invece di Where().First()**
   ```csharp
   // ✅ CORRETTO
   var student = students.FirstOrDefault(s => s.Age >= 18);
   
   // ⚠️ MENO EFFICIENTE
   var student = students.Where(s => s.Age >= 18).FirstOrDefault();
   ```

3. **Filtra prima di ordinare**
   ```csharp
   // ✅ CORRETTO
   var result = students
       .Where(s => s.Age >= 18)  // Filtra prima
       .OrderBy(s => s.Name);     // Poi ordina
   ```

### ❌ Cosa Evitare

1. **Non iterare più volte su query LINQ**
   ```csharp
   // ❌ SBAGLIATO - Query eseguita due volte
   var query = students.Where(s => s.Age >= 18);
   var count = query.Count();
   var list = query.ToList();
   
   // ✅ CORRETTO - Memorizza risultato
   var list = students.Where(s => s.Age >= 18).ToList();
   var count = list.Count;
   ```

2. **Non usare ToList() inutile**
   ```csharp
   // ❌ SBAGLIATO
   var result = students.ToList().Where(s => s.Age >= 18);
   
   // ✅ CORRETTO
   var result = students.Where(s => s.Age >= 18);
   ```

---

## 14. Domande Frequenti (FAQ)

### Q: Qual è la differenza tra First() e FirstOrDefault()?
**R:** `First()` lancia eccezione se la collezione è vuota. `FirstOrDefault()` ritorna il valore di default (null per reference types).

### Q: Quando viene eseguita una query LINQ?
**R:** Quando iteriamo (foreach) o chiamiamo operatori di esecuzione immediata (ToList, ToArray, Count, First, etc.).

### Q: Qual è la differenza tra Select e SelectMany?
**R:** `Select` proietta ogni elemento. `SelectMany` appiattisce collezioni annidate.

### Q: LINQ è più lento dei loop tradizionali?
**R:** Per operazioni semplici può essere leggermente più lento, ma la differenza è minima e la leggibilità compensa.

---

## Conclusioni

LINQ è uno strumento potente che:
- ✅ Migliora la leggibilità del codice
- ✅ Fornisce type-safety
- ✅ Unifica la sintassi per diverse sorgenti dati
- ✅ Supporta esecuzione differita per performance

Usa LINQ per scrivere codice più dichiarativo e manutenibile!

---

_Documento creato per spiegare LINQ in C# con esempi pratici e best practices._

