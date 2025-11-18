# SQL Server Fundamentals

## Introduzione

**SQL Server** è un sistema di gestione di database relazionali (RDBMS) sviluppato da Microsoft. Questo documento copre i concetti fondamentali, le query essenziali, e le best practices per sviluppatori .NET che lavorano con SQL Server.

---

## 1. Concetti Base

### Cos'è SQL Server?

SQL Server è un database relazionale che:
- Memorizza dati in tabelle organizzate
- Supporta transazioni ACID
- Fornisce sicurezza e controllo degli accessi
- Supporta stored procedures, funzioni e trigger
- Offre strumenti per backup e recovery

### Struttura di un Database

```
Database
├── Tables (Tabelle)
│   ├── Columns (Colonne)
│   └── Rows (Righe)
├── Views (Viste)
├── Stored Procedures
├── Functions
├── Triggers
└── Indexes (Indici)
```

---

## 2. Tipi di Dati

### Tipi Numerici

```sql
-- Interi
INT                 -- -2,147,483,648 a 2,147,483,647
BIGINT              -- Numeri molto grandi
SMALLINT            -- -32,768 a 32,767
TINYINT             -- 0 a 255

-- Decimali
DECIMAL(10, 2)      -- Numeri decimali precisi (10 cifre totali, 2 decimali)
NUMERIC(10, 2)      -- Sinonimo di DECIMAL
FLOAT               -- Numeri a virgola mobile approssimati
REAL                -- FLOAT a precisione singola
MONEY               -- Valute (precisione 4 decimali)
```

### Tipi di Testo

```sql
CHAR(10)            -- Stringa a lunghezza fissa (10 caratteri)
VARCHAR(50)         -- Stringa a lunghezza variabile (max 50)
VARCHAR(MAX)        -- Stringa molto lunga (fino a 2GB)
TEXT                -- Testo molto lungo (deprecato, usare VARCHAR(MAX))
NCHAR(10)           -- Unicode a lunghezza fissa
NVARCHAR(50)        -- Unicode a lunghezza variabile
NVARCHAR(MAX)       -- Unicode molto lungo
```

### Tipi Data/Ora

```sql
DATE                -- Solo data (YYYY-MM-DD)
TIME                -- Solo ora (HH:MM:SS)
DATETIME            -- Data e ora (precisione 3.33ms)
DATETIME2           -- Data e ora migliorata (precisione 100ns)
SMALLDATETIME       -- Data e ora compatta (precisione 1 minuto)
DATETIMEOFFSET      -- Data/ora con fuso orario
```

### Altri Tipi

```sql
BIT                 -- Booleano (0 o 1)
UNIQUEIDENTIFIER    -- GUID
BINARY(50)          -- Dati binari
VARBINARY(MAX)      -- Dati binari grandi
XML                 -- Dati XML
JSON                -- Dati JSON (SQL Server 2016+)
```

---

## 3. Creazione di Tabelle

### Sintassi Base

```sql
-- Creare una tabella
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    BirthDate DATE,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1
);

-- Creare una tabella con foreign key
CREATE TABLE Orders (
    OrderId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    OrderDate DATETIME2 DEFAULT GETDATE(),
    TotalAmount DECIMAL(10, 2) NOT NULL,
    Status NVARCHAR(20) DEFAULT 'Pending',
    
    CONSTRAINT FK_Orders_Users 
        FOREIGN KEY (UserId) REFERENCES Users(UserId)
);
```

### Vincoli (Constraints)

```sql
CREATE TABLE Products (
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Price DECIMAL(10, 2) CHECK (Price > 0),
    Stock INT DEFAULT 0 CHECK (Stock >= 0),
    CategoryId INT NOT NULL,
    
    CONSTRAINT FK_Products_Categories 
        FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId),
    
    CONSTRAINT UQ_ProductName 
        UNIQUE (Name)
);
```

---

## 4. Query SELECT

### SELECT Base

```sql
-- Selezionare tutte le colonne
SELECT * FROM Users;

-- Selezionare colonne specifiche
SELECT FirstName, LastName, Email 
FROM Users;

-- Alias per colonne
SELECT 
    FirstName AS Nome,
    LastName AS Cognome,
    Email AS Email
FROM Users;

-- DISTINCT: rimuove duplicati
SELECT DISTINCT CategoryId FROM Products;
```

