using EFP48.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFP48.Data
{
    // Клас контексту даних, який містить набори сутностей

    /*
     Для створення міграції:
    1) У верхньому меню студії, знаходимо пункт Tools(інструменти)
    2) Знаходимо NuGet Package Manager -> відкриваємо Package Manager Console
    3) Прописуємо Add-Migration <тут ви вказуєте ім'я міграції> 
        (Ім'я може бути будь-яким, головне щоб воно відображало суть змін: додали нову сутність, видалили попали, додали поле, як коміти на гіт)
    4) Після успішного створення міграції(знаком успіху, студія Вас одразу перекидує на файл зі "свіжою" міграцією), її треба застосувати до БД.
    5) В тому ж PM Console прописуємо: Update-Database
    6) У якості успіху ви отримаєте повідомлення, що міграція була успішно застосована
     */
    public class DataContext : DbContext
    {
        // Щоб ваш Ентіті став таблицею необхідно створити DbSet<YourEntity>
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        // Конфігурація EntityFramework:
        // тут налаштовується підключення до БД
        // також є можливість налаштувати логування та безліч інших параметрів
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Кажемо EntityFramework що у якості провайдера БД 
            // використовуємо Microsoft SQL Server
            // налаштовуємо логування(тільки вивод SQL-запитів)
            optionsBuilder.UseSqlServer(
                @"Data Source=(LocalDB)\MSSQLLocalDB;
                Initial Catalog=efp48;
                Integrated Security=True")
                .LogTo(Console.WriteLine, new[] { Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuted })
                .EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Необов'язково прописувати. Якщо EF бачить навігаційні властивості, і відповідні(ForeignKey Id's) він сам створить зовнішні ключі
            // Цей метод потрібен для більшого контролю, якщо ви самі хочете створити зв'язки
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .HasPrincipalKey(c => c.Id);
        }
    }
}
