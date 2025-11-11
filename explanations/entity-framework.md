# Entity Framework Core

## Introduzione

**Entity Framework (EF) Core** è un ORM (Object-Relational Mapping) moderno e open-source per .NET. Permette agli sviluppatori di lavorare con database usando oggetti .NET invece di SQL diretto, migliorando la produttività e la manutenibilità.

---

## 1. Cos'è Entity Framework?

### Definizione

Entity Framework Core è un ORM che:
- Mappa oggetti .NET a tabelle del database
- Esegue query usando LINQ invece di SQL
- Gestisce relazioni tra entità
- Traccia le modifiche e le sincronizza con il database

### Vantaggi

- ✅ **Productivity**: Meno codice SQL manuale
- ✅ **Type Safety**: Compile-time checking
- ✅ **LINQ**: Query espressive in C#
- ✅ **Change Tracking**: Modifiche automatiche
- ✅ **Migrations**: Gestione schema automatica

### Diagramma: Entity Framework

```
┌─────────────────────────────────────────────┐
│  C# Code                                    │
│  var persona = context.Personas             │
│      .FirstOrDefault(p => p.Id == 1);     │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Entity Framework     │
        │  - LINQ to SQL        │
        │  - Mapping            │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  SQL Query            │
        │  SELECT * FROM Personas│
        │  WHERE Id = 1         │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Database             │
        └───────────────────────┘
```

---

## 2. Installazione e Setup

### Installazione Package

```bash
# Package NuGet
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools

# Per migrations
dotnet add package Microsoft.EntityFrameworkCore.Design
```

### Creazione DbContext

```csharp
// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;

namespace MyApp.Data;

public class ApplicationDbContext : DbContext {
    public DbSet<Persona> Personas { get; set; }
    public DbSet<Ordine> Ordini { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        // Configurazioni
        modelBuilder.Entity<Persona>(entity => {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
        });
    }
}
```

### Configurazione in Program.cs

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Configurazione DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

var app = builder.Build();
app.Run();
```

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MyAppDb;Trusted_Connection=True;"
  }
}
```

---

## 3. Entity Classes

### Entity Base

```csharp
// Models/Persona.cs
public class Persona {
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public DateTime DataNascita { get; set; }
    
    // Navigation Property
    public List<Ordine> Ordini { get; set; }
}
```

### Entity con Configurazione

```csharp
// Models/Persona.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Personas")]
public class Persona {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Nome { get; set; }
    
    [Required]
    [MaxLength(255)]
    [EmailAddress]
    public string Email { get; set; }
    
    [Column("DataNascita", TypeName = "date")]
    public DateTime DataNascita { get; set; }
    
    // Navigation Property
    public virtual ICollection<Ordine> Ordini { get; set; }
}
```

### Fluent API Configuration

```csharp
// Data/ApplicationDbContext.cs
protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.Entity<Persona>(entity => {
        // Primary Key
        entity.HasKey(e => e.Id);
        
        // Properties
        entity.Property(e => e.Nome)
            .IsRequired()
            .HasMaxLength(100);
        
        entity.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(255);
        
        entity.Property(e => e.DataNascita)
            .HasColumnType("date");
        
        // Index
        entity.HasIndex(e => e.Email)
            .IsUnique();
        
        // Relationships
        entity.HasMany(e => e.Ordini)
            .WithOne(o => o.Persona)
            .HasForeignKey(o => o.PersonaId)
            .OnDelete(DeleteBehavior.Cascade);
    });
}
```

---

## 4. CRUD Operations

### Create (Inserimento)

```csharp
// Creazione nuova entità
public async Task<int> CreatePersonaAsync(Persona persona) {
    using var context = new ApplicationDbContext(options);
    
    context.Personas.Add(persona);
    await context.SaveChangesAsync();
    
    return persona.Id;
}

// Creazione multipla
public async Task CreateMultipleAsync(List<Persona> persone) {
    using var context = new ApplicationDbContext(options);
    
    context.Personas.AddRange(persone);
    await context.SaveChangesAsync();
}
```

### Read (Lettura)

```csharp
// Lettura singola
public async Task<Persona> GetByIdAsync(int id) {
    using var context = new ApplicationDbContext(options);
    
    return await context.Personas
        .FirstOrDefaultAsync(p => p.Id == id);
}

// Lettura con filtro
public async Task<List<Persona>> GetByNomeAsync(string nome) {
    using var context = new ApplicationDbContext(options);
    
    return await context.Personas
        .Where(p => p.Nome.Contains(nome))
        .ToListAsync();
}

// Lettura con include (eager loading)
public async Task<Persona> GetWithOrdiniAsync(int id) {
    using var context = new ApplicationDbContext(options);
    
    return await context.Personas
        .Include(p => p.Ordini)
        .FirstOrDefaultAsync(p => p.Id == id);
}
```

