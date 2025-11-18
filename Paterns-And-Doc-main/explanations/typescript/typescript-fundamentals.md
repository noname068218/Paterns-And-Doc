# TypeScript Fundamentals

## Introduzione

**TypeScript** è un superset di JavaScript che aggiunge tipizzazione statica opzionale. TypeScript compila in JavaScript puro e offre strumenti per sviluppare applicazioni più robuste e manutenibili.

---

## 1. Cos'è TypeScript?

### Vantaggi di TypeScript

- **Type Safety**: Rileva errori a compile-time invece che runtime
- **IntelliSense**: Miglior autocompletamento nell'IDE
- **Refactoring**: Più sicuro grazie al type checking
- **Documentazione**: I tipi servono come documentazione
- **ECMAScript**: Supporta le ultime features di JavaScript

### Installazione

```bash
# Installazione globale
npm install -g typescript

# In un progetto
npm install --save-dev typescript

# Compilare TypeScript
tsc file.ts

# Watch mode
tsc --watch
```

---

## 2. Tipi Base

### Tipi Primitivi

```typescript
// String
let name: string = "Mario";
let message: string = `Hello, ${name}!`;

// Number
let age: number = 30;
let price: number = 99.99;

// Boolean
let isActive: boolean = true;
let isCompleted: boolean = false;

// Null e Undefined
let value: null = null;
let undefinedValue: undefined = undefined;

// Any (evitare quando possibile)
let anything: any = "can be anything";
anything = 42;
anything = true;
```

### Array

```typescript
// Array di numeri
let numbers: number[] = [1, 2, 3, 4, 5];
let numbers2: Array<number> = [1, 2, 3]; // Sintassi alternativa

// Array di stringhe
let names: string[] = ["Mario", "Luigi", "Peach"];

// Array misto (tuple)
let tuple: [string, number] = ["Mario", 30];
```

### Object

```typescript
// Tipo oggetto
let user: { name: string; age: number } = {
  name: "Mario",
  age: 30
};

// Oggetto con proprietà opzionali
let user2: { name: string; age?: number } = {
  name: "Luigi"
  // age è opzionale
};
```

---

## 3. Type Annotations

### Variabili

```typescript
// Esplicito
let count: number = 0;
let name: string = "Mario";

// Inferenza (TypeScript deduce il tipo)
let count = 0; // number
let name = "Mario"; // string
```

### Funzioni

```typescript
// Parametri e return type
function add(a: number, b: number): number {
  return a + b;
}

// Funzione senza return (void)
function logMessage(message: string): void {
  console.log(message);
}

// Funzione che restituisce never (non ritorna mai)
function throwError(message: string): never {
  throw new Error(message);
}

// Parametri opzionali
function greet(name: string, title?: string): string {
  if (title) {
    return `Hello, ${title} ${name}!`;
  }
  return `Hello, ${name}!`;
}

// Parametri con default
function multiply(a: number, b: number = 1): number {
  return a * b;
}

// Rest parameters
function sum(...numbers: number[]): number {
  return numbers.reduce((total, num) => total + num, 0);
}
```

### Arrow Functions

```typescript
const add = (a: number, b: number): number => {
  return a + b;
};

// Implicit return
const multiply = (a: number, b: number): number => a * b;

// Con parametri opzionali
const greet = (name: string, age?: number): string => {
  return age ? `${name} is ${age} years old` : `Hello, ${name}`;
};
```

---

## 4. Interfaces

### Definire Interface

```typescript
// Interface base
interface User {
  name: string;
  age: number;
  email: string;
}

// Utilizzo
const user: User = {
  name: "Mario",
  age: 30,
  email: "mario@example.com"
};
```

### Proprietà Opzionali e Readonly

```typescript
interface User {
  name: string;
  age: number;
  email?: string; // Opzionale
  readonly id: number; // Solo lettura
}

const user: User = {
  name: "Mario",
  age: 30,
  id: 1
};

// user.id = 2; // ❌ ERRORE: readonly
```

### Interface con Metodi

```typescript
interface Calculator {
  add(a: number, b: number): number;
  subtract(a: number, b: number): number;
}

const calc: Calculator = {
  add: (a, b) => a + b,
  subtract: (a, b) => a - b
};
```

