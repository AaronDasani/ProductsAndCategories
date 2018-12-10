using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace ProductNCatergories.Models
{
    public class ProductCatergoryContext:DbContext
    {
        public ProductCatergoryContext(DbContextOptions<ProductCatergoryContext> options):base(options){}
        public DbSet<Product> Products{get;set;}
        public DbSet<Category> Categories{get;set;}
        public DbSet<Association> Associations{get;set;}

    }
}