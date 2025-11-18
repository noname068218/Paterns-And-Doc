# CQRS (Command Query Responsibility Segregation) Pattern

## Introduzione

**CQRS** (Command Query Responsibility Segregation) è un pattern architetturale che separa le operazioni di **lettura** (Query) da quelle di **scrittura** (Command). Questo pattern migliora la scalabilità, le performance e la manutenibilità delle applicazioni.

---

## 1. Cos'è CQRS?

### Definizione

CQRS separa il modello di dati in due modelli distinti:
- **Command Model**: per le operazioni di scrittura (Create, Update, Delete)
- **Query Model**: per le operazioni di lettura (Read)

### Problema: Modello Unico

```csharp
// ❌ PROBLEMA: Modello unico per lettura e scrittura
public class Persona {
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public DateTime DataCreazione { get; set; }
    public List<Ordine> Ordini { get; set; }
    // ... molte altre proprietà
}

public class PersonaRepository {
    // Metodi per lettura e scrittura mescolati
    public Persona GetById(int id) { }
    public void Save(Persona persona) { }
    public List<Persona> GetAll() { }
    public void Update(Persona persona) { }
}
```

**Problemi:**
- Modello complesso per operazioni semplici
- Performance ottimizzate per un caso d'uso compromettono l'altro
- Difficile scalare lettura e scrittura indipendentemente

### Soluzione: CQRS

```csharp
// ✅ SOLUZIONE: Separazione Command e Query

// Command Model (scrittura)
public class CreatePersonaCommand {
    public string Nome { get; set; }
    public string Email { get; set; }
}

public class UpdatePersonaCommand {
    public int Id { get; set; }
    public string Nome { get; set; }
}

// Query Model (lettura)
public class PersonaDto {
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
}

public class PersonaListDto {
    public int Id { get; set; }
    public string Nome { get; set; }
}
```

### Diagramma: CQRS Pattern

```
┌─────────────────────────────────────────────┐
│  Applicazione                               │
└─────────────────────────────────────────────┘
                    │
        ┌───────────┴───────────┐
        │                       │
        ▼                       ▼
┌───────────────┐      ┌───────────────┐
│   COMMANDS    │      │    QUERIES    │
│  (Scrittura)  │      │   (Lettura)   │
└───────────────┘      └───────────────┘
        │                       │
        ▼                       ▼
┌───────────────┐      ┌───────────────┐
│ Command Model│      │ Query Model   │
│              │      │               │
│ - Write DB   │      │ - Read DB     │
│ - Business   │      │ - Optimized   │
│   Logic      │      │   for reads   │
└───────────────┘      └───────────────┘
```

---

## 2. Command Pattern

### Definizione

Un **Command** rappresenta un'intenzione di modificare lo stato del sistema. Ogni comando è un oggetto che incapsula l'operazione.

### Struttura Base

```csharp
// Interfaccia base per i comandi
public interface ICommand { }

// Interfaccia per i comandi con risultato
public interface ICommand<TResult> { }

// Command Handler
public interface ICommandHandler<TCommand, TResult> 
    where TCommand : ICommand<TResult> {
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
}
```

### Esempio: Command per Creare Persona

```csharp
// Command
public class CreatePersonaCommand : ICommand<int> {
    public string Nome { get; set; }
    public string Email { get; set; }
    public DateTime DataNascita { get; set; }
}

// Command Handler
public class CreatePersonaCommandHandler 
    : ICommandHandler<CreatePersonaCommand, int> {
    
    private readonly IPersonaWriteRepository _repository;
    
    public CreatePersonaCommandHandler(IPersonaWriteRepository repository) {
        _repository = repository;
    }
    
    public async Task<int> HandleAsync(
        CreatePersonaCommand command, 
        CancellationToken cancellationToken) {
        
        // Validazione
        if (string.IsNullOrEmpty(command.Nome)) {
            throw new ArgumentException("Nome è obbligatorio");
        }
        
        // Creazione entità
        var persona = new Persona {
            Nome = command.Nome,
            Email = command.Email,
            DataNascita = command.DataNascita,
            DataCreazione = DateTime.UtcNow
        };
        
        // Salvataggio
        await _repository.SaveAsync(persona);
        
        return persona.Id;
    }
}
```

### Diagramma: Command Flow

```
┌─────────────────────────────────────────────┐
│  CreatePersonaCommand                       │
│  { Nome, Email, DataNascita }                │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Command Handler      │
        │  - Valida              │
        │  - Crea entità         │
        │  - Salva               │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Write Repository     │
        │  (Database scrittura) │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Risultato (Id)       │
        └───────────────────────┘
```

