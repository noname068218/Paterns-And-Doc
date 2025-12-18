# Вложенные циклы (Nested Loops) в C#

## Введение

**Вложенные циклы** — это циклы, расположенные внутри других циклов. Они используются для обработки многомерных структур данных, матриц, таблиц и других сложных структур, где требуется перебрать элементы в нескольких измерениях.

---

## 1. Что такое вложенные циклы?

### Определение

Вложенный цикл — это цикл, который находится внутри тела другого цикла. Внутренний цикл выполняется полностью для каждой итерации внешнего цикла.

### Базовый пример

```csharp
// Внешний цикл
for (int i = 0; i < 3; i++) {
    Console.WriteLine($"Внешний цикл: {i}");
    
    // Внутренний цикл
    for (int j = 0; j < 2; j++) {
        Console.WriteLine($"  Внутренний цикл: {j}");
    }
}

// Вывод:
// Внешний цикл: 0
//   Внутренний цикл: 0
//   Внутренний цикл: 1
// Внешний цикл: 1
//   Внутренний цикл: 0
//   Внутренний цикл: 1
// Внешний цикл: 2
//   Внутренний цикл: 0
//   Внутренний цикл: 1
```

### Диаграмма: Выполнение вложенных циклов

```
┌─────────────────────────────────────────────┐
│  Внешний цикл (i = 0)                       │
│  │                                          │
│  │  ┌──────────────────────────────────┐   │
│  │  │  Внутренний цикл (j = 0)         │   │
│  │  │  Выполняется полностью            │   │
│  │  │  j = 0, j = 1                     │   │
│  │  └──────────────────────────────────┘   │
│  │                                          │
│  ▼                                          │
│  Внешний цикл (i = 1)                       │
│  │                                          │
│  │  ┌──────────────────────────────────┐   │
│  │  │  Внутренний цикл (j = 0)         │   │
│  │  │  Выполняется снова полностью      │   │
│  │  │  j = 0, j = 1                     │   │
│  │  └──────────────────────────────────┘   │
│  │                                          │
│  ▼                                          │
│  Внешний цикл (i = 2)                       │
│  ...                                        │
└─────────────────────────────────────────────┘
```

---

## 2. Типы вложенных циклов

### for внутри for

```csharp
// Классический пример: таблица умножения
for (int i = 1; i <= 10; i++) {
    for (int j = 1; j <= 10; j++) {
        Console.Write($"{i * j,4}"); // Выравнивание на 4 символа
    }
    Console.WriteLine(); // Новая строка после каждой строки таблицы
}
```

### while внутри for

```csharp
for (int i = 0; i < 5; i++) {
    int j = 0;
    while (j < 3) {
        Console.WriteLine($"i = {i}, j = {j}");
        j++;
    }
}
```

### foreach внутри foreach

```csharp
var matrix = new List<List<int>> {
    new List<int> { 1, 2, 3 },
    new List<int> { 4, 5, 6 },
    new List<int> { 7, 8, 9 }
};

foreach (var row in matrix) {
    foreach (var element in row) {
        Console.Write($"{element} ");
    }
    Console.WriteLine();
}
```

### do-while внутри for

```csharp
for (int i = 0; i < 3; i++) {
    int j = 0;
    do {
        Console.WriteLine($"i = {i}, j = {j}");
        j++;
    } while (j < 2);
}
```

---

## 3. Работа с двумерными массивами

### Вывод матрицы

```csharp
int[,] matrix = {
    { 1, 2, 3 },
    { 4, 5, 6 },
    { 7, 8, 9 }
};

// Вывод матрицы
for (int i = 0; i < matrix.GetLength(0); i++) {
    for (int j = 0; j < matrix.GetLength(1); j++) {
        Console.Write($"{matrix[i, j]} ");
    }
    Console.WriteLine();
}

// Вывод:
// 1 2 3
// 4 5 6
// 7 8 9
```

### Заполнение матрицы

```csharp
int rows = 3;
int cols = 4;
int[,] matrix = new int[rows, cols];

// Заполнение матрицы значениями
for (int i = 0; i < rows; i++) {
    for (int j = 0; j < cols; j++) {
        matrix[i, j] = i * cols + j + 1; // Последовательные числа
    }
}
```

### Поиск элемента в матрице

