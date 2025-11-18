# Async e Await in C#

## Introduzione

**async** e **await** sono parole chiave in C# che permettono di scrivere codice asincrono in modo semplice e leggibile. Permettono di eseguire operazioni lunghe senza bloccare il thread principale, migliorando la responsività delle applicazioni.

---

## 1. Perché Async/Await?

### Problema: Codice Sincrono Bloccante

```csharp
// ❌ PROBLEMA: Blocca il thread durante l'operazione
public string ScaricaDati() {
    // Simula operazione lenta (database, API, file)
    Thread.Sleep(3000);  // Blocca per 3 secondi!
    return "Dati scaricati";
}

// Utilizzo
string dati = ScaricaDati();  // L'applicazione si blocca qui!
Console.WriteLine(dati);
```

### Soluzione: Codice Asincrono

```csharp
// ✅ SOLUZIONE: Non blocca il thread
public async Task<string> ScaricaDatiAsync() {
    // Simula operazione asincrona
    await Task.Delay(3000);  // Non blocca!
    return "Dati scaricati";
}

// Utilizzo
string dati = await ScaricaDatiAsync();  // L'applicazione rimane responsiva!
Console.WriteLine(dati);
```

### Diagramma: Sincrono vs Asincrono

```
┌─────────────────────────────────────────────┐
│  CODICE SINCRONO (Bloccante)                │
├─────────────────────────────────────────────┤
│  Thread principale                          │
│  │                                          │
│  │─── ScaricaDati() ────────────────────│  │
│  │     (bloccato per 3 secondi)         │  │
│  │──────────────────────────────────────│  │
│  │─── Continua esecuzione ──────────────│  │
│  │                                      │  │
│  ❌ Applicazione non responsiva!         │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  CODICE ASINCRONO (Non bloccante)           │
├─────────────────────────────────────────────┤
│  Thread principale                          │
│  │                                          │
│  │─── ScaricaDatiAsync() ────────────────│  │
│  │     (attende, ma non blocca)          │  │
│  │─── Continua altre operazioni ──────────│  │
│  │     (UI responsiva!)                   │  │
│  │─── Riceve risultato ───────────────────│  │
│  │                                      │  │
│  ✅ Applicazione sempre responsiva!        │
└─────────────────────────────────────────────┘
```

---

## 2. Sintassi Base: async e await

### async

La parola chiave `async` modifica un metodo, indicando che contiene operazioni asincrone.

```csharp
// Metodo async
public async Task<int> MetodoAsync() {
    // Corpo del metodo
    return 42;
}
```

### await

La parola chiave `await` sospende l'esecuzione del metodo async fino al completamento dell'operazione asincrona.

```csharp
public async Task<int> MetodoAsync() {
    // await sospende l'esecuzione qui
    int risultato = await OperazioneAsincrona();
    
    // Questo codice viene eseguito dopo il completamento
    return risultato;
}
```

### Esempio Completo

```csharp
public class DataService {
    // Metodo asincrono
    public async Task<string> ScaricaDatiAsync() {
        // Simula chiamata HTTP
        await Task.Delay(2000);  // Attende 2 secondi senza bloccare
        return "Dati dal server";
    }
    
    public async Task<int> CalcolaAsync() {
        await Task.Delay(1000);
        return 100;
    }
}

// Utilizzo
public async Task MetodoChiamante() {
    var service = new DataService();
    
    string dati = await service.ScaricaDatiAsync();  // Attende qui
    Console.WriteLine(dati);
    
    int risultato = await service.CalcolaAsync();  // Attende qui
    Console.WriteLine(risultato);
}
```

### Diagramma: Flusso Async/Await

```
┌─────────────────────────────────────────────┐
│  MetodoChiamante()                         │
│  ↓                                          │
│  await ScaricaDatiAsync()                  │
│  ↓                                          │
│  [Sospende esecuzione]                     │
│  ↓                                          │
│  Thread libero per altre operazioni         │
│  ↓                                          │
│  [Operazione completa]                      │
│  ↓                                          │
│  Continua esecuzione                       │
│  ↓                                          │
│  Console.WriteLine(dati)                    │
└─────────────────────────────────────────────┘
```

---

## 3. Tipi di Ritorno Async

### Task<T>

