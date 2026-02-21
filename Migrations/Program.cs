using EFP48.Data;
using EFP48.Data.Entity;

namespace EFP48.Migrations
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DataContext dataContext = new();

            //var product = new Product
            //{
            //    Id = Guid.NewGuid(),
            //    Name = "Samsung S26 Ultra",
            //    Price = 1500,
            //    CreatedAt = DateTime.Now,
            //};

            //dataContext.Products.Add(product);
            //dataContext.SaveChanges();

            var products = dataContext.Products.ToList();

            Console.WriteLine("\nProducts: ");
            foreach (var p in products) {
                Console.WriteLine(p);
            }

            products = dataContext.Products
                .Where(p=>p.Price < 500)
                .ToList();

            Console.WriteLine("\nProducts.Price<500: ");
            foreach (var p in products)
            {
                Console.WriteLine(p);
            }

            Guid searchId = Guid.Parse("f2844eab-c8c3-413c-8a55-c9fb1c95b690");

            var product = dataContext.Products.Find(searchId);
            if (product != null)
            {
                Console.WriteLine("Product: \n{0}", product);
                product.Price = 340;
                dataContext.SaveChanges();

                dataContext.Products.Remove(product);
                dataContext.SaveChanges();
            }


        }
    }
}
