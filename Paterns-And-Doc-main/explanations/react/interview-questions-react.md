# Domande e Risposte per Colloqui - React

## 1. Cos'è React e quali sono i suoi vantaggi?

**Risposta:**
React è una libreria JavaScript per costruire interfacce utente.

**Vantaggi:**
- Component-based architecture
- Virtual DOM per performance
- Unidirectional data flow
- Grande ecosistema
- Reusability dei componenti
- JSX per sintassi dichiarativa

---

## 2. Spiega la differenza tra State e Props.

**Risposta:**

**Props:**
- Dati passati da componente padre a figlio
- Read-only (immutabili)
- Simili a parametri di funzione

```jsx
function User({ name, age }) {
  return <div>{name} - {age}</div>;
}
```

**State:**
- Dati gestiti internamente dal componente
- Mutabile (con setState)
- Causa re-render quando cambia

```jsx
function Counter() {
  const [count, setCount] = useState(0);
  return <button onClick={() => setCount(count + 1)}>{count}</button>;
}
```

---

## 3. Spiega il Virtual DOM.

**Risposta:**
Il Virtual DOM è una rappresentazione in memoria del DOM reale.

**Come funziona:**
1. React crea Virtual DOM quando lo state cambia
2. Confronta (diff) con il Virtual DOM precedente
3. Calcola le modifiche minime necessarie
4. Aggiorna solo quelle parti del DOM reale

**Vantaggi:**
- Performance migliori (meno manipolazioni DOM)
- Batching delle modifiche
- Cross-browser compatibility

---

## 4. Spiega il ciclo di vita dei componenti (Lifecycle).

**Risposta:**

**Class Components (vecchio):**
- `componentDidMount`: Dopo il primo render
- `componentDidUpdate`: Dopo ogni update
- `componentWillUnmount`: Prima della rimozione

**Functional Components con Hooks:**
```jsx
useEffect(() => {
  // componentDidMount + componentDidUpdate
  return () => {
    // componentWillUnmount (cleanup)
  };
}, [dependencies]); // [] = solo mount
```

---

## 5. Spiega useState Hook.

**Risposta:**
`useState` permette di aggiungere state ai functional components.

```jsx
const [count, setCount] = useState(0);
const [user, setUser] = useState({ name: '', email: '' });
```

**Aggiornamento:**
```jsx
// Diretto
setCount(count + 1);

// Funzionale (quando dipende dal valore precedente)
setCount(prev => prev + 1);

// Oggetti
setUser(prev => ({ ...prev, name: 'Mario' }));
```

---

## 6. Spiega useEffect Hook.

**Risposta:**
`useEffect` gestisce side effects (fetch, subscriptions, DOM manipulation).

```jsx
// Esegue dopo ogni render
useEffect(() => {
  document.title = `Count: ${count}`;
});

// Solo al mount
useEffect(() => {
  fetchData();
}, []);

// Quando count cambia
useEffect(() => {
  console.log('Count changed:', count);
}, [count]);

// Con cleanup
useEffect(() => {
  const interval = setInterval(() => {
    setCount(prev => prev + 1);
  }, 1000);
  
  return () => clearInterval(interval);
}, []);
```

---

## 7. Spiega useCallback e useMemo.

**Risposta:**

**useCallback:**
Memorizza una funzione per evitare ricreazioni.

```jsx
const handleClick = useCallback(() => {
  console.log('Clicked');
}, [dependencies]);
```

**useMemo:**
Memorizza il risultato di un calcolo costoso.

```jsx
const expensiveValue = useMemo(() => {
  return computeExpensiveValue(data);
}, [data]);
```

**Quando usare:**
- Quando passi funzioni/valori come props a componenti memoizzati
- Per ottimizzare calcoli costosi

---

## 8. Spiega Context API.

**Risposta:**
Context permette di condividere dati senza prop drilling.

```jsx
const ThemeContext = createContext();

function App() {
  const [theme, setTheme] = useState('light');
  return (
    <ThemeContext.Provider value={{ theme, setTheme }}>
      <Header />
    </ThemeContext.Provider>
  );
}

function Header() {
  const { theme } = useContext(ThemeContext);
  return <div className={theme}>Header</div>;
}
```

---

## 9. Spiega le Keys in React.

**Risposta:**
Le keys aiutano React a identificare quali elementi sono cambiati.

```jsx
{items.map(item => (
  <Item key={item.id} data={item} />
))}
```

**Perché importanti:**
- Aiutano React a riconciliare efficacemente
- Evitano bug quando gli elementi cambiano ordine
- Migliorano performance

**Best Practices:**
- Usa ID univoci quando possibile
- Evita index come key se la lista cambia
- Keys devono essere uniche tra siblings

---

## 10. Spiega il Controlled vs Uncontrolled Components.

**Risposta:**

**Controlled:**
React controlla il valore dell'input.

```jsx
const [value, setValue] = useState('');
<input value={value} onChange={(e) => setValue(e.target.value)} />
```

**Uncontrolled:**
Il DOM controlla il valore.

```jsx
const inputRef = useRef();
<input ref={inputRef} defaultValue="Initial" />
// Accedi con: inputRef.current.value
```

**Quando usare:**
- Controlled: Per validazione, form complessi
- Uncontrolled: Per semplicità, integrazione con librerie non-React

---

## 11. Spiega il Lifting State Up.

**Risposta:**
Spostare lo state al componente padre comune quando più componenti ne hanno bisogno.

```jsx
function App() {
  const [count, setCount] = useState(0);
  return (
    <>
      <Counter count={count} onIncrement={() => setCount(count + 1)} />
      <Display count={count} />
    </>
  );
}
```

---

## 12. Spiega i Custom Hooks.

**Risposta:**
Custom hooks permettono di estrarre logica riutilizzabile.

```jsx
function useFetch(url) {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  
  useEffect(() => {
    fetch(url)
      .then(res => res.json())
      .then(data => {
        setData(data);
        setLoading(false);
      });
  }, [url]);
  
  return { data, loading };
}

// Utilizzo
const { data, loading } = useFetch('/api/users');
```

---

## 13. Spiega React.memo.

**Risposta:**
`React.memo` previene re-render non necessari.

```jsx
const ExpensiveComponent = React.memo(({ data }) => {
  return <div>{/* Expensive rendering */}</div>;
}, (prevProps, nextProps) => {
  // Custom comparison (opzionale)
  return prevProps.data.id === nextProps.data.id;
});
```

---

## 14. Spiega Error Boundaries.

**Risposta:**
Error Boundaries catturano errori JavaScript nei componenti tree.

```jsx
class ErrorBoundary extends React.Component {
  state = { hasError: false };
  
  static getDerivedStateFromError(error) {
    return { hasError: true };
  }
  
  componentDidCatch(error, errorInfo) {
    console.error(error, errorInfo);
  }
  
  render() {
    if (this.state.hasError) {
      return <h1>Something went wrong.</h1>;
    }
    return this.props.children;
  }
}
```

---

## 15. Spiega le Performance Optimization in React.

**Risposta:**

**Tecniche:**
- `React.memo` per componenti
- `useMemo` per valori costosi
- `useCallback` per funzioni
- Code splitting con `React.lazy`
- Virtualizzazione per liste lunghe

```jsx
const LazyComponent = React.lazy(() => import('./LazyComponent'));

function App() {
  return (
    <Suspense fallback={<div>Loading...</div>}>
      <LazyComponent />
    </Suspense>
  );
}
```

---

*Documento creato per la preparazione ai colloqui tecnici - React*

