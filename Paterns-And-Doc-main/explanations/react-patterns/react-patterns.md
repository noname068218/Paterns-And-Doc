# React Patterns

## Introduzione

I **React Patterns** sono soluzioni comuni e best practices per risolvere problemi ricorrenti nello sviluppo con React. Conoscere questi pattern aiuta a scrivere codice più pulito, manutenibile e scalabile.

---

## 1. Higher-Order Components (HOC)

### Cos'è un HOC?

Un Higher-Order Component è una funzione che prende un componente e restituisce un nuovo componente con funzionalità aggiuntive.

### Esempio Base

```jsx
// HOC che aggiunge loading state
function withLoading(Component) {
  return function WithLoadingComponent({ isLoading, ...props }) {
    if (isLoading) {
      return <div>Loading...</div>;
    }
    return <Component {...props} />;
  };
}

// Utilizzo
const UserProfile = ({ user }) => (
  <div>
    <h2>{user.name}</h2>
    <p>{user.email}</p>
  </div>
);

const UserProfileWithLoading = withLoading(UserProfile);

// Nel componente padre
function App() {
  const [loading, setLoading] = useState(true);
  const [user, setUser] = useState(null);

  return (
    <UserProfileWithLoading isLoading={loading} user={user} />
  );
}
```

### HOC con Props Aggiuntive

```jsx
// HOC che aggiunge autenticazione
function withAuth(Component) {
  return function AuthenticatedComponent(props) {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [user, setUser] = useState(null);

    useEffect(() => {
      // Verifica autenticazione
      checkAuth().then(authData => {
        setIsAuthenticated(authData.isAuthenticated);
        setUser(authData.user);
      });
    }, []);

    if (!isAuthenticated) {
      return <div>Please log in</div>;
    }

    return <Component {...props} user={user} />;
  };
}

// Utilizzo
const Dashboard = ({ user }) => (
  <div>
    <h1>Welcome, {user.name}!</h1>
    {/* Dashboard content */}
  </div>
);

const AuthenticatedDashboard = withAuth(Dashboard);
```

### HOC con Hooks (Moderno)

```jsx
// HOC che usa hooks internamente
function withDataFetching(url) {
  return function(Component) {
    return function DataFetchingComponent(props) {
      const [data, setData] = useState(null);
      const [loading, setLoading] = useState(true);
      const [error, setError] = useState(null);

      useEffect(() => {
        fetch(url)
          .then(res => res.json())
          .then(data => {
            setData(data);
            setLoading(false);
          })
          .catch(err => {
            setError(err);
            setLoading(false);
          });
      }, [url]);

      if (loading) return <div>Loading...</div>;
      if (error) return <div>Error: {error.message}</div>;

      return <Component {...props} data={data} />;
    };
  };
}

// Utilizzo
const UserList = ({ data }) => (
  <ul>
    {data.map(user => (
      <li key={user.id}>{user.name}</li>
    ))}
  </ul>
);

const UserListWithData = withDataFetching('/api/users')(UserList);
```

---

## 2. Render Props Pattern

### Cos'è Render Props?

Il Render Props pattern consiste nel passare una funzione come prop che restituisce elementi React. Questo permette di condividere logica tra componenti.

### Esempio Base

```jsx
// Componente con render prop
function Mouse({ render }) {
  const [position, setPosition] = useState({ x: 0, y: 0 });

  useEffect(() => {
    const handleMouseMove = (e) => {
      setPosition({ x: e.clientX, y: e.clientY });
    };

    window.addEventListener('mousemove', handleMouseMove);
    return () => window.removeEventListener('mousemove', handleMouseMove);
  }, []);

  return render(position);
}

// Utilizzo
function App() {
  return (
    <Mouse
      render={({ x, y }) => (
        <div>
          <p>Mouse position: {x}, {y}</p>
        </div>
      )}
    />
  );
}
```

### Render Props con Children

```jsx
// Usando children come funzione
function Toggle({ children }) {
  const [isOn, setIsOn] = useState(false);

  const toggle = () => setIsOn(!isOn);

  return children({ isOn, toggle });
}

// Utilizzo
function App() {
  return (
    <Toggle>
      {({ isOn, toggle }) => (
        <div>
          <button onClick={toggle}>
            {isOn ? 'ON' : 'OFF'}
          </button>
          <p>Status: {isOn ? 'Enabled' : 'Disabled'}</p>
        </div>
      )}
    </Toggle>
  );
}
```

### Render Props per Data Fetching

```jsx
function DataFetcher({ url, children }) {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetch(url)
      .then(res => res.json())
      .then(data => {
        setData(data);
        setLoading(false);
      })
      .catch(err => {
        setError(err);
        setLoading(false);
      });
  }, [url]);

  return children({ data, loading, error });
}

// Utilizzo
function UserList() {
  return (
    <DataFetcher url="/api/users">
      {({ data, loading, error }) => {
        if (loading) return <div>Loading...</div>;
        if (error) return <div>Error: {error.message}</div>;
        return (
          <ul>
            {data.map(user => (
              <li key={user.id}>{user.name}</li>
            ))}
          </ul>
        );
      }}
    </DataFetcher>
  );
}
```

