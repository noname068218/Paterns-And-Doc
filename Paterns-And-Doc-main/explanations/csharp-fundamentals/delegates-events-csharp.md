# Delegates e Events in C#

## Introduzione

I **Delegates** e gli **Events** sono meccanismi fondamentali in C# per implementare il pattern Observer, il callback pattern e la comunicazione tra componenti in modo disaccoppiato.

---

## 1. Cos'è un Delegate?

### Definizione

Un **delegate** è un tipo che rappresenta riferimenti a metodi con una firma specifica. Permette di trattare i metodi come entità che possono essere assegnate a variabili e passate come parametri.

### Analogia

```
┌─────────────────────────────────────────────┐
│  Delegate = "Puntatore a funzione"           │
│                                              │
│  Come una variabile che contiene            │
│  un riferimento a un metodo                 │
└─────────────────────────────────────────────┘
```

### Sintassi Base

```csharp
// Dichiarazione delegate
public delegate void MyDelegate(string message);

// Metodo compatibile
public void PrintMessage(string msg) {
    Console.WriteLine(msg);
}

// Utilizzo
MyDelegate del = PrintMessage;
del("Ciao!"); // Chiama PrintMessage("Ciao!")
```

---

## 2. Tipi di Delegates

### Delegate Personalizzato

```csharp
// Delegate che accetta un int e ritorna un int
public delegate int MathOperation(int a, int b);

// Metodi compatibili
public int Add(int a, int b) => a + b;
public int Multiply(int a, int b) => a * b;

// Utilizzo
MathOperation operation = Add;
int result = operation(5, 3); // 8

operation = Multiply;
result = operation(5, 3); // 15
```

### Action (Delegate senza ritorno)

```csharp
// Action<T> - accetta parametri, nessun ritorno
Action<string> printAction = (msg) => Console.WriteLine(msg);
printAction("Hello");

// Action con più parametri
Action<int, int> addAction = (a, b) => Console.WriteLine(a + b);
addAction(5, 3); // 8
```

### Func (Delegate con ritorno)

```csharp
// Func<T, TResult> - accetta parametri e ritorna valore
Func<int, int> square = (x) => x * x;
int result = square(5); // 25

// Func con più parametri
Func<int, int, int> add = (a, b) => a + b;
int sum = add(5, 3); // 8

// Func con più parametri di input
Func<int, int, string> format = (a, b) => $"{a} + {b} = {a + b}";
string output = format(5, 3); // "5 + 3 = 8"
```

### Predicate (Delegate che ritorna bool)

```csharp
// Predicate<T> - accetta un parametro, ritorna bool
Predicate<int> isEven = (x) => x % 2 == 0;
bool result = isEven(4); // true

// Utilizzo con List
var numbers = new List<int> { 1, 2, 3, 4, 5 };
var evens = numbers.FindAll(isEven); // { 2, 4 }
```

### Diagramma: Tipi di Delegates

```
┌─────────────────────────────────────────────┐
│           DELEGATES                        │
└─────────────────────────────────────────────┘
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│  Action   │ │   Func    │ │ Predicate │
│           │ │           │ │           │
│ void      │ │ TResult   │ │ bool      │
│ Method()  │ │ Method()  │ │ Method()  │
└───────────┘ └───────────┘ └───────────┘
```

---

## 3. Multicast Delegates

### Concetto

Un delegate può contenere riferimenti a **più metodi**. Quando viene invocato, tutti i metodi vengono chiamati in sequenza.

```csharp
public delegate void Notifier(string message);

public void SendEmail(string msg) {
    Console.WriteLine($"Email: {msg}");
}

public void SendSMS(string msg) {
    Console.WriteLine($"SMS: {msg}");
}

// Multicast delegate
Notifier notifier = SendEmail;
notifier += SendSMS; // Aggiunge altro metodo
notifier += (msg) => Console.WriteLine($"Log: {msg}"); // Lambda

notifier("Notifica importante");
// Output:
// Email: Notifica importante
// SMS: Notifica importante
// Log: Notifica importante
```

### Rimuovere Metodi

```csharp
Notifier notifier = SendEmail;
notifier += SendSMS;

notifier -= SendEmail; // Rimuove SendEmail
notifier("Test"); // Chiama solo SendSMS
```

### Diagramma: Multicast Delegate

```
┌─────────────────────────────────────────────┐
│  Notifier delegate                          │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐ │
│  │ SendEmail│→ │ SendSMS  │→ │   Log    │ │
│  └──────────┘  └──────────┘  └──────────┘ │
└─────────────────────────────────────────────┘
                    │
                    ▼ Invoke()
        ┌───────────────────────┐
        │  Esegue tutti i metodi│
        │  in sequenza          │
        └───────────────────────┘
```

---

## 4. Lambda Expressions

### Sintassi Lambda

