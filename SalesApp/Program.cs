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

            var price = products[(int)(product_id-1)].Price;
            var total_price = count_user * (price-price*buyer.Discount/100);

            Show.PrintLn($"Вам необходимо заплатить - {total_price}");
                    
        }

        
    }
}