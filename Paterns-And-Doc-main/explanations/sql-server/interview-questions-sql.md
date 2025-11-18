# Domande e Risposte per Colloqui - SQL Server

## 1. Spiega le differenze tra INNER JOIN, LEFT JOIN, RIGHT JOIN e FULL OUTER JOIN.

**Risposta:**

**INNER JOIN:**
Restituisce solo le righe che hanno corrispondenza in entrambe le tabelle.

```sql
SELECT u.Name, o.OrderDate
FROM Users u
INNER JOIN Orders o ON u.UserId = o.UserId;
```

**LEFT JOIN:**
Restituisce tutte le righe della tabella sinistra e le corrispondenze della destra (NULL se non c'è corrispondenza).

```sql
SELECT u.Name, o.OrderDate
FROM Users u
LEFT JOIN Orders o ON u.UserId = o.UserId;
-- Include utenti senza ordini
```

**RIGHT JOIN:**
Restituisce tutte le righe della tabella destra e le corrispondenze della sinistra.

**FULL OUTER JOIN:**
Restituisce tutte le righe da entrambe le tabelle (NULL dove non c'è corrispondenza).

---

## 2. Cos'è un Indice e quando usarlo?

**Risposta:**
Un indice è una struttura dati che migliora la velocità di recupero dei dati.

**Tipi:**
- **Clustered**: Determina l'ordine fisico dei dati (solo uno per tabella)
- **Non-Clustered**: Struttura separata che punta ai dati

**Quando creare:**
- Colonne usate frequentemente in WHERE
- Colonne usate in JOIN
- Colonne usate in ORDER BY
- Foreign keys

**Quando evitare:**
- Tabelle molto piccole
- Colonne che cambiano frequentemente
- Colonne con molti valori NULL

```sql
CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Orders_UserDate ON Orders(UserId, OrderDate);
```

---

## 3. Spiega le Transazioni ACID.

**Risposta:**
ACID è un acronimo per le proprietà delle transazioni:

**Atomicity (Atomicità):**
Tutte le operazioni della transazione vengono eseguite o nessuna.

**Consistency (Consistenza):**
Il database rimane in uno stato consistente dopo la transazione.

**Isolation (Isolamento):**
Le transazioni concorrenti non interferiscono tra loro.

**Durability (Durevolezza):**
Le modifiche sono permanenti anche dopo un crash.

```sql
BEGIN TRANSACTION;
BEGIN TRY
    UPDATE Accounts SET Balance = Balance - 100 WHERE Id = 1;
    UPDATE Accounts SET Balance = Balance + 100 WHERE Id = 2;
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
END CATCH;
```

---

## 4. Cos'è una Stored Procedure e quali sono i vantaggi?

**Risposta:**
Una stored procedure è codice SQL precompilato salvato nel database.

**Vantaggi:**
- Performance migliori (precompilata)
- Sicurezza (previene SQL injection)
- Logica centralizzata
- Riduce traffico di rete

```sql
CREATE PROCEDURE GetUserOrders
    @UserId INT
AS
BEGIN
    SELECT * FROM Orders WHERE UserId = @UserId;
END;

EXEC GetUserOrders @UserId = 1;
```

---

## 5. Spiega le Normalizzazioni del Database.

**Risposta:**
La normalizzazione riduce la ridondanza dei dati.

**1NF (First Normal Form):**
Ogni colonna contiene valori atomici (non array/liste).

**2NF (Second Normal Form):**
1NF + tutte le colonne non-chiave dipendono completamente dalla chiave primaria.

**3NF (Third Normal Form):**
2NF + nessuna dipendenza transitiva (colonne non-chiave non dipendono da altre colonne non-chiave).

**Esempio di denormalizzazione:**
```sql
-- ❌ Non normalizzato
Orders (OrderId, CustomerName, CustomerEmail, ProductName, Price)

-- ✅ Normalizzato
Orders (OrderId, CustomerId)
Customers (CustomerId, Name, Email)
OrderDetails (OrderDetailId, OrderId, ProductId, Price)
Products (ProductId, Name)
```

---

## 6. Cos'è un Trigger?

**Risposta:**
Un trigger è codice SQL eseguito automaticamente quando si verifica un evento specifico.

```sql
CREATE TRIGGER UpdateStock
ON OrderDetails
AFTER INSERT
AS
BEGIN
    UPDATE Products
    SET Stock = Stock - inserted.Quantity
    FROM Products p
    INNER JOIN inserted ON p.ProductId = inserted.ProductId;
END;
```

**Tipi:**
- AFTER (dopo l'operazione)
- INSTEAD OF (invece dell'operazione)

---

## 7. Spiega le Viste (Views).

**Risposta:**
Una view è una query salvata che può essere usata come una tabella virtuale.

```sql
CREATE VIEW ActiveUsers AS
SELECT UserId, Name, Email
FROM Users
WHERE IsActive = 1;

SELECT * FROM ActiveUsers;
```

**Vantaggi:**
- Nasconde complessità
- Sicurezza (limita accesso a colonne specifiche)
- Riutilizzabilità

---

## 8. Cos'è il Deadlock?

**Risposta:**
Un deadlock si verifica quando due o più transazioni si bloccano a vicenda.

**Esempio:**
- Transazione A blocca Riga 1, attende Riga 2
- Transazione B blocca Riga 2, attende Riga 1
- Deadlock!

**Prevenzione:**
- Accedi alle risorse nello stesso ordine
- Usa timeout
- Mantieni transazioni brevi

```sql
SET DEADLOCK_PRIORITY LOW; -- Questa transazione sarà killata per prima
```

---

## 9. Spiega le Funzioni Aggregate.

**Risposta:**
Le funzioni aggregate calcolano un singolo valore da un insieme di righe.

```sql
SELECT 
    COUNT(*) AS TotalOrders,
    SUM(TotalAmount) AS TotalRevenue,
    AVG(TotalAmount) AS AverageOrder,
    MIN(OrderDate) AS FirstOrder,
    MAX(OrderDate) AS LastOrder
FROM Orders;
```

**Con GROUP BY:**
```sql
SELECT CategoryId, COUNT(*) AS ProductCount
FROM Products
GROUP BY CategoryId;
```

---

## 10. Cos'è il Subquery e quando usarlo?

**Risposta:**
Un subquery è una query annidata dentro un'altra query.

**Correlato:**
```sql
SELECT * FROM Orders o
WHERE o.TotalAmount > (
    SELECT AVG(TotalAmount) 
    FROM Orders 
    WHERE UserId = o.UserId
);
```

**Non correlato:**
```sql
SELECT * FROM Users
WHERE UserId IN (
    SELECT DISTINCT UserId FROM Orders
);
```

**EXISTS vs IN:**
```sql
-- EXISTS è spesso più efficiente
SELECT * FROM Users u
WHERE EXISTS (
    SELECT 1 FROM Orders o WHERE o.UserId = u.UserId
);
```

---

## 11. Spiega le Window Functions.

**Risposta:**
Le window functions calcolano valori su un insieme di righe correlate.

```sql
SELECT 
    Name,
    Salary,
    ROW_NUMBER() OVER (ORDER BY Salary DESC) AS Rank,
    RANK() OVER (ORDER BY Salary DESC) AS RankWithTies,
    LAG(Salary) OVER (ORDER BY Salary) AS PreviousSalary
FROM Employees;
```

**Funzioni comuni:**
- `ROW_NUMBER()`, `RANK()`, `DENSE_RANK()`
- `LAG()`, `LEAD()`
- `SUM()`, `AVG()` con OVER

---

## 12. Cos'è il CTE (Common Table Expression)?

**Risposta:**
Un CTE è una query temporanea definita con WITH.

```sql
WITH HighValueOrders AS (
    SELECT * FROM Orders WHERE TotalAmount > 1000
)
SELECT * FROM HighValueOrders
WHERE Status = 'Completed';
```

**CTE Ricorsivo:**
```sql
WITH EmployeeHierarchy AS (
    -- Anchor
    SELECT EmployeeId, ManagerId, Name, 0 AS Level
    FROM Employees WHERE ManagerId IS NULL
    
    UNION ALL
    
    -- Recursive
    SELECT e.EmployeeId, e.ManagerId, e.Name, eh.Level + 1
    FROM Employees e
    INNER JOIN EmployeeHierarchy eh ON e.ManagerId = eh.EmployeeId
)
SELECT * FROM EmployeeHierarchy;
```

---

## 13. Spiega la Differenza tra DELETE, TRUNCATE e DROP.

**Risposta:**

**DELETE:**
- Elimina righe specifiche
- Può essere rollback
- Mantiene struttura tabella
- Più lento
- Trigger eseguiti

```sql
DELETE FROM Users WHERE Age < 18;
```

**TRUNCATE:**
- Elimina tutte le righe
- Non può essere rollback
- Mantiene struttura tabella
- Più veloce
- Reset IDENTITY
- Trigger NON eseguiti

```sql
TRUNCATE TABLE TempTable;
```

**DROP:**
- Elimina completamente la tabella
- Non può essere rollback

```sql
DROP TABLE Users;
```

---

## 14. Cos'è il Plan Cache?

**Risposta:**
Il Plan Cache memorizza i piani di esecuzione delle query per riutilizzarli.

**Vantaggi:**
- Performance migliori (non ricompila ogni volta)
- Riduce CPU usage

**Problemi:**
- Parameter sniffing
- Cache bloat

**Soluzioni:**
```sql
-- Forza ricompilazione
EXEC sp_executesql @sql WITH RECOMPILE;

-- Pulisci cache
DBCC FREEPROCCACHE;
```

---

## 15. Spiega le Isolation Levels.

**Risposta:**
I livelli di isolamento controllano come le transazioni vedono le modifiche di altre transazioni.

**READ UNCOMMITTED:**
- Legge dati non committati (dirty reads)
- Più veloce, meno sicuro

**READ COMMITTED (default):**
- Legge solo dati committati
- Previene dirty reads

**REPEATABLE READ:**
- Previene dirty reads e non-repeatable reads
- Blocca righe lette

**SERIALIZABLE:**
- Isolamento massimo
- Previene phantom reads
- Più lento

```sql
SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
```

---

*Documento creato per la preparazione ai colloqui tecnici - SQL Server*