### Estendere Interface

```typescript
interface Person {
  name: string;
  age: number;
}

interface Employee extends Person {
  employeeId: number;
  department: string;
}

const employee: Employee = {
  name: "Mario",
  age: 30,
  employeeId: 123,
  department: "IT"
};
```

### Interface per Funzioni

```typescript
interface MathOperation {
  (a: number, b: number): number;
}

const add: MathOperation = (a, b) => a + b;
const multiply: MathOperation = (a, b) => a * b;
```

---

## 5. Types

### Type Alias

```typescript
// Type alias per tipi primitivi
type ID = string | number;
type Status = "pending" | "approved" | "rejected";

// Utilizzo
const userId: ID = "123";
const orderStatus: Status = "pending";
```

### Type vs Interface

```typescript
// Interface: meglio per oggetti
interface User {
  name: string;
  age: number;
}

// Type: più flessibile
type UserType = {
  name: string;
  age: number;
};

// Type può fare union, intersection, etc.
type StringOrNumber = string | number;
type UserWithId = User & { id: number };
```

### Union Types

```typescript
// Union type
type StringOrNumber = string | number;

function processValue(value: StringOrNumber): string {
  if (typeof value === "string") {
    return value.toUpperCase();
  }
  return value.toString();
}
```

### Intersection Types

```typescript
interface Person {
  name: string;
  age: number;
}

interface Employee {
  employeeId: number;
  department: string;
}

type EmployeePerson = Person & Employee;

const employee: EmployeePerson = {
  name: "Mario",
  age: 30,
  employeeId: 123,
  department: "IT"
};
```

---

## 6. Generics

### Funzioni Generiche

```typescript
// Funzione generica
function identity<T>(arg: T): T {
  return arg;
}

// Utilizzo
const number = identity<number>(42);
const string = identity<string>("Hello");
const inferred = identity("Hello"); // TypeScript inferisce il tipo
```

### Array Generici

```typescript
function getFirstElement<T>(array: T[]): T | undefined {
  return array[0];
}

const numbers = [1, 2, 3];
const firstNumber = getFirstElement(numbers); // number | undefined

const strings = ["a", "b", "c"];
const firstString = getFirstElement(strings); // string | undefined
```

### Interface Generiche

```typescript
interface Box<T> {
  value: T;
}

const numberBox: Box<number> = { value: 42 };
const stringBox: Box<string> = { value: "Hello" };
```

### Constraints

```typescript
// Constraint: T deve avere una proprietà length
function getLength<T extends { length: number }>(item: T): number {
  return item.length;
}

getLength("hello"); // 5
getLength([1, 2, 3]); // 3
// getLength(42); // ❌ ERRORE: number non ha length
```

### Multiple Type Parameters

```typescript
function merge<T, U>(obj1: T, obj2: U): T & U {
  return { ...obj1, ...obj2 };
}

const merged = merge(
  { name: "Mario" },
  { age: 30 }
); // { name: string; age: number }
```

---

## 7. Utility Types

### Partial

```typescript
interface User {
  name: string;
  age: number;
  email: string;
}

// Partial rende tutte le proprietà opzionali
type PartialUser = Partial<User>;
// { name?: string; age?: number; email?: string; }

function updateUser(user: User, updates: Partial<User>): User {
  return { ...user, ...updates };
}
```

### Required

```typescript
interface User {
  name?: string;
  age?: number;
}

// Required rende tutte le proprietà obbligatorie
type RequiredUser = Required<User>;
// { name: string; age: number; }
```

### Readonly

```typescript
interface User {
  name: string;
  age: number;
}

// Readonly rende tutte le proprietà readonly
type ReadonlyUser = Readonly<User>;
// { readonly name: string; readonly age: number; }
```

### Pick e Omit

```typescript
interface User {
  name: string;
  age: number;
  email: string;
  password: string;
}

// Pick: seleziona proprietà specifiche
type UserPublic = Pick<User, "name" | "email">;
// { name: string; email: string; }

// Omit: rimuove proprietà specifiche
type UserWithoutPassword = Omit<User, "password">;
// { name: string; age: number; email: string; }
```

