# ASP.NET Web API

## Introduzione

**ASP.NET Web API** è un framework Microsoft per costruire API HTTP RESTful. Permette di creare servizi web che possono essere consumati da una varietà di client, inclusi browser, applicazioni mobili e altri servizi.

---

## 1. Cos'è ASP.NET Web API?

### Definizione

ASP.NET Web API è un framework per costruire servizi HTTP che possono essere utilizzati da vari client, supportando:
- **RESTful APIs**
- **JSON e XML**
- **Routing flessibile**
- **Content Negotiation**
- **Model Binding e Validation**

### Caratteristiche Principali

- ✅ **RESTful**: Supporta i principi REST
- ✅ **HTTP-based**: Usa i metodi HTTP standard (GET, POST, PUT, DELETE)
- ✅ **Cross-platform**: Funziona su .NET, .NET Core, .NET Framework
- ✅ **JSON/XML**: Supporta multiple formati di serializzazione
- ✅ **Routing**: Sistema di routing flessibile

### Diagramma: Architettura Web API

```
┌─────────────────────────────────────────────┐
│  Client (Browser, Mobile, etc.)             │
└─────────────────────────────────────────────┘
                    │
                    │ HTTP Request
                    │ (GET, POST, PUT, DELETE)
                    ▼
┌─────────────────────────────────────────────┐
│  ASP.NET Web API                            │
│  - Routing                                  │
│  - Controller                               │
│  - Model Binding                            │
│  - Action Result                            │
└─────────────────────────────────────────────┘
                    │
                    │ Business Logic
                    ▼
┌─────────────────────────────────────────────┐
│  Service Layer / Repository                  │
└─────────────────────────────────────────────┘
                    │
                    │ Data Access
                    ▼
┌─────────────────────────────────────────────┐
│  Database                                    │
└─────────────────────────────────────────────┘
```

---

## 2. Creazione di un Web API

### Setup Progetto

```bash
# Crea nuovo progetto Web API
dotnet new webapi -n MyWebAPI

# Oppure con .NET Framework
# File -> New Project -> ASP.NET Web Application -> Web API
```

### Struttura Base

```
MyWebAPI/
├── Controllers/
│   └── PersonaController.cs
├── Models/
│   └── Persona.cs
├── Program.cs (o Startup.cs)
└── appsettings.json
```

### Program.cs

```csharp
// Program.cs (.NET 6+)
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

---

## 3. Controller e Actions

### Controller Base

```csharp
// Controllers/PersonaController.cs
using Microsoft.AspNetCore.Mvc;

namespace MyWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonaController : ControllerBase {
    
    private readonly List<Persona> _persone = new();
    
    // GET: api/persona
    [HttpGet]
    public ActionResult<IEnumerable<Persona>> GetAll() {
        return Ok(_persone);
    }
    
    // GET: api/persona/5
    [HttpGet("{id}")]
    public ActionResult<Persona> GetById(int id) {
        var persona = _persone.FirstOrDefault(p => p.Id == id);
        
        if (persona == null) {
            return NotFound();
        }
        
        return Ok(persona);
    }
    
    // POST: api/persona
    [HttpPost]
    public ActionResult<Persona> Create(Persona persona) {
        persona.Id = _persone.Count + 1;
        _persone.Add(persona);
        
        return CreatedAtAction(nameof(GetById), new { id = persona.Id }, persona);
    }
    
    // PUT: api/persona/5
    [HttpPut("{id}")]
    public IActionResult Update(int id, Persona persona) {
        var existing = _persone.FirstOrDefault(p => p.Id == id);
        
        if (existing == null) {
            return NotFound();
        }
        
        existing.Nome = persona.Nome;
        existing.Email = persona.Email;
        
        return NoContent();
    }
    
    // DELETE: api/persona/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id) {
        var persona = _persone.FirstOrDefault(p => p.Id == id);
        
        if (persona == null) {
            return NotFound();
        }
        
        _persone.Remove(persona);
        return NoContent();
    }
}
```

### Model

```csharp
// Models/Persona.cs
namespace MyWebAPI.Models;

public class Persona {
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public int Eta { get; set; }
}
```

### Diagramma: Controller Flow

```
┌─────────────────────────────────────────────┐
│  HTTP Request                               │
│  GET /api/persona/1                         │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Routing              │
        │  Trova controller     │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  PersonaController    │
        │  GetById(1)           │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Business Logic       │
        │  Trova persona        │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  HTTP Response        │
        │  200 OK + JSON        │
        └───────────────────────┘
