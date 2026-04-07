namespace CleverenceTest.Task3
{
    class Program
    {
        static async Task Main(string[] args)
        {

            Console.WriteLine("Начало обработки логов...");

            using FileProvider fileProvider = new();
            Parser parser = new();

            if (!fileProvider.CheckInputFile().IsSuccess)
            {
                Console.WriteLine("файл input.log не найден");
                Console.ReadKey();
                return;
            }

            //await ParseAndWriteRealTime(fileProvider, parser);
            
            await ParseAndWriteClassic(fileProvider, parser);
        }

        /// <summary>
        /// Сперва выгружает строки из файла input, затем парсит и загружает в файлы output и problems, надежней чем ParseAndWriteRealTime, т.к. загружает строки в output/problems только при полном успехе выгрузки из input
        /// </summary>
        static async Task ParseAndWriteClassic(FileProvider fileProvider, Parser parser)
        {
            Dictionary<int, string> logLines = await fileProvider.GetLogLinesAsync();
            (Dictionary<int, string> outputLines, Dictionary<int, string> problemLines) parseResult = await parser.ParseLogsAsync(logLines);

            await fileProvider.SaveLinesToFiles(parseResult.outputLines, parseResult.problemLines);
        }

        /// <summary>
        /// Читает и парсит в реальном времени, чтобы не грузить память, можно использовать когда очень много строк
        /// </summary>
        static async Task ParseAndWriteRealTime(FileProvider fileProvider, Parser parser)
        {
            await parser.ParseAndWriteRealTime(fileProvider);
        }
    }
}