### WHERE Clause

```sql
-- Filtri base
SELECT * FROM Users WHERE IsActive = 1;

-- Operatori di confronto
SELECT * FROM Products WHERE Price > 100;
SELECT * FROM Products WHERE Price BETWEEN 50 AND 200;
SELECT * FROM Users WHERE Email LIKE '%@gmail.com';

-- Operatori logici
SELECT * FROM Products 
WHERE Price > 100 AND Stock > 0;

SELECT * FROM Users 
WHERE IsActive = 1 OR CreatedAt > '2024-01-01';

-- IN e NOT IN
SELECT * FROM Products 
WHERE CategoryId IN (1, 2, 3);

SELECT * FROM Users 
WHERE UserId NOT IN (SELECT UserId FROM Orders);

-- IS NULL e IS NOT NULL
SELECT * FROM Users WHERE BirthDate IS NULL;
SELECT * FROM Users WHERE Email IS NOT NULL;
```

### ORDER BY

```sql
-- Ordinamento ascendente
SELECT * FROM Products ORDER BY Price ASC;

-- Ordinamento discendente
SELECT * FROM Products ORDER BY Price DESC;

-- Ordinamento multiplo
SELECT * FROM Users 
ORDER BY LastName ASC, FirstName ASC;

-- TOP: primi N record
SELECT TOP 10 * FROM Products 
ORDER BY Price DESC;
```

### GROUP BY e Aggregazioni

```sql
-- Funzioni di aggregazione
SELECT 
    COUNT(*) AS TotalUsers,
    AVG(Price) AS AveragePrice,
    SUM(TotalAmount) AS TotalSales,
    MIN(Price) AS MinPrice,
    MAX(Price) AS MaxPrice
FROM Products;

-- GROUP BY
SELECT CategoryId, COUNT(*) AS ProductCount
FROM Products
GROUP BY CategoryId;

-- HAVING: filtro su aggregazioni
SELECT CategoryId, AVG(Price) AS AvgPrice
FROM Products
GROUP BY CategoryId
HAVING AVG(Price) > 100;
```

---

## 5. JOIN

### INNER JOIN

```sql
-- Join tra due tabelle
SELECT 
    u.FirstName,
    u.LastName,
    o.OrderDate,
    o.TotalAmount
FROM Users u
INNER JOIN Orders o ON u.UserId = o.UserId;
```

### LEFT JOIN

```sql
-- Tutti gli utenti, anche senza ordini
SELECT 
    u.FirstName,
    u.LastName,
    o.OrderDate,
    o.TotalAmount
FROM Users u
LEFT JOIN Orders o ON u.UserId = o.UserId;
```

### RIGHT JOIN

```sql
-- Tutti gli ordini, anche senza utente (raro)
SELECT 
    u.FirstName,
    u.LastName,
    o.OrderDate,
    o.TotalAmount
FROM Users u
RIGHT JOIN Orders o ON u.UserId = o.UserId;
```

### FULL OUTER JOIN

```sql
-- Tutti i record da entrambe le tabelle
SELECT 
    u.FirstName,
    u.LastName,
    o.OrderDate,
    o.TotalAmount
FROM Users u
FULL OUTER JOIN Orders o ON u.UserId = o.UserId;
```

### Multiple JOINs

```sql
-- Join multipli
SELECT 
    u.FirstName,
    u.LastName,
    o.OrderDate,
    p.Name AS ProductName,
    od.Quantity,
    od.Price
FROM Users u
INNER JOIN Orders o ON u.UserId = o.UserId
INNER JOIN OrderDetails od ON o.OrderId = od.OrderId
INNER JOIN Products p ON od.ProductId = p.ProductId;
```

---

## 6. INSERT, UPDATE, DELETE

### INSERT

```sql
-- Insert singolo
INSERT INTO Users (FirstName, LastName, Email)
VALUES ('Mario', 'Rossi', 'mario@example.com');

-- Insert multiplo
INSERT INTO Users (FirstName, LastName, Email)
VALUES 
    ('Luigi', 'Verdi', 'luigi@example.com'),
    ('Anna', 'Bianchi', 'anna@example.com');

-- Insert da SELECT
INSERT INTO Users (FirstName, LastName, Email)
SELECT FirstName, LastName, Email
FROM TempUsers;
```

