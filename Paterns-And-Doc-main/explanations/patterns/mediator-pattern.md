# Mediator Pattern in C#

## Introduzione

Il **Mediator Pattern** è un pattern comportamentale che definisce un oggetto che incapsula come un insieme di oggetti interagiscono. Promuove il disaccoppiamento impedendo agli oggetti di riferirsi esplicitamente l'uno all'altro, permettendo di variare le loro interazioni in modo indipendente.

---

## 1. Cos'è il Mediator Pattern?

### Definizione

Il Mediator Pattern definisce un oggetto che centralizza la comunicazione tra oggetti in un sistema. Invece di comunicare direttamente, gli oggetti comunicano attraverso un mediatore.

### Problema: Comunicazione Diretta

```csharp
// ❌ PROBLEMA: Comunicazione diretta tra oggetti
public class Button {
    private TextBox textBox;
    private CheckBox checkBox;
    
    public void Click() {
        textBox.Clear();
        checkBox.SetChecked(false);
    }
}

public class TextBox {
    private Button button;
    
    public void TextChanged() {
        button.SetEnabled(true);
    }
}

public class CheckBox {
    private Button button;
    
    public void CheckChanged() {
        button.SetEnabled(true);
    }
}
```

**Problemi:**
- Accoppiamento stretto tra componenti
- Difficile modificare un componente
- Difficile riutilizzare componenti
- Comunicazione complessa

### Diagramma: Problema

```
┌──────────┐      ┌──────────┐      ┌──────────┐
│  Button  │─────▶│ TextBox  │─────▶│ CheckBox │
└──────────┘      └──────────┘      └──────────┘
       │                │                 │
       └────────────────┴─────────────────┘
              (Comunicazione diretta,
               accoppiamento stretto)
```

### Soluzione: Mediator Pattern

```csharp
// ✅ SOLUZIONE: Comunicazione attraverso Mediator

// Mediator
public interface IMediator {
    void Notify(object sender, string eventName);
}

public class FormMediator : IMediator {
    private Button button;
    private TextBox textBox;
    private CheckBox checkBox;
    
    public FormMediator(Button button, TextBox textBox, CheckBox checkBox) {
        this.button = button;
        this.textBox = textBox;
        this.checkBox = checkBox;
        
        button.SetMediator(this);
        textBox.SetMediator(this);
        checkBox.SetMediator(this);
    }
    
    public void Notify(object sender, string eventName) {
        if (eventName == "Click") {
            textBox.Clear();
            checkBox.SetChecked(false);
        }
        else if (eventName == "TextChanged") {
            button.SetEnabled(true);
        }
        else if (eventName == "CheckChanged") {
            button.SetEnabled(true);
        }
    }
}

// Componenti
public class Button {
    private IMediator mediator;
    
    public void SetMediator(IMediator mediator) {
        this.mediator = mediator;
    }
    
    public void Click() {
        mediator?.Notify(this, "Click");
    }
}
```

### Diagramma: Soluzione

```
┌──────────┐      ┌──────────┐      ┌──────────┐
│  Button  │      │ TextBox  │      │ CheckBox │
└──────────┘      └──────────┘      └──────────┘
       │                │                 │
       └────────────────┴─────────────────┘
                        │
                        ▼
              ┌─────────────────┐
              │ FormMediator    │
              │ (centralizza    │
              │  comunicazione) │
              └─────────────────┘
```

---

## 2. Mediator Pattern con MediatR

### MediatR Library

**MediatR** è una libreria .NET che implementa il Mediator Pattern in modo elegante, supportando anche CQRS.

### Installazione

```bash
dotnet add package MediatR
```

### Struttura Base

```csharp
using MediatR;

// Request (per comandi e query)
public interface IRequest<out TResponse> { }
public interface IRequest : IRequest<Unit> { }

// Handler
public interface IRequestHandler<in TRequest, TResponse> 
    where TRequest : IRequest<TResponse> {
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
```

### Esempio: Command Handler

