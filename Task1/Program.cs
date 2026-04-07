using Task1.Compressors;

namespace Task1
{
    String? ValidateText(String text)
    {
        if (text.IsWhiteSpace()) return "Вы не ввели текст";
        if (text.Any(Char.IsDigit)) return "Строка не может содержать цифры";
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Укажите строку, которую хотите сжать: ");
            String? textToBeCompressed = Console.ReadLine();
            
            while (textToBeCompressed.IsWhiteSpace())
            {
                Console.WriteLine("Вы не ввели текст, попробуйте еще раз:");
                textToBeCompressed = Console.ReadLine();
            }

            ICompressor compressor = new RLECompressor(); //сжиматель текста
            
            Console.WriteLine();
            Console.WriteLine("Результат: ");
            

        }
    }
}
