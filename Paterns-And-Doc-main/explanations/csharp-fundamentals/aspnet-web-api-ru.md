# ASP.NET Web API

## Введение

**ASP.NET Web API** — это фреймворк Microsoft для создания RESTful HTTP API. Он позволяет создавать веб-сервисы, которые могут использоваться различными клиентами, включая браузеры, мобильные приложения и другие сервисы.

---

## 1. Что такое ASP.NET Web API?

### Определение

ASP.NET Web API — это фреймворк для создания HTTP-сервисов, которые могут быть использованы различными клиентами, поддерживающий:
- **RESTful APIs**
- **JSON и XML**
- **Гибкую маршрутизацию**
- **Content Negotiation**
- **Model Binding и Validation**

### Основные характеристики

- ✅ **RESTful**: Поддерживает принципы REST
- ✅ **HTTP-based**: Использует стандартные HTTP-методы (GET, POST, PUT, DELETE)
- ✅ **Cross-platform**: Работает на .NET, .NET Core, .NET Framework
- ✅ **JSON/XML**: Поддерживает множество форматов сериализации
- ✅ **Routing**: Гибкая система маршрутизации

### Диаграмма: Архитектура Web API

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

## 2. Создание Web API

### Настройка проекта

```bash
# Create new Web API project
dotnet new webapi -n MyWebAPI

# Or with .NET Framework
# File -> New Project -> ASP.NET Web Application -> Web API
```

### Базовая структура

```
MyWebAPI/
├── Controllers/
│   └── PersonController.cs
├── Models/
│   └── Person.cs
├── Program.cs (or Startup.cs)
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

## 3. Контроллеры и действия

### Базовый контроллер

```csharp
// Controllers/PersonController.cs
using Microsoft.AspNetCore.Mvc;

namespace MyWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase {
    
    private readonly List<Person> _people = new();
    
    // GET: api/person
    [HttpGet]
    public ActionResult<IEnumerable<Person>> GetAll() {
        return Ok(_people);
    }
    
    // GET: api/person/5
    [HttpGet("{id}")]
    public ActionResult<Person> GetById(int id) {
        var person = _people.FirstOrDefault(p => p.Id == id);
        
        if (person == null) {
            return NotFound();
        }
        
        return Ok(person);
    }
    
    // POST: api/person
    [HttpPost]
    public ActionResult<Person> Create(Person person) {
        person.Id = _people.Count + 1;
        _people.Add(person);
        
        return CreatedAtAction(nameof(GetById), new { id = person.Id }, person);
    }
    
    // PUT: api/person/5
    [HttpPut("{id}")]
    public IActionResult Update(int id, Person person) {
        var existing = _people.FirstOrDefault(p => p.Id == id);
        
        if (existing == null) {
            return NotFound();
        }
        
        existing.Name = person.Name;
        existing.Email = person.Email;
        
        return NoContent();
    }
    
    // DELETE: api/person/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id) {
        var person = _people.FirstOrDefault(p => p.Id == id);
        
        if (person == null) {
            return NotFound();
        }
        
        _people.Remove(person);
        return NoContent();
    }
}
```

### Модель

```csharp
// Models/Person.cs
namespace MyWebAPI.Models;

public class Person {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
}
```

### Диаграмма: Поток контроллера

```
┌─────────────────────────────────────────────┐
│  HTTP Request                               │
│  GET /api/person/1                          │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Routing              │
        │  Find controller      │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  PersonController     │
        │  GetById(1)           │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Business Logic       │
        │  Find person          │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  HTTP Response        │
        │  200 OK + JSON        │
        └───────────────────────┘
