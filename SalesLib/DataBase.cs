using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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

        public List<Order> GetOrders()
        {
            Open();
            var list = new List<Order>();
            var sql = "SELECT id,buyer_id,seller_id, date, product_id, amount, total_price FROM tab_orders;";
            command.CommandText = sql;
            var res = command.ExecuteReader();
            if (!res.HasRows) return null;
            while (res.Read())
            {
                var id = res.GetUInt32("id");
                var buyer_id = res.GetUInt32("buyer_id");
                var seller_id = res.GetUInt32("seller_id");
                var date = res.GetString("date");
                var product_id = res.GetUInt32("product_id");
                var amount = res.GetUInt32("amount");
                var total_price = res.GetUInt32("total_price");
                list.Add(new Order { Id = id, Buyer_id = buyer_id, Seller_id = seller_id, Date=date, Product_id=product_id, Amount=amount, Total_price=total_price }) ;

            }
            Close();
            return list;

        }

        public List<People> GetPeoplesWithDiscounts()
        {
            Open();
            var list = new List<People>();
            var sql = @"SELECT tab_people.id,first_name,last_name,phone,type,discount FROM tab_people
                          JOIN tab_buyers on tab_people.id = tab_buyers.people_id
                          JOIN tab_discounts on tab_buyers.discount_id = tab_discounts.id;";
            command.CommandText = sql;
            var res = command.ExecuteReader();
            if (!res.HasRows) return null;
            while (res.Read())
            {
                var id = res.GetUInt32("id");
                var first_name = res.GetString("first_name");
                var last_name = res.GetString("last_name");
                var phone = res.GetString("phone");
                var type_discount = res.GetString("type");
                var discount = res.GetUInt32("discount");                
                list.Add(new People { Id = id, First_name=first_name, Last_name=last_name, Phone=phone, Type_discount=type_discount, Discount=discount});

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

        public void ExportOrdersToCSV(string path)
        {
            var orders = GetOrders();
            using var file = new StreamWriter(path, false);
            foreach (var order in orders)
            {
                file.WriteLine($"{order.Id}|{order.Buyer_id}|{order.Seller_id}|{order.Date}|{order.Product_id}|{order.Amount}|{order.Total_price}");
            }
        }

        public void ExportProductsToCSV(string path)
        {
            var products = GetProducts();
            using var file = new StreamWriter(path, false);
            foreach (var product in products)
            {
                file.WriteLine($"{product.Id}|{product.Name}|{product.Price}");
            }
        }

        public void ExportPeoplesWithDiscountsToCSV(string path)
        {
            var peoples = GetPeoplesWithDiscounts();
            using var file = new StreamWriter(path, false);
            foreach (var person in peoples)
            {
                file.WriteLine($"{person.Id}|{person.First_name}|{person.Last_name}|{person.Phone}|{person.Type_discount}|{person.Discount}");
            }
        }

        public void ImportProductsFromCSV(string path)
        {
            var products_csv = new List<Product>();
            using var file = new StreamReader(path);
            var line = string.Empty;
            while ((line=file.ReadLine())!=null)
            {
                var temp = line.Split('|');
                var product = new Product();
                product.Id = uint.Parse(temp[0]);
                product.Name = temp[1];
                product.Price = uint.Parse(temp[2]);
                products_csv.Add(product);
            }
     
            Open();
            foreach (var product in products_csv)
            {
                var sql = $"INSERT INTO tab_products (name, price) VALUES ('{product.Name}',{product.Price});";
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
            Close();
        }

        public void ImportPeoplesWithDiscountsFromCSV(string path)
        {
            var peoples_csv = new List<People>();
            using var file = new StreamReader(path);
            var line = string.Empty;
            while ((line = file.ReadLine()) != null)
            {
                var temp = line.Split('|');
                var people = new People();
                people.Id = uint.Parse(temp[0]);
                people.First_name = temp[1];
                people.Last_name = temp[2];
                people.Phone = temp[3];
                people.Type_discount = temp[4];
                people.Discount = uint.Parse(temp[5]);                
                peoples_csv.Add(people);
            }

            Open();
            foreach (var people in peoples_csv)
            {
                int flag_1=0, flag_2=0;
                var sql = $"INSERT INTO tab_people (first_name, last_name, phone) VALUES ('{people.First_name}','{people.Last_name}','{people.Phone}');";                             
                command.CommandText = sql;
                flag_1 = command.ExecuteNonQuery();
                if (flag_1 == 1)
                {
                    sql = $"INSERT INTO tab_discounts (type, discount) VALUES ('{people.Type_discount}',{people.Discount});";
                    command.CommandText = sql;
                    flag_2 = command.ExecuteNonQuery();
                }
                if (flag_2 == 1)
                {
                    sql = $"SELECT max(id) as max_people FROM tab_people;";
                    command.CommandText = sql;
                    var res = command.ExecuteReader();
                    res.Read();
                    var people_id = res.GetUInt32("max_people");
                    res.Close();
                    sql = $"SELECT max(id) as max_discounts FROM tab_discounts;";
                    command.CommandText = sql;
                    res = command.ExecuteReader();
                    res.Read();
                    var discount_id = res.GetUInt32("max_discounts");
                    res.Close();
                    sql = $"INSERT INTO tab_buyers (people_id, discount_id) VALUES ({people_id},{discount_id});";
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
                
            }
            Close();
        }//Триггеры не поддерживаются в БД


    }
}
