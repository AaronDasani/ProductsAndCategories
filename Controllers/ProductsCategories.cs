using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using ProductNCatergories.Models;
using System.Linq;

namespace ProductNCatergories
{
    public class ProdNCaterController:Controller
    {
        public ProductCatergoryContext dbContext;

        private productAssociationInfo productInfo(int product_id)
        {
            var modelInfo=new productAssociationInfo();
            modelInfo.product=dbContext.Products.Include(p=>p.categories)
                                                .ThenInclude(a=>a.category)
                                                .FirstOrDefault(product=>product.product_id==product_id);

            modelInfo.allCategory=dbContext.Categories.Include(c=>c.products).ThenInclude(p=>p.category)
                                                .Where(c=>c.products.All(p=>p.product_id != product_id)) 
                                                .ToList();

            return modelInfo;

        }
        private categoryAssociationInfo categoryInfo(int category_id)
        {
            var modelInfo=new categoryAssociationInfo();
            modelInfo.category=dbContext.Categories.Include(c=>c.products)
                                                    .ThenInclude(p=>p.product)
                                                    .FirstOrDefault(c=>c.category_id==category_id);

            modelInfo.allProducts=dbContext.Products.Include(p=>p.categories)
                                                    .Where(p=>p.categories.All(c=>c.category_id !=category_id))
                                                    .ToList();
            return(modelInfo);
        }
        
        public ProdNCaterController(ProductCatergoryContext context)
        {
            dbContext=context;
        }

        [HttpGet("")]
        [HttpGet("menu")]
        public IActionResult Menu()
        {
            return View("menu");
        }


        [HttpGet("productDashboard")]
        public IActionResult ProductDashboard()
        {
            var productInfoModel=new productPackageInfo();
            productInfoModel.allProducts=dbContext.Products.OrderByDescending(p=>p.created_at).ToList();
            return View("ProductDashboard",productInfoModel);
        }


        [HttpGet("categoryDashboard")]
        public IActionResult CategoryDashboard()
        {
            var categoryInfoModel=new categoryPackageInfo();
            categoryInfoModel.allCategory=dbContext.Categories.OrderByDescending(c=>c.created_at).ToList();
            return View("CategoryDashboard",categoryInfoModel);
        }

        [HttpGet("product/{product_id}")]
        public IActionResult Product(int product_id)
        {

            var modelInfo=productInfo(product_id);
            return View("Product",modelInfo);
        }


        [HttpGet("category/{category_id}")]
        public IActionResult Category(int category_id)
        {
            var modelInfo=categoryInfo(category_id);
            return View("Category",modelInfo);
        }


        // ---------Process Forms ------------

        [HttpPost("createProduct")]
        public IActionResult CreateProduct(productPackageInfo productInfo)
        {
            if (ModelState.IsValid)
            {
                dbContext.Products.Add(productInfo.product);
                dbContext.SaveChanges();
                return RedirectToAction("ProductDashboard");
            }

            var productInfoModel=new productPackageInfo();
            productInfoModel.allProducts=dbContext.Products.ToList();
            return View("ProductDashboard",productInfoModel);
        }


        [HttpPost("createCategory")]
        public IActionResult CreateCategory(categoryPackageInfo categoryInfo)
        {
            if (ModelState.IsValid)
            {
                dbContext.Categories.Add(categoryInfo.category);
                dbContext.SaveChanges();
                return RedirectToAction("CategoryDashboard");
            }

            var categoryInfoModel=new categoryPackageInfo();
            categoryInfoModel.allCategory=dbContext.Categories.ToList();
            return View("CategoryDashboard",categoryInfoModel);
        }

        // --------adding a category to a product-------
        [HttpPost("addCategory")]
        public IActionResult addCategory(productAssociationInfo associationInfo)
        {
            if (ModelState.IsValid)
            {
                dbContext.Associations.Add(associationInfo.association);
                dbContext.SaveChanges();
                // var newCategory=dbContext.Categories.FirstOrDefault(c=>c.category_id==associationInfo.association.category_id);
                // return Json(newCategory);
                return RedirectToAction("Product",new{product_id=associationInfo.association.product_id});
            }

            var modelInfo=productInfo(associationInfo.association.product_id);
            return View("Product",modelInfo);
        }



        // --------adding a product to a category-------
        [HttpPost("addProduct")]
        public IActionResult allProduct(categoryAssociationInfo associationInfo)
        {
            if (ModelState.IsValid)
            {
                dbContext.Associations.Add(associationInfo.association);
                dbContext.SaveChanges();
                return RedirectToAction("Category",new{category_id=associationInfo.association.category_id});
            }

            
            var modelInfo=productInfo(associationInfo.association.category_id);
            return View("Category",modelInfo);
        }
        

    }
    
}