```

---

## 4. Маршрутизация

### Attribute Routing

```csharp
[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase {
    
    // GET api/person
    [HttpGet]
    public IActionResult GetAll() { }
    
    // GET api/person/5
    [HttpGet("{id}")]
    public IActionResult GetById(int id) { }
    
    // GET api/person/search?name=John
    [HttpGet("search")]
    public IActionResult Search(string name) { }
    
    // POST api/person
    [HttpPost]
    public IActionResult Create(Person person) { }
    
    // PUT api/person/5
    [HttpPut("{id}")]
    public IActionResult Update(int id, Person person) { }
    
    // DELETE api/person/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id) { }
}
```

### Кастомные маршруты

```csharp
// Custom route
[Route("api/v1/[controller]")]
public class PersonController : ControllerBase { }

// Route with constraint
[HttpGet("{id:int}")]  // Only integers
public IActionResult GetById(int id) { }

[HttpGet("{id:guid}")]  // Only GUID
public IActionResult GetById(Guid id) { }

// Route with optional parameters
[HttpGet("{id?}")]  // Optional
public IActionResult GetById(int? id) { }
```

### Диаграмма: Маршрутизация

```
┌─────────────────────────────────────────────┐
│  HTTP Request                               │
│  GET /api/person/123                        │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Route Matching        │
        │  - Controller: Person  │
        │  - Action: GetById     │
        │  - Parameter: id=123   │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  PersonController     │
        │  GetById(123)          │
        └───────────────────────┘
```

---

## 5. HTTP-методы и коды состояния

### Стандартные HTTP-методы

```csharp
[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase {
    
    // GET - Retrieve resources
    [HttpGet]
    public IActionResult GetAll() {
        return Ok(_people);  // 200 OK
    }
    
    // POST - Create new resource
    [HttpPost]
    public IActionResult Create(Person person) {
        _people.Add(person);
        return CreatedAtAction(nameof(GetById), 
            new { id = person.Id }, person);  // 201 Created
    }
    
    // PUT - Update resource (complete replacement)
    [HttpPut("{id}")]
    public IActionResult Update(int id, Person person) {
        // Update completely
        return NoContent();  // 204 No Content
    }
    
    // PATCH - Update resource (partial)
    [HttpPatch("{id}")]
    public IActionResult PartialUpdate(int id, JsonPatchDocument<Person> patch) {
        // Update partially
        return NoContent();
    }
    
    // DELETE - Delete resource
    [HttpDelete("{id}")]
    public IActionResult Delete(int id) {
        _people.RemoveAll(p => p.Id == id);
        return NoContent();  // 204 No Content
    }
}
```

### Коды состояния

```csharp
public class PersonController : ControllerBase {
    
    [HttpGet("{id}")]
    public IActionResult GetById(int id) {
        var person = _people.FirstOrDefault(p => p.Id == id);
        
        if (person == null) {
            return NotFound();  // 404
        }
        
        return Ok(person);  // 200
    }
    
    [HttpPost]
    public IActionResult Create(Person person) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);  // 400
        }
        
        _people.Add(person);
        return CreatedAtAction(nameof(GetById), 
            new { id = person.Id }, person);  // 201
    }
    
    [HttpPut("{id}")]
    public IActionResult Update(int id, Person person) {
        if (id != person.Id) {
            return BadRequest();  // 400
        }
        
        var existing = _people.FirstOrDefault(p => p.Id == id);
        if (existing == null) {
            return NotFound();  // 404
        }
        
        // Update
        return NoContent();  // 204
    }
}
```

### Таблица: Распространённые коды состояния

| Код состояния | Значение | Когда использовать |
|-------------|-------------|--------------|
| **200 OK** | Успех | GET, PUT, PATCH с телом |
| **201 Created** | Ресурс создан | POST с успехом |
| **204 No Content** | Успех без тела | PUT, DELETE, PATCH |
| **400 Bad Request** | Неверный запрос | Валидация не прошла |
| **401 Unauthorized** | Не аутентифицирован | Требуется аутентификация |
| **403 Forbidden** | Не авторизован | Недостаточно прав |
| **404 Not Found** | Ресурс не найден | Ресурс не существует |
| **500 Internal Server Error** | Ошибка сервера | Общая ошибка |

---

## 6. Model Binding и валидация

### Model Binding

```csharp
// Binding from query string
[HttpGet("search")]
public IActionResult Search([FromQuery] string name) {
    // GET /api/person/search?name=John
}

// Binding from route
[HttpGet("{id}")]
public IActionResult GetById([FromRoute] int id) {
    // GET /api/person/123
}

// Binding from body
[HttpPost]
public IActionResult Create([FromBody] Person person) {
    // POST /api/person
    // Body: { "name": "John", "email": "john@example.com" }
}

// Binding from form
[HttpPost("upload")]
public IActionResult Upload([FromForm] IFormFile file) {
    // POST /api/person/upload (multipart/form-data)
}

// Binding from header
[HttpGet("info")]
public IActionResult GetInfo([FromHeader] string authorization) {
    // GET /api/person/info
    // Header: Authorization: Bearer token
}
```

### Валидация с Data Annotations

```csharp
// Models/Person.cs
using System.ComponentModel.DataAnnotations;