Restituisce un valore di tipo `T`.

```csharp
public async Task<int> OttieniNumeroAsync() {
    await Task.Delay(1000);
    return 42;
}

// Utilizzo
int numero = await OttieniNumeroAsync();  // ✅ 42
```

### Task

Non restituisce un valore (equivalente a `void`).

```csharp
public async Task SalvaDatiAsync() {
    await Task.Delay(1000);
    Console.WriteLine("Dati salvati");
    // Nessun return
}

// Utilizzo
await SalvaDatiAsync();  // ✅ Attende il completamento
```

### void (Non raccomandato)

```csharp
// ⚠️ EVITARE: Non puoi aspettare void
public async void MetodoAsync() {
    await Task.Delay(1000);
    // Problemi: non puoi aspettare, errori non catturati
}
```

### Diagramma: Tipi di Ritorno

```
┌─────────────────────────────────────────────┐
│  async Task<T>                               │
│  ✅ Restituisce valore                      │
│  ✅ Puoi aspettare                          │
│  ✅ Gestione errori                         │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  async Task                                │
│  ✅ Nessun valore di ritorno               │
│  ✅ Puoi aspettare                          │
│  ✅ Gestione errori                         │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  async void                                 │
│  ❌ Non puoi aspettare                      │
│  ❌ Errori non catturati                   │
│  ⚠️  Usare solo per event handlers        │
└─────────────────────────────────────────────┘
```

---

## 4. Operazioni Asincrone Comuni

### 4.1 HTTP Requests

```csharp
using System.Net.Http;

public class ApiService {
    private HttpClient httpClient = new HttpClient();
    
    public async Task<string> GetDataAsync(string url) {
        // Chiamata HTTP asincrona
        HttpResponseMessage response = await httpClient.GetAsync(url);
        string content = await response.Content.ReadAsStringAsync();
        return content;
    }
    
    public async Task<string> PostDataAsync(string url, string data) {
        var content = new StringContent(data);
        HttpResponseMessage response = await httpClient.PostAsync(url, content);
        return await response.Content.ReadAsStringAsync();
    }
}

// Utilizzo
var service = new ApiService();
string dati = await service.GetDataAsync("https://api.example.com/data");
```

### 4.2 File I/O

```csharp
using System.IO;

public class FileService {
    public async Task<string> LeggiFileAsync(string percorso) {
        // Lettura asincrona di un file
        using (var reader = new StreamReader(percorso)) {
            return await reader.ReadToEndAsync();
        }
    }
    
    public async Task ScriviFileAsync(string percorso, string contenuto) {
        // Scrittura asincrona di un file
        using (var writer = new StreamWriter(percorso)) {
            await writer.WriteAsync(contenuto);
        }
    }
}

// Utilizzo
var fileService = new FileService();
string contenuto = await fileService.LeggiFileAsync("file.txt");
await fileService.ScriviFileAsync("output.txt", contenuto);
```

### 4.3 Database Operations

```csharp
using System.Data.SqlClient;

public class DatabaseService {
    private string connectionString = "Server=...";
    
    public async Task<List<Persona>> GetPersoneAsync() {
        var persone = new List<Persona>();
        
        using (var connection = new SqlConnection(connectionString)) {
            await connection.OpenAsync();  // Connessione asincrona
            
            using (var command = new SqlCommand("SELECT * FROM Persone", connection)) {
                using (var reader = await command.ExecuteReaderAsync()) {  // Lettura asincrona
                    while (await reader.ReadAsync()) {
                        persone.Add(new Persona {
                            Id = reader.GetInt32(0),
                            Nome = reader.GetString(1)
                        });
                    }
                }
            }
        }
        
        return persone;
    }
}
```

### Diagramma: Operazioni Asincrone

```
┌─────────────────────────────────────────────┐
│  Operazioni Asincrone                        │
├─────────────────────────────────────────────┤
│  HTTP Request                                │
│  ↓ await httpClient.GetAsync()              │
│  ↓ Non blocca il thread                      │
│  ↓ Riceve risposta                           │
├─────────────────────────────────────────────┤
│  File I/O                                    │
│  ↓ await File.ReadAllTextAsync()            │
│  ↓ Non blocca il thread                      │
│  ↓ Legge file                                │
├─────────────────────────────────────────────┤
│  Database                                    │
│  ↓ await connection.OpenAsync()             │
│  ↓ await command.ExecuteReaderAsync()      │
│  ↓ Non blocca il thread                      │
│  ↓ Riceve dati                                │
└─────────────────────────────────────────────┘
```

