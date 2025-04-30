using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EFCOREbookMangment
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var context = new AppDbContext();
            context.Database.EnsureCreated();

            if (!await context.Authors.AnyAsync())
            {
                var authors = new List<Author>
                {
                    new() { FirstName = "J.K.", LastName = "Rowling", Books = new()
                    {
                        new() { Title = "Harry Potter 1", PublicationYear = 1997 },
                        new() { Title = "Harry Potter 2", PublicationYear = 1998 }
                    }},
                    new() { FirstName = "George", LastName = "Orwell", Books = new()
                    {
                        new() { Title = "1984", PublicationYear = 1949 }
                    }}
                };

                await context.Authors.AddRangeAsync(authors);
                await context.SaveChangesAsync();
            }
        }
    }
}