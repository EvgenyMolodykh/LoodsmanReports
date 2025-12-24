using System;
using Ascon.Plm.ServerApi;
using Ascon.Plm.AppServer.Contracts;

class Program
{
    static void Main()
    {
        var factory = new ConnectionFactory();

        // твой хост и порт
        IConnection connection = factory.CreateConnection("ascon", 8076);

        IMainSystem main = connection.MainSystem;

        object code = 0;
        object message = "";

        // Получить список БД
        main.GetDBList(ref code, ref message);
        Console.WriteLine($"GetDBList: code = {code}, message = {message}");

        // Ввести имя БД
        Console.Write("Введите имя БД Лоцмана: ");
        string dbName = Console.ReadLine();

        object connCode;
        object connMsg;

        // Подключиться к выбранной БД
        main.ConnectToDB(dbName, out connCode, out connMsg);
        Console.WriteLine($"ConnectToDB: code = {connCode}, message = {connMsg}");

        Console.ReadLine();
    }
}