---

## 5. Esecuzione Parallela

### Task.WhenAll

Esegue multiple operazioni asincrone in parallelo.

```csharp
public async Task<string[]> ScaricaTuttiDatiAsync() {
    var task1 = ScaricaDati1Async();
    var task2 = ScaricaDati2Async();
    var task3 = ScaricaDati3Async();
    
    // Attende che tutte le operazioni completino
    string[] risultati = await Task.WhenAll(task1, task2, task3);
    return risultati;
}

// Oppure con array
public async Task<string[]> ScaricaTuttiDatiAsync() {
    var tasks = new[] {
        ScaricaDati1Async(),
        ScaricaDati2Async(),
        ScaricaDati3Async()
    };
    
    return await Task.WhenAll(tasks);
}
```

### Diagramma: Task.WhenAll

```
┌─────────────────────────────────────────────┐
│  Task 1 ────────────────┐                    │
│  Task 2 ────────────────┼─── Task.WhenAll() │
│  Task 3 ────────────────┘                    │
│                         │                    │
│  Tutte in parallelo     │                    │
│  ↓                      │                    │
│  Attende completamento  │                    │
│  ↓                      │                    │
│  Ritorna tutti i risultati                    │
└─────────────────────────────────────────────┘
```

### Task.WhenAny

Attende la prima operazione che completa.

```csharp
public async Task<string> ScaricaPrimoDisponibileAsync() {
    var task1 = ScaricaDaServer1Async();
    var task2 = ScaricaDaServer2Async();
    var task3 = ScaricaDaServer3Async();
    
    // Attende la prima che completa
    Task<string> primoCompletato = await Task.WhenAny(task1, task2, task3);
    return await primoCompletato;
}
```

### Diagramma: Task.WhenAny

```
┌─────────────────────────────────────────────┐
│  Task 1 ────────────────┐                    │
│  Task 2 ────────────✅───┼─── Task.WhenAny() │
│  Task 3 ────────────────┘                    │
│                         │                    │
│  Tutte in parallelo     │                    │
│  ↓                      │                    │
│  Prima completa (Task 2) │                    │
│  ↓                      │                    │
│  Ritorna risultato Task 2                     │
└─────────────────────────────────────────────┘
```

---

## 6. Gestione Errori

### try-catch con async

```csharp
public async Task<string> ScaricaDatiAsync() {
    try {
        string dati = await OperazioneAsincrona();
        return dati;
    }
    catch (HttpRequestException ex) {
        Console.WriteLine($"Errore HTTP: {ex.Message}");
        throw;
    }
    catch (Exception ex) {
        Console.WriteLine($"Errore generico: {ex.Message}");
        throw;
    }
}

// Utilizzo
try {
    string dati = await ScaricaDatiAsync();
}
catch (Exception ex) {
    Console.WriteLine($"Errore: {ex.Message}");
}
```

### Diagramma: Gestione Errori

```
┌─────────────────────────────────────────────┐
│  await OperazioneAsincrona()                │
│  ↓                                          │
│  [Operazione in corso]                      │
│  ↓                                          │
│  ┌───────────┐      ┌───────────┐          │
│  │ Successo  │      │  Errore   │          │
│  │           │      │           │          │
│  │ Continua  │      │  catch    │          │
│  │           │      │  gestisce │          │
│  └───────────┘      └───────────┘          │
└─────────────────────────────────────────────┘
```

---

## 7. ConfigureAwait

### Problema: Context Capture

```csharp
// Per default, await cattura il SynchronizationContext
public async Task MetodoAsync() {
    await OperazioneAsync();  // Cattura il context (UI thread)
    // Continuazione sul UI thread
}
```

### Soluzione: ConfigureAwait(false)

```csharp
// ConfigureAwait(false) - non cattura il context
public async Task MetodoAsync() {
    await OperazioneAsync().ConfigureAwait(false);
    // Continuazione su thread pool (più efficiente)
}
```

### Quando Usare

