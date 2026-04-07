using CleverenceTest.Task2.Interactors;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleverenceTest.Task2
{
    internal class Program
    {
        static void Main()
        {
            List<Reader> readers = [.. Enumerable.Range(1, 10).Select(i => new Reader(i))]; //Создаем 10 читателей
            List<Writer> writers = [.. Enumerable.Range(1, 2).Select(i => new Writer(i))]; // и 2 писателя

            Task[] tasks = new Task[30];
            Random rnd = new Random();
            Boolean areWriterExecuted = false;

            for (Int32 i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    if (rnd.Next(0, 5) == 0) // иногда пишем
                    {
                        Writer writer = writers[rnd.Next(writers.Count)];
                        writer.WriteToServer();
                        areWriterExecuted = true;
                    }
                    else //а иногда читаем
                    {
                        Reader reader = readers[rnd.Next(readers.Count)];
                        reader.ReadFromServer();
                    }

                    if(i == tasks.Length - 7 && !areWriterExecuted) // в случае если рандом не сработает, запустим хотя бы 1 писателя принудительно
                    {
                        Writer writer = writers[rnd.Next(writers.Count)];
                        writer.WriteToServer();
                        areWriterExecuted = true;
                    }
                });
            }

            Task.WaitAll(tasks);

            Console.WriteLine("Работа завершена, любая клавиша закроет окно...");

            Console.ReadKey();
        }
    }
}