### Update (Aggiornamento)

```csharp
// Update tramite tracking
public async Task UpdatePersonaAsync(Persona persona) {
    using var context = new ApplicationDbContext(options);
    
    context.Personas.Update(persona);
    await context.SaveChangesAsync();
}

// Update con ricerca
public async Task UpdatePersonaAsync(int id, Persona personaModificata) {
    using var context = new ApplicationDbContext(options);
    
    var persona = await context.Personas.FindAsync(id);
    if (persona != null) {
        persona.Nome = personaModificata.Nome;
        persona.Email = personaModificata.Email;
        await context.SaveChangesAsync();
    }
}

// Update parziale
public async Task UpdateNomeAsync(int id, string nuovoNome) {
    using var context = new ApplicationDbContext(options);
    
    var persona = await context.Personas.FindAsync(id);
    if (persona != null) {
        persona.Nome = nuovoNome;
        await context.SaveChangesAsync();
    }
}
```

### Delete (Eliminazione)

```csharp
// Delete tramite entità
public async Task DeletePersonaAsync(Persona persona) {
    using var context = new ApplicationDbContext(options);
    
    context.Personas.Remove(persona);
    await context.SaveChangesAsync();
}

// Delete tramite ID
public async Task DeletePersonaAsync(int id) {
    using var context = new ApplicationDbContext(options);
    
    var persona = await context.Personas.FindAsync(id);
    if (persona != null) {
        context.Personas.Remove(persona);
        await context.SaveChangesAsync();
    }
}

// Delete multiplo
public async Task DeleteMultipleAsync(List<int> ids) {
    using var context = new ApplicationDbContext(options);
    
    var persone = await context.Personas
        .Where(p => ids.Contains(p.Id))
        .ToListAsync();
    
    context.Personas.RemoveRange(persone);
    await context.SaveChangesAsync();
}
```

### Diagramma: CRUD Operations

```
┌─────────────────────────────────────────────┐
│  CREATE                                     │
│  context.Personas.Add(persona)              │
│  context.SaveChanges()                      │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  READ                                       │
│  context.Personas                            │
│      .Where(p => p.Id == id)               │
│      .FirstOrDefault()                      │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  UPDATE                                     │
│  persona.Nome = "Nuovo Nome"               │
│  context.SaveChanges()                      │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  DELETE                                     │
│  context.Personas.Remove(persona)           │
│  context.SaveChanges()                      │
└─────────────────────────────────────────────┘
```

---

## 5. LINQ Queries

### Query Syntax

```csharp
// Query Syntax
var persone = from p in context.Personas
              where p.Eta > 18
              orderby p.Nome
              select p;

var lista = await persone.ToListAsync();
```

### Method Syntax

```csharp
// Method Syntax
var persone = await context.Personas
    .Where(p => p.Eta > 18)
    .OrderBy(p => p.Nome)
    .ToListAsync();
```

### Query Complesse

```csharp
// Query con join
var risultato = await context.Personas
    .Join(
        context.Ordini,
        persona => persona.Id,
        ordine => ordine.PersonaId,
        (persona, ordine) => new {
            PersonaNome = persona.Nome,
            OrdineId = ordine.Id,
            Totale = ordine.Totale
        }
    )
    .ToListAsync();

// Query con group by
var gruppo = await context.Ordini
    .GroupBy(o => o.PersonaId)
    .Select(g => new {
        PersonaId = g.Key,
        TotaleOrdini = g.Count(),
        TotaleSpeso = g.Sum(o => o.Totale)
    })
    .ToListAsync();

// Query con aggregazioni
var statistiche = await context.Personas
    .Select(p => new {
        TotalePersone = context.Personas.Count(),
        EtaMedia = context.Personas.Average(p => p.Eta),
        EtaMinima = context.Personas.Min(p => p.Eta),
        EtaMassima = context.Personas.Max(p => p.Eta)
    })
    .FirstOrDefaultAsync();
```

---

## 6. Relationships

### One-to-Many

```csharp
// Models/Persona.cs
public class Persona {
    public int Id { get; set; }
    public string Nome { get; set; }
    
    // Navigation Property
    public List<Ordine> Ordini { get; set; }
}

// Models/Ordine.cs
public class Ordine {
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public decimal Totale { get; set; }
    
    // Foreign Key
    public int PersonaId { get; set; }
    
    // Navigation Property
    public Persona Persona { get; set; }
}

// Configuration
protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.Entity<Persona>()
        .HasMany(p => p.Ordini)
        .WithOne(o => o.Persona)
        .HasForeignKey(o => o.PersonaId);
}
```

### Many-to-Many

