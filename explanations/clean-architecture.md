# Clean Architecture in C#

## Introduzione

**Clean Architecture** (nota anche come Onion Architecture o Hexagonal Architecture) è un pattern architetturale che separa le responsabilità dell'applicazione in livelli concentrici, con dipendenze che puntano verso l'interno. Questo crea un sistema indipendente da framework, UI e database esterni.

---

## 1. Principi della Clean Architecture

### Regola della Dipendenza

Le dipendenze devono puntare **solo verso l'interno**. I livelli esterni dipendono dai livelli interni, mai il contrario.

### Diagramma: Livelli Concentrici

```
┌─────────────────────────────────────────────┐
│  Framework & Drivers                         │
│  (UI, Web, DB, External Services)           │
└─────────────────────────────────────────────┘
                    │
                    ▼ (dipende da)
┌─────────────────────────────────────────────┐
│  Interface Adapters                          │
│  (Controllers, Presenters, Gateways)       │
└─────────────────────────────────────────────┘
                    │
                    ▼ (dipende da)
┌─────────────────────────────────────────────┐
│  Application Business Rules                 │
│  (Use Cases, Application Services)          │
└─────────────────────────────────────────────┘
                    │
                    ▼ (dipende da)
┌─────────────────────────────────────────────┐
│  Enterprise Business Rules                  │
│  (Entities, Domain Models)                 │
└─────────────────────────────────────────────┘

Dipendenze: Solo verso l'interno!
```

---

## 2. Struttura dei Livelli

### Livelli Principali

```
CleanArchitecture/
├── Domain/              (Enterprise Business Rules)
│   ├── Entities/
│   ├── ValueObjects/
│   ├── Interfaces/
│   └── Exceptions/
├── Application/         (Application Business Rules)
│   ├── UseCases/
│   ├── DTOs/
│   ├── Interfaces/
│   └── Mappings/
├── Infrastructure/      (Framework & Drivers)
│   ├── Data/
│   ├── Repositories/
│   ├── ExternalServices/
│   └── Configurations/
└── Presentation/        (Interface Adapters)
    ├── Controllers/
    ├── ViewModels/
    └── Views/
```

### Diagramma: Struttura Livelli

```
┌─────────────────────────────────────────────┐
│  PRESENTATION                               │
│  - Controllers                              │
│  - ViewModels                               │
│  - Views                                    │
└─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────┐
│  APPLICATION                                │
│  - Use Cases                                │
│  - DTOs                                     │
│  - Application Services                     │
└─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────┐
│  DOMAIN                                     │
│  - Entities                                │
│  - Value Objects                           │
│  - Domain Services                          │
└─────────────────────────────────────────────┘
                    ▲
                    │
┌─────────────────────────────────────────────┐
│  INFRASTRUCTURE                             │
│  - Repositories                             │
│  - External Services                        │
│  - Database                                 │
└─────────────────────────────────────────────┘
```

---

## 3. Domain Layer (Livello Dominio)

### Definizione

Il **Domain Layer** contiene le entità di business e le regole aziendali. È il cuore dell'applicazione e non dipende da nessun altro layer.

### Entity

```csharp
// Domain/Entities/Persona.cs
namespace Domain.Entities;

public class Persona {
    public int Id { get; private set; }
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public DateTime DataNascita { get; private set; }
    
    // Costruttore
    private Persona() { }  // Per EF Core
    
    public Persona(string nome, string email, DateTime dataNascita) {
        if (string.IsNullOrEmpty(nome)) {
            throw new DomainException("Nome non può essere vuoto");
        }
        
        if (!IsValidEmail(email)) {
            throw new DomainException("Email non valida");
        }
        
        Nome = nome;
        Email = email;
        DataNascita = dataNascita;
    }
    
    // Metodi di business
    public void CambiaNome(string nuovoNome) {
        if (string.IsNullOrEmpty(nuovoNome)) {
            throw new DomainException("Nome non può essere vuoto");
        }
        Nome = nuovoNome;
    }
    
    public int CalcolaEta() {
        return DateTime.UtcNow.Year - DataNascita.Year;
    }
    
    private bool IsValidEmail(string email) {
        return email.Contains("@") && email.Contains(".");
    }
}
```