### UPDATE

```sql
-- Update singolo
UPDATE Users 
SET Email = 'nuovaemail@example.com'
WHERE UserId = 1;

-- Update multiplo
UPDATE Products 
SET Price = Price * 1.1  -- Aumenta del 10%
WHERE CategoryId = 1;

-- Update con JOIN
UPDATE p
SET p.Stock = p.Stock - od.Quantity
FROM Products p
INNER JOIN OrderDetails od ON p.ProductId = od.ProductId
WHERE od.OrderId = 123;
```

### DELETE

```sql
-- Delete con WHERE
DELETE FROM Users 
WHERE UserId = 1;

-- Delete con JOIN
DELETE u
FROM Users u
LEFT JOIN Orders o ON u.UserId = o.UserId
WHERE o.OrderId IS NULL;  -- Elimina utenti senza ordini

-- TRUNCATE: elimina tutti i record (più veloce)
TRUNCATE TABLE TempTable;
```

---

## 7. Indici (Indexes)

### Cos'è un Indice?

Un indice migliora le performance delle query creando una struttura dati che permette di trovare rapidamente i record.

### Creare Indici

```sql
-- Indice non clusterizzato (default)
CREATE INDEX IX_Users_Email ON Users(Email);

-- Indice univoco
CREATE UNIQUE INDEX IX_Users_Email_Unique ON Users(Email);

-- Indice composto
CREATE INDEX IX_Orders_UserDate ON Orders(UserId, OrderDate);

-- Indice clusterizzato (solo uno per tabella)
CREATE CLUSTERED INDEX IX_Orders_OrderId ON Orders(OrderId);

-- Indice con colonne incluse
CREATE INDEX IX_Orders_UserId 
ON Orders(UserId) 
INCLUDE (OrderDate, TotalAmount);
```

### Quando Creare Indici

✅ **Crea indici su:**
- Colonne usate frequentemente in WHERE
- Colonne usate in JOIN
- Colonne usate in ORDER BY
- Foreign keys

❌ **Non creare indici su:**
- Tabelle molto piccole
- Colonne raramente usate in query
- Colonne con molti valori NULL
- Colonne che cambiano frequentemente

---

## 8. Transazioni

### Cos'è una Transazione?

Una transazione è un'unità di lavoro che deve essere eseguita completamente o non eseguita affatto (ACID).

### Sintassi Transazioni

```sql
-- Inizio transazione
BEGIN TRANSACTION;

BEGIN TRY
    -- Operazioni
    INSERT INTO Orders (UserId, TotalAmount)
    VALUES (1, 100.00);
    
    UPDATE Products 
    SET Stock = Stock - 1 
    WHERE ProductId = 5;
    
    -- Commit: conferma le modifiche
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    -- Rollback: annulla le modifiche
    ROLLBACK TRANSACTION;
    
    -- Gestione errore
    PRINT 'Errore: ' + ERROR_MESSAGE();
END CATCH;
```

### Esempio Pratico

```sql
BEGIN TRANSACTION;

BEGIN TRY
    -- 1. Crea ordine
    DECLARE @OrderId INT;
    INSERT INTO Orders (UserId, TotalAmount)
    VALUES (1, 500.00);
    SET @OrderId = SCOPE_IDENTITY();
    
    -- 2. Aggiungi dettagli ordine
    INSERT INTO OrderDetails (OrderId, ProductId, Quantity, Price)
    VALUES (@OrderId, 1, 2, 250.00);
    
    -- 3. Aggiorna stock
    UPDATE Products 
    SET Stock = Stock - 2 
    WHERE ProductId = 1;
    
    COMMIT TRANSACTION;
    PRINT 'Ordine creato con successo';
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'Errore nella creazione dell''ordine: ' + ERROR_MESSAGE();
END CATCH;
```

---

## 9. Stored Procedures

### Cos'è una Stored Procedure?

Una stored procedure è un insieme di istruzioni SQL precompilate salvate nel database.

### Creare Stored Procedures

```sql
-- Stored procedure semplice
CREATE PROCEDURE GetUserById
    @UserId INT
AS
BEGIN
    SELECT * FROM Users WHERE UserId = @UserId;
END;

-- Eseguire stored procedure
EXEC GetUserById @UserId = 1;
```

