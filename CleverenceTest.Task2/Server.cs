namespace CleverenceTest.Task2
{
    using System.Threading;

    internal static class Server
    {
        private static readonly String _serverSays = "Сервер говорит:";

        private static Int32 _count = 0;

        private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        private static void SayToConsole(String text)
        {
            var date = DateTime.Now;

            Console.WriteLine($"\x1b[34m[Сервер]\x1b[0m [{date.Second}:{date.Millisecond}] {text}");
        }

        public static Int32 GetCount(Int32 readerId)
        {
            _lock.EnterReadLock();
            try
            {
                SayToConsole($"Читатель {readerId} читает");
                Thread.Sleep(500); //симулируем долгую работу
                return _count;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public static void AddToCount(Int32 writerId, Int32 value)
        {
            _lock.EnterWriteLock();
            try
            {
                SayToConsole($"Писатель {writerId} записывает");
                Thread.Sleep(1500); //симулируем долгую работу
                _count += value;
                SayToConsole($"Писатель {writerId} закончил");
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}
