using System;
using MySql.Data.MySqlClient;

namespace SalesApp
{
    class Program
    {
        static void Main()
        {
            const string CONN_STR = "Server=mysql60.hostland.ru;Database=host1323541_sbd06;Uid=host1323541_itstep;Pwd=269f43dc;";
            var data_base = new MySqlConnection(CONN_STR);
            data_base.Open();

            ShowInfo("Выберите продукт, остатки которого хотите посмотреть");
            ShowInfo("1. Phone");
            ShowInfo("2. Car");
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
                    ShowSuccess($"count = {count}");
                } while (res.NextResult());
            }
            else
            {
                ShowError("Вернулась пустая таблица");
            }

            data_base.Close();
        }

        static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] {message}");
            Console.ResetColor();
        }

        static void ShowSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[SUCCESS] {message}");
            Console.ResetColor();
        }

        static void ShowInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"[INFO] {message}");
            Console.ResetColor();
        }
    }
}