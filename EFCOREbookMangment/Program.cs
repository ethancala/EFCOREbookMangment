using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

/*sources:
https://learn.microsoft.com/en-us/ef/core/performance/advanced-performance-topics?tabs=with-di%2Cexpression-api-with-constant
https://learn.microsoft.com/en-us/ef/core/modeling/
https://www.perplexity.ai/
https://stackoverflow.com/questions/70679395/entity-framework-core-seed-data-only-if-it-does-not-exist
https://www.reddit.com/r/dotnet/comments/1hrspme/should_i_use_ef_core_async_methods/
*/
namespace EFCOREbookMangment
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var context = new AppDbContext();
            context.Database.EnsureCreated();

            // THIS IS WHERE I SEEDED THE DATA---
            /*
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
            */
            // --- END OF DATA SEEDING---

            
            //As for the queries, I used Stack overflow and Perplexity AI to help. Let me know if you want the links and Conversation
            
            //also I dont really log any data here, but u can check the database for it being updated
            //  get all data
            var allAuthors = await context.Authors.ToListAsync();
            var allBooks = await context.Books.ToListAsync();

            // get number of books, 3
            var numBooks = 3;
            var someBooks = await context.Books.Take(numBooks).ToListAsync();

            // first book with condition
            var firstSciFi = await context.Books
                .FirstOrDefaultAsync(b => b.PublicationYear > 2000); //published from 2000+
            Console.WriteLine(firstSciFi?.Title ?? "Not found"); //else not found

            //  sorting
            var sortedAuthors = await context.Authors
                .OrderBy(a => a.LastName) //sort by  last name
                .ThenByDescending(a => a.FirstName) //and decsencding order
                .ToListAsync();

            // column subset
            var bookTitles = await context.Books
                .Where(b => b.PublicationYear > 2000)
                .Select(b => new { b.Title, b.PublicationYear }) //get only the title and publication year for books published after 2000
                .ToListAsync();

            //  multitable query
            var authorBooks = await context.Authors // combine data from authors and books tables, pairing each book with its authors last name
                .Join(context.Books,
                    a => a.Id,
                    b => b.AuthorId,
                    (a, b) => new { a.LastName, b.Title })
                .OrderBy(x => x.LastName)
                .ToListAsync();

            // aggregation
            var avgYear = await context.Books
                .AverageAsync(b => b.PublicationYear); //calculate the average publication year of all books.

            // Books per author
            var booksPerAuthor = await context.Authors
                .Select(a => new  //for each author, get their last name and how many books they have written
                {
                    Author = a.LastName,
                    BookCount = a.Books.Count
                }).ToListAsync();
        }
        
        //according to perplexity: // Using asynchronous functions (async/await) allows our program to perform database and I/O operations
// without blocking the main thread. This keeps the application responsive (especially important for UI apps)
// and allows the program to handle more tasks concurrently, improving scalability and performance.
// For example, while waiting for a database query to complete, the application can continue processing other requests.
// This is especially beneficial for web and server applications.
    }
}