```

---

## 4. Routing

### Attribute Routing

```csharp
[ApiController]
[Route("api/[controller]")]
public class PersonaController : ControllerBase {
    
    // GET api/persona
    [HttpGet]
    public IActionResult GetAll() { }
    
    // GET api/persona/5
    [HttpGet("{id}")]
    public IActionResult GetById(int id) { }
    
    // GET api/persona/search?nome=Mario
    [HttpGet("search")]
    public IActionResult Search(string nome) { }
    
    // POST api/persona
    [HttpPost]
    public IActionResult Create(Persona persona) { }
    
    // PUT api/persona/5
    [HttpPut("{id}")]
    public IActionResult Update(int id, Persona persona) { }
    
    // DELETE api/persona/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id) { }
}
```

### Custom Routes

```csharp
// Route personalizzata
[Route("api/v1/[controller]")]
public class PersonaController : ControllerBase { }

// Route con constraint
[HttpGet("{id:int}")]  // Solo interi
public IActionResult GetById(int id) { }

[HttpGet("{id:guid}")]  // Solo GUID
public IActionResult GetById(Guid id) { }

// Route con parametri opzionali
[HttpGet("{id?}")]  // Opzionale
public IActionResult GetById(int? id) { }
```

### Diagramma: Routing

```
┌─────────────────────────────────────────────┐
│  HTTP Request                               │
│  GET /api/persona/123                       │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Route Matching        │
        │  - Controller: Persona │
        │  - Action: GetById     │
        │  - Parameter: id=123   │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  PersonaController    │
        │  GetById(123)          │
        └───────────────────────┘
```

---

## 5. HTTP Methods e Status Codes

### Metodi HTTP Standard

```csharp
[ApiController]
[Route("api/[controller]")]
public class PersonaController : ControllerBase {
    
    // GET - Recuperare risorse
    [HttpGet]
    public IActionResult GetAll() {
        return Ok(_persone);  // 200 OK
    }
    
    // POST - Creare nuova risorsa
    [HttpPost]
    public IActionResult Create(Persona persona) {
        _persone.Add(persona);
        return CreatedAtAction(nameof(GetById), 
            new { id = persona.Id }, persona);  // 201 Created
    }
    
    // PUT - Aggiornare risorsa (sostituzione completa)
    [HttpPut("{id}")]
    public IActionResult Update(int id, Persona persona) {
        // Aggiorna completamente
        return NoContent();  // 204 No Content
    }
    
    // PATCH - Aggiornare risorsa (parziale)
    [HttpPatch("{id}")]
    public IActionResult PartialUpdate(int id, JsonPatchDocument<Persona> patch) {
        // Aggiorna parzialmente
        return NoContent();
    }
    
    // DELETE - Eliminare risorsa
    [HttpDelete("{id}")]
    public IActionResult Delete(int id) {
        _persone.RemoveAll(p => p.Id == id);
        return NoContent();  // 204 No Content
    }
}
```

### Status Codes

```csharp
public class PersonaController : ControllerBase {
    
    [HttpGet("{id}")]
    public IActionResult GetById(int id) {
        var persona = _persone.FirstOrDefault(p => p.Id == id);
        
        if (persona == null) {
            return NotFound();  // 404
        }
        
        return Ok(persona);  // 200
    }
    
    [HttpPost]
    public IActionResult Create(Persona persona) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);  // 400
        }
        
        _persone.Add(persona);
        return CreatedAtAction(nameof(GetById), 
            new { id = persona.Id }, persona);  // 201
    }
    
    [HttpPut("{id}")]
    public IActionResult Update(int id, Persona persona) {
        if (id != persona.Id) {
            return BadRequest();  // 400
        }
        
        var existing = _persone.FirstOrDefault(p => p.Id == id);
        if (existing == null) {
            return NotFound();  // 404
        }
        
        // Aggiorna
        return NoContent();  // 204
    }
}
```

### Tabella: Status Codes Comuni

| Status Code | Significato | Quando Usare |
|-------------|-------------|--------------|
| **200 OK** | Successo | GET, PUT, PATCH conbody |
| **201 Created** | Risorsa creata | POST con successo |
| **204 No Content** | Successo senza body | PUT, DELETE, PATCH |
| **400 Bad Request** | Richiesta errata | Validazione fallita |
| **401 Unauthorized** | Non autenticato | Autenticazione richiesta |
| **403 Forbidden** | Non autorizzato | Permessi insufficienti |
| **404 Not Found** | Risorsa non trovata | Risorsa inesistente |
| **500 Internal Server Error** | Errore server | Errore generico |

---

## 6. Model Binding e Validation

### Model Binding

```csharp
// Binding da query string
[HttpGet("search")]
public IActionResult Search([FromQuery] string nome) {
    // GET /api/persona/search?nome=Mario
}