---

## 3. Query Pattern

### Definizione

Una **Query** rappresenta una richiesta di dati senza modificare lo stato del sistema. Le query sono ottimizzate per la lettura.

### Struttura Base

```csharp
// Interfaccia base per le query
public interface IQuery<TResult> { }

// Query Handler
public interface IQueryHandler<TQuery, TResult> 
    where TQuery : IQuery<TResult> {
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken);
}
```

### Esempio: Query per Ottenere Persona

```csharp
// Query
public class GetPersonaByIdQuery : IQuery<PersonaDto> {
    public int Id { get; set; }
}

// DTO per la risposta
public class PersonaDto {
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public int Eta { get; set; }
}

// Query Handler
public class GetPersonaByIdQueryHandler 
    : IQueryHandler<GetPersonaByIdQuery, PersonaDto> {
    
    private readonly IPersonaReadRepository _repository;
    
    public GetPersonaByIdQueryHandler(IPersonaReadRepository repository) {
        _repository = repository;
    }
    
    public async Task<PersonaDto> HandleAsync(
        GetPersonaByIdQuery query, 
        CancellationToken cancellationToken) {
        
        var persona = await _repository.GetByIdAsync(query.Id);
        
        if (persona == null) {
            return null;
        }
        
        return new PersonaDto {
            Id = persona.Id,
            Nome = persona.Nome,
            Email = persona.Email,
            Eta = CalcolaEta(persona.DataNascita)
        };
    }
    
    private int CalcolaEta(DateTime dataNascita) {
        return DateTime.UtcNow.Year - dataNascita.Year;
    }
}
```

### Diagramma: Query Flow

```
┌─────────────────────────────────────────────┐
│  GetPersonaByIdQuery                         │
│  { Id = 123 }                                │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Query Handler        │
        │  - Legge dati          │
        │  - Trasforma in DTO    │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Read Repository       │
        │  (Database lettura     │
        │   ottimizzato)         │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  PersonaDto           │
        │  (solo dati necessari) │
        └───────────────────────┘
```

---

## 4. Separazione Write e Read Database

### Due Database Separati

```csharp
// Write Database (per comandi)
public interface IPersonaWriteRepository {
    Task<int> SaveAsync(Persona persona);
    Task UpdateAsync(Persona persona);
    Task DeleteAsync(int id);
}

// Read Database (per query)
public interface IPersonaReadRepository {
    Task<PersonaDto> GetByIdAsync(int id);
    Task<List<PersonaListDto>> GetAllAsync();
    Task<List<PersonaDto>> SearchAsync(string nome);
}
```

### Implementazione con Database Diversi

```csharp
// Write Repository - SQL Server (transazionale)
public class PersonaWriteRepository : IPersonaWriteRepository {
    private readonly SqlConnection _connection;
    
    public async Task<int> SaveAsync(Persona persona) {
        // Salva in SQL Server con transazioni
        // ...
        return persona.Id;
    }
}

// Read Repository - MongoDB (ottimizzato per lettura)
public class PersonaReadRepository : IPersonaReadRepository {
    private readonly IMongoCollection<PersonaDto> _collection;
    
    public async Task<PersonaDto> GetByIdAsync(int id) {
        // Legge da MongoDB (denormalizzato)
        return await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();
    }
}
```

### Diagramma: Database Separati

```
┌─────────────────────────────────────────────┐
│  COMMANDS                                   │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Write Database       │
        │  (SQL Server)         │
        │  - Transazionale       │
        │  - Normalizzato          │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Event/Replication    │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Read Database        │
        │  (MongoDB/Redis)      │
        │  - Denormalizzato     │
        │  - Ottimizzato lettura│
        └───────────────────────┘
                    ▲
                    │
┌─────────────────────────────────────────────┐
│  QUERIES                                    │
└─────────────────────────────────────────────┘
```

---

## 5. Event Sourcing con CQRS

### Definizione

Event Sourcing memorizza gli eventi invece dello stato corrente. Lo stato viene ricostruito applicando gli eventi.

### Esempio: Event Sourcing