```csharp
// Command
public class CreatePersonaCommand : IRequest<int> {
    public string Nome { get; set; }
    public string Email { get; set; }
}

// Command Handler
public class CreatePersonaCommandHandler 
    : IRequestHandler<CreatePersonaCommand, int> {
    
    private readonly IPersonaRepository _repository;
    
    public CreatePersonaCommandHandler(IPersonaRepository repository) {
        _repository = repository;
    }
    
    public async Task<int> Handle(
        CreatePersonaCommand request, 
        CancellationToken cancellationToken) {
        
        var persona = new Persona {
            Nome = request.Nome,
            Email = request.Email
        };
        
        await _repository.SaveAsync(persona);
        return persona.Id;
    }
}

// Utilizzo
public class PersonaController : ControllerBase {
    private readonly IMediator _mediator;
    
    public PersonaController(IMediator mediator) {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreatePersonaCommand command) {
        var id = await _mediator.Send(command);
        return Ok(id);
    }
}
```

### Diagramma: MediatR Flow

```
┌─────────────────────────────────────────────┐
│  Controller                                  │
│  _mediator.Send(command)                    │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  MediatR              │
        │  (Mediator)           │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Trova Handler        │
        │  appropriato          │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Command Handler      │
        │  Esegue logica        │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Risultato            │
        └───────────────────────┘
```

---

## 3. Query Handler

### Esempio: Query Handler

```csharp
// Query
public class GetPersonaByIdQuery : IRequest<PersonaDto> {
    public int Id { get; set; }
}

// Query Handler
public class GetPersonaByIdQueryHandler 
    : IRequestHandler<GetPersonaByIdQuery, PersonaDto> {
    
    private readonly IPersonaRepository _repository;
    
    public GetPersonaByIdQueryHandler(IPersonaRepository repository) {
        _repository = repository;
    }
    
    public async Task<PersonaDto> Handle(
        GetPersonaByIdQuery request, 
        CancellationToken cancellationToken) {
        
        var persona = await _repository.GetByIdAsync(request.Id);
        
        if (persona == null) {
            return null;
        }
        
        return new PersonaDto {
            Id = persona.Id,
            Nome = persona.Nome,
            Email = persona.Email
        };
    }
}

// Utilizzo
public class PersonaController : ControllerBase {
    private readonly IMediator _mediator;
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id) {
        var query = new GetPersonaByIdQuery { Id = id };
        var persona = await _mediator.Send(query);
        
        if (persona == null) {
            return NotFound();
        }
        
        return Ok(persona);
    }
}
```

---

## 4. Notifications (Eventi)

### Definizione

Le **Notifications** permettono di pubblicare eventi che possono essere gestiti da più handler.

### Esempio: Notification

```csharp
// Notification
public class PersonaCreatedNotification : INotification {
    public int PersonaId { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
}

// Notification Handler 1
public class SendWelcomeEmailHandler 
    : INotificationHandler<PersonaCreatedNotification> {
    
    public async Task Handle(
        PersonaCreatedNotification notification, 
        CancellationToken cancellationToken) {
        
        // Invia email di benvenuto
        await SendEmailAsync(notification.Email, "Benvenuto!");
    }
}

// Notification Handler 2
public class LogPersonaCreatedHandler 
    : INotificationHandler<PersonaCreatedNotification> {
    
    public async Task Handle(
        PersonaCreatedNotification notification, 
        CancellationToken cancellationToken) {
        
        // Log dell'evento
        Console.WriteLine($"Persona creata: {notification.Nome}");
    }
}

// Utilizzo
public class CreatePersonaCommandHandler 
    : IRequestHandler<CreatePersonaCommand, int> {
    
    private readonly IPersonaRepository _repository;
    private readonly IMediator _mediator;
    
    public async Task<int> Handle(CreatePersonaCommand request) {
        var persona = new Persona {
            Nome = request.Nome,
            Email = request.Email
        };
        
        await _repository.SaveAsync(persona);
        
        // Pubblica evento
        await _mediator.Publish(new PersonaCreatedNotification {
            PersonaId = persona.Id,
            Nome = persona.Nome,
            Email = persona.Email
        });
        
        return persona.Id;
    }
}
```

### Diagramma: Notifications

```
┌─────────────────────────────────────────────┐
│  Command Handler                            │
│  _mediator.Publish(notification)           │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  MediatR              │
        │  (Publish)            │
        └───────────────────────┘
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│ Handler 1 │ │ Handler 2 │ │ Handler 3 │
│ (Email)   │ │ (Log)     │ │ (SMS)     │
└───────────┘ └───────────┘ └───────────┘
```

