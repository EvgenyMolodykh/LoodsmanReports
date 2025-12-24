using System;
using System.Collections.Generic;
using Ascon.Plm.ServerApi;

namespace LoodsmanBackend
{
    /// <summary>
    /// Упрощённая обёртка над Ascon.Plm.ServerApi.
    /// </summary>
    public class LoodsmanApi : IDisposable
    {
        private readonly IConnection _connection;

        public LoodsmanApi(string host, int port)
        {
            _connection = new ConnectionFactory().CreateConnection(host, port);
        }

        /// <summary>Подключиться к базе Лоцмана.</summary>
        public void ConnectToDb(string dbName)
        {
            _connection.MainSystem.ConnectToDB(
                dbName,
                out object errCode,
                out object errMsg);

            CheckError(errCode, errMsg);
        }

        /// <summary>Получить список всех зарегистрированных баз (GetDBList).</summary>
        public IReadOnlyList<string> GetDbList()
        {
            // Параметры по ссылке (ref / out) – как требует сигнатура GetDBList.
            object retCode = 0;
            object errMsg = null;

            // ВАЖНО: если в твоей сборке сигнатура с out,
            // замени "ref" на "out" в следующей строке.
            var variant = _connection.MainSystem.GetDBList(ref retCode, ref errMsg);

            CheckError(retCode, errMsg);

            // Документация: возвращает строку "DB,DB2,...,DBN"
            var str = variant?.ToString() ?? string.Empty;
            var items = str.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < items.Length; i++)
                items[i] = items[i].Trim();

            return items;
        }

        private static void CheckError(object errCode, object errMsg)
        {
            if (Convert.ToInt32(errCode) != 0)
                throw new Exception($"Error {errCode}: {errMsg}");
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
