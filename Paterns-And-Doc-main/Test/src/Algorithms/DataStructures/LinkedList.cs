using System;
using System.Text;

namespace Test.src.Algorithms.DataStructures
{
    /// <summary>
    /// Представляет узел односвязного списка.
    /// Каждый узел содержит данные и ссылку на следующий узел в последовательности.
    /// </summary>
    /// <typeparam name="T">Тип данных, хранящихся в узле.</typeparam>
    public class ListNode<T>
    {
        /// <summary>
        /// Получает или задаёт значение данных, хранящихся в этом узле.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Получает или задаёт ссылку на следующий узел в списке.
        /// null если это последний узел в списке.
        /// </summary>
        public ListNode<T>? Next { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса ListNode с заданными данными.
        /// </summary>
        /// <param name="data">Данные для хранения в этом узле.</param>
        public ListNode(T data)
        {
            Data = data;
            Next = null; // Изначально этот узел не ссылается ни на какой другой узел
        }
    }

    /// <summary>
    /// Реализует структуру данных односвязный список.
    /// 
    /// <para>
    /// Связный список — это линейная структура данных, где элементы хранятся в узлах,
    /// и каждый узел указывает на следующий узел через ссылку (указатель).
    /// В отличие от массивов, связанные списки не хранят элементы в смежных областях памяти.
    /// </para>
    /// 
    /// <para>
    /// Временная сложность:
    /// - Доступ по индексу: O(n) — необходимо пройти от головы
    /// - Вставка в голову: O(1) — за постоянное время
    /// - Вставка в хвост: O(n) — требуется пройти к концу (O(1), если есть указатель на хвост)
    /// - Удаление: O(n) — сначала нужно найти элемент
    /// - Поиск: O(n) — приходится проходить список
    /// </para>
    /// 
    /// <para>
    /// Пространственная сложность: O(n) — хранит n элементов плюс n указателей.
    /// </para>
    /// 
    /// <para>
    /// Преимущества: динамический размер, эффективная вставка/удаление в голову.
    /// Недостатки: нет случайного доступа, лишняя память под указатели, хуже кэш-производительность.
    /// </para>
    /// </summary>
    /// <typeparam name="T">Тип элементов в связанном списке.</typeparam>
    public class LinkedList<T>
    {
        /// <summary>
        /// Получает или задаёт голову (первый узел) связного списка.
        /// null если список пуст.
        /// </summary>
        private ListNode<T>? Head { get; set; }

        /// <summary>
        /// Получает количество элементов в связанном списке.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса LinkedList (пустой список).
        /// </summary>
        public LinkedList()
        {
            Head = null;
            Count = 0;
        }

        /// <summary>
        /// Добавляет новый узел с указанным значением в начало списка (голову).
        /// Временная сложность: O(1) — операция за постоянное время.
        /// </summary>
        /// <param name="value">Значение для добавления в список.</param>
        public void AddFirst(T value)
        {
            // Создаем новый узел с заданным значением
            ListNode<T> newNode = new ListNode<T>(value);

            // Новый узел указывает на текущую голову
            // Если список пуст, Head равен null, значит newNode.Next тоже будет null (что верно для последнего узла)
            newNode.Next = Head;

            // Новый узел становится новой головой
            Head = newNode;

            // Увеличиваем счетчик размера списка
            Count++;
        }

        /// <summary>
        /// Добавляет новый узел с указанным значением в конец списка (хвост).
        /// Временная сложность: O(n) — приходится проходить до конца списка.
        /// </summary>
        /// <param name="value">Значение для добавления в список.</param>
        public void AddLast(T value)
        {
            // Создаем новый узел с заданным значением
            ListNode<T> newNode = new ListNode<T>(value);

            // Если список пуст, новый узел становится головой
            if (Head == null)
            {
                Head = newNode;
            }
            else
            {
                // Проходим до последнего узла в списке
                // Начинаем с головы и идем по указателям Next, пока не дойдем до null
                ListNode<T> current = Head;
                
                // Пока не найдем узел, чей Next равен null (последний узел)
                while (current.Next != null)
                {
                    current = current.Next;
                }

                // Теперь current указывает на последний узел
                // Делаем его Next указывать на наш новый узел
                current.Next = newNode;
            }

            // Увеличиваем счетчик
            Count++;
        }

