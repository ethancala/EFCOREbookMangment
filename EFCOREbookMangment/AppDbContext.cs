namespace EFCOREbookMangment;

using Microsoft.EntityFrameworkCore;  // Essential for DbContext


public class AppDbContext : DbContext
{
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer(@"Server=Ethans-PC;Database=LibraryDB;Trusted_Connection=True; TrustServerCertificate=True");
    
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }


}