```csharp
// ✅ Usa ConfigureAwait(false) in librerie
public class LibraryClass {
    public async Task<string> GetDataAsync() {
        // Non ci interessa il context
        return await httpClient.GetStringAsync(url).ConfigureAwait(false);
    }
}

// ⚠️ Non usare in UI code se devi aggiornare l'UI
public async Task UpdateUIAsync() {
    // Senza ConfigureAwait - continua sul UI thread
    string dati = await GetDataAsync();
    textBox.Text = dati;  // ✅ OK - siamo sul UI thread
}

public async Task UpdateUIAsync() {
    // Con ConfigureAwait(false) - attenzione!
    string dati = await GetDataAsync().ConfigureAwait(false);
    textBox.Text = dati;  // ❌ ERRORE! Non siamo sul UI thread!
}
```

---

## 8. Esempi Pratici Completi

### Esempio 1: Scaricamento Multiplo di File

```csharp
public class FileDownloader {
    private HttpClient httpClient = new HttpClient();
    
    public async Task<List<string>> ScaricaFileAsync(List<string> url) {
        var tasks = url.Select(url => ScaricaFileSingoloAsync(url));
        return (await Task.WhenAll(tasks)).ToList();
    }
    
    private async Task<string> ScaricaFileSingoloAsync(string url) {
        Console.WriteLine($"Inizio scaricamento: {url}");
        string contenuto = await httpClient.GetStringAsync(url);
        Console.WriteLine($"Completato: {url}");
        return contenuto;
    }
}

// Utilizzo
var downloader = new FileDownloader();
var urls = new List<string> {
    "https://api.example.com/data1",
    "https://api.example.com/data2",
    "https://api.example.com/data3"
};

List<string> risultati = await downloader.ScaricaFileAsync(urls);
// Tutti i file vengono scaricati in parallelo!
```

### Esempio 2: Servizio con Timeout

```csharp
public class ApiService {
    private HttpClient httpClient = new HttpClient();
    
    public async Task<string> GetDataConTimeoutAsync(string url, int timeoutMs = 5000) {
        using (var cts = new CancellationTokenSource(timeoutMs)) {
            try {
                return await httpClient.GetStringAsync(url, cts.Token);
            }
            catch (TaskCanceledException) {
                throw new TimeoutException("Operazione timeout");
            }
        }
    }
}

// Utilizzo
var service = new ApiService();
try {
    string dati = await service.GetDataConTimeoutAsync("https://api.example.com", 3000);
}
catch (TimeoutException ex) {
    Console.WriteLine($"Timeout: {ex.Message}");
}
```

### Esempio 3: Processing Pipeline Asincrono

```csharp
public class DataProcessor {
    public async Task<string> ProcessDataAsync(string input) {
        // Step 1: Valida
        string validated = await ValidateAsync(input);
        
        // Step 2: Trasforma
        string transformed = await TransformAsync(validated);
        
        // Step 3: Salva
        await SaveAsync(transformed);
        
        return transformed;
    }
    
    private async Task<string> ValidateAsync(string data) {
        await Task.Delay(100);
        return data;
    }
    
    private async Task<string> TransformAsync(string data) {
        await Task.Delay(200);
        return data.ToUpper();
    }
    
    private async Task SaveAsync(string data) {
        await Task.Delay(150);
        // Salva i dati
    }
}

// Utilizzo
var processor = new DataProcessor();
string risultato = await processor.ProcessDataAsync("test");
// Esegue tutti gli step in sequenza
```

---

## 9. Anti-Patterns da Evitare

### ❌ async void

```csharp
// ❌ SBAGLIATO
public async void MetodoAsync() {
    await Task.Delay(1000);
}

// ✅ CORRETTO
public async Task MetodoAsync() {
    await Task.Delay(1000);
}
```

### ❌ Blocking su Async

```csharp
// ❌ SBAGLIATO - Deadlock possibile!
public string ScaricaDati() {
    return ScaricaDatiAsync().Result;  // ❌ Blocca!
}

// ❌ SBAGLIATO
public string ScaricaDati() {
    return ScaricaDatiAsync().GetAwaiter().GetResult();  // ❌ Blocca!
}

// ✅ CORRETTO
public async Task<string> ScaricaDati() {
    return await ScaricaDatiAsync();
}
```