```csharp
// Models/Studente.cs
public class Studente {
    public int Id { get; set; }
    public string Nome { get; set; }
    public List<CorsoStudente> CorsiStudenti { get; set; }
}

// Models/Corso.cs
public class Corso {
    public int Id { get; set; }
    public string Nome { get; set; }
    public List<CorsoStudente> CorsiStudenti { get; set; }
}

// Junction Entity
public class CorsoStudente {
    public int StudenteId { get; set; }
    public int CorsoId { get; set; }
    public Studente Studente { get; set; }
    public Corso Corso { get; set; }
}

// Configuration
protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.Entity<CorsoStudente>()
        .HasKey(cs => new { cs.StudenteId, cs.CorsoId });
    
    modelBuilder.Entity<CorsoStudente>()
        .HasOne(cs => cs.Studente)
        .WithMany(s => s.CorsiStudenti)
        .HasForeignKey(cs => cs.StudenteId);
    
    modelBuilder.Entity<CorsoStudente>()
        .HasOne(cs => cs.Corso)
        .WithMany(c => c.CorsiStudenti)
        .HasForeignKey(cs => cs.CorsoId);
}
```

### One-to-One

```csharp
// Models/Persona.cs
public class Persona {
    public int Id { get; set; }
    public string Nome { get; set; }
    public Indirizzo Indirizzo { get; set; }
}

// Models/Indirizzo.cs
public class Indirizzo {
    public int Id { get; set; }
    public string Via { get; set; }
    public string Citta { get; set; }
    public int PersonaId { get; set; }
    public Persona Persona { get; set; }
}

// Configuration
protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.Entity<Persona>()
        .HasOne(p => p.Indirizzo)
        .WithOne(i => i.Persona)
        .HasForeignKey<Indirizzo>(i => i.PersonaId);
}
```

### Diagramma: Relationships

```
┌─────────────────────────────────────────────┐
│  ONE-TO-MANY                                │
│  Persona (1) ──< (Many) Ordine              │
│  - Persona.Ordini: List<Ordine>            │
│  - Ordine.Persona: Persona                 │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  MANY-TO-MANY                                │
│  Studente (Many) ──< >── (Many) Corso      │
│  Junction: CorsoStudente                   │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  ONE-TO-ONE                                 │
│  Persona (1) ──── (1) Indirizzo            │
│  - Persona.Indirizzo: Indirizzo            │
│  - Indirizzo.Persona: Persona              │
└─────────────────────────────────────────────┘
```

---

## 7. Loading Strategies

### Eager Loading

```csharp
// Include - carica relazioni subito
var persona = await context.Personas
    .Include(p => p.Ordini)
    .FirstOrDefaultAsync(p => p.Id == id);

// Include multipli
var persona = await context.Personas
    .Include(p => p.Ordini)
        .ThenInclude(o => o.Prodotti)
    .FirstOrDefaultAsync(p => p.Id == id);
```

### Lazy Loading

```csharp
// Abilita Lazy Loading
// Install package: Microsoft.EntityFrameworkCore.Proxies

// Program.cs
options.UseLazyLoadingProxies();

// Automatic loading quando accedi alla proprietà
var persona = await context.Personas.FindAsync(id);
var ordini = persona.Ordini;  // Carica automaticamente
```

### Explicit Loading

```csharp
// Carica esplicitamente
var persona = await context.Personas.FindAsync(id);
await context.Entry(persona)
    .Collection(p => p.Ordini)
    .LoadAsync();
```

### Diagramma: Loading Strategies

```
┌─────────────────────────────────────────────┐
│  EAGER LOADING                               │
│  .Include(p => p.Ordini)                    │
│  → Carica tutto subito                      │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  LAZY LOADING                                │
│  persona.Ordini                             │
│  → Carica quando accedi                     │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  EXPLICIT LOADING                            │
│  context.Entry(persona)                     │
│      .Collection(p => p.Ordini).Load()      │
│  → Carica quando dici tu                    │
└─────────────────────────────────────────────┘
```

---

## 8. Migrations

### Creare Migration

```bash
# Crea migration
dotnet ef migrations add InitialCreate

# Applica migration
dotnet ef database update

# Rimuovi ultima migration
dotnet ef migrations remove
```

### Migration Files

```csharp
// Migrations/20231201120000_InitialCreate.cs
public partial class InitialCreate : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
        migrationBuilder.CreateTable(
            name: "Personas",
            columns: table => new {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                DataNascita = table.Column<DateTime>(type: "date", nullable: false)
            },
            constraints: table => {
                table.PrimaryKey("PK_Personas", x => x.Id);
            });
    }
    
    protected override void Down(MigrationBuilder migrationBuilder) {
        migrationBuilder.DropTable(name: "Personas");
    }
}
```

### Diagramma: Migrations

