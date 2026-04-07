using CleverenceTest.Tools;
using System.Net.Http.Headers;
using CleverenceTest.Task1.Compressors;

namespace CleverenceTest.Task1
{
    internal class Program
    {
        private static ValidationResult ValidateText(String? text, out ValidationResult validationResult)
        {
            if (text is null) validationResult = ValidationResult.Error("Текст был null");
            else if (text.IsWhiteSpace()) validationResult = ValidationResult.Error("Вы не ввели текст");
            else if (text.Any(Char.IsDigit)) validationResult = ValidationResult.Error("Не используйте цифры");
            else validationResult = ValidationResult.Success();
            
            return validationResult;
        }

        static void Main(string[] args)
        {
            Console.Write("Укажите строку, которую хотите сжать: ");
            String? textToBeCompressed = Console.ReadLine();
            
            while (!ValidateText(textToBeCompressed, out ValidationResult result).IsSuccess) //цикл повторяющийся пока пользователь не введет норм. текст
            {
                Console.WriteLine($"{result.ErrorText}, попробуйте еще раз");
                Console.WriteLine();
                Console.Write("Укажите строку, которую хотите сжать: ");
                textToBeCompressed = Console.ReadLine();
            }

            ICompressor compressor = new RLECompressor(); //сжиматель текста
            
            Console.WriteLine();
            Console.Write("Результат: ");

            Console.WriteLine(compressor.CompressString(textToBeCompressed!));
            Console.WriteLine();

            Console.WriteLine("Работа завершена, любая клавиша закроет окно...");
            Console.ReadKey();
        }
    }
}
