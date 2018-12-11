using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
    
namespace ProductNCatergories.Models
{
    public class Category
    {
        [Key]
        public int category_id{get;set;}
        
        [Required]
        [Display(Name="Catergory Name")]
        [MinLength(3,ErrorMessage="Catergory name is too short. Must be atleast 3 characters")]
        public string name{get;set;}
        public DateTime created_at{get;set;}=DateTime.Now;
        public DateTime updated_at{get;set;}=DateTime.Now;  

        public List<Association> products{get;set;}

    }
    public class categoryPackageInfo
    {
        public Category category{get;set;}=new Category();
        public List<Category> allCategory{get;set;}
    }
}