### ❌ Fire-and-Forget Incorretto

```csharp
// ❌ SBAGLIATO - errori non catturati
public void Metodo() {
    OperazioneAsync();  // Errore perso!
}

// ✅ CORRETTO
public void Metodo() {
    _ = OperazioneAsync().ContinueWith(task => {
        if (task.IsFaulted) {
            // Gestisci errore
        }
    }, TaskContinuationOptions.OnlyOnFaulted);
}
```

### Diagramma: Anti-Patterns

```
┌─────────────────────────────────────────────┐
│  ❌ async void                               │
│  - Errori non catturati                     │
│  - Non puoi aspettare                       │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  ❌ .Result o .Wait()                        │
│  - Deadlock possibile                        │
│  - Blocca thread                             │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│  ❌ Fire-and-forget senza gestione errori   │
│  - Errori persi                              │
│  - Difficile debug                           │
└─────────────────────────────────────────────┘
```

---

## 10. Best Practices

### ✅ Cosa Fare

1. **Usa async/await per operazioni I/O**
   ```csharp
   // ✅ CORRETTO
   public async Task<string> GetDataAsync() {
       return await httpClient.GetStringAsync(url);
   }
   ```

2. **Propaga async attraverso la call chain**
   ```csharp
   // ✅ CORRETTO
   public async Task Metodo1Async() {
       await Metodo2Async();
   }
   
   public async Task Metodo2Async() {
       await Metodo3Async();
   }
   ```

3. **Usa Task.WhenAll per operazioni parallele**
   ```csharp
   // ✅ CORRETTO
   var risultati = await Task.WhenAll(task1, task2, task3);
   ```

4. **Usa ConfigureAwait(false) in librerie**
   ```csharp
   // ✅ CORRETTO
   return await httpClient.GetStringAsync(url).ConfigureAwait(false);
   ```

### ❌ Cosa Evitare

1. **Non usare async per operazioni CPU-intensive**
   ```csharp
   // ❌ SBAGLIATO
   public async Task<int> CalcolaAsync() {
       return await Task.Run(() => CalcoloPesante());  // ⚠️ Usa Task.Run solo se necessario
   }
   ```

2. **Non creare async void (tranne event handlers)**
   ```csharp
   // ❌ SBAGLIATO
   public async void MetodoAsync() { }
   ```

3. **Non bloccare su codice async**
   ```csharp
   // ❌ SBAGLIATO
   var risultato = MetodoAsync().Result;
   ```

---

## 11. Performance e Considerazioni

### Vantaggi

- ✅ **Non blocca il thread** - UI responsiva
- ✅ **Scalabilità** - gestisce più operazioni concorrenti
- ✅ **Efficienza** - migliore utilizzo delle risorse

### Quando Usare

- ✅ **Operazioni I/O**: HTTP, File, Database
- ✅ **Operazioni lunghe**: non bloccanti
- ✅ **UI applications**: mantenere responsività

### Quando NON Usare

- ❌ **Operazioni CPU-intensive**: usa Task.Run() o thread separati
- ❌ **Operazioni sincrone semplici**: non serve async
- ❌ **Hot paths critici**: overhead minimo ma presente

---

## 12. Domande Frequenti (FAQ)

### Q: Async rende il codice più veloce?
**R:** No, async non rende il codice più veloce, ma migliora la scalabilità e la responsività dell'applicazione.

### Q: Quanti thread crea async?
**R:** Async non crea thread aggiuntivi. Usa thread pool esistente in modo efficiente.

### Q: Posso usare await senza async?
**R:** No, await può essere usato solo in metodi marcati con async.

### Q: Come gestisco i timeout?
**R:** Usa `CancellationTokenSource` con timeout o `Task.WaitAsync()`.

---

## Conclusioni

async/await in C# sono fondamentali per:

- ✅ Scrivere codice asincrono leggibile
- ✅ Migliorare la responsività delle applicazioni
- ✅ Gestire operazioni I/O in modo efficiente
- ✅ Scalare applicazioni che gestiscono molte operazioni concorrenti

Async/await è il modo moderno e raccomandato per gestire operazioni asincrone in C#.

---

*Documento creato per spiegare async/await in C# con esempi pratici e diagrammi.*