### Record

```typescript
// Record crea un tipo oggetto con chiavi e valori specificati
type UserRoles = Record<string, boolean>;

const roles: UserRoles = {
  admin: true,
  user: false,
  guest: false
};
```

---

## 8. Enums

### Numeric Enums

```typescript
enum Status {
  Pending,    // 0
  Approved,   // 1
  Rejected    // 2
}

const orderStatus: Status = Status.Approved;
console.log(orderStatus); // 1
```

### String Enums

```typescript
enum Status {
  Pending = "PENDING",
  Approved = "APPROVED",
  Rejected = "REJECTED"
}

const orderStatus: Status = Status.Approved;
console.log(orderStatus); // "APPROVED"
```

### Const Enums

```typescript
// Const enum viene inlined nel codice compilato
const enum Direction {
  Up,
  Down,
  Left,
  Right
}

const dir = Direction.Up; // Compila in: const dir = 0;
```

---

## 9. Classes

### Class Base

```typescript
class User {
  name: string;
  age: number;

  constructor(name: string, age: number) {
    this.name = name;
    this.age = age;
  }

  greet(): string {
    return `Hello, I'm ${this.name}`;
  }
}

const user = new User("Mario", 30);
console.log(user.greet());
```

### Access Modifiers

```typescript
class User {
  public name: string;        // Accessibile ovunque
  private age: number;        // Solo dentro la classe
  protected email: string;    // Classe e sottoclassi

  constructor(name: string, age: number, email: string) {
    this.name = name;
    this.age = age;
    this.email = email;
  }

  public getAge(): number {
    return this.age; // ✅ OK: dentro la classe
  }
}

const user = new User("Mario", 30, "mario@example.com");
console.log(user.name); // ✅ OK
// console.log(user.age); // ❌ ERRORE: private
// console.log(user.email); // ❌ ERRORE: protected
```

### Readonly Properties

```typescript
class User {
  readonly id: number;
  name: string;

  constructor(id: number, name: string) {
    this.id = id;
    this.name = name;
  }
}

const user = new User(1, "Mario");
// user.id = 2; // ❌ ERRORE: readonly
```

### Getters e Setters

```typescript
class User {
  private _age: number;

  get age(): number {
    return this._age;
  }

  set age(value: number) {
    if (value < 0) {
      throw new Error("Age cannot be negative");
    }
    this._age = value;
  }
}

const user = new User();
user.age = 30; // Usa il setter
console.log(user.age); // Usa il getter
```

### Inheritance

```typescript
class Person {
  constructor(public name: string, public age: number) {}

  greet(): string {
    return `Hello, I'm ${this.name}`;
  }
}

class Employee extends Person {
  constructor(
    name: string,
    age: number,
    public employeeId: number
  ) {
    super(name, age);
  }

  work(): string {
    return `${this.name} is working`;
  }
}

const employee = new Employee("Mario", 30, 123);
console.log(employee.greet()); // Eredita da Person
console.log(employee.work()); // Metodo proprio
```

### Abstract Classes

```typescript
abstract class Animal {
  constructor(public name: string) {}

  abstract makeSound(): string; // Deve essere implementato

  move(): string {
    return `${this.name} is moving`;
  }
}

class Dog extends Animal {
  makeSound(): string {
    return "Woof!";
  }
}

const dog = new Dog("Buddy");
console.log(dog.makeSound());
console.log(dog.move());
```

---

## 10. Type Guards

### typeof Type Guard

```typescript
function processValue(value: string | number): string {
  if (typeof value === "string") {
    return value.toUpperCase(); // TypeScript sa che è string
  }
  return value.toString(); // TypeScript sa che è number
}
```

### instanceof Type Guard

```typescript
class Dog {
  bark(): string {
    return "Woof!";
  }
}

class Cat {
  meow(): string {
    return "Meow!";
  }
}

function makeSound(animal: Dog | Cat): string {
  if (animal instanceof Dog) {
    return animal.bark(); // TypeScript sa che è Dog
  }
  return animal.meow(); // TypeScript sa che è Cat
}
```

### Custom Type Guard

```typescript
interface Fish {
  swim(): void;
}