### Stored Procedure con Parametri

```sql
CREATE PROCEDURE CreateUser
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @UserId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO Users (FirstName, LastName, Email)
    VALUES (@FirstName, @LastName, @Email);
    
    SET @UserId = SCOPE_IDENTITY();
END;

-- Eseguire
DECLARE @NewUserId INT;
EXEC CreateUser 
    @FirstName = 'Mario',
    @LastName = 'Rossi',
    @Email = 'mario@example.com',
    @UserId = @NewUserId OUTPUT;
    
PRINT 'Nuovo UserId: ' + CAST(@NewUserId AS NVARCHAR);
```

### Stored Procedure con Transazioni

```sql
CREATE PROCEDURE ProcessOrder
    @UserId INT,
    @ProductId INT,
    @Quantity INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Verifica stock
        DECLARE @Stock INT;
        SELECT @Stock = Stock FROM Products WHERE ProductId = @ProductId;
        
        IF @Stock < @Quantity
        BEGIN
            RAISERROR('Stock insufficiente', 16, 1);
            RETURN;
        END
        
        -- Crea ordine
        DECLARE @OrderId INT;
        DECLARE @Price DECIMAL(10, 2);
        SELECT @Price = Price FROM Products WHERE ProductId = @ProductId;
        
        INSERT INTO Orders (UserId, TotalAmount)
        VALUES (@UserId, @Price * @Quantity);
        SET @OrderId = SCOPE_IDENTITY();
        
        -- Aggiungi dettagli
        INSERT INTO OrderDetails (OrderId, ProductId, Quantity, Price)
        VALUES (@OrderId, @ProductId, @Quantity, @Price);
        
        -- Aggiorna stock
        UPDATE Products 
        SET Stock = Stock - @Quantity 
        WHERE ProductId = @ProductId;
        
        COMMIT TRANSACTION;
        SELECT @OrderId AS OrderId;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
```

---

## 10. Funzioni

### Funzioni Scalar

```sql
-- Funzione che calcola l'età
CREATE FUNCTION CalculateAge(@BirthDate DATE)
RETURNS INT
AS
BEGIN
    DECLARE @Age INT;
    SET @Age = DATEDIFF(YEAR, @BirthDate, GETDATE());
    
    IF DATEADD(YEAR, @Age, @BirthDate) > GETDATE()
        SET @Age = @Age - 1;
    
    RETURN @Age;
END;

-- Utilizzo
SELECT 
    FirstName,
    BirthDate,
    dbo.CalculateAge(BirthDate) AS Age
FROM Users;
```

### Funzioni Table-Valued

```sql
-- Funzione che restituisce una tabella
CREATE FUNCTION GetUserOrders(@UserId INT)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        OrderId,
        OrderDate,
        TotalAmount,
        Status
    FROM Orders
    WHERE UserId = @UserId
);

-- Utilizzo
SELECT * FROM dbo.GetUserOrders(1);
```

---

## 11. Views

### Cos'è una View?

Una view è una query salvata che può essere usata come una tabella virtuale.

### Creare Views

```sql
-- View semplice
CREATE VIEW ActiveUsers AS
SELECT 
    UserId,
    FirstName,
    LastName,
    Email
FROM Users
WHERE IsActive = 1;

-- Utilizzo
SELECT * FROM ActiveUsers;
```

### View con JOIN

```sql
CREATE VIEW OrderSummary AS
SELECT 
    u.FirstName + ' ' + u.LastName AS CustomerName,
    o.OrderId,
    o.OrderDate,
    o.TotalAmount,
    o.Status,
    COUNT(od.OrderDetailId) AS ItemCount
FROM Orders o
INNER JOIN Users u ON o.UserId = u.UserId
LEFT JOIN OrderDetails od ON o.OrderId = od.OrderId
GROUP BY 
    u.FirstName,
    u.LastName,
    o.OrderId,
    o.OrderDate,
    o.TotalAmount,
    o.Status;

-- Utilizzo
SELECT * FROM OrderSummary WHERE Status = 'Completed';
```

---

## 12. Performance e Ottimizzazione

### Best Practices

