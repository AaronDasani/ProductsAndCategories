using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace ProductNCatergories.Models
{
    public class Association
    {
        [Key]
        public int association_id{get;set;}

        [Required]
        [Display(Name="Add Category:")]
        public int category_id{get;set;}

        [Required]
        public int product_id{get;set;}

        public Product product{get;set;}
        public Category category{get;set;}
    }
    public class productAssociationInfo
    { 
        public Association association{get;set;}=new Association();
        public Product product{get;set;}
        public List<Category> allCategory{get;set;}
    }
    public class categoryAssociationInfo
    { 
        public Association association{get;set;}=new Association();
        public Category category{get;set;}
        public List<Product> allProducts{get;set;}
    }
}