```csharp
// Eventi
public abstract class DomainEvent {
    public DateTime OccurredOn { get; set; } = DateTime.UtcNow;
}

public class PersonaCreatedEvent : DomainEvent {
    public int PersonaId { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
}

public class PersonaUpdatedEvent : DomainEvent {
    public int PersonaId { get; set; }
    public string Nome { get; set; }
}

// Event Store
public interface IEventStore {
    Task SaveEventsAsync(int aggregateId, IEnumerable<DomainEvent> events);
    Task<List<DomainEvent>> GetEventsAsync(int aggregateId);
}

// Command Handler con Event Sourcing
public class CreatePersonaCommandHandler {
    private readonly IEventStore _eventStore;
    
    public async Task<int> HandleAsync(CreatePersonaCommand command) {
        var personaId = GenerateId();
        
        var evento = new PersonaCreatedEvent {
            PersonaId = personaId,
            Nome = command.Nome,
            Email = command.Email
        };
        
        await _eventStore.SaveEventsAsync(personaId, new[] { evento });
        
        return personaId;
    }
}
```

### Diagramma: Event Sourcing

```
┌─────────────────────────────────────────────┐
│  Command: CreatePersona                     │
└─────────────────────────────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Crea Evento          │
        │  PersonaCreatedEvent   │
        └───────────────────────┘
                    │
                    ▼
        ┌───────────────────────┐
        │  Event Store          │
        │  (sequenza eventi)     │
        └───────────────────────┘
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│  Read     │ │  Read     │ │  Read     │
│  Model 1  │ │  Model 2  │ │  Model 3  │
│  (replay  │ │  (replay  │ │  (replay  │
│   events) │ │   events) │ │   events) │
└───────────┘ └───────────┘ └───────────┘
```

---

## 6. Esempio Completo: Sistema Ordini

### Struttura del Progetto

```
SistemaOrdini/
├── Commands/
│   ├── CreateOrdineCommand.cs
│   ├── UpdateOrdineCommand.cs
│   └── Handlers/
│       ├── CreateOrdineCommandHandler.cs
│       └── UpdateOrdineCommandHandler.cs
├── Queries/
│   ├── GetOrdineByIdQuery.cs
│   ├── GetOrdiniByClienteQuery.cs
│   └── Handlers/
│       ├── GetOrdineByIdQueryHandler.cs
│       └── GetOrdiniByClienteQueryHandler.cs
├── WriteModels/
│   └── Ordine.cs
├── ReadModels/
│   └── OrdineDto.cs
└── Repositories/
    ├── IOrdineWriteRepository.cs
    └── IOrdineReadRepository.cs
```

### Implementazione Completa

```csharp
// ========== COMMANDS ==========

public class CreateOrdineCommand : ICommand<int> {
    public int ClienteId { get; set; }
    public List<ProdottoOrdineDto> Prodotti { get; set; }
}

public class CreateOrdineCommandHandler 
    : ICommandHandler<CreateOrdineCommand, int> {
    
    private readonly IOrdineWriteRepository _writeRepository;
    private readonly IClienteRepository _clienteRepository;
    
    public async Task<int> HandleAsync(CreateOrdineCommand command) {
        // Validazione
        var cliente = await _clienteRepository.GetByIdAsync(command.ClienteId);
        if (cliente == null) {
            throw new ArgumentException("Cliente non trovato");
        }
        
        // Creazione ordine
        var ordine = new Ordine {
            ClienteId = command.ClienteId,
            DataCreazione = DateTime.UtcNow,
            Stato = StatoOrdine.Pending
        };
        
        foreach (var prodotto in command.Prodotti) {
            ordine.AggiungiProdotto(prodotto.ProdottoId, prodotto.Quantita, prodotto.Prezzo);
        }
        
        // Salvataggio
        await _writeRepository.SaveAsync(ordine);
        
        return ordine.Id;
    }
}

// ========== QUERIES ==========

public class GetOrdineByIdQuery : IQuery<OrdineDto> {
    public int Id { get; set; }
}

public class OrdineDto {
    public int Id { get; set; }
    public string ClienteNome { get; set; }
    public DateTime DataCreazione { get; set; }
    public decimal Totale { get; set; }
    public string Stato { get; set; }
    public List<ProdottoOrdineDto> Prodotti { get; set; }
}

public class GetOrdineByIdQueryHandler 
    : IQueryHandler<GetOrdineByIdQuery, OrdineDto> {
    
    private readonly IOrdineReadRepository _readRepository;
    
    public async Task<OrdineDto> HandleAsync(GetOrdineByIdQuery query) {
        // Query ottimizzata per lettura (già denormalizzata)
        return await _readRepository.GetByIdAsync(query.Id);
    }
}

public class GetOrdiniByClienteQuery : IQuery<List<OrdineListDto>> {
    public int ClienteId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class OrdineListDto {
    public int Id { get; set; }
    public DateTime DataCreazione { get; set; }
    public decimal Totale { get; set; }
    public string Stato { get; set; }
}

public class GetOrdiniByClienteQueryHandler 
    : IQueryHandler<GetOrdiniByClienteQuery, List<OrdineListDto>> {
    
    private readonly IOrdineReadRepository _readRepository;
    
    public async Task<List<OrdineListDto>> HandleAsync(
        GetOrdiniByClienteQuery query) {
        
        return await _readRepository.GetByClienteIdAsync(
            query.ClienteId, 
            query.Page, 
            query.PageSize);
    }
}
```

