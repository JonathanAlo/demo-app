using back.Models;
using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.EntityFrameworkCore;



namespace back;

public class ExampleContext : DbContext
{
    private readonly IEncryptionProvider _provider;
    public ExampleContext()
    {
        this._provider = new GenerateEncryptionProvider("mysmallkey1234551298765134567890");
    }
   
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("PostgreSQL"));
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); 
        modelBuilder.UseEncryption(this._provider);
        modelBuilder.HasDefaultSchema("finally");
        modelBuilder.Entity<User>().ToTable("user"); ;
        modelBuilder.Entity<Role>().ToTable("role"); ;
       




        base.OnModelCreating(modelBuilder);
    }
}
