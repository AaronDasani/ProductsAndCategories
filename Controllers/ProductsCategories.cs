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
            productInfoModel.allProducts=dbContext.Products.ToList();
            return View("ProductDashboard",productInfoModel);
        }


        [HttpGet("categoryDashboard")]
        public IActionResult CategoryDashboard()
        {
            var categoryInfoModel=new categoryPackageInfo();
            categoryInfoModel.allCategory=dbContext.Categories.ToList();
            return View("CategoryDashboard",categoryInfoModel);
        }

        [HttpGet("product/{product_id}")]
        public IActionResult Product(int product_id)
        {
            var modelInfo=new productAssociationInfo();

            // modelInfo.product=dbContext.Products.Include(p=>p.categories)
            //                                     .ThenInclude(a=>a.category)
            //                                     .FirstOrDefault(product=>product.product_id==product_id);
            var prod=dbContext.Products.Include(p=>p.categories).ToList();
            
            modelInfo.product=dbContext.Products.Where(product=>product.product_id==product_id)
                                                .Include(p=>p.categories)
                                                .ThenInclude(a=>a.category).FirstOrDefault();

            modelInfo.allCategory=dbContext.Categories.ToList();
            return View("Product",modelInfo);
        }


        [HttpGet("category")]
        public IActionResult Category()
        {
            return View("Category");
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
                return RedirectToAction("Product",new{product_id=associationInfo.product.product_id});
            }

            var modelInfo=new productAssociationInfo();
            modelInfo.product=dbContext.Products.Include(p=>p.categories)
                                                .ThenInclude(a=>a.category)
                                                .FirstOrDefault(product=>product.product_id==associationInfo.product.product_id);

            modelInfo.allCategory=dbContext.Categories.ToList();
            return View("Product",modelInfo);
        }

    }
    
}