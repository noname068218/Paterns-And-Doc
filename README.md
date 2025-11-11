# 🎯 C# & .NET Learning Project

A comprehensive learning sandbox for mastering C#, .NET, ASP.NET Core, algorithms, and software architecture.

## 📚 Project Purpose

This repository serves as a personal training ground for:
- Practical and theoretical learning of **C# 12** and **.NET 8**
- Implementing **SOLID principles** and **design patterns**
- Experimenting with **ASP.NET Core Web API**
- Practicing **algorithms and data structures**
- Testing new libraries and frameworks
- Applying **Clean Architecture**, **CQRS**, and **DDD** concepts

## 🏗️ Project Structure

```
paterns/
├── Test/                           # Main C# project
│   ├── src/
│   │   ├── OopPrinciples/         # OOP principles demonstrations
│   │   │   ├── Abstraction/
│   │   │   ├── Encapsulation/
│   │   │   ├── Polymorphism/
│   │   │   └── Coupling/
│   │   └── SOLID/                 # SOLID principles examples
│   │       ├── S/                 # Single Responsibility
│   │       ├── O/                 # Open/Closed
│   │       ├── L/                 # Liskov Substitution
│   │       ├── I/                 # Interface Segregation
│   │       └── D/                 # Dependency Inversion
│   └── documentation2/            # Code examples and demos
│       ├── constructor/
│       ├── properties/
│       ├── recursFunctions/
│       ├── refOutParams/
│       ├── structDoc/
│       └── valueRefTypes/
└── explanations/                  # Theoretical documentation (Markdown)
    ├── principi-solid.md
    ├── principi-oop.md
    ├── design-patterns.md
    ├── clean-architecture.md
    ├── async-await-csharp.md
    ├── linq-csharp.md
    ├── entity-framework.md
    └── ... and more
```

## 🧩 Technologies & Concepts

### Core Technologies
- **C# 12** / **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **xUnit** for testing

### Principles & Patterns
- ✅ **SOLID Principles** (Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, Dependency Inversion)
- ✅ **OOP Principles** (Encapsulation, Abstraction, Inheritance, Polymorphism, Coupling/Cohesion)
- ✅ **Design Patterns** (Creational, Structural, Behavioral)
- ✅ **Clean Architecture**
- ✅ **CQRS** (Command Query Responsibility Segregation)
- ✅ **DDD** (Domain-Driven Design) basics

### Key Topics Covered
- 🔹 Async/Await and Task-based programming
- 🔹 LINQ (Language Integrated Query)
- 🔹 Generics and Collections
- 🔹 Delegates, Events, and Lambda expressions
- 🔹 Exception Handling and Logging
- 🔹 Dependency Injection
- 🔹 Middleware and Configuration
- 🔹 Threading and Concurrency
- 🔹 Value Types vs Reference Types (Stack/Heap)
- 🔹 Algorithms and Data Structures

## 🧠 Learning Focus

This project is designed for developers aiming to:
- Transition from **Junior to Mid-level** C#/.NET developer
- Build a solid foundation in **software architecture**
- Understand **design patterns** and when to apply them
- Master **ASP.NET Core Web API** development
- Practice **clean code** and maintainable solutions

## 🚀 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 / VS Code / Rider

### Running the Project

```bash
# Clone the repository
git clone <your-gitlab-repo-url>
cd paterns

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
cd Test
dotnet run
```

## 📖 Documentation

The `explanations/` folder contains detailed markdown documentation (in Italian/Russian) covering:
- Theoretical concepts
- Step-by-step explanations
- Code examples with comments
- Best practices and optimization tips

## 🎯 Code Style & Guidelines

- **Clean Code**: following SOLID and DRY principles
- **XML Documentation**: all public methods and classes documented
- **Unit Tests**: using xUnit framework
- **Async Programming**: leveraging async/await where applicable
- **One Class - One Responsibility**: clear separation of concerns

## 🧪 Examples Included

### SOLID Principles
- **S**: User management with separated responsibilities
- **O**: Extensible shape calculation system
- **L**: Proper inheritance hierarchies (Rectangle/Square)
- **I**: Segregated interfaces for 2D/3D shapes
- **D**: Dependency injection with Engine/Car example

### OOP Principles
- **Encapsulation**: BankAccount with private fields
- **Abstraction**: Email service abstraction
- **Polymorphism**: Vehicle hierarchy (Car, Motorcycle, Plane)
- **Coupling**: Low coupling examples with Order/EmailSender

## 📝 Notes

- This is a **learning project**, not production code
- Code is optimized for **readability** and **maintainability**
- Examples include **step-by-step comments** explaining the logic
- Focused on understanding **how things work under the hood**

## 🤝 Contributing

This is a personal learning project, but suggestions and feedback are always welcome!

## 📄 License

This project is open-source and available for educational purposes.

---

**Happy Learning! 🚀**