```
┌─────────────────────────────────────────────┐
│  Modello Entity                             │
│  public class Persona { ... }               │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  dotnet ef migrations│
        │  add InitialCreate    │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Migration File       │
        │  Up() / Down()        │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  dotnet ef database   │
        │  update               │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Database Updated      │
        └───────────────────────┘
```

---

## 9. Repository Pattern con EF Core

### Repository Interface

```csharp
// Repositories/IPersonaRepository.cs
public interface IPersonaRepository {
    Task<Persona> GetByIdAsync(int id);
    Task<List<Persona>> GetAllAsync();
    Task<Persona> GetByEmailAsync(string email);
    Task<int> CreateAsync(Persona persona);
    Task UpdateAsync(Persona persona);
    Task DeleteAsync(int id);
}
```

### Repository Implementation

```csharp
// Repositories/PersonaRepository.cs
public class PersonaRepository : IPersonaRepository {
    private readonly ApplicationDbContext _context;
    
    public PersonaRepository(ApplicationDbContext context) {
        _context = context;
    }
    
    public async Task<Persona> GetByIdAsync(int id) {
        return await _context.Personas
            .Include(p => p.Ordini)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    
    public async Task<List<Persona>> GetAllAsync() {
        return await _context.Personas.ToListAsync();
    }
    
    public async Task<Persona> GetByEmailAsync(string email) {
        return await _context.Personas
            .FirstOrDefaultAsync(p => p.Email == email);
    }
    
    public async Task<int> CreateAsync(Persona persona) {
        _context.Personas.Add(persona);
        await _context.SaveChangesAsync();
        return persona.Id;
    }
    
    public async Task UpdateAsync(Persona persona) {
        _context.Personas.Update(persona);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id) {
        var persona = await _context.Personas.FindAsync(id);
        if (persona != null) {
            _context.Personas.Remove(persona);
            await _context.SaveChangesAsync();
        }
    }
}
```

---

## 10. Best Practices

### ✅ Cosa Fare

1. **Usa async/await per operazioni I/O**
   ```csharp
   // ✅ CORRETTO
   public async Task<Persona> GetByIdAsync(int id) {
       return await _context.Personas.FindAsync(id);
   }
   ```

2. **Usa Include per eager loading quando necessario**
   ```csharp
   // ✅ CORRETTO
   var persona = await _context.Personas
       .Include(p => p.Ordini)
       .FirstOrDefaultAsync(p => p.Id == id);
   ```

3. **Usa AsNoTracking per query di sola lettura**
   ```csharp
   // ✅ CORRETTO
   var persone = await _context.Personas
       .AsNoTracking()
       .ToListAsync();
   ```

4. **Usa Transaction per operazioni multiple**
   ```csharp
   // ✅ CORRETTO
   using var transaction = await _context.Database.BeginTransactionAsync();
   try {
       // Operazioni multiple
       await transaction.CommitAsync();
   } catch {
       await transaction.RollbackAsync();
   }
   ```

### ❌ Cosa Evitare

1. **Non usare ToList() prima di filtrare**
   ```csharp
   // ❌ SBAGLIATO
   var tutti = await _context.Personas.ToListAsync();
   var filtrati = tutti.Where(p => p.Eta > 18).ToList();
   
   // ✅ CORRETTO
   var filtrati = await _context.Personas
       .Where(p => p.Eta > 18)
       .ToListAsync();
   ```

2. **Non fare N+1 queries**
   ```csharp
   // ❌ SBAGLIATO
   var persone = await _context.Personas.ToListAsync();
   foreach (var persona in persone) {
       var ordini = persona.Ordini;  // Query per ogni persona!
   }
   
   // ✅ CORRETTO
   var persone = await _context.Personas
       .Include(p => p.Ordini)
       .ToListAsync();
   ```

---

## 11. Domande Frequenti (FAQ)

### Q: Come cambio il database provider?
**R:** Cambia il metodo `UseSqlServer()` con `UseSqlite()`, `UsePostgreSQL()`, ecc.

### Q: Come faccio rollback di una migration?
**R:** Usa `dotnet ef database update NomeMigrationPrecedente`.

### Q: Come ottimizzo le query lente?
**R:** Usa `AsNoTracking()`, evita `ToList()` prima di filtrare, usa indici.

### Q: Come gestisco le transazioni?
**R:** Usa `BeginTransactionAsync()` o `SaveChanges()` per transazioni automatiche.

---

## Conclusioni

Entity Framework Core è essenziale per:

- ✅ Sviluppo più veloce con meno codice SQL
- ✅ Type-safe queries con LINQ
- ✅ Gestione automatica delle relazioni
- ✅ Migrations per gestione schema
- ✅ Supporto multipli database

EF Core è lo standard de facto per l'accesso ai dati in .NET.

---

*Documento creato per spiegare Entity Framework Core con esempi pratici e diagrammi.*

