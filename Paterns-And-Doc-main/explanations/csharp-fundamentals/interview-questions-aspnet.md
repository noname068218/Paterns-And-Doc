# Domande e Risposte per Colloqui - ASP.NET Core

## 1. Cos'è ASP.NET Core e quali sono i suoi vantaggi?

**Risposta:**
ASP.NET Core è un framework cross-platform per costruire applicazioni web moderne.

**Vantaggi:**
- Cross-platform (Windows, Linux, macOS)
- Open-source
- Alta performance
- Modulare e leggero
- Supporto per Docker e microservizi
- Dependency Injection integrato
- Middleware pipeline flessibile

---

## 2. Spiega il ciclo di vita di una richiesta HTTP in ASP.NET Core.

**Risposta:**

1. **Request arriva** al server
2. **Middleware Pipeline** processa la richiesta (in ordine)
3. **Routing** determina il controller/action
4. **Model Binding** mappa i dati della richiesta
5. **Action Execution** esegue il metodo del controller
6. **Result Execution** genera la risposta
7. **Middleware Pipeline** processa la risposta (ordine inverso)
8. **Response** inviata al client

**Middleware Pipeline:**
```csharp
app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());
```

---

## 3. Cos'è il Middleware e come funziona?

**Risposta:**
Il Middleware è codice che viene eseguito nella pipeline di richiesta/risposta.

```csharp
// Middleware personalizzato
public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation("Request: {Method} {Path}", 
            context.Request.Method, context.Request.Path);
        
        await _next(context); // Passa al prossimo middleware
        
        _logger.LogInformation("Response: {StatusCode}", 
            context.Response.StatusCode);
    }
}

// Registrazione
app.UseMiddleware<LoggingMiddleware>();
```

**Ordine importante:**
- Middleware eseguiti nell'ordine di registrazione per le richieste
- Ordine inverso per le risposte

---

## 4. Spiega Dependency Injection in ASP.NET Core.

**Risposta:**
ASP.NET Core ha DI integrato nel container.

**Lifetime:**
- **Transient**: Nuova istanza ogni volta
- **Scoped**: Una istanza per scope (richiesta HTTP)
- **Singleton**: Una istanza per tutta l'applicazione

```csharp
// Registrazione
services.AddTransient<IEmailService, EmailService>();
services.AddScoped<IOrderService, OrderService>();
services.AddSingleton<IConfiguration, Configuration>();

// Iniezione
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }
}
```

**Best Practices:**
- Usa interfacce per le dipendenze
- Evita Singleton per servizi con stato
- Usa Scoped per servizi che usano DbContext

---

## 5. Cos'è il Model Binding?

**Risposta:**
Il Model Binding mappa automaticamente i dati della richiesta HTTP a parametri del metodo.

```csharp
[HttpPost]
public IActionResult Create([FromBody] CreateUserRequest request)
{
    // request è popolato automaticamente dal body JSON
    return Ok();
}

public class CreateUserRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
}
```

**Binding Sources:**
- `[FromBody]`: Request body (JSON, XML)
- `[FromQuery]`: Query string
- `[FromRoute]`: Route parameters
- `[FromForm]`: Form data
- `[FromHeader]`: Headers

---

## 6. Spiega le Action Results.

**Risposta:**
Action Results rappresentano la risposta HTTP.

```csharp
// Ok (200)
return Ok(data);

// Created (201)
return CreatedAtAction(nameof(Get), new { id }, data);

// BadRequest (400)
return BadRequest(errorMessage);

// NotFound (404)
return NotFound();

// Unauthorized (401)
return Unauthorized();

// View (HTML)
return View(model);

// JSON
return Json(data);
```

**IActionResult:**
```csharp
public IActionResult GetUser(int id)
{
    var user = _userService.GetById(id);
    if (user == null)
        return NotFound();
    return Ok(user);
}
```

---

## 7. Cos'è Entity Framework Core?

**Risposta:**
EF Core è un ORM (Object-Relational Mapper) per .NET.

**Vantaggi:**
- Code-first o Database-first
- LINQ queries
- Migrations
- Change tracking
- Lazy/Eager loading

