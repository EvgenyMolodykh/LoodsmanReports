using System;

namespace LoodsmanBackend
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Loodsman backend test (.NET Framework 4.8)");

            Console.Write("Введите хост (по умолчанию ascon): ");
            var host = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(host))
                host = "ascon";

            int port = 8076; // подставь свой порт сервера приложений

            try
            {
                using (var api = new LoodsmanApi(host, port))
                {
                    // 1. Получаем список баз
                    var dbList = api.GetDbList();
                    Console.WriteLine("Список баз данных:");
                    foreach (var db in dbList)
                        Console.WriteLine($" - {db}");

                    // 2. Выбираем базу
                    Console.WriteLine();
                    Console.Write("Введите имя базы для подключения: ");
                    var dbName = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(dbName))
                    {
                        Console.WriteLine("База не задана, выходим.");
                        return;
                    }

                    // 3. Подключаемся к выбранной базе
                    api.ConnectToDb(dbName);
                    Console.WriteLine($"Подключение к базе {dbName} успешно.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка:");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine();
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }
    }
}
