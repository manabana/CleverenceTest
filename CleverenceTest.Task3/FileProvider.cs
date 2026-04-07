using CleverenceTest.Tools;

namespace CleverenceTest.Task3
{
    internal class FileProvider : IDisposable
    {
        // Пути к файлам
        private const string InputPath = "input.log";
        private const string OutputPath = "output.log";
        private const string ProblemsPath = "problems.txt";

        private StreamReader? _streamReader;
        private StreamWriter? _streamWriter;
        private StreamWriter? _problemStreamWriter;

        public ValidationResult CheckInputFile()
        {
            if (!File.Exists(InputPath)) return ValidationResult.Error($"Входной файл {InputPath} не найден");

            return ValidationResult.Success();
        }

        public StreamReader StreamReader
        {
            get
            {
                _streamReader ??= new StreamReader(InputPath);
                return _streamReader;
            }
        }

        public StreamWriter StreamWriter
        {
            get
            {
                _streamWriter ??= new StreamWriter(OutputPath);
                return _streamWriter;
            }
        }

        public StreamWriter ProblemStreamWriter
        {
            get
            {
                _problemStreamWriter ??= new StreamWriter(ProblemsPath);
                return _problemStreamWriter;
            }
        }

        /// <summary>
        /// Читает логи из input файла
        /// </summary>
        /// <returns>Возвращает словарь со строками и порядковыми номерами</returns>
        public async Task<Dictionary<Int32, String>> GetLogLinesAsync() //создал словари, чтобы закрепить порядок строк
        {
            string? line;

            var logLines = new Dictionary<Int32, String>();
            int lineNumber = 0;

            try
            {
                while ((line = await StreamReader.ReadLineAsync()) != null)
                {
                    logLines.Add(lineNumber, line);

                    lineNumber++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении строк: {ex.Message}");
                return [];
            }

            return logLines;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputLines">Строки, которые будут записаны в output файл</param>
        /// <param name="problemFiles">Строки, которые будут записаны в problems файл</param>
        /// <returns></returns>
        public async Task SaveLinesToFiles(Dictionary<Int32, String> outputLines, Dictionary<Int32, String> problemFiles)
        {
            List<String> sortedProblemFileLines =
                problemFiles
                    .OrderBy(line => line.Key)
                    .Select(line => line.Value)
                    .ToList();

            List<String> sortedOutputFileLines =
                outputLines
                    .OrderBy(line => line.Key)
                    .Select(line => line.Value)
                    .ToList();
            try
            {
                foreach (var line in sortedOutputFileLines)
                {
                    await StreamWriter.WriteLineAsync(line);
                }

                foreach (var line in sortedProblemFileLines)
                {
                    await ProblemStreamWriter.WriteLineAsync(line);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении: {ex.Message}");
            }

            Console.WriteLine("Строки сохранены");
        }

        public void Dispose()
        {
            _streamReader?.Dispose();
            _streamWriter?.Dispose();
            _problemStreamWriter?.Dispose();
        }
    }
}
