# React Fundamentals

## Introduzione

**React** è una libreria JavaScript per costruire interfacce utente, sviluppata da Facebook. React utilizza un approccio basato su componenti, permettendo di creare UI riutilizzabili e modulari.

---

## 1. Cos'è React?

### Caratteristiche Principali

- **Component-Based**: UI costruita con componenti riutilizzabili
- **Virtual DOM**: Rendering efficiente delle modifiche
- **Unidirectional Data Flow**: Flusso dati prevedibile
- **JSX**: Sintassi che combina HTML e JavaScript
- **Hooks**: Gestione dello stato e side effects

### Installazione

```bash
# Creare una nuova app React
npx create-react-app my-app

# Oppure con Vite (più veloce)
npm create vite@latest my-app -- --template react
```

---

## 2. Componenti

### Componente Funzionale (Moderno)

```jsx
// Componente funzionale semplice
function Welcome(props) {
  return <h1>Hello, {props.name}!</h1>;
}

// Oppure con arrow function
const Welcome = (props) => {
  return <h1>Hello, {props.name}!</h1>;
};

// Utilizzo
function App() {
  return (
    <div>
      <Welcome name="Mario" />
      <Welcome name="Luigi" />
    </div>
  );
}
```

### Componente con JSX

```jsx
// JSX permette di scrivere HTML-like in JavaScript
function UserCard({ name, email, age }) {
  return (
    <div className="user-card">
      <h2>{name}</h2>
      <p>Email: {email}</p>
      <p>Age: {age}</p>
    </div>
  );
}

// Utilizzo
function App() {
  return (
    <UserCard 
      name="Mario Rossi" 
      email="mario@example.com" 
      age={30} 
    />
  );
}
```

### Componente con Logica

```jsx
function Counter() {
  const [count, setCount] = useState(0);

  const increment = () => {
    setCount(count + 1);
  };

  const decrement = () => {
    setCount(count - 1);
  };

  return (
    <div>
      <h2>Count: {count}</h2>
      <button onClick={increment}>+</button>
      <button onClick={decrement}>-</button>
    </div>
  );
}
```

---

## 3. Props

### Passare Props

```jsx
// Componente padre
function App() {
  const user = {
    name: "Mario",
    email: "mario@example.com"
  };

  return <UserProfile user={user} />;
}

// Componente figlio
function UserProfile({ user }) {
  return (
    <div>
      <h1>{user.name}</h1>
      <p>{user.email}</p>
    </div>
  );
}
```

### Props con Valori di Default

```jsx
function Button({ text = "Click me", onClick, disabled = false }) {
  return (
    <button onClick={onClick} disabled={disabled}>
      {text}
    </button>
  );
}

// Utilizzo
<Button text="Submit" onClick={handleClick} />
<Button /> // Usa valori di default
```

### Children Props

```jsx
function Card({ title, children }) {
  return (
    <div className="card">
      <h2>{title}</h2>
      <div className="card-content">
        {children}
      </div>
    </div>
  );
}

// Utilizzo
<Card title="User Info">
  <p>This is the content</p>
  <button>Action</button>
</Card>
```

---

## 4. State con useState

### useState Hook

```jsx
import { useState } from 'react';

function Counter() {
  // useState restituisce [valore, funzione per aggiornare]
  const [count, setCount] = useState(0);

  return (
    <div>
      <p>You clicked {count} times</p>
      <button onClick={() => setCount(count + 1)}>
        Click me
      </button>
    </div>
  );
}
```

### State con Oggetti

```jsx
function UserForm() {
  const [user, setUser] = useState({
    name: '',
    email: '',
    age: 0
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setUser(prevUser => ({
      ...prevUser,
      [name]: value
    }));
  };

  return (
    <form>
      <input
        name="name"
        value={user.name}
        onChange={handleChange}
        placeholder="Name"
      />
      <input
        name="email"
        value={user.email}
        onChange={handleChange}
        placeholder="Email"
      />
      <input
        name="age"
        type="number"
        value={user.age}
        onChange={handleChange}
        placeholder="Age"
      />
    </form>
  );
}
```

### State con Array

