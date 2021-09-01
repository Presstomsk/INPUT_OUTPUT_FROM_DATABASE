using System;
using MySql.Data.MySqlClient;
using CLI;

namespace SalesApp
{
    class Program
    {
        static void Main()
        {
            
            var data_base = new MySqlConnection(CONN_STR);
            data_base.Open();

            Show.Info("Выберите продукт, остатки которого хотите посмотреть");
            Show.Info("1. Phone");
            Show.Info("2. Car");
            var select = Console.ReadLine();

            var sql = $"SELECT count FROM tab_products_stock JOIN tab_products ON tab_products_stock.product_id = tab_products.id WHERE product_id = {select}";
            var command = new MySqlCommand
            {
                CommandText = sql,
                Connection = data_base
            };
            var res = command.ExecuteReader();

            if (res.HasRows)
            {
                do
                {
                    res.Read();
                    var count = res.GetInt32("count");
                    Show.Success($"count = {count}");
                } while (res.NextResult());
            }
            else
            {
                Show.Error("Вернулась пустая таблица");
            }

            data_base.Close();
        }

        
    }
}