### Value Object

```csharp
// Domain/ValueObjects/Email.cs
namespace Domain.ValueObjects;

public class Email {
    public string Value { get; private set; }
    
    private Email() { }
    
    public Email(string value) {
        if (!IsValid(value)) {
            throw new DomainException("Email non valida");
        }
        Value = value;
    }
    
    private bool IsValid(string email) {
        return !string.IsNullOrEmpty(email) && 
               email.Contains("@") && 
               email.Contains(".");
    }
    
    public override bool Equals(object obj) {
        if (obj is Email email) {
            return Value == email.Value;
        }
        return false;
    }
    
    public override int GetHashCode() {
        return Value.GetHashCode();
    }
}
```

### Domain Service

```csharp
// Domain/Services/PersonaDomainService.cs
namespace Domain.Services;

public interface IPersonaDomainService {
    bool IsEmailUnico(string email);
}

public class PersonaDomainService : IPersonaDomainService {
    private readonly IPersonaRepository _repository;
    
    public PersonaDomainService(IPersonaRepository repository) {
        _repository = repository;
    }
    
    public bool IsEmailUnico(string email) {
        return _repository.GetByEmail(email) == null;
    }
}
```

### Domain Interface (Repository)

```csharp
// Domain/Interfaces/Repositories/IPersonaRepository.cs
namespace Domain.Interfaces.Repositories;

public interface IPersonaRepository {
    Task<Persona> GetByIdAsync(int id);
    Task<Persona> GetByEmailAsync(string email);
    Task<List<Persona>> GetAllAsync();
    Task<int> SaveAsync(Persona persona);
    Task DeleteAsync(int id);
}
```

---

## 4. Application Layer (Livello Applicazione)

### Definizione

Il **Application Layer** contiene la logica di applicazione e i use cases. Dipende solo dal Domain Layer.

### Use Case (Command)

```csharp
// Application/UseCases/Persona/CreatePersonaUseCase.cs
namespace Application.UseCases.Persona;

public class CreatePersonaCommand {
    public string Nome { get; set; }
    public string Email { get; set; }
    public DateTime DataNascita { get; set; }
}

public class CreatePersonaUseCase {
    private readonly IPersonaRepository _repository;
    private readonly IPersonaDomainService _domainService;
    
    public CreatePersonaUseCase(
        IPersonaRepository repository,
        IPersonaDomainService domainService) {
        _repository = repository;
        _domainService = domainService;
    }
    
    public async Task<int> ExecuteAsync(CreatePersonaCommand command) {
        // Validazione business
        if (!await _domainService.IsEmailUnico(command.Email)) {
            throw new ApplicationException("Email già esistente");
        }
        
        // Creazione entità
        var persona = new Domain.Entities.Persona(
            command.Nome,
            command.Email,
            command.DataNascita
        );
        
        // Salvataggio
        return await _repository.SaveAsync(persona);
    }
}
```

### DTO (Data Transfer Object)

```csharp
// Application/DTOs/PersonaDto.cs
namespace Application.DTOs;

public class PersonaDto {
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public int Eta { get; set; }
}

public class PersonaListDto {
    public int Id { get; set; }
    public string Nome { get; set; }
}
```

### Use Case (Query)

```csharp
// Application/UseCases/Persona/GetPersonaByIdUseCase.cs
namespace Application.UseCases.Persona;

public class GetPersonaByIdQuery {
    public int Id { get; set; }
}

public class GetPersonaByIdUseCase {
    private readonly IPersonaRepository _repository;
    
    public GetPersonaByIdUseCase(IPersonaRepository repository) {
        _repository = repository;
    }
    
    public async Task<PersonaDto> ExecuteAsync(GetPersonaByIdQuery query) {
        var persona = await _repository.GetByIdAsync(query.Id);
        
        if (persona == null) {
            return null;
        }
        
        return new PersonaDto {
            Id = persona.Id,
            Nome = persona.Nome,
            Email = persona.Email,
            Eta = persona.CalcolaEta()
        };
    }
}
```

