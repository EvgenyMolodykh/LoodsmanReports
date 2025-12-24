using System;
using Ascon.Plm.ServerApi;
using Ascon.Plm.AppServer.Contracts;

namespace Loodsman.Core
{
   
    public class LoodsmanApi : IDisposable
    {
        private readonly string _host;
        private readonly int _port;

        private IConnection _connection;
        private IMainSystem _main;

        public LoodsmanApi(string host, int port)
        {
            _host = host;
            _port = port;
        }

        /// <summary>
        /// Подключение к серверу приложений.
        /// </summary>
        public void ConnectToServer()
        {
            if (_connection != null)
                return;

            var factory = new ConnectionFactory();
            _connection = factory.CreateConnection(_host, _port);
            _main = _connection.MainSystem;
        }

        /// <summary>
        /// Получить список доступных БД (сырой текст из GetDBList).
        /// </summary>
        public string GetDatabaseList(out string errorCode)
        {
            ConnectToServer();

            object code = 0;
            object msg = "";
            _main.GetDBList(ref code, ref msg);

            errorCode = code?.ToString();
            return msg?.ToString();
        }

        /// <summary>
        /// Подключиться к конкретной БД.
        /// </summary>
        public void ConnectToDatabase(string dbName, out string errorCode, out string errorMessage)
        {
            ConnectToServer();

            object errCode;
            object errMsg;
            _main.ConnectToDB(dbName, out errCode, out errMsg);

            errorCode = errCode?.ToString();
            errorMessage = errMsg?.ToString();
        }

        /// <summary>
        /// Поиск объектов через IMainSystem.FindObjects.
        /// typeName  – тип объекта (например, "TASK").
        /// projects  – список проектов или "".
        /// condition – условие поиска (как в запросах ЛОЦМАН).
        /// sort      – строка сортировки или "".
        /// context   – контекст или "".
        /// user      – пользователь или "".
        /// options   – дополнительные опции или "".
        /// </summary>
        public object FindObjects(
            string dbName,
            string typeName,
            string projects,
            string condition,
            string sort,
            string context,
            string user,
            string options,
            out string errCode,
            out string errMsg)
        {
            // гарантируем подключение к нужной БД
            ConnectToDatabase(dbName, out var connectCode, out var connectMsg);
            if (!string.IsNullOrEmpty(connectCode) && connectCode != "0")
            {
                errCode = connectCode;
                errMsg = "ConnectToDB: " + connectMsg;
                return null;
            }

            object eCode;
            object eMsg;

            object result = _main.FindObjects(
                typeName,
                projects,
                condition,
                sort,
                context,
                user,
                options,
                out eCode,
                out eMsg);

            errCode = eCode?.ToString();
            errMsg = eMsg?.ToString();
            return result;
        }

        public void Dispose()
        {
            try
            {
                _connection?.Dispose();
            }
            catch
            {
                // игнорируем ошибки закрытия
            }
        }
    }
}
