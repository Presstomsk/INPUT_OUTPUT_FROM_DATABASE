using System;
using MySql.Data.MySqlClient;
using CLI;
using SalesLib;


namespace SalesApp
{
    class Program
    {
        static void Main()
        {
            Show.Menu();
            var select = Console.ReadLine();
            switch(select)
            {
                case "1":  //1. Оформление заказа
                    InitOrder();
                    break;
                case "2":  //2. Экспорт списка продуктов
                    ExportProducts();
                    break;
                case "3":  //3. Импорт списка продуктов
                    ImportProducts();
                    break;
                case "4": //4. Экспорт списка заказов
                    ExportOrders(); 
                    break;
                case "5": //5. Экспорт списка людей с дисконтом
                    ExportPeoplesWithDiscounts();
                    break;
                case "6"://6. Импорт списка людей c дисконтом
                    ImportPeoples();
                    break;

            }

        }

        static void InitOrder()
        {
            var db = new DataBase();
            var products = db.GetProducts();
            var buyers = db.GetBuyers();
            
            Buyer buyer;


            foreach (var item in buyers)
            {
                Show.PrintLn($"{item.Id}: {item.Name}");
            }

            Show.PrintLn("Введите номер покупателя:");
            var buyer_id = uint.Parse(Console.ReadLine());

            if (buyer_id == 0)
            {
                buyer = new Buyer();
            }
            else
            {
                buyer = buyers[(int)(buyer_id - 1)];
            }

            foreach (var product in products)
            {
                Show.PrintLn($"{product.Id}:{product.Name},{product.Price}");
            }
            Show.Print("Введите номер продукта:");
            var product_id = uint.Parse(Console.ReadLine());
            Show.Print("Введите количество:");
            var count_user = uint.Parse(Console.ReadLine());

            var count_stock = db.GetProductCount(product_id);

            if (count_user > count_stock)
            {
                Show.Error("На складе нет необходимого количества товара");
                return;
            }

            var price = products[(int)(product_id - 1)].Price;
            var total_price = count_user * (price - price * buyer.Discount / 100);

            Show.PrintLn($"Вам необходимо заплатить - {total_price}");
            string symbol;
            do
            {
                Show.Print("Вы хотите преобрести товар? (y/n):");
                symbol = Console.ReadLine();
                if (symbol == "n") return;

            } while ((symbol != "y") && (symbol != "n"));

            // Добавить в программу  возможность добавления данных о покупке после ввода всей необходимой информации.
            // Т.е.нужно от пользователя получить данные для всех полей таблицы tab_orders и написать запрос на добавление в неё строки. 
            // Id продавца задан по умолчанию.

            Order order = new Order
            {
                Id = 0,
                Buyer_id = buyer_id,
                Seller_id = 1,
                Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Product_id = product_id,
                Amount = count_user,
                Total_price = total_price
            };

            db.AddNewOrder(order);
        }
        static void ExportProducts()
        {
            var db = new DataBase();
            db.ExportProductsToCSV("products.csv");
        }

        static void ExportOrders()
        {
            var db = new DataBase();
            db.ExportOrdersToCSV("orders.csv");
        }

        static void ExportPeoplesWithDiscounts()
        {
            var db = new DataBase();
            db.ExportPeoplesWithDiscountsToCSV("peoples.csv");
        }

        static void ImportProducts()
        {
            var db = new DataBase();
            db.ImportProductsFromCSV("products.csv");
        }

        static void ImportPeoples()
        {
            var db = new DataBase();
            db.ImportPeoplesWithDiscountsFromCSV("peoples.csv");
        }
    }
}