---

## 3. Custom Hooks Pattern

### Cos'è un Custom Hook?

Un Custom Hook è una funzione che inizia con "use" e può chiamare altri hooks. Permette di estrarre logica riutilizzabile.

### Custom Hook per Data Fetching

```jsx
// useFetch custom hook
function useFetch(url) {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    setLoading(true);
    fetch(url)
      .then(res => res.json())
      .then(data => {
        setData(data);
        setLoading(false);
      })
      .catch(err => {
        setError(err);
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
  return <div>{data.name}</div>;
}
```

### Custom Hook per Local Storage

```jsx
function useLocalStorage(key, initialValue) {
  const [storedValue, setStoredValue] = useState(() => {
    try {
      const item = window.localStorage.getItem(key);
      return item ? JSON.parse(item) : initialValue;
    } catch (error) {
      return initialValue;
    }
  });

  const setValue = (value) => {
    try {
      setStoredValue(value);
      window.localStorage.setItem(key, JSON.stringify(value));
    } catch (error) {
      console.error(error);
    }
  };

  return [storedValue, setValue];
}

// Utilizzo
function Settings() {
  const [theme, setTheme] = useLocalStorage('theme', 'light');

  return (
    <div>
      <button onClick={() => setTheme(theme === 'light' ? 'dark' : 'light')}>
        Current theme: {theme}
      </button>
    </div>
  );
}
```

### Custom Hook per Debounce

```jsx
function useDebounce(value, delay) {
  const [debouncedValue, setDebouncedValue] = useState(value);

  useEffect(() => {
    const handler = setTimeout(() => {
      setDebouncedValue(value);
    }, delay);

    return () => {
      clearTimeout(handler);
    };
  }, [value, delay]);

  return debouncedValue;
}

// Utilizzo
function SearchInput() {
  const [searchTerm, setSearchTerm] = useState('');
  const debouncedSearchTerm = useDebounce(searchTerm, 500);

  useEffect(() => {
    if (debouncedSearchTerm) {
      // Esegui ricerca
      console.log('Searching for:', debouncedSearchTerm);
    }
  }, [debouncedSearchTerm]);

  return (
    <input
      value={searchTerm}
      onChange={(e) => setSearchTerm(e.target.value)}
      placeholder="Search..."
    />
  );
}
```

---

## 4. Compound Components Pattern

### Cos'è Compound Components?

Il Compound Components pattern permette di creare componenti che lavorano insieme condividendo stato implicito.

### Esempio: Tabs Component

```jsx
// Context per condividere stato
const TabsContext = createContext();

function Tabs({ children, defaultTab }) {
  const [activeTab, setActiveTab] = useState(defaultTab);

  return (
    <TabsContext.Provider value={{ activeTab, setActiveTab }}>
      <div className="tabs">{children}</div>
    </TabsContext.Provider>
  );
}

function TabList({ children }) {
  return <div className="tab-list">{children}</div>;
}

function Tab({ id, children }) {
  const { activeTab, setActiveTab } = useContext(TabsContext);
  const isActive = activeTab === id;

  return (
    <button
      className={isActive ? 'tab active' : 'tab'}
      onClick={() => setActiveTab(id)}
    >
      {children}
    </button>
  );
}

function TabPanels({ children }) {
  return <div className="tab-panels">{children}</div>;
}

function TabPanel({ id, children }) {
  const { activeTab } = useContext(TabsContext);
  if (activeTab !== id) return null;
  return <div className="tab-panel">{children}</div>;
}

// Utilizzo
function App() {
  return (
    <Tabs defaultTab="tab1">
      <TabList>
        <Tab id="tab1">Tab 1</Tab>
        <Tab id="tab2">Tab 2</Tab>
        <Tab id="tab3">Tab 3</Tab>
      </TabList>
      <TabPanels>
        <TabPanel id="tab1">Content 1</TabPanel>
        <TabPanel id="tab2">Content 2</TabPanel>
        <TabPanel id="tab3">Content 3</TabPanel>
      </TabPanels>
    </Tabs>
  );
}
```

### Esempio: Modal Component

