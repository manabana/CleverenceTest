using System.Globalization;
using System.Text.RegularExpressions;

namespace CleverenceTest.Task3
{
    internal class Parser
    {
        // Регулярные выражения для двух форматов
        // Формат 1: 10.03.2025 15:14:49.523 INFORMATION Сообщение
        private static readonly Regex RegexFormat1 = new(@"^(\d{2}\.\d{2}\.\d{4})\s+(\d{2}:\d{2}:\d{2}\.\d+)\s+(\w+)\s+(.*)$", RegexOptions.Compiled);

        // Формат 2: 2025-03-10 15:14:51.5882| INFO|11|Method| Сообщение
        private static readonly Regex RegexFormat2 = new(@"^(\d{4}-\d{2}-\d{2})\s+(\d{2}:\d{2}:\d{2}\.\d+)\|\s*(\w+)\s*\|\d+\|([^|]+)\|\s*(.*)$", RegexOptions.Compiled);

        private static bool TryParseLine(string line, out LogEntry entry)
        {
            entry = default;

            // Формат 1
            var match1 = RegexFormat1.Match(line);
            if (match1.Success)
            {
                entry = new LogEntry
                {
                    Date = ConvertDate(match1.Groups[1].Value, "dd.MM.yyyy"),
                    Time = match1.Groups[2].Value,
                    Level = MapLogLevel(match1.Groups[3].Value),
                    Method = "DEFAULT",
                    Message = match1.Groups[4].Value.Trim()
                };
                return true;
            }

            // Формат 2
            var match2 = RegexFormat2.Match(line);
            if (match2.Success)
            {
                entry = new LogEntry
                {
                    Date = ConvertDate(match2.Groups[1].Value, "yyyy-MM-dd"),
                    Time = match2.Groups[2].Value,
                    Level = MapLogLevel(match2.Groups[3].Value),
                    Method = match2.Groups[4].Value.Trim(),
                    Message = match2.Groups[5].Value.Trim()
                };
                return true;
            }

            return false;
        }

        private static string ConvertDate(string dateStr, string inputFormat)
        {
            // Приводим дату к формату DD-MM-YYYY согласно ТЗ
            if (DateTime.TryParseExact(dateStr, inputFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                return dt.ToString("dd-MM-yyyy");
            }
            return dateStr; // Если не распарсилось, возвращаем как есть
        }

        private static string MapLogLevel(string level)
        {
            return level.ToUpper() switch
            {
                "INFORMATION" => "INFO",
                "WARNING" => "WARN",
                "INFO" => "INFO",
                "WARN" => "WARN",
                "ERROR" => "ERROR",
                "DEBUG" => "DEBUG",
                _ => level
            };
        }

        /// <summary>
        /// Читает и парсит в реальном времени, чтобы не грузить память, можно использовать когда очень много строк
        /// </summary>
        public async Task ParseAndWriteRealTime(FileProvider fileProvider)
        {
            String? line;
            int processedCount = 0;
            int problemsCount = 0;
            try
            {
                while ((line = await fileProvider.StreamReader.ReadLineAsync()) != null)
                {
                    if (String.IsNullOrWhiteSpace(line)) continue;

                    if (TryParseLine(line, out var entry))
                    {
                        String outputLine = $"{entry.Date}\t{entry.Time}\t{entry.Level}\t{entry.Method}\t{entry.Message}";
                        await fileProvider.StreamWriter.WriteLineAsync(outputLine);
                        processedCount++;
                    }
                    else
                    {
                        await fileProvider.ProblemStreamWriter.WriteLineAsync(line);
                        problemsCount++;
                    }
                }

                Console.WriteLine("Обработка завершена.");
                Console.WriteLine($"- Успешно: {processedCount}");
                Console.WriteLine($"- Проблемных записей: {problemsCount}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении/парсинге в реальном времени: {ex.Message}");
            }
        }

        /// <summary>
        /// Пытается спарсить входящие строки, неудавшиеся вернутся в виде problemLines
        /// </summary>
        /// <param name="inputFileLines"></param>
        /// <returns>Возвращает строки с output и problems значениями (порядок: 1.outputLines, 2.problemLines)</returns>
        public async Task<(Dictionary<Int32, String>, Dictionary<Int32, String>)> ParseLogsAsync(Dictionary<Int32, String> inputFileLines)
        {
            int processedCount = 0;
            int problemsCount = 0;

            //создал словари, чтобы закрепить порядок строк
            Dictionary<Int32, String> parsedLines = new();
            Dictionary<Int32, String> problemLines = new();

            List<String> sortedInputFileLines =
                inputFileLines
                    .OrderBy(line => line.Key)
                    .Select(line => line.Value)
                    .ToList();

            foreach (var line in sortedInputFileLines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                if (TryParseLine(line, out var entry))
                {
                    String outputLine = $"{entry.Date}\t{entry.Time}\t{entry.Level}\t{entry.Method}\t{entry.Message}";
                    parsedLines.Add(processedCount, outputLine);
                    processedCount++;
                }
                else
                {
                    problemLines.Add(problemsCount, line);
                    problemsCount++;
                }
            }

            Console.WriteLine("Обработка завершена.");
            Console.WriteLine($"- Успешно: {processedCount}");
            Console.WriteLine($"- Проблемных записей: {problemsCount}");

            return (parsedLines, problemLines);
        }

    }
}