**Esempio:**
```csharp
public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
}

// Utilizzo
var users = await _context.Users
    .Where(u => u.Age > 18)
    .ToListAsync();
```

**Migrations:**
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## 8. Spiega l'Autenticazione e Autorizzazione.

**Risposta:**

**Autenticazione:** Verifica chi è l'utente
**Autorizzazione:** Verifica cosa può fare l'utente

**JWT Authentication:**
```csharp
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
        };
    });
```

**Autorizzazione:**
```csharp
[Authorize]
public class OrderController : ControllerBase
{
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id) { }
}
```

---

## 9. Cos'è il Routing?

**Risposta:**
Il Routing mappa le richieste HTTP ai controller actions.

**Convention-based:**
```csharp
// /api/users/1 -> UsersController.Get(1)
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet("{id}")]
    public IActionResult Get(int id) { }
}
```

**Attribute Routing:**
```csharp
[Route("api/v1/users")]
public class UsersController : ControllerBase
{
    [HttpGet("{id:int}")]
    public IActionResult GetUser(int id) { }
}
```

---

## 10. Spiega la Gestione degli Errori.

**Risposta:**

**Exception Middleware:**
```csharp
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        
        var exception = context.Features.Get<IExceptionHandlerFeature>();
        var response = new { error = exception.Error.Message };
        
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    });
});
```

**Custom Exception:**
```csharp
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

// In controller
try
{
    var user = _userService.GetById(id);
    if (user == null)
        throw new NotFoundException("User not found");
}
catch (NotFoundException ex)
{
    return NotFound(ex.Message);
}
```

---

## 11. Cos'è CORS e come configurarlo?

**Risposta:**
CORS (Cross-Origin Resource Sharing) permette richieste da domini diversi.

```csharp
services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("https://example.com")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

app.UseCors("AllowSpecificOrigin");
```

---

## 12. Spiega la Configurazione in ASP.NET Core.

**Risposta:**
La configurazione usa il pattern Options.

**appsettings.json:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=..."
  },
  "Jwt": {
    "Key": "secret-key",
    "Issuer": "myapp.com"
  }
}
```

**Utilizzo:**
```csharp
public class JwtSettings
{
    public string Key { get; set; }
    public string Issuer { get; set; }
}

services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

// Iniezione
public class AuthService
{
    private readonly JwtSettings _jwtSettings;
    
    public AuthService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }
}
```

---

## 13. Cos'è il Logging?

**Risposta:**
ASP.NET Core ha un sistema di logging integrato.

```csharp
public class OrderService
{
    private readonly ILogger<OrderService> _logger;
    
    public OrderService(ILogger<OrderService> logger)
    {
        _logger = logger;
    }
    
    public void ProcessOrder(Order order)
    {
        _logger.LogInformation("Processing order {OrderId}", order.Id);
        _logger.LogError("Error processing order: {Error}", ex.Message);
    }
}
```

**Livelli:**
- Trace, Debug, Information, Warning, Error, Critical

---

## 14. Spiega le API RESTful.

**Risposta:**
REST (Representational State Transfer) è uno stile architetturale per API.

**Principi:**
- Stateless
- Resource-based URLs
- HTTP methods (GET, POST, PUT, DELETE)
- JSON/XML come formato dati

**Esempio:**
```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet]           // GET /api/users
    public IActionResult GetAll() { }
    
    [HttpGet("{id}")]   // GET /api/users/1
    public IActionResult Get(int id) { }
    
    [HttpPost]          // POST /api/users
    public IActionResult Create([FromBody] User user) { }
    
    [HttpPut("{id}")]   // PUT /api/users/1
    public IActionResult Update(int id, [FromBody] User user) { }
    
    [HttpDelete("{id}")] // DELETE /api/users/1
    public IActionResult Delete(int id) { }
}
```

---

## 15. Cos'è Swagger/OpenAPI?

**Risposta:**
Swagger fornisce documentazione interattiva per API.

```csharp
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "My API", 
        Version = "v1" 
    });
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});
```

---

*Documento creato per la preparazione ai colloqui tecnici - ASP.NET Core*

