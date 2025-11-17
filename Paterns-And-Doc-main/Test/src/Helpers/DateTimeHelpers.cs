using System;
using System.Collections.Generic;
using System.Linq;

namespace Test.src.Helpers
{
    /// <summary>
    /// Утилитарный класс для работы с датами и временем (DateTime).
    /// Содержит полезные методы для решения различных задач с датами.
    /// </summary>
    public static class DateTimeHelpers
    {
        /// <summary>
        /// Проверяет, является ли год високосным.
        /// Год является високосным, если он делится на 4, но не на 100, или делится на 400.
        /// </summary>
        /// <param name="year">Год для проверки.</param>
        /// <returns>True, если год високосный.</returns>
        public static bool IsLeapYear(int year)
        {
            return DateTime.IsLeapYear(year);
        }

        /// <summary>
        /// Получает первый день месяца для указанной даты.
        /// </summary>
        /// <param name="date">Дата для обработки.</param>
        /// <returns>DateTime, представляющий первый день месяца.</returns>
        public static DateTime FirstDayOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// Получает последний день месяца для указанной даты.
        /// </summary>
        /// <param name="date">Дата для обработки.</param>
        /// <returns>DateTime, представляющий последний день месяца.</returns>
        public static DateTime LastDayOfMonth(DateTime date)
        {
            int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            return new DateTime(date.Year, date.Month, daysInMonth);
        }

        /// <summary>
        /// Получает первый день недели для указанной даты (понедельник).
        /// </summary>
        /// <param name="date">Дата для обработки.</param>
        /// <returns>DateTime, представляющий понедельник этой недели.</returns>
        public static DateTime FirstDayOfWeek(DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-diff).Date;
        }