public class Person {
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; }
    
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email")]
    public string Email { get; set; }
    
    [Range(0, 150, ErrorMessage = "Age must be between 0 and 150")]
    public int Age { get; set; }
    
    [Url]
    public string Website { get; set; }
}

// Controller with automatic validation
[ApiController]  // Enables automatic validation
[Route("api/[controller]")]
public class PersonController : ControllerBase {
    
    [HttpPost]
    public IActionResult Create(Person person) {
        // Automatic validation if [ApiController]
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        
        // Save person
        return CreatedAtAction(nameof(GetById), new { id = person.Id }, person);
    }
}
```

### Диаграмма: Model Binding

```
┌─────────────────────────────────────────────┐
│  HTTP Request                               │
│  POST /api/person                           │
│  Body: { "name": "John", "email": "..." }  │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Model Binding        │
        │  - Deserialize JSON   │
        │  - Map to Person      │
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
│  Valid    │          │  Invalid  │
│  → OK     │          │  → 400    │
│           │          │           │
└───────────┘          └───────────┘
```

---

## 7. Dependency Injection

### Настройка

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();

builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();
app.Run();
```

### Сервис

```csharp
// Services/IPersonService.cs
public interface IPersonService {
    Task<List<Person>> GetAllAsync();
    Task<Person> GetByIdAsync(int id);
    Task<int> CreateAsync(Person person);
    Task UpdateAsync(Person person);
    Task DeleteAsync(int id);
}

// Services/PersonService.cs
public class PersonService : IPersonService {
    private readonly IPersonRepository _repository;
    
    public PersonService(IPersonRepository repository) {
        _repository = repository;
    }
    
    public async Task<List<Person>> GetAllAsync() {
        return await _repository.GetAllAsync();
    }
    
    public async Task<Person> GetByIdAsync(int id) {
        return await _repository.GetByIdAsync(id);
    }
    
    public async Task<int> CreateAsync(Person person) {
        // Business logic
        if (await _repository.EmailExistsAsync(person.Email)) {
            throw new InvalidOperationException("Email already exists");
        }
        
        return await _repository.CreateAsync(person);
    }
    
    public async Task UpdateAsync(Person person) {
        await _repository.UpdateAsync(person);
    }
    
    public async Task DeleteAsync(int id) {
        await _repository.DeleteAsync(id);
    }
}
```

### Контроллер с DI

```csharp
[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase {
    private readonly IPersonService _service;
    
    // Dependency Injection via constructor
    public PersonController(IPersonService service) {
        _service = service;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Person>>> GetAll() {
        var people = await _service.GetAllAsync();
        return Ok(people);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Person>> GetById(int id) {
        var person = await _service.GetByIdAsync(id);
        
        if (person == null) {
            return NotFound();
        }
        
        return Ok(person);
    }
    
    [HttpPost]
    public async Task<ActionResult<Person>> Create(Person person) {
        try {
            var id = await _service.CreateAsync(person);
            return CreatedAtAction(nameof(GetById), new { id }, person);
        }
        catch (InvalidOperationException ex) {
            return BadRequest(ex.Message);
        }
    }
}
```

---

## 8. Middleware и Pipeline

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

### Диаграмма: Middleware Pipeline

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

### Настройка Swagger

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "My API",
        Version = "v1",
        Description = "API for managing people"
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
public class PersonController : ControllerBase {
    
    /// <summary>
    /// Get all people
    /// </summary>
    /// <returns>List of people</returns>
    /// <response code="200">Returns the list</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Person>> GetAll() {
        return Ok(_people);
    }
    
    /// <summary>
    /// Get a person by ID
    /// </summary>
    /// <param name="id">Person ID</param>
    /// <returns>Person</returns>
    /// <response code="200">Person found</response>
    /// <response code="404">Person not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Person> GetById(int id) {
        var person = _people.FirstOrDefault(p => p.Id == id);
        
        if (person == null) {
            return NotFound();
        }
        
        return Ok(person);
    }
}
```

---

## 10. Полный пример: RESTful API

### Полная структура

```csharp
// ========== MODEL ==========
public class Person {
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public int Age { get; set; }
}

// ========== REPOSITORY ==========
public interface IPersonRepository {
    Task<List<Person>> GetAllAsync();
    Task<Person> GetByIdAsync(int id);
    Task<int> CreateAsync(Person person);
    Task UpdateAsync(Person person);
    Task DeleteAsync(int id);
}