---

## 5. Infrastructure Layer (Livello Infrastruttura)

### Definizione

Il **Infrastructure Layer** implementa le interfacce definite nel Domain e Application Layer. Contiene l'accesso ai dati, servizi esterni, ecc.

### Repository Implementation

```csharp
// Infrastructure/Data/Repositories/PersonaRepository.cs
namespace Infrastructure.Data.Repositories;

public class PersonaRepository : IPersonaRepository {
    private readonly ApplicationDbContext _context;
    
    public PersonaRepository(ApplicationDbContext context) {
        _context = context;
    }
    
    public async Task<Domain.Entities.Persona> GetByIdAsync(int id) {
        return await _context.Personas.FindAsync(id);
    }
    
    public async Task<Domain.Entities.Persona> GetByEmailAsync(string email) {
        return await _context.Personas
            .FirstOrDefaultAsync(p => p.Email == email);
    }
    
    public async Task<List<Domain.Entities.Persona>> GetAllAsync() {
        return await _context.Personas.ToListAsync();
    }
    
    public async Task<int> SaveAsync(Domain.Entities.Persona persona) {
        if (persona.Id == 0) {
            _context.Personas.Add(persona);
        } else {
            _context.Personas.Update(persona);
        }
        
        await _context.SaveChangesAsync();
        return persona.Id;
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

### DbContext (Entity Framework)

```csharp
// Infrastructure/Data/ApplicationDbContext.cs
namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext {
    public DbSet<Domain.Entities.Persona> Personas { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        // Configurazioni
        modelBuilder.Entity<Domain.Entities.Persona>(entity => {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
        });
    }
}
```

### Dependency Injection

```csharp
// Infrastructure/DependencyInjection.cs
namespace Infrastructure;