```jsx
function TodoList() {
  const [todos, setTodos] = useState([]);
  const [input, setInput] = useState('');

  const addTodo = () => {
    if (input.trim()) {
      setTodos([...todos, { id: Date.now(), text: input }]);
      setInput('');
    }
  };

  const removeTodo = (id) => {
    setTodos(todos.filter(todo => todo.id !== id));
  };

  return (
    <div>
      <input
        value={input}
        onChange={(e) => setInput(e.target.value)}
        placeholder="Add todo"
      />
      <button onClick={addTodo}>Add</button>
      <ul>
        {todos.map(todo => (
          <li key={todo.id}>
            {todo.text}
            <button onClick={() => removeTodo(todo.id)}>Remove</button>
          </li>
        ))}
      </ul>
    </div>
  );
}
```

---

## 5. useEffect Hook

### useEffect Base

```jsx
import { useState, useEffect } from 'react';

function UserProfile({ userId }) {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  // Esegue dopo ogni render
  useEffect(() => {
    fetchUser(userId).then(data => {
      setUser(data);
      setLoading(false);
    });
  }); // ⚠️ Senza array di dipendenze = esegue sempre

  if (loading) return <div>Loading...</div>;
  return <div>{user?.name}</div>;
}
```

### useEffect con Dipendenze

```jsx
function UserProfile({ userId }) {
  const [user, setUser] = useState(null);

  // Esegue solo quando userId cambia
  useEffect(() => {
    fetchUser(userId).then(setUser);
  }, [userId]); // ✅ Array di dipendenze

  return <div>{user?.name}</div>;
}
```

### useEffect con Cleanup

```jsx
function Timer() {
  const [seconds, setSeconds] = useState(0);

  useEffect(() => {
    const interval = setInterval(() => {
      setSeconds(prev => prev + 1);
    }, 1000);

    // Cleanup: esegue quando il componente si smonta
    return () => {
      clearInterval(interval);
    };
  }, []); // Esegue solo al mount

  return <div>Seconds: {seconds}</div>;
}
```

### useEffect per Side Effects

```jsx
function DocumentTitle({ title }) {
  useEffect(() => {
    document.title = title;

    // Cleanup: ripristina il titolo originale
    return () => {
      document.title = 'My App';
    };
  }, [title]);

  return null;
}
```

---

## 6. Event Handling

### Eventi Base

```jsx
function Button() {
  const handleClick = () => {
    console.log('Button clicked!');
  };

  return <button onClick={handleClick}>Click me</button>;
}
```

### Eventi con Parametri

```jsx
function TodoList() {
  const [todos, setTodos] = useState(['Task 1', 'Task 2']);

  const handleRemove = (index) => {
    setTodos(todos.filter((_, i) => i !== index));
  };

  return (
    <ul>
      {todos.map((todo, index) => (
        <li key={index}>
          {todo}
          <button onClick={() => handleRemove(index)}>Remove</button>
        </li>
      ))}
    </ul>
  );
}
```

### Eventi di Form

```jsx
function LoginForm() {
  const [formData, setFormData] = useState({
    email: '',
    password: ''
  });

  const handleSubmit = (e) => {
    e.preventDefault(); // Previene il refresh della pagina
    console.log('Form submitted:', formData);
  };

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  return (
    <form onSubmit={handleSubmit}>
      <input
        name="email"
        type="email"
        value={formData.email}
        onChange={handleChange}
      />
      <input
        name="password"
        type="password"
        value={formData.password}
        onChange={handleChange}
      />
      <button type="submit">Login</button>
    </form>
  );
}
```

---

## 7. Conditional Rendering

### if/else

```jsx
function Greeting({ isLoggedIn }) {
  if (isLoggedIn) {
    return <h1>Welcome back!</h1>;
  } else {
    return <h1>Please sign in.</h1>;
  }
}
```

### Operatore Ternario

```jsx
function UserStatus({ user }) {
  return (
    <div>
      {user ? (
        <p>Welcome, {user.name}!</p>
      ) : (
        <p>Please log in</p>
      )}
    </div>
  );
}
```

### Operatore &&

```jsx
function Notification({ message }) {
  return (
    <div>
      {message && <div className="notification">{message}</div>}
    </div>
  );
}
```