// ========== SERVICE ==========
public interface IPersonService {
    Task<List<Person>> GetAllAsync();
    Task<Person> GetByIdAsync(int id);
    Task<int> CreateAsync(Person person);
    Task UpdateAsync(Person person);
    Task DeleteAsync(int id);
}

public class PersonService : IPersonService {
    private readonly IPersonRepository _repository;
    
    public PersonService(IPersonRepository repository) {
        _repository = repository;
    }
    
    public async Task<List<Person>> GetAllAsync() {
        return await _repository.GetAllAsync();
    }
    
    public async Task<Person> GetByIdAsync(int id) {
        return await _repository.GetByIdAsync(id);
    }
    
    public async Task<int> CreateAsync(Person person) {
        return await _repository.CreateAsync(person);
    }
    
    public async Task UpdateAsync(Person person) {
        await _repository.UpdateAsync(person);
    }
    
    public async Task DeleteAsync(int id) {
        await _repository.DeleteAsync(id);
    }
}

// ========== CONTROLLER ==========
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PersonController : ControllerBase {
    private readonly IPersonService _service;
    
    public PersonController(IPersonService service) {
        _service = service;
    }
    
    // GET api/person
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Person>>> GetAll() {
        var people = await _service.GetAllAsync();
        return Ok(people);
    }
    
    // GET api/person/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Person>> GetById(int id) {
        var person = await _service.GetByIdAsync(id);
        
        if (person == null) {
            return NotFound();
        }
        
        return Ok(person);
    }
    
    // POST api/person
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Person>> Create(Person person) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        
        var id = await _service.CreateAsync(person);
        return CreatedAtAction(nameof(GetById), new { id }, person);
    }
    
    // PUT api/person/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, Person person) {
        if (id != person.Id) {
            return BadRequest();
        }
        
        var existing = await _service.GetByIdAsync(id);
        if (existing == null) {
            return NotFound();
        }
        
        await _service.UpdateAsync(person);
        return NoContent();
    }
    
    // DELETE api/person/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id) {
        var person = await _service.GetByIdAsync(id);
        if (person == null) {
            return NotFound();
        }
        
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
```

---

## 11. Best Practices

### ✅ Что делать

1. **Используйте [ApiController] для автоматической валидации**
   ```csharp
   [ApiController]
   [Route("api/[controller]")]
   public class PersonController : ControllerBase { }
   ```

2. **Используйте DTO вместо Entity напрямую**
   ```csharp
   // ✅ CORRECT
   public class PersonDto {
       public int Id { get; set; }
       public string Name { get; set; }
   }
   ```

3. **Используйте async/await для I/O операций**
   ```csharp
   // ✅ CORRECT
   public async Task<ActionResult<Person>> GetById(int id) {
       var person = await _service.GetByIdAsync(id);
       return Ok(person);
   }
   ```

4. **Используйте соответствующие коды состояния**
   ```csharp
   // ✅ CORRECT
   return CreatedAtAction(...);  // 201
   return NoContent();          // 204
   return NotFound();           // 404
   ```

### ❌ Чего избегать

1. **Не размещайте бизнес-логику в контроллерах**
   ```csharp
   // ❌ WRONG
   [HttpPost]
   public IActionResult Create(Person person) {
       // Business logic here - NO!
       if (person.Email.Contains("@domain.com")) {
           // ...
       }
   }
   ```

2. **Не выставляйте сущности напрямую**
   ```csharp
   // ❌ WRONG
   public async Task<ActionResult<PersonEntity>> GetById(int id) {
       // Exposes internal entity
   }
   ```

---

## 12. Часто задаваемые вопросы (FAQ)

### Q: В чём разница между Web API и MVC?
**A:** Web API предназначен для HTTP/REST сервисов, MVC — для веб-приложений с HTML-представлениями.

### Q: Как управлять аутентификацией?
**A:** Используйте JWT токены, Identity, или OAuth с middleware аутентификации.

### Q: Как управлять CORS?
**A:** Настройте CORS в Program.cs с помощью `AddCors()` и `UseCors()`.

### Q: Как сериализовать в XML вместо JSON?
**A:** Добавьте `AddXmlSerializerFormatters()` в `AddControllers()`.

---

## Заключение

ASP.NET Web API необходим для:

- ✅ Создания RESTful API
- ✅ Поддержки множества клиентов
- ✅ Реализации современных архитектур
- ✅ Построения микросервисов

Web API — это стандартный фреймворк для создания HTTP-сервисов в .NET.

---

*Документ создан для объяснения ASP.NET Web API с практическими примерами и диаграммами.*