```csharp
int[,] matrix = {
    { 1, 2, 3 },
    { 4, 5, 6 },
    { 7, 8, 9 }
};

int target = 5;
bool found = false;
int rowIndex = -1, colIndex = -1;

for (int i = 0; i < matrix.GetLength(0); i++) {
    for (int j = 0; j < matrix.GetLength(1); j++) {
        if (matrix[i, j] == target) {
            found = true;
            rowIndex = i;
            colIndex = j;
            break; // Выход из внутреннего цикла
        }
    }
    if (found) break; // Выход из внешнего цикла
}

if (found) {
    Console.WriteLine($"Найдено на позиции [{rowIndex}, {colIndex}]");
}
```

---

## 4. Трёхмерные и многомерные структуры

### Трёхмерный массив

```csharp
int[,,] cube = new int[2, 3, 4];

// Заполнение трёхмерного массива
for (int i = 0; i < cube.GetLength(0); i++) {
    for (int j = 0; j < cube.GetLength(1); j++) {
        for (int k = 0; k < cube.GetLength(2); k++) {
            cube[i, j, k] = i * 100 + j * 10 + k;
        }
    }
}

// Вывод трёхмерного массива
for (int i = 0; i < cube.GetLength(0); i++) {
    Console.WriteLine($"Слой {i}:");
    for (int j = 0; j < cube.GetLength(1); j++) {
        for (int k = 0; k < cube.GetLength(2); k++) {
            Console.Write($"{cube[i, j, k],4} ");
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}
```

### Массив массивов (Jagged Array)

```csharp
int[][] jaggedArray = new int[3][];
jaggedArray[0] = new int[] { 1, 2, 3 };
jaggedArray[1] = new int[] { 4, 5 };
jaggedArray[2] = new int[] { 6, 7, 8, 9 };

// Обработка зубчатого массива
for (int i = 0; i < jaggedArray.Length; i++) {
    for (int j = 0; j < jaggedArray[i].Length; j++) {
        Console.Write($"{jaggedArray[i][j]} ");
    }
    Console.WriteLine();
}
```

---

## 5. Практические примеры

### Пример 1: Таблица умножения

```csharp
public void PrintMultiplicationTable(int size) {
    Console.Write("   "); // Отступ для заголовка
    
    // Заголовок
    for (int i = 1; i <= size; i++) {
        Console.Write($"{i,4}");
    }
    Console.WriteLine();
    
    // Разделительная линия
    Console.Write("   ");
    for (int i = 1; i <= size; i++) {
        Console.Write("----");
    }
    Console.WriteLine();
    
    // Таблица умножения
    for (int i = 1; i <= size; i++) {
        Console.Write($"{i,2}|"); // Номер строки
        for (int j = 1; j <= size; j++) {
            Console.Write($"{i * j,4}");
        }
        Console.WriteLine();
    }
}

// Использование
PrintMultiplicationTable(10);
```

### Пример 2: Поиск дубликатов в матрице

```csharp
public bool HasDuplicates(int[,] matrix) {
    int rows = matrix.GetLength(0);
    int cols = matrix.GetLength(1);
    
    // Сравниваем каждый элемент со всеми остальными
    for (int i = 0; i < rows; i++) {
        for (int j = 0; j < cols; j++) {
            int current = matrix[i, j];
            
            // Проверяем все элементы после текущего
            for (int k = i; k < rows; k++) {
                int startCol = (k == i) ? j + 1 : 0;
                for (int l = startCol; l < cols; l++) {
                    if (matrix[k, l] == current) {
                        return true; // Найден дубликат
                    }
                }
            }
        }
    }
    return false; // Дубликатов нет
}
```

### Пример 3: Транспонирование матрицы

```csharp
public int[,] TransposeMatrix(int[,] matrix) {
    int rows = matrix.GetLength(0);
    int cols = matrix.GetLength(1);
    int[,] transposed = new int[cols, rows];
    
    // Транспонирование: matrix[i, j] -> transposed[j, i]
    for (int i = 0; i < rows; i++) {
        for (int j = 0; j < cols; j++) {
            transposed[j, i] = matrix[i, j];
        }
    }
    
    return transposed;
}

// Использование
int[,] original = {
    { 1, 2, 3 },
    { 4, 5, 6 }
};

int[,] transposed = TransposeMatrix(original);
// Результат:
// { 1, 4 }
// { 2, 5 }
// { 3, 6 }
```

### Пример 4: Умножение матриц