// Binding da route
[HttpGet("{id}")]
public IActionResult GetById([FromRoute] int id) {
    // GET /api/persona/123
}

// Binding da body
[HttpPost]
public IActionResult Create([FromBody] Persona persona) {
    // POST /api/persona
    // Body: { "nome": "Mario", "email": "mario@example.com" }
}

// Binding da form
[HttpPost("upload")]
public IActionResult Upload([FromForm] IFormFile file) {
    // POST /api/persona/upload (multipart/form-data)
}

// Binding da header
[HttpGet("info")]
public IActionResult GetInfo([FromHeader] string authorization) {
    // GET /api/persona/info
    // Header: Authorization: Bearer token
}
```

### Data Annotations Validation

```csharp
// Models/Persona.cs
using System.ComponentModel.DataAnnotations;

public class Persona {
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Nome è obbligatorio")]
    [StringLength(100, MinimumLength = 2)]
    public string Nome { get; set; }
    
    [Required]
    [EmailAddress(ErrorMessage = "Email non valida")]
    public string Email { get; set; }
    
    [Range(0, 150, ErrorMessage = "Età deve essere tra 0 e 150")]
    public int Eta { get; set; }
    
    [Url]
    public string Website { get; set; }
}

// Controller con validazione automatica
[ApiController]  // Abilita validazione automatica
[Route("api/[controller]")]
public class PersonaController : ControllerBase {
    
    [HttpPost]
    public IActionResult Create(Persona persona) {
        // Validazione automatica se [ApiController]
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        
        // Salva persona
        return CreatedAtAction(nameof(GetById), new { id = persona.Id }, persona);
    }
}
```

### Diagramma: Model Binding

```
┌─────────────────────────────────────────────┐
│  HTTP Request                               │
│  POST /api/persona                          │
│  Body: { "nome": "Mario", "email": "..." } │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Model Binding        │
        │  - Deserializza JSON  │
        │  - Mappa a Persona    │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Validation           │
        │  - Required           │
        │  - EmailAddress        │
        │  - Range              │
        └───────────────────────┘
                    │
        ┌───────────┴───────────┐
        │                       │
        ▼                       ▼
┌───────────┐          ┌───────────┐
│  Valido   │          │  Non      │
│  → OK     │          │  Valido   │
│           │          │  → 400    │
└───────────┘          └───────────┘
```

---

## 7. Dependency Injection

### Setup

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Registra servizi
builder.Services.AddScoped<IPersonaService, PersonaService>();
builder.Services.AddScoped<IPersonaRepository, PersonaRepository>();

builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();
app.Run();
```

### Service

```csharp
// Services/IPersonaService.cs
public interface IPersonaService {
    Task<List<Persona>> GetAllAsync();
    Task<Persona> GetByIdAsync(int id);
    Task<int> CreateAsync(Persona persona);
    Task UpdateAsync(Persona persona);
    Task DeleteAsync(int id);
}

// Services/PersonaService.cs
public class PersonaService : IPersonaService {
    private readonly IPersonaRepository _repository;
    
    public PersonaService(IPersonaRepository repository) {
        _repository = repository;
    }
    
    public async Task<List<Persona>> GetAllAsync() {
        return await _repository.GetAllAsync();
    }
    
    public async Task<Persona> GetByIdAsync(int id) {
        return await _repository.GetByIdAsync(id);
    }
    
    public async Task<int> CreateAsync(Persona persona) {
        // Business logic
        if (await _repository.EmailExistsAsync(persona.Email)) {
            throw new InvalidOperationException("Email già esistente");
        }
        
        return await _repository.CreateAsync(persona);
    }
    
    public async Task UpdateAsync(Persona persona) {
        await _repository.UpdateAsync(persona);
    }
    
    public async Task DeleteAsync(int id) {
        await _repository.DeleteAsync(id);
    }
}
```

