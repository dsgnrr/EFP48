using EFP48.Data;
using EFP48.Data.Entity;

namespace EFP48
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Створюємо єкземпляр ДатаКонтексту
            // За допомогою дата-контексту у нас є можливість надсилати запити до БД
            DataContext dataContext = new();

            // Створення нового продутку
            //var product = new Product
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "Samsung S26 Ultra",
            //    Price = 1500,
            //    CreatedAt = DateTime.Now,
            //};

            
            // Додаємо продукт у DbSet
            //dataContext.Products.Add(product);
            // Оновлюємо дані таблиці, зберігаємо зміни
            //dataContext.SaveChanges();


            // Отримання всіх продуктів
            List<Product> products = dataContext.Products.ToList();

            Console.WriteLine("\nProducts: ");
            foreach (var p in products)
            {
                Console.WriteLine(p);
            }

            // Отримання продуктів за умовою
            products = dataContext.Products
               .Where(p => p.Price < 500)
               .ToList();

            if (products.Count > 0)
            {
                Console.WriteLine("\nProducts.Price<500: ");
                foreach (var p in products)
                {
                    Console.WriteLine(p);
                }
            }

            // Отримання продукту за ідентифікатором
            Guid searchId = Guid.Parse("f2844eab-c8c3-413c-8a55-c9fb1c95b690");

            var product = dataContext.Products.Find(searchId);
            if (product != null)
            {
                Console.WriteLine("Product: \n{0}", product);
                
                // Оновлюємо продукт
                product.Price = 340;
                // Зберігаємо зміни
                dataContext.SaveChanges();

                // Видаляємо продукт
                dataContext.Products.Remove(product);
                // Зберігаємо зміни
                dataContext.SaveChanges();
            }


        }
    }
}