interface Bird {
  fly(): void;
}

function isFish(animal: Fish | Bird): animal is Fish {
  return (animal as Fish).swim !== undefined;
}

function move(animal: Fish | Bird) {
  if (isFish(animal)) {
    animal.swim(); // TypeScript sa che è Fish
  } else {
    animal.fly(); // TypeScript sa che è Bird
  }
}
```

---

## 11. TypeScript con React

### Componenti Funzionali

```typescript
import React from 'react';

interface UserProps {
  name: string;
  age: number;
  email?: string;
}

const User: React.FC<UserProps> = ({ name, age, email }) => {
  return (
    <div>
      <h2>{name}</h2>
      <p>Age: {age}</p>
      {email && <p>Email: {email}</p>}
    </div>
  );
};

// Oppure senza React.FC
const User = ({ name, age, email }: UserProps) => {
  // ...
};
```

### useState con TypeScript

```typescript
import { useState } from 'react';

function Counter() {
  const [count, setCount] = useState<number>(0);
  const [name, setName] = useState<string>("");
  const [user, setUser] = useState<User | null>(null);

  return (
    <div>
      <p>Count: {count}</p>
      <button onClick={() => setCount(count + 1)}>Increment</button>
    </div>
  );
}
```

### useEffect con TypeScript

```typescript
import { useEffect, useState } from 'react';

interface User {
  id: number;
  name: string;
  email: string;
}

function UserProfile({ userId }: { userId: number }) {
  const [user, setUser] = useState<User | null>(null);

  useEffect(() => {
    fetch(`/api/users/${userId}`)
      .then(res => res.json())
      .then((data: User) => setUser(data));
  }, [userId]);

  if (!user) return <div>Loading...</div>;
  return <div>{user.name}</div>;
}
```

---

## 12. Best Practices

### ✅ Usa Type Inference Quando Possibile

```typescript
// ✅ Buono: TypeScript inferisce il tipo
const name = "Mario";
const age = 30;

// ⚠️ Non necessario
const name: string = "Mario";
```

### ✅ Evita `any`

```typescript
// ❌ Cattivo
function process(data: any) {
  return data.value;
}

// ✅ Buono
function process<T extends { value: unknown }>(data: T) {
  return data.value;
}
```

### ✅ Usa Interface per Oggetti

```typescript
// ✅ Buono
interface User {
  name: string;
  age: number;
}

// ⚠️ Meno comune
type User = {
  name: string;
  age: number;
};
```

---

## 13. Domande Frequenti (FAQ)

### Q: Qual è la differenza tra type e interface?
**R:** Interface è meglio per oggetti e può essere estesa/merged. Type è più flessibile e può fare union, intersection, etc.

### Q: Quando usare `any`?
**R:** Evita `any` quando possibile. Usa `unknown` se non conosci il tipo, poi fai type checking.

### Q: Come gestire le librerie JavaScript senza tipi?
**R:** Installa i type definitions: `npm install --save-dev @types/library-name`

### Q: TypeScript è più lento di JavaScript?
**R:** No, TypeScript compila in JavaScript. Il tempo di compilazione è l'unico overhead, ma offre molti vantaggi.

---

## 14. Esercizi Pratici

### Esercizio 1: Creare Interface per E-commerce

```typescript
// Crea interface per:
// - Product (id, name, price, category)
// - Cart (items, total)
// - User (name, email, address)
```

### Esercizio 2: Funzione Generica per Array

```typescript
// Crea una funzione generica che:
// - Filtra un array
// - Mappa i valori
// - Restituisce il risultato
```

---

## Conclusioni

TypeScript offre:

- ✅ **Type safety** per codice più robusto
- ✅ **Migliore tooling** con IntelliSense
- ✅ **Refactoring sicuro** grazie al type checking
- ✅ **Documentazione** attraverso i tipi

**Ricorda:**
- Usa type inference quando possibile
- Evita `any`, usa `unknown` se necessario
- Usa interface per oggetti
- Sfrutta i generics per codice riutilizzabile

---

*Documento creato per spiegare i fondamenti di TypeScript con esempi pratici e best practices.*