### Switch Case

```jsx
function StatusBadge({ status }) {
  switch (status) {
    case 'pending':
      return <span className="badge yellow">Pending</span>;
    case 'approved':
      return <span className="badge green">Approved</span>;
    case 'rejected':
      return <span className="badge red">Rejected</span>;
    default:
      return <span className="badge gray">Unknown</span>;
  }
}
```

---

## 8. Lists e Keys

### Rendering Liste

```jsx
function UserList({ users }) {
  return (
    <ul>
      {users.map(user => (
        <li key={user.id}>
          {user.name} - {user.email}
        </li>
      ))}
    </ul>
  );
}
```

### Keys Importanti

```jsx
// ✅ Buono: ID univoco
{items.map(item => (
  <Item key={item.id} data={item} />
))}

// ⚠️ Accettabile: Index (solo se lista non cambia)
{items.map((item, index) => (
  <Item key={index} data={item} />
))}

// ❌ Cattivo: Nessuna key
{items.map(item => (
  <Item data={item} />
))}
```

### Liste con Filtri

```jsx
function FilteredList({ items, filter }) {
  const filteredItems = items.filter(item =>
    item.name.toLowerCase().includes(filter.toLowerCase())
  );

  return (
    <ul>
      {filteredItems.map(item => (
        <li key={item.id}>{item.name}</li>
      ))}
    </ul>
  );
}
```

---

## 9. Forms e Controlled Components

### Controlled Input

```jsx
function ControlledInput() {
  const [value, setValue] = useState('');

  return (
    <input
      value={value}
      onChange={(e) => setValue(e.target.value)}
    />
  );
}
```

### Form Completo

```jsx
function ContactForm() {
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    message: ''
  });

  const [errors, setErrors] = useState({});

  const validate = () => {
    const newErrors = {};
    if (!formData.name) newErrors.name = 'Name is required';
    if (!formData.email) newErrors.email = 'Email is required';
    if (!formData.message) newErrors.message = 'Message is required';
    return newErrors;
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const newErrors = validate();
    if (Object.keys(newErrors).length === 0) {
      console.log('Form is valid:', formData);
    } else {
      setErrors(newErrors);
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
    // Rimuovi errore quando l'utente inizia a digitare
    if (errors[name]) {
      setErrors(prev => ({
        ...prev,
        [name]: ''
      }));
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <input
          name="name"
          value={formData.name}
          onChange={handleChange}
          placeholder="Name"
        />
        {errors.name && <span className="error">{errors.name}</span>}
      </div>
      <div>
        <input
          name="email"
          type="email"
          value={formData.email}
          onChange={handleChange}
          placeholder="Email"
        />
        {errors.email && <span className="error">{errors.email}</span>}
      </div>
      <div>
        <textarea
          name="message"
          value={formData.message}
          onChange={handleChange}
          placeholder="Message"
        />
        {errors.message && <span className="error">{errors.message}</span>}
      </div>
      <button type="submit">Submit</button>
    </form>
  );
}
```

---

## 10. Custom Hooks

### Creare Custom Hook

```jsx
// useCounter.js
import { useState } from 'react';

function useCounter(initialValue = 0) {
  const [count, setCount] = useState(initialValue);

  const increment = () => setCount(count + 1);
  const decrement = () => setCount(count - 1);
  const reset = () => setCount(initialValue);

  return { count, increment, decrement, reset };
}

// Utilizzo
function Counter() {
  const { count, increment, decrement, reset } = useCounter(0);

  return (
    <div>
      <p>Count: {count}</p>
      <button onClick={increment}>+</button>
      <button onClick={decrement}>-</button>
      <button onClick={reset}>Reset</button>
    </div>
  );
}
```

### Custom Hook per Fetch

```jsx
// useFetch.js
import { useState, useEffect } from 'react';

function useFetch(url) {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    setLoading(true);
    fetch(url)
      .then(response => response.json())
      .then(data => {
        setData(data);
        setLoading(false);
      })
      .catch(error => {
        setError(error);
        setLoading(false);
      });
  }, [url]);

  return { data, loading, error };
}

// Utilizzo
function UserProfile({ userId }) {
  const { data, loading, error } = useFetch(`/api/users/${userId}`);

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error.message}</div>;
  return <div>{data?.name}</div>;
}
```

