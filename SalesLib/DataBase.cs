using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesLib
{
    class DataBase
    {
        private const string CONN_STR = "Server=mysql60.hostland.ru;Database=host1323541_sbd06;Uid=host1323541_itstep;Pwd=269f43dc;";
        private MySqlConnection db;
        private MySqlCommand command;

        public DataBase()
        {
            db = new MySqlConnection(CONN_STR);
            command = new MySqlCommand { Connection = db };
        }           
        

        public void Open() => db.Open();

        public void Close() => db.Close();

        public List<Product> GetProducts()
        {
            var sql = "SELECT id, name, price FROM tab_products;";
            command.CommandText = sql;
            var res=command.ExecuteReader();

        }
    }
}