### Controller con DI

```csharp
[ApiController]
[Route("api/[controller]")]
public class PersonaController : ControllerBase {
    private readonly IPersonaService _service;
    
    // Dependency Injection tramite costruttore
    public PersonaController(IPersonaService service) {
        _service = service;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Persona>>> GetAll() {
        var persone = await _service.GetAllAsync();
        return Ok(persone);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Persona>> GetById(int id) {
        var persona = await _service.GetByIdAsync(id);
        
        if (persona == null) {
            return NotFound();
        }
        
        return Ok(persona);
    }
    
    [HttpPost]
    public async Task<ActionResult<Persona>> Create(Persona persona) {
        try {
            var id = await _service.CreateAsync(persona);
            return CreatedAtAction(nameof(GetById), new { id }, persona);
        }
        catch (InvalidOperationException ex) {
            return BadRequest(ex.Message);
        }
    }
}
```

---

## 8. Middleware e Pipeline

### Middleware Pipeline

```csharp
// Program.cs
var app = builder.Build();

// Middleware order matters!
app.UseHttpsRedirection();      // 1. Redirect HTTP to HTTPS
app.UseAuthentication();        // 2. Authentication
app.UseAuthorization();         // 3. Authorization
app.UseRouting();               // 4. Routing
app.MapControllers();           // 5. Controllers
app.Run();                      // 6. Run
```

### Custom Middleware

```csharp
// Middleware/RequestLoggingMiddleware.cs
public class RequestLoggingMiddleware {
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;
    
    public RequestLoggingMiddleware(
        RequestDelegate next, 
        ILogger<RequestLoggingMiddleware> logger) {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context) {
        _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");
        
        await _next(context);
        
        _logger.LogInformation($"Response: {context.Response.StatusCode}");
    }
}

// Extension method
public static class RequestLoggingMiddlewareExtensions {
    public static IApplicationBuilder UseRequestLogging(
        this IApplicationBuilder builder) {
        return builder.UseMiddleware<RequestLoggingMiddleware>();
    }
}

// Program.cs
app.UseRequestLogging();
```

### Diagramma: Middleware Pipeline

```
┌─────────────────────────────────────────────┐
│  HTTP Request                               │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  UseHttpsRedirection  │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  UseAuthentication   │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  UseAuthorization     │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  UseRouting           │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  MapControllers       │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  HTTP Response        │
        └───────────────────────┘
```

---

## 9. Swagger/OpenAPI

### Setup Swagger

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "My API",
        Version = "v1",
        Description = "API per gestione persone"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.Run();
```

### Swagger Annotations

```csharp
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PersonaController : ControllerBase {
    
    /// <summary>
    /// Ottiene tutte le persone
    /// </summary>
    /// <returns>Lista di persone</returns>
    /// <response code="200">Ritorna la lista</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Persona>> GetAll() {
        return Ok(_persone);
    }
    
    /// <summary>
    /// Ottiene una persona per ID
    /// </summary>
    /// <param name="id">ID della persona</param>
    /// <returns>Persona</returns>
    /// <response code="200">Persona trovata</response>
    /// <response code="404">Persona non trovata</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Persona> GetById(int id) {
        var persona = _persone.FirstOrDefault(p => p.Id == id);
        
        if (persona == null) {
            return NotFound();
        }
        
        return Ok(persona);
    }
}
```

---

## 10. Esempio Completo: API RESTful

### Struttura Completa

```csharp
// ========== MODEL ==========
public class Persona {
    public int Id { get; set; }
    [Required]
    public string Nome { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public int Eta { get; set; }
}

// ========== REPOSITORY ==========
public interface IPersonaRepository {
    Task<List<Persona>> GetAllAsync();
    Task<Persona> GetByIdAsync(int id);
    Task<int> CreateAsync(Persona persona);
    Task UpdateAsync(Persona persona);
    Task DeleteAsync(int id);
}

// ========== SERVICE ==========
public interface IPersonaService {
    Task<List<Persona>> GetAllAsync();
    Task<Persona> GetByIdAsync(int id);
    Task<int> CreateAsync(Persona persona);
    Task UpdateAsync(Persona persona);
    Task DeleteAsync(int id);
}

public class PersonaService : IPersonaService {
    private readonly IPersonaRepository _repository;
    