---

## 11. Context API

### Creare Context

```jsx
// ThemeContext.js
import { createContext, useContext, useState } from 'react';

const ThemeContext = createContext();

export function ThemeProvider({ children }) {
  const [theme, setTheme] = useState('light');

  const toggleTheme = () => {
    setTheme(prev => prev === 'light' ? 'dark' : 'light');
  };

  return (
    <ThemeContext.Provider value={{ theme, toggleTheme }}>
      {children}
    </ThemeContext.Provider>
  );
}

export function useTheme() {
  const context = useContext(ThemeContext);
  if (!context) {
    throw new Error('useTheme must be used within ThemeProvider');
  }
  return context;
}
```

### Utilizzare Context

```jsx
// App.js
function App() {
  return (
    <ThemeProvider>
      <Header />
      <Content />
    </ThemeProvider>
  );
}

// Header.js
function Header() {
  const { theme, toggleTheme } = useTheme();

  return (
    <header className={theme}>
      <button onClick={toggleTheme}>Toggle Theme</button>
    </header>
  );
}
```

---

## 12. Best Practices

### ✅ Componenti Piccoli e Riutilizzabili

```jsx
// ✅ Buono: Componente piccolo e focalizzato
function Button({ children, onClick, variant = 'primary' }) {
  return (
    <button className={`btn btn-${variant}`} onClick={onClick}>
      {children}
    </button>
  );
}
```

### ✅ Usa Keys Appropriatamente

```jsx
// ✅ Buono: ID univoco
{items.map(item => <Item key={item.id} data={item} />)}
```

### ✅ Evita Mutazioni Dirette

```jsx
// ❌ Cattivo
const newArray = oldArray;
newArray.push(newItem);

// ✅ Buono
const newArray = [...oldArray, newItem];
```

### ✅ Usa useCallback per Funzioni

```jsx
import { useCallback } from 'react';

function Parent({ items }) {
  const handleClick = useCallback((id) => {
    console.log('Clicked:', id);
  }, []);

  return items.map(item => (
    <Child key={item.id} data={item} onClick={handleClick} />
  ));
}
```

---

## 13. Domande Frequenti (FAQ)

### Q: Qual è la differenza tra state e props?
**R:** Props sono dati passati da un componente padre, state è dati gestiti internamente dal componente. Props sono read-only, state può essere modificato.

### Q: Quando usare useEffect?
**R:** Usa useEffect per:
- Fetching data
- Setting up subscriptions
- Manually changing the DOM
- Cleanup operations

### Q: Cos'è il Virtual DOM?
**R:** Il Virtual DOM è una rappresentazione in memoria del DOM reale. React lo usa per calcolare le modifiche minime necessarie e aggiornare il DOM reale in modo efficiente.

### Q: Quando creare un custom hook?
**R:** Crea un custom hook quando:
- Hai logica riutilizzabile tra componenti
- Vuoi separare la logica dalla presentazione
- Vuoi testare la logica separatamente

---

## 14. Esercizi Pratici

### Esercizio 1: Todo App Completa

```jsx
// Crea una Todo App con:
// - Aggiungere todo
// - Rimuovere todo
// - Marcare come completato
// - Filtrare (tutti, attivi, completati)
```

### Esercizio 2: Weather App

```jsx
// Crea un'app che:
// - Mostra il meteo corrente
// - Permette di cercare per città
// - Mostra loading e error states
```

---

## Conclusioni

React è una potente libreria per costruire UI moderne che offre:

- ✅ **Componenti riutilizzabili** per codice modulare
- ✅ **Hooks** per gestione state e side effects
- ✅ **Virtual DOM** per performance ottimali
- ✅ **Ecosistema ricco** con molte librerie

**Ricorda:**
- Usa componenti funzionali con hooks
- Mantieni componenti piccoli e focalizzati
- Usa keys appropriatamente nelle liste
- Gestisci state e side effects con hooks

---

*Documento creato per spiegare i fondamenti di React con esempi pratici e best practices.*