        /// <summary>
        /// Удаляет первое вхождение указанного значения из списка.
        /// Временная сложность: O(n) — сначала ищется значение.
        /// </summary>
        /// <param name="value">Значение для удаления из списка.</param>
        /// <returns>True если значение найдено и удалено; иначе false.</returns>
        public bool Remove(T value)
        {
            // Если список пуст — удалять нечего
            if (Head == null)
            {
                return false;
            }

            // Особый случай: если в голове лежит нужное значение — удаляем голову
            if (Head.Data != null && Head.Data.Equals(value))
            {
                // Перемещаем голову на следующий узел (или null если в списке был один узел)
                Head = Head.Next;
                Count--;
                return true;
            }

            // Общий случай: ищем нужное значение
            // Нужно запоминать предыдущий узел, чтобы обновить его Next
            ListNode<T>? current = Head;

            // Проходим список, пока текущий узел не null
            // Проверяем — не содержит ли следующий узел нужное значение
            while (current != null && current.Next != null)
            {
                // Если следующий узел содержит нужное нам значение
                if (current.Next.Data != null && current.Next.Data.Equals(value))
                {
                    // Минусуем следующий узел, делая current.Next ссылкой на следующий за ним
                    // Так фактически current.Next удаляется из списка
                    current.Next = current.Next.Next;
                    Count--;
                    return true;
                }

                // Переходим к следующему узлу
                current = current.Next;
            }

            // Значение не найдено в списке
            return false;
        }

        /// <summary>
        /// Проверяет, содержится ли в списке указанное значение.
        /// Временная сложность: O(n) — возможно потребуется пройти весь список.
        /// </summary>
        /// <param name="value">Значение для поиска в списке.</param>
        /// <returns>True если значение найдено; иначе false.</returns>
        public bool Contains(T value)
        {
            // Начинаем с головы и проходим по всем узлам
            ListNode<T>? current = Head;

            // Пока не прошли все узлы (current станет null)
            while (current != null)
            {
                // Сравниваем данные текущего узла с искомым значением
                if (current.Data != null && current.Data.Equals(value))
                {
                    return true; // Нашли!
                }

                // Переходим к следующему узлу
                current = current.Next;
            }

            // Дошли до конца списка без нахождения значения
            return false;
        }

        /// <summary>
        /// Возвращает значение по указанному индексу (нумерация с нуля).
        /// Временная сложность: O(n) — нужно пройти от головы до индекса.
        /// </summary>
        /// <param name="index">Нулевой индекс элемента для возврата.</param>
        /// <returns>Значение по указанному индексу.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Выбрасывается, если индекс вне допустимого диапазона.</exception>
        public T GetAt(int index)
        {
            // Проверяем корректность индекса
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), 
                    $"Индекс должен быть между 0 и {Count - 1}.");
            }

            // Проходим до узла с нужным индексом
            ListNode<T>? current = Head;
            
            // Делаем index шагов вперёд от головы
            for (int i = 0; i < index; i++)
            {
                // Здесь current не может быть null, т.к. мы проверили index < Count
                current = current!.Next;
            }

            // Возвращаем данные из найденного узла
            return current!.Data;
        }

        /// <summary>
        /// Возвращает строковое представление связного списка.
        /// Формат: "значение1 -> значение2 -> значение3 -> null"
        /// </summary>
        /// <returns>Строковое представление списка.</returns>
        public override string ToString()
        {
            // Используем StringBuilder для эффективного склеивания строк
            StringBuilder result = new StringBuilder();

            // Проходим по списку от головы до хвоста
            ListNode<T>? current = Head;

            while (current != null)
            {
                // Добавляем данные текущего узла
                result.Append(current.Data?.ToString() ?? "null");

                // Если есть следующий узел, добавляем стрелку-разделитель
                if (current.Next != null)
                {
                    result.Append(" -> ");
                }

                // Переходим к следующему узлу
                current = current.Next;
            }

            // Если список пуст — отображаем соответствующее сообщение
            if (result.Length == 0)
            {
                result.Append("(empty list)");
            }

            return result.ToString();
        }

        /// <summary>
        /// Демонстрирует структуру данных LinkedList через разные операции.
        /// </summary>
        public static void Demonstrate()
        {
            Console.WriteLine("=== Демонстрация связного списка ===\n");

            // Создаем новый связный список
            LinkedList<int> list = new LinkedList<int>();

            // Добавляем элементы в список
            Console.WriteLine("Добавляем элементы в список:");
            list.AddFirst(10);
            list.AddFirst(20);
            list.AddLast(30);
            list.AddLast(40);
            Console.WriteLine("Список: {0}", list);
            Console.WriteLine("Количество: {0}\n", list.Count);

            // Проверяем, есть ли в списке значение
            Console.WriteLine("Содержит 30? {0}", list.Contains(30));
            Console.WriteLine("Содержит 50? {0}\n", list.Contains(50));

            // Получаем значение по индексу
            Console.WriteLine("Значение по индексу 1: {0}", list.GetAt(1));
            Console.WriteLine("Значение по индексу 3: {0}\n", list.GetAt(3));

            // Удаляем элемент
            Console.WriteLine("Удаляем 30:");
            list.Remove(30);
            Console.WriteLine("Список: {0}", list);
            Console.WriteLine("Количество: {0}\n", list.Count);

            // Удаляем первый элемент
            Console.WriteLine("Удаляем 20 (голову):");
            list.Remove(20);
            Console.WriteLine("Список: {0}", list);
            Console.WriteLine("Количество: {0}", list.Count);
        }
    }
}