```csharp
public int[,] MultiplyMatrices(int[,] matrixA, int[,] matrixB) {
    int rowsA = matrixA.GetLength(0);
    int colsA = matrixA.GetLength(1);
    int rowsB = matrixB.GetLength(0);
    int colsB = matrixB.GetLength(1);
    
    if (colsA != rowsB) {
        throw new ArgumentException("Количество столбцов A должно равняться количеству строк B");
    }
    
    int[,] result = new int[rowsA, colsB];
    
    // Умножение матриц: C[i, j] = Σ(A[i, k] * B[k, j])
    for (int i = 0; i < rowsA; i++) {
        for (int j = 0; j < colsB; j++) {
            int sum = 0;
            for (int k = 0; k < colsA; k++) {
                sum += matrixA[i, k] * matrixB[k, j];
            }
            result[i, j] = sum;
        }
    }
    
    return result;
}
```

### Пример 5: Поиск максимального элемента и его позиции

```csharp
public (int value, int row, int col) FindMaxElement(int[,] matrix) {
    int maxValue = matrix[0, 0];
    int maxRow = 0;
    int maxCol = 0;
    
    for (int i = 0; i < matrix.GetLength(0); i++) {
        for (int j = 0; j < matrix.GetLength(1); j++) {
            if (matrix[i, j] > maxValue) {
                maxValue = matrix[i, j];
                maxRow = i;
                maxCol = j;
            }
        }
    }
    
    return (maxValue, maxRow, maxCol);
}
```

---

## 6. Управление вложенными циклами

### break во вложенных циклах

```csharp
// break выходит только из внутреннего цикла
for (int i = 0; i < 3; i++) {
    for (int j = 0; j < 5; j++) {
        if (j == 2) {
            break; // Выход только из цикла j
        }
        Console.WriteLine($"i = {i}, j = {j}");
    }
}

// Вывод:
// i = 0, j = 0
// i = 0, j = 1
// i = 1, j = 0
// i = 1, j = 1
// ...
```

### Выход из всех вложенных циклов

```csharp
// Способ 1: Использование флага
bool found = false;
for (int i = 0; i < 3 && !found; i++) {
    for (int j = 0; j < 5 && !found; j++) {
        if (someCondition) {
            found = true;
            break;
        }
    }
}

// Способ 2: Использование goto (не рекомендуется, но иногда полезно)
for (int i = 0; i < 3; i++) {
    for (int j = 0; j < 5; j++) {
        if (someCondition) {
            goto ExitLoops;
        }
    }
}
ExitLoops:
// Продолжение выполнения

// Способ 3: Вынос в отдельный метод
public void SearchInMatrix() {
    for (int i = 0; i < 3; i++) {
        for (int j = 0; j < 5; j++) {
            if (someCondition) {
                return; // Выход из метода
            }
        }
    }
}
```

### continue во вложенных циклах

```csharp
// continue пропускает текущую итерацию внутреннего цикла
for (int i = 0; i < 3; i++) {
    for (int j = 0; j < 5; j++) {
        if (j == 2) {
            continue; // Пропустить j = 2, продолжить с j = 3
        }
        Console.WriteLine($"i = {i}, j = {j}");
    }
}
```

---

## 7. Производительность вложенных циклов

### Сложность алгоритмов

```csharp
// O(n²) - квадратичная сложность
for (int i = 0; i < n; i++) {
    for (int j = 0; j < n; j++) {
        // Операции
    }
}

// O(n³) - кубическая сложность
for (int i = 0; i < n; i++) {
    for (int j = 0; j < n; j++) {
        for (int k = 0; k < n; k++) {
            // Операции
        }
    }
}
```

### Оптимизация вложенных циклов

```csharp
// ❌ ПЛОХО: Неэффективный порядок циклов
int[,] matrix = new int[1000, 100];
for (int j = 0; j < 100; j++) {
    for (int i = 0; i < 1000; i++) {
        matrix[i, j] = i + j; // Медленный доступ по столбцам
    }
}

// ✅ ХОРОШО: Эффективный порядок циклов
for (int i = 0; i < 1000; i++) {
    for (int j = 0; j < 100; j++) {
        matrix[i, j] = i + j; // Быстрый доступ по строкам
    }
}
```

---

## 8. Работа с коллекциями

### Вложенные списки

```csharp
var matrix = new List<List<int>> {
    new List<int> { 1, 2, 3 },
    new List<int> { 4, 5, 6 },
    new List<int> { 7, 8, 9 }
};

// Обработка вложенных списков
for (int i = 0; i < matrix.Count; i++) {
    for (int j = 0; j < matrix[i].Count; j++) {
        Console.Write($"{matrix[i][j]} ");
    }
    Console.WriteLine();
}
```

### Словарь списков

```csharp
var data = new Dictionary<string, List<int>> {
    { "A", new List<int> { 1, 2, 3 } },
    { "B", new List<int> { 4, 5 } },
    { "C", new List<int> { 6, 7, 8, 9 } }
};

// Обработка словаря списков
foreach (var kvp in data) {
    Console.Write($"{kvp.Key}: ");
    foreach (var value in kvp.Value) {
        Console.Write($"{value} ");
    }
    Console.WriteLine();
}
```