---

## 5. Esempio Completo: Sistema Ordini

### Struttura

```csharp
// ========== COMMANDS ==========

public class CreateOrdineCommand : IRequest<int> {
    public int ClienteId { get; set; }
    public List<ProdottoOrdineDto> Prodotti { get; set; }
}

public class CreateOrdineCommandHandler 
    : IRequestHandler<CreateOrdineCommand, int> {
    
    private readonly IOrdineRepository _repository;
    private readonly IMediator _mediator;
    
    public async Task<int> Handle(CreateOrdineCommand request) {
        var ordine = new Ordine {
            ClienteId = request.ClienteId,
            DataCreazione = DateTime.UtcNow,
            Stato = StatoOrdine.Pending
        };
        
        foreach (var prodotto in request.Prodotti) {
            ordine.AggiungiProdotto(prodotto.ProdottoId, prodotto.Quantita);
        }
        
        await _repository.SaveAsync(ordine);
        
        // Pubblica evento
        await _mediator.Publish(new OrdineCreatedNotification {
            OrdineId = ordine.Id,
            ClienteId = ordine.ClienteId,
            Totale = ordine.CalcolaTotale()
        });
        
        return ordine.Id;
    }
}

// ========== NOTIFICATIONS ==========

public class OrdineCreatedNotification : INotification {
    public int OrdineId { get; set; }
    public int ClienteId { get; set; }
    public decimal Totale { get; set; }
}

// Handler per inviare email
public class SendOrdineConfirmationEmailHandler 
    : INotificationHandler<OrdineCreatedNotification> {
    
    private readonly IEmailService _emailService;
    private readonly IClienteRepository _clienteRepository;
    
    public async Task Handle(OrdineCreatedNotification notification) {
        var cliente = await _clienteRepository.GetByIdAsync(notification.ClienteId);
        
        await _emailService.SendAsync(cliente.Email, 
            $"Ordine #{notification.OrdineId} creato con successo!");
    }
}

// Handler per aggiornare inventario
public class UpdateInventarioHandler 
    : INotificationHandler<OrdineCreatedNotification> {
    
    private readonly IInventarioService _inventarioService;
    
    public async Task Handle(OrdineCreatedNotification notification) {
        // Aggiorna inventario
        await _inventarioService.DecrementaProdottiAsync(notification.OrdineId);
    }
}

// ========== QUERIES ==========

public class GetOrdineByIdQuery : IRequest<OrdineDto> {
    public int Id { get; set; }
}

public class GetOrdineByIdQueryHandler 
    : IRequestHandler<GetOrdineByIdQuery, OrdineDto> {
    
    private readonly IOrdineRepository _repository;
    
    public async Task<OrdineDto> Handle(GetOrdineByIdQuery request) {
        var ordine = await _repository.GetByIdAsync(request.Id);
        
        return new OrdineDto {
            Id = ordine.Id,
            ClienteId = ordine.ClienteId,
            DataCreazione = ordine.DataCreazione,
            Totale = ordine.CalcolaTotale(),
            Stato = ordine.Stato.ToString()
        };
    }
}

// ========== CONTROLLER ==========

[ApiController]
[Route("api/[controller]")]
public class OrdiniController : ControllerBase {
    private readonly IMediator _mediator;
    
    public OrdiniController(IMediator mediator) {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateOrdineCommand command) {
        var id = await _mediator.Send(command);
        return Ok(new { Id = id });
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id) {
        var query = new GetOrdineByIdQuery { Id = id };
        var ordine = await _mediator.Send(query);
        
        if (ordine == null) {
            return NotFound();
        }
        
        return Ok(ordine);
    }
}
```

---

## 6. Configurazione di MediatR

### Setup in ASP.NET Core

```csharp
// Program.cs o Startup.cs
public void ConfigureServices(IServiceCollection services) {
    // Registra MediatR
    services.AddMediatR(cfg => {
        cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    });
    
    // Oppure registra manualmente
    services.AddMediatR(typeof(CreatePersonaCommandHandler).Assembly);
    
    // Registra repository e servizi
    services.AddScoped<IPersonaRepository, PersonaRepository>();
}
```

### Dependency Injection

