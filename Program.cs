using EFP48.Data;
using EFP48.Data.Entity;

namespace EFP48
{
    public class Program
    {
        public static int ShowProductCrudMenu()
        {
            Console.WriteLine(@"
1. Get all products
2. Get product by id
3. Create new product
4. Update product
5. Delete product
6. Exit
");
            int result;
            while (!int.TryParse(Console.ReadLine(), out result)) {
                Console.WriteLine("Enter number 1-6:");
              }
            return result;
        }


        public static void printEntity<T>(List<T> values, string entityName)
        {
            if (values.Count != 0)
            {
                Console.WriteLine($"\n-------------- ${entityName} --------------");
                foreach (var item in values)
                {
                    Console.WriteLine(item);
                }
            }
            else Console.WriteLine($"No: {entityName}");
        }

        public static List<Product> getAllProducts(DataContext? dataContext, bool showDeleted=false)
        {
            if(dataContext== null)
            {
                Console.WriteLine("Error: Data context not provided");
                throw new Exception("Data Context not provided");
            }
            var products = dataContext.Products.AsQueryable();

            if (!showDeleted) products = products.Where(p => p.DeletedAt == null);

            return products.ToList();
        }

        public static Product? getProductById(DataContext? dataContext, Guid productId)
        {
            if (dataContext == null)
            {
                Console.WriteLine("Error: Data context not provided");
                throw new Exception("Data Context not provided");
            }
            // search by primary key
            //var product = dataContext.Products.Find(productId);
            // First(throws InvalidOperationExecption якщо не знаходить об'єкт), FirstOrDefault(не кидає виключень, повертає null)
            //return dataContext.Products.First(p => p.Id == productId);

            var product = dataContext.Products.FirstOrDefault(p => p.Id == productId);
            return product;
        }

        public static bool deleteProduct(DataContext? dataContext, Guid productId)
        {
            var product = getProductById(dataContext, productId);
            if (product is not null)
            {
                product.DeletedAt = DateTime.Now;
                dataContext!.SaveChanges();
                return true;
            }
            Console.WriteLine("Product nor found");
            return false;
        }

        public static void Main(string[] args)
        {

            // CRUD - Create, Read, Update, Delete

            DataContext dataContext = new();

            bool isExit = false;
            Guid guidParseResult;
            string userRequest;
            while (!isExit)
            {
                Console.Clear();
                int menuItem = ShowProductCrudMenu();
                switch (menuItem)
                {
                    case 1: // READ
                        Console.Clear();
                        var products = getAllProducts(dataContext);
                        printEntity<Product>(products, "Products");
                        Console.ReadKey();
                        break;
                    case 2:
                        Console.Clear();
                        Console.Write("Enter productId: ");
                        userRequest = Console.ReadLine();
                        if (!Guid.TryParse(userRequest, out guidParseResult)){
                            Console.Clear();
                            Console.WriteLine("Guid format is incorrect!");
                            Console.ReadKey();
                            break;
                        }
                        var product = getProductById(dataContext, guidParseResult);
                        if(product is null)
                        {
                            Console.WriteLine("Product not found!");
                            Console.ReadKey();
                        }
                        else Console.WriteLine(product);
                            Console.ReadKey();
                        break;
                    case 5:
                        Console.Clear();
                        Console.Write("Enter productId: ");
                        userRequest = Console.ReadLine();
                        if (!Guid.TryParse(userRequest, out guidParseResult))
                        {
                            Console.Clear();
                            Console.WriteLine("Guid format is incorrect!");
                            Console.ReadKey();
                            break;
                        }
                        if(deleteProduct(dataContext, guidParseResult))
                        {
                            Console.WriteLine("Product successfuly deleted!");
                        }
                        else Console.WriteLine("Product not delete");
                            Console.ReadKey();
                        break;

                }

            }
            #region Lesson1
            /*
            DataContext dataContext = new();
            ShowProductCrudMenu();
            // Створюємо єкземпляр ДатаКонтексту
            // За допомогою дата-контексту у нас є можливість надсилати запити до БД


            // Створення нового продутку
            var new_product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Sony PlayStation 5 Pro",
                Price = 1500,
                CreatedAt = DateTime.Now,
            };


            // Додаємо продукт у DbSet
            dataContext.Products.Add(new_product);
            // Оновлюємо дані таблиці, зберігаємо зміни
            dataContext.SaveChanges();


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

            */
            #endregion

        }
    }
}
