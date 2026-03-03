using EFP48.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFP48
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            DataContext _dataContext = new();

            // Include - дозволяє включити пов'зані сутності. В нашому прикладі, разом з категоріями, будуть підвантажуватися
            // продукти відповідно конкретній категорії.
            var query = _dataContext.Categories.Include(c => c.Products.OrderBy(p=>p.Name));
            var categories = query.ToList();
            foreach (var category in categories) {
                Console.WriteLine($"CategoryID: {category.Id}, CategoryName: ${category.Name}\n");
                if (category.Products is not null)
                {
                    Console.WriteLine("------------------------------------");
                    foreach (var product in category.Products)
                    {
                        Console.WriteLine(product);
                    }
                    Console.WriteLine("------------------------------------");
                }
            }


        }
    }
}