```csharp
// Lambda expression - sintassi compatta
Func<int, int> square = x => x * x;

// Lambda con più parametri
Func<int, int, int> add = (a, b) => a + b;

// Lambda con corpo multi-linea
Func<int, int> factorial = n => {
    int result = 1;
    for (int i = 1; i <= n; i++) {
        result *= i;
    }
    return result;
};
```

### Lambda vs Metodi Tradizionali

```csharp
// Metodo tradizionale
public int Add(int a, int b) {
    return a + b;
}

// Lambda equivalente
Func<int, int, int> add = (a, b) => a + b;

// Utilizzo identico
int result1 = Add(5, 3);
int result2 = add(5, 3);
```

---

## 5. Events (Eventi)

### Definizione

Un **event** è un wrapper speciale attorno a un delegate che aggiunge controlli di accesso. Gli eventi permettono di implementare il pattern Observer in modo sicuro.

### Problema con Delegate Pubblico

```csharp
// ❌ PROBLEMA: Delegate pubblico può essere sovrascritto
public class Button {
    public Action Click; // Chiunque può fare: button.Click = null;
}

// Utilizzo pericoloso
var button = new Button();
button.Click = null; // Rimuove tutti i subscriber!
```

### Soluzione: Event

```csharp
// ✅ SOLUZIONE: Event protegge il delegate
public class Button {
    private Action _click;
    
    public event Action Click {
        add { _click += value; }
        remove { _click -= value; }
    }
    
    public void OnClick() {
        _click?.Invoke();
    }
}

// Utilizzo sicuro
var button = new Button();
button.Click += HandleClick; // ✅ OK
// button.Click = null; // ❌ ERRORE! Non permesso
```

### Sintassi Semplificata

```csharp
// ✅ Versione semplificata (più comune)
public class Button {
    public event Action Click;
    
    public void OnClick() {
        Click?.Invoke();
    }
}
```

### Diagramma: Event Pattern

```
┌─────────────────────────────────────────────┐
│  Publisher (Button)                          │
│  - event Action Click                       │
│  + OnClick()                                │
└─────────────────────────────────────────────┘
                    │
                    │ Subscribe (+=)
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌───────────┐ ┌───────────┐ ┌───────────┐
│ Handler 1 │ │ Handler 2 │ │ Handler 3 │
│           │ │           │ │           │
│ OnClick() │ │ OnClick() │ │ OnClick() │
└───────────┘ └───────────┘ └───────────┘
```

---

## 6. Event Handler Pattern Standard

### EventHandler<T>

```csharp
// Classe per dati evento
public class ButtonClickEventArgs : EventArgs {
    public DateTime ClickTime { get; set; }
    public string ButtonName { get; set; }
}

// Publisher
public class Button {
    public event EventHandler<ButtonClickEventArgs> Click;
    
    public void OnClick(string buttonName) {
        Click?.Invoke(this, new ButtonClickEventArgs {
            ClickTime = DateTime.Now,
            ButtonName = buttonName
        });
    }
}

// Subscriber
public class Form {
    private Button button;
    
    public Form() {
        button = new Button();
        button.Click += Button_Click; // Subscribe
    }
    
    private void Button_Click(object sender, ButtonClickEventArgs e) {
        Console.WriteLine($"Button {e.ButtonName} clicked at {e.ClickTime}");
    }
}
```

### EventHandler Standard (senza dati custom)

```csharp
public class Button {
    public event EventHandler Click;
    
    public void OnClick() {
        Click?.Invoke(this, EventArgs.Empty);
    }
}

// Utilizzo
button.Click += (sender, e) => {
    Console.WriteLine("Button clicked!");
};
```

---

## 7. Esempi Pratici Completi

### Esempio 1: Sistema di Notifiche

```csharp
public class NotificationService {
    public event Action<string> NotificationSent;
    
    public void SendNotification(string message) {
        // Logica di invio
        Console.WriteLine($"Invio notifica: {message}");
        
        // Notifica i subscriber
        NotificationSent?.Invoke(message);
    }
}

// Utilizzo
var service = new NotificationService();
service.NotificationSent += (msg) => Console.WriteLine($"Email: {msg}");
service.NotificationSent += (msg) => Console.WriteLine($"SMS: {msg}");
service.NotificationSent += (msg) => Console.WriteLine($"Log: {msg}");

service.SendNotification("Nuovo messaggio");
```

### Esempio 2: Timer con Eventi

```csharp
public class Timer {
    public event Action<int> Tick;
    public event Action Completed;
    
    public async Task StartAsync(int seconds) {
        for (int i = seconds; i > 0; i--) {
            Tick?.Invoke(i);
            await Task.Delay(1000);
        }
        Completed?.Invoke();
    }
}

// Utilizzo
var timer = new Timer();
timer.Tick += (remaining) => Console.WriteLine($"Tempo rimanente: {remaining}s");
timer.Completed += () => Console.WriteLine("Timer completato!");

await timer.StartAsync(5);
```

### Esempio 3: Observer Pattern con Eventi