public static class DependencyInjection {
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration) {
        
        // Database
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        // Repositories
        services.AddScoped<IPersonaRepository, PersonaRepository>();
        
        // Domain Services
        services.AddScoped<IPersonaDomainService, PersonaDomainService>();
        
        return services;
    }
}
```

---

## 6. Presentation Layer (Livello Presentazione)

### Definizione

Il **Presentation Layer** contiene i controller, view models e le viste. Dipende dall'Application Layer.

### Controller

```csharp
// Presentation/Controllers/PersonaController.cs
namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonaController : ControllerBase {
    private readonly CreatePersonaUseCase _createUseCase;
    private readonly GetPersonaByIdUseCase _getByIdUseCase;
    
    public PersonaController(
        CreatePersonaUseCase createUseCase,
        GetPersonaByIdUseCase getByIdUseCase) {
        _createUseCase = createUseCase;
        _getByIdUseCase = getByIdUseCase;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreatePersonaCommand command) {
        try {
            var id = await _createUseCase.ExecuteAsync(command);
            return Ok(new { Id = id });
        }
        catch (ApplicationException ex) {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id) {
        var query = new GetPersonaByIdQuery { Id = id };
        var persona = await _getByIdUseCase.ExecuteAsync(query);
        
        if (persona == null) {
            return NotFound();
        }
        
        return Ok(persona);
    }
}
```

### Dependency Injection

```csharp
// Presentation/DependencyInjection.cs
namespace Presentation;

public static class DependencyInjection {
    public static IServiceCollection AddPresentation(
        this IServiceCollection services) {
        
        // Use Cases
        services.AddScoped<CreatePersonaUseCase>();
        services.AddScoped<GetPersonaByIdUseCase>();
        
        return services;
    }
}
```

---

## 7. Configurazione Completa

### Program.cs / Startup.cs

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

// Application Layer
builder.Services.AddApplication();

// Infrastructure Layer
builder.Services.AddInfrastructure(builder.Configuration);

// Presentation Layer
builder.Services.AddPresentation();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

### Diagramma: Flusso Completo

```
┌─────────────────────────────────────────────┐
│  HTTP Request                               │
└─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────┐
│  Controller (Presentation)                  │
│  - Riceve request                           │
│  - Chiama Use Case                          │
└─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────┐
│  Use Case (Application)                     │
│  - Valida                                    │
│  - Crea Entity                               │
│  - Chiama Repository                         │
└─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────┐
│  Repository (Infrastructure)                  │
│  - Implementa Domain Interface               │
│  - Accede a Database                         │
└─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────┐
│  Entity (Domain)                             │
│  - Business Logic                            │
│  - Regole di dominio                         │
└─────────────────────────────────────────────┘
```

---

## 8. Vantaggi della Clean Architecture

### Tabella Riepilogativa

| Vantaggio | Descrizione |
|-----------|-------------|
| **Indipendenza** | Indipendente da framework, UI, database |
| **Testabilità** | Facile testare ogni layer isolatamente |
| **Manutenibilità** | Cambiamenti isolati per layer |
| **Flessibilità** | Facile cambiare implementazioni |
| **Business Logic** | Business logic al centro, protetta |

### Diagramma: Vantaggi

```
┌─────────────────────────────────────────────┐
│  Indipendenza                                │
│  - Framework agnostic                      │
│  - UI intercambiabile                       │
│  - Database intercambiabile                 │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  Testabilità                                 │
│  - Test isolati per layer                   │
│  - Mock facili                              │
│  - Test di business logic                  │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  Manutenibilità                              │
│  - Cambiamenti isolati                     │
│  - Codice organizzato                      │
│  - Facile da capire                        │
└─────────────────────────────────────────────┘
```

---

## 9. Best Practices

### ✅ Cosa Fare

1. **Mantieni il Domain Layer puro**
   ```csharp
   // ✅ CORRETTO - Nessuna dipendenza esterna
   public class Persona {
       // Solo business logic
   }
   ```

2. **Usa interfacce nel Domain**
   ```csharp
   // ✅ CORRETTO
   public interface IPersonaRepository {
       Task<Persona> GetByIdAsync(int id);
   }
   ```

3. **DTOs nel Application Layer**
   ```csharp
   // ✅ CORRETTO
   public class PersonaDto {
       // Solo dati necessari
   }
   ```

### ❌ Cosa Evitare

1. **Non fare dipendere Domain da altri layer**
   ```csharp
   // ❌ SBAGLIATO
   using Infrastructure.Data;  // NO!
   ```

2. **Non mettere business logic nei controller**
   ```csharp
   // ❌ SBAGLIATO
   [HttpPost]
   public IActionResult Create(CreatePersonaCommand command) {
       // Business logic qui - NO!
       if (string.IsNullOrEmpty(command.Nome)) {
           // ...
       }
   }
   ```

---

## 10. Domande Frequenti (FAQ)

### Q: Clean Architecture è troppo complessa per progetti piccoli?
**R:** Sì, per progetti molto piccoli può essere over-engineering. Usala per progetti di media/grande dimensione.

### Q: Quanti layer devo creare?
**R:** Almeno Domain, Application e Infrastructure. Presentation è opzionale per API.

### Q: Devo usare CQRS con Clean Architecture?
**R:** No, ma si integrano bene insieme.

### Q: Come gestisco le transazioni?
**R:** Nel Application Layer o Infrastructure Layer, usando Unit of Work pattern.

---

## Conclusioni

Clean Architecture è utile per:

- ✅ Creare applicazioni indipendenti da framework
- ✅ Proteggere la business logic
- ✅ Facilitare i test
- ✅ Migliorare la manutenibilità
- ✅ Permettere cambiamenti flessibili

Usa Clean Architecture per progetti complessi che richiedono manutenibilità e testabilità a lungo termine.

---

*Documento creato per spiegare Clean Architecture con esempi pratici e diagrammi.*

