namespace CleverenceTest.Task2.Interactors
{
    internal class Writer : Interactor // тот кто будет записывать на сервер
    {
        public Writer(Int32 id)
        {
            Id = id;
        }

        private void SayToConsole(String text)
        {
            var date = DateTime.Now;
            Console.WriteLine($"\x1b[32m[Писатель {Id}]\x1b[0m [{date.Second}:{date.Millisecond}] {text}");
        }

        public void WriteToServer()
        {
            Int32 addValue = Id + 1;
            SayToConsole($"Хочу ДОБАВИТЬ к count на сервере {addValue}");
            Server.AddToCount(Id, addValue);
        }
    }
}