```csharp
public class PersonaService {
    private readonly IMediator _mediator;
    
    public PersonaService(IMediator mediator) {
        _mediator = mediator;  // Iniettato automaticamente
    }
    
    public async Task<int> CreaPersona(string nome, string email) {
        var command = new CreatePersonaCommand {
            Nome = nome,
            Email = email
        };
        
        return await _mediator.Send(command);
    }
}
```

---

## 7. Vantaggi del Mediator Pattern

### Tabella Riepilogativa

| Vantaggio | Descrizione |
|-----------|-------------|
| **Disaccoppiamento** | Componenti non conoscono altri componenti |
| **Manutenibilità** | Facile modificare le interazioni |
| **Riutilizzabilità** | Componenti riutilizzabili in contesti diversi |
| **Testabilità** | Facile testare componenti isolatamente |
| **Chiarezza** | Logica di comunicazione centralizzata |

### Diagramma: Vantaggi

```
┌─────────────────────────────────────────────┐
│  Disaccoppiamento                           │
│  - Componenti indipendenti                 │
│  - Cambiamenti isolati                     │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  Manutenibilità                              │
│  - Logica centralizzata                     │
│  - Facile da modificare                    │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  Testabilità                                 │
│  - Mock del mediator                        │
│  - Test isolati                             │
└─────────────────────────────────────────────┘
```

---

## 8. Quando Usare Mediator Pattern

### ✅ Quando Usare

1. **Comunicazione complessa tra oggetti**
   - Molti oggetti che comunicano
   - Logica di comunicazione complessa

2. **Disaccoppiamento necessario**
   - Componenti devono essere indipendenti
   - Cambiamenti frequenti

3. **CQRS Pattern**
   - Separazione command/query
   - Handler dedicati

4. **Event-Driven Architecture**
   - Pubblicazione eventi
   - Multiple subscription

### ❌ Quando NON Usare

1. **Sistemi semplici**
   - Pochi oggetti
   - Comunicazione diretta sufficiente

2. **Performance critiche**
   - Overhead del mediator
   - Comunicazione diretta più veloce

---

## 9. Best Practices

### ✅ Cosa Fare

1. **Un handler per request**
   ```csharp
   // ✅ CORRETTO
   public class CreatePersonaCommandHandler 
       : IRequestHandler<CreatePersonaCommand, int> { }
   ```

2. **Usa notifications per eventi**
   ```csharp
   // ✅ CORRETTO
   await _mediator.Publish(new PersonaCreatedNotification { });
   ```

3. **Validazione nei handler**
   ```csharp
   // ✅ CORRETTO
   public async Task<int> Handle(CreatePersonaCommand request) {
       if (string.IsNullOrEmpty(request.Nome)) {
           throw new ValidationException("Nome obbligatorio");
       }
       // ...
   }
   ```

### ❌ Cosa Evitare

1. **Non creare handler troppo complessi**
   ```csharp
   // ❌ SBAGLIATO
   public async Task<int> Handle(CreatePersonaCommand request) {
       // 500 righe di codice
   }
   ```

2. **Non usare mediator per operazioni semplici**
   ```csharp
   // ❌ SBAGLIATO - Overhead non necessario
   var query = new GetSimpleValueQuery { };
   var value = await _mediator.Send(query);
   ```

---

## 10. Domande Frequenti (FAQ)

### Q: MediatR è necessario per usare Mediator Pattern?
**R:** No, puoi implementare il pattern manualmente, ma MediatR semplifica l'implementazione.

### Q: Qual è la differenza tra Send e Publish?
**R:** Send è per request con un handler, Publish è per notifications con più handler.

### Q: Mediator Pattern aumenta le performance?
**R:** No, anzi può aggiungere overhead. Usalo per disaccoppiamento, non per performance.

### Q: Posso usare MediatR con CQRS?
**R:** Sì, MediatR è perfetto per implementare CQRS.

---

## Conclusioni

Il Mediator Pattern è utile per:

- ✅ Disaccoppiare componenti
- ✅ Centralizzare la logica di comunicazione
- ✅ Migliorare la manutenibilità
- ✅ Implementare CQRS in modo elegante

MediatR è una libreria eccellente che semplifica l'implementazione del pattern in .NET.

---

*Documento creato per spiegare il Mediator Pattern con esempi pratici e diagrammi.*