```sql
-- ✅ Usa WHERE per filtrare prima di JOIN
SELECT * FROM Orders o
INNER JOIN Users u ON o.UserId = u.UserId
WHERE o.OrderDate > '2024-01-01';  -- Filtra prima

-- ✅ Evita SELECT *
SELECT FirstName, LastName, Email 
FROM Users;  -- Specifica colonne

-- ✅ Usa EXISTS invece di IN per subquery
SELECT * FROM Users u
WHERE EXISTS (
    SELECT 1 FROM Orders o 
    WHERE o.UserId = u.UserId
);

-- ✅ Usa UNION ALL invece di UNION se non servono duplicati
SELECT * FROM Table1
UNION ALL
SELECT * FROM Table2;
```

### Query Plan

```sql
-- Visualizza execution plan
SET STATISTICS IO ON;
SET STATISTICS TIME ON;

SELECT * FROM Users WHERE Email = 'test@example.com';

SET STATISTICS IO OFF;
SET STATISTICS TIME OFF;
```

---

## 13. Domande Frequenti (FAQ)

### Q: Qual è la differenza tra CHAR e VARCHAR?
**R:** CHAR ha lunghezza fissa (riempie con spazi), VARCHAR ha lunghezza variabile. Usa VARCHAR per risparmiare spazio.

### Q: Quando usare INNER JOIN vs LEFT JOIN?
**R:** Usa INNER JOIN quando vuoi solo i record che hanno corrispondenza in entrambe le tabelle. Usa LEFT JOIN quando vuoi tutti i record della tabella sinistra.

### Q: Cosa sono gli indici clusterizzati e non clusterizzati?
**R:** Un indice clusterizzato determina l'ordine fisico dei dati nella tabella (solo uno per tabella). Gli indici non clusterizzati sono strutture separate che puntano ai dati.

### Q: Quando usare stored procedures vs query dirette?
**R:** Usa stored procedures per:
- Logica complessa riutilizzabile
- Sicurezza (prevenire SQL injection)
- Performance (precompilate)
- Centralizzazione della logica

### Q: Come gestire le transazioni in C#?
**R:** Usa `SqlTransaction` o `TransactionScope`:

```csharp
using (var connection = new SqlConnection(connectionString))
{
    connection.Open();
    using (var transaction = connection.BeginTransaction())
    {
        try
        {
            // Operazioni
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
```

---

## 14. Esercizi Pratici

### Esercizio 1: Query Complessa

```sql
-- Trova gli utenti che hanno fatto ordini per più di 1000€ totali
SELECT 
    u.UserId,
    u.FirstName + ' ' + u.LastName AS FullName,
    SUM(o.TotalAmount) AS TotalSpent
FROM Users u
INNER JOIN Orders o ON u.UserId = o.UserId
WHERE o.Status = 'Completed'
GROUP BY u.UserId, u.FirstName, u.LastName
HAVING SUM(o.TotalAmount) > 1000
ORDER BY TotalSpent DESC;
```

### Esercizio 2: Stored Procedure

```sql
-- Crea una stored procedure che restituisce i prodotti più venduti
CREATE PROCEDURE GetTopSellingProducts
    @TopCount INT = 10
AS
BEGIN
    SELECT TOP (@TopCount)
        p.ProductId,
        p.Name,
        SUM(od.Quantity) AS TotalSold,
        SUM(od.Quantity * od.Price) AS TotalRevenue
    FROM Products p
    INNER JOIN OrderDetails od ON p.ProductId = od.ProductId
    INNER JOIN Orders o ON od.OrderId = o.OrderId
    WHERE o.Status = 'Completed'
    GROUP BY p.ProductId, p.Name
    ORDER BY TotalSold DESC;
END;
```

---

## Conclusioni

SQL Server è un potente RDBMS che offre:

- ✅ **Gestione dati robusta** con transazioni ACID
- ✅ **Performance ottimizzate** con indici
- ✅ **Sicurezza** con stored procedures
- ✅ **Flessibilità** con views e funzioni

**Ricorda:**
- Usa indici per migliorare le performance
- Usa transazioni per garantire consistenza
- Usa stored procedures per logica complessa
- Ottimizza le query evitando SELECT * e usando WHERE appropriatamente

---

*Documento creato per spiegare i fondamenti di SQL Server con esempi pratici e best practices.*