---

## 7. Vantaggi di CQRS

### Tabella Riepilogativa

| Vantaggio | Descrizione |
|-----------|-------------|
| **Scalabilità** | Scala lettura e scrittura indipendentemente |
| **Performance** | Ottimizzazione separata per lettura e scrittura |
| **Flessibilità** | Modelli diversi per scopi diversi |
| **Manutenibilità** | Separazione chiara delle responsabilità |
| **Sicurezza** | Controlli di accesso separati per read/write |

### Diagramma: Vantaggi

```
┌─────────────────────────────────────────────┐
│  Scalabilità                                  │
│  - Più server per lettura                    │
│  - Server dedicati per scrittura             │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  Performance                                  │
│  - Write DB: normalizzato, transazionale     │
│  - Read DB: denormalizzato, ottimizzato      │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  Manutenibilità                               │
│  - Modelli separati                          │
│  - Cambiamenti isolati                       │
└─────────────────────────────────────────────┘
```

---

## 8. Quando Usare CQRS

### ✅ Quando Usare CQRS

1. **Alto volume di letture vs scritture**
   - Sistema di reporting con molte query
   - Dashboard con molte visualizzazioni

2. **Modelli di lettura e scrittura molto diversi**
   - Scrittura: entità normalizzate
   - Lettura: viste denormalizzate

3. **Necessità di scalare indipendentemente**
   - Più server per lettura
   - Server dedicati per scrittura

4. **Event Sourcing**
   - Storia completa degli eventi
   - Ricostruzione dello stato

### ❌ Quando NON Usare CQRS

1. **Applicazioni semplici CRUD**
   - Overhead non necessario
   - Complessità aggiuntiva

2. **Lettura e scrittura simili**
   - Modelli quasi identici
   - Nessun beneficio dalla separazione

3. **Team piccoli**
   - Complessità maggiore
   - Più codice da mantenere

---

## 9. Best Practices

### ✅ Cosa Fare

1. **Usa DTO specifici per le query**
   ```csharp
   // ✅ CORRETTO
   public class PersonaListDto {
       public int Id { get; set; }
       public string Nome { get; set; }
   }
   ```

2. **Validazione nei command handlers**
   ```csharp
   // ✅ CORRETTO
   public async Task<int> HandleAsync(CreatePersonaCommand command) {
       if (string.IsNullOrEmpty(command.Nome)) {
           throw new ValidationException("Nome obbligatorio");
       }
       // ...
   }
   ```

3. **Separazione netta tra read e write**
   ```csharp
   // ✅ CORRETTO
   IOrdineWriteRepository writeRepo;
   IOrdineReadRepository readRepo;
   ```

### ❌ Cosa Evitare

1. **Non mescolare read e write nello stesso handler**
   ```csharp
   // ❌ SBAGLIATO
   public async Task<PersonaDto> HandleAsync(CreatePersonaCommand command) {
       // Crea e poi legge - non è CQRS puro
   }
   ```

2. **Non usare modelli di scrittura per query**
   ```csharp
   // ❌ SBAGLIATO
   public async Task<Persona> GetByIdAsync(int id) {
       // Restituisce modello di scrittura
   }
   ```

---

## 10. Domande Frequenti (FAQ)

### Q: CQRS richiede sempre due database?
**R:** No, puoi avere due database o anche un solo database con modelli separati.

### Q: CQRS è necessario per ogni applicazione?
**R:** No, solo per applicazioni complesse con alta separazione tra read e write.

### Q: Come sincronizzare read e write database?
**R:** Usa eventi, messaggistica o replicazione del database.

### Q: CQRS aumenta la complessità?
**R:** Sì, ma offre benefici in termini di scalabilità e performance per applicazioni complesse.

---

## Conclusioni

CQRS è un pattern potente per:

- ✅ Separare le responsabilità di lettura e scrittura
- ✅ Scalare indipendentemente read e write
- ✅ Ottimizzare performance per casi d'uso specifici
- ✅ Migliorare la manutenibilità del codice

Usa CQRS quando hai bisogno di scalabilità e ottimizzazione per applicazioni complesse con alta separazione tra operazioni di lettura e scrittura.

---

*Documento creato per spiegare il pattern CQRS con esempi pratici e diagrammi.*

