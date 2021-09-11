using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesLib
{
    public class DataBase
    {        
        private MySqlConnection db;
        private MySqlCommand command;

        public DataBase()
        {
            var connectionString = ConnectionString.Init(@"db_connection.ini");
            db = new MySqlConnection(connectionString);
            command = new MySqlCommand { Connection = db };
        }           
        

        public void Open() => db.Open();

        public void Close() => db.Close();

        public List<Product>GetProducts()
        {
            Open();
            var list = new List<Product>();
            var sql = "SELECT id, name, price FROM tab_products;";
            command.CommandText = sql;
            var res=command.ExecuteReader();
            if (!res.HasRows) return null;
            while (res.Read()) 
            {                    
                    var id = res.GetUInt32("id");
                    var name = res.GetString("name");
                    var price = res.GetUInt32("price");
                    list.Add(new Product { Id=id, Name=name, Price=price });

            }
            Close();
            return list;
            
        }

        public uint GetProductCount(uint id)
        {
            Open();
            var sql = @$"SELECT count 
                        FROM tab_products_stock 
                        JOIN tab_products
                         ON tab_products_stock.product_id=tab_products.id
                         WHERE product_id={id};";
            command.CommandText = sql;
            var res = command.ExecuteReader();
            if (!res.HasRows) return 0;

            res.Read();            
            var count = res.GetUInt32("count");            
                            
            Close();

            return count;

        }

        public List<Buyer> GetBuyers()
        {
            Open();
            var list = new List<Buyer>();            
            var sql = @"SELECT tab_buyers.id, first_name, last_name, discount
                        FROM tab_buyers
                        JOIN tab_people
                        ON tab_buyers.people_id=tab_people.id
                        JOIN tab_discounts
                        ON tab_buyers.discount_id=tab_discounts.id;";
            command.CommandText = sql;
            var res = command.ExecuteReader();
            if (!res.HasRows) return null;

            while (res.Read())
            {
                var id = res.GetUInt32("id");
                var name = $"{res.GetString("first_name")} {res.GetString("last_name")}";
                var discount = res.GetUInt32("discount");

                list.Add(new Buyer { Id = id, Name = name, Discount = discount });
            }

            Close();
            return list;
        }

        public void AddNewOrder(Order ord)
        {
            Open();
            var sql = @$"INSERT INTO tab_orders 
                          (buyer_id, seller_id, date, product_id, amount, total_price)
                        VALUES ({ord.Buyer_id}, {ord.Seller_id}, '{ord.Date}', {ord.Product_id}, {ord.Amount}, {ord.Total_price});";
            command.CommandText = sql;
            command.ExecuteNonQuery();
            Close();
        }

       
    }
}