    public PersonaService(IPersonaRepository repository) {
        _repository = repository;
    }
    
    public async Task<List<Persona>> GetAllAsync() {
        return await _repository.GetAllAsync();
    }
    
    public async Task<Persona> GetByIdAsync(int id) {
        return await _repository.GetByIdAsync(id);
    }
    
    public async Task<int> CreateAsync(Persona persona) {
        return await _repository.CreateAsync(persona);
    }
    
    public async Task UpdateAsync(Persona persona) {
        await _repository.UpdateAsync(persona);
    }
    
    public async Task DeleteAsync(int id) {
        await _repository.DeleteAsync(id);
    }
}

// ========== CONTROLLER ==========
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PersonaController : ControllerBase {
    private readonly IPersonaService _service;
    
    public PersonaController(IPersonaService service) {
        _service = service;
    }
    
    // GET api/persona
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Persona>>> GetAll() {
        var persone = await _service.GetAllAsync();
        return Ok(persone);
    }
    
    // GET api/persona/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Persona>> GetById(int id) {
        var persona = await _service.GetByIdAsync(id);
        
        if (persona == null) {
            return NotFound();
        }
        
        return Ok(persona);
    }
    
    // POST api/persona
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Persona>> Create(Persona persona) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        
        var id = await _service.CreateAsync(persona);
        return CreatedAtAction(nameof(GetById), new { id }, persona);
    }
    
    // PUT api/persona/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, Persona persona) {
        if (id != persona.Id) {
            return BadRequest();
        }
        
        var existing = await _service.GetByIdAsync(id);
        if (existing == null) {
            return NotFound();
        }
        
        await _service.UpdateAsync(persona);
        return NoContent();
    }
    
    // DELETE api/persona/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id) {
        var persona = await _service.GetByIdAsync(id);
        if (persona == null) {
            return NotFound();
        }
        
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
```

---

## 11. Best Practices

### ✅ Cosa Fare

1. **Usa [ApiController] per validazione automatica**
   ```csharp
   [ApiController]
   [Route("api/[controller]")]
   public class PersonaController : ControllerBase { }
   ```

2. **Usa DTOs invece di Entity direttamente**
   ```csharp
   // ✅ CORRETTO
   public class PersonaDto {
       public int Id { get; set; }
       public string Nome { get; set; }
   }
   ```

3. **Usa async/await per operazioni I/O**
   ```csharp
   // ✅ CORRETTO
   public async Task<ActionResult<Persona>> GetById(int id) {
       var persona = await _service.GetByIdAsync(id);
       return Ok(persona);
   }
   ```

4. **Usa status codes appropriati**
   ```csharp
   // ✅ CORRETTO
   return CreatedAtAction(...);  // 201
   return NoContent();          // 204
   return NotFound();           // 404
   ```

### ❌ Cosa Evitare

1. **Non fare business logic nei controller**
   ```csharp
   // ❌ SBAGLIATO
   [HttpPost]
   public IActionResult Create(Persona persona) {
       // Business logic qui - NO!
       if (persona.Email.Contains("@domain.com")) {
           // ...
       }
   }
   ```

2. **Non esporre entità direttamente**
   ```csharp
   // ❌ SBAGLIATO
   public async Task<ActionResult<PersonaEntity>> GetById(int id) {
       // Espone entità interna
   }
   ```

---

## 12. Domande Frequenti (FAQ)

### Q: Qual è la differenza tra Web API e MVC?
**R:** Web API è per servizi HTTP/REST, MVC è per applicazioni web con viste HTML.

### Q: Come gestisco l'autenticazione?
**R:** Usa JWT tokens, Identity, o OAuth con middleware di autenticazione.

### Q: Come gestisco CORS?
**R:** Configura CORS in Program.cs con `AddCors()` e `UseCors()`.

### Q: Come serializzo in XML invece di JSON?
**R:** Aggiungi `AddXmlSerializerFormatters()` in `AddControllers()`.

---

## Conclusioni

ASP.NET Web API è essenziale per:

- ✅ Creare API RESTful
- ✅ Supportare multiple client
- ✅ Implementare architetture moderne
- ✅ Costruire microservizi

Web API è il framework standard per creare servizi HTTP in .NET.

---

*Documento creato per spiegare ASP.NET Web API con esempi pratici e diagrammi.*

