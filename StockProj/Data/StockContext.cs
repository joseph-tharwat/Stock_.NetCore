using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace StockTrading.Models;

public partial class StockContext : DbContext
{
    //public StockContext()
    //{
    //}

    public StockContext(DbContextOptions<StockContext> options)
        : base(options)
    {
        //var databaseCreateor = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
        //if (databaseCreateor != null)
        //{
        //    if (databaseCreateor.CanConnect() == false)
        //    {
        //        databaseCreateor.Create();
        //    }
        //    if (databaseCreateor.HasTables() == false)
        //    {
        //        databaseCreateor.CreateTables();
        //    }
        //}

    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       

        //OnModelCreatingPartial(modelBuilder);
    }

    //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