```jsx
const ModalContext = createContext();

function Modal({ children, isOpen, onClose }) {
  if (!isOpen) return null;

  return (
    <ModalContext.Provider value={{ onClose }}>
      <div className="modal-overlay" onClick={onClose}>
        <div className="modal-content" onClick={(e) => e.stopPropagation()}>
          {children}
        </div>
      </div>
    </ModalContext.Provider>
  );
}

function ModalHeader({ children }) {
  const { onClose } = useContext(ModalContext);
  return (
    <div className="modal-header">
      <h2>{children}</h2>
      <button onClick={onClose}>×</button>
    </div>
  );
}

function ModalBody({ children }) {
  return <div className="modal-body">{children}</div>;
}

function ModalFooter({ children }) {
  return <div className="modal-footer">{children}</div>;
}

// Utilizzo
function App() {
  const [isOpen, setIsOpen] = useState(false);

  return (
    <>
      <button onClick={() => setIsOpen(true)}>Open Modal</button>
      <Modal isOpen={isOpen} onClose={() => setIsOpen(false)}>
        <ModalHeader>Title</ModalHeader>
        <ModalBody>Content here</ModalBody>
        <ModalFooter>
          <button onClick={() => setIsOpen(false)}>Close</button>
        </ModalFooter>
      </Modal>
    </>
  );
}
```

---

## 5. Provider Pattern

### Cos'è il Provider Pattern?

Il Provider Pattern usa Context API per condividere stato globale attraverso l'albero dei componenti.

### Esempio: Theme Provider

```jsx
const ThemeContext = createContext();

export function ThemeProvider({ children }) {
  const [theme, setTheme] = useState('light');

  const toggleTheme = () => {
    setTheme(prev => prev === 'light' ? 'dark' : 'light');
  };

  const value = {
    theme,
    toggleTheme,
    isDark: theme === 'dark'
  };

  return (
    <ThemeContext.Provider value={value}>
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

// Utilizzo
function App() {
  return (
    <ThemeProvider>
      <Header />
      <Content />
    </ThemeProvider>
  );
}

function Header() {
  const { theme, toggleTheme } = useTheme();
  return (
    <header className={theme}>
      <button onClick={toggleTheme}>Toggle Theme</button>
    </header>
  );
}
```

### Esempio: Auth Provider

```jsx
const AuthContext = createContext();

export function AuthProvider({ children }) {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Verifica autenticazione
    checkAuth().then(userData => {
      setUser(userData);
      setLoading(false);
    });
  }, []);

  const login = async (email, password) => {
    const userData = await authenticate(email, password);
    setUser(userData);
  };

  const logout = () => {
    setUser(null);
  };

  const value = {
    user,
    loading,
    login,
    logout,
    isAuthenticated: !!user
  };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within AuthProvider');
  }
  return context;
}
```

---

## 6. Controlled vs Uncontrolled Components

### Controlled Components

```jsx
// Il componente React controlla il valore
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

### Uncontrolled Components

```jsx
// Il DOM controlla il valore
function UncontrolledInput() {
  const inputRef = useRef();

  const handleSubmit = () => {
    console.log(inputRef.current.value);
  };

  return (
    <>
      <input ref={inputRef} defaultValue="Initial value" />
      <button onClick={handleSubmit}>Submit</button>
    </>
  );
}
```

---

## 7. Best Practices

### ✅ Quando Usare Ogni Pattern

- **HOC**: Quando devi aggiungere funzionalità a molti componenti
- **Render Props**: Quando vuoi massima flessibilità nella renderizzazione
- **Custom Hooks**: Quando vuoi condividere logica con stato
- **Compound Components**: Quando crei componenti complessi con parti correlate
- **Provider Pattern**: Quando condividi stato globale

### ✅ Performance Considerations

```jsx
// Usa useMemo per calcoli costosi
const expensiveValue = useMemo(() => {
  return computeExpensiveValue(data);
}, [data]);

// Usa useCallback per funzioni passate come props
const handleClick = useCallback(() => {
  // ...
}, [dependencies]);

// Usa React.memo per componenti costosi
const ExpensiveComponent = React.memo(({ data }) => {
  // ...
});
```

---

## 8. Domande Frequenti (FAQ)

### Q: Quando usare HOC vs Custom Hooks?
**R:** Custom Hooks sono preferiti in React moderno. Usa HOC solo se devi aggiungere props o wrappare componenti.

### Q: Render Props vs Custom Hooks?
**R:** Custom Hooks sono più semplici e leggibili. Render Props offrono più flessibilità nella renderizzazione.

### Q: Quando usare Compound Components?
**R:** Quando crei componenti complessi come Tabs, Accordion, Modal dove le parti devono lavorare insieme.

---

## Conclusioni

I React Patterns offrono:

- ✅ **Soluzioni testate** per problemi comuni
- ✅ **Codice riutilizzabile** e manutenibile
- ✅ **Flessibilità** nella progettazione
- ✅ **Best practices** della community

**Ricorda:**
- Scegli il pattern giusto per il problema
- Custom Hooks sono spesso la soluzione migliore
- Compound Components per componenti complessi
- Provider Pattern per stato globale

---

*Documento creato per spiegare i React Patterns con esempi pratici e best practices.*