```csharp
public class Stock {
    private decimal _price;
    
    public event EventHandler<PriceChangedEventArgs> PriceChanged;
    
    public decimal Price {
        get => _price;
        set {
            if (_price != value) {
                decimal oldPrice = _price;
                _price = value;
                PriceChanged?.Invoke(this, new PriceChangedEventArgs {
                    OldPrice = oldPrice,
                    NewPrice = value
                });
            }
        }
    }
}

public class PriceChangedEventArgs : EventArgs {
    public decimal OldPrice { get; set; }
    public decimal NewPrice { get; set; }
}

// Utilizzo
var stock = new Stock();
stock.PriceChanged += (sender, e) => {
    Console.WriteLine($"Prezzo cambiato da {e.OldPrice:C} a {e.NewPrice:C}");
};

stock.Price = 100.50m; // Trigger evento
```

---

## 8. Delegate come Parametri

### Callback Pattern

```csharp
public class DataProcessor {
    // Accetta un delegate come parametro
    public void ProcessData(int[] data, Func<int, int> operation) {
        for (int i = 0; i < data.Length; i++) {
            data[i] = operation(data[i]);
        }
    }
}

// Utilizzo
var processor = new DataProcessor();
var numbers = new[] { 1, 2, 3, 4, 5 };

// Passa lambda come callback
processor.ProcessData(numbers, x => x * 2);
// numbers ora: { 2, 4, 6, 8, 10 }

processor.ProcessData(numbers, x => x + 10);
// numbers ora: { 12, 14, 16, 18, 20 }
```

### LINQ con Delegates

```csharp
var numbers = new[] { 1, 2, 3, 4, 5 };

// LINQ usa delegates internamente
var evens = numbers.Where(x => x % 2 == 0);
var doubled = numbers.Select(x => x * 2);
var sum = numbers.Aggregate((acc, x) => acc + x);
```

---

## 9. Best Practices

### ✅ Cosa Fare

1. **Usa event invece di delegate pubblico**
   ```csharp
   // ✅ CORRETTO
   public event Action Click;
   ```

2. **Usa EventHandler<T> per eventi standard**
   ```csharp
   // ✅ CORRETTO
   public event EventHandler<CustomEventArgs> SomethingHappened;
   ```

3. **Controlla null prima di invocare**
   ```csharp
   // ✅ CORRETTO
   Click?.Invoke();
   ```

4. **Unsubscribe quando non serve più**
   ```csharp
   // ✅ CORRETTO
   button.Click -= HandleClick;
   ```

### ❌ Cosa Evitare

1. **Non esporre delegate pubblici**
   ```csharp
   // ❌ SBAGLIATO
   public Action Click;
   
   // ✅ CORRETTO
   public event Action Click;
   ```

2. **Non dimenticare di fare unsubscribe**
   ```csharp
   // ❌ SBAGLIATO - Memory leak potenziale
   button.Click += HandleClick;
   // Mai rimosso!
   
   // ✅ CORRETTO
   button.Click += HandleClick;
   // ...
   button.Click -= HandleClick; // Cleanup
   ```

3. **Non invocare eventi da costruttore**
   ```csharp
   // ❌ SBAGLIATO - Subscriber potrebbero non essere pronti
   public Button() {
       Click?.Invoke();
   }
   ```

---

## 10. Confronto: Delegate vs Event

### Tabella Comparativa

| Caratteristica | Delegate | Event |
|----------------|----------|-------|
| **Accesso esterno** | Pubblico (può essere sovrascritto) | Protetto (solo += e -=) |
| **Uso principale** | Callback, parametri | Observer pattern |
| **Sicurezza** | Meno sicuro | Più sicuro |
| **Multicast** | ✅ Sì | ✅ Sì |
| **Quando usare** | Parametri metodo | Comunicazione tra componenti |

---

## 11. Domande Frequenti (FAQ)

### Q: Qual è la differenza tra delegate e event?
**R:** Un `event` è un wrapper attorno a un delegate che aggiunge controlli di accesso. Gli eventi possono essere sottoscritti solo con `+=` e `-=`, non possono essere sovrascritti.

### Q: Quando usare Action vs Func vs Predicate?
**R:** 
- `Action` per metodi void
- `Func` per metodi con ritorno
- `Predicate` per metodi che ritornano bool (specifico per condizioni)

### Q: Come evitare memory leak con eventi?
**R:** Sempre fare unsubscribe (`-=`) quando l'oggetto subscriber non è più necessario, specialmente se il publisher vive più a lungo.

### Q: Posso passare più parametri a un event?
**R:** Sì, usando `EventHandler<CustomEventArgs>` dove `CustomEventArgs` contiene tutti i dati necessari.

---

## Conclusioni

Delegates e Events sono fondamentali per:
- ✅ Implementare pattern Observer
- ✅ Disaccoppiare componenti
- ✅ Creare callback flessibili
- ✅ Gestire comunicazione tra oggetti

Usa **delegates** per callback e parametri, **events** per comunicazione tra componenti!

---

_Documento creato per spiegare Delegates e Events in C# con esempi pratici e best practices._

