namespace CleverenceTest.Task2.Interactors
{
    internal class Reader : Interactor //тот кто будет читать с сервера
    {
        public Reader(Int32 id)
        {
            Id = id;
        }

        private void SayToConsole(String text)
        {
            var date = DateTime.Now;
            Console.WriteLine($"\x1b[33m[Читатель {Id}]\x1b[0m [{date.Second}:{date.Millisecond}] {text}");
        }

        public void ReadFromServer()
        {
            SayToConsole("Хочу ПРОЧИТАТЬ значение с сервера");
            Int32 value = Server.GetCount(Id);
            SayToConsole($"Прочитанное значение {value}");
        }
    }
}