---

## 9. Практические задачи

### Задача 1: Печать треугольника из звёздочек

```csharp
public void PrintTriangle(int height) {
    for (int i = 1; i <= height; i++) {
        // Пробелы перед звёздочками
        for (int j = 0; j < height - i; j++) {
            Console.Write(" ");
        }
        // Звёздочки
        for (int j = 0; j < 2 * i - 1; j++) {
            Console.Write("*");
        }
        Console.WriteLine();
    }
}

// Вывод для height = 5:
//     *
//    ***
//   *****
//  *******
// *********
```

### Задача 2: Поиск всех пар элементов с заданной суммой

```csharp
public List<(int, int)> FindPairsWithSum(int[] array, int targetSum) {
    var pairs = new List<(int, int)>();
    
    for (int i = 0; i < array.Length; i++) {
        for (int j = i + 1; j < array.Length; j++) {
            if (array[i] + array[j] == targetSum) {
                pairs.Add((array[i], array[j]));
            }
        }
    }
    
    return pairs;
}

// Использование
int[] numbers = { 2, 7, 11, 15, 3, 6 };
var pairs = FindPairsWithSum(numbers, 9);
// Результат: [(2, 7), (3, 6)]
```

### Задача 3: Пузырьковая сортировка

```csharp
public void BubbleSort(int[] array) {
    int n = array.Length;
    
    for (int i = 0; i < n - 1; i++) {
        for (int j = 0; j < n - i - 1; j++) {
            if (array[j] > array[j + 1]) {
                // Обмен элементов
                int temp = array[j];
                array[j] = array[j + 1];
                array[j + 1] = temp;
            }
        }
    }
}
```

---

## 10. Best Practices

### ✅ Что делать

1. **Используйте понятные имена переменных**
   ```csharp
   // ✅ ХОРОШО
   for (int row = 0; row < rows; row++) {
       for (int col = 0; col < cols; col++) {
           // ...
       }
   }
   ```

2. **Ограничивайте вложенность (максимум 3 уровня)**
   ```csharp
   // ✅ ХОРОШО: 2-3 уровня вложенности
   for (int i = 0; i < n; i++) {
       for (int j = 0; j < m; j++) {
           // Логика
       }
   }
   ```

3. **Выносите сложную логику в отдельные методы**
   ```csharp
   // ✅ ХОРОШО
   for (int i = 0; i < rows; i++) {
       for (int j = 0; j < cols; j++) {
           ProcessElement(matrix[i, j]);
       }
   }
   ```

### ❌ Чего избегать

1. **Не создавайте слишком глубокую вложенность**
   ```csharp
   // ❌ ПЛОХО: Слишком много уровней
   for (int i = 0; i < n; i++) {
       for (int j = 0; j < m; j++) {
           for (int k = 0; k < p; k++) {
               for (int l = 0; l < q; l++) {
                   // Слишком сложно!
               }
           }
       }
   }
   ```

2. **Не используйте одинаковые имена переменных**
   ```csharp
   // ❌ ПЛОХО: Путаница с переменными
   for (int i = 0; i < 3; i++) {
       for (int i = 0; i < 5; i++) { // Ошибка компиляции!
           // ...
       }
   }
   ```

---

## 11. Часто задаваемые вопросы (FAQ)

### Q: Сколько уровней вложенности можно использовать?
**A:** Технически неограниченно, но рекомендуется не более 3-4 уровней для читаемости кода.

### Q: Как выйти из всех вложенных циклов сразу?
**A:** Используйте флаг, вынесите логику в отдельный метод с return, или (редко) используйте goto.

### Q: Влияет ли порядок циклов на производительность?
**A:** Да, особенно при работе с многомерными массивами. Доступ по строкам обычно быстрее, чем по столбцам.

### Q: Можно ли использовать разные типы циклов во вложенности?
**A:** Да, можно комбинировать for, while, foreach, do-while в любых комбинациях.

---

## Заключение

Вложенные циклы полезны для:

- ✅ Обработки многомерных массивов и матриц
- ✅ Реализации алгоритмов поиска и сортировки
- ✅ Работы с вложенными структурами данных
- ✅ Решения задач, требующих перебора в нескольких измерениях

Используйте вложенные циклы осознанно, следите за производительностью и читаемостью кода!

---

*Документ создан для объяснения вложенных циклов в C# с практическими примерами и best practices.*