        /// <summary>
        /// Получает последний день недели для указанной даты (воскресенье).
        /// </summary>
        /// <param name="date">Дата для обработки.</param>
        /// <returns>DateTime, представляющий воскресенье этой недели.</returns>
        public static DateTime LastDayOfWeek(DateTime date)
        {
            int diff = (7 - (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(diff).Date;
        }

        /// <summary>
        /// Вычисляет возраст по дате рождения.
        /// </summary>
        /// <param name="birthDate">Дата рождения.</param>
        /// <param name="referenceDate">Дата для вычисления возраста (по умолчанию сегодня).</param>
        /// <returns>Возраст в годах.</returns>
        public static int CalculateAge(DateTime birthDate, DateTime? referenceDate = null)
        {
            DateTime refDate = referenceDate ?? DateTime.Now;

            if (birthDate > refDate)
                throw new ArgumentException("Дата рождения не может быть больше текущей даты.", nameof(birthDate));

            int age = refDate.Year - birthDate.Year;

            // Уменьшаем возраст на 1, если день рождения ещё не наступил в этом году
            if (refDate.Month < birthDate.Month || 
                (refDate.Month == birthDate.Month && refDate.Day < birthDate.Day))
            {
                age--;
            }

            return age;
        }

        /// <summary>
        /// Форматирует временной интервал в удобочитаемый вид (например, "2 часа 30 минут").
        /// </summary>
        /// <param name="timeSpan">Временной интервал для форматирования.</param>
        /// <returns>Строка с описанием интервала.</returns>
        public static string FormatTimeSpan(TimeSpan timeSpan)
        {
            List<string> parts = new List<string>();

            if (timeSpan.Days > 0)
                parts.Add($"{timeSpan.Days} {(timeSpan.Days == 1 ? "день" : "дней")}");

            if (timeSpan.Hours > 0)
                parts.Add($"{timeSpan.Hours} {(timeSpan.Hours == 1 ? "час" : "часов")}");

            if (timeSpan.Minutes > 0)
                parts.Add($"{timeSpan.Minutes} {(timeSpan.Minutes == 1 ? "минута" : "минут")}");

            if (timeSpan.Seconds > 0 && timeSpan.TotalHours < 1)
                parts.Add($"{timeSpan.Seconds} {(timeSpan.Seconds == 1 ? "секунда" : "секунд")}");

            return parts.Count > 0 ? string.Join(" ", parts) : "0 секунд";
        }

        /// <summary>
        /// Проверяет, попадает ли дата в указанный диапазон (включительно).
        /// </summary>
        /// <param name="date">Дата для проверки.</param>
        /// <param name="startDate">Начало диапазона.</param>
        /// <param name="endDate">Конец диапазона.</param>
        /// <returns>True, если дата попадает в диапазон.</returns>
        public static bool IsInRange(DateTime date, DateTime startDate, DateTime endDate)
        {
            return date >= startDate && date <= endDate;
        }

        /// <summary>
        /// Получает все даты между двумя датами (включительно).
        /// </summary>
        /// <param name="startDate">Начальная дата.</param>
        /// <param name="endDate">Конечная дата.</param>
        /// <returns>Список всех дат в диапазоне.</returns>
        public static List<DateTime> GetDateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                throw new ArgumentException("Начальная дата не может быть больше конечной.", nameof(startDate));

            List<DateTime> dates = new List<DateTime>();
            DateTime current = startDate.Date;

            while (current <= endDate.Date)
            {
                dates.Add(current);
                current = current.AddDays(1);
            }

            return dates;
        }

        /// <summary>
        /// Получает все рабочие дни (понедельник-пятница) между двумя датами.
        /// </summary>
        /// <param name="startDate">Начальная дата.</param>
        /// <param name="endDate">Конечная дата.</param>
        /// <returns>Список рабочих дней в диапазоне.</returns>
        public static List<DateTime> GetWorkDays(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                throw new ArgumentException("Начальная дата не может быть больше конечной.", nameof(startDate));

            List<DateTime> workDays = new List<DateTime>();
            DateTime current = startDate.Date;

            while (current <= endDate.Date)
            {
                if (current.DayOfWeek != DayOfWeek.Saturday && current.DayOfWeek != DayOfWeek.Sunday)
                {
                    workDays.Add(current);
                }
                current = current.AddDays(1);
            }

            return workDays;
        }

        /// <summary>
        /// Получает количество рабочих дней между двумя датами.
        /// </summary>
        /// <param name="startDate">Начальная дата.</param>
        /// <param name="endDate">Конечная дата.</param>
        /// <returns>Количество рабочих дней.</returns>
        public static int CountWorkDays(DateTime startDate, DateTime endDate)
        {
            return GetWorkDays(startDate, endDate).Count;
        }

        /// <summary>
        /// Вычисляет количество дней между двумя датами.
        /// </summary>
        /// <param name="startDate">Начальная дата.</param>
        /// <param name="endDate">Конечная дата.</param>
        /// <returns>Количество дней между датами.</returns>
        public static int DaysBetween(DateTime startDate, DateTime endDate)
        {
            return Math.Abs((endDate.Date - startDate.Date).Days);
        }

        /// <summary>
        /// Получает название дня недели на русском языке.
        /// </summary>
        /// <param name="date">Дата для получения дня недели.</param>
        /// <returns>Название дня недели.</returns>
        public static string GetDayOfWeekName(DateTime date)
        {
            return date.DayOfWeek switch
            {
                DayOfWeek.Monday => "Понедельник",
                DayOfWeek.Tuesday => "Вторник",
                DayOfWeek.Wednesday => "Среда",
                DayOfWeek.Thursday => "Четверг",
                DayOfWeek.Friday => "Пятница",
                DayOfWeek.Saturday => "Суббота",
                DayOfWeek.Sunday => "Воскресенье",
                _ => date.DayOfWeek.ToString()
            };
        }

        /// <summary>
        /// Получает название месяца на русском языке.
        /// </summary>
        /// <param name="date">Дата для получения месяца.</param>
        /// <returns>Название месяца.</returns>
        public static string GetMonthName(DateTime date)
        {
            return date.Month switch
            {
                1 => "Январь",
                2 => "Февраль",
                3 => "Март",
                4 => "Апрель",
                5 => "Май",
                6 => "Июнь",
                7 => "Июль",
                8 => "Август",
                9 => "Сентябрь",
                10 => "Октябрь",
                11 => "Ноябрь",
                12 => "Декабрь",
                _ => date.ToString("MMMM")
            };
        }

        /// <summary>
        /// Получает количество дней в месяце для указанной даты.
        /// </summary>
        /// <param name="date">Дата для обработки.</param>
        /// <returns>Количество дней в месяце.</returns>
        public static int DaysInMonth(DateTime date)
        {
            return DateTime.DaysInMonth(date.Year, date.Month);
        }

        /// <summary>
        /// Демонстрирует работу всех методов класса DateTimeHelpers.
        /// </summary>
        public static void Demonstrate()
        {
            Console.WriteLine("=== Демонстрация методов DateTimeHelpers ===\n");

            DateTime now = DateTime.Now;
            DateTime birthDate = new DateTime(1990, 5, 15);

            // IsLeapYear
            Console.WriteLine($"IsLeapYear(2024): {IsLeapYear(2024)}");
            Console.WriteLine($"IsLeapYear(2023): {IsLeapYear(2023)}");

            // FirstDayOfMonth/LastDayOfMonth
            Console.WriteLine($"FirstDayOfMonth: {FirstDayOfMonth(now):yyyy-MM-dd}");
            Console.WriteLine($"LastDayOfMonth: {LastDayOfMonth(now):yyyy-MM-dd}");

            // FirstDayOfWeek/LastDayOfWeek
            Console.WriteLine($"FirstDayOfWeek: {FirstDayOfWeek(now):yyyy-MM-dd}");
            Console.WriteLine($"LastDayOfWeek: {LastDayOfWeek(now):yyyy-MM-dd}");

            // CalculateAge
            Console.WriteLine($"CalculateAge (born {birthDate:yyyy-MM-dd}): {CalculateAge(birthDate)} лет");

            // FormatTimeSpan
            TimeSpan interval = new TimeSpan(2, 3, 30, 45);
            Console.WriteLine($"FormatTimeSpan: {FormatTimeSpan(interval)}");

            // IsInRange
            Console.WriteLine($"IsInRange: {IsInRange(now, now.AddDays(-1), now.AddDays(1))}");

            // GetDateRange
            var dateRange = GetDateRange(now.AddDays(-3), now);
            Console.WriteLine($"GetDateRange (3 дня): {dateRange.Count} дней");

            // GetWorkDays
            var workDays = GetWorkDays(now.AddDays(-7), now);
            Console.WriteLine($"GetWorkDays (7 дней назад): {workDays.Count} рабочих дней");

            // DaysBetween
            Console.WriteLine($"DaysBetween: {DaysBetween(birthDate, now)} дней");

            // GetDayOfWeekName/GetMonthName
            Console.WriteLine($"GetDayOfWeekName: {GetDayOfWeekName(now)}");
            Console.WriteLine($"GetMonthName: {GetMonthName(now)}");

            // DaysInMonth
            Console.WriteLine($"DaysInMonth: {DaysInMonth(now)} дней");
        }
    }
}
