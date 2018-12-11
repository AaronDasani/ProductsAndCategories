using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductNCatergories.Models
{
    public class Product
    {
        [Key]
        public int product_id{get;set;}

        [Required]
        [Display(Name="Product Name")]
        [MinLength(3,ErrorMessage="Product name is too short. Must be atleast 3 characters")]
        public string name{get;set;}

        [Required]
        [Display(Name="Product Description")]
        [MinLength(3,ErrorMessage="Product Description is too short. Must be atleast 3 characters")]
        [MaxLength(200,ErrorMessage="Description is too long. Must be less that 200 chracters")]
        public string description{get;set;}

        [Required]
        [Display(Name="Product Price")]
        [Range(1,Int32.MaxValue,ErrorMessage="Must be a valid price")]
        public double price{get;set;}
        public DateTime created_at{get;set;}=DateTime.Now;
        public DateTime updated_at{get;set;}=DateTime.Now;

        
        public List<Association> categories{get;set;}
    }
    public class productPackageInfo
    {
        public Product product{get;set;}=new Product();
        public List<Product> allProducts{get;set;